<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="CommonQuestionDetail.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.CommonQuestionDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="backAdminCommonQuestionDetailContainer">
        <div class="row gy-3">
            <div class="col-md-8">
                <div class="row mb-3">
                    <label for='<%= this.txtCommonQuestionName.ClientID %>' class="col-sm-2 col-form-label">
                        常用問題名稱：
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtCommonQuestionName" CssClass="form-control" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <label for='<%= this.ddlCategoryList.ClientID %>' class="col-sm-2 col-form-label">
                        種類：
                    </label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlCategoryList" runat="server" />
                    </div>
                </div>

                <div class="row mb-3">
                    <label for='<%= this.txtQuestionNameOfCommonQuestion.ClientID %>' class="col-sm-2 col-form-label">
                        問題：
                    </label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtQuestionNameOfCommonQuestion" CssClass="form-control" runat="server" />
                    </div>

                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlTypingList" runat="server" />
                    </div>

                    <div class="col-sm-2">
                        <label for='<%= this.ckbQuestionRequiredOfCommonQuestion.ClientID %>'>
                            <asp:CheckBox ID="ckbQuestionRequiredOfCommonQuestion" runat="server" />
                            必選
                        </label>
                    </div>
                </div>

                <div class="row mb-3">
                    <label for='<%= this.txtQuestionAnswerOfCommonQuestion.ClientID %>' class="col-sm-2 col-form-label">
                        回答：
                    </label>
                    <div class="col-sm-5">
                        <asp:TextBox ID="txtQuestionAnswerOfCommonQuestion" CssClass="form-control" runat="server" />
                    </div>

                    <div class="col-sm-5">
                        <button id="btnAddQuestionOfCommonQuestion">加入</button>
                    </div>
                </div>
            </div>

            <div class="col-md-10">
                <button id="btnDeleteQuestionOfCommonQuestion">刪除</button>
            </div>

            <div class="col-md-10">
                <div id="divQuestionListOfCommonQuestionContainer"></div>
            </div>

            <div class="col-md-10">
                <div class="d-flex">
                    <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>