Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Public Module GIIFunctions
    '******************************************************************************
    ' GEMINI II SPECIFIC FUNCTIONS
    '******************************************************************************

    Private Const ACClass As String = "GIIFunctions"

    Public Const HelpContext As Integer = 1

    Public Enum CtlType
        ctlnone
        ctllabel
        ctlTextbox
        ctlcombobox
        ctlCheckbox
        ctlYesNoCheck
        ctlCommand
        ctluctDropdown
        ctlMSflexGrid
        ctlPictureBox
        ctlSSPanel
        ctlOptionButton
        ctlFrame
        ' 15/05/2001 PSA - Start
        ctlMEB
        ' 15/05/2001 PSA - End
    End Enum

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer

    Private Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer) As Integer

    Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal hwnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer

    Private Const GWL_STYLE As Integer = -16
    Private Const TVM_SETBKCOLOR As Integer = 4381
    Private Const TVM_GETBKCOLOR As Integer = 4383
    Private Const TVS_HASLINES As Integer = 2

    ' ***************************************************************** '
    '
    ' Name: SetTreeViewBackground
    '
    ' Description:
    '
    ' History: 17/01/2000 RT - Created.
    '
    ' ***************************************************************** '
    Public Sub SetTreeViewBackground(ByRef tvControl As Object, ByVal lColor As Integer)

        Dim lngStyle As Integer

        Try


            SendMessage(tvControl.hwnd, TVM_SETBKCOLOR, 0, lColor) 'Change the background

            ' Now reset the style so that the tree lines appear properly

            lngStyle = GetWindowLong(tvControl.hwnd, GWL_STYLE)


            SetWindowLong(tvControl.hwnd, GWL_STYLE, lngStyle - TVS_HASLINES)


            SetWindowLong(tvControl.hwnd, GWL_STYLE, lngStyle)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTreeViewBackground Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTreeViewBackground", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: FormShowInTaskBar
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '

    Public Sub FormShowInTaskBar(ByVal InHwnd As Integer, ByVal bState As Boolean)

        ' /* Minimize all the forms to Task Bar */

        'The trick is to mimic the keyboard events required to bring the Taskbar
        'popup menu and send it the letter "M" to select the "Minimize All Windows"
        'option. This is accomplished with three calls to the keybd_event API.
        '
        'The second argument for the keybd_event call is the hardware scan code,
        'and, in this case, you could use the value 91. However, because applications
        'should not use the scan code, it has been left as 0.
        ' 77 is the character code for the letter 'M'

        '    Call keybd_event(VK_LWIN, 0, 0, 0)
        '    Call keybd_event(77, 0, 0, 0)
        '    Call keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, 0)

        '   /* Code to Show in Task Bar */

        Dim lS As Integer = GetWindowLong(InHwnd, GWL_EXSTYLE)
        If bState Then
            lS = lS Or WS_EX_APPWINDOW
            lS = lS And Not WS_EX_TOOLWINDOW
        Else
            lS = lS And Not WS_EX_APPWINDOW
        End If

        Application.DoEvents()

        SetWindowLong(InHwnd, GWL_EXSTYLE, lS)

        '    Making the window to Show in Front

        iPMFunc.SetForegroundWindow(InHwnd)

        iPMFunc.SetFocusAPI(InHwnd)


    End Sub

    ' ***************************************************************** '
    '
    ' Name: SetScreenDisplay
    '
    ' Description:
    '
    ' History: 06/03/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function SetScreenDisplay(Optional ByVal ctl As Control = Nothing, Optional ByRef r_sFirstBackColour As String = "", Optional ByRef r_sSecondBackColour As String = "", Optional ByRef r_sThirdBackColour As String = "", Optional ByRef r_sFourthBackColour As String = "", Optional ByRef r_sMandatoryForeColour As String = "", Optional ByRef r_sOptionalForeColour As String = "", Optional ByRef r_sDisabledForeColour As String = "", Optional ByRef r_sDisabledForeColour2 As String = "", Optional ByRef r_sSecondDisabledForeColour As String = "", Optional ByRef r_sFontFamily As String = "", Optional ByRef r_sFontsize As String = "", Optional ByRef r_sFontWeight As String = "", Optional ByVal v_vControlType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lControlType As CtlType
        Dim lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            ' Get the control type

            If Not Information.IsNothing(v_vControlType) Then

                lControlType = CInt(v_vControlType)
            Else
                lReturn = ControlType(v_ctl:=ctl, r_lControlType:=lControlType)
            End If

            'retrive values from registry and pass them back

            ' Set screen values

            Select Case lControlType
                Case CtlType.ctllabel
                    'Get mandatory label colour vfrom the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="MandForeColour", r_sSettingValue:=r_sMandatoryForeColour, v_sSubKey:="DisplayValues\ctllabel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Mandatory text colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sMandatoryForeColour = "" Then

                        r_sMandatoryForeColour = CStr(Color.White)
                    End If

                    'Get Optional label colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="OptForeColour", r_sSettingValue:=r_sOptionalForeColour, v_sSubKey:="DisplayValues\ctllabel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Optional text colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sOptionalForeColour = "" Then

                        r_sOptionalForeColour = CStr(Color.White)
                    End If

                    'Get Disabled label colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DisForeColour", r_sSettingValue:=r_sDisabledForeColour, v_sSubKey:="DisplayValues\ctllabel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve disabled text colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sDisabledForeColour = "" Then
                        r_sDisabledForeColour = CStr(Color.White)
                    End If

                    'Get Second Disabled label colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DisForeColour2", r_sSettingValue:=r_sDisabledForeColour2, v_sSubKey:="DisplayValues\ctllabel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve second disabled text colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sDisabledForeColour2 = "" Then
                        r_sDisabledForeColour2 = CStr(Color.White)
                    End If

                Case CtlType.ctlYesNoCheck
                    'Get First colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", r_sSettingValue:=r_sFirstBackColour, v_sSubKey:="DisplayValues\ctlYesNoCheck")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve First YesNoCheck control Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sFirstBackColour = "" Then
                        r_sFirstBackColour = CStr(Color.LightBlue)
                    End If

                    'Get Second colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", r_sSettingValue:=r_sSecondBackColour, v_sSubKey:="DisplayValues\ctlYesNoCheck")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve second YesNoCheck control Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sSecondBackColour = "" Then
                        r_sSecondBackColour = CStr(Color.LightCyan)
                    End If

                Case CtlType.ctlCommand
                    'Get First colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", r_sSettingValue:=r_sFirstBackColour, v_sSubKey:="DisplayValues\ctlCommand")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve First Command button Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'Set default if there's nothing set in registry
                    If r_sFirstBackColour = "" Then
                        r_sFirstBackColour = CStr(Color.LightCyan)
                    End If

                    'Get Second background colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", r_sSettingValue:=r_sSecondBackColour, v_sSubKey:="DisplayValues\ctlCommand")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve second command button Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sSecondBackColour = "" Then
                        r_sSecondBackColour = CStr(Color.LightBlue)
                    End If

                    'Get Third background colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour3", r_sSettingValue:=r_sThirdBackColour, v_sSubKey:="DisplayValues\ctlCommand")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve third command button Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sThirdBackColour = "" Then
                        r_sThirdBackColour = CStr(Color.LightCyan)
                    End If

                Case CtlType.ctlPictureBox
                    'Get First colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", r_sSettingValue:=r_sFirstBackColour, v_sSubKey:="DisplayValues\ctlPictureBox")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve First picture box Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sFirstBackColour = "" Then
                        r_sFirstBackColour = CStr(Color.LightCyan)
                    End If

                    'Get Second background colour vfrom the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", r_sSettingValue:=r_sSecondBackColour, v_sSubKey:="DisplayValues\ctlPictureBox")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve second picture box Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sSecondBackColour = "" Then
                        r_sSecondBackColour = CStr(Color.LightBlue)
                    End If

                    'Get Third background colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour3", r_sSettingValue:=r_sThirdBackColour, v_sSubKey:="DisplayValues\ctlPictureBox")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve third picure box backgoround colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sThirdBackColour = "" Then
                        r_sThirdBackColour = ColorTranslator.ToOle(SystemColors.Window).ToString()
                    End If

                    'Get Fourth background colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour4", r_sSettingValue:=r_sFourthBackColour, v_sSubKey:="DisplayValues\ctlPictureBox")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Fourth backgoround colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sFourthBackColour = "" Then
                        r_sFourthBackColour = CStr(Color.LightCyan)
                    End If

                Case CtlType.ctlSSPanel, CtlType.ctlFrame
                    'Get First colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", r_sSettingValue:=r_sFirstBackColour, v_sSubKey:="DisplayValues\ctlSSPanel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve First Panel Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sFirstBackColour = "" Then
                        r_sFirstBackColour = CStr(Color.LightBlue)
                    End If

                    'Get Second background colour vfrom the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", r_sSettingValue:=r_sSecondBackColour, v_sSubKey:="DisplayValues\ctlSSPanel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve second Panel Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sSecondBackColour = "" Then
                        r_sSecondBackColour = CStr(Color.LightBlue)
                    End If

                    'Get Third background colour vfrom the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour3", r_sSettingValue:=r_sThirdBackColour, v_sSubKey:="DisplayValues\ctlSSPanel")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve third Panel Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sThirdBackColour = "" Then
                        r_sThirdBackColour = ColorTranslator.ToOle(SystemColors.Window).ToString()
                    End If

                Case CtlType.ctlCheckbox
                    'Get First colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", r_sSettingValue:=r_sFirstBackColour, v_sSubKey:="DisplayValues\ctlCheckbox")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve First Checkbox Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sFirstBackColour = "" Then
                        r_sFirstBackColour = CStr(Color.LightCyan)
                    End If

                Case CtlType.ctlOptionButton
                    'Get mandatory Fore-colour vfrom the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="MandForeColour", r_sSettingValue:=r_sMandatoryForeColour, v_sSubKey:="DisplayValues\ctlOptionButton")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Mandatory Fore-colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sMandatoryForeColour = "" Then

                        r_sMandatoryForeColour = CStr(Color.White)
                    End If

                    'Get Optional Fore-colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="OptForeColour", r_sSettingValue:=r_sOptionalForeColour, v_sSubKey:="DisplayValues\ctlOptionButton")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Optional Fore-colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sOptionalForeColour = "" Then

                        r_sOptionalForeColour = CStr(Color.White)
                    End If

                    'Get Disabled Fore-colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DisForeColour", r_sSettingValue:=r_sDisabledForeColour, v_sSubKey:="DisplayValues\ctlOptionButton")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve disabled Fore-colour of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sDisabledForeColour = "" Then

                        r_sDisabledForeColour = CStr(Color.White)
                    End If

                    'Get First back colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", r_sSettingValue:=r_sFirstBackColour, v_sSubKey:="DisplayValues\ctlOptionButton")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve First option button Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sFirstBackColour = "" Then
                        r_sFirstBackColour = CStr(Color.LightCyan)
                    End If

                    'Get Second back colour from the regsitry
                    lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", r_sSettingValue:=r_sSecondBackColour, v_sSubKey:="DisplayValues\ctlOptionButton")
                    'Check return
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve second option button Background colour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set default if there's nothing set in registry
                    If r_sSecondBackColour = "" Then
                        r_sSecondBackColour = CStr(Color.LightBlue)
                    End If

                Case CtlType.ctlcombobox, CtlType.ctluctDropdown, CtlType.ctlTextbox

                Case CtlType.ctlMSflexGrid


                Case Else
                    ' Do nothing a the moment

            End Select


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetScreenDisplay Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: StoreScreenDisplay
    '
    ' Description: Stores screen Display Values to the registry
    '
    ' History: 15/05/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function StoreScreenDisplay(Optional ByVal v_bIsGIIDefault As Boolean = False, Optional ByVal v_bIsWindowsDefault As Boolean = False, Optional ByVal v_sBackColourType As String = "", Optional ByVal v_sForeColourType As String = "", Optional ByVal v_sBackcolour1 As String = "", Optional ByVal v_sBackcolour2 As String = "", Optional ByVal v_sBackcolour3 As String = "", Optional ByVal v_sBackcolour4 As String = "", Optional ByVal v_sBackcolour5 As String = "", Optional ByVal v_sBackcolour6 As String = "", Optional ByVal v_sBackcolour7 As String = "", Optional ByVal v_sBackcolour8 As String = "", Optional ByVal v_sForecolour1 As String = "", Optional ByVal v_sForecolour2 As String = "", Optional ByVal v_sForecolour3 As String = "", Optional ByVal v_sForecolour4 As String = "", Optional ByVal v_sForecolour5 As String = "", Optional ByVal v_sForecolour6 As String = "", Optional ByVal v_sFont As String = "", Optional ByVal v_sFontsize As String = "", Optional ByVal v_bFontBold As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Backcolourtype
            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColourType", v_sSettingValue:=v_sBackColourType, v_sSubKey:="DisplayValues") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set BackColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Forecolourtype
            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ForeColourType", v_sSettingValue:=v_sForeColourType, v_sSubKey:="DisplayValues") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set ForeColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set Colours to the regsitry

            'Checkbox
            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Backcolour1", v_sSettingValue:=v_sBackcolour1, v_sSubKey:="DisplayValues\ctlCheckbox") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Command Button
            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Backcolour1", v_sSettingValue:=v_sBackcolour1, v_sSubKey:="DisplayValues\ctlCommand") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Backcolour2", v_sSettingValue:=v_sBackcolour3, v_sSubKey:="DisplayValues\ctlCommand") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Backcolour3", v_sSettingValue:=v_sBackcolour2, v_sSubKey:="DisplayValues\ctlCommand") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Label control

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="MandForecolour", v_sSettingValue:=v_sForecolour1, v_sSubKey:="DisplayValues\ctlLabel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="OptForecolour", v_sSettingValue:=v_sForecolour2, v_sSubKey:="DisplayValues\ctlLabel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DisForecolour", v_sSettingValue:=v_sForecolour3, v_sSubKey:="DisplayValues\ctlLabel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DisForecolour2", v_sSettingValue:=v_sForecolour4, v_sSubKey:="DisplayValues\ctlLabel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MSFlexGrid

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", v_sSettingValue:=v_sBackcolour2, v_sSubKey:="DisplayValues\ctlMSflexGrid") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Option Button

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DisForecolour", v_sSettingValue:=v_sForecolour3, v_sSubKey:="DisplayValues\ctloptionButton") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="MandForecolour", v_sSettingValue:=v_sForecolour1, v_sSubKey:="DisplayValues\ctloptionButton") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="OtpForecolour", v_sSettingValue:=v_sForecolour2, v_sSubKey:="DisplayValues\ctloptionButton") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", v_sSettingValue:=v_sBackcolour4, v_sSubKey:="DisplayValues\ctloptionButton") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", v_sSettingValue:=v_sBackcolour1, v_sSubKey:="DisplayValues\ctloptionButton") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Picture Box


            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", v_sSettingValue:=v_sBackcolour1, v_sSubKey:="DisplayValues\ctlPictureBox") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", v_sSettingValue:=v_sBackcolour2, v_sSubKey:="DisplayValues\ctlPictureBox") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour3", v_sSettingValue:=v_sBackcolour3, v_sSubKey:="DisplayValues\ctlPictureBox") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour4", v_sSettingValue:=v_sBackcolour4, v_sSubKey:="DisplayValues\ctlPictureBox") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Panel

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", v_sSettingValue:=v_sBackcolour4, v_sSubKey:="DisplayValues\ctlSSPanel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", v_sSettingValue:=v_sBackcolour5, v_sSubKey:="DisplayValues\ctlSSPanel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour3", v_sSettingValue:=v_sBackcolour3, v_sSubKey:="DisplayValues\ctlSSPanel") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Textbox

            'uctDropdown

            'YesNOCheckbox


            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour1", v_sSettingValue:=v_sBackcolour4, v_sSubKey:="DisplayValues\ctlYesNoCheck") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColour2", v_sSettingValue:=v_sBackcolour1, v_sSubKey:="DisplayValues\ctlYesNoCheck") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreScreenDisplay Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreScreenDisplay", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetFormSettings
    '
    ' Description:  Get Form settings from the registry
    '
    ' History: 30/05/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetFormSettings(Optional ByRef r_sBackColourType As String = "", Optional ByRef r_sForeColourType As String = "", Optional ByRef r_sBackcolour1 As String = "", Optional ByRef r_sBackcolour2 As String = "", Optional ByRef r_sBackcolour3 As String = "", Optional ByRef r_sBackcolour4 As String = "", Optional ByRef r_sBackcolour5 As String = "", Optional ByRef r_sBackcolour6 As String = "", Optional ByRef r_sBackcolour7 As String = "", Optional ByRef r_sBackcolour8 As String = "", Optional ByRef r_sForecolour1 As String = "", Optional ByRef r_sForecolour2 As String = "", Optional ByRef r_sForecolour3 As String = "", Optional ByRef r_sForecolour4 As String = "", Optional ByRef r_sForecolour5 As String = "", Optional ByRef r_sForecolour6 As String = "", Optional ByRef r_sFontFamily As String = "", Optional ByRef r_sFontsize As String = "", Optional ByRef r_bHighlightMandatory As Boolean = False, Optional ByRef r_bPromptInputMessage As Boolean = False, Optional ByRef r_sPFColourType As String = "", Optional ByRef r_sExcessColourType As String = "", Optional ByRef r_sPFBackColour As String = "", Optional ByRef r_sExcessBackColour As String = "") As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sHightlightMand, sPromptInput As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Background type
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColourType", r_sSettingValue:=r_sBackColourType, v_sSubKey:="DisplayValues") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get BackColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Foreground type
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ForeColourType", r_sSettingValue:=r_sForeColourType, v_sSubKey:="DisplayValues") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get ForeColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Quote Display PF Colour Type
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PFColourType", r_sSettingValue:=r_sPFColourType, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get PfColourType from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Quote Display Excess Colour Type
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ExcessColourType", r_sSettingValue:=r_sExcessColourType, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get ExcessColourType from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Quote Display PF Back Colour
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Scrn2_PF_Colour", r_sSettingValue:=r_sPFBackColour, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get PfBackColour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Quote Display Excess Back Colour
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Scrn2_Excess_Colour", r_sSettingValue:=r_sExcessBackColour, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get ExcessBackColour from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Set Colours to the regsitry

            'Backcolour
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour1", r_sSettingValue:=r_sBackcolour1, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour2", r_sSettingValue:=r_sBackcolour2, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour3", r_sSettingValue:=r_sBackcolour3, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour4", r_sSettingValue:=r_sBackcolour4, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour5", r_sSettingValue:=r_sBackcolour5, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour6", r_sSettingValue:=r_sBackcolour6, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour7", r_sSettingValue:=r_sBackcolour7, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour8", r_sSettingValue:=r_sBackcolour8, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Foreground
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour1", r_sSettingValue:=r_sForecolour1, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour2", r_sSettingValue:=r_sForecolour2, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour3", r_sSettingValue:=r_sForecolour3, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour4", r_sSettingValue:=r_sForecolour4, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour5", r_sSettingValue:=r_sForecolour5, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour6", r_sSettingValue:=r_sForecolour6, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="FontFamily", r_sSettingValue:=r_sFontFamily, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get font Family to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="FontSize", r_sSettingValue:=r_sFontsize, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Fontsize to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'r_bHighlightMandatory
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HighlightControl", r_sSettingValue:=sHightlightMand, v_sSubKey:="DisplayValues\HighLightMandatory") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get value for highlighting mandatory controls from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sHightlightMand <> "" Then
                r_bHighlightMandatory = (CInt(sHightlightMand) = 1)
            Else
                r_bHighlightMandatory = True
            End If

            'JSB 30102000 - Retrieve value for prompting for input, messages
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PromptMessage", r_sSettingValue:=sPromptInput, v_sSubKey:="DisplayValues\PromptInputMessage") <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get value for Prompting input messages from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sPromptInput <> "" Then
                r_bPromptInputMessage = (CInt(sPromptInput) = 1)
            Else
                r_bPromptInputMessage = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFormSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetFormSettings
    '
    ' Description: Store Form settings to the registry
    '
    ' History: 30/05/2000 JSB - Created.
    '
    '
    ' ***************************************************************** '
    Public Function SetFormSettings(Optional ByVal v_bIsGIIDefault As Boolean = False, Optional ByVal v_bIsWindowsDefault As Boolean = False, Optional ByVal v_sBackColourType As String = "", Optional ByVal v_sForeColourType As String = "", Optional ByVal v_sBackcolour1 As String = "&HEDC192", Optional ByVal v_sBackcolour2 As String = "&HCE9F66", Optional ByVal v_sBackcolour3 As String = "&H8000000F", Optional ByVal v_sBackcolour4 As String = "&HFA9A4B", Optional ByVal v_sBackcolour5 As String = "&HE49932", Optional ByVal v_sBackcolour6 As String = "&H80000010", Optional ByVal v_sBackcolour7 As String = "", Optional ByVal v_sBackcolour8 As String = "", Optional ByVal v_sForecolour1 As String = "&HFFFFFF", Optional ByVal v_sForecolour2 As String = "&HE0E0E0", Optional ByVal v_sForecolour3 As String = "&HC0C0C0", Optional ByVal v_sForecolour4 As String = "&H808080", Optional ByVal v_sForecolour5 As String = "", Optional ByVal v_sForecolour6 As String = "", Optional ByVal v_sFontFamily As String = "Verdana", Optional ByVal v_sFontsize As String = "8", Optional ByVal v_bHighlightMandatory As Boolean = True, Optional ByVal v_bPromptInputMessage As Boolean = True, Optional ByVal v_sPFColourType As String = "", Optional ByVal v_sExcessColourType As String = "", Optional ByVal v_sPFBackColour As String = "", Optional ByVal v_sExcessBackColour As String = "") As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Background type
            If v_sBackColourType <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="BackColourType", v_sSettingValue:=v_sBackColourType, v_sSubKey:="DisplayValues") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set BackColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Foreground type
            If v_sForeColourType <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ForeColourType", v_sSettingValue:=v_sForeColourType, v_sSubKey:="DisplayValues") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set ForeColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Quote Display PF Colour Type
            If v_sPFColourType <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PFColourType", v_sSettingValue:=v_sPFColourType, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set PFColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Quote Display Excess Colour Type
            If v_sExcessColourType <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ExcessColourType", v_sSettingValue:=v_sExcessColourType, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set ExcessColourType to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Quote Display PF Back Colour
            If v_sPFBackColour <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Scrn2_PF_Colour", v_sSettingValue:=v_sPFBackColour, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set PFBackColour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Quote Display Excess Back Colour
            If v_sExcessBackColour <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Scrn2_Excess_Colour", v_sSettingValue:=v_sExcessBackColour, v_sSubKey:="GII Quote Display") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set ExcessBackColour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Set Colours to the regsitry

            'Backcolour
            If v_sBackcolour1 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour1", v_sSettingValue:=v_sBackcolour1, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour2 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour2", v_sSettingValue:=v_sBackcolour2, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour3 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour3", v_sSettingValue:=v_sBackcolour3, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour4 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour4", v_sSettingValue:=v_sBackcolour4, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour5 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour5", v_sSettingValue:=v_sBackcolour5, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour6 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour6", v_sSettingValue:=v_sBackcolour6, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour7 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour7", v_sSettingValue:=v_sBackcolour7, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sBackcolour8 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour8", v_sSettingValue:=v_sBackcolour8, v_sSubKey:="DisplayValues\Background") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Foreground
            If v_sForecolour1 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour1", v_sSettingValue:=v_sForecolour1, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sForecolour2 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour2", v_sSettingValue:=v_sForecolour2, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sForecolour3 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour3", v_sSettingValue:=v_sForecolour3, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sForecolour4 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour4", v_sSettingValue:=v_sForecolour4, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sForecolour5 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour5", v_sSettingValue:=v_sForecolour5, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sForecolour6 <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Colour6", v_sSettingValue:=v_sForecolour6, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Colour to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sFontFamily <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="FontFamily", v_sSettingValue:=v_sFontFamily, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set font to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_sFontsize <> "" Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="FontSize", v_sSettingValue:=v_sFontsize, v_sSubKey:="DisplayValues\Foreground") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Fontsize to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Entry for highlighting mandatory controls
            If v_bHighlightMandatory Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HighlightControl", v_sSettingValue:="1", v_sSubKey:="DisplayValues\HighLightMandatory") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Mandatory Property to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HighlightControl", v_sSettingValue:="0", v_sSubKey:="DisplayValues\HighLightMandatory") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Mandatory Property to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'JSB30102000 - entry for prompting input messages
            If v_bPromptInputMessage Then
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PromptMessage", v_sSettingValue:="1", v_sSubKey:="DisplayValues\PromptInputMessage") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set PromptInputMessage Property to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PromptMessage", v_sSettingValue:="0", v_sSubKey:="DisplayValues\PromptInputMessage") <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set PromptInputMessage Property to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFormSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateDateField
    '
    ' Description: Validate the date in the textbox passed through
    '
    ' History: 30/11/1999 JSB - Created.
    '          31/03/2000 BB - Added Date rang validation, optional to and from
    ' dates may be passed either as a textbox or a date value or "Date" as in today's date
    '          23/05/2000 JSB  - Function amended to remove 'exit function' statements
    '                            from 'With' statement, to prevent memory errors.
    '
    ' ***************************************************************** '
    Public Function ValidateDateField(ByRef TxtDate As Control, Optional ByVal v_vFromDate As Object = Nothing, Optional ByVal v_vToDate As Object = Nothing, Optional ByVal v_vItemname As String = "") As Integer

        Dim result As Integer = 0
        Dim dtThisDate, dtFromDate, dtToDate As Date
        Dim bValidDate As Boolean
        Dim sItemName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bValidDate = True

            'sj 13/6/2000 - Get the correct name for error messaging

            If Not Information.IsNothing(v_vItemname) Then
                sItemName = v_vItemname
            ElseIf Convert.ToString(ControlHelper.GetTag(TxtDate)) <> "" Then
                sItemName = Convert.ToString(ControlHelper.GetTag(TxtDate))
            Else
                sItemName = "Item"
            End If

            ' Nothing entered so exit funtion

            If CInt(CStr(TxtDate.Text = "").Trim()) Or TxtDate.Text = GEMShortDate Then
                TxtDate.Text = ""
                'sj 31/05/2000 - start
                ' Allow date to be empty
                'ValidateDateField = PMFalse
                'sj 31/05/2000 - end
                Return result
            End If

            With TxtDate
                ' If no date has been entered

                Dim dbNumericTemp As Double
                If .Text = GEMShortDate Then
                    .Text = ""
                    ' Assume digits only is a year
                ElseIf (Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                    .Text = "01/01/" & .Text
                End If

                ' If we have a date Convert date to long format on LostFocus

                If Information.IsDate(.Text) Then

                    dtThisDate = .Text

                    .Text = StringsHelper.Format(.Text, GEMLongDate)

                    '        ' Nothing entered
                    '        ElseIf .Text = "" Then
                    '            Exit Function

                    ' Not a valid date so display prompt
                Else
                    bValidDate = False

                    '            If TxtDate.Tag = "" Then
                    '                TxtDate.Tag = "Item"
                    '            End If
                    '            'Prompt user as to the field that needs populating
                    '            MsgBox TxtDate.Tag & " requires a Valid Date Entry", vbCritical + vbOKOnly, TxtDate.Tag
                    '            .Text = ""
                    '            .SetFocus
                    '            Exit Function
                End If

            End With

            If Not bValidDate Then

                '        If TxtDate.Tag = "" Then
                '            TxtDate.Tag = "Item"
                '        End If
                'Prompt user as to the field that needs populating
                MessageBox.Show(sItemName & " requires a Valid Date Entry", sItemName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                TxtDate.Text = ""
                TxtDate.Focus()
                Return result

            End If

            ' At this point we should have a valid date

            ' Check to see if we have any From Date restriction passed

            If Not Information.IsNothing(v_vFromDate) Then
                If Information.IsDate(v_vFromDate) Then

                    dtFromDate = CDate(v_vFromDate)
                    ' We have a From Date compare it with date entered
                    If dtThisDate < dtFromDate Then
                        'Prompt user that date is too early
                        MessageBox.Show(sItemName & " must not be before " & DateTimeHelper.ToString(dtFromDate), sItemName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        TxtDate.Text = ""
                        TxtDate.Focus()
                        Return result
                    End If
                End If
            End If

            ' Check to see if we have any To Date restriction passed

            If Not Information.IsNothing(v_vToDate) Then
                If Information.IsDate(v_vToDate) Then

                    dtToDate = CDate(v_vToDate)
                    ' We have a To Date compare it with date entered
                    If dtThisDate > dtToDate Then
                        'Prompt user that date is too late
                        MessageBox.Show(sItemName & " must not be after " & DateTimeHelper.ToString(dtToDate), sItemName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        TxtDate.Text = ""
                        TxtDate.Focus()
                        Return result
                    End If
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateDateField Failed to validate[" & TxtDate.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateDateField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateCardDateField
    '
    ' Description: Validate the date in the textbox passed through
    '
    ' History: 26/04/2001 PSA - Created.
    ' ***************************************************************** '
    Public Function ValidateCardDateField(ByVal TxtDate As Control, Optional ByVal v_vItemname As String = "") As Integer

        'Dim dtThisDate As Date
        'Dim dtFromDate As Date
        'Dim dtToDate As Date
        Dim result As Integer = 0
        Dim bValidDate As Boolean
        Dim sItemName As String = ""

        Const GEMCardDate As String = "MM/YY"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bValidDate = True

            ' Get the correct name for error messaging

            If Not Information.IsNothing(v_vItemname) Then
                sItemName = v_vItemname
            ElseIf Convert.ToString(ControlHelper.GetTag(TxtDate)) <> "" Then
                sItemName = Convert.ToString(ControlHelper.GetTag(TxtDate))
            Else
                sItemName = "Item"
            End If

            ' Nothing entered so exit funtion

            If CBool(CStr(TxtDate.Text = "__/__").Trim()) Then
                Return result
            End If


            If TxtDate.Text = GEMCardDate Then
                TxtDate.Text = ""
                Return result
            End If

            With TxtDate
                ' If no date has been entered

                If .Text = GEMCardDate Then
                    .Text = ""
                    ' Assume digits only is a year
                ElseIf (Strings.Len(.Text) > 5) Then
                    bValidDate = False
                End If

                ' Check date is valid

                If Not Information.IsDate(.Text) Then
                    bValidDate = False
                End If

            End With

            If Not bValidDate Then

                'Prompt user as to the field that needs populating
                MessageBox.Show(sItemName & " requires a Valid Date Entry", sItemName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                TxtDate.Mask = ""
                TxtDate.Text = "MM/YY"
                TxtDate.Focus()
                Return result

            End If

            ' At this point we should have a valid date

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateCardDateField Failed to validate[" & TxtDate.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateCardDateField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateTimeField
    '
    ' Description:
    '
    ' History: 02/02/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function ValidateTimeField(ByRef TxtTime As TextBox) As Integer

        Dim result As Integer = 0
        Try



            '    With TxtTime
            '
            '        'Do checks
            '        if()then
            '
            '        'if it's not then show error message
            '        ElseIf .Text <> "" Then
            '            If TxtTime.Tag = "" Then
            '                TxtTime.Tag = "Item"
            '            End If
            '            'Prompt user as to the field that needs populating
            '            MsgBox TxtTime.Tag & " requires a Valid time Entry", vbCritical + vbOKOnly, TxtTime.Tag
            '            .Text = ""
            '            .SetFocus
            '        End If
            '
            '    End With
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateTimeField Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateTimeField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateNumericField
    '
    ' Description: Validate the number in the textbox passed through
    '
    ' History: 30/11/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function ValidateNumericField(ByRef TxtNumber As TextBox) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With TxtNumber
                ' If entry not numeric
                Dim dbNumericTemp As Double
                If (Not Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And .Text <> "" Then
                    'BB061299 If no Tag entry use "Item"
                    If Convert.ToString(TxtNumber.Tag) = "" Then
                        TxtNumber.Tag = "Item"
                    End If
                    'Prompt user as to the field that needs populating asuming the tag of offending textbox contains the name
                    MessageBox.Show(Convert.ToString(TxtNumber.Tag) & " requires a Numeric Entry", Convert.ToString(TxtNumber.Tag), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    .Text = ""
                    .Focus()
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateNumericField Failed to validate[" & TxtNumber.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateNumericField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateListField
    '
    ' Description:
    '
    ' History: 03/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function ValidateListField(ByRef ddList As Control) As Integer

        Dim result As Integer = 0
        Dim sText As String = ""
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result ' TODO: deepak added correct the error
            bFound = False

            ' hold the current value

            sText = ddList.Text.ToUpper()

            ' Loop through the list

            For lCount As Integer = 0 To CInt(ddList.ListCount - 1)

                ' See if the value matches the list item

                If ddList.List(lCount).ToUpper() = sText Then
                    ' If so, set the value to the list item

                    ddList.Text = ddList.List(lCount)
                    bFound = True
                End If

            Next

            If Not bFound Then
                ddList.Text = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateListField Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateListField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidatePostcodeFormat
    '
    ' Description: Validate postcode for correct format
    '
    ' Postcode formats:         X9 9XX
    '                           X99 9XX
    '                           XX9 9XX
    '                           XX99 9XX
    '                           XX9X 9XX
    '
    ' History: 08/02/2000 JSB - Created.
    '          18/10/2000 JSB - Also validate format - X9X 9XX
    '
    ' ***************************************************************** '
    Public Function ValidatePostcodeFormat(ByVal ctlPostcodeControl As Control, Optional ByRef r_sPolarisPCode As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sPolarisPCode As String = ""
        Dim lPolarisPCodeLen As Integer
        Dim sPCodeTemp As String = ""
        Dim lPCodeTempLen As Integer

		On Error GoTo Err_ValidatePostcodeFormat

        result = gPMConstants.PMEReturnCode.PMTrue

        With ctlPostcodeControl
            'Make it uppercase

            .Text = .Text.ToUpper()
            'Check that the length of the postcode is between 5 and 7 characters

            If CStr(.Text).IndexOf(" "c) + 1 Then

                If (Strings.Len(.Text) <= 5) Or (Strings.Len(.Text) > 8) Then
                    If Convert.ToString(ControlHelper.GetTag(ctlPostcodeControl)) = "" Then
                        ControlHelper.SetTag(ctlPostcodeControl, "Postcode")
                    End If
                    GoTo Err_IncorrectPostcode
                End If
            Else

                If (Strings.Len(.Text) <= 4) Or (Strings.Len(.Text) >= 8) Then
                    If Convert.ToString(ControlHelper.GetTag(ctlPostcodeControl)) = "" Then
                        ControlHelper.SetTag(ctlPostcodeControl, "Postcode")
                    End If
                    GoTo Err_IncorrectPostcode
                End If
            End If

            'Check that the last character is a letter

            If .Text.Trim().Substring(.Text.Trim().Length - 1) Like "[A-Z]" Then
                'postcode ok so far
            Else
                GoTo Err_IncorrectPostcode
            End If


            lPCodeTempLen = Strings.Len(.Text) - 1


            sPCodeTemp = .Text.Substring(0, lPCodeTempLen)

            'Check that the second last character is a letter

            If .Text.Trim().Substring(.Text.Trim().Length - 1) Like "[A-Z]" Then
                'postcode ok so far
            Else
                GoTo Err_IncorrectPostcode
            End If

            'Get the length of the remaining postcode
            lPCodeTempLen -= 1

            'Get the remaining postcode

            sPCodeTemp = .Text.Substring(0, lPCodeTempLen)

            'Set these for comparison later against polaris list
            lPolarisPCodeLen = lPCodeTempLen - 1


            If CStr(.Text).IndexOf(" "c) + 1 Then

                sPolarisPCode = CStr(.Text).Substring(0, Math.Min(.Text.Length, (CStr(.Text).IndexOf(" "c) + 1) + 1))
            Else
                sPolarisPCode = sPCodeTemp.Substring(0, lPolarisPCodeLen) & " " & sPCodeTemp.Substring(sPCodeTemp.Length - 1)
            End If

            'Check that the last character of the remaining postcode is numeric
            If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[0-9]" Then
                'postcode ok so far
            Else
                GoTo Err_IncorrectPostcode
            End If

            'Get the length of the remaining postcode
            lPCodeTempLen -= 1

            'Get the remaining postcode

            sPCodeTemp = .Text.Substring(0, lPCodeTempLen)

            'If a space has been enter this where it should be
            If sPCodeTemp.EndsWith(" ") Then
                'Get the length of the remaining postcode
                lPCodeTempLen -= 1

                'Get the remaining postcode

                sPCodeTemp = .Text.Substring(0, lPCodeTempLen)
            Else
                'do nothing, everything's fine
            End If

            'remaining postcode must be at atleast 2 characters
            If lPCodeTempLen < 2 Then
                GoTo Err_IncorrectPostcode
                'If there are only 2 character left
            ElseIf (lPCodeTempLen = 2) Then
                'Check that the last character of the remaining postcode is numeric
                If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[0-9]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                'Check that the first character is a letter
                If sPCodeTemp.Trim().Substring(0, 1) Like "[A-Z]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                'Insert space

                .Text = .Text.Trim().Substring(0, 2) & " " & .Text.Trim().Substring(.Text.Trim().Length - 3)
                'If there are 3 character left
            ElseIf (lPCodeTempLen = 3) Then
                'JSB18102000 - changed to allow for postcodes starting X9X
                '            'Check that the last character of the remaining postcode is numeric
                '            If (Right(Trim$(sPCodeTemp), 1) Like "[0-9]") Then
                'Check that the last character of the remaining postcode is a letter or number
                If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[A-Z]" Or sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[0-9]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                lPCodeTempLen -= 1

                'Get the remaining postcode

                sPCodeTemp = .Text.Substring(0, lPCodeTempLen)

                'Check that the last character of the remaining postcode is a letter or number
                If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[A-Z]" Or sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[0-9]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                'Check that the first character is a letter
                If sPCodeTemp.Trim().Substring(0, 1) Like "[A-Z]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                'Insert space

                .Text = .Text.Trim().Substring(0, 3) & " " & .Text.Trim().Substring(.Text.Trim().Length - 3)
                'If there are 4 character left
            ElseIf (lPCodeTempLen = 4) Then
                'Check that the last character of the remaining postcode is a letter or number
                If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[A-Z]" Or sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[0-9]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                lPCodeTempLen -= 1

                'Get the remaining postcode

                sPCodeTemp = .Text.Substring(0, lPCodeTempLen)

                'Check that the last character of the remaining postcode is a letter or number
                If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[0-9]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                lPCodeTempLen -= 1

                'Get the remaining postcode

                sPCodeTemp = .Text.Substring(0, lPCodeTempLen)

                'Check that the last character of the remaining postcode is a letter or number
                If sPCodeTemp.Trim().Substring(sPCodeTemp.Trim().Length - 1) Like "[A-Z]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                'Check that the first character is a letter
                If sPCodeTemp.Trim().Substring(0, 1) Like "[A-Z]" Then
                    'postcode ok so far
                Else
                    GoTo Err_IncorrectPostcode
                End If

                'Insert space

                .Text = .Text.Trim().Substring(0, 4) & " " & .Text.Trim().Substring(.Text.Trim().Length - 3)
            Else
                GoTo Err_IncorrectPostcode
            End If

        End With

        ' Set Polaris postcode to return
        r_sPolarisPCode = sPolarisPCode

        Return result

Err_IncorrectPostcode:

        If Convert.ToString(ControlHelper.GetTag(ctlPostcodeControl)) = "" Then
            ControlHelper.SetTag(ctlPostcodeControl, "Postcode")
        End If

        MessageBox.Show(ctlPostcodeControl.Text & " is an incorrect postcode format,", Convert.ToString(ControlHelper.GetTag(ctlPostcodeControl)), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        ctlPostcodeControl.Focus()

        Return result

Err_ValidatePostcodeFormat:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePostcodeFormat Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePostcodeFormat", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    '************************************************************************************************
    ' Name             :   HighlightControl
    ' Created by       :   Ram Chandrabose
    ' Date             :   29-Oct-1999
    ' Function for     :   Highlight the contents of the control
    ' Called from      :   Control's Got_Focus Event
    ' Input Parameters :   1.  Ctl              - Control
    '                      2.  optBoolDateField - Boolean  ( Optional Parameter )
    '                               if True     - Set the Control with 'DD/MM/YYYY' as default value.
    ' Edit History     :
    '*************************************************************************************************
    Public Function HighlightContol(ByRef ctl As Control, Optional ByRef optBoolDateField As Boolean = False, Optional ByRef optBoolDropDown As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            With ctl

                ' For date fields only
                If Not False Then
                    If optBoolDateField Then
                        ' Convert date to short format on GotFocus

                        If Information.IsDate(.Text) Then

                            .Text = StringsHelper.Format(.Text, GEMShortDate)
                        Else
                            .Text = GEMShortDate
                        End If
                    End If
                End If

                ' Highlight the contents of the control

                .SelStart = 0


                .SelLength = Strings.Len(.Text)
            End With

            ' To Make explicit Drop Down using API
            If Not False Then
                If optBoolDropDown Then
                    SendMessage(ctl.Handle.ToInt32(), CB_SHOWDROPDOWN, True, 0)
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in HightlightControl [" & ctl.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="HighlightContol", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetYesNoValue
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetYesNoValue(ByVal sValue As String) As Integer

        Dim result As Integer = 0
        Try

            Select Case sValue
                Case ""
                    result = GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckNone
                Case "No"
                    result = GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckNo
                Case "Yes"
                    result = GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckYes
            End Select

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetYesNoValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetYesNoValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetYesNoValueString
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetYesNoValueString(ByVal iValue As Integer) As String

        Dim result As String = String.Empty
        Try

            Select Case iValue
                Case GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckNone
                    result = ""
                Case GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckNo
                    result = "No"
                Case GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckYes
                    result = "Yes"
            End Select

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetYesNoValueStringFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetYesNoValueString", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCodeValue
    '
    ' Description: Populates the Risk Explorer treeview
    '
    '
    ' ***************************************************************** '
    Public Function GetCodeValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sDesc As String, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sDesc, sABICode As String
        Dim vListData, vListDataCodes, vSearch As Object
        Dim iCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sCode = ""

            '    lReturn& = g_oGis.GetListAndCodes(v_sObjectName, v_sPropertyName, vListData, vListDataCodes, v_sDesc)
            If v_sDesc <> "" Then

                lReturn = g_oGis.GetABICodeFromDescription(v_sObjectName, v_sPropertyName, r_sCode, v_sDesc)
            End If
            '    If IsArray(vListDataCodes) Then
            '        If UBound(vListDataCodes) > 0 Then
            '            For iCnt% = LBound(vListData) To UBound(vListData)
            '                If vListData(iCnt%) = v_sDesc$ Then
            '                    r_sCode$ = vListDataCodes(iCnt%)
            '                End If
            '            Next iCnt%
            '        Else
            '            r_sCode$ = vListDataCodes(0)
            '        End If
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCodeValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: ControlType
    '
    ' Description: Returns as string containing the Name of the control type
    '
    ' History: 06/12/1999 JSB - Created.
    '          15/05/2001 PSA - Added Masked Edit Box
    '
    ' ***************************************************************** '
    Public Function ControlType(ByVal v_ctl As Control, ByRef r_lControlType As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'IJR 2003-03-05 Start
            '    If (TypeOf v_ctl Is Label) Then
            '        r_lControlType& = ctllabel
            '    ElseIf (TypeOf v_ctl Is TextBox) Then
            '        r_lControlType& = ctlTextbox
            '    ElseIf (TypeOf v_ctl Is ComboBox) Then
            '        r_lControlType& = ctlcombobox
            '    ElseIf (TypeOf v_ctl Is CheckBox) Then
            '        r_lControlType& = ctlCheckbox
            '    ElseIf (TypeOf v_ctl Is YesNoCheck) Then
            '        r_lControlType& = ctlYesNoCheck
            '    ElseIf (TypeOf v_ctl Is CommandButton) Then
            '        r_lControlType& = ctlCommand
            '    ElseIf (TypeOf v_ctl Is uctDropdown) Then
            '        r_lControlType& = ctluctDropdown
            '    ElseIf (TypeOf v_ctl Is MSFlexGrid) Then
            '        r_lControlType& = ctlMSflexGrid
            '    ElseIf (TypeOf v_ctl Is PictureBox) Then
            '        r_lControlType& = ctlPictureBox
            '    ElseIf (TypeOf v_ctl Is SSPanel) Then
            '        r_lControlType& = ctlSSPanel
            '    ElseIf (TypeOf v_ctl Is OptionButton) Then
            '        r_lControlType& = ctlOptionButton
            '    ElseIf (TypeOf v_ctl Is Frame) Then
            '        r_lControlType& = ctlFrame
            '    ' 15/05/2001 PSA - Start
            '    ElseIf (TypeOf v_ctl Is MaskEdBox) Then
            '        r_lControlType& = ctlMEB
            '    ' 15/05/2001 PSA - End
            '    Else
            '        r_lControlType = ctlnone
            '    End If

            Select Case v_ctl.GetType().Name
                ' Standard VB Controls
                Case "Label"
                    r_lControlType = CtlType.ctllabel
                Case "TextBox"
                    r_lControlType = CtlType.ctlTextbox
                Case "ComboBox"
                    r_lControlType = CtlType.ctlcombobox
                Case "CheckBox"
                    r_lControlType = CtlType.ctlCheckbox
                Case "OptionButton"
                    r_lControlType = CtlType.ctlOptionButton
                Case "CommandButton"
                    r_lControlType = CtlType.ctlCommand
                Case "PictureBox"
                    r_lControlType = CtlType.ctlPictureBox
                Case "Frame"
                    r_lControlType = CtlType.ctlFrame

                    ' Sirius UserControls
                Case "YesNoCheck"
                    r_lControlType = CtlType.ctlYesNoCheck
                Case "uctDropdown"
                    r_lControlType = CtlType.ctluctDropdown

                    ' Microsoft Common Controls
                Case "IMSFlexGrid"
                    r_lControlType = CtlType.ctlMSflexGrid
                Case "ISSPNCtrl", "SSPanel"


                    r_lControlType = CtlType.ctlSSPanel
                Case "IMSMask"





                    r_lControlType = CtlType.ctlMEB

                    ' Unknown
                Case Else
                    r_lControlType = CtlType.ctlnone
            End Select
            'IJR 2003-03-05 End
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ControlType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ControlType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckMandatoryControl
    '
    ' Description: Used to check for data entry in the passed control and display
    '              suitable prompt if entry required and set focus to control.
    '
    ' History: 06/12/1999 JSB - Created.
    '          11/01/2000 BB - Updated
    '          31/10/2000 JSB - Amended, so that the focus is still set to the control
    '                           even when the message box is not displayed
    '          15/05/2001 PSA - Added Masked Edit Box
    '
    ' ***************************************************************** '
    Public Function CheckMandatoryControl(ByVal ctl As Control, ByRef bMandatoryPrompted As Boolean, Optional ByVal v_bDisplayErrorMessage As Boolean = True) As Integer
        Dim result As Integer = 0
        Dim lControlType As CtlType
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim ctlLabelControl As Control
        Dim sLabelCaption, sFieldName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bMandatoryPrompted = False

            ' Get the control type
            lReturn = CType(ControlType(v_ctl:=ctl, r_lControlType:=lControlType), gPMConstants.PMEReturnCode)

            ' Test control data input based on control type

            Select Case lControlType
                Case CtlType.ctlTextbox, CtlType.ctluctDropdown, CtlType.ctlcombobox, CtlType.ctlMEB

                    If ctl.Text.Trim() = "" Then
                        lReturn = CType(PromptForInput(v_ctlDataControl:=ctl, r_sPromptText:=" requires an Entry", r_bMandatoryPrompted:=bMandatoryPrompted, v_bDisplayInputMessage:=v_bDisplayErrorMessage), gPMConstants.PMEReturnCode)
                    End If

                Case CtlType.ctlCheckbox

                    'If (ctl.Value = System.Windows.Forms.CheckState.UnChecked) Then
                    If (DirectCast(ctl, CheckBox).CheckState = System.Windows.Forms.CheckState.Unchecked) Then
                        lReturn = CType(PromptForInput(v_ctlDataControl:=ctl, r_sPromptText:=" needs to be checked", r_bMandatoryPrompted:=bMandatoryPrompted, v_bDisplayInputMessage:=v_bDisplayErrorMessage), gPMConstants.PMEReturnCode)
                    End If

                Case CtlType.ctlYesNoCheck

                    If ctl.Value = GEMControlLib.YesNoCheck.YesNoCheckValues.YesNoCheckNone Then
                        lReturn = CType(PromptForInput(v_ctlDataControl:=ctl, r_sPromptText:=" requires a 'Yes' or 'No' answer", r_bMandatoryPrompted:=bMandatoryPrompted, v_bDisplayInputMessage:=v_bDisplayErrorMessage), gPMConstants.PMEReturnCode)
                    End If

                Case CtlType.ctlMSflexGrid

                    If ctl.Rows < 2 Then
                        lReturn = CType(PromptForInput(v_ctlDataControl:=ctl, r_sPromptText:=" requires at least one Entry", r_bMandatoryPrompted:=bMandatoryPrompted, v_bDisplayInputMessage:=v_bDisplayErrorMessage), gPMConstants.PMEReturnCode)
                    End If

                Case CtlType.ctlCommand

                    ' 15/05/2001 PSA - Start
                Case Else
                    ' Do nothing a the moment

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatoryControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatoryControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PromptForInput
    '
    ' Description: Displays a Message Box detailing the Item to be completed
    '              and sets focus to the required control
    '
    ' History: 11/01/2000 BB - Created.
    '
    ' ***************************************************************** '
    Public Function PromptForInput(ByVal v_ctlDataControl As Control, ByRef r_sPromptText As String, ByRef r_bMandatoryPrompted As Boolean, ByVal v_bDisplayInputMessage As Boolean) As Integer
        Dim result As Integer = 0
        Dim sFieldPrompt, sFieldName As String
        Dim ctlLabelControl As Control

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bMandatoryPrompted = False

            ' Check the control is enabled
            If Not ControlHelper.GetEnabled(v_ctlDataControl) Then
                Return result
            End If

            'JSB31102000 - Check that message needs to be displayed
            If v_bDisplayInputMessage Then
                ' Get the descriptive name of the input field
                ' If we have a non numeric Tag entry use that
                Dim dbNumericTemp As Double
                If Convert.ToString(ControlHelper.GetTag(v_ctlDataControl)) <> "" And Not Double.TryParse(Convert.ToString(ControlHelper.GetTag(v_ctlDataControl)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    sFieldName = Convert.ToString(ControlHelper.GetTag(v_ctlDataControl))

                    ' Nothing useful in the tag so try to find a matching label to use
                Else
                    m_lReturn = CType(FindMatchingLabel(v_ctlDataControl:=v_ctlDataControl, r_ctlLabelControl:=ctlLabelControl, r_sLabelCaption:=sFieldName), gPMConstants.PMEReturnCode)

                    ' Nothing found so use default text
                    If sFieldName = "" Then
                        sFieldName = "Input Field"
                    End If

                End If

                ' Prompt is combination of name and prompt text
                sFieldPrompt = sFieldName & r_sPromptText

                'Prompt user as to the mandatory field that needs populating
                MessageBox.Show(sFieldPrompt, "Mandatory Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            End If

            r_bMandatoryPrompted = True

            ' If the control is visible set focus to it
            If ControlHelper.GetVisible(v_ctlDataControl) Then

                v_ctlDataControl.Focus()

                ' If not, make it so
            Else

                ' Find the right Tab and make it visible
                m_lReturn = CType(FindOnTab(v_ctlDataControl), gPMConstants.PMEReturnCode)

                ' Then set focus
                If ControlHelper.GetVisible(v_ctlDataControl) Then
                    v_ctlDataControl.Focus()
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PromptForInput Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PromptForInput", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindMatchingLabel
    '
    ' Description: Find label for a given field control, with same name as
    ' the field but with a prefix of lbl
    '
    ' This routine relies on the Data control and corresponding Label having matching names
    ' eg txtDataName lblDataName. If there are multiple lines for a single label the last char
    ' numeric will be ignored so do not include in label eg txtAddress1 txtAddress2 lblAddress
    '
    ' History: 5/1/2000 BB - Created.
    '
    '
    ' ***************************************************************** '
    Public Function FindMatchingLabel(ByVal v_ctlDataControl As Control, ByRef r_ctlLabelControl As Label, ByRef r_sLabelCaption As String) As Integer

        Dim result As Integer = 0
        Dim sDataControlName As String = ""
        Dim bLabelFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_ctlLabelControl = Nothing
            r_sLabelCaption = ""
            bLabelFound = False

            ' Get the name minus the control type prefix
            ' If it's a Yes/No Check or DropDown assume 2 char prefix
            'IJR 2003-03-05 Start
            '    If TypeOf v_ctlDataControl Is YesNoCheck _
            ''    Or TypeOf v_ctlDataControl Is uctDropdown Then
            '        sDataControlName = Mid(v_ctlDataControl.Name, 3)
            '    ' Otherwise assume 3 char prefix
            '    Else
            '        sDataControlName = Mid(v_ctlDataControl.Name, 4)
            '    End If

            Select Case v_ctlDataControl.GetType().Name
                Case "YesNoCheck", "uctDropdown"
                    sDataControlName = Mid(v_ctlDataControl.Name, 3)
                    ' Otherwise assume 3 char prefix
                Case Else
                    sDataControlName = Mid(v_ctlDataControl.Name, 4)
            End Select
            'IJR 2003-03-05 End

            ' Remove any last numeric char (for address etc) see header note above
            Dim dbNumericTemp As Double
            If Double.TryParse(sDataControlName.Substring(sDataControlName.Length - 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                sDataControlName = sDataControlName.Substring(0, sDataControlName.Length - 1)
            End If

            ' Loop round currently loaded Forms
            For i As Integer = 0 To Application.OpenForms.Count - 1
                ' Loop round Controls in Form


                For j As Integer = 0 To ContainerHelper.Controls(Application.OpenForms.Item(i)).Count - 1
                    ' Is the control a Label?

                    If TypeOf ContainerHelper.Controls(Application.OpenForms.Item(i))(j) Is Label Then
                        ' Does the Name without the lbl prefix match the Input Control Name

                        If Mid(ContainerHelper.Controls(Application.OpenForms.Item(i))(j).Name, 4) = sDataControlName Then
                            ' Label matches set Control and Caption
                            bLabelFound = True

                            r_ctlLabelControl = ContainerHelper.Controls(Application.OpenForms.Item(i))(j)

                            r_sLabelCaption = ContainerHelper.Controls(Application.OpenForms.Item(i))(j).Text
                            Exit For
                        End If
                    End If
                Next j
                If bLabelFound Then
                    Exit For
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindMatchingLabel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindMatchingLabel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FindOnTab
    '
    ' Description: If a control is hidden on a tab then loop through
    '              tabs until the control is visible
    '
    ' History: 22/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function FindOnTab(ByRef ctlFindControl As Control) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            For Each ctlTab As Control In ContainerHelper.Controls(ctlFindControl.FindForm())
                If TypeOf ctlTab Is TabControl Then
                    '  click each tab until the control becomes visible
                    For i As Integer = 0 To SSTabHelper.GetTabCount(CType(ctlTab, TabControl)) - 1
                        SSTabHelper.SetSelectedIndex(CType(ctlTab, TabControl), i)
                        Application.DoEvents()
                        If ControlHelper.GetVisible(ctlFindControl) Then
                            Return gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Next i
                End If
            Next ctlTab

            ' Control Not Found

            Return gPMConstants.PMEReturnCode.PMNotFound

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindOnTab Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindOnTab", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetFormPositioning
    '
    ' Description: Retrives the form Top, left, height and width values
    '              from the registry
    '
    ' History: 08/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetFormPositioning(ByVal v_sSubLevelKey As String, ByRef r_sTop As String, ByRef r_sLeft As String, ByRef r_sHeight As String, ByRef r_sWidth As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Get top position from registry
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Top", r_sSettingValue:=r_sTop, v_sSubKey:="FormPositions\" & v_sSubLevelKey)

            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve top position of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get left position from the regsitry
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Left", r_sSettingValue:=r_sLeft, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Left position of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Height value from the regsitry
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Height", r_sSettingValue:=r_sHeight, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve Height value of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Width value from the regsitry
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Width", r_sSettingValue:=r_sWidth, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve width value of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFormPositioning Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetFormPositioning
    '
    ' Description: Sets the form Top, left, height and width values to
    '              the registry
    '
    ' History: 08/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function SetFormPositioning(ByVal v_sSubLevelKey As String, ByVal v_sTop As String, ByVal v_sLeft As String, ByVal v_sHeight As String, ByVal v_sWidth As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Set top position to registry
            lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Top", v_sSettingValue:=v_sTop, v_sSubKey:="FormPositions\" & v_sSubLevelKey)

            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set top position of form to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set left position to the regsitry
            lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Left", v_sSettingValue:=v_sLeft, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Left position of form to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set Height value to the regsitry
            lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Height", v_sSettingValue:=v_sHeight, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to set Height value of form to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set Width value to the regsitry
            lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="Width", v_sSettingValue:=v_sWidth, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Set width value of form to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFormPositioning Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormPositioning", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAviPath
    '
    ' Description:
    '
    ' History: 10/04/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetAviPath(ByVal v_sSettingName As String, ByRef r_sAVIPath As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Get AVI path from the regsitry
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=r_sAVIPath, v_sSubKey:="AVIs")
            'Check return
            '    If (lReturn& <> PMTrue) Then
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Unable to retrieve AVI path from registry", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetAviPath", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        GetAviPath = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAviPath Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAviPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetFormState
    '
    ' Description:
    '
    ' History: 13/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetFormState(ByVal v_sSubLevelKey As String, ByRef r_sFormState As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Get Form state value from the regsitry
            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="FormState", r_sSettingValue:=r_sFormState, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to retrieve form state value of form from registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFormState Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFormState", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetFormState
    '
    ' Description:
    '
    ' History: 13/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function SetFormState(ByVal v_sSubLevelKey As String, ByVal v_sFormState As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Set form state value to the regsitry
            lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="FormState", v_sSettingValue:=v_sFormState, v_sSubKey:="FormPositions\" & v_sSubLevelKey)
            'Check return
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Set form state value of form to registry", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFormState Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormState", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetHelpfile
    '
    ' Description: This function is called from the interface Initialise
    '              method and returns the position of the helpfile and
    '              asigns it to the form
    '
    ' History: 13/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function GetHelpfile(ByVal v_sSettingName As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sHelpFile As String = ""
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sHelpFile, v_sSubKey:="HelpFile"), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile location from registry", Application.ProductName)
                Return result
            End If
            'Assign the returned value
            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHelpfile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHelpfile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '********************************************************************'
    '*********          THE FOLLOWING FUNCTION NEEDS           **********'
    '*********          TO BE ADDED TO THE MAIN MODULE         **********'
    '*********          OF THE PROJECT, IF THE FORM            **********'
    '*********          HELP IS REQUIRED                       **********'
    '********************************************************************'

    '' ***************************************************************** '
    ''
    '' Name: ShowHelp
    ''
    '' Description:  This function is called from the click method of
    ''               the help button on a form.
    ''
    ''              A Microsoft Common Dialog control named: dlgHelp
    ''              is required on the form
    ''
    ''              Declare Global constant : ScreenHelpID in
    ''              the main module with it's value being the
    ''              ID number in the helpfile of the component
    ''
    ''              Add the help file id's to the HelpContextID and
    ''              WhatsThisHelpID properties
    ''
    '' History: 13/12/1999 JSB - Created.
    ''
    '' ***************************************************************** '
    'Public Function ShowHelp( _
    ''                    ByVal dlgHelp As Control, _
    ''                    ByVal lContextID As Long, _
    ''                    Optional ByVal sHelpType As String) As Long
    '
    '    On Error GoTo Err_ShowHelp
    '
    '    ShowHelp = PMTrue
    '
    '    'Check that we have a helpfile
    '    If (App.HelpFile = "") Then
    '        MsgBox "This form does not have a associated help file", vbInformation + vbOKOnly, ACApp
    '        Exit Function
    '    End If
    '
    '    ' Fire up the Common Dialog Control
    '    'and assign properties
    '    With dlgHelp
    '
    '        .HelpFile = App.HelpFile
    '        If (LCase(sHelpType) = "index") Then
    '            .HelpCommand = cdlHelpKey
    '        ElseIf (LCase(sHelpType) = "context") Then
    '            .HelpCommand = HelpContext
    '        Else
    '            .HelpCommand = HelpContext
    '        End If
    '        .HelpContext = lContextID&
    '        .ShowHelp
    '
    '    End With
    '
    '    Exit Function
    '
    'Err_ShowHelp:
    '
    '    ShowHelp = PMError
    '
    '    ' Log Error Message
    '    iPMFunc.LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="ShowHelp Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="ShowHelp", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function


    ' ***************************************************************** '
    ' Name: GetErrorMessages
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetErrorMessages(ByVal v_lGISSchemeId As Integer, ByVal v_oGIS As Object, ByVal v_lProcessType As Integer, Optional ByVal v_lMessageType As Integer = 0, Optional ByRef r_iErrorLevel As Integer = 0, Optional ByVal v_vBusinessType As String = "") As Integer

        ' TB 12/4/00 - Get Error level and GIS property of the field
        ' from the error message table (NB GIS prop was screen name)

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sj 22/09/2000 - start
            'KB - 14/01/2003 - Added Check for truck

            If Not Information.IsNothing(v_vBusinessType) Then
                If v_vBusinessType <> PMGISBusinessTypeHousehold And v_vBusinessType <> PMGISBusinessTypeTruck Then
                    v_vBusinessType = PMGISBusinessTypeMotor
                End If
            Else
                v_vBusinessType = PMGISBusinessTypeMotor
            End If

            '    m_lReturn = g_oGIS.NBPostQuoteProcess( _
            ''        v_lProcessType:=v_lProcessType, _
            ''        v_sGISBusinessTypeCode:=PMGISBusinessTypeMotor, _
            ''        v_lGISSchemeId:=v_lGISSchemeId _
            ''        )


            m_lReturn = g_oGis.NBPostQuoteProcess(v_lProcessType:=v_lProcessType, v_sGisBusinessTypeCode:=v_vBusinessType, v_lGISSchemeId:=v_lGISSchemeId)
            'sj 22/09/2000 - end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' KBrown - 14/01/03 - No check for Truck
            ' TB 03/12/01 Check ErrorMessages
            If v_vBusinessType = PMGISBusinessTypeTruck Then
                Return result
            End If

            m_lReturn = CType(CheckErrorLevel(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error may be quote data dependent.  Please contact Sirius Support with full quote details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetErrorMessages")
                Return result
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            r_iErrorLevel = 0
            m_lReturn = CType(DisplayErrors(r_iErrorLevel:=r_iErrorLevel), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or r_iErrorLevel >= 1 Then
                ' This returns a sever error flag, or display failed
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetErrorMessages Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetErrorMessages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckErrorLevel.  Check the Error messages for level 99
    '                         This means a cobol crash - usually module 153
    ' Description:
    '
    ' History: 03/12/2001 TB - Created.
    '
    ' ***************************************************************** '
    Public Function CheckErrorLevel() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iErrorLevel As Integer
            Dim vOIKeyArray() As Object
            Dim sError, sLevel, sGISProp As String


            ' Display any errors

            m_lReturn = g_oGis.GetAllOIKey(v_sObjectName:=ACGIIMQuoteErrorBreakdown, r_vOIKeyArray:=vOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vOIKeyArray) Then


                For i As Integer = 0 To vOIKeyArray.GetUpperBound(0)
                    ' Error message Text


                    m_lReturn = g_oGis.GetPropertyValue(ACGIIMQuoteErrorBreakdown, ACGIIMQuoteErrorBreakdown_Description, CStr(vOIKeyArray(i)), sError)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Error Level


                    m_lReturn = g_oGis.GetPropertyValue(ACGIIMQuoteErrorBreakdown, ACGIIMQuoteErrorBreakdown_Level, CStr(vOIKeyArray(i)), sLevel)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' GIS property of error field


                    m_lReturn = g_oGis.GetPropertyValue(ACGIIMQuoteErrorBreakdown, ACGIIMQuoteErrorBreakdown_ScreenName, CStr(vOIKeyArray(i)), sGISProp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    Dim dbNumericTemp As Double
                    If Double.TryParse(sLevel, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        iErrorLevel = CInt(sLevel)
                        If iErrorLevel = 99 Then
                            result = gPMConstants.PMEReturnCode.PMFalse ' The cobol call has failed
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sError, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckErrorLevel", vErrNo:=iErrorLevel, vErrDesc:=sGISProp & " Run-time Error")
                            '  MsgBox sError, vbExclamation + vbOKOnly, ACApp
                        End If
                    End If
                    ' The fatal cobol error is only 3 lines
                    If i > 3 Then
                        Return result
                    End If
                Next i

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckErrorLevel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckErrorLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function DisplayErrors(ByRef r_iErrorLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim vOIKeyArray() As Object
        Dim sError, sLevel, sGISProp As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Display any errors

            m_lReturn = g_oGis.GetAllOIKey(v_sObjectName:=ACGIIMQuoteErrorBreakdown, r_vOIKeyArray:=vOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vOIKeyArray) Then


                For i As Integer = 0 To vOIKeyArray.GetUpperBound(0)
                    ' Error message Text


                    m_lReturn = g_oGis.GetPropertyValue(ACGIIMQuoteErrorBreakdown, ACGIIMQuoteErrorBreakdown_Description, CStr(vOIKeyArray(i)), sError)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Error Level


                    m_lReturn = g_oGis.GetPropertyValue(ACGIIMQuoteErrorBreakdown, ACGIIMQuoteErrorBreakdown_Level, CStr(vOIKeyArray(i)), sLevel)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' GIS property of error field


                    m_lReturn = g_oGis.GetPropertyValue(ACGIIMQuoteErrorBreakdown, ACGIIMQuoteErrorBreakdown_ScreenName, CStr(vOIKeyArray(i)), sGISProp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Display the error text
                    MessageBox.Show(sError, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Dim dbNumericTemp As Double
                    If Double.TryParse(sLevel, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        r_iErrorLevel = CInt(sLevel)
                    Else
                        r_iErrorLevel = gPMConstants.PMEReturnCode.PMTrue
                    End If
                    ' Drop out on First Severe error
                    'If sLevel = "3" Or sLevel = "4" Or sLevel = "5" Then
                    ' TB 5/12/01 level 8 is EDI error, report all
                    If sLevel <> "0" And sLevel <> "6" And sLevel <> "8" Then
                        Return result
                    End If
                Next i

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Display Errors Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayErrors", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyStatusDescription
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetPolicyStatusDescription(ByRef r_sStatusDescription As String, ByVal r_lPolicyStatus As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case r_lPolicyStatus
                Case GIIPolicyIncomplete
                    r_sStatusDescription = "New Business(Incomplete)"
                Case GIIPolicyQuote
                    r_sStatusDescription = "Quoted"
                Case GIIPolicyNBComplete
                    r_sStatusDescription = "New Business(Complete)"
                Case GIIPolicyRequoteRequired
                    r_sStatusDescription = "Requote required"
                Case GIIPolicyRequoted
                    r_sStatusDescription = "Requoted"
                Case GIIPolicyPending
                    r_sStatusDescription = "Pending (Not Transmitted)"
                Case GIIPolicyPendingTransmitted
                    r_sStatusDescription = "Pending (Transmitted)"
                Case GIIPolicyLive
                    r_sStatusDescription = "Live"
                    ' BSJ 20/12/00 - Add MTA cases
                Case GIIPolicyMTACancellation
                    r_sStatusDescription = "MTA - Cancellation"
                Case GIIPolicyMTAIncomplete
                    r_sStatusDescription = "MTA - Incomplete"
                Case GIIPolicyMTAPermanent
                    r_sStatusDescription = "MTA - Permanent"
                Case GIIPolicyMTAReinstatement
                    r_sStatusDescription = "MTA - Reinstatement"
                Case GIIPolicyMTATemporary
                    r_sStatusDescription = "MTA - Temporary"
                    'sj 20/12/2001 - start
                Case GEMPolicyCancelled
                    r_sStatusDescription = "Cancelled"
                Case GEMPolicyCancelPending
                    r_sStatusDescription = "Cancellation Pending"
                    'sj 20/12/2001 - end
                Case Else
                    r_sStatusDescription = "Unknown Status - " & r_lPolicyStatus
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyStatusDescription Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyStatusDescription", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveLoadMaxExcess
    '
    ' Description:
    '
    ' History: 17/03/2000 JSB - Created.
    ' 22/5/01: TB added param available excesses & how it works
    '
    ' ***************************************************************** '
    Public Function SaveLoadMaxExcess(Optional ByVal v_bSave As Boolean = False, Optional ByVal v_bLoad As Boolean = False, Optional ByVal v_sSelectedExcess As String = "", Optional ByVal v_sExcesses As String = "", Optional ByRef r_sExcesses As String = "", Optional ByVal v_bAvailXS As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGeminiII
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            result = gPMConstants.PMEReturnCode.PMTrue

            ' TB changed how this works. On load, get Maximum Excesses, if not
            ' available get Available Excesses, if that not available, set up Available
            If v_bSave Then
                If v_sSelectedExcess <> "" Then
                    lReturn = CType(gPMFunctions.SetPMRegSetting(eRegSettingRoot, eProductFamily, eRegSettingLevel, "Maximum Excess", v_sSelectedExcess, "GII Quote Display"), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                ElseIf v_sExcesses <> "" Then
                    lReturn = CType(gPMFunctions.SetPMRegSetting(eRegSettingRoot, eProductFamily, eRegSettingLevel, "Maximum Excesses", v_sExcesses, "GII Quote Display"), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            ElseIf (v_bLoad) Then

                If Not v_bAvailXS Then ' i.e. param not supplied
                    lReturn = CType(gPMFunctions.GetPMRegSetting(eRegSettingRoot, eProductFamily, eRegSettingLevel, "Maximum Excesses", r_sExcesses, "GII Quote Display"), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If r_sExcesses = "" Or v_bAvailXS Then
                    lReturn = CType(gPMFunctions.GetPMRegSetting(eRegSettingRoot, eProductFamily, eRegSettingLevel, "Available Excesses", r_sExcesses, "GII Quote Display"), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                ' Use default maximum excesses if not in registry
                If r_sExcesses = "" Then
                    r_sExcesses = "0~50~100~250~500"
                    ' TB 21/5/01
                    ' And then put it into the registry
                    lReturn = CType(gPMFunctions.SetPMRegSetting(eRegSettingRoot, eProductFamily, eRegSettingLevel, "Available Excesses", r_sExcesses, "GII Quote Display"), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveLoadMaxExcess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveLoadMaxExcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ParseExcesses
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ParseExcesses(ByVal v_sExcesses As String, ByRef r_sExcess_Level_One As String, ByRef r_sExcess_Level_Two As String, ByRef r_sExcess_Level_Three As String, ByRef r_sExcess_Level_Four As String, ByRef r_sExcess_Level_Five As String) As Integer

        Dim result As Integer = 0
        Dim iCnt, nStart, nPos As Integer
        Dim curExcess As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add each maximum excess to combo box.
            nStart = 1
            nPos = Strings.InStr(nStart, v_sExcesses, "~")
            iCnt = 1
            Do While nPos > 0
                curExcess = CDec(v_sExcesses.Substring(nStart - 1, Math.Min(v_sExcesses.Length, nPos - nStart)))
                Select Case iCnt
                    Case 1
                        r_sExcess_Level_One = StringsHelper.Format(curExcess, "###0")
                    Case 2
                        r_sExcess_Level_Two = StringsHelper.Format(curExcess, "###0")
                    Case 3
                        r_sExcess_Level_Three = StringsHelper.Format(curExcess, "###0")
                    Case 4
                        r_sExcess_Level_Four = StringsHelper.Format(curExcess, "###0")
                    Case 5
                        r_sExcess_Level_Five = StringsHelper.Format(curExcess, "###0")
                End Select
                iCnt += 1
                nStart = nPos + 1
                nPos = Strings.InStr(nStart, v_sExcesses, "~")
            Loop
            If nStart < v_sExcesses.Length Then
                r_sExcess_Level_Five = CStr(CDec(v_sExcesses.Substring(nStart - 1, Math.Min(v_sExcesses.Length, v_sExcesses.Length - nStart + 1))))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParseExcessesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ParseExcesses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DisableFormCloseButton
    '
    ' Description: Disables 'X' button on the form using an API call
    '              Requires the form title to be passed through
    '
    ' History: 25/05/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function DisableFormCloseButton(ByVal sFormWindowName As String) As Integer

        Dim result As Integer = 0
        Dim hwndHandle, hMenuHandle, hClose As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            hwndHandle = iPMFunc.FindWindow(0, sFormWindowName)
            If hwndHandle <> 0 Then
                hMenuHandle = GetSystemMenu(hwndHandle, False)
                If hMenuHandle <> 0 Then
                    hClose = DeleteMenu(hMenuHandle, SC_CLOSE, MF_BYCOMMAND)
                End If
            Else
                MessageBox.Show(sFormWindowName & " window not found", Application.ProductName)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableFormCloseButton Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableFormCloseButton", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: EnableFormCloseButton
    '
    ' Description: Enables 'X' button on the form using an API call
    '              Requires the form title to be passed through
    '
    ' History: 26/05/2000 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function EnableFormCloseButton(ByVal sFormWindowName As String) As Integer
        Dim result As Integer = 0
        Dim hwndHandle, hMenuHandle, hClose As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            hwndHandle = iPMFunc.FindWindow(0, sFormWindowName)
            If hwndHandle <> 0 Then
                ' restores setings of the menu, thus enabling the x button for exit
                hMenuHandle = GetSystemMenu(hwndHandle, True)
            Else
                MessageBox.Show(sFormWindowName & " window not found", Application.ProductName)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableFormCloseButton Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableFormCloseButton", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AutoScroll
    '
    ' Description: Called when user tabs to a control which is above or
    '              below the visible section of the screen.  This procedure
    '              adjusts the scrollbar so that the control becomes visible.
    '
    ' Author: Richard Taylor
    '
    ' Date: 5/7/2000
    '
    ' ***************************************************************** '
    Public Sub AutoScroll(ByVal v_ctlControl As Control, ByVal v_ctlPanel As Control, ByVal v_ctlPicMain As Control, ByRef r_VScrollPicMain As VScrollBar)

        Dim w As Integer ' Represents the height of the Control with focus
        Dim x As Integer ' Represents the top of the panel which moves up and down
        Dim y As Integer ' Represents the top of the control in relation to the panel which moves up and down
        Dim z As Integer ' Represents the height of the visible area on screen

        Try

            x = CInt(-VB6.PixelsToTwipsY(v_ctlPanel.Top))
            y = CInt(VB6.PixelsToTwipsY(v_ctlControl.Parent.Top) + VB6.PixelsToTwipsY(v_ctlControl.Top))
            w = CInt(VB6.PixelsToTwipsY(v_ctlControl.Height))
            z = CInt(VB6.PixelsToTwipsY(v_ctlPicMain.Height))

            ' If the control is below the bottom of the screen
            If ((y + w) - x) > z Then
                r_VScrollPicMain.Value = -(VB6.PixelsToTwipsY(v_ctlPanel.Top) - (((y + w) - x) - z))
                ' ElseIf the control is above the top of the screen
            ElseIf (y - x) < 0 Then
                r_VScrollPicMain.Value = y
            End If

        Catch



            'RDT161000 - This is not at all fatal so don't report error
            '    ' Log Error Message
            '    LogMessage _
            ''        iType:=PMLogOnError, _
            ''        sMsg:="AutoScrollFailed", _
            ''        vApp:=ACApp, _
            ''        vClass:=ACClass, _
            ''        vMethod:="AutoScroll", _
            ''        vErrNo:=Err.Number, _
            ''        vErrDesc:=Err.Description

            Exit Sub
        End Try


    End Sub



    ' ***************************************************************** '
    '
    ' Name: CreateNewTask
    '
    ' Description:
    '
    ' History: 26/10/2000 RDT - Created.
    '
    ' ***************************************************************** '
    Public Sub CreateNewTask(ByVal sBusinessType As String, ByVal sClientcode As String)
        Dim iGiiCreateTaskWrapper As Object

        Dim lReturn As Integer
        Dim oCreateTask, vKeyArray As Object

        Try

            oCreateTask = New iGiiCreateTaskWrapper.Interface()


            lReturn = oCreateTask.Initialise
            If lReturn <> 1 Then
                Exit Sub
            End If


            oCreateTask.BusinessType = sBusinessType

            oCreateTask.Client = sClientcode


            lReturn = oCreateTask.Start
            If lReturn <> 1 Then
                Exit Sub
            End If


            oCreateTask.Dispose()

            oCreateTask = Nothing

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNewTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetFormCaption
    '
    ' Description:
    '
    ' History: 22/03/2001 PA - Created.
    '
    ' ***************************************************************** '
    Public Function SetFormCaption(ByVal v_sBusinessType As String, ByRef r_sCaption As String) As Integer

        Dim result As Integer = 0
        Dim vTitle As String = ""
        Dim vInitial As String = ""
        Dim vSurname As String = ""
        Dim vShortName As Object
        Dim vResolvedName As String = ""
        Dim vModelName As String = ""
        Dim vRegNo As String = ""
        Dim vOIKeyArray() As Object
        Dim sGIIMPolicyOIKey, sProposerPolicyHolderOIKey, sVehicleOIKey As String
        Dim vChildOIKey As Object
        Dim sGIIMControlOIKey, sGIITClientOIKey As String
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sBusinessType = PMGISBusinessTypeMotor Then

                lReturn = g_oGis.GetAllOIKey(v_sObjectName:="GIIMGemPolicy", r_vOIKeyArray:=vOIKeyArray)

                If Information.IsArray(vOIKeyArray) Then
                    ' Get the Key


                    sGIIMPolicyOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))
                Else
                    Return result
                End If


                lReturn = g_oGis.GetAllOIKey(v_sObjectName:="control_block", r_vOIKeyArray:=vOIKeyArray)

                If Information.IsArray(vOIKeyArray) Then
                    ' Get the Key


                    sGIIMControlOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))
                Else
                    Return result
                End If


                lReturn = g_oGis.GetChildOIKey("GIIMGemPolicy", sGIIMPolicyOIKey, "Proposer_Policyholder", vChildOIKey)

                If Information.IsArray(vChildOIKey) Then


                    sProposerPolicyHolderOIKey = CStr(vChildOIKey(vChildOIKey.GetLowerBound(0)))
                Else
                    Return result
                End If

                ' Client Code

                lReturn = g_oGis.GetPropertyValue(v_sObjectName:="control_block", v_sPropertyName:="client_key", v_sOIKey:=sGIIMControlOIKey, r_vPropertyValue:=vShortName)

                ' Build the name field

                lReturn = g_oGis.GetPropertyValue("Proposer_Policyholder", "Title_Text", sProposerPolicyHolderOIKey, vTitle)

                ' First initial

                lReturn = g_oGis.GetPropertyValue("Proposer_Policyholder", "Forename_Initial_1", sProposerPolicyHolderOIKey, vInitial)

                'Surname

                lReturn = g_oGis.GetPropertyValue("Proposer_Policyholder", "Surname", sProposerPolicyHolderOIKey, vSurname)

                vResolvedName = vTitle & " " & vInitial & " " & vSurname


                lReturn = g_oGis.GetAllOIKey(v_sObjectName:="Vehicle", r_vOIKeyArray:=vOIKeyArray)

                If Information.IsArray(vOIKeyArray) Then
                    ' Get the Key


                    sVehicleOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))


                    lReturn = g_oGis.GetPropertyValue(v_sObjectName:="Vehicle", v_sPropertyName:="Model_Name", v_sOIKey:=sVehicleOIKey, r_vPropertyValue:=vModelName)


                    lReturn = g_oGis.GetPropertyValue(v_sObjectName:="Vehicle", v_sPropertyName:="Reg_No", v_sOIKey:=sVehicleOIKey, r_vPropertyValue:=vRegNo)

                End If
            End If

            If v_sBusinessType = PMGISBusinessTypeTruck Then
                ' Get the Client Details

                m_lReturn = g_oGis.GetAllOIKey(v_sObjectName:="truck_client_details", r_vOIKeyArray:=vOIKeyArray)

                If Not Information.IsArray(vOIKeyArray) Then
                    Return result
                End If


                sGIITClientOIKey = CStr(vOIKeyArray(0))

                ' Client Code

                m_lReturn = g_oGis.GetPropertyValue("truck_client_details", "tk_cli_code", sGIITClientOIKey, vShortName)

                ' Client Name

                m_lReturn = g_oGis.GetPropertyValue("truck_client_details", "tk_cli_name", sGIITClientOIKey, vResolvedName)

                ' Vehicle Details

                lReturn = g_oGis.GetAllOIKey(v_sObjectName:="truck_vehicle", r_vOIKeyArray:=vOIKeyArray)

                If Information.IsArray(vOIKeyArray) Then
                    ' Get the Key


                    sVehicleOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))


                    lReturn = g_oGis.GetPropertyValue(v_sObjectName:="truck_vehicle", v_sPropertyName:="tk_veh_model", v_sOIKey:=sVehicleOIKey, r_vPropertyValue:=vModelName)


                    lReturn = g_oGis.GetPropertyValue(v_sObjectName:="truck_vehicle", v_sPropertyName:="tk_veh_reg_no", v_sOIKey:=sVehicleOIKey, r_vPropertyValue:=vRegNo)

                End If
            End If


            r_sCaption = "Client Code: " & CStr(vShortName)

            If vResolvedName.Trim() <> "" Then
                r_sCaption = r_sCaption & " Name: " & vResolvedName
            End If

            If vModelName.Trim() <> "" Then
                r_sCaption = r_sCaption & " Vehicle: " & vModelName
            End If

            If vRegNo.Trim() <> "" Then
                r_sCaption = r_sCaption & " Reg No: " & vRegNo
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetFormCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RestrictChars
    '
    ' Description: Restricts KeyPresses to a-z A-Z Space - ' ( ) 0-9 & . /
    '               within address lines. IsName denoting whether it is a
    '               name or address field
    '
    ' History: 11/10/2001 MSS - Created.
    ' History: 12/10/2001 MSS - Added IsName switch
    '
    ' ***************************************************************** '
    Public Sub RestrictChars(ByRef KeyAscii As Integer, Optional ByVal IsName As Boolean = False)

        Try


            Select Case KeyAscii
                ' Did it this way so it's easier to add allowed chars
                ' rather than having to alter the case statement for the
                ' ones we want to allow.
                Case Is < 33 ' Tab, Return and all other needed stuff
                Case 65 To 90 'A To Z.
                Case 97 To 122 ' a To z
                Case 45 To 58 ' - . / and 0 To 9 and Space
                    'MSS 090102 - Removed below restriction. Left code in case the powers
                    ' that be change their minds again
                    ' Restrict 0-9 . / for names
                    '    If IsName Then
                    '        If KeyAscii > 45 And KeyAscii < 58 Then
                    '            KeyAscii = 0
                    '        End If
                    '    End If

                Case 38 To 41 '() & '

                Case Else
                    KeyAscii = 0
            End Select

        Catch excep As System.Exception



            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Restrict Chars", vApp:=ACApp, vClass:=ACClass, vMethod:="RestrictChars", excep:=excep)

            Exit Sub



        End Try

    End Sub
End Module
