<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Address.aspx.vb" Inherits="Nexus.Modal_Address"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server" ID="sm">
        <Services>
            <asp:ServiceReference Path="~/services/pageServices.asmx"></asp:ServiceReference>
        </Services>
    </asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {

            $(".address-change").change(function () {
                $('#<%= txtLookup_GNAFID.ClientID %>').val("");
                $('#<%= txtLookup_DPID.ClientID %>').val("");
                $('#<%= txtLookup_DPID_Barcode.ClientID %>').val("");
                $('#<%= txtLookup_Latitude.ClientID %>').val("");
                $('#<%= txtLookup_Longitude.ClientID %>').val("");
            });

        });


        var manager = Sys.WebForms.PageRequestManager.getInstance();

        manager.add_endRequest(function () {
            $(".address-change").change(function () {
                $('#<%= txtLookup_GNAFID.ClientID %>').val("");
                $('#<%= txtLookup_DPID.ClientID %>').val("");
                $('#<%= txtLookup_DPID_Barcode.ClientID %>').val("");
                $('#<%= txtLookup_Latitude.ClientID %>').val("");
                $('#<%= txtLookup_Longitude.ClientID %>').val("");
            });
        });

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }
        function CheckDuplicateRecord(src, args) {
            //src,args
            var oDDL;
            args.IsValid = true;
            var iCount = 0;
            var sCorrespondanceAddress = "3131 XCO";
            var AddressKey = document.getElementById('<%=AddressKey.ClientID %>').value;

            oDDL = document.getElementById('<%=GISLookup_Type.ClientID %>');
            var sAddressCollection = document.getElementById('<%=AddressType.ClientID %>').value.split(";");
            if (sAddressCollection.length > 0) {

                //Need this function to find the duplecate values in case of "Update"
                if (document.getElementById('<%=txtMode.ClientID %>').value == "Update") {
                    //Update Mode
                    //it replaces the value temporarily then count
                    if (oDDL.options[oDDL.selectedIndex].value == sCorrespondanceAddress) {
                        sAddressCollection[AddressKey] = oDDL.options[oDDL.selectedIndex].value;
                    }
                }

                for (var CountVar = 0; CountVar < sAddressCollection.length; CountVar++) {
                    //Add Mode
                    if (document.getElementById('<%=txtMode.ClientID %>').value == "Add") {
                        if (oDDL.options[oDDL.selectedIndex].value == sCorrespondanceAddress && sAddressCollection[CountVar] == oDDL.options[oDDL.selectedIndex].value) {
                            args.IsValid = false;
                        }
                    }
                    else {
                        //Update Mode
                        if (oDDL.options[oDDL.selectedIndex].value == sCorrespondanceAddress && sAddressCollection[CountVar] == oDDL.options[oDDL.selectedIndex].value) {
                            iCount = iCount + 1;
                        }
                    }
                }
                if (iCount > 1 && document.getElementById('<%=txtMode.ClientID %>').value == "Update") {
                    //For Update Mode if validation fails
                    args.IsValid = false;
                }
            }
        }
        function UpdateAddressData() {
            var AddressData;
            var oDDL;
            //Fire Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Get address data to pass to parent page
                AddressData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                oDDL = document.getElementById('<%=GISLookup_Type.ClientID %>');
                AddressData += oDDL.options[oDDL.selectedIndex].value + ";";
                AddressData += document.getElementById('<%=TxtLookup_Street.ClientID %>').value + ";";
                AddressData += document.getElementById('<%=TxtLookup_Locality.ClientID %>').value + ";";
                AddressData += document.getElementById('<%=TxtLookup_Town.ClientID %>').value + ";";
                if (document.getElementById('<%=hdnstate.ClientID %>').value == "1") {
                    oDDL = document.getElementById('<%=GISLookup_State.ClientID %>');
                    //State Description
                    AddressData += oDDL.options[oDDL.selectedIndex].text + ";";
                    //StateCode
                    AddressData += oDDL.options[oDDL.selectedIndex].value + ";";
                }
                else {
                    AddressData += document.getElementById('<%=TxtLookup_State.ClientID %>').value + ";";

                    AddressData += ";";
                }
                AddressData += document.getElementById('<%=TxtLookup_Postcode.ClientID %>').value + ";";
                oDDL = document.getElementById('<%=GISLookup_Country.ClientID %>');
                AddressData += oDDL.options[oDDL.selectedIndex].value + ";";
                AddressData += oDDL.options[oDDL.selectedIndex].text + ";";
                AddressData += document.getElementById('<%=AddressKey.ClientID %>').value + ";";

                var oExtraField;
                oExtraField = document.getElementById('<%=txtLookup_PropDescription.ClientID %>');
                AddressData += oExtraField != null ? oExtraField.value + ";" : ";";
                oExtraField = document.getElementById('<%=txtLookup_GNAFID.ClientID %>');
        AddressData += oExtraField != null ? oExtraField.value + ";" : ";";
        oExtraField = document.getElementById('<%=txtLookup_DPID.ClientID %>');
        AddressData += oExtraField != null ? oExtraField.value + ";" : ";";
        oExtraField = document.getElementById('<%=txtLookup_DPID_Barcode.ClientID %>');
                AddressData += oExtraField != null ? oExtraField.value + ";" : ";";
                oExtraField = document.getElementById('<%=txtLookup_Latitude.ClientID %>');
                AddressData += oExtraField != null ? oExtraField.value + ";" : ";";
                oExtraField = document.getElementById('<%=txtLookup_Longitude.ClientID %>');
                AddressData += oExtraField != null ? oExtraField.value + ";" : ";";

                //close modal and pass data back
                self.parent.tb_remove();
                self.parent.ReceiveAddressData(AddressData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }

            else {
                self.parent.resizeDialog($('.page-container'));
            }
        }

        function findAddress() {
            //make call back to lookup addresses that match postcode entered
            var postCode = $('#<%=TxtLookup_Postcode.ClientID %>').val();
            if (postCode.length == 0)
                return false;
            var guid = $('#<%=hdnGuid.ClientID %>').val();
            //clear drop down list and add single "Please wait..." option
            $("select[id$=<%=selAddressList.ClientID %>] > option").remove();
            $('#addressList').show(100);
            $('#<%=selAddressList.ClientID %>').attr('disabled', 'disabled'); //so that nothing happens if user clicks whilst we are getting the results
            addOption('Searching...', '0'); //todo - should be in resource file

            var postcodeUser = $('#<%=hdnPostodeUser.ClientID %>').val();
            var postcodePass = $('#<%=hdnPostodePass.ClientID %>').val();
            var postcodeSerialNo = $('#<%=hdnPostcodeSerialNo.ClientID %>').val();

            var postcodeQueryString = "data=address&task=lookup&fields=standard&lookup=" + postCode + "&serial=" + postcodeSerialNo + "&password=" + postcodePass + "&callback=json";

            //make ajax call to get addresses
            
            pageServices.FindPostLookupAddresses(postcodeUser, postcodePass, postcodeSerialNo, postCode, postcodeQueryString, guid, OnSucceeded, OnFailed);
        }

        function addOption(text, value) {
            var optn = document.createElement("option");
            optn.text = text;
            optn.value = value;
            document.getElementById('<%=selAddressList.ClientID %>').options.add(optn);
        }

        var oAddressResult;
        function OnSucceeded(res, destCtrl) {
           
            if (validateResponse(res)) {
                $('#addressList').hide(100);
                return;
            }

            //get address object returned as json

            var oAddresses = $.parseJSON(res);
            oAddressResult = oAddresses.Item;

            if (oAddressResult.length == 1) {
                selectAddress(0);
            }

            if (oAddressResult.length > 0) {
                //clear the drop down then populate with results
                $("select[id$=<%=selAddressList.ClientID %>] > option").remove();
                $("select[id$=<%=selAddressList.ClientID %>]").attr("size", oAddressResult.length);               
                for (var i = 0; i <= oAddressResult.length - 1; i++) {
                    var oAddress = oAddressResult[i];
                    var sOrganisation = oAddress.Organisation;
                    var sProperty = oAddress.Property;
                    var sStreet  = oAddress.Street;                   
                    
                    if ($.trim(sOrganisation).length > 0)
                    {
                        sOrganisation = sOrganisation + ",";
                    }

                    if ($.trim(sProperty).length > 0) {
                        sProperty = sProperty + ",";
                    }

                    var sAddressLine1 = sOrganisation + sProperty + oAddress.Street;
                    var thisAddress = sAddressLine1 + "," + oAddress.Locality + "," + oAddress.Town + "," + oAddress.TraditionalCounty + "," + oAddress.Postcode;
                    //Create thisaddress from the  JSON oAddress property and add to the select option
                    addOption(thisAddress, i)
                }
                $("select[id$=<%=selAddressList.ClientID %>]").prepend('<option value="Please Select" >--Please Select--</option>').val('Please Select');
                $('#<%=selAddressList.ClientID %>').removeAttr('disabled');
            }
            else {
                alert("<%= GetLocalResourceObject("err_NoMatch").ToString() %>");
                $('#addressList').hide(100);

                $('#<%=TxtLookup_Street.ClientID %>').val('');
                $('#<%=TxtLookup_Locality.ClientID %>').val('');
                $('#<%=TxtLookup_Town.ClientID %>').val('');
                $('#<%=TxtLookup_Postcode.ClientID %>').val('');
                $('#<%=GISLookup_Country.ClientID %>').val('');
                $('#<%=GISLookup_Country.ClientID %> option').removeAttr("selected");
                if (document.getElementById('<%=hdnstate.ClientID %>').value == "1") {
                    $("#<%= GISLookup_State.ClientID %>").val('');
                    $('#<%=GISLookup_State.ClientID %> option').removeAttr("selected");
                }
                else {
                    $("#<%= TxtLookup_State.ClientID %>").val('');
                }
            }
        }

        function OnFailed(res, destCtrl) {
            $('#addressList').hide(100);
            alert(res._message);
        }

        var selectedState;
        function selectAddress(i) {
            if (i != "Please Select") {
                $('#addressList').hide(100);                
                var oAddress = oAddressResult[i].Street + "," + oAddressResult[i].Locality + "," + oAddressResult[i].Town + "," + oAddressResult[i].TraditionalCounty + "," + oAddressResult[i].Postcode;
                var oAddressCollection = oAddress.split(',');
                var oProcessedAddress = new Array(5);
                var addressLength = oAddressCollection.length;

                // Street 
                var sOrganisation = oAddressResult[i].Organisation;
                var sProperty = oAddressResult[i].Property;
                var sStreet = oAddressResult[i].Street;
                if ($.trim(sOrganisation).length > 0) {
                    sOrganisation = sOrganisation + ", ";
                }
                if ($.trim(sProperty).length > 0) {
                    sProperty = sProperty + ", ";
                }
                var sAddressLine1 = sOrganisation + sProperty + oAddressResult[i].Street;
                oProcessedAddress[0] = $.trim(sAddressLine1);

                // Locality                    
                oProcessedAddress[1] = $.trim(oAddressCollection[1]);

                // Town           
                oProcessedAddress[2] = $.trim(oAddressCollection[2])

                // State            
                oProcessedAddress[3] = $.trim(oAddressCollection[3]);

                // Postcode
                oProcessedAddress[4] = $.trim(oAddressCollection[4]);

                //populate textboxes from the address object
                $('#<%=TxtLookup_Street.ClientID %>').val(oProcessedAddress[0]);
                $('#<%=TxtLookup_Locality.ClientID %>').val(oProcessedAddress[1]);
                $('#<%=TxtLookup_Town.ClientID %>').val(oProcessedAddress[2]);

                //populate Postcode from the address object
                $('#<%=TxtLookup_Postcode.ClientID %>').val(oProcessedAddress[4]);

                //set the country to UK if there is a matching option. The postcode lookup only handles UK addresses so this makes sense                 
                $("#<%=GISLookup_Country.ClientID %> option:contains('United Kingdom')").attr('selected', 'selected');
                var selectedIndex = $('#<%=GISLookup_Country.ClientID %> option:selected').val();
                $('#<%=GISLookup_Country.ClientID %>').val(selectedIndex).trigger('change');
                //find the text in the state drop down which matches Address line 3
                selectedState = oProcessedAddress[3]

               
            }
            else {
                $('#<%=TxtLookup_Street.ClientID %>').val('');
                $('#<%=TxtLookup_Locality.ClientID %>').val('');
                $('#<%=TxtLookup_Town.ClientID %>').val('');

                //populate Postcode from the address object
                $('#<%=TxtLookup_Postcode.ClientID %>').val('');
            }
        }

        function validateResponse(res) {
            var error = false;
            $.each($.parseJSON(res), function (key, value) {
                // Error Handling 
                if (key == "error") {
                    switch (value) {
                        case "NO_SERVICE_CONFIGURED":
                            errorMsg = "<%= GetLocalResourceObject("err_NoServiceConfigured").ToString()%>";
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
                        case "SERIALNUMBER_NOT_CONFIGURED":
                            errorMsg = "<%= GetLocalResourceObject("err_NoSerialNumConfigured").ToString()%>";
                            alert(errorMsg);
                            break;
                        case "INVALID_RESPONSE":
                            errorMsg = "<%= GetLocalResourceObject("err_InvalidServiceResponse").ToString()%>";
                            alert(errorMsg);
                            break;
                        case "SERVICE_ERROR":
                            errorMsg = "<%= GetLocalResourceObject("err_ServiceError").ToString()%>";
                            alert(errorMsg);
                            break;
                        default:
                            alert(value);
                    }
                    error = true;
                }
            });
            return error;
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                for (i = 0; i < sender._asyncPostBackControlClientIDs.length; i++) {
                    if (sender._asyncPostBackControlClientIDs[i] == $('#<%=GISLookup_Country.ClientID %>').attr("id") && selectedState != undefined && selectedState != '') {
                        if (document.getElementById('<%=hdnstate.ClientID %>').value == "1") {
                            var matchingValue = $('#<%= GISLookup_State.ClientID %> option').filter(function () {
                                return $.trim($('#<%= GISLookup_State.ClientID %> [value~=' + this.value + ']').text().toLowerCase()) == $.trim(selectedState.toLowerCase());
                            }).attr('value');
                            $("#<%= GISLookup_State.ClientID %>").val(matchingValue);
                        }
                        else {
                            $("#<%= TxtLookup_State.ClientID %>").val(selectedState);
                        }
                        selectedState = '';
                        break;
                    }
                }
            });
        }
    </script>

    <div id="Modal_Address">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Address_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Address_Details %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Label ID="lblAddressType" runat="server" AssociatedControlID="GISLookup_Type" Text="<%$ Resources:lbl_Address_Type %>" class="col-md-3 control-label"></asp:Label>
                        <div class="col-md-6">
                            <NexusProvider:LookupList ID="GISLookup_Type" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Address_usage_type" CssClass=" form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdAddress_Type" runat="server" InitialValue="" ValidationGroup="AddressGroup" ControlToValidate="GISLookup_Type" ErrorMessage="<%$ Resources:Err_RqdAddress_Type %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Label ID="lblStreet" runat="server" AssociatedControlID="txtLookup_Street" Text="<%$ Resources:lbl_Address_Street %>" class="col-md-3 control-label"></asp:Label>
                        <div class="col-md-6">
                            <asp:TextBox ID="TxtLookup_Street" runat="server" CssClass="form-control" CausesValidation="true"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdLookup_Street" runat="server" ControlToValidate="TxtLookup_Street" ValidationGroup="AddressGroup" ErrorMessage="<%$ Resources:Err_RqdLookup_Street %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Label ID="lblLocality" runat="server" AssociatedControlID="TxtLookup_Locality" Text="<%$ Resources:lbl_Address_Locality %>" class="col-md-3 control-label"></asp:Label>
                        <div class="col-md-6">
                            <asp:TextBox ID="TxtLookup_Locality" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdLookup_Locality" runat="server" ControlToValidate="TxtLookup_Locality" ValidationGroup="AddressGroup" ErrorMessage="<%$ Resources:Err_RqdLookup_Locality %>" SetFocusOnError="True" Display="none" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Label ID="lblTown" runat="server" AssociatedControlID="txtLookup_Town" Text="<%$ Resources:lbl_Address_Town %>" class="col-md-3 control-label"></asp:Label>
                        <div class="col-md-6">
                            <asp:TextBox ID="TxtLookup_Town" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdLookup_Town" runat="server" ControlToValidate="TxtLookup_Town" ValidationGroup="AddressGroup" ErrorMessage="<%$ Resources:Err_RqdLookup_Town %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtLookup_PostCode" Text="<%$ Resources:lbl_Address_PostCode %>" class="col-md-3 control-label"></asp:Label>
                        <div class="col-md-6">
                            <asp:TextBox ID="TxtLookup_Postcode" runat="server" CssClass="field-Postcode form-control"></asp:TextBox>
                            <input type="button" runat="server" id="btnFindAddress" visible="false" class="field-small" onclick="findAddress();" value="<%$ Resources:btn_Find %>">
                        </div>
                        <div id="addressList" style="display: none;">
                            <select runat="server" id="selAddressList" onchange="selectAddress(this.value);" size="6" visible="false"></select>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdLookUp_Postcode" Display="none" runat="server" ValidationGroup="AddressGroup" ControlToValidate="TxtLookup_Postcode" ErrorMessage="<%$ Resources:Err_RqdLookUp_Postcode %>" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="vldPpostcodeRegex" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_RegExpr_PostCode %>" ControlToValidate="TxtLookup_Postcode" SetFocusOnError="True" ValidationGroup="AddressGroup" EnableClientScript="true"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Panel ID="pnlGISLookupCountry" runat="server">
                            <asp:Label ID="lblCountry" runat="server" AssociatedControlID="GISLookup_Country" Text="<%$ Resources:lbl_Address_Country %>" class="col-md-3 control-label"></asp:Label>
                             <div class="col-md-6">
                                <NexusProvider:LookupList ID="GISLookup_Country" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="COUNTRY" CssClass=" form-control address-change form-select"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="RqdLookup_Country" ValidationGroup="AddressGroup" runat="server" Display="None" InitialValue="" ControlToValidate="GISLookup_Country" ErrorMessage="<%$ Resources:Err_RqdLookup_Country %>" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                        </asp:Panel>
                    </div>
                    <div class="form-group form-group-sm col-md-8 col-sm-6">
                        <asp:Label ID="lblState" runat="server" AssociatedControlID="GISLookup_State" Text="<%$ Resources:lbl_Address_State %>" class="col-md-3 control-label"></asp:Label>
                        <asp:UpdatePanel ID="UpdateState" runat="server" UpdateMode="Conditional" class="col-md-6">
                            <ContentTemplate>
                                <div>
                                    <asp:TextBox ID="TxtLookup_State" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    <NexusProvider:LookupList ID="GISLookup_State" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="STATE" CssClass=" form-control address-change form-select" ParentLookupListID="GISLookup_Country" ParentFieldName="COUNTRY_ID" Visible="false"></NexusProvider:LookupList>
                                </div>
                                <asp:HiddenField ID="hdnstate" runat="server" Value="0"></asp:HiddenField>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="GISLookup_Country" EventName="SelectedIndexChange"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <Nexus:ProgressIndicator ID="uprogState" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdateState" runat="server">
                        </Nexus:ProgressIndicator>
                        <asp:RequiredFieldValidator ID="RqdLookup_State" runat="server" ControlToValidate="GISLookup_State"
                            ErrorMessage="<%$ Resources:Err_RqdLookup_State %>" ValidationGroup="AddressGroup"
                            SetFocusOnError="true" Display="none" Enabled="false" />
                        <asp:RequiredFieldValidator ID="RqdFindLookup_State" runat="server" ControlToValidate="GISLookup_State" ErrorMessage="<%$ Resources:Err_RqdLookup_State %>"
                            ValidationGroup="AddressGroup" SetFocusOnError="true" Display="none" Enabled="false">
                        </asp:RequiredFieldValidator>
                    </div>

                </div>
            </div>

            <div class="card-body clearfix">
                <asp:PlaceHolder ID="PnlSpatialDetails" runat="server" Visible="false">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblSpatialHeading" runat="server" Text="<%$ Resources:lblSpatialHeading %>"></asp:Label>
                    </legend>

                    <div id="divPropDescription" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                        <asp:Label ID="lblPropertyDescription" runat="server" AssociatedControlID="txtLookup_PropDescription"
                            Text="<%$ Resources:lbl_Address_PropertyDesc %>" class="col-md-4 col-sm-3 control-label">
                        </asp:Label>

                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLookup_PropDescription" runat="server" CssClass="form-control"  MaxLength="60"></asp:TextBox>
                        </div>
                    </div>
                    <div id="divGNAFID" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" >
                        <asp:Label ID="lblGNAFID" runat="server" AssociatedControlID="txtLookup_GNAFID" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_Address_GNAF_ID %>" />
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLookup_GNAFID" runat="server" CssClass="form-control" MaxLength="60"  Enabled="false"/>
                        </div>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" id="divDPID" runat="server" >
                        <asp:Label ID="lblDPID" runat="server" AssociatedControlID="txtLookup_DPID" CssClass="col-md-4 col-sm-3 control-label" Text="<%$ Resources:lbl_Address_DPID %>" />
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLookup_DPID" runat="server" CssClass="form-control" Enabled="false" MaxLength="60" />
                        </div>

                    </div>
                    <div id="divDPID_Barcode" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" >
                        <asp:Label ID="lblDPID_Barcode" runat="server" AssociatedControlID="txtLookup_DPID_Barcode" CssClass="col-md-4 col-sm-3 control-label"
                            Text="<%$ Resources:lbl_Address_DPID_Barcode %>" />
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLookup_DPID_Barcode" runat="server" CssClass="form-control" MaxLength="60" Enabled="false" />
                        </div>

                    </div>
                    <div id="divDPID_Latitude" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" >
                        <asp:Label ID="lblLatitude" runat="server" AssociatedControlID="txtLookup_Latitude" CssClass="col-md-4 col-sm-3 control-label"
                            Text="<%$ Resources:lbl_Address_Latitude %>" />
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLookup_Latitude" runat="server" CssClass="form-control" MaxLength="60" Enabled="false"/>
                        </div>

                    </div>
                    <div id="divDPID_Longitude" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" >
                        <asp:Label ID="lblLongitude" runat="server" AssociatedControlID="txtLookup_Longitude" CssClass="col-md-4 col-sm-3 control-label"
                            Text="<%$ Resources:lbl_Address_Longitude %>" />
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLookup_Longitude" runat="server" CssClass="form-control" MaxLength="60" Enabled="false" />
                        </div>
                    </div>

                </div>
                    </asp:PlaceHolder>

            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnUpdateAddress" runat="server" Text="<%$ Resources:lbl_Address_Update %>" Visible="false" ValidationGroup="AddressGroup" CausesValidation="true" OnClientClick="UpdateAddressData()" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnAddAddress" runat="server" Text="<%$ Resources:lbl_Address_Add %>" ValidationGroup="AddressGroup" CausesValidation="true" OnClientClick="UpdateAddressData()" SkinID="btnPrimary" style="margin-right:25px;"></asp:LinkButton>
            </div>
        </div>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="AddressType" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="AddressKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnGuid" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnEnableSpatialFeilds" runat="server"></asp:HiddenField>
        <asp:CustomValidator ID="custValidator" runat="server" ErrorMessage="<%$ Resources:lbl_CustValidation %>" Display="None" ValidationGroup="AddressGroup" ClientValidationFunction="CheckDuplicateRecord"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_Address_ValidationSummary %>" runat="server" ValidationGroup="AddressGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:HiddenField ID="hdnPostodeUser" runat="server" />
        <asp:HiddenField ID="hdnPostodePass" runat="server" />
        <asp:HiddenField ID="hdnPostcodeSerialNo" runat="server" />
	</div>
	 <script runat="server">
        Protected Sub Page_Load_PostCode(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            hdnPostodeUser.Value = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"),
                       Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()) _
                       .AddressControl.PostcodeUser
            hdnPostodePass.Value = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"),
                       Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()) _
                       .AddressControl.PostcodePass
            hdnPostcodeSerialNo.Value = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"),
                       Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()) _
                       .AddressControl.PostcodeSerialNo
        End Sub
    </script>
</asp:Content>
