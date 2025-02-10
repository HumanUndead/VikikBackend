<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubLogin.aspx.cs" Inherits="Admin_SubLogin" %>
<!DOCTYPE html>
<html lang="en" class="login">
<head id="Head1" runat="server">
    <title><%=Resources.CMS.Client %> Website Administration Tool</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logincontent">
        <div class="logintitle"><img src="images/rhino.png" />Kensoftware CMS <smaller><%=Resources.CMS.Client %></smaller></div>         
    <div class="maincont ">

<br  class="clr"/>
<div class="centerme" >

<asp:Login ID="lgnAdmin" runat="server" DestinationPageUrl="Default.aspx" OnAuthenticate="lgnAdmin_Authenticate">
<LayoutTemplate>
<asp:Panel ID="Panel1" DefaultButton="btnLogin" runat="server" CssClass="login">
<p>
      <label for="login">UserName:</label>
     <ken:TextBox runat="server" id="UserName" Required="true" ValidationGroup="login" CssClass="fieldsbox"></ken:TextBox>
    </p>

    <p>
      <label for="password">Password:</label>
      <ken:TextBox runat="server" id="Password" TextMode="Password" Required="true" ValidationGroup="login" CssClass="fieldsbox"></ken:TextBox>
    </p>

    <p class="login-submit">
    <ken:Button ID="btnLogin" ValidationGroup="login" runat="server" CommandName="Login" CssClass="login-button" Text="Login"></ken:Button>
    </p>

    <p class="forgot-password"><a href="forgotpassword.aspx"></a></p>
</asp:Panel>
</LayoutTemplate>
</asp:Login>

</div>
</div>
    </div>
         <footer>
          <div class="footcont">
        <p class="pull-left"> <%=Resources.CMS.Copyright %>
                        &copy;
                       <%=DateTime.Now.Year %> <a href="http://www.kensoftware.com" target="_blank">
                                <img id="ksLogo" alt="website development, design, hosting, CMS systems, asp.net" runat="server"
                                    src="~/images/kslogo.png" /></a></p>
              </div>
      </footer>
    </form>
</body>
</html>

