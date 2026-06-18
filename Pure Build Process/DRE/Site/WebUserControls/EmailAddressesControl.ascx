<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebUserControls_EmailAddressesControl" Codebehind="EmailAddressesControl.ascx.cs" %>
<asp:Panel CssClass="complexentryfield" ID="pnlEmailAddressComplexEntryField" Width="400px" runat="server">
        <asp:UpdatePanel runat="server" id="UpdatePanel" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger controlid="btnAddEmailAddress" eventname="Click" />
        </Triggers>
            <ContentTemplate>
                <asp:Panel HorizontalAlign="Left" ForeColor="Black" Width="100%" Font-Bold="true" ID="pnlHeader" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblHeading" runat="server" Font-Bold="false" />
                                <asp:CustomValidator 
                            ID="EmailAddressCustomValidator" 
                            runat="server" 
                            OnServerValidate="EmailAddressCustomValidator_ServerValidate" 
                            ValidationGroup="SystemValidation"
                            Text="*"
                            ToolTip="There must be at least one email" />
                            </td>
                            <td align="right">
                                <asp:Label ID="lblShowHide" runat="server" />                        
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel HorizontalAlign="Left" Width="100%" ID="pnlEmailAddresses" runat="server">
                    <div>
                        <asp:GridView ID="gvEmailAddresses" runat="server" Width="100%" 
                            AutoGenerateColumns="false"
                            OnRowCommand="gvEmailAddresses_RowCommand" 
                            CssClass="tableStyle" >
                            <AlternatingRowStyle CssClass="alternatingrowstyle" />
                            <HeaderStyle CssClass="EmailAddressesHeaderStyle" HorizontalAlign="Left" />
                            <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />                    
                            <Columns>
                                <asp:TemplateField HeaderText="Email Address">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmailAddress" runat="server"
                                            Text='<%# DataBinder.Eval(Container, "DataItem") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    
                                    </FooterTemplate>
                                </asp:TemplateField>                            
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibEdit" 
                                            ImageUrl="~/App_Themes/Images/edit.png" 
                                            CommandName="EditAddress"
                                            CommandArgument='<%# DataBinder.Eval(Container, "DataItem") %>'                                
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>                            
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibDelete" 
                                            ImageUrl="~/App_Themes/Images/delete.png" 
                                                 CommandName="DeleteAddress"
                                                 CommandArgument='<%# DataBinder.Eval(Container, "DataItem") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>                            
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divDataEntry" class="EmailAddressDataEntry" runat="server">
                        <asp:TextBox ID="tbEmailAddress" runat="server" Width="300px" />
                        <asp:Button ID="btnAddEmailAddress" runat="server" Text="Add" Width="40px" OnClick="btnAddEmailAddress_Click" />
                        
                            <ajax:TextBoxWatermarkExtender ID="tbweEmailAddress" runat="server"
                                TargetControlID="tbEmailAddress"
                                WatermarkText="Type New Email Address Here"
                                WatermarkCssClass="watermark" />
                                <asp:RegularExpressionValidator ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ID="revEmailAddress" runat="server"
                                ErrorMessage="Invalid Email Address" ToolTip="Invalid Email Address" Text="*" Display="Static" ControlToValidate="tbEmailAddress">
                            </asp:RegularExpressionValidator>                        
                            <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="tbEmailAddress"
                                ValidationGroup="" Display="Static" Text="*" ToolTip="Enter email address to add" />
                    </div>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="cpeEmailAddresses" runat="Server" TargetControlID="pnlEmailAddresses"
                    Collapsed="True" ExpandControlID="pnlHeader" CollapseControlID="pnlHeader"
                    TextLabelID="lblShowHide" AutoCollapse="False" AutoExpand="False" ExpandDirection="Vertical"
                    CollapsedText="(Show...)"
                    ExpandedText="(Hide)" Enabled="false"  />    
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
