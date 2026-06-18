<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ReserveAndRecovery.ascx.vb"
    Inherits="Controls_ReserveAndRecovery" %>

<script type="text/javascript">

    //    function ReviseAmount(dInitialReserve, dRevisedReserve, dAmount, ctrlRevised, sProcessType) {
    //        if (IsNumeric(dAmount) == true) {
    //            if (sProcessType == "1") {
    //                ctrlRevised.value = parseFloat(dAmount);
    //            }
    //            else {
    //                ctrlRevised.value = parseFloat(dInitialReserve) + parseFloat(dRevisedReserve) + parseFloat(dAmount);
    //            }
    //            if (ctrlRevised.value == 'NaN') {
    //                if (sProcessType == "1") {
    //                    ctrlRevised.value = parseFloat(dInitialReserve); //parseFloat(dAmount) ;
    //                }
    //                else {
    //                    ctrlRevised.value = parseFloat(dInitialReserve) + parseFloat(dRevisedReserve);
    //                }
    //            }
    //        }
    //        else {
    //            if (sProcessType == "1") {
    //                ctrlRevised.value = parseFloat(dInitialReserve); //parseFloat(dAmount) ;
    //            }
    //            else {
    //                ctrlRevised.value = parseFloat(dInitialReserve) + parseFloat(dRevisedReserve);
    //            }
    //        }
    //    }

    function ReviseAmount(dInitialReserve, dRevisedReserve, dAmount, ctrlTxtAmount, sProcessType) {
        //    debugger;alert('rakesh');
        dInitialReserve = dInitialReserve.toString();
        dInitialReserve = dInitialReserve.replace(/,/g, "");
        dRevisedReserve = dRevisedReserve.toString();
        dRevisedReserve = dRevisedReserve.replace(/,/g, "");
        dAmount = dAmount.toString();
        dAmount = dAmount.replace(/,/g, "");
        if (IsNumeric(dAmount) == true) {
            if (sProcessType == "1") {
                ctrlTxtAmount.value = parseFloat(RemoveComma(dAmount));
                CheckExceedReserve();
            }
            else {
                ctrlTxtAmount.value = (parseFloat(RemoveComma(dAmount)) - (parseFloat(RemoveComma(dInitialReserve)) + parseFloat(RemoveComma(dRevisedReserve)))).toFixed(2);
                ctrlTxtAmount.value = (Math.round(ctrlTxtAmount.value * 100)) / 100;
            }
            if (ctrlTxtAmount.value == 'NaN') {
                if (sProcessType == "1") {
                    ctrlTxtAmount.value = ""; //parseFloat(dAmount) ;
                }
                else {
                    ctrlTxtAmount.value = "";
                }
            }
        }
        else {
            if (sProcessType == "1") {
                    ctrlTxtAmount.value = ""; //parseFloat(dAmount) ;
            }
            else {
                ctrlTxtAmount.value = "";
            }
        }
    }
    
    function ReviseAmountForGross(dInitialReserve, dRevisedReserve, dAmount, ctrlTxtAmount, sProcessType, ctrlLblTax, ctrlLblNewReserveNet) {
        dInitialReserve = dInitialReserve.toString();
        dInitialReserve = dInitialReserve.replace(/,/g, "");
        dRevisedReserve = dRevisedReserve.toString();
        dRevisedReserve = dRevisedReserve.replace(/,/g, "");
        dAmount = dAmount.toString();
        dAmount = dAmount.replace(/,/g, "");

        // calculate tax and Net value
        ctrlLblTax.value = parseFloat(dAmount) - (parseFloat(dAmount) / (1 + parseFloat(document.getElementById('<%=hdnClaimsReserveTaxPerc.ClientID %>').value) / 100));
        ctrlLblNewReserveNet.value = (parseFloat(dAmount) - parseFloat(ctrlLblTax.value)).toFixed(2);


        if (IsNumeric(ctrlLblNewReserveNet.value) == true) {
            if (sProcessType == "1") {
                ctrlTxtAmount.value = parseFloat(ctrlLblNewReserveNet.value);
            }
            else {
                ctrlTxtAmount.value = (parseFloat(ctrlLblNewReserveNet.value) - (parseFloat(dInitialReserve) + parseFloat(dRevisedReserve))).toFixed(2);
            }
            if (ctrlTxtAmount.value == 'NaN') {
                if (sProcessType == "1") {
                    ctrlTxtAmount.value = ""; //parseFloat(dAmount) ;
                }
                else {
                    ctrlTxtAmount.value = "";
                }
            }
        }
        else {
            if (sProcessType == "1") {
                    ctrlTxtAmount.value = ""; //parseFloat(dAmount) ;
            }
            else {
                ctrlTxtAmount.value = "";
            }
        }
    }
    
    function IsNumeric(sNumber) {
        var bIsValid = true;

        if (sNumber.length > 0) {
            for (i = 0; i < sNumber.length; i++) {
                if (sNumber.charAt(i) == '+' || sNumber.charAt(i) == '-'
                        || sNumber.charAt(i) == '.' || sNumber.charAt(i) == ','
                        || parseInt(sNumber.charAt(i)) >= 0) {
                }
                else {
                    bIsValid = false;
                }
            }
        }
        else {
            bIsValid = false;
        }

        return bIsValid;
    }
	
	function RemoveComma(sNumber) {
        var newNumber;

        if (sNumber.length > 0) {
            newNumber = sNumber.replace(/,/g, '');
        }
        else {
            newNumber = sNumber;
        }
        return newNumber;
    }
	
    function isInteger(e) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);
        if (keychar == "-") {
            return true;
        }
        else {
            reg = /\d/;
            return reg.test(keychar);
        }
    }

    function ConfirmEditReserve(ConfirmEditReserveMsg) {
        IsUnconfirm = confirm(ConfirmEditReserveMsg);
        if (IsUnconfirm == false) {
            document.getElementById('<%=btnEditReserve.ClientID%>').disabled = true;
            var grid = document.getElementById("<%= grdvReserveItem.ClientID %>").getElementsByTagName("tr");
            if (grid.length > 0) {
                for (rowCount = 1; rowCount < grid.length ; rowCount++) {
                    var id = grid[rowCount].cells[4].getElementsByTagName("input")[1].id;
                    document.getElementById(id).disabled = true;
                }
            }
        }
        return IsUnconfirm;
    }

    function CheckExceedReserve() {
        var grid = document.getElementById("<%= grdvReserveItem.ClientID %>").getElementsByTagName("tr");

        var TotalReserveValue = 0.0;
        var ReserveValue = 0.0;

        if (grid.length > 0) {
            for (rowCount = 1; rowCount < grid.length - 3; rowCount++) {
                ReserveValue = grid[rowCount].cells[4].getElementsByTagName("input")[0].value;
                if (IsNumeric(ReserveValue) == true) {
                    TotalReserveValue = parseFloat(TotalReserveValue) + parseFloat(ReserveValue);
                }
            }
            document.getElementById('<%=hTotalReserveValue.ClientID %>').value = TotalReserveValue
        }
    }
    $(document).ready(function () {
        $("[id*='btn_Next']").click(function () {

            var grid = document.getElementById("<%= grdvReserveItem.ClientID %>").getElementsByTagName("tr");
            if (grid.length > 0) {
                for (rowCount = 1; rowCount < grid.length ; rowCount++) {
                    var id = grid[rowCount].cells[4].getElementsByTagName("input")[1].id;
                    if (document.getElementById(id).disabled == false) {
                        document.getElementById(id).onblur();
                    }
                }
            }
            return true;
        });
});
</script>


<div id="Controls_ReserveAndRecovery">
    <asp:UpdatePanel ID="updReserveAndRecovery" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="hTotalReserveValue" runat="server"></asp:HiddenField>
            <input id="hdnCalculate" runat="server" type="hidden" value="0">
            <input id="hdnClaimsReserveForGross" runat="server" type="hidden" value="0">
            <input id="hdnClaimsReserveTaxPerc" runat="server" type="hidden" value="0">
            <legend>
                <asp:Label ID="lblPageTitle" runat="server" Text="<%$ Resources:lblPageTitle %>"></asp:Label>
            </legend>
            <div class="card-body clearfix no-padding">
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvReserveItem" runat="server" AutoGenerateColumns="false" GridLines="None" EnableViewState="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_Description_heading %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_InitialReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:Label ID="lblInitialReserve" runat="server"></asp:Label>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvRevisedItem_CurrentReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:Label ID="lblCurrentReserve" runat="server"></asp:Label>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvRevisedItem_TaxCurrentReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:Label ID="lblTaxCurrentReserve" runat="server"></asp:Label>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvRevisedItem_GrossCurrentReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:Label ID="lblGrossCurrentReserve" runat="server"></asp:Label>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvRevisedItem_GrossReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                   <asp:RangeValidator ID="RngtxtGrossReserve" ControlToValidate="txtGrossReserve" runat="server" ErrorMessage="<%$ Resources:lbl_Failed_GrossReserve_Amt %>" 
                                        MinimumValue="-79228162514264337593543950335" MaximumValue="79228162514264337593543950335" Type="Currency"></asp:RangeValidator>
                                   <asp:TextBox ID="txtGrossReserve" runat="server" CssClass="form-control e-num2" size="8"
                                        AutoCompleteType="None" OnTextChanged="txtGrossRsrv_TextChanged" onClick="this.select();" AutoPostBack="true"></asp:TextBox>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvRevisedItem_Tax_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:Label ID="lblTax" runat="server"></asp:Label>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_Amount_heading %>" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                <ItemTemplate>
                                    <asp:TextBox CssClass="form-control" ID="txtAmount" runat="server" size="6" ReadOnly="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_NewReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:HiddenField ID="HiddenInitReserve" runat="server"></asp:HiddenField>   
                                    <asp:HiddenField ID="HiddenRevsReserve" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lblNewReserveNet" runat="server"></asp:Label>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_NewReserve_heading %>" DataType="Currency">
                                <itemtemplate>
                                    <asp:HiddenField ID="HiddenCurrReserve" runat="server"></asp:HiddenField>                                        
                                    <asp:RangeValidator MaximumValue="79228162514264337593543950335" MinimumValue="0" Type="Currency" ID="RngNewReserve" ControlToValidate="lblNewReserve" runat="server" ErrorMessage="<%$ Resources:lbl_Failed_Revised_Amt %>"></asp:RangeValidator>
                                    <asp:TextBox ID="lblNewReserve" CssClass="form-control e-num2" runat="server" size="8"></asp:TextBox>
                                </itemtemplate>
                            </Nexus:TemplateField>
                            <asp:BoundField DataField="TypeCode" HeaderText="TypeCode"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="p-v-sm text-right">
                <asp:LinkButton ID="btnEditReserve" Visible="false" runat="server" Text="<%$ Resources:btnEditReserve %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
            <asp:HiddenField ID="hdnAllowNegativeReserve" runat ="server" />
            <asp:CustomValidator ID="IsValidReserve" runat="server" Display="none"></asp:CustomValidator></ContentTemplate>
    </asp:UpdatePanel>
    <Nexus:ProgressIndicator ID="piReserveAndRecovery" OverlayCssClass="updating" AssociatedUpdatePanelID="updReserveAndRecovery" runat="server">
        <progresstemplate>
        </progresstemplate>
    </Nexus:ProgressIndicator>
</div>
