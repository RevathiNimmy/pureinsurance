<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FinancePlan.ascx.vb" Inherits="Controls_FinancePlan" %>

<asp:Panel ID="pnlPlanSummary" runat="server">
    <h2>Plan Summary – Plan Reference <asp:Literal ID="lbl_Plan_ref" runat="server" ></asp:Literal> </h2>
    <div class="md-whiteframe-z0 bg-white">
        <ul class="nav nav-lines nav-tabs b-danger">
            <li><a href="#tab-summary-FinancePlan" data-bs-toggle="tab" aria-expanded="true" class="active">
                <asp:Literal ID="lbl_Tab_Summary" runat="server" Text="<%$ Resources:lbl_Tab_Summary %>"></asp:Literal></a></li>
            <li><a href="#tab-instalments-FinancePlan" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbl_Tab_Instalments" runat="server" Text="<%$ Resources:lbl_Tab_Instalments %>"></asp:Literal>
            </a></li>
            <li><a href="#tab-breakdown-FinancePlan" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbl_Tab_Breakdown" runat="server" Text="<%$ Resources:lbl_Tab_Breakdown %>"></asp:Literal></a></li>
            <li><a href="#tab-finance-FinancePlan" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbl_Tab_Finance_Details" runat="server" Text="<%$ Resources:lbl_Tab_Finance_Details %>"></asp:Literal></a></li>
            <li><a href="#tab-deposit-FinancePlan" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbl_Tab_Deposit" runat="server" Text="<%$ Resources:lbl_Tab_Deposit %>"></asp:Literal></a></li>
            <li><a href="#tab-bank-details-FinancePlan" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbl_Tab_Bank_Details" runat="server" Text="<%$ Resources:lbl_Tab_Bank_Details %>"></asp:Literal></a></li>
            <li><a href="#tab-transaction-FinancePlan" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbl_Tab_Transaction" runat="server" Text="<%$ Resources:lbl_Tab_Transaction %>"></asp:Literal></a></li>
        </ul>
        <div class="tab-content clearfix p b-t b-t-2x">
            <div id="tab-summary-FinancePlan" class="tab-pane animated fadeIn active" role="tabpanel">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblInstalmentQuteSummary" runat="server" Text="<%$ Resources:lbl_Title_Summary %>"></asp:Label>
                        <asp:Label ID="lblschemeinfo" runat="server" Style="font-weight: normal; font-size: small"></asp:Label>
                    </legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFinancedAmount" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtFinancedAmount" Text="<%$ Resources:lbl_Financed_Amount %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" CssClass="form-control" ID="txtFinancedAmount" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalPayable" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTotalPayable" Text="<%$ Resources:lbl_Total_Payable %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTotalPayable" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTransactions" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTransactions" Text="<%$ Resources:lbl_Transactions %>"></asp:Label>

                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTransactions" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInstallements" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtInstallements" Text="<%$ Resources:lbl_Installements %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtInstallements" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRate" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtRate" Text="<%$ Resources:lbl_Rate %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtRate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAPR" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAPR" Text="<%$ Resources:lbl_APR %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtAPR" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatus" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtStatus" Text="<%$ Resources:lbl_Status %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtStatus" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

            </div>
            <div id="tab-instalments-FinancePlan" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblInstalments" runat="server" Text="<%$ Resources:lbl_Title_Instalments %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFirstInstalmentDate" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtFirstInstalmentDate" Text="<%$ Resources:lbl_First_Instalment_Date %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtFirstInstalmentDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFirstInstalment" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtFirstInstalment" Text="<%$ Resources:lbl_First_Instalment %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtFirstInstalment" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblNextInstalment" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtNextInstalment" Text="<%$ Resources:lbl_Next_Instalment %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtNextInstalment" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblOtherInstalment" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtOtherInstalment" Text="<%$ Resources:lbl_Other_Instalment %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtOtherInstalment" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLastInstalment" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtLastInstalment" Text="<%$ Resources:lbl_Last_Instalment %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtLastInstalment" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTaxes" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTaxes" Text="<%$ Resources:lbl_Taxes %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTaxes" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

            </div>
            <div id="tab-breakdown-FinancePlan" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblBreakDown" runat="server" Text="<%$ Resources:lbl_Title_Breakdown %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDeposit" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtDeposit" Text="<%$ Resources:lbl_Deposit %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtDeposit" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAdminCharge" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAdminCharge" Text="<%$ Resources:lbl_Admin_Charge %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtAdminCharge" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProtectionCharge" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtProtectionCharge" Text="<%$ Resources:lbl_Protection_Charge %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtProtectionCharge" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInterest" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtInterest" Text="<%$ Resources:lbl_Interest %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtInterest" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                </div>

            </div>
            <div id="tab-finance-FinancePlan" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblFinanceDetails" runat="server" Text="<%$ Resources:lbl_Title_Finance_Details %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblGrossDue" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtGrossDue" Text="<%$ Resources:lbl_GrossDueFromClient %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtGrossDue" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalFees" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTotalFees" Text="<%$ Resources:lbl_TotalFeesExcludedForFinancing %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTotalFees" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalTaxes" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTotalTaxes" Text="<%$ Resources:lbl_TotalTaxesExcludedForFinancing %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTotalTaxes" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalAmount" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTotalAmount" Text="<%$ Resources:lbl_TotalAmountAbleToFinanced %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTotalAmount" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                </div>

            </div>
            <div id="tab-deposit-FinancePlan" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="form-horizontal">


                    <legend>
                        <asp:Label ID="lblMinimumDepositRequired" runat="server" Text="<%$ Resources:lbl_Title_MinimumDepositRequired %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalFeesCollect" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTotalFeesCollect" Text="<%$ Resources:lbl_TotalFeeDeposit %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTotalFeesCollect" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalTaxesCollect" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTotalTaxesCollect" Text="<%$ Resources:lbl_TotalTaxesDeposit %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtTotalTaxesCollect" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblMinimumDeposit" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtMinimumDeposit" Text="<%$ Resources:lbl_MinimumDeposit %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtMinimumDeposit" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>

                </div>

            </div>
            <div id="tab-bank-details-FinancePlan" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="form-horizontal">


                    <legend>
                        <asp:Label ID="lblBenkDetails" runat="server" Text="<%$ Resources:lbl_BankDetails_heading %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccountType" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="ddlAccountType" Text="<%$ Resources:lbl_AccountType %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-control form-select" Enabled="false"></asp:DropDownList>
                        </div>

                        <asp:CustomValidator ID="custValAccountType" runat="server" ControlToValidate="ddlAccountType" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" ControlToValidate="ddlAccountType" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>" InitialValue="" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBankName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBankName" Text="<%$ Resources:lbl_BankName %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtBankName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress1" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAddress1" Text="<%$ Resources:lbl_BankAddressLine1 %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtAddress1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranch" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBranch" Text="<%$ Resources:lbl_BranchName %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtBranch" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranchCode" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBranchCode" Text="<%$ Resources:lbl_BranchCode %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtBranchCode" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccountName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccountName" Text="<%$ Resources:lbl_AccountName %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtAccountName" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccountNumber" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccountNumber" Text="<%$ Resources:lbl_AccountNumber %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtAccountNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBIC" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBIC" Text="<%$ Resources:lbl_BIC %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtBIC" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIBAN" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtIBAN" Text="<%$ Resources:lbl_IBAN %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ReadOnly="true" ID="txtIBAN" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>

                    </div>
                </div>

            </div>
            <div id="tab-transaction-FinancePlan" class="tab-pane animated fadeIn" role="tabpanel">
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdInstallmentQuotes" runat="server" GridLines="None" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="InstalmentNumber" HeaderText="<%$ Resources:grdinst_InstalmentNo %>"></asp:BoundField>
                            <asp:BoundField DataField="DueDate" HeaderText="<%$ Resources:grdinst_Duedate %>"></asp:BoundField>
                            <asp:BoundField DataField="PaymentDate" HeaderText="<%$ Resources:grdinst_PaymentDate %>"></asp:BoundField>
                            <asp:BoundField DataField="Amount" HeaderText="<%$ Resources:grdinst_Amount %>"></asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="<%$ Resources:grdinst_Status %>"></asp:BoundField>
                            <asp:BoundField DataField="Reason" HeaderText="<%$ Resources:grdinst_Reason %>"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>

        </div>
    </div>

</asp:Panel>
