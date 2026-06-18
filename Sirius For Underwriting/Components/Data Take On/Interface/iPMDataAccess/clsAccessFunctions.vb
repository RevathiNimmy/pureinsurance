Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend NotInheritable Class clsAccessFunctions

    Implements IDisposable
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Public vPersonalContactArray As Object
    Public vCorporateContactArray As Object

    Private m_lReturn As Integer

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: OpenAccessDatabase
    '
    ' Description:
    '
    ' History: 16/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function OpenAccessDatabase() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oAccessDatabase Is Nothing Then
                m_oAccessDatabase = New dPMDAO.Database()
            End If

            lResult = CType(m_oAccessDatabase.OpenDatabase("sirius", 1, 1, "iPMDataAccess", , , , "Gemini"), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenAccessDatabase Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="OpenAccessDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 16/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(Optional ByRef sUsername As String = "", Optional ByRef sPassword As String = "", Optional ByRef iUserID As Integer = 0, Optional ByRef iSourceID As Integer = 0, Optional ByRef iLanguageID As Integer = 0, Optional ByRef iCurrencyID As Integer = 0, Optional ByRef iLogLevel As Integer = 0, Optional ByRef sCallingAppName As String = "", Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oObjectManager = New bObjectManager.ObjectManager()

            lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACAPP)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    MessageBox.Show("Failed to create new instance of bObjectManager.ObjectManager", "Fatal Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                m_oObjectManager = Nothing
                Return lReturn
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With m_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
                g_sUserName = .UserName
                g_sPassword = .Password
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTypeOfBusinessGeneric
            m_dtEffectiveDate = DateTime.Now

            'set username and password
            '    g_sUserName$ = sUserName
            '    g_sPassword$ = sPassword
            '    g_iUserId% = iUserID
            '    g_sCallingAppName$ = sCallingAppName
            '    g_iLanguageID% = iLanguageID
            '    g_iSourceID% = iSourceID
            '    g_iCurrencyID% = iCurrencyID%
            '    g_iLogLevel% = iLogLevel%

            'Set the objects
            m_oDatabase = New dPMDAO.Database()

            Dim temp_m_oParty As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oParty = temp_m_oParty

            'Error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Dim temp_m_oInsuranceFile As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFile = temp_m_oInsuranceFile

            'Error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Dim temp_m_oUserDetail As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oUserDetail, "bGisUserDefDetail.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserDetail = temp_m_oUserDetail

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Dim temp_m_oLookupMaintenance As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oLookupMaintenance, "bPMMaintainLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oLookupMaintenance = temp_m_oLookupMaintenance

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


            lReturn = m_oLookupMaintenance.ConnectDatabase(v_lProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Dim temp_m_oArchLookupMaintenance As Object
            lReturn = m_oObjectManager.GetInstance(temp_m_oArchLookupMaintenance, "bPMMaintainLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oArchLookupMaintenance = temp_m_oArchLookupMaintenance

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


            lReturn = m_oArchLookupMaintenance.ConnectDatabase(v_lProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 16/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oParty IsNot Nothing Then
                    m_oParty.Dispose()
                    m_oParty = Nothing
                End If
                If m_oInsuranceFile IsNot Nothing Then
                    m_oInsuranceFile.Dispose()
                    m_oInsuranceFile = Nothing
                End If
                If Not (m_oDatabase Is Nothing) Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If Not (m_oArchitectureDatabase Is Nothing) Then
                    m_oArchitectureDatabase.CloseDatabase()
                    m_oArchitectureDatabase = Nothing
                End If
                If m_oLookupMaintenance IsNot Nothing Then
                    m_oLookupMaintenance.Dispose()
                    m_oLookupMaintenance = Nothing
                End If
                If m_oArchLookupMaintenance IsNot Nothing Then
                    m_oArchLookupMaintenance.Dispose()
                    m_oArchLookupMaintenance = Nothing
                End If
                If m_oAddressObject IsNot Nothing Then
                    m_oAddressObject.Dispose()
                    m_oAddressObject = Nothing
                End If
                If m_oPolicyNumber IsNot Nothing Then
                    m_oPolicyNumber.Dispose()
                    m_oPolicyNumber = Nothing
                End If
                If m_oOrionUpdate IsNot Nothing Then
                    m_oOrionUpdate.Dispose()
                    m_oOrionUpdate = Nothing
                End If
                m_oAccessDatabase = Nothing
                m_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: ReopenParty
    '
    ' Description:
    '
    ' History: 17/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function ReopenParty() As Integer

        Dim result As Integer = 0
        Dim lResult As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oParty Is Nothing) Then

                m_oParty.Dispose()
            End If

            m_oParty = Nothing

            Dim temp_m_oParty As Object
            lResult = m_oObjectManager.GetInstance(temp_m_oParty, "bSIRParty.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oParty = temp_m_oParty

            'Error
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReopenParty Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="ReopenParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenSiriusDatabase
    '
    ' Description:
    '
    ' History: 17/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function OpenSiriusDatabase() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResult = CType(m_oDatabase.OpenDatabase("sirius", 1, 1, "iPMDataAccess", "sa", "", "SiriusSolutions"), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenSiriusDatabase Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="OpenSiriusDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CloseDatabase
    '
    ' Description:
    '
    ' History: 17/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function CloseDatabase() As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase Is Nothing Then
                Return result
            End If

            lResult = m_oDatabase.CloseDatabase()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseDatabase Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CloseDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CloseAccessDatabase
    '
    ' Description:
    '
    ' History: 17/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function CloseAccessDatabase() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oAccessDatabase Is Nothing Then
                Return result
            End If

            lResult = m_oAccessDatabase.CloseDatabase()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseAccessDatabase Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CloseAccessDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: BeginTrans
    '
    ' Description:
    '
    ' History: 31/08/2000 MSB - Created.
    '
    ' ***************************************************************** '

    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResult = m_oDatabase.SQLBeginTrans()

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CommitTrans
    '
    ' Description:
    '
    ' History: 31/08/2000 MSB - Created.
    '
    ' ***************************************************************** '

    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResult = m_oDatabase.SQLCommitTrans()

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RollOverTrans
    '
    ' Description:
    '
    ' History: 31/08/2000 MSB - Created.
    '
    ' ***************************************************************** '

    Public Function RollOverTrans() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResult = m_oDatabase.SQLRollbackTrans()

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollOverTrans Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="RollOverTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenSiriusArchDatabase
    '
    ' Description:
    '
    ' History: 01/09/2000 MSB - Created.
    '
    ' ***************************************************************** '

    Public Function OpenSiriusArchDatabase() As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oArchitectureDatabase Is Nothing Then
                m_oArchitectureDatabase = New dPMDAO.Database()
            End If

            lResult = CType(m_oArchitectureDatabase.OpenDatabase("sirius", 1, 1, "iPMDataAccess", "sa", , "SiriusArchitecture"), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenSiriusArchDatabase Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="OpenSiriusArchDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CloseArchitectureDatabase
    '
    ' Description:
    '
    ' History: 17/08/2000 MSB - Created.
    '
    ' ***************************************************************** '
    Public Function CloseArchitectureDatabase() As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oArchitectureDatabase Is Nothing Then
                Return result
            End If

            lResult = m_oArchitectureDatabase.CloseDatabase()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseArchitectureDatabase Failed", vApp:=ACAPP, vClass:=ACClass, vMethod:="CloseArchitectureDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
