Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("LicenceManager_NET.LicenceManager")> _
Public NotInheritable Class LicenceManager
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: LicenceManager
    '
    ' Date: 03 July 1996
    '
    ' Description: Main Class to accompany the interface form.
    '
    ' Edit History:
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
    Private g_iLicenceLimit As Integer
    ' The size of the pool of free objects the
    ' Licence Manager maintains.
    Private g_iPoolSize As Integer
    Private g_iHomeCountryID As Integer

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
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
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

            Return CheckLicenceLimit()

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Logon
    '
    ' Description: Logs a user onto the system.
    '              Checks Licence Limit and Password details first.
    ' ***************************************************************** '
    Public Function Logon(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_sClientSystemName As String, ByRef r_oClientManager As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_oClientManager = Nothing

            ' Check that we have a valid Licence and
            ' that we are not exceeding it.
            result = CheckLicenceLimit()

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

            result = r_oClientManager.Logon(sUserName:=v_sUsername, sPassword:=v_sPassword, sLoggedOnAtClient:=r_sClientSystemName)

            ' If logon failed, destroy Client Manager
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_oClientManager.Dispose()
                r_oClientManager = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the return object to nothing.

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Logon object", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetClientManager
    '
    ' Description: Return a free instance of the client manager to the
    '              client.
    '
    ' ***************************************************************** '
    Private Function GetClientManager(ByRef oClientManager As Object) As Integer
        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Creat a new Instance of Client Manager
        oClientManager = New bClientManager.ClientManager()

        ' Initialise Client Manager

        ' Not Setting the following properties deliberately
        ' .ID       - Used to be used by LicenceManager
        ' .Parent   - Used to be used by LicencManager
        ' .PMLink   - Used by the broking link. Set by LogonManager.
        ' .Company  - Used by the broking link. Set by LogonManager.

        Return oClientManager.Initialise(iSourceID:=m_iSourceID, iCountryID:=g_iHomeCountryID, iLanguageID:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, sCallingAppName:=m_sCallingAppName)

    End Function

    ' ***************************************************************** '
    ' Name: CheckLicenceLimit
    '
    ' Description: Gets the licence limit and checks that it will not be
    '              exceeded.
    ' ***************************************************************** '
    Private Function CheckLicenceLimit() As Integer
        Dim result As Integer = 0
        Dim oPMSystem As bPMSystem.Business

        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim iSystemID, iProductID As Integer
        Dim sSystemName, sLicenceKey As String
        Dim vTimestamp As Object
        Dim iLicencesInUse As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create an instance of the system object into memory.
        oPMSystem = New bPMSystem.Business()

        ' Call the initialise method.
        lErrorValue = CType(oPMSystem, SSP.S4I.Interfaces.IBusiness).Initialise(sUserName:="", sPassword:="", iUserID:=m_iUserID, iLanguageID:=m_iLanguageID, iSourceID:=m_iSourceID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get system details from the system object.
        lErrorValue = oPMSystem.GetValidSystem(sProductCode:=gPMConstants.PMProduct, iSystemID:=iSystemID, iProductID:=iProductID, sSystemName:=sSystemName, iDefaultSourceID:=m_iSourceID, iHomeCountryID:=g_iHomeCountryID, iCurrencyID:=m_iCurrencyID, iLanguageID:=m_iLanguageID, iLicenceLimit:=g_iLicenceLimit, sLicenceKey:=sLicenceKey, iLogLevel:=m_iLogLevel, iPoolSize:=g_iPoolSize, vTimestamp:=vTimestamp)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then

            ' System Record is not OK, we must return the error value.
            result = gPMConstants.PMEReturnCode.PMInvalidLicenceKey

            ' Set Licence details to zero.
            g_iLicenceLimit = 0

            ' Release PMSystem
            oPMSystem.Dispose()
            oPMSystem = Nothing

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Valid System Record for " & gPMConstants.PMProduct & "/" & gPMConstants.PMCustomer, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLicenceLimit")

            Return result

        End If

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
        If iLicencesInUse >= g_iLicenceLimit Then
            ' Yes, so return LicenceExceeded
            result = gPMConstants.PMEReturnCode.PMLicenceExceeded
        End If

        ' Call the terminate method.
        oPMSystem.Dispose()
        ' Release Instance
        oPMSystem = Nothing

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.



    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

