
/*To make a REST/ASMX Service Call */
function postDataToServer(sContext, sPostUrl, sJsnFormParams, sCallBack, sDataType, bSynchronous) {
    try {
        var bIsSynchronous = false;
        if (bSynchronous != undefined && (bSynchronous == true || bSynchronous.toString().toLowerCase() == "true")) bIsSynchronous = true;
        $.ajax({
            url: sPostUrl,
            type: "POST",
            async: bIsSynchronous,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(sJsnFormParams),
            success: function(oResponse) {
            receiveServerData(oResponse, sContext, sCallBack, sDataType);
            },
            error: function(oResponse) {
                alert(oResponse.responseText);
            }
        });
    } catch (error) {
        alert(error.message);
    }
}

/* TO collect success response from ajax call*/
function receiveServerData(oResponse, sContext, sCallBack, sDataType) {
    try {
        if (validateServerResponse(sContext, oResponse, sDataType) === true) {
            if (sCallBack != 'undefined' && sCallBack !== "") {
                sCallBack(oResponse, sContext);
            }
        }
    } catch (error) {
        alert(error.message);
    }
}

/*This method validate response from server Context for which server call was made Response from server Response type*/
function validateServerResponse(sContext, oResponse, sDataType) 
{
    var blnIsValid = true;
    try {
        var strErrorMessage = "";
        switch (sDataType) {
            case "text":
                if (oResponse.substring(0, 9) == "**ERROR**") {
                    strErrorMessage = oResponse.replace("**ERROR** ", "");
                    blnIsValid = false;
                }
                break;
            case "xml":
                if ($(oResponse).find("error").text() == "true") {
                    strErrorMessage = $(oResponse).find("output").text();
                    blnIsValid = false;
                }
                break;
            case "json":
                if (oResponse.error == "true") {
                    strErrorMessage = oResponse.output;
                    blnIsValid = false;
                }
                break;
            default:
                if (oResponse.substring(0, 9) == "**ERROR**") {
                    strErrorMessage = oResponse.replace("**ERROR** ", "");
                    blnIsValid = false;
                }
                break;
        }
        if (!blnIsValid) 
        {
           alert(sContext + " : " +strErrorMessage);
        }
    } 
    catch (oError) 
    {
        alert(oError.message);
    }
    return blnIsValid;
}

