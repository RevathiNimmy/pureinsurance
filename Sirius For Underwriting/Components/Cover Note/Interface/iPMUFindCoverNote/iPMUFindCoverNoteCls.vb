Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
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

    Private m_iTask As gPMConstants.PMEComponentAction
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
    Private m_vSourceArray As Object
    Private m_iBranchID As Integer
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

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.1)


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
        Dim sMessage, sTitle, sHelpFile As String
        Dim m_lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Const kMethodName As String = "Initialise"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                gPMFunctions.RaiseError(kMethodName, "Failed to initialise the object manager", gPMConstants.PMELogLevel.PMLogError)
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

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
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
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
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

        Const kMethodName As String = "SetKeys"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    '            Case ACTKeyNameLedgerID
                    '               m_iLedgerID% = CInt(vKeyArray(PMKeyValue, lRow&))

                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
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
        Dim lRow As Integer

        Const kMethodName As String = "GetKeys"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
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
        Const kMethodName As String = "GetSummary"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)


            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Account Short Code"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sShortCode

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
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
        Const kMethodName As String = "Start"
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.2)
        Dim sValue As String = ""
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.2)
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.2)

            ' Get System Option for Disable Wildcard Search
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bDisableWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = "1")
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.2)


            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start.", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to load interface", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to display interface", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to unload interface", gPMConstants.PMELogLevel.PMLogError)
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
        Const kMethodName As String = "LoadInterface"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_frmInterface = New iPMUFindCoverNote.frmInterface()

        ' Assign the parameters to the interface properties.
        With m_frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Task = m_iTask
            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.3)
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.6.1.3)
        End With

        ' Load the instance of the interface into memory.

        'Developer Guide No. 68

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
        Const kMethodName As String = "UnLoadInterface"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_frmInterface

            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}

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
        Const kMethodName As String = "ShowInterface"


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

        Const kMethodName As String = "Class_Initialize"
        Try




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally



        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
