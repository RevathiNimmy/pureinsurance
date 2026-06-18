<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Customers" Title="Untitled Page" Codebehind="CustomerList.aspx.cs" %>
<%@ Import Namespace="RulesEngine.EngineCommon"%>
<%@ Register src="../../WebUserControls/ActionsControl.ascx" tagname="ActionsControl" tagprefix="uc1" %>
<%@ Register src="../../WebUserControls/Action.ascx" tagname="Action" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <table class="GridTableStyle">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvCustomers" runat="server"
                  EmptyDataText="No customers found..." 
                  AutoGenerateColumns="false" 
                  CssClass="tableStyle"
                  HtmlEncode="false" 
                  AllowPaging="True" PageSize="20" 
                  OnRowCommand="gvCustomers_RowCommand"
                  OnRowDatabound="gvCustomers_RowDataBound"
                  OnPageIndexChanging="gvCustomers_PageIndexChanging" >
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />                    
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblCustomerNameHeader"  runat="server" Text="Customer Name" CommandName="Sort" CommandArgument="RCU_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.RCU_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblDescriptionHeader"  runat="server" Text="Description" CommandName="Sort" CommandArgument="RCU_DESCRIPTION" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.RCU_DESCRIPTION") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                                                        
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblSystemNameHeader"  runat="server" Text="System Name" CommandName="Sort" CommandArgument="RSY_NAME" />
                                <asp:DropDownList ID="ddlSys" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSystem_SelectedIndexChanged">
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbSystemName" runat="server" 
                                    Text='<%# DataBinder.Eval(Container, "DataItem.RSY_NAME") %>' 
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RCU_SYSTEM_CDE") %>'
                                    CommandName="ViewSystem" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="EnumeratedColumn" ItemStyle-CssClass="EnumeratedColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStatusHeader"  runat="server" Text="Status" CommandName="Sort" CommandArgument="RCU_ACTIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"
                                    Text='<%# (RuleEngineStatus)(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RCU_ACTIVE"))) %>' />                                
                            </ItemTemplate>                                                       
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="NumericColumn" ItemStyle-CssClass="NumericColumn">
                         <HeaderTemplate>
                                <asp:LinkButton ID="lblProfileCountLiveHeader"  runat="server" Text="Profiles<br>(Live / Test)" CommandName="Sort" CommandArgument="PROFILESCOUNT_LIVE, PROFILESCOUNT_TRAIN" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProfileCountLive" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.PROFILESCOUNT_LIVE") + " / " + DataBinder.Eval(Container, "DataItem.PROFILESCOUNT_TRAIN") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                                                       
                        
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>' >
                                    <Actions>
                                        <uc1:Action runat="server" Order="0" ID="ActionView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_CUSTOMER_KEY") %>' ToolTip="View" CommandName="ViewCustomer" Text="View" />
                                        <uc1:Action runat="server" Order="1" ID="ActionEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_CUSTOMER_KEY") %>' ToolTip="Edit" CommandName="EditCustomer" Text="Edit" />
                                        <uc1:Action runat="server" Order="2" ID="ActionEnvironments" CommandArgument='<%# (DataBinder.Eval(Container, "DataItem.RCU_SYSTEM_CDE") + "," + DataBinder.Eval(Container, "DataItem.RULE_CUSTOMER_KEY")) %>' ToolTip="Environment" CommandName="Environments" Text="Environment" />
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
                <asp:Button ID="btnAddNewCustomer" runat="server" OnClick="btnAddNewCustomer_Click" Text="Add" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table> 
</asp:Content>

