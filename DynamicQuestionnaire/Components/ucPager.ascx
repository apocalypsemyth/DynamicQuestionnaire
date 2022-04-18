<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPager.ascx.cs" Inherits="DynamicQuestionnaire.Components.ucPager" %>

<div class="Pager">
    <a id="aLinkFirst" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=1">首頁</a>
    <a id="aLinkPrev" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=1">上一頁</a>

    <a id="aLinkPage1" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=1">1</a>
    <a id="aLinkPage2" class="text-decoration-none" runat="server" href="">2</a>
    <a id="aLinkPage3" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=3">3</a>
    <a id="aLinkPage4" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=4">4</a>
    <a id="aLinkPage5" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=5">5</a>

    <a id="aLinkNext" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=3">下一頁</a>
    <a id="aLinkLast" class="text-decoration-none" runat="server" href="QuestionnaireList.aspx?Index=10">末頁</a>
</div>
