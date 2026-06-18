Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129    
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctSummary_NET.uctSummary")> _
Partial Public Class uctSummary
    Inherits System.Windows.Forms.UserControl

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctSummary"

    ' System variables
    Private m_lReturn As Integer
    Private m_oBusiness As Object
    Private m_bHasChanged As Boolean
    Private m_bRefHasChanged As Boolean
    Private m_sRecoveryAgentPartyTypeCode As String = ""

    ' Private properties passed in
    Private m_cAmount As Decimal
    Private m_cArrears As Decimal
    Private m_lSourceID As Integer
    Private m_lPremFinanceCnt As Integer
    Private m_lPremFinanceVersion As Integer
    Private m_lPartyCnt As Integer
    Private m_lClaimDebtID As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lCompanyNo As Integer
    Private m_lSchemeNo As Integer
    Private m_lSchemeVersion As Integer
    Private m_lPFRF_ID As Integer
    Private m_lAgentCnt As Integer
    Private m_iProcess_Mode As gPMConstants.PMEComponentAction
    Private m_lMediaTypeID As Integer
    Private m_lPFFrequencyID As Integer

    ' Interface values
    Private m_sAgentName As String = ""
    Private m_sAgentRef As String = ""
    Private m_cBalance As Decimal
    Private m_sPlanStatus As String = ""
    Private m_cDepositAmount As Decimal
    Private m_cAdminAmount As Decimal
    Private m_cProtectionAmount As Decimal
    Private m_cInterestAmount As Decimal
    Private m_cTaxesAmount As Decimal
    Private m_dtFirstInstDate As Date
    Private m_dtNextInstDate As Date
    Private m_dtLastInstDate As Date
    Private m_cFirstInstAmount As Decimal
    Private m_cNextInstAmount As Decimal
    Private m_cLastInstAmount As Decimal

    ' Quote and Plan arrays
    Private m_vPFPlanArray(,) As Object
    Private m_vQuoteArray(,) As Object
    Private m_vMediaTypesArray(,) As Object

    ' Public Get properties

    ' Public Let properties
    <Browsable(True)> _
    Public Property Amount() As Decimal
        Get
            Return m_cAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cAmount = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property Arrears() As Decimal
        Get
            Return m_cArrears
        End Get
    End Property
    <Browsable(True)> _
    Public Property PremFinanceCnt() As Integer
        Get
            Return m_lPremFinanceCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPremFinanceCnt = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property PremFinanceVersion() As Integer
        Get
            Return m_lPremFinanceVersion
        End Get
        Set(ByVal Value As Integer)
            m_lPremFinanceVersion = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property ClaimDebtID() As Integer
        Get
            Return m_lClaimDebtID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimDebtID = Value
        End Set
    End Property
    <Browsable(True)> _
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property MediaTypeID() As Integer
        Get
            Return m_lMediaTypeID
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property PFFrequencyID() As Integer
        Get
            Return m_lPFFrequencyID
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property CompanyNo() As Integer
        Get
            Return m_lCompanyNo
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property SchemeNo() As Integer
        Get
            Return m_lSchemeNo
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property SchemeVersion() As Integer
        Get
            Return m_lSchemeVersion
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property PFRF_ID() As Integer
        Get
            Return m_lPFRF_ID
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property AgentCnt() As Integer
        Get
            Return m_lAgentCnt
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property Process_Mode() As Integer
        Get
            Return m_iProcess_Mode
        End Get
    End Property
    <Browsable(False)> _
    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Status() As String
        Get
            Return gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0))
        End Get
    End Property


    '*******************************************************************************
    ' Name: EditPlan
    ' Description: Puts the plan ni Edit mode
    ' Author: Alix Bergeret
    '*******************************************************************************
    Public Function EditPlan() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we are in view mode
            If m_iProcess_Mode = gPMConstants.PMEComponentAction.PMView Then
                ' If no plan currently loaded
                If m_lPremFinanceCnt = 0 Then
                    ' we switch to add mode
                    SwitchMode(gPMConstants.PMEComponentAction.PMAdd)
                Else
                    ' we edit the loaded plan
                    SwitchMode(gPMConstants.PMEComponentAction.PMEdit)

                    ' reset some fields, ready for a new quote!
                    m_cAmount = m_cBalance
                    m_cBalance = 0
                    m_cArrears = 0
                    m_sPlanStatus = ""
                    m_cDepositAmount = 0
                    m_cAdminAmount = 0
                    m_cProtectionAmount = 0
                    m_cInterestAmount = 0
                    m_cTaxesAmount = 0
                    m_dtFirstInstDate = CDate("00:00:00")
                    m_cFirstInstAmount = 0
                    m_dtNextInstDate = CDate("00:00:00")
                    m_cNextInstAmount = 0
                    m_dtLastInstDate = CDate("00:00:00")
                    m_cLastInstAmount = 0
                    m_cArrears = 0
                    m_lPFRF_ID = 0

                    ' update interface
                    DataToInterface()

                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditPlan failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditPlan", excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: LoadPlan
    ' Description: Load the plan specified by the instalments PK
    ' Author: Alix Bergeret
    '*******************************************************************************
    Public Function LoadPlan() As Integer
        Dim result As Integer = 0
        Dim cRefund As Decimal
        Dim oFindAgentBusiness As bSIRFindParty.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have sufficient data to load a plan
            If m_lClaimDebtID = 0 And m_lPremFinanceCnt = 0 Then

                '****************
                ' MEvans : 12-06-2003 : 223
                ' if we dont have enough information to load a plan
                ' then switch into add mode..
                SwitchMode(gPMConstants.PMEComponentAction.PMAdd)
                '****************

                '        LoadPlan = PMFail
                '        Call LogMessagePopup(iType:=PMLogOnError, _
                ''                             sMsg:="ClaimDebtID or PremFinanceCnt must be passed in in order to load a plan.", _
                ''                             vApp:=ACApp, _
                ''                             vClass:=ACClass, _
                ''                             vMethod:="LoadPlan", _
                ''                             vErrNo:=Err.Number, _
                ''                             vErrDesc:=Err.Description)
                Return result
            End If

            ' Load a plan, by claim debt ID if available, by plan count and version if not
            If m_lClaimDebtID > 0 Then

                m_lReturn = m_oBusiness.GetSingleFinancePlanFromClaimDebtID(v_lClaimDebtID:=m_lClaimDebtID, r_vPFPremiumFinance:=m_vPFPlanArray)
            Else

                m_lReturn = m_oBusiness.GetSingleFinancePlan(v_lFinanceCount:=m_lPremFinanceCnt, v_lFinanceVersion:=m_lPremFinanceVersion, r_vPFPremiumFinance:=m_vPFPlanArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '****************
                ' MEvans : 12-06-2003 : 223
                ' if the plan fails to load put the control into add mode..
                SwitchMode(gPMConstants.PMEComponentAction.PMAdd)
                Return result
                '****************

                '        LoadPlan = PMFalse
                '        Call LogMessagePopup(iType:=PMLogOnError, _
                ''                             sMsg:="Failed to load plan.", _
                ''                             vApp:=ACApp, _
                ''                             vClass:=ACClass, _
                ''                             vMethod:="LoadPlan", _
                ''                             vErrNo:=Err.Number, _
                ''                             vErrDesc:=Err.Description)
                '        Exit Function
            End If

            ' Calculate what has already been paid for this plan

            m_lReturn = m_oBusiness.SettlePlanCalculate(v_vPremiumFinance:=m_vPFPlanArray, r_crSettlement:=m_cBalance, r_crRefund:=cRefund)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to calculate balance.", ACApp, ACClass, "LoadPlan", Information.Err().Number, Information.Err().Description)
                oFindAgentBusiness = Nothing
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Calculate arrears for this plan

            m_lReturn = m_oBusiness.GetArrears(v_lPremFinancePlan_Cnt:=m_lPremFinanceCnt, v_lPremFinancePlan_Ver:=m_lPremFinanceVersion, r_cArrearsAmount:=m_cArrears)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to calculate Arrears.", ACApp, ACClass, "LoadPlan", Information.Err().Number, Information.Err().Description)
                oFindAgentBusiness = Nothing
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Update global variables
            m_cAmount = m_cBalance
            m_lPartyCnt = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientId, 0))))
            m_lInsuranceFileCnt = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0))))
            m_lCompanyNo = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0))))
            m_lSchemeNo = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0))))
            m_lSchemeVersion = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSchemeVersion, 0))))
            m_lPFRF_ID = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanPFRF_ID, 0))))
            m_lAgentCnt = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0))))
            m_lMediaTypeID = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanMediaType_ID, 0))))
            m_lPFFrequencyID = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanPfFrequency_ID, 0))))
            m_lAgentCnt = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0))))
            m_sAgentRef = gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0))

            ' Copy some values to screens
            m_cAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAmountToFinance, 0))
            m_dtFirstInstDate = CDate(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0))
            m_dtNextInstDate = CDate(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanNextInstalmentdate, 0))
            m_dtLastInstDate = CDate(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanLastInstalmentdate, 0))
            m_cFirstInstAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFirstInstalment, 0))
            m_cNextInstAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanOtherInstalments, 0))
            m_cLastInstAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanLastInstalment, 0))
            m_cInterestAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanInterestCost, 0))
            m_cDepositAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0))
            m_cTaxesAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanTaxCost, 0))
            m_cAdminAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFinanceCharge, 0))
            m_cProtectionAmount = CDec(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanCostOfProtection, 0))

            ' We need to load agent's name separatly since it is not saved in plan
            If m_lAgentCnt <> 0 Then
                Dim temp_oFindAgentBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oFindAgentBusiness, "bSIRFindParty.Business", gPMConstants.PMGetViaClientManager)
                oFindAgentBusiness = temp_oFindAgentBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create following object: bSIRFindParty.Business.", ACApp, ACClass, "LoadPlan", Information.Err().Number, Information.Err().Description)
                    oFindAgentBusiness = Nothing
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                m_lReturn = oFindAgentBusiness.GetResolvedName(lPartyCnt:=m_lAgentCnt, sPartyResolvedName:=m_sAgentName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to load agent's name.", ACApp, ACClass, "LoadPlan", Information.Err().Number, Information.Err().Description)
                    oFindAgentBusiness = Nothing
                    Return gPMConstants.PMEReturnCode.PMFail
                End If
                oFindAgentBusiness = Nothing
            Else
                m_sAgentName = ""
            End If

            ' Init user inputs
            For iIndex As Integer = 0 To cboMediaType.Items.Count - 1
                If VB6.GetItemData(cboMediaType, iIndex) = m_lMediaTypeID Then
                    cboMediaType.SelectedIndex = iIndex
                End If
            Next iIndex
            For iIndex As Integer = 0 To cboFrequency.Items.Count - 1
                If VB6.GetItemData(cboFrequency, iIndex) = m_lPFFrequencyID Then
                    cboFrequency.SelectedIndex = iIndex
                End If
            Next iIndex
            txtInstalment.Text = gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanOtherInstalments, 0))
            txtFirstPayment.Text = gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0))
            If gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFrequencyPeriod, 0)) = "m" Then
                cboWeekday.SelectedIndex = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDayOfWeekOrMonth, 0))) - 1)
            Else
                cboDayInMonth.SelectedIndex = gPMFunctions.ToSafeInteger(Conversion.Val(gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDayOfWeekOrMonth, 0))) - 1)
            End If

            ' Set status to live
            m_sPlanStatus = "Live"

            ' Update interface
            DataToInterface()

            ' Switch to view mode
            SwitchMode(gPMConstants.PMEComponentAction.PMView)

            ' Reset flags
            m_bHasChanged = False
            m_bRefHasChanged = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadPlan failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPlan", excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: SavePlan
    ' Description: Saves the user selection as a plan in PFPremiumFinance
    ' Author: Alix Bergeret
    '*******************************************************************************
    Public Function SavePlan() As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If view mode, exit
            If m_iProcess_Mode = gPMConstants.PMEComponentAction.PMView Then

                'Developer Guide No 243
                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 813, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            ' If not quote, cant save!
            If m_sPlanStatus = "" Then
                If Not m_bHasChanged And m_bRefHasChanged Then
                    ' Only ref has changed, we can still save
                Else

                    'developer guide no 243
                    sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 814, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    MessageBox.Show(sMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
            End If

            ' If we are going to insert a new version of an existing plan, we warn user
            If m_lPremFinanceCnt <> 0 And m_bHasChanged Then
                ' Ask user if he agrees to insert a new version of the plan

                'developer guide no. 243
                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 804, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                If MessageBox.Show(sMessage, "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) <> System.Windows.Forms.DialogResult.Yes Then
                    Return result
                End If
            End If

            ' Populate plan array, unless already populated
            If m_sPlanStatus = "Quote" Or m_bHasChanged Then
                m_lReturn = PopulatePlanArray()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate finance plan array.", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePlan", excep:=New Exception(Information.Err().Description))
                    Return result
                End If
            End If

            ' If agentcnt=0 then null it, to avoid breaking Foreign Key
            If gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0)) = "0" Then

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0) = DBNull.Value
            End If

            ' Save plan
            If m_lPremFinanceCnt = 0 Then
                ' Insert new plan

                m_lReturn = m_oBusiness.InsertNewPFPlan(r_vPFPlanArray:=m_vPFPlanArray, r_lPFPlanCnt:=m_lPremFinanceCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create new finance plan.", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePlan", excep:=New Exception(Information.Err().Description))
                    Return result
                End If

                ' Assume version is 1
                m_lPremFinanceVersion = 1

                ' Open maintenance form
                m_lReturn = OpenMaintenanceForm()

                ' DEBUG ONLY - Create instalments

                m_lReturn = m_oBusiness.TransactPlanInHouse(m_vPFPlanArray, "", 0)

            Else
                ' If only ref has changed, then update existing record
                If Not m_bHasChanged And m_bRefHasChanged Then

                    ' Insert new ref in array
                    m_sAgentRef = txtReference.Text.Trim()
                    m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0) = m_sAgentRef

                    ' Update record

                    m_lReturn = m_oBusiness.UpdateExistingRecord(vExistingRecord:=m_vPFPlanArray, vPremiumFinanceCnt:=m_lPremFinanceCnt, vPremiumFinanceVersion:=m_lPremFinanceVersion)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update existing plan.", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePlan", excep:=New Exception(Information.Err().Description))
                        Return result
                    End If

                    ' Open maintenance form
                    m_lReturn = OpenMaintenanceForm()

                    ' Else, we create a new version of plan
                Else

                    ' Date review and date confirmed are blanked

                    m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDateConfirmed, 0) = DBNull.Value

                    m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDateReview, 0) = DBNull.Value

                    ' Open maintenance form
                    m_lReturn = OpenMaintenanceForm()

                    ' Insert new version of existing plan

                    m_lReturn = m_oBusiness.InsertSupercedingPlan(r_vNewPFPlan:=m_vPFPlanArray, v_lPremiumFinanceCnt:=m_lPremFinanceCnt, r_lPremiumFinanceVersion:=m_lPremFinanceVersion, v_vTransDetailArray:="")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create new version of existing plan.", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePlan", excep:=New Exception(Information.Err().Description))
                        Return result
                    End If
                End If
            End If

            ' Reset haschanged flag
            m_bHasChanged = False
            m_bRefHasChanged = False

            ' Reload plan we have just saved
            m_lReturn = LoadPlan()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SavePlan failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SavePlan", excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: CancelEdit
    ' Description: Cancels the edit mode
    ' Author: Alix Bergeret
    '*******************************************************************************
    Public Function CancelEdit(Optional ByVal v_bPrompt As Boolean = True) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""
        Dim iResponse As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if already in view mode, we dont do anything
            If m_iProcess_Mode = gPMConstants.PMEComponentAction.PMView Then
                Return result
            End If

            ' Prompt if user has entered any information
            If v_bPrompt And m_bHasChanged Then
                ' Get mesasge box message from resource file

                'developer guide no. 243
                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 803, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                iResponse = MessageBox.Show(sMessage, "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            Else
                iResponse = System.Windows.Forms.DialogResult.Yes
            End If

            ' If user said YES
            If iResponse = System.Windows.Forms.DialogResult.Yes Then

                If m_lPremFinanceCnt <> 0 Then
                    ' If plan is loaded, then reload it
                    m_lReturn = LoadPlan()
                Else
                    ' We just blanck all the fields
                    BlankDataAndInterface()
                End If

                ' switch to view mode
                SwitchMode(gPMConstants.PMEComponentAction.PMView)

            Else
                Return result
            End If

            ' reset flag
            m_bHasChanged = False
            m_bRefHasChanged = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelEdit failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelEdit", excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: Initialise
    ' Description: To be called before using the control
    ' Author: Alix Bergeret
    '*******************************************************************************
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String
        Dim sRecoveryAgentPartyTypeID As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            'Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to " & "initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            ' Store the language ID from the object manager to the private variables
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername = .UserName
            End With

            'Get an instance of the business object via the public object manager
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPremiumFinance.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                'developer guide no. 243
                sTitle = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 702, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 703, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            ' Default the interface
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to default interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' Get the RecoveryAgentPartyType System Option
            m_lReturn = iPMFunc.GetSystemOption(1028, sRecoveryAgentPartyTypeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not get System Option: " & "RecoveryAgentPartyType", ACApp, ACClass, "Initialise")
                Return result
            End If

            ' Get RecoveryAgentPartyTypeID

            m_lReturn = m_oBusiness.GetPartyTypeCode(sRecoveryAgentPartyTypeID, m_sRecoveryAgentPartyTypeCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogError, "Could not get Party Type Code for: " & sRecoveryAgentPartyTypeID, ACApp, ACClass, "Initialise")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: SetInterfaceDefaults
    ' Description: Init controls
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' By default, set to View mode
            SwitchMode(gPMConstants.PMEComponentAction.PMView)

            ' populate day of week combo
            With cboWeekday
                'Developer Guide No 243
                .Items.Insert(0, gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 704, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
                .Items.Insert(1, gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 705, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
                .Items.Insert(2, gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 706, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
                .Items.Insert(3, gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 707, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))
                .Items.Insert(4, gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 708, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)))

                .SelectedIndex = 0
            End With

            ' Populate day of month combo
            For iCount As Integer = 1 To 31
                cboDayInMonth.Items.Insert(iCount - 1, gPMFunctions.ToSafeString(iCount))
            Next iCount
            cboDayInMonth.SelectedIndex = 0

            ' Populate frequency and media type combos
            GetMediaTypes()
            cboFrequency.Items.Clear()

            ' Enable or disable agent button
            EnableAgentButton()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", excep:=excep)

            Return result
        End Try
    End Function


    Private isInitializingComponent As Boolean
    Private Sub cboDayInMonth_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDayInMonth.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bHasChanged = True
    End Sub


    Private Sub cboFrequency_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFrequency.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bHasChanged = True
    End Sub


    Private Sub cboMediaType_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaType.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        m_bHasChanged = True
        EnableAgentButton()

        If cboMediaType.SelectedIndex >= 0 Then
            GetFrequencies(VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex))
        End If

    End Sub

    Private Sub cboMediaType_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaType.SelectionChangeCommitted

        m_bHasChanged = True
        EnableAgentButton()

        If cboMediaType.SelectedIndex >= 0 Then
            GetFrequencies(VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex))
        End If

    End Sub


    Private Sub cboWeekday_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboWeekday.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bHasChanged = True
    End Sub

    Private Sub cmdAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgent.Click

        Try

            ' Select agent
            m_lReturn = SelectAgent()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Display agent name and ref
            txtAgent.Text = m_sAgentName
            txtReference.Text = m_sAgentRef

            ' Save to array
            If Information.IsArray(m_vPFPlanArray) Then
                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0) = m_lAgentCnt
                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0) = m_sAgentRef
            End If

            ' Set flag
            m_bHasChanged = True

        Catch
        End Try




    End Sub

    Private Sub cmdCalculate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCalculate.Click
        m_bHasChanged = True
        CalculateSingleQuote()
    End Sub

    Private Sub txtFirstPayment_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstPayment.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bHasChanged = True
    End Sub

    Private Sub txtFirstPayment_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFirstPayment.DoubleClick
        'developer guide no. 40
        txtFirstPayment.Text = DateTime.Today
    End Sub

    Private Sub txtInstalment_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInstalment.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bHasChanged = True
    End Sub

    'Private Sub txtInstalment_KeyPress(KeyAscii As Integer)
    ''    If KeyAscii = 13 Then
    ''
    ''    End If
    'End Sub

    Private Sub txtReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bRefHasChanged = True
    End Sub

    ' Control events
    Private Sub UserControl_Initialize()

    End Sub

    Private Sub UserControl_Terminate()
        m_oBusiness = Nothing
    End Sub

    '*******************************************************************************
    ' Name:         GetFrequencies
    ' Description:  Populate the frequency combo list with all available frequencies
    '               for a given mediatypeID
    ' Author:       Alix Bergeret
    '*******************************************************************************
    Private Function GetFrequencies(ByVal v_lMediaTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim bFoundOne As Boolean
        Dim sMessage As String = ""

        Try

            ' Validate inputs
            If v_lMediaTypeID <= 0 Or Not Information.IsArray(m_vMediaTypesArray) Then
                Return result
            End If

            ' Loop through them and add to combo
            With cboFrequency
                .Items.Clear()
                bFoundOne = False
                For nRow As Integer = m_vMediaTypesArray.GetLowerBound(1) To m_vMediaTypesArray.GetUpperBound(1)
                    ' Only add if this frequency is available for this mediatype

                    If gPMFunctions.ToSafeInteger(m_vMediaTypesArray(0, nRow)) = v_lMediaTypeID And Not (Convert.IsDBNull(m_vMediaTypesArray(3, nRow)) Or IsNothing(m_vMediaTypesArray(3, nRow))) Then
                        .Items.Add(gPMFunctions.ToSafeString(m_vMediaTypesArray(3, nRow)))

                        VB6.SetItemData(cboFrequency, "NewIndex", gPMFunctions.ToSafeInteger(m_vMediaTypesArray(2, nRow)))
                        bFoundOne = True
                    End If
                Next nRow
            End With

            ' If none found, warn user
            If Not bFoundOne Then

                'Developer Guide No 243
                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 812, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, "No Frequency available", MessageBoxButtons.OK)
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFrequencies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFrequencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: GetMediaTypes
    ' Description: Get mediatypes from DB
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function GetMediaTypes() As Integer

        Dim result As Integer = 0
        Dim lPrevID As Integer

        Try

            ' Get all mediatypes from DB

            m_lReturn = m_oBusiness.GetMediaTypesAndFrequencies(m_vMediaTypesArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            lPrevID = 0
            With cboMediaType
                .Items.Clear()
                ' Loop through all media types
                For nRow As Integer = m_vMediaTypesArray.GetLowerBound(1) To m_vMediaTypesArray.GetUpperBound(1)
                    ' Get ride of doubles
                    If lPrevID <> gPMFunctions.ToSafeInteger(m_vMediaTypesArray(0, nRow)) Then
                        .Items.Add(gPMFunctions.ToSafeString(m_vMediaTypesArray(1, nRow)).Trim())

                        VB6.SetItemData(cboMediaType, "NewIndex", gPMFunctions.ToSafeInteger(m_vMediaTypesArray(0, nRow)))
                        lPrevID = gPMFunctions.ToSafeInteger(m_vMediaTypesArray(0, nRow))
                    End If
                Next nRow
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: SwitchMode
    ' Description: Used to switch between PMAdd, PMView and PMEdit modes
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function SwitchMode(ByVal v_iMode As Integer) As Integer

        Dim result As Integer = 0
        Try

            m_iProcess_Mode = v_iMode

            Select Case v_iMode
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
                    ' We enable controls
                    cboWeekday.Enabled = True
                    cboDayInMonth.Enabled = True
                    txtFirstPayment.Enabled = True
                    cboMediaType.Enabled = True
                    cboFrequency.Enabled = True
                    txtInstalment.Enabled = True
                    cmdAgent.Enabled = True
                    txtReference.Enabled = True
                    ' disable agent button if wrong media type
                    EnableAgentButton()
                Case Else
                    ' View mode, we lock controls
                    cboWeekday.Enabled = False
                    cboDayInMonth.Enabled = False
                    txtFirstPayment.Enabled = False
                    cboMediaType.Enabled = False
                    cboFrequency.Enabled = False
                    txtInstalment.Enabled = False
                    cmdAgent.Enabled = False
                    txtReference.Enabled = False
            End Select


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchMode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchMode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: BlankDataAndInterface
    ' Description: Blank interface and matching variables
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function BlankDataAndInterface() As Integer

        Dim result As Integer = 0
        Try

            ' Blank controls
            cboWeekday.SelectedIndex = 0
            cboDayInMonth.SelectedIndex = 0
            txtFirstPayment.Text = ""
            cboMediaType.SelectedIndex = -1
            cboFrequency.SelectedIndex = -1
            txtInstalment.Text = ""

            txtAgent.Text = ""
            txtReference.Text = ""
            txtOriginalDebt.Text = ""
            txtArrears.Text = ""
            txtBalance.Text = ""
            txtPlanStatus.Text = ""
            txtDeposit.Text = ""
            txtAdminCharge.Text = ""
            txtProtection.Text = ""
            txtInterest.Text = ""
            txtTaxes.Text = ""
            txtFirstInstalmentDate.Text = ""
            txtFirstInstAmount.Text = ""
            txtNextInstalmentDate.Text = ""
            txtOtherInstAmount.Text = ""
            txtLastInstalmentDate.Text = ""
            txtLastInstAmount.Text = ""

            ' Blank local variable
            m_bHasChanged = False
            m_bRefHasChanged = False
            m_cAmount = 0
            m_cArrears = 0
            m_lPremFinanceCnt = 0
            m_lPremFinanceVersion = 0
            m_lPartyCnt = 0
            m_lClaimDebtID = 0
            m_lInsuranceFileCnt = 0
            m_lCompanyNo = 0
            m_lSchemeNo = 0
            m_lSchemeVersion = 0
            m_lPFRF_ID = 0
            m_lAgentCnt = 0

            m_sAgentName = ""
            m_sAgentRef = ""
            m_cBalance = 0
            m_sPlanStatus = ""
            m_cDepositAmount = 0
            m_cAdminAmount = 0
            m_cProtectionAmount = 0
            m_cInterestAmount = 0
            m_cTaxesAmount = 0
            m_dtFirstInstDate = CDate("00:00:00")
            m_dtNextInstDate = CDate("00:00:00")
            m_dtLastInstDate = CDate("00:00:00")
            m_cFirstInstAmount = 0
            m_cNextInstAmount = 0
            m_cLastInstAmount = 0


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BlankDataAndInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BlankDataAndInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: CalculateSingleQuote
    ' Description: Uses user input to calculate a quote
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function CalculateSingleQuote() As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' First we checks if we have sufficient and valid data
            If ValidateUserInput(sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show(sMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

            ' Disable user inputs
            SwitchMode(gPMConstants.PMEComponentAction.PMView)

            ' Update status
            If m_sPlanStatus = "" Then
                m_sPlanStatus = "Quote"
                m_cBalance = 0
                m_cArrears = 0
                DataToInterface()
            End If

            ' then we make the call to the business object

            'developer guide no. 98
            m_lReturn = m_oBusiness.CalculateSingleQuote(v_dtPreferredDate:=txtFirstPayment.Text, v_iDayInMonth:=cboDayInMonth.SelectedIndex + 1, v_iDayInWeek:=cboWeekday.SelectedIndex + 1, v_lMediaTypeID:=VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex), v_lPFFrequencyID:=VB6.GetItemData(cboFrequency, cboFrequency.SelectedIndex), v_cTotalAmount:=m_cAmount, v_cInstalmentAmount:=txtInstalment.Text, v_sProductCode:="TPR", r_vQuoteArray:=m_vQuoteArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateSingleQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateSingleQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Re-enable user inputs
                SwitchMode(gPMConstants.PMEComponentAction.PMEdit)
                txtInstalment.Focus()
                Return result
            End If

            ' display some values of the quote on the screen
            If Information.IsArray(m_vQuoteArray) Then

                ' Populate global variables and display them
                m_dtFirstInstDate = CDate(m_vQuoteArray(k_PFQuoteFirstInstalmentDate, 0))
                m_dtNextInstDate = CDate(m_vQuoteArray(k_PFQuoteNextInstalmentDate, 0))
                m_dtLastInstDate = CDate(m_vQuoteArray(k_PFQuoteLastInstalmentDate, 0))
                m_cFirstInstAmount = CDec(m_vQuoteArray(k_PFQuoteFirstInstalmentAmount, 0))
                m_cNextInstAmount = CDec(m_vQuoteArray(k_PFQuoteOtherInstalmentAmount, 0))
                m_cLastInstAmount = CDec(m_vQuoteArray(k_PFQuoteLastInstalmentAmount, 0))
                m_cInterestAmount = CDec(m_vQuoteArray(k_PFQuoteAprRate, 0))
                m_cDepositAmount = CDec(m_vQuoteArray(k_PFQuoteDepositAmount, 0))
                m_cTaxesAmount = CDec(m_vQuoteArray(k_PFQuoteTaxAmount, 0))
                m_cAdminAmount = CDec(m_vQuoteArray(k_PFQuoteFinanceCharge, 0))
                m_cProtectionAmount = CDec(m_vQuoteArray(k_PFQuoteProtectionAmount, 0))
                DataToInterface()

                ' Also populate other global variables
                m_lCompanyNo = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteCompanyNo, 0))
                m_lSchemeNo = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteSchemeNo, 0))
                m_lSchemeVersion = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteSchemeVersion, 0))
                m_lPFRF_ID = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuotePFRF_ID, 0))
                m_lMediaTypeID = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteMediaTypeID, 0))
                m_lPFFrequencyID = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteFrequencyID, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateSingleQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateSingleQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Re-enable user inputs
                SwitchMode(gPMConstants.PMEComponentAction.PMEdit)
                txtInstalment.Focus()

                Return result
            End If

            ' Re-enable user inputs
            SwitchMode(gPMConstants.PMEComponentAction.PMEdit)
            txtInstalment.Focus()

            Return result

        Catch excep As System.Exception



            ' Re-enable user inputs
            SwitchMode(gPMConstants.PMEComponentAction.PMEdit)
            txtInstalment.Focus()

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateSingleQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateSingleQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: ValidateUserInput
    ' Description: Checks if user inputs are valid and sufficient to precoeed with quote
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function ValidateUserInput(ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Day of week
            If cboWeekday.SelectedIndex = -1 Then
                r_sMessage = "Please select weekday."

                'Developer Guide No 243
                r_sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 805, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Day of month
            If cboDayInMonth.SelectedIndex = -1 Then
                r_sMessage = "Please select day in month."

                'Developer Guide No 243
                r_sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 806, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Preferred date of first payment
            If Not Information.IsDate(txtFirstPayment.Text) Then
                r_sMessage = "Please specify a preferred date of first payment."

                'Developer Guide No 243
                r_sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 807, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Media type
            If cboMediaType.SelectedIndex = -1 Then
                r_sMessage = "Please select media type."

                'Developer Guide No 243
                r_sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 808, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Frequency
            If cboFrequency.SelectedIndex = -1 Then
                r_sMessage = "Please select frequency."

                'Developer Guide No 243
                r_sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 809, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' instalments amount
            Dim dbNumericTemp As Double
            If Not Double.TryParse(txtInstalment.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Or Conversion.Val(txtInstalment.Text) = 0 Then
                r_sMessage = "Please specify amount of instalments."

                'Developer Guide No 243
                r_sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 810, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateUserInput Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateUserInput", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: PopulatePlanArray
    ' Description: -
    ' Author: Alix Bergeret
    '*******************************************************************************
    Private Function PopulatePlanArray() As Integer

        Dim result As Integer = 0
        Dim vClientDetailsArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Resize plan array
            ReDim m_vPFPlanArray(bSIRPremFinConst.k_PFPlanCountOfFields, 0)

            ' Populate
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanPFCnt, 0) = m_lPremFinanceCnt
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanPFVersion, 0) = 0
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanStatusInd, 0) = bSIRPremFinConst.PFStatusIndSaved

            ' Details from quote array
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSchemeNo, 0) = m_vQuoteArray(k_PFQuoteSchemeNo, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanCompanyNo, 0) = m_vQuoteArray(k_PFQuoteCompanyNo, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanCompanyName, 0) = m_vQuoteArray(k_PFQuoteCompanyName, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSchemeVersion, 0) = m_vQuoteArray(k_PFQuoteSchemeVersion, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSchemeName, 0) = m_vQuoteArray(k_PFQuoteSchemeName, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanPfFrequency_ID, 0) = m_vQuoteArray(k_PFQuoteFrequencyID, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanMediaType_ID, 0) = m_vQuoteArray(k_PFQuoteMediaTypeID, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanProductClass, 0) = m_vQuoteArray(k_PFQuoteProductClass, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAmountToFinance, 0) = m_vQuoteArray(k_PFQuoteTotalAmountInput, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanNoOfInstalments, 0) = m_vQuoteArray(k_PFQuoteInstalmentsToPay, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFirstInstalmentdate, 0) = m_vQuoteArray(k_PFQuoteFirstInstalmentDate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanNextInstalmentdate, 0) = m_vQuoteArray(k_PFQuoteNextInstalmentDate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanLastInstalmentdate, 0) = m_vQuoteArray(k_PFQuoteLastInstalmentDate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFirstInstalment, 0) = m_vQuoteArray(k_PFQuoteFirstInstalmentAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanOtherInstalments, 0) = m_vQuoteArray(k_PFQuoteOtherInstalmentAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanLastInstalment, 0) = m_vQuoteArray(k_PFQuoteLastInstalmentAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanTotalCost, 0) = m_vQuoteArray(k_PFQuoteTotalInstalmentsAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAPR, 0) = m_vQuoteArray(k_PFQuoteAprRate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanInterestRate, 0) = m_vQuoteArray(k_PFQuoteInterestRate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDeposit, 0) = m_vQuoteArray(k_PFQuoteDepositAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanInterestCost, 0) = m_vQuoteArray(k_PFQuoteInterestAmount, 0)

            ' Claculate Net amount (AmountToFinance less Deposit)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanNetAmount, 0) = CDbl(m_vQuoteArray(k_PFQuoteTotalAmountInput, 0)) - CDbl(m_vQuoteArray(k_PFQuoteDepositAmount, 0))

            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanTaxCost, 0) = m_vQuoteArray(k_PFQuoteTaxAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanCostOfProtection, 0) = m_vQuoteArray(k_PFQuoteProtectionAmount, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDaysDelay, 0) = m_vQuoteArray(k_PFQuoteDaysDelay, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanMediaTypeValidation, 0) = m_vQuoteArray(k_PFQuoteMediaTypeValidation, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSchemeType_ID, 0) = m_vQuoteArray(k_PFQuoteSchemeTypeCode, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanPFRF_ID, 0) = m_vQuoteArray(k_PFQuotePFRF_ID, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFinanceCharge, 0) = m_vQuoteArray(k_PFQuoteFinanceCharge, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFrequencyPeriod, 0) = m_vQuoteArray(k_PFQuoteFrequencyPeriod, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFrequencyAmount, 0) = m_vQuoteArray(k_PFQuoteFrequencyAmount, 0)

            ' Local details
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanSource_ID, 0) = m_lSourceID
            'm_vPFPlanArray(k_PFPlanProduct_ID, 0) = m_lProductID
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanStartDate, 0) = m_vQuoteArray(k_PFQuoteFirstInstalmentDate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanEndDate, 0) = m_vQuoteArray(k_PFQuoteLastInstalmentDate, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanTransactionType, 0) = m_vQuoteArray(k_PFQuoteProductCode, 0)
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanInsuranceFileCnt, 0) = m_lInsuranceFileCnt
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientId, 0) = m_lPartyCnt
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanIsQuote, 0) = 1
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanInterestFree, 0) = "N"
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentCnt, 0) = m_lAgentCnt
            m_sAgentRef = txtReference.Text.Trim()
            m_vPFPlanArray(bSIRPremFinConst.k_PFPlanAgentRef, 0) = m_sAgentRef

            If gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFrequencyPeriod, 0)) = "w" Then
                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDayOfWeekOrMonth, 0) = cboWeekday.SelectedIndex + 1
            ElseIf gPMFunctions.ToSafeString(m_vPFPlanArray(bSIRPremFinConst.k_PFPlanFrequencyPeriod, 0)) = "m" Then
                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanDayOfWeekOrMonth, 0) = cboDayInMonth.SelectedIndex + 1
            End If

            ' Client Details

            m_oBusiness.PopulateClientDetails(vClientDetailsArray, m_lPartyCnt)
            If Information.IsArray(vClientDetailsArray) Then

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientCode, 0) = vClientDetailsArray(1, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientName, 0) = vClientDetailsArray(6, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientAddress1, 0) = vClientDetailsArray(7, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientAddress2, 0) = vClientDetailsArray(8, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientAddress3, 0) = vClientDetailsArray(9, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientAddress4, 0) = vClientDetailsArray(10, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientPostcode, 0) = vClientDetailsArray(11, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientAreaCode, 0) = vClientDetailsArray(12, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientPhone, 0) = vClientDetailsArray(13, 0)

                m_vPFPlanArray(bSIRPremFinConst.k_PFPlanClientExtn, 0) = vClientDetailsArray(14, 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulatePlanArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePlanArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*************************************************************************
    'Name:          OpenMaintenanceForm
    'Description:   Passes properties and dislays the PF Maint screen
    '               If the user cancels this screen, then PF is deleted
    'History:       Alix - Copied from uctInstalments on 10/04/2003
    '*************************************************************************
    Public Function OpenMaintenanceForm() As Integer

        Dim result As Integer = 0

        'developer guide no. 88
        Dim oPFInterface As Object
        Dim lReturn As Integer


        Try

            ' Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the Maintenance Form
            Dim temp_oPFInterface As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oPFInterface, "iPMBFinancePlanMaint.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oPFInterface = temp_oPFInterface
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the properties that the form needs to know
            With oPFInterface

                .FinancePlanCnt = m_lPremFinanceCnt

                .FinancePlanVersion = m_lPremFinanceVersion

                .PartyCnt = m_lPartyCnt

                .Spawned = True
                ' Display the form

                lReturn = .Start()
            End With
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_lReturn = LoadPlan()
            End If

            'Clean up

            oPFInterface.Dispose()
            oPFInterface = Nothing



            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error during Display Main Screen", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenMaintenanceForm", excep:=excep)
            Return result

            'developer guide no 32. 

            Return result
        End Try
    End Function

    '*******************************************************************************
    ' Name: ClearAll
    ' Description: Resets everything on order to start a new plan
    ' Author: Alix Bergeret
    '*******************************************************************************
    Public Function ClearAll() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear everything
            BlankDataAndInterface()

            ' Switch to view mode
            SwitchMode(gPMConstants.PMEComponentAction.PMView)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAll failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearAll", excep:=excep)

            Return result
        End Try
    End Function

    '*************************************************************************
    'Name:          SelectAgent
    'Description:   Shows the Party search screen for the type of Party
    '               stated in System Option "RecoveryAgentPartyTypeCode"
    'History:       Alix - 11/04/2003 - Copied from iPMBFinancePlanMaint
    '*************************************************************************
    Private Function SelectAgent() As Integer
        Dim result As Integer = 0
        'developer guide no. 88
        Dim oFindAgent As Object
        Dim oFindAgentBusiness As bSIRFindParty.Business
        Dim vKeyArray(,) As Object
        Dim sAddress1 As String = String.Empty
        Dim sAddress2 As String = String.Empty
        Dim sAddress3 As String = String.Empty
        Dim sAddress4 As String = String.Empty
        Dim sPostalCode As String = String.Empty
        Dim sMessage As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Find Party object
            Dim temp_oFindAgent As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAgent, "iPMBFindParty.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oFindAgent = temp_oFindAgent
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not display Find Agent Screen", Application.ProductName)
                oFindAgent = Nothing
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Set the process modes

            oFindAgent.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=m_iProcess_Mode, vTransactionType:="", vEffectiveDate:=DateTime.Today)

            ' Set the properties.
            ReDim vKeyArray(1, 0)

            vKeyArray(0, 0) = "special_party"

            vKeyArray(1, 0) = m_sRecoveryAgentPartyTypeCode

            m_lReturn = oFindAgent.SetKeys(vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not display Find Agent Screen", Application.ProductName)
                oFindAgent = Nothing
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            oFindAgent.CallingAppName = ACApp

            ' Display the search screen

            m_lReturn = oFindAgent.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Could not display Find Agent Screen", Application.ProductName)
                Return result
            End If

            ' Make sure that the search screen exited ok & user did not cancel

            If oFindAgent.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Save selected Agent Cnt to the array holding the rest of
                ' the plans details (but not to the db)

                m_lAgentCnt = oFindAgent.PartyCnt

                m_sAgentRef = oFindAgent.ShortName

                m_sAgentName = oFindAgent.LongName

                ' Display agent name and ref
                txtAgent.Text = m_sAgentName
                txtReference.Text = m_sAgentRef

                ' Check if agent's address is sufficient
                Dim temp_oFindAgentBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oFindAgentBusiness, "bSIRFindParty.Business", gPMConstants.PMGetViaClientManager)
                oFindAgentBusiness = temp_oFindAgentBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to create following object: bSIRFindParty.Business.", ACApp, ACClass, "SelectAgent", Information.Err().Number, Information.Err().Description)
                    oFindAgentBusiness = Nothing
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                m_lReturn = oFindAgentBusiness.GetFullAddress(v_lPartyCnt:=m_lAgentCnt, r_vAddress1:=sAddress1, r_vAddress2:=sAddress2, r_vAddress3:=sAddress3, r_vAddress4:=sAddress4, r_vPostalCode:=sPostalCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to load agent's address.", ACApp, ACClass, "SelectAgent", Information.Err().Number, Information.Err().Description)
                    oFindAgentBusiness = Nothing
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                If sAddress1.Trim() = "" Or sAddress2.Trim() = "" Then
                    ' address is insufficient, warn user

                    'Developer Guide No 243
                    sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 811, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    MessageBox.Show(sMessage, "Address Incomplete", MessageBoxButtons.OK)
                End If
                oFindAgentBusiness = Nothing
            End If

            ' Destroy Find Party object

            oFindAgent.Dispose()
            oFindAgent = Nothing

            Return result

        Catch excep As System.Exception


            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process Find Party.", ACApp, ACClass, "SelectAgent", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          EnableAgentButton
    'Description:   -
    'History:       Alix - 11/04/2003 - Created
    '*************************************************************************
    Private Function EnableAgentButton() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If no media type selected, disable agent button
            If cboMediaType.SelectedIndex = -1 Then
                cmdAgent.Enabled = False
                txtReference.Enabled = False
                m_lAgentCnt = 0
                m_sAgentName = ""
                m_sAgentRef = ""
                txtAgent.Text = ""
                txtReference.Text = ""
                Return result
            End If

            ' Check if agent button should be enabled for this medai type
            For iIndex As Integer = m_vMediaTypesArray.GetLowerBound(1) To m_vMediaTypesArray.GetUpperBound(1)
                ' We found the mediatype in array
                If CDbl(m_vMediaTypesArray(0, iIndex)) = VB6.GetItemData(cboMediaType, cboMediaType.SelectedIndex) Then
                    ' We check its is_via_third_party field
                    If gPMFunctions.ToSafeString(m_vMediaTypesArray(4, iIndex)) = "1" Then
                        cmdAgent.Enabled = True
                        txtReference.Enabled = True
                    Else
                        cmdAgent.Enabled = False
                        txtReference.Enabled = False
                        m_lAgentCnt = 0
                        m_sAgentName = ""
                        m_sAgentRef = ""
                        txtAgent.Text = ""
                        txtReference.Text = ""
                    End If
                    Exit For
                End If
            Next iIndex

            Return result

        Catch excep As System.Exception


            cmdAgent.Enabled = True
            txtReference.Enabled = True
            result = gPMConstants.PMEReturnCode.PMFail
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "EnableAgentButton Failed.", ACApp, ACClass, "EnableAgentButton", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          DataToInterface
    'Description:   -
    'History:       Alix - 14/04/2003 - Created
    '*************************************************************************
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtAgent.Text = m_sAgentName.Trim()
            txtReference.Text = m_sAgentRef.Trim()
            txtOriginalDebt.Text = m_cAmount.ToString("C")
            txtArrears.Text = m_cArrears.ToString("C")
            txtBalance.Text = m_cBalance.ToString("C")
            txtPlanStatus.Text = m_sPlanStatus
            txtDeposit.Text = m_cDepositAmount.ToString("C")
            txtAdminCharge.Text = m_cAdminAmount.ToString("C")
            txtProtection.Text = m_cProtectionAmount.ToString("C")
            txtInterest.Text = m_cInterestAmount.ToString("C")
            txtTaxes.Text = m_cTaxesAmount.ToString("C")
            'developer guide no. 40
            txtFirstInstalmentDate.Text = m_dtFirstInstDate
            txtFirstInstAmount.Text = m_cFirstInstAmount.ToString("C")
            'developer guide no. 40
            txtNextInstalmentDate.Text = m_dtNextInstDate
            txtOtherInstAmount.Text = m_cNextInstAmount.ToString("C")
            'developer guide no. 40
            txtLastInstalmentDate.Text = m_dtLastInstDate
            txtLastInstAmount.Text = m_cLastInstAmount.ToString("C")

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFail
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "DataToInterface Failed.", ACApp, ACClass, "DataToInterface", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Calculate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-06-2003 : 223
    ' ***************************************************************** '
    Public Function Calculate() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "Calculate"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bHasChanged Then
                CalculateSingleQuote()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.			
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function
End Class
