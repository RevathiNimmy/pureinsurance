<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EditTax.ascx.vb" Inherits="Nexus.Controls_EditTax" %>

<script type="text/javascript">

    $(document).ready(function() {

        $('#<%=grdvRiskTax.ClientID%> [type = checkbox]').each(function() {
            if ($(this).prop('checked')) {
                $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> [type=text]').removeAttr('disabled');
                $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
            }
            else {
                $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').removeAttr('disabled');
                $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
            }

        });

        $('#<%=grdvRiskTax.ClientID%> [type = checkbox]').click(function () {

            if ($(this).prop('checked')) {

                $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> input[type=text]').removeAttr('disabled');
                $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
            }
            else {

                $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').removeAttr('disabled');
                $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
            }

                });



        $('#<%=grdvRiskTax.ClientID%> [type = text]').each(function() {
            $(this).keypress(function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                var keychar = String.fromCharCode(code);
                if (keychar == "." || keychar == "-") {
                    return true;
                }
                else {
                    reg = /\d/;
                    return reg.test(keychar);
                }
            });
        });

        roundOfTaxPercentage();

    });


    function CheckRateAmount() {

        var grid = document.getElementById("<%= grdvRiskTax.ClientID%>").getElementsByTagName("tr");
        var txRateValue;
        if (grid.length > 0) {
            for (rowCount = 1; rowCount < grid.length ; rowCount++) {
                var IsValueType = grid[rowCount].cells[6].getElementsByTagName("input")[0].checked;
                if (IsValueType == true) {
                    //Tax Value has to be less than a length of 12.
                    var vValue = grid[rowCount].cells[4].getElementsByTagName("input")[0].value.replace('%', '').replace('-', '');
                    var vTaxAmountValue = vValue;
                    if ((vValue.toString().indexOf(".") > 12) ||
                        (vValue.toString().indexOf(".") > -1 && (vValue.toString().length - vValue.toString().indexOf(".") - 1) > 4) ||
                        (vValue.toString().indexOf(".") == -1 && vValue.toString().length > 12)) {
                        alert("<%= GetLocalResourceObject("lbl_vldTaxAmountFailed").ToString()%>");
                        return false;
                    }
                    //Tax value has to be numerics only.
                    reg = /^-?\d+\.?\d*$/;
                    if (reg.test(vTaxAmountValue) == false) {
                        alert("<%= GetLocalResourceObject("lbl_vldOnlyNumeric").ToString() %>");
                        return false;
                    }

                }
                else {

                    //Tax rate has to be entered in a way that tax amount is of value is less than a length of 12.
                    var vRate = grid[rowCount].cells[5].getElementsByTagName("input")[0].value.replace('%', '').replace('-', '');
                    txRateValue = vRate;
                    var vPremium = grid[rowCount].cells[7].getElementsByTagName("input")[0].value;
                    var vTaxAmount = (vRate / 100) * vPremium;
                    if ((vTaxAmount.toString().indexOf(".") > 12) || (vTaxAmount.toString().indexOf(".") == -1 && vTaxAmount.toString().length > 12)) {
                        alert("<%= GetLocalResourceObject("lbl_vldTaxAmountFailed").ToString() %>");
                        return false;
                    }

                    //Tax rate has to be numerics only.

                    reg = /^-?\d+\.?\d*$/;
                    if (reg.test(txRateValue) == false) {
                        alert("<%= GetLocalResourceObject("lbl_vldOnlyNumeric").ToString() %>");
                        return false;

                    }
                }
            }
        }
        return true;
    }

    function BindEvents() {
        $(document).ready(function() {
            $('#<%=grdvRiskTax.ClientID%> [type = checkbox]').each(function() {
                if ($(this).prop('checked')) {
                    $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type=text]').removeAttr('disabled');
                    $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
                    
                }
                else {
                    $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').removeAttr('disabled');
                    $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
                }

            });

            $('#<%=grdvRiskTax.ClientID%> [type = checkbox]').click(function () {

                if ($(this).prop('checked')) {

                    $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> input[type=text]').removeAttr('disabled');
                    $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
                }
                else {

                    $(this).parent().parent().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').removeAttr('disabled');
                    $(this).parent().parent().prev().prev().children('#<%=grdvRiskTax.ClientID%> [type = text]').attr('disabled', 'true');
                }
            });

            
            $('#<%=grdvRiskTax.ClientID%> [type = text]').each(function() {
                $(this).keypress(function (e) {
                    var code = (e.keyCode ? e.keyCode : e.which);
                    var keychar = String.fromCharCode(code);
                    if (keychar == "." || keychar == "-") {
                        return true;
                    }
                    else {
                        reg = /\d/;
                        return reg.test(keychar);
                    }
                });
            });


        });

    }

    function roundOfTaxPercentage() {
        var grid = document.getElementById("<%= grdvRiskTax.ClientID%>").getElementsByTagName("tr");
        if (grid.length > 0) {
            for (rowCount = 1; rowCount < grid.length ; rowCount++) {
                var vRate = grid[rowCount].cells[5].getElementsByTagName("input")[0].value.replace('%', '').replace('-', '');
                if (vRate != null || vRate != "undefined") {
                    grid[rowCount].cells[5].getElementsByTagName("input")[0].value = parseFloat(vRate).toFixed(2) + "%";
                }
            }
        }
    }
    function isInteger(e, ctrl) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);
        if (keychar == "-" || keychar == ".") {

            //Prevent the Decimal places except rate feild
            if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {

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

</script>

<div id="policytax_control" runat="server">
    <div class="card card-secondary">
        <div class="card-heading">
            <h4>Taxes</h4>
        </div>
        <div class="card-body clearfix">
            <div class="grid-card table-responsive no-margin">
                <asp:UpdatePanel ID="UpdatePanel_RiskTax" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindEvents);
                            Sys.Application.add_load(setupNumericControls);
                        </script>
                        <asp:GridView ID="grdvRiskTax" EnableViewState="true" runat="server" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_TextAppliedOn %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTempAppliedOn" Text='<%#Eval("TransType")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_TaxGroup %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTempTaxGroup" Text='<%#Eval("TaxGroupDescription")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_TaxBand %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTempTaxBand" Text='<%#Eval("TaxBandDescription")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TaxBandRateDesc">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTempTaxBandDesc" Text='<%#Eval("TaxBandRateDescription")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_TaxAmount %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtItemTempTaxAmount" CssClass="form-control e-num2" Text='<%#Eval("TaxValue")%>' Enabled="false" onkeypress="javascript:return isInteger(event,this);" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_Rate %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtItemTempRate" CssClass="form-control e-per" MaxLength="50" Text='<%#Eval("TaxPercentage")%>' Enabled='<%#Eval("IsEdit") = True%>' runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_IsValueType %>">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="IsValueType" runat="server" Checked='<%#Eval("IsValue")%>' Enabled='<%#Eval("IsEdit") = True%>' Text=" " CssClass="asp-check"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbEdit" CommandName="EDIT_TAX" CommandArgument="1" SkinID="btnGrid" runat="server" Text="<%$ Resources:lbl_Edit_g %>"></asp:LinkButton>
                                        <input type="hidden" value='<%#Eval("Premium")%>' id="hdnItemTempPremium" runat="server">
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
         <asp:HiddenField ID="hdnIsSuppressDecimals" runat="server"></asp:HiddenField>  
        <div class="card-footer">
        <asp:LinkButton ID="btnUpdateRiskTax" runat="server" CausesValidation="false" Text="<%$ Resources:lbl_btnUpdateRiskTax %>" OnClientClick="return CheckRateAmount();" SkinID="btnSM"></asp:LinkButton>
    </div>
    </div>
    
</div>
