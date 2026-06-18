Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmChangePolicyDetails
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmChangePolicyDetails"

    Private m_oPolicyNumberGeneration As Object

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PolicyNumber
    Private m_sPolicyNumber As String = ""
    ' CoverStartDate
    Private m_sCoverStartDate As String = ""
    ' CoverExpiryDate
    Private m_sCoverExpiryDate As String = ""
    ' Status
    Private m_lStatus As gPMConstants.PMEReturnCode
    ' ProductId
    Private m_lProductId As Integer
    ' BusinessType
    Private m_lBusinessType As Integer
    ' AgentId
    Private m_lAgentId As Integer
    ' BranchId
    Private m_lBranchId As Integer
    Private m_lPartyCnt As Integer

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    Public Property BranchId() As Integer
        Get
            Return m_lBranchId
        End Get
        Set(ByVal Value As Integer)
            m_lBranchId = Value
        End Set
    End Property

    Public Property AgentId() As Integer
        Get
            Return m_lAgentId
        End Get
        Set(ByVal Value As Integer)
            m_lAgentId = Value
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

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
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

    Public Property CoverExpiryDate() As String
        Get
            Return m_sCoverExpiryDate
        End Get
        Set(ByVal Value As String)
            m_sCoverExpiryDate = Value
        End Set
    End Property

    Public Property CoverStartDate() As String
        Get
            Return m_sCoverStartDate
        End Get
        Set(ByVal Value As String)
            m_sCoverStartDate = Value
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

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0

        Dim oBusiness As Object
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object.
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oBusiness.GetProductdetails(v_lProductId:=m_lProductId, v_lOption:=38, r_vValue:=vValue)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vValue = "" Or StringsHelper.ToDoubleSafe(vValue) = 0 Then
                cmdChange.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

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
        m_sCoverExpiryDate = txtCoverExpiryDate.Text.Trim()
        m_sCoverStartDate = txtCoverStartDate.Text.Trim()
        m_sPolicyNumber = txtPolicyNum.Text.Trim()

        Me.Hide()

    End Sub

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

    End Sub


    Private Sub frmChangePolicyDetails_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

        txtPolicyNum.Text = m_sPolicyNumber
        txtPolicyNum.Enabled = False
        txtCoverStartDate.Text = m_sCoverStartDate
        txtCoverExpiryDate.Text = m_sCoverExpiryDate
        '    txtCoverStartDate.Text = Format(m_sCoverStartDate, "dd/mm/ccyy")
        '    txtCoverExpiryDate.Text = Format(m_sCoverExpiryDate, "dd/mm/ccyy")

        m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

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

            m_lReturn = m_oPolicyNumberGeneration.GeneratePolicyNumber(v_lBusinessType:=m_lBusinessType, v_iBranch:=m_lBranchId, v_lProductId:=m_lProductId, v_lAgent:=m_lAgentId, r_sGeneratedPolicyNumber:=sNewPolicyNumber, v_dtTransactionDate:=m_sCoverStartDate, v_lPartyCnt:=PartyCnt)
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

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

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
        Dim sFailureReason, sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If the policy number has been changed then validate it.
            If Not (m_oPolicyNumberGeneration Is Nothing) Then

                m_lReturn = m_oPolicyNumberGeneration.ValidatePolicyNumber(sEnteredNumber:=txtPolicyNum.Text.Trim(), sFailureReason:=sFailureReason)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Get description from the resource file.

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidationTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    MessageBox.Show(sFailureReason, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtPolicyNum.Focus()
                    Return result
                End If
            End If

            'Check dates have been entered
            If CBool(CStr(txtCoverStartDate.Text = "").Trim()) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidationTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryStartDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCoverStartDate.Focus()
                Return result
            End If
            If CBool(CStr(txtCoverExpiryDate.Text = "").Trim()) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidationTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryExpiryDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCoverExpiryDate.Focus()
                Return result
            End If

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidationTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If Not Information.IsDate(txtCoverStartDate.Text) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please Enter a Valid Cover Start Date", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCoverStartDate.Focus()
                Return result
            End If

            If Not Information.IsDate(txtCoverExpiryDate.Text) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("Please Enter a Valid Cover Expiry Date", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCoverExpiryDate.Focus()
                Return result
            End If

            'If dates have been entered then check Expiry > Start.
            If CDate(txtCoverStartDate.Text.Trim()) > CDate(txtCoverExpiryDate.Text.Trim()) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidationTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryStartGreaterThanExpiry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCoverStartDate.Focus()
                Return result
            End If

            'making sure new period is not overlapping with old policy version
            If CDate(txtCoverStartDate.Text.Trim()) < CDate(m_sCoverStartDate) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidationTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show("Start date is overlapping with previous version of the policy", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCoverStartDate.Focus()
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
