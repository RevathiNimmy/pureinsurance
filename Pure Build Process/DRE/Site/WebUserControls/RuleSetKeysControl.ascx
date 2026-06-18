<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebUserControls_RuleSetKeysControl"
    CodeBehind="RuleSetKeysControl.ascx.cs" %>
<%@ Register Src="~/WebUserControls/RuleSetTreeControl.ascx" TagName="RuleSetTreeControl"
    TagPrefix="wuc" %>
<asp:Panel ID="pnlRuleSetKeysControl" runat="server" Width="60%" CssClass="ruleSetKeysControlKeys">
    <asp:Panel HorizontalAlign="Left" ForeColor="Black" Width="100%" Font-Bold="true"
        ID="pnlHeader" runat="server">
        <div class="RuleSetKeysControlHeaderStyle">
            <asp:Label ID="lblHeading" AssociatedControlID="lblHeading" Text="RuleSet Keys" CssClass="title"
                runat="server" />
            <asp:Label ID="lblShowHide" AssociatedControlID="lblShowHide" Text="" CssClass="showhide"
                runat="server" />
        </div>
    </asp:Panel>
    <asp:Panel HorizontalAlign="Left" Width="100%" ID="pnlRuleSetKeys" runat="server">
        <div class="ruleSetKeys">
            <div class="ruleSetKeysHeader">
                <%--Header's layout needs to be a table for exact alignment on top of re-ordered list which is rendered as table --%>
                <table id="EditableHeader" runat="server" cellspacing="0" cellpadding="0" border="0"
                    style="border-width: 0px; border-collapse: collapse; display: none;">
                    <tr>
                        <td>
                            <div>
                                <div class="dragHeader">
                                    <asp:Label ID="lblDragImageHeader" AssociatedControlID="lblDragImageHeader" runat="server"
                                        Text="" CssClass="drag" />
                                </div>
                            </div>
                        </td>
                        <td style="width: 100%;">
                            <div class="headerArea">
                                <asp:Label ID="lblSelectHeader" AssociatedControlID="lblSelectHeader" runat="server"
                                    Text="" CssClass="select" />
                                <asp:Label ID="lblPropertyHeader" AssociatedControlID="lblPropertyHeader" runat="server"
                                    Text="Property" CssClass="property" />
                                <asp:Label ID="lblFunctionHeader" AssociatedControlID="lblFunctionHeader" runat="server"
                                    Text="Function" CssClass="function" />
                                <asp:Label ID="lblValueHeader" AssociatedControlID="lblValueHeader" runat="server"
                                    Text="Value" CssClass="value" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="ReadonlyHeader" class="headerArea" runat="server" style="display: block;">
                    <asp:Label ID="lblSelectHeaderReadonly" AssociatedControlID="lblSelectHeaderReadonly"
                        runat="server" Text="" CssClass="select" />
                    <asp:Label ID="lblPropertyHeaderReadonly" AssociatedControlID="lblPropertyHeaderReadonly"
                        runat="server" Text="Property" CssClass="property" />
                    <asp:Label ID="lblFunctionHeaderReadonly" AssociatedControlID="lblFunctionHeaderReadonly"
                        runat="server" Text="Function" CssClass="function" />
                    <asp:Label ID="lblValueHeaderReadonly" AssociatedControlID="lblValueHeaderReadonly"
                        runat="server" Text="Value" CssClass="value" />
                </div>
            </div>
            <ajax:ReorderList ID="rlRuleSetKeys" runat="server" PostBackOnReorder="False" SortOrderField="Sequence"
                DataKeyField="Key" DataSourceID="odsRuleSetKeys" DragHandleAlignment="Left" AllowReorder="False"
                CssClass="ruleSetKeysList" OnItemCommand="rlRuleSetKeys_ItemCommand" OnItemReorder="rlRuleSetKeys_ItemReorder">
                <ItemTemplate>
                    <%--This is a template for the items to be reordered.--%>
                    <div class="itemArea">
                        <asp:LinkButton ID="lbKey" runat="server" Text='<%# ((((Int32)DataBinder.Eval(Container, "DataItem.Sequence")) + 1) * 100) %>' CommandArgument='<%# ((AjaxControlToolkit.ReorderListItem) Container).DisplayIndex %>' />
                        <asp:Label ID="lblProperty" AssociatedControlID="lblProperty" CssClass="property"
                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Property") %>' />
                        <asp:Label ID="lblFunction" AssociatedControlID="lblFunction" CssClass="function"
                            runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Function") %>' />
                        <asp:Label ID="lblValue" AssociatedControlID="lblValue" CssClass="value" runat="server"
                            Text='<%# DataBinder.Eval(Container, "DataItem.Value") %>' />
                    </div>
                </ItemTemplate>
                <EmptyListTemplate>
                    <%--This template will show up if there were no items in the list to reorder.--%>
                    <div>
                        No keys specified.
                    </div>
                </EmptyListTemplate>
                <ReorderTemplate>
                    
                </ReorderTemplate>
                <DragHandleTemplate>
                    <div class="dragHandle">
                        <asp:Image ID="imgReorder" runat="server" AlternateText="move" ImageUrl="~/App_Themes/Images/reorder.gif" />
                    </div>
                </DragHandleTemplate>
            </ajax:ReorderList>
            <asp:ObjectDataSource ID="odsRuleSetKeys" runat="server" SelectMethod="RuleSetKeys"
                TypeName="RulesEngine.Website.ReorderRuleSetKeys" UpdateMethod="ReorderKeys">
                <UpdateParameters>
                    <asp:Parameter Name="key" Type="Int32" />
                    <asp:Parameter Name="property" Type="String" />
                    <asp:Parameter Name="function" Type="String" />
                    <asp:Parameter Name="value" Type="String" />
                    <asp:Parameter Name="sequence" Type="Int32" />
                </UpdateParameters>
            </asp:ObjectDataSource>
            <div style="clear:both;" />
        </div>
        <div id="divDataEntry" class="RuleSetKeysDataEntry" runat="server">
            <asp:Panel HorizontalAlign="Left" ForeColor="Black" Width="100%" Font-Bold="true"
                ID="pnlHeaderDataEntry" runat="server">
                <div class="RuleSetKeysDataEntryHeaderStyle">
                    <asp:Label ID="lblHeaderDataEntry" AssociatedControlID="lblHeaderDataEntry" Text="Add/Edit RuleSet Keys"
                        CssClass="title" runat="server" />
                    <asp:Label ID="lblShowHideDataEntry" AssociatedControlID="lblShowHideDataEntry" CssClass="showhide"
                        runat="server" />
                </div>
            </asp:Panel>
            <asp:Panel HorizontalAlign="Left" Width="100%" ID="pnlRuleSetKeysDataEntry" runat="server">
                <div class="rulesetkeys">
                    <wuc:RuleSetTreeControl ID="treePropertyExplorer" Height="150" runat="server" />
                    <div class="dataentry">
                        <div class="question">
                            <asp:Label Text="Line" runat="server" AssociatedControlID="tbLineNumber" />
                            <asp:TextBox ID="tbLineNumber" runat="server" />
                        </div>
                        <div class="question">
                            <asp:Label ID="lblProperty" AssociatedControlID="tbProperty" Text="Property" runat="server" />
                            <asp:HiddenField ID="hfRuleSetKeyIndex" runat="server" />
                            <asp:HiddenField ID="tbProperty" runat="server" />
                        </div>
                        <div class="question">
                            <asp:Label ID="lblFunction" AssociatedControlID="ddlFunction" Text="Function" runat="server" />
                            <asp:DropDownList ID="ddlFunction" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvFunction" runat="server" ControlToValidate="ddlFunction"
                                ValidationGroup="RuleSetKeys" Display="Static" Text="*" ToolTip="Function is mandatory" />
                        </div>
                        <div class="question">
                            <asp:Label ID="lblValue" AssociatedControlID="tbValue" Text="Value" runat="server" />
                            <asp:TextBox ID="tbValue" runat="server" Width="135px" />
                            <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="tbValue"
                                ValidationGroup="RuleSetKeys" Display="Static" Text="*" ToolTip="Value is mandatory" />
                        </div>
                        <div class="buttons">
                            <asp:Button ID="btnAddRuleSetKey" runat="server" Text="Add" OnClick="UpdateRuleSetKeys"
                                ValidationGroup="RuleSetKeys" />
                            <asp:Button ID="btnUpdateRuleSetKey" runat="server" Text="Update" OnClick="UpdateRuleSetKeys"
                                ValidationGroup="RuleSetKeys" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="UpdateRuleSetKeys"
                                CausesValidation="false" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel Update" OnClick="UpdateRuleSetKeys"
                                CausesValidation="false" />
                            <ajax:TextBoxWatermarkExtender ID="tbweValue" runat="server" TargetControlID="tbValue"
                                WatermarkText="Type Value Here" WatermarkCssClass="watermark" />
                        </div>
                    </div>
                </div>
                 <div style="clear: both" />
            </asp:Panel>
           
           <ajax:CollapsiblePanelExtender ID="cpeDataEntry" runat="Server" TargetControlID="pnlRuleSetKeysDataEntry"
                Collapsed="True" ExpandControlID="pnlHeaderDataEntry" CollapseControlID="pnlHeaderDataEntry"
                TextLabelID="lblShowHideDataEntry" AutoCollapse="False" AutoExpand="False" ExpandDirection="Vertical"
                CollapsedText="(Show...)" ExpandedText="(Hide)" />
        </div>
        
    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeRuleSetKeys" runat="Server" TargetControlID="pnlRuleSetKeys"
        Collapsed="False" ExpandControlID="pnlHeader" CollapseControlID="pnlHeader" TextLabelID="lblShowHide"
        AutoCollapse="False" AutoExpand="False" ExpandDirection="Vertical" CollapsedText="(Show...)"
        ExpandedText="(Hide)" CollapsedSize="0" />
</asp:Panel>
