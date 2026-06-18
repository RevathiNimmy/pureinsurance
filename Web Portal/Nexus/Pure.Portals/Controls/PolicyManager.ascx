<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PolicyManager.ascx.vb" Inherits="Nexus.Controls_PolicyManager" %>
<%@ Register Src="~/Controls/CtrlLetterWriting.ascx" TagName="LetterWriting" TagPrefix="uc01" %>

<div id="Controls_PolicyManager">
    <div class="row row-sm">
        <div class="col-sm-3">
            <div class="card card-secondary no-margin">
                <div class="card-heading">
                    <h4>
                        <asp:Literal ID="lblHeading" runat="server" Text="Policy Navigator"></asp:Literal></h4>
                </div>
                <div class="card-body no-padding">
                    <div class="grey-50 p-sm">
                        <asp:UpdatePanel ID="updBtnSearch" runat="server">
                            <ContentTemplate>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="txtPolicyNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnSearch" runat="server" SkinID="btnModal" Text="<%$ Resources:lbl_btnSearch %>"></asp:LinkButton>
                                    </span>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="ProgressIndicator3" OverlayCssClass="updating" AssociatedUpdatePanelID="updPolicyTree" runat="server">

                            <progresstemplate>
                        </progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                    <div id="policy-navigator" class="scrollable" style="height: 400px;">
                        <asp:UpdatePanel ID="updPolicyTree" runat="server">
                            <ContentTemplate>
                                <%-- <asp:TreeView runat="server" ID="tvClientPolicy" ViewStateMode="Enabled" SelectedNodeStyle-CssClass="TreeView-Selected" CssClass="treeview-parent" OnSelectedNodeChanged="tvClientPolicy_SelectedNodeChanged" SkinID="Simple" ExpandDepth="1">
                            </asp:TreeView>--%>
                                <asp:TreeView runat="server" ID="tvClientPolicy" ViewStateMode="Enabled" OnSelectedNodeChanged="tvClientPolicy_SelectedNodeChanged" ExpandDepth="1">
                                </asp:TreeView>
                                <asp:HiddenField ID="hvInsuranceFolderKey" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvSelectedTreeNode" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvMarketPlacePolicy" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvRetainPolNum" runat="server"></asp:HiddenField>                             
								  <asp:HiddenField ID="hvVoidConfirm" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hvVoidMessage" runat="server"></asp:HiddenField>   
                            </ContentTemplate>

                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="ProgressIndicator1" OverlayCssClass="updating" AssociatedUpdatePanelID="updPolicyTree" runat="server">
                            <progresstemplate>
                        </progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-9">

            <asp:UpdatePanel ID="updPolicyView" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card card-secondary m-b-md">
                        <div class="card-heading">
                            <h4>
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:lbl_Quote_header %>"></asp:Literal>
                            </h4>
                        </div>
                        <div class="card-body no-padding">
                            <asp:Panel ID="PanelViewAllQuotesPolicies" runat="server" CssClass="grey-50 p-sm">
                                <asp:CheckBox ID="chkViewAllPolicies" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                <asp:Label ID="lbl_ViewAllPolicies" runat="server" Text="Include Cancelled Quotes/Voided Versions/Quote History Versions"></asp:Label>
                                <asp:HiddenField ID="hvGridIDs" runat="server" EnableViewState="true"></asp:HiddenField>
                            </asp:Panel>
                            <div class="grid-card table-responsive no-margin">
                                <asp:GridView ID="gvPolicyVersions" runat="server" AutoGenerateColumns="False" DataKeyNames="InsuranceFolderKey,BaseInsuranceFolderKey,InsuranceFileKey,InsuranceFileTypeCode,MarkedQuoteForCollection,PolicyStatusCode,IsCurrent,QuoteExpiryDate,Reference,CoverStartDate,EventDesc,IsMarketPlacePolicy,BaseInsuranceFileKey,IsMigratedPolicy,IsReadOnly" AutoGenerateSelectButton="true" OnSelectedIndexChanged="gvPolicyVersions_SelectedIndexChanged" OnRowDataBound="gvPolicyVersions_RowDataBound" GridLines="None" PageSize="5" SelectedRowStyle-CssClass="AspNet-Gridview-Selected" AllowPaging="true" AllowSorting="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" EnableViewState="true" EnablePersistedSelection="true">
                                    <Columns>
                                        <asp:BoundField DataField="CoverStartDate" SortExpression="CoverStartDate" HeaderText="<%$ Resources:lbl_CoverFrom %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>

                                        <asp:BoundField DataField="RenewalDate" SortExpression="RenewalDate" HeaderText="<%$ Resources:lbl_RenewalDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                        <asp:BoundField DataField="LapseCancelDate" SortExpression="LapseCancelDate" HeaderText="<%$ Resources:lbl_LapsedDate %>" HtmlEncode="false" DataFormatString="{0:d}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                        <asp:BoundField DataField="DateIssued" SortExpression="DateIssued" HeaderText="<%$ Resources:lbl_DateIssued %>" DataFormatString="{0:d}"></asp:BoundField>

                                        <asp:BoundField DataField="Intermediary" SortExpression="Intermediary" HeaderText="<%$ Resources:lbl_AgentName %>"></asp:BoundField>
                                        <asp:BoundField DataField="InsuranceFileTypeDesc" SortExpression="InsuranceFileTypeDesc" HeaderText="<%$ Resources:lbl_PolicyType %>"></asp:BoundField>
                                        <asp:BoundField DataField="EventDesc" SortExpression="EventDesc" HeaderText="<%$ Resources:lbl_EventDescription %>"></asp:BoundField>
                                        <%--<asp:BoundField DataField="InsuranceFileTypeCode" SortExpression="PolicyStatus" HeaderText="<%$ Resources:lbl_Status %>" />--%>
                                        <asp:TemplateField HeaderText="<%$ Resources:lbl_Status %>" SortExpression="PolicyStatus">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Status" runat="server" Text='<%# If((CType(Eval("IsCurrent"), Boolean) = True AndAlso (Not Eval("PolicyStatus").ToUpper().Equals("LAPSED")) AndAlso (Not Eval("PolicyStatus").ToUpper().Equals("CANCELLED"))), GetLocalResourceObject("lbl_CurrentStatus"), If(Eval("PolicyStatus").ToUpper().Equals("LAPSED"), GetLocalResourceObject("lbl_Lapsed"), Eval("PolicyStatus")))%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TransactionDate" SortExpression="TransactionDate" HeaderText="<%$ Resources:lbl_TransactionDate %>" HtmlEncode="false" DataFormatString="{0:G}" ItemStyle-CssClass="span-4"></asp:BoundField>
                                        <asp:BoundField DataField="PolicyVersion" SortExpression="PolicyVersion" HeaderText="Version"></asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:lbl_AssociatedClients %>" SortExpression="AssociatedClients" ItemStyle-CssClass="span-4" Visible="false">                                           
                                            <ItemTemplate>
                                                <asp:Label ID="lblMasterClientName" runat="server" Text='<%# Eval("InsuredPersons")%>' Style="background-color: inherit; color: Red;" Font-Bold="true"></asp:Label>                          
                                                <asp:Repeater ID="rptrAssociateClient" runat="server">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssociateClientName" runat="server" Text='<%#CType(Container.DataItem, System.Xml.XmlElement).GetAttribute("Name")%>' Width ="200px"></asp:Label>
                                                        <br />
                                                    </ItemTemplate>
                                                    </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="card-footer text-right grey-100">
                            <asp:TextBox ID="hdMarkedtext" runat="server" Style="display: none"></asp:TextBox>
							<asp:LinkButton ID="btnVoid" runat="server" Text="<%$ Resources:lbl_Void %>" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnEdit" runat="server" Text="<i class='fa fa-pencil' aria-hidden='true'></i> Edit" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnBuy" runat="server" Text="<i class='fa fa-shopping-cart' aria-hidden='true'></i> Buy" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnCopy" runat="server" Text="<i class='fa fa-copy' aria-hidden='true'></i> Copy" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnChange" runat="server" Text="<i class='fa fa-exchange'></i> Change" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnView" runat="server" Text="<i class='fa fa-eye' aria-hidden='true'></i> View" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnDetails" runat="server" Text="Details" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnClaim" runat="server" Text="<i class='fa fa-legal'></i> Claim" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <asp:LinkButton ID="btnReinstatement" runat="server" Text="Reinstatement" CausesValidation="true" SkinID="btnGrid"></asp:LinkButton>
                            <uc01:LetterWriting ID="ctrlLetterWriting" runat="server" Visible="false"></uc01:LetterWriting>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvPolicyVersions" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvPolicyVersions" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvPolicyVersions" EventName="RowEditing"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvRisks" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <Nexus:ProgressIndicator ID="UpPnlEvents" OverlayCssClass="updating" AssociatedUpdatePanelID="updPolicyView" runat="server">
                <progresstemplate>
                </progresstemplate>
            </Nexus:ProgressIndicator>

            <div class="card card-secondary no-margin">
                <div class="card-heading">
                    <h4>
                        <span id="Span1" runat="server" title="<%$ Resources:titleExpandCollapse %>">
                            <asp:Literal ID="Literal1" runat="server" Text="Risks"></asp:Literal>
                        </span>
                    </h4>
                </div>
                <div class="card-body p-xs">
                    <div class="grid-card table-responsive no-margin">
                        <asp:UpdatePanel ID="updRisks" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvRisks" runat="server" AutoGenerateColumns="False" DataKeyNames="Key" GridLines="None" PageSize="5" AllowPaging="true" AllowSorting="false" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:RiskErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField DataField="RiskNumber" SortExpression="RiskNumber" HeaderText="<%$ Resources:lblRiskNumber %>" />
                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="<%$ Resources:lblRiskDescription %>"></asp:BoundField>
                                        <Nexus:BoundField DataField="TotalSumInsured" SortExpression="TotalSumInsured" HeaderText="<%$ Resources:lblSumInsured %>" DataType="Currency"></Nexus:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:lblGrossPremium %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltPremiumAmount" runat="server"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StatusCode" SortExpression="StatusCode" HeaderText="<%$ Resources:lblRiskStatus %>"></asp:BoundField>
                                        <asp:BoundField DataField="RiskLinkStatusFlag" HeaderText="<%$ Resources:lbl_LinkStatus %>"></asp:BoundField>
                                        <asp:BoundField DataField="RiskLinkChangeDate" HeaderText="<%$ Resources:lbl_LinkDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <div class="rowMenu">
                                                    <ol id='menu_<%# Eval("Key") %>' class="list-inline no-margin">
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnSelect" runat="server" CausesValidation="False" CommandName="ViewRisk" CommandArgument='<%#Eval("Key")%>' Text="<i class='fa fa-eye' aria-hidden='true'></i> View" SkinID="btnGrid"></asp:LinkButton></li>
                                                    </ol>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="ProgressIndicator2" OverlayCssClass="updating" AssociatedUpdatePanelID="updPolicyView" runat="server">
                            <progresstemplate>
                        </progresstemplate>
                        </Nexus:ProgressIndicator>
                    </div>
                </div>
            </div>
        </div>
</div>
</div>
