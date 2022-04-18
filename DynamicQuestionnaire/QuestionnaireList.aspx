<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="QuestionnaireList.aspx.cs" Inherits="DynamicQuestionnaire.QuestionnaireList" %>

<%@ Register Src="~/Components/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaireListContainer">
        <div class="row align-items-center justify-content-center gy-3">
            <div class="col-md-10">
                <div id="questionnaireSearch">
                    問卷標題：<asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>

                    <div>
                        開始 / 結束：<asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearchQuestionnaire" runat="server" Text="搜尋" OnClick="btnSearchQuestionnaire_Click" />
                    </div>
                </div>
            </div>

            <div class="col-md-10">
                <asp:GridView ID="gvQuestionnaireList" CssClass="table table-bordered w-auto" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="QuestionnaireID" HeaderText="問卷代碼" />

                        <asp:TemplateField HeaderText="問卷">
                            <ItemTemplate>
                                <a href="QuestionnaireDetail.aspx?ID=<%# Eval("QuestionnaireID") %>"><%# Eval("Caption") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="IsEnable" HeaderText="狀態" />
                        <asp:BoundField DataField="StartDate" HeaderText="開始時間" DataFormatString="{0:D}" />
                        <asp:BoundField DataField="EndDate" HeaderText="結束時間" DataFormatString="{0:D}" NullDisplayText="---" />

                        <asp:TemplateField HeaderText="觀看統計">
                            <ItemTemplate>
                                <a href="QuestionnaireDetail.aspx?ID=<%# Eval("QuestionnaireID") %>">前往</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:PlaceHolder ID="plcEmpty" runat="server" Visible="false">
                    <p>尚未有資料 </p>
                </asp:PlaceHolder>
            </div>

            <div class="col-md-10">
                <uc1:ucPager runat="server" ID="ucPager" PageSize="10" />
            </div>
        </div>
    </div>
</asp:Content>
