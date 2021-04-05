<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Intro.aspx.cs" Inherits="HighlightApp.Intro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div style="text-align:center; vertical-align:middle; height:400px; margin: 10px;" >
        <div style="display: inline-block; text-align: left; vertical-align:middle;height: 100%">
            <p style="text-align:center;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Content/default/highlightLogo.png" Height="120" />
                </p>
            <%--<h2 style=" font-family: 'Verdana'; background: -webkit-linear-gradient(#0094ff, #b200ff); -webkit-background-clip: text;-webkit-text-fill-color: transparent;"> Survey Highlight Application </h2>--%>
         <p style="font-family:'Lucida Console', monospace; line-height:150%;">
            Build 1.0.0<br />
            Copyright 2017 Datassential, Inc. All rights reserved.<br />
           Last modified 22 July 2017<br />
             </p>
            <%--<p style="font-family:'Lucida Console', monospace"><u># Change Log</u>
                 </p>
            <p style="font-family:'Lucida Console', monospace">
                ## [Unreleased]<br />
                ## [1.0.0] - 2017-06-01<br />
             </p>
             <p style="font-family:'Lucida Console', monospace">
                ### Added<br />
                - Reset button for respondent side<br />
                - Cancel Button <br />
                <br />============
             </p>
             <p style="font-family:'Lucida Console', monospace">
                ## [Unreleased]<br />
                ## [1.0.0] - 2017-05-17<br />
             </p>
             <p style="font-family:'Lucida Console', monospace">
                ### Changed<br />
                - Improved Kendo Text Editor<br />
                ### Added<br />
                - Delete and Edit feature for highlight questions<br />
                - Kendo Color Picker<br />
                - Addded Highlighting Method Per Word <br />
                 ### Fixed<br />
                - Question Name Duplicate<br />
                <br />============
             </p>--%>
               
             </div>
       
       
    </div>
    </form>
</body>
</html>
