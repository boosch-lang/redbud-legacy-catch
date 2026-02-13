<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Maddux.Catch.Master" CodeBehind="calllist.aspx.cs" Inherits="Maddux.Catch.calllist.calllist" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/bootstrap-multiselect.css") %>" type="text/css" />
    <style>
        .rating {
            border: none;
            display: block;
        }

            .rating img {
                display: inline-block;
                width: 18px;
                height: 17px;
                border: 0;
                cursor: pointer;
            }

        .form-inline {
            display: flex;
            flex-flow: row wrap;
            align-items: center;
            margin-left: 15px;
        }

            .form-inline .wrap {
                width: 20%;
            }


            .form-inline label {
                margin: 5px 10px 5px 0;
            }

            .form-inline .input {
                vertical-align: middle;
                margin: 5px 10px 5px 0;
                width: 100%;
                box-sizing: border-box;
                flex: 0 0 200px;
            }

        @media screen and (max-width: 1400px) {
            .form-inline .wrap {
                width: 50%;
            }

            .space-province {
                margin-left: 9px !important;
            }

            .space-type {
                margin-left: 48px !important;
            }
        }

        @media screen and (max-width: 530px) {
            .form-inline .wrap {
                width: 100%;
            }

            .space-campaign {
                margin-left: 18px !important;
            }

            .space-type {
                margin-left: 48px !important;
            }

            .space-province {
                margin-left: 24px !important;
            }
        }

        @media (max-width: 991px) {
            .content-header > .breadcrumb {
                background-color: transparent;
            }
        }

        @media screen and (max-width: 767px) {
            .table-responsive {
                border: none !important;
            }
        }


        label {
            display: inline-block;
            max-width: 100%;
            margin-bottom: 0px;
            font-weight: 700;
            margin-top: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="form-inline">
        <div class="form-group p-1">
            <label class="font-weight-bold">Campaign:</label>
            <asp:ListBox ID="ddlCampaignFilter" ClientIDMode="Static" OnSelectedIndexChanged="ddlCampaignFilter_SelectedIndexChanged" SelectionMode="Multiple" runat="server" CssClass="form-control"></asp:ListBox>
        </div>
        <div class="form-group p-1">
            <label class="font-weight-bold">Membership:</label>
            <asp:ListBox Rows="1" SelectionMode="Multiple" ID="ddlMembershipFilter" ClientIDMode="Static" runat="server" CssClass="form-control multiselect"></asp:ListBox>
        </div>
        <div class="form-group p-1">
            <label class="font-weight-bold">Province:</label>
            <asp:ListBox SelectionMode="Multiple" ID="ddlProvinceFilter" runat="server" CssClass="form-control multiselect"></asp:ListBox>
        </div>
        <div class="form-group p-1">
            <label class="font-weight-bold">Type:</label>
            <asp:ListBox SelectionMode="Multiple" ID="ddlCustomerFilter" runat="server" CssClass="form-control multiselectNoSearch"></asp:ListBox>
        </div>
        <asp:Button ID="btnGenerate" runat="server" OnClick="btnGenerate_Click" CssClass="btn btn-primary p-1" Text="Generate List" />
    </div>

    <div class="row top-buffer">
        <div class="col-sm-12 ">
            <div class="table-responsive">
                <asp:GridView ID="dgvCalllist" runat="server"
                    CssClass="table table-hover table-bordered table-hover dataTable"
                    GridLines="Horizontal" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    ShowFooter="true" OnRowDataBound="dgvCalllist_RowDataBound"
                    EmptyDataText="There are no customers to display.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-3" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-lg-3">
                            <HeaderTemplate>Customer</HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" href='/customer/customerdetail.aspx?CustomerID=<%# Eval("CustomerId")%>'>
                                    <%# Eval("Company") %>
                                </a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="visible-md visible-lg col-md-2" ItemStyle-CssClass="visible-md visible-lg col-md-2">
                            <HeaderTemplate>Contact</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblContact" runat="server" />
                                <asp:HyperLink ID="hypContact" runat="server"><%# Eval("Contact") %></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CityState" HeaderText="City" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2">
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LastOrderDate" HeaderText="Last Order Placed" DataFormatString="{0:MMMM dd, yyyy}"
                            HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg col-xs-3 col-md-2">
                            <ItemStyle VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Rating">
                            <ItemTemplate>
                                <div style="display: inline-table; width: auto;">
                                    <div class="rating">
                                        <asp:Literal ID="litStars" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/scripts/bootstrap-datetimepicker.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/bootstrap-multiselect.js") %>"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('.multiselect').multiselect({
                buttonContainer: '<div class="btn-group multiselect-dropdown-div" />',
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: true,
                enableFullValueFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            $("#ddlCampaignFilter").multiselect({
                buttonContainer: '<div class="btn-group multiselect-dropdown-div" />',
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: true,
                enableFullValueFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownHide: function (event) {
                    $("#frmMain").submit();
                }
            });
            $('.multiselectNoSearch').multiselect({
                includeSelectAllOption: true,
                numberDisplayed: 1,
                maxHeight: 250,
                enableFiltering: false,
                includeSelectAllOption: false,
            });
            //$(document).ready(function () {
            //    $(".multiselect-dropdown-div").find("button.multiselect").addClass("form-control")
            //})

            //btn-group
            //$("#ddlCampaignFilter").on("change", function (e) {
            //    e.preventDefault();
            //    var membershipDropdown=$("#ddlMembershipFilter")
            //    var selectedCampaigns = $("#ddlCampaignFilter").val();
            //    console.log($("#ddlCampaignFilter").val())
            //    var data = new FormData();
            //    data.append("campaignIDs",selectedCampaigns)
            //    $.ajax({
            //        url: "/calllist/requests/GetMembershipForSelectedCampaigns.ashx",
            //        data: data,
            //        type: 'POST',
            //        processData: false,
            //        contentType: false,
            //        success: function (data)
            //        {
            //            console.log(data)
            //            $.each(data.memberships, function(index, item) { // Iterates through a collection
            //                 $("#ddlMembershipFilter").append( // Append an object to the inside of the select box
            //                     `<option value="${item.val}">${item.text}</option>`
            //                 );
            //            });
            //            $("#ddlMembershipFilter").multiselect({
            //                includeSelectAllOption: true,
            //                numberDisplayed: 1,
            //                maxHeight: 250,
            //                enableFiltering: false,
            //                includeSelectAllOption: false
            //            })



            //        }
            //    })
            //})

        });
    </script>

</asp:Content>
