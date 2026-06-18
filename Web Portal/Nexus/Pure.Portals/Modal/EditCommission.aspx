<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="EditCommission.aspx.vb" Inherits="Nexus.Modal_EditCommission" Title="Edit Commission" EnableSessionState="True" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">
        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
            manager.add_endRequest(OnEndRequest);
        }

        $(document).ready(function () {

            var session = '<%= HttpContext.Current.Session("COMMISSION_WARNING")%>';
            if (session == null || session == '') {

                document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "none";
                document.getElementById('<%= lblWarning.ClientID%>').style.display = "none";
            }
            else {

                if (session.indexOf("CommissionAmend") > -1) {

                    document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "block";
                }
                else { document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "none"; }

                if ((session.indexOf("TaxAmend") > -1) || (session.indexOf("TaxGroupAmend") > -1)) {

                    document.getElementById('<%= lblWarning.ClientID%>').style.display = "block";
                 }
                 else { document.getElementById('<%= lblWarning.ClientID%>').style.display = "none"; }


             }
        });

         function CheckCommissionvalueAmend() {

             var OldCommissionRate = document.getElementById('<%= hdnOCommissionValue.ClientID%>');
            OldCommissionRate.value = currDefaultFormat(OldCommissionRate.value);
            var NewCommissionRate = document.getElementById('<%= txtCommissionValue.ClientID%>');
            NewCommissionRate.value = currDefaultFormat(NewCommissionRate.value);
            if (OldCommissionRate.value != NewCommissionRate.value) {
                document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "block";
            }
            else {
                document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "none";
            }
        }
        function OnBeginRequest(sender, args) {
            //disable the button (or whatever else we need to do) here
            var btnOk = document.getElementById('<%= btnOk.ClientId%>');
            var btnCancel = document.getElementById('<%= btnCancel.ClientId%>');

            if (btnOk != null) {
                btnOk.disabled = true;
            }

            if (btnCancel != null) {
                btnCancel.disabled = true;
            }

        }

        function OnEndRequest(sender, args) {
            //enable the button (or whatever else we need to do) here
            var btnOk = document.getElementById('<%= btnOk.ClientId%>');
            var btnCancel = document.getElementById('<%= btnCancel.ClientId%>');

            if (btnOk != null) {
                btnOk.disabled = false;
            }

            if (btnCancel != null) {
                btnCancel.disabled = false;
            }
        }
        onload = function () {
            if (document.getElementById('<%= txtCommission.ClientID%>').value.length == 0) {
                document.getElementById('<%= txtCommission.ClientID%>').value = currDefaultFormat(document.getElementById('<%= txtCommission.ClientID%>').value);
            }
            if (document.getElementById('<%= txtCommissionValue.ClientID%>').value.length == 0) {
                document.getElementById('<%= txtCommissionValue.ClientID%>').value = currDefaultFormat(document.getElementById('<%= txtCommissionValue.ClientID%>').value);
            }
        }

        function CalculateCommission(commissionRate, commission, premium, cntrl) {
            // debugger
            var hdCommissionValue = document.getElementById('<%= hdCommissionValue.ClientID%>');
            var hdCommissionRate = document.getElementById('<%= hdCommissionRate.ClientID%>');
            if (cntrl == 'Commission') {
                var commissionValue = document.getElementById(commission);
                if (premium != 0) {
                    if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {

                        commissionValue.value = ((premium * commissionRate.value / 100) * 100) / 100;
                        //Handle suppress decimal events
                        var crTemValue = new Number(commissionValue.value);
                        crTemValue = crTemValue.toFixed();

                        // Maintain 2 Decimal zero formatting for display
                        crTemValue = parseFloat(crTemValue).toFixed(2);

                        commissionValue.value = crTemValue;
                        hdCommissionValue.value = crTemValue;
                    }
                    else {
                        commissionValue.value = Math.round((premium * commissionRate.value / 100) * 100) / 100;
                        hdCommissionValue.value = commissionValue.value;
                    }
                }
                if (commissionRate.value != "Infinity") {
                    hdCommissionRate.value = commissionRate.value;
                }
            }
            else {
                var commRate = document.getElementById(commission);
                if (premium != 0) {
                    commRate.value = Math.round(((100 * commissionRate.value) / premium) * 100) / 100;
                    hdCommissionRate.value = commRate.value
                }
                if (commissionRate.value != "Infinity") {
                    hdCommissionValue.value = commissionRate.value;
                }
            }
        }

        function isInteger(e, ctrl) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            if (keychar == "-" || keychar == ".") {

                //Prevent the Decimal places except rate feild
                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1" && ctrl.id != document.getElementById('<%= txtCommission.ClientID%>').id) {

                    return false;
                }
                else {
                    return true;
                }

            }
            else {
                reg = /\d/;
                return reg.test(keychar);
            }
        }
        function currDefaultCommFormat(num_SI) {
            // alert("test");
            num_SI = num_SI.toString().replace(/\$|\,/g, '');
            if (isNaN(num_SI))
                num_SI = "0";
            sign = (num_SI == (num_SI = Math.abs(num_SI)));
            num_SI = Math.floor(num_SI * 10000 + 0.50000000001);
            cents = num_SI % 10000;
            num_SI = Math.floor(num_SI / 10000).toString();
            if (cents < 10)
                //if not use . then it will go inside
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num_SI.length - (1 + i)) / 3) ; i++)
                //num_SI = num_SI.substring(0,num_SI.length-(4*i+3))+','+
                num_SI = num_SI.substring(0, num_SI.length - (4 * i + 3)) +
                num_SI.substring(num_SI.length - (4 * i + 3));
            num_SI = (((sign) ? '' : '-') + num_SI + '.' + cents);
            return num_SI;
        }
        function currDefaultFormat(num_SI) {

            // alert("test");
            num_SI = num_SI.toString().replace(/\$|\,/g, '');
            if (isNaN(num_SI))
                num_SI = "0";
            sign = (num_SI == (num_SI = Math.abs(num_SI)));
            num_SI = Math.floor(num_SI * 100 + 0.50000000001);
            cents = num_SI % 100;
            num_SI = Math.floor(num_SI / 100).toString();
            if (cents < 10)
                //if not use . then it will go inside
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num_SI.length - (1 + i)) / 3) ; i++)
                //num_SI = num_SI.substring(0,num_SI.length-(4*i+3))+','+
                num_SI = num_SI.substring(0, num_SI.length - (4 * i + 3)) +
                num_SI.substring(num_SI.length - (4 * i + 3));
            num_SI = (((sign) ? '' : '-') + num_SI + '.' + cents);
            return num_SI;
        }

        function CheckTaxAmendanother(taxgroup) {
            var OldTaxValue = document.getElementById('<%= hdnOTaxGroup.ClientID%>');
            var AmendTaxValue2 = taxgroup;
            if (document.getElementById('<%= lblWarning.ClientID%>') != null) {
                if (OldTaxValue.value != AmendTaxValue2.value) {
                    document.getElementById('<%= lblWarning.ClientID%>').style.display = "block";
                }
                else {
                    document.getElementById('<%= lblWarning.ClientID%>').style.display = "none";
                }
            }
        }

        function CheckTaxAmend() {

            var OldTaxValue = document.getElementById('<%= hdnOTaxValue.ClientID%>');
            OldTaxValue.value = currDefaultFormat(OldTaxValue.value);
            var AmendTaxValue = document.getElementById('<%= txtTaxValue.ClientID%>');
            AmendTaxValue.value = currDefaultFormat(AmendTaxValue.value);
            if (document.getElementById('<%= lblWarning.ClientID%>') != null) {
                 if (OldTaxValue.value != AmendTaxValue.value) {
                     document.getElementById('<%= lblWarning.ClientID%>').style.display = "block";
                }
                else {
                    document.getElementById('<%= lblWarning.ClientID%>').style.display = "none";
                }
            }
        }

        function CheckCommissionAmend() {

            var OldCommissionRate = document.getElementById('<%= hdnOCommissionRate.ClientID%>');
            OldCommissionRate.value = currDefaultFormat(OldCommissionRate.value);
            //alert("OldCommissionRate" + OldCommissionRate.value)
            var NewCommissionRate = document.getElementById('<%= txtCommission.ClientID%>');
            //alert("NewCommissionRate" + NewCommissionRate.value)
            NewCommissionRate.value = currDefaultFormat(NewCommissionRate.value);
            if (OldCommissionRate.value != NewCommissionRate.value) {
                document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "block";
            }
            else {
                document.getElementById('<%= lblCommissionWarning.ClientID%>').style.display = "none";
            }
        }


    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Modal_EditCommission">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lblHeading %>"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">

                    <asp:UpdatePanel ID="UpdEditCommission" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAgent" runat="server" AssociatedControlID="ltAgentValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltAgent" runat="server" Text="<%$ Resources:lblAgent %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <p class="form-control-static font-bold">
                                        <asp:Literal ID="ltAgentValue" runat="server"></asp:Literal>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblRiskType" runat="server" AssociatedControlID="ltRiskTypeValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltRiskType" runat="server" Text="<%$ Resources:lblRiskType %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <p class="form-control-static font-bold">
                                        <asp:Literal ID="ltRiskTypeValue" runat="server"></asp:Literal>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAgentType" runat="server" AssociatedControlID="ltAgentTypeValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltAgentType" runat="server" Text="<%$ Resources:lblAgentType %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <p class="form-control-static font-bold">
                                        <asp:Literal ID="ltAgentTypeValue" runat="server"></asp:Literal>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCommissionBand" runat="server" AssociatedControlID="ltCommissionBandValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCommissionBand" runat="server" Text="<%$ Resources:lblCommissionBand %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <p class="form-control-static font-bold">
                                        <asp:Literal ID="ltCommissionBandValue" runat="server"></asp:Literal>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblPremium" runat="server" AssociatedControlID="ltPremiumValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltPremium" runat="server" Text="<%$ Resources:lblPremium %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <p class="form-control-static font-bold">
                                        <asp:Label ID="ltPremiumValue" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCommission" runat="server" AssociatedControlID="txtCommission" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCommission" runat="server" Text="<%$ Resources:lblCommission %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtCommission" MaxLength="13" runat="server" onkeypress="javascript:return isInteger(event,this);" AutoPostBack="true" onchange="CheckCommissionAmend()" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:HiddenField ID="hdnOldCommission" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hdCommissionRate" runat="Server"></asp:HiddenField>
                                <asp:RequiredFieldValidator ID="rqdCommission" runat="server" ControlToValidate="txtCommission" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lblCommission_err %>"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCommissionValue" runat="server" AssociatedControlID="txtCommissionValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCommissionValue" runat="server" Text="<%$ Resources:lblCommissionValue %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtCommissionValue" MaxLength="13" runat="server" onkeypress="javascript:return isInteger(event,this);" onchange="javascript:CheckCommissionvalueAmend();this.value=currDefaultFormat(this.value);" AutoPostBack="true" CssClass="form-control"> </asp:TextBox>
                                </div>
                                <asp:HiddenField ID="hdCommissionValue" runat="Server"></asp:HiddenField>

                                <asp:RequiredFieldValidator ID="rqdCommissionVal" runat="server" ControlToValidate="txtCommissionValue" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lblCommissionValue_err %>"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdnCommissionValue" runat="server"></asp:HiddenField>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTaxGroup" runat="server" AssociatedControlID="ddlTaxGroup" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltTaxGroup" runat="server" Text="<%$ Resources:lblTaxGroup %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <NexusProvider:LookupList ID="ddlTaxGroup" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="TAX_GROUP" DefaultText="<%$ Resources:lbl_TaxGroup_defaulttext %>" CssClass="field-medium form-control form-select" Enabled="false" AutoPostBack="true"></NexusProvider:LookupList>
                                </div>
                                <asp:HiddenField ID="hdnamdTaxValue" runat="server"></asp:HiddenField>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTaxValue" runat="server" AssociatedControlID="txtTaxValue" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltTaxValue" runat="server" Text="<%$ Resources:lblTaxValue %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtTaxValue" runat="server" Enabled="false" AutoPostBack="true" onkeypress="javascript:return isInteger(event,this);" onChange="javascript:CheckTaxAmend();" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:RangeValidator ID="rvTaxValue" runat="server" ControlToValidate="txtTaxValue" MinimumValue="-99999999999999" MaximumValue="99999999999999" Display="None" SetFocusOnError="true" Type="Double" ErrorMessage="<%$ Resources:lbl_InvalidTaxValue%>"></asp:RangeValidator>
                                <asp:TextBox ID="txthiddenIsValue" runat="server" Style="display: none"></asp:TextBox>
                                <asp:HiddenField ID="hdnOldTaxValue" runat="server"></asp:HiddenField>
                                <asp:TextBox ID="txtOldTaxValue" runat="server" Style="display: none" />
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblOverride" runat="server" AssociatedControlID="txtOverride" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltOverride" runat="server" Text="<%$ Resources:lblOverride %>"></asp:Literal>
                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtOverride" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rqdOverride" runat="server" ControlToValidate="txtOverride" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lblOverride_err %>"></asp:RequiredFieldValidator>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtCommission" EventName="TextChanged"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="txtCommissionValue" EventName="TextChanged"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="ddlTaxGroup" EventName="SelectedIndexChange"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <nexus:ProgressIndicator ID="upEditComission" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdEditCommission" runat="server">
                        <ProgressTemplate>
                        </ProgressTemplate>
                    </nexus:ProgressIndicator>
                    <asp:HiddenField ID="hdnOCommissionValue" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnOTaxValue" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnOCommissionRate" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnOTaxGroup" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnNTaxGroup" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnIsSuppressDecimals" runat="server" />
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_btnCancel %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_btnOk %>" UseSubmitBehavior="true" SkinID="btnPrimary"></asp:LinkButton>

            </div>
        </div>
        <asp:Label ID="lblCommissionWarning" runat="server" CssClass="alert alert-warning" Text="<%$ Resources:lbl_CommWarning%>"></asp:Label>
        <asp:Label ID="lblWarning" runat="server" Text="<%$ Resources:lbl_Warning%>" CssClass="alert alert-warning"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:CustomValidator ID="vldMaximumCommission" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_vldMaximumCommission %>" Enabled="false"></asp:CustomValidator>
        <asp:CustomValidator ID="vldPremiumExceeded" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_vldPremiumExceeded %>" Enabled="false"></asp:CustomValidator>
    </div>
</asp:Content>
