function ToggleTab(show, tab) {
    if (show == 0) {
        HideTab(tab)
    }
    else {
        ShowTab(tab)
    }
}

function RoundToTwo(num) {
    num = num * 100;
    num = Math.round(num);
    num = num / 100;
    return num;
}

/// <summary>
/// Toggle a fields visibility and associated label if applicable
/// </summary>
/// <param name="show">Value indicating whether the fields must be visible of hidden</param>
/// <param name="">Build in parameter array, this could be n number of fields to toggle</param>
/// <example>toggleFields(0, 'EE__EXT_REINSTATE_LOI','EE__EXT_REINSTATE_PREM','EE__EXT_REINSTATE_PERC','EE__EXT_REINSTATE_AMT')</example>
function toggleFields(show) {
    for (var i = 1; i < toggleFields.arguments.length; i++) {
        if (toggleFields.arguments[i] != '') {
            toggleField(show, toggleFields.arguments[i]);
        }
    }
}

/// <summary>
/// Toggles a fields visibility and associated label if applicable
/// </summary>
/// <param name="show">Value indicating whether the fields must be visible of hidden</param>
/// <param name="fieldName">Field that must be shown or hidden</param>
/// <example>toggleFields(0, 'EE__EXT_REINSTATE_LOI')</example>
function toggleField(show, fieldName) {
    var control = $("[id$='" + fieldName + "']");
    for (var j = 0; j < control.length; j++) {
        if (show == true) {
            $(control[j]).show();
            $(control[j])[0].disabled = false;
            $('label[for="' + control[j].id + '"]').show();
            //enableControl($('label[for="' + control[j].id + '"]').attr('id'), true);
        }
        else {
            // Hide or disable control
            if (show == false) {
                $(control[j]).hide();
                $('label[for="' + control[j].id + '"]').hide();
                //enableControl($('label[for="' + control[j].id + '"]').attr('id'), false);
            }
            else {
                $(control[j])[0].disabled = true;
            }

            // clear values
            var targetControl = $(control[j])[0];
            if (targetControl.type == "text") {
                targetControl.value = '';
            }
            else if (targetControl.type == "checkbox") {
                targetControl.checked = false;
            }
            else if (targetControl.type == "select-one") {
                targetControl.selectedIndex = 0;
            }
        }

        for (var i = 0; i < Page_Validators.length; i++) {
            if (Page_Validators[i].controltovalidate == control[j].id) {
                Page_Validators[i].enabled = show == true;
                Page_Validators[i].isvalid = show != true;
            }
        }
    }
}

/// <summary>
/// Gets the base/root url of the page eg http://localhost/Nexus/
/// </summary>
var appRootPath = ''; // Set by Master.
function getBaseURL() {
    return appRootPath;
}

function cleanNumber(num) {
    var re = new RegExp("(£)|(\\s)|(,)|([A-Za-z])|(%)", "g");
    return num.replace(re, '');
}

var e_curFmt = "£ #,###.00";
var e_num0Fmt = "#,##0";
var e_num2Fmt = "#,###.00";
var e_num4Fmt = "#,###.0000";
var e_num5Fmt = "#,###.00000";
var e_perFmt = "#.00000%%";

function setupNumericControls() {
    configNumericControls('input:text[class*="e-cur"]', e_curFmt);
    configNumericControls('input:text[class*="e-num0"]', e_num0Fmt);
    configNumericControls('input:text[class*="e-num2"]', e_num2Fmt);
    configNumericControls('input:text[class*="e-num4"]', e_num4Fmt);
    configNumericControls('input:text[class*="e-num5"]', e_num5Fmt);
    configNumericControls('input:text[class*="e-per"]', e_perFmt);
}

function configNumericControls(byControl, e_format) {
    $(byControl).each(function () {

               $(this).numeric(".");
        if ($(this).val() != "") {
            if (e_format != "") {
                if (e_format == e_perFmt) {
                    $(this).val(parseFloat($(this).val()).toFixed(2));
                    $(this).format({ format: "#.00%%", locale: "us" });
                }
                else {
                    $(this).val(cleanNumber($(this).val()));

                    if (parseFloat($(this).val()) < 0) {
                        $(this).val(-1 * $(this).val());
                        $(this).format({ format: e_format, locale: "us" });
                        $(this).val("-" + $(this).val());
                    } else {
                        $(this).format({ format: e_format, locale: "us" });
                    }                                
                }
            }
        }

        $(this).focus(function () {
            if ($(this).val() != "") {
                if ($(this).attr("interactive") != "0") {
                    $(this).val(cleanNumber($(this).val()));
                }
            }
            $(this).select();
        });

        $(this).blur(function () {
            if ($(this).val() == ".") $(this).val('');
            if ($(this).val() != "") {
                if (e_format != "") {
                    if ($(this).attr("interactive") != "0") {
                        if (e_format == e_perFmt) {
                            $(this).val(parseFloat($(this).val()).toFixed(2));
                            $(this).format({ format: "#.00%%", locale: "us" });
                        }
                        else {
                            if (parseFloat($(this).val()) < 0) {
                                $(this).val(-1 * $(this).val());
                                $(this).format({ format: e_format, locale: "us" });
                                $(this).val("-" + $(this).val());
                            } else {
                                $(this).format({ format: e_format, locale: "us" });
                            }
                        }
                    }
                }
            }
        });


    });
}

function removeNumricFormatting() {
    removeNumricFormat('input:text[class*="e-cur"]');
    removeNumricFormat('input:text[class*="e-num0"]');
    removeNumricFormat('input:text[class*="e-num2"]');
    removeNumricFormat('input:text[class*="e-num4"]');
    removeNumricFormat('input:text[class*="e-num5"]');
    removeNumricFormat('input:text[class*="e-per"]');
}
function removeNumricFormat(byControl) {
    $(byControl).each(function () {
        if ($(this).val() != "") {
            if ($(this).attr("interactive") != "0") {
                $(this).val(cleanNumber($(this).val()));
            }
        }
    });
}

function setupForms() {
    $('div.panelBlock').mouseover(function () {
        $(this).css('background-color', '#DBE5F1');
    });
    $('div.panelBlock').mouseout(function () {
        $(this).css('background-color', '#F0F5FF');
    });
}

function setupToggleFields() {
    var checkBoxWithToggleFields = $("span[toggleFields] > input:checkbox");
    for (var i = 0; i < checkBoxWithToggleFields.length; i++) {
        var fieldsToToggle = checkBoxWithToggleFields[i].parentElement.getAttribute('toggleFields').split(',');
        var checkBox = checkBoxWithToggleFields[i];
        for (var j = 0; j < fieldsToToggle.length; j++) {
            handleCheckboxFieldVisibleChange(checkBox.id, jQuery.trim(fieldsToToggle[j]));
        }
    }
}

function handleCheckboxFieldVisibleChange(key, target) {
    if ($('#' + key).is(":checked")) {
        toggleField(true, target);
    }
    else {
        toggleField(false, target);
    }

    $('#' + key).click(

      function () {
          if ($('#' + key).is(":checked")) {
              toggleField(true, target);
          }
          else {
              toggleField(false, target);
          }
      }
     );
}

function setupReadOnlyFields() {
    var checkBoxWithToggleFields = $("span[toggleReadOnly] > input:checkbox");
    for (var i = 0; i < checkBoxWithToggleFields.length; i++) {
        var fieldsToToggle = checkBoxWithToggleFields[i].parentElement.getAttribute('toggleReadOnly').split(',');
        var checkBox = checkBoxWithToggleFields[i];
        for (var j = 0; j < fieldsToToggle.length; j++) {
            handleCheckboxFieldReadOnlyChange(checkBox.id, jQuery.trim(fieldsToToggle[j]));
        }
    }
}

function handleCheckboxFieldReadOnlyChange(key, target) {
    if ($('#' + key).is(":checked")) {
        toggleField(true, target);
    }
    else {
        toggleField("readonly", target);
    }

    $('#' + key).click(

      function () {
          if ($('#' + key).is(":checked")) {
              toggleField(true, target);
          }
          else {
              toggleField("readonly", target);
          }
      }
     );
}

function setupToggleTabs() {
    var checkBoxWithToggleTabs = $("span[toggleTabs] > input:checkbox");
    for (var i = 0; i < checkBoxWithToggleTabs.length; i++) {
        var tabsToToggle = checkBoxWithToggleTabs[i].parentElement.getAttribute('toggleTabs').split(',');
        var checkBox = checkBoxWithToggleTabs[i];
        for (var j = 0; j < tabsToToggle.length; j++) {
            handleCheckboxTabVisibleChange(checkBox.id, jQuery.trim(tabsToToggle[j]));
        }
    }
}

function handleCheckboxTabVisibleChange(key, target) {
    if ($('#' + key).is(":checked")) {
        ShowTab(target);
    }
    else {
        HideTab(target);
    }

    $('#' + key).click(

      function () {
          if ($('#' + key).is(":checked")) {
              ShowTab(target);
          }
          else {
              HideTab(target);
          }
      }
     );
}

function setupToggleColumns() {
    var checkBoxWithToggleColumns = $("span[toggleColumns] > input:checkbox");
    for (var i = 0; i < checkBoxWithToggleColumns.length; i++) {
        var tableColumnsToToggle = checkBoxWithToggleColumns[i].parentElement.getAttribute('toggleColumns').split('|');
        var checkBox = checkBoxWithToggleColumns[i];
        for (var j = 0; j < tableColumnsToToggle.length; j++) {
            var cleanString = jQuery.trim(tableColumnsToToggle[j]);
            var startPosition = cleanString.indexOf('[');
            var endPosition = cleanString.indexOf(']');
            var tableName = cleanString.substring(0, startPosition);
            var columns = cleanString.substring(startPosition + 1, endPosition).replace(' ', '');
            handleCheckboxColumnVisibleChange(checkBox.id, tableName, columns.split(','));
        }
    }
}

function handleCheckboxColumnVisibleChange(key, tableName, columns) {
    if ($('#' + key).is(":checked")) {
        $('#' + tableName).showColumns(columns);
    }
    else {
        $('#' + tableName).hideColumns(columns);
    }

    $('#' + key).click(
      function () {
          $('#' + tableName).toggleColumns(columns);
      }
     );
}

/// <summary>
/// Calculates the sum for given controls and assignes the result to 'ControlToShowResult'
/// </summary>
/// <param name="ControlToShowResult">The control to get show result</param>
/// <param name="fieldNames">Field that should be used to get total</param>
/// <example>getTotal('MF__FLEET_PREMIUM','MF__NUM_UNITS','MF__PREMIUM_PER_UNIT')</example>
function getTotal(ControlToShowResult) {
    var TotalAmount = 0;
    for (var i = 1; i < getTotal.arguments.length; i++) {
        if (getTotal.arguments[i] != '') {
            var control = $("[id$='" + getTotal.arguments[i] + "']");
            for (var j = 0; j < control.length; j++) {
                if ($(control[j]).val() != '') {
                    TotalAmount = TotalAmount + parseFloat(cleanNumber($(control[j]).val()));
                }
            }
        }
    }

    var control = $("[id$='" + ControlToShowResult + "']");
    for (var j = 0; j < control.length; j++) {
        $(control[j]).val(TotalAmount);
        if ($(control[j]).attr("class") == "e-num2") { $(control[j]).format({ format: e_num2Fmt, locale: "us" }); }
        else if ($(control[j]).attr("class") == "e-num0") { $(control[j]).format({ format: e_num0Fmt, locale: "us" }); }
    }
}

/// <summary>
/// Calculates the multiplication for given controls and assignes the result to 'ControlToShowResult'
/// </summary>
/// <param name="ControlToShowResult">The control to get show result</param>
/// <param name="fieldNames">Field that should be used to get multiplication</param>
/// <example>getMultiply('MF__FLEET_PREMIUM','MF__NUM_UNITS','MF__PREMIUM_PER_UNIT')</example>
function getMultiply(ControlToShowResult) {
    var CalculatedAmount = 0;
    for (var i = 1; i < getMultiply.arguments.length; i++) {
        if (getMultiply.arguments[i] != '') {
            var control = $("[id$='" + getMultiply.arguments[i] + "']");
            for (var j = 0; j < control.length; j++) {
                if ($(control[j]).val() != '') {
                    if (i == 1) { CalculatedAmount = parseFloat(cleanNumber($(control[j]).val())) }
                    else { CalculatedAmount = CalculatedAmount * parseFloat(cleanNumber($(control[j]).val())) }
                }
            }
        }
    }

    var control = $("[id$='" + ControlToShowResult + "']");
    for (var j = 0; j < control.length; j++) {
        $(control[j]).val(CalculatedAmount);

        if ($(control[j]).attr("class") == "e-num2") { $(control[j]).format({ format: e_num2Fmt, locale: "us" }); }
        else if ($(control[j]).attr("class") == "e-num0") { $(control[j]).format({ format: e_num0Fmt, locale: "us" }); }
    }
}

function isInteger(e) {
    var key = window.event ? e.keyCode : e.which;
    var keychar = String.fromCharCode(key);
    reg = /\d/;
    return reg.test(keychar);
}


function DisableAllControls() {
    var text = "";
    if (document.getElementById('ctl00_cntMainBody_hvMTC') != null) {
        document.getElementById('ctl00_cntMainBody_hvMTC').value = 1;
    }
    $('input:text').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            this.readOnly = true;
        }
    });
    $('input:hidden').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            if (this.id == 'ctl00_cntMainBody_hdnIsReadOnly') {
                document.getElementById('ctl00_cntMainBody_hdnIsReadOnly').value = true;
            }
        }
    });
    $('select').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            if (this.id != 'ctl00_cntMainBody_GPIT__ADDRESS_CNT_GISAddress_Country') {
                this.disabled = true;
            }
        }
    });
    $(".table-wrapper a").each(function () {
        if (this.innerText == "Delete") {
            $(this).hide();
        }
        else if (this.innerText == "Edit") {
            this.innerText = "View";
        }
    });

    $(".table-wrapper .linkbutton").each(function () {
        $(".table-wrapper .linkbutton").hide();
    });
    $('img').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            this.onclick = "";
        }
    });
    $('textarea').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            this.readOnly = true;
        }
    });
    $('input:submit').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            if (!(this.id.indexOf('btnNext') != -1 || this.id.indexOf('btnBack') != -1 || this.id.indexOf('btnFinish') != -1)) {
                this.disabled = true;
            }
        }
    });
    $('input:radio').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            this.disabled = true;
        }
    });
    $('input:checkbox').each(function () {
        if (this.id.indexOf('cntMainBody') != -1) {
            (this).disabled = true;
        }
    });
}
//This method is written to enable disabled checkboxes during MTC postback, so that checkboxes data is posted to the server
function EnableCheckBoxes() {

    if (document.getElementById('ctl00_hvMTC') != null) {
        if (document.getElementById('ctl00_hvMTC').value == 1) {
            $('input:checkbox').each(function () {
                if (this.id.indexOf('cntMainBody') != -1) {
                    (this).disabled = false;
                }
            });
            $('input:radio').each(function () {
                if (this.id.indexOf('cntMainBody') != -1) {
                    (this).disabled = false;
                }
            });
        }
    }
}


function configPercentage(byControl, e_format) {
    $(byControl).each(function () {
        $(this).numeric(".");

        $(this).focus(function () {
            $(this).select();
        });



        $(this).keypress(function (event) {
            if ($(this).val().split('.').length > 1) {
                var strNumber = $(this).val();
                var strAfterDecimal = strNumber.split('.')[1];
                var iCursorPosition = $(this).caret().start;
                if (($(this).val().indexOf('%')) > 0) {
                    if (strAfterDecimal.length > 4 && iCursorPosition > $(this).val().indexOf('.')) {
                        event.preventDefault();
                    }
                }
                else {
                    if (strAfterDecimal.length > 3 && iCursorPosition > $(this).val().indexOf('.')) {
                        event.preventDefault();
                    }
                }
            }
        });
    });
    $(byControl).css('text-align', 'right');
}

$(function () {
    $('.two-digits').keyup(function () {
        if ($(this).val().indexOf('.') != -1) {
            if ($(this).val().split(".")[1].length > 2) {
                if (isNaN(parseFloat(this.value))) return;
                var sAfterDec = parseInt($(this).val().split(".")[1]).toString().substring(0, 2);
            }
            if (($(this).val().split(".")[1].length != 0) && ($(this).val().split(".")[1].length <= 2)) {
                var sAfterDec = parseInt($(this).val().split(".")[1]).toString();
            }
            if ($(this).val().split(".")[1].length == 0) {
                var sAfterDec = "";
            }

            if ($(this).val().split(".")[0].length > 12) {
                var sBforeDec = parseInt($(this).val().split(".")[0]).toString().substring(0, 12);
            }
            if ($(this).val().split(".")[0].length <= 12) {
                var sBforeDec = parseInt($(this).val().split(".")[0]).toString()
            }
            this.value = sBforeDec + '.' + sAfterDec
        }
        if ($(this).val().indexOf('.') == -1) {
            if ($(this).val().length > 12) {
                this.value = parseInt($(this).val().toString().substring(0, 12));
            }

        }
        return this; //for chaining
    });
});

$(function () {
    $('.two-digits').keyup(function () {
        if ($(this).val().indexOf('.') != -1) {
            if ($(this).val().split(".")[1].length > 2) {
                if (isNaN(parseFloat(this.value))) return;
                var sAfterDec = parseInt($(this).val().split(".")[1]).toString().substring(0, 2);
            }
            if (($(this).val().split(".")[1].length != 0) && ($(this).val().split(".")[1].length <= 2)) {
                var sAfterDec = parseInt($(this).val().split(".")[1]).toString();
            }
            if ($(this).val().split(".")[1].length == 0) {
                var sAfterDec = "";
            }

            if ($(this).val().split(".")[0].length > 12) {
                var sBforeDec = parseInt($(this).val().split(".")[0]).toString().substring(0, 12);
            }
            if ($(this).val().split(".")[0].length <= 12) {
                var sBforeDec = parseInt($(this).val().split(".")[0]).toString()
                if (sBforeDec == "NaN") {
                    sBforeDec = 0
                }

            }
            this.value = sBforeDec + '.' + sAfterDec
        }
        if ($(this).val().indexOf('.') == -1) {
            if ($(this).val().length > 12) {
                this.value = parseInt($(this).val().toString().substring(0, 12));
            }

        }
        return this; //for chaining
    });
});

// Code add by vipin for bootstrap UI

$(document).ready(function () {
    $('a').on('mousedown', function (e) {
        e.preventDefault();
    });

    $('[data-toggle="tooltip"]').tooltip()
    $('textarea[data-toggle="tooltip"]').keyup(function () {
        $(this).attr('data-original-title', $(this).val());
    });
    //if ($('.grid').offset() != undefined) {
    //    $("html, body").animate({
    //        scrollTop: $('.grid').offset().top
    //    }, 1000);
    //}
});

function HideGridMenuInCaseOfNoButtons() {
    $('.rowMenu').each(function () {
        if ($(this).find('a[data-bs-toggle^="dropdown"]').length > 0) {
            if ($(this).find('a').length <= 1) {
                $(this).hide();
            }
        }
    });
}
//function checkUncheckFolderMenu(ctrl) {
//    if (ctrl.checked == true) {
//        setCookie('FolderMenu', 'On', 1)
//        if (document.getElementById("aside") != undefined) {
//            document.getElementById("aside").className = "app-aside modal folded fade";
//        }
//    }
//    else {
//            setCookie('FolderMenu', 'Off', 1)
//            if (document.getElementById("aside") != undefined) {
//                document.getElementById("aside").className = "app-aside modal fade";
//            }
//    }
//}
//$(document).ready(function () {
//    if (getCookie('FolderMenu') == 'On') {
//        $('#chkFoldedMenu').prop('checked', true);
//        if (document.getElementById("aside") != undefined) {
//            document.getElementById("aside").className = "app-aside modal folded fade";
//        }
//    } else {
//            $('#chkFoldedMenu').prop('checked', false);
//            if (document.getElementById("aside") != undefined) {
//                document.getElementById("aside").className = "app-aside modal fade";
//            }
//    }
//});

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}