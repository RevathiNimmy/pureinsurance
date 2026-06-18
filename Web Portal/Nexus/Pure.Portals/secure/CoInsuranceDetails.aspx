<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoInsuranceDetails.aspx.vb"
    Inherits="Nexus.secure_CoInsurance" MasterPageFile="~/default.master" EnableViewState="true" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">

    <script language="Javascript" type="text/javascript">
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                tb_init('a.thickbox');
            }
        }

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="secure_CoInsuranceDetails">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblFindInsurerHeading" runat="server" Text="<%$ Resources:lbl_CoinsuranceDetails %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <div class="col-md-6">
                            <asp:CheckBox ID="chkIsRecovered" runat="server" Text="<%$ Resources:lbl_IsRecovered %>" CssClass="asp-check"></asp:CheckBox>
                        </div>
                        <div class="col-md-6">
                            <asp:CheckBox ID="chkIsSurcharged" runat="server" Text="<%$ Resources:lbl_IsSurcharged %>" CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litAllocatedPer" runat="server" Text="<%$ Resources:lbl_AllocatedPer %>"></asp:Literal>
                        </label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="lblTotalShare" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                            <asp:HiddenField ID="hidTotalShare" runat="server"></asp:HiddenField>
                        </div>
                    </div>
                </div>


                <asp:UpdatePanel ID="UpdCoInsurer" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="drgCoInsurance" runat="server" AutoGenerateColumns="False" GridLines="None" OnSelectedIndexChanged="drgCoInsurance_SelectedIndexChanged" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="CoInsurer" HeaderText="<%$ Resources:lbl_CoInsurer %>"></asp:BoundField>
                                    <asp:BoundField DataField="ArrangementRef" HeaderText="<%$ Resources:lbl_ArrangementRef %>"></asp:BoundField>
                                    <Nexus:BoundField DataField="SharePerc" HeaderText="<%$ Resources:lbl_SharePerc %>" DataType="Percentage"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="CommissionPerc" HeaderText="<%$ Resources:lbl_CommissionPerc %>" DataType="Percentage"></Nexus:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("CoInsurerKey") %>" class="list-inline no-margin">
                                                    <li id="liDelete" runat="server">
                                                        <asp:LinkButton ID="hypCoInsuranceEdit" runat="server" Text="<%$ Resources:lbl_hypCoInsuranceEdit %>" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="true" ButtonType="link" DeleteText="Delete" CausesValidation="false"></asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="drgCoInsurance" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upCoinsuranceDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdCoInsurer" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
                <asp:LinkButton ID="hypCoInsurance" runat="server" CausesValidation="false" Text="<%$ Resources:lbl_hypCoInsuranceNew %>" SkinID="btnSM"></asp:LinkButton>

            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_OK %>" OnClick="btnOk_Click" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="cusValidShare" runat="server" Display="None"></asp:CustomValidator>
        <asp:CustomValidator ID="cusIsRetained" runat="server" Display="None"></asp:CustomValidator>
        <asp:ValidationSummary ID="vldSummeryBankGuarantee" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
