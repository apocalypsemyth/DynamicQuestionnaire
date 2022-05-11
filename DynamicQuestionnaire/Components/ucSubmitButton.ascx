<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSubmitButton.ascx.cs" Inherits="DynamicQuestionnaire.Components.ucSubmitButton" %>
<script src="../Scripts/jquery.min.js"></script>
<script src="../JavaScript/BackAdmin/Funcs/QuestionnaireDetailFunc.js"></script>

<asp:Button ID="btnSubmit" runat="server" Text="送出" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />