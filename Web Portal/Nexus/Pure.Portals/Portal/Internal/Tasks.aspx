<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Tasks.aspx.vb" Inherits="Portal_Tasks" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:content id="cntMainBody" contentplaceholderid="cntMainBody" runat="Server">
    <div class="tasks">
        
    
            
        
                
                
            
                    
                    <div class="card">
                        <div class="card-body clearfix">
                        
                            <div class="form-horizontal">
                                <legend><span>Select Task </span></legend>
                                
                            
                                    <div class="first form-group form-group-sm col-lg-6 col-md-6 col-sm-12" id="liNewClient" runat="server">
                                        <asp:HyperLink ID="hypAddClient1" Target="_parent" CausesValidation="false" NavigateUrl="~/secure/agent/PersonalClientDetails.aspx?mode=add" runat="server">New Client</asp:HyperLink>
                                    </div>
                                    <div id="liFindClient" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypFindClient" Target="_parent" CausesValidation="false" NavigateUrl="~/secure/agent/FindClient.aspx" runat="server">Find Client</asp:HyperLink>
                                    </div>
                                     <div id="liBrokerView" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypBrokerView" Target="_parent" CausesValidation="false" NavigateUrl="~/secure/BrokerView.aspx" runat="server">Broker View</asp:HyperLink>
                                    </div>
                                    <div id="liClaimIntimation" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypClaimIntimation" Target="_parent" CausesValidation="false" EnableViewState="false" runat="server" NavigateUrl="~/Claims/FindInsuranceFile.aspx">Open Claim</asp:HyperLink>
                                    </div>
                                    <div id="liClaimSearch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypClaimSearch" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/Claims/FindClaim.aspx" runat="server">Claim Search</asp:HyperLink>
                                    </div>
                                    <div id="liClaimPaymentProcessing" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypClaimPaymentProcessing" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/payment/claimpaymentprocessing.aspx" runat="server">Claim Payment Processing</asp:HyperLink>
                                    </div>
                                    <div id="liAuthoriseclaimpayments" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypAuthoriseclaimpayments" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/Authoriseclaimpayments.aspx" runat="server">Authorise Claim Payments</asp:HyperLink>
                                    </div>
                                    <div id="liCashChequePayment" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypCashChequePayment" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/payment/CashList.aspx?Mode=Payment" runat="server">Cash Cheque Payment</asp:HyperLink>
                                    </div>
                                    <div id="liCoverNoteManagement" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypCoverNoteManagement" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/FindCoverNoteBook.aspx" runat="server">Cover Note Management</asp:HyperLink>
                                    </div>
                                    <div id="liWorkManager" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypWorkManager" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/workmanager.aspx" runat="server">Work Manager</asp:HyperLink>
                                    </div>
                                    <div id="liEventList" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypEventList" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/EventList.aspx" runat="server">Event List</asp:HyperLink>
                                    </div>
                                    <div id="liBankGuarantee" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypBankGuarantee" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/BankGuarantee.aspx" runat="server">Bank Guarantee</asp:HyperLink>
                                    </div>
                                    <div id="liSearchTransactions" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypSearchTransactions" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/SearchTransactions.aspx" runat="server">Search Transactions</asp:HyperLink>
                                    </div>
                                    
                                    <div id="liAuthorisePayments" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="hypAuthorisePayments" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/payment/CashListItem.aspx" runat="server">Authorise Payments</asp:HyperLink>
                                    </div>
                                    <div id="liCashChequeReceipts" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkCashChequeReceipts" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/payment/CashList.aspx?Mode=Receipt" runat="server">Cash Cheque Receipts</asp:HyperLink>
                                    </div>
                                    <div id="liRenewalManager" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkRenewalManager" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/RenewalManager.aspx" runat="server">Renewal Manager</asp:HyperLink>
                                    </div>
                                    <div id="liRenewalSelection" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkRenewalSelection" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/agent/RenewalSelection.aspx" runat="server">Renewal Selection</asp:HyperLink>
                                    </div>
                                    <div id="liInsurerPayment" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkInsurerPayment" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/InsurerPayments.aspx" runat="server">Insurer Payments</asp:HyperLink>
                                    </div>
                                    <div id="liFindReceipts" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkFindReceipts" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/FindReceipts.aspx" runat="server">Find Receipts</asp:HyperLink>
                                    </div>
                                    <div id="liQuoteCollection" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkQuoteCollection" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/QuoteCollection.aspx" runat="server">Quote Collection</asp:HyperLink>
                                    </div>
                                    <div id="liCashDeposit" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkCashDeposit" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/FindCDAccount.aspx" runat="server">Find Cash Deposit Account</asp:HyperLink>
                                    </div>
                                    <div id="li1" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkCoverNoteBook" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/FindCoverNoteBook.aspx" runat="server">Manage Cover Note Book</asp:HyperLink>
                                    </div>
                                    <div id="li2" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkSalvage" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/Claims/FindClaim.aspx?Type=Salvage" runat="server">Salvage</asp:HyperLink>
                                    </div>
                                    <div id="li3" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkThirdPartyRecovery" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/Claims/FindClaim.aspx?Type=TPRecovery" runat="server">Third Party Recovery </asp:HyperLink>
                                    </div>
                                      <div id="li4" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkReports" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/reports.aspx" runat="server">Reports</asp:HyperLink>
                                    </div>
                                    <div id="liManualJournal" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:HyperLink ID="lnkManualJournal" Target="_parent" CausesValidation="false" EnableViewState="false" NavigateUrl="~/secure/ManualJournal.aspx" runat="server">Manual Journal</asp:HyperLink>
                                    </div>
                                </div>
                        </div>
                        
                    </div>
                </div>
</asp:content>
