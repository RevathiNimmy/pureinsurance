<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Profiles" Title="Profiles" CodeBehind="ProfileList.aspx.cs" %>

<%@ Import Namespace="RulesEngine.EngineCommon" %>
<%@ Register Src="../../WebUserControls/ActionsControl.ascx" TagName="ActionsControl"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebUserControls/Action.ascx" TagName="Action" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="ajax" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="GridTableStyle">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvProfiles" runat="server" EmptyDataText="No profiles found..."
                    AutoGenerateColumns="false" CssClass="tableStyle" AllowPaging="True" PageSize="20"
                    OnRowCommand="gvProfiles_RowCommand" OnRowCreated="gvProfiles_RowCreated" OnPageIndexChanging="gvProfiles_PageIndexChanging">
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />
                    <Columns>
                    
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblProfileNameHeader"  runat="server" Text="Profile Name" CommandName="Sort" CommandArgument="RPR_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProfileName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RPR_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn"
                            ItemStyle-CssClass="TextColumn">
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
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn"
                            ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblCustomerNameHeader"  runat="server" Text="Customer Name" CommandName="Sort" CommandArgument="RCU_NAME" />
                                <br />
                                <asp:DropDownList ID="ddlCust" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbCustomerName" runat="server" Text='<%# 
                                        (DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(DBNull.Value) ? 
                                            "All Customers" : 
                                            DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(String.Empty) ? 
                                                "All Customers" : 
                                                DataBinder.Eval(Container, "DataItem.RCU_NAME")) %>' CommandArgument='<%# 
                                        (DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(DBNull.Value) ? 
                                            0 : 
                                            DataBinder.Eval(Container, "DataItem.RCU_NAME").Equals(String.Empty) ? 
                                                0 : 
                                                DataBinder.Eval(Container, "DataItem.RULE_CUSTOMER_KEY")) %>' CommandName="ViewCustomer" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="DateColumn" ItemStyle-CssClass="DateColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblEffectiveDateHeader"  runat="server" Text="Effective Date" CommandName="Sort" CommandArgument="RPR_EFFECTIVE_DATE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.RPR_EFFECTIVE_DATE")).ToShortDateString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="EnumeratedColumn" ItemStyle-CssClass="EnumeratedColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStatusHeader"  runat="server" Text="Status" CommandName="Sort" CommandArgument="RPR_ACTIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# (RuleEngineStatus)(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RPR_ACTIVE"))) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="NumericColumn" ItemStyle-CssClass="NumericColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblVersionHeader"  runat="server" Text="Version" CommandName="Sort" CommandArgument="RPR_VERSION" />
                                <br />
                                <asp:CheckBox ID="chkVer" Text="Show All" EnableViewState="true" AutoPostBack="true"
                                    runat="server" Checked="false" OnCheckedChanged="chkVersion_CheckedChanged">
                                </asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="tbVersion" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RPR_VERSION") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblTokenHeader"  runat="server" Text="Token" CommandName="Sort" CommandArgument="RPR_TOKEN" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblToken" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RPR_TOKEN") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>'>
                                    <Actions>
                                        <uc1:Action Order="0" runat="server" ID="ActionView" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY") %>' ToolTip="View" CommandName="ViewProfile" Text="View" />
                                        <uc1:Action Order="1" runat="server" ID="ActionEdit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY") %>' ToolTip="Edit" CommandName="EditProfile" Text="Edit" />
                                        <uc1:Action Order="2" runat="server" ID="ActionGrids" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY") %>' ToolTip="Grids" CommandName="ProfileGrids" Text="Grids" />
                                        <uc1:Action Order="3" runat="server" ID="ActionCopy" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY") %>' ToolTip="Copy" CommandName="CopyProfile" Text="Copy" />
                                    </Actions>
                                </uc1:ActionsControl>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField Visible="false" HeaderText="SystemLocked" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfdSystemLocked" runat="server" Value='<%# 
                                        DataBinder.Eval(Container, "DataItem.RSY_LOCKED").Equals(DBNull.Value) ?
                                            "1" :
                                            DataBinder.Eval(Container, "DataItem.RSY_LOCKED")
                                    %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField Visible="false" HeaderText="CustomerLocked" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfdCustomerActive" runat="server" Value='<%# 
                                        DataBinder.Eval(Container, "DataItem.RCU_ACTIVE").Equals(DBNull.Value) ?
                                            "1" :
                                            DataBinder.Eval(Container, "DataItem.RCU_ACTIVE")
                                    %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right" width="95%">
                <asp:Button ID="btnAddNewProfile" runat="server" OnClick="btnAddNewProfile_Click"
                    Text="Add" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="ruleset">
        <asp:Panel runat="server" ID="pnlCopyProfile" CssClass="hidden">
            <asp:HiddenField ID="hdnProfileKey" runat="server" />
            <div class="question">
                <h2>
                    Copy Profile</h2>
            </div>
            <div class="question">
                <asp:Label ID="lblProfile1" runat="server" AssociatedControlID="txtProfile1" Text="Copy From Profile" />
                <asp:TextBox ID="txtProfile1" ReadOnly="true" runat="server" Width="185px" Style="text-align: left" />
            </div>
            <div class="question">
                <asp:Label ID="lblProfile2" runat="server" AssociatedControlID="txtProfile2" Text="New Profile Name" />
                <asp:TextBox ID="txtProfile2" runat="server" Width="185px" Style="text-align: left" />
                <asp:CustomValidator ID="cvProfile2" runat="server" ClientValidationFunction="cvProfile2_ClientValidate"
                    OnServerValidate="cvProfile2_ServerValidate" Display="Dynamic" Text="*" ToolTip="Profile name is required and must be unique"
                    ControlToValidate="txtProfile2"  ValidationGroup="CopyProfileValidation" ValidateEmptyText="true" />

                <script type="text/javascript" language="javascript">
                    function cvProfile2_ClientValidate(source, args) {
                        if (document.getElementById("<%= pnlCopyProfile.ClientID %>").className == "hidden") {
                            args.IsValid = true;
                            return;
                        }

                        if (document.getElementById("<%= txtProfile2.ClientID %>").value == '') {
                            args.IsValid = false;
                            return;
                        }

                        if (document.getElementById("<%= txtProfile2.ClientID %>").value == '<%= tbwmEffectiveDate.WatermarkText %>') {
                            args.IsValid = false;
                            return;
                        }

                        args.IsValid = true;
                    }
                </script>

            </div>
            <br />
            <div class="question">

                <script type="text/javascript" language="javascript">
                    function cvEffectiveDate_ClientValidate(source, args) {
                        if (document.getElementById("<%= pnlCopyProfile.ClientID %>").className == "hidden") {
                            args.IsValid = true;
                            return;
                        }

                        var effectiveDate = new Date(document.getElementById("<%= txtEffectiveDate.ClientID %>").getAttribute("value"));

                        if (isNaN(effectiveDate)) {
                            args.IsValid = false;
                            return;
                        }

                        var hoursDDL = document.getElementById("<%= ddlEffectiveTimeHours.ClientID %>");
                        var minutesDDL = document.getElementById("<%= ddlEffectiveTimeMinutes.ClientID %>");

                        var hours = hoursDDL.options[hoursDDL.selectedIndex].text;

                        var minutes = minutesDDL.options[minutesDDL.selectedIndex].text;

                        effectiveDate.setHours(hours, minutes, 0, 0);

                        if (effectiveDate < new Date()) {
                            args.IsValid = false;
                        }
                        else {
                            args.IsValid = true;
                        }
                    }
                </script>

                <asp:Label ID="lblEffectiveDate" runat="server" AssociatedControlID="txtEffectiveDate"
                    Text="Effective Date" />
                <asp:TextBox ID="txtEffectiveDate" runat="server" Width="185px" />
                <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/App_Themes/Images/Calendar_scheduleHS.png" />
                <asp:CustomValidator ID="cvEffectiveDate" runat="server" ClientValidationFunction="cvEffectiveDate_ClientValidate"
                    OnServerValidate="cvEffectiveDate_ServerValidate" Display="Dynamic" Text="*"
                    ToolTip="Must be after today’s date and time"  ValidationGroup="CopyProfileValidation" />
                <ajax:CalendarExtender PopupButtonID="imgEffectiveDate" ID="ceEffectiveDate" runat="server"
                    TargetControlID="txtEffectiveDate" Enabled="true" />
                <ajax:TextBoxWatermarkExtender ID="tbwmEffectiveDate" runat="server" TargetControlID="txtProfile2"
                    WatermarkText="Type Name Of New Profile Here" WatermarkCssClass="watermark" />
            </div>
            <div class="question">
                <asp:Label ID="tbEffectiveTime" runat="Server" Text="Effective Time:" AssociatedControlID="ddlEffectiveTimeHours" />
                <asp:DropDownList ID="ddlEffectiveTimeHours" runat="server" Width="4em" />
                <asp:Label ID="Label4" runat="Server" Text=":&nbsp;" Width="5px" AssociatedControlID="ddlEffectiveTimeMinutes" />
                <asp:DropDownList ID="ddlEffectiveTimeMinutes" runat="server" Width="4em" />
            </div>
            <br />
            <div class="question">
                <asp:Label ID="lblGrids" runat="server" AssociatedControlID="cbxHasGrids">Grids</asp:Label>
                <asp:CheckBox ID="cbxHasGrids" runat="server" />
            </div>
            <div class="question">
                <asp:Label ID="lblRuleSets" runat="server" AssociatedControlID="cbxHasRuleSets">Rule Sets</asp:Label>
                <asp:CheckBox ID="cbxHasRuleSets" runat="server" />
            </div>
            <div class="question">
                <asp:Label ID="lblRuleSetsIncludeKeys" runat="server" AssociatedControlID="cbxHasRuleSetsIncludeKeys">Include Ruleset Keys?</asp:Label>
                <asp:CheckBox ID="cbxHasRuleSetsIncludeKeys" runat="server" Checked="true" />
            </div>
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="CopyProfileValidation" CausesValidation="True" OnClick="btnSave_Click" />
                <asp:Button ID="btnGoBack" runat="server" Text="Cancel" CausesValidation="False" />
            </div>
        </asp:Panel>
        <div class="hidden" id="washoutbox">
        </div>
    </div>
</asp:Content>
