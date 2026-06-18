<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="ManualJournalItem.aspx.vb" Inherits="secure_ManualJournalItem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntScriptIncludes" ContentPlaceHolderID="ScriptIncludes" runat="server">

    <script type="text/javascript">
        function setAccount(sShortCode, shiddenShortCode, sAccountName, sPartyKey, sCurrencyCode) //setAccount
        {
            tb_remove();
            document.getElementById('<%= txtAccount.ClientId%>').value = unescape(sShortCode);
            document.getElementById('<%= hiddenAccountKey.ClientId%>').value = shiddenShortCode;
            document.getElementById('<%= hiddenAccountName.ClientId%>').value = sAccountName;
        }
        function UpdateBaseAmount() {
            if (!isNaN(document.getElementById('<%= txtAmount.ClientId %>').value)) {
                document.getElementById('<%= txtBaseAmount.ClientId %>').value = currDefaultFormat(document.getElementById('<%= txtCurrencyRate.ClientId %>').value * document.getElementById('<%= txtAmount.ClientId %>').value);
                document.getElementById('<%= txtAmount.ClientId %>').value = currDefaultFormat(document.getElementById('<%= txtAmount.ClientId %>').value);
            }
        }
        function currDefaultFormat(num_SI) {
            num_SI = num_SI.toString().replace(/\$|\,/g, '');
            num_SI = num_SI.replace('£', '');
            num_SI = num_SI.replace('%', '');

            if (isNaN(num_SI))
                num_SI = "0";
            sign = (num_SI == (num_SI = Math.abs(num_SI)));
            num_SI = Math.floor(num_SI * 100 + 0.50000000001);
            cents = num_SI % 100;
            num_SI = Math.floor(num_SI / 100).toString();
            if (cents < 10)
                //if not use . then it will go inside
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num_SI.length - (1 + i)) / 3) ; i++)
                num_SI = num_SI.substring(0, num_SI.length - (4 * i + 3)) + ',' +
    num_SI.substring(num_SI.length - (4 * i + 3));
            num_SI = (((sign) ? '' : '-') + num_SI + '.' + cents);
            return num_SI;
        }

        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            var postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'btnAdd') {
                $get(uprogQuotes).style.display = "block";
            }
        }

    </script>

</asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <div id="secure_ManualJournalItem">
        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="panelManuaJournalItem" runat="server" DefaultButton="btnOk">
                    <div class="card">
                        <div class="card-heading">
                            <h1>
                                <asp:Literal ID="lblPageHeader" runat="server" Text="<%$Resources:lblPageHeader %>"></asp:Literal></h1>
                        </div>
                        <div class="card-body clearfix">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblSearchFormLegend" runat="server" Text="<%$Resources:lblSearchFormLegend %>"></asp:Label></legend>


                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCurrency" AssociatedControlID="PMLookup_CurrencyType" runat="server" Text="<%$Resources:lblCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="PMLookup_CurrencyType" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="CURRENCY" CssClass="field-medium field-mandatory form-control" AutoPostBack="true" TabIndex="1"></NexusProvider:LookupList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="vldrqdCurrencyType" runat="server" ControlToValidate="PMLookup_CurrencyType" Display="None" ErrorMessage="<%$Resources:vldrqdCurrencyType %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccount" Text="<%$Resources:btnAccountCode %>" ID="lblbtnAccountCode"></asp:Label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="txtAccount" CssClass="field-mandatory form-control" TabIndex="3"></asp:TextBox><span class="input-group-btn"><asp:LinkButton ID="btnAccountCode" runat="server" OnClientClick="<%$Resources:btnAccountCodeScript %>" TabIndex="2" SkinID="btnModal">
                                                <i class="fa fa-search"></i>
                                               <span class="btn-fnd-txt">Account</span>  
                                            </asp:LinkButton></span>
                                        </div>
                                    </div>


                                    <asp:HiddenField ID="hiddenAccountKey" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hiddenAccountName" runat="server"></asp:HiddenField>
                                    <asp:RequiredFieldValidator ID="vldrqdAccount" runat="server" ControlToValidate="txtAccount" Display="None" ErrorMessage="<%$Resources:vldrqdAccount %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblAmount" AssociatedControlID="txtAmount" Text="<%$Resources:lblAmount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtAmount" onblur="<%$Resources:txtAmountScript %>" CssClass="field-mandatory form-control" TabIndex="4">0.00</asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="vldrqdAmount" runat="server" ControlToValidate="txtAmount" Display="None" ErrorMessage="<%$Resources:vldrqdAmount %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="custvldAmount" runat="server" ControlToValidate="txtAmount" OnServerValidate="custvldAmount_ServerValidate" ErrorMessage="<%$Resources:custvldAmount %>" SetFocusOnError="true" Display="None"></asp:CustomValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblCurrencyRate" AssociatedControlID="txtCurrencyRate" Text="<%$Resources:lblCurrencyRate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtCurrencyRate" TabIndex="5" CssClass="field-mandatory form-control"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="vldrqdCurrencyRate" runat="server" ControlToValidate="txtCurrencyRate" Display="None" ErrorMessage="<%$Resources:vldrqdCurrencyRate %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblBaseAmount" AssociatedControlID="txtBaseAmount" Text="<%$Resources:lblBaseAmount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtBaseAmount" TabIndex="6" CssClass="field-mandatory form-control">0.00</asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="vldrqdBaseAmount" runat="server" ControlToValidate="txtBaseAmount" Display="None" ErrorMessage="<%$Resources:vldrqdBaseAmount %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblAltReference" AssociatedControlID="txtAltReference" Text="<%$Resources:lblAltReference %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtAltReference" TabIndex="7" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblComment" AssociatedControlID="txtComment" Text="<%$Resources:lblComment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtComment" MaxLength="255" TabIndex="8" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblUnderwritingYear" AssociatedControlID="PMLookup_UnderwritingYear" runat="server" Text="<%$Resources:lblUnderwritingYear %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="PMLookup_UnderwritingYear" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Underwriting_Year" CssClass="field-medium form-control form-select" DefaultText=" " TabIndex="9"></NexusProvider:LookupList>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCostCentre" AssociatedControlID="PMLookup_CostCentre" runat="server" Text="<%$Resources:lblCostCentre %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="PMLookup_CostCentre" runat="server" DataItemText="Description" DataItemValue="Code" ListCode="CostCentre" DefaultText=" " ListType="PMLookup" CssClass="field-medium form-control form-select" TabIndex="10"></NexusProvider:LookupList>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblInsuranceRef" AssociatedControlID="txtInsuranceRef" Text="<%$Resources:lblInsuranceRef %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtInsuranceRef" TabIndex="11" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblPurchaseInvoiceNumber" AssociatedControlID="txtPurchaseInvoiceNumber" Text="<%$Resources:lblPurchaseInvoiceNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtPurchaseInvoiceNumber" TabIndex="13" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="server" ID="lblPONumber" AssociatedControlID="txtPONumber" Text="<%$Resources:lblPONumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox runat="server" ID="txtPONumber" TabIndex="12" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-footer">
                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="false" Text="<%$Resources:btnCancel %>" TabIndex="14" SkinID="btnSecondary"></asp:LinkButton>
                            <asp:LinkButton ID="btnOk" runat="server" Text="<%$Resources:btnOk %>" TabIndex="15" SkinID="btnPrimary"></asp:LinkButton>
                        </div>

                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ValidationSummary ID="vldItemSummary" HeaderText="<%$ Resources:vldItemSummary %>" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <nexus:ProgressIndicator ID="upManualJournalItem" OverlayCssClass="updating" AssociatedUpdatePanelID="up1" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </nexus:ProgressIndicator>
    </div>
</asp:Content>
