<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="Mid.aspx.vb" Inherits="Nexus.secure_Mid" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div class="card">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblHeader" runat="server" Text="<%$ Resources:lblHeader %>"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">
            <div class="grid-card table-responsive">
                <asp:GridView ID="gvGetMIDFile" runat="server" AllowSorting="True" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" DataKeyNames="MIDFileKey" EmptyDataText="<%$ Resources:ErrorMessage %>" PageSize="30">
                    <Columns>
                        <asp:BoundField HeaderText="<%$ Resources:lbl_MIDFileSequenceNo %>" DataField="FileSequenceNumber" SortExpression="FileSequenceNumber"></asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:lbl_DateGenerated %>" DataField="DateGenerated" SortExpression="DateGenerated" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:lbl_Status %>" DataField="StatusDescription" SortExpression="StatusDescription"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink Text="<%$ Resources:lblDetails %>" ID="lnkDetails" runat="server" Visible="false"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
