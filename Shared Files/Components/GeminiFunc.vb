Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Public Module GeminiFunctions
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '


    Declare Function SetParent Lib "user32" (ByVal hWndChild As Integer, ByVal hWndNewParent As Integer) As Integer

    Public Const HelpContext As Integer = 1

    ' **************************************************************
    ' Name : ParseTag
    '
    ' Description : Splits a screen control tag into the Polaris
    ' Property Type and Property ID and the Database Table and
    ' and Field Names.
    '
    ' **************************************************************
    Public Function ParseTag(ByRef sTag As String, ByRef iPropertyType As Integer, ByRef lPropertyID As Integer, ByRef sTable As String, ByRef sField As String) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Dim sTmpPropertyType, sTmpPropertyID, sTmpTable, sTmpField, sTmpTag As String
        Dim iChar As Integer
        Dim sChar As String = ""
        Dim bComma As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise variables
            sTmpPropertyType = ""
            sTmpPropertyID = ""
            sTmpTable = ""
            sTmpField = ""

            ' Get a copy of the tag to play with
            sTmpTag = sTag

            ' Property type is the first character
            sTmpPropertyType = sTag.Substring(0, 1)

            ' Property ID is the next 8 characters
            sTmpPropertyID = sTmpTag.Substring(1, Math.Min(sTmpTag.Length, 8)).Trim()

            ' Trim off the first 9 characters
            sTmpTag = sTmpTag.Substring(sTmpTag.Length - (sTmpTag.Length - 9))

            bComma = False

            iChar = (sTmpTag.IndexOf(","c) + 1)

            If iChar = 0 Then
                sTmpTable = sTmpTag
            Else
                sTmpTable = sTmpTag.Substring(0, iChar - 1)
                sTmpField = sTmpTag.Substring(iChar)
            End If

            '    ' Now Loop through the remainder
            '    For iChar% = 1 To Len(sTmpTag$)
            '
            '        sChar$ = Mid$(sTmpTag$, iChar%, 1)
            '
            '        If (bComma = False) Then
            '
            '            If sChar$ = "," Then
            '                bComma = True
            '            Else
            '                ' Add to Table Name
            '                sTmpTable$ = sTmpTable$ & sChar$
            '            End If
            '
            '        Else
            '            ' Add to Field Name
            '            sTmpField$ = sTmpField$ & sChar$
            '        End If
            '
            '    Next

            ' Set return Values
            iPropertyType = Conversion.Val(sTmpPropertyType)
            lPropertyID = CInt(Conversion.Val(sTmpPropertyID))
            sTable = sTmpTable.Trim()
            sField = sTmpField.Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Tag : " & sTmpTag, vApp:=ACApp, vClass:=ACClass, vMethod:="ParseTag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    '******************************************************************************
    ' GEMRegSettings
    '
    ' Get the Gemini Settings for Polaris etc from the Registry
    '
    ' Russell Griffiths 05/11/97
    '******************************************************************************
    Public Function GEMRegSettings(ByRef sSchemePath As String, ByRef sAppPath As String, ByRef lAppID As Integer, ByRef iAppVer As Integer) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Dim sAppID As String = ""
        Dim sAppVer As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            gPMFunctions.GetRegSettings(sAppPath, "Gemini", "Polaris", "AppPath", "")
            gPMFunctions.GetRegSettings(sAppID, "Gemini", "Polaris", "AppId", "")
            gPMFunctions.GetRegSettings(sAppVer, "Gemini", "Polaris", "AppVer", "")
            gPMFunctions.GetRegSettings(sSchemePath, "Gemini", "Polaris", "SchemePath", "")

            lAppID = CInt(sAppID)
            iAppVer = CInt(sAppVer)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GEMRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '******************************************************************************
    ' GEMApostrophes
    '
    ' Take a string and replace ' with ''
    '
    ' Tom O''Toole 19/11/97
    '******************************************************************************
    Public Function GEMApostrophes(ByRef sString As String) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Dim i As Integer
        Dim sTemp2 As String = ""
        Dim sTemp As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sString.Length = 0 Then
                Return result
            End If

            sTemp = New StringBuilder("")
            sTemp2 = sString

            Do
                i = (sTemp2.IndexOf("'"c) + 1)

                If i = 0 Then
                    sTemp.Append(sTemp2)
                    Exit Do
                End If

                sTemp.Append(sTemp2.Substring(0, i - 1) & "''")
                sTemp2 = sTemp2.Substring(i)

                ' DN 16/02/99 - Stop apostrophes from multiplying
                If sTemp2.StartsWith("'") Then
                    Return result
                End If

            Loop

            sString = sTemp.ToString()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GEMApostrophes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '******************************************************************************
    ' GEMApostrophesIn
    '
    ' Take a string and replace '' with '
    '
    ' David Newson 15/02/99
    '******************************************************************************
    Public Function GEMApostrophesIn(ByRef sString As String, ByRef sStringOut As String) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Dim i As Integer
        Dim sTemp2 As String = ""
        Dim sTemp As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sString.Length = 0 Then
                Return result
            End If

            sTemp = New StringBuilder("")
            sTemp2 = sString

            Do
                i = (sTemp2.IndexOf("''") + 1)

                If i = 0 Then
                    sTemp.Append(sTemp2)
                    Exit Do
                End If

                sTemp.Append(sTemp2.Substring(0, i - 1) & "'")
                sTemp2 = sTemp2.Substring(i + 1)

            Loop

            sStringOut = sTemp.ToString()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GEMApostrophesIn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' **********************************************************
    ' Bubble Sort a two-dimensional Variant Array Alphabetically
    ' or numerically on the specified column.
    '
    ' N.B. Assume Rows is second dimension and Columns is first
    ' **********************************************************
    Public Sub BubbleSortArray(ByRef vArray(,) As Object, ByRef lColumn As Integer, Optional ByRef Descending As Byte = 0)

        Dim lEnd, lStart As Integer
        Dim iPosStep, iNegStep As Integer

        ' Do we want descending sort ?
        Dim bDesc As Boolean = False

        If Not Information.IsNothing(Descending) Then
            If Descending <> 0 Then
                bDesc = True
            End If
        End If


        ' Set up looping variables to give
        ' ascending or descending sort
        If Not bDesc Then

            lEnd = vArray.GetUpperBound(1)
            lStart = vArray.GetLowerBound(1)

            iPosStep = -1
            iNegStep = 1

        Else

            lStart = vArray.GetUpperBound(1)
            lEnd = vArray.GetLowerBound(1)

            iPosStep = 1
            iNegStep = -1

        End If


        ' Step down through the rows
        For i As Integer = lEnd To lStart + iNegStep Step iPosStep

            ' Step up through rows to meet current row
            For j As Integer = lStart To i + iPosStep Step iNegStep

                ' If the value in the selected column if greater than the
                ' next row up then swap the rows



                If vArray(lColumn, j) > vArray(lColumn, j + iNegStep) Then

                    SwapRows(vArray, j, j + iNegStep)

                End If

            Next j

        Next i

    End Sub


    ' Swap two rows in an array
    Private Sub SwapRows(ByRef vArray(,) As Object, ByRef lRow1 As Integer, ByRef lRow2 As Integer)
        Dim vTemp As Object

        For lCol As Integer = vArray.GetLowerBound(0) To vArray.GetUpperBound(0)
            vTemp = vArray(lCol, lRow1)
            vArray(lCol, lRow1) = vArray(lCol, lRow2)
            vArray(lCol, lRow2) = vTemp
        Next
    End Sub

    '******************************************************************************
    ' ShowHelp
    '
    ' Fire up a Common Dialog Control as Help with a specified Context ID
    '
    ' Russell Griffiths 13/02/98
    '******************************************************************************
    Public Function ShowHelp(ByRef dlgHelp As Control, ByRef lContextID As Integer) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Dim sHelpFile As String = ""

        Try

            ' Get Help File path
            gPMFunctions.GetRegSettings(sHelpFile, "Gemini", "Programs", "HelpFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Fire up the Common Dialog Control Help with the specified context ID
            ' (Context ID's defained in main.hh)

            'With dlgHelp


            '    .HelpFile = sHelpFile

            '    .HelpCommand = HelpContext

            '    .HelpContext = lContextID

            '    .result()



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in ShowHelp", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHelp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub ShowPolView(ByRef oPolCall As Object)
        Dim lReturn As Integer


        'Dim oDebug As Object = New ClassInterface()
        Dim oDebug As Object = New Object()

        oDebug.PolCall = oPolCall

        lReturn = oDebug.Start()

    End Sub



    '******************************************************************************
    ' CreateTestCase
    '
    ' Create the TestCase files in a new directory off the BackupXtra directory
    '
    ' G.Pagett 03/04/2001
    '******************************************************************************
    Public Function CreateTestCase(ByRef oPolCall As Object, ByRef sTestCaseFileName As String) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing
        Dim m_lReturn As Integer

        Dim sTestCasePath, sTestCaseAppend As String

        Try

            ' Get TestCase path and Append switch
            gPMFunctions.GetRegSettings(sTestCasePath, "Gemini", "Programs", "TestCasePath")
            gPMFunctions.GetRegSettings(sTestCaseAppend, "Gemini", "Settings", "TestCaseAppend")

            sTestCaseFileName = sTestCasePath & "\" & sTestCaseFileName

            ' Append turned off, delete testcase before re-creating
            If sTestCaseAppend = "0" Then

                If FileSystem.Dir(sTestCaseFileName, FileAttribute.Normal) <> "" Then
                    File.Delete(sTestCaseFileName)
                End If

            End If

            ' Call polaris to write the testcase

            m_lReturn = oPolCall.WriteTestCase(sTestCaseFileName, DateTime.Now.ToString("dd/MM HH:mm"))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in CreateTestCase", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTestCase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module