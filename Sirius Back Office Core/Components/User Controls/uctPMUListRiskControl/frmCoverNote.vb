Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Partial Class frmCoverNote
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	'
	' Name:         frmCoverNote
	'
	' Description:  Cover Note form to capture the cover note From and
	'               To date
	' Created:
	'
	' ***************************************************************** '
	
	Private Const ACClass As String = "frmCoverNote"
	
	
	'********************************
	' General Property variables
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lPMAuthorityLevel As Integer
	Private m_bError As Integer
	Private m_oBusiness As Object
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	'********************************
	
	Private m_oFormFields As iPMFormControl.FormFields

	Private m_oPolicyNumMaint As bSIRPolicyNumMaint.Business
	
	' Property Module Level Variables Declaration
	Private m_lRiskId As Integer
	Private m_lRiskNo As Integer
	Private m_sRiskDesc As String = ""
	Private m_sCoverNoteNo As String = ""
	Private m_dDateFrom As Date
	Private m_dDateTo As Date
	
	Private m_tTimeFrom As Date
	Private m_tTimeTo As Date
	Private m_lReasonForFailure As gPMConstants.PMEReturnCode
	
	Private m_lSourceID As Integer
	Private m_lProductID As Integer
    Private m_lAgentID As Integer
    Private m_stransactionType As String

	
	
	
	
	
	
	Public Property ReasonForFailure() As Integer
		Get
			Return m_lReasonForFailure
		End Get
		Set(ByVal Value As Integer)
			m_lReasonForFailure = Value
		End Set
	End Property
	
	
	Public Property SourceID() As Integer
		Get
			Return m_lSourceID
		End Get
		Set(ByVal Value As Integer)
			m_lSourceID = Value
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
	
	
	Public Property AgentID() As Integer
		Get
			Return m_lAgentID
		End Get
		Set(ByVal Value As Integer)
			m_lAgentID = Value
		End Set
	End Property
	
    Public Property TransactionType() As String
        Get
            Return m_stransactionType
        End Get
        Set(ByVal Value As String)
            m_stransactionType = Value
        End Set
    End Property

	
	Public Property RiskID() As Integer
		Get
			Return m_lRiskId
		End Get
		Set(ByVal Value As Integer)
			m_lRiskId = Value
		End Set
	End Property
	
	
	
	Public Property RiskNo() As Integer
		Get
			Return m_lRiskNo
		End Get
		Set(ByVal Value As Integer)
			m_lRiskNo = Value
		End Set
	End Property
	
	
	
	Public Property RiskDesc() As String
		Get
			Return m_sRiskDesc
		End Get
		Set(ByVal Value As String)
			m_sRiskDesc = Value
		End Set
	End Property
	
	
	Public Property CoverNoteNo() As String
		Get
			Return m_sCoverNoteNo
		End Get
		Set(ByVal Value As String)
			m_sCoverNoteNo = Value
		End Set
	End Property
	
	
	Public Property DateFrom() As Date
		Get
			Return m_dDateFrom
		End Get
		Set(ByVal Value As Date)
			m_dDateFrom = Value
		End Set
	End Property
	
	
	Public Property DateTo() As Date
		Get
			Return m_dDateTo
		End Get
		Set(ByVal Value As Date)
			m_dDateTo = Value
		End Set
	End Property
	
	
	
	
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

		m_dDateFrom = CDate(m_oFormFields.UnformatControl(ctlControl:=txtCNDateFrom))

		m_dDateTo = CDate(m_oFormFields.UnformatControl(ctlControl:=txtCNDateTo))
        If TransactionType = "NB" Then
            If m_dDateFrom < DateTime.Today Then
                MessageBox.Show("Cover Note From Date cannot be lesser than today's date.", "Cover Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCNDateFrom.Text = (DateTime.Now)
                m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDateFrom)
                txtCNDateFrom.Focus()
                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If m_dDateTo < DateTime.Today Then
                MessageBox.Show("Cover Note To Date cannot be lesser than today's date.", "Cover Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDateTo)
                txtCNDateTo.Focus()
                Exit Sub
            End If

            If CDate((m_dDateFrom) & " " & txtCNTimeFrom.Text) < CDate(DateTime.Now.ToString("dd-MMM-yyyy HH:MM")) Then
                MessageBox.Show("Cover Note From Time cannot be lesser than today's time.", "Cover Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDateTo)
                txtCNTimeFrom.Focus()
                Exit Sub
            End If

            If CDate((m_dDateFrom) & " " & txtCNTimeFrom.Text) > CDate((m_dDateTo) & " " & txtCNTimeTo.Text) Then
                MessageBox.Show("Cover Note To Date & Time cannot be lesser than Cover Note From Date & Time.", "Cover Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDateTo)
                txtCNDateTo.Focus()
                Exit Sub
            End If

        Else
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
        End If

        'm_dDateFrom = ToSafeDate(txtCNDateFrom.Text & " " & txtCNTimeFrom.Text)
        Me.Close()
    End Sub

    Private Function SetFieldValidation() As Integer
        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCNDateFrom, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCNDateTo, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
    End Function
    '''Public Property Get TimeFrom() As Date
    '''    TimeFrom = m_tTimeFrom
    '''End Property
    '''
    '''Public Property Let TimeFrom(ByVal Value As Date)
    '''    m_tTimeFrom = Value
    '''End Property
    '''
    '''Public Property Get TimeTo() As Date
    '''    TimeTo = m_tTimeTo
    '''End Property
    '''
    '''Public Property Let TimeTo(ByVal Value As Date)
    '''    m_tTimeTo = Value
    '''End Property

    ''''
    ''''Public Property Get sReasonForFailure() As String
    ''''    sReasonForFailure = m_sReasonForFailure
    ''''End Property
    ''''
    ''''Public Property Let sReasonForFailure(ByVal Value As String)
    ''''    m_sReasonForFailure = Value
    ''''End Property


    Private Sub frmCoverNote_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        m_oFormFields = New iPMFormControl.FormFields()


        m_lReturn = SetFieldValidation()

        m_lReturn = BusinessToData()


    End Sub

    Private Function BusinessToData() As Integer

        Dim lSourceID, lProductId, lAgentID As Integer
        m_lReturn = GenerateCoverNoteNumber(lSourceID, lProductId, lAgentID, m_sCoverNoteNo)
        m_lReturn = DataToInterface()

    End Function

    Private Function DataToInterface() As Integer

        m_lReturn = SetText(txtRiskNo, gPMFunctions.ToSafeString(m_lRiskNo))
        m_lReturn = SetText(txtRiskDescription, m_sRiskDesc)
        m_lReturn = SetText(txtCNNumber, m_sCoverNoteNo)
        m_lReturn = SetText(txtCNDateFrom, (DateFrom))


        m_lReturn = SetText(txtCNTimeFrom, DateTime.Now.ToString("HH:MM"))


        Dim bIsMidinightRenewal As Boolean = False
        Dim lCoverNoteDefaultPeriod As Integer

        Dim temp_m_oPolicyNumMaint As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumMaint, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oPolicyNumMaint = temp_m_oPolicyNumMaint


        m_lReturn = m_oPolicyNumMaint.GetMidnightRenewalOption(v_lProductId:=m_lProductID, r_bIsMidnightRenewal:=bIsMidinightRenewal)


        If bIsMidinightRenewal Then
            'Dim TempDate As Date
            'm_lReturn = SetText(txtCNTimeTo, IIf(DateTime.TryParse("23:59", TempDate), TempDate.ToString("HH:MM"), "23:59"))
            m_lReturn = SetText(txtCNTimeTo, "23:59")
            txtCNTimeTo.Enabled = False
        Else
            m_lReturn = SetText(txtCNTimeTo, DateTime.Now.ToString("HH:MM"))
            'm_lReturn = SetText(txtCNTimeTo, "23:59")
            txtCNTimeTo.Enabled = False
        End If

        Dim temp_m_oPolicyNumMaint2 As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumMaint2, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oPolicyNumMaint = temp_m_oPolicyNumMaint2


        m_lReturn = m_oPolicyNumMaint.GetCoverNoteDefaultPeriod(v_lProductId:=m_lProductID, r_lCoverNoteDefaultPeriod:=lCoverNoteDefaultPeriod)

        If lCoverNoteDefaultPeriod <> 0 Then
            m_dDateTo = m_dDateTo.AddDays(lCoverNoteDefaultPeriod)
        Else
            m_dDateTo = m_dDateTo
        End If

        m_lReturn = SetText(txtCNDateTo, gPMFunctions.ToSafeString(m_dDateTo))

    End Function
	
	
	
	Private Function GenerateCoverNoteNumber(ByVal v_lSourceID As Integer, ByVal v_lProductId As Integer, ByVal v_lAgentId As Integer, ByRef r_sGeneratedCoverNoteCode As String) As Integer
		
		Dim result As Integer = 0
		Dim sReasonForFailure As String = ""
		Const kMethodName As String = "GenerateCoverNoteNumber"
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Dim temp_m_oPolicyNumMaint As Object
		m_lReturn = g_oObjectManager.GetInstance(temp_m_oPolicyNumMaint, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
		m_oPolicyNumMaint = temp_m_oPolicyNumMaint
		
		
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
		End If
		

		m_lReturn = m_oPolicyNumMaint.GenerateCoverNoteNumber(v_lSourceID:=m_lSourceID, v_lProductId:=m_lProductID, v_lAgentId:=m_lAgentID, r_sGeneratedCoverNoteCode:=m_sCoverNoteNo, r_sFailureReason:=sReasonForFailure)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
		End If
		

		
		Catch ex As Exception
		
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		
		Finally
		


		
		

		End Try
		Return result
	End Function
	
	Private Function SetText(ByRef r_txtText As TextBox, ByRef sText As String) As Integer
		r_txtText.Text = sText
	End Function
	
	Private Sub frmCoverNote_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		If m_lStatus = gPMConstants.PMEReturnCode.PMOK Then
			m_lReasonForFailure = gPMConstants.PMEReturnCode.PMTrue
			Exit Sub
		End If
		
		m_lReturn = MessageBox.Show("Changes will be lost." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to cancel?", "Cover Note", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
		
		If m_lReturn = System.Windows.Forms.DialogResult.Cancel Then
			Cancel = 1
		Else
			m_lReasonForFailure = gPMConstants.PMEReturnCode.PMCancel
		End If
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub txtCNDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNDateFrom.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDateFrom)
	End Sub
	
	Private Sub txtCNDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNDateFrom.Leave
		'''If IsDate(txtCNDateFrom.Text) Then
		'''    txtCNDateFrom.Text = Format(txtCNDateFrom.Text, ACDateDispaly)
		'''Else
		'''    If (Len(Trim(txtCNDateFrom.Text)) = 0) Then
		'''        txtCNDateFrom.Text = Format(Now, ACDateDispaly)
		'''    Else
		'''        MsgBox "Enter a Valid Function"
		'''    End If
		'''
		'''End If
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCNDateFrom)
		
	End Sub
	
	Private Sub txtCNDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNDateTo.Enter
		m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCNDateTo)
	End Sub
	
	Private Sub txtCNDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCNDateTo.Leave
		m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCNDateTo)
		
	End Sub
	
	Private Sub txtCNTimeFrom_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles txtCNTimeFrom.Validating
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim m_lArr() As String = txtCNTimeFrom.Text.Split(":"c)
		If m_lArr.GetUpperBound(0) > 1 And m_lArr.GetUpperBound(0) <= 0 Then
			Cancel = True
		Else
			If gPMFunctions.ToSafeInteger(m_lArr(0)) > 23 Or gPMFunctions.ToSafeInteger(m_lArr(0)) < 0 Or gPMFunctions.ToSafeInteger(m_lArr(1)) > 59 Or gPMFunctions.ToSafeInteger(m_lArr(1)) < 0 Then
				Cancel = True
			End If
		End If
		If Cancel Then
			MessageBox.Show("Invalid Time Format.", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		End If
		eventArgs.Cancel = Cancel
	End Sub
End Class
