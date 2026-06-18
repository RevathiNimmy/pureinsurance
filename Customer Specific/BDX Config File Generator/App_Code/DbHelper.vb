Option Explicit On
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports System.Xml.XPath
Imports System.IO
Imports System.Text
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Drawing
Imports Microsoft.Office.Interop.Excel

Public Class DbHelper

    Public Function GetProductCode() As System.Data.DataTable
        Dim command As SqlCommand = Nothing
        Dim connectionObj As SqlConnection = Nothing
        Dim adapter As SqlDataAdapter = Nothing
        Dim dtProductCode As System.Data.DataTable = Nothing
        Try
            adapter = New SqlDataAdapter
            dtProductCode = New System.Data.DataTable
            connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
            connectionObj.Open()
            command = New SqlCommand("dbo.spu_XMLGenerator_GetProduct")
            command.Connection = connectionObj
            command.CommandType = CommandType.StoredProcedure
            adapter.SelectCommand = command

            adapter.Fill(dtProductCode)

            Return dtProductCode
        Finally
            command.Dispose()
            If (connectionObj.State = ConnectionState.Open) Then
                connectionObj.Close()
                connectionObj.Dispose()
            End If

        End Try
    End Function



    Public Function GetRiskCode(ByVal Productid As String) As System.Data.DataTable
        Dim command As SqlCommand = Nothing
        Dim connectionObj As SqlConnection = Nothing
        Dim adapter As SqlDataAdapter = Nothing
        Dim dtRiskCode As System.Data.DataTable = Nothing
        Try
            adapter = New SqlDataAdapter
            dtRiskCode = New System.Data.DataTable
            connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
            connectionObj.Open()
            command = New SqlCommand("dbo.spu_XMLGenerator_GetRiskCode")
            command.Connection = connectionObj
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.Add("@Productid", SqlDbType.Int, 20).Value = Productid
            adapter.SelectCommand = command

            adapter.Fill(dtRiskCode)
            dtRiskCode.Rows.Add("ALL")
            Return dtRiskCode
        Finally
            command.Dispose()
            If (connectionObj.State = ConnectionState.Open) Then
                connectionObj.Close()
                connectionObj.Dispose()
            End If

        End Try
    End Function

    Public Function GetScreenDetails(ByVal RiskTypeId As String) As System.Data.DataTable
        If RiskTypeId = "" Then
            RiskTypeId = "0"
        End If
        Dim command As SqlCommand = Nothing
        Dim connectionObj As SqlConnection = Nothing
        Dim adapter As SqlDataAdapter = Nothing
        Dim dtScreenDetails As System.Data.DataTable = Nothing
        Try
            adapter = New SqlDataAdapter
            dtScreenDetails = New System.Data.DataTable
            connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
            connectionObj.Open()
            command = New SqlCommand("dbo.spu_XMLGenerator_GetScreenDetails")
            command.Connection = connectionObj
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.Add("@RiskTypeId", SqlDbType.Int, 20).Value = RiskTypeId
            adapter.SelectCommand = command

            adapter.Fill(dtScreenDetails)

            Return dtScreenDetails
        Finally
            command.Dispose()
            If (connectionObj.State = ConnectionState.Open) Then
                connectionObj.Close()
                connectionObj.Dispose()
            End If

        End Try
    End Function

    Public Function GetPropertyDetailsInObject(ByVal dtScreenDetails As System.Data.DataTable) As String(,)
        Dim childNode As Integer = 1
        Dim indx As Integer = 0
        Dim oPropertyDetails_temp(indx, 5) As String
        Dim oPropertyDetails_temp_1(indx, 5) As String
        Dim count As Integer = 0
        Dim countrow As Integer = 0
        Dim loopCnt As Boolean = True
        While loopCnt
            Description_temp = dtScreenDetails.Rows(countrow)(0).ToString
            oPropertyDetails_temp(count, 1) = GetDataTypeValue(Integer.Parse(dtScreenDetails.Rows(countrow)(3).ToString), Integer.Parse(dtScreenDetails.Rows(countrow)(6).ToString))
            oPropertyDetails_temp(count, 2) = If(oPropertyDetails_temp(count, 1).ToString() = "String", Char6, "")
            oPropertyDetails_temp(count, 3) = "false"
            oPropertyDetails_temp(count, 4) = GetDestMappingValue(dtScreenDetails.Rows(countrow)(1).ToString(), dtScreenDetails.Rows(countrow)(2).ToString(), Integer.Parse(dtScreenDetails.Rows(countrow)(5).ToString()), Description_temp, childNode)
            oPropertyDetails_temp(count, 5) = dtScreenDetails.Rows(countrow)(2).ToString()

            Description_temp = Description_temp.Replace("&", " and ")
            Description_temp = Description_temp.Replace("<strong>", "")
            Description_temp = Description_temp.Replace("</strong>", "")
            If childNode = 1 Then
                oPropertyDetails_temp(count, 0) = Description_temp
            Else
                If childNode = 0 Then
                    oPropertyDetails_temp(count, 0) = Description_temp + "_3"
                    childNode = 1
                Else
                    oPropertyDetails_temp(count, 0) = Description_temp + "_" + (childNode - 1).ToString()
                    countrow = countrow - 1
                End If

            End If
            countrow = countrow + 1
            count = count + 1
            If countrow = dtScreenDetails.Rows.Count() Then
                loopCnt = False
                Exit While
            End If
            oPropertyDetails_temp_1 = oPropertyDetails_temp
            indx = indx + 1
            ReDim oPropertyDetails_temp(indx, 5)
            Dim lpropertylength As Integer = oPropertyDetails_temp_1.Length / 6
            For countar As Integer = 0 To lpropertylength - 1
                oPropertyDetails_temp(countar, 0) = oPropertyDetails_temp_1(countar, 0)
                oPropertyDetails_temp(countar, 1) = oPropertyDetails_temp_1(countar, 1)
                oPropertyDetails_temp(countar, 2) = oPropertyDetails_temp_1(countar, 2)
                oPropertyDetails_temp(countar, 3) = oPropertyDetails_temp_1(countar, 3)
                oPropertyDetails_temp(countar, 4) = oPropertyDetails_temp_1(countar, 4)
                oPropertyDetails_temp(countar, 5) = oPropertyDetails_temp_1(countar, 5)
            Next
            ReDim oPropertyDetails_temp_1(indx, 5)
           
        End While



        Return oPropertyDetails_temp
        
    End Function
    Enum DataType
        ListType = 2
        IntegerType1 = 20
        IntegerType2 = 23
        CurrencyType = 21
        DateType = 1
        StringType1 = 7
        StringType2 = 5
        PercentType = 22
    End Enum
    Public Function GetDataTypeValue(ByVal DataTypeId As Integer, ByVal SpecialDataTypeId As Integer) As String
        Select Case DataTypeId
            Case DataType.ListType
                If SpecialDataTypeId = 2 Then
                    Return "List"
                Else
                    Return "Integer"
                End If
            Case DataType.IntegerType1, DataType.IntegerType2
                Return "Integer"
            Case DataType.CurrencyType
                Return "Currency"
            Case DataType.DateType
                Return "Date"
            Case DataType.StringType1, DataType.StringType2
                Return "String"
            Case DataType.PercentType
                Return "Percent"
            Case Else
                Return "String"
        End Select
    End Function

    Public Function GetDestMappingValue(ByVal sObjectName As String, ByVal sPropertyName As String, ByVal sGisScreenId As Integer, ByRef Description As String, ByRef nChieldCount As Integer) As String
        Dim lProperty As List(Of Object) = New List(Of Object)
        Dim sTempPropertyId As Integer = 0
        Dim sGisScreenId_Temp As Integer = sGisScreenId
        Dim sRetrurnString As String

        lProperty.Add(sPropertyName)
        lProperty.Add(sObjectName)
        While sTempPropertyId <> -1
            Dim command As SqlCommand = Nothing
            Dim connectionObj As SqlConnection = Nothing
            Dim adapter As SqlDataAdapter = Nothing
            Dim dtScreenDetails As System.Data.DataTable = Nothing
            Try
                adapter = New SqlDataAdapter
                dtScreenDetails = New System.Data.DataTable
                connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
                connectionObj.Open()
                command = New SqlCommand("dbo.spu_XMLGenerator_GetScreenCode")
                command.Connection = connectionObj
                command.CommandType = CommandType.StoredProcedure
                command.Parameters.Add("@ObjectName", SqlDbType.VarChar).Value = sObjectName
                command.Parameters.Add("@ScreenId", SqlDbType.VarChar).Value = sGisScreenId_Temp
                adapter.SelectCommand = command
                adapter.Fill(dtScreenDetails)
            Finally
                command.Dispose()
                If (connectionObj.State = ConnectionState.Open) Then
                    connectionObj.Close()
                    connectionObj.Dispose()
                End If
            End Try
            If dtScreenDetails.Rows(0)(0).ToString <> "" Then
                lProperty.Add(Trim(dtScreenDetails.Rows(0)(0).ToString()))
            End If
            If dtScreenDetails.Rows(0)(1).ToString <> "" Then
                sObjectName = dtScreenDetails.Rows(0)(0).ToString()
            Else
                ' lProperty.Add(Trim(dtScreenDetails.Rows(0)(0).ToString()))
                Exit While
            End If
        End While
        sRetrurnString = Char1
        sRetrurnString = sRetrurnString + "/" + Trim(lProperty(lProperty.Count - 1)).ToUpper
        Dim nChieldCount_temp As Integer = 0
        For count As Integer = lProperty.Count - 2 To 1 Step -1
            If nChieldCount_temp = 0 Then
                sRetrurnString = sRetrurnString + "/" + lProperty(count)
                nChieldCount_temp = 1

            Else
                sRetrurnString = sRetrurnString + "/" + lProperty(count) + "[" + nChieldCount.ToString() + "]"
                nChieldCount = nChieldCount + 1
            End If
            If nChieldCount > 3 Then
                nChieldCount = 0
            End If
        Next
        sRetrurnString = sRetrurnString + "/@" + lProperty(0)
        If Trim(Description) = "" OrElse Trim(Description).ToUpper = "[BLANK]" Then
            Description = lProperty(0).ToString()
            If Not lProperty(0).ToString().Contains("CLAUSES") AndAlso Not lProperty(0).ToString().Contains("Endorsement") Then
                Description = GetCaption(lProperty(0).ToString())
            End If
        End If
        
        Return sRetrurnString
    End Function
    Public Function GetCaption(ByVal PropertyName As String) As String
        Dim command As SqlCommand = Nothing
        Dim connectionObj As SqlConnection = Nothing
        Dim adapter As SqlDataAdapter = Nothing
        Dim dtRiskCode As System.Data.DataTable = Nothing
        Try
            adapter = New SqlDataAdapter
            dtRiskCode = New System.Data.DataTable
            connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
            connectionObj.Open()
            command = New SqlCommand("dbo.spu_XMLGenerator_GetCaptionProperty")
            command.Connection = connectionObj
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.Add("@PropertyName", SqlDbType.VarChar).Value = PropertyName
            adapter.SelectCommand = command

            adapter.Fill(dtRiskCode)
            If dtRiskCode.Rows.Count > 0 Then
                Return (dtRiskCode.Rows(0)(0).ToString().Trim + "_" + PropertyName)
            Else
                Return PropertyName
            End If
        Catch
            Return PropertyName
        Finally
            command.Dispose()
            If (connectionObj.State = ConnectionState.Open) Then
                connectionObj.Close()
                connectionObj.Dispose()
            End If

        End Try
    End Function
    Public Char1 As String = "[PB]/DATA_SET/RISK_OBJECTS"
    Public Char2 As String = "<Field SourceValue=" + Convert.ToChar(34)
    Public Char3 As String = Convert.ToChar(34) + " Description=" + Convert.ToChar(34)
    Public Char4 As String = Convert.ToChar(34) + " Datatype=" + Convert.ToChar(34)
    Public Char5 As String = Convert.ToChar(34)
    Public Char6 As String = " Length = " + Convert.ToChar(34) + "255" + Convert.ToChar(34)
    Public Char7 As String = " Required=" + Convert.ToChar(34) + "false" + Convert.ToChar(34) + " DestMapping=" + Convert.ToChar(34)
    Public Char8 As String = Convert.ToChar(34) + " />"
    Public Char9 As String = Convert.ToChar(34) + " >"
    Public Char10 As String = "<Validation>"
    Public Char11 As String = "<List>"
    Public Char12 As String = "<Item SourceValue=" + Convert.ToChar(34)
    Public Char13 As String = Convert.ToChar(34) + " DestValue=" + Convert.ToChar(34)
    Public Char14 As String = "</Validation>"
    Public Char15 As String = "</List>"
    Public Char16 As String = "</Field>"
    Dim Description_temp As String = ""
    Public lXMLDataOut As List(Of Object) = New List(Of Object)
    Public Function GenerateMappingXML(ByVal oPropertyDetails(,) As String) As List(Of Object)
        Dim command As SqlCommand = Nothing
        Dim connectionObj As SqlConnection = Nothing
        Dim adapter As SqlDataAdapter = Nothing
        Dim dtUDLDetails As System.Data.DataTable = Nothing

        Dim sStr As String = ""


        For count As Integer = oPropertyDetails.GetLowerBound(0) To oPropertyDetails.GetUpperBound(0)
            If Not oPropertyDetails(count, 0).ToString().Contains("CLAUSES") AndAlso Not oPropertyDetails(count, 0).ToString().Contains("Endorsement") Then
                sStr = Char2 + "$" + Char3 + oPropertyDetails(count, 0).ToString() + Char4 + oPropertyDetails(count, 1).ToString() + Char5
                If oPropertyDetails(count, 1).ToString() = "String" Then
                    sStr = sStr + Char6
                End If
                sStr = sStr + Char7 + oPropertyDetails(count, 4).ToString()

                If oPropertyDetails(count, 1).ToString() = "List" Then

                    Try
                        adapter = New SqlDataAdapter
                        dtUDLDetails = New System.Data.DataTable
                        connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
                        connectionObj.Open()
                        command = New SqlCommand("dbo.spu_XMLGenerator_GetPropertyUDLData")
                        command.Connection = connectionObj
                        command.CommandType = CommandType.StoredProcedure
                        command.Parameters.Add("@PropertyName", SqlDbType.VarChar).Value = oPropertyDetails(count, 5).ToString()
                        adapter.SelectCommand = command
                        adapter.Fill(dtUDLDetails)
                    Finally
                        command.Dispose()
                        If (connectionObj.State = ConnectionState.Open) Then
                            connectionObj.Close()
                            connectionObj.Dispose()
                        End If
                    End Try
                    If dtUDLDetails.Rows.Count > 0 Then
                        sStr = sStr + Char9 + vbNewLine + Char10 + vbNewLine + Char11
                    Else
                        sStr = sStr + Char8
                        sStr = sStr.Replace(Convert.ToChar(34) + "List" + Convert.ToChar(34), Convert.ToChar(34) + "Integer" + Convert.ToChar(34))
                    End If

                    For count1 As Integer = 0 To dtUDLDetails.Rows.Count - 1
                        sStr = sStr + vbNewLine + Char12 + (dtUDLDetails.Rows(count1)(1).ToString()).Trim + Char13 + dtUDLDetails.Rows(count1)(0).ToString() + Char8
                    Next
                    If dtUDLDetails.Rows.Count > 0 Then
                        sStr = sStr + vbNewLine + Char15 + vbNewLine + Char14 + vbNewLine + Char16
                    End If
                Else
                    sStr = sStr + Char8
                End If
                sStr = sStr.Replace("&", " and ")
                sStr = sStr.Replace("<strong>", "")
                sStr = sStr.Replace("</strong>", "")
                lXMLDataOut.Add(sStr)
            End If
        Next

        Return lXMLDataOut
    End Function
    Public AlphabetArray() As String
    Public Sub ExcelSheet_AlphabetArray_Generation()
        Dim DigitToTest As String
        Dim Permut As New List(Of String)
        Dim WorkingArray() As Char
        DigitToTest = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Permut = New List(Of String)()
        WorkingArray = DigitToTest.ToCharArray
        For x As Integer = 0 To WorkingArray.Length - 1
            Permut.Add(WorkingArray(x))
        Next
        GetPermut(Permut, DigitToTest, 1, DigitToTest.Length + 1, 3)
        AlphabetArray = Permut.ToArray
    End Sub


    Private Shared Sub GetPermut(ByRef Permut As List(Of String), ByVal DigitsToTest As String, ByVal pos As Integer, Optional ByVal Max As Integer = 4, Optional ByVal count As Integer = 0)
        Dim WorkingArray() As Char = DigitsToTest.ToCharArray
        Dim St As String = ""
        Dim Ch As Char
        Dim levl As Integer
        Dim dd As Integer = Permut(Permut.Count - 1).Length
        For x As Integer = 0 To Permut.Count - 1
            Ch = CChar(Permut(x).Substring(Permut(x).Length - 1))
            levl = DigitsToTest.IndexOf(Ch)
            For y As Integer = 0 To WorkingArray.Length - 1
                'If Not (Permut(x).Contains(WorkingArray(y))) Then
                St = Permut(x) & WorkingArray(y)
                If St.Length > dd Then
                    If St.Length <= count Then
                        Permut.Add(St)
                    End If
                    If St.ToString() = "XFD" Then
                        count = 0
                    End If

                End If
                'End If
            Next
        Next
        pos += 1
        If pos < Max Then
            GetPermut(Permut, DigitsToTest, pos, Max, count)
        End If
    End Sub

    Public lXMLData As List(Of Object) = New List(Of Object)
    Private m_vFileArray() As Object

    Public Function ExcelSheet_MappedFields(ByVal path As String, ByVal startColumn As String, ByVal mappedRowNo As Integer, ByVal sheetName As String, excelpath As String) As Integer
        Dim SplitData() As String
        Dim Excel_Index As Integer
        startColumn = (Trim(startColumn)).ToUpper
        If Not AlphabetArray.Contains(startColumn) Then
            Return 0
        Else
            Excel_Index = Array.IndexOf(AlphabetArray, startColumn) - 1

            Dim FILE_NAME As String = path

            Dim TextLine As String

            If System.IO.File.Exists(FILE_NAME) = True Then
                Dim objReader As New System.IO.StreamReader(FILE_NAME)
                Do While objReader.Peek() <> -1
                    TextLine = TextLine & objReader.ReadLine() & vbNewLine
                Loop
            End If


            BreakStringIntoArray("<Field", "</Field>", TextLine, m_vFileArray, True)
            Dim strxml As String = 0
            Dim cmpText As String = ""
            For counttemp As Integer = 0 To m_vFileArray.Length - 1
                If (m_vFileArray(counttemp)).Trim <> "" Then
                    Excel_Index = Excel_Index + 1
                    strxml = m_vFileArray(counttemp).ToString()
                    Dim nEndIndx As Integer = strxml.IndexOf("/", strxml.IndexOf("RISK_OBJECTS") + 13)
                    nEndIndx = nEndIndx - (strxml.IndexOf("RISK_OBJECTS") + 13)
                    Dim ntxtstrt As String = strxml.Substring(strxml.IndexOf("RISK_OBJECTS") + 13, nEndIndx)
                    If cmpText <> ntxtstrt Then

                        Excel_Index = Excel_Index + 5
                        cmpText = ntxtstrt
                    End If
                    m_vFileArray(counttemp) = strxml.Replace("$", "$" + AlphabetArray(Excel_Index))
                End If
            Next
            EditEmpDetails(excelpath, mappedRowNo, Array.IndexOf(AlphabetArray, startColumn), sheetName, m_vFileArray)
        End If
        path = path.Replace(".", "_updated.")
        Dim pathxml As String = path
        ' Create or overwrite the file.
        Dim fs As FileStream = File.Create(pathxml)
        For count1 As Integer = 0 To m_vFileArray.Count - 1
            ' Add text to the file.
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(m_vFileArray(count1).ToString() + vbNewLine)
            fs.Write(info, 0, info.Length)
        Next
        fs.Close()
        Return 1
    End Function

    Public Function BreakStringIntoArray(ByVal v_sStartTag As String, _
                                       ByVal v_sEndTag As String, _
                                       ByRef r_sString As String, ByRef r_vArray As Object, _
                                       Optional ByVal v_bFormatString As Boolean = False) As Integer
        Dim sSTR() As String
        Dim lCnt As Long
        Dim sTmpLine As String
        Dim sEndFragment As String

        Dim lPos As Integer
        Dim result As Integer = 1

        v_sStartTag = GetTagProperName(v_sStartTag, r_sString)

        sSTR = Split(r_sString, v_sStartTag)

        For lCnt = 0 To UBound(sSTR)

            If lCnt = 0 Then
                If Len(sSTR(lCnt)) > 0 Then
                    SetValueToArray(r_vArray, sSTR(lCnt), v_bFormatString)
                End If
            Else
                lPos = InStr(1, sSTR(lCnt), v_sEndTag)

                If lPos > 0 Then
                    sTmpLine = Left$(sSTR(lCnt), lPos - 1 + Len(v_sEndTag))
                    sEndFragment = Mid$(sSTR(lCnt), lPos + Len(v_sEndTag))

                    SetValueToArray(r_vArray, v_sStartTag & sTmpLine, v_bFormatString)

                    If Len(sEndFragment) > 0 Then
                        SetValueToArray(r_vArray, sEndFragment, v_bFormatString)
                    End If

                Else
                    SetValueToArray(r_vArray, v_sStartTag & sSTR(lCnt), v_bFormatString)
                End If

            End If

        Next

        Return result

    End Function
    Public Function SetValueToArray(ByRef r_vArray As Object, ByVal v_vValue As Object, _
                               Optional ByVal v_bForamtString As Boolean = False) As Integer

        Dim lReturn As Long
        Dim result As Integer = 1


        SetArrayIndexWithPreserve(r_vArray)

        If v_bForamtString Then


            r_vArray(UBound(r_vArray)) = v_vValue
        Else
            r_vArray(UBound(r_vArray)) = v_vValue
        End If

        Return result

    End Function
    Public Function GetTagProperName(ByVal v_sTag As String, ByRef r_sString As String) As String
        Dim lPos As Integer
        Dim sChar As String

        lPos = 1

        GetTagProperName = ""

        Do While True
            lPos = InStr(lPos, r_sString, v_sTag)

            If lPos > 0 Then
                sChar = Mid$(r_sString, lPos + Len(v_sTag), 1)

                If sChar = ">" Or sChar = " " Then
                    GetTagProperName = v_sTag & sChar
                    Exit Do
                End If
            Else
                Exit Do
            End If

            lPos = lPos + 1
        Loop
    End Function
    Public Function SetArrayIndexWithPreserve(ByRef r_vArray As Object) As Integer

        Dim result As Integer = 1

        If IsArray(r_vArray) Then
            ReDim Preserve r_vArray(UBound(r_vArray) + 1)
        Else
            ReDim r_vArray(0)
        End If

        Return result
    End Function
   
    Private Sub EditEmpDetails(ByVal sFile As String, ByVal row As Integer, ByVal column As Integer, ByVal sheetname As String, ByVal strData() As Object)
        Dim xl As New Excel.Application
        Dim wb As Excel.Workbook
        Dim st As Excel.Worksheet

        wb = xl.Workbooks.Open(sFile)
        st = wb.Worksheets(1)
        Dim strxml As String = ""
        Dim cmpText As String = ""
        Dim colorchange As Integer = 0
        Dim style As Excel.Style
        Try
            style = wb.Styles.Add("NewStyle")
        Catch ex As Exception
            style = wb.Styles("NewStyle")
        End Try
        style.WrapText = True
        For counttemp As Integer = 0 To strData.Length - 1
            If (strData(counttemp)).Trim <> "" Then
                strxml = strData(counttemp).ToString()
                Dim nEndIndx As Integer = strxml.IndexOf(Convert.ToChar(34), strxml.IndexOf("Description=") + 13)
                nEndIndx = nEndIndx - (strxml.IndexOf("Description=") + 13)
                Dim ntxtstrt As String = strxml.Substring(strxml.IndexOf("Description=") + 13, nEndIndx)

                Dim nEndIndx_1 As Integer = strxml.IndexOf("/", strxml.IndexOf("RISK_OBJECTS") + 13)
                nEndIndx_1 = nEndIndx_1 - (strxml.IndexOf("RISK_OBJECTS") + 13)
                Dim ntxtstrt_1 As String = strxml.Substring(strxml.IndexOf("RISK_OBJECTS") + 13, nEndIndx_1)
                If cmpText <> ntxtstrt_1 Then
                    column = column + 5
                    cmpText = ntxtstrt_1
                    st.Range(AlphabetArray(column - 1).ToString() + row.ToString).Value = GetRiskDescription(ntxtstrt_1.Replace("_POLICY_BINDER", ""))
                    If colorchange Mod 2 = 0 Then
                        style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange)
                        colorchange = colorchange + 1
                    Else
                        style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow)
                        colorchange = colorchange + 1
                    End If
                End If
                st.Range(AlphabetArray(column).ToString() + row.ToString).Value = ntxtstrt
                st.Range(AlphabetArray(column).ToString() + row.ToString).Style = "NewStyle"

                column = column + 1
            End If
        Next

        wb.Save()
        xl.Quit()
        xl = Nothing
        wb = Nothing
        st = Nothing
    End Sub

    Public Function GetRiskDescription(ByVal ModelCode As String) As String
        Dim command As SqlCommand = Nothing
        Dim connectionObj As SqlConnection = Nothing
        Dim adapter As SqlDataAdapter = Nothing
        Dim dtRiskCode As System.Data.DataTable = Nothing
        Try
            adapter = New SqlDataAdapter
            dtRiskCode = New System.Data.DataTable
            connectionObj = New SqlConnection(ConfigurationManager.ConnectionStrings("XMLGenerator").ConnectionString)
            connectionObj.Open()
            command = New SqlCommand("dbo.spu_XMLGenerator_GetRiskDescription")
            command.Connection = connectionObj
            command.CommandType = CommandType.StoredProcedure
            command.Parameters.Add("@ModelName", SqlDbType.VarChar).Value = ModelCode
            adapter.SelectCommand = command
            adapter.Fill(dtRiskCode)
            Return dtRiskCode.Rows(0)(0).ToString()
        Finally
            command.Dispose()
            If (connectionObj.State = ConnectionState.Open) Then
                connectionObj.Close()
                connectionObj.Dispose()
            End If

        End Try
    End Function


End Class
