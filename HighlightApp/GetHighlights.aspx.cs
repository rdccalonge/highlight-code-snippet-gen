﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HighlightApp
{
    public partial class GetHighlights : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            respid = Request.QueryString["id"];
            questionname = Request.QueryString["question"];
            studyname = Request.QueryString["study"];
        }

        public class Results
        { 
            public string Field_Name { get; set; }
            public string Field_Text { get; set; }
            
        }

        public static string respid { get; set; }
        public static string questionname { get; set; }
        public static string studyname { get; set; }


        [WebMethod]

        public static List<Results> GetResults()
        {
            
            
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Field_Name, Field_Text FROM HighlightFields f LEFT JOIN HighlightQuestions q ON q.Question_Id = f.Question_Id LEFT JOIN HighlightStudy s ON s.Study_Id = f.Study_Id where s.Study_Name = '" + studyname + "' AND q.Question_Name = '" + questionname + "' AND f.Resp_Id = '" + respid + "'"))
                {
                    cmd.Connection = con;
                    List<Results> result = new List<Results>();
                    try {
                    
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string fieldname = sdr["Field_Name"].ToString();
                            string fieldtext = sdr["Field_Text"].ToString();

                            result.Add(new Results
                            {
                                Field_Name = fieldname,
                                Field_Text = fieldtext,
                            });
                        }
                    }
                    }
                    finally
                    {
                        con.Close();
                    }
                    
                    return result; 
                }
            }
        }
    }
}