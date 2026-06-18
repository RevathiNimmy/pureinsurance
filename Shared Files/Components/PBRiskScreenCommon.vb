Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
<System.Runtime.InteropServices.ProgId("PBRiskScreenCommon_NET.PBRiskScreenCommon")> _
Public Module PBRiskScreenCommon
    '
    ' History:
    ' CJB 12/10/2005 : PN24820 Moved globals (g_vDataDictionary and g_vScreenValues) from here to RiskScreen as module
    '                  level variables (m_vDataDictionary and m_vScreenValues) in order to prevent corruption of variable values
    '                  when using multiple instances of this ocx (as in the case of child screens).
    '                  Note I have left g_vOriginalScreenValues as is as it is not used...should it be used it may need to be changed too.


    Public Declare Function BringWindowToTop Lib "user32" (ByVal hwnd As Integer) As Integer
    Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
    Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Private Const ACClass As String = "PBRiskScreenCommon"

    'non database controls
    Public Const ndcFreeFormatText As Integer = -1
    Public Const ndcHyperlink As Integer = -2
    'Jes added for Find control
    Public Const ndcFindControl As Integer = -3

    'Array constants for the Frame Details
    Public Const ACFTabNumber As Integer = 0
    Public Const ACFFrameNumber As Integer = 1
    Public Const ACFIsDeleted As Integer = 2
    Public Const ACFChildId As Integer = 3
    Public Const ACFGISObjectId As Integer = 4
    Public Const ACFTabSetIndex As Integer = 5
    Public Const ACFTabSnapToGrid As Integer = 6
    Public Const ACFDataModelType As Integer = 7
    Public Const ACFHelpText As Integer = 8 'added for Swift to PB screen generator support
    Public Const ACFLastArrayPosition As Integer = 8

    'Array constants for the Control Details
    Public Const ACCFrameNumber As Integer = 0
    Public Const ACCIsDeleted As Integer = 1
    Public Const ACCHelpText As Integer = 2
    Public Const ACCPreQuote As Integer = 3
    Public Const ACCPostQuote As Integer = 4
    Public Const ACCPurchase As Integer = 5
    Public Const ACCIsValuation As Integer = 6
    Public Const ACCIsRateAndPremium As Integer = 7
    Public Const ACCIncludeInList As Integer = 8
    Public Const ACCChildId As Integer = 9
    Public Const ACCGISObjectId As Integer = 10
    Public Const ACCPMFormat As Integer = 11
    Public Const ACCColumnPosition As Integer = 12
    Public Const ACCTabSetIndex As Integer = 13
    Public Const ACCLastArrayPosition As Integer = 13

    'chkYesNoDontKnow offsets
    Public Const cCheckBoxVerticalOffset As Integer = 30
    Public Const cCheckBoxHorizontalOffset As Integer = 200
    Public Const cCheckBoxCaptionWidth As Integer = 220
    ' Public Const cCheckBoxTriStateCaptionWidth As Integer = 1080
    Public Const cCheckBoxTriStateCaptionWidth As Integer = 1140   'Increased width for 'unknown' state
    Public Const cControlHorizontalOffset As Integer = 60

    'global variables
    Public g_sControlName As String = ""
    Public g_lx As Integer
    Public g_ly As Integer
    'holds the value of the current checkbox
    Public g_iCheckBoxValue As Integer

    ' CJB Moved the following into RiskScreen class to prevent COM errors in use of child screens PN24820
    '#If CodeBase = 19 Then
    'Public g_vDataDictionary(GISDMTypeLast) As Variant
    'Public g_vScreenValues As Variant
    '#End If

    ' AMB 10/01/03 - Start - IAG 217 Spec
    Public g_vOriginalScreenValues As Object
    ' AMB 10/01/03 - End - IAG 217 Spec
    ' AMB 08/01/03 - Start - Constant for 'Original Value'
    Public Const ORIGINAL_VALUE_STR As String = "Original Value: "
    ' AMB 08/01/03 - End - Constant for 'Original Value'

    Public Const ACFormatStandardMask As Integer = &H3FS
    Public Const ACFormatStandardClear As Integer = &HFFFFC0
    Public Const ACFormatCalendarMask As Integer = &H40S
    Public Const ACFormatCalendarClear As Integer = &HFFFFBF

    ' ***************************************************************** '
    '
    ' Name: SortThreeElementArray
    '
    ' Description:
    '
    ' History: 08/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub SortThreeElementArray(ByRef vArray(,) As Object, Optional ByVal iMode As Integer = 0)

        Dim vSwap As Object

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SortThreeElementArray")

        Try

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1) - 1
                For lTemp2 As Integer = lTemp + 1 To vArray.GetUpperBound(1)



                    If ToSafeLong(vArray(0, lTemp)) > ToSafeLong(vArray(0, lTemp2)) Then

                        vSwap = vArray(0, lTemp)


                        vArray(0, lTemp) = vArray(0, lTemp2)

                        vArray(0, lTemp2) = vSwap
                        Select Case iMode
                            Case 1
                                vSwap = vArray(1, lTemp)
                                vArray(1, lTemp) = vArray(1, lTemp2)
                                vArray(1, lTemp2) = vSwap
                            Case Else

                                vSwap = vArray(1, lTemp)


                                vArray(1, lTemp) = vArray(1, lTemp2)

                                vArray(1, lTemp2) = vSwap
                        End Select

                        vSwap = vArray(2, lTemp)


                        vArray(2, lTemp) = vArray(2, lTemp2)

                        vArray(2, lTemp2) = vSwap
                    End If
                Next lTemp2
            Next lTemp

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SortThreeElementArray")

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".SortThreeElementArray")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SortThreeElementArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SortThreeElementArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: StripColonFromCaption
    '
    ' Description:
    '
    ' History: 10/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function StripColonFromCaption(ByRef sCaption As String) As String



        Dim result As String = String.Empty
        If sCaption.EndsWith(":") Then
            result = sCaption.Substring(0, sCaption.Length - 1)
        Else
            result = sCaption
        End If

        result = result.Replace(Strings.Chr(13) & Strings.Chr(10), " ")
        Return result.Replace("  ", " ")

    End Function
End Module