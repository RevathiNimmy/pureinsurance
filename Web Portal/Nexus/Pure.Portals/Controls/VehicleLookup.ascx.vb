Imports System.Net
Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Linq
Imports System.Xml


Partial Class Controls_VehicleLookup
    Inherits System.Web.UI.UserControl

    Public Property MappedControls() As String

    Public Property OutputProperty() As String

    Private aPropertyKeyes As String()
    Private aOutputProperty As String()
    Private aMappedControls As String()
    Private cMappedCtrlValue As New Dictionary(Of String, String)

    Protected Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        aOutputProperty = OutputProperty.Split(";")
        aMappedControls = MappedControls.Split(";")

        Dim sVehicleLookupWebServiceUrl As String = AppSettings("VehicleLookupWebServiceURL")
        Dim sVehicleLookupCustCode As String = AppSettings("VehicleLookupCustomerCode")
        Dim sVehicleLookupPassword As String = AppSettings("VehicleLookupPassword")
        Dim sRegistrationNumer As String = txtRegNumber.Text.Trim()

        If (sVehicleLookupWebServiceUrl Is Nothing) Then
            hdnVehicleLookupResponse.Value = "NO_WEB_SERVICE_CONFIGURED"
            Return
        End If

        If (sVehicleLookupCustCode Is Nothing) Then
            hdnVehicleLookupResponse.Value = "NO_CUST_CODE_CONFIGURED"
            Return
        End If

        If (sVehicleLookupPassword Is Nothing) Then
            hdnVehicleLookupResponse.Value = "NO_PASSWORD_CONFIGURED"
            Return
        End If

        'Clear down existing data in mapped fields
        For i As Integer = 0 To aMappedControls.Length - 1
            Dim oControl As TextBox = CType(Parent.FindControl(aMappedControls(i)), TextBox)
            If oControl IsNot Nothing Then
                oControl.Text = ""
            End If
        Next

        'Invoke HPI Webservice to get vehicle details
        InvokeService(sVehicleLookupWebServiceUrl, sVehicleLookupCustCode, sVehicleLookupPassword, sRegistrationNumer)

    End Sub

    ''' <summary>
    ''' Inovke the HPI service with SOAP XML request 
    ''' </summary>
    ''' <param name="sUrl"></param>
    ''' <param name="sCustomerCode"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="sRegNumber"></param>
    Public Sub InvokeService(ByVal sUrl As String, ByVal sCustomerCode As String, ByVal sPassword As String, ByVal sRegNumber As String)
        Dim oWebRequest As HttpWebRequest = Nothing
        Dim oWebResponse As HttpWebResponse = Nothing
        Dim cParams As Dictionary(Of String, String) = New Dictionary(Of String, String)()
        Dim sResponse As String = String.Empty
        Dim sResultString As String = String.Empty
        Dim sbSoapRequest As New StringBuilder
        Dim sWarningAndError As String = String.Empty
        Dim jSearializer As New System.Web.Script.Serialization.JavaScriptSerializer()

        Try
            sbSoapRequest.Append("<soapenv:Envelope xmlns:soapenv = ""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sup = ""http://webservices.hpi.co.uk/SupplementaryEnquiryV1"" >")
            sbSoapRequest.Append("<soapenv:Header/>")
            sbSoapRequest.Append("<soapenv:Body>")
            sbSoapRequest.Append("<sup:EnquiryRequest>")
            sbSoapRequest.Append("<sup:Authentication>")
            sbSoapRequest.Append("<sup:SubscriberDetails>")
            sbSoapRequest.Append("<sup:CustomerCode>{0}</sup:CustomerCode>")
            sbSoapRequest.Append("<sup:Initials>{1}</sup:Initials>")
            sbSoapRequest.Append("<sup:Password>{2}</sup:Password>")
            sbSoapRequest.Append("</sup:SubscriberDetails>")
            sbSoapRequest.Append("</sup:Authentication>")
            sbSoapRequest.Append("<sup:Request>")
            sbSoapRequest.Append("<sup:Asset>")
            sbSoapRequest.Append("<sup:Vrm>{3}</sup:Vrm>")
            sbSoapRequest.Append("<sup:Vin></sup:Vin>")
            sbSoapRequest.Append("<sup:Mileage>0</sup:Mileage>")
            sbSoapRequest.Append("<sup:Reference></sup:Reference>")
            sbSoapRequest.Append("</sup:Asset>")
            sbSoapRequest.Append("<sup:PrimaryProduct>")
            sbSoapRequest.Append("<sup:Code>HPI75</sup:Code>")
            sbSoapRequest.Append("</sup:PrimaryProduct>")
            sbSoapRequest.Append("<sup:SupplementaryProduct>")
            sbSoapRequest.Append("<sup:Code>ADSMT</sup:Code>")
            sbSoapRequest.Append("</sup:SupplementaryProduct>")
            sbSoapRequest.Append("</sup:Request>")
            sbSoapRequest.Append("</sup:EnquiryRequest>")
            sbSoapRequest.Append("</soapenv:Body>")
            sbSoapRequest.Append("</soapenv:Envelope>")

            oWebRequest = DirectCast(WebRequest.Create(sUrl), HttpWebRequest)
            oWebRequest.ContentType = "text/xml;charset=""utf-8"""
            oWebRequest.Accept = "text/xml"
            oWebRequest.Method = "POST"

            Using stResponseStream As Stream = oWebRequest.GetRequestStream()
                Dim sSoapRequest As String = String.Format(sbSoapRequest.ToString(), sCustomerCode, "SSP", sPassword, sRegNumber)
                Using swHpiReq As StreamWriter = New StreamWriter(stResponseStream)
                    swHpiReq.Write(sSoapRequest)
                End Using
            End Using

            Using responseReader As StreamReader = New StreamReader(oWebRequest.GetResponse().GetResponseStream())
                sResponse = responseReader.ReadToEnd()
            End Using

            'Validate response Warnings and errors
            sWarningAndError = GetDatafromXML("//ns1:Warning/ns1:Description", sResponse)
            If Not String.IsNullOrEmpty(sWarningAndError) Then
                cMappedCtrlValue.Add("warning", sWarningAndError)
                hdnVehicleLookupResponse.Value = jSearializer.Serialize(cMappedCtrlValue)
                Return
            End If

            sWarningAndError = GetDatafromXML("//ns1:HpiSoapFault/ns1:Error/ns1:Description", sResponse)
            If Not String.IsNullOrEmpty(sWarningAndError) Then
                cMappedCtrlValue.Add("error", sWarningAndError)
                hdnVehicleLookupResponse.Value = jSearializer.Serialize(cMappedCtrlValue)
                Return
            End If

            For i As Integer = 0 To aMappedControls.Length - 1
                Dim oMappedControl As Object = CType(Parent.FindControl(aMappedControls(i)), Object)
                If oMappedControl IsNot Nothing AndAlso aOutputProperty.Length > 0 Then
                    cMappedCtrlValue.Add(oMappedControl.ClientID, GetPropertyValue(aOutputProperty(i), sResponse))
                End If
            Next

            hdnVehicleLookupResponse.Value = jSearializer.Serialize(cMappedCtrlValue)

        Catch ex As System.Net.WebException
            cMappedCtrlValue.Add("error", "INVALID_RESPONSE")
            hdnVehicleLookupResponse.Value = jSearializer.Serialize(cMappedCtrlValue)
            txtRegNumber.Text = ""
        Catch ex As System.Exception
            cMappedCtrlValue.Add("error", "INVALID_RESPONSE")
            hdnVehicleLookupResponse.Value = jSearializer.Serialize(cMappedCtrlValue)
            txtRegNumber.Text = ""
        Finally
            oWebRequest = Nothing
            oWebResponse = Nothing
            cParams = Nothing
            sbSoapRequest = Nothing
            jSearializer = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Use to get property value from DVLA XML response 
    ''' </summary>
    ''' <param name="sPropertyName"></param>
    ''' <param name="sXmlResponse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetPropertyValue(sPropertyName As String, sXmlResponse As String) As String
        Dim sValue As String = String.Empty
        Dim aPropertyKeyes As String() = sPropertyName.Split(".")
        If sPropertyName.ToUpper = "CORE_REG_NUMBER" Then
            sValue = txtRegNumber.Text.Trim()
        ElseIf aPropertyKeyes.Length > 0 Then
            Dim sXpath As String = "//"
            For nCount As Integer = 0 To aPropertyKeyes.Length - 2
                aPropertyKeyes(nCount) = "ns1:" & aPropertyKeyes(nCount)
                sXpath = sXpath & aPropertyKeyes(nCount) & "/"
            Next

            sXpath = sXpath & "ns1:" & aPropertyKeyes(aPropertyKeyes.Length - 1)
            sValue = GetDatafromXML(sXpath, sXmlResponse)
        End If

        Return sValue
    End Function

    ''' <summary>
    ''' Get XML child not value
    ''' </summary>
    ''' <param name="Xpath"></param>
    ''' <param name="sXmlResponse"></param>
    ''' <returns></returns>
    Public Function GetDatafromXML(ByVal Xpath As String, ByVal sXmlResponse As String) As String
        Dim dStrValue As String = ""
        If Not String.IsNullOrEmpty(sXmlResponse) Then
            'Create the XmlDocument.
            Dim xdocResponse As XmlDocument = New XmlDocument()
            xdocResponse.LoadXml(sXmlResponse)

            Dim nsmgr As New XmlNamespaceManager(xdocResponse.NameTable)
            nsmgr.AddNamespace("ns1", "http://webservices.hpi.co.uk/SupplementaryEnquiryV1")
            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance")
            nsmgr.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/")

            Dim oNode As XmlNode = xdocResponse.SelectSingleNode(Xpath, nsmgr)
            If oNode IsNot Nothing Then
                dStrValue = oNode.InnerText
            End If
        End If
        Return dStrValue
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If (CType(Session.Item(CNMode), Mode) = Mode.View OrElse CType(Session.Item(CNMode), Mode) = Mode.Review OrElse CType(Session.Item(CNMode), Mode) = Mode.ViewClaim) Then
                btnFind.Enabled = False
                txtRegNumber.Enabled = False
            End If
        End If
    End Sub
End Class
