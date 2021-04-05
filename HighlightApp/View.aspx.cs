using HighlightApp.HelperMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HighlightApp
{
    public partial class View : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        int studyid;


        MainHelper helper = new MainHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                //DISABLE CONTROLS IN FIRST LOAD
                DisableFunctions();
                //GET STUDY LIST
                GetStudy();
            }
            //ENABLE CONTROLS
            Enable_btnNewQuestion();
        }

        protected void DisableFunctions()
        {
            frameQ.Attributes["src"] = "Intro.aspx";
            btnAddQuestion.Attributes.Remove("href");
            btnAddQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "grey";
            btnAddQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            btnAddQuestion.Enabled = false;

            btnEditQuestion.Attributes.Remove("href");
            btnEditQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "grey";
            btnEditQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            btnEditQuestion.Enabled = false;

            btnDelQuestion.Attributes.Remove("href");
            btnDelQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "grey";
            btnDelQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            btnDelQuestion.Enabled = false;

            btnCodeSnippet.Attributes.Remove("href");
            btnCodeSnippet.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "grey";
            btnCodeSnippet.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            btnCodeSnippet.Enabled = false;

            btnEdit.Attributes.Remove("href");
            btnEdit.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "#CCCCCC";
            btnEdit.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            btnEdit.Enabled = false;

            btnDelete.Attributes.Remove("href");
            btnDelete.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "#CCCCCC";
            btnDelete.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "default";
            btnDelete.Enabled = false;


            listQuestion.Items.Clear();

        }
        protected void GetStudy()
        {
            DataTable Study = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM HighlightStudy", con);
                    adapter.Fill(Study);

                    ddlStudy.DataSource = Study;
                    ddlStudy.DataTextField = "Study_Name";
                    ddlStudy.DataValueField = "Study_Id";
                    ddlStudy.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }

            // Add the initial item - you can add this even if the options from the
            // db were not successfully loaded
            ddlStudy.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        public void getQuestions(int studyid)
        {

            DataTable Questions = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM HighlightQuestions where Study_Id = '" + studyid + "'", con);
                    adapter.Fill(Questions);
                    var test = Questions.Rows.Count;
                    listQuestion.DataSource = Questions;
                    listQuestion.DataTextField = "Question_Name";
                    listQuestion.DataValueField = "Question_Id";
                    listQuestion.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }



            }
            if (studyid != 0)
            {
                btnAddQuestion.Enabled = true;
            }

        }

        protected void listQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {

            string questionname;
            string studyname;

            if (listQuestion.SelectedItem == null)
            {
                btnEditQuestion.Enabled = false;
                btnDelQuestion.Enabled = false;
                return;

            }
            else
            {

                studyname = ddlStudy.SelectedItem.Text;
                questionname = listQuestion.SelectedItem.Text;
                Session["HideFooter"] = true;
                frameQ.Attributes["src"] = "Page.aspx?Study=" + studyname + "&Question=" + questionname;
                btnEditQuestion.Enabled = true;
                btnEditQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "white";
                btnEditQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "hand";

                btnDelQuestion.Enabled = true;
                btnDelQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "white";
                btnDelQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "hand";

                btnCodeSnippet.Enabled = true;
                btnCodeSnippet.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "white";
                btnCodeSnippet.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "hand";
            }
        }

        protected void ddlStudy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Int32.Parse(ddlStudy.SelectedItem.Value) != 0)
            {
                //GET LIST OF QUESTIONS
                studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
                getQuestions(studyid);
                Enable_btnNewQuestion();
            }
            else
            {
                DisableFunctions();

            }
        }

        protected void btnNewStudy_Click(object sender, EventArgs e)
        {
            //MODAL NEW STUDY
            modaltitle.InnerHtml = "Create a new study.";
            btnAddStudy.Text = "Add";
            btnAddStudy.CssClass = "btn btn-success";
            txtNewStudy.Text = "";
            ModalPanel.Update();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalNew", "$('#modalNew').modal();", true);
            ModalPanel.Update();
        }

        protected void btnAddStudy_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            string newstudy = txtNewStudy.Text;
            if (newstudy != "")
            {
                if (btnAddStudy.Text == "Add")
                {
                    if (ValidateStudy(newstudy))
                    {
                        lblErrorMessage.Text = "This studyname is already been used.";
                        return;
                    }
                    else
                    {
                        helper.AddStudy(newstudy);
                    }
                }
                else
                {
                    studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
                    if (ddlStudy.SelectedItem.Text != newstudy)
                    {
                        if (ValidateStudy(newstudy))
                        {
                            lblErrorMessage.Text = "This studyname is already been used.";
                            return;
                        }
                        else
                        {
                            helper.EditStudy(newstudy, studyid);
                        }
                    }
                    else
                    {

                        helper.EditStudy(newstudy, studyid);
                    }
                }

            }
            else
            {
                lblErrorMessage.Text = "Please input a study name.";
                return;
            }
            ddlStudy.Items.Clear();
            GetStudy();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "modalNew", "$('#modalNew').modal('hide');", true);


        }

        public bool ValidateStudy(string study)
        {
            bool exists = false;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // create a command to check if the username exists
                using (SqlCommand cmd = new SqlCommand("select count(*) from [HighlightStudy] where Study_name = '" + study + "'", con))
                {
                    exists = (int)cmd.ExecuteScalar() > 0;
                }
            }
            return exists;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
            string studyname;
            if (studyid != 0)
            {
                studyname = ddlStudy.SelectedItem.Text;
                modaltitle.InnerHtml = "Edit Study Name";
                btnAddStudy.Text = "Edit";
                btnAddStudy.CssClass = "btn btn-primary";
                txtNewStudy.Text = studyname;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalNew", "$('#modalNew').modal();", true);

                ModalPanel.Update();
            }
            else
            {
                return;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            string studyname;
            studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
            if (studyid != 0)
            {
                studyname = ddlStudy.SelectedItem.Text;

                delStudy.InnerHtml = studyname;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirmdelete", "$('#confirmdelete').modal();", true);
                deletePanel.Update();
            }
            else
            {
                return;
            }
        }

        protected void btnDelStudy_Click(object sender, EventArgs e)
        {
            int studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
            helper.DelStudy(studyid);
            ddlStudy.Items.Clear();
            listQuestion.Items.Clear();
            GetStudy();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "confirmdelete", "$('#confirmdelete').modal('hide');", true);

        }

        protected void Enable_btnNewQuestion()
        {
            int studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
            if (studyid == 0)
            {
                btnAddQuestion.Enabled = false;
            }
            else
            {

                btnAddQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "white";
                btnAddQuestion.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "hand";
                btnAddQuestion.Enabled = true;

                btnEdit.Enabled = true;
                btnEdit.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "#333333";
                btnEdit.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "hand";

                btnDelete.Enabled = true;
                btnDelete.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "#333333";
                btnDelete.Attributes.CssStyle[HtmlTextWriterStyle.Cursor] = "hand";

            }
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
            Session["StudyName"] = ddlStudy.SelectedItem.Text;
            Response.Redirect("Question.aspx?Study=" + studyid);
        }

        protected void btnDelQuestion_Click(object sender, EventArgs e)
        {
            if (listQuestion.SelectedIndex != -1)
            {
                var delete_id = listQuestion.SelectedItem.Value;
                if (delete_id != "")
                {

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirmdeletequestion", "$('#confirmdeletequestion').modal();", true);
                    deletePanel.Update();
                }
                else
                {
                    return;
                }
            }
            return;
        }

        protected void btnOKDeleteQuestion_Click(object sender, EventArgs e)
        {
            var delete_id = listQuestion.SelectedItem.Value;
            helper.DelQuestion(delete_id);
            ddlStudy.Items.Clear();
            listQuestion.Items.Clear();
            GetStudy();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "confirmdeletequestion", "$('#confirmdeletequestion').modal('hide');", true);
            frameQ.Attributes["src"] = "Intro.aspx";

        }

        protected void btnEditQuestion_Click(object sender, EventArgs e)
        {
            if (listQuestion.SelectedIndex != -1)
            {

                int questionid;
                questionid = Int32.Parse(listQuestion.SelectedValue);
                studyid = Int32.Parse(ddlStudy.SelectedItem.Value);
                Session["StudyName"] = ddlStudy.SelectedItem.Text;
                Response.Redirect("Question.aspx?Study=" + studyid + "&Edit=" + questionid);
            }
            else
            {
                return;
            }
        }

        protected void btnCodeSnippet_Click(object sender, EventArgs e)
        {
            
            if (listQuestion.SelectedIndex != -1)
            {

                var question_id = Int32.Parse(listQuestion.SelectedItem.Value);
                var study_id = Int32.Parse(ddlStudy.SelectedValue);
                if (question_id != 0 && study_id != 0)
                {

                    var tuple = helper.GetFieldCount(study_id, question_id);

                    if (tuple.Item1 != 0)
                    {
                        
                        //FIELDS
                        txtFields.Text = tuple.Item1.ToString();
                        ////HEADER


                        txtHeader.Text = @"<iframe id='iframe2' name='hl_frame' frameborder='0'  height='500' width='850' src='http://surveys.datassential.com/HighlightApp/Page.aspx?Study=" + tuple.Item3 + "&Question=" + tuple.Item2 + @"&Id=[% EncodeForUrl(tk) %]'>
                        <p> Your browser does not support iframes, please use a different browser to upload images.</p>
                         </iframe>";

                        txtVariables.Text = "";

                        for (var i = 1; i <= tuple.Item1; i++)
                        {
                            if (tuple.Item4 != "")
                            {
                                txtVariables.Text = txtVariables.Text + @"<input name=""" + tuple.Item2 + "_Like" + i + @""" id=""" + tuple.Item2 + "_Like" + i + @""" type=""hidden"" size=""50"">" + Environment.NewLine;
                            }
                            if (tuple.Item5 != "")
                            {
                                txtVariables.Text = txtVariables.Text + @"<input name=""" + tuple.Item2 + "_Dislike" + i + @""" id=""" + tuple.Item2 + "_Dislike" + i + @""" type=""hidden"" size=""50"">" + Environment.NewLine;
                            }
                        }

                        //FOOTER SAW 8
                        txtFooter8.Text = @"<script>
                                   $(document).ready(function() {
                                   $('button').click(function() {
                                   var question = '[% QUESTIONNAME() %]';
                                   var iframe = document.getElementById('iframe2');
                                   var innerDoc = iframe.contentDocument || iframe.contentWindow.document;
                                      $(""input[name*='_Dislike']"").each(function(index){
                                           $(this).val(innerDoc.getElementById(question + ""_DisLike"" + index).value);
                                         });
                                      $(""input[name*='_Like']"").each(function(index){
                                           $(this).val(innerDoc.getElementById(question + ""_Like"" + index).value);
                                         });
                                  });
                                  });
                                  </script> ";

                        //FOOTER SAW 7
                        txtFooter7.Text = @"<script type=""text/javascript"" src=""http://code.jquery.com/jquery-1.7.1.min.js""></script>
                                  <script>
                                  var $j = jQuery.noConflict();
                                   $j(document).ready(function() {
                                   $j('button').click(function() {
                                   var question = '[% QUESTIONNAME() %]';
                                   var iframe = document.getElementById('iframe2');
                                   var innerDoc = iframe.contentDocument || iframe.contentWindow.document;
                                      $j(""input[name*='_Dislike']"").each(function(index){
                                           $j(this).val(innerDoc.getElementById(question + ""_DisLike"" + index).value);
                                         });
                                      $j(""input[name*='_Like']"").each(function(index){
                                           $j(this).val(innerDoc.getElementById(question + ""_Like"" + index).value);
                                         });
                                  });
                                  });
                                  </script> ";

                        txtVerif8.Text = "";
                        txtVerif7.Text = "";
                        txtVerif7.Text = txtVerif7.Text + @"var question = '[% QuestionName() %]'";
                        if (tuple.Item5 != "")
                        {
                            txtVerif8.Text = txtVerif8.Text + @"
                              //REQUIRED DISLIKE
                              var ctrDislike = 0;
                              $(""input[name*='_Dislike']"").each(function(index){
                                  if ($(this).val() != ''){
                                      ctrDislike++;
                                  }
                              });
                              if (ctrDislike == 0)
                              {
                                  strErrorMessage = 'Please highlight atleast one word or phrase you DISLIKE.';
                              }";




                            txtVerif7.Text = txtVerif7.Text + @"
                              //REQUIRED DISLIKE
                              var ctrDislike = 0;
                             for(var x = 1; x <= " + tuple.Item1 + @"; x++){
                              if(document.getElementById(question + '_Dislike' + [x]).value.length > 0){
                              ctrDislike++;
                              }
                              }
                              if(ctrDislike == 0){
                               strErrorMessage = 'Please highlight atleast one word or phrase you DISLIKE.';
                              }";
                        }
                        if (tuple.Item4 != "")
                        {
                            txtVerif8.Text = txtVerif8.Text + @"
                                  //REQUIRED LIKE
                                  var ctrLike = 0;
                                  $(""input[name *= '_Like']"").each(function(index){
                                  if ($(this).val() != ''){
                                     ctrLike++;
                                      }
                                  });
                                  if (ctrLike == 0)
                                  {
                                      strErrorMessage = 'Please highlight atleast one word or phrase you LIKE.';
                                  }
                                  ";
                            txtVerif7.Text = txtVerif7.Text + @"
                                  //REQUIRED LIKE
                                  var ctrLike = 0;
                                  for(var i = 1; i <=" + tuple.Item1 + @"; i++){
                                  if(document.getElementById(question + '_Like' + [i]).value.length > 0){
                                  ctrLike++;
                                  }
                                  }
                                  if(ctrLike == 0){
                                   strErrorMessage = 'Please highlight atleast one word or phrase you LIKE.';
                                  }
                                  ";
                        }

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showCodeSnippet", "$('#showCodeSnippet').modal();", true);
                        modalSnippet.Update();

                    }
                    else
                    {
                        ErrorTitle.InnerText = tuple.Item2;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "errorPopUp", "$('#errorPopUp').modal();", true);
                        modalError.Update();
                    }
                }
                else
                {
                    return;
                }
            }
          
        }

        protected void btnHelp_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "helpPopUp", "$('#helpPopUp').modal();", true);
            modalHelp.Update();

        }

        protected void Help(object sender, EventArgs e)
        {
            string page = (sender as LinkButton).Text;
            //HELP PAGINATION
            switch (Int32.Parse(page))
            {
                case 1:
                    this.PageImage.ImageUrl = "~/Content/default/Page1.png";
                    break;
                case 2:
                    this.PageImage.ImageUrl = "~/Content/default/Page2.png";
                    break;
                case 3:
                    this.PageImage.ImageUrl = "~/Content/default/Page3.png";
                    break;
                case 4:
                    this.PageImage.ImageUrl = "~/Content/default/Page4.png";
                    break;
                case 5:
                    this.PageImage.ImageUrl = "~/Content/default/Page5.png";
                    break;
                default:
                    break;


            }

        }


    }
}