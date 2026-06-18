<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClientEvents.ascx.vb"
    Inherits="Nexus.Controls_ClientEvents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypEventDetails') {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_ClientEvents">
    <asp:Panel ID="pnlClientEvents" runat="server" Visible="true">
        <div class="fieldset-wrapper">
            
            <fieldset>
                <legend><span id="pnlEventsTitle" runat="server" title="<%$ Resources:titleExpandCollapse %>">
                    <asp:Literal ID="litEventsHeader" runat="server" Text="<%$ Resources:lbl_Events_header %>"></asp:Literal></span></legend>
                <asp:Panel ID="pnlEvents" runat="server">
                    <div class="grid-card table-responsive">
                        <asp:UpdatePanel ID="updPanelClientEvents" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdvEvents" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                    <Columns>
                                        <asp:BoundField DataField="EventDate" HeaderText="<%$ Resources:lbl_EventDate %>" HtmlEncode="False" SortExpression="EventDate"></asp:BoundField>
                                        <asp:BoundField DataField="EventType" HeaderText="<%$ Resources:lbl_EventType %>" SortExpression="EventType"></asp:BoundField>
                                        <asp:BoundField DataField="PolicyCode" HeaderText="<%$ Resources:lbl_PolicyCode %>" SortExpression="PolicyCode"></asp:BoundField>
                                        <asp:BoundField DataField="CaseNumber" HeaderText="<%$ Resources:lbl_CaseNumber %>" SortExpression="CaseNumber"></asp:BoundField>
                                        <asp:BoundField DataField="ClaimNumber" HeaderText="<%$ Resources:lbl_ClaimNumber %>" SortExpression="ClaimNumber"></asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:lbl_Description %>" SortExpression="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEventDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lbl_UserName %>" SortExpression="UserName"></asp:BoundField>
                                        <asp:BoundField DataField="Priority" HeaderText="<%$ Resources:lbl_Priority %>" SortExpression="Priority"></asp:BoundField>
                                        <asp:TemplateField HeaderText="<%$ Resources:lbl_Status %>" SortExpression="EventDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Visible="true" Enabled="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <div class="rowMenu">
                                                    <ol id="menu_<%# Eval("EventKey") %>" Class="list-inline no-margin">
                                                        <li>
                                                            <asp:HyperLink ID="hypEventDetails" runat="server" Text="<%$ Resources:lbl_EventDetails %>" CssClass="thickbox" CausesValidation="False" SkinID="btnGrid"></asp:HyperLink>
                                                        </li>
                                                    </ol>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <nexus:ProgressIndicator ID="upClientEvents" OverlayCssClass="updating" AssociatedUpdatePanelID="updPanelClientEvents" runat="server">
                            <ProgressTemplate>
                            </ProgressTemplate>
                        </nexus:ProgressIndicator>
                    </div>
                </asp:Panel>
            </fieldset>
        </div>
        
    </asp:Panel>
</div>
