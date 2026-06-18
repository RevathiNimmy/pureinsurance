Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    '*************************************************************************
    ' Form Name: frmInterface
    ' Date: 17/02/1997
    ' Description: Main interface.
    ' Edit History: 170297 - Created
    ' TF240498 - ProcessPartyInterface() added to activate refresh on
    '           return to Find
    ' SP011298 - changes to support new business roadmap
    ' TR28112002 - Changed to use the new Instalments Control
    ' RAW 27/02/2003 : ISS2487 : set the correct m_lStatus value from cmdSelect_Click
    '*************************************************************************

    '=================
    'Private Constants
    '=================
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    '==========================
    'Private Standard Variables
    '==========================
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_iTask As Integer
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_bLoadingForm As Boolean
    'Private m_lPreviousInsuranceFileCnt As Long
    Private m_lInsuranceFileCnt As Integer
    Private m_bIsSingleInstalmentPlan As Boolean
    'PN61609
    Private m_bIsFinanceAmountNetPremium As Boolean
    '=================
    'Public Properties
    '=================
    Public WriteOnly Property SourceId() As Integer
        Set(ByVal Value As Integer)
            uctInstalmentsControl.Source_ID = Value
        End Set
    End Property
    Public WriteOnly Property ProductCode() As String
        Set(ByVal Value As String)
            uctInstalmentsControl.Product_Code = Value
        End Set
    End Property
    Public Property PremiumFinanceCnt() As Integer
        Get
            Return uctInstalmentsControl.PremiumFinanceCnt
        End Get
        Set(ByVal Value As Integer)
            uctInstalmentsControl.PremiumFinanceCnt = Value
        End Set
    End Property
    Public Property PremiumFinanceVersion() As Integer
        Get
            Return uctInstalmentsControl.PremiumFinanceVersion
        End Get
        Set(ByVal Value As Integer)
            uctInstalmentsControl.PremiumFinanceVersion = Value
        End Set
    End Property
    Public Property PremiumFinanceTransactions() As Object
        Get
            Return uctInstalmentsControl.PremiumFinanceTransactions
        End Get
        Set(ByVal Value As Object)


            uctInstalmentsControl.PremiumFinanceTransactions = Value
        End Set
    End Property
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            uctInstalmentsControl.PartyCnt = Value
        End Set
    End Property
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            uctInstalmentsControl.InsuranceFileCnt = Value
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public Property MTAType() As Integer
        Get
            Return uctInstalmentsControl.MTAType
        End Get
        Set(ByVal Value As Integer)
            uctInstalmentsControl.MTAType = Value
        End Set
    End Property
    Public ReadOnly Property FinanceDeposit() As Decimal
        Get
            Return uctInstalmentsControl.DepositAmount
        End Get
    End Property
    Public WriteOnly Property InsuranceFileCnt_Renewal() As Integer
        Set(ByVal Value As Integer)
            uctInstalmentsControl.InsuranceFileCnt_Renewal = Value
        End Set
    End Property

    'PN61609
    Public WriteOnly Property FinanceAmountNetPremium() As Boolean
        Set(ByVal Value As Boolean)
            uctInstalmentsControl.IsFinanceAmountNetPremium = Value
        End Set
    End Property

    '==========================
    'Standard Public Properties
    '==========================
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Standard Property.
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the interface exit status.
            m_lStatus = Value
        End Set
    End Property
    Public Property Task() As Integer
        Get
            ' Return the objects task.
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the objects task.
            m_iTask = Value
            uctInstalmentsControl.Task = Value
        End Set
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Standard Property.
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property
    Public WriteOnly Property IsSingleInstalmentPlan() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsSingleInstalmentPlan = Value
        End Set
    End Property
    '===============
    'Private Methods
    '===============
    '*************************************************************************
    ' Name:         SetInterfaceDefaults
    ' Description:  Sets all of the interface default values.
    '*************************************************************************
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface default values.
            iPMFunc.CenterForm(Me)

            'TR - Display all language specific captions.
            'Developer guide No 243
            iPMForms.DisplayCaptions(Me, bResFile:=My.Resources.ResourceManager)
            iPMForms.SetFieldValidation(Me, m_oFormFields)

            ' Set the status of the Navigate button.
            Select Case m_lNavigate
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = False
                    cmdNavigate.Enabled = False
                Case Else
                    cmdNavigate.Visible = False
            End Select

            cmdSelect.Visible = True
            cmdSelect.Enabled = True
            cmdRequote.Visible = True
            cmdRequote.Enabled = True

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the " & "interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:         DisableInterface
    ' Description:  Disables parts of the interface while a search is
    '              in progress.
    '*************************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (DisableInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisableInterface(ByVal v_bDisable As Boolean)
    '    cmdRequote.Enabled = Not v_bDisable
    'cmdSelect.Enabled = Not v_bDisable
    'End Sub

    Private Sub cmdOverride_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOverride.Click
        If uctInstalmentsControl.ValidateData() <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        uctInstalmentsControl.OverrideQuote()
    End Sub

    Private Sub cmdRequote_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRequote.Click
        uctInstalmentsControl.Refresh()
        cmdSelect.Enabled = uctInstalmentsControl.QuoteAvailable
    End Sub
    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click

        'To stop posting negative total amount and Instalments
        If uctInstalmentsControl.TotalPayableAmount < 0 Then
            MessageBox.Show("Unable to proceed. Total Payable amount is negative.")
            Exit Sub
        End If
        If uctInstalmentsControl.ValidateData() <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If
        uctInstalmentsControl.IsSingleInstalmentPlan = m_bIsSingleInstalmentPlan
        ' RAW 27/02/2003 : ISS2487 : changed to pass PMOK when successful otherwise
        ' display a message and keep form displayed
        If uctInstalmentsControl.SaveQuote() <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to create new Instalment Plan", "Premium Finance Quote", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()
    End Sub
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        'Call DisableInterface(True)
        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        ' Get strings for an "are you sure" message

        'Developer Guide No 243
        Dim sTitle As String = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 300, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        Dim sMessage As String = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 301, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
        If MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
            Me.Hide()
        End If
        'Call DisableInterface(False)
    End Sub
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'developer guide no. 51 (no solution)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub
    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
        cmdSelect_Click(cmdSelect, New EventArgs())
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            If Not m_bLoadingForm Then
                uctInstalmentsControl.TransactionType = m_sTransactionType
                uctInstalmentsControl.LoadCurrencyInfo()
                uctInstalmentsControl.Refresh()
                cmdSelect.Enabled = uctInstalmentsControl.QuoteAvailable
            End If
        End If
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try
            m_bLoadingForm = True
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            Application.DoEvents()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Alix Bergeret - 28/03/2003 - Issue 3259
            uctInstalmentsControl.Source_ID = g_iSourceID

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            m_bLoadingForm = False

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub
    Private Sub Form_Initialize_Renamed()
        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

    End Sub

    Public Function GetSourceID() As Integer
        Dim result As Integer = 0
        Dim bSIRPremiumFinance As Object
        Dim dFeeExcluded As Double = 0
        Dim dTaxExcluded As Double = 0
        Dim dIncludeTaxInPF As Double = 0

        Const kMethodName As String = "GetSourceID"


        Dim oPremiumFinance As bSIRPremiumFinance.Business
        Dim vPFPremiumFinance As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oPremiumFinance As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oPremiumFinance, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPremiumFinance = temp_oPremiumFinance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bSIRPremiumFinance.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the premium finance plan details
            If uctInstalmentsControl.PremiumFinanceCnt <> 0 Then
                m_lReturn = oPremiumFinance.GetSingleFinancePlan(v_lFinanceCount:=uctInstalmentsControl.PremiumFinanceCnt, v_lFinanceVersion:=uctInstalmentsControl.PremiumFinanceVersion, r_vPFPremiumFinance:=vPFPremiumFinance)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oPremiumFinance.GetSingleFinancePlan", "v_lFinanceCount:=" & uctInstalmentsControl.PremiumFinanceCnt, gPMConstants.PMELogLevel.PMLogError)
                End If

                'Did we return any data
                If Information.IsArray(vPFPremiumFinance) Then
                    uctInstalmentsControl.Source_ID = gPMFunctions.ToSafeInteger(vPFPremiumFinance(bSIRPremFinConst.k_PFPlanSource_ID, 0))
                End If
            End If

            oPremiumFinance.GetPolicyFeeAndTaxes(m_lInsuranceFileCnt, dFeeExcluded, dTaxExcluded, dIncludeTaxInPF)
            uctInstalmentsControl.FeeExcluded = dFeeExcluded
            uctInstalmentsControl.TaxExcluded = dTaxExcluded
            uctInstalmentsControl.GrossDue = 0

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oPremiumFinance Is Nothing) Then

                oPremiumFinance.Dispose()
                oPremiumFinance = Nothing
            End If

        End Try
        Return result
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctInstalmentsControl.Controls("ssTabMain"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            DirectCast(uctInstalmentsControl.Controls("ssTabMain"), TabControl).SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            DirectCast(uctInstalmentsControl.Controls("ssTabMain"), TabControl).SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            DirectCast(uctInstalmentsControl.Controls("ssTabMain"), TabControl).SelectedIndex = 3
        End If
    End Sub
End Class