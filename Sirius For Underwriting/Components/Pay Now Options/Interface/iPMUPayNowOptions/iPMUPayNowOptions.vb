Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17th August 2006
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' Float Balance and Pre-Payment Work
    '
    ' Created By :    Deepak
    '
    ' ***************************************************************** '


    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    Private m_oAccount As bACTAccount.Form
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""

    Private m_sCallingAppName As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_sAgentType As String = ""
    Private m_sPolicyRef As String = ""
    Private m_lAgentCnt As Integer
    Private m_lClientId As Integer
    Private m_lAccountId As Integer
    Private m_bIsOverDraftAccount As Boolean
    Private m_bIsFloatBalanceAccount As Boolean
    Private m_cAccountBalance As Decimal
    Private m_cFloatBalance As Decimal
    Private m_cOverDraftBalance As Decimal
    Private m_cFloatBalanceLimit As Decimal
    Private m_cOverDraftLimit As Decimal
    Private m_vOverDraftExpiry As Object
    Private m_bLoading As Boolean
    Private m_crAmountDue As Decimal
    Private m_bOkClick As Boolean

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_frmInterface As iPMUPayNowOptions.frmInterface

    'Grid Column Constants
    Private Const TDBGridCol_IsSelected As Integer = 0
    Private Const TDBGridCol_TransDetailID As Integer = 1
    Private Const TDBGridCol_DocumentRef As Integer = 2
    Private Const TDBGridCol_MediaType As Integer = 3
    Private Const TDBGridCol_Reference As Integer = 4
    Private Const TDBGridCol_Amount As Integer = 5
    Private Const TDBGridCol_AccountID As Integer = 6
    'Rahul
    Private Const TDBGridCol_CollectionDate As Integer = 7
    Private Const TDBGridCol_AmountAllocated As Integer = 8


    Private m_GridArray As XArrayHelper
    Private m_iDebitAgainst As gPMConstants.PMDebitAgainst
    Private m_lPaymentAccountId As Integer
    Private m_vCreditTransactions As Object
    Private m_vUnallocatedTransactions(,) As Object
    Private m_bGridPopulated As Boolean
    'To check that Multiple Policies are Selected (Renewals Case)
    Private m_bMultiplePoliciesSelected As Boolean
    'Rahul
    Private m_dCoverStartDate As Date
    Private m_vLetters(,) As Object

    Public WriteOnly Property CoverStartDate() As Date
        Set(ByVal Value As Date)

            m_dCoverStartDate = Value

        End Set
    End Property


    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property


    Public Property OKClick() As Boolean
        Get

            Return m_bOkClick

        End Get
        Set(ByVal Value As Boolean)

            m_bOkClick = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property


    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property

    Public WriteOnly Property AmountDue() As Decimal
        Set(ByVal Value As Decimal)

            m_crAmountDue = Value

        End Set
    End Property


    Public Property DebitAgainst() As Integer
        Get

            Return m_iDebitAgainst

        End Get
        Set(ByVal Value As Integer)

            m_iDebitAgainst = Value

        End Set
    End Property


    Public Property PaymentAccountID() As Integer
        Get

            Return m_lPaymentAccountId

        End Get
        Set(ByVal Value As Integer)

            m_lPaymentAccountId = Value

        End Set
    End Property


    Public Property CreditTransactions() As Object
        Get

            Return m_vCreditTransactions

        End Get
        Set(ByVal Value As Object)



            m_vCreditTransactions = Value

        End Set
    End Property


    Public Property Letters() As Object
        Get

            Return VB6.CopyArray(m_vLetters)

        End Get
        Set(ByVal Value As Object)

            m_vLetters = Value

        End Set
    End Property

    Public WriteOnly Property AgentCnt() As Integer
        Set(ByVal Value As Integer)

            m_lAgentCnt = Value

        End Set
    End Property


    Public WriteOnly Property MultiplePoliciesSelected() As Boolean
        Set(ByVal Value As Boolean)

            m_bMultiplePoliciesSelected = Value

        End Set
    End Property

    'WPR12- Enhancement Quote Collection Process
    Public WriteOnly Property ClientId() As Integer
        Set(ByVal Value As Integer)
            m_lClientId = Value
        End Set
    End Property

    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim vResultArray, vAccountBalance As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}
            If Not m_bMultiplePoliciesSelected Then

                'Get Insurance File Reference From Insurance File Cnt

                m_lReturn = m_oBusiness.GetInsuranceRef(m_lInsuranceFileCnt, vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Insurance Reference", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                If Not Information.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insurance File Details Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If


                m_sPolicyRef = Convert.ToString(vResultArray(7, 0)).Trim()
                'Get the AgentCnt

                ' m_lAgentCnt = gPMFunctions.ToSafeLong(CInt(vResultArray(10, 0)))
                m_lAgentCnt = gPMFunctions.ToSafeInteger(vResultArray(10, 0))
                'Get the Client ID

                m_lClientId = gPMFunctions.ToSafeLong(ToSafeInteger(vResultArray(13, 0)))


                'Developer Guide No. 12
                vResultArray = Nothing
                m_bLoading = True
            End If
            'get Agent Type
            If m_lAgentCnt > 0 Then
                'Agent Exists
                If m_bMultiplePoliciesSelected Then

                    m_lReturn = m_oBusiness.GetAgentDetailsFromAgentID(m_lAgentCnt, vResultArray)
                Else

                    m_lReturn = m_oBusiness.GetAgentType(m_lInsuranceFileCnt, vResultArray)
                End If


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Agent Type.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                If Not Information.IsArray(vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Agent Type Found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                m_sAgentType = Convert.ToString(vResultArray(0, 0)).Trim()


                m_bIsFloatBalanceAccount = gPMFunctions.ToSafeBoolean(ToSafeInteger(vResultArray(1, 0)))

                m_bIsOverDraftAccount = gPMFunctions.ToSafeBoolean(ToSafeInteger(vResultArray(2, 0)))


                m_cFloatBalanceLimit = gPMFunctions.ToSafeCurrency(ToSafeInteger(vResultArray(3, 0)))

                m_cOverDraftLimit = gPMFunctions.ToSafeCurrency(ToSafeInteger(vResultArray(4, 0)))

                m_vOverDraftExpiry = vResultArray(5, 0)


                'Developer Guide No. 146 (latest guide)
                'Array.Clear(vResultArray, 0, vResultArray.Length)
                vResultArray = Nothing
                'Get the Account Balacne
                m_lReturn = CType(GetAccountDetails(lPartyCnt:=m_lAgentCnt, r_vAccountBalance:=vAccountBalance, r_vResultArray:=vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Account Details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                If Not Information.IsArray(vResultArray) Then
                    Return result
                End If


                m_cAccountBalance = gPMFunctions.ToSafeCurrency(ToSafeDouble(vResultArray(0, 0)))

                m_cFloatBalance = gPMFunctions.ToSafeCurrency(ToSafeDouble(vResultArray(2, 0)))

                m_cOverDraftBalance = gPMFunctions.ToSafeCurrency(ToSafeDouble(vResultArray(3, 0)))

            End If
            m_bLoading = False

            If Not (m_lAgentCnt > 0) Then
                'Developer Guide No. 146
                'Array.Clear(vResultArray, 0, vResultArray.Length)
                vResultArray = Nothing

                'Get the Account Balacne
                m_lReturn = CType(GetAccountDetails(lPartyCnt:=m_lClientId, r_vAccountBalance:=vAccountBalance, r_vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Account Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                If Not Information.IsArray(vResultArray) Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Account Details Not Found.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_cAccountBalance = gPMFunctions.ToSafeCurrency(ToSafeInteger(vResultArray(0, 0)))
                txtAccountBalance.Text = CStr(m_cAccountBalance)

            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function GetUnallocatedCredits(ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bMultiplePoliciesSelected Then
                If m_lAgentCnt > 0 Then

                    m_lReturn = m_oBusiness.GetUnallocatedCredits(0, False, r_vResultArray, m_lAgentCnt)
                Else

                    m_lReturn = m_oBusiness.GetUnallocatedCredits(0, True, r_vResultArray, m_lClientId)
                End If
            ElseIf optclient.Checked Then

                m_lReturn = m_oBusiness.GetUnallocatedCredits(m_lInsuranceFileCnt, True, r_vResultArray)
            Else

                m_lReturn = m_oBusiness.GetUnallocatedCredits(m_lInsuranceFileCnt, False, r_vResultArray)
            End If

            If Information.IsArray(r_vResultArray) Then
                m_lReturn = CType(AssignValuesToGrid(r_vResultArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_bGridPopulated = True
            Else
                m_GridArray.Clear()
                grdCredit.ReBind()
                m_bGridPopulated = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnallocatedCredits Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnallocatedCredits", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    Private Function AssignValuesToGrid(ByVal vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue



            If Information.IsArray(vResultArray) Then
                If Not (vResultArray.GetUpperBound(0) > 0) Then
                    grdCredit.Enabled = False
                End If

                m_GridArray = New XArrayHelper()

                m_GridArray.RedimXArray(New Integer() {vResultArray.GetUpperBound(1), vResultArray.GetUpperBound(0) + 3}, New Integer() {0, 0})

                For i_Row As Integer = 0 To vResultArray.GetUpperBound(1)
                    m_GridArray(i_Row, 0) = 0
                    For i_Col As Integer = 0 To vResultArray.GetUpperBound(0)

                        m_GridArray(i_Row, i_Col + 1) = vResultArray(i_Col, i_Row)
                    Next i_Col
                Next i_Row

                ' Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
                m_GridArray.AcceptChanges()
                grdCredit.AutoGenerateColumns = False
                grdCredit.DataSource = m_GridArray

                With grdCredit
                    .Columns(2).DataPropertyName = "Column3"
                    .Columns(3).DataPropertyName = "Column4"
                    .Columns(4).DataPropertyName = "Column5"
                    .Columns(5).DataPropertyName = "Column6"
                    .Columns(7).DataPropertyName = "Column8"
                    '.Columns(6).DataPropertyName = "Column6"
                    '.Columns(7).DataPropertyName = "Column7"
                    '.Columns(8).DataPropertyName = "Column8"
                End With

                'grdCredit.ReBind()
            End If


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AssignValuesToGrid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AssignValuesToGrid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Update the interface details.
            If m_bMultiplePoliciesSelected Then
                txtPolicyRef.Text = "Multiple Policies"
            Else
                txtPolicyRef.Text = m_sPolicyRef
            End If
            txtPolicyRef.Enabled = False

            txtTotalDue.Text = CStr(m_crAmountDue)
            txtTotalDue.Enabled = False

            optAccount.Visible = False
            txtAccountBalance.Visible = False

            txtFloatBalance.Text = CStr(-1 * m_cFloatBalance)
            txtOverdraft.Text = CStr(-1 * m_cOverDraftBalance)
            txtFloatBalance.Enabled = False
            txtOverdraft.Enabled = False
            txtUnallocatedCredit.Enabled = False

            'Fill the Grid
            SetUpControls()

            'With grdCredit
            '    Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
            '    .DataSource = bindingSource
            '    .ReBind()
            'End With

            m_GridArray.AcceptChanges()
            grdCredit.AutoGenerateColumns = False
            grdCredit.DataSource = m_GridArray

            With grdCredit
                .Columns(2).DataPropertyName = "Column3"
                .Columns(3).DataPropertyName = "Column4"
                .Columns(4).DataPropertyName = "Column5"
                .Columns(5).DataPropertyName = "Column6"
                .Columns(7).DataPropertyName = "Column8"
                '.Columns(6).DataPropertyName = "Column6"
                '.Columns(7).DataPropertyName = "Column7"
                '.Columns(8).DataPropertyName = "Column8"
            End With

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function InterfaceToData() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            PaymentAccountID = m_lAccountId

            If optFloatBalance.Checked Then
                DebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstFloatBalance
            ElseIf optOverdraft.Checked Then
                DebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstOverDraft
            ElseIf optUnallocatedCredit.Checked Then
                DebitAgainst = gPMConstants.PMDebitAgainst.PMDebitAgainstUnallocatedCredit
            End If


            CreditTransactions = m_vCreditTransactions
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Data", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set Interface Defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Business To Interface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If

            ' Check the task.
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        OKClick = False
        'Hide the Form
        Me.Visible = False

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = CType(ValidateForm(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Get the Account Id for Agent/Client
            m_lReturn = CType(GetAccountID(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            If optUnallocatedCredit.Checked And Conversion.Val(txtTotalDue.Text) > 0 Then
                m_lReturn = FillCreditTransactionsArray()
            ElseIf Conversion.Val(txtTotalDue.Text) < 0 Then

                m_vCreditTransactions = ""
            End If
            m_lReturn = InterfaceToData()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                OKClick = True
                Me.Hide()
            End If

        Catch excep As System.Exception


            OKClick = False
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Function GetAccountID() As Integer
        Dim result As Integer = 0
        Try


            Dim vResultArray(,) As Object
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.
            If optAgent.Checked Then

                m_lReturn = m_oBusiness.GetAccountID(m_lAgentCnt, vResultArray)
            Else

                m_lReturn = m_oBusiness.GetAccountID(m_lClientId, vResultArray)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lAccountId = gPMFunctions.ToSafeLong(ToSafeInteger(vResultArray(0, 0)))


            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the AccountID", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub Form_Initialize_Renamed()
        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPayNowOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                ' Display error stating the Problem

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

        Catch excep As System.Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                gPMFunctions.RaiseError(kMethodName, "SetupForm failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If


            m_lReturn = GetInterfaceDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                gPMFunctions.RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)

                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            m_GridArray = New XArrayHelper()
            'm_GridArray.RedimXArray(New Integer() {0, 0}, New Integer() {0, 0})

            'Call m_GridArray.AppendRows(1)


            With grdCredit
                '.ReBind()
                '.Refresh()

                'Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
                '.DataSource = bindingSource
                .Columns(TDBGridCol_IsSelected).HeaderText = ""
                .Columns(TDBGridCol_IsSelected).ReadOnly = False
                .Columns(TDBGridCol_IsSelected).Width = 25

                .Columns(TDBGridCol_TransDetailID).HeaderText = "TransDetail ID"
                .Columns(TDBGridCol_TransDetailID).ReadOnly = True
                .Columns(TDBGridCol_TransDetailID).Width = 0
                .Columns(TDBGridCol_TransDetailID).Visible = False


                .Columns(TDBGridCol_DocumentRef).HeaderText = "Document Ref"
                .Columns(TDBGridCol_DocumentRef).ReadOnly = True
                .Columns(TDBGridCol_DocumentRef).Width = 100

                .Columns(TDBGridCol_MediaType).HeaderText = "Media Type"
                .Columns(TDBGridCol_MediaType).ReadOnly = True
                .Columns(TDBGridCol_MediaType).Width = 100

                .Columns(TDBGridCol_Reference).HeaderText = "Reference"
                .Columns(TDBGridCol_Reference).ReadOnly = True
                .Columns(TDBGridCol_Reference).Width = 100

                .Columns(TDBGridCol_Amount).HeaderText = "Amount"
                .Columns(TDBGridCol_Amount).ReadOnly = True
                .Columns(TDBGridCol_Amount).Width = 100

                .Columns(TDBGridCol_AccountID).Width = 0
                .Columns(TDBGridCol_AccountID).Visible = False


                .Columns(TDBGridCol_AmountAllocated).HeaderText = "Amount Allocated"
                .Columns(TDBGridCol_AmountAllocated).ReadOnly = True
                .Columns(TDBGridCol_AmountAllocated).Width = 0
                .Columns(TDBGridCol_AmountAllocated).Visible = False

                .Columns(TDBGridCol_CollectionDate).HeaderText = "Collection Date"
                .Columns(TDBGridCol_CollectionDate).ReadOnly = True
                .Columns(TDBGridCol_CollectionDate).Width = 100
                .Columns(TDBGridCol_CollectionDate).Visible = True
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try

            Dim cWriteOffLimit As Decimal

            result = gPMConstants.PMEReturnCode.PMTrue

            If optclient.Enabled And optAgent.Enabled And Not optclient.Checked And Not optAgent.Checked Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("At least one of the Account options needs to be selected", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            If optAccount.Visible Then
                If Not optAccount.Checked And Not optUnallocatedCredit.Checked Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("At least one of the Debit options needs to be selected", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            Else
                If Not optFloatBalance.Checked And Not optOverdraft.Checked And Not optUnallocatedCredit.Checked Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("At least one of the Debit options needs to be selected", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            End If
            If optAccount.Visible And optAccount.Checked And Conversion.Val(txtTotalDue.Text) > 0 And Conversion.Val(txtTotalDue.Text) > Math.Abs(Conversion.Val(txtAccountBalance.Text)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Sufficient Account balance is not available.", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            If Not optUnallocatedCredit.Checked Then

            End If
            If Conversion.Val(txtTotalDue.Text) > 0 Then

                If optOverdraft.Checked And Conversion.Val(txtTotalDue.Text) > -1 * Conversion.Val(txtOverdraft.Text) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Sufficient Overdraft balance is not available.", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                If optFloatBalance.Checked And Conversion.Val(txtTotalDue.Text) > -1 * Conversion.Val(txtFloatBalance.Text) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Sufficient Float balance is not available.", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                If m_vOverDraftExpiry <> "" Then
                    If optOverdraft.Enabled And optOverdraft.Checked And CDate(m_vOverDraftExpiry) <= DateTime.Today Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Overdraft Facility has expired, please try an alternative settlement method.", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return result
                    End If
                End If


                m_lReturn = m_oBusiness.GetUserWriteOffLimit(r_cWriteOffLimit:=cWriteOffLimit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If optUnallocatedCredit.Checked And (Conversion.Val(txtTotalDue.Text) > ((-1 * Conversion.Val(txtUnallocatedCredit.Text)) + cWriteOffLimit) Or Conversion.Val(txtUnallocatedCredit.Text) = 0) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("You have not provided Sufficient Unallocated Credit.", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                If txtAccountBalance.Visible And optAccount.Visible And optAccount.Checked And Conversion.Val(txtAccountBalance.Text) > 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Account balance is not available", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                If txtAccountBalance.Visible And optAccount.Visible And optAccount.Checked And Conversion.Val(txtAccountBalance.Text) < 0 And Conversion.Val(txtTotalDue.Text) > -1 * Conversion.Val(txtAccountBalance.Text) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Account balance is not available", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            End If
            'Rahul
            If Not Information.IsArray(m_GridArray) AndAlso m_GridArray.Columns.Count > 0 Then
                For i_var As Integer = 0 To m_GridArray.GetUpperBound(0)
                    'Developer Guide No. 188(latest guide)
                    If m_GridArray(i_var, 0) = -1 Then
                        'Developer Guide No. 188 (latest guide)
                        If Information.IsDate(m_GridArray(i_var, TDBGridCol_CollectionDate)) Then
                            'Start (Sriram P)PN 54241
                            'Developer Guide No. 188 (latest guide)
                            If m_GridArray(i_var, TDBGridCol_CollectionDate) > m_dtEffectiveDate Then
                                'End (Sriram P)PN 54241
                                MessageBox.Show("Cover from date cannot be prior to collection date", "Pay Now Options", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                Next
            End If
            'End
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function FillCreditTransactionsArray() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iSelectedItems, iArrayIndex As Integer

            For i_var As Integer = 0 To m_GridArray.GetUpperBound(0)
                'Developer Guide No. 188 (latest guide)
                If m_GridArray(i_var, 0) = -1 Then
                    iSelectedItems += 1
                End If
            Next
            If iSelectedItems >= 1 Then

                ReDim m_vCreditTransactions(2, iSelectedItems - 1)

                ReDim m_vLetters(1, iSelectedItems - 1)
            End If

            For i_var As Integer = 0 To m_GridArray.GetUpperBound(0)
                'Developer Guide No. 188 (latest guide)
                If m_GridArray(i_var, 0) = -1 Then
                    'Copy Account ID

                    'Developer Guide No. 188 (latest guide)
                    m_vCreditTransactions(0, iArrayIndex) = m_GridArray(i_var, TDBGridCol_AccountID)
                    'Copy Trans ID

                    'Developer Guide No. 188 (latest guide)
                    m_vCreditTransactions(1, iArrayIndex) = m_GridArray(i_var, TDBGridCol_TransDetailID)
                    'Copy the Amount

                    'Developer Guide No. 188 (latest guide)
                    m_vCreditTransactions(2, iArrayIndex) = m_GridArray(i_var, TDBGridCol_AmountAllocated)
                    'Developer Guide No. 188 (latest guide)
                    m_vLetters(1, iArrayIndex) = m_GridArray(i_var, TDBGridCol_DocumentRef)

                    If m_lClientId <> 0 Then
                        m_vLetters(0, iArrayIndex) = m_lAgentCnt
                    Else
                        m_vLetters(0, iArrayIndex) = m_lAgentCnt
                    End If

                    iArrayIndex += 1
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillCreditTransactionsArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillCreditTransactionsArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub SetUpControls()
        Try
            Dim cUnAllocatedCredit As Decimal

            optFloatBalance.Enabled = m_bIsFloatBalanceAccount
            optOverdraft.Enabled = m_bIsOverDraftAccount

            If m_sAgentType.Trim() <> "" Then 'Direct Business
                optclient.Left = VB6.TwipsToPixelsX(5640)
                optAgent.Visible = True
                optOverdraft.Visible = True
                optFloatBalance.Visible = True
                txtFloatBalance.Visible = True
                txtOverdraft.Visible = True
                optAccount.Visible = False
                txtAccountBalance.Visible = False
            End If
            txtUnallocatedCredit.Text = ""

            If optclient.Checked Then
                optOverdraft.Enabled = False
                optFloatBalance.Enabled = False
            End If

            m_lReturn = CType(GetUnallocatedCredits(m_vUnallocatedTransactions), gPMConstants.PMEReturnCode)

            If m_sAgentType.Trim() = "Broker" And m_crAmountDue >= 0 Then
                optclient.Enabled = False
                optAgent.Checked = True
            ElseIf m_sAgentType.Trim() = "Broker" And m_crAmountDue < 0 Then
                optclient.Enabled = False
                optAgent.Checked = True
                HideGrid()
                If m_bGridPopulated Then
                    m_lReturn = CType(GetTotalUnallocatedAmount(r_cUnAllocatedAmount:=cUnAllocatedCredit), gPMConstants.PMEReturnCode)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'SetUpControls = m_lReturn
                End If

                txtUnallocatedCredit.Text = CStr(cUnAllocatedCredit)
            ElseIf m_sAgentType.Trim() = "Sub-Agent" Then


            ElseIf m_sAgentType.Trim() = "Comm Acc" And m_crAmountDue >= 0 Then

                optAgent.Enabled = False 'PN 30859
                optclient.Checked = True
                txtFloatBalance.Text = ""
                txtOverdraft.Text = ""
            ElseIf m_sAgentType.Trim() = "Comm Acc" And m_crAmountDue < 0 Then
                optAgent.Enabled = False
                optclient.Checked = True
                txtFloatBalance.Text = ""
                txtOverdraft.Text = ""
                HideGrid()
                If m_bGridPopulated Then
                    m_lReturn = CType(GetTotalUnallocatedAmount(r_cUnAllocatedAmount:=cUnAllocatedCredit), gPMConstants.PMEReturnCode)
                End If

                txtUnallocatedCredit.Text = CStr(cUnAllocatedCredit)
            ElseIf m_sAgentType.Trim() = "Reins" Then

            ElseIf m_sAgentType.Trim() = "Intermed" And m_crAmountDue >= 0 Then
                'Both Client and Agent Option are enabled
                If m_bMultiplePoliciesSelected Then
                    optclient.Enabled = False
                End If
            ElseIf m_sAgentType.Trim() = "Intermed" And m_crAmountDue < 0 Then
                HideGrid()
                If m_bGridPopulated Then
                    m_lReturn = CType(GetTotalUnallocatedAmount(r_cUnAllocatedAmount:=cUnAllocatedCredit), gPMConstants.PMEReturnCode)
                End If

                txtUnallocatedCredit.Text = CStr(cUnAllocatedCredit)
            Else
                'Direct Business
                If m_crAmountDue < 0 Then
                    HideGrid()
                    If m_bGridPopulated Then
                        m_lReturn = CType(GetTotalUnallocatedAmount(r_cUnAllocatedAmount:=cUnAllocatedCredit), gPMConstants.PMEReturnCode)
                    End If
                    txtUnallocatedCredit.Text = CStr(cUnAllocatedCredit)
                End If
                optclient.Left = optAgent.Left
                optAgent.Visible = False
                optclient.Enabled = False
                optclient.Checked = True
                optAccount.Visible = True
                optAccount.Left = optOverdraft.Left
                txtAccountBalance.Visible = True
                txtAccountBalance.Enabled = False
                txtOverdraft.Visible = False
                txtFloatBalance.Visible = False
                optOverdraft.Visible = False
                optFloatBalance.Visible = False
            End If

            grdCredit.Enabled = optUnallocatedCredit.Checked And m_bGridPopulated And Conversion.Val(txtTotalDue.Text) > 0

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetUpControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
    Private Sub HideGrid()
        Try

            'Hide the Grid in case
            '   1. Agent Type :  Intermediary and Credit Amount
            '   2. Agent Type :  Broker and Credit Amount

            'optUnallocatedCredit.Visible = False
            'txtUnallocatedCredit.Visible = False
            grdCredit.Visible = False
            fraDebitAgainst.Height = VB6.TwipsToPixelsY(1695)
            fraAccount.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraDebitAgainst.Top) + VB6.PixelsToTwipsY(fraDebitAgainst.Height) + 170)
            cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraAccount.Top) + VB6.PixelsToTwipsY(fraAccount.Height) + 100)
            cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraAccount.Top) + VB6.PixelsToTwipsY(fraAccount.Height) + 100)
            Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdOK.Top) + VB6.PixelsToTwipsY(cmdOK.Height) + 600)

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideGrid", vApp:=ACApp, vClass:=ACClass, vMethod:="HideGrid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Sub






    Public Function GetAccountDetails(ByVal lPartyCnt As Integer, ByRef r_vAccountBalance As Object, Optional ByRef r_vResultArray(,) As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try


            Dim sSQL As String = ""
            Dim vResultArray(,) As Object
            Dim vAccountID As Object
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetAccountID(v_lPartyCnt:=lPartyCnt, r_vResults:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            vAccountID = vResultArray(0, 0)

            Dim temp_m_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oAccount.GetAccountBalance(r_vdAccountBalance:=r_vAccountBalance, v_vAccountID:=vAccountID, v_vAccountingDate:=Nothing, r_vResultArray:=r_vResultArray, r_vlAccountCurrencyId:=Nothing, r_vdAccountDebt:=Nothing, r_vAccountFloatBalance:=Nothing, r_vAccountOverDraftBalance:=Nothing)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountdetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' Get the Total Unallocated Amount for Credit Entries

    Private Function GetTotalUnallocatedAmount(ByRef r_cUnAllocatedAmount As Decimal) As Integer

        Try

            For i_var As Integer = 0 To m_GridArray.GetUpperBound(0)
                'Developer Guide No. 188 (latest guide)
                r_cUnAllocatedAmount += m_GridArray(i_var, TDBGridCol_Amount)
            Next

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTotalUnallocatedAmount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTotalUnallocatedAmount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Function

    Private isInitializingComponent As Boolean
    Private Sub optAccount_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optAccount.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            grdCredit.Enabled = False
            txtUnallocatedCredit.Text = ""
        End If
    End Sub

    Private Sub optclient_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optclient.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            optFloatBalance.Checked = False
            optOverdraft.Checked = False
            SetUpControls()
        End If
    End Sub

    Private Sub optFloatBalance_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optFloatBalance.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetUpControls()
        End If
    End Sub

    Private Sub optOverdraft_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optOverdraft.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetUpControls()
        End If
    End Sub

    Private Sub optUnallocatedCredit_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optUnallocatedCredit.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetUpControls()
        End If
    End Sub
    Private Sub optAgent_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optAgent.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            optFloatBalance.Checked = False
            optOverdraft.Checked = False
            SetUpControls()
        End If
    End Sub

    Private Sub grdCredit_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdCredit.CellFormatting
        Select Case e.ColumnIndex
            Case TDBGridCol_Amount
                e.Value = Decimal.Round(gPMFunctions.ToSafeDecimal(e.Value), 2)
        End Select
    End Sub

    Private Sub grdCredit_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdCredit.CellValueChanged
        Dim ColIndex As Integer = e.ColumnIndex
        Try
            If grdCredit.RowsCount = 0 Then
                Exit Sub
            End If

            Dim vUnAllocatedCredit As Decimal

            If ColIndex = 0 Then

                grdCredit.UpdateCurrentRow()

                Dim chkBoxCell As DataGridViewCheckBoxCell = grdCredit.Rows(e.RowIndex).Cells(0)

                If Not chkBoxCell.Value Is Nothing AndAlso chkBoxCell.Value Then
                    m_GridArray(e.RowIndex, 0) = -1
                Else
                    m_GridArray(e.RowIndex, 0) = 0
                End If

                'Show the Total Unallocated Credit
                txtUnallocatedCredit.Text = CStr(0)

                vUnAllocatedCredit = Conversion.Val(txtTotalDue.Text)

                For i_var As Integer = 0 To m_GridArray.GetUpperBound(0)
                    'Developer Guide No. 188 (latest guide)
                    If m_GridArray(i_var, 0) = -1 Then

                        'Developer Guide No. 188 (Latest guide) 
                        txtUnallocatedCredit.Text = CStr(Conversion.Val(txtUnallocatedCredit.Text) + -1 * m_GridArray(i_var, TDBGridCol_Amount))
                        'Developer Guide No. 188 (Latest guide)
                        If vUnAllocatedCredit >= -1 * m_GridArray(i_var, TDBGridCol_Amount) Then
                            'Developer Guide No. 188 (Latest guide)
                            m_GridArray(i_var, TDBGridCol_AmountAllocated) = -1 * m_GridArray(i_var, TDBGridCol_Amount)
                            'Developer Guide No. 188 (latest guide)
                            vUnAllocatedCredit -= m_GridArray(i_var, TDBGridCol_AmountAllocated)
                        Else
                            m_GridArray(i_var, TDBGridCol_AmountAllocated) = vUnAllocatedCredit
                            vUnAllocatedCredit = 0
                        End If
                    Else
                        m_GridArray(i_var, TDBGridCol_AmountAllocated) = 0
                    End If
                Next

                If txtUnallocatedCredit.Text <> "" Then
                    txtUnallocatedCredit.Text = CStr(CDbl(txtUnallocatedCredit.Text) * -1)
                End If
            End If
            grdCredit.UpdateCurrentRow()
            grdCredit.ReBind()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="grdCredit_AfterColUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="grdCredit_ColEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub grdCredit_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdCredit.CurrentCellDirtyStateChanged
        If DirectCast(sender, Artinsoft.Windows.Forms.ExtendedDataGridView).CurrentColumnIndex = 0 Then
            If DirectCast(sender, Artinsoft.Windows.Forms.ExtendedDataGridView).IsCurrentCellDirty Then
                DirectCast(sender, Artinsoft.Windows.Forms.ExtendedDataGridView).CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End If
    End Sub
End Class
