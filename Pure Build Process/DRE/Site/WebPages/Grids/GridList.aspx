<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    Inherits="GridList" 
    MasterPageFile="~/MasterPage.master" 
    Codebehind="GridList.aspx.cs" 
 %>

<%@ Register src="../../WebUserControls/ActionsControl.ascx" tagname="ActionsControl" tagprefix="uc1" %>
<%@ Register src="../../WebUserControls/Action.ascx" tagname="Action" tagprefix="uc1" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">   
 
    <asp:GridView ID="gvGrids" 
                  runat="server" 
                  EmptyDataText="No grids found..." 
                  AutoGenerateColumns="false" 
                  CssClass="tableStyle" 
                  AllowPaging="True" PageSize="20" 
                  OnRowCommand="gvGrids_RowCommand">
        <AlternatingRowStyle CssClass="alternatingrowstyle" />
        <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" />
        <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />                    
        <Columns>
        
            <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                <HeaderTemplate>
                    <asp:LinkButton ID="lblSystemNameHeader"  runat="server" Text="Name" CommandName="Sort" CommandArgument="RGR_NAME" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSystemName" runat="server"
                        Text='<%# DataBinder.Eval(Container, "DataItem.RGR_NAME") %>' />
                </ItemTemplate>
            </asp:TemplateField>   
            
            <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                <HeaderTemplate>
                    <asp:LinkButton ID="lblEffectiveDateHeader"  runat="server" Text="Effective Date" CommandName="Sort" CommandArgument="RGR_EFFECTIVE_DATE" />
                    <asp:TextBox ID="tbEffectiveDate" runat="server" AutoPostBack="true" OnTextChanged="tbEffectiveDate_TextChanged" />
                    <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/App_Themes/Images/Calendar_scheduleHS.png" />
                    <ajax:CalendarExtender ID="ceEffectiveDate" runat="server" 
                             TargetControlID="tbEffectiveDate" PopupButtonID="imgEffectiveDate" Enabled="true" />
                </HeaderTemplate>                
                <ItemTemplate>
                    <asp:Label ID="lblEffectiveDate" runat="server"
                        Text='<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.RGR_EFFECTIVE_DATE")).ToShortDateString() %>' />
                </ItemTemplate>
            </asp:TemplateField>   
            
            <asp:TemplateField HeaderStyle-CssClass="NumericColumn" ItemStyle-CssClass="NumericColumn">
                <HeaderTemplate>
                    <asp:LinkButton ID="tbVersionHeader"  runat="server" Text="Version" CommandName="Sort" CommandArgument="RGR_VERSION" />
                    <asp:CheckBox ID="chkVer" Text="Show All" EnableViewState="true" AutoPostBack="true" runat="server" Checked="false" OnCheckedChanged="chkVersion_CheckedChanged">
                    </asp:CheckBox>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="tbVersion" runat="server"
                        Text='<%# DataBinder.Eval(Container, "DataItem.RGR_VERSION") %>' />                                
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                <HeaderTemplate>
                    <asp:LinkButton ID="lblFacetsHeader"  runat="server" Text="Dimension" CommandName="Sort" CommandArgument="RGR_GRID_TYPE" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFacets" runat="server"
                        Text='<%# DataBinder.Eval(Container, "DataItem.RGR_GRID_TYPE") %>' />
                </ItemTemplate>
            </asp:TemplateField> 
            
            <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                <HeaderTemplate>
                    <asp:LinkButton ID="lblInputContentsHeader"  runat="server" Text="Grid Columns" CommandName="Sort" CommandArgument="RGR_COLUMN_HEADERS" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblInputContents" runat="server"
                        Text='<%#   (DataBinder.Eval(Container, "DataItem.RGR_COLUMN_HEADERS")).Equals(DBNull.Value) ?
                                        String.Empty :
                                        ((((String)(DataBinder.Eval(Container, "DataItem.RGR_COLUMN_HEADERS"))).Length < 150) ?
                                            ((DataBinder.Eval(Container, "DataItem.RGR_COLUMN_HEADERS")).Equals(DBNull.Value) ? 
                                                string.Empty : 
                                                ((String)(DataBinder.Eval(Container, "DataItem.RGR_COLUMN_HEADERS"))).Replace(",", ", ")) :
                                            (((DataBinder.Eval(Container, "DataItem.RGR_COLUMN_HEADERS")).Equals(DBNull.Value) ? 
                                                string.Empty : 
                                                ((String)(DataBinder.Eval(Container, "DataItem.RGR_COLUMN_HEADERS"))).Replace(",", ", ")).Substring(0, 150) + "..."))
                               %>' />
                </ItemTemplate>
            </asp:TemplateField>  
             
            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                <ItemTemplate>
                    <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>' >
                        <Actions>
                            <uc1:Action runat="server" Order="0" ID="ActionView" CommandArgument='<%# ((int)base.RequestParams.DREItem).ToString() + ", " + DataBinder.Eval(Container, "DataItem.RULE_GRID_KEY").ToString() %>'  ToolTip="View" CommandName="ViewGrid" Text="View"  />
                            <uc1:Action runat="server" Order="1" ID="ActionEdit" CommandArgument='<%# ((int)base.RequestParams.DREItem).ToString() + ", " + DataBinder.Eval(Container, "DataItem.RULE_GRID_KEY").ToString() %>' ToolTip="Edit" CommandName="EditGrid" Text="Edit" />
                        </Actions>
                    </uc1:ActionsControl>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:Button ID="btnAddNewGrid" runat="server" OnClick="btnAddNewGrid_Click" Text="Add" />
    <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" />

</asp:Content>
