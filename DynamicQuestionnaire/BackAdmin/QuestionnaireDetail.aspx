<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/QuestionnaireDetailMaster.master" AutoEventWireup="true" CodeBehind="QuestionnaireDetail.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.QuestionnaireDetail1" %>

<%@ Register Src="~/Components/ucCancelButton.ascx" TagPrefix="uc1" TagName="ucCancelButton" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaire" class="tab-pane show active">
        <div class="row gy-3">
            <div class="col-md-8 gy-3">
                <div class="row">
                    <label for='<%= this.txtCaption.ClientID %>' class="col-sm-2 col-form-label">問卷名稱：</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtCaption" CssClass="form-control" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <label for='<%= this.txtDescription.ClientID %>' class="col-sm-2 col-form-label">描述內容：</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5" />
                    </div>
                </div>
        
                <div class="row">
                    <label for='<%= this.txtStartDate.ClientID %>' class="col-sm-2 col-form-label">開始時間：</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" />
                    </div>
                </div>
        
                <div class="row">
                    <label for='<%= this.txtEndDate.ClientID %>' class="col-sm-2 col-form-label">結束時間：</label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <label for='<%= this.ckbIsEnable.ClientID %>'>
                    <asp:CheckBox ID="ckbIsEnable" runat="server" />
                    已啟用
                </label>
            </div>

            <div class="col-md-8">
                <div class="d-flex justify-content-end">
                    <uc1:ucCancelButton runat="server" ID="ucCancelButtonInQuestionnaireTab" />
                    <asp:Button ID="btnSubmitInQuestionnaireTab" runat="server" Text="送出" OnClick="btnSubmitInQuestionnaireTab_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="question" class="tab-pane">
        <div class="row gy-3">
            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.ddlCategoryList.ClientID %>' class="col-sm-2 col-form-label">種類：</label>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddlCategoryList" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtQuestionName.ClientID %>' class="col-sm-2 col-form-label">問題：</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtQuestionName" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlTypingList" runat="server" />
                    </div>
                    <div class="col-sm-3">
                        <label for='<%= this.ckbQuestionRequired.ClientID %>'>
                            <asp:CheckBox ID="ckbQuestionRequired" runat="server" />
                            必選
                        </label>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtQuestionAnswer.ClientID %>' class="col-sm-2 col-form-label">回答：</label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtQuestionAnswer" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-6">
                        <button id="btnAddQuestion">加入</button>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <button id="btnDeleteQuestion">刪除</button>
            </div>

            <div class="col-md-8">
                <div id="divQuestionListContainer"></div>
            </div>

            <div class="col-md-8">
                <div class="d-flex justify-content-end">
                    <uc1:ucCancelButton runat="server" ID="ucCancelButtonInQuestionTab" />
                    <asp:Button ID="btnSubmitInQuestionTab" runat="server" Text="送出" OnClick="btnSubmitInQuestionTab_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="question-info" class="tab-pane">
        <asp:Button ID="btnExportAndDownloadDataToCSV" runat="server" Text="匯出" OnClick="btnExportAndDownloadDataToCSV_Click" />

        <div id="divUserListContainer"></div>

        <div id="divUserListPagerContainer"></div>

        <div id="divUserAnswerContainer"></div>
    </div>

    <div id="statistics" class="tab-pane">
        <div id="divStatisticsContainer"></div>
    </div>
</asp:Content>
