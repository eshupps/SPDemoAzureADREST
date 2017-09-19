<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SPDemo.AzureAD.REST._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-top:10px;text-align:right;width:100%;">
        <asp:Hyperlink ID="authLink" Text="Authorize" runat="server"></asp:Hyperlink>
    </div>
    <div class="jumbotron">
        <h1>Azure AD REST Demo</h1>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Calendar Events</h2>
        </div>
        <div class="col-md-4 contentDiv" id="ContentDiv" runat="server">
            
        </div>
    </div>

</asp:Content>
