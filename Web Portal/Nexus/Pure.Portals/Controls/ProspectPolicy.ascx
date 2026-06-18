<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProspectPolicy.ascx.vb"
    Inherits="Nexus.Controls_ProspectPolicy" %>

<script language="javascript" type="text/javascript">
    function ReceiveProspectPolicyData(sProspectPolicyData, sPostBackTo) {
        document.getElementById('<%=txtProspectPolicyData.ClientID %>').value = sProspectPolicyData;
        __doPostBack(sPostBackTo, 'UpdatesProspectPolicy');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypProspectPolicy' || postBackElement.id == 'hypProspectPolicyEdit') {
            $get(uprogQuotes).style.display = "block";
        }
    }

</script>

<div id="Controls_ProspectPolicy">
    <asp:HiddenField ID="txtProspectPolicyData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlProspectPolicy" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblProspectPolicyHeading" runat="server" Text="<%$ Resources:lbl_ProspectPolicy_heading %>"></asp:Label>
            </legend>

            <asp:Label runat="server" ID="Label9"></asp:Label>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgProspectPolicy" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="ProspectTypeCode" HeaderText="<%$ Resources:lbl_ProspectPolicy_Type %>"></asp:BoundField>
                        <asp:BoundField DataField="RenewalDate" HeaderText="<%$ Resources:lbl_ProspectPolicy_RenewalDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="TimesQuoted" HeaderText="<%$ Resources:lbl_ProspectPolicy_TimesQuoted %>"></asp:BoundField>
                        <asp:BoundField DataField="TargetPremium" HeaderText="<%$ Resources:lbl_ProspectPolicy_TargetPremium %>"></asp:BoundField>
                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_ProspectPolicy_Key %>" Visible="false"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypProspectPolicyEdit" runat="server" Text="<%$ Resources:lbl_ProspectPolicy_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypProspectPolicyDelete" runat="server" Text="<%$ Resources:lbl_ProspectPolicy_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
             <div class ="m-v-sm">
            <asp:LinkButton ID="hypProspectPolicy" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_ProspectPolicy %>"></asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upProspectPolicy" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlProspectPolicy" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
