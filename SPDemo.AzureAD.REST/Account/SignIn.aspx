<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SignIn.aspx.cs" Inherits="SPDemo.AzureAD.REST.Account.SignIn" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">  
    <div id="signinContent" runat="server" visible="false">
        <h1>Azure AD Single Sign-On</h1>
        <div style="">
            <p>If this is the first time anyone in your organisation has attempted to access Training+, the app must first be authorised by an Azure tenant administrator. If you are not an Azure administrator, please contact the appropriate department in your organisation for technical assistance before continuing.</p>
            <p>Please click the "Sign In" button below if the app has already been authorised for your organisation or click "Cancel" to return to the main page.</p>  
            <p><br /><i>Note: This site uses cookies. If you wish to avoid seeing this page in the future, please ensure that cookies are enabled in your browser.</i><br /><br /></p> 
        </div>
        <div style="">
            <asp:Button ID="btnSignIn" CssClass="button" runat="server" Text="Sign In" OnClick="btnSignIn_Click" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click" />
        </div>
    </div> 
</asp:Content>