Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

'Developer Guide No.: 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/09/00
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
    Private Const ACAuthoriseTask As String = "Authorise Claim Payments"

    Private Const ACClaimPaymentsType As Integer = 1
    Private Const ACPaymentsType As Integer = 2
    Private Const ACTClaimAdminTaskGroupID As Integer = 10
    Private Const ACTBrokingTaskGroupID As Integer = 21


    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_lPMAuthorityLevel As Integer

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer


    Private m_oBusiness As bCLMChangeClaimStatus.Business

    Private m_lClaimId As Integer
    Private m_lOriginalClaimID As Integer

    Private m_cAmount As Decimal
    Private m_lCurrencyId As Integer

    ' Stores the exit status of the interface.
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_cThisPaymentAmount As Decimal
    'PN: 45635
    Private m_lThisPaymentCurrencyId As Integer
    Private m_sUnderwritingOrAgency As String = ""

    ' Indicate that we are running in balance and close mode
    Private m_bBalanceAndCloseClaim As Boolean

    Private m_lDocumentId As Integer
    Private m_lMediaTypeId As Integer
    Private m_lAccountId As Integer
    Private m_lClaimPaymentId As Integer
    Private m_crTotalClaimPaymentAmount As Decimal
    Private m_lSourceId As Integer
    Private m_bNoTransactions As Boolean
    Private m_bProductOptRecommender As Boolean
    Private m_bDisplayExternalClaimHandling As Boolean
    Private m_bDisplayCashPaymentProcess As Boolean
    Private m_bDisplayDescriptionforChange As Boolean
    Private m_iClaimWorkFlowID As Integer
    Private m_lPartycnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_bIs_Multiple_Claims_Payments As Boolean
    Private m_bIsSuppressReserve As Boolean
    Private m_cOriginalThisPaymentAmount As Decimal
    Private m_lOriginalThisPaymentCurrencyId As Integer
    Private m_bIsReserveUpdatednTaskCompleted As Boolean
    Private m_nAuthorisation_Threshold As Decimal


    Private ReadOnly Property IsMultipleClaimsPayments() As Boolean
        Get
            Return m_bIs_Multiple_Claims_Payments
        End Get
    End Property



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
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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


            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            If g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMChangeClaimStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oBusiness = temp_m_oBusiness


                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse

            Else
                m_oBusiness = temp_m_oBusiness
            End If

            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Underwriting/Agency flag", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

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
                    m_oBusiness = Nothing
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
                        m_iTask = CType(gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), gPMConstants.PMEComponentAction)

                        'PSL  09/07/2003 5210 5212 these are passed in on new claim
                        'and the code to get them does not work becuase it is
                        'in the work claim table, not the claim table
                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt
                        g_lInsuranceFoldercnt = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                        g_lInsuranceFilecnt = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimPayment
                        m_cAmount = gPMFunctions.ToSafeCurrency(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.ACTKeyNameCurrencyID
                        m_lCurrencyId = gPMFunctions.ToSafeLong(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameBalanceAndCloseClaim
                        m_bBalanceAndCloseClaim = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case "no_transactions"
                        m_bNoTransactions = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameDisplayExternalClaimHandling
                        m_bDisplayExternalClaimHandling = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameDisplayCashPaymentProcess
                        m_bDisplayCashPaymentProcess = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameDisplayDescriptionForChange
                        m_bDisplayDescriptionforChange = gPMFunctions.ToSafeBoolean(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId
                        m_iClaimWorkFlowID = gPMFunctions.ToSafeInteger(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "ReserveUpdatednTaskCompleted"
                        m_bIsReserveUpdatednTaskCompleted = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                End Select
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
        Dim vDocumentIds(0) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' only set up the array if we have a valid document id
            If m_lDocumentId <> 0 Then

                vDocumentIds(0) = m_lDocumentId
            Else
                Erase vDocumentIds
            End If

            ' {* USER DEFINED CODE (Begin) *}
            ReDim vKeyArray(1, 11)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId

            ' If the original claim is populated hurrah, otherwise use the claimid which
            ' under most circumstances should be the id of the new claim record anyway.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameRealClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lOriginalClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = "UNDERWRITING"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lCurrencyId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = vDocumentIds


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameMediaTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lMediaTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lAccountId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameScreenType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = "CLP"


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameClaimPaymentId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_lClaimPaymentId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.ACTKeyNameTotalPremium

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_crTotalClaimPaymentAmount


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameSourceId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_lSourceId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "SilentMultiCurrencyScreen"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = True
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try


            ' {* USER DEFINED CODE (Begin) *}

            '    ReDim vSummaryArray(2, 0)

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMNavSummHeading, 0) = "insurance_ref"
            '    vSummaryArray(PMNavSummValue, 0) = m_sNewPolicyNumber

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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


            m_lReturn = m_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
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

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    '''  Claims Versioning Changes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessInterface() As Integer
        Dim result As Integer = 0
        Dim iPMBGetChangeReason As Object

        Dim oGetChangeReason As iPMBGetChangeReason.Interface_Renamed
        Dim sClaimOldPolicyRef As String = ""
        Const kMethodName As String = "ProcessInterface"
        Const kSpoolSilentMode As Integer = 4
        Const kClaimIsFullyProcessed As Integer = 0

        Dim iReturn As gPMConstants.PMEReturnCode
        Dim lEventType As Integer
        Dim sDescription As String = ""
        Dim lClaimDateChanged As gPMConstants.PMEReturnCode
        Dim lAddTask As Integer
        Dim vClaimDetails As Object
        Dim bUserAuthorised As Boolean
        Dim lPMWrkTaskInstanceCnt As Integer
        Dim bSavedStats, bCheckPaymentAuthorisation, bClaimIsDirtyFlagUpdated As Boolean
        Dim sClaimVersionDescription, sValue As String
        Dim lOriginalSuppressReserves As Integer
        Dim vDataArray(,) As Object
        Dim vResults(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Cursor.Current = Cursors.WaitCursor

            result = gPMConstants.PMEReturnCode.PMTrue
            lClaimDateChanged = gPMConstants.PMEReturnCode.PMFalse
            lAddTask = gPMConstants.PMEReturnCode.PMFalse
            bSavedStats = False

            iReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007,
                                           v_vBranch:=g_oObjectManager.SourceID,
                                       r_vUnderwriting:=sValue)

            If (iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to get the value for SIROPTEnableRI2007.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMReverse Then
                bSavedStats = True
            End If

            If m_iTask = ACModeAuthorise Then
                bSavedStats = True
            ElseIf NullToString(sValue) = "1" And m_sTransactionType = "C_CP" Then
                iReturn = m_oBusiness.GetClaimTransactionType(v_lClaimId:=m_lClaimId, r_vResults:=vResults)
                If IsArray(vResults) Then
                    'Check if transaction type has changed
                    If vResults(0, 0) = "C_CR" Then
                        m_sTransactionType = "C_CR"
                        m_lReturn = m_oBusiness.SetProcessModes(vTransactionType:=m_sTransactionType)
                    End If
                End If
            End If

            If m_bNoTransactions Then
                'switch on reserve suppression

                iReturn = m_oBusiness.UpdateClaimSuppression(lClaimID:=m_lClaimId, lSuppressReserves:=1, lSuppressPayments:=-1, lSuppressRecoveries:=-1, lOriginalSuppressReserves:=lOriginalSuppressReserves)
                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.UpdateClaimSuppression Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


            ' determine event type from transaction type
            Select Case m_sTransactionType
                Case "C_CO"
                    lEventType = PMBConst.PMBEventNewClaim
                Case Else
                    lEventType = PMBConst.PMBEventClaChange
            End Select



            iReturn = CType(GetProductDetails(r_bIsRecommender:=m_bProductOptRecommender, r_bIs_Multiple_Claims_Payments:=m_bIs_Multiple_Claims_Payments, r_bIsSuppressReserve:=m_bIsSuppressReserve, r_nAuthorisation_Threshold:=m_nAuthorisation_Threshold), gPMConstants.PMEReturnCode) 'PN-69520
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bIsSuppressReserve Then
                MessageBox.Show("No reserves are recorded in accounts for this claim ", "Suppress Reserve", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' Do not raise event in case of authorisation mode
            If m_iTask <> gPMConstants.PMEComponentAction.PMReverse Then

                If Not m_bBalanceAndCloseClaim Then
                    ' don't ask this question for New Claims
                    If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                        ' sDescription = Trim(InputBox("Enter description for change", "Change Claim Status"))
                        Dim temp_oGetChangeReason As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oGetChangeReason, sClassName:="iPMBGetChangeReason.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                        oGetChangeReason = temp_oGetChangeReason

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "oObjectManager.GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'get claim details

                        iReturn = m_oBusiness.GetClaimDetails(v_lClaimId:=m_lClaimId, r_vResultArray:=vClaimDetails)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.GetClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        g_lInsuranceFilecnt = CInt(vClaimDetails(1, 0))

                        m_lReturn = m_oBusiness.GetInsFileCntProductId(v_lInsuranceFilecnt:=g_lInsuranceFilecnt, r_vResultArray:=vDataArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vDataArray) Then
                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the ProductId", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            'Exit Function
                        End If

                        If Not (m_iClaimWorkFlowID >= 1 And m_iClaimWorkFlowID <= 3 And Not m_bDisplayDescriptionforChange) Then
                            If Information.IsArray(vDataArray) Then


                                oGetChangeReason.ProductID = vDataArray(0, 0)
                            End If
                            oGetChangeReason.TransactionType = "CLAIM"
                            oGetChangeReason.FormCaption = "Change Claim"
                            oGetChangeReason.Start()
                            m_lStatus = oGetChangeReason.Status
                            sDescription = oGetChangeReason.ReasonDescription
                            oGetChangeReason.Dispose()
                            oGetChangeReason = Nothing
                        End If
                        If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then Exit Function
                    End If

                    Select Case m_sTransactionType
                        Case "C_CO"
                            If m_bNoTransactions Then
                                sClaimVersionDescription = "Opened Claim via task Open Claim No Transaction"
                            Else
                                sClaimVersionDescription = "Opened Claim"
                            End If
                        Case "C_CR"
                            sClaimVersionDescription = "Maintained Claim"
                        Case "C_SA"
                            sClaimVersionDescription = "Salvage Recovery"
                        Case "C_RV"
                            sClaimVersionDescription = "Third Party Recovery"
                        Case "C_CP"
                            sClaimVersionDescription = "Payment Of Claim"
                        Case Else
                            sClaimVersionDescription = "Unknown"
                    End Select
                    'In case of pay claim description will be decided later on depending upon authorization requirement
                    If m_sTransactionType <> "C_CP" And sDescription <> "" Then
                        sClaimVersionDescription = sClaimVersionDescription & ", Comments - " & sDescription
                    End If
                Else
                    sClaimVersionDescription = "Claim balanced and closed automatically"
                End If
            End If

            'If were not cancelling
            If m_iTask <> gPMConstants.PMEComponentAction.PMDelete Then

                ' if we are balancing and closing this claim update status to closed
                If m_bBalanceAndCloseClaim Then
                    ' set the claim status to closed

                    iReturn = m_oBusiness.UpdateClaimStatus(v_lClaimId:=m_lClaimId, v_lClaimStatusID:=3)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.UpdateClaimStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                ' if this is maintain claim
                If m_sTransactionType = "C_CR" Then
                    ' if claim status is set to closed then check to see if reserve/recovery are zero
                    ' otherwise warn user and reset back to live
                    iReturn = CType(ProcessClaimStatus(m_lClaimId), gPMConstants.PMEReturnCode)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessClaimStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                ' if this is open claim
                If m_sTransactionType = "C_CO" Then
                    ' then create the documaster folder entry for the new claim

                    iReturn = m_oBusiness.CreateDMEClaimFolder(v_lClaimId:=m_lClaimId)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' log this issue and carry on processing

                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.CreateDMEClaimFolder Failed", gPMConstants.PMELogLevel.PMLogError)

                    End If
                End If

                ' Do not raise event in case of authorisation mode
                If m_iTask <> gPMConstants.PMEComponentAction.PMReverse Then
                    ' tidy up claims - complete work that was previously done when
                    ' copying the data back from the work tables
                    ' such as moving this payment values to payment
                    '                this reserve values to reserve and so on

                    iReturn = m_oBusiness.FinaliseClaimDetails(v_lClaimId:=m_lClaimId, v_sClaimVersionDescription:=sClaimVersionDescription)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "FinaliseClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                ' if we are in claim payment mode
                If m_sTransactionType = "C_CP" Then

                    ' check if the user is or needs authorisation to make a payment
                    ' determine whether authorisation scripts need to be run
                    iReturn = CType(UseAuthorisedScriptsForClaimPayments(bCheckPaymentAuthorisation), gPMConstants.PMEReturnCode)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' log this issue and carry on processing

                        gPMFunctions.RaiseError(kMethodName, "UseAuthorisedScriptsForClaimPayments Failed", gPMConstants.PMELogLevel.PMLogError)

                    End If

                    ' get the total payment amount made by this user on this claim
                    ' this should include any historical payments made by this user
                    ' on this claim not just those made in this session and the
                    ' total payment amount in this session as this could be zero

                    iReturn = m_oBusiness.GetPaymentAmount(v_lClaimId:=m_lClaimId, r_crPaymentAmount:=m_cAmount, r_crThisPaymentAmount:=m_cThisPaymentAmount, r_lCurrencyId:=m_lThisPaymentCurrencyId, r_lOriginalPaymentAmount:=m_cOriginalThisPaymentAmount, r_lOriginalCurrencyId:=m_lOriginalThisPaymentCurrencyId)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.GetPaymentAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    '*******************************************
                    ' get payment details for the latest claim payment
                    '*******************************************
                    ' if this isnt authorise mode then
                    ' get the details for creating the cashlistitem
                    ' from the payment
                    If m_iTask <> gPMConstants.PMEComponentAction.PMReverse And m_cThisPaymentAmount <> 0 Then
                        iReturn = CType(GetClaimPaymentAccountsDetails(v_lClaimId:=m_lClaimId), gPMConstants.PMEReturnCode)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentAccountsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    If Not bCheckPaymentAuthorisation Then
                        bUserAuthorised = True
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMReverse Then
                        bUserAuthorised = True
                    End If

                    ' if no payment has been made in this session then
                    ' the user is always authorised.....
                    If m_cThisPaymentAmount = 0 Or bUserAuthorised Then
                        bUserAuthorised = True
                    Else
                        ' check if the users payment authority has been exceeded
                        iReturn = CType(CheckUserPaymentAuthority(r_bUserAuthorised:=bUserAuthorised), gPMConstants.PMEReturnCode)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bUserAuthorised = False
                            ' gPMFunctions.RaiseError(kMethodName, "CheckUserPaymentAuthority Failed", gPMConstants.PMELogLevel.PMLogError)

                        End If
                    End If

                Else
                    ' all other transaction types are always authorised..
                    ' only payments may need authorisation and they
                    ' can only be made in pay claim mode.
                    bUserAuthorised = True
                End If

                ' raise transactions only if user is authorised to do so
                If bUserAuthorised Or m_iTask = gPMConstants.PMEComponentAction.PMReverse Then

                    ' Update insurance_file_system table before doing transactions.

                    iReturn = m_oBusiness.UpdateInsuranceFileSystem(m_lClaimId)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.UpdateInsuranceFileSystem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' Raise Transactions

                    iReturn = m_oBusiness.RaiseTransactions(v_lClaimId:=m_lClaimId, v_bSavedStats:=bSavedStats, r_lDocumentId:=m_lDocumentId)

                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.RaiseTransactions Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If m_iTask <> gPMConstants.PMEComponentAction.PMReverse Then

                        ' update the claim is dirty indicator to show all processing has been completed.

                        iReturn = m_oBusiness.UpdateClaimIsDirty(v_lClaimId:=m_lClaimId, v_lIsDirty:=kClaimIsFullyProcessed)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.UpdateClaimIsDirty Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        bClaimIsDirtyFlagUpdated = True

                    End If

                    ' reset the referred payment

                    iReturn = m_oBusiness.SetPaymentReferred(m_lClaimId, ACModeProcessed)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetPaymentReferred Failed", gPMConstants.PMELogLevel.PMLogError)
                    Else
                        If m_iTask = gPMConstants.PMEComponentAction.PMReverse Then
                            sClaimVersionDescription = "Payment of Claim - Payment Authorised"
                        Else
                            If sDescription.Trim().Length > 0 And m_sTransactionType = "C_CP" Then
                                sClaimVersionDescription = sClaimVersionDescription & ", Comments - " & sDescription
                            End If
                        End If
                        'Since transactions has been raised this is safest place to update claim description

                        iReturn = m_oBusiness.UpdateClaimDesc(v_lClaimId:=m_lClaimId, v_sClaimVersionDescription:=sClaimVersionDescription)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "UpdateClaimDesc Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                Else
                    ' the user isnt authorised to make this payment

                    ' display message for authorisation failure
                    MessageBox.Show("This payment could not be authorised. It is now being forwarded to the " &
                                    "Claim Payment Authorisation Group." & vbCrLf & "Failed to load rule." & " Check Maintain Autority Rules", "Authorisation Failure", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    If sClaimVersionDescription = "Payment Of Claim" Then
                        sClaimVersionDescription = sClaimVersionDescription & " - Awaiting Claim Payment Authority"
                        If sDescription.Trim() <> "" Then
                            sClaimVersionDescription = sClaimVersionDescription & ", Comments - " & sDescription
                        End If
                    End If


                    iReturn = m_oBusiness.UpdateClaimDesc(v_lClaimId:=m_lClaimId, v_sClaimVersionDescription:=sClaimVersionDescription)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdateClaimDesc Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' update the payment and mark it as referred

                    iReturn = m_oBusiness.SetPaymentReferred(m_lClaimId, ACModeReferred)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetPaymentReferred Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If m_bProductOptRecommender Then

                        If (m_nAuthorisation_Threshold = 0 OrElse m_nAuthorisation_Threshold < m_cThisPaymentAmount) Then

                            iReturn = m_oBusiness.SetPaymentRecommendation(m_lClaimId, ACModeRecommend)
                            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "SetPaymentRecommandation Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If

                    End If

                    ' create work manager task for claims-supervisor group
                    iReturn = CType(CreateReferralTask(r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt), gPMConstants.PMEReturnCode)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CreateReferralTask Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If
                If m_sTransactionType = "C_CR" Then

                    ' determine if the claims loss dates have changed

                    iReturn = m_oBusiness.IsClaimDateChanged(v_lClaimId:=m_lClaimId, r_lChanged:=lClaimDateChanged)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.IsClaimDateChanged Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'create event if we changed claim date
                    If lClaimDateChanged = gPMConstants.PMEReturnCode.PMTrue Then
                        sDescription = "Changed Claim Date"

                        iReturn = m_oBusiness.CreateEvent(v_lClaimId:=m_lClaimId, v_lEventType:=lEventType, v_sDescription:=sDescription)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateEvent Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                End If

                ' determine if an external handler task need to be added

                iReturn = m_oBusiness.IsAddTask(v_lClaimId:=m_lClaimId, v_sTransactionType:=m_sTransactionType, r_lAddTask:=lAddTask)
                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "IsAddTask Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'add external handler task
                'WR05 - External Claim Handling msgbox will be shown only if Product option DisplayExternalClaimHandling is set
                If Not (m_iClaimWorkFlowID >= 1 And m_iClaimWorkFlowID <= 3 And Not m_bDisplayExternalClaimHandling) Then
                    If lAddTask Then
                        If MessageBox.Show("Is this claim to be handled externally?", "Change Claim Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            'get claim details

                            iReturn = m_oBusiness.GetClaimDetails(v_lClaimId:=m_lClaimId, r_vResultArray:=vClaimDetails)
                            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.GetClaimDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If


                            iReturn = CType(AddExternalHandlerTask(v_sClaimNumber:=CStr(vClaimDetails(0, 0))), gPMConstants.PMEReturnCode)
                            If iReturn = gPMConstants.PMEReturnCode.PMFail Then
                                MessageBox.Show("Claim Task Group and Claim User Group options are not setup for this product.", "Change Claim Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return result
                            End If
                            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "AddExternalHandlerTask Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(vClaimDetails(1, 0))
                            m_lPartycnt = gPMFunctions.ToSafeLong(vClaimDetails(6, 0))


                            iReturn = CType(GenerateDocument(v_iDocType:=gSIRLibrary.SIRDocTypeClaims, v_sTransactionType:=gSIRLibrary.SIRTransactionCodeClaimExternalHandler, v_iMode:=kSpoolSilentMode, v_lInsuranceFilecnt:=CInt(vClaimDetails(1, 0)), v_lPartyCnt:=CInt(vClaimDetails(6, 0)), v_lClaimId:=m_lClaimId, v_sSpoolDesc:="External Handler Notification"), gPMConstants.PMEReturnCode)
                            If iReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("External Handler Notification spooled", "Information " &
                                                "Checklist", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show("Failed to spool External Handler Notification", "Information Checklist", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If

                        End If
                    End If
                End If
                ' if we havent already updated the claims "is dirty" indicator
                If m_iTask <> gPMConstants.PMEComponentAction.PMReverse And Not bClaimIsDirtyFlagUpdated Then

                    ' update the claim "is dirty" indicator to show all processing has been successfully completed.

                    iReturn = m_oBusiness.UpdateClaimIsDirty(v_lClaimId:=m_lClaimId, v_lIsDirty:=kClaimIsFullyProcessed)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.UpdateClaimIsDirty Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    bClaimIsDirtyFlagUpdated = True
                End If

            Else
                ' remove the claim

                iReturn = m_oBusiness.DeleteClaim(v_lClaimId:=m_lClaimId)
                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bCLMChangeClaimStatus.DeleteClaim Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            Dim sSkipCashlistProcessForNegativePayment As String
            'If the Transaction type is Claim Payment
            If m_sTransactionType = "C_CP" Then
                ' if we didnt get a valid document returned
                ' or the total payment amount is zero
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5115, r_sOptionValue:=sSkipCashlistProcessForNegativePayment, v_iSourceID:=g_iSourceID)
                If Not (Not m_bDisplayCashPaymentProcess And m_iClaimWorkFlowID >= 1 And m_iClaimWorkFlowID <= 3) Then
                    If m_lDocumentId = 0 Or (m_cThisPaymentAmount < 0 AndAlso sSkipCashlistProcessForNegativePayment = "1") OrElse m_crTotalClaimPaymentAmount = 0 Then
                        m_lStatus = gPMConstants.PMEReturnCode.PMNavAction1
                    End If
                End If
            ElseIf m_sTransactionType = "C_CR" And m_iClaimWorkFlowID = 3 Then
                'Abort the process as further steps in the roadmap is specific to claim payment
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            End If


            If m_iTask <> gPMConstants.PMEComponentAction.PMReverse Then

                m_lReturn = ClaimPolicyChanged(v_lClaimId:=m_lClaimId, r_sClaimOldPolicyRef:=sClaimOldPolicyRef)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If sClaimOldPolicyRef.Trim().Length > 0 Then

                        iReturn = m_oBusiness.UpdateClaimEvents(v_lClaimId:=m_lClaimId)
                        If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Update Claim Events Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        sClaimVersionDescription = "Claim transferred from Policy Number " & sClaimOldPolicyRef
                    End If
                End If


                iReturn = m_oBusiness.CreateEvent(v_lClaimId:=m_lClaimId, v_lEventType:=lEventType, v_sDescription:=sClaimVersionDescription)

                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateEvent Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = RepostClaimTransactions()

            If bClaimIsDirtyFlagUpdated And m_bIsReserveUpdatednTaskCompleted Then
                'Send information to previous client that claim has been completed.
                m_lReturn = AddInfoOnlyCompletedTaskUpdate()
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = PMEReturnCode.PMFalse
        Finally
            If m_bNoTransactions Then
                'switch off reserve suppression

                iReturn = m_oBusiness.UpdateClaimSuppression(lClaimID:=m_lClaimId, lSuppressReserves:=lOriginalSuppressReserves, lSuppressPayments:=-1, lSuppressRecoveries:=-1)
                If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
                End If
            End If

            ' unlocking the claims is the last action that should take place
            If m_sTransactionType <> "C_CO" Then
                If m_iTask <> gPMConstants.PMEComponentAction.PMReverse Then

                    ' get the original (base) claim id before unlocking

                    iReturn = m_oBusiness.GetOriginalClaimId(v_lClaimId:=m_lClaimId, r_lOriginalClaimId:=m_lOriginalClaimID)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
                    End If

                    ' unlock the claim
                    iReturn = CType(UnlockClaim(v_lOriginalClaimID:=m_lOriginalClaimID), gPMConstants.PMEReturnCode)
                    If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
                    End If
                End If
            End If

            Cursor.Current = Cursors.Default
        End Try
        Return result

    End Function

    Private Function AddInfoOnlyCompletedTaskUpdate() As Integer
        Dim result As Integer = 0
        Dim lTaskInstanceCnt, lPreviousUserID As Integer
        Dim sUserName As String = String.Empty
        Dim dReserveEntered As Decimal
        Dim sTaskCode As String = String.Empty
        Dim sTaskDesc As String = String.Empty
        Dim oAuthorisePayments As bCLMAuthorisePayments.Business

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If oAuthorisePayments Is Nothing Then
                oAuthorisePayments = New bCLMAuthorisePayments.Business()
            End If

            m_lReturn = oAuthorisePayments.Initialise(g_oObjectManager.UserName, g_oObjectManager.Password, g_oObjectManager.UserID, g_oObjectManager.SourceID, g_oObjectManager.LanguageID, g_oObjectManager.CurrencyID, g_oObjectManager.LogLevel, m_sCallingAppName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = "C_CO" Then
                sTaskCode = "OPENCLM"
            ElseIf m_sTransactionType = "C_CR" Then
                sTaskCode = "MAINCLM"
            End If

            m_lReturn = m_oBusiness.GetUserIDForTaskCompleteIntimation(m_lClaimId, lPreviousUserID, sUserName, dReserveEntered)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            sTaskDesc = "Information: Claim Reserve Limit has been successfully updated for this User '" & sUserName & "' and Reserve = " & dReserveEntered & "."

            m_lReturn = oAuthorisePayments.AddTaskToWorkManager(r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_sCustomer:="SYSTEM", v_sDescription:=sTaskDesc, v_dtTaskDueDate:=DateTime.Now,
                                                                  v_sTaskCode:=sTaskCode, v_sTaskGroupCode:="CLAIMSUPER", v_sUserGroupCode:=String.Empty, v_iUserID:=lPreviousUserID,
                                                                  v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSComplete)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create InfoOnlyCompletedTaskUpdate Work Manager Task.", vApp:=ACApp, vClass:=ACClass, vMethod:="AddInfoOnlyCompletedTaskUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        Finally
            If Not (oAuthorisePayments Is Nothing) Then
                oAuthorisePayments.Dispose()
                oAuthorisePayments = Nothing
            End If
        End Try
        Return result
    End Function
    '**********************************************
    'Method         :   RepostClaimTransactions
    '
    'Created By     :   GA
    '**********************************************

    Private Function RepostClaimTransactions() As Integer

        Dim result As Integer = 0

        Dim vOptionValue As Object
        Dim bIsRI2007Enabled, bClaimReversalRequired As Boolean
        Dim lVersionId As Integer
        Dim sClaimVersionDescription As String = ""
        Const kMethodName As String = "RepostClaimTransactions"


        result = gPMConstants.PMEReturnCode.PMTrue


        'developer guide no.98
        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get the value for SIROPTEnableRI2007.", gPMConstants.PMELogLevel.PMLogError)
        End If


        If gPMFunctions.NullToString(vOptionValue) = "1" Then
            bIsRI2007Enabled = True
        End If



        m_lReturn = m_oBusiness.IsClaimReversalRequired(v_lClaimId:=m_lClaimId, r_bClaimReversalRequired:=bClaimReversalRequired)

        If bClaimReversalRequired And m_sTransactionType = "C_CR" And bIsRI2007Enabled Then


            m_lReturn = m_oBusiness.CreateReverseTransactions(lClaim_id:=m_lClaimId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="CreateReverseTransactions", r_lFunctionReturn:=result)
            End If


            m_lReturn = m_oBusiness.RepostClaimTransactions(lClaim_id:=m_lClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="RepostClaimTransactions", r_lFunctionReturn:=result)
            End If


            m_lReturn = m_oBusiness.GetClaimRIArrangementDetails(v_lClaimId:=m_lClaimId, r_lVersionId:=lVersionId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetClaimRIArrangementDetails", r_lFunctionReturn:=result)
            End If
            'If reversing then get the RiArrangement_version
            sClaimVersionDescription = "Reversal due to R/I map change to version" & lVersionId

            ''''''''''''''''''''''''lReturn = m_oBusiness.CreateEvent( _
            'v_lClaimId:=m_lClaimId, v_lEventType:=lEventType, _
            'v_sDescription:=sClaimVersionDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateEvent Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If
        Return result

    End Function

    '*******************************************************************************
    ' Name : AddExternalHandlerTask
    '
    ' Desc : add external handler tasks to work manager
    '
    ' Hist : 23 June 2001 Created - Tinny
    '*******************************************************************************
    Private Function AddExternalHandlerTask(ByVal v_sClaimNumber As String) As Integer
        Dim result As Integer = 0
        Dim oObject As bCLMInfoChklst.Business



        result = gPMConstants.PMEReturnCode.PMTrue


        Dim temp_oObject As Object
        If g_oObjectManager.GetInstance(temp_oObject, "bCLMInfoChklst.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
            oObject = temp_oObject

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise Information Check List business object", vApp:=ACApp, vClass:=ACClass, vMethod:="AddExternalHandlerTask")

        Else
            oObject = temp_oObject
        End If


        m_lReturn = oObject.CreateClaimHandlerTasks(v_sClaimNumber)

        If m_lReturn = gPMConstants.PMEReturnCode.PMFail Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateDocument
    '
    ' Description: Retrieves document id from FindDocTemplate component for
    '               given type and Policy Id and generates document via
    '               Document Template component.
    '
    ' History: 06/03/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateDocument(ByVal v_iDocType As Integer, ByVal v_sTransactionType As Object, ByVal v_iMode As Integer, ByVal v_lInsuranceFilecnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_sSpoolDesc As String) As Integer
        Dim result As Integer = 0

        Dim oDocTemplate As Object

        Dim obPMBDocLink As bPMBDocLink.Business
        Dim lLeadAgentcnt As Integer
        Dim iIsClientCopy, iIsAgentCopy, iIsOfficeCopy, iCopies As Integer
        Dim vDocumentArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_obPMBDocLink As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_obPMBDocLink, "bPMBDocLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        obPMBDocLink = temp_obPMBDocLink

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GenerateDocument", "bPMBDocLink.Business - Cannot create business object")
        End If


        m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=2, v_lInsurance_File_Cnt:=v_lInsuranceFilecnt, v_lProcessType_Docs_ID:=v_iDocType, v_lProcess_Type_Code:=v_sTransactionType, v_dtEffectiveDate:=m_dtEffectiveDate, r_vResultArray:=vDocumentArray)

        If Information.IsArray(vDocumentArray) Then
            '     r_lDocumentTemplateId = r_vDocumentArray(0, 0)
            '    r_lDocumentTypeId = r_vDocumentArray(1, 0)
        End If


        If Information.IsArray(vDocumentArray) Then

            For lCount As Integer = vDocumentArray.GetLowerBound(1) To vDocumentArray.GetUpperBound(1)

                lLeadAgentcnt = gPMFunctions.ToSafeLong(vDocumentArray(6, lCount), 0)

                iIsClientCopy = gPMFunctions.ToSafeInteger(vDocumentArray(2, lCount))

                'If there is not Lead Agent means its a direct business
                'So agent copy should not be spooled
                If lLeadAgentcnt > 0 Then
                    iIsAgentCopy = gPMFunctions.ToSafeInteger(vDocumentArray(3, lCount), 0)
                Else
                    iIsAgentCopy = 0
                End If

                iIsOfficeCopy = gPMFunctions.ToSafeInteger(vDocumentArray(4, lCount), 0)

                iCopies = iIsClientCopy + iIsAgentCopy + iIsOfficeCopy

                If iCopies > 0 Then
                    For iCopyCount As Integer = 1 To iCopies
                        m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong(vDocumentArray(0, lCount), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong(vDocumentArray(1, lCount), 0), r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iIsClient:=gPMFunctions.ToSafeInteger(vDocumentArray(2, lCount)), v_iIsAgent:=gPMFunctions.ToSafeInteger(vDocumentArray(3, lCount)), v_iIsOffice:=gPMFunctions.ToSafeInteger(vDocumentArray(4, lCount)), v_iProductionOrder:=gPMFunctions.ToSafeInteger(vDocumentArray(5, lCount)))

                    Next iCopyCount
                Else
                    m_lReturn = UseTheTemplate(r_lDocId:=gPMFunctions.ToSafeLong(vDocumentArray(0, lCount), 0), r_lDocTypeId:=gPMFunctions.ToSafeLong(vDocumentArray(1, lCount), 0), r_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_iIsClient:=gPMFunctions.ToSafeInteger(vDocumentArray(2, lCount)), v_iIsAgent:=gPMFunctions.ToSafeInteger(vDocumentArray(3, lCount)), v_iIsOffice:=gPMFunctions.ToSafeInteger(vDocumentArray(4, lCount)), v_iProductionOrder:=gPMFunctions.ToSafeInteger(vDocumentArray(5, lCount)))

                End If
            Next lCount
        Else

        End If

        oDocTemplate = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDocID (Private)
    '
    ' Description: Get document template id and document type id
    '
    ' ***************************************************************** '

    'Private Function GetDocID(ByVal v_lInsuranceFilecnt As Integer, ByVal v_iDocType As Integer, ByVal v_sTransactionType As String, ByRef r_lDocTemplateID As Integer, ByRef r_lDocTypeId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oFindDocTemplate As iPMBFindDocTemplate.Interface_Renamed
    'Dim vKeys As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oFindDocTemplate = New iPMBFindDocTemplate.Interface_Renamed()
    '
    'If oFindDocTemplate Is Nothing Then
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMBFindDocTemplate object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    'initialise the object
    'If CType(oFindDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindDocTemplate = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oFindDocTemplate.SetProcessModes(vTransactionType:=v_sTransactionType)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindDocTemplate = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'oFindDocTemplate.InsuranceFileCnt = v_lInsuranceFilecnt
    'oFindDocTemplate.ProcessType = v_iDocType
    'oFindDocTemplate.Mode = 2 'invisible merge
    '
    'If oFindDocTemplate.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindDocTemplate = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'r_lDocTemplateID = oFindDocTemplate.DocumentTemplateId
    'r_lDocTypeId = oFindDocTemplate.DocumentTypeId
    '
    'm_lReturn = oFindDocTemplate.Terminate()
    '
    'oFindDocTemplate = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document template id and document type id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    '*******************************************************************************
    ' Name : PostTPRecoveryToOrion
    '
    ' Desc : post total amount of recovery to orion for this peril
    '
    ' Hist : 28/03/2001 Created - Tinny
    '*******************************************************************************

    'Private Function PostTPRecoveryToOrion(ByVal v_lInsuranceFilecnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lPerilID As Integer, ByVal v_cTransAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_sClassOfBusiness As String, Optional ByVal v_lPartyCnt As Integer = 0) As Integer
    'Dim result As Integer = 0
    'Dim bControlTransClaims As Object
    '
    '

    'Dim oObject As bControlTransClaims.Automated
    'Dim lDebitAccountID, lCreditAccountID As Integer
    'Dim sCreditAccountCode As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sCreditAccountCode = "CLMEXP" & v_sClassOfBusiness
    '
    'create object to send to orion
    'Dim temp_oObject As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bControlTransClaims.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oObject = temp_oObject
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bControlTransClaims object", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTPRecoveryToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End If
    '
    'get debit account id - use party count if we have it
    'If v_lPartyCnt <> 0 Then

    'm_lReturn = oObject.GetAccountID(r_lAccountId:=lDebitAccountID, v_lPartyCnt:=v_lPartyCnt)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oObject.Terminate()
    'oObject = Nothing
    '
    'Return result
    'End If
    '
    'Else
    '

    'm_lReturn = oObject.GetAccountID(r_lAccountId:=lDebitAccountID, v_sAccountCode:=v_sDebitAccountCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oObject.Terminate()
    'oObject = Nothing
    '
    'Return result
    'End If
    '
    'End If
    '
    'get credit account id

    'm_lReturn = oObject.GetAccountID(r_lAccountId:=lCreditAccountID, v_sAccountCode:=sCreditAccountCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oObject.Terminate()
    'oObject = Nothing
    '
    'Return result
    'End If
    '
    'data which goes in transfer export folder

    'oObject.DebitTransLedgerCode = "SL"

    'oObject.DebitAccountLedgerCode = v_sDebitAccountCode

    'oObject.CreditTransLedgerCode = "SL"

    'oObject.CreditAccountLedgerCode = sCreditAccountCode
    '
    'data which goes in stats folder/detail and transaction detail

    'oObject.DebitAccountID = lDebitAccountID

    'oObject.CreditAccountID = lCreditAccountID

    'oObject.TransactionTypeID = 28

    'oObject.TransactionTypeCode = "C_CR" 'claim maintenance

    'oObject.DocumentTypeId = 29 'Claim receipt

    'oObject.InsuranceFileCnt = v_lInsuranceFilecnt

    'oObject.ClaimId = v_lClaimId

    'oObject.PerilID = v_lPerilID

    'oObject.DebitCredit = "C"

    'oObject.DocumentComment = "Salvage for claim number " & v_lClaimId

    'oObject.TransactionAmount = v_cTransAmount
    '

    'm_lReturn = oObject.ProcessJournal()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTPRecoveryToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'End If
    '

    'm_lReturn = oObject.Terminate()
    '
    'oObject = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostTPRecoveryToOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTPRecoveryToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    '*******************************************************************************
    ' Name : PostSalvageToOrion
    '
    ' Desc : post total amount of salvage to orion for this peril
    '
    ' Hist : 28/03/2001 Created - Tinny
    '*******************************************************************************

    'Private Function PostSalvageToOrion(ByVal v_lInsuranceFilecnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lPerilID As Integer, ByVal v_cTransAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_sClassOfBusiness As String, Optional ByVal v_lPartyCnt As Integer = 0) As Integer
    'Dim result As Integer = 0
    'Dim bControlTransClaims As Object
    '
    '

    'Dim oObject As bControlTransClaims.Automated
    'Dim lDebitAccountID, lCreditAccountID As Integer
    'Dim sCreditAccountCode As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sCreditAccountCode = "CLMEXP" & v_sClassOfBusiness
    '
    'create object to send to orion
    'Dim temp_oObject As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, "bControlTransClaims.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oObject = temp_oObject
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bControlTransClaims object", vApp:=ACApp, vClass:=ACClass, vMethod:="PostSalvageToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End If
    '
    'get debit account id - use party count if we have it
    'If v_lPartyCnt <> 0 Then

    'm_lReturn = oObject.GetAccountID(r_lAccountId:=lDebitAccountID, v_lPartyCnt:=v_lPartyCnt)
    '
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oObject.Terminate()
    'oObject = Nothing
    '
    'Return result
    'End If
    '
    'Else
    '

    'm_lReturn = oObject.GetAccountID(r_lAccountId:=lDebitAccountID, v_sAccountCode:=v_sDebitAccountCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oObject.Terminate()
    'oObject = Nothing
    '
    'Return result
    'End If
    '
    'End If
    '
    'get credit account id

    'm_lReturn = oObject.GetAccountID(r_lAccountId:=lCreditAccountID, v_sAccountCode:=sCreditAccountCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lReturn = oObject.Terminate()
    'oObject = Nothing
    '
    'Return result
    'End If
    '
    'data which goes in transfer export folder

    'oObject.DebitTransLedgerCode = "SL"

    'oObject.DebitAccountLedgerCode = v_sDebitAccountCode

    'oObject.CreditTransLedgerCode = "SL"

    'oObject.CreditAccountLedgerCode = sCreditAccountCode
    '
    'data which goes in stats folder/detail and transaction detail

    'oObject.DebitAccountID = lDebitAccountID

    'oObject.CreditAccountID = lCreditAccountID

    'oObject.TransactionTypeID = 28

    'oObject.TransactionTypeCode = "C_CR" 'claim maintenance

    'oObject.DocumentTypeId = 29 'Claim receipt

    'oObject.InsuranceFileCnt = v_lInsuranceFilecnt

    'oObject.ClaimId = v_lClaimId

    'oObject.PerilID = v_lPerilID

    'oObject.DebitCredit = "C"

    'oObject.DocumentComment = "Salvage for claim number " & v_lClaimId

    'oObject.TransactionAmount = v_cTransAmount
    '

    'm_lReturn = oObject.ProcessJournal()
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostSalvageToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'End If
    '

    'm_lReturn = oObject.Terminate()
    '
    'oObject = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSalvageToOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostSalvageToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockClaim
    '
    ' Description:
    '
    ' History: 17/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User



        result = gPMConstants.PMEReturnCode.PMTrue

        'Tracy Richards - Protect against trying to unlcok claims of id = 0,
        'which may be the case for brand new claims, but which do not need unlocking.
        If v_lOriginalClaimID > 0 Then

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)

            ' DD 26/7/2004 - PN13122
            ' Only error if return = PMError. If return = PMFalse, it just means
            ' the claim was not locked in the first place.
            'If (m_lReturn <> PMTrue) Then
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnUnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result

            End If

            oPMLock = Nothing
        End If

        Return result

    End Function

    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: CheckUserPaymentAuthority
    '
    ' Description: Checks the current user has sufficient authority to create a payment
    '
    ' History : 06/05/2003 - Ajit Kumar - created
    '
    ' ***************************************************************** '
    Private Function CheckUserPaymentAuthority(ByRef r_bUserAuthorised As Boolean) As Integer
        Dim result As Integer = 0
        Dim oAuthorityBusiness As bSIRCheckAuthorityLevel.Business
        Dim vDataArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the business object via
        ' the public object manager.
        Dim temp_oAuthorityBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oAuthorityBusiness, "bSIRCheckAuthorityLevel.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oAuthorityBusiness = temp_oAuthorityBusiness ' Alix - PN10416 - Changed from PMGetLocalBusiness to PMGetViaClientManager

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'bSIRCheckAuthorityLevel.Business'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserPaymentAuthority", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            r_bUserAuthorised = False
            Return result
        End If

        If g_lInsuranceFilecnt > 0 Then
            'get the product Id associated with this ins file cnt

            m_lReturn = m_oBusiness.GetInsFileCntProductId(v_lInsuranceFilecnt:=g_lInsuranceFilecnt, r_vResultArray:=vDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vDataArray) Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetInsFileCntProductId", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserPaymentAuthority", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                r_bUserAuthorised = False
                Return result
            End If

        Else
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsuranceFileCnt not set", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserPaymentAuthority", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            r_bUserAuthorised = False
            Return result
        End If


        oAuthorityBusiness.CurrentUserId = g_oObjectManager.UserID

        oAuthorityBusiness.AuthUserId = g_oObjectManager.UserID


        oAuthorityBusiness.ProductID = vDataArray(0, 0)

        oAuthorityBusiness.TransTypeCode = m_sTransactionType

        ' If this option is switched on then user can do non cumulative payments
        ' keeping in context with the option ' Max Unauthorised No of claim payments'.
        ' Checks to validate this option is already made prior to reaching here.
        oAuthorityBusiness.PaymentAmount = CDbl(m_cThisPaymentAmount)



        oAuthorityBusiness.PaymentCurrencyID = m_lThisPaymentCurrencyId

        oAuthorityBusiness.InsuranceFileCnt = g_lInsuranceFilecnt

        oAuthorityBusiness.OriginalPaymentCurrencyID = m_lOriginalThisPaymentCurrencyId

        oAuthorityBusiness.OriginalPaymentAmount = m_cOriginalThisPaymentAmount


        m_lReturn = oAuthorityBusiness.LoadRule()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            m_lReturn = oAuthorityBusiness.ExecuteRule(r_bUserAuthorised)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ExecuteRule", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserPaymentAuthority", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
        Else
            ' Log Error.
            ' iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Load  Rule / Rules file may be missing", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUserPaymentAuthority", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        oAuthorityBusiness.Dispose()
        oAuthorityBusiness = Nothing

        Return result

    End Function


    'AK 270503 - Function to create work manager task for claims payment referrals
    Public Function CreateReferralTask(ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal IsRecommender As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim oWrkMgrTaskControl As bPMWrkTaskInstance.TaskControl
        Dim oStepAuthorization As bCLMAuthorisePayments.StepAuthorization
        Try

            Dim lTaskInstanceCnt, lTaskGroupId As Integer
            Dim lUserGroupId As Integer
            Dim vOptionValue As Object
            Dim sUserGroupCode As String = ""
            Dim vKeyArray(,) As Object
            Dim sTaskDescription, sClaimNumber As String
            Dim sErrorMessage As String

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Default TaskGroupID

            lTaskGroupId = ACTClaimAdminTaskGroupID

            'Get an instance of the business object via the public object manager.
            Dim temp_oWrkMgrTaskControl As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkMgrTaskControl, "bPMWrkTaskInstance.TaskControl", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oWrkMgrTaskControl = temp_oWrkMgrTaskControl
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateReferralTask failed to create bPMWrkTaskInstance.TaskControl", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Initialise with the Sirius user and password

            m_lReturn = oWrkMgrTaskControl.Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise WorkManager Task Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                If Not (oWrkMgrTaskControl Is Nothing) Then

                    oWrkMgrTaskControl.Dispose()
                    oWrkMgrTaskControl = Nothing
                End If
                Return result
            End If

            'Determine if we have the MultiStepApprova flag set
            'developer guide no.98
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiStepApproval, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateReferralTask failed to getProductOptionValue", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                If Not (oWrkMgrTaskControl Is Nothing) Then

                    oWrkMgrTaskControl.Dispose()
                    oWrkMgrTaskControl = Nothing
                End If
                Return result
            End If

            If gPMFunctions.NullToString(vOptionValue) = "1" And IsRecommender = False Then

                Dim temp_oStepAuthorization As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oStepAuthorization, "bCLMAuthorisePayments.StepAuthorization", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oStepAuthorization = temp_oStepAuthorization
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateReferralTask failed to create bCLMAuthorisePayments.StepAuthorization", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    If Not (oWrkMgrTaskControl Is Nothing) Then

                        oWrkMgrTaskControl.Dispose()
                        oWrkMgrTaskControl = Nothing
                    End If
                    Return result
                End If

                'Initialise with the Sirius user and password

                m_lReturn = oStepAuthorization.Initialise(sUserName:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise Claim Authorise Payment Step Approval Object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    If Not (oStepAuthorization Is Nothing) Then

                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If

                    If Not (oWrkMgrTaskControl Is Nothing) Then

                        oWrkMgrTaskControl.Dispose()
                        oWrkMgrTaskControl = Nothing
                    End If
                    Return result
                End If


                oStepAuthorization.PaymentType = ACClaimPaymentsType

                oStepAuthorization.PaymentID = m_lClaimId

                oStepAuthorization.PaymentAmount = CDbl(m_cAmount)

                oStepAuthorization.PaymentCreatorUserID = g_oObjectManager.UserID

                'Get the step Group code
                sUserGroupCode = ""

                m_lReturn = oStepAuthorization.GetStepGroupCode(r_sGroupCode:=sUserGroupCode, r_sErrorMessage:=sErrorMessage)
                If sErrorMessage <> "" Then
                    MsgBox(sErrorMessage, vbOKOnly)
                    If (oStepAuthorization Is Nothing) = False Then
                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If

                    If (oWrkMgrTaskControl Is Nothing) = False Then
                        oWrkMgrTaskControl.Dispose()
                        oWrkMgrTaskControl = Nothing
                    End If
                    Return result
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso sErrorMessage <> "" Then
                    result = m_lReturn
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetStepGroupCode", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    If Not (oStepAuthorization Is Nothing) Then

                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If

                    If Not (oWrkMgrTaskControl Is Nothing) Then

                        oWrkMgrTaskControl.Dispose()
                        oWrkMgrTaskControl = Nothing
                    End If
                    Return result
                End If

                'This will return the ID for the group from the step NOT the Claim ID, just used exsiting functionality

                m_lReturn = m_oBusiness.GetClaimAdminGroup(r_lGroupId:=lUserGroupId, v_sGroupCode:=sUserGroupCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateReferralTask failed to GetClaimAdminGroup from the Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    If Not (oStepAuthorization Is Nothing) Then

                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If

                    If Not (oWrkMgrTaskControl Is Nothing) Then

                        oWrkMgrTaskControl.Dispose()
                        oWrkMgrTaskControl = Nothing
                    End If
                    Return result
                End If

                m_lReturn = m_oBusiness.GetClaimNumber(v_lClaimId:=m_lClaimId, r_sClaimNumber:=sClaimNumber)

                sTaskDescription = "Authorise Claim Payment for Claim Number: " & sClaimNumber & " for the amount of: " & StringsHelper.Format(m_cThisPaymentAmount, "#,##0.00")
            Else

                m_lReturn = m_oBusiness.GetClaimAdminGroup(lUserGroupId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateReferralTask failed to GetClaimAdminGroup from the Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    If Not (oStepAuthorization Is Nothing) Then

                        oStepAuthorization.Dispose()
                        oStepAuthorization = Nothing
                    End If

                    If Not (oWrkMgrTaskControl Is Nothing) Then

                        oWrkMgrTaskControl.Dispose()
                        oWrkMgrTaskControl = Nothing
                    End If
                    Return result
                End If
                m_lReturn = m_oBusiness.GetClaimNumber(v_lClaimId:=m_lClaimId, r_sClaimNumber:=sClaimNumber)

                sTaskDescription = sClaimNumber & " - " & ACAuthoriseTask
            End If

            ReDim vKeyArray(1, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRealClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lClaimId

            ' We should not remove the old ones as we can create one for each payment.
            '    'Remove old authorisation tasks, to make sure there is only one of these
            '    m_lReturn& = m_oBusiness.RemoveAuthTasks(v_sDescription:=ACAuthoriseTask)
            '    If m_lReturn <> PMTrue Then
            '        CreateReferralTask = m_lReturn&
            '        ' Log Error Message
            '        LogMessage iType:=PMLogOnError, _
            ''                   sMsg:="Failed to Remove old work manager tasks for authorising payments", _
            ''                   vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", _
            ''                   vErrNo:=Err.Number, vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            'Create the WorkManager Task

            m_lReturn = oWrkMgrTaskControl.CreateNewByCode(v_lPMWrkTaskGroupID:=lTaskGroupId, v_sPMWrkTaskCode:="AUTHPMNT", v_sCustomer:="System", v_dtTaskDueDate:=DateTime.Today, v_lPMUserGroupID:=lUserGroupId, v_sDescription:=sTaskDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_vKeyArray:=vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create WorkManager Task.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                If Not (oStepAuthorization Is Nothing) Then

                    oStepAuthorization.Dispose()
                    oStepAuthorization = Nothing
                End If

                If Not (oWrkMgrTaskControl Is Nothing) Then

                    oWrkMgrTaskControl.Dispose()
                    oWrkMgrTaskControl = Nothing
                End If
                Return result
            End If

            ' Work Manager task create successfully Assign the return value
            r_lPMWrkTaskInstanceCnt = lTaskInstanceCnt

            If Not (oWrkMgrTaskControl Is Nothing) Then

                oWrkMgrTaskControl.Dispose()
                oWrkMgrTaskControl = Nothing
            End If

            If Not (oStepAuthorization Is Nothing) Then

                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create WorkManager Task", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReferralTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oStepAuthorization Is Nothing) Then

                oStepAuthorization.Dispose()
                oStepAuthorization = Nothing
            End If

            If Not (oWrkMgrTaskControl Is Nothing) Then

                oWrkMgrTaskControl.Dispose()
                oWrkMgrTaskControl = Nothing
            End If

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UseAuthorisedScriptsForClaimPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-03-2005 : PN19467
    ' ***************************************************************** '
    Private Function UseAuthorisedScriptsForClaimPayments(ByRef r_bCheckAuthorisation As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UseAuthorisedScriptsForClaimPayments"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vValue As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = CType(GetProductDetails(r_bCheckAuthorisation:=r_bCheckAuthorisation), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed ", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    '**************************************************************************************
    ' if claim status id is 3 (closed) and reserve and recovery outstandings are not zero
    ' then display message and reset status to 2 (live)
    '**************************************************************************************
    Private Function ProcessClaimStatus(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue


        If m_oBusiness.GetReserveRecoveryOS(v_lClaimId, vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get outstanding amount for reserve/recovery", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result
        End If

        If Not Information.IsArray(vResultArray) Then
            MessageBox.Show("Reserve/Recovery details not found", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result
        End If

        'is it closed?
        ''Saurabh PN  58931
        If gPMFunctions.ToSafeLong(vResultArray(0, 0), 0) = 3 Or gPMFunctions.ToSafeLong(vResultArray(0, 0), 0) = 5 Then
            'warn user and change status back to live if either reserve or recovery is not zero
            If Math.Round(gPMFunctions.ToSafeCurrency(vResultArray(1, 0), 0)) <> 0 Or Math.Round(gPMFunctions.ToSafeCurrency(vResultArray(2, 0), 0)) <> 0 Then
                MessageBox.Show("Claim cannot be closed unless all outstanding reserves and recoveries are set to zero." & Strings.Chr(13) & Strings.Chr(10) &
                                "Claim will remain open.", "Change Claim Status to Closed", MessageBoxButtons.OK, MessageBoxIcon.Information)

                'change work claim status back to live

                result = m_oBusiness.UpdateClaimStatus(m_lClaimId)
            End If
        End If


        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetClaimPaymentAccountsDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-01-2006 : Cheque Production Workflow (ATD16)
    ' ***************************************************************** '
    Private Function GetClaimPaymentAccountsDetails(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentAccountsDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vClaimPaymentDetails As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        ' get the claim payment details

        lReturn = m_oBusiness.GetClaimPaymentAccountsDetails(v_lClaimId:=v_lClaimId, r_vResults:=vClaimPaymentDetails)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentAccountsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Information.IsArray(vClaimPaymentDetails) Then
            ' get required details for creation of accounts cashlist / cashlistitem
            m_lClaimPaymentId = gPMFunctions.ToSafeLong(vClaimPaymentDetails(kClaimPaymentAccountDetailsClaimPaymentId, 0), 0)
            m_crTotalClaimPaymentAmount = gPMFunctions.ToSafeCurrency(vClaimPaymentDetails(kClaimPaymentAccountDetailsTotalPaymentAmount, 0), 0)
            m_lAccountId = gPMFunctions.ToSafeLong(vClaimPaymentDetails(kClaimPaymentAccountDetailsAccountId, 0), 0)
            m_lCurrencyId = gPMFunctions.ToSafeLong(vClaimPaymentDetails(kClaimPaymentAccountDetailsCurrencyId, 0), 0)
            m_lMediaTypeId = gPMFunctions.ToSafeLong(vClaimPaymentDetails(kClaimPaymentAccountDetailsMediaTypeID, 0), 0)
            m_lSourceId = gPMFunctions.ToSafeLong(vClaimPaymentDetails(kClaimPaymentAccountDetailsSourceId, 0), 0)
        Else
            gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentAccountsDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function


    Public Function ClaimPolicyChanged(ByVal v_lClaimId As Integer, ByRef r_sClaimOldPolicyRef As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "ClaimPolicyChanged"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_lReturn = m_oBusiness.GetClaimOldPolicy(v_lClaimId, r_sClaimOldPolicyRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ClaimPolicyChanged Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lClaimId", v_lClaimId)
            oDict.Add("r_sClaimOldPolicyRef", r_sClaimOldPolicyRef)
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to determine the Policy Changed on a Claim", vApp:=ACApp, vClass:=ACClass, vMethod:="ClaimPolicyChanged", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    Private Function GetProductDetails(Optional ByRef r_bCheckAuthorisation As Boolean = False, Optional ByRef r_bIsRecommender As Boolean = False, Optional ByRef r_bIs_Multiple_Claims_Payments As Boolean = False, Optional ByRef r_bIsSuppressReserve As Boolean = False, Optional ByRef r_nAuthorisation_Threshold As Decimal = False) As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object 'PN-69520

        Const kMethodName As String = "GetProductDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_ProductBusiness As bSIRProduct.Business
        Dim vProductDetails As Object
        Dim bIs_Recommend_Claim_Payment, bCheckAuthorisation, bRun_authorisation_scripts, bIs_Multiple_Claims_Payments As Boolean
        'Start - Sushil Kumar(PN-69520)
        Dim bIsSuppressReserve As Boolean
        'End - Sushil Kumar(PN-69520)

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_o_ProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimId, r_bIs_Recommend_Claim_Payment:=bIs_Recommend_Claim_Payment, r_bRun_authorisation_scripts_claim_payments:=bRun_authorisation_scripts, r_bIs_Multiple_claims_payments:=bIs_Multiple_Claims_Payments, r_bSuppressReserve:=bIsSuppressReserve, r_nAuthorisation_Threshold:=r_nAuthorisation_Threshold)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_bCheckAuthorisation = bRun_authorisation_scripts
            r_bIsRecommender = bIs_Recommend_Claim_Payment
            r_bIs_Multiple_Claims_Payments = bIs_Multiple_Claims_Payments
            'Start - Sushil Kumar(PN-69520)
            r_bIsSuppressReserve = bIsSuppressReserve
            'End - Sushil Kumar(PN-69520)


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            o_ProductBusiness = Nothing

        End Try
        Return result
    End Function

    Private Function UseTheTemplate(ByRef r_lDocId As Integer, ByRef r_lDocTypeId As Integer, ByRef r_lInsuranceFileCnt As Integer, Optional ByVal v_iIsClient As Integer = 0, Optional ByVal v_iIsAgent As Integer = 0, Optional ByVal v_iIsOffice As Integer = 0, Optional ByVal v_iProductionOrder As Integer = 1) As Integer
        Dim result As Integer = 0
        Dim oObject As iPMBDocTemplate.Interface_Renamed



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oObject = temp_oObject

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If


        oObject.PartyCnt = m_lPartycnt

        oObject.InsuranceFileCnt = r_lInsuranceFileCnt
        ''oObject.InsuranceFolderCnt = m_lInsuranceFolderCnt 'MSB301001


        oObject.DocumentTemplateId = r_lDocId

        oObject.DocumentTypeId = r_lDocTypeId

        oObject.Mode = 4 'Spool Doc Mode

        oObject.IsClient = v_iIsClient

        oObject.IsAgent = v_iIsAgent

        oObject.IsOffice = v_iIsOffice

        oObject.ProductionOrder = v_iProductionOrder


        m_lReturn = oObject.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If

        oObject.Dispose()

        oObject = Nothing

        Return result

    End Function
End Class