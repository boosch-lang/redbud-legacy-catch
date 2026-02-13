<%@ Page Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="racklist.aspx.cs" Inherits="Maddux.Catch.racks.racklist" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-inline">
                <div class="form-group">
                    <a href="/racks/rackdetail.aspx" class="btn btn-primary">
                        <i class="fa fa-plus"></i>
                        New Rack</a>
                </div>
                <asp:Button type="button" runat="server" OnClick="btnExportRack_Click"
                    class="btn btn-info" Text="Export Racks"></asp:Button>
                <div class="form-group pull-right">
                    <asp:Label ID="lblRackRecordCount" runat="server" Text="RecordCount"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <div class="row top-buffer">
        <div class="col-xs-12">
            <div class="form-inline pull-right">
                <div class="form-group" style="padding-right: 15px;">
                    <asp:DropDownList ID="ddlCatalogs" CssClass="form-control" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCatalogs_SelectedIndexChanged" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblShowAll" runat="server">
                        <asp:CheckBox ID="chkShowAll" CssClass="form-control" runat="server" AutoPostBack="True"  OnCheckedChanged="chkShowAll_CheckedChanged" />
                        Show All Racks</asp:Label>
                </div>
            </div>
        </div>
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvRacks" runat="server"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="25"
                    OnPageIndexChanging="SubmitRacksGrid_PageIndexChanging"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                    ShowFooter="false"
                    EmptyDataText="There are no racks to display.">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>Rack #</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='rackdetail.aspx?RackID=<%# Eval("RackID")%>'>
                                    <%# Eval("RackID") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>RackName</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='rackdetail.aspx?RackID=<%# Eval("RackID")%>'>
                                    <%# Eval("RackName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>Catalog #</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/catalogs/catalogdetail.aspx?CatalogID=<%# Eval("CatalogID")%>'>
                                    <%# Eval("CatalogName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RackDesc" HeaderText="Rack Description">
                            <ItemStyle Width="10%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="RackSize" HeaderText="Rack Size">
                            <ItemStyle Width="5%" VerticalAlign="Top" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
