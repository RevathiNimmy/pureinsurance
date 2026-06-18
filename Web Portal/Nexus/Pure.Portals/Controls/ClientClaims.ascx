<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClientClaims.ascx.vb"
    Inherits="Nexus.Controls_ClientClaims" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'lnkDetails') {
            $get(uprogQuotes).style.display = "block";
        }
    }
</script>

<div id="Controls_ClientClaims">
    <asp:Panel ID="pnlClientClaims" runat="server" Visible="true">
        <legend><span id="pnlClaimsTitle" runat="server" title="<%$ Resources:titleExpandCollapse %>">
            <asp:Literal ID="litClaimsHeader" runat="server" Text="<%$ Resources:lbl_Claims_header %>"></asp:Literal></span></legend>
        <asp:Panel ID="pnlClaims" runat="server">
            <asp:UpdatePanel ID="updPanelClientClaims" runat="server">
                <ContentTemplate>
                    <div class="showall">
                        <asp:Panel ID="PanelViewAllClaims" runat="server">
                            <asp:CheckBox ID="chkViewAllClaims" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                            <asp:Label ID="lbl_ViewAllClaims" runat="server" Text="<%$ Resources:lbl_ViewAllClaims %>"></asp:Label>
                        </asp:Panel>
                    </div>
                    <div class="grid-card table-responsive no-margin">
                        <asp:GridView ID="grdvClientClaims" runat="server" AutoGenerateColumns="false" GridLines="None" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" DataKeyNames="ClaimDescription">
                            <Columns>
                                <asp:BoundField DataField="CaseNumber" SortExpression="CaseNumber" HeaderText="<%$ Resources:lbl_CaseNumber%>"></asp:BoundField>
                                <asp:BoundField DataField="ClaimNumber" SortExpression="ClaimNumber" HeaderText="<%$ Resources:lbl_ClaimNumber%>"></asp:BoundField>
                                <asp:BoundField DataField="InsuranceRef" SortExpression="InsuranceRef" HeaderText="<%$ Resources:lbl_PolicyRef %>"></asp:BoundField>
                                <asp:BoundField DataField="ClientName" SortExpression="ClientName" HeaderText="<%$ Resources:lbl_Client %>"></asp:BoundField>
                                <asp:BoundField DataField="ProductDescription" SortExpression="ProductDescription" HeaderText="<%$ Resources:lbl_ProductCode %>"></asp:BoundField>
                                <asp:BoundField DataField="ClaimRiskField" HeaderText="<%$ Resources:lbl_ClaimRiskField %>" SortExpression="ClaimRiskField" /> 
                                  <asp:BoundField DataField="LossDateFrom" SortExpression="LossDateFrom" HeaderText="<%$ Resources:lbl_LossDate %>"
                                            HtmlEncode="false" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="PrimaryCauseDescription" HeaderText="<%$ Resources:lbl_PrimaryCause %>" SortExpression="PrimaryCauseDescription" />
                                        <asp:BoundField DataField="SecondaryCauseDescription" HeaderText="<%$ Resources:lbl_SecondaryCause %>" SortExpression="SecondaryCauseDescription" />
                                                                        
                                          
                                        <asp:BoundField DataField="ProgressStatusDescription" SortExpression="ProgressStatusDescription"
                                            HeaderText="<%$ Resources:lbl_Status %>" />
                                <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>                                             
                                                       <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("ClaimDescription") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton Text="<%$ Resources:lbl_ClaimSelect %>" ID="lnkDetails" SkinID="btnGrid" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ClaimNumber")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <Nexus:ProgressIndicator ID="upClientClaims" OverlayCssClass="updating" AssociatedUpdatePanelID="updPanelClientClaims" runat="server">
                <progresstemplate>
                        </progresstemplate>
            </Nexus:ProgressIndicator>
        </asp:Panel>
    </asp:Panel>
</div>
