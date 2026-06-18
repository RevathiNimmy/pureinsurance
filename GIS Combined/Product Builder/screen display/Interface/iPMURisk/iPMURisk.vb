Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    '
    ' History :
    ' CJB 08/03/2005 PN19313 For new renewal what-if quotes only, if the quote has not been saved and the
    '                user is exiting, delete the quote and the policy version. This is done in iPMUScreenControl and
    '                uctRiskScreenControl but new keyarray values in here were required - RenewalConfirmationMode and
    '                IsWhatIfQ.
    ' CJB 20/10/2006 PN24176 Changed to have Property Proc for KeyArray and in Form_Load pass value onto uctRiskScreen.
    '                This is all required to eventually pass into Dynamic Logic - necessary to determine which risk
    '                code has been selected (so that Stargate Risk Screens can be viewed in Back Office) and other
    '                values may be useful too.


    Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer

    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0


    ' Process mode variables
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Private variables
    Private m_lChildIndex As Integer 'index number of child record beinf added (used as count var in XML)
    Private m_vRiskTypeDetails As Object 'read by top level screen and passed into child screens
    Private m_lPartyCnt As Integer
    Private m_lPerilTypeId As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lRiskId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lProductId As Integer
    Private m_lScreenId As Integer
    Private m_sScreenDesc As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lReturn As Integer
    Private m_lErrorNumber As Integer
    Private m_bSubScreen As Boolean
    Private m_sParentOIKey As String = ""
    Private m_sChildOIKey As String = ""
    Private m_sParentObjectName As String = ""
    Private m_sChildObjectName As String = ""
    Private m_sGISObjectName As String = ""
    'Developer Guide No.101
    Private m_oGIS As Object
    Private m_vScreenValues As Object
    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH BEGIN
    '                   Add new variables for claim stuff
    '-------------------------------------------------------------------------------------
    Private m_lClaimID As Integer
    Private m_lClaimPerilID As Integer
    Private m_lPerilID As Integer
    Private m_lClaimWorkID As Integer
    Private m_lClaimWorkPerilID As Integer
    Private m_sClaimTransactionType As String = ""
    Private m_lClaimInsFileCnt As Integer
    Private m_lClaimRiskId As Integer
    Private m_bLossSchedule As Boolean
    Private m_lLossScheduleTypeId As Integer
    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH END
    '-------------------------------------------------------------------------------------

    Private m_iIsRiAtRiskLevel As Integer
    Private m_iIsAutoReinsured As Integer

    Private m_sReferReasons As String = ""
    Private m_sDeclineReasons As String = ""
    Private m_sMessages As String = ""
    Private m_sQuoteType As String = ""

    ' SourceID
    Private m_iSourceID As Integer

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021021 - NRMA Changes - Sirius Process No 126 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private m_oPartySummary As iSIRPartySummary.Interface_Renamed
    Private m_oPolicySummary As iSIRPolicySummary.Interface_Renamed
    Private m_sInsuranceRef As String = ""
    Private m_lClaimMode As Integer

    Private m_sScreenCaption As String = ""

    '1.8.6 branch
    Private m_lObjectType As Integer
    Private m_oGISOriginal As Object

    'DP 30/05/2003 - added for Stargate 1.2
    Private m_cList As Collection
    Private m_aArray() As Object
    'DP end

    Private m_bClientPolicyDetailsLoaded As Boolean
    Private m_sClaimNo As String = ""
    Private m_lEventClaimID As Integer

    'DP 30/05/2003 - added for passing data from parent to child
    Private m_aChildDataFromParent As Object
    'DP end

    Private m_vXMLDataSet As Object

    Private m_lOriginalClaimId As Integer

    Private m_bCopyRisk As Boolean
    Private m_lGISPolicyLinkID As Integer

    Private m_bRenewalConfirmationMode As Boolean 'PN19313
    Private m_bIsWhatIfQ As Boolean 'PN19313

    Private m_vKeyArray(,) As Object 'PN24176
    Private m_bNoTransactions As Boolean

    'Plico 24-28
    Private m_lCaseID As Integer
    Private m_lBaseCaseID As Integer
    Private m_sCaseNumber As String = ""
    Private m_sCallingAppName As String = ""
    Private m_bIsSilentQuote As Boolean = False
    Private m_bReserveLimitExceeded As Boolean
    Private m_dExceededReserve As Decimal

    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click
        ' Variables declared to retrieve the details via business object
        Dim lInsuranceFolderCnt As Integer
        Dim sInsuranceRef, sPartyShortName As String
        Dim dtDueDate As Date
        Dim sClaimNo As String = ""
        Dim lPartyCnt As Integer

        ' Variables declared to be set to Task Manager Screen
        Dim sDescription As String = ""

        If m_sTransactionType <> "C_NC" And m_sTransactionType <> "C_EC" Then
            m_lReturn = AddTaskGetDetails(lPartyCnt, lInsuranceFolderCnt, sInsuranceRef, sPartyShortName, dtDueDate, sClaimNo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        End If

        If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR" Then
            sDescription = sClaimNo
        ElseIf m_sTransactionType = "NB" Or m_sTransactionType = "MTA" Or m_sTransactionType = "MTC" Then
            sDescription = sInsuranceRef
        ElseIf m_sTransactionType = "C_NC" Then
            sDescription = "New Case"
        ElseIf m_sTransactionType = "C_EC" Then
            sDescription = "Edit Case [ " & m_sCaseNumber & " ]"
        Else
            sDescription = ""
        End If

        m_lReturn = CreateWorkManagerTask(sPartyShortName:=sPartyShortName, dDueDate:=dtDueDate, sDescription:=sDescription)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If
        MyBase.Focus()
    End Sub

    Private Sub cmdCheckList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCheckList.Click
        m_lReturn = ShowCheckList()
    End Sub

    Private Sub uctRiskScreen1_ClaimPaymentUnrecoverableError(ByVal Sender As Object, ByVal e As EventArgs) Handles uctRiskScreen1.ClaimPaymentUnrecoverableError
        cmdOK.Enabled = False
    End Sub
    Private Sub uctRiskScreen1_DebugModeChanged(ByVal bDebugEnabled As Boolean)

        If bDebugEnabled Then
            Me.Text = m_sScreenCaption & " (DEBUG MODE ON)"
        Else
            Me.Text = m_sScreenCaption
        End If
    End Sub

    Private Sub uctRiskScreen1_PerilEdit(ByVal Sender As Object, ByVal e As EventArgs) Handles uctRiskScreen1.PerilEdit
        ' destroy any left over reference to
        ' the policy summary before we move
        ' to the peril screen....
        If Not (m_oPolicySummary Is Nothing) Then
            m_oPolicySummary.Dispose()
            m_oPolicySummary = Nothing
        End If
    End Sub

    Public Property ReserveLimitExceeded() As Boolean
        Get
            Return m_bReserveLimitExceeded
        End Get
        Set(ByVal Value As Boolean)
            m_bReserveLimitExceeded = Value
        End Set
    End Property
    Public Property ExceededReserve() As Decimal
        Get
            Return m_dExceededReserve
        End Get
        Set(ByVal Value As Decimal)
            m_dExceededReserve = Value
        End Set
    End Property

    Public ReadOnly Property XMLDataSet() As Object
        Get
            Return m_vXMLDataSet
        End Get
    End Property

    Public WriteOnly Property NoTransactions() As Boolean
        Set(ByVal Value As Boolean)
            m_bNoTransactions = Value
        End Set
    End Property

    'DP 30/05/2003 - for Stargate v1.2
    Public WriteOnly Property List() As Collection
        Set(ByVal Value As Collection)
            m_cList = Value
        End Set
    End Property

    Public WriteOnly Property vArray() As Object()
        Set(ByVal Value As Object())
            m_aArray = Value
        End Set
    End Property
    'DP end

    'DP 30/05/2003 - added for passing data from parent to child
    Public WriteOnly Property ChildDataFromParent() As Object
        Set(ByVal Value As Object)


            m_aChildDataFromParent = Value
        End Set
    End Property
    'DP end

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021021 - NRMA Changes - Sirius Process No 126 - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public WriteOnly Property GISOriginal() As Object
        Set(ByVal Value As Object)
            ' AMB 10/01/03 - Start - IAG 217 Spec
            m_oGISOriginal = Value
            ' AMB 10/01/03 - End - IAG 217 Spec
        End Set
    End Property
    Public WriteOnly Property ChildIndex() As Integer
        Set(ByVal Value As Integer)
            m_lChildIndex = Value
        End Set
    End Property
    Public WriteOnly Property RiskTypeDetails() As Object
        Set(ByVal Value As Object)


            m_vRiskTypeDetails = Value
        End Set
    End Property

    Public Property ObjectType() As Integer
        Get
            Return m_lObjectType
        End Get
        Set(ByVal Value As Integer)
            m_lObjectType = Value
        End Set
    End Property

    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public Property KeyArray() As Object
        Get 'PN24176
            Return VB6.CopyArray(m_vKeyArray)
        End Get
        Set(ByVal Value As Object) 'PN24176
            m_vKeyArray = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    Public Property SubScreen() As Boolean
        Get
            Return m_bSubScreen
        End Get
        Set(ByVal Value As Boolean)
            m_bSubScreen = Value
        End Set
    End Property

    Public Property ParentOIKey() As String
        Get
            Return m_sParentOIKey
        End Get
        Set(ByVal Value As String)
            m_sParentOIKey = Value
        End Set
    End Property

    Public Property ChildOIKey() As String
        Get
            Return m_sChildOIKey
        End Get
        Set(ByVal Value As String)
            m_sChildOIKey = Value
        End Set
    End Property

    Public Property ParentObjectName() As String
        Get
            Return m_sParentObjectName
        End Get
        Set(ByVal Value As String)
            m_sParentObjectName = Value
        End Set
    End Property

    Public Property LossSchedule() As Boolean
        Get
            Return m_bLossSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bLossSchedule = Value
        End Set
    End Property
    Public Property LossScheduleTypeId() As Integer
        Get
            Return m_lLossScheduleTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lLossScheduleTypeId = Value
        End Set
    End Property
    Public Property PerilTypeId() As Integer
        Get
            Return m_lPerilTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeId = Value
        End Set
    End Property
    Public Property ChildObjectName() As String
        Get
            Return m_sChildObjectName
        End Get
        Set(ByVal Value As String)
            m_sChildObjectName = Value
        End Set
    End Property

    Public Property GISObjectName() As String
        Get
            Return m_sGISObjectName
        End Get
        Set(ByVal Value As String)
            m_sGISObjectName = Value
        End Set
    End Property
    'Developer Guide No.101
    Public Property GIS() As Object
        Get
            Return m_oGIS
        End Get
        Set(ByVal Value As Object)
            m_oGIS = Value
        End Set
    End Property

    Public Property ScreenValues() As Object
        Get
            Return m_vScreenValues
        End Get
        Set(ByVal Value As Object)


            m_vScreenValues = Value
        End Set
    End Property

    Public Property IsRiAtRiskLevel() As Integer
        Get
            Return m_iIsRiAtRiskLevel
        End Get
        Set(ByVal Value As Integer)
            m_iIsRiAtRiskLevel = Value
        End Set
    End Property

    Public Property IsAutoReinsured() As Integer
        Get
            Return m_iIsAutoReinsured
        End Get
        Set(ByVal Value As Integer)
            m_iIsAutoReinsured = Value
        End Set
    End Property

    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH BEGIN
    '                   Add new property get/let for Claims stuff
    '-------------------------------------------------------------------------------------

    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property


    Public Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property


    Public Property ClaimPerilID() As Integer
        Get
            Return m_lClaimPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimPerilID = Value
        End Set
    End Property


    Public Property WorkClaimID() As Integer
        Get
            Return m_lClaimWorkID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimWorkID = Value
        End Set
    End Property


    Public Property WorkClaimPerilID() As Integer
        Get
            Return m_lClaimWorkPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimWorkPerilID = Value
        End Set
    End Property


    Public Property ClaimTransactionType() As String
        Get
            Return m_sClaimTransactionType
        End Get
        Set(ByVal Value As String)
            m_sClaimTransactionType = Value
        End Set
    End Property


    Public Property ClaimInsFileCnt() As Integer
        Get
            Return m_lClaimInsFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimInsFileCnt = Value
        End Set
    End Property


    Public Property ClaimRiskId() As Integer
        Get
            Return m_lClaimRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lClaimRiskId = Value
        End Set
    End Property
    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH END
    '-------------------------------------------------------------------------------------

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021021 - NRMA Changes - Sirius Process No 126 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public WriteOnly Property ClaimMode() As Integer
        Set(ByVal Value As Integer)
            m_lClaimMode = Value
        End Set
    End Property
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021021 - NRMA Changes - Sirius Process No 126 - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ' RVH 24/12/2004 - pass caption in
    Public WriteOnly Property ScreenCaption() As String
        Set(ByVal Value As String)
            m_sScreenCaption = Value
        End Set
    End Property

    Public WriteOnly Property CopyRisk() As Boolean
        Set(ByVal Value As Boolean)
            m_bCopyRisk = Value
        End Set
    End Property

    Public WriteOnly Property GISPolicyLinkID() As Integer
        Set(ByVal Value As Integer)
            m_lGISPolicyLinkID = Value
        End Set
    End Property

    Public WriteOnly Property RenewalConfirmationMode() As Boolean
        Set(ByVal Value As Boolean) 'PN19313
            m_bRenewalConfirmationMode = Value
        End Set
    End Property
    Public WriteOnly Property IsWhatIfQ() As Boolean
        Set(ByVal Value As Boolean) 'PN19313
            m_bIsWhatIfQ = Value
        End Set
    End Property


    Public Property CaseID() As Integer
        Get
            Return m_lCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property

    Public Property BaseCaseID() As Integer
        Get
            Return m_lBaseCaseID
        End Get
        Set(ByVal Value As Integer)
            m_lBaseCaseID = Value
        End Set
    End Property

    Public Property CaseNumber() As String
        Get
            Return m_sCaseNumber
        End Get
        Set(ByVal Value As String)
            m_sCaseNumber = Value
        End Set
    End Property


    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property IsSilentQuote() As Boolean
        Set(value As Boolean)
            m_bIsSilentQuote = value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        'Click event of the Cancel button.
        'Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20021023 : Close the Party  Policy Summary Screen, if they
            '               are loaded
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Application.DoEvents()
            m_lReturn = ClosePartyPolicySummary()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Process the next set of actions depending
            ' upon the interface task etc.

            m_lReturn = uctRiskScreen1.CancelClick()
            Application.DoEvents()
            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'update the risk cnt property - in case it has changed
                m_lRiskId = uctRiskScreen1.RiskId ' RAW 03/09/2004 : added

                'if cancelling a child add, then must delete that instance
                If m_sChildObjectName <> "" And (m_sChildOIKey = "" Or uctRiskScreen1.ChildAddStatus) Then
                    m_lReturn = uctRiskScreen1.DelObjectInstance(v_sObjectName:=m_sChildObjectName, v_sOIKey:=uctRiskScreen1.ChildOIKey)
                End If
                ' Everything OK, so we can hide the interface.
                'Developer Guide No. 231
                Me.Hide()
            End If

        Catch excep As System.Exception
            ' Error Section.
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        '    m_lReturn& = ShowHelp(dlgHelp, ScreenHelpID)

        'Developer Guide No.50

        Dim objFrmLegend As New frmLegend()
        objFrmLegend.ShowDialog()

    End Sub

    Private Sub cmdLossSchedule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLossSchedule.Click

        ShowLossSchedule()

    End Sub

    Private Sub ShowLossSchedule()
        'to do list
        'Dim oLossSchedule As iCLMLossSchedule.Interface_Renamed
        Dim oLossSchedule As Object
        Dim vKeyArray(,) As Object

        Const PMKeyRowLossSchedule As Integer = 0
        Const PMKeyRowLossScheduleTypeId As Integer = 1
        Const PMKeyRowWorkClaimPerilId As Integer = 2
        Const PMKeyRowRiskId As Integer = 3
        Const PMKeyRowClaimId As Integer = 4
        Const PMKeyRowTransactionType As Integer = 5
        Const PMKeyRowPartyCnt As Integer = 6
        Const PMKeyRowPerilTypeId As Integer = 7
        Const PMKeyRowClaimInsFileCnt As Integer = 8


        Try

            'Call the LossSchedule component
            Dim temp_oLossSchedule As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oLossSchedule, sClassName:="iCLMLossSchedule.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oLossSchedule = temp_oLossSchedule

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLossSchedule", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            m_lReturn = oLossSchedule.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oLossSchedule.Dispose()
                oLossSchedule = Nothing

                Exit Sub
            End If

            'Set lossscheduletypeid in set keys
            'Were going to show a form if this isnt set
            ReDim vKeyArray(1, 8)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowLossSchedule) = PMNavKeyConst.PMKeyNameLossSchedule

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowLossSchedule) = m_bLossSchedule


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowLossScheduleTypeId) = PMNavKeyConst.PMKeyNameLossScheduleTypeId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowLossScheduleTypeId) = m_lLossScheduleTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowWorkClaimPerilId) = PMNavKeyConst.PMKeyNameWorkClaimPerilID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowWorkClaimPerilId) = m_lClaimWorkPerilID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowRiskId) = PMNavKeyConst.PMKeyNameRiskID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowRiskId) = m_lRiskId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimId) = PMNavKeyConst.PMKeyNameClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimId) = m_lClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowTransactionType) = PMNavKeyConst.PMKeyNameTransactionType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowTransactionType) = m_sTransactionType


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowPartyCnt) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPartyCnt) = m_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowPerilTypeId) = PMNavKeyConst.PMKeyNamePerilID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPerilTypeId) = m_lPerilTypeId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimInsFileCnt) = PMNavKeyConst.PMKeyNameClaimInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimInsFileCnt) = m_lClaimInsFileCnt


            m_lReturn = oLossSchedule.SetKeys(vKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If



            m_lReturn = oLossSchedule.Start()

            m_lReturn = uctRiskScreen1.UpdateLossScheduleReserve()

            oLossSchedule = Nothing

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowLossSchedule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLossSchedule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bOKToProceed As Boolean
        Try


            '****************
            ' MEvans : 17-08-2004 : CQ6555
            ' wrapped okclick functionality as it is used
            ' by loss schedule click.
            ' RAW 18/08/2004 : CQ6555 : added r_bOKToProceed param
            m_lReturn = ProcessRiskScreenOkClick(r_bOKToProceed:=bOKToProceed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            ' RAW 18/08/2004 : CQ6555 : added
            If Not bOKToProceed Then
                Exit Sub
            End If


            '**************
            ' MEvans : 05-09-2003 :  CQ2455
            ' destroy the any policies associated
            ' with this specific screen
            If Not (m_oPolicySummary Is Nothing) Then
                m_oPolicySummary.Dispose()
                m_oPolicySummary = Nothing
            End If
            '**************

            ' Everything OK, so we can hide the interface.
            Me.Hide()
            '****************

            Exit Sub

        Catch ex As Exception
            ' Error Section.
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Exit Sub
        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        Try

            ' Add the Hook
            iPMFunc.ShowFormInTaskBar_Attach()

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Initialize failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        'iPMFunc.ForceForegroundWindow(Handle.ToInt32())
        iPMFunc.ShowFormInTaskBar_Detach()

        Dim lToolBarOffset As Integer
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            With uctRiskScreen1

                .Task = m_iTask%
                .TransactionType = m_sTransactionType
                ' Party Cnt
                .PartyCnt = m_lPartyCnt
                .ShortName = m_sShortName
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .RiskId = m_lRiskId
                .RiskTypeId = m_lRiskTypeId
                .ProductId = m_lProductId
                .ScreenId = m_lScreenId
                .SubScreen = m_bSubScreen
                .ParentObjectName = m_sParentObjectName
                .ChildObjectName = m_sChildObjectName
                .ParentOIKey = m_sParentOIKey
                .ChildOIKey = m_sChildOIKey
                .ObjectType = m_lObjectType

                .RiskTypeDetails = m_vRiskTypeDetails
                .ChildIndex = m_lChildIndex
                .CopyRisk = m_bCopyRisk
                .GISPolicyLinkID = m_lGISPolicyLinkID

                If Not (m_oGIS Is Nothing) Then

                    .GIS = m_oGIS
                End If

                '-------------------------------------------------------------------------------------
                '   15/07/2002  RVH BEGIN
                '                   Add new variables for claim stuff
                '-------------------------------------------------------------------------------------
                .ClaimID = m_lClaimID
                .ClaimPerilID = m_lClaimPerilID
                .PerilID = m_lPerilID
                .WorkClaimID = m_lClaimWorkID
                .WorkClaimPerilID = m_lClaimWorkPerilID
                .ClaimTransactionType = m_sClaimTransactionType
                .ClaimInsFileCnt = m_lClaimInsFileCnt
                .ClaimRiskId = m_lClaimRiskId
                '-------------------------------------------------------------------------------------
                '   15/07/2002  RVH END
                '-------------------------------------------------------------------------------------
                'DP 30/05/2003 - added for Stargate 1.2
                If Not (m_cList Is Nothing) Then
                    .List = m_cList
                End If
                If Information.IsArray(m_aArray) Then

                    .vArray = VB6.CopyArray(m_aArray)
                End If
                'DP end

                'DP 30/05/2003 - added for passing data from parent to child
                If Information.IsArray(m_aChildDataFromParent) Then


                    .ChildDataFromParent = m_aChildDataFromParent
                End If
                'DP end

                .NoTransactions = m_bNoTransactions

                .RenewalConfirmationMode = m_bRenewalConfirmationMode 'PN19313
                .IsWhatIfQ = m_bIsWhatIfQ 'PN19313

                'Developer Guide No.24      
                .KeyArray = m_vKeyArray 'PN24176
                .CaseID = m_lCaseID
                .BaseCaseID = m_lBaseCaseID
            End With

            'm_lReturn = CType(uctRiskScreen1, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = uctRiskScreen1.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Throw New Exception(Information.Err().Description)
            End If

            m_lReturn = uctRiskScreen1.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Throw New Exception(Information.Err().Description)
            End If

            m_lReturn = uctRiskScreen1.GetRisk()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the risk.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Throw New Exception(Information.Err().Description)
            End If

            With uctRiskScreen1

                ' Now get the risk id in case it has changed
                m_lRiskId = .RiskId ' RAW 03/09/2004 : Resilience (#2) : added

                If m_lObjectType = GISDataModelType.GISOTAssociatedClient And m_lChildIndex > 0 Then
                    ' PW231003 - CQ2927 - don't use childindex property

                    .Interface_Renamed.GIS.GetPropertyValue("ASSOCIATED_CLIENT", "PARTY_CNT", m_sChildOIKey, m_lPartyCnt)

                    .PartyCnt = m_lPartyCnt
                Else
                    .PartyCnt = m_lPartyCnt
                End If

            End With

            ' RVH 24/12/2004 - override screen caption if one passed
            If m_sScreenCaption.Trim() = "" Then
                Me.Text = uctRiskScreen1.ScreenDesc
            Else
                'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5)
                Me.Text = uctRiskScreen1.ScreenDesc & " " & m_sScreenCaption
                'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.5)
            End If

            'Enable LossSchedule if set
            cmdLossSchedule.Visible = m_bLossSchedule

            ' Only show the check list button if this is a claims screen - AND at the peril level...
            If m_sChildObjectName.ToUpper() = "WORK_CLAIM_PERIL" Then
                cmdCheckList.Visible = True
            Else
                cmdCheckList.Visible = False
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'RAM20021023 :  NRMA Changes - Sirius Process Number 126 - Start
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'We need to show the Toolbar only if:
            ' a) If it is a Claims Builder Risk Screen  ' if m_lClaimID > 0 then it is
            'AND
            ' b) If it is a Top level screen.           ' if m_bSubScreen = false then it is top level screen
            '
            'Amended to also show toolbar on peril screen as per existing iCLMPeril screen functionality
            If (Not m_bSubScreen Or m_sChildObjectName.ToUpper() = "WORK_CLAIM_PERIL") And m_lClaimID > 0 Then
                ' Make the toolbar visible
                Toolbar1.Items.Item("Policy").Visible = False
                Toolbar1.Items.Item("Risk").Visible = False
                Toolbar1.Items.Item("Party").Visible = False
                Toolbar1.Items.Item("Event").Visible = False
                Toolbar1.Items.Item("Financial").Visible = False
                Toolbar1.Visible = True
                lToolBarOffset = 420 ' Height of Toolbar = 420
            Else
                If m_sTransactionType = "C_NC" Or m_sTransactionType = "C_EC" Or m_sTransactionType = "C_VC" Then

                    Toolbar1.Items.Item("Event").Visible = True
                    Toolbar1.Items.Item("Notes").Visible = True
                    Toolbar1.Items.Item("History").Visible = True
                    Toolbar1.Visible = True

                    If Task = gPMConstants.PMEComponentAction.PMAdd Then
                        Toolbar1.Items.Item("Event").Enabled = False
                        Toolbar1.Items.Item("Notes").Enabled = False
                        Toolbar1.Items.Item("History").Enabled = False
                    End If

                    lToolBarOffset = 420
                Else
                    ' Make the toolbar Invisible
                    Toolbar1.Visible = False
                    lToolBarOffset = 0
                End If
            End If

            uctRiskScreen1.Top = VB6.TwipsToPixelsY(lToolBarOffset + 60) ' Normal top of the Risk Screen Control

            Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(uctRiskScreen1.Height) + 1150 + lToolBarOffset) ' RAM20021023 : Added an Offset
            Me.Width = uctRiskScreen1.Width + VB6.TwipsToPixelsX(315)

            cmdHelp.Top = Me.ClientRectangle.Height - (cmdHelp.Height + VB6.TwipsToPixelsY(100))
            cmdCancel.Top = cmdHelp.Top
            cmdOK.Top = cmdHelp.Top
            cmdLossSchedule.Top = cmdHelp.Top
            cmdCheckList.Top = cmdHelp.Top


            'Add Task Button not required here
            cmdAddTask.Visible = True            
            cmdAddTask.Top = cmdHelp.Top + 2            

            cmdAddTask.Enabled = Not (m_sTransactionType = "")
            ' If loss schedule not shown, then move checklist button around
            If Not m_bLossSchedule Then
                cmdCheckList.Left = cmdLossSchedule.Left
            Else
                cmdCheckList.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdLossSchedule.Left) + VB6.PixelsToTwipsX(cmdLossSchedule.Width) + 100)
            End If

            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(1380)
            cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(1290)
            cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(1290)
            cmdAddTask.Left = cmdOK.Left - VB6.TwipsToPixelsX(1410)

            If m_lClaimID > 0 Then
                Toolbar1.Items.Item("Policy").Visible = True
                Toolbar1.Items.Item("Risk").Visible = True
                Toolbar1.Items.Item("Party").Visible = True
                Toolbar1.Items.Item("Event").Visible = True

                If Not m_bSubScreen Then
                    Toolbar1.Items.Item("Financial").Visible = True
                End If
            Else
                Toolbar1.Items.Item("Policy").Visible = False
                Toolbar1.Items.Item("Risk").Visible = False
                Toolbar1.Items.Item("Party").Visible = False
                Toolbar1.Items.Item("Event").Visible = False
                Toolbar1.Items.Item("Financial").Visible = False
            End If
            If m_sTransactionType = "C_NC" Or m_sTransactionType = "C_EC" Or m_sTransactionType = "C_VC" Then

                Toolbar1.Items.Item("Event").Visible = True
                Toolbar1.Items.Item("Notes").Visible = True
                Toolbar1.Items.Item("History").Visible = True

                If Task = gPMConstants.PMEComponentAction.PMAdd Then
                    Toolbar1.Items.Item("Event").Enabled = False
                    Toolbar1.Items.Item("Notes").Enabled = False
                    Toolbar1.Items.Item("History").Enabled = False
                End If

            End If

            If m_sCallingAppName.Trim().ToUpper() = "ICLMFINDCASE" Then
                cmdHelp.Visible = False
                cmdCancel.Left = Me.Width - VB6.TwipsToPixelsX(1380)
                cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(1290)
                cmdAddTask.Left = cmdOK.Left - VB6.TwipsToPixelsX(1410)
            End If
            'Me.ZOrder (1)
            'BringWindowToTop(Me.Handle.ToInt32())

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Me.Dispose()
        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20021023 : Close the Party  Policy Summary Screen, if they
            '               are loaded
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            m_lReturn = ClosePartyPolicySummary()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode AndAlso Not m_bIsSilentQuote Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                If m_bReserveLimitExceeded Then
                    uctRiskScreen1.CancelSilently = True
                End If

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctRiskScreen1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            uctRiskScreen1.Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                               sMsg:="Failed to terminate the interface", _
                               vApp:=ACApp, _
                               vClass:=ACClass, _
                               vMethod:="Form_QueryUnload", _
                               vErrNo:=Information.Err().Number, _
                               vErrDesc:=excep.Message, _
                               excep:=excep)
            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : switchTo
    '
    ' Description   : Switches focus to this form.
    '
    ' Edit History  :
    ' RAM20021024   : Created
    ' ***************************************************************** '
    Public Function SwitchTo() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            SetForegroundWindow(Me.Handle.ToInt32())

            ' Set the focus
            Me.Activate()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="switchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub Form_Terminate_Renamed()
        iPMFunc.ShowFormInTaskBar_Detach()
    End Sub

    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar1_Button1.Click, _Toolbar1_Button2.Click, _Toolbar1_Button3.Click, _Toolbar1_Button4.Click, _Toolbar1_Button5.Click, _Toolbar1_Button6.Click, _Toolbar1_Button7.Click, _Toolbar1_Button8.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)
        Dim iCLMFinSumm, bOpenClaim, bPMUPolicy As Object

        Dim oPMUPolicy As bPMUPolicy.Business


        Dim oBusiness As bOpenClaim.Business
        Dim UnderwritingOrAgency As String = ""

        Dim oFinancialSummary As iCLMFinSumm.Interface_Renamed
        Try

            If g_oObjectManager Is Nothing Then
                g_oObjectManager = New bObjectManager.ObjectManager()
                g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)
            End If

            If Not m_bClientPolicyDetailsLoaded And (m_sTransactionType <> "C_NC" And m_sTransactionType <> "C_EC" And m_sTransactionType <> "C_VC") Then
                Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bOpenClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusiness = temp_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    Exit Sub
                End If

                If m_sTransactionType = "C_CO" Then
                    m_lEventClaimID = 0 'we don't have any events for this claim yet
                Else

                    m_lReturn = oBusiness.GetOriginalClaimNo(v_lClaimId:=m_lClaimID, r_lOriginalClaimID:=m_lOriginalClaimId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="oBusiness.GetOriginalClaimNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick")
                        Exit Sub
                    End If

                    m_lEventClaimID = m_lOriginalClaimId
                End If



                m_lReturn = oBusiness.GetClaimNumber(v_lClaimId:=m_lClaimID, r_sClaimNumber:=m_sClaimNo)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="oBusiness.GetClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick")
                    Exit Sub
                End If


                m_lReturn = oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lPartyCnt:=m_lPartyCnt, r_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, r_sInsuranceRef:=m_sInsuranceRef)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="oBusiness.GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick")
                    Exit Sub
                End If

                ' Remove the instance of the object

                oBusiness.Dispose()
                oBusiness = Nothing

                m_bClientPolicyDetailsLoaded = True
            End If

            'm_lReturn = iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), False)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("ToolBar_Buttonclick", "SetWindowPlacement Failed")
            'End If

            Dim vClientCode As Object
            Dim sClientCode As String = ""
            Dim sOption, sSPUrl, sDocLIB As String
            Select Case (Button.Name)
                Case "Financial"
                    Dim temp_oFinancialSummary As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oFinancialSummary, sClassName:="iCLMFinSumm.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oFinancialSummary = temp_oFinancialSummary

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If

                    If oFinancialSummary Is Nothing Then Exit Sub

                    CType(oFinancialSummary, SSP.S4I.Interfaces.ILocalInterface).Initialise()

                    oFinancialSummary.SetProcessModes(Task, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)

                    oFinancialSummary.ClaimId = m_lClaimID

                    oFinancialSummary.Start()

                    oFinancialSummary.Dispose()
                    oFinancialSummary = Nothing

                Case "Party"
                    ' RAM20021023 : NRMA Changes (Sirius Process No 126)
                    m_lReturn = ShowPartySummaryDetails()
                Case "Policy"
                    ' RAM20021023 : NRMA Changes (Sirius Process No 126)
                    m_lReturn = ShowPolicySummaryDetails()
                Case "Risk"
                    m_lReturn = ShowRiskDetails()
                    ' RB1408003 : CQ717 - End
                Case "Event", "Notes"

                    m_lReturn = ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lClaimCnt:=m_lEventClaimID, v_sInsuranceRef:=m_sInsuranceRef, v_sClaimRef:=m_sClaimNo, v_sTransactionType:=m_sTransactionType, v_bSearchOnPartyCnt:=False, v_sButtonKey:=Button.Name)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick")
                        Exit Sub
                    End If
                Case "History"
                    ShowCaseHistory()
                    'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)
                    'This button Doc Archive is newly added for implementing the document archive functionality
                Case "DocArchive"
                    m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return
                    End If

                    If sOption = "2" Then
                        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5085, r_sOptionValue:=sSPUrl, v_iSourceID:=g_iSourceID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return
                        End If


                        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDocLIB, v_iSourceID:=g_iSourceID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return
                        End If
                    End If


                    Dim temp_oPMUPolicy As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oPMUPolicy, "bPMUPolicy.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oPMUPolicy = temp_oPMUPolicy

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMUPolicy.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Exit Sub
                    End If


                    m_lReturn = oPMUPolicy.GetClientCode(v_iPartyID:=m_lPartyCnt, r_vClientarray:=vClientCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMUPolicy.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Exit Sub
                    End If


                    sClientCode = gPMFunctions.ToSafeString(CStr(vClientCode(0, 0)))

                    oPMUPolicy.Dispose()
                    If m_sTransactionType = ACTransactionType Then
                        If sOption = "1" Then
                            m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=sClientCode.Trim() & "1")
                        ElseIf sOption = "2" Then
                            System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & sClientCode.Trim())
                        End If

                    Else
                        If sOption = "1" Then
                            m_lReturn = iPMFunc.RunDocumaster(v_sLinkCode:=m_sClaimNo.Trim() & "2")
                        ElseIf sOption = "2" Then
                            System.Diagnostics.Process.Start(sSPUrl & sDocLIB & "\" & sClientCode.Trim() & "\Claim\" & m_sClaimNo.Trim())
                        End If

                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)

            End Select
            'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)

            'm_lReturn = SetWindowPlacement(Me.hwnd, True)
            'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("ToolBar_Buttonclick", "SetWindowPlacement Failed")
            'End If
            MyBase.Focus()


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick", excep:=ex)
        Finally

        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name          : ShowPartySummaryDetails
    '
    ' Description   : This function will create the Party Summary Interface,
    '                   set it keys and show the interface
    '
    ' Edit History  :
    ' RAM20021023   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Public Function ShowPartySummaryDetails() As Integer


        Dim result As Integer = 0
        Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal ' Show the Form vbModal

        Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless ' Show the Form vbModeless

        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 2) ' Set the Navigator Keys


            vKeyArray(0, 0) = PMNavKeyConst.PMKeyNamePartyCnt ' Sent in the Party Cnt

            vKeyArray(1, 0) = m_lPartyCnt


            vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameShortName

            vKeyArray(1, 1) = m_sShortName.Trim() ' Sent in the Party Short Name


            vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameDisplayMode

            vKeyArray(1, 2) = ACShowFormModeLess ' Note : Show Form as ModeLess for NRMA

            If m_oPartySummary Is Nothing Then

                ' Create the Interface if not available
                m_oPartySummary = New iSIRPartySummary.Interface_Renamed()

                m_lReturn = m_oPartySummary.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oPartySummary.CallingAppName = ACApp


                m_lReturn = m_oPartySummary.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oPartySummary.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Swith to the Party Summary Interface (i.e show it on top of all interface)
                m_lReturn = m_oPartySummary.switchTo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPartySummary.switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPartySummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPartySummaryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : ShowPolicySummaryDetails
    '
    ' Description   : This function will create the Policy Summary Interface,
    '                   set it keys and show the interface
    '
    ' Edit History  :
    ' RAM20021023   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Public Function ShowPolicySummaryDetails() As Integer


        Dim result As Integer = 0
        Dim ACShowFormModal As FormShowConstants = FormShowConstants.Modal ' Show the Form vbModal

        Dim ACShowFormModeLess As FormShowConstants = FormShowConstants.Modeless ' Show the Form vbModeless

        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 5) ' Set the Navigator Keys


            vKeyArray(0, 0) = PMNavKeyConst.PMKeyNamePartyCnt ' Sent in the Party Cnt

            vKeyArray(1, 0) = m_lPartyCnt


            vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameShortName

            vKeyArray(1, 1) = m_sShortName.Trim()


            vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

            vKeyArray(1, 2) = m_lInsuranceFolderCnt


            vKeyArray(0, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

            vKeyArray(1, 3) = m_lInsuranceFileCnt


            vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameInsReference

            vKeyArray(1, 4) = m_sInsuranceRef.Trim()


            vKeyArray(0, 5) = PMNavKeyConst.PMKeyNameDisplayMode

            vKeyArray(1, 5) = ACShowFormModeLess ' Note : Show Form as ModeLess for NRMA

            If m_oPolicySummary Is Nothing Then

                ' Create the Interface if not available
                m_oPolicySummary = New iSIRPolicySummary.Interface_Renamed()

                m_lReturn = m_oPolicySummary.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oPolicySummary.CallingAppName = ACApp


                m_lReturn = m_oPolicySummary.SetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oPolicySummary.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                ' Swith to the Policy Summary Interface  (i.e show it on top of all interface)
                m_lReturn = m_oPolicySummary.switchTo()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPolicySummary.switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowPolicySummaryDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowPolicySummaryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : ClosePartyPolicySummary
    '
    ' Description   : This function will Close the Party & Policy Summary
    '                   Interfaces, if they are loaded
    '
    ' Edit History  :
    ' RAM20021023   : Created  - NRMA Changes (Sirius Process No 126)
    ' ***************************************************************** '
    Private Function ClosePartyPolicySummary() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Terminate the objects if necessary and available
            If m_oPartySummary Is Nothing Then
                ' Do nothing
            Else
                ' Terminate
                m_oPartySummary.Dispose()
                'Clear it
                m_oPartySummary = Nothing
            End If

            If m_oPolicySummary Is Nothing Then
                ' Do nothing
            Else
                ' Terminate
                m_oPolicySummary.Dispose()
                'Clear it
                m_oPolicySummary = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClosePartyPolicySummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClosePartyPolicySummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : ShowRiskDetails
    '
    ' Description   : This function will show the Risk Details Interface,
    '                 set it's keys and show the interface
    '
    ' Edit History  :
    ' RB14082003    : Created for CQ717
    ' ***************************************************************** '
    Private Function ShowRiskDetails() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim vResultArray(,) As Object
        Dim oObject As Interface_Renamed

        Dim oBusiness As bCLMRiskDetails.Business
        Dim UnderwritingOrAgency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            If g_oObjectManager Is Nothing Then
                g_oObjectManager = New bObjectManager.ObjectManager()
                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails")
                    Return result
                End If
            End If
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bCLMRiskDetails.Business", vInstanceManager:="ClientManager")
            oBusiness = temp_oBusiness

            m_lReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=UnderwritingOrAgency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Show Risk Details Failed - getUnderwritingOrAgency", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails")
                Return result
            End If


            oBusiness.UnderwritingOrAgency = UnderwritingOrAgency


            m_lReturn = oBusiness.GetRiskDetails_U(v_lClaimId:=m_lClaimID, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMURisk.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oObject.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject.Dispose()
                oObject = Nothing
                Return result
            End If


            oObject.InsuranceFolderCnt = CInt(vResultArray(0, 0))

            oObject.InsuranceFileCnt = CInt(vResultArray(1, 0))

            oObject.ProductId = CInt(vResultArray(2, 0))

            oObject.RiskId = CInt(vResultArray(3, 0))

            oObject.RiskTypeId = CInt(vResultArray(4, 0))

            oObject.ScreenId = CInt(vResultArray(5, 0))

            m_lReturn = oObject.Start()

            ' Note - There is no need to run dynamic logic here since nothing in this form has changed
            oObject.Dispose()

            'termiante oBusiness object

            oBusiness.Dispose()

            'clean up and release memory for objects
            oBusiness = Nothing
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRiskDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowEvents
    '
    ' Description:
    '
    ' History: 02/10/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function ShowEvents(ByVal v_lPartyCnt As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0, Optional ByVal v_sInsuranceRef As String = "", Optional ByVal v_sClaimRef As String = "", Optional ByVal v_sTransactionType As String = "", Optional ByVal v_lAccountKey As Integer = 0, Optional ByVal v_bSearchOnPartyCnt As Boolean = True, Optional ByVal v_sButtonKey As String = "") As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim oListEvents As iPMBListEvents.Interface_Renamed

            If g_oObjectManager Is Nothing Then
                g_oObjectManager = New bObjectManager.ObjectManager()
                m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents")
                    Return result
                End If
            End If

            Dim temp_oListEvents As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oListEvents, sClassName:="iPMBListEvents.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oListEvents = temp_oListEvents

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Creation of object oListEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents")
                result = gPMConstants.PMEReturnCode.PMFalse
                oListEvents = Nothing
                Return result
            End If


            m_lReturn = oListEvents.Initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oListEvents.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents")
                result = gPMConstants.PMEReturnCode.PMFalse
                oListEvents = Nothing
                Return result
            End If

            'CMG/PB 20022003 Bug Fix 1838, Show all events for the policy,not just this client
            'This will then show all Lead Client and Corresponance Flag changes
            ' Alix - Added an optional parameter to check if we search on the client or not
            'If v_bSearchOnPartyCnt Then

            oListEvents.PartyCnt = v_lPartyCnt
            'End If
            'End CMG

            oListEvents.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oListEvents.InsuranceFileCnt = v_lInsuranceFileCnt
            'If m_sTransactionType <> "C_CR" Then

            oListEvents.ClaimCnt = m_lClaimID
            ' End If

            oListEvents.InsuranceRef = v_sInsuranceRef

            oListEvents.ClaimRef = v_sClaimRef

            oListEvents.TransactionType = v_sTransactionType

            oListEvents.AccountKey = v_lAccountKey

            oListEvents.CaseID = m_lCaseID

            oListEvents.CaseNumber = m_sCaseNumber

            oListEvents.BaseCaseID = m_lBaseCaseID

            oListEvents.BaseClaimId = m_lOriginalClaimId

            If m_sTransactionType = "C_NC" Or m_sTransactionType = "C_EC" Or m_sTransactionType = "C_VC" Then
                Select Case v_sButtonKey
                    Case "Event"

                        oListEvents.ShowNonNotes = True

                        oListEvents.ShowNotes = False
                    Case "Notes"

                        oListEvents.ShowNotes = True

                        oListEvents.ShowNonNotes = False

                        oListEvents.RTFNotes = True
                End Select
            End If

            If m_sTransactionType = "REN" Then
                oListEvents.EventGroupCode = "N_CLAIMS"
            End If

            m_lReturn = oListEvents.Start
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oListEvents.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents")
                result = gPMConstants.PMEReturnCode.PMFalse
                oListEvents = Nothing
                Return result
            End If


            ' Note - There is no need to run dynamic logic here since nothing in this form has changed



            oListEvents.Dispose()

            oListEvents = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ProcessRiskScreenOkClick
    '
    ' Parameters: n/a
    '
    ' Description: Wrappered all ok click functionality as this is
    '               reused in calls to salvage / debt recovery /
    '                loss schedule...
    ' History:
    '           Created : MEvans : 17-08-2004 : CQ6555
    ' RAW 18/08/2004 : added r_bOKToProceed param
    ' ***************************************************************** '
    Public Function ProcessRiskScreenOkClick(ByRef r_bOKToProceed As Boolean) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "ProcessRiskScreenOkClick"

        Dim sTemp, sFailure As String
        Dim bFailureIsTerminal, bIsInsured, bTalkedToPerson As Boolean
        Dim tTemp As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            r_bOKToProceed = False ' RAW 18/08/2004 : CQ6555 : added
            'PN 29235
            If uctRiskScreen1.ValueEdited Then
                uctRiskScreen1.CallLostFocusAndValidateEvent()
            End If

            m_lReturn = uctRiskScreen1.OKClick()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the insurance file and folder count
            m_lInsuranceFileCnt = uctRiskScreen1.InsuranceFileCnt
            m_lInsuranceFolderCnt = uctRiskScreen1.InsuranceFolderCnt
            m_lRiskId = uctRiskScreen1.RiskId
            m_iIsRiAtRiskLevel = uctRiskScreen1.IsRiAtRiskLevel
            m_iIsAutoReinsured = uctRiskScreen1.IsAutoReinsured


            m_vScreenValues = uctRiskScreen1.ScreenValues
            m_sChildOIKey = uctRiskScreen1.ChildOIKey

            m_sDeclineReasons = uctRiskScreen1.DeclineReasons
            m_sReferReasons = uctRiskScreen1.ReferReasons
            m_sMessages = uctRiskScreen1.Messages
            m_sQuoteType = uctRiskScreen1.QuoteType

            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH BEGIN
            '                   Add new variables for claim stuff
            '-------------------------------------------------------------------------------------
            m_lClaimID = uctRiskScreen1.ClaimID
            m_lClaimPerilID = uctRiskScreen1.ClaimPerilID
            m_lPerilID = uctRiskScreen1.PerilID
            m_lClaimWorkID = uctRiskScreen1.WorkClaimID
            m_lClaimWorkPerilID = uctRiskScreen1.WorkClaimPerilID
            m_sClaimTransactionType = uctRiskScreen1.ClaimTransactionType
            m_lClaimInsFileCnt = uctRiskScreen1.ClaimInsFileCnt
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH END
            '-------------------------------------------------------------------------------------
            m_lCaseID = uctRiskScreen1.CaseID
            m_sCaseNumber = uctRiskScreen1.CaseNumber

            ' RAW 03/09/2004 : moved to after module variables have been set from uctRiskScreen1
            'fix up keys and do any additional tests for special objects
            Select Case m_lObjectType
                ' Process the OK in the control
                Case GISDataModelType.GISOTAssociatedClient
                    'fix up keys and values

                    With uctRiskScreen1.Interface_Renamed.GIS
                        If m_sChildOIKey = "" Then
                            ' RAW 23/06/2003 : CQ786 : replaced existing code by getting the correct ChildOIKey property directly
                            m_sChildOIKey = uctRiskScreen1.ChildOIKey
                        End If

                        .SetPropertyValue("ASSOCIATED_CLIENT", "PARTY_CNT", m_sChildOIKey, m_lPartyCnt)
                        ' PW050903 - CQ1912 - key is now file not folder

                        .SetPropertyValue("ASSOCIATED_CLIENT", "INSURANCE_FILE_CNT", m_sChildOIKey, m_lInsuranceFileCnt)

                        .SetPropertyValue("ASSOCIATED_CLIENT", "RISK_CNT", m_sChildOIKey, m_lRiskId)

                        'don't allow save if GISOTAssociatedClient and no associated client

                        .GetPropertyValue("ASSOCIATED_CLIENT", "IS_INSURED", m_sChildOIKey, bIsInsured)
                        If m_lPartyCnt = 0 And Not bIsInsured Then
                            Interaction.MsgBox("Please create an associated client before saving this screen", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Warning")
                            Return result
                        End If
                    End With
                Case GISDataModelType.GISOTDisclosure
                    'fix up keys
                    If m_sChildOIKey = "" Then
                        ' RAW 23/06/2003 : CQ786 : replaced existing code by getting the correct ChildOIKey property directly
                        m_sChildOIKey = uctRiskScreen1.ChildOIKey
                    End If

                    With uctRiskScreen1.Interface_Renamed.GIS

                        .GetPropertyValue("ASSOCIATED_CLIENT", "PARTY_CNT", m_sParentOIKey, m_lPartyCnt)

                        .SetPropertyValue("DISCLOSURE", "PARTY_CNT", m_sChildOIKey, m_lPartyCnt)

                        .SetPropertyValue("DISCLOSURE", "INSURANCE_FOLDER_CNT", m_sChildOIKey, m_lInsuranceFolderCnt)

                        ' RAW 24/06/2003 : CQ786 : added
                        ' if user has talked to the person and the associated client is insured then
                        ' the disclosure should apply to all risks ( ie set it to null)

                        .GetPropertyValue("ASSOCIATED_CLIENT", "IS_INSURED", m_sParentOIKey, bIsInsured)

                        .GetPropertyValue("DISCLOSURE", "TALKED_TO_PERSON", m_sChildOIKey, bTalkedToPerson)
                        If bIsInsured And bTalkedToPerson Then


                            .SetPropertyValue("DISCLOSURE", "RISK_CNT", m_sChildOIKey, DBNull.Value)
                        Else

                            .SetPropertyValue("DISCLOSURE", "RISK_CNT", m_sChildOIKey, m_lRiskId)
                        End If

                    End With
            End Select
            ' RAW 03/09/2004 : end

            Select Case m_sQuoteType
                'Tomo 11062001 - remove Validation from the 'not terminal' list,
                'as we want to stay in the form.
                '    Case "Default", "Validation", "User Authority Limits"
                Case "Default", "User Authority Limits"
                    sTemp = m_sQuoteType
                Case "Validation"
                    sTemp = m_sQuoteType
                    bFailureIsTerminal = True
                Case Else
                    sTemp = "Quote"
                    bFailureIsTerminal = True
            End Select

            'We get only decline reasons from non-quote calls to NBQuote
            If m_sDeclineReasons <> "" Then
                ' RAW 20/09/2004 : CQ6832 : replaced (sTemp="Quote") with (bFailureIsTerminal = False)
                If Not bFailureIsTerminal Then
                    sFailure = "declined"
                Else
                    sFailure = "failed"
                End If

                ''------------PN:69816 Upender PN:70683 Nishchal------------------------------------------------
                'This code is added to print the validation message row wise.

                tTemp = m_sDeclineReasons.Replace(" <br />", Environment.NewLine)

                MessageBox.Show(sTemp & " " & sFailure & " because:" & " " & Strings.Chr(13) & Strings.Chr(10) & tTemp & Strings.Chr(13) & Strings.Chr(10), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                ''---------------------------------------------------------------------

                If bFailureIsTerminal Then
                    Return result
                End If
                ' Reset the interface status so we don't continue with peril allocation

                m_lStatus = gPMConstants.PMEReturnCode.PMCancel ' RAW 20/09/2004 : CQ6832 : replaced PMError
            End If

            ' User Authority limits can get here!
            If m_sReferReasons <> "" Then
                ' RAW 20/09/2004 : CQ6832 : replaced (sTemp="Quote") with (bFailureIsTerminal = False)
                If Not bFailureIsTerminal Then
                    sFailure = "referred"
                Else
                    sFailure = "failed"
                End If

                MessageBox.Show(sTemp & " " & sFailure & " because:" & Strings.Chr(13) & Strings.Chr(10) & m_sReferReasons, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                If bFailureIsTerminal Then
                    Return result
                End If
                ' Reset the interface status so we don't continue with peril allocation
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel ' RAW 20/09/2004 : CQ6832 : replaced PMError
            End If

            If m_sMessages <> "" Then
                MessageBox.Show("Note: " & m_sMessages, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            'if launched from Swift Black Box and not a sub-screen
            If m_lInsuranceFolderCnt = -1 And Not m_bSubScreen Then
                'get XML as string


                m_vXMLDataSet = uctRiskScreen1.XMLDataSet
            End If

            r_bOKToProceed = True ' RAW 18/08/2004 : CQ6555 : added

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    Private Function ShowCheckList() As Integer
        Dim result As Integer = 0

        Const sFunctionName As String = "ShowCheckList"

        'To do list
        'Dim oObject As iCLMInfoChklst.Interface_Renamed
        Dim oObject As Object
        Dim vKeyArray(1, 4) As Object
        Dim sClaimRef As String = ""

        Dim oBusiness As bOpenClaim.Business

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bOpenClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " - Failed to create object 'bOpenClaim.Business'.", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            m_lReturn = oBusiness.GetClaimNumber(v_lClaimId:=m_lClaimID, r_sClaimNumber:=sClaimRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Failed To Get Claim Reference", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                oBusiness = Nothing
                Return result
            End If

            oBusiness = Nothing

            Dim temp_oObject As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iCLMInfoChklst.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " - Failed to create object 'iCLMInfoChklst.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            vKeyArray(0, 0) = "claim_ref"

            vKeyArray(1, 0) = sClaimRef

            vKeyArray(0, 1) = "risk_type_id"

            vKeyArray(1, 1) = m_lRiskId

            vKeyArray(0, 2) = "claim_cnt"

            vKeyArray(1, 2) = m_lClaimID

            vKeyArray(0, 3) = "claim_mode"

            vKeyArray(1, 3) = gPMConstants.PMEComponentAction.PMEdit

            vKeyArray(0, 4) = "DeleteWorkTableFlag"

            vKeyArray(1, 4) = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = oObject.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)


            oObject.CallingAppName = ACApp ' Need to set AppName to prevent from deleteting Claim in Checklist

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oObject.SetKeys(vKeyArray:=vKeyArray)
            End If

            'm_lReturn = iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), False)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("ShowCheckList", "SetWindowPlacement Failed")
            'End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oObject.Start()
            End If


            oObject.Dispose()

            oObject = Nothing

            'm_lReturn = iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), True)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("ShowCheckList", "SetWindowPlacement Failed")
            'End If

            Return result

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    Private Function CreateWorkManagerTask(Optional ByVal sPartyShortName As String = "", Optional ByVal dDueDate As Date = #12/30/1899#, Optional ByVal sDescription As String = "") As Integer
        Dim result As Integer = 0

        Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
        Dim lReturn, v_lAction As Integer
        Dim vKeyArray(,) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_lAction = 1

            ' Create the Component
            Dim temp_oTaskInstance As Object
            lReturn = g_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskInstance = temp_oTaskInstance
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Process Modes

            lReturn = oTaskInstance.SetProcessModes(vTask:=v_lAction, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Task Group Id and Task Id

            oTaskInstance.PMWrkTaskGroupId = 5

            oTaskInstance.PMWrkTaskId = 18


            ReDim vKeyArray(1, 1)
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "keep_window_on_top"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = 1
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePartyCnt
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lPartyCnt
            lReturn = oTaskInstance.SetKeys(vKeyArray)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateWorkManagerTask = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Pass the MultipleTaskInstanceDisplayForm relevant values
            If m_sTransactionType = "C_NC" Or m_sTransactionType = "C_EC" Then


                oTaskInstance.Customer = g_oObjectManager.UserName.Trim()
            Else


                oTaskInstance.Customer = sPartyShortName.Trim() & " " & sDescription.Trim()
            End If


            oTaskInstance.DueDate = dDueDate

            oTaskInstance.Description = sDescription

            oTaskInstance.DisableCustomer = gPMConstants.PMEReturnCode.PMTrue

            oTaskInstance.TaskStatus = 2

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Task Instance Display Form:-      iPMWrkTaskInstanceDisplay.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' If the User Cancelled then exit as we do not need
            ' to Refresh the Form details.

            If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                'r_vPMWrkTaskInstanceCntArray = ""

                oTaskInstance.Dispose()
                oTaskInstance = Nothing
                Return result
            End If

            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask", vApp:=ACApp, vClass:=ACClass, vMethod:=" CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    Private Function AddTaskGetDetails(Optional ByRef lPartyCnt As Integer = 0, Optional ByRef lInsuranceFolderCnt As Integer = 0, Optional ByRef sInsuranceRef As String = "0", Optional ByRef sPartyShortName As String = "", Optional ByRef dtDueDate As Date = #12/30/1899#, Optional ByRef sClaimNo As String = "") As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            Dim oBusiness As bOpenClaim.Business
            result = gPMConstants.PMEReturnCode.PMTrue
            If Not m_bClientPolicyDetailsLoaded Then
                Dim temp_oBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bOpenClaim.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oBusiness = temp_oBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                m_lReturn = oBusiness.GetClientPolicyDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lPartyCnt:=lPartyCnt, r_lInsuranceFolderCnt:=lInsuranceFolderCnt, r_sInsuranceRef:=sInsuranceRef, r_sPartyShortName:=sPartyShortName, r_vRenewalDate:=dtDueDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="oBusiness.GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick")
                    Return result
                End If

                If m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR" Then

                    m_lReturn = oBusiness.GetClaimNumber(v_lClaimId:=m_lClaimID, r_sClaimNumber:=sClaimNo)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="oBusiness.GetClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tbrRisk_ButtonClick")
                        Return result
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskGetDetails", vApp:=ACApp, vClass:=ACClass, vMethod:=" AddTaskGetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowCaseHistory
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Private Function ShowCaseHistory() As Integer
        Dim result As Integer = 0
        Dim iCLMCaseHistory As Object

        Const kMethodName As String = "ShowCaseHistory"

        Dim lReturn As gPMConstants.PMEReturnCode

        'To do list alkesh
        'Dim oObject As iCLMCaseHistory.Interface_Renamed
        Dim oObject As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oObject As Object
            lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iCLMCaseHistory.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oObject.BaseCaseID = m_lBaseCaseID

            oObject.CaseNumber = m_sCaseNumber


            m_lReturn = oObject.Initialise
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " oObject.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oObject.Start
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " oObject.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If Not (oObject Is Nothing) Then

                oObject.Dispose()
            End If



        End Try
        Return result
    End Function


    'Private Sub uctRiskScreen1_PerilEditClick(ByRef KeepOnTop As Boolean)
    'If KeepOnTop Then
    'BringWindowToTop(Me.Handle.ToInt32())
    'Else
    'm_lReturn = iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), False)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("uctRiskScreen1_PerilEditClick", "SetWindowPlacement Failed")
    'End If
    'End If
    'End Sub
    ''' <summary>
    ''' Wrappered all ok click functionality as this is
    ''' reused in calls to salvage / debt recovery / loss schedule...
    ''' Quote the risk without showing the interface
    ''' </summary>
    ''' <param name="r_bOKToProceed"></param>
    ''' <param name="bIsSilentQuote"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessRiskScreenOkClick(ByRef r_bOKToProceed As Boolean,
                                             ByVal bIsSilentQuote As Boolean) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const sFunctionName As String = "ProcessRiskScreenOkClick"
        Dim sTemp2 As String = ""
        Dim sTemp, sFailure As String
        Dim bFailureIsTerminal, bIsInsured, bTalkedToPerson As Boolean
        Dim sStr As Object 'PN 69817
        Dim iIncr As Integer 'PN 69817
        Dim tTemp As String = "" 'PN 69817


        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            r_bOKToProceed = False ' RAW 18/08/2004 : CQ6555 : added
            'PN 29235

            If uctRiskScreen1.ValueEdited Then
                uctRiskScreen1.CallLostFocusAndValidateEvent()
            End If
            m_lReturn = uctRiskScreen1.OKClick()
            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the insurance file and folder count
            m_lInsuranceFileCnt = uctRiskScreen1.InsuranceFileCnt
            m_lInsuranceFolderCnt = uctRiskScreen1.InsuranceFolderCnt
            If m_lRiskId = 0 Then
                m_lRiskId = uctRiskScreen1.RiskId
            End If
            m_iIsRiAtRiskLevel = uctRiskScreen1.IsRiAtRiskLevel
            m_iIsAutoReinsured = uctRiskScreen1.IsAutoReinsured
            m_vScreenValues = uctRiskScreen1.ScreenValues
            m_sChildOIKey = uctRiskScreen1.ChildOIKey
            m_sDeclineReasons = uctRiskScreen1.DeclineReasons
            m_sReferReasons = uctRiskScreen1.ReferReasons
            m_sMessages = uctRiskScreen1.Messages
            m_sQuoteType = uctRiskScreen1.QuoteType

            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH BEGIN
            '                   Add new variables for claim stuff
            '-------------------------------------------------------------------------------------
            m_lClaimID = uctRiskScreen1.ClaimID
            m_lClaimPerilID = uctRiskScreen1.ClaimPerilID
            m_lPerilID = uctRiskScreen1.PerilID
            m_lClaimWorkID = uctRiskScreen1.WorkClaimID
            m_lClaimWorkPerilID = uctRiskScreen1.WorkClaimPerilID
            m_sClaimTransactionType = uctRiskScreen1.ClaimTransactionType
            m_lClaimInsFileCnt = uctRiskScreen1.ClaimInsFileCnt
            '-------------------------------------------------------------------------------------
            '   15/07/2002  RVH END
            '-------------------------------------------------------------------------------------
            m_lCaseID = uctRiskScreen1.CaseID
            m_sCaseNumber = uctRiskScreen1.CaseNumber

            ' RAW 03/09/2004 : moved to after module variables have been set from uctRiskScreen1
            'fix up keys and do any additional tests for special objects
            Select Case m_lObjectType
                ' Process the OK in the control
                Case GISDataModelType.GISOTAssociatedClient
                    'fix up keys and values

                    With uctRiskScreen1.Interface_Renamed.GIS
                        If m_sChildOIKey = "" Then
                            ' RAW 23/06/2003 : CQ786 : replaced existing code by getting the correct ChildOIKey property directly
                            m_sChildOIKey = uctRiskScreen1.ChildOIKey
                        End If

                        .SetPropertyValue("ASSOCIATED_CLIENT", "PARTY_CNT", m_sChildOIKey, m_lPartyCnt)
                        ' PW050903 - CQ1912 - key is now file not folder

                        .SetPropertyValue("ASSOCIATED_CLIENT", "INSURANCE_FILE_CNT", m_sChildOIKey, m_lInsuranceFileCnt)

                        .SetPropertyValue("ASSOCIATED_CLIENT", "RISK_CNT", m_sChildOIKey, m_lRiskId)

                        'don't allow save if GISOTAssociatedClient and no associated client

                        .GetPropertyValue("ASSOCIATED_CLIENT", "IS_INSURED", m_sChildOIKey, bIsInsured)
                        If m_lPartyCnt = 0 And Not bIsInsured Then
                            If Not bIsSilentQuote Then
                                Interaction.MsgBox("Please create an associated client before saving this screen",
                                                   MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Warning")
                            Else
                                HandleErrorOnSilentQuote(sMsg:="Associated Client not Present",
                                                         sMethod:="ProcessRiskScreenOKClick")
                            End If
                            Return nResult
                        End If
                    End With
                Case GISDataModelType.GISOTDisclosure
                    'fix up keys
                    If m_sChildOIKey = "" Then
                        ' RAW 23/06/2003 : CQ786 : replaced existing code by getting the correct ChildOIKey property directly
                        m_sChildOIKey = uctRiskScreen1.ChildOIKey
                    End If

                    With uctRiskScreen1.Interface_Renamed.GIS

                        .GetPropertyValue("ASSOCIATED_CLIENT", "PARTY_CNT", m_sParentOIKey, m_lPartyCnt)

                        .SetPropertyValue("DISCLOSURE", "PARTY_CNT", m_sChildOIKey, m_lPartyCnt)

                        .SetPropertyValue("DISCLOSURE", "INSURANCE_FOLDER_CNT", m_sChildOIKey, m_lInsuranceFolderCnt)

                        ' RAW 24/06/2003 : CQ786 : added
                        ' if user has talked to the person and the associated client is insured then
                        ' the disclosure should apply to all risks ( ie set it to null)

                        .GetPropertyValue("ASSOCIATED_CLIENT", "IS_INSURED", m_sParentOIKey, bIsInsured)

                        .GetPropertyValue("DISCLOSURE", "TALKED_TO_PERSON", m_sChildOIKey, bTalkedToPerson)
                        If bIsInsured And bTalkedToPerson Then


                            .SetPropertyValue("DISCLOSURE", "RISK_CNT", m_sChildOIKey, DBNull.Value)
                        Else

                            .SetPropertyValue("DISCLOSURE", "RISK_CNT", m_sChildOIKey, m_lRiskId)
                        End If

                    End With
            End Select
            ' RAW 03/09/2004 : end

            Select Case m_sQuoteType
                'Tomo 11062001 - remove Validation from the 'not terminal' list,
                'as we want to stay in the form.
                '    Case "Default", "Validation", "User Authority Limits"
                Case "Default", "User Authority Limits"
                    sTemp = m_sQuoteType
                Case "Validation"
                    sTemp = m_sQuoteType
                    bFailureIsTerminal = True
                Case Else
                    sTemp = "Quote"
                    bFailureIsTerminal = True
            End Select

            'We get only decline reasons from non-quote calls to NBQuote
            If m_sDeclineReasons <> "" Then
                ' RAW 20/09/2004 : CQ6832 : replaced (sTemp="Quote") with (bFailureIsTerminal = False)
                If Not bFailureIsTerminal Then
                    sFailure = "declined"
                Else
                    sFailure = "failed"
                End If

                ''------------PN:69816 Upender PN:70683 Nishchal------------------------------------------------
                'This code is added to print the validation message row wise.

                If Not bIsSilentQuote Then
                    sTemp2 = m_sDeclineReasons.Replace(" <br />", Environment.NewLine)

                    MessageBox.Show(sTemp & " " & sFailure & " because:" & " " & Strings.Chr(13) &
                                Strings.Chr(10) & sTemp2 & Strings.Chr(13) & Strings.Chr(10),
                                Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    HandleErrorOnSilentQuote(sMsg:=sTemp & " " & sFailure & " because:" & m_sDeclineReasons,
                                             sMethod:="ProcessRiskScreenOKClick")
                End If

                If bFailureIsTerminal Then
                    Return nResult
                End If
                ' Reset the interface status so we don't continue with peril allocation

                m_lStatus = gPMConstants.PMEReturnCode.PMCancel ' RAW 20/09/2004 : CQ6832 : replaced PMError
            End If

            ' User Authority limits can get here!
            If m_sReferReasons <> "" Then
                ' RAW 20/09/2004 : CQ6832 : replaced (sTemp="Quote") with (bFailureIsTerminal = False)
                If Not bFailureIsTerminal Then
                    sFailure = "referred"
                Else
                    sFailure = "failed"
                End If

                If Not bIsSilentQuote Then
                    MessageBox.Show(sTemp & " " & sFailure & " because:" & Strings.Chr(13) & Strings.Chr(10) & m_sReferReasons, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    HandleErrorOnSilentQuote(sMsg:=sTemp & " " & sFailure & " because:" & vbCrLf & m_sReferReasons,
                                        sMethod:="ProcessRiskScreenOKClick")
                End If

                If bFailureIsTerminal Then
                    Return nResult
                End If
                ' Reset the interface status so we don't continue with peril allocation
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel ' RAW 20/09/2004 : CQ6832 : replaced PMError
            End If

            If m_sMessages <> "" Then
                If Not bIsSilentQuote Then
                    MessageBox.Show("Note: " & m_sMessages, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            'if launched from Swift Black Box and not a sub-screen
            If m_lInsuranceFolderCnt = -1 And Not m_bSubScreen Then
                'get XML as string
                m_vXMLDataSet = uctRiskScreen1.XMLDataSet
            End If

            r_bOKToProceed = True ' RAW 18/08/2004 : CQ6555 : added

            Return nResult
        Catch ex As System.Exception
            Throw New Exception(sFunctionName + " Failed", ex)
        End Try
    End Function
    ''' <summary>
    ''' ProcessOKClickForSilentQuote
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function ProcessOKClickForSilentQuote() As Long
        Const kMethodName As String = "ProcessOKClickForSilentQuote"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim bOKToProceed As Boolean

        Try

            ProcessOKClickForSilentQuote = gPMConstants.PMEReturnCode.PMTrue

            With uctRiskScreen1
                .Task = m_iTask%
                .TransactionType = m_sTransactionType
                .PartyCnt = m_lPartyCnt
                .ShortName = m_sShortName
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .RiskId = m_lRiskId
                .RiskTypeId = m_lRiskTypeId
                .ProductId = m_lProductId
                .ScreenId = m_lScreenId
                .SubScreen = m_bSubScreen
                .ParentObjectName = m_sParentObjectName
                .ChildObjectName = m_sChildObjectName
                .ParentOIKey = m_sParentOIKey
                .ChildOIKey = m_sChildOIKey
                .ObjectType = m_lObjectType

                .RiskTypeDetails = m_vRiskTypeDetails
                .ChildIndex = m_lChildIndex
                .CopyRisk = m_bCopyRisk
                .GISPolicyLinkID = m_lGISPolicyLinkID
                If Not (m_oGIS Is Nothing) Then
                    .GIS = m_oGIS
                End If
                If Not (m_cList Is Nothing) Then
                    .List = m_cList
                End If
                If Information.IsArray(m_aArray) Then

                    .vArray = VB6.CopyArray(m_aArray)
                End If
                If Information.IsArray(m_aChildDataFromParent) Then
                    .ChildDataFromParent = m_aChildDataFromParent
                End If
                .NoTransactions = m_bNoTransactions
            End With

            m_lReturn = uctRiskScreen1.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Throw New Exception(Information.Err().Description)
            End If
            uctRiskScreen1.IsSilentQuote = True
            m_lReturn = uctRiskScreen1.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Throw New Exception(Information.Err().Description)
            End If

            m_lReturn = uctRiskScreen1.GetRisk()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the risk.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Throw New Exception(Information.Err().Description)
            End If

            nReturn = ProcessRiskScreenOkClick(r_bOKToProceed:=bOKToProceed,
                                             bIsSilentQuote:=True)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                HandleErrorOnSilentQuote(v_sMsg:="ProcessRiskScreenOKClick Failed",
                                         v_vMethod:=kMethodName)
                ProcessOKClickForSilentQuote = gPMConstants.PMEReturnCode.PMFalse
            End If

            If bOKToProceed = False Then
                ProcessOKClickForSilentQuote = gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not m_oPolicySummary Is Nothing Then
                ' Call m_oPolicySummary.Terminate()
                m_oPolicySummary = Nothing
            End If

            Return nReturn
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessOKClickForSilentQuote)
        End Try
    End Function

    Private Sub HandleErrorOnSilentQuote(ByVal v_sMsg As String,
                                    ByVal v_vMethod As Object)
        Dim lReturn As Long
        Const kMethodName As String = "HandleErrorOnSilentQuote"

        Try
            'Try to Log the message Silently
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_vMethod, bSilent:=True)

            'Set the status to PMCancel
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Catch
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn)
        End Try
    End Sub




    ''' <summary>
    '''HandleErrorOnSilentQuote
    ''' </summary>
    ''' <param name="sMsg"></param>
    ''' <param name="sMethod"></param>
    ''' <remarks></remarks>
    Private Sub HandleErrorOnSilentQuote(ByVal sMsg As String, _
                                         ByVal sMethod As String)

        Const kMethodName As String = "HandleErrorOnSilentQuote"

        Try
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:=sMsg, _
                       vApp:=ACApp, vClass:=ACClass, _
                       vMethod:=sMethod, bSilent:=True)
            m_lStatus = PMEReturnCode.PMCancel
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName, _
                                           iType:=PMELogLevel.PMLogOnError, _
                                           sMsg:=kMethodName + " Failed", _
                                           vApp:=ACApp, vClass:=ACClass, _
                                           vMethod:=kMethodName, _
                                           vErrNo:=Err().Number, _
                                           vErrDesc:=ex.Message, excep:=ex)
        End Try
    End Sub
    Private Sub frmInterface_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt Then
            CType(uctRiskScreen1.Controls.Find("TabStrip1", True)(0), TabControl).Focus()
        End If
    End Sub
End Class
