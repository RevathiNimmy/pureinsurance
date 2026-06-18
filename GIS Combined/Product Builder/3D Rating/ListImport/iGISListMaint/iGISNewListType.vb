Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmNewListType
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Module Name: frmNewListType
	'
	' Date: 28/06/2002
	'
	' Description:  This will hold new list type details
	'
	' Edit History:
	'   28/06/2002 SJP  - Tidied up after merge from Carole Nash
	' ***************************************************************** '
	
	' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    Private frmNewListType As frmNewListType
	
	
	' ***************************************************************** '
	'
	' Name: cmdCancel_Click()
	'
	' Description:  This will unload form
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdcancel_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	
	' ***************************************************************** '
	'
	' Name: cmdOK_Click()
	'
	' Description:  This will save new list type
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ISS1266 - stop codes being entered incorrectly JAS 12/11/02
	' PN17256 - prevent type mismatch error by referencing the text attribute rather than the txtbox control itself and relying on VB using the default property (RAW 02/12/2004)
	' ***************************************************************** '
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Dim sCode, sDescription As String
		
		Try 
			
			sCode = txtCode.Text
			sDescription = txtDescription.Text
			
			m_lReturn = m_oFormFields.CheckMandatoryControls()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'    'validation
			'    If sCode = "" Or sDescription = "" Then
			'        MsgBox "Both a code and description are required for a save", vbOKOnly, "List Management"
			'        Exit Sub
			'    End If
			
			
			' ISS1266
			If sCode.IndexOf(" "c) >= 0 Then
				MessageBox.Show("Codes must not contain spaces", "List Management", MessageBoxButtons.OK)
				Exit Sub
			End If
			
			' ISS1266
			If sCode.IndexOf("'"c) >= 0 Then
				MessageBox.Show("Codes must not contain apostrophes", "List Management", MessageBoxButtons.OK)
				Exit Sub
			End If
			
			
			'check to see if code or description are already in use

            If m_oBusiness.IsUnique(sCode, sDescription) Then

                'save it then

                m_lReturn = m_oBusiness.SaveNewListType(sCode, sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception("1, cmdOK_Click, Failed to save New list Type")
                    Exit Sub
                End If

                Me.Close()
            Else
                MessageBox.Show("The code or description is already in use please choose another", "List Maintenance", MessageBoxButtons.OK)
            End If

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save new list type", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


	Private Sub frmNewListType_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		Dim m_lErrorNumber As Integer
		
		m_oFormFields = New iPMFormControl.FormFields()
		
		' Validate fields using Forms Control
		m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			Exit Sub
		End If
		
	End Sub
	
	
	Private Function SetFieldValidation() As Integer
		
		
		Dim result As Integer = 0
		Try 
			
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
