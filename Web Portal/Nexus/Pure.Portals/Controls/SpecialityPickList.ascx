<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SpecialityPickList.ascx.vb"
    Inherits="Nexus.Controls_SpecialityPickList" %>
<%@ Register Assembly="SCS.PickList" Namespace="SCS.Web.UI.WebControls" TagPrefix="cc1" %>
<div id="Controls_SpecialityPickList">
    <div id="specialitypicklist">
        <legend>
            <asp:Literal ID="ltHeading" runat="server" Text="<%$ Resources:ltHeading %>"></asp:Literal>
        </legend>
        <asp:Panel runat="server" DefaultButton="btnFindSpecialities" CssClass="picklist" ID="pnlPickList">
            <ol id="olFindSpeciality" visible="false" runat="server">
                <li>
                    <asp:Label runat="server" ID="lblSpecialityCode" Text="<%$ Resources:lblSpecialityCode %>" AssociatedControlID="txtSpecialityCode"></asp:Label>
                    <asp:TextBox ID="txtSpecialityCode" runat="server"></asp:TextBox>
                    <asp:Button runat="server" ID="btnFindSpecialities" CssClass="submit" Text="Find"></asp:Button></li>
            </ol>
            <cc1:PickList ID="PckSpeciality" EnableViewState="true" runat="server" AvailableLabelText="<%$ Resources:lbl_AvailableSpeciality %>" 
                AddAllButtonText="<%$ Resources:btnAddAll %>" AddButtonText="<%$ Resources:btnAdd %>" RemoveAllButtonText="<%$ Resources:btnRemoveAll %>" 
                RemoveButtonText="<%$ Resources:btnRemove %>" CurrentLabelText="<%$ Resources:lbl_CurrentSpeciality %>" DisplayAddAllButton="true" 
                DisplayRemoveAllButton="true" DisplayAddButton="true" DisplayRemoveButton="true"></cc1:PickList>
        </asp:Panel>
      
    </div>
    <asp:CustomValidator ID="VldPckSpeciality" runat="server" ErrorMessage="<%$ Resources:Err_InvalidSpecialitySearch %>"></asp:CustomValidator>
</div>
