<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Loyalty.ascx.vb" Inherits="Nexus.Controls_Loyalty" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function ReceiveLoyaltyData(sLoyaltyData, sPostBackTo) {
        document.getElementById('<%=txtLoyaltyData.ClientID %>').value = sLoyaltyData;
        __doPostBack(sPostBackTo, 'UpdatesLoyalty');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypLoyalty' || postBackElement.id == "hypLoyaltyEdit") {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_Loyalty">
    <asp:HiddenField ID="txtLoyaltyData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlLoyalty" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
                    <legend>
                        <asp:Label ID="lblLoyaltyHeader" runat="server" Text="<%$ Resources:lbl_Loyalty_heading %>"></asp:Label></legend>
                    <asp:Panel ID="pnlLoyaltyPanel" runat="server">
                     
                            <asp:Label runat="server" ID="Label8"></asp:Label>
                            <div class="grid-card table-responsive no-margin">
                                <asp:GridView ID="drgLoyalty" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                    <Columns>
                                        <asp:BoundField DataField="LoyaltySchemeCode" HeaderText="<%$ Resources:lbl_Loyalty_Scheme %>"></asp:BoundField>
                                        <asp:BoundField DataField="MembershipNumber" HeaderText="<%$ Resources:lbl_Loyalty_MembershipNumber %>"></asp:BoundField>
                                        <asp:BoundField DataField="OtherReference" HeaderText="<%$ Resources:lbl_Loyalty_OtherReference %>"></asp:BoundField>
                                        <asp:BoundField DataField="StartDate" HeaderText="<%$ Resources:lbl_Loyalty_StartDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:BoundField DataField="EndDate" HeaderText="<%$ Resources:lbl_Loyalty_EndDate %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:BoundField DataField="MainMember" HeaderText="<%$ Resources:lbl_Loyalty_MainMember %>"></asp:BoundField>
                                        <asp:BoundField DataField="Active" HeaderText="<%$ Resources:lbl_Loyalty_Active %>"></asp:BoundField>
                                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Loyalty_Key %>" Visible="false"></asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <div class="rowMenu"><ol class="list-inline no-margin"><li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                    <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                        <li>
                                                            <asp:linkbutton id="hypLoyaltyEdit" runat="server" text="<%$ Resources:lbl_Loyalty_Edit %>" causesvalidation="False"></asp:linkbutton>
                                                        </li>
                                                        <li>
                                                            <asp:linkbutton id="hypLoyaltyDelete" runat="server" text="<%$ Resources:lbl_Loyalty_Delete %>" causesvalidation="False" commandname="DeleteRow"></asp:linkbutton>
                                                        </li>
                                                    </ol>
                                                </li></ol></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                             <div class ="m-v-sm">
                            <asp:LinkButton ID="hypLoyalty" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_AddLoyalty %>"></asp:LinkButton>
                        </div>
                    </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <nexus:ProgressIndicator ID="upLoyalty" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlLoyalty" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>
</div>
