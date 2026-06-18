Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'developer guide no.129 
Imports SSP.Shared

Imports System.Transactions
Imports System.Threading

<System.Runtime.InteropServices.ProgId("AutoMta_NET.AutoMta")>
Public NotInheritable Class AutoMta
    '*************************************************************************
    ' History
    ' RAW 19/09/2003 : CQ2614 : Ensure that LeadCommission is updated when processing Agent Commission
    ' RAW 26/09/2003 : CQ828 : added more details to reinstated risk event description
    ' RAW 13/11/2003 : CQ1765 : pass OriginalInsuranceFileCnt & RunMode to bControlTrans object
    ' RAW 24/11/2003 : CQ685 : allow NilPremiumRefund to be set for multiple insurance file cnts
    ' RAW 20/01/2004 : CQ3535 : use new date as the end date when auto cancelling a policy version and keep the original start date
    ' RAW 18/02/2004 : CQ3665 : get the InsuranceFileCnt of the policy version that was originally cancelled during a policy reinstatement
    ' RAW 08/03/2004 : CQ4180 : fix bug when getting original insurance file cnt
    ' RAW 28/04/2004 : CQ5143 : clear GIS output before quoting
    '                           set CalledFromAutoMTA property for renewals
    ' RAW 05/05/2004 : CQ753 : include risk tax when calculating policy premium
    ' TR  17/08/2004 : RESILIENCE changes - Removed all controls passed to this and added transactional support
    ' RAW 15/11/2003 : Pricing changes : pass original rating details to NBQuote
    ' ************************************************'*************************************************************************
    Private Const ACClass As String = "AutoMTA"

    'Reasons for replacing a policy version
    Private Const ACReasonBackdatedCancellation As String = "REPLACED: Backdated cancellation "
    Private Const ACReasonBackdatedMTA As String = "REPLACED: Backdated MTA "
    Private Const ACReasonBackdatedReinstatement As String = "REPLACED: Backdated Reinstatement "
    Private Const ACReasonBackdatedRiskReinstatement As String = "REPLACED: Backdated Risk Reinstatement "
    Private Const ACReasonReplaced As String = "REPLACED: "

    'Newly created policy versions
    Private Const ACAutoMTAInsFileCnt As Integer = 0
    Private Const ACAutoMTATransType As Integer = 1
    Private Const ACAutoMTAVersion As Integer = 2
    Private Const ACAutoMTAArraySize As Integer = 2

    'WPR 33-75 added
    Private Const ACBDMTAInsFileCnt As Integer = 0
    Private Const ACBDMTATransTypeId As Integer = 1
    Private Const ACBDMTAVersion As Integer = 2
    ''
    Private Const ACAffectedInsuranceFileCnt As Integer = 0
    Private Const ACAffectedCoverStartDate As Integer = 1
    Private Const ACAffectedPolicyVersion As Integer = 2
    Private Const ACAffectedInsFileType As Integer = 3
    Private Const ACAffectedExpiryDate As Integer = 4
    Private Const ACAffectedInsFileStatus As Integer = 5
    Private Const ACAffectedArraySize As Integer = 5

    'Declines, Referrals and Messages
    Private Const ACDRInsuranceFileCnt As Integer = 0
    Private Const ACDRRiskCnt As Integer = 1
    Private Const ACDRType As Integer = 2 'Decline/Refer or Message
    Private Const ACDRReason As Integer = 3
    Private Const ACDRArraySize As Integer = 3

    Private Const ACDRTypeCodeDecline As String = "Decline"
    Private Const ACDRTypeCodeRefer As String = "Refer"
    Private Const ACDRTypeCodeMessage As String = "Message"

    Private Const ACOPObjectName As Integer = 0
    Private Const ACOPPropertyName As Integer = 1
    Private Const ACOPValue As Integer = 2

    'Array for storing list of risks for merging MTA changes
    Private Const ACRPreChangeRiskCnt As Integer = 0
    Private Const ACRPostChangeRiskCnt As Integer = 1
    Private Const ACRCurrentRiskCnt As Integer = 2
    Private Const ACRRiskStatusFlag As Integer = 3
    Private Const ACRRiskNo As Integer = 4
    Private Const ACRStatus As Integer = 5
    Private Const ACRSize As Integer = 5

    Private m_lReturn As Integer

    ' Added to replace global variables 26/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    'Objects
    Private m_oReinsurance As Object
    Private m_oRITax As bSIRRITax.Business
    Private m_oAgentCommission As BSirAgentCommission.Business
    Private m_oFindInsurance As Object
    Private m_oChangePolicyStatus As bSIRChangePolicyStatus.Business
    Private m_oFindRisk As bSIRFindRisk.Form
    Private m_oListRisks As bSIRListRisks.Business
    Private m_oRiskData As bSIRRiskData.Business
    Private m_oRenSelection As bSIRRenSelection.Business
    Private m_oPerilAllocation As bSirPerilAllocation.Business
    Private m_oControlTrans As bControlTrans.Automated
    Private m_oAutomaticRenewalsSel As bSIRAutomaticRenewalsSel.Business
    Private m_oAutomaticRenewalsAccept As bSIRAutomaticRenewalsAccept.Business
    Private m_oAutoMTAMerge As AutoMTAMerge
    Private m_oGIS As bGIS.Application
    Private m_oDataset As cGISDataSetControl.Application
    Private m_oInsuranceFile As bSIRInsuranceFile.Business
    Private m_oPremiumFinance As Object
    'WPR 33-75 added
    'Private m_oDatabase As Object
    Private m_oDatabase As dPMDAO.Database
    Private m_oDocumentReversal As bACTAllocationPost.Automated
    Private m_oSIRPartyFee As bSIRPartyFee.UBusiness

    Private m_vAutoMTAInsFileCnts(,) As Object
    Private m_vAffectedInsuranceFileCnts As Object ' AffectedInsuranceFileCnts
    Private m_lInsuranceFolderCnt As Integer ' InsuranceFolderCnt
    Private m_sTransactionType As String = "" ' TransactionType
    Private m_dtEffectiveDate As Date ' EffectiveDate
    Private m_vDeclineReasons As Object ' DeclineReasons
    Private m_vReferReasons As Object ' ReferReasons
    Private m_vMessages(,) As Object ' Messages
    Private m_vPolicyStatusMessages(,) As Object ' Policy status messages
    Private m_lLastDeletedRiskCnt As Integer
    Private m_lLastDeletedInsuranceFileCnt As Integer
    Private m_sEventReason As String = ""
    Private m_bUpdateStats As Boolean ' UpdateStats
    Private m_lNewInsuranceFileCnt As Integer ' NewInsuranceFileCnt
    Private bIsSingleInstalmentForCCProcess As Boolean
    'developer guide no. 17
    Private m_vObjectPropertyArray As Object ' ObjectPropertyArray
    Private m_dtCurrentDate As Date ' CurrentDate
    Private m_lDeletedRiskInsuranceFileCnt As Integer ' DeletedRiskInsuranceFileCnt
    Private m_lDeletedRiskCnt As Integer ' DeletedRiskCnt
    Private m_bIsReinstateRisk As Boolean ' IsReinstateRisk
    Private m_sReinstatementReason As String = "" ' ReinstatementReason
    Private m_lPartyCnt As Integer ' PartyCnt
    Private m_bMergeRisks As Boolean ' MergeRisks
    Private m_sNCDReason As String = "" ' NCDReason
    Private m_bIsNCDChange As Boolean ' IsNCDChange
    Private m_bIsPolicyReinstatement As Boolean
    Private m_lRunMode As Integer ' RAW 13/11/2003 : CQ1765 : added
    Private m_oRenewal As bSIRRenewal.Business
    Private m_bBackDateMTA As Boolean ' BackDateMTA
    Private m_bIsSingleInstalmentPlan As Boolean
    ''Start(Saurabh Agrawal) Out of sequence Bug fixing
    Private m_bRI2007Enabled As Boolean
    ''End(Saurabh Agrawal) Out of sequence Bug fixing

    ' HG261103 - Private reference of object manager - COM+ Changes
    Private m_oObjectManager As Object

    Private m_lBDInsuranceFileCnt As Integer

    ''WPR 33-75 added
    Private m_bIsPostStatsForLapsedPolicy As Boolean
    Private m_bIsInteractive As Boolean

    Private m_bCalledFromPlanMaintainence As Boolean
    ' HG261103 - Private reference of object manager - COM+ Changes
    Public WriteOnly Property ObjectManagerRef() As Object
        Set(ByVal Value As Object)
            m_oObjectManager = Value
        End Set
    End Property
    Public WriteOnly Property IsNCDChange() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsNCDChange = Value
        End Set
    End Property
    Public WriteOnly Property NCDReason() As String
        Set(ByVal Value As String)
            m_sNCDReason = Value
        End Set
    End Property
    Public WriteOnly Property MergeRisks() As Boolean
        Set(ByVal Value As Boolean)
            m_bMergeRisks = Value
        End Set
    End Property
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public WriteOnly Property ReinstatementReason() As String
        Set(ByVal Value As String)
            m_sReinstatementReason = Value
        End Set
    End Property
    Public WriteOnly Property IsReinstateRisk() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsReinstateRisk = Value
        End Set
    End Property
    Public WriteOnly Property DeletedRiskInsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lDeletedRiskInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property DeletedRiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_lDeletedRiskCnt = Value
        End Set
    End Property
    Public WriteOnly Property CurrentDate() As Date
        Set(ByVal Value As Date)
            m_dtCurrentDate = Value
        End Set
    End Property
    Public WriteOnly Property ObjectPropertyArray() As String()
        Set(ByVal Value As String())
            m_vObjectPropertyArray = Value
        End Set
    End Property
    Public WriteOnly Property IsSingleInstalmentPlan() As Boolean
        Set(ByVal bIsSingleInstalmentPlan As Boolean)
            m_bIsSingleInstalmentPlan = bIsSingleInstalmentPlan
        End Set
    End Property
    Public ReadOnly Property MultipleVersionsExist() As Boolean
        Get
            Dim result As Boolean = False

            If Not Informations.IsArray(m_vAutoMTAInsFileCnts) Then
                'Rebuild the internal arrays from the mta_insurance_file_link table
                m_lReturn = RebuildArrays()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildArrays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                    Return result
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return result
                End If
            End If

            If Informations.IsArray(m_vAutoMTAInsFileCnts) Then
                If m_vAutoMTAInsFileCnts.GetUpperBound(1) > 0 Then
                    result = True
                End If
            End If

            Return result
        End Get
    End Property

    Public WriteOnly Property IsSingleInstalmentForCCProcess() As Boolean
        Set(ByVal Value As Boolean)
            bIsSingleInstalmentForCCProcess = Value
        End Set
    End Property

    Public Property NewInsuranceFileCnt() As Integer
        Get
            Return m_lNewInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lNewInsuranceFileCnt = Value
        End Set
    End Property
    Public WriteOnly Property UpdateStats() As Boolean
        Set(ByVal Value As Boolean)
            m_bUpdateStats = Value
        End Set
    End Property
    Public ReadOnly Property ReferReasons() As Object
        Get
            Return m_vReferReasons
        End Get
    End Property
    Public ReadOnly Property DeclineReasons() As Object
        Get
            Return m_vDeclineReasons
        End Get
    End Property
    Public ReadOnly Property Messages() As String()
        Get
            Return m_vMessages.Clone
        End Get
    End Property
    Public ReadOnly Property PolicyStatusMessages() As String()
        Get
            Return m_vPolicyStatusMessages.Clone
        End Get
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    Public Property AffectedInsuranceFileCnts() As Array
        Get
            Return m_vAffectedInsuranceFileCnts
        End Get
        Set(ByVal Value As Array)
            m_vAffectedInsuranceFileCnts = Value
        End Set
    End Property
    ' RAW 13/11/2003 : CQ1765 : added
    Public WriteOnly Property RunMode() As Integer
        Set(ByVal Value As Integer)
            m_lRunMode = Value
        End Set
    End Property
    Public WriteOnly Property BackDateMTA() As Boolean
        Set(ByVal Value As Boolean)
            m_bBackDateMTA = Value
        End Set
    End Property

    'WPR 33-75 added
    Public WriteOnly Property IsPostStatsForLapsedPolicy() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsPostStatsForLapsedPolicy = Value
        End Set
    End Property

    'WPR 33-75 added
    Public WriteOnly Property IsInteractive() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsInteractive = Value
        End Set
    End Property

    Public Property IsCalledFromPlanMaintainence() As Boolean
        Get
            Return m_bCalledFromPlanMaintainence
        End Get
        Set(ByVal Value As Boolean)
            m_bCalledFromPlanMaintainence = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' AUTOMATIC BACKDATED CANCELLATIONS
    ' ***************************************************************** '

    '*************************************************************************
    ' Name:         AutoCancelMTA
    ' Description:  Automatically cancel a policy
    ' History:      08/01/2003 sj - Created.
    '               16/08/2004 TR - RESILIENCE changes
    '*************************************************************************
    ' 'WPR 33-75 added
    Public Function AutoCancelMTA(ByRef r_sErrorText As String, Optional ByVal lBaseInsuranceFileCnt As Integer = 0, Optional ByRef v_bIsDirty As Boolean = False, Optional ByVal bUpdateStats As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
        Dim bBackdatingRequired, bTransStarted As Boolean

        Try
            If bUpdateStats = False Then
                m_vAutoMTAInsFileCnts = Nothing
            End If
            result = gPMConstants.PMEReturnCode.PMTrue

            m_sEventReason = ACReasonBackdatedCancellation

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, v_bIsCancellation:=True, r_bBackdatingRequired:=bBackdatingRequired)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lErrorCode <> 0 Then
                'We have no valid policy version for this date
                Select Case lErrorCode
                    Case 1
                        r_sErrorText = "Future adjustment found"
                    Case 2
                        r_sErrorText = "Overlaps with temporary MTA"
                    Case 3
                        r_sErrorText = "No version found"
                End Select

                Return gPMConstants.PMEReturnCode.PMError
            End If

            'RESILIENCE - now start a transaction
            'Wrap in transaction to make sure all Updates are completed successfully or none are
            m_lReturn = m_oDatabase.SQLBeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                bTransStarted = True
            End If

            'RESILIENCE - This is where most updates are done
            'Loop around the policy versions cancelling each one
            'WPR 33-75 added
            If m_bUpdateStats = False Or m_bBackDateMTA = False Then
                m_lReturn = AutoCancelPolicyVersions(v_lVersion:=lPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate, v_lNewMTAInsuranceFileCnt:=lBaseInsuranceFileCnt, v_bIsDirty:=v_bIsDirty)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Roll back this transaction
                    m_oDatabase.SQLRollbackTrans()
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                    result = gPMConstants.PMEReturnCode.PMFalse

                    'Delete any policy versions we have created and set the status
                    ' of the originals back to live
                    m_lReturn = RestoreAutoRunMTA()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                    End If
                    Return result

                End If

                'WPR 33-75 added
            Else
            End If
            'WPR 33-75 added below line is commented
            If m_bUpdateStats Then

                'refresh versions to transact
                If Not Informations.IsArray(m_vAutoMTAInsFileCnts) Then
                    m_lReturn = GetAffectedBackDatedMTAVersions()
                End If
                'Update the accounts
                m_lReturn = TransactPolicyVersions()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_oDatabase.SQLRollbackTrans()
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' 'WPR 33-75 added
                'WPR 33
                'Update IsDirty flage =False
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="BaseInsuranceFileCnt", vValue:=lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_sTransactionType = "MTC" Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Transaction_Type", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' Execute the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateIsDirtyForBackDatedVersionsSQL, sSQLName:=ACUpdateIsDirtyForBackDatedVersionsName, bStoredProcedure:=ACUpdateIsDirtyForBackDatedVersionsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'End WPR 33

            'If we got here then everything must be OK, return set at top
            m_oDatabase.SQLCommitTrans()

            Return result

        Catch excep As System.Exception


            If bTransStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name:         AutoCancelPolicyVersions
    ' Description:
    ' History:      02/01/2003 sj - Created.
    '*************************************************************************
    Public Function AutoCancelPolicyVersions(ByVal v_lVersion As Integer, ByVal v_dtEffectiveDate As Date, Optional ByVal v_lNewMTAInsuranceFileCnt As Integer = 0, Optional ByRef v_bIsDirty As Object = False) As Integer

        Dim result As Integer = 0
        Const klInsuranceFileStatusId_Cancelled As Integer = 1 ' RAW 20/01/2004 : CQ3535 : added

        Dim dtMTADate As Date
        Dim lNewInsuranceFileCnt As Integer
        Dim dtMTAEndDate As Date
        Dim sTransactionType As String = ""
        Dim bCopyPolicy, bFirst As Boolean
        Dim lFirstNewInsuranceFileCnt As Integer
        Dim bIsRealCancellation As Boolean
        Dim vPremiumFinance As Object
        Dim bMTAApplied As Boolean
        Const kFinancePlanStatusIND As Integer = 64
        Const kFinancePlanOnHoldStatus As String = "140"
        Const kFinancePlanLiveStatus As String = "040"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_vDeclineReasons = ""

            m_vReferReasons = ""
            bFirst = True


            'Get the list of affected policy versions in ascending order
            m_lReturn = ReverseArray(r_vArray:=m_vAffectedInsuranceFileCnts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' RAW 20/01/2004 : CQ3535 : added
            ' Remove entries from the end of the array that are already cancelled so that
            ' we can detect the last 'valid' version within the next loop.
            ' Those at the start or in the middle of the array will be left as they can be detected within the loop.
            ' We could have excluded these from the orignal sql query but were unsure about changing a 'core' function and didn't have time to dig any further
            For i As Integer = m_vAffectedInsuranceFileCnts.GetUpperBound(1) To 0 Step -1
                If gPMFunctions.ToSafeDouble(m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i)) = klInsuranceFileStatusId_Cancelled Then
                    If i <> 0 Then
                        m_vAffectedInsuranceFileCnts = ArraysHelper.RedimPreserve(Of Object(,))(m_vAffectedInsuranceFileCnts, New Integer() {m_vAffectedInsuranceFileCnts.GetUpperBound(0) - m_vAffectedInsuranceFileCnts.GetLowerBound(0) + 1, i - 1 - m_vAffectedInsuranceFileCnts.GetLowerBound(1) + 1}, New Integer() {m_vAffectedInsuranceFileCnts.GetLowerBound(0), m_vAffectedInsuranceFileCnts.GetLowerBound(1)})
                    End If
                Else
                    Exit For
                End If
            Next

            Dim lLatestLivePolicyCnt As Integer = 0

            If Not m_bBackDateMTA Then
                For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)
                    If m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i) = v_lVersion Then
                        dtMTAEndDate = CDate(m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i))
                        lLatestLivePolicyCnt = m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)

                        Exit For
                    End If
                Next
            End If

            For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)
                bCopyPolicy = True
                bIsRealCancellation = False

                ' RAW 20/01/2004 : CQ3535 : added test for cancelled versions
                If gPMFunctions.ToSafeDouble(m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i)) = klInsuranceFileStatusId_Cancelled Then

                Else
                    If m_sTransactionType = "MTC" Then
                        'WPR 33-75 added
                        If m_bBackDateMTA = True Then
                            dtMTADate = m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)
                            dtMTAEndDate = CDate(m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i))
                        Else
                            dtMTADate = v_dtEffectiveDate 'm_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)
                        End If

                    Else
                        dtMTADate = v_dtEffectiveDate
                        dtMTAEndDate = CDate(m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i))
                    End If


                    If bFirst Then

                        'This is a real cancellation rather than just reversing out the
                        'existing premiums
                        bIsRealCancellation = True
                        'this to ensure that when m_lNewInsuranceFileCnt is Base Insurancefile cnt then we copy policy
                        If m_lNewInsuranceFileCnt <> 0 AndAlso m_lNewInsuranceFileCnt <> v_lNewMTAInsuranceFileCnt Then
                            ' We have already copied the first policy
                            bCopyPolicy = False
                        End If
                    End If

                    If i = m_vAffectedInsuranceFileCnts.GetUpperBound(1) Then

                        ' This is the last version (ie newest one)

                        If m_lNewInsuranceFileCnt > 0 Then
                            'We are going through the cancellation road map so we do not want to
                            'set all versions to cancelled at this stage
                            sTransactionType = "MTCA"
                        Else
                            sTransactionType = "MTC"
                        End If
                    Else
                        sTransactionType = "MTCA"
                    End If

                    'WPR 33-75 added
                    'PN - 2401
                    If (i > 0 And m_bBackDateMTA = True) Or (i = 0 And m_bBackDateMTA = False) Then

                        If lLatestLivePolicyCnt = 0 Then
                            lLatestLivePolicyCnt = ToSafeInteger(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i))
                        End If

                        If m_bIsSingleInstalmentPlan = False Then
                            'If i > 0 Then
                            ' Cancel any associated instalment Plan first

                            m_lReturn = m_oPremiumFinance.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=ToSafeInteger(lLatestLivePolicyCnt), r_vPFPremiumFinance:=vPremiumFinance)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleFinancePlanFromInsFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If Informations.IsArray(vPremiumFinance) Then
                                m_lReturn = m_oPremiumFinance.CancelPlanInHouse(vPremiumFinance:=vPremiumFinance, dRefund:=0)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleFinancePlanFromInsFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        End If
                        'Remove the policy from Renewal
                        m_lReturn = DeletePolicyFromRenewal(v_lInsuranceFileCnt:=lLatestLivePolicyCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicyFromRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' RAW 20/01/2004 : CQ3535 : added v_vMTAEndDate param
                        'WPR 33-75 added
                        m_lReturn = AutoRunCancellation(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), r_lVersion:=v_lVersion, v_dtMTAStartDate:=dtMTADate, v_vMTAEndDate:=dtMTAEndDate, v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_sTransactionType:=sTransactionType, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_bCopyPolicy:=bCopyPolicy, v_bIsRealCancellation:=bIsRealCancellation, v_bIsBackdatedMTA:=m_bBackDateMTA)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        NewInsuranceFileCnt = lNewInsuranceFileCnt
                    ElseIf (m_sCallingAppName = "iACTCreditControlProcessing" OrElse m_sCallingAppName = "CreditControlCLI" OrElse m_sCallingAppName = "SiriusImport") Then
                        If m_bIsSingleInstalmentPlan = False Then
                            'If i > 0 Then
                            ' Cancel any associated instalment Plan first
                            m_lReturn = m_oPremiumFinance.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), r_vPFPremiumFinance:=vPremiumFinance)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleFinancePlanFromInsFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If Informations.IsArray(vPremiumFinance) AndAlso (vPremiumFinance(kFinancePlanStatusIND, 0) = kFinancePlanOnHoldStatus OrElse vPremiumFinance(kFinancePlanStatusIND, 0) = kFinancePlanLiveStatus) Then
                                m_lReturn = m_oPremiumFinance.CancelPlanInHouse(vPremiumFinance:=vPremiumFinance, dRefund:=0)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleFinancePlanFromInsFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            End If
                        End If
                        ' RAW 20/01/2004 : CQ3535 : end
                    End If

                    If bFirst Then
                        'This is the first one
                        m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink(v_lInsuranceFileCnt:=ToSafeInteger(NewInsuranceFileCnt), bIsDirty:=v_bIsDirty)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        lFirstNewInsuranceFileCnt = NewInsuranceFileCnt
                        If v_lNewMTAInsuranceFileCnt > 0 Then
                            m_lReturn = UpdateBaseInsuranceFile(v_lNewMTAInsuranceFileCnt, NewInsuranceFileCnt)
                        End If
                        bFirst = False
                    Else
                        If ToSafeLong(v_lNewMTAInsuranceFileCnt) > 0 Then
                            m_lReturn = UpdateInsuranceFileReplaced(NewInsuranceFileCnt, 1)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AutoCancelPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            If v_lNewMTAInsuranceFileCnt > 0 Then
                                m_lReturn = UpdateBaseInsuranceFile(v_lNewMTAInsuranceFileCnt, NewInsuranceFileCnt)
                            End If
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AutoCancelPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

                    'WPR 33-75 added  Commented as deleted for WPR 33-75

                    'Update the mta_insurance_file_link table
                    m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vCancelledLinkedInsuranceFileCnt:=lNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                        Return result
                    End If
                    'End If

                    If bMTAApplied Then
                        'Update the mta_insurance_file_link table with the new insurance_file_cnt
                        m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vNewLinkedInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return result
                        End If
                        bMTAApplied = False
                    End If
                End If
            Next i



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'PN-71588 (Nitesh Dwivedi as on 07-05-2010)
    Private Function UpdateCurrencyToInsuranceFile(ByVal lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCurrencyToInsuranceFile"
        Dim sFailMsg As String = ""

        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim lAccountID, lSourceID As Integer
        Dim iTransactionCurrencyID As Integer
        Dim cTransactionAmount As Decimal
        Dim iBaseCurrencyID As Integer
        Dim cBaseCurrentAmount As Decimal
        Dim iAccountCurrencyID As Integer
        Dim cAccountCurrentAmount As Decimal
        Dim iSystemCurrencyID As Integer
        Dim cSystemCurrentAmount As Decimal
        Dim dTransToBaseExchangeRate As Double
        Dim dtEffectiveDateOfExchange As Date
        Dim dAccountToBaseExchangeRate, dSystemToBaseExchangeRate As Double
        Dim iRateOverrideReasonID As Integer
        Dim m_vTemp(,) As Object


        result = gPMConstants.PMEReturnCode.PMTrue

        oCurrencyConvert = New bACTCurrencyConvert.Form
        If oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("InsfileCnt", lNewInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyInfoSQL, sSQLName:=ACGetPolicyInfo, bStoredProcedure:=False, vResultArray:=m_vTemp)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'iTransactionCurrencyID = m_oInsuranceFile.CurrencyID
        'lSourceID = m_oInsuranceFile.SourceID


        If Informations.IsArray(m_vTemp) Then

            lSourceID = CInt(m_vTemp(0, 0))

            iTransactionCurrencyID = CInt(m_vTemp(1, 0))
        End If


        If Convert.IsDBNull(dtEffectiveDateOfExchange) Or Informations.IsNothing(dtEffectiveDateOfExchange) Or dtEffectiveDateOfExchange = CDate("00:00:00") Then
            dtEffectiveDateOfExchange = DateTime.Today
        End If

        m_lReturn = oCurrencyConvert.DoCurrencyConversion(v_lAccountID:=lAccountID, v_lCompanyId:=lSourceID, v_iCurrencyID:=iTransactionCurrencyID, v_cCurrencyAmountUnrounded:=cTransactionAmount, r_iBaseCurrencyID:=iBaseCurrencyID, r_cBaseAmount:=cBaseCurrentAmount, r_iAccountCurrencyID:=iAccountCurrencyID, r_cAccountAmount:=cAccountCurrentAmount, r_iSystemCurrencyID:=iSystemCurrencyID, r_cSystemAmount:=cSystemCurrentAmount, r_dCurrencyBaseXrate:=dTransToBaseExchangeRate, r_dtCurrencyBaseDate:=dtEffectiveDateOfExchange, r_dAccountBaseXrate:=dAccountToBaseExchangeRate, r_dtAccountBaseDate:=dtEffectiveDateOfExchange, r_dSystemBaseXrate:=dSystemToBaseExchangeRate, r_dtSystemBaseDate:=dtEffectiveDateOfExchange)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oCurrencyConvert.Dispose()
            oCurrencyConvert = Nothing
            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End If

        'A zero here means that the exchange rates have not been set up
        If dTransToBaseExchangeRate = 0 Or dSystemToBaseExchangeRate = 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oCurrencyConvert.Dispose()
            oCurrencyConvert = Nothing
            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End If

        m_lReturn = oCurrencyConvert.UpdateInsuranceFile(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_dCurrencyBaseXrate:=dTransToBaseExchangeRate, v_dtCurrencyBaseDate:=dtEffectiveDateOfExchange, v_dAccountBaseXrate:=0, v_dtAccountBaseDate:=DateTime.MinValue, v_dSystemBaseXrate:=dSystemToBaseExchangeRate, v_dtSystemBaseDate:=dtEffectiveDateOfExchange, v_lRateOverrideReasonID:=iRateOverrideReasonID, v_iBaseCurrencyID:=iBaseCurrencyID, v_iAccountCurrencyID:=0)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sFailMsg = "Failed to update Insurance File Data"
            oCurrencyConvert.Dispose()
            oCurrencyConvert = Nothing
            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End If

        oCurrencyConvert.Dispose()
        oCurrencyConvert = Nothing

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AutoRunCancellation
    '
    ' Description:
    '
    ' History: 06/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AutoRunCancellation(ByVal v_lBaseInsuranceFileCnt As Integer, ByRef r_lVersion As Integer, ByVal v_dtMTAStartDate As Date, ByVal v_sTransactionType As String, ByVal v_lExistingPolicyVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, Optional ByVal v_vMTAEndDate As Object = Nothing, Optional ByVal v_bCopyPolicy As Boolean = True, Optional ByVal v_bIsRealCancellation As Boolean = False, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer


        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        Dim nNewInsuranceFileCnt As Integer
        Dim sTransactionType As String = ""
        Dim bCopyDeletedLink As Boolean
        Dim nCnt As Integer
        Dim vOptionValue As Object = ""
        Dim bCopyRiskOnMTA As Boolean = False
        'Make a copy of the policy
        If v_bCopyPolicy Then
            bCopyDeletedLink = Not v_bIsRealCancellation
            m_lReturn = CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate, v_vMTAEndDate:=v_vMTAEndDate, v_bCopyDeletedLink:=bCopyDeletedLink, v_bCancellation:=v_bIsRealCancellation, v_sTransactionType:=v_sTransactionType, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            nNewInsuranceFileCnt = m_lNewInsuranceFileCnt
        End If

        r_lNewInsuranceFileCnt = nNewInsuranceFileCnt

        'Keep a record of all the new policies we are creating
        If Informations.IsArray(m_vAutoMTAInsFileCnts) Then
            nCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, nCnt)
        Else
            nCnt = 0
            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, nCnt)
        End If
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, nCnt) = nNewInsuranceFileCnt
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, nCnt) = "MTC"
        m_vAutoMTAInsFileCnts(ACAutoMTAVersion, nCnt) = r_lVersion + 1

        If v_bIsRealCancellation Then
            sTransactionType = "MTC"
        Else
            sTransactionType = "MTCR"
        End If

        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, v_vBranch:=1, r_vUnderwriting:=vOptionValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If vOptionValue = "1" AndAlso sTransactionType = "MTCR" Then
            bCopyRiskOnMTA = True
        End If
        'Quote all the risks
        If m_sTransactionType = "MTC" OrElse bCopyRiskOnMTA Then
            m_lReturn = ProcessRisks(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:=sTransactionType, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, v_bCopyRiskOnMTA:=bCopyRiskOnMTA)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'Update the policy status
        'WPR 33-75 added
        If sTransactionType = "MTCR" Then
            m_lReturn = ProcessChangePolicyStatus(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:=sTransactionType, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)
        Else
            m_lReturn = ProcessChangePolicyStatus(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:=v_sTransactionType)
        End If
        'PN-71588 (Nitesh Dwivedi as on 07-05-2010)--Start
        m_lReturn = UpdateCurrencyToInsuranceFile(nNewInsuranceFileCnt)
        'PN-71588 (Nitesh Dwivedi as on 07-05-2010)--End

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_sTransactionType = "MTC" Then
            'Policy level tax
            If sTransactionType <> "MTCR" Then
                m_lReturn = ProcessRITax(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_lRiskCnt:=0, sTransactionType:=sTransactionType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If
        'WPR 33-75 added
        If m_sTransactionType = "MTC" And v_bIsBackdatedMTA Then
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=v_lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_agent_commission_rev", sSQLName:="Reverse Comm", bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If
            m_lReturn = m_oDatabase.SQLAction("Update insurance_file Set risk_processed = 1 Where insurance_file_cnt = " & nNewInsuranceFileCnt, "Update processed", False)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AutoRunCancellation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            ' reverse fee
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=v_lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee", bStoredProcedure:=True)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction("Update insurance_file Set risk_processed = 1 Where insurance_file_cnt = " & nNewInsuranceFileCnt, "Update processed", False)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AutoRunCancellation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        If m_sTransactionType = "MTA" Then
            'Set the status of the new policy version to cancelled
            'Only do this for cancelled versions created during the MTA process, real
            'cancellations will have all there policy versions set to cancelled when the
            'policy is made live
            m_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sInsuranceFileStatusCode:="CAN")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return nResult
            End If
            'WPR 33-75 added Else replaced with ElseIf
            'Else
        ElseIf m_bUpdateStats = True Then
            'Update the insurance file type to cancelled when this is a real reinstateable
            'cancellation
            m_lReturn = UpdateInsuranceFileType(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sInsuranceFileTypeCode:="MTACAN")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return nResult
            End If
        End If

        'Increment the version number
        r_lVersion += 1

        Return nResult

    End Function
    ''' <summary>
    ''' AutoReinstateMTA
    ''' </summary>
    ''' <param name="sErrorText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function AutoReinstateMTA(ByRef sErrorText As String,
                          Optional ByVal nBaseInsuranceFileCnt As Integer = 0,
                          Optional ByVal bIsDirty As Boolean = False) As Integer

        Return AutoReinstateMTA(sErrorText:=sErrorText, v_bIsBackdatedMTA:=False,
                                nBaseInsuranceFileCnt:=nBaseInsuranceFileCnt, bIsDirty:=bIsDirty)

    End Function
    ''' <summary>
    ''' AutoReinstateMTA
    ''' </summary>
    ''' <param name="sErrorText"></param>
    ''' <param name="nBaseInsuranceFileCnt"></param>
    ''' <param name="bIsDirty"></param>
    ''' <param name="v_bIsBackdatedMTA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function AutoReinstateMTA(ByRef sErrorText As String, ByVal v_bIsBackdatedMTA As Boolean,
                              Optional ByVal nBaseInsuranceFileCnt As Integer = 0,
                              Optional ByVal bIsDirty As Boolean = False) As Integer


        Dim nInsuranceFileCnt As Integer
        Dim nPolicyVersion As Integer
        Dim nErrorCode As Integer
        Dim bBackdatingRequired As Boolean
        Dim bTransStarted As Boolean
        Dim sMessage As String

        Dim nResult As Integer = 0


        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            AutoReinstateMTA = gPMConstants.PMEReturnCode.PMTrue

            m_sEventReason = ACReasonBackdatedReinstatement

            nInsuranceFileCnt = nBaseInsuranceFileCnt

            If m_bUpdateStats = False Then
                'Get a list of all the policy versions which need to be processed and store
                'them in the array - m_vAffectedInsuranceFileCnts
                m_lReturn = GetVersionByDate(r_lInsuranceFileCnt:=nInsuranceFileCnt,
                         v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=nPolicyVersion,
                         r_lErrorCode:=nErrorCode, v_bIsReinstatement:=True,
                         r_bBackdatingRequired:=bBackdatingRequired)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AutoReinstateMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA")
                    Return nResult
                End If

                If (nErrorCode <> 0) Then
                    'We have no valid policy version for this date
                    Select Case nErrorCode
                        Case 1
                            sErrorText = "Future adjustment found"
                        Case 2
                            sErrorText = "Overlaps with temporary MTA"
                        Case 3
                            sErrorText = "No version found"
                    End Select

                    nResult = gPMConstants.PMEReturnCode.PMError
                    Exit Function
                End If
            End If

            'TR - Start a transaction here
            m_lReturn = m_oDatabase.SQLBeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AutoReinstateMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA")
                Return nResult
            Else
                bTransStarted = True
            End If

            If m_bUpdateStats = False Then
                m_lReturn = AutoRunReinstatement(nVersion:=nPolicyVersion, v_bBackDateMTA:=v_bIsBackdatedMTA)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_oDatabase.SQLRollbackTrans = gPMConstants.PMEReturnCode.PMTrue Then
                        bTransStarted = False
                    End If
                    'Do not goto error handler as there is more processing to do
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                                 "AutoRunReinstatement Failed", vApp:=ACApp, vClass:=ACClass,
                                 vMethod:="AutoReinstateMTA")
                    AutoReinstateMTA = gPMConstants.PMEReturnCode.PMFalse

                    'Delete any policy versions we have created and set the status
                    ' of the originals back to live
                    m_lReturn = RestoreAutoRunMTA()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "RestoreAutoRunMTA Failed"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    Exit Function
                End If
            Else
                'Update the accounts
                m_lReturn = TransactPolicyVersions()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    sMessage = "TransactPolicyVersions Failed"
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="BaseInsuranceFileCnt",
                                                       vValue:=nBaseInsuranceFileCnt,
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                       iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    AutoReinstateMTA = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                ' Execute the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateIsDirtyForBackDatedVersionsSQL,
                                                  sSQLName:=ACUpdateIsDirtyForBackDatedVersionsName,
                                                  bStoredProcedure:=ACUpdateIsDirtyForBackDatedVersionsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    AutoReinstateMTA = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If


            'If we got here then everything must be OK, return set at top
            m_oDatabase.SQLCommitTrans()

            Exit Function
        Catch ex As Exception
            'Is this a genuine error or did we get here because a call failed?
            'If a call failed, then we will have set a message string
            If Len(sMessage) > 0 Then
                'Ok so we're here because m_lReturn came back not PMTrue
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA", excep:=ex)
            Else
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=
                             "AutoReinstateMTA Failed", vApp:=ACApp, vClass:=ACClass,
                             vMethod:="AutoBackdatedMTA", vErrNo:=Informations.Err.Number,
                                   vErrDesc:=Informations.Err.Description, excep:=ex)
            End If
            'either way - roll back the transaction if we got that far
            If bTransStarted = True Then
                m_oDatabase.SQLRollbackTrans()
            End If
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' AutoRunReinstatement
    ''' </summary>
    ''' <param name="nVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoRunReinstatement(ByVal nVersion As Integer) As Integer

        Return AutoRunReinstatement(nVersion:=nVersion,
                                 v_bBackDateMTA:=False)

    End Function
    ''' <summary>
    ''' AutoRunReinstatement
    ''' </summary>
    ''' <param name="nVersion"></param>
    ''' <param name="v_bBackDateMTA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoRunReinstatement(ByVal nVersion As Integer, ByVal v_bBackDateMTA As Boolean) As Integer

        Dim i As Integer
        Dim dtMTADate As Object
        Dim nNewInsuranceFileCnt As Integer
        Dim bCopyPolicy As Boolean
        Dim bFirst As Boolean
        Dim nFirstNewInsuranceFileCnt As Integer
        Dim nOriginalLinkedInsuranceFileCnt As Object
        Dim bIsRealReinstatement As Boolean
        Dim bRenewals As Object
        Dim nResult As Integer = 0

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_vDeclineReasons = ""
            m_vReferReasons = ""
            bFirst = True

            m_bIsPolicyReinstatement = True

            'Get the list of affected policy versions in ascending order
            m_lReturn = ReverseArray(r_vArray:=m_vAffectedInsuranceFileCnts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                            sMsg:="ReverseArray Failed", vApp:=ACApp,
                            vClass:=ACClass, vMethod:="AutoRunReinstatement")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'Loop around the affected policy versions
            For i = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)
                bCopyPolicy = True
                If bFirst = False Then
                    'Get the original version that was cancelled
                    m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(
                                 v_lNewLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i),
                                 r_lOriginalLinkedInsuranceFileCnt:=nOriginalLinkedInsuranceFileCnt,
                                 r_dtExpiryDate:=dtMTADate, v_bLookForCancelled:=True, r_bRenewals:=bRenewals)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername,
                                         iType:=gPMConstants.PMEReturnCode.PMError,
                                         sMsg:="m_oFindInsurance.GetOriginalLinkedVersion Failed",
                                         vApp:=ACApp,
                                         vClass:=ACClass,
                                         vMethod:="AutoRunReinstatement")
                        Return nResult
                    End If

                    If bRenewals = True Then
                        'We need to create a renewal version
                        m_lReturn = AutoRunRenewalsForReinstatement(
                                             nBaseInsuranceFileCnt:=nFirstNewInsuranceFileCnt,
                                             nInsuranceFileCnt:=nOriginalLinkedInsuranceFileCnt,
                                             nNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                             nAffectedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername,
                                                      iType:=gPMConstants.PMEReturnCode.PMError,
                                                      sMsg:="AutoRunRenewalsForReinstatement Failed",
                                                      vApp:=ACApp,
                                                      vClass:=ACClass,
                                                      vMethod:="AutoRunReinstatement")
                            Return nResult
                        End If
                    Else
                        m_lReturn = AutoRunSingleReinstatement(
                                    nBaseInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i),
                                    nVersion:=nVersion,
                                    dtMTAStartDate:=m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i),
                                    nNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                    oMTAEndDate:=m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i),
                                    bCopyPolicy:=bCopyPolicy,
                                    bIsRealReinstatement:=bIsRealReinstatement)


                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername,
                                           iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="AutoRunCancellation Failed",
                                           vApp:=ACApp,
                                           vClass:=ACClass,
                                           vMethod:="AutoRunReinstatement")
                            Return nResult
                        End If
                    End If

                    m_lReturn = UpdateCurrencyToInsuranceFile(nNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername,
                                       iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="UpdateCurrencyToInsuranceFile Failed",
                                       vApp:=ACApp,
                                       vClass:=ACClass,
                                       vMethod:="AutoRunReinstatement")
                        Return nResult
                    End If

                    ' replace original version
                    m_lReturn = UpdateInsuranceFileReplaced(nNewInsuranceFileCnt, 1)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername,
                                       iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="UpdateInsuranceFileReplaced Failed",
                                       vApp:=ACApp,
                                       vClass:=ACClass,
                                       vMethod:="AutoRunReinstatement")
                        Return nResult
                    End If

                    'replace original version
                    m_lReturn = UpdateBaseInsuranceFile(m_lNewInsuranceFileCnt, nNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername,
                                       iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="UpdateBaseInsuranceFile Failed",
                                       vApp:=ACApp,
                                       vClass:=ACClass,
                                       vMethod:="AutoRunReinstatement")
                        Return nResult
                    End If

                    m_lReturn = UpdateInsuranceFileDetailsOOSReinstate(m_lNewInsuranceFileCnt, nNewInsuranceFileCnt, bRenewals, v_bBackDateMTA)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername,
                                       iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="UpdateInsuranceFileDetailsOOSReinstate Failed",
                                       vApp:=ACApp,
                                       vClass:=ACClass,
                                       vMethod:="AutoRunReinstatement")
                        Return nResult
                    End If

                    m_lReturn = m_oDatabase.SQLAction("Update insurance_file Set risk_processed = 1 Where insurance_file_cnt = " & nNewInsuranceFileCnt, "Update processed", False)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If
                Else
                    'This is the first one
                    'Create the mta_insurance_file_link table
                    m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink(
                                     v_lInsuranceFileCnt:=ToSafeInteger(m_lNewInsuranceFileCnt), bIsDirty:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername,
                                             iType:=gPMConstants.PMEReturnCode.PMError,
                                             sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed",
                                             vApp:=ACApp,
                                             vClass:=ACClass,
                                             vMethod:="AutoRunReinstatement")
                        Return nResult
                    End If
                    nFirstNewInsuranceFileCnt = m_lNewInsuranceFileCnt

                    ' replace original version
                    m_lReturn = UpdateBaseInsuranceFile(m_lNewInsuranceFileCnt, m_lNewInsuranceFileCnt)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Return nResult
                    End If

                    bFirst = False
                End If

                'Update the mta_insurance_file_link table
                m_lReturn = UpdateMTAInsuranceFileLink(
                             v_iType:=3,
                             v_vInsuranceFileCnt:=nFirstNewInsuranceFileCnt,
                             v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i),
                             v_vNewLinkedInsuranceFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername,
                                     iType:=gPMConstants.PMEReturnCode.PMError,
                                     sMsg:="UpdateMTAInsuranceFileLink Failed",
                                     vApp:=ACApp,
                                     vClass:=ACClass,
                                     vMethod:="AutoRunReinstatement")
                    Return nResult
                End If

            Next i

            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return nResult

        End Try


    End Function

    ' ***************************************************************** '
    ' Name: AutoRunSingleReinstatement
    '
    ' Description:
    '
    ' History: 06/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AutoRunSingleReinstatement) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Function AutoRunSingleReinstatement(
                         ByVal nBaseInsuranceFileCnt As Integer,
                         ByRef nVersion As Integer,
                         ByVal dtMTAStartDate As Date,
                         ByRef nNewInsuranceFileCnt As Integer,
                         Optional ByVal oMTAEndDate As Object = Nothing,
                         Optional ByVal bCopyPolicy As Boolean = True,
                         Optional ByVal bIsRealReinstatement As Boolean = True) As Integer


        Dim nResult = gPMConstants.PMEReturnCode.PMFalse
        Dim nCnt As Long
        Dim nNewInsuranceFileCnt1 As Long
        Dim sTransactionType As String




        nResult = gPMConstants.PMEReturnCode.PMTrue
        'Make a copy of the policy
        If bCopyPolicy = True Then
            m_lReturn = CopyPolicy(
                        v_lOldInsuranceFileCnt:=nBaseInsuranceFileCnt,
                        r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt1,
                        v_lVersion:=nVersion,
                        v_bPermanentMTA:=True,
                        v_dtMTADate:=dtMTAStartDate,
                        v_vMTAEndDate:=oMTAEndDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername,
                           iType:=gPMConstants.PMELogLevel.PMLogOnError,
                           sMsg:="AutoRunSingleReinstatement Failed",
                           vApp:=ACApp,
                           vClass:=ACClass,
                           vMethod:="AutoRunSingleReinstatement")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            ' RAW 03/02/2004 : moved from end of function
            'Increment the version number
            nVersion = nVersion + 1

        Else
            nNewInsuranceFileCnt1 = m_lNewInsuranceFileCnt
        End If

        nNewInsuranceFileCnt = nNewInsuranceFileCnt1

        'Keep a record of all the new policies we are creating
        If Informations.IsArray(m_vAutoMTAInsFileCnts) = True Then
            nCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, nCnt)
        Else
            nCnt = 0
            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, nCnt)
        End If
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, nCnt) = nNewInsuranceFileCnt1
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, nCnt) = "MTA"
        ' RAW 03/02/2004 : removed + 1 since now done earlier
        m_vAutoMTAInsFileCnts(ACAutoMTAVersion, nCnt) = nVersion

        If bIsRealReinstatement = False Then
            sTransactionType = "MTCR"
        Else
            sTransactionType = "MTR"
        End If

        'Quote all the risks
        m_lReturn = ProcessRisks(
                    v_lInsuranceFileCnt:=nNewInsuranceFileCnt1,
                    v_sTransactionType:=sTransactionType,
                    v_lBaseInsuranceFileCnt:=nBaseInsuranceFileCnt, v_bIsBackdatedMTA:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername,
                       iType:=gPMConstants.PMELogLevel.PMLogOnError,
                       sMsg:="ProcessRisks Failed",
                       vApp:=ACApp,
                       vClass:=ACClass,
                       vMethod:="AutoRunSingleReinstatement")
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Return nResult
        End If

        'Policy level tax
        m_lReturn = ProcessRITax(
                    v_lInsuranceFileCnt:=nNewInsuranceFileCnt1,
                    v_lRiskCnt:=0)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername,
                       iType:=gPMConstants.PMELogLevel.PMLogOnError,
                       sMsg:="ProcessRITax Failed",
                       vApp:=ACApp,
                       vClass:=ACClass,
                       vMethod:="AutoRunSingleReinstatement")
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Return nResult
        End If

        Return nResult


    End Function

    ''' <summary>
    ''' It is used for copy the Policy associates from prior version.
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt">Previous Version cnt</param>
    ''' <param name="nNewInsuranceFileCnt">Current Vesrion Cnt</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyPolicyAssociates(ByVal nOldInsuranceFileCnt As Integer, ByVal nNewInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = 0

        nResult = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oDatabase.Parameters.Clear()
            nResult = m_oDatabase.Parameters.Add(sName:="nOld_Insurance_File_Cnt",
                                                    vValue:=nOldInsuranceFileCnt,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="nNew_Insurance_File_Cnt",
                                                    vValue:=nNewInsuranceFileCnt,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Copy the Policy Associates
            Return m_oDatabase.SQLAction(sSQL:=ACCopyPolicyAssociatesSQL,
                                         sSQLName:=ACCopyPolicyAssociatesName,
                                         bStoredProcedure:=True)

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyAssociates Failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates", vErrNo:=Informations.Err().Number,
                                   vErrDesc:=ex.Message, excep:=ex)
            Return nResult

        End Try


    End Function


    ''' <summary>
    ''' AutoBackdatedMTA
    ''' </summary>
    ''' <param name="r_sErrorText"></param>
    ''' <param name="lBaseInsuranceFileCnt"></param>
    ''' <param name="r_bShowQuoteMsg"></param>
    ''' <param name="v_bIsDirty"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoBackdatedMTA(ByRef r_sErrorText As String, ByVal lBaseInsuranceFileCnt As Integer, Optional ByRef r_bShowQuoteMsg As Boolean = False, Optional ByRef v_bIsDirty As Boolean = False) As Integer

        Dim nResult As Integer = 0
        Dim nInsuranceFileCnt As Integer
        Dim nPolicyVersion As Integer
        Dim nErrorCode As Integer
        Dim bBackdatingRequired, bTransStarted As Boolean
        Dim sMessage As String = ""

        Dim sInsuranceFileTypeCode As Object = ""


        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'See if we already have already created multiple backdated Mta
            'policy versions for this quote
            If Not m_bUpdateStats Then m_vAutoMTAInsFileCnts = Nothing

            If MultipleVersionsExist And Not m_bUpdateStats Then
                Return nResult
            End If

            'Assume this is a permanant MTA
            m_sEventReason = ACReasonBackdatedMTA

            m_lReturn = m_oFindInsurance.GetInsuranceFileType(v_lInsuranceFileCnt:=ToSafeInteger(m_lNewInsuranceFileCnt), r_sInsuranceFileTypeCode:=sInsuranceFileTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.GetInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sInsuranceFileTypeCode <> "MTAQTETEMP" Then
                nErrorCode = 1
            Else
                nErrorCode = 0
            End If

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = GetVersionByDate(r_lInsuranceFileCnt:=nInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=nPolicyVersion, r_lErrorCode:=nErrorCode, r_bBackdatingRequired:=bBackdatingRequired, v_bIsPermanentMTA:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "GetVersionByDate Failed"
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            If nErrorCode <> 0 Then
                'We have no valid policy version for this date
                Select Case nErrorCode
                    Case 1
                        r_sErrorText = "Future adjustment found"
                    Case 2
                        r_sErrorText = "Overlaps with temporary MTA"
                    Case 3
                        r_sErrorText = "No version found"
                End Select

                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                bTransStarted = True
            End If

            If Not m_bUpdateStats Then
                m_lReturn = AutoRunMTA(v_lVersion:=nPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate, v_lNewMTAInsuranceFileCnt:=m_lNewInsuranceFileCnt, v_bIsBackdatedMTA:=True, r_bShowQuoteMsg:=r_bShowQuoteMsg, v_bIsDirty:=v_bIsDirty)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_oDatabase.SQLRollbackTrans = gPMConstants.PMEReturnCode.PMTrue Then
                        bTransStarted = False
                    End If
                    'Don't go to error handler as there is more processing to do
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA")
                    nResult = gPMConstants.PMEReturnCode.PMFalse

                    'Delete any policy versions we have created and set the status
                    ' of the originals back to live
                    m_lReturn = RestoreAutoRunMTA()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "RestoreAutoRunMTA Failed"
                        Throw New Exception()
                    End If
                    Return nResult
                Else

                    'Update lapsed status
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="base_insurance_file_cnt", vValue:=lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        AutoBackdatedMTA = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=m_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        AutoBackdatedMTA = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    ' Execute the stored procedure
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdLapsedStatusSQL,
                                                      sSQLName:=ACUpdLapsedStatusName,
                                                      bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        AutoBackdatedMTA = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            Else
                m_lReturn = GetAffectedBackDatedMTAVersions()
                'Update the accounts
                m_lReturn = TransactPolicyVersions()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    sMessage = "TransactPolicyVersions Failed"
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If

                'Update IsDirty flage =False
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="BaseInsuranceFileCnt", vValue:=lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Execute the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateIsDirtyForBackDatedVersionsSQL, sSQLName:=ACUpdateIsDirtyForBackDatedVersionsName, bStoredProcedure:=ACUpdateIsDirtyForBackDatedVersionsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'End WPR 33
            End If

            'If we got here then everything must be OK, return set at top
            m_oDatabase.SQLCommitTrans()

            Return nResult

        Catch excep As System.Exception


            'Is this a genuine error or did we get here because a call failed?
            'If a call failed, then we will have set a message string
            If sMessage.Length > 0 Then
                'Ok so we're here because m_lRetrun came back not PMTrue
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA", excep:=excep)
            Else
                'Genuine error
                nResult = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoBackdatedMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            'either way - roll back the transaction if we got that far
            If bTransStarted Then

                m_oDatabase.SQLRollbackTrans()
            End If

            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AutoRunMTA
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    'WPR 33-75 added
    Public Function AutoRunMTA(ByVal v_lVersion As Integer, ByVal v_dtEffectiveDate As Date, Optional ByVal v_lNewMTAInsuranceFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByRef r_bShowQuoteMsg As Boolean = False, Optional ByRef v_bIsDirty As Object = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim i As Integer
            Dim lCancelledInsuranceFileCnt, lCnt As Integer
            Dim bFirst, bCancelled As Boolean
            Dim lFirstNewInsuranceFileCnt As Integer
            Dim bMTAApplied As Boolean
            Dim lNewTempMTAInsuranceFileCnt, lNewMTAInsuranceFileCnt As Integer
            'WPR 33-75 added
            Dim lNewRiskCnt As Integer
            Dim lFacRiskId As Integer
            Dim vOldAffectedFileCnts As Array
            lCnt = 0

            m_vDeclineReasons = ""

            m_vReferReasons = ""
            bFirst = True

            'Get the list of affected policy versions in ascending order
            m_lReturn = ReverseArray(r_vArray:=m_vAffectedInsuranceFileCnts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            i = 0
            For i = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)

                bCancelled = False
                bMTAApplied = False

                If i > 0 Then

                    '*************************************************************************
                    'Create cancelled version of Original MTA
                    '*************************************************************************
                    If m_bIsReinstateRisk And v_lNewMTAInsuranceFileCnt = m_lDeletedRiskInsuranceFileCnt Then
                        'Don't cancel
                    Else
                        vOldAffectedFileCnts = m_vAffectedInsuranceFileCnts
                        'First cancel the original poliy version
                        'WPR 33-75 added
                        m_lReturn = AutoRunCancellation(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), r_lVersion:=v_lVersion, v_dtMTAStartDate:=CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)), v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_sTransactionType:="MTCA", r_lNewInsuranceFileCnt:=lCancelledInsuranceFileCnt, v_vMTAEndDate:=m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i), v_bIsBackdatedMTA:=v_bIsBackdatedMTA)
                        m_vAffectedInsuranceFileCnts = vOldAffectedFileCnts
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        bCancelled = True
                    End If

                    '*************************************************************************
                    'This is an MTA so we need to reapply the existing MTA changes to
                    ' the new policy version
                    '*************************************************************************
                    If CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, i)).Trim() = "POLICY" Then
                        'Reapply the renewal
                        m_lReturn = AutoRunRenewalsForMTA(v_lPreChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i - 1)), v_lPostChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lBaseInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, r_bShowQuoteMsg:=r_bShowQuoteMsg)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunRenewalsForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                            'WPR 33-75 added
                        ElseIf r_bShowQuoteMsg And v_bIsBackdatedMTA = False Then
                            Return result
                        Else
                            ' Successfully went through a renewal then increase the version count
                            v_lVersion += 1
                        End If

                    Else
                        'REapply the MTA
                        If m_sTransactionType <> "MTC" Then
                            vOldAffectedFileCnts = m_vAffectedInsuranceFileCnts
                            'WPR 33-75 added
                            Dim sRiskExpiryDate As String
                            If i < UBound(m_vAffectedInsuranceFileCnts, 2) Then
                                'sRiskExpiryDate = VB6.Format(Informations.DateAdd(Microsoft.VisualBasic.DateInterval.day, -1, m_vAffectedInsuranceFileCnts(1, i + 1)), "yyyy/mm/dd")
                                sRiskExpiryDate = Informations.DateAdd("d", -1, m_vAffectedInsuranceFileCnts(1, i + 1)).ToString("yyyy/mm/dd")
                            Else
                                sRiskExpiryDate = "NULL"
                            End If

                            'WPR 33-75 added
                            m_lReturn = AutoRunMTAOnly(v_lPreChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i - 1)), v_lPostChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lBaseInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_dtMTAStartDate:=CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)), r_lVersion:=v_lVersion, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_vMTAEndDate:=m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i), v_bIsBackdatedMTA:=v_bIsBackdatedMTA, v_sRiskExpiryDate:=sRiskExpiryDate)
                            m_vAffectedInsuranceFileCnts = vOldAffectedFileCnts
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTAOnly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

                    bMTAApplied = True
                End If

                If i = 0 Then

                    If v_bApplyRiskChange Then
                        '*************************************************************************
                        'Auto apply risk change - generate the first MTA
                        '*************************************************************************
                        m_lReturn = AutoRunMTAOnly(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_dtMTAStartDate:=m_dtEffectiveDate, r_lVersion:=v_lVersion, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_bApplyRiskChange:=v_bApplyRiskChange)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTAOnly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        bMTAApplied = True

                    ElseIf v_bIsBackdatedMTA Then
                        '*************************************************************************
                        'This is the actual MTA quote so we need to update the premium on
                        'the insurance_file record before displaying on the screen
                        '*************************************************************************
                        bMTAApplied = True
                    End If
                End If

                If bMTAApplied Then
                    'WPR 33-75 added
                    If v_bIsBackdatedMTA = False Then
                        m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt)
                    End If
                    'We have run the mta process
                    If bFirst Then
                        'This is the first one
                        lFirstNewInsuranceFileCnt = v_lNewMTAInsuranceFileCnt
                        'WPR 33-75 added
                        m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink(v_lInsuranceFileCnt:=ToSafeInteger(lFirstNewInsuranceFileCnt), bIsDirty:=v_bIsDirty)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'WPR 33-75 added commented as per WPR
                        'bFirst = False
                    End If
                    If bCancelled Then
                        'Update the mta_insurance_file_link table with the cancelled insurance_file_cnt
                        m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vCancelledLinkedInsuranceFileCnt:=lCancelledInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return result
                        End If
                    End If

                    If CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, i)).Trim() = "MTA TEMP" Then
                        lNewMTAInsuranceFileCnt = lNewTempMTAInsuranceFileCnt
                    Else
                        'BDMTA - this one
                        lNewMTAInsuranceFileCnt = v_lNewMTAInsuranceFileCnt
                    End If
                    'Update the mta_insurance_file_link table with the new insurance_file_cnt
                    m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vNewLinkedInsuranceFileCnt:=lNewMTAInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                        Return result
                    End If

                    'WPR 33-75 added
                    If v_bIsBackdatedMTA = True And bFirst = False Then
                        ' keep the versions dirty until transacted
                        If ToSafeLong(v_lNewMTAInsuranceFileCnt) > 0 Then
                            m_lReturn = UpdateInsuranceFileReplaced(v_lNewMTAInsuranceFileCnt, 1)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileReplaced Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                                Return result
                            End If
                            m_lReturn = UpdateBaseInsuranceFile(m_lNewInsuranceFileCnt, v_lNewMTAInsuranceFileCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateBaseInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                                Return result
                            End If
                        End If

                        If ToSafeLong(lCancelledInsuranceFileCnt) > 0 Then
                            m_lReturn = UpdateInsuranceFileReplaced(lCancelledInsuranceFileCnt, 1)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileReplaced Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                                Return result
                            End If
                            m_lReturn = UpdateBaseInsuranceFile(m_lNewInsuranceFileCnt, lCancelledInsuranceFileCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateBaseInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                                Return result
                            End If
                        End If
                    Else
                        m_lReturn = UpdateBaseInsuranceFile(m_lNewInsuranceFileCnt, m_lNewInsuranceFileCnt)
                        bFirst = False
                    End If
                End If

            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileStatus
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateInsuranceFileStatus(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sInsuranceFileStatusCode As String = "", Optional ByVal v_lInsuranceFileStatusId As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        If v_sInsuranceFileStatusCode <> "" Then
            m_lReturn = m_oFindInsurance.UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_sInsuranceFileStatusCode:=ToSafeString(v_sInsuranceFileStatusCode))
        Else
            m_lReturn = m_oFindInsurance.UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_lInsuranceFileStatusId:=ToSafeInteger(v_lInsuranceFileStatusId))
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileStatus")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileType
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateInsuranceFileType(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceFileTypeCode As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = m_oFindInsurance.UpdateInsuranceFileType(v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_sInsuranceFileTypeCode:=ToSafeString(v_sInsuranceFileTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.UpdateInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileType")
            Return result
        End If

        Return result

    End Function

    ''' <summary>
    ''' Auto Run Renewals For Reinstatement
    ''' </summary>
    ''' <param name="nBaseInsuranceFileCnt"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function AutoRunRenewalsForReinstatement(
     ByVal nBaseInsuranceFileCnt As Integer,
     ByVal nInsuranceFileCnt As Integer,
     Optional ByRef nNewInsuranceFileCnt As Object = 0,
     Optional ByVal nAffectedInsuranceFileCnt As Integer = 0) As Integer



        Dim iCnt As Integer

        'Run the selection process
        m_oAutomaticRenewalsSel.InsuranceFileCnt = nInsuranceFileCnt
        m_oAutomaticRenewalsSel.dtSelectionDate = Date.Now
        m_oAutomaticRenewalsSel.AffectedInsuranceFileCnt = nAffectedInsuranceFileCnt

        m_lReturn = m_oAutomaticRenewalsSel.RenewalSelectionForMTA
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername,
                         iType:=gPMConstants.PMELogLevel.PMLogOnError,
                         sMsg:="m_oAutomaticRenewalsSel.RenewalSelectionForMTA Failed",
                         vApp:=ACApp,
                         vClass:=ACClass,
                         vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_oAutomaticRenewalsSel.FailureReason <> "" Then
            bPMFunc.LogMessage(m_sUsername,
                         iType:=gPMConstants.PMELogLevel.PMLogOnError,
                         sMsg:="AutoRunRenewalsForReinstatement Failed: - " & m_oAutomaticRenewalsSel.FailureReason,
                         vApp:=ACApp,
                         vClass:=ACClass,
                         vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            'Keep a record of all the new policies we are creating
            If Informations.IsArray(m_vAutoMTAInsFileCnts) = True Then
                iCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
                ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, iCnt)
            Else
                iCnt = 0
                ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, iCnt)
            End If
            m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, iCnt) = nNewInsuranceFileCnt
            m_vAutoMTAInsFileCnts(ACAutoMTATransType, iCnt) = "RENEWAL"
        End If

        nNewInsuranceFileCnt = m_oAutomaticRenewalsSel.NewInsuranceFileCnt

        If nNewInsuranceFileCnt = 0 Then
            bPMFunc.LogMessage(m_sUsername,
                         iType:=gPMConstants.PMELogLevel.PMLogOnError,
                         sMsg:="Failed to create new renewal policy version ",
                         vApp:=ACApp,
                         vClass:=ACClass,
                         vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_bBackDateMTA = False Then
            'Run the accept process (but do not update any transactions)
            m_oAutomaticRenewalsAccept.InsuranceFileCnt = nNewInsuranceFileCnt
            m_lReturn = m_oAutomaticRenewalsAccept.RenewalAcceptForMTA
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername,
                             iType:=gPMConstants.PMELogLevel.PMLogOnError,
                             sMsg:="m_oAutomaticRenewalsAccept.RenewalAcceptForMTA Failed",
                             vApp:=ACApp,
                             vClass:=ACClass,
                             vMethod:="AutoRunRenewalsForReinstatement")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add an entry to the mta_insurance_file_link table
            m_lReturn = m_oFindInsurance.AddToMTAInsuranceFileLink(
                     v_lBaseInsuranceFileCnt:=ToSafeInteger(nBaseInsuranceFileCnt),
                     v_vOriginalLinkedInsuranceFileCnt:=ToSafeInteger(nInsuranceFileCnt),
                     v_vNewLinkedInsuranceFileCnt:=nNewInsuranceFileCnt
                     )
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AutoRunRenewalsForReinstatement = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername,
                             iType:=gPMConstants.PMEReturnCode.PMError,
                             sMsg:="m_oFindInsurance.AddToMTAInsuranceFileLink Failed",
                             vApp:=ACApp,
                             vClass:=ACClass,
                             vMethod:="AutoRunRenewalsForReinstatement")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            'don't keep policy in renewal cycle
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLAction("Delete renewal_status Where insurance_file_cnt = " & nInsuranceFileCnt, "Delete renewal_status", False)
        End If

        Return m_lReturn


    End Function

    ''' <summary>
    ''' AutoRunRenewalsForMTA
    ''' </summary>
    ''' <param name="v_lBaseInsuranceFileCnt"></param>
    ''' <param name="v_lPreChangeInsFileCnt"></param>
    ''' <param name="v_lPostChangeInsFileCnt"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="r_bShowQuoteMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AutoRunRenewalsForMTA(ByVal v_lBaseInsuranceFileCnt As Integer,
                                           ByVal v_lPreChangeInsFileCnt As Integer,
                                           ByVal v_lPostChangeInsFileCnt As Integer,
                                           ByRef r_lNewInsuranceFileCnt As Integer,
                                           Optional ByRef r_bShowQuoteMsg As Boolean = False) As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Dim nNewInsuranceFileCnt, nUbound As Integer
        Dim nRunMode As Integer
        Dim bMergeRisks As Boolean

        'Run the selection process

        m_oAutomaticRenewalsSel.InsuranceFileCnt = v_lBaseInsuranceFileCnt

        m_oAutomaticRenewalsSel.dtSelectionDate = DateTime.Now.AddDays(2000)

        nRunMode = 0
        bMergeRisks = True
        If m_bIsReinstateRisk Then
            bMergeRisks = False
            If v_lPostChangeInsFileCnt = m_lDeletedRiskInsuranceFileCnt Then
                nRunMode = 1
            Else
                nRunMode = 2
            End If
        End If

        If m_bIsNCDChange Then
            nRunMode = 2
        End If


        m_lReturn = m_oAutomaticRenewalsSel.RenewalSelectionForMTA(v_bMergeRisks:=bMergeRisks,
                                                                   v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt,
                                                                   v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt,
                                                                   r_lDeletedRiskInsuranceFileCnt:=m_lDeletedRiskInsuranceFileCnt,
                                                                   r_lDeletedRiskCnt:=m_lDeletedRiskCnt,
                                                                   v_iRunMode:=nRunMode,
                                                                   r_bShowQuoteMsg:=r_bShowQuoteMsg)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="m_oAutomaticRenewalsSel.RenewalSelectionForMTA Failed",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return PMEReturnCode.PMFalse
        End If

        If m_oAutomaticRenewalsSel.FailureReason <> "" And Not r_bShowQuoteMsg Then
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="AutoRunRenewalsForMTA Failed: - " & m_oAutomaticRenewalsSel.FailureReason,
                               vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return PMEReturnCode.PMFalse
        End If


        nNewInsuranceFileCnt = m_oAutomaticRenewalsSel.NewInsuranceFileCnt

        m_lReturn = UpdateCurrencyToInsuranceFile(nNewInsuranceFileCnt)

        'Add an entry to the array for processing transactions
        If nNewInsuranceFileCnt > 0 Then
            nUbound = m_vAutoMTAInsFileCnts.GetUpperBound(1)
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, nUbound + 1)
            m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, nUbound + 1) = nNewInsuranceFileCnt
            m_vAutoMTAInsFileCnts(ACAutoMTATransType, nUbound + 1) = "RENEWAL"
        End If

        r_lNewInsuranceFileCnt = nNewInsuranceFileCnt

        If nNewInsuranceFileCnt = 0 Then
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to create new renewal policy version ",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return PMEReturnCode.PMFalse
        End If

        'No need to accept renewal
        If r_bShowQuoteMsg = False And bMergeRisks = False Then
            'Run the accept process (but do not update any transactions)
            m_oAutomaticRenewalsAccept.InsuranceFileCnt = nNewInsuranceFileCnt

            m_lReturn = m_oAutomaticRenewalsAccept.RenewalAcceptForMTA
            If m_lReturn <> PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="m_oAutomaticRenewalsAccept.RenewalAcceptForMTA Failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
                Return PMEReturnCode.PMFalse
            End If
            'WPR 33-75 added
        ElseIf bMergeRisks = True Then
            'don't keep policy in renewal cycle
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLAction("Delete renewal_status Where insurance_file_cnt = " & v_lPostChangeInsFileCnt & " and renewal_insurance_file_cnt = " & nNewInsuranceFileCnt, "Delete renewal_status", False)
        End If

        If v_lPostChangeInsFileCnt > 0 Then
            'This is the original version of the MTA which has just been reapplied
            'so set the status to replaced
            'WPR 33-75 added
            'm_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sInsuranceFileStatusCode:="REP")
            m_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sInsuranceFileStatusCode:="REPBDMTA")
            If m_lReturn <> PMEReturnCode.PMTrue Then
                result = PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
                Return result
            End If
            m_lReturn = UpdateCurrencyToInsuranceFile(r_lNewInsuranceFileCnt)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                result = PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=PMEReturnCode.PMError, sMsg:="UpdateCurrencyToInsuranceFile Failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
                Return result
            End If
            'Record a reason against this replaced record
            m_lReturn = CreateEvent(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sReason:=m_sEventReason)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                result = PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=PMEReturnCode.PMError, sMsg:="CreateEvent Failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
                Return result
            End If

        End If

        Return result

    End Function
    ''' <summary>
    ''' TransactPolicyVersions
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TransactPolicyVersions() As Integer

        Dim nResult As Integer = 0
        Dim bTransStarted As Boolean
        Dim oIFileArray(,) As Object
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            Dim nStart As Integer
            Dim nOriginalLinkedInsuranceFileCnt As Object
            Dim nMTAAllocation As Integer
            Dim vTransIdArray(,) As Object


            If Not Informations.IsArray(m_vAutoMTAInsFileCnts) And m_lNewInsuranceFileCnt <> 0 Then
                'Rebuild the internal arrays from the mta_insurance_file_link table
                m_lReturn = RebuildArrays()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildArrays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If
            'refresh versions to transact
            m_lReturn = GetAffectedBackDatedMTAVersions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAffectedBackDatedMTAVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nStart = 0

            'Start a new transaction
            m_lReturn = m_oControlTrans.BeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bTransStarted = True

            m_oControlTrans.RunMode = m_lRunMode

            If Informations.IsArray(m_vAutoMTAInsFileCnts) Then

                ' For each of the new insurance files raised during this process
                For i As Integer = nStart To m_vAutoMTAInsFileCnts.GetUpperBound(1)

                    'Get the original version that was cancelled
                    If (m_bBackDateMTA) Then
                        If m_sTransactionType = "MTR" Then
                            m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=CType(m_vAutoMTAInsFileCnts(ACAffectedInsuranceFileCnt, i), Object(,)), r_lOriginalLinkedInsuranceFileCnt:=nOriginalLinkedInsuranceFileCnt, v_bLookForCancelled:=False)

                            ' replace original version
                            m_lReturn = UpdateInsuranceFileReplaced(nOriginalLinkedInsuranceFileCnt, 1)

                            m_lReturn = UpdateInsuranceFileReplaced(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), 0)

                            If i = 0 Then ' update original cancelled line
                                m_oDatabase.Parameters.Clear()
                                m_lReturn = m_oDatabase.SQLAction("Update mta_insurance_file_link Set processed_ind = 1 Where type_ind = 1 AND insurance_file_cnt = " & m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), "Update processed", False)
                                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                    Call m_oControlTrans.RollbackTrans()
                                    TransactPolicyVersions = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If
                            End If
                        Else
                            If CStr(m_vAutoMTAInsFileCnts(ACAutoMTATransType, i)) = "MTC" Then

                                ' Get the original insurance file affected by this new cancellation Insurance File
                                m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), r_lOriginalLinkedInsuranceFileCnt:=nOriginalLinkedInsuranceFileCnt, v_bLookForCancelled:=True)
                                ' replace original version
                                m_lReturn = UpdateInsuranceFileReplaced(nOriginalLinkedInsuranceFileCnt, 1)

                                If m_sTransactionType = "MTC" Then
                                    ' if OOS MTC then update replaced version back to live
                                    m_lReturn = UpdateInsuranceFileReplaced(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), 0)
                                End If
                            Else
                                ' Get the original insurance file affected by this new Insurance File
                                m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), r_lOriginalLinkedInsuranceFileCnt:=nOriginalLinkedInsuranceFileCnt, v_bLookForCancelled:=False)
                                m_lReturn = UpdateInsuranceFileReplaced(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), 0)

                            End If
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.GetOriginalLinkedVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                            'Rollback the transaction
                            m_oControlTrans.RollbackTrans()
                            Return nResult
                        End If
                        m_lReturn = GetOutOfSequenceMTAProductDetails(v_lInsurance_file_cnt:=nOriginalLinkedInsuranceFileCnt, r_iMTAAllocation:=nMTAAllocation)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetOutOfSequenceMTAAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                            'Rollback the transaction
                            m_oControlTrans.RollbackTrans()
                            Return nResult
                        End If
                        Dim aFileArray(,) As Object
                        If m_vAutoMTAInsFileCnts(ACAutoMTATransType, i) = "MTA" Then
                            ' Clear the Database Parameters Collection
                            m_oDatabase.Parameters.Clear()

                            ' Add the Insurance File Cnt parameter (INPUT)
                            bPMAddParameter.AddParameterLite(m_oDatabase, "nInsuranceFileCnt", CInt(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                            ' Execute SQL Statement   
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGETInsuranceFileTypeIDAndCodeNameSQL, sSQLName:=ACGETInsuranceFileTypeIDAndCodeName, bStoredProcedure:=ACGETInsuranceFileTypeIDAndCodeStored, vResultArray:=aFileArray)
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                Return gPMConstants.PMEReturnCode.PMFalse

                            Else
                                If Informations.IsArray(aFileArray) Then
                                    ' we want to change specific versions only avoiding bSIRChangePolicyStatus
                                    If Trim(CStr(aFileArray(0, 0))).ToUpper() = "MTAQCAN" Then
                                        ' Clear the Database Parameters Collection
                                        m_oDatabase.Parameters.Clear()
                                        ' Add the Insurance File Cnt parameter (INPUT)
                                        bPMAddParameter.AddParameterLite(m_oDatabase, "nInsuranceFileCnt", CInt(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                                        bPMAddParameter.AddParameterLite(m_oDatabase, "sInsuranceFileTypeCode", CStr("MTACAN"), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                        ' Execute SQL Statement   
                                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDATEInsuranceFileTypeIdFromCodeSQL, sSQLName:=ACUPDATEInsuranceFileTypeIdFromCodeName, bStoredProcedure:=ACUPDATEInsuranceFileTypeIdFromCodeStored)
                                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    ElseIf Trim(CStr(aFileArray(0, 0))).ToUpper() = "MTAQREINS" Then
                                        ' Add the Insurance File Cnt parameter (INPUT)
                                        bPMAddParameter.AddParameterLite(m_oDatabase, "nInsuranceFileCnt", CInt(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                                        bPMAddParameter.AddParameterLite(m_oDatabase, "sInsuranceFileTypeCode", CStr("MTAREINS"), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                        ' Execute SQL Statement   
                                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDATEInsuranceFileTypeIdFromCodeSQL, sSQLName:=ACUPDATEInsuranceFileTypeIdFromCodeName, bStoredProcedure:=ACUPDATEInsuranceFileTypeIdFromCodeStored)
                                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        oIFileArray = Nothing
                        If m_vAutoMTAInsFileCnts(ACAutoMTATransType, i) = "MTA" Then
                            m_oDatabase.Parameters.Clear()
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFileTypeSQL,
                                      sSQLName:=ACGetInsuranceFileTypeName,
                                      bStoredProcedure:=ACGetInsuranceFileTypeStored,
                                      vResultArray:=oIFileArray)
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                TransactPolicyVersions = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            Else
                                If Informations.IsArray(oIFileArray) Then
                                    ' we want to change specific versions only avoiding bSIRChangePolicyStatus
                                    If oIFileArray(1, 0) = "MTAQCAN" Then
                                        m_oDatabase.Parameters.Clear()

                                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_type_id", vValue:=8, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileTypeIdSQL, sSQLName:=ACUpdateInsuranceFileTypeIdName, bStoredProcedure:=ACUpdateInsuranceFileTypeIdStored)
                                    ElseIf oIFileArray(1, 0) = "MTAQREINS" Then
                                        m_oDatabase.Parameters.Clear()

                                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_type_id", vValue:=9, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileTypeIdSQL, sSQLName:=ACUpdateInsuranceFileTypeIdName, bStoredProcedure:=ACUpdateInsuranceFileTypeIdStored)
                                        m_vAutoMTAInsFileCnts(ACAutoMTATransType, i) = "MTR"
                                    Else
                                        oIFileArray = Nothing
                                    End If
                                End If
                            End If
                        End If
                    End If


                    ' Process Stats if it is not lapsed
                    If m_bIsPostStatsForLapsedPolicy = True Or m_bUpdateStats = True Then

                        'Create the stats tables and update the accounts in Orion
                        m_lReturn = ProcessStats(v_lInsuranceFileCnt:=CInt(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)), v_sTransactionType:=CStr(m_vAutoMTAInsFileCnts(ACAutoMTATransType, i)), v_lOriginalInsuranceFileCnt:=nOriginalLinkedInsuranceFileCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Rollback the transaction
                            m_lReturn = m_oControlTrans.RollbackTrans
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Not Informations.IsArray(oIFileArray) Then
                            'Update the policy status
                            m_lReturn = ProcessChangePolicyStatus(v_lInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), v_sTransactionType:=m_vAutoMTAInsFileCnts(ACAutoMTATransType, i))
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                    End If
                    If m_bBackDateMTA = True AndAlso nMTAAllocation = 1 Then
                        m_lReturn = GetTransactionIdsForReversal(v_lInsurance_file_cnt:=nOriginalLinkedInsuranceFileCnt, r_vArray:=vTransIdArray)

                        If Informations.IsArray(vTransIdArray) And Not Informations.IsArray(vTransIdArray) Then

                            For iCounter As Integer = vTransIdArray.GetLowerBound(1) To vTransIdArray.GetUpperBound(1)
                                m_lReturn = m_oDocumentReversal.ReverseAllocation(v_lTransDetailID:=vTransIdArray(0, iCounter))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reverse the allocation", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                m_lReturn = UpdateComment(v_lTransaction_Id:=gPMFunctions.ToSafeLong(CInt(vTransIdArray(0, iCounter))))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    'Do not exit from the function
                                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the comment", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                                End If
                            Next iCounter
                        End If
                    End If
                Next i
            End If

            'Commit the transaction

            m_lReturn = m_oControlTrans.CommitTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bTransStarted = False

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            If bTransStarted Then

                m_oControlTrans.RollbackTrans()
            End If

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function
    'WPR 33-75 added
    Private Function CheckIfLapsed(ByVal v_lInsuranceFileCnt As Integer, ByRef bIsLapsed As Boolean) As Integer

        Dim vlLapsedReasonId As Integer



        CheckIfLapsed = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_reason_id", vValue:=vlLapsedReasonId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIfLapsedSQL, sSQLName:=ACCheckIfLapsedName, bStoredProcedure:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            vlLapsedReasonId = ToSafeLong(m_oDatabase.Parameters.Item("lapsed_reason_id").Value, 0)
            If vlLapsedReasonId <> 0 Then
                bIsLapsed = True
            Else
                bIsLapsed = False
            End If

        End If
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    ' ***************************************************************** '
    '
    ' Name: RebuildArrays
    '
    ' Description:
    '
    ' History: 16/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function RebuildArrays() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const AC5OriginalInsuranceFileCnt As Integer = 0
        Const AC5NewInsuranceFileCnt As Integer = 1
        Const AC5TypeInd As Integer = 2
        Const AC5CoverStartDate As Integer = 3
        Const AC5PolicyVersion As Integer = 4
        Const AC5InsuranceFileType As Integer = 5
        Const AC5ExpiryDate As Integer = 6
        Const AC5InsuranceFileStatus As Integer = 7

        Dim vResultArray As Object
        Dim sType As String = ""
        Dim lUbound As Integer

        'Rebuild the internal arrays from the mta_insurance_file_link table

        m_lReturn = m_oFindInsurance.RebuildArrayFromLinkedPolicyVersions(r_vResultArray:=vResultArray, v_lInsuranceFileCnt:=ToSafeInteger(m_lNewInsuranceFileCnt))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="RebuildArrayFromLinkedPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildArrays")
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        If m_bIsReinstateRisk Then

            m_lDeletedRiskInsuranceFileCnt = CInt(vResultArray(AC5OriginalInsuranceFileCnt, 0))
        End If


        lUbound = vResultArray.GetUpperBound(1)

        ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lUbound)
        m_vAffectedInsuranceFileCnts = Array.CreateInstance(GetType(Object), New Integer() {ACAffectedArraySize + 1, lUbound + 1}, New Integer() {0, 0})


        For i As Integer = 0 To vResultArray.GetUpperBound(1)
            Select Case vResultArray(AC5TypeInd, i)
                Case 1
                    sType = "MTC"
                Case 2
                    m_bIsPolicyReinstatement = True
                    sType = "MTA"
                Case Else
                    sType = "MTA"
            End Select

            'New insurance file records

            m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i) = vResultArray(AC5NewInsuranceFileCnt, i)
            m_vAutoMTAInsFileCnts(ACAutoMTATransType, i) = sType

            m_vAutoMTAInsFileCnts(ACAutoMTAVersion, i) = vResultArray(AC5PolicyVersion, i)

            'Original insurance file records

            m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i) = vResultArray(AC5OriginalInsuranceFileCnt, i)

            m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i) = vResultArray(AC5CoverStartDate, i)

            m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i) = vResultArray(AC5PolicyVersion, i)

            m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, i) = vResultArray(AC5InsuranceFileType, i)

            m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i) = vResultArray(AC5ExpiryDate, i)

            m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i) = vResultArray(AC5InsuranceFileStatus, i)
        Next i

        Return result

    End Function

    '*************************************************************************
    ' Name:         RestoreAutoRunMTA
    ' Description:
    ' History:      06/01/2003 sj - Created.
    '               16/08/2004 TR - RESILIENCE work - added transactional support
    '*************************************************************************
    Public Function RestoreAutoRunMTA(Optional ByRef v_bKeepQuote As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim bTransStarted As Boolean
        Dim sMessage As String = ""
        Dim sTransactionType As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (Not Informations.IsArray(m_vAutoMTAInsFileCnts) Or Not Informations.IsArray(m_vAffectedInsuranceFileCnts)) And m_lNewInsuranceFileCnt <> 0 Then
                'Rebuild the internal arrays from the mta_insurance_file_link table
                m_lReturn = RebuildArrays()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    sMessage = "RebuildArrays Failed"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return result
                End If
            End If

            If Not Informations.IsArray(m_vAutoMTAInsFileCnts) Or Not Informations.IsArray(m_vAffectedInsuranceFileCnts) Then
                Return result
            End If

            'TR - RESILIENCE start a transaction
            'Wrap in transaction to make sure all Updates are completed successfully or none are

            m_lReturn = m_oDatabase.SQLBeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Failed to start transaction"
                Throw New Exception()
            Else
                bTransStarted = True
            End If

            For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)

                'Set the policy status of the affected versions back to what they
                'were before
                'RESILIENCE - there is 1 update in this function
                m_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lInsuranceFileStatusId:=CInt(Val(CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i)))))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "UpdateInsuranceFileStatus Failed"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If

                'Update the original cancelled record in the
                'mta_insurance_file_link table BACK to unprocessed
                If m_bIsPolicyReinstatement Then
                    'RESILIENCE - there is 1 update here also
                    m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=2, v_vCancelledLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vProcessedInd:=0)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "UpdateMTAInsuranceFileLink Failed"
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If
                End If
            Next i

            For i As Integer = 0 To m_vAutoMTAInsFileCnts.GetUpperBound(1)
                If CDbl(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)) <> m_lNewInsuranceFileCnt Or Not v_bKeepQuote Then
                    'Delete all the policy versions we have created

                    m_lReturn = m_oRenSelection.DeletePolicy(v_lInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "m_oRenewalSelection.DeletePolicy Failed"
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Throw New Exception()
                    End If
                End If

            Next i

            If v_bKeepQuote And m_lNewInsuranceFileCnt > 0 Then
                'Update the status of the original quote back to quoted
                Select Case m_sTransactionType
                    Case "MTA"
                        sTransactionType = "MTAQUOTE"
                    Case "MTC"
                        sTransactionType = "MTAQCAN"
                    Case "MTR"
                        sTransactionType = "MTAQREINS"
                    Case Else
                        sTransactionType = "MTAQUOTE"
                End Select
                m_lReturn = UpdateInsuranceFileType(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt, v_sInsuranceFileTypeCode:=sTransactionType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "UpdateInsuranceFileStatus Failed"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception()
                End If
            End If

            'Delete the mta_insurance_file_link records we have created
            If CStr(m_vAutoMTAInsFileCnts(ACAutoMTATransType, 0)) = "MTC" Then
                m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=5, v_vCancelledLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, 0))
            Else
                m_lReturn = UpdateMTAInsuranceFileLink(v_iType:=5, v_vNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, 0))
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "UpdateMTAInsuranceFileLink Failed"
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception()
            End If

            'developer guide no solution 28
            m_vAutoMTAInsFileCnts = Nothing

            'developer guide no solution 28
            m_vAffectedInsuranceFileCnts = Nothing


            'If we got here then everything must be OK, return set at top

            m_oDatabase.SQLCommitTrans()

            Return result

        Catch excep As System.Exception


            'Is this a genuine error or did we get here because a call failed?
            'If a call failed, then we will have set a message string
            If sMessage.Length > 0 Then
                'Ok so we're here because m_lRetrun came back not PMTrue
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA", excep:=excep)
            Else
                'Genuine error
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            'either way - roll back the transaction if we got that far
            If bTransStarted Then

                m_oDatabase.SQLRollbackTrans()
            End If

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AutoRunMTAOnly
    '
    ' Description:
    '
    ' History: 06/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    'WPR 33-75 added
    Private Function AutoRunMTAOnly(ByVal v_lBaseInsuranceFileCnt As Integer, ByVal v_dtMTAStartDate As Date, ByVal v_lExistingPolicyVersion As Integer, ByRef r_lVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_vMTAEndDate As Object = Nothing, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal v_bMarkAsUnquoted As Object = False, Optional ByVal v_sRiskExpiryDate As String = "NULL") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        Dim aArray(,) As Object
        Dim bReinstatement As Boolean
        Dim bCancellation As Boolean
        Dim lCnt As Integer
        Dim oArray As Object

        'Make a copy of the policy
        If m_bIsReinstateRisk And m_lDeletedRiskInsuranceFileCnt = v_lBaseInsuranceFileCnt Then
            m_lReturn = CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate, v_bCopyDeletedLink:=True)

            'WPR 33-75 added
        ElseIf Not Informations.IsNothing(v_vMTAEndDate) And m_sTransactionType <> "MTC" Then
            If v_bIsBackdatedMTA = True And v_lPostChangeInsFileCnt > 0 Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "nInsuranceFileCnt", CInt(v_lPostChangeInsFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                ' Add the Insurance File Cnt parameter (INPUT)
                ' Execute SQL Statement   
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGETInsuranceFileTypeIDAndCodeNameSQL, sSQLName:=ACGETInsuranceFileTypeIDAndCodeName, bStoredProcedure:=ACGETInsuranceFileTypeIDAndCodeStored, vResultArray:=aArray)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If Informations.IsArray(aArray) Then
                    ' reinstate if original version was reinstated
                    If Trim(CStr(aArray(0, 0))).ToUpper() = "MTAREINS" Then
                        bReinstatement = True
                    ElseIf Trim(CStr(aArray(0, 0))).ToUpper() = "MTACAN" Then
                        bCancellation = True
                    End If
                End If
            End If
            'Temporary MTA if BackdateMTA is false else Permanent MTA
            m_lReturn = CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=v_bIsBackdatedMTA, v_dtMTADate:=v_dtMTAStartDate, v_vMTAEndDate:=v_vMTAEndDate, v_bCancellation:=bCancellation, bReinstatement:=bReinstatement)
        Else
            m_lReturn = CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'WPR 33-75 added
        If v_bIsBackdatedMTA = True And v_vMTAEndDate IsNot Nothing And m_sTransactionType <> "MTC" Then
            m_lReturn = m_oDatabase.SQLAction("Update insurance_file Set renewal_date = (Select renewal_date From Insurance_File Where insurance_file_cnt = " & v_lPostChangeInsFileCnt & " ) Where insurance_file_cnt = " & r_lNewInsuranceFileCnt, "Update renewal date", False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'If we are doing a risk reinstatement record the reason against the policy version
        'we are reinstating
        If m_bIsReinstateRisk And m_lDeletedRiskInsuranceFileCnt = v_lBaseInsuranceFileCnt And m_sReinstatementReason <> "" Then

            m_lNewInsuranceFileCnt = r_lNewInsuranceFileCnt
        End If

        ' Check the return value.
        'Keep a record of all the new policies we are creating
        If Informations.IsArray(m_vAutoMTAInsFileCnts) Then
            lCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        Else
            lCnt = 0
            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        End If
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, lCnt) = r_lNewInsuranceFileCnt
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, lCnt) = "MTA"
        m_vAutoMTAInsFileCnts(ACAutoMTAVersion, lCnt) = r_lVersion + 1

        m_lReturn = UpdateCurrencyToInsuranceFile(r_lNewInsuranceFileCnt)

        If m_sTransactionType = "MTR" Then
            'Quote all the risks
            If m_bIsReinstateRisk Then
                m_lReturn = ProcessRisksForRiskReinstatement(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_dtMTAStartDate:=v_dtMTAStartDate, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt)
            ElseIf m_bIsNCDChange And v_lPostChangeInsFileCnt > 0 Then
                m_lReturn = ProcessRisksForNCDChange(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sTransactionType:="MTACR", v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt)
            ElseIf m_bMergeRisks And Not v_bApplyRiskChange Then
                m_lReturn = ProcessRisksWithMerge(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sTransactionType:="MTA", v_dtMTAStartDate:=v_dtMTAStartDate, v_lPreChangeInsFileCnt:=v_lBaseInsuranceFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bApplyRiskChange:=v_bApplyRiskChange, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)
            Else
                m_lReturn = ProcessRisksForMultiThreading(nInsuranceFileCnt:=r_lNewInsuranceFileCnt, sTransactionType:="MTA", dtMTAStartDate:=v_dtMTAStartDate, nPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, nPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, bApplyRiskChange:=v_bApplyRiskChange, bIsBackdatedMTA:=v_bIsBackdatedMTA)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Policy level tax
            m_lReturn = ProcessRITax(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lRiskCnt:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Call the agent commission component
            m_lReturn = ProcessAgentCommission(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If m_bIsReinstateRisk And m_lDeletedRiskInsuranceFileCnt = v_lBaseInsuranceFileCnt Then
            'In this case we are not replacing an existing MTA but creating a
            'new MTA to reinstate the risk
        Else
            If v_lPostChangeInsFileCnt > 0 Then
                'This is the original version of the MTA which has just been reapplied
                'so set the status to replaced
                If v_bIsBackdatedMTA Then

                    m_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sInsuranceFileStatusCode:="REPBDMTA")
                Else

                    m_lReturn = UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sInsuranceFileStatusCode:="REP")
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                    Return result
                End If
                m_lReturn = UpdateCurrencyToInsuranceFile(r_lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateCurrencyToInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                    Return result
                End If
                'Record a reason against this replaced record
                m_lReturn = CreateEvent(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sReason:=m_sEventReason)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                    Return result
                End If
                m_lReturn = UpdateInsuranceFileDetails(v_lPostChangeInsFileCnt, r_lNewInsuranceFileCnt)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                    Return result
                End If
            End If
        End If

        'Increment the version number
        r_lVersion += 1

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: UpdateMTAInsuranceFileLink
    '
    ' Description:
    '
    ' History: 14/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateMTAInsuranceFileLink(ByVal v_iType As Integer, Optional ByVal v_vInsuranceFileCnt As Integer = 0, Optional ByVal v_vOriginalLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vCancelledLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vNewLinkedInsuranceFileCnt As Object = Nothing, Optional ByVal v_vProcessedInd As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Informations.IsNothing(v_vNewLinkedInsuranceFileCnt) Then

            m_lReturn = m_oFindInsurance.UpdateMTAInsuranceFileLink(v_iType:=ToSafeInteger(v_iType), v_vInsuranceFileCnt:=ToSafeInteger(v_vInsuranceFileCnt), v_vOriginalLinkedInsuranceFileCnt:=CType(v_vOriginalLinkedInsuranceFileCnt, Object), v_vNewLinkedInsuranceFileCnt:=CType(v_vNewLinkedInsuranceFileCnt, Object), v_vProcessedInd:=CType(v_vProcessedInd, Object))
        Else

            m_lReturn = m_oFindInsurance.UpdateMTAInsuranceFileLink(v_iType:=ToSafeInteger(v_iType), v_vInsuranceFileCnt:=ToSafeInteger(v_vInsuranceFileCnt), v_vOriginalLinkedInsuranceFileCnt:=CType(v_vOriginalLinkedInsuranceFileCnt, Object), v_vCancelledLinkedInsuranceFileCnt:=CType(v_vCancelledLinkedInsuranceFileCnt, Object), v_vProcessedInd:=CType(v_vProcessedInd, Object))
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: ProcessRisks
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Friend Function ProcessRisks(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_dtMTAStartDate As Date = #12/30/1899#, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal v_bCopyRiskOnMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vDataArray(,) As Object
            Dim sFailureReason As String = ""
            Dim lNewRiskCnt As Integer
            Dim vSelectionArray(,) As Object
            Dim iSelected As Integer
            Dim lRiskId As Integer

            'WPR 33-75 added
            Dim sRiskStatus As String
            Dim i As Integer
            Dim bIsEdited As Boolean
            Dim oArray(,) As Object
            Dim sRiskMergeStatus As String

            'WPR 33-75 added
            If v_bIsBackdatedMTA = False Then
                m_lReturn = GetListOfRisks(v_lInsuranceFileCnt, vDataArray)
                If Informations.IsArray(vDataArray) Then
                    sRiskStatus = vDataArray(ACIRiskStatusFlag, i)
                End If
            Else
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nBase_insurance_file_cnt", vValue:=m_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nNew_insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'Get a list of all the risks with edited flag for version being cancelled and generate refund only for edited risk in that version
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_SIR_Get_reversal_risks",
                                                  sSQLName:="spu_SIR_Get_reversal_risks",
                                                  bStoredProcedure:=True,
                                                  vResultArray:=vDataArray)
                If Informations.IsArray(vDataArray) Then
                    sRiskStatus = vDataArray(1, i)
                End If
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsArray(vDataArray) Then
                'Should always have a least one risk
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If



            ReDim vSelectionArray(1, vDataArray.GetUpperBound(1))


            For i = 0 To vDataArray.GetUpperBound(1)
                'WPR 33-75 added
                bIsEdited = True
                sRiskMergeStatus = ""
                If v_bIsBackdatedMTA = False Then
                    lRiskId = CInt(vDataArray(ACIRiskId, i))
                    sRiskStatus = vDataArray(ACIRiskStatusFlag, i).ToString.ToUpper
                Else
                    lRiskId = vDataArray(0, i)
                    sRiskStatus = vDataArray(1, i).ToString.ToUpper
                    oArray = Nothing
                    'AndAlso wasn't edited in original version; little tricky to establish when multiple OOS done on same policy
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nOriginal_insurance_file_cnt", vValue:=v_lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nRisk_cnt", vValue:=lRiskId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    'Get edited flag for version being cancelled and generate refund only for edited risk in that version
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_SIR_Get_original_risk_status",
                                                      sSQLName:="spu_SIR_Get_original_risk_status",
                                                      bStoredProcedure:=True,
                                                      vResultArray:=oArray)
                    If Informations.IsArray(oArray) = True Then
                        If m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                            ' cancel risk if edited in any of the original version; deleted risks must be reinstated back as refund should come from previous live version
                            If (ToSafeInteger(oArray(0, 0)) = 0) Then
                                bIsEdited = False
                            ElseIf (ToSafeString(oArray(2, 0)) = "D") And m_sTransactionType = "MTR" Then
                                sRiskStatus = "D"
                                sRiskMergeStatus = ACRStatusDeletedPostChange
                            End If
                        Else
                            ' Ignore if not edited in base version and original version and wasn't deleted in original version
                            If (ToSafeInteger(oArray(0, 0)) = 0 Or ToSafeInteger(vDataArray(2, i)) = 0) And ToSafeString(oArray(2, 0)) <> "D" Then
                                bIsEdited = False
                            End If
                        End If
                    Else
                        bIsEdited = False
                    End If
                End If

                'WPR 33-75 added
                'If CStr(vDataArray(ACIRiskStatusFlag, i)) = "U" Or CStr(vDataArray(ACIRiskStatusFlag, i)) = "D" Then
                If (sRiskStatus = "U" OrElse sRiskStatus = "D") AndAlso (bIsEdited OrElse v_bCopyRiskOnMTA) AndAlso v_bIsBackdatedMTA Then

                    m_lReturn = ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lBaseInsuranceFileCnt:=If(v_sTransactionType = "MTCR", ToSafeLong(oArray(1, 0)), v_lBaseInsuranceFileCnt), v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, nIsRiskEdited:=1, v_sRiskMergeStatus:=sRiskMergeStatus)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                ElseIf (sRiskStatus = "U" OrElse sRiskStatus = "D") AndAlso bIsEdited = True AndAlso v_bIsBackdatedMTA = False Then
                    m_lReturn = ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, nIsRiskEdited:=1, v_sRiskMergeStatus:=sRiskMergeStatus)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    'The risk has already been copied and quoted
                    lNewRiskCnt = lRiskId
                    iSelected = 1
                End If

                'Calculate product fees on policy cancellation
                If v_sTransactionType = "MTC" Then
                    m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
                    m_lReturn = m_oSIRPartyFee.RecalculatePolicyFees(
                                                v_lTransactionTypeId:=7,
                                                v_lProductId:=-1,
                                                v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oSIRPartyFee.RecalculatePolicyFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If


                'If this has quoted set status to selected

                vSelectionArray(0, i) = lNewRiskCnt
                vSelectionArray(1, i) = iSelected
                If v_sTransactionType = "MTCR" Then
                    ' Reverse tax at risk level
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nOldRiskCnt", vValue:=lRiskId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewRiskCnt", vValue:=lNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=kReverseRiskLevelTaxesSQL, sSQLName:=kReverseRiskLevelTaxesName, bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_risk_tax_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Reverse fee at risk level
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nOldRiskCnt", vValue:=lRiskId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewRiskCnt", vValue:=lNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=kReverseRiskLevelFeeSQL, sSQLName:=kReverseRiskLevelFeeName, bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_sir_risk_fee_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Next i

            'Update the risk selection status
            m_lReturn = m_oListRisks.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListRisks.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If v_sTransactionType = "MTCR" Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=v_lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_agent_commission_rev", sSQLName:="Reverse Comm", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_sir_agent_commission_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' reverse fee 
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=v_lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_sir_policy_fee_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Reverse Tax at Policy Level
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nOldInsuranceFileCnt", vValue:=v_lBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewInsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                m_lReturn = m_oDatabase.SQLAction(sSQL:=kReversePolicyLevelTaxesSQL, sSQLName:=kReversePolicyLevelTaxesName, bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_Insurance_file_tax_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Call the agent commission component
                m_lReturn = ProcessAgentCommission(
                            v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListRisks.ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Policy level tax
            If v_sTransactionType <> "MTCR" Then
                m_lReturn = ProcessRITax(
                        v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                        v_lRiskCnt:=0)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListRisks.ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessRisksWithMerge
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessRisksWithMerge(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByVal v_dtMTAStartDate As Date, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vDataArray As Object
            Dim sFailureReason As String = ""
            Dim lNewRiskCnt As Integer
            Dim vSelectionArray(,) As Object
            Dim iSelected As Integer
            Dim lCurrentRiskId, lPostChangeRiskCnt As Integer
            Dim sRiskMergeStatus As String = ""
            Dim lRiskCount, lFindRiskArrayIndex As Integer

            Dim bExistsPreAndPost As Boolean
            Dim oArray(,) As Object
            Dim nTransactionTypeId As Integer
            m_oAutoMTAMerge = New AutoMTAMerge()

            'm_lReturn = CType(m_oAutoMTAMerge, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            m_lReturn = m_oAutoMTAMerge.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)


            'Get a list of all the risks
            With m_oAutoMTAMerge
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .PreChangeInsFileCnt = v_lPreChangeInsFileCnt
                .PostChangeInsFileCnt = v_lPostChangeInsFileCnt
                .CurrentInsFileCnt = v_lInsuranceFileCnt
                .BaseInsFileCnt = m_lNewInsuranceFileCnt
                .MTAStartDate = v_dtMTAStartDate



                'developer guide no. 24
                .FindRisk = m_oFindRisk

                m_lReturn = .GetListOfRisks()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTAMerge.GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            lRiskCount = m_oAutoMTAMerge.RiskCount
            If lRiskCount = -1 Then
                'Should always have a least one risk
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ReDim vSelectionArray(1, lRiskCount)
            For i As Integer = 0 To lRiskCount

                m_oAutoMTAMerge.CurrentRiskIndex = i


                vDataArray = m_oAutoMTAMerge.FindRiskArray
                lFindRiskArrayIndex = m_oAutoMTAMerge.FindRiskArrayIndex
                'WPR 33-75 added
                Dim sCopyRiskOnMTA As String = ""
                m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, v_vBranch:=1, r_vUnderwriting:=sCopyRiskOnMTA)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oAutoMTAMerge.MergeStatus <> ACRStatusNoProcess OrElse sCopyRiskOnMTA = "1" Then
                    If m_oAutoMTAMerge.MergeStatus = gSIRLibrary.ACRStatusAddPostChange Then
                        lCurrentRiskId = m_oAutoMTAMerge.PostChangeRiskCnt
                    Else
                        lCurrentRiskId = m_oAutoMTAMerge.CurrentRiskCnt
                    End If
                    'lPreChangeRiskCnt = m_oAutoMTAMerge.PreChangeRiskCnt
                    lPostChangeRiskCnt = m_oAutoMTAMerge.PostChangeRiskCnt
                    'sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus

                    bExistsPreAndPost = Not ((m_oAutoMTAMerge.PreChangeRiskCnt) = 0) And Not ((m_oAutoMTAMerge.PostChangeRiskCnt) = 0)
                    lNewRiskCnt = 0
                    m_lReturn = ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lCurrentRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, v_lPostChangeRiskCnt:=lPostChangeRiskCnt, v_bExistsPreAndPost:=bExistsPreAndPost)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'If this has quoted set status to selected

                    'vSelectionArray(1, i + 1) = lNewRiskCnt
                    vSelectionArray(0, i) = lNewRiskCnt
                    If m_oAutoMTAMerge.MergeStatus = ACRStatusAddPostChange And lNewRiskCnt = 0 Then
                        lNewRiskCnt = lCurrentRiskId
                    End If
                    m_lReturn = UpdateRisksInRiskFolder(
                            nBaseInsuranceFileCnt:=m_lNewInsuranceFileCnt,
                            nNewInsuranceFileCnt:=v_lInsuranceFileCnt,
                            nOriginalInsuranceFileCnt:=v_lPostChangeInsFileCnt,
                            nPreChangeInsuranceFileCnt:=v_lPreChangeInsFileCnt,
                            nOldRiskCnt:=lCurrentRiskId,
                            nNewRiskCnt:=lNewRiskCnt)


                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="UpdateRisksInRiskFolder Failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="ProcessRisksWithMerge")
                        ProcessRisksWithMerge = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    m_oAutoMTAMerge.NewRiskCnt = lNewRiskCnt
                    'vSelectionArray(2, i + 1) = iSelected
                    'WPR 33-75 added
                    ' vSelectionArray(1, i) = iSelected
                    vSelectionArray(1, i) = 1
                    m_oAutoMTAMerge.NewRiskCnt = lNewRiskCnt
                Else
                    lCurrentRiskId = m_oAutoMTAMerge.CurrentRiskCnt
                    lNewRiskCnt = m_oAutoMTAMerge.CurrentRiskCnt

                    m_lReturn = UpdateRisksInRiskFolder(
                                    nBaseInsuranceFileCnt:=m_lNewInsuranceFileCnt,
                                    nNewInsuranceFileCnt:=v_lInsuranceFileCnt,
                                    nOriginalInsuranceFileCnt:=v_lPostChangeInsFileCnt,
                                    nPreChangeInsuranceFileCnt:=v_lPreChangeInsFileCnt,
                                    nOldRiskCnt:=lCurrentRiskId,
                                    nNewRiskCnt:=lNewRiskCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="UpdateRisksInRiskFolder Failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="ProcessRisksWithMerge")
                        ProcessRisksWithMerge = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            Next i

            'Update the risk selection status

            m_lReturn = m_oListRisks.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListRisks.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If v_bIsBackdatedMTA = True Then

                m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessRisksWithMerge = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                ' copy fee\comm rates from original version
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=v_lPostChangeInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_agent_commission_rev", sSQLName:="Reverse Comm", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessRisksWithMerge = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                ' reverse fee
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_insurance_File_Cnt", vValue:=v_lPostChangeInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_File_Cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_policy_fees_copy", sSQLName:="Reverse Fee", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessRisksWithMerge = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                oArray = Nothing

                nTransactionTypeId = 9
                m_lReturn = m_oDatabase.SQLSelect("Select ISNULL(insurance_file_type_id, 0) From insurance_file Where insurance_file_cnt = " & v_lInsuranceFileCnt, "Get Type", False, , oArray)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ProcessRisksWithMerge = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                Else
                    If Informations.IsArray(oArray) Then
                        If oArray(0, 0) = 11 Then
                            nTransactionTypeId = 7
                        ElseIf oArray(0, 0) = 10 Then
                            nTransactionTypeId = 20
                        ElseIf oArray(0, 0) = 3 Then
                            nTransactionTypeId = 10
                        End If
                    End If
                End If

                If m_sTransactionType <> "MTCR" And nTransactionTypeId <> 0 Then
                    ' Recalculate the Policy taxes
                    m_lReturn = m_oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                                 v_lProductId:=-1,
                                                                 v_lTransactionTypeId:=nTransactionTypeId,
                                                                 v_bUseExistingFeeDetail:=False)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="RecalculatePolicyFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
                        Return result
                    End If
                End If

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisksWithMerge Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessRisksForNCDChange
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessRisksForNCDChange(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vDataArray(,) As Object
        Dim sFailureReason As String = ""
        Dim lNewRiskCnt As Integer
        Dim vSelectionArray(,) As Object
        Dim iSelected As Integer
        Dim lRiskId As Integer

        'Get a list of all the risks
        m_lReturn = GetListOfRisks(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vDataArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vDataArray) Then
            'Should always have a least one risk
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'Delete the original insurance_file_risk_entries for the copied
        'unchanged risk links

        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.DeleteInsuranceFileRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        v_sTransactionType = "MTACR"


        ReDim vSelectionArray(1, vDataArray.GetUpperBound(1))


        For i As Integer = 0 To vDataArray.GetUpperBound(1)


            lRiskId = CInt(vDataArray(ACIRiskId, i))

            'If vDataArray(ACIRiskStatusFlag, i) = "U" Then
            m_lReturn = ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=DateTime.Now, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Else
            'The risk has already been copied and quoted
            '   lNewRiskCnt = lRiskId
            '  iSelected = 1
            'End If

            'If this has quoted set status to selected

            vSelectionArray(1, i + 1) = lNewRiskCnt

            vSelectionArray(2, i + 1) = iSelected
        Next i

        'Update the risk selection status

        m_lReturn = m_oListRisks.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListRisks.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function



    ' ***************************************************************** '
    '
    ' Name: ProcessRisksForRiskReinstatement
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessRisksForRiskReinstatement(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtMTAStartDate As Date, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vDataArray(,) As Object
        Dim sFailureReason As String = ""
        Dim lNewRiskCnt As Integer
        Dim vSelectionArray(,) As Object
        Dim iSelected As Integer
        Dim lRiskId As Integer
        Dim bProcess As Boolean
        Dim lUbound1, lUbound2 As Integer
        Dim bUpdateDeletedRiskCnt As Boolean
        Dim sTransactionType, sDescription As String


        'Get a list of all the risks
        m_lReturn = GetListOfRisks(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vDataArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Delete the original insurance_file_risk_entries for the copied
        'unchanged risk links

        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.DeleteInsuranceFileRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vDataArray) Then
            'Should always have a least one risk
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        lUbound1 = vDataArray.GetUpperBound(0)

        lUbound2 = vDataArray.GetUpperBound(1)

        If v_lPostChangeInsFileCnt <> m_lDeletedRiskInsuranceFileCnt Then
            ReDim Preserve vDataArray(lUbound1, lUbound2 + 1)

            vDataArray(ACIRiskStatusFlag, lUbound2 + 1) = "C"

            vDataArray(ACIRiskId, lUbound2 + 1) = m_lLastDeletedRiskCnt
        End If


        ReDim vSelectionArray(1, vDataArray.GetUpperBound(1))


        For i As Integer = 0 To vDataArray.GetUpperBound(1)


            lRiskId = CInt(vDataArray(ACIRiskId, i))

            bProcess = False
            bUpdateDeletedRiskCnt = False

            If v_lPostChangeInsFileCnt = m_lDeletedRiskInsuranceFileCnt Then

                If CStr(vDataArray(ACIRiskStatusFlag, i)) = "C" Then

                    bProcess = True
                    'This is an unchanged risk so we will just copy the original sections
                    sTransactionType = "MTACR"

                ElseIf CStr(vDataArray(ACIRiskStatusFlag, i)) = "U" Then


                    m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskId, v_sStatusFlag:="U")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oRiskData.AddRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
                        Return result
                    End If

                ElseIf CStr(vDataArray(ACIRiskStatusFlag, i)) = "D" And lRiskId = m_lDeletedRiskCnt Then

                    'Copy the deleted risk as well
                    bUpdateDeletedRiskCnt = True
                    bProcess = True
                    'This is the deleted risk so we need to copy the sections from the
                    'risk the deleted version one was created from but use the premiums
                    'from the deleted risk reversed
                    sTransactionType = "MTADR"

                End If
            Else


                If CStr(vDataArray(ACIRiskStatusFlag, i)) = "C" Then
                    bProcess = True
                Else

                    m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskId, v_sStatusFlag:="U")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oRiskData.AddRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
                        Return result
                    End If
                End If
                If i = lUbound2 + 1 Then
                    'The last risk will always be the deleted risk that we are restoring
                    'which has been tagged on to the end of the array
                    v_lPostChangeInsFileCnt = m_lLastDeletedInsuranceFileCnt
                    bUpdateDeletedRiskCnt = True
                    'This is the deleted risk we have copied so requote it when
                    'creating the new one with the appropriate rates
                    sTransactionType = "MTA"
                Else
                    'This is an unchanged risk so we will just copy the original sections
                    sTransactionType = "MTACR"
                End If
            End If

            If bProcess Then
                m_lReturn = ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=False, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If bUpdateDeletedRiskCnt Then
                    'If this was the deleted risk or a new copy of it
                    m_lLastDeletedRiskCnt = lNewRiskCnt
                    m_lLastDeletedInsuranceFileCnt = v_lInsuranceFileCnt

                    ' RAW 26/09/2003 : CQ828 : moved from AutoRunMTAOnly added
                    ' more details to event description replaced call to
                    ' m_oFindInsurance.CreateEvent with my own


                    sDescription = "Reinstated Risk: " &
                                   CStr(vDataArray(ACIRiskNo, i)) & " " &
                                   CStr(vDataArray(ACIRiskTypeDescription, i)) & " " &
                                   m_sReinstatementReason

                    m_lReturn = CreateEvent(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sReason:=sDescription)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else
                'The risk has already been copied and quoted
                lNewRiskCnt = lRiskId
                iSelected = 1
            End If

            'If this has quoted set status to selected

            vSelectionArray(1, i + 1) = lNewRiskCnt

            vSelectionArray(2, i + 1) = iSelected
        Next i

        'Update the risk selection status

        m_lReturn = m_oListRisks.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListRisks.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessSingleRisk
    '
    ' Description:
    '
    ' History: 20/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessSingleRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskId As Integer, ByVal v_iRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_iSelected As Integer, ByVal v_sTransactionType As String, ByVal v_dtMTAStartDate As Date, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal v_lPostChangeRiskCnt As Integer = 0, Optional ByVal v_bExistsPreAndPost As Boolean = False, Optional ByVal nIsRiskEdited As Integer = 0, Optional ByVal v_sRiskMergeStatus As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sFailureReason, sRiskMergeStatus As String
        'WPR 33-75 added
        Dim bRiskEdited As Boolean
        Dim vArray(,) As Object
        Dim sCopyRiskOnMTA As String = ""
        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, v_vBranch:=1, r_vUnderwriting:=sCopyRiskOnMTA)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus
        If Not (m_oAutoMTAMerge Is Nothing) And v_sTransactionType = "MTA" AndAlso sCopyRiskOnMTA <> "1" Then
            sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus
        Else
            sRiskMergeStatus = v_sRiskMergeStatus
        End If

        'Create a copy of the risk data
        ' RAW 15/11/2003 : Pricing changes : added r_vRiskDetailsArray and r_lRiskDetailsArrayIndex param
        m_lReturn = CopyRiskData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskCnt:=v_lRiskId, r_lNewRiskCnt:=r_lNewRiskCnt, r_sFailureReason:=sFailureReason, v_sTransactionType:=v_sTransactionType, v_sRiskMergeStatus:=sRiskMergeStatus, v_lOriginalFlag:=1, v_bDeleteOriginalRiskLink:=True, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'WPR 33-75 added
        bRiskEdited = True
        If m_bIsInteractive And v_sTransactionType = "MTA" Then
            If m_oAutoMTAMerge.MergeStatus = ACRStatusAddPostChange Then
                ' must be pointed; exit out
                Return m_lReturn
            End If
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect("Select ISNULL(is_risk_edited, 0) From insurance_file_risk_link Where status_flag <> 'U' AND risk_cnt = " & v_lPostChangeRiskCnt, "Select risk_edited", False, , vArray)

            If Informations.IsArray(vArray) Then
                If vArray(0, 0) = 0 And ToSafeString(sRiskMergeStatus) <> ACRStatusDeletedPostChange Then
                    bRiskEdited = False
                End If
            End If
        End If

        'WPR 33-75 added
        If bRiskEdited = True Then
            ' Update is_risk_edited from insurance_file_risk_link
            m_lReturn = m_oListRisks.UpdateIFRLInkRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskID:=r_lNewRiskCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lPostChangeRiskCnt > 0 Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.SQLAction("Update risk Set description = (Select description From risk Where risk_cnt = " & v_lPostChangeRiskCnt & ") Where risk_cnt = " & r_lNewRiskCnt, "Update risk", False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If r_lNewRiskCnt > 0 Then
            'Now do the quote
            ' RAW 15/11/2003 : Pricing changes : added r_vRiskDetailsArray and v_lRiskDetailsArrayIndex param
            m_lReturn = QuoteRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=r_lNewRiskCnt, v_iRiskNo:=v_iRiskCnt, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, v_lPostChangeRiskCnt:=v_lPostChangeRiskCnt, v_bExistsPreAndPost:=v_bExistsPreAndPost)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Dim nFacRiskId As Integer
            If (v_sTransactionType = "MTA" And v_bIsBackdatedMTA = True) Then

                If m_oAutoMTAMerge.MergeStatus = ACRStatusMerge Then
                    nFacRiskId = m_oAutoMTAMerge.PostChangeRiskCnt
                Else
                    nFacRiskId = 0
                End If
            End If
            'WPR 33-75 added
            If Not (m_sTransactionType = "MTA" And v_bIsBackdatedMTA = True) Or v_sTransactionType = "MTCR" Then
                'Do risk level reinsurance

                m_lReturn = ProcessRiskReinsurance(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=r_lNewRiskCnt, v_sTransactionType:=v_sTransactionType, lFac_risk_cnt:=nFacRiskId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRiskReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Calculate Risk fees on policy cancellation
            If v_sTransactionType = "MTC" Then
                m_lReturn = m_oSIRPartyFee.RecalculateRiskFees(
                                                        v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                        v_lTransactionTypeId:=7,
                                                        v_lRiskCnt:=r_lNewRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oSIRPartyFee.RecalculateRiskFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Do risk level tax
            m_lReturn = ProcessRITax(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=r_lNewRiskCnt, sTransactionType:=m_sTransactionType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        r_iSelected = 1

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetListOfRisks
    ' Description:
    ' History: 08/01/2003 sj - Created.
    ' ***************************************************************** '
    Private Function GetListOfRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=r_vResultArray, v_vInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: ApplyRiskChanges
    '
    ' Description:
    '
    ' History: 19/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ApplyRiskChanges(ByVal v_lRiskId As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sObjectName, sPropertyName As String
        Dim vValue As String = ""
        Dim vOIKeyArray As Object

        If Not Informations.IsArray(m_vObjectPropertyArray) Then
            Return result
        End If

        For i As Integer = 0 To m_vObjectPropertyArray.GetUpperBound(1)

            sObjectName = CStr(m_vObjectPropertyArray(ACOPObjectName, i))
            sPropertyName = CStr(m_vObjectPropertyArray(ACOPPropertyName, i))
            vValue = CStr(m_vObjectPropertyArray(ACOPValue, i))


            m_lReturn = m_oDataset.GetAllOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.GetAllOIKey Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyRiskChanges")
                Return result
            End If
            If Informations.IsArray(vOIKeyArray) Then


                m_lReturn = m_oDataset.SetPropertyValue(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=CStr(vOIKeyArray(0)), v_vPropertyValue:=vValue, v_bIsAssumedInfo:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.SetPropertyValue Failed for " & sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyRiskChanges")
                    Return result
                End If
            End If
        Next i

        'oObject.risk.Item("test").Item("SumInsuredTest").Value = lSumInsured


        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: MergeExistingMTAChanges
    '
    ' Description:
    '
    ' History: 07/02/2003 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (MergeExistingMTAChanges) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function MergeExistingMTAChanges(ByVal v_lPreChangeRiskCnt As Integer, ByVal v_lPostChangeRiskCnt As Integer, ByVal v_lCurrentRiskCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: QuoteRisk
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    ' RAW 15/11/2004 : Pricing Changes : added r_vRiskDetailsArray and v_lRiskDetailsArrayIndex  param
    ' ***************************************************************** '
    Private Function QuoteRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_iRiskNo As Integer, ByVal v_sTransactionType As String, ByVal v_dtMTAStartDate As Date, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal v_lPostChangeRiskCnt As Integer = 0, Optional ByVal v_bExistsPreAndPost As Boolean = False) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sGisDataModelCode As String = ""
        Dim lTransactionType As Integer
        Dim lQuoteType As Integer
        Dim sDeclineReasons, sReferReasons, sMessages, sXMLDataSetDef, sXMLDataSet As String
        Dim vRatingSectionArray As Object
        Dim cTotalAnnualTax As Decimal
        Dim sSysOptionBDMTA As String = ""
        Dim sRiskMergeStatus As String


        m_oPerilAllocation.TransactionType = v_sTransactionType


        m_oPerilAllocation.InsuranceFolderCnt = m_lInsuranceFolderCnt

        m_oPerilAllocation.InsuranceFileCnt = v_lInsuranceFileCnt

        m_oPerilAllocation.RiskID = v_lRiskCnt

        If (m_oAutoMTAMerge Is Nothing = False And v_sTransactionType = "MTA") Then
            sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus
        Else
            sRiskMergeStatus = ""
        End If

        Dim bIsMandatoryRisk As Boolean = False
        m_lReturn = m_oPerilAllocation.CheckMandatoryRisk(v_lRiskCnt, bIsMandatoryRisk)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Cancelling/reinstating or copying existing risks - no need to rate
        If ((v_sTransactionType <> "MTC") And (v_sTransactionType <> "MTR") And (v_sTransactionType <> "MTCR") And (v_sTransactionType <> "MTADR") And (v_sTransactionType <> "MTACR")) OrElse (IsCalledFromPlanMaintainence AndAlso bIsMandatoryRisk) Then


            m_lReturn = m_oPerilAllocation.GetDataModel(sGISDataModel:=sGisDataModelCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.GetDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lTransactionType = m_oPerilAllocation.TransactionTypeId

            'Merge in the risk details from the original MTA
            ''''''''''''!!!!!!!!!!!!Very Very Important!!!!!!!!!'''''''
            ''!!!Merge only those risk those have been edited in successive versions else carry forward the changes of the risk!!!'''


            'get system option "Apply Back-Dated Risk Editing Restrictions"

            'WPR 33-75 added
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=CInt("5079"), r_sOptionValue:=sSysOptionBDMTA)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("QuoteRisk", "Failed to get System Option [5056]", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'WPR 33-75 added
            'If sSysOptionBDMTA = "1" Then
            '    
            '    lReturn = m_oListRisks.IsSubsequentRiskVersionsEdited(v_lRiskId:=v_lRiskCnt, v_dtMTAEffectiveDate:=m_dtEffectiveDate)
            'End If


            'If m_bMergeRisks And Not (m_oAutoMTAMerge Is Nothing) And lReturn = gPMConstants.PMEReturnCode.PMTrue Then

            If m_bIsInteractive = False And m_bMergeRisks = True And m_oAutoMTAMerge Is Nothing = False Then


                'developer guide no. 24
                m_oAutoMTAMerge.Gis = m_oGIS
                m_oAutoMTAMerge.DataModelCode = sGisDataModelCode
                m_oAutoMTAMerge.NewRiskCnt = v_lRiskCnt


                'developer guide no. 24
                m_oAutoMTAMerge.Dataset = m_oDataset

                m_lReturn = m_oAutoMTAMerge.MergeExistingMTAChanges()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oAutoMTAMerge.MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return result
                End If

                sXMLDataSetDef = m_oAutoMTAMerge.XMLDataSetDef
                sXMLDataSet = m_oAutoMTAMerge.XMLDataSet

            Else

                m_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=sGisDataModelCode, v_lInsuranceFileCnt:=m_lInsuranceFolderCnt, v_lRiskID:=v_lRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.LoadFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If v_bApplyRiskChange Then
                ' Load Data as XML
                m_lReturn = m_oDataset.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Apply a fixed change to the risk
                m_lReturn = ApplyRiskChanges(v_lRiskId:=v_lRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyRiskChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Return as XmL String
                m_lReturn = m_oDataset.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            '        lQuoteType = (lTransactionType * 10000) + 1
            EncodeTransactionScreenAndType(lQuoteType, lTransactionType, 0, 1)

            ' RAW 28/04/2004 : CQ5143 : added
            ' clear existing quote entries from within the RISK node before requoting

            m_lReturn = m_oGIS.ClearPBQuoteOutputs(v_sGisDataModelCode:=sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oGis.ClearPBQuoteOutputs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 28/04/2004 : CQ5143 : end


            'Call NBQuote on bGIS.
            ' Sasria : Pass isBackdatedMTA into m_oGIS.NBQuote
            'WPR 33-75 added
            '  m_lReturn = m_oGIS.NBQuote(v_sGisDataModelCode:=sGisDataModelCode, v_lQuoteType:=lQuoteType, v_sGISBusinessTypeCode:="NB", v_dtEffectiveDate:=v_dtMTAStartDate, r_sXMLDataset:=sXMLDataSet, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)
            m_lReturn = m_oGIS.NBQuote(v_sGisDataModelCode:=sGisDataModelCode, v_lQuoteType:=lQuoteType, v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=v_dtMTAStartDate, r_sXMLDataset:=sXMLDataSet, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save it to the DataBase

            m_lReturn = m_oGIS.SaveToDB(v_sGisDataModelCode:=sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'Populate sections and perils
        Select Case v_sTransactionType
            Case "MTCR", "MTR"
                ' RAW 05/05/2004 : CQ753 : added r_vResultArray param

                m_lReturn = m_oPerilAllocation.PopulateRatingSectionsFromExistingSections(v_lPreviousDeletedRiskInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_vResultArray:=vRatingSectionArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)
            Case "MTADR", "MTACR"

                m_lReturn = m_oPerilAllocation.PopulateRatingSectionsFromExistingSections(v_lPreviousDeletedRiskInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vRatingSectionArray)
            Case Else

                m_lReturn = m_oPerilAllocation.PopulateRatingSections(r_vResultArray:=vRatingSectionArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, r_lPostChangeRiskCnt:=v_lPostChangeRiskCnt, v_dtMTADateCurrent:=m_dtEffectiveDate, v_bExistsPreAndPost:=v_bExistsPreAndPost,
                     v_sRiskMergeStatus:=sRiskMergeStatus)
        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.PopulateRatingSections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RAW 05/05/2004 : CQ753 : added
        ' only call this to get tax

        m_lReturn = m_oPerilAllocation.RecalculatePremium(r_vRatingSection:=vRatingSectionArray, r_cTotalAnnualTax:=cTotalAnnualTax)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.RecalculatePremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oPerilAllocation.AnnualTaxTotal = cTotalAnnualTax
        ' RAW 05/05/2004 : CQ753 : end


        'Store any decline or referral messages
        m_lReturn = StoreMessagesReferalsAndDeclines(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=v_lRiskCnt, v_sDeclineReasons:=sDeclineReasons, v_sReferReasons:=sReferReasons, v_sMessages:=sMessages)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="StoreMessagesReferalsAndDeclines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return result
        End If

        'Update risk with values from sections an perils

        m_lReturn = m_oPerilAllocation.UpdateRisk
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.UpdateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: StoreMessagesReferalsAndDeclines
    '
    ' Description:
    '
    ' History: 29/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function StoreMessagesReferalsAndDeclines(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, Optional ByVal v_sDeclineReasons As String = "", Optional ByVal v_sReferReasons As String = "", Optional ByVal v_sMessages As String = "", Optional ByVal v_sPolicyStatusMessages As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lUbound As Integer

        'Declines
        If v_sDeclineReasons <> "" Then
            If Not Informations.IsArray(m_vDeclineReasons) Then
                lUbound = 0
                ReDim m_vDeclineReasons(ACDRArraySize, lUbound)
            Else

                lUbound = m_vDeclineReasons.GetUpperBound(1) + 1
                ReDim Preserve m_vDeclineReasons(ACDRArraySize, lUbound)
            End If


            m_vDeclineReasons(ACDRInsuranceFileCnt, lUbound) = v_lInsuranceFileCnt

            m_vDeclineReasons(ACDRRiskCnt, lUbound) = v_lRiskCnt

            m_vDeclineReasons(ACDRType, lUbound) = ACDRTypeCodeDecline

            m_vDeclineReasons(ACDRReason, lUbound) = v_sDeclineReasons

        End If

        'Referrals
        If v_sReferReasons <> "" Then
            If Not Informations.IsArray(m_vReferReasons) Then
                lUbound = 0
                ReDim m_vReferReasons(ACDRArraySize, lUbound)
            Else

                lUbound = m_vReferReasons.GetUpperBound(1) + 1
                ReDim Preserve m_vReferReasons(ACDRArraySize, lUbound)
            End If


            m_vReferReasons(ACDRInsuranceFileCnt, lUbound) = v_lInsuranceFileCnt

            m_vReferReasons(ACDRRiskCnt, lUbound) = v_lRiskCnt

            m_vReferReasons(ACDRType, lUbound) = ACDRTypeCodeRefer

            m_vReferReasons(ACDRReason, lUbound) = v_sReferReasons

        End If

        'Messages
        If v_sMessages <> "" Then
            If Not Informations.IsArray(m_vMessages) Then
                lUbound = 0
                ReDim m_vMessages(ACDRArraySize, lUbound)
            Else
                lUbound = m_vMessages.GetUpperBound(1) + 1
                ReDim Preserve m_vMessages(ACDRArraySize, lUbound)
            End If

            m_vMessages(ACDRInsuranceFileCnt, lUbound) = v_lInsuranceFileCnt
            m_vMessages(ACDRRiskCnt, lUbound) = v_lRiskCnt
            m_vMessages(ACDRType, lUbound) = ACDRTypeCodeMessage
            m_vMessages(ACDRReason, lUbound) = v_sMessages

        End If

        'Policy Status Messages
        If v_sPolicyStatusMessages <> "" Then
            If Not Informations.IsArray(m_vPolicyStatusMessages) Then
                lUbound = 0
                ReDim m_vPolicyStatusMessages(ACDRArraySize, lUbound)
            Else
                lUbound = m_vPolicyStatusMessages.GetUpperBound(1)
                ReDim Preserve m_vPolicyStatusMessages(ACDRArraySize, lUbound)
            End If

            m_vPolicyStatusMessages(ACDRInsuranceFileCnt, lUbound) = v_lInsuranceFileCnt
            m_vPolicyStatusMessages(ACDRRiskCnt, lUbound) = v_lRiskCnt
            m_vPolicyStatusMessages(ACDRType, lUbound) = ACDRTypeCodeMessage
            m_vPolicyStatusMessages(ACDRReason, lUbound) = v_sMessages

        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: EncodeTransactionScreenAndType
    '
    ' Description: Encodes Transaction, Screen id and tYpe from encoded value
    '              Originally TTTSSYY
    '              Now        1TTTSSSSYY
    '
    ' History: 19/12/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Integer, ByRef r_lGISScreenId As Byte, ByRef r_lQuoteType As Byte)

        Try

            'new format 1TTTSSSSYY
            r_lEncoded = 1000000000 + (r_lTransactionType * 1000000) + (r_lGISScreenId * 100) + r_lQuoteType

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeTransactionScreenAndType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: CopyRiskData - PW311002
    '
    ' Desc: copy a specific risk
    '       copy all GIS details attached
    '       based on CopyRiskData in iPMURenSelection
    '       20/12/2002 - sj Add Original Flag

    ' ***************************************************************** '
    Private Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String, ByVal v_sTransactionType As String, Optional ByVal v_sRiskMergeStatus As String = "", Optional ByVal v_lOriginalFlag As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bDeleteOriginalRiskLink As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean
        Dim lNewGisPolicyLinkID As Integer
        Dim vGisPolicyLinkArray, vRiskArray(,) As Object
        Dim lCount As Integer
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lInsuranceFileCnt As Integer
        Dim bCreateDeletedRiskLink, bKeepOriginalRiskCnt As Boolean
        'WPR 33-75 added
        Dim vArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lDeletedRiskInsuranceFileCnt > 0 And v_sTransactionType.StartsWith("MTA") Then
            '  If m_bIsReinstateRisk = True Then
            lInsuranceFileCnt = v_lPostChangeInsFileCnt
        Else
            lInsuranceFileCnt = v_lInsuranceFileCnt
        End If

        If m_bMergeRisks And v_sRiskMergeStatus = gSIRLibrary.ACRStatusAddPostChange Then
            lInsuranceFileCnt = v_lPostChangeInsFileCnt
        End If

        If m_bIsNCDChange And v_sTransactionType = "MTACR" Then
            lInsuranceFileCnt = v_lPostChangeInsFileCnt
        End If
        ' Get all risks associate with the InsuranceFileCnt

        m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResultArray:=vRiskArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "Getting Risk"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check if we have any risks
        If Not Informations.IsArray(vRiskArray) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No risks found", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Find the risk that matches the passed risk count, i.e. the one we want
        ' to copy
        bFound = False

        For lCount = 0 To vRiskArray.GetUpperBound(1)

            If CDbl(vRiskArray(0, lCount)) = v_lRiskCnt Then
                bFound = True
                Exit For
            End If
        Next

        ' Check if we have found the risk to copy
        If Not bFound Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot find risk to copy", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "Cannot find risk to copy"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_sRiskMergeStatus = gSIRLibrary.ACRStatusDeletedPostChange Then
            'This risk was deleted post change so create the insurance_file_risk_link
            'record with a status of deleted
            bCreateDeletedRiskLink = True
            vRiskArray(29, lCount) = "DP"
        Else
            bCreateDeletedRiskLink = False
        End If

        Dim sCopyRiskOnMTA As String = ""
        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTCopyRiskInMTA, v_vBranch:=1, r_vUnderwriting:=sCopyRiskOnMTA)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_sRiskMergeStatus = ACRStatusAddPostChange AndAlso sCopyRiskOnMTA <> "1" Then
            bKeepOriginalRiskCnt = False
            ' point and exit
            m_lReturn = m_oRiskData.AddRiskLink(
                        v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt,
                        v_lRiskCnt:=v_lRiskCnt, v_sStatusFlag:="U")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRiskData = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername,
                           iType:=gPMConstants.PMEReturnCode.PMError,
                           sMsg:="m_oRiskData.AddRiskLink Failed",
                           vApp:=ACApp,
                           vClass:=ACClass,
                           vMethod:="CopyRiskData")
            End If
            Return PMEReturnCode.PMTrue
        Else
            bKeepOriginalRiskCnt = True
        End If
        ' Copy risk with same insurance file cnt

        m_lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=r_lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_bAutoCancellation:=True, v_sRiskMergeStatus:=If(ToSafeString(vRiskArray(29, lCount)) = "R", "A", v_sRiskMergeStatus))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "Copy Risk"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'WPR 33-75 added
        'Need a bit of tweaking here as we want to keep data of future version
        If m_bIsInteractive = True And v_sTransactionType = "MTA" And v_sRiskMergeStatus <> ACRStatusAddPostChange Then
            ' get post risk_cnt
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="iFileCnt", vValue:=v_lPostChangeInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=vRiskArray(ACRiskPosCnt, lCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPostRiskCntSQL, sSQLName:=ACGetPostRiskCntName, bStoredProcedure:=False, vResultArray:=vArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("GetOutOfSequenceMTAProductDetails", "Failed to fetch post risk_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vArray) Then
                vRiskArray(ACRiskPosCnt, lCount) = vArray(0, 0)
            End If

        End If
        ' Prepare details to copy GIS Stuff attached to current risk

        ' Get policy link detail
        m_lReturn = m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=vRiskArray(ACRiskPosCnt, lCount), r_vResultArray:=vGisPolicyLinkArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetGISPolicyLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "GetGISPolicyLink"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any data?
        Dim auxVar As Object = vGisPolicyLinkArray(0, 0)


        If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then
            ' Make sure GIS object present.


            'WPR 33-75 added
            m_lReturn = m_oRenSelection.GIS_LoadFromDB(CStr(vGisPolicyLinkArray(4, 0)).Trim(), v_lInsuranceFolderCnt, vGisPolicyLinkArray(0, 0), vRiskArray(ACRiskPosCnt, lCount))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.GIS_LoadFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "LoadFromDB"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
            ' So we pass existing folder_cnt in for old and new file_cnt.
            'sj 12/02/2003 - start
            'PS104

            'WPR 33-75 added
            m_lReturn = m_oRenSelection.CopyDataSet(v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim(), r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vOldRiskID:=vRiskArray(ACRiskPosCnt, lCount), v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vNewRiskID:=r_lNewRiskCnt) ', |            v_vNewInsuranceFileCnt:=v_lInsuranceFileCnt)
            '        m_lReturn = m_oRenSelection.CopyDataSet( _
            ''            v_sDataModelCode:=Trim$(vGisPolicyLinkArray(4, 0)), _
            ''            r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, _
            ''            r_sXMLDataSetDef:=sXMLDataSetDef, _
            ''            r_sXMLDataSet:=sXMLDataSet, _
            ''            v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), _
            ''            v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt, _
            ''            v_vOldRiskID:=vRiskArray(0, lCount&), _
            ''            v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt, _
            ''            v_vNewRiskID:=r_lNewRiskCnt)
            'sj 12/02/2003 - end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.CopyDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "CopyDataSet"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Initialise the Data Set with the Object/Properties

            m_lReturn = m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "LoadFromXML"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(28/02/2001)

            m_lReturn = m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.GIS_SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "SaveToDB"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'WPR 33-75 added
            m_lReturn = m_oRenSelection.CopyRiskStandardWordings(v_lOldPolicyBinderId:=vGisPolicyLinkArray(0, 0), v_lNewPolicyBinderId:=lNewGisPolicyLinkID, v_sDataModelCode:=Trim(vGisPolicyLinkArray(4, 0)))
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.CopyRiskStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "CopyRiskStandardWordings"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function

    ''' <summary>
    ''' ProcessStats
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="v_lOriginalInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessStats(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByVal v_lOriginalInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = 0
        Dim dThisPremium As Decimal = 0.0
        Dim nRIRowsToPostCnt As Integer = 0
        nResult = gPMConstants.PMEReturnCode.PMTrue
        Dim sFailureReason As String = ""

        m_oControlTrans.InsuranceFileCnt = v_lInsuranceFileCnt
        m_oControlTrans.OriginalInsuranceFileCnt = v_lOriginalInsuranceFileCnt ' RAW 13/11/2003 : CQ1765 : added
        If (m_bBackDateMTA) Then
            m_oControlTrans.BackDateMTA = True
        Else
            m_oControlTrans.BackDateMTA = False
        End If

        'WPR 33-75 added
        '   Need to record RI movement
        'm_lReturn = m_oControlTrans.GetThisPremium(dThisPremium, nRIRowsToPostCnt)
        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.GetThisPremium( Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStats")
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If

        'If dThisPremium = 0 AndAlso nRIRowsToPostCnt = 0 Then
        '    'Nothing to do
        '    Return nResult
        'End If


        m_lReturn = m_oControlTrans.SetProcessModes(vTransactionType:=v_sTransactionType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStats")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oControlTrans.Start
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            sFailureReason = m_oControlTrans.Message
            sFailureReason = "Statistics process failed :" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sFailureReason
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureReason, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStats")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return nResult

    End Function

    Public Function CreateBusinessObjectsLocal(ByVal v_oDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Dim sClassName As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase = v_oDatabase

            'Reinsurance
            If m_bRI2007Enabled Then
                sClassName = "bSIRReinsuranceRI2007.Form"
            Else
                sClassName = "bSIRReinsurance.Form"
            End If
            m_lReturn = CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:=sClassName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'RI Tax
            sClassName = "bSIRRITax.Business"
            m_oRITax = New bSIRRITax.Business
            m_lReturn = m_oRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Agent Commission
            sClassName = "bSirAgentCommission.Business"
            m_oAgentCommission = New BSirAgentCommission.Business
            m_lReturn = m_oAgentCommission.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Find Insurance
            sClassName = "bSIRFindInsurance.Form"
            m_lReturn = CreateBusinessObject(r_oObject:=m_oFindInsurance, v_sClassName:=sClassName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Change Policy Status
            sClassName = "bSIRChangePolicyStatus.Business"
            m_oChangePolicyStatus = New bSIRChangePolicyStatus.Business
            m_lReturn = m_oChangePolicyStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Find Risk
            sClassName = "bSIRFindRisk.Form"
            m_oFindRisk = New bSIRFindRisk.Form
            m_lReturn = m_oFindRisk.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'List Risks
            sClassName = "bSIRListRisks.Business"
            m_oListRisks = New bSIRListRisks.Business
            m_lReturn = m_oListRisks.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Risk Data
            'developer guide no. 267
            sClassName = "bSIRRiskData.Business"
            m_oRiskData = New bSIRRiskData.Business
            m_lReturn = m_oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Renewal Selection
            'developer guide no. 267
            sClassName = "bSIRRenSelection.Business"
            m_oRenSelection = New bSIRRenSelection.Business
            m_lReturn = m_oRenSelection.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Peril Allocation
            sClassName = "bSirPerilAllocation.Business"
            m_oPerilAllocation = New bSirPerilAllocation.Business
            m_lReturn = m_oPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Control Trans
            sClassName = "bControlTrans.Automated"
            m_oControlTrans = New bControlTrans.Automated
            m_lReturn = m_oControlTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Automatic Renewals Accept
            sClassName = "bSIRAutomaticRenewalsAccept.Business"
            m_oAutomaticRenewalsAccept = New bSIRAutomaticRenewalsAccept.Business
            m_lReturn = m_oAutomaticRenewalsAccept.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Gis
            'developer guide no. 267
            sClassName = "bGIS.Application"
            m_oGIS = New bGIS.Application
            m_lReturn = m_oGIS.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Insurance File
            ''  sClassName = "bSIRInsuranceFile.Business"
            m_oInsuranceFile = New bSIRInsuranceFile.Business
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Instalments
            'Instalments
            sClassName = "bSIRPremiumFinance.Business"
            m_lReturn = CreateBusinessObject(r_oObject:=m_oPremiumFinance, v_sClassName:=sClassName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            'Renewal
            sClassName = "bSIRRenewal.Business"
            m_oRenewal = New bSIRRenewal.Business
            m_lReturn = m_oRenewal.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Auto Renewal
            sClassName = "bSIRAutomaticRenewalsSel.Business"
            m_oAutomaticRenewalsSel = New bSIRAutomaticRenewalsSel.Business
            m_lReturn = m_oAutomaticRenewalsSel.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            m_oAutomaticRenewalsSel.CalledFromAutoMTA = True
            'Document Reversal
            sClassName = "bACTAllocationPost.Automated"
            m_oDocumentReversal = New bACTAllocationPost.Automated
            m_lReturn = m_oDocumentReversal.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            sClassName = "bSIRPartyFee.UBusiness"
            m_oSIRPartyFee = New bSIRPartyFee.UBusiness
            m_lReturn = m_oSIRPartyFee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'Dataset
            m_oDataset = New cGISDataSetControl.Application()

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsLocal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ''' <summary>
    ''' ProcessRiskReinsurance
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lRiskId"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="lFac_risk_cnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessRiskReinsurance(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskId As Integer, Optional ByRef v_sTransactionType As Object = "", Optional ByRef lFac_risk_cnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bApplyReinsurance As Boolean

            If v_sTransactionType <> "" Then
                m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:=v_sTransactionType)
            Else
                m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            End If

            m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:=v_sTransactionType)

            With m_oReinsurance

                .InsuranceFileCnt = v_lInsuranceFileCnt

                .RiskId = v_lRiskId
                'WPR 33-75 added
                If m_bRI2007Enabled = True Then
                    .CopyFACRiskCnt = lFac_risk_cnt
                End If
            End With


            m_lReturn = m_oReinsurance.ApplyReinsurance(ToSafeBoolean(bApplyReinsurance))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ApplyReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bApplyReinsurance Then

                If v_lRiskId <> 0 Then
                    'Set the status to quoted

                    m_lReturn = m_oReinsurance.ChangeRiskStatus

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ChangeRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                Return result
            End If

            'Generate reinsurance details for risk

            m_lReturn = m_oReinsurance.CalculateRI
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ApplyReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the risk status to quoted

            m_lReturn = m_oReinsurance.ChangeRiskStatus
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ChangeRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRiskReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ProcessRITax
    '
    ' Description:
    '
    ' History: 07/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessRITax(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, Optional ByRef sTransactionType As String = "") As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim bApplyTaxes As Boolean
            Dim oRITax As Object
            Dim sDesc As String = ""
            Dim bTaxesSwitchedOff As Boolean
            CreateBusinessObjectsLocal(m_oDatabase)
            'Do we need to apply taxes?

            m_lReturn = m_oRITax.ApplyTaxes(v_lInsuranceFileCnt, v_lRiskCnt, bApplyTaxes, bTaxesSwitchedOff)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.ApplyTaxes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                Return nResult
            End If

            If Not bApplyTaxes Then
                'Nothing to do
                Return nResult
            End If


            m_lReturn = m_oRITax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=sTransactionType)

            With m_oRITax

                .InsuranceFileCnt = v_lInsuranceFileCnt

                .RiskCnt = v_lRiskCnt
            End With

            If v_lRiskCnt > 0 Then

                m_oRITax.RiskCnt = v_lRiskCnt

                m_lReturn = m_oRITax.GetRiskTax(r_vRiskTax:=oRITax, r_sDescription:=sDesc, iTask:=gPMConstants.PMEComponentAction.PMEdit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.GetRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                    Return nResult
                End If

                ' update risk tax to actually set the tax amount

                m_lReturn = m_oRITax.UpdateRiskTax(v_vRiskTax:=oRITax)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.GetRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                    Return nResult
                End If

            Else

                m_oRITax.InsuranceFileCnt = v_lInsuranceFileCnt

                m_lReturn = m_oRITax.GetInsuranceFileTax(r_vInsuranceFileTax:=oRITax, r_sDescription:=sDesc, iTask:=gPMConstants.PMEComponentAction.PMEdit, v_sTransType:=sTransactionType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.GetInsuranceFileTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                    Return nResult
                End If
            End If

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' ProcessChangePolicyStatus
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="v_sSelectedPolicyStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessChangePolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef v_sTransactionType As String, Optional ByVal v_sSelectedPolicyStatus As String = "", Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        Dim vRisks(,) As Object
        Dim sMessage As String = ""
        Dim bSelectedRisks As Boolean
        Dim nLevel As Integer


        If m_sTransactionType = "MTC" And (m_sCallingAppName = "iPMBFinancePlanMaint" Or m_sCallingAppName = "bSIRPremiumFinance" Or m_sCallingAppName = "SiriusTransactionService") And (v_bIsBackdatedMTA = False) Then
            m_oChangePolicyStatus.TransactionType = m_sTransactionType
        Else
            If (m_sTransactionType = "MTC" Or v_sTransactionType = "MTCR") And m_bUpdateStats = False Then
                m_oChangePolicyStatus.TransactionType = "MTCA"
            Else
                m_oChangePolicyStatus.TransactionType = m_sTransactionType
            End If
        End If


        m_lReturn = m_oChangePolicyStatus.GetRisksByStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vRisks:=vRisks)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vRisks) Then
            sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "There are no risks on this policy"
        Else
            sMessage = ""
            nLevel = 0

            For lTemp As Integer = vRisks.GetLowerBound(1) To vRisks.GetUpperBound(1)

                If CDbl(vRisks(1, lTemp)) = 1 Then
                    bSelectedRisks = True
                    Select Case vRisks(0, lTemp)
                        Case 1, 2, 4
                            If nLevel < 3 Then
                                nLevel = 3
                                sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                           "At least one risk on this policy is unquoted"
                            End If
                        Case 8

                            If CStr(vRisks(5, lTemp)) = "U" Then
                                If nLevel < 2 Then
                                    nLevel = 2
                                    sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "At least one risk on this policy has no reinsurance"
                                End If
                            End If
                        Case 5, 6, 7
                            If nLevel < 1 Then
                                nLevel = 1
                                sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                                           "At least one risk on this policy has questions to be answered"
                            End If
                    End Select
                End If
            Next lTemp
        End If

        If Not bSelectedRisks Then
            sMessage = "Cannot proceed -" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "At least one risk on this policy must be selected to make it live"
        End If


        If sMessage <> "" Then
            'Store any decline or referral messages
            m_lReturn = StoreMessagesReferalsAndDeclines(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=0, v_sPolicyStatusMessages:=sMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="StoreMessagesReferalsAndDeclines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
                Return nResult
            End If
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        m_lReturn = m_oChangePolicyStatus.DeleteRisks(v_vrisks:=vRisks)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.DeleteRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=CInt(vRisks(2, 0)))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.RenumberRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        vRisks = Nothing


        m_oChangePolicyStatus.Mode = 0


        m_lReturn = m_oChangePolicyStatus.ChangePolicyStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_bBackDatedMTAsAllowed:=True, v_sSelectedPolicyStatus:=v_sSelectedPolicyStatus)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.ChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_sTransactionType <> "MTA" Or m_bIsInteractive = False Then
            m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.UpdatePolicyPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return nResult

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetVersionByDate
    '
    ' Description:
    '
    ' History: 08/01/2003 SJ - Created.
    ' RAW 13/11/2003 : CQ1765 : added v_bIsPermanentMTA param
    ' ***************************************************************** '
    Public Function GetVersionByDate(ByRef r_lInsuranceFileCnt As Object, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Object, ByRef r_lErrorCode As Object, Optional ByRef r_bBackdatingRequired As Object = False, Optional ByVal v_bIsReinstatement As Boolean = False, Optional ByVal v_bIsCancellation As Boolean = False, Optional ByVal v_lDeletedRiskInsuranceFileCnt As Integer = 0, Optional ByVal v_bIsPermanentMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oFindInsurance.BackDatedMTA = m_bBackDateMTA
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - Allow Return Premium.doc) - (5.1.1.1)
            'Changed the parameter v_bIsPermanentMTA:=v_bIsPermanentMTA to v_lMTAType:= kMTATypePermanentAndTemporary
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs - Allow Return Premium.doc) - (5.1.1.1)
            ' RAW 13/11/2003 : CQ1765 : added v_bIsPermanentMTA param
            m_lReturn = m_oFindInsurance.GetVersionsByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=ToSafeDate(v_dtStartDate), r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=ToSafeInteger(m_lInsuranceFolderCnt), r_bBackdatingRequired:=r_bBackdatingRequired, r_vAffectedInsuranceFileCnts:=m_vAffectedInsuranceFileCnts, v_bIsReinstatement:=ToSafeBoolean(v_bIsReinstatement), v_bIsCancellation:=ToSafeBoolean(v_bIsCancellation), v_lDeletedRiskInsuranceFileCnt:=ToSafeInteger(v_lDeletedRiskInsuranceFileCnt), v_lMTAType:=gPMConstants.kMTATypePermanentAndTemporary)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionByDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CopyPolicy
    '
    ' Description:
    '
    ' History: 08/01/2003 SJ - created
    '
    ' ***************************************************************** '
    Public Function CopyPolicy(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Object, ByVal v_lVersion As Integer, ByVal v_bPermanentMTA As Boolean, ByVal v_dtMTADate As Date, Optional ByVal v_vMTAEndDate As Object = Nothing, Optional ByVal v_bCopyDeletedLink As Boolean = False, Optional ByVal v_bCancellation As Boolean = False, Optional ByVal v_sTransactionType As String = "", Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal bReinstatement As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 20/01/2004 : CQ3535 : added v_vPolicyCancelledOnDate param

            m_lReturn = m_oFindInsurance.CopyPolicy(v_lOldInsuranceFileCnt:=ToSafeInteger(v_lOldInsuranceFileCnt), r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=ToSafeInteger(v_lVersion), v_bPermanentMTA:=ToSafeBoolean(v_bPermanentMTA), v_dtMTADate:=ToSafeDate(v_dtMTADate), v_vMTAEndDate:=v_vMTAEndDate, v_bCancellation:=ToSafeBoolean(v_bCancellation), v_sTransactionType:=ToSafeString(v_sTransactionType), v_bIsBackdatedMTA:=ToSafeBoolean(v_bIsBackdatedMTA), v_bReinstatement:=ToSafeBoolean(bReinstatement))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SetNillPremiumRefund
    '
    ' Description:
    '
    ' History: 31/01/2003 sj - Created.
    ' RAW 24/11/2003 : CQ685 : added v_vAffectedInsuranceFileCnts param
    ' ***************************************************************** '
    Public Function SetNillPremiumRefund(Optional ByVal v_vAffectedInsuranceFileCnts() As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oFindInsurance.SetNillPremiumRefund(v_lInsuranceFileCnt:=ToSafeInteger(m_lNewInsuranceFileCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.SetNillPremiumRefund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' RAW 24/11/2003 : CQ685 : added
            If Not Informations.IsNothing(v_vAffectedInsuranceFileCnts) And Informations.IsArray(v_vAffectedInsuranceFileCnts) Then

                For i As Integer = v_vAffectedInsuranceFileCnts.GetLowerBound(0) To v_vAffectedInsuranceFileCnts.GetUpperBound(0)


                    If CDbl(v_vAffectedInsuranceFileCnts(i)) <> m_lNewInsuranceFileCnt Then


                        m_lReturn = m_oFindInsurance.SetNillPremiumRefund(v_lInsuranceFileCnt:=v_vAffectedInsuranceFileCnts(i))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.SetNillPremiumRefund Failed for other InsuranceFile (" & i & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next i
            End If
            ' RAW 24/11/2003 : CQ685 : end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetNillPremiumRefund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessAgentCommission
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessAgentCommission(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bCommissionRequired As Boolean
            Dim vAgentCommission As Object


            m_oAgentCommission.InsuranceFileCnt = v_lInsuranceFileCnt

            'Do we require agent commission

            m_lReturn = m_oAgentCommission.CheckDisplayCommission(r_bDisplayScreen:=bCommissionRequired)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAgentCommission.CheckDisplayCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bCommissionRequired Then
                'No processing required
                Return result
            End If

            'Calculate agent commission

            m_lReturn = m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=m_sTransactionType, r_vntResult:=vAgentCommission)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAgentCommission.CalculateAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 19/09/2003 : CQ2614 : added
            'Calculate lead commission

            m_lReturn = m_oAgentCommission.UpdateLeadCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oAgentCommission.UpdateLeadCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 19/09/2003 : CQ2614 : end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ReverseArray
    '
    ' Description:
    '
    ' History: 06/01/2003 sj - Created.
    ' ***************************************************************** '
    Private Function ReverseArray(ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lCols, lRows As Integer
        Dim vArray(,) As Object
        Dim lNewRowCnt As Integer
        If Informations.IsArray(r_vArray) Then
            lCols = r_vArray.GetUpperBound(0)
            lRows = r_vArray.GetUpperBound(1)


            vArray = r_vArray.Clone

            ReDim r_vArray(lCols, lRows)

            For lRowCnt As Integer = lRows To 0 Step -1
                For lColCnt As Integer = 0 To lCols


                    r_vArray(lColCnt, lNewRowCnt) = vArray(lColCnt, lRowCnt)
                Next lColCnt

                lNewRowCnt += 1
            Next lRowCnt
        End If
        Return result

    End Function

    ''' <summary>
    ''' IsBackdatedMTARequired
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function IsBackdatedMTARequired() As Boolean

        Dim result As Boolean = False
        Dim lPolicyVersion, lErrorCode As Integer
        Dim bBackdatingRequired As Boolean
        Dim sInsuranceFileTypeCode As Object = ""
        Dim iBDMTAAlloweddates As Integer
        Dim bIsReinstatement As Boolean

        Try



            m_lReturn = m_oFindInsurance.GetInsuranceFileType(v_lInsuranceFileCnt:=ToSafeInteger(m_lNewInsuranceFileCnt), r_sInsuranceFileTypeCode:=sInsuranceFileTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.GetInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sInsuranceFileTypeCode <> "MTAQTETEMP" Then
                lErrorCode = 1
            Else
                lErrorCode = 0
            End If

            m_lReturn = GetOutOfSequenceMTAProductDetails(v_lInsurance_file_cnt:=m_lNewInsuranceFileCnt, r_iMTAAllowedDates:=iBDMTAAlloweddates)
            'Get a list of all the policy versions which need to be processed and
            'store them in the array - m_vAffectedInsuranceFileCnts


            m_lReturn = GetVersionByDate(r_lInsuranceFileCnt:=m_lNewInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, r_bBackdatingRequired:=bBackdatingRequired, v_bIsReinstatement:=m_sTransactionType = "MTR")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Bracket () Implemented in starting and before bBackdatingRequired (PN-72127) Nitesh Dwivedi
            If Informations.IsArray(m_vAffectedInsuranceFileCnts) Then
                'WPR 33-75 added
                'If ((m_vAffectedInsuranceFileCnts.GetUpperBound(1) > 0 And iBDMTAAlloweddates = 1) Or (m_vAffectedInsuranceFileCnts.GetUpperBound(1) >= 0 And iBDMTAAlloweddates > 1)) And bBackdatingRequired Then
                If ((m_vAffectedInsuranceFileCnts.GetUpperBound(1) > 0 And iBDMTAAlloweddates = 1) Or (m_vAffectedInsuranceFileCnts.GetUpperBound(1) >= 0 And iBDMTAAlloweddates > 1)) And bBackdatingRequired Then
                    'More than one version affected
                    result = True
                End If
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsBackDatedMTARequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return m_lReturn

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateEvent
    '
    ' Description:
    '
    ' History: 21/02/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function CreateEvent(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sReason As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oInsuranceFile.PartyCnt = m_lPartyCnt

        m_oInsuranceFile.InsuranceFolderCnt = m_lInsuranceFolderCnt

        m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt


        m_oInsuranceFile.EventDescription = v_sReason

        ' populate collections before calling make event
        m_lReturn = m_oInsuranceFile.GetDetails(vInsuranceFileCnt:=v_lInsuranceFileCnt)
        m_lReturn = m_oInsuranceFile.MakeEvent

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CreateBusinessObject
    '
    ' Description: Creates an instance of the class name passed.
    '
    ' ***************************************************************** '
    Public Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0
        Dim lReturnCode As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'r_oObject = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).FullName, v_sClassName).Unwrap()
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of " & v_sClassName
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sClassName, vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Dim oDatabase As Object = Nothing
            'lReturnCode = r_oObject.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=oDatabase)
            'm_oDatabase = CType(oDatabase, dPMDAO.Database)
            '' Check for errors.
            'If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = gPMConstants.PMEReturnCode.PMFalse
            'End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Set the object to nothing

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object instance (" & v_sClassName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ''Start(Saurabh Agrawal) Out of sequence MTA Bug Fixing
            Const kMethodName As String = "Initialise"
            m_lReturn = GetProductOptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductOptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch excep As System.Exception
            ''End(Saurabh Agrawal) Out of sequence MTA Bug Fixing


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Terminate() As Integer

        Dim result As Integer = 0
        Static bTerminated As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bTerminated Then
                Return result
            Else
                bTerminated = True
            End If


            If Not (m_oReinsurance Is Nothing) Then

                m_oReinsurance.Dispose()
                m_oReinsurance = Nothing
            End If

            If Not (m_oRITax Is Nothing) Then

                m_oRITax.Dispose()
                m_oRITax = Nothing
            End If

            If Not (m_oAgentCommission Is Nothing) Then

                m_oAgentCommission.Dispose()
                m_oAgentCommission = Nothing
            End If

            If Not (m_oFindInsurance Is Nothing) Then

                m_oFindInsurance.Dispose()
                m_oFindInsurance = Nothing
            End If

            If Not (m_oChangePolicyStatus Is Nothing) Then

                m_oChangePolicyStatus.Dispose()
                m_oChangePolicyStatus = Nothing
            End If

            If Not (m_oFindRisk Is Nothing) Then

                m_oFindRisk.Dispose()
                m_oFindRisk = Nothing
            End If

            If Not (m_oListRisks Is Nothing) Then

                m_oListRisks.Dispose()
                m_oListRisks = Nothing
            End If

            If Not (m_oRiskData Is Nothing) Then

                m_oRiskData.Dispose()
                m_oRiskData = Nothing
            End If

            If Not (m_oRenSelection Is Nothing) Then

                m_oRenSelection.Dispose()
                m_oRenSelection = Nothing
            End If

            If Not (m_oPerilAllocation Is Nothing) Then

                m_oPerilAllocation.Dispose()
                m_oPerilAllocation = Nothing
            End If

            If Not (m_oControlTrans Is Nothing) Then

                m_oControlTrans.Dispose()
                m_oControlTrans = Nothing
            End If

            If Not (m_oAutomaticRenewalsAccept Is Nothing) Then

                m_oAutomaticRenewalsAccept.Dispose()
                m_oAutomaticRenewalsAccept = Nothing
            End If

            If Not (m_oInsuranceFile Is Nothing) Then

                m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

            If Not (m_oGIS Is Nothing) Then

                m_oGIS.Dispose()
                m_oGIS = Nothing
            End If

            If Not (m_oRenewal Is Nothing) Then

                m_oRenewal.Dispose()
                m_oRenewal = Nothing
            End If

            If Not (m_oDataset Is Nothing) Then
                m_oDataset.Dispose()
                m_oDataset = Nothing
            End If

            'PLico 45
            If Not (m_oAutomaticRenewalsSel Is Nothing) Then

                m_oAutomaticRenewalsSel.Dispose()
                m_oAutomaticRenewalsSel = Nothing
            End If

            If Not (m_oDocumentReversal Is Nothing) Then

                m_oDocumentReversal.Dispose()
                m_oDocumentReversal = Nothing
            End If

            If Not (m_oSIRPartyFee Is Nothing) Then
                m_oSIRPartyFee.Dispose()
                m_oSIRPartyFee = Nothing
            End If

            m_oAutoMTAMerge = Nothing
            m_oDatabase = Nothing


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    '**********************************************************************
    ' remove renewal version of policy, renewal_status and all associate records
    '**********************************************************************
    Private Function DeletePolicyFromRenewal(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oRenewal As Object



        result = m_oRenewal.DeletePolicyFromRenewal(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)



        Return result
    End Function

    Private Function GetOutOfSequenceMTAProductDetails(ByVal v_lInsurance_file_cnt As Long, Optional ByRef r_iMTAAllocation As Integer = 0, Optional ByRef r_iMTAAllowedDates As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()


        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsurance_file_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) ' Sankar - Changed PMInteger to PMLong

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACOutOfSequenceMTADetailsSQL, sSQLName:=ACOutOfSequenceMTADetailsName, bStoredProcedure:=ACOutOfSequenceMTADetailsStored, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetOutOfSequenceMTAProductDetails", "Failed to fetch MTA allocation details", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Informations.IsArray(vArray) Then
            Return result
        End If


        r_iMTAAllocation = CInt(vArray(0, 0))

        r_iMTAAllowedDates = CInt(vArray(1, 0))

        vArray = Nothing




        Return result
    End Function

    Private Function GetTransactionIdsForReversal(ByVal v_lInsurance_file_cnt As Integer, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()


        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsurance_file_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) 'Sankar - Changed PMInteger to PMLong

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransIDForReversalSQL, sSQLName:=ACGetTransIDForReversalName, bStoredProcedure:=ACGetTransIDForReversalStored, vResultArray:=r_vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetTransactionIdsForReversal", "Failed to fetch transaction ids", gPMConstants.PMELogLevel.PMLogError)
        End If




        Return result
    End Function
    Private Function UpdateComment(ByVal v_lTransaction_Id As Long) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()


        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_id", vValue:=v_lTransaction_Id, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCommentSQL, sSQLName:=ACUpdateCommentName, bStoredProcedure:=ACUpdateCommentStored)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("UpdateComment", "Failed to update comment", gPMConstants.PMELogLevel.PMLogError)
        End If




        Return result
    End Function

    Public Function CheckRENAffectedbyBDMTA() As Boolean

        Dim result As Boolean = False
        Try


            If Informations.IsArray(m_vAffectedInsuranceFileCnts) Then
                For i As Integer = m_vAffectedInsuranceFileCnts.GetUpperBound(1) To 0 Step -1
                    'PN58130 -Amit
                    If CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, i)).Trim() = "POLICY" And gPMFunctions.ToSafeInteger(CStr(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)).Trim()) <> 1 Then
                        Return True
                    End If

                Next
            End If
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRENAffectedbyBDMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRENAffectedbyBDMTA", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    ''start(Saurabh Agrawal) Out of sequence Bug fixing
    Private Function GetProductOptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductOptions"

        Dim lReturn As Integer
        Dim vValue As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        '*****************************************



        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, "getProductOptionValue Failed " &
                                    " to return value for Option:" & CStr(gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007), gPMConstants.PMELogLevel.PMLogError)

        End If


        m_bRI2007Enabled = CStr(vValue) = "1"

        Return result
    End Function
    'WPR 33-75 added
    Private Function GetAffectedBackDatedMTAVersions() As Integer

        Const kMethodName As String = "GetAffectedBackDatedMTAVersions"

        Dim lReturn As Integer
        Dim vResultArray(,) As Object
        Dim iCount As Integer
        Dim lUbound As Integer




        GetAffectedBackDatedMTAVersions = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        lReturn = m_oDatabase.Parameters.Add(sName:="base_insurance_file_cnt", vValue:=m_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAffectedBackDatedMTAVersionsSQL, sSQLName:=ACGetAffectedBackDatedMTAVersionsName, bStoredProcedure:=ACGetAffectedBackDatedMTAVersionsStored, vResultArray:=vResultArray)

        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError("GetAffectedBackDatedMTAVersions", "Failed to fetch Affected BackDated MTA Versions.", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Informations.IsArray(vResultArray) Then
            lUbound = vResultArray.GetUpperBound(1)

            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lUbound)

            For iCount = 0 To vResultArray.GetUpperBound(1)
                '           If iCount = 0 And m_sTransactionType = "MTC" Then
                '               sType = "MTA"
                '           Else
                '            Select Case vResultArray(ACBDMTATransTypeId, iCount)
                '                Case 8
                '                    sType = "MTC"
                '                Case Else
                '                    sType = "MTA"
                '            End Select
                '           End If
                m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, iCount) = vResultArray(ACBDMTAInsFileCnt, iCount)
                m_vAutoMTAInsFileCnts(ACAutoMTATransType, iCount) = vResultArray(3, iCount)
                m_vAutoMTAInsFileCnts(ACAutoMTAVersion, iCount) = vResultArray(ACBDMTAVersion, iCount)
            Next iCount

        End If

    End Function
    ''' <summary>
    ''' Update Insurance File Replaced
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nCancelled"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function UpdateInsuranceFileReplaced(ByVal nInsuranceFileCnt As Integer,
                                                ByVal nCancelled As Integer) As Integer


        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt",
                                               vValue:=nInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nCancelled",
                                               vValue:=nCancelled,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.SQLAction(
                                        sSQL:=ACUpdateInsuranceFileReplacedStatusSQL,
                                        sSQLName:=ACUpdateInsuranceFileReplacedStatusName,
                                        bStoredProcedure:=ACUpdateInsuranceFileReplacedStatusStored)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileReplacedStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileReplaced")
            Return nResult
        End If
        Return nResult


    End Function

    ''' <summary>
    ''' Update Risks In RiskFolder
    ''' </summary>
    ''' <param name="nBaseInsuranceFileCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <param name="nOriginalInsuranceFileCnt"></param>
    ''' <param name="nPreChangeInsuranceFileCnt"></param>
    ''' <param name="nOldRiskCnt"></param>
    ''' <param name="nNewRiskCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function UpdateRisksInRiskFolder(ByVal nBaseInsuranceFileCnt As Integer,
                                                    ByVal nNewInsuranceFileCnt As Integer,
                                                        ByVal nOriginalInsuranceFileCnt As Integer,
                                                            ByVal nPreChangeInsuranceFileCnt As Integer,
                                                                ByVal nOldRiskCnt As Integer,
                                                                    ByVal nNewRiskCnt As Integer) As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nBaseInsuranceFileCnt", vValue:=nBaseInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nOriginalInsuranceFileCnt", vValue:=nOriginalInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nPreChangeInsuranceFileCnt", vValue:=nPreChangeInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nOldRiskCnt", vValue:=nOldRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewRiskCnt", vValue:=nNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRisksInRiskFolderSQL,
                                          sSQLName:=ACUpdateRisksInRiskFolderName,
                                          bStoredProcedure:=ACUpdateRisksInRiskFolderStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateRisksInRiskFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisksInRiskFolder")
            Return nResult
        End If
        Return nResult

    End Function

    Private Function UpdateBaseInsuranceFile(ByVal v_lBaseInsuranceFileCnt As Integer,
                                                    ByVal v_lInsuranceFileCnt As Integer) As Integer


        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue
        'Update Base Insurance File Cnt
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BaseIfileCnt",
                                               vValue:=v_lBaseInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewIFileCnt",
                                               vValue:=v_lInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Execute the stored procedure
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdBaseInsuranceFileCntSQL,
                                          sSQLName:=ACUpdBaseInsuranceFileCntName,
                                          bStoredProcedure:=ACUpdBaseInsuranceFileCntStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdBaseInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdBaseInsuranceFile")
            Return nResult
        End If
        Return nResult

    End Function

    Private Function UpdateInsuranceFileDetails(ByVal nBaseInsuranceFileCnt As Integer,
                                                ByVal nInsuranceFileCnt As Integer) As Integer


        Dim nResult As Integer = 0

        nResult = gPMConstants.PMEReturnCode.PMTrue
        'Update Base Insurance File Cnt
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt",
                                               vValue:=nBaseInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewInsuranceFileCnt",
                                               vValue:=nInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Execute the stored procedure
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdInsuranceFileDetailsSQL,
                                          sSQLName:=ACUpdInsuranceFileDetailsName,
                                          bStoredProcedure:=ACUpdInsuranceFileDetailsStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdBaseInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdBaseInsuranceFile")
            Return nResult
        End If
        Return nResult

    End Function



    Private Function UpdateInsuranceFileDetailsOOSReinstate(ByVal nBaseInsuranceFileCnt As Integer,
                                                ByVal nInsuranceFileCnt As Integer,
                                                ByVal bRenewals As Boolean, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim nTransactionType As Integer
        Dim nResult As Integer = 0
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue


            'Update Base Insurance File Cnt
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt",
                                                   vValue:=nBaseInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewInsuranceFileCnt",
                                                   vValue:=nInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdInsuranceFileDetailsOOSReinsSQL,
                                              sSQLName:=ACUpdInsuranceFileDetailsOOSReinsName,
                                              bStoredProcedure:=ACUpdInsuranceFileDetailsOOSReinsStored)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ACUpdInsuranceFileDetailsOOSReinsSQL Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileDetailsOOSReinstate")
                Return nResult
            End If


            ProcessAgentCommission(nInsuranceFileCnt)

            nTransactionType = 10

            ' Recalculate the Policy taxes
            m_lReturn = m_oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=nInsuranceFileCnt,
                                                         v_lProductId:=-1,
                                                         v_lTransactionTypeId:=nTransactionType,
                                                         v_bUseExistingFeeDetail:=False,
                                                         v_bIsBackdatedMTA:=v_bIsBackdatedMTA)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="RecalculatePolicyFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileDetailsOOSReinstate")
                Return nResult
            End If
            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername,
                       iType:=gPMConstants.PMEReturnCode.PMError,
                       sMsg:="UpdateInsuranceFileDetailsOOSReinstate Failed",
                       vApp:=ACApp,
                       vClass:=ACClass,
                       vMethod:="UpdateInsuranceFileDetailsOOSReinstate",
                       vErrNo:=Informations.Err.Number,
                       vErrDesc:=Informations.Err.Description)
            Return nResult
        End Try

    End Function

    Friend Function ProcessRisksForMultiThreading(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String, Optional ByVal dtMTAStartDate As Date = #12/30/1899#, Optional ByVal nBaseInsuranceFileCnt As Integer = 0, Optional ByVal nPreChangeInsFileCnt As Integer = 0, Optional ByVal nPostChangeInsFileCnt As Integer = 0, Optional ByVal bApplyRiskChange As Boolean = False, Optional ByVal bIsBackdatedMTA As Boolean = False) As Integer

        Const kMethodName As String = "ProcessRisksForMultiThreading"
        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim aDataArray(,) As Object
            Dim sFailureReason As String = ""
            Dim oSelectionArray As Object
            Dim sRiskMergeStatus As String = ""

            If bIsBackdatedMTA = False Then
                m_lReturn = GetListOfRisks(nInsuranceFileCnt, aDataArray)
            Else
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nBase_insurance_file_cnt", vValue:=m_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="nNew_insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'Get a list of all the risks with edited flag for version being cancelled and generate refund only for edited risk in that version
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_SIR_Get_reversal_risks",
                                                  sSQLName:="spu_SIR_Get_reversal_risks",
                                                  bStoredProcedure:=True,
                                                  vResultArray:=aDataArray)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsArray(aDataArray) Then
                'Should always have a least one risk
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ReDim oSelectionArray(1, aDataArray.GetUpperBound(1))

            Dim iNumberOfThreads As Integer = GetMultithreadLimit()
            Dim nNumberOfRisks As Integer
            Dim iRiskTracker As Integer

            nNumberOfRisks = aDataArray.GetUpperBound(1) + 1

            If iNumberOfThreads > nNumberOfRisks Then
                If nNumberOfRisks = 0 Then
                    iNumberOfThreads = 1
                Else
                    iNumberOfThreads = nNumberOfRisks
                End If
            End If

            Dim option2 As TransactionOptions = New TransactionOptions()
            option2.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
            Using scope As New TransactionScope()

                Dim currentTransaction As Transaction = Transaction.Current
                Dim dt As DependentTransaction
                dt = currentTransaction.DependentClone(DependentCloneOption.RollbackIfNotComplete)
                Dim oMTAThreads(iNumberOfThreads - 1) As Thread
                For iRiskLoop As Integer = 0 To nNumberOfRisks - 1
                    Dim oMTAParameters As RiskData

                    For iLoop As Integer = 0 To iNumberOfThreads - 1
                        If iRiskTracker <= nNumberOfRisks - 1 Then
                            oMTAParameters = New RiskData
                            With oMTAParameters
                                .nInsuranceFileCnt = nInsuranceFileCnt
                                .sTransactionType = sTransactionType
                                .oArray = aDataArray
                                .nCount = iRiskTracker
                                .bIsBackdatedMTA = bIsBackdatedMTA
                                .dtMTAStartDate = dtMTAStartDate
                                .nPreChangeInsFileCnt = nPreChangeInsFileCnt
                                .nPostChangeInsFileCnt = nPostChangeInsFileCnt
                                .bApplyRiskChange = bApplyRiskChange
                                .nBaseInsuranceFileCnt = nBaseInsuranceFileCnt
                                .dependentTransaction = dt
                            End With

                            oMTAThreads(iLoop) = New Thread(New ParameterizedThreadStart(AddressOf ActionProcessRisk))
                            oMTAThreads(iLoop).Start(oMTAParameters)

                            'need to keep the risk loop in sync with the thread loop
                            iRiskTracker += 1
                        End If
                    Next

                    For iLoop As Integer = 0 To iNumberOfThreads - 1
                        oMTAThreads(iLoop).Join()
                    Next

                Next 'end risk loop
            End Using

            m_lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=nInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.RenumberRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.UpdatePolicyPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sTransactionType = "MTCR" Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=nBaseInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_agent_commission_rev", sSQLName:="Reverse Comm", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_sir_agent_commission_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForMultiThreading")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' reverse fee
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=nBaseInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_sir_policy_fee_rev Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForMultiThreading")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Call the agent commission component
                m_lReturn = ProcessAgentCommission(
                            v_lInsuranceFileCnt:=nInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername,
                               iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="ProcessAgentCommission Failed",
                               vApp:=ACApp,
                               vClass:=ACClass,
                               vMethod:="AutoRunMTAOnly")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Policy level tax
            m_lReturn = ProcessRITax(
                        v_lInsuranceFileCnt:=nInsuranceFileCnt,
                        v_lRiskCnt:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername,
                           iType:=gPMConstants.PMELogLevel.PMLogOnError,
                           sMsg:="ProcessRITax Failed",
                           vApp:=ACApp,
                           vClass:=ACClass,
                           vMethod:="AutoRunMTAOnly")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    'Implementing Multithreeading functionality 02 Feb 2016
    Private Function ActionProcessRisk(ByVal oMTAParameters As RiskData) As Integer

        Const kMethodName As String = "ActionProcessRisk"
        Dim bIsEdited As Boolean
        Dim sRiskMergeStatus As String = ""
        Dim nRiskId As Integer
        Dim sRiskStatus As String
        Dim aArray(,) As Object
        Dim nNewRiskCnt As Integer
        Dim iSelected As Integer
        Dim oDataBase As dPMDAO.Database = Nothing
        Try
            Dim oAutoMTA As New bSIRAutoMTA.AutoMta

            DBConnect(oDataBase)
            'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ActionProcessRisk just done DBConnect", vApp:=ACApp, vClass:=ACClass, _
            '                   vMethod:=kMethodName)
            m_lReturn = oAutoMTA.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            'need to set the properties from the .net AUTOMTA to the VB version here
            oAutoMTA.InsuranceFolderCnt = m_lInsuranceFolderCnt
            oAutoMTA.NewInsuranceFileCnt = m_lNewInsuranceFileCnt
            oAutoMTA.BackDateMTA = m_bBackDateMTA
            oAutoMTA.IsInteractive = m_bIsInteractive
            oAutoMTA.MergeRisks = m_bMergeRisks
            oAutoMTA.CreateBusinessObjectsLocal(v_oDatabase:=oDataBase)

            bIsEdited = True
            sRiskMergeStatus = ""
            If Not oMTAParameters.bIsBackdatedMTA Then
                nRiskId = CInt(oMTAParameters.oArray(ACIRiskId, oMTAParameters.nCount))
                sRiskStatus = oMTAParameters.oArray(ACIRiskStatusFlag, oMTAParameters.nCount).ToString.ToUpper
            Else
                nRiskId = oMTAParameters.oArray(0, oMTAParameters.nCount)
                sRiskStatus = oMTAParameters.oArray(1, oMTAParameters.nCount).ToString.ToUpper

                aArray = Nothing

                oDataBase.Parameters.Clear()
                m_lReturn = oDataBase.Parameters.Add(sName:="nOriginal_insurance_file_cnt", vValue:=oMTAParameters.nBaseInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                m_lReturn = oDataBase.Parameters.Add(sName:="nRisk_cnt", vValue:=nRiskId, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                'Get edited flag for version being cancelled and generate refund only for edited risk in that version
                m_lReturn = oDataBase.SQLSelect(sSQL:="spu_SIR_Get_original_risk_status",
                                                  sSQLName:="spu_SIR_Get_original_risk_status",
                                                  bStoredProcedure:=True,
                                                  vResultArray:=aArray)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ActionProcessRisk spu_SIR_Get_original_risk_status failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName)
                    m_lReturn = PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(aArray) Then

                    If m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                        ' cancel risk if edited in any of the original version; deleted risks must be reinstated back as refund should come from previous live version
                        If (ToSafeInteger(aArray(0, 0)) = 0) Then
                            bIsEdited = False
                        ElseIf (ToSafeString(aArray(2, 0)) = "D") And m_sTransactionType = "MTR" Then
                            sRiskStatus = "D"
                            sRiskMergeStatus = ACRStatusDeletedPostChange
                        End If
                    Else
                        ' Ignore if not edited in base version and original version and wasn't deleted in original version
                        If (ToSafeInteger(aArray(0, 0)) = 0 Or ToSafeInteger(oMTAParameters.oArray(2, oMTAParameters.nCount)) = 0) And ToSafeString(aArray(2, 0)) <> "D" Then
                            bIsEdited = False
                        End If
                    End If
                Else
                    bIsEdited = False
                End If

                If (sRiskStatus = "U" Or sRiskStatus = "D") AndAlso bIsEdited AndAlso oMTAParameters.bIsBackdatedMTA Then

                    m_lReturn = oAutoMTA.ProcessSingleRisk(v_lInsuranceFileCnt:=oMTAParameters.nInsuranceFileCnt, v_lRiskId:=nRiskId, v_iRiskCnt:=oMTAParameters.nCount, r_lNewRiskCnt:=nNewRiskCnt,
                                                               r_iSelected:=iSelected, v_sTransactionType:=oMTAParameters.sTransactionType, v_dtMTAStartDate:=oMTAParameters.dtMTAStartDate,
                                                               v_bApplyRiskChange:=oMTAParameters.bApplyRiskChange, v_lBaseInsuranceFileCnt:=oMTAParameters.nBaseInsuranceFileCnt,
                                                               v_lPreChangeInsFileCnt:=oMTAParameters.nPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=oMTAParameters.nPostChangeInsFileCnt,
                                                               v_bIsBackdatedMTA:=oMTAParameters.bIsBackdatedMTA)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call to ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                ElseIf (sRiskStatus = "U" OrElse sRiskStatus = "D") AndAlso bIsEdited = True AndAlso oMTAParameters.bIsBackdatedMTA = False Then
                    m_lReturn = oAutoMTA.ProcessSingleRisk(v_lInsuranceFileCnt:=oMTAParameters.nInsuranceFileCnt, v_lRiskId:=nRiskId, v_iRiskCnt:=oMTAParameters.nCount, r_lNewRiskCnt:=nNewRiskCnt,
                                                                r_iSelected:=iSelected, v_sTransactionType:=oMTAParameters.sTransactionType, v_dtMTAStartDate:=oMTAParameters.dtMTAStartDate,
                                                                v_bApplyRiskChange:=oMTAParameters.bApplyRiskChange, v_lBaseInsuranceFileCnt:=oMTAParameters.nBaseInsuranceFileCnt,
                                                                v_lPreChangeInsFileCnt:=oMTAParameters.nPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=oMTAParameters.nPostChangeInsFileCnt,
                                                                v_bIsBackdatedMTA:=oMTAParameters.bIsBackdatedMTA, nIsRiskEdited:=1, v_sRiskMergeStatus:=sRiskMergeStatus)
                Else
                    'The risk has already been copied and quoted  
                    nNewRiskCnt = nRiskId
                    iSelected = 1
                End If

                oDataBase.Parameters.Clear()
                m_lReturn = oDataBase.Parameters.Add(sName:="risk_cnt",
                                           vValue:=nNewRiskCnt,
                                           iDirection:=PMEParameterDirection.PMParamInput,
                                           iDataType:=PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="add parameter failed : risk_cnt", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = oDataBase.Parameters.Add(sName:="is_selected",
                                           vValue:=If(iSelected, 1, 0),
                                           iDirection:=PMEParameterDirection.PMParamInput,
                                           iDataType:=PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="add parameter failed : is_selected", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                ' Execute the stored procedure
                m_lReturn = oDataBase.SQLAction(sSQL:=ACUpdateRiskSelectionStatusSQL,
                                                  sSQLName:=ACUpdateRiskSelectionStatusName,
                                                  bStoredProcedure:=ACUpdateRiskSelectionStatusStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ACUpdateRiskSelectionStatusSQL failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End If
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ActionProcessRisk exception caught", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex, vErrNo:="", vErrDesc:=ex.InnerException.ToString)
            Return gPMConstants.PMEReturnCode.PMFalse

        Finally
            oDataBase.CloseDatabase()
            oDataBase = Nothing
        End Try

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    Public Sub DBConnect(ByRef oDatabase As dPMDAO.Database)
        Dim nReturn As Integer

        oDatabase = New dPMDAO.Database

        ' Connect to database
        nReturn = oDatabase.OpenDatabase(m_sUsername, m_iSourceID, m_iLanguageID, ACApp)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to connect to Sirius database")
        End If
    End Sub

    Private Function GetMultithreadLimit() As Integer
        Dim sThreadLimit As String = String.Empty
        bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=CInt("5147"), r_sOptionValue:=sThreadLimit)
        If Not String.IsNullOrEmpty(sThreadLimit) AndAlso CInt(sThreadLimit) > 1 Then
            Return CInt(sThreadLimit)
        Else
            Return 1
        End If
    End Function

    Public Class RiskData

        Public nInsuranceFileCnt As Integer
        Public sTransactionType As String
        Public oArray(,) As Object
        Public nCount As Integer
        Public bIsBackdatedMTA As Boolean
        Public dtMTAStartDate As Date
        Public nPreChangeInsFileCnt As Integer
        Public nPostChangeInsFileCnt As Integer
        Public bApplyRiskChange As Boolean
        Public nBaseInsuranceFileCnt As Integer
        Public dependentTransaction As DependentTransaction
        Public selectionArray As Object
    End Class

End Class
