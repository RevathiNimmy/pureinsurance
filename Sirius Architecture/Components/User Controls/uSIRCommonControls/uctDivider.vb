Option Strict Off
Option Explicit On
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("uctDivider_NET.uctDivider")> _
Public Partial Class uctDivider
	Inherits System.Windows.Forms.UserControl
	
	
	
	<Browsable(True)> _
	Public Property Caption() As String
		Get
			Return fraDivider.Text
		End Get
		Set(ByVal Value As String)
			fraDivider.Text = Value
		End Set
	End Property
	
	
	

	Private Sub UserControl_InitProperties()
		fraDivider.Text = MyBase.Name
	End Sub
	


    'Modified by Archana Tokas on 4/20/2010 1:09:18 PM refer developer guide no. 1
    'Private Sub UserControl_ReadProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        fraDivider.Text = CStr(PropBag.ReadProperty("Caption", MyBase.Name))
    End Sub

    Private Sub uctDivider_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            fraDivider.SetBounds(VB6.TwipsToPixelsX(-30), VB6.TwipsToPixelsY(15), ClientRectangle.Width + VB6.TwipsToPixelsX(60), ClientRectangle.Height + VB6.TwipsToPixelsY(30))

        Catch
        End Try
    End Sub



    'Modified by Archana Tokas on 4/20/2010 1:09:18 PM refer developer guide no. 1
    'Private Sub UserControl_WriteProperties(ByRef PropBag As PropertyBag)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("Caption", fraDivider.Text, MyBase.Name)
    End Sub
End Class