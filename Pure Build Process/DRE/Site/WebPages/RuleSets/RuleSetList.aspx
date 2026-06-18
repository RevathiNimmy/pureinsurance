<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="RuleSets" Title="RuleSets" CodeBehind="RuleSetList.aspx.cs" %>

<%@ Import Namespace="RulesEngine.EngineCommon" %>

<%@ Register src="../../WebUserControls/ActionsControl.ascx" tagname="ActionsControl" tagprefix="uc1" %>
<%@ Register src="../../WebUserControls/Action.ascx" tagname="Action" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="form1">
    <table class="GridTableStyle">
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvRuleSets" runat="server" EmptyDataText="No ruleSets found..."
                    AutoGenerateColumns="false" CssClass="tableStyle" AllowPaging="True" PageSize="20"
                    OnRowCommand="gvRuleSet_RowCommand" OnRowCreated="gvRuleSet_RowCreated" OnPageIndexChanging="gvRuleSet_PageIndexChanging">
                    <AlternatingRowStyle CssClass="alternatingrowstyle" />
                    <HeaderStyle CssClass="headerstyle" HorizontalAlign="Left" VerticalAlign="Top" />
                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton runat="server" Text="*"  CommandName="Sort" CommandArgument="RUS_OVERRIDE_OUTPUT" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#
                                    (DataBinder.Eval(Container, "DataItem.RUS_OVERRIDE_OUTPUT").Equals(DBNull.Value) ? 
                                        "" : 
                                        (DataBinder.Eval(Container, "DataItem.RUS_OVERRIDE_OUTPUT").Equals("1") ?
                                            "*" :
                                            ""))
                                %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblRuleSetNameHeader"  runat="server" Text="RuleSet Name" CommandName="Sort" CommandArgument="RUS_NAME" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRuleSetName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RUS_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblSystemNameHeader"  runat="server" Text="System Name" CommandName="Sort" CommandArgument="RSY_NAME" />
                                <br />
                                <asp:DropDownList ID="ddlSys" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSystem_SelectedIndexChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbSystemName" runat="server" Text='<%# 
                                            (DataBinder.Eval(Container, "DataItem.RSY_NAME").Equals(DBNull.Value) ? 
                                                DataBinder.Eval(Container, "DataItem.CUSTOMER_RSY_NAME") :
                                                (DataBinder.Eval(Container, "DataItem.RSY_NAME").Equals(String.Empty) ? 
                                                    DataBinder.Eval(Container, "DataItem.CUSTOMER_RSY_NAME") :
                                                    DataBinder.Eval(Container, "DataItem.RSY_NAME"))) %>' CommandArgument='<%# 
                                            (DataBinder.Eval(Container, "DataItem.RSY_NAME").Equals(DBNull.Value) ? 
                                                DataBinder.Eval(Container, "DataItem.CUSTOMER_RULE_SYSTEM_KEY") :
                                                (DataBinder.Eval(Container, "DataItem.RSY_NAME").Equals(String.Empty) ? 
                                                    DataBinder.Eval(Container, "DataItem.CUSTOMER_RULE_SYSTEM_KEY") :
                                                    DataBinder.Eval(Container, "DataItem.RULE_SYSTEM_KEY"))) %>'
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
                        
                        <asp:TemplateField HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblProfileNameHeader"  runat="server" Text="Profile Name" CommandName="Sort" CommandArgument="RPR_NAME" />
                                <br />
                                <asp:DropDownList ID="ddlProf" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProfile_SelectedIndexChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblProfileName" runat="server" Text='<%# 
                                            (DataBinder.Eval(Container, "DataItem.RPR_NAME").Equals(DBNull.Value) ? 
                                                    "All Profiles" :
                                                    (DataBinder.Eval(Container, "DataItem.RPR_NAME").Equals(String.Empty) ? 
                                                        "All Profiles" :
                                                        DataBinder.Eval(Container, "DataItem.RPR_NAME"))) %>' CommandArgument='<%# 
                                            (DataBinder.Eval(Container, "DataItem.RPR_NAME").Equals(DBNull.Value) ? 
                                                    0 :
                                                    (DataBinder.Eval(Container, "DataItem.RPR_NAME").Equals(String.Empty) ? 
                                                        0 :
                                                        DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY")))  %>'
                                    CommandName="ViewProfile" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="DateColumn" ItemStyle-CssClass="DateColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblEffectiveDateHeader"  runat="server" Text="Effective Date" CommandName="Sort" CommandArgument="RUS_EFFECTIVE_DATE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.RUS_EFFECTIVE_DATE")).ToShortDateString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderStyle-CssClass="EnumeratedColumn" ItemStyle-CssClass="EnumeratedColumn">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStatusHeader"  runat="server" Text="Status" CommandName="Sort" CommandArgument="RUS_ACTIVE" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# (RuleEngineStatus)(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RUS_ACTIVE"))) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Locked" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn">
                            <ItemTemplate>
                                <div style="text-align:center">
                                <asp:Image ID="Image1"
                                    ToolTip='<%# DataBinder.Eval(Container, "DataItem.LOCKED_BY_USER").Equals(DBNull.Value) ? 
                                                    string.Empty : 
                                                    DataBinder.Eval(Container, "DataItem.LOCKED_BY_USER")  %>' 
                                    runat="server" ImageUrl="~/App_Themes/Images/Locked.png" 
                                    Visible='<%# DataBinder.Eval(Container, "DataItem.LOCKED_BY_USER").Equals(DBNull.Value) ? false : true  %>' Width="23px" />
                                </div>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="TextColumn" ItemStyle-CssClass="TextColumn" ItemStyle-Width="200px">
                            <ItemTemplate>                                   
                                <uc1:ActionsControl ID="ActionsControl1" runat="server" RowIndex='<%# Container.DataItemIndex %>' >
                                    <Actions>
                                        <uc1:Action Order='<%# (((RuleEngineStatus)(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RUS_ACTIVE")))).Equals(RuleEngineStatus.Live) ? 1 : 0) %>' runat="server" ID="ActionEditRules" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_SET_KEY")) + "," + Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY")) %>' ToolTip="Edit Rules" CommandName="EditRules" Text="Edit Lines" />
                                        <uc1:Action Order='<%# (((RuleEngineStatus)(Convert.ToInt32(DataBinder.Eval(Container, "DataItem.RUS_ACTIVE")))).Equals(RuleEngineStatus.Live) ? 0 : 1) %>' runat="server" ID="ActionViewRules" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_SET_KEY")) + "," + Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY")) %>' ToolTip="View Lines" CommandName="ViewRules" Text="View Lines" />
                                        <uc1:Action Order="2" runat="server" ID="ActionEdit" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_SET_KEY")) + "," + Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY")) %>' ToolTip="Edit" CommandName="EditRuleSet" Text="Edit Header" />
                                        <uc1:Action Order="3" runat="server" ID="ActionView" CommandArgument='<%# Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_SET_KEY")) + "," + Convert.ToString(DataBinder.Eval(Container, "DataItem.RULE_PROFILE_KEY")) %>'  ToolTip="View" CommandName="ViewRuleSet" Text="View Header" />
                                        <uc1:Action Order="4" runat="server" ID="ActionGrids" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_SET_KEY") %>' ToolTip="Grids" CommandName="RuleSetGrids" Text="Grids" />
                                        <uc1:Action Order="5" runat="server" ID="ActionCopyRuleSet" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RULE_SET_KEY") %>' ToolTip="Copy" CommandName="CopyRuleset" Text="Copy" />
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
                <asp:Button ID="btnAddNewRuleSet" runat="server" OnClick="btnAddNewRuleSet_Click"
                    Text="Add" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="ruleset">
        
        
        <asp:Panel runat="server" ID="pnlCopyRuleSet" CssClass="hidden">
            <asp:HiddenField ID="hdnRuleSetName" runat="server" />
            <asp:HiddenField ID="hdnRuleSetKey" runat="server" />
            <div class="question">
                <h2>
                    Copy Rule Set</h2>
            </div>
            <div class="question">
                <asp:Label ID="lblCreateNewRuleSet" runat="server" AssociatedControlID="cbxCreateNewRuleSet" Text="Create New Rule Set" />
                <asp:CheckBox ID="cbxCreateNewRuleSet" runat="server" />
            </div>
            <div class="question">
                <asp:Label ID="lblNewRuleSetName" runat="server" AssociatedControlID="txtNewRuleSetName" Text="New Rule Set Name" />
                <asp:TextBox ID="txtNewRuleSetName" runat="server" Width="300px" Style="text-align: left" />
            
                <ajax:TextBoxWatermarkExtender ID="tbwmNewRuleSetName" runat="server" TargetControlID="txtNewRuleSetName"
                    WatermarkText="Enter Rule Set Name Here If You Are Creating A New Rule Set" WatermarkCssClass="watermark" />
                
                <asp:CustomValidator ID="cvNewRuleSetName" runat="server" ClientValidationFunction="cvNewRuleSetName_ClientValidate"
                    OnServerValidate="cvNewRuleSetName_ServerValidate" Display="Dynamic" Text="*"
                    ToolTip="Rule set name must be unique for the profile" />

                <script type="text/javascript" language="javascript">
                    function cvNewRuleSetName_ClientValidate(source, args) {
                        if (document.getElementById("<%= pnlCopyRuleSet.ClientID %>").className == "hidden") {
                            args.IsValid = true;
                            return;
                        }

                        if (document.getElementById("<%= cbxCreateNewRuleSet.ClientID %>").checked == false) {
                            args.IsValid = true;
                            return;
                        }
                        
                        if (document.getElementById("<%= txtNewRuleSetName.ClientID %>").value == '') {
                            args.IsValid = false;
                            return;
                        }

                        if (document.getElementById("<%= txtNewRuleSetName.ClientID %>").value == '<%= tbwmNewRuleSetName.WatermarkText %>') {
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
                        if (document.getElementById("<%= pnlCopyRuleSet.ClientID %>").className == "hidden") {
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
                    ToolTip="Must be after today’s date and time" ValidationGroup="CopyRuleSetGroup" />
                <asp:CustomValidator ID="cvValidate" runat="server"
                    OnServerValidate="cvValidate_ServerValidate" Display="Dynamic" Text="*"
                    ToolTip="Name and effectivate date combination already in use" ValidationGroup="CopyRuleSetGroup" />
                <ajax:CalendarExtender PopupButtonID="imgEffectiveDate" ID="ceEffectiveDate" runat="server"
                    TargetControlID="txtEffectiveDate" Enabled="true" />
                
            </div>
            <div class="question">
                <asp:Label ID="tbEffectiveTime" runat="Server" Text="Effective Time:" AssociatedControlID="ddlEffectiveTimeHours" />
                <asp:DropDownList ID="ddlEffectiveTimeHours" runat="server" Width="4em" />
                <asp:Label ID="Label6" runat="Server" Text=":&nbsp;" Width="5px" AssociatedControlID="ddlEffectiveTimeMinutes" />
                <asp:DropDownList ID="ddlEffectiveTimeMinutes" runat="server" Width="4em" />
            </div>
            <br />
            <div class="question">
                <asp:Label ID="lblCopyGrids" runat="server" AssociatedControlID="cbxCopyGrids">Copy Grids</asp:Label>
                <asp:CheckBox ID="cbxCopyGrids" runat="server" />
            </div>
            <div class="question">
                <asp:Label ID="lblCopyRuleSetsKeys" runat="server" AssociatedControlID="cbxCopyRuleSetKeys">Copy Rule Set Keys</asp:Label>
                <asp:CheckBox ID="cbxCopyRuleSetKeys" runat="server" />
            </div>
            <div class="buttons">
                <asp:Button ID="btnCopy" runat="server" Text="Copy" ValidationGroup="CopyRuleSetGroup" CausesValidation="True" OnClick="btnCopyClick" />
                <asp:Button ID="btnGoBack" runat="server" Text="Cancel" CausesValidation="False" />
            </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlLockedInformation" CssClass="hidden">
            <div>
                <h2>
                    Locked</h2>
                <p>
                    This record has been locked by another user</p>
                <asp:HiddenField ID="hfdEntityID" runat="server" />
                <div class="question">
                    <asp:Label ID="Label1" runat="server" Text="Username:" AssociatedControlID="lblPnlLockedInformationUsername" />
                    <asp:Label ID="lblPnlLockedInformationUsername" runat="server" />
                </div>
                <br />
                <div class="question">
                    <asp:Label ID="Label4" runat="server" Text="Name:" AssociatedControlID="lblPnlLockedInformationName" />
                    <asp:Label ID="lblPnlLockedInformationName" runat="server" />
                </div>
                <div class="question">
                    <asp:Label ID="Label5" runat="server" Text="Email:" AssociatedControlID="lblPnlLockedInformationEmailHyperLink" />
                    <asp:HyperLink ID="lblPnlLockedInformationEmailHyperLink" runat="server" />
                </div>
                <br />
                <div class="question">
                    <asp:Label ID="Label2" runat="server" Text="Lock Created:" AssociatedControlID="lblPnlLockedInformationLockCreated" />
                    <asp:Label ID="lblPnlLockedInformationLockCreated" runat="server" />
                </div>
                <div class="question">
                    <asp:Label ID="Label3" runat="server" Text="Lock Timeout:" AssociatedControlID="lblPnlLockedInformationLockTimeOut" />
                    <asp:Label ID="lblPnlLockedInformationLockTimeOut" runat="server" />
                </div>
            </div>
            <div class="buttons">
                <input type="button" value="Close" onclick="javascript:hidePopup('<%=pnlLockedInformation.ClientID%>');" />
                <asp:Button ID="btnForceUnlock" runat="server" OnClick="btnForceUnlock_Click" Text="Force Unlock" />
            </div>
        </asp:Panel>
        
        <div class="hidden" id="washoutbox" />
    </div>
    </form>
</asp:Content>
