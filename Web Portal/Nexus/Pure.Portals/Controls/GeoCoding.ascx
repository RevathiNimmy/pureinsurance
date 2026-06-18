<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GeoCoding.ascx.vb" Inherits="Controls_GeoCoding" %>
<script lang="javascript" type="text/javascript">
    var isGeoCheckTrigger = false;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        prm.add_endRequest(function (sender, e) {
            var geocodingResponse = $("#<%=hdnGeocodingResponse.ClientID%>").val();          
         
            if (geocodingResponse.length > 0 && isGeoCheckTrigger) {

                $.each($.parseJSON(geocodingResponse), function (key, value) {
                    // Error Handling 
                    if (key == "error") {
                        switch (value) {
                            case "NO_WEB_SERVICE_CONFIGURED":
                                errorMsg = "<%= GetLocalResourceObject("err_NoWebServiceConfigured").ToString()%>";
                                alert(errorMsg);
                                break;
                            case "USERNAME_NOT_CONFIGURED":
                                errorMsg = "<%= GetLocalResourceObject("err_NoUsernameConfigured").ToString()%>";
                                alert(errorMsg);
                                break;
                            case "PASSWORD_NOT_CONFIGURED":
                                errorMsg = "<%= GetLocalResourceObject("err_NoPasswordConfigured").ToString()%>";
                                alert(errorMsg);
                                break;
                            case "INPUT_PARAMETERS_NOT_CONFIGURED":
                                errorMsg = "<%= GetLocalResourceObject("err_NoInputParametersConfigured").ToString()%>";
                                alert(errorMsg);
                                MapValueToControl(value, key);
                                break;
                            case "INVALID_RESPONSE":
                                errorMsg = "<%= GetLocalResourceObject("err_InvalidServiceResponse").ToString()%>";
                                alert(errorMsg);
                                break;
                            case "WEB_SERVICE_ERROR":
                                errorMsg = "<%= GetLocalResourceObject("err_ServiceError").ToString()%>";
                                alert(errorMsg);
                                break;
                            case "GEOCODING_FUNCTION_ERROR":
                                errorMsg = "<%= GetLocalResourceObject("err_GeocodingFunctionError").ToString()%>";
                                alert(errorMsg);
                                break;
                        }
                    }
                    else {
                        MapValueToControl(value, key);
                    }

                });
                isGeoCheckTrigger = false;
            }

            if (sender._postBackSettings.panelsToUpdate != null) {
                if (String(sender._postBackSettings.panelsToUpdate).toUpperCase().indexOf("GEOCODING") > 0) {
                    isGeoCheckTrigger = true;
                }
            }
        });
    }

    function MapValueToControl(objProperty, ctrId) {       
        if (objProperty != undefined && ctrId != undefined) {
            $("#" + ctrId).val(objProperty);
            $("#" + ctrId).next('input').val(objProperty);          
        }

    }
</script>

<asp:UpdatePanel ID="upGeoCoding" runat="server"  UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdnGeocodingResponse" EnableViewState="true" runat="server" />
        <asp:Button ID="btnGeoCoding" runat="server" Text="<%$ Resources:btn_GeoCaption %>"   ValidationGroup="GL" />
    </ContentTemplate> 
 
</asp:UpdatePanel>

 