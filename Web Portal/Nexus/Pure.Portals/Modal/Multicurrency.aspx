<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Multicurrency.aspx.vb" Inherits="Nexus.Modal_Multicurrency" MasterPageFile="~/default.master" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smMulticurrency" runat="server"></asp:ScriptManager>
    <div id="Modal_Multicurrency">
        <div class="card">
            <div class="card-heading">
                <h1><asp:Literal ID="lblTitle" runat="server" Text="Currency conversions"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblTransactionHeading" runat="server" Text="Transaction"></asp:Label>
                    </legend>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblTransactionCurrency" runat="server" AssociatedControlID="ddlTransactionCurrency" Text="Transaction Currency" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <NexusProvider:LookupList ID="ddlTransactionCurrency" runat="server" DataItemValue="Code" DataItemText="Description" 
                                                                Sort="ASC" ListType="PMLookup" ListCode="Currency" DefaultText=" " CssClass="field-mandatory form-control form-select" />
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="rqdTransactionCurrency" runat="server" ControlToValidate="ddlTransactionCurrency" ErrorMessage="Transaction Currency is required" Display="none" SetFocusOnError="true" ValidationGroup="CurrencyGroup"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Panel ID="pnlTransactionAmount" runat="server" CssClass="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblTransactionAmount" runat="server" AssociatedControlID="txtTransactionAmount" Text="Transaction Amount" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtTransactionAmount" runat="server" CssClass="form-control" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblDateOfExchange" runat="server" AssociatedControlID="txtDateOfExchange" Text="Date of Exchange" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDateOfExchange" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateOfExchange" runat="server" LinkedControl="txtDateOfExchange" HLevel="1"></uc1:CalendarLookup>
                                    </div>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="rqdDateOfExchange" runat="server" ControlToValidate="txtDateOfExchange" ErrorMessage="Date of Exchange is required" Display="none" SetFocusOnError="true" ValidationGroup="CurrencyGroup"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    <legend>
                        <asp:Label ID="lblBaseCurrencyHeading" runat="server" Text="Base Currency"></asp:Label>
                    </legend>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblBaseCurrency" runat="server" Text="Base Currency" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <NexusProvider:LookupList ID="ddlBaseCurrency" runat="server" DataItemValue="Code" DataItemText="Description" 
                                            Sort="ASC" ListType="PMLookup" ListCode="Currency" DefaultText=" " CssClass="field-mandatory form-control form-select" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblBaseCurrencyRate" runat="server" AssociatedControlID="txtBaseCurrencyRate" Text="Base Currency Rate" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtBaseCurrencyRate" runat="server" CssClass="field-mandatory form-control" AutoPostBack="true" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ID="rqdBaseCurrencyRate" runat="server" ControlToValidate="txtBaseCurrencyRate" ErrorMessage="Base Currency Rate is required" Display="none" SetFocusOnError="true" ValidationGroup="CurrencyGroup"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <asp:Panel ID="pnlBaseCurrencyAmount" runat="server">
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblBaseCurrencyAmount" runat="server" AssociatedControlID="txtBaseCurrencyAmount" Text="Base Currency Amount" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtBaseCurrencyAmount" runat="server" CssClass="form-control" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlAccountCurrencySection" runat="server">
                    <legend>
                        <asp:Label ID="lblAccountCurrencyHeading" runat="server" Text="Account Currency"></asp:Label>
                    </legend>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblAccountCurrency" runat="server" AssociatedControlID="ddlAccountCurrency" Text="Account Currency" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <NexusProvider:LookupList ID="ddlAccountCurrency" runat="server" DataItemValue="Code" DataItemText="Description" 
                                            Sort="ASC" ListType="PMLookup" ListCode="Currency" DefaultText=" " CssClass="field-mandatory form-control form-select" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblAccountCurrencyRate" runat="server" AssociatedControlID="txtAccountCurrencyRate" Text="Account Currency Rate" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAccountCurrencyRate" runat="server" CssClass="form-control" AutoPostBack="true" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlAccountCurrencyAmount" runat="server">
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblAccountCurrencyAmount" runat="server" AssociatedControlID="txtAccountCurrencyAmount" Text="Account Currency Amount" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAccountCurrencyAmount" runat="server" CssClass="form-control" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:Panel>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlSystemCurrencySection" runat="server">
                    <legend>
                        <asp:Label ID="lblSystemCurrencyHeading" runat="server" Text="System Currency"></asp:Label>
                    </legend>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblSystemCurrency" runat="server" AssociatedControlID="ddlSystemCurrency" Text="System Currency" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <NexusProvider:LookupList ID="ddlSystemCurrency" runat="server" DataItemValue="Code" DataItemText="Description" 
                                                Sort="ASC" ListType="PMLookup" ListCode="Currency" DefaultText=" " CssClass="field-mandatory form-control form-select" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblSystemCurrencyRate" runat="server" AssociatedControlID="txtSystemCurrencyRate" Text="System Currency Rate" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSystemCurrencyRate" runat="server" CssClass="form-control" AutoPostBack="true" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlSystemCurrencyAmount" runat="server">
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblSystemCurrencyAmount" runat="server" AssociatedControlID="txtSystemCurrencyAmount" Text="System Currency Amount" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSystemCurrencyAmount" runat="server" CssClass="form-control" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:Panel>
                    </asp:Panel>
                    
                    <asp:Panel ID="pnlLossCurrencySection" runat="server">
                    <legend>
                        <asp:Label ID="lblLossCurrencyHeading" runat="server" Text="Loss Currency"></asp:Label>
                    </legend>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblLossCurrency" runat="server" AssociatedControlID="ddlLossCurrency" Text="Loss Currency" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <NexusProvider:LookupList ID="ddlLossCurrency" runat="server" DataItemValue="Code" DataItemText="Description" 
                                                Sort="ASC" ListType="PMLookup" ListCode="Currency" DefaultText=" " CssClass="field-mandatory form-control form-select" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblLossCurrencyAmount" runat="server" AssociatedControlID="txtLossCurrencyAmount" Text="Loss Currency Amount" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtLossCurrencyAmount" runat="server" CssClass="form-control" onkeypress="return isNumeric(event)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:Panel>
                    
                    <legend>
                        <asp:Label ID="lblRateOverrideReasonHeading" runat="server" Text="Rate Override Reason"></asp:Label>
                    </legend>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group form-group-sm">
                                <asp:Label ID="lblReason" runat="server" AssociatedControlID="ddlReason" Text="Reason" class="col-sm-5 control-label"></asp:Label>
                                <div class="col-sm-7">
                                    <NexusProvider:LookupList ID="ddlReason" runat="server" DataItemValue="Code" DataItemText="Description" 
                                                    Sort="ASC" ListType="PMLookup" ListCode="Exchange_Rate_Override_Reason" DefaultText=" " CssClass="field-mandatory form-control form-select" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="OK" ValidationGroup="CurrencyGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="Please correct the following errors:" runat="server" ValidationGroup="CurrencyGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:CustomValidator ID="vldInvalidCurrency" runat="server" Display="None" ValidationGroup="CurrencyGroup"></asp:CustomValidator>
    </div>
    <script type="text/javascript">
        function isNumeric(e) {
            var key = e.which || e.keyCode;
            var val = e.target.value;
            if (key === 8 || key === 9 || key === 13 || key === 27 || (key >= 35 && key <= 39) || key === 46) return true;
            if (key === 46 && val.indexOf('.') === -1) return true;
            if (key >= 48 && key <= 57) return true;
            if (key >= 96 && key <= 105) return true;
            return false;
        }

        $(document).ready(function () {
            var initialDateValue = $('#<%= txtDateOfExchange.ClientID %>').val();
            $('#<%= txtDateOfExchange.ClientID %>').on('change', function () {
        if ($(this).val() !== initialDateValue) {
            initialDateValue = $(this).val();
            __doPostBack('<%= txtDateOfExchange.UniqueID %>', '');
        }
            });
        });
    </script>

</asp:Content>
