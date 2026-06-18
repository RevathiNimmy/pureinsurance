Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMPerilRT_NET.uctCLMPerilRT")> _
Partial Public Class uctCLMPerilRT
    Inherits System.Windows.Forms.UserControl
    '
    ' History:
    ' CJB 200705 PN22497 Changed LoadControl to only call CheckIsLegacyClaim for u/w
    ' CJB 200705 PN22498 Changed LoadPerilData to only add PolicyCurrency & LossCurrency columns to listview for u/w
    '

    Private Const ACClass As String = "uctCLMPerilRTControl"

    Private m_oBusiness As Object

    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lclaimid As Integer
    Private m_sClaimRiskDesciption As String = ""
    Private m_lRisk As Integer
    Private m_lPolicyId As Integer
    Private m_sClaimNumber As String = ""
    Private m_bLossSchedule As Boolean
    Private m_lReturn As Integer
    Private m_bAlreadyEdited As Boolean
    Private m_lClaimMode As Integer
    Private m_lOriginalClaimId As Integer
    Private m_bClaimsBuilder As Boolean
    Private m_sUnderwritingOrAgency As String = ""
    Private m_sRiskDescription As String = ""
    Private m_lRiskType As Integer
    Private m_bViewRiskFlag As Boolean
    Private m_lTotalPerilRows As Integer
    Private m_bEditCheckFlag As Boolean

    Public Event PerilListChanged(ByVal Sender As Object, ByVal e As EventArgs)
    Public Event AddClick(ByVal Sender As Object, ByVal e As AddClickEventArgs)
    Public Event EditClick(ByVal Sender As Object, ByVal e As EditClickEventArgs)
    Public Event DeleteClick(ByVal Sender As Object, ByVal e As DeleteClickEventArgs)
    Public Event OnControlGotFocus(ByVal Sender As Object, ByVal e As OnControlGotFocusEventArgs)

    Private Const ACColWidthMoney As Integer = 1500

    Private Const ACColumnClaimID As Integer = 0
    Private Const ACColumnClaimPerilID As Integer = 1
    Private Const ACColumnRiskDescription As Integer = 2
    Private Const ACColumnDescription As Integer = 3
    Private Const ACColumnSum_Insured As Integer = 4
    Private Const ACColumnCurrentReserve As Integer = 5
    Private Const ACColumnGISScreen As Integer = 6
    Private Const ACColumnOriginalClaimPerilID As Integer = 7
    Private Const ACColumnLossScheduleTypeID As Integer = 8
    Private Const ACColumnPerilTypeID As Integer = 9
    Private Const ACColumnPolicyCurrencyCode As Integer = 10
    Private Const ACColumnLossCurrencyCode As Integer = 11
    Private Const ACColumnIncurred As Integer = 12
    Private Const ACColumnTPRecovery As Integer = 13
    Private Const ACColumnSalvageRecovery As Integer = 14
    Private Const ACColumnPaidLossAmount As Integer = 15

    Private Const ACColumnPerilTypeID_BR As Integer = 10

    Private m_bLegacyClaim As Boolean
    Private m_bOpenClaimNoTrans As Boolean
    Private m_bRI2007Enabled As Boolean
    Private bIsPaymentsReadOnly As Boolean?
    'End CMG
    ''' <summary>
    ''' Holds the flag for IsPaymentsReadOnly configured in product maintenanc.
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property IsPaymentReadOnly() As Boolean
        Get
            If Not bIsPaymentsReadOnly.HasValue Then
                bIsPaymentsReadOnly = GetIsPaymentsReadOnly()
            End If
            Return bIsPaymentsReadOnly
        End Get
    End Property
    'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)
    Private m_sScreenCaption As String = ""
    <Browsable(True)> _
    Public Property ScreenCaption() As String
        Get
            ' Return the interface exit status.
            Return m_sScreenCaption
        End Get
        Set(ByVal Value As String)
            m_sScreenCaption = Value
        End Set
    End Property
    'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)

    <Browsable(True)> _
    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            ' Set the interface exit status.
            m_lStatus = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property PerilCount() As Integer
        Get
            Return lvwPerils.Items.Count
        End Get
    End Property


    <Browsable(True)> _
    Public Property Policy() As Integer
        Get
            Return m_lPolicyId
        End Get
        Set(ByVal Value As Integer)
            m_lPolicyId = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property Risk() As Integer
        Get
            Return m_lRisk
        End Get
        Set(ByVal Value As Integer)
            m_lRisk = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property Claimid() As Integer
        Get
            Return m_lclaimid
        End Get
        Set(ByVal Value As Integer)
            m_lclaimid = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ClaimMode() As Integer
        Set(ByVal Value As Integer)
            m_lClaimMode = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property ViewRiskFlag() As Boolean
        Get
            Return m_bViewRiskFlag
        End Get
        Set(ByVal Value As Boolean)
            m_bViewRiskFlag = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property IsOpenClaimNoTrans() As Boolean
        Get
            Return m_bOpenClaimNoTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
    End Property

    Private Sub cmdPerilAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPerilAdd.Click
        Dim vDataArray(,) As Object
        Dim sMessage, sTitle As String
        'JMK 23/05/2001 'Dim iPerilID As Integer: change to Long
        Dim lPerilID As Integer

        Dim vKeyArray(,) As Object
        ReDim vKeyArray(1, 5)

        Const ACPerilType As Integer = 0
        Const ACDescription As Integer = 1
        Const PMKeyClaimGISScreenID As String = "GIS_Screen_id"


        m_lReturn = GetPerilTypes(vDataArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            SetPerilButtons()
            Exit Sub
        End If

        If Not Information.IsArray(vDataArray) Then

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddPerilTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddPerilDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
            SetPerilButtons()
            Exit Sub
        End If

        Dim oFrmAddPeril As New frmAddPeril

        oFrmAddPeril.tabAddPeril.Top = VB6.TwipsToPixelsY(120)
        oFrmAddPeril.tabAddPeril.Left = VB6.TwipsToPixelsX(120)

        oFrmAddPeril.tabAddPeril.Height = oFrmAddPeril.cmdCancel.Top - VB6.TwipsToPixelsY(120) - oFrmAddPeril.tabAddPeril.Top
        oFrmAddPeril.tabAddPeril.Width = oFrmAddPeril.Width - VB6.TwipsToPixelsX(240) - oFrmAddPeril.tabAddPeril.Left

        ' Get Peril types and populate the FRMPeril's  cmbPeril combobox
        If Information.IsArray(vDataArray) Then

            For iPeril As Integer = vDataArray.GetLowerBound(1) To vDataArray.GetUpperBound(1)
                Dim cmbPeril_NewIndex As Integer = -1

                cmbPeril_NewIndex = oFrmAddPeril.cmbPeril.Items.Add(CStr(vDataArray(ACDescription, iPeril)))

                VB6.SetItemData(oFrmAddPeril.cmbPeril, cmbPeril_NewIndex, CInt(vDataArray(ACPerilType, iPeril)))
            Next iPeril
        End If

        If oFrmAddPeril.cmbPeril.Items.Count > 0 Then
            oFrmAddPeril.cmbPeril.SelectedIndex = 0
        End If

        oFrmAddPeril.ShowDialog()

        If oFrmAddPeril.Status <> gPMConstants.PMEReturnCode.PMOK Then
            oFrmAddPeril = Nothing
            SetPerilButtons()
            Exit Sub
        End If


        'Public Function AddClaimPeril(ByVal v_iClaimId As Integer, ByVal v_iPerilTypeId As Integer, r_iClaimPerilId As Integer) As Long
        m_lReturn = AddClaimPeril(v_iPerilTypeId:=VB6.GetItemData(oFrmAddPeril.cmbPeril, oFrmAddPeril.cmbPeril.SelectedIndex), r_lPerilID:=lPerilID, v_sDescription:=oFrmAddPeril.txtDescription.Text)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            SetPerilButtons()
            Exit Sub
            oFrmAddPeril = Nothing
        End If

        'JMK 23/05/2001 - call Sub LoadPerilData with new optional flag
        '                   (to indicate where it was called from)
        LoadPerilData(1)

        If m_sUnderwritingOrAgency = "U" Then
            For Each oItem As ListViewItem In lvwPerils.Items
                If Convert.ToString(oItem.Tag) <> "" Then
                    If Convert.ToString(oItem.Tag) = lPerilID Then

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyClaimGISScreenID
                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(oItem, kSubItemsGisScreen).Text, 0)

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameWorkClaimPerilID
                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lPerilID

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameLossSchedule
                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_bLossSchedule

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePerilTypeId
                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = ListViewHelper.GetListViewSubItem(oItem, kSubItemsPerilTypeId).Text

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameLossScheduleTypeId
                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = ListViewHelper.GetListViewSubItem(oItem, kSubItemsLossScheduleTypeId).Text

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameNoTransaction
                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_bOpenClaimNoTrans

                    End If
                End If
            Next oItem

            RaiseEvent PerilListChanged(Me, Nothing)

            RaiseEvent AddClick(Me, New AddClickEventArgs(vKeyArray))

        End If

        oFrmAddPeril = Nothing

        ' set peril buttons (disable in in claim payment mode)
        SetPerilButtons()

    End Sub

    Private Sub cmdPerilDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPerilDelete.Click
        DeletePeril()
    End Sub


    Private Sub cmdPerilEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPerilEdit.Click

        'clicked again
        If m_bEditCheckFlag Then
            m_bEditCheckFlag = False
            Exit Sub
        End If

        m_bEditCheckFlag = True

        'constants for the SetKeys declaration
        Const PMKeyRiskType As String = "risk_type"
        Const PMKeyRiskID As String = "risk_id"
        Const PMKeyClaimGISScreenID As String = "GIS_Screen_id"

        Const PMKeyClaimMode As String = "claim_mode"

        Const PMKeyRowPerilID As Integer = 0
        Const PMKeyRowClaimID As Integer = 1
        Const PMKeyRowRiskType As Integer = 2
        Const PMKeyRowInsuranceFilecnt As Integer = 3
        Const PMKeyRowRiskID As Integer = 4
        Const PMKeyRowClaimMode As Integer = 5
        Const PMKeyRowClaimGISScreen As Integer = 6
        Const PMKeyRowInsuranceFoldercnt As Integer = 7
        Const PMKeyRowWorkClaimId As Integer = 8
        Const PMKeyRowWorkClaimPerilId As Integer = 9
        Const PMKeyRowClaimTransactionType As Integer = 10
        Const PMKeyRowClaimInsFileCnt As Integer = 11
        Const PMKeyRowClaimRiskId As Integer = 12
        Const PMKeyRowLossSchedule As Integer = 13
        Const PMKeyRowLossScheduleTypeId As Integer = 14
        Const PMKeyRowPerilTypeId As Integer = 15
        Const PMKeyNoTrans As Integer = 16
        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)
        Const PMKeyRowScreenCaption As Integer = 17
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)


        'developer guide no.108
        Dim oPeril As Object
        'developer guide no. 71 
        Dim vKeyArray(,) As Object
        Dim lClaimId, lClaimPerilId As Integer
        Dim nCurrentReserve, nSumInsured As Single

        ' PW140703 - PS68
        Dim lWorkScreenID As Integer

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        If lvwPerils.FocusedItem Is Nothing Then
            m_bEditCheckFlag = False

            SetPerilButtons()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If
        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)
        ReDim vKeyArray(1, 17)
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)




        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowPerilID) = PMNavKeyConst.PMKeyNameClaimPerilID


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPerilID) = Convert.ToString(lvwPerils.FocusedItem.Tag)


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimID) = PMNavKeyConst.PMKeyNameRealClaimID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimID) = m_lclaimid


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowRiskType) = PMKeyRiskType

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowRiskType) = m_sRiskDescription


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowInsuranceFilecnt) = PMNavKeyConst.PMKeyNameInsFileCnt

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowInsuranceFilecnt) = m_lPolicyId


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowRiskID) = PMKeyRiskID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowRiskID) = m_lRisk


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimMode) = PMKeyClaimMode

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimMode) = m_lClaimMode


        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyNoTrans) = PMNavKeyConst.PMKeyNameNoTransaction

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyNoTrans) = m_bOpenClaimNoTrans
        'Start(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowScreenCaption) = PMNavKeyConst.PMKeyNameScreenCaption

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowScreenCaption) = m_sScreenCaption
        'End(Sriram P)Tech Spec - LOA003 - CLAIMS (GR1.14) Insured Name and Claim Number to Be Displayed V3 - App.doc sec(5.7)
        If m_sUnderwritingOrAgency <> "U" Then

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowPerilTypeId) = PMNavKeyConst.PMKeyNamePerilTypeId

            ' Developer Guide No.52
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPerilTypeId) = lvwPerils.FocusedItem.SubItems(kSubItemsPerilTypeId_BR).Text
        End If

        'GSD 150702
        If m_bClaimsBuilder And (m_sUnderwritingOrAgency = "U") And Not m_bLegacyClaim Then

            '-------------------------------------------------------------------------------------
            '   23/07/2002  RVH BEGIN
            '                   Check to see if there was a GIS_Screen associated with this
            '                   peril type...if not, signal an error.
            '-------------------------------------------------------------------------------------
            m_bEditCheckFlag = False
            'If lvwPerils.listViewHelper1.GetListViewSubItem(lvwPerils.FocusedItem, kSubItemsGisScreen).Text.Trim() = "" Then
            'developer guide no. 52 of Guide
            If lvwPerils.FocusedItem.SubItems.Count < 9 Then

                MessageBox.Show("There is no peril screen associated with this peril type. You must assign one before continuing.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            ElseIf lvwPerils.FocusedItem.SubItems(kSubItemsGisScreen).Text.Trim() = "" Then

                MessageBox.Show("There is no peril screen associated with this peril type. You must assign one before continuing.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            '-------------------------------------------------------------------------------------
            '   23/07/2002  RVH END
            '-------------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------------
            '   17/07/2002  RVH BEGIN
            '                   Use new "hidden" columns to get the gis screen id and the
            '                   original claim peril id (if this is an edit) and pass it on
            '                   - Note the use of the keys...
            '                     Insurance File & Insurance Folder both set to ClaimID
            '                     Risk ID set to Claim Peril ID
            '                   - Data passed will differ dependent on current transaction
            '                     type too - when it is NOT an OPEN, we need to pass the
            '                     ORIGINAL keys to the claim and the claim_peril...as these
            '                     are the values that will have been stashed on the
            '                     GIS_Policy_Link
            '-------------------------------------------------------------------------------------
            If m_sTransactionType <> "C_CO" Then
                lClaimId = m_lOriginalClaimId

                'CMG/PB 20112002 PS202 Only if we havent just added this peril type
                'If lvwPerils.FocusedItem.SubItems(kSubItemsPerilTypeId).Text <> "" Then
                '    lClaimPerilId = CInt(lvwPerils.FocusedItem.SubItems(kSubItemsPerilTypeId).Text)
                'End If
                'this is done because 'lvwPerils.FocusedItem.Tag' stores the claim peril id and we are wrongly
                'assigning periltypeID to claimperilID which goes wrong.
                lClaimPerilId = Convert.ToString(lvwPerils.FocusedItem.Tag)
            Else
                lClaimId = m_lclaimid

                lClaimPerilId = Convert.ToString(lvwPerils.FocusedItem.Tag)
            End If


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimGISScreen) = PMKeyClaimGISScreenID

            'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimGISScreen) = CInt(lvwPerils.listViewHelper1.GetListViewSubItem(lvwPerils.FocusedItem, kSubItemsGisScreen).Text)
            'developer guide no. 52 of Guide
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimGISScreen) = CStr(CInt(lvwPerils.FocusedItem.SubItems(kSubItemsGisScreen).Text))

            'It's important to note that because of the way the GIS_Policy_Link
            'table is being manipulated to use the Insurance_File_Cnt to store
            'the claim id, we pass that HERE into the key array...

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowInsuranceFilecnt) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowInsuranceFilecnt) = lClaimId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowInsuranceFoldercnt) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowInsuranceFoldercnt) = lClaimId

            'It's important to note that because of the way the GIS_Policy_Link
            'table is being manipulated to use the Risk_Id to store
            'the claim peril id, we pass that HERE into the key array...

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowRiskID) = PMKeyRiskID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowRiskID) = lClaimPerilId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowWorkClaimId) = PMNavKeyConst.PMKeyNameWorkClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowWorkClaimId) = m_lclaimid


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowWorkClaimPerilId) = PMNavKeyConst.PMKeyNameWorkClaimPerilID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowWorkClaimPerilId) = Convert.ToString(lvwPerils.FocusedItem.Tag)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimTransactionType) = PMNavKeyConst.PMKeyNameClaimTransactionType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimTransactionType) = m_sTransactionType


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimInsFileCnt) = PMNavKeyConst.PMKeyNameClaimInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimInsFileCnt) = m_lPolicyId


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowClaimRiskId) = PMNavKeyConst.PMKeyNameClaimRiskID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimRiskId) = m_lRisk

            'CMG/PB 12092002 Pass LossSchedule key to display loss schedule button in iPMURisk

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowLossSchedule) = PMNavKeyConst.PMKeyNameLossSchedule

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowLossSchedule) = m_bLossSchedule

            'Were going to show a form if this isnt set

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowLossScheduleTypeId) = PMNavKeyConst.PMKeyNameLossScheduleTypeId

            'Developer Guide No.52
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowLossScheduleTypeId) = lvwPerils.FocusedItem.SubItems(kSubItemsLossScheduleTypeId).Text


            'This is needed if the loss schedule type is not set, we will prompt the user to set it.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, PMKeyRowPerilTypeId) = PMNavKeyConst.PMKeyNamePerilTypeId

            'Developer Guide No. 52
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPerilTypeId) = lvwPerils.FocusedItem.SubItems(kSubItemsPerilTypeId).Text
            'End CMG

            ' PW140703 - PS68 - Use version of claims screen that it was originally
            ' created with, if applicable: start
            Select Case m_sTransactionType
                Case "C_CO"
                    ' Store the screen ID in the work_claim table

                    'Developer Guide No.52
                    m_lReturn = m_oBusiness.SaveGISScreenID(lClaimId:=lClaimPerilId, lScreenId:=CInt(lvwPerils.FocusedItem.SubItems(kSubItemsGisScreen).Text), bPerilLevel:=True)
                    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowPerilTypeId) = lvwPerils.FocusedItem.SubItems(1).Text
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save Screen ID in Work Claim table", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                Case "C_CR", "C_CP"
                    ' Retrieve the screen ID from the work_claim table

                    m_lReturn = m_oBusiness.GetGISScreenID(lClaimId:=lClaimPerilId, r_lScreenId:=lWorkScreenID, bPerilLevel:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Screen ID from Work Claim table", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    ' Override the screen ID retrieved if one existed on the work table
                    If lWorkScreenID <> 0 Then

                        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, PMKeyRowClaimGISScreen) = lWorkScreenID
                    End If
            End Select
            ' PW140703 - PS68: end

            RaiseEvent EditClick(Me, New EditClickEventArgs(vKeyArray))
            'PM027696 JP
            Me.ParentForm.Activate()

            If CBool(ReflectionHelper.GetMember(MyBase.ParentForm, "ReserveLimitExceeded")) = True Then
                MyBase.ParentForm.Close()
            End If

            '*****************
            ' MEvans : 10-03-2003 : Issue 2847
            ' on return - reset the reserve and suminsured details as these may have changed
            m_lReturn = GetReserveDetails(Convert.ToString(lvwPerils.FocusedItem.Tag), nSumInsured, nCurrentReserve)
            '*****************

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'developer guide no. 52
                lvwPerils.FocusedItem.SubItems(kSubItemsSumInsured).Text = StringsHelper.Format(nSumInsured, "0.00")
                lvwPerils.FocusedItem.SubItems(kSubItemsCurrentReserve).Text = StringsHelper.Format(nCurrentReserve, "0.00")
            Else
                '*****************
                ' MEvans : 11-02-2003 : Issue 2144
                ' Stopped Sum Insured being cleared down in no reserve found
                ' lvwPerils.SelectedItem.SubItems(2) = "0.00"
                ' FIX COPIED FROM ORIGINAL COMPONENT iCLMRiskDetails
                'developer guide no. 52 
                lvwPerils.FocusedItem.SubItems(kSubItemsCurrentReserve).Text = "0.00"
                '*****************
            End If
            '*****************

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' AMB 10/03/2003: IS2851 - need to raise this event here before we go!
            RaiseEvent PerilListChanged(Me, Nothing)

            'DGRIFF 27/08/2003 : CQ2250 - Reload list in case description changed by vbscript
            LoadPerilData()

            ' payment mode and just exited the peril screen so lock the user out.
            If (m_sTransactionType = "C_CP" Or m_bOpenClaimNoTrans) And m_lStatus = gPMConstants.PMEReturnCode.PMOK Then
                m_bAlreadyEdited = True
            End If

            ' set peril buttons (disable in in claim payment mode)
            SetPerilButtons()
            Me.Focus()
            Exit Sub

            '-------------------------------------------------------------------------------------
            '   17/07/2002  RVH END
            '-------------------------------------------------------------------------------------
            'm_lReturn = g_oObjectManager.GetInstance(oObject:=oPeril, _
            'sClassName:="iPMURisk.Interface", _
            'vInstanceManager:=PMGetLocalInterface)
        Else
            Dim temp_oPeril As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPeril, sClassName:="iCLMPeril.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPeril = temp_oPeril
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_bEditCheckFlag = False
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object '.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericRisk", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        m_lReturn = oPeril.Initialise
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_bEditCheckFlag = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If


        m_lReturn = oPeril.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_bEditCheckFlag = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If

        If (m_iTask = gPMConstants.PMEComponentAction.PMView) And (m_sTransactionType = "C_CR") Then

            oPeril.DisableScreen = gPMConstants.PMEReturnCode.PMTrue
        End If


        m_lReturn = oPeril.SetKeys(vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_bEditCheckFlag = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If


        m_lReturn = oPeril.Start
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_bEditCheckFlag = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        If CBool(oPeril.ReserveLimitExceeded) = True Then
            ReflectionHelper.SetMember(MyBase.ParentForm, "ReserveLimitExceeded", True)
            ReflectionHelper.SetMember(MyBase.ParentForm, "ExceededReserve", oPeril.ExceededReserve)
            MyBase.ParentForm.Close()
        End If


        If oPeril.Status <> gPMConstants.PMEReturnCode.PMOK Then
            m_bEditCheckFlag = False

            oPeril.Dispose()
            oPeril = Nothing
            SetPerilButtons()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        m_lReturn = oPeril.GetKeys(vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_bEditCheckFlag = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        LoadPerilData()


        If oPeril.Status <> gPMConstants.PMEReturnCode.PMCancel Then
            'developer guide no. 52 of Guide
            lvwPerils.Items.Item(0).SubItems(kSubItemsAddMode).Text = "0"
        End If
        'End If


        oPeril.Dispose()

        oPeril = Nothing


        If m_sTransactionType = "C_CP" Or m_bOpenClaimNoTrans Then
            m_bAlreadyEdited = True
        End If

        SetPerilButtons()
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        RaiseEvent PerilListChanged(Me, Nothing)
        m_bEditCheckFlag = False

    End Sub


    Private Sub lvwPerils_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPerils.DoubleClick
        'Start - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
        'Description :- Bug Id - 22,Peril list : Edit button is always enabled. Should only enable when peril highlighted
        '               Bug Id - 23, Peril list : Delete button always enabled :If trying to delete, gives message which is ambiguous.Does not allow deletion of peril just added.
        '
        ' To rectify the bug the following code has been added.
        'END - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
        If lvwPerils.FocusedItem Is Nothing Then Exit Sub
        If Not cmdPerilEdit.Enabled Then Exit Sub
        cmdPerilEdit_Click(cmdPerilEdit, New EventArgs())
    End Sub

    Private Sub lvwPerils_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwPerils.Enter
        RaiseEvent OnControlGotFocus(Me, New OnControlGotFocusEventArgs("lvwPerils"))
    End Sub
    Private Sub cmdPerilAdd_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPerilAdd.Enter
        RaiseEvent OnControlGotFocus(Me, New OnControlGotFocusEventArgs("cmdPerilAdd"))
    End Sub
    Private Sub cmdPerilEdit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPerilEdit.Enter
        RaiseEvent OnControlGotFocus(Me, New OnControlGotFocusEventArgs("cmdPerilEdit"))
    End Sub
    Private Sub cmdPerilDelete_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPerilDelete.Enter
        RaiseEvent OnControlGotFocus(Me, New OnControlGotFocusEventArgs("cmdPerilcmdPerilDelete"))
    End Sub
    'developer guide no. 
    Private Sub lvwPerils_ItemSelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwPerils.ItemSelectionChanged
        If e.Item.Index + 1 > m_lTotalPerilRows + 1 Then
            e.Item.Selected = False
            cmdPerilEdit.Enabled = False
            cmdPerilDelete.Enabled = False
        End If
        SetPerilButtons()
    End Sub

    Private Sub lvwPerils_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPerils.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'Start - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG
        'Description :- Bug Id - 22,Peril list : Edit button is always enabled. Should only enable when peril highlighted
        '               Bug Id - 23, Peril list : Delete button always enabled :If trying to delete, gives message which is ambiguous.Does not allow deletion of peril just added.
        '
        ' To rectify the bug the following code has been added.
        'END - Bug ID - 22& 23, Date 18th Oct 2000, Author: DG

        If lvwPerils.GetItemAt(x, y) Is Nothing Then
            cmdPerilEdit.Enabled = False
            cmdPerilDelete.Enabled = False
        Else
            SetPerilButtons()
        End If
    End Sub

    Private Sub uctCLMPerilRT_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        Dim lNewWidth As Integer = CInt(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(fraButtons.Width))
        Dim lNewHeight As Integer = CInt(VB6.PixelsToTwipsY(Me.Height))
        Dim lNewLeft As Integer = CInt(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(fraButtons.Width))

        If lNewWidth > 0 Then
            lvwPerils.Width = VB6.TwipsToPixelsX(lNewWidth)
        End If

        lvwPerils.Height = VB6.TwipsToPixelsY(lNewHeight)

        If lNewLeft > 0 Then
            fraButtons.Left = MyBase.Width - fraButtons.Width
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: LoadPerilData
    '
    ' Description: Load peril list view with all the perils defined for
    '              a ClaimId
    '
    ' ***************************************************************** '
    Private Sub LoadPerilData(Optional ByRef m_iCalledFrom As Integer = 0)
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            Dim vDataArray(,) As Object
            Dim lstNewItem As ListViewItem
            Dim lWidth As Integer

            Const ACAddPeril As Integer = 1
            'PN 13417 JT 22-09-2004
            Const ACColumnPolicyCurrency As Integer = 10
            Const ACColumnLossCurrency As Integer = 11

            'JMK 23/05/2001 - don't need to redo this if called from AddPeril
            'Start
            If m_iCalledFrom <> ACAddPeril Then
                lvwPerils.Columns.Clear()


                lvwPerils.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                lvwPerils.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPerilDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                lvwPerils.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSumInsured, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CInt(VB6.TwipsToPixelsX(ACColWidthMoney)))


                ' hide the additional columns from broking
                If m_sUnderwritingOrAgency <> "U" Then
                    lWidth = 0
                Else
                    lWidth = ACColWidthMoney
                End If

                lvwPerils.Columns.Add("Incurred", CInt(VB6.TwipsToPixelsX(lWidth)), HorizontalAlignment.Right)
                lvwPerils.Columns.Add("Paid", CInt(VB6.TwipsToPixelsX(lWidth)), HorizontalAlignment.Right)
                lvwPerils.Columns.Add("Recoveries", CInt(VB6.TwipsToPixelsX(lWidth)), HorizontalAlignment.Right)
                lvwPerils.Columns.Add("Salvage", CInt(VB6.TwipsToPixelsX(lWidth)), HorizontalAlignment.Right)


                lvwPerils.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrentReserve, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CInt(VB6.TwipsToPixelsX(ACColWidthMoney)))

                lvwPerils.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)))

                If m_sUnderwritingOrAgency <> "U" Then
                    lvwPerils.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)))
                End If

                '-------------------------------------------------------------------------------------
                '   17/07/2002  RVH BEGIN
                '                   Add new "hidden" columns to contain the gis screen id and the
                '                   original claim peril id
                '-------------------------------------------------------------------------------------
                If m_sUnderwritingOrAgency = "U" Then
                    lvwPerils.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)))
                    lvwPerils.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)))
                    'CMG/PB 19092002 Loss Schedule Type Id
                    lvwPerils.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)))
                    'Peril Type for Loss Schedule
                    lvwPerils.Columns.Add("", CInt(VB6.TwipsToPixelsX(0)))
                    'End CMG
                End If

                If m_sUnderwritingOrAgency = "U" Then 'PN22498
                    'PN-13417 JT 21-09-04

                    lvwPerils.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

                    lvwPerils.Columns.Add(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClossCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)
                End If

                '-------------------------------------------------------------------------------------
                '   17/07/2002  RVH END
                '-------------------------------------------------------------------------------------

                'TN20010425 Start
                'right justify sum insured and current reserve
                If m_sUnderwritingOrAgency = "U" Then
                    lvwPerils.Columns.Item(kColHeaderSumInsured - 1).TextAlign = HorizontalAlignment.Right
                    lvwPerils.Columns.Item(kColHeaderCurrentReserve - 1).TextAlign = HorizontalAlignment.Right
                End If
                'TN20010425 End

                ' moved this functionality into open claim component to be in line
                ' with claims builder....
                '        If (Task = PMAdd) Or (m_sTransactionType = "C_CR" And m_iTask <> PMView) Then
                'If m_iCalledFrom = ACAddPeril Then
                'If (m_iTask = PMAdd) Then
                'RVH 24/2/2003 - Only call business object method "AddPerilForClaimRisk" if
                '                ClaimsBuilder is switched off, as this call will have already
                '                been made in iOpen claim if it is switched on.
                '   If m_bClaimsBuilder = False Then
                '       If m_sUnderwritingOrAgency = "U" Then
                '           m_lReturn = m_oBusiness.AddPerilForClaimRisk(Policy, Risk, Claimid)
                '       End If
                '   End If
                'RVH 24/2/2003 - End
                'End If
                'End If

            End If
            'JMK end

            If Not m_bViewRiskFlag Then


                m_lReturn = m_oBusiness.GetPerilForClaimRisk(m_lclaimid, m_lRiskType, vDataArray)
                If Not Information.IsArray(vDataArray) Then Exit Sub

                lvwPerils.Items.Clear()


                For iPeril As Integer = vDataArray.GetLowerBound(1) To vDataArray.GetUpperBound(1)
                    lstNewItem = lvwPerils.Items.Add(CStr(vDataArray(ACColumnRiskDescription, iPeril)))

                    ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPerilDescription).Text = CStr(vDataArray(ACColumnDescription, iPeril))


                    If CStr(vDataArray(ACColumnSum_Insured, iPeril)) <> "" Then

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vDataArray(ACColumnSum_Insured, iPeril)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSumInsured).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vDataArray.GetValue(ACColumnSum_Insured, iPeril))
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSumInsured).Text = StringsHelper.Format(vDataArray.GetValue(ACColumnSum_Insured, iPeril), ACFormatforNumber)
                        Else
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSumInsured).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSumInsured).Text = ACFormatforNumber
                        End If
                    Else
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSumInsured).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                        'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSumInsured).Text = ACFormatforNumber
                    End If


                    '**************************************
                    ' Additional Fields For EUROPA GENERAL
                    If m_sUnderwritingOrAgency = "U" Then

                        ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
                        ''Check for RI 2007 if enabled display Incurred as sum of “Initial Reserve” + “Revision Amount” + “Recoveries Received” + Salvage Received"
                        ''Else Display tin he existing(Note:In formula the recoveries amount are treated as negative but in code we get the positive value so we are subtracting.

                        If m_bRI2007Enabled Then

                            If StringsHelper.Format(vDataArray.GetValue(ACColumnIncurred, iPeril), ACFormatforNumber) = "" And StringsHelper.Format(vDataArray.GetValue(ACColumnTPRecovery, iPeril), ACFormatforNumber) = "" Then
                                ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                                'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = ACFormatforNumber
                            Else

                                ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(vDataArray.GetValue(ACColumnIncurred, iPeril)))
                                'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = StringsHelper.Format(gPMFunctions.ToSafeString(gPMFunctions.ToSafeDecimal(CStr(vDataArray.GetValue(ACColumnIncurred, iPeril)), ACFormatforNumber) - gPMFunctions.ToSafeDecimal(CStr(vDataArray.GetValue(ACColumnTPRecovery, iPeril)), ACFormatforNumber)), ACFormatforNumber)
                            End If
                        Else

                            If StringsHelper.Format(vDataArray.GetValue(ACColumnIncurred, iPeril), ACFormatforNumber) = "" Then
                                ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                                'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = ACFormatforNumber
                            Else

                                ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vDataArray.GetValue(ACColumnIncurred, iPeril))
                                'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = StringsHelper.Format(vDataArray.GetValue(ACColumnIncurred, iPeril), ACFormatforNumber)
                            End If
                        End If
                        ''End(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance


                        If StringsHelper.Format(vDataArray.GetValue(ACColumnPaidLossAmount, iPeril), ACFormatforNumber) = "" Then
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPaid).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPaid).Text = ACFormatforNumber
                        Else

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPaid).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vDataArray.GetValue(ACColumnPaidLossAmount, iPeril))
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPaid).Text = StringsHelper.Format(vDataArray.GetValue(ACColumnPaidLossAmount, iPeril), ACFormatforNumber)
                        End If


                        If StringsHelper.Format(vDataArray.GetValue(ACColumnTPRecovery, iPeril), ACFormatforNumber) = "" Then
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsRecoveries).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsRecoveries).Text = ACFormatforNumber
                        Else

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsRecoveries).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vDataArray.GetValue(ACColumnTPRecovery, iPeril))
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsRecoveries).Text = StringsHelper.Format(vDataArray.GetValue(ACColumnTPRecovery, iPeril), ACFormatforNumber)
                        End If


                        If StringsHelper.Format(vDataArray.GetValue(ACColumnSalvageRecovery, iPeril), ACFormatforNumber) = "" Then
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSalvage).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSalvage).Text = ACFormatforNumber
                        Else

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSalvage).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vDataArray.GetValue(ACColumnSalvageRecovery, iPeril))
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSalvage).Text = StringsHelper.Format(vDataArray.GetValue(ACColumnSalvageRecovery, iPeril), ACFormatforNumber)
                        End If
                    Else
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsIncurred).Text = CStr(0)
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPaid).Text = CStr(0)
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsRecoveries).Text = CStr(0)
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsSalvage).Text = CStr(0)
                    End If
                    '*************************************


                    If CStr(vDataArray(ACColumnCurrentReserve, iPeril)) <> "" Then

                        Dim dbNumericTemp2 As Double
                        If Double.TryParse(CStr(vDataArray(ACColumnCurrentReserve, iPeril)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vDataArray.GetValue(ACColumnCurrentReserve, iPeril))
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsCurrentReserve).Text = StringsHelper.Format(vDataArray.GetValue(ACColumnCurrentReserve, iPeril), ACFormatforNumber)
                        Else
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                            'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsCurrentReserve).Text = ACFormatforNumber
                        End If
                    Else
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ACFormatforNumber)
                        'ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsCurrentReserve).Text = ACFormatforNumber
                    End If
                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsAddMode).Text = "1"
                    Else
                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsAddMode).Text = "0"
                    End If

                    If m_sUnderwritingOrAgency <> "U" Then

                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPerilTypeId_BR).Text = CStr(vDataArray(ACColumnPerilTypeID_BR, iPeril))
                    End If

                    '-------------------------------------------------------------------------------------
                    '   17/07/2002  RVH BEGIN
                    '                   Add new "hidden" columns to contain the gis screen id, and the
                    '                   original claim peril id
                    '-------------------------------------------------------------------------------------
                    If m_sUnderwritingOrAgency = "U" Then

                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsGisScreen).Text = CStr(vDataArray(ACColumnGISScreen, iPeril))

                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsOriginalClaimPerilId).Text = CStr(vDataArray(ACColumnOriginalClaimPerilID, iPeril))
                        'CMG/PB 19092002 Loss Schedule


                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsLossScheduleTypeId).Text = CStr(vDataArray(ACColumnLossScheduleTypeID, iPeril))
                        'Peril Type

                        ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPerilTypeId).Text = CStr(vDataArray(ACColumnPerilTypeID, iPeril))
                        'End CMG
                    End If

                    ' CJB 240904 The next PN should be for U/W only as broking don't return these
                    If m_sUnderwritingOrAgency = "U" Then
                        '--PN- 13417

                        If CStr(vDataArray(ACColumnPolicyCurrency, iPeril)) <> "" Then

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPolicyCurrency).Text = CStr(vDataArray(ACColumnPolicyCurrency, iPeril))
                        Else
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsPolicyCurrency).Text = ""
                        End If

                        If CStr(vDataArray(ACColumnLossCurrency, iPeril)) <> "" Then

                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsLossCurrency).Text = CStr(vDataArray(ACColumnLossCurrency, iPeril))
                        Else
                            ListViewHelper.GetListViewSubItem(lstNewItem, kSubItemsLossCurrency).Text = ""
                        End If
                        '***end
                    End If

                    '-------------------------------------------------------------------------------------
                    '   17/07/2002  RVH END
                    '-------------------------------------------------------------------------------------


                    lstNewItem.Tag = CStr(vDataArray(ACColumnClaimPerilID, iPeril))

                Next iPeril

                If m_sUnderwritingOrAgency = "U" Then

                    lReturn = CType(PopulateTotals(vDataArray.GetUpperBound(1)), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed get perils", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPeril", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If
                End If


                m_lTotalPerilRows = vDataArray.GetUpperBound(1)

            End If

        Catch excep As System.Exception




            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed get perils", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPeril", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub


        End Try
    End Sub

    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Static bIsInitialised As Boolean
        Dim sMessage, sTitle As String
        Dim vReturn As String = ""
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sHelpFile As String = ""

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

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
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMRiskDetails.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            Else
                ' Initialise new object

                m_lReturn = m_oBusiness.Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iuserid:=g_iUserId, isourceid:=g_iSourceID, ilanguageid:=g_iLanguageID, icurrencyid:=g_oObjectManager.CurrencyID, iloglevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Business object initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLossSchedule, v_vBranch:=g_iSourceID, r_vUnderwriting:=vReturn)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue failed for Loss Schedule", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            End If

            m_bLossSchedule = (gPMFunctions.NullToString(vReturn) = "1")

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTClaimsBuilder, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vReturn)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue failed for Claims Builder", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            End If

            If vReturn = "1" Then
                m_bClaimsBuilder = True
            End If

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vReturn)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue failed for Enable RI 2007", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            End If

            If vReturn = "1" Then
                m_bRI2007Enabled = True
            End If


            'RWH(12/03/2001)
            'Made full row select on list views
            'developer guide no.303
            lvwPerils.FullRowSelect = True
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwPerils.Handle.ToInt32(), v_vShowRowSelect:=True)
            '' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            bIsInitialised = True

            Return result

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""

        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            ' Set the RiskTypeId & Description
            If Not ViewRiskFlag Then
                m_lReturn = GetRiskDetails()
            Else
                m_lclaimid = 0
                m_lRiskType = Risk
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdPerilAdd.Enabled = False
            End If
            ' Load the peril data
            LoadPerilData(0)

            If m_sUnderwritingOrAgency = "U" Then 'PN22497
                If m_sTransactionType <> "C_CO" Then
                    ' check if the claim is a legacy claim
                    CheckIsLegacyClaim(m_lclaimid)
                End If
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


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

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            SetPerilButtons()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            ' set the caption for the Add button

            cmdPerilAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' set the caption for the Edit button

            cmdPerilEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' set the caption for the Delete button

            cmdPerilDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:        GetPerilTypes
    '
    ' Description: GetPerilTypes gets all the peril type defined for a Policy for
    '               Underwriting or all the peril types for broking. but the Peril types
    '               should not be defined, already, in the Claim_Peril table
    '
    ' ***************************************************************** '
    Private Function GetPerilTypes(ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'MSS260901 - Added for merge
            'TN20010423 Start
            If m_sUnderwritingOrAgency <> "U" Then

                m_lReturn = m_oBusiness.GetPerilTypeForRisk(m_lclaimid, m_lRiskType, m_lPolicyId, r_vDataArray, m_bClaimsBuilder)
            Else

                m_lReturn = m_oBusiness.GetPerilTypeForRisk(m_lclaimid, m_lRisk, m_lPolicyId, r_vDataArray, m_bClaimsBuilder)
            End If
            'TN20010423 End
            'MSS260901 - Merge end

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get peril types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub SetPerilButtons()

        cmdPerilAdd.Enabled = Not (ViewRiskFlag Or m_iTask = gPMConstants.PMEComponentAction.PMView Or (m_sTransactionType = "C_CP") Or m_bRI2007Enabled Or m_bOpenClaimNoTrans)

        If Not (lvwPerils.FocusedItem Is Nothing) Then
            'DC070601
            'If ViewRiskFlag <> True And Task <> PMView Then
            cmdPerilDelete.Enabled = Not ViewRiskFlag And (m_sTransactionType <> "C_CP") And (m_iTask <> gPMConstants.PMEComponentAction.PMView) And Not m_bOpenClaimNoTrans

            cmdPerilEdit.Enabled = ((Not ViewRiskFlag) AndAlso (Not m_bAlreadyEdited)) OrElse IsPaymentReadOnly

            'Fix for PN 4208
            If (lvwPerils.FocusedItem.Text = "") Then
                If (ListViewHelper.GetListViewSubItem(lvwPerils.FocusedItem, kSubItemsSumInsured).Text = "Total") Then
                    cmdPerilEdit.Enabled = False
                    cmdPerilDelete.Enabled = False
                End If
            End If


        Else
            cmdPerilDelete.Enabled = False
            cmdPerilEdit.Enabled = False
        End If

    End Sub

    ' ***************************************************************** '
    ' Name:        AddClaimPeril
    '
    ' Description: AddClaimPeril is called from cmdPerilAdd_ClickOn event.
    '               If OK was pressed on the Add screen peril will be added
    '               in the Claim_Peril table. To add to Claim_Peril the business object is called
    '
    ' JMK 23/05/3001: v_iPerilTypeId As Int - change to Long
    '               'add Risk as Long
    ' ***************************************************************** '
    Private Function AddClaimPeril(ByVal v_iPerilTypeId As Integer, ByRef r_lPerilID As Integer, Optional ByRef v_sDescription As String = "") As Integer
        Dim result As Integer = 0
        Const AC_EVENT_TYPE_UPDATECLAIM As Integer = 6
        Dim sEventDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'ByVal v_iClaimId As Integer, ByVal v_iPerilTypeId As Integer, r_iClaimPerilId As Integer) As Long

            m_lReturn = m_oBusiness.AddClaimPeril(Claimid, v_iPerilTypeId, r_lPerilID, Risk, v_sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oBusiness.PerilID = r_lPerilID

            m_oBusiness.Claimid = Claimid
            sEventDescription = Interaction.InputBox("Enter the Event Description", "Event Log", sEventDescription)

            m_lReturn = m_oBusiness.CreateEvent(AC_EVENT_TYPE_UPDATECLAIM, sEventDescription)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add ClaimPeril", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClaimPeril", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetReserveDetails
    '
    ' Description: Get the reserve details...for non-ClaimsBuilder screens
    '              this is returned from the Peril screen in a key array.
    '              For ClaimsBuilder, we have to re-get the data...
    '
    ' History: 22/07/2002   RVH Created
    '
    ' ***************************************************************** '
    Private Function GetReserveDetails(ByVal lPerilID As Integer, ByRef r_nSumInsured As Single, ByRef r_nCurrentReserve As Single) As Integer
        Dim result As Integer = 0
        Dim oBusiness As Object
        Dim r_vReserveDetailsArray(,) As Object
        Dim r_vRecoveryDetailsArray(,) As Object
        Dim r_vArray(,) As Object
        Dim nSalvage, nRecovery As Single

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_nSumInsured = 0
            r_nCurrentReserve = 0
            '
            '   Get instance of Peril business object
            '
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bCLMPeril.Business", vInstanceManager:="ClientManager")
            oBusiness = temp_oBusiness

            '   Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create business object to re-get reserve details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            '   Have to "trick" the business object into re-setting the main
            '   key details into the data layer...
            '

            oBusiness.PerilID = lPerilID

            oBusiness.Claimid = m_lclaimid


            m_lReturn = oBusiness.GetControls(r_vArray)

            '   Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed calling the GetControls method on the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            '   Get the reserve details back into an array
            '

            m_lReturn = oBusiness.GetReserveDetails(m_lPolicyId, m_lRisk, r_vReserveDetailsArray)

            '   Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-get reserve details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            '   Check if we got some data back...
            '

            If Not Information.IsArray(r_vReserveDetailsArray) Or Object.Equals(r_vReserveDetailsArray, Nothing) Then

                '******************************
                ' MEvans : 11-02-2003 : Issue - 2144
                ' Should have been logging info silently - not to an error popup
                ' The Empty Array is handled by calling procedure being returned false from
                ' this routine
                ' FIX COPIED FROM ORIGINAL COMPONENT iCLMRiskDetails
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("lPerilID", lPerilID)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Reserve details not found for passed policy/risk", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' call the function to collect the details for the Recovery Type Third Party

            m_lReturn = oBusiness.GetRecoveryDetails(0, r_vRecoveryDetailsArray)

            '   Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-get recovery details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            '   Check for empty strings and replace with zero
            '
            For lCount As Integer = 0 To 2

                If CStr(r_vRecoveryDetailsArray(lCount, 0)) = "" Then

                    r_vRecoveryDetailsArray(lCount, 0) = 0
                End If
            Next lCount

            '   (0,0)=Initial, (1,0)=Revised, (2,0)=Received



            nRecovery = (CDec(r_vRecoveryDetailsArray(0, 0)) + CDec(r_vRecoveryDetailsArray(1, 0))) - CDec(r_vRecoveryDetailsArray(2, 0))

            '   call the function to collect the details for the Salvage Type Third Party

            m_lReturn = oBusiness.GetRecoveryDetails(1, r_vRecoveryDetailsArray)

            '   Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-get salvage details for screen display", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            '   Check for empty strings and replace with zero
            '
            For lCount As Integer = 0 To 2

                If CStr(r_vRecoveryDetailsArray(lCount, 0)) = "" Then

                    r_vRecoveryDetailsArray(lCount, 0) = 0
                End If
            Next lCount

            '   (0,0)=Initial, (1,0)=Revised, (2,0)=Received



            nSalvage = (CDec(r_vRecoveryDetailsArray(0, 0)) + CDec(r_vRecoveryDetailsArray(1, 0))) - CDec(r_vRecoveryDetailsArray(2, 0))
            '
            '   Run through the reserve details and compute out the sum insured and
            '   current reserve
            '

            For lCount As Integer = r_vReserveDetailsArray.GetLowerBound(1) To r_vReserveDetailsArray.GetUpperBound(1)
                For lColumn As Integer = 1 To 6

                    If CStr(r_vReserveDetailsArray(lColumn, lCount)) = "" Then

                        r_vReserveDetailsArray(lColumn, lCount) = 0
                    End If
                Next lColumn

                r_nSumInsured += CDbl(r_vReserveDetailsArray(4, lCount))



                r_nCurrentReserve = r_nCurrentReserve + ((CDec(r_vReserveDetailsArray(1, lCount)) + CDec(r_vReserveDetailsArray(3, lCount))) - CDec(r_vReserveDetailsArray(2, lCount))) - (nSalvage - nRecovery)
            Next lCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReserveDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:        DeletePeril
    '
    ' Description: DeletePeril deletes the peril type. If there are reserves or
    '               payments, recoveries, receipt or peril_party then it will not delete
    '               but give out an error message.
    '
    ' ***************************************************************** '
    Private Sub DeletePeril()
        Try

            Dim bCanDelete As Boolean
            Dim sMessage, sTitle As String
            Dim lClaimPerilId As Integer
            Const AC_EVENT_TYPE_UPDATECLAIM As Integer = 6
            Dim sEventDescription As String = ""

            If lvwPerils.FocusedItem Is Nothing Then
                SetPerilButtons()
                Exit Sub
            End If

            Dim dbNumericTemp As Double
            If Not Double.TryParse(Convert.ToString(lvwPerils.FocusedItem.Tag), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then Exit Sub


            lClaimPerilId = Convert.ToString(lvwPerils.FocusedItem.Tag)

            m_lReturn = m_oBusiness.CheckDeletionForPeril(lClaimPerilId, bCanDelete)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Not bCanDelete Then


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeletePerilTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeletePerilDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)

                'TN20010424 Start
                Exit Sub
                'TN20010424 End
            End If


            m_lReturn = m_oBusiness.DeletePeril(lClaimPerilId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oBusiness.PerilID = lClaimPerilId

            m_oBusiness.Claimid = Claimid
            sEventDescription = Interaction.InputBox("Enter the Event Description", "Event Log", sEventDescription)

            m_lReturn = m_oBusiness.CreateEvent(AC_EVENT_TYPE_UPDATECLAIM, sEventDescription)
            lvwPerils.Items.RemoveAt(lvwPerils.FocusedItem.Index)

            SetPerilButtons()

            Dim vKeyArray(1, 2) As Object


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNamePerilID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lClaimPerilId

            RaiseEvent PerilListChanged(Me, Nothing)

            RaiseEvent DeleteClick(Me, New DeleteClickEventArgs(vKeyArray))

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete peril", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePeril", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    '*******************************************************************
    'Name        : GetRiskDetails
    'Description : GetRiskDetails procedure is used for getting basic details
    '              of a risk
    '********************************************************************
    Private Function GetRiskDetails() As Integer
        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the risk details for the risk pertaining to a policy

            m_lReturn = m_oBusiness.GetRiskDetails(m_lRisk, m_lPolicyId, vResultArray)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (Not Information.IsArray(vResultArray)) Then
                If m_sUnderwritingOrAgency = "A" Then
                    m_lRiskType = m_lRisk

                    m_sRiskDescription = CStr(vResultArray(1, 0))
                Else

                    m_lRiskType = CInt(vResultArray(0, 0))

                    m_sRiskDescription = CStr(vResultArray(1, 0))
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get risk details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    'Private Sub SetListviewWidths()
    'Dim lngNewWidth As Integer
    '
    'If lvwPerils.Columns.Count > 0 Then
    'lngNewWidth = CInt(((VB6.PixelsToTwipsX(lvwPerils.Width) - (2 * ACColWidthMoney)) - 1200) / 2)
    'If lngNewWidth > 0 Then
    'lvwPerils.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(lngNewWidth))
    'lvwPerils.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(lngNewWidth))
    'lvwPerils.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(ACColWidthMoney))
    'lvwPerils.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(ACColWidthMoney))
    'End If
    'End If
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: PopulateTotals
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PopulateTotals(ByVal v_lTotalPerils As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateTotals"

        Dim lReturn As Integer
        Dim lstItem As ListViewItem
        Dim crIncurred, crPaid, crTPRecovery, crSalvageRecovery, crTotalReserve As Decimal



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            For lItem As Integer = 1 To v_lTotalPerils + 1

                lstItem = lvwPerils.Items.Item(lItem - 1)

                crIncurred += ToSafeDouble(ListViewHelper.GetListViewSubItem(lstItem, kSubItemsIncurred).Text)
                crPaid += ToSafeDouble(ListViewHelper.GetListViewSubItem(lstItem, kSubItemsPaid).Text)
                crTPRecovery += ToSafeDouble(ListViewHelper.GetListViewSubItem(lstItem, kSubItemsRecoveries).Text)
                crSalvageRecovery += ToSafeDouble(ListViewHelper.GetListViewSubItem(lstItem, kSubItemsSalvage).Text)
                crTotalReserve += ToSafeDouble(ListViewHelper.GetListViewSubItem(lstItem, kSubItemsCurrentReserve).Text)

            Next

            lstItem = lvwPerils.Items.Add("")

            ListViewHelper.GetListViewSubItem(lstItem, kSubItemsSumInsured).Text = "Total"
            ListViewHelper.GetListViewSubItem(lstItem, kSubItemsIncurred).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crIncurred)
            'ListViewHelper.GetListViewSubItem(lstItem, kSubItemsIncurred).Text = StringsHelper.Format(crIncurred, ACFormatforNumber)
            ListViewHelper.GetListViewSubItem(lstItem, kSubItemsPaid).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crPaid)
            'ListViewHelper.GetListViewSubItem(lstItem, kSubItemsPaid).Text = StringsHelper.Format(crPaid, ACFormatforNumber)
            ListViewHelper.GetListViewSubItem(lstItem, kSubItemsRecoveries).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTPRecovery)
            'ListViewHelper.GetListViewSubItem(lstItem, kSubItemsRecoveries).Text = StringsHelper.Format(crTPRecovery, ACFormatforNumber)
            ListViewHelper.GetListViewSubItem(lstItem, kSubItemsSalvage).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crSalvageRecovery)
            'ListViewHelper.GetListViewSubItem(lstItem, kSubItemsSalvage).Text = StringsHelper.Format(crSalvageRecovery, ACFormatforNumber)
            ListViewHelper.GetListViewSubItem(lstItem, kSubItemsCurrentReserve).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, crTotalReserve)
            'ListViewHelper.GetListViewSubItem(lstItem, kSubItemsCurrentReserve).Text = StringsHelper.Format(crTotalReserve, ACFormatforNumber)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CheckIsLegacyClaim
    '
    ' Parameters: n/a
    '
    ' Description: Confirm if the claim is a legacy one. Legacy claims
    '               are claims raised against generic claims.
    '
    ' History:
    '           Created : MEvans : 08-07-2005 : PN22223
    ' ***************************************************************** '
    Private Function CheckIsLegacyClaim(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIsLegacyClaim"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lClaimId, lOriginalClaimId As Integer
        Dim vGisPolicyLink As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            lReturn = m_oBusiness.GetOriginalClaimID(v_lClaimId:=v_lClaimId, r_lOriginalClaimID:=lOriginalClaimId)

            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                m_bLegacyClaim = False
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetOriginalClaimId", gPMConstants.PMELogLevel.PMLogError)
            End If

            lClaimId = v_lClaimId


            lReturn = m_oBusiness.GetGisPolicyLinkDetails(v_lClaimId:=lClaimId, r_vResults:=vGisPolicyLink)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetGisPolicyLinkDetails", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bLegacyClaim = Not Information.IsArray(vGisPolicyLink)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function
    ''' <summary>
    ''' Holds the flag for IsPaymentsReadOnly configured in product maintenanc.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIsPaymentsReadOnly() As Boolean
        Dim nProductID As Integer
        Dim oProduct As Object = Nothing
        Dim oIsPaymentsReadonly(,) As Object = Nothing

        If Policy = 0 Then
            Return False
        End If

        m_lReturn = g_oObjectManager.GetInstance(oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get Product Business  object", ACApp, ACClass, "GetIsPaymentsReadOnly", Information.Err().Number, Information.Err().Description)
            Return False
        End If

        m_lReturn = oProduct.GetProductid(ifilecnt:=Policy, vProduct_id:=nProductID)
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oProduct.GetProductValue(nProductID, "is_Payments_read_only", oIsPaymentsReadonly)
        End If

        If IsArray(oIsPaymentsReadonly) AndAlso ToSafeString(oIsPaymentsReadonly(0, 0)) = "1" Then
            Return True
        Else
            Return False
        End If


        oProduct.dispose()
        oProduct = Nothing
        oIsPaymentsReadonly = Nothing

    End Function

End Class
