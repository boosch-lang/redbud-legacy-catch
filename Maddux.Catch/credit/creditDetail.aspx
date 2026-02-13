<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="creditDetail.aspx.cs" Inherits="Maddux.Catch.credit.creditDetail" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row">
        <div class="col-md-6">
            <asp:Button Text="Save" runat="server" ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" />
            <button id="DeleteBtn" class="btn btn-danger">Delete</button>
        </div>
        <div class="col-md-6 text-right">
            <span id="EmailConfirmBtn" runat="server" ClientIDMode="Static" Style="border: solid 1px #808080" class="btn btn-secondary">Email Credit Memo</span>
            <asp:Button Text="Print Credit Memo" ID="PrintMemo" CssClass="btn btn-secondary" Style="border: solid 1px #808080" OnClick="PrintMemo_Click" runat="server" />
            <asp:Button Text="Send Confirmation" ID="btnEmailCreditMemo" ClientIDMode="Static" CausesValidation="false" OnClick="btnEmailCreditMemo_Click"   Style="display: none" CssClass="btn btn-primary d-none" runat="server" />

        </div>
    </div>

    <asp:Panel ID="panelCustomer" runat="server" GroupingText="&nbsp;Details&nbsp;&nbsp;&nbsp;" Font-Bold="True">
        <div class="row">
            <div class="col-md-6">
                <label class="required font-weight-bold">Date: <span class="text-danger">*</span></label>
                <div class='input-group datepicker'>
                    <asp:TextBox ID="txtDate" runat="server" data-date-format="MMMM DD, YYYY" onkeypress="return false;" CssClass="form-control"></asp:TextBox>
                    <span class="input-group-addon">
                        <span class="fa fa-calendar"></span>
                    </span>
                </div>
            </div>
            <div class="col-md-6"></div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <label class="font-weight-bold">Credit Notes:</label>
                <asp:TextBox ID="txtNotes" runat="server" BorderStyle="None" Height="160px" TextMode="MultiLine"
                    CssClass="form-control" TabIndex="1"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <label class=" font-weight-bold">Office Notes:</label>
                <asp:TextBox ID="txtOfficeNotes" runat="server" BorderStyle="None" Height="160px" TextMode="MultiLine"
                    CssClass="form-control" TabIndex="1"></asp:TextBox>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlItems" GroupingText="&nbsp;Items&nbsp;&nbsp;&nbsp;" Font-Bold="True">
        <div class="row">
            <div class="col-xs-12">
                <a href="#" class="btn btn-default" title="New Contact" data-toggle="modal" data-target="#modalView" data-remote="false">Add Item</a>
            </div>
        </div>

        <asp:GridView ID="dgvItems" runat="server"
            CssClass="table table-hover table-bordered table-hover dataTable"
            GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            EmptyDataText="There are no items to display.">
            <Columns>
                <%--<asp:TemplateField>
                    <HeaderTemplate>Units</HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtUnits" runat="server" CssClass="form-control" Text='<%# Eval("Units") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                <%-- <asp:TemplateField>
                    <HeaderTemplate>Packages</HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtPackages" runat="server" CssClass="form-control" Text='<%# Eval("PackagesPerUnit") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:BoundField HeaderStyle-CssClass="col-2" ItemStyle-CssClass="col-2" DataField="Category" HeaderText="Category" />
                <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="Units" HeaderText="Unit(s)" />
                <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="ItemNumberInternal" HeaderText="Item #" />

                <asp:TemplateField HeaderStyle-CssClass="col-3" ItemStyle-CssClass="col-3">
                    <HeaderTemplate>Description</HeaderTemplate>
                    <ItemTemplate>
                        <a href="/products/productDetail.aspx?productid=<%# Eval("ProductID") %>" target="_blank">
                            <%# Eval("Description") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="Size" HeaderText="Size" />
                <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="ItemsPerPack" HeaderText="It./Pck" />
                <asp:TemplateField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1">
                    <HeaderTemplate>Each Price($)</HeaderTemplate>
                    <ItemTemplate>
                       <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Text='<%#  Convert.ToDouble(Eval("EachPrice")).ToString("F") %>' ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"  FilterType="Numbers"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="Total" HeaderText="Total" />
                <asp:TemplateField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1">
                    <ItemTemplate>
                        <asp:Button Text="Remove" CssClass="btn btn-link" ID="RemoveProduct" CausesValidation="true" OnClick="RemoveProduct_Click" CommandArgument='<%# Eval("CreditItemId") %>' Style="color: #a94442!important" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="row">
            <div class="col-md-6"></div>
            <div class="col-md-6">
                <table class="table table-hover table-bordered table-hover">
                    <tr id="SubtotalDiv">
                        <td class="text-right">
                            <label class="font-weight-bold">Subtotal</label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblSubtotal"></asp:Label></td>
                    </tr>
                    <tr id="FreightDiv">
                        <td class="">
                            <div class="row">
                                <div class="col-xs-6" style="padding-left: 20px">
                                    <asp:CheckBox Text="" CssClass="chk" ID="chkOverrideFreightCredit" ClientIDMode="Static" runat="server" />
                                    &nbsp; Override Freight Credit
                                </div>
                                <div class="col-xs-6 text-right">
                                    <label class="font-weight-bold">Freight</label>
                                </div>
                            </div>

                        </td>
                        <td>
                            <div id="FreightLabelDiv" clientidmode="static" runat="server">
                                <asp:Label runat="server" ID="lblFreight"></asp:Label>
                            </div>
                            <div id="FreightTextDiv" clientidmode="static" runat="server">
                                <asp:TextBox runat="server" type="text" ID="txtFreight" min="0" Width="200" CssClass="form-control" />
                            </div>
                        </td>
                    </tr>
                    <tr id="GstDiv">
                        <td class="text-right">
                            <label id="TaxTypeText" class="font-weight-bold" runat="server">GST</label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblGST"></asp:Label></td>
                    </tr>
                    <tr id="TotalDiv">
                        <td class="text-right">
                            <label class="font-weight-bold">Total</label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTotal"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RegularExpressionValidator CssClass="text-danger" ErrorMessage="Please enter valid freight charge!" ValidationExpression="^[0-9]\d*(\.\d+)?$" ControlToValidate="txtFreight" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </asp:Panel>
    <div class="modal fade" id="modalView" tabindex="-1" role="dialog" aria-labelledby="modalView" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="row">
                        <h4 class="modal-title col-xs-11" id="modalViewTitle">Add Items</h4>
                        <div class="text-right col-xs-1">

                            <a href="#" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></a>
                        </div>
                    </div>
                </div>

                <div class="modal-body">
                    <asp:Literal Text="" ID="litModalMessage" runat="server" />
                    <div class="row">
                        <div class="col-md-4">
                            <label class="font-weight-bold">Shipment:</label>
                        </div>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddShipmentList" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddShipmentList_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>

                        </div>
                    </div>
                    <div style="margin-top: 10px!important" class="row">
                        <div class="col-xs-12">
                            <asp:GridView ID="grdOrderItems" runat="server"
                                CssClass="table table-hover table-bordered table-hover dataTable"
                                GridLines="Horizontal" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                ShowFooter="true"
                                EmptyDataText="There are no items to display.">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1">
                                        <HeaderTemplate>Credit Qty</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUnits" Width="75" min="0" max='<%# Eval("Quantity") %>' runat="server" CssClass="form-control" type="number" Text="0"></asp:TextBox>
                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("ProductID") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnPrice" runat="server" Value='<%# Eval("EachPrice") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnPacksPerUnit" runat="server" Value='<%# Eval("PacksPerUnit") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="Quantity" HeaderText="Qty Shipped" />
                                    <%--                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="PacksPerUnit" HeaderText="Pck / Unit" />
                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="ItemsPerPack" HeaderText="It. / Pck" />--%>
                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="ItemNumber" HeaderText="Item #" />
                                    <asp:TemplateField HeaderStyle-CssClass="col-4" ItemStyle-CssClass="col-4">
                                        <HeaderTemplate>Description</HeaderTemplate>
                                        <ItemTemplate>
                                            <a href="/products/productDetail.aspx?productid=<%# Eval("ProductID") %>" target="_blank">

                                                <%# Eval("Description") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="Size" HeaderText="Size" />
                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="EachPrice" DataFormatString="{0:C}" HeaderText="Item Price" />
                                    <asp:BoundField HeaderStyle-CssClass="col-1" ItemStyle-CssClass="col-1" DataField="Discount" HeaderText="Discount" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="saveAndClose" CssClass="btn btn-primary" runat="server" OnClick="saveAndClose_Click" Text="Add" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                </div>
            </div>
        </div>
    </div>
    <div id="DeleteCreditMemoConfirm" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">Delete Credit Memo
                <span class="pull-right">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </span>
                    </h3>
                </div>
                <div class="modal-body">
                    <br />
                    <br />
                    <h4 class="text-center">Are you sure you want to remove this credit memo?</h4>
                    <br />
                    <br />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnDelete" runat="server" Text="Yes, Delete" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
                </div>
            </div>
        </div>
    </div>
    <!--Email Modal -->
    <div class="modal fade" id="emailModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <span id="EmailTitle"></span>
                        <span class="pull-right">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </span>
                    </h4>

                </div>
                <div class="modal-body">
                    <asp:Literal Text="" ID="litEmailMessage" runat="server" />
                    <label class="font-weight-bold">To</label>
                    <asp:TextBox ID="emailTo" type="email" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Cc</label>
                    <asp:TextBox ID="emailCC" type="email" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Subject</label>
                    <asp:TextBox ID="emailSubject" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="font-weight-bold">Body</label>
                    <asp:TextBox ID="emailBody" ClientIDMode="Static" CssClass="form-control" TextMode="MultiLine" Rows="8" runat="server" />
                    <br />
                    <!--
                    <div class="">
                        <asp:CheckBox Text="" CssClass="" ID="chkUpdateConfirmationDate" Checked="true" runat="server" />&nbsp; <span style="font-weight: 700">Update Sent date.</span>
                    </div>
                    -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <span class="btn btn-primary" id="SendConfirmation">Send Credit Memo</span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script>
        $("#chkOverrideFreightCredit").change(function (e) {
            if ($(this).is(':checked')) {
                $("#FreightLabelDiv").hide();
                $("#FreightTextDiv").show();
            }
            else {
                $("#FreightTextDiv").hide();
                $("#FreightLabelDiv").show();
            }
        });
        $("#DeleteBtn").on("click", function (e) {
            e.preventDefault();
            $("#DeleteCreditMemoConfirm").modal("toggle");
        })
        $("#EmailConfirmBtn").on("click", function (e) {
            $("#EmailTitle").text("Email Confirmation")
            $("#emailModal").modal("show");
        })
        $("#SendConfirmation").on("click", function (e) {
            $("#emailModal").modal("hide");
            $("#btnEmailCreditMemo").click();
        })
    </script>
    <div id="ScriptDIV" runat="server"></div>

</asp:Content>
