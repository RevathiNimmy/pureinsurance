Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: {TodaysDate}
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '   MEvans : 24-11-2004 : PN16357 - if the rating sections currency id
    '     is null default to using the currency id from the policy (as discussed with DD)
    ' 19/10/2005 RKS Premium Override
    ' ***************************************************************** '
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskId As Integer
    Private m_lOriginalRiskcnt As Integer

    Private m_sGISDataModelCode As String = ""
    Private m_lPolicyLinkId As Integer

    Private m_iProRata As Integer
    Private m_dProRataRate As Double
    Private m_sProRataMessage As String = ""

    Private m_cTotalPremium As Decimal


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSirPerilAllocation.Business

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_vRateTypes(,) As Object

    ' Premium totals
    Private m_cReturnPremium As Decimal
    Private m_cReturnPremiumAnnual As Decimal
    Private m_cNetPremium As Decimal

    Private m_lInsCompanyID As Integer
    Private m_lDefaultTab As Integer

    Private m_sStatusSimpleText As String = ""
    Private m_bIsBackDatedMTA As Boolean

    Private r_bUserAllowRatingSectionAddDelete As Boolean
    Private r_bUserAllowRatingSectionEdit As Boolean
    Private r_bAllowRatingSectionAdd As Boolean
    Private r_bAllowRatingSectionEdit As Boolean
    Private r_bAllowRatingSectionDelete As Boolean
    Private r_bAllowEditRatingSectionRateType As Boolean
    Private r_bAllowEditRatingSectionRate As Boolean
    Private r_bAllowEditRatingSectionSumInsured As Boolean
    Private r_bAllowEditRatingSectionThisPremium As Boolean
    'Only applicable for % type of rates
    Private m_sDecimalFormat As String

    'Developer Guide No. 50
    Dim frmDetail As frmDetail
    Public Shared iCache As ICacheManager
    Private sKey As String = String.Empty
    Private oCurrency As bACTCurrency.Currency

    Private m_bIsSilentQuote As Boolean = False
    Private m_sApplyMTATaxRatesonRen As String = ""
    Public WriteOnly Property IsSilentQuote() As Boolean
        Set(value As Boolean)
            m_bIsSilentQuote = value
        End Set
    End Property

    Public WriteOnly Property InsuranceFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property RiskId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskId = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property DefaultTab() As Integer
        Set(ByVal Value As Integer)
            m_lDefaultTab = Value
        End Set
    End Property



    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property

    Public Property Task() As Integer
        Get
            ' Return the objects task.
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            ' Set the objects task.
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public WriteOnly Property IsBackDatedMTA() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsBackDatedMTA = Value
        End Set
    End Property

    Public Property ApplyMTATaxRatesonRen() As String
        Get
            Return m_sApplyMTATaxRatesonRen
        End Get
        Set(ByVal Value As String)
            m_sApplyMTATaxRatesonRen = Value
        End Set
    End Property


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

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Default state for section buttons
            cmdAdd.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView)
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            SSTabHelper.SetSelectedIndex(SSTab1, m_lDefaultTab)

            'PN 46490 (RC)
            cmdUpSection.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView)
            cmdDownSection.Enabled = (m_iTask <> gPMConstants.PMEComponentAction.PMView)
            Return result

        Catch excep As System.Exception



            ' Error Section.
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
            Dim bIsTrueMonthlyPolicy As Boolean = m_oBusiness.GetTMPStatus()


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            ' Command buttons

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Labels

            lblPolicyHolderShort.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyHolderLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblPolicyRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPolicyRefLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblOldAnnualPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOldAnnualPremiumLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblNewAnnualPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewAnnualPremiumLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblOldPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReturnPremiumLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblNewPremium.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTotalPremiumLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblPremiumDue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPremiumDueLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Get rating headers


            lvwRatingSection.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRatingSectionGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwRatingSection.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEarningPatternGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwRatingSection.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRateTypeGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwRatingSection.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRateGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwRatingSection.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSumInsuredGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwRatingSection.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPremiumGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwRatingSection.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThisPremiumGridColumn, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If Not IsNothing(bIsTrueMonthlyPolicy) AndAlso bIsTrueMonthlyPolicy = True Then
                lvwRatingSection.Columns.Item(6).Text = lvwRatingSection.Columns.Item(6).Text.Replace("Annual", "Monthly")
            End If

            ' Copy rating headers to original headers
            lvwOriginal.Columns.Item(0).Text = lvwRatingSection.Columns.Item(0).Text
            lvwOriginal.Columns.Item(1).Text = lvwRatingSection.Columns.Item(1).Text
            lvwOriginal.Columns.Item(2).Text = lvwRatingSection.Columns.Item(2).Text
            lvwOriginal.Columns.Item(3).Text = lvwRatingSection.Columns.Item(3).Text
            lvwOriginal.Columns.Item(4).Text = lvwRatingSection.Columns.Item(4).Text
            lvwOriginal.Columns.Item(5).Text = lvwRatingSection.Columns.Item(5).Text
            lvwOriginal.Columns.Item(6).Text = lvwRatingSection.Columns.Item(6).Text

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Perform the process for the command button pressed by the user.
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim lRatingSectionTypeId, lEarningPatternID, lRateTypeId As Integer
        Dim cAnnualPremium, cThisPremium, cAnnualRate, cSumInsured As Decimal
        Dim lCountryId, lStateId As Integer
        Dim lvItem As ListViewItem
        Dim sTemp As String = ""
        Dim iDefinedCurrencyID As Integer

        Dim iIsAmended As Integer
        Dim cCalculatedPremium As Decimal
        Dim sOverrideReason As String = ""



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Return Value
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Return result
            End If

            ' Validate input
            m_lReturn = ValidateDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sStatusSimpleText = ""


            m_lReturn = m_oBusiness.DeleteSectionAndPerils()

            If lvwOriginal.Visible Then
                For icount As Integer = 1 To lvwOriginal.Items.Count

                    lvItem = lvwOriginal.Items.Item(icount - 1)

                    lRatingSectionTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRatingSectionTypeIdCol).Text.Trim())
                    lEarningPatternID = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACEarningPatternIDCol).Text.Trim())
                    lRateTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRateTypeIdCol).Text.Trim())

                    'unformat the sum insured column
                    txtCurrency.Text = ListViewHelper.GetListViewSubItem(lvItem, ACSumInsuredCol).Text

                    cSumInsured = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                    'Unformat the Rate column
                    '        txtPercent.Text = lvItem.SubItems(ACRateCol)
                    '        cAnnualRate = m_oFormFields.UnformatControl(ctlControl:=txtPercent)
                    sTemp = ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text
                    If sTemp.EndsWith("%") Then
                        sTemp = sTemp.Substring(0, sTemp.Length - 1)
                    End If

                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(sTemp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        cAnnualRate = 0
                    Else
                        cAnnualRate = CDec(sTemp)
                    End If

                    'Unformat the Premium column
                    txtCurrency.Text = ListViewHelper.GetListViewSubItem(lvItem, ACPremiumCol).Text

                    cAnnualPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                    'Unformat the Premium column
                    txtCurrency.Text = ListViewHelper.GetListViewSubItem(lvItem, ACThisPremiumCol).Text

                    cThisPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                    If ListViewHelper.GetListViewSubItem(lvItem, ACDefinedCurrencyID).Text = "" Then
                        iDefinedCurrencyID = 0
                    Else
                        iDefinedCurrencyID = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACDefinedCurrencyID).Text)
                    End If

                    ' get country and state
                    lCountryId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACCountryIDCol).Text)
                    lStateId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACStateIDCol).Text)

                    iIsAmended = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACIsAmendedCol).Text)
                    If iIsAmended = 1 Then
                        m_sStatusSimpleText = "WARNING: Premium has been amended"
                    End If
                    cCalculatedPremium = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvItem, ACCalculatedPremiumCol).Text)
                    sOverrideReason = gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvItem, ACOverrideReasonCol).Text)


                    'Call the function in BO to add the peril sections

                    m_lReturn = m_oBusiness.AddSectionAndPerils(v_lRatingSectionTypeID:=lRatingSectionTypeId, v_lPolicySectionTypeid:=0, v_cAnnualPremium:=cAnnualPremium, v_cThisPremium:=cThisPremium, v_cAnnualRate:=cAnnualRate, v_cSumInsured:=cSumInsured, v_lRateTypeId:=lRateTypeId, v_lOriginalFlag:=1, v_iDefinedCurrencyID:=iDefinedCurrencyID, v_lCountryId:=lCountryId, v_lStateId:=lStateId, v_iIsAmended:=iIsAmended, v_cCalculatedPremium:=cCalculatedPremium, v_sOverrideReason:=sOverrideReason, v_lEarningPatternId:=lEarningPatternID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next icount
            End If

            For icount As Integer = 1 To lvwRatingSection.Items.Count

                lvItem = lvwRatingSection.Items.Item(icount - 1)

                lRatingSectionTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRatingSectionTypeIdCol).Text.Trim())
                lEarningPatternID = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACEarningPatternIDCol).Text.Trim())
                lRateTypeId = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACRateTypeIdCol).Text.Trim())

                'unformat the sum insured column
                txtCurrency.Text = ListViewHelper.GetListViewSubItem(lvItem, ACSumInsuredCol).Text

                cSumInsured = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                'Unformat the Rate column
                '        txtPercent.Text = lvItem.SubItems(ACRateCol)
                '        cAnnualRate = m_oFormFields.UnformatControl(ctlControl:=txtPercent)
                sTemp = ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text
                If sTemp.Trim().Length > 0 Then
                    If sTemp.EndsWith("%") Then
                        sTemp = sTemp.Substring(0, sTemp.Length - 1)
                    End If
                End If

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(sTemp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    cAnnualRate = 0
                Else
                    cAnnualRate = CDec(sTemp)
                End If

                'Unformat the Premium column
                txtCurrency.Text = ListViewHelper.GetListViewSubItem(lvItem, ACPremiumCol).Text

                cAnnualPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                'Unformat the Premium column
                txtCurrency.Text = ListViewHelper.GetListViewSubItem(lvItem, ACThisPremiumCol).Text

                cThisPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                If ListViewHelper.GetListViewSubItem(lvItem, ACDefinedCurrencyID).Text = "" Then
                    iDefinedCurrencyID = 0
                Else
                    iDefinedCurrencyID = CInt(ListViewHelper.GetListViewSubItem(lvItem, ACDefinedCurrencyID).Text)
                End If

                ' get country and state
                lCountryId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACCountryIDCol).Text)
                lStateId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvItem, ACStateIDCol).Text)

                iIsAmended = gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvItem, ACIsAmendedCol).Text)
                If iIsAmended = 1 Then
                    m_sStatusSimpleText = "WARNING: Premium has been amended"
                End If
                cCalculatedPremium = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvItem, ACCalculatedPremiumCol).Text)
                sOverrideReason = gPMFunctions.ToSafeString(ListViewHelper.GetListViewSubItem(lvItem, ACOverrideReasonCol).Text)

                'Call the function in BO to add the peril sections

                m_lReturn = m_oBusiness.AddSectionAndPerils(v_lRatingSectionTypeID:=lRatingSectionTypeId, v_lPolicySectionTypeid:=0, v_cAnnualPremium:=cAnnualPremium, v_cThisPremium:=cThisPremium, v_cAnnualRate:=cAnnualRate, v_cSumInsured:=cSumInsured, v_lRateTypeId:=lRateTypeId, v_lOriginalFlag:=0, v_iDefinedCurrencyID:=iDefinedCurrencyID, v_lCountryId:=lCountryId, v_lStateId:=lStateId, v_iIsAmended:=iIsAmended, v_cCalculatedPremium:=cCalculatedPremium, v_sOverrideReason:=sOverrideReason, v_lEarningPatternId:=lEarningPatternID)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next icount


            m_lReturn = m_oBusiness.ApplyCoinsuranceToRisk()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.UpdateRisk()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            _StatusBar1_Panel1.Text = m_sStatusSimpleText

            Return result

        Catch excep As System.Exception


            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Private Function ValidateDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for positive cancellation amounts
            If (m_sTransactionType = "MTC") And (m_cNetPremium > 0) Then
                ' Check product option

                Select Case m_oBusiness.AllowPositiveCancellation
                    Case gPMConstants.PM_ALLOW
                        ' This is okay
                    Case gPMConstants.PM_DENY
                        ' Warn the user
                        MessageBox.Show("This Product has not been configured to allow an additional premium on Policy Cancellation.", "Policy Cancellation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        ' Cancel
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Case gPMConstants.PM_PROMPT
                        ' Warn the user
                        If MessageBox.Show("Premium Due is an Additional Premium. Do you wish to proceed?", "Policy Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                            ' Cancel
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                End Select
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process ValidateDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    Private Function Quote() As Integer

        Dim result As Integer = 0
        Dim lTransactionType, lQuoteType As Integer
        Dim oObject As iGIS.Application
        Dim bIsMTCRatingRulesEnabled As Boolean
        Dim bIsMandatoryRisks As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Tomo140901
            If m_iTask <> gPMConstants.PMEComponentAction.PMDelete Then
                'JMK 01/08/2001
                'Cancelling a policy - no need to rate

                If m_sTransactionType = "MTC" Then
                    m_lReturn = m_oBusiness.CheckMTCRatingRules(m_lInsuranceFileCnt, bIsMTCRatingRulesEnabled)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_lReturn = m_oBusiness.CheckMandatoryRisk(m_lRiskId, bIsMandatoryRisks)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_sTransactionType <> "MTC" Or bIsMandatoryRisks Or bIsMTCRatingRulesEnabled Then
                    'This also clears the output table

                    m_lReturn = m_oBusiness.GetDataModel(sGISDataModel:=m_sGISDataModelCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    lTransactionType = m_oBusiness.TransactionTypeId

                    Dim temp_oObject As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oObject, "iGIS.Application", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    oObject = temp_oObject
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lPolicyLinkId = -1

                    m_lReturn = oObject.SetProcessModes(vTask:=m_iTask,
                                        vNavigate:=m_lNavigate,
                                        vProcessMode:=m_lProcessMode,
                                        vTransactionType:=m_sTransactionType,
                                        vEffectiveDate:=m_dtEffectiveDate)

                    m_lReturn = oObject.LoadFromDB(v_sGisDataModelCode:=m_sGISDataModelCode, r_vInsuranceFileCnt:=m_lInsuranceFolderCnt, r_vPolicyLinkID:=m_lPolicyLinkId, r_vRiskID:=m_lRiskId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    EncodeTransactionScreenAndType(r_lEncoded:=lQuoteType, r_lTransactionType:=lTransactionType, r_lGISScreenId:=0, r_lQuoteType:=1)


                    m_lReturn = oObject.NBQuote(v_lQuoteType:=lQuoteType, v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=DateTime.Today)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = oObject.SaveToDB()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    oObject.Dispose()

                    oObject = Nothing
                End If
                'JMK 01/08/2001 end
            End If

            If m_sTransactionType <> "DRI" And m_sTransactionType <> "PT" Then

                m_lReturn = m_oBusiness.PopulateRatingSections(r_vResultArray:=Nothing, v_bIsBackDatedMTA:=m_bIsBackDatedMTA)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                m_lReturn = m_oBusiness.UpdateRiskStatus(v_lRiskCnt:=m_lRiskId, v_lRiskStatusId:=8)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' RDC 09082005 update risk else rating sections won't be added

            m_lReturn = m_oBusiness.UpdateRisk()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform process command", vApp:=ACApp, vClass:=ACClass, vMethod:="Quote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function RecalculatePremium() As Integer

        Dim result As Integer = 0
        Dim lCount As Integer
        Dim bRoundingFound, bMultiRounding As Boolean
        ' Rating variables
        Dim sRateTypeCode, sTemp As String
        Dim cRate As Decimal
        ' Working Premiums
        Dim cPremium, cThisPremium As Decimal
        ' Premium Totals
        Dim cTotalAnnualPremium, cTotalThisPremium As Decimal

        Try

            ' Set flags and lookups
            result = gPMConstants.PMEReturnCode.PMTrue

            ' There's a problem that came to light when manual rating takes place, rounding goes
            ' belly-up when rating sections are added. The safest thing to do is to reshuffle the
            ' list and put the rounding section at the bottom...

            If m_oBusiness.RoundPremium = 1 Then
                ' Walk the list, but not by ForEach as we may be moving things
                lCount = 1
                Do While lCount < lvwRatingSection.Items.Count
                    ' Get the current lines type
                    If IsRoundingSection(lvwRatingSection.Items.Item(lCount - 1)) Then
                        If Not bRoundingFound Then
                            ' Move item to end of list
                            m_lReturn = ListViewMoveItem(lvwRatingSection, lvwRatingSection.Items.Item(lCount - 1), ListView6Func.SIRListViewMoveItemEnum.sirLVMIMoveLast)

                            bRoundingFound = True
                        Else
                            ' Remove additional rounding section
                            lvwRatingSection.Items.RemoveAt(lCount - 1)

                            bMultiRounding = True
                        End If
                    Else
                        lCount += 1
                    End If
                Loop

                ' If we have found duplicate rounding sections and removed
                ' some, the least we can do is let the user know about it.
                If bMultiRounding Then
                    MessageBox.Show("Multiple Rounding Sections were detected." & Strings.Chr(13) & Strings.Chr(10) & "Duplicates have been removed.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If

            ' Total return premiums
            If m_cReturnPremium = 0 Then
                ' Just in case
                m_cReturnPremiumAnnual = 0

                For Each oListItem As ListViewItem In lvwOriginal.Items
                    ' Get and unformat annual premium
                    txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, ACPremiumCol).Text

                    cPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))
                    m_cReturnPremiumAnnual += cPremium

                    ' Get and unformat this premium
                    txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, ACThisPremiumCol).Text

                    cThisPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))
                    m_cReturnPremium += cThisPremium
                Next oListItem
            End If

            ' New rating sections
            For Each oListItem As ListViewItem In lvwRatingSection.Items
                'Don't include the rounding item if we're rounding...

                ' Get premium annual premium
                txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, ACPremiumCol).Text

                cPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                ' Get this premium
                txtCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, ACThisPremiumCol).Text

                cThisPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                If IsRoundingSection(oListItem) Then

                    If cPremium > 0 Then
                        cThisPremium = GetRoundAmount(cThisPremium)
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, ACThisPremiumCol).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, CStr(cThisPremium))

                End If

                ' Get the rating type
                m_lReturn = GetRateType(lRateTypeId:=CInt(ListViewHelper.GetListViewSubItem(oListItem, ACRateTypeIdCol).Text), sRateTypeCode:=sRateTypeCode)

                ' Is this a "Percentage of running total type?"
                If sRateTypeCode = "T" Then
                    ' Get the rate
                    sTemp = ListViewHelper.GetListViewSubItem(oListItem, ACRateCol).Text
                    sTemp = sTemp.Replace("%", "")
                    Dim dbNumericTemp As Double
                    cRate = IIf(Double.TryParse(sTemp, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp), CDec(sTemp), 0)

                    ' Calculate premiums
                    cPremium = cTotalAnnualPremium * cRate / 100.0#
                    cThisPremium = cTotalThisPremium * cRate / 100.0#

                    ' Write the annual premium back
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=cPremium)

                    ListViewHelper.GetListViewSubItem(oListItem, ACPremiumCol).Text = txtCurrency.Text

                    'One way to cope with rounding...

                    cPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))

                    ' Write this premium back
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=cThisPremium)

                    ListViewHelper.GetListViewSubItem(oListItem, ACThisPremiumCol).Text = txtCurrency.Text

                    'One way to cope with rounding...

                    cThisPremium = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))
                End If

                ' Update running totals
                cTotalAnnualPremium += cPremium
                cTotalThisPremium += cThisPremium

            Next oListItem

            ' Return premiums...
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOldPremiumNet, vControlValue:=-m_cReturnPremium)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtOldAnnPremNet, vControlValue:=m_cReturnPremiumAnnual)

            ' New premiums...
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNewPremiumNet, vControlValue:=cTotalThisPremium)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtNewAnnPremNet, vControlValue:=cTotalAnnualPremium)

            ' New premium...
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPremiumDueNet, vControlValue:=cTotalThisPremium + m_cReturnPremium)

            ' Store total premium as it may be useful (IT IS USEFUL)
            m_cNetPremium = cTotalThisPremium + m_cReturnPremium

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Calculate Premium", vApp:=ACApp, vClass:="", vMethod:="RecalculatePremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetRateType(ByRef lRateTypeId As Integer, ByRef sRateTypeCode As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sRateTypeCode = "O"

            If Information.IsArray(m_vRateTypes) Then
                For nCount As Integer = m_vRateTypes.GetLowerBound(1) To m_vRateTypes.GetUpperBound(1)
                    If CDbl(m_vRateTypes(0, nCount)) = lRateTypeId Then
                        sRateTypeCode = CStr(m_vRateTypes(1, nCount)).Trim()
                        Exit For
                    End If
                Next

            End If

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Rate Type", vApp:=ACApp, vClass:="", vMethod:="GetRateType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DisplayListViews
    '
    ' Description:
    '
    ' History: 15/08/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function DisplayListViews(ByRef lOriginalFlag As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lOriginalFlag = 1 Then
                fraOriginal.Visible = True
            Else
                fraOriginal.Visible = False
                lblOldAnnualPremium.Visible = False
                txtOldAnnPremNet.Visible = False
                lblOldPremium.Visible = False
                txtOldPremiumNet.Visible = False
                lblNewPremium.Visible = False
                txtNewPremiumNet.Visible = False
                'changed the size as in VB size is calculated from Form top but here its calculated from Frame top
                'fraRatingSection.Top = VB6.TwipsToPixelsY(360) 'fraRatingSection.Top - ACShiftUpForNoOrigFrame
                fraRatingSection.Top = VB6.TwipsToPixelsY(60) 'fraRatingSection.Top - ACShiftUpForNoOrigFrame
                fraRatingSection.Height = VB6.TwipsToPixelsY(4815) 'fraRatingSection.Height + ACShiftUpForNoOrigStretch
                lvwRatingSection.Height = VB6.TwipsToPixelsY(4095) ' lvwRatingSection.Height + ACShiftUpForNoOrigStretch
                cmdAdd.Top = VB6.TwipsToPixelsY(4365) 'cmdAdd.Top + ACShiftUpForNoOrigStretch
                cmdDelete.Top = VB6.TwipsToPixelsY(4365) 'cmdDelete.Top + ACShiftUpForNoOrigStretch
                cmdEdit.Top = VB6.TwipsToPixelsY(4365) 'cmdEdit.Top + ACShiftUpForNoOrigStretch

                'fraSummary.Top = fraSummary.Top + ACShiftUpForNoOrigCtl
                'fraSummary.Height = fraSummary.Height - ACShiftUpForNoOrigCtl
                'lblNewAnnualPremium.Top = lblNewAnnualPremium.Top - ACShiftUpForNoOrigCtl
                'txtNewAnnPremNet.Top = txtNewAnnPremNet.Top - ACShiftUpForNoOrigCtl
                'lblPremiumDue.Top = lblPremiumDue.Top - ACShiftUpForNoOrigCtl
                'txtPremiumDueNet.Top = txtPremiumDueNet.Top - ACShiftUpForNoOrigCtl

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayListViews Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayListViews", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
    Public Sub EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Object, ByRef r_lGISScreenId As Object, ByRef r_lQuoteType As Object)

        Try

            'new format 1TTTSSSSYY



            r_lEncoded = CInt(1000000000 + (CDbl(r_lTransactionType) * 1000000) + (CDbl(r_lGISScreenId) * 100) + CDbl(r_lQuoteType))

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeTransactionScreenAndType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeTransactionScreenAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        frmDetail = New frmDetail
        frmDetail.RiskId = m_lRiskId
        frmDetail.BusinessObject = m_oBusiness 'QBENZ022
        frmDetail.OriginalRiskCnt = m_lOriginalRiskcnt
        frmDetail.TransactionCurrencyID = cboCurrency.CurrencyId
        frmDetail.Mode = gPMConstants.PMEComponentAction.PMAdd
        frmDetail.ProRata = m_iProRata
        frmDetail.ProRataRate = m_dProRataRate
        frmDetail.OriginalAnnualPremium = 0
        frmDetail.CompanyID = m_lInsCompanyID
        frmDetail.InsuranceFileCnt = m_lInsuranceFileCnt
        frmDetail.cboCountry.ItemId = 0
        frmDetail.AutoCalculated = False

        If m_oBusiness.IsTrueMonthlyPolicy() Then
            frmDetail.lblPremium.Text = frmDetail.lblPremium.Text.Replace("Annual", "Monthly")
        End If
        iPMFunc.CenterForm(frmDetail)

        'Show the Details screen in New mode
        frmDetail.ShowDialog(Me)
        If frmDetail.Status = gPMConstants.PMEReturnCode.PMOK Then
            m_lReturn = RecalculatePremium()

            ' Refresh the add/edit/delete buttons
            SetSectionButtons()
        End If

        frmDetail.Close()

        ' for now on every change to the rating section alway rerun
        ' process command routines
        m_lReturn = ProcessCommand()

        ' call recalculate mode for fees and taxes
        m_lReturn = Recalculate(kRecalculateModeRatings)

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        ' Delete current item
        If lvwRatingSection.FocusedItem Is Nothing Then
            cmdDelete.Enabled = False
            Exit Sub
        End If

        ' Delete section
        lvwRatingSection.Items.RemoveAt(lvwRatingSection.FocusedItem.Index)

        ' Recalculate
        m_lReturn = RecalculatePremium()

        ' Refresh the add/edit/delete buttons
        SetSectionButtons()

        ' for now on every change to the rating section alway rerun
        ' process command routines
        m_lReturn = ProcessCommand()

        ' call recalculate mode for fees and taxes
        m_lReturn = Recalculate(kRecalculateModeRatings)

    End Sub

    Private Sub cmdDownSection_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDownSection.Click

        Dim oListItem As ListViewItem
        Dim bEnabled As Boolean

        Try
            If lvwRatingSection.Items.Count = 0 OrElse lvwRatingSection.SelectedItems.Count = 0 Then
                Exit Sub
            End If

            ' If we have an item and it's not the rounding section move and recalc.
            If Not (lvwRatingSection.SelectedItems(0) Is Nothing) Then
                ' Disable for cleanliness
                bEnabled = lvwRatingSection.Enabled
                lvwRatingSection.Enabled = False

                If Not IsRoundingSection(lvwRatingSection.FocusedItem) Then
                    m_lReturn = ListViewMoveItem(lvwRatingSection, lvwRatingSection.SelectedItems(0), ListView6Func.SIRListViewMoveItemEnum.sirLVMIMoveNext, NewListItem:=oListItem)

                    ' Reselect item
                    oListItem.Selected = True

                    m_lReturn = RecalculatePremium()
                End If

                ' Re-enable and setfocus
                lvwRatingSection.Enabled = bEnabled
                If bEnabled And lvwRatingSection.Visible Then
                    lvwRatingSection.Focus()
                End If

                ' for now on every change to the rating section alway rerun
                ' process command routines to save data back to database
                m_lReturn = ProcessCommand()

                ' recalculate as the premium for the item being moved
                ' could have been changed by moving its position
                ' i.e Rating Section Premiums based on Percentage of Running Value
                ' change depending on there position...
                m_lReturn = Recalculate(kRecalculateModeRatings)


            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDownSection_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDownSection_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        frmDetail = New frmDetail
        ' Check for active section
        If lvwRatingSection.FocusedItem Is Nothing Then
            cmdEdit.Enabled = False
            Exit Sub
        End If

        'Get the Rating Type Id
        Dim nItem As Integer = lvwRatingSection.FocusedItem.Index + 1

        frmDetail.m_bFormLoaded = False

        frmDetail.AllowEditRatingSectionRateType = r_bAllowEditRatingSectionRateType
        frmDetail.AllowEditRatingSectionRate = r_bAllowEditRatingSectionRate
        frmDetail.AllowEditRatingSectionSumInsured = r_bAllowEditRatingSectionSumInsured
        frmDetail.AllowEditRatingSectionThisPremium = r_bAllowEditRatingSectionThisPremium

        frmDetail.TransactionCurrencyID = cboCurrency.CurrencyId

        frmDetail.frmLoad()

        'AJM (18/07/2001) - pass transaction type to details screen
        frmDetail.TransactionType = m_sTransactionType


        frmDetail.BusinessObject = m_oBusiness 'QBENZ022
        frmDetail.RiskId = m_lRiskId 'QBENZ022
        frmDetail.AutoCalculated = False ' QBENZ022
        frmDetail.OriginalRiskCnt = m_lOriginalRiskcnt

        frmDetail.cboRatingSectionType.SelectedIndex = -1
        'Developer Guide No. 52
        frmDetail.cboEarningPattern.ItemId = CInt(Conversion.Val(lvwRatingSection.FocusedItem.SubItems(ACEarningPatternIDCol).Text))
        frmDetail.RatingSectionTypeId = CInt(ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACRatingSectionTypeIdCol).Text)
        frmDetail.EarningPatternID = CInt(Conversion.Val(ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACEarningPatternIDCol).Text))
        frmDetail.RateTypeId = CInt(ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACRateTypeIdCol).Text)
        frmDetail.txtSumInsured.Text = ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACSumInsuredCol).Text
        frmDetail.txtRate.Text = ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACRateCol).Text
        frmDetail.txtRate2.Text = ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACRateCol).Text
        frmDetail.txtRate3.Text = ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACRateCol).Text
        frmDetail.txtPremium.Text = ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACPremiumCol).Text
        frmDetail.txtThisPremium.Text = ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACThisPremiumCol).Text
        'Developer Guide No. 52
        frmDetail.cboCountry.ItemId = gPMFunctions.ToSafeLong(lvwRatingSection.FocusedItem.SubItems(ACCountryIDCol).Text)
        frmDetail.cboState.ItemId = gPMFunctions.ToSafeLong(lvwRatingSection.FocusedItem.SubItems(ACStateIDCol).Text)

        frmDetail.ProRata = m_iProRata
        frmDetail.ProRataRate = m_dProRataRate
        frmDetail.OriginalAnnualPremium = 0
        frmDetail.CompanyID = m_lInsCompanyID
        frmDetail.InsuranceFileCnt = m_lInsuranceFileCnt

        If m_oBusiness.IsTrueMonthlyPolicy() Then
            frmDetail.lblPremium.Text = frmDetail.lblPremium.Text.Replace("Annual", "Monthly")
        End If

        ' if the currency id specified on the rating section is not set (is null) then use the
        ' currency id from the policy ( as discussed with DD )
        Dim cCurrencyId As Decimal = ReplaceNullWithDefault(ListViewHelper.GetListViewSubItem(lvwRatingSection.FocusedItem, ACDefinedCurrencyID).Text, 0)
        If cCurrencyId = 0 Then
            cCurrencyId = cboCurrency.CurrencyId
        End If

        frmDetail.DefinedCurrencyID = cCurrencyId

        frmDetail.Mode = gPMConstants.PMEComponentAction.PMEdit

        'frmdetail.CalculatedPremium = gPMFunctions.ToSafeCurrency(lvwRatingSection.listViewHelper1.GetListViewSubItem(lvwRatingSection.FocusedItem, ACCalculatedPremiumCol).Text)
        'frmdetail.OverrideReason = gPMFunctions.ToSafeString(lvwRatingSection.listViewHelper1.GetListViewSubItem(lvwRatingSection.FocusedItem, ACOverrideReasonCol).Text)
        frmDetail.CalculatedPremium = gPMFunctions.ToSafeCurrency(lvwRatingSection.FocusedItem.SubItems(ACCalculatedPremiumCol).Text)
        frmDetail.OverrideReason = gPMFunctions.ToSafeString(lvwRatingSection.FocusedItem.SubItems(ACOverrideReasonCol).Text)

        m_lReturn = frmDetail.CheckPremiumOverride()

        'Show the Details form in Edit mode
        frmDetail.ShowDialog(Me)

        If frmDetail.Status = gPMConstants.PMEReturnCode.PMOK Then
            m_lReturn = RecalculatePremium()
        End If

        frmDetail.Close()


        ' for now on every change to the rating section alway rerun
        ' process command routines
        m_lReturn = ProcessCommand()

        ' call recalculate mode for fees and taxes
        m_lReturn = Recalculate(kRecalculateModeRatings)

        _StatusBar1_Panel1.Text = m_sStatusSimpleText

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)
    End Sub

    Private Sub cmdUpSection_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUpSection.Click

        Dim oListItem As ListViewItem
        Dim bEnabled As Boolean

        Try
            If lvwRatingSection.Items.Count = 0 OrElse lvwRatingSection.SelectedItems.Count = 0 Then
                Exit Sub
            End If

            ' If we have an item and it's not the rounding section move and recalc.
            If Not (lvwRatingSection.SelectedItems(0) Is Nothing) Then
                ' Disable for cleanliness
                bEnabled = lvwRatingSection.Enabled
                lvwRatingSection.Enabled = False

                If Not IsRoundingSection(lvwRatingSection.FocusedItem) Then
                    m_lReturn = ListViewMoveItem(lvwRatingSection, lvwRatingSection.SelectedItems(0), ListView6Func.SIRListViewMoveItemEnum.sirLVMIMovePrevious, NewListItem:=oListItem)

                    ' Reselect item and set focus
                    oListItem.Selected = True

                    m_lReturn = RecalculatePremium()
                End If

                ' Re-enable and setfocus
                lvwRatingSection.Enabled = bEnabled
                If bEnabled And lvwRatingSection.Visible Then
                    lvwRatingSection.Focus()
                End If

                ' for now on every change to the rating section alway rerun
                ' process command routines to save data back to database
                m_lReturn = ProcessCommand()

                ' recalculate as the premium for the item being moved
                ' could have been changed by moving its position
                ' i.e Rating Section Premiums based on Percentage of Running Value
                ' change depending on there position...
                m_lReturn = Recalculate(kRecalculateModeRatings)

            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdUpSection_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdUpSection_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String


        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSirPerilAllocation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Sub Form_Load()

        ' Forms load event.
        Dim sShortName, sInsHolderName, sResolvedName, sInsRef, sInsFolderDesc, sInsCurrencyCode, sInsCurrencyCaption As String

        Dim vRatingSection(,) As Object
        Dim lvItem As ListViewItem
        Dim cTemp As Decimal
        Dim sDeclineReasons, sReferReasons, sMessages As String
        Dim cTotalThisPremium As Decimal
        Dim iNewCount As Integer
        Dim bRoundingSectionDone As Boolean
        Dim iInsCurrencyID As Integer
        Dim sValue As String
        Dim nCount As Integer

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnable6DPGISPercentage, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            If ToSafeString(sValue = "1") Then
                m_sDecimalFormat = "##0.000000"
            Else
                m_sDecimalFormat = "##0.0000"
            End If

            'TN20010514 start

            m_oBusiness.TransactionType = m_sTransactionType
            'TN20010514 end


            m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oBusiness.RiskId = m_lRiskId

            m_oFormFields = New iPMFormControl.FormFields()

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOldAnnPremNet, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNewAnnPremNet, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOldPremiumNet, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtNewPremiumNet, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremiumDueNet, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)


            m_lReturn = m_oBusiness.GetInsuranceHeaderDetails(r_sInsuranceHolderShortName:=sShortName, r_sInsuranceHolderName:=sInsHolderName, r_sInsuranceHolderResolvedName:=sResolvedName, r_sInsuranceRef:=sInsRef, r_sInsuranceFolderDescription:=sInsFolderDesc, r_sInsuranceCurrencyCode:=sInsCurrencyCode, r_sInsuranceCurrencyCaption:=sInsCurrencyCaption, r_iInsuranceCurrencyID:=iInsCurrencyID, r_lInsuranceCompanyID:=m_lInsCompanyID)

            'Assign the values in the text box
            panPolicyDesc.Text = sInsFolderDesc
            panPolicyHolder.Text = sInsHolderName
            panPolicyHolderFull.Text = sShortName
            panPolicyRef.Text = sInsRef
            cboCurrency.CurrencyId = iInsCurrencyID

            txtCurrencyDesc.Text = sInsCurrencyCaption

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                'Perhaps make this optional?  Are they allowed to override the values?
                m_lReturn = Quote()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If


                sDeclineReasons = m_oBusiness.DeclineReasons

                sReferReasons = m_oBusiness.ReferReasons

                sMessages = m_oBusiness.Messages


                m_iProRata = m_oBusiness.ProRata

                m_dProRataRate = m_oBusiness.ProRataRate

                m_sProRataMessage = m_oBusiness.ProRataMessage

                If m_sProRataMessage <> "" AndAlso Not m_bIsSilentQuote Then
                    MessageBox.Show(m_sProRataMessage, "Peril Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                If m_dProRataRate = 0 Then
                    m_dProRataRate = 1
                End If

                If sDeclineReasons <> "" AndAlso Not m_bIsSilentQuote Then
                    MessageBox.Show("Quote declined because:" & Strings.Chr(13) & Strings.Chr(10) & sDeclineReasons, "Peril Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMCancel

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                ElseIf (sReferReasons <> "") AndAlso Not m_bIsSilentQuote Then
                    MessageBox.Show("Quote refered because:" & Strings.Chr(13) & Strings.Chr(10) & sReferReasons, "Peril Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMCancel

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                ElseIf (sMessages <> "") AndAlso Not m_bIsSilentQuote Then
                    MessageBox.Show("Note: " & sMessages, "Peril Allocation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If


            m_lOriginalRiskcnt = m_oBusiness.OriginalRiskCnt

            'Get the Rating section types from the database to be used in the detail form

            m_lReturn = m_oBusiness.GetRatingSectionTypes(vResultArray:=g_vRatingSectionType)

            'Get the Rating types from the database to be used

            m_lReturn = m_oBusiness.GetRateTypes(vResultArray:=m_vRateTypes)

            'Get the values of Rating section from the database

            m_lReturn = m_oBusiness.GetRatingSections(vResultArray:=vRatingSection)

            m_cTotalPremium = 0
            cTotalThisPremium = 0
            m_cReturnPremium = 0

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'We need to decide if we're displaying the
                If Information.IsArray(vRatingSection) Then

                    Dim dbNumericTemp As Double
                    If Double.TryParse(CStr(vRatingSection(ACOriginalFlagCol, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        m_lReturn = DisplayListViews(lOriginalFlag:=CInt(vRatingSection(ACOriginalFlagCol, 0)))
                    Else
                        For nCount = LBound(vRatingSection, 2) To UBound(vRatingSection, 2)
                            ' Need to loop through until encounter original flag or last item and run the DisplayListViews for it
                            If CLng(vRatingSection(ACOriginalFlagCol, nCount)) = 1 Or nCount = UBound(vRatingSection, 2) Then
                                m_lReturn = DisplayListViews(lOriginalFlag:=CLng(vRatingSection(ACOriginalFlagCol, nCount)))
                                Exit For 'at first instance as we just need to know if there is any original flag exists once
                            End If
                        Next nCount
                    End If
                Else
                    m_lReturn = DisplayListViews(lOriginalFlag:=0)
                End If

                'Fill the retrieved data into the List view
                lvwRatingSection.Items.Clear()
                bRoundingSectionDone = False

                If Information.IsArray(vRatingSection) Then

                    For icount As Integer = vRatingSection.GetLowerBound(1) To vRatingSection.GetUpperBound(1)

                        ' JMK 27/07/2001 start
                        ' make sure rounding section is at the bottom of the list
                        iNewCount = icount

                        If m_oBusiness.RoundPremium Then


                            If bRoundingSectionDone Then
                                iNewCount = icount - 1
                            ElseIf CStr(vRatingSection(ACRatingSectionTypeIdCol, icount)) = m_oBusiness.RoundingSectionID Then
                                ' JMK 30/07/2001 - just in case, user could pick a rounding section which
                                '   has been used already, so check premium value and rate type as well...
                                '   (rounding section will have zero premium and rate type 1 - no rating)


                                If CDbl(vRatingSection(ACPremiumCol, icount)) = 0 And CDbl(vRatingSection(ACRateTypeIdCol, icount)) = 1 Then

                                    iNewCount = vRatingSection.GetUpperBound(1)
                                    bRoundingSectionDone = True
                                End If
                            End If
                        End If

                        ' JMK 27/07/2001 end (..of this chunk, I also replaced iCount with iNewCount in rest of this loop)


                        If CDbl(vRatingSection(ACOriginalFlagCol, iNewCount)) = 0 Then

                            lvItem = lvwRatingSection.Items.Add(CStr(vRatingSection(ACRatingSectionTypeCol, iNewCount)))
                        Else

                            lvItem = lvwOriginal.Items.Add(CStr(vRatingSection(ACRatingSectionTypeCol, iNewCount)))
                        End If

                        lvItem.Tag = CStr(iNewCount)


                        ListViewHelper.GetListViewSubItem(lvItem, ACEarningPatternCol).Text = CStr(vRatingSection(ACEarningPatternArrPos, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACRateTypeCol).Text = CStr(vRatingSection(ACRateTypeCol, iNewCount))

                        'Format the field before assigning

                        '                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercent, _
                        'vControlValue:=vRatingSection(ACRateCol, iNewCount))


                        Select Case CStr(vRatingSection(ACRateTypeCol, iNewCount)).ToUpper()
                            Case "PERCENTAGE", "PER 100", "PERCENTAGE OF RUNNING TOTAL"

                                Dim dbNumericTemp2 As Double
                                If Not Double.TryParse(CStr(vRatingSection(ACRateCol, iNewCount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text = ""
                                Else

                                    ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text = StringsHelper.Format(vRatingSection(ACRateCol, iNewCount), m_sDecimalFormat).Trim() & "%"
                                End If

                            Case "PER 1000", "PER MILLION"

                                Dim dbNumericTemp3 As Double
                                If Not Double.TryParse(CStr(vRatingSection(ACRateCol, iNewCount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                                    ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text = ""
                                Else

                                    ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text = StringsHelper.Format(vRatingSection(ACRateCol, iNewCount), "##0.0000").Trim()
                                End If

                            Case Else

                                Dim dbNumericTemp4 As Double
                                If Not Double.TryParse(CStr(vRatingSection(ACRateCol, iNewCount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                    txtCurrency.Text = ""
                                Else
                                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=vRatingSection(ACRateCol, iNewCount))
                                End If
                                ListViewHelper.GetListViewSubItem(lvItem, ACRateCol).Text = txtCurrency.Text
                        End Select

                        'Format the field before assigning
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=vRatingSection(ACSumInsuredCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACSumInsuredCol).Text = txtCurrency.Text

                        'Format the field before assigning
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=vRatingSection(ACPremiumCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACPremiumCol).Text = txtCurrency.Text


                        cTemp = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))
                        m_cTotalPremium += cTemp

                        'This one needs to be recalculated before playing.

                        'There is a possibility of NULLs in the policy section type Id. Set to -1 as Id for them

                        If CStr(vRatingSection(ACEarningPatternIDArrPos, iNewCount)) = "" Then
                            ListViewHelper.GetListViewSubItem(lvItem, ACEarningPatternIDCol).Text = CStr(-1)
                        Else

                            ListViewHelper.GetListViewSubItem(lvItem, ACEarningPatternIDCol).Text = CStr(vRatingSection(ACEarningPatternIDArrPos, iNewCount))
                        End If

                        ListViewHelper.GetListViewSubItem(lvItem, ACRateTypeIdCol).Text = CStr(vRatingSection(ACRateTypeIdCol, iNewCount))


                        ListViewHelper.GetListViewSubItem(lvItem, ACRatingSectionIdCol).Text = CStr(vRatingSection(ACRatingSectionIdCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACRatingSectionTypeIdCol).Text = CStr(vRatingSection(ACRatingSectionTypeIdCol, iNewCount))

                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=vRatingSection(ACThisPremiumCol, iNewCount))


                        cTemp = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCurrency))
                        cTotalThisPremium += cTemp

                        'Format the field before assigning
                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=vRatingSection(ACThisPremiumCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACThisPremiumCol).Text = txtCurrency.Text

                        ListViewHelper.GetListViewSubItem(lvItem, ACDefinedCurrencyID).Text = CStr(vRatingSection(ACDefinedCurrencyID, iNewCount))


                        ListViewHelper.GetListViewSubItem(lvItem, ACCountryCol).Text = CStr(vRatingSection(ACCountryCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACCountryIDCol).Text = CStr(vRatingSection(ACCountryIDCol, iNewCount))


                        ListViewHelper.GetListViewSubItem(lvItem, ACStateCol).Text = CStr(vRatingSection(ACStateCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACStateIDCol).Text = CStr(vRatingSection(ACStateIDCol, iNewCount))


                        ListViewHelper.GetListViewSubItem(lvItem, ACIsAmendedCol).Text = CStr(vRatingSection(ACIsAmendedCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACCalculatedPremiumCol).Text = CStr(vRatingSection(ACCalculatedPremiumCol, iNewCount))

                        ListViewHelper.GetListViewSubItem(lvItem, ACOverrideReasonCol).Text = CStr(vRatingSection(ACOverrideReasonCol, iNewCount))


                        ListViewHelper.GetListViewSubItem(lvItem, ACAutoCalculatedCol).Text = CStr(vRatingSection(ACAutoCalculatedCol, iNewCount)) ' QBENZ022
                    Next icount
                End If
            End If


            m_lReturn = m_oBusiness.GetPerilAllocationSecurity(lRiskCnt:=m_lRiskId, iUserID:=g_iUserID, r_bUserAllowRatingSectionAddDelete:=r_bUserAllowRatingSectionAddDelete, r_bUserAllowRatingSectionEdit:=r_bUserAllowRatingSectionEdit, r_bAllowRatingSectionAdd:=r_bAllowRatingSectionAdd, r_bAllowRatingSectionEdit:=r_bAllowRatingSectionEdit, r_bAllowRatingSectionDelete:=r_bAllowRatingSectionDelete, r_bAllowEditRatingSectionRateType:=r_bAllowEditRatingSectionRateType, r_bAllowEditRatingSectionRate:=r_bAllowEditRatingSectionRate, r_bAllowEditRatingSectionSumInsured:=r_bAllowEditRatingSectionSumInsured, r_bAllowEditRatingSectionThisPremium:=r_bAllowEditRatingSectionThisPremium)


            If m_lReturn <> (gPMConstants.PMEReturnCode.PMTrue) Then

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilAllocationSecurity Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

            End If



            ' if this isnt new business then
            ' refresh results so the screen is updated automatically
            If m_sTransactionType <> "NB" And m_iTask <> gPMConstants.PMEComponentAction.PMView Then

                m_lReturn = m_oBusiness.UpdateRisk()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = RecalculatePremium()


            m_lReturn = SetupFees()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupFees Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If

            m_lReturn = SetupRITaxes()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupRITaxes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If

            ' do any processing required if the risk
            ' being processed is a copy of an existing risk
            m_lReturn = ProcessCopiedRisks()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCopiedRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If

            ' populate totals
            m_lReturn = PopulateTotals()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateTotals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
            End If


            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            SetSectionButtons()

            ' Gets the interface details to be displayed.
            'm_lReturn = BusinessToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'PN:28355 - Don't allow Annual Premium > This Premium on cancellation and MTA
            If (m_sTransactionType = "MTC" Or m_sTransactionType = "MTA") And m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = ValidateAnnualPremium()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = MessageBox.Show("Return Premium is Greater than Premium Billed", "PerilAllocation", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)
                    If m_lReturn = DialogResult.Cancel Then
                        Exit Sub
                    End If
                End If
            End If

            'Call the
            ' this should already have been done
            ' by the add / edit / delete calls
            ' so there is no need to do it again here
            ' but if it hasnt then
            'm_lStatus = ProcessCommand()

            'If (m_lReturn = PMTrue) Then
            Me.Hide()
            'End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub



    'UPGRADE_NOTE: (7001) The following declaration (Label1_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Label1_Click()
    '
    'End Sub

    Private Sub lvwOriginal_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwOriginal.Enter
        ' PW170502 - disable the edit/delete buttons
        cmdDelete.Enabled = False
        cmdEdit.Enabled = False
    End Sub

    Private Sub lvwOriginal_ItemClick(ByVal Item As ListViewItem)
        ' SetSectionButtons
    End Sub

    Private Sub lvwRatingSection_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRatingSection.DoubleClick
        'Call the Edit button Click event
        'But only if we're not viewing...
        'PW170502 - check if edit button is enabled
        If (m_iTask <> gPMConstants.PMEComponentAction.PMView) And cmdEdit.Enabled Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If
    End Sub

    Private Sub lvwRatingSection_ItemClick(ByVal Item As ListViewItem)
        ' Refresh the add/edit/delete buttons
        SetSectionButtons()
    End Sub

    Private Sub SetSectionButtons()

        Dim bIsNotView, bIsNotRounding, bHasSections As Boolean

        Try

            bIsNotView = (Task <> gPMConstants.PMEComponentAction.PMView)
            bIsNotRounding = Not IsRoundingSection(lvwRatingSection.FocusedItem)
            bHasSections = (lvwRatingSection.Items.Count > 0)

            ' Set states

            cmdAdd.Enabled = bIsNotView And r_bUserAllowRatingSectionAddDelete And r_bAllowRatingSectionAdd
            cmdDelete.Enabled = bIsNotView And bHasSections And r_bUserAllowRatingSectionAddDelete And r_bAllowRatingSectionDelete

            cmdEdit.Enabled = bIsNotView And bHasSections And bIsNotRounding And r_bUserAllowRatingSectionEdit And r_bAllowRatingSectionEdit

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process sections", vApp:=ACApp, vClass:=ACClass, vMethod:="SetSectionButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    Private Function IsRoundingSection(ByVal vListItem As ListViewItem) As Boolean

        Dim result As Boolean = False
        Try

            ' Check if passed item is the rounding section...
            If Not (vListItem Is Nothing) Then

                If m_oBusiness.RoundPremium Then

                    If ListViewHelper.GetListViewSubItem(vListItem, ACRatingSectionTypeIdCol).Text = m_oBusiness.RoundingSectionID Then
                        result = True
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Just log the error and return default value.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for rounding section", vApp:=ACApp, vClass:=ACClass, vMethod:="IsRoundingSection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Function GetRoundAmount(ByVal Value As Decimal) As Decimal

        Try
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim cWholeNumber As Decimal
            'Now changed to standard banker's Rounding Rule
            'S4i uses standard Bankers Rounding that rounds .5 up
            'if the preseding number is even and down
            'if the preceding number is odd.
            If Value Then
                'GetRoundAmount = -(Value - Round(Value + (Value / Abs(Value) * 0.0001), 0))

                lReturn = m_oBusiness.CLngRounding(Value, cWholeNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetRoundAmount", "Failed rounding to whole number on " & Value, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            Return cWholeNumber

        Catch excep As System.Exception



            ' Log Error, return value will be zero.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed rounding to whole number on " & Value, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRoundAmount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReplaceNullWithDefault
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-11-2004
    ' ***************************************************************** '
    'Developer Guide No. 101
    Private Function ReplaceNullWithDefault(ByRef v_vValue As Object, ByVal v_vDefault As Object) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const sFunctionName As String = "ReplaceNullWithDefault"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide No. 56
            If String.IsNullOrEmpty(v_vValue) OrElse Convert.IsDBNull(v_vValue) OrElse IsNothing(v_vValue) OrElse v_vValue = gPMConstants.PMEReturnCode.PMFalse OrElse v_vValue = "" Then

                v_vValue = v_vDefault

            End If


            Return v_vValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)

            Return 0

        End Try
    End Function

    Private Sub uctPMUFees1_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMUFees1.Change
        Recalculate(kRecalculateModeFees)
    End Sub

    Private Sub uctPMURITax1_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPMURITax1.Change
        Recalculate(kRecalculateModeRITax)
    End Sub

    ' ***************************************************************** '
    ' Name: Recalculate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function Recalculate(ByVal v_lRecalculateMode As Integer, Optional ByVal v_lLevel As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Recalculate"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' increment level
            v_lLevel += 1

            If v_lLevel = 1 Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                _StatusBar1_Panel1.Text = "Recalculating..."
            End If


            Select Case v_lRecalculateMode
                Case kRecalculateModeRatings

                    ' recalculate fees
                    lReturn = uctPMUFees1.Recalculate()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate Fees Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    lReturn = CType(Recalculate(kRecalculateModeFees, v_lLevel), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case kRecalculateModeFees

                    lReturn = uctPMURITax1.Recalculate()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculateRITaxes Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case kRecalculateModeFees
                    ' do nothing for now... as currently nothing follows this process

            End Select

            If v_lLevel = 1 Then

                lReturn = PopulateTotals()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateTotals Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                _StatusBar1_Panel1.Text = ""

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
    ' Name: SetupFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupFees() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupFees"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No.9
            uctPMUFees1.Initialise()
            uctPMUFees1.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)

            uctPMUFees1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctPMUFees1.RiskCnt = m_lRiskId
            uctPMUFees1.ReadOnly_Renamed = (m_iTask = gPMConstants.PMEComponentAction.PMView)

            ' if we are viewing the initial values should be loaded.
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                uctPMUFees1.ReadOnly_Renamed = True
                'Developer Guide No. 68
                uctPMUFees1.Load_Renamed()
                ' for all other modes the initial values should be recalculated
            Else
                uctPMUFees1.Recalculate()
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
    ' Name: SetupRITaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function SetupRITaxes() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupRITaxes"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No.9
            uctPMURITax1.Initialise()
            uctPMURITax1.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)
            uctPMURITax1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctPMURITax1.RiskCnt = m_lRiskId
            uctPMURITax1.ApplyMTATaxRatesonRen = ApplyMTATaxRatesonRen

            ' if we are viewing the initial values should be loaded.
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                uctPMURITax1.ReadOnly_Renamed = True
                uctPMURITax1.LoadWithoutCalculation = True
                uctPMURITax1.Load_Renamed()
                ' for all other modes the initial values should be recalculated
            Else
                uctPMURITax1.Recalculate()
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
    ' Name: PopulateTotals
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function PopulateTotals() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateTotals"

        Dim lReturn As Integer
        Dim crNetTotal, crFeeTotal, crTaxTotal, crGrossTotal As Decimal
        Dim vRatingSection(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oBusiness.GetRatingSections(vResultArray:=vRatingSection)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNetAmount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            '    lReturn = GetNetAmount(crNetTotal)
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "GetNetAmount Failed", PMLogError
            '    End If
            If Information.IsArray(vRatingSection) Then

                For icount As Integer = vRatingSection.GetLowerBound(1) To vRatingSection.GetUpperBound(1)

                    If CDbl(vRatingSection(ACIsLeviTax, icount)) <> 1 Then

                        crNetTotal = CDbl(vRatingSection(ACThisPremiumCol, icount)) + crNetTotal
                    Else

                        crTaxTotal = CDbl(vRatingSection(ACThisPremiumCol, icount)) + crTaxTotal
                    End If
                Next
            End If

            crFeeTotal = uctPMUFees1.TotalFees
            crTaxTotal = uctPMUFees1.TotalTax + uctPMURITax1.TotalTax + crTaxTotal

            sKey = "KEY_LOOKUP_SelectCurrency_ID_" & uctPMURITax1.CurrencyId.ToString
            If (m_sCallingAppName <> "iPMUDataFixUtility") Then
                iCache = CacheFactory.GetCacheManager("PureCache")
                If iCache IsNot Nothing AndAlso Not iCache.Contains(sKey) Then
                    Dim o_bActCurrencyForm As New bACTCurrency.Form
                    o_bActCurrencyForm.Initialise(sUserName:=g_sUsername, sPassword:=g_sPassword, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
                    o_bActCurrencyForm.GetDetails(uctPMURITax1.CurrencyId)
                End If
                If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
                    oCurrency = iCache.GetData(sKey)
                End If
                If oCurrency IsNot Nothing Then
                    crTaxTotal = Math.Round(crTaxTotal, oCurrency.RoundToPlaces)
                End If
            End If
            txtNetTotal.Text = StringsHelper.Format(crNetTotal, "0.00")
            txtFeeTotal.Text = StringsHelper.Format(crFeeTotal, "0.00")
            txtTaxTotal.Text = StringsHelper.Format(crTaxTotal, "0.00")
            txtGrossTotal.Text = StringsHelper.Format(crNetTotal + crFeeTotal + crTaxTotal, "0.00")



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNetAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetNetAmount(ByRef r_crTotalNetAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNetAmount"

        Dim lReturn As Integer
        Dim crNetAmount As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the original list view is visible
            If lvwOriginal.Items.Count > 0 Then
                ' get the net total from there

                For lRating As Integer = 1 To lvwOriginal.Items.Count

                    crNetAmount = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwOriginal.Items.Item(lRating - 1), ACThisPremiumCol).Text) 'PN 42103

                    r_crTotalNetAmount += crNetAmount

                Next

            End If


            For lRating As Integer = 1 To lvwRatingSection.Items.Count

                crNetAmount = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwRatingSection.Items.Item(lRating - 1), ACThisPremiumCol).Text) 'PN 42103

                r_crTotalNetAmount += crNetAmount

            Next


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


    ' ***************************************************************** '
    ' Name: ProcessCopiedRisks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ProcessCopiedRisks() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCopiedRisks"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vRiskDetails As Object
        Dim lNoOfPerils, lNoOfRatings As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get details of perils and ratings

            lReturn = m_oBusiness.IsRiskACopy(v_lRiskCnt:=m_lRiskId, r_vResults:=vRiskDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "IsRiskACopy Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if there are ratings or perils
            If Information.IsArray(vRiskDetails) Then

                ' get the number of each

                lNoOfPerils = ReplaceNullWithDefault(vRiskDetails(0, 0), gPMConstants.PMEReturnCode.PMFalse)

                lNoOfRatings = ReplaceNullWithDefault(vRiskDetails(1, 0), gPMConstants.PMEReturnCode.PMFalse)

                ' if there are no perils but there are ratings
                ' then this is a copy of an existing risk
                If lNoOfPerils = 0 And lNoOfRatings <> 0 Then

                    ' in order to create the perils
                    ' and to get correct actual fee and tax values etc
                    ' process the interface immediately on loading.
                    lReturn = ProcessCommand()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessCommand Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    lReturn = CType(Recalculate(kRecalculateModeRatings), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Recalculate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

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


    ' ***************************************************************** '
    ' Name: ValidateAnnualPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : RChoudhary : Date : 12 May 2006
    ' ***************************************************************** '
    Public Function ValidateAnnualPremium() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateAnnualPremium"

        Dim l_cTotalBilledPremium As Decimal
        Dim vArray(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate in case of return premium
            If gPMFunctions.ToSafeCurrency(txtNetTotal.Text) < 0 Then

                m_lReturn = m_oBusiness.GetRisksBilledPremium(v_lRiskCnt:=m_lRiskId, r_vResults:=vArray)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Validate Annual Premium Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Start Girija - Bug fixing
                l_cTotalBilledPremium = gPMFunctions.ToSafeCurrency(vArray(0, 0)) + gPMFunctions.ToSafeCurrency(vArray(1, 0))
                'End Girija - Bug fixing

                If l_cTotalBilledPremium < Math.Abs(CDbl(txtNetTotal.Text)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            SSTab1.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            SSTab1.SelectedIndex = 2
        End If
    End Sub
    ''' <summary>
    ''' ProcessOKClickForSilentQuote
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessOKClickForSilentQuote() As Integer
        Const kMethodName As String = "ProcessOKClickForSilentQuote"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            ' Set the interface status.
            m_lStatus = PMEReturnCode.PMOK

            'PN:28355 - Don't allow Annual Premium > This Premium on cancellation and MTA
            If (m_sTransactionType = "MTC" Or m_sTransactionType = "MTA") And m_iTask <> PMEComponentAction.PMAdd Then
                ProcessOKClickForSilentQuote = ValidateAnnualPremium()
            End If

            Return nReturn
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        End Try
    End Function

End Class
