Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctClaimPayment_NET.uctClaimPayment")> _
Public Partial Class uctClaimPayment
	Inherits System.Windows.Forms.UserControl
	
	Private m_lMinimumWidth As Integer
	Private m_lMinimumHeight As Integer
	
	<Browsable(True)> _
	Public Property MinimumWidth() As Integer
		Get
			Return m_lMinimumWidth
		End Get
		Set(ByVal Value As Integer)
			m_lMinimumWidth = Value
		End Set
	End Property
	
	<Browsable(True)> _
	Public Property MinimumHeight() As Integer
		Get
			Return m_lMinimumHeight
		End Get
		Set(ByVal Value As Integer)
			m_lMinimumHeight = Value
		End Set
	End Property
	
    <Browsable(False)> _
    Public Shadows Property Height() As Integer
        'developer guide no. Added the get block
        Get
            Return m_lMinimumHeight
        End Get
        Set(ByVal Value As Integer)
            If Value < m_lMinimumHeight Then
                'Modified the code
                'Me.Height = m_lMinimumHeight
                MyBase.Height = m_lMinimumHeight
            End If
        End Set
    End Property
	
    <Browsable(False)> _
    Public Shadows Property Width() As Integer
        'developer guide no. Added the get block
        Get
            Return m_lMinimumWidth
        End Get
        Set(ByVal Value As Integer)
            If Value < m_lMinimumWidth Then
                'Modified the code
                'Me.Width = m_lMinimumWidth
                MyBase.Width = m_lMinimumWidth
            End If
        End Set
    End Property
	
	Private Sub UserControl_Initialize()

		MinimumWidth = 13050
		MinimumHeight = 6630
		
        'developer guide no. 
        Dim lNewWidth As Integer = MyBase.Width
        Dim lNewHeight As Integer = MyBase.Height
		
		If lNewWidth < m_lMinimumWidth Then
			MyBase.Width = VB6.TwipsToPixelsX(m_lMinimumWidth)
		End If
		
		If lNewHeight < m_lMinimumHeight Then
			MyBase.Height = VB6.TwipsToPixelsY(m_lMinimumHeight)
		End If
		
		
		'    sngWidth = lvwPayment.Width / 7
		'
		'    ' insert the column headers
		'    lvwPayment.ColumnHeaders.Add Index:=1, Text:="               ", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(1).Alignment = lvwColumnLeft
		'    lvwPayment.ColumnHeaders.Add Index:=2, Text:="Initial Reserve", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(2).Alignment = lvwColumnRight
		'
		'    'TN20010904 - start - add revision amount column
		'    lvwPayment.ColumnHeaders.Add Index:=3, Text:="Revision Amount", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(3).Alignment = lvwColumnRight
		'
		'    lvwPayment.ColumnHeaders.Add Index:=4, Text:="Paid to Date", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(4).Alignment = lvwColumnRight
		'    lvwPayment.ColumnHeaders.Add Index:=5, Text:="This Payment", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(5).Alignment = lvwColumnRight
		'    lvwPayment.ColumnHeaders.Add Index:=6, Text:="Current Reserve", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(6).Alignment = lvwColumnRight
		'    lvwPayment.ColumnHeaders.Add Index:=7, Text:="Incurred", Width:=sngWidth
		'    lvwPayment.ColumnHeaders(7).Alignment = lvwColumnRight
		
	End Sub
	
	Private Sub uctClaimPayment_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		'    Dim sngWidth As Single
		'    Dim iCol As Integer
		'
		If VB6.PixelsToTwipsY(MyBase.Height) < MinimumHeight Then
			MyBase.Height = VB6.TwipsToPixelsY(MinimumHeight)
		End If
		
		If VB6.PixelsToTwipsX(MyBase.Width) < MinimumWidth Then
			MyBase.Width = VB6.TwipsToPixelsX(MinimumWidth)
		End If
		'
		'    lvwPayment.Height = UserControl.Height - 100
		'    lvwPayment.Width = UserControl.Width - 1400
		'
		'    cmdEdit.Left = UserControl.Width - 1200
		'    cmdEdit.Top = lvwPayment.Top
		'
		'    MinimumWidth = 2000
		'    MinimumHeight = 2000
		'
		'    sngWidth = (lvwPayment.Width - 100) / 7
		'
		'    For iCol = 1 To lvwPayment.ColumnHeaders.Count
		'        lvwPayment.ColumnHeaders(iCol).Width = sngWidth
		'    Next iCol
	End Sub
End Class