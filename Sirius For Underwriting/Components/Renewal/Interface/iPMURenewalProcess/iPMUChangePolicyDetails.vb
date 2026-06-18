Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmChangePolicyDetails
    Inherits System.Windows.Forms.Form
    Private Sub frmChangePolicyDetails_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub

    Private Const ACClass As String = "frmChangePolicyDetails"


    Private m_oPolicyNumberGeneration As bSIRPolicyNumMaint.Business

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_sPolicyNumber As String = ""
    Private m_dCoverStartDate As Date
    Private m_dCoverExpiryDate As Date
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lProductID As Integer
    Private m_lBusinessType As Integer
    Private m_lAgentID As Integer
    Private m_lBranchID As Integer
    Private m_lPartyCnt As Integer
    Public Property BranchID() As Integer
        Get
            Return m_lBranchID
        End Get
        Set(ByVal Value As Integer)
            m_lBranchID = Value
        End Set
    End Property


    Public Property AgentId() As Integer
        Get
            Return m_lAgentID
        End Get
        Set(ByVal Value As Integer)
            m_lAgentID = Value
        End Set
    End Property


    Public Property BusinessType() As Integer
        Get
            Return m_lBusinessType
        End Get
        Set(ByVal Value As Integer)
            m_lBusinessType = Value
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


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property


    Public Property CoverExpiryDate() As Date
        Get
            Return m_dCoverExpiryDate
        End Get
        Set(ByVal Value As Date)
            m_dCoverExpiryDate = Value
        End Set
    End Property


    Public Property CoverStartDate() As Date
        Get
            Return m_dCoverStartDate
        End Get
        Set(ByVal Value As Date)
            m_dCoverStartDate = Value
        End Set
    End Property


    Public Property PolicyNumber() As String
        Get
            Return m_sPolicyNumber
        End Get
        Set(ByVal Value As String)
            m_sPolicyNumber = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

    End Sub

    Private Sub cmdChange_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdChange.Click

        m_lReturn = ChangePolicyNumber()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        m_lReturn = ValidateChanges()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK


        m_dCoverExpiryDate = dtpCoverExpiryDate.Value
        m_dCoverExpiryDate = dtpCoverExpiryDate.Value

        m_dCoverStartDate = dtpCoverStartDate.Value
        m_sPolicyNumber = txtPolicyNum.Text.Trim()

        Me.Hide()

    End Sub


    Private Sub frmChangePolicyDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim vResult As String = ""

        txtPolicyNum.Text = m_sPolicyNumber
        txtPolicyNum.Enabled = False
        dtpCoverStartDate.Value = m_dCoverStartDate
        dtpCoverExpiryDate.Value = m_dCoverExpiryDate

        'disable button if option is not set in product screen
        'cmdChange.Enabled


        m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product", v_vReturnColumn:="change_policy_number_at_renewal", v_sKeyColumn:="product_id", v_sKeyValue:=m_lProductID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResult)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get details from product", "Renewal Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        cmdChange.Enabled = (vResult = "1")

        'Start - Renuka - (WPR87 Paralleling)

        m_lReturn = g_oBusiness.GetValueFromTable(v_sTableName:="Product", v_vReturnColumn:="Change_ren_policy_no_auto", v_sKeyColumn:="product_id", v_sKeyValue:=m_lProductID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResult)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to get details from product", "Renewal Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        If vResult = "1" Then
            m_lReturn = ChangePolicyNumber()
        End If
        'End - Renuka - (WPR87 Paralleling)
    End Sub
    ' ***************************************************************** '
    '
    ' Name: ChangePolicyNumber
    '
    ' Description:
    '
    ' History: 05/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function ChangePolicyNumber() As Integer
        Dim result As Integer = 0
        Dim sNewPolicyNumber As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPolicyNumberGeneration Is Nothing Then
                ' Create business  object
                Dim temp_m_oPolicyNumberGeneration As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumberGeneration, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPolicyNumberGeneration = temp_m_oPolicyNumberGeneration

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End If

            'Start - Renuka - (WPR87 Paralleling)

            m_lReturn = m_oPolicyNumberGeneration.GeneratePolicyNumber(v_lBusinessType:=m_lBusinessType, v_iBranch:=m_lBranchID, v_lProductId:=m_lProductID, v_lAgent:=m_lAgentID, r_sGeneratedPolicyNumber:=sNewPolicyNumber, v_dtTransactionDate:=m_dCoverStartDate, v_lPartyCnt:=PartyCnt)
            'End - Renuka - (WPR87 Paralleling)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sNewPolicyNumber.Trim() <> "" Then
                m_sPolicyNumber = sNewPolicyNumber
                txtPolicyNum.Text = m_sPolicyNumber
            Else
                txtPolicyNum.Text = ""
                txtPolicyNum.Enabled = True
                txtPolicyNum.Focus()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangePolicyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangePolicyNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmChangePolicyDetails_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        If Not (m_oPolicyNumberGeneration Is Nothing) Then

            m_oPolicyNumberGeneration.Dispose()
            m_oPolicyNumberGeneration = Nothing
        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ValidateChanges
    '
    ' Description:
    '
    ' History: 05/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateChanges() As Integer
        Dim result As Integer = 0
        Dim sFailureReason, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If the policy number has been changed then validate it.
            If Not (m_oPolicyNumberGeneration Is Nothing) Then

                m_lReturn = m_oPolicyNumberGeneration.ValidatePolicyNumber(sEnteredNumber:=txtPolicyNum.Text.Trim(), sFailureReason:=sFailureReason)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    MessageBox.Show(sFailureReason, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtPolicyNum.Focus()
                    Return result
                End If
            End If

            'If dates have been entered then check Expiry > Start.
            If DateAndTime.DateDiff("d", dtpCoverStartDate.Value, dtpCoverExpiryDate.Value, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show("Cover start date cannot be greater than cover expiry date", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                dtpCoverStartDate.Focus()

                Return result
            End If

            'making sure new period is not overlapping with old policy version
            If DateAndTime.DateDiff("d", dtpCoverStartDate.Value, m_dCoverStartDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) > 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show("Start date is overlapping with previous version of the policy", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                dtpCoverStartDate.Focus()
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class