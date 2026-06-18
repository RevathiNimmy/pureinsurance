Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms

'Developer Guide No: 129
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: {TodaysDate}
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    'Developer Guide No. 50
    Dim frmInterface As frmInterface
    Const PMKeyNameOperateMode As String = "claim_mode"
    Const PMKeyNamePolicyID As String = "insurancefile_cnt"
    Const PMKeyNamePolicyNumber As String = "policy_number"
    Const PMKeyNameClaimNumber As String = "claim_ref"
    Const PMKeyNameClaimID As String = "claim_cnt"
    Const PMKeyNameRiskTypeID As String = "risk_type_id"
    Const PMKeyNameClaimDate As String = "claim_date"
    Const PMKeyNameViewRiskFlag As String = "view_risk_flag"
    Const PMKeyNameNoTransaction As String = "no_transactions"
    Const PMKeyNameIsReserveLimitExceeded As String = "is_reserve_limit_exceeded"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_lclaimid As Integer
    Private m_lRisk As Integer
    Private m_lPolicyId As Integer
    Private m_bViewRiskFlag As Boolean
    Private m_lPMAuthorityLevel As Integer
    Private m_sClaimNumber As String = ""
    Private m_oGeneral As iCLMRiskDetails.General
    Private m_lErrorNumber As Integer
    Private m_lClaimMode As Integer
    Private m_lPartyCnt As Integer
    ' Stores the exit status of the interface.
    Private m_lStatus As Integer
    Private m_cThisPayment As Decimal
    Private m_lThisPaymentCurrencyID As Integer
    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Has Find Claim been run?
    Private m_bFindClaim As Boolean
    'Set to PMTrue to delete work table when cancelled
    Private m_lDeleteWorkTableFlag As gPMConstants.PMEReturnCode
    'If the roadmap
    Private m_bFurtherPayments As Boolean
    'If returning to do more payments
    Private m_lOriginalClaimId As Integer
    Private m_lRiskTypeId As Integer

    ' RVH 29/03/2004 CQ5026: Store claim details at the point claim is loaded - need this info to compare when claimsbuilder screen is complete
    Private m_lClaimStatusOnLoad As Integer
    Private m_lClaimProgressStatusOnLoad As Integer
    Private m_bBalanceAndCloseClaim As Boolean
    Private m_bOpenClaimNoTrans As Boolean
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
    Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
    Private m_bReserveLimitExceeded As Boolean
    Private m_bIsReserveUpdatednTaskCompleted As Boolean
    Dim oBusiness As bCLMRiskDetails.Business

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

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

            m_oGeneral = New iCLMRiskDetails.General()

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

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
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
        Dim bMissingKey As Boolean 'KN(CMG) 31/10/02 Added MKW130303 PN2382 Catchup
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_vKeyArray = VB6.CopyArray(vKeyArray)
            ' If the Flag is true then the Risk Details screen is opened from
            ' RiskType Admin screen else from the road map.
            m_bViewRiskFlag = False
            bMissingKey = False

            'KR 13/02/2003 default delete work_claim record unless specifically set otherwise
            m_lDeleteWorkTableFlag = gPMConstants.PMEReturnCode.PMTrue

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the correct key array item.
                ' {* USER DEFINED CODE (Begin) *}

                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMKeyNameClaimID

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            m_lclaimid = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Else
                            m_lclaimid = -1
                        End If
                    Case PMKeyNameRiskTypeID
                        ' IMP - The Risk in Underwriting is RiskID
                        '       The Risk in Broking is RiskTypeId
                        '       This is to be checked with the SET keys & Get keys
                        '       of open claim & maintain claim. - DG

                        If (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))) <> "" Then

                            Dim dbNumericTemp2 As Double
                            If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                                m_lRisk = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            Else
                                m_lRisk = -1
                            End If
                        Else
                            result = gPMConstants.PMEReturnCode.PMError
                            bMissingKey = True
                            MessageBox.Show("You Cannot enter Risk Details without Completing Find Claims and Claim Details beforehand." & Strings.Chr(13) & Strings.Chr(10) & "Please restart the process.", "Risk Details", MessageBoxButtons.OK)
                            Exit For
                        End If
                    Case PMKeyNamePolicyID

                        Dim dbNumericTemp3 As Double
                        If Double.TryParse(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                            m_lPolicyId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        Else
                            m_lPolicyId = -1
                        End If
                    Case PMKeyNameViewRiskFlag

                        m_bViewRiskFlag = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMKeyNameOperateMode

                        m_lClaimMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "DeleteWorkTableFlag"

                        m_lDeleteWorkTableFlag = CType(CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), gPMConstants.PMEReturnCode)
                    Case "FurtherPayments"

                        m_bFurtherPayments = (CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = 1)
                    Case PMNavKeyConst.PMKeyNameRealClaimID

                        m_lOriginalClaimId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameBalanceAndCloseClaim

                        m_bBalanceAndCloseClaim = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMKeyNameNoTransaction

                        m_bOpenClaimNoTrans = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
                    Case PMNavKeyConst.PMKeyNameScreenCaption

                        m_sScreenCaption = gPMFunctions.ToSafeString(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
                    Case PMKeyNameIsReserveLimitExceeded

                        m_bReserveLimitExceeded = gPMFunctions.ToSafeBoolean(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                End Select
            Next lRow

            'MKW130303 PN2382 1.6.9 --> 1.8.6 Catchup
            'KN (CMG) 31/10/02 Start
            If bMissingKey Then
                m_oGeneral.Dispose()
                
            End If

            'Destroy the instance of the General Object
            'from memory
            m_oGeneral = Nothing

            'KN (CMG) 31/10/02 End
            'END MKW130303 PN2382 1.6.9 --> 1.8.6 Catchup

            ' RDC 27022003 we cannot terminate component here as this will 'error' the navigator process,
            '              so we must inform the Start method if there is a problem.
            m_bFindClaim = True

            'DC310303 -ISS3136 -Only give Find Claim message if not just veiwing risk details
            If Not m_bViewRiskFlag Then
                If (m_lPolicyId = -1 And m_lclaimid = -1) Or (m_lPolicyId = 0 And m_lclaimid = 0) Then
                    m_bFindClaim = False

                    ' claim has not been selected
                    MessageBox.Show("A claim has not been selected:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Use 'Find Claim' to search and select the required claim.", "Open Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If
            End If

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
        Const PMClaimNumber As String = "claim_ref"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            vKeyArray = Nothing
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
            ReDim vKeyArray(1, 5)
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMClaimNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sClaimNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_cThisPayment


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameCurrencyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lThisPaymentCurrencyID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMKeyNameClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lclaimid
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameScreenCaption

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "ReserveUpdatednTaskCompleted"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_bIsReserveUpdatednTaskCompleted
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            'DC010503 -ISS3387 -was vKeyArray
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "claim_ref"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sClaimNumber

            ' {* USER DEFINED CODE (End) *}

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

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CType(CInt(vNavigate), gPMConstants.PMENavigateButtonStatus)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
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
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        Dim result As Integer = 0

        'Const PMKeyNameSumInsured As String = "sum_insured"
        'Const PMKeyNameCurrentReserve As String = "current_reserve"
        'Const PMKeyRiskType As String = "risk_type"
        'Const PMKeyPartycnt As String = "party_cnt"
        Const PMKeyRiskID As String = "risk_id"
        Const PMKeyClaimGISScreenID As String = "GIS_Screen_id"
        Const PMKeyClaimMode As String = "claim_mode"

        Const PMKeyRowClaimID As Integer = 0
        Const PMKeyRowInsuranceFilecnt As Integer = 1
        Const PMKeyRowRiskID As Integer = 2
        Const PMKeyRowClaimMode As Integer = 3
        Const PMKeyRowClaimGISScreen As Integer = 4
        Const PMKeyRowWorkClaimId As Integer = 5
        Const PMKeyRowClaimTransactionType As Integer = 6
        Const PMKeyRowClaimInsFileCnt As Integer = 7
        Const PMKeyRowClaimRiskId As Integer = 8
        Const PMKeyRowPartyCnt As Integer = 9

        '***********
        ' MEvans : 28-05-2003 : 223
        Const PMKeyRowRiskTypeId As Integer = 10
        '***********

        Const PMKeyRowScreenCaption As Integer = 11
        Const PMKeyRowNoTransactions As Integer = 12


        Dim objClaimBuilder As iPMURisk.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim lOriginalClaimId As Integer

        '  Dim oBusiness As bCLMRiskDetails.Business
        Dim lScreenId, lClaimId As Integer

        '***********
        ' MEvans : 28-05-2003 : 223
        Dim vRiskType As Object
        '***********

        ' PW140703 - PS68
        Dim lWorkScreenID As Integer

        Dim vOrigClaimData As Object
        Dim strCaption As String = ""
        Dim vGisPolicyLink As Object
        Dim bLegacyClaim, bClaimsBuilder As Boolean


        Dim oClaimStatus As bCLMFindClaim.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_bFindClaim) Then
                Return result
            End If

            bClaimsBuilder = ClaimBuilderIsEnable()
            bLegacyClaim = False

            ' Get business object if we are running claims builder or in quick close mode
            ' If bClaimsBuilder Or m_bBalanceAndCloseClaim Then
            Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bCLMRiskDetails.Business", vInstanceManager:="ClientManager")
                oBusiness = temp_oBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="g_oObjectManager.GetInstance Failed for bCLMRiskDetails.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set process modes on business component", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            ' End If

            ' Check for balance and close mode
            If m_bBalanceAndCloseClaim Then
                ' Balance the claim

                m_lReturn = oBusiness.BalanceClaim(m_lclaimid)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.RaiseError("oBusiness.BalanceClaim", "Unable to balance reserves for this claim")
                End If

                ' And exit without displaying any UI
                Return result
            End If

            If bClaimsBuilder Then
                ' get original claim id
                If m_sTransactionType <> "C_CO" Then

                    ' check if being called to make an additional claim payment
                    If (m_sTransactionType = "C_CP") And m_bFurtherPayments Then
                        m_lReturn = ReloadClaimDetails()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reload claim while attempting to make additional claim payment.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If


                    m_lReturn = oBusiness.GetOriginalClaimID(v_lClaimId:=m_lclaimid, r_lOriginalClaimID:=lOriginalClaimId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get original claim id (GetOriginalClaimID)", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    lClaimId = lOriginalClaimId


                    Dim temp_oClaimStatus As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oClaimStatus, "bCLMFindClaim.Business", vInstanceManager:="ClientManager")
                    oClaimStatus = temp_oClaimStatus
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bCLMFindClaim.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If



                    If m_sTransactionType = "C_CR" And oClaimStatus.IsInfoOnlyClaim(v_lClaimId:=m_lclaimid) Then
                        'Information Only version and now will behave as open claim
                        m_sTransactionType = "C_CO"
                    Else

                        m_lReturn = oBusiness.GetGisPolicyLinkDetails(v_lClaimId:=m_lclaimid, r_vResults:=vGisPolicyLink)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed in GetGisPolicyLinkDetails for claim id: " & lClaimId, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Not Information.IsArray(vGisPolicyLink) Then
                            bLegacyClaim = True
                        End If
                    End If

                    'Dont need this anymore
                    oClaimStatus = Nothing
                Else
                    lClaimId = m_lclaimid
                End If
            End If

            ' Alix - 22/01/2003
            ' (legacy claims are classed as any claim that was created before claimsbuilder was enabled)
            If bClaimsBuilder And Not bLegacyClaim Then
                ' Call the new claim builder screen
                ' Create object
                Dim temp_objClaimBuilder As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_objClaimBuilder, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                objClaimBuilder = temp_objClaimBuilder

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="g_oObjectManager.GetInstance Failed for iPMURisk.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' get screen ID

                m_lReturn = oBusiness.GetRiskTypeScreenID(m_lRisk, lScreenId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Screen ID from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lScreenId = 0 Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No screen configured for the current risk type.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                'jmf 30/4/2003

                m_lReturn = oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lPolicyId, r_lPartyCnt:=m_lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get policy details from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '**********
                ' MEvans : 28-05-2003 : 223

                If oBusiness.GetRiskType(r_vResults:=vRiskType, v_lRiskId:=m_lRisk) = gPMConstants.PMEReturnCode.PMTrue Then

                    If Information.IsArray(vRiskType) Then

                        m_lRiskTypeId = CInt(vRiskType(0, 0))
                    End If

                End If
                '**********

                ' RVH 29/03/2004 CQ5026: Get basic claim details including current progress status

                m_lReturn = oBusiness.GetBasicClaimDetails(v_lClaimId:=m_lclaimid, r_vResultArray:=vOrigClaimData)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get basic claim details from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vOrigClaimData) Then

                    m_lClaimProgressStatusOnLoad = CInt(vOrigClaimData(2, 0))

                    m_lClaimStatusOnLoad = CInt(vOrigClaimData(5, 0))






                    strCaption = CStr(iPMFunc.GetResData(iLangID:=g_oObjectManager.LanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)) & " [" & CStr(vOrigClaimData(ACClient_name, 0)) & " " & CStr(vOrigClaimData(ACPolicy_Number, 0)) & "]" & New String(" "c, 2) & "Claim no. " & CStr(vOrigClaimData(ACClaim_Number, 0))

                End If

                ' PW140703 - PS68 - Use version of claims screen that it was originally
                ' created with, if applicable: start
                Select Case m_sTransactionType
                    Case "C_CO"
                        ' Store the screen ID in the work_claim table
                        ' PW151003 - Use work claim ID (advised by Russ Hill)

                        m_lReturn = oBusiness.SaveGISScreenID(lClaimId:=m_lclaimid, lScreenId:=lScreenId, bPerilLevel:=False)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Screen ID in Work Claim table", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case "C_CR", "C_CP"
                        ' Retrieve the screen ID from the work_claim table
                        ' PW151003 - Use work claim ID (advised by Russ Hill)

                        m_lReturn = oBusiness.GetGISScreenID(lClaimId:=m_lclaimid, r_lScreenId:=lWorkScreenID, bPerilLevel:=False)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Screen ID from Work Claim table", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' Override the screen ID retrieved if one existed on the work table
                        If lWorkScreenID <> 0 Then
                            lScreenId = lWorkScreenID
                        End If
                End Select
                ' PW140703 - PS68: end

                ' pass parameters to object

                '**********
                ' MEvans : 28-05-2003 : 223
                ' Increased size of array to hold risktypeid
                ReDim vKeyArray(1, PMKeyRowNoTransactions)
                '**********


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimID) = PMNavKeyConst.PMKeyNameRealClaimID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimID) = m_lclaimid


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowInsuranceFilecnt) = PMNavKeyConst.PMKeyNameInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowInsuranceFilecnt) = m_lPolicyId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowRiskID) = PMKeyRiskID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowRiskID) = m_lRisk


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimMode) = PMKeyClaimMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimMode) = m_lClaimMode


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimGISScreen) = PMKeyClaimGISScreenID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimGISScreen) = lScreenId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowWorkClaimId) = PMNavKeyConst.PMKeyNameWorkClaimID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowWorkClaimId) = lClaimId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimTransactionType) = PMNavKeyConst.PMKeyNameClaimTransactionType

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimTransactionType) = m_sTransactionType


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimInsFileCnt) = PMNavKeyConst.PMKeyNameClaimInsFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimInsFileCnt) = m_lPolicyId


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimRiskId) = PMNavKeyConst.PMKeyNameClaimRiskID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimRiskId) = m_lRisk


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowPartyCnt) = PMNavKeyConst.PMKeyNamePartyCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPartyCnt) = m_lPartyCnt

                '****************
                ' MEvans : 28-05-2003 : 223

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowRiskTypeId) = PMKeyNameRiskTypeID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowRiskTypeId) = m_lRiskTypeId
                '****************

                ' RVH 24/12/2004 - Add screen caption key...used to override caption on iPMURisk...

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowScreenCaption) = PMNavKeyConst.PMKeyNameScreenCaption
                'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowScreenCaption) = m_sScreenCaption
                'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)



                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowNoTransactions) = PMKeyNameNoTransaction

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowNoTransactions) = m_bOpenClaimNoTrans


                m_lReturn = objClaimBuilder.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed when calling SetKeys on iPMURisk.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_sTransactionType = "C_CR" And m_lClaimMode = 0 Then

                    m_lReturn = objClaimBuilder.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vTransactionType:=m_sTransactionType)
                Else

                    m_lReturn = objClaimBuilder.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed when calling SetProcessModes on iPMURisk.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' process object

                objClaimBuilder.Start()

                ' terminate object and free resources

                If objClaimBuilder.Status <> gPMConstants.PMEReturnCode.PMOK Then

                    ' cancel roadmap
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                    ' only delete the claim when not in view mode
                    If m_iTask <> gPMConstants.PMEComponentAction.PMView AndAlso Not objClaimBuilder.ReserveLimitExceeded AndAlso Not m_bReserveLimitExceeded Then
                        ' Tidy up - delete work table entries, unlock claim...now moved to business component so can
                        ' be called from other components...

                        m_lReturn = oBusiness.TidyUpAfterCancel(v_lClaimId:=m_lclaimid, v_lClaimMode:=m_lClaimMode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed when calling TidyUpAfterCancel", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    ElseIf objClaimBuilder.ReserveLimitExceeded Then

                        oBusiness.ClaimId = m_lclaimid
                        If Not m_bReserveLimitExceeded Then
                            If m_vKeyArray IsNot Nothing AndAlso Information.IsArray(m_vKeyArray) AndAlso m_vKeyArray.GetUpperBound(0) > 0 Then
                                ReDim Preserve m_vKeyArray(m_vKeyArray.GetUpperBound(0), m_vKeyArray.GetUpperBound(1) + 1)
                                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vKeyArray.GetUpperBound(1)) = "is_reserve_limit_exceeded"
                                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vKeyArray.GetUpperBound(1)) = "True"
                            End If
                        End If
                        m_lReturn = oBusiness.CreateReserveLimitFailureTasknEvent(v_vKeyArray:=m_vKeyArray, v_dExceededReserve:=objClaimBuilder.ExceededReserve)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log the event and create work manager task.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                If m_bReserveLimitExceeded And Not objClaimBuilder.ReserveLimitExceeded Then
                    m_bIsReserveUpdatednTaskCompleted = True
                End If

                m_sClaimNumber = CStr(vOrigClaimData(ACClaim_Number, 0))


                objClaimBuilder.Dispose()
                objClaimBuilder = Nothing
                oBusiness.Dispose()
                oBusiness = Nothing


            Else
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
        Dim bPrevInfoOnlyStatus As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode


        'Developer guide No. 50
        frmInterface = New frmInterface
        result = gPMConstants.PMEReturnCode.PMTrue


        If m_sTransactionType <> "C_CO" Then
            bPrevInfoOnlyStatus = frmInterface.InfoOnlyStatus(m_lclaimid)

            If bPrevInfoOnlyStatus Then
                m_iTask = gPMConstants.PMEComponentAction.PMAdd
                m_sTransactionType = "C_CO"
            End If
        End If

        If (m_sTransactionType = "C_CP") And m_bFurtherPayments Then

            lReturn = ReloadClaimDetails()


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If



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

        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Claimid = m_lclaimid
            .Risk = m_lRisk
            .RiskType = m_lRisk
            .Policy = m_lPolicyId
            .ViewRiskFlag = m_bViewRiskFlag
            .ClaimMode = m_lClaimMode
            .DeleteWorkTableFlag = m_lDeleteWorkTableFlag
            .LanguageID = g_oObjectManager.LanguageID
            .IsOpenClaimNoTrans = m_bOpenClaimNoTrans
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
            .ScreenCaption = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
        End With

        ' Load the instance of the interface into memory.

        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
        If frmInterface.Text.IndexOf("["c) >= 0 Then 'PN_69381
            frmInterface.Text = Mid(frmInterface.Text, 1, frmInterface.Text.IndexOf("["c)) & " " & m_sScreenCaption
        End If 'PN_69381
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.4)
        m_sClaimNumber = frmInterface.ClaimNumber
        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status
            m_cThisPayment = .ThisPayment
            m_lThisPaymentCurrencyID = .ThisPaymentCurrencyID

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

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
        VB6.ShowForm(frmInterface, lDisplayState)
        'Added the following line to set the claim number
        m_sClaimNumber = frmInterface.ClaimNumber

        If frmInterface.Status <> gPMConstants.PMEReturnCode.PMOK Then

            ' cancel roadmap
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' only delete the claim when not in view mode
            If m_iTask <> gPMConstants.PMEComponentAction.PMView AndAlso Not frmInterface.ReserveLimitExceeded AndAlso Not m_bReserveLimitExceeded Then
                ' Tidy up - delete work table entries, unlock claim...now moved to business component so can
                ' be called from other components...

                m_lReturn = oBusiness.TidyUpAfterCancel(v_lClaimId:=m_lclaimid, v_lClaimMode:=m_lClaimMode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed when calling TidyUpAfterCancel", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            ElseIf frmInterface.ReserveLimitExceeded Then

                oBusiness.ClaimId = m_lclaimid
                If Not m_bReserveLimitExceeded Then
                    If m_vKeyArray IsNot Nothing AndAlso Information.IsArray(m_vKeyArray) AndAlso m_vKeyArray.GetUpperBound(0) > 0 Then
                        ReDim Preserve m_vKeyArray(m_vKeyArray.GetUpperBound(0), m_vKeyArray.GetUpperBound(1) + 1)
                        m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, m_vKeyArray.GetUpperBound(1)) = "is_reserve_limit_exceeded"
                        m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, m_vKeyArray.GetUpperBound(1)) = "True"
                    End If
                End If
                m_lReturn = oBusiness.CreateReserveLimitFailureTasknEvent(v_vKeyArray:=m_vKeyArray, v_dExceededReserve:=frmInterface.ExceededReserve)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log the event and create work manager task.", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If m_bReserveLimitExceeded And Not frmInterface.ReserveLimitExceeded Then
            m_bIsReserveUpdatednTaskCompleted = True
        End If


        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
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


    ' ***************************************************************** '
    ' Name: ReloadClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description: Reload the claim details and re-lock the claim so
    '               no other updates can take place...
    '
    ' History:
    '           Created : MEvans : 26-11-2004 : PN17073
    ' ***************************************************************** '
    Public Function ReloadClaimDetails() As Integer
        Dim result As Integer = 0
        Dim bCLMFindClaim As Object

        Const kMethodName As String = "ReloadClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oClaimStatus As bCLMFindClaim.Business
        Dim lClaimId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' lock claim details
            lReturn = LockClaim()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("lockClaim", "Failed to lock claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create instance of bCLMFindClaim.Business
            Dim temp_oClaimStatus As Object
            lReturn = g_oObjectManager.GetInstance(temp_oClaimStatus, "bCLMFindClaim.Business", vInstanceManager:="ClientManager")
            oClaimStatus = temp_oClaimStatus
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bCLMFindClaim.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oClaimStatus.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMFindCLaim.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' reload the claim into the work tables

            lReturn = oClaimStatus.ProcessCopyClaim(v_lClaimId:=m_lclaimid, r_lCopyClaimId:=lClaimId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bCLMFindClaim.Business.CopyClaimToWork Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lclaimid = lClaimId


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oClaimStatus = Nothing

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    '**********************************************************************
    ' Function Name:    ClaimBuilderIsEnable
    ' Author:           Alix Bergeret
    ' Date:             17/10/2001
    ' Description:      Check if SIROPTClaimsBuilder product option is ON
    '**********************************************************************
    Private Function ClaimBuilderIsEnable() As Boolean

        Dim result As Boolean = False
        'developer guide no.98
        Dim vResult As Object

        Try

            result = True
            'developer guide no.98
            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=g_oObjectManager.SourceID, r_vUnderwriting:=vResult)


            Return vResult = 1

        Catch
        End Try



        Return False

    End Function

    ' ***************************************************************** '
    ' Name: LockClaim
    '
    ' Parameters: n/a
    '
    ' Description: Lock the specified claim using the pmlock method
    '
    ' History:
    '           Created : MEvans : 29-11-2004 : PN17073
    ' ***************************************************************** '
    Public Function LockClaim() As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object

        Const kMethodName As String = "LockClaim"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oPMLock As bPMLock.User
        Dim sLockedBy As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get instance of bpmlock.user
            Dim temp_oPMLock As Object
            lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_objectManger.GetInstance", "Failed to get instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' lock claim record based on the claim id

            lReturn = oPMLock.LockKey(sKeyName:=PMNavKeyConst.PMKeyNameRealClaimID, vKeyValue:=m_lOriginalClaimId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)

            ' check the return code
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                ' to determine if an error occurred or
                ' if the record is locked by someone else
                If sLockedBy = "ERROR" Then
                    gPMFunctions.RaiseError("LockKey", "Error trying to lock record", gPMConstants.PMELogLevel.PMLogError)
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse

                    MessageBox.Show("Claim currently locked by " & sLockedBy & _
                                Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Find Claim")

                    Return result
                End If

            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oPMLock = Nothing

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UnlockClaim
    ' Description:
    ' History: 22/01/2003 - Alix
    ' ***************************************************************** '

    'Private Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
    'Dim result As Integer = 0
    'Dim bPMLock As Object
    '

    'Dim oPMLock As bPMLock.User
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Get bPMLock
    'Dim temp_oPMLock As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oPMLock = temp_oPMLock
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    'Return result
    'End If
    '
    'If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

    'm_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)
    'End If
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End If
    '
    'oPMLock = Nothing
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result


    'Return result
    'End Try
    'End Function
End Class

