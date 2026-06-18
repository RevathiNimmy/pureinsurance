Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Modified by Sumeet Singh on 5/11/2010 6:47:39 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("AutoMta_NET.AutoMta")> _
Public NotInheritable Class AutoMta

    ' History
    ' RAW 19/09/2003 : CQ2614 : Ensure that LeadCommission is updated when processing Agent Commission
    ' RAW 26/09/2003 : CQ828 : added more details to reinstated risk event description
    ' RAW 13/11/2003 : CQ1765 : pass OriginalInsuranceFileCnt & RunMode to bControlTrans object
    ' RAW 24/11/2003 : CQ685 : allow NilPremiumRefund to be set for multiple insurance file cnts
    ' RAW 20/01/2004 : CQ3535 : use new date as the end date when auto cancelling a policy version and keep the original start date
    ' RAW 18/02/2004 : CQ3665 : get the InsuranceFileCnt of the version that was originally cancelled when reinstating a policy
    ' RAW 02/03/2004 : CQ3665 : create business objects from another business object if they are to participate in the same DB transaction
    ' RAW 08/03/2004 : CQ4180 : fix bug when getting original insurance file cnt
    ' RAW 28/04/2004 : CQ5143 : clear GIS output before quoting
    '                           set CalledFromAutoMTA property for renewals
    ' RAW 05/05/2004 : CQ753 : include risk tax when calculating policy premium
    ' RAW 15/11/2003 : Pricing changes : pass original rating details to NBQuote

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

    Private m_lReturn As gPMConstants.PMEReturnCode

    'Objects
    Private m_oReinsurance As Object
    Private m_oRITax As Object
    Private m_oAgentCommission As Object
    Private m_oFindInsurance As Object
    Private m_oChangePolicyStatus As Object
    Private m_oFindRisk As Object
    Private m_oListRisks As Object
    Private m_oRiskData As Object
    Private m_oRenSelection As Object
    Private m_oPerilAllocation As Object
    Private m_oControlTrans As Object
    Private m_oAutomaticRenewalsSel As Object
    Private m_oAutomaticRenewalsAccept As Object
    Private m_oAutoMTAMerge As AutoMTAMerge
    Private m_oGIS As Object
    Private m_oDataset As cGISDataSetControl.Application
    Private m_oInsuranceFile As Object
    Private m_oDatabase As Object

    Private m_oListPolicies As Object
    Private m_oStatusBar As Object

    Private m_vAutoMTAInsFileCnts(,) As Object
    ' AffectedInsuranceFileCnts
    Private m_vAffectedInsuranceFileCnts As Array
    ' InsuranceFolderCnt
    Private m_lInsuranceFolderCnt As Integer
    ' TransactionType
    Private m_sTransactionType As String = ""
    ' EffectiveDate
    Private m_dtEffectiveDate As Date
    ' DeclineReasons
    Private m_vDeclineReasons As Object
    ' ReferReasons
    Private m_vReferReasons As Object
    'Messages
    Private m_vMessages(,) As Object
    'Policy status messages
    Private m_vPolicyStatusMessages(,) As Object

    Private m_lLastDeletedRiskCnt As Integer
    Private m_lLastDeletedInsuranceFileCnt As Integer

    Private m_sEventReason As String = ""

    ' UpdateStats
    Private m_bUpdateStats As Boolean
    ' NewInsuranceFileCnt
    Private m_lNewInsuranceFileCnt As Integer
    Private m_sStatusMessage As String = ""
    ' ObjectPropertyArray
    '  Private m_vObjectPropertyArray(,) As Object
    Private m_vObjectPropertyArray As Object
    ' CurrentDate
    Private m_dtCurrentDate As Date
    ' DeletedRiskInsuranceFileCnt
    Private m_lDeletedRiskInsuranceFileCnt As Integer
    ' DeletedRiskCnt
    Private m_lDeletedRiskCnt As Integer
    ' IsReinstateRisk
    Private m_bIsReinstateRisk As Boolean
    ' ReinstatementReason
    Private m_sReinstatementReason As String = ""
    ' PartyCnt
    Private m_lPartyCnt As Integer
    ' MergeRisks
    Private m_bMergeRisks As Boolean
    ' NCDReason
    Private m_sNCDReason As String = ""
    ' IsNCDChange
    Private m_bIsNCDChange As Boolean

    Private m_bIsPolicyReinstatement As Boolean

    Private m_lRunMode As Integer ' RAW 13/11/2003 : CQ1765 : added

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
    'Public Property Let DeletedRiskInsuranceFileCnt(lDeletedRiskInsuranceFileCnt As Long)
    '    m_lDeletedRiskInsuranceFileCnt& = lDeletedRiskInsuranceFileCnt&
    'End Property

    Public WriteOnly Property CurrentDate() As Date
        Set(ByVal Value As Date)
            m_dtCurrentDate = Value
        End Set
    End Property
    Public WriteOnly Property ObjectPropertyArray() As Object()
        Set(ByVal Value As Object())
            m_vObjectPropertyArray = Value
        End Set
    End Property
    Public ReadOnly Property MultipleVersionsExist() As Boolean
        Get
            Dim result As Boolean = False

            If Not Information.IsArray(m_vAutoMTAInsFileCnts) Then
                'Rebuild the internal arrays from the mta_insurance_file_link table
                m_lReturn = RebuildArrays()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildArrays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                    Return result
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return result
                End If
            End If

            If Information.IsArray(m_vAutoMTAInsFileCnts) Then
                If m_vAutoMTAInsFileCnts.GetUpperBound(1) > 0 Then
                    result = True
                End If
            End If

            Return result
        End Get
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
    Public ReadOnly Property Messages() As Object
        Get
            Return VB6.CopyArray(m_vMessages)
        End Get
    End Property
    Public ReadOnly Property PolicyStatusMessages() As Object
        Get
            Return VB6.CopyArray(m_vPolicyStatusMessages)
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
    Public Property AffectedInsuranceFileCnts() As Object
        Get
            Return m_vAffectedInsuranceFileCnts
        End Get
        Set(ByVal Value As Object)
            m_vAffectedInsuranceFileCnts = Value
        End Set
    End Property
    Public WriteOnly Property ListPolicies() As Object
        Set(ByVal Value As Object)
            m_oListPolicies = Value
        End Set
    End Property
    Public WriteOnly Property StatusBar() As Object
        Set(ByVal Value As Object)
            m_oStatusBar = Value
        End Set
    End Property

    ' RAW 13/11/2003 : CQ1765 : added
    Public WriteOnly Property RunMode() As Integer
        Set(ByVal Value As Integer)
            m_lRunMode = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' AUTOMATIC BACKDATED CANCELLATIONS
    ' ***************************************************************** '

    ' ***************************************************************** '
    '
    ' Name: AutoCancelMTA
    '
    ' Description: Automatically cancel a policy
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoCancelMTA(ByRef r_sErrorText As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean

            m_sEventReason = ACReasonBackdatedCancellation

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, v_bIsCancellation:=True, r_bBackdatingRequired:=bBackdatingRequired), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
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

            'Loop around the policy versions cancelling each one
            m_lReturn = CType(AutoCancelPolicyVersions(v_lVersion:=lPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Delete any policy versions we have created and set the status
                ' of the originals back to live
                m_lReturn = RestoreAutoRunMTA()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                End If
                Return result
            Else
                'Update the accounts
                If m_bUpdateStats Then
                    m_lReturn = TransactPolicyVersions()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AutoCancelPolicyVersions
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoCancelPolicyVersions(ByVal v_lVersion As Integer, ByVal v_dtEffectiveDate As Date, Optional ByVal v_lNewMTAInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim dtMTADate As Date
            Dim lNewInsuranceFileCnt As Integer
            Dim dtMTAEndDate As Date
            Dim sTransactionType As String = ""
            Dim bCopyPolicy, bFirst As Boolean
            Dim lFirstNewInsuranceFileCnt As Integer
            Dim bIsRealCancellation As Boolean

            Const klInsuranceFileStatusId_Cancelled As Integer = 1 ' RAW 20/01/2004 : CQ3535 : added



            m_vDeclineReasons = ""

            m_vReferReasons = ""
            bFirst = True

            'Get the list of affected policy versions in ascending order
            m_lReturn = CType(ReverseArray(r_vArray:=m_vAffectedInsuranceFileCnts), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' RAW 20/01/2004 : CQ3535 : added
            ' Remove entries from the end of the array that are already cancelled so that
            ' we can detect the last 'valid' version within the next loop.
            ' Those at the start or in the middle of the array will be left as they can be detected within the loop.
            ' We could have excluded these from the orignal sql query but were unsure about changing a 'core' function and didn't have time to dig any further
            For i As Integer = m_vAffectedInsuranceFileCnts.GetUpperBound(1) To 0 Step -1
                If CDbl(m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i)) = klInsuranceFileStatusId_Cancelled Then
                    If i <> 0 Then
                        m_vAffectedInsuranceFileCnts = ArraysHelper.RedimPreserve(Of Object(,))(m_vAffectedInsuranceFileCnts, New Integer() {m_vAffectedInsuranceFileCnts.GetUpperBound(0) - m_vAffectedInsuranceFileCnts.GetLowerBound(0) + 1, i - 1 - m_vAffectedInsuranceFileCnts.GetLowerBound(1) + 1}, New Integer() {m_vAffectedInsuranceFileCnts.GetLowerBound(0), m_vAffectedInsuranceFileCnts.GetLowerBound(1)})
                    End If
                Else
                    Exit For
                End If
            Next
            ' RAW 20/01/2004 : CQ3535 : end

            For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)

                bCopyPolicy = True
                bIsRealCancellation = False

                ' RAW 20/01/2004 : CQ3535 : added test for cancelled versions
                If CDbl(m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i)) = klInsuranceFileStatusId_Cancelled Then
                    ' ignore this version
                Else

                    ' RAW 20/01/2004 : CQ3535 : reworked this bit of code to do the same thing  but hopefully a little clearer

                    If bFirst Then

                        'Process the first policy version ( ie the oldest)

                        ' RAW 20/01/2004 : CQ3535 : use effective date as the end date instead of start date
                        'Cover start date remains as the start date of the version we are cancelling
                        dtMTADate = CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i))
                        ' Use the effective date as the end date of the policy
                        dtMTAEndDate = v_dtEffectiveDate
                        ' RAW 20/01/2004 : CQ3535 : end

                    Else
                        dtMTADate = CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i))
                        dtMTAEndDate = CDate(m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i))
                    End If

                    If bFirst Then

                        'This is a real cancellation rather than just reversing out the
                        'existing premiums
                        bIsRealCancellation = True

                        If m_lNewInsuranceFileCnt <> 0 Then
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


                    ' RAW 20/01/2004 : CQ3535 : added v_vMTAEndDate param
                    m_lReturn = CType(AutoRunCancellation(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), r_lVersion:=v_lVersion, v_dtMTAStartDate:=dtMTADate, v_vMTAEndDate:=dtMTAEndDate, v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_sTransactionType:=sTransactionType, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_bCopyPolicy:=bCopyPolicy, v_bIsRealCancellation:=bIsRealCancellation), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RAW 20/01/2004 : CQ3535 : end


                    If bFirst Then
                        'This is the first one

                        m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink(v_lInsuranceFileCnt:=lNewInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        lFirstNewInsuranceFileCnt = lNewInsuranceFileCnt
                        bFirst = False
                    End If

                    'Update the mta_insurance_file_link table
                    m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vCancelledLinkedInsuranceFileCnt:=lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions")
                        Return result
                    End If
                End If

            Next i


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelPolicyVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AutoRunCancellation
    '
    ' Description:
    '
    ' History: 06/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AutoRunCancellation(ByVal v_lBaseInsuranceFileCnt As Integer, ByRef r_lVersion As Integer, ByVal v_dtMTAStartDate As Date, ByVal v_sTransactionType As String, ByVal v_lExistingPolicyVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, Optional ByVal v_vMTAEndDate As Object = Nothing, Optional ByVal v_bCopyPolicy As Boolean = True, Optional ByVal v_bIsRealCancellation As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lCnt, lNewInsuranceFileCnt As Integer
        Dim sTransactionType As String = ""
        Dim bCopyDeletedLink As Boolean

        m_sStatusMessage = "Cancelling policy version " & v_lExistingPolicyVersion
        DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage)

        'Make a copy of the policy
        If v_bCopyPolicy Then
            bCopyDeletedLink = Not v_bIsRealCancellation

            m_lReturn = CType(CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate, v_vMTAEndDate:=v_vMTAEndDate, v_bCopyDeletedLink:=bCopyDeletedLink), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            lNewInsuranceFileCnt = m_lNewInsuranceFileCnt
        End If

        r_lNewInsuranceFileCnt = lNewInsuranceFileCnt

        'Keep a record of all the new policies we are creating
        If Information.IsArray(m_vAutoMTAInsFileCnts) Then
            lCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        Else
            lCnt = 0
            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        End If
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, lCnt) = lNewInsuranceFileCnt
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, lCnt) = "MTC"
        m_vAutoMTAInsFileCnts(ACAutoMTAVersion, lCnt) = r_lVersion + 1

        If v_bIsRealCancellation Then
            sTransactionType = "MTC"
        Else
            sTransactionType = "MTCR"
        End If

        'Quote all the risks
        m_lReturn = CType(ProcessRisks(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_sTransactionType:=sTransactionType, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Update the policy status
        m_lReturn = CType(ProcessChangePolicyStatus(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_sTransactionType:=v_sTransactionType), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Policy level tax
        m_lReturn = CType(ProcessRITax(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_lRiskCnt:=0), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Call the agent commission component
        m_lReturn = CType(ProcessAgentCommission(v_lInsuranceFileCnt:=lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_sTransactionType = "MTA" Then
            'Set the status of the new policy version to cancelled
            'Only do this for cancelled versions created during the MTA process, real
            'cancellations will have all there policy versions set to cancelled when the
            'policy is made live
            m_lReturn = CType(UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sInsuranceFileStatusCode:="CAN"), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return result
            End If
        Else
            'Update the insurance file type to cancelled when this is a real reinstateable
            'cancellation
            m_lReturn = CType(UpdateInsuranceFileType(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sInsuranceFileTypeCode:="MTACAN"), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunCancellation")
                Return result
            End If
        End If

        'Display the policy version
        If Not (m_oListPolicies Is Nothing) Then

            m_lReturn = m_oListPolicies.GetPolicies()
        End If

        'Increment the version number
        r_lVersion += 1

        Return result

    End Function

    ' ***************************************************************** '
    ' AUTOMATIC BACKDATED POLICY REINSTATEMENTS
    ' ***************************************************************** '

    ' ***************************************************************** '
    '
    ' Name: AutoReinstateMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoReinstateMTA(ByRef r_sErrorText As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean

            m_sEventReason = ACReasonBackdatedReinstatement

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, v_bIsReinstatement:=True, r_bBackdatingRequired:=bBackdatingRequired), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA")
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

            m_lReturn = CType(AutoRunReinstatement(v_lVersion:=lPolicyVersion), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Delete any policy versions we have created and set the status
                ' of the originals back to live
                m_lReturn = RestoreAutoRunMTA()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA")
                End If
                Return result
            Else
                'Update the accounts
                If m_bUpdateStats Then
                    m_lReturn = TransactPolicyVersions()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoReinstateMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AutoRunReinstatement
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoRunReinstatement(ByVal v_lVersion As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim dtMTADate As Date
            Dim lNewInsuranceFileCnt As Integer
            Dim lCnt As Integer
            Dim sTransactionType As String = ""
            Dim bCopyPolicy, bFirst As Boolean
            Dim lFirstNewInsuranceFileCnt, lOriginalLinkedInsuranceFileCnt As Integer
            Dim bIsRealReinstatement As Boolean

            lCnt = 0

            m_vDeclineReasons = ""

            m_vReferReasons = ""
            bFirst = True

            m_bIsPolicyReinstatement = True

            'Get the list of affected policy versions in ascending order
            m_lReturn = CType(ReverseArray(r_vArray:=m_vAffectedInsuranceFileCnts), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Loop around the affected policy versions
            For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)

                bCopyPolicy = True

                If i = 0 Then
                    If m_lNewInsuranceFileCnt <> 0 Then
                        ' We have already copied the first policy
                        bCopyPolicy = False
                    End If
                End If

                If i = 0 Then
                    'The first one will always be the real cancellation rather than the
                    ' MTAs which were reversed out
                    bIsRealReinstatement = True
                Else
                    bIsRealReinstatement = False
                End If

                m_lReturn = CType(AutoRunSingleReinstatement(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), r_lVersion:=v_lVersion, v_dtMTAStartDate:=CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)), v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_vMTAEndDate:=m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i), v_bCopyPolicy:=bCopyPolicy, v_bIsRealReinstatement:=bIsRealReinstatement), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If bFirst Then
                    'This is the first one
                    'Create the mta_insurance_file_link table

                    m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink(v_lInsuranceFileCnt:=lNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    lFirstNewInsuranceFileCnt = lNewInsuranceFileCnt
                    bFirst = False
                End If

                'Update the mta_insurance_file_link table
                m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=3, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vNewLinkedInsuranceFileCnt:=lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                    Return result
                End If

                'Get the original version that was cancelled
                ' RAW 08/03/2004 : CQ4180 : renamed and added v_bLookForCancelled param

                m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), r_lOriginalLinkedInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt, r_dtExpiryDate:=dtMTADate, v_bLookForCancelled:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.GetOriginalLinkedVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                    Return result
                End If

                ' Restore the status of the original version to live
                m_lReturn = CType(UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt, v_lInsuranceFileStatusId:=0), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                    Return result
                End If

                If i = m_vAffectedInsuranceFileCnts.GetUpperBound(1) Then
                    'This is the Last version
                    If dtMTADate < m_dtCurrentDate Then
                        'We need to create a renewal version
                        m_lReturn = CType(AutoRunRenewalsForReinstatement(v_lBaseInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_lInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="AutoRunRenewalsForReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                            Return result
                        End If

                    End If
                End If

                'Update the original cancelled record in the mta_insurance_file_link table
                'to processed
                m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=2, v_vCancelledLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vProcessedInd:=1), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement")
                    Return result
                End If

            Next i


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunReinstatement", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
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
    Private Function AutoRunSingleReinstatement(ByVal v_lBaseInsuranceFileCnt As Integer, ByRef r_lVersion As Integer, ByVal v_dtMTAStartDate As Date, ByVal v_lExistingPolicyVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, Optional ByVal v_vMTAEndDate As Object = Nothing, Optional ByVal v_bCopyPolicy As Boolean = True, Optional ByRef v_bIsRealReinstatement As Boolean = True) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lCnt, lNewInsuranceFileCnt As Integer
        Dim sTransactionType As String = ""

        'Update the status bar
        m_sStatusMessage = "Reinstating policy version " & v_lExistingPolicyVersion
        DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage)

        'Make a copy of the policy
        If v_bCopyPolicy Then
            m_lReturn = CType(CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate, v_vMTAEndDate:=v_vMTAEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunSingleReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunSingleReinstatement")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 03/02/2004 : moved from end of function
            'Increment the version number
            r_lVersion += 1

        Else
            lNewInsuranceFileCnt = m_lNewInsuranceFileCnt
        End If

        r_lNewInsuranceFileCnt = lNewInsuranceFileCnt

        'Keep a record of all the new policies we are creating
        If Information.IsArray(m_vAutoMTAInsFileCnts) Then
            lCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        Else
            lCnt = 0
            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        End If
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, lCnt) = lNewInsuranceFileCnt
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, lCnt) = "MTA"
        ' RAW 03/02/2004 : removed + 1 since now done earlier
        m_vAutoMTAInsFileCnts(ACAutoMTAVersion, lCnt) = r_lVersion

        If Not v_bIsRealReinstatement Then
            sTransactionType = "MTCR"
        Else
            sTransactionType = "MTR"
        End If

        'Quote all the risks
        m_lReturn = CType(ProcessRisks(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_sTransactionType:=sTransactionType, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunSingleReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Update the policy status
        m_lReturn = CType(ProcessChangePolicyStatus(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_sTransactionType:=m_sTransactionType), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunSingleReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Policy level tax
        m_lReturn = CType(ProcessRITax(v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_lRiskCnt:=0), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunSingleReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Call the agent commission component
        m_lReturn = CType(ProcessAgentCommission(v_lInsuranceFileCnt:=lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunSingleReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Update the insurance file type to reinstated
        m_lReturn = CType(UpdateInsuranceFileType(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sInsuranceFileTypeCode:="MTAREINS"), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunSingleReinstatement")
            Return result
        End If

        'Display the policy version
        If Not (m_oListPolicies Is Nothing) Then

            m_lReturn = m_oListPolicies.GetPolicies()
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' AUTOMATIC BACKDATED RISK REINSTATEMENTS
    ' ***************************************************************** '

    ' ***************************************************************** '
    '
    ' Name: AutoReinstateRisk
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoReinstateRisk(ByRef r_sErrorText As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean

            m_lLastDeletedRiskCnt = 0
            m_lLastDeletedInsuranceFileCnt = 0

            m_sEventReason = ACReasonBackdatedRiskReinstatement

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            ' RAW 13/11/2003 : CQ1765 : replace v_bIsCancellation=True param with v_bIsPermanentMTA param
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, r_bBackdatingRequired:=bBackdatingRequired, v_lDeletedRiskInsuranceFileCnt:=m_lDeletedRiskInsuranceFileCnt, v_bIsPermanentMTA:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk")
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

            m_lReturn = CType(AutoRunMTA(v_lVersion:=lPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate, v_lNewMTAInsuranceFileCnt:=m_lDeletedRiskInsuranceFileCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Delete any policy versions we have created and set the status
                ' of the originals back to live
                m_lReturn = RestoreAutoRunMTA()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk")
                End If
                Return result
            Else
                'Update the accounts
                If m_bUpdateStats Then
                    m_lReturn = TransactPolicyVersions()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoReinstateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' AUTOMATIC BACKDATED RISK CHANGE
    ' ***************************************************************** '

    ' ***************************************************************** '
    '
    ' Name: AutoRiskChangeMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoRiskChangeMTA(ByRef r_sErrorText As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, v_bIsCancellation:=True, r_bBackdatingRequired:=bBackdatingRequired), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA")
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

            m_lReturn = CType(AutoRunMTA(v_lVersion:=lPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate, v_bApplyRiskChange:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Delete any policy versions we have created and set the status
                ' of the originals back to live
                m_lReturn = RestoreAutoRunMTA()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA")
                End If
                Return result
            Else
                'Update the accounts
                If m_bUpdateStats Then
                    m_lReturn = TransactPolicyVersions()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRiskChangeMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' AUTOMATIC NCD CHANGE DUE TO CLAIM
    ' ***************************************************************** '

    ' ***************************************************************** '
    '
    ' Name: AutoNCDChangeMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoNCDChangeMTA(ByRef r_sErrorText As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean

            m_sEventReason = ACReasonReplaced & m_sNCDReason

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, v_bIsCancellation:=True, r_bBackdatingRequired:=bBackdatingRequired), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA")
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

            m_lReturn = CType(AutoRunMTA(v_lVersion:=lPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Delete any policy versions we have created and set the status
                ' of the originals back to live
                m_lReturn = RestoreAutoRunMTA()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA")
                End If
                Return result
            Else
                'Update the accounts
                If m_bUpdateStats Then
                    m_lReturn = TransactPolicyVersions()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoNCDChangeMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' AUTOMATIC BACKDATED MTA
    ' ***************************************************************** '

    ' ***************************************************************** '
    '
    ' Name: AutoBackdatedMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoBackdatedMTA(ByRef r_sErrorText As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lInsuranceFileCnt, lPolicyVersion, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean

            'See if we already have already created multiple backdated Mta
            'policy versions for this quote
            If MultipleVersionsExist Then
                Return result
            End If

            m_sEventReason = ACReasonBackdatedMTA

            'Assume this is a permanant MTA

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            ' RAW 13/11/2003 : CQ1765 : added v_bIsPermanentMTA param to replace the fudge by setting ErrorCode = 1
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, r_bBackdatingRequired:=bBackdatingRequired, v_bIsPermanentMTA:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA")
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

            m_lReturn = CType(AutoRunMTA(v_lVersion:=lPolicyVersion, v_dtEffectiveDate:=m_dtEffectiveDate, v_lNewMTAInsuranceFileCnt:=m_lNewInsuranceFileCnt, v_bIsBackdatedMTA:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA")
                result = gPMConstants.PMEReturnCode.PMFalse

                'Delete any policy versions we have created and set the status
                ' of the originals back to live
                m_lReturn = RestoreAutoRunMTA()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA")
                End If
                Return result
            Else
                'Update the accounts
                If m_bUpdateStats Then
                    m_lReturn = TransactPolicyVersions()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoBackdatedMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoBackdatedMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '' ***************************************************************** '
    '' Name: AutoRunMTA
    ''
    '' Description:
    ''
    '' History: 02/01/2003 sj - Created.
    ''
    '' ***************************************************************** '
    'Public Function AutoRunMTA( _
    ''    ByVal v_lVersion As Long, _
    ''    ByVal v_dtEffectiveDate As Date, _
    ''    Optional ByVal v_lNewMTAInsuranceFileCnt As Long, _
    ''    Optional ByVal v_bApplyRiskChange As Boolean = False) As Long
    '
    '    On Error GoTo Err_AutoRunMTA
    '
    '    AutoRunMTA = PMTrue
    '
    '    Dim oListRisks As Object
    '    Dim vKeyArray As Variant
    '    Dim i As Integer
    '    Dim dtMTADate As Date
    '    Dim lNewInsuranceFileCnt As Long
    '    Dim dtMTAEndDate As Date
    '    Dim dtMTAStartDate As Date
    '    Dim lCnt As Long
    '    Dim sTransactionType As String
    '    Dim bCopyPolicy As Boolean
    '    Dim bFirst As Boolean
    '    Dim bCancelled As Boolean
    '    Dim lFirstNewInsuranceFileCnt As Long
    '    Dim bMTAApplied As Boolean
    '
    '    lCnt = 0
    '    m_vDeclineReasons = ""
    '    m_vReferReasons = ""
    '    bFirst = True
    '
    '    'Get the list of affected policy versions in ascending order
    '    m_lReturn = ReverseArray( _
    ''        r_vArray:=m_vAffectedInsuranceFileCnts)
    '    If m_lReturn <> PMTrue Then
    '        LogMessage _
    ''           iType:=PMLogOnError, _
    ''           sMsg:="ReverseArray Failed", _
    ''           vApp:=ACApp, _
    ''           vClass:=ACClass, _
    ''           vMethod:="AutoRunMTA"
    '        AutoRunMTA = PMFalse
    '        Exit Function
    '    End If
    '
    '
    '    If m_bIsReinstateRisk = True Then
    '         'Reinstating a risk
    '         v_lNewMTAInsuranceFileCnt = m_lDeletedRiskInsuranceFileCnt
    '    End If
    '
    '    For i = 0 To UBound(m_vAffectedInsuranceFileCnts, 2)
    '
    '        bCopyPolicy = True
    '        bCancelled = False
    '        bMTAApplied = False
    '
    '        Select Case i
    '
    '            Case 0
    '                'this is the first record we are processing so no cancellation required
    '
    '            Case UBound(m_vAffectedInsuranceFileCnts, 2)
    '
    '               'Process the last policy version
    '
    ''               If i = 0 Then
    ''                   'We only have one record to process
    ''                   dtMTADate = v_dtEffectiveDate
    ''                   If m_lNewInsuranceFileCnt <> 0 Then
    ''                       ' We have already copied the first policy
    ''                       bCopyPolicy = False
    ''                    End If
    ''               Else
    '                   dtMTADate = m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)
    ''               End If
    '
    ''               If m_lNewInsuranceFileCnt > 0 Then
    ''                   'We are going through the cancellation road map so we do not want to
    ''                   'set all versions to cancelled at this stage
    ''                   sTransactionType = "MTCA"
    ''               Else
    ''                   If m_sTransactionType = "MTA" Then
    '                        sTransactionType = "MTCA"
    ''                   Else
    ''                       sTransactionType = "MTC"
    ''                   End If
    ''               End If
    '
    ''               If i = 0 Then
    ''                  'We have only one version and we are doing an MTA not a policy cancellation
    ''                  'so do nothing
    ''               Else
    '
    '                   m_lReturn = AutoRunCancellation( _
    ''                        v_lBaseInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), _
    ''                        r_lVersion:=v_lVersion, _
    ''                        v_dtMTAStartDate:=dtMTADate, _
    ''                        v_lExistingPolicyVersion:=m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i), _
    ''                        v_sTransactionType:=sTransactionType, _
    ''                        r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt)
    '                    If m_lReturn <> PMTrue Then
    '                        LogMessage _
    ''                           iType:=PMLogOnError, _
    ''                           sMsg:="AutoRunCancellation Failed", _
    ''                           vApp:=ACApp, _
    ''                           vClass:=ACClass, _
    ''                           vMethod:="AutoRunMTA"
    '                        AutoRunMTA = PMFalse
    '                        Exit Function
    '                    End If
    '
    '                    bCancelled = True
    ''                End If
    '
    '            Case Else
    '                'If we have MTAs done after the effective date then the expiry date will
    '                ' be the day before the start of the next MTA
    ''                If Trim(m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, (i + 1))) = "MTA TEMP" Then
    ''                    'Temporary MTA has it's own end date
    ''                    dtMTAEndDate = _
    '''                        m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, (i + 1))
    ''                Else
    ''                    dtMTAEndDate = _
    '''                        DateAdd("d", -1, m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, (i + 1)))
    ''                End If
    '
    '                m_lReturn = AutoRunCancellation( _
    ''                    v_lBaseInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), _
    ''                    r_lVersion:=v_lVersion, _
    ''                    v_dtMTAStartDate:=m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i), _
    ''                    v_lExistingPolicyVersion:=m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i), _
    ''                    v_sTransactionType:="MTCA", _
    ''                    r_lNewInsuranceFileCnt:=lNewInsuranceFileCnt)
    '
    '                If m_lReturn <> PMTrue Then
    '                    LogMessage _
    ''                       iType:=PMLogOnError, _
    ''                       sMsg:="AutoRunCancellation Failed", _
    ''                       vApp:=ACApp, _
    ''                       vClass:=ACClass, _
    ''                       vMethod:="AutoRunMTA"
    '                    AutoRunMTA = PMFalse
    '                    Exit Function
    '                End If
    '                bCancelled = True
    '        End Select
    '
    '        If bCancelled = True Then
    '            'We have run the cancellation process
    '            If bFirst = True Then
    '                'This is the first one
    '                m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink( _
    ''                    v_lInsuranceFileCnt:=lNewInsuranceFileCnt)
    '                If m_lReturn <> PMTrue Then
    '                    LogMessage _
    ''                        iType:=PMError, _
    ''                        sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", _
    ''                        vApp:=ACApp, _
    ''                        vClass:=ACClass, _
    ''                        vMethod:="AutoRunMTA"
    '                    AutoRunMTA = PMFalse
    '                    Exit Function
    '                End If
    '                lFirstNewInsuranceFileCnt = lNewInsuranceFileCnt
    '                bFirst = False
    '            End If
    '
    '            'Update the mta_insurance_file_link table
    '            m_lReturn = UpdateMTAInsuranceFileLink( _
    ''                v_iType:=1, _
    ''                v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, _
    ''                v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), _
    ''                v_vCancelledLinkedInsuranceFileCnt:=lNewInsuranceFileCnt _
    ''                )
    '            If m_lReturn <> PMTrue Then
    '                AutoRunMTA = PMFalse
    '                LogMessage _
    ''                    iType:=PMError, _
    ''                    sMsg:="UpdateMTAInsuranceFileLink Failed", _
    ''                    vApp:=ACApp, _
    ''                    vClass:=ACClass, _
    ''                    vMethod:="AutoRunMTA"
    '                Exit Function
    '            End If
    '        End If
    '
    '        If i > 0 Then
    '            'This is an MTA so we need to reapply the existing MTA changes to
    '            ' the new policy version
    '            m_lReturn = AutoRunMTAOnly( _
    ''                v_lPreChangeInsFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i - 1), _
    ''                v_lPostChangeInsFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), _
    ''                v_lBaseInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, _
    ''                v_lExistingPolicyVersion:=m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i), _
    ''                v_dtMTAStartDate:=m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i), _
    ''                r_lVersion:=v_lVersion, _
    ''                r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt)
    '            If m_lReturn <> PMTrue Then
    '                LogMessage _
    ''                   iType:=PMLogOnError, _
    ''                   sMsg:="AutoRunMTAOnly Failed", _
    ''                   vApp:=ACApp, _
    ''                   vClass:=ACClass, _
    ''                   vMethod:="AutoRunMTA"
    '                AutoRunMTA = PMFalse
    '                Exit Function
    '            End If
    '
    '            bMTAApplied = True
    '
    '        Else
    '            If i = 0 _
    ''            And v_bApplyRiskChange = True Then
    '                m_lReturn = AutoRunMTAOnly( _
    ''                    v_lBaseInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), _
    ''                    v_lExistingPolicyVersion:=m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i), _
    ''                    v_dtMTAStartDate:=m_dtEffectiveDate, _
    ''                    r_lVersion:=v_lVersion, _
    ''                    r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, _
    ''                    v_bApplyRiskChange:=v_bApplyRiskChange)
    '                If m_lReturn <> PMTrue Then
    '                    LogMessage _
    ''                       iType:=PMLogOnError, _
    ''                       sMsg:="AutoRunMTAOnly Failed", _
    ''                       vApp:=ACApp, _
    ''                       vClass:=ACClass, _
    ''                       vMethod:="AutoRunMTA"
    '                    AutoRunMTA = PMFalse
    '                    Exit Function
    '                End If
    '
    '                bMTAApplied = True
    '            End If
    '        End If
    '
    '        If bMTAApplied = True Then
    '            'We have run the mta process
    '            If bFirst = True Then
    '                'This is the first one
    '                m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink( _
    ''                    v_lInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt)
    '                If m_lReturn <> PMTrue Then
    '                    LogMessage _
    ''                        iType:=PMError, _
    ''                        sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", _
    ''                        vApp:=ACApp, _
    ''                        vClass:=ACClass, _
    ''                        vMethod:="AutoRunMTA"
    '                    AutoRunMTA = PMFalse
    '                    Exit Function
    '                End If
    '                lFirstNewInsuranceFileCnt = v_lNewMTAInsuranceFileCnt
    '                bFirst = False
    '            End If
    '
    '            'Update the mta_insurance_file_link table
    '            m_lReturn = UpdateMTAInsuranceFileLink( _
    ''                v_iType:=1, _
    ''                v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, _
    ''                v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), _
    ''                v_vNewLinkedInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt _
    ''                )
    '            If m_lReturn <> PMTrue Then
    '                AutoRunMTA = PMFalse
    '                LogMessage _
    ''                    iType:=PMError, _
    ''                    sMsg:="UpdateMTAInsuranceFileLink Failed", _
    ''                    vApp:=ACApp, _
    ''                    vClass:=ACClass, _
    ''                    vMethod:="AutoRunMTA"
    '                Exit Function
    '            End If
    '        End If
    '
    '    Next i
    '
    '
    '    Exit Function
    '
    'Err_AutoRunMTA:
    '
    '    AutoRunMTA = PMFalse
    '
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="AutoRunMTA Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="AutoRunMTA", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    'Resume
    'End Function

    ' ***************************************************************** '
    ' Name: AutoRunMTA
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoRunMTA(ByVal v_lVersion As Integer, ByVal v_dtEffectiveDate As Date, Optional ByVal v_lNewMTAInsuranceFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim lCancelledInsuranceFileCnt, lCnt As Integer
            Dim bFirst, bCancelled As Boolean
            Dim lFirstNewInsuranceFileCnt As Integer
            Dim bMTAApplied As Boolean
            Dim lNewTempMTAInsuranceFileCnt, lNewMTAInsuranceFileCnt As Integer

            lCnt = 0

            m_vDeclineReasons = ""

            m_vReferReasons = ""
            bFirst = True

            'Get the list of affected policy versions in ascending order
            m_lReturn = CType(ReverseArray(r_vArray:=m_vAffectedInsuranceFileCnts), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)

                bCancelled = False
                bMTAApplied = False

                If i > 0 Then

                    '*************************************************************************
                    'Create cancelled version of Original MTA
                    '*************************************************************************
                    If m_bIsReinstateRisk And v_lNewMTAInsuranceFileCnt = m_lDeletedRiskInsuranceFileCnt Then
                        'Don't cancel
                    Else
                        'First cancel the original poliy version
                        m_lReturn = CType(AutoRunCancellation(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), r_lVersion:=v_lVersion, v_dtMTAStartDate:=CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)), v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_sTransactionType:="MTCA", r_lNewInsuranceFileCnt:=lCancelledInsuranceFileCnt), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
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
                        m_lReturn = CType(AutoRunRenewalsForMTA(v_lPreChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i - 1)), v_lPostChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lBaseInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunRenewalsForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    ElseIf CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, i)).Trim() = "MTA TEMP" Then
                        'Temporary MTA
                        m_lReturn = CType(AutoRunMTAOnly(v_lPreChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i - 1)), v_lPostChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lBaseInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_dtMTAStartDate:=CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)), r_lVersion:=v_lVersion, r_lNewInsuranceFileCnt:=lNewTempMTAInsuranceFileCnt, v_vMTAEndDate:=m_vAffectedInsuranceFileCnts(ACAffectedExpiryDate, i)), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTAOnly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Else
                        'REapply the MTA
                        m_lReturn = CType(AutoRunMTAOnly(v_lPreChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i - 1)), v_lPostChangeInsFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lBaseInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_dtMTAStartDate:=CDate(m_vAffectedInsuranceFileCnts(ACAffectedCoverStartDate, i)), r_lVersion:=v_lVersion, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTAOnly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    bMTAApplied = True
                End If

                If i = 0 Then

                    If v_bApplyRiskChange Then
                        '*************************************************************************
                        'Auto apply risk change - generate the first MTA
                        '*************************************************************************
                        m_lReturn = CType(AutoRunMTAOnly(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_dtMTAStartDate:=m_dtEffectiveDate, r_lVersion:=v_lVersion, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_bApplyRiskChange:=v_bApplyRiskChange), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTAOnly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        bMTAApplied = True

                    ElseIf m_bIsNCDChange Then
                        '*************************************************************************
                        'NCD Change Due to Claim - auto generate the first MTA
                        '*************************************************************************
                        m_lReturn = CType(AutoRunMTAOnly(v_lBaseInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lExistingPolicyVersion:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedPolicyVersion, i)), v_dtMTAStartDate:=m_dtEffectiveDate, r_lVersion:=v_lVersion, r_lNewInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_bApplyRiskChange:=v_bApplyRiskChange), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTAOnly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Record a reason the auto generated MTA
                        m_lReturn = CType(CreateEvent(v_lInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt, v_sReason:=m_sNCDReason), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return result
                        End If

                        bMTAApplied = True

                    ElseIf v_bIsBackdatedMTA Then
                        '*************************************************************************
                        'This is the actual MTA quote so we need to update the premium on
                        'the insurance_file record before displaying on the screen
                        '*************************************************************************

                        m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lNewMTAInsuranceFileCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.UpdatePolicyPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMta")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        bMTAApplied = True
                    End If
                End If

                If bMTAApplied Then
                    'We have run the mta process
                    If bFirst Then
                        'This is the first one
                        lFirstNewInsuranceFileCnt = v_lNewMTAInsuranceFileCnt

                        m_lReturn = m_oFindInsurance.CreateMTAInsuranceFileLink(v_lInsuranceFileCnt:=lFirstNewInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.CreateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        bFirst = False
                    End If

                    If bCancelled Then
                        'Update the mta_insurance_file_link table with the cancelled insurance_file_cnt
                        m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vCancelledLinkedInsuranceFileCnt:=lCancelledInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                            Return result
                        End If
                    End If

                    If CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileType, i)).Trim() = "MTA TEMP" Then
                        lNewMTAInsuranceFileCnt = lNewTempMTAInsuranceFileCnt
                    Else
                        lNewMTAInsuranceFileCnt = v_lNewMTAInsuranceFileCnt
                    End If
                    'Update the mta_insurance_file_link table with the new insurance_file_cnt
                    m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=1, v_vInsuranceFileCnt:=lFirstNewInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vNewLinkedInsuranceFileCnt:=lNewMTAInsuranceFileCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA")
                        Return result
                    End If
                End If

            Next i


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = m_oFindInsurance.UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceFileStatusCode:=v_sInsuranceFileStatusCode)
        Else

            m_lReturn = m_oFindInsurance.UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFileStatusId:=v_lInsuranceFileStatusId)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileStatus")
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


        m_lReturn = m_oFindInsurance.UpdateInsuranceFileType(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceFileTypeCode:=v_sInsuranceFileTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.UpdateInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileType")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AutoRunRenewalsForReinstatement
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AutoRunRenewalsForReinstatement(ByVal v_lBaseInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lNewInsuranceFileCnt, lUbound As Integer

        'Run the selection process

        m_oAutomaticRenewalsSel.InsuranceFileCnt = v_lInsuranceFileCnt

        m_oAutomaticRenewalsSel.dtSelectionDate = DateTime.Now.AddDays(2000)


        m_lReturn = m_oAutomaticRenewalsSel.RenewalSelectionForMTA
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutomaticRenewalsSel.RenewalSelectionForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If m_oAutomaticRenewalsSel.FailureReason <> "" Then

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunRenewalsForReinstatement Failed: - " & m_oAutomaticRenewalsSel.FailureReason, vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        lNewInsuranceFileCnt = m_oAutomaticRenewalsSel.NewInsuranceFileCnt

        If lNewInsuranceFileCnt = 0 Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create new renewal policy version ", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Run the accept process (but do not update any transactions)

        m_oAutomaticRenewalsAccept.InsuranceFileCnt = lNewInsuranceFileCnt

        m_lReturn = m_oAutomaticRenewalsAccept.RenewalAcceptForMTA
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutomaticRenewalsAccept.RenewalAcceptForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add an entry to the mta_insurance_file_link table

        m_lReturn = m_oFindInsurance.AddToMTAInsuranceFileLink(v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=v_lInsuranceFileCnt, v_vNewLinkedInsuranceFileCnt:=lNewInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.AddToMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForReinstatement")
            Return result
        End If

        'Add an entry to the array for processing transactions
        lUbound = m_vAutoMTAInsFileCnts.GetUpperBound(1)
        ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lUbound + 1)
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, lUbound + 1) = lNewInsuranceFileCnt
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, lUbound + 1) = "RENEWAL"

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AutoRunRenewalsForMTA
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AutoRunRenewalsForMTA(ByVal v_lBaseInsuranceFileCnt As Integer, ByVal v_lPreChangeInsFileCnt As Integer, ByVal v_lPostChangeInsFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lNewInsuranceFileCnt, lUbound As Integer
        Dim iRunMode As Integer
        Dim bMergeRisks As Boolean

        'Run the selection process

        m_oAutomaticRenewalsSel.InsuranceFileCnt = v_lBaseInsuranceFileCnt

        m_oAutomaticRenewalsSel.dtSelectionDate = DateTime.Now.AddDays(2000)

        iRunMode = 0
        bMergeRisks = True
        If m_bIsReinstateRisk Then
            bMergeRisks = False
            If v_lPostChangeInsFileCnt = m_lDeletedRiskInsuranceFileCnt Then
                iRunMode = 1
            Else
                iRunMode = 2
            End If
        End If

        If m_bIsNCDChange Then
            iRunMode = 2
        End If



        m_lReturn = m_oAutomaticRenewalsSel.RenewalSelectionForMTA(v_bMergeRisks:=bMergeRisks, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, r_lDeletedRiskInsuranceFileCnt:=m_lDeletedRiskInsuranceFileCnt, r_lDeletedRiskCnt:=m_lDeletedRiskCnt, v_iRunMode:=iRunMode)

        ' RAW 28/04/2004 : CQ5143 : moved
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutomaticRenewalsSel.RenewalSelectionForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RAW 28/04/2004 : CQ5143 : moved

        If m_oAutomaticRenewalsSel.FailureReason <> "" Then

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRunRenewalsForMTA Failed: - " & m_oAutomaticRenewalsSel.FailureReason, vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        lNewInsuranceFileCnt = m_oAutomaticRenewalsSel.NewInsuranceFileCnt

        If lNewInsuranceFileCnt = 0 Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create new renewal policy version ", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add an entry to the array for processing transactions
        If lNewInsuranceFileCnt > 0 Then
            lUbound = m_vAutoMTAInsFileCnts.GetUpperBound(1)
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lUbound + 1)
            m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, lUbound + 1) = lNewInsuranceFileCnt
            m_vAutoMTAInsFileCnts(ACAutoMTATransType, lUbound + 1) = "RENEWAL"
        End If

        r_lNewInsuranceFileCnt = lNewInsuranceFileCnt

        'Run the accept process (but do not update any transactions)

        m_oAutomaticRenewalsAccept.InsuranceFileCnt = lNewInsuranceFileCnt


        m_lReturn = m_oAutomaticRenewalsAccept.RenewalAcceptForMTA
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutomaticRenewalsAccept.RenewalAcceptForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    'Add an entry to the mta_insurance_file_link table
        '    m_lReturn = m_oFindInsurance.AddToMTAInsuranceFileLink( _
        ''        v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, _
        ''        v_vOriginalLinkedInsuranceFileCnt:=v_lInsuranceFileCnt, _
        ''        v_vNewLinkedInsuranceFileCnt:=lNewInsuranceFileCnt _
        ''        )
        '    If m_lReturn <> PMTrue Then
        '        AutoRunRenewalsForMTA = PMFalse
        '        LogMessage _
        ''            iType:=PMError, _
        ''            sMsg:="m_oFindInsurance.AddToMTAInsuranceFileLink Failed", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="AutoRunRenewalsForMTA"
        '        Exit Function
        '    End If



        If v_lPostChangeInsFileCnt > 0 Then
            'This is the original version of the MTA which has just been reapplied
            'so set the status to replaced
            m_lReturn = CType(UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sInsuranceFileStatusCode:="REP"), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
                Return result
            End If

            'Record a reason against this replaced record
            m_lReturn = CType(CreateEvent(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sReason:=m_sEventReason), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunRenewalsForMTA")
                Return result
            End If

        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: TransactPolicyVersions
    '
    ' Description:
    '
    ' History: 02/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function TransactPolicyVersions() As Integer

        Dim result As Integer = 0
        Dim bTransStarted As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iStart As Integer
            Dim lOriginalLinkedInsuranceFileCnt As Integer

            If Not Information.IsArray(m_vAutoMTAInsFileCnts) And m_lNewInsuranceFileCnt <> 0 Then
                'Rebuild the internal arrays from the mta_insurance_file_link table
                m_lReturn = RebuildArrays()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildArrays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            '    'If we are going via the road map then the original version will be
            '    'transacted using the existing methods so do not process this version
            '    If m_lNewInsuranceFileCnt > 0 Then
            '        iStart = 1
            '    Else
            '        iStart = 0
            '    End If

            iStart = 0

            'Start a new transaction

            m_lReturn = m_oControlTrans.BeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bTransStarted = True ' RAW 20/01/2004 : CQ3535 : added


            m_oControlTrans.RunMode = m_lRunMode ' RAW 13/11/2003 : CQ1765 : added

            ' RAW 20/01/2004 : CQ3535 : added
            If Information.IsArray(m_vAutoMTAInsFileCnts) Then

                ' For each of the new insurance files raised during this process
                For i As Integer = iStart To m_vAutoMTAInsFileCnts.GetUpperBound(1)

                    'Update the status bar
                    If Not (m_oListPolicies Is Nothing) Then

                        m_oListPolicies.statustext = "Updating transactions for  policy version " & _
                                                     CStr(m_vAutoMTAInsFileCnts(ACAutoMTAVersion, i))
                    End If


                    ' RAW 18/02/2004 : CQ3665 : added
                    'Get the original version that was cancelled
                    If m_bIsPolicyReinstatement Then

                        ' When reinstating a policy we will have raised only one new insurance file
                        ' so m_vAutoMTAInsFileCnts will only contain one element.
                        ' There may be more than one insurance file to be reinstated but they are
                        ' themselves all linked to the cancellation insurance file that was raised
                        ' during the original cancellation. It is this cancellation file that is
                        ' in the m_vAffectedInsuranceFileCnts array.
                        ' Because there should be a 1:1 match between the two arrays we can use the
                        ' same index for both
                        '
                        ' Get the Insurance file linked to the cancellation that is about
                        ' to be reinstated
                        ' RAW 08/03/2004 : CQ4180 : renamed and added v_bLookForCancelled param

                        m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), r_lOriginalLinkedInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt, v_bLookForCancelled:=True)

                    Else
                        ' RAW 08/03/2004 : CQ4180 : added
                        If CStr(m_vAutoMTAInsFileCnts(ACAutoMTATransType, i)) = "MTC" Then

                            ' Get the original insurance file affected by this new cancellation Insurance File

                            m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), r_lOriginalLinkedInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt, v_bLookForCancelled:=True)
                        Else
                            ' Get the original insurance file affected by this new Insurance File

                            m_lReturn = m_oFindInsurance.GetOriginalLinkedVersion(v_lNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i), r_lOriginalLinkedInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt, v_bLookForCancelled:=False)
                        End If
                        ' RAW 08/03/2004 : CQ4180 : end
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oFindInsurance.GetOriginalLinkedVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                        'Rollback the transaction

                        m_oControlTrans.RollbackTrans()
                        Return result
                    End If
                    ' RAW 18/02/2004 : CQ3665 : end

                    'Create the stats tables and update the accounts in Orion
                    ' RAW 13/11/2003 : CQ1765 : added v_lOriginalInsuranceFileCnt param
                    ' RAW 18/02/2004 : CQ3665 : changed v_lOriginalLinkedInsuranceFileCnt param
                    m_lReturn = CType(ProcessStats(v_lInsuranceFileCnt:=CInt(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)), v_sTransactionType:=CStr(m_vAutoMTAInsFileCnts(ACAutoMTATransType, i)), v_lOriginalInsuranceFileCnt:=lOriginalLinkedInsuranceFileCnt), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Rollback the transaction

                        m_lReturn = m_oControlTrans.RollbackTrans
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next i
            End If

            If m_bIsReinstateRisk Then
                'Instead of deleting set the insurance_fiel_risk_link status to "R"
                'To prevent it being reselected

                m_lReturn = m_oRiskData.UpdateInsuranceFileRiskLink(v_lInsuranceFileCnt:=m_lDeletedRiskInsuranceFileCnt, v_lRiskCnt:=m_lDeletedRiskCnt, v_sStatusFlag:="R")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    m_oControlTrans.RollbackTrans() ' RAW 20/01/2004 : CQ3535 : added
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="DeleteInsuranceFileRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                    Return result
                End If

                '        'If we are reinstating a risk we need to delete the deleted link
                '        ' from the original policy version to prevent the risk being
                '        'reinstated again
                '        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink( _
                ''            v_lInsuranceFileCnt:=m_lDeletedRiskInsuranceFileCnt, _
                ''            v_lRiskCnt:=m_lDeletedRiskCnt _
                ''            )
                '        If m_lReturn <> PMTrue Then
                '            TransactPolicyVersions = PMFalse
                '            LogMessage _
                ''                iType:=PMError, _
                ''                sMsg:="DeleteInsuranceFileRiskLink Failed", _
                ''                vApp:=ACApp, _
                ''                vClass:=ACClass, _
                ''                vMethod:="TransactPolicyVersions"
                '            Exit Function
                '        End If
            End If

            If m_bIsPolicyReinstatement Then
                'Restore the policy status back to what it was prior to cancellation on
                'all the cancelled policy versions

                m_lReturn = m_oFindInsurance.RestorePolicyStatusAfterPolicyReinstatement(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_oControlTrans.RollbackTrans() ' RAW 20/01/2004 : CQ3535 : added
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.RestorePolicyStatusAfterPolicyReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Commit the transaction

            m_lReturn = m_oControlTrans.CommitTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bTransStarted = False ' RAW 20/01/2004 : CQ3535 : added

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 20/01/2004 : CQ3535 : added
            If bTransStarted Then

                m_oControlTrans.RollbackTrans()
            End If

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
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

        Dim vResultArray(,) As Object
        Dim sType As String = ""
        Dim lUbound As Integer

        'Rebuild the internal arrays from the mta_insurance_file_link table

        m_lReturn = m_oFindInsurance.RebuildArrayFromLinkedPolicyVersions(r_vResultArray:=vResultArray, v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="RebuildArrayFromLinkedPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RebuildArrays")
            Return result
        End If

        If Not Information.IsArray(vResultArray) Then
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

    ' ***************************************************************** '
    '
    ' Name: RestoreAutoRunMTA
    '
    ' Description:
    '
    ' History: 06/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RestoreAutoRunMTA(Optional ByRef v_bKeepQuote As Boolean = True) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If (Not Information.IsArray(m_vAutoMTAInsFileCnts) Or Not Information.IsArray(m_vAffectedInsuranceFileCnts)) And m_lNewInsuranceFileCnt <> 0 Then
                'Rebuild the internal arrays from the mta_insurance_file_link table
                m_lReturn = RebuildArrays()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildArrays Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    Return result
                End If
            End If

            If Not Information.IsArray(m_vAutoMTAInsFileCnts) Or Not Information.IsArray(m_vAffectedInsuranceFileCnts) Then
                Return result
            End If

            'If we are going via the road map then we want to keep the first version
            'as quote
            '    If m_lNewInsuranceFileCnt > 0 Then
            '        iStart = 1
            '    Else
            '        iStart = 0
            '    End If

            For i As Integer = 0 To m_vAffectedInsuranceFileCnts.GetUpperBound(1)

                'Set the policy status of the affected versions back to what they were before
                m_lReturn = CType(UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=CInt(m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i)), v_lInsuranceFileStatusId:=CInt(Conversion.Val(CStr(m_vAffectedInsuranceFileCnts(ACAffectedInsFileStatus, i))))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Update the original cancelled record in the mta_insurance_file_link table
                'BACK to unprocessed
                If m_bIsPolicyReinstatement Then
                    m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=2, v_vCancelledLinkedInsuranceFileCnt:=m_vAffectedInsuranceFileCnts(ACAffectedInsuranceFileCnt, i), v_vProcessedInd:=0), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                        Return result
                    End If
                End If
            Next i

            For i As Integer = 0 To m_vAutoMTAInsFileCnts.GetUpperBound(1)
                If CDbl(m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i)) <> m_lNewInsuranceFileCnt Or Not v_bKeepQuote Then
                    'Delete all the policy versions we have created

                    m_lReturn = m_oRenSelection.DeletePolicy(v_lInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenewalSelection.DeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Next i

            If v_bKeepQuote And m_lNewInsuranceFileCnt > 0 Then
                'Update the status of the original quote back to quoted
                m_lReturn = CType(UpdateInsuranceFileType(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt, v_sInsuranceFileTypeCode:="MTAQUOTE"), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Delete the mta_insurance_file_link records we have created
            If CStr(m_vAutoMTAInsFileCnts(ACAutoMTATransType, 0)) = "MTC" Then
                m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=5, v_vCancelledLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, 0)), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(UpdateMTAInsuranceFileLink(v_iType:=5, v_vNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, 0)), gPMConstants.PMEReturnCode)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateMTAInsuranceFileLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                Return result
            End If
            'Modified by Sumeet Singh on 5/11/2010 6:53:38 PM refer developer guide no. 146 (Latest Guide)
            'm_vAutoMTAInsFileCnts = VB6.CopyArray("")
            m_vAutoMTAInsFileCnts = Nothing
            'Modified by Sumeet Singh on 5/11/2010 6:54:31 PM refer developer guide no. 146
            'm_vAffectedInsuranceFileCnts = ""
            m_vAffectedInsuranceFileCnts = Nothing

            '     For i = 0 To UBound(m_vAutoMTAInsFileCnts, 2)
            '        'Delete the mta_insurance_file_link records we have created
            '        If m_vAutoMTAInsFileCnts(ACAutoMTATransType, i) = "MTC" Then
            '            m_lReturn = UpdateMTAInsuranceFileLink( _
            ''                v_iType:=4, _
            ''                v_vCancelledLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i) _
            ''                )
            '        Else
            '            m_lReturn = UpdateMTAInsuranceFileLink( _
            ''                v_iType:=4, _
            ''                v_vNewLinkedInsuranceFileCnt:=m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, i) _
            ''                )
            '        End If
            '        If m_lReturn <> PMTrue Then
            '            RestoreAutoRunMTA = PMFalse
            '            LogMessage _
            ''                iType:=PMError, _
            ''                sMsg:="UpdateMTAInsuranceFileLink Failed", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="RestoreAutoRunMTA"
            '            Exit Function
            '        End If
            '    Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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
    Private Function AutoRunMTAOnly(ByVal v_lBaseInsuranceFileCnt As Integer, ByVal v_dtMTAStartDate As Date, ByVal v_lExistingPolicyVersion As Integer, ByRef r_lVersion As Integer, ByRef r_lNewInsuranceFileCnt As Integer, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_vMTAEndDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lCnt As Integer

        'Update the status bar
        m_sStatusMessage = "Applying MTA to  policy version " & v_lExistingPolicyVersion
        DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage)

        'Make a copy of the policy

        If m_bIsReinstateRisk And m_lDeletedRiskInsuranceFileCnt = v_lBaseInsuranceFileCnt Then
            m_lReturn = CType(CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate, v_bCopyDeletedLink:=True), gPMConstants.PMEReturnCode)

        ElseIf Not Information.IsNothing(v_vMTAEndDate) Then
            'Temporary MTA
            m_lReturn = CType(CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=False, v_dtMTADate:=v_dtMTAStartDate, v_vMTAEndDate:=v_vMTAEndDate), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(CopyPolicy(v_lOldInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=r_lVersion, v_bPermanentMTA:=True, v_dtMTADate:=v_dtMTAStartDate), gPMConstants.PMEReturnCode)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'If we are doing a risk reinstatement record the reason against the policy version
        'we are reinstating
        If m_bIsReinstateRisk And m_lDeletedRiskInsuranceFileCnt = v_lBaseInsuranceFileCnt And m_sReinstatementReason <> "" Then

            m_lNewInsuranceFileCnt = r_lNewInsuranceFileCnt

            ' RAW 26/09/2003 : CQ828 : removed creation of event to within ProcessRisksForRiskReinstatement
            '        m_lReturn& = m_oFindInsurance.CreateEvent( _
            ''            vPartyCnt:=m_lPartyCnt, _
            ''            vInsuranceFolderCnt:=m_lInsuranceFolderCnt, _
            ''            vInsuranceFileCnt:=r_lNewInsuranceFileCnt, _
            ''            vEventTypeCode:="POLCHANGE", _
            ''            vDescription:=m_sReinstatementReason, _
            ''            vUserID:=g_iUserID)
            '        If m_lReturn <> PMTrue Then
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="m_oFindInsurance.CreateEvent Failed", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="AutoRunMTAOnly"
            '            AutoRunMTAOnly = PMFalse
            '            Exit Function
            '        End If
        End If

        ' Check the return value.
        'Keep a record of all the new policies we are creating
        If Information.IsArray(m_vAutoMTAInsFileCnts) Then
            lCnt = m_vAutoMTAInsFileCnts.GetUpperBound(1) + 1
            ReDim Preserve m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        Else
            lCnt = 0
            ReDim m_vAutoMTAInsFileCnts(ACAutoMTAArraySize, lCnt)
        End If
        m_vAutoMTAInsFileCnts(ACAutoMTAInsFileCnt, lCnt) = r_lNewInsuranceFileCnt
        m_vAutoMTAInsFileCnts(ACAutoMTATransType, lCnt) = "MTA"
        m_vAutoMTAInsFileCnts(ACAutoMTAVersion, lCnt) = r_lVersion + 1


        'Quote all the risks
        If m_bIsReinstateRisk Then
            m_lReturn = CType(ProcessRisksForRiskReinstatement(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_dtMTAStartDate:=v_dtMTAStartDate, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt), gPMConstants.PMEReturnCode)
        ElseIf m_bIsNCDChange And v_lPostChangeInsFileCnt > 0 Then
            m_lReturn = CType(ProcessRisksForNCDChange(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sTransactionType:="MTACR", v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt), gPMConstants.PMEReturnCode)
        ElseIf m_bMergeRisks And Not v_bApplyRiskChange Then
            m_lReturn = CType(ProcessRisksWithMerge(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sTransactionType:="MTA", v_dtMTAStartDate:=v_dtMTAStartDate, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bApplyRiskChange:=v_bApplyRiskChange), gPMConstants.PMEReturnCode)
        Else
            m_lReturn = CType(ProcessRisks(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sTransactionType:="MTA", v_dtMTAStartDate:=v_dtMTAStartDate, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, v_bApplyRiskChange:=v_bApplyRiskChange), gPMConstants.PMEReturnCode)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Update the policy status
        m_lReturn = CType(ProcessChangePolicyStatus(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_sTransactionType:=m_sTransactionType), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Policy level tax
        m_lReturn = CType(ProcessRITax(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lRiskCnt:=0), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Call the agent commission component
        m_lReturn = CType(ProcessAgentCommission(v_lInsuranceFileCnt:=r_lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_bIsReinstateRisk And m_lDeletedRiskInsuranceFileCnt = v_lBaseInsuranceFileCnt Then
            'In this case we are not replacing an existing MTA but creating a
            'new MTA to reinstate the risk
        Else
            If v_lPostChangeInsFileCnt > 0 Then
                'This is the original version of the MTA which has just been reapplied
                'so set the status to replaced
                m_lReturn = CType(UpdateInsuranceFileStatus(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sInsuranceFileStatusCode:="REP"), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateInsuranceFileStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                    Return result
                End If

                'Record a reason against this replaced record
                m_lReturn = CType(CreateEvent(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, v_sReason:=m_sEventReason), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRunMTAOnly")
                    Return result
                End If
            End If
        End If
        'Display the policy versions
        If Not (m_oListPolicies Is Nothing) Then

            m_lReturn = m_oListPolicies.GetPolicies()
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


        If Not Information.IsNothing(v_vNewLinkedInsuranceFileCnt) Then

            m_lReturn = m_oFindInsurance.UpdateMTAInsuranceFileLink(v_iType:=v_iType, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=v_vOriginalLinkedInsuranceFileCnt, v_vNewLinkedInsuranceFileCnt:=v_vNewLinkedInsuranceFileCnt, v_vProcessedInd:=v_vProcessedInd)
        Else

            m_lReturn = m_oFindInsurance.UpdateMTAInsuranceFileLink(v_iType:=v_iType, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, v_vOriginalLinkedInsuranceFileCnt:=v_vOriginalLinkedInsuranceFileCnt, v_vCancelledLinkedInsuranceFileCnt:=v_vCancelledLinkedInsuranceFileCnt, v_vProcessedInd:=v_vProcessedInd)
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
    Private Function ProcessRisks(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_dtMTAStartDate As Date = #12/30/1899#, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vDataArray(,) As Object
        Dim sFailureReason As String = ""
        Dim lNewRiskCnt As Integer
        Dim vSelectionArray(,) As Object
        Dim iSelected As Integer
        Dim lRiskId As Integer

        'Get a list of all the risks
        m_lReturn = CType(GetListOfRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vDataArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vDataArray) Then
            'Should always have a least one risk
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        ReDim vSelectionArray(1, vDataArray.GetUpperBound(1))


        For i As Integer = 0 To vDataArray.GetUpperBound(1)


            DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage & "/risk " & CStr(vDataArray(ACIRiskNo, i)))


            lRiskId = CInt(vDataArray(ACIRiskId, i))


            If CStr(vDataArray(ACIRiskStatusFlag, i)) = "U" Then
                m_lReturn = CType(ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
                    Return gPMConstants.PMEReturnCode.PMFalse
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisks")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

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
        m_lReturn = CType(GetListOfRisks(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vDataArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vDataArray) Then
            'Should always have a least one risk
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'Delete the original insurance_file_risk_entries for the copied
        'unchanged risk links

        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.DeleteInsuranceFileRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        v_sTransactionType = "MTACR"


        ReDim vSelectionArray(1, vDataArray.GetUpperBound(1))


        For i As Integer = 0 To vDataArray.GetUpperBound(1)


            DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage & "/risk " & CStr(vDataArray(ACIRiskNo, i)))


            lRiskId = CInt(vDataArray(ACIRiskId, i))

            'If vDataArray(ACIRiskStatusFlag, i) = "U" Then
            m_lReturn = CType(ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=DateTime.Now, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForNCDChange")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

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
    Private Function ProcessRisksWithMerge(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByVal v_dtMTAStartDate As Date, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vDataArray As Object
        Dim sFailureReason As String = ""
        Dim lNewRiskCnt As Integer
        Dim vSelectionArray(,) As Object
        Dim iSelected As Integer
        Dim lCurrentRiskId As Integer
        Dim sRiskMergeStatus As String = ""
        Dim lRiskCount, lFindRiskArrayIndex As Integer

        m_oAutoMTAMerge = New AutoMTAMerge()

        'Get a list of all the risks
        With m_oAutoMTAMerge
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .PreChangeInsFileCnt = v_lPreChangeInsFileCnt
            .PostChangeInsFileCnt = v_lPostChangeInsFileCnt
            .CurrentInsFileCnt = v_lInsuranceFileCnt


            'Modified by Sumeet Singh on 5/11/2010 6:55:29 PM refer developer guide no. 24
            '.set_FindRisk(m_oFindRisk)
            .FindRisk = m_oFindRisk
            m_lReturn = .GetListOfRisks()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTAMerge.GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
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


            DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage & "/risk " & CStr(vDataArray(ACIRiskNo, lFindRiskArrayIndex)))

            If m_oAutoMTAMerge.MergeStatus = gSIRLibrary.ACRStatusAddPostChange Then
                lCurrentRiskId = m_oAutoMTAMerge.PostChangeRiskCnt
            Else
                lCurrentRiskId = m_oAutoMTAMerge.CurrentRiskCnt
            End If
            'lPreChangeRiskCnt = m_oAutoMTAMerge.PreChangeRiskCnt
            'lPostChangeRiskCnt = m_oAutoMTAMerge.PostChangeRiskCnt
            'sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus

            m_lReturn = CType(ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lCurrentRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If this has quoted set status to selected

            vSelectionArray(1, i + 1) = lNewRiskCnt

            vSelectionArray(2, i + 1) = iSelected
        Next i

        'Update the risk selection status

        m_lReturn = m_oListRisks.UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectionArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksWithMerge")
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
        m_lReturn = CType(GetListOfRisks(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vDataArray), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Delete the original insurance_file_risk_entries for the copied
        'unchanged risk links

        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.DeleteInsuranceFileRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(vDataArray) Then
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


            DisplayStatusMessage(v_sStatusMessage:=m_sStatusMessage & "/risk " & CStr(vDataArray(ACIRiskNo, i)))


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
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oRiskData.AddRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
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

                    '            ElseIf vDataArray(ACIRiskStatusFlag, i) = "D" Then
                    '
                    '                m_lReturn = m_oRiskData.AddRiskLink( _
                    ''                    v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, _
                    ''                    v_lRiskCnt:=lRiskId, _
                    ''                    v_sStatusFlag:="D" _
                    ''                    )
                    '                If m_lReturn <> PMTrue Then
                    '                    ProcessRisksForRiskReinstatement = PMFalse
                    '                    LogMessage _
                    ''                        iType:=PMError, _
                    ''                        sMsg:="m_oRiskData.AddRiskLink Failed", _
                    ''                        vApp:=ACApp, _
                    ''                        vClass:=ACClass, _
                    ''                        vMethod:="ProcessRisksForRiskReinstatement"
                    '                    Exit Function
                    '                End If
                End If
            Else


                If CStr(vDataArray(ACIRiskStatusFlag, i)) = "C" Then
                    bProcess = True
                Else

                    m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskId, v_sStatusFlag:="U")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oRiskData.AddRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
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
                m_lReturn = CType(ProcessSingleRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=lRiskId, v_iRiskCnt:=i, r_lNewRiskCnt:=lNewRiskCnt, r_iSelected:=iSelected, v_sTransactionType:=sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=False, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSingleRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If bUpdateDeletedRiskCnt Then
                    'If this was the deleted risk or a new copy of it
                    m_lLastDeletedRiskCnt = lNewRiskCnt
                    m_lLastDeletedInsuranceFileCnt = v_lInsuranceFileCnt

                    ' RAW 26/09/2003 : CQ828 : moved from AutoRunMTAOnly
                    '                          added more details to event description
                    '                          replaced call to m_oFindInsurance.CreateEvent with my own


                    sDescription = "Reinstated Risk: " & _
                                   CStr(vDataArray(ACIRiskNo, i)) & " " & _
                                   CStr(vDataArray(ACIRiskTypeDescription, i)) & " " & _
                                   m_sReinstatementReason

                    m_lReturn = CType(CreateEvent(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sReason:=sDescription), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRisksForRiskReinstatement")
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
    Private Function ProcessSingleRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskId As Integer, ByVal v_iRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_iSelected As Integer, ByVal v_sTransactionType As String, ByVal v_dtMTAStartDate As Date, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sFailureReason, sRiskMergeStatus As String
        Dim vRiskDetailsArray(,) As Object
        Dim lRiskDetailsArrayIndex As Integer

        'sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus
        If Not (m_oAutoMTAMerge Is Nothing) And v_sTransactionType = "MTA" Then
            sRiskMergeStatus = m_oAutoMTAMerge.MergeStatus
        Else
            sRiskMergeStatus = ""
        End If

        'Create a copy of the risk data
        ' RAW 15/11/2003 : Pricing changes : added r_vRiskDetailsArray and r_lRiskDetailsArrayIndex param
        m_lReturn = CType(CopyRiskData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskCnt:=v_lRiskId, r_lNewRiskCnt:=r_lNewRiskCnt, r_sFailureReason:=sFailureReason, v_sTransactionType:=v_sTransactionType, v_sRiskMergeStatus:=sRiskMergeStatus, v_lOriginalFlag:=1, v_bDeleteOriginalRiskLink:=True, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, r_vRiskDetailsArray:=vRiskDetailsArray, r_lRiskDetailsArrayIndex:=lRiskDetailsArrayIndex), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'Now do the quote
        ' RAW 15/11/2003 : Pricing changes : added r_vRiskDetailsArray and v_lRiskDetailsArrayIndex param

        m_lReturn = CType(QuoteRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=r_lNewRiskCnt, v_iRiskNo:=v_iRiskCnt, v_sTransactionType:=v_sTransactionType, v_dtMTAStartDate:=v_dtMTAStartDate, v_bApplyRiskChange:=v_bApplyRiskChange, v_lBaseInsuranceFileCnt:=v_lBaseInsuranceFileCnt, v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt, v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt, r_vRiskDetailsArray:=vRiskDetailsArray, v_lRiskDetailsArrayIndex:=lRiskDetailsArrayIndex), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Do risk level reinsurance
        m_lReturn = CType(ProcessRiskReinsurance(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskId:=r_lNewRiskCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRiskReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Do risk level tax
        m_lReturn = CType(ProcessRITax(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=r_lNewRiskCnt), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_iSelected = 1

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetListOfRisks
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetListOfRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        m_lReturn = m_oFindRisk.SearchAll(r_vResultArray:=r_vResultArray, v_vInsuranceFileCnt:=v_lInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindRisk.SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListOfRisks")
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

        If Not Information.IsArray(m_vObjectPropertyArray) Then
            Return result
        End If

        For i As Integer = 0 To m_vObjectPropertyArray.GetUpperBound(1)

            sObjectName = CStr(m_vObjectPropertyArray(ACOPObjectName, i))
            sPropertyName = CStr(m_vObjectPropertyArray(ACOPPropertyName, i))
            vValue = CStr(m_vObjectPropertyArray(ACOPValue, i))


            m_lReturn = m_oDataset.GetAllOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.GetAllOIKey Failed for " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyRiskChanges")
                Return result
            End If
            If Information.IsArray(vOIKeyArray) Then


                m_lReturn = m_oDataset.SetPropertyValue(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, v_sOiKey:=CStr(vOIKeyArray(0)), v_vPropertyValue:=vValue, v_bisassumedInfo:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oDataset.SetPropertyValue Failed for " & sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyRiskChanges")
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeExistingMTAChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    Private Function QuoteRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_iRiskNo As Integer, ByVal v_sTransactionType As String, ByVal v_dtMTAStartDate As Date, ByRef r_vRiskDetailsArray(,) As Object, ByVal v_lRiskDetailsArrayIndex As Integer, Optional ByVal v_lBaseInsuranceFileCnt As Integer = 0, Optional ByVal v_bApplyRiskChange As Boolean = False, Optional ByVal v_lPreChangeInsFileCnt As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sGisDataModelCode As String = ""
        Dim lTransactionType As Integer
        Dim lQuoteType As Integer
        Dim sDeclineReasons, sReferReasons, sMessages, sXMLDataSetDef, sXMLDataSet As String
        Dim vRatingSectionArray As Object
        Dim cTotalAnnualTax As Decimal

        Dim vRatingMethodProductOption As Integer
        Dim lRiskTypeRuleSetId As Integer
        Dim dtRatingCoverStartDate, dtRatingQuoteDate As Date
        Dim vAdditionalDataArray As Object

        Const klRatingRules_UseIAG As Integer = 2

        Const klRiskDetails_RiskTypeRuleSetId As Integer = 38
        Const klRiskDetails_RatingCoverStartDate As Integer = 39
        Const klRiskDetails_RatingQuoteDate As Integer = 40

        Const ksKeyName_RiskId As String = "RISK_ID"
        Const ksKeyName_OverrideRiskTypeRuleSetId As String = "ORIGINAL_RULE"
        Const ksKeyName_OverrideRatingCoverStartDate As String = "RATING_START_DATE_OVERRIDE"
        Const ksKeyName_OverrideRatingQuoteDate As String = "RATING_QUOTE_DATE_OVERRIDE"


        ' RAW 15/11/2004 : Pricing Changes : added
        '    If GetRatingMethodOptions( _
        ''                                vRatingMethodProductOption, _
        ''                                vXYZSystemOption) <> PMTrue Then
        '        QuoteRisk = PMFalse
        '        LogMessage iType:=PMLogOnError, _
        ''            sMsg:="Failed to get rating method options", _
        ''            vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk"
        '        Exit Function
        '    End If
        ' RAW 15/11/2004 : Pricing Changes : end



        m_oPerilAllocation.TransactionType = v_sTransactionType


        m_oPerilAllocation.InsuranceFolderCnt = m_lInsuranceFolderCnt

        m_oPerilAllocation.InsuranceFileCnt = v_lInsuranceFileCnt

        m_oPerilAllocation.RiskId = v_lRiskCnt

        'Cancelling/reinstating or copying existing risks - no need to rate
        If (v_sTransactionType <> "MTC") And (v_sTransactionType <> "MTR") And (v_sTransactionType <> "MTCR") And (v_sTransactionType <> "MTADR") And (v_sTransactionType <> "MTACR") Then


            m_lReturn = m_oPerilAllocation.GetDataModel(sGISDataModel:=sGisDataModelCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.GetDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lTransactionType = m_oPerilAllocation.TransactionTypeId


            ' RAW 15/11/2003 : Pricing changes : added
            If Information.IsArray(r_vRiskDetailsArray) Then

                If vRatingMethodProductOption = klRatingRules_UseIAG Then


                    If Convert.IsDBNull(r_vRiskDetailsArray(klRiskDetails_RiskTypeRuleSetId, v_lRiskDetailsArrayIndex)) Or IsNothing(r_vRiskDetailsArray(klRiskDetails_RiskTypeRuleSetId, v_lRiskDetailsArrayIndex)) Then
                        lRiskTypeRuleSetId = 0
                    Else

                        lRiskTypeRuleSetId = CInt(r_vRiskDetailsArray(klRiskDetails_RiskTypeRuleSetId, v_lRiskDetailsArrayIndex))
                    End If

                    ' RAW 15/11/2004 : Pricing Changes : added

                    If Convert.IsDBNull(r_vRiskDetailsArray(klRiskDetails_RatingCoverStartDate, v_lRiskDetailsArrayIndex)) Or IsNothing(r_vRiskDetailsArray(klRiskDetails_RatingCoverStartDate, v_lRiskDetailsArrayIndex)) Then
                        dtRatingCoverStartDate = #12/30/1899#
                    Else

                        dtRatingCoverStartDate = CDate(r_vRiskDetailsArray(klRiskDetails_RatingCoverStartDate, v_lRiskDetailsArrayIndex))
                    End If

                    ' RAW 15/11/2004 : Pricing Changes : added

                    If Convert.IsDBNull(r_vRiskDetailsArray(klRiskDetails_RatingQuoteDate, v_lRiskDetailsArrayIndex)) Or IsNothing(r_vRiskDetailsArray(klRiskDetails_RatingQuoteDate, v_lRiskDetailsArrayIndex)) Then
                        dtRatingQuoteDate = #12/30/1899#
                    Else

                        dtRatingQuoteDate = CDate(r_vRiskDetailsArray(klRiskDetails_RatingQuoteDate, v_lRiskDetailsArrayIndex))
                    End If
                End If
            End If


            'Merge in the risk details from the original MTA
            If m_bMergeRisks And Not (m_oAutoMTAMerge Is Nothing) Then



                'Modified by Sumeet Singh on 5/11/2010 6:56:30 PM refer developer guide no. 24
                'm_oAutoMTAMerge.set_Gis(m_oGIS)
                m_oAutoMTAMerge.Gis = m_oGIS
                m_oAutoMTAMerge.DataModelCode = sGisDataModelCode
                m_oAutoMTAMerge.NewRiskCnt = v_lRiskCnt

                'Modified by Sumeet Singh on 5/11/2010 6:56:51 PM refer developer guide no. 24
                'm_oAutoMTAMerge.set_Dataset(m_oDataset)
                m_oAutoMTAMerge.Dataset = m_oDataset

                m_lReturn = CType(m_oAutoMTAMerge.MergeExistingMTAChanges(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oAutoMTAMerge.MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return result
                End If

                sXMLDataSetDef = m_oAutoMTAMerge.XMLDataSetDef
                sXMLDataSet = m_oAutoMTAMerge.XMLDataSet

            Else

                m_lReturn = m_oGIS.LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGisDataModelCode:=sGisDataModelCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lRiskId:=v_lRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.LoadFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If v_bApplyRiskChange Then
                ' Load Data as XML
                m_lReturn = m_oDataset.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Apply a fixed change to the risk
                m_lReturn = ApplyRiskChanges(v_lRiskId:=v_lRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyRiskChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Return as XmL String
                m_lReturn = m_oDataset.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            '        lQuoteType = (lTransactionType * 10000) + 1
            ' rating script (PBCQemQuoteTypeQuote)
            EncodeTransactionScreenAndType(lQuoteType, lTransactionType, 0, 1)


            ' RAW 28/04/2004 : CQ5143 : added
            ' clear existing quote entries from within the RISK node before requoting

            m_lReturn = m_oGIS.ClearPBQuoteOutputs(v_sGisDataModelCode:=sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oGis.ClearPBQuoteOutputs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 28/04/2004 : CQ5143 : end


            ' RAW 15/11/2003 : Pricing changes : added
            If Information.IsArray(r_vRiskDetailsArray) Then

                ' build additional data array
                ReDim vAdditionalDataArray(1, 3)

                vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = ksKeyName_RiskId

                vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lRiskCnt

                ' Note - renewals are not processed through this function so we only need to use the normal rating values
                If lRiskTypeRuleSetId <> 0 Then

                    vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = ksKeyName_OverrideRiskTypeRuleSetId

                    vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lRiskTypeRuleSetId
                End If
                If dtRatingCoverStartDate <> #12/30/1899# Then

                    vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = ksKeyName_OverrideRatingCoverStartDate

                    vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = dtRatingCoverStartDate
                End If
                If dtRatingQuoteDate <> #12/30/1899# Then

                    vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = ksKeyName_OverrideRatingQuoteDate

                    vAdditionalDataArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = dtRatingQuoteDate
                End If
            End If


            'Call NBQuote on bGIS.
            ' RAW 15/11/2004 : Pricing Changes : added r_vAdditionalDataArray param

            m_lReturn = m_oGIS.NBQuote(v_sGisDataModelCode:=sGisDataModelCode, v_lQuoteType:=lQuoteType, v_sGISBusinessTypeCode:="NB", v_dtEffectiveDate:=v_dtMTAStartDate, r_sXMLDataset:=sXMLDataSet, r_sXMLDataset:=sXMLDataSet, r_vAdditionalDataArray:=vAdditionalDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save it to the DataBase

            m_lReturn = m_oGIS.SaveToDB(v_sGisDataModelCode:=sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGis.SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'Populate sections and perils
        Select Case v_sTransactionType
            Case "MTCR", "MTR"
                ' RAW 05/05/2004 : CQ753 : added r_vResultArray param

                m_lReturn = m_oPerilAllocation.PopulateRatingSectionsFromExistingSections(v_lPreviousDeletedRiskInsuranceFileCnt:=v_lBaseInsuranceFileCnt, r_vResultArray:=vRatingSectionArray)
            Case "MTADR", "MTACR"
                ' RAW 05/05/2004 : CQ753 : added r_vResultArray param

                m_lReturn = m_oPerilAllocation.PopulateRatingSectionsFromExistingSections(v_lPreviousDeletedRiskInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vRatingSectionArray)
            Case Else
                ' RAW 05/05/2004 : CQ753 : added r_vResultArray param

                m_lReturn = m_oPerilAllocation.PopulateRatingSections(r_vResultArray:=vRatingSectionArray)
        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.PopulateRatingSections Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' RAW 05/05/2004 : CQ753 : added
        ' only call this to get tax

        m_lReturn = m_oPerilAllocation.RecalculatePremium(r_vRatingSection:=vRatingSectionArray, r_cTotalAnnualTax:=cTotalAnnualTax)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.RecalculatePremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oPerilAllocation.AnnualTaxTotal = cTotalAnnualTax
        ' RAW 05/05/2004 : CQ753 : end


        'Store any decline or referral messages
        m_lReturn = CType(StoreMessagesReferalsAndDeclines(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=v_lRiskCnt, v_sDeclineReasons:=sDeclineReasons, v_sReferReasons:=sReferReasons, v_sMessages:=sMessages), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="StoreMessagesReferalsAndDeclines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
            Return result
        End If

        'Update risk with values from sections an perils

        m_lReturn = m_oPerilAllocation.UpdateRisk
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPerilAllocation.UpdateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteRisk")
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
            If Not Information.IsArray(m_vDeclineReasons) Then
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
            If Not Information.IsArray(m_vReferReasons) Then
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
            If Not Information.IsArray(m_vMessages) Then
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
            If Not Information.IsArray(m_vPolicyStatusMessages) Then
                lUbound = 0
                ReDim m_vPolicyStatusMessages(ACDRArraySize, lUbound)
            Else
                lUbound = m_vPolicyStatusMessages.GetUpperBound(1) + 1
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeTransactionScreenAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' RAW 15/11/2003 : Pricing changes : added r_vRiskDetailsArray and r_lRiskDetailsArrayIndex param
    ' ***************************************************************** '
    Private Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String, ByVal v_sTransactionType As String, ByRef r_vRiskDetailsArray As Object, ByRef r_lRiskDetailsArrayIndex As Integer, Optional ByVal v_sRiskMergeStatus As String = "", Optional ByVal v_lOriginalFlag As Integer = 0, Optional ByVal v_lPostChangeInsFileCnt As Integer = 0, Optional ByVal v_bDeleteOriginalRiskLink As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean
        Dim lNewGisPolicyLinkID As Integer
        Dim vGisPolicyLinkArray As Object
        '    Dim vRiskArray As Variant               ' RAW 15/11/2003 : Pricing changes : replaced by r_vRiskDetailsArray
        '    Dim lCount As Long                      ' RAW 15/11/2003 : Pricing changes : replaced by r_lRiskDetailsArrayIndex
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lInsuranceFileCnt As Integer
        Dim bCreateDeletedRiskLink, bKeepOriginalRiskCnt As Boolean



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

        m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_vResultArray:=r_vRiskDetailsArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "Getting Risk"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check if we have any risks
        If Not Information.IsArray(r_vRiskDetailsArray) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No risks found", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Find the risk that matches the passed risk count, i.e. the one we want
        ' to copy
        bFound = False

        For r_vRiskDetailsArray = 0 To r_vRiskDetailsArray.GetUpperBound(1)

            If CDbl(r_vRiskDetailsArray(0, r_vRiskDetailsArray)) = v_lRiskCnt Then
                bFound = True
                Exit For
            End If
        Next

        ' Check if we have found the risk to copy
        If Not bFound Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot find risk to copy", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "Cannot find risk to copy"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If v_sRiskMergeStatus = gSIRLibrary.ACRStatusDeletedPostChange Then
            'This risk was deleted post change so create the insurance_file_risk_link
            'record with a status of deleted
            bCreateDeletedRiskLink = True
        Else
            bCreateDeletedRiskLink = False
        End If

        bKeepOriginalRiskCnt = Not (v_sRiskMergeStatus = gSIRLibrary.ACRStatusAddPostChange)
        ' Copy risk with same insurance file cnt

        m_lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_vRiskDetail:=r_vRiskDetailsArray, v_lPosNo:=r_vRiskDetailsArray, r_lRiskCnt:=r_lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lOriginalFlag:=v_lOriginalFlag, v_bDeleteOriginalRiskLink:=v_bDeleteOriginalRiskLink, v_bCreateDeletedRiskLink:=bCreateDeletedRiskLink, v_bKeepOriginalRiskCnt:=bKeepOriginalRiskCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "Copy Risk"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Prepare details to copy GIS Stuff attached to current risk

        ' Get policy link detail

        m_lReturn = m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskId:=r_vRiskDetailsArray(ACRiskPosCnt, r_vRiskDetailsArray), r_vResultArray:=vGisPolicyLinkArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.GetGISPolicyLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
            r_sFailureReason = "GetGISPolicyLink"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any data?
        Dim auxVar As Object = vGisPolicyLinkArray(0, 0)


        If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
            ' Make sure GIS object present.



            m_lReturn = m_oRenSelection.GIS_LoadFromDB(CStr(vGisPolicyLinkArray(4, 0)).Trim(), v_lInsuranceFolderCnt, vGisPolicyLinkArray(0, 0), r_vRiskDetailsArray(0, r_vRiskDetailsArray))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.GIS_LoadFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "LoadFromDB"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
            ' So we pass existing folder_cnt in for old and new file_cnt.
            'sj 12/02/2003 - start
            'PS104


            m_lReturn = m_oRenSelection.CopyDataSet(v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim(), r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), v_vOldInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vOldRiskID:=r_vRiskDetailsArray(0, r_vRiskDetailsArray), v_vNewInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vNewRiskID:=r_lNewRiskCnt, v_vNewInsuranceFileCnt:=v_lInsuranceFileCnt)
            '        m_lReturn = m_oRenSelection.CopyDataSet( _
            ''            v_sDataModelCode:=Trim$(vGisPolicyLinkArray(4, 0)), _
            ''            r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, _
            ''            r_sXMLDataSetDef:=sXMLDataSetDef, _
            ''            r_sXMLDataSet:=sXMLDataSet, _
            ''            v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), _
            ''            v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt, _
            ''            v_vOldRiskID:=r_vRiskDetailsArray(0, r_vRiskDetailsArray), _
            ''            v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt, _
            ''            v_vNewRiskID:=r_lNewRiskCnt)
            'sj 12/02/2003 - end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.CopyDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "CopyDataSet"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Initialise the Data Set with the Object/Properties

            m_lReturn = m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "LoadFromXML"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(28/02/2001)


            m_lReturn = m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenSelection.GIS_SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                r_sFailureReason = "SaveToDB"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: ProcessStats
    '
    ' Description:
    '
    ' History: 03/01/2003 sj - Created.
    ' RAW 13/11/2003 : CQ1765 : added v_lOriginalInsuranceFileCnt param
    ' ***************************************************************** '
    Private Function ProcessStats(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByVal v_lOriginalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        Dim cThisPremium As Decimal
        Dim sFailureReason As String = ""


        m_oControlTrans.InsuranceFileCnt = v_lInsuranceFileCnt

        m_oControlTrans.OriginalInsuranceFileCnt = v_lOriginalInsuranceFileCnt ' RAW 13/11/2003 : CQ1765 : added



        m_lReturn = m_oControlTrans.GetThisPremium(cThisPremium)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.GetThisPremium( Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStats")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If cThisPremium = 0 Then
            'Nothing to do
            Return result
        End If


        m_lReturn = m_oControlTrans.SetProcessModes(vTransactionType:=v_sTransactionType)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oControlTrans.SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStats")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oControlTrans.Start
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            sFailureReason = m_oControlTrans.message
            sFailureReason = "Statistics process failed :" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sFailureReason
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailureReason, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStats")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

    End Function

    'RESILIENCE - This function is not used either internally or Externally
    'Public Function CreateBusinessObjectsLocal(ByVal v_oDatabase As Object) As Long
    '
    '    On Error GoTo Err_CreateBusinessObjectsLocal
    '
    '    CreateBusinessObjectsLocal = PMTrue
    '
    '    Dim sClassName As String
    '
    '    Set m_oDatabase = v_oDatabase
    '
    '    'Reinsurance
    '    sClassName = "bSIRReinsurance.Form"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oReinsurance, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'RI Tax
    '    sClassName = "bSIRRITax.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oRITax, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Agent Commission
    '    sClassName = "bSirAgentCommission.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oAgentCommission, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Find Insurance
    '    sClassName = "bSIRFindInsurance.Form"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oFindInsurance, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Change Policy Status
    '    sClassName = "bSIRChangePolicyStatus.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oChangePolicyStatus, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Find Risk
    '    sClassName = "bSIRFindRisk.Form"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oFindRisk, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'List Risks
    '    sClassName = "bSIRListRisks.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oListRisks, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Risk Data
    '    sClassName = "bSirRiskData.business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oRiskData, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Renewal Selection
    '    sClassName = "bSirRenSelection.business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oRenSelection, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Peril Allocation
    '    sClassName = "bSirPerilAllocation.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oPerilAllocation, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Control Trans
    '    sClassName = "bControlTrans.Automated"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oControlTrans, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Automatic Renewal Selection
    '    sClassName = "bSIRAutomaticRenewalsSel.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oAutomaticRenewalsSel, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    m_oAutomaticRenewalsSel.CalledFromAutoMTA = True                ' RAW 28/04/2004 : CQ5143 : added
    '
    '
    '    'Automatic Renewals Accept
    '    sClassName = "bSIRAutomaticRenewalsAccept.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oAutomaticRenewalsAccept, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Gis
    '    sClassName = "bGis.Application"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oGIS, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Insurance File
    '    sClassName = "bSIRInsuranceFile.Business"
    '    m_lReturn = CreateBusinessObject(r_oObject:=m_oInsuranceFile, _
    ''        v_sClassName:=sClassName)
    '    If m_lReturn <> PMTrue Then
    '        GoTo Err_CreateBusinessObjectsLocal
    '    End If
    '
    '    'Dataset
    '    Set m_oDataset = New cGISDataSetControl.Application
    '
    '    Exit Function
    '
    'Err_CreateBusinessObjectsLocal:
    '
    '    CreateBusinessObjectsLocal = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to create " & sClassName, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CreateBusinessObjectsLocal", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObjectsServer
    '
    ' Description:
    '
    ' History: 07/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function CreateBusinessObjectsServer() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Control Trans
            Dim temp_m_oControlTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oControlTrans, "bControlTrans.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oControlTrans = temp_m_oControlTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create " & "instance of bControlTrans.Automated", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' =============================================================
            ' All other objects are created from m_oControlTrans so that they share
            ' the same DB object and can participate in the same DB transaction
            ' =============================================================
            ' RAW 02/03/2004 : CQ3665 : replaced all other instance of GetInstance with
            ' m_oControlTrans.CreateBusinessObject

            'Reinsurance

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create " & "instance of bSIRReinsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RI Tax

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oRITax, v_sClassName:="bSIRRITax.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create " & "instance of bSIRRITax.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Agent Commission

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oAgentCommission, v_sClassName:="bSirAgentCommission.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSirAgentCommission.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Find Insurance

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oFindInsurance, v_sClassName:="bSIRFindInsurance.Form")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRFindInsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Change Policy Status

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oChangePolicyStatus, v_sClassName:="bSIRChangePolicyStatus.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRChangePolicyStatus.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Find Risk

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oFindRisk, v_sClassName:="bSIRFindRisk.Form")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRFindRisk.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'List Risks

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oListRisks, v_sClassName:="bSIRListRisks.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRListRisks.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Risk Data

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oRiskData, v_sClassName:="bSirRiskData.business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSirRiskData.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Renewal Selection

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oRenSelection, v_sClassName:="bSirRenSelection.business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSirRenSelection.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Peril Allocation

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oPerilAllocation, v_sClassName:="bSirPerilAllocation.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSirPerilAllocation.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Automatic Renewals Selection

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oAutomaticRenewalsSel, v_sClassName:="bSIRAutomaticRenewalsSel.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRAutomaticRenewalsSel.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oAutomaticRenewalsSel.CalledFromAutoMTA = True ' RAW 05/05/2004 : CQ753 : added



            'Automatic Renewals Accept

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oAutomaticRenewalsAccept, v_sClassName:="bSIRAutomaticRenewalsAccept.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRAutomaticRenewalsAccept.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Gis

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oGIS, v_sClassName:="bGis.Application")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bGis.Application", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Insurance File

            m_lReturn = m_oControlTrans.CreateBusinessObject(r_oObject:=m_oInsuranceFile, v_sClassName:="bSIRInsuranceFile.Business")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRInsuranceFile.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataset = New cGISDataSetControl.Application()

            g_iUserID = g_oObjectManager.UserID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBusinessObjectsServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjectsServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessRiskReinsurance
    '
    ' Description:
    '
    ' History: 07/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessRiskReinsurance(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bApplyReinsurance As Boolean


            m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            With m_oReinsurance

                .InsuranceFileCnt = v_lInsuranceFileCnt

                .RiskId = v_lRiskId
            End With


            m_lReturn = m_oReinsurance.ApplyReinsurance(bApplyReinsurance)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ApplyReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bApplyReinsurance Then

                If v_lRiskId <> 0 Then
                    'Set the status to quoted

                    m_lReturn = m_oReinsurance.ChangeRiskStatus

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ChangeRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                Return result
            End If

            'Generate reinsurance details for risk

            m_lReturn = m_oReinsurance.CalculateRI
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ApplyReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the risk status to quoted

            m_lReturn = m_oReinsurance.ChangeRiskStatus
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oReinsurance.ChangeRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRiskReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function ProcessRITax(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bApplyTaxes As Boolean
            Dim vRITax As Object
            Dim sDesc As String = ""

            'Do we need to apply taxes?

            m_lReturn = m_oRITax.ApplyTaxes(v_lInsuranceFileCnt, v_lRiskCnt, bApplyTaxes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.ApplyTaxes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                Return result
            End If

            If Not bApplyTaxes Then
                'Nothing to do
                Return result
            End If


            m_lReturn = m_oRITax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            With m_oRITax

                .InsuranceFileCnt = v_lInsuranceFileCnt

                .Riskcnt = v_lRiskCnt
            End With

            If v_lRiskCnt > 0 Then

                m_oRITax.Riskcnt = v_lRiskCnt

                m_lReturn = m_oRITax.GetRiskTax(r_vRiskTax:=vRITax, r_sDescription:=sDesc, iTask:=gPMConstants.PMEComponentAction.PMEdit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.GetRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                    Return result
                End If
            Else

                m_oRITax.InsuranceFileCnt = v_lInsuranceFileCnt

                m_lReturn = m_oRITax.GetInsuranceFileTax(r_vInsuranceFileTax:=vRITax, r_sDescription:=sDesc, iTask:=gPMConstants.PMEComponentAction.PMEdit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRITax.GetInsuranceFileTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax")
                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessRITax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessRITax", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAgentCommission.CheckDisplayCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bCommissionRequired Then
                'No processing required
                Return result
            End If

            'Calculate agent commission

            m_lReturn = m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=m_sTransactionType, r_vntResult:=vAgentCommission)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAgentCommission.CalculateAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 19/09/2003 : CQ2614 : added
            'Calculate lead commission

            m_lReturn = m_oAgentCommission.UpdateLeadCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oAgentCommission.UpdateLeadCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 19/09/2003 : CQ2614 : end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAgentCommission", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ProcessChangePolicyStatus
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessChangePolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef v_sTransactionType As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vRisks(,) As Object
        Dim sMessage As String = ""
        Dim bSelectedRisks As Boolean
        Dim lLevel As Integer


        m_oChangePolicyStatus.TransactionType = v_sTransactionType


        m_lReturn = m_oChangePolicyStatus.GetRisksByStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vRisks:=vRisks)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '1 = Referred
        '2 = Declined
        '3 = Quoted
        '4 = Unquoted
        '5 = Purchase question to be answered
        '6 = Post quote questions to be answered
        '7 = Pre quote questions to be answered
        '8 = Pending Reinsurance

        'This had better have something in it...
        If Not Information.IsArray(vRisks) Then
            sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                       "There are no risks on this policy"
        Else
            sMessage = ""
            lLevel = 0

            For lTemp As Integer = vRisks.GetLowerBound(1) To vRisks.GetUpperBound(1)
                ' PW311002 - if selected do checks, else don't bother

                If CDbl(vRisks(1, lTemp)) = 1 Then
                    bSelectedRisks = True
                    Select Case vRisks(0, lTemp)
                        Case 1, 2, 4
                            If lLevel < 3 Then
                                lLevel = 3
                                sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "At least one risk on this policy is unquoted"
                            End If
                        Case 8
                            If lLevel < 2 Then
                                lLevel = 2
                                sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "At least one risk on this policy has no reinsurance"
                            End If
                        Case 5, 6, 7
                            If lLevel < 1 Then
                                lLevel = 1
                                sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                                           "At least one risk on this policy has questions to be answered"
                            End If
                    End Select
                End If
            Next lTemp
        End If

        ' PW311002 - Check if any of the risks are flagged as "selected"
        If Not bSelectedRisks Then
            sMessage = "Cannot proceed -" & Strings.Chr(13) & Strings.Chr(10) & _
                       "At least one risk on this policy must be selected to make it live"
        End If


        If sMessage <> "" Then
            'Store any decline or referral messages
            m_lReturn = CType(StoreMessagesReferalsAndDeclines(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=0, v_sPolicyStatusMessages:=sMessage), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="StoreMessagesReferalsAndDeclines Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
                Return result
            End If
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' PW151102 - If going live, delete any unselected risks' link
        '            records

        m_lReturn = m_oChangePolicyStatus.DeleteRisks(v_vRisks:=vRisks)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.DeleteRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' PW311002 - Re-jig the risk and variation numbers of the remaining
        '            risks on this policy


        m_lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=CInt(vRisks(2, 0)))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.RenumberRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        vRisks = Nothing


        m_oChangePolicyStatus.Mode = 0


        m_lReturn = m_oChangePolicyStatus.ChangePolicyStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.ChangePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oChangePolicyStatus.UpdatePolicyPremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessChangePolicyStatus")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

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
    Public Function GetVersionByDate(ByRef r_lInsuranceFileCnt As Integer, ByVal v_dtStartDate As Date, ByRef r_lPolicyVersion As Integer, ByRef r_lErrorCode As Integer, Optional ByRef r_bBackdatingRequired As Boolean = False, Optional ByVal v_bIsReinstatement As Boolean = False, Optional ByVal v_bIsCancellation As Boolean = False, Optional ByVal v_lDeletedRiskInsuranceFileCnt As Integer = 0, Optional ByVal v_bIsPermanentMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 13/11/2003 : CQ1765 : added v_bIsPermanentMTA param

            m_lReturn = m_oFindInsurance.GetVersionByDate(r_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_dtStartDate:=v_dtStartDate, r_lPolicyVersion:=r_lPolicyVersion, r_lErrorCode:=r_lErrorCode, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_bBackdatingRequired:=r_bBackdatingRequired, r_vAffectedInsuranceFileCnts:=m_vAffectedInsuranceFileCnts, v_bIsReinstatement:=v_bIsReinstatement, v_bIsCancellation:=v_bIsCancellation, v_lDeletedRiskInsuranceFileCnt:=v_lDeletedRiskInsuranceFileCnt, v_bIsPermanentMTA:=v_bIsPermanentMTA)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVersionByDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
    Public Function CopyPolicy(ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByVal v_lVersion As Integer, ByVal v_bPermanentMTA As Boolean, ByVal v_dtMTADate As Date, Optional ByVal v_vMTAEndDate As Object = Nothing, Optional ByVal v_bCopyDeletedLink As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 20/01/2004 : CQ3535 : added v_vPolicyCancelledOnDate param

            m_lReturn = m_oFindInsurance.CopyPolicy(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lVersion:=v_lVersion, v_bPermanentMTA:=v_bPermanentMTA, v_dtMTADate:=v_dtMTADate, v_vMTAEndDate:=v_vMTAEndDate, v_bCopyDeletedLink:=v_bCopyDeletedLink, v_vPolicyCancelledOnDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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


            m_lReturn = m_oFindInsurance.SetNillPremiumRefund(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.SetNillPremiumRefund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' RAW 24/11/2003 : CQ685 : added

            If Not Information.IsNothing(v_vAffectedInsuranceFileCnts) And Information.IsArray(v_vAffectedInsuranceFileCnts) Then

                For i As Integer = v_vAffectedInsuranceFileCnts.GetLowerBound(0) To v_vAffectedInsuranceFileCnts.GetUpperBound(0)


                    If CDbl(v_vAffectedInsuranceFileCnts(i)) <> m_lNewInsuranceFileCnt Then


                        m_lReturn = m_oFindInsurance.SetNillPremiumRefund(v_lInsuranceFileCnt:=v_vAffectedInsuranceFileCnts(i))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.SetNillPremiumRefund Failed for other InsuranceFile (" & i & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund")
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetNillPremiumRefund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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
    '
    ' ***************************************************************** '
    Private Function ReverseArray(ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lCols, lRows As Integer
        Dim vArray(,) As Object
        Dim lNewRowCnt As Integer

        lCols = r_vArray.GetUpperBound(0)
        lRows = r_vArray.GetUpperBound(1)


        vArray = VB6.CopyArray(r_vArray)

        ReDim r_vArray(lCols, lRows)

        For lRowCnt As Integer = lRows To 0 Step -1
            For lColCnt As Integer = 0 To lCols


                r_vArray(lColCnt, lNewRowCnt) = vArray(lColCnt, lRowCnt)
            Next lColCnt

            lNewRowCnt += 1
        Next lRowCnt

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: DisplayStatusMessage
    '
    ' Description:
    '
    ' History: 17/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusMessage(ByVal v_sStatusMessage As String)



        If Not (m_oListPolicies Is Nothing) Then

            m_oListPolicies.statustext = v_sStatusMessage
        End If
        If Not (m_oStatusBar Is Nothing) Then

            m_oStatusBar.SimpleText = v_sStatusMessage
        End If


    End Sub

    Public Function IsBackdatedMTARequired() As Boolean

        Dim result As Boolean = False
        Try


            Dim lPolicyVersion, lInsuranceFileCnt, lErrorCode As Integer
            Dim bBackdatingRequired As Boolean
            Dim sInsuranceFileTypeCode As String = ""


            m_lReturn = m_oFindInsurance.GetInsuranceFileType(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt, r_sInsuranceFileTypeCode:=sInsuranceFileTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oFindInsurance.GetInsuranceFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sInsuranceFileTypeCode <> "MTAQTETEMP" Then
                lErrorCode = 1
            Else
                lErrorCode = 0
            End If

            'Get a list of all the policy versions which need to be processed and store
            'them in the array - m_vAffectedInsuranceFileCnts
            m_lReturn = CType(GetVersionByDate(r_lInsuranceFileCnt:=lInsuranceFileCnt, v_dtStartDate:=m_dtEffectiveDate, r_lPolicyVersion:=lPolicyVersion, r_lErrorCode:=lErrorCode, r_bBackdatingRequired:=bBackdatingRequired), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetVersionByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(m_vAffectedInsuranceFileCnts) Then
                If m_vAffectedInsuranceFileCnts.GetUpperBound(1) > 0 Then
                    'More than one version affected
                    result = True
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsBackDatedMTARequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackDatedMTARequired", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

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

        '    ' Get any previous reasons for this MTA
        '    m_lReturn = m_oInsuranceFile.GetPreviousMTAReasons( _
        ''        v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
        ''        r_vResultArray:=vResultArray, _
        ''        r_lEventTypeID:=lMTAReasonsEventTypeID)
        '    If m_lReturn <> PMTrue Then
        '        CreateEvent = PMFalse
        '        LogMessage _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="m_oInsuranceFile.GetPreviousMTAReasons Failed", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="CreateEvent"
        '        Exit Function
        '    End If

        '    If IsArray(vResultArray) = True Then
        '        bPreviousReasonsExist = True
        '        'Append the new reason to the old ones
        '        v_sReason = Trim(vResultArray(0, 0)) & v_sReason
        '    End If
        '
        ' RAW 26/09/2003 : CQ828 : removed "||" suffix

        m_oInsuranceFile.EventDescription = v_sReason

        '    If bPreviousReasonsExist Then
        '        'Delete the previous reasons
        '        m_lReturn = m_oInsuranceFile.DeleteMTAEvent( _
        ''            v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
        ''            v_lEventTypeID:=lMTAReasonsEventTypeID)
        '        If m_lReturn <> PMTrue Then
        '            CreateEvent = PMFalse
        '            LogMessage _
        ''                iType:=PMLogOnError, _
        ''                sMsg:="m_oInsuranceFile.DeleteMTAEvent Failed", _
        ''                vApp:=ACApp, _
        ''                vClass:=ACClass, _
        ''                vMethod:="CreateEvent"
        '            Exit Function
        '        End If
        '
        '    End If


        m_lReturn = m_oInsuranceFile.MakeEvent

        '    m_lReturn = m_oInsuranceFile.MakeEvent( _
        ''        v_lEventTypeID:=lMTAReasonsEventTypeID)
        '    If m_lReturn <> PMTrue Then
        '        CreateEvent = PMFalse
        '        LogMessage _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="m_oInsuranceFile.MakeEvent Failed", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="CreateEvent"
        '        Exit Function
        '    End If

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

            r_oObject = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).FullName, v_sClassName).Unwrap()


            lReturnCode = r_oObject.Initialise(sUsername:=g_sUsername.Value, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=g_iLogLevel, sCallingAppName:=g_sCallingAppName, vDatabase:=m_oDatabase)

            ' Check for errors.
            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Set the object to nothing

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_sUsername.Value, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object instance (" & v_sClassName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", excep:=excep)

            Return result

        End Try
    End Function

    Protected Overrides Sub Finalize()

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

        If Not (m_oAutomaticRenewalsSel Is Nothing) Then

            m_oAutomaticRenewalsSel.Dispose()
            m_oAutomaticRenewalsSel = Nothing
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

        If Not (m_oDataset Is Nothing) Then
            m_oDataset.Dispose()
            m_oDataset = Nothing
        End If

        m_oListPolicies = Nothing
        m_oStatusBar = Nothing
        m_oAutoMTAMerge = Nothing
        m_oDatabase = Nothing

    End Sub
End Class
