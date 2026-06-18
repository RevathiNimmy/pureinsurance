Imports System.Drawing
Imports System.Windows.Forms


Public Class ToolStripSpringTextBox
    Inherits ToolStripTextBox
    Public Overrides Function GetPreferredSize(ByVal constrainingSize As Size) As Size

        If IsOnOverflow OrElse Owner.Orientation = Orientation.Vertical Then
            Return DefaultSize
        End If


        Dim width As Int32 = Owner.DisplayRectangle.Width


        If Owner.OverflowButton.Visible Then
            width = width - Owner.OverflowButton.Width - Owner.OverflowButton.Margin.Horizontal
        End If

        Dim springBoxCount As Int32 = 0

        For Each item As ToolStripItem In Owner.Items
            If item.IsOnOverflow Then
                Continue For
            End If

            If TypeOf item Is ToolStripSpringTextBox Then
                springBoxCount += 1
                width -= item.Margin.Horizontal
            Else
                width = width - item.Width - item.Margin.Horizontal
            End If
        Next

        If springBoxCount > 1 Then
            width /= springBoxCount
        End If

        If width < DefaultSize.Width Then
            width = DefaultSize.Width
        End If

        Dim size As Size = MyBase.GetPreferredSize(constrainingSize)
        size.Width = width
        Return size
    End Function
End Class

