<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DirectDebit.aspx.vb" Inherits="Nexus.secure_payment_DirectDebit"
    MasterPageFile="~/default.master" Title="Untitled Page" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <uc3:ProgressBar ID="uctProgressBar" runat="server"></uc3:ProgressBar>
    <div id="secure_payment_DirectDebit">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTransactionHeading" runat="server" Text="<%$ Resources:lbl_DirectDebit_heading %>"></asp:Literal></h1>
            </div>
            <asp:Literal ID="lblDirectDebitText" runat="server" Text="<%$ Resources:lbl_DirectDebit_text %>"></asp:Literal>
            <asp:Literal ID="lblErrorMsg" runat="server" Text="<%$ Resources:lbl_ErrorMsg %>" Visible="false"></asp:Literal>
            <div id="pnlPaymentDetails" class="card-body clearfix" runat="server">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lbl_Payment_heading %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="LblPremiumHeading" runat="server" Text="<%$ Resources:lbl_Premium_heading %>" AssociatedControlID="LblPremium" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal runat="server" ID="LblPremium"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="LblChargesHeading" runat="server" Text="<%$ Resources:lbl_Charges_heading %>" AssociatedControlID="LblCharges" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal runat="server" ID="LblCharges"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="LblTotalHeading" runat="server" Text="<%$ Resources:lbl_TotalToPay_heading %>" AssociatedControlID="LblTotal" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal runat="server" ID="LblTotal"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblnumberofinstallment" runat="server" Text="<%$ Resources:lbl_Installments_No %>" AssociatedControlID="LblInstallmentNo" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal runat="server" ID="LblInstallmentNo"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="LblInstallmentsHeading" runat="server" Text="<%$ Resources:lbl_Installments_heading %>" AssociatedControlID="LblInstallments" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal runat="server" ID="LblInstallments"></asp:Literal>
                    </div>
                </div>
                <!--end of payment details-->
                <!--start of customer details-->
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:lbl_CustomerDetails_heading %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_Title" runat="server" AssociatedControlID="txtTitle" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit_Title" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldTitle" ControlToValidate="txtTitle" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_Title %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_forename" runat="server" AssociatedControlID="txtForename" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit_forename" runat="server" Text="<%$ Resources:lbl_Forenames %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="Txtforename" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldforename" ControlToValidate="Txtforename" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_Forenames %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_surname" runat="server" AssociatedControlID="txtSurname" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit_surname" runat="server" Text="<%$ Resources:lbl_Surname %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="Txtsurname" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldsurname" ControlToValidate="Txtforename" runat="server" Display="none" ErrorMessage="<%$ Resources:Lbl_surname %>"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <!--end of customer details-->
                <%--Start of address details--%>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lbl_AddressDetails_heading %>"></asp:Label>
                    </legend>
                    <uc2:AddressCntrl ID="AddressCntrl" runat="server"></uc2:AddressCntrl>
                </div>
                <%--end of address details--%>
                <!--start of bank details-->
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:lbl_BankDetails_heading %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_SortCode" runat="server" AssociatedControlID="TxtSort1" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit_SortCode" runat="server" Text="<%$ Resources:lbl_SortCode %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="TxtSort1" runat="server" MaxLength="2" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="TxtSort2" runat="server" MaxLength="2" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox ID="TxtSort3" runat="server" MaxLength="2" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldsortcode1" ControlToValidate="TxtSort1" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_SortCodePartOne %>"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="vldsortcode2" ControlToValidate="TxtSort2" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_SortCodePartTwo %>"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="vldsortcode3" ControlToValidate="TxtSort3" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_SortCodePartThree %>"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustvldSortCode1" ControlToValidate="TxtSort1" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_SortCodeMessage %>"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustvldSortCode2" ControlToValidate="TxtSort2" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_SortCodeMessage %>"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustvldSortCode3" ControlToValidate="TxtSort3" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_SortCodeMessage %>"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_AccNumber" runat="server" AssociatedControlID="TxtAccNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_AccountNumber %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="TxtAccNumber" runat="server" MaxLength="9" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldTxtAccNumber" ControlToValidate="TxtAccNumber" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_AccountNumberWarning %>"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custvldAccountNumber" ControlToValidate="TxtAccNumber" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_AccountNumberRestriction %>"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_AccName" runat="server" AssociatedControlID="TxtAccName" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:lbl_AccountName %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="TxtAccName" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldTxtAccName" Display="none" ControlToValidate="TxtAccName" runat="server" ErrorMessage="<%$ Resources:lbl_AccountNameWarning %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_InceptionDate" runat="server" AssociatedControlID="txtInceptionDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit_InceptionDate" runat="server" Text="<%$ Resources:lbl_InceptionDate %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox runat="server" ID="txtInceptionDate" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="txtInceptionDate_uctCalendarLookup" runat="server" LinkedControl="txtInceptionDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="vldtxtInceptionDate" runat="server" ControlToValidate="txtInceptionDate" Display="None" SetFocusOnError="True" ErrorMessage="<%$ Resources:lbl_InceptiondateWarning %>"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custvldtxtInceptionDate" ControlToValidate="txtInceptionDate" Display="none" runat="server" ErrorMessage="<%$ Resources:lbl_InceptiondateRestriction %>"></asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_PaymentDate" runat="server" AssociatedControlID="ddlPaymentDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit_PaymentDate" runat="server" Text="<%$ Resources:lbl_PreferredPaymentDate %>"></asp:Literal><span class="required">*</span></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlPaymentDate" runat="Server" CssClass="form-control form-select">
                                <asp:ListItem Text="1"></asp:ListItem>
                                <asp:ListItem Text="2"></asp:ListItem>
                                <asp:ListItem Text="3"></asp:ListItem>
                                <asp:ListItem Text="4"></asp:ListItem>
                                <asp:ListItem Text="5"></asp:ListItem>
                                <asp:ListItem Text="6"></asp:ListItem>
                                <asp:ListItem Text="7"></asp:ListItem>
                                <asp:ListItem Text="8"></asp:ListItem>
                                <asp:ListItem Text="9"></asp:ListItem>
                                <asp:ListItem Text="10"></asp:ListItem>
                                <asp:ListItem Text="11"></asp:ListItem>
                                <asp:ListItem Text="12"></asp:ListItem>
                                <asp:ListItem Text="13"></asp:ListItem>
                                <asp:ListItem Text="14"></asp:ListItem>
                                <asp:ListItem Text="15"></asp:ListItem>
                                <asp:ListItem Text="16"></asp:ListItem>
                                <asp:ListItem Text="17"></asp:ListItem>
                                <asp:ListItem Text="18"></asp:ListItem>
                                <asp:ListItem Text="19"></asp:ListItem>
                                <asp:ListItem Text="20"></asp:ListItem>
                                <asp:ListItem Text="21"></asp:ListItem>
                                <asp:ListItem Text="22"></asp:ListItem>
                                <asp:ListItem Text="23"></asp:ListItem>
                                <asp:ListItem Text="24"></asp:ListItem>
                                <asp:ListItem Text="25"></asp:ListItem>
                                <asp:ListItem Text="26"></asp:ListItem>
                                <asp:ListItem Text="27"></asp:ListItem>
                                <asp:ListItem Text="28"></asp:ListItem>
                                <asp:ListItem Text="29"></asp:ListItem>
                                <asp:ListItem Text="30"></asp:ListItem>
                                <asp:ListItem Text="31"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Literal ID="lblReview" runat="server" Text="<%$ Resources:lbl_Review %>"></asp:Literal>
                <asp:LinkButton ID="btnPay" runat="server" Text="<%$ Resources:AppResources.strings, btn_Submit %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
