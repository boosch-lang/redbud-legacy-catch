<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="userlist.aspx.cs" Inherits="Maddux.Catch.users.userlist" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="col-xs-12" style="padding-left: 0px;">
        <div class="form-inline">
            <div class="btn-group">
                <asp:Button Text="Add User" runat="server" ID="btnAddUser" OnClick="btnAddUser_Click" CssClass="btn btn-success" />
            </div>
            <div class="form-group">
                <asp:Label ID="lblShowAll" runat="server">
                    <asp:CheckBox ID="chkShowAll" CssClass="form-control" runat="server" AutoPostBack="True"
                        OnCheckedChanged="chkShowAll_CheckedChanged" />
                    Show All Users</asp:Label>
            </div>
        </div>
    </div>

    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvUsers" runat="server"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="20"
                    OnPageIndexChanging="SubmitUsersGrid_PageIndexChanging"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="false" OnRowDataBound="GridUsers_RowDataBound"
                    EmptyDataText="There are no users to display.">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField
                            HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1"
                            ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Name</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href="userdetail.aspx?UserId=<%# Eval("UserID") %>"><%# Eval("FirstName") %> <%# Eval("LastName") %></a>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1"
                            ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Email</HeaderTemplate>
                            <ItemTemplate>
                                <p><%# Eval("EmailAddress") %></p>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate>Active</HeaderTemplate>
                            <ItemTemplate><%# (Boolean.Parse(Eval("Active").ToString())) ? "Yes" : "No" %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-1">
                            <HeaderTemplate></HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" id="lnkResetUserPassword" href="/myaccount.aspx?ResetPasswordUserId=<%# Eval("UserID") %>" class="btn btn-primary">Reset Password</a>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>

