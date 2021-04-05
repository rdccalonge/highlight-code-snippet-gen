using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace HighlightApp
{
    /// <summary>
    /// Summary description for GetHL
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class GetHL : System.Web.Services.WebService
    {
        public class Results
        {
            public string Field_Name { get; set; }
            public string Field_Text { get; set; }

        }

        [WebMethod]
        public void GetResults(string study, string question, string respid)
        {
            //Results results = new Results();

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Field_Name, Field_Text FROM HighlightFields f LEFT JOIN HighlightQuestions q ON q.Question_Id = f.Question_Id LEFT JOIN HighlightStudy s ON s.Study_Id = f.Study_Id where s.Study_Name = '" + study + "' AND q.Question_Name = '" + question + "' AND f.Resp_Id = '" + respid + "'"))
                {
                    cmd.Connection = con;
                    List<Results> result = new List<Results>();
                    try
                    {

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

                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Context.Response.Write(js.Serialize(result));

                }
            }
        }
    }
}
