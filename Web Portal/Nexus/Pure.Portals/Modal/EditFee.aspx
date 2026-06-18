<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="EditFee.aspx.vb" Inherits="Modal_EditFee" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        function isInteger(e) {

            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);

            if ((keychar == "-" || keychar == ".")) {

                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1" && document.getElementById("<%= rbtRateTypeVal.ClientID%>").checked) {
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

         $(document).ready(function () {
             ShowProRateField($('#<%= hIsValue.ClientID %>').val());
            CalculateProRateAmount();

        });

        function ShowProRateField(bIsValue) {
            if ($('#<%= hIsProRated.ClientID %>').val() == "True" && bIsValue == "True") {
                $('#liProRatedAmount').show();
            }
            else {
                $('#liProRatedAmount').hide();
            }
        }

        function CalculateProRateAmount() {
            if ($('#<%= hIsProRated.ClientID %>').val() == "True") {
                var dRate = $('#<%= txtRate.ClientID %>').val();
                var dProRataRate = $('#<%= hProRataRate.ClientID %>').val();
                var dProRateAmount = parseFloat(dProRataRate) * parseFloat(dRate);

                //If Suppress Decimal Config is On then Round up to zero decimals
                if ($('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {
                    $('#<%= txtProRatedAmount.ClientID %>').val(dProRateAmount.toFixed());
                }
                else {
                    $('#<%= txtProRatedAmount.ClientID %>').val(dProRateAmount);
                }


                var strProRateAmount = $('#<%= txtProRatedAmount.ClientID %>').val();
                var strAfterDecimal = strProRateAmount.split('.')[1];
                if (strAfterDecimal != undefined) {
                    if (strAfterDecimal.length > 4) {
                        dProRateAmount = dProRateAmount.toFixed(4);
                        $('#<%= txtProRatedAmount.ClientID %>').val(dProRateAmount);
                    }
                }
            }
            RoundUpTozeroDecimals();
        }
        //Round upto zero decimals if decimal supress functionlaity is ON
        function RoundUpTozeroDecimals() {
            //Handle suppress decimal events
            var crTemValue = new Number($('#<%= txtRate.ClientID%>').val());
             if (document.getElementById("<%= rbtRateTypeVal.ClientID%>").checked && $('#<%= hdnIsSuppressDecimals.ClientID%>').val() == "1") {
                 if (crTemValue.toFixed() != 'NaN') {
                     crTemValue = crTemValue.toFixed();
                     $('#<%= txtRate.ClientID%>').val(crTemValue);
                }


            }
        }



    </script>
    <div id="Modal_EditFee">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_EditFee_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblDetails" Text="<%$ Resources:lbl_Heading_EditFee %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblRateType" runat="server" Text="" class="col-lg-2 col-md-3 col-sm-3 control-label"></asp:Label>
                        <div class="col-lg-8 col-md-3 col-sm-9">
                            <asp:RadioButton ID="rbtRateTypePer" GroupName="GrpPercentage" Text="<%$ Resources:li_Percentage %>" onclick="ShowProRateField('False');" runat="server" TabIndex="1" CssClass="asp-radio"></asp:RadioButton>
                            <asp:RadioButton ID="rbtRateTypeVal" GroupName="GrpPercentage" Text="<%$ Resources:li_Value %>" onclick="ShowProRateField('True');CalculateProRateAmount();" runat="server" TabIndex="1" CssClass="asp-radio"></asp:RadioButton>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ Resources:lblRate%>" class="col-lg-2 col-md-3 col-sm-3 control-label"></asp:Label>
                        <div class="col-lg-2 col-md-3 col-sm-9">
                            <asp:TextBox ID="txtRate" TabIndex="2" runat="server" MaxLength="14" CssClass="form-control" onkeypress="javascript:return isInteger(event,this);" onchange="CalculateProRateAmount();"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdRate" runat="server" ControlToValidate="txtRate" Display="None" ErrorMessage="<%$ Resources:lblReqdRate_err %>"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvFeeValid" ControlToValidate="txtRate" runat="server" Display="None"></asp:CustomValidator>
                    </div>
                    <div id="liProRatedAmount" class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                        <asp:Label ID="lblProRatedAmount" runat="server" AssociatedControlID="txtProRatedAmount" Text="<%$ Resources:lblProRatedAmount%>" class="col-lg-2 col-md-3 col-sm-3 control-label"></asp:Label>
                        <div class="col-lg-2 col-md-3 col-sm-9">
                            <asp:TextBox ID="txtProRatedAmount" TabIndex="3" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" TabIndex="4" Text="<%$ Resources:btnCancel %>" CausesValidation="false" OnClientClick="self.parent.tb_remove();"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" CausesValidation="true" SkinID="btnPrimary" runat="server" TabIndex="3" Text="<%$ Resources:btnOk %>"></asp:LinkButton>

            </div>
        </div>
        <asp:HiddenField ID="hRateType" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hFeeKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hIsValue" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hRate" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hIsProRated" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hProRataRate" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnIsSuppressDecimals" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
