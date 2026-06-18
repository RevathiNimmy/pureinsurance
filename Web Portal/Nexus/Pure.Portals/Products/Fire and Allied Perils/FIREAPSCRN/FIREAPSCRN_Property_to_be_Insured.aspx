<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="FIREAPSCRN_Property_to_be_Insured.aspx.vb" Inherits="Nexus.PB2_FIREAPSCRN_Property_to_be_Insured" %>

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
function onValidate_PROPERTY__Earthquake(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Earthquake", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Earthquake");
        		
        		var valueExp = new Expression("PROPERTY.EarthquakeRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Earthquake == true")) ? new Expression("PROPERTY.Earthquake == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.EarthquakeRate.setValue( 0)") ? new Expression("PROPERTY.EarthquakeRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Standard_Explosion(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Standard_Explosion", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Standard_Explosion");
        		
        		var valueExp = new Expression("PROPERTY.Standard_ExplosionRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Standard_Explosion == true")) ? new Expression("PROPERTY.Standard_Explosion == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.Standard_ExplosionRate.setValue( 0)") ? new Expression("PROPERTY.Standard_ExplosionRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Riot_And_Strike(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Riot_And_Strike", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Riot_And_Strike");
        		
        		var valueExp = new Expression("PROPERTY.Riot_And_StrikeRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Riot_And_Strike == true")) ? new Expression("PROPERTY.Riot_And_Strike == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.Riot_And_StrikeRate.setValue( 0)") ? new Expression("PROPERTY.Riot_And_StrikeRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Bush_Fire(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Bush_Fire", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Bush_Fire");
        		
        		var valueExp = new Expression("PROPERTY.Bush_FireRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Bush_Fire == true")) ? new Expression("PROPERTY.Bush_Fire == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.Bush_FireRate.setValue( 0)") ? new Expression("PROPERTY.Bush_FireRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Flood(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Flood", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Flood");
        		
        		var valueExp = new Expression("PROPERTY.FloodRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Flood == true")) ? new Expression("PROPERTY.Flood == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.FloodRate.setValue( 0)") ? new Expression("PROPERTY.FloodRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Malicious_Damage(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Malicious_Damage", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Malicious_Damage");
        		
        		var valueExp = new Expression("PROPERTY.Malicious_DamageRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Malicious_Damage == true")) ? new Expression("PROPERTY.Malicious_Damage == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.Malicious_DamageRate.setValue( 0)") ? new Expression("PROPERTY.Malicious_DamageRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Storm(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Storm", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Storm");
        		
        		var valueExp = new Expression("PROPERTY.StormRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Storm == true")) ? new Expression("PROPERTY.Storm == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.StormRate.setValue( 0)") ? new Expression("PROPERTY.StormRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Special_Perils(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Special_Perils", "Checkbox");
        })();
        /**
         * @fileoverview Evaluate an expression when the field value changes.
         * OnChange
         */
        (function(){
        	
        	if (isOnLoad) {		
        		
        		// Setup an instance of the field
        		var field = Field.getInstance("PROPERTY", "Special_Perils");
        		
        		var valueExp = new Expression("PROPERTY.Special_PerilsRate.setValue( 0.01)");
        		var whenExp = (Expression.isValidParameter("PROPERTY.Special_Perils == true")) ? new Expression("PROPERTY.Special_Perils == true") : null;
        		var elseExp = Expression.isValidParameter("PROPERTY.Special_PerilsRate.setValue( 0)") ? new Expression("PROPERTY.Special_PerilsRate.setValue( 0)") : null;
        		
        		events.listen(field, "change", function(e){
        			
        			// Evaluate the expression when the field changes.
        			if (whenExp == null || whenExp.valueOf() == true){
        				valueExp.valueOf();
        			} else if (elseExp){
        				elseExp.valueOf();
        			}
        			
        		}, false, this);
        		
        	
        		
        	};
        })();
}
function onValidate_PROPERTY__Rate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Rate", "Percentage");
        })();
        /**
         * @fileoverview
         * SetValue
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getWithQuery("type=Percentage&objectName=PROPERTY&propertyName=Rate&name={name}");
        		
        		var value = new Expression(" 0.1 + PROPERTY.EarthquakeRate + PROPERTY.Standard_ExplosionRate + PROPERTY.Riot_And_StrikeRate + PROPERTY.Bush_FireRate + PROPERTY.FloodRate + PROPERTY.Malicious_DamageRate + PROPERTY.StormRate + PROPERTY.Special_PerilsRate"), 
        			condition = (Expression.isValidParameter("{1}")) ? new Expression("{1}") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
}
function onValidate_PROPERTY__EarthquakeRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "EarthquakeRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "EarthquakeRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__Standard_ExplosionRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Standard_ExplosionRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "Standard_ExplosionRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__Riot_And_StrikeRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Riot_And_StrikeRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "Riot_And_StrikeRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__Bush_FireRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Bush_FireRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "Bush_FireRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__FloodRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "FloodRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "FloodRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__Malicious_DamageRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Malicious_DamageRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "Malicious_DamageRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__StormRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "StormRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "StormRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__Special_PerilsRate(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Special_PerilsRate", "Percentage");
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
        			field = Field.getInstance("PROPERTY", "Special_PerilsRate");
        		}
        		//window.setProperty(field, "V", "{1}", "{2}", "{3}");
        
            var paramValue = "V",
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
}
function onValidate_PROPERTY__Key_Risk_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "Key_Risk_Sum_Insured", "Currency");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("PROPERTY", "Key_Risk_Sum_Insured");
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
              var field = Field.getInstance("PROPERTY.Key_Risk_Sum_Insured");
        			window.setControlWidth(field, "0.75", "PROPERTY", "Key_Risk_Sum_Insured");
        		})();
        	}
        })();
        
         /**
          * @fileoverview GetColumn
          * @param PROPERTY The Parent (Root) object name.
          * @param PROPERTY_DETAILS.Total_Sum_Insured The object.property to sum the totals of.
          * @param MAX The type of query to do. Accepts TOTAL, COUNT, MIN, MAX, AVERAGE
          * @param {3} Deprecated, The condition for each child row, in the child row context
          */ 
        (function(){
        	
        	if (isOnLoad) {		
        	
        		var screenObjectStr = "PROPERTY".toUpperCase().replace(/^\s+|\s+$/g, '');
        		var childFieldStr = "PROPERTY_DETAILS.Total_Sum_Insured";
        		//count, average, total, min, max
        		var type = "MAX".toUpperCase().replace(/^\s+|\s+$/g, '');
        		var condition = (Expression.isValidParameter("{3}")) ? ("{3}") : ("true");
        		
        		
        		// Get the object and property from the child field string expects object.property
        		var temp = childFieldStr.split(".");
                if (temp.length < 2 || temp.length > 2) {
                    var error = new Error("Invalid Object Property '" + childFieldStr + "' for GetColumn rule.");
                    error.display();
                    //throw error;
                };
                var strObject = temp[0].toUpperCase().replace(/^\s+|\s+$/g, '');
                var strProperty = temp[1].replace(/^\s+|\s+$/g, '');
        		
        		var field = Field.getInstance("PROPERTY", "Key_Risk_Sum_Insured");
        		try {
        
        			var getCurrentNode2 = function (xmldata, optParentObject) {
        				if (window["ThisOI"] == null) return null;
        				
        				var nodes = xmlData.selectNodes(".//*[@OI=\"" + window["ThisOI"] + "\"]"),
        				node = nodes[nodes.length - 1] || null;
        				
        				// Check that ThisOI matches the object we expect.  Parent screens can have multiple OIs.
        				if (optParentObject && node){
        					if (node.node.nodeName.toUpperCase() != optParentObject) {
        						nodes = xmlData.selectNodes("//" + optParentObject);
        						node = nodes[nodes.length - 1] || null;
        					}
        				}
        				
        				return node || xmlData;
        			};
        			
        			var xmlData = pb.xml.Document.loadXml(window.XMLDataSet);
        			var screenObjectNode = getCurrentNode2(xmlData, screenObjectStr);
        			/*var uc = xmlData.getCurrentNode();
        			if (uc == xmlData){
        				// We are at the top level as there is no UC node specified
        				// This means we should get the first element that matches the child screen
        				// object name. There should only be one.
        				var childScreenObjectsArray = xmlData.selectNodes("//" + screenObjectStr);
        				if (childScreenObjectsArray.length > 1){
        					if (console && console.warn){
        						//console.warn(); 
        						throw "Ambiguous xml data set while looking for " + screenObjectStr + ".";
        					}
        				}
        				screenObjectNode = childScreenObjectsArray[0];
        			} else if (uc != null && uc.node.nodeName.toUpperCase() == screenObjectStr){
        				screenObjectNode = uc;
        			}*/
        	
        			var total = 0, count = 0, min = null, max = null, average = null;
        			if (screenObjectNode){
        				
        				var objects = screenObjectNode.selectNodes(strObject);
        				goog.array.forEach(objects, function(obj){
        					//return obj.getAttribute(strProperty);
        					
        					// Check that condition is true, if it is not then do not include this obj.
        					// First override the parser so identifiers relate to this obj.
        					var evaluator = new perseus.Evaluator(condition, {
        						// Provide a context and override the getIdentifier method
        						getIdentifier: function(token){
        							var value = token.valueOf();
        							if (value.indexOf(".") != -1) {
        								var parts = value.split(".");
        								if (parts[0].toUpperCase() == strObject.toUpperCase())
        									return obj.getAttribute(parts[1].toUpperCase());
        							}
        							// Use the original get identifier function
        							return perseus.Evaluator.prototype.getIdentifier.call(this, token);
        						}
        					});
        					var result = evaluator.valueOf();
        					
        					if (! result) return;
        					
        					
        					var originalValue = obj.getAttribute(strProperty);
        					var value = window.parseFloat(originalValue);
        					
        					if (!window.isNaN(value)){
        						total += value;
        					} else {
        						value = originalValue;
        					}
        					count += 1;
        					if (min == null) min = value;
        					if (max == null) max = value;
        					if (value < min) min = value;
        					if (value > max) max = value;
        				});
        			} else {
        				if (console && console.warn){
        					console.warn("Possible incorrect child screen object name used in GetColumn rule.");
        				}
        			}
        			if (count != 0) {
        				average = total / count;
        			}
        			
        			var fieldValue;
        			switch ("MAX".toUpperCase()){
        				case "COUNT": fieldValue = count; break;
        				case "AVERAGE": fieldValue = average; break;
        				case "MIN": fieldValue = min; break;
        				case "MAX": fieldValue = max; break;
        				case "TOTAL": 
        				default: fieldValue = total; break;
        			}
        			field.setValue(fieldValue);
        		} catch (e){
        			// Most likely we hit a parse error due to using an expression
        			// in an old format that's not supported by the latest parser.
        			// Therefore use the old method.
        			var exp;
        			if (type == "COUNT"){
        				exp = new Expression(screenObjectStr + "[" + strObject + "] #TOTAL (" + condition + ") ? (1)");
        			} else {
        				exp = new Expression(screenObjectStr + "[" + strObject + "] #" + type + " (" + condition + ") ? (" + strObject + "." + strProperty + ")");
        			}
        			
        			var field = Field.getInstance("PROPERTY", "Key_Risk_Sum_Insured");
        			field.setValue(exp.getValue());
        		}
        	};
        })();
}
function onValidate_PROPERTY__eml_percentage(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "eml_percentage", "Percentage");
        })();
        /**
         * @fileoverview
         * Check if a mandatory field has been left empty.
         * Check performed only when the page is submitted
         */
        (function(){
        	
        	if (isOnLoad) {		
        		var field = Field.getInstance("PROPERTY", "eml_percentage");
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
              var field = Field.getInstance("PROPERTY.eml_percentage");
        			window.setControlWidth(field, "0.75", "PROPERTY", "eml_percentage");
        		})();
        	}
        })();
}
function onValidate_PROPERTY__PROPERTY(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY", "PROPERTY", "ChildScreen");
        })();
}
function DoLogic(isOnLoad) {
    onValidate_PROPERTY__Earthquake(null, null, null, isOnLoad);
    onValidate_PROPERTY__Standard_Explosion(null, null, null, isOnLoad);
    onValidate_PROPERTY__Riot_And_Strike(null, null, null, isOnLoad);
    onValidate_PROPERTY__Bush_Fire(null, null, null, isOnLoad);
    onValidate_PROPERTY__Flood(null, null, null, isOnLoad);
    onValidate_PROPERTY__Malicious_Damage(null, null, null, isOnLoad);
    onValidate_PROPERTY__Storm(null, null, null, isOnLoad);
    onValidate_PROPERTY__Special_Perils(null, null, null, isOnLoad);
    onValidate_PROPERTY__Rate(null, null, null, isOnLoad);
    onValidate_PROPERTY__EarthquakeRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__Standard_ExplosionRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__Riot_And_StrikeRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__Bush_FireRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__FloodRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__Malicious_DamageRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__StormRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__Special_PerilsRate(null, null, null, isOnLoad);
    onValidate_PROPERTY__Key_Risk_Sum_Insured(null, null, null, isOnLoad);
    onValidate_PROPERTY__eml_percentage(null, null, null, isOnLoad);
    onValidate_PROPERTY__PROPERTY(null, null, null, isOnLoad);
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
<div id="idcf5ec9c7c20349018d69ad8701f4fd38" class="general-layout-container">
				
         
				
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="frmOtherPerils" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading9" runat="server" Text="Rates and Other Perils" /></legend>
				
				
				<div data-column-count="2" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Earthquake" 
		id="pb-container-checkbox-PROPERTY-Earthquake">
		<label id="ctl00_cntMainBody_lblPROPERTY_Earthquake" for="ctl00_cntMainBody_PROPERTY__Earthquake" class="col-md-4 col-sm-3 control-label">
		Earthquake</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Earthquake" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Earthquake" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Earthquake"
			ClientValidationFunction="onValidate_PROPERTY__Earthquake" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Standard_Explosion" 
		id="pb-container-checkbox-PROPERTY-Standard_Explosion">
		<label id="ctl00_cntMainBody_lblPROPERTY_Standard_Explosion" for="ctl00_cntMainBody_PROPERTY__Standard_Explosion" class="col-md-4 col-sm-3 control-label">
		Standard Explosion</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Standard_Explosion" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Standard_Explosion" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Standard Explosion"
			ClientValidationFunction="onValidate_PROPERTY__Standard_Explosion" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Riot_And_Strike" 
		id="pb-container-checkbox-PROPERTY-Riot_And_Strike">
		<label id="ctl00_cntMainBody_lblPROPERTY_Riot_And_Strike" for="ctl00_cntMainBody_PROPERTY__Riot_And_Strike" class="col-md-4 col-sm-3 control-label">
		Riot and Strike</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Riot_And_Strike" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Riot_And_Strike" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Riot and Strike"
			ClientValidationFunction="onValidate_PROPERTY__Riot_And_Strike" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Bush_Fire" 
		id="pb-container-checkbox-PROPERTY-Bush_Fire">
		<label id="ctl00_cntMainBody_lblPROPERTY_Bush_Fire" for="ctl00_cntMainBody_PROPERTY__Bush_Fire" class="col-md-4 col-sm-3 control-label">
		Bush Fire</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Bush_Fire" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Bush_Fire" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Bush Fire"
			ClientValidationFunction="onValidate_PROPERTY__Bush_Fire" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Flood" 
		id="pb-container-checkbox-PROPERTY-Flood">
		<label id="ctl00_cntMainBody_lblPROPERTY_Flood" for="ctl00_cntMainBody_PROPERTY__Flood" class="col-md-4 col-sm-3 control-label">
		Flood</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Flood" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Flood" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Flood"
			ClientValidationFunction="onValidate_PROPERTY__Flood" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Malicious_Damage" 
		id="pb-container-checkbox-PROPERTY-Malicious_Damage">
		<label id="ctl00_cntMainBody_lblPROPERTY_Malicious_Damage" for="ctl00_cntMainBody_PROPERTY__Malicious_Damage" class="col-md-4 col-sm-3 control-label">
		Malicious Damage</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Malicious_Damage" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Malicious_Damage" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Malicious Damage"
			ClientValidationFunction="onValidate_PROPERTY__Malicious_Damage" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Storm" 
		id="pb-container-checkbox-PROPERTY-Storm">
		<label id="ctl00_cntMainBody_lblPROPERTY_Storm" for="ctl00_cntMainBody_PROPERTY__Storm" class="col-md-4 col-sm-3 control-label">
		Storm</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Storm" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Storm" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Storm"
			ClientValidationFunction="onValidate_PROPERTY__Storm" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Checkbox -->
<div class="form-group form-group-sm">
	<span class="field-container" 
		data-field-type="Checkbox" 
		data-object-name="PROPERTY" 
		data-property-name="Special_Perils" 
		id="pb-container-checkbox-PROPERTY-Special_Perils">
		<label id="ctl00_cntMainBody_lblPROPERTY_Special_Perils" for="ctl00_cntMainBody_PROPERTY__Special_Perils" class="col-md-4 col-sm-3 control-label">
		Special Perils</label>
		<div class="col-md-8 col-sm-9">
		<asp:TextBox ID="PROPERTY__Special_Perils" runat="server" CssClass="form-control hidden" />
		<asp:CustomValidator ID="valPROPERTY_Special_Perils" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Special Perils"
			ClientValidationFunction="onValidate_PROPERTY__Special_Perils" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
		</div>
	</span>
</div>
<!-- /Checkbox -->

								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="Rate" 
		id="pb-container-percentage-PROPERTY-Rate">
		<asp:Label ID="lblPROPERTY_Rate" runat="server" AssociatedControlID="PROPERTY__Rate" 
			Text="Rate" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__Rate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_Rate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Rate"
			ClientValidationFunction="onValidate_PROPERTY__Rate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
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
		styleString += "#frmOtherPerils label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#frmOtherPerils label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmOtherPerils label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmOtherPerils label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#frmOtherPerils input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmOtherPerils input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmOtherPerils input{text-align:left;}"; break;
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
<div id="frmPerils Rate" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading10" runat="server" Text="Perils Rates" /></legend>
				
				
				<div data-column-count="2" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="EarthquakeRate" 
		id="pb-container-percentage-PROPERTY-EarthquakeRate">
		<asp:Label ID="lblPROPERTY_EarthquakeRate" runat="server" AssociatedControlID="PROPERTY__EarthquakeRate" 
			Text="Earthquake" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__EarthquakeRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_EarthquakeRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Earthquake"
			ClientValidationFunction="onValidate_PROPERTY__EarthquakeRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="Standard_ExplosionRate" 
		id="pb-container-percentage-PROPERTY-Standard_ExplosionRate">
		<asp:Label ID="lblPROPERTY_Standard_ExplosionRate" runat="server" AssociatedControlID="PROPERTY__Standard_ExplosionRate" 
			Text="Standard Explosion" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__Standard_ExplosionRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_Standard_ExplosionRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Standard Explosion"
			ClientValidationFunction="onValidate_PROPERTY__Standard_ExplosionRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="Riot_And_StrikeRate" 
		id="pb-container-percentage-PROPERTY-Riot_And_StrikeRate">
		<asp:Label ID="lblPROPERTY_Riot_And_StrikeRate" runat="server" AssociatedControlID="PROPERTY__Riot_And_StrikeRate" 
			Text="Riot and Strike" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__Riot_And_StrikeRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_Riot_And_StrikeRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Riot and Strike"
			ClientValidationFunction="onValidate_PROPERTY__Riot_And_StrikeRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="Bush_FireRate" 
		id="pb-container-percentage-PROPERTY-Bush_FireRate">
		<asp:Label ID="lblPROPERTY_Bush_FireRate" runat="server" AssociatedControlID="PROPERTY__Bush_FireRate" 
			Text="Bush Fire" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__Bush_FireRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_Bush_FireRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Bush Fire"
			ClientValidationFunction="onValidate_PROPERTY__Bush_FireRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="FloodRate" 
		id="pb-container-percentage-PROPERTY-FloodRate">
		<asp:Label ID="lblPROPERTY_FloodRate" runat="server" AssociatedControlID="PROPERTY__FloodRate" 
			Text="Flood" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__FloodRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_FloodRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Flood"
			ClientValidationFunction="onValidate_PROPERTY__FloodRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="Malicious_DamageRate" 
		id="pb-container-percentage-PROPERTY-Malicious_DamageRate">
		<asp:Label ID="lblPROPERTY_Malicious_DamageRate" runat="server" AssociatedControlID="PROPERTY__Malicious_DamageRate" 
			Text="Malicious Damage" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__Malicious_DamageRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_Malicious_DamageRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Malicious Damage"
			ClientValidationFunction="onValidate_PROPERTY__Malicious_DamageRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="StormRate" 
		id="pb-container-percentage-PROPERTY-StormRate">
		<asp:Label ID="lblPROPERTY_StormRate" runat="server" AssociatedControlID="PROPERTY__StormRate" 
			Text="Storm" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__StormRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_StormRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Storm"
			ClientValidationFunction="onValidate_PROPERTY__StormRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="Special_PerilsRate" 
		id="pb-container-percentage-PROPERTY-Special_PerilsRate">
		<asp:Label ID="lblPROPERTY_Special_PerilsRate" runat="server" AssociatedControlID="PROPERTY__Special_PerilsRate" 
			Text="Special Perils" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__Special_PerilsRate" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_Special_PerilsRate" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Special Perils"
			ClientValidationFunction="onValidate_PROPERTY__Special_PerilsRate" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
									</li>
							
							
						
					</ul>
				
				</div>
				
			
</div>

<script type="text/javascript">
	(function(){
		var container = document.getElementById('frmPerils Rate'),
		zippy = new pb.ui.Zippy(
			container,
			goog.dom.query("legend", container)[0],
			goog.dom.query(".column-content", container)[0],
			true
		);
		
		zippy.collapse();
		
	})();
</script>


<script type="text/javascript">
	var labelAlign = "";
	var textAlign = "";
	var labelWidth = "";
	
	var styleString = "";
	if (labelWidth != ""){
		if ((new Expression("IsNumeric('" + labelWidth + "')")).valueOf()){
			labelWidth = labelWidth + "px";
		}
		styleString += "#frmPerils Rate label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#frmPerils Rate label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmPerils Rate label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmPerils Rate label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#frmPerils Rate input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#frmPerils Rate input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#frmPerils Rate input{text-align:left;}"; break;
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
<div id="Key Information" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading11" runat="server" Text="Ownership Details" /></legend>
				
				
				<div data-column-count="2" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY" 
		data-property-name="Key_Risk_Sum_Insured" 
		id="pb-container-currency-PROPERTY-Key_Risk_Sum_Insured">
		<asp:Label ID="lblPROPERTY_Key_Risk_Sum_Insured" runat="server" AssociatedControlID="PROPERTY__Key_Risk_Sum_Insured" 
			Text="Top Location Sum Insured" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY__Key_Risk_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_Key_Risk_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Top Location Sum Insured"
			ClientValidationFunction="onValidate_PROPERTY__Key_Risk_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:50%;">
								
								
										<!-- Percentage -->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Percentage" 
		data-object-name="PROPERTY" 
		data-property-name="eml_percentage" 
		id="pb-container-percentage-PROPERTY-eml_percentage">
		<asp:Label ID="lblPROPERTY_eml_percentage" runat="server" AssociatedControlID="PROPERTY__eml_percentage" 
			Text="EML Percentage" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
			<div class="col-md-8 col-sm-9">
		        <asp:TextBox ID="PROPERTY__eml_percentage" runat="server" CssClass="form-control" />
		    </div>
		<asp:CustomValidator ID="valPROPERTY_eml_percentage" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for EML Percentage"
			ClientValidationFunction="onValidate_PROPERTY__eml_percentage" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>
<!-- /Percentage -->
								
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
		styleString += "#Key Information label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#Key Information label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#Key Information label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#Key Information label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#Key Information input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#Key Information input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#Key Information input{text-align:left;}"; break;
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
		
				
	              <legend><asp:Label ID="lblHeading12" runat="server" Text="" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Child -->
	<div class="child-card" id="ctl00_cntMainBody_PROPERTY__PROPERTY"
		data-field-type="Child" 
		data-object-name="PROPERTY" 
		data-property-name="PROPERTY" 
		id="pb-container-childscreen-PROPERTY-PROPERTY">
		
		    <legend>Property to be Insured</legend>
		 
			        <div class="grid-card table-responsive no-margin">
						<nexus:ItemGrid ID="PROPERTY__PROPERTY_DETAILS" runat="server" ScreenCode="PROPERTY" AutoGenerateColumns="false"
							GridLines="None" ChildPage="PROPERTY/PROPERTY_Property_Details.aspx" emptydatatext="sac">
							<columns>
						<Nexus:RiskAttribute HeaderText="Plot Number" DataField="Plot_No" DataFormatString=""/>
<Nexus:RiskAttribute HeaderText="Street" DataField="Street" DataFormatString=""/>
<Nexus:RiskAttribute HeaderText="Town" DataField="Town" DataFormatString=""/>
<Nexus:RiskAttribute HeaderText="Building Occupied As" DataField="Occuipied_As" DataFormatString=""/>
<Nexus:RiskAttribute HeaderText="Building SI" DataField="Building_Sum_Insured" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Machinery Plant Utensils SI" DataField="MPU_Sum_Insured" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Stock SI" DataField="Stock_Sum_Insured" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Furniture SI" DataField="Furniture_Sum_Insured" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Other Items Description" DataField="Other_Items" DataFormatString=""/>
<Nexus:RiskAttribute HeaderText="Other Items SI" DataField="Others_Sum_Insured" DataFormatString="{0:N}"/>
<Nexus:RiskAttribute HeaderText="Total Sum Insured" DataField="Total_Sum_Insured" DataFormatString="{0:N}"/>

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
				
					<asp:CustomValidator ID="valPROPERTY_PROPERTY" 
						runat="server" 
						Text="*" 
						ErrorMessage="A validation error occurred for Property to be Insured"
						ClientValidationFunction="onValidate_PROPERTY__PROPERTY" 
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