<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Addresses.ascx.vb" Inherits="Nexus.Controls_Addresses" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<script language="javascript" type="text/javascript">
    function ReceiveAddressData(sAddressData, sPostBackTo) {
        document.getElementById('<%=txtAddressData.ClientID %>').value = sAddressData;
        __doPostBack(sPostBackTo, 'UpdateAddress');
    }

    //addEvent(window,'load',function(){setTimeout(resizeIframe,500);});
    //setTimeout(pageLoad(),500);
    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypAddress' || postBackElement.id == "hypAddressEdit") {
            $get(upAddress).style.display = "block";
        }
    }

</script>

<div id="Controls_Addresses">
    <asp:HiddenField ID="txtAddressData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlAddresses" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblAddressHeading" runat="server" Text="<%$ Resources:lbl_Address_heading %>"></asp:Label></legend>

            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgAddresses" runat="server" AutoGenerateColumns="False" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="AddressType" HeaderText="<%$ Resources:lbl_Type %>"></asp:BoundField>
                        <asp:BoundField DataField="Address1" HeaderText="<%$ Resources:lbl_Address1 %>"></asp:BoundField>
                        <asp:BoundField DataField="Address2" HeaderText="<%$ Resources:lbl_Address2 %>"></asp:BoundField>
                        <asp:BoundField DataField="Address3" HeaderText="<%$ Resources:lbl_Address3 %>"></asp:BoundField>
                        <asp:BoundField DataField="Address4" HeaderText="<%$ Resources:lbl_Address4 %>"></asp:BoundField>
                        <asp:BoundField DataField="PostCode" HeaderText="<%$ Resources:lbl_PinCode %>"></asp:BoundField>
                        <asp:BoundField DataField="CountryDescription" HeaderText="<%$ Resources:lbl_Country %>"></asp:BoundField>
                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Key %>" Visible="false"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypAddressEdit" runat="server" Text="<%$ Resources:lbl_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypAddressDelete" runat="server" Text="<%$ Resources:lbl_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
    </asp:UpdatePanel>
     <div class ="m-v-sm">
    <asp:LinkButton ID="hypAddress" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_AddAddress %>"></asp:LinkButton>
    </div>
     <nexus:ProgressIndicator ID="upAddresses" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlAddresses" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>
</div>
