<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/QuestionnaireDetailMaster.master" AutoEventWireup="true" CodeBehind="QuestionnaireDetail.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.QuestionnaireDetail1" %>

<%@ Register Src="~/Components/ucCancelButton.ascx" TagPrefix="uc1" TagName="ucCancelButton" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaire" class="tab-pane show active">
        <div class="row gy-3 p-3">
            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtCaption.ClientID %>' class="col-sm-2 col-form-label">
                        問卷名稱：
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtCaption" CssClass="form-control" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtDescription.ClientID %>' class="col-sm-2 col-form-label">
                        描述內容：
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="5" />
                    </div>
                </div>
            </div>
            
            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtStartDate.ClientID %>' class="col-sm-2 col-form-label">
                        開始時間：
                    </label>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" />
                    </div>
                </div>
            </div>
            
            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtEndDate.ClientID %>' class="col-sm-2 col-form-label">
                        結束時間：
                    </label>
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
                <div class="d-flex align-items-center justify-content-end gap-1">
                    <uc1:ucCancelButton ID="ucCancelButtonInQuestionnaireTab" runat="server" />
                    <asp:Button ID="btnSubmitInQuestionnaireTab" CssClass="btn btn-primary" runat="server" Text="送出" OnClick="btnSubmitInQuestionnaireTab_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="question" class="tab-pane">
        <div class="row gy-3 p-3">
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
                    <label for='<%= this.txtQuestionName.ClientID %>' class="col-sm-2 col-form-label">
                        問題：
                    </label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtQuestionName" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-6 align-self-center">
                        <div class="d-flex align-items-center gap-3">
                            <asp:DropDownList ID="ddlTypingList" runat="server" />
                            <label for='<%= this.ckbQuestionRequired.ClientID %>'>
                                <asp:CheckBox ID="ckbQuestionRequired" runat="server" />
                                必選
                            </label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtQuestionAnswer.ClientID %>' class="col-sm-2 col-form-label">
                        回答：
                    </label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtQuestionAnswer" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-6 align-self-center">
                        <div class="d-flex align-items-center gap-3">
                            <div class="d-flex align-items-center justify-content-center">
                                <h5>(多個答案以 <b>;</b> 分隔)</h5>
                            </div>
                            <button id="btnAddQuestion" class="btn btn-success">加入</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <button id="btnDeleteQuestion" class="btn btn-danger mt-md-5">刪除</button>
            </div>

            <div class="col-md-8">
                <div id="divQuestionListContainer"></div>
            </div>

            <div class="col-md-8">
                <div class="d-flex align-items-center justify-content-end gap-1">
                    <uc1:ucCancelButton ID="ucCancelButtonInQuestionTab" runat="server" />
                    <asp:Button ID="btnSubmitInQuestionTab" CssClass="btn btn-primary" runat="server" Text="送出" OnClick="btnSubmitInQuestionTab_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="question-info" class="tab-pane">
        <div class="d-flex flex-column gap-3 p-3">
            <div id="btnExportAndDownloadDataToCSVContainer" class="w-auto">
                <asp:Button ID="btnExportAndDownloadDataToCSV" CssClass="btn btn-info" runat="server" Text="匯出" OnClick="btnExportAndDownloadDataToCSV_Click" />
            </div>

            <div id="divUserListContainer"></div>

            <div id="divUserListPagerContainer"></div>

            <div id="divUserAnswerContainer"></div>
        </div>
    </div>

    <div id="statistics" class="tab-pane">
        <div class="p-3">
            <div id="divStatisticsContainer"></div>
        </div>
    </div>
</asp:Content>
