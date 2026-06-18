<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Associates.ascx.vb" Inherits="Nexus.Controls_Associates" %>

<script language="javascript" type="text/javascript">
    function ReceiveAssociateData(sAssociateData, sPostBackTo) {
        document.getElementById('<%=txtAssociateData.ClientID %>').value = sAssociateData;
        __doPostBack(sPostBackTo, 'UpdateAssociate');
    }
</script>

<div id="Controls_Associates">
    <asp:HiddenField ID="txtAssociateData" runat="server"></asp:HiddenField>
    <asp:UpdatePanel ID="PnlAssociate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <legend>
                <asp:Label ID="lblAssociateHeading" runat="server" Text="<%$ Resources:lbl_Associate_heading %>"></asp:Label>
            </legend>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="drgAssociate" runat="server" AutoGenerateColumns="false" GridLines="None" DataKeyNames="Key" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="AssociateCode" HeaderText="<%$ Resources:lbl_Associate_Associate %>"></asp:BoundField>
                        <asp:BoundField DataField="AssociateName" HeaderText="<%$ Resources:lbl_Associate_Name %>"></asp:BoundField>
                        <asp:BoundField DataField="RelationshipDescription" HeaderText="<%$ Resources:lbl_Associate_Relationship %>"></asp:BoundField>
                        <Nexus:BoundField DataField="AccountBalance" HeaderText="<%$ Resources:lbl_Account_Balance %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="ClaimIncurred" HeaderText="<%$ Resources:lbl_Claim_Incurred %>" DataType="Currency"></Nexus:BoundField>
                        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:lbl_Associate_Key %>" Visible="false"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("Key") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypAssociateEdit" runat="server" Text="<%$ Resources:lbl_Associate_Edit %>" CausesValidation="False"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypAssociateDelete" runat="server" Text="<%$ Resources:lbl_Associate_Delete %>" CausesValidation="False" CommandName="DeleteRow"></asp:LinkButton>
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
                 <asp:LinkButton ID="hypAssociate" runat="server" SkinID="btnSM" Text="<%$ Resources:lit_AddAssociate %>"></asp:LinkButton>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drgAssociate" EventName="RowDeleting"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="upAssociate" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlAssociate" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
