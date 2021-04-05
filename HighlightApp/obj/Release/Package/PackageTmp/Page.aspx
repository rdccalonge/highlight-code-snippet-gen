<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Page.aspx.cs" Inherits="HighlightApp.Page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	
	<link type="text/css" rel="stylesheet" media="screen"  runat="server" href="Content/bootstrap.min.css" />
	<link type="text/css" rel="stylesheet"  media="screen"  runat="server" href="Content/PageStyle.css" />
    <!--[if lt IE 9]><!-->
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <!--<![endif]-->
    <!--[if gte IE 9]><!-->
	 <script type="text/javascript" src="Scripts/jquery-3.1.1.min.js"></script>
    <!--<![endif]-->
</head>
<body>
	<form id="page_form" runat="server">
	<div class="wrapper">
		<div class="row container">
		<div class="row">
			<div class="col-md-12 instruction-text">
				<%--<div id="lblHeader" class="col-md-12"  runat="server">
					
				</div>--%>
				<div class="directions">
					
					<%--<span id="spanLike">Highlight the words/phrases you <b id="liketext" runat="server">LIKE in <%=colorlike.ToUpper() %></b> <br /></span>--%>
                        <asp:Label ID="lblInstruction" runat="server" style="padding:2px"></asp:Label>
                        <%--<asp:Label ID="labelLike" runat="server">Highlight the words/phrases you <b id="liketext" runat="server">LIKE in <%=colorlike.ToUpper() %></b><br /></asp:Label>
                        <asp:Label ID="labelDislike" runat="server">Highlight the words/phrases you <b id="disliketext" runat="server">DISLIKE in <%=colordislike.ToUpper() %></b></asp:Label>--%>
					<%--<span id="spanDislike">Highlight the words/phrases you <b id="disliketext" runat="server">DISLIKE in <%=colordislike.ToUpper() %></b> <br /></span>--%>
					
				</div>
                <div class="functions-div">
                    <table class="nav-table">
                        <tr>
                            <td class="function">
                                
                            </td>
                            <td class="function">
                                
                                <asp:Label ID="likeselect" CssClass="colorselect" style="padding:10px" runat="server" onclick="changeColor(this)"><%=colorlike %></asp:Label>
                                <asp:Label ID="dislikeselect" CssClass="colorselect" style="padding:10px" runat="server" onclick="changeColor(this)"><%=colordislike %></asp:Label>
                               <%-- <span id="likeselect" class="colorselect"  runat="server" style="padding:10px" onclick="changeColor(this)"><%=colorlike %></span>--%>
					            <%--<span id="dislikeselect" class="colorselect" runat="server" style="padding:10px" onclick="changeColor(this)"><%=colordislike %></span>--%>
                            </td>
                            <td class="reset">
                                <asp:LinkButton ID="btn_reset" CssClass="btn btn-default" runat="server" style="text-align:right;" OnClick="btn_reset_Click" OnClientClick="Reset()">RESET</asp:LinkButton>
                                 
                                </td>
                        </tr>
                    </table>
                    
                   
                </div>
            </div>
			 </div>
		</div>
		<div class="row">

			<div id="lblContent" class="col-md-12"  runat="server">
		   
				</div>
		</div>
			<div class="row">
				<div class="col-md-12 redirect">
					<asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="SaveToJson()" Visible="False"><span class="glyphicon glyphicon-floppy-saved"></span></asp:LinkButton>
				</div>
				</div>
		
		<div class="row fields">
			<input type="hidden" id="colorType" name="colorType"/>
			<input type="hidden" id="LikeResults" name="LikeResults" runat="server"/>
			<input type="hidden" id="DislikeResults" name="DislikeResults" runat="server"/>
		   <div class="col-md-12 hidden-like" >
			   
			   <asp:HiddenField ID="hiddenQuestion" runat="server" />
		   </div>
			  <div class="col-md-12 hidden-dislike">
			   
		   </div>
		</div>
	</div>
 
		
	</form>
	<script type="text/javascript">

		var varQuestion = document.getElementById('<%= hiddenQuestion.ClientID %>').value;
	    $(document).ready(function () {
	       
			if (self != top) {
			     //Save Function For Testing
			    //btnSave.style.display = 'none';
			};
			
            //Create Text Box For Highlight
			$('.select').each(function(index,value){
				$('<input>').attr({
					type: 'hidden',
					id: varQuestion +  '_Like' + index,
					name: varQuestion + '_Like' + index,
				}).appendTo($('.hidden-like'));
				$('<input>').attr({
					type: 'hidden',
					id: varQuestion + '_DisLike' + index,
					name: varQuestion + '_DisLike' + index,
				}).appendTo($('.hidden-dislike'));
		 });
			$(".select").click(function () { selectText(this); });
		});

        //Change Color When Clicked
		function changeColor(color) {
			document.getElementById('colorType').value=color.innerHTML;
		}
		
		//Save to Json Object
		function SaveToJson() {
			var likeArr = [];
			var dislikeArr = [];

			//LIKE
			$('.hidden-like input:text').each(function () {
				likeArr.push( $(this).val());
			});

			//DISLIKE
			$('.hidden-dislike input:text').each(function () {
				dislikeArr.push($(this).val());
			});
			document.getElementById("LikeResults").value = likeArr;
			document.getElementById("DislikeResults").value = dislikeArr;
		}



		function selectText(element) {
		   
			var userSelection;
			var range;
			var color;
			var like = "";
			var dislike = "";

		    try{
		        like = document.getElementById('likeselect').innerHTML;
		    }
		    catch (err) {

		    }
		    try {
		        dislike = document.getElementById('dislikeselect').innerHTML;
		    }
		    catch (err) {

		    }
		    
		   
			if (window.getSelection) { //for IE 9 UP
				userSelection = window.getSelection();
				color = document.getElementById('colorType').value;
				range = document.createRange();
				range.selectNode(element);
				userSelection.removeAllRanges();
				userSelection.addRange(range);
				
			    //IMAGE
				var img;
				if (element.classList.contains('image')) {
				    img = element.getElementsByTagName("img")[0];
				    range = img.alt;
				}
				
			    //ONE COLOR VALIDATION
				if (color == "") {
				    if (like == "") {
				        color = dislike;
				    }
				    else if (dislike == "") {
				        color = like;
				    }
				    else {
				        color = like;
				    }
				}
				var dislikeElement = document.getElementById(varQuestion + "_DisLike" + element.id);
				var likeElement = document.getElementById(varQuestion + "_Like" + element.id);
				if (color == dislike) {
				    element.style.color = dislike;
				    if (img != null) {
				        img.style.border = "1px solid " + dislike;
				    }
				    else {
				        element.style.border = "1px solid " + dislike;
				    }
				    range = range.toString().trim();
				    dislikeElement.value = range;
				    likeElement.value = "";

					}
				else if(color == like){
				    element.style.color = like;
				    if (img != null) {
				        img.style.border = "1px solid " + like;
				    }
				    else {
				        element.style.border = "1px solid " + like;
				    }
				    range = range.toString().trim();
				    likeElement.value = range;
				    dislikeElement.value = "";
					}
			} //FOR IE <=8 
			else if (document.selection) {
                
			    userSelection = document.selection;
		         color = document.getElementById('colorType').value;
		         range = document.body.createTextRange();
		         range.moveToElementText(element);
		         range.select();

			    ////IMAGE
		         var alt = "";
		         var img;
		         var classArr = element.className.split(' ');
		         if (classArr[1] == 'image') {
		             img = element.getElementsByTagName("img")[0];
		             alt = img.alt;
		         }
               
			    //ONE COLOR VALIDATION
                if(color == ""){
		             if (like == "") {
		                 color = dislike;
		             }
		             else if (dislike == "") {
		                 color = like;
		             }
		             else {
		                 color = like;
		             }
                }   
                
                if (color == dislike) {
			        element.style.color = dislike;
			            if (alt != "") {
			            img.style.border = "1px solid " + dislike;
			                document.getElementById(varQuestion + "_DisLike" + element.id).value = alt;
			            }
			            else {
			                element.style.border = "1px solid " + dislike;
			                document.getElementById(varQuestion + "_DisLike" + element.id).value = range.text;
			            }
			        document.getElementById(varQuestion + "_Like" + element.id).value = "";
			        }
			    else if(color == like){
			        element.style.color = like;
			        
			        document.getElementById(varQuestion + "_DisLike" + element.id).value = "";
			        if (alt != "") {
			            img.style.border = "1px solid " + like;
			                document.getElementById(varQuestion + "_Like" + element.id).value = alt;
			            }
			            else {
			                element.style.border = "1px solid " + like;
			                document.getElementById(varQuestion + "_Like" + element.id).value = range.text;
			            }
			        }
		         }
		
		}
		function Reset() {
            $(":text").val('');
		}
</script>
</body>
</html>
