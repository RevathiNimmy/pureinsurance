<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AddressCntrl.ascx.vb"
    Inherits="Nexus.AddressCntrl" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:ScriptManagerProxy runat="server" ID="smpProxy">
    <Services>
        <asp:ServiceReference Path="~/Services/pageServices.asmx"></asp:ServiceReference>
    </Services>
</asp:ScriptManagerProxy>

<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $(".address-change").change(function () {
            $('#<%= txtAddress_Line5.ClientID %>').val("");
            $('#<%= txtAddress_Line6.ClientID %>').val("");
            $('#<%= txtAddress_Line7.ClientID %>').val("");
            $('#<%= txtAddress_Line8.ClientID %>').val("");
            $('#<%= txtAddress_Line9.ClientID %>').val("");
            $('#<%= txtAddress_Line10.ClientID %>').val("");
        });


        $(".postaddress-change").change(function () {
            $('#<%= txtLookup_PropDescription.ClientID %>').val("");
            $('#<%= txtLookup_GNAFID.ClientID %>').val("");
            $('#<%= txtLookup_DPID.ClientID %>').val("");
            $('#<%= txtLookup_DPID_Barcode.ClientID %>').val("");
            $('#<%= txtLookup_Latitude.ClientID %>').val("");
            $('#<%= txtLookup_Longitude.ClientID %>').val("");
        });

    });
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
        addOption('Searching...', '0');

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

    var oAddresses;

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
                var sStreet = oAddress.Street;

                if ($.trim(sOrganisation).length > 0) {
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
            $("#<%= GISLookup_State.ClientID %>").val('');
            $('#<%=GISLookup_State.ClientID %> option').removeAttr("selected");

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
            $('#<%=TxtLookup_County.ClientID %>').val(oProcessedAddress[3]);

            //populate Postcode from the address object
            $('#<%=TxtLookup_Postcode.ClientID %>').val(oProcessedAddress[4]);

            //set the country to UK if there is a matching option. The postcode lookup only handles UK addresses so this makes sense                 
            $("#<%=GISLookup_Country.ClientID %> option:contains('United Kingdom')").attr('selected', 'selected');
            var selectedIndex = $('#<%=GISLookup_Country.ClientID %> option:selected').val();
            $('#<%=GISLookup_Country.ClientID %>').val(selectedIndex).trigger('change');
            //find the text in the state drop down which matches Address line 3  
            selectedState = oProcessedAddress[3];

            //assign newly added values to new textboxes.
            if ($('#<%=txtLookup_PropDescription.ClientID %>')) {
                $('#<%=txtLookup_PropDescription.ClientID %>').val(oAddress.sAddress5 == null ? '' : oAddress.sAddress5);
            }
            if ($('#<%=txtLookup_GNAFID.ClientID %>')) {
                $('#<%=txtLookup_GNAFID.ClientID %>').val(oAddress.sAddress6 == null ? '' : oAddress.sAddress6);
            }
            if ($('#<%=txtLookup_DPID.ClientID %>')) {
                $('#<%=txtLookup_DPID.ClientID %>').val(oAddress.sAddress7 == null ? '' : oAddress.sAddress7);
            }
            if ($('#<%=txtLookup_DPID_Barcode.ClientID %>')) {
                $('#<%=txtLookup_DPID_Barcode.ClientID %>').val(oAddress.sAddress8 == null ? '' : oAddress.sAddress8);
            }
            if ($('#<%=txtLookup_Latitude.ClientID %>')) {
                $('#<%=txtLookup_Latitude.ClientID %>').val(oAddress.sAddress9 == null ? '' : oAddress.sAddress9);
            }
            if ($('#<%=txtLookup_Longitude.ClientID %>')) {
                $('#<%=txtLookup_Longitude.ClientID %>').val(oAddress.sAddress10 == null ? '' : oAddress.sAddress10);
            }
        }
    }

    function validateResponse(res) {
        var error = false;
        $.each($.parseJSON(res), function(key, value) {
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
    function DisableAddressFileds() {
        document.getElementById('<%= txtAddress_Line1.ClientId%>').disabled = true;
        document.getElementById('<%= txtAddress_Line2.ClientId%>').disabled = true;
        document.getElementById('<%= txtAddress_Line3.ClientId%>').disabled = true;
        document.getElementById('<%= TxtAddress_Postcode.ClientId%>').disabled = true;
        document.getElementById('<%= GISAddress_Country.ClientId%>').disabled = true;
        document.getElementById('<%= GISAddressLookup_State.ClientId%>').disabled = true;
    }
</script>

<div id="Controls_AddressCntrl">
    <asp:PlaceHolder ID="PnlPostcodeLookup" runat="server">
        <asp:HiddenField ID="hdnGuid" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hdnEnableSpatialFeilds" runat="server"></asp:HiddenField>
 <asp:HiddenField ID="hdnPostodeUser" runat="server" />
        <asp:HiddenField ID="hdnPostodePass" runat="server" />
        <asp:HiddenField ID="hdnPostcodeSerialNo" runat="server" />
        <div class="form-horizontal">
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblStreet" runat="server" AssociatedControlID="txtLookup_Street" Text="<%$ Resources:lbl_Postcodelookup_Steet %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <asp:TextBox ID="TxtLookup_Street" runat="server" CssClass="form-control postaddress-change"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookup_Street" runat="server" ControlToValidate="TxtLookup_Street" ErrorMessage="<%$ Resources:lbl_ErrMsg_Street %>" ToolTip="<%$ Resources:lbl_TPMsg_Street %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblLocality" runat="server" AssociatedControlID="TxtLookup_Locality" Text="<%$ Resources:lbl_Postcodelookup_Locality %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <asp:TextBox ID="TxtLookup_Locality" runat="server" CssClass="form-control postaddress-change"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookup_Locality" runat="server" ControlToValidate="TxtLookup_Locality" ErrorMessage="<%$ Resources:lbl_ErrMsg_Locality %>" ToolTip="<%$ Resources:lbl_TPMsg_Locality %>" SetFocusOnError="True" Display="none" Enabled="false"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblTown" runat="server" AssociatedControlID="txtLookup_Town" Text="<%$ Resources:lbl_Postcodelookup_Town %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <asp:TextBox ID="TxtLookup_Town" runat="server" CssClass="form-control postaddress-change"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookup_Town" runat="server" ControlToValidate="TxtLookup_Town" ErrorMessage="<%$ Resources:lbl_ErrMsg_Town %>" ToolTip="<%$ Resources:lbl_TPMsg_Town %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
            </div>
            <div id="liStateText" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblCounty" runat="server" AssociatedControlID="txtLookup_County" Text="<%$ Resources:lbl_Postcodelookup_County %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <asp:TextBox ID="TxtLookup_County" runat="server" CssClass="form-control postaddress-change"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookup_County" runat="server" ControlToValidate="TxtLookup_County" ErrorMessage="<%$ Resources:lbl_ErrMsg_County %>" ToolTip="<%$ Resources:lbl_TPMsg_County %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
            </div>
            <div runat="server" id="liCountry" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblCountry" runat="server" AssociatedControlID="GISLookup_Country" Text="<%$ Resources:lbl_Postcodelookup_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <NexusProvider:LookupList ID="GISLookup_Country" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="COUNTRY" DefaultText="(Please Select)" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookup_Country" runat="server" Display="None" InitialValue="" ControlToValidate="GISLookup_Country" ErrorMessage="<%$ Resources:lbl_ErrMsg_Country %>" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtLookup_PostCode" Text="<%$ Resources:lbl_Postcodelookup_Postcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <asp:TextBox ID="TxtLookup_Postcode" runat="server" CssClass="field-small form-control postaddress-change"></asp:TextBox>
                    <input type="button" runat="server" id="btnFindAddress" visible="true" class="field-medium" onclick="findAddress();" value="<%$ Resources:btn_Find %>" validationgroup="PostCodeLookup" >
                </div>
                <div id="addressList" style="display: none;">
                    <select runat="server" id="selAddressList" onchange="selectAddress(this.value);" size="6"></select>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookUp_Postcode" Display="none" runat="server" ControlToValidate="TxtLookup_Postcode" ErrorMessage="<%$ Resources:lbl_ErrMsg_PostCode %>" ToolTip="<%$ Resources:lbl_TPMsg_PostCode %>" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="vldPpostcodeRegex" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_RegExpr_PostCode %>" ControlToValidate="TxtLookup_Postcode" SetFocusOnError="True"></asp:RegularExpressionValidator>
            </div>
            <div id="liStateLookup" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                <asp:Label ID="lblState" runat="server" AssociatedControlID="GISLookup_State" Text="<%$ Resources:lbl_Postcodelookup_County %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                <div class="col-md-8 col-sm-9">
                    <NexusProvider:LookupList ID="GISLookup_State" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="State" DefaultText="(Please Select)" CssClass="field-medium form-control form-select" ParentFieldName="country_id" ParentLookupListID="GISLookup_Country"></NexusProvider:LookupList>
                </div>
                <asp:RequiredFieldValidator ID="RqdLookup_State" runat="server" ControlToValidate="GISLookup_State" ErrorMessage="<%$ Resources:lbl_ErrMsg_County %>" ToolTip="<%$ Resources:lbl_TPMsg_County %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
            </div>
        </div>
        <asp:PlaceHolder ID="PnlSpatialPostCodeLookup" runat="server" Visible="false">
              <legend>
                        <asp:Label ID="lblSpatialheadinglookup" runat="server" Text="<%$ Resources:lblSpatialHeading %>"></asp:Label>
                    </legend>
            <div class="form-horizontal">

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPropertyDescription" runat="server" AssociatedControlID="txtLookup_PropDescription"
                        Text="<%$ Resources:lbl_PnlAddress_Address5 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtLookup_PropDescription" CssClass="form-control"
                            Enabled="true" MaxLength="60"/>
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGNAFID" runat="server" AssociatedControlID="txtLookup_GNAFID"
                        Text="<%$ Resources:lbl_PnlAddress_Address6 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtLookup_GNAFID" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDPID" runat="server" AssociatedControlID="txtLookup_DPID"
                        Text="<%$ Resources:lbl_PnlAddress_Address7 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtLookup_DPID" CssClass="form-control" Enabled="false" />
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDPID_Barcode" runat="server" AssociatedControlID="txtLookup_DPID_Barcode"
                        Text="<%$ Resources:lbl_PnlAddress_Address8 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtLookup_DPID_Barcode" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblLatitude" runat="server" AssociatedControlID="txtLookup_Latitude"
                        Text="<%$ Resources:lbl_PnlAddress_Address9 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtLookup_Latitude" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblLongitude" runat="server" AssociatedControlID="txtLookup_Longitude"
                        Text="<%$ Resources:lbl_PnlAddress_Address10 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtLookup_Longitude" CssClass="form-control" Enabled="false" />
                    </div>

                </div>

            </div>
        </asp:PlaceHolder>

    </asp:PlaceHolder>
    <asp:PlaceHolder ID="PnlAddress" runat="server">
        <div id="address-control2">
            <div class="form-horizontal">
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress1" runat="server" AssociatedControlID="txtAddress_Line1" Text="<%$ Resources:lbl_PnlAddress_Address1 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="TxtAddress_Line1" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdAddress_Line1" runat="server" ControlToValidate="TxtAddress_Line1" ErrorMessage="<%$ Resources:lbl_ErrMsg_Address1 %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress2" runat="server" AssociatedControlID="txtAddress_Line2" Text="<%$ Resources:lbl_PnlAddress_Address2 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="TxtAddress_Line2" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdAddress_Line2" runat="server" ControlToValidate="TxtAddress_Line2" ErrorMessage="<%$ Resources:lbl_ErrMsg_Address2 %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress3" runat="server" AssociatedControlID="txtAddress_Line3" Text="<%$ Resources:lbl_PnlAddress_Address3 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="TxtAddress_Line3" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdAddress_Line3" runat="server" ControlToValidate="TxtAddress_Line3" ErrorMessage="<%$ Resources:lbl_ErrMsg_Address3 %>" Display="none" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                </div>
                <div id="litxtStateText" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress4" runat="server" AssociatedControlID="txtAddress_Line4" Text="<%$ Resources:lbl_PnlAddress_Address4 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="TxtAddress_Line4" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdAddress_Line4" runat="server" ControlToValidate="TxtAddress_Line4" ErrorMessage="<%$ Resources:lbl_ErrMsg_Address4 %>" Display="none" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Panel ID="pnlGISAddressCountry" runat="server">
                        <asp:Label ID="lblAddressCountry" runat="server" AssociatedControlID="GISAddress_Country" Text="<%$ Resources:lbl_PnlAddress_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISAddress_Country" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="COUNTRY" DefaultText="(Please Select)" CssClass="form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdAddress_Country" runat="server" InitialValue="" ControlToValidate="GISAddress_Country" ErrorMessage="<%$ Resources:lbl_ErrMsg_AddressCountry %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                    </asp:Panel>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddressPostCode" runat="server" AssociatedControlID="txtAddress_PostCode" Text="<%$ Resources:lbl_PnlAddress_Postcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="TxtAddress_Postcode" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdAddress_PostCode" runat="server" ControlToValidate="TxtAddress_Postcode" ErrorMessage="<%$ Resources:lbl_ErrMsg_AddressPostcode %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="vldAddresspostcodeRegex" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_RegExpr_PostCode %>" ControlToValidate="TxtAddress_Postcode" SetFocusOnError="True"></asp:RegularExpressionValidator>
                </div>
                <div id="litxtStateLookup" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbltxtState" runat="server" AssociatedControlID="txtAddress_Line4" Text="<%$ Resources:lbl_PnlAddress_Address4 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <NexusProvider:LookupList ID="GISAddressLookup_State" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" AutoPostBack="true"  ListType="PMLookup" ListCode="State" DefaultText="(Please Select)" CssClass="field-medium form-control form-select" ParentFieldName="country_id" ParentLookupListID="GISAddress_Country"></NexusProvider:LookupList>
                    </div>
                    <asp:RequiredFieldValidator ID="RqdAddress_State" runat="server" ControlToValidate="GISAddressLookup_State" ErrorMessage="<%$ Resources:lbl_ErrMsg_County %>" ToolTip="<%$ Resources:lbl_TPMsg_County %>" SetFocusOnError="true" Display="none" Enabled="false"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:PlaceHolder ID="PnlSpatial" runat="server" Visible="false">
            <div class="form-horizontal">
                 <legend>
                        <asp:Label ID="lblSpatialHeading" runat="server" Text="<%$ Resources:lblSpatialHeading %>"></asp:Label>
                    </legend>
                <div runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress5" runat="server" AssociatedControlID="txtAddress_Line5"
                        Text="<%$ Resources:lbl_PnlAddress_Address5 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtAddress_Line5" CssClass="form-control" Enabled="true" MaxLength="60"/>
                    </div>

                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress6" runat="server" AssociatedControlID="txtAddress_Line6"
                        Text="<%$ Resources:lbl_PnlAddress_Address6 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtAddress_Line6" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress7" runat="server" AssociatedControlID="txtAddress_Line7"
                        Text="<%$ Resources:lbl_PnlAddress_Address7 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtAddress_Line7" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress8" runat="server" AssociatedControlID="txtAddress_Line8"
                        Text="<%$ Resources:lbl_PnlAddress_Address8 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtAddress_Line8" CssClass="form-control" Enabled="false" />
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblAddress9" runat="server" AssociatedControlID="txtAddress_Line9"
                        Text="<%$ Resources:lbl_PnlAddress_Address9 %>" class="col-md-4 col-sm-3 control-label" />
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox runat="server" ID="txtAddress_Line9" CssClass="form-control" Enabled="false" />
                    </div>

                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                <asp:Label ID="lblAddress10" runat="server" AssociatedControlID="txtAddress_Line10"
                    Text="<%$ Resources:lbl_PnlAddress_Address10 %>" class="col-md-4 col-sm-3 control-label" />
                <div class="col-md-8 col-sm-9">
                    <asp:TextBox runat="server" ID="txtAddress_Line10" CssClass="form-control" Enabled="false" MaxLength="60" />
                </div>
            </div>

            </div>
                </asp:PlaceHolder>
            



        </div>
		 <script runat="server">
		     Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    </asp:PlaceHolder>
</div>
