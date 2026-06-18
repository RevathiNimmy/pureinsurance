Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 13/12/1996
	'
	' Description: Main View Form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_lErrorNumber As Integer
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)


    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
	
	Public Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the form.
			Return m_lErrorNumber
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the current form's error number.
			m_lErrorNumber = Value
			
		End Set
	End Property
	
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: PropertyToForm
    '
    ' Description: Updates all form details from the property members.
    '
    ' ***************************************************************** '
	Public Function PropertyToForm() As Integer
		
		Dim result As Integer = 0


		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Update the form details.
		
        lblPolicyMaster.Text = lblPolicyMaster.Text & CStr(DateTime.Today.Year)
		
		Return result
		
Err_PropertyToForm: 
		
		' Error Section.
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the form details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertyToForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	' PRIVATE Methods (End)
	
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms Initialise Event.
		
		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section
			'
			'ErrorNumber = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the form object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim lErrorValue As Integer
		
		' Sets up the forms defaults.
		
		Try 
			
			' Center the form.
			iPMFunc.CenterForm(Me)
			
			' Display the form details
			lErrorValue = PropertyToForm()
			
			' Check for errors.
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	' PRIVATE Events (End)
End Class
