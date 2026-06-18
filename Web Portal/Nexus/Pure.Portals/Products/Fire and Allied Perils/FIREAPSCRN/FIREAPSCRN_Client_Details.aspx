<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="FIREAPSCRN_Client_Details.aspx.vb" Inherits="Nexus.PB2_FIREAPSCRN_Client_Details" %>

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
         * Set the value of a field. When any of the passed in expressions
         * change the set value is re-evaluated.
         * @param {pb.fields.AbstractBase} field The field
         * @param {Expression} value The value to give the field
         * @param {Expression} opt_condition If specified the value will 
         * only be set when this evaluates to true.
         * @param {Expression} opt_elseValue If specified this is the value
         * that will be set when the condition evaluates to false, if 
         * omitted then no value will be set on condition false.
         */
        window.setValue = function(field, value, opt_condition, opt_elseValue){
        	
        		// Helper class to handle the condition and else logic.
        		var valueWhen = new ValueWhenHelper(value, opt_condition, opt_elseValue);
        		
        		var update;
        		events.listen(valueWhen, "change", update = function(e){
        			// Don't set a value if one doesn't exist, this occurs if the condition
        			// is false but no else value is provided.
        			var value = valueWhen.valueOf();
        			// null is a valid value
        			if (value === undefined)
        				return;
        			
        			field.setValue(value);
        		}, false, this);
        		update();
        };
function onValidate_CLIENTDETAILS__COMPANY_NAME(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "COMPANY_NAME", "Text");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("CLIENTDETAILS", "COMPANY_NAME");
        		var errorMessage = "{0}";
        		field.setMandatory(true, (Expression.isValidParameter(errorMessage)) ? errorMessage : undefined);
        	};
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.COMPANY_NAME");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "COMPANY_NAME");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_COMPANY_NAME");
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
function onValidate_CLIENTDETAILS__ADDRESS_LINE1(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "ADDRESS_LINE1", "Text");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("CLIENTDETAILS", "ADDRESS_LINE1");
        		var errorMessage = "{0}";
        		field.setMandatory(true, (Expression.isValidParameter(errorMessage)) ? errorMessage : undefined);
        	};
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.ADDRESS_LINE1");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "ADDRESS_LINE1");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_ADDRESS_LINE1");
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
function onValidate_CLIENTDETAILS__ADDRESS_LINE2(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "ADDRESS_LINE2", "Text");
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.ADDRESS_LINE2");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "ADDRESS_LINE2");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_ADDRESS_LINE2");
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
function onValidate_CLIENTDETAILS__ADDRESS_LINE3(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "ADDRESS_LINE3", "Text");
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.ADDRESS_LINE3");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "ADDRESS_LINE3");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_ADDRESS_LINE3");
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
function onValidate_CLIENTDETAILS__ADDRESS_LINE4(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "ADDRESS_LINE4", "Text");
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.ADDRESS_LINE4");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "ADDRESS_LINE4");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_ADDRESS_LINE4");
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
function onValidate_CLIENTDETAILS__POSTCODE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "POSTCODE", "Text");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("CLIENTDETAILS", "POSTCODE");
        		var errorMessage = "{0}";
        		field.setMandatory(true, (Expression.isValidParameter(errorMessage)) ? errorMessage : undefined);
        	};
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.POSTCODE");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "POSTCODE");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_POSTCODE");
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
function onValidate_CLIENTDETAILS__TELEPHONE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "TELEPHONE", "Text");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("CLIENTDETAILS", "TELEPHONE");
        		var errorMessage = "{0}";
        		field.setMandatory(true, (Expression.isValidParameter(errorMessage)) ? errorMessage : undefined);
        	};
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.TELEPHONE");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "TELEPHONE");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_TELEPHONE");
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
function onValidate_CLIENTDETAILS__MOBILE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "MOBILE", "Text");
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.MOBILE");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "MOBILE");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_MOBILE");
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
function onValidate_CLIENTDETAILS__E_MAIL(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "CLIENTDETAILS", "E_MAIL", "Text");
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("CLIENTDETAILS.E_MAIL");
        			window.setControlWidth(field, "1.75", "CLIENTDETAILS", "E_MAIL");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblCLIENTDETAILS_E_MAIL");
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
function onValidate_OTHERDETAILS__Occupation(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "OTHERDETAILS", "Occupation", "Text");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("OTHERDETAILS", "Occupation");
        		var errorMessage = "{0}";
        		field.setMandatory(true, (Expression.isValidParameter(errorMessage)) ? errorMessage : undefined);
        	};
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("OTHERDETAILS.Occupation");
        			window.setControlWidth(field, "1.75", "OTHERDETAILS", "Occupation");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblOTHERDETAILS_Occupation");
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
function onValidate_OTHERDETAILS__Ownership(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "OTHERDETAILS", "Ownership", "RateList");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("OTHERDETAILS", "Ownership");
        		var errorMessage = "{0}";
        		field.setMandatory(true, (Expression.isValidParameter(errorMessage)) ? errorMessage : undefined);
        	};
        })();
        /**
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("OTHERDETAILS.Ownership");
        			window.setControlWidth(field, "1.75", "OTHERDETAILS", "Ownership");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblOTHERDETAILS_Ownership");
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
function onValidate_OTHERDETAILS__OwnershipDetails(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "OTHERDETAILS", "OwnershipDetails", "Comment");
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
        			field = Field.getInstance("OTHERDETAILS", "OwnershipDetails");
        		}
        		//window.setProperty(field, "VEM", "Caption(OTHERDETAILS.Ownership) == 'Other'", "R", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "Caption(OTHERDETAILS.Ownership) == 'Other'",
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
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("OTHERDETAILS.OwnershipDetails");
        			window.setControlWidth(field, "1.75", "OTHERDETAILS", "OwnershipDetails");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblOTHERDETAILS_OwnershipDetails");
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
         * @fileoverview
         * SetValue
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getWithQuery("type=Comment&objectName=OTHERDETAILS&propertyName=OwnershipDetails&name={name}");
        		
        		var value = new Expression("' '"), 
        			condition = (Expression.isValidParameter("Caption(OTHERDETAILS.Ownership) != 'Other'")) ? new Expression("Caption(OTHERDETAILS.Ownership) != 'Other'") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
}
function onValidate_OTHERDETAILS__AssignTP(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "OTHERDETAILS", "AssignTP", "Checkbox");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblOTHERDETAILS_AssignTP");
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
function onValidate_OTHERDETAILS__TPDetails(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "OTHERDETAILS", "TPDetails", "Comment");
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
        			field = Field.getInstance("OTHERDETAILS", "TPDetails");
        		}
        		//window.setProperty(field, "VEM", "OTHERDETAILS.AssignTP == true ", "R", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "OTHERDETAILS.AssignTP == true ",
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
         * Set the control width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		(function(){
              var field = Field.getInstance("OTHERDETAILS.TPDetails");
        			window.setControlWidth(field, "1.75", "OTHERDETAILS", "TPDetails");
        		})();
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
        				var label = document.getElementById("ctl00_cntMainBody_lblOTHERDETAILS_TPDetails");
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
         * @fileoverview
         * SetValue
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getWithQuery("type=Comment&objectName=OTHERDETAILS&propertyName=TPDetails&name={name}");
        		
        		var value = new Expression("' '"), 
        			condition = (Expression.isValidParameter("OTHERDETAILS.AssignTP == false ")) ? new Expression("OTHERDETAILS.AssignTP == false ") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
}
function DoLogic(isOnLoad) {
    onValidate_CLIENTDETAILS__COMPANY_NAME(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__ADDRESS_LINE1(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__ADDRESS_LINE2(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__ADDRESS_LINE3(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__ADDRESS_LINE4(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__POSTCODE(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__TELEPHONE(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__MOBILE(null, null, null, isOnLoad);
    onValidate_CLIENTDETAILS__E_MAIL(null, null, null, isOnLoad);
    onValidate_OTHERDETAILS__Occupation(null, null, null, isOnLoad);
    onValidate_OTHERDETAILS__Ownership(null, null, null, isOnLoad);
    onValidate_OTHERDETAILS__OwnershipDetails(null, null, null, isOnLoad);
    onValidate_OTHERDETAILS__AssignTP(null, null, null, isOnLoad);
    onValidate_OTHERDETAILS__TPDetails(null, null, null, isOnLoad);
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
<div id="idb8bdf319d8b344f29861f701a75f3dc4" class="general-layout-container">
				
         
				
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="id6559531299374bba93ddf45793662976" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading3" runat="server" Text="Client Details" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="COMPANY_NAME" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-COMPANY_NAME">

		
		<asp:Label ID="lblCLIENTDETAILS_COMPANY_NAME" runat="server" AssociatedControlID="CLIENTDETAILS__COMPANY_NAME" 
			Text="Company Name" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__COMPANY_NAME" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_COMPANY_NAME" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Company Name"
					ClientValidationFunction="onValidate_CLIENTDETAILS__COMPANY_NAME"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="ADDRESS_LINE1" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-ADDRESS_LINE1">

		
		<asp:Label ID="lblCLIENTDETAILS_ADDRESS_LINE1" runat="server" AssociatedControlID="CLIENTDETAILS__ADDRESS_LINE1" 
			Text="Address Line 1" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE1" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_ADDRESS_LINE1" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Address Line 1"
					ClientValidationFunction="onValidate_CLIENTDETAILS__ADDRESS_LINE1"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="ADDRESS_LINE2" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-ADDRESS_LINE2">

		
		<asp:Label ID="lblCLIENTDETAILS_ADDRESS_LINE2" runat="server" AssociatedControlID="CLIENTDETAILS__ADDRESS_LINE2" 
			Text="Address Line 2" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE2" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_ADDRESS_LINE2" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Address Line 2"
					ClientValidationFunction="onValidate_CLIENTDETAILS__ADDRESS_LINE2"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="ADDRESS_LINE3" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-ADDRESS_LINE3">

		
		<asp:Label ID="lblCLIENTDETAILS_ADDRESS_LINE3" runat="server" AssociatedControlID="CLIENTDETAILS__ADDRESS_LINE3" 
			Text="Address Line 3" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE3" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_ADDRESS_LINE3" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Address Line 3"
					ClientValidationFunction="onValidate_CLIENTDETAILS__ADDRESS_LINE3"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="ADDRESS_LINE4" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-ADDRESS_LINE4">

		
		<asp:Label ID="lblCLIENTDETAILS_ADDRESS_LINE4" runat="server" AssociatedControlID="CLIENTDETAILS__ADDRESS_LINE4" 
			Text="Address Line 4" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE4" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_ADDRESS_LINE4" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Address Line 4"
					ClientValidationFunction="onValidate_CLIENTDETAILS__ADDRESS_LINE4"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="POSTCODE" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-POSTCODE">

		
		<asp:Label ID="lblCLIENTDETAILS_POSTCODE" runat="server" AssociatedControlID="CLIENTDETAILS__POSTCODE" 
			Text="Postal Code" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__POSTCODE" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_POSTCODE" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Postal Code"
					ClientValidationFunction="onValidate_CLIENTDETAILS__POSTCODE"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="TELEPHONE" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-TELEPHONE">

		
		<asp:Label ID="lblCLIENTDETAILS_TELEPHONE" runat="server" AssociatedControlID="CLIENTDETAILS__TELEPHONE" 
			Text="Telephone - Office" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__TELEPHONE" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_TELEPHONE" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Telephone - Office"
					ClientValidationFunction="onValidate_CLIENTDETAILS__TELEPHONE"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="MOBILE" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-MOBILE">

		
		<asp:Label ID="lblCLIENTDETAILS_MOBILE" runat="server" AssociatedControlID="CLIENTDETAILS__MOBILE" 
			Text="Mobile No." CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__MOBILE" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_MOBILE" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Mobile No."
					ClientValidationFunction="onValidate_CLIENTDETAILS__MOBILE"
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
		
		data-object-name="CLIENTDETAILS" 
		data-property-name="E_MAIL" 
		 
		
		 
		id="pb-container-text-CLIENTDETAILS-E_MAIL">

		
		<asp:Label ID="lblCLIENTDETAILS_E_MAIL" runat="server" AssociatedControlID="CLIENTDETAILS__E_MAIL" 
			Text="E-mail Address" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="CLIENTDETAILS__E_MAIL" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valCLIENTDETAILS_E_MAIL" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for E-mail Address"
					ClientValidationFunction="onValidate_CLIENTDETAILS__E_MAIL"
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
		
		data-object-name="OTHERDETAILS" 
		data-property-name="Occupation" 
		 
		
		 
		id="pb-container-text-OTHERDETAILS-Occupation">

		
		<asp:Label ID="lblOTHERDETAILS_Occupation" runat="server" AssociatedControlID="OTHERDETAILS__Occupation" 
			Text="Occupation/Nature of Business" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="OTHERDETAILS__Occupation" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valOTHERDETAILS_Occupation" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Occupation/Nature of Business"
					ClientValidationFunction="onValidate_OTHERDETAILS__Occupation"
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
		styleString += "#id6559531299374bba93ddf45793662976 label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#id6559531299374bba93ddf45793662976 label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id6559531299374bba93ddf45793662976 label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id6559531299374bba93ddf45793662976 label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#id6559531299374bba93ddf45793662976 input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id6559531299374bba93ddf45793662976 input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id6559531299374bba93ddf45793662976 input{text-align:left;}"; break;
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
<div id="id9ec3213725cf4e1abd3596eadd8f1ad6" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading4" runat="server" Text="Ownership Details" /></legend>
				
				
				<div data-column-count="2" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- List -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="List" 
		data-object-name="OTHERDETAILS" 
		data-property-name="Ownership" 
		id="pb-container-list-OTHERDETAILS-Ownership">
		<asp:Label ID="lblOTHERDETAILS_Ownership" runat="server" AssociatedControlID="OTHERDETAILS__Ownership" 
			Text="State the Proposer's interest in the property to be insured:" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
				<NexusProvider:LookupListV2 ID="OTHERDETAILS__Ownership" runat="server" CssClass="form-control" ListType="UserDefined" ListCode="OWNERSHIP" ParentLookupListID="" DataItemValue="Code" DataItemText="Description" DefaultText="--Please Select--" onChange="onValidate_OTHERDETAILS__Ownership(null, null, this);" data-type="List" />
			<asp:CustomValidator ID="valOTHERDETAILS_Ownership" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for State the Proposer's interest in the property to be insured:"
			ClientValidationFunction="onValidate_OTHERDETAILS__Ownership" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
		  
	</span>
</div>
<!-- /List -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Comment -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Comment" 
		data-object-name="OTHERDETAILS" 
		data-property-name="OwnershipDetails" 
		id="pb-container-comment-OTHERDETAILS-OwnershipDetails">
		<asp:Label ID="lblOTHERDETAILS_OwnershipDetails" runat="server" class="col-md-4 col-sm-3 control-label" AssociatedControlID="OTHERDETAILS__OwnershipDetails" 
			Text="State the Owner"></asp:Label>
		
		 <div class="col-md-8 col-sm-9">
	        <asp:HiddenField ID="OTHERDETAILS__OwnershipDetails" runat="server" />
		
		<asp:CustomValidator ID="valOTHERDETAILS_OwnershipDetails" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for State the Owner"
			ClientValidationFunction="onValidate_OTHERDETAILS__OwnershipDetails"
			ValidationGroup="" 
			Display="Dynamic"
			EnableClientScript="true"/>
         </div>
		
	
	</span>
	
</div>
<!-- /Comment -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="OTHERDETAILS" 
		data-property-name="AssignTP" 
		id="pb-container-checkbox-OTHERDETAILS-AssignTP">
		<label id="ctl00_cntMainBody_lblOTHERDETAILS_AssignTP" for="ctl00_cntMainBody_OTHERDETAILS__AssignTP" class="col-md-4 col-sm-3 control-label">
		Is the Policy to be assigned to any Party?</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="OTHERDETAILS__AssignTP" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valOTHERDETAILS_AssignTP" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Is the Policy to be assigned to any Party?"
			ClientValidationFunction="onValidate_OTHERDETAILS__AssignTP" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Comment -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Comment" 
		data-object-name="OTHERDETAILS" 
		data-property-name="TPDetails" 
		id="pb-container-comment-OTHERDETAILS-TPDetails">
		<asp:Label ID="lblOTHERDETAILS_TPDetails" runat="server" class="col-md-4 col-sm-3 control-label" AssociatedControlID="OTHERDETAILS__TPDetails" 
			Text="Give Party Full Name and Address"></asp:Label>
		
		 <div class="col-md-8 col-sm-9">
	        <asp:HiddenField ID="OTHERDETAILS__TPDetails" runat="server" />
		
		<asp:CustomValidator ID="valOTHERDETAILS_TPDetails" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Give Party Full Name and Address"
			ClientValidationFunction="onValidate_OTHERDETAILS__TPDetails"
			ValidationGroup="" 
			Display="Dynamic"
			EnableClientScript="true"/>
         </div>
		
	
	</span>
	
</div>
<!-- /Comment -->
								
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
		styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id9ec3213725cf4e1abd3596eadd8f1ad6 input{text-align:left;}"; break;
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