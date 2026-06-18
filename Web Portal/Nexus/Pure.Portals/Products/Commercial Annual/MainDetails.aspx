<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MainDetails.aspx.vb" Inherits="Nexus.Products_Motor_MainDetails" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/BOCoverDates.ascx" TagName="CoverDate" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/SubAgents.ascx" TagName="SubAgents" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/Contacts.ascx" TagName="Contact" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("divMain").scrollTop = strPos;
            }
        }
        function SetDivPosition() {
            var intY = document.getElementById("divMain").scrollTop;
            document.cookie = "yPos=!~" + intY + "~!";
        }

        $(document).ready(function () {
            MakeFieldsDisabled();
            if ($('#<%=hdnIsBroker.ClientID %>').val() == "1") {
                $('#<%=POLICYHEADER__BUSINESSTYPE.ClientID %>').attr('disabled', true);
                $('#<%= POLICYHEADER__AGENTCODE.ClientID%>').attr('disabled', true);
                $('#<%= POLICYHEADER__BTNAGENTCODE.ClientID%>').attr('disabled', true);
            }
        });
        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        }

        function OnBeginRequest(sender, args) {
            //disable the button (or whatever else we need to do) here
            var btnNext = document.getElementById('<%= btnNext.ClientId%>');

            if (btnNext != null) {
                btnNext.disabled = true;
            }
        }

        function OnEndRequest(sender, args) {
            //enable the button (or whatever else we need to do) here
            var btnNext = document.getElementById('<%= btnNext.ClientId%>');

            if (btnNext != null) {
                btnNext.disabled = false;
            }
        }

        function LapseConfirmation() { return confirm('Are you sure to lapse this quote.'); }
        //   function setAgent(sName,sKey,sCode,sAgentType)
        //   {
        //   debugger;
        //   tb_remove();  
        //   document.getElementById('<%= POLICYHEADER__AGENTCODE.ClientId%>').value=sCode;
        //   document.getElementById('<%= POLICYHEADER__AGENT.ClientId%>').value=sKey;
        //   document.getElementById('<%= hAgentType.ClientId%>').value=sAgentType;
        //   //document.getElementById('<%= POLICYHEADER__AGENTCODE.ClientId%>').focus
        //   __doPostBack('Agent' , 'RefreshAgent');
        //   }

        function setAgent(sName, sKey, sCode, sAgentType) {

            tb_remove();
            document.getElementById('ctl00_cntMainBody_POLICYHEADER__AGENTCODE').value = sName;
            document.getElementById('ctl00_cntMainBody_POLICYHEADER__AGENT').value = sKey;
            document.getElementById('ctl00_cntMainBody_DRIVERDET__AGENTCODE').value = sKey;
            document.getElementById('<%= hAgentType.ClientId%>').value = sAgentType;
            //document.getElementById('ctl00_cntMainBody_POLICYHEADER__BUSINESSTYPE').value = "AGENCY";
            var bType = document.getElementById('ctl00_cntMainBody_POLICYHEADER__BUSINESSTYPE').value;
            switch (bType) {
                case "COIN LEAD":
                    document.getElementById('ctl00_cntMainBody_POLICYHEADER__BUSINESSTYPE').value = "COIN LEAD";
                    break;
                case "AGENCY":
                    document.getElementById('ctl00_cntMainBody_POLICYHEADER__BUSINESSTYPE').value = "AGENCY";
                    break;
                default:
                    document.getElementById('ctl00_cntMainBody_POLICYHEADER__BUSINESSTYPE').value = "AGENCY";
            }
            document.getElementById('<%=hvAgentName.ClientID %>').value = sName;

            __doPostBack('Agent', 'RefreshAgent');
        }
        function setAccountHandler(sName, sKey, sCode) {
            tb_remove();
            document.getElementById('<%= POLICYHEADER__HANDLER.ClientId%>').value = sCode;
            document.getElementById('<%= hiddenHandlerCode.ClientId%>').value = sKey;
            document.getElementById('<%= POLICYHEADER__HANDLER.ClientId%>').focus
        }
        function ChangeEndDate(iPeriod, oControlStartdate, oControlEndDate, MidnightRenewal, iControl) {


            //if iControl = 0 then for Renewal date if iControl =1 then Cover To date
            dCoverStartDate = oControlStartdate.value;
            var arStartDate = dCoverStartDate.split('/');
            var dtTempDate = new Date(arStartDate[2], arStartDate[1] - 1, arStartDate[0]);
            if (StartDateClientValidate(dCoverStartDate) == false) {
                oControlStartdate.value = '';
                oControlEndDate.value = '';
                return false;
            }
            if (iControl == 1) {
                iPeriod = 12;
                dtTempDate.setMonth(dtTempDate.getMonth() + parseInt(iPeriod));
            }

            if (MidnightRenewal == 1) {

                if (iControl == 1) {
                    dtTempDate.setDate(dtTempDate.getDate() - 1);
                }
                else {
                    dtTempDate.setDate(dtTempDate.getDate() + 1);
                }
            }
            else if (MidnightRenewal == 0) {
                dtTempDate.setDate(dtTempDate.getDate());
            }
            if (dtTempDate.getMonth() > 8) {
                //dCoverEndDate=(dtTempDate.getMonth()+1)+'/'+dtTempDate.getDate()+'/'+dtTempDate.getYear()
                dCoverEndDate = dtTempDate.getDate() + '/' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear() // DD/MM/YYYY Format.
            }
            else {
                dCoverEndDate = dtTempDate.getDate() + '/' + '0' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear() // DD/MM/YYYY Format.
            }
            oControlEndDate.value = dCoverEndDate;
        }

        function StartDateClientValidate(valueDate) {
            if (valueDate == '')//If Start Date is blank 
            {
                return false;
            }
            else {
                var result, regEx;//Declare variables.
                //regEx = new RegExp(/^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1})))$/);  //Create regular expression object (mm/dd/yyyy)
                regEx = new RegExp(/^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$/);  //Create regular expression object.(DD/MM/YYYY)
                result = regEx.test(valueDate);  //Test for match.
                return result;
            }
        }

        function fillDate(iMode) {
            var CoverFromDate = $get('<%=POLICYHEADER__COVERSTARTDATE.ClientID%>');
            var CoverFromDateValue = CoverFromDate.value;
            var GracePeriod = $get('<%=hiddenGracePeriod.ClientID%>');
            var OptionSetting = $get('<%=hiddenOptionSetting.ClientID%>');
            var MidnightRenewalSettings = $get('<%=hiddenMidnightRenewalSettings.ClientID%>');
            var CoverToDate = $get('<%=POLICYHEADER__COVERENDDATE.ClientID%>');
            var Inception = $get('<%=POLICYHEADER__INCEPTION.ClientID%>');
            var Renewal = $get('<%=POLICYHEADER__RENEWAL.ClientID%>');
            var InceptionTPI = $get('<%=POLICYHEADER__INCEPTIONTPI.ClientID%>');
            var ProposalDate = $get('<%=POLICYHEADER__PROPOSALDATE.ClientID%>');
            var QuoteExpiryDate = $get('<%=POLICYHEADER__QUOTEEXPIRYDATE.ClientID%>');

            if (MidnightRenewalSettings.value == 1) {

                ChangeEndDate(OptionSetting.value, CoverFromDate, CoverToDate, MidnightRenewalSettings.value, 1);
                ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, 0);
            }
            else {

                ChangeEndDate(OptionSetting.value, CoverFromDate, CoverToDate, MidnightRenewalSettings.value, 1);
                ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, 0);
            }
            //if(iMode==0)
            // {
            Inception.value = CoverFromDate.value;
            InceptionTPI.value = CoverFromDate.value;
            //  }
            ProposalDate.value = CoverFromDate.value;

        }
        function fillDateRenewal() {


            var GracePeriod = $get('<%=hiddenGracePeriod.ClientID%>');
            var OptionSetting = $get('<%=hiddenOptionSetting.ClientID%>');
            var MidnightRenewalSettings = $get('<%=hiddenMidnightRenewalSettings.ClientID%>');
            var CoverToDate = $get('<%=POLICYHEADER__COVERENDDATE.ClientID%>');
            var Renewal = $get('<%=POLICYHEADER__RENEWAL.ClientID%>');

            if (MidnightRenewalSettings.value == 1) {

                ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, 0);
            }
            else {

                ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, 0);

            }
        }

        function CoverSheetValidation_Client(source, arguments) {
            //debugger;
            var IndexValue = $get('<%=POLICYHEADER__COVERNOTEBOOKNO.ClientID %>').selectedIndex;
            // Retrieve selected value of Dropdown List
            if (IndexValue >= 0) {
                var SelectedVal = $get('<%=POLICYHEADER__COVERNOTEBOOKNO.ClientID %>').options[IndexValue].value;
                var IsChecked = $get('<%=chkIsCoverNoteUsed.ClientID%>');
                var SheetIndexValue = $get('<%=POLICYHEADER__COVERNOTESHEETNO.ClientID %>').selectedIndex;
                if (SheetIndexValue >= 0) {
                    var SheetSelectedVal = $get('<%=POLICYHEADER__COVERNOTESHEETNO.ClientID %>').options[IndexValue].value;
                    if (IsChecked.checked == true && SelectedVal != "" && arguments.IsValid == true) {
                        if (SheetSelectedVal != "") {
                            arguments.IsValid = true;
                        }
                        else {
                            arguments.IsValid = false;
                        }
                    }
                }
            }
        }
        function AgentValidation_Client(source, arguments) {
            //debugger;
            var txtControl = $get('<%=POLICYHEADER__AGENTCODE.ClientID%>');
            // Retrieve selected index of Dropdown List
            var IndexValue = $get('<%=POLICYHEADER__BUSINESSTYPE.ClientID %>').selectedIndex;
            // Retrieve selected value of Dropdown List
            var SelectedVal = $get('<%=POLICYHEADER__BUSINESSTYPE.ClientID %>').options[IndexValue].value;
            if (SelectedVal != "" && arguments.IsValid == true) {
                if (SelectedVal != 'DIRECT' && txtControl.value.length == 0) {
                    arguments.IsValid = false;
                }
                else {
                    arguments.IsValid = true;
                }
            }
        }

        function NextButton() {
            //debugger;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function MakeFieldsDisabled() {
            document.getElementById('<%=POLICYHEADER__POLICYSTATUSCODE.ClientID%>').disabled = true;
            document.getElementById('<%= POLICYHEADER__INCEPTION.ClientID %>').readOnly = true;
            document.getElementById('<%= POLICYHEADER__RENEWAL.ClientID %>').readOnly = true;
            document.getElementById('<%= POLICYHEADER__INCEPTIONTPI.ClientID %>').readOnly = true;
            document.getElementById('<%= POLICYHEADER__QUOTEEXPIRYDATE.ClientID %>').readOnly = true;
        }

    </script>

    <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server"></asp:ScriptManager>
    <div class="main-details-page">
        <div class="nexus-fluid-layout" id="divMain" onscroll="SetDivPosition()">
            <uc3:ProgressBar ID="ProgressBar1" runat="server"></uc3:ProgressBar>
            <div class="card">
                <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
                <div class="card-body clearfix">
                    <asp:HiddenField ID="DRIVERDET__IS_BROKER" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="DRIVERDET__AGENTCODE" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnIsReadOnly" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hvAgentCode" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hvAgentName" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnIsBroker" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="DRIVERDET__CONTACT_EMAIL" runat="server"></asp:HiddenField>
                     <asp:HiddenField ID="hAgentType" runat="server"></asp:HiddenField>
                    <legend>
                        <asp:Label ID="lblHeading1" runat="server" Text="Basic Details"></asp:Label>
                    </legend>
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblInsuredName" runat="server" AssociatedControlID="POLICYHEADER__INSUREDNAME" Text="Insured Name" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="POLICYHEADER__INSUREDNAME" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdInsuredName" runat="server" ControlToValidate="POLICYHEADER__INSUREDNAME" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Error in Insured Name"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPolicyStatus" runat="server" AssociatedControlID="POLICYHEADER__POLICYSTATUSCODE" Text="Policy Status Code" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__POLICYSTATUSCODE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Policy_Status" DefaultText="(Please Select)" CssClass="field-medium form-control" AutoPostBack="true"></NexusProvider:LookupList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lbAlternateRef" runat="server" AssociatedControlID="POLICYHEADER__ALTERNATEREF" Text="Alternate Referance" class="col-md-4 col-sm-3 control-label"><em id="markAlternateRef" runat="server" visible="false">*</em></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="POLICYHEADER__ALTERNATEREF" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdAlternateRef" runat="server" ControlToValidate="POLICYHEADER__ALTERNATEREF" Display="none" Enabled="false" SetFocusOnError="true" ErrorMessage="Error in Alternate Referance"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="POLICYHEADER__POLICYNUMBER" Text="Policy No" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="POLICYHEADER__POLICYNUMBER" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdPolicyNumber" runat="server" ControlToValidate="POLICYHEADER__POLICYNUMBER" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Error in Policy Number"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblSelectProduct" runat="server" AssociatedControlID="POLICYHEADER__PRODUCT" Text="Select Product" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="POLICYHEADER__PRODUCT" runat="server" CssClass="field-medium form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="POLICYHEADER__BRANCH" Text="Branch Code" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="POLICYHEADER__BRANCH" runat="server" AutoPostBack="True" CssClass="field-mandatory form-control" OnSelectedIndexChanged="ddlBranchCode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="vldBranchCodeRequired" runat="server" Display="None" ControlToValidate="POLICYHEADER__BRANCH" ErrorMessage="Error in Branch Code" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAnalysisCode" runat="server" AssociatedControlID="POLICYHEADER__ANALYSISCODE" Text="Analysis Code" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__ANALYSISCODE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Analysis_Code" DefaultText="(Please Select)" CssClass="field-medium form-control"></NexusProvider:LookupList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblSubBranchCode" runat="server" AssociatedControlID="POLICYHEADER__SUBBRANCH" Text="Sub Branch Code" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <asp:UpdatePanel ID="POLICYHEADER__UpdatePanelSubBranch" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="POLICYHEADER__SUBBRANCH" runat="server" CssClass="field-mandatory field-medium form-control">
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BRANCH" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:RequiredFieldValidator ID="vldSubBranchCodeRequired" runat="server" Display="None" ControlToValidate="POLICYHEADER__SUBBRANCH" ErrorMessage="Error in Sub Branch Code" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBusinessSource" runat="server" AssociatedControlID="POLICYHEADER__BUSINESSTYPE" Text="Business Source" class="col-md-4 col-sm-3 control-label">
                                            
                            </asp:Label><div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__BUSINESSTYPE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Business_Type" DefaultText="(Please Select)" CssClass="field-mandatory form-control" AutoPostBack="true" OnSelectedIndexChange="POLICYHEADER__BUSINESSTYPE_SelectedIndexChange"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdBusinessSource" runat="server" ControlToValidate="POLICYHEADER__BUSINESSTYPE" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Error in Business"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Panel ID="POLICYHEADER__pnlcoinsplacement" runat="server">
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCoinsurancePlacement" runat="server" AssociatedControlID="POLICYHEADER__COINSURANCEPLACEMENT" Text="<%$ Resources:lblCoinsurancePlacement %>" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:RadioButtonList ID="POLICYHEADER__COINSURANCEPLACEMENT" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="asp-radio">
                                        <asp:ListItem Text="<%$ Resources:li_Gross %>" Value="GROSS"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:li_Net %>" Value="NETT"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:RequiredFieldValidator ID="vldCoinsurancePlacementRequired" runat="server" Enabled="false" ControlToValidate="POLICYHEADER__COINSURANCEPLACEMENT" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:rqdCoinsurancePlacement %>"></asp:RequiredFieldValidator>
                            </div>
                        </asp:Panel>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:UpdatePanel ID="POLICYHEADER__updPanelAgent" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblAgent" AssociatedControlID="POLICYHEADER__AGENTCODE" Text="Agent Code" class="col-md-4 col-sm-3 control-label">
                                    </asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="POLICYHEADER__AGENTCODE" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="POLICYHEADER__BTNAGENTCODE" runat="server" SkinID="btnModal" OnClientClick="tb_show(null , '../../Modal/FindAgent.aspx?FromPage=MainDetails&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                                        <i class="glyphicon glyphicon-search"></i>
                                                        <span class="btn-fnd-txt">Agent Code</span>
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hAgentCode" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="POLICYHEADER__AGENT" runat="server"></asp:HiddenField>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="POLICYHEADER__BUSINESSTYPE"></asp:PostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div id="liContactName" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:UpdatePanel ID="POLICYHEADER__updPanelContactName" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblWhoCanContacted" Text="<%$ Resources:lblWhoCanContacted %>" AssociatedControlID="POLICYHEADER__CONTACT_NAME" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="POLICYHEADER__CONTACT_NAME" runat="server" CssClass="field-medium form-control" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvWhoCanContact" runat="server" Display="none" ErrorMessage="<%$ Resources:vldRequiredWhoCanContacted %>" ControlToValidate="POLICYHEADER__CONTACT_NAME" SetFocusOnError="true" InitialValue="" Enabled="false"></asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <nexus:ProgressIndicator ID="upContactName" OverlayCssClass="updating" AssociatedUpdatePanelID="POLICYHEADER__updPanelContactName" runat="server">
                                <ProgressTemplate>
                                </ProgressTemplate>
                            </nexus:ProgressIndicator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label runat="server" ID="lblHandler" Text="Account Handler" AssociatedControlID="POLICYHEADER__HANDLER" class="col-md-4 col-sm-3 control-label">
                                       
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__HANDLER" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnHandler" runat="server" SkinID="btnModal" CausesValidation="false" OnClientClick="tb_show(null , '../../Modal/FindAccountHandler.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;">
                                               <i class="glyphicon glyphicon-search"></i>
                                               <span class="btn-fnd-txt">Account Handler</span>
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <asp:HiddenField ID="hiddenHandlerCode" runat="server"></asp:HiddenField>
                        </div>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:UpdatePanel ID="POLICYHEADER__UpdatePanelCurrency" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="POLICYHEADER__CURRENCY" Text="Currency" class="col-md-4 col-sm-3 control-label">
                                    </asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="POLICYHEADER__CURRENCY" runat="server" CssClass="field-mandatory form-control">
                                        </asp:DropDownList>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BRANCH" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblRegarding" runat="server" AssociatedControlID="POLICYHEADER__REGARDING" Text="Regarding" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="POLICYHEADER__REGARDING" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblSector" runat="server" AssociatedControlID="ddlSector" Text="Sector" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlSector" runat="server" CssClass="field-medium form-control">
                                    <asp:ListItem>(Please Select)</asp:ListItem>
                                    <asp:ListItem>Urban</asp:ListItem>
                                    <asp:ListItem>Rural</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblFrequency" runat="server" AssociatedControlID="POLICYHEADER__FREQUENCY" Text="Frequency" class="col-md-4 col-sm-3 control-label">
                                          
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="POLICYHEADER__FREQUENCY" runat="server" CssClass="field-mandatory form-control" AutoPostBack="true">
                                    <asp:ListItem Text="Annual" Value="ANNUAL"></asp:ListItem>
                                    <asp:ListItem Text="Monthly" Value="MONTH"></asp:ListItem>
                                    <asp:ListItem Text="Quaterly" Value="QUARTER"></asp:ListItem>
                                    <asp:ListItem Text="BiAnnual" Value="BIANNUAL"></asp:ListItem>
                                    <asp:ListItem Text="24 Motnhs" Value="TWYEAR"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <%--  <NexusProvider:LookupList ID="POLICYHEADER__FREQUENCY" runat="server" DataItemValue="Code"
                                            DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Renewal_Frequency"
                                            CssClass="field-medium" />--%>
                            <asp:RequiredFieldValidator ID="vldrqdFrequency" runat="server" ControlToValidate="POLICYHEADER__FREQUENCY" ErrorMessage="Error in Frequency" Display="None" SetFocusOnError="true" Enabled="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblRenewalMethod" runat="server" AssociatedControlID="POLICYHEADER__RENEWALMETHOD" Text="Renewal Method" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__RENEWALMETHOD" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Renewal_Method" DefaultText="(Please Select)" CssClass="field-medium form-control"></NexusProvider:LookupList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblLTUExpiry" runat="server" AssociatedControlID="POLICYHEADER__LTUEXPIRYDATE" Text="LTU Expiry Date" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__LTUEXPIRYDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calLTUExpiry" runat="server" LinkedControl="POLICYHEADER__LTUEXPIRYDATE" HLevel="1"></uc1:CalendarLookup>
                                </div>
                            </div>

                            <%-- <asp:RegularExpressionValidator ID="regexvldLTUExpiryDate" runat="server" ControlToValidate="POLICYHEADER__LTUEXPIRYDATE"
                                            Display="None" ErrorMessage="Invalid LTU Expiry Date" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d"
                                            SetFocusOnError="true" />--%>
                            <asp:CompareValidator ID="cmpLTUExpiryDate" Type="Date" ControlToValidate="POLICYHEADER__LTUEXPIRYDATE" Operator="DataTypeCheck" runat="server" Display="None" ErrorMessage="Invalid LTU Expiry Date"></asp:CompareValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblLapseCancelReason" runat="server" AssociatedControlID="POLICYHEADER__LAPSECANCELREASON" Text="Lapse/Cancel Reason" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__LAPSECANCELREASON" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="lapsed_reason" CssClass="field-medium form-control" DefaultText="(Please Select)"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdLapseCancelReason" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELREASON" ErrorMessage="Lapse/Cancel reason not specified" Display="None" SetFocusOnError="true" Enabled="false" ValidationGroup="grpLapseQuote"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblLapseCancelDate" runat="server" Text="Lapse/Cancel Date" AssociatedControlID="POLICYHEADER__LAPSECANCELDATE" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__LAPSECANCELDATE" runat="server" CssClass="field form-control"></asp:TextBox><uc1:CalendarLookup ID="calLapseDate" runat="server" LinkedControl="POLICYHEADER__LAPSECANCELDATE" HLevel="1"></uc1:CalendarLookup>
                                </div>
                            </div>

                            <asp:RequiredFieldValidator ID="vldrqdLapseCancelDate" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELDATE" ErrorMessage="Lapse/Cancel date not specified" Display="None" SetFocusOnError="true" Enabled="false" ValidationGroup="grpLapseQuote"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rngLapseCancelDate" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELDATE" Display="None" ErrorMessage="Please enter a date between 01/01/1900 and 31/12/9998" SetFocusOnError="true" Type="Date" MaximumValue="31/12/9998" MinimumValue="01/01/1900" ValidationGroup="grpLapseQuote"></asp:RangeValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblPOLICYHEADER_CORRESPONDENCETYPE" runat="server" AssociatedControlID="POLICYHEADER__CORRESPONDENCETYPE" Text="Client Correspondence" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__CORRESPONDENCETYPE" runat="server" DataItemValue="Code" DefaultText="(Please Select)" DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Correspondence_Type" onchange='onValidate_POLICYHEADER__CORRESPONDENCETYPE(null, null, this);' CssClass=" form-control" AutoPostBack="True" ParentFieldName="" ParentLookupListID="" Value="" data-type="List"></NexusProvider:LookupList>
                                <asp:TextBox ID="POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE" runat="server" CssClass="form-control m-t-xs" data-type="Text" Visible="false" Enabled="false"></asp:TextBox>
                            </div>
                            <asp:HiddenField ID="POLICYHEADER__DEFAULTCORRESPONDENCECODE" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE" runat="server"></asp:HiddenField>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblTimesRenewed" runat="server" AssociatedControlID="lblRenewedTimes" Text="Times Renewed" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <p class="form-control-static font-bold">
                                    <asp:Label ID="lblRenewedTimes" runat="server"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblReferredAtRenewal" runat="server" AssociatedControlID="POLICYHEADER__ReferredAtRenewal" Text="Referred at renewal ?" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox ID="POLICYHEADER__REFERREDATRENEWAL" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblReferredAtMTA" runat="server" AssociatedControlID="POLICYHEADER__ReferredAtMTA" Text="Referred at MTA ?" class="col-md-4 col-sm-3 control-label">
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox ID="POLICYHEADER__REFERREDATMTA" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                    <legend>Endorsement</legend>
                    <NexusControl:StandardWording ID="FAMILYDETAILS__ENDORSEMENT" runat="server" SupportRiskLevel="false" AllowEdit="true"></NexusControl:StandardWording>
                    <asp:Panel ID="POLICYHEADER__COVERDATESPANEL" runat="server">
                        <legend>
                            <asp:Label ID="lblCoverDateHeading" runat="server" Text="Cover Dates"></asp:Label>
                        </legend>
                        <div class="form-horizontal">
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCoverStartDate" runat="server" AssociatedControlID="POLICYHEADER__COVERSTARTDATE" Text="Cover Start Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="POLICYHEADER__COVERSTARTDATE" runat="server" CssClass="field-mandatory form-control" onblur="fillDate('<%=iMode%>')"></asp:TextBox><uc1:CalendarLookup ID="calCoverFromDate" runat="server" LinkedControl="POLICYHEADER__COVERSTARTDATE" HLevel="2"></uc1:CalendarLookup>
                                    </div>
                                </div>

                                <asp:RequiredFieldValidator ID="vldrqdCoverFromDate" runat="server" Display="none" ErrorMessage="Invalid Cover Start Date" ControlToValidate="POLICYHEADER__COVERSTARTDATE" Enabled="true" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="custFromDate" runat="server" Display="none" ControlToValidate="POLICYHEADER__COVERSTARTDATE" SetFocusOnError="true" OnServerValidate="custFromDate_ServerValidate"></asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="regexvldFromDate" runat="server" ControlToValidate="POLICYHEADER__COVERSTARTDATE" Display="None" ErrorMessage="Invalid Cover Start Date" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                                <asp:HiddenField ID="hiddenGracePeriod" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenOptionSetting" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenCoverFromDate" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hiddenMidnightRenewalSettings" runat="server"></asp:HiddenField>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lbl_CoverEndDate" runat="server" AssociatedControlID="POLICYHEADER__COVERENDDATE" Text="Cover End Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="POLICYHEADER__COVERENDDATE" runat="server" CssClass="field-mandatory form-control" onblur="fillDateRenewal()"></asp:TextBox><uc1:CalendarLookup ID="calCoverEndDate" runat="server" LinkedControl="POLICYHEADER__COVERENDDATE" HLevel="2"></uc1:CalendarLookup>
                                    </div>
                                </div>

                                <asp:RequiredFieldValidator ID="vldrqdCoverEndDate" runat="server" Display="none" ErrorMessage="Invalid Cover End Date.. Cover End Date can not be greater than #CoverEndDate" ControlToValidate="POLICYHEADER__COVERENDDATE" Enabled="true" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regexvldEndDate" runat="server" ControlToValidate="POLICYHEADER__COVERENDDATE" Display="None" ErrorMessage="Invalid End Date Format" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="custToDate" runat="server" Display="none" ControlToValidate="POLICYHEADER__COVERENDDATE" SetFocusOnError="true" OnServerValidate="custToDate_ServerValidate"></asp:CustomValidator>
                                <asp:CompareValidator ID="compvldCoverEndDate" runat="server" ControlToCompare="POLICYHEADER__COVERSTARTDATE" Display="None" ErrorMessage="Invalid End Date" ControlToValidate="POLICYHEADER__COVERENDDATE" SetFocusOnError="true" Operator="GreaterThan" Type="Date" Enabled="True"></asp:CompareValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblInception" runat="server" AssociatedControlID="POLICYHEADER__INCEPTION" Text="Inception Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="POLICYHEADER__INCEPTION" runat="server" CssClass="field-date form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblRenewal" runat="server" AssociatedControlID="POLICYHEADER__RENEWAL" Text="Renewal Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="POLICYHEADER__RENEWAL" runat="server" CssClass="field-date form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblInceptionTPI" runat="server" AssociatedControlID="POLICYHEADER__INCEPTIONTPI" Text="Inception TPI Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="POLICYHEADER__INCEPTIONTPI" runat="server" CssClass="field-date form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblProposalDate" runat="server" AssociatedControlID="POLICYHEADER__PROPOSALDATE" Text="Proposal Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="POLICYHEADER__PROPOSALDATE" runat="server" CssClass="field-date form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblQuoteExpiryDate" runat="server" AssociatedControlID="POLICYHEADER__QUOTEEXPIRYDATE" Text="Quote Expiry Date" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="POLICYHEADER__QUOTEEXPIRYDATE" runat="server" CssClass="field-date form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </asp:Panel>
                    <asp:Panel ID="POLICYHEADER__COVERNOTEPANEL" runat="server">
                        <legend>
                            <asp:Label ID="lblHeading3" runat="server" Text="Cover Note Details"></asp:Label>
                        </legend>
                        <div class="form-horizontal">
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblIsCoverNote" runat="server" AssociatedControlID="chkIsCoverNoteUsed" Text="Is Cover Note Used ?" __designer:wfdid="w34" class="col-md-4 col-sm-3 control-label">
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:CheckBox ID="chkIsCoverNoteUsed" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsCoverNoteUsed_CheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCoverNoteBookNo" runat="server" AssociatedControlID="POLICYHEADER__COVERNOTEBOOKNO" Text="Cover Note Book No" class="col-md-4 col-sm-3 control-label">
                                                        
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="POLICYHEADER__COVERNOTEBOOKNO" AutoPostBack="true" runat="server" CssClass="field-medium form-control" OnSelectedIndexChanged="POLICYHEADER__COVERNOTEBOOKNO_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <asp:TextBox ID="txtCoverNoteBookNo" runat="server" Visible="false" CssClass="field-medium" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCoverNoteNumber" runat="server" AssociatedControlID="POLICYHEADER__COVERNOTESHEETNO" Text="Cover Note Sheet Number" __designer:wfdid="w36" class="col-md-4 col-sm-3 control-label">
                                                        
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="POLICYHEADER__COVERNOTESHEETNO" runat="server" CssClass="field-medium form-control" __designer:wfdid="w37">
                                    </asp:DropDownList>
                                </div>
                                <%-- <asp:RequiredFieldValidator ID="vldrqdCoverNoteSheetNumber" runat="server" ErrorMessage="Please Select the Cover Note Sheet No"
                                                SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="POLICYHEADER__COVERNOTESHEETNO"
                                                InitialValue="(Please Select)" Visible="true"></asp:RequiredFieldValidator>--%>
                                <asp:TextBox ID="txtCoverNoteSheetNo" runat="server" Visible="false" CssClass="field-medium" ReadOnly="true"></asp:TextBox>
                            </div>
                            <%-- <li>
                                            <asp:Label ID="lblPlaceOfIssue" runat="server" AssociatedControlID="txtPlaceOfIssue"
                                                Text="Place Of Issue" __designer:wfdid="w42">
                                            </asp:Label>
                                            <asp:TextBox ID="txtPlaceOfIssue" runat="server" CssClass="field-medium" Enabled="true"></asp:TextBox>
                                          <asp:RequiredFieldValidator ID="vldRqdPlaceOfIssue" runat="server" ErrorMessage="invalid place of the issue"
                                                SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="txtPlaceOfIssue"
                                                __designer:wfdid="w44"></asp:RequiredFieldValidator>
                                        </li>--%>
                            <%-- <li>
                                            <asp:Label ID="lblCoverNoteIssuedDate" runat="server" AssociatedControlID="txtCoverNoteIssuedDate"
                                                Text="Cover Note Issued Date">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCoverNoteIssuedDate" runat="server" CssClass="field-date"  Enabled="true"></asp:TextBox>
                                            <uc1:CalendarLookup ID="calCoverNoteIssuedDate" runat="server" HLevel="2" LinkedControl="txtCoverNoteIssuedDate">
                                            </uc1:CalendarLookup>
                                            <asp:CustomValidator ID="VldCoverNoteIsueDate" runat="server" Display="None" ErrorMessage="Invalid Cover Note Issue Date"
                                                OnServerValidate="VldCoverNoteIsueDate_ServerValidate"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="vldrqdCoverNoteIssuedDate" runat="server" ErrorMessage="Invalid Cover Note Issued Date"
                                                SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="txtCoverNoteIssuedDate"></asp:RequiredFieldValidator>
                                        </li>--%>
                        </div>
                    </asp:Panel>
                    <legend>Auto Post Back Functionality</legend>
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblStopReason" runat="server" AssociatedControlID="POLICYHEADER__STOPREASON" Text="Stop Reason" class="col-md-4 col-sm-3 control-label"> </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__STOPREASON" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="renewal_stop_code" CssClass="field-medium form-control" DefaultText="(Please Select)" AutoPostBack="true"></NexusProvider:LookupList>
                            </div>
                        </div>
                    </div>
                    <uc4:SubAgents ID="POLICYHEADER__SUBAGENTS" runat="server"></uc4:SubAgents>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnLapseQuote" runat="server" Text="Lapse Quote" OnClick="btnLapseQuote_Click" Visible="false" ValidationGroup="grpLapseQuote" OnClientClick="return LapseConfirmation();" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="refreshCV" runat="server" Style="display: none" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" OnClientClick="return NextButton();" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
            <asp:CustomValidator ID="cust_VldAgent" runat="server" Display="None" ErrorMessage="Error in Agent Code" ClientValidationFunction="AgentValidation_Client" OnServerValidate="cust_VldAgent_ServerValidate"></asp:CustomValidator>
            <asp:CustomValidator ID="cust_VldCoverSheet" runat="server" Display="None" ErrorMessage="Please Select the Cover Note Sheet No" ClientValidationFunction="CoverSheetValidation_Client" OnServerValidate="cust_VldCoverSheet_ServerValidate"></asp:CustomValidator>
            <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>
</asp:Content>
