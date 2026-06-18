<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SearchResults.ascx.vb"
    Inherits="Nexus.controls_SearchResults" EnableViewState="true" %>
<div id="Controls_SearchResults">
    <br>
    <br>
    <asp:MultiView ID="mvPages" runat="server" EnableTheming="True">
        <asp:View runat="server" ID="vwPagesLink">
            <div class="panel">
                <div>
                    <strong>
                        <br>
                        Pages<br>
                    </strong>
                    <br>
                    <asp:LinkButton ID="cmdShowPages" runat="server"></asp:LinkButton></div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwPagesResults">
            <div class="panel">
                <br>
                <strong>Pages</strong><br>
                <br>
                <asp:UpdatePanel ID="UpdResultsPage" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvResultsPages" runat="server" AutoGenerateColumns="False" CellSpacing="5" GridLines="None" AllowPaging="True" PageSize="5" ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Pages">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkTitle" runat="server" Text='<%# CStr(Eval("Label")) %>' NavigateUrl='<%# NavURL(CStr(Eval("xml_content")), CStr(Eval("page_type_id")), CStr(Eval("sitemap_id")), CStr(Eval("parent_id"))) %>'></asp:HyperLink><br>
                                            <asp:Label ID="lblSummary" runat="server" Text='<%# SummaryText(CStr(Eval("xml_content")), CStr(Eval("page_type_id"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NextPrevious" NextPageText="More &amp;gt;" PreviousPageText="&amp;lt; Back"></PagerSettings>
                                <PagerStyle ForeColor="Black"></PagerStyle>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvResultsPages" EventName="Load"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="gvResultsPages" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upResults" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdResultsPage" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </nexus:ProgressIndicator>
            </div>
        </asp:View>
    </asp:MultiView>
    <asp:MultiView ID="mvNews" runat="server">
        <asp:View runat="server" ID="vwNewsLink">
            <div class="panel">
                <div>
                    <strong>
                        <br>
                        News</strong><br>
                    <br>
                    <asp:LinkButton ID="cmdShowNews" runat="server"></asp:LinkButton></div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwNewsResults">
            <div class="panel">
                <br>
                <strong>News<br>
                </strong>
                <br>
                <asp:UpdatePanel ID="UpdResultsNews" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvResultsNews" runat="server" AutoGenerateColumns="False" CellSpacing="5" GridLines="None" AllowPaging="True" PageSize="5" ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="News">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkTitle" runat="server" Text='<%# CStr(Eval("Label")) %>' NavigateUrl='<%# NavURL(CStr(Eval("xml_content")), CStr(Eval("page_type_id")), CStr(Eval("sitemap_id")), CStr(Eval("parent_id"))) %>'></asp:HyperLink><br>
                                            <asp:Label ID="lblSummary" runat="server" Text='<%# SummaryText(CStr(Eval("xml_content")), CStr(Eval("page_type_id"))) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NextPrevious" NextPageText="More &amp;gt;" PreviousPageText="&amp;lt; Back"></PagerSettings>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvResultsNews" EventName="Load"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="gvResultsNews" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upResultNews" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdResultsNews" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </nexus:ProgressIndicator>
            </div>
        </asp:View>
    </asp:MultiView>
    <asp:MultiView ID="mvMedia" runat="server">
        <asp:View runat="server" ID="vwMediaLink">
            <div class="panel">
                <div>
                    <strong>
                        <br>
                        Media/Documents</strong><br>
                    <br>
                    <asp:LinkButton ID="cmdShowMedia" runat="server"></asp:LinkButton></div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwMediaResults">
            <div class="panel">
                <br>
                <strong>Media/Documents</strong><br>
                <br>
                <asp:UpdatePanel ID="UpdMedia" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvMedia" runat="server" AutoGenerateColumns="False" CellSpacing="5" GridLines="None" AllowPaging="True" PageSize="5" ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Media/Documents">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkLink" runat="server" NavigateUrl='<%# MediaPath(CStr(Eval("path")), CStr(Eval("category_id"))) %>'>
                                                <asp:Image ID="imgMedia" runat="server" BorderWidth="0" ImageUrl='<%# MediaPreview(CStr(Eval("path")), CStr(Eval("category_id"))) %>'></asp:Image><br>
                                                <asp:Label ID="lblMedia" runat="server" Text='<%# MediaAlt(CStr(Eval("alt"))) %>'></asp:Label></asp:HyperLink>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NextPrevious" NextPageText="More &amp;gt;" PreviousPageText="&amp;lt; Back"></PagerSettings>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvMedia" EventName="Load"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="gvMedia" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upMedia" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdMedia" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </nexus:ProgressIndicator>
            </div>
        </asp:View>
    </asp:MultiView>
</div>
