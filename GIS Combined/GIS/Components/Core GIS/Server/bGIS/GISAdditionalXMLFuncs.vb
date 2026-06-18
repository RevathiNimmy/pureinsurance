Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices
Imports System.Xml
Imports SSP.Shared

Module GISAdditionalXMLFuncs
    ' ***************************************************************** '
    ' Module Name: GISAdditionalXMLFuncs
    '
    ' Date:  RAG201100
    '
    ' Description: Functions to handle Additional Data Array <--> XML
    '
    ' Edit History:
    ' RFC01/08/01 - Need to convert dates in to year first format so
    ' ***************************************************************** '


    Private Const ACClass As String = "GISAdditionalXMLFuncs"

    Private Const ACValueAttrib As String = "V"
    Private Const ACTypeAttrib As String = "T"
    Private Const ACDataElement As String = "DATA"
    Private Const ACAdditionalDataElement As String = "ADDITIONAL_DATA"


    ' ***************************************************************** '
    '
    ' Name: FormatArrayToXML
    '
    ' Description:  Takes a array and stores it in the XML. This
    '               generic method avoids problems when the array
    '               dimensions change.
    '
    ' History:  23/10/2001 DD - Created.
    '           08/11/2001 DD - Added support for single dimension arrays.
    '
    ' ***************************************************************** '
    Public Function FormatArrayToXML(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_sTagName As String, ByVal v_vArray As Array) As Integer

        Dim result As Integer = 0
        Dim oActionArrayRow, oActionArrayCol As XmlElement

        Dim iType As Integer
        Dim vValue As Object

        'Trap the array dimensions

        Try
            Dim lCheckBound As Integer = v_vArray.GetLowerBound(1)
            Dim bSingleDimension As Boolean = (Informations.Err().Number <> 0)

            Try

                'Handle the version array using a generic method
                If Informations.IsArray(v_vArray) Then
                    If bSingleDimension Then
                        'loop through a single dimension array
                        oActionArrayRow = r_oDocument.CreateElement(v_sTagName)
                        oActionArrayRow.SetAttribute("Dimension", 1)
                        For lCol As Integer = v_vArray.GetLowerBound(0) To v_vArray.GetUpperBound(0)
                            oActionArrayCol = r_oDocument.CreateElement("Col" & lCol)


                            vValue = CStr(v_vArray(lCol))
                            '**** Added By: AAB  -  Added On:  11-Sep-2002 13:01 ****
                            '**** Added to get over the NULL Problem.

                            If Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                                iType = VariantType.String
                            Else
                                iType = (vValue)
                            End If

                            'Store dates in fixed format to avoid regional problems
                            If iType = VariantType.Date Then
                                Dim TempDate3 As Date
                                If (If(DateTime.TryParse(vValue, TempDate3), TempDate3.ToString("HH:mm:ss"), vValue)) <> "00:00:00" Then
                                    Dim TempDate As Date
                                    vValue = If(DateTime.TryParse(vValue, TempDate), TempDate.ToString("yyyy/MM/dd HH:mm:ss"), vValue)
                                Else
                                    Dim TempDate2 As Date
                                    vValue = If(DateTime.TryParse(vValue, TempDate2), TempDate2.ToString("yyyy/MM/dd"), vValue)
                                End If
                            End If

                            oActionArrayCol.SetAttribute(ACTypeAttrib, iType)
                            '**** Added By: AAB  -  Added On:  11-Sep-2002 13:01 ****
                            '**** Added to get over the NULL Problem.
                            oActionArrayCol.InnerText = vValue & ""

                            oActionArrayRow.AppendChild(oActionArrayCol)
                        Next lCol


                        r_oParentElem.AppendChild(oActionArrayRow)
                    Else
                        'loop through a 2D array
                        For lRow As Integer = v_vArray.GetLowerBound(1) To v_vArray.GetUpperBound(1)
                            oActionArrayRow = r_oDocument.CreateElement(v_sTagName)
                            oActionArrayRow.SetAttribute("Dimension", 2)

                            For lCol As Integer = v_vArray.GetLowerBound(0) To v_vArray.GetUpperBound(0)
                                oActionArrayCol = r_oDocument.CreateElement("Col" & lCol)


                                vValue = CStr(v_vArray(lCol, lRow))
                                '**** Added By: AAB  -  Added On:  11-Sep-2002 11:29 ****
                                '**** Added the IF statement to deal with NULL values

                                If Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                                    iType = VariantType.String
                                Else
                                    iType = Informations.VarType(vValue)
                                End If

                                'Store dates in fixed format to avoid regional problems
                                If iType = VariantType.Date Then
                                    Dim TempDate6 As Date
                                    If (If(DateTime.TryParse(vValue, TempDate6), TempDate6.ToString("HH:mm:ss"), vValue)) <> "00:00:00" Then
                                        Dim TempDate4 As Date
                                        vValue = If(DateTime.TryParse(vValue, TempDate4), TempDate4.ToString("yyyy/MM/dd HH:mm:ss"), vValue)
                                    Else
                                        Dim TempDate5 As Date
                                        vValue = If(DateTime.TryParse(vValue, TempDate5), TempDate5.ToString("yyyy/MM/dd"), vValue)
                                    End If
                                End If

                                oActionArrayCol.SetAttribute(ACTypeAttrib, iType)
                                '**** Added the blank to deal with NULL values
                                oActionArrayCol.InnerText = vValue & ""

                                oActionArrayRow.AppendChild(oActionArrayCol)
                            Next lCol

                            r_oParentElem.AppendChild(oActionArrayRow)
                        Next lRow
                    End If

                    oActionArrayCol = Nothing
                    oActionArrayRow = Nothing
                End If

                Return gPMConstants.PMEReturnCode.PMTrue

            Catch excep As System.Exception

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatArrayToXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatArrayToXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End Try

        Catch exc As System.Exception
            ''NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: UnformatXMLtoArray
    '
    ' Description:  Rebuilds an Array from one stored by the
    '               FormatArraytoXML function.
    '
    ' History:  23/10/2001 DD - Created.
    '           08/11/2001 DD - Added support for single dimension arrays.
    '
    ' ***************************************************************** '
    Public Function UnformatXMLtoArray(ByVal v_oParentElem As XmlElement, ByVal v_sTagName As String, ByRef r_vResultArray As Array) As Integer

        Dim result As Integer = 0
        Dim oMatches As XmlNodeList
        Dim oActionArrayRow, oActionArrayCol As XmlElement
        Dim lRows, lCols As Integer
        Dim vValue As Object
        Dim iType As Integer
        Dim bSingleDimension As Boolean

        Try

            oMatches = v_oParentElem.GetElementsByTagName(v_sTagName)
            If Not (oMatches Is Nothing) Then

                ' How Many Matches are there
                lRows = oMatches.Count

                ' If there are matches then
                If lRows > 0 Then
                    ' Determine the dimensions
                    oActionArrayRow = oMatches.Item(0)

                    bSingleDimension = (CDbl(oActionArrayRow.GetAttribute("Dimension")) = 1)

                    ' Now work out the other dimension
                    lCols = oActionArrayRow.ChildNodes.Count

                    If bSingleDimension Then
                        ' Resize the Array to hold the results
                        r_vResultArray = Array.CreateInstance(GetType(Object), New Integer() {lCols}, New Integer() {0})

                        ' Now move through the XML filling the array
                        oActionArrayCol = oActionArrayRow.FirstChild
                        For lCol As Integer = 0 To lCols - 1



                            r_vResultArray(lCol) = ConvertToType(CStr(oActionArrayCol.GetAttribute(ACTypeAttrib)), oActionArrayCol.InnerText)
                            If Not (oActionArrayCol.NextSibling Is Nothing) Then
                                oActionArrayCol = oActionArrayCol.NextSibling
                            End If
                        Next lCol
                    Else

                        ' Resize the Array to hold the results
                        r_vResultArray = Array.CreateInstance(GetType(Object), New Integer() {lCols, lRows}, New Integer() {0, 0})

                        ' Now move through the XML filling the array
                        For lRow As Integer = 0 To lRows - 1
                            oActionArrayRow = oMatches.Item(lRow)
                            oActionArrayCol = oActionArrayRow.FirstChild
                            For lCol As Integer = 0 To lCols - 1



                                r_vResultArray(lCol, lRow) = ConvertToType(CStr(oActionArrayCol.GetAttribute(ACTypeAttrib)), oActionArrayCol.InnerText)
                                If Not (oActionArrayCol.NextSibling Is Nothing) Then
                                    oActionArrayCol = oActionArrayCol.NextSibling
                                End If
                            Next lCol
                        Next lRow
                    End If
                End If

                oActionArrayCol = Nothing
                oActionArrayRow = Nothing
                oMatches = Nothing
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnformatXMLtoArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnformatXMLtoArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: FormatAdditionalDataXML
    '
    ' Description: Takes a Name/Value array and stores it in the XML
    '
    ' History: 02/10/2000 RFC - Created.
    '          20/11/2000 RAG - Moved to GISAdditionalXMLFuncs.bas
    '
    ' ***************************************************************** '
    Public Function FormatAdditionalDataXML(ByRef r_oDocument As XmlDocument, ByRef r_oParentElem As XmlElement, ByVal v_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lFrom, lTo As Integer

        Dim sName As String = ""

        Dim vValue As Object
        Dim iType As VariantType

        Dim oAddData, oData As XmlElement

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we Have an Array
            If Not Informations.IsArray(v_vAdditionalDataArray) Then
                Return result
            End If

            ' Get the Row From/To
            lFrom = v_vAdditionalDataArray.GetLowerBound(1)
            lTo = v_vAdditionalDataArray.GetUpperBound(1)

            ' Create the Additional Data Element
            oAddData = r_oDocument.CreateElement(ACAdditionalDataElement)

            ' Append the Add Data element to the Parent

            r_oParentElem.AppendChild(oAddData)

            ' Add each Data Item
            For lRow As Integer = lFrom To lTo


                sName = CStr(v_vAdditionalDataArray(0, lRow))
                ' RDC 02082001 vValue needs to be retrieved before it can be tested START
                '        iType = VarType(vValue)
                ' RFC01/08/01 - Need to convert dates in to year first format so
                ' that we do not get day/month transpostion problems with us/uk servers
                '        If (iType = vbDate) Then
                '            vValue = Format$(v_vAdditionalDataArray(1, lRow), "yyyy/mm/dd")
                '        Else
                '            vValue = v_vAdditionalDataArray(1, lRow)
                '        End If


                vValue = CStr(v_vAdditionalDataArray(1, lRow))

                iType = Informations.VarType(vValue)

                ' RFC01/08/01 - Need to convert dates in to year first format so
                ' that we do not get day/month transpostion problems with us/uk servers
                If iType = VariantType.Date Then
                    Dim TempDate As Date
                    vValue = If(DateTime.TryParse(vValue, TempDate), TempDate.ToString("yyyy/MM/dd"), vValue)
                End If
                ' RDC 02082001 vValue needs to be retrieved before it can be tested END

                ' Create a Data Element
                oData = r_oDocument.CreateElement(ACDataElement)

                ' Set text to be the Name
                oData.InnerText = sName.Trim()

                ' Set the Type Attrib
                oData.SetAttribute(ACTypeAttrib, iType)

                ' Set the Value Attrib
                oData.SetAttribute(ACValueAttrib, vValue)

                ' Append it to the Add Data Element

                oAddData.AppendChild(oData)

                oData = Nothing

            Next lRow

            oData = Nothing
            oAddData = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatAdditionalDataXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatAdditionalDataXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UnFormatAdditionalDataXML
    '
    ' Description: Rebuilds the Name/Value array from the XML
    '
    ' History: 02/10/2000 RFC - Created.
    '          20/11/2000 RAG - Moved to GISAdditionalXMLFuncs.bas
    '
    ' ***************************************************************** '
    Public Function UnFormatAdditionalDataXML(ByRef r_oDocument As XmlDocument, ByRef r_vAdditionalDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRow, lNum As Integer

        Dim sName As String = ""
        Dim vValue As Object
        Dim sType As String = ""

        Dim oAddData, oData As XmlElement
        Dim oNodes As XmlNodeList

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have any Additional Data
            oNodes = r_oDocument.GetElementsByTagName(ACAdditionalDataElement)

            If oNodes Is Nothing Then
                Return result
            End If


            '**** Added By: AAB  -  Added On:  04-Sep-2002 14:51 ****
            '**** Commented this line out was causing an error.
            'r_vAdditionalDataArray = ""

            ' Get the Add Data Elem
            oAddData = oNodes.Item(0)

            If oAddData Is Nothing Then
                Return result
            End If

            ' Get the Number of State Value
            lNum = oAddData.ChildNodes.Count

            ' If there aren't any entries then exit
            If lNum < 1 Then
                Return result
            End If

            ReDim r_vAdditionalDataArray(1, lNum - 1)

            lRow = 0

            oData = oAddData.FirstChild

            Do While Not (oData Is Nothing)

                sName = oData.InnerText


                vValue = oData.GetAttribute(ACValueAttrib)

                sType = CStr(oData.GetAttribute(ACTypeAttrib))



                vValue = ConvertToType(sType, vValue)


                r_vAdditionalDataArray(0, lRow) = sName


                r_vAdditionalDataArray(1, lRow) = vValue
                oData = oData.NextSibling

                lRow += 1

            Loop

            oData = Nothing
            oAddData = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnFormatAdditionalDataXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnFormatAdditionalDataXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ConvertToType
    '
    ' Description: Converts the variant from a string to its proper type.
    '
    ' History: RFC210900 - Created
    '          20/11/2000 RAG - Moved to GISAdditionalXMLFuncs.bas
    '          06/11/2001 RFC - Changed to public, so it can be called by ActionXMLFuncsGeneric
    ' ***************************************************************** '
    Public Function ConvertToType(ByVal v_sVarType As String, ByRef v_vValue As Object) As Object

        Try

            ' Return the Value, typed accordingly

            Select Case v_sVarType
                Case CStr(VariantType.String)

                    Return CStr(v_vValue)
                Case CStr(VariantType.Date)

                    Return CDate(v_vValue)
                Case CStr(VariantType.Decimal)

                    Return CDec(v_vValue)
                Case CStr(VariantType.Decimal)

                    Return CDec(v_vValue)
                Case CStr(VariantType.Boolean)

                    Return CBool(v_vValue)
                Case CStr(VariantType.Short), CStr(VariantType.Integer)

                    Return CInt(v_vValue)
                Case CStr(VariantType.Single)

                    Return CSng(v_vValue)
                Case CStr(VariantType.Double)

                    Return CDbl(v_vValue)

                Case CStr(VariantType.Object)

                    Return CObj(v_vValue)
                Case CStr(VariantType.Empty)
                    Return Nothing
                Case CStr(VariantType.Null)

                    Return DBNull.Value
                Case Else
                    Return v_vValue
            End Select

        Catch
        End Try



        Throw New System.Exception(Informations.Err().Number.ToString() + ", " + Informations.Err().Source + ", " + Informations.Err().Description)

        Exit Function


    End Function


    ' ***************************************************************** '
    '
    ' Name: bz
    '
    ' Description: Blank to Zero function - converts a blank to a zero
    '
    ' History: 09/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function bz(ByVal v_vValue As Byte, Optional ByVal v_vDefault As Byte = 0) As Byte

        Try

            If Marshal.SizeOf(v_vValue) = 0 Then
                Return v_vDefault
            Else
                Return v_vValue
            End If

        Catch exc As System.Exception
            ''NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return v_vDefault
    End Function
End Module

