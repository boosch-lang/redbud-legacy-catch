<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reorder.aspx.cs" Inherits="Maddux.Pitch.reorder" %>--%>
<%@ Page Title="Reorder" Language="C#" MasterPageFile="~/Redbud.Master" AutoEventWireup="true" CodeBehind="reorder.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Maddux.Pitch.reorder" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="css/swipebox.css" />
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <div class="modal fade" id="modalStores"  tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Reorder</h4>
                </div>
                <div id="divStoreBody" class="modal-body">
                    <input type="hidden" id="hdnRackID" runat="server" class="hdnRackID" />
                    <asp:Label ID="litErrorModal" CssClass="text-danger" ClientIDMode="Static"  runat="server" />
                    <div class="row">
                        <div class="col-lg-6">
                            <label for="PONumber">Purchase Order # </label>
                            <asp:TextBox runat="server" CssClass="form-control" ClientIDMode="Static" ID="PONumber" />
                           <%-- <asp:RequiredFieldValidator ErrorMessage="Purchase Order # is required" CssClass="text-danger" ControlToValidate="PONumber" runat="server" /><br />--%>
                            <asp:RegularExpressionValidator ErrorMessage="Purchase Order # can't be longer than 20 characters" ValidationExpression="^[a-zA-Z0-9].{1,20}$" CssClass="text-danger" ControlToValidate="PONumber" runat="server" />
                        </div>
                        <div class="col-lg-6">
                            <label for="OrderByField">Order Placed By <span class="text-danger">*</span></label>
                            <asp:TextBox runat="server" CssClass="form-control"  ClientIDMode="Static" ID="OrderByField" />
                            <%--<asp:RequiredFieldValidator ErrorMessage="Order Placed By field is required!" CssClass="text-danger" ControlToValidate="OrderByField" runat="server" /><br />--%>
                            <asp:RegularExpressionValidator ErrorMessage="Order Placed By can't be longer than 50 characters" ValidationExpression="^[a-zA-Z].{1,50}$" CssClass="text-danger" ControlToValidate="OrderByField" runat="server" />
                        </div>
                    </div>
                    <h3>1. Choose Ship Date(s):</h3>
                    <div class="col-xs-12 dateSelector">
                        <div class="checkbox checkboxlist" style="margin-top: -10px;">
                            <asp:Repeater OnItemDataBound="rptShipDates_ItemDataBound" runat="server" ID="rptShipDates">
                                <ItemTemplate>
                                    <asp:CheckBoxList ID="lstShipDateSelector" runat="server" CssClass="form-control no-border order-date-selector" RepeatDirection="Vertical" RepeatLayout="Flow" Height="100%"></asp:CheckBoxList>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <br />
                    <br />
                     <div class="row">
                        <div class="col-xs-9">
                            <h3>2. Select Rack Quantity </h3>
                        </div>
                        <div id="SelectAllDiv" runat="server" class="col-xs-3">
                            <div class="input-group mb-3">
                                <input class="form-control" id="QuantityForAll" min="0" type="number" value="1" />
                                <span style="padding: 5px 12px!important;background-color:#ed174f!important" class="input-group-addon">
                                    <button id="btnApplyAll" style="padding: 0px!important;color:white!important" class="btn btn-link">Set All</button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlSubCustomers" runat="server" CssClass="row  bottom-buffer-small">
                        <div class="col-xs-12">
                            <div class="row">
                                <div class="col-xs-12">
                                    <table class="table table-striped" style="width: 100%">
                                        <tbody>
                                            <asp:Repeater ID="rptStores" runat="server" OnItemDataBound="rptStores_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hdnCompanyId" Value='<%# Eval("CustomerId") %>' runat="server" />
                                                            <asp:CheckBox runat="server" ID="chkStoreSelected" CssClass="chkSelected" Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="StoreName" Text='<%# Eval("Company") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRackQuantity" type="Number" Style="width: 80px; float: right" CssClass="form-control text-right popup-quantity-text" runat="server" MaxLength="10" min="0" onkeydown="return numberKeysOnly(event);" onblur="validateNumberInput(this);">1</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>

                                    <div class="row">
                                        <div class="col-xs-12">
                                            <label class="text-danger" id="errorMsg">
                                            </label>
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                </div>
                <div class="modal-footer">
                    <div class="pull-left"><a href="order.aspx" style="color:white!important" class="btn btn-warning"><i class="fas fa-arrow-circle-left"></i> Go Back</a></div>
                    <asp:Button ID="btnPlaceOrder" CssClass="btn btn-primary AddToCartButton" ClientIDMode="Static" CausesValidation="true" runat="server" Text="Add to Cart" OnClick="btnPlaceOrder_Click" OnClientClick="return validatePlaceOrder();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script>
        $(document).ready(function () {
            $("#modalStores").modal("show");
            $("#btnPlaceOrder").prop("disabled", true);
            CheckAddToCart();
        })
        function CheckAddToCart()
        {
            var $OrderBy = $("#OrderByField").val();
            if ($(".order-date-selector input:checked").length > 0  && $OrderBy.length > 0) {
                $(".AddToCartButton").prop("disabled", false);
            }
            else {
                 $(".AddToCartButton").prop("disabled", true);
            }
        }
        $(document).on('click', ".order-date-selector", function (e) {
            CheckAddToCart()
        });
        $("#PONumber").on('change', function () {
            CheckAddToCart()
        });
         $("#OrderByField").on('change', function () {
            CheckAddToCart()
        });
        $(document).on('click', '.chkSelected', function () {
             CheckAddToCart()
        })
        function validatePlaceOrder() {
            var textBoxes = $("#modalStores").find("input[name='ctl00$cphBody$rptStores$ctl00$txtRackQuantity']");
            
            var OPlacedBy = $("#OrderByField");
            var flag = true;
            var msg="";
            
            if (OPlacedBy.val() == "" || OPlacedBy.val().length === 0)
            {
                msg +="<br /> Order Placed By is Required!"
                flag=false;
            }
            var qtyMsg = "";
            console.log(flag);
            $.each(textBoxes, function (i, v) {
                var val = v.value;
                if ($.isNumeric(val) == false) {
                    console.log(val);
                    qtyMsg = "You must specify a whole number for quantity!";
                    flag=false;
                }
            });
            console.log(flag);
            if (flag === false) {
                $("#litErrorModal").val(`${msg}<br />${qtyMsg}`);
                $("#modalStores").modal("show");
                return flag;
            }
            else {
                return true;
            }
            
        }
        $("#btnApplyAll").click(function (e) {
            e.preventDefault();
            var qty = $("#QuantityForAll").val()
            $(".popup-quantity-text").val(qty);
        });
    </script>
   
</asp:Content>