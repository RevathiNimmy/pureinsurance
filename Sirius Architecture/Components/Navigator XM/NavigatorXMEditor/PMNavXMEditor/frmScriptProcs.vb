Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.ExceptionServices
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmScriptProcs
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_sProcedureName As String = ""
	Private m_sScriptFilename As String = ""
	
	Private Const ACClass As String = "frmScriptProcs"
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property ProcedureName() As String
		Get
			Return m_sProcedureName
		End Get
	End Property
	
	Public WriteOnly Property ScriptFilename() As String
		Set(ByVal Value As String)
			m_sScriptFilename = Value
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
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Showform", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetInterfaceDetails
	'
	' Description:
	'
	' History: RDC 28032003 created
	' ***************************************************************** '
	Private Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
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
	' Description:
	'
	' History: RDC 28032003 created
	' ***************************************************************** '
	<HandleProcessCorruptedStateExceptions>
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Dim lLoop, lWidth As Integer
		Dim oItem As ListViewItem
		
		Dim lFH As Integer
		Dim sCode As String = ""
		
		Dim oProc As MSScriptControl.IScriptProcedure
		Dim oProcs As MSScriptControl.IScriptProcedureCollection
		Dim oMSScriptControl As MSScriptControl.ScriptControl
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			lvwProcs.View = View.Details
			
			lWidth = CInt(VB6.PixelsToTwipsX(lvwProcs.Width) - 60)
			
			lvwProcs.Columns.Clear()
			lvwProcs.Columns.Add("Proc", "Procedure name", CInt(VB6.TwipsToPixelsX(lWidth)))
			
			lvwProcs.Items.Clear()
			
			oMSScriptControl = New MSScriptControl.ScriptControl()
			
			lFH = FileSystem.FreeFile()
			
			FileSystem.FileOpen(lFH, m_sScriptFilename, OpenMode.Input)
			
			sCode = FileSystem.InputString(lFH, FileSystem.LOF(lFH)).Trim()
			
			FileSystem.FileClose(lFH)
			
			If sCode = "" Then
				MessageBox.Show("Code file is empty", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				
				Return result
			End If
			
			txtScript.Text = sCode
			
			oMSScriptControl.Language = "VBScript"
			
			oMSScriptControl.AddCode(sCode)
			
			oProcs = oMSScriptControl.Procedures
			
			oMSScriptControl = Nothing
			
			lLoop = 1
			
			For	Each oProc2 As MSScriptControl.IScriptProcedure In oProcs
				oProc = oProc2
				
				If Not oProc.HasReturnValue Then
					oItem = lvwProcs.Items.Add("T" & lLoop, oProc.Name, "")
					
					lLoop += 1
				End If
			Next oProc2
			
			oProc = Nothing
			oProcs = Nothing
			
			
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
	
	Private Sub frmScriptProcs_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_sProcedureName = "" And m_lStatus = gPMConstants.PMEReturnCode.PMOk Then
			' nothing selected when Ok clicked
			Cancel = True
			Exit Sub
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmScriptProcs_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdCancel.Height) - 120)
		cmdOk.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOk.Height) - 120)
		
		cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdCancel.Width) - 120)
		cmdOk.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdCancel.Left) - VB6.PixelsToTwipsX(cmdOk.Width) - 60)
		
		tabMain.Height = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(720)
		tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240)
		
		txtScript.Height = tabMain.Height - VB6.TwipsToPixelsY(2040)
		txtScript.Width = tabMain.Width - VB6.TwipsToPixelsX(240)
		
		lvwProcs.Width = txtScript.Width
		lvwProcs.Columns.Item(0).Width = CInt(lvwProcs.Width - VB6.TwipsToPixelsX(75))
		
	End Sub
	
	Private Sub lvwProcs_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwProcs.DoubleClick
		
		If m_sProcedureName <> "" Then
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Close()
		End If
		
	End Sub
	
	Private Sub lvwProcs_ItemClick(ByVal Item As ListViewItem)
		
		m_sProcedureName = Item.Text
		
	End Sub
	
	
	Private Sub txtScript_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtScript.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		KeyCode = 0
	End Sub
	
	Private Sub txtScript_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtScript.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		KeyAscii = 0
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
End Class
