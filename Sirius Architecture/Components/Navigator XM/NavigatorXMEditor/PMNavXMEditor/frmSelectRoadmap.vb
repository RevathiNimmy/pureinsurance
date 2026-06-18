Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmSelectRoadmap
	Inherits System.Windows.Forms.Form
	Private Sub frmSelectRoadmap_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	Private m_lNavXMProcessID As Integer
	Private m_lNavXMProcessVersionID As Integer
	
	Private m_vPrimary As Object
	Private m_vSecondary( ,  ) As Object
	

	Private m_oBusiness As bPMNavXMEditor.Business
	
	Private Const ACClass As String = "frmSelectRoadmap"
	
	' array constants
	Private Const PRIMARY_STEP_TASK_ID As Integer = 0
	Private Const PRIMARY_STEP_CODE As Integer = 1
	Private Const PRIMARY_STEP_DESC As Integer = 2
	Private Const PRIMARY_STEP_RM_ID As Integer = 3
	
	Private Const SECONDARY_RM_ID As Integer = 0
	Private Const SECONDARY_RM_PROCESS_ID As Integer = 1
	Private Const SECONDARY_RM_TIMESTAMP As Integer = 2
	Private Const SECONDARY_RM_VERSION As Integer = 3
	Private Const SECONDARY_RM_DESC As Integer = 4
	Private Const SECONDARY_RM_CODE As Integer = 5
	
	
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public ReadOnly Property ProcessID() As Integer
		Get
			Return m_lNavXMProcessID
		End Get
	End Property
	
	Public ReadOnly Property ProcessVersionID() As Integer
		Get
			Return m_lNavXMProcessVersionID
		End Get
	End Property
	
	Public WriteOnly Property Business() As bPMNavXMEditor.Business
		Set(ByVal Value As bPMNavXMEditor.Business)
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
			

            m_lReturn = m_oBusiness.GetTaskMaps(m_vPrimary)

            ' Check for other errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get primary roadmaps from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If


			m_lReturn = m_oBusiness.GetTaskMapVersions(m_vSecondary)
			
			' Check for other errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
				' Failed to get details.
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get secondary roadmaps from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
				
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
		Dim oNode As TreeNode
		Dim sTimestamp As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_lNavXMProcessID = ID_NO_VALUE
			m_lNavXMProcessVersionID = ID_NO_VALUE
			
			tvwRoadmaps.Nodes.Clear()
			
			oNode = tvwRoadmaps.Nodes.Add("ROOT", "Roadmaps", "Roadmap")
			
			oNode.Expand()
			
			' add the core roadmaps

            For lLoop As Integer = m_vPrimary.GetLowerBound(1) To m_vPrimary.GetUpperBound(1)


                oNode = tvwRoadmaps.Nodes.Find("ROOT", True)(0).Nodes.Add("R" & CStr(m_vPrimary(PRIMARY_STEP_RM_ID, lLoop)), CStr(m_vPrimary(PRIMARY_STEP_CODE, lLoop)).Trim() & ": " & CStr(m_vPrimary(PRIMARY_STEP_DESC, lLoop)), "Roadmap")
            Next
			
			' add the custom copies
			If Information.IsArray(m_vSecondary) Then
				For lLoop As Integer = m_vSecondary.GetLowerBound(1) To m_vSecondary.GetUpperBound(1)
					sTimestamp = CDate(m_vSecondary(SECONDARY_RM_TIMESTAMP, lLoop)).ToString("dd MMM yyyy HH:mm")
					
					oNode = tvwRoadmaps.Nodes.Find("R" & CStr(m_vSecondary(SECONDARY_RM_PROCESS_ID, lLoop)), True)(0).Nodes.Add("S" & CStr(m_vSecondary(SECONDARY_RM_ID, lLoop)), sTimestamp &  _
					        " - " & CStr(m_vSecondary(SECONDARY_RM_CODE, lLoop)).Trim() &  _
					        " - " & CStr(m_vSecondary(SECONDARY_RM_DESC, lLoop)).Trim(), "Roadmap")
				Next 
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
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
	
	Private Sub frmSelectRoadmap_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		If m_lNavXMProcessID = ID_NO_VALUE And m_lStatus = gPMConstants.PMEReturnCode.PMOk Then
			' nothing selected when Ok clicked
			Cancel = True
			Exit Sub
		End If
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmSelectRoadmap_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Const FORM_HEIGHT As Integer = 5535
		Const FORM_WIDTH As Integer = 4650
		
		If Me.WindowState = FormWindowState.Minimized Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsY(Me.Height) < FORM_HEIGHT Then
			Me.Height = VB6.TwipsToPixelsY(FORM_HEIGHT)
		End If
		
		If VB6.PixelsToTwipsX(Me.Width) < FORM_WIDTH Then
			Me.Width = VB6.TwipsToPixelsX(FORM_WIDTH)
		End If
		
		cmdCancel.Left = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(120) - cmdCancel.Width
		cmdCancel.Top = Me.ClientRectangle.Height - VB6.TwipsToPixelsY(120) - cmdCancel.Height
		
		cmdOk.Left = cmdCancel.Left - VB6.TwipsToPixelsX(60) - cmdOk.Width
		cmdOk.Top = cmdCancel.Top
		
		tabMain.Width = Me.ClientRectangle.Width - VB6.TwipsToPixelsX(240)
		tabMain.Height = cmdCancel.Top - VB6.TwipsToPixelsY(240)
		
		tvwRoadmaps.Width = tabMain.Width - VB6.TwipsToPixelsX(240)
		tvwRoadmaps.Height = tabMain.Height - VB6.TwipsToPixelsY(540)
		
		
	End Sub
	
	Private Sub tvwRoadmaps_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwRoadmaps.AfterSelect
		Dim Node As TreeNode = eventArgs.Node
		
		If Node.Name <> "ROOT" Then
			If Node.Parent.Text = "Roadmaps" Then
				' it's a core roadmap
				m_lNavXMProcessID = CInt(Mid(Node.Name, 2))
				m_lNavXMProcessVersionID = ID_NO_VALUE
			Else
				' it's a custom copy
				m_lNavXMProcessID = CInt(Mid(Node.Parent.Name, 2))
				m_lNavXMProcessVersionID = CInt(Mid(Node.Name, 2))
			End If
		End If
	End Sub
End Class
