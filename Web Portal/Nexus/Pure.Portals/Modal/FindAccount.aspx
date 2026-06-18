<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindAccount.aspx.vb" Inherits="Nexus.Modal_FindAccount"
    MasterPageFile="~/default.master" ValidateRequest="false" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function CloseFindAccount() {
            tb_remove();
        }
    </script>

    <asp:ScriptManager ID="smFindAccount" runat="server"></asp:ScriptManager>
    <div id="Modal_FindAccount">
        <asp:Panel ID="PnlFindAccount" runat="server" DefaultButton="btnFindNow" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblDetails" Text="<%$ Resources:lbl_Heading_Details %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblShortName" runat="server" AssociatedControlID="txtShortName" Text="<%$ Resources:lblShortName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtShortName" TabIndex="1" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" TabIndex="2" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccount" runat="server" AssociatedControlID="ddlAccount" Text="<%$ Resources:lblAccount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlAccount" runat="server" DefaultText="(All types)" ListCode="accounttype" ListType="PMLookup" Sort="ASC" DataItemText="Description" DataItemValue="Code" TabIndex="3" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblShowAccountBalance" AssociatedControlID="chkIsShowAccountBalance" runat="server" Text="<%$ Resources:lblShowAccountBalance %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIsShowAccountBalance" runat="server" TabIndex="4" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblShowDeletedAccounts" AssociatedControlID="chkShowDeletedAccounts" runat="server" Text="<%$ Resources:lblShowDeletedAccounts %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkShowDeletedAccounts" runat="server" TabIndex="5" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div id="liLedger" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLedger" runat="server" AssociatedControlID="ddlLedger" Text="<%$ Resources:lblLedger %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlLedger" runat="server" TabIndex="6" CssClass="field-medium form-control form-select">
                                <asp:ListItem Value="" Selected="True">(All types)</asp:ListItem>
                                <asp:ListItem Value="AG">Agent</asp:ListItem>
                                <asp:ListItem Value="SA">Client</asp:ListItem>
                                <asp:ListItem Value="CO">Commission</asp:ListItem>
                                <asp:ListItem Value="DI">Discount</asp:ListItem>
                                <asp:ListItem Value="FE">Fees</asp:ListItem>
                                <asp:ListItem Value="IN">Insurer</asp:ListItem>
                                <asp:ListItem Value="NO">Nominal</asp:ListItem>
                                <asp:ListItem Value="OP">Other Party Payable</asp:ListItem>
                                <asp:ListItem Value="OR">Other Party R''able</asp:ListItem>
                                <asp:ListItem Value="RF">Premium Finance</asp:ListItem>
                                <asp:ListItem Value="PU">Purchase</asp:ListItem>
                                <asp:ListItem Value="UB">Sub Agent</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblReference" Text="<%$ Resources:lblReference %>"></asp:Label>
                    </legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblinsuranceRef" runat="server" AssociatedControlID="txtinsuranceRef" Text="<%$ Resources:lblinsuranceRef %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtinsuranceRef" TabIndex="7" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:lblOperator %>" AssociatedControlID="ddlOperator" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlOperator" TabIndex="8" runat="server" CssClass="form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPurchaseOrderNo" runat="server" AssociatedControlID="txtPurchaseOrderNo" Text="<%$ Resources:lblPurchaseOrderNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPurchaseOrderNo" TabIndex="9" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPurchaseInvoiceNo" runat="server" AssociatedControlID="txtPurchaseInvoiceNo" Text="<%$ Resources:lblPurchaseInvoiceNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPurchaseInvoiceNo" TabIndex="10" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" TabIndex="12" Text="<%$ Resources:btnNewSearch %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnFindNow" runat="server" TabIndex="11" Text="<%$ Resources:btnFindNow %>" SkinID="btnPrimary"></asp:LinkButton>
                
                <asp:HiddenField ID="hType" runat="server"></asp:HiddenField>
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtShortName,txtName,txtinsuranceRef,txtPurchaseOrderNo,txtPurchaseInvoiceNo" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

        <asp:UpdatePanel ID="updFindAccounts" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvFindAccount" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="ShortCode" HeaderText="<%$ Resources:lblShortName_g %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="AccountName" HeaderText="<%$ Resources:lblName_g %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="PersonalClientForename" HeaderText="<%$ Resources:lblFirstName_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Addressline1" HeaderText="<%$ Resources:lblAddressLine1_g %>"></asp:BoundField>
                            <asp:BoundField DataField="AccountStatus" HeaderText="<%$ Resources:lblStatus_g %>"></asp:BoundField>
                            <asp:BoundField DataField="LedgerCode" HeaderText="<%$ Resources:lblLedgerCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="AccountTypeCode" HeaderText="<%$ Resources:lblAccountTypeCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="AccountBalance" HeaderText="<%$ Resources:lblAccountBalance_g %>"></asp:BoundField>
                            <asp:BoundField DataField="SourceCode" HeaderText="<%$ Resources:lblSourceCode_g %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("ShortCode") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnSelect" SkinID="btnGrid" runat="server" OnClientClick=<%# "self.parent.setAccount('" + Nexus.Utils.PureEncode(Eval("ShortCode")).ToString() + "','" + Eval("AccountKey").ToString() + "','" + Nexus.Utils.PureEncode(Eval("AccountName")).ToString() + "','" + Eval("PartyKey").ToString() + "','" + Eval("CurrencyCode").ToString() + "','" + hType.Value.ToString() + "','" + Eval("LedgerCode").ToString() + "','" + Nexus.Utils.PureEncode(Eval("ContactName")).ToString() + "','" + Nexus.Utils.PureEncode(Eval("AccountName")).ToString() + "');"%>><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="grdvFindAccount" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvFindAccount" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvFindAccount" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upFndAccounts" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindAccounts" runat="server">
            <progresstemplate>
                            </progresstemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
