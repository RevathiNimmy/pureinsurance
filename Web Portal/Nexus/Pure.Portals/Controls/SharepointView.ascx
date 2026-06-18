<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SharepointView.ascx.vb"
    Inherits="Nexus.Controls_SharepointView" %>
<%@ Register TagPrefix="uc1" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>

<script type="text/javascript">
    function emailSpDocs() {
        //launch email dialog, passing the selected document ids in the query string
        var docIDs = "";
        $('span[class^="spID"]').each(function () {
            if ($(this).find('input:first').is(':checked')) {
                docIDs += $(this).attr('class');
            }
        });
        var key = document.getElementById('<%= hdnKey.ClientId%>').value;
        show_modal('<%= ResolveUrl("~/modal/SendEmail.aspx") %>?Docs=' + docIDs + '&modal=true&loc=sharep&key=' + key, null);
    }

    function viewInSharepoint() {
        myWindow = window.open(document.getElementById('<%= hdnSpLoc.ClientID %>').value, "sharepoint");
        myWindow.focus()

    }

</script>

<div class="Controls_DocumentManager">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="card-body clearfix no-padding">
                    <div id="folder-navigation" class="p-v-sm">
                        <asp:Repeater ID="rptrFolderNavigation" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="hypPath" runat="server" CommandArgument='<%# Eval("FullPath") %>' SkinID="btnSM" Text='<%# eval("Name") %>' CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                            <SeparatorTemplate>
                            </SeparatorTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvSharePoint" runat="server" AllowPaging="True" PageSize="10" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" Caption="SharePointView" EmptyDataRowStyle-CssClass="noData" DataKeyNames="" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMarkedOutTran" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server" ID="lblDocName" Text="<%$ Resources:lblDocName %>"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <asp:LinkButton ID="hypFile" Text='<%# Eval("Filename") %>' CausesValidation="false" CommandArgument='<%# Eval("URL") %>' SkinID="lnkGrid" runat="server"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkFolder" runat="server" CommandName="Folder" CausesValidation="false" CommandArgument='<%# Eval("Filename") %>' SkinID="lnkGrid" Text='<%# Eval("Filename") %>'></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DocumentTemplateGroup" HeaderText="<%$ Resources:Category %>"></asp:BoundField>
                            <asp:BoundField DataField="DocumentTemplateSubGroup" HeaderText="<%$ Resources:SubCategory %>"></asp:BoundField>
                            <asp:BoundField DataField="LastModifiedDate" HeaderText="<%$ Resources:Modified %>" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField DataField="ItemType" HeaderText="<%$ Resources:Type %>"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="card-footer no-padding-h">
                <asp:LinkButton runat="server" ID="btnViewInSharepoint" Text="<%$ Resources:btnViewInSharepoint %>" OnClientClick="viewInSharepoint();return false;" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnEmailSelected" Visible="false" runat="server" Text="<%$ Resources:btnEmailSelected %>" OnClientClick="emailSpDocs();return false;" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton Text="<i class='fa fa-refresh' aria-hidden='true'></i> Refresh" runat="server" ID="btnRefresh" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>
            <asp:HiddenField runat="server" ID="hdnKey"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnSpLoc"></asp:HiddenField>


        </ContentTemplate>
    </asp:UpdatePanel>
</div>
