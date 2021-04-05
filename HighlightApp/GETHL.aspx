<%@ Page Language="C#"%>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<script runat="server">
     void Page_Load()
        {
            WebRequest request = WebRequest.Create("http://localhost:81/HighlightApp/GetHL.asmx/GetResults?respid=Reni&question=A7&study=SAWZ1701SSI");
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            Response.Write(responseFromServer);
            Response.End();
        }
</script>

<!DOCTYPE html>

<html>
	<head>
	    <meta http-equiv="content-type" content="text/json; charset=UTF-8">
	    <title>Datassential</title>
	</head>
	<body>
	</body>
</html>

