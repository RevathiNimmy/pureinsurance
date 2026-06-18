<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="FIREAPSCRN_Policy_Header_Details.aspx.vb" Inherits="Nexus.PB2_FIREAPSCRN_Policy_Header_Details" %>

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
function onValidate_POLICYHEADER__POLICYNUMBER(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "POLICYNUMBER", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_POLICYNUMBER");
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
              var field = Field.getInstance("POLICYHEADER.POLICYNUMBER");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "POLICYNUMBER");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__BRANCH(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "BRANCH", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_BRANCH");
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
              var field = Field.getInstance("POLICYHEADER.BRANCH");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "BRANCH");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__SUBBRANCH(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "SUBBRANCH", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_SUBBRANCH");
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
              var field = Field.getInstance("POLICYHEADER.SUBBRANCH");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "SUBBRANCH");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__BUSINESSTYPE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "BUSINESSTYPE", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_BUSINESSTYPE");
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
              var field = Field.getInstance("POLICYHEADER.BUSINESSTYPE");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "BUSINESSTYPE");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__AGENTCODE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "AGENTCODE", "Text");
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
        			field = Field.getInstance("POLICYHEADER", "AGENTCODE");
        		}
        		//window.setProperty(field, "H", "caption(POLICYHEADER.BUSINESSTYPE) == 'Direct Business'", "VM", "{3}");
        
            var paramValue = "H",
            paramCondition = "caption(POLICYHEADER.BUSINESSTYPE) == 'Direct Business'",
            paramElseValue = "VM",
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_AGENTCODE");
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
              var field = Field.getInstance("POLICYHEADER.AGENTCODE");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "AGENTCODE");
        		})();
        	}
        })();
        /**
         * @fileoverview
         * Set Value, this is a duplicate of SetValue, this version
         * is deprecated.
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getInstance("POLICYHEADER", "AGENTCODE");
        		
        		var value = new Expression("''"), 
        			condition = (Expression.isValidParameter("caption(POLICYHEADER.BUSINESSTYPE) == 'Direct Business'")) ? new Expression("caption(POLICYHEADER.BUSINESSTYPE) == 'Direct Business'") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
}
function onValidate_POLICYHEADER__CURRENCY(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "CURRENCY", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_CURRENCY");
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
              var field = Field.getInstance("POLICYHEADER.CURRENCY");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "CURRENCY");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__ANALYSISCODE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "ANALYSISCODE", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_ANALYSISCODE");
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
              var field = Field.getInstance("POLICYHEADER.ANALYSISCODE");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "ANALYSISCODE");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__HANDLER(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "HANDLER", "Text");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_HANDLER");
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
              var field = Field.getInstance("POLICYHEADER.HANDLER");
        			window.setControlWidth(field, "1.75", "POLICYHEADER", "HANDLER");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__POLICYSTATUSCODE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "POLICYSTATUSCODE", "Date");
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_POLICYSTATUSCODE");
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
function onValidate_POLICYHEADER__COVERSTARTDATE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "COVERSTARTDATE", "Date");
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
        			field = Field.getInstance("POLICYHEADER", "COVERSTARTDATE");
        		}
        		//window.setProperty(field, "VEM", "{1}", "{2}", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "{1}",
            paramElseValue = "{2}",
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_COVERSTARTDATE");
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
              var field = Field.getInstance("POLICYHEADER.COVERSTARTDATE");
        			window.setControlWidth(field, "0.5", "POLICYHEADER", "COVERSTARTDATE");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__COVERENDDATE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "COVERENDDATE", "Date");
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
        			field = Field.getInstance("POLICYHEADER", "COVERENDDATE");
        		}
        		//window.setProperty(field, "VEM", "{1}", "{2}", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "{1}",
            paramElseValue = "{2}",
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
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_COVERENDDATE");
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
              var field = Field.getInstance("POLICYHEADER.COVERENDDATE");
        			window.setControlWidth(field, "0.5", "POLICYHEADER", "COVERENDDATE");
        		})();
        	}
        })();
        /**
         * @fileoverview
         * Set Value, this is a duplicate of SetValue, this version
         * is deprecated.
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getInstance("POLICYHEADER", "COVERENDDATE");
        		
        		var value = new Expression("AddYears(POLICYHEADER.COVERSTARTDATE, 1)"), 
        			condition = (Expression.isValidParameter("TransactionType == 'NB'")) ? new Expression("TransactionType == 'NB'") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
        /**
         * @fileoverview
         * Set Value, this is a duplicate of SetValue, this version
         * is deprecated.
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getInstance("POLICYHEADER", "COVERENDDATE");
        		
        		var value = new Expression("AddYears(POLICYHEADER.COVERSTARTDATE, 1)"), 
        			condition = (Expression.isValidParameter("TransactionType == 'RN'")) ? new Expression("TransactionType == 'RN'") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
}
function onValidate_POLICYHEADER__INCEPTION(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "INCEPTION", "Date");
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
        			field = Field.getInstance("POLICYHEADER", "INCEPTION");
        		}
        		//window.setProperty(field, "VEM", "{1}", "{2}", "{3}");
        
            var paramValue = "VEM",
            paramCondition = "{1}",
            paramElseValue = "{2}",
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
        
        			var width = window.parseFloat("1.75");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_INCEPTION");
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
              var field = Field.getInstance("POLICYHEADER.INCEPTION");
        			window.setControlWidth(field, "0.5", "POLICYHEADER", "INCEPTION");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__RENEWAL(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "RENEWAL", "Date");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("1.75");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_RENEWAL");
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
              var field = Field.getInstance("POLICYHEADER.RENEWAL");
        			window.setControlWidth(field, "0.5", "POLICYHEADER", "RENEWAL");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__PROPOSALDATE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "PROPOSALDATE", "Date");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("1.75");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_PROPOSALDATE");
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
              var field = Field.getInstance("POLICYHEADER.PROPOSALDATE");
        			window.setControlWidth(field, "0.5", "POLICYHEADER", "PROPOSALDATE");
        		})();
        	}
        })();
}
function onValidate_POLICYHEADER__QUOTEEXPIRYDATE(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "POLICYHEADER", "QUOTEEXPIRYDATE", "Date");
        })();
        /**
         * Set the label width
         */
        (function(){
        	
        	if (isOnLoad) {	
        		window.setTimeout(function(){
        
        			var width = window.parseFloat("1.75");
        			var standardWidth = 165;
        			if ("{name}" != "{na" + "me}"){
        				var label = document.getElementById("{name}");
        				// Walk up the dom, if a co-cell is found use that intead
        				if (label.parentNode.parentNode.parentNode.className.toLowerCase() == "co-cell")
        					label = label.parentNode.parentNode.parentNode;
        			} else {
        				var label = document.getElementById("ctl00_cntMainBody_lblPOLICYHEADER_QUOTEEXPIRYDATE");
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
              var field = Field.getInstance("POLICYHEADER.QUOTEEXPIRYDATE");
        			window.setControlWidth(field, "0.5", "POLICYHEADER", "QUOTEEXPIRYDATE");
        		})();
        	}
        })();
}
function DoLogic(isOnLoad) {
    onValidate_POLICYHEADER__POLICYNUMBER(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__BRANCH(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__SUBBRANCH(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__BUSINESSTYPE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__AGENTCODE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__CURRENCY(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__ANALYSISCODE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__HANDLER(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__POLICYSTATUSCODE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__COVERSTARTDATE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__COVERENDDATE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__INCEPTION(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__RENEWAL(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__PROPOSALDATE(null, null, null, isOnLoad);
    onValidate_POLICYHEADER__QUOTEEXPIRYDATE(null, null, null, isOnLoad);
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
<div id="id79b66dc041cc46cd82dff13d264b0368" class="general-layout-container">
				
         
				
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="idef56996672f54a38a93ff12aad9ec2a6" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading1" runat="server" Text="Policy Details" /></legend>
				
				
				<div data-column-count="2" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="POLICYHEADER" 
		data-property-name="POLICYNUMBER" 
		 
		
		 
		id="pb-container-text-POLICYHEADER-POLICYNUMBER">

		
		<asp:Label ID="lblPOLICYHEADER_POLICYNUMBER" runat="server" AssociatedControlID="POLICYHEADER__POLICYNUMBER" 
			Text="Policy Number" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="POLICYHEADER__POLICYNUMBER" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPOLICYHEADER_POLICYNUMBER" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Policy Number"
					ClientValidationFunction="onValidate_POLICYHEADER__POLICYNUMBER"
					ValidationGroup=""
					Display="Dynamic"
					EnableClientScript="true"
					/>
                </div>
					
		
	
		
	</span>
</div>
        
<!-- /Text -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderDropdown -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="List" 
		data-object-name="POLICYHEADER" 
		data-property-name="BRANCH" 
		id="pb-container-list-POLICYHEADER-BRANCH">
		<asp:Label ID="lblPOLICYHEADER_BRANCH" runat="server" AssociatedControlID="POLICYHEADER__BRANCH" 
			Text="Branch Code" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		    <div class="col-md-8 col-sm-9">
				<asp:DropDownList id="POLICYHEADER__BRANCH" AutoPostBack="False" runat="server" CssClass="form-control" onChange="onValidate_POLICYHEADER__BRANCH(null, null, this);" data-type="List"/>
				<asp:CustomValidator ID="valPOLICYHEADER_BRANCH" 
				runat="server" 
				Text="*" 
				ErrorMessage="A validation error occurred for Branch Code"
				ClientValidationFunction="onValidate_POLICYHEADER__BRANCH" 
				ValidationGroup=""
				Display="Dynamic"
				EnableClientScript="true"/>
			</div>
	</span>
</div>
<!-- /PolicyHeaderDropdown -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderSubBranch -->
	<!-- Sub branch is updated when branch is selected. -->
<div class="form-group form-group-sm">
	<span class="field-container">
	<asp:Label ID="lblPOLICYHEADER_SUBBRANCH" runat="server" AssociatedControlID="POLICYHEADER__SUBBRANCH" Text="Sub Branch Code" 
	CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
	  <div class="col-md-8 col-sm-9">
	     <asp:DropDownList id="POLICYHEADER__SUBBRANCH" CssClass="form-control" AutoPostBack="False" runat="server" />
	  </div>
	</span>
</div>
	<script type="text/javascript">
		(function(){
			var container = Array.prototype.pop.call(Array.prototype.slice.call(goog.dom.query(".field-container")));

			var instance = new fields.List("POLICYHEADER", "SUBBRANCH");
			instance.decorate(container);
			Field.registerInstance(instance);

			if (window.rulesDisabled && instance.setReadOnly){
				instance.makeUnchangeable();
				instance.makeReadOnlyForever();
			}
		})();
	</script>
<!-- /PolicyHeaderSubBranch -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderBusinessType -->
<div class="form-group form-group-sm">
   <span class="field-container"
	data-field-type="List" 
	data-object-name="POLICYHEADER" 
	data-property-name="BUSINESSTYPE" 
	id="pb-container-list-POLICYHEADER-BUSINESSTYPE">
	<asp:Label ID="lblPOLICYHEADER_BUSINESSTYPE" runat="server" AssociatedControlID="POLICYHEADER__BUSINESSTYPE" 
			Text="Business Type" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>

	<div class="col-md-8 col-sm-9">
		<NexusProvider:LookupList ID="POLICYHEADER__BUSINESSTYPE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Business_Type" DefaultText="(Please Select)" onchange='onValidate_POLICYHEADER__BUSINESSTYPE(null, null, this);' CssClass="form-control" AutoPostBack="False" ParentFieldName="" ParentLookupListID="" Value="" data-type="List" />
		<asp:CustomValidator ID="valPOLICYHEADER_BUSINESSTYPE" 
		runat="server" 
		Text="*" 
		ErrorMessage="A validation error occurred for Business Type"
		ClientValidationFunction="onValidate_POLICYHEADER__BUSINESSTYPE" 
		ValidationGroup=""
		Display="Dynamic"
		EnableClientScript="true"/>
    </div>
   </span>
</div>
<!-- /PolicyHeaderBusinessType -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderAgentCode -->
	<script language="javascript" type="text/javascript">
		function setAgent(sName,sKey,sCode,sAgentType)
			{
				tb_remove();
				document.getElementById('<%= POLICYHEADER__AGENTCODE.ClientId %>').value=sCode;
				var input = document.getElementsByName("ctl00$cntMainBody$POLICYHEADER__AGENTCODE")[0] || null;
				if (input) input.value = sCode;
				document.getElementById('<%= POLICYHEADER__AGENT.ClientId%>').value = sKey;
				 var ac = Field.getInstance('POLICY.TempAgentCode');
                if (ac != null) {
                    ac.setValue(sCode)
				}


			}
	</script>


	<div class="form-group form-group-sm">
    
    
	<label Class="col-md-4 col-sm-3 control-label">Lead Agent</label>
	
	<div class="col-md-8 col-sm-9">
       <div class="input-group">
		<asp:TextBox ID="POLICYHEADER__AGENTCODE" runat="server" CssClass="form-control" Visible="true"/>
		 <span class="input-group-btn">
			 <asp:HyperLink ID="POLICYHEADER_AGENTCODE_hypAddress" runat="server" NavigateUrl="~/Modal/FindAgent.aspx?FromPage=MainDetails&modal=true&KeepThis=true&TB_iframe=true&height=400&width=800"
			 Visible="true" CssClass="btn btn-sm btn-default" data="modal">
			 <i class="glyphicon glyphicon-search"></i>
				
				
					<asp:Button ID="POLICYHEADER_AGENTCODE_btnAgentCode" CssClass="modal" runat="server" class="hide" Text="Lead Agent" Visible="true" CausesValidation="false" />
				
			</asp:HyperLink>
		 </span>
	  </div>
	    <asp:HiddenField ID="POLICYHEADER__AGENT" runat="server" />
		<asp:CustomValidator ID="valPOLICYHEADER_AGENTCODE" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Lead Agent"
			ClientValidationFunction="onValidate_POLICYHEADER__AGENTCODE"
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
    </div>	
</div>	
<!-- /PolicyHeaderAgentCode -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderDropdown -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="List" 
		data-object-name="POLICYHEADER" 
		data-property-name="CURRENCY" 
		id="pb-container-list-POLICYHEADER-CURRENCY">
		<asp:Label ID="lblPOLICYHEADER_CURRENCY" runat="server" AssociatedControlID="POLICYHEADER__CURRENCY" 
			Text="Currency" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		    <div class="col-md-8 col-sm-9">
				<asp:DropDownList id="POLICYHEADER__CURRENCY" AutoPostBack="False" runat="server" CssClass="form-control" onChange="onValidate_POLICYHEADER__CURRENCY(null, null, this);" data-type="List"/>
				<asp:CustomValidator ID="valPOLICYHEADER_CURRENCY" 
				runat="server" 
				Text="*" 
				ErrorMessage="A validation error occurred for Currency"
				ClientValidationFunction="onValidate_POLICYHEADER__CURRENCY" 
				ValidationGroup=""
				Display="Dynamic"
				EnableClientScript="true"/>
			</div>
	</span>
</div>
<!-- /PolicyHeaderDropdown -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderAnalysisCode -->
<div class="form-group form-group-sm">
	<asp:Label ID="lblPOLICYHEADER_ANALYSISCODE" CssClass="col-md-4 col-sm-3 control-label" runat="server" AssociatedControlID="POLICYHEADER__ANALYSISCODE" Text="Analysis Code"></asp:Label>
    <div class="col-md-8 col-sm-9">
	<NexusProvider:LookupList ID="POLICYHEADER__ANALYSISCODE"
	runat="server" DataItemValue="Code" DataItemText="Description" Sort="Asc" ListType="PMLookup"
	ListCode="Analysis_Code" DefaultText="(Please Select)" onchange='onValidate_POLICYHEADER__ANALYSISCODE(null, null, this);'
	CssClass="form-control" AutoPostBack="False"
	ParentFieldName="" ParentLookupListID="" Value="" />
	</div>
</div>
<!-- /PolicyHeaderAnalysisCode -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderHandler -->
	<script language="javascript" type="text/javascript">
	function setAccountHandler(sName, sKey, sCode) {
				tb_remove();
				document.getElementById('<%= POLICYHEADER__HANDLER.ClientId%>').value = sCode;
				document.getElementById('<%= hiddenHandlerCode.ClientId%>').value = sKey;
				document.getElementById('<%= POLICYHEADER__HANDLER.ClientId%>').focus;
			}
	</script>
<div class="form-group form-group-sm">
<label Class="col-md-4 col-sm-3 control-label">Account Handler</label>
 <div class="col-md-8 col-sm-9">
       <div class="input-group">
	   <asp:TextBox ID="POLICYHEADER__HANDLER" Visible="True" runat="server" CssClass="form-control" meta:resourcekey="POLICYHEADER__HANDLERResource1" />
	    <span class="input-group-btn">
			<asp:HyperLink ID="hypHandler" runat="server" NavigateUrl="~/Modal/FindAccountHandler.aspx?modal=true&KeepThis=true&TB_iframe=true&height=400&width=800" Visible="True" CssClass="btn btn-sm btn-default" data="modal" meta:resourcekey="hypHandlerResource1">
			    <i class="glyphicon glyphicon-search"></i>
				<span class="btn-fnd-txt">Account Handler</span>
				<asp:Button ID="btnHandler" runat="server" Text="Account Handler" Visible="True" CausesValidation="false" class="hide" meta:resourcekey="btnHandlerResource1" />
			</asp:HyperLink>
	   </span>
      </div>
	  <asp:HiddenField ID="hiddenHandlerCode" runat="server" />
 </div>	  
</div>
<!-- /PolicyHeaderHandler -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- PolicyHeaderPolicyStatusCode -->
<div class="form-group form-group-sm">
	<asp:Label ID="lblPOLICYHEADER_POLICYSTATUSCODE" CssClass="col-md-4 col-sm-3 control-label" runat="server" AssociatedControlID="POLICYHEADER__POLICYSTATUSCODE" Text="Policy Status Code"></asp:Label>
    <div class="col-md-8 col-sm-9">
	<NexusProvider:LookupList ID="POLICYHEADER__POLICYSTATUSCODE"
	runat="server" DataItemValue="Code" DataItemText="Description" Sort="Asc" ListType="PMLookup"
	ListCode="Policy_Status" DefaultText="(Please Select)" onchange='onValidate_POLICYHEADER__POLICYSTATUSCODE(null, null, this);'
	CssClass="form-control" AutoPostBack="False"
	ParentFieldName="" ParentLookupListID="" Value="" />
    </div>
</div>
<!-- /PolicyHeaderPolicyStatusCode -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Date -->
 <div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Date" 
		data-object-name="POLICYHEADER" 
		data-property-name="COVERSTARTDATE" 
		id="pb-container-datejquerycompatible-POLICYHEADER-COVERSTARTDATE">
		<asp:Label ID="lblPOLICYHEADER_COVERSTARTDATE" runat="server" AssociatedControlID="POLICYHEADER__COVERSTARTDATE" 
			Text="Cover Start Date" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			 <div class="col-md-8 col-sm-9">
			  <div class="input-group">
				<asp:TextBox ID="POLICYHEADER__COVERSTARTDATE" runat="server" CssClass="form-control" data-type="Date" />
				<uc1:CalendarLookup ID="calPOLICYHEADER__COVERSTARTDATE" runat="server" LinkedControl="POLICYHEADER__COVERSTARTDATE" HLevel="1" />
		     </div>
			 <asp:CustomValidator ID="valPOLICYHEADER_COVERSTARTDATE" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Cover Start Date"
			ClientValidationFunction="onValidate_POLICYHEADER__COVERSTARTDATE" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
	</span>
</div>
<!-- /Date -->


								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Date -->
 <div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Date" 
		data-object-name="POLICYHEADER" 
		data-property-name="COVERENDDATE" 
		id="pb-container-datejquerycompatible-POLICYHEADER-COVERENDDATE">
		<asp:Label ID="lblPOLICYHEADER_COVERENDDATE" runat="server" AssociatedControlID="POLICYHEADER__COVERENDDATE" 
			Text="Cover End Date" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			 <div class="col-md-8 col-sm-9">
			  <div class="input-group">
				<asp:TextBox ID="POLICYHEADER__COVERENDDATE" runat="server" CssClass="form-control" data-type="Date" />
				<uc1:CalendarLookup ID="calPOLICYHEADER__COVERENDDATE" runat="server" LinkedControl="POLICYHEADER__COVERENDDATE" HLevel="1" />
		     </div>
			 <asp:CustomValidator ID="valPOLICYHEADER_COVERENDDATE" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Cover End Date"
			ClientValidationFunction="onValidate_POLICYHEADER__COVERENDDATE" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
	</span>
</div>
<!-- /Date -->


								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Date -->
 <div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Date" 
		data-object-name="POLICYHEADER" 
		data-property-name="INCEPTION" 
		id="pb-container-datejquerycompatible-POLICYHEADER-INCEPTION">
		<asp:Label ID="lblPOLICYHEADER_INCEPTION" runat="server" AssociatedControlID="POLICYHEADER__INCEPTION" 
			Text="Inception Date" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			 <div class="col-md-8 col-sm-9">
			  <div class="input-group">
				<asp:TextBox ID="POLICYHEADER__INCEPTION" runat="server" CssClass="form-control" data-type="Date" />
				<uc1:CalendarLookup ID="calPOLICYHEADER__INCEPTION" runat="server" LinkedControl="POLICYHEADER__INCEPTION" HLevel="1" />
		     </div>
			 <asp:CustomValidator ID="valPOLICYHEADER_INCEPTION" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Inception Date"
			ClientValidationFunction="onValidate_POLICYHEADER__INCEPTION" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
	</span>
</div>
<!-- /Date -->


								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Date -->
 <div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Date" 
		data-object-name="POLICYHEADER" 
		data-property-name="RENEWAL" 
		id="pb-container-datejquerycompatible-POLICYHEADER-RENEWAL">
		<asp:Label ID="lblPOLICYHEADER_RENEWAL" runat="server" AssociatedControlID="POLICYHEADER__RENEWAL" 
			Text="Renewal Date" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			 <div class="col-md-8 col-sm-9">
			  <div class="input-group">
				<asp:TextBox ID="POLICYHEADER__RENEWAL" runat="server" CssClass="form-control" data-type="Date" />
				<uc1:CalendarLookup ID="calPOLICYHEADER__RENEWAL" runat="server" LinkedControl="POLICYHEADER__RENEWAL" HLevel="1" />
		     </div>
			 <asp:CustomValidator ID="valPOLICYHEADER_RENEWAL" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Renewal Date"
			ClientValidationFunction="onValidate_POLICYHEADER__RENEWAL" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
	</span>
</div>
<!-- /Date -->


								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Date -->
 <div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Date" 
		data-object-name="POLICYHEADER" 
		data-property-name="PROPOSALDATE" 
		id="pb-container-datejquerycompatible-POLICYHEADER-PROPOSALDATE">
		<asp:Label ID="lblPOLICYHEADER_PROPOSALDATE" runat="server" AssociatedControlID="POLICYHEADER__PROPOSALDATE" 
			Text="Proposal Date" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			 <div class="col-md-8 col-sm-9">
			  <div class="input-group">
				<asp:TextBox ID="POLICYHEADER__PROPOSALDATE" runat="server" CssClass="form-control" data-type="Date" />
				<uc1:CalendarLookup ID="calPOLICYHEADER__PROPOSALDATE" runat="server" LinkedControl="POLICYHEADER__PROPOSALDATE" HLevel="1" />
		     </div>
			 <asp:CustomValidator ID="valPOLICYHEADER_PROPOSALDATE" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Proposal Date"
			ClientValidationFunction="onValidate_POLICYHEADER__PROPOSALDATE" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
	</span>
</div>
<!-- /Date -->


								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Date -->
 <div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Date" 
		data-object-name="POLICYHEADER" 
		data-property-name="QUOTEEXPIRYDATE" 
		id="pb-container-datejquerycompatible-POLICYHEADER-QUOTEEXPIRYDATE">
		<asp:Label ID="lblPOLICYHEADER_QUOTEEXPIRYDATE" runat="server" AssociatedControlID="POLICYHEADER__QUOTEEXPIRYDATE" 
			Text="Quote Expiry Date" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			 <div class="col-md-8 col-sm-9">
			  <div class="input-group">
				<asp:TextBox ID="POLICYHEADER__QUOTEEXPIRYDATE" runat="server" CssClass="form-control" data-type="Date" />
				<uc1:CalendarLookup ID="calPOLICYHEADER__QUOTEEXPIRYDATE" runat="server" LinkedControl="POLICYHEADER__QUOTEEXPIRYDATE" HLevel="1" />
		     </div>
			 <asp:CustomValidator ID="valPOLICYHEADER_QUOTEEXPIRYDATE" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Quote Expiry Date"
			ClientValidationFunction="onValidate_POLICYHEADER__QUOTEEXPIRYDATE" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		    </div>
	</span>
</div>
<!-- /Date -->


								
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
		styleString += "#idef56996672f54a38a93ff12aad9ec2a6 label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#idef56996672f54a38a93ff12aad9ec2a6 label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#idef56996672f54a38a93ff12aad9ec2a6 label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#idef56996672f54a38a93ff12aad9ec2a6 label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#idef56996672f54a38a93ff12aad9ec2a6 input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#idef56996672f54a38a93ff12aad9ec2a6 input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#idef56996672f54a38a93ff12aad9ec2a6 input{text-align:left;}"; break;
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