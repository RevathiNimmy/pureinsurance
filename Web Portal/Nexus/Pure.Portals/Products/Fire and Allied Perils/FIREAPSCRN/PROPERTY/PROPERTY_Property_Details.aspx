<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="PROPERTY_Property_Details.aspx.vb" Inherits="Nexus.PB2_PROPERTY_Property_Details" %>

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
        
        
        window.Colours = {};
        window.Colours.SetTextColour = function(field, colour, condition, elseColour){
        	var element = field.getElement();
        	var update = function(){		
        		var useColour;
        		if (condition == null)
        		{
        			useColour = colour;
        		} else if (condition.getValue() == true) {
        			useColour = colour;
        		} else if (elseColour != null){
        			useColour = elseColour || "inherit";
        		}
        		
        		useColour = window.Colours.stripSingleQuotes(useColour);
        		
        		if (field.setColour){
        			field.setColour(useColour);
        			return;
        		}
        		
        		element.style.color = useColour;
        		// Need to update the fake input too
        		if (window.Formatting){
        			var fake = window.Formatting.getFakeInput(element);
        			if (fake != null)
        				fake.style.color = useColour;
        		}
        	}
        	
        	update();
        	if (condition != null)
        		events.listen(condition, "change", update, false, this);
        };
        window.Colours.SetBackgroundColour = function(field, colour, condition, elseColour){
        
        	
        
        	var element = field.getElement();
        	var update = function(){		
        		var useColour;
        		if (condition == null)
        		{
        			useColour = colour;
        		} else if (condition.getValue() == true) {
        			useColour = colour;
        		} else if (elseColour != null){
        			useColour = elseColour || "inherit";
        		}
        		
        		
        		useColour = window.Colours.stripSingleQuotes(useColour);
        		
        		if (field.setBackgroundColour){
        			field.setBackgroundColour(useColour);
        			return;
        		}
        		
        		element.style.backgroundColor = useColour;
        		// Need to update the fake input too
        		if (window.Formatting){
        			var fake = window.Formatting.getFakeInput(element);
        			if (fake != null)
        				fake.style.backgroundColor = useColour;
        		}
        	}
        	
        	update();
        	if (condition != null)
        		events.listen(condition, "change", update, false, this);
        		
        };
        
        window.Colours.stripSingleQuotes = function(value){
        	if (value.slice(0,1) == "'" && value.slice(-1) == "'")
        		value = value.slice(1, -1);
        	return value;
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
function onValidate_PROPERTY_DETAILS__Plot_No(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Plot_No", "Text");
        })();
}
function onValidate_PROPERTY_DETAILS__Street(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Street", "Text");
        })();
}
function onValidate_PROPERTY_DETAILS__Town(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Town", "Text");
        })();
}
function onValidate_PROPERTY_DETAILS__Occuipied_As(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Occuipied_As", "Text");
        })();
        /**
         * @fileoverview
         * Adds an info icon and appends to the info icon's tooltip.
         */
        (function(){
        	var helpText = goog.string.unescapeEntities("e.g. Manufacturing Company");
        	
        	// Get the field
        	var field = Field.getWithQuery("type=Text&objectName=PROPERTY_DETAILS&propertyName=Occuipied_As&name={name}");
        		
        	field.addHelpText(helpText);
        })();
}
function onValidate_PROPERTY_DETAILS__Wall(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Wall", "Text");
        })();
        /**
         * @fileoverview
         * Adds an info icon and appends to the info icon's tooltip.
         */
        (function(){
        	var helpText = goog.string.unescapeEntities("Give full description e.g. ironsheets, timber, steel, tiles");
        	
        	// Get the field
        	var field = Field.getWithQuery("type=Text&objectName=PROPERTY_DETAILS&propertyName=Wall&name={name}");
        		
        	field.addHelpText(helpText);
        })();
}
function onValidate_PROPERTY_DETAILS__Roof(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Roof", "Text");
        })();
}
function onValidate_PROPERTY_DETAILS__Building_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Building_Sum_Insured", "Currency");
        })();
}
function onValidate_PROPERTY_DETAILS__MPU_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "MPU_Sum_Insured", "Currency");
        })();
}
function onValidate_PROPERTY_DETAILS__Stock_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Stock_Sum_Insured", "Currency");
        })();
}
function onValidate_PROPERTY_DETAILS__Furniture_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Furniture_Sum_Insured", "Currency");
        })();
}
function onValidate_PROPERTY_DETAILS__Other_Items(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Other_Items", "Text");
        })();
        /**
         * Set the control watermark
         */
        (function(){
        	
        	if (isOnLoad) {	
        		var field = Field.getInstance("PROPERTY_DETAILS", "Other_Items");
        		if (field.setPlaceholder){
        			field.setPlaceholder("Description of Items");
        			return;
        		}
        		var placeholder = new Placeholder("Description of Items");
        		placeholder.decorate(field.getElement());
        	}
        })();
        /**
         * @fileoverview
         * Adds an info icon and appends to the info icon's tooltip.
         */
        (function(){
        	var helpText = goog.string.unescapeEntities("Other Items Description");
        	
        	// Get the field
        	var field = Field.getWithQuery("type=Text&objectName=PROPERTY_DETAILS&propertyName=Other_Items&name={name}");
        		
        	field.addHelpText(helpText);
        })();
}
function onValidate_PROPERTY_DETAILS__Others_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Others_Sum_Insured", "Currency");
        })();
}
function onValidate_PROPERTY_DETAILS__Total_Sum_Insured(source, args, sender, isOnLoad) {
        
        /**
         * @fileoverview
         * GeneralValidation
         */
        (function(){
        	GeneralValidationHandler.Validate(isOnLoad, args, "PROPERTY_DETAILS", "Total_Sum_Insured", "Currency");
        })();
        
        /**
         * @fileoverview Makes a control bold.
         * MakeBold
         */
        (function(){
        	
        	if (isOnLoad) {	
        		var instance;
        		if ("{name}" != "{na" + "me}"){
        			instance = Field.getLabel("{name}");
        		} else { 
        			instance = Field.getInstance("PROPERTY_DETAILS", "Total_Sum_Insured");
        		}
        		
        		// If instance implements setBold, then do it.
        		if (instance.setBold) instance.setBold(true);
        	}
        })();
        /**
         * @fileoverview
         * Read Only, make the field Read Only
         */
        (function(){
        	
        	var field = Field.getInstance("PROPERTY_DETAILS", "Total_Sum_Insured");
        	field.setReadOnly(true);
        })();
        /**
         * @fileoverview
         * SetValue
         */
        (function(){
        	if (isOnLoad) {		
        		// Get the field
        		var field = Field.getWithQuery("type=Currency&objectName=PROPERTY_DETAILS&propertyName=Total_Sum_Insured&name={name}");
        		
        		var value = new Expression("PROPERTY_DETAILS.Building_Sum_Insured + PROPERTY_DETAILS.MPU_Sum_Insured + PROPERTY_DETAILS.Stock_Sum_Insured + PROPERTY_DETAILS.Furniture_Sum_Insured + PROPERTY_DETAILS.Others_Sum_Insured + PROPERTY_DETAILS.Total_Sum_Insured"), 
        			condition = (Expression.isValidParameter("{1}")) ? new Expression("{1}") : null, 
        			elseValue = (Expression.isValidParameter("{2}")) ? new Expression("{2}") : null;
        		
        		window.setValue(field, value, condition, elseValue);
        	};
        })();
        /**
         * @fileoverview
         * This rule is the same as ValidWnen, except the error message can be an expression.
         * This cant be incorporated into the ValidWhen rule as all existing error messages would have to be quoted.
         */
        (function(){
        	
        	if (args && args.IsValid == true){
        	
        		var setInvalid = function(){
        			var message = (Expression.isValidParameter("'Total Sum Insured must be greater than 0'")) ? (new Expression("'Total Sum Insured must be greater than 0'")).valueOf() : null;
        			args.IsValid = false;
        			if (message != null){
        				var node = document.getElementById("ctl00_cntMainBody_val" + "PROPERTY_DETAILS".toUpperCase() + "__" + "Total_Sum_Insured");
        				if (node == null){
        					// Try slightly different format with only one underscore
        					node = document.getElementById("ctl00_cntMainBody_val" + "PROPERTY_DETAILS".toUpperCase() + "_" + "Total_Sum_Insured");
        				}
        				if (node != null){
        					node.errormessage = message;
        				}
        			}
        		};
        	
        		var exp;
        		try {
        			exp = new Expression("PROPERTY_DETAILS.Total_Sum_Insured > 0");
        		} catch (e){
        			setInvalid();
        			return;
        		}
        		if (exp.getValue() != true)
        			setInvalid();
        	}
        })();
}
function DoLogic(isOnLoad) {
    onValidate_PROPERTY_DETAILS__Plot_No(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Street(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Town(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Occuipied_As(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Wall(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Roof(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Building_Sum_Insured(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__MPU_Sum_Insured(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Stock_Sum_Insured(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Furniture_Sum_Insured(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Other_Items(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Others_Sum_Insured(null, null, null, isOnLoad);
    onValidate_PROPERTY_DETAILS__Total_Sum_Insured(null, null, null, isOnLoad);
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
<div id="idc53cf30ad6084e9c822c5e809b567b7a" class="general-layout-container">
				
         
				
					
						
						
							<div class="clearfix p-xs">
						
						
								<!-- ColumnLayoutContainer -->
<div id="id95fedf0902c3419381286a8bc03fe42b" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading6" runat="server" Text="Building Details" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Plot_No" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Plot_No">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Plot_No" runat="server" AssociatedControlID="PROPERTY_DETAILS__Plot_No" 
			Text="Plot Number" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Plot_No" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Plot_No" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Plot Number"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Plot_No"
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
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Street" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Street">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Street" runat="server" AssociatedControlID="PROPERTY_DETAILS__Street" 
			Text="Street" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Street" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Street" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Street"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Street"
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
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Town" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Town">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Town" runat="server" AssociatedControlID="PROPERTY_DETAILS__Town" 
			Text="Town" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Town" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Town" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Town"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Town"
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
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Occuipied_As" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Occuipied_As">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Occuipied_As" runat="server" AssociatedControlID="PROPERTY_DETAILS__Occuipied_As" 
			Text="Building Occupied As" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Occuipied_As" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Occuipied_As" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Building Occupied As"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Occuipied_As"
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
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Wall" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Wall">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Wall" runat="server" AssociatedControlID="PROPERTY_DETAILS__Wall" 
			Text="Wall Construction Material" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Wall" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Wall" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Wall Construction Material"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Wall"
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
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Roof" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Roof">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Roof" runat="server" AssociatedControlID="PROPERTY_DETAILS__Roof" 
			Text="Roofing Material" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Roof" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Roof" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Roofing Material"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Roof"
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
		styleString += "#id95fedf0902c3419381286a8bc03fe42b label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#id95fedf0902c3419381286a8bc03fe42b label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id95fedf0902c3419381286a8bc03fe42b label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id95fedf0902c3419381286a8bc03fe42b label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#id95fedf0902c3419381286a8bc03fe42b input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#id95fedf0902c3419381286a8bc03fe42b input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#id95fedf0902c3419381286a8bc03fe42b input{text-align:left;}"; break;
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
<div id="ida20929a026fd4a878c4132c86d9727ec" class="column-layout-container  ">
		
				
	              <legend><asp:Label ID="lblHeading7" runat="server" Text="Sums Insured" /></legend>
				
				
				<div data-column-count="1" data-column-ratio="" data-layout="" class="clearfix form-horizontal">
				
					<ul class="column-content">
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Building_Sum_Insured" 
		id="pb-container-currency-PROPERTY_DETAILS-Building_Sum_Insured">
		<asp:Label ID="lblPROPERTY_DETAILS_Building_Sum_Insured" runat="server" AssociatedControlID="PROPERTY_DETAILS__Building_Sum_Insured" 
			Text="Building SI" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY_DETAILS__Building_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_DETAILS_Building_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Building SI"
			ClientValidationFunction="onValidate_PROPERTY_DETAILS__Building_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="MPU_Sum_Insured" 
		id="pb-container-currency-PROPERTY_DETAILS-MPU_Sum_Insured">
		<asp:Label ID="lblPROPERTY_DETAILS_MPU_Sum_Insured" runat="server" AssociatedControlID="PROPERTY_DETAILS__MPU_Sum_Insured" 
			Text="Machinery Plant Utensils SI" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY_DETAILS__MPU_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_DETAILS_MPU_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Machinery Plant Utensils SI"
			ClientValidationFunction="onValidate_PROPERTY_DETAILS__MPU_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Stock_Sum_Insured" 
		id="pb-container-currency-PROPERTY_DETAILS-Stock_Sum_Insured">
		<asp:Label ID="lblPROPERTY_DETAILS_Stock_Sum_Insured" runat="server" AssociatedControlID="PROPERTY_DETAILS__Stock_Sum_Insured" 
			Text="Stock SI" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY_DETAILS__Stock_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_DETAILS_Stock_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Stock SI"
			ClientValidationFunction="onValidate_PROPERTY_DETAILS__Stock_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Furniture_Sum_Insured" 
		id="pb-container-currency-PROPERTY_DETAILS-Furniture_Sum_Insured">
		<asp:Label ID="lblPROPERTY_DETAILS_Furniture_Sum_Insured" runat="server" AssociatedControlID="PROPERTY_DETAILS__Furniture_Sum_Insured" 
			Text="Furniture SI" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY_DETAILS__Furniture_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_DETAILS_Furniture_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Furniture SI"
			ClientValidationFunction="onValidate_PROPERTY_DETAILS__Furniture_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Text -->

<div class="form-group form-group-sm">
	<span class="field-container" 
		
		data-field-type="Text" 
		
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Other_Items" 
		 
		
		 
		id="pb-container-text-PROPERTY_DETAILS-Other_Items">

		
		<asp:Label ID="lblPROPERTY_DETAILS_Other_Items" runat="server" AssociatedControlID="PROPERTY_DETAILS__Other_Items" 
			Text="Other Items Description" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		
		
			
		

		
		         <div class="col-md-8 col-sm-9">
					<asp:TextBox ID="PROPERTY_DETAILS__Other_Items" runat="server" CssClass="form-control" data-type="Text" />
					<asp:CustomValidator ID="valPROPERTY_DETAILS_Other_Items" 
					runat="server" 
					Text="*" 
					ErrorMessage="A validation error occurred for Other Items Description"
					ClientValidationFunction="onValidate_PROPERTY_DETAILS__Other_Items"
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
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Others_Sum_Insured" 
		id="pb-container-currency-PROPERTY_DETAILS-Others_Sum_Insured">
		<asp:Label ID="lblPROPERTY_DETAILS_Others_Sum_Insured" runat="server" AssociatedControlID="PROPERTY_DETAILS__Others_Sum_Insured" 
			Text="Other Items SI" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY_DETAILS__Others_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_DETAILS_Others_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Other Items SI"
			ClientValidationFunction="onValidate_PROPERTY_DETAILS__Others_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
									</li>
							
							
						
							
							
								
								
									<li class="co-cell"  style="width:100%;">
								
								
										<!-- Currency --->
<div class="form-group form-group-sm">
	<span class="field-container"
		data-field-type="Currency" 
		data-object-name="PROPERTY_DETAILS" 
		data-property-name="Total_Sum_Insured" 
		id="pb-container-currency-PROPERTY_DETAILS-Total_Sum_Insured">
		<asp:Label ID="lblPROPERTY_DETAILS_Total_Sum_Insured" runat="server" AssociatedControlID="PROPERTY_DETAILS__Total_Sum_Insured" 
			Text="Total Sum Insured" CssClass="col-md-4 col-sm-3 control-label"></asp:Label>
		   <div class="col-md-8 col-sm-9">
		     <asp:TextBox ID="PROPERTY_DETAILS__Total_Sum_Insured" runat="server" CssClass="form-control" />
		   </div>
		<asp:CustomValidator ID="valPROPERTY_DETAILS_Total_Sum_Insured" 
			runat="server" 
			Text="*" 
			ErrorMessage="A validation error occurred for Total Sum Insured"
			ClientValidationFunction="onValidate_PROPERTY_DETAILS__Total_Sum_Insured" 
			ValidationGroup=""
			Display="Dynamic"
			EnableClientScript="true"/>
	</span>
</div>	
<!-- /Currency -->
								
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
		styleString += "#ida20929a026fd4a878c4132c86d9727ec label{width: " + labelWidth + ";}";
	}
	if (labelAlign != ""){
		switch (labelAlign.toLowerCase()){
			case "right": styleString += "#ida20929a026fd4a878c4132c86d9727ec label{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#ida20929a026fd4a878c4132c86d9727ec label{text-align:center;}"; break;
			case "left": 
			default: styleString += "#ida20929a026fd4a878c4132c86d9727ec label{text-align:left;}"; break;
		}
	}
	if (textAlign != ""){
		switch (textAlign.toLowerCase()){
			case "right": styleString += "#ida20929a026fd4a878c4132c86d9727ec input{text-align:right;}"; break;
			case "centre":
			case "center":
			case "middle": styleString += "#ida20929a026fd4a878c4132c86d9727ec input{text-align:center;}"; break;
			case "left": 
			default: styleString += "#ida20929a026fd4a878c4132c86d9727ec input{text-align:left;}"; break;
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