Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmTasks
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lNavXMProcessID As Integer
	
	Private m_vTaskMaps( ,  ) As Object
	
	Private m_oBusiness As Object
	
	Private Const ACClass As String = "frmTasks"
	
	' m_vTaskMaps array constants
	Private Const TASK_MAP_TASK_ID As Integer = 0
	Private Const TASK_MAP_TASK_CODE As Integer = 1
	Private Const TASK_MAP_TASK_DESC As Integer = 2
	Private Const TASK_MAP_XML_PROC_ID As Integer = 3
	Private Const TASK_MAP_XML_PROC_FILENAME As Integer = 4
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property NavXMProcessID() As Integer
		Get
			Return m_lNavXMProcessID
		End Get
	End Property
	
	Public WriteOnly Property Business() As Object
		Set(ByVal Value As Object)
			m_oBusiness = Value
		End Set
	End Property
	
	
	' ***************************************************************** '
	' Name: Initialise (Standard Method)
	'
	' Description: Initialise the form
	'
	' ***************************************************************** '
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Set the interface status to cancelled. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialse failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Load (Standard Method)
	'
	' Description: Load the form details
	'
	' ***************************************************************** '
	Public Function Load_Renamed() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to busy.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Gets the interface details to be displayed.
			m_lReturn = GetInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
				Return result
			End If
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Load failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ShowForm (Standard Method)
	'
	' Description: Show the form using the display state passed
	'
	' ***************************************************************** '
	Public Function ShowForm(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Show the the form, allow user input etc.
			VB6.ShowForm(Me, lDisplayState)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowForm failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Private Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Get the details from the business object.
			

			m_lReturn = m_oBusiness.GetTaskMaps(m_vTaskMaps)
			
			' Check for other errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get details.
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: GetInterfaceDetails
	'
	' Description: get the roadmap info to display on listview
	'
	' History: RDC 28032003 created
	' ***************************************************************** '
	Private Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Get the interface details from the
			' business object.
			m_lReturn = GetBusiness()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the details.
				Return result
			End If
			
			' Assign the details from the List data storage
			' to the interface.
			m_lReturn = DataToInterface()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInterfaceDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DataToInterface
	'
	' Description: display the roadmap info on the listview
	'
	' History: RDC 28032003 created
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Dim lWidth As Integer
		Dim oItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			lvwTasks.View = View.Details
			
			lWidth = CInt((VB6.PixelsToTwipsX(lvwTasks.Width) / 3) - 45)
			
			lvwTasks.Columns.Clear()
			lvwTasks.Columns.Add("TaskCode", "Task Code", CInt(VB6.TwipsToPixelsX(lWidth)))
			lvwTasks.Columns.Add("TaskDesc", "Task Description", CInt(VB6.TwipsToPixelsX(lWidth)))
			lvwTasks.Columns.Add("FileName", "XML Doc Filename", CInt(VB6.TwipsToPixelsX(lWidth)))
			
			lvwTasks.Items.Clear()
			
			For lLoop As Integer = m_vTaskMaps.GetLowerBound(1) To m_vTaskMaps.GetUpperBound(1)
				
				oItem = lvwTasks.Items.Add("T" & CStr(m_vTaskMaps(TASK_MAP_XML_PROC_ID, lLoop)), CStr(m_vTaskMaps(TASK_MAP_TASK_CODE, lLoop)), "")
				ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vTaskMaps(TASK_MAP_TASK_DESC, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 2).Text = CStr(m_vTaskMaps(TASK_MAP_XML_PROC_FILENAME, lLoop))
				
			Next 
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	
	Private Sub frmTasks_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_lNavXMProcessID = ID_NO_VALUE And m_lStatus = gPMConstants.PMEReturnCode.PMOk Then
			' nothing selected when Ok clicked
			Cancel = True
			Exit Sub
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private Sub lvwTasks_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTasks.DoubleClick
		
		If m_lNavXMProcessID <> ID_NO_VALUE Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Close()
		End If
		
	End Sub
	
	Private Sub lvwTasks_ItemClick(ByVal Item As ListViewItem)
		
		m_lNavXMProcessID = CInt(Mid(Item.Name, 2))
		
	End Sub
End Class
