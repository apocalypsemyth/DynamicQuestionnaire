<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="CommonQuestionList.aspx.cs" Inherits="DynamicQuestionnaire.BackAdmin.CommonQuestionList" %>

<%@ Register Src="~/Components/ucPager.ascx" TagPrefix="uc1" TagName="ucPager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="backAdminCommonQuestionListContainer">
        <div class="row gy-3">
            <div class="col-md-8">
                <div class="row mb-3">
                    <label for='<%= this.txtKeyword.ClientID %>' class="col-sm-2 col-form-label">
                        常用問題名稱：
                    </label>
                    <div class="col-sm-10">
                        <div class="d-flex align-items-center justify-content-center gap-3">
                            <asp:TextBox ID="txtKeyword" CssClass="form-control" runat="server" />

                            <asp:Button ID="btnSearchCommonQuestion" runat="server" Text="搜尋" OnClick="btnSearchCommonQuestion_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="d-flex align-items-center justify-content-end">
                    <asp:Button ID="btnDeleteCommonQuestion" runat="server" Text="刪除" OnClick="btnDeleteCommonQuestion_Click" />
                    <asp:Button ID="btnCreateCommonQuestion" runat="server" Text="新增" OnClick="btnCreateCommonQuestion_Click" />
                </div>
            </div>

            <div class="col-md-10">
                <asp:GridView ID="gvCommonQuestionList" CssClass="table table-bordered w-auto" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbDeleteCommonQuestion" runat="server" />
                                <asp:HiddenField ID="hfCommonQuestionID" runat="server" Value='<%# Eval("CommonQuestionID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:Literal ID="ltlCommonQuestionListNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="常用問題">
                            <ItemTemplate>
                                <asp:Literal ID="ltlCommonQuestionName" Text='<%# Eval("CommonQuestionName") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="詳細">
                            <ItemTemplate>
                                <a href="CommonQuestionDetail.aspx?ID=<%# Eval("CommonQuestionID") %>">詳細</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:PlaceHolder ID="plcEmptyCommonQuestion" runat="server" Visible="false">
                    <p>尚未有資料 </p>
                </asp:PlaceHolder>
            </div>

            <div class="col-md-8">
                <uc1:ucPager runat="server" ID="ucPager" />
            </div>
        </div>
    </div>
</asp:Content>
