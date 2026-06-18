Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

<System.Runtime.InteropServices.ProgId("PMHelpFunc_NET.PMHelpFunc")> _
 Public Module PMHelpFunc
	
    Declare Function WinHelp Lib "User32" Alias "WinHelpA" (ByVal hWnd As Integer, ByVal lpHelpFile As String, ByVal wCommand As Integer, ByVal dwData As Integer) As Integer

    Public Const HelpContext As Integer = &H1
    Public Const HelpSearch As Integer = &HB
	
	Private Const ACClass As String = "PMHelpFunc"
    'Added by Deepak Sharma on 4/20/2010 6:00:24 PM refer developer guide no. 

    Public g_sProductFamily As gPMConstants.PMEProductFamily


    '******************************************************************************
    ' ShowHelp
    '
    ' Description: Function used to add help to the compontent
    '
    '              'A Microsoft Common Dialog control named: dlgHelp
    '              'is required on the form
    '
    '              'Declare Global constant : ScreenHelpID in
    '              'the main module with it's value being the
    '              'ID number in the helpfile of the component
    '
    '              'Declare Global Constant : g_sProductFamily  in
    '              'the main module with it's value being the product
    '              'family of the component, for example pmepfOrion
    '
    '              'Add the help file id's to the HelpContextID and
    '              'WhatsThisHelpID properties
    '
    '              'Add the following code to the to the Initialise
    '              'method of the Interface class module:-
    '
    '                    Dim sHelpFile As String
    '                    Dim eRegSettingRoot As PMERegSettingRoot
    '                    Dim eRegSettingLevel As PMERegSettingLevel
    '                    Dim eProductFamily As PMEProductFamily
    '
    '                    eRegSettingRoot = pmeRSRLocalMachine
    '                    eProductFamily = g_sProductFamily
    '                    eRegSettingLevel = pmeRSLClient
    '
    '                    'Find out from the registry where the Help File is
    '                    m_lReturn = GetPMRegSetting( _
    ''                        v_lPMERegSettingRoot:=eRegSettingRoot, _
    ''                        v_lPMEProductFamily:=eProductFamily, _
    ''                        v_lPMERegsettinglevel:=eRegSettingLevel, _
    ''                        v_sSettingName:="HelpFile", _
    ''                        r_sSettingValue:=sHelpFile)
    '
    '                    If (m_lReturn <> PMTrue) Then
    '                        MsgBox "Failed to retrive Helpfile"
    '                        Exit Function
    '                    End If
    '                    If (sHelpFile$ <> "") Then
    '                        App.HelpFile = sHelpFile$
    '                    End If
    '
    '' JSB 14/10/98
    '******************************************************************************

    'Call this function from the click method of the help button on the form

    'Modified by Archana Tokas on 4/30/2010 11:11:36 AM no parameters required
    'Public Function ShowHelp(ByVal dlgHelp As Object, ByVal lContextID As Integer, Optional ByVal sHelpType As String = "") As Integer
    Public Function ShowHelp(ByRef objCnt As System.Windows.Forms.Control) As Integer

        Dim result As Integer = 0
        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try
            'Get path of the help file from the registry
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            'Check that we have a valid return
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue



            ' Fire up the Common Dialog Control
            'and assign properties
            'Modified by Archana Tokas on 4/30/2010 11:13:27 AM refer developer guide no. 20
            'With dlgHelp


            '    ReflectionHelper.SetMember(dlgHelp, "HelpFile", sHelpFile)
            '    If sHelpType.ToLower() = "index" Then
            '        ' CTAF 120802 - start

            'ReflectionHelper.SetMember(dlgHelp, "HelpCommand", HelpContext)
            '        '.HelpCommand = cdlHelpKey
            '        ' CTAF 120802 - end
            '    ElseIf (sHelpType.ToLower() = "context") Then

            '        ReflectionHelper.SetMember(dlgHelp, "HelpCommand", HelpContext)
            '    Else

            '        ReflectionHelper.SetMember(dlgHelp, "HelpCommand", HelpContext)
            '    End If

            '    ReflectionHelper.SetMember(dlgHelp, "HelpContext", lContextID)

            '    ReflectionHelper.Invoke(dlgHelp, "ShowHelp", New Object() {})


            'Modified by Archana Tokas on 4/30/2010 11:13:32 AM refer developer guide no. 20
            'System.Diagnostics.Process.Start(sHelpFile)
            WinHelp(objCnt.Handle.ToInt32(), sHelpFile, HelpSearch, 0)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in ShowHelp", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHelp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Overloaded function to prevent error
    Public Function ShowHelp(ByRef objCnt As System.Windows.Forms.Control, ByVal lContextID As Integer) As Integer

        Dim result As Integer = 0
        Dim sHelpFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try



            'Get path of the help file from the registry
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            'Check that we have a valid return
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue



            ' Fire up the Common Dialog Control
            'and assign properties
            'Modified by Archana Tokas on 4/30/2010 11:13:27 AM refer developer guide no. 20
            'With dlgHelp


            '    ReflectionHelper.SetMember(dlgHelp, "HelpFile", sHelpFile)
            '    If sHelpType.ToLower() = "index" Then
            '        ' CTAF 120802 - start

            '        ReflectionHelper.SetMember(dlgHelp, "HelpCommand", HelpContext)
            '        '.HelpCommand = cdlHelpKey
            '        ' CTAF 120802 - end
            '    ElseIf (sHelpType.ToLower() = "context") Then

            '        ReflectionHelper.SetMember(dlgHelp, "HelpCommand", HelpContext)
            '    Else

            '        ReflectionHelper.SetMember(dlgHelp, "HelpCommand", HelpContext)
            '    End If

            '    ReflectionHelper.SetMember(dlgHelp, "HelpContext", lContextID)

            '    ReflectionHelper.Invoke(dlgHelp, "ShowHelp", New Object() {})


            'changes to show help file 

            'TODO: context id to be set
            'Help.ShowHelp(objCnt, sHelpFile, HelpNavigator.TopicId, lContextID.ToString)
            'Help.ShowHelp(objCnt, sHelpFile)

            WinHelp(objCnt.Handle.ToInt32(), sHelpFile, HelpContext, lContextID)
            'WinHelp(objCnt.Handle.ToInt32(), sHelpFile, 257, lContextID)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in ShowHelp", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHelp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
