Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Imports System.Text

Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 17/02/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions to identify which application this is.
#If IsBusiness = 1 Then

	Public Const ACApp = "bPMBDocManager"
#Else
    Public Const ACApp As String = "iPMBDocManager"
#End If
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    Public Const DbTag As String = "DB"
    Public Const LoopTag As String = "LOOP"
    Public Const EndLoopTag As String = "ENDLOOP"
    Public Const FileTag As String = "FILE"
    Public Const QuestionTag As String = "KEY0"
    Public Const MandQuestionTag As String = "KEY0M"
    Public Const Separator As String = "_"
    Public Const ClauseTag As String = "CL"
    Public Const RiskLoopTag As String = "RSKLOOP"
    Public Const RiskHeaderTag As String = "RSKHEADER"
    Public Const RiskDocPrefix As String = "RK"
    Public Const StandardWordingsTag As String = "STANDARDWORDINGS"
    Public Const StandardWordingNPTag As String = "STANDARDWORDINGSNP"
    Public Const StandardWordingsCodeTag As String = "STANDARDWORDINGS_CODE"
    Public Const StandardWordingsDescTag As String = "STANDARDWORDINGS_DESC"
    Public Const IfTag As String = "IF"
    Public Const EndIfTag As String = "ENDIF"
    Public Const TotalTag As String = "TOTAL"
    Public Const VariableTag As String = "VAR"
    Public Const FunctionTag As String = "FUNC"
    Public Const ReportTag As String = "REPORT"
    Public Const ParameterTag As String = "PARAM"
    Public Const SubDocumentTag As String = "SUBDOC"
    Public Const DocumentSplitTag As String = "DOCUMENTSPLIT"


    'Bookmark array values
    Public Const BookmarkCode As Integer = 0
    Public Const BookmarkName As Integer = 1
    Public Const BookmarkValue As Integer = 2
    Public Const BookmarkType As Integer = 3
    Public Const BookmarkInstance1 As Integer = 4
    Public Const BookmarkInstance2 As Integer = 5
    Public Const BookmarkInstance3 As Integer = 6

    'Field array values
    Public Const FieldCode As Integer = 0
    Public Const FieldName As Integer = 1
    Public Const FieldValue As Integer = 2
    Public Const FieldType As Integer = 3
    Public Const FieldInstance1 As Integer = 4

    'Defines minimum number of field instances
    Public Const MinFieldInstances As Integer = 3

    'If extra instances added update array created in Interface.Start
    Public Const InstanceCount As Integer = 10
    Public iInstanceIndexArray(InstanceCount - 1) As Integer

    Public Const lCLAUSE_TYPE_ID As Integer = 7
    Public Const lLETTER_TYPE_ID As Integer = 5
    Public Const lSUBDOC_TYPE_ID As Integer = 9

    Public Const ACRiskMode As Integer = 0
    Public Const ACMarketMode As Integer = 1

    Public g_sDocPreBodyFragment As String
    Public g_sDocEndBodyFragment As String

    Sub Main_Renamed()
    End Sub
    Public Function BreakStringIntoArray(ByVal v_sStartTag As String, _
                                        ByVal v_sEndTag As String, _
                                        ByRef r_sString As String, ByRef r_vArray() As Object, _
                                        Optional ByVal v_bFormatString As Boolean = False) As Integer
        Dim sSTR() As String
        Dim lCnt As Long
        Dim sTmpLine As String
        Dim sEndFragment As String

        Dim lPos As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

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
    Public Function SetArrayIndexWithPreserve(ByRef r_vArray As Object) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        If IsArray(r_vArray) Then
            ReDim Preserve r_vArray(UBound(r_vArray) + 1)
        Else
            ReDim r_vArray(0)
        End If

        Return result
    End Function
    Public Function SetValueToArray(ByRef r_vArray As Object, ByVal v_vValue As Object, _
                               Optional ByVal v_bForamtString As Boolean = False) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue


        SetArrayIndexWithPreserve(r_vArray)

        If v_bForamtString Then

            RemoveInvalidCharacters(v_vValue)

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
    Public Function RemoveInvalidCharacters(ByRef r_sXML As String) As Integer
        If r_sXML Is Nothing Then
            r_sXML = ""
        End If
        If Len(r_sXML) > 0 Then
            'r_sXML = Replace(r_sXML, "£", Chr(194) & "£", , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(145), Chr(39), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(146), Chr(39), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(147), Chr(34), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(148), Chr(34), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(150), Chr(45), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(151), Chr(45), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(160), " ", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&", "&amp;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;lt;", "&lt;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;gt;", "&gt;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;amp;", "&amp;", , , vbTextCompare)
            'r_sXML = Replace(r_sXML, ChrW(-4056), "(", , , vbTextCompare)
            'r_sXML = Replace(r_sXML, ChrW(-3913), "(", , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(133), "...", , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(183), "", , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(148), Chr(34))
            r_sXML = Replace(r_sXML, Chr(147), Chr(34))
            r_sXML = Replace(r_sXML, "#amp#", "&amp;", , , vbTextCompare)
        End If
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function
End Module
