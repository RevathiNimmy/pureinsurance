<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RenewalQuotes.ascx.vb"
    Inherits="Nexus.Controls_RenewalQuotes" EnableViewState="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<script language="javascript" type="text/javascript">
    function setAgent(sName, sKey, sCode) {
        tb_remove();
        document.getElementById('<%= txtAgentCode.ClientId%>').value = unescape(sCode);
        document.getElementById('<%= txtAgentKey.ClientId%>').value = sKey;
    }

    function clearClient() {
        tb_init('a.thickbox');
    }
    function setClient(sClientName, sClientKey) {
        tb_remove();
        document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
        document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
        document.getElementById('<%= txtClient.ClientId%>').focus();
    }
</script>

<asp:ScriptManager ID="ScriptManagerRenewalManager" runat="server"></asp:ScriptManager>
<div id="Controls_RenewalQuotes">
    <h1>
        <asp:Literal ID="lblSummaryheadertitle" runat="server" Text="<%$ Resources:lblHeader%>"></asp:Literal></h1>
    <div class="card">
        <div class="card-body clearfix">

            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblAdvancedHeading" runat="server" Text="<%$ Resources:lbl_FilterHeading %>"></asp:Label></legend>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblRenewalDate" runat="server" AssociatedControlID="txtRenewalDate" Text="<%$ Resources:lbl_RenewalDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="txtRenewalDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-8 col-sm-9">
                        <asp:CheckBox ID="chkRenewalDate" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                    </div>
                    <uc1:CalendarLookup ID="RenewalDate_CalendarLookup" runat="server" LinkedControl="txtRenewalDate" HLevel="2"></uc1:CalendarLookup>
                    <asp:RangeValidator ID="rngRenewalDate" runat="server" ControlToValidate="txtRenewalDate" Display="None" ValidationGroup="RenewalStatusGroup" Type="Date" MinimumValue="01/01/1900" MaximumValue="01/12/9998" ErrorMessage="<%$ Resources:Err_InvalidRenewalDate %>"></asp:RangeValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblStatusType" runat="server" AssociatedControlID="RenewalStatusType" Text="<%$ Resources:lbl_StatusType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RenewalStatusType" runat="server" CssClass="field-large form-control form-select">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblProduct" runat="server" AssociatedControlID="ddlProductType" Text="<%$ Resources:lbl_ProductType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlProductType" runat="server" CssClass="field-medium form-control form-select">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="BranchCode" Text="<%$ Resources:lbl_BranchCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="BranchCode" runat="server" CssClass="form-control form-select">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="AgentPanel" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAgentCode" Text="Agent Code" ID="lblbtnAgentCode"></asp:Label><div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtAgentCode" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                <asp:LinkButton ID="btnAgentCode" runat="server" CausesValidation="false" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Agent Code</span></asp:LinkButton>

                            </span>
                        </div>
                    </div>


                    <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client %>" ID="lblbtnClient"></asp:Label><div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="txtClient" runat="server" CssClass="form-control"></asp:TextBox><span class="input-group-btn">
                                <asp:LinkButton ID="btnClient" runat="server" CausesValidation="false" SkinID="btnModal"><i class="fa fa-search"></i><span class="btn-fnd-txt">Client</span></asp:LinkButton></span>
                        </div>
                    </div>


                    <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                </div>
            </div>
        </div>

        <div class="card-footer">
            <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:lbl_btnNewSearch %>" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnFilter" ValidationGroup="RenewalStatusGroup" runat="server" Text="<%$ Resources:lbl_Filter %>" SkinID="btnPrimary"></asp:LinkButton>

        </div>
        <div id="GridViewRenewals" runat="server" visible="false">
            <div class="showall">
                <asp:Literal ID="ltRenewalMessage" runat="server" Text="<%$ Resources:lbl_Renewal_Title %>"></asp:Literal>
            </div>
            <asp:UpdatePanel ID="UpdRenewal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="grid-card table-responsive">
                        <asp:Panel ID="updpnlRenewal" runat="server">
                            <%--<ContentTemplate>--%>
                            <asp:GridView ID="grdvRenQuotes" runat="server" AlternatingRowStyle-BorderStyle="none" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,InsuranceFileKey" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric">
                                <Columns>
                                    <asp:BoundField DataField="InsuranceFileKey" ItemStyle-CssClass="span-2"></asp:BoundField>
                                    <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="span-3">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelection" runat="server" CausesValidation="False" AutoPostBack="true" OnCheckedChanged="GrdChkSelected" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PartyName" HeaderText="<%$ Resources:lbl_PartyName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                    <asp:BoundField DataField="Reference" HeaderText="<%$ Resources:lbl_Reference %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                    <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:lbl_ProductCode %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                    <asp:BoundField DataField="RenewalPremium" HeaderText="<%$ Resources:lbl_RenewalPremium %>" HtmlEncode="false" DataFormatString="{0:f}" ItemStyle-CssClass="span-5"></asp:BoundField>
                                    <asp:BoundField DataField="CoverStartDate" HeaderText="<%$ Resources:lbl_RenewalDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                    <asp:BoundField DataField="RenewalStatusTypeDescription" HeaderText="<%$ Resources:lbl_RenewalStatusTypeDescription %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                    <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="span-3">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnSelect" Text="<%$ Resources:lbl_lnkbtnSelect %>" runat="server" CausesValidation="False" CommandName="Details"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <p align="center">
                                        <asp:Label ID="lblNoPolicies" runat="server" Text="<%$ Resources:lbl_NoQuote %>"></asp:Label>
                                    </p>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="grdvRenQuotes"></asp:PostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <Nexus:ProgressIndicator ID="upRenewalQuotes" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdRenewal" runat="server">
                <progresstemplate>
                </progresstemplate>
            </Nexus:ProgressIndicator>
        </div>
    </div>
    <div id="NoPolicesFound" class="card-footer" visible="false" runat="server">
        <asp:Label ID="lblNoPoliciesFound" CssClass="error" runat="server" Text="<%$ Resources:lbl_NoQuote %>"></asp:Label>
    </div>
    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
        <div class="card-footer">
            <asp:LinkButton ID="btnTransfer" runat="server" Text="<%$ Resources:lbl_transfer %>" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnLapse" runat="server" Text="<%$ Resources:lbl_lapse %>" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnDelete" runat="server" Text="<%$ Resources:lbl_delete %>" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnStatus" runat="server" Text="<%$ Resources:lbl_status %>" SkinID="btnPrimary"></asp:LinkButton><br>
            <asp:Label ID="lblNoPoliciesSelected" CssClass="error" Visible="false" runat="server" Text="<%$ Resources:lbl_NoPoliciesSelected %>"></asp:Label>
        </div>
    </asp:Panel>
    <asp:ValidationSummary ID="vldRenewalManager" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="RenewalStatusGroup" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
</div>
