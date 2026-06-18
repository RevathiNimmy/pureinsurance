<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindCoverNoteBook.aspx.vb"
    MasterPageFile="~/default.master" Inherits="Nexus.secure_FindCoverNoteBook" Title="Cover Note Book" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            EnableCalender(false, 'Update')
            EnableCalender(false, 'Assigned')

         });
        function setAgent(sName, sKey) {
            tb_remove();
            document.getElementById('<%= txtAgentCode.ClientId%>').value = unescape(sName.replace(/\+/g, " "));;
            document.getElementById('<%= hiddenAgentName.ClientId%>').value = unescape(sName.replace(/\+/g, " "));;
            document.getElementById('<%= hiddenAgentCode.ClientId%>').value = sKey;
        }

        //Enable or Disable the calender control and linked textbox 
        //on change of associated checkbox
        function EnableCalender(status, calenderCtl) {
            if (status == true && calenderCtl == 'Update') {
                $('#<%= txtCoverNoteLastUpdate.ClientId %>').datepicker('enable').removeAttr('disabled');
            }
            else if (status == false && calenderCtl == 'Update') {
                $('#<%= txtCoverNoteLastUpdate.ClientId %>').datepicker('disable').attr('disabled', 'disabled');
            }
            else if (status == true && calenderCtl == 'Assigned') {
                $('#<%= txtAssignedDate.ClientId %>').datepicker('enable').removeAttr('disabled');
            }
            else if (status == false && calenderCtl == 'Assigned') {
                $('#<%= txtAssignedDate.ClientId %>').datepicker('disable').attr('disabled', 'disabled');
            }
}

//Will be called on page load
//This will enable/disable calender controls on the basis of associated checkbox value
onload = function () {
    var LastUpdate = $('#<%= ChkLastUpdate.ClientId%>');
    var AssignedDate = $('#<%= ChkAssignedDate.ClientId%>')

    if (LastUpdate.attr('checked') == false) {
        $('#<%= txtCoverNoteLastUpdate.ClientId %>').datepicker('disable');
    }

    if (AssignedDate.attr('checked') == false) {
        $('#<%= txtAssignedDate.ClientId %>').datepicker('disable');
    }
}


function updated() {
    tb_remove();
    document.getElementById('<%= btnFindNow.ClientId%>').click();
}

function pageLoad() {
    //this is needed if the trigger is external to the update panel   
    var manager = Sys.WebForms.PageRequestManager.getInstance();
    manager.add_beginRequest(OnBeginRequest);
}

function OnBeginRequest(sender, args) {
    var postBackElement = args.get_postBackElement();
    if (postBackElement.id == 'btnNew') {
        $get(uprogQuotes).style.display = "block";
    }
}

    </script>

    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <div id="secure_FindCoverNoteBook">
        <asp:Panel ID="PnlFindCNB" runat="server" CssClass="card" DefaultButton="btnFindNow">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Literal ID="lblFindCoverNoteBook" runat="server" Text="<%$ Resources:lbl_FindCoverNoteBook %>"></asp:Literal>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBookNumber" runat="server" AssociatedControlID="txtBookNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litBookNumber" runat="server" Text="<%$ Resources:lbl_BookNumber %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtBookNumber" CssClass="form-control" runat="server" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStartNumber" runat="server" AssociatedControlID="txtStartNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litStartNumber" runat="server" Text="<%$ Resources:lbl_StartNumber %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtStartNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEndNumber" runat="server" AssociatedControlID="txtEndNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litEndNumber" runat="server" Text="<%$ Resources:lbl_EndNumber%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtEndNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAgentCode" Text="<%$ Resources:lbl_AgentCode%>" ID="lblbtnAgent"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtAgentCode" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnAgent" runat="server" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Agent Code</span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <asp:HiddenField ID="hiddenAgentCode" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenAgentName" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_CoverNoteLastUpdate" runat="server" AssociatedControlID="txtCoverNoteLastUpdate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litCoverNoteLastUpdate" runat="server" Text="<%$ Resources:lbl_CoverNoteLastUpdate%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <span class="input-group-addon">
                                        <asp:CheckBox ID="ChkLastUpdate" runat="server" OnClick="EnableCalender(this.checked,'Update')" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </span>
                                <asp:TextBox ID="txtCoverNoteLastUpdate" runat="server" CssClass="form-control"></asp:TextBox>
                                <uc1:CalendarLookup ID="calCoverNoteLastUpdate" runat="server" LinkedControl="txtCoverNoteLastUpdate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAssignedDate" runat="server" AssociatedControlID="txtAssignedDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litAssignedDate" runat="server" Text="<%$ Resources:lbl_AssignedDate%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <span class="input-group-addon">
                                        <asp:CheckBox ID="ChkAssignedDate" runat="server" OnClick="EnableCalender(this.checked,'Assigned')" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </span>
                                <asp:TextBox ID="txtAssignedDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <uc1:CalendarLookup ID="calCoverNoteAssignedDate" runat="server" LinkedControl="txtAssignedDate" HLevel="2"></uc1:CalendarLookup>
                            </div>

                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranch" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litBranch" runat="server" Text="<%$ Resources:lbl_Branch%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="field-medium form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="txtPolicyNumber" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litPolicyNumber" runat="server" Text="<%$ Resources:lbl_PolicyNumber%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPolicyNumber" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverNoteStatus" runat="server" AssociatedControlID="ddlCoverNoteStatus" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litCoverNoteStatus" runat="server" Text="<%$ Resources:lbl_CoverNoteStatus%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlCoverNoteStatus" runat="server" CssClass="field-medium form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:lbl_NewSearch %>" TabIndex="3" OnClientClick="javascript:Page_ValidationActive = false;" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNew" runat="server" Text="<%$ Resources:lbl_New%>" PostBackUrl="../secure/CoverNoteBook.aspx?Mode=Add" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFindNow" runat="server" Text="<%$ Resources:lbl_FindNow %>" TabIndex="3" SkinID="btnPrimary"></asp:LinkButton>
                
                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtBookNumber,txtStartNumber,txtEndNumber,txtAgentCode,txtPolicyNumber" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:CustomValidator ID="VldDate" runat="server" Display="None"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel ID="UpdFindCNB" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="BookNumber" HeaderText="<%$ Resources:lbl_BookNumber_g %>"></asp:BoundField>
                            <asp:BoundField DataField="StartNumber" HeaderText="<%$ Resources:lbl_StartNumber_g %>"></asp:BoundField>
                            <asp:BoundField DataField="EndNumber" HeaderText="<%$ Resources:lbl_EndNumber_g %>"></asp:BoundField>
                            <asp:BoundField DataField="AgentName" HeaderText="<%$ Resources:lbl_Agent_g %>"></asp:BoundField>
                            <asp:BoundField DataField="CoverNoteStatusDescription" HeaderText="<%$ Resources:lbl_Status_g %>"></asp:BoundField>
                            <asp:BoundField DataField="CoverNoteBranchDescription" HeaderText="<%$ Resources:lbl_Branch_g %>"></asp:BoundField>
                            <asp:BoundField DataField="LastUpdated" HeaderText="<%$ Resources:lbl_DateUpdated_g %>" DataFormatString="{0:d}" HtmlEncode="false"></asp:BoundField>
                            <asp:BoundField DataField="DateCreated" HeaderText="<%$ Resources:lbl_CreatedDate_g %>" DataFormatString="{0:d}" HtmlEncode="false"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("CoverNoteBookKey") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton Text="<%$ Resources:lbl_Edit %>" ID="lnkEdit" runat="server" SkinID="btnGrid"></asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </ContentTemplate>
            <Triggers>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upFindCoverNB" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdFindCNB" runat="server">
            <progresstemplate>
                        </progresstemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
