Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctAddress_NET.uctAddress")> _
Partial Public Class uctAddress
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

    Private Sub UserControl_Initialize()

        MinimumWidth = 3000
        MinimumHeight = 2130

    End Sub

    ''' <summary>
    ''' Resize the length of textboxes for address fields.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub uctAddress_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        If VB6.PixelsToTwipsY(MyBase.Height) < MinimumHeight Then
            MyBase.Height = VB6.TwipsToPixelsY(MinimumHeight)
        End If

        If VB6.PixelsToTwipsX(MyBase.Width) < MinimumWidth Then
            MyBase.Width = VB6.TwipsToPixelsX(MinimumWidth)
        End If

        txtAddress1.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress2.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress3.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress4.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress7.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress8.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress9.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress10.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress11.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress12.Width = MyBase.Width - VB6.TwipsToPixelsX(120)



        txtAddress7.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress8.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress9.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress10.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress11.Width = MyBase.Width - VB6.TwipsToPixelsX(120)
        txtAddress12.Width = MyBase.Width - VB6.TwipsToPixelsX(120)

        cmdAddress.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(MyBase.Width) - VB6.PixelsToTwipsX(cmdAddress.Width) - 120)

    End Sub
End Class