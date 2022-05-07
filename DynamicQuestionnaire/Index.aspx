<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DynamicQuestionnaire.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>前台 - 問卷填寫起始頁面</title>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="row align-items-center justify-content-center g-0 gy-3 mt-md-5">
            <div class="col-md-10">
                <div class="d-flex align-items-center justify-content-center">
                    <h1>填寫問卷</h1>
                </div>
            </div>

            <div class="col-md-10">
                <div class="d-flex align-items-center justify-content-center">
                    <asp:Button ID="btnGoToQuestionnaireList" CssClass="btn btn-info" runat="server" Text="前往" OnClick="btnGoToQuestionnaireList_Click" />
                </div>
            </div>
        </div>
    </form>

    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
