<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Instalments.ascx.vb" Inherits="Nexus.Controls_Instalments" %>

<script language="javascript" type="text/javascript">

    var vMtaSession = "<%=  Session.Item(Nexus.Constants.CNMTAType)%>";
    var vRnewalSession = "<%=  Session.Item(Nexus.Constants.CNRenewal)%>";
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(onLoad);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EnableFields);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(OnOverrideDepositTypeChange);
    $(document).ready(function () {
        onLoad();
    });
    function onLoad() {
        if (document.getElementById('<%= txtNewRate.ClientID %>') != null) {
            if (document.getElementById('<%= chkOverrideInterest.ClientID %>').checked == false) {
                document.getElementById('<%= txtNewRate.ClientID %>').readOnly = true;
            }
            if (document.getElementById('<%= chkOverrideCommission.ClientID %>').checked == false) {
                document.getElementById('<%= txtReference.ClientID %>').readOnly = true;
            }
            if (document.getElementById('<%= chkOverrideDeposit.ClientID %>').checked == false) {
                document.getElementById('<%= txtOverrideDeposit.ClientID %>').readOnly = true;
                    $('#<%=optOverrideDepositType.ClientID%> input[type=radio]').attr("disabled", true);
                    OnOverrideDepositTypeChange();
                }
                ValidatorEnable($("#<%=vldNewRateRange.ClientID%>")[0], true);
            ValidatorEnable($("#<%=rfvOverrideDepositRange.ClientID%>")[0], true);
            ValidatorEnable($("#<%=cfvOverrideDeposit.ClientID%>")[0], true);
        }
        if (document.getElementById('<%= vldNewRate.ClientID %>') != null) {
            ValidatorEnable($("#<%=vldNewRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%=rfvReference.ClientID%>")[0], false);
                ValidatorEnable($("#<%=rfvOverrideDeposit.ClientID%>")[0], false);
                ValidatorEnable($("#<%=vldNewRateRange.ClientID%>")[0], false);
                if (document.getElementById('<%= chkOverrideDeposit.ClientID %>').checked == false) {
                    ValidatorEnable($("#<%=rfvOverrideDepositRange.ClientID%>")[0], false);
                ValidatorEnable($("#<%=cfvOverrideDeposit.ClientID%>")[0], false);
            }
        }
        var vMediaType = "";
        if (document.getElementById("<%= grdInstallmentQuotes.ClientID %>") != null) {

            var vInstalmentGrid = document.getElementById("<%= grdInstallmentQuotes.ClientID %>").getElementsByTagName("tr");

            if (vInstalmentGrid.length > 1) {
                vMediaType = vInstalmentGrid[1].cells[2].innerText;
            }
        }


        if (document.getElementById('ctl00_cntMainBody_btnNext') != null) {
            if (vMediaType == "Credit Card" && vMtaSession == "" && vRnewalSession == "") {
                if (document.getElementById('ctl00_cntMainBody_btnBuy') != null) {
                    document.getElementById('ctl00_cntMainBody_btnBuy').style.display = 'none';
                }
                document.getElementById('ctl00_cntMainBody_btnNext').style.display = 'inline-block';
            } else {
                if (document.getElementById('ctl00_cntMainBody_btnBuy') != null) {
                    document.getElementById('ctl00_cntMainBody_btnBuy').style.display = 'inline-block';
                }
                document.getElementById('ctl00_cntMainBody_btnNext').style.display = 'none';
            }
        }
    }
    function isInteger(e, ctrl) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);

        if (keychar == "-" || keychar == ".") {
            if (typeof (ctrl) != "undefined") {
                //Prevent the Decimal places except Per feild
                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1"
                         && ctrl.id == document.getElementById('<%= txtOverrideDeposit.ClientID%>').id
                         && $('#<%=optOverrideDepositType.ClientID%> input[type=radio]:checked').val() == "A") {

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
            reg = /\d/;
            return reg.test(keychar);
        }
    }


    function EnableFields(ctrl) {
        if (document.getElementById('<%= txtNewRate.ClientID %>') != null) {
            if (document.getElementById('<%= chkOverrideInterest.ClientID %>').checked == true) {
                document.getElementById('<%= txtNewRate.ClientID %>').readOnly = false;
                ValidatorEnable($("#<%=vldNewRate.ClientID%>")[0], true);
                ValidatorEnable($("#<%=vldNewRateRange.ClientID%>")[0], true);

                document.getElementById('<%= hdnRate.ClientID%>').value = '1';
            }
            else {
                document.getElementById('<%= txtNewRate.ClientID %>').value = '';
                document.getElementById('<%= txtNewRate.ClientID %>').readOnly = true;
                ValidatorEnable($("#<%=vldNewRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%=vldNewRateRange.ClientID%>")[0], false);
            }
            if (document.getElementById('<%= chkOverrideCommission.ClientID %>').checked == true) {
                document.getElementById('<%= txtReference.ClientID %>').readOnly = false;
                ValidatorEnable($("#<%=rfvReference.ClientID%>")[0], true);

                document.getElementById('<%= hdnCommission.ClientID%>').value = '1';
            }
            else {
                document.getElementById('<%= txtReference.ClientID %>').value = '';
                document.getElementById('<%= txtReference.ClientID %>').readOnly = true;
                ValidatorEnable($("#<%=rfvReference.ClientID%>")[0], false);
            }
            if (document.getElementById('<%= chkOverrideDeposit.ClientID %>').checked == true) {
                document.getElementById('<%= txtOverrideDeposit.ClientID %>').readOnly = false;
                $('#<%=optOverrideDepositType.ClientID%> input[type=radio]').attr("disabled", false);
                ValidatorEnable($("#<%=rfvOverrideDeposit.ClientID%>")[0], true);
                ValidatorEnable($("#<%=rfvOverrideDepositRange.ClientID%>")[0], true);
                ValidatorEnable($("#<%=cfvOverrideDeposit.ClientID%>")[0], true);
                document.getElementById('<%= hdnDeposit.ClientID%>').value = '1';

                if (typeof (ctrl) != "undefined") {
                    //Prevent the Decimal places except Per feild
                    if (ctrl.id == document.getElementById('<%= chkOverrideDeposit.ClientID%>').id) {

                        $('#<%=optOverrideDepositType.ClientID%> input[type=radio][value="A"]').prop("checked", true);
                    }
                }

                //Round up to zero Decimals if Config is ON and  amount is selected
                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1" && $('#<%=optOverrideDepositType.ClientID%> input[type=radio]:checked').val() == "A") {

                    var crTemValue = new Number($('#<%= txtOverrideDeposit.ClientID%>').val());
                    if (!isNaN(parseFloat(crTemValue))) {

                        crTemValue = new Number(parseFloat(crTemValue).toFixed());
                        // Maintain 2 Decimal zero formatting for display
                        crTemValue = (crTemValue).toFixed(2);
                        $('#<%= txtOverrideDeposit.ClientID%>').val(crTemValue)

                    }
                }
            }
            else {
                document.getElementById('<%= txtOverrideDeposit.ClientID %>').value = '0.00';
                document.getElementById('<%= txtOverrideDeposit.ClientID %>').readOnly = true;
                $('#<%=optOverrideDepositType.ClientID%> input[type=radio]').attr("disabled", true);
                $('#<%=optOverrideDepositType.ClientID%> input[type=radio]').attr("checked", false);
                ValidatorEnable($("#<%=rfvOverrideDeposit.ClientID%>")[0], false);
                ValidatorEnable($("#<%=rfvOverrideDepositRange.ClientID%>")[0], false);
                ValidatorEnable($("#<%=cfvOverrideDeposit.ClientID%>")[0], false);
            }
        }
    }

    function setHiddenVariable() {
        if (document.getElementById('<%= chkPaymentProtection.ClientID%>').checked == true) {
            document.getElementById('<%= hdnProtection.ClientID%>').value = '1';
        }
        else {
            document.getElementById('<%= hdnProtection.ClientID%>').value = '0';
        }
    }


    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
        if (document.getElementById('ctl00_cntMainBody_btnNext') != null) {
            if (document.getElementById('<%= hdnMediaType.ClientID %>').value == "Credit Card" && vMtaSession == "" && vRnewalSession == "") {
                    if (document.getElementById('ctl00_cntMainBody_btnBuy') != null) {
                        document.getElementById('ctl00_cntMainBody_btnBuy').style.display = 'none';
                    }
                    document.getElementById('ctl00_cntMainBody_btnNext').style.display = 'inline-block';

                } else {
                    if (document.getElementById('ctl00_cntMainBody_btnBuy') != null) {
                        document.getElementById('ctl00_cntMainBody_btnBuy').style.display = 'inline-block';
                    }
                    document.getElementById('ctl00_cntMainBody_btnNext').style.display = 'none';
                }
            }
        });
        function OnOverrideDepositTypeChange() {
            if (document.getElementById('<%= txtNewRate.ClientID %>') != null) {
            if ($('#<%=optOverrideDepositType.ClientID%> input[type=radio]:checked').val() == "P") {
                document.getElementById('<%= lblOverrideDeposit.ClientID%>').textContent = "Deposit (%)";
            }
            else {
                document.getElementById('<%= lblOverrideDeposit.ClientID%>').textContent = "Deposit";
                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {

                    var crTemValue = new Number($('#<%= txtOverrideDeposit.ClientID%>').val());
                    if (!isNaN(parseFloat(crTemValue))) {

                        crTemValue = new Number(parseFloat(crTemValue).toFixed());
                        // Maintain 2 Decimal zero formatting for display
                        crTemValue = (crTemValue).toFixed(2);
                        $('#<%= txtOverrideDeposit.ClientID%>').val(crTemValue)

                    }
                }
            }
        }
    }
    function cfvOverrideDeposit_ClientValidate(source, arguments) {
        if ($('#<%=optOverrideDepositType.ClientID%> input[type=radio]:checked').val() == "P") {
            if (arguments.Value < 0 || arguments.Value > 100) {
                arguments.IsValid = false;
            } else {
                arguments.IsValid = true;
            }
        }
        if ($('#<%=optOverrideDepositType.ClientID%> input[type=radio]:checked').val() == "A") {
            if (arguments.Value < 0) {
                arguments.IsValid = false;
            } else {
                arguments.IsValid = true;
            }
        }
    }

    function ValidateSelectedCard() {
        var gv = document.getElementById("<%=grdCard.ClientID%>");

        var rbs = gv.getElementsByTagName("input");
        for (var i = 0; i < rbs.length; i++) {

            if (rbs[i].type == "radio") {

                if (rbs[i].checked) {

                    return true

                    break;

                }

            }

        }
        alert("Please Select One Card");
        return false;
    }
    function RadioCheck(rb) {

        var gv = document.getElementById("<%=grdCard.ClientID%>");

          var rbs = gv.getElementsByTagName("input");



          var row = rb.parentNode.parentNode;

          for (var i = 0; i < rbs.length; i++) {

              if (rbs[i].type == "radio") {

                  if (rbs[i].checked && rbs[i] != rb) {

                      rbs[i].checked = false;

                      break;

                  }

              }

          }

      }


</script>

<div id="secure_payment_Installments">

    <asp:HiddenField ID="hdnMediaType" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnDeposit" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hdnCommission" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hdnProtection" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="hdnRate" runat="server" Value="0"></asp:HiddenField>

    <asp:Literal ID="lblErrorMsg" runat="server" Text="<%$ Resources:lbl_ErrorMsg %>" Visible="false"></asp:Literal>
    <asp:Panel ID="pnlInstallmentQuotes" runat="server">
        <asp:Panel ID="pnlInstalmentType" runat="server" Visible="false">
            <asp:HiddenField ID="hvSchemeNo" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hvSchemeVersion" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hvCompanyNo" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hvActualIndex" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hvFrequencyId" runat="server"></asp:HiddenField>
            <div class="card-body clearfix no-padding">
                <legend>
                    
                <asp:RadioButtonList ID="rbInstalmentType" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="asp-radio">
                    <asp:ListItem Text="<%$ Resources:lbl_InstalmentTypeSpreadOver %>" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:lbl_InstalmentTypeNextInstalment %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:lbl_InstalmentTypeNewPlan %>" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </asp:Panel>
        <div class="card-body clearfix no-padding">
            <legend>
                <asp:Literal ID="lblDirectDebitText" runat="server" Text="<%$ Resources:lbl_DirectDebit_text %>"></asp:Literal>
            </legend>
            <div class="grid-card table-responsive">
                <asp:GridView ID="grdInstallmentQuotes" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" DataKeyNames="SchemeNo,SchemeVersion,CompanyNo,FrequencyID" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField DataField="SchemeName" SortExpression="SchemeName" HeaderText="<%$ Resources:lbl_Header_SchemeName %>"></asp:BoundField>
                        <asp:BoundField DataField="FrequencyDescription" SortExpression="FrequencyDescription" HeaderText="<%$ Resources:lbl_Header_PaymentType %>"></asp:BoundField>
                        <asp:BoundField DataField="MediaTypeDescription" SortExpression="MediaTypeDescription" HeaderText="<%$ Resources:lbl_Header_MediaType %>"></asp:BoundField>
                        <Nexus:BoundField DataField="DepositAmount" SortExpression="DepositAmount" HeaderText="<%$ Resources:lbl_Header_Deposit_Amt %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="Amount" SortExpression="Amount" HeaderText="<%$ Resources:lbl_Header_Amount %>" DataType="Currency"></Nexus:BoundField>
                        <asp:BoundField DataField="ProductCode" SortExpression="ProductCode" HeaderText="<%$ Resources:lbl_Header_Type %>"></asp:BoundField>
                        <asp:BoundField DataField="Terms" SortExpression="Terms" HeaderText="<%$ Resources:lbl_Header_Funding %>"></asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol id='menu_<%# Eval("SchemeNo") %>' class="list-inline no-margin">
                                        <li id="liSelect" runat="server">
                                            <asp:LinkButton ID="lnkbtnSelect" Text="<%$ Resources:lbl_Select %>" runat="server" CommandArgument='<%# Eval("MediaTypeDescription") %>' CausesValidation="False" CommandName="Select" SkinID="btnGrid"></asp:LinkButton>
                                        </li>
                                    </ol>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPlanSummary" runat="server" Visible="false">
        <div id="divChangeDate" runat="server">
            <div class="card-body clearfix no-padding">
                <div class="form-horizontal">
                    <legend>Plan Summary – Plan Reference
                        <asp:Literal ID="lbl_Plan_ref" runat="server"></asp:Literal>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDayinMonth" runat="server" AssociatedControlID="ddlDayinMonth" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_DayinMonth %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlDayinMonth" CssClass="form-control form-select" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                <asp:ListItem Text="31" Value="31"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFirstPaymentDate" runat="server" AssociatedControlID="ddlFirstPaymentDate" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_FirstPaymentDate %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlFirstPaymentDate" runat="server" CssClass="form-control form-select" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblUseTransactionCurrency" runat="server" AssociatedControlID="chkUseTransactionCurrency" CssClass="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litUseTransactionCurrency" runat="server" Text="<%$ Resources:lbl_UseTransactionCurrency  %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkUseTransactionCurrency" runat="server" AutoPostBack="true" Enabled="false" OnCheckedChanged="chkUseTransactionCurrency_CheckedChanged" TabIndex="15" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-body clearfix no-padding">
            <div class="md-whiteframe-z0 bg-white">
                <ul class="nav nav-lines nav-tabs b-danger">
                    <li><a href="#tab-summary" data-bs-toggle="tab" aria-expanded="true" class="active">
                        <asp:Literal ID="lbl_Tab_Summary" runat="server" Text="<%$ Resources:lbl_Tab_Summary %>"></asp:Literal></a></li>
                    <li><a href="#tab-instalments" data-bs-toggle="tab" aria-expanded="true">
                        <asp:Literal ID="lbl_Tab_Instalments" runat="server" Text="<%$ Resources:lbl_Tab_Instalments %>"></asp:Literal></a></li>
                    <li><a href="#tab-breakdown" data-bs-toggle="tab" aria-expanded="true">
                        <asp:Literal ID="lbl_Tab_Breakdown" runat="server" Text="<%$ Resources:lbl_Tab_Breakdown %>"></asp:Literal></a></li>
                    <li id="tabOverride"><a href="#tab-Override" data-bs-toggle="tab" aria-expanded="true">
                        <asp:Literal ID="lbl_Tab_Override" runat="server" Text="<%$ Resources:lbl_Tab_Override %>"></asp:Literal></a></li>
                    <li><a href="#tab-finance" data-bs-toggle="tab" aria-expanded="true">
                        <asp:Literal ID="lbl_Tab_Finance_Details" runat="server" Text="<%$ Resources:lbl_Tab_Finance_Details %>"></asp:Literal></a></li>
                    <li><a href="#tab-deposit" data-bs-toggle="tab" aria-expanded="true">
                        <asp:Literal ID="lbl_Tab_Deposit" runat="server" Text="<%$ Resources:lbl_Tab_Deposit %>"></asp:Literal></a></li>
                </ul>
                <div class="tab-content clearfix p b-t b-t-2x">
                    <div id="tab-summary" class="tab-pane animated fadeIn active" role="tabpanel">
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblInstalmentQuteSummary" runat="server" Text="<%$ Resources:lbl_Title_Summary %>"></asp:Label></legend>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblFinancedAmount" runat="server" AssociatedControlID="txtFinancedAmount" Text="<%$ Resources:lbl_Financed_Amount %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtFinancedAmount" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalPayable" runat="server" AssociatedControlID="txtTotalPayable" Text="<%$ Resources:lbl_Total_Payable %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTotalPayable" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                    <%--<asp:RangeValidator ID="RngTotalPayable" runat="server" MinimumValue="0.00" MaximumValue="999999999999999" Display="None" ErrorMessage="<%$ Resources:TotalPayable_Negative %>" Type="Currency" ControlToValidate="txtTotalPayable"></asp:RangeValidator>--%>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTransactions" runat="server" AssociatedControlID="txtTransactions" Text="<%$ Resources:lbl_Transactions %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTransactions" runat="server" CssClass="Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblInstallements" runat="server" AssociatedControlID="txtInstallements" Text="<%$ Resources:lbl_Installements %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtInstallements" runat="server" CssClass="Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ Resources:lbl_Rate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtRate" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAPR" runat="server" AssociatedControlID="txtAPR" Text="<%$ Resources:lbl_APR %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtAPR" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="tab-instalments" class="tab-pane animated fadeIn" role="tabpanel">
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblInstalments" runat="server" Text="<%$ Resources:lbl_Title_Instalments %>"></asp:Label></legend>

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblFirstInstalmentDate" runat="server" AssociatedControlID="txtFirstInstalmentDate" Text="<%$ Resources:lbl_First_Instalment_Date %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtFirstInstalmentDate" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblFirstInstalment" runat="server" AssociatedControlID="txtFirstInstalment" Text="<%$ Resources:lbl_First_Instalment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtFirstInstalment" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblNextInstalment" runat="server" AssociatedControlID="txtNextInstalment" Text="<%$ Resources:lbl_Next_Instalment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtNextInstalment" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblOtherInstalment" runat="server" AssociatedControlID="txtOtherInstalment" Text="<%$ Resources:lbl_Other_Instalment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtOtherInstalment" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblLastInstalment" runat="server" AssociatedControlID="txtLastInstalment" Text="<%$ Resources:lbl_Last_Instalment %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtLastInstalment" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTaxes" runat="server" AssociatedControlID="txtTaxes" Text="<%$ Resources:lbl_Taxes %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTaxes" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="tab-breakdown" class="tab-pane animated fadeIn" role="tabpanel">
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblBreakDown" runat="server" Text="<%$ Resources:lbl_Title_Breakdown %>"></asp:Label></legend>

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblDeposit" runat="server" AssociatedControlID="txtDeposit" Text="<%$ Resources:lbl_Deposit %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtDeposit" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblAdminCharge" runat="server" AssociatedControlID="txtAdminCharge" Text="<%$ Resources:lbl_Admin_Charge %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtAdminCharge" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblProtectionCharge" runat="server" AssociatedControlID="txtProtectionCharge" Text="<%$ Resources:lbl_Protection_Charge %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtProtectionCharge" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblInterest" runat="server" AssociatedControlID="txtInterest" Text="<%$ Resources:lbl_Interest %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtInterest" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="tab-Override" class="tab-pane animated fadeIn" role="tabpanel">
                        <div class="card-body clearfix no-padding">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblOverride" runat="server" Text="<%$ Resources:lbl_Title_Override %>"></asp:Label></legend>


                                <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                                    <div class="col-lg-3 col-md-12 col-sm-12">
                                        <asp:CheckBox runat="server" ID="chkOverrideInterest" Text="<%$ Resources:chk_OverrideInterest %>" TextAlign="Right" onclick="EnableFields(this);" onkeypress="javascript:return isInteger(event);" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                    <div class="col-lg-9 col-md-12 col-sm-12">
                                        <asp:Label ID="lblNewRate" runat="server" AssociatedControlID="txtNewRate" Text="<%$ Resources:lbl_NewRate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtNewRate" runat="server" CssClass="e-num2 Doub form-control" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="vldNewRate" runat="server" Display="none" ControlToValidate="txtNewRate" ErrorMessage="<%$ Resources:NewRate_ErrMsg%>" Enabled="false" ValidationGroup="grpOverRide"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="vldNewRateRange" runat="server" Display="none" ControlToValidate="txtNewRate" ErrorMessage="<%$ Resources:NewRateRange_ErrMsg%>" Enabled="false" ValidationGroup="grpOverRide" Type="Currency" MaximumValue="100.00" MinimumValue="0.00"></asp:RangeValidator>
                                    </div>
                                </div>

                                <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                                    <div class="col-lg-3 col-md-12 col-sm-12">
                                        <asp:CheckBox runat="server" ID="chkOverrideCommission" Text="<%$ Resources:chk_OverrideCommission %>" TextAlign="Right" onclick="EnableFields(this);" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                    <div class="col-lg-9 col-md-12 col-sm-12">
                                        <asp:Label ID="lblReference" runat="server" AssociatedControlID="txtReference" Text="<%$ Resources:lbl_Reference %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtReference" runat="server" CssClass="Doub form-control" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvReference" runat="server" Display="none" ControlToValidate="txtReference" ErrorMessage="<%$ Resources:Reference_ErrMsg%>" Enabled="false" ValidationGroup="grpOverRide"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                                    <div class="col-lg-3 col-md-12 col-sm-12">
                                        <asp:CheckBox runat="server" ID="chkOverrideDeposit" Text="<%$ Resources:chk_OverrideDeposit %>" TextAlign="Right" onclick="EnableFields(this);" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                    <div class="col-lg-9 col-md-12 col-sm-12">
                                        <asp:Label ID="lblOverrideDeposit" runat="server" AssociatedControlID="txtOverrideDeposit" Text="<%$ Resources:lbl_OverrideDeposit %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-4 col-sm-9">
                                            <asp:TextBox ID="txtOverrideDeposit" runat="server" CssClass="e-num2 Doub form-control" onkeypress="javascript:return isInteger(event,this);"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4 col-sm-9">
                                            <asp:RadioButtonList ID="optOverrideDepositType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" onClick="OnOverrideDepositTypeChange();" CssClass="asp-radio">
                                                <asp:ListItem Text="Amount" Value="A" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="%" Value="P"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <asp:CustomValidator ID="cfvOverrideDeposit" runat="server" Display="none" ClientValidationFunction="cfvOverrideDeposit_ClientValidate" ControlToValidate="txtOverrideDeposit" Enabled="false" ErrorMessage="<%$ Resources:OverrideDepositRange_ErrMsg%>" ValidationGroup="grpOverRide"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="rfvOverrideDeposit" runat="server" Display="none" ControlToValidate="txtOverrideDeposit" Enabled="false" ErrorMessage="<%$ Resources:OverrideDeposit_ErrMsg%>" ValidationGroup="grpOverRide"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rfvOverrideDepositRange" runat="server" Display="none" ControlToValidate="txtOverrideDeposit" ErrorMessage="<%$ Resources:OverrideDepositRange_ErrMsg%>" Enabled="false" ValidationGroup="grpOverRide" Type="Currency" MaximumValue="0.00" MinimumValue="-999999.00"></asp:RangeValidator>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="card-footer no-padding">
                            <asp:LinkButton ID="btnOverride" runat="server" Text="<%$ Resources:lbl_Override %>" ValidationGroup="grpOverRide" SkinID="btnPrimary"></asp:LinkButton>
                        </div>
                        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="grpOverRide" CssClass="validation-summary"></asp:ValidationSummary>
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblAdditionalOption" runat="server" Text="<%$ Resources:lbl_Title_AdditionalOption %>"></asp:Label></legend>

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:CheckBox runat="server" ID="chkPaymentProtection" Text="<%$ Resources:chk_PaymentProtection %>" TextAlign="Right" onclick="setHiddenVariable();" CssClass="asp-check"></asp:CheckBox>
                            </div>

                        </div>
                    </div>
                    <div id="tab-finance" class="tab-pane animated fadeIn" role="tabpanel">
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblFinanceDetails" runat="server" Text="<%$ Resources:lbl_Title_Finance_Details %>"></asp:Label></legend>

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblGrossDue" runat="server" AssociatedControlID="txtGrossDue" Text="<%$ Resources:lbl_GrossDueFromClient %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtGrossDue" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalFees" runat="server" AssociatedControlID="txtTotalFees" Text="<%$ Resources:lbl_TotalFeesExcludedForFinancing %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTotalFees" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalTaxes" runat="server" AssociatedControlID="txtTotalTaxes" Text="<%$ Resources:lbl_TotalTaxesExcludedForFinancing %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTotalTaxes" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalAmount" runat="server" AssociatedControlID="txtTotalAmount" Text="<%$ Resources:lbl_TotalAmountAbleToFinanced %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTotalAmount" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div id="tab-deposit" class="tab-pane animated fadeIn" role="tabpanel">
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblMinimumDepositRequired" runat="server" Text="<%$ Resources:lbl_Title_MinimumDepositRequired %>"></asp:Label></legend>

                            <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalFeesCollect" runat="server" AssociatedControlID="txtTotalFeesCollect" Text="<%$ Resources:lbl_TotalFeeDeposit %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTotalFeesCollect" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblTotalTaxesCollect" runat="server" AssociatedControlID="txtTotalTaxesCollect" Text="<%$ Resources:lbl_TotalTaxesDeposit %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtTotalTaxesCollect" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class=" form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblMinimumDeposit" runat="server" AssociatedControlID="txtMinimumDeposit" Text="<%$ Resources:lbl_MinimumDeposit %>" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ReadOnly="true" ID="txtMinimumDeposit" runat="server" CssClass="e-num2 Doub form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>
    <asp:Panel runat="server" ID="pnlBankDetails" Visible="false">
        <asp:UpdatePanel ID="upBankDetails" runat="server">
            <ContentTemplate>
                <div class="card-body clearfix no-padding m-t-sm">
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="lblBenkDetails" runat="server" Text="<%$ Resources:lbl_BankDetails_heading %>"></asp:Label></legend>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAccountType" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="ddlAccountType" Text="<%$ Resources:lbl_AccountType %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="ddlAccountType" runat="server" AutoPostBack="true" CssClass="form-control form-select" onChange="funAccountType();"></asp:DropDownList>
                            </div>
                            <asp:CustomValidator ID="custValAccountType" runat="server" ControlToValidate="ddlAccountType" Enabled="false" Display="None" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>"></asp:CustomValidator><asp:RequiredFieldValidator ID="rfvAccountType" runat="server" ControlToValidate="ddlAccountType" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_AccountType %>" InitialValue="" Enabled="false" ValidationGroup="grpOverRide"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12 m-b-md" id="liEditBank" runat="server">
                            <asp:LinkButton ID="hypBank" runat="server" SkinID="btnSM" Text="<%$ Resources:lkbtn_AddBank %>"></asp:LinkButton>
                            <asp:LinkButton ID="hypBankEdit" runat="server" SkinID="btnSM" Text="<%$ Resources:lkbtn_EditBank %>" Visible="false"></asp:LinkButton>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBankName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBankName" Text="<%$ Resources:lbl_BankName %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtBankName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="txtBankName" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_BankName %>" Enabled="false" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAddress1" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAddress1" Text="<%$ Resources:lbl_BankAddressLine1 %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtAddress1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ControlToValidate="txtAddress1" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_Address %>" Enabled="false" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBranch" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBranch" Text="<%$ Resources:lbl_BranchName %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtBranch" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="txtBranch" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_Branch %>" Enabled="false" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBranchCode" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBranchCode" Text="<%$ Resources:lbl_BranchCode %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtBranchCode" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <asp:RequiredFieldValidator ID="rfvBranchCode" runat="server" ControlToValidate="txtBranchCode" Display="none" ErrorMessage="<%$ Resources:lbl_ErrorMsg_txtBranchCode %>" Enabled="false" ></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAccountName" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccountName" Text="<%$ Resources:lbl_AccountName %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtAccountName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAccountNumber" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtAccountNumber" Text="<%$ Resources:lbl_AccountNumber %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtAccountNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblBIC" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtBIC" Text="<%$ Resources:lbl_BIC %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtBIC" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblIBAN" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtIBAN" Text="<%$ Resources:lbl_IBAN %>"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ReadOnly="true" ID="txtIBAN" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ddlDayinMonth"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="ddlFirstPaymentDate"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="ddlAccountType"></asp:PostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="piBankDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="upBankDetails" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCreditCardDetails" Visible="false">
        <asp:UpdatePanel ID="updCreditCardDetails" runat="server">
            <ContentTemplate>
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblCreditCard" runat="server" Text="<%$ Resources:lbl_CreditCardDetails_heading %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTokenNo" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtTokenNo" Text="<%$ Resources:lbl_TokenNo %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTokenNo" MaxLength="16" CssClass="form-control" runat="server" ValidationGroup="grpOverRide"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RqdTokenNo" ControlToValidate="txtTokenNo"
                                ValidationGroup="grpOverRide" runat="server" ErrorMessage="<%$ Resources:cv_ValidateToken %>"
                                Display="Dynamic" SetFocusOnError="true" EnableClientScript="false"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblExistingTokens" runat="server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="ddlExistingTokens" Text="<%$ Resources:lbl_ExistingTokens %>"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlExistingTokens" CssClass="form-control form-select" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="none"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:CustomValidator ID="cvValidateToken" runat="server" Display="none" ErrorMessage="<%$ Resources:cv_ValidateToken %>" ValidationGroup="grpOverRide"></asp:CustomValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="ProgressIndicator1" OverlayCssClass="updating" AssociatedUpdatePanelID="updCreditCardDetails" runat="server">
            <progresstemplate>
            </progresstemplate>
        </Nexus:ProgressIndicator>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCreditCard" Visible="false">
        <div class="form-horizontal">
            <legend>
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lbl_CreditCardDetails_heading %>"></asp:Label></legend>
            <div class="grid-card table-responsive no-margin">
                <asp:GridView ID="grdCard" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found" AllowPaging="False" AllowSorting="true" DataKeyNames="PartyBankKey">

                    <Columns>


                        <asp:BoundField DataField="Number" HeaderText="Card Number"></asp:BoundField>
                        <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date"></asp:BoundField>
                        <asp:BoundField DataField="NameOnCreditCard" HeaderText="Name" Visible="false"></asp:BoundField>
                        <asp:TemplateField>

                            <ItemTemplate>

                                <asp:RadioButton ID="rdDefaultCard" runat="server"
                                    onclick="RadioCheck(this);" Checked='<%#Eval("IsDefaultCreditCard")%>' />

                                <asp:HiddenField ID="hdnTokenNo" runat="server"
                                    Value='<%#Eval("TrackingNumber")%>' />
                                <asp:HiddenField ID="hdnAuthCode" runat="server"
                                    Value='<%#Eval("ManualAuthCode")%>' />
                            </ItemTemplate>

                        </asp:TemplateField>



                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
    <asp:CustomValidator ID="cvZeroPremium" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_ZeroPremium_Error %>" Display="none"></asp:CustomValidator>
    <asp:CustomValidator ID="cvInstalmentPlans" runat="server" Display="None" OnServerValidate="cvInstalmentPlans_ServerValidate"></asp:CustomValidator>


</div>

<asp:HiddenField ID="txtBankDetailData" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdnNextInstalmentDueDate" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdnFirstPaymentDateSelected" runat="server"></asp:HiddenField>
<asp:HiddenField ID="hdnIsSuppressDecimals" runat="server"></asp:HiddenField>

<script type="text/javascript">
    function funAccountType() {
        if (document.getElementById('<%=ddlAccountType.ClientID %>').value == '') //
        {
            document.getElementById('<%=txtBankName.ClientID %>').value = '';
            document.getElementById('<%=txtAddress1.ClientID %>').value = '';
            document.getElementById('<%=txtBranch.ClientID %>').value = '';
            document.getElementById('<%=txtBranchCode.ClientID %>').value = '';
            document.getElementById('<%=txtAccountName.ClientID %>').value = '';
            document.getElementById('<%=txtAccountNumber.ClientID %>').value = '';
            document.getElementById('<%=txtBIC.ClientID %>').value = '';
            document.getElementById('<%=txtIBAN.ClientID %>').value = '';
        }
    }
    function ReceiveBankData(sContactData, sPostBackTo) {
        document.getElementById('<%= txtBankDetailData.ClientID %>').value = sContactData;
        __doPostBack(sPostBackTo, 'UpdateBank');
    }
</script>
