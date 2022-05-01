<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="QuestionnaireList.aspx.cs" Inherits="DynamicQuestionnaire.QuestionnaireList" %>

<%@ Register Src="~/Components/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="questionnaireListContainer">
        <div class="row align-items-center justify-content-center gy-3">
            <div class="col-md-8">
                <div class="row">
                    <label for='<%= this.txtKeyword.ClientID %>' class="col-sm-2 col-form-label">
                        問卷標題：
                    </label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtKeyword" CssClass="form-control" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="row mb-md-5">
                    <label for='<%= this.txtStartDate.ClientID %>' class="col-sm-2 col-form-label">
                        開始 / 結束：
                    </label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtEndDate" CssClass="form-control" runat="server" />
                    </div>
                    <div class="col-sm-2 align-self-center">
                        <asp:Button ID="btnSearchQuestionnaire" CssClass="btn btn-primary" runat="server" Text="搜尋" OnClick="btnSearchQuestionnaire_Click" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <asp:GridView ID="gvQuestionnaireList" CssClass="table table-bordered w-auto" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="問卷代碼">
                            <ItemTemplate>
                                <asp:Literal ID="ltlQuestionListNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

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
                                <a href="QuestionnaireStatistics.aspx?ID=<%# Eval("QuestionnaireID") %>">前往</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:PlaceHolder ID="plcEmpty" runat="server" Visible="false">
                    <p>尚未有資料 </p>
                </asp:PlaceHolder>
            </div>

            <div class="col-md-8">
                <uc1:ucPager runat="server" ID="ucPager" PageSize="10" />
            </div>
        </div>
    </div>
</asp:Content>
