Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("iPMFunc_NET.iPMFunc")> _
Public Module iPMFunc
    ' ***************************************************************** '
    '
    ' Interface general functions module. Contains all of the global
    ' functions that might be useful when writing the interface layer.
    '
    ' RFC 27/02/1998 - GetResData from gPMlibs added.
    ' RFC 27/02/1998 - Encrypt from PMFunc added.
    ' RFC 27/02/1998 - GetCommandLine from PMFunc added.
    ' Ram 26/08/1999 - ShowFormInTaskBar is Added
    ' DB  08/02/2000 - ShellSort subroutine Added
    ' DD 30/09/2002: Add new ShowFormInTaskBar version
    ' AG 16/12/20004  - Added GetSystemSecurityModel method
    ' ***************************************************************** '


    Private vProductOptions As Object

    '--------------------------------------------------------------------------------
    'DD 30/09/2002
    'Used by ShowFormInTaskBar functions
    Private Structure CWPSTRUCT
        Dim lParam As Integer
        Dim wParam As Integer
        Dim message As Integer
        Dim hwnd As Integer
    End Structure

    Private Structure CREATESTRUCT
        Dim lpCreateParams As Integer
        Dim hInstance As Integer
        Dim hMenu As Integer
        Dim hWndParent As Integer
        Dim cy As Integer
        Dim cx As Integer
        Dim y As Integer
        Dim x As Integer
        Dim style As Integer
        Dim lpszName As Integer ' String
        Dim lpszClass As Integer ' String
        Dim ExStyle As Integer
    End Structure



    'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.2.1.1)
    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As Integer, ByVal lpWindowName As String) As Integer

    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.2.1.1)



    Private Declare Function GetWindowThreadProcessId Lib "user32" (ByVal hwnd As Integer, ByRef lpdwProcessId As Integer) As Integer
    Private Declare Function AttachThreadInput Lib "user32" (ByVal idAttach As Integer, ByVal idAttachTo As Integer, ByVal fAttach As Integer) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" () As Integer

    Private Declare Function CallNextHookEx Lib "user32" (ByVal hHook As Integer, ByVal ncode As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Integer, ByVal hwnd As Integer, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal hpvDest As Integer, ByVal hpvSource As Integer, ByVal cbCopy As Integer)
    Private Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer
    Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    Private Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As Integer, ByVal hmod As Integer, ByVal dwThreadId As Integer) As Integer
    Private Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hHook As Integer) As Integer
    Private Declare Function GetClassName Lib "user32" Alias "GetClassNameA" (ByVal hwnd As Integer, ByVal lpClassName As String, ByVal nMaxCount As Integer) As Integer

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Private Declare Function GetTempPathAPI Lib "kernel32" Alias "GetTempPathA" (ByVal nBufferLength As Integer, ByVal lpBuffer As String) As Integer

    Private Const WH_CALLWNDPROC As Integer = 4

    ' Misc Windows messages
    Private Const WM_CREATE As Integer = &H1S

    ' VB5 & VB6 class names:
    Private Const C_MDIFORMCLASS_IDE As String = "ThunderMDIForm"
    Private Const C_MDIFORMCLASS_EXE As String = "ThunderRT6MDIForm"
    Private Const C_MDIFORMCLASS5_IDE As String = "ThunderMDIForm"
    Private Const C_MDIFORMCLASS5_EXE As String = "ThunderRT5MDIForm"

    Private Const C_FORMCLASS_IDE_DC As String = "ThunderFormDC"
    Private Const C_FORMCLASS_EXE_DC As String = "ThunderRT6FormDC"
    Private Const C_FORMCLASS_IDE As String = "ThunderForm"
    Private Const C_FORMCLASS_EXE As String = "ThunderRT6Form"
    Private Const C_FORMCLASS5_IDE As String = "ThunderForm"
    Private Const C_FORMCLASS5_EXE As String = "ThunderRT5Form"

    Private Const GWL_WNDPROC As Integer = (-4)

    Private m_hHook As Integer
    Private m_lHookWndProc As Integer
    Private m_lReturn As Integer

    Private Const CB_SETDROPPEDWIDTH As Integer = &H160S
    ' DD 30/09/2002: End
    '--------------------------------------------------------------------------------

    ' /* For Showing Modal Form in Task Bar
    ' Ram - 26-Aug-1999
    ' Added the Sub routine called  ShowFormInTaskBar
    ' Added the following Declarations for the above routine

    Declare Function SetFocusAPI Lib "user32" Alias "SetFocus" (ByVal hwnd As Integer) As Integer
    Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
    Private Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)

    Private Const WS_EX_APPWINDOW As Integer = &H40000
    Private Const WS_EX_TOOLWINDOW As Integer = &H80
    Private Const GWL_EXSTYLE As Integer = (-20)
    Private Const KEYEVENTF_KEYUP As Integer = &H2S
    Private Const VK_LWIN As Integer = &H5BS

    ' Constant for the methods to identify which class this is.
    Private Const ACClass As String = "iPMFunc"

    ' Get GUID function - Start
    Private Structure GUID
        Dim Data1 As Integer
        Dim Data2 As Integer
        Dim Data3 As Integer
        <VBFixedArray(7)> _
        Dim Data4() As Byte
        Public Shared Function CreateInstance() As GUID
            Dim result As New GUID
            ReDim result.Data4(7)
            Return result
        End Function
    End Structure


    Private Declare Function CoCreateGuid Lib "OLE32.DLL" (ByRef pGuid As GUID) As Integer
    ' Get GUID function - End

    Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Public Const SWP_NOMOVE As Integer = &H2S
    Public Const SWP_NOSIZE As Integer = &H1S
    Public Const HWND_TOPMOST As Integer = -1
    Public Const HWND_TOP As Integer = 0
    Public Const HWND_NOTOPMOST As Integer = -2
    Public Const HWND_BOTTOM As Integer = 1

    Public Function SetWindowPlacement(ByVal lWindowHwnd As Integer, ByVal bKeepInFront As Boolean) As Integer

        Try

            If bKeepInFront Then
                m_lReturn = SetWindowPos(lWindowHwnd, HWND_TOPMOST, 1, 1, 1, 1, SWP_NOMOVE Or SWP_NOSIZE)
            Else
                m_lReturn = SetWindowPos(lWindowHwnd, HWND_NOTOPMOST, 1, 1, 1, 1, SWP_NOMOVE Or SWP_NOSIZE)
            End If

            If m_lReturn = 0 Then
                Throw New Exception
            End If

        Catch ex As Exception

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the window position", vApp:=ACApp, vClass:=ACClass, vMethod:="SetWindowPlacement", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Finally
            'Return gPMConstants.PMEReturnCode.PMTrue
        End Try
        Return m_lReturn
    End Function

    ' ***************************************************************** '
    ' Name: LogMessage
    '
    ' Description: Wrapper function to the LogMessage method call.
    '
    ' Changes:
    ' CTAF 230701 - Changed to use shiny new PMMessageV2 class
    '               This was supposed to use PMMessage initially anyway!!!
    ' CTAF 270701 - Trap unable to create PMMessageV2
    '
    ' ***************************************************************** '
    Public Sub LogMessage(ByVal iType As Integer, ByVal sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing, Optional ByRef sUsername As String = "", Optional ByRef bSilent As Boolean = False)

        Dim lReturn As Integer

        ' CTAF 270701

        Try

            ' Create an instance of the message object
            Dim oMessage As New iPMMessage.PMMessageV2

            ' CTAF 270701
            Try

                If Not (oMessage Is Nothing) Then

                    ' Log the message





                    lReturn = oMessage.LogMessage(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), vErrNo:=CStr(vErrNo), vErrDesc:=CStr(vErrDesc), bSilent:=bSilent)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' If it fails, then





                        gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))
                    End If

                    oMessage = Nothing

                Else

                    ' CTAF 270701 - Log the message as normal instead

                    ' Failed to log message, so we must call the
                    ' function to popup the message instead.





                    gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))

                End If

                Exit Sub

            Catch ex As System.Exception








                gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=ex)

                Exit Sub
            End Try

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SelectText
    '
    ' Description: Hightlights any text within the control passed.
    '
    ' ***************************************************************** '
    Public Sub SelectText(ByRef ctlControl As Control)

        Try

            ' Set the controls properties.
            With ctlControl

                'TODO check at runtime
                '.SelStart = 0

                'TODO check at runtime
                '.SelLength = Strings.Len(ctlControl.ToString())
            End With

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select the text", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetMousePointer
    '
    ' Description: Sets the mouse pointer to the state passed.
    '
    ' ***************************************************************** '
    Public Sub SetMousePointer(ByRef iMouseState As Integer)

        Static iMouseCounter As Integer

        Try

            ' Check the mouse state.
            Select Case (iMouseState)
                Case gPMConstants.PMEMousePointerStatus.PMMouseBusy
                    ' Set to busy mode.

                    ' Increament the mouse counter.
                    iMouseCounter += 1

                    ' Set the mouse pointer to the busy state,
                    ' but only set it once.
                    If iMouseCounter = 1 Then
                        Cursor.Current = Cursors.WaitCursor
                    End If

                Case gPMConstants.PMEMousePointerStatus.PMMouseNormal
                    ' Set to normal mode.

                    If iMouseCounter > 0 Then
                        ' Decreament the mouse counter.
                        iMouseCounter -= 1
                    End If

                    ' Set the mouse pointer to the normal state.
                    If iMouseCounter = 0 Then
                        Cursor.Current = Cursors.Default
                    End If

                Case gPMConstants.PMEMousePointerStatus.PMMouseReset
                    ' Reset to normal mode.

                    ' Reset the mouse counter.
                    iMouseCounter = 0

                    ' Set the mouse pointer to the normal state.
                    Cursor.Current = Cursors.Default

                Case Else
                    ' Invaild mouse state.

            End Select

        Catch excep As System.Exception



            ' Error Section.

            ' Reset the mouse counter.
            iMouseCounter = 0

            ' Set the mouse pointer to the normal state.
            Cursor.Current = Cursors.Default

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the mouse pointer", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMousePointer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CenterForm
    '
    ' Description: Center's the form to the screen.
    '
    ' ***************************************************************** '
    Public Sub CenterForm(ByRef frmForm As Form)

        Try

            ' Center the form
            frmForm.SetBounds((Screen.PrimaryScreen.Bounds.Width / 2) - (frmForm.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (frmForm.Height / 2), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to center the form", vApp:=ACApp, vClass:=ACClass, vMethod:="CenterForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PositionForm
    '
    ' Description: Positions the form using the coordinates passed.
    '
    ' ***************************************************************** '
    Public Sub PositionForm(ByRef frmForm As Form, ByRef lTop As Integer, ByRef lLeft As Integer)

        Try

            ' Center the form
            frmForm.SetBounds(VB6.TwipsToPixelsX(lLeft), VB6.TwipsToPixelsY(lTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to position the form", vApp:=ACApp, vClass:=ACClass, vMethod:="PositionForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ForceLostFocus
    '
    ' Author: Stewart Peart
    '
    ' Description: Used to ensure that the Lost Focus event is processed
    ' on the last active control prior to processing the Command button
    ' Click Event or similar. If not used, pressing Return or using the
    ' Accelerator key will miss the previous Lost Focus event and any
    ' associated validation.
    '
    ' ***************************************************************** '

    Public Function ForceLostFocus(ByRef ctlCommand As Control) As Boolean

        Dim result As Boolean = False
        Try

            result = True

            ' Did we arrive here via a Default key press ?

            If Not (ctlCommand.FindForm().ActiveControl Is ctlCommand) Then
                ctlCommand.Focus() ' Trigger lost focus on previous control
            End If

            Application.DoEvents() ' Force any pending lost focus to be processed

            ' If the lost focus code gave focus elsewhere then validation
            ' has failed, so return False to be checked to cause event exit.

            If Not (ctlCommand.FindForm().ActiveControl Is ctlCommand) Then
                result = False
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to force lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="ForceLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Encrypt
    '
    ' Description: Encrypts string passed and returns the result.
    '
    ' ***************************************************************** '
    ' ************* If you change this function you MUST also change
    ' ************* the bPMFunc version.
    ' *************
    Public Function Encrypt(ByRef sPassword As String, ByRef sEncryptedPassword As String) As Integer

        Dim result As Integer = 0
        Dim sAString As String = ""
        Dim sBString As New StringBuilder
        Dim iCntr As Integer
        Dim sChar1 As New FixedLengthString(1)
        Dim sChar2 As New FixedLengthString(1)
        Dim iSn As Integer
        Dim sCodeString As String = ""
        Dim iClen As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Encrypts the supplied string returning the encrypted
            ' result. Encrypted string will always be 2 characters
            ' longer than original (leave space!)
            '
            ' Encrypted string contains only ASCII characters in
            ' range 32-126

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            iClen = sCodeString.Length

            sAString = sPassword
            iCntr = sAString.Length

            If iCntr < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                sEncryptedPassword = ""

                Return result
            End If

            sChar1.Value = sCodeString.Substring((Strings.Asc(sAString.Substring(0, 1)(0)) + iCntr) Mod iClen, 1)
            sChar2.Value = sCodeString.Substring(Strings.Asc(sAString.Substring(sAString.Length - 1)(0)) Mod iClen, 1)
            iSn = ((Strings.Asc(sChar1.Value(0)) + Strings.Asc(sChar2.Value(0))) Mod iClen) + 1
            sBString = New StringBuilder(sChar2.Value)

            For iCntr2 As Integer = 1 To iCntr
                sBString.Append(sCodeString.Substring((Strings.Asc(sAString.Substring(iCntr2 - 1, 1)(0)) + iSn + iCntr2) Mod iClen, 1))
            Next iCntr2

            sBString.Append(sChar1.Value)

            ' Return the result.
            sEncryptedPassword = sBString.ToString().Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            sEncryptedPassword = ""

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to encrypt the string", vApp:=ACApp, vClass:=ACClass, vMethod:="Encrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: LicenceEncrypt
    '
    ' Description: Encrypts string passed and returns the result.
    '              Copied from Encrypt but returns a string.
    '
    ' ************* If you change this function you MUST also change
    ' ************* the bPMFunc version.
    ' *************
    '
    ' History: 24/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function LicenceEncrypt(ByRef sLicence As String, ByRef sLicenceKey As String) As Integer
        Dim result As Integer = 0
        Dim sAString As String = ""
        Dim sBString As New StringBuilder
        Dim iCntr As Integer
        Dim sChar1 As New FixedLengthString(1)
        Dim sChar2 As New FixedLengthString(1)
        Dim iSn As Integer
        Dim sCodeString As String = ""
        Dim iClen As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Encrypts the supplied string returning the encrypted
            ' result. Encrypted string will always be 2 characters
            ' longer than original (leave space!)
            '
            ' Encrypted string contains only ASCII characters in
            ' range 32-126

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            iClen = sCodeString.Length

            sAString = sLicence
            iCntr = sAString.Length

            If iCntr < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                sLicenceKey = ""

                Return result
            End If

            sChar1.Value = sCodeString.Substring((Strings.Asc(sAString.Substring(0, 1)(0)) + iCntr) Mod iClen, 1)
            sChar2.Value = sCodeString.Substring(Strings.Asc(sAString.Substring(sAString.Length - 1)(0)) Mod iClen, 1)
            iSn = ((Strings.Asc(sChar1.Value(0)) + Strings.Asc(sChar2.Value(0))) Mod iClen) + 1
            sBString = New StringBuilder(sChar2.Value)

            For iCntr2 As Integer = 1 To iCntr
                sBString.Append(sCodeString.Substring((Strings.Asc(sAString.Substring(iCntr2 - 1, 1)(0)) + iSn + iCntr2) Mod iClen, 1))
            Next iCntr2

            sBString.Append(sChar1.Value)

            ' Return the result.
            sLicenceKey = sBString.ToString().Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LicenceEncrypt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LicenceEncrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetCommandLine
    '
    ' Description: Gets the command line passed and splits it down
    '              into the seporate arguments.
    '
    ' ***************************************************************** '
    ' ************* If you change this function you MUST also change
    ' ************* the bPMFunc version.
    ' *************
    Public Function GetCommandLine(ByRef vArgArray As Object, Optional ByRef vMaxArgs As Object = Nothing) As Integer

        'Declare variables.
        Dim result As Integer = 0
        Dim sChar, sCmdLine As String
        Dim iCmdLineLen As Integer
        Dim bInArg As Boolean
        Dim iMaxArgs, iNumArgs As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'See if MaxArgs was provided.

            Dim dbNumericTemp As Double

            If (Not Information.IsNothing(vMaxArgs)) And (Double.TryParse(CStr(vMaxArgs), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                iMaxArgs = CInt(vMaxArgs)
            Else
                iMaxArgs = 100
            End If

            'Make array of the correct size.
            ReDim vArgArray(iMaxArgs)

            ' Initialise
            iNumArgs = 0
            bInArg = False

            'Get command line arguments.
            sCmdLine = Interaction.Command()

            ' Get the length of the command line
            iCmdLineLen = sCmdLine.Length

            'Go thru command line one character at a time.
            For iSub As Integer = 1 To iCmdLineLen

                sChar = Mid(sCmdLine, iSub, 1)

                'Test for space or tab.
                If (sChar <> " ") And (sChar <> Strings.Chr(9)) Then

                    'Neither space nor tab.

                    'Test if already in argument.
                    If Not bInArg Then

                        'New argument begins.

                        'Test for too many arguments.
                        If iNumArgs >= iMaxArgs Then
                            Exit For
                        End If

                        iNumArgs += 1
                        bInArg = True

                    End If

                    'Concatenate character to current argument.


                    vArgArray(iNumArgs - 1) = CStr(vArgArray(iNumArgs - 1)) & sChar

                Else

                    'Found a space or tab.
                    'Set bInArg flag to False.
                    bInArg = False

                End If

            Next iSub

            'Resize array just enough to hold arguments.
            If iNumArgs > 0 Then
                ReDim Preserve vArgArray(iNumArgs - 1)
            Else

                vArgArray = ""
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMELogLevel.PMLogOnError

            ' Log Error.
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process the Command Line :- " & sCmdLine, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommandLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Sub ShowFormInTaskBar(ByVal InHwnd As Integer, ByVal bState As Boolean)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ShowFormInTaskBar
        ' PURPOSE: 'Often it is rather handy to put a modal form in the task bar - say for example
        'a login dialog which shows before your main form - otherwise the user can
        'loose this window behind other ones.
        '
        'The rules the taskbar uses to decide whether a button should be shown for a
        'window aren 't very well documented. Here is how it is done:
        '
        'When a window is created, the taskbar examines the window’s extended style
        'to see if either the WS_EX_APPWINDOW  (&H40000)
        '                  or WS_EX_TOOLWINDOW (&H80)
        'style is turned on.

        'If WS_EX_APPWINDOW is turned on, the taskbar shows a button for the window,
        'and if WS_EX_ TOOLWINDOW is turned on, the taskbar does not show a button for
        'the window.

        'Note : A window should never have both of these extended styles. If the
        'window doesn't have either of these styles, the taskbar decides to create
        'a button, if the window is unowned and does not create a button if the window
        'is owned.
        '
        'You should call this Procedure once your window has a valid hWnd and is visible,
        'for example, in the Form_Activate event.

        ' AUTHOR: Ram Chandrabose
        ' DATE: 23-Aug-1999
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            Dim lS As Integer

            ' /* Minimize all the forms to Task Bar */

            'The trick is to mimic the keyboard events required to bring the Taskbar
            'popup menu and send it the letter "M" to select the "Minimize All Windows"
            'option. This is accomplished with three calls to the keybd_event API.
            '
            'The second argument for the keybd_event call is the hardware scan code,
            'and, in this case, you could use the value 91. However, because applications
            'should not use the scan code, it has been left as 0.
            ' 77 is the character code for the letter 'M'
            keybd_event(VK_LWIN, 0, 0, 0)
            keybd_event(77, 0, 0, 0)
            keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, 0)

            '/* Code ends for Minimising all the forms. */

            lS = GetWindowLong(InHwnd, GWL_EXSTYLE)
            If bState Then
                lS = lS Or WS_EX_APPWINDOW
                lS = lS And Not WS_EX_TOOLWINDOW
            Else
                lS = lS And Not WS_EX_APPWINDOW
            End If

            SetWindowLong(InHwnd, GWL_EXSTYLE, lS)
            SetForegroundWindow(InHwnd)
            SetFocusAPI(InHwnd)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowFormInTaskBar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Finally
        End Try
    End Sub

    ' ************************************************************************ '
    ' Name: ShellSort
    '
    ' Description: Shell Sort half inched off the web.
    ' A = Variant Array, Lb = Lower Bound of Array, Ub = Upper Bound of Array
    ' ************************************************************************ '

    Public Sub ShellSort(ByRef A As Array, ByVal Lb As Integer, ByVal Ub As Integer)

        Dim j As Integer
        Dim t As Object

        ' sort array[lb..ub]

        ' compute largest increment
        Dim n As Integer = Ub - Lb + 1
        Dim h As Integer = 1
        If n < 14 Then
            h = 1
        Else
            Do While h < n
                h = 3 * h + 1
            Loop
            h = h \ 3
            h = h \ 3
        End If

        Do While h > 0
            ' sort by insertion in increments of h
            For i As Integer = Lb + h To Ub


                t = A(i)
                For j = i - h To Lb Step -h



                    If A(j) <= t Then Exit For


                    A(j + h) = A(j)
                Next j


                A(j + h) = t
            Next i
            h = h \ 3
        Loop

    End Sub

    ' ************************************************************************ '
    ' Name: ShellSortDistinct
    '
    ' Description: ShellSort and array and remove duplicate values.
    ' A = Variant Array, Lb = Lower Bound of Array, Ub = Upper Bound of Array
    ' ************************************************************************ '
    Public Sub ShellSortDistinct(ByRef vArray As Array)

        Dim vLastvalue As Object

        ' Get array bounds
        Dim lLower As Integer = vArray.GetLowerBound(0)
        Dim lUpper As Integer = vArray.GetUpperBound(0)

        ' Sort the array
        ShellSort(vArray, lLower, lUpper)

        ' Remove duplicates
        Dim lTargetRow As Integer = lLower

        ' Loop the entire array
        For lSourceRow As Integer = lLower To lUpper
            ' Is our current row is different to our current target?


            If Not vArray(lTargetRow).Equals(vArray(lSourceRow)) Then
                ' If so increment target row and copy across value
                lTargetRow += 1


                vArray(lTargetRow) = vArray(lSourceRow)
            End If
        Next lSourceRow

        ' Redimension our array to the last target row
        vArray = ArraysHelper.RedimPreserve(Of Object())(vArray, New Integer() {lTargetRow - lLower + 1}, New Integer() {lLower})

    End Sub


    'eck070601 Version of SetChildFormPosition in Orion
    Public Function CascadeForm(ByRef frmParent As Form, ByRef frmChild As Form) As Integer

        Dim result As Integer = 0
        Const CStandardOffset As Integer = 600 ' Twips

        Dim lHorizontalOffset As Integer = 0
        If (VB6.PixelsToTwipsX(frmParent.Left) + CStandardOffset + VB6.PixelsToTwipsX(frmChild.Width)) <= VB6.PixelsToTwipsX(Screen.PrimaryScreen.Bounds.Width) Then
            lHorizontalOffset = CStandardOffset
        Else
            If (VB6.PixelsToTwipsX(frmParent.Left) - CStandardOffset) >= 0 Then
                lHorizontalOffset = -CStandardOffset
            End If
        End If

        Dim lVerticalOffset As Integer = 0
        If (VB6.PixelsToTwipsY(frmParent.Top) + CStandardOffset + VB6.PixelsToTwipsY(frmChild.Height)) <= VB6.PixelsToTwipsY(Screen.PrimaryScreen.Bounds.Height) Then
            lVerticalOffset = CStandardOffset
        Else
            If (VB6.PixelsToTwipsY(frmParent.Top) - CStandardOffset) >= 0 Then
                lVerticalOffset = -CStandardOffset
            End If
        End If

        frmChild.Left = frmParent.Left + VB6.TwipsToPixelsX(lHorizontalOffset)
        frmChild.Top = frmParent.Top + VB6.TwipsToPixelsY(lVerticalOffset)

        Return gPMConstants.PMEReturnCode.PMTrue



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cascade Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CascadeForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.2.1.2)
    Public Function RunDocumaster(ByRef v_sLinkCode As String) As Integer

        Dim result As Integer = 0
        Dim iDocManager As iDOCManager.Interface
        Dim lWinHand As Integer
        Dim sLinkCodeAndLevel As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check link level is correct i.e. 1 = client (drawer) level,
            '                                  2 = policy (folder) level
            If (Not v_sLinkCode.EndsWith("1")) And (Not v_sLinkCode.EndsWith("2")) Then

                ' error
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Link Code must be 1(client) or 2(policy)", vApp:=ACApp, vClass:=ACClass, vMethod:="RunDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' and link code not blank
            If v_sLinkCode = "" Then

                ' error
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Link Code incorrectly set to blank", vApp:=ACApp, vClass:=ACClass, vMethod:="RunDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Create Link Code and Level to pass to documaster
            sLinkCodeAndLevel = "SBO" & v_sLinkCode
            'sLinkCodeAndLevel = "SBO" & Left$(v_sLinkCode, Len(v_sLinkCode) - 1)


            ' See if Documaster is already running
            'SP040898 - see above
            lWinHand = FindWindow(0, "DocuMaster Enterprise  ")

            If lWinHand <> 0 Then

                'DocuMaster is already running
                iDocManager = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCManager.Interface")

                'Show the interface
                m_lReturn = iDocManager.Activate(sLinkCodeAndLevel)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'error
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Active DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="RunDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()

                    iDocManager = Nothing

                    Return result

                End If

            Else

                'DocuMaster is not already running
                iDocManager = New iDOCManager.Interface()

                'initialise the main interface
                m_lReturn = CType(iDocManager, SSP.S4I.Interfaces.ILocalInterface).Initialise()



                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue

                    Case gPMConstants.PMEReturnCode.PMCancel

                        iDocManager.Dispose()

                        iDocManager = Nothing

                        Return result

                    Case Else

                        'error
                        LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Initilise DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="RunDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        result = gPMConstants.PMEReturnCode.PMFalse
                        iDocManager.Dispose()

                        iDocManager = Nothing

                        Return result

                End Select

                'Start the interface
                m_lReturn = iDocManager.Start(sLinkCodeAndLevel)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'error
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Start DocManager", vApp:=ACApp, vClass:=ACClass, vMethod:="RunDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    iDocManager.Dispose()

                    iDocManager = Nothing

                    Return result

                End If

            End If

            'Finished now
            iDocManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunDocumaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunDocumaster", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.2.1.2)

    ' ******************************************************'
    '
    ' Name: retrieveProductOptions
    '
    ' Description: This will create a business object to
    '               populate the array
    '
    ' History: 06/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function retrieveProductOptions(ByRef r_vProductOptions(,) As Object) As Integer
        Dim result As Integer = 0
        Dim bSIRProductOptions As Object


        Dim oProductOption As bSIRProductOptions.Business
        Dim oObjectManager As bObjectManager.ObjectManager
        Dim lResult As Integer

        Try

            ' Create an instance of the object manager and Initialise
            oObjectManager = New bObjectManager.ObjectManager()

            lResult = oObjectManager.Initialise(sCallingAppName:=ACApp)

            '   If not initialised then call error handler
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError
                oObjectManager = Nothing

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOption", excep:=New Exception(Information.Err().Description))

                Return result

            End If

            '   Find the Business Class
            Dim temp_oProductOption As Object
            lResult = oObjectManager.GetInstance(temp_oProductOption, "bSIRProductOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProductOption = temp_oProductOption

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError
                oObjectManager = Nothing

                ' RDC 14042005 changed to correct object name - it's not bSIRLibraries!
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise bSIRProductOptions object", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOption", excep:=New Exception(Information.Err().Description))

                Return result

            End If

            '   Get the business object to populate the array

            lResult = oProductOption.getAllHiddenOptions(r_vResultArray:=r_vProductOptions)

            '   Should find at least one value in the database
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lResult


                oProductOption.Dispose()
                oProductOption = Nothing
                oObjectManager.Dispose()
                oObjectManager = Nothing

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve product options", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOption", excep:=New Exception(Information.Err().Description))

                Return result
            End If

            '   Terminate the business object

            oProductOption.Dispose()
            oProductOption = Nothing
            '   Terminate the object Manager
            oObjectManager.Dispose()
            ' Destroy the instance of the object manager from memory
            oObjectManager = Nothing


            Return lResult

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="retrieveProductOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="retrieveProductOptions", excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: RetrieveSingleSystemOption
    '
    ' Description:  gets the system option required for the current branch
    '               held in g_iSourceID
    '
    ' History: SW 07/04/2003
    '
    ' ***************************************************************** '
    Public Function RetrieveSingleSystemOption(ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim bSIROptions As Object


        Dim oSystemOptions As bSIROptions.Business
        Dim oObjectManager As bObjectManager.ObjectManager
        Dim lResult As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager and Initialise
            oObjectManager = New bObjectManager.ObjectManager()

            lResult = oObjectManager.Initialise(sCallingAppName:=ACApp)

            '   If not initialised then call error handler
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObjectManager = Nothing
                Return result
            End If

            '   Find the Business Class
            Dim temp_oSystemOptions As Object
            lResult = oObjectManager.GetInstance(temp_oSystemOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSystemOptions = temp_oSystemOptions

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObjectManager = Nothing
                Return result
            End If

            'get the system option

            lResult = oSystemOptions.getOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue, v_iSourceID:=v_iSourceID)

            If lResult = gPMConstants.PMEReturnCode.PMNotFound Then
                'Return 0 to stop errors occuring when option is not available
                r_sOptionValue = "0"
            ElseIf lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            oSystemOptions.Dispose()
            oSystemOptions = Nothing
            oObjectManager.Dispose()
            oObjectManager = Nothing

            Return result
            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)

        Finally


        End Try
        Return result
    End Function



    ' ***************************************************************** '
    '
    ' Name: CreateUserControl
    '
    ' Description: Function for creating a late bound user control
    '
    ' Paramters :
    '   v_sProgID = ProgID of control e.g.PartyPCControl.uctPartyPCControl
    '   v_sObjectName = Name of the object (used internally) e.g. uctPartyPC
    '   v_oForm = Form the controls going on to
    '   r_oContainer = Placemarker for where the form will go, size etc...
    '   r_oControl = Returns the control that's been created
    '   v_vLicenseKey = Optional License Key needed to create the control
    '
    ' History: 20020726 CTAF - Created.
    '          20020806 CTAF - Moved into iPMFunc and parameters described
    '
    ' ***************************************************************** '

    Public Function CreateUserControl(ByVal v_sProgID As String, ByVal v_sObjectName As String, ByVal v_oForm As Object, ByRef r_oContainer As Object, ByRef r_oControl As Object, Optional ByVal v_vLicenseKey As String = "", Optional ByVal v_bNoResizeControl As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sLicense As String = ""
        Dim bExists, bNoResizeControl As Boolean 'MKW080503 PN3967


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'MKW080503 PN3967 START
            If False Then
                bNoResizeControl = False
            Else
                bNoResizeControl = v_bNoResizeControl
            End If
            'MKW080503 PN3967 END

            ' Check the license isn't added already
            bExists = False
            'TODO check at runtime

            'If Not Information.IsNothing(v_vLicenseKey) Then

            '  For Each oLicInfo As LicenseInfo In Licenses
            ' If the progid and licencekey matches then we have a winner


            '    If (oLicInfo.ProgId = v_sProgID) And (oLicInfo.LicenseKey = v_vLicenseKey) Then
            '        bExists = True
            '        Exit For
            '    End If
            '    Next oLicInfo

            'Else

            '    For Each oLicInfo As LicenseInfo In Licenses
            '        ' If the progid's match then we have a winner

            '        If oLicInfo.ProgId = v_sProgID Then
            '            bExists = True
            '            Exit For
            '        End If
            '    Next oLicInfo
            'End If

            ' If its not in the license collection then add it
            'If Not bExists Then

            '    ' Add the license

            '    If Not Information.IsNothing(v_vLicenseKey) Then

            '        sLicense = Licenses.Add(v_sProgID, v_vLicenseKey)
            '    Else

            '        sLicense = Licenses.Add(v_sProgID)
            '    End If

            ' End If

            ' Add the control

            r_oControl = v_oForm.Controls.Add(v_sProgID, v_sObjectName)

            ' Set the dimensions and location of the control to match those
            ' of the placemarker


            r_oControl.Left = r_oContainer.Left


            r_oControl.Top = r_oContainer.Top

            'MKW080503 PN3967 START - Do Not Resize Control (if variable states)
            If Not (bNoResizeControl) Then


                r_oControl.Width = r_oContainer.Width


                r_oControl.Height = r_oContainer.Height
            End If
            'MKW080503 PN3967 END

            ' Hide the placemarker

            r_oContainer.Visible = False
            ' Show the new user control

            r_oControl.Visible = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateUserControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUserControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Public Sub ShowFormInTaskBar_Attach()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ShowFormInTaskBar_Attach
        ' PURPOSE: Hooks the calling window so that the WM_CREATE message can be
        ' trapped. This call is made from the Form_Initialize event which occurs
        ' before Windows does the form creation.
        ' *IMPORTANT* Forms must have their ShowInTaskBar property=FALSE
        ' AUTHOR: Danny Davis
        ' DATE: 30/09/2002, 16:58
        ' CHANGES:
        ' ---------------------------------------------------------------------------



        If IsInIDE() Then
            Exit Sub 'do not hook if in VB IDE. Bad thinks happen if program crashes
        End If

        Try


            'TODO check at runtime
            'm_hHook = SetWindowsHookEx(WH_CALLWNDPROC, AddressOf AppHook(), VB6.GetHInstance().ToInt32(), Thread.CurrentThread.ManagedThreadId)
            Debug.Assert(m_hHook <> 0, "")


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowFormInTaskBar_Attach", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Finally
        End Try
    End Sub

    Public Sub ShowFormInTaskBar_Detach()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ShowFormInTaskBar_Detach
        ' PURPOSE: Removes the hook from the form. This is called from the Form_Load
        ' event once the form is created on screen.
        ' *IMPORTANT* Forms must have their ShowInTaskBar property=FALSE
        ' AUTHOR: Danny Davis
        ' DATE: 30/09/2002, 17:00
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            If m_hHook <> 0 Then
                UnhookWindowsHookEx(m_hHook)
                m_hHook = 0
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowFormInTaskBar_Detach", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

        Finally
        End Try
    End Sub

    Private Function AppHook(ByVal idHook As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AppHook
        ' PURPOSE: The ShowInTaskBar hook points to here. This traps the WM_CREATE
        ' message and calls the Form_WndProc function.
        ' AUTHOR: Danny Davis
        ' DATE: 30/09/2002, 17:02
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Catch_Renamed)")

        Dim CWP As New CWPSTRUCT
        Dim k As Integer
        Dim aClass As String = ""

        If idHook >= 0 Then
            Dim handle As GCHandle = GCHandle.Alloc(CWP, GCHandleType.Pinned)
            Dim handle2 As GCHandle = GCHandle.Alloc(lParam, GCHandleType.Pinned)

            Try

                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

                CopyMemory(tmpPtr, tmpPtr2, Marshal.SizeOf(CWP))
            Finally
                handle.Free()
                handle2.Free()
            End Try
            Select Case CWP.message
                Case WM_CREATE
                    aClass = New String(" "c, 128)
                    k = GetClassName(CWP.hwnd, aClass, 128)
                    aClass = aClass.Substring(0, k)
                    'TODO check at runtime
                    'If IsIn(aClass, C_MDIFORMCLASS_IDE, C_MDIFORMCLASS_EXE, C_MDIFORMCLASS5_IDE, C_MDIFORMCLASS5_EXE, C_FORMCLASS_IDE_DC, C_FORMCLASS_EXE_DC, C_FORMCLASS_IDE, C_FORMCLASS_EXE, C_FORMCLASS5_IDE, C_FORMCLASS5_EXE) Then

                    '    m_lHookWndProc = SetWindowLong(CWP.hwnd, GWL_WNDPROC, AddressOf Form_WndProc())
                    'End If
            End Select
        End If
        Dim handle3 As GCHandle = GCHandle.Alloc(lParam, GCHandleType.Pinned)

        Try
            Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()
            result = CallNextHookEx(m_hHook, idHook, wParam, tmpPtr3)
        Finally
            handle3.Free()
        End Try

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------

        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

Catch_Renamed:
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AppHook", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
        End Select

Finally_Renamed:
        Return result


    End Function

    Private Function Form_WndProc(ByVal hwnd As Integer, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Form_WndProc
        ' PURPOSE: This function takes intercepted WM_CREATE message and alters the
        ' form's style tricking Windows into generating a button on the Task Bar.
        ' Only APPWINDOW windows do this in Windows - applying this style to the
        ' form lets us show Modals as normal applications.
        ' AUTHOR: Danny Davis
        ' DATE: 30/09/2002, 17:03
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Catch_Renamed)")

        Dim tCS As New CREATESTRUCT
        Dim lSetStyleEX As Integer

        ' SPM - specific wnd proc for a form.  Only called once for the WM_CREATE message.
        Select Case Msg
            Case WM_CREATE
                Dim handle As GCHandle = GCHandle.Alloc(tCS, GCHandleType.Pinned)
                Dim handle2 As GCHandle = GCHandle.Alloc(lParam, GCHandleType.Pinned)

                Try

                    Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

                    CopyMemory(tmpPtr, tmpPtr2, Marshal.SizeOf(tCS))
                Finally
                    handle.Free()
                    handle2.Free()
                End Try
                lSetStyleEX = GetWindowLong(hwnd, GWL_EXSTYLE)
                lSetStyleEX = lSetStyleEX Or WS_EX_APPWINDOW
                lSetStyleEX = lSetStyleEX And (Not WS_EX_TOOLWINDOW)
                tCS.ExStyle = lSetStyleEX
                Dim handle3 As GCHandle = GCHandle.Alloc(lParam, GCHandleType.Pinned)
                Dim handle4 As GCHandle = GCHandle.Alloc(tCS, GCHandleType.Pinned)

                Try

                    Dim tmpPtr4 As IntPtr = handle4.AddrOfPinnedObject()

                    Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()
                    CopyMemory(tmpPtr3, tmpPtr4, Marshal.SizeOf(tCS))
                Finally
                    handle3.Free()
                    handle4.Free()
                End Try
                SetWindowLong(hwnd, GWL_EXSTYLE, tCS.ExStyle)
                SetWindowLong(hwnd, GWL_WNDPROC, m_lHookWndProc)
        End Select
        result = CallWindowProc(m_lHookWndProc, hwnd, Msg, wParam, lParam)

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------

        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

Catch_Renamed:
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_WndProc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
        End Select

Finally_Renamed:
        Return result


    End Function

    Public Function IsIn(ByVal vComp As String, ByVal ParamArray vTo() As Object) As Boolean
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: IsIn
        ' PURPOSE: Used by the AppHook function.
        ' AUTHOR: Danny Davis
        ' DATE: 30/09/2002, 17:05
        ' CHANGES:
        ' ---------------------------------------------------------------------------



        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Catch_Renamed)")

        Dim iL, iU As Integer


        Try
            iU = vTo.GetUpperBound(0)
            If Information.Err().Number = 0 Then
                iL = vTo.GetLowerBound(0)
                For i As Integer = iL To iU

                    If vComp = CStr(vTo(i)) Then
                        Return True
                    End If
                Next i
            End If

            GoTo Finally_Renamed

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

Catch_Renamed:
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="IsIn", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    GoTo Finally_Renamed
            End Select

Finally_Renamed:
        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try



    End Function


    ' ***************************************************************** '
    '
    ' Name: IsInIDE
    '
    ' Description: Returns true if program is running in the VB IDE else retuns False
    '
    ' History: 30/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function IsInIDE() As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".IsInIDE")

        Try

            result = False

            Debug.WriteLine(1 / 0)

            Return result

        Catch


            Return True
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: ForceForegroundWindow
    '
    ' Description: Force a window in an out of proc server to the front
    '
    '
    ' History: 18/03/2003 Chris Ridgard - Created.
    '
    ' ***************************************************************** '
    Public Function ForceForegroundWindow(ByVal hwnd As Integer) As Boolean
        Dim ThreadID1, ThreadID2, nRet As Integer
        '
        ' Nothing to do if already in foreground.
        '
        If hwnd = GetForegroundWindow() Then
            Return True
        Else
            '
            ' First need to get the thread responsible for this window,
            ' and the thread for the foreground window.
            '
            ThreadID1 = GetWindowThreadProcessId(GetForegroundWindow(), 0)
            ThreadID2 = GetWindowThreadProcessId(hwnd, 0)
            '
            ' By sharing input state, threads share their concept of
            ' the active window.
            '
            If ThreadID1 <> ThreadID2 Then
                AttachThreadInput(ThreadID1, ThreadID2, True)
                nRet = SetForegroundWindow(hwnd)
                AttachThreadInput(ThreadID1, ThreadID2, False)
            Else
                nRet = SetForegroundWindow(hwnd)
            End If


            Return nRet
        End If
    End Function

    ' ***************************************************************************
    ' Moved from gSirLibrary as Part of the Global Data Changes
    ' ***************************************************************************

    ' ******************************************************'
    '
    ' Name: getProductOption
    '
    ' Description: This will retrieve all product options from
    '               the business option if not already retrieved
    '               It will then find a value for the option
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Private Function getProductOption(ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String, ByVal v_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer



        lReturn = gPMConstants.PMEReturnCode.PMTrue

        '   This will check whether array is empty
        '   If so then it will create a business object to retrieve
        '   into the array
        If Not Information.IsArray(vProductOptions) Then

            lReturn = retrieveProductOptions(vProductOptions)
        End If

        '   If product options have been found then will return value
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            lReturn = findValueInArray(v_vOptionNumber, v_vBranch, r_vUnderwriting, v_bValue)
        End If


        Return lReturn

    End Function

    ' ******************************************************'
    '
    ' Name: getProductOptionValue
    '
    ' Description: This provides a public interface for Product Options
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Public Function getProductOptionValue(ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String) As Integer

        Dim result As Integer = 0
        Try


            Return getProductOption(v_vOptionNumber, v_vBranch, r_vUnderwriting, True)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOptionValue", excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getUnderwritingOrAgency
    '
    ' Description: This provides a replacement for the legacy
    '               UnderwritingOrAgency interface
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Public Function getUnderwritingOrAgency(ByRef r_vUnderwriting As String) As Integer

        Dim result As Integer = 0
        Try

            r_vUnderwriting = "A"
            result = getProductOption(gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, gPMConstants.SIRBCHHeadOffice, r_vUnderwriting, True)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If (r_vUnderwriting <> "A") And (r_vUnderwriting <> "U") Then
                    r_vUnderwriting = "A"
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getUnderwritingOrAgency", excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getUnderwritingType
    '
    ' Description: This provides a replacement for the legacy
    '               UnderwritingType interface
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' RFC15102003 Moved from gSirLibrary as Part of the Global Data Changes
    '*******************************************************'
    Public Function getUnderwritingType(ByRef r_vUnderwriting As String) As Integer

        Dim result As Integer = 0
        Try

            r_vUnderwriting = "U"

            result = getProductOption(gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, gPMConstants.SIRBCHHeadOffice, r_vUnderwriting, False)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If (r_vUnderwriting <> "A") And (r_vUnderwriting <> "U") Then
                    r_vUnderwriting = "U"
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getUnderwritingType", excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: findValueInArray
    '
    ' Description: This will find the value in the product options array
    '
    ' History: 06/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function findValueInArray(ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String, ByVal v_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim i As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        r_vUnderwriting = ""

        '   Find the option Number within the array

        For i = 0 To vProductOptions.GetUpperBound(1)

            If CInt(vProductOptions(0, i)) = v_vOptionNumber Then

                If CInt(vProductOptions(1, i)) = v_vBranch Then
                    Exit For
                End If
            End If
        Next i

        '   If it was not found then find the option for the Head Office value

        If i > vProductOptions.GetUpperBound(1) Then
            v_vBranch = gPMConstants.SIRBCHHeadOffice

            For i = 0 To vProductOptions.GetUpperBound(1)

                If CInt(vProductOptions(0, i)) = v_vOptionNumber Then

                    If CInt(vProductOptions(1, i)) = v_vBranch Then
                        Exit For
                    End If
                End If
            Next i
        End If

        '   If found then get the value or UnderwritingType (only used by getUnderwritingType)

        Dim tempString As String = ""
        If i <= vProductOptions.GetUpperBound(1) Then
            If v_bValue Then

                tempString = CStr(vProductOptions(2, i))
            Else

                tempString = CStr(vProductOptions(3, i))
            End If

            If Not (Convert.IsDBNull(tempString) Or IsNothing(tempString)) Then
                r_vUnderwriting = tempString.Trim()
            End If
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetSystemOption
    '
    ' Description:  gets the system option required for the Source passed
    '               if v_iSourceID is ommitted then the branch will be taken from the global
    '               variable g_iSourceID
    '
    ' History: SW 07/04/2003
    '
    ' RFC15102003 Moved from gPMFunctions as Part of the Global Data Changes
    ' ***************************************************************** '
    Public Function GetSystemOption(ByRef v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef v_iSourceID As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oSystemOptions As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If RetrieveSingleSystemOption(v_iOptionNumber, r_sOptionValue, v_iSourceID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            ' sw 07/04/2003: Raise the error to the calling function so it can be logged correctly
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LogError
    '
    ' Description: Called by the Catch Error Handlers
    '
    ' v_sUsername       The current sirius username.
    ' v_sClass          The class in which the error is occurring
    ' v_sMethod         The function/sub in which the error is occurring
    ' r_lFunctionReturn The will reutn the standard sirius return value
    '                   depending on the type of error.
    '                       PMFalse for calls to other methods etc.
    '                       PMError for VB exceptions.
    '                   If a specific (non PMTrue) value has already been assigned
    '                   as the function return value then that value will be preserved.
    ' DJM 24/11/2005 : v_sUsername added as an optional parameter. It is not used in this function.
    '                  Was only added to allow this function to match the same function in bPMFunc.
    '                  Allows files to be shared between interface and business components (e.g. iPMBDocManager and bPMBDocManager).
    ' ***************************************************************** '
    Public Sub LogError(ByVal v_sClass As String, ByVal v_sMethod As String, ByRef r_lFunctionReturn As Integer, Optional ByVal v_sUsername As String = "")

        Dim eErrLevel As gPMConstants.PMELogLevel
        Dim sMsg As String = ""

        ' Grab the Error details first before they disappear.
        Dim llineNo As Integer = Information.Erl()
        Dim lErrNumber As gPMConstants.PMEReturnCode = Information.Err().Number
        v_sMethod = v_sMethod & Strings.Chr(13) & Strings.Chr(10) & "Line Number     : " & CStr(llineNo)
        Dim sErrDesc As String = Information.Err().Description & Strings.Chr(13) & Strings.Chr(10) & "Source          : " & Information.Err().Source
        Dim lLogLevel As Integer = Information.Err().HelpContext


        Try

            ' Subtract VBObjectError to see if its one of our errors or a VB Native one
            lErrNumber = CType(lErrNumber - Constants.vbObjectError, gPMConstants.PMEReturnCode)

            ' What sort of error?

            Select Case lErrNumber
                ' One of Ours
                Case gPMConstants.PMEReturnCode.PMBackOfficeError

                    ' If the Function already has a non PMTrue return value then leave it alone, otherwise set to PMFalse
                    If r_lFunctionReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        r_lFunctionReturn = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If lLogLevel > 0 Then
                        eErrLevel = lLogLevel
                    Else
                        eErrLevel = gPMConstants.PMELogLevel.PMLogDebug1
                    End If

                    sMsg = "Call to another method failed. See Error description for details."

                    ' VB Native
                Case Else

                    ' If the Function already has a non PMTrue return value then leave it alone, otherwise set to PMError
                    If r_lFunctionReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        r_lFunctionReturn = gPMConstants.PMEReturnCode.PMError
                    End If

                    eErrLevel = gPMConstants.PMELogLevel.PMLogOnError
                    sMsg = "Internal Exception. See Error description for details."

                    ' Add it back on so that we get a VB error number that we recognise e.g. 429
                    lErrNumber = CType(lErrNumber + Constants.vbObjectError, gPMConstants.PMEReturnCode)

            End Select

            LogMessage(iType:=eErrLevel, sMsg:=sMsg, vApp:=ACApp, vClass:=v_sClass, vMethod:=v_sMethod, vErrNo:=lErrNumber, vErrDesc:=sErrDesc)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: GetSystemSecurityModel
    '
    ' Description:  Returns the SystemSecurityModel from Product Option
    '
    ' ***************************************************************** '
    Public Function GetSystemSecurityModel(ByRef vValue As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SystemSecurityModel is NOT Branch specific.
            'It should always use the default Branch.

            m_lReturn = getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAlternativeLogon, v_vBranch:=1, r_vUnderwriting:=CStr(vValue))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemSecurityModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemSecurityModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function



    '**************************************************************
    'PURPOSE: Automatically size the combo box drop down width
    '         based on the width of the longest item in the combo box
    'JT       31/04/2005
    '**************************************************************
    Public Function AutoSizeDropDownComboWidth(ByRef Cbo As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim lmax_wid, lcur_wid, lsavemode As Integer

            With Cbo
                'Save Fom's ScaleMode

                lsavemode = .Parent.ScaleMode
                'Reset to vbPixels for TextWidth to work properly


                .Parent.ScaleMode = 3
                'Find longest member

                For i As Integer = 0 To .ListCount - 1


                    lcur_wid = .Parent.TextWidth(.List(i))
                    If lmax_wid < lcur_wid Then
                        lmax_wid = lcur_wid
                    End If
                Next i

                ' Set the width for the dropdown list, adding a margin.
                Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
                Try
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

                    m_lReturn = SendMessage(.hwnd, CB_SETDROPPEDWIDTH, lmax_wid + 10, tmpPtr)
                Finally
                    handle.Free()
                End Try
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                'restore form's ScaleMode

                .Parent.ScaleMode = lsavemode
            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoSizeDropDownComboWidth Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoSizeDropDownComboWidth", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SetComboBoxValue
    '
    ' Description: Sets the value of a ComboBox based on an items ItemData
    '
    ' oCombo            The ComboBox to populate.
    '                   Defined as object to allow use with embeded usercontrols
    ' sItemData         The ItemData value to look for.
    '                   Typically this would be the ID behind a description.
    ' ***************************************************************** '
    Public Function SetComboBoxValue(ByVal oCombo As Object, ByVal sItemData As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetComboBoxValue"
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Search for the appropriate value

            For lCount As Integer = 0 To CInt(oCombo.ListCount - 1)

                If sItemData = oCombo.ItemData(lCount) Then
                    ' Found it, set index and exit

                    oCombo.ListIndex = lCount
                    Return result
                End If
            Next

            ' Didn't find it, return false
            result = gPMConstants.PMEReturnCode.PMFalse

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        Finally
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: GetOptionValue
    '
    ' Description: Gets the value of the selected option button
    '
    ' oOptions          The optionbutton control collection to check.
    ' ***************************************************************** '
    Public Function GetOptionValue(ByVal oOptions As Object) As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "GetOptionValue"
        Dim lReturn As Integer

        Try

            result = -1

            ' Search for the appropriate value
            For Each oOption As RadioButton In oOptions
                If oOption.Checked Then
                    result = ContainerHelper.GetControlIndex(oOption)
                    Exit For
                End If
            Next oOption

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn)

        Finally
        End Try
        Return result
    End Function

    ' Moves the specified SSTab control to the next active tab
    Public Function SSTabMoveNext(ByVal oTab As Object) As Object


        Try


            Do


                If oTab.Tab >= oTab.Tabs - 1 Then

                    oTab.Tab = 0
                Else

                    oTab.Tab += 1
                End If
            Loop Until oTab.TabEnabled

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    ' Moves the specified SSTab control to the previous active tab
    Public Function SSTabMovePrevious(ByVal oTab As Object) As Object


        Try


            Do

                If oTab.Tab <= 0 Then


                    oTab.Tab = oTab.Tabs - 1
                Else

                    oTab.Tab -= 1
                End If
            Loop Until oTab.TabEnabled

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: GetGUID

    ' Description: Generate a Globaly Unique Identifier.
    '              Copied from Technet Article.
    ' RFC 11/08/00 - Get GUID function
    ' ***************************************************************** '
    Public Function GetGUID() As String
        '(c) 2000 Gus Molina


        Dim result As String = String.Empty
        Dim udtGUID As GUID = GUID.CreateInstance()

        Try

            result = ""

            If CoCreateGuid(udtGUID) = 0 Then
                result = New String("0", 8 - udtGUID.Data1.ToString("X").Length) & udtGUID.Data1.ToString("X") & _
                         New String("0", 4 - udtGUID.Data2.ToString("X").Length) & udtGUID.Data2.ToString("X") & _
                         New String("0", 4 - udtGUID.Data3.ToString("X").Length) & udtGUID.Data3.ToString("X") & _
                         (IIf((udtGUID.Data4(0) < &H10S), "0", "")) & udtGUID.Data4(0).ToString("X") & _
                         (IIf((udtGUID.Data4(1) < &H10S), "0", "")) & udtGUID.Data4(1).ToString("X") & _
                         (IIf((udtGUID.Data4(2) < &H10S), "0", "")) & udtGUID.Data4(2).ToString("X") & _
                         (IIf((udtGUID.Data4(3) < &H10S), "0", "")) & udtGUID.Data4(3).ToString("X") & _
                         (IIf((udtGUID.Data4(4) < &H10S), "0", "")) & udtGUID.Data4(4).ToString("X") & _
                         (IIf((udtGUID.Data4(5) < &H10S), "0", "")) & udtGUID.Data4(5).ToString("X") & _
                         (IIf((udtGUID.Data4(6) < &H10S), "0", "")) & udtGUID.Data4(6).ToString("X") & _
                         (IIf((udtGUID.Data4(7) < &H10S), "0", "")) & udtGUID.Data4(7).ToString("X")
            End If

            Return result

        Catch




            Return ""
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetTempPath

    ' Description: Get the local temp folder for the logged user
    ' ***************************************************************** '
    Public Function GetTempPath() As String

        Const knReturnLength As Integer = 260 ' MAX_PATH


        Dim sReturnValue As String = New String(Strings.Chr(0), knReturnLength)
        Dim nBytesCopied As Integer = GetTempPathAPI(knReturnLength, sReturnValue)

        If nBytesCopied > 0 Then
            sReturnValue = sReturnValue.Substring(0, nBytesCopied)
        Else
            sReturnValue = Nothing
        End If

        Return sReturnValue.Trim()

    End Function
End Module
