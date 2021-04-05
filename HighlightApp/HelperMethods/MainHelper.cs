using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HighlightApp.HelperMethods
{
    public class MainHelper
    {

        string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public void AddStudy(string study)
        {
            //add new study
            string query = "INSERT INTO HighlightStudy (Study_Name) VALUES ('" + study + "');";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }

        }
        public void EditStudy(string studyname, int studyid)
        {
            //edit study name
            string query = "UPDATE s SET Study_Name = '" + studyname + "' From HighlightStudy s Where Study_Id = '" + studyid + "';";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }

        }

        public void DelStudy(int studyid)
        {
            //delete study
            string query = "DELETE FROM HighlightStudy Where Study_Id = '" + studyid + "'; DELETE FROM HighlightQuestions Where Study_Id = '" + studyid + "'; DELETE FROM HighlightFields Where Question_Id = '" + studyid + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }

        }
        public void DelQuestion(string questionid)
        {
            //delete question
            string query = "DELETE FROM HighlightQuestions Where Question_Id = '" + questionid + "'; DELETE FROM HighlightFields Where Question_Id = '" + questionid + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }

        }

        public Tuple<int, string, string, string, string> GetFieldCount(int studyid, int questionid)
        {
            //get set variable for code snippet
            var fieldcount = 0;
            var questionname = "";
            var studyname = "";
            var like = "";
            var dislike = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand("select * from HighlightQuestions LEFT JOIN highlightStudy ON highlightStudy.Study_Id = highlightQuestions.Study_id where Question_Id = '" + questionid + "' and highlightStudy.Study_Id ='" + studyid + "'", con))
                {
                    SqlDataReader rdr = null;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        fieldcount = Int32.Parse(rdr["Fields_Count"].ToString());
                        questionname = rdr["Question_Name"].ToString();
                        studyname = rdr["Study_Name"].ToString();
                        like = rdr["Color_Like"].ToString();
                        dislike = rdr["Color_Dislike"].ToString();

                    }
                }
            }
            return Tuple.Create(fieldcount, questionname, studyname, like, dislike);
        }
    }
}