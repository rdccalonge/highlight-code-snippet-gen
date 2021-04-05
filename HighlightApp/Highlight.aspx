<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Highlight.aspx.cs" Inherits="HighlightApp.Highlight" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Highlight</title>

    <link rel="stylesheet" href="Content/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/MyStyle.css" />

    <script src="Scripts/jquery-3.1.1.min.js"></script>

</head>
<body>
    <div class="jumbotron text-center">
        <h1>Highlight Configuration</h1>
    </div>
    <form id="form1" runat="server">
        <div class="container">
           
            <div class="row row-flex row-flex-wrap">

                <div class="col-md-7">
                    <div id="highlightedtext" class="well">
                        <asp:Literal ID="Labelhighlight" runat="server" Mode="PassThrough"></asp:Literal>
                    </div>
                </div>
                <div class="col-md-1 btn-add">
                    <div>
                        <a href="#" class="get_highlight"><i class="glyphicon glyphicon-plus"></i></a>
                    </div>
                </div>
                 
                <div class="col-md-4 ">
                    <div class="well">
                        <div id="fields" runat="server">
                            <%--HIGHLIGHT FIELDS--%>
                            <asp:Label ID="Label1" runat="server" Text="HIGHLIGHTED FIELDS" Style="text-align: center;" Width="100%" Font-Bold="True" ForeColor="#006600"></asp:Label>
                            <input runat="server" id="Highlighted_1" name="Highlighted_1" class="form-control highlighted" readonly="true" />
                            <input runat="server" id="Highlighted_2" name="Highlighted_2" class="form-control highlighted" readonly="true"/>
                            <input runat="server" id="Highlighted_3" name="Highlighted_3" class="form-control highlighted" readonly="true"/>
                            <input runat="server" id="Highlighted_4" name="Highlighted_4" class="form-control highlighted" readonly="true"/>
                            <input runat="server" id="Highlighted_5" name="Highlighted_5" class="form-control highlighted" readonly="true"/>
                        </div>
                        
                        <asp:LinkButton ID="btnAddField" runat="server" OnClick="btnAddField_Click" OnClientClick="SaveToHidden()" CssClass="btn btn-success" ToolTip="Add another field." Width="100%">ADD ANOTHER FIELD</asp:LinkButton>
                        <input type="hidden" id="field_used" name="field_used" />
                        <input type="hidden" id="field_count" name="field_count" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7">
                    <h3>Notes:</h3>
                <ul>
                    <li>Highlighted fields should be in top to bottom order to avoid error/confusion.</li>
                    <li>When highlighting a HTML formatted phrase, avoid leading or trailing space.</li>
                </ul>  
                </div>
                  <div class="col-md-1">
                    
                </div>
                <div class="col-md-4" style="text-align: center; vertical-align:middle;">
                     <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="SaveToHidden()" ToolTip="Save Your Work"><span class="glyphicon glyphicon-floppy-disk hl_buttons"></span></asp:LinkButton>
                    <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" OnClientClick="Reset()" ToolTip="Reset all Highlighted Text"><span class="glyphicon glyphicon-refresh hl_buttons"></span></asp:LinkButton>
                </div>
                
            </div>
           
            <div class="row">
                <%--STORE TEXT WITH SPAN TAGS--%>
                <input type="hidden" id="div_content" name="div_content" />
            </div>
        </div>
    </form>


    <script>

       
        //check if highlighted is within/between range
        function isOrContains(node, container) {
            while (node) {
                if (node === container) {
                    return true;
                }
                node = node.parentNode;
            }
            return false;
        }
        //get selected range
        function elementContainsSelection(el) {
            var sel;
            if (window.getSelection) {
                sel = window.getSelection();
                if (sel.rangeCount > 0) {
                    for (var i = 0; i < sel.rangeCount; ++i) {
                        if (!isOrContains(sel.getRangeAt(i).commonAncestorContainer, el)) {
                            return false;
                        }
                    }
                    return true;
                }
            } else if ((sel = document.selection) && sel.type != "Control") {
                return isOrContains(sel.createRange().parentElement(), el);
            }
            return false;
        }


        $(document).ready(function () {

            $('#field_count').val($('.highlighted').length);
            $('.get_highlight').on('click', function () {
                if (window.getSelection) {
                    //if inside the highlight box
                    if (elementContainsSelection(document.getElementById('highlightedtext'))) {
                        try {
                            var sel = window.getSelection();

                            //get all previous highlighted text
                            var highlightedText = document.getElementsByClassName("noselect");

                            if (highlightedText.length != 0) {
                                for (var i = 0; i < highlightedText.length; i++) {
                                    //check if highlighted contains previous
                                    if (sel.containsNode(highlightedText[i], true)) {
                                        alert("The selection must not contain characters that was already highlighted.");
                                        return;
                                    }
                                }
                            }


                            //continue
                            var ctrErr = 0;
                            //var span = document.createElement("span");
                            //span.className = "select";
                            //span.onclick = "selectText(this)";
                            var range = window.getSelection().getRangeAt(0),
                            span = document.createElement('span');
                            span.className = "select";
                            span.onclick = "selectText(this)";

                            //if element is an image
                            var images = document.getElementsByTagName('img');
                            var img_alt = "";
                            if (images.length != 0) {
                                for (var i = 0; i < images.length; i++) {
                                    //check if highlighted contains previous
                                    if (sel.containsNode(images[i], true)) {
                                        span.className = span.className + " image";
                                        img_alt = images[i].alt;
                                    }
                                }
                            }

                            //new

                            


                            //var range = sel.getRangeAt(0).cloneRange();
                            //validate each highlight fields
                            $(".highlighted").each(function (index) {
                                if ($(this).val() == "") {
                                    span.id = index;
                                    if (img_alt != "") {
                                        $(this).val(img_alt);
                                    }
                                    else {
                                        $(this).val(sel);
                                    }
                                    try {
                                        span.appendChild(range.extractContents());
                                        range.insertNode(span);
                                        //range.surroundContents(span);

                                    }
                                    catch (e) {
                                        $(this).val("");
                                        alert(e.message);
                                        return;
                                    }
                                    //Remove Highlight Selection
                                        if (window.getSelection) {
                                            if (window.getSelection().empty) {  // Chrome
                                                window.getSelection().empty();
                                            } else if (window.getSelection().removeAllRanges) {  // Firefox
                                                window.getSelection().removeAllRanges();
                                            }
                                        } else if (document.selection) {  // IE?
                                            document.selection.empty();
                                        }
                                    ctrErr = 1;
                                    return false;

                                }
                            });

                            if (ctrErr == 0) {
                                alert("All highlighted options have been filled. Please add another field or clear existing.");

                                return;
                            }

                        } catch (e) {
                            alert(e.message);
                            return;
                        }

                    }
                    else {

                        alert("Please highlight inside the question text!");
                        return;
                    }
                }
            });

        });
        function SaveToHidden() {
            //save results to div content
            var fields_count = 0;
            $(".highlighted").each(function () {
                if ($(this).val() != '') {
                    fields_count++;
                }
            });
            //save number of fields
            document.getElementById("field_used").value = fields_count;
            var content = document.getElementById("highlightedtext").innerHTML;

            document.getElementById("div_content").value = content;

        };
        function Reset() {
            //clear highlighted fields
            $(".highlighted").val('')
        };

    </script>
    <script src="~/Scripts/bootstrap.min.js"></script>

</body>
</html>
