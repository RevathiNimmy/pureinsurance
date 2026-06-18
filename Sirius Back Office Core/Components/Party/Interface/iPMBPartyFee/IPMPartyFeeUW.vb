Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterfaceUW
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterfaceUW
    '
    ' Date:
    '
    ' Description: Main interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterfaceUW"

    '********************************
    ' General Property variables
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lPMAuthorityLevel As Integer
    Private m_bError As Integer

    'TODO: iteration3,and declare Private m_oBusiness As Object
    'Private m_oBusiness As bSIRPartyFee.UBusiness
    Private m_oBusiness As Object
    Private m_lReturn As Integer
    Private m_bInterfaceError As Boolean
    Private m_lTask As gPMConstants.PMEComponentAction
    '********************************

    Private m_lFeeAmountID As Integer
    Private m_vCurrency(,) As Object
    Private m_vProduct(,) As Object
    Private m_vPerilGroup(,) As Object
    Private m_vRiskTypeGroup(,) As Object
    Private m_vTaxGroup(,) As Object
    Private m_vFeeAmountDetails(,) As Object
    Private m_lPartyCnt As Integer

    Private m_oMakeLiveOptions As Object
    Private m_oPaymentTerm As Object
    Private m_oEnableDebitOrder As Object
    Private m_bSuppressDecimalValues As Boolean
    Private m_sUniqueId As String
    Private m_sScreenHeirarchy As String

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    Public m_bDeleteClick As Boolean

    Public WriteOnly Property FeeAmountID() As Integer
        Set(ByVal Value As Integer)
            m_lFeeAmountID = Value
        End Set
    End Property

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    '********************************
    ' General Interface Properties
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
            Return m_lStatus
        End Get
    End Property
    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_lTask = Value
        End Set
    End Property
    Public ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_bError
        End Get
    End Property

    Public Property UniqueId() As String
        Get

            Return m_sUniqueId

        End Get
        Set(ByVal Value As String)

            m_sUniqueId = Value

        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get

            Return m_sScreenHeirarchy

        End Get
        Set(ByVal Value As String)

            m_sScreenHeirarchy = Value

        End Set
    End Property
    ''' <summary>
    ''' Holds The decimal suppress configuration.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsSuppressDecimalValues() As Boolean
        Get
            Return m_bSuppressDecimalValues
        End Get
        Set(ByVal Value As Boolean)
            m_bSuppressDecimalValues = Value
        End Set
    End Property
    Public Property DeleteClick() As Boolean
        Get
            Return m_bDeleteClick
        End Get
        Set(ByVal Value As Boolean)
            m_bDeleteClick = Value
        End Set
    End Property
    '********************************

    Private Sub chkIncludeToInstalment_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIncludeToInstalment.CheckStateChanged
        If chkIncludeToInstalment.CheckState = CheckState.Unchecked Then
            chkSpreadAcrossInstalment.Enabled = False
            chkSpreadAcrossInstalment.Enabled = False
            chkSpreadAcrossInstalment.CheckState = CheckState.Unchecked
        Else
            chkSpreadAcrossInstalment.Enabled = True
            chkSpreadAcrossInstalment.CheckState = CheckState.Checked
        End If

    End Sub
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        m_bDeleteClick = False
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Save()
    End Sub

    ' ***************************************************************** '
    ' Name: Form_Initialize
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

        Const sFunction As String = "Form_Initialize"

        Try

            ' initialise form error indicator
            m_bError = False

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            If g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oBusiness = temp_m_oBusiness

                ' interface error shut down
                m_bError = False

                ' Failed to get an instance of the business object.
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & "Failed to create business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=New Exception(Information.Err().Description))

                ' reset mouse pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Else
                m_oBusiness = temp_m_oBusiness
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

        Catch excep As System.Exception

            ' Log Error.


            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunction & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunction, excep:=excep)

            ' reset mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: Form_QueryUnload
    '
    ' Description: Determines whether any actions need to take place
    '               before unload.
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Private Sub frmInterfaceUW_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        eventArgs.Cancel = Cancel <> 0
    End Sub
    ' ***************************************************************** '
    ' Name: Form_Unload
    '
    ' Description: Destroys all object references
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Private Sub frmInterfaceUW_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Const sFunctionName As String = "Form_Unload"

        Try

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' destroy object reference
            m_oBusiness = Nothing

        Catch excep As System.Exception
            m_bInterfaceError = True

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************
            Exit Sub
        End Try
    End Sub
    ''' <summary>
    ''' Form_Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>

    Private Sub frmInterfaceUW_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

        Const sFunctionName As String = "Form_Load"

        Try
            m_bDeleteClick = DirectCast(eventSender, iPMBPartyFee.frmInterfaceUW).DeleteClick
            ' set up interface
            m_lReturn = SetUpForm(m_bDeleteClick)

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'Get the Decimal Suppression flag
            Dim sTempOptionValue As String = ""
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)

            If Trim(sTempOptionValue) <> "" AndAlso Trim(sTempOptionValue) = "1" Then
                IsSuppressDecimalValues = True
            End If

        Catch excep As System.Exception

            m_bInterfaceError = True

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '******************************
            Exit Sub
        End Try
    End Sub
    ''' <summary>
    ''' SetUpForm
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function SetUpForm() As Integer

        Return SetUpForm(v_bDeleteClick:=False)

    End Function
    ''' <summary>
    ''' SetUpForm
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Overloads Function SetUpForm(ByVal v_bDeleteClick As Boolean) As Integer

        Dim nResult As Integer
        Const sFunctionName As String = "SetUpForm"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            SetFieldValidation()

            GetPaymentTermProductOption()

            SetInterfaceDefaults(v_bDeleteClick)

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFeeAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function GetFeeAmount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetFeeAmount"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lTask = gPMConstants.PMEComponentAction.PMEdit Then

                If m_lFeeAmountID <> 0 Then

                    ' get fee amount details

                    lReturn = m_oBusiness.GetUWFeeAmount(v_lFeeAmountId:=m_lFeeAmountID, r_vResults:=m_vFeeAmountDetails)

                    ' if the call failed
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetUWFeeAmount Failed for Fee_Amount_Id:" & m_lFeeAmountID)
                    End If

                    ' if no details were returned...
                    If Not Information.IsArray(m_vFeeAmountDetails) Then
                        gPMFunctions.RaiseError(kMethodName, "GetUWFeeAmount Failed to return any details for Fee_Amount_Id:" & m_lFeeAmountID)
                    End If

                Else
                    gPMFunctions.RaiseError(kMethodName, "No fee amount id has been specified", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            Return result
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
    ''' <summary>
    ''' PopulateForm
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function PopulateForm() As Integer

        Return PopulateForm(v_bDeleteClick:=False)

    End Function
    ''' <summary>
    ''' PopulateForm
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function PopulateForm(ByVal v_bDeleteClick As Boolean) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "PopulateForm"

        Dim nProductId As Integer
        Dim nRiskTypeGroupId As Integer
        Dim nPerilGroupId As Integer
        Dim nFeeAmountID As Integer
        Dim crFeeAmount As Decimal
        Dim crFeePercentage As Decimal
        Dim nCurrencyID As Integer
        Dim nTransactionSubType As Integer
        Dim dtEffectiveDate As Date
        Dim nTaxGroupId As Integer
        Dim bIsTaxAppliedToCr As Boolean
        Dim bIncludeInInstalment As Boolean
        Dim bSpreadAcrossInstalment As Boolean

        Dim nMakeLiveOptions As Integer
        Dim nPaymentTerm As Integer
        Dim bNetPremiumWithTax As Boolean
        Dim bApplyProrated As Boolean
        Dim bACFeeAmountOverrideRateAmount As Boolean
        Dim bUseWhenDeleted As Boolean
        Dim bDeletedFee As Boolean

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' populate combos with array values
            PopulateCombo(m_vCurrency, cboCurrency)
            PopulateCombo(m_vPerilGroup, cboPerilGroup)
            PopulateCombo(m_vProduct, cboProduct, "(All)")
            PopulateCombo(m_vRiskTypeGroup, cboRiskTypeGroup)
            PopulateCombo(m_vTaxGroup, cboTaxGroup, "(none)")
            PopulateCombo(m_oMakeLiveOptions, cboMakeLiveOptions, "(All)")
            PopulateCombo(m_oPaymentTerm, cboPaymentTerm, "(All)")

            If m_lTask = gPMConstants.PMEComponentAction.PMEdit Then

                nFeeAmountID = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountId, 0), gPMConstants.PMEReturnCode.PMFalse)
                nProductId = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountProductId, 0), -1, False)
                nRiskTypeGroupId = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountRiskTypeGroupId, 0), -1)
                nPerilGroupId = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountPerilGroupId, 0), -1)
                'always default to display first item in the list <none> if nothing else has been selected
                nTaxGroupId = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountTaxGroupId, 0), gPMConstants.PMEReturnCode.PMFalse)
                crFeePercentage = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountFeePercentage, 0), gPMConstants.PMEReturnCode.PMFalse)
                crFeeAmount = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountFeeAmount, 0), gPMConstants.PMEReturnCode.PMFalse)
                nCurrencyID = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountCurrencyId, 0), gPMConstants.PMEReturnCode.PMFalse)

                If CLng(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountTransactionTypeID, 0), 0)) <> 0 Then
                    nTransactionSubType = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountTransactionSubType, 0), gPMConstants.PMEReturnCode.PMFalse)
                Else
                    nTransactionSubType = kTransSubTypeAll
                End If

                'changes as pervb6 code
                dtEffectiveDate = CDate(ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountEffectiveDate, 0), "00:00:00"))
                bIsTaxAppliedToCr = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountIsTaxAppliedToCr, 0), gPMConstants.PMEReturnCode.PMFalse)
                bIncludeInInstalment = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountIncludeIns, 0), gPMConstants.PMEReturnCode.PMFalse)
                bSpreadAcrossInstalment = ReplaceNullWithDefault(m_vFeeAmountDetails(ACFeeAmountSpread, 0), gPMConstants.PMEReturnCode.PMFalse)

                nMakeLiveOptions = CLng(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountMakeLiveOptions, 0), 0))
                nPaymentTerm = CLng(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountPaymentTerm, 0), 0))
                bNetPremiumWithTax = CBool(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountNetPremiumWithTax, 0), 0))
                bApplyProrated = CBool(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountApplyProrated, 0), 0))
                bACFeeAmountOverrideRateAmount = CBool(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountOverrideRateAmount, 0), 0))
                bUseWhenDeleted = CBool(ReplaceNullWithDefault(m_vFeeAmountDetails(kFeeAmountUseWhenDeleted, 0), 0))

                cboProduct.Visible = False
                optProduct.Checked = False
                cboRiskTypeGroup.Visible = False
                optRiskTypeGroup.Checked = False
                cboPerilGroup.Visible = False
                optPerilGroup.Checked = False

                optNetPremium.Checked = True
                If nProductId <> -1 Then
                    optProduct.Checked = True
                    cboProduct.Visible = True
                    fraCalculationBasis.Enabled = True
                    fraPaymentMethod.Enabled = True
                    Call SelectcboItem(cboMakeLiveOptions, nMakeLiveOptions)
                    Call SelectcboItem(cboPaymentTerm, nPaymentTerm)
                    If bNetPremiumWithTax Then
                        optNetPremiumWithTax.Checked = True
                    Else
                        optNetPremium.Checked = True
                    End If
                ElseIf nRiskTypeGroupId <> -1 Then
                    optRiskTypeGroup.Checked = True
                    cboRiskTypeGroup.Visible = True
                    SelectcboItem(cboMakeLiveOptions, 0)
                    SelectcboItem(cboPaymentTerm, 0)
                ElseIf nPerilGroupId <> -1 Then
                    optPerilGroup.Checked = True
                    cboPerilGroup.Visible = True
                    SelectcboItem(cboMakeLiveOptions, 0)
                    SelectcboItem(cboPaymentTerm, 0)
                End If

                OptPercentage.Checked = False
                OptAmount.Checked = False

                If nCurrencyID = 0 AndAlso crFeePercentage <> 0 Then
                    OptPercentage.Checked = True
                    txtRate.Text = CStr(crFeePercentage)
                    chkApplyProRated.Checked = CheckState.Unchecked
                ElseIf bApplyProrated = True Then
                    OptAmount.Checked = True
                    txtRate.Text = StringsHelper.Format(CStr(crFeeAmount), "0.00")
                    If bApplyProrated Then
                        chkApplyProRated.Checked = 1
                    Else
                        chkApplyProRated.Checked = 0
                    End If
                ElseIf CStr(crFeeAmount) <> "" AndAlso bApplyProrated = False AndAlso crFeePercentage = 0 Then
                    txtRate.Text = CStr(crFeeAmount)
                    OptAmount.Checked = True
                End If

                If crFeePercentage = 0 And crFeeAmount = 0 And nCurrencyID > 0 Then
                    txtRate.Text = StringsHelper.Format(CStr(crFeeAmount), "0.00")
                    OptAmount.Checked = True
                ElseIf crFeePercentage = 0 And crFeeAmount = 0 And nCurrencyID <= 0 Then
                    txtRate.Text = 0
                    OptPercentage.Checked = True
                End If

                optNewBusiness.Checked = False
                optAdditionalMTA.Checked = False
                OptReturnMTA.Checked = False
                OptCancellation.Checked = False
                OptRenewal.Checked = False
                OptReInstatement.Checked = False
                optAll.Checked = True
                chkOverrideFee.Enabled = False

                Select Case nTransactionSubType
                    Case gPMConstants.kTransSubTypeNB
                        optNewBusiness.Checked = True
                    Case gPMConstants.kTransSubTypeAdditionMTA
                        optAdditionalMTA.Checked = True
                    Case gPMConstants.kTransSubTypeReturnMTA
                        OptReturnMTA.Checked = True
                    Case gPMConstants.kTransSubTypeCancellation
                        OptCancellation.Checked = True
                    Case gPMConstants.kTransSubTypeRenewal
                        OptRenewal.Checked = True
                    Case gPMConstants.kTransSubTypeReInstatement
                        OptReInstatement.Checked = True
                    Case 0
                        chkOverrideFee.Enabled = True
                End Select
                If optAll.Checked Then
                    chkOverrideFee.Enabled = True
                End If
                txtEffectiveDate.Text = dtEffectiveDate.ToString("dd/MM/yyyy")

                If bIsTaxAppliedToCr Then
                    chkApplyToCreditTransaction.CheckState = CheckState.Checked
                Else
                    chkApplyToCreditTransaction.CheckState = CheckState.Unchecked
                End If
                If bIncludeInInstalment Then
                    chkIncludeToInstalment.CheckState = CheckState.Checked
                Else
                    chkIncludeToInstalment.CheckState = CheckState.Unchecked
                End If
                If bSpreadAcrossInstalment Then

                    chkSpreadAcrossInstalment.CheckState = CheckState.Checked
                Else
                    chkSpreadAcrossInstalment.CheckState = CheckState.Unchecked
                End If

                SelectcboItem(cboProduct, nProductId)
                SelectcboItem(cboRiskTypeGroup, nRiskTypeGroupId)
                SelectcboItem(cboPerilGroup, nPerilGroupId)
                If nCurrencyID > 0 Then
                    SelectcboItem(cboCurrency, nCurrencyID)
                End If
                SelectcboItem(cboTaxGroup, nTaxGroupId)

                If bACFeeAmountOverrideRateAmount Then
                    chkOverrideFee.CheckState = CheckState.Checked
                Else
                    chkOverrideFee.CheckState = CheckState.Unchecked
                End If

                If bUseWhenDeleted Then
                    chkUseWhenDeleted.CheckState = CheckState.Checked
                Else
                    chkUseWhenDeleted.CheckState = CheckState.Unchecked
                End If

                m_bDeleteClick = v_bDeleteClick

                If v_bDeleteClick Then
                    fraFeeAppliesTo.Enabled = False
                    fraEffectiveTrans.Enabled = False
                    fraTax.Enabled = False
                    Instalment.Enabled = False
                    chkApplyProRated.Enabled = False
                    txtRate.Enabled = False
                    cboCurrency.Enabled = False
                    OptAmount.Enabled = False
                    OptPercentage.Enabled = False
                    chkUseWhenDeleted.Enabled = False
                End If
            Else
                ' PMAdd
                optProduct.Checked = True
                OptPercentage.Checked = True
                optNewBusiness.Checked = True
                optNetPremium.Checked = True
                txtEffectiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy")
                optAll.Checked = True
                chkOverrideFee.Enabled = True
                SelectcboItem(cboTaxGroup, 0)
                SelectcboItem(cboMakeLiveOptions, 0)
                SelectcboItem(cboPaymentTerm, 0)

            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Save
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Save() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "Save"
        Dim nFeeAmountID As Integer
        Dim nRiskTypeGroupId As Integer
        Dim crFeePercentage As Decimal
        Dim crFeeAmount As Decimal
        Dim nTransactionTypeId As Integer
        Dim nCurrencyID As Integer
        Dim nProductId As Integer
        Dim nPerilGroupId As Integer
        Dim nTransactionSubType As Integer
        Dim nTaxGroupId As Integer
        Dim nIsFeeAppliedToCr As Integer
        Dim dtEffectiveDate As Date
        Dim nIncludeToInstalment As Integer
        Dim nSpreadAcrossInstalment As Integer

        Dim nMakeLiveOptions As Integer
        Dim nPaymentTerm As Integer
        Dim nApplyProrated As Integer
        Dim nOverrideRateAmount As Integer
        Dim bNetPremiumWithTax As Boolean
        Dim nUseWhenDeleted As Integer

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            nResult = ValidateFormData()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = CType(GetScreenValues(r_lFeeAmountId:=nFeeAmountID, r_lRiskTypeGroupId:=nRiskTypeGroupId, r_crFeepercentage:=crFeePercentage, r_crFeeAmount:=crFeeAmount, r_lTransactionTypeId:=nTransactionTypeId,
                                            r_lCurrencyId:=nCurrencyID, r_lProductId:=nProductId, r_lPerilGroupId:=nPerilGroupId, r_lTransactionSubType:=nTransactionSubType,
                                            r_lTaxGroupId:=nTaxGroupId, r_lIsFeeAppliedToCr:=nIsFeeAppliedToCr, r_dtEffectiveDate:=dtEffectiveDate,
                                            r_lIncludeToInstalment:=nIncludeToInstalment, r_lSpreadAcrossInstalment:=nSpreadAcrossInstalment, r_nMakeLiveOptions:=nMakeLiveOptions,
                                            r_nPaymentTerm:=nPaymentTerm, r_bNetPremiumWithTax:=bNetPremiumWithTax, r_nApplyProrated:=nApplyProrated,
                                            r_nOverrideRateAmount:=nOverrideRateAmount, r_nUseWhenDeleted:=nUseWhenDeleted), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetScreenValues", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_sUniqueId = "" Then
                m_sUniqueId = gPMFunctions.GetUniqueID()
            End If

            If m_sScreenHeirarchy <> "" Then
                m_sScreenHeirarchy = GetScreenHierarchyName() & $"/({txtEffectiveDate.Text.Trim()})"
            End If
            If m_lTask = gPMConstants.PMEComponentAction.PMEdit Then

                nResult = m_oBusiness.EditUWFeeAmount(v_vFeeAmountId:=nFeeAmountID, v_vRiskTypeGroupId:=nRiskTypeGroupId, v_vFeepercentage:=crFeePercentage,
                                                      v_vFeeAmount:=crFeeAmount, v_vTransactionTypeId:=nTransactionTypeId, v_vCurrencyId:=nCurrencyID, v_vProductId:=nProductId,
                                                      v_vPerilGroupId:=nPerilGroupId, v_vTransactionSubType:=nTransactionSubType, v_vTaxGroupId:=nTaxGroupId,
                                                      v_vIsFeeAppliedToCr:=nIsFeeAppliedToCr, v_vEffectiveDate:=dtEffectiveDate, v_vIncludeToInstalment:=nIncludeToInstalment,
                                                      v_vSpreadAcrossInstalment:=nSpreadAcrossInstalment, v_oMakeLiveOptions:=nMakeLiveOptions, v_oPaymentTerm:=nPaymentTerm,
                                                      v_oNetPremiumWithTax:=bNetPremiumWithTax, v_oApplyProrated:=nApplyProrated, v_oOverrideRateAmount:=nOverrideRateAmount, v_nUseWhenDeleted:=nUseWhenDeleted, v_sUniqueId:=m_sUniqueId, v_sScreenHeirarchy:=m_sScreenHeirarchy)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "EditUWFeeAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            ElseIf m_lTask = gPMConstants.PMEComponentAction.PMAdd Then

                nResult = m_oBusiness.AddUWFeeAmount(v_vRiskTypeGroupId:=nRiskTypeGroupId, v_vFeepercentage:=crFeePercentage, v_vFeeAmount:=crFeeAmount, v_vTransactionTypeId:=nTransactionTypeId, v_vCurrencyId:=nCurrencyID, v_vProductId:=nProductId, v_vPerilGroupId:=nPerilGroupId, v_vTransactionSubType:=nTransactionSubType, v_vTaxGroupId:=nTaxGroupId, v_vIsFeeAppliedToCr:=nIsFeeAppliedToCr, v_vEffectiveDate:=dtEffectiveDate, v_vPartyCnt:=m_lPartyCnt, v_vIncludeToInstalment:=nIncludeToInstalment, v_vSpreadAcrossInstalment:=nSpreadAcrossInstalment, v_oMakeLiveOptions:=nMakeLiveOptions, v_oPaymentTerm:=nPaymentTerm, v_oNetPremiumWithTax:=bNetPremiumWithTax, v_oApplyProrated:=nApplyProrated, v_oOverrideRateAmount:=nOverrideRateAmount, v_nUseWhenDeleted:=nUseWhenDeleted, v_sUniqueId:=m_sUniqueId, v_sScreenHeirarchy:=m_sScreenHeirarchy)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddUWFeeAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If
            If m_bDeleteClick Then
                Dim sConfirmDelete As String
                sConfirmDelete = CStr(MessageBox.Show("Fee will be deleted and will not display on the Fee Account screen", "Confirm Delete", MessageBoxButtons.OKCancel))
                Select Case sConfirmDelete
                    Case CStr(System.Windows.Forms.DialogResult.Cancel)
                        Exit Function
                End Select

            End If
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Me.Hide()

            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function SetInterfaceDefaults() As Integer

        Return SetInterfaceDefaults(v_bDeleteClick:=False)

    End Function
    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <param name="v_bDeleteClick"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function SetInterfaceDefaults(ByVal v_bDeleteClick As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = SetCaptions()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = GetLookups()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookups Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_lTask = gPMConstants.PMEComponentAction.PMEdit Then
                lReturn = GetFeeAmount()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetFeeAmount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            lReturn = PopulateForm(v_bDeleteClick)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateForm Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

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
    ' Name: GetLookupValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function GetLookupValues(ByVal v_sTableName As String, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupValues"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oBusiness.GetTableLookupValues(v_sTableName:=v_sTableName, r_vResults:=r_vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed for table :" & v_sTableName)
            End If

            If Not Information.IsArray(r_vArray) Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed to return any values for table :" & v_sTableName)
            End If

            Return result
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
    ' Name: GetArrayItem
    '
    ' Parameters: n/a
    '
    ' Description: Returns the code for a specifed item description
    '                  in a specified lookup table..
    '
    ' History:
    '           Created : MEvans : 07-11-2003 : CQ3077
    ' ***************************************************************** '

    'Private Function GetArrayItem(ByVal v_vArray( ,  ) As Object, ByRef r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const sFunctionName As String = "GetArrayItem"
    '
    '
    'Dim v_vLookupItem As String = ""
    'Dim lLookupItem, lLBound, lUBound As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' set lookup properties
    'If r_lItemId <> StringsHelper.ToDoubleSafe("") Then
    'v_vLookupItem = CStr(r_lItemId)
    'lLookupItem = 0
    '
    'ElseIf r_sItemDesc <> "" Then 
    'v_vLookupItem = r_sItemDesc
    'lLookupItem = 1
    '
    'ElseIf r_sItemCode <> "" Then 
    'v_vLookupItem = r_sItemCode
    'lLookupItem = 2
    'End If
    '
    ' loop around the available items in the specified array
    'For 'lItem As Integer = lLBound To lUBound
    '
    ' look for a match

    'If CStr(v_vArray(lLookupItem, lItem)).Trim() = v_vLookupItem Then
    '
    ' return the requested code, id, description

    'r_sItemDesc = CStr(v_vArray(ACDetailDesc, lItem)).Trim()

    'r_sItemCode = CStr(v_vArray(ACDetailCode, lItem)).Trim()

    'r_lItemId = CInt(CStr(v_vArray(ACDetailKey, lItem)).Trim())
    '
    'Exit For
    'End If
    '
    'Next lItem
    '
    ' if we dont find the values specified then return false
    'If r_sItemCode = "" Then
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMerror
    '
    '******************************
    ' Log Error.
    'gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '*******************************
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count
                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

            End If

            If bItemNotFound And v_lSelectedId <> -1 Then

                ' log that we havent found the specified item			
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) &
                                              " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
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


    ' ***************************************************************** '
    ' Name: SetCaptions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-04-2005 : AUS005
    ' ***************************************************************** '
    Public Function SetCaptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetCaptions"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue





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
    ' Name: GetLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetLookups() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookups"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetLookupValues(kTableCurrency, m_vCurrency), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetLookupValues(kTableProduct, m_vProduct), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetLookupValues(kTablePerilGroup, m_vPerilGroup), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetLookupValues(kTableRiskTypeGroup, m_vRiskTypeGroup), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetLookupValues(kTableTaxGroup, m_vTaxGroup), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetLookupValues(kTableMakeLiveOptions, m_oMakeLiveOptions), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = CType(GetLookupValues(kTableDoPaymentTerms, m_oPaymentTerm), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: PopulateCombo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function PopulateCombo(ByVal v_vArray(,) As Object, ByVal oComboBox As ComboBox, Optional ByVal sFirstItem As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateCombo"

        Dim lReturn, lLBound, lUBound As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If sFirstItem <> "" Then
                Dim oComboBox_NewIndex As Integer = -1
                oComboBox_NewIndex = oComboBox.Items.Add(sFirstItem)
                VB6.SetItemData(oComboBox, oComboBox_NewIndex, 0)
            End If

            If Information.IsArray(v_vArray) Then

                lLBound = v_vArray.GetLowerBound(1)
                lUBound = v_vArray.GetUpperBound(1)

                For lItem As Integer = lLBound To lUBound

                    oComboBox.Items.Add(New VB6.ListBoxItem(v_vArray(MainModule.ACDetailDesc, lItem), CInt(v_vArray(MainModule.ACDetailKey, lItem))))
                Next

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
    ' Name: ReplaceNullWithDefault
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-06-2004 : CQ4740
    ' ***************************************************************** '

    Private Function ReplaceNullWithDefault(ByRef v_vValue As Object, ByVal v_vDefault As Object, Optional ByVal v_bIncludeZeroValuesAsNull As Object = Nothing) As Object

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const sFunctionName As String = "ReplaceNullWithDefault"

        Try

            Dim bProcessed As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue



            If Convert.IsDBNull(v_vValue) OrElse IsNothing(v_vValue) OrElse v_vValue = "" Then
                v_vValue = v_vDefault
                bProcessed = True
            End If

            If Not bProcessed Then
                If v_bIncludeZeroValuesAsNull Then
                    If v_vValue = gPMConstants.PMEReturnCode.PMFalse Then
                        v_vValue = v_vDefault
                    End If
                End If
            End If


            Return v_vValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)

            Return result

        End Try
    End Function

    Private isInitializingComponent As Boolean
    Private Sub OptPercentage_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptPercentage.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetupFeeAmount()
        End If
    End Sub

    Private Sub optPerilGroup_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optPerilGroup.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetupAppliesTo()
        End If
    End Sub

    Private Sub optProduct_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optProduct.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetupAppliesTo()
        End If
    End Sub

    Private Sub optRiskTypeGroup_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optRiskTypeGroup.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetupAppliesTo()
        End If
    End Sub
    ''' <summary>
    ''' SetupAppliesTo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetupAppliesTo() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "SetupAppliesTo"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If optRiskTypeGroup.Checked Then
                cboRiskTypeGroup.BringToFront()
                cboRiskTypeGroup.Visible = True
                cboProduct.Visible = False
                cboPerilGroup.Visible = False

                optNetPremium.Checked = True
                fraCalculationBasis.Enabled = False
                cboPaymentTerm.Enabled = False
                fraPaymentMethod.Enabled = False
                optAll.Enabled = False
                If optAll.Checked = True Then
                    optNewBusiness.Checked = True
                    chkOverrideFee.Checked = False
                    chkOverrideFee.Enabled = True
                End If
                SelectcboItem(cboMakeLiveOptions, 0)
                SelectcboItem(cboPaymentTerm, 0)
            ElseIf optProduct.Checked Then
                cboProduct.BringToFront()
                cboProduct.Visible = True
                cboRiskTypeGroup.Visible = False
                cboPerilGroup.Visible = False

                fraCalculationBasis.Enabled = True
                fraPaymentMethod.Enabled = True
                optAll.Enabled = True
                ' optAll.Checked = True
                ' chkOverrideFee.Enabled = True
                If m_oEnableDebitOrder And UCase(cboMakeLiveOptions.Text) = "INVOICE" Then
                    cboPaymentTerm.Enabled = True
                Else
                    cboPaymentTerm.Enabled = False
                End If
            ElseIf optPerilGroup.Checked Then
                cboPerilGroup.BringToFront()
                cboPerilGroup.Visible = True
                cboRiskTypeGroup.Visible = False
                cboProduct.Visible = False
                optNetPremium.Checked = True
                fraCalculationBasis.Enabled = False
                cboPaymentTerm.Enabled = False
                fraPaymentMethod.Enabled = False
                optAll.Enabled = False
                If optAll.Checked = True Then
                    optNewBusiness.Checked = True
                    chkOverrideFee.Checked = False
                    chkOverrideFee.Enabled = True
                End If
                SelectcboItem(cboMakeLiveOptions, 0)
                SelectcboItem(cboPaymentTerm, 0)
            End If

            If OptAmount.Checked = True Then
                chkApplyProRated.Visible = True
            Else
                chkApplyProRated.Visible = False
            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupFeeAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetupFeeAmount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupFeeAmount"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If OptPercentage.Checked Then
                cboCurrency.Visible = False
                lblCurrency.Visible = False
                chkApplyProRated.Visible = False
            ElseIf OptAmount.Checked Then
                cboCurrency.Visible = True
                lblCurrency.Visible = True
                chkApplyProRated.Visible = True
            End If

            If optAll.Checked = True Then
                chkOverrideFee.Enabled = True
            Else
                chkOverrideFee.Enabled = False
                chkOverrideFee.Checked = False
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



    Private Sub OptAmount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OptAmount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            If IsSuppressDecimalValues Then txtRate.Text = 0.0
            SetupFeeAmount()

        End If
    End Sub
    ''' <summary>
    ''' GetScreenValues
    ''' </summary>
    ''' <param name="r_lFeeAmountId"></param>
    ''' <param name="r_lRiskTypeGroupId"></param>
    ''' <param name="r_crFeepercentage"></param>
    ''' <param name="r_crFeeAmount"></param>
    ''' <param name="r_lTransactionTypeId"></param>
    ''' <param name="r_lCurrencyId"></param>
    ''' <param name="r_lProductId"></param>
    ''' <param name="r_lPerilGroupId"></param>
    ''' <param name="r_lTransactionSubType"></param>
    ''' <param name="r_lTaxGroupId"></param>
    ''' <param name="r_lIsFeeAppliedToCr"></param>
    ''' <param name="r_dtEffectiveDate"></param>
    ''' <param name="r_lIncludeToInstalment"></param>
    ''' <param name="r_lSpreadAcrossInstalment"></param>
    ''' <param name="r_nMakeLiveOptions"></param>
    ''' <param name="r_nPaymentTerm"></param>
    ''' <param name="r_bNetPremiumWithTax"></param>
    ''' <param name="r_nApplyProrated"></param>
    ''' <param name="r_nOverrideRateAmount"></param>
    ''' <param name="r_nUseWhenDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetScreenValues(ByRef r_lFeeAmountId As Integer, ByRef r_lRiskTypeGroupId As Integer, ByRef r_crFeepercentage As Object, ByRef r_crFeeAmount As Object, ByRef r_lTransactionTypeId As Integer, ByRef r_lCurrencyId As Integer, ByRef r_lProductId As Integer, ByRef r_lPerilGroupId As Integer, ByRef r_lTransactionSubType As Integer, ByRef r_lTaxGroupId As Integer, ByRef r_lIsFeeAppliedToCr As Integer, ByRef r_dtEffectiveDate As Date, ByRef r_lIncludeToInstalment As Integer, ByRef r_lSpreadAcrossInstalment As Integer, ByRef r_nMakeLiveOptions As Integer, ByRef r_nPaymentTerm As Integer, ByRef r_bNetPremiumWithTax As Boolean, ByRef r_nApplyProrated As Integer, ByRef r_nOverrideRateAmount As Integer, ByRef r_nUseWhenDeleted As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetScreenValues"
        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lTask = gPMConstants.PMEComponentAction.PMEdit Then
                r_lFeeAmountId = m_lFeeAmountID
            End If

            '*****************************
            '*****************************
            ' fee applies to

            r_lRiskTypeGroupId = 0
            r_lPerilGroupId = 0
            r_lProductId = 0
            r_bNetPremiumWithTax = False
            If optProduct.Checked Then
                r_lProductId = VB6.GetItemData(cboProduct, cboProduct.SelectedIndex)
                r_nMakeLiveOptions = VB6.GetItemData(cboMakeLiveOptions, cboMakeLiveOptions.SelectedIndex)
                r_nPaymentTerm = VB6.GetItemData(cboPaymentTerm, cboPaymentTerm.SelectedIndex)

                If optNetPremiumWithTax.Checked = True Then
                    r_bNetPremiumWithTax = True
                Else
                    r_bNetPremiumWithTax = False
                End If
            End If

            If optRiskTypeGroup.Checked Then
                r_lRiskTypeGroupId = VB6.GetItemData(cboRiskTypeGroup, cboRiskTypeGroup.SelectedIndex)
            End If

            If optPerilGroup.Checked Then
                r_lPerilGroupId = VB6.GetItemData(cboPerilGroup, cboPerilGroup.SelectedIndex)
            End If

            '*****************************
            '*****************************
            ' fee amount

            r_crFeepercentage = 0
            r_crFeeAmount = 0
            r_lCurrencyId = 0
            r_nApplyProrated = 0
            If OptAmount.Checked Then
                r_crFeeAmount = CDec(txtRate.Text)
                r_lCurrencyId = VB6.GetItemData(cboCurrency, cboCurrency.SelectedIndex)
                r_nApplyProrated = ToSafeInteger(Math.Abs(chkApplyProRated.CheckState))
            End If

            If OptPercentage.Checked Then
                r_crFeepercentage = CDec(txtRate.Text)
            End If

            '*****************************
            '*****************************
            ' effective transactions

            If optNewBusiness.Checked Then
                r_lTransactionSubType = 0
                r_lTransactionTypeId = 4
            ElseIf optAdditionalMTA.Checked Then
                r_lTransactionSubType = 1
                r_lTransactionTypeId = 9
            ElseIf OptReturnMTA.Checked Then
                r_lTransactionSubType = 2
                r_lTransactionTypeId = 9
            ElseIf OptCancellation.Checked Then
                r_lTransactionSubType = 3
                r_lTransactionTypeId = 7
            ElseIf OptRenewal.Checked Then
                r_lTransactionSubType = 4
                r_lTransactionTypeId = 10
            ElseIf OptReInstatement.Checked Then
                r_lTransactionSubType = 5
                r_lTransactionTypeId = 20
            Else
                r_lTransactionSubType = 6
                r_lTransactionTypeId = 0
            End If

            r_dtEffectiveDate = ToSafeDate(txtEffectiveDate.Text)

            '*****************************
            '*****************************
            ' Tax
            r_lTaxGroupId = VB6.GetItemData(cboTaxGroup, cboTaxGroup.SelectedIndex)
            r_lIsFeeAppliedToCr = ToSafeInteger(Math.Abs(ToSafeInteger(chkApplyToCreditTransaction.CheckState)))
            r_lIncludeToInstalment = ToSafeInteger(Math.Abs(chkIncludeToInstalment.CheckState))
            r_lSpreadAcrossInstalment = ToSafeInteger(Math.Abs(chkSpreadAcrossInstalment.CheckState))
            r_nOverrideRateAmount = ToSafeInteger(Math.Abs(chkOverrideFee.CheckState))
            r_nUseWhenDeleted = ToSafeInteger(chkUseWhenDeleted.CheckState)

        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateFormData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ValidateFormData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateFormData"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            '*************************
            '*************************
            ' fee applies to

            If optProduct.Checked Then
                If cboProduct.SelectedIndex = -1 Then
                    MessageBox.Show("A product must be selected.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboProduct.Focus()
                    Return result
                End If
            End If

            If optRiskTypeGroup.Checked Then
                If cboRiskTypeGroup.SelectedIndex = -1 Then
                    MessageBox.Show("A risk type group must be selected.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboRiskTypeGroup.Focus()
                    Return result
                End If
            End If

            If optPerilGroup.Checked Then
                If cboPerilGroup.SelectedIndex = -1 Then
                    MessageBox.Show("A peril group must be selected.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboPerilGroup.Focus()
                    Return result
                End If
            End If

            '*************************
            '*************************
            ' fee amount

            If txtRate.Text = "" Then
                MessageBox.Show("A rate must be specified.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtRate.Focus()
                Return result
            End If

            If OptAmount.Checked Then
                If cboCurrency.SelectedIndex = -1 Then
                    MessageBox.Show("A currency must be selected.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboCurrency.Focus()
                    Return result
                End If
                If ToSafeInteger(txtRate.Text) < 0 Then
                    MessageBox.Show("A Value must be greater than 0.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtRate.Focus()
                    Return result
                End If
            ElseIf OptPercentage.Checked Then
                If CInt(txtRate.Text) < 0 Or CInt(txtRate.Text) > 100 Then
                    MessageBox.Show("A percentage rate must be between (0-100).", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtRate.Focus()
                    Return result
                End If
            End If

            '*************************
            '*************************
            ' effective transactions

            If txtEffectiveDate.Text = "" Then
                MessageBox.Show("An effective date must be specified.", "Data Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtEffectiveDate.Focus()
                Return result
            End If

            result = gPMConstants.PMEReturnCode.PMTrue


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
    ' Name: SetFieldValidation
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRate, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddNewFormField Failed txtRate", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddNewFormField Failed txtEffectiveDate", gPMConstants.PMELogLevel.PMLogError)
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

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRate)
    End Sub

    Private Sub txtRate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRate.KeyPress
        If IsSuppressDecimalValues AndAlso OptAmount.Checked = True Then
            'Disallow the decimals
            gPMFunctions.NumPress(sender, e)
        End If
    End Sub

    Private Sub txtRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRate)
    End Sub

    Private Sub frmInterfaceUW_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt And e.KeyCode = Keys.D1 Then
            TabMain.SelectedIndex = 0
        End If
    End Sub
    ''' <summary>
    ''' cboMakeLiveOptions_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cboMakeLiveOptions_Click(sender As Object, e As EventArgs) Handles cboMakeLiveOptions.Click, cboMakeLiveOptions.SelectedIndexChanged
        If m_oEnableDebitOrder AndAlso UCase(cboMakeLiveOptions.Text) = "INVOICE" Then
            cboPaymentTerm.Enabled = True
        Else
            cboPaymentTerm.Enabled = False
            SelectcboItem(cboPaymentTerm, 0)
        End If
    End Sub
    ''' <summary>
    ''' optAdditionalMTA_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub optAdditionalMTA_Click(sender As Object, e As EventArgs) Handles optAdditionalMTA.Click
        SetupFeeAmount()
    End Sub
    ''' <summary>
    ''' optAll_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub optAll_Click(sender As Object, e As EventArgs) Handles optAll.Click
        SetupFeeAmount()
    End Sub
    ''' <summary>
    ''' OptCancellation_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OptCancellation_Click(sender As Object, e As EventArgs) Handles OptCancellation.Click
        SetupFeeAmount()
    End Sub
    ''' <summary>
    ''' optNewBusiness_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub optNewBusiness_Click(sender As Object, e As EventArgs) Handles optNewBusiness.Click
        SetupFeeAmount()
    End Sub

    Private Sub OptReInstatement_Click(sender As Object, e As EventArgs) Handles OptReInstatement.Click
        SetupFeeAmount()
    End Sub

    Private Sub OptRenewal_Click(sender As Object, e As EventArgs) Handles OptRenewal.Click
        SetupFeeAmount()
    End Sub

    Private Sub OptReturnMTA_Click(sender As Object, e As EventArgs) Handles OptReturnMTA.Click
        SetupFeeAmount()
    End Sub

    ''' <summary>
    ''' GetPaymentTermProductOption
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPaymentTermProductOption() As Integer

        Const kMethodName As String = "GetPaymentTermProductOption"

        Dim nReturn As Integer

        Try
            GetPaymentTermProductOption = gPMConstants.PMEReturnCode.PMTrue

            nReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDebitOrder, 1, m_oEnableDebitOrder)

            If Trim(m_oEnableDebitOrder) = "" Or Len(Trim(m_oEnableDebitOrder)) < 1 Then
                m_oEnableDebitOrder = "0"
            End If
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "getProductOptionValue", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return nReturn
        Catch ex As Exception
            nReturn = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentTermProductOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

            Return nReturn
        End Try

    End Function

    Public Function GetScreenHierarchyName() As String

        If optProduct.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & $"/Product({cboProduct.SelectedItem.ToString.Trim()})"
        ElseIf optRiskTypeGroup.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & $"/RiskTypeGroup({cboRiskTypeGroup.SelectedItem.ToString.Trim()})"
        ElseIf optPerilGroup.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & $"/PerilGroup({cboPerilGroup.SelectedItem.ToString.Trim()})"
        End If

        If OptReInstatement.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(Re-Instatement)"
        ElseIf optAdditionalMTA.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(Additional MTA)"
        ElseIf optNewBusiness.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(New Business)"
        ElseIf OptReturnMTA.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(Return MTA)"
        ElseIf OptCancellation.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(Cancellation)"
        ElseIf OptRenewal.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(Renewal)"
        ElseIf optAll.Checked Then
            m_sScreenHeirarchy = m_sScreenHeirarchy & "/(All)"
        End If

        Return m_sScreenHeirarchy
    End Function
End Class
