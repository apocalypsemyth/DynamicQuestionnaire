<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="QuestionnaireStatistics.aspx.cs" Inherits="DynamicQuestionnaire.QuestionnaireStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaireStatisticsContainer">
        <div class="row align-items-center justify-content-center gy-3">
            <div class="col-md-10">
                <div class="d-flex align-items-center justify-content-center">
                    <h1>
                        <asp:Literal ID="ltlCaption" runat="server" />
                    </h1>
                </div>
            </div>

            <div class="col-md-10">
                <div id="divQuestionnaireStatisticsContainer"></div>
            </div>
        </div>
    </div>
</asp:Content>
