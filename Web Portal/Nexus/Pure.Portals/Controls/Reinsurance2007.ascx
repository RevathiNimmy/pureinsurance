<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Reinsurance2007.ascx.vb"
    Inherits="Nexus.Controls_Reinsurance2007" %>
<%@ Register Src="~/Controls/RIModelSummary.ascx" TagName="RIModel" TagPrefix="uc1" %>


<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        var currentURL = location.toString();
        if ((currentURL.indexOf("?RICode") != -1)) {
            $('.nav-tabs a[href="#tab-ReinsuranceCntrl"]').tab('show')
        }
        gridButtonHandling();
    });

    function allowNumericOnly(e) {
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 46) {
            e.preventDefault();
            return false;
        }
        return true;
    }

    function configPercentage(byControl, e_format) {
        $(byControl).each(function () {
            $(this).numeric(".");

            $(this).focus(function () {
                $(this).select();
            });
            $(this).keypress(function (event) {
                if ($(this).val().split('.').length > 1) {
                    var strNumber = $(this).val();
                    var strAfterDecimal = strNumber.split('.')[1];
                    var iCursorPosition = 0;
                    try {
                        if (typeof $(this).caret === 'function') {
                            var caretInfo = $(this).caret();
                            if (caretInfo && typeof caretInfo.start !== 'undefined') {
                                iCursorPosition = caretInfo.start;
                            } else {
                                iCursorPosition = this.selectionStart || 0;
                            }
                        } else {
                            iCursorPosition = this.selectionStart || 0;
                        }
                    } catch (ex) {
                        iCursorPosition = this.selectionStart || 0;
                    }
                    if (($(this).val().indexOf('%')) > 0) {
                        if (strAfterDecimal.length > 4 && iCursorPosition > $(this).val().indexOf('.')) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if (strAfterDecimal.length > 3 && iCursorPosition > $(this).val().indexOf('.')) {
                            event.preventDefault();
                        }
                    }
                }
            });
            $(this).bind('paste', function (e) {
                var browserName = navigator.appName;
                if (browserName == "Microsoft Internet Explorer") {
                    var strNumber = clipboardData.getData("text");
                    var havePercentSymbol = false;
                    if (strNumber.indexOf('%') > 0) {
                        strNumber = strNumber.replace('%', '')
                        havePercentSymbol = true;
                    }
                    var strBeforeDecimal = strNumber.split('.')[0];
                    var strAfterDecimal = strNumber.split('.')[1];

                    if (strAfterDecimal.length > 4) {
                        strAfterDecimal = strAfterDecimal.substring(0, 4)
                    }
                    var numberToPaste = strBeforeDecimal + '.' + strAfterDecimal;
                    if (havePercentSymbol == true) {
                        numberToPaste = numberToPaste + '%';
                    }
                    clipboardData.setData("text", numberToPaste);
                }
            });
        });

        $(byControl).css('text-align', 'right');
    }

    function alertMe(sMessage) {
        alert(sMessage);
    }

    function ShowMsg(sMessage) {
        alert(sMessage);
    }

    function ConfirmRIMsg(sMessage) {
        var ret = confirm(sMessage);
        return ret;
    }

    function gridButtonHandling() {
        $('.rowMenu').each(function () {
            if ($(this).find('a[data-bs-toggle^="dropdown"]').length > 0) {
                if ($(this).find('a').length <= 1) {
                    $(this).hide();
                }
            }
        });
    }

    function DisableButton(sender, args) {
        //disable the button (or whatever else we need to do) here
        var btnOK = document.getElementById('<%= btnOk.ClientId%>');
        if (btnOK != null) {
            btnOK.disabled = true;
        }
        gridButtonHandling();
    }

    function EnableButton(sender, args) {
        //enable the button (or whatever else we need to do) here
        var btnOK = document.getElementById('<%= btnOK.ClientId%>');
            if (btnOK != null) {
                btnOK.disabled = false;
            }
            gridButtonHandling();
        }
  
        function pageLoad() {
            //this is needed if the trigger is external to the update panel   
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(DisableButton);
            manager.add_endRequest(EnableButton);
        }

        function ValidateXOLRiModelAndRIModelExistance(sMessage, bIsPTAmendment) {
            alert(sMessage);
            if (bIsPTAmendment == true) {
                window.location = '<%=ResolveClientUrl("~/secure/RIAmendRiskList.aspx") %>';
            }
            else {
                window.location = '<%=ResolveClientUrl("~/secure/PremiumDisplay.aspx") %>';
            }
        }

    function confirmDeleteTreaty(treatyName) {
        return confirm('Are you sure you want to delete treaty "' + treatyName + '"?');
    }

    function showLoadingIndicator() {
        if ($('.loading-overlay').length === 0) {
            $('body').append('<div class="loading-overlay"><div class="loading-spinner"><i class="fa fa-spinner fa-spin fa-3x"></i></div></div>');
        }
    }

    function hideLoadingIndicator() {
        $('.loading-overlay').remove();
    }

</script>
<style type="text/css">
    /* Task 13.1: Add Treaty Buttons */
    #btnAddPropTreaty, #btnAddXOLTreaty {
        margin-left: 5px;
        font-weight: 500;
    }

    /* Task 13.2: Treaty Selection Modal - ThickBox Overrides */
    #TB_overlay {
        background: rgba(0, 0, 0, 0.5) !important;
        z-index: 9999 !important;
    }

    #TB_window {
        background: white !important;
        border-radius: 4px !important;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1) !important;
    }

    #TB_ajaxContent {
        padding: 20px !important;
        overflow-y: auto !important;
    }

    /* Task 13.4: Validation Error Messages */
    .validation-error {
        color: #dc3545;
        font-weight: 500;
        padding: 10px;
        margin: 10px 0;
        border: 1px solid #dc3545;
        border-radius: 4px;
        background-color: #f8d7da;
    }

    .validation-error i {
        margin-right: 5px;
    }

    /* Task 13.5: Warning Messages */
    .warning-message {
        color: #856404;
        font-weight: 500;
        padding: 10px;
        margin: 10px 0;
        border: 1px solid #ffc107;
        border-radius: 4px;
        background-color: #fff3cd;
    }

    .warning-message i {
        margin-right: 5px;
    }

    /* Task 13.7: Loading Indicators */
    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.3);
        z-index: 10000;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .loading-spinner {
        color: #007bff;
    }

    /* Task 13.8: Tooltips */
    .treaty-tooltip {
        position: relative;
        display: inline-block;
        cursor: help;
    }

    .treaty-tooltip .tooltiptext {
        visibility: hidden;
        width: 250px;
        background-color: #555;
        color: #fff;
        text-align: left;
        border-radius: 6px;
        padding: 8px;
        position: absolute;
        z-index: 1;
        bottom: 125%;
        left: 50%;
        margin-left: -125px;
        opacity: 0;
        transition: opacity 0.3s;
        font-size: 12px;
        line-height: 1.4;
    }

    .treaty-tooltip .tooltiptext::after {
        content: "";
        position: absolute;
        top: 100%;
        left: 50%;
        margin-left: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: #555 transparent transparent transparent;
    }

    .treaty-tooltip:hover .tooltiptext {
        visibility: visible;
        opacity: 1;
    }

   

    /* Ensure grid table layout handles column widths properly */
    #gvReinsurance {
        table-layout: auto;
    }

    #gvReinsurance td:nth-child(1),
    #gvReinsurance th:nth-child(1),
    #gvReinsurance td:nth-child(2),
    #gvReinsurance th:nth-child(2) {
        min-width: 100px;
        white-space: nowrap;
    }

    #gvReinsurance td:nth-child(8),
    #gvReinsurance td:nth-child(9) {
        white-space: nowrap;
    }

</style>
<div id="Controls_Reinsurance2007">
    <div class="md-whiteframe-z0 bg-white">
        <ul class="nav nav-lines nav-tabs b-danger">
            <li><a href="#tab-ReinsuranceMain" data-bs-toggle="tab" aria-expanded="true" class="active">
                <asp:Literal ID="lbltab_ReinsuranceMain" runat="server" Text="<%$Resources:lbl_tab_Reinsurance %>"></asp:Literal></a></li>
            <li><a href="#tab-ReinsuranceModelSummary" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="lbltab_ReinsuranceModelSummary" runat="server" Text="<%$Resources:lbl_tab_RIModelSummary %>"></asp:Literal></a></li>
        </ul>
        <div class="tab-content clearfix p b-t b-t-2x">
            <div id="tab-ReinsuranceMain" class="tab-pane animated fadeIn active" role="tabpanel">
                <asp:UpdatePanel ID="UpdatePanelReinsurance" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblReinsuranceMain" runat="server" Text="<%$ Resources:lbl_Reinsurance %>"></asp:Label></legend>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblReinsuranceBand" runat="server" AssociatedControlID="ddlReinsurance" Text="<%$ Resources:lblReinsuranceBand %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="ddlReinsurance" runat="server" CssClass="field-medium form-control form-select" AutoPostBack="True"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="ddlRIVersion" runat="server" CssClass="field-medium form-control form-select" AutoPostBack="True" Visible="false"></asp:DropDownList>
                                </div>
                                <asp:Label ID="lblEffectiveDate" runat="server" Visible="false" Text="<%$ Resources:lblEffectiveDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <asp:Label ID="lblVersionEffectiveDate" runat="server" Visible="false" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <asp:Panel ID="pnlOverrideReason" runat="server" Visible="false" CssClass="form-group form-group-sm col-sm-12" style="margin:0;padding:0;">
                                    <asp:Label ID="lblOverrideReason" runat="server"
                                               Text="Override Reason:" AssociatedControlID="ddlOverrideReason" class="col-md-4 col-sm-3 control-label" />
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="ddlOverrideReason" runat="server" AutoPostBack="false" CssClass="field-medium form-control form-select" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="grid-card table-responsive no-margin">
                            <asp:GridView ID="gvReinsurance" runat="server" AutoGenerateColumns="False" GridLines="None" AllowPaging="false" ShowHeader="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="IsNew" HeaderText="Is New"></asp:BoundField>
                                    <asp:BoundField DataField="IsEdited" HeaderText="Is Edited"></asp:BoundField>
                                    <asp:BoundField DataField="Placement" HeaderText="<%$ Resources:lblPlacement %>"></asp:BoundField>
                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblName %>" HtmlEncode="False"></asp:BoundField>
                                    <nexus:BoundField DataField="Retained" HeaderText="<%$ Resources:lblRetained %>" DataType="Percentage"></nexus:BoundField>
                                    <nexus:BoundField DataField="DefaultPerc" HeaderText="<%$ Resources:lblDefault %>" DataType="Percentage" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc"></nexus:BoundField>
                                    <nexus:TemplateField HeaderText="<%$ Resources:lblThis %>" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc" DataType="Percentage">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtThisPerc" runat="server" Visible="false" Text='<%# XPath("@ThisPerc") %>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' OnTextChanged="txtThisPerc_TextChanged" AutoPostBack="true" CssClass="PercTextBox"></asp:TextBox>
                                            <asp:RangeValidator ID="rngThisPerc" runat="server" Display="None" ControlToValidate="txtThisPerc" MinimumValue="0.0001" MaximumValue="100" Type="Double" ErrorMessage="<%$ Resources:Err_InvalidRange %>" Enabled="false"></asp:RangeValidator>
                                            <asp:Label ID="lblThisPerc" runat="server" Visible="false" Text='<%# String.Format("{0}{1}", XPath("@ThisPerc"), "%")%>' HtmlEncode="false" CssClass="Perc"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:TemplateField HeaderText="FAC Prop Premium %" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc" DataType="Percentage">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFACPropPremiumPerc" runat="server" Visible="false" Text='<%# XPath("@FACPropPremiumPerc") %>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' OnTextChanged="txtFACPropPremiumPerc_TextChanged" AutoPostBack="true" CssClass="PercTextBox"></asp:TextBox>
                                            <asp:RangeValidator ID="rngThisFACPerc" runat="server" Display="None" ControlToValidate="txtFACPropPremiumPerc" MinimumValue="0.0001" MaximumValue="100" Type="Double" ErrorMessage="<%$ Resources:Err_InvalidRange %>" Enabled="false"></asp:RangeValidator>
                                             <asp:Label ID="lblFACPropPremiumPerc" runat="server" Visible="false" Text='<%# String.Format("{0}{1}", XPath("@FACPropPremiumPerc"), "%")%>' HtmlEncode="false" CssClass="Perc"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:TemplateField HeaderText="<%$ Resources:lblLowerLimit %>" DataType="Currency">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtLowerLimit" runat="server" Visible="false" Text='<%# XPath("@LowerLimit")%>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' CssClass="PercTextBox" OnTextChanged="txtLowerLimit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="lblLowerLimit" runat="server" Visible="false" Text='<%# XPath("@LowerLimit")%>' CssClass="Doub"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:TemplateField HeaderText="<%$ Resources:lblUpperLimit %>" DataType="Currency">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUpperLimit" runat="server" Visible="false" Text='<%# XPath("@LineLimit")%>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' CssClass="PercTextBox" OnTextChanged="txtUpperLimit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="lblUpperLimit" runat="server" Visible="false" Text='<%# XPath("@LineLimit")%>' CssClass="Doub"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:TemplateField HeaderText="<%$ Resources:lblSumInsured %>" DataType="Currency">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSumInsured" runat="server" Visible="false" Text='<%# XPath("@SumInsured")%>' ToolTip='<%# XPath("@RIArrangementLineKey")%>'  AutoPostBack="true" CssClass="PercTextBox" OnTextChanged="txtSumInsured_TextChanged"></asp:TextBox>
                                            <asp:Label ID="lblSumInsured" runat="server" Visible="false" Text='<%# XPath("@SumInsured")%>' CssClass="Doub"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:TemplateField HeaderText="<%$ Resources:lblPremium %>" DataType="Currency">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPremium" runat="server" Visible="false" Text='<%# XPath("@Premium")%>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' OnTextChanged="txtPremium_TextChanged" AutoPostBack="true" CssClass="PercTextBox" onkeypress="return allowNumericOnly(event);"></asp:TextBox>
                                            <asp:Label ID="lblPremium" runat="server" Visible="false" Text='<%# XPath("@Premium")%>' CssClass="Doub"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:BoundField DataField="Tax" HeaderText="<%$ Resources:lblTax %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:TemplateField HeaderText="<%$ Resources:lblCommissionPercentage %>" DataType="Percentage">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCommissionPerc" runat="server" Visible="false" Text='<%# String.Format("{0}{1}", XPath("@CommissionPerc"), "%")%>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' OnTextChanged="txtCommissionPerc_TextChanged" AutoPostBack="true" CssClass="PercTextBox"></asp:TextBox>
                                            <asp:Label ID="lblCommissionPerc" runat="server" Visible="false" Text='<%# String.Format("{0}{1}", XPath("@CommissionPerc"), "%")%>' CssClass="Perc"></asp:Label>
                                        </ItemTemplate>
                                    </nexus:TemplateField>
                                    <nexus:BoundField DataField="Commission" HeaderText="<%$ Resources:lblCommission %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="CommissionTax" HeaderText="<%$ Resources:lblCommTax %>" DataType="Currency"></nexus:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lblAgreement %>" HeaderStyle-CssClass="str">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAgreement" runat="server" Visible="false" Text='<%# XPath("@Agreement")%>' ToolTip='<%# XPath("@RIArrangementLineKey")%>' OnTextChanged="txtAgreement_TextChanged" AutoPostBack="true" CssClass="strTextBox"></asp:TextBox>
                                            <asp:Label ID="lblAgreement" runat="server" Visible="false" Text='<%# XPath("@Agreement")%>' CssClass="str"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol class="list-inline no-margin">
                                                    <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                        <ol id="menu_<%# Eval("RIArrangementLineKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                            <li>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkDelete_text %>" Visible="false" ToolTip='<%# XPath("@RIArrangementLineKey")%>' CommandName="Delete"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkView" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkView_text %>" Visible="false" ToolTip='<%# XPath("@RIArrangementLineKey")%>' CommandName="View"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkEdit_text %>" Visible="false" ToolTip='<%# XPath("@RIArrangementLineKey")%>' CommandName="EditRow"></asp:LinkButton>
                                                            </li>
                                                        </ol>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RIarrangementKey" HeaderText="RIarrangementKey" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="RIArrangementLineKey" HeaderText="RIArrangementLineKey" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="IsRIBroker" HeaderText="IsRIBroker" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="Type" HeaderText="Type" Visible="false"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="ri-org-header-row">
                            <asp:Label ID="lblOrgReinsurance" runat="server" Text="<%$ Resources:lbl_OrgReinsurance %>" CssClass="ri-org-label"></asp:Label>
                            <asp:Panel ID="pnlOrgOverrideReason" runat="server" Visible="false" CssClass="ri-org-override-panel">
                                <asp:Label ID="lblOrgOverrideReasonCaption" runat="server"
                                           Text="Override Reason (Original):" AssociatedControlID="ddlOrgOverrideReason" CssClass="control-label ri-org-override-label" />
                                <div class="ri-org-override-ddl">
                                    <asp:DropDownList ID="ddlOrgOverrideReason" runat="server" AutoPostBack="false" Enabled="false" CssClass="field-medium form-control form-select" />
                                </div>
                                <asp:Label ID="lblOrgOverrideReasonValue" runat="server" Visible="false" />
                            </asp:Panel>
                        </div>
                        <div class="grid-card table-responsive no-margin">
                            <asp:GridView ID="gvOrgReinsurance" runat="server" AutoGenerateColumns="False" GridLines="None" AllowPaging="false" ShowHeader="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="Placement" HeaderText="<%$ Resources:lblPlacement %>"></asp:BoundField>
                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblOrgName %>"></asp:BoundField>
                                    <nexus:BoundField DataField="Retained" HeaderText="<%$ Resources:lblRetained %>" DataType="Percentage" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc"></nexus:BoundField>
                                    <nexus:BoundField DataField="DefaultPerc" HeaderText="<%$ Resources:lblOrgDefault %>" DataType="Percentage" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc"></nexus:BoundField>
                                    <nexus:BoundField DataField="ThisPerc" HeaderText="<%$ Resources:lblOrgThis %>" DataType="Percentage" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc"></nexus:BoundField>
                                    <nexus:BoundField DataField="LowerLimit" HeaderText="<%$ Resources:lblLowerLimit %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="LineLimit" HeaderText="<%$ Resources:lblUpperLimit %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="SumInsured" HeaderText="<%$ Resources:lblOrgSumInsured %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lblOrgPremium %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="Tax" HeaderText="<%$ Resources:lblOrgTax %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="CommissionPerc" HeaderText="<%$ Resources:lblOrgCommissionPercentage %>" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc" DataType="Percentage"></nexus:BoundField>
                                    <nexus:BoundField DataField="Commission" HeaderText="<%$ Resources:lblOrgCommission %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField DataField="CommissionTax" HeaderText="<%$ Resources:lblOrgCommTax %>" DataType="Currency"></nexus:BoundField>
                                    <asp:BoundField DataField="Agreement" HeaderText="<%$ Resources:lblOrgAgreement %>" ItemStyle-CssClass="str" HeaderStyle-CssClass="str"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvReinsurance" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upReinsurance" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdatePanelReinsurance" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </nexus:ProgressIndicator>

                <asp:UpdatePanel ID="updSubmitArea" runat="server">
                    <ContentTemplate>
                        <div class="p-v-sm text-right">
                            <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" Text="<%$ Resources:btnCancel%>" CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnAddPropTreaty" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnAddPropTreaty%>" CausesValidation="false" Visible="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnAddXOLTreaty" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnAddXOLTreaty%>" CausesValidation="false" Visible="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnAddFacProp" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnAddFacProp%>" PostBackUrl="~/Modal/FACPlacement.aspx?Type=FACPROP" CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="btnAddFacXOL" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnAddFacXOL%>" CausesValidation="false" PostBackUrl="~/Modal/FACPlacement.aspx?Type=FACXOL"></asp:LinkButton>
                            <asp:LinkButton ID="btnOk" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnOk%>" CausesValidation="false"></asp:LinkButton>
                            <asp:HiddenField ID="hdnRetainedPremiumConfirmed" runat="server" Value="0" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div id="tab-ReinsuranceModelSummary" class="tab-pane animated fadeIn" role="tabpanel">
                <uc1:RIModel ID="RIModelSummaryCntrl" runat="server" Visible="false"></uc1:RIModel>
            </div>
        </div>
    </div>
</div>
