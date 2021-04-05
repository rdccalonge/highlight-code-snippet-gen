using HighlightApp.HelperMethods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace HighlightApp
{
    public partial class _Default : Page
    {


   

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.User.Identity.IsAuthenticated)
            {
                Response.Redirect("View.aspx");
            }
        }

     
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtUsername.Text == "admin" && txtPassword.Text == "nimda77@")
            {

                FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, false);
            }
            else
            {
                lblError.Text = "Username and Password do not match."; 
                return; 
            }
        }


       
    }
}