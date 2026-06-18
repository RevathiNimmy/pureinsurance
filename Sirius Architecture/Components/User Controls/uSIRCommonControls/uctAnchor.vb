Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctAnchor_NET.uctAnchor")> _
Public Partial Class uctAnchor
	Inherits System.Windows.Forms.UserControl

    'Modified by Archana Tokas on 4/20/2010 1:09:18 PM added to be checked later todolist
    Public ScaleWidth As String
    Public ScaleHeight As String

    ' Enumerators
	Public Enum ControlAnchorEnum
		AnchorTop = 1
		AnchorLeft = 2
		AnchorBottom = 4
		AnchorRight = 8
	End Enum
	
	Private m_cControls As Collection
	
	
	
	Public Sub Add(ByRef rControl As Control, ByVal vAnchor As ControlAnchorEnum)
		
		Dim oEntry As New AnchorEntry
		
		If TypeOf rControl Is Control Then
			' Set properties
			oEntry.Control = rControl
			oEntry.Anchor = vAnchor
			
			' Add to collection
			m_cControls.Add(oEntry, oEntry.Key)
		End If
	End Sub
	
	Public Sub Clear()
		' Recreate collection
		m_cControls = New Collection()
	End Sub
	
	Public Sub Resize_Renamed(ByVal oldX As Integer, ByVal oldY As Integer, ByVal newX As Integer, ByVal newY As Integer)
		
		
		' Get change
		Dim xChange As Integer = newX - oldX
		Dim yChange As Integer = newY - oldY
		
		' Walk our collection
		For	Each oEntry As AnchorEntry In m_cControls
			' Are we anchored to the top?
			If oEntry.Anchor And ControlAnchorEnum.AnchorTop Then
				' Yes, if we are also anchored to the bottom stretch
				If oEntry.Anchor And ControlAnchorEnum.AnchorBottom Then
					oEntry.Control.Height += VB6.TwipsToPixelsY(yChange)
				End If
			Else
				' We are not anchored, move the control
				oEntry.Control.Top += VB6.TwipsToPixelsY(yChange)
			End If
			
			' Are we anchored to the left?
			If oEntry.Anchor And ControlAnchorEnum.AnchorLeft Then
				' Yes, if we are also anchored to the right stretch
				If oEntry.Anchor And ControlAnchorEnum.AnchorRight Then
					oEntry.Control.Width += VB6.TwipsToPixelsX(xChange)
				End If
			Else
				' We are not anchored, move the control down
				oEntry.Control.Left += VB6.TwipsToPixelsX(xChange)
			End If
		Next oEntry
	End Sub
	
	
	
	Private Sub UserControl_Initialize()
		' Initialise the collection
		m_cControls = New Collection()
	End Sub
	
	' Usercontrol events
	Private Sub uctAnchor_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		' Fix height to label

		ScaleWidth = VB6.PixelsToTwipsX(lblCaption.Width)

		ScaleHeight = VB6.PixelsToTwipsY(lblCaption.Height)
	End Sub
End Class