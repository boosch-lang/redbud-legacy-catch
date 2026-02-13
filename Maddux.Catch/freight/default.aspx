<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Maddux.Catch.freight._default" %>

<%@ MasterType VirtualPath="~/Maddux.Catch.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row">
        <div class="col-md-12">
            <div class="pull-right">
                <a target="_blank" href="edit.aspx" class="btn btn-primary"><i class="fa fa-plus"></i>&nbsp;New Freight Charges</a>
                <button id="ImportCharges" class="btn btn-success"><i class="fa fa-upload"></i>&nbsp;Import</button>
            </div>

        </div>
        <div class="col-md-4">
            <div class="input-group">
                <asp:TextBox runat="server" ID="SearctCriteria" CssClass="form-control" />
                <span style="padding: 0px" class="input-group-addon">
                    <button runat="server" class="btn btn-primary btn-sm" onserverclick="Search_ServerClick" id="Search" title="Search">
                        <i class="fa fa-search"></i>Search
                    </button>
                </span>
            </div>
        </div>
        <div class="col-md-4">
            <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" ID="ddlProvince">
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:DropDownList runat="server" DataTextField="Text" DataValueField="Value" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" ID="ddlRegion">
            </asp:DropDownList>
        </div>
        <div class="col-md-12">

            <asp:GridView runat="server"
                CssClass="table table-hover table-bordered table-hover"
                ID="dgvFreightCharges"
                AllowSorting="true"
                SortMode="Automatic"
                GridLines="Horizontal"
                AutoGenerateColumns="False"
                ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                OnRowDataBound="dgvFreightCharges_RowDataBound"
                EmptyDataText="There are no freight charges assigned to display.">
                <Columns>
                    <asp:TemplateField HeaderText="Area Code" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1">
                        <ItemTemplate>
                            <a target="_blank" href="edit.aspx?aID=<%# Eval("AreaID") %>"><%# Eval("AreaID") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PlaceName" HeaderText="Place Name" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-4" />
                    <asp:BoundField DataField="Province" HeaderText="Province" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                    <asp:BoundField DataField="Region" HeaderText="Region" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                    <asp:BoundField DataField="Charge" HeaderText="Charge" DataFormatString="{0} %" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-5" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-5" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-5">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="ImportModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Import Freight Charges
                <span class="pull-right">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </span>
                    </h4>

                </div>
                <div class="modal-body">
                    <div class="p-5">
                        <asp:FileUpload ID="ImportFile" accept=".xlsx, .xls, .csv" runat="server" />
                        <br />
                        <span class="text-info"><small>Please select excel file you want import</small></span>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button Text="Import" ID="btnImportFreightCharges" CssClass="btn btn-success" OnClick="btnImportFreightCharges_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">
    <script>
        $("#ImportCharges").on("click", function (e) {
            e.preventDefault();
            $("#ImportModal").modal("show");
        })
    </script>
</asp:Content>
