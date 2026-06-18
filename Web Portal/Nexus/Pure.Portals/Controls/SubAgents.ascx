<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubAgents.ascx.vb" Inherits="Nexus.Controls_SubAgents" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<script language="javascript" type="text/javascript">
    function setSubAgent(sName, sKey, sCode, sAgentType) {
        tb_remove();
        document.getElementById('<%= hAgentCode.ClientId%>').value = sCode;
        document.getElementById('<%= hAgentKey.ClientId%>').value = sKey;
        document.getElementById('<%= hAgentName.ClientId%>').value = unescape(sName);
        __doPostBack('<%= PnlSubAgent.ClientId%>', 'RefreshSubAgent');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypSubAgent') {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_SubAgents">
    <asp:UpdatePanel ID="PnlSubAgent" runat="server" UpdateMode="conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblAddressHeading" runat="server" Text="<%$ Resources:lbl_SubAgents_header %>"></asp:Label></legend>
            <div class="grid-card table-responsive">
                <asp:GridView ID="grdvSubAgents" runat="server" AutoGenerateColumns="False" GridLines="None" DataKeyNames="PartyKey" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="Code" HeaderText="<%$ Resources:lbl_Code %>"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lbl_Name %>"></asp:BoundField>
                        <nexus:BoundField DataField="Percentage" HeaderText="<%$ Resources:lbl_Percentage %>" DataType="Percentage"></nexus:BoundField>
                        <nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:lbl_Amount %>" DataType="Currency"></nexus:BoundField>
                        <asp:BoundField DataField="PartyKey" HeaderText="<%$ Resources:lbl_Key %>" Visible="false"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="hypSubAgentDelete" runat="server" Text="<%$ Resources:lbl_Delete %>" CausesValidation="False" SkinID="btnGrid" CommandName="DeleteRow" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div>
                <asp:LinkButton ID="hypSubAgent" runat="server" Text="<%$ Resources:hyp_AddSubAgent %>" SkinID="btnSM"></asp:LinkButton>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>
    <nexus:ProgressIndicator ID="upSubAgent" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlSubAgent" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>
    <asp:HiddenField ID="hAgentCode" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hAgentName" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hAgentKey" runat="server"></asp:HiddenField>
</div>
