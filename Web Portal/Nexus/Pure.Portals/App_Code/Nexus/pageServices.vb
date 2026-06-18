Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports System.IO
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Session
Imports System.Security.Cryptography.X509Certificates

Imports System.Web.Script.Serialization
Imports System.Security.Authentication
Imports System.Net

<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://ssp-uk.com/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
Public Class pageServices
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Do postcode lookup and return json formatted address object collection
    ''' </summary>
    ''' <param name="PostCode">postcode to match</param>
    ''' <returns>String of json serialised address objects</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Function findAddress(ByVal PostCode As String, ByVal sGuid As String) As String
        'check if guid passed in is valid
        If sGuid <> Session(CNSecureGuid) Then
            Throw New Exception("Security check failed")
        End If

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

                    If Not premise.organisation Is Nothing AndAlso premise.organisation <> "" Then
                        oAddress.Address1 = premise.organisation & ", " & oAddress.Address1
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

    ''' <summary>
    ''' To Set QAS Authentication header
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerateHeader() As QAS.QAQueryHeader
        Return New QAS.QAQueryHeader() With { _
          .QAAuthentication = New QAS.QAAuthentication() With { _
           .Username = AppSettings("QASUserName"), _
           .Password = AppSettings("QASPassword") _
         } _
        }

    End Function


    ''' <summary>
    ''' TO find all matching address from QAS service
    ''' </summary>
    ''' <param name="sSearchString">pipe separated address lines</param>
    ''' <param name="sCountryCode">need to pass country code e.g. AUS for australia, NZL for New Zealand</param>
    ''' <param name="sGuid">unique key to indentify the caller</param>
    ''' <returns>Address List of Type NexusProvider.AddressCollection</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Overloads Function findQASAddress(ByVal sSearchString As String, ByVal sCountryCode As String, ByVal sGuid As String) As NexusProvider.AddressCollection
        Dim oAddressCollection As New NexusProvider.AddressCollection
        Dim oAddress As NexusProvider.Address

        Try
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolTypeExtensions.Tls12
            'check if guid passed in is valid
            If sGuid <> Session(CNSecureGuid) Then
                Throw New Exception("Security check failed")
            End If

            If sSearchString <> String.Empty And sCountryCode <> String.Empty Then
                Dim oQASOnDemandIntermediary As New QAS.QASOnDemandIntermediary()
                Dim oDoSearchResponse As New QAS.QASearchResult

                oQASOnDemandIntermediary.Url = AppSettings("QASServerURL")
                oQASOnDemandIntermediary.QAQueryHeaderValue = GenerateHeader()

                Dim EngineType As Integer
                If AppSettings("QASEngineMode") IsNot Nothing AndAlso Convert.ToInt16(AppSettings("QASEngineMode")) >= 0 AndAlso Convert.ToInt16(AppSettings("QASEngineMode")) <= 4 Then
                    EngineType = Convert.ToInt16(AppSettings("QASEngineMode"))
                Else
                    EngineType = QAS.EngineEnumType.Intuitive
                End If

                Dim oDoSearchRequest As New QAS.QASearch() With {
                    .Search = sSearchString,
                    .FormattedAddressInPicklist = True,
                    .Country = sCountryCode,
                    .Layout = AppSettings("QASLayout") + " " + sCountryCode,
                    .Localisation = AppSettings("QASLocalisation"),
                    .RequestTag = "Single Line address search",
                    .Engine = New QAS.EngineType() With {.Value = EngineType}
                }

                oDoSearchResponse = oQASOnDemandIntermediary.DoSearch(oDoSearchRequest)

                If oDoSearchResponse.QAPicklist.PicklistEntry(0).Moniker <> String.Empty Then
                    For iCt As Integer = 0 To oDoSearchResponse.QAPicklist.PicklistEntry.Length - 1
                        oAddress = New NexusProvider.Address

                        Dim aAddress() As String = oDoSearchResponse.QAPicklist.PicklistEntry(iCt).PartialAddress.Split(CType(",", Char))

                        oAddress.Monikar = oDoSearchResponse.QAPicklist.PicklistEntry(iCt).Moniker
                        oAddress.CountryCode = sCountryCode
                        oAddress.Address1 = aAddress(0)
                        If aAddress.Length > 1 Then
                            oAddress.Address2 = aAddress(1)
                        Else
                            oAddress.Address2 = ""
                        End If
                        oAddress.PostCode = oDoSearchResponse.QAPicklist.PicklistEntry(iCt).Postcode

                        oAddressCollection.Add(oAddress)
                    Next
                Else
                    oAddress = New NexusProvider.Address
                    oAddress.Address1 = oDoSearchResponse.QAPicklist.PicklistEntry(0).Picklist
                    oAddressCollection.Add(oAddress)
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return oAddressCollection

    End Function


    ''' <summary>
    ''' To get full address detail from QAS Service
    ''' </summary>
    ''' <param name="sMoniker">Monikar returned for selected address from DOSearch method</param>
    ''' <returns>Object of type NexusProvider.Address</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Overloads Function findQASFullAddress(ByVal sMoniker As String, ByVal sCountryCode As String) As NexusProvider.Address

        Dim oAddress As New NexusProvider.Address

        Try

            If sMoniker <> String.Empty Then
                Dim oQASOnDemandIntermediary As New QAS.QASOnDemandIntermediary()
                Dim oDoGetAddressResponse As New QAS.Address

                oQASOnDemandIntermediary.Url = AppSettings("QASServerURL")
                oQASOnDemandIntermediary.QAQueryHeaderValue = GenerateHeader()

                Dim oDoGetAddressRequest As New QAS.QAGetAddress() With { _
                    .Moniker = sMoniker, _
                    .Layout = AppSettings("QASLayout") + " " + sCountryCode, _
                    .Localisation = AppSettings("QASLocalisation"), _
                    .RequestTag = "Return final address" _
               }

                oDoGetAddressResponse = oQASOnDemandIntermediary.DoGetAddress(oDoGetAddressRequest)

                If oDoGetAddressResponse.QAAddress IsNot Nothing Then
                    oAddress.Monikar = sMoniker
                    For Each oQASAddressLine As QAS.AddressLineType In oDoGetAddressResponse.QAAddress.AddressLine
                        Select Case oQASAddressLine.Label
                            Case "Address Line 1"
                                oAddress.Address1 = oQASAddressLine.Line
                            Case "PAF Locality"
                                oAddress.Address2 = oQASAddressLine.Line
                            Case "State code"
                                oAddress.Address3 = oQASAddressLine.Line
                            Case "PAF Postcode"
                                oAddress.PostCode = oQASAddressLine.Line
                            Case "Country"
                                oAddress.CountryDescription = oQASAddressLine.Line
                            Case "G-NAF PID"
                                oAddress.QASGNAFPID = oQASAddressLine.Line
                        End Select
                    Next
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return oAddress

    End Function

    ''' <summary>
    ''' Do postcode lookup and return json formatted address object collection
    ''' </summary>
    ''' <param name="sPostCode">postcode to match</param>
    ''' <param name="sGuid">Secure session id</param>
    ''' <returns>String of json serialised address objects</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)>
    Public Function FindPostLookupAddresses(ByVal sPostodeUser As String, ByVal sPostcodePass As String, ByVal sPostcodeSerialNo As String,
    ByVal sPostCode As String, ByVal sPostcodeQueryString As String, ByVal sGuid As String) As String
        'check if guid passed in is valid
        If sGuid <> Session(CNSecureGuid) Then
            Throw New Exception("Security check failed")
        End If

        Dim oRequest As System.Net.HttpWebRequest = Nothing
        Dim oResponse As System.Net.HttpWebResponse = Nothing
        Dim sUrl As String = String.Empty
        Dim strResponseFromServer As String = String.Empty
        Dim sResponse As String = String.Empty
        Dim cErrorCollection As New Dictionary(Of String, String)
        Dim oEncoding As Encoding = Nothing
        Dim jSearializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim stResponseStream As Stream = Nothing
        Dim srLookupValues As StreamReader = Nothing
        Dim sPostcodeServiceUrl As String = String.Empty
        Dim sPostcodeUserName As String = String.Empty
        Dim sPostcodePassword As String = String.Empty
        Dim sSerialNumber As String = String.Empty
        Try
            sPostcodeServiceUrl = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), 
                       Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()) _
                       .AddressControl.PostCodeWebServiceUrl

            If (String.IsNullOrEmpty(sPostodeUser)) Then
                sPostcodeUserName = AppSettings("PostcodeUser")
                sPostcodePassword = AppSettings("PostcodePass")
                sSerialNumber = AppSettings("PostcodeSerialNo")
            Else
                sPostcodeUserName = sPostodeUser
                sPostcodePassword = sPostcodePass
                sSerialNumber = sPostcodeSerialNo
            End If

            If (String.IsNullOrEmpty(sPostcodeServiceUrl)) Then
                cErrorCollection.Add("error", "NO_SERVICE_CONFIGURED")
            ElseIf (String.IsNullOrEmpty(sPostcodeUserName)) Then
                cErrorCollection.Add("error", "USERNAME_NOT_CONFIGURED")
            ElseIf (String.IsNullOrEmpty(sPostcodePassword)) Then
                cErrorCollection.Add("error", "PASSWORD_NOT_CONFIGURED")
            ElseIf (String.IsNullOrEmpty(sSerialNumber)) Then
                cErrorCollection.Add("error", "SERIALNUMBER_NOT_CONFIGURED")
            End If

            If (cErrorCollection.Count > 0) Then
                sResponse = jSearializer.Serialize(cErrorCollection)
                Return sResponse
            End If

            If (System.Web.HttpContext.Current.Cache(sPostCode) IsNot Nothing) Then
                sResponse = System.Web.HttpContext.Current.Cache(sPostCode)
            Else

                sUrl = String.Format("{0}?{1}", sPostcodeServiceUrl, sPostcodeQueryString)
                oRequest = DirectCast(WebRequest.Create(sUrl), HttpWebRequest)
                oResponse = DirectCast(oRequest.GetResponse(), HttpWebResponse)
                stResponseStream = oResponse.GetResponseStream()
                srLookupValues = New StreamReader(stResponseStream)
                strResponseFromServer = srLookupValues.ReadToEnd()

                If (strResponseFromServer.Trim().Length = 0) Then
                    cErrorCollection.Add("error", "INVALID_RESPONSE")
                Else

                    strResponseFromServer = strResponseFromServer.Substring(5, strResponseFromServer.Length - 6)
                    Dim jss As New JavaScriptSerializer()
                    Dim oJsonResult As Dictionary(Of Object, Object) = jss.Deserialize(Of Dictionary(Of Object, Object))(strResponseFromServer.Trim())
                    If oJsonResult.ContainsKey("ErrorText") AndAlso oJsonResult("ErrorText").ToString().Length > 0 Then
                        cErrorCollection.Add("error", oJsonResult("ErrorText"))
                    Else
                        sResponse = strResponseFromServer
                        System.Web.HttpContext.Current.Cache.Insert(sPostCode, sResponse, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    End If

                End If

                If (cErrorCollection.Count > 0) Then
                    sResponse = jSearializer.Serialize(cErrorCollection)
                    Return sResponse
                End If
                Return sResponse
            End If

            If (cErrorCollection.Count > 0) Then
                sResponse = jSearializer.Serialize(cErrorCollection)
                Return sResponse
            End If

        Catch ex As System.Net.WebException
            cErrorCollection.Add("error", "SERVICE_ERROR")
        Catch ex As System.Exception
            cErrorCollection.Add("error", "INVALID_RESPONSE")
        Finally
            oRequest = Nothing
            oResponse = Nothing
            cErrorCollection = Nothing
            srLookupValues = Nothing
            strResponseFromServer = Nothing
        End Try

        Return sResponse
    End Function

End Class
