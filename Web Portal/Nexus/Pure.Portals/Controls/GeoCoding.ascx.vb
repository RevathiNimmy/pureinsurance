Imports System.Net
Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports System.Runtime.Serialization.Json
Imports System.Collections.Generic
Imports System.Xml
Imports System.Web.Script.Serialization

Partial Class Controls_Geocoding
    Inherits System.Web.UI.UserControl

    Public Property PostCodeCtrl() As String

    Public Property LongitudeCtrl() As String

    Public Property LatitudeCtrl() As String

    Public Property MappedControls() As String

    Public Property OutputProperty() As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnGeoCoding_Click(sender As Object, e As EventArgs) Handles btnGeoCoding.Click
        Dim sPostCode As String = String.Empty
        Dim sLatitude As String = String.Empty
        Dim sLongitude As String = String.Empty
        Dim aOutputProperty As String() = OutputProperty.Split(";")
        Dim aMappedControls As String() = MappedControls.Split(";")
        Dim cMappedCtrlValue As New Dictionary(Of String, String)
        Dim bIsGeocodingCheck As Boolean = True

        Dim txtPostCode As TextBox = CType(Parent.FindControl(Me.PostCodeCtrl), TextBox)
        If (txtPostCode IsNot Nothing) Then
            sPostCode = txtPostCode.Text
        End If

        Dim txtLatitude As TextBox = CType(Parent.FindControl(Me.LatitudeCtrl), TextBox)
        If (txtLatitude IsNot Nothing) Then
            sLatitude = txtLatitude.Text
        End If

        Dim txtLongitude As TextBox = CType(Parent.FindControl(Me.LongitudeCtrl), TextBox)
        If (txtLongitude IsNot Nothing) Then
            sLongitude = txtLongitude.Text

        End If

        If (ViewState("PostCode") IsNot Nothing AndAlso ViewState("PostCode").ToString() = sPostCode) AndAlso _
            (ViewState("Latitude") IsNot Nothing AndAlso ViewState("Latitude").ToString() = sLatitude) AndAlso _
            (ViewState("Longitude") IsNot Nothing AndAlso ViewState("Longitude").ToString() = sLongitude) Then
            bIsGeocodingCheck = False
        End If

        If bIsGeocodingCheck Then
            'Clear down existing data in mapped fields
            For i As Integer = 0 To aMappedControls.Length - 1
                Dim oControl As TextBox = CType(Parent.FindControl(aMappedControls(i)), TextBox)
                If oControl IsNot Nothing Then
                    oControl.Text = ""
                End If
            Next

            'Call Geocoding Service Provider and serialization of response into JSON
            cMappedCtrlValue = GeocodingCheck(aOutputProperty, sPostCode, sLatitude, sLongitude, aMappedControls)

            Dim jSearializer As New System.Web.Script.Serialization.JavaScriptSerializer()
            hdnGeocodingResponse.Value = jSearializer.Serialize(cMappedCtrlValue)
        End If
    End Sub

    ''' <summary>
    ''' Use to get geocoding response XML based on search criteria i.e. latitude,logitude and postcode
    ''' </summary>
    ''' <param name="aOutputProperty"></param>
    ''' <param name="sPostCode"></param>
    ''' <param name="sLatitude"></param>
    ''' <param name="sLongitude"></param>
    ''' <param name="aMappedControls"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GeocodingCheck(aOutputProperty As String(), sPostCode As String, sLatitude As String, sLongitude As String, aMappedControls As String()) As Dictionary(Of String, String)
        Dim cMappedCtrlValue As New Dictionary(Of String, String)
        Dim oRequest As HttpWebRequest = Nothing
        Dim oResponse As HttpWebResponse = Nothing
        Dim sUrl As String = String.Empty
        Dim sResponseFromServer As String = String.Empty
        Dim sCredidentials As String = String.Empty
        Dim sAuthorization As String = String.Empty
        Dim oEncoding As Encoding = Nothing

        Try
            Dim sGeocodingWebServiceUrl As String = AppSettings("GeocodingServiceURL")
            Dim sGeocodingUserName As String = AppSettings("GeocodingUserName")
            Dim sGeocodingPassword As String = AppSettings("GeocodingPassword")

            If (String.IsNullOrEmpty(sGeocodingWebServiceUrl)) Then
                cMappedCtrlValue.Add("error", "NO_WEB_SERVICE_CONFIGURED")
                ClearViewState()
                Return cMappedCtrlValue
            ElseIf (String.IsNullOrEmpty(sGeocodingUserName)) Then
                cMappedCtrlValue.Add("error", "USERNAME_NOT_CONFIGURED")
                ClearViewState()
                Return cMappedCtrlValue
            ElseIf (String.IsNullOrEmpty(sGeocodingPassword)) Then
                cMappedCtrlValue.Add("error", "PASSWORD_NOT_CONFIGURED")
                ClearViewState()
                Return cMappedCtrlValue
            End If

            Dim sGeocodeRequestData As String = String.Empty

            If (Not String.IsNullOrEmpty(sLatitude) AndAlso Not String.IsNullOrEmpty(sLongitude)) Then
                sGeocodeRequestData = "{""datasets"":[""TOWERGATEAIUA_POINT_IN_POLYGON""],""criteria"":{""long"":[""" + sLongitude + """],""lat"":[""" + sLatitude + """]},""result"":{""maxrows"":""1""}}"
            ElseIf (Not String.IsNullOrEmpty(sPostCode)) Then
                sGeocodeRequestData = "{""datasets"":[""TOWERGATEAIUA_POINT_IN_POLYGON""],""criteria"":{""postcode"":[""" + sPostCode + """]},""result"":{""maxrows"":""1""}}"
            Else
                'Handle Input Parameter Error
                cMappedCtrlValue.Add("error", "INPUT_PARAMETERS_NOT_CONFIGURED")
                For i As Integer = 0 To aMappedControls.Length - 1
                    Dim oMappedControl As Object = CType(Parent.FindControl(aMappedControls(i)), Object)
                    If oMappedControl IsNot Nothing Then
                        cMappedCtrlValue.Add(oMappedControl.ClientID, "")
                    End If
                Next
                ClearViewState()
                Return cMappedCtrlValue
            End If

            oEncoding = New UTF8Encoding()
            Dim bData As Byte() = oEncoding.GetBytes(sGeocodeRequestData)
            oRequest = DirectCast(WebRequest.Create(sGeocodingWebServiceUrl), HttpWebRequest)
            sCredidentials = sGeocodingUserName + ":" + sGeocodingPassword
            sAuthorization = Convert.ToBase64String(Encoding.Default.GetBytes(sCredidentials))
            oRequest.Headers.Add("Authorization", "Basic " + sAuthorization)

            oRequest.Method = "POST"
            oRequest.ContentType = "application/json"
            oRequest.ContentLength = bData.Length
            Using stream As Stream = oRequest.GetRequestStream()
                stream.Write(bData, 0, bData.Length)
            End Using

            oResponse = oRequest.GetResponse()

            Using stResponseStream As StreamReader = New StreamReader(oResponse.GetResponseStream())
                sResponseFromServer = stResponseStream.ReadToEnd()
            End Using

            If (sResponseFromServer.Trim().Length = 0) Then
                cMappedCtrlValue.Add("error", "INVALID_RESPONSE")
                ClearViewState()
                Return cMappedCtrlValue
            Else
                For i As Integer = 0 To aMappedControls.Length - 1
                    Dim oMappedControl As Object = CType(Parent.FindControl(aMappedControls(i)), Object)
                    If oMappedControl IsNot Nothing Then
                        cMappedCtrlValue.Add(oMappedControl.ClientID, GetPropertyValue(aOutputProperty(i), sResponseFromServer))
                    End If
                Next
            End If

            ViewState("PostCode") = sPostCode
            ViewState("Latitude") = sLatitude
            ViewState("Longitude") = sLongitude

        Catch ex As System.Net.WebException
            cMappedCtrlValue.Add("error", "WEB_SERVICE_ERROR")
            ClearViewState()
        Catch ex As System.Exception
            cMappedCtrlValue.Add("error", "GEOCODING_FUNCTION_ERROR")
            ClearViewState()
        Finally
            oRequest = Nothing
            oResponse = Nothing
            oEncoding = Nothing
        End Try

        Return cMappedCtrlValue
    End Function

    ''' <summary>
    ''' Use to get property value from geocoding response JSON
    ''' </summary>
    ''' <param name="sPropertyName"></param>
    ''' <param name="sResponseJSON"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetPropertyValue(sPropertyName As String, sResponseJSON As String) As String
        Dim sValue As String = String.Empty
        Dim jss As New JavaScriptSerializer()
        Dim oJsonResult As Dictionary(Of Object, Object) = jss.Deserialize(Of Dictionary(Of Object, Object))(sResponseJSON)
        If oJsonResult.Count > 0 AndAlso oJsonResult("locations") IsNot Nothing AndAlso
            oJsonResult("locations")("TOWERGATEAIUA_POINT_IN_POLYGON") IsNot Nothing _
            AndAlso oJsonResult("locations")("TOWERGATEAIUA_POINT_IN_POLYGON").Length > 0 Then

            Try
                sValue = oJsonResult("locations")("TOWERGATEAIUA_POINT_IN_POLYGON")(0)(sPropertyName)
            Catch ex As Exception

            End Try

        End If
        Return sValue
    End Function

    Sub ClearViewState()
        ViewState("PostCode") = Nothing
        ViewState("Latitude") = Nothing
        ViewState("Longitude") = Nothing
    End Sub
End Class







