<%@ page title="" language="C#" masterpagefile="~/Admin.master" autoeventwireup="true" CodeFile="ReportInventory.aspx.cs" inherits="ReportInventory" %>

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
    <!-- Single button -->
    <div class="row">
<div class="btn-group pull-left col-md-6" >
  <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    Action <span class="caret"></span>
  </button>
  <ul class="dropdown-menu">
  </ul>
</div>
    <div class="pull-right col-md-6">
    <div class="input-group">
      <ken:DatePicker runat="server" ID="minDate" placeholder="from date" cssclass="form-control"></ken:DatePicker>
        <ken:DatePicker runat="server" ID="maxDate" placeholder="to date" cssclass="form-control"></ken:DatePicker>
      <span class="input-group-btn">
        <ken:Button runat="server" ID="btnSearch" CssClass="btn btn-default" Text="Filter" OnClick="BtnSearch_Click" />
      </span>
    </div><!-- /input-group -->
  </div>
        </div>
    <br />
    <table class="table table-striped">
        <tr><th>Item ID</th><th>Item Name</th><th>Category</th><th>Brand</th><th>Qty</th><th>Selling Price</th><th>Moving Average</th></tr>
        <asp:Repeater runat="server" ID="repOrders">
            <ItemTemplate>
            <tr><td><a href="default?id=<%#DataBinder.Eval(Container.DataItem,"ID") %>"><%#DataBinder.Eval(Container.DataItem,"ID") %></a></td><td><%#DataBinder.Eval(Container.DataItem,"Name") %></td><td><%#DataBinder.Eval(Container.DataItem,"catName") %></td><th><%#DataBinder.Eval(Container.DataItem,"BrandName") %></th><td><%#DataBinder.Eval(Container.DataItem,"calculatedqty") %></td><td><%#DataBinder.Eval(Container.DataItem,"price") %></td><td><%#DataBinder.Eval(Container.DataItem,"movingaverage") %></td></tr>
        </ItemTemplate>
        </asp:Repeater>
</table>
    <nav>
  <ul class="pagination pull-right">
    <li runat="server" id="liPrev">
      <a href="reportinventory?pnum=<%=PageNumber-1 %>&minStat=<%=Request["minStat"] %>&maxStat=<%=Request["maxStat"] %>&minDate=<%=Request["minDate"] %>&maxDate=<%=Request["maxDate"] %>" aria-label="Previous">
        <span aria-hidden="true">&laquo;</span>
      </a>
    </li>
      <asp:Repeater runat="server" ID="repPages">
          <ItemTemplate>
              <li <%#(int)Container.DataItem==PageNumber?"class=\"active\"":"" %>><a href="reportinventory?pnum=<%#Container.DataItem.ToString() %>&minStat=<%=Request["minStat"] %>&maxStat=<%=Request["maxStat"] %>&minDate=<%=Request["minDate"] %>&maxDate=<%=Request["maxDate"] %>"><%#Container.DataItem.ToString() %></a></li>
          </ItemTemplate>
      </asp:Repeater>
    <li runat="server" id="liNext">
      <a href="reportinventory?pnum=<%=PageNumber+1 %>&minStat=<%=Request["minStat"] %>&maxStat=<%=Request["maxStat"] %>&minDate=<%=Request["minDate"] %>&maxDate=<%=Request["maxDate"] %>" aria-label="Next" >
        <span aria-hidden="true">&raquo;</span>
      </a>
    </li>
  </ul>
</nav>
    <script type="text/javascript">
        var goToOrder = function () {
            document.location = 'orderdetails?id=' + $('#orderNo').val();
        }
        $("#orderNo").keyup(function (e) {
            if (e.keyCode == 13) {
                goToOrder();
            }
        });
    </script>
</asp:Content>

