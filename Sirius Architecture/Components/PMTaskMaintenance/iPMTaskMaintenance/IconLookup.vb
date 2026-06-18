Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SListBar.ListBarControl
Friend Partial Class frmIconLookup
	Inherits System.Windows.Forms.Form
	Private Sub frmIconLookup_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Private m_bCancelled As Boolean
	Private m_fXPos As Integer
	Private m_fYPos As Integer
	Private m_lDisplayIcon As Integer
	
	Public Property Cancelled() As Boolean
		Get
			Return m_bCancelled
		End Get
		Set(ByVal Value As Boolean)
			m_bCancelled = Value
		End Set
	End Property
	
	Public Property DisplayIcon() As Integer
		Get
			Return m_lDisplayIcon
		End Get
		Set(ByVal Value As Integer)
			m_lDisplayIcon = Value
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		Me.Hide()
	End Sub
	
	Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
		Cancelled = False
		DisplayIcon = lvwIcons.FocusedItem.Index + 1
		Me.Hide()
	End Sub
	
	Private Sub lvwIcons_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwIcons.DoubleClick
		Cancelled = False
		DisplayIcon = lvwIcons.GetItemAt(m_fXPos, m_fYPos).Index + 1
		Me.Hide()
    End Sub
    'Developer guide no. 50
    Dim frmInterface As frmInterface

	Private Sub frmIconLookup_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		
		Cancelled = True
		'DAK200999 - load images into image list
		If frmInterface.albImageStore.LargeImageList.Images.Count > 1 Then
			imgTask.Images.Clear()
			imgTask.ImageSize = New Size(imgTask.ImageSize.Width, 32)
			imgTask.ImageSize = New Size(32, imgTask.ImageSize.Height)
			For Each oIcon As ListBar.SSImage In frmInterface.albImageStore.LargeImageList.Images

				imgTask.Images.Add(oIcon.Key, CType(oIcon.Picture, Image))
			Next oIcon
		End If

		'For Each oImage As Image In imgTask.Images
		For Each oImage As ListBar.SSImage In imgTask.Images


			lvwIcons.Items.Insert(oImage.Index - 1, oImage.Key, oImage.Key, oImage.Index)
		Next oImage

		lvwIcons.Items.Item(m_lDisplayIcon - 1).Selected = True

    End Sub
	
	Private Sub lvwIcons_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwIcons.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		m_fXPos = CInt(x)
		m_fYPos = CInt(y)
		
	End Sub
End Class