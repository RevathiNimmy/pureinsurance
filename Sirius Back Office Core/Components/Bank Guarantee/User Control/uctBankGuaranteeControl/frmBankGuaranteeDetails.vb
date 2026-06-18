Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmBankGuaranteeDetails
    Inherits System.Windows.Forms.Form
    'For Add - set last value of a array to 1
    'For Edit - set last value of a array to 2
    'For Delete - set last value of a array to 3

    Private Const ACClass As String = "uctBankGuaranteeControl"
    'developer guide no. 33
    Private m_vGuaranteeItem As Object
    Private m_vAttachedPolicies As Array
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lSystemCurrencyId As Integer

    'developer guide no. 7
    Private Const vbFormCode As Integer = 0

    Private m_vBankNameId As Integer
    Private m_sShortCode As String = ""
    Private m_sAccountName As String = ""



    Public WriteOnly Property SystemCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_lSystemCurrencyId = Value
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

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property GuaranteeItem() As Object
        Get
            Return VB6.CopyArray(m_vGuaranteeItem)
        End Get
        Set(ByVal Value As Object)
            m_vGuaranteeItem = Value
        End Set
    End Property

    Private Function ConvertToSystem(ByVal lCurrencyId As Integer, ByVal crCurrencyAmountFrom As Decimal, ByRef crCurrencyAmountTo As Decimal) As Integer
        Dim result As Integer = 0
        Dim bACTCurrencyConvert As Object
        Const kMethodName As String = "ConvertToSystem"

        Dim lReturn As Integer
        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Try



            result = gPMConstants.PMEReturnCode.PMTrue




            'Get Currency Convert Object.
            Dim temp_oCurrencyConvert As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oCurrencyConvert = temp_oCurrencyConvert

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to create instance of " & "bACTCurrencyConvert.Form")
            End If



            m_lReturn = oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=lCurrencyId, v_crCurrencyAmountFrom:=crCurrencyAmountFrom, v_lCompanyID:=1, v_lCurrencyIdTo:=m_lSystemCurrencyId, r_crCurrencyAmountTo:=crCurrencyAmountTo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'txtConvRate.Text = ToSafeDouble(crCurrencyAmountTo / crCurrencyAmountFrom)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub cboCurrency_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.Click
        Dim crCurrencyAmount As Decimal

        If cboCurrency.ItemId = m_lSystemCurrencyId Then
            'txtConvRate.Visible = False
            txtAmountSysCurr.Visible = False
            'lblConvRate.Visible = False
            lblAmtSysCurr.Visible = False

            lblLmitsAvailable.Visible = False
            txtLimitsAvailable.Visible = False
            'txtCurrLimitAvailable.Visible = False
        ElseIf cboCurrency.ItemId <> m_lSystemCurrencyId Then
            '        txtConvRate.Visible = True
            '        txtAmountSysCurr.Visible = True
            '        lblConvRate.Visible = True
            '        lblAmtSysCurr.Visible = True
            '        lblLmitsAvailable.Visible = True
            '        txtLimitsAvailable.Visible = True
            '        txtCurrLimitAvailable.Visible = True

            m_lReturn = CType(ConvertToSystem(lCurrencyId:=cboCurrency.ItemId, crCurrencyAmountFrom:=gPMFunctions.ToSafeDouble(txtAmount.Text), crCurrencyAmountTo:=crCurrencyAmount), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If

            txtAmountSysCurr.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(crCurrencyAmount))
        End If
    End Sub

    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click

        m_lReturn = CreateWorkManagerTask()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Unable to create work task manager", "Task Window", MessageBoxButtons.OK, MessageBoxIcon.Error)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Close()
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



            result = gPMConstants.PMEReturnCode.PMTrue
            '
            '    If (IsMissing(vTask) = False) Then
            '        m_iTask = CInt(vTask)
            '    End If
            '
            '    If (IsMissing(vNavigate) = False) Then
            '        m_lNavigate = CLng(vNavigate)
            '    End If
            '
            '    If (IsMissing(vProcessMode) = False) Then
            '        m_lProcessMode = CLng(vProcessMode)
            '    End If
            '
            '    If (IsMissing(vTransactionType) = False) Then
            '        m_sTransactionType = CStr(vTransactionType)
            '    End If
            '
            '    If (IsMissing(vEffectiveDate) = False) Then
            '        m_dtEffectiveDate = CDate(vEffectiveDate)
            '    End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateForm"
        Dim A As Integer
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            If txtBankNameId.Text = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Name.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf txtBankBranch.Text = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Branch.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf txtBGNo.Text = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Guarantee Ref No..", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf cboCurrency.ItemCaption = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Guarantee Currency.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf txtAmount.Text = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Guarantee Amount.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
                'ElseIf txtEffectiveDate.Text = "" Then
                '    ValidateForm = PMFalse
                '    MsgBox "Please specify Bank Guarantee Effective Date.", vbInformation, "Mandatory Field"
                '    Exit Function
                'ElseIf txtConvRate.Text = "" Then
                'ElseIf txtAmountSysCurr.Text = "" Then
            ElseIf txtIssueDate.Text = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Guarantee Issue Date.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf txtExpiryDate.Text = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please specify Bank Guarantee Expiry Date.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf Not Information.IsArray(PickListProducts.GetSelectedItems) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please attach atleast one Product .", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf Not Information.IsArray(PickListBranches.GetSelectedItems) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please attach atleast one Branch.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            ElseIf gPMFunctions.ToSafeDate(txtIssueDate.Text) > gPMFunctions.ToSafeDate(txtExpiryDate.Text) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Issue Date can't be greater than Expiry Date.", "Mandatory Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        Const kMethodName As String = "cmdApply_Click"

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

            m_lReturn = ValidateForm()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            cmdOk.Enabled = True
            m_lReturn = PopulateBankItemArray()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateCaseClaimList Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If
    End Sub

    Private Function BuildLookupArray(ByRef cPMLookUpCombo As PMLookupControl.cboPMLookup, ByRef vLookUpArray() As Object) As Integer

        ReDim vLookUpArray(MainModule.ENPMLookups.uboundeNPMLookups)

        vLookUpArray(MainModule.ENPMLookups.Id) = cPMLookUpCombo.ItemId

        vLookUpArray(MainModule.ENPMLookups.Description) = cPMLookUpCombo.ItemCaption

    End Function

    ' Need to see the mapping
    Private Function PopulateBankItemArray() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBankItemArray"

        Dim vCurrency() As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim Preserve m_vGuaranteeItem(MainModule.ENBankGuarantee.uBoundBankGuarantee)

            Dim vBankName(1) As Object

            vBankName(0) = m_vBankNameId

            vBankName(1) = m_sAccountName
            '    m_lReturn = BuildLookupArray(cPMLookUpCombo:="22", _
            ''                                    vLookUpArray:=vBankName)


            m_lReturn = CType(BuildLookupArray(cPMLookUpCombo:=cboCurrency, vLookUpArray:=vCurrency), gPMConstants.PMEReturnCode)



            m_vGuaranteeItem(MainModule.ENBankGuarantee.RowStatus) = gPMConstants.PMEComponentAction.PMAdd


            m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId) = vBankName

            m_vGuaranteeItem(MainModule.ENBankGuarantee.IsDeleted) = 0

            m_vGuaranteeItem(MainModule.ENBankGuarantee.AvailableBal) = txtLimitsAvailable.Text

            m_vGuaranteeItem(MainModule.ENBankGuarantee.BankBranch) = txtBankBranch.Text


            m_vGuaranteeItem(MainModule.ENBankGuarantee.BGCurrencyId) = vCurrency

            m_vGuaranteeItem(MainModule.ENBankGuarantee.BGLimit) = txtAmount.Text

            m_vGuaranteeItem(MainModule.ENBankGuarantee.BGRef) = txtBGNo.Text

            m_vGuaranteeItem(MainModule.ENBankGuarantee.IssueDate) = txtIssueDate.Text
            'Start - Sankar - Bank Guarantee Bug Fixing


            m_vGuaranteeItem(MainModule.ENBankGuarantee.Branches) = PickListBranches.GetItemDetails


            m_vGuaranteeItem(MainModule.ENBankGuarantee.Products) = PickListProducts.GetItemDetails '.GetSelectedItems

            m_vGuaranteeItem(MainModule.ENBankGuarantee.BGStatusId) = 1
            'End - Sankar - Bank Guarantee Bug Fixing

            m_vGuaranteeItem(MainModule.ENBankGuarantee.CustodyBranchId) = cboCustodyBranch.ItemId

            m_vGuaranteeItem(MainModule.ENBankGuarantee.IssueDate) = txtIssueDate.Text

            m_vGuaranteeItem(MainModule.ENBankGuarantee.ExpiryDate) = txtExpiryDate.Text

            m_vGuaranteeItem(MainModule.ENBankGuarantee.IsPolicyLock) = chkIsSinglePolicyLock.CheckState

            m_vGuaranteeItem(MainModule.ENBankGuarantee.RowStatus) = gPMConstants.PMEComponentAction.PMAdd



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub cmdFindBank_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindBank.Click
        m_lReturn = GetBankInfo()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CmdClient_Click", "GetBGHolderInfo Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub

    Public Function GetBankInfo() As Integer

        Dim result As Integer = 0

        'developer guide no.108
        Dim oFindBank As iACTFindBank.Interface_Renamed
        Const kMethodName As String = "GetAgentInfo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oFindBank As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindBank, sClassName:="iACTFindBank.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindBank = temp_oFindBank

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oFindBank.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_vBankNameId = oFindBank.BankID

            m_sShortCode = oFindBank.ShortCode

            m_sAccountName = oFindBank.AccountName

            txtBankNameId.Text = m_sAccountName
            ' Destroy Find Party object

            oFindBank.Dispose()
            oFindBank = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        'Me.Close()
    End Sub

    Private Function SetupInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetupInterfaceDefaults"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd
                    SSTabHelper.SetTabVisible(SSBGDetails, 3, False)
                    'SSBGDetails.Tab = 0
                Case gPMConstants.PMEComponentAction.PMEdit

                Case gPMConstants.PMEComponentAction.PMView
                    cmdFindBank.Enabled = False
                    txtBankNameId.ReadOnly = True
                    txtAmount.ReadOnly = True
                    txtBankBranch.ReadOnly = True
                    txtBGNo.ReadOnly = True
                    txtExpiryDate.ReadOnly = True
                    txtIssueDate.ReadOnly = True
                    txtLimitsAvailable.ReadOnly = True
                    chkIsSinglePolicyLock.Enabled = False
                    cboCurrency.Enabled = False
                    cboCustodyBranch.Enabled = False
                    PickListProducts.Enabled = False
                    PickListBranches.Enabled = False
                    cmdApply.Enabled = False

            End Select
            cmdOk.Enabled = False




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Private Sub frmBankGuaranteeDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"

        'developer guide no. 220
        Me.cboCustodyBranch.FirstItem = ""
        Me.cboCurrency.FirstItem = ""
        m_lReturn = SetupInterfaceDefaults()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetupInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        If Not (m_iTask = PMEComponentAction.PMAdd) Then
            SSTabHelper.SetSelectedTabIndex(SSBGDetails, 3)
        End If
        SSBGDetails.Focus()
        cmdFindBank.Focus()
        cmdFindBank.Select()
    End Sub

    Private Function GetAttachedPolicies() As Integer
        Dim result As Integer = 0
        Dim bSIRBankGuarantee As Object
        Const kMethodName As String = "GetAttachedPolicies"
        Dim oBusiness As bSIRBankGuarantee.Business
        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            ' Get an instance of the business object via the public object manager.
            Dim temp_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRBankGuarantee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRBankGuarantee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oBusiness.GetAttachedPolicies(vGetAttachedPolicies:=m_vAttachedPolicies, vBg_Id:=m_vGuaranteeItem(MainModule.ENBankGuarantee.BGId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "GetAttachedPolicies of bSIRBankGuarantee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetBusiness(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Private Function GetBusiness() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                m_lReturn = CType(GetAttachedPolicies(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetAttachedPolicies Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Private Function DataToInterface() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd
                    '        SSBankDetails.TabVisible(0) = True
                    '        SSBankDetails.Tab = 0
                    '        SSBankDetails.TabVisible(1) = False
                    '        m_ENMediaType = IsBank
                Case gPMConstants.PMEComponentAction.PMEdit

                    txtLimitsAvailable.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.AvailableBal))

                    txtBankBranch.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankBranch))

                    cboCurrency.ItemId = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.BGCurrencyId)(MainModule.ENPMLookups.Id))

                    txtAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BGLimit)))

                    txtBGNo.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BGRef))


                    txtIssueDate.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.IssueDate))

                    txtBankNameId.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Description))

                    txtExpiryDate.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.ExpiryDate))


                    chkIsSinglePolicyLock.CheckState = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.IsPolicyLock))


                    m_vBankNameId = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Id))

                    m_sAccountName = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Description))

                    cboCustodyBranch.ItemId = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.CustodyBranchId))

                    SetPickList()

                    m_lReturn = CType(PopulatePolicyDetailsList(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulatePolicyDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Case gPMConstants.PMEComponentAction.PMView

                    txtLimitsAvailable.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.AvailableBal))

                    txtBankBranch.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankBranch))

                    cboCurrency.ItemId = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.BGCurrencyId)(MainModule.ENPMLookups.Id))

                    txtAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BGLimit)))

                    txtBGNo.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BGRef))


                    txtIssueDate.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.IssueDate))

                    txtBankNameId.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Description))

                    txtExpiryDate.Text = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.ExpiryDate))


                    chkIsSinglePolicyLock.CheckState = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.IsPolicyLock))


                    m_vBankNameId = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Id))

                    m_sAccountName = CStr(m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Description))

                    cboCustodyBranch.ItemId = CInt(m_vGuaranteeItem(MainModule.ENBankGuarantee.CustodyBranchId))

                    SetPickList()

                    m_lReturn = CType(PopulatePolicyDetailsList(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulatePolicyDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

            End Select



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function



    Private Sub frmBankGuaranteeDetails_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing

        'If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
        '    Me.Close()
        'End If



        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        ' Check if the interface has been terminated by means
        ' other than pressing the command buttons.

        If UnloadMode <> vbFormCode Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            'Me.Close()
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub
    'Start - Sankar - Bank Guarantee Bug Fixing
    Private Sub lvwPolicydetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPolicydetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwPolicydetails.Columns(eventArgs.Column)
        ListViewHelper.SetSortedProperty(lvwPolicydetails, False)
        ListViewHelper.SetSortKeyProperty(lvwPolicydetails, ColumnHeader.Index + 1 - 1)
        ListViewHelper.SetSortOrderProperty(lvwPolicydetails, (ListViewHelper.GetSortOrderProperty(lvwPolicydetails) + 1) Mod 2)
        ListViewHelper.SetSortedProperty(lvwPolicydetails, True)
    End Sub
    'End - Sankar - Bank Guarantee Bug Fixing

    Private Sub PickListBranches_Find(ByVal Sender As Object, ByVal e As EventArgs) Handles PickListBranches.Find

        For lCount As Integer = PickListBranches.ForeignKeys.Count To 1 Step -1
            PickListBranches.ForeignKeys.Remove(lCount)
        Next



        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "WhereClause"
        Key.ValueType = gPMConstants.PMEDataType.PMString
        PickListBranches.ForeignKeys.Add(Key, Key:="WhereClause")


        PickListBranches.ForeignKeys.Item("WhereClause").value = PickListBranches.SearchString

        PickListBranches.PickListType = "FindSource"
        m_lReturn = PickListBranches.LoadSearched()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

    End Sub

    Private Sub PickListProducts_Find(ByVal Sender As Object, ByVal e As EventArgs) Handles PickListProducts.Find

        For lCount As Integer = PickListProducts.ForeignKeys.Count To 1 Step -1
            PickListProducts.ForeignKeys.Remove(1)
        Next


        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "WhereClause"
        Key.ValueType = gPMConstants.PMEDataType.PMString
        PickListProducts.ForeignKeys.Add(Key, Key:="WhereClause")


        PickListProducts.ForeignKeys.Item("WhereClause").value = PickListProducts.SearchString

        PickListProducts.PickListType = "FindProduct"
        m_lReturn = PickListProducts.LoadSearched()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If
    End Sub


    Private isInitializingComponent As Boolean
    Private Sub txtAmount_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            txtLimitsAvailable.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=(CStr(gPMFunctions.ToSafeCurrency(txtAmount.Text))))
        End If
    End Sub

    Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave
        txtAmount.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(txtAmount.Text)))
    End Sub


    Private Sub txtExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpiryDate.Leave
        txtExpiryDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateMedium, vFieldValue:=txtExpiryDate.Text)
    End Sub

    Private Sub txtIssueDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIssueDate.Leave
        txtIssueDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateMedium, vFieldValue:=txtIssueDate.Text)
    End Sub

    Private Function SetPickList() As Integer

        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "Bg_Id"
        Key.ValueType = gPMConstants.PMEDataType.PMLong
        PickListProducts.ForeignKeys.Add(Key, Key:="Bg_Id")



        PickListProducts.ForeignKeys.Item("Bg_Id").value = m_vGuaranteeItem(MainModule.ENBankGuarantee.BGId)
        PickListProducts.PickListType = "Product"
        'developer guide no. 108
        m_lReturn = PickListProducts.Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

        PickListBranches.ForeignKeys.Add(Key, Key:="Bg_Id")


        PickListBranches.ForeignKeys.Item("Bg_Id").value = m_vGuaranteeItem(MainModule.ENBankGuarantee.BGId)

        PickListBranches.PickListType = "Source"
        'developer guide no. 108
        m_lReturn = PickListBranches.Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Branches", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If


        Dim vArray As Object = PickListProducts.ItemArray
    End Function

    ' ***************************************************************** '
    ' Name: PopulateBankDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-07-2007 :
    ' ***************************************************************** '
    Private Function PopulatePolicyDetailsList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulatePolicyDetailsList"
        Dim lTotalTransAmt As Integer
        Dim oListItem As ListViewItem

        Try



            result = gPMConstants.PMEReturnCode.PMTrue




            If gPMFunctions.IsArrayEmpty(m_vAttachedPolicies) Then
                Return result
            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwPolicydetails.Items.Clear()

            For i As Integer = m_vAttachedPolicies.GetLowerBound(1) To m_vAttachedPolicies.GetUpperBound(1)
                If gPMFunctions.ToSafeDouble(m_vAttachedPolicies(MainModule.ENBankGuarantee.RowStatus, i)) <> gPMConstants.PMEReturnCode.PMNotFound Then

                    oListItem = lvwPolicydetails.Items.Add(CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.ClientCode, i)).Trim(), "")
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndexClientName).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.ClientName, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndexInsuranceRef).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.InsuranceRef, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndexAgentCode).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.AgentCode, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndexBranch).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.Branch, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndexProduct).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.product, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndexAmount).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.Amount, i)).Trim())))

                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndeCoverFrom).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.CoverFrom, i)).Trim()
                    ListViewHelper.GetListViewSubItem(oListItem, kPolicyDetailsColHIndeCoverTo).Text = CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.CoverTo, i)).Trim()

                    oListItem.Tag = CStr(m_vAttachedPolicies(MainModule.ENBankGuarantee.RowIndex, i))
                    'Start - Sankar - Bank Guarantee Bug Fixing
                    lTotalTransAmt = CInt(lTotalTransAmt + gPMFunctions.ToSafeCurrency(CStr(m_vAttachedPolicies(MainModule.ENPolicyDetails.Amount, i)).Trim(), 0))
                    'End - Sankar - Bank Guarantee Bug Fixing
                End If

            Next i
            'Start - Sankar - Bank Guarantee Bug Fixing
            txtTotalTransAmt.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(lTotalTransAmt))
            'End - Sankar - Bank Guarantee Bug Fixing



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Sub txtLimitsAvailable_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLimitsAvailable.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        txtLimitsAvailable.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(txtLimitsAvailable.Text)))

    End Sub

    Private Function CreateWorkManagerTask() As Integer
        'Start - Sankar - Bank Guarantee Bug Fixing
        Dim result As Integer = 0
        Const kMethodName As String = "CreateWorkManagerTask"
        'End - Sankar - Bank Guarantee Bug Fixing
        Dim oTaskInstance As iPMWrkTaskInstance.Interface_Renamed
        Dim lReturn As Integer
        Dim dtDueDate As Date
        Dim v_lAction As Integer
        Dim temp_oTaskInstance As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            'developer guide no. 108 (guide)



            v_lAction = 1

            ' Create the Component

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

            'Start - Sankar - Bank Guarantee Bug Fixing
            If Not gPMFunctions.IsArrayEmpty(m_vGuaranteeItem) Then


                oTaskInstance.Customer = Convert.ToString(m_vGuaranteeItem(MainModule.ENBankGuarantee.Resolved_Name))


                oTaskInstance.Description = m_vGuaranteeItem(MainModule.ENBankGuarantee.BGRef)
            End If
            'End - Sankar - Bank Guarantee Bug Fixing


            oTaskInstance.DueDate = DateTime.Now

            oTaskInstance.DisableCustomer = gPMConstants.PMEReturnCode.PMTrue

            ' Set Task Group Id and Task Id (Memo Task)

            oTaskInstance.PMWrkTaskGroupId = 5

            oTaskInstance.PMWrkTaskId = 18

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oTaskInstance.Start Failed", gPMConstants.PMELogLevel.PMLogError)
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



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PopulatePolicyDetailsList(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function



End Class
