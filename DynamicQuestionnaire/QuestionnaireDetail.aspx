<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="QuestionnaireDetail.aspx.cs" Inherits="DynamicQuestionnaire.QuestionnaireDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaireDetailContainer">
        <div class="row align-items-center justify-content-center gy-3">
            <div class="col-md-10">
                <asp:Repeater ID="rptQuestionnaireDetail" runat="server">
                    <ItemTemplate>
                        <div class="d-flex flex-column align-items-end justify-content-center">
                            <h4>
                                <asp:Literal
                                    ID="ltlIsEnable"
                                    runat="server"
                                    Text='<%# (bool)Eval("IsEnable")
                                    ? "投票中" 
                                    : "已結束" %>' />
                            </h4>

                            <h4>
                                <asp:Literal
                                    ID="ltlStartAndEndDate"
                                    runat="server"
                                    Text='<%# Eval("StartDate") + 
                                        "~" + 
                                        (!string.IsNullOrWhiteSpace(Eval("EndDate") as string) 
                                        ? Eval("EndDate") 
                                        : "未知") %>' />
                            </h4>
                        </div>

                        <div class="d-flex flex-column align-items-center justify-content-center">
                            <h1>
                                <asp:Literal ID="ltlCaption" runat="server" Text='<%# Eval("Caption") %>' />
                            </h1>

                            <h2>
                                <asp:Literal ID="ltlDescription" runat="server" Text='<%# Eval("Description") %>' />
                            </h2>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="col-md-10">
                <div id="questionnaireUserForm" runat="server">
                    <div class="row mb-3">
                        <label for='<%= this.txtUserName.ClientID %>' class="col-sm-2 col-form-label">
                            姓名
                        </label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtUserName" CssClass="form-control" aria-describedby="divValidateUserName" runat="server" />
                            <div id="divValidateUserName" class="invalid-feedback"></div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label for='<%= this.txtUserPhone.ClientID %>' class="col-sm-2 col-form-label">
                            手機
                        </label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtUserPhone" CssClass="form-control" aria-describedby="divValidateUserPhone" runat="server" />
                            <div id="divValidateUserPhone" class="invalid-feedback"></div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label for='<%= this.txtUserEmail.ClientID %>' class="col-sm-2 col-form-label">
                            Email
                        </label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtUserEmail" CssClass="form-control" aria-describedby="divValidateUserEmail" runat="server" />
                            <div id="divValidateUserEmail" class="invalid-feedback"></div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label for='<%= this.txtUserAge.ClientID %>' class="col-sm-2 col-form-label">
                            年齡
                        </label>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtUserAge" CssClass="form-control" aria-describedby="divValidateUserAge" runat="server" />
                            <div id="divValidateUserAge" class="invalid-feedback"></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-10">
                <div class="row align-items-center justify-content-center gy-3">
                    <asp:Repeater ID="rptQuestionList" runat="server" OnPreRender="rptQuestionList_PreRender">
                        <ItemTemplate>
                            <div class="col-11 col-md-10">
                                <div class="d-flex flex-column">
                                    <h3>
                                        <asp:Literal ID="ltlQuestionName" runat="server" Text='<%#(Container.ItemIndex + 1).ToString() + ". " + Eval("QuestionName") + ((bool)Eval("QuestionRequired") ? " (必填)" : null) %>' />
                                    </h3>

                                    <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />
                                    <asp:HiddenField ID="hfQuestionTyping" runat="server" Value='<%# Eval("QuestionTyping") %>' />
                                    <asp:Literal ID="ltlQuestionAnswer" runat="server" Text='<%# Eval("QuestionAnswer") %>' />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="col-md-10">
                <div class="d-flex align-item-center justify-content-end">
                    <asp:Button ID="btnCancel" CssClass="btn btn-secondary" runat="server" Text="取消" OnClick="btnCancel_Click" />
                    <a id="aLinkCheckingQuestionnaireDetail" class="btn btn-success" runat="server">
                        送出
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
