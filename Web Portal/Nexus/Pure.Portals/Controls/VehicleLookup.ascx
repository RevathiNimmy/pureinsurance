<%@ Control Language="VB" AutoEventWireup="false" CodeFile="VehicleLookup.ascx.vb" Inherits="Controls_VehicleLookup" %>
<script lang="javascript" type="text/javascript">
    function IsAlphaNumeric(evt) {
        evt = evt || window.event;
        var charCode = evt.which || evt.keyCode;
        var charStr = String.fromCharCode(charCode);
        reg = /^[A-Za-z0-9]+$/;
        return reg.test(charStr);
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        prm.add_endRequest(function (sender, e) {
            if (sender._postBackSettings.panelsToUpdate != null) {
                resetControlValue();
                var vehicleLookupResponse = $("#<%=hdnVehicleLookupResponse.ClientID%>").val();               
                if (vehicleLookupResponse == "NO_WEB_SERVICE_CONFIGURED") {
                    errorMsg = "<%= GetLocalResourceObject("err_NoWebServiceConfigured").ToString()%>";
                    alert(errorMsg);
                    return;
                }
                else if (vehicleLookupResponse == "NO_CUST_CODE_CONFIGURED") {
                    errorMsg = "<%= GetLocalResourceObject("err_NoCustCodeConfigured").ToString()%>";
                    alert(errorMsg);
                    return;
                }
                else if (vehicleLookupResponse == "NO_PASSWORD_CONFIGURED") {
                    errorMsg = "<%= GetLocalResourceObject("err_NoPasswordConfigured").ToString()%>";
                    alert(errorMsg);
                    return;
                }
                else {
                    ValidateResponse(vehicleLookupResponse);
                }
            }
        });
    };

    function ValidateResponse(vehicleLookupResponse) {
        var jsonResponseObj = $.parseJSON(vehicleLookupResponse);
        var errorMsg = "";
        if (jsonResponseObj.warning != undefined) {
            errorMsg = jsonResponseObj.warning;
        }

        if (jsonResponseObj.error != undefined) {
            if (jsonResponseObj.error == "WEB_SERVICE_ERROR") {
                errorMsg = "<%= GetLocalResourceObject("err_WebServiceError").ToString()%>";
                alert(errorMsg);
                return;
            }
            else if (jsonResponseObj.error == "INVALID_RESPONSE") {
                errorMsg = "<%= GetLocalResourceObject("err_InvalidResponse").ToString()%>";
                    alert(errorMsg);
                    return;
                }
                else {
                    errorMsg = jsonResponseObj.error;
                }
        }

        if (errorMsg.length > 0) {
            alert(errorMsg);
            $("#<%=txtRegNumber.ClientID %>").val('');
            return;
        }
        else {
            $.each($.parseJSON(vehicleLookupResponse), function (key, value) {
                MapValueToControl(value, key);
            });
        }
    }

    function MapValueToControl(objProperty, ctrId) {

        if (objProperty != undefined && ctrId != undefined) {
            $("#" + ctrId).val(objProperty);
            $("#" + ctrId).next('input').val(objProperty);
        }

    }   

    function resetControlValue() {
        var sCtrlCollection = "<%=MappedControls%>".split(";");         
        if (sCtrlCollection.length > 0) {
            for (i = 0; i < sCtrlCollection.length; i++) {
                if (sCtrlCollection[i] != undefined) {                   
                   $("#ctl00_cntMainBody_" + sCtrlCollection[i].trim()).val('');
                }
            }
        }
    }
    function caseChangeRegNo() {
        var regNo = $("#<%=txtRegNumber.ClientID%>").val().toUpperCase();
        $("#<%=txtRegNumber.ClientID%>").val(regNo);
    }

</script>

<div id="Controls_VehicleLookup">
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <asp:UpdatePanel ID="upVehicleLookup" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnVehicleLookupResponse" EnableViewState="true" runat="server"></asp:HiddenField>
                        
                    
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblRegNumber" runat="server" Text="<%$ Resources:lbl_RegNumber %>" AssociatedControlID="txtRegNumber" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtRegNumber" runat="server" onkeypress="return IsAlphaNumeric(event)" CssClass="form-control"></asp:TextBox></div>
                                <asp:RequiredFieldValidator ID="rfvRegNumber" runat="server" ControlToValidate="txtRegNumber" ValidationGroup="VL" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:err_EmptyRegistraionNumber %>"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revRegNumber" runat="server" ControlToValidate="txtRegNumber" ValidationGroup="VL" ValidationExpression="^[a-zA-Z0-9]*$" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:err_AlphaNumeric %>"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblFind" runat="server" Text="<%$ Resources:lbl_Find %>" AssociatedControlID="btnFind" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <asp:ValidationSummary ID="vsVehicleLookupErrorSummary" ShowMessageBox="true" ValidationGroup="VL" runat="server" ShowSummary="False" CssClass="validation-summary"></asp:ValidationSummary>
                                <asp:Button ID="btnFind" runat="server" OnClientClick="caseChangeRegNo();" Text="<%$ Resources:btn_Find %>" ValidationGroup="VL"></asp:Button>
                            </div>
                        </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
