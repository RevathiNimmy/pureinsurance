Option Strict Off
Option Explicit On
'developer guide no.129
Imports SSP.Shared
Public Interface LicenceManager
    Property WarningMessage As String
    Property ErrorMessage As String
    Function Initialise() As Integer
    Function Logon(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_sClientSystemName As String, ByRef r_bPMBLinkRequired As Boolean, ByRef r_oClientManager As BCLIENTMANAGER.ClientManager) As Integer
    Sub Dispose()
End Interface
<System.Runtime.InteropServices.ProgId("LicenceManager_CoClass_NET.LicenceManager_CoClass")>
Public NotInheritable Class LicenceManager_CoClass
    Implements LicenceManager
    ' ***************************************************************** '
    ' Class Name: LicenceManager
    '
    ' Date: 21 April 1998
    '
    ' Description: Policy Master Licence Management.
    '
    ' Edit History:
    ' RFC 21/04/1998 - Original
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Maximum number of instances allowed to be
    ' instatiated at once.
    Private m_iLicenceLimit As Integer
    ' The size of the pool of free objects the
    ' Licence Manager maintains.
    Private m_iPoolSize As Integer
    Private m_iHomeCountryID As Integer


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "LicenceManager"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '              Checks the licence limit.
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements LicenceManager.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '
            '    ' *******************************************************************
            '    ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            '    ' Set Username and Password
            '    m_sUsername$ = sUserName$
            '    m_sPassword$ = sPassword$
            '    m_iUserID% = iUserID%
            '    m_sCallingAppName$ = sCallingAppName$
            '    m_iLanguageID% = iLanguageID%
            '    m_iSourceID% = iSourceID%
            '    m_iCurrencyID% = iCurrencyID%
            '    m_iLogLevel% = iLogLevel%

            ' Check that we have a valid Licence and
            ' that we are not exceeding it.

            Return CheckLicenceLimit(ErrorMessage, WarningMessage)

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Logon
    '
    ' Description: Logs a user onto the system.
    '              Checks Licence Limit and Password details first.
    ' ***************************************************************** '
    Public Function Logon(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_sClientSystemName As String, ByRef r_bPMBLinkRequired As Boolean, ByRef r_oClientManager As BCLIENTMANAGER.ClientManager) As Integer Implements LicenceManager.Logon

        Dim result As Integer = 0
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' r_oClientManager = Nothing

            ' Check that we have a valid Licence and
            ' that we are not exceeding it.
            result = CheckLicenceLimit(ignoreMessage:=False)

            ' If PMInvalidLicenceKey OR PMLicenceExceeded ,
            ' OR any other error, EXIT Function
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' All OK
            ' Get a free instance of the client manager
            result = GetClientManager(r_oClientManager)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Call the logon method of the client manager.
            result = r_oClientManager.Logon(sUsername:=v_sUsername, sPassword:=v_sPassword, sLoggedOnAtClient:=r_sClientSystemName, bPMBLinkRequired:=r_bPMBLinkRequired)

            ' If logon failed, destroy Client Manager
            If result <> gPMConstants.PMEReturnCode.PMTrue AndAlso result <> gPMConstants.PMEReturnCode.PMUserPasswordExpired AndAlso result <> gPMConstants.PMEReturnCode.PMUserTemporaryPassword AndAlso result <> gPMConstants.PMEReturnCode.PMUserWeakPassword AndAlso result <> gPMConstants.PMEReturnCode.PMNewBuildUpgrade Then

                r_oClientManager.Dispose()
                r_oClientManager = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the return object to nothing.

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Logon object", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Public Property WarningMessage As String Implements LicenceManager.WarningMessage
    Public Property ErrorMessage As String Implements LicenceManager.ErrorMessage


    Public Sub Dispose() Implements LicenceManager.Dispose
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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetClientManager
    '
    ' Description: Return a free instance of the client manager to the
    '              client.
    '
    ' ***************************************************************** '
    Private Function GetClientManager(ByRef oClientManager As BCLIENTMANAGER.ClientManager) As Integer
        Dim result As Integer = 0
        Dim sInProc As String = ""
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' RFC231100 - Use InProcess Client Manager to run in COM+
        sInProc = ""

        ' Get the PMDAO Version Mode
        ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\
        lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=gPMConstants.ACRegKeyClientManagerCOMPlus, r_sSettingValue:=sInProc)

        'Modified by Vikas Kapoor on 5/10/2010 7:58:08 PM Code commented as bPMClientManager project is deleted.
        ' If In Process is required
        'If StringsHelper.ToDoubleSafe(sInProc) = 1 Then
        ' Creat a new Instance of the InProcess Client Manager (COM+)
        '	oClientManager = New bPMClientManager.ClientManager()
        'Else

        ' Creat a new Instance of the Out of Process Client Manager
        oClientManager = New BCLIENTMANAGER.ClientManager()
        'End If

        ' Initialise Client Manager


        ' Not Setting the following properties deliberately
        ' .ID       - Used to be used by LicenceManager
        ' .Parent   - Used to be used by LicencManager
        ' .PMLink   - Used by the broking link. Set by LogonManager.
        ' .Company  - Used by the broking link. Set by LogonManager.

        Return oClientManager.Initialise(iSourceID:=m_iSourceID, iCountryID:=m_iHomeCountryID, iLanguageId:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, sCallingAppName:=m_sCallingAppName)

    End Function

    ' ***************************************************************** '
    ' Name: CheckLicenceLimit
    '
    ' Description: Gets the licence limit and checks that it will not be
    '              exceeded.
    ' ***************************************************************** '
    Private Function CheckLicenceLimit(Optional ByRef errorMessage As String = "", Optional ByRef warningMaessage As String = "", Optional ByVal ignoreMessage As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.Business

        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim iSystemID, iProductID As Integer
        Dim sSystemName, sLicenceKey As String
        Dim vTimestamp As Object
        Dim iLicencesInUse As Integer
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create an instance of the system object into memory.
        oPMSystem = New BPMSYSTEM.Business()

        ' Call the initialise method..
        'developers guide no 9
        lErrorValue = oPMSystem.Initialise(sUserName:="", sPassword:="", iUserID:=m_iUserID, iLanguageID:=m_iLanguageID, iSourceID:=m_iSourceID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Dim iccs As String = String.Empty
        result = CType(oPMSystem.GetICCS(iccs), gPMConstants.PMEReturnCode)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Dim oLicense As New LicenseManager()
        '    Dim errorMessage As String = String.Empty
        Dim isValid As Boolean = oLicense.IsThisLicenseValid(iccs, errorMessage)

        If (isValid = False) Then
            Return PMEReturnCode.PMInvalidLicenceKey
        Else

            ' Get system details from the system object.
            lErrorValue = oPMSystem.GetValidSystem(sProductCode:=gPMConstants.PMProduct, iSystemID:=iSystemID, iProductID:=iProductID, sSystemName:=sSystemName, iDefaultSourceID:=m_iSourceID, iHomeCountryID:=m_iHomeCountryID, iCurrencyID:=m_iCurrencyID, iLanguageID:=m_iLanguageID, iLicenceLimit:=m_iLicenceLimit, sLicenceKey:=sLicenceKey, iLogLevel:=m_iLogLevel, iPoolSize:=m_iPoolSize, vTimestamp:=vTimestamp)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Release PMSystem
                oPMSystem.Dispose()
                oPMSystem = Nothing

                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Valid System Record for " & gPMConstants.PMProduct & "/" & gPMConstants.PMCustomer, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLicenceLimit")

                Return PMEReturnCode.PMInvalidLicenceKey

            End If
            m_iLicenceLimit = oLicense.Quantity
            ' Get the Licences In Use
            lErrorValue = oPMSystem.GetLicencesInUse(r_iLicencesInUse:=iLicencesInUse)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Release PMSystem
                oPMSystem.Dispose()
                oPMSystem = Nothing
                Return result
            End If

            ' Have we exceeded the limit.
            If iLicencesInUse >= m_iLicenceLimit Then
                ' Yes, so return LicenceExceeded
                result = gPMConstants.PMEReturnCode.PMLicenceExceeded
            End If
            If ignoreMessage = False Then
                If (oLicense.ExpirationDays <= oLicense.LoginReminderToStart) Then
                    If oLicense.ExpirationDays <= 1 Then
                        warningMaessage = "License will expire today."
                    ElseIf oLicense.ExpirationDays > 1 And oLicense.ExpirationDays <= 2 Then
                        warningMaessage = "License will expire tomorrow."
                    Else
                        warningMaessage = "License will expire after " + oLicense.ExpirationDays.ToString() + " days."
                    End If
                End If
            End If

            ' Call the terminate method.
            oPMSystem.Dispose()
            ' Release Instance
            oPMSystem = Nothing

        End If
        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try


    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

