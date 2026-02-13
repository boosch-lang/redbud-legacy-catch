<%@ Page Title="" Language="C#" MasterPageFile="~/Maddux.Catch.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Maddux.Catch.pages.edit" ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Maddux.Catch.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <style>
        label {
            font-size: 14px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Literal Text="" ID="litMessage" runat="server" />
    <div class="row">
        <div class="col-md-4">
            <div class="col-xs-12">
                <asp:HiddenField runat="server" ClientIDMode="Static" ID="BannerImagePath" />
                <div class="text-center">
                    <asp:Image ImageUrl="/img/program-not-available.jpg" ID="pageImage" CssClass="img-thumbnail" ClientIDMode="Static" runat="server" /><br />
                    <br />
                    <button type="button" class="btn btn-default " onclick="moxman.browse({fields: 'BannerImagePath', rootpath: '/uploads/files/pages'}); ">Upload new Image</button>
                </div>
                <br />
            </div>
        </div>
        <div class="col-md-8">
            <div class="form-group">
                <label for="TitleText">Page Title</label>
                <asp:TextBox runat="server" ID="TitleText" ClientIDMode="Static" CssClass="form-control" />
                <asp:RequiredFieldValidator ErrorMessage="Page title is required!" CssClass="text-danger" ControlToValidate="TitleText" runat="server" />
            </div>
            <div class="form-group">
                <label for="Description">Description</label>
                <asp:TextBox runat="server" ID="Description" ClientIDMode="Static" CssClass="form-control" />
                <asp:RequiredFieldValidator ErrorMessage="Description is required!" CssClass="text-danger" ControlToValidate="Description" runat="server" />
            </div>
            <div class="form-group">
                <label for="HTML">Content</label><br />
                <asp:TextBox ID="HTML" runat="server" CssClass="tinyMCE" Placeholder="Bio" TextMode="MultiLine" Rows="5"></asp:TextBox>
            </div>
            <div class="form-group">
                <div class="text-center">
                    <asp:Button Text="Save" ID="Save" CausesValidation="true" CssClass="btn btn-success" OnClick="Save_Click" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="server">

    <script src="https://cloud.tinymce.com/stable/jquery.tinymce.min.js"></script>
    <script src="https://cloud.tinymce.com/stable/tinymce.min.js?apiKey=YOUR-API-KEY-GOES-HERE"></script>
    <script src="../js/tinyMCE.js"></script>
    <script src="../js/moxiemanager/js/moxman.loader.min.js"></script>
    <script>
        $(".breadcrumb").prop("style", "display:none");
        $(document).ready(function () {
            if ($("#BannerImagePath").val().length > 0) {
                $("#pageImage").prop("src", `${$("#BannerImagePath").val()}`);
            }

        })
        $("#BannerImagePath").on("change", function (e) {
            e.preventDefault();
            $("#pageImage").prop("src", `${$("#BannerImagePath").val()}`);

        })
    </script>
</asp:Content>
