Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("CheckInstall_NET.CheckInstall")> _
Public NotInheritable Class CheckInstall
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CheckInstall
    '
    ' Date: 09/02/1999
    '
    ' Description: Checks the currently installed version of a Product
    '              against the latest one.
    '
    ' Edit History:
    ' DAK221299 - Add function to check each product in turn to determine
    '             if it needs upgrading.
    ' DAK221299a - Check Client key exists
    '              Only give version number not found error message for
    '              Sirius Architecture.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "CheckInstall"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_fStartInstall As frmStartInstall
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: CheckClientInstalls
    '
    ' Description: Checks what products are installed and checks each
    '              if they need to be upgraded.
    '
    ' History: 22/12/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function CheckClientInstalls(ByVal v_oClientManager As Object) As Integer
        Dim result As Integer = 0
        Dim sRegKey As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'DAK221299a
            sRegKey = ACRegRoot & ACRegSiriusSolutions & ACRegClient
            If KeyExists(gPMConstants.HKEY_LOCAL_MACHINE, sRegKey) Then
                m_lReturn = CheckClientInstall(v_oClientManager, PMEProductFamily.pmePFSiriusSolutions)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClientInstalls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientInstalls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckClientInstall
    '
    ' Description: Checks to see if the latest Client Version for the
    '              Product has been installed.
    ' ***************************************************************** '
    Public Function CheckClientInstall(ByVal v_oClientManager As Object, ByVal v_lPMEProductFamily As Integer) As Integer
        Dim result As Integer = 0
        Dim sExistingClientVersion As String
        Dim sLatestClientVersion As String
        Dim iIsLatestClientMandatory As Integer
        Dim iIsClientAutoInstallable As Integer
        Dim sClientInstallPath As String
        Dim sClientInstallProgram As String
        Dim iClientRebootLevel As Integer
        Dim sMsg As String
        Dim sTitle As String
        Dim iResponce As Integer
        Dim iOKCancel As Integer
        Dim bStartInstall As Boolean
        Dim sPMProductCode As String
        Dim sMandatory1 As String
        Dim sMandatory2 As String
        Dim sReboot As String
        Dim lReturn As Long
        Dim oBusiness As Object
        Dim sProductDescription As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bStartInstall = False

            ' Have we been supplied Client Manager
            If (v_oClientManager Is Nothing = True) Then
                LogMessageToFile( _
                    sUsername:="", _
                    iType:=gPMConstants.PMELogLevel.PMLogError, _
                    sMsg:="Client Manager Reference NOT Supplied.", _
                    vApp:=ACApp, _
                    vClass:=ACClass, _
                    vMethod:="CheckClientInstall")
                CheckClientInstall = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the business object
            m_lReturn = v_oClientManager.GetInstance( _
                oObject:=oBusiness, _
                sClassName:="bPMProductClientInstall.CheckInstall", _
                sCallingAppName:=ACApp)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                LogMessageToFile( _
                    sUsername:="", _
                    iType:=gPMConstants.PMELogLevel.PMLogError, _
                    sMsg:="Failed to create business object: " & "bPMProductClientInstall.CheckInstall", _
                    vApp:=ACApp, _
                    vClass:=ACClass, _
                    vMethod:="CheckClientInstall")
                CheckClientInstall = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Product Code
            sPMProductCode = PMProductCode(v_lPMEProductFamily)

            ' Get the Existing Client Version
            m_lReturn = GetClientVersion( _
                v_lPMEProductFamily:=v_lPMEProductFamily, _
                r_sVersion:=sExistingClientVersion)

            ' Check the Client Version
            m_lReturn = oBusiness.CheckClientVersion( _
                v_lPMEProductFamily:=v_lPMEProductFamily, _
                v_sExistingClientVersion:=sExistingClientVersion, _
                r_sLatestClientVersion:=sLatestClientVersion, _
                r_iIsLatestClientMandatory:=iIsLatestClientMandatory, _
                r_iIsClientAutoInstallable:=iIsClientAutoInstallable, _
                r_sClientInstallPath:=sClientInstallPath, _
                r_sClientInstallProgram:=sClientInstallProgram, _
                r_iClientRebootLevel:=iClientRebootLevel, _
                r_sProductDescription:=sProductDescription)

            oBusiness.Dispose()
            oBusiness = Nothing

            ' Work Out what to do
            Select Case m_lReturn
                ' Same Client Version
                Case gPMConstants.PMEReturnCode.PMTrue

                    ' No Install Required

                    ' Old Client Version
                Case gPMConstants.PMEReturnCode.PMEarlier

                    If (iIsLatestClientMandatory = gPMConstants.PMEReturnCode.PMTrue) Then
                        sTitle = "Automatic Upgrade (Mandatory)"
                        sMandatory1 = "MUST"
                        sMandatory2 = vbCrLf & vbCrLf & "The Upgrade will be started when you click OK."
                        iOKCancel = vbOKOnly
                    Else
                        sTitle = "Automatic Upgrade (Optional)"
                        sMandatory1 = "can"
                        sMandatory2 = vbCrLf & vbCrLf & "Do you want to start the Upgrade now ?"
                        iOKCancel = vbYesNo
                    End If

                    If (iIsClientAutoInstallable = gPMConstants.PMEReturnCode.PMFalse) Then
                        sTitle = "Non Automatic Upgrade"
                        sMandatory2 = vbCrLf & vbCrLf & "The Upgrade MUST be performed Manually."
                        sMandatory2 = sMandatory2 & vbCrLf & vbCrLf & "Contact your System Administrator."
                        iOKCancel = vbOKOnly
                    Else

                        Select Case iClientRebootLevel
                            Case ACLogoffOnly
                                sReboot = vbCrLf & vbCrLf & "NOTE: You will be logged off your PC." & vbCrLf & "The Upgrade will start when you next logon."
                            Case ACFullReboot
                                sReboot = vbCrLf & vbCrLf & "NOTE: Your PC will be rebooted BEFORE the Upgrade starts."
                            Case ACNoReboot
                                sReboot = ""
                            Case Else
                                sReboot = ""
                        End Select

                    End If

                    sMsg = "Your PC has Client Version " & sExistingClientVersion & " of " & sProductDescription & " installed."
                    sMsg = sMsg & vbCrLf & vbCrLf
                    sMsg = sMsg & "It " & sMandatory1 & " be upgraded to Version " & sLatestClientVersion
                    sMsg = sMsg & sMandatory2
                    sMsg = sMsg & sReboot

                    MsgBox(sMsg, vbOKOnly Or vbExclamation, sTitle)
                    ' Show Message
                    'ShowMessage(v_sMessage:=sMsg, _
                    '    v_sTitle:=sTitle, _
                    '    v_iYesNo:=iOKCancel, _
                    '    v_sPath:=sClientInstallPath, _
                    '    r_iResponce:=iResponce)

                    'If iResponce = vbYes _
                    'And iIsClientAutoInstallable = gPMConstants.PMEReturnCode.PMTrue Then
                    '    bStartInstall = True
                    'Else
                    result = gPMConstants.PMEReturnCode.PMTrue
                    'End If

                    ' Newer Client Version
                Case gPMConstants.PMEReturnCode.PMLater

                    sMsg = "Your PC has Client Version " & sExistingClientVersion & " of " & sProductDescription & " installed."
                    sMsg = sMsg & vbCrLf & vbCrLf
                    sMsg = sMsg & "This is NEWER than the Server Version " & sLatestClientVersion
                    sMsg = sMsg & vbCrLf & vbCrLf & "Contact your System Administrator."
                    MsgBox(sMsg, vbOKOnly Or vbExclamation, "Newer Client Version than Server")

                    result = gPMConstants.PMEReturnCode.PMTrue

                    ' Error
                Case Else

                    sMsg = "Error encountered checking Client Version."
                    MsgBox(sMsg, vbOKOnly Or vbExclamation, "Error")

                    result = m_lReturn

            End Select

            ' If we need to Start an Install then Start It.
            If (bStartInstall = True) Then
                CheckClientInstall = StartInstall( _
                    v_sPMProductCode:=sPMProductCode, _
                    v_sClientInstallPath:=sClientInstallPath, _
                    v_sClientInstallProgram:=sClientInstallProgram, _
                    v_iClientRebootLevel:=iClientRebootLevel)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClientInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ShowMessage
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub ShowMessage(ByVal v_sMessage As String, ByVal v_sTitle As String, ByVal v_iYesNo As Integer, ByVal v_sPath As String, ByRef r_iResponce As Integer)

        Dim fMessage As frmMessage

        ' developer guide no. 
        fMessage = New frmMessage()


        'fMessage.Show()

        fMessage.Text = v_sTitle
        fMessage.lblMessage.Text = v_sMessage

        'Try
        '    fMessage.rtbReadme.LoadFile(v_sPath & "readme.rtf")

        'Catch
        'End Try

        'developers guide no.32



        'NIIT - Replaced with the Migrated code 1144 
        'If fMessage.rtbReadme.FileName = "" Then
        If UpgradeStubs.RichTextLib_RichTextBox.getFileName(fMessage.rtbReadme) = "" Then
            fMessage.rtbReadme.Visible = False
            fMessage.cmdNo.Top -= VB6.TwipsToPixelsY(2400)
            fMessage.cmdYes.Top = fMessage.cmdNo.Top
            fMessage.Height -= VB6.TwipsToPixelsY(2400)
        End If

        If v_iYesNo = MsgBoxStyle.OkOnly Then
            fMessage.cmdYes.Visible = False
            fMessage.cmdNo.Text = "&OK"
        End If

        fMessage.ShowDialog()

        r_iResponce = gPMConstants.PMEReturnCode.PMTrue

        If v_iYesNo = MsgBoxStyle.YesNo Then
            r_iResponce = fMessage.Status
        End If

        fMessage.Close()
        fMessage = Nothing

        Exit Sub

Err_ShowMessage:


        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowMessageFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub


    ' ***************************************************************** '
    ' Name: StartInstall
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function StartInstall(ByVal v_sPMProductCode As String, ByVal v_sClientInstallPath As String, ByVal v_sClientInstallProgram As String, ByVal v_iClientRebootLevel As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        m_fStartInstall = New frmStartInstall()


        m_fStartInstall.Show()

        lReturn = CType(m_fStartInstall.StartInstallTimer(v_sPMProductCode, v_sClientInstallPath, v_sClientInstallProgram, v_iClientRebootLevel, Me), gPMConstants.PMEReturnCode)


        Return gPMConstants.PMEReturnCode.PMInstallStarted

    End Function

    ' ***************************************************************** '
    ' Name: GetClientVersion
    '
    ' Description: Gets the Client Version of the installed PM Product.
    '
    ' ***************************************************************** '
    Private Function GetClientVersion(ByVal v_lPMEProductFamily As Integer, ByRef r_sVersion As String) As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        r_sVersion = ""

        ' Get the Version from the Registry
        result = GetPMRegSetting( _
            v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, _
            v_lPMEProductFamily:=v_lPMEProductFamily, _
            v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, _
            v_sSettingName:=PMRegKeyVersion, _
            r_sSettingValue:=r_sVersion)

        'DAK221299a
        If (Trim$(r_sVersion) = "" And _
            v_lPMEProductFamily = PMEProductFamily.pmePFSiriusArchitecture) Then

            iPMFunc.LogMessage( _
                iType:=gPMConstants.PMELogLevel.PMLogError, _
                sMsg:="Failed to get CLIENT Software Version for Product Family: " & v_lPMEProductFamily, _
                vApp:=ACApp, _
                vClass:=ACClass, _
                vMethod:="GetClientVersion")
            GetClientVersion = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

