<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" CodeFile="FindPolicy.aspx.vb" Inherits="Nexus.secure_FindPolicy" %>

<%@ Register Src="~/controls/FindParty.ascx" TagName="FindParty" TagPrefix="NexusPartyControl" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        function setAgent(sName, sKey, sCode) {
            tb_remove();
            $('#<%= txtAgentName.ClientId%>').val(unescape(sName));
            $('#<%= hfAgentName.ClientID%>').val(unescape(sName));
            $('#<%= txtAgentName.ClientId%>').focus();
            $('#<%=hfAgentKey.ClientID%>').val(sKey);
        }
        function selectPolicyVersion(postbacktarget, commandName, insuranceFileKey, markedQuoteForCollection, isMarketPlacePolicy) { //closes the modal, triggering a postback
            hide_modal();
            $('#<%= hfPolicyVersionInsuranceFileKey.ClientID%>').val(insuranceFileKey);
            $('#<%= hfAnswerForMarkedQuoteCollection.ClientID%>').val(markedQuoteForCollection);
            $('#<%= hfAnswerForMarketPlacePolicy.ClientID%>').val(isMarketPlacePolicy);
            __doPostBack(postbacktarget, commandName);
        }
    </script>
    <div class="FindPolicy">
        <asp:Panel ID="pnlsearch" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">

                <div class="form-horizontal">


                    <asp:HiddenField ID="hfPolicyVersionInsuranceFileKey" Value="" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfAnswerForMarkedQuoteCollection" Value="False" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfAnswerForMarketPlacePolicy" Value="False" runat="server"></asp:HiddenField>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRecordType" runat="server" AssociatedControlID="ddlRecordType" Text="<%$ Resources:lbl_RecordType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlRecordType" runat="server" CssClass="field-medium form-control form-select" TabIndex="1">
                                <asp:ListItem Text="<%$ Resources:li_All %>" Value="ALL"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:li_Policy %>" Value="POLICY"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:li_Quote %>" Value="QUOTE"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvRecordType" runat="server" ControlToValidate="ddlRecordType" Enabled="false" Display="None" InitialValue="All" ErrorMessage="<%$ Resources:lbl_vldMsgRecordType %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProduct" runat="server" AssociatedControlID="ddlProductType" Text="<%$ Resources:lbl_Product %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlProductType" runat="server" CssClass="field-medium form-control form-select" TabIndex="2">
                                
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ControlToValidate="ddlProductType" Enabled="false" Display="None" InitialValue="All" ErrorMessage="<%$ Resources:lbl_vldMsgProduct %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" Text="<%$ Resources:lbl_PolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" TabIndex="3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="txtPolicyNumber" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_vldMsgPolicyNumber %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblAgent" AssociatedControlID="txtAgentName" Text="<%$ Resources:lbl_btnFindAgent %>" class="col-md-4 col-sm-3 control-label">
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtAgentName" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnAgent" runat="server" TabIndex="4" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Agent Name</span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvAgentName" runat="server" ControlToValidate="txtAgentName" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_vldMsgAgentName %>"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hfAgentKey" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfAgentName" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInsuredName" runat="server" AssociatedControlID="txtInsuredName" Text="<%$ Resources:lbl_InsuredName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtInsuredName" CssClass="form-control" runat="server" TabIndex="5"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvInsuredName" runat="server" ControlToValidate="txtInsuredName" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_vldMsgInsuredName %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblQuoteDate" runat="server" AssociatedControlID="txtQuoteDate" Text="<%$ Resources:lblQuoteLiveDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtQuoteDate" CssClass="field-date form-control" runat="server" TabIndex="6"></asp:TextBox><uc1:CalendarLookup ID="dtQuoteLiveDate" runat="server" LinkedControl="txtQuoteDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvQuoteDate" runat="server" ControlToValidate="txtQuoteDate" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_vldMsgQuoteDate %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverStartDate" runat="server" AssociatedControlID="txtCoverStartDate" Text="<%$ Resources:lbl_CoverStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtCoverStartDate" TabIndex="7" CssClass="field-date form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="dtCoverStartDate" runat="server" LinkedControl="txtCoverStartDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvCoverStartDate" runat="server" ControlToValidate="txtCoverStartDate" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_vldMsgCoverStartDate %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRiskIndex" CssClass="form-control" runat="server" TabIndex="8"></asp:TextBox>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" TabIndex="9" Text="<%$ Resources:lbl_btnNewSearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="8" Text="<%$ Resources:lbl_btnSearch %>" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </asp:Panel>
        <asp:ValidationSummary ID="vldFindPolicy" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtPolicyNumber,txtInsuredName" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>

        <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">
            <div class="grid-card table-responsive">
                <asp:UpdatePanel ID="updSearchResults" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdSearchResult" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="0" CellSpacing="0" AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:lbl_NoResultMessage %>" EmptyDataRowStyle-CssClass="noData" DataKeyNames="InsuranceFileKey">
                            <Columns>
                                <asp:BoundField DataField="InsuranceRef" SortExpression="InsuranceRef" HeaderText="<%$ Resources:gvh_PolicyReference %>"></asp:BoundField>
                                <asp:TemplateField HeaderText="<%$ Resources:gvh_PolicyStatus %>" SortExpression="InsuranceFileStatusDescription">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Status" runat="server" Text='<%# Eval("InsuranceFileStatusDescription")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="InsuranceFileType" SortExpression="InsuranceFileType" HeaderText="<%$ Resources:gvh_PolicyType %>"></asp:BoundField>
                                <asp:BoundField DataField="ProductDescription" SortExpression="ProductDescription" HeaderText="<%$ Resources:gvh_ProductName %>"></asp:BoundField>
                                <asp:BoundField DataField="IssuedDate" SortExpression="IssuedDate" HeaderText="<%$ Resources:gvh_QuoteDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                <asp:BoundField DataField="LeadAgentName" SortExpression="LeadAgentName" HeaderText="<%$ Resources:gvh_AgentName %>"></asp:BoundField>
                                <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="<%$ Resources:gvh_InsuredName %>"></asp:BoundField>
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
                                <asp:BoundField DataField="CoverFrom" SortExpression="CoverFrom" HeaderText="<%$ Resources:gvh_CoverFromDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                <asp:BoundField DataField="RenewalDate" SortExpression="RenewalDate" HeaderText="<%$ Resources:gvh_RenewalDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                <asp:BoundField DataField="TransactionDate" SortExpression="TransactionDate" HeaderText="<%$ Resources:gvh_TransactionDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                    <ol id='menu_<%# Eval("InsuranceFileKey") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li>
                                                            <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:lbl_View %>" CommandName="View" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnVersions" runat="server" Enabled='<%# If(CType(Eval("NoOfVersions"),Integer) > 1, "True", "False") %>' Text="<%$ Resources:lbl_Versions %>" CausesValidation="False" CommandName="Versions"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnAction" runat="server" Text="<%$ Resources:lbl_Change %>" CommandName="Change" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                        <li id="liRenew" runat="server" visible="false">
                                                            <asp:LinkButton ID="lnkbtnRenew" runat="server" Text="<%$ Resources:lbl_Renew %>" CommandName="Renew" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false"></asp:LinkButton>
                                                        </li>
                                                    </ol>
                                                </li>
                                            </ol>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
