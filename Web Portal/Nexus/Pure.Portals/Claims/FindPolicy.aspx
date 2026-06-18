<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FindPolicy.aspx.vb" Inherits="Nexus.Framework_FindPolicy" %>

<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="CntMainBody" runat="Server">
    <div id="Claims_FindPolicy">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="ltrlpageheading" runat="server" Text="<%$ Resources:findpolicy_pageheading %>"></asp:Literal>
                </h1>
            </div>
            <p>
                <asp:Literal ID="ltrlPleaseEnterMessage" runat="server" Text="<%$ Resources:findpolicy_PleaseEnterMessage %>"></asp:Literal>
            </p>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>Find Policy</legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLossDate" runat="server" AssociatedControlID="CONTROL__LOSS_DATE" class="col-md-4 col-sm-3 control-label"><asp:Literal runat="server" Text="<%$ Resources:findpolicy_lossdate %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="CONTROL__LOSS_DATE" Columns="10" runat="server" CssClass="field-medium form-control"></asp:TextBox><uc1:CalendarLookup ID="LossDate_uctCalendarLookup" runat="server" LinkedControl="CONTROL__LOSS_DATE" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="RqdLossDate" runat="server" CssClass="error" ControlToValidate="CONTROL__LOSS_DATE" ErrorMessage="<%$Resources:findclaim_LossDateError %>" Display="none"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rngLossDate" runat="server" CssClass="error" ControlToValidate="CONTROL__LOSS_DATE" ErrorMessage="<%$Resources:findclaim_InvalidFormat %>" Type="Date" MinimumValue="01/01/1900" Display="none"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="CONTROL__POLICY_NUMBER" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal runat="server" Text="<%$ Resources:findpolicy_policynumber %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="CONTROL__POLICY_NUMBER" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdPolicyNo" runat="server" CssClass="error" ControlToValidate="CONTROL__POLICY_NUMBER" ErrorMessage="<%$ Resources:findclaim_PolicyNumberError %>" Display="none"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnFind" runat="server" Text="<%$ Resources:findpolicy_btn_find %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator runat="server" ID="IsPolicyExist" CssClass="error" ErrorMessage="<%$ Resources:findpolicy_lblNoResults  %>" Display="none"></asp:CustomValidator>
        <asp:CustomValidator ID="IsValidDate" runat="server" CssClass="error" ErrorMessage="<%$ Resources:findpolicy_lblInvalidLossDate %>" Display="none"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
