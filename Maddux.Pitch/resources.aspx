<%@ Page Title="Resources | Redbud" Language="C#" AutoEventWireup="true" MasterPageFile="~/Authorized.Master" CodeBehind="resources.aspx.cs" Inherits="Maddux.Pitch.resources" %>

<asp:Content ID="headerContent" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="stylesheet" href="css/swipebox.css" />
    <style>
        a {
            color: black;
        }
    </style>
</asp:Content>


<asp:Content ID="bodyContent" ContentPlaceHolderID="cphBody" runat="server">

    <div class="bg-white mt-5" style="border-radius: 10px">
        <div class="row">
            <div class="bg-white px-5" style="border-radius: 10px">
                <div class="row pt-5 pb-5">
                    <div class="col">
                        <h1 class="ms-400 fs-25">Staff Resources</h1>
                    </div>
                </div>

                <div class="py-5">
                    <p>
                        <a href="uploads/Air%20Plant%20care.pdf" target="_blank" class="ms-300 fs-25">Air Plant care</a>
                    </p>
                    <p>
                        <a href="uploads/Cacti%20&%20Succulent%20care.pdf" target="_blank" class="ms-300 fs-25">Cacti & Succulent care</a>
                    </p>
                    <p>
                        <a href="uploads/Tropical%20Foliage%20care.pdf" target="_blank" class="ms-300 fs-25">Tropical Foliage care</a>
                    </p>
                    <br />
                    <p>
                        <a href="uploads/Les-Tillandsias-QC.pdf" target="_blank" class="ms-300 fs-25">Les Tillandsias</a>
                    </p>
                    <p>
                        <a href="uploads/Cactus-et-Succulentes-QC.pdf" target="_blank" class="ms-300 fs-25">Cactus et Succulents</a>
                    </p>
                    <p>
                        <a href="uploads/Plantes-Tropicales-QC.pdf" target="_blank" class="ms-300 fs-25">Plantes Tropicales</a>
                    </p>
                </div>

                <div style="height: 150px"></div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="scriptContent" ContentPlaceHolderID="cphScript" runat="server">
    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
</asp:Content>
