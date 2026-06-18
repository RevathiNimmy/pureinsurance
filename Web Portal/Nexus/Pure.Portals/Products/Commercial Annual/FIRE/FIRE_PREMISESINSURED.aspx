<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_PREMISESINSURED.aspx.vb" Inherits="Nexus.FIRE_PREMISESINSURED" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="risk-screen">


        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">
                <legend><span>
                    <asp:Label runat="server" Text="<%$ Resources:lblADDRESS %>"></asp:Label></span></legend>
                <div id="liGROUP_FIRE__ADDRESS_CNT">
                    <NexusControl:addresscntrl id="GROUP_FIRE__ADDRESS_CNT" runat="server"></NexusControl:addresscntrl>
                </div>
                <div class="form-horizontal">
                    <legend><span>
                        <asp:Label runat="server" Text="<%$ Resources:lblINDUSTRY %>"></asp:Label></span></legend>

                    <div id="liGROUP_FIRE__PRIMARY_INDUSTRYN" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" Text="<%$ Resources:lblPrimaryIndustry %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__PRIMARY_INDUSTRYN" runat="server" listtype="UserDefined" listcode="PRIIND" parentlookuplistid="" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                        <asp:RequiredFieldValidator ID="rfvPrimaryIndustry" runat="server" ControlToValidate="GROUP_FIRE__PRIMARY_INDUSTRYN" ErrorMessage="Primary Industry is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liGROUP_FIRE__SECONDARY_INDUSTRYN" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" Text="<%$ Resources:lblSecondryIndustry %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__SECONDARY_INDUSTRYN" runat="server" listtype="UserDefined" listcode="SECIND" parentlookuplistid="GROUP_FIRE__PRIMARY_INDUSTRYN" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                        <asp:RequiredFieldValidator ID="rfvSecondaryIndustry" runat="server" ControlToValidate="GROUP_FIRE__SECONDARY_INDUSTRYN" ErrorMessage="Secondary Industry is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liGROUP_FIRE__TERITARY_INDUSTRYN" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" Text="<%$ Resources:lblTeritaryIndustry %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__TERITARY_INDUSTRYN" runat="server" listtype="UserDefined" listcode="TERIND" parentlookuplistid="GROUP_FIRE__SECONDARY_INDUSTRYN" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                        <asp:RequiredFieldValidator ID="rfvTeritaryIndustry" runat="server" ControlToValidate="GROUP_FIRE__TERITARY_INDUSTRYN" ErrorMessage="Teritary Industry is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liGROUP_FIRE__INDUSTRYN" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" Text=" <%$ Resources:lblIndustry %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__INDUSTRYN" runat="server" listtype="UserDefined" listcode="INDUSTRY" parentlookuplistid="GROUP_FIRE__TERITARY_INDUSTRYN" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                        <asp:RequiredFieldValidator ID="rfvIndustry" runat="server" ControlToValidate="GROUP_FIRE__INDUSTRYN" ErrorMessage="Industry is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>

                </div>
            </div>
            <div class='card-footer'>
                <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" HeaderText="<h2>There are errors on this page</h2><p>Please review these errors and re-submit the form</p>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
