<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Contacts.ascx.vb" Inherits="Nexus.Controls_Contacts" %>


<script language="javascript" type="text/javascript">
    function ReceiveContactData(sContactData, sPostBackTo) {
        document.getElementById('<%=txtContactData.ClientID %>').value = sContactData;
        __doPostBack(sPostBackTo, 'UpdateContact');
    }

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypContact' || postBackElement.id == "hypContactEdit") {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_Contacts">
    <asp:HiddenField ID="txtContactData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlContact" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
        <ContentTemplate>
                    <legend>
                <asp:Label ID="lblContactHeading" runat="server" Text="<%$ Resources:lbl_Contact_heading %>"></asp:Label>
                    </legend>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgContact" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                        <asp:BoundField DataField="ContactTypeDescription" HeaderText="<%$ Resources:lbl_Contact_Type %>"></asp:BoundField>
                        <asp:BoundField DataField="AreaCode" HeaderText="<%$ Resources:lbl_Contact_AreaCode %>"></asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Contact_Description %>"></asp:BoundField>
                        <asp:BoundField DataField="Number" HeaderText="<%$ Resources:lbl_Contact_Number %>"></asp:BoundField>
                        <asp:BoundField DataField="Extension" HeaderText="<%$ Resources:lbl_Contact_Extension %>"></asp:BoundField>
                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Contact_Key %>" Visible="false"></asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                    <li>
                                                    <asp:LinkButton ID="hypContactEdit" runat="server" Text="<%$ Resources:lbl_Contact_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                    <asp:LinkButton ID="hypContactDelete" runat="server" Text="<%$ Resources:lbl_Contact_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
    <div class="m-v-sm">
        <asp:LinkButton ID="hypContact" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_AddContacts %>"></asp:LinkButton>
    </div>
    <asp:Panel ID="pnlAdditionalInfo" runat="server">
    <div class="form-inline">
        <div class="form-group form-group-sm">
            <asp:Label ID="lblPreferredCorrespondence" runat="server" Text="<%$ Resources:lbl_PreferredCorrespondence %>"></asp:Label>
            <asp:DropDownList ID="ddlPreferredCorrespondenceType" runat="server" CssClass="form-control form-select"></asp:DropDownList>
                </div>
            </div>
    
    </asp:Panel>
    <Nexus:ProgressIndicator ID="upContact" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlContact" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
