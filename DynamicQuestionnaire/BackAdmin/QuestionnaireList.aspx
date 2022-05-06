<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="QuestionnaireList.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.QuestionnaireList" %>

<%@ Register Src="~/Components/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="backAdminQuestionnaireListContainer">
        <div class="row gy-3">
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
                <div class="row">
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
                <div class="d-flex gap-1 mt-3 mt-md-5">
                    <asp:Button ID="btnDeleteQuestionnaire" CssClass="btn btn-danger" runat="server" Text="刪除" OnClick="btnDeleteQuestionnaire_Click" />
                    <asp:Button ID="btnCreateQuestionnaire" CssClass="btn btn-success" runat="server" Text="新增" OnClick="btnCreateQuestionnaire_Click" />
                </div>
            </div>

            <div class="col-md-10">
                <asp:GridView ID="gvQuestionnaireList" CssClass="table table-bordered w-auto" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvQuestionnaireList_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbDeleteQuestionnaire" runat="server" />
                                <asp:HiddenField ID="hfQuestionnaireID" runat="server" Value='<%# Eval("QuestionnaireID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Literal ID="ltlQuestionListNumber" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="問卷">
                            <ItemTemplate>
                                <a href="QuestionnaireDetail.aspx?ID=<%# Eval("QuestionnaireID") %>"><%# Eval("Caption") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="狀態">
                            <ItemTemplate>
                                <asp:Literal ID="ltlIsEnable" Text='<%# 
                                    (bool)Eval("IsEnable") 
                                    ? "開放" 
                                    : "已關閉" %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="StartDate" HeaderText="開始時間" DataFormatString="{0:D}" />
                        <asp:BoundField DataField="EndDate" HeaderText="結束時間" DataFormatString="{0:D}" nulldisplaytext="---" />

                        <asp:TemplateField HeaderText="觀看統計">
                            <ItemTemplate>
                                <a id="aLinkQuestionnaireDetailStatistics" href="QuestionnaireDetail.aspx?ID=<%# Eval("QuestionnaireID") %>" onclick="SetActiveTabOfQuestionnaireDetailSession();">前往</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:PlaceHolder ID="plcEmpty" runat="server" Visible="false">
                    <p>尚未有資料 </p>
                </asp:PlaceHolder>
            </div>

            <div class="col-md-8">
                <uc1:ucPager runat="server" id="ucPager" PageSize="10" />
            </div>
        </div>
    </div>
</asp:Content>
