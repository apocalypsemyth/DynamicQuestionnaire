<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/QuestionnaireDetailMaster.master" AutoEventWireup="true" CodeBehind="QuestionnaireDetail.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.QuestionnaireDetail1" %>

<%@ Register Src="~/Components/ucCancelButton.ascx" TagPrefix="uc1" TagName="ucCancelButton" %>
<%@ Register Src="~/Components/ucSubmitButton.ascx" TagPrefix="uc1" TagName="ucSubmitButton" %>
<%@ Register Src="~/Components/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaire" class="tab-pane show active">
        問卷名稱：<asp:TextBox ID="txtCaption" runat="server"></asp:TextBox><br />
        描述內容：<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox><br />
        開始時間：<asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox><br />
        結束時間：<asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox><br />

        <asp:CheckBox ID="ckbIsEnable" runat="server" />已啟用<br />
        <uc1:ucCancelButton runat="server" ID="ucCancelButtonInQuestionnaireTab" />
        <uc1:ucSubmitButton runat="server" ID="ucSubmitButtonInQuestionnaireTab" />
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

        <div id="divQuestionListContainer"></div>
        <uc1:ucCancelButton runat="server" ID="ucCancelButtonInQuestionTab" />
        <uc1:ucSubmitButton runat="server" ID="ucSubmitButtonInQuestionTab" />
    </div>

    <div id="question-info" class="tab-pane">
        <asp:Button ID="btnExportAndDownloadDataToCSV" runat="server" Text="匯出" OnClick="btnExportAndDownloadDataToCSV_Click" />

        <div id="divUserListContainer"></div>

        <div id="divUserListPagerContainer"></div>

        <div id="divUserAnswerContainer"></div>
    </div>

    <div id="statistics" class="tab-pane">
        統計分頁
    </div>
</asp:Content>
