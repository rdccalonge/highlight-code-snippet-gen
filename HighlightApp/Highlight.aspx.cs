using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HighlightApp
{
    public partial class Highlight : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        string studyid;
        string questionname;
        string likecolor = "";
        string dislikecolor = "";
        int questionid = 0;
        string text = "";
        string instruction = "";




        protected void Page_Load(object sender, EventArgs e)
        {

            studyid = Request.QueryString["Study"];
            questionname = Request.QueryString["Question"];
            likecolor = Session["Color_Like"].ToString();
            dislikecolor = Session["Color_Dislike"].ToString();
            

            if (IsPostBack)
            {
                List<string> field_text = new List<string>();
                //load existing on postback
                int fieldCount = Int32.Parse(Request.Form["field_count"]);
                for (int i = 1; i <= fieldCount; i++)
                {
                    field_text.Add(Request.Form["Highlighted_" + i]);
                }
                
                for (int i = 6; i <= fieldCount; i++)
                {
                    TextBox tb = new TextBox();
                    tb.ID = "Highlighted_" + i;
                    tb.CssClass = "form-control highlighted";
                    tb.ReadOnly = true;
                    tb.Text = field_text[i - 1];
                    fields.Controls.Add(tb);
                }

                Labelhighlight.Text = Request.Form["div_content"];

            }
            else
            {

                text = Session["Question_Text"].ToString();

                if (text != null)
                {
                    Labelhighlight.Text = text;
                }
                Session["Fields"] = 0;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            int fields_used = 0;
            questionid = Int32.Parse(Session["Question_Id"].ToString());
            instruction = Session["Instruction_Text"].ToString();
            string content = Request.Form["div_content"];
            fields_used = Int32.Parse(Request.Form["field_used"]);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    if (questionid == 0)
                    {
                        //add new highlight question 
                        cmd.CommandText = "INSERT INTO HighlightQuestions (Question_Name,Question_Text,Instruction_Text,Study_Id,Question_Redirect, Fields_Count, Color_Like, Color_Dislike, Raw_Text, Method) VALUES ('" + questionname + "','" + content + "','" + instruction + "','" + studyid + "','','" + fields_used + "','" + likecolor + "','" + dislikecolor + "', '" + Session["Question_Text"].ToString() + "', 2)";
                    }
                    else
                    {
                        //edit highlight question 
                        cmd.CommandText = "UPDATE HQ set HQ.Question_Name = '" + questionname + "', HQ.Question_Text = '" + content + "', HQ.Fields_Count =  '" + fields_used + "', HQ.Color_Like = '" + likecolor + "', HQ.Color_Dislike = '" + dislikecolor + "', HQ.Raw_Text = '" + Session["Question_Text"].ToString() + "', HQ.Instruction_Text = '" + instruction + "', HQ.Method = 2 from highlightquestions HQ where HQ.Question_Id = '" + questionid + "'";
                    }
                    cmd.Connection = con;
                    con.Open();
                    int check = cmd.ExecuteNonQuery();
                    con.Close();
                    if (check != 0)
                    {
                        Session["StoredId"] = studyid;
                        Session["StoredQuestionName"] = questionname;
                        Response.Redirect("View.aspx");
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

        public void btnAddField_Click(object sender, EventArgs e)
        {
            //additional field
            List<string> field_text = new List<string>();
            int fieldCount = Int32.Parse(Request.Form["field_count"]);
            for (int i = 1; i <= fieldCount; i++)
            {
                field_text.Add(Request.Form["Highlighted_" + i]);
            }
            fieldCount = fieldCount + 1;
            TextBox tb = new TextBox();
            tb.ID = "Highlighted_" + fieldCount;
            tb.CssClass = "form-control highlighted";
            tb.ReadOnly = true;
            fields.Controls.Add(tb);
            Session["Fields"] = fieldCount;
            Session["Field_Text"] = field_text;
           
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Labelhighlight.Text = Session["Question_Text"].ToString();
        }
    }
}