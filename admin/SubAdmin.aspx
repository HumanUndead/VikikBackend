<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.master" AutoEventWireup="true" CodeFile="SubAdmin.aspx.cs" Inherits="Admin_SubAdmin" %>

<asp:Content ID="cntWorkArea" ContentPlaceHolderID="cphWorkArea" Runat="Server">
<asp:PlaceHolder  runat="server" ID="phControl"></asp:PlaceHolder>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphTitle">
<asp:ImageButton runat="server" ID="btnBack" ImageUrl="images/back.png" CssClass="backbtn" OnClick="btnBack_Click"/>
<asp:Literal runat="server" ID="ltlTitle"></asp:Literal>
</asp:Content>

