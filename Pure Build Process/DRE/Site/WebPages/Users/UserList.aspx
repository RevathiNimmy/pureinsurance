<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Users" Title="Untitled Page" CodeBehind="UserList.aspx.cs" %>

<%@ Register Src="../../WebUserControls/ActionsControl.ascx" TagName="ActionsControl"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebUserControls/Action.ascx" TagName="Action" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <table class="GridTableStyle">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvUsers" runat="server" EmptyDataText="No users found..." AutoGenerateColumns="false"
                    CssClass="tableStyle" AllowPaging="True" PageSize="20" OnRowCommand="gvUsers_RowCommand"
                    OnPageIndexChanging="gvUsers_PageIndexChanging">
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblUserLoginHeader"  runat="server" Text="User Login" CommandName="Sort" CommandArgument="RSU_LOGON_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUserLogin" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RSU_LOGON_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblUserNameHeader"  runat="server" Text="User Name" CommandName="Sort" CommandArgument="RSU_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RSU_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblSystemNameHeader"  runat="server" Text="System Name" CommandName="Sort" CommandArgument="RSY_NAME" />
                                <br />
                                <asp:DropDownList ID="ddlSys" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSystem_SelectedIndexChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbSystemName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RSY_NAME") %>'
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_SYSTEM_KEY") %>'
                                    CommandName="ViewSystem" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblCustomerNameHeader"  runat="server" Text="Customer Name" CommandName="Sort" CommandArgument="RCU_NAME" />
                                <br />
                                <asp:DropDownList ID="ddlCust" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCustomerName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RCU_NAME") %>'
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RSU_CUSTOMER_CDE") %>'
                                    CommandName="ViewCustomer" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStatusHeader"  runat="server" Text="Status" CommandName="Sort" CommandArgument="RSU_LOGIN_ATTEMPTS_LEFT, RSU_ACTIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSU_LOGIN_ATTEMPTS_LEFT")) == 0 ? "Locked" : (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSU_ACTIVE")) == 1 ? "Active" : "Inactive")  ) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="BooleanColumn" ItemStyle-CssClass="BooleanColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblActiveHeader"  runat="server" Text="Active" CommandName="Sort" CommandArgument="RSU_ACTIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblActive" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSU_ACTIVE")) == 1 ? "Yes" : "No") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="BooleanColumn" ItemStyle-CssClass="BooleanColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblSuperUserHeader"  runat="server" Text="Super User" CommandName="Sort" CommandArgument="RSU_SUPERUSER" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSuperUser" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSU_SUPERUSER")) == 1 ? "Yes" : "No") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="BooleanColumn" ItemStyle-CssClass="BooleanColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblAdministratorHeader"  runat="server" Text="Administrator" CommandName="Sort" CommandArgument="RSU_ADMIN" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAdministrator" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSU_ADMIN")) == 1 ? "Yes" : "No") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>'>
                                    <Actions>
                                        <uc1:Action Order="0" runat="server" ID="ActionView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_USER_KEY") %>' ToolTip="View" CommandName="ViewUser" Text="View" />
                                        <uc1:Action Order="1" runat="server" ID="ActionEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_USER_KEY") %>' ToolTip="Edit" CommandName="EditUser" Text="Edit" />
                                    </Actions>
                                </uc1:ActionsControl>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" width="95%">
                <asp:Button ID="btnAddNewUser" runat="server" OnClick="btnAddNewUser_Click" Text="Add" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </form>
</asp:Content>
