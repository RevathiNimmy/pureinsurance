<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BranchPickList.ascx.vb"
    Inherits="Nexus.Controls_BranchPickList" %>
<%@ Register Assembly="SCS.PickList" Namespace="SCS.Web.UI.WebControls" TagPrefix="cc1" %>
<div id="Controls_BranchPickList">
    <div id="branchpicklist">
        <legend>
            <asp:Literal ID="ltHeading" runat="server" Text="<%$ Resources:ltHeading %>"></asp:Literal></legend>
        <asp:Panel runat="server" DefaultButton="btnFindBranches" CssClass="picklist" ID="pnlPickList">
            <ol id="olFindBranch" visible="false" runat="server">
                <li>
                    <asp:Label runat="server" ID="lblBranchCode" Text="<%$ Resources:lblBranchCode %>" AssociatedControlID="txtBranchCode"></asp:Label>
                    <asp:TextBox ID="txtBranchCode" runat="server"></asp:TextBox>
                    <asp:Button runat="server" ID="btnFindBranches" CssClass="submit" Text="Find"></asp:Button></li>
            </ol>
            <cc1:PickList ID="PckBranch" EnableViewState="true" runat="server" AvailableLabelText="<%$ Resources:lbl_AvailableBranch %>" AddAllButtonText="<%$ Resources:btnAddAll %>" AddButtonText="<%$ Resources:btnAdd %>" RemoveAllButtonText="<%$ Resources:btnRemoveAll %>" RemoveButtonText="<%$ Resources:btnRemove %>" CurrentLabelText="<%$ Resources:lbl_CurrentBranch %>" DisplayAddAllButton="true" DisplayRemoveAllButton="true" DisplayAddButton="true" DisplayRemoveButton="true"></cc1:PickList>
        </asp:Panel>

      
    </div>
    <asp:CustomValidator ID="VldPckBranch" runat="server" ErrorMessage="<%$ Resources:Err_InvalidBranchSearch %>"></asp:CustomValidator>
</div>
