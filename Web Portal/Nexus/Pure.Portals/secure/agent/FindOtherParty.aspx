<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" CodeFile="~/secure/agent/FindOtherParty.aspx.vb"
    Inherits="Secure_FindOtherParty" EnableViewState="true" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindOtherParty">
        <asp:Panel ID="PnlFindOtherParty" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="litPageHeader" runat="server" Text="<%$ Resources:lbl_Page_header %>" EnableViewState="false"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPartyname" runat="server" AssociatedControlID="txtPartyName" Text="<%$ Resources:lblPartyname %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPartyName" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPartyCode" runat="server" AssociatedControlID="txtPartyCode" Text="<%$ Resources:lblPartyCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPartyCode" TabIndex="2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" TabIndex="3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" Text="<%$ Resources:lblAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAddress" TabIndex="4" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lblClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" TabIndex="5" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lblRiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" TabIndex="6" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" Text="<%$ Resources:lblPhone %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPhone" TabIndex="7" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostcode" runat="server" AssociatedControlID="txtPostcode" Text="<%$ Resources:lblPostcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostcode" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="txtType" Text="<%$ Resources:lblType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtType" TabIndex="8" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPartyType" runat="server" AssociatedControlID="ddlPartyType" Text="<%$ Resources:lblPartyType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlPartyType" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="vldPartyType" runat="server" Display="none" ControlToValidate="ddlPartyType"
                            ValidationGroup="ValidationNewParty" ErrorMessage="<%$ Resources:err_PartyType %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIncludeClosedBranches" runat="server" AssociatedControlID="chkIncludeClosedBranches" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litIncludeClosedBranches" runat="server" Text="<%$ Resources:chkIncludeClosedBranches  %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIncludeClosedBranches" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <asp:Panel CssClass="error" ID="PnlError" runat="server" Visible="false">
                    <asp:Label runat="server" ID="lblError" Text="<%$ Resources:ErrorMessage %>"></asp:Label>
                </asp:Panel>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnReset" runat="server" TabIndex="6" Text="<%$ Resources:btnReset %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNewOtherParty" runat="server" TabIndex="18" Text="<%$ Resources:btnNewOtherParty %>" SkinID="btnPrimary" CausesValidation="true" ValidationGroup="ValidationNewParty"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="5" Text="<%$ Resources:btnSearch %>" SkinID="btnPrimary" CausesValidation="false"></asp:LinkButton>

            </div>
        </asp:Panel>
        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtPartyName,txtPartyCode,txtFileCode,txtAddress,txtClaimNumber,txtRiskIndex,txtPhone,txtPostcode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </nexus:WildCardValidator>

        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"
            ValidationGroup="ValidationNewParty"></asp:ValidationSummary>
        <asp:UpdatePanel ID="updFindOtherParty" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None"
                        OnPageIndexChanging="grdvSearchResults_PageIndexChanging" OnRowCommand="grdvSearchResults_RowCommand" DataKeyNames="UserName"
                        EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblUserName_g %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="ResolvedName" HeaderText="<%$ Resources:lblResolvedName_g %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="AddressLine1" HeaderText="<%$ Resources:lblAddressLine1_g %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="AddressLine2" HeaderText="<%$ Resources:lblAddressLine2_g %>"></asp:BoundField>
                            <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:lblPostcode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Type" HeaderText="<%$ Resources:lblType_g %>"></asp:BoundField>
                            <asp:BoundField DataField="PartySourceDescription" HeaderText="<%$ Resources:lblPartySourceDescription_g %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("UserName") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="lnkDetails" runat="server" SkinID="btnGrid" Text="<%$ Resources:lblDetails %>"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PartyTypeId" HeaderStyle-CssClass ="Hidden"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <nexus:ProgressIndicator ID="upFindOtherParty" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindOtherParty" runat="server">
            <progresstemplate>
            </progresstemplate>
        </nexus:ProgressIndicator>
    </div>
</asp:Content>

