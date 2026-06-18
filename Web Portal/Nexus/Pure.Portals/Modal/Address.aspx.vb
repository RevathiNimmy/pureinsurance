Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.IO
Imports System.Web.UI
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Xml

Namespace Nexus
    Partial Class Modal_Address : Inherits System.Web.UI.Page
        Private bShowCountry As Boolean = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressControl.ShowCountry
        Dim oAddress As New NexusProvider.Address()
        Private sAddressMandatoryFields As String 
        Private sEnableExtraAddressFields As String = CType(AppSettings("EnableExtraAddressFields"), String)
        Dim oParty As NexusProvider.BaseParty = Nothing
        Private icountrykey As Integer
        Private bis_state_lookup As Boolean
        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
            'PnlPostcodeLookup.Visible = True
            Dim SetAddressScript As String = "function " & Me.ID & "_SetAddress(addressString){var addr; " & _
            " addr = addressString.split(', '); " & _
            " document.getElementById('" & TxtLookup_Street.ClientID & "').value=addr[0]; " & _
            " if (addr.length == 4) {  " & _
            " document.getElementById('" & TxtLookup_Locality.ClientID & "').value=addr[1]; " & _
            " document.getElementById('" & TxtLookup_Town.ClientID & "').value=addr[2]; " & _
            " } else if (addr.length == 3) {  " & _
            " document.getElementById('" & TxtLookup_Locality.ClientID & "').value='';} " & _
            " document.getElementById('" & TxtLookup_Town.ClientID & "').value=addr[1];}"

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim iPortalID As Integer
            If HttpContext.Current.Cache("PortalID") IsNot Nothing Then
                iPortalID = HttpContext.Current.Cache("PortalID")
            Else
                iPortalID = Portal.Functions.GetPortalID
            End If
            If (iPortalID = 0) Then
                sAddressMandatoryFields = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressMandatoryFields
            Else
                sAddressMandatoryFields = oNexusConfig.Portals.Portal(iPortalID).AddressMandatoryFields

            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "SetAddressScript_" & Me.ID, SetAddressScript, True)
            ' pnlGISAddressCountry.Visible = bShowCountry
            'Mandatory Field specified
            If Not String.IsNullOrEmpty(sAddressMandatoryFields) And Session(CNMode) <> Mode.View Then
                If sAddressMandatoryFields.Contains("AT") Then
                    lblAddressType.Visible = True
                    RqdAddress_Type.Enabled = True
                End If
                If sAddressMandatoryFields.Contains("1") Then
                    'lblStreetRequired.Visible = True
                    TxtLookup_Street.CssClass = TxtLookup_Street.CssClass & " field-mandatory"
                    RqdLookup_Street.Enabled = True
                End If
                If sAddressMandatoryFields.Contains("2") Then
                    'lblLocalityRequired.Visible = True
                    TxtLookup_Locality.CssClass = TxtLookup_Locality.CssClass & " field-mandatory"
                    RqdLookup_Locality.Enabled = True
                End If
                If sAddressMandatoryFields.Contains("3") Then
                    'lblTownRequired.Visible = True
                    TxtLookup_Town.CssClass = TxtLookup_Town.CssClass & " field-mandatory"
                    RqdLookup_Town.Enabled = True
                End If
                If sAddressMandatoryFields.Contains("PC") Then
                    'lblPostCodeRequired.Visible = True
                    TxtLookup_Postcode.CssClass = TxtLookup_Postcode.CssClass & " field-mandatory"
                    RqdLookUp_Postcode.Enabled = True
                End If
                If sAddressMandatoryFields.Contains("CO") Then
                    'lblCountryRequired.Visible = True
                    GISLookup_Country.CssClass = GISLookup_Country.CssClass & " field-mandatory"
                    RqdLookup_Country.Enabled = True
                End If
                'Can have 4 for State
                If sAddressMandatoryFields.Contains("4") Then
                    'lblStateRequired.Visible = True
                    GISLookup_State.CssClass = GISLookup_State.CssClass & " field-mandatory"
                    RqdLookup_State.Enabled = True
                End If
            End If
            'Additional Address Fields Enabled
            If Not String.IsNullOrEmpty(sEnableExtraAddressFields) And Session(CNMode) <> Mode.View Then
                If Not sEnableExtraAddressFields.Contains("Address5") Then
                    txtLookup_PropDescription.Enabled = False
                Else
                    txtLookup_PropDescription.Enabled = True
                End If
                If Not sEnableExtraAddressFields.Contains("Address6") Then
                    txtLookup_GNAFID.Enabled = False
                Else
                    txtLookup_GNAFID.Enabled = True
                End If
                If Not sEnableExtraAddressFields.Contains("Address7") Then
                    txtLookup_DPID.Enabled = False
                Else
                    txtLookup_DPID.Enabled = True
                End If
                If Not sEnableExtraAddressFields.Contains("Address8") Then
                    txtLookup_DPID_Barcode.Enabled = False
                Else
                    txtLookup_DPID_Barcode.Enabled = True
                End If
                If Not sEnableExtraAddressFields.Contains("Address9") Then
                    txtLookup_Latitude.Enabled = False
                Else
                    txtLookup_Latitude.Enabled = True
                End If
                If Not sEnableExtraAddressFields.Contains("Address10") Then
                    txtLookup_Longitude.Enabled = False
                Else
                    txtLookup_Longitude.Enabled = True
                End If
            End If

        End Sub

        Public Property Address() As NexusProvider.Address
            Get

                Dim oAddress As New NexusProvider.Address(Address1, PostCode, CountryCode)

                With oAddress
                    .AddressType = Type
                    .Address1 = Address1
                    .Address2 = Address2
                    .Address3 = Address3
                    .Address4 = Address4
                    .PostCode = PostCode
                    .CountryCode = CountryCode
                    .Address5 = Address5
                    .Address6 = Address6
                    .Address7 = Address7
                    .Address8 = Address8
                    .Address9 = Address9
                    .Address10 = Address10
                End With
                Return oAddress
            End Get
            Set(ByVal value As NexusProvider.Address)

                If value Is Nothing Then
                    Type = String.Empty
                    Address1 = String.Empty
                    Address2 = String.Empty
                    Address3 = String.Empty
                    Address4 = String.Empty
                    PostCode = String.Empty
                    CountryCode = String.Empty
                    Address5 = String.Empty
                    Address6 = String.Empty
                    Address7 = String.Empty
                    Address8 = String.Empty
                    Address9 = String.Empty
                    Address10 = String.Empty
                Else
                    With value
                        Address1 = .Address1
                        Address2 = .Address2
                        Address3 = .Address3
                        Address4 = .Address4
                        PostCode = .PostCode
                        CountryCode = .CountryCode
                        Address5 = .Address5
                        Address6 = .Address6
                        Address7 = .Address7
                        Address8 = .Address8
                        Address9 = .Address9
                        Address10 = .Address10
                    End With
                End If
            End Set
        End Property

        Public Property Type() As NexusProvider.AddressType
            Get
                Return GISLookup_Type.Value
            End Get
            Set(ByVal value As NexusProvider.AddressType)
                GISLookup_Type.Value = value
            End Set
        End Property

        Public Property Address1() As String
            Get
                Return Me.TxtLookup_Street.Text
            End Get
            Set(ByVal value As String)
                Me.TxtLookup_Street.Text = value
            End Set
        End Property

        Public Property Address2() As String
            Get
                Return Me.TxtLookup_Locality.Text
            End Get
            Set(ByVal value As String)
                Me.TxtLookup_Locality.Text = value
            End Set
        End Property

        Public Property Address3() As String
            Get
                Return Me.TxtLookup_Town.Text
            End Get
            Set(ByVal value As String)
                Me.TxtLookup_Town.Text = value
            End Set
        End Property

        Public Property Address4() As String
            Get
                Return TxtLookup_State.Text
            End Get
            Set(ByVal value As String)
                TxtLookup_State.Text = value
            End Set
        End Property

        Public ReadOnly Property StateCode() As String
            Get
                Return Me.GISLookup_State.Value
            End Get
        End Property

        Public Property PostCode() As String
            Get
                Return Me.TxtLookup_Postcode.Text
            End Get
            Set(ByVal value As String)
                Me.TxtLookup_Postcode.Text = value
            End Set
        End Property

        Public ReadOnly Property Country() As String
            Get
                Return GISLookup_Country.Text
            End Get
        End Property

        Public Property CountryCode() As String
            Get
                Return GISLookup_Country.Value
            End Get
            Set(ByVal value As String)
                GISLookup_Country.Value = value
            End Set
        End Property

        ''' <summary>
        ''' Address Line5 of Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address5() As String
            Get
                Return Me.txtLookup_PropDescription.Text
            End Get
            Set(ByVal value As String)
                Me.txtLookup_PropDescription.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Address Line6 of Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address6() As String
            Get
                Return Me.txtLookup_GNAFID.Text
            End Get
            Set(ByVal value As String)
                Me.txtLookup_GNAFID.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Address Line7 of Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address7() As String
            Get
                Return Me.txtLookup_DPID.Text
            End Get
            Set(ByVal value As String)
                Me.txtLookup_DPID.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Address Line8 of Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address8() As String
            Get
                Return Me.txtLookup_DPID_Barcode.Text
            End Get
            Set(ByVal value As String)
                Me.txtLookup_DPID_Barcode.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Address Line9 of Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address9() As String
            Get
                Return Me.txtLookup_Latitude.Text
            End Get
            Set(ByVal value As String)
                Me.txtLookup_Latitude.Text = value
            End Set
        End Property

        ''' <summary>
        ''' Address Line10 of Address
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address10() As String
            Get
                Return Me.txtLookup_Longitude.Text()
            End Get
            Set(ByVal value As String)
                Me.txtLookup_Longitude.Text = value
            End Set
        End Property



        Public WriteOnly Property ValidationGroup() As String
            Set(ByVal value As String)
                RqdLookup_Street.ValidationGroup = value
                RqdLookup_Town.ValidationGroup = value
                'RqdLookup_State.ValidationGroup = value
                RqdLookUp_Postcode.ValidationGroup = value
            End Set
        End Property

        Public WriteOnly Property TabIndex() As Integer
            Set(ByVal value As Integer)
                TxtLookup_Street.TabIndex = value
                TxtLookup_Locality.TabIndex = value
                TxtLookup_Town.TabIndex = value
                TxtLookup_State.TabIndex = value
                TxtLookup_Postcode.TabIndex = value
                GISLookup_Country.TabIndex = value
                txtLookup_PropDescription.TabIndex = value
                txtLookup_GNAFID.TabIndex = value
                txtLookup_DPID.TabIndex = value
                txtLookup_DPID_Barcode.TabIndex = value
                txtLookup_Latitude.TabIndex = value
                txtLookup_Longitude.TabIndex = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(GISLookup_Type)

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim sDefaultCountry As String = oNexusConfig.Portals.Portal(Portal.GetPortalID().ToString()).Countries.DefaultCountryKey

             vldPpostcodeRegex.Enabled = True
             vldPpostcodeRegex.ValidationExpression = oNexusConfig.Portals.Portal(Portal.GetPortalID()).PostcodeValidationRegex

            'Need to store this value in hidden in order to read from javascript
            txtPostBackTo.Value = Request.QueryString("PostbackTo")

            If Not IsPostBack Then
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                            oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                    End Select
                End If

                If Request("AddressID") <> "" Then
                    Dim oAddress As NexusProvider.Address
                    'load the address and change visibility of buttons
                    AddressKey.Value = Request("AddressID")
                    txtMode.Value = "Update"
                    btnAddAddress.Visible = False
                    btnUpdateAddress.Visible = True

                    If Request.QueryString("Page") IsNot Nothing AndAlso Request.QueryString("Page").ToString.Trim = "Agent" Then
                        Dim oInsurerDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        If oInsurerDetails.Insurer IsNot Nothing Then
                            oAddress = oInsurerDetails.Insurer.Address
                        End If
                    ElseIf Request.QueryString("Page") IsNot Nothing AndAlso Request.QueryString("Page").ToString.Trim = "Client" Then
                        Dim oClientDetails As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        If oClientDetails.Client IsNot Nothing Then
                            oAddress = oClientDetails.Client.Address
                        End If
                    Else
                        oAddress = oParty.Addresses.Item(CType(Request("AddressID"), Integer))
                    End If

                    Select Case oAddress.AddressType
                        Case NexusProvider.AddressType.BillingAddress
                            GISLookup_Type.Value = "3131 XBI"
                        Case NexusProvider.AddressType.BranchAddress
                            GISLookup_Type.Value = "3131 XBA"
                        Case NexusProvider.AddressType.BusinessAddress
                            GISLookup_Type.Value = "3131 002"
                        Case NexusProvider.AddressType.CorrespondenceAddress
                            GISLookup_Type.Value = "3131 XCO"
                        Case NexusProvider.AddressType.EmailAddress
                            GISLookup_Type.Value = "3131 ECK"
                        Case NexusProvider.AddressType.HomeAddress
                            GISLookup_Type.Value = "3131 001"
                        Case NexusProvider.AddressType.OtherAddress
                            GISLookup_Type.Value = "3131 0X9"
                        Case NexusProvider.AddressType.PreviousAddress
                            GISLookup_Type.Value = "3131 XPR"
                        Case NexusProvider.AddressType.RegisteredAddress
                            GISLookup_Type.Value = "3131 XRE"
                        Case NexusProvider.AddressType.SiteAddress
                            GISLookup_Type.Value = "3131 XSA"
                        Case NexusProvider.AddressType.SubAgent
                            GISLookup_Type.Value = "3131 0XR"
                    End Select

                    Me.TxtLookup_Street.Text = oAddress.Address1
                    Me.TxtLookup_Locality.Text = oAddress.Address2
                    Me.TxtLookup_Town.Text = oAddress.Address3
                    Me.TxtLookup_Postcode.Text = oAddress.PostCode
                    'Call SAM to get the Value by passing the Code
                    If oAddress.CountryCode IsNot Nothing Then
                        GISLookup_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, oAddress.CountryCode, "COUNTRY", False) 'oAddress.CountryCode
                    ElseIf oAddress.CountryKey > 0 Then
                        GISLookup_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, oAddress.CountryKey, "COUNTRY", False) 'oAddress.CountryCode
                    End If

                    If hdnstate.Value <> "1" Then
                        'GISLookup_State.Value = GetKeyForDescription(NexusProvider.ListType.PMLookup, oAddress.Address4, "STATE", False)
                        TxtLookup_State.Text = oAddress.Address4
                    Else
                        'GISLookup_State.Value = oAddress.StateCode
                        For iCount = 0 To GISLookup_State.Items.Count - 1
                            If Convert.ToString(GISLookup_State.Items(iCount).Description).Trim = oAddress.Address4 Then
                                GISLookup_State.Value = Convert.ToString(GISLookup_State.Items(iCount).Key)
                                Exit For
                            End If
                        Next


                    End If

                    Me.txtLookup_PropDescription.Text = oAddress.Address5
                    Me.txtLookup_GNAFID.Text = oAddress.Address6
                    Me.txtLookup_DPID.Text = oAddress.Address7
                    Me.txtLookup_DPID_Barcode.Text = oAddress.Address8
                    Me.txtLookup_Latitude.Text = oAddress.Address9
                    Me.txtLookup_Longitude.Text = oAddress.Address10
                Else
                    txtMode.Value = "Add"
                    GISLookup_Country.Value = sDefaultCountry
                    GISLookup_Type.Value = "3131 XCO"
                End If

                If oParty IsNot Nothing Then
                    If oParty.Addresses IsNot Nothing AndAlso oParty.Addresses.Count > 0 Then
                        For iCount As Integer = 0 To oParty.Addresses.Count - 1
                            Select Case oParty.Addresses(iCount).AddressType
                                Case NexusProvider.AddressType.BillingAddress
                                    AddressType.Value &= "3131 XBI" & ";"
                                Case NexusProvider.AddressType.BranchAddress
                                    AddressType.Value &= "3131 XBA" & ";"
                                Case NexusProvider.AddressType.BusinessAddress
                                    AddressType.Value &= "3131 002" & ";"
                                Case NexusProvider.AddressType.CorrespondenceAddress
                                    AddressType.Value &= "3131 XCO" & ";"
                                Case NexusProvider.AddressType.EmailAddress
                                    AddressType.Value &= "3131 ECK" & ";"
                                Case NexusProvider.AddressType.HomeAddress
                                    AddressType.Value &= "3131 001" & ";"
                                Case NexusProvider.AddressType.OtherAddress
                                    AddressType.Value &= "3131 0X9" & ";"
                                Case NexusProvider.AddressType.PreviousAddress
                                    AddressType.Value &= "3131 XPR" & ";"
                                Case NexusProvider.AddressType.RegisteredAddress
                                    AddressType.Value &= "3131 XRE" & ";"
                                Case NexusProvider.AddressType.SiteAddress
                                    AddressType.Value &= "3131 XSA" & ";"
                                Case NexusProvider.AddressType.SubAgent
                                    AddressType.Value &= "3131 0XR" & ";"
                            End Select
                        Next
                    End If
                End If
                'check if postcode lookup is enabled, and if it is make btnFindAddress visible
                If CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressControl.EnablePostCodeLookup Then
                    btnFindAddress.Visible = True
                    selAddressList.Visible = True
                    hdnGuid.Value = setSecureGuid()
                End If
                CheckStateLookup()
            End If
            If (Not IsPostBack) Then
                Dim v_sOptionList As System.Xml.XmlElement = Nothing
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oCountryList As NexusProvider.LookupListCollection

                oCountryList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Country", True, False, , , , v_sOptionList)
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(sXML)
                Dim oNodeList As XmlNodeList
                bis_state_lookup = False
                If GISLookup_Country.Value.Trim().ToString() <> "" Then
                    oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/Country[is_deleted=0 and country_id=" + GISLookup_Country.Value + "]")
                    If oNodeList IsNot Nothing And oNodeList.Count > 0 Then
                        Dim oNode As XmlNode
                        oNode = oNodeList.Item(0)
                        If (oNode IsNot Nothing And oNode.ChildNodes IsNot Nothing) Then
                            For iCount = 0 To oNode.ChildNodes.Count - 1
                                If oNode.ChildNodes(iCount) IsNot Nothing And Convert.ToString(oNode.ChildNodes(iCount).Name) = "is_state_lookup" Then
                                    bis_state_lookup = oNode.ChildNodes(iCount).InnerText
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                    If (bis_state_lookup = True) Then
                        hdnstate.Value = "1"
                        TxtLookup_State.Visible = False
                        GISLookup_State.Visible = True
                    Else
                        TxtLookup_State.Visible = True
                        GISLookup_State.Visible = False
                    End If
                End If
            End If
            If Not String.IsNullOrEmpty(sAddressMandatoryFields) And Session(CNMode) <> Mode.View Then

                'Can have 4 for State
                If sAddressMandatoryFields.Contains("4") And bis_state_lookup = True Then

                    GISLookup_State.CssClass = GISLookup_State.CssClass & " field-mandatory"
                    RqdLookup_State.Enabled = False
                    RqdFindLookup_State.Enabled = True
                ElseIf sAddressMandatoryFields.Contains("4") Then
                    TxtLookup_State.CssClass = TxtLookup_State.CssClass & " field-mandatory"
                    RqdLookup_State.Enabled = True
                    RqdFindLookup_State.Enabled = False
                Else
                    RqdLookup_State.Enabled = False
                    RqdFindLookup_State.Enabled = False
                End If

            End If


            If hdnEnableSpatialFeilds.Value Is Nothing OrElse Trim(hdnEnableSpatialFeilds.Value) = "" Then
                Dim oWebService As NexusProvider.ProviderBase = Nothing
                Dim oEnableSpatialAddressfeilds As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oEnableSpatialAddressfeilds = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.EnableSpatialAddressFields)
                If oEnableSpatialAddressfeilds IsNot Nothing Then
                    hdnEnableSpatialFeilds.Value = oEnableSpatialAddressfeilds.OptionValue

                End If
            End If
            If hdnEnableSpatialFeilds.Value = "1" Then
                PnlSpatialDetails.Visible = True
            Else
                PnlSpatialDetails.Visible = False
            End If

        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        ''' <summary>
        ''' Supporting method to do postcode lookup and return json formatted address object collection
        ''' </summary>
        ''' <param name="PostCode">postcode to match</param>
        ''' <returns>String of json serialised address objects</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Shared Function findAddress(ByVal PostCode As String) As String
            Dim oAddress As New NexusProvider.Address
            Dim oAddressCollection As New NexusProvider.AddressCollection

            Dim pcLookup As New PostCodeLookUpService()

            Dim pcResults As PremiseListResult = pcLookup.ReturnAddressList(AppSettings("PostcodeRef"), PostCode, AppSettings("PostcodeUser"), AppSettings("PostcodePass"))
            If pcResults IsNot Nothing Then
                Dim premiseAddress As PremiseListAddress
                For Each premiseAddress In pcResults.addresses
                    For Each premise As PremiseList In premiseAddress.premise
                        oAddress = New NexusProvider.Address
                        'check if the premise returned is numeric, ignore the last character to take into account flats
                        'e.g. "12A Some Street"
                        If IsNumeric(Left(premise.premise, Len(premise.premise) - 1)) Or (Len(premise.premise) = 1 And IsNumeric(premise.premise)) Then
                            'numeric address, premise and street name make up address1
                            oAddress.Address1 = premise.premise & " " & premiseAddress.street
                            oAddress.Address2 = premiseAddress.post_town
                        Else
                            'non numeric, i.e. house name
                            oAddress.Address1 = premise.premise
                            oAddress.Address2 = premiseAddress.street
                            oAddress.Address3 = premiseAddress.post_town
                        End If

                        oAddress.Address4 = premiseAddress.county
                        oAddress.PostCode = premiseAddress.postcode
                        oAddressCollection.Add(oAddress)
                    Next
                Next
            End If

            'json serialise the results and return as a string
            'we have to add Address as a known type in order to serialise it
            Dim knownTypeList As New List(Of Type)
            knownTypeList.Add(oAddress.GetType)

            Dim serializer As System.Runtime.Serialization.Json.DataContractJsonSerializer = _
                New System.Runtime.Serialization.Json.DataContractJsonSerializer(oAddressCollection.GetType, knownTypeList)
            Dim ms As MemoryStream = New MemoryStream
            serializer.WriteObject(ms, oAddressCollection)
            Return Encoding.Default.GetString(ms.ToArray)
        End Function

        Protected Sub GISLookup_Country_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles GISLookup_Country.SelectedIndexChange
            CheckStateLookup()
        End Sub

        Protected Sub CheckStateLookup()
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCountryList As NexusProvider.LookupListCollection
            Dim olist As NexusProvider.LookupListCollection

            oCountryList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Country", True, False, , , , v_sOptionList)
            Dim sXML As String = v_sOptionList.OuterXml
            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.LoadXml(sXML)
            Dim oNodeList As XmlNodeList
            bis_state_lookup = False
            oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/Country[is_deleted=0 and country_id=" + GISLookup_Country.Value + "]")
            If oNodeList IsNot Nothing And oNodeList.Count > 0 Then
                Dim oNode As XmlNode
                oNode = oNodeList.Item(0)
                If (oNode IsNot Nothing And oNode.ChildNodes IsNot Nothing) Then
                    For iCount = 0 To oNode.ChildNodes.Count - 1
                        If oNode.ChildNodes(iCount) IsNot Nothing And Convert.ToString(oNode.ChildNodes(iCount).Name) = "is_state_lookup" Then
                            bis_state_lookup = oNode.ChildNodes(iCount).InnerText
                            Exit For
                        End If
                    Next
                    If oNode.ChildNodes(0) IsNot Nothing Then
                        icountrykey = Convert.ToInt32(oNode.ChildNodes(0).InnerText)
                    End If
                End If
                If (bis_state_lookup = True) Then
                    hdnstate.Value = "1"
                    olist = oWebservice.GetList(NexusProvider.ListType.PMLookup, "State", True, False, "country_id", GISLookup_Country.Value)
                    If olist IsNot Nothing AndAlso olist.Count > 0 Then
                        GISLookup_State.Visible = True
                        TxtLookup_State.Visible = False
                    Else
                        GISLookup_State.Visible = False
                        TxtLookup_State.Visible = True
                    End If
                Else
                    hdnstate.Value = "0"
                    TxtLookup_State.Visible = True
                    GISLookup_State.Visible = False

                End If
                ' Change the lable of AddressLine1, AddressLine2, AddressLine3, AddressLine4
                Dim sAddressLine1 As String = String.Empty
                Dim sAddressLine2 As String = String.Empty
                Dim sAddressLine3 As String = String.Empty
                Dim sAddressLine4 As String = String.Empty
                If (oNode IsNot Nothing And oNode.ChildNodes IsNot Nothing) Then
                    For iCount = 0 To oNode.ChildNodes.Count - 1
                        If oNode.ChildNodes(iCount) IsNot Nothing And Convert.ToString(oNode.ChildNodes(iCount).Name) = "address_line_1_caption" Then
                            sAddressLine1 = oNode.ChildNodes(iCount).InnerText
                        End If
                        If oNode.ChildNodes(iCount) IsNot Nothing And Convert.ToString(oNode.ChildNodes(iCount).Name) = "address_line_2_caption" Then
                            sAddressLine2 = oNode.ChildNodes(iCount).InnerText
                        End If
                        If oNode.ChildNodes(iCount) IsNot Nothing And Convert.ToString(oNode.ChildNodes(iCount).Name) = "address_line_3_caption" Then
                            sAddressLine3 = oNode.ChildNodes(iCount).InnerText
                        End If
                        If oNode.ChildNodes(iCount) IsNot Nothing And Convert.ToString(oNode.ChildNodes(iCount).Name) = "address_line_4_caption" Then
                            sAddressLine4 = oNode.ChildNodes(iCount).InnerText
                        End If
                    Next
                    If Not String.IsNullOrEmpty(sAddressLine1) Then
                        lblStreet.Text = sAddressLine1
                    Else
                        lblStreet.Text = GetLocalResourceObject("lbl_Address_Street")
                    End If
                    If Not String.IsNullOrEmpty(sAddressLine2) Then
                        lblLocality.Text = sAddressLine2
                    Else
                        lblLocality.Text = GetLocalResourceObject("lbl_Address_Locality")
                    End If
                    If Not String.IsNullOrEmpty(sAddressLine3) Then
                        lblTown.Text = sAddressLine3
                    Else
                        lblTown.Text = GetLocalResourceObject("lbl_Address_Town")
                    End If
                    If Not String.IsNullOrEmpty(sAddressLine4) Then
                        lblState.Text = sAddressLine4
                    Else
                        lblState.Text = GetLocalResourceObject("lbl_Address_State")
                    End If
                End If
            End If

            If Not String.IsNullOrEmpty(sAddressMandatoryFields) And Session(CNMode) <> Mode.View Then

                'Can have 4 for State
                If sAddressMandatoryFields.Contains("4") And bis_state_lookup = True Then

                    GISLookup_State.CssClass = GISLookup_State.CssClass & " field-mandatory"
                    RqdLookup_State.Enabled = False
                    RqdFindLookup_State.Enabled = True
                ElseIf sAddressMandatoryFields.Contains("4") Then
                    TxtLookup_State.CssClass = TxtLookup_State.CssClass & " field-mandatory"
                    RqdLookup_State.Enabled = True
                    RqdFindLookup_State.Enabled = False
                Else
                    RqdLookup_State.Enabled = False
                    RqdFindLookup_State.Enabled = False
                End If

            End If
        End Sub
    End Class

End Namespace
