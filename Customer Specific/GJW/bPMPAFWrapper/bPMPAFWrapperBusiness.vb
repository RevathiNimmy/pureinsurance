Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Runtime.InteropServices

<ComVisible(True)> _
Public Class Business

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 12/06/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, rules required to manipulate
    '              a uctPMAddressControl.
    '
    ' Edit History: TF120698 - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"
    Private Const ksStreetLevel As String = "1"
    Private Const ksPropertyLevel As String = "2"


    Private m_sAFDServiceURL As String = "http://pce.afd.co.uk"
    Private m_sAFDSerial As String = "825326"
    Private m_sAFDPassword As String = "GdMRKcvU"
    Private m_sAFDUserID As String = "SOFTW08"

    <ComVisible(True)> _
    Public Function FindAddress(ByVal v_sPostCode As String, ByVal v_sAddressLine1 As String, ByRef r_oAddressArray As Object) As Long
        Dim r_oKeyArray As Object
        Dim oAddressArray As Object
        FindAFDAddress(r_oAddressArray, r_oKeyArray, v_sPostCode, v_sAddressLine1)
        If r_oKeyArray IsNot Nothing Then
            For iCount As Integer = 0 To UBound(r_oKeyArray, 2)
                Dim r_sPostCode, r_sAddressLine1, r_sAddressLine2, r_sAddressLine3, r_sAddressLine4, r_sCountry As String
                GetAFDDetails(r_oKeyArray(0, iCount), r_sPostCode, r_sAddressLine1, r_sAddressLine2, r_sAddressLine3, r_sAddressLine4, r_sCountry)
                If oAddressArray IsNot Nothing Then
                    ReDim Preserve oAddressArray(5, UBound(oAddressArray, 2) + 1)
                    oAddressArray(0, UBound(oAddressArray, 2)) = Replace(r_sAddressLine1, ",", " ")
                    oAddressArray(1, UBound(oAddressArray, 2)) = Replace(r_sAddressLine2, ",", " ")
                    oAddressArray(2, UBound(oAddressArray, 2)) = Replace(r_sAddressLine3, ",", " ")
                    oAddressArray(3, UBound(oAddressArray, 2)) = Replace(r_sAddressLine4, ",", " ")
                    oAddressArray(4, UBound(oAddressArray, 2)) = r_sPostCode
                    oAddressArray(5, UBound(oAddressArray, 2)) = r_sCountry
                Else
                    ReDim oAddressArray(5, 0)
                    oAddressArray(0, 0) = Replace(r_sAddressLine1, ",", " ")
                    oAddressArray(1, 0) = Replace(r_sAddressLine2, ",", " ")
                    oAddressArray(2, 0) = Replace(r_sAddressLine3, ",", " ")
                    oAddressArray(3, 0) = Replace(r_sAddressLine4, ",", " ")
                    oAddressArray(4, 0) = r_sPostCode
                    oAddressArray(5, 0) = r_sCountry
                End If
            Next
        End If
        r_oAddressArray = oAddressArray
        Return 1
    End Function


    Private Function FindAFDAddress( _
        ByRef r_vAddressArray As Object, _
        ByRef r_vKeyArray As Object, _
        Optional ByRef r_sPostCode As String = "", _
        Optional ByRef r_sAddressLine1 As String = "", _
        Optional ByRef r_sAddressLine2 As String = "", _
        Optional ByRef r_sAddressLine3 As String = "", _
        Optional ByRef r_sAddressLine4 As String = "") As Long

        Const kMethodName As String = "FindAFDAddress"

        Dim xmlDoc As Object
        Dim root As Object
        Dim pcFromNode As Object
        Dim dataNode As Object
        Dim itemNodes As Object
        Dim listNode As Object
        Dim keyNode As Object
        Dim xmlLocation As String
        Dim lCount As Integer

        Dim vDetails As Object
        Dim iSepPos As Integer



        FindAFDAddress = gPMConstants.PMEReturnCode.PMTrue

        ' Initialise the Microsoft XML Document Object Model
        xmlDoc = CreateObject("Microsoft.XMLDOM")
        xmlDoc.async = False

        ' Build up the XML query string
        xmlLocation = m_sAFDServiceURL + "/afddata.pce?"
        xmlLocation = xmlLocation + "Serial=" + m_sAFDSerial + "&"
        xmlLocation = xmlLocation + "Password=" + m_sAFDPassword + "&"
        xmlLocation = xmlLocation + "UserID=" + m_sAFDUserID + "&"
        xmlLocation = xmlLocation + "Data=Address&Task=Search&Fields=List"

        ' Set the maximum number of records to return
        xmlLocation = xmlLocation + "&MaxQuantity=100"

        ' Set the Country Name or ISO code for International operations
        xmlLocation = xmlLocation + "&Country=UK"

        If Trim(r_sAddressLine1) <> "" Then
            iSepPos = InStr(1, r_sAddressLine1, ",")
            If ToSafeInteger(iSepPos) > 0 Then
                r_sAddressLine1 = Mid$(r_sAddressLine1, 1, iSepPos - 1)
            End If
        End If

        ' Set the parameters to search on
        xmlLocation = xmlLocation + "&Property=" + r_sAddressLine1
        xmlLocation = xmlLocation + "&Locality=" + r_sAddressLine2
        xmlLocation = xmlLocation + "&Town=" + r_sAddressLine3
        xmlLocation = xmlLocation + "&PostalCounty=" + r_sAddressLine4
        xmlLocation = xmlLocation + "&Postcode=" + r_sPostCode

        ' Load the XML from the webserver with the query string
        xmlDoc.Load(xmlLocation)

        ' Check for any XML Parser error
        If xmlDoc.parseError.ErrorCode < 0 Then
            RaiseError("FindAFDAddress", "Error: " & xmlDoc.parseError.reason & vbCrLf & "Microsoft.XMLDOM Error Code: " & Str(xmlDoc.parseError.ErrorCode), gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Check if PCE returned an error and if the document is valid
        root = xmlDoc.documentElement
        dataNode = root.selectSingleNode("Result")
        itemNodes = root.selectNodes("Item")

        If dataNode Is Nothing Or itemNodes Is Nothing Then
            RaiseError("FindAFDAddress", "Invalid PCE XML Document", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Val(dataNode.Text) < 1 Then
            dataNode = root.selectSingleNode("ErrorText")
            If dataNode Is Nothing Then
                RaiseError("FindAFDAddress", "Invalid PCE XML Document", gPMConstants.PMELogLevel.PMLogError)
            Else
                RaiseError("FindAFDAddress", dataNode.Text, gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        ReDim r_vKeyArray(0, itemNodes.length - 1)

        lCount = 0
        For Each dataNode In itemNodes
            listNode = dataNode.selectSingleNode("List")
            r_vKeyArray(0, lCount) = dataNode.selectSingleNode("Key").Text
            lCount = lCount + 1
        Next


    End Function
    Public Function GetAFDDetails( _
        ByVal v_sAddressKey As String, _
        Optional ByRef r_sPostCode As String = "", _
        Optional ByRef r_sAddressLine1 As String = "", Optional ByRef r_sAddressLine2 As String = "", _
        Optional ByRef r_sAddressLine3 As String = "", Optional ByRef r_sAddressLine4 As String = "", _
        Optional ByRef r_sCountry As String = "") As Long

        Const kMethodName As String = "GetAFDDetails"

        Dim xmlDoc As Object
        Dim root As Object
        Dim pcFromNode As Object
        Dim dataNode As Object
        Dim itemNodes As Object
        Dim xmlLocation As String

        Try

            GetAFDDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the Microsoft XML Document Object Model
            xmlDoc = CreateObject("Microsoft.XMLDOM")
            xmlDoc.async = False

            ' Build up the XML query string
            xmlLocation = m_sAFDServiceURL + "/afddata.pce?"
            xmlLocation = xmlLocation + "Serial=" + m_sAFDSerial + "&"
            xmlLocation = xmlLocation + "Password=" + m_sAFDPassword + "&"
            xmlLocation = xmlLocation + "UserID=" + m_sAFDUserID + "&"
            xmlLocation = xmlLocation + "Data=Address&Task=Retrieve&Fields=Standard"

            ' Set XML parameter to retrieve the selected record
            xmlLocation = xmlLocation + "&Key=" + v_sAddressKey

            ' Load the XML from the webserver with the query string
            xmlDoc.Load(xmlLocation)

            ' Check for any XML Parser error
            If xmlDoc.parseError.ErrorCode < 0 Then
                RaiseError(kMethodName, "Error: " & xmlDoc.parseError.reason & vbCrLf & "Microsoft.XMLDOM Error Code: " & Str(xmlDoc.parseError.ErrorCode), gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Check if PCE returned an error and if the document is valid
            root = xmlDoc.documentElement
            dataNode = root.selectSingleNode("Result")
            itemNodes = root.selectNodes("Item")
            If dataNode Is Nothing Or itemNodes Is Nothing Then
                RaiseError(kMethodName, "Invalid PCE XML Document", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Val(dataNode.Text) < 1 Then
                dataNode = root.selectSingleNode("ErrorText")
                If dataNode Is Nothing Then
                    RaiseError(kMethodName, "Invalid PCE XML Document", gPMConstants.PMELogLevel.PMLogError)
                Else
                    RaiseError(kMethodName, dataNode.Text, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' Now Assign required fields to your application (only one will be returned)
            dataNode = itemNodes(0).selectSingleNode("Country")
            If Not (dataNode Is Nothing) Then r_sCountry = dataNode.Text
            dataNode = itemNodes(0).selectSingleNode("Property")
            If Not (dataNode Is Nothing) Then r_sAddressLine1 = dataNode.Text

            If ToSafeString(r_sAddressLine1) <> "" Then
                r_sAddressLine1 = r_sAddressLine1 & ", "
            End If

            dataNode = itemNodes(0).selectSingleNode("Street")
            If Not (dataNode Is Nothing) Then r_sAddressLine1 = r_sAddressLine1 & dataNode.Text
            dataNode = itemNodes(0).selectSingleNode("Locality")
            If Not (dataNode Is Nothing) Then r_sAddressLine2 = dataNode.Text
            dataNode = itemNodes(0).selectSingleNode("Town")
            If Not (dataNode Is Nothing) Then r_sAddressLine3 = dataNode.Text
            dataNode = itemNodes(0).selectSingleNode("PostalCounty")
            If Not (dataNode Is Nothing) Then r_sAddressLine4 = dataNode.Text
            dataNode = itemNodes(0).selectSingleNode("Postcode")
            If Not (dataNode Is Nothing) Then r_sPostCode = dataNode.Text

        Catch ex As Exception

            GetAFDDetails = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage("Sirius", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Address Details Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAFDDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Finally
        End Try

    End Function



End Class
