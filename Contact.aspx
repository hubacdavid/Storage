<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <address>
        Dávid Hubač<br />
        Senická cesta 66<br />
        <abbr title="Phone">M:</abbr>
        +421 908 404 678
    </address>

    <address>
        <strong>Owner:</strong>   <a href="mailto:hubac.david@gmail.com">hubac.david@gmail.com</a><br />
    </address>
</asp:Content>
