Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms

Imports SharedFiles
<ComClass()> _
<Serializable()> _
<System.Runtime.InteropServices.ProgId("LogonManager_NET.LogonManager")> _
Public NotInheritable Class LogonManager
    Inherits MarshalByRefObject
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: LogonManager
    '
    ' Date: 03 July 1996
    '
    ' Description: Main Class to accompany the interface form.
    '
    ' Edit History:
    ' RFC 23/04/1998 - Added LoggedOnLocally, LoggedOnToPMB and
    ' RFC 23/04/1998 - PMBCompanyNumber properties.
    ' RFC 17/06/1998 - Server Printer Details added.
    ' RFC161098 - Keep a count of how many apps have a reference to LogonManager
    ' RFC161098 - ApplicationsRunning property added. Used by LogonStatus
    '             Manager to display a warning message if the user chooses
    '             to logoff when there are apps running.
    ' RFC100299 - Check Client Installations (CheckClientInstall Method)
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "LogonManager"

    Private Shared m_logoninstance As LogonManager

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Form cancelled state.
    Private m_bCancelled As Boolean

    ' RDC 15042002 - return code
    Private m_lReturn As Integer

    Private m_iViewGraphics As gPMConstants.PMEReturnCode

    ' RDC 11072002
    Private m_bUnifiedLogon As Boolean
    Private m_sUnifiedLogonUsername As String = ""
    ' PRIVATE Data Members (End)
    'developer guide no. 107
    <ThreadStatic()> _
    Public Shared objFrmInterface As frmInterface
    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property m_obclientManager() As bClientManager.ClientManager
        Get
            Return g_oClientManager
        End Get
    End Property
    Public ReadOnly Property UnifiedLogon() As Boolean
        Get
            Return g_bUnifiedLogon
        End Get
    End Property

    Public ReadOnly Property UnifiedLogonUsername() As String
        Get
            Return g_sUnifiedLogonUserName
        End Get
    End Property
    ' PUBLIC Property Procedures (End)

    ' PUBLIC Property Procedures (Begin)

    ' RFC010202 - Logon Manager will drop the Client Manager if it is the Stateless version
    Public ReadOnly Property StatelessClientManager() As Boolean
        Get
            Return g_bStatelessClientManager
        End Get
    End Property

    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property AppReferenceCount() As Integer
        Get
            ' If the Reference Count is greater than one then there
            ' are other Applications running.
            Return g_lAppReferenceCount
        End Get
        Set(ByVal value As Integer)
            g_lAppReferenceCount = value
        End Set
    End Property

    Public Property Cancelled() As Boolean
        Get

            ' Standard Property.

            ' Return the cancelled flag from the form.
            Return m_bCancelled

        End Get
        Set(ByVal Value As Boolean)

            ' Standard Property.

            ' set the cancelled flag from the form.
            m_bCancelled = Value

        End Set
    End Property

    Public ReadOnly Property UserName() As String
        Get

            ' Return the user name from the public
            ' variable.
            Return g_sUserName

        End Get
    End Property

    Public ReadOnly Property Userid() As Integer
        Get

            ' Return the user name from the public
            ' variable.
            Return g_iUserID

        End Get
    End Property

    ' RDC 12082003 true until all dialogues complete
    Public ReadOnly Property LogonInProgress() As Boolean
        Get
            Return g_bLogonInProgress
        End Get
    End Property

    Public ReadOnly Property SourceID() As Integer
        Get

            ' Return the Source ID from the public
            ' variable.
            Return g_iSourceID

        End Get
    End Property

    Public ReadOnly Property SourceName() As String
        Get

            ' Return the user name from the public
            ' variable.
            Return g_sSourceName

        End Get
    End Property
    Public ReadOnly Property CurrencyID() As Integer
        Get

            ' Return the user name from the public
            ' variable.
            Return g_iCurrencyID

        End Get
    End Property
    Public ReadOnly Property CountryID() As Integer
        Get

            ' Return the user name from the public
            ' variable.
            Return g_iCountryID

        End Get
    End Property

    Public ReadOnly Property ClientManager() As Object
        Get


            Dim result As Object = Nothing
            Dim lReturn As Integer

            ' RFC010202 - Are we using the Stateless ClientManager
            If g_bStatelessClientManager Then

                ' RFC010202 - We are already logged on so, just create an Client Manager
                '             and re-hydrate it with the Users data.
                lReturn = RecreateClientManager(g_oClientManager)

                'Return the instance of clientmanager
                result = g_oClientManager

                ' RFC010202 - Release LogonManager reference to ClientManager
                '             so that we are not holding resources on the Server.
                g_oClientManager = Nothing

            Else

                'Return the instance of clientmanager
                result = g_oClientManager

            End If

            Return result
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get

            ' Return the password from the public
            ' variable.
            Return g_sPassword

        End Get
    End Property

    Public ReadOnly Property LogonTime() As Date
        Get

            ' Return the date and time the user
            ' logged on.
            Return g_dLogonTime

        End Get
    End Property

    Public ReadOnly Property LogLevel() As Integer
        Get

            ' Return the system log level.
            Return g_iLogLevel

        End Get
    End Property

    Public ReadOnly Property LanguageID() As Integer
        Get

            ' Return the language ID.
            Return g_iLanguageID

        End Get
    End Property

    ' Is the user Logged on Locally
    Public ReadOnly Property LoggedOnLocally() As Boolean
        Get
            Return True
        End Get
    End Property
    ' Is the User Logged On To PM Broking
    Public ReadOnly Property LoggedOnToPMB() As Boolean
        Get
            Return g_bLoggedOnToPMB
        End Get
    End Property
    ' The PM Broking Company selected by the user
    Public ReadOnly Property PMBCompanyNumber() As String
        Get
            Return g_sPMBCompanyNumber
        End Get
    End Property
    ' RFC 17061998 - Server Printer Details
    ' The Server Printer
    Public ReadOnly Property ServerPrinter() As String
        Get
            Return g_sServerPrinter
        End Get
    End Property
    ' Is the User allowed to change the Printer
    Public ReadOnly Property IsPrinterChangeable() As Integer
        Get
            ' Return the language ID.
            Return g_iIsPrinterChangeable
        End Get
    End Property

    'RFC161098
    Public ReadOnly Property ApplicationsRunning() As Boolean
        Get
            ' If the Reference Count is greater than one then there
            ' are other Applications running.
            Return g_lAppReferenceCount > 1
        End Get
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '

    Public Function Initialise(ByRef oClientManager As Object) As Integer

        Dim result As Integer = 0
        Dim lErrorValue As gPMConstants.PMEReturnCode
        'DAK130100
        Dim sViewGraphics As String = ""
        ' RDC 11072002

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Initialisation Code.

        ' The first application that uses this object will
        ' cause the following code section to be performed.
        ' We do this by checking the public username.

        'DAK130100
        ' Get the View Graphics setting
        lErrorValue = CType(gPMFunctions.GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, gPMConstants.PMERegSettingLevel.pmeRSLClient, gPMConstants.ACRegKeyViewGraphics, sViewGraphics), gPMConstants.PMEReturnCode)

        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Default is PMTrue - otherwise whatever it is set to
        Dim dbNumericTemp As Double
        If sViewGraphics.Trim() = "" Then
            m_iViewGraphics = gPMConstants.PMEReturnCode.PMTrue
            ' Is it Numeric
        ElseIf (Double.TryParse(sViewGraphics, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
            m_iViewGraphics = CType(CInt(sViewGraphics), gPMConstants.PMEReturnCode)
        Else
            m_iViewGraphics = gPMConstants.PMEReturnCode.PMTrue
        End If

        m_bUnifiedLogon = True 'Automatically try to logon using 'Unified' mode

        If Not m_bUnifiedLogon Then
            m_sUnifiedLogonUsername = ""
        Else
            lErrorValue = CType(gPMFunctions.GetNTUsernameEx(m_sUnifiedLogonUsername), gPMConstants.PMEReturnCode)

            If lErrorValue = gPMConstants.PMEReturnCode.PMError Then
                'If this function errors then it could mean that this is an NT machine
                'and does not have secur32.dll. We should continue without unified logon.
                m_sUnifiedLogonUsername = ""
                m_bUnifiedLogon = False
            ElseIf lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'MessageBox.Show(g_sUserName.Trim())
        'Process.GetProcessesByName("iLogonManager")
        'g_sUserName = ""
        If g_sUserName.Trim() = "" Then

            ' RDC 12082003
            g_bLogonInProgress = True

            ' Call the method to process the form
            ' logon.
            lErrorValue = CType(ProcessFormMode(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the form.
                result = gPMConstants.PMEReturnCode.PMFalse

                g_oClientManager = Nothing

                Return result
            End If

            ' RDC 08082002 source (branch) selection
            Static sPurePath As String = ""

            If sPurePath = "" Then
                gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPurePath)
                sPurePath &= "\Pure\Application\"
            End If

            If lErrorValue = gPMConstants.PMEReturnCode.PMTrue And Not (g_oClientManager Is Nothing) Then
                lErrorValue = CType(SourceSelection(), gPMConstants.PMEReturnCode)
                System.Diagnostics.Process.Start(sPurePath & "iLogonStatusManager")
            End If

            ' RFC010202 - Check to see if this is the Stateless ClientManager or not

            Try
                g_bStatelessClientManager = g_oClientManager.StatelessClientManager

            Catch
            End Try



            ' RFC010202 - Are we using the Stateless ClientManager
            If g_bStatelessClientManager Then
                'RFC010202 - Release LogonManager reference to ClientManager
                '  so that we are not holding resources on the Server.
                g_oClientManager = Nothing
            End If

            ' RDC 12082003
            g_bLogonInProgress = False

        End If

        'MessageBox.Show("ilogon " & g_oClientManager.UserName)
        ' Return the instance of the public client manager.
        'oClientManager = New bClientManager.ClientManager
        'oClientManager.UserName = g_oClientManager.UserName
        oClientManager = g_oClientManager

        ' if cancelled property is true return pmcancel
        If Cancelled Then
            result = gPMConstants.PMEReturnCode.PMCancel
        End If

        ' If the client manager is nothing, and not cancelled we return false.
        If g_oClientManager Is Nothing And Cancelled = gPMConstants.PMEReturnCode.PMFalse Then
            If Not g_bStatelessClientManager Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'Display for debug purposes only.
        'Debug.Print "Interface Entry: Initialised"
        'MessageBox.Show("ilogon " & oClientManager.UserName)
        Return result

Err_Initialise:

        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        g_oClientManager = Nothing

        ' Log Error.
        gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise object", ACApp, ACClass, "Initialise", excep:=New Exception(Information.Err().Description))

        Return result

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
            If Not m_bCancelled Then
                ' no more apps and logon wasn't cancelled
                'If StatelessClientManager Then
                ' RDC 03062002 we're using bPMClientManager, so
                ' logoff method needs to be called
                m_lReturn = ClientManagerLogoff()
                'End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: ClientManagerLogoff
    '
    ' Description: Creates ClientManager and calls its logoff method
    '              CM used to run its own Logoff method when it terminated
    '              but it cannot do this in the COM+ version as it does
    '              not persist - we'll have to do it externally.
    '
    ' ***************************************************************** '
    Private Function ClientManagerLogoff() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse



        ' fire up and initialse ClientManager
        m_lReturn = RecreateClientManager(g_oClientManager)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' call CM's logoff method. This in turn calls bPMUser's logoff which
        ' sets 'logged_on_at_client' in database to NULL
        'MessageBox.Show("ClientManagerLogoff")
        'm_lReturn = g_oClientManager.Logoff()

        ' not really interested if it fails (but check the log). We just want to terminate it
        'MessageBox.Show("g_oClientManager.Terminate()")
        'm_lReturn = g_oClientManager.Terminate()
        g_oClientManager.Dispose()

        g_oClientManager = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' RFC100299 - Check Client Installations
    ' ***************************************************************** '
    ' Name: CheckClientInstall
    '
    ' Description: Checks to see if an install is required for the
    '              Supplied Product Family.
    '
    ' ***************************************************************** '
    Public Function CheckClientInstall(ByVal v_lPMEProductFamily As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return CheckClientInstallation(v_lPMEProductFamily)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClientInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPropertyValues
    '
    ' Description: Returns the System Proprty Values for the logged on user.
    '
    '              (This method is implemented for performance reasons,
    '              as it is quicker to call one method than it is to
    '              access eight property gets.
    '
    ' ***************************************************************** '
    Public Function GetPropertyValues(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iCountryID As Integer, ByRef iLanguageId As Integer, ByRef iLogLevel As Integer, ByRef iCurrencyID As Integer, ByRef lPartyCnt As Integer, ByRef sCallingAppName As String, Optional ByRef sServerPrinter As String = "", Optional ByRef iIsPrinterChangeable As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return Username and Password
            sUserName = g_sUserName
            sPassword = g_sPassword

            ' Return the User ID
            iUserID = g_iUserID

            ' Return the Client Manager System Values
            iSourceID = g_iSourceID
            iCountryID = g_iCountryID
            iLanguageId = g_iLanguageID
            iLogLevel = g_iLogLevel
            iCurrencyID = g_iCurrencyID
            lPartyCnt = g_lPartyCnt
            sCallingAppName = g_sCallingAppName
            sServerPrinter = g_sServerPrinter
            iIsPrinterChangeable = g_iIsPrinterChangeable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Return the SystemValues", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessFormMode (Standard Method)
    '
    ' Description: Calls the appropriate methods etc when using the
    '              interface form.
    '
    ' ***************************************************************** '
    Private Function ProcessFormMode() As Integer

        Dim result As Integer = 0
        Dim lErrorValue As gPMConstants.PMEReturnCode



        'start'
        objFrmInterface = New frmInterface
        'end'
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the form into memory.
        lErrorValue = CType(LoadForm(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the form.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the form from memory.
            lErrorValue = CType(UnloadForm(), gPMConstants.PMEReturnCode)
            Return result
        End If

        'Do the Unified Login
        If m_bUnifiedLogon Then


            lErrorValue = CType(objFrmInterface.DoUnifiedLogin(), gPMConstants.PMEReturnCode)
            'Check for return value
            'Show the Login Screen only if unified login failed
            'Don't Display if login failed for Unified Only Mode
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                m_bUnifiedLogon = False

                objFrmInterface.UnifiedLogon = False
                ' Display the form.
                lErrorValue = CType(ShowForm(), gPMConstants.PMEReturnCode)
                ' Check for errors.
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to display the form.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Destroy the form from memory.
                    lErrorValue = CType(UnloadForm(), gPMConstants.PMEReturnCode)
                    Return result
                End If
            End If
        Else
            ' Display the form.
            lErrorValue = CType(ShowForm(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the form.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Destroy the form from memory.
                lErrorValue = CType(UnloadForm(), gPMConstants.PMEReturnCode)
                Return result
            End If
        End If





        'check whether the form was cancelled


        If objFrmInterface.FormCancelled Then
            'set LogonManager Cancelled property to pmtrue
            Cancelled = True
        End If

        ' Destroy the form from memory.
        lErrorValue = CType(UnloadForm(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the form.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadForm (Standard Method)
    '
    ' Description: Loads the instance of the interface form into
    '              memory.
    '
    ' ***************************************************************** '
    Private Function LoadForm() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the instance of the interface
        ' form into memory.
        'object declared but never used

        If objFrmInterface.ErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        objFrmInterface.UnifiedLogon = m_bUnifiedLogon
        objFrmInterface.UnifiedLogonUsername = m_sUnifiedLogonUsername

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadForm (Standard Method)
    '
    ' Description: Unloads the instance of the interface form from
    '              memory.
    '
    ' ***************************************************************** '
    Private Function UnloadForm() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Destroy the instance of the interface
        ' form from memory.

        objFrmInterface.Dispose()
        objFrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Displays the instance of the interface form,
    '              modally.
    '
    ' ***************************************************************** '
    Private Function ShowForm() As Integer

        Dim result As Integer = 0


        ' Display's the interface form modally.

        objFrmInterface.ShowDialog()

        ' Check for any form errors.

        If (objFrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMTrue) Or (objFrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMCancel) Then
            result = gPMConstants.PMEReturnCode.PMTrue
        Else

            result = objFrmInterface.ErrorNumber
        End If

        ' Store the form's exit state.

        Cancelled = objFrmInterface.FormCancelled

        Return result

    End Function

    ' RFC010202 - Create a ClientManager and Rehydrate it with the users data
    ' ***************************************************************** '
    ' Name: RecreateClientManager
    '
    ' Description: Create a ClientManager and Rehydrate it with the users data
    '
    ' ***************************************************************** '

    Private Function RecreateClientManager(ByRef oClientManager As bClientManager.ClientManager) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        'Dim sServers(3)



        result = gPMConstants.PMEReturnCode.PMTrue

        '    sServers(0) = "scalelab002"
        '    sServers(1) = "scalelab003"
        '    sServers(2) = "scalelab004"
        '    sServers(3) = "scalelab005"

        '    Randomize
        '    lIndex = Int((4 * Rnd))

        '    sServer = sServers(lIndex)

        ' RDC 12042002 - remove benchtest-specific create

        oClientManager = New bClientManager.ClientManager()

        If oClientManager Is Nothing Then
            MessageBox.Show("Error reconecting to the Server Client Manager." & Strings.Chr(10).ToString() & _
                            "Contact you system Administrator to resolve the problem." & Strings.Chr(10).ToString(), "E0020 - Unable to Create Server ClientManager", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Initialise Client Manager
        'MessageBox.Show("oClientManager.Initialise")
        lReturn = CType(oClientManager.Initialise(iSourceID:=CShort(g_iSourceID), iCountryID:=CShort(g_iCountryID), iLanguageId:=CShort(g_iLanguageID), iLogLevel:=CShort(g_iLogLevel), iCurrencyID:=CShort(g_iCurrencyID), sCallingAppName:=g_sCallingAppName), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Error reconecting to the Server Client Manager." & Strings.Chr(10).ToString() & _
                            "Contact you system Administrator to resolve the problem." & Strings.Chr(10).ToString(), "E0021 - Unable to Initialise Server ClientManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return lReturn
        End If

        ' Re-hydrate the state relating to the User

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Error reconecting to the Server Client Manager." & Strings.Chr(10).ToString() & _
                            "Contact you system Administrator to resolve the problem." & Strings.Chr(10).ToString(), "E0022 - Unable to set properties on the Server ClientManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return lReturn
        End If

        Return result

    End Function

    Private Function SourceSelection() As Integer

        Dim result As Integer = 0
        Dim iSourceID As Integer
        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim sSourceName As String = ""
        Dim vSources As Object
        Dim oSource As frmSource
        Dim vOption As Object
        ' RDC 29112002
        Dim iLoop As Integer



        result = gPMConstants.PMEReturnCode.PMFalse

        ' RDC get multi-branch option from hidden_options
        ' hardcoded until code is transfered from gSIRLibrary to gPMFunctions
        ' RDC 17012003 changed from 16 to 22 Multi-BRANCH-accounting
        ' RDC 21012003 changed to new SIROPTEnableBranchSelectAtLogon (37)
        lErrorValue = CType(g_oClientManager.GetSystemOption(37, 1, vOption), gPMConstants.PMEReturnCode)


        If Convert.IsDBNull(vOption) Or IsNothing(vOption) OrElse vOption <> "1" Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        ' get all sources for this ID
        lErrorValue = CType(g_oClientManager.GetSources(iUserID:=CShort(g_iUserID), vSources:=vSources), gPMConstants.PMEReturnCode)

        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        If vSources.GetLowerBound(1) = vSources.GetUpperBound(1) Then
            ' there's only one source for this ID

            g_iSourceID = CInt(vSources(0, 0))
            g_iCountryID = CInt(vSources(2, 0))
            g_sSourceName = CStr(vSources(1, 0))

            g_oClientManager.SourceID = CShort(g_iSourceID)

            g_oClientManager.CountryID = CInt(vSources(2, 0))

            'g_oStatusManager.SourceID = g_iSourceID

            'g_oStatusManager.SourceName = CStr(vSources(1, 0))

            'g_oStatusManager.CountryID = CInt(vSources(2, 0))

            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        ' show sourceselection dialogue
        oSource = New frmSource()


        oSource.Sources = vSources


        'Load(oSource)

        oSource.ShowDialog()

        lErrorValue = oSource.Status

        iSourceID = oSource.SourceID
        sSourceName = oSource.SourceName

        oSource.Close()

        oSource = Nothing

        If lErrorValue <> gPMConstants.PMEReturnCode.PMOK Then
            ' dialogue failed or was cancelled
            Return result
        End If

        g_iSourceID = iSourceID


        For iLoop = vSources.GetLowerBound(1) To vSources.GetUpperBound(1)

            If CDbl(vSources(0, iLoop)) = iSourceID Then
                Exit For
            End If
        Next

        ' pass the sourceID/name to Client Manager and Logon Status Manager
        g_oClientManager.SourceID = CShort(iSourceID)

        g_oClientManager.CountryID = CInt(vSources(2, iLoop))

        g_iSourceID = iSourceID
        g_sSourceName = sSourceName
        g_iCountryID = CInt(vSources(2, iLoop))
        ' g_oStatusManager.SourceID = iSourceID
        ' g_oStatusManager.SourceName = sSourceName

        ' g_oStatusManager.CountryID = CInt(vSources(2, iLoop))


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    'PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        Try

            'RFC161098 - Increase the App Reference Count
            g_lAppReferenceCount += 1
            'MessageBox.Show(g_lAppReferenceCount)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error Message
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function
End Class

