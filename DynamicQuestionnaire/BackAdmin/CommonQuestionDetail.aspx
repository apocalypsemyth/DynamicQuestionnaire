<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="CommonQuestionDetail.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.CommonQuestionDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="backAdminCommonQuestionDetailContainer">
        <div class="row gy-3">
            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtCommonQuestionName.ClientID %>' class="col-sm-2 col-form-label">
                        常用問題名稱：
                    </label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtCommonQuestionName" CssClass="form-control" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.ddlCategoryList.ClientID %>' class="col-sm-2 col-form-label">
                        種類：
                    </label>
                    <div class="col-sm-10 align-self-center">
                        <asp:DropDownList ID="ddlCategoryList" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtQuestionNameOfCommonQuestion.ClientID %>' class="col-sm-2 col-form-label">
                        問題：
                    </label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtQuestionNameOfCommonQuestion" CssClass="form-control" runat="server" />
                    </div>

                    <div class="col-sm-5 align-self-center">
                        <div class="d-flex align-items-center gap-3">
                            <asp:DropDownList ID="ddlTypingList" runat="server" />
                            <label for='<%= this.ckbQuestionRequiredOfCommonQuestion.ClientID %>'>
                                <asp:CheckBox ID="ckbQuestionRequiredOfCommonQuestion" runat="server" />
                                必選
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtQuestionAnswerOfCommonQuestion.ClientID %>' class="col-sm-2 col-form-label">
                        回答：
                    </label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtQuestionAnswerOfCommonQuestion" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-5 align-self-center">
                        <div class="d-flex align-items-center gap-3">
                            <div class="d-flex align-items-center justify-content-center">
                                <h5>(多個答案以 <b>;</b> 分隔)</h5>
                            </div>
                            <button id="btnAddQuestionOfCommonQuestion" class="btn btn-success">加入</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-10">
                <button id="btnDeleteQuestionOfCommonQuestion" class="btn btn-danger mt-md-5">刪除</button>
            </div>

            <div class="col-md-10">
                <div id="divQuestionListOfCommonQuestionContainer"></div>
            </div>

            <div class="col-md-6">
                <div class="d-flex align-items-center justify-content-end gap-1">
                    <asp:Button ID="btnCancel" CssClass="btn btn-secondary" runat="server" Text="取消" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="送出" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>