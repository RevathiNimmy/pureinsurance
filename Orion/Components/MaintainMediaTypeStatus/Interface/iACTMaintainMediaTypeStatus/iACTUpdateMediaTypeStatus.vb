Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmUpdateMediaTypeStatus
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmUpdateMediaTypeStatus"
	Private m_oGeneral As iACTMaintainMediaTypeStatus.General
	Private m_oFormFields As iPMFormControl.FormFields
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	Private m_oBusiness As Object
	Private m_lReturn As Integer
	Private m_oPMUser As Object
    Private Const vbFormCode As Integer = 0
	Public Function FillProperties() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "FillProperties"
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		cboMediaTypeStatus.FirstItem = "(Any)"
		cboMediaTypeStatus.ItemId = 0
		txtUpdateDate.Text = StringsHelper.Format(DateTime.Today, ACDateDispaly)
		txtCommments.Text = ""
		
		
		Catch ex As Exception
		
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="FillProperties", r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)
		
		Finally
'		Return result
'		Resume 
		
'		Return result
		End Try
		Return result
	End Function
	
	Private Sub cboMediaTypeStatus_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMediaTypeStatus.Click
		cmdOK.Enabled = Not (cboMediaTypeStatus.ItemId < 1)
	End Sub
    'developer guide no. 191
    'Private Sub cboMediaTypeStatus_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboMediaTypeStatus.KeyDown
    '	Dim KeyCode As Integer = eventArgs.KeyCode
    '	Dim Shift As Integer = eventArgs.KeyData \ &H10000
    '	cmdOK.Enabled = Not (cboMediaTypeStatus.ItemId < 1)
    'End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		

        Const kMethodName As String = "cmdCancel_Click"
        Try


            If MessageBox.Show("Do you really wish to cancel?", "Maintain Media type Status", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                m_lStatus = gPMConstants.PMEReturnCode.PMcancel
                'developer guie no. 50.
                objFrmInterface.Status = gPMConstants.PMEReturnCode.PMCancel
                Me.Hide()
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
        Const kMethodName As String = "cmdOK_Click"
        Try


            'developer guie no. 50
            With objFrmInterface
                .MediaTypeStatus = cboMediaTypeStatus.ItemCaption
                .MediaTypeStatusId = cboMediaTypeStatus.ItemId
                .Comments = txtCommments.Text.Trim()
                .Status = gPMConstants.PMEReturnCode.PMOK
                If txtUpdateDate.Text = "" Then
                    m_lReturn = MessageBox.Show("This field is missing. You must enter  data in this field", "Mandatory Field-Update Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtUpdateDate.Focus()
                    Exit Sub
                Else

                    .UpdateDate = CDate(StringsHelper.Format(txtUpdateDate.Text.Trim(), ACDateDispaly))
                End If
            End With
            Me.Hide()




        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub
	
	Private Sub Form_Initialize_Renamed()
		

        Const kMethodName As String = "Form_Initialize"
        Try

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            m_oGeneral = New iACTMaintainMediaTypeStatus.General()

            m_oFormFields = New iPMFormControl.FormFields()

            m_lStatus = gPMConstants.PMEReturnCode.PMcancel

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception


            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
	End Sub
	
	

	Private Sub frmUpdateMediaTypeStatus_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		

        Const kMethodName As String = "Form_Load()"
        Try



            m_lReturn = FillProperties()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " FillProperties method failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If

            cmdOK.Enabled = False



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub
	
	Private Sub frmUpdateMediaTypeStatus_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Const kMethodName As String = "Form_QueryUnload()"
        Dim iMsgResult As Integer
        Try

            'developer guie no. 50.
            'frmInterface.Status = gPMConstants.PMEReturnCode.PMcancel
            If UnloadMode <> vbFormCode Then
                objFrmInterface.Status = gPMConstants.PMEReturnCode.PMCancel
                Me.Hide()
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally


        End Try
    End Sub
	
	Private Sub txtUpdateDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUpdateDate.Leave
		If (Not Information.IsDate(txtUpdateDate.Text)) And txtUpdateDate.Text.Trim().Length >= 1 Then
			txtUpdateDate.Text = StringsHelper.Format(DateTime.Now, ACDateDispaly)
		Else
			txtUpdateDate.Text = StringsHelper.Format(txtUpdateDate.Text, ACDateDispaly)
		End If
	End Sub
	
	Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"


		Try
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtUpdateDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		Catch ex As Exception
		
		result = gPMConstants.PMEReturnCode.PMError
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

		Finally
	
		End Try
		Return result
	End Function
    'developer guide no.191
    Private Sub cboMediaTypeStatus_KeyDown(ByVal Sender As System.Object, ByVal e As PMLookupControl.cboPMLookup.KeyDownEventArgs) Handles cboMediaTypeStatus.KeyDown
        Dim KeyCode As Integer = e.KeyCode
        'developer guide no.192
        Dim Shift As Integer = e.Shift \ &H10000
        cmdOK.Enabled = Not (cboMediaTypeStatus.ItemId < 1)
    End Sub
End Class
