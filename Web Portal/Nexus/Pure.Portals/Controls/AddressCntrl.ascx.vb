Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports System.Xml

Namespace Nexus

    Partial Class AddressCntrl : Inherits System.Web.UI.UserControl

        Dim oAddress As New NexusProvider.Address()

        Private bEnablePostCodeLookup As Boolean = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressControl.EnablePostCodeLookup
        Private bShowCountry As Boolean = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressControl.ShowCountry
        Private sDefaultCountry As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).Countries.DefaultCountryCode
        Private sDefaultCountryKey As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).Countries.DefaultCountryKey
        Private sAddressMandatoryFields As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).AddressMandatoryFields
        Dim bIsShowStateAsLookup As Boolean = False
        Private sEnableExtraAddressFields As String = CType(AppSettings("EnableExtraAddressFields"), String)
        Dim RestrictedPageURLs As String
        Dim bIsRestrictedPageUrl As Boolean = False

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim RequestedPageURL As String = Request.Url.Segments(Request.Url.Segments.Length - 1).ToString.ToUpper

            RestrictedPageURLs = "payclaim.aspx,CashListItem.aspx,PayNow.aspx,PerilDetails.aspx"
            If DirectCast(Me.Parent, System.Web.UI.Control).ClientID.ToUpper.Contains("PAYCLAIM") Then
                vldPpostcodeRegex.Enabled = False
                vldAddresspostcodeRegex.Enabled = False
                bEnablePostCodeLookup = False
                bIsRestrictedPageUrl = True
            End If
            'Disable post code validation on above mentioned pages
            If RestrictedPageURLs.ToUpper.Contains(RequestedPageURL) = True Then
                vldPpostcodeRegex.Enabled = False
                vldAddresspostcodeRegex.Enabled = False
                bEnablePostCodeLookup = False
                bIsRestrictedPageUrl = True
            End If

            If bIsRestrictedPageUrl Then
                RqdAddress_Line1.Enabled = False
                RqdAddress_Line2.Enabled = False
                RqdAddress_Line3.Enabled = False
                RqdAddress_Line4.Enabled = False
                RqdAddress_PostCode.Enabled = False
                RqdAddress_Country.Enabled = False
            End If

            bIsShowStateAsLookup = CheckStateLookup()

            If bIsShowStateAsLookup = True Then
                If bEnablePostCodeLookup = True Then
                    liStateText.Visible = False
                    liStateLookup.Visible = True
                Else
                    litxtStateLookup.Visible = True
                    litxtStateText.Visible = False
                End If
            End If

            If bEnablePostCodeLookup = True Then
                PnlPostcodeLookup.Visible = True
                PnlAddress.Visible = False
                PnlSpatialPostCodeLookup.Visible = True
                PnlSpatial.Visible = False
                Dim SetAddressScript As String = "function " & Me.ID & "_SetAddress(addressString){var addr; " & _
                " addr = addressString.split(', '); " & _
                " document.getElementById('" & TxtLookup_Street.ClientID & "').value=addr[0]; " & _
                " if (addr.length == 4) {  " & _
                " document.getElementById('" & TxtLookup_Locality.ClientID & "').value=addr[1]; " & _
                " document.getElementById('" & TxtLookup_Town.ClientID & "').value=addr[2]; " & _
                " document.getElementById('" & TxtLookup_County.ClientID & "').value=addr[3];" & _
                " } else if (addr.length == 3) {  " & _
                " document.getElementById('" & TxtLookup_Locality.ClientID & "').value=''; " & _
                " document.getElementById('" & TxtLookup_Town.ClientID & "').value=addr[1]; " & _
                " document.getElementById('" & TxtLookup_County.ClientID & "').value=addr[2];}" & _
                " }   "
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "SetAddressScript_" & Me.ID, SetAddressScript, True)

                liCountry.Visible = bShowCountry

                'Mandatory Field specified

                If Not String.IsNullOrEmpty(sAddressMandatoryFields) AndAlso Session(CNMode) <> Mode.View AndAlso Session(CNMode) <> Mode.DeclinePayment AndAlso Session(CNMode) <> Mode.ViewClaimPayment _
                    AndAlso Not bIsRestrictedPageUrl AndAlso Session(CNMode) <> Mode.Recommend AndAlso Session(CNMode) <> Mode.Authorise AndAlso Session(CNMode) <> Mode.ViewClaim Then
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
                    If sAddressMandatoryFields.Contains("4") Then
                        'lblCountyRequired.Visible = True
                        If bIsShowStateAsLookup Then
                            GISLookup_State.CssClass = GISLookup_State.CssClass & " field-mandatory"
                            RqdLookup_State.Enabled = True
                        Else
                            TxtLookup_County.CssClass = TxtLookup_County.CssClass & " field-mandatory"
                            RqdLookup_County.Enabled = True
                        End If
                    End If
                    If sAddressMandatoryFields.Contains("PC") Then
                        'lblPostCodeRequired.Visible = True
                        TxtAddress_Postcode.CssClass = TxtAddress_Postcode.CssClass & " field-mandatory"
                        RqdLookUp_Postcode.Enabled = True
                    End If
                    If sAddressMandatoryFields.Contains("CO") Then
                        'lblCountryRequired.Visible = True
                        GISLookup_Country.CssClass = GISLookup_Country.CssClass & " field-mandatory"
                        RqdLookup_Country.Enabled = True
                    End If
                End If
                'Additional Address Fields Enabled
                If Not String.IsNullOrEmpty(sEnableExtraAddressFields) And Session(CNMode) <> Mode.View _
                AndAlso Not bIsRestrictedPageUrl Then
                    If sEnableExtraAddressFields.Contains("Address5") Then
                        txtLookup_PropDescription.Enabled = True
                    End If
                    If sEnableExtraAddressFields.Contains("Address6") Then
                        txtLookup_GNAFID.Enabled = True
                    End If
                    If sEnableExtraAddressFields.Contains("Address7") Then
                        txtLookup_DPID.Enabled = True
                    End If
                    If sEnableExtraAddressFields.Contains("Address8") Then
                        txtLookup_DPID_Barcode.Enabled = True
                    End If
                    If sEnableExtraAddressFields.Contains("Address9") Then
                        txtLookup_Latitude.Enabled = True
                    End If
                    If sEnableExtraAddressFields.Contains("Address10") Then
                        txtLookup_Longitude.Enabled = True
                    End If
                End If
            Else

                PnlPostcodeLookup.Visible = False
                PnlAddress.Visible = True

                pnlGISAddressCountry.Visible = bShowCountry
                ''Mandatory Field specified
                If Not String.IsNullOrEmpty(sAddressMandatoryFields) And Session(CNMode) <> Mode.View AndAlso Session(CNMode) <> Mode.DeclinePayment AndAlso Session(CNMode) <> Mode.ViewClaimPayment _
                AndAlso Not bIsRestrictedPageUrl AndAlso Session(CNMode) <> Mode.Recommend AndAlso Session(CNMode) <> Mode.Authorise AndAlso Session(CNMode) <> Mode.ViewClaim Then
                    If sAddressMandatoryFields.Contains("1") Then
                        'lblAddress1Required.Visible = True
                        TxtAddress_Line1.CssClass = TxtAddress_Line1.CssClass & " field-mandatory"
                        RqdAddress_Line1.Enabled = True
                    End If
                    If sAddressMandatoryFields.Contains("2") Then
                        'lblAddress2Required.Visible = True
                        TxtAddress_Line2.CssClass = TxtAddress_Line2.CssClass & " field-mandatory"
                        RqdAddress_Line2.Enabled = True
                    End If
                    If sAddressMandatoryFields.Contains("3") Then
                        'lblAddress3Required.Visible = True
                        TxtAddress_Line3.CssClass = TxtAddress_Line3.CssClass & " field-mandatory"
                        RqdAddress_Line3.Enabled = True
                    End If
                    If sAddressMandatoryFields.Contains("4") Then
                        'lblAddress4Required.Visible = True
                        If bIsShowStateAsLookup Then
                            GISAddressLookup_State.CssClass = GISAddressLookup_State.CssClass & " field-mandatory"
                            RqdAddress_State.Enabled = True
                        Else
                            TxtAddress_Line4.CssClass = TxtAddress_Line4.CssClass & " field-mandatory"
                            RqdAddress_Line4.Enabled = True
                        End If
                    End If
                    If sAddressMandatoryFields.Contains("PC") Then
                        RqdAddress_PostCode.Enabled = True
                        'lblAddressPostCodeRequired.Visible = True
                        TxtAddress_Postcode.CssClass = TxtAddress_Postcode.CssClass & " field-mandatory"
                    End If
                    If sAddressMandatoryFields.Contains("CO") Then
                        'lblAddressCountryRequired.Visible = True
                        GISAddress_Country.CssClass = GISAddress_Country.CssClass & " field-mandatory"
                        RqdAddress_Country.Enabled = True
                    End If
                    'Additional Address Fields Enabled
                End If
            End If
            If (Session(CNMode) = Mode.ViewClaim) Then
                TxtAddress_Line1.Enabled = False
                RqdAddress_Line1.Enabled = False
                TxtAddress_Line2.Enabled = False
                RqdAddress_Line2.Enabled = False
                TxtAddress_Line3.Enabled = False
                RqdAddress_Line3.Enabled = False
                TxtAddress_Line4.Enabled = False
                RqdAddress_Line4.Enabled = False
                RqdAddress_PostCode.Enabled = False
                TxtAddress_Postcode.Enabled = False
                GISAddress_Country.Enabled = False
                RqdAddress_Country.Enabled = False
            End If
            vldAddresspostcodeRegex.ValidationExpression = oNexusConfig.Portals.Portal(Portal.GetPortalID()).PostcodeValidationRegex
            vldPpostcodeRegex.ValidationExpression = oNexusConfig.Portals.Portal(Portal.GetPortalID()).PostcodeValidationRegex
        End Sub
        Public Property Address() As NexusProvider.Address
            Get
                Dim oAddress As New NexusProvider.Address(Address1, Postcode, CountryCode)

                With oAddress
                    .Key = .Key
                    .Address2 = Address2
                    .Address3 = Address3
                    .Address4 = Address4
                    .PostCode = Postcode
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
                    Key = String.Empty
                    Address1 = String.Empty
                    Address2 = String.Empty
                    Address3 = String.Empty
                    Address4 = String.Empty
                    Postcode = String.Empty
                    CountryCode = String.Empty

                    Address5 = String.Empty
                    Address6 = String.Empty
                    Address7 = String.Empty
                    Address8 = String.Empty
                    Address9 = String.Empty
                    Address10 = String.Empty

                Else

                    With value
                        Key = .Key
                        Address1 = .Address1
                        Address2 = .Address2
                        Address3 = .Address3
                        Address4 = .Address4
                        Postcode = .PostCode
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


        Public Property AddressKey() As String

            '21-11-07 - DH - Rewrote to use the Nexus Provider

            Get

                If bEnablePostCodeLookup = True Then
                    oAddress.Key = Key
                    oAddress.Address1 = TxtLookup_Street.Text
                    oAddress.Address2 = TxtLookup_Locality.Text
                    oAddress.Address3 = TxtLookup_Town.Text
                    If bIsShowStateAsLookup = True Then
                        oAddress.Address4 = GISLookup_State.Value
                    Else
                        oAddress.Address4 = TxtLookup_County.Text
                    End If
                    oAddress.PostCode = TxtLookup_Postcode.Text
                    If bShowCountry Then
                        If GISAddress_Country.DataItemValue = NexusProvider.DataItemTypes.Key Then
                            oAddress.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, GISLookup_Country.Value, "Country", True)
                        Else
                            oAddress.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, GISLookup_Country.Value, "Country", False)
                        End If
                    Else
                        oAddress.CountryCode = AppSettings("DefaultCountryCode")
                    End If

                    oAddress.Address5 = txtLookup_PropDescription.Text.Trim
                    oAddress.Address6 = txtLookup_GNAFID.Text.Trim
                    oAddress.Address7 = txtLookup_DPID.Text.Trim
                    oAddress.Address8 = txtLookup_DPID_Barcode.Text.Trim
                    oAddress.Address9 = txtLookup_Latitude.Text.Trim
                    oAddress.Address10 = txtLookup_Longitude.Text.Trim


                Else
                    oAddress.Key = Key
                    oAddress.Address1 = TxtAddress_Line1.Text
                    oAddress.Address2 = TxtAddress_Line2.Text
                    oAddress.Address3 = TxtAddress_Line3.Text
                    If bIsShowStateAsLookup = True Then
                        oAddress.Address4 = GISAddressLookup_State.Value
                    Else
                        oAddress.Address4 = TxtAddress_Line4.Text
                    End If
                    oAddress.PostCode = TxtAddress_Postcode.Text
                    If bShowCountry Then
                        oAddress.CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, GISAddress_Country.Value, "Country", True)
                    Else
                        oAddress.CountryCode = AppSettings("DefaultCountryCode")
                    End If
                    oAddress.Address5 = txtAddress_Line5.Text.Trim
                    oAddress.Address6 = txtAddress_Line6.Text.Trim
                    oAddress.Address7 = txtAddress_Line7.Text.Trim
                    oAddress.Address8 = txtAddress_Line8.Text.Trim
                    oAddress.Address9 = txtAddress_Line9.Text.Trim
                    oAddress.Address10 = txtAddress_Line10.Text.Trim


                End If

                ''Don't add the address if we've already got a key
                'If String.IsNullOrEmpty(Key) Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    'Create the address
                    oWebService.AddAddress(oAddress)
                    Key = oAddress.Key
                    hdnKey.Value = oAddress.Key
                Finally
                    oWebService = Nothing
                End Try

                Return oAddress.Key

            End Get

            Set(ByVal Value As String)

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                Try
                    'Retrieve an address, as specified by the Key
                    Address = oWebService.GetAddress(Value)
                    oAddress.Key = Value
                    hdnKey.Value = Value
                    Key = Value
                Finally
                    oWebService = Nothing
                End Try

            End Set

        End Property
        ''' <summary>
        ''' Handled to handle update of existing address.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key() As String
            Get
                Return hdnKey.Value
            End Get
            Set(ByVal value As String)
                hdnKey.Value = value
            End Set
        End Property
        Public Property Address1() As String
            Get
                If bEnablePostCodeLookup = True Then
                    Return TxtLookup_Street.Text
                Else
                    Return TxtAddress_Line1.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    If String.IsNullOrEmpty(value) Then
                        TxtLookup_Street.Text = String.Empty
                    Else
                        TxtLookup_Street.Text = value
                    End If

                Else
                    TxtAddress_Line1.Text = value
                End If
            End Set
        End Property

        Public Property Address2() As String
            Get
                If bEnablePostCodeLookup = True Then
                    Return TxtLookup_Locality.Text
                Else
                    Return TxtAddress_Line2.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    TxtLookup_Locality.Text = value
                Else
                    TxtAddress_Line2.Text = value
                End If
            End Set
        End Property

        Public Property Address3() As String
            Get
                If bEnablePostCodeLookup = True Then
                    Return TxtLookup_Town.Text
                Else
                    Return TxtAddress_Line3.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    TxtLookup_Town.Text = value
                Else
                    TxtAddress_Line3.Text = value
                End If
            End Set
        End Property

        Public Property Address4() As String
            Get
                If bEnablePostCodeLookup = True Then
                    If bIsShowStateAsLookup = True Then
                        Return GISLookup_State.Value
                    Else
                        Return TxtLookup_County.Text
                    End If
                Else
                    If bIsShowStateAsLookup = True Then
                        Return GISAddressLookup_State.Value
                    Else
                        Return TxtAddress_Line4.Text
                    End If
                End If
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    If bEnablePostCodeLookup = True Then
                        If bIsShowStateAsLookup = True Then
                            GISLookup_State.Value = value
                        Else
                            TxtLookup_County.Text = value
                        End If

                    Else
                        If bIsShowStateAsLookup = True Then
                            For iCount = 0 To GISAddressLookup_State.Items.Count - 1
                                If Convert.ToString(GISAddressLookup_State.Items(iCount).Description).Trim.ToUpper = value.Trim.ToUpper Then
                                    GISAddressLookup_State.Value = Convert.ToString(GISAddressLookup_State.Items(iCount).Code)
                                    Exit For
                                End If
                            Next
                        Else
                            TxtAddress_Line4.Text = value
                        End If
                    End If
                End If
            End Set
        End Property

        Public Property Postcode() As String
            Get
                If bEnablePostCodeLookup = True Then
                    If TxtLookup_Postcode.Text.IndexOf(" ") >= 0 Then
                        Return UCase(TxtLookup_Postcode.Text)
                    Else
                        If TxtLookup_Postcode.Text.Length > 4 Then
                            Dim iLen As Int16 = TxtLookup_Postcode.Text.Length
                            Return UCase(Left(TxtLookup_Postcode.Text, iLen - 3) & " " & Mid(TxtLookup_Postcode.Text, iLen - 2))
                        Else
                            Return UCase(TxtLookup_Postcode.Text)
                        End If
                    End If
                Else
                    Return TxtAddress_Postcode.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    TxtLookup_Postcode.Text = value
                Else
                    TxtAddress_Postcode.Text = value
                End If
            End Set
        End Property

        Public ReadOnly Property Country() As String
            Get
                If bEnablePostCodeLookup = True Then
                    If bShowCountry Then
                        Return GISLookup_Country.Text
                    Else
                        Return String.Empty
                    End If
                Else
                    If bShowCountry Then
                        Return GISAddress_Country.Text
                    Else
                        Return String.Empty
                    End If
                End If
            End Get
        End Property

        Public Property CountryCode() As String
            Get
                If bEnablePostCodeLookup = True Then
                    If bShowCountry Then
                        Return GetCodeForKey(NexusProvider.ListType.PMLookup, GISLookup_Country.Value, "Country", True)
                    Else
                        Return sDefaultCountry 'AppSettings("DefaultCountryCode")
                    End If
                Else
                    If bShowCountry Then
                        If GISAddress_Country.DataItemValue = NexusProvider.DataItemTypes.Key Then
                            Return GetCodeForKey(NexusProvider.ListType.PMLookup, GISAddress_Country.Value, "Country", True)
                        Else
                            Return GetCodeForKey(NexusProvider.ListType.PMLookup, GISAddress_Country.Value, "Country", False)
                        End If
                    Else
                        Return sDefaultCountry 'AppSettings("DefaultCountryCode")
                    End If
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    GISLookup_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, value, "Country", False)
                Else
                    If GISAddress_Country.DataItemValue = NexusProvider.DataItemTypes.Key Then
                        GISAddress_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, value, "Country", False)
                    Else
                        GISAddress_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, value, "Country", True)
                    End If

                End If
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
                If bEnablePostCodeLookup = True Then
                    Return txtLookup_PropDescription.Text
                Else
                    Return txtAddress_Line5.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    If String.IsNullOrEmpty(value) Then
                        txtLookup_PropDescription.Text = String.Empty
                    Else
                        txtLookup_PropDescription.Text = value
                    End If

                Else
                    txtAddress_Line5.Text = value
                End If
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
                If bEnablePostCodeLookup = True Then
                    Return txtLookup_GNAFID.Text
                Else
                    Return txtAddress_Line6.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    txtLookup_GNAFID.Text = value
                Else
                    txtAddress_Line6.Text = value
                End If
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
                If bEnablePostCodeLookup = True Then
                    Return txtLookup_DPID.Text
                Else
                    Return txtAddress_Line7.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    txtLookup_DPID.Text = value
                Else
                    txtAddress_Line7.Text = value
                End If
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
                If bEnablePostCodeLookup = True Then
                    Return txtLookup_DPID_Barcode.Text
                Else
                    Return txtAddress_Line8.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    txtLookup_DPID_Barcode.Text = value
                Else
                    txtAddress_Line8.Text = value
                End If
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
                If bEnablePostCodeLookup = True Then
                    Return txtLookup_Latitude.Text
                Else
                    Return txtAddress_Line9.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    txtLookup_Latitude.Text = value
                Else
                    txtAddress_Line9.Text = value
                End If
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
                If bEnablePostCodeLookup = True Then
                    Return txtLookup_Longitude.Text
                Else
                    Return txtAddress_Line10.Text
                End If
            End Get
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    txtLookup_Longitude.Text = value
                Else
                    txtAddress_Line10.Text = value
                End If
            End Set
        End Property



        Public WriteOnly Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                If bEnablePostCodeLookup = True Then
                    TxtLookup_Street.Enabled = value
                    TxtLookup_Locality.Enabled = value
                    TxtLookup_Town.Enabled = value
                    TxtLookup_County.Enabled = value
                    TxtLookup_Postcode.Enabled = value
                    GISAddress_Country.Enabled = value
                    btnFindAddress.Visible = value
                    GISLookup_Country.Enabled = value
                    'Additional Address Fields Enabled
                    If Not String.IsNullOrEmpty(sEnableExtraAddressFields) Then
                        If sEnableExtraAddressFields.Contains("Address5") Then
                            txtLookup_PropDescription.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address6") Then
                            txtLookup_GNAFID.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address7") Then
                            txtLookup_DPID.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address8") Then
                            txtLookup_DPID_Barcode.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address9") Then
                            txtLookup_Latitude.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address10") Then
                            txtLookup_Longitude.Enabled = value
                        End If
                    End If
                Else
                    TxtAddress_Line1.Enabled = value
                    TxtAddress_Line2.Enabled = value
                    TxtAddress_Line3.Enabled = value
                    TxtAddress_Line4.Enabled = value
                    TxtAddress_Postcode.Enabled = value
                    GISAddress_Country.Enabled = value
                    'Additional Address Fields Enabled
                    If Not String.IsNullOrEmpty(sEnableExtraAddressFields)  Then
                        If sEnableExtraAddressFields.Contains("Address5") Then
                            txtAddress_Line5.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address6") Then
                            txtAddress_Line6.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address7") Then
                            txtAddress_Line7.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address8") Then
                            txtAddress_Line8.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address9") Then
                            txtAddress_Line9.Enabled = value
                        End If
                        If sEnableExtraAddressFields.Contains("Address10") Then
                            txtAddress_Line10.Enabled = value
                        End If
                    End If
                End If
            End Set
        End Property

        Public WriteOnly Property ValidationGroup() As String
            Set(ByVal value As String)
                If bEnablePostCodeLookup = True Then
                    RqdLookup_Street.ValidationGroup = value
                    RqdLookup_Town.ValidationGroup = value
                    RqdLookup_County.ValidationGroup = value
                    RqdLookUp_Postcode.ValidationGroup = value

                Else
                    RqdAddress_Line1.ValidationGroup = value
                    RqdAddress_Line2.ValidationGroup = value
                    RqdAddress_PostCode.ValidationGroup = value
                    RqdAddress_Country.ValidationGroup = value
                End If
            End Set
        End Property

        Public WriteOnly Property TabIndex() As Integer
            Set(ByVal value As Integer)
                If bEnablePostCodeLookup = True Then
                    TxtLookup_Street.TabIndex = value
                    TxtLookup_Locality.TabIndex = value
                    TxtLookup_Town.TabIndex = value
                    TxtLookup_County.TabIndex = value
                    TxtLookup_Postcode.TabIndex = value
                    GISLookup_Country.TabIndex = value
                    txtLookup_PropDescription.TabIndex = value
                    txtLookup_GNAFID.TabIndex = value
                    txtLookup_DPID.TabIndex = value
                    txtLookup_DPID_Barcode.TabIndex = value
                    txtLookup_Latitude.TabIndex = value
                    txtLookup_Longitude.TabIndex = value
                Else
                    TxtAddress_Line1.TabIndex = value
                    TxtAddress_Line2.TabIndex = value
                    TxtAddress_Line3.TabIndex = value
                    TxtAddress_Line4.TabIndex = value
                    TxtAddress_Postcode.TabIndex = value
                    GISLookup_Country.TabIndex = value
                    GISAddress_Country.TabIndex = value
                    txtAddress_Line5.TabIndex = value
                    txtAddress_Line6.TabIndex = value
                    txtAddress_Line7.TabIndex = value
                    txtAddress_Line8.TabIndex = value
                    txtAddress_Line9.TabIndex = value
                    txtAddress_Line10.TabIndex = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Controls which all extra address additional fields will be visible.
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowExtraAddressFields() As String
            Set(ByVal value As String)
                If bEnablePostCodeLookup Then
                    txtLookup_PropDescription.Visible = IIf(value.Contains("Address5"), True, False)
                    txtLookup_GNAFID.Visible = IIf(value.Contains("Address6"), True, False)
                    txtLookup_DPID.Visible = IIf(value.Contains("Address7"), True, False)
                    txtLookup_DPID_Barcode.Visible = IIf(value.Contains("Address8"), True, False)
                    txtLookup_Latitude.Visible = IIf(value.Contains("Address9"), True, False)
                    txtLookup_Longitude.Visible = IIf(value.Contains("Address10"), True, False)

                    lblPropertyDescription.Visible = IIf(value.Contains("Address5"), True, False)
                    lblGNAFID.Visible = IIf(value.Contains("Address6"), True, False)
                    lblDPID.Visible = IIf(value.Contains("Address7"), True, False)
                    lblDPID_Barcode.Visible = IIf(value.Contains("Address8"), True, False)
                    lblLatitude.Visible = IIf(value.Contains("Address9"), True, False)
                    lblLongitude.Visible = IIf(value.Contains("Address10"), True, False)
                Else
                    txtAddress_Line5.Visible = IIf(value.Contains("Address5"), True, False)
                    txtAddress_Line6.Visible = IIf(value.Contains("Address6"), True, False)
                    txtAddress_Line7.Visible = IIf(value.Contains("Address7"), True, False)
                    txtAddress_Line8.Visible = IIf(value.Contains("Address8"), True, False)
                    txtAddress_Line9.Visible = IIf(value.Contains("Address9"), True, False)
                    txtAddress_Line10.Visible = IIf(value.Contains("Address10"), True, False)

                    lblAddress5.Visible = IIf(value.Contains("Address5"), True, False)
                    lblAddress6.Visible = IIf(value.Contains("Address6"), True, False)
                    lblAddress7.Visible = IIf(value.Contains("Address7"), True, False)
                    lblAddress8.Visible = IIf(value.Contains("Address8"), True, False)
                    lblAddress9.Visible = IIf(value.Contains("Address9"), True, False)
                    lblAddress10.Visible = IIf(value.Contains("Address10"), True, False)
                End If
            End Set
        End Property



        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            hdnGuid.Value = setSecureGuid()
            If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                TxtAddress_Line1.Attributes.Add("readonly", "readonly")
                TxtAddress_Line2.Attributes.Add("readonly", "readonly")
                TxtAddress_Line3.Attributes.Add("readonly", "readonly")
                TxtAddress_Line4.Attributes.Add("readonly", "readonly")
                TxtAddress_Postcode.Attributes.Add("readonly", "readonly")
                GISAddress_Country.Enabled = False
                GISAddressLookup_State.Enabled = False
            End If
            If Not Page.IsPostBack Then

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
                    If bEnablePostCodeLookup Then
                        PnlSpatialPostCodeLookup.Visible = False
                    Else
                        PnlSpatial.Visible = True
                    End If
                End If

            End If
            If Session(CNClaim) IsNot Nothing AndAlso Session(CNMode) = Mode.PayClaim Then
                If TxtAddress_Line1.Text.Length > 0 Then
                    GISAddress_Country.CssClass = GISAddress_Country.CssClass & " field-mandatory"
                    RqdAddress_Country.Enabled = True
                End If
            End If
        End Sub

        Private Function CheckStateLookup() As Boolean
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCountryList As NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            oCountryList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Country", True, False, , , , v_sOptionList)
            'Load the xml element 
            If v_sOptionList IsNot Nothing Then
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(sXML)
                Dim oNodeList As XmlNodeList
                'oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/BankAccount_Default[cashlisttype_id=3 and source_id=" & iSourceId & " and is_deleted=0]")
                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/Country[is_deleted=0]")
                If oNodeList IsNot Nothing AndAlso oNodeList.Count > 0 Then
                    For Each oNode As XmlNode In oNodeList
                        If oNode.Item("is_state_lookup") IsNot Nothing Then
                            If oNode.Item("is_state_lookup").InnerText = "1" Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    Next
                End If
            End If
            Return False
        End Function


        Public WriteOnly Property DisableClientSeValidation() As String
            Set(ByVal value As String)
                RqdLookup_Street.EnableClientScript = value
                RqdLookup_Town.EnableClientScript = value
                RqdLookup_County.EnableClientScript = value
                RqdLookUp_Postcode.EnableClientScript = value
                RqdLookup_Locality.EnableClientScript = value
                RqdAddress_Line1.EnableClientScript = value
                RqdAddress_Line2.EnableClientScript = value
                RqdAddress_Line3.EnableClientScript = value
                RqdAddress_PostCode.EnableClientScript = value
                RqdAddress_Country.EnableClientScript = value
                RqdAddress_Line4.EnableClientScript = value

            End Set
        End Property

        Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
            Dim oAddress As New NexusProvider.Address(Address1, Postcode, CountryCode)

            If (CountryCode Is Nothing AndAlso sDefaultCountryKey IsNot Nothing AndAlso
                String.IsNullOrEmpty(GISLookup_Country.Value) AndAlso bShowCountry) Then
                GISLookup_Country.Value = sDefaultCountryKey
            End If

               If Session(CNClaim) IsNot Nothing AndAlso Session(CNMode) = Mode.PayClaim Then
                    If GISAddress_Country.Value <> "" Then
                        TxtAddress_Line1.CssClass = TxtAddress_Line1.CssClass & " field-mandatory"
                        RqdAddress_Line1.Enabled = True
                        RqdAddress_Line1.ErrorMessage = GetLocalResourceObject("lbl_address1")
                    Else
                        TxtAddress_Line1.CssClass = TxtAddress_Line1.CssClass.Replace("field-mandatory", "")
                        RqdAddress_Line1.Enabled = False
                        GISAddress_Country.CssClass = GISAddress_Country.CssClass.Replace("field-mandatory", "")
                        RqdAddress_Country.Enabled = False
                    End If
                End If
        End Sub
    End Class

End Namespace
