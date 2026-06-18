<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProductPickList.ascx.vb"
    Inherits="Nexus.Controls_ProductPickList" %>
<%@ Register Assembly="SCS.PickList" Namespace="SCS.Web.UI.WebControls" TagPrefix="cc1" %>
<script>
    $(document).ready(function () {
        $('table').addClass('picklist-table');
    });
</script>
<div id="productpicklist">
    <div class="card-divider">
        <h5>
            <asp:Literal ID="ltHeading" runat="server" Text="<%$ Resources:ltHeading %>" /></h5>
    </div>
    <div class="card-body clearfix">
        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFindProducts">
            <ol id="olFindProduct" visible="false" runat="server">
                <li>
                    <asp:Label runat="server" ID="lblProductCode" Text="<%$ Resources:lblProductCode %>"
                        AssociatedControlID="txtProductCode" />
                    <asp:TextBox ID="txtProductCode" runat="server" />
                    <asp:Button runat="server" ID="btnFindProducts" CssClass="submit" Text="Find" /></li>
            </ol>
            <cc1:PickList ID="PckProduct" EnableViewState="true" runat="server" AvailableLabelText="<%$ Resources:lbl_AvailableProduct %>"
                CurrentLabelText="<%$ Resources:lbl_CurrentProduct %>" DisplayAddAllButton="true"
                AddAllButtonText="<%$ Resources:btnAddAll %>" AddButtonText="<%$ Resources:btnAdd %>"
                RemoveAllButtonText="<%$ Resources:btnRemoveAll %>" RemoveButtonText="<%$ Resources:btnRemove %>"
                DisplayRemoveAllButton="true" DisplayAddButton="true" DisplayRemoveButton="true" />
        </asp:Panel>

        <asp:CustomValidator ID="VldPckProduct" runat="server" ErrorMessage="<%$ Resources:Err_InvalidProductSearch %>"></asp:CustomValidator>

    </div>
</div>
