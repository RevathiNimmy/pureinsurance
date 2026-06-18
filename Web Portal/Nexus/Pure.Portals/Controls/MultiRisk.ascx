<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MultiRisk.ascx.vb" Inherits="Nexus.Controls_MultiRisk" %>

<script type="text/javascript">

    function updatePremium(chk, status) {
        var premiumValue = chk.value;
        var olblPremiumCntrl = document.getElementById('ctl00_cntMainBody_litTotalPremium');
        var olblControlValue = Number(olblPremiumCntrl.innerText);

        if (status == true) {
            olblPremiumCntrl.innerText = (Number(olblControlValue) + Number(premiumValue)) + ".00";
        }
        else {
            olblPremiumCntrl.innerText = (Number(olblControlValue) - Number(premiumValue)) + ".00";
        }
    }

    function updated(ctrlClicked, clientType) {
        tb_remove();
        document.getElementById('ctl00_cntMainBody_MultiRisk1_btnRefreshRiskType').click();
    }

    function RiskIsPartOfPolicy(sErrorMessage) {
        alert(sErrorMessage);
        return false;
    }


</script>

<div id="Controls_MultiRisk">
    <div class="card card-secondary">
        <div class="card-heading">
            <h4>
                <asp:Label ID="lblMultiRisk" runat="server" Text="<%$ Resources:lbl_MultiRisk %>"></asp:Label></h4>
        </div>
        <div class="card-body clearfix">
            <asp:UpdatePanel ID="UpdMultiRisk" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="grid-card table-responsive no-margin">
                        <asp:GridView ID="grdvRisk" runat="server" AllowPaging="true" DataKeyNames="Key,RiskNumber" AutoGenerateColumns="False" GridLines="None" PagerSettings-Mode="Numeric" PageSize="5" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_SelectAll %>">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRiskSelect" runat="server" value='<%# Eval("Premium") %>' AutoPostBack="True" OnCheckedChanged="chkRiskSelect_CheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Risk Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTempRiskNumber" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <nexus:BoundField DataField="RiskNumber" HeaderText="Risk Number" DataType="Number"></nexus:BoundField>--%>
                                <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_RiskDescription %>"></asp:BoundField>
                                <Nexus:BoundField DataField="TotalSumInsured" HeaderText="<%$ Resources:lbl_SumInsured %>" DataType="Currency"></Nexus:BoundField>
                                <Nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lbl_Premium %>" DataType="Currency"></Nexus:BoundField>
                                <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Key %>"></asp:BoundField>
                                <asp:BoundField DataField="StatusDescription" HeaderText="<%$ Resources:lbl_RiskStatus %>"></asp:BoundField>
                                <asp:BoundField DataField="RiskLinkStatusFlag" HeaderText="<%$ Resources:lbl_LinkStatus %>"></asp:BoundField>
                                <asp:BoundField DataField="RiskLinkChangeDate" HeaderText="<%$ Resources:lbl_LinkDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <div class="rowMenu">
                                            <ol class="list-inline no-margin">
                                                <li class="dropdown no-padding">
                                                    <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle">
                                                        <i class="fa fa-ellipsis-v" aria-hidden="true"></i>
                                                    </a>
                                                    <ol id='menu_<%# Eval("RiskNumber") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="<%$ Resources:lbl_Edit %>">
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnNoChange" runat="server" CausesValidation="False" CommandName="NoChange" Text="<%$ Resources:lbl_NoChange %>" Visible="false">
                                                            </asp:LinkButton>
                                                        </li>

                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnCopy" runat="server" CausesValidation="False" Text="<%$ Resources:lbl_Copy %>"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="False" Text="<%$ Resources:lbl_Delete %>"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:HyperLink ID="hypDetails" runat="server" Style="cursor: pointer;" Text="<%$ Resources:lbl_Details %>" CssClass="thickbox"></asp:HyperLink>
                                                        </li>
                                                    </ol>
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
                    <asp:PostBackTrigger ControlID="grdvRisk"></asp:PostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <Nexus:ProgressIndicator ID="upMultiRisk" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdMultiRisk" runat="server">
                <progresstemplate>
                    </progresstemplate>
            </Nexus:ProgressIndicator>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnAddRisk" runat="server" CausesValidation="false" Text="<%$ Resources:lbl_AddRisk %>" SkinID="btnSM"></asp:LinkButton>
            <asp:LinkButton ID="btnQuoteAll" runat="server" CausesValidation="false" Text="<%$ Resources:lbl_QuoteAll %>" SkinID="btnSM"></asp:LinkButton>

            <asp:LinkButton ID="btnNoChangeAll" runat="server" CausesValidation="false" Text="<%$ Resources:lbl_NoChangeAll  %>" Visible="false" SkinID="btnSM"></asp:LinkButton>
        </div>
    </div>
    <asp:CustomValidator ID="vldCompQuote" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_vldCompQuote_Error %>" Display="none"></asp:CustomValidator>
</div>
