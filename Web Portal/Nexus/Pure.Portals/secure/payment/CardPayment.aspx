<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="CardPayment.aspx.vb" Inherits="Nexus.secure_payment_CardPayment" Title="Untitled Page" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smInstalments" runat="server">
    </asp:ScriptManager>
    <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
    <div id="secure_payment_CardPayment">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblTransactionHeading" runat="server" Text="<%$ Resources:lbl_CardPayment_heading %>"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <!--start of payment details-->
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="LblPaymentDetails" runat="server" Text="<%$ Resources:lbl_PaymentDetails_heading %>"></asp:Label></legend>
                    <asp:PlaceHolder ID="pnlCC" runat="server">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="LblPremiumHeading" runat="server" Text="<%$ Resources:lbl_Premium_heading %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <p class="form-control-static font-bold">
                                    <asp:Label runat="server" ID="LblPremium"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="LblChargesHeading" runat="server" Text="<%$ Resources:lbl_Charges_heading %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <p class="form-control-static font-bold">
                                    <asp:Label runat="server" ID="LblCharges"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="LblTotalHeading" runat="server" Text="<%$ Resources:lbl_TotalToPay_heading %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <p class="form-control-static font-bold">
                                    <asp:Label runat="server" ID="LblTotal" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="pnlDC" Visible="false" runat="server">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                            <asp:Label ID="LblDCTotalHeading" runat="server" Text="<%$ Resources:lbl_TotalToPay_heading %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <p class="form-control-static font-bold">
                                    <asp:Label runat="server" ID="LblDCTotal"></asp:Label>
                                </p>
                            </div>
                            <%--Total amount:--%>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <!--end of payment details-->
                <!--start of customer details-->
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="LblCustomerDetails" runat="server" Text="<%$ Resources:lbl_CustomerDetails_heading %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Lbl_title" runat="server" Text="<%$ Resources:lbl_Title %>" AssociatedControlID="txtTitle" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Lbl_forename" runat="server" Text="<%$ Resources:lbl_Forename %>" AssociatedControlID="txtForename" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="Txtforename" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="ReqForename" Display="none" runat="server" ErrorMessage="Forename" ControlToValidate="Txtforename" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Lbl_surname" runat="server" Text="<%$ Resources:lbl_Surname %>" AssociatedControlID="txtsurname" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="Txtsurname" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="ReqSurname" runat="server" Display="none" ErrorMessage="Surname" ControlToValidate="Txtsurname" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEmailAddress" runat="server" Text="<%$ Resources:lbl_Email %>" AssociatedControlID="txtEmailAddress" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="60" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTelephone" runat="server" Text="<%$ Resources:lbl_Telephone %>" AssociatedControlID="txtTelephone" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTelephone" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <!--end of customer details-->
                <%--Start of address details--%>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblAddressHeading" runat="server" Text="<%$ Resources:lbl_AddressDetails_heading %>"></asp:Label></legend>
                    <uc2:AddressCntrl ID="AddressCntrl" runat="server"></uc2:AddressCntrl>
                </div>
                <%--end of address details--%>
                <!--start of Card details-->
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="LblCardDetails" runat="server" Text="<%$ Resources:lbl_CardDetails_heading %>"></asp:Label></legend>

                    <asp:PlaceHolder ID="phldrDC" runat="server">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblDCCardType" runat="server" Text="<%$ Resources:lbl_CardType %>" AssociatedControlID="ddlCardTypeDC" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlCardTypeDC" runat="Server" CssClass="form-control form-select">
                                    <asp:ListItem>Maestro</asp:ListItem>
                                    <asp:ListItem>Solo</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phldrCC" runat="server" Visible="false">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblCCCardType" runat="server" Text="<%$ Resources:lbl_CardType %>" AssociatedControlID="ddlCardTypeCC" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlCardTypeCC" runat="Server" CssClass="form-control form-select">
                                    <asp:ListItem>Visa</asp:ListItem>
                                    <asp:ListItem>MasterCard</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_CCNumber" runat="server" Text="<%$ Resources:lbl_CardNumber %>" AssociatedControlID="txtCCNumber" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="TxtCCNumber" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_ExpiryDate" runat="server" Text="<%$ Resources:lbl_ExpiryDate %>" AssociatedControlID="ddlMonth" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-4 col-sm-5">
                            <asp:DropDownList ID="ddlMonth" runat="Server" CssClass="form-control form-select">
                                <asp:ListItem Value="01"></asp:ListItem>
                                <asp:ListItem Value="02"></asp:ListItem>
                                <asp:ListItem Value="03"></asp:ListItem>
                                <asp:ListItem Value="04"></asp:ListItem>
                                <asp:ListItem Value="05"></asp:ListItem>
                                <asp:ListItem Value="06"></asp:ListItem>
                                <asp:ListItem Value="07"></asp:ListItem>
                                <asp:ListItem Value="08"></asp:ListItem>
                                <asp:ListItem Value="09"></asp:ListItem>
                                <asp:ListItem Value="10"></asp:ListItem>
                                <asp:ListItem Value="11"></asp:ListItem>
                                <asp:ListItem Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control form-select">
                                <asp:ListItem Value="07"></asp:ListItem>
                                <asp:ListItem Value="08"></asp:ListItem>
                                <asp:ListItem Value="09"></asp:ListItem>
                                <asp:ListItem Value="10"></asp:ListItem>
                                <asp:ListItem Value="11"></asp:ListItem>
                                <asp:ListItem Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_CVVNum" runat="server" Text="<%$ Resources:lbl_SecurityCode %>" AssociatedControlID="txtCVVNum" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCVVNum" runat="server" MaxLength="3" CssClass="w-xs form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="pnlDebitCards" runat="server">
                    <div class="form-horizontal">
                        <legend><asp:Label ID="lblDebitCards" runat="server" Text="<%$ Resources:lbl_DebitCardsOnly %>"></asp:Label></legend>
                            
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblStartDate" runat="server" Text="<%$ Resources:lbl_StartDate %>" AssociatedControlID="ddlMonth2" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <asp:Label ID="lblMonth2" runat="server" Text="<%$ Resources:lblMonth %>" Style="display: none;" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-4 col-sm-5">
                                <asp:DropDownList ID="ddlMonth2" runat="server" CssClass="form-control form-select">
                                    <asp:ListItem Value="01"></asp:ListItem>
                                    <asp:ListItem Value="02"></asp:ListItem>
                                    <asp:ListItem Value="03"></asp:ListItem>
                                    <asp:ListItem Value="04"></asp:ListItem>
                                    <asp:ListItem Value="05"></asp:ListItem>
                                    <asp:ListItem Value="06"></asp:ListItem>
                                    <asp:ListItem Value="07"></asp:ListItem>
                                    <asp:ListItem Value="08"></asp:ListItem>
                                    <asp:ListItem Value="09"></asp:ListItem>
                                    <asp:ListItem Value="10"></asp:ListItem>
                                    <asp:ListItem Value="11"></asp:ListItem>
                                    <asp:ListItem Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 col-sm-4">
                                <asp:DropDownList ID="ddlYear2" runat="server" CssClass="form-control form-select">
                                    <asp:ListItem Value="07"></asp:ListItem>
                                    <asp:ListItem Value="08"></asp:ListItem>
                                    <asp:ListItem Value="09"></asp:ListItem>
                                    <asp:ListItem Value="10"></asp:ListItem>
                                    <asp:ListItem Value="11"></asp:ListItem>
                                    <asp:ListItem Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblIssueNum" runat="server" Text="<%$ Resources:lbl_IssueNumber %>" AssociatedControlID="txtIssueNum" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtIssueNum" runat="server" MaxLength="3" CssClass="w-xs form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <!--end of Card details-->
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnPay" runat="server" Text="Submit" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
