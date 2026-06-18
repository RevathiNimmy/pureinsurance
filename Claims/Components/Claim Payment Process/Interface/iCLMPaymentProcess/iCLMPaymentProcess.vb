Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 17/03/08
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    Private m_oBusiness As Object

    Private m_lClaimId As Integer
    Private m_lOriginalClaimID As Integer
    Private m_sClaimRef As Integer
    Private m_sClaimReference As String = ""

    Private m_lCurrencyId As Integer

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_sUnderwritingOrAgency As String = ""

    ' Indicate that we are running in balance and close mode
    Private m_bBalanceAndCloseClaim As Boolean

    Private m_lSourceId As Integer

    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed
    Private m_bNavCompleted As Boolean
    Private m_bProcessComplete As Boolean

    Private m_lWorkflowId As Integer
    Private m_bClaim_Payment_Process As Boolean
    Private m_iClaimPaymentValid As Integer
    Private m_bUserAuthRunClaimPayment As Boolean
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

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

    ' ***************************************************************** '
    ' Region: m_oNavStart Events
    ' ***************************************************************** '
    Private Sub m_oNavStart_NavigatorClose() Handles m_oNavStart.NavigatorClose
        m_bNavCompleted = True
    End Sub

    Private Sub m_oNavStart_SetProcessStatus(ByVal v_bProcessComplete As Boolean) Handles m_oNavStart.SetProcessStatus
        m_bProcessComplete = v_bProcessComplete
    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise the Object Manager.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.RaiseError(kMethodName, "Failed to get Underwriting/Agency flag", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()

                End If
                m_oBusiness = Nothing
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
        Dim iCount As Integer
        Dim iParamCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the correct key array item.

                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameClaimID
                        m_lClaimId = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameRealClaimID
                        m_lOriginalClaimID = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameOperateMode
                        m_iTask = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCurrencyID
                        m_lCurrencyId = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameBalanceAndCloseClaim
                        m_bBalanceAndCloseClaim = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimNumber
                        m_sClaimReference = gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId
                        m_lWorkflowId = CInt(gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))

                    Case "display_claim_payment_process"
                        m_bClaim_Payment_Process = CBool(gPMFunctions.ToSafeString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClaimPaymentValid
                        m_iClaimPaymentValid = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameUserAuthRunClaimPayment
                        m_bUserAuthRunClaimPayment = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                End Select

            Next lRow


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

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
        Const kMethodName As String = "GetKeys"
        Dim lRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            ReDim vKeyArray(1, 4)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameRealClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lOriginalClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = "UNDERWRITING"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lCurrencyId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameSourceId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lSourceId

            ' {* USER DEFINED CODE (End) *}


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Const kMethodName As String = "GetSummary"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

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


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vDataArray(,) As Object
        Dim lWorkflowId As Integer
        Dim bHasClaimPaymentAuthority As Boolean
        Dim vKeyArray(1, 1) As Object
        Dim sClaimStatus As String = ""

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bBalanceAndCloseClaim Or Not m_bClaim_Payment_Process Then
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Return result
            End If
            m_lReturn = GetUserAuthorities(r_bHasClaimPaymentsAuthority:=bHasClaimPaymentAuthority)

            'PN-71999 Sushil Kumar
            m_lReturn = GetClaimStatus(v_lClaimId:=m_lClaimId, r_sStatus:=sClaimStatus)

            If m_iClaimPaymentValid = 1 And bHasClaimPaymentAuthority And m_bUserAuthRunClaimPayment And sClaimStatus <> "CLOSED" Then
                If MessageBox.Show("Do you wish to proceed to claim payments?", "Claim Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    Return result
                End If
            Else
                Return result
            End If

            ' determine event type from transaction type
            Select Case m_sTransactionType
                Case "C_CO"
                    lWorkflowId = gPMConstants.PMWorkflowOpenClaim
                Case "C_CR"
                    lWorkflowId = gPMConstants.PMWorkflowMaintainClaim
            End Select

            m_oNavStart = New iPMNavStart.Interface_Renamed()
            'Developer guide no. 9
            lReturn = m_oNavStart.Initialise()
            m_oNavStart.NavXMLFile = "PAYCLM.XML"

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimWorkflowId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lWorkflowId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimReference

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_sClaimReference

            ' set the navigators processes keys
            lReturn = m_oNavStart.SetKeys(vKeyArray:=vKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMNavStart.Interface.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' default the navigation completed actions to false
            m_bProcessComplete = False
            m_bNavCompleted = False

            ' start the specified navigator process
            m_oNavStart.CallingAppName = ACApp
            m_oNavStart.IsChildNavigatorON = True
            lReturn = m_oNavStart.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUNavStart.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' wait while the navigator process is completed
            Do
                Application.DoEvents()
            Loop While Not m_bNavCompleted



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            If Not (m_oNavStart Is Nothing) Then
                ' terminate this instance of the navigator process
                m_oNavStart.Dispose()
                ' clean up the object instances
                m_oNavStart = Nothing
            End If


        End Try
        Return result
    End Function


    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        Const kMethodName As String = "Class_Initialize"
        ' Class Initialise Event.
        Try


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Function GetUserAuthorities(ByRef r_bHasClaimPaymentsAuthority As Boolean, Optional ByRef r_cPaymentsAmount As Decimal = 0) As Integer
        Dim result As Integer = 0
        Dim bACTUserAuthorities As Object

        Const kMethodName As String = "GetUserAuthorities"
        Dim vResults As Object
        Dim crConvertedCurrency As Decimal
        Dim iPaymentsCurrencyID As Integer

        Dim oUserAuthorities As bACTUserAuthorities.Business


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oUserAuthorities As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oUserAuthorities = temp_oUserAuthorities
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("kMethodName", "Failed to create instance of UserAuthorities")
        End If


        m_lReturn = oUserAuthorities.GetDetails(vUserId:=g_oObjectManager.UserID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Unable to get User Authorities Details")
        End If


        m_lReturn = oUserAuthorities.GetNext(vHasClaimPaymentsAuthority:=r_bHasClaimPaymentsAuthority, vClaimPaymentsAmount:=r_cPaymentsAmount, vPaymentsCurrencyID:=iPaymentsCurrencyID)

        Return result
    End Function


    'PN-71999 - Sushil Kumar
    Private Function GetClaimStatus(ByVal v_lClaimId As Integer, ByRef r_sStatus As String) As Integer
        Dim result As Integer = 0
        Dim bCLMChangeClaimStatus As Object
        Const kMethodName As String = "GetClaimStatus"

        Dim oChangeClaimStatus As bCLMChangeClaimStatus.Business


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oChangeClaimStatus As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oChangeClaimStatus, "bCLMChangeClaimStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oChangeClaimStatus = temp_oChangeClaimStatus
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("kMethodName", "Failed to create instance of Change Claim Status")
        End If


        m_lReturn = oChangeClaimStatus.GetClaimStatus(v_lClaimId:=v_lClaimId, r_sStatus:=r_sStatus)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Unable to get Change Claim Status")
        End If

        Return result
    End Function
End Class
