<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="Instalments.aspx.vb" Inherits="Nexus.secure_payment_Instalments" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smInstalments" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">
        function funAccountType() {
            if (document.getElementById('<%=ddlAccountType.ClientID %>').value == '') //
            {
                document.getElementById('<%=txtBankName.ClientID %>').value = '';
                document.getElementById('<%=txtAddress1.ClientID %>').value = '';
                document.getElementById('<%=txtBranch.ClientID %>').value = '';
                document.getElementById('<%=txtBranchCode.ClientID %>').value = '';
                document.getElementById('<%=txtAccountName.ClientID %>').value = '';
                document.getElementById('<%=txtAccountNumber.ClientID %>').value = '';
            }
        }
        function ReceiveBankData(sContactData, sPostBackTo) {
            document.getElementById('<%= txtBankDetailData.ClientID %>').value = sContactData;
            __doPostBack(sPostBackTo, 'UpdateBank');
        }
    </script>
    
<asp:HiddenField ID="txtBankDetailData" runat="server"></asp:HiddenField>
    <div id="secure_payment_Installments">
        
    
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
            
        
                
                
            
                    
                    <asp:Panel ID="pnlInstallmentQuotes" runat="server" DefaultButton="btnNext">
                        <h1>
                            <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_DirectDebit_heading %>"></asp:Literal></h1>
                        <h2>
                            <asp:Literal ID="lblDirectDebitText" runat="server" Text="<%$ Resources:lbl_DirectDebit_text %>"></asp:Literal>
                            <asp:Literal ID="lblErrorMsg" runat="server" Text="<%$ Resources:lbl_ErrorMsg %>" Visible="false"></asp:Literal>
                        </h2>
                        <asp:Panel ID="pnlInstalmentType" runat="server" Visible="false">
                            <div class="fieldset-wrapper">
                                
                                <fieldset>
                                    <ol>
                                        <li class="doublewidth">
                                            <asp:RadioButtonList ID="rbInstalmentType" runat="server" AutoPostBack="true" CssClass="asp-radio">
                                                <asp:ListItem Text="<%$ Resources:lbl_InstalmentTypeSpreadOver %>" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:lbl_InstalmentTypeNextInstalment %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:lbl_InstalmentTypeNewPlan %>" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </li>
                                    </ol>
                                </fieldset>
                            </div>
                        </asp:Panel>
                        
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdInstallmentQuotes" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" DataKeyNames="SchemeNo,SchemeVersion,CompanyNo" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="SchemeName" SortExpression="SchemeName" HeaderText="<%$ Resources:lbl_Header_SchemeName %>"></asp:BoundField>
                                    <asp:BoundField DataField="FrequencyDescription" SortExpression="FrequencyDescription" HeaderText="<%$ Resources:lbl_Header_PaymentType %>"></asp:BoundField>
                                    <asp:BoundField DataField="MediaTypeDescription" SortExpression="MediaTypeDescription" HeaderText="<%$ Resources:lbl_Header_MediaType %>"></asp:BoundField>
                                    <nexus:BoundField DataField="DepositAmount" SortExpression="DepositAmount" HeaderText="<%$ Resources:lbl_Header_Deposit_Amt %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="Amount" SortExpression="Amount" HeaderText="<%$ Resources:lbl_Header_Amount %>" DataType="Currency"></nexus:BoundField>
                                    <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="<%$ Resources:lbl_Header_Type %>"></asp:BoundField>
                                    <asp:BoundField DataField="Terms" SortExpression="Terms" HeaderText="<%$ Resources:lbl_Header_Funding %>"></asp:BoundField>
                                    
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("SchemeNo") %>" Class="list-inline no-margin">
                                                    <li id="liSelect" runat="server">
                                                        <asp:LinkButton ID="lnkbtnSelect" Text="<%$ Resources:lbl_Select %>" runat="server" CausesValidation="False" CommandName="Select" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Panel ID="pnlPlanSummary" runat="server" Visible="false">
                            <h2>
                                Plan Summary</h2>
                                <div class="fieldset-wrapper" id="divChangeDate" runat="server">
                                <fieldset>
                                    <ol>
                                        <li>
                                        <asp:Label ID="lblDayinMonth" runat="server" AssociatedControlID="ddlDayinMonth" Text="<%$ Resources:lbl_DayinMonth %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlDayinMonth" runat="server" AutoPostBack="true" CssClass=" form-select">
                                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                                        </asp:DropDownList>
                                        </li>
                                        <li>
                                        <asp:Label ID="lblFirstPaymentDate" runat="server" AssociatedControlID="ddlFirstPaymentDate" Text="<%$ Resources:lbl_FirstPaymentDate %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlFirstPaymentDate" runat="server" AutoPostBack="true" CssClass=" form-select"></asp:DropDownList>
                                        </li>
                                        </ol>
                                        </fieldset>
                                </div>
                            <div class="md-whiteframe-z0 bg-white"><ul class="nav nav-lines nav-tabs b-danger">
                                    <li><a href="#tab-summary" data-bs-toggle="tab" aria-expanded="true" class="active">
                                        <asp:Literal ID="lbl_Tab_Summary" runat="server" Text="<%$ Resources:lbl_Tab_Summary %>"></asp:Literal></a></li>
                                    <li><a href="#tab-instalments" data-bs-toggle="tab" aria-expanded="true">
                                        <asp:Literal ID="lbl_Tab_Instalments" runat="server" Text="<%$ Resources:lbl_Tab_Instalments %>"></asp:Literal>
                                    </a></li>
                                    <li><a href="#tab-breakdown" data-bs-toggle="tab" aria-expanded="true">
                                        <asp:Literal ID="lbl_Tab_Breakdown" runat="server" Text="<%$ Resources:lbl_Tab_Breakdown %>"></asp:Literal></a></li>
                                    <li><a href="#tab-finance" data-bs-toggle="tab" aria-expanded="true">
                                        <asp:Literal ID="lbl_Tab_Finance_Details" runat="server" Text="<%$ Resources:lbl_Tab_Finance_Details %>"></asp:Literal></a></li>
                                    <li><a href="#tab-deposit" data-bs-toggle="tab" aria-expanded="true">
                                        <asp:Literal ID="lbl_Tab_Deposit" runat="server" Text="<%$ Resources:lbl_Tab_Deposit %>"></asp:Literal></a></li>
                                </ul><div class="tab-content clearfix p b-t b-t-2x">
                                
                                <div id="tab-summary" class="tab-pane animated fadeIn active" role="tabpanel">
                                    <div class="fieldset-wrapper">
                                        
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblInstalmentQuteSummary" runat="server" Text="<%$ Resources:lbl_Title_Summary %>"></asp:Label></legend>
                                            <ol>
                                                <li>
                                                    <asp:Label ID="lblFinancedAmount" runat="server" AssociatedControlID="txtFinancedAmount" Text="<%$ Resources:lbl_Financed_Amount %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtFinancedAmount" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblTotalPayable" runat="server" AssociatedControlID="txtTotalPayable" Text="<%$ Resources:lbl_Total_Payable %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTotalPayable" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblTransactions" runat="server" AssociatedControlID="txtTransactions" Text="<%$ Resources:lbl_Transactions %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTransactions" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblInstallements" runat="server" AssociatedControlID="txtInstallements" Text="<%$ Resources:lbl_Installements %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtInstallements" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ Resources:lbl_Rate %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtRate" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblAPR" runat="server" AssociatedControlID="txtAPR" Text="<%$ Resources:lbl_APR %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtAPR" runat="server"></asp:TextBox>
                                                </li>
                                            </ol>
                                        </fieldset>
                                    </div>
                                    
                                </div>
                                <div id="tab-instalments" class="tab-pane animated fadeIn" role="tabpanel">
                                    <div class="fieldset-wrapper">
                                        
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblInstalments" runat="server" Text="<%$ Resources:lbl_Title_Instalments %>"></asp:Label></legend>
                                            <ol>
                                                <li>
                                                    <asp:Label ID="lblFirstInstalmentDate" runat="server" AssociatedControlID="txtFirstInstalmentDate" Text="<%$ Resources:lbl_First_Instalment_Date %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtFirstInstalmentDate" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblFirstInstalment" runat="server" AssociatedControlID="txtFirstInstalment" Text="<%$ Resources:lbl_First_Instalment %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtFirstInstalment" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblNextInstalment" runat="server" AssociatedControlID="txtNextInstalment" Text="<%$ Resources:lbl_Next_Instalment %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtNextInstalment" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblOtherInstalment" runat="server" AssociatedControlID="txtOtherInstalment" Text="<%$ Resources:lbl_Other_Instalment %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtOtherInstalment" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblLastInstalment" runat="server" AssociatedControlID="txtLastInstalment" Text="<%$ Resources:lbl_Last_Instalment %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtLastInstalment" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblTaxes" runat="server" AssociatedControlID="txtTaxes" Text="<%$ Resources:lbl_Taxes %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTaxes" runat="server"></asp:TextBox>
                                                </li>
                                            </ol>
                                        </fieldset>
                                    </div>
                                    
                                </div>
                                <div id="tab-breakdown" class="tab-pane animated fadeIn" role="tabpanel">
                                    <div class="fieldset-wrapper">
                                        
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblBreakDown" runat="server" Text="<%$ Resources:lbl_Title_Breakdown %>"></asp:Label></legend>
                                            <ol>
                                                <li>
                                                    <asp:Label ID="lblDeposit" runat="server" AssociatedControlID="txtDeposit" Text="<%$ Resources:lbl_Deposit %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtDeposit" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblAdminCharge" runat="server" AssociatedControlID="txtAdminCharge" Text="<%$ Resources:lbl_Admin_Charge %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtAdminCharge" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblProtectionCharge" runat="server" AssociatedControlID="txtProtectionCharge" Text="<%$ Resources:lbl_Protection_Charge %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtProtectionCharge" runat="server"></asp:TextBox>
                                                </li>
                                                <li>
                                                    <asp:Label ID="lblInterest" runat="server" AssociatedControlID="txtInterest" Text="<%$ Resources:lbl_Interest %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtInterest" runat="server"></asp:TextBox>
                                                </li>
                                            </ol>
                                        </fieldset>
                                    </div>
                                    
                                </div>
                                <div id="tab-finance" class="tab-pane animated fadeIn" role="tabpanel">
                                    <div class="fieldset-wrapper">
                                        
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblFinanceDetails" runat="server" Text="<%$ Resources:lbl_Title_Finance_Details %>"></asp:Label></legend>
                                            <ol>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblGrossDue" CssClass="doublewidth" runat="server" AssociatedControlID="txtGrossDue" Text="<%$ Resources:lbl_GrossDueFromClient %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtGrossDue" runat="server"></asp:TextBox>
                                                </li>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblTotalFees" CssClass="doublewidth" runat="server" AssociatedControlID="txtTotalFees" Text="<%$ Resources:lbl_TotalFeesExcludedForFinancing %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTotalFees" runat="server"></asp:TextBox>
                                                </li>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblTotalTaxes" CssClass="doublewidth" runat="server" AssociatedControlID="txtTotalTaxes" Text="<%$ Resources:lbl_TotalTaxesExcludedForFinancing %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTotalTaxes" runat="server"></asp:TextBox>
                                                </li>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblTotalAmount" CssClass="doublewidth" runat="server" AssociatedControlID="txtTotalAmount" Text="<%$ Resources:lbl_TotalAmountAbleToFinanced %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTotalAmount" runat="server"></asp:TextBox>
                                                </li>
                                            </ol>
                                        </fieldset>
                                    </div>
                                    
                                </div>
                                <div id="tab-deposit" class="tab-pane animated fadeIn" role="tabpanel">
                                    <div class="fieldset-wrapper">
                                        
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblMinimumDepositRequired" runat="server" Text="<%$ Resources:lbl_Title_MinimumDepositRequired %>"></asp:Label></legend>
                                            <ol>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblTotalFeesCollect" CssClass="doublewidth" runat="server" AssociatedControlID="txtTotalFeesCollect" Text="<%$ Resources:lbl_TotalFeeDeposit %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTotalFeesCollect" runat="server"></asp:TextBox>
                                                </li>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblTotalTaxesCollect" CssClass="doublewidth" runat="server" AssociatedControlID="txtTotalTaxesCollect" Text="<%$ Resources:lbl_TotalTaxesDeposit %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtTotalTaxesCollect" runat="server"></asp:TextBox>
                                                </li>
                                                <li class="doublewidth">
                                                    <asp:Label ID="lblMinimumDeposit" CssClass="doublewidth" runat="server" AssociatedControlID="txtMinimumDeposit" Text="<%$ Resources:lbl_MinimumDeposit %>"></asp:Label>
                                                    <asp:TextBox ReadOnly="true" ID="txtMinimumDeposit" runat="server"></asp:TextBox>
                                                </li>
                                            </ol>
                                        </fieldset>
                                    </div>
                                    
                                </div>
                            </div></div>
                            <asp:Panel runat="server" ID="pnlBankDetails" Visible="false">
                                <asp:UpdatePanel ID="upBankDetails" runat="server">
                                    <ContentTemplate>
                                        <div class="fieldset-wrapper">
                                            
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lblBenkDetails" runat="server" Text="<%$ Resources:lbl_BankDetails_heading %>"></asp:Label></legend>
                                                <ol>
                                                    <li>
                                                        <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="ddlAccountType" Text="<%$ Resources:lbl_AccountType %>"></asp:Label>
                                                        <asp:DropDownList ID="ddlAccountType" runat="server" AutoPostBack="true" onChange="funAccountType();" CssClass=" form-select"></asp:DropDownList>
                                                        <asp:CustomValidator ID="custValAccountType" runat="server" ControlToValidate="ddlAccountType" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>"></asp:CustomValidator>
                                                        <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" ControlToValidate="ddlAccountType" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>" InitialValue="" Enabled="false"></asp:RequiredFieldValidator>
                                                    </li>
                                                    <li id="liEditBank" runat="server">
                                                        <asp:LinkButton ID="hypBank" runat="server" Text="<%$ Resources:lkbtn_AddBank %>"></asp:LinkButton>
                                                        <asp:LinkButton ID="hypBankEdit" runat="server" Text="<%$ Resources:lkbtn_EditBank %>" Visible="false"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" Text="<%$ Resources:lbl_BankName %>"></asp:Label>
                                                        <asp:TextBox ReadOnly="true" ID="txtBankName" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="txtBankName" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_BankName %>" Enabled="false"></asp:RequiredFieldValidator>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblAddress1" runat="server" AssociatedControlID="txtAddress1" Text="<%$ Resources:lbl_BankAddressLine1 %>"></asp:Label>
                                                        <asp:TextBox ReadOnly="true" ID="txtAddress1" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtAddress1" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_Address %>" Enabled="false"></asp:RequiredFieldValidator>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="txtBranch" Text="<%$ Resources:lbl_BranchName %>"></asp:Label>
                                                        <asp:TextBox ReadOnly="true" ID="txtBranch" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="txtBranch" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_Branch %>" Enabled="false"></asp:RequiredFieldValidator>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="txtBranchCode" Text="<%$ Resources:lbl_BranchCode %>"></asp:Label>
                                                        <asp:TextBox ReadOnly="true" ID="txtBranchCode" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvBranchCode" runat="server" ControlToValidate="txtBranchCode" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_txtBranchCode %>" Enabled="false"></asp:RequiredFieldValidator>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="<%$ Resources:lbl_AccountName %>"></asp:Label>
                                                        <asp:TextBox ReadOnly="true" ID="txtAccountName" runat="server"></asp:TextBox>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="<%$ Resources:lbl_AccountNumber %>"></asp:Label>
                                                        <asp:TextBox ReadOnly="true" ID="txtAccountNumber" runat="server"></asp:TextBox>
                                                    </li>
                                                    
                                                </ol>
                                            </fieldset>
                                        </div>
                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <nexus:ProgressIndicator ID="piBankDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="upBankDetails" runat="server">
                                    <ProgressTemplate>
                                    </ProgressTemplate>
                                </nexus:ProgressIndicator>
                            </asp:Panel>
                        </asp:Panel>
                    </asp:Panel>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:lbl_btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                </div>
</asp:Content>
