Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Sudhanshu Behera on 6/24/2010 11:27:16 AM refer developer guide no. 129
Imports SharedFiles
Friend Partial Class frmLossScheduleType
	Inherits System.Windows.Forms.Form
	Private m_lStatus As Integer
	
	

	'Private Sub Status(ByVal Value As Integer)
		' Set the interface exit status.
		'm_lStatus = Value
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			' Return the interface exit status.
			Return m_lStatus
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		Me.Hide()
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		FileSystem.FileClose()
	End Sub
	

	Private Sub frmLossScheduleType_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		SetInterfaceDefaults()
		FileSystem.FileClose()
	End Sub
	
	Private Sub SetInterfaceDefaults()
		Dim m_lReturn As Integer = DisplayCaptions()
	End Sub
	
	
	
	Private Function DisplayCaptions() As Integer
		Dim result As Integer = 0
		Dim ACClass As Object
		' ***************************************************************** '
		' Name: DisplayCaptions
		'
		' Description: Display all language specific captions.
		'
		' History : 17092002 CMG/PB - Created
		'
		' ***************************************************************** '
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all language specific captions.
			
			'Form

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACLossScheduleTypeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Tabs

			SSTabHelper.SetTabCaption(tabLossScheduleType, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACLossTab1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Buttons

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			'Form Controls

			lblLossScheduleType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACLossScheduleType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
End Class
