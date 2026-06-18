Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no.129
Imports SharedFiles
Imports Artinsoft.VB6
Imports Artinsoft.VB6.Utils

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 03 July 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' RFC 23/04/1998 - Amended to allow local or remote logon.
    ' RFC 17/06/1998 - Server Printer Properties Added.
    ' RFC161098 - Keep a count of how many apps have a reference to LogonManager
    ' RFC100299 - Check Client Installations
    ' DAK221299 - Check all client installations
    ' ***************************************************************** '


    ' Main global constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iLogonManager"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Local and Remote LicenceManagers
    Public Const ACLocalLicenceManager As String = "bPMLocalLicenceManager.LicenceManager"
    Public Const ACRemoteLicenceManager As String = "bPMRemoteLicenceManager.LicenceManager"
    ' RFC 11/08/03- Load Balance LicenceManager
    Public Const ACCOMPlusLicenceManager As String = "bPMLicenceManager.LicenceManagerCOMPlus"

    ' Public variables to be exposed always.
    Public g_sUserName As String = ""
    
    Public g_sPassword As String = ""
    
    Public g_dLogonTime As Date
    
    Public g_iLanguageID As Integer
    
    Public g_iLogLevel As Integer
    
    Public g_bUnifiedLogon As Boolean
    
    Public g_sUnifiedLogonUserName As String = ""

    ' RFC010202 - New Global variables needed for the Stateless Clientmanager
    
    Public g_iSourceID As Integer
    
    Public g_iUserID As Integer
    
    Public g_iCountryID As Integer
    Public g_sSourceName As String = ""
    
    Public g_iCurrencyID As Integer
    
    Public g_lPartyCnt As Integer
    
    Public g_sCallingAppName As String = ""
    
    Public g_bStatelessClientManager As Boolean

    ' RDC 17102000 constants for timebomb checking
    Public Const TBStatusRestricted As Integer = 0
    Public Const TBStatusUnrestricted As Integer = 1
    Public Const TBStatusNoWarning As Integer = 2
    Public Const TBStatusWarning As Integer = 3
    Public Const TBStatusNoAccess As Integer = 4
    Public Const TBStatusNotStarted As Integer = 5

    ' Is a  link to PM Broking Required
    
    Public g_bPMBLinkRequired As Boolean
    ' Are we logged on to PM Broking
    
    Public g_bLoggedOnToPMB As Boolean
    ' The PMB Company we have selected
    
    Public g_sPMBCompanyNumber As String = ""
    ' Was a Local Logon Requested
    
    ' RFC 17061998 - Server Printer Details
    
    Public g_sServerPrinter As String = ""
    
    Public g_iIsPrinterChangeable As Integer

    ' Public instance of the client manager.
    'changes as per vb6 code
    ' Public g_oClientManager As BCLIENTMANAGER.ClientManager

    Public g_oClientManager As Object

    ' Public instance of the client status manager.

    ' Public g_oStatusManager As iLogonStatusManager.LogonStatusManager

    ' RDC 12082003 public property on class LogonManager
    ' Checked by ObjectManager

    Public g_bLogonInProgress As Boolean

    'RFC161098 - Keep a count of how many apps have a reference to LogonManager
    'RFC161098 - With only Logon Status Manager running the count will be 1.
    
    Public g_lAppReferenceCount As Integer

    Sub Main()

        ' Main entry point for the component

        ' Must initialise the public user name, so
        ' we can evaluate it later.
        g_sUserName = ""

    End Sub

    ' RFC100299 - Check Client Installations
    ' ***************************************************************** '
    ' Name: CheckClientInstallation
    '
    ' Description: NO LONGER USED
    '
    ' ***************************************************************** '
    Public Function CheckClientInstallation(ByVal v_lPMEProductFamily As Integer) As Integer

        Dim result As Integer = 0
        Dim oInstallCheck As iPMClientInstallCheck.CheckInstall
        Dim m_lreturn As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            oInstallCheck = New iPMClientInstallCheck.CheckInstall()

            m_lreturn = oInstallCheck.Initialise()
            If (m_lreturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            m_lreturn = oInstallCheck.CheckClientInstalls(g_oClientManager)
            If (m_lreturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oInstallCheck.Dispose()
            oInstallCheck = Nothing

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,sMsg:="CheckClientInstallationFailed",vApp:=ACApp,vClass:=ACClass,vMethod:="CheckClientInstallation",vErrNo:=Err.Number,vErrDesc:=Err.Description, excep:=ex)

            Return result
        End Try

    End Function

    '' ***************************************************************** '
    '' Name: LogMessage
    ''
    '' Description: Dummy LogMessage function.
    ''              All code uses LogMessagePopup to display errors.
    ''              This function is used as some of the functions in
    ''              GeneralFunc.bas use LogMessage().
    ''
    '' ***************************************************************** '
    'Public Sub LogMessage(iType As Integer, sMsg As String, Optional vApp, Optional vClass, Optional vMethod, Optional vErrNo, Optional vErrDesc)
    '
    '    LogMessagePopup _
    ''        iType:=iType, _
    ''        sMsg:=sMsg, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    'End Sub
    '
    '' ***************************************************************** '
    '' Name: Encrypt
    ''
    '' Description: Encrypts string passed and returns the result.
    ''
    '' ***************************************************************** '
    'Public Function Encrypt(sPassword As String, sEncryptedPassword) As Long
    '
    'Dim sAString As String
    'Dim sBString As String
    'Dim iCntr As Integer
    'Dim iCntr2 As Integer
    'Dim sChar1 As String * 1
    'Dim sChar2 As String * 1
    'Dim iSn As Integer
    'Dim sCodeString As String
    'Dim iClen As Integer
    '
    '    On Error GoTo Err_Encrypt
    '
    '    Encrypt = PMTrue
    '
    '    ' Encrypts the supplied string returning the encrypted
    '    ' result. Encrypted string will always be 2 characters
    '    ' longer than original (leave space!)
    '    '
    '    ' Encrypted string contains only ASCII characters in
    '    ' range 32-126
    '
    '    sCodeString$ = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
    '    iClen% = Len(sCodeString$)
    '
    '    sAString$ = sPassword$
    '    iCntr% = Len(sAString$)
    '
    '    If (iCntr% < 1) Then
    '        Encrypt = PMFalse
    '
    '        sEncryptedPassword = ""
    '
    '        Exit Function
    '    End If
    '
    '    sChar1$ = Mid$(sCodeString$, (((Asc(Left$(sAString$, 1)) + iCntr%) Mod iClen%) + 1), 1)
    '    sChar2$ = Mid$(sCodeString$, ((Asc(Right$(sAString$, 1)) Mod iClen%) + 1), 1)
    '    iSn = ((Asc(sChar1$) + Asc(sChar2$)) Mod iClen%) + 1
    '    sBString$ = sChar2$
    '
    '    For iCntr2% = 1 To iCntr%
    '        sBString$ = sBString$ + Mid$(sCodeString$, ((Asc(Mid$(sAString$, iCntr2%, 1)) + _
    ''            iSn% + iCntr2%) Mod iClen%) + 1, 1)
    '    Next iCntr2%
    '
    '    sBString$ = sBString$ + sChar1$
    '
    '    ' Return the result.
    '    sEncryptedPassword = Trim$(sBString$)
    '
    '    Exit Function
    '
    'Err_Encrypt:
    '
    '    ' Error Section
    '
    '    Encrypt = PMError
    '
    '    sEncryptedPassword = ""
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to encrypt the string", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Encrypt", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
End Module
