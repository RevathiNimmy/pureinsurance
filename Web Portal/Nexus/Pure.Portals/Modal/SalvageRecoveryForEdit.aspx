<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SalvageRecoveryForEdit.aspx.vb"
    Inherits="Nexus.Modal_SalvageRecoveryForEdit" MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">
        var NumberAsFloat = function (price) {

            // StartNumber.replace(/\,/g,&#8216;&#8216;);
            var value = price.replace(/\,/g, '');
            return parseFloat(value, 10);
        }
        function CalculateRecovery() {
            var initialReserve = document.getElementById('<%=txtInitialReserve.ClientID %>');
            var thisRevision = document.getElementById('<%=txtThisRevision.ClientID %>');
            var receipts = document.getElementById('<%=txtReceipts.ClientID %>');
            var reserveAfterReceipt = document.getElementById('<%=txtreserveAfterReceipt.ClientID %>');
            var reserveAfterRevision = document.getElementById('<%=txtReserveAfterRevision.ClientID %>');
            var revisedReserve = document.getElementById('<%=txtRevisedReserve.ClientID %>');
            var totalReserve = document.getElementById('<%=txtTotalReserve.ClientID %>');
            
            // Balance Reserve = Initial Reserve - Receipts (before revision)
            var balanceValue = NumberAsFloat(initialReserve.value) - NumberAsFloat(receipts.value);
            reserveAfterReceipt.value = FormatCurrency(isNaN(balanceValue) ? "0.00" : balanceValue.toFixed(2));
            
            // Reserve After Revision = Balance Reserve - This Revision (if negative, deduct; if positive, add)
            var thisRevisionValue = NumberAsFloat(thisRevision.value);
            var reserveAfterValue = balanceValue + thisRevisionValue + NumberAsFloat(revisedReserve.value);
            reserveAfterRevision.value = FormatCurrency(isNaN(reserveAfterValue) ? "0.00" : reserveAfterValue.toFixed(2));
            
            // Total Reserve = Initial Reserve + This Revision (consider sign entered in text box)
            var totalValue = NumberAsFloat(initialReserve.value) + thisRevisionValue + NumberAsFloat(revisedReserve.value)
            totalReserve.value = FormatCurrency(isNaN(totalValue) ? "0.00" : totalValue.toFixed(2));
            
            // Format This Revision field
            var revisionValue = new Number(thisRevision.value);
            if (revisionValue.toFixed(2) != 'NaN') {
                thisRevision.value = FormatCurrency(revisionValue.toFixed(2));
            }
        }

        function FormatCurrency(value) {
            var parts = value.toString().split(".");
            return parts[0].replace(/\B(?=(\d{3})+(?=$))/g, ",") + (parts[1] ? "." + parts[1] : "");
        }

        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "0123456789.-";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
        }

    </script>

    <div id="Modal_SalvageRecoveryForEdit">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text=""></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblSalvageRecoveryReserve" runat="server" Text=""></asp:Label></legend>
                    <div class="row">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblRecoveryType" runat="server" AssociatedControlID="ddlRecoveryType" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltRecoveryType" runat="server" Text="<%$ Resources:lbl_RecoveryType%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlRecoveryType" runat="server" Enabled="false" CssClass="form-control form-select">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPartyType" runat="server" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltPartyType" runat="server" Text="<%$ Resources:lbl_PartyType%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:Label ID="lblPartyTypeValue" runat="server" CssClass="form-control"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblParty" runat="server" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltParty" runat="server" Text="<%$ Resources:lbl_Party%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:Label ID="lblPartyValue" runat="server" CssClass="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblInitialReserve" runat="server" AssociatedControlID="txtInitialReserve" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltInitialReserve" runat="server" Text="<%$ Resources:lbl_InitialReserve%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtInitialReserve" runat="server" CssClass="form-control form-control" MaxLength="15"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblRevisedReserve" runat="server" AssociatedControlID="txtRevisedReserve" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltRevisedReserve" runat="server" Text="<%$ Resources:lbl_RevisedReserve%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtRevisedReserve" runat="server" CssClass="form-control form-control" MaxLength="15"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblReceipts" runat="server" AssociatedControlID="txtReceipts" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltReceipts" runat="server" Text="<%$ Resources:lbl_Receipts%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtReceipts" runat="server" CssClass="form-control form-control" MaxLength="15" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblTotalReserve" runat="server" AssociatedControlID="txtTotalReserve" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltTotalReserve" runat="server" Text="<%$ Resources:lbl_TotalReserve%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtTotalReserve" runat="server" CssClass="form-control form-control" MaxLength="15" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblreserveAfterReceipt" runat="server" AssociatedControlID="txtreserveAfterReceipt" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltreserveAfterReceipt" runat="server" Text="<%$ Resources:lbl_reserveAfterReceipt%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtreserveAfterReceipt" runat="server" CssClass="form-control form-control" MaxLength="15" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblThisRevision" runat="server" AssociatedControlID="txtThisRevision" Text="<%$ Resources:lbl_ThisRevision%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtThisRevision" runat="server" CssClass="form-control field-mandatory form-control" MaxLength="15"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblReserveAfterRevision" runat="server" AssociatedControlID="txtReserveAfterRevision" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltReserveAfterRevision" runat="server" Text="<%$ Resources:lbl_ReserveAfterRevision%>"></asp:Literal>
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtReserveAfterRevision" runat="server" CssClass="form-control form-control" MaxLength="15" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </div>
        <asp:CustomValidator ID="CustValidType" runat="server" ControlToValidate="txtThisRevision" SetFocusOnError="true" Display="none"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
