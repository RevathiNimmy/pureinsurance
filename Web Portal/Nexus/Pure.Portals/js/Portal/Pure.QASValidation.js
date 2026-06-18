(function($) {
    $.fn.QASValidation = function (options) {
        
        // Establish our default settings
        var oSettings = $.extend({
            addressToValidate: null,
            countryCodeControlId: null,
            callbackFunction: null,
            guid: null
        }, options);

        var oPrmJsnRegisteredVariables = {
            "sGUID": oSettings.guid,
            "sCallbackFunction": oSettings.callbackFunction
        };
        var oQasGlobal = new QasOperations(oPrmJsnRegisteredVariables);
        oQasGlobal.PublicMethods.initialize();

        this.bind("click", function (e) {
            
            var sSearchAddressString = "";
            var aControlsToValidate = oSettings.addressToValidate.split(",");

            for (var iCt = 0; iCt < aControlsToValidate.length; iCt++) {
                if (sSearchAddressString != "") {
                    sSearchAddressString += "|" + $('#' + aControlsToValidate[iCt]).val();
                } else
                    sSearchAddressString = $('#' + aControlsToValidate[iCt]).val();
            }

            var sCountryCode = $('#' + oSettings.countryCodeControlId).val();
            var oJsonParams = {
                "sSearchString": sSearchAddressString,
                "sCountryCode": sCountryCode,
                "sGuid": oSettings.guid
            };

            
            var sPostUrl = "../../../Services/pageServices.asmx/findQASAddress";
            postDataToServer("findQASAddress", sPostUrl, oJsonParams, onValidationSucceess, "json", false);
            e.preventDefault();
        });

        function onValidationSucceess(res) {
            var sHtmlTable;
            //get address object returned as json
            var oAddresses = res.d;
            var Monikar;
            var Address1;
            if (oAddresses.length > 0) {
                Monikar = oAddresses[0].Monikar;
                Address1 = oAddresses[0].Address1;
            }

            if (Monikar != undefined && Monikar != '' && Monikar != "") {

                var elm = "<div id='divAddressResultModal' title='Found Item'>" +
                    "<div class='QASFindResultContainer' id='divAddressResults'></div>" +
                    "<div class='submitarea'>" +
                    "<input  class='submit' id='btnQASAccept' type='submit' value='Accept' />" +
                    "<input  class='submit' id='btnQASCancel' type='submit' value='Cancel' />" +
                    "</div>" +
                    "</div>";


                var divAddressResult = document.getElementById('divAddressResultModal');
                if (divAddressResult === null)
                {
                    $(elm).appendTo("body");
                }
                    
                


                sHtmlTable = "<table id='rblAddressList' border='0'><tbody>";
                for (var iCt = 0; iCt <= oAddresses.length - 1; iCt++) {
                    var rbId = 'rblAddressList_' + iCt;
                    //create a table show that table on modal screen
                    sHtmlTable = sHtmlTable + "<tr><td><input id='" + rbId + "' countryCode='" + oAddresses[iCt].CountryCode + "' name='rblAddressList' type='radio' value='" + oAddresses[iCt].Monikar + "'><label for='" + rbId + "'>" + oAddresses[iCt].Address1 + "," + oAddresses[iCt].Address2 + "</label></td></tr>";
                }
                sHtmlTable = sHtmlTable + "</tbody></table>";
                $('#divAddressResults').html(sHtmlTable);
                $('#divAddressResultModal').dialog({
                    dialogClass: "no-close",
                    maxHeight: 450,
                    width: 460,
                    resizable: false,
                    closeOnEscape: false,
                    overlay: {
                        opacity: 0.5,
                        background: "black"
                    }
                });
            } else {
                if (Address1 != undefined && Address1 != '' && Address1 != "") {
                    alert(Address1);
                }
                else {
                    alert('No match found');
                }
            }
        }
    };

} (jQuery));


var QasOperations = function(oPrmJsnRegisteredVariables) {
    try {
        var oPrivateVariables = {
            "sGUID": oPrmJsnRegisteredVariables.sGUID,
            "sCallbackFunction": oPrmJsnRegisteredVariables.sCallbackFunction,
            "oSelectedAddress": null
        };
        var privateMethods = {
            onValidationFailed: function(response) {
                alert(response._message);
            },
            onAddressSelect: function() {
                var sMonikar = $('input[name=rblAddressList]:checked').val();
                if (sMonikar == null) {
                    alert('Select any address to accept');
                }
                else {
                    //Call get address detail for complte address and return to calling function
                    var jsonParams = {
                        "sMoniker": sMonikar,
                        "sCountryCode": $('input[name=rblAddressList]:checked').attr('countryCode').toString()
                    };
                    var sPostUrl = "../../../Services/pageServices.asmx/findQASFullAddress";
                    postDataToServer("findQASFullAddress", sPostUrl, jsonParams, this.onSuccessAddressSelect, "json", false);
                }
            },
            onSuccessAddressSelect: function(res) {
                oPrivateVariables.oSelectedAddress = res.d;
                if ($.isFunction(oPrivateVariables.sCallbackFunction)) {
                    oPrivateVariables.sCallbackFunction(oPrivateVariables.oSelectedAddress);
                }
                $('#divAddressResultModal').dialog('close');
            },
            onCancelClick: function() {
                if ($.isFunction(oPrivateVariables.sCallbackFunction)) {
                    oPrivateVariables.oSelectedAddress = null;
                    oPrivateVariables.sCallbackFunction.call(null);
                }
                $('#divAddressResultModal').dialog('close');
            }
        };
        this.PublicMethods = {
            initialize: function() {
                try {
                    $("body").delegate('#btnQASCancel', 'click', function(e) {

                        privateMethods.onCancelClick();
                        e.preventDefault();
                    });

                    $("body").delegate('#btnQASAccept', 'click', function(e) {
                        privateMethods.onAddressSelect();
                        e.preventDefault();
                    });
                }
                catch (error) {
                    alert(error.message);
                }
            }
        };

    } catch (oError) {
        throw oError;
    }
};
