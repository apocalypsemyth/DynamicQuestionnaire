<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="QuestionnaireDetail.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.QuestionnaireDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="backAdminQuestionnaireDetailContainer">
        <div class="row align-items-center justify-content-center gy-3">
            <div class="col-md-10">
                <ul id="ulQuestionnaireDetailTabs" class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link active" data-bs-toggle="tab" href="#questionnaire">問卷</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#question">問題</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#question-info">填寫資料</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="tab" href="#statistics">統計</a>
                    </li>
                </ul>

                <div class="tab-content">
                    <div id="questionnaire" class="tab-pane show active">
                        問卷名稱：<asp:TextBox ID="txtCaption" runat="server"></asp:TextBox><br />
                        描述內容：<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox><br />
                        開始時間：<asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox><br />
                        結束時間：<asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox><br />

                        <asp:CheckBox ID="ckbIsEnable" runat="server" />已啟用<br />
                    </div>

                    <div id="question" class="tab-pane">
                        種類：<asp:DropDownList ID="ddlCategoryList" runat="server"></asp:DropDownList><br />

                        <div class="d-flex">
                            問題：<asp:TextBox ID="txtQuestionName" runat="server"></asp:TextBox>
                            <asp:DropDownList ID="ddlTypingList" runat="server"></asp:DropDownList>
                            <asp:CheckBox ID="ckbQuestionRequired" runat="server" />必選
                        </div>

                        <div class="d-flex">
                            回答：<asp:TextBox ID="txtQuestionAnswer" runat="server"></asp:TextBox>
                            <button id="btnAddQuestion">加入</button>
                        </div>

                        <button id="btnDeleteQuestion">刪除</button>

                        <div id="divGvQuestionListContainer"></div>
                    </div>
                </div>
            </div>

            <div class="col-md-10">
                <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>
</asp:Content>
