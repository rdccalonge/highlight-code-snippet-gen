<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetHighlights.aspx.cs" Inherits="HighlightApp.GetHighlights" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    
</head>

<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="scm" runat="server" EnablePageMethods="true" />
        <script type="text/javascript">
            //var urlString = "GetHighlights.aspx/GetResults?study=" + studyname + "&question=" + questionname + "&id=" + respid + "";
            $(document).ready(function () {
                var test = "";
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "GetHighlights.aspx/GetResults",
                data: "{}",
                dataType: "json",
                success: function (data) {
                    var items = data.d;
                    var fragment = "<ul>";
                    $.each(items, function (index, val) {

                        var fieldName = val.Field_Name;
                        var fieldText = val.Field_Text;
                        fragment += "<li> " + fieldName + " :: " + fieldText + "</li>";
                    });
                    $("#contentholder").append(fragment);
                },
                error: function (result) {
                    alert("Error");
                }
            });
        });

    </script>
    <div id="contentholder">
        
    
    </div>
    </form>
</body>
</html>
