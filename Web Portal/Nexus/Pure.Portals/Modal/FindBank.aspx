<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindBank.aspx.vb" Inherits="Nexus.Modal_FindBank"
    MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function CloseFindAccount() {
            tb_remove();
        }
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindBank">
        <asp:Panel ID="PnlFindBank" runat="server" DefaultButton="btnFindNow" CssClass="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblDetails" Text="<%$ Resources:lbl_Heading_FindBank %>"></asp:Label>
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
                </div>
            </div>
            <div class="card-footer">
                
                <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" TabIndex="13" Text="<%$ Resources:btnCancel %>" CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="btnNewSearch" SkinID="btnSecondary" runat="server" TabIndex="14" Text="<%$ Resources:btnNewSearch %>" CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="btnFindNow" SkinID="btnPrimary" runat="server" TabIndex="12" Text="<%$ Resources:btnFindNow %>"></asp:LinkButton> 

            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtShortName,txtName" Condition="Auto" Display="none" runat="server" EnableClientScript="true"></Nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

        <asp:UpdatePanel ID="updFindBank" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvBankResult" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="BankName" HeaderText="<%$ Resources:lblShortName_g %>" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="BranchCode" HeaderText="<%$ Resources:lblCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="HeadOffice" HeaderText="<%$ Resources:lblBranchCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="BankAddress" HeaderText="<%$ Resources:lblHeadOffice_g %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("BankKey") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnSelect" SkinID="btnGrid"  runat="server" OnClientClick=<%# "self.parent.setBank('" + Eval("BankName").ToString() + "','" + Eval("BankKey").ToString() + "','" + Eval("BranchCode").ToString() + "');"%>><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                                <%--<a href="#" onclick="self.parent.setBank('<%# Nexus.Utils.PureEncode(DataBinder.Eval(Container.DataItem, "BankName"))%>','<%# DataBinder.Eval(Container.DataItem,"BankKey")%>','<%# Nexus.Utils.PureEncode(DataBinder.Eval(Container.DataItem, "BranchCode"))%>');">select</a>--%>
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
                <asp:AsyncPostBackTrigger ControlID="grdvBankResult" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvBankResult" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvBankResult" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upFndBank" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindBank" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </Nexus:ProgressIndicator>

    </div>
</asp:Content>
