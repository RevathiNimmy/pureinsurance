<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RatingDetails.ascx.vb"
    Inherits="Nexus.Controls_RatingDetails" EnableViewState="false" %>
<div id="Controls_RatingDetails">
    <legend>
        <asp:Label ID="lblRatingDetails" runat="server" Text="<%$ Resources:lblRatingDetails %>" />
    </legend>
    <div class="card-body no-padding-h p-v-sm">
        <div class="grid-card table-responsive no-margin">
            <asp:GridView ID="grdViewRatingSection" runat="server" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" AllowPaging="false" ShowHeader="True" ShowFooter="true" EmptyDataText="<%$ Resources:lbl_NoDataFound %>">
            </asp:GridView>
        </div>
    </div>
    <div class="p-v-sm">
        <asp:LinkButton ID="btnAdd" runat="server" SkinID="btnSM" Text="<%$ Resources:btn_Add %>"></asp:LinkButton>
    </div>
</div>
