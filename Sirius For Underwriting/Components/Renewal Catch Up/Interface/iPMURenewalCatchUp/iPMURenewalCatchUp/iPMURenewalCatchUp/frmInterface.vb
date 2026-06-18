Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Imports bSIRRenSelection
Imports bSIRRenewalProcess
Imports bSIRGetDocument
Public Class frmInterface


    Private coverStartDT As DateTime
    Private coverEndDT As DateTime
    Private m_oDocTemplate As Object
    Private Const ACClass As String = "frmInterface"
    Private Const ACSilentRenewal As Integer = 1
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_sGenerateReport As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    ' Declare an instance of the general interface object.
    'Private m_oGeneral As iPMURenSelection.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As bSIRRenSelection.Business
    Private m_oInsuranceFile As Object
    'RKS PN13438
    Private m_oInsuranceFileBusiness As Object
    'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy numbering (5.3.2)
    Private m_oPolicyNumMaint As Object
    'End(Saurabh Agrawal) Tech Spec VAL P14 Policy numbering (5.3.2)

    Private m_oReinsurance As Object
    Private m_oTax As Object
    Private m_oRiskData As Object
    Private m_oPerilAllocation As Object
    Private m_oAgentCommission As Object
    Private m_oChangePolicyStatus As Object
    Private m_bManualRenewalsExist As Boolean
    Private m_bAutoRenewalsExist As Boolean
    ' Declare an instance of the FormControl object
    'Private m_oFormFields As iPMFormControl.FormFields
    ' Control array to store the first and last
    ' text box controls for each tab.
    'Private m_ctlTabFirstLast( ,  ) As Control
    Private m_ctlTabFirstLast(,) As Control
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    Private m_vProductID As Integer
    Private m_bRoundOff As Boolean = False
    'Report
    Private m_iPrintMode As Integer
    Private m_lRenewalMode As Integer
    Private m_lInsFolderCnt As Integer 'RWH(15/05/01)
    Private m_bCalledFromLocalForm As Boolean
    'Thinh Nguyen 19/03/2002 (start)
    Private m_vSourceID As Integer
    'Thinh Nguyen 19/03/2002 (end)
    Private m_bExtraRiskDetails As Boolean 'SET (10/6/02)
    'RKS PN13438
    Private m_bUnderwritingYearID As Boolean 'Is option switched on/off

    Private m_lPolicyVersionIncrement As Integer
    Private m_sInsuranceRef As String = ""

    'default renewal lock name
    Private Const ACLockName As String = "RenewalProcess"

    Private m_obSIRRenewal As Object
    Private bolActivated As Boolean
    Private bIsValid As Boolean
    Private oBatchRenewalBusiness As bSIRRenewalBusiness
    Private g_oBusiness As bSIRRenewalProcess.Business
    Private i_oBusiness As dSIRInsuranceFile.SIRInsuranceFile

    Private m_vRenewalPolicy(,) As Object
    Private p_vRenewalPolicy(,) As Object
    Dim m_lPrepayment As Object
    Private m_lCount As Integer
    Private m_Anniversary As Boolean
    Private m_crRoundOffAmount As Decimal = 0

    Private obSIRPremiumFinance As bSIRPremiumFinance.Business

    'Public m_ofrmLapseRenewal As frmLapseRenewal

    'Public m_ofrmChangeStatus As frmChangeStatus

    'Public m_ofrmChangePolicyDetails As frmChangePolicyDetails
    'Private Const ACClass As String = "frmRenewalProcess"

    'system option number for generating report in the renewal process
    Private Const ACGenerateRenewalStatusReport As Integer = 1012
    Private Const ACGenerateRenewalAgentList As Integer = 1013
    Private Const ACRenSchedulePrinting As Integer = 1036
    Private Const ACRenCertificatePrinting As Integer = 1037
    Private Const ACRenDebitNotePrinting As Integer = 1038
    Private Const ACCreditControlEnabled As Integer = 5001

    'default renewal lock name
    'Private Const ACLockName As String = "RenewalProcess"

    Private Const ACIconManual As Integer = 1
    Private Const ACIconAccept As Integer = 2
    Private Const ACIconInvite As Integer = 3
    Private Const ACIconWrite As Integer = 4

    'Private m_sCallingAppName As String = ""
    'Private m_lErrorNumber As Integer
    'Private m_lReturn As Integer
    'Private m_lStatus As gPMConstants.PMEReturnCode

    'Private m_vRenewalPolicy(,) As Object  'list of policies which are in renewal and match selected criteria
    Private m_lFormActivate As gPMConstants.PMEReturnCode

    'filter variables
    'Private m_sInsuranceRef As String = ""
    Private m_dRenewalDate As Date
    Private m_lProductID As Integer
    Private m_lBranchID As Integer
    Private m_lRenewalType As Integer  'Acceptance, Amendment, Invite or All
    Private m_lLeadAgentCnt As Integer
    Private m_lAgentcode As Integer
    'store system option value
    'Private m_sGenerateReport As String = ""
    Private m_sGenerateAgentList As String = ""  'use when do renewal invite

    Private m_oReportPrint As Object
    Private m_oFindDocTemplate As Object
    'Private m_oDocTemplate As Object

    'Private m_lRenewalMode As Integer
    Private schemeNumber As Integer
    Private m_bCanTransferBroker As Boolean

    Private m_sRenSchedulePrinting As String = ""  'option number 1036
    Private m_sRenCertificatePrinting As String = ""  'option number 1037
    Private m_sRenDebitNotePrinting As String = ""  'option number 1038

    ' Last size variables for screen resizing
    Private m_lWidth As Integer
    Private m_lHeight As Integer

    Private m_policyRef As String
    Private m_lPaymentAccountID As Integer
    Private m_iDebitAgainst As Integer
    Private m_vCreditTransactions As Object
    Private m_lCashListID As Integer
    Private m_lCashListItemID As Integer
    Private m_lTransactionID As Integer
    Private m_cTransactionAmount As Decimal
    Private m_lWrittenUsed As Long
    ' Tech  Written Status.doc
    Private m_bIsAmendedPolicyWritten As Boolean
    Dim m_iPolicyMakeLiveStatus As Integer
    'Dim m_lPrepayment As Object
    'Private m_lCount As Integer

    'Private m_crRoundOffAmount As Decimal = 0
    'Private m_bRoundOff As Boolean = False
    Private reComplete As Boolean
    Private message As String = ""

    Private Property isWorkMngrTaskCreated As Boolean = False

    Public Property CoverStart() As DateTime
        Get
            Return coverStartDT
        End Get
        Set(value As DateTime)
            coverStartDT = value
        End Set
    End Property

    Public Property CoverEnd() As DateTime
        Get
            Return coverEndDT
        End Get
        Set(value As DateTime)
            coverEndDT = value
        End Set
    End Property
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public Property SchemeNo() As Integer
        Get
            Return schemeNumber
        End Get
        Set(ByVal Value As Integer)
            schemeNumber = Value
        End Set
    End Property


    Public Property RenComplete() As Boolean
        Get
            Return reComplete
        End Get
        Set(ByVal Value As Boolean)
            reComplete = Value
        End Set
    End Property
    Public Property PolicyRef() As String
        Get
            Return m_policyRef
        End Get
        Set(ByVal value As String)
            m_policyRef = value
        End Set
    End Property

    Public Property RenewalDate() As Date
        Get
            Return m_dRenewalDate
        End Get
        Set(ByVal Value As Date)
            m_dRenewalDate = Value
        End Set
    End Property

    Public Property ProductID() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property

    Public Property BranchID() As Integer
        Get
            Return m_lBranchID
        End Get
        Set(ByVal Value As Integer)
            m_lBranchID = Value
        End Set
    End Property

    Public Property RenewalType() As Integer
        Get
            Return m_lRenewalType
        End Get
        Set(ByVal Value As Integer)
            m_lRenewalType = Value
        End Set
    End Property

    Public WriteOnly Property RenewalMode() As Integer
        Set(ByVal Value As Integer)
            m_lRenewalMode = Value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return m_sInsuranceRef
        End Get
        Set(ByVal Value As String)
            m_sInsuranceRef = Value
        End Set
    End Property
    Private Sub frmInterface_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblVersions.Visible = False
        Const kMethodName As String = "frmInterface_Load"
        lblSuccessfullyRenewed.Visible = False
        prgbVersion.Visible = False
        lvwVersionDetails.Visible = False
        btnSuccessfullyRenewedOk.Visible = False

    End Sub


    Private Sub Form_Initialize_Renamed()

        Const kmethodname As String = "form_initialize"

        Dim lreturn As gPMConstants.PMEReturnCode
        Dim lsubvalue As Integer

        Try

            Dim temp_m_orenselection As Object
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            lreturn = MainModule.g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)
            If lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kmethodname, "Get Instance of bObjectManager.ObjectManager Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            lreturn = MainModule.g_oObjectManager.GetInstance(temp_m_orenselection, "bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_orenselection
            If lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kmethodname, "failed to get instance of bsirrenselection.business", gPMConstants.PMELogLevel.PMLogError)

            End If
            Dim temp_m_orenprocess As Object
            lreturn = MainModule.g_oObjectManager.GetInstance(temp_m_orenprocess, "bSIRRenewalProcess.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            g_oBusiness = temp_m_orenprocess
            If lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kmethodname, "failed to get instance of bSIRRenewalProcess.Business", gPMConstants.PMELogLevel.PMLogError)

            End If

            Dim temp_m_ifile As Object
            lreturn = MainModule.g_oObjectManager.GetInstance(temp_m_ifile, "dSIRInsuranceFile.SIRInsuranceFile", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            i_oBusiness = temp_m_ifile
            If lreturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kmethodname, "failed to get instance of dSIRInsuranceFile.SIRInsuranceFile", gPMConstants.PMELogLevel.PMLogError)

            End If
            Dim tempbSIRPremiumFinance As Object
            lreturn = MainModule.g_oObjectManager.GetInstance(tempbSIRPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            obSIRPremiumFinance = tempbSIRPremiumFinance

        Catch ex As Exception

            ' do not call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kmethodname, r_lFunctionReturn:=lsubvalue, excep:=ex)

            ' if you want to rollback a transaction or something, do it here

        Finally

            ' set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub
    Private Sub btnRenewNo_Click(sender As Object, e As EventArgs) Handles btnRenewNo.Click
        Me.Close()
    End Sub

    Private Sub btnRenewYes_Click(sender As Object, e As EventArgs) Handles btnRenewYes.Click

        Dim sLockedBy As String = ""
        Dim bProductAutoRenew As Boolean
        Dim lInsurancefolderCnt As Long
        Dim vArray(,) As Object
        Dim sPreviousStatus As String = ""
        Dim sCurrentStatus As String = ""

        lblVersions.Visible = True
        prgbVersion.Visible = True
        lvwVersionDetails.Visible = True
        'Dim lReturn As gPMConstants.PMEReturnCode
        'Dim temp_m_oRenSelection As Object

        'lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oRenSelection, "bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        'm_oBusiness = temp_m_oRenSelection
        'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    gPMFunctions.RaiseError("kMethodName", "Failed to get instance of bSirRenSelection.business", gPMConstants.PMELogLevel.PMLogError)

        'End If

        m_lReturn = m_oBusiness.GetInsFolderFromInsRef(v_sInsuranceRef:=Trim(m_policyRef),
                                                                    r_vResultArray:=vArray)

        If IsArray(vArray) Then
            lInsurancefolderCnt = ToSafeLong(vArray(0, 0))
        Else
            lInsurancefolderCnt = 0
        End If
        If lInsurancefolderCnt = 0 Then
            'MsgBox("No data found for current criteria", vbOKOnly, "Renewal " &
            '        "Selection")
            'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'm_lStatus = gPMConstants.PMEReturnCode.PMCancel
            'Exit Sub
        Else
            m_lInsFolderCnt = lInsurancefolderCnt
        End If
        If LockPolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        Dim coverStartDate As DateTime = coverStartDT
        Dim coverEndDate As DateTime = coverEndDT
        Dim i As Integer
        'i = DateDiff(DateInterval.Month, coverStartDT, Date.Today)
        Dim arrLVItem(0) As ListViewItem
        Dim j As Integer
        j = 0
        Dim isUnderRen As Boolean = False

        'coverStartDate = DateTime.Now.AddDays(-1)
        While coverStartDate <= DateTime.Today

            If coverEndDate < DateTime.Today AndAlso j < 11 Then
                prgbVersion.Value = 0
                'm_bCalledFromLocalForm = True
                m_lReturn = SilentRenewal(m_lInsFolderCnt, bProductAutoRenew)
                If message <> "" Then
                    MessageBox.Show(message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                message = ""
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'StatusBar1.Items.Item("Message").Text = " Ready"
                    'Me.StatusBar1.Refresh()
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    UnLockPolicy()
                    Exit Sub
                End If
                UnLockPolicy()



                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso Not isUnderRen Then
                    '***************Call fo renewal*****************
                    RenewalProcess()

                    Dim nResult As Integer = 0
                    Dim oResultArray(,) As Object = Nothing
                    Dim crGrossTotal As Decimal = 0

                    nResult = g_oBusiness.GetPolicyGrossTotal(v_lInsuranceFileCnt:=p_vRenewalPolicy(PMFieldPosInsuranceFileCnt, 0),
                                                         r_vResults:=oResultArray)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessAccept", "Failed to Get GetPolicyGrossTotal", gPMConstants.PMELogLevel.PMLogError)

                    End If
                    If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                        crGrossTotal = crGrossTotal + gPMFunctions.ToSafeDecimal(oResultArray(4, 0), 0)
                    End If
                    If prgbVersion.Value = 100 Then


                        i_oBusiness.InsuranceFileCnt = p_vRenewalPolicy(PMFieldPosInsuranceFileCnt, 0)
                        i_oBusiness.SelectSingle()

                        arrLVItem(0) = New ListViewItem
                        arrLVItem(0).SubItems(0).Text = i_oBusiness.CoverStartDate
                        arrLVItem(0).SubItems.Add(i_oBusiness.RenewalDate)
                        arrLVItem(0).SubItems.Add(IIf((i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "DIRECT DEBIT" Or i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "PREMIUM FINANCE"), "Instalments", i_oBusiness.PaymentMethod))
                        arrLVItem(0).SubItems.Add(crGrossTotal)
                        arrLVItem(0).SubItems.Add("Current")
                        arrLVItem(0).ImageIndex = j
                        lvwVersionDetails.Items.AddRange(arrLVItem)

                        For index As Integer = 0 To lvwVersionDetails.Items.Count - 1
                            sPreviousStatus = lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems(4).Text
                            sCurrentStatus = lvwVersionDetails.Items.Item(index).SubItems(4).Text
                            If sPreviousStatus <> "" AndAlso sPreviousStatus.ToLower() <> "replaced" AndAlso index > 0 Then
                                lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.RemoveAt(4)
                                lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.Add("Replaced")
                                lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.Insert(4, lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems(4))
                                lvwVersionDetails.Refresh()
                            End If
                        Next
                        lvwVersionDetails.Refresh()
                        j = j + 1
                    Else
                        i_oBusiness.InsuranceFileCnt = p_vRenewalPolicy(PMFieldPosInsuranceFileCnt, 0)
                        i_oBusiness.SelectSingle()

                        arrLVItem(0) = New ListViewItem
                        arrLVItem(0).SubItems(0).Text = i_oBusiness.CoverStartDate
                        arrLVItem(0).SubItems.Add(i_oBusiness.RenewalDate)
                        arrLVItem(0).SubItems.Add(IIf((i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "DIRECT DEBIT" Or i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "PREMIUM FINANCE"), "Instalments", i_oBusiness.PaymentMethod))
                        arrLVItem(0).SubItems.Add(crGrossTotal)
                        arrLVItem(0).SubItems.Add("Current")
                        arrLVItem(0).ImageIndex = j
                        lvwVersionDetails.Items.AddRange(arrLVItem)
                        For index As Integer = 0 To lvwVersionDetails.Items.Count - 1
                            sPreviousStatus = lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems(4).Text
                            sCurrentStatus = lvwVersionDetails.Items.Item(index).SubItems(4).Text
                            If sPreviousStatus <> "" AndAlso sPreviousStatus.ToLower() <> "replaced" AndAlso index > 0 Then
                                lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.RemoveAt(4)
                                lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.Add("Replaced")
                                lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.Insert(4, lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems(4))
                                lvwVersionDetails.Refresh()
                            End If
                        Next
                        lvwVersionDetails.Refresh()
                        j = j + 1
                        isUnderRen = True
                        'MessageBox.Show("Policy auto renewal failed. Please, renew the next versions manually.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'lblSuccessfullyRenewed.Text = "The automatic renewal process has failed, Please renew policy manually."
                    End If
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso isUnderRen Then
                    i_oBusiness.InsuranceFileCnt = p_vRenewalPolicy(PMFieldPosInsuranceFileCnt, 0)
                    Dim v_latestInsurance(,) As Object = Nothing
                    m_oBusiness.GetRenewalVersion(m_lInsFolderCnt, v_latestInsurance)

                    i_oBusiness.SelectSingle()

                    Dim nResult As Integer = 0
                    Dim oResultArray(,) As Object = Nothing
                    Dim crGrossTotal As Decimal = 0

                    nResult = g_oBusiness.GetPolicyGrossTotal(v_lInsuranceFileCnt:=v_latestInsurance(0, 0),
                                                         r_vResults:=oResultArray)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessAccept", "Failed to Get GetPolicyGrossTotal", gPMConstants.PMELogLevel.PMLogError)

                    End If
                    If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                        crGrossTotal = crGrossTotal + gPMFunctions.ToSafeDecimal(oResultArray(4, 0), 0)
                    End If

                    arrLVItem(0) = New ListViewItem
                    arrLVItem(0).SubItems(0).Text = coverStartDate
                    Dim MyDate1 As Date = coverStartDate.AddMonths(1)
                    RenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                    arrLVItem(0).SubItems.Add(RenewalDate)
                    arrLVItem(0).SubItems.Add(IIf((i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "DIRECT DEBIT" Or i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "PREMIUM FINANCE"), "Instalments", i_oBusiness.PaymentMethod))
                    arrLVItem(0).SubItems.Add(crGrossTotal)
                    arrLVItem(0).SubItems.Add("Under Renewal")
                    arrLVItem(0).ImageIndex = j
                    lvwVersionDetails.Items.AddRange(arrLVItem)
                    lvwVersionDetails.Refresh()
                    j = j + 1
                    isUnderRen = True
                    'MessageBox.Show("Policy auto renewal failed. Please, renew the next versions manually.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If Not m_Anniversary Then
                        lblSuccessfullyRenewed.Text = "The automatic renewal process has failed, Please renew policy manually."
                    End If
                    Exit While
                End If

            Else
                i_oBusiness.InsuranceFileCnt = p_vRenewalPolicy(PMFieldPosInsuranceFileCnt, 0)
                i_oBusiness.SelectSingle()
                Dim nResult As Integer = 0
                Dim oResultArray(,) As Object = Nothing
                Dim crGrossTotal As Decimal = 0

                nResult = g_oBusiness.GetPolicyGrossTotal(v_lInsuranceFileCnt:=p_vRenewalPolicy(PMFieldPosInsuranceFileCnt, 0),
                                                         r_vResults:=oResultArray)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessAccept", "Failed to Get GetPolicyGrossTotal", gPMConstants.PMELogLevel.PMLogError)

                End If
                If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                    crGrossTotal = crGrossTotal + gPMFunctions.ToSafeDecimal(oResultArray(4, 0), 0)
                End If

                arrLVItem(0) = New ListViewItem
                arrLVItem(0).SubItems(0).Text = coverStartDate
                Dim MyDate1 As Date = coverStartDate.AddMonths(1)
                RenewalDate = DateSerial(MyDate1.Year, MyDate1.Month, 1)
                arrLVItem(0).SubItems.Add(RenewalDate)
                arrLVItem(0).SubItems.Add(IIf((i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "DIRECT DEBIT" Or i_oBusiness.PaymentMethod.ToString.ToUpper.Trim() = "PREMIUM FINANCE"), "Instalments", i_oBusiness.PaymentMethod))
                arrLVItem(0).SubItems.Add(crGrossTotal)
                arrLVItem(0).SubItems.Add("Current")
                arrLVItem(0).ImageIndex = j
                'lblSuccessfullyRenewed.Visible = True
                'btnSuccessfullyRenewedOk.Visible = True
                lvwVersionDetails.Items.AddRange(arrLVItem)

                Dim IsProcessTerminate As Boolean = False
                For index As Integer = 0 To lvwVersionDetails.Items.Count - 1
                    sPreviousStatus = lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems(4).Text
                    sCurrentStatus = lvwVersionDetails.Items.Item(index).SubItems(4).Text
                    If sPreviousStatus <> "" AndAlso sPreviousStatus.ToLower() <> "replaced" Then
                        lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.RemoveAt(4)
                        lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.Add("Replaced")
                        lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems.Insert(4, lvwVersionDetails.Items.Item(If(index = 0, index, index - 1)).SubItems(4))
                        lvwVersionDetails.Refresh()
                    ElseIf j + 1 = 13 AndAlso j + 1 > 12 Then
                        lvwVersionDetails.Items.Item(11).SubItems.RemoveAt(4)
                        lvwVersionDetails.Items.Item(11).SubItems.Add("Current")
                        lvwVersionDetails.Items.Item(11).SubItems.Insert(4, lvwVersionDetails.Items.Item(12).SubItems(4))
                        lvwVersionDetails.Items.Item(12).SubItems.RemoveAt(4)
                        lvwVersionDetails.Items.Item(12).SubItems.Add("Renewal Quote")
                        lvwVersionDetails.Items.Item(12).SubItems.Insert(4, lvwVersionDetails.Items.Item(12).SubItems(4))
                        lvwVersionDetails.Refresh()
                        IsProcessTerminate = True
                    End If
                Next
                lvwVersionDetails.Refresh()
                j = j + 1
                If IsProcessTerminate Then
                    Exit While
                End If

            End If

            Dim MyDate As Date = coverStartDate.AddMonths(1)
            coverEndDate = New Date(MyDate.Year, MyDate.Month, Date.DaysInMonth(MyDate.Year, MyDate.Month))
            coverStartDate = DateSerial(MyDate.Year, MyDate.Month, 1)
        End While
        lblSuccessfullyRenewed.Visible = True
        btnSuccessfullyRenewedOk.Visible = True
        btnRenewYes.Enabled = False
        btnRenewNo.Enabled = False
    End Sub


    Public Sub RenewalProcess()
        Dim sNewPolicyRef As String = ""
        Dim dNewStartDate, dNewEndDate As Date
        Dim bChanged As Boolean
        Dim bContinue As Boolean
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim sLockedBy As String = ""
        Dim bLocked As Boolean
        Dim lIndex, lListCount As Integer

        Dim sFailureMessage, sOptionValue As String
        Dim sReportText As New StringBuilder

        Dim sRenStatusDesc As String = ""
        Dim lRenStatusTypeID As Integer

        Dim sMsgBox As String = ""
        Dim lYesNo As DialogResult
        Dim lNumberTick, lInvalidTMPCount As Integer
        'Start - Prakash - WPR85_Paralleling
        Dim bIsCashDeposit, bIsCashDepositCancel As Boolean
        'End - Prakash - WPR85_Paralleling
        Try
            m_lCount = 0
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ''disable menu
            'mnuRenewalProcess.Enabled = False

            ''check to see if we have any ticked
            'm_lReturn = ListViewIsTick(lvwRenewalProcess, lNumberTick)

            'Select Case m_lReturn
            '    Case gPMConstants.PMEReturnCode.PMError
            '        MessageBox.Show("Warning! An error has occurred whilst trying to check for selected items in list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        Exit Sub
            '    Case gPMConstants.PMEReturnCode.PMFalse
            '        MessageBox.Show("Warning! Please select an item from the list", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            '        Exit Sub
            'End Select
            m_sInsuranceRef = p_vRenewalPolicy(PMFieldPosInsuranceRef, 0)
            m_dRenewalDate = DateTime.Now
            'm_dRenewalDate = p_vRenewalPolicy(PMFieldPosRenewalDate, 0)
            'm_lProductID = p_vRenewalPolicy(PMFieldPosProductID, 0)
            ''m_lBranchID = p_vRenewalPolicy()
            ''m_lRenewalType = p_vRenewalPolicy()
            ''m_lLeadAgentCnt = p_vRenewalPolicy()
            ''m_lAgentcode = p_vRenewalPolicy()

            m_lReturn = GetBusiness(m_vRenewalPolicy, 0, m_sInsuranceRef, m_dRenewalDate, 0, 0, 0, 0, 0) = gPMConstants.PMEReturnCode.PMTrue
            'Start - Prakash - WPR85_Paralleling
            m_lReturn = CheckForCashDepositPaymentMethod(bIsCashDeposit)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If bIsCashDeposit Then
                    MessageBox.Show("Batch renewal is not supported for Cash Deposit." &
                                    " Choose Another Batch.", "Renewals", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            Else
                MessageBox.Show("Failed to  check for CashDeposit PaymentMethod", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            'End - Prakash - WPR85_Paralleling

            'stbMain.Items.Item("Message").Text = "Processing renewal acceptance please wait..."
            'Me.stbMain.Refresh()
            'lListCount = lvwRenewalProcess.Items.Count
            'step backwards so we can remove processed items from list
            prgbVersion.Value = 55

            'Start - Prakash - WPR85_Paralleling
            bIsCashDepositCancel = False
            'End - Prakash - WPR85_Paralleling
            bLocked = False
            bContinue = False
            sMsgBox = ""
            lYesNo = System.Windows.Forms.DialogResult.No
            m_lReturn = CheckRenewalStatus(0, "ACCEPT", sMsgBox, lYesNo, lNumberTick)

            'get array position
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("CheckRenewalStatus() error", Me.Text, MessageBoxButtons.OK)

            Else
                If sMsgBox <> "" Then
                    If lYesNo = System.Windows.Forms.DialogResult.Yes Then
                        If MessageBox.Show(sMsgBox, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                            Exit Sub
                        End If
                    Else
                        message = sMsgBox
                    End If
                Else
                    bContinue = True
                End If
            End If

            'm_lCount = lCount
            'check to see if current renewal status is ok for acceptance



            prgbVersion.Value = 65
            If bContinue Then
                'lock this renewal status count to stop others from processing it

                m_lReturn = g_oBusiness.LockKey(v_sKeyName:=ACLockName, v_lKeyValue:=m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex), v_lUserID:=g_oObjectManager.UserID, r_sLockedBy:=sLockedBy)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    bLocked = True
                    If g_oBusiness.IsQuoted(v_lInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), r_lResult:=lIsQuoted) = gPMConstants.PMEReturnCode.PMTrue Then
                        If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = ProcessAccept(v_vPolicy:=m_vRenewalPolicy, v_lIndex:=lIndex, v_bPolicyChanged:=bChanged, v_sNewPolicyNumber:=sNewPolicyRef, v_dNewStartDate:=dNewStartDate, v_dNewEndDate:=dNewEndDate, r_sFailureMessage:=sFailureMessage)

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                'did we fail in producing any of the document or creating work task
                                sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Successful" & Strings.Chr(13) & Strings.Chr(10) & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                            Else
                                'Start - Prakash - WPR85_Paralleling
                                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then bIsCashDepositCancel = True
                                'End - Prakash - WPR85_Paralleling
                                sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - " & sFailureMessage & Strings.Chr(13) & Strings.Chr(10))
                                If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalIsTrueMonthlyPolicy, lIndex))) = 1 Then
                                    lInvalidTMPCount += 1
                                End If
                            End If
                        Else
                            sReportText.Append(Strings.Chr(13) & Strings.Chr(10) & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, lIndex)) & " - Unquoted" & Strings.Chr(13) & Strings.Chr(10))

                            m_lReturn = GetRenewalStatusType(v_sRenStatusCode:="ManReview", r_sDesc:=sRenStatusDesc, r_lRenewalStatusTypeID:=lRenStatusTypeID)

                            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                'reset status to awaiting manual review

                                If g_oBusiness.SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, lIndex)), v_lRenewalStatusTypeID:=lRenStatusTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    If MessageBox.Show("Failed to set renewal status to awaiting manual review." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue ?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                                        Exit Sub
                                    End If
                                End If
                            Else
                                If MessageBox.Show("Failed to get renewal status type details." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue ?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    Else
                        If MessageBox.Show("Failed to check quote status." & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue ?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                            Exit Sub
                        End If
                    End If
                    'Start - Prakash - WPR85_Paralleling
                    If Not bIsCashDepositCancel Then
                        'mark this as deleted from listview
                        m_vRenewalPolicy(ACRenewalDeleteFromListView, lIndex) = "1"

                        'remove from list to stop user from selecting this again
                        'lvwRenewalProcess.Items.RemoveAt(lCount - 1)
                    End If
                    'End - Prakash - WPR85_Paralleling

                    'unlock renewal policy

                    If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                    bLocked = False 'so we won't try to unlock it later

                End If 'ChangePolicyDetail
            Else
                If sLockedBy = "ERROR" Then
                    If MessageBox.Show("Failed to lock policy for, renewal status count : " & CStr(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Do you want to process next selected policy?", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                        Exit Sub
                    End If
                Else

                    Exit Sub

                End If
            End If 'LockKey()


            prgbVersion.Value = 80

            If lInvalidTMPCount > 0 Then
                MessageBox.Show(CStr(lInvalidTMPCount) & " anniversary renewal/s could not be processed." &
                                " Anniversary Renewals cannot be accepted until " &
                                " the last monthly cycle has been accepted", "True Monthly Policy Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            If sReportText.ToString() <> "" Then
                If m_sGenerateReport = "1" Then
                    If RenewalReport(v_sReportTitle:="Renewal Acceptance", v_sReportText:=sReportText.ToString()) <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to do Renewal report", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If

            prgbVersion.Value = 90
            'lvwRenewalProcess.Refresh()
            'lvwRenewalProcess.FullRowSelect = True
            prgbVersion.Value = 100
        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process renewal acceptance", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRenewalProcessAccept_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            'unlock current policy
            If bLocked Then

                If g_oBusiness.UnLockKey(v_sKeyName:=ACLockName, v_lKeyValue:=CInt(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)), v_lUserID:=g_oObjectManager.UserID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to unlock KeyName: " & ACLockName & Strings.Chr(13) & Strings.Chr(10) & "KeyValue: " & CStr(m_vRenewalPolicy(ACIRenewalStatusCnt, lIndex)) & Strings.Chr(13) & Strings.Chr(10) & "Process terminate.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

            'lvwVersionDetails.Items.Add(m_vRenewalPolicy(ACIRenewalCoverStartDate, 0), 0)

            'lvwRenewalProcess.Refresh()
            'stbMain.Items.Item("Message").Text = "Ready"
            'Me.stbMain.Refresh()

            'DisplayListViewCount()

            'mnuRenewalProcess.Enabled = True

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Function CheckRenewalStatus(ByVal v_lIndex As Integer, ByVal v_sStatusType As String, ByRef r_sMessage As String, ByRef r_lYesNo As Integer, Optional ByVal v_lNumberTick As Integer = 0) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            r_lYesNo = System.Windows.Forms.DialogResult.No
            r_sMessage = ""

            If Not Information.IsArray(m_vRenewalPolicy) Then
                r_sMessage = "Policy list is empty"
                Return result
            End If

            If v_sStatusType.ToUpper() <> "TRANSFER" Then
                If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)), 0) = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    r_sMessage = "Agent/Broker for this policy is in transfer mode." & Strings.Chr(13) & Strings.Chr(10) & "Please contact the System Administrator."
                Else
                    Select Case v_sStatusType.ToUpper()
                        Case "AMEND"
                            'Start (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                            ' Removing the Restrictions here to let the policies in PMBRenewalStatusTypeAwaitUpdate to proceed for Amendment
                            If CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypePolicyChanged Then
                                'Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitUpdate Then

                                r_sMessage = "Renewal status of selected policy is set for acceptance."
                            End If
                            'End (Prakash Varghese) - (Tech Spec - TRAC 4755 Policy Renewal Status.docx) - (6.1.1.1)
                        Case "ACCEPT"
                            'First check is this policy belongs to a Closed Branch - if it
                            'does then stop the accept and advise the user to amend this first
                            If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalIsBranchDeleted, v_lIndex)), 0) = 1 Then
                                r_sMessage = "Unable to proceed - this Policy is attached to a branch that is closed. Please Amend " & CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, v_lIndex))
                                'Here check that the status type isn't a failure type.
                                'PN74111 : Priya
                            ElseIf CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeAwaitManualRating Or CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeAutoRatedFailed Or CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeManualReview Or CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) = gPMConstants.PMBRenewalStatusTypeAutoRated Then

                                r_sMessage = "Renewal status of selected policy is not set for acceptance."
                                'PN62891 - Only able to accept policies via the 'Renewal Acceptance without Amendment' task that have a status of 'Awaiting Update'
                            ElseIf m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) <> PMBRenewalStatusTypeAwaitUpdate _
                            And m_lRenewalMode = ACRenModeRA Then
                                r_sMessage = "Renewal status of selected policy is not set for acceptance."

                            End If
                            'Start  Written Status
                        Case "WRITE"
                            ' Check if the product allows write status
                            m_lReturn = g_oBusiness.IsWrittenUsed(ToSafeInteger(m_vRenewalPolicy(ACIRenewalProductId, v_lIndex)))
                            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                                'Sankar - PN 71391
                                r_sMessage = "Warning: Product Risk Maintenance option not set for renewal record(s) selected. " &
                                                 "Please re-select"
                            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                If ToSafeLong(m_vRenewalPolicy(ACIRenewalIsBranchDeleted, v_lIndex), 0) = 1 Then
                                    r_sMessage = "Unable to proceed - this Policy is attached to a branch that is closed. Please Amend " _
                                                & m_vRenewalPolicy(ACIRenewalInsuranceRef, v_lIndex)
                                ElseIf m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitManualRating _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAutoRatedFailed _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeManualReview _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAwaitBrokerTransfer _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypeAutoRated _
                                    Or m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex) = PMBRenewalStatusTypePolicyChanged Then
                                    'Sankar - PN 71392
                                    r_sMessage = "Written Status is only available for renewal records with a status of " & """Awaiting Update"""
                                End If
                            Else
                                RaiseError("CheckRenewalStatus", "IsWrittenUsed Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            'End  Written Status
                        Case "INVITE"
                            If CDbl(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)) <> gPMConstants.PMBRenewalStatusTypeAutoRated Then
                                r_sMessage = "Renewal status of policy (" & Strings.Chr(13) & Strings.Chr(10) &
                                             CStr(m_vRenewalPolicy(ACIRenewalInsuranceRef, v_lIndex)) & ") " & Strings.Chr(13) & Strings.Chr(10) &
                                             " is not set for notice print."
                            End If
                    End Select
                End If
            Else
                If gPMFunctions.ToSafeLong(CStr(m_vRenewalPolicy(ACIRenewalStatusTypeId, v_lIndex)), 0) <> gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer Then
                    r_sMessage = "Renewal status on selected policy is not set to transfer."
                End If
            End If

            If r_sMessage <> "" Then
                If v_lNumberTick > 1 Then
                    r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you want to continue processing next policy?"
                    r_lYesNo = System.Windows.Forms.DialogResult.Yes
                End If
            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check renewal status", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewalStatus()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    Private Function GetBusiness(ByRef r_vResultArray(,) As Object, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByVal v_dRenewalDate As Date, ByVal v_lProductID As Integer, ByVal v_lBranchID As Integer, ByVal v_lRenewalType As Integer, ByVal v_lLeadAGentCnt As Integer, Optional ByVal v_lAgentCode As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            'stbMain.Items.Item("Message").Text = "Selecting renewal please wait..."
            'Me.stbMain.Refresh()

            result = g_oBusiness.GetRenewalPolicy(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceRef:=v_sInsuranceRef, v_dRenewalDate:=v_dRenewalDate, v_lProductID:=v_lProductID, v_lBranchID:=v_lBranchID, v_lRenewalType:=v_lRenewalType, v_lLeadAgentCnt:=v_lLeadAGentCnt, v_lAgentcode:=v_lAgentCode, r_vResult:=r_vResultArray)



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policies in renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    Private Function RenewalReport(ByVal v_sReportTitle As String, ByVal v_sReportText As String, Optional ByVal v_sFileName As String = "", Optional ByVal v_sPath As String = "", Optional ByVal v_bDeleteFile As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'get path from registry if its not passed in
            If v_sPath = "" Then
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=v_sPath)
            End If

            'make sure we have a backslash at the end
            If Not v_sPath.EndsWith("\") Then
                v_sPath = v_sPath & "\"
            End If

            If v_sFileName = "" Then
                v_sFileName = "Renewal_" & g_oObjectManager.UserName & "_" & DateTime.Now.ToString("yyyyMMddHHMMss") & ".log"
            End If

            If AppendText(v_sFile:=v_sPath & v_sFileName, v_sTextLine:=v_sReportTitle & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & v_sReportText) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If SpoolDoc(v_sFileName:=v_sPath & v_sFileName, v_sSpoolDesc:=v_sReportTitle) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'delete original file
            Dim sOptionValueisSharePointOnline As String = ""
            'For SharePoint Online donot delete the file
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSystemOptionIsSharePointOnline, r_sOptionValue:=sOptionValueisSharePointOnline)

            ' RAM20040209 : Bug fix for PN Issue 10231
            '               1. Changed the Dir Command to IsFileExists command
            '               2. Use Delete File Function to delete file
            If sOptionValueisSharePointOnline <> "1" Then
                If v_bDeleteFile Then
                    File.Delete(v_sPath & v_sFileName)
                End If
            End If
        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to spool renewal report", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalReport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally
        End Try
        Return result
    End Function



    Private Function SpoolDoc(ByVal v_sFileName As String, ByVal v_sSpoolDesc As String) As Integer

        Dim result As Integer = 0
        Dim sDocTypeID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Document_Type", v_vReturnColumn:="document_type_id", v_sKeyColumn:="Code", v_sKeyValue:="LETTER", v_iDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=sDocTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oDocTemplate.DocName = v_sFileName

            m_oDocTemplate.SpoolDesc = v_sSpoolDesc

            m_oDocTemplate.DocumentTypeId = CInt(sDocTypeID)

            m_oDocTemplate.Mode = 5 'spool report

            result = m_oDocTemplate.Start()

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SpoolDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SpoolDoc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Private Function AppendText(ByVal v_sFile As String, ByVal v_sTextLine As String, Optional ByVal v_sMode As String = "Output") As Integer

        Dim result As Integer = 0
        Dim lFileNo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get free handle
            lFileNo = FileSystem.FreeFile()

            Select Case v_sMode.ToUpper()
                Case "APPEND"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Append)
                Case "BINARY"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Binary)
                Case "INPUT"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Input)
                Case "OUTPUT"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Output)
                Case "RANDOM"
                    FileSystem.FileOpen(lFileNo, v_sFile, OpenMode.Random)
            End Select

            FileSystem.PrintLine(lFileNo, v_sTextLine)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write to " & v_sFile, vApp:=ACApp, vClass:=ACClass, vMethod:="AppendText", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            FileSystem.FileClose(lFileNo)

        End Try
        Return result
    End Function
    '*********************************************************************************
    ' Get renewal status type details
    '*********************************************************************************
    Private Function GetRenewalStatusType(ByVal v_sRenStatusCode As String, ByRef r_sDesc As String, ByRef r_lRenewalStatusTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vReturnColumn As Object
        Dim vResult(,) As Object
        Dim sMessage As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vReturnColumn(1)

            vReturnColumn(0) = "renewal_status_type_id"

            vReturnColumn(1) = "description"

            If g_oBusiness.GetValueFromTable(v_sTableName:="Renewal_Status_Type", v_vReturnColumn:=vReturnColumn, v_sKeyColumn:="Code", v_sKeyValue:=v_sRenStatusCode, v_iDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=vResult) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to get Renewal Status Type details"
                Throw New Exception()
            End If

            If Not Information.IsArray(vResult) Then
                sMessage = "No details found for Renewal Status Type (" & v_sRenStatusCode & ")"
                Throw New Exception()
            End If

            r_lRenewalStatusTypeID = CInt(vResult(0, 0))

            r_sDesc = CStr(vResult(1, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sMessage = "" Then
                sMessage = "Failed to get renewal status type details"
            End If

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalStatusType()", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function ProcessAccept(ByVal v_vPolicy As Object, ByVal v_lIndex As Object, ByVal v_bPolicyChanged As Object, ByVal v_sNewPolicyNumber As Object, ByVal v_dNewStartDate As Date, ByVal v_dNewEndDate As Date, ByRef r_sFailureMessage As String) As Integer

        Dim nResult As Integer = 0
        Dim nRenewalPolicyCnt As Integer = 0
        Dim nOldPolicyCnt As Integer = 0
        Dim nRenewalStatusCnt As Integer = 0
        Dim nInsuranceFolder As Integer = 0
        Dim nPartyCnt As Integer = 0
        Dim nAnniversaryCopy As Integer = 0
        Dim bIsTrueMonthlyPolicy As Boolean = False
        Dim oKeyArray(,) As Object = Nothing
        Dim bGenerateDocs As Boolean = False
        Dim oValidationResults(,) As Object = Nothing
        Dim bAcceptIsValid As Boolean = False
        Dim bProduceSchedule As Boolean = False
        Dim bProduceDebitNote As Boolean = False
        Dim bProduceCertificate As Boolean = False
        Dim nProductId As Integer = 0
        ' Dim sValue As String = ""
        Dim oResultArray(,) As Object = Nothing
        Dim crGrossTotal As Decimal = 0
        Dim oProductBusiness As bSIRProduct.Business
        Dim oPrintOptions(,) As Object = Nothing
        Dim oFileCntArray(,) As Object = Nothing
        Dim sPaymentMethod As String = String.Empty
        Dim dtCoverStartDate As Date

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            nOldPolicyCnt = CInt(v_vPolicy(ACIRenewalLivePolicyCnt, v_lIndex))

            nRenewalStatusCnt = CInt(v_vPolicy(ACIRenewalStatusCnt, v_lIndex))

            nRenewalPolicyCnt = CInt(v_vPolicy(ACIRenewalPolicyCnt, v_lIndex))

            nInsuranceFolder = CInt(v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex))

            nPartyCnt = CInt(v_vPolicy(ACIRenewalInsuranceHolder, v_lIndex))
            sPaymentMethod = LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex)))

            nAnniversaryCopy = gPMFunctions.ToSafeInteger(CStr(v_vPolicy(ACIRenewalAnniversaryCopy, v_lIndex)))

            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeBoolean(gPMFunctions.ToSafeInteger(v_vPolicy(ACIRenewalIsTrueMonthlyPolicy, v_lIndex)) = 1)

            'nResult = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            nProductId = gPMFunctions.ToSafeLong(CStr(v_vPolicy(ACIRenewalProductId, v_lIndex)), 0)

            dtCoverStartDate = CDate(v_vPolicy(ACIRenewalCoverStartDate, v_lIndex))
            If bIsTrueMonthlyPolicy = True And nAnniversaryCopy = 1 Then
                m_lReturn = g_oBusiness.GetAnnivPriorVersionInsFileCnt(nFolderCnt:=v_vPolicy(ACIRenewalInsuranceFolder, v_lIndex),
                            nPolicyCnt:=nRenewalPolicyCnt,
                            r_oFileCntArray:=oFileCntArray)

                If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                    If IsArray(oFileCntArray) Then nOldPolicyCnt = ToSafeLong(oFileCntArray(0, 0))
                Else
                    gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If
            End If

            nResult = g_oObjectManager.GetInstance(oProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            nResult = oProductBusiness.GetProductValue(v_lProductId:=nProductId,
                                                 v_sColumnName:="is_roundoff_to_zero",
                                                  r_vProductArray:=oResultArray)

            oProductBusiness = Nothing

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            Else
                If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                    m_bRoundOff = IIf(oResultArray(0, 0) = 1, 1, 0)
                    If m_bRoundOff Then
                        nResult = g_oBusiness.GetPolicyGrossTotal(v_lInsuranceFileCnt:=nRenewalPolicyCnt,
                                                                 r_vResults:=oResultArray)

                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("ProcessAccept", "Failed to Get GetPolicyGrossTotal", gPMConstants.PMELogLevel.PMLogError)
                            Return nResult
                        End If
                        If oResultArray IsNot Nothing AndAlso TypeOf (oResultArray) Is Object(,) Then
                            crGrossTotal = crGrossTotal + gPMFunctions.ToSafeDecimal(oResultArray(4, 0), 0)
                        End If

                        m_crRoundOffAmount = PMRoundupValueCurrency(crGrossTotal, PMECurrencyNoOfDP.pmeCurDPZero, PMERoundupFactor.pmeRFactor50Up) - crGrossTotal
                    End If
                End If

            End If

            nResult = g_oBusiness.GetPrePaymentOptionValue(nProductId, m_lPrepayment)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            If nAnniversaryCopy = gPMConstants.PMEReturnCode.PMTrue Then

                nResult = g_oBusiness.ValidateAcceptTMPIsValidAction(v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_sInsuranceRef:=m_sInsuranceRef, r_vResults:=oValidationResults)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessAccept", "ProcessAccept Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf m_lPrepayment(0, 0) = "1" AndAlso (LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "paynow" OrElse LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "invoice") Then

                If Information.IsArray(oValidationResults) Then
                    bAcceptIsValid = Not (gPMFunctions.ToSafeInteger(CStr(oValidationResults(0, 0)), 0) = 0)
                Else
                    bAcceptIsValid = True
                End If

                If Not bAcceptIsValid Then
                    r_sFailureMessage = "Anniversary renewal can not be accepted until the last monthly cycle has been accepted."
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
            Else
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If


            ''Call Pay Now Process
            'If m_lRenewalMode = ACRenModeRA Or m_lRenewalMode = ACRenModeStandard Then
            '    'Start - Prakash - WPR85_Paralleling

            '    If gPMFunctions.ToSafeString(CStr(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "CashDeposit" Then
            '        nResult = ProcessCashDeposit()

            '        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            '            nResult = m_lReturn
            '            stbMain.Items.Item(0).Text = ""
            '            Return nResult
            '        End If

            '    ElseIf m_lPrepayment(0, 0) = "1" AndAlso (LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "paynow") OrElse (m_lPrepayment(0, 0) = "1" And LCase(ToSafeString(v_vPolicy(ACIPaymentMethod, v_lIndex))) = "invoice") Then
            '        nResult = ShowPayNow(CStr(m_vRenewalPolicy(ACIPaymentMethod, v_lIndex)))

            '        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            '            stbMain.Items.Item(0).Text = ""
            '            Return nResult
            '        End If
            '    End If
            'End If
            If v_bPolicyChanged Then
                nResult = g_oBusiness.AcceptRenewal(v_lOldInsuranceFileCnt:=nOldPolicyCnt, v_lNewInsuranceFileCnt:=nRenewalPolicyCnt, v_lRenewalStatusCnt:=nRenewalStatusCnt, v_sNewPolicyRef:=v_sNewPolicyNumber, v_dNewStartDate:=v_dNewStartDate, v_dNewExpiryDate:=v_dNewEndDate, r_sFailureMessage:=r_sFailureMessage, v_lAccountId:=m_lPaymentAccountID)
            Else
                nResult = g_oBusiness.AcceptRenewal(v_lOldInsuranceFileCnt:=nOldPolicyCnt, v_lNewInsuranceFileCnt:=nRenewalPolicyCnt, v_lRenewalStatusCnt:=nRenewalStatusCnt, r_sFailureMessage:=r_sFailureMessage, v_lAccountId:=m_lPaymentAccountID)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'do stats
            ReDim oKeyArray(1, 11)

            oKeyArray(0, 0) = "insurance_file_cnt"

            oKeyArray(1, 0) = nRenewalPolicyCnt

            oKeyArray(0, 1) = PMNavKeyConst.PMKeyNameIsTrueMonthlyPolicy

            oKeyArray(1, 1) = bIsTrueMonthlyPolicy

            'Float Balance and Pre-Payment

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "Payment Account ID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPaymentAccountID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Debit Against"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_iDebitAgainst

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Credit Transactions"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_vCreditTransactions

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = "Cash List ID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lCashListID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = "Cash ListItem ID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_lCashListItemID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = "TransactionID"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_lTransactionID

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = "TransactionAmount"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_cTransactionAmount

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = "round_off_amount"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_crRoundOffAmount

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = gSIRLibrary.SIRLookupPaymentMethod

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = sPaymentMethod

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = "allocation_calling_app_name"

            oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = "iPMURenewalCatchUp"

            If RunProcess(v_sComponent:="iPMUStats.Interface_Renamed", v_vKeyArray:=oKeyArray, r_sFailureMessage:=r_sFailureMessage, v_lProcessMode:=gPMConstants.PMEComponentAction.PMAdd) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'do accumulation using the same keyarray() as (do stats)
            If RunProcess(v_sComponent:="iPMUAccumulationValues.Interface_Renamed", v_vKeyArray:=oKeyArray, r_sFailureMessage:=r_sFailureMessage, v_lProcessMode:=gPMConstants.PMEComponentAction.PMAdd) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'create renewal acceptance event

            nResult = g_oBusiness.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=nPartyCnt, v_vInsuranceFolderCnt:=nInsuranceFolder, v_vInsuranceFileCnt:=nRenewalPolicyCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Accept Renewal - " & CStr(v_vPolicy(ACIRenewalInsuranceRef, v_lIndex)))

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to create an event for renewal acceptance"
                Return nResult
            End If

            'create broker/agent transfer event

            If gPMFunctions.ToSafeInteger(CStr(v_vPolicy(ACIRenewalIsInTransferMode, v_lIndex)), 0) <> 0 Then

                nResult = g_oBusiness.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=nPartyCnt, v_vInsuranceFolderCnt:=nInsuranceFolder, v_vInsuranceFileCnt:=nRenewalPolicyCnt, v_vEventType:=5, v_vUserId:=g_oObjectManager.UserID, v_vEventDate:=DateTime.Today, v_vDescription:="Renewal Accepted - Broker Transfer From " & CStr(v_vPolicy(ACIRenewalLivePolicyAgentCode, v_lIndex)).Trim() &
                            " to " & CStr(v_vPolicy(ACIRenewalLeadAgentCode, v_lIndex)).Trim())

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to create an event for broker/agent transfer"
                    Return nResult
                End If
            End If

            '*****************************************************
            ' DONT DO ANY DOCUMENT PRODUCTION UNLESS THIS IS THE
            ' ANNIVERSARY VERSION OF THE TRUE MONTHLY POLICY
            '*****************************************************

            ' by default generate documents
            bGenerateDocs = True

            ' if the renewal policy is based on a "true monthly policy" product
            If bIsTrueMonthlyPolicy Then
                ' if this version of the policy is not flagged as the anniversary copy
                If nAnniversaryCopy <> 1 Then
                    ' do not produce any documents
                    bGenerateDocs = False
                End If
            End If

            If bGenerateDocs Then

                nResult = g_oBusiness.GetProdPrintOptions(nProductId, oPrintOptions)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(oPrintOptions) Then
                    bProduceSchedule = gPMFunctions.ToSafeBoolean(oPrintOptions(0, 0))
                    bProduceCertificate = gPMFunctions.ToSafeBoolean(oPrintOptions(1, 0))
                    bProduceDebitNote = gPMFunctions.ToSafeBoolean(oPrintOptions(2, 0))
                End If

                If m_sRenSchedulePrinting = "1" And bProduceSchedule Then
                    'Generate schedule document.
                    nResult = GenerateDocument(v_lProcessType:=ACDocTypeSchedule, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_lInsuranceFolderCnt:=nInsuranceFolder, v_lPartyCnt:=nPartyCnt, v_sSpoolDesc:="Accept Renewal - Schedule Document", r_sFailureMessage:=r_sFailureMessage)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to generate schedule document"
                        Return nResult
                    End If
                End If

                If m_sRenCertificatePrinting = "1" And bProduceCertificate Then

                    'Generate certificate document.
                    nResult = GenerateDocument(v_lProcessType:=ACDocTypeCertificate, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_lInsuranceFolderCnt:=nInsuranceFolder, v_lPartyCnt:=nPartyCnt, v_sSpoolDesc:="Accept Renewal -  Certificate Document", r_sFailureMessage:=r_sFailureMessage)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to generate certificate document"
                        Return nResult
                    End If
                End If

                If m_sRenDebitNotePrinting = "1" And bProduceDebitNote Then

                    'Generate debit note
                    nResult = GenerateDocument(v_lProcessType:=ACDOCTypeDebitNote, v_lMode:=ACSpoolSilentMode, v_lInsuranceFileCnt:=nRenewalPolicyCnt, v_lInsuranceFolderCnt:=nInsuranceFolder, v_lPartyCnt:=nPartyCnt, v_sSpoolDesc:="Accept Renewal -  Debit Note Document", r_sFailureMessage:=r_sFailureMessage)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to generate debit note document"
                        Return nResult
                    End If

                End If

            End If

            'nResult = CancelMTAQuotes(nRenewalPolicyCnt, nInsuranceFolder, nPartyCnt)
            'If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            '    'Business must have logged it.
            'End If

        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to accept renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccept()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            If oProductBusiness IsNot Nothing Then
                oProductBusiness = Nothing
            End If
        End Try
        Return nResult

    End Function


    Private Function GenerateDocument(ByVal v_lProcessType As Integer, ByVal v_lMode As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sSpoolDesc As String, ByRef r_sFailureMessage As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateDocument"

        Dim oGetDocument As iPMUGetDocument.Interface_Renamed
        Dim vKeyArray(,) As Object
        Dim bPMBDocLink As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 4)
            Dim obPMBDocLink As bPMBDocLink.Business
            Dim oResultArray(,) As Object
            Dim temp_obPMBDocLink As Object
            Dim m_iFuntionalArea As Integer

            'Generate document.
            oGetDocument = New iPMUGetDocument.Interface_Renamed()

            If oGetDocument Is Nothing Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMUGetDocument object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = g_oObjectManager.GetInstance(temp_obPMBDocLink, "bPMBDocLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            obPMBDocLink = temp_obPMBDocLink


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Doc Link object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTheTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            oGetDocument.Initialise()

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsuranceFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_lProcessType

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lInsuranceFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameProductID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lProductID

            m_lReturn = oGetDocument.SetKeys(vKeyArray:=vKeyArray)

            oGetDocument.FunctionalArea = 1

            If v_lProcessType = 6 Then  'This renewal notice print
                oGetDocument.TransactionType = "RNI"
            Else
                oGetDocument.TransactionType = "RN"
            End If

            m_lReturn = oGetDocument.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get Values from
            'For time being funtional area is set to 1 i.e. document linking for policy
            m_iFuntionalArea = 1

            m_lReturn = obPMBDocLink.GetSFIDocumentTemplatesForProcessType(v_iFunctionalArea:=m_iFuntionalArea, v_lInsurance_File_Cnt:=v_lInsuranceFileCnt, v_lProcessType_Docs_ID:=v_lProcessType, v_lProcess_Type_Code:="RN", v_dtEffectiveDate:=DateTime.Now, r_vResultarray:=oResultArray, v_bCalledFromSAM:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(oResultArray) Then
                If (oResultArray(10, 0) = "Lapse") Then
                    MessageBox.Show("Lapse Document(s) spooled." & Strings.Chr(13) & Strings.Chr(10) & "Process complete", "Lapse Renewal", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            oGetDocument.Dispose()
            oGetDocument = Nothing

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "GenerateDocument() - " & Information.Err().Description

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate document", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    '*******************************************************************************************************
    'Desc: run relevant component with provided keys and get back required keys from component if required
    '*******************************************************************************************************

    Private Function RunProcess(ByVal v_sComponent As String, ByRef r_sFailureMessage As String, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByRef r_vGetKeyArray(,) As Object = Nothing, Optional ByVal v_bDisplayMessage As Boolean = True, Optional ByVal v_lProcessMode As Integer = gPMConstants.PMEComponentAction.PMEdit, Optional ByVal v_sTransactionType As String = "REN", Optional ByVal v_vSetProperty(,) As Object = Nothing, Optional ByRef r_iStatus As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oObject As Object
        Dim bNavigatorV3 As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'are we using NavigatorV3 or interface class?
            bNavigatorV3 = (v_sComponent.ToUpper().IndexOf(".NAVIGATORV3") >= 0)

            'create an instance of required object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=v_sComponent, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sFailureMessage = "Failed to instantiate object " & v_sComponent
                If v_bDisplayMessage Then
                    MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return result
            End If

            If bNavigatorV3 Then

                If Not Information.IsNothing(v_vKeyArray) Then
                    'pass in relevant keys

                    If oObject.NavigatorV3_SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'set process mode

                If oObject.NavigatorV3_SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'set required properties

                If Not Information.IsNothing(v_vSetProperty) Then
                    If Information.IsArray(v_vSetProperty) Then
                        For lCount As Integer = 0 To v_vSetProperty.GetUpperBound(1)
                            Select Case v_vSetProperty(0, lCount)
                                Case "FinancePlanCnt"

                                    oObject.FinancePlanCnt = CInt(v_vSetProperty(1, lCount))
                                Case "FinancePlanVersion"

                                    oObject.FinancePlanVersion = CInt(v_vSetProperty(1, lCount))
                                Case "Spawned"

                                    oObject.Spawned = (CStr(v_vSetProperty(1, lCount)) = "True")
                                Case "DontDeleteScheme"
                                    'UPGRADE_TODO: (1067) Member DontDeleteScheme is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                                    'UPGRADE_WARNING: (1068) v_vSetProperty(1, lCount) of type Variant is being forced to Scalar. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                                    oObject.DontDeleteScheme = v_vSetProperty(1, lCount)
                            End Select
                        Next lCount
                    End If
                End If

                'start component

                If oObject.NavigatorV3_Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'get keys back if required

                'If Not Information.IsNothing(r_vGetKeyArray) Then
                If Not Information.IsArray(r_vGetKeyArray) Then

                    If oObject.NavigatorV3_GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'did we cancel or error?

                If oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    result = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If
                r_iStatus = oObject.NavigatorV3_Status
            Else
                'we are using interface class

                If Not Information.IsNothing(v_vKeyArray) Then
                    'pass in relevant keys

                    If oObject.SetKeys(v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to set relevant Keys to " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'set process mode

                If oObject.SetProcessModes(vTask:=v_lProcessMode, vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to set process mode to " & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'set required properties

                If Not Information.IsNothing(v_vSetProperty) Then
                    If Information.IsArray(v_vSetProperty) Then
                        For lCount As Integer = 0 To v_vSetProperty.GetUpperBound(1)
                            Select Case v_vSetProperty(0, lCount)
                                Case "FinancePlanCnt"

                                    oObject.FinancePlanCnt = CInt(v_vSetProperty(1, lCount))
                                Case "FinancePlanVersion"

                                    oObject.FinancePlanVersion = CInt(v_vSetProperty(1, lCount))
                                Case "Spawned"

                                    oObject.Spawned = (CStr(v_vSetProperty(1, lCount)) = "True")
                                Case "DontDeleteScheme"
                                    'UPGRADE_TODO: (1067) Member DontDeleteScheme is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                                    'UPGRADE_WARNING: (1068) v_vSetProperty(1, lCount) of type Variant is being forced to Scalar. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                                    oObject.DontDeleteScheme = v_vSetProperty(1, lCount)
                            End Select

                        Next lCount
                    End If
                End If

                'start component


                If oObject.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureMessage = "Failed to start" & v_sComponent
                    If v_bDisplayMessage Then
                        MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return result
                End If

                'get keys back if required

                If Not Information.IsNothing(r_vGetKeyArray) Then

                    If oObject.GetKeys(vKeyArray:=r_vGetKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        r_sFailureMessage = "Failed to get keys from " & v_sComponent
                        If v_bDisplayMessage Then
                            MessageBox.Show(r_sFailureMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return result
                    End If
                End If

                'did we cancel or error?

                If oObject.Status = gPMConstants.PMEReturnCode.PMError Then
                    r_sFailureMessage = "Error in " & v_sComponent
                    result = gPMConstants.PMEReturnCode.PMError
                ElseIf oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    r_sFailureMessage = "Process was cancelled"
                    result = gPMConstants.PMEReturnCode.PMCancel
                End If


            End If  'NavigatorV3



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sFailureMessage = "Failed to run component " & v_sComponent

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sFailureMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RunProcess()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If Not (oObject Is Nothing) Then

                oObject.Dispose()
                oObject = Nothing
            End If



        End Try
        Return result
    End Function


    Private Function CheckForCashDepositPaymentMethod(ByRef bCashDeposit As Boolean) As Integer
        'This method will check if any of the selected policy has Cash Depsoit as payment method.
        Dim result As Integer = 0
        Dim lPolicycnt As Integer
        Dim sPaymentMethod As String = ""
        Dim vResults As Object
        Dim iPosInArray, iSelectedItemCount As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bCashDeposit = False
            iSelectedItemCount = 0

            iPosInArray = 0

            lPolicycnt = CInt(m_vRenewalPolicy(ACIRenewalPolicyCnt, iPosInArray))
            m_lReturn = g_oBusiness.GetPaymentMethod(v_lInsuranceFileCnt:=lPolicycnt, r_vResults:=vResults)

            sPaymentMethod = CStr(vResults(0, 0))
            If sPaymentMethod = "CashDeposit" Then
                bCashDeposit = True
            End If

            If (iSelectedItemCount > 1) And bCashDeposit Then
                bCashDeposit = True
            ElseIf (iSelectedItemCount = 1) And bCashDeposit Then
                bCashDeposit = False
            ElseIf Not bCashDeposit Then
                bCashDeposit = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForCashDepositPaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForCashDepositPaymentMethod", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Public Function SilentRenewal(ByVal v_lInsuranceFolderCnt As Object, Optional ByRef r_bIsProductAutoRenewable As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vRenewalPolicy As Object 'should only have one policy in it
        Dim sInsuranceRef As String = ""
        Dim bRenewalsExist As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFolderCnt = 0 Then
                m_lReturn = SelectPolicy()
            End If

            'RWH(15/05/2001) Put in extra shenaneghans to use this function for single
            'policy selected on this renewal form.
            'If m_bCalledFromLocalForm Then
            '    StatusBar1.Items.Item("Message").Text = "Checking for existing versions in " &
            '                                            "renewal..."
            '    Me.StatusBar1.Refresh()
            '    StatusBar1.Items.Item("Count").Text = "1/1"
            '    Me.StatusBar1.Refresh()


            m_lReturn = m_oBusiness.GetRenewalsForPolicy(v_lInsFolderCnt:=v_lInsuranceFolderCnt, r_bRenewalsExist:=bRenewalsExist)
            If bRenewalsExist Then
                'If MessageBox.Show("A version of this policy is already in renewal." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to re-select the policy?", "Renewals", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then

                '    StatusBar1.Items.Item("Message").Text = "Deleting existing versions in renewal..."
                '    Me.StatusBar1.Refresh()
                m_lReturn = m_oBusiness.DeleteRenewalsForPolicy()
                'Else
                '    StatusBar1.Items.Item("Policy").Text = ""
                '    Me.StatusBar1.Refresh()
                '    StatusBar1.Items.Item("Message").Text = "Ready"
                '    Me.StatusBar1.Refresh()
                '    Return gPMConstants.PMEReturnCode.PMFalse
                'End If
            End If
            'Else
            '    'delete all versions of this policy from renewal

            '    m_lReturn = m_oBusiness.DeleteRenewalPolicy(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
            'End If
            prgbVersion.Value = 5
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Delete Existing Renewal For Selected Policy", Me.Text, MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete all in renewal_report table ready for new data

            If m_oBusiness.DelRenewalReport() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Delete Previous Renewal Report", Me.Text, MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If m_bCalledFromLocalForm Then
            '    StatusBar1.Items.Item("Message").Text = "Retrieving policy details"
            '    Me.StatusBar1.Refresh()
            'End If

            'get correct version of this policy for renewal

            m_lReturn = m_oBusiness.GetPolicyForRenewal(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=vRenewalPolicy)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Get Correct Version Of Policy For Renewal", Me.Text, MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create relevant business objects
            m_lReturn = CreateBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CloseBusinessObject()
                Return result
            End If
            prgbVersion.Value = 10
            'Tracy Richards - Get the Product ID for this Policy

            r_bIsProductAutoRenewable = CDbl(vRenewalPolicy(9, 0)) = 1
            '3547- Priya
            If Not ToSafeBoolean(vRenewalPolicy(33, 0)) Then
                MessageBox.Show("Policy is not renewable. Kindly check the product configuration.", Me.Text, MessageBoxButtons.OK)
                m_lReturn = CloseBusinessObject()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' default policy version additional increment
            m_lPolicyVersionIncrement = 0


            'create renewal version for this policy
            m_lReturn = CreateRenewalPolicyWrapper(r_vRenewalList:=vRenewalPolicy, v_lCount:=0, v_lDispStatusBar:=IIf(m_bCalledFromLocalForm, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed To Create Renewal Version Of Policy", Me.Text, MessageBoxButtons.OK)
                m_lReturn = CloseBusinessObject()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            p_vRenewalPolicy = vRenewalPolicy
            ' if this is a renewal policy on a true monthly policy
            ' determine whether or not we need to create an anniversary renewal policy
            prgbVersion.Value = 40
            m_lReturn = CreateTMPAnniversaryRenewal(r_vRenewalList:=vRenewalPolicy, v_lCount:=0, v_lDispStatusBar:=IIf(m_bCalledFromLocalForm, gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMFalse))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("CreateTMPAnniversaryRenewal Failed", Me.Text, MessageBoxButtons.OK)
                m_lReturn = CloseBusinessObject()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            prgbVersion.Value = 50

            If m_bCalledFromLocalForm Then
                'StatusBar1.Items.Item("Policy").Text = ""
                'Me.StatusBar1.Refresh()
                'StatusBar1.Items.Item("Message").Text = "Complete"
                'Me.StatusBar1.Refresh()
            End If

            m_lReturn = CloseBusinessObject()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SilentRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SilentRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CreateRenewalPolicyWrapper(ByRef r_vRenewalList As Object, ByVal v_lCount As Integer, Optional ByVal v_lDispStatusBar As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateRenewalPolicyWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode
        '    Dim bIsTrueMonthlyPolicy        As Boolean
        '    Dim bCreateRenewalPolicy        As Boolean
        '    Dim dtOriginalRenewalDate       As Date
        '    Dim dtOriginalAnniversaryDate   As Date
        '    Dim vRenewalPolicy              As Variant

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            '
            '    ' default action is to create the renewal policy
            '    bCreateRenewalPolicy = True
            '
            '    ' determine whether the associated policies product is a "True Monthly Policy"
            '    bIsTrueMonthlyPolicy = CBool(ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1)
            '
            '    ' if the associated product is "True Monthly Policy"
            '    If bIsTrueMonthlyPolicy Then
            '
            '        ' get dates
            '        dtOriginalRenewalDate = r_vRenewalList(PMFieldPosRenewalDate, v_lCount)
            '        dtOriginalAnniversaryDate = r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)
            '
            '        ' check whether or not it should be the anniversary renewal
            '        ' is its cover start date >= original anniversary date
            '        If dtOriginalRenewalDate >= dtOriginalAnniversaryDate Then
            '
            '            ' flag that a renewal policy should not be created at this point
            '            ' NB: the following procedure "CreateTMPAnniversaryRenewal" code will
            '            ' create the anniversary version of the renewal if it is required
            '            bCreateRenewalPolicy = False
            '        End If
            '
            '    End If
            '
            '    ' if a renewal should be created
            '    If bCreateRenewalPolicy = True Then

            ' set additional policy version increment
            m_lPolicyVersionIncrement = 1

            ' create the renewal


            lReturn = CType(CreateRenewalPolicy(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount, v_lDispStatusBar:=v_lDispStatusBar), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateRenewalPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            '   End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Public Function CreateTMPAnniversaryRenewal(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer, Optional ByVal v_lDispStatusBar As Integer = 0) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "CreateTMPAnniversaryRenewal"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim lAnniversaryRenewalWeeks As Integer
        Dim sInsuranceRef As String = ""
        Dim dtCoverStartDate As Date
        Dim vAnniversaryCopy As Object
        Dim lAnniversaryCopyCount As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine whether the associated policies product is a "True Monthly Policy"
            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1

            ' if the associated product is "True Monthly Policy"
            If bIsTrueMonthlyPolicy Then

                ' get the anniversary renewal weeks
                ' this is the number of weeks prior to the anniversary date that the
                ' that the anniversary renewal version of the policy should be created

                lAnniversaryRenewalWeeks = CInt(r_vRenewalList(PMFieldPosAnniversaryRenewalWeeks, v_lCount))

                ' if the anniversary renewal version of the policy should have been created
                ' by now

                If gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >= gPMFunctions.ToSafeDate(DateAndTime.DateAdd("ww", -lAnniversaryRenewalWeeks, ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)))) Then


                    sInsuranceRef = CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)).Trim()

                    ' cover start date for the anniversary copy is the anniversary date of the
                    ' original policy

                    dtCoverStartDate = ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount))

                    'Debug.Print m_oBusiness.TransactionType

                    ' check if it has been created

                    lReturn = m_oBusiness.FindAnniversaryCopy(v_sInsuranceRef:=sInsuranceRef, v_dtCoverStartDAte:=dtCoverStartDate, r_vResults:=vAnniversaryCopy)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "FindAnniversaryCopy Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' if there is an array we have an anniversary copy
                    ' if not go ahead and create the anniversary copy
                    If Not Information.IsArray(vAnniversaryCopy) Then
                        lAnniversaryCopyCount = 0
                    Else

                        lAnniversaryCopyCount = CInt(vAnniversaryCopy(0, 0))
                    End If

                    If lAnniversaryCopyCount = 0 Then

                        ' create an anniversary renewal version of the policy
                        lReturn = CType(CreateRenewalPolicy(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount, v_lDispStatusBar:=v_lDispStatusBar, v_bTMPAnniversary:=True), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateRenewalPolicy Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                End If

            Else
                ' do nothing
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function CreateRenewalPolicy(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Object, Optional ByVal v_lDispStatusBar As Object = Nothing, Optional ByVal v_bTMPAnniversary As Object = Nothing) As Integer
        Dim nResult As Integer
        Dim nRenewalStatusTypeID As Integer 'renewal status to go on the Renewal Status table
        Dim sFailureCriterion As String = ""
        Dim nNewInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lEligibleForRenewal As gPMConstants.PMEReturnCode
        Dim lProductId, lInsuredCnt As Integer
        Dim nSourceID As Integer = 0
        Dim bMidnightRenewal As Boolean
        Dim vKeyArray(,) As Object
        Dim bIsPremiumZero As Boolean
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim oArray(,) As Object = Nothing
        Dim oInsuranceFileTax As Object = Nothing
        Dim sDescription, sDateInteval As String
        Dim lDateIntervalNumber As Integer
        Dim lIsAgentCancelled As gPMConstants.PMEReturnCode
        Dim oRenewalFrequency As Object = Nothing
        Dim nUnderwritingYearID As Integer
        Dim sFailureMessage As String = ""
        Dim oBrokerTransferPorfolio(,) As Object = Nothing
        Dim nBrokerXferStatusTypeID As Integer = 0
        ' True monthly policy changes
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim nAnniversaryCopy As Integer = 0
        Dim nRenewalDayNumber As Integer = 0
        Dim dtTMPCoverStartDate As Date
        Dim dtTMPExpiryDate As Date
        Dim dtTMPRenewalDate As Date
        Dim dtTMPAnniversaryDate As Date
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bPutOnnextInstalmentRenewal As Boolean
        Dim nOriginalInsuranceFileCnt As Integer
        Dim bContinue As Boolean
        Dim bRenewalStatusAutomatic As Boolean
        Dim oListRisksBusiness As bSIRListRisks.Business
        Dim bInvoiceEnabled, bInstalmentsEnabled, bPayNowEnabled As Boolean
        Dim sPayment_Method As String = ""
        Dim nRenewalProductId As Integer = 0
        Dim sInstFreqency As String = ""
        Dim bisOriginalProductTMP, bSwapProducts As Boolean
        Dim vDefaultRenewalMths As String = ""
        Dim oResultsInst(,) As Object = Nothing
        Dim oProduct As bSIRProduct.Business
        Dim oUseNbPaymentTermAtRenSelection(,) As Object = Nothing
        Dim lInstalmentInsFileCnt As Integer
        Dim bChanged As Boolean
        Const kPolicyBusinessType As Integer = 2
        Dim bCashDepositEnabled As Boolean
        Dim bisOnlyAgentTransfer As Boolean
        Dim bIsReferred As Boolean
        Dim oGracePeriod As Object
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)
            m_lReturn = CreateBusinessObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)

            vDefaultRenewalMths = ""
            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy"
                'Me.StatusBar1.Refresh()
            End If

            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then

                'StatusBar1.Items.Item("Policy").Text = " " & CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount))
                'Me.StatusBar1.Refresh()
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Get " & "current policy details"
                'Me.StatusBar1.Refresh()
            End If

            'assign current InsuranceFileCnt to object


            m_oInsuranceFile.InsuranceFileCnt = r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)

            'get details of current policy

            If m_oInsuranceFile.Getdetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set policy status to Renewal

            m_oInsuranceFile.InsuranceFileStatus = "REN"


            m_oInsuranceFile.LapsedReasonID = Nothing


            m_oInsuranceFile.LapsedDate = Nothing

            'PN 30936 ------------------------------------------ START
            Dim temp_oListRisksBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_oListRisksBusiness, "bSIRListRisks.Business",
                                                   vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oListRisksBusiness = temp_oListRisksBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "Failed to get instance of bSIRListRisks.Business"
            End If
            'Get current PaymentTerms
            'Start - Prakash - WPR85_Paralleling
            'Prakash Varghese - Added the parameter r_bCashDepositEnabled as part of Tech Spec -UIIC_WPR85_Cash_Deposit_Process-Part 2 work


            lReturn = oListRisksBusiness.GetPaymentTerms(v_lInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                         v_lPMUserID:=g_oObjectManager.UserID,
                                                         r_bInvoiceEnabled:=bInvoiceEnabled,
                                                         r_bInstalmentsEnabled:=bInstalmentsEnabled,
                                                         r_bPayNowEnabled:=bPayNowEnabled,
                                                         r_bCashDepositEnabled:=bCashDepositEnabled)
            'End - Prakash - WPR85_Paralleling

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "bSIRListRisks.Business.GetPaymentTerms Failed"
            End If




            ' Tech Spec PGR 8.8 Renewals ---------------------------------
            Dim temp_oProduct As Object
            lReturn = g_oObjectManager.GetInstance(temp_oProduct, "bSIRProduct.Business",
                                                   vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oProduct = temp_oProduct
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "Failed to get instance of bSIRProduct.Business"
            End If

            'Get current PaymentTerms


            lReturn = oProduct.GetProductValue(v_lProductId:=m_oInsuranceFile.ProductID,
                                               v_sColumnName:="use_nb_payment_term_at_renselection",
                                               r_vProductArray:=oUseNbPaymentTermAtRenSelection)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "bSIRProduct.Business.GetProductValue Failed"
            End If
            ' End ----------------------------------------------------------

            Dim sOptionValue As String = "0"

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=SharedFiles.gPMConstants.kSystemOptionAutoInstalment, r_sOptionValue:=sOptionValue, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "iPMFunc.GetSystemOption Failed"
            End If

            oBatchRenewalBusiness = New bSIRRenewalBusiness()
            oBatchRenewalBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
            If oBatchRenewalBusiness Is Nothing Then
                Throw New ApplicationException("Failed to create instance of bSIRRenewalBusiness.Business")
            End If
            If Information.IsArray(oUseNbPaymentTermAtRenSelection) Then

                If Not (Not (CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0)) Then
                    If (sOptionValue.ToString = "1") Then
                        lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                        sPayment_Method = sPayment_Method.ToLower()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            sFailureCriterion = "GetInitialPolicyDetails Failed"
                        Else

                            m_oInsuranceFile.PaymentMethod = sPayment_Method

                        End If
                    Else
                        sPayment_Method = gPMFunctions.ToSafeString(m_oInsuranceFile.PaymentMethod).ToLower().Trim()
                        lInstalmentInsFileCnt = m_oInsuranceFile.InsuranceFileCnt
                    End If
                Else
                    'Check Auto Instalment is checked or not
                    If (sOptionValue.ToString = "1") Then
                        lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                    Else
                        lReturn = m_oBusiness.GetInitialPolicyDetails(lInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  lLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                    End If

                    sPayment_Method = sPayment_Method.ToLower()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        sFailureCriterion = "GetInitialPolicyDetails Failed"

                    End If
                End If
            Else

                sPayment_Method = gPMFunctions.ToSafeString(m_oInsuranceFile.PaymentMethod).ToLower().Trim()
                lInstalmentInsFileCnt = nOriginalInsuranceFileCnt
            End If
            If (oBatchRenewalBusiness IsNot Nothing) Then
                oBatchRenewalBusiness.Dispose()
                oBatchRenewalBusiness = Nothing
            End If

            Dim vUsePriorTermSchemeAtRenewal(,) As Object

            lReturn = oProduct.GetProductValue(v_lProductId:=m_oInsuranceFile.ProductID,
                                                   v_sColumnName:="use_prior_term_scheme_at_ren",
                                                   r_vProductArray:=vUsePriorTermSchemeAtRenewal)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "bSIRProduct.Business.GetProductValue Failed"
            End If



            Dim instalmentInsFileCnt = m_oInsuranceFile.InsuranceFileCnt

            'Dim oriInsuranceFileCnt = lInstalmentInsFileCnt

            'If Information.IsArray(vUsePriorTermSchemeAtRenewal) Then

            '    If ((CDbl(vUsePriorTermSchemeAtRenewal(0, 0)) = 0) Or (sOptionValue.ToString = "1")) Then
            '        instalmentInsFileCnt = oriInsuranceFileCnt ' Change From Spec
            '    Else
            '        m_lReturn = m_oBusiness.GetPriorTermSchemeInsuranceFile(lInsuranceFileCnt:=oriInsuranceFileCnt,
            '                                               lPriorTermSchemeInsuranceFileCnt:=instalmentInsFileCnt,
            '                                               v_bUsePriorTermSchemeAtRenewal:=vUsePriorTermSchemeAtRenewal(0, 0))
            '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '            'r_sFailureMessage = "GetPriorTermSchemeInsuranceFile Failed"
            '            Exit Function
            '        End If
            '        obSIRPremiumFinance.UsePriorTermSchemeAtRenewal = True
            '    End If
            'Else
            '    instalmentInsFileCnt = oriInsuranceFileCnt ' Change From Spec
            'End If

            Dim dtStartDate, dtEndDate As Date
            Dim crAmountToFinance As Decimal
            Dim iDayOfWeekOrMonth As Integer
            Dim bHasProtection, bFoundExactCopy As Boolean
            Dim lCount, lPartyCnt As Integer
            Dim vAmountToFinance(,) As Object = Nothing
            Dim vQuoteArray(,) As Object = Nothing
            'Dim lReturn As gPMConstants.PMEReturnCode
            'Dim oRITax As bSIRRITax.Business
            Dim cTaxNotApplied As Decimal
            Dim bProductChangedAtREN As Boolean

            i_oBusiness.InsuranceFileCnt = instalmentInsFileCnt
            i_oBusiness.SelectSingle()

            bFoundExactCopy = False

            crAmountToFinance = i_oBusiness.AnnualPremium
            ' Alix Bergeret - 21/03/2003 - Issue 2428
            ' We also need to add the tax!
            'If IsNumeric(vResultArray(67, 0)) Then
            '    crAmountToFinance = crAmountToFinance + vResultArray(67, 0)
            'End If
            ' /Alix

            dtStartDate = i_oBusiness.CoverStartDate.AddMonths(1)

            dtEndDate = i_oBusiness.ExpiryDate.AddMonths(1)
            ' Alix Bergeret - 20/03/2003 - Issue 2428
            ' If partycnt was NOT passed in, we get it from the insurance file array
            lPartyCnt = lInsuredCnt
            If lPartyCnt = 0 Then

                lPartyCnt = gPMFunctions.ToSafeInteger(i_oBusiness.InsuredCnt)
            End If

            m_lReturn = obSIRPremiumFinance.Calculate_Quotes(0, "REN", DateTime.Now, dtStartDate, dtEndDate, CDate("00:00:00"), iDayOfWeekOrMonth, iDayOfWeekOrMonth, crAmountToFinance, bHasProtection, -1, False, lPartyCnt, vQuoteArray, v_lInsuranceFileCnt:=instalmentInsFileCnt, v_lRenewalInsFileCnt:=nNewInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vQuoteArray) Then
                'bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "Failed to calculate the Quotes " & "for this renewal: " & gPMFunctions.ToSafeString(v_lRenewalInsuranceFileCnt), ACApp, ACClass, k_sFUNCTION_NAME)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Loop through the Quotes and determine which was for the same
            'Frequency/medi as the previous years (already been highlighted by
            'the Calculate_Quotes function)
            'TR - 24/03/03 - TS17 Recovery By Instalments changes
            'SMJB 17/10/03: Change made on behalf of Ashley Cottle

            For lCount = vQuoteArray.GetLowerBound(0) To vQuoteArray.GetUpperBound(1)
                'TR - See if the HighlightCell property is set to true - this
                'signifies that this is the same scheme as used last time

                If vQuoteArray(2, lCount) = schemeNumber Then
                    bFoundExactCopy = True


                End If
            Next lCount

            If Not bFoundExactCopy Then
                sPayment_Method = "invoice"

            End If
            'if Payment_Method is not PayNow and Instalments then it must be Invoice
            'Start - Prakash - WPR85_Paralleling
            If _
                sPayment_Method <> "instalments" AndAlso sPayment_Method <> "direct debit" AndAlso sPayment_Method <> "paynow" AndAlso
                sPayment_Method <> "credit card" AndAlso sPayment_Method <> "cashdeposit" Then
                sPayment_Method = "invoice"
            End If
            'End - Prakash - WPR85_Paralleling

            'Check if the Payment_Method is Invalid
            If sPayment_Method = "invoice" And Not bInvoiceEnabled Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (Invoice) is Invalid"
            ElseIf _
                (sPayment_Method = "instalments" Or sPayment_Method = "direct debit" Or sPayment_Method = "credit card") And
                Not bInstalmentsEnabled Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (Instalments) is Invalid"
            ElseIf sPayment_Method = "paynow" And Not bPayNowEnabled Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (PayNow) is Invalid"
                'Start - Prakash - WPR85_Paralleling
            ElseIf sPayment_Method = "cashdeposit" And Not bCashDepositEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (CashDeposit) is Invalid"
            End If

            If sPayment_Method = "cashdeposit" Then
                'If payment method is cash deposit, always set for manual review.
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                'Start - Renuka -  Changes according to the WPR85 process sheet updation
                sFailureCriterion = "Manual Review - Payment option cash depsoit"
                'End - Renuka -  Changes according to the WPR85 process sheet updation
            End If
            'End - Prakash - WPR85_Paralleling


            If sFailureCriterion <> "" Then

                'If there's an Err, Add it to Renewal_report table

                m_lReturn = m_oBusiness.AddRenewalReport(
                    v_sReportType:=IIf(bRenewalStatusAutomatic, "AutoRenewal", "ManualRenewal"),
                    v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                    v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                    v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                    v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                    v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                    v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                    v_vFailureCriterion:=sFailureCriterion,
                    v_vFailureDetail:="",
                    v_vInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                sFailureCriterion = ""
            End If
            'PN 30936 ------------------------------------------ END
            If _
                sPayment_Method = "paynow" AndAlso
                (Trim(m_oInsuranceFile.businesstype) = "COIN FOLL" OrElse Trim(m_oInsuranceFile.businesstype) = "IN FAC") _
                Then

                sFailureCriterion = "Manual Review Required for Co-Insurance Follow / Inward Facultative"

                'Add Error to Renewal_report table
                m_lReturn = m_oBusiness.AddRenewalReport(
                    v_sReportType:=IIf(bRenewalStatusAutomatic,
                                         "AutoRenewal", "ManualRenewal"),
                    v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                    v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                    v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                    v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                    v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                    v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                    v_vFailureCriterion:=sFailureCriterion)

            End If

            'PN 30909 ------------------------------------------ START

            If m_oInsuranceFile.BusinessType.Trim() = "COIN FOLL" Or m_oInsuranceFile.BusinessType.Trim() = "IN FAC" _
                Then

                sFailureCriterion = "Manual Review Required for Co-Insurance Follow / Inward Facultative"

                'Add Error to Renewal_report table

                m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                         v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                         v_vPolicyNumber:=
                                                            r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                         v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                         v_vCoverStartDate:=
                                                            r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                         v_vCoverEndDate:=
                                                            r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                         v_vProductCode:=
                                                            r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                         v_vFailureCriterion:=sFailureCriterion,
                                                         v_vFailureDetail:="",
                                                         v_vInsuranceFileCnt:=
                                                            r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))


            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Update " & "current policy status"
                'Me.StatusBar1.Refresh()
            End If

            '01/08/2003 Tracy Richards - Add a transaction to umbrella all these
            'calls, to make sure that records are not left in a mid-state.

            m_lReturn = m_oInsuranceFile.BeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update current policy to status Renewal

            m_lReturn = m_oInsuranceFile.UpdatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set policy type to Renewal

            m_oInsuranceFile.InsuranceFileType = "RENEWAL"
            'set policy to Live (i.e. status = Null).


            m_oInsuranceFile.InsuranceFileStatus = Nothing

            lProductId = m_oInsuranceFile.ProductID

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Is policy" & " midnight renewal?"
                'Me.StatusBar1.Refresh()
            End If


            m_lReturn = m_oBusiness.GetMidnightRenewal(v_lProductId:=lProductId, r_bMidnight:=bMidnightRenewal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get renewal frequency number (number of months)


            m_lReturn = m_oBusiness.GetRenewalFrequencyDetail(v_lFrequencyID:=m_oInsuranceFile.RenewalFrequencyID,
                                                              r_vResult:=oRenewalFrequency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' determine which if any policy discount details need
            ' to be applied to the renewal....

            ' if the original policy has a recurring type of discount then

            If m_oInsuranceFile.DiscountRecurringTypeId = kDiscountRecurringTypeIdPolicy Then
                ' retain the existing policy discount information from the original policy
            Else
                ' otherwise clear down this information

                m_oInsuranceFile.DiscountPercentage = 0

                m_oInsuranceFile.DiscountReasonID = 0

                m_oInsuranceFile.MatchDiscountedPremiumFlag = 0

                m_oInsuranceFile.DiscountedPremium = 0

                m_oInsuranceFile.DiscountRecurringTypeId = 0
            End If


            Dim oAltRefMandetory As Object = Nothing


            If gPMFunctions.ToSafeLong(m_oInsuranceFile.LeadAgentCnt, 0) <> 0 Then

                '        m_lReturn = m_oInsuranceFileBusiness.GetFromTable("party_agent", "alternate_reference_for_each_transaction", "party_cnt", m_oInsuranceFile.LeadAgentCnt, vAltRefForEachTrans)


                m_lReturn = m_oInsuranceFileBusiness.GetFromTable("party_agent", "alternate_reference_mandatory",
                                                                  "party_cnt", m_oInsuranceFile.LeadAgentCnt,
                                                                  oAltRefMandetory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeBoolean(oAltRefMandetory, 0) And m_oInsuranceFile.AlternateReference = "" Then

                    'm_oInsuranceFile.AlternateReference = ""

                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "The Alternate Reference must be entered for this renewal policy." &
                                        "You must amend the renewal before the renewal can be accepted."
                End If
            End If
            'PN 33800 (RC) ----------------------------End


            sDateInteval = "m"

            lDateIntervalNumber = CInt(oRenewalFrequency(2, 0))
            'check if original product is tmp


            m_lReturn = m_oBusiness.IsTrueMonthlyPolicyProduct(m_oInsuranceFile.ProductID, bisOriginalProductTMP)

            'get the instalmment scheme frequency
            If _
                sPayment_Method = "instalments" Or sPayment_Method = "direct debit" Or
                sPayment_Method <> "paynow" And sPayment_Method <> "credit card" Then


                m_lReturn =
                    m_oBusiness.GetInstalmentFrequency(
                        gPMFunctions.ToSafeLong(IIf(CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0,
                                                    (r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                                                    lInstalmentInsFileCnt)), sInstFreqency)
            End If

            bSwapProducts = False
            If _
                bisOriginalProductTMP AndAlso
                gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >=
                gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)) Then
                bSwapProducts = True
            ElseIf Not bisOriginalProductTMP AndAlso (sInstFreqency <> "Q" AndAlso sInstFreqency <> "A") Then
                bSwapProducts = True
            End If

            '1.12 Wr25
            If (gPMFunctions.ToSafeString(r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)) <> "") And bSwapProducts _
                Then


                m_oInsuranceFile.OriginalProductID = r_vRenewalList(PMFieldPosProductID, v_lCount)


                m_oInsuranceFile.ProductID = r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)


                m_oInsuranceFile.RenewalProductID = r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)

                If _
                    bisOriginalProductTMP And
                    gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >=
                    gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)) Then


                    m_lReturn = m_oInsuranceFileBusiness.GetFromTable("PRODUCT", "default_renewal_months", "product_id",
                                                                      m_oInsuranceFile.ProductID, vDefaultRenewalMths)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' determine whether the associated policies product is a "True Monthly Policy"
            'bIsTrueMonthlyPolicy = CBool(ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1)


            m_lReturn = m_oBusiness.IsTrueMonthlyPolicyProduct(m_oInsuranceFile.ProductID, bIsTrueMonthlyPolicy)

            ' Switch off the TMP flag on a policy as its getting moved to another product
            If bisOriginalProductTMP AndAlso bSwapProducts AndAlso Not bIsTrueMonthlyPolicy Then

                r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount) = 0
            End If

            ' if the associated product is "True Monthly Policy"
            If bIsTrueMonthlyPolicy Then
                If Not v_bTMPAnniversary Then

                    v_bTMPAnniversary =
                        gPMFunctions.ToSafeBoolean(
                            r_vRenewalList(PMFieldPosRenewalDate, v_lCount).Equals(
                                r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)))
                    m_Anniversary = v_bTMPAnniversary
                End If
                ' get the appropriate dates for the true monthly policy
                lReturn = CType(GetTrueMonthlyPolicyDates(v_bMidnightRenewal:=bMidnightRenewal,
                                                          v_bTMPAnniversary:=v_bTMPAnniversary, v_lCount:=v_lCount,
                                                          r_vRenewalList:=r_vRenewalList,
                                                          r_dtCoverStartDate:=dtTMPCoverStartDate,
                                                          r_dtExpiryDate:=dtTMPExpiryDate,
                                                          r_dtRenewalDate:=dtTMPRenewalDate,
                                                          r_dtAnniversaryDate:=dtTMPAnniversaryDate,
                                                          r_lRenewalDayNumber:=nRenewalDayNumber,
                                                          r_lAnniversaryCopy:=nAnniversaryCopy),
                                gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_oInsuranceFile.RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' save the data back to the insurance file

                m_oInsuranceFile.CoverStartDate = dtTMPCoverStartDate

                m_oInsuranceFile.ExpiryDate = dtTMPExpiryDate

                m_oInsuranceFile.RenewalDAte = dtTMPRenewalDate

                m_oInsuranceFile.InceptionTPI = dtTMPCoverStartDate

                m_oInsuranceFile.AnniversaryDate = dtTMPAnniversaryDate

                m_oInsuranceFile.AnniversaryCopy = nAnniversaryCopy

                m_oInsuranceFile.RenewalDayNumber = nRenewalDayNumber

                m_oInsuranceFile.PutOnNextInstalmentRenewal = 0


                bPutOnnextInstalmentRenewal =
                    gPMFunctions.ToSafeLong(CDbl(r_vRenewalList(PMFieldPosPutOnNextInstalmentRenewal, v_lCount)) = 1)

                ' if this is the anniversary copy of the policy then
                ' reset the inception date to be that of the new cover start date
                If v_bTMPAnniversary Then

                    m_oInsuranceFile.InceptionDate = dtTMPCoverStartDate
                End If

            Else

                'set new cover period
                'The problem here is that cover start date is that of this version of
                'the policy not of the policy as a whole.  So let's use the renewal
                'date instead, as that won't have changed

                If gPMFunctions.ToSafeInteger(vDefaultRenewalMths) > 0 Then
                    lDateIntervalNumber = gPMFunctions.ToSafeInteger(vDefaultRenewalMths)
                End If

                If bMidnightRenewal Then

                    'Thinh Nguyen 08/10/2002 - add one day to expiry date for next start date


                    m_oInsuranceFile.CoverStartDate = CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddDays(1)


                    m_oInsuranceFile.ExpiryDate =
                        DateAndTime.DateAdd(sDateInteval, lDateIntervalNumber, m_oInsuranceFile.CoverStartDate).AddDays(
                            -1)

                Else


                    m_oInsuranceFile.CoverStartDate = r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)


                    m_oInsuranceFile.ExpiryDate = DateAndTime.DateAdd(sDateInteval, lDateIntervalNumber,
                                                                      m_oInsuranceFile.CoverStartDate)

                End If

                'Thinh Nguyen 13/01/2003 - renewal date is cover from date plus period of the policy


                m_oInsuranceFile.RenewalDAte = DateAndTime.DateAdd(sDateInteval, lDateIntervalNumber,
                                                                   m_oInsuranceFile.CoverStartDate)


                m_oInsuranceFile.InceptionTPI = m_oInsuranceFile.CoverStartDate

                ' store anniversary date as renewal date


                m_oInsuranceFile.AnniversaryDate = m_oInsuranceFile.RenewalDAte


                m_oInsuranceFile.RenewalDayNumber = DateAndTime.Day(m_oInsuranceFile.RenewalDAte)

            End If

            Dim sValue As String
            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, 1, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If sValue = "1" Then
                m_bUnderwritingYearID = True
            Else
                m_bUnderwritingYearID = False
            End If

            'RKS PN13438 and DD 14537
            If m_bUnderwritingYearID Then


                m_lReturn = m_oInsuranceFileBusiness.GetUnderwritingYear(m_oInsuranceFile.CoverStartDate,
                                                                         nUnderwritingYearID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Underwriting Year for " & m_sTransactionType & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                ElseIf nUnderwritingYearID = 0 Then
                    'PN14537 - Log the problem, don't prompt the user
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review

                    sFailureCriterion = "No Underwriting Year exists for " &
                                        StringsHelper.Format(m_oInsuranceFile.CoverStartDate, "General Date")

                    m_oInsuranceFile.UnderwritingYearID = nUnderwritingYearID
                    lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                Else

                    m_oInsuranceFile.UnderwritingYearID = nUnderwritingYearID
                End If
            End If
            'RKS PN13438


            m_oInsuranceFile.EventDescription = "Policy Copied To Renewal"

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Create renewal policy"
                'Me.StatusBar1.Refresh()
            End If

            If v_bTMPAnniversary Then
                ' if this is an anniversary policy it is possible that
                ' a normal renewal (non anniversary) has already been run
                ' so a further increment of the Policy Version is required

                m_oInsuranceFile.PolicyVersion = m_oInsuranceFile.PolicyVersion + 1 + m_lPolicyVersionIncrement
            Else

                m_oInsuranceFile.PolicyVersion += 1
            End If

            'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)

            sInsuranceRef = m_oInsuranceFile.InsuranceRef
            'Start - Renuka - (WPR87 Paralleling)


            m_lReturn =
                m_oPolicyNumMaint.GenerateRenewalPolicyNumber(
                    v_iPolicy_cnt:=gPMFunctions.ToSafeInteger(CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt,
                                                                                         v_lCount))),
                    v_lBusinessType:=kPolicyBusinessType,
                    v_iBranch:=gPMFunctions.ToSafeInteger(m_oInsuranceFile.SourceID),
                    v_lProductId:=gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosProductID, v_lCount)),
                    v_lAgent:=gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)),
                    r_sGeneratedPolicyNumber:=sInsuranceRef, r_bChanged:=bChanged,
                    v_dtTransactionDate:=m_oInsuranceFile.CoverStartDate)
            'End - Renuka - (WPR87 Paralleling)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'create new policy of type Renewal
            If bChanged Then

                m_oInsuranceFile.InsuranceRef = sInsuranceRef
                'Start(Sriram P)PN55579

                r_vRenewalList(PMFieldPosInsuranceRef, v_lCount) = sInsuranceRef
                'End(Sriram P)PN55579
            End If






            m_oInsuranceFile.PaymentMethod = sPayment_Method

            'End(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)
            m_oInsuranceFile.QuoteExpiryDate = Nothing
            'Update Renewal Quote Expiry date
            lReturn = oProduct.GetProductValue(v_lProductId:=m_oInsuranceFile.ProductID, v_sColumnName:="grace_period", r_vProductArray:=oGracePeriod)
            If Information.IsArray(oGracePeriod) Then
                Dim iGracePeriod As Integer = CInt(oGracePeriod.GetValue(0, 0))
                If iGracePeriod > 0 Then
                    m_oInsuranceFile.QuoteExpirydate = DateTime.Today.AddDays(iGracePeriod)
                End If
            End If

            'create new policy of type Renewal

            m_lReturn = m_oInsuranceFile.CreatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
                'Tracy Richards - Commit trans here as all essential processing has been done.
            Else

                m_oInsuranceFile.CommitTrans()
            End If

            'get client and branch

            lInsuredCnt = m_oInsuranceFile.InsuredCnt

            nSourceID = m_oInsuranceFile.SourceID

            'get new insurance file cnt

            nNewInsuranceFileCnt = m_oInsuranceFile.InsuranceFileCnt

            sInsuranceRef = m_oInsuranceFile.InsuranceRef

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying standard wording"
                'Me.StatusBar1.Refresh()
            End If


            m_lReturn = m_oBusiness.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount), v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, v_dtEffectiveDate:=CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddYears(1))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying coinsurance"
                'Me.StatusBar1.Refresh()
            End If

            'RWH(14/06/01) Copy coinsurance.

            m_lReturn = m_oBusiness.CopyCoinsurance(
                v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                v_lNewInsFileCnt:=nNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Failed to copy coinsurance"
            End If

            'Tomo160801
            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying agent commission"
                'Me.StatusBar1.Refresh()
            End If

            'Copy agent commission.


            If _
                Not _
                (bIsTrueMonthlyPolicy AndAlso
                 CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 And
                 CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then

                m_lReturn =
                    m_oBusiness.CopyAgentCommission(
                        v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                        v_lNewInsFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "Failed to copy agent commission"
                End If
            End If

            'Tomo170801
            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Copying policy agents"
                'Me.StatusBar1.Refresh()
            End If

            'Copy insurance file agents.

            m_lReturn =
                m_oBusiness.CopyInsuranceFileAgent(
                    v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                    v_lNewInsFileCnt:=nNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Failed to copy policy agents"
            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Risks"
                'Me.StatusBar1.Refresh()
            End If

            'RWH(22/08/01) Must set the Task in Tax as this is passed into stored procedures
            'as Mode. If it is wrong the new tax records will not be created.

            m_lReturn = m_oTax.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vTransactionType:="REN")

            'copy risk details to new policy
            m_lReturn = CopyRiskData(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                     v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                     r_sFailureReason:=sFailureCriterion, r_lEligibleForRenewal:=lEligibleForRenewal,
                                     v_bIsTrueMonthlyPolicy:=bIsTrueMonthlyPolicy,
                                     v_bTMPAnniversary:=v_bTMPAnniversary, r_bIsReferred:=bIsReferred)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Always update policy premium to avoid magical figures during manual review

                m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Leave alone. Failure is anyway due to copy risk data
                End If

                If sFailureCriterion = "" Then
                    sFailureCriterion = "Failed to copy risk data"
                Else
                    sFailureCriterion = "Failed to copy risk data (" & sFailureCriterion & ")"
                End If
                If nRenewalStatusTypeID = 0 Then
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                End If
            Else

                ' default to continue with process
                bContinue = True
                'PN 67489 -Always update policy premium to avoid magical figures during manual review

                m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Leave alone. Failure is anyway due to copy risk data
                End If

                ' if the renewal policy is subject to a policy discount

                If m_oInsuranceFile.DiscountRecurringTypeId = kDiscountRecurringTypeIdPolicy Then

                    ' indicate to the user that we are applying the discount
                    If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                        'StatusBar1.Items.Item("Message").Text = "Copying Policy - Applying Policy Discount"
                        'Me.StatusBar1.Refresh()
                    End If

                    ' apply any discount that is defined


                    m_lReturn = ApplyPolicyDiscount(m_oInsuranceFile.InsuranceFileCnt, m_oInsuranceFile.ProductID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' if the apply discount process failed then we dont want to continue
                        ' so drop out of the process
                        bContinue = False

                        ' indicate this item needs to be manually reviewed.
                        If sFailureCriterion = "" Then
                            sFailureCriterion = "Failed to apply policy discount"
                        Else
                            sFailureCriterion = "Failed to apply policy discount (" & sFailureCriterion & ")"
                        End If

                        If nRenewalStatusTypeID = 0 Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                        End If

                    End If

                End If

                If bContinue Then

                    If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                        'StatusBar1.Items.Item("Message").Text = "Copying Policy - Checking renewal criteria"
                        'Me.StatusBar1.Refresh()
                    End If

                    'Check Auto-renewal eligibility.

                    If _
                        m_oBusiness.CheckRenewalCriteria(r_vRenewalList, v_lCount, bisOnlyAgentTransfer) =
                        gPMConstants.PMEReturnCode.PMFalse Then
                        lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'TN20010719 - start
                    If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue Then
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            'StatusBar1.Items.Item("Message").Text = "Copying Policy - Checking quote status"
                            'Me.StatusBar1.Refresh()
                        End If


                        m_lReturn = m_oBusiness.IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                         r_lIsQuoted:=lIsQuoted)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            lIsQuoted = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If lIsQuoted = gPMConstants.PMEReturnCode.PMFalse Then
                            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Adding renewal report " &
                                '                                        "record"
                                'Me.StatusBar1.Refresh()
                            End If

                            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse


                            m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                                     v_vClientName:=
                                                                        r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                                     v_vPolicyNumber:=
                                                                        r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                                     v_vAgentCode:=
                                                                        r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                                     v_vCoverStartDate:=
                                                                        r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                       v_lCount),
                                                                     v_vCoverEndDate:=
                                                                        r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                                     v_vProductCode:=
                                                                        r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                                     v_vFailureCriterion:=PMIsQuotedDesc,
                                                                     v_vFailureDetail:="",
                                                                     v_vInsuranceFileCnt:=
                                                                        r_vRenewalList(PMFieldPosInsuranceFileCnt,
                                                                                       v_lCount))

                        End If
                    End If

                    'If not eligible for renewal, either because a risk failed rating OR
                    'policy level auto-renewal criteria were failed, then set RenewalStatusType
                    'and message appropriately.
                    If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                        If bisOnlyAgentTransfer Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated
                        Else
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                        End If
                        If sFailureCriterion = "" Then
                            sFailureCriterion = "Manual Review"
                        End If
                        m_lReturn = m_oBusiness.IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                         r_lIsQuoted:=lIsQuoted)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            lIsQuoted = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    If lIsQuoted = gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMTrue

                        If bIsTrueMonthlyPolicy And Not v_bTMPAnniversary Then
                            'Set status to say we are ready to print the renewal notice.
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitUpdate
                        ElseIf nRenewalStatusTypeID <> gPMConstants.PMBRenewalStatusTypeManualReview Then
                            'Set status to say we are ready to print the renewal notice.
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated

                        End If
                        'DO POLICY LEVEL STUFF
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            'StatusBar1.Items.Item("Message").Text = "Copying Policy - Policy level reinsurance"
                            'Me.StatusBar1.Refresh()
                        End If

                        'Reset RiskId to ensure Policy level reinsurance is done.

                        m_oReinsurance.InsuranceFileCnt = nNewInsuranceFileCnt

                        m_oReinsurance.RiskId = 0


                        m_lReturn = m_oReinsurance.Getdetails

                        'Do policy taxes.
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            'StatusBar1.Items.Item("Message").Text = "Copying Policy - Policy level tax"
                            'Me.StatusBar1.Refresh()
                        End If


                        m_oTax.InsuranceFileCnt = nNewInsuranceFileCnt


                        m_lReturn = m_oTax.GetInsuranceFileTax(oInsuranceFileTax, sDescription)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to do Policy Taxes"
                        End If

                        ' create any fees that apply for the renewal policy

                        m_lReturn = m_oBusiness.CreateRenewalFees(nNewInsuranceFileCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to calculate renewal fees"
                        End If

                        'Update policy premium.
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            'StatusBar1.Items.Item("Message").Text = "Copying Policy - Update policy premium"
                            'Me.StatusBar1.Refresh()
                        End If


                        m_lReturn =
                            m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to update Policy Premium"
                        End If

                        'Do agent commission.
                        If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                            'StatusBar1.Items.Item("Message").Text = "Copying Policy - Agent commission"
                            'Me.StatusBar1.Refresh()
                        End If


                        m_oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt

                        'Tomo17102001 - The Get deletes the existing records, and recalculates them
                        'but does _not_ write them back to the database.  The calculate does...


                        If _
                            Not _
                            (bIsTrueMonthlyPolicy AndAlso
                             CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 AndAlso
                             CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then

                            m_lReturn = m_oBusiness.CopyAgentCommission(v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                                                        v_lNewInsFileCnt:=nNewInsuranceFileCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                                sFailureCriterion = "Failed to copy agent commission"
                            End If

                            m_lReturn =
                                m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                            v_sTransactionType:="REN",
                                                                            r_vntResult:=oArray)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "Failed to do Agent Commission"
                            End If
                            m_lReturn =
                                m_oAgentCommission.CopyPolicyCommission(
                                    r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount), nNewInsuranceFileCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                CreateRenewalPolicy = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                                sFailureCriterion = "Failed to copy Commission"
                            End If
                        End If

                        'TMP
                        If Not bIsTrueMonthlyPolicy Then
                            ' Is the premium zero ?

                            m_lReturn = m_oBusiness.IsPremiumZero(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                  r_bIsPremiumZero:=bIsPremiumZero)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "Failed to check IsPremiumZero"
                            Else
                                'If the premium is zero then fail and let the user know why in the report
                                If bIsPremiumZero Then
                                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                    'Awaiting Manual Review
                                    sFailureCriterion = "Premium Is Zero"
                                End If
                            End If
                        End If

                        'Thinh Nguyen 15/08/2003 (start) - set to manual review if lead agent is cancelled


                        If _
                            CStr(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)) <> "" Or
                            r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount) Is DBNull.Value Then


                            m_lReturn =
                                m_oBusiness.IsAgentCancelled(
                                    v_lpartyCnt:=CInt(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)),
                                    r_lIsCancelled:=lIsAgentCancelled)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "IsAgentCancelled Failed"
                            ElseIf lIsAgentCancelled = gPMConstants.PMEReturnCode.PMTrue Then
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                                'Awaiting Manual Review
                                sFailureCriterion = "Agent has been cancelled"

                            End If
                        End If
                        'Thinh Nguyen 15/08/2003 (start) - set to manual review if lead agent is cancelled
                    End If
                End If
            End If

            sFailureMessage = ""
            oBatchRenewalBusiness = New bSIRRenewalBusiness()
            oBatchRenewalBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
            If oBatchRenewalBusiness Is Nothing Then
                Throw New ApplicationException("Failed to create instance of bSIRRenewalBusiness.Business")
            End If
            If bPutOnnextInstalmentRenewal Then
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                nOriginalInsuranceFileCnt =
                    gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLatestInstalmentPlanInsuranceFileCnt, v_lCount), 0)
            Else

                nOriginalInsuranceFileCnt = CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                'do we have instalment plan on this policy

                If (sOptionValue.ToString = "1") Then

                    m_lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     lInstalmentInsFileCnt)
                    If ToSafeInteger(lInstalmentInsFileCnt) <> 0 Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                    End If
                Else
                    If (CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0) Then
                        m_lReturn = m_oBusiness.IsInstalment(v_lInsuranceFileCnt:=(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
                    Else
                        m_lReturn = m_oBusiness.IsInstalment(v_lInsuranceFileCnt:=lInstalmentInsFileCnt)
                    End If
                End If

            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'create quote plan for the renewal version


                m_lReturn = m_oBusiness.CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=lInstalmentInsFileCnt,
                                                              v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                              v_lPartyCnt:=lInsuredCnt,
                                                              r_sFailureMessage:=sFailureMessage,
                                                              v_lProductId:=
                                                                 gPMFunctions.ToSafeLong(
                                                                     r_vRenewalList(PMFieldPosProductID, v_lCount)))



                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'mark it as manual renewal
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                End If

            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                sFailureMessage = "Check for existing instalment plan failed for Policy ID " &
                                  CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
            End If

            If bPutOnnextInstalmentRenewal Then
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                nOriginalInsuranceFileCnt =
                    gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLatestInstalmentPlanInsuranceFileCnt, v_lCount), 0)
            Else

                nOriginalInsuranceFileCnt = CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                'do we have instalment plan and Active Party Bank on this policy

                m_lReturn =
                    m_oBusiness.IsInstalmentAndActivePartyBank(
                        v_lInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                        r_vResults:=oResultsInst)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Information.IsArray(oResultsInst) Then


                    If CStr(oResultsInst(0, 0)) = "1" AndAlso CStr(oResultsInst(1, 0)) = "0" Then
                        'mark it as manual renewal
                        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview

                        sFailureMessage = "Party Bank Account is inactive for Policy ID " &
                                          CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                    End If
                End If

            End If

            m_oInsuranceFileBusiness.ValidateProductBranch(m_oInsuranceFile.ProductId, m_oInsuranceFile.SourceId,
                                                           bIsValid)
            If bIsValid <> True Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = " The Product is not available through the selected Branch. " &
                                    "You must amend the renewal before the renewal can be accepted."
                sFailureMessage = sFailureCriterion
            End If

            'if we didn't fail then check broker transfer portfolio
            If sFailureMessage = "" Then


                m_lReturn =
                    m_oBusiness.GetBrokerTransferPortfolioDetail(
                        v_lInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                        r_vResultArray:=oBrokerTransferPorfolio)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to check broker transfer portfolio"
                Else
                    If Information.IsArray(oBrokerTransferPorfolio) Then
                        If gPMFunctions.ToSafeLong(oBrokerTransferPorfolio(1, 0), 0) = 1 Then
                            nBrokerXferStatusTypeID = nRenewalStatusTypeID
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer
                        End If
                    End If
                End If
            End If

            If sFailureMessage <> "" Then

                m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                         v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                         v_vPolicyNumber:=
                                                            r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                         v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                         v_vCoverStartDate:=
                                                            r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                         v_vCoverEndDate:=
                                                            r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                         v_vProductCode:=
                                                            r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                         v_vFailureCriterion:="", v_vFailureDetail:=sFailureMessage,
                                                         v_vInsuranceFileCnt:=
                                                            r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))

            End If

            If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                'StatusBar1.Items.Item("Message").Text = "Copying Policy - Adding renewal status " &
                '                                        "record"
                'Me.StatusBar1.Refresh()
            End If


            m_lReturn = m_oBusiness.AddRenewalStatus(v_lProductId:=r_vRenewalList(PMFieldPosProductID, v_lCount),
                                                     v_lRenewalStatusTypeID:=nRenewalStatusTypeID,
                                                     v_lInsuranceHolderCnt:=
                                                        r_vRenewalList(PMFieldPosInsuranceHolderCnt, v_lCount),
                                                     v_lInsuranceFileCnt:=
                                                        r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                                     v_vLeadAgentCnt:=r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount),
                                                     v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                     v_lBrokerXferStatusTypeID:=nBrokerXferStatusTypeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'as far as Silent Renewal is concerned its now in renewal
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            '1.12 Wr25
            ' get the Product Id of New Insurancefile cnt

            m_lReturn = m_oBusiness.SelectRenewalProduct(nNewInsuranceFileCnt, nRenewalProductId)

            If _
                gPMFunctions.ToSafeLong(m_oInsuranceFile.RenewalProductID) <> gPMFunctions.ToSafeLong(nRenewalProductId) And
                gPMFunctions.ToSafeLong(nRenewalProductId) > 0 Then
                ' update the renewal product in the base insurancefile then delete the renewal version

                m_lReturn =
                    m_oBusiness.UpdateRenewalProduct(
                        v_lInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                        v_lProductId:=nRenewalProductId)

                m_lReturn = m_oBusiness.DeleteRenewal(nNewInsuranceFileCnt)
                Return nResult
            End If


            'RWH(16/11/2000) if the policy is not elligible for renewal then we have already
            'added a record to the report.
            If _
                (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) AndAlso
                (lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue) Then
                'add to Renewal_Report table
                If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                    'StatusBar1.Items.Item("Message").Text = "Copying Policy - Adding renewal report " &
                    '                                        "record"
                    'Me.StatusBar1.Refresh()
                End If

                'If (lRenewalStatusTypeID <>  PMBRenewalStatusTypeAutoRated)  AND (lRenewalStatusTypeID  <> PMBRenewalStatusTypeAwaitUpdate) And
                'bRenewalStatusAutomatic = ((lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated) Or ((lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitUpdate) And (Not v_bTMPAnniversary And bIsTrueMonthlyPolicy)))
                bRenewalStatusAutomatic = ((nRenewalStatusTypeID = PMBRenewalStatusTypeAutoRated) Or
                                           ((nRenewalStatusTypeID = PMBRenewalStatusTypeAwaitUpdate) And
                                            (v_bTMPAnniversary = False And bIsTrueMonthlyPolicy = True)) Or
                                           (nRenewalStatusTypeID = PMBRenewalStatusTypeAwaitBrokerTransfer))
                If sFailureCriterion <> "" OrElse bRenewalStatusAutomatic Then

                    m_lReturn = m_oBusiness.AddRenewalReport(
                        v_sReportType:=IIf(bRenewalStatusAutomatic, "AutoRenewal", "ManualRenewal"),
                        v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                        v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                        v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                        v_vCoverStartDate:=m_oInsuranceFile.CoverStartDate,
                        v_vCoverEndDate:=m_oInsuranceFile.ExpiryDate,
                        v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                        v_vFailureCriterion:=sFailureCriterion, v_vFailureDetail:="", v_vInsuranceFileCnt:=nNewInsuranceFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If sFailureCriterion <> "" Then
                sFailureCriterion = "Renewal - " & sInsuranceRef & " - " & sFailureCriterion

                If v_lDispStatusBar = gPMConstants.PMEReturnCode.PMTrue Then
                    'StatusBar1.Items.Item("Message").Text = "Copying Policy - Add task to work manager"
                    'Me.StatusBar1.Refresh()
                End If

                ReDim vKeyArray(1, 1)


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "insurance_file_cnt"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = nNewInsuranceFileCnt

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameRunMode

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

                If Not isWorkMngrTaskCreated Then
                    m_lReturn = m_oBusiness.AddTaskToWorkManager(
                        v_sClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount), v_sDescription:=sFailureCriterion,
                        v_dtDueDate:=DateAndTime.DateAdd("ww", 2, DateTime.Today), v_vKeyArray:=vKeyArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    isWorkMngrTaskCreated = True
                End If
                Return nResult

            Else

                If Not (m_obSIRRenewal Is Nothing) Then


                    m_lReturn = m_obSIRRenewal.GenerateCustomerRenewalEmail(v_lpartyCnt:=lInsuredCnt,
                                                                            v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                            v_sType:="selection")
                    If _
                        m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso
                        m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        ' Call m_oInsuranceFile.RollbackTrans
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            End If


            If m_oInsuranceFile.BusinessType.Trim() = "AGENCY" Then

                m_lReturn = ValidateCertificateYear(bIsValid, nNewInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to getCertificate Year for " & m_sTransactionType & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                End If
            End If
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRenewalPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return nResult
        End Try
    End Function

    Public Function ValidateCertificateYear(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sValue As String = ""
        Dim r_sMessage As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, 1, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option " & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            If sValue = "1" Then
                m_lReturn = m_obSIRRenewal.GetAndValidateSubAgentDetailsViaInsFile(bIsValid, lNewInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsValid = False Then
                    m_obSIRRenewal.UpdateRenewalStatus(gPMConstants.PMBRenewalStatusTypeManualReview, r_sMessage)
                    System.Windows.Forms.MessageBox.Show("You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", Me.Text, MessageBoxButtons.OK)
                    Return result
                End If

            End If
            Return result
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="ValidateCertificateYear", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Public Function ApplyPolicyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ApplyPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' apply policy discount

            lReturn = m_oBusiness.ApplyPolicyDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=v_lProductId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ApplyPolicyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    Private Function CopyRiskData(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Object, ByVal v_lNewInsuranceFileCnt As Object, ByRef r_lEligibleForRenewal As Integer, ByRef r_sFailureReason As String, Optional ByVal v_bIsTrueMonthlyPolicy As Object = Nothing, Optional ByVal v_bTMPAnniversary As Object = Nothing, Optional ByRef r_bIsReferred As Boolean = False) As Integer

        Dim nResult As Integer
        Dim nNewGisPolicyLinkID As Integer
        Dim oGisPolicyLinkArray(,) As Object = Nothing
        Dim oRiskArray(,) As Object = Nothing
        Dim nNewRiskCnt As Integer
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim nOldPolicyBinderId As Integer = 0
        Dim nNewPolicyBinderId As Integer = 0
        Dim sFailureDetail As String = String.Empty
        Dim sFailureCriterion As String = String.Empty
        Dim oRiskTax As Object
        Dim sDescription As String = ""
        Dim nTransactionType As Integer = 0
        Dim nQuoteType As Integer = 0
        ' Changes for new reinsurance
        Dim nReinsPremiumOrSumInsured As Integer = 0
        Dim nReinsBand As Integer = 0
        Dim bIsRIValid As Boolean
        Const kFieldPosRiskID As Integer = 0
        Const kFieldPosGisScreenID As Integer = 21

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

            'StatusBar1.Items.Item("Message").Text = "Copying Risks - Get relevant risks"
            'Me.StatusBar1.Refresh()

            m_oRiskData.TransactionType = "REN"

            'get all risks associate with OldInsuranceFileCnt

            If _
                m_oRiskData.GetRisk(v_lInsuranceFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                    r_vResultArray:=oRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Getting Risk"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any risks
            If Not Information.IsArray(oRiskArray) Then
                'RWH(16/11/2000) We do not need to report an error if there are no risks.
                r_sFailureReason = "No risks found"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'loop thro and copy each risk details

            For iCount As Integer = 0 To oRiskArray.GetUpperBound(1)
                'StatusBar1.Items.Item("Message").Text = "Copying Risk Data"
                'copy risk to NewInsuranceFileCnt

                m_lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                 v_vRiskDetail:=oRiskArray, v_lPosNo:=iCount,
                                                 r_lRiskCnt:=nNewRiskCnt,
                                                 v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "Copy Risk"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'prepare details to copy GIS Stuff attached to current risk
                'get policy link detail
                ' Pass folder_cnt instead of file_cnt.
                m_lReturn =
                    m_oRiskData.GetGISPolicyLink(
                        v_lInsuranceFolderCnt:=r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                        v_lRiskID:=oRiskArray(ACRiskPosCnt, iCount), r_vResultArray:=oGisPolicyLinkArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "GetGISPolicyLink"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'do we have any data
                Dim auxVar_2 As Object = oGisPolicyLinkArray(0, 0)


                If Not (Convert.IsDBNull(auxVar_2) OrElse IsNothing(auxVar_2)) Then
                    'Make sure GIS object present.
                    m_lReturn = m_oBusiness.GIS_LoadFromDB(CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                           r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                           oGisPolicyLinkArray(0, 0), oRiskArray(0, iCount)) _
                    'copy GIS details to NewInsuranceFileCnt
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "LoadFromDB"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RWH(20/11/2000) REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
                    'So we pass existing folder_cnt in for old and new file_cnt.


                    m_lReturn = m_oBusiness.CopyDataSet(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        r_lNewGISPolicyLinkId:=nNewGisPolicyLinkID,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet,
                                                        v_vOldGISPolicyLinkId:=oGisPolicyLinkArray(0, 0),
                                                        v_vOldInsuranceFileCnt:=
                                                           r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                        v_vOldRiskID:=oRiskArray(0, iCount),
                                                        v_vNewInsuranceFileCnt:=
                                                           r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                        v_vNewRiskID:=nNewRiskCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "CopyDataSet"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Initialise the Data Set with the Object/Properties
                    m_lReturn = m_oBusiness.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "LoadFromXML"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "SaveToDB"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get Policy Binder Ids
                    m_lReturn = m_oBusiness.GetPolicyBinderId(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lGISPolicyLinkId:=nNewGisPolicyLinkID,
                                                              r_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "GetPolicyBinderId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GetPolicyBinderId(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lGISPolicyLinkId:=oGisPolicyLinkArray(0, 0),
                                                              r_lPolicyBinderId:=nOldPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "GetPolicyBinderId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' start (run the renewal scripts)
                    m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "DeleteOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")
                    nTransactionType = m_oBusiness.TransactionType
                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 5)

                    'run renewal script


                    m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed Renewal Script"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Clear rating output variable before new test.
                    sFailureDetail = ""
                    'Check Output table to see if risk has been referred or declined.


                    m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                             v_lPolicyBinderId:=nNewPolicyBinderId,
                                                             r_sReasons:=sFailureDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'StatusBar1.Items.Item("Message").Text = "Copying Risk Data - Standard Wordings"
                    'Me.StatusBar1.Refresh()


                    m_lReturn = m_oBusiness.CopyRiskStandardWordings(v_lOldPolicyBinderId:=nOldPolicyBinderId, v_lNewPolicyBinderId:=nNewPolicyBinderId, v_sDataModelCode:=CStr(oGisPolicyLinkArray.GetValue(4, 0)).Trim(), v_dtEffectiveDate:=CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddYears(1))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CopyRiskStandardWordings"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Index linking"
                    'Me.StatusBar1.Refresh()
                    If (v_bTMPAnniversary AndAlso v_bIsTrueMonthlyPolicy) OrElse (Not v_bIsTrueMonthlyPolicy) Then
                        ' INDEX LINKING GIS STUFF 
                        Dim auxVar As Object = oRiskArray(kFieldPosGisScreenID, iCount)


                        If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                            'index link GIS
                            m_lReturn = m_oBusiness.GisIndexLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                                 v_lRiskID:=oRiskArray(kFieldPosRiskID, iCount),
                                                                 v_vGisScreenID:=
                                                                    oRiskArray(kFieldPosGisScreenID, iCount),
                                                                 v_dtEffectiveDate:=
                                                                    CDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                         v_lCount)).AddYears(1),
                                                                 v_sGisDataModelCode:=
                                                                    CStr(oGisPolicyLinkArray(4, 0)).Trim())
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If r_sFailureReason <> "" Then
                                    r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                                End If
                                r_sFailureReason = r_sFailureReason & "Index Link"
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

                    ' check to see if this option is in use
                    If m_bExtraRiskDetails Then
                        'StatusBar1.Items.Item("Message").Text = "Copying risk data - Sum insured"
                        'Me.StatusBar1.Refresh()
                        'copy RSA_Sum_Insured

                        m_lReturn = m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=oGisPolicyLinkArray(0, 0),
                                                                  v_lNewPolicyLinkID:=nNewGisPolicyLinkID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "CopyRSASumInsured"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Prepare output table"
                    'Me.StatusBar1.Refresh()

                    m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "DeleteOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Underwriting authority " &
                    '                                        "limits"
                    'Me.StatusBar1.Refresh()
                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 3)

                    'Check Underwriting Authority Limits.


                    m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Check Underwriting Authority Limits"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Check Output table to see if risk has been referred or declined.
                    m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                             v_lPolicyBinderId:=nNewPolicyBinderId,
                                                             r_sReasons:=sFailureDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Prepare output table"
                    'Me.StatusBar1.Refresh()


                    m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                              v_lPolicyBinderId:=nNewPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "DeleteOutputTable2"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Quote risk"
                    'Me.StatusBar1.Refresh()
                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 1)

                    'Quote risk.
                    m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                        r_sXMLDataSetDef:=sXMLDataSetDef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed to Quote"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Check output table"
                    'Me.StatusBar1.Refresh()
                    'Check Output table to see if risk has been referred or declined.


                    m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                             v_lPolicyBinderId:=nNewPolicyBinderId,
                                                             r_sReasons:=sFailureDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'PerilAllocation - Rating Sections
                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Peril allocation"
                    'Me.StatusBar1.Refresh()
                    'Set required params for PerilAllocation

                    m_oPerilAllocation.InsuranceFileCnt = v_lNewInsuranceFileCnt


                    m_oPerilAllocation.InsuranceFolderCnt = r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount)

                    m_oPerilAllocation.RiskId = nNewRiskCnt

                    'RWH(24/08/01) Change put in for Tom.

                    m_oPerilAllocation.TransactionType = "REN"

                    'Do PerilAllocation/Rating Sections stuff.

                    m_lReturn = m_oPerilAllocation.PopulateRatingSections

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "PopulateRatingSections"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RWH(24/08/01) Change put in for Tom.
                    'Update the risk premium

                    m_lReturn = m_oPerilAllocation.UpdateRisk


                    'RWH(13/08/01) Moved the reinsurance part from within condition above.
                    'Reinsurance should be copied whether rating has succeeded or not.
                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Risk reinsurance"
                    'Me.StatusBar1.Refresh()
                    ' Set RI properties

                    m_oReinsurance.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    m_oReinsurance.RiskId = nNewRiskCnt

                    If _
                        ((v_bIsTrueMonthlyPolicy AndAlso nTransactionType = 10) AndAlso Not v_bTMPAnniversary) AndAlso
                        ToSafeInteger(r_vRenewalList(PMFieldPosTMPAutoRenFAC, v_lCount)) = 1 Then
                        m_oReinsurance.TMPRiskCntUnderRenewal = ToSafeLong(oRiskArray(ACRiskPosCnt, iCount))
                    End If

                    ' Calculate RI

                    m_lReturn = m_oReinsurance.CalculateRI
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Calculating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Load details to validate

                    m_lReturn = m_oReinsurance.Getdetails
                    If _
                        m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso
                        m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Getting Reinsurance Details"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Update details, this will ensure minor rounding is handled

                    m_lReturn = m_oReinsurance.Update
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Updating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Validate the reinsurance is all OK
                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Validating reinsurance"
                    'Me.StatusBar1.Refresh()
                    m_lReturn = m_oReinsurance.ValidateBands(nReinsPremiumOrSumInsured, nReinsBand)

                    ' Must return true AND a zero for lReinsPremiumOrSumInsured
                    bIsRIValid = (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) AndAlso (nReinsPremiumOrSumInsured = 0)
                    If Not bIsRIValid Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Validating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'StatusBar1.Items.Item("Message").Text = "Copying risk data - Risk tax"
                    'Me.StatusBar1.Refresh()

                    m_oTax.RiskCnt = nNewRiskCnt
                    m_oTax.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    m_lReturn = m_oTax.GetRiskTax(oRiskTax, sDescription)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "GetRiskTax"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'set risk status to QUOTED if reinsurance is complete, Unquoted otherwise
                    If sFailureDetail <> "" Then

                        If sFailureDetail.Substring(0, 8) = "DECLINED" Then
                            m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                     v_lRiskStatusID:=2)
                        ElseIf sFailureDetail.Substring(0, 8) = "REFERRED" Then

                            m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                    v_lRiskStatusID:=1)
                        End If
                    Else
                        m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                 v_lRiskStatusID:=IIf(bIsRIValid = True, 3, 4))
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.Chr(13) & Strings.Chr(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed to update risk status"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    If sFailureDetail <> "" Then
                        'StatusBar1.Items.Item("Message").Text = "Copying risk data - Add renewal report " &
                        '                                        "record"
                        'Me.StatusBar1.Refresh()

                        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                        sFailureCriterion = PMBConst.PMFailedReRateDesc
                        If sFailureDetail.Contains("REFERRED") Then
                            r_bIsReferred = True
                        End If

                        m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                                 v_vClientName:=
                                                                    r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                                 v_vPolicyNumber:=
                                                                    r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                                 v_vAgentCode:=
                                                                    r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                                 v_vCoverStartDate:=
                                                                    r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                                 v_vCoverEndDate:=
                                                                    r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                                 v_vProductCode:=
                                                                    r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                                 v_vFailureCriterion:=sFailureCriterion,
                                                                 v_vFailureDetail:=sFailureDetail,
                                                                 v_vInsuranceFileCnt:=
                                                                    r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))

                    End If

                Else
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureReason = "No Gis detail"

                    ' Log Error Message


                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lNewInsuranceFileCnt", v_lNewInsuranceFileCnt)
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="No Gis detail for InsuranceFileCnt:" &
                                                  CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)) &
                                                  " RiskID:" & CStr(oRiskArray(1, iCount)), vApp:=ACApp,
                                                  vClass:=ACClass, vMethod:="CopyRiskData", oDicParms:=oDict)

                End If
            Next iCount

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return nResult
        End Try
    End Function

    Public Function GetTrueMonthlyPolicyDates(ByVal v_bMidnightRenewal As Boolean, ByVal v_bTMPAnniversary As Boolean, ByVal v_lCount As Integer, ByRef r_vRenewalList(,) As Object, ByRef r_dtCoverStartDate As Date, ByRef r_dtExpiryDate As Date, ByRef r_dtRenewalDate As Date, ByRef r_dtAnniversaryDate As Date, ByRef r_lRenewalDayNumber As Integer, ByRef r_lAnniversaryCopy As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTrueMonthlyPolicyDates"

        Dim dtOriginalRenewalDate, dtOriginalAnniversaryDate As Date
        Dim lOriginalRenewalDayNumber As Integer
        Dim lDay, lYear, lMonth As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            dtOriginalRenewalDate = CDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount))
            dtOriginalAnniversaryDate = gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount))

            lOriginalRenewalDayNumber = CInt(r_vRenewalList(PMFieldPosRenewalDayNumber, v_lCount))
            r_lRenewalDayNumber = lOriginalRenewalDayNumber

            If Not v_bTMPAnniversary Then

                ' cover start date
                r_dtCoverStartDate = dtOriginalRenewalDate

                lDay = DateAndTime.Day(r_dtCoverStartDate)
                lMonth = r_dtCoverStartDate.Month
                lYear = r_dtCoverStartDate.Year
                ' New Renewal Date = Renewal Date + 1 Month Aligned to Day Number
                If lOriginalRenewalDayNumber > DateAndTime.Day(DateAndTime.DateSerial(lYear, lMonth + 2, 0)) Then
                    r_dtRenewalDate = DateAndTime.DateSerial(lYear, lMonth + 2, 0)
                Else
                    r_dtRenewalDate = GetClosestDate(lOriginalRenewalDayNumber, dtOriginalRenewalDate.Month + 1, dtOriginalRenewalDate.Year)
                End If
                If v_bMidnightRenewal Then
                    r_dtExpiryDate = r_dtRenewalDate.AddDays(-1)
                Else
                    r_dtExpiryDate = r_dtRenewalDate
                End If

                ' The new renewals anniversary date = polcies anniversary date
                r_dtAnniversaryDate = dtOriginalAnniversaryDate

                ' Anniversary Copy = 0
                r_lAnniversaryCopy = 0

            Else

                ' Cover Start Date = Anniversary Date
                r_dtCoverStartDate = dtOriginalAnniversaryDate
                lDay = DateAndTime.Day(r_dtCoverStartDate)
                lMonth = r_dtCoverStartDate.Month
                lYear = r_dtCoverStartDate.Year
                ' Expiry Date = Anniversary Date + 1 Month Aligned to Renewal Day Number
                If lOriginalRenewalDayNumber > DateAndTime.Day(DateAndTime.DateSerial(lYear, lMonth + 2, 0)) Then
                    r_dtRenewalDate = DateAndTime.DateSerial(lYear, lMonth + 2, 0)
                Else
                    r_dtRenewalDate = GetClosestDate(lOriginalRenewalDayNumber, dtOriginalAnniversaryDate.Month + 1, dtOriginalAnniversaryDate.Year)
                End If
                ' If This Is a Midnight Renewal then the Expiry Date (Cover To Date) = Renewal Date - 1 Day
                If v_bMidnightRenewal Then
                    r_dtExpiryDate = r_dtRenewalDate.AddDays(-1)
                Else
                    ' Expiry Date (Cover To Date) = Renewal Date
                    r_dtExpiryDate = r_dtRenewalDate
                End If

                ' Anniversary Date = Anniversary Date + 1 Year
                r_dtAnniversaryDate = dtOriginalAnniversaryDate.AddYears(1)

                ' Anniversary Copy = 1
                r_lAnniversaryCopy = 1

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Public Function GetClosestDate(ByVal v_lDay As Integer, ByVal v_lMonth As Integer, ByVal v_lYear As Integer) As Date

        Dim result As Date = DateTime.FromOADate(0)
        Const kMethodName As String = "GetClosestDate"

        Dim lReturn, lNewMonth As Integer
        Dim dtSerial As Date
        Dim dtMonthStart As Date



        Try



            result = DateTime.FromOADate(gPMConstants.PMEReturnCode.PMTrue)

            If v_lMonth > 12 Then
                v_lMonth = 1
                v_lYear += 1
            End If

            ' serialise the date from the passed data
            dtSerial = DateAndTime.DateSerial(v_lYear, v_lMonth, v_lDay)

            ' get the month from the newly serialised date
            lNewMonth = dtSerial.Month

            ' if the month of the new date doesnt match the
            ' month passed in then
            If lNewMonth <> v_lMonth Then

                dtMonthStart = DateAndTime.DateSerial(v_lYear, lNewMonth, 1)

                dtSerial = dtMonthStart.AddDays(-1)

            End If

            ' return the serialised date
            ' or the last day in the specified month
            result = dtSerial


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function CloseBusinessObject() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oInsuranceFile Is Nothing) Then

                m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

            'RKS PN13438
            If Not (m_oInsuranceFileBusiness Is Nothing) Then

                m_oInsuranceFileBusiness.Dispose()
                m_oInsuranceFileBusiness = Nothing
            End If


            If Not (m_oReinsurance Is Nothing) Then

                m_oReinsurance.Dispose()
                m_oReinsurance = Nothing
            End If

            If Not (m_oTax Is Nothing) Then

                m_oTax.Dispose()
                m_oTax = Nothing
            End If

            If Not (m_oRiskData Is Nothing) Then

                m_oRiskData.Dispose()
                m_oRiskData = Nothing
            End If

            If Not (m_oPerilAllocation Is Nothing) Then

                m_oPerilAllocation.Dispose()
                m_oPerilAllocation = Nothing
            End If

            If Not (m_oAgentCommission Is Nothing) Then

                m_oAgentCommission.Dispose()
                m_oAgentCommission = Nothing
            End If

            If Not (m_oChangePolicyStatus Is Nothing) Then

                m_oChangePolicyStatus.Dispose()
                m_oChangePolicyStatus = Nothing
            End If
            'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.3.2)
            If Not (m_oPolicyNumMaint Is Nothing) Then

                m_oPolicyNumMaint.Dispose()
                m_oPolicyNumMaint = Nothing
            End If
            'End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.3.2)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseBusinessObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CreateBusinessObject() As Integer

        Dim result As Integer = 0
        Dim vIsRI2007 As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFile = temp_m_oInsuranceFile

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRInsuranceFile.Services", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'RKS PN13438
            Dim temp_m_oInsuranceFileBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFileBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFileBusiness = temp_m_oInsuranceFileBusiness


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirInsuranceFile.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'RKS PN13438

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vIsRI2007)

            If ToSafeInteger(vIsRI2007) = 1 Then
                Dim temp_m_oReinsurance As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oReinsurance, "bSIRReinsuranceRI2007.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oReinsurance = temp_m_oReinsurance
            Else
                Dim temp_m_oReinsurance2 As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oReinsurance2, "bSIRReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oReinsurance = temp_m_oReinsurance2

            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirReinsurance.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:="REN")

            Dim temp_m_oTax As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oTax, "bSIRRITax.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oTax = temp_m_oTax

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRRITax.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oRiskData As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskData, "bSIRRiskData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskData = temp_m_oRiskData

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirRiskData.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oPerilAllocation As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPerilAllocation, "bSirPerilAllocation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPerilAllocation = temp_m_oPerilAllocation

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirPerilAllocation.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oAgentCommission As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAgentCommission, "bSirAgentCommission.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAgentCommission = temp_m_oAgentCommission

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirAgentCommission.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Dim temp_m_oChangePolicyStatus As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oChangePolicyStatus, "bSIRChangePolicyStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oChangePolicyStatus = temp_m_oChangePolicyStatus

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirChangePolicyStatus.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
            'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering(5.3.2)
            Dim temp_m_oPolicyNumMaint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumMaint, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPolicyNumMaint = temp_m_oPolicyNumMaint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSirPolicyNumMaint.business", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
            'End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering(5.3.2)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBusinessObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function SelectPolicy() As Integer
        Dim result As Integer = 0

        Dim oFindInsurance As iPMBFindInsurance.Interface_Renamed
        Dim iStatus As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Policy interface object
            Dim temp_oFindInsurance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindInsurance, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindInsurance = temp_oFindInsurance

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsurance.InsFileType = gSIRLibrary.SIRInsFileTypePolicy

            oFindInsurance.FindMode = 1
            ' Alix - Include policies attached to closed branch

            oFindInsurance.IncludeClosedBranches = True


            m_lReturn = oFindInsurance.Start
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                iStatus = oFindInsurance.Status

                'Retrieve BusinessTypeId to pass into coinsurance.
                If iStatus = gPMConstants.PMEReturnCode.PMOK Then

                    m_policyRef = oFindInsurance.InsReference.Trim()

                    m_lInsFolderCnt = oFindInsurance.InsuranceFolderCnt
                End If
            Else

                oFindInsurance.Dispose()
                oFindInsurance = Nothing
                ' Failed to get an instance of the object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            oFindInsurance.Dispose()
            oFindInsurance = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function LockPolicy() As Long

        Dim sLockedBy As String = String.Empty
        Dim oPMLock As Object = Nothing
        Const kMethodName As String = "LockPolicy"

        Try
            '   Find the Business Class
            m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=oPMLock,
                    sClassName:="bPMLock.User",
                vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oPMLock.LOCKKEY("insurance_folder_cnt",
                    vKeyValue:=m_lInsFolderCnt,
                    iUserID:=g_oObjectManager.UserID,
                    v_bOtherUserOnly:=False,
                    sCurrentlyLockedBy:=sLockedBy$)

            Select Case m_lReturn

                Case gPMConstants.PMEReturnCode.PMTrue
                    Return gPMConstants.PMEReturnCode.PMTrue

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If (sLockedBy$ = "ERROR") Then
                        gPMFunctions.RaiseError(kMethodName, "Error trying to lock record", gPMConstants.PMELogLevel.PMLogError)
                    Else

                        MsgBox("Policy currently locked by " & sLockedBy$ &
                                vbCrLf & "Please try later", , "Policy Lock")
                        ' Return false but dont raise exception rather we will like to show message and use PMFalse status to handle code from calling Subroutine
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    gPMFunctions.RaiseError(kMethodName, "Failed to lock the policy", gPMConstants.PMELogLevel.PMLogError)
            End Select

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally
            'Terminate the business object
            oPMLock.Dispose()
            oPMLock = Nothing
        End Try

    End Function

    'Author: Tariq Rashid
    ''' <summary>
    ''' Unlocks Current Policy so that it is available to other users.
    ''' </summary>
    ''' <returns>Returns gPMConstants.PMEReturnCode which is of base type long</returns>
    ''' <remarks></remarks>
    Private Function UnLockPolicy() As Long

        Dim oPMLock As Object = Nothing
        Const kMethodName As String = "UnLockPolicy"

        Try
            '   Find the Business Class
            m_lReturn = g_oObjectManager.GetInstance(
                    oObject:=oPMLock,
                    sClassName:="bPMLock.User",
                    vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'UnLock the Key
            m_lReturn = oPMLock.UNLOCKKEY("insurance_folder_cnt",
                                          vKeyValue:=m_lInsFolderCnt,
                                          iUserID:=g_oObjectManager.UserID)

            'Check for any error
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                gPMFunctions.RaiseError(kMethodName, "Failed to unlock the policy", gPMConstants.PMELogLevel.PMLogError)
            End If
            reComplete = True
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        Finally
            'Terminate the business object
            oPMLock.Dispose()
            oPMLock = Nothing
        End Try


    End Function

    Private Sub btnSuccessfullyRenewedOk_Click(sender As Object, e As EventArgs) Handles btnSuccessfullyRenewedOk.Click
        Me.Close()
    End Sub
End Class