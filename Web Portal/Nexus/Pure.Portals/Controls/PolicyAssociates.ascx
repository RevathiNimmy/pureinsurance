<%@ Control Language="VB" AutoEventWireup="false"  CodeFile="PolicyAssociates.ascx.vb"
    Inherits="Nexus.Controls_PolicyAssociates" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<div id="Controls_Associates" runat="server">

    <script type="text/javascript">

        function ConfirmOnDelete() {
            var sConfirmMessage = ('<%= GetLocalResourceObject("msgConfirmDeleteAssociate") %>');
            if (confirm(sConfirmMessage) == true)
                return true;
            else
                return false;
        }

    </script>

    <asp:HiddenField ID="txtAssociateData" runat="server" />
    <asp:UpdatePanel ID="PnlAssociate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <legend>
                <asp:Label ID="lblAssociateHeading" runat="server" Text="<%$ Resources:lbl_Associate_heading %>"></asp:Label>
            </legend>
            <div class="grid-card table-responsive">
                <asp:GridView ID="grdAssociate" runat="server" AutoGenerateColumns="false" GridLines="None"
                    DataKeyNames="InsuranceFileAssociatesKey" EmptyDataRowStyle-CssClass="noData"
                    OnPageIndexChanging="grdAssociate_PageIndexChanging" PageSize="10" AllowPaging="true"
                    AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="PartyType" HeaderText="<%$ Resources:lbl_Associate_ClientType %>"
                            SortExpression="PartyType" />
                        <asp:BoundField DataField="PartyCode" HeaderText="<%$ Resources:lbl_Associate_ClientCode %>"
                            SortExpression="PartyCode" />
                        <asp:BoundField DataField="PartyName" HeaderText="<%$ Resources:lbl_Associate_ClientName %>"
                            SortExpression="PartyName" />
                        <asp:BoundField DataField="CompleteAddress" HeaderText="<%$ Resources:lbl_Associate_Address %>"
                            SortExpression="CompleteAddress" />
                        <asp:BoundField DataField="Postcode" HeaderText="<%$ Resources:lbl_Associate_PostCode %>"
                            SortExpression="PostCode" />
                        <asp:BoundField DataField="AssociationTypeKey" HeaderText="<%$ Resources:lbl_Associate_Association %>"
                            SortExpression="AssociationTypeKey" />
                        <asp:BoundField DataField="AssociationDetail" HeaderText="<%$ Resources:lbl_Associate_AssociationDetail %>"
                            SortExpression="AssociationDetail" />
                        <asp:BoundField DataField="DateAttached" HeaderText="<%$ Resources:lbl_Associate_DateAttached %>"
                            DataFormatString="{0:d}" SortExpression="DateAttached" />
                        <asp:BoundField DataField="DateRemoved" HeaderText="<%$ Resources:lbl_Associate_DateRemoved %>"
                            DataFormatString="{0:d}" SortExpression="DateRemoved" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("RowKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="hypAssociateView" runat="server" Text="<%$ Resources:lbl_Associate_View %>"
                                                        CausesValidation="False" />
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypAssociateEdit" runat="server" Text="<%$ Resources:lbl_Associate_Edit %>"
                                                        CausesValidation="False" />
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="hypAssociateDelete" runat="server" Text="<%$ Resources:lbl_Associate_Delete %>"
                                                        CausesValidation="False" CommandName="DeleteRow" />
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
            <asp:AsyncPostBackTrigger ControlID="grdAssociate" EventName="RowDeleting" />
            <asp:PostBackTrigger ControlID="btnAddAssociate" />
        </Triggers>
    </asp:UpdatePanel>
    <nexus:ProgressIndicator ID="upAssociate" OverlayCssClass="updating" AssociatedUpdatePanelID="PnlAssociate" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>

    <asp:LinkButton ID="btnAddAssociate" SkinID="btnSM" runat="server" Text="<%$ Resources:lit_AddAssociate %>"></asp:LinkButton>

</div>