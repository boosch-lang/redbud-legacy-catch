<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="programlist.aspx.cs" Inherits="Maddux.Catch.products.programlist" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal runat="server" ID="litMessage"></asp:Literal>
    <div class="row">
        <div class="col-xs-12">
            <div class="form-inline">
                <div class="form-group">
                    <a href="/products/programdetail.aspx" class="btn btn-primary">
                        <i class="fa fa-plus"></i>
                        New Program</a>
                </div>
                <div class="form-group">
                    <label id="lblProgram" runat="server">Program:</label>
                    <asp:DropDownList ID="ddlProgram" runat="server"
                        AutoPostBack="True" Width="350px"
                        CssClass="form-control"
                        OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <asp:Label ID="lblShowAll" runat="server">
                        <asp:CheckBox ID="chkShowAll" CssClass="form-control" runat="server" AutoPostBack="True"
                            OnCheckedChanged="chkShowAll_CheckedChanged" />
                        Show All Programs</asp:Label>
                </div>
                <div class="form-group pull-right">
                    <asp:Label ID="lblProgramRecordCount" runat="server" Text="RecordCount"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <div class="row top-buffer">
        <div class="col-xs-12">
            <div class="table-responsive">
                <asp:GridView ID="gridProgramCatalogs" runat="server"
                    AutoGenerateColumns="False"
                    AutoGenerateEditButton="false"
                    AllowPaging="true"
                    AllowSorting="true"
                    SortMode="Automatic"
                    PageSize="25"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    OnPageIndexChanging="SubmitCatalogsGrid_PageIndexChanging"
                    EnableModelValidation="True" Width="100%" CellPadding="3"
                    EmptyDataText="No records found"
                    GridLines="Horizontal" OnRowDataBound="gridPrograms_RowDataBound">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>Program Name</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='programdetail.aspx?ProgramID=<%# Eval("ProgramID")%>'>
                                    <%# Eval("ProgramName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CatalogId" HeaderText="Catalog #">
                            <ItemStyle Width="7%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <HeaderTemplate>Catalog Name</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/catalogs/catalogdetail.aspx?CatalogID=<%# Eval("CatalogId")%>'>
                                    <%# Eval("CatalogName") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="CatalogGroupId" HeaderText="Catalog Group #">
                            <ItemStyle Width="7%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CatalogClassId" HeaderText="Catalog Class #">
                            <ItemStyle Width="7%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CatalogSeason" HeaderText="Catalog Season">
                            <ItemStyle Width="7%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CatalogYear" HeaderText="Catalog Year">
                            <ItemStyle Width="7%" VerticalAlign="Top" />
                        </asp:BoundField>

                        <asp:TemplateField>
                            <HeaderTemplate>Active</HeaderTemplate>
                            <ItemTemplate><%# (Boolean.Parse(Eval("Active").ToString())) ? "Yes" : "No" %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Notes" HeaderText="Notes">
                            <ItemStyle Width="30%" VerticalAlign="Top" />
                        </asp:BoundField>

                        
                    </Columns>
                    <EmptyDataRowStyle CssClass="grdNoData" />
                    <HeaderStyle CssClass="grdHeader" Font-Bold="False" />
                    <RowStyle CssClass="grdRow" BorderStyle="Solid" BorderColor="#DDDDDD" BorderWidth="1px" Height="22px" VerticalAlign="Middle" />
                </asp:GridView>
            </div>
        </div>
    </div>

    <link href="../css/pagination.css" rel="stylesheet" />
    <link href="../css/extra.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
</asp:Content>
