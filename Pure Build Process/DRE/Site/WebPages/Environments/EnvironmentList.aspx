<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Environments" Title="Untitled Page" CodeBehind="EnvironmentList.aspx.cs" EnableEventValidation="true" %>

<%@ Register Src="../../WebUserControls/ActionsControl.ascx" TagName="ActionsControl"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebUserControls/Action.ascx" TagName="Action" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="GridTableStyle">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvServerClass" runat="server" EmptyDataText="No Classifications found..."
                    AutoGenerateColumns="false" CssClass="tableStyle" AllowPaging="True" PageSize="20"
                    OnRowCommand="gvServerClass_RowCommand" OnRowCreated="gvServerClass_RowCreated"
                    OnRowDataBound="gvServerClass_RowDataBound" OnPageIndexChanging="gvServerClass_PageIndexChanging">
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblServerClassNameHeader" runat="server" Text="Server Classification Name"
                                    CommandName="Sort" CommandArgument="SVC_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblServerClassName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SVC_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbCustomerNameHeader" runat="server" Text="Customer" CommandName="Sort"
                                    CommandArgument="RCU_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCustomerName" runat="server" Text='<%# 
                                            (DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(DBNull.Value) ? 
                                                "All Customers" :
                                                (DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(String.Empty) ? 
                                                    "All Customers" :
                                                    DataBinder.Eval(Container, "DataItem.RCU_NAME"))) %>' CommandArgument='<%# 
                                            (DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(DBNull.Value) ? 
                                                0 :
                                                (DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(String.Empty) ? 
                                                    0 :
                                                    DataBinder.Eval(Container, "DataItem.RULE_CUSTOMER_KEY"))) %>'
                                    CommandName="ViewCustomer" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Live Server" HeaderStyle-CssClass="EnumeratedColumn"
                            ItemStyle-CssClass="EnumeratedColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblLocksHeader" runat="server" Text="Live Server" CommandName="Sort"
                                    CommandArgument="SVC_LIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLocks" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.SVC_LIVE")) == 1 ? "Yes" : "No") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>'>
                                    <Actions>
                                        <uc1:Action runat="server" ID="ActionView1" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.SERVER_CLASS_KEY") %>' Order="0" ToolTip="View" CommandName="ViewClass" Text="View" />
                                        <uc1:Action runat="server" ID="ActionEdit1" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.SERVER_CLASS_KEY") %>' Order="1" ToolTip="Edit" CommandName="EditClass" Text="Edit" />
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
                <asp:Button ID="btnAddClass" runat="server" OnClick="btnAddNewClass_Click" Text="Add" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvEnvironments" runat="server" EmptyDataText="No Environments found..."
                    AutoGenerateColumns="false" CssClass="tableStyle" AllowPaging="True" PageSize="20"
                    OnRowCommand="gvEnvironments_RowCommand" OnRowCreated="gvEnvironments_RowCreated"
                    OnRowDataBound="gvEnvironments_RowDataBound" OnPageIndexChanging="gvEnvironments_PageIndexChanging">
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblEnvironmentNameHeader" runat="server" Text="Server Location Name"
                                    CommandName="Sort" CommandArgument="SVL_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEnvironmentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SVL_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblUrlHeader" runat="server" Text="Physical Url" CommandName="Sort"
                                    CommandArgument="SVL_LOCATION" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUrl" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SVL_LOCATION") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblClassNameHeader" runat="server" Text="Server Classification"
                                    CommandName="Sort" CommandArgument="SVC_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClassName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.SVC_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbCustomerName2Header" runat="server" Text="Customer" CommandName="Sort"
                                    CommandArgument="RCU_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCustomerName2" runat="server" Text='<%# 
                                            (DataBinder.Eval(Container, "DataItem.SVC_CUSTOMER_CDE").Equals(DBNull.Value) ? 
                                                "All Customers" :
                                                (DataBinder.Eval(Container, "DataItem.SVC_CUSTOMER_CDE").Equals(String.Empty) ? 
                                                    "All Customers" :
                                                    DataBinder.Eval(Container, "DataItem.RCU_NAME"))) %>' CommandArgument='<%# 
                                            (DataBinder.Eval(Container, "DataItem.SVC_CUSTOMER_CDE").Equals(DBNull.Value) ? 
                                                0 :
                                                (DataBinder.Eval(Container, "DataItem.SVC_CUSTOMER_CDE").Equals(String.Empty) ? 
                                                    0 :
                                                    DataBinder.Eval(Container, "DataItem.SVC_CUSTOMER_CDE"))) %>'
                                    CommandName="ViewCustomer" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>'>
                                    <Actions>
                                        <uc1:Action runat="server" Order="0" ID="ActionView" CommandArgument='<%# (DataBinder.Eval(Container, "DataItem.SERVER_LOCATION_KEY")) %>' ToolTip="View" CommandName="ViewEnvironment" Text="View" />
                                        <uc1:Action runat="server" Order="1" ID="ActionEdit" CommandArgument='<%# (DataBinder.Eval(Container, "DataItem.SERVER_LOCATION_KEY")) %>' ToolTip="Edit" CommandName="EditEnvironment" Text="Edit" />
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
                <asp:Button ID="btnAddNewEnvironment" runat="server" OnClick="btnAddNewEnvironment_Click"
                    Text="Add" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
