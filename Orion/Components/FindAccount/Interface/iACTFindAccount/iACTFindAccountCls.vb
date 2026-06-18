Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 06 May 1997
    '
    ' Description: Main public class to accompany the interface form
    '
    ' Edit History:
    ' CJB 070405 PN14472 Set m_bNotEditable to True in Initialise as we only want to make visible
    '            if specifically set to False (in Account Explorer).
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAccountID As Integer
    Private m_iLedgerID As Integer
    Private m_iLedgerTypeID As Integer
    Private m_sShortCode As String = ""
    Private m_sFullKey As String = ""
    Private m_sAccountName As String = ""
    Private m_lAccountUIK As Integer
    Private m_lNominalAccountID As Integer
    Private m_bAllowStoppedAccounts As Boolean
    'eck090500
    Private m_vSourceArray(,) As Object
    Private m_iBranchID As Integer
    ''' <summary>
    ''' Variable declared to store AgentCnt of Agent linked to logged in User
    ''' </summary>
    Private m_nAgentCnt As integer
    ''' <summary>
    ''' Variable to Store Calling Component Name
    ''' </summary>
    Private m_sAppName As string
    'eck200600
    Private m_bInsurersAgents As Boolean
    Private m_bExcludeInsurersAgents As Boolean
    Private m_bOnlyUpdatableAccounts As Boolean
    Private m_bNotEditable As Boolean
    'eck PN6169
    Private m_iAccountCompanyId As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Instance of the form
    Private m_frmInterface As frmInterface

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    Private Const kDefaultvalue As String = "1"
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.1)

    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    ''' <summary>
    ''' Property to store Calling Component Name
    ''' </summary>
    ''' <returns>m_sAppName</returns>
    Public  Property AppName() As String
        Get
            Return m_sAppName
        End Get
        Set(ByVal Value As String)

            m_sAppName = Value

        End Set
    End Property

    ' CF 071298 - Added for NavV3 class
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property AccountID() As Integer
        Get

            Return m_lAccountID

        End Get
        Set(ByVal Value As Integer)

            m_lAccountID = Value

        End Set
    End Property

    Public Property LedgerID() As Integer
        Get

            Return m_iLedgerID

        End Get
        Set(ByVal Value As Integer)

            m_iLedgerID = Value

        End Set
    End Property

    Public Property LedgerTypeID() As Integer
        Get

            Return m_iLedgerTypeID

        End Get
        Set(ByVal Value As Integer)

            m_iLedgerTypeID = Value

        End Set
    End Property

    Public Property ShortCode() As String
        Get

            Return m_sShortCode

        End Get
        Set(ByVal Value As String)

            m_sShortCode = Value

        End Set
    End Property

    Public Property FullKey() As String
        Get

            Return m_sFullKey

        End Get
        Set(ByVal Value As String)

            m_sFullKey = Value

        End Set
    End Property

    Public ReadOnly Property AccountName() As String
        Get

            Return m_sAccountName

        End Get
    End Property

    Public Property AccountUIK() As Integer
        Get

            Return m_lAccountUIK

        End Get
        Set(ByVal Value As Integer)

            m_lAccountUIK = Value

        End Set
    End Property

    ' CF150199
    Public ReadOnly Property NominalAccountID() As Integer
        Get
            Return m_lNominalAccountID
        End Get
    End Property

    Public WriteOnly Property CompanyID() As Integer
        Set(ByVal Value As Integer)

            ' Set by user control (iACTAccountLookup)
            g_iCompanyID = Value

        End Set
    End Property

    'eck090500
    Public ReadOnly Property SourceArray() As Object
        Get

            ' Return the Source Array

            Return VB6.CopyArray(m_vSourceArray)

        End Get
    End Property

    'DD 28/01/2003: Added for compatibility with iPMBParty

    Public Property NotEditable() As Boolean
        Get
            Return m_bNotEditable
        End Get
        Set(ByVal Value As Boolean)
            m_bNotEditable = Value
        End Set
    End Property

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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle, sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the object parameters to default values

            'Tomo220199
            'This on need to be set too, else it'll do nowt on a second call
            m_lAccountID = 0

            m_iLedgerID = -1 ' Undefined
            m_iLedgerTypeID = -1
            m_sShortCode = ""
            m_sFullKey = ""
            m_bNotEditable = True 'PN14472

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Abort application
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                Return result
            End If

            ' Store the language ID etc from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iCompanyID = .SourceID
                g_sUsername.Value = .UserName
                g_sPassword.Value = .Password
                g_iUserID = .UserID
                g_iCurrencyID = .CurrencyID
                g_iLogLevel = .LogLevel
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bACTFindAccount.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bACTFindAccount.Business", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' Settings for Help files
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'developer guide no. 39 (No solution)
                'App.HelpFile = sHelpFile
            End If


            m_lReturn = g_oBusiness.GetUserAuthorities
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive User Authorities", Application.ProductName)
            End If

            Return result

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
            Me.disposedValue = True
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Default this value
            m_bAllowStoppedAccounts = False
            'eck200600
            m_bInsurersAgents = False

            m_bExcludeInsurersAgents = False

            'DD 15/07/2002: Added
            m_bOnlyUpdatableAccounts = False

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.ACTKeyNameLedgerID

                        m_iLedgerID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameLedgerTypeID

                        m_iLedgerTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameShortCode

                        m_sShortCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameFullKey

                        m_sFullKey = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClientUIK

                        m_lAccountUIK = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyAllowStoppedAccounts

                        m_bAllowStoppedAccounts = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck200600
                    Case PMNavKeyConst.ACTKeyOnlyInsurerAndAgentAccounts

                        m_bInsurersAgents = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameOnlyUpdatableAccounts

                        m_bOnlyUpdatableAccounts = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyDisallowInsurerAndAgentAccounts

                        m_bExcludeInsurersAgents = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck PN61619
                    Case PMNavKeyConst.ACTKeyNameBranchID

                        m_iBranchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAgentCnt
                        If not string.IsNullOrEmpty(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Then
                            m_nAgentCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMNavKeyConst.kACTKeyNameCallingComponent

                        m_sAppName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            'eck PN6169
            'ReDim vKeyArray(1, 7)
            ReDim vKeyArray(1, 8)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameLedgerID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_iLedgerID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameLedgerTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_iLedgerTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sShortCode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameFullKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sFullKey

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameAccountName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_sAccountName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lAccountUIK

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.ACTKeyNameNominalAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lNominalAccountID
            'PN6169

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.ACTKeyNameBranchID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_iAccountCompanyId

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)


            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Account Short Code"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sShortCode

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckAccountStatus
    '
    ' Description: Calls the business and finds out the status of an account.
    '
    ' ***************************************************************** '
    Private Function CheckAccountStatus(ByVal v_lAccountID As Integer, ByRef r_bAccountStopped As Boolean) As Integer

        Dim result As Integer = 0
        Dim iAccountStatus As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the account status for the account passed, and if its stopped or not

        m_lReturn = g_oBusiness.GetAccountStatus(v_lAccountID:=v_lAccountID, r_iAccountStatus:=iAccountStatus, r_bIsStopped:=r_bAccountStopped)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim bAccountStopped As Boolean
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.2)
        Dim sValue As String = ""
        Const kMethodName As String = "Start"
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.2)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'eck090500
            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck090500

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.2)

            ' Get System Option for Disable Wildcard Search
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bDisableWildcardSearchOption = (sValue = kDefaultvalue)

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = kDefaultvalue)
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.2)




            ' Set it to true
            bAccountStopped = True

            If m_lAccountID > 0 Then
                m_lReturn = CType(CheckAccountStatus(v_lAccountID:=m_lAccountID, r_bAccountStopped:=bAccountStopped), gPMConstants.PMEReturnCode)
            End If

            ' Check if the AccountID is greater than zero. If so,
            ' there is no need to proceed with the interface. We
            ' can therefore return straight back out.
            If m_lAccountID > 0 And Not bAccountStopped Then

                ' ID is greater than zero.
                m_lStatus = gPMConstants.PMEReturnCode.PMOK

            Else

                ' Check for valid ID from supplied Shortname
                '        If (Trim$(m_sShortCode$) <> "") Then
                '            m_lReturn& = GetID( _
                ''                lID:=m_lAccountID&, _
                ''                vShortName:=m_sShortCode$)
                '
                '            ' If valid AccountID, can return straight back out.
                '            If (m_lReturn& = PMTrue) Then
                '                m_lStatus& = PMOK
                '                Exit Function
                '            End If
                '        End If

                ' ID is not greater than zero.

                ' Starts the interface processing.
                m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetID (Standard Method)
    '
    ' Description: Gets the (last) AccountID matching
    '              the given parameters from the business object.
    '
    ' ***************************************************************** '
    Public Function GetID() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (m_iLedgerID = -1) And (m_iLedgerTypeID <> -1) Then
                ' Get the LedgerID from the LedgerTypeID
                m_lReturn = CType(GetLedgerID(iLedgerTypeID:=m_iLedgerTypeID, iLedgerID:=m_iLedgerID), gPMConstants.PMEReturnCode)
            End If

            ' Get the ID from the busines object.

            m_lReturn = g_oBusiness.GetID(lAccountID:=m_lAccountID, iCompanyID:=g_iCompanyID, vLedgerID:=m_iLedgerID, vFullKey:=m_sFullKey, vShortCode:=m_sShortCode)

            ' Check for errors (PMNotFound=OK)
            If (m_lReturn = gPMConstants.PMEReturnCode.PMFalse) Or (m_lReturn = gPMConstants.PMEReturnCode.PMError) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID")
            End If

            ' Return the value.

            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    'eck090500
    ' ***************************************************************** '
    ' Name: GetValidSources (Standard Method)
    '
    ' Description: Calls the appropriate methods to get the Sources
    '              which the the current user can access
    '
    ' ***************************************************************** '
    Private Function GetValidSources() As Integer

        Dim result As Integer = 0
        Dim r_bbranchAccess As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        'eck PN6169 If multi company only pick accounts for the current company
        r_bbranchAccess = False
        m_lReturn = CType(CheckBranchAccess(r_bbranchAccess:=r_bbranchAccess), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check branch access", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))
            Return result
        End If
        If r_bbranchAccess Then
            m_iBranchID = g_iSourceID
        End If


        'Valid source has been passed in via keys
        If m_iBranchID > 0 Then
            ReDim m_vSourceArray(3, 3)
            m_vSourceArray(1, 1) = m_iBranchID
            m_vSourceArray(2, 1) = ""
            m_vSourceArray(3, 1) = ""
            Return result
        End If
        'Call PMUser to get the Sources
        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_g_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_g_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        g_oPMUser = temp_g_oPMUser

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Display error stating the problem.

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If
        'KB PN 7526
        'But not if we are filtering for multi-branch
        'eck PN6169 Moved to beginning of the routine
        '    r_bbranchAccess = False
        '
        '    m_lReturn& = CheckBranchAccess(r_bbranchAccess:=r_bbranchAccess)


        'eck200600 If we are trying to find Insurers or Agents get all source
        '   If m_bInsurersAgents = True Then
        If (m_bInsurersAgents) And (Not r_bbranchAccess) Then

            m_lReturn = g_oPMUser.GetAllSources(r_vSourceArray:=m_vSourceArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If

        Else

            m_lReturn = g_oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=g_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

                Return result
            End If
        End If
        ' Remove instance of PMUser
        If Not (g_oPMUser Is Nothing) Then

            g_oPMUser.Dispose()
            g_oPMUser = Nothing
        End If

        Return result

    End Function


    'KB PN 7526
    'to determine if the user can see this account
    Private Function CheckBranchAccess(ByRef r_bbranchAccess As Boolean) As Integer

        Dim result As Integer = 0
        Dim v_vValue As String = ""

        'Consider product options:
        'SIROPTMultiTreeAccounting  16
        'SIROPTEnableBranchSelectAtLogon 37
        'If both are set to 1 then we limit accounts available to users

        result = gPMConstants.PMEReturnCode.PMTrue 'PN6169

        m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=v_vValue), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log an error and return false
            result = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:=" CheckBranchAccess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Decipher the return value
        Dim r_bCheckBranchAccess As Boolean = v_vValue = "1"

        If r_bCheckBranchAccess Then
            m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=v_vValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log an error and return false
                result = False
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:=" CheckBranchAccess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Decipher the return value
            r_bCheckBranchAccess = v_vValue = "1"
        End If

        r_bbranchAccess = r_bCheckBranchAccess

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New iACTFindAccount.frmInterface()

        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}

            .LedgerID = m_iLedgerID
            .LedgerTypeID = m_iLedgerTypeID
            .ShortCode = m_sShortCode
            .FullKey = m_sFullKey
            .AccountUIK = m_lAccountUIK

            .PMAuthorityLevel = m_lPMAuthorityLevel
            'eck090500
            'developer guide no. 24
            '.set_SourceArray(m_vSourceArray)
            .SourceArray = m_vSourceArray

            'DD 15/07/2002
            .OnlyUpdatableAccounts = m_bOnlyUpdatableAccounts

            'DD 28/01/2003: Added for iPMBParty compatiblity
            .NotEditable = m_bNotEditable

            .InsurersAgents = m_bInsurersAgents

            .ExcludeInsurersAgents = m_bExcludeInsurersAgents
            ' {* USER DEFINED CODE (End) *}

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.3)
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.10.1.3)
            .AgentCnt = m_nAgentCnt
            .AppName = m_sAppName
        End With

        ' Check if we have had an error so far.
        If m_frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface

            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}

            m_lAccountID = .AccountID
            m_iLedgerID = .LedgerID
            m_iLedgerTypeID = .LedgerTypeID
            m_sShortCode = .ShortCode
            m_sFullKey = .FullKey
            m_sAccountName = .AccountName
            m_lAccountUIK = .AccountUIK
            m_lNominalAccountID = .NominalAccountID

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_frmInterface.Close()
        m_frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_frmInterface.ErrorNumber <> 0 Then
                result = m_frmInterface.ErrorNumber
            End If
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

