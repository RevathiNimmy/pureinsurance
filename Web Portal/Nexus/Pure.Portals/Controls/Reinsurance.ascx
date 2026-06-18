<%@ Control Language="VB" AutoEventWireup="True" CodeFile="Reinsurance.ascx.vb" Inherits="Nexus.Control_Reinsurance" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<div id="Controls_Reinsurance">
    <script type="text/javascript">
        function addFac(key, code, name, commissionPerc, taxPerc) {
            $('#<%= hfKey.ClientID %>').val(key);
            $('#<%= hfCode.ClientID%>').val(code);
            $('#<%= hfName.ClientID%>').val(name);
                   $('#<%= hfCommissionPercentage.ClientID%>').val(commissionPerc);
            $('#<%= hfTaxPercentage.ClientID%>').val(taxPerc);
            __doPostBack('', 'AddFac');
        }
        function addTreaty(key, code, name) {
            $('#<%= hfKey.ClientID %>').val(key);
            $('#<%= hfCode.ClientID%>').val(code);
            $('#<%= hfName.ClientID%>').val(name);
            __doPostBack('', 'AddTreaty');
        }
         function clearTabAndSubmit() {
            localStorage.removeItem('activeMainTab');
            __doPostBack('<%= btnOk.UniqueID %>', '');
        }

    </script>

    <div class="card">
        <asp:HiddenField ID="hfKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfCode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfName" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfCommissionPercentage" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hfTaxPercentage" runat="server"></asp:HiddenField>

        <div class="card-body clearfix">
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
                    <asp:UpdatePanel runat="server" ID="uplDetail" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblRIOverrideReason" runat="server" AssociatedControlID="GISLookup_RIOverrideReason" Text="<%$ Resources:lblRIOverrideReason %>" class="col-md-4 col-sm-3 control-label" Visible="false"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="GISLookup_RIOverrideReason" runat="server" AutoPostBack="true" DefaultText=" " DataItemText="Description" DataItemValue="Key" ListCode="Ri_Override_Reason" ListType="PMLookup" Sort="Asc" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
            <div>
                <asp:UpdatePanel ID="UpdatePanelReinsurance" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvReinsurance" runat="server" AutoGenerateColumns="False" GridLines="None" AllowPaging="false" ShowHeader="True" EmptyDataRowStyle-CssClass="noData" DataKeyNames="RIArrangementLineKey,PartyKey,TreatyCode" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblName %>"></asp:BoundField>
                                    <Nexus:BoundField DataField="DefaultPerc" HeaderText="<%$ Resources:lblDefault %>" DataType="Percentage"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ThisPerc" HeaderText="<%$ Resources:lblThis %>" DataType="Percentage"></Nexus:BoundField>
                                    <Nexus:TemplateField HeaderText="<%$ Resources:lblSumInsured %>" DataType="Currency">
                                        <itemtemplate>
                                            <asp:TextBox ID="txtSumInsured" runat="server" Visible="false" Text='<%# Eval("SumInsured")%>' OnTextChanged="txtSumInsured_TextChanged" AutoPostBack="true" CssClass="form-control e-num2" interactive="0"></asp:TextBox>
                                            <asp:Label ID="lblSumInsured" runat="server" Visible="false" Text='<%# Eval("SumInsured")%>' CssClass="Doub"></asp:Label>
                                        </itemtemplate>
                                    </Nexus:TemplateField>
                                    <Nexus:TemplateField HeaderText="<%$ Resources:lblPremium %>" DataType="Currency">
                                        <itemtemplate>
                                            <asp:TextBox ID="txtPremium" runat="server" Visible="false" Text='<%# Format(Math.Round(CDbl(Eval("Premium")), 2), "#,#0.00")%>'  OnTextChanged="txtPremium_TextChanged" AutoPostBack="true" CssClass="form-control e-num2" interactive="0"></asp:TextBox>
                                            <asp:Label ID="lblPremium" runat="server" Visible="false" Text='<%# Format(Math.Round(CDbl(Eval("Premium")), 2), "#,#0.00")%>' CssClass="Doub"></asp:Label>
                                        </itemtemplate>
                                    </Nexus:TemplateField>
                                    <Nexus:BoundField DataField="Tax" HeaderText="<%$ Resources:lblTax %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:TemplateField HeaderText="<%$ Resources:lblCommissionPercentage %>" ItemStyle-CssClass="Perc" HeaderStyle-CssClass="Perc" DataType="Percentage">
                                        <itemtemplate>
                                            <asp:TextBox ID="txtCommissionPerc" runat="server" Visible="false" Text='<%# String.Format("{0}{1}", Eval("CommissionPerc"), "%")%>' OnTextChanged="txtCommissionPerc_TextChanged" AutoPostBack="true" CssClass="PercTextBox" interactive="0"></asp:TextBox>
                                            <asp:RangeValidator ID="rngCommissionPerc" runat="server" Display="None" ControlToValidate="txtCommissionPerc" MinimumValue="0.0001" MaximumValue="100" Type="Double" ErrorMessage="<%$ Resources:err_InvalidCommissionPercent %>" Enabled="false"></asp:RangeValidator>
                                            <asp:Label ID="lblCommissionPerc" runat="server" Visible="false" Text='<%# String.Format("{0}{1}", Eval("CommissionPerc"), "%")%>' HtmlEncode="false" CssClass="Perc"></asp:Label>
                                        </itemtemplate>
                                    </Nexus:TemplateField>
                                    <Nexus:BoundField DataField="Commission" HeaderText="<%$ Resources:lblCommission %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="CommissionTax" HeaderText="<%$ Resources:lblCommTax %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:TemplateField HeaderText="<%$ Resources:lblAgreement %>">
                                        <itemtemplate>
                                            <asp:TextBox ID="txtAgreement" runat="server" Visible="false" Text='<%# Eval("Agreement")%>' CssClass="form-control"  OnTextChanged="txtAgreement_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Label ID="lblAgreement" runat="server" Visible="false" Text='<%# Eval("Agreement")%>'></asp:Label>
                                        </itemtemplate>
                                    </Nexus:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id='menu_<%# Eval("Name")%>' class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="<%$ Resources:lblDelete %>" Visible="false" CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <Nexus:BoundField DataField="DefaultLine" HeaderText="" DataType="Integer"></Nexus:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <p></p>
                            <div class="form-horizontal">
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblOrgReinsurance" runat="server" AssociatedControlID="litOrgReinsurance">
                                        <asp:Literal ID="litOrgReinsurance" runat="server" Text="<%$ Resources:lbl_OrgReinsurance %>"></asp:Literal></asp:Label>

                                </div>
                                <div  class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblOrigRiOverrideResason" runat="server" AssociatedControlID="GISLookup_RIOverrideReasonOrig" Text="<%$ Resources:lblRIOverrideReason %>" class="col-md-4 col-sm-3 control-label" Visible="false"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="GISLookup_RIOverrideReasonOrig" runat="server" DataItemText="Description" DataItemValue="Key" ListCode="Ri_Override_Reason" ListType="PMLookup" Sort="Asc" CssClass="field-medium form-control form-select" Visible="false"></NexusProvider:LookupList>
                                    </div>
                                </div>
                            </div>
                        
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvOrgReinsurance" runat="server" AutoGenerateColumns="False" GridLines="None" AllowPaging="false" ShowHeader="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblOrgName %>"></asp:BoundField>
                                    <Nexus:BoundField DataField="DefaultPerc" HeaderText="<%$ Resources:lblOrgDefault %>" DataType="Percentage"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ThisPerc" HeaderText="<%$ Resources:lblOrgThis %>" DataType="Percentage"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="SumInsured" HeaderText="<%$ Resources:lblOrgSumInsured %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:TemplateField HeaderText="<%$ Resources:lblOrgPremium %>" DataType="Currency">
                                        <itemtemplate>
                                           <asp:Label ID="lblPremium" runat="server" Text='<%# Format(Math.Round(CDbl(Eval("Premium")), 2), "#,#0.00")%>' CssClass="Doub"></asp:Label>
                                        </itemtemplate>
                                    </Nexus:TemplateField>
                                    <Nexus:BoundField DataField="Tax" HeaderText="<%$ Resources:lblOrgTax %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="CommissionPerc" HeaderText="<%$ Resources:lblOrgCommissionPercentage %>" DataType="Percentage"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="Commission" HeaderText="<%$ Resources:lblOrgCommission %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="CommissionTax" HeaderText="<%$ Resources:lblOrgCommTax %>" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="Agreement" HeaderText="<%$ Resources:lblOrgAgreement %>"></Nexus:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlReinsurance" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <Nexus:ProgressIndicator ID="upReinsurance" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdatePanelReinsurance" runat="server">
            </Nexus:ProgressIndicator>
        </div>
        <div class="card-footer">
            <asp:UpdatePanel ID="updSubmitArea" runat="server">
                <ContentTemplate>
                    <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" Text="<%$ Resources:btnCancel%>" CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="btnAddFacProp" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnAddFacProp%>"></asp:LinkButton>
                    <asp:LinkButton ID="btnAddTreaty" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnAddTreaty%>"></asp:LinkButton>
                    <asp:LinkButton ID="btnOk" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btnOk%>" CausesValidation="false"  OnClientClick="clearTabAndSubmit(); return false;"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
