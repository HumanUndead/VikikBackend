<%@ page title="" language="C#" autoeventwireup="true" CodeFile="OrderPrint.aspx.cs" inherits="OrderPrint" %>

<html><head runat="server">
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
        @media print {
            .noprint {
                display:none;
            }
        }
    </style>
      </head>
    <body>
        <form runat="server"></form>
<section>
    <div class="second-page-container">
        <div class="block">
            <div class="container">
                <header><img src="images/logo.png" class="pull-left" /><h2 class="pull-right">Order Receipt</h2></header>
                <div class="row">
                    <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="block-form box-border wow fadeInLeft animated" data-wow-duration="1s">
                            <h3> <i class="fa fa-info"></i>Order # <asp:Literal runat="server" ID="ltlOrderID"></asp:Literal></h3>
                            <p>Order date: <asp:Literal runat="server" ID="ltlOrderDate"></asp:Literal></p>
                            <p>Shipment ID: <asp:Literal runat="server" ID="ltlShipment"></asp:Literal></p>
                            <p>Customer: <asp:Literal runat="server" ID="ltlOwnerName"></asp:Literal></p>
                            <p>Email: <asp:Literal runat="server" ID="ltlEmail"></asp:Literal></p>
                            <hr>
                            <br/>
                            <h3> <i class="fa fa-info"></i>Items</h3>
                            <table class="cart-table table wow fadeInLeft" data-wow-duration="1s">
                                <thead>
                                    <tr>
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
                                            <td class="card_product_name" ><%#DataBinder.Eval(Container.DataItem,"Name") %><br /><%#DataBinder.Eval(Container.DataItem,"Notes") %></td>
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
                                <li>Total: <strong>AED <asp:Literal runat="server" ID="ltlOrderTotal"></asp:Literal></strong></li>
                            </ul>
                        </div>
                        <a href="orders" class="btn-default-1 grey noprint">Back To Orders</a>
                    </article>
                </div>
            </div>
        </div>
    </div>
</section>
        <script>
            window.print();
        </script>
    </body>
</html>

