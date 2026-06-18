Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Imports System.Windows.Forms
Friend Partial Class frmUpdateLog
	Inherits System.Windows.Forms.Form
	Private Sub frmUpdateLog_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_vUpdateLog( ,  ) As Object
	

	Private m_oBusiness As bPMNavXMEditor.Business
	
	Private Const UPDATE_ID As Integer = 0
	Private Const UPDATE_TIMESTAMP As Integer = 1
	Private Const UPDATE_VERSION As Integer = 2
	Private Const UPDATE_FILENAME As Integer = 3
	Private Const UPDATE_PCODE As Integer = 4
	Private Const UPDATE_PDESC As Integer = 5
	Private Const UPDATE_CCODE As Integer = 6
	Private Const UPDATE_CDESC As Integer = 7
	
	Private Const ACClass As String = "frmUpdateLog"
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public WriteOnly Property Business() As bPMNavXMEditor.Business
		Set(ByVal Value As bPMNavXMEditor.Business)
			m_oBusiness = Value
		End Set
	End Property
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Close()
		
	End Sub
	
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)
			
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
			

			m_lReturn = m_oBusiness.GetUpdateLog(m_vUpdateLog)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				Return result
			End If
			
			
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
			m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the details.
				Return result
			End If
			
			' Assign the details from the List data storage
			' to the interface.
			m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
			
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
        Dim oItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			lvwLog.View = View.Details
			
			lvwLog.Columns.Clear()
			lvwLog.Columns.Add("Date", "Date", CInt(VB6.TwipsToPixelsX(1500)))
			lvwLog.Columns.Add("Version", "Version", CInt(VB6.TwipsToPixelsX(900)))
			lvwLog.Columns.Add("Filename", "Filename", 94)
			lvwLog.Columns.Add("Pcode", "Parent code", CInt(VB6.TwipsToPixelsX(1200)))
			lvwLog.Columns.Add("Pdesc", "Parent desc", 94)
			lvwLog.Columns.Add("Ccode", "Child code", CInt(VB6.TwipsToPixelsX(1200)))
			lvwLog.Columns.Add("Cdesc", "Child desc", 94)
			
			lvwLog.Columns.Item(1).TextAlign = HorizontalAlignment.Right
			
			lvwLog.Items.Clear()
			
			For lLoop As Integer = m_vUpdateLog.GetLowerBound(1) To m_vUpdateLog.GetUpperBound(1)
				
				oItem = lvwLog.Items.Add("I" & CStr(m_vUpdateLog(UPDATE_ID, lLoop)), CDate(m_vUpdateLog(UPDATE_TIMESTAMP, lLoop)).ToString("dd/MM/yy HH:mm:ss"), "")
				
				ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vUpdateLog(UPDATE_VERSION, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 2).Text = CStr(m_vUpdateLog(UPDATE_FILENAME, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 3).Text = CStr(m_vUpdateLog(UPDATE_PCODE, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 4).Text = CStr(m_vUpdateLog(UPDATE_PDESC, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 5).Text = CStr(m_vUpdateLog(UPDATE_CCODE, lLoop))
				ListViewHelper.GetListViewSubItem(oItem, 6).Text = CStr(m_vUpdateLog(UPDATE_CDESC, lLoop))
				
			Next 
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private isInitializingComponent As Boolean
	Private Sub frmUpdateLog_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsX(Me.Width) < 6345 Then
			Me.Width = VB6.TwipsToPixelsX(6345)
		End If
		
		If VB6.PixelsToTwipsY(Me.Height) < 3225 Then
			Me.Height = VB6.TwipsToPixelsY(3225)
		End If
		
		cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOk.Height) - 120)
		cmdOk.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdOk.Width) - 120)
		
		tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240)
		tabMain.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(720)
		
		lvwLog.Width = tabMain.Width - VB6.TwipsToPixelsX(240)
		lvwLog.Height = tabMain.Height - VB6.TwipsToPixelsY(600)
		
	End Sub
End Class
