<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CheckingQuestionnaireDetail.aspx.cs" Inherits="DynamicQuestionnaire.CheckingQuestionnaireDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="checkingQuestionnaireDetailContainer">
        <div class="row align-items-center justify-content-center gy-3 gy-md-5">
            <div class="col-md-10">
                <asp:Repeater ID="rptCheckingQuestionnaireDetail" runat="server">
                    <ItemTemplate>
                        <div class="d-flex flex-column align-items-end justify-content-center mb-3 mb-md-5">
                            <h4>
                                <asp:Literal 
                                ID="ltlIsEnable" 
                                runat="server" 
                                Text='<%# (bool)Eval("IsEnable")
                                    ? "投票中" 
                                    : "已結束" %>' 
                                />
                            </h4>

                            <h4>
                                <asp:Literal 
                                    ID="ltlStartAndEndDate" 
                                    runat="server" 
                                    Text='<%# DateTime.Parse(Eval("StartDate").ToString())
                                        .ToShortDateString() + 
                                        " ~ " + 
                                        (!string.IsNullOrWhiteSpace(Eval("EndDate")?.ToString())
                                        ? DateTime.Parse(Eval("EndDate").ToString()).ToShortDateString()
                                        : "未知") %>' 
                                />
                            </h4>
                        </div>

                        <div class="d-flex flex-column align-items-center justify-content-center mb-3 mb-md-5">
                            <h1>
                                <asp:Literal ID="ltlCaption" runat="server" Text='<%# Eval("Caption") %>' />
                            </h1>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="col-md-9">
                <div id="checkingQuestionnaireUserForm">
                    <div class="row mb-3">
                        <label class="col-sm-2 col-form-label">姓名</label>
                        <div class="col-sm-10 align-self-center">
                            <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label class="col-sm-2 col-form-label">手機</label>
                        <div class="col-sm-10 align-self-center">
                            <asp:Literal ID="ltlUserPhone" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label class="col-sm-2 col-form-label">Email</label>
                        <div class="col-sm-10 align-self-center">
                            <asp:Literal ID="ltlUserEmail" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <label class="col-sm-2 col-form-label">年齡</label>
                        <div class="col-sm-10 align-self-center">
                            <asp:Literal ID="ltlUserAge" runat="server"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-9">
                <div id="divCheckingQuestionList" class="row align-items-center justify-content-center gy-3 gy-md-5">
                    <asp:Repeater ID="rptCheckingQuestionList" runat="server" OnPreRender="rptCheckingQuestionList_PreRender">
                        <ItemTemplate>
                            <div class="col-12">
                                <div class="d-flex flex-column">
                                    <h3>
                                        <asp:Literal ID="ltlQuestionName" runat="server" Text='<%#(Container.ItemIndex + 1).ToString() + ". " + Eval("QuestionName") %>' />
                                    </h3>

                                    <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />
                                    <asp:Literal ID="ltlQuestionAnswer" runat="server" Text='<%# Eval("QuestionAnswer") %>' />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="col-md-10">
                <div class="d-flex align-item-center justify-content-end gap-1">
                    <asp:Button ID="btnEdit" CssClass="btn btn-secondary" runat="server" Text="修改" />
                    <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" Text="送出" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
