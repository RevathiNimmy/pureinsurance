Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
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

    Public objfrmInterface As frmInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 30/07/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:Pandu-Added Keys Required For Integration into Road Map
    '
    ' Ram 17-11-2000 : Task / Process Ref : Internal Process 004.
    '                  Claims Detail Tab.doc
    '
    ' JMK(28/02/2001): Add Claim Payment mode, allows limited editing. Assigned to g_nPMMode
    '
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.1)
    Private m_sScreenCaption As String = ""
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.1)

    Private m_sPolicyHolder As String = ""

    Private m_lNavigate As Integer

    Private m_lProcessMode As Integer

    Private m_sTransactionType As String = ""

    Private m_dtEffectiveDate As Date

    Private m_nOpenClaim As Integer

    Private m_lAddressCnt As Integer

    Private m_nMaintain As Integer

    Private m_nPayment As Integer

    Private m_sAddress1 As String = ""
    Private m_sAddress2 As String = ""
    Private m_sAddress3 As String = ""
    Private m_sAddress4 As String = ""
    Private m_sPostalCode As String = ""
    Private m_sReference As String = ""
    Private m_sPostCode As String = ""

    ' JMK 16/05/2001 pass claim number to roadmap
    Private m_sClaimNumber As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Stores FSA compliance product option.
    Private m_vFSAComplianceValue As String = ""
    'Plico 24/28
    Private m_lBaseCaseID As Integer

    'WR5
    Private m_lWorkflowId As Integer
    Private m_bCheck_Unpaid_Status As Boolean
    Private m_bSalvage_Recovery As Boolean
    Private m_bThird_Party_Recovery As Boolean
    Private m_bExternal_Claim_Handling As Boolean
    Private m_bDescription_for_Change As Boolean
    Private m_bClaim_Doc_Message As Boolean
    Private m_bGenerate_Claim_Doc As Boolean
    Private m_bClaim_Payment_Process As Boolean
    Private m_bCheck_Deferred_Reinsurance As Boolean
    Private m_bCash_Payment_Process As Boolean
    Private m_bMake_Further_Payments As Boolean
    Private m_bFastTrackEnabled As Boolean
    Private m_iClaimPaymentValid As Integer
    Private m_bUserAuthRunCashPayment As Boolean
    Private m_bUserAuthRunClaimPayment As Boolean
    'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.1.1)
    Private m_bShowPaymentView As Boolean
    'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.1.1)
    Private m_bShowAllClaimVersionEvents As Boolean
    'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.1.1)

    Public Property ShowPaymentView() As Boolean
        Get

            Return m_bShowPaymentView

        End Get
        Set(ByVal Value As Boolean)

            m_bShowPaymentView = Value

        End Set
    End Property
    'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.1.1)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

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

    Public Property OpenClaim() As Integer
        Get

            Return m_nOpenClaim

        End Get
        Set(ByVal Value As Integer)

            m_nOpenClaim = Value

        End Set
    End Property

    Public WriteOnly Property AddressCnt() As Integer
        Set(ByVal Value As Integer)

            m_lAddressCnt = Value

        End Set
    End Property




    Public Property Maintain() As Integer
        Get

            Return m_nMaintain

        End Get
        Set(ByVal Value As Integer)

            m_nMaintain = Value

        End Set
    End Property




    Public Property Payment() As Integer
        Get

            Return m_nPayment

        End Get
        Set(ByVal Value As Integer)

            m_nPayment = Value

        End Set
    End Property
    Public Property ShowAllClaimVersionEvents() As Boolean
        Get

            Return m_bShowAllClaimVersionEvents

        End Get
        Set(ByVal Value As Boolean)

            m_bShowAllClaimVersionEvents = Value

        End Set
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
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.-in main module
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
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.

            '*****Start of Code Change Internal Bug 2 changed property of instance manager**********************
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bOpenClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness
            '*****end  of Code Change Internal Bug 2 **********************

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
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
            'developer guide no.(Done to reassign values to the public members of the main module)
            'start add
            g_lClaimID = 0
            g_sClaimNo = ""
            'end add
            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                'developer guide no.248
                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameOperateMode


                        g_nPMMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePolicyID

                        g_lPolicyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        g_lOrPolicyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyNumber

                        g_sPolicyNo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimID


                        g_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimNumber


                        g_sClaimNo = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameClaimDate


                        g_vClaimDate = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePolicyHolder, PMNavKeyConst.PMKeyNameClaimPolicyHolder 'MKR 14/09/2004 PN 11258 for showing the client name in the title bar...


                        m_sPolicyHolder = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'DC031104 PN14948
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        g_lPartyCnt = CInt(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case "base_case_id"

                        m_lBaseCaseID = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClaimWorkflowId

                        m_lWorkflowId = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                End Select

            Next lRow

            'DC081203 -PN8955 -when task created to go back and go to open claim at claim details
            'level, roadmap does not redisplay info as thinks it is still adding
            'a new claim.  EDITADDMODE will allow details to be amended, even though
            'continuing in add mode
            If g_nPMMode = gPMConstants.PMEComponentAction.PMAdd And g_lClaimID <> 0 Then
                g_nPMMode = g_nEDITADDMODE
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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the key array with the number of keys needed to be returned.
            ' Note: Remember arrays are zero based.
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.2)
            ReDim vKeyArray(1, 29)
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.2)

            ' Note: Claims stores risk_cnt in risk_type_id !!!
            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameRiskTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = g_lRiskTypeID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = g_lClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameClaimDate

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = g_vClaimDate


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameOperateMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = g_nPMMode


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNamePolicyID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = g_lPolicyID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNamePolicyNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = g_sPolicyNo


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameClientHolder

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = g_sClientName


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameClaimNumber

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = g_sClaimNo


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = g_lPolicyID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = g_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameBalanceAndCloseClaim

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = g_bBalanceAndCloseClaim


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameDisplayClaimReinsurance

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = g_bDisplayClaimReinsurance


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNameDisplayCheckUnpaidStatus

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_bCheck_Unpaid_Status


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameDisplaySalvageRecovery

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = m_bSalvage_Recovery


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameDisplayThirdPartyRecovery

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = m_bThird_Party_Recovery


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyNameDisplayExternalClaimHandling

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = m_bExternal_Claim_Handling


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.PMKeyNameDisplayDescriptionForChange

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = m_bDescription_for_Change


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNameDisplayClaimDocMessage

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = m_bClaim_Doc_Message


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 18) = PMNavKeyConst.PMKeyNameGenerateClaimDocument

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 18) = m_bGenerate_Claim_Doc


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 19) = PMNavKeyConst.PMKeyNameDisplayClaimPaymentProcess

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 19) = m_bClaim_Payment_Process


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 20) = PMNavKeyConst.PMKeyNameCheckDeferredReinsurance

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 20) = m_bCheck_Deferred_Reinsurance


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 21) = PMNavKeyConst.PMKeyNameDisplayCashPaymentProcess

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 21) = m_bCash_Payment_Process


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 22) = PMNavKeyConst.PMKeyNameDisplayMakeFurtherPayments

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 22) = m_bMake_Further_Payments


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 23) = PMNavKeyConst.PMKeyNameClaimPaymentValid

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 23) = m_iClaimPaymentValid


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 24) = PMNavKeyConst.PMKeyNameFastTrackClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 24) = m_bFastTrackEnabled


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 25) = PMNavKeyConst.PMKeyNameClaimWorkflowId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 25) = m_lWorkflowId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 26) = PMNavKeyConst.PMKeyNameUserAuthRunCashPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 26) = m_bUserAuthRunCashPayment


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 27) = PMNavKeyConst.PMKeyNameUserAuthRunClaimPayment

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 27) = m_bUserAuthRunClaimPayment
            'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.2)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 28) = PMNavKeyConst.PMKeyNameScreenCaption

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 28) = m_sScreenCaption
            'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.2)
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 29) = PMNavKeyConst.PMKeyNameShowAllClaimVersionEvents
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 29) = m_bShowAllClaimVersionEvents
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

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.
            'vSummaryArray(PMNavSummHeading, 0) = "Claim Reference"

            'JMK 16/05/2001 get the Claim Reference (to display in Roadmap)

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "claim_ref"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sClaimNumber

            ''    vSummaryArray(PMNavSummValue, 0) = m_sClaimRef$

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
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get FSA product option value

            '2005 Client Manager Security
            m_lReturn = CType(CheckSecurity(r_bEditClaimAuthority:=g_bEditClaimAuthority), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not g_bEditClaimAuthority Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If
            '2005 Client Manager Security

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
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
    ' Description: Gets the ID for the search parameter from the
    '              business object.
    '
    ' ***************************************************************** '
    Public Function GetID() As Integer

        Dim result As Integer = 0

        Try


            ' Get the ID from the business object.
            ' TF021298 - This function does not exist on Business
            '    m_lReturn& = g_oBusiness.GetID( _
            ''        vSearch:=CVar(m_sClaimReference$), _
            ''        vID:=CVar(m_lClaimCnt&))

            ' Check for errors
            '    If (m_lReturn& = PMFalse Or _
            ''    m_lReturn& = PMError) Then
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to get the ID from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetID"
            '    End If

            ' Return the value.
            '    GetID = m_lReturn&

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CheckSecurity (Standard Method)
    '
    ' Description: Check whether the user has authority to view clients
    ' History:     2005 Client Security  20/04/2005
    '
    ' ***************************************************************** '
    Private Function CheckSecurity(ByRef r_bEditClaimAuthority As Boolean) As Integer

        Dim result As Integer = 0


        Dim sValue As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue



        r_bEditClaimAuthority = True

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

        m_lReturn = SetupClaimWorkflow()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = GetUserAuthoritytoRunCashListPaymentTask()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = GetUserAuthoritytoRunClaimPaymentTask()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    If m_bFastTrackEnabled = False Or m_sTransactionType <> "C_CP" Then
        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        '# Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.

        If m_bFastTrackEnabled And m_sTransactionType = "C_CP" And (m_lWorkflowId = 1 Or m_lWorkflowId = 2) Then
            ' Donot Show Interface

        Else
            m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)
        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If
        'End If

        m_iClaimPaymentValid = objfrmInterface.ClaimPaymentValid
        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)
        If m_bFastTrackEnabled And m_sTransactionType = "C_CP" And (m_lWorkflowId >= 1 And m_lWorkflowId <= 3) And m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
            m_lStatus = gPMConstants.PMEReturnCode.PMNavAction1
        End If
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
        Dim sClaimTitle As String = ""


        objfrmInterface = New frmInterface
        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACClaim As String = "Claim Details"

        ' Assign the parameters to the interface properties.

        With objfrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Task = m_iTask

            'developer guide no. 24
            .FSAComplianceFlag = m_vFSAComplianceValue
            .BaseCaseID = m_lBaseCaseID
            .WorkflowId = m_lWorkflowId
            'Start - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.1.2)
            .ShowPaymentView = m_bShowPaymentView
            'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.2.1.2)
        End With

        ' Load the instance of the interface into memory.
        'Commented as tempLoadForm declared but never used
        'Dim tempLoadForm As frmInterface = frmInterface
        objfrmInterface.frmInterfaceLoad()

        ' Check if we have had an error so far.

        If objfrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.

            result = objfrmInterface.ErrorNumber

            m_iClaimPaymentValid = objfrmInterface.ClaimPaymentValid
        Else


            Select Case g_nPMMode
                Case g_nADDMODE
                    'Set the caption according to the mode

                    objfrmInterface.Text = ACClaim & " [ " & g_sClientName & " ] " & g_sPolicyNo

                    'RWH(09/11/2000) Claims numbering.

                    'RWH(10/11/2000) Always prov. claim for Add.
                    sClaimTitle = "Prov. Claim No. "

                    If CBool(CStr(g_sClaimNo = "").Trim()) Then
                        'If no claim no. is generated we leave blank.
                        'Default value persisted to database is handled by stored proc.
                        'spu_Claim_add.

                        objfrmInterface.Text = objfrmInterface.Text & New String(" "c, 2) & sClaimTitle
                    Else

                        objfrmInterface.Text = objfrmInterface.Text & New String(" "c, 2) & sClaimTitle & " " & g_sClaimNo
                    End If


                    'JMK(28/02/2001) include Claim Payment mode
                    'DC020403 ISS3153 added view mode
                Case g_nEDITMODE, g_nREADMODE, g_nPAYMODE, g_nVIEWMODE

                    'RWH(09/11/2000) Claims numbering.

                    If g_nInfoOnly = 1 Then
                        sClaimTitle = "Prov. Claim No. "
                    Else
                        sClaimTitle = "Claim No. "
                    End If
                    'RWH(10/11/2000) Updated to display Policy AND claim no.
                    'Start(Sriram P)S4I_Bug_Report_1.14.xls (LOA003-CLAIMS ClaimNo Display)

                    objfrmInterface.Text = ACClaim & " [ " & g_sClientName & " ] " & g_sPolicyNo & New String(" "c, 2) & sClaimTitle & " " & g_sClaimNo
                    'End(Sriram P)S4I_Bug_Report_1.14.xls (LOA003-CLAIMS ClaimNo Display)

            End Select

            ' Ram 17-11-2000    ******* Start
            ' Task / Process Ref : Internal Process 004. Claims Detail Tab.doc


            objfrmInterface.lblReportedToDate.Visible = False

            objfrmInterface.txtOpenClaim(10).Visible = False

            objfrmInterface.txtOpenClaim(11).Visible = False
            ' Ram 17-11-2000    ******* End

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

        With objfrmInterface
            m_lStatus = .Status
            'JMK 16/05/2001 pass claim number to roadmap
            m_sClaimNumber = .ClaimNo
            'DC031104 PN14948
            g_lPartyCnt = .PartyCnt
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.

        objfrmInterface.Close()

        objfrmInterface = Nothing

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

        VB6.ShowForm(objfrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.

            If objfrmInterface.ErrorNumber <> 0 Then

                result = objfrmInterface.ErrorNumber
            End If
        End If
        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.1)
        m_sScreenCaption = " [ " & g_sClientName.Trim() & " ] " & New String(" "c, 2) & g_sPolicyNo.Trim() & New String(" "c, 2) & (IIf(g_nInfoOnly, "Prov. Claim No. ", "Claim No. ")) & " " & g_sClaimNo
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.1.1.1)
        Return result

    End Function

    Private Function SetupClaimWorkflow() As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "SetupClaimWorkflow"

        Dim lReturn, lProductId As Integer

        Dim oProductBusiness As bSIRProduct.Business
        Dim vResults As Object
        ''Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.5.1)
        Dim vAuthorityValue As Object
        ''End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.5.1)

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetInsuranceFile(g_lPolicyID, vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vResults) Then
                'store claims productId

                lProductId = gPMFunctions.ToSafeLong(CStr(vResults(ACInsFileProductId, 0)))

                If lProductId > 0 Then
                    If m_lWorkflowId = 0 Then
                        'set workflowId if not already set to push into roadmap
                        If m_sTransactionType = "C_CO" Then
                            m_lWorkflowId = gPMConstants.PMWorkflowOpenClaim
                        ElseIf m_sTransactionType = "C_CR" Then
                            m_lWorkflowId = gPMConstants.PMWorkflowMaintainClaim
                        ElseIf m_sTransactionType = "C_CP" Then
                            m_lWorkflowId = gPMConstants.PMWorkflowPayClaim
                        Else
                            'View Claim
                            m_bCheck_Unpaid_Status = True
                            m_bSalvage_Recovery = True
                            m_bThird_Party_Recovery = True
                            m_bExternal_Claim_Handling = True
                            m_bDescription_for_Change = True
                            g_bDisplayClaimReinsurance = True
                            Return result
                        End If
                    End If
                Else
                    gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim temp_oProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProductBusiness = temp_oProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = oProductBusiness.GetClaimWorkflow(r_vResults:=vResults, v_lProductID:=lProductId, v_lWorkflowID:=m_lWorkflowId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vResults) Then
                gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
            Else

                m_bFastTrackEnabled = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EFast_Track_Claims, 0)))

                m_bCheck_Unpaid_Status = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status, 0)))

                m_bSalvage_Recovery = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.ESalvage_Recovery, 0)))

                m_bThird_Party_Recovery = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery, 0)))

                m_bExternal_Claim_Handling = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EExternal_Claim_Handling, 0)))

                m_bClaim_Payment_Process = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process, 0)))

                m_bCheck_Deferred_Reinsurance = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.ECheck_Deferred_Reinsurance, 0)))

                m_bCash_Payment_Process = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.ECash_Payment_process, 0)))

                m_bMake_Further_Payments = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EMake_Further_Payments, 0)))

                If m_sTransactionType = "C_CP" Then

                    g_bDisplayClaimReinsurance = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EReinsurance_Payment, 0)))

                    m_bDescription_for_Change = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment, 0)))

                    m_bClaim_Doc_Message = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message, 0)))

                    m_bGenerate_Claim_Doc = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc, 0)))
                Else

                    g_bDisplayClaimReinsurance = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery, 0)))

                    m_bDescription_for_Change = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Reserve, 0)))

                    m_bClaim_Doc_Message = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message, 0)))

                    m_bGenerate_Claim_Doc = gPMFunctions.ToSafeBoolean(CStr(vResults(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc, 0)))
                End If
            End If
            'Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.5.1)
            If g_bDisplayClaimReinsurance Then


                lReturn = g_oBusiness.GetSpecificUserAuthority(v_vAuthority:="display_claim_reinsurance", r_vAuthorityValue:=vAuthorityValue)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupClaimWorkflow Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                g_bDisplayClaimReinsurance = Not (gPMFunctions.ToSafeInteger(vAuthorityValue) = 0)

            End If
            'End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.5.1)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            If Not (oProductBusiness Is Nothing) Then
                ' terminate this instance of the navigator process

                oProductBusiness.Dispose()
                oProductBusiness = Nothing

            End If

        End Try
        Return result
    End Function

    Private Function GetUserAuthoritytoRunCashListPaymentTask() As Integer
        Dim result As Integer = 0
        Dim bPMTask As Object

        Const kMethodName As String = "GetUserAuthoritytoRunCashListPaymentTask"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lProductId As Integer

        Dim oBusiness As bPMTask.Business
        Dim vResults As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oBusiness As Object
        lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMTask.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oBusiness = temp_oBusiness
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInstance of bPMTask.Business Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        lReturn = oBusiness.GetUserAuthorityToRunTask(r_vResults:=vResults, v_iUserID:=g_oObjectManager.UserID, v_sTaskCode:="ACTPAYV2")
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetUserAuthorityToRunTask Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_bUserAuthRunCashPayment = Information.IsArray(vResults)

        Return result
    End Function

    Private Function GetUserAuthoritytoRunClaimPaymentTask() As Integer
        Dim result As Integer = 0
        Dim bPMTask As Object

        Const kMethodName As String = "GetUserAuthoritytoRunClaimPaymentTask"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lProductId As Integer

        Dim oBusiness As bPMTask.Business
        Dim vResults As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oBusiness As Object
        lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMTask.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oBusiness = temp_oBusiness
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInstance of bPMTask.Business Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        lReturn = oBusiness.GetUserAuthorityToRunTask(r_vResults:=vResults, v_iUserID:=g_oObjectManager.UserID, v_sTaskCode:="PAYCLM")
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetUserAuthorityToRunTask Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_bUserAuthRunClaimPayment = Information.IsArray(vResults)

        Return result
    End Function



    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()
        'developer guide no.301
        g_sClientTelNo = ""
        g_sClientTelNoOff = ""
        g_sClientFaxNo = ""
        g_sClientMobileNo = ""
        g_sClientEMail = ""
        g_sClientClaimNo = ""
        g_sInsurerTelNo = ""
        g_sInsurerFaxNo = ""
        g_sInsurerEmail = ""
        g_sInsurerContact = ""
        g_sInsurerClaimNo = ""
        g_sVATRegisteredNo = ""
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

End Class

