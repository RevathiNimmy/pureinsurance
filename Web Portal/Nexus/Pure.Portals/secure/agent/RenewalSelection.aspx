<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RenewalSelection.aspx.vb"
    Inherits="Nexus.secure_RenewalSelection" Title="Renewal Selection" MasterPageFile="~/Default.master"
    EnableViewState="true" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">

    <script language="javascript" type="text/javascript">
        function setClient(sClientName, sClientKey) {
            tb_remove();
            document.getElementById('<%= txtClient.ClientId%>').value = unescape(sClientName);
            document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
            document.getElementById('<%= txtClient.ClientId%>').focus();

        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="secure_agent_RenewalSelection">
        <asp:UpdatePanel ID="updRenewalSelectionUI_LAyout" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
            <ContentTemplate>
                <div class="card">
                    <div class="card-heading">
                        <h1>
                            <asp:Literal ID="litRenewalSelectionHeader" runat="server" Text="<%$ Resources:lbl_RenewalSelection_header%>" EnableViewState="false"></asp:Literal>
                        </h1>
                    </div>
                    <asp:Panel ID="PnlRenewalSelectionFile" runat="server" CssClass="card-body clearfix" DefaultButton="btnFindNow">
                        <div class="form-horizontal">
                            <legend><span>
                                <asp:Literal ID="lblHeader" runat="server" Text="<%$ Resources:lbl_SubHeader %>"></asp:Literal></span></legend>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblReference" runat="server" AssociatedControlID="txtReference" Text="<%$ Resources:lbl_Reference %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtReference" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtRiskIndex" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label runat="server" ID="lblHandler" AssociatedControlID="txtClient" Text="<%$ Resources:btn_Client %>" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtClient" runat="server" CssClass="form-control" TabIndex="2" MaxLength="20"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnClient" runat="server" TabIndex="1" SkinID="btnModal">
                                                <i class="fa fa-search"></i>
                                                 <span class="btn-fnd-txt">Client</span>
                                            </asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btn_NewSearch%>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:btn_FindNow%>" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </div>
                <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtReference,txtRiskIndex,txtClient" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
                </Nexus:WildCardValidator>
                <asp:CustomValidator ID="custValidate" runat="server" CssClass="error" SetFocusOnError="true" Display="None"></asp:CustomValidator>
                <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                <asp:Panel CssClass="fieldset-wrapper" ID="PnlMessage" runat="server" Visible="false">
                    <asp:Label runat="server" ID="lblMessage"></asp:Label>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnNewSearch" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="PIRenewalSelectionUI_LAyout" OverlayCssClass="updating" AssociatedUpdatePanelID="updRenewalSelectionUI_LAyout" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </Nexus:ProgressIndicator>
        <asp:UpdatePanel ID="UpdSearchResults" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" EnableViewState="true" AutoGenerateColumns="false" DataKeyNames="IsMarketPlacePolicy" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="InsuranceRef" HeaderText="<%$ Resources:lbl_FolderReference%>"></asp:BoundField>
                            <asp:BoundField DataField="ProductDescription" HeaderText="<%$ Resources:lbl_Product %>"></asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="<%$ Resources:lbl_Type %>"></asp:BoundField>
                            <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_InsuranceHolder %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AssociatedClients %>" SortExpression="AssociatedClients"
                                ItemStyle-CssClass="span-4">
                                <ItemTemplate>
                                    <asp:Repeater ID="rptrAssociateClient" runat="server">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssociateClientName" runat="server" Text='<%#CType(Container.DataItem, System.Xml.XmlElement).GetAttribute("Name")%>'></asp:Label>
                                            <br />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <SeparatorTemplate>
                                        </SeparatorTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LastModifiedDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:lbl_DateModified %>"></asp:BoundField>
                            

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("InsuranceFileKey") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="lnkSelect" runat="server" CommandName="select" Text="<%$ Resources:lbl_lnkSelect %>" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceFolderKey" runat="server" CausesValidation="False" Text='<%#Eval("InsuranceFolderKey") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsuranceFileKey" runat="server" CausesValidation="False" Text='<%#Eval("InsuranceFileKey")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="UpSearchResult" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdSearchResults" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
