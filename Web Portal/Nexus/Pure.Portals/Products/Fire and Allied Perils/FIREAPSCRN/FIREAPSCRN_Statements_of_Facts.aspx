<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="FIREAPSCRN_Statements_of_Facts.aspx.vb" Inherits="Nexus.PB2_FIREAPSCRN_Statements_of_Facts" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/BOCoverDates.ascx" TagName="CoverDate" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/SubAgents.ascx" TagName="SubAgents" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/Contacts.ascx" TagName="Contact" TagPrefix="uc5" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc6" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Src="~/Controls/StandardWordings.ascx" TagName="SW" TagPrefix="uc7" %>
<%@ Register Src="~/Controls/FindParty.ascx" TagName="FindParty" TagPrefix="NexusControl" %>
<%@ Register Src="~/Controls/VehicleLookup.ascx" TagName="VehicleLookup" TagPrefix="uc8" %> 
<%@ Register Src="~/Controls/Geocoding.ascx" TagName="Geocoding" TagPrefix="uc9" %> 

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
<div class="itl">
    <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server" />
	<script type="text/javascript">
		window['XMLDataSet'] = '<asp:Literal ID="XMLDataSet" runat="server"></asp:Literal>';
		window['TransactionType'] = '<%=GetTransactionType()%>';
		window['PolicyNumber'] = '<%=CType(Session.Item(Nexus.Constants.Session.CNQuote), NexusProvider.Quote).InsuranceFileRef %>';
		window['CurrencyCode'] = '<%=CType(Session.Item(Nexus.Constants.Session.CNQuote), NexusProvider.Quote).CurrencyCode %>';
		window['PolicyCurrencyCode'] = '<%=CType(Session.Item(Nexus.Constants.Session.CNParty), NexusProvider.BaseParty).Currency %>';
		window['NoCurrencySymbols'] = true;
		window['ThisOI'] = '<asp:Literal ID="ThisOI" runat="server"></asp:Literal>';
		<% If CType(Session.Item(Nexus.Constants.CNMode), Nexus.Constants.Mode) = Nexus.Constants.Mode.View Or CType(Session.Item(Nexus.Constants.CNMode), Nexus.Constants.Mode) = Nexus.Constants.Mode.Review Then %>
		window["rulesDisabled"] = true;
		<% End If %>
	</script>
	
	<!-- ITL Externals -->
	<script src="<%=ResolveUrl("~/App_Themes/Standard/js/es5-shim.min.js")%>" type="text/javascript"></script>
	<script src="<%=ResolveUrl("~/App_Themes/Standard/js/es6-shim.min.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/App_Themes/Standard/js/closure-v1.5.1.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/App_Themes/Standard/js/buildComponents-v1.5.1.js")%>" type="text/javascript"></script>
    <link href="<%=ResolveUrl("~/App_Themes/Standard/css/closure-v1.5.1.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~/App_Themes/Standard/internal-differences.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=ResolveUrl("~/App_Themes/Standard/internal-differences-addon-3-to-3.1.css")%>" rel="stylesheet" type="text/css" />
	<!-- /ITL Externals -->
	
	<script type="text/javascript">
		perseus.identifiers.loginName = '<%= Session(Nexus.Constants.CNLoginName).ToString()%>';

		/**
		 * Enables or disables the next button on the current tab.
		 * @param {boolean} value Disable or enable next
		 */
		var enableNext = function(value){
			var btnNext = document.getElementById('ctl00_cntMainBody_btnNext');
			var btnNextTop = document.getElementById('ctl00_cntMainBody_btnNextTop');
			if (value){
				if (btnNext) btnNext.removeAttribute("disabled");
				if (btnNextTop) btnNextTop.removeAttribute("disabled");
			} else {
				if (btnNext) btnNext.setAttribute("disabled", "disabled");
				if (btnNextTop) btnNextTop.setAttribute("disabled", "disabled");
			}
		}
	</script>

	<script type="text/javascript">
		var scrollPositionHandler = {};
		(function(){
		
			var cookies = new goog.net.Cookies(document);
		
			/**
			 * Gets the current page filename.
			 * @return {string}
			 * @private
			 */
			this.getFileName = function(){
				var url = window.location.href;
				return url.slice(url.lastIndexOf("/") + 1);
			};
			
			/**
			 * Get the query data
			 * @return {goog.net.QueryData}
			 * @private
			 */
			this.getQueryData = function(){
				// Check it's retrieving scroll positions for the same policy
				if (cookies.get("lastPolicyNumber") != '<%=CType(Session.Item(Nexus.Constants.Session.CNQuote), NexusProvider.Quote).InsuranceFileRef %>'){
					cookies.remove("scrollPosTracker");
					cookies.remove("lastPolicyNumber");
					return new goog.Uri.QueryData();
				}
				return this.queryData_ || new goog.Uri.QueryData(cookies.get("scrollPosTracker"));
			};
			
			/**
			 * Update the scroll position cookie record for the current page 
			 * @public
			 */
			this.updateCookieScrollPos = function(){
				var url = this.getFileName();
				var queryData = this.getQueryData();
				var intY = document.getElementById("divMain").scrollTop;
				queryData.set(url, intY);
				cookies.set("scrollPosTracker", queryData.toString());
				cookies.set("lastPolicyNumber", '<%=CType(Session.Item(Nexus.Constants.Session.CNQuote), NexusProvider.Quote).InsuranceFileRef %>');
			};
			
			/**
			 * Update the scroll position cookie record for the current page 
			 * @public
			 */
			this.clearCookieScrollPos = function(){
				var url = this.getFileName();
				var queryData = this.getQueryData();
				var intY = 0;
				queryData.set(url, intY);
				cookies.set("scrollPosTracker", queryData.toString());
				cookies.set("lastPolicyNumber", '<%=CType(Session.Item(Nexus.Constants.Session.CNQuote), NexusProvider.Quote).InsuranceFileRef %>');
			};
			
			/**
			 * Set the page scroll position based on data in the cookie.
			 * @public
			 */
			 this.setPageScrollPosition = function(){
				var url = this.getFileName();
				var queryData = this.getQueryData();
				var intY = queryData.get(url);
				if(intY != null)
					document.getElementById("divMain").scrollTop = intY;
			 };
		}).call(scrollPositionHandler);
		
		
		function GetLastDivPosition() {
			scrollPositionHandler.setPageScrollPosition();
		};
		
		function SetDivPosition() {
			scrollPositionHandler.updateCookieScrollPos();
		};
		
		function clearCookieScrollPos() {
			scrollPositionHandler.clearCookieScrollPos();
		};
		
	</script>
	
	
	
    <script type="text/javascript">
        /**
         * @fileoverview
         * GeneralValidation.h
         */ 
        	var GeneralValidationHandler = {};
        
        	GeneralValidationHandler.isDate = function(value, sepVal, dayIdx, monthIdx, yearIdx) {
        		try {
        			//Change the below values to determine which format of date you wish to check. It is set to dd/mm/yyyy by default.
        			var DayIndex = dayIdx !== undefined ? dayIdx : 0; 
        			var MonthIndex = monthIdx !== undefined ? monthIdx : 1;
        			var YearIndex = yearIdx !== undefined ? yearIdx : 2;
        	 
        			value = value.replace(/-/g, "/").replace(/\./g, "/"); 
        			var SplitValue = value.split(sepVal || "/");
        			var OK = true;
        			if (!(SplitValue[DayIndex].length == 1 || SplitValue[DayIndex].length == 2)) {
        				OK = false;
        			}
        			if (OK && !(SplitValue[MonthIndex].length == 1 || SplitValue[MonthIndex].length == 2)) {
        				OK = false;
        			}
        			if (OK && SplitValue[YearIndex].length != 4) {
        				OK = false;
        			}
        			if (OK) {
        				var Day = parseInt(SplitValue[DayIndex], 10);
        				var Month = parseInt(SplitValue[MonthIndex], 10);
        				var Year = parseInt(SplitValue[YearIndex], 10);
        	 
        				if (OK = ((Year > 1900) && (Year < (100 + new Date().getFullYear())))) {
        					if (OK = (Month <= 12 && Month > 0)) {
        
        						var LeapYear = (((Year % 4) == 0) && ((Year % 100) != 0) || ((Year % 400) == 0));   
        						
        						if(OK = Day > 0)
        						{
        							if (Month == 2) {  
        								OK = LeapYear ? Day <= 29 : Day <= 28;
        							} 
        							else {
        								if ((Month == 4) || (Month == 6) || (Month == 9) || (Month == 11)) {
        									OK = Day <= 30;
        								}
        								else {
        									OK = Day <= 31;
        								}
        							}
        						}
        					}
        				}
        			}
        			return OK;
        		}
        		catch (e) {
        			return false;
        		}
        	} 
        	
        	GeneralValidationHandler.Validate = function(isOnLoad, args, obj, prop, type){
        		type = type.toLowerCase();
        		var node = document.getElementById("ctl00_cntMainBody_val" + obj.toUpperCase() + "__" + prop);
        		if (node == null){
        			// Try slightly different format with only one underscore
        			node = document.getElementById("ctl00_cntMainBody_val" + obj.toUpperCase() + "_" + prop);
        		}
        
        		if (isOnLoad) {
        			//
        			//	Add a blur event to call validation function
        			if (node != null) {
        				if (node.addEventListener) {
        					node.addEventListener('blur', function() { window['onValidate_' + obj + '__' + prop](null, null, this); } );
        				} else {
        					node.attachEvent('onblur', function() { window['onValidate_' + obj + '__' + prop](null, null, this); } );
        				}
        			}
        		} else {
        			// Ensure the field contents is the correct format on exit from the field
        			var field = Field.getInstance(obj, prop);
        			var errorMessage = null;
        			var isValid = true;
        			
        			if (type == "integer") {
        				var value = field.getValue();
        				
        				if (value != null && value != '') {
        					var regExp = /^-?[0-9]+$/;
        
        					if (regExp.test(value)){
        						if (value <= -1000000000 || value >= 1000000000){
        							errorMessage = "Is out of range";
        							isValid = false;
        						}
        					} else {
        						isValid = false;
        						errorMessage = "Must be numeric";
        					}
        				}
        			}
        
        			if (type == "date") {
        				var date = field.getValue();
        				//if ((date instanceof Date) && window.isNaN(date.getTime())) {
        				///if (date.length > 0 && !GeneralValidationHandler.isDate(date, "/", 0, 1, 2)) {
        				if (! date instanceof Date){
        					isValid = false;
        					errorMessage = "Must be a valid date";
        				}
        			}
        
        			if (type == "currency") {
        				var value = field.getValue();
        				if (value <= -1000000000000000 || value >= 1000000000000000){
        					errorMessage = "Is out of range";
        					isValid = false;
        				}
        			}
        
        			if (type == "percentage") {
        				var value = field.getValue();
        				if (value <= -1000 || value >= 1000){
        					errorMessage = "Is out of range";
        					isValid = false;
        				}
        			}
        			
        			if (type == "text") {
        				var value = field.getValue();
        				if (value.length > 255){
        					errorMessage = "Too many characters";
        					isValid = false;
        				}
        			
        			}
        			
        			if (!isValid) {
        				if (node != null){
        					var lbl = document.getElementById('ctl00_cntMainBody_lbl' + obj + '_' + prop);
        					if (lbl != null) {
        						node.errormessage = lbl.innerHTML + " -- " + errorMessage;
        						if (args && args.IsValid == true){
        							args.IsValid = false;
        						}
        					}
        				}
        			}
        		}
        	}
        
        
        String.prototype.killWhiteSpace = function() {
            return this.replace(/\s/g, '');
        };
        
        String.prototype.reduceWhiteSpace = function() {
            return this.replace(/\s+/g, ' ');
        };
        
        var SetControlProperties = function( field, setControl, strFeaturesOn, strFeaturesOff, paramValidationMessage ){
        
        	var strFeaturesToUse
        
        	if (setControl == true){
        		strFeaturesToUse = strFeaturesOn;
        		
        		// Don't do anything if no features are provided, i.e. empty parameter
        		if (strFeaturesOn == "")
        			return;
        	} else {               
        		strFeaturesToUse = strFeaturesOff;
        	}
        
        
        	// L = Leave the attributes only
        	if (strFeaturesToUse.toLowerCase().indexOf("l") == -1
        		&& strFeaturesToUse.toLowerCase().indexOf("n") == -1) {
        	
        		if (strFeaturesToUse.toLowerCase().indexOf("e") != -1){
        			field.setReadOnly(false);
        		} else {
        			field.setReadOnly(true);
        		};
        		   
        		if (strFeaturesToUse.toLowerCase().indexOf("v") != -1){
        			field.setVisible(true);
        		} else {
        			field.setVisible(false);
        		}   
        		   
        		if (strFeaturesToUse.toLowerCase().indexOf("m") != -1){
        			field.setMandatory(true, paramValidationMessage);
        			if (strFeaturesToUse.indexOf("m") != -1){
        				// If a lower case m, 0 is not valid
        				var exp = new Expression(field.getObjectName() + "." + field.getPropertyName() + " == 0")
        				
        				goog.events.listen(exp, "change", function(){
        					if (exp.getValue() == true){
        						field.setValid(false);
        					} else {
        						field.setValid(true);
        					}
        				}, false, this);
        			}
        		} else {
        			field.setMandatory(false);
        		}   
        		
        		if (strFeaturesToUse.toLowerCase().indexOf("r") != -1){
        			field.setHidden(true);
        		} else {
        			field.setHidden(false);
        		}   
        
        		if (strFeaturesToUse.toLowerCase().indexOf("h") != -1){
        			field.setMandatory(false);
        			field.setVisible(false);
        			field.setReadOnly(true);
        		} 
        
                
        
        
        	}
        };
        
        
        
        var NormaliseCurrencyString = function(theString) {
        	
        	// Force to string
        	theString = "" + theString;
        
            var strValidChars, strNormalised, i, strChar;
        
        	strValidChars  = "0123456789.";
        	strNormalised = "";
        
        	for (var i = 0; i < theString.length; i++){
        		strChar = theString.slice(i, 1);
        		if (strValidChars.indexOf(strChar) != -1){
        			strNormalised += strChar;
        		}
        	}
        
        	if (theString.length == 0)
                strNormalised = "0";
             
        
            return strNormalised; 
        };
        
        /**
         * Get the column total.
         * @param screenObjectStr The screen object name
         * @param strObject The object string of the column to total
         * @param strProperty The property string of the column to total
         */
        var getColumnTotal = function(screenObjectStr, strObject, strProperty, condition){
        
        	var root = new XMLDataSetReader(window.XMLDataSet);
        
        	var childScreenObject = root.getObjects(screenObjectStr)[0];
        	if (childScreenObject == null)
        		return 0;
        	var childObjects = childScreenObject.getObjects(strObject);
        	
        	var total = 0;
        	for (var i = 0; i < childObjects.length; i++){
        		if (condition != null){
        			var conditionExpression = new Expression(condition);
        		} else {
        			total += window.parseFloat(childObjects[i].getPropertyValue(strProperty)) || 0;
        		}
        	};
        	return total;
        };
        /**
         * @fileoverview
         * Set property
         */
        window.setProperty = function(field, value, condition, elseValue, validationMessage){
        		
        		
        	var paramValue = value,
        		paramCondition = condition,
        		paramElseValue = elseValue,
        		paramValidationMessage = validationMessage;
        		
        	paramValidationMessage = (Expression.isValidParameter(paramValidationMessage)) ? paramValidationMessage : undefined;
        	
        	
        	
        	if (paramValue != ""){
        		var paramValueExpression = new Expression(paramValue);
        	}
        	if (Expression.isValidParameter(paramCondition)){
        		// Check for condition
        		
        		var condition = new Expression(paramCondition);
        		var update = function(){
        			paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
        			var value = condition.getValue();
        			if (value == true){
        				SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
        			} else if (Expression.isValidParameter(paramElseValue)){
        				if (paramElseValue != "U") {
        					SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
        				}
        			} else {
        				// No else value provided
        				// Set field to not visible/ non editable/ non mandatory
        				field.setVisible(false);
        				field.setMandatory(false);
        				field.setReadOnly(true);
        			}
        		};
        		events.listen(condition, "change", update);
        		update();
        	} else {
        		// Set to the value
        		paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
        		SetControlProperties(field, true, paramValue, undefined, paramValidationMessage);
        	}
        };
        /**
         * Set the control width
         */
        window.setControlWidth = function(field, width, obj, prop){
        	
        	// width sanitisation
        	if (typeof width == "string" 
        		&& ((width.slice(0,1) == "'" && width.slice(-1) == "'") || (width.slice(0,1) == "\"" || width.slice(-1) == "\""))){
        		
        		// As there is loads of rules in the spreadsheet in this format correcting this now will throw
        		// all the sizes of fields out of sync to what they were before. So to keep the same
        		// behaviour without erroring will exit out here instead.
        		return;
        		//width = width.slice(1, -1);
        	}
        	if (typeof width == "string"){
        		width = window.parseFloat(width);
        		if (width == NaN)
        			return; // Don't continue
        	}
        	
        	
        	// If the control supports setWidth, use that else fall back on other method
        	// for older controls
        	// TO keep resizing consistent we will hard code a standard width
        	var standardWidth = 165;
        	//if (field.setWidth && field.getWidth){
        	if (field.setWidth){
        		field.setWidth(standardWidth * width);
        	}
        	
        	
        	// Fall back for older fields
        	var ele = document.getElementById('ctl00_cntMainBody_' + obj + '__' + prop);
        	//var bounds = window.getBounds(ele);
        	var widthPx = Math.round(width * standardWidth);
        	if (ele != null)
        		ele.style.width = ((widthPx > 790) ? 790 : widthPx) + "px";
        	// Check for text area also
        	var textarea = document.getElementById('ctl00_cntMainBody_' + obj + '_' + prop + '_textarea');
        	if (textarea != null){
        		//bounds = window.getBounds(textarea);
        		textarea.style.width = ((widthPx > 790) ? 790 : widthPx) + "px";
        	}
        	
        };
function onValidate_STATEMENTS__Engange_Hazardous(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Engange_Hazardous", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Engange_Hazardous");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Hazardous_Details(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Hazardous_Details", "Text");
        })();
        /**
         * @fileoverview
         * Set property
         */
        (function(){
        	if (isOnLoad) {	
        		var field
        		if ("{name}" != "{na" + "me}"){
        			field = Field.getLabel("{name}");
        		} else { 
        			field = Field.getInstance("STATEMENTS", "Hazardous_Details");
        		}
        		//window.setProperty(field, "VEM", "STATEMENTS.Engange_Hazardous == true ", "R", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "STATEMENTS.Engange_Hazardous == true ",
            paramElseValue = "R",
            paramValidationMessage = "{3}";
            
            paramValidationMessage = (Expression.isValidParameter(paramValidationMessage)) ? paramValidationMessage : undefined;
            
            if (paramValue != ""){
              var paramValueExpression = new Expression(paramValue);
            }
            if (Expression.isValidParameter(paramCondition)){
              // Check for condition
              
              var condition = new Expression(paramCondition);
              var update = function(){
                paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
                var value = condition.getValue();
                if (value == true){
                  SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                } else if (Expression.isValidParameter(paramElseValue)){
                  if (paramElseValue != "U") {
                    SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                  }
                } else {
                  // No else value provided
                  // Set field to not visible/ non editable/ non mandatory
                  field.setVisible(false);
                  field.setMandatory(false);
                  field.setReadOnly(true);
                }
              };
              events.listen(condition, "change", update);
              update();
            } else {
              // Set to the value
              paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
              SetControlProperties(field, true, paramValue, undefined, paramValidationMessage);
            }
        
        	}
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Hazardous_Details");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("STATEMENTS.Hazardous_Details");
        			window.setControlWidth(field, "1.75", "STATEMENTS", "Hazardous_Details");
        		})();
        	}
        })();
}
function onValidate_STATEMENTS__Inflammables(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Inflammables", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Inflammables");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Inflammables_Type(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Inflammables_Type", "Text");
        })();
        /**
         * @fileoverview
         * Set property
         */
        (function(){
        	if (isOnLoad) {	
        		var field
        		if ("{name}" != "{na" + "me}"){
        			field = Field.getLabel("{name}");
        		} else { 
        			field = Field.getInstance("STATEMENTS", "Inflammables_Type");
        		}
        		//window.setProperty(field, "VEM", "STATEMENTS.Inflammables == true ", "R", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "STATEMENTS.Inflammables == true ",
            paramElseValue = "R",
            paramValidationMessage = "{3}";
            
            paramValidationMessage = (Expression.isValidParameter(paramValidationMessage)) ? paramValidationMessage : undefined;
            
            if (paramValue != ""){
              var paramValueExpression = new Expression(paramValue);
            }
            if (Expression.isValidParameter(paramCondition)){
              // Check for condition
              
              var condition = new Expression(paramCondition);
              var update = function(){
                paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
                var value = condition.getValue();
                if (value == true){
                  SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                } else if (Expression.isValidParameter(paramElseValue)){
                  if (paramElseValue != "U") {
                    SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                  }
                } else {
                  // No else value provided
                  // Set field to not visible/ non editable/ non mandatory
                  field.setVisible(false);
                  field.setMandatory(false);
                  field.setReadOnly(true);
                }
              };
              events.listen(condition, "change", update);
              update();
            } else {
              // Set to the value
              paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
              SetControlProperties(field, true, paramValue, undefined, paramValidationMessage);
            }
        
        	}
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Inflammables_Type");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("STATEMENTS.Inflammables_Type");
        			window.setControlWidth(field, "1.75", "STATEMENTS", "Inflammables_Type");
        		})();
        	}
        })();
}
function onValidate_STATEMENTS__Declined_to_Insure(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Declined_to_Insure", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Declined_to_Insure");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Required_Special_Terms(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Required_Special_Terms", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Required_Special_Terms");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Cancelled_Or_Refused(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Cancelled_Or_Refused", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Cancelled_Or_Refused");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Increase_Premium(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Increase_Premium", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Increase_Premium");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Other_Insurance(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Other_Insurance", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Other_Insurance");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Insurance_Type(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Insurance_Type", "Text");
        })();
        /**
         * @fileoverview
         * Set property
         */
        (function(){
        	if (isOnLoad) {	
        		var field
        		if ("{name}" != "{na" + "me}"){
        			field = Field.getLabel("{name}");
        		} else { 
        			field = Field.getInstance("STATEMENTS", "Insurance_Type");
        		}
        		//window.setProperty(field, "VEM", "STATEMENTS.Other_Insurance == true ", "R", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "STATEMENTS.Other_Insurance == true ",
            paramElseValue = "R",
            paramValidationMessage = "{3}";
            
            paramValidationMessage = (Expression.isValidParameter(paramValidationMessage)) ? paramValidationMessage : undefined;
            
            if (paramValue != ""){
              var paramValueExpression = new Expression(paramValue);
            }
            if (Expression.isValidParameter(paramCondition)){
              // Check for condition
              
              var condition = new Expression(paramCondition);
              var update = function(){
                paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
                var value = condition.getValue();
                if (value == true){
                  SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                } else if (Expression.isValidParameter(paramElseValue)){
                  if (paramElseValue != "U") {
                    SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                  }
                } else {
                  // No else value provided
                  // Set field to not visible/ non editable/ non mandatory
                  field.setVisible(false);
                  field.setMandatory(false);
                  field.setReadOnly(true);
                }
              };
              events.listen(condition, "change", update);
              update();
            } else {
              // Set to the value
              paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
              SetControlProperties(field, true, paramValue, undefined, paramValidationMessage);
            }
        
        	}
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Insurance_Type");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("STATEMENTS.Insurance_Type");
        			window.setControlWidth(field, "1.75", "STATEMENTS", "Insurance_Type");
        		})();
        	}
        })();
}
function onValidate_STATEMENTS__Additional_Facts(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Additional_Facts", "Checkbox");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Additional_Facts");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Facts_Details(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Facts_Details", "Text");
        })();
        /**
         * @fileoverview
         * Set property
         */
        (function(){
        	if (isOnLoad) {	
        		var field
        		if ("{name}" != "{na" + "me}"){
        			field = Field.getLabel("{name}");
        		} else { 
        			field = Field.getInstance("STATEMENTS", "Facts_Details");
        		}
        		//window.setProperty(field, "VEM", "STATEMENTS.Additional_Facts == true ", "R", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "STATEMENTS.Additional_Facts == true ",
            paramElseValue = "R",
            paramValidationMessage = "{3}";
            
            paramValidationMessage = (Expression.isValidParameter(paramValidationMessage)) ? paramValidationMessage : undefined;
            
            if (paramValue != ""){
              var paramValueExpression = new Expression(paramValue);
            }
            if (Expression.isValidParameter(paramCondition)){
              // Check for condition
              
              var condition = new Expression(paramCondition);
              var update = function(){
                paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
                var value = condition.getValue();
                if (value == true){
                  SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                } else if (Expression.isValidParameter(paramElseValue)){
                  if (paramElseValue != "U") {
                    SetControlProperties(field, value, paramValue, paramElseValue, paramValidationMessage);
                  }
                } else {
                  // No else value provided
                  // Set field to not visible/ non editable/ non mandatory
                  field.setVisible(false);
                  field.setMandatory(false);
                  field.setReadOnly(true);
                }
              };
              events.listen(condition, "change", update);
              update();
            } else {
              // Set to the value
              paramValue = (paramValueExpression) ? paramValueExpression.getValue() : paramValue;
              SetControlProperties(field, true, paramValue, undefined, paramValidationMessage);
            }
        
        	}
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Facts_Details");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("STATEMENTS.Facts_Details");
        			window.setControlWidth(field, "1.75", "STATEMENTS", "Facts_Details");
        		})();
        	}
        })();
}
function onValidate_STATEMENTS__Business(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Business", "Text");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Business");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__Losses(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "Losses", "Checkbox");
        })();
        /** 
         * ToggleContainer
         * @param frmClaimsGrid The element to toggle
         * @param false True if the element should be toggle'd when the control is unticked instead of ticked.
         * Defaults to false.
         */
        (function(){
        	
        	
        	if (isOnLoad) {
        		var field = Field.getInstance("STATEMENTS","Losses");
        	
        		var inverse = (Expression.isValidParameter("false") && ("false".toLowerCase() == "true")) ? true : false;
        		var update = function(){
        			var value = (field.getValue() != true) ? "false" : "true";
        			
        			if (!inverse){
        				(new Expression("SetElementDisplay('frmClaimsGrid', !!" + value + ")")).valueOf();
        			} else {
        				(new Expression("SetElementDisplay('frmClaimsGrid', !" + value + ")")).valueOf();
        			}
        		};
        		events.listen(Field.getInstance("STATEMENTS", "Losses"), "change", update);
        		update();
        	}
        
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("2");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblSTATEMENTS_Losses");
        			}
        			var bounds = goog.style.getBounds(label);
        			if (bounds.width != 0)
        				standardWidth = bounds.width;
        			
        			//var bounds = window.getBounds(ele);
        			if (label != null)
        				label.style.width = Math.round(width * standardWidth) + "px";
        			
        		}, 4);
        	}
        })();
}
function onValidate_STATEMENTS__FIRECLAIMS(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "STATEMENTS", "FIRECLAIMS", "ChildScreen");
        })();
}
function DoLogic(isOnLoad) {
    onValidate_STATEMENTS__Engange_Hazardous(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Hazardous_Details(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Inflammables(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Inflammables_Type(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Declined_to_Insure(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Required_Special_Terms(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Cancelled_Or_Refused(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Increase_Premium(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Other_Insurance(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Insurance_Type(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Additional_Facts(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Facts_Details(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Business(null, null, null, isOnLoad);
    onValidate_STATEMENTS__Losses(null, null, null, isOnLoad);
    onValidate_STATEMENTS__FIRECLAIMS(null, null, null, isOnLoad);
}
</script>


	
	

        <div id="divMain" onscroll="SetDivPosition()">
           <uc3:ProgressBar ID="ucProgressBar" runat="server" />
            <div class="card">
                <nexus:TabIndex ID="ctrlTabIndex" runat="server" CssClass="TabContainer" TabContainerClass="page-progress"
                    ActiveTabClass="ActiveTab" DisabledClass="DisabledTab" Scrollable="false" />
					<!-- Duplicate buttons -->
					<div class='card-footer clearfix'>
                        <asp:Button ID="refreshCVTop" runat="server" SkinID="buttonSecondary" Style="display: none" />
						<asp:Button ID="btnBackTop" runat="server" Text="Back" OnClick="BackButton" CausesValidation="false" OnClientClick="clearCookieScrollPos()"
                            SkinID="buttonSecondary" />
						<asp:Button ID="btnNextTop" runat="server" Text="Next" OnClick="NextButton" OnClientClick="clearCookieScrollPos()"
                            SkinID="buttonPrimary" disabled="disabled" />	
                        <asp:Button ID="btnFinishTop" runat="server" Text="Finish" OnClick="FinishButton" SkinID="buttonPrimary" OnClientClick="clearCookieScrollPos()"
                            OnPreRender="PreRenderFinish" />
						
					</div>
                    <div class="card-body clearfix">
                  			<div id="inner_content" class="">
								<!-- GeneralLayoutContainer -->
<div id="id76b83eabe8b44dae8c77d463aa3f93e1" class="general-layout-container">
				
         
				
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="idbb5af4ed342b4e3a948a53517f07f2e8" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading16" runat="server" Text="Hazardous and Flammable Materials" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Engange_Hazardous" 
		id="pb-container-checkbox-STATEMENTS-Engange_Hazardous">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Engange_Hazardous" for="ctl00_cntMainBody_STATEMENTS__Engange_Hazardous" class="col-md-4 col-sm-3 control-label">
		Engaged in Hazardous Activities or Materials?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Engange_Hazardous" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Engange_Hazardous" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Engaged in Hazardous Activities or Materials?"
			ClientValidationFunction="onValidate_STATEMENTS__Engange_Hazardous" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="STATEMENTS" 
		data-property-name="Hazardous_Details" 
		 
		
		 
		id="pb-container-text-STATEMENTS-Hazardous_Details">

		
		<asp:Label ID="lblSTATEMENTS_Hazardous_Details" runat="server" AssociatedControlID="STATEMENTS__Hazardous_Details" 
			Text="Please give getails" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="STATEMENTS__Hazardous_Details" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valSTATEMENTS_Hazardous_Details" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Please give getails"
					ClientValidationFunction="onValidate_STATEMENTS__Hazardous_Details"
					ValidationGroup=""
					Display="Dynamic"
					EnableClientScript="true"
					/>
                </div>
					
		
	
		
	</span>
</div>
        
<!-- /Text -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Inflammables" 
		id="pb-container-checkbox-STATEMENTS-Inflammables">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Inflammables" for="ctl00_cntMainBody_STATEMENTS__Inflammables" class="col-md-4 col-sm-3 control-label">
		Do you store any inflammable(s)?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Inflammables" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Inflammables" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Do you store any inflammable(s)?"
			ClientValidationFunction="onValidate_STATEMENTS__Inflammables" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="STATEMENTS" 
		data-property-name="Inflammables_Type" 
		 
		
		 
		id="pb-container-text-STATEMENTS-Inflammables_Type">

		
		<asp:Label ID="lblSTATEMENTS_Inflammables_Type" runat="server" AssociatedControlID="STATEMENTS__Inflammables_Type" 
			Text="If so, state type and contity" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="STATEMENTS__Inflammables_Type" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valSTATEMENTS_Inflammables_Type" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for If so, state type and contity"
					ClientValidationFunction="onValidate_STATEMENTS__Inflammables_Type"
					ValidationGroup=""
					Display="Dynamic"
					EnableClientScript="true"
					/>
                </div>
					
		
	
		
	</span>
</div>
        
<!-- /Text -->
								
									</li>
							
							
						
					</ul>
				
				</div>
				
			
</div>


<script type="text/javascript">
	var labelAlign = "";
	var textAlign = "";
	var labelWidth = "";
	
	var styleString = "";
	if (labelWidth != ""){
		if ((new Expression("IsNumeric('" + labelWidth + "')")).valueOf()){
			labelWidth = labelWidth + "px";
		}
		styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#idbb5af4ed342b4e3a948a53517f07f2e8 input{text-align:left;}"; break;
		}
	}
	
	if (styleString != ""){
		goog.style.installStyles(styleString);
	}
</script>
<!-- /ColumnLayoutContainer -->	
							
							</div>
					 
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="id50f5ebecc2594655aea5401813e75a47" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading17" runat="server" Text="Insurance History" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Label -->
	<span id="pb-container-label-InsuranceQns">
		<span class="label" id="InsuranceQns">Has any Company or Insurer in respect of the Insurance proposed:</span>
	</span>
<!-- /Label -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Declined_to_Insure" 
		id="pb-container-checkbox-STATEMENTS-Declined_to_Insure">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Declined_to_Insure" for="ctl00_cntMainBody_STATEMENTS__Declined_to_Insure" class="col-md-4 col-sm-3 control-label">
		Have you ever been decline to Insure?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Declined_to_Insure" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Declined_to_Insure" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Have you ever been decline to Insure?"
			ClientValidationFunction="onValidate_STATEMENTS__Declined_to_Insure" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Required_Special_Terms" 
		id="pb-container-checkbox-STATEMENTS-Required_Special_Terms">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Required_Special_Terms" for="ctl00_cntMainBody_STATEMENTS__Required_Special_Terms" class="col-md-4 col-sm-3 control-label">
		Required Special Terms?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Required_Special_Terms" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Required_Special_Terms" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Required Special Terms?"
			ClientValidationFunction="onValidate_STATEMENTS__Required_Special_Terms" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Cancelled_Or_Refused" 
		id="pb-container-checkbox-STATEMENTS-Cancelled_Or_Refused">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Cancelled_Or_Refused" for="ctl00_cntMainBody_STATEMENTS__Cancelled_Or_Refused" class="col-md-4 col-sm-3 control-label">
		Cancelled or Refused to renew?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Cancelled_Or_Refused" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Cancelled_Or_Refused" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Cancelled or Refused to renew?"
			ClientValidationFunction="onValidate_STATEMENTS__Cancelled_Or_Refused" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Increase_Premium" 
		id="pb-container-checkbox-STATEMENTS-Increase_Premium">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Increase_Premium" for="ctl00_cntMainBody_STATEMENTS__Increase_Premium" class="col-md-4 col-sm-3 control-label">
		Premium increased at renewal</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Increase_Premium" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Increase_Premium" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Premium increased at renewal"
			ClientValidationFunction="onValidate_STATEMENTS__Increase_Premium" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Other_Insurance" 
		id="pb-container-checkbox-STATEMENTS-Other_Insurance">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Other_Insurance" for="ctl00_cntMainBody_STATEMENTS__Other_Insurance" class="col-md-4 col-sm-3 control-label">
		Do you have any other Insurance here or elsewhere?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Other_Insurance" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Other_Insurance" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Do you have any other Insurance here or elsewhere?"
			ClientValidationFunction="onValidate_STATEMENTS__Other_Insurance" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="STATEMENTS" 
		data-property-name="Insurance_Type" 
		 
		
		 
		id="pb-container-text-STATEMENTS-Insurance_Type">

		
		<asp:Label ID="lblSTATEMENTS_Insurance_Type" runat="server" AssociatedControlID="STATEMENTS__Insurance_Type" 
			Text="If so, state type and Insurers" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="STATEMENTS__Insurance_Type" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valSTATEMENTS_Insurance_Type" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for If so, state type and Insurers"
					ClientValidationFunction="onValidate_STATEMENTS__Insurance_Type"
					ValidationGroup=""
					Display="Dynamic"
					EnableClientScript="true"
					/>
                </div>
					
		
	
		
	</span>
</div>
        
<!-- /Text -->
								
									</li>
							
							
						
					</ul>
				
				</div>
				
			
</div>


<script type="text/javascript">
	var labelAlign = "";
	var textAlign = "";
	var labelWidth = "";
	
	var styleString = "";
	if (labelWidth != ""){
		if ((new Expression("IsNumeric('" + labelWidth + "')")).valueOf()){
			labelWidth = labelWidth + "px";
		}
		styleString += "#id50f5ebecc2594655aea5401813e75a47 label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#id50f5ebecc2594655aea5401813e75a47 label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id50f5ebecc2594655aea5401813e75a47 label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id50f5ebecc2594655aea5401813e75a47 label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#id50f5ebecc2594655aea5401813e75a47 input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id50f5ebecc2594655aea5401813e75a47 input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id50f5ebecc2594655aea5401813e75a47 input{text-align:left;}"; break;
		}
	}
	
	if (styleString != ""){
		goog.style.installStyles(styleString);
	}
</script>
<!-- /ColumnLayoutContainer -->	
							
							</div>
					 
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="frmLossesOthers" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading18" runat="server" Text="Additional Factors and Losses" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Additional_Facts" 
		id="pb-container-checkbox-STATEMENTS-Additional_Facts">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Additional_Facts" for="ctl00_cntMainBody_STATEMENTS__Additional_Facts" class="col-md-4 col-sm-3 control-label">
		Any additional facts affecting this insurance?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Additional_Facts" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Additional_Facts" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Any additional facts affecting this insurance?"
			ClientValidationFunction="onValidate_STATEMENTS__Additional_Facts" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="STATEMENTS" 
		data-property-name="Facts_Details" 
		 
		
		 
		id="pb-container-text-STATEMENTS-Facts_Details">

		
		<asp:Label ID="lblSTATEMENTS_Facts_Details" runat="server" AssociatedControlID="STATEMENTS__Facts_Details" 
			Text="Please give getails" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="STATEMENTS__Facts_Details" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valSTATEMENTS_Facts_Details" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Please give getails"
					ClientValidationFunction="onValidate_STATEMENTS__Facts_Details"
					ValidationGroup=""
					Display="Dynamic"
					EnableClientScript="true"
					/>
                </div>
					
		
	
		
	</span>
</div>
        
<!-- /Text -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="STATEMENTS" 
		data-property-name="Business" 
		 
		
		 
		id="pb-container-text-STATEMENTS-Business">

		
		<asp:Label ID="lblSTATEMENTS_Business" runat="server" AssociatedControlID="STATEMENTS__Business" 
			Text="How long have you conducted business in these premises?" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="STATEMENTS__Business" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valSTATEMENTS_Business" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for How long have you conducted business in these premises?"
					ClientValidationFunction="onValidate_STATEMENTS__Business"
					ValidationGroup=""
					Display="Dynamic"
					EnableClientScript="true"
					/>
                </div>
					
		
	
		
	</span>
</div>
        
<!-- /Text -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="STATEMENTS" 
		data-property-name="Losses" 
		id="pb-container-checkbox-STATEMENTS-Losses">
		<label id="ctl00_cntMainBody_lblSTATEMENTS_Losses" for="ctl00_cntMainBody_STATEMENTS__Losses" class="col-md-4 col-sm-3 control-label">
		Have you ever sustained any loss in respect of the Risk(s) proposed?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="STATEMENTS__Losses" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valSTATEMENTS_Losses" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Have you ever sustained any loss in respect of the Risk(s) proposed?"
			ClientValidationFunction="onValidate_STATEMENTS__Losses" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
					</ul>
				
				</div>
				
			
</div>


<script type="text/javascript">
	var labelAlign = "";
	var textAlign = "";
	var labelWidth = "";
	
	var styleString = "";
	if (labelWidth != ""){
		if ((new Expression("IsNumeric('" + labelWidth + "')")).valueOf()){
			labelWidth = labelWidth + "px";
		}
		styleString += "#frmLossesOthers label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#frmLossesOthers label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmLossesOthers label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmLossesOthers label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#frmLossesOthers input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmLossesOthers input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmLossesOthers input{text-align:left;}"; break;
		}
	}
	
	if (styleString != ""){
		goog.style.installStyles(styleString);
	}
</script>
<!-- /ColumnLayoutContainer -->	
							
							</div>
					 
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="frmClaimsGrid" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading19" runat="server" Text="Previous Detail" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Child -->
	<div class="child-card" id="ctl00_cntMainBody_STATEMENTS__FIRECLAIMS"
		data-field-type="Child" 
		data-object-name="STATEMENTS" 
		data-property-name="FIRECLAIMS" 
		id="pb-container-childscreen-STATEMENTS-FIRECLAIMS">
		
		    <legend>Previous Claims</legend>
		 
			        <div class="grid-card table-responsive no-margin">
						<nexus:ItemGrid ID="STATEMENTS__CLAIMS_DETAILS" runat="server" ScreenCode="FIRECLAIMS" AutoGenerateColumns="false"
							GridLines="None" ChildPage="FIRECLAIMS/FIRECLAIMS_Previous__Details.aspx" emptydatatext="sac">
							<columns>
						<Nexus:RiskAttribute HeaderText="Incident Date" DataField="Incident_Date" DataFormatString="{0:d}"/>
<Nexus:RiskAttribute HeaderText="Incident Short Description" DataField="Incident_Description" DataFormatString=""/>
<Nexus:RiskAttribute HeaderText="Paid Amount" DataField="Paid_Amount" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Outstanding Amount" DataField="OS_Amount" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Total Incurred" DataField="Total_Incurred" DataFormatString="{0:N}"/>

										<%--
										<Nexus:RiskAttribute HeaderText="Vehicle Make" DataField="VEHICLE_MAKE" FilterByControl="txtVehicleMake" />
										<Nexus:RiskAttribute HeaderText="On Date" DataField="ON_DATE" DataFormatString="{0:d}" />
						<Nexus:GISLookupField HeaderText="Cover Type" ListType="UserDefined" ListCode="FLEETCOVER" DataField="CoverType" DataItemValue="key" />
										--%>
							</columns>
						</nexus:ItemGrid>
						<%--<NexusProvider:LookupList ID="Vehicle_Type" runat="server" CssClass="field-medium"
							ListType="UserDefined" ListCode="FLEETTYPE" DataItemValue="Description" DataItemText="Key"
							Visible="false" DefaultText="(Please Select)" />
						<NexusProvider:LookupList ID="CoverType" runat="server" CssClass="field-medium" ListType="UserDefined"
							ListCode="FLEETCOVER" ParentLookupListID="" DataItemValue="Description" DataItemText="Key"
							Visible="false" DefaultText="(Please Select)" />--%>
					</div>
				
					<asp:CustomValidator ID="valSTATEMENTS_FIRECLAIMS" 
						runat="server" 
						Text="*" 
						ErrorMessage="A validation error occurred for Previous Claims"
						ClientValidationFunction="onValidate_STATEMENTS__FIRECLAIMS" 
						Display="Dynamic"
						EnableClientScript="true"/>
	</div>
<!-- /Child -->
								
									</li>
							
							
						
					</ul>
				
				</div>
				
			
</div>


<script type="text/javascript">
	var labelAlign = "";
	var textAlign = "";
	var labelWidth = "";
	
	var styleString = "";
	if (labelWidth != ""){
		if ((new Expression("IsNumeric('" + labelWidth + "')")).valueOf()){
			labelWidth = labelWidth + "px";
		}
		styleString += "#frmClaimsGrid label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#frmClaimsGrid label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmClaimsGrid label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmClaimsGrid label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#frmClaimsGrid input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmClaimsGrid input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmClaimsGrid input{text-align:left;}"; break;
		}
	}
	
	if (styleString != ""){
		goog.style.installStyles(styleString);
	}
</script>
<!-- /ColumnLayoutContainer -->	
							
							</div>
					 
					
				
         
		
</div>


<!-- /GeneralLayoutContainer -->
							</div>
						                     
                    </div>
				    <div class='card-footer clearfix'>
                        <asp:Button ID="refreshCV" runat="server" SkinID="buttonSecondary" Style="display: none" />
						<% 	
							' TODO: Set this condition to hide the button on the first page
							Dim firstPage As Boolean = True
						
							If firstPage %>
                        <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="BackButton" CausesValidation="false" OnClientClick="clearCookieScrollPos()"
                            SkinID="buttonSecondary" />
						<% End If %>
						<asp:Button ID="btnNext" runat="server" Text="Next" OnClick="NextButton" OnClientClick="clearCookieScrollPos()"
                            SkinID="buttonPrimary" disabled="disabled" />
                        <asp:Button ID="btnFinish" runat="server" Text="Finish" OnClick="FinishButton" SkinID="buttonPrimary" OnClientClick="clearCookieScrollPos()"
                            OnPreRender="PreRenderFinish" />
						
					</div>
             </div>
			<asp:ValidationSummary ID="validationSummeryBox" runat="server" DisplayMode="BulletList" HeaderText="Correct the below given errors" EnableClientScript="true" CssClass="validation-summary" />
        </div>
  
</div>
</asp:Content>