<%@ Page title="" language="C#" masterpagefile="~/Admin.master" autoeventwireup="true" CodeFile="OrderDetails.aspx.cs" inherits="OrderDetails" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHead" Runat="Server">
    <!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">

<!-- Optional theme -->
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap-theme.min.css">

<!-- Latest compiled and minified JavaScript -->
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
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
 
<section>
    <div class="second-page-container">
        <div class="block">
            <div class="container">
                   <div class="btn-group pull-right">
  <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
    Action <span class="caret"></span>
  </button>
  <ul class="dropdown-menu">
      <li><a runat="server" id="lnkPackOrder" data-toggle="modal" data-target="#packModal" visible="false">Order Packed</a></li>
      <li><a runat="server" id="lnkShipped" onserverclick="lnkShipped_ServerClick"  visible="false" onclick="return confirm('Has order been shipped?')" >Ship Order</a></li>
      <li><a runat="server" id="lnkComplete" onserverclick="lnkComplete_ServerClick"  visible="false" onclick="return confirm('Did you receive the money and complete the order?')" >Complete Order</a></li>
      <li role="separator" class="divider"></li>
      <li><a href="orderedit?id=<%=Request["id"] %>">Edit Order</a></li>
      <li><a runat="server" id="lnkRefund" onserverclick="lnkRefund_ServerClick" onclick="return confirm('are you sure you want to refund this order?')" >Refund Order</a></li>
      <li><a runat="server" id="lnkCancel" onserverclick="lnkCancel_ServerClick" onclick="return confirm('are you sure you want to cancel this order?')" >Cancel Order</a></li>
      <li role="separator" class="divider"></li>
      <li><a href="orderexport?id=<%=Request["id"] %>" target="_blank">Export</a></li>
  </ul>
</div>
                <div class="header-for-light">
                    <h1 class="wow fadeInRight animated" data-wow-duration="1s">Order <span>Details</span></h1>
                </div>
                <div class="row">
                    <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="block-form box-border wow fadeInLeft animated" data-wow-duration="1s">
                            <h3> <i class="fa fa-info"></i>Order # <asp:Literal runat="server" ID="ltlOrderID"></asp:Literal></h3>
                            <p>Order date: <asp:Literal runat="server" ID="ltlOrderDate"></asp:Literal></p>
                            <p>Customer: <asp:Literal runat="server" ID="ltlOwnerName"></asp:Literal></p>
                            <p>Email: <asp:Literal runat="server" ID="ltlEmail"></asp:Literal></p>
                            <hr>
                            <div>Status: <strong><asp:Literal runat="server" ID="ltlStatus"></asp:Literal></strong></div>
                            <br/>
                            <h3> <i class="fa fa-info"></i>Items</h3>
                            <table class="cart-table table wow fadeInLeft" data-wow-duration="1s">
                                <thead>
                                    <tr>
                                        <th class="card_product_image">Image</th>
                                        <th class="card_product_name">Product Name</th>
                                        <th class="card_product_quantity">Quantity</th>
                                        <th class="card_product_price">Unit Price</th>
                                        <th class="card_product_total">Total</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="repItems">
                                        <ItemTemplate>
                                        <tr>
                                            <td class="card_product_image" data-th="Image"><a href="../shop/<%#DataBinder.Eval(Container.DataItem,"Item.ProductID") %>/<%#((string)DataBinder.Eval(Container.DataItem,"Name")).Replace(" ","-").Replace("/","").Replace("&","-").Replace("+","").Replace(".","").Replace(",","")%>"><img src="../content/products/<%#DataBinder.Eval(Container.DataItem,"ThumbURL") %>_400x500.jpg"></a></td>
                                            <td class="card_product_name" ><a href="../shop/<%#DataBinder.Eval(Container.DataItem,"Item.ProductID") %>/<%#((string)DataBinder.Eval(Container.DataItem,"Name")).Replace(" ","-").Replace("/","").Replace("&","-").Replace("+","").Replace(".","").Replace(",","")%>"><%#DataBinder.Eval(Container.DataItem,"Name") %></a><br /><%#DataBinder.Eval(Container.DataItem,"Notes") %></td>
                                            <td class="card_product_quantity" data-th="Quantity">
                                                <%#DataBinder.Eval(Container.DataItem,"Quantity") %>
                                                &nbsp;
                                                &nbsp;<a href="#"><i class="icon-trash icon-large"></i> </a>
                                            </td>
                                            <td class="card_product_price" data-th="Unit Price">AED <%#DataBinder.Eval(Container.DataItem,"UnitPrice") %></td>
                                            <td class="card_product_total" data-th="Total">AED <span id=""><%#DataBinder.Eval(Container.DataItem,"TotalPrice") %></span></td>
                                        </tr>
                                       </ItemTemplate>
                                    </asp:Repeater>

                                </tbody>
                            </table>
                        </div>
                    </article>
                    <article class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <div class="block-order-total box-border wow fadeInRight" data-wow-duration="1s">
                            <h3><i class="fa fa-building"></i> Address</h3>
                            <hr>
                            <ul class="list-unstyled">
                                <li><asp:Literal runat="server" ID="ltlAddress1"></asp:Literal></li>
                                <li><asp:Literal runat="server" ID="ltlAddress2"></asp:Literal></li>
                                <li><asp:Literal runat="server" ID="ltlCity"></asp:Literal> , <asp:Literal runat="server" ID="ltlCountry"></asp:Literal></li>
                                <li>Postal: <asp:Literal runat="server" ID="ltlPostal"></asp:Literal></li>
                                <li><asp:Literal runat="server" ID="ltlPhone1"></asp:Literal></li>
                                <li><asp:Literal runat="server" ID="ltlPhone2"></asp:Literal></li>
                                
                            </ul>
                            <a target="_blank" runat="server" id="lnkTrack" visible="false">Track Shipment</a>
                        </div>
                    </article>
                    <article class="col-xs-12 col-sm-12 col-md-6 col-lg-6">
                        <div class="block-order-total box-border wow fadeInRight" data-wow-duration="1s">
                            <h3><i class="fa fa-dollar"></i> Total</h3>
                            <hr>
                            <ul class="list-unstyled">
                                <li>Payment Method: <strong><asp:Literal runat="server" ID="ltlPayment"></asp:Literal></strong></li>
                                <li>Items Count: <strong><asp:Literal runat="server" ID="ltlItemCount"></asp:Literal></strong></li>
                                <li>Total: <strong>JOD <asp:Literal runat="server" ID="ltlOrderTotal"></asp:Literal></strong></li>
                            </ul>
                        </div>
                        <a href="orders" class="btn-default-1 grey">Back To Orders</a>
                    </article>
                </div>
            </div>
        </div>
    </div>
</section>

    <!-- Tracking NO Modal -->
<div class="modal fade" id="packModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Tracking Number</h4>
      </div>
      <div class="modal-body">
        <ken:TextBox runat="server" ID="txtTracking" Required="true" ValidationGroup="tracking"></ken:TextBox>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <asp:Button runat="server" ID="btnSaveTracking" type="button" CssClass="btn btn-primary" OnClick="btnSaveTracking_Click" ValidationGroup="tracking" Text="Save & Packed"></asp:Button>
      </div>
    </div>
  </div>
</div>
</asp:Content>

