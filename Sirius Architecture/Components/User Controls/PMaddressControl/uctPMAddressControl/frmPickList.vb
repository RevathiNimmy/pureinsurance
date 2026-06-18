Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmPickList
	Inherits System.Windows.Forms.Form
	' #############################################################################################
	' PMAddressControl.frmPickList
	' History:
	' RDC 02012003 added third image to imgList for QAS Names support
	' #############################################################################################
	
	
	Private m_lSelectedNodeID As Integer
	
	'Form Resize Constants
	Private Const ACSepWidth As Integer = 130
	Private Const ACMinHeight As Integer = 3855
	Private Const ACMinWidth As Integer = 7350
	Public ReadOnly Property SelectedNodeID() As Integer
		Get
			
			Return m_lSelectedNodeID
			
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lSelectedNodeID = 0
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Check if none selected
		If trvPickList.SelectedNode Is Nothing Then
			m_lSelectedNodeID = 0
		Else
			' Get Node from Selected Item Key value (='N?')
            m_lSelectedNodeID = trvPickList.SelectedNode.Index + 1
		End If
		
		Me.Hide()
		
	End Sub
	

	Private Sub frmPickList_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Centralise Form
		Left = (Screen.PrimaryScreen.Bounds.Width - Width) / 2
		Top = (Screen.PrimaryScreen.Bounds.Height - Height) / 2
		
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub frmPickList_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		Static bResizing As Boolean
		
		Try 
			
			If bResizing Then
				Exit Sub
			End If
			
			bResizing = True
			
			'Make sure that a Minimum Width is maintained
			If VB6.PixelsToTwipsX(Me.Width) < ACMinWidth Then
				Me.Width = VB6.TwipsToPixelsX(ACMinWidth)
			End If
			
			'Make sure that a Minimum Height is maintained
			If VB6.PixelsToTwipsY(Me.Height) < ACMinHeight Then
				Me.Height = VB6.TwipsToPixelsY(ACMinHeight)
			End If
			
			SSFrame1.Width = Me.ClientRectangle.Width - SSFrame1.Left * 2
			SSFrame1.Height = Me.ClientRectangle.Height - (VB6.TwipsToPixelsY(VB6.PixelsToTwipsX(SSFrame1.Left) * 2) + SSFrame1.Top + cmdOK.Height)
			
			trvPickList.Width = SSFrame1.Width - trvPickList.Left * 2
			trvPickList.Height = SSFrame1.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(trvPickList.Top) * 1.8)
			
			cmdOK.Left = Me.ClientRectangle.Width - (2 * SSFrame1.Left + cmdOK.Width + cmdCancel.Width)
			cmdOK.Top = Me.ClientRectangle.Height - (SSFrame1.Left + cmdOK.Height)
			cmdCancel.Left = Me.ClientRectangle.Width - (SSFrame1.Left + cmdCancel.Width)
			cmdCancel.Top = Me.ClientRectangle.Height - (SSFrame1.Left + cmdCancel.Height)
			
			bResizing = False
		
		Catch 
		End Try
		
		
		
	End Sub
	
	
	
	
	
	Private Sub trvPickList_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles trvPickList.DoubleClick
		
		cmdOK_Click(cmdOK, New EventArgs())
		
	End Sub
End Class