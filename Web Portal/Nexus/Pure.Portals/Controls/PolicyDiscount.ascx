<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PolicyDiscount.ascx.vb"
    Inherits="Nexus.Controls_PolicyDiscount" EnableViewState="true" %>

<div id="Controls_PolicyDiscount">
    <asp:UpdatePanel ID="updPolicyDiscount" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>

            <%-- Policy Discount / Loading checkbox — only visible when system option is active --%>
            <asp:Panel ID="pnlDiscountCheckbox" runat="server" Visible="false">
                <asp:Label ID="lblPolicyDiscountHeader" runat="server" Font-Bold="true"
                    AssociatedControlID="chkPolicyDiscount"
                    Text="<%$ Resources:lbl_PolicyDiscountHeader %>"></asp:Label>
                <asp:CheckBox ID="chkPolicyDiscount" runat="server"
                    AutoPostBack="true" CssClass="asp-check" Text=" " />
            </asp:Panel>

            <%-- Discount / Loading frame — hidden by default, shown when checkbox is checked --%>
            <asp:Panel ID="pnlDiscountFrame" runat="server" Visible="false">
                <div class="card card-secondary">
                    <div class="card-heading">
                        <h4><asp:Literal ID="litDiscountLoading" runat="server" Text="<%$ Resources:lbl_DiscountLoading %>"></asp:Literal></h4>
                    </div>
                    <div class="card-body clearfix">

                        <%-- Single inline row — all 4 label+field pairs spread across full width --%>
                        <div style="display:flex; align-items:center; justify-content:space-between; width:100%; flex-wrap:nowrap;">

                            <div style="display:flex; align-items:center; gap:5px;">
                                <asp:Label ID="lblDiscountReason" runat="server"
                                    AssociatedControlID="ddlDiscountReason"
                                    Text="<%$ Resources:lbl_DiscountLoadingReason %>"
                                    CssClass="control-label"
                                    style="white-space:nowrap;"></asp:Label>
                                <asp:DropDownList ID="ddlDiscountReason" runat="server"
                                    CssClass="form-control input-sm"
                                    AutoPostBack="true"
                                    style="width:180px;">
                                </asp:DropDownList>
                            </div>

                            <div style="display:flex; align-items:center; gap:5px;">
                                <asp:Label ID="lblRecurring" runat="server"
                                    AssociatedControlID="ddlRecurring"
                                    Text="<%$ Resources:lbl_Recurring %>"
                                    CssClass="control-label"
                                    style="white-space:nowrap;"></asp:Label>
                                <asp:DropDownList ID="ddlRecurring" runat="server"
                                    CssClass="form-control input-sm"
                                    Enabled="false"
                                    AutoPostBack="true"
                                    style="width:180px;">
                                </asp:DropDownList>
                            </div>

                            <div style="display:flex; align-items:center; gap:5px;">
                                <asp:Label ID="lblDiscountPercentage" runat="server"
                                    AssociatedControlID="txtDiscountPercentage"
                                    Text="<%$ Resources:lbl_DiscountLoadingPercentage %>"
                                    CssClass="control-label"
                                    style="white-space:nowrap;"></asp:Label>
                                <asp:TextBox ID="txtDiscountPercentage" runat="server"
                                    CssClass="form-control input-sm"
                                    Enabled="false"
                                    MaxLength="20"
                                    style="width:180px;"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revPercentage" runat="server"
                                    ControlToValidate="txtDiscountPercentage"
                                    ValidationExpression="^-?\d{1,10}(\.\d{1,8})?$"
                                    ErrorMessage="<%$ Resources:err_Percentage %>"
                                    Display="Dynamic" CssClass="error" ValidationGroup="PolicyDiscount"
                                    EnableClientScript="true" />
                            </div>

                            <div style="display:flex; align-items:center; gap:5px;">
                                <asp:Label ID="lblDiscountedPremium" runat="server"
                                    AssociatedControlID="txtDiscountedPremium"
                                    Text="<%$ Resources:lbl_DiscountedLoadedPremium %>"
                                    CssClass="control-label"
                                    style="white-space:nowrap;"></asp:Label>
                                <asp:TextBox ID="txtDiscountedPremium" runat="server"
                                    CssClass="form-control input-sm"
                                    Enabled="false"
                                    MaxLength="20"
                                    style="width:180px;"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revPremium" runat="server"
                                    ControlToValidate="txtDiscountedPremium"
                                    ValidationExpression="^-?\d{1,15}(\.\d{1,2})?$"
                                    ErrorMessage="<%$ Resources:err_Premium %>"
                                    Display="Dynamic" CssClass="error" ValidationGroup="PolicyDiscount"
                                    EnableClientScript="true" />
                            </div>

                        </div>

                        <%-- Apply Discounts button — right-aligned below the fields row --%>
                        <div style="text-align:right; padding-top:10px;">
                            <asp:Button ID="btnApplyDiscount" runat="server"
                                Text="<%$ Resources:btn_ApplyDiscounts %>"
                                CssClass="btn btn-primary btn-sm"
                                Enabled="false"
                                OnClientClick="this.disabled=true; this.value='Applying...'; __doPostBack(this.name, '');"
                                ValidationGroup="PolicyDiscount" />
                        </div>

                        <asp:HiddenField ID="hdnTotalPremium" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnAppliedPercentage" runat="server" Value="" />
                        <asp:HiddenField ID="hdnAppliedPremium" runat="server" Value="" />
                        <asp:HiddenField ID="hdnAppliedReasonId" runat="server" Value="" />
                        <asp:HiddenField ID="hdnAppliedRecurringId" runat="server" Value="" />
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="chkPolicyDiscount" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlDiscountReason" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlRecurring" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnApplyDiscount" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <nexus:ProgressIndicator ID="uprogDiscount" OverlayCssClass="updating"
        AssociatedUpdatePanelID="updPolicyDiscount" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>
</div>

<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () { policyDiscount_wireEvents(); });
    $(document).ready(function () { policyDiscount_wireEvents(); });

    function policyDiscount_wireEvents() {
        var txtPct = document.getElementById('<%= txtDiscountPercentage.ClientID %>');
        var txtPrem = document.getElementById('<%= txtDiscountedPremium.ClientID %>');
        var btnApplyId = '<%= btnApplyDiscount.ClientID %>';
        var hdnTotalId = '<%= hdnTotalPremium.ClientID %>';
        var hdnAppliedPctId = '<%= hdnAppliedPercentage.ClientID %>';
        var hdnAppliedPremId = '<%= hdnAppliedPremium.ClientID %>';
        var hdnAppliedReasonId = '<%= hdnAppliedReasonId.ClientID %>';
        var hdnAppliedRecurringId = '<%= hdnAppliedRecurringId.ClientID %>';
        var ddlReasonId = '<%= ddlDiscountReason.ClientID %>';
        var ddlRecurringId = '<%= ddlRecurring.ClientID %>';
        if (txtPct) {
            txtPct.onblur = function () {
                policyDiscount_onPercentageBlur(this,
                    '<%= txtDiscountedPremium.ClientID %>', hdnTotalId, btnApplyId, hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, ddlReasonId, ddlRecurringId);
            };
            var _pctDebounceTimer;
            txtPct.oninput = function () {
                policyDiscount_enableApplyOnChange(btnApplyId);
                clearTimeout(_pctDebounceTimer);
                _pctDebounceTimer = setTimeout(function () {
                    policyDiscount_onPercentageBlur(txtPct,
                        '<%= txtDiscountedPremium.ClientID %>', hdnTotalId, btnApplyId, hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, ddlReasonId, ddlRecurringId);
                }, 1000);
            };
        }
        if (txtPrem) {
            txtPrem.onblur = function () {
                policyDiscount_onPremiumBlur(this,
                    '<%= txtDiscountPercentage.ClientID %>', hdnTotalId, btnApplyId, hdnAppliedPctId, hdnAppliedPremId, hdnAppliedReasonId, hdnAppliedRecurringId, ddlReasonId, ddlRecurringId);
            };
            txtPrem.oninput = function () {
                policyDiscount_enableApplyOnChange(btnApplyId);
            };
        }
    }
</script>
