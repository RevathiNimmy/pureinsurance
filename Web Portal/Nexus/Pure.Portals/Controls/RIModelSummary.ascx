<%@ Control Language="VB" AutoEventWireup="True" CodeFile="RIModelSummary.ascx.vb"
    Inherits="Nexus.secure_Control_RIModelSummary" %>
<div id="Controls_RIModelSummary">
    <asp:UpdatePanel ID="updRIModelSummary" runat="server">
        <ContentTemplate>
            <div class="treeview-wrapper">
                <asp:TreeView runat="server" ID="treeRIModel" OnTreeNodePopulate="PopulateNode" ExpandDepth="1"></asp:TreeView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
