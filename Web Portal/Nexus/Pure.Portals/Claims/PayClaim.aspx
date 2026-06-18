<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PayClaim.aspx.vb" Inherits="Nexus.Claims_PayClaim" Title="Untitled Page" %>

<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="uc1" TagName="Address" Src="~/Controls/AddressCntrl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function setConfirmation(sMessage, sChoice, sUrl) {
            var IsConfirm;
            if (sChoice == '1') {
                IsConfirm = window.confirm(sMessage);
                if (IsConfirm) {
                    tb_show(null, sUrl, null);
                }
            }
            else {
                tb_show(null, sUrl, null);
            }
        }
        function setReInsurer(sName, sKey) //setReinsurer
        {
            document.getElementById('<%=txtParty.ClientID %>').value = unescape(sName);
            document.getElementById('<%=hPartyKey.ClientID %>').value = sKey;
            tb_remove()
        }
        function setOtherParty(sName, sKey, sAgentCode) //setOtherParty
        {
            document.getElementById('<%= txtParty.ClientId%>').value = sAgentCode;
            document.getElementById('<%= hPartyKey.ClientId%>').value = sKey;
            tb_remove()
        }
        onload = function () {
            var obj = document.getElementById('<%= rblPayee.ClientId%>');
            if (obj != null) {
                obj.focus();
            }
        }
    </script>

    <asp:ScriptManager ID="smPayClaim" runat="Server"></asp:ScriptManager>
    <div id="Claims_PayClaim">

        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblClaimInformation" runat="server" Text="<%$ Resources:lt_ClaimInformation %>"></asp:Literal></h1>
            </div>
            <div class="md-whiteframe-z0 bg-white">
                <ul class="nav nav-lines nav-tabs b-danger">
                    <li><a href="#tab-details" data-bs-toggle="tab" aria-expanded="true" class="active">
                        <asp:Literal ID="liDetails" runat="server" Text="<%$ Resources:lt_liDetails %>"></asp:Literal></a></li>
                    <li><a href="#tab-thispayment" data-bs-toggle="tab" aria-expanded="true">
                        <asp:Literal ID="ltThisPayment" runat="server" Text="<%$ Resources:lt_ThisPayment %>"></asp:Literal></a></li>
                </ul>
                <div class="tab-content clearfix p b-t b-t-2x">
                    <div id="tab-details" class="tab-pane animated fadeIn active" role="tabpanel">
                        <asp:UpdatePanel ID="updPayClaim_UI" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="lblPayee" runat="server" Text="<%$ Resources:lt_Payee %>"></asp:Label></legend>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblRiskTypeHeader" runat="server" AssociatedControlID="lblRiskType" Text="<%$ Resources:lbl_RiskType%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:Label ID="lblRiskType" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblLossCurrencyHeader" runat="server" AssociatedControlID="lblLossCurrency" Text="<%$ Resources:lbl_LossCurrency%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:Label ID="lblLossCurrency" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblDateOfLossHeader" runat="server" AssociatedControlID="lblDateOfLoss" Text="<%$ Resources:lbl_DateOfLoss%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:Label ID="lblDateOfLoss" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblPerilInfoHeader" runat="server" AssociatedControlID="lblPerilInfo" Text="<%$ Resources:lbl_PerilInfo%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:Label ID="lblPerilInfo" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <div class="col-md-8 col-sm-9">
                                            <asp:RadioButtonList ID="rblPayee" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="asp-radio">
                                                <asp:ListItem Text="<%$ Resources:li_ClaimPayable %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:li_Party %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:li_Agent %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:li_Client %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:li_Insurer %>" Value="4"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtParty" Text="<%$ Resources:btn_party%>" ID="lblbtnParty"></asp:Label><div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtParty" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                                    <asp:LinkButton ID="btnParty" runat="server" Enabled="false" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Party</span></asp:LinkButton>

                                                                                                                                </span>
                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hPartyKey" runat="server"></asp:HiddenField>
                                    </div>

                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblExGratia" runat="server" Visible="false" AssociatedControlID="chkExGratia" Text="<%$ Resources:lbl_ExGratia%> " class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:CheckBox ID="chkExGratia" runat="server" AutoPostBack="true" Visible="false" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rblPayee" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="piPayClaim_UI" OverlayCssClass="updating" AssociatedUpdatePanelID="updPayClaim_UI" runat="server">
                            <progresstemplate>
                                        </progresstemplate>
                        </Nexus:ProgressIndicator>



                        <legend>
                            <asp:Literal runat="server" ID="ltPageHeading"></asp:Literal>
                        </legend>
                        <div class="grid-card table-responsive">
                            <asp:UpdatePanel ID="updReceipt" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:GridView ID="gvSalvageDetails" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" Enabled="false" Visible="false" ShowHeader="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                        <Columns>
                                            <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_RecoveryType %>"></asp:BoundField>
                                            <asp:BoundField DataField="TotalRecovery" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lbl_TotalReserve %>"></asp:BoundField>
                                            <asp:BoundField DataField="ReceiptedAmount" HeaderText="<%$ Resources:lbl_RecoverdTotal %>" DataFormatString="{0:N2}"></asp:BoundField>
                                            <asp:BoundField DataField="ReceiptedTaxAmount" HeaderText="<%$ Resources:lbl_RecoverdTax %>" DataFormatString="{0:N2}"></asp:BoundField>
                                            <asp:BoundField DataField="LossThisReceiptINCLTax" HeaderText="<%$ Resources:lbl_ThisReceiptINCLTax %>" DataFormatString="{0:N2}"></asp:BoundField>
                                            <asp:BoundField DataField="LossThisReceiptTax" HeaderText="<%$ Resources:lbl_ThisReceiptTax %>" DataFormatString="{0:N2}"></asp:BoundField>
                                            <asp:BoundField DataField="LossThisNet" HeaderText="<%$ Resources:lbl_ThisNet %>" DataFormatString="{0:N2}"></asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="hypEditPayment" runat="server" SkinID="btnGrid" Text="<%$ Resources:btn_EditPayment %>"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="Load"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                            <Nexus:ProgressIndicator ID="piReceipt" OverlayCssClass="updating" AssociatedUpdatePanelID="updReceipt" runat="server">
                                <progresstemplate>
                                            </progresstemplate>
                            </Nexus:ProgressIndicator>
                        </div>
                        <div class="grid-card table-responsive">
                            <asp:UpdatePanel ID="updPayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:GridView ID="gvPaymentDetails" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" Enabled="false" Visible="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                        <RowStyle></RowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="Description" HeaderText=""></asp:BoundField>
                                            <asp:BoundField DataField="TotalReserve" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lbl_TotalReserve %>"></asp:BoundField>
                                            <asp:BoundField DataField="ThisRevision" DataFormatString="{0:N2}" HeaderText="<%$ Resources:lbl_ThisRevision %>"></asp:BoundField>
                                            <asp:BoundField DataField="PaidToDate" HeaderText="<%$ Resources:lbl_PaidtoDate %>"></asp:BoundField>
                                            <asp:BoundField DataField="PaidToDateTax" HeaderText="<%$ Resources:lbl_PaidtoDateTax %>"></asp:BoundField>
                                            <asp:BoundField DataField="CurrentReserve" HeaderText="<%$ Resources:lbl_CurrentReserve %>"></asp:BoundField>
                                            <asp:BoundField DataField="ThisPaymentINCLTax" HeaderText="<%$ Resources:lbl_ThisPaymentInclTax %>"></asp:BoundField>
                                            <asp:BoundField DataField="ThisPaymentTax" HeaderText="<%$ Resources:lbl_ThisPaymentTax %>"></asp:BoundField>
                                            <asp:BoundField DataField="CostToClaim" HeaderText="<%$ Resources:lbl_CostToClaim %>"></asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="hypEditPayment" SkinID="btnGrid" runat="server" Text="<%$ Resources:btn_EditPayment %>"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="Load"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                            <Nexus:ProgressIndicator ID="piPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="updPayment" runat="server">
                                <progresstemplate>
                                            </progresstemplate>
                            </Nexus:ProgressIndicator>
                        </div>
                    </div>
                    <div id="tab-thispayment" class="tab-pane animated fadeIn" role="tabpanel">
                        <asp:UpdatePanel ID="updThisPayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:Panel ID="pnlThisPaymentTab" runat="server" Visible="false">

                                    <legend>
                                        <asp:Literal ID="lblThisPayment" runat="server" Text="<%$ Resources:lt_ThisPayment %>"></asp:Literal></legend>


                                    <div class="form-horizontal">
                                        <legend>
                                            <asp:Label ID="ThisPaymentSummary" runat="server" Text="<%$ Resources:lbl_ThisPaymentSummary %>"></asp:Label>
                                        </legend>

                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblGrossPayment" runat="server" AssociatedControlID="txtGrossPayment" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltGrossPayment" runat="server" Text="<%$ Resources:lbl_GrossPayment%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtGrossPayment" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTotalTax" runat="server" AssociatedControlID="txtTotalTax" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTotalTax" runat="server" Text="<%$ Resources:lbl_TotalTax%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtTotalTax" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTotalWHTax" runat="server" AssociatedControlID="txtTotalWHTax" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTotalWHTax" runat="server" Text="<%$ Resources:lbl_TotalWHTax%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtTotalWHTax" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblNetPayment" runat="server" AssociatedControlID="txtNetPayment" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltNetPayment" runat="server" Text="<%$ Resources:lbl_NetPayment%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtNetPayment" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>



                                    <legend>
                                        <asp:Literal ID="lblTaxesOnThisPayment" runat="server" Text="<%$ Resources:lbl_TaxesOnThisPayment %>"></asp:Literal></legend>
                                    <div class="grid-card table-responsive">
                                        <asp:GridView ID="gvTaxesonThisPayment" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" Visible="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                            <Columns>
                                                <asp:BoundField HeaderText="ReserveType" DataField="ReserveType"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxGroup" DataField="TaxGroupCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxBand" DataField="TaxBandCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="Percentage" DataField="Percentage" DataFormatString="{0:N2}%"></asp:BoundField>
                                                <asp:BoundField HeaderText="Amount" DataField="Amount" DataFormatString="{0:N2}"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="grid-card table-responsive">
                                        <asp:GridView ID="gvTaxesonThisReceipt" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" Visible="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                            <Columns>
                                                <asp:BoundField HeaderText="RecoveryType" DataField="RecoveryType"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxGroup" DataField="TaxGroupCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxBand" DataField="TaxBandCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="Percentage" DataField="Percentage" DataFormatString="{0:N2}%"></asp:BoundField>
                                                <asp:BoundField HeaderText="Amount" DataField="Amount" DataFormatString="{0:N2}"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="form-horizontal">
                                        <legend>
                                            <asp:Label ID="lblPaymentDetails" runat="server" Text="<%$ Resources:lbl_PaymentDetails %>"></asp:Label></legend>

                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblMediatype" runat="server" AssociatedControlID="GISLookup_MediaType" Text="<%$ Resources:lbl_Mediatype %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <NexusProvider:LookupList ID="GISLookup_MediaType" runat="server" DataItemText="Description" DefaultText="(Please Select)" DataItemValue="Code" ListCode="MediaType" ListType="PMLookup" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblMediaRef" runat="server" AssociatedControlID="txtMediaRef" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltMediaRef" runat="server" Text="<%$ Resources:lbl_MediaRef%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtMediaRef" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblPayeeName" runat="server" AssociatedControlID="txtPayeeName" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltPayeeName" runat="server" Text="<%$ Resources:lbl_PayeeName%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtPayeeName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="liChequeDate" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblChequeDate" runat="server" AssociatedControlID="txtChequeDate" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltChequeDate" runat="server" Text="<%$ Resources:lbl_ChequeDate%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            <uc1:CalendarLookup ID="calChequeDate" runat="server" LinkedControl="txtChequeDate" HLevel="3"></uc1:CalendarLookup>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltBankName" runat="server" Text="<%$ Resources:lbl_BankName%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBankCode" runat="server" AssociatedControlID="txtBankCode" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltBankCode" runat="server" Text="<%$ Resources:lbl_BankCode%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtBankCode" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBankAccNumber" runat="server" AssociatedControlID="txtBankAccNumber" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltBankAccNumber" runat="server" Text="<%$ Resources:lbl_BankAccNumber%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtBankAccNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="liThisReference" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblThisReference" runat="server" AssociatedControlID="txtThisReference" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltThisReference" runat="server" Text="<%$ Resources:lbl_ThisReference%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtThisReference" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltComments" runat="server" Text="<%$ Resources:lbl_Comments%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="divAddress" runat="server">
                                        <legend>
                                            <asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:lbl_Address %>"></asp:Label></legend>
                                        <uc1:Address ID="Address" runat="server"></uc1:Address>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="piThisPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="updThisPayment" runat="server">
                            <progresstemplate>
                                    </progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                    <asp:CustomValidator ID="IsValidReserve" runat="server" Display="none" ValidationGroup="PayClaim"></asp:CustomValidator>
                    <asp:CustomValidator ID="IsPaymentReceived" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_PaymentReceived_Error %>" Display="none" ValidationGroup="PayClaim"></asp:CustomValidator><asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" ValidationGroup="PayClaim" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>

                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="btn_Back" runat="server" SkinID="btnSecondary" Text="<%$ Resources:btn_Back %>"></asp:LinkButton>
                    <asp:LinkButton ID="btnOk" runat="server" SkinID="btnPrimary" Text="<%$ Resources:btn_Ok %>" ValidationGroup="PayClaim"></asp:LinkButton>
                    <asp:LinkButton ID="btnInstalments" runat="server" SkinID="btnPrimary" Text="Instalments" Visible="false" CausesValidation="false"></asp:LinkButton>
                    <asp:HiddenField ID="HidMediaTypeFieldMandatory" runat="server"></asp:HiddenField>
                </div>

                <%-- Instalments Panel: shown when btnInstalments clicked, replaces This Receipt content --%>
                <asp:Panel ID="pnlInstalments" runat="server" Visible="false" CssClass="card-body clearfix">
                    <asp:Label ID="lblInstalmentsMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    <%-- T3: Instalments Tab UI controls will be added here --%>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
