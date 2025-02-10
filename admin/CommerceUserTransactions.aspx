<%@ page title="" language="C#" masterpagefile="~/Admin.master" autoeventwireup="true" CodeFile="CommerceUserTransactions.aspx.cs" inherits="CommerceUserTransactions" %>

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
   <h3 class="col-md-6"><asp:Literal runat="server" ID="ltlUserName"></asp:Literal></h3>
    <h3 class="col-md-5">Total Balance: AED <asp:Literal runat="server" ID="ltlBalance"></asp:Literal></h3>
        <h3 class="col-md-1"><a href="commerceusers">Back</a></h3>
        </div>
    <div class="row">
<div class="btn-group pull-left col-md-6" >
  <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    Action <span class="caret"></span>
  </button>
  <ul class="dropdown-menu">
      <li><a href="CommerceTransactionAdd?uid=<%=Request["id"]%>">Add Transaction</a></li>
  </ul>
</div>
    <div class="pull-right col-md-6">
  </div>
        </div>
    <br />
    <table class="table table-striped">
        <tr><th>Date</th><th>Transaction Type</th><th>Amount</th><th>Payment ID</th><th>Remarks</th><th>Extra Info</th></tr>
        <asp:Repeater runat="server" ID="repTransactions">
            <ItemTemplate>
            <tr class="<%#((CMS.TransactionType)DataBinder.Eval(Container.DataItem,"TranType"))==CMS.TransactionType.Credit||((CMS.TransactionType)DataBinder.Eval(Container.DataItem,"TranType"))==CMS.TransactionType.Payment||((CMS.TransactionType)DataBinder.Eval(Container.DataItem,"TranType"))==CMS.TransactionType.Cancel?"success":"danger" %>"><td><%#((DateTime)DataBinder.Eval(Container.DataItem,"date")).ToString("dd/MM/yyyy") %></td><td><%#((CMS.TransactionType)DataBinder.Eval(Container.DataItem,"TranType")).ToString() %></td><th>AED <%#DataBinder.Eval(Container.DataItem,"Amount") %></th><td><%#DataBinder.Eval(Container.DataItem,"PaymentID") %></td><td><%#DataBinder.Eval(Container.DataItem,"remarks") %></td><td><%#DataBinder.Eval(Container.DataItem,"extrainfo") %></td></tr>
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

