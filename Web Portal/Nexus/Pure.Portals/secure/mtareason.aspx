<%@ Page Language="VB" MasterPageFile="~/Default.master" AutoEventWireup="false"
    CodeFile="MtaReason.aspx.vb" Inherits="Nexus.secure_mtareason" Title="MTA Reason" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        var Cancellationreason;
        var SelectedMTAReason;
        
        var cntMainBody = "ctl00_cntMainBody_";

        $(window).load(function() {		
            if (document.all.rdoMTAReasonList != undefined) {
                if (document.all.rdoMTAReasonList[0] != null) {
                    document.all.rdoMTAReasonList[0].focus();
                }
            }

            var bMidTermAdjustmentTemp = "<%=bMidTermAdjustmentTemp %>";			
            var tempConfirmCalenderCtrl = false;
            if(bMidTermAdjustmentTemp)
            {
                $("#ctl00_cntMainBody_txtExpiryDate_uctCalendarLookup_txtExpiryDate_btnCalendar").click(function () {
                    tempConfirmCalenderCtrl = true;
                    return TempConfirmation("<%= GetLocalResourceObject("lbl_WarningMsg") %>");                    
                });

                 $("#ctl00_cntMainBody_txtExpiryDate").click(function () {                    
                     tempConfirmCalenderCtrl = false;
                });

                $("#ctl00_cntMainBody_txtExpiryDate").blur(function () {
                    if (!tempConfirmCalenderCtrl) {
                        tempConfirmCalenderCtrl = true;
                        return TempConfirmation("<%= GetLocalResourceObject("lbl_WarningMsg") %>");                        
                    }
                });
            }
        });
        //this will give us the cancellation code which is in web.config   
        function pageload(SelectedPaymentOption) {
            Cancellationreason = SelectedPaymentOption;
            //alert(Cancellationreason);

        }
        //THIS WILL LOOP THROUGHT THE SELECTED RADIOBUTTON AND CHECK IF CANCELLATION CODE IS 
        //SELECTED IF NOT THEN EDIT CLIENT DETAILS WILL BE VISIBLE.   
        function SelectedRD() {
            var rdoMTAReasonListCount = document.all["rdoMTAReasonList"].length;
            for (var CountVar = 0; CountVar < rdoMTAReasonListCount; CountVar++) {
                if (document.all.rdoMTAReasonList[CountVar].checked) {
                    SelectedMTAReason = document.all.rdoMTAReasonList[CountVar].value;
                    if (document.all.rdoMTAReasonList[CountVar].value == Cancellationreason) {
                        document.getElementById('<%=lnkEditDetails.ClientID%>').disabled = true;
                        //if cancellation is selected effective till should be disabled o.w. enabled
                        if (document.getElementById('<%=txtExpiryDate.ClientID%>') != null) {
                        document.getElementById('<%=txtExpiryDate.ClientID%>').disabled = true;
                    }
                    }
                    else {
                        document.getElementById('<%=lnkEditDetails.ClientID%>').disabled = false;
                        //if cancellation is selected effective till should be disabled o.w. enabled
                        if (document.getElementById('<%=txtExpiryDate.ClientID%>') != null) {
                        document.getElementById('<%=txtExpiryDate.ClientID%>').disabled = false;
                    }
                    }

                }
            }
        }

        function MTAReasonRequired_ClientValidation(source, arguments) {
            //at least one Radio Button should be selected
            var rdoMTAReasonListCount = document.all["rdoMTAReasonList"].length; // count the radio buttons
            if (rdoMTAReasonListCount == null) {//if only one reason is in the list then rdoMTAReasonListCount==null
                if (document.all.rdoMTAReasonList.checked) {
                    //if there is only one and IsSelected then return true
                    arguments.IsValid = true;
                    return;
                }
            }

            //if more than two records then need to check which one is selected using the loop 
            for (var CountVar = 0; CountVar < rdoMTAReasonListCount; CountVar++) {
                if (document.all.rdoMTAReasonList[CountVar].checked) {
                    //if any one is selected then return true
                    arguments.IsValid = true;
                    return;
                }
            }
            //if no one is selected then return false
            arguments.IsValid = false;
        }
        function statementUpdated() {
            tb_remove();
        }

        //Will return date in mm/dd/yyyy format, need to pass date in dd/mm/yyyy format
        function formatDate(value) {
            var dateparts = value.split("/");
            return dateparts[1] + "/" + dateparts[0] + "/" + dateparts[2];
        }
        function ReceiveDescription(Description) {
            document.getElementById('<%=MTADescription.ClientID %>').value = Description;
            __doPostBack(null, 'GetMTADescription');

        }       

        function ConfirmRenewal(bDoNotDeleteRenewalQuoteOnMTA) {
            var sConfirmPolicyUnderRenewal = '<%= sConfirmPolicyUnderRenewal %>';
            var sConfirmPolicyUnderRenewalToDelete = '<%= sConfirmPolicyUnderRenewalToDelete %>';
            if (bDoNotDeleteRenewalQuoteOnMTA == 'True') {
                return confirm(sConfirmPolicyUnderRenewal);
            }
            else {
                return confirm(sConfirmPolicyUnderRenewalToDelete);
            }
        }

        function ValidateMTADateAndRenewal() {
            var sMsg_ConfirmClaimMessageForBackdatedMTA = '<%= sMsg_ConfirmClaimMessageForBackdatedMTA %>';
            var bIsPolicyInAnnualRenewal = '<%= bIsPolicyInAnnualRenewal %>';
            var bIsPolicyInRenewal = '<%= bIsPolicyInRenewal %>';
            var bDoNotDeleteRenewalQuoteOnMTA = '<%= bDoNotDeleteRenewalQuoteOnMTA %>'; //This will depend on newly added system option
            var bDeleteRenQuoteReRunRenewal='<%= bDeleteRenQuoteReRunRenewal%>'
            var sConfirmPolicyUnderRenewal = '<%= sConfirmPolicyUnderRenewal %>';
            var sConfirmPolicyUnderRenewalToDelete = '<%= sConfirmPolicyUnderRenewalToDelete %>';
            var sMsg_ConfirmClaimMessage = '<%= sMsg_ConfirmClaimMessage %>';
            var effectiveDate = formatDate(document.getElementById("ctl00_cntMainBody_txtEffectiveDate").value);
            var MaxClaimDate = formatDate('<%= dt_LastClaimDate %>');
            var strMTADate = formatDate(document.getElementById('<%=txtEffectiveDate.ClientID%>').value);
            var strCoverStartDate = formatDate('<%= dt_coverStartDate %>');
            var dtMTADate = new Date(strMTADate);
            var dt_coverStartDate = new Date(strCoverStartDate);
            var MTAValue, valOfHidden;

            valOfHidden = htmlDecode($("#<%= hdnMtaReasonList.ClientID%>").val());
            var MtaList = $.parseJSON(valOfHidden);

            if (SelectedMTAReason != null && SelectedMTAReason.toUpperCase() == "CANCEL") {
                if (new Date(dtMTADate).toLocaleDateString() <= new Date(MaxClaimDate).toLocaleDateString()) {
                      return confirm(sMsg_ConfirmClaimMessageForBackdatedMTA);
                }
                
            }

            if (dtMTADate >= dt_coverStartDate)   //Not Backdated MTA
            {
                if (effectiveDate <= MaxClaimDate && SelectedMTAReason == Cancellationreason) {
                    return confirm(sMsg_ConfirmClaimMessage);
                }
            }

            var bResultRenewal = true;
            if (CheckValidation() == true) {
                if (bDoNotDeleteRenewalQuoteOnMTA == 'True' || bDeleteRenQuoteReRunRenewal == 'True') {

                    if (bDoNotDeleteRenewalQuoteOnMTA == 'True') {
                        if (bIsPolicyInAnnualRenewal == 'True' && SelectedMTAReason != Cancellationreason) {
                            bResultRenewal = confirm(sConfirmPolicyUnderRenewal);
                        }
                        else if (bIsPolicyInRenewal == 'True') {
                            bResultRenewal = confirm(sConfirmPolicyUnderRenewalToDelete);
                        }
                    }
                }
                else {
                    if (bIsPolicyInRenewal == 'True') {
                        bResultRenewal = confirm(sConfirmPolicyUnderRenewalToDelete);
                    }
                }

                //Check For Other
                if (bResultRenewal == true) {
                    MTAValue = MtaValueMethod(MtaList, SelectedMTAReason);
                    if (MTAValue == "1") {
                        tb_show(null, '../modal/MTAReasonDesc.aspx?modal=true&height="100"', null);
                        return false;
                    }
                }
                else { return false; }
                //Check For Other
            }
            else {
                return false;
            }
        }

        function CheckValidation() {
            var sPolicyStatus = '<%= sPolicyStatus %>';
            var sMsg_MTAonLapsedConfirmation = '<%= sMsg_MTAonLapsedConfirmation %>';
            var sMsg_TempMTA = '<%= sTempMTAMessage %>';
            if (document.getElementById("ctl00_cntMainBody_txtEffectiveDate") != undefined) {
            if (document.getElementById("ctl00_cntMainBody_txtEffectiveDate").value == document.getElementById("ctl00_cntMainBody_txtHiddenShortDateFormat").value) {
                ValidatorEnable(document.getElementById("ctl00_cntMainBody_rangevldDateFrom"), false);
                ValidatorEnable(document.getElementById("ctl00_cntMainBody_cmpVldEffectiveDate"), false);
                return false;
            }
            else {
                    //Check if MTA is temporary
                    var bIsTempMTA = false;
                    if (document.getElementById("ctl00_cntMainBody_txtExpiryDate") != undefined) {
                        if (document.getElementById("ctl00_cntMainBody_txtExpiryDate").value.length > 0) {
                            bIsTempMTA = true;
            }
                    }

                    if (bIsTempMTA == true) {
                        ValidatorEnable(document.getElementById("ctl00_cntMainBody_RangevldExpiryDate"), true);
                        Page_ClientValidate();
                        if (Page_IsValid == true) {
                            alert(sMsg_TempMTA);
                            return true;
        }
                        else {
                            return false;
                        }
                    }
                    else {
                        Page_ClientValidate();
                        if (Page_IsValid == true) {
                            var bMTAonLapsedPolicyResult = true;
                            //if doing MTA on lapsed policy, then need to collect confirmation response from user
                            if (sPolicyStatus == 'LAP') {
                                bMTAonLapsedPolicyResult = confirm(sMsg_MTAonLapsedConfirmation);
                            }
        
                            if (bMTAonLapsedPolicyResult == true) {
                                return validateBackDatedMTA();
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            return false;
                        }
                    }
                }
            }
        }
        var showMsg = true;      

        function TempConfirmation(warningMsg) {
            if (document.getElementById("ctl00_cntMainBody_txtExpiryDate").value.length > 0 && showMsg) {
                alert(warningMsg);
                //set min date for rangevldDateFrom to be policy start date
                var strPolicyStartDate = formatDate('<%= dt_PolicyStartDate %>');
                document.getElementById("ctl00_cntMainBody_rangevldDateFrom").minimumvalue = strPolicyStartDate;
                showMsg = false;                                    
            }
            else {
                showMsg = true;
                var strMinMTADate = formatDate('<%= dt_MinMTADate %>');
                document.getElementById("ctl00_cntMainBody_rangevldDateFrom").minimumvalue = strMinMTADate;
            }
        }        
        function validateBackDatedMTA() {
            //convert date in mm/dd/yyyy format because javascript Date function does not works for dd/mm/yyyy
            var strMTADate = formatDate(document.getElementById('<%=txtEffectiveDate.ClientID%>').value);
            var strCoverStartDate = formatDate('<%= dt_coverStartDate %>');
            var dtMTADate = new Date(strMTADate);
            var dt_coverStartDate = new Date(strCoverStartDate);
            var sMsg_BackDatedConfirmation = '<%= sMsg_BackDatedConfirmation %>';
            var sMTAReasonForCancelation = '<%= sMTAReasonForCancelation %>';
            var bAllowBackDatedMTAForProduct = '<%= bAllowBackDatedMTAForProduct %>';
            var bAllowBackDatedMTCForProduct = '<%= bAllowBackDatedMTCForProduct %>';
            var sMsg_BackDatedMTAForProduct = '<%= sMsg_BackDatedMTAForProduct %>';
            var sMsg_BackDatedMTCForProduct = '<%= sMsg_BackDatedMTCForProduct %>';
            var sMsg_ConfirmClaimMessageForBackdatedMTA = '<%= sMsg_ConfirmClaimMessageForBackdatedMTA %>';
            var bHasOOSPendingVersions = '<%= bHasOOSPendingVersions %>';
            var sMsg_ConfirmSaveOOSMTAQuoteMessage = '<%= sMsg_ConfirmSaveOOSMTAQuoteMessage%>';
           
            var dt_MaxClaimDate = formatDate('<%= dt_LastClaimDate %>');
            dt_MaxClaimDate = new Date(dt_MaxClaimDate);
            Page_ClientValidate();

            if (Page_IsValid == true) {
                if (dtMTADate < dt_coverStartDate)   //Backdated MTA
                {
                    if (SelectedMTAReason == Cancellationreason) {
                        if (bAllowBackDatedMTCForProduct == 'False' && bAllowBackDatedMTAForProduct == 'False') {
                            alert(sMsg_BackDatedMTCForProduct);
                            return false;
                        }
                    }
                    else {
                        if (bAllowBackDatedMTAForProduct == 'False') {
                            alert(sMsg_BackDatedMTAForProduct);
                            return false;
                        }
                    }

                    if (dtMTADate <= dt_MaxClaimDate) {
                        var bWantToContinueWithClaim = confirm(sMsg_ConfirmClaimMessageForBackdatedMTA);
                        if (bWantToContinueWithClaim == true) {
                            return confirm(sMsg_BackDatedConfirmation);
                                }
                                else {
                            return false;
                                }
                            }
                            else {
                        if (confirm(sMsg_BackDatedConfirmation) == true) {
                            if (bHasOOSPendingVersions == 'True') {
                                if (confirm(sMsg_ConfirmSaveOOSMTAQuoteMessage) == false) {
                                return false;
                            }
                                else {
                                    return true;
                        }
                            }
                        else {
                                return true;
                        }
                    }
                    else {
                                return false;
                            }
                    }
                }
                            else {
                                return true;
                            }
                        }
                        else {
                            return false;
                        }
                    }

        function validateBackDatedReinstatement() {
            var strHighestPolicyStartDate = new Date(formatDate('<%= dt_LastCancelledDate %>'));
            var strReinstatementDate = new Date(formatDate(document.getElementById('<%=txtReinstatementDate.ClientID%>').value));
            var hfReinstatementBackdatedIndicator = $('#<%= hfReinstatementBackdatedIndicator.ClientID %>');
            var sMsg_ConfirmSaveOOSMTAQuoteMessage = '<%= sMsg_ConfirmSaveOOSMTAQuoteMessage%>';
            var bHasOOSPendingVersions = '<%= bHasOOSPendingVersions %>';
            
            if (strHighestPolicyStartDate > strReinstatementDate) {
                var result = confirm('<%= GetLocalResourceObject("msg_ConfirmOOSReinstatement") %>');
                if (result) {
                    hfReinstatementBackdatedIndicator.val('True'); 
                    if (bHasOOSPendingVersions == 'True') {
                        if (confirm(sMsg_ConfirmSaveOOSMTAQuoteMessage) == false) {
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                    else {
                        return true;
                    }                    
                }
                else {
                    return false;
                }
            }
            else {
                hfReinstatementBackdatedIndicator.val('False');
                return true;
        }
        }
        function MtaValueMethod(table, SelectedMTAReason) {
            var MtaValue;
            $.each(table, function (key, value) {
                var key = key
                if (key == SelectedMTAReason) {
                    MtaValue = value
                    return false
                }
            });
            return MtaValue;
        }

        function htmlDecode(value) {
            if (value) {
                return $('<div/>').html(value).text();
            }
        }
        
        <%--function ConfirmRenewal() {
            
            var sConfirmPolicyUnderRenewal = '<%= sConfirmPolicyUnderRenewal %>';
            return confirm(sConfirmPolicyUnderRenewal);

        }

        function ValidateMTADateAndRenewal() {
            var bAskForRenewalConfirmation = '<%= bAskForRenewalConfirmation %>';
            if (CheckValidation() == true) {
                if (bAskForRenewalConfirmation == 'True') {
                    return ConfirmRenewal();
                }
            }
			else
			{
			return false;
			}
        }--%>
        
    </script>

   <div id="secure_mtareason">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPageInfo" runat="server" Text="<%$ Resources:lbl_PageHeading %>"></asp:Label></h1>
                    </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive">
                            <asp:GridView ID="GridMTAReasons" runat="server" AutoGenerateColumns="False" Caption="<%$ Resources:lbl_MTAReason_Heading %>">
                                <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_PaymentTable_Col1Heading %>">
                                        <ItemTemplate>
                                    <asp:Literal ID="literal" runat="server" Text='<%#Eval("Column1")%>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_PaymentTable_Col2Heading %>">
                                        <ItemTemplate>
                                    <span class="asp-radio">
                                        <input type="radio" id="rdoMTAReasonList" onclick="SelectedRD();" name="rdoMTAReasonList" value='<%#Eval("Column2")%>' <%#Eval("column3")%> />
                                        <label name="rdoMTAReasonList"></label>
                                    </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                    <asp:HiddenField ID="MTADescription" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfReinstatementBackdatedIndicator" runat="server" Value="False"></asp:HiddenField>
                </div>
                <div class="form-horizontal">
                    <asp:CustomValidator ID="CustVldMTAReasonRequired" runat="server" ErrorMessage="<%$ Resources:lbl_MTAReason %>" ClientValidationFunction="MTAReasonRequired_ClientValidation" SetFocusOnError="true" Display="None" ValidationGroup="MTAReasonGroup"></asp:CustomValidator>
                    <asp:CustomValidator ID="custvldPolicyCancellationOnLivePlan" runat="server" ErrorMessage="<%$ Resources:msg_PolicyCancellationValidationOnLivePlan %>" SetFocusOnError="true" Display="none" ValidationGroup="MTAReasonGroup"></asp:CustomValidator>
                    <asp:Panel ID="PnlMTADateFrom" runat="server">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="txtEffectiveDate" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="lbl_DateFromMesssage" runat="server" Text="<%$ Resources:lbl_DateFromMessage %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="txtEffectiveDate_uctCalendarLookup" runat="server" LinkedControl="txtEffectiveDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                        </div>
                            </div>
                        <asp:RequiredFieldValidator ID="vldDateFrom" Display="None" ControlToValidate="txtEffectiveDate" runat="server" ErrorMessage="<%$ Resources:lbl_EnterEffectiveDate %>" SetFocusOnError="True" ValidationGroup="MTAReasonGroup"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rangevldDateFrom" runat="server" Display="None" ControlToValidate="txtEffectiveDate" SetFocusOnError="true" ValidationGroup="MTAReasonGroup" Type="Date" MaximumValue="12/12/2099" MinimumValue="01/01/1900"></asp:RangeValidator>
                        <asp:CompareValidator ID="cmpVldEffectiveDate" runat="server" ErrorMessage="<%$ Resources:lbl_InvalidEffectiveDateFormat %>" ControlToValidate="txtEffectiveDate" Type="Date" Display="None" Operator="DataTypeCheck" ValidationGroup="MTAReasonGroup" SetFocusOnError="true"></asp:CompareValidator><asp:CustomValidator ID="custvldDateFrom" Display="None" ControlToValidate="txtEffectiveDate" runat="server" SetFocusOnError="true" ValidationGroup="MTAReasonGroup"></asp:CustomValidator>
                                
                                </asp:Panel>
                                <asp:Panel ID="PnlMTADateTo" runat="server">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblToDate" runat="server" AssociatedControlID="txtExpiryDate" CssClass="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="lbl_DateToMesssage" runat="server" Text="<%$ Resources:lbl_DateToMessage %>"></asp:Literal>
                            </asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtExpiryDate" ValidationGroup="MTAReasonGroup" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="txtExpiryDate_uctCalendarLookup" runat="server" LinkedControl="txtExpiryDate" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>
                    </div>
                        <asp:RangeValidator ID="RangevldExpiryDate" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_InvalidExpiryDate %>" ControlToValidate="txtExpiryDate" SetFocusOnError="True" Enabled="false" ValidationGroup="MTAReasonGroup" Type="Date" MaximumValue="12/12/2099" MinimumValue="01/01/1900"></asp:RangeValidator>
                        <asp:CustomValidator ID="CustvldExpiryDate" runat="server" ControlToValidate="txtExpiryDate" Display="None" ValidationGroup="MTAReasonGroup"></asp:CustomValidator><br>
                        <asp:CompareValidator ID="cmpVldExpiryDate" runat="server" ErrorMessage="<%$ Resources:lbl_InvalidEffectiveDateFormat %>" ControlToValidate="txtExpiryDate" Enabled="True" Type="Date" SetFocusOnError="true" Operator="DataTypeCheck" ValidationGroup="MTAReasonGroup" Display="None"></asp:CompareValidator><asp:CompareValidator ID="cmpVldExpiryDate1" runat="server" ErrorMessage="<%$ Resources:lbl_ValidExpriryDate %>" ControlToCompare="txtEffectiveDate" ControlToValidate="txtExpiryDate" Enabled="True" Type="Date" SetFocusOnError="true" Operator="GreaterThanEqual" ValidationGroup="MTAReasonGroup" Display="None"></asp:CompareValidator><p>
                            <asp:Literal ID="lbl_MTATypeMessage" runat="server" Text="<%$Resources:lbl_MTATypeMessage %>"></asp:Literal>
                            <asp:Literal ID="lbl_MTACancellationMessage" runat="server" Text="<%$Resources:lbl_MTACancellationMessage %>"></asp:Literal>
                    </asp:Panel>
                    <asp:Panel ID="pnlReinstatementDate" runat="server">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lbl_ReinstatementDate" runat="server" AssociatedControlID="txtReinstatementDate" Text="<%$ Resources:lbl_ReinstatementDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="txtReinstatementDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="txtReinstatementDate_uctCal" runat="server" LinkedControl="txtReinstatementDate" HLevel="2"></uc1:CalendarLookup>
                        </div>
                    </div>
                </div>
                        <asp:RequiredFieldValidator ID="reqReinstatementDate" runat="server" Display="None" ControlToValidate="txtReinstatementDate" ErrorMessage="<%$Resources:lbl_MTAReinstatementMandatoryDate %>" ValidationGroup="MTAReasonGroup" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpVldReinstatementDate" runat="server" ErrorMessage="<%$ Resources:lbl_MTAReinstatementInvalidDate %>" ControlToValidate="txtReinstatementDate" Enabled="false" Type="Date" SetFocusOnError="true" Operator="DataTypeCheck" ValidationGroup="MTAReasonGroup" Display="None"></asp:CompareValidator><asp:CustomValidator ID="custvldReinstatementDate" runat="server" ControlToValidate="txtReinstatementDate" Display="None" ValidationGroup="MTAReasonGroup" Enabled="false"></asp:CustomValidator><br>
                    </asp:Panel>
                    </div>
                </div>
            <div class="card-footer">
                <asp:Literal ID="lblBtnMessage" runat="server" Text="<%$ Resources:lblBtnMessage %>"></asp:Literal>
                <asp:LinkButton ID="lnkEditDetails" runat="server" Text="<%$ Resources:lbl_Edit %>" ValidationGroup="MTAReasonGroup" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btnsubmit %>" ValidationGroup="MTAReasonGroup" SkinID="btnPrimary"></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hdnMtaReasonList" runat="server" />
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="MTAReasonGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:TextBox ID="txtHiddenShortDateFormat" runat="server" Style="visibility: hidden"></asp:TextBox>
         <asp:HiddenField ID="Hdnrenwalflag" runat="server"  />
    </div>



</asp:Content>
