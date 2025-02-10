<%@ page title="" language="C#" masterpagefile="~/Admin.master" autoeventwireup="true" CodeFile="CommerceTransactionAdd.aspx.cs" inherits="CommerceTransactionAdd" %>

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
    <br />
 <div class="form-group">
    <label for="exampleInputEmail1">Amount</label>
    <ken:TextBox runat="server" CssClass="form-control" id="txtAmount" NumbersOnly="true" Required="true" placeholder="AED" ValidationGroup="transaction"></ken:TextBox>
  </div>
  <div class="form-group">
    <label for="exampleInputPassword1">Date</label>
    <ken:DatePicker runat="server" ID="dpDate" CssClass="form-control" ></ken:DatePicker>
  </div>
    <div class="form-group">
    <label for="exampleInputEmail1">Transaction</label>
    <asp:DropDownList runat="server" ID="ddlTransType" CssClass="form-control"></asp:DropDownList>
  </div>
    <div class="form-group">
    <label for="exampleInputEmail1">Notes</label>
    <ken:TextBox runat="server" CssClass="form-control" id="txtNotes" TextMode="MultiLine"></ken:TextBox>
  </div>

  <ken:Button runat="server" type="submit" ID="btnSave" CssClass="btn btn-default" Text="Save"  ValidationGroup="transaction" OnClick="BtnSave_Click"/>
</asp:Content>

