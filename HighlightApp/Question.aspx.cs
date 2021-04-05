using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HighlightApp
{
    public partial class Question : System.Web.UI.Page
    {
        string studyid;
        string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        string question_name;
        string instruction_text;
        string colorLike;
        string colorDislike;
        int questionid;
        protected void Page_Load(object sender, EventArgs e)
        {
            var studyname = Session["StudyName"].ToString();
            studyid = Request.QueryString["Study"];
            lblStudy.Text = studyname;
           
            //POSTBACK
            if (IsPostBack)
            {
                //RUN JAVASCRIPT
                ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = true;", true);
                question_name = txtQuestionName.Text;
                colorLike = txtLike.Text;
                colorDislike = txtDislike.Text;
                //DECODE QUESTION TEXT
                qtext.InnerHtml = Server.HtmlDecode(qtext.InnerHtml);
                instruction_header.InnerHtml = Server.HtmlDecode(instruction_header.InnerHtml);

            }
            try
            {
                questionid = Int32.Parse(Request.QueryString["Edit"]);

            }
            catch
            {
            }
            if (!IsPostBack)
            {
                try
                {

                    QuestionEdit(questionid);
                }
                catch
                {
                }

                if (txtLike.Text == "")
                {
                    chkLike.Checked = false;
                }
                if (txtDislike.Text == "")
                {
                    chkDislike.Checked = false;
                }
            }
        }


        protected void nextEventMethod(object sender, EventArgs e)
        {
            lblErrorMessage2.Text = "";
            lblErrorMessage.Text = "";
            Label1.Text = "";
            //DECODE TEXT EDITORS
            var question_text = Server.HtmlDecode(Request.Form["qtext"]);
            instruction_text = Server.HtmlDecode(Request.Form["instruction_header"]);

            //validate blank question name
            if (question_name.Trim() != "")
            {
                //validate duplication of question name
                if (ValidateQuestion(question_name))
                {
                    Session["question_text"] = Request.Form["qtext"];
                    lblErrorMessage.Text = "Question name was already used.";
                }
                else
                {
                    //validate blank instruction text
                    if (instruction_text != "")
                    {
                        //validate same color pallette
                        if (ValidateColor(colorLike, colorDislike))
                        {
                            //validate question text
                            if (question_text.Trim() != "")
                            {
                                //by phrase method
                                if (radMethod.SelectedValue == "2")
                                {
                                    Session["Question_Text"] = question_text;
                                    Session["Instruction_Text"] = instruction_text;
                                    Session["Color_Like"] = colorLike;
                                    Session["Color_Dislike"] = colorDislike;
                                    Session["Question_Id"] = questionid;
                                    Response.Redirect("Highlight.aspx?Study=" + studyid + "&Question=" + question_name);
                                }
                                else
                                {
                                    //per word method
                                    SplitText(question_text);
                                };
                            }
                            else
                            {
                                Label1.Text = "Question text is required.";

                            }
                        }
                    }
                    else
                    {
                        lblErrorMessage2.Text = "Instruction Text is required.";
                    }
                }


            }
            else
            {
                lblErrorMessage.Text = "Question name is required.";

            }
        }
        protected bool ValidateQuestion(string question)
        {

            bool exists = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // check if question name has duplicate
                using (SqlCommand cmd = new SqlCommand("select count(*) from HighlightQuestions where Question_Name = '" + question + "' AND Study_ID = '" + studyid + "' AND Question_Id != '" + questionid + "'", con))
                {
                    exists = (int)cmd.ExecuteScalar() > 0;
                }
            }
            return exists;
        }

        protected bool ValidateRedirect(string url)
        {
            Uri validateUrl;

            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out validateUrl);
        }

        public void SplitText(string text)
        {
            List<String> finalList = new List<string>();
            bool insideHtml = false;
            StringBuilder sb = new StringBuilder();
            //add space between html tags
            text = text.Replace("><", "> <");
            text = text.Replace("<br />", "<br/>");
            string[] textsplit = text.Split(' ');

            //split html tags
            foreach (string t in textsplit)
            {
                //unselect br
                if (t.Contains("<") )
                {
                    sb.Append(" " + t);
                    insideHtml = true;
                    if (t.Contains(">"))
                    {
                        finalList.Add(sb.ToString());
                        sb.Clear();
                        insideHtml = false;
                    }
                }
                else if (t.Contains(">"))
                {
                    sb.Append(" " + t);
                    finalList.Add(sb.ToString());
                    sb.Clear();
                    insideHtml = false;
                }
                else
                {
                    if (insideHtml)
                    {
                        sb.Append(" " + t);
                    }
                    else
                    {
                        finalList.Add(t);
                    }
                }
            }
            
            
            string pattern = @"(?=[<])|(?<=[>])";
            var content = "";
            int finalctr = 0;
            for (var i = 0; i < finalList.Count; i++)
            {
                //validate splitted if contains html tag
                if (finalList[i].Contains("<") && finalList[i].Contains(">"))
                {
                    string splitResult = "";
                    string[] splitHtml = Regex.Split(finalList[i], pattern);

                    int ctr = 0;
                    //string toReplace = "";
                    for (int index = 0; index < splitHtml.Length; index++)
                    {

                        if (!(splitHtml[index] == "." || splitHtml[index] == ","))
                        {
                            //validate text
                            if ((!(splitHtml[index].Contains('>') || splitHtml[index].Contains('<'))) && (splitHtml[index].Trim() != ""))
                            {
                                //add span to text
                                ctr += 1;
                                splitHtml[index] = "<span class=\"select\" id=\"" + (finalctr) + "\">" + splitHtml[index] + "</span>";
                                finalctr += 1;

                            }
                            //validate image
                            if (splitHtml[index].Contains("<img"))
                            {
                                //add span to image with
                                ctr += 1;
                                splitHtml[index] = " <span class=\"select image\" id=\"" + (finalctr) + "\">" + splitHtml[index] + "</span> ";
                                finalctr += 1;
                            }
                        }
                        splitResult += splitHtml[index];

                    }

                    finalList[i] = splitResult;
                }
                else
                {
                    
                    //if not html add span
                    finalList[i] = " <span class=\"select\" id=\"" + (finalctr) + "\">" + finalList[i] + "</span> ";
                    finalctr += 1;
                }
                content += finalList[i];
                content = content.Replace("<br/>", "<br />");
            }

            
            if (content != "")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        if (questionid == 0)
                        {
                            //new question
                            cmd.CommandText = "INSERT INTO HighlightQuestions (Question_Name,Question_Text,Instruction_Text,Study_Id,Question_Redirect, Fields_Count, Color_Like, Color_Dislike, Raw_Text, Method) VALUES ('" + question_name + "','" + content + "','" + instruction_text + "','" + studyid + "','','" + finalctr + "','" + colorLike + "','" + colorDislike + "', '" + text + "' , '" + radMethod.SelectedValue + "')";

                        }
                        else
                        {
                            //edit question
                            cmd.CommandText = "UPDATE HQ set HQ.Question_Name = '" + question_name + "', HQ.Question_Text = '" + content + "', HQ.Fields_Count =  '" + finalctr + "', HQ.Color_Like = '" + colorLike + "', HQ.Color_Dislike = '" + colorDislike + "', HQ.Raw_Text = '" + text + "', HQ.Instruction_Text = '" + instruction_text + "', HQ.Method = '" +  radMethod.SelectedValue + "' from highlightquestions HQ where HQ.Question_Id = '" + questionid + "'";
                        }
                        cmd.Connection = con;
                        con.Open();
                        int check = cmd.ExecuteNonQuery();
                        con.Close();
                        if (check != 0)
                        {
                            Session["StoredId"] = studyid;
                            Session["StoredQuestionName"] = question_name;
                            Response.Redirect("View.aspx");
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        protected bool ValidateColor(string like, string dislike)
        {
            //if no function is checked
            if (chkLike.Checked == false && chkDislike.Checked == false)
            {
                colorError.Text = "Enable atleast one highlight function.";
                return false;

            }
            else
            {
                //clear hidden function text 
                if (chkDislike.Checked == false)
                {
                    colorDislike = "";
                }
                if (chkLike.Checked == false)
                {
                    colorLike = "";
                }
            }
            //if checked but no color
            if (chkLike.Checked == true && like == "")
            {
                colorError.Text = "Please select a color for like.";
                return false;
            }
            if (chkDislike.Checked == true && dislike == "")
            {
                colorError.Text = "Please select a color for dislike.";
                return false;
            }

            if (like != dislike)
            {
                colorError.Text = "";
                return true;
            }
            else
            {
                colorError.Text = "Highlight color should not be the same.";
                return false;
            }

        }

        public void QuestionEdit(int id)
        {
            //load existing values on edit
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                
                using (SqlCommand cmd = new SqlCommand("select * from HighlightQuestions where Question_Id = '" + id + "' and Study_Id ='" + studyid + "'", con))
                {
                    SqlDataReader rdr = null;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtQuestionName.Text = rdr["Question_Name"].ToString();
                        qtext.InnerHtml = Server.HtmlEncode(rdr["Raw_Text"].ToString());
                        instruction_header.InnerHtml = Server.HtmlEncode(rdr["Instruction_Text"].ToString());
                        txtLike.Text = rdr["Color_Like"].ToString();
                        txtDislike.Text = rdr["Color_Dislike"].ToString();
                        radMethod.SelectedValue = rdr["Method"].ToString();
                    }
                }
            }
        }

        protected void cancelbtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("View.aspx");
        }

        //public string GetStringBetween(string st,string start, string end)
        //{
        //    string result = "";
        //    int pFrom = st.IndexOf("");
        //    int pTo = st.LastIndexOf("");

        //    return result;
        //}
    }
}