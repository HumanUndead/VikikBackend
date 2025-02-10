<%@ page title="" language="C#" masterpagefile="~/Admin.master" autoeventwireup="true" CodeFile="CommerceUsers.aspx.cs" inherits="CommerceUsers" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" Runat="Server">
    <!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
<!-- Optional theme -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css" />
<!-- Latest compiled and minified JavaScript -->
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js" type="text/javascript"></script>
    <style>
        a
        {
            color:#0094ff !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitle" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphWorkArea" Runat="Server">
    <div class="row">
<div class="btn-group pull-left col-md-6" >

</div>
    <div class="pull-right col-md-6">
    <div class="input-group">
      <input type="text" id="srch" class="form-control" placeholder="User name or email...">
      <span class="input-group-btn">
        <button class="btn btn-default" type="button" onclick="search()">Search</button>
      </span>
    </div><!-- /input-group -->
  </div>
        </div>
    <br />
    <table class="table table-striped">
        <tr><th>Name</th><th>Email</th><th>Balance</th><th>Registration Date</th><th>Orders</th><th>Transactions</th></tr>
        <asp:Repeater runat="server" ID="repUsers">
            <ItemTemplate>
            <tr><td><a href="default?mod=usr-p&id=<%#DataBinder.Eval(Container.DataItem,"ID") %>"><%#DataBinder.Eval(Container.DataItem,"Name") %></a></td><td><%#DataBinder.Eval(Container.DataItem,"Email") %></td><th>AED <%#DataBinder.Eval(Container.DataItem,"Balance") %></th><td><%#((DateTime)DataBinder.Eval(Container.DataItem,"registrationdate")).ToString("dd/MM/yyyy") %></td><td><a href="orders?uid=<%#DataBinder.Eval(Container.DataItem,"ID") %>">Orders</a></td><td><a href="commerceusertransactions?id=<%#DataBinder.Eval(Container.DataItem,"ID") %>">Transactions</a></td></tr>
        </ItemTemplate>
        </asp:Repeater>
</table>
    <nav>
  <ul class="pagination pull-right">
    <li runat="server" id="liPrev">
      <a href="commerceusers?pnum=<%=PageNumber-1 %>&srch=<%=Request["srch"] %>" aria-label="Previous">
        <span aria-hidden="true">&laquo;</span>
      </a>
    </li>
      <asp:Repeater runat="server" ID="repPages">
          <ItemTemplate>
              <li <%#(int)Container.DataItem==PageNumber?"class=\"active\"":"" %>><a href="commerceusers?pnum=<%#Container.DataItem.ToString() %>&srch=<%=Request["srch"] %>"><%#Container.DataItem.ToString() %></a></li>
          </ItemTemplate>
      </asp:Repeater>
    <li runat="server" id="liNext">
      <a href="commerceusers?pnum=<%=PageNumber+1 %>&srch=<%=Request["srch"] %>" aria-label="Next" >
        <span aria-hidden="true">&raquo;</span>
      </a>
    </li>
  </ul>
</nav>
    <script type="text/javascript">
        var search = function () {
            document.location = 'commerceusers?srch=' + $('#srch').val();
        }
        $("#srch").keyup(function (e) {
            if (e.keyCode == 13) {
                search();
            }
        });
    </script>
</asp:Content>

