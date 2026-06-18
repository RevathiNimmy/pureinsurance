<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="RatingSectionDetails.aspx.vb" Inherits="Nexus.Modal_RatingSectionDetails"
    Title="Rating Section Details" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

   <script type="text/javascript">
       
        function CalculateThisPremium() {

            var dthispremium2
            var dcurrencyrate = document.getElementById('<%= txtCurrencyRate.ClientID %>').value.replace(",", "").replace(",", "").replace(",", "");
            if (dcurrencyrate > 0) {

                dthispremium2 = document.getElementById('<%= txtThisPremium2.ClientID %>').value;

                // Round up to zero decimals if suppress decimal configuration is ON
                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {
                    document.getElementById('<%= txtthisPremium.ClientID %>').value = (dthispremium2 / dcurrencyrate).toFixed();
                }
                else {
                    document.getElementById('<%= txtthisPremium.ClientID %>').value = (dthispremium2 / dcurrencyrate).toFixed(2);
                }


            }


        }
        function formatFields() {

            var varSI = parseFloat(cleanNumber($("#<%=txtSumInsured.ClientID%>").val()));
            if (varSI == '') {
                $("#<%=txtSumInsured.ClientID%>").val('0.00');
            }

            var varRate = parseFloat(cleanNumber($("#<%=txtRate.ClientID%>").val()));
            if (varRate == '') {
                $("#<%=txtRate.ClientID%>").val('0.00');
            }
            $("#<%=txtAnnualPremium.ClientID%>").val(parseFloat(cleanNumber($("#<%=txtAnnualPremium.ClientID%>").val())));
            if (document.getElementById('<%= txtthisPremium.ClientID %>').value != '') {
            $("#<%=txtThisPremium.ClientID%>").val(parseFloat(cleanNumber($("#<%=txtThisPremium.ClientID%>").val())));
            }
            $("#<%=txtAnnualPremium.ClientID%>").blur();
            $("#<%=txtThisPremium.ClientID%>").blur();
            var thisPremium2 = document.getElementById("<%=txtThisPremium2.ClientID %>");
            if (thisPremium2 != null) {
                $("#<%=txtThisPremium2.ClientID%>").val(parseFloat(cleanNumber($("#<%=txtThisPremium2.ClientID%>").val())));
                $("#<%=txtAnnualPremium2.ClientID%>").val(parseFloat(cleanNumber($("#<%=txtAnnualPremium2.ClientID%>").val())));

            }
        }

        $(document).ready(function() {
            $("#<%=txtSumInsured.ClientID%>").blur(function() {
                formatFields();
            });
            $("#<%=txtRate.ClientID%>").blur(function() {
                formatFields();
            });
            $("#<%=ddlRateType.ClientID %>").change(function() {
                removeNumricFormatting();
                formatFields();
            });
            $("#<%=ddlRatingSectionType.ClientID %>").change(function() {
                removeNumricFormatting();
                formatFields();
            });

            $("#<%=txtSumInsured.ClientID %>").keypress(function() {
                var var_sumInsured = $(this).val();
                if (var_sumInsured > 99999999999.99) {
                    return false;
                }
            });
            $("#<%=txtRate.ClientID %>").keypress(function() {
                var var_sumInsured = $(this).val();

                if (var_sumInsured > 99999999999.99) {
                    return false;
                }
            });

        });

        function EnableSelect() {
            removeNumricFormatting();
            if (Page_IsValid) {
                document.getElementById('<%= txtSumInsured.ClientID %>').disabled = false;
                document.getElementById('<%= txtAnnualPremium.ClientID %>').disabled = false;
                document.getElementById('<%= txtThisPremium.ClientID %>').disabled = false;
                document.getElementById('<%= ddlRateType.ClientID %>').disabled = false;
                document.getElementById('<%= txtRate.ClientID %>').disabled = false;
                document.getElementById('<%= ddlEarningPattern.ClientID %>').disabled = false;

            }
        }

        function isNumeric(e) {

            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var Value = document.getElementById('<%= txtThisPremium.ClientID %>').value;
            if ((Value.indexOf(".") >= 0 && keychar == "."))
                return false;
            else {
                var ValidChars = "0123456789.";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
            }

        }
            $(document).ready(function() {
                $(".e-num3").each(function() {
                    $(this).keypress(function(e) {
                        var decimal = $.data(this, "numeric.decimal");
                        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                        // allow enter/return key (only when in an input box)
                        if (key == 13 && this.nodeName.toLowerCase() == "input") {
                            return true;
                        }
                        else if (key == 13) {
                            return false;
                        }
                        var allow = false;
                        // allow Ctrl+A
                        if ((e.ctrlKey && key == 97 /* firefox */) || (e.ctrlKey && key == 65) /* opera */) return true;
                        // allow Ctrl+X (cut)
                        if ((e.ctrlKey && key == 120 /* firefox */) || (e.ctrlKey && key == 88) /* opera */) return true;
                        // allow Ctrl+C (copy)
                        if ((e.ctrlKey && key == 99 /* firefox */) || (e.ctrlKey && key == 67) /* opera */) return true;
                        // allow Ctrl+Z (undo)
                        if ((e.ctrlKey && key == 122 /* firefox */) || (e.ctrlKey && key == 90) /* opera */) return true;
                        // allow or deny Ctrl+V (paste), Shift+Ins
                        if ((e.ctrlKey && key == 118 /* firefox */) || (e.ctrlKey && key == 86) || (e.shiftKey && key == 45)) return true;
                        // if a number was not pressed
                        if (key < 48 || key > 57) {
                            /* '-' only allowed at start */
                            //if (key == 45 && this.value.length == 0) return true;
                            /* '-' not allowed at all */

                            //if (key == 45) return f;
                            /* only one decimal separator allowed */
                            if (decimal && key == decimal.charCodeAt(0) && this.value.indexOf(decimal) != -1) {
                                allow = false;
                            }
                            // check for other keys that have special purposes
                            if (key != 8 /* backspace */ && 
                                key != 9 /* tab */ &&
			                    key != 13 /* enter */ &&
			                    key != 35 /* end */ &&
			                    key != 36 /* home */ &&
			                    key != 37 /* left */ &&
			                    key != 39 /* right */ &&
			                    key != 46 /* del */

		                    ) {
                                allow = false;
                            }
                            else {
                                // for detecting special keys (listed above)
                                // IE does not support 'charCode' and ignores them in keypress anyway
                                if (typeof e.charCode != "undefined") {
                                    // special keys have 'keyCode' and 'which' the same (e.g. backspace)
                                    if (e.keyCode == e.which && e.which != 0) {
                                        allow = true;
                                        // . and delete share the same code, don't allow . (will be set to true later if it is the decimal point)
                                        if (e.which == 46) allow = false;
                                    }
                                    // or keyCode != 0 and 'charCode'/'which' = 0
                                    else if (e.keyCode != 0 && e.charCode == 0 && e.which == 0) {
                                        allow = true;
                                    }
                                }
                            }
                            var minus = '-';
                            var txtval = this.value
                            if (key == 45 && txtval == 0) {
                                this.value = '';
                            }
                            if (key == 45 && (this.value == '')) {

                                allow = true;
                            }
                            if (key == 46 && txtval == 0) {

                                allow = true
                            }

                            if (key == 46 && this.value.indexOf('.')>=0 && txtval != 0) {
                                allow = false;
                            }
                            else {
                                if (key == 46) {
                                    allow = true;
                                }
                            }
                            // if key pressed is the decimal and it is not already in the field
                            if (decimal && key == decimal.charCodeAt(0)) {
                                if (this.value.indexOf(decimal) == -1) {
                                    allow = true;
                                }
                                else {

                                    allow = false;
                                }
                            }
                        }
                        else {
                            allow = true;
                        }
                        return allow;

                    });
                });
            });
        function isSpecialChar(e) {
            var evt = (e) ? e : window.event;
            var key = (evt.keyCode) ? evt.keyCode : evt.which;
            if (key != null) {
                key = parseInt(key, 10);
                if ((key < 48 || key > 128)) {
                    if (key == 32)
                        return true;
                    if (!IsUserFriendlyChar(key, "Decimals")) {
                        return false;
                    }
                }
                else {
                    if (evt.shiftKey) {
                        return true;
                    }
                }
            }
            return true;
        }
        function IsUserFriendlyChar(val, step) {
            // Backspace, Tab, Enter, Insert, and Delete
            if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
                return true;
            }
            // Ctrl, Alt, CapsLock, Home, End, and Arrows  
            if ((val > 16 && val < 21) || (val > 34 && val <= 41)) {
                return true;
            }
            if (step == "Decimals") {
                if (val == 190 || val == 110) {  //Check dot key code should be allowed
                    return true;
                }
            }
            // The rest  
            return false;
        }
        function CalculateThisPremium2() {

            var dcurrencyrate = document.getElementById('<%= txtCurrencyRate.ClientID %>').value;
            if (dcurrencyrate > 0) {

                var dthispremium = document.getElementById('<%= txtThisPremium.ClientID %>').value.replace(",", "").replace(",", "").replace(",", "");
                document.getElementById('<%= txtthisPremium2.ClientID %>').value = (dthispremium * dcurrencyrate).toFixed(2); ;
                
            }
        }
        function CalculateThisPremium() {
            var dthispremium2
            var dcurrencyrate = document.getElementById('<%= txtCurrencyRate.ClientID %>').value.replace(",", "").replace(",", "").replace(",", "");
            if (dcurrencyrate > 0) {

                dthispremium2 = document.getElementById('<%= txtThisPremium2.ClientID %>').value;
                document.getElementById('<%= txtthisPremium.ClientID %>').value = (dthispremium2 / dcurrencyrate).toFixed(2);
                
            }


        }
        function CalculatePremium() {

            var rate = document.getElementById('<%= txtRate.ClientID %>').value.replace(/,/gi, "");
            var proratarate = document.getElementById('<%= txtProRataRate.ClientID %>').value;
            var dcurrencyrate = document.getElementById('<%= txtCurrencyRate.ClientID %>').value;
            var SI = document.getElementById('<%= txtSumInsured.ClientID %>').value.replace(",", "").replace(",", "").replace(",", "");
            var d = document.getElementById('<%= ddlRateType.ClientID %>');
            var dthispremium, dannualpremium, dthispremium2, dannualpremium2;
            var sRateType = d.options[d.selectedIndex].value;
            // dropdown value
            if (SI == 0 || rate == 0) {
                if (sRateType == "V") {
                    if (String(rate) == "") {
                        dannualpremium = 0.00;

                    }
                    else {
                        dannualpremium = rate;
                    }
                }
                else {
                    dthispremium = 0.0;
                    dannualpremium = 0.0;
                    dthispremium2 = 0.0;
                    dannualpremium2 = 0.0;
                }
            }

            if (SI > 0) {
                switch (sRateType) {
                    case "V":
                        dannualpremium = rate;
                        break;
                    case "C":
                        dannualpremium = rate * SI / 100.0;
                        break;
                    case "P":
                        dannualpremium = rate * SI / 1000.0;
                        break;
                    case "M":
                        dannualpremium = rate * SI / 1000000.0;
                        break;
                    case "Q":
                        dannualpremium = rate * SI;
                        break;
                    case "O":
                        dannualpremium = 0.0;
                        break;
                    default:
                        dannualpremium = 0.0;
                }



            }

            // Round up to zero decimals if suppress decimal configuration is ON

            if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {
                dannualpremium = parseFloat(dannualpremium).toFixed();
                dthispremium = parseFloat((dannualpremium * proratarate)).toFixed();

                //Maintain Format upto standard decimal place once rounded upto zero
                dannualpremium = parseFloat(dannualpremium).toFixed(4);
                dthispremium = parseFloat((dthispremium)).toFixed(4);
            }
            else {
                dannualpremium = parseFloat(dannualpremium).toFixed(4);
                dthispremium = parseFloat((dannualpremium * proratarate)).toFixed(4);
            }


            if (dcurrencyrate == 0) {
                dthispremium2 = parseFloat(dthispremium).toFixed(2);
                dannualpremium2 = parseFloat(dannualpremium).toFixed(2);
            }
            else {
                dthispremium2 = parseFloat((dthispremium / dcurrencyrate)).toFixed(2);
                dannualpremium2 = parseFloat((dannualpremium / dcurrencyrate)).toFixed(2);
            }

            document.getElementById('<%= txtAnnualPremium.ClientID %>').value = dannualpremium;
            document.getElementById('<%= txtthisPremium.ClientID %>').value = dthispremium;
            document.getElementById('<%= hdnAnnualPremium.ClientID %>').value = dannualpremium;
            document.getElementById('<%= hdnThisPremium.ClientID %>').value = dthispremium;


            if (dcurrencyrate > 0 && (typeof document.getElementById('<%= txtAnnualPremium2.ClientID %>') == "undefined")) {
                document.getElementById('<%= txtAnnualPremium2.ClientID %>').value = dannualpremium2;
                document.getElementById('<%= txtthisPremium2.ClientID %>').value = dthispremium2;
            }
            if (document.getElementById('<%= hvOptionValue.ClientID %>').value == '1') {
                var rate = document.getElementById('<%= txtRate.ClientID %>').value;
                document.getElementById('<%= txtRate.ClientID %>').value = parseFloat(rate.replace(/,/g, '')).toFixed(6);
            }
            else {
                var rate = document.getElementById('<%= txtRate.ClientID %>').value;
                document.getElementById('<%= txtRate.ClientID %>').value = parseFloat(rate.replace(/,/g, '')).toFixed(4);
            }

        }
        function isInteger(e, element) {
            
            var IsNumber = true;
            var txtThispremium = document.getElementById('<%= txtThisPremium.ClientID %>').value;
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "-0123456789.";

            // If Supress Decimal Configuration is On then prevent Decimals values.
            if ((keychar == ".") && $('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {
                IsNumber = false
                return IsNumber;
            }
            else if ($(element).val().indexOf('-') != -1 && (key == 45)) {
                IsNumber = false
                return IsNumber;
            }

            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }

            return IsNumber;

        }


   </script>

    <div id="Modal_RatingSectionDetails">
        <asp:HiddenField ID="hvOptionValue" runat="server"></asp:HiddenField>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_RatingSectionDetailsTitle %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRatingSectionType" runat="server" AssociatedControlID="ddlRatingSectionType" Text="<%$ Resources:lbl_RatingSectionType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlRatingSectionType" runat="server" AutoPostBack="True" CssClass="field-medium form-control form-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEarningPattern" runat="server" AssociatedControlID="ddlEarningPattern" Text="<%$ Resources:lbl_EarningPattern %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlEarningPattern" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Earning_pattern" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRateType" runat="server" AssociatedControlID="ddlRateType" Text="<%$ Resources:lbl_RateType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="ddlRateType" runat="server" DataItemValue="Code" DataItemText="Description" AutoPostBack="True" Sort="ASC" ListType="PMLookup" ListCode="Rate_Type" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ Resources:lbl_Rate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRate" runat="server" onChange="CalculatePremium()" CssClass="e-num4 form-control" Text="0.0000">
                            </asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="chkRateRequired" runat="server" CssClass="error" ControlToValidate="txtrate" ErrorMessage="<%$ Resources:lbl_RateInValid_error %>" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSumInsured" runat="server" AssociatedControlID="txtSumInsured" Text="<%$ Resources:lbl_SumInsured %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtSumInsured" runat="server" onChange="CalculatePremium()" onkeypress="javascript:return isInteger(event,this);" CssClass="e-num2 form-control" Text="0.00"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="chkSumInsuredReq" runat="server" CssClass="error" ControlToValidate="txtSumInsured" ErrorMessage="<%$ Resources:lbl_SIInValid_error %>" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblOverRideReason" runat="server" AssociatedControlID="txtOverRideReason" Text="<%$ Resources:lbl_OverRideReason %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtOverRideReason" runat="server" CssClass="form-control" onkeydown="javascript:return isSpecialChar(event);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAnnualPremium" runat="server" AssociatedControlID="txtAnnualPremium" Text="<%$ Resources:lbl_AnnualPremium %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAnnualPremium" runat="server" CssClass="form-control" Text="0.00"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="chkAnnualPremiumReq" runat="server" CssClass="error" ControlToValidate="txtAnnualPremium" ErrorMessage="<%$ Resources:lbl_AnnualPremiumInvalid_error %>" Display="none"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="chkAnnualPremium" ControlToValidate="txtAnnualPremium" SetFocusOnError="false" OnServerValidate="Field_Validate" ErrorMessage='<%$ Resources:lbl_AnnualPremiumInvalid_error %>' Display="None" runat="server">
                        </asp:CustomValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAnnualPremium2" runat="server" AssociatedControlID="txtAnnualPremium2" Text="<%$ Resources:lbl_AnnualPremium %>" Visible="false" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAnnualPremium2" runat="server" Visible="false" CssClass="e-num2 form-control" Text="0.00"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblThisPremium" runat="server" AssociatedControlID="txtThisPremium" Text="<%$ Resources:lbl_ThisPremium %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtThisPremium" runat="server" CssClass="form-control" onkeypress="return isInteger(event,this);" Text="0.00"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="chkThisPremiumReq" runat="server" CssClass="error" ControlToValidate="txtThisPremium" ErrorMessage="<%$ Resources:lbl_ThisPremiumInvalid_error %>" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblThisPremium2" runat="server" AssociatedControlID="txtThisPremium2" Text="<%$ Resources:lbl_ThisPremium %>" Visible="false" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtThisPremium2" runat="server" CssClass="e-num2 form-control" onChange="CalculateThisPremium()" Text="0.00" Visible="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" CausesValidation="false" Text="<%$ Resources:btn_Cancel %>"></asp:LinkButton>
                <asp:LinkButton ID="btnOK" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btn_Ok %>" OnClientClick="EnableSelect();"></asp:LinkButton>
                
            </div>
        </div>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" EnableClientScript="true" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:HiddenField ID="txtCountry" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtState" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtCurrency" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtProRataRate" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtCurrencyRate" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnIsSuppressDecimals" runat="server" />
        <%--Introducing two Additional Fields because value of Readonly fields 
            if changed using Client Script is not posted back to server.
        --%>
        <asp:HiddenField ID="hdnThisPremium" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnAnnualPremium" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnCalculatedPremium" runat="server" />
    </div>
</asp:Content>
