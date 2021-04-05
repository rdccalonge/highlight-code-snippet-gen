using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HighlightApp
{
    
    public partial class Page : System.Web.UI.Page
    {
        string studyname;
        string questionname;
        string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        string instruction_text;
        string questiontext;
        string questionredirect;
        public string colorlike;
        public string colordislike;
        int studyid;
        int questionid;
        string respid;
        //string[] likes;
        //string[] dislikes;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            studyname = Request.QueryString["Study"];
            questionname = Request.QueryString["Question"];
            respid = Request.QueryString["Id"];
            GetQuestionText();
            hiddenQuestion.Value = questionname;
            lblInstruction.Text = instruction_text;
            lblContent.InnerHtml = questiontext;
            //liketext.Style.Add("color", colorlike);
            likeselect.Style.Add("background", colorlike);
            //disliketext.Style.Add("color", colordislike);
            dislikeselect.Style.Add("background", colordislike);
            
            if(colorlike == "")
            {
                //this.labelLike.Style.Add("display", "none");
                this.dislikeselect.Style.Add("visibility", "hidden");
                //labelLike.Visible = false;
                likeselect.Visible = false;
                //dislikeselect.Visible = false;
            }
            if (colordislike == "")
            {
                //this.labelDislike.Style.Add("display", "none");
                this.likeselect.Style.Add("visibility", "hidden");
                //labelDislike.Visible = false;
                //likeselect.Visible = false;
                dislikeselect.Visible = false;
            }
            
            
        }

        private void GetQuestionText()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                
                con.Open();
                // create a command to check if the username exists
                using (SqlCommand cmd = new SqlCommand("select q.Question_Text,q.Instruction_Text,Question_Redirect,q.Study_Id,q.Question_Id,q.Color_Like,q.Color_Dislike from HighlightQuestions q LEFT JOIN HighlightStudy s ON s.Study_Id = q.Study_Id where q.Question_Name = '" + questionname + "' and s.Study_Name = '" + studyname + "'" , con))
                {
                    SqlDataReader rdr = null;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        instruction_text = rdr["Instruction_Text"].ToString();
                        questiontext = rdr["Question_Text"].ToString();
                        questionredirect = rdr["Question_Redirect"].ToString();
                        studyid = (int)rdr["Study_Id"];
                        questionid = (int)rdr["Question_Id"];
                        colorlike = rdr["Color_Like"].ToString();
                        colordislike = rdr["Color_Dislike"].ToString();

                    }
                }
            }
            

        }

   
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var likes = Request.Form["LikeResults"].Split(',');
            var dislikes = Request.Form["DislikeResults"].Split(',');
            SaveResultsToDB(likes, dislikes);
        }

        private void SaveResultsToDB(string[] LIKES, string[] DISLIKES)
        {
            
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                int likesCounter = 1;
                con.Open();
                foreach (string like in LIKES)
                {
                    if (like != "")
                    {
                        try { 
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO HighlightFields  ([Study_Id],[Question_Id],[Resp_Id],[Field_Name],[Field_Text]) VALUES (@param1, @param2, @param3, @param4, @param5)", con))
                        {
                                cmd.Parameters.AddWithValue("@param1", studyid);
                                cmd.Parameters.AddWithValue("@param2", questionid);
                                cmd.Parameters.AddWithValue("@param3", respid);
                                cmd.Parameters.AddWithValue("@param4", questionname + "Like" + likesCounter);
                                cmd.Parameters.AddWithValue("@param5", like);
                            cmd.ExecuteNonQuery();
                            
                        }
                        likesCounter++;
                        }
                        catch(SqlException e) when(e.Number == 8178){
                            throw new System.ArgumentException("Parameter cannot be null", "Respondent ID");
                        }
                    }
                }

                int dislikesCounter = 1;
                foreach (string dislike in DISLIKES)
                {
                    if (dislike != "")
                    {

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO HighlightFields  ([Study_Id],[Question_Id],[Resp_Id],[Field_Name],[Field_Text]) VALUES (@param1, @param2, @param3, @param4, @param5)", con))
                        {
                            cmd.Parameters.AddWithValue("@param1", studyid);
                            cmd.Parameters.AddWithValue("@param2", questionid);
                            cmd.Parameters.AddWithValue("@param3", respid);
                            cmd.Parameters.AddWithValue("@param4", questionname + "Dislike" + dislikesCounter);
                            cmd.Parameters.AddWithValue("@param5", dislike);
                            cmd.ExecuteNonQuery();

                        }
                        dislikesCounter++;
                    }
                }


            }
            
        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            //GetQuestionText();
            //lblContent.InnerHtml = questiontext;
        }
    }
}