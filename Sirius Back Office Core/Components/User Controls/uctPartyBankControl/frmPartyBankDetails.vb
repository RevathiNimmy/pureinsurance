Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports PMLookupControl
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmPartyBankDetails
    Inherits System.Windows.Forms.Form
    'For Add - set last value of a array to 1
    'For Edit - set last value of a array to 2
    'For Delete - set last value of a array to 3

    Dim sManualAuthCode As String = String.Empty
    Private Const ACClass As String = "uctPartyBankControl"

    Private m_vBankItem() As Object
    Private vOldBankItem() As Object

    Private m_vOldBankItem As Object
    Private m_vSelectedPaymentTypes() As Object

    Private m_iTask As PMEComponentAction
    Private m_lStatus As PMEReturnCode
    Private m_lReturn As Integer

    Private m_sPartyName As String = ""
    Private m_sBankPaymentTypeCode As String = ""
    Private m_ENMediaType As MediaType

    Private m_bMultiSelect As Boolean

    Private m_bSamePaymentTypes As Boolean


    Private m_oBusiness As bSIRPartyBank.Business

    Private m_vPartyCnt As Object

    Private m_sAccountType As String = ""
    Private m_bBankDetailsEdited As Boolean
    Private m_bPartyBankArrayExists As Boolean
    Private m_bMediaTypeChanged As Boolean
    Private m_iOriginalMediaType As MediaType
    Private Enum MediaType
        IsBank = 0
        IsCreditCard = 1
    End Enum
    Public Property ResetPreviousOne As Boolean = False
    Public WriteOnly Property PartyName() As String
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property

    Public WriteOnly Property MultiSelect() As Boolean
        Set(ByVal Value As Boolean)
            m_bMultiSelect = Value
        End Set
    End Property

    Public WriteOnly Property SamePaymentTypes() As Boolean
        Set(ByVal Value As Boolean)
            m_bSamePaymentTypes = Value
        End Set
    End Property

    Public WriteOnly Property SetBusiness() As bSIRPartyBank.Business
        Set(ByVal Value As bSIRPartyBank.Business)
            m_oBusiness = Value
        End Set
    End Property

    Public WriteOnly Property PartyCnt() As Object
        Set(ByVal Value As Object)


            m_vPartyCnt = Value
        End Set
    End Property

    Public WriteOnly Property AccountType() As String
        Set(ByVal Value As String)
            m_sAccountType = Value
        End Set
    End Property

    Private ReadOnly Property MediaType_Renamed() As MediaType
        Get
            Return m_ENMediaType
        End Get
    End Property

    Public WriteOnly Property BankPaymentTypeCode() As String
        Set(ByVal Value As String)
            m_sBankPaymentTypeCode = Value
        End Set
    End Property
    Public ReadOnly Property IsMediaTypeChanged() As Boolean
        Get
            '    IsMediaTypeChanged = (m_iOriginalMediaType = MediaType)
            'developer guide no. 108
            Return Not (m_iOriginalMediaType.IsBank = MediaType_Renamed Or m_iOriginalMediaType.IsCreditCard = MediaType_Renamed)
        End Get
    End Property

    Public Property DefaultBankPaymentType() As String
    Public Property DefaultAccountType() As String
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property BankItem() As Object
        Get
            Return VB6.CopyArray(m_vBankItem)
        End Get
        Set(ByVal Value As Object)
            m_vBankItem = Value
        End Set
    End Property

    Public Property OldBankItem() As Object
        Get
            Return m_vOldBankItem
        End Get
        Set(ByVal Value As Object)


            m_vOldBankItem = Value
        End Set
    End Property


    Public Property SelectedPaymentTypes() As Object
        Get
            Return VB6.CopyArray(m_vSelectedPaymentTypes)
        End Get
        Set(ByVal Value As Object)

            If Not Object.Equals(Value, Nothing) Then

                m_vSelectedPaymentTypes = Value
            End If
        End Set
    End Property

    Private Sub chkIsRegistered_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsRegistered.CheckStateChanged
        If chkIsRegistered.CheckState = CheckState.Checked Then
            uctPMAddressCC.Enabled = False
            uctPMAddressCC.SetMandatoryFlagAddressLine1 = False
            uctPMAddressCC.AddressLine1 = ""
            uctPMAddressCC.AddressLine2 = ""
            uctPMAddressCC.AddressLine3 = ""
            uctPMAddressCC.AddressLine4 = ""
            uctPMAddressCC.PostCode = ""
        ElseIf chkIsRegistered.CheckState = CheckState.Unchecked Then
            uctPMAddressCC.Enabled = True
            uctPMAddressCC.SetMandatoryFlagAddressLine1 = True
            If Not IsArrayEmpty(m_vBankItem) Then
                'Developer Guide No 149
                uctPMAddressCC.AddressLine1 = Convert.ToString(m_vBankItem(MainModule.ENPartyBank.CCAdd1))
                uctPMAddressCC.AddressLine2 = Convert.ToString(m_vBankItem(MainModule.ENPartyBank.CCAdd2))
                uctPMAddressCC.AddressLine3 = Convert.ToString(m_vBankItem(MainModule.ENPartyBank.CCTown))
                uctPMAddressCC.AddressLine4 = Convert.ToString(m_vBankItem(MainModule.ENPartyBank.CCAdd3))
                uctPMAddressCC.PostCode = Convert.ToString(m_vBankItem(MainModule.ENPartyBank.CCPCode))
                If Information.IsArray(m_vBankItem(MainModule.ENPartyBank.CCCountry)) Then

                    If CStr(m_vBankItem(MainModule.ENPartyBank.CCCountry)(MainModule.ENPMLookups.Id)) <> "" Then

                        uctPMAddressCC.CountryId = CInt(m_vBankItem(MainModule.ENPartyBank.CCCountry)(MainModule.ENPMLookups.Id))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = PMEReturnCode.PMCancel
        'Start(Sriram P)61128


        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)


        If iMsgResult = System.Windows.Forms.DialogResult.No Then

        Else
            Me.Close()
        End If

        'End(Sriram P)61128
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optBankAccount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optBankAccount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SSTabHelper.SetTabVisible(SSBankDetails, 0, True)
            'developer guide no. 
            'TODO: Need to discuss this baffling behavior. If the below line of code is commented,
            ' controls under Bank tab are not displayed
            SSTabHelper.SetTabEnabled(SSBankDetails, 0, True)
            SSTabHelper.SetSelectedIndex(SSBankDetails, 0)
            SSTabHelper.SetTabEnabled(SSBankDetails, 1, False)
            SSTabHelper.SetTabVisible(SSBankDetails, 1, False)
            m_ENMediaType = MediaType.IsBank
        End If
    End Sub

    Private Sub optCreditCard_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optCreditCard.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SSTabHelper.SetTabVisible(SSBankDetails, 1, True)
            'developer guide no. 
            'TODO: Need to discuss this baffling behavior. If the below line of code is commented,
            ' controls under Bank tab are not displayed
            SSTabHelper.SetTabEnabled(SSBankDetails, 1, True)
            SSTabHelper.SetSelectedIndex(SSBankDetails, 1)
            SSTabHelper.SetTabEnabled(SSBankDetails, 0, False)
            SSTabHelper.SetTabVisible(SSBankDetails, 0, False)
            m_ENMediaType = MediaType.IsCreditCard
            If m_iTask = PMEComponentAction.PMAdd Then
                chkIsRegistered.CheckState = CheckState.Checked
                uctPMAddressCC.SetMandatoryFlagAddressLine1 = False
            End If
            With uctCreditCardDetails
                .ViewOnlyMode = False
                .Initialise()
                .DefaultBankPaymentType = DefaultBankPaymentType
                .DefaultAccountType = DefaultAccountType
                .ShowPartyCreditCardScreen()
            End With

        End If
    End Sub
    ' ***************************************************************** '
    ' Name: SetProcessModes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-07-2007 :
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"

        Dim lReturn As Integer

        Try



            result = PMEReturnCode.PMTrue
            '

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

    Private Function ValidateCC() As Integer

        Dim nResult As Integer
        Const kMethodName As String = "ValidateCC"

        Try
            nResult = PMEReturnCode.PMTrue

            If txtAccountType.Text.Trim() = "" Then
                MsgBox("Please Specify Account Type.", vbInformation, "Mandatory Field")
                txtAccountType.Focus()
                Return PMEReturnCode.PMFalse
            End If
            nResult = uctCreditCardDetails.Validate()
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If chkIsRegistered.CheckState = CheckState.Unchecked Then
                If uctPMAddressCC.AddressLine1 = "" Then
                    MessageBox.Show("Please specify No & Street Name.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return PMEReturnCode.PMFalse
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return PMEReturnCode.PMFalse
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Validate Bank Details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateBank() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "ValidateBank"
        Try
            If txtAccountType.Text.Trim() = "" Then
                nResult = PMEReturnCode.PMFalse
                MsgBox("Please specify Account Type.", vbInformation, "Mandatory Field")
                txtAccountType.Focus()
            ElseIf txtAccNumber.Text = "" Then
                nResult = PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Account Number.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtAccNumber.Focus()
            ElseIf txtBankBranchCode.Text = "" Then
                nResult = PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Branch Code.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtBankBranchCode.Focus()
            ElseIf cboBankName.ItemCaption = "(None)" Then
                nResult = PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Name.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cboBankName.Focus()
            End If

        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' ValidateForm
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateForm() As Integer
        Dim nResult As Integer = 0
        Const kMethodName As String = "ValidateForm"
        Try

            nResult = PMEReturnCode.PMTrue

            Dim lEntryExist, lIsAccountTypeNullExist, lIsDuplicateAccountExist As Integer

            If cboPaymentType.ItemCaption = "(Select Payment Type)" Then
                MessageBox.Show("Please select payment type.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ValidateForm = PMEReturnCode.PMFalse
                cboPaymentType.Focus()
                Exit Function
            End If
            If Not IsArrayEmpty(SelectedPaymentTypes) Then

                If m_iTask = PMEComponentAction.PMAdd Then
                    For lPaymentId As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0) Step 2


                        If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) And CStr(SelectedPaymentTypes(lPaymentId + 1)).Trim() = "" Then
                            nResult = PMEReturnCode.PMFalse
                            MessageBox.Show("An instance of this Payment Type exists without a unique Account Type." & Strings.Chr(13) & Strings.Chr(10) &
                                            "It will not be possible to add another instance until this field has been populated." & Strings.Chr(13) & Strings.Chr(10) &
                                            "Please return to the payment Details main screen and" & Strings.Chr(13) & Strings.Chr(10) &
                                            "add an Account Type for the existing instance of this Payment Type", "Account Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtAccountType.Focus()
                            Return nResult
                        End If
                    Next

                    If txtAccountType.Text.Trim() = "" Then
                        For lPaymentId As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0) Step 2

                            If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) Then
                                lblAccountType.Font = VB6.FontChangeBold(lblAccountType.Font, True)
                                MessageBox.Show("Please specify Account Type", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                nResult = PMEReturnCode.PMFalse
                                txtAccountType.Focus()
                                Return nResult
                            End If
                        Next
                    End If
                    'Check to find that details against this payment is not already entered
                    For lPaymentId As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0) Step 2


                        If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) And txtAccountType.Text.Trim().ToUpper() = CStr(SelectedPaymentTypes(lPaymentId + 1)).Trim().ToUpper() Then
                            nResult = PMEReturnCode.PMFalse
                            MessageBox.Show("An Account Type with this narrative already exists." & Strings.Chr(13) & Strings.Chr(10) &
                                            "Please ensure that the narrative is unique within the Payment Type", "Account Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtAccountType.Focus()
                            Return nResult
                        End If
                    Next

                ElseIf m_iTask = PMEComponentAction.PMEdit Then
                    If m_sAccountType.Trim().ToUpper() <> "" Then
                        For lPaymentId As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0) Step 2


                            If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) And CStr(SelectedPaymentTypes(lPaymentId + 1)).Trim() = "" Then
                                nResult = PMEReturnCode.PMFalse
                                MessageBox.Show("An instance of this Payment Type exists without a unique Account Type." & Strings.Chr(13) & Strings.Chr(10) &
                                                "It will not be possible to add another instance until this field has been populated." & Strings.Chr(13) & Strings.Chr(10) &
                                                "Please return to the payment Details main screen and" & Strings.Chr(13) & Strings.Chr(10) &
                                                "add an Account Type for the existing instance of this Payment Type", "Account Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                txtAccountType.Focus()
                                Return nResult
                            End If
                        Next
                        If txtAccountType.Text.Trim() = "" Then
                            For lPaymentId As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0) Step 2

                                If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) Then
                                    lblAccountType.Font = VB6.FontChangeBold(lblAccountType.Font, True)
                                    MessageBox.Show("Please specify Account Type", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    nResult = PMEReturnCode.PMFalse
                                    txtAccountType.Focus()
                                    Return nResult
                                End If
                            Next
                        End If
                    End If
                    'Check to find that details against this payment is not already entered
                    For lPaymentId As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0) Step 2

                        If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) And txtAccountType.Text.Trim().ToUpper() <> m_sAccountType.Trim().ToUpper() Then


                            If cboPaymentType.ItemId = CDbl(SelectedPaymentTypes(lPaymentId)) And txtAccountType.Text.Trim().ToUpper() = CStr(SelectedPaymentTypes(lPaymentId + 1)).Trim().ToUpper() Then
                                nResult = PMEReturnCode.PMFalse
                                MessageBox.Show("An Account Type with this narrative already exists." & Strings.Chr(13) & Strings.Chr(10) &
                                                "Please ensure that the narrative is unique within the Payment Type", "Account Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                txtAccountType.Focus()
                                Return nResult
                            End If
                        End If
                    Next
                End If 'If m_iTask = PMadd Then
            Else

                'If the Array is Empry
                If m_iTask = PMEComponentAction.PMAdd And m_sBankPaymentTypeCode <> "" Then

                    m_lReturn = m_oBusiness.isExistPaymentType(m_vPartyCnt, cboPaymentType.ItemId, txtAccountType.Text.Trim(), lEntryExist, lIsAccountTypeNullExist, lIsDuplicateAccountExist)
                    If m_lReturn = PMEReturnCode.PMTrue Then
                        If lIsAccountTypeNullExist > 0 Then
                            nResult = PMEReturnCode.PMFalse
                            MessageBox.Show("An instance of this Payment Type exists without a unique Account Type." & Strings.Chr(13) & Strings.Chr(10) &
                                            "It will not be possible to add another instance until this field has been populated." & Strings.Chr(13) & Strings.Chr(10) &
                                            "Please return to the payment Details main screen and" & Strings.Chr(13) & Strings.Chr(10) &
                                            "add an Account Type for the existing instance of this Payment Type", "Account Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtAccountType.Focus()
                            Return nResult
                        End If
                    Else
                        nResult = PMEReturnCode.PMFalse
                        RaiseError(kMethodName, "ValiateForm Failed", PMELogLevel.PMLogError)
                    End If

                    If txtAccountType.Text.Trim() = "" Then
                        If lEntryExist > 0 Then
                            lblAccountType.Font = VB6.FontChangeBold(lblAccountType.Font, True)
                            MessageBox.Show("Please specify Account Type", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            nResult = PMEReturnCode.PMFalse
                            txtAccountType.Focus()
                            Return nResult
                        End If
                    End If

                    If lIsDuplicateAccountExist > 0 Then
                        nResult = PMEReturnCode.PMFalse
                        MessageBox.Show("An Account Type with this narrative already exists." & Strings.Chr(13) & Strings.Chr(10) &
                                        "Please ensure that the narrative is unique within the Payment Type", "Account Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtAccountType.Focus()
                        Return nResult
                    End If

                End If
            End If

            If txtAccHolderName.Text = "" Then
                nResult = PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Account Holder's Name.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtAccHolderName.Focus()
                Return nResult
            End If


            Select Case MediaType_Renamed
                Case MediaType.IsBank
                    m_lReturn = ValidateBank()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If
                Case MediaType.IsCreditCard
                    m_lReturn = ValidateCC()
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If


                    Dim nAccountId As Integer
                    Dim bIsEntryExists As Integer
                    'oObjectManager = New bObjectManager.ObjectManager()
                    If Not m_vBankItem Is Nothing Then
                        nAccountId = m_vBankItem(MainModule.ENPartyBank.AccountId)
                    End If
                    If sManualAuthCode <> uctCreditCardDetails.CCManualAuthCode AndAlso nAccountId > 0 Then
                        m_lReturn = m_oBusiness.CheckManualAuthCodeIsInUse(sManualAuthCode, nAccountId, bIsEntryExists)
                        If m_lReturn = PMEReturnCode.PMTrue Then
                            If bIsEntryExists Then
                                nResult = PMEReturnCode.PMFalse
                                MessageBox.Show("This authorization code is being used by other instalment plan .." & Strings.Chr(13) & Strings.Chr(10) &
                                                "Cannot change", "Manual Auth Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                uctCreditCardDetails.CCManualAuthCode = sManualAuthCode
                                Return PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

            End Select


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return nResult
    End Function

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        Const kMethodName As String = "cmdApply_Click"

        m_lReturn = ValidateForm()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            Exit Sub

        End If
        m_lReturn = PopulateTempBankItemArray()

        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "PopulateTempBankItemArray Failed", PMELogLevel.PMLogError)
        End If

        m_lReturn = CheckForChangeBankDetail(m_vBankItem, 0, vOldBankItem, m_bBankDetailsEdited)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "CheckForChangeBankDetail Failed", PMELogLevel.PMLogError)
        End If

        If m_ENMediaType = MediaType.IsBank And m_bBankDetailsEdited Then
            m_lReturn = ValidateAccountNumber()
        End If

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        cmdOk.Enabled = True

        m_vOldBankItem = VB6.CopyArray(m_vBankItem) 'Keep Existing record in an Array

        m_lReturn = PopulateBankItemArray()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "PopulateCaseClaimList Failed", PMELogLevel.PMLogError)
        End If
        m_bPartyBankArrayExists = True
        ResetPreviousOne = uctCreditCardDetails.ResetPreviousOne
        cmdOk.Enabled = True
    End Sub

    Private Function BuildLookupArray(ByRef cPMLookUpCombo As PMLookupControl.cboPMLookup, ByRef vLookUpArray() As Object) As Integer

        ReDim vLookUpArray(MainModule.ENPMLookups.uboundeNPMLookups)

        vLookUpArray(MainModule.ENPMLookups.Id) = cPMLookUpCombo.ItemId

        vLookUpArray(MainModule.ENPMLookups.Description) = cPMLookUpCombo.ItemCaption

    End Function

    ''' <summary>
    ''' PopulateBankItemArray
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateBankItemArray() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "PopulateBankItemArray"
        Dim vAccountDesc As Object = Nothing
        Dim vPaymentDesc As Object = Nothing
        Dim vBankName As Object = Nothing
        Dim vCountry As Object = Nothing

        Try

            ReDim Preserve m_vBankItem(ENPartyBank.uBoundPartyBank)

            m_lReturn = BuildLookupArray(cPMLookUpCombo:=cboPaymentType, vLookUpArray:=vPaymentDesc)

            m_lReturn = BuildLookupArray(cPMLookUpCombo:=cboBankName, vLookUpArray:=vBankName)

            m_vBankItem(ENPartyBank.RowStatus) = PMEComponentAction.PMAdd
            If Not m_bMultiSelect Then

                m_vBankItem(ENPartyBank.BankPaymentTypeId) = vPaymentDesc
                m_vBankItem(ENPartyBank.BankAccountTypeId) = txtAccountType.Text
            End If
            m_vBankItem(ENPartyBank.AccountHolderName) = txtAccHolderName.Text

            Select Case MediaType_Renamed
                Case MediaType.IsBank

                    m_vBankItem(ENPartyBank.AccountNumber) = txtAccNumber.Text
                    m_vBankItem(ENPartyBank.BIC) = txtBIC.Text
                    m_vBankItem(ENPartyBank.IBAN) = txtIBAN.Text
                    m_vBankItem(ENPartyBank.IsBank) = 1
                    m_vBankItem(ENPartyBank.BankNameId) = vBankName
                    m_vBankItem(ENPartyBank.BankBranch) = txtBankBranch.Text
                    m_vBankItem(ENPartyBank.BankBranchCode) = txtBankBranchCode.Text
                    m_vBankItem(ENPartyBank.BankAdd1) = uctPMBankAddress.AddressLine1
                    m_vBankItem(ENPartyBank.BankAdd2) = uctPMBankAddress.AddressLine2
                    m_vBankItem(ENPartyBank.BankTown) = uctPMBankAddress.AddressLine3
                    m_vBankItem(ENPartyBank.BankPCode) = uctPMBankAddress.PostCode
                    m_vBankItem(ENPartyBank.BankRegion) = uctPMBankAddress.AddressLine4

                    ReDim vCountry(ENPMLookups.uboundeNPMLookups)

                    vCountry(ENPMLookups.Id) = uctPMBankAddress.CountryId
                    vCountry(ENPMLookups.Description) = uctPMBankAddress.CountryName
                    m_vBankItem(ENPartyBank.BankCountry) = vCountry

                    ' Clear Credit Card Details
                    m_vBankItem(ENPartyBank.CCNum) = ""
                    m_vBankItem(ENPartyBank.CCStartDate) = ""
                    m_vBankItem(ENPartyBank.CCExpiryDate) = ""
                    m_vBankItem(ENPartyBank.CCIssueNum) = ""
                    m_vBankItem(ENPartyBank.IsRegistered) = 0
                    m_vBankItem(ENPartyBank.CCPIN) = ""
                    m_vBankItem(ENPartyBank.CCAdd1) = ""
                    m_vBankItem(ENPartyBank.CCAdd2) = ""
                    m_vBankItem(ENPartyBank.CCTown) = ""
                    m_vBankItem(ENPartyBank.CCPCode) = ""

                    ReDim vCountry(ENPMLookups.uboundeNPMLookups)
                    vCountry(ENPMLookups.Id) = 0
                    vCountry(ENPMLookups.Description) = ""
                    m_vBankItem(ENPartyBank.CCCountry) = vCountry
                    m_vBankItem(ENPartyBank.CCNameOnCard) = ""
                    m_vBankItem(ENPartyBank.CCManualAuthorisationNum) = ""
                    m_vBankItem(ENPartyBank.CCIsDefault) = "0"

                Case MediaType.IsCreditCard

                    m_vBankItem(ENPartyBank.IsBank) = "0"
                    m_vBankItem(ENPartyBank.CCNum) = uctCreditCardDetails.CCNumber
                    m_vBankItem(ENPartyBank.CCStartDate) = uctCreditCardDetails.CCStart
                    m_vBankItem(ENPartyBank.CCExpiryDate) = uctCreditCardDetails.CCExpiry
                    m_vBankItem(ENPartyBank.CCIssueNum) = uctCreditCardDetails.CCIssue
                    m_vBankItem(ENPartyBank.CCPIN) = uctCreditCardDetails.CCPIN
                    m_vBankItem(ENPartyBank.CCNameOnCard) = uctCreditCardDetails.CCName
                    m_vBankItem(ENPartyBank.CCManualAuthorisationNum) = uctCreditCardDetails.CCManualAuthCode
                    m_vBankItem(ENPartyBank.CCIsDefault) = uctCreditCardDetails.CCIsDefault
                    uctCreditCardDetails.Encrypt = True

                    If chkIsRegistered.CheckState = CheckState.Unchecked Then
                        m_vBankItem(ENPartyBank.IsRegistered) = 0
                        m_vBankItem(ENPartyBank.CCAdd1) = uctPMAddressCC.AddressLine1
                        m_vBankItem(ENPartyBank.CCAdd2) = uctPMAddressCC.AddressLine2
                        m_vBankItem(ENPartyBank.CCAdd3) = uctPMAddressCC.AddressLine4
                        m_vBankItem(ENPartyBank.CCTown) = uctPMAddressCC.AddressLine3
                        m_vBankItem(ENPartyBank.CCPCode) = uctPMAddressCC.PostCode

                        ReDim vCountry(ENPMLookups.uboundeNPMLookups)

                        vCountry(ENPMLookups.Id) = uctPMAddressCC.CountryId
                        vCountry(ENPMLookups.Description) = uctPMAddressCC.CountryName
                        m_vBankItem(ENPartyBank.CCCountry) = vCountry

                    Else
                        ReDim vCountry(ENPMLookups.uboundeNPMLookups)

                        vCountry(ENPMLookups.Id) = 0
                        vCountry(ENPMLookups.Description) = ""
                        m_vBankItem(ENPartyBank.CCCountry) = vCountry
                        m_vBankItem(ENPartyBank.IsRegistered) = 1
                    End If

                    ' Clear Bank Details
                    m_vBankItem(ENPartyBank.AccountNumber) = ""
                    vBankName(ENPMLookups.Id) = DBNull.Value
                    vBankName(ENPMLookups.Description) = DBNull.Value
                    m_vBankItem(ENPartyBank.BankNameId) = vBankName
                    m_vBankItem(ENPartyBank.BankBranch) = ""
                    m_vBankItem(ENPartyBank.BankBranchCode) = ""
                    m_vBankItem(ENPartyBank.BankAdd1) = ""
                    m_vBankItem(ENPartyBank.BankAdd2) = ""
                    m_vBankItem(ENPartyBank.BankAdd3) = ""
                    m_vBankItem(ENPartyBank.BankTown) = ""
                    m_vBankItem(ENPartyBank.BankPCode) = ""
                    m_vBankItem(ENPartyBank.BankRegion) = ""
                    m_vBankItem(ENPartyBank.BIC) = ""
                    m_vBankItem(ENPartyBank.IBAN) = ""

                    ReDim vCountry(ENPMLookups.uboundeNPMLookups)

                    vCountry(ENPMLookups.Id) = 0
                    vCountry(ENPMLookups.Description) = ""
                    m_vBankItem(ENPartyBank.BankCountry) = vCountry
            End Select

            m_vBankItem(ENPartyBank.IsDeleted) = 0

            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        End Try
        Return nResult
    End Function

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        m_lStatus = PMEReturnCode.PMOK
        Me.Close()
    End Sub

    Private Function SetupInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetupInterfaceDefaults"
        Try



            result = PMEReturnCode.PMTrue


            Select Case m_iTask
                Case PMEComponentAction.PMAdd
                    SSTabHelper.SetTabVisible(SSBankDetails, 0, True)
                    SSTabHelper.SetSelectedIndex(SSBankDetails, 0)
                    SSTabHelper.SetTabVisible(SSBankDetails, 1, False)
                    m_ENMediaType = MediaType.IsBank
                    txtAccHolderName.Text = m_sPartyName
                Case PMEComponentAction.PMEdit
                    cboPaymentType.Enabled = False
                    If m_bMultiSelect Or m_sBankPaymentTypeCode <> "" Then
                        txtAccountType.Enabled = False
                        optCreditCard.Enabled = False
                        optBankAccount.Enabled = False
                    End If
            End Select
            cmdOk.Enabled = False
            If m_sBankPaymentTypeCode <> "" Then
                cboPaymentType.WhereClause = "code IN ('ANY','" & m_sBankPaymentTypeCode & " ')"
                cboPaymentType.RefreshList()
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Private Sub frmPartyBankDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'developer guide no. 220
        Me.cboBankName.FirstItem = "None"
        Me.cboPaymentType.FirstItem = ""
        Const kMethodName As String = "Form_Load"

        Dim oOptions As bSIROptions.Business
        Dim sValue As String = ""
        ' Create an instance of the object manager and Initialise
        Dim oObjectManager As New bObjectManager.ObjectManager

        ' Call the initialise method.
        m_lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)

        Dim temp_oOptions As Object = Nothing
        m_lReturn = oObjectManager.GetInstance(temp_oOptions, "bSIROptions.Business", vInstanceManager:=PMGetViaClientManager)
        oOptions = temp_oOptions
        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "DataToInterface Failed", PMELogLevel.PMLogError)
        End If


        m_lReturn = oOptions.GetOption(iOptionNumber:=13, sValue:=sValue)
        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "DataToInterface Failed", PMELogLevel.PMLogError)
        End If

        ' Set the correct QAS database
        uctPMBankAddress.QASDatabaseID = CInt(Conversion.Val(sValue))
        If Conversion.Val(sValue) <> 0 Then
            uctPMBankAddress.PMDatabaseID = 0
        Else
            uctPMBankAddress.PMDatabaseID = 1
        End If

        'PN: 48589
        uctPMAddressCC.QASDatabaseID = CInt(Conversion.Val(sValue))
        If Conversion.Val(sValue) <> 0 Then
            uctPMAddressCC.PMDatabaseID = 0
        Else
            uctPMAddressCC.PMDatabaseID = 1
        End If

        'developer guide no. 9
        m_lReturn = uctPMBankAddress.Initialise()
        m_lReturn = uctPMAddressCC.Initialise()


        oOptions.Dispose()
        oOptions = Nothing

        If m_sBankPaymentTypeCode.ToUpper() = "CLM" Then
            m_lReturn = iPMFunc.SetWindowPlacement(Me.Handle.ToInt32(), True)
        End If
        m_lReturn = SetupInterfaceDefaults()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "SetupInterfaceDefaults Failed", PMELogLevel.PMLogError)
        End If

        m_lReturn = DataToInterface()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "DataToInterface Failed", PMELogLevel.PMLogError)
        End If
        oOptions = Nothing
        m_bBankDetailsEdited = True
        '   Terminate the object Manager
        oObjectManager.Dispose()
        ' Destroy the instance of the object manager from memory
        oObjectManager = Nothing
    End Sub

    ''' <summary>
    ''' DataToInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataToInterface() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "DataToInterface"
        Try

            Select Case m_iTask
                Case PMEComponentAction.PMAdd
                    SSTabHelper.SetTabVisible(SSBankDetails, 0, True)
                    SSTabHelper.SetSelectedIndex(SSBankDetails, 0)
                    SSTabHelper.SetTabVisible(SSBankDetails, 1, False)
                    m_ENMediaType = MediaType.IsBank
                Case PMEComponentAction.PMEdit

                    If CDbl(m_vBankItem(ENPartyBank.IsBank)) = 1 Then
                        optBankAccount.Checked = True
                        SSTabHelper.SetTabVisible(SSBankDetails, 0, True)
                        SSTabHelper.SetSelectedIndex(SSBankDetails, 0)
                        SSTabHelper.SetTabVisible(SSBankDetails, 1, False)
                        m_ENMediaType = MediaType.IsBank
                        If Information.IsArray(m_vBankItem(ENPartyBank.BankNameId)) Then
                            cboBankName.ItemId = CInt(m_vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id))
                        End If
                        m_iOriginalMediaType = MediaType.IsBank
                    Else
                        optCreditCard.Checked = True
                        SSTabHelper.SetTabVisible(SSBankDetails, 1, True)
                        SSTabHelper.SetSelectedIndex(SSBankDetails, 1)
                        SSTabHelper.SetTabVisible(SSBankDetails, 0, False)
                        m_ENMediaType = MediaType.IsCreditCard
                        m_iOriginalMediaType = MediaType.IsCreditCard
                    End If
                    If m_bMultiSelect Then
                        If m_bSamePaymentTypes Then
                            cboPaymentType.ItemId = CInt(m_vBankItem(ENPartyBank.BankPaymentTypeId)(ENPMLookups.Id))
                        Else
                            cboPaymentType.ListIndex = -1
                        End If
                        txtAccountType.Text = ""
                    Else
                        cboPaymentType.ItemId = CInt(m_vBankItem(ENPartyBank.BankPaymentTypeId))
                        txtAccountType.Text = ToSafeString(m_vBankItem(ENPartyBank.BankAccountTypeId))
                    End If

                    txtAccHolderName.Text = CStr(m_vBankItem(ENPartyBank.AccountHolderName))
                    ' m_vBankItem(ENPartyBank.AccountId) = DBNull.Value
                    txtAccNumber.Text = CStr(m_vBankItem(ENPartyBank.AccountNumber))
                    txtBIC.Text = CStr(m_vBankItem(ENPartyBank.BIC))
                    txtIBAN.Text = CStr(m_vBankItem(ENPartyBank.IBAN))
                    txtBankBranch.Text = CStr(m_vBankItem(ENPartyBank.BankBranch))
                    txtBankBranchCode.Text = CStr(m_vBankItem(ENPartyBank.BankBranchCode))
                    uctPMBankAddress.AddressLine1 = CStr(m_vBankItem(ENPartyBank.BankAdd1))
                    uctPMBankAddress.AddressLine2 = CStr(m_vBankItem(ENPartyBank.BankAdd2))
                    uctPMBankAddress.AddressLine3 = CStr(m_vBankItem(ENPartyBank.BankTown))
                    uctPMBankAddress.PostCode = CStr(m_vBankItem(ENPartyBank.BankPCode))
                    uctPMBankAddress.AddressLine4 = CStr(m_vBankItem(ENPartyBank.BankRegion))
                    If CStr(m_vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Id)) <> "" Then
                        uctPMBankAddress.CountryId = CInt(m_vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Id))
                    End If
                    uctCreditCardDetails.CCNumber1 = CStr(m_vBankItem(ENPartyBank.CCNum))
                    uctCreditCardDetails.CCStart = CStr(m_vBankItem(ENPartyBank.CCStartDate))
                    uctCreditCardDetails.CCExpiry = CStr(m_vBankItem(ENPartyBank.CCExpiryDate))
                    uctCreditCardDetails.CCIssue = CStr(m_vBankItem(ENPartyBank.CCIssueNum))
                    uctCreditCardDetails.ControlInitialisedFlag = False
                    uctCreditCardDetails.AccountID = CInt(m_vBankItem(ENPartyBank.AccountId))
                    If CDbl(m_vBankItem(ENPartyBank.IsRegistered)) = 0 And ToSafeInteger(m_vBankItem(ENPartyBank.IsBank)) = 0 Then
                        chkIsRegistered.CheckState = CheckState.Unchecked
                        uctPMAddressCC.SetMandatoryFlagAddressLine1 = True
                        uctPMAddressCC.AddressLine1 = Convert.ToString(m_vBankItem(ENPartyBank.CCAdd1))
                        uctPMAddressCC.AddressLine2 = Convert.ToString(m_vBankItem(ENPartyBank.CCAdd2))
                        uctPMAddressCC.AddressLine3 = Convert.ToString(m_vBankItem(ENPartyBank.CCTown))
                        uctPMAddressCC.AddressLine4 = Convert.ToString(m_vBankItem(ENPartyBank.CCAdd3))
                        uctPMAddressCC.PostCode = Convert.ToString(m_vBankItem(ENPartyBank.CCPCode))
                        If Information.IsArray(m_vBankItem(ENPartyBank.CCCountry)) Then
                            If CStr(m_vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Id)) <> "" Then
                                uctPMAddressCC.CountryId = CInt(m_vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Id))
                            End If
                        End If
                    Else
                        chkIsRegistered.CheckState = CheckState.Checked
                    End If
                    uctCreditCardDetails.CCName = CStr(m_vBankItem(ENPartyBank.CCNameOnCard))
                    uctCreditCardDetails.CCManualAuthCode = CStr(m_vBankItem(ENPartyBank.CCManualAuthorisationNum))
                    uctCreditCardDetails.CCIsDefault = ToSafeInteger(m_vBankItem(ENPartyBank.CCIsDefault))
            End Select



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return nResult
    End Function

    Private Sub txtAccNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtAccNumber.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii >= 48 And KeyAscii <= 57) Or KeyAscii = 8 Then
            'Fill AccountNumber
            m_bBankDetailsEdited = True
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtBankBranchCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtBankBranchCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii >= 48 And KeyAscii <= 57) Or KeyAscii = 8 Then
            'Fill BankBranchCode
            m_bBankDetailsEdited = True
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ''' <summary>
    ''' ValidateAccountNumber
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateAccountNumber() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oSirMediaTypeValidation As bSIRMediaTypeValidation.Business
        Const kMethodName As String = "ValidateAccountNumber"

        Try

            If AlphanumericValidation(Trim(txtBIC.Text)) <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Only alphanumeric characters allowed in BIC field.", "Bank Validation",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cmdOk.Enabled = False
                Return PMEReturnCode.PMFalse
            End If

            If AlphanumericValidation(Trim(txtIBAN.Text)) <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Only alphanumeric characters allowed in IBAN field.", "Bank Validation",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cmdOk.Enabled = False
                Return PMEReturnCode.PMFalse
            End If

            Dim bValid As Boolean
            Dim sStrippedString As String = ""
            Dim sBankName As String = ""
            Dim sAddress1 As String = ""
            Dim sAddress2 As String = ""
            Dim sAddress3 As String = ""
            Dim sAddress4 As String = ""
            Dim sPostalCode As String = ""

            Dim vValidationMessage As Object = Nothing
            Dim bValidationOverridable As Boolean
            Dim oObjectManager As bObjectManager.ObjectManager

            ' Create an instance of the object manager and Initialise
            oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)

            Dim temp_oSirMediaTypeValidation As Object = Nothing

            m_lReturn = oObjectManager.GetInstance(temp_oSirMediaTypeValidation, "bSIRMediaTypeValidation.Business", PMGetViaClientManager)
            oSirMediaTypeValidation = temp_oSirMediaTypeValidation

            'TR PN5080- Append the Account Number to the Sort Code field. Do
            'not check the Sort code field as some customers will put the sort code into
            'the account field (IAG), but for other (i.e. GB) customers the sort
            'code goes into it's own field. So sort code can be blank.
            'Strip the Spaces from the SortCode & AccountNumber before Validation
            sStrippedString = txtBankBranchCode.Text.Replace(" ", "") & "|" & _
                              txtAccNumber.Text.Replace(" ", "") & "|" & _
                              txtAccountType.Text.Replace(" ", "")
            sBankName = cboBankName.ItemCode.Replace(" ", "")


            'TR - Perform the validation


            oSirMediaTypeValidation.ValidateNumber(0, uctPMBankAddress.CountryId, sStrippedString, bValid,
                                                   sBankName, sAddress1, sAddress2, sAddress3, sAddress4, sPostalCode,
                                                   vValidationMessage, bValidationOverridable, "Bank",
                                                   sBIC:=Trim(txtBIC.Text), sIBAN:=Trim(txtIBAN.Text))


            Dim sMessage As String = String.Empty
            Dim IsValid As String
            If Not bValid Then
                If Information.IsArray(vValidationMessage) Then

                    For iErrCount As Integer = 0 To vValidationMessage.GetUpperBound(0)

                        sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & CStr(vValidationMessage(iErrCount))
                    Next
                Else
                    'if there is no message then store the generic message
                    sMessage = "Bank details have failed validation"
                End If

                'if validation are overridable then show the message with vbYesNo
                If bValidationOverridable Then
                    sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to override the bank validation?"
                    IsValid = CStr(MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                    If IsValid = System.Windows.Forms.DialogResult.Yes Then
                        'PN: 73953
                        sBankName = cboBankName.ItemCaption
                        uctPMBankAddress.AddressLine1 = sAddress1
                        uctPMBankAddress.AddressLine2 = sAddress2
                        uctPMBankAddress.AddressLine3 = sAddress3
                        uctPMBankAddress.AddressLine4 = sAddress4
                        uctPMBankAddress.PostCode = sPostalCode
                        uctPMBankAddress.CountryId = oObjectManager.CountryID
                        nResult = PMEReturnCode.PMTrue

                        m_bBankDetailsEdited = False
                    Else
                        nResult = PMEReturnCode.PMFalse
                        cmdOk.Enabled = False 'PN: 73953
                    End If
                ElseIf Not bValidationOverridable Then
                    MessageBox.Show(sMessage, "Bank Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    m_lReturn = SelectcboItem(cboBankName, sBankName)
                    uctPMBankAddress.AddressLine1 = sAddress1
                    uctPMBankAddress.AddressLine2 = sAddress2
                    uctPMBankAddress.AddressLine3 = sAddress3
                    uctPMBankAddress.AddressLine4 = sAddress4
                    uctPMBankAddress.PostCode = sPostalCode
                    uctPMBankAddress.CountryId = oObjectManager.CountryID
                    nResult = PMEReturnCode.PMFalse
                    cmdOk.Enabled = False
                End If
            ElseIf bValid Then
                If sPostalCode.Trim() <> "" Then
                    m_lReturn = SelectcboItem(cboBankName, sBankName)
                    uctPMBankAddress.AddressLine1 = sAddress1
                    uctPMBankAddress.AddressLine2 = sAddress2
                    uctPMBankAddress.AddressLine3 = sAddress3
                    uctPMBankAddress.AddressLine4 = sAddress4
                    uctPMBankAddress.PostCode = sPostalCode
                    uctPMBankAddress.CountryId = oObjectManager.CountryID
                End If

                nResult = PMEReturnCode.PMTrue
                m_bBankDetailsEdited = False
            End If

            oSirMediaTypeValidation = Nothing
            '   Terminate the object Manager
            oObjectManager.Dispose()
            ' Destroy the instance of the object manager from memory
            oObjectManager = Nothing



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return nResult
    End Function

    Private Function SelectcboItem(ByRef r_oCbo As PMLookupControl.cboPMLookup, ByVal v_sSelectedText As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try



            result = PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_sSelectedText <> "" Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.ListCount - 1

                    ' search the item data array for a match
                    If r_oCbo.ItemCaption(r_oCbo.ItemData(lItem)) = v_sSelectedText Then

                        ' found a match - select the item
                        r_oCbo.ItemId = r_oCbo.ItemData(lItem)
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

                If bItemNotFound Then

                    ' log that we havent found the specified item
                    LogMessageToFile(sUsername:="", iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to find item with id:" & v_sSelectedText & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)

                End If

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' PopulateTempBankItemArray
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateTempBankItemArray() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "PopulateBankItemArray"
        Dim oPaymentDesc As Object = Nothing
        Dim oBankName As Object = Nothing
        Dim oCountry As Object = Nothing

        Try

            ReDim Preserve vOldBankItem(ENPartyBank.uBoundPartyBank)

            m_lReturn = BuildLookupArray(cPMLookUpCombo:=cboPaymentType, vLookUpArray:=oPaymentDesc)
            m_lReturn = BuildLookupArray(cPMLookUpCombo:=cboBankName, vLookUpArray:=oBankName)
            vOldBankItem(ENPartyBank.RowStatus) = PMEComponentAction.PMAdd

            If Not m_bMultiSelect Then

                vOldBankItem(ENPartyBank.BankPaymentTypeId) = oPaymentDesc
                vOldBankItem(ENPartyBank.BankAccountTypeId) = txtAccountType.Text
            End If

            vOldBankItem(ENPartyBank.AccountHolderName) = txtAccHolderName.Text
            Select Case MediaType_Renamed
                Case MediaType.IsBank

                    vOldBankItem(ENPartyBank.AccountNumber) = txtAccNumber.Text
                    vOldBankItem(ENPartyBank.BIC) = txtBIC.Text
                    vOldBankItem(ENPartyBank.IBAN) = txtIBAN.Text
                    vOldBankItem(ENPartyBank.IsBank) = 1
                    vOldBankItem(ENPartyBank.BankNameId) = oBankName
                    vOldBankItem(ENPartyBank.BankBranch) = txtBankBranch.Text
                    vOldBankItem(ENPartyBank.BankBranchCode) = txtBankBranchCode.Text
                    vOldBankItem(ENPartyBank.BankAdd1) = uctPMBankAddress.AddressLine1
                    vOldBankItem(ENPartyBank.BankAdd2) = uctPMBankAddress.AddressLine2
                    vOldBankItem(ENPartyBank.BankTown) = uctPMBankAddress.AddressLine3
                    vOldBankItem(ENPartyBank.BankPCode) = uctPMBankAddress.PostCode
                    vOldBankItem(ENPartyBank.BankRegion) = uctPMBankAddress.AddressLine4

                    ReDim oCountry(ENPMLookups.uboundeNPMLookups)

                    oCountry(ENPMLookups.Id) = uctPMBankAddress.CountryId
                    oCountry(ENPMLookups.Description) = uctPMBankAddress.CountryName
                    vOldBankItem(ENPartyBank.BankCountry) = uctPMBankAddress.CountryId

                    ' Clear Credit Card Details
                    vOldBankItem(ENPartyBank.CCNum) = ""
                    vOldBankItem(ENPartyBank.CCStartDate) = ""
                    vOldBankItem(ENPartyBank.CCExpiryDate) = ""
                    vOldBankItem(ENPartyBank.CCIssueNum) = ""
                    vOldBankItem(ENPartyBank.IsRegistered) = 0
                    vOldBankItem(ENPartyBank.CCPIN) = ""
                    vOldBankItem(ENPartyBank.CCAdd1) = ""
                    vOldBankItem(ENPartyBank.CCAdd2) = ""
                    vOldBankItem(ENPartyBank.CCTown) = ""
                    vOldBankItem(ENPartyBank.CCPCode) = ""

                    ReDim oCountry(ENPMLookups.uboundeNPMLookups)

                    oCountry(ENPMLookups.Id) = 0
                    oCountry(ENPMLookups.Description) = ""
                    vOldBankItem(ENPartyBank.CCCountry) = oCountry
                    vOldBankItem(ENPartyBank.CCNameOnCard) = ""
                    vOldBankItem(ENPartyBank.CCManualAuthorisationNum) = ""
                Case MediaType.IsCreditCard

                    vOldBankItem(ENPartyBank.IsBank) = "0"
                    vOldBankItem(ENPartyBank.CCNum) = uctCreditCardDetails.CCNumber
                    vOldBankItem(ENPartyBank.CCStartDate) = uctCreditCardDetails.CCStart
                    vOldBankItem(ENPartyBank.CCExpiryDate) = uctCreditCardDetails.CCExpiry
                    vOldBankItem(ENPartyBank.CCIssueNum) = uctCreditCardDetails.CCIssue
                    vOldBankItem(ENPartyBank.CCPIN) = uctCreditCardDetails.CCPIN
                    vOldBankItem(ENPartyBank.CCNameOnCard) = uctCreditCardDetails.CCName
                    vOldBankItem(ENPartyBank.CCManualAuthorisationNum) = uctCreditCardDetails.CCManualAuthCode
                    vOldBankItem(ENPartyBank.CCIsDefault) = uctCreditCardDetails.CCIsDefault
                    uctCreditCardDetails.Encrypt = True

                    If chkIsRegistered.CheckState = CheckState.Unchecked Then

                        vOldBankItem(ENPartyBank.IsRegistered) = 0
                        vOldBankItem(ENPartyBank.CCAdd1) = uctPMAddressCC.AddressLine1
                        vOldBankItem(ENPartyBank.CCAdd2) = uctPMAddressCC.AddressLine2
                        vOldBankItem(ENPartyBank.CCAdd3) = uctPMAddressCC.AddressLine4
                        vOldBankItem(ENPartyBank.CCTown) = uctPMAddressCC.AddressLine3
                        vOldBankItem(ENPartyBank.CCPCode) = uctPMAddressCC.PostCode

                        ReDim oCountry(ENPMLookups.uboundeNPMLookups)

                        oCountry(ENPMLookups.Id) = uctPMAddressCC.CountryId
                        oCountry(ENPMLookups.Description) = uctPMAddressCC.CountryName
                        vOldBankItem(ENPartyBank.CCCountry) = oCountry
                    Else
                        ReDim oCountry(ENPMLookups.uboundeNPMLookups)

                        oCountry(ENPMLookups.Id) = 0
                        oCountry(ENPMLookups.Description) = ""
                        vOldBankItem(ENPartyBank.CCCountry) = oCountry
                        vOldBankItem(ENPartyBank.IsRegistered) = 1
                    End If

                    ' Clear Bank Details

                    vOldBankItem(ENPartyBank.AccountNumber) = ""
                    vOldBankItem(MainModule.ENPartyBank.BIC) = ""
                    vOldBankItem(MainModule.ENPartyBank.IBAN) = ""
                    oBankName(ENPMLookups.Id) = DBNull.Value
                    oBankName(ENPMLookups.Description) = DBNull.Value
                    vOldBankItem(ENPartyBank.BankNameId) = oBankName
                    vOldBankItem(ENPartyBank.BankBranch) = ""
                    vOldBankItem(ENPartyBank.BankBranchCode) = ""
                    vOldBankItem(ENPartyBank.BankAdd1) = ""
                    vOldBankItem(ENPartyBank.BankAdd2) = ""
                    vOldBankItem(ENPartyBank.BankAdd3) = ""
                    vOldBankItem(ENPartyBank.BankTown) = ""
                    vOldBankItem(ENPartyBank.BankPCode) = ""
                    vOldBankItem(ENPartyBank.BankRegion) = ""

                    ReDim oCountry(ENPMLookups.uboundeNPMLookups)

                    oCountry(ENPMLookups.Id) = 0
                    oCountry(ENPMLookups.Description) = ""
                    vOldBankItem(ENPartyBank.BankCountry) = oCountry
            End Select

            vOldBankItem(ENPartyBank.IsDeleted) = 0

            'GoTo Finally_Renamed
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' CheckForChangeBankDetail
    ''' </summary>
    ''' <param name="vBankItem"></param>
    ''' <param name="lIndex"></param>
    ''' <param name="vOldBankItem"></param>
    ''' <param name="r_bIsEdit"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckForChangeBankDetail(ByVal vBankItem As Object, ByVal lIndex As Integer, ByVal vOldBankItem() As Object, ByRef r_bIsEdit As Boolean) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "CheckForChangeBankDetail"
        Try

            If m_iTask = PMEComponentAction.PMAdd And m_bPartyBankArrayExists Or m_iTask = PMEComponentAction.PMEdit Then
                r_bIsEdit = PMEReturnCode.PMFalse 'PN: 73953
                If ToSafeString(m_vBankItem(ENPartyBank.IsBank)) <> CStr(vOldBankItem(ENPartyBank.IsBank)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.AccountHolderName)) <> CStr(vOldBankItem(ENPartyBank.AccountHolderName)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BIC)) <> CStr(vOldBankItem(ENPartyBank.BIC)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.IBAN)) <> CStr(vOldBankItem(ENPartyBank.IBAN)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.AccountNumber)) <> CStr(vOldBankItem(ENPartyBank.AccountNumber)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankBranch)) <> CStr(vOldBankItem(ENPartyBank.BankBranch)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankBranchCode)) <> CStr(vOldBankItem(ENPartyBank.BankBranchCode)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankAdd1)) <> CStr(vOldBankItem(ENPartyBank.BankAdd1)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankAdd2)) <> CStr(vOldBankItem(ENPartyBank.BankAdd2)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankAdd3)) <> CStr(vOldBankItem(ENPartyBank.BankAdd3)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankTown)) <> CStr(vOldBankItem(ENPartyBank.BankTown)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankPCode)) <> CStr(vOldBankItem(ENPartyBank.BankPCode)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BankRegion)) <> CStr(vOldBankItem(ENPartyBank.BankRegion)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCNum)) <> CStr(vOldBankItem(ENPartyBank.CCNum)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCStartDate)) <> CStr(vOldBankItem(ENPartyBank.CCStartDate)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCExpiryDate)) <> CStr(vOldBankItem(ENPartyBank.CCExpiryDate)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCIssueNum)) <> CStr(vOldBankItem(ENPartyBank.CCIssueNum)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCPIN)) <> CStr(vOldBankItem(ENPartyBank.CCPIN)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.IsRegistered)) <> CStr(vOldBankItem(ENPartyBank.IsRegistered)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCAdd1)) <> CStr(vOldBankItem(ENPartyBank.CCAdd1)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCAdd2)) <> CStr(vOldBankItem(ENPartyBank.CCAdd2)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCAdd3)) <> CStr(vOldBankItem(ENPartyBank.CCAdd3)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCTown)) <> CStr(vOldBankItem(ENPartyBank.CCTown)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCPCode)) <> CStr(vOldBankItem(ENPartyBank.CCPCode)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCNameOnCard)) <> CStr(vOldBankItem(ENPartyBank.CCNameOnCard)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCManualAuthorisationNum)) <> CStr(vOldBankItem(ENPartyBank.CCManualAuthorisationNum)) Then
                    r_bIsEdit = True
                    Return nResult
                End If
                If optBankAccount.Checked Then
                    If Not IsArray(m_vBankItem(ENPartyBank.BankNameId)) Then
                        r_bIsEdit = True
                        Return nResult
                    Else
                        If ToSafeString(m_vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id)) <> CStr(vOldBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id)) Then
                            r_bIsEdit = True
                            Return nResult
                        End If
                    End If
                    If ToSafeString(m_vBankItem(ENPartyBank.BankCountry)(0)) <> ToSafeString(vOldBankItem(ENPartyBank.BankCountry)) Then
                        r_bIsEdit = True
                        Return nResult
                    End If
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.BIC)) <> CStr(vOldBankItem(ENPartyBank.BIC)) Then
                    r_bIsEdit = True
                    Return nResult
                End If
                If ToSafeString(m_vBankItem(ENPartyBank.IBAN)) <> CStr(vOldBankItem(ENPartyBank.IBAN)) Then
                    r_bIsEdit = True
                    Return nResult
                End If

                If ToSafeString(m_vBankItem(ENPartyBank.CCIsDefault)) <> CStr(vOldBankItem(ENPartyBank.CCIsDefault)) Then
                    r_bIsEdit = True
                    Return nResult
                End If
            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Called on KeyPress event of textox to validate only alphanumeric input
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AlphanumericValidation(sender As Object, e As KeyPressEventArgs) Handles txtBIC.KeyPress, txtIBAN.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(e.KeyChar)
        If (KeyAscii >= 48 AndAlso KeyAscii <= 57) OrElse _
            (KeyAscii >= 65 AndAlso KeyAscii <= 90) OrElse _
            (KeyAscii >= 97 And KeyAscii <= 122) _
            OrElse KeyAscii = 8 OrElse KeyAscii = 127 Then
            m_bBankDetailsEdited = True
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            e.Handled = True
        End If
        e.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    ''' <summary>
    ''' Called to validate alphanumeric validation via Copy/Paste 
    ''' </summary>
    ''' <param name="sInput"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AlphanumericValidation(ByVal sInput As String) As Integer
        If System.Text.RegularExpressions.Regex.IsMatch(sInput, "^[a-zA-Z0-9]*$") Then
            Return PMEReturnCode.PMTrue
        Else
            Return PMEReturnCode.PMFalse
        End If
    End Function
End Class