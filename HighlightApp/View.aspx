<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="HighlightApp.View" MasterPageFile="Site.Master" ValidateRequest="false"%>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <hr />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <%--QUESTION CONTROLS--%>
                    <div class="col-md-2">
                        <asp:LinkButton ID="btnAddQuestion" runat="server" OnClick="btnAddQuestion_Click" CssClass="btn btn-primary" Width="23%" ToolTip="Add Question"><span   class="glyphicon glyphicon-plus  btn-question"></span></asp:LinkButton>
                        <asp:LinkButton ID="btnEditQuestion" runat="server" OnClick="btnEditQuestion_Click" CssClass="btn btn-primary" Width="23%" Enabled="False" ToolTip="Edit Question"><span   class="glyphicon glyphicon-edit  btn-question"></span></asp:LinkButton>
                        <asp:LinkButton ID="btnDelQuestion" runat="server" OnClick="btnDelQuestion_Click" CssClass="btn btn-primary" Width="23%" ToolTip="Delete Question"><span   class="glyphicon glyphicon-minus btn-question"></span></asp:LinkButton>
                        <asp:LinkButton ID="btnCodeSnippet" runat="server" OnClick="btnCodeSnippet_Click" CssClass="btn btn-primary btn-question" Width="23%" Font-Bold="True" ToolTip="Show Code Snippet">&lt;&#x2F;&gt;</asp:LinkButton>
                    </div>
                    <div class="col-md-4">
                        <%--STUDY SELECTION--%>
                        <asp:DropDownList ID="ddlStudy" runat="server" CssClass="form-control input-block optionddl" AutoPostBack="true" OnSelectedIndexChanged="ddlStudy_SelectedIndexChanged" ToolTip="Select Study"></asp:DropDownList>
                    </div>
                    <div class="col-md-6 bs-component">
                        <%--STUDY CONTROLS--%>
                        <div class="btn-group btn-group-justified nav-study" style="text-align: right;">
                            <asp:LinkButton ID="btnNewStudy" runat="server" OnClick="btnNewStudy_Click" CssClass="btn btn-default " Width="30%" ToolTip="Create New Study"><span >Create New Study</span></asp:LinkButton>
                            <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" Enabled="False" CssClass="btn btn-default btn-study" Width="30%" ToolTip="Edit Study Name"><span>Edit Study Name</span></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Enabled="False" CssClass="btn btn-default btn-study " Width="30%" ToolTip="Delete Study"><span >Delete Study</span></asp:LinkButton>
                            <asp:LinkButton ID="btnHelp" runat="server" OnClick="btnHelp_Click" CssClass="btn btn-info btn-study " Width="10%" ToolTip="Help Section"><span >Help?</span></asp:LinkButton>
                        </div>
                    </div>
                </div>



                <div class="row">
                    <div class="col-md-2 left-pane">
                        <%--QUESTION LIST--%>
                        <asp:ListBox class="form-control optionlb" ID="listQuestion" runat="server" AutoPostBack="true" Rows="25" Height="480" Width="100%" OnSelectedIndexChanged="listQuestion_SelectedIndexChanged" ToolTip="Question List"></asp:ListBox>
                    </div>
                    <div class="col-md-10">
                        <iframe runat="server" id="frameQ" frameborder="1" style="width: 97%; position: absolute;" scrolling="yes" height="480"></iframe>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
     
        <%--NEW STUDY MODAL TOGGLE--%>
        <div id="modalNew" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <asp:UpdatePanel ID="ModalPanel" runat="server" EnableViewState="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 id="modaltitle" class="modal-title" runat="server">Create new study name.</h4>
                            </div>
                            <div class="modal-body" >
                                <div class="form-group" >
                                    <label class="control-label col-md-4" for="txtNewStudy" style="margin-top:7px; text-align: center;">Study Name</label>
                                    
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtNewStudy" runat="server" CssClass="form-control" placeholder="eg. SAWZ1701SSI"></asp:TextBox>
                                    </div>
                                    
                                </div>
                                <div class="col-md-4"></div>
                                    <div class="col-md-8">
                                        <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="#CC0000"></asp:Label>
                                    </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="btnAddStudy" runat="server" OnClick="btnAddStudy_Click" CssClass="btn btn-success">Save</asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Modal -->
            </div>
        </div>
    </div>

    <%--DELETE STUDY MODAL TOGGLE--%>
    <div class="modal fade" id="confirmdelete" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="deletePanel" runat="server" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h3 id="delStudy" class="modal-title" runat="server">Delete Study</h3>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="Label1" runat="server" Text="Label">Are you sure you want to delete this study and all of its questions and highlighted response?</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnDelStudy" OnClick="btnDelStudy_Click" runat="server" CssClass="btn btn-danger btn-ok" Text="Delete" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Modal -->
        </div>
    </div>

    <%--CONFIRM DELETE POP UP MODAL--%>
    <div class="modal fade" id="confirmdeletequestion" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h3 id="H1" class="modal-title" runat="server">Delete Question</h3>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Label ID="Label2" runat="server" Text="Label">Are you sure you want to delete this question including all of its highlighted response?</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <asp:Button ID="btnOKDeleteQuestion" OnClick="btnOKDeleteQuestion_Click" runat="server" CssClass="btn btn-danger btn-ok" Text="Delete" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Modal -->
        </div>
    </div>

    <!-- CODE SNIPPET MODAL TOGGLE-->
    <div id="showCodeSnippet" class="modal fade"  tabindex="-1" role="dialog" aria-labelledby="myModal">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="modalSnippet" runat="server" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" >&times;</button>
                            <h3 id="H2" class="modal-title" runat="server">Code Snippet for Free Format Question</h3>
                        </div>
                        <div class="modal-body" style="margin-bottom:680px !important;">

                            <div class="form-group">
                                   
                                <div class="col-md-12">

                                    <span>Fields Count:</span>
                                    <asp:TextBox ID="txtFields" runat="server" TextMode="Number" Height="100%" Width="10%"></asp:TextBox>
                                    <h3>Header 2</h3>
                                    <asp:TextBox ID="txtHeader" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox>
                                    <h3>Footer</h3>
                                    <h4>Sawtooth 8</h4>
                                     <asp:TextBox ID="txtFooter8" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox>
                                    <h4>Sawtooth 7</h4>
                                    <asp:TextBox ID="txtFooter7" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox>
                                    <h3>Variables</h3>
                                    <asp:TextBox ID="txtVariables" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox>
                                    <h3>Javacript Verification (Optional)</h3>
                                    <h4>Sawtooth 8</h4>
                                    <asp:TextBox ID="txtVerif8" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox>
                                    <h4>Sawtooth 7</h4>
                                    <asp:TextBox ID="txtVerif7" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Modal -->
        </div>
    </div>

    <!-- ERROR CODE MODAL TOGGLE -->
    <div class="modal fade" id="errorPopUp" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="modalError" runat="server" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h3 id="ErrorTitle" class="modal-title" runat="server">Error</h3>
                        </div>
                        <div class="modal-body">
                         

                            <div class="form-group">
                                <div class="col-md-12">
                                    <h5 style="text-align: center; color: red">No Highlight Fields Found. Please contact administrator.</h5>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer" style="outline:none !important; ">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Modal -->
        </div>
    </div>

    <!-- HELP CODE MODAL TOGGLE -->
    <div class="modal fade" id="helpPopUp" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="modalHelp" runat="server" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h3 id="H3" class="modal-title" runat="server">Help</h3>
                        </div>
                        <div class="modal-body">
                            <asp:Image ID="PageImage" runat="server" ImageUrl="~/Content/default/Page1.png" />
                        </div>
                        <div class="modal-footer">
                            <ul class="pagination">
                                <li>
                                    <asp:LinkButton ID="help1" OnClick="Help" runat="server">1</asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="help2" OnClick="Help" runat="server">2</asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="help3" OnClick="Help" runat="server">3</asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="help4" OnClick="Help" runat="server">4</asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="help5" OnClick="Help" runat="server">5</asp:LinkButton></li>
                            </ul>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Modal -->
        </div>
    </div>





</asp:Content>
