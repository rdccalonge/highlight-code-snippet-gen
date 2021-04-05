<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HighlightApp._Default" EnableEventValidation="false" EnableViewState="true" %>
<%--EnableEventValidation="false" EnableViewState="true"--%>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
     <%--LOGIN--%>
    <div class="container">
	<div class="wrapper login" align="center">
         <div class="form-control group" style="width: 500px;" >
	    <h3 class="form-signin-heading">Login - Highlight App</h3>
       
			  <hr class="colorgraph "><br>
                   <asp:Label ID="lblError" runat="server" Text="" ForeColor="#FF3300"></asp:Label>
     
			  <asp:TextBox ID="txtUsername" runat="server" cssClass="form-control username" required="required" placeholder="Username"></asp:TextBox>
             <asp:TextBox ID="txtPassword" runat="server" cssClass="form-control password" required="required" placeholder="Password" TextMode="Password"></asp:TextBox> 
                		  
			 <asp:Button ID="btnLogin" cssClass="btn btn-lg btn-primary btn-block" runat="server" Text="Login" OnClick="btnLogin_Click" />
			       </div>
        	</div>
</div>
    </asp:Content>