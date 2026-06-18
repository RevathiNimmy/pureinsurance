<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BrokerView.aspx.vb" Inherits="Nexus.secure_BrokerView"
    MasterPageFile="~/Default.master" EnableViewState="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/FindParty.ascx" TagName="FindParty" TagPrefix="NexusPartyControl" %>
<%@ Register Src="~/Controls/SubGrid.ascx" TagName="SubGrid" TagPrefix="NexusSubGrid" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">

    <script type="text/javascript" language="javascript">


        function setAgent(sName, sKey, sCode) {
            tb_remove();
            document.getElementById('<%= txtAgentName.ClientId%>').value = unescape(sName);
            document.getElementById('<%= txtAgentKey.ClientId%>').value = sKey;
            document.getElementById('<%= txtAgentName.ClientId%>').focus();
            document.getElementById('<%=hvAgentName.ClientID %>').value = unescape(sName);
            document.getElementById('<%=hvAgentKey.ClientID %>').value = sKey;

        }
        $(document).ready(function () {

            collapseAllGrid();
            showHideLi();
            document.getElementById('divSubBroker').style.display = 'none';
        });

        function showHideLi() {

            if (document.getElementById('<%=hvIsBroker.ClientID %>').value == '1')
                document.getElementById('liAgent').style.display = 'none';
            else
                document.getElementById('liAgent').style.display = 'block';
        }

        function collapseAllGrid() {
            var ids = document.getElementById('<%=hvGridIDs.ClientID %>').value;
            var array = ids.split(',');
            var i = 0;
            for (i = 0; i < array.length; i++) {
                if (document.getElementById(array[i]) != null) {
                    document.getElementById(array[i]).style.display = 'none';
                }
            }
        }

        function toggle(obj, flag) {
            if (flag) {
                document.getElementById(obj.substr(0, 35) + 'grdvSubBroker').style.display = 'none';
                document.getElementById(obj).style.display = 'none';
                document.getElementById(obj.substr(0, 38) + 'Expand').style.display = 'block';

            }
            else {
                document.getElementById(obj.substr(0, 35) + 'grdvSubBroker').style.display = 'block';
                document.getElementById(obj).style.display = 'block';

                document.getElementById(obj.substr(0, 38) + 'Collapse').style.display = 'block';
                document.getElementById(obj.substr(0, 38) + 'Expand').style.display = 'none';
            }


        }

    </script>
    <asp:ScriptManager ID="scriptManager" runat="server"></asp:ScriptManager>
    <div class="Brokerview">
        <asp:Panel ID="pnlsearch" runat="server" DefaultButton="btnFilter" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <asp:HiddenField ID="hvGridIDs" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvIsBroker" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hvMarketPlacePolicy" runat="server"></asp:HiddenField>
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRecordType" runat="server" AssociatedControlID="ddlRecordType" Text="<%$ Resources:lbl_RecordType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlRecordType" runat="server" CssClass="field-medium form-control form-select" TabIndex="1">
                                <asp:ListItem Text="<%$ Resources:li_All %>" Value="All"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:li_Policy %>" Value="Policy"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:li_Quote %>" Value="Quote"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProduct" runat="server" AssociatedControlID="ddlProductType" Text="<%$ Resources:lbl_Product %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlProductType" runat="server" CssClass="field-medium form-control form-select" TabIndex="2">
                                <asp:ListItem Text="<%$ Resources:li_All %>" Value="All"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="li_logintype" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblResults" runat="server" AssociatedControlID="ddlResults" Text="<%$ Resources:lbl_Results %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlResults" runat="server" CssClass="field-medium form-control form-select" TabIndex="3">
                                <asp:ListItem Text="<%$ Resources:li_User_Only %>" Value="UserOnly"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:li_All %>" Value="All"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" Text="<%$ Resources:lbl_PolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" TabIndex="4" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" id="liAgent">
                        <asp:UpdatePanel ID="POLICYHEADER__updPanelAgent" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblAgent" AssociatedControlID="txtAgentName" Text="Agent Name" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtAgentName" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnAgent" runat="server" SkinID="btnModal" OnClientClick="tb_show(null , '../Modal/FindAgent.aspx?FromPage=MainDetails&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                                <i class="fa fa-search"></i>
                                                <span class="btn-fnd-txt">Agent Name</span>
                                            </asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </ContentTemplate>

                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="upSearchAgent" OverlayCssClass="updating" AssociatedUpdatePanelID="POLICYHEADER__updPanelAgent" runat="server" Visible="false">
                            <progresstemplate>
                                            </progresstemplate>
                        </Nexus:ProgressIndicator>
                        <asp:HiddenField ID="hvAgentName" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="txtAgentKey" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvAgentKey" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvVoidConfirm" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvVoidMessage" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lbl_Name %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" TabIndex="6" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblQuoteDate" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblQuoteLiveDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtQuoteDate" TabIndex="7" CssClass="field-date form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="dtQuoteLiveDate" runat="server" LinkedControl="txtQuoteDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtStartDate" TabIndex="8" CssClass="field-date form-control" runat="server"></asp:TextBox><uc1:CalendarLookup ID="dtStartDate" runat="server" LinkedControl="txtStartDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                    </div>
                     <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskIndex" runat="server" AssociatedControlID="txtRiskIndex" Text="<%$ Resources:lbl_RiskIndex %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtRiskIndex" TabIndex="9" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="New Search" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnFilter" runat="server" Text="<%$ Resources:lbl_Filter %>" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </asp:Panel>
        <asp:ValidationSummary ID="vldRenewalManager" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtPolicyNumber,txtName" Condition="Auto" Display="none" runat="server" EnableClientScript="true"></Nexus:WildCardValidator>
        <asp:Panel ID="pnl_grdvBroker" runat="server" Visible="false">
            <div class="fieldset-wrapper">

                <fieldset>
                    <legend><span id="pnlQuotesPoliciesTitle" runat="server" class="expander" title="<%$ Resources:titleExpandCollapse %>">
                        <asp:Literal ID="lbl_Quote_header" runat="server" Text="<%$ Resources:lbl_Quote_header %>"></asp:Literal></span></legend>
                    <asp:Panel ID="pnl_QuotesPolicies" runat="server">
                        <div class="showall">
                            <asp:Panel ID="PanelViewAllQuotesPolicies" runat="server">
                                <asp:CheckBox ID="chkViewAllPolicies" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                <asp:Label ID="lbl_ViewAllPolicies" runat="server" Text="<%$ Resources:lbl_ViewAllPolicies %>"></asp:Label>
                            </asp:Panel>
                        </div>
                        <asp:Label ID="lbl_FD_MTAExist" runat="server" Text="<%$ Resources:lbl_FD_MTAExistMsg %>" Visible="false" CssClass="error"></asp:Label>
                    </asp:Panel>
                </fieldset>
            </div>

            <div class="grid-card table-responsive">
                <asp:GridView ID="grdvBroker" runat="server" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,BaseInsuranceFolderKey" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle CssClass="border"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="ImgCollapse" runat="server" ImageUrl="~/images/collapse.gif" border="0" Style="display: none;" onClick="toggle(this.id,1);" Visible="false"></asp:Image>
                                <asp:Image ID="ImgExpand" runat="server" ImageUrl="~/images/expand.gif" border="0" onClick="toggle(this.id,0);" Visible="false"></asp:Image>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="InsuranceFileTypeCode" SortExpression="InsuranceFileTypeCode" HeaderText="<%$ Resources:lbl_Status %>" ItemStyle-CssClass="span-3" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="<%$ Resources:lbl_Reference %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                        <asp:TemplateField HeaderText="Status" SortExpression="InsuranceFileTypeCode">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Status" runat="server" Text='<%# If(CType(Eval("IsCurrent"), Boolean) = True, GetLocalResourceObject("lbl_CurrentStatus"), Eval("PolicyStatus"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RiskStatus" SortExpression="RiskStatus" HeaderText="<%$ Resources:lbl_RiskStatus %>"></asp:BoundField>
                        <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="<%$ Resources:lbl_Type %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                        <asp:BoundField DataField="DateIssued" SortExpression="DateIssued" HeaderText="<%$ Resources:lbl_DateIssued %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                        <asp:BoundField DataField="LeadAgent" SortExpression="LeadAgent" HeaderText="<%$ Resources:lbl_AgentName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                        <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="<%$ Resources:lbl_InsurerShortName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                        <asp:BoundField DataField="CoverStartDate" SortExpression="CoverStartDate" HeaderText="<%$ Resources:lbl_StartDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                        <asp:BoundField DataField="ExpiryDate" SortExpression="ExpiryDate" HeaderText="<%$ Resources:lbl_EndDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-3">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnSelect" runat="server" CausesValidation="False" CommandName="Select"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-2">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnSelect2" runat="server" CausesValidation="False" CommandName="Select"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-2" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnCopyQuote" runat="server" Visible="false" Text="<%$ Resources:lbl_CopyQuote %>" CommandName="CopyQuote" CausesValidation="False"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-2" >
                            <ItemTemplate>
                                 <asp:LinkButton ID="lnkbtnVoid" runat="server" Visible="false" Text="<%$ Resources:lbl_Void %>" CommandName="VoidPolicy" CausesValidation="False"></asp:LinkButton>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td colspan="100%">
                                        <div style="position: relative; background-color: White; overflow: auto; left: 0px; max-height: 200px;" id="divSubBroker">
                                            <asp:GridView ID="grdvSubBroker" runat="server" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,InsuranceFileKey" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="20" AllowPaging="False" AllowSorting="true" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" Width="100%" OnRowCommand="grdvSubBroker_RowCommand" OnRowDataBound="grdvSubBroker_RowDataBound" ShowFooter="false">
                                                <Columns>
                                                    <asp:BoundField DataField="Reference" HeaderText="<%$ Resources:lbl_Reference %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_ChildStatus" runat="server" Text='<%#Eval("InsuranceFileTypeCode")%>'></asp:Label>
                                                            <%--  <asp:BoundField DataField="InsuranceFileTypeCode" SortExpression="InsuranceFileTypeCode"
                                                                    HeaderText="<%$ Resources:lbl_Status %>" ItemStyle-CssClass="span-3" />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:lbl_Type %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                    <asp:BoundField DataField="DateIssued" HeaderText="<%$ Resources:lbl_DateIssued %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                                    <asp:BoundField DataField="LeadAgent" HeaderText="<%$ Resources:lbl_AgentName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                    <asp:BoundField DataField="ClientName" HeaderText="<%$ Resources:lbl_InsurerShortName %>" ItemStyle-CssClass="span-2"></asp:BoundField>
                                                    <asp:BoundField DataField="CoverStartDate" HeaderText="<%$ Resources:lbl_StartDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                                    <asp:BoundField DataField="ExpiryDate" HeaderText="<%$ Resources:lbl_EndDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="span-3">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnDetails" runat="server" CausesValidation="False" CommandName="Details" Text="Details" CommandArgument='<%#Eval("InsuranceFileKey") %>'></asp:LinkButton>
                                                            <%-- <asp:LinkButton ID="lnkbtnDetails" runat="server" CausesValidation="False" CommandName="Details"
                                                                            Text="Details" CommandArgument='<%#Eval("BaseInsuranceFolderKey") %>' />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblInsuranceFolderKey" runat="server" CausesValidation="False" Text='<%#Eval("InsuranceFolderKey") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
