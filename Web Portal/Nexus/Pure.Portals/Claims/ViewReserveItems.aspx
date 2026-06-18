<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewReserveItems.aspx.vb"
    Inherits="Nexus.Framework_ViewReserveItems" MasterPageFile="~/Default.master" %>

<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
    <div id="Claims_ViewReserveItems">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPageHeading" runat="server" Text="<%$ Resources:ViewReserveItems_pageheading %>"></asp:Label>
                </h1>
            </div>
            <input id="hdnCalculate" runat="server" type="hidden" value="0">
            <div class="card-body clearfix">
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvReserveItem" runat="server" AutoGenerateColumns="false" GridLines="None" EnableViewState="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_Description_heading %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_CurrentReserve_heading %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrentReserve" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:lbl_btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
