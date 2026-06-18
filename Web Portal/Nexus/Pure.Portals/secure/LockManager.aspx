<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="LockManager.aspx.vb" Inherits="Nexus.LockManager" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="secure_lockmanager">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <asp:UpdatePanel ID="updLockManager" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="card-body clearfix">
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvLockManager" runat="server" AutoGenerateColumns="False" AllowPaging="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:TemplateField SortExpression="chkSelect">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$ Resources:lblName %>" DataField="LockName"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lblValue %>" DataField="LockValue"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lblSubValue %>" DataField="SessionID"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lblUser %>" DataField="LockUserName"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lblTime %>" DataField="LockedTime"></asp:BoundField>
                                    <asp:BoundField DataField="IsSystemLock" Visible="false"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("LockName") %>_<%# Eval("LockValue") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="lnkUnlock" runat="server" CommandName="Unlock" Text="<%$ Resources:lnkUnlock %>" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnUnlock" runat="server" Text="<%$ Resources:btnUnlock %>" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvLockManager" EventName="Load"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvLockManager" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvLockManager" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="gvLockManager" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnUnlock" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
