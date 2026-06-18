<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SupplyPickList.ascx.vb"
    Inherits="Nexus.Controls_SupplyPickList" %>
<%@ Register Assembly="SCS.PickList" Namespace="SCS.Web.UI.WebControls" TagPrefix="cc1" %>
<div id="Controls_SupplyPickList">
    <div id="supplypicklist">
        <legend>
            <asp:Literal ID="ltHeading" runat="server" Text="<%$ Resources:ltHeading %>"></asp:Literal>

        </legend>
        <asp:Panel runat="server" DefaultButton="btnFindSupplies" CssClass="picklist" ID="pnlPickList">
            <ol id="olFindSupply" visible="false" runat="server">
                <li>
                    <asp:Label runat="server" ID="lblSupplyCode" Text="<%$ Resources:lblSupplyCode %>" AssociatedControlID="txtSupplyCode"></asp:Label>
                    <asp:TextBox ID="txtSupplyCode" runat="server"></asp:TextBox>
                    <asp:Button runat="server" ID="btnFindSupplies" CssClass="submit" Text="Find"></asp:Button></li>

            </ol>
            <cc1:PickList ID="PckSupply" EnableViewState="true" runat="server" AvailableLabelText="<%$ Resources:lbl_AvailableSupply %>" AddAllButtonText="<%$ Resources:btnAddAll %>" AddButtonText="<%$ Resources:btnAdd %>" RemoveAllButtonText="<%$ Resources:btnRemoveAll %>" RemoveButtonText="<%$ Resources:btnRemove %>" CurrentLabelText="<%$ Resources:lbl_CurrentSupply %>" DisplayAddAllButton="true" DisplayRemoveAllButton="true" DisplayAddButton="true" DisplayRemoveButton="true"></cc1:PickList>
        </asp:Panel>
      
    </div>
    <asp:CustomValidator ID="VldPckSupply" runat="server" ErrorMessage="<%$ Resources:Err_InvalidSupplySearch %>"></asp:CustomValidator>
</div>
