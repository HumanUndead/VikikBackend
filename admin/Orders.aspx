<%@ page title="" language="C#" masterpagefile="~/Admin.master" autoeventwireup="true" CodeFile="Orders.aspx.cs" inherits="Orders" %>

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
      <li><a href="Orders?min=<%=CMS.CartStatus.New.ToString("d") %>&max=<%=CMS.CartStatus.Paid.ToString("d") %>">New Orders</a></li>
      <li><a href="Orders?min=<%=CMS.CartStatus.ReadyToShip.ToString("d") %>&max=<%=CMS.CartStatus.ReadyToShip.ToString("d") %>">Pending Shipping</a></li>
      <li><a href="Orders?min=<%=CMS.CartStatus.Shipped.ToString("d") %>&max=<%=CMS.CartStatus.Shipped.ToString("d") %>">Shipped Orders</a></li>
      <li><a href="Orders?min=<%=CMS.CartStatus.Complete.ToString("d") %>&max=<%=CMS.CartStatus.Complete.ToString("d") %>">Completed Orders</a></li>
      <li><a href="Orders?min=<%=CMS.CartStatus.Cancelled.ToString("d") %>&max=<%=CMS.CartStatus.Cancelled.ToString("d") %>">Cancelled Orders</a></li>
      <li role="separator" class="divider"></li>
      <li><a href="OrdersExport?min=<%=Request["min"] %>&max=<%=Request["max"] %>&uid=<%=Request["uid"] %>" target="_blank">Export All</a></li>
  </ul>
</div>
    <div class="pull-right col-md-6">
    <div class="input-group">
      <input type="text" id="orderNo" class="form-control" placeholder="Order Number...">
      <span class="input-group-btn">
        <button class="btn btn-default" type="button" onclick="goToOrder()">Go!</button>
      </span>
    </div><!-- /input-group -->
  </div>
        </div>
    <br />
    <table class="table table-striped">
        <tr><th>Order #</th><th>Order Date</th><th>Customer Name</th><th>Amount</th><th>Payment Method</th><th>Status</th><th>Export</th></tr>
        <asp:Repeater runat="server" ID="repOrders">
            <ItemTemplate>
            <tr><td><a href="orderdetails?id=<%#DataBinder.Eval(Container.DataItem,"OrderID") %>"><%#DataBinder.Eval(Container.DataItem,"OrderID") %></a></td><td><%#((DateTime)DataBinder.Eval(Container.DataItem,"OrderDate")).ToString("dd/MM/yyyy HH:mm") %></td><td><%#DataBinder.Eval(Container.DataItem,"OwnerName") %></td><th><%#DataBinder.Eval(Container.DataItem,"FormattedOrderTotal") %></th><td><%#Resources.CMS.ResourceManager.GetString(((CMS.DeliveryMethod)Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Delivery"))).ToString()) %></td><td><%#DataBinder.Eval(Container.DataItem,"Status") %></td><td><a target="_blank" href="orderexport?id=<%#DataBinder.Eval(Container.DataItem,"OrderID") %>">Export</a>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         </td></tr>
        </ItemTemplate>
        </asp:Repeater>
</table>
    <nav>
  <ul class="pagination pull-right">
    <li runat="server" id="liPrev">
      <a href="orders?pnum=<%=PageNumber-1 %>&min=<%=Request["min"] %>&max=<%=Request["max"] %>&uid=<%=Request["uid"] %>" aria-label="Previous">
        <span aria-hidden="true">&laquo;</span>
      </a>
    </li>
      <asp:Repeater runat="server" ID="repPages">
          <ItemTemplate>
              <li <%#(int)Container.DataItem==PageNumber?"class=\"active\"":"" %>><a href="orders?pnum=<%#Container.DataItem.ToString() %>&min=<%=Request["min"] %>&max=<%=Request["max"] %>&uid=<%=Request["uid"] %>"><%#Container.DataItem.ToString() %></a></li>
          </ItemTemplate>
      </asp:Repeater>
    <li runat="server" id="liNext">
      <a href="orders?pnum=<%=PageNumber+1 %>&min=<%=Request["min"] %>&max=<%=Request["max"] %>&uid=<%=Request["uid"] %>" aria-label="Next" >
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

