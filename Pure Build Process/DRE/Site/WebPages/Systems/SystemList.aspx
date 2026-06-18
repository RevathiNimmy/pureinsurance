<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Systems" Title="Untitled Page" Codebehind="SystemList.aspx.cs" %>
<%@ Import Namespace="System.Data"%>
<%@ Import Namespace="RulesEngine.EngineCommon"%>

<%@ Register src="../../WebUserControls/ActionsControl.ascx" tagname="ActionsControl" tagprefix="uc1" %>
<%@ Register src="../../WebUserControls/Action.ascx" tagname="Action" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <table class="GridTableStyle">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvSystems" runat="server" 
                  EmptyDataText="No systems found..." 
                  AutoGenerateColumns="false" 
                  CssClass="tableStyle"
                  HtmlEncode="false" 
                  AllowPaging="True" PageSize="20" 
                  OnRowCommand="gvSystems_RowCommand" 
                  OnRowCreated="gvSystems_RowCreated"
                  OnRowDatabound="gvSystems_RowDataBound"
                  OnPageIndexChanging="gvSystems_PageIndexChanging" >
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />                    
                    <Columns>
                    
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblSystemNameHeader"  runat="server" Text="System Name" CommandName="Sort" CommandArgument="RSY_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSystemName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RSY_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                            
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblDescriptionHeader"  runat="server" Text="Description" CommandName="Sort" CommandArgument="RSY_DESCRIPTION" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.RSY_DESCRIPTION") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>                            
                        
                        <asp:TemplateField HeaderStyle-CssClass="EnumeratedColumn" ItemStyle-CssClass="EnumeratedColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStatusHeader"  runat="server" Text="Status" CommandName="Sort" CommandArgument="RSY_ACTIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%#  (RuleEngineStatus)(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSY_ACTIVE"))) %>' />
                            </ItemTemplate>                                                       
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="BooleanColumn" ItemStyle-CssClass="BooleanColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblLockedHeader"  runat="server" Text="Locked" CommandName="Sort" CommandArgument="RSY_LOCKED" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLocked" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RSY_LOCKED")) == 1 ? "Yes" : "No") %>' />                                
                            </ItemTemplate>                                                       
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="NumericColumn" ItemStyle-CssClass="NumericColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblProfileCountLiveHeader"  runat="server" Text="Profiles<br />(Live / Test)" CommandName="Sort" CommandArgument="PROFILESCOUNT_LIVE, PROFILESCOUNT_TRAIN" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProfileCountLive" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PROFILESCOUNT_LIVE") + " / " + DataBinder.Eval(Container, "DataItem.PROFILESCOUNT_TRAIN") %>' />
                            </ItemTemplate>                          
                        </asp:TemplateField>                                                                                                              
                        
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <uc1:ActionsControl  ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>' >
                                    <Actions>
                                        <uc1:Action runat="server" ID="ActionView" Order="0" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_SYSTEM_KEY") %>' ToolTip="View" CommandName="ViewSystem" Text="View" />
                                        <uc1:Action runat="server" ID="ActionEdit" Order="1" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_SYSTEM_KEY") %>'  ToolTip="Edit" CommandName="EditSystem" Text="Edit" />
                                        <uc1:Action runat="server" ID="ActionEnvironments" Order="2" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_SYSTEM_KEY")) + ",0" %>'  CommandName="Environments" Text="Environment" />
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
                <asp:Button ID="btnAddNewSystem" runat="server" OnClick="btnAddNewSystem_Click" Text="Add" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
  
</asp:Content>

