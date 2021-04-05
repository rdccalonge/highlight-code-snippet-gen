<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="HighlightApp.Question" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/kendo.common.min.css" />
    <link rel="stylesheet" href="Content/kendo.default.min.css" />
    <link rel="stylesheet" href="Content/kendo.default.mobile.min.css" />
    <link rel="stylesheet" href="Content/MyStyle.css" />
    <script src="Scripts/jquery.min.js"></script>
    <script src="Scripts/kendo.all.min.js"></script>
</head>
<body>

    <div class="container">
        <form method="post" runat="server" id="formQuestion">
            <div class="col-md-12 main-question">
                <div class="form-group study-title">
                    <h1>
                        <asp:Label ID="lblStudy" runat="server" Text="" ForeColor="#333333" ToolTip="Study Name"></asp:Label></h1>
                    <hr />
                </div>

                <div class="form-group">
                    <%--QUESTION NAME--%>
                    <label for="txtQuestionName">Question Name </label>
                    <asp:TextBox ID="txtQuestionName" class="form-control" type="text" placeholder="eg. A1" runat="server" ToolTip="Question Name"></asp:TextBox>
                    <asp:Label ID="lblErrorMessage" runat="server" Text="" CssClass="help-block"></asp:Label>
                </div>

                <div class="form-group">
                    <%--METHOD RADIO--%>
                    <label for="radMethod">Method : </label>
                    <asp:RadioButtonList ID="radMethod" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="test" ToolTip="Highlight Method">
                        <asp:ListItem Value="1" Text="<span class='radiolist'>Per Word</span>" Selected="True" />
                        <asp:ListItem Value="2" Text="<span class='radiolist'>By Phrase</span>" />
                    </asp:RadioButtonList>
                </div>

                <label>Instruction Header</label>
                <asp:Label ID="lblErrorMessage2" runat="server" Text="" CssClass="help-block"></asp:Label>

                <div id="example" class="form-group">

                    <div class="well well-medium">
                        <%--INSTRUCTION TEXT--%>
                        <textarea id="instruction_header" name="instruction_header" style="height: 10px" class="form-control editor" rows="5" runat="server"></textarea>
                        <div style="padding: 10px;">
                            <%--LIKE SELECTOR--%>
                            <div class="box-col col-md-6">
                                <asp:CheckBox ID="chkLike" runat="server" Checked="True" />
                                <label>Like : </label>
                                <div id="paletteLike" data-role="colorpalette"></div>
                            </div>
                            <%--DISLIKE SELECTOR--%>
                            <div class="box-col col-md-6">
                                <asp:CheckBox ID="chkDislike" runat="server" Checked="True" />
                                <label>Dislike : </label>
                                <div id="paletteDislike" data-role="colorpalette"></div>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="form-group" style="text-align: center;">
                    <asp:Label ID="colorError" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>

                <div class="form-group">
                    <%--QUESTION TEXT--%>
                    <label for="qeditor">Question Text </label>
                    <asp:Label ID="Label1" runat="server" Text="" CssClass="help-block"></asp:Label>
                    <textarea id="qtext" name="qtext" class="form-control editor" rows="20" runat="server"></textarea>
                </div>

                <asp:Button ID="nextbtn" Text="NEXT" class="btn btn-primary" runat="server" OnClick="nextEventMethod" ToolTip="Next Button" />
                <asp:Button ID="cancelbtn" Text="CANCEL" class="btn btn-danger" runat="server" OnClick="cancelbtn_Click" ToolTip="Return" />
                <asp:TextBox ID="txtLike" runat="server" Style="display: none;" Text="green"></asp:TextBox>
                <asp:TextBox ID="txtDislike" runat="server" Style="display: none;" Text="red"></asp:TextBox>
            </div>
        </form>
    </div>

    <%--style="display: none;"--%>

    <script>

        //Text HTML Editor
        $("#paletteLike").kendoColorPalette({
            palette: ["green", "orange", "brown", "red", "blue", "purple"],
            columns: 6
        });
        $("#paletteDislike").kendoColorPalette({
            palette: ["red", "orange", "brown", "green", "blue", "purple"],
            columns: 6
        });



        $(document).ready(function () {
            //LIKE PALLETE
            var likeVal = document.getElementById('<%=txtLike.ClientID %>');
            var colorLike = $("#paletteLike").data("kendoColorPalette");
            colorLike.bind("change", function (e) {
                switch (e.value) {
                    case '#008000':
                        likeVal.value = 'green';
                        break;
                    case '#ffa500':
                        likeVal.value = 'orange';
                        break;
                    case '#a52a2a':
                        likeVal.value = 'brown';
                        break;
                    case '#ff0000':
                        likeVal.value = 'red';
                        break;
                    case '#0000ff':
                        likeVal.value = 'blue';
                        break;
                    case '#800080':
                        likeVal.value = 'purple';
                        break;
                    default:
                        alert('error');
                }
            });
            if (likeVal.value != "") {
                colorLike.value(likeVal.value);
            }

            //DISABLE DISLIKE PALLETE ON LOAD
            if ($("#chkLike").is(':checked') == false) {
                colorLike.enable(false);
            }


            //DISABLE LIKE FUNCTION
            $("#chkLike").change(function () {
                colorLike.enable(this.checked);
                if (this.checked == false) {
                    likeVal.value = '';
                    colorLike.value('none')
                }
            });

            //DISABLE DISLIKE FUNCTION
            $("#chkDislike").change(function () {
                colorDislike.enable(this.checked);
                if (this.checked == false) {
                    dislikeVal.value = '';
                    colorDislike.value('none');
                }
            });


            //DISLIKE PALETTE
            var dislikeVal = document.getElementById('<%=txtDislike.ClientID %>');
            var colorDislike = $("#paletteDislike").data("kendoColorPalette");
            colorDislike.bind("change", function (e) {
                switch (e.value) {
                    case '#008000':
                        dislikeVal.value = 'green';
                        break;
                    case '#ffa500':
                        dislikeVal.value = 'orange';
                        break;
                    case '#a52a2a':
                        dislikeVal.value = 'brown';
                        break;
                    case '#ff0000':
                        dislikeVal.value = 'red';
                        break;
                    case '#0000ff':
                        dislikeVal.value = 'blue';
                        break;
                    case '#800080':
                        dislikeVal.value = 'purple';
                        break;
                    default:
                        alert('error');
                }
            });
            if (dislikeVal.value != "") {
                colorDislike.value(dislikeVal.value);
            }

            //DISABLE DISLIKE PALLETE ON LOAD
            if ($("#chkDislike").is(':checked') == false) {
                colorDislike.enable(false);
            }

            //KENDO TEXT EDITOR
            var input = $("#qtext").val();
            ;




            //QUESTION TEXT EDITOR
            $("#qtext").kendoEditor({


                //execute: function (e) {
                //    var widget = this;
                //    if (e.name == "insertImage") {
                //        var command = e.command;
                //        setTimeOut(function(command){
                //            var form = command._dialog.element;
                //            var insertButton = form.find(".k-dialog-insert");

                //            //remove original events
                //            insertButton.off();

                //            //initialize Kendo UI validator
                //            form.kendoValidator(validatorOptions);

                //            insertButton.on("click", $.proxy(function (e) {
                //                alert('test');
                //                if (form.data("kendoValidator").validate()) {
                //                    //if validation pass close the editor command:
                //                    this._apply(e);
                //                } else {
                //                    e.preventDefault();
                //                }
                //            }, command));

                //        }, 0, command)
                //    }
                //   }
                    
                //,
                pasteCleanup: {
                    custom: function (html) {
                        return html.replace(/<img[^>]*>/, "");
                    }
                },
                tools: [
                 "bold",
                "italic",
                "underline",
                "strikethrough",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "insertUnorderedList",
                "insertOrderedList",
                "indent",
                "outdent",
                "createLink",
                "unlink",
                "insertImage",
                "insertFile",
                "subscript",
                "superscript",
                "tableWizard",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "formatting",
                "cleanFormatting",
                "fontName",
                "fontSize",
                "foreColor",
                "backColor",

                ]
            });

            //INSTRUCTION TEXT EDITOR
            $("#instruction_header").kendoEditor({
                pasteCleanup: {
                    custom: function (html) {
                        return html.replace(/<img[^>]*>/, "");
                    }
                },
                tools: [
                 "bold",
                "italic",
                "underline",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "insertUnorderedList",
                "insertOrderedList",
                "tableWizard",
                "createTable",
                "addRowAbove",
                "addRowBelow",
                "addColumnLeft",
                "addColumnRight",
                "deleteRow",
                "deleteColumn",
                "viewHtml",
                "foreColor",
                "backColor",
                "formatting",
                "cleanFormatting",
                "fontName",
                "fontSize",


                ]

            });

            //var validatorOptions = {
            //    messages: {
            //        customUrl: "The URL should be set to 'sometext'.",
            //    },
            //    rules: {
            //        customUrl: function (input) {
            //            if (input.is("#k-editor-link-url")) {

            //                if (input.val() != "sometext") {
            //                    input.attr("name", "k-editor-link-url");
            //                    return false;
            //                }
            //            }
            //            return true;
            //        }
            //    }
            //};
        });



    </script>
</body>
</html>
