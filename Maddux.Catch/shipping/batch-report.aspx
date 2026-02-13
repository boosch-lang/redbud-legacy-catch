<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="batch-report.aspx.cs" Inherits="Maddux.Catch.shipping.batch_report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/font-awesome.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/rbstyle.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("https://catch.redbud.com/css/print-css.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/jquery-2.2.3.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("https://catch.redbud.com/js/utils.js") %>"></script>
     <style>
        tr td {
            padding:2px!important;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="row">
                <div class="col-xs-3">
                    <img class="set-logo-width" src="<%= ResolveUrl("https://catch.redbud.com/img/Redbud_logo_black.png") %>" />
                    <p>
                        P.O. Box 81187
                        <br />
                        Ancaster, ON L9G 4X2
                    </p>
                    <br />
                    <p>
                        Phone: 1-888-733-2830
                    <br />
                        Fax: 1-888-733-2850
                    <br />
                        Email: sales@redbud.com
                    </p>

                </div>
                <div class="col-xs-4"></div>
                <div class="col-xs-5">
                    <h3 id="lblType" style="font-weight:700" runat="server">Shipping Batch Report </h3>
                    <div class="row">
                        <label class="col-xs-5">Batch Number:</label>
                        <div class="col-xs-7">
                            <label class="font-weight-normal" runat="server" id="lblBatchNumber" />
                        </div>
                    </div>
                    <div class="row">
                        <label class="col-xs-5">Batch Date:</label>
                        <div class="col-xs-7">
                            <label class="font-weight-normal" runat="server" id="lblBatchDate" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:GridView ID="dgvBatchReport" runat="server"
                        CssClass="table table-sm"
                        GridLines="None" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                        ShowHeader="true" ShowFooter="false"
                        OnRowDataBound="dgvBatchReport_RowDataBound"
                        EmptyDataText="There are no batch items.">
                        <Columns>
                            
                            <asp:BoundField DataField="InvNo" HeaderText="Inv. No." HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                            <asp:BoundField DataField="InvDate" HeaderText="Inv. Date." DataFormatString="{0:dd-MMM-yyyy}"  SortExpression="DefaultQuantity" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" />
                            <asp:BoundField DataField="CustomerName"   HeaderText="Customer" SortExpression="ProductName" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" />
                            <asp:BoundField DataField="Copies" HeaderText="Copies"  HeaderStyle-CssClass= "visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1"/>
                            <asp:TemplateField HeaderText="Emailed" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1">
                                <ItemTemplate>
                                    <asp:CheckBox Text="" CssClass="chk" Checked='<%# Eval("Emailed") %>' runat="server" />
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-2" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="UnitPrice" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" FooterStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-1" />
                              <asp:TemplateField HeaderText="Posted" HeaderStyle-CssClass="col-xs-1" ItemStyle-CssClass="col-xs-1" FooterStyle-CssClass="col-xs-1">
                                <ItemTemplate>
                                    <input type="text" style="width:50px" name="name" value="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
               
            </div>
        </div>
    </form>
    
    <div style="page-break-after: always"></div>
    <script>
        $(".chk").each(function (i, v) {
            $(v).find("input[type=checkbox]").prop("class", "form-control");
        })
    </script>
</body>
</html>