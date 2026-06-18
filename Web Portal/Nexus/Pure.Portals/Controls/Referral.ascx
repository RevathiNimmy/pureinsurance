<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Referral.ascx.vb" Inherits="Nexus.Controls_Referral"
    EnableViewState="false" %>

<script language="javascript" type="text/javascript">
    function SetUser(sUser, ctrlchkAuth, ctrlchkDealtWith, ctrlDealtWithBy) {
        if (ctrlchkAuth.checked == true) {
            ctrlchkDealtWith.disabled = false;
        }
        else {
            ctrlchkDealtWith.checked = false;
            ctrlchkDealtWith.disabled = true;
            ctrlDealtWithBy.value = '';
        }

        if (ctrlchkDealtWith.checked == true) {
            ctrlDealtWithBy.value = sUser;
        }
        else
        { ctrlDealtWithBy.value = ''; }
    }

</script>

<div id="Controls_Referral">
    <asp:GridView ID="grdvReferral" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="<%$Resources:lbl_grdvReferral_Reasons_heading%>">
                <ItemTemplate>
                    <asp:Label ID="ReferralReasons" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("REASON").Value%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:lbl_grdvAuthorise_heading%>">
                <ItemTemplate>
                    <asp:CheckBox ID="chkAuthorize" runat="server" AutoPostBack="True" OnCheckedChanged="chkAuth_OnCheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:lbl_grdvAuthorised_by_heading%>">
                <ItemTemplate>
                    <asp:Label ID="AuthorisedBy" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:lbl_grdvADealt_with_heading%>">
                <ItemTemplate>
                    <asp:CheckBox ID="chkDealtwith" runat="server" AutoPostBack="True" OnCheckedChanged="chkDealt_OnCheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="<%$Resources:lbl_grdvDealt_with_by_heading%>">
                <ItemTemplate>
                    <asp:Label ID="DealtWithBy" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="false" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblOI" Text='<%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value%>' Visible="false" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
