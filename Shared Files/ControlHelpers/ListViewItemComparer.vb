Option Strict Off
Option Explicit On
Imports System.Collections
Imports System.Windows.Forms
Public NotInheritable Class ListViewItemComparer 
    Implements IComparer
    Private col As Integer
    Private order As SortOrder
    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub
    Public Sub New(ByVal column As Integer, ByVal order As SortOrder)
        col = column
        Me.order = order
    End Sub
    Private Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnValue As Integer
        Try
            Dim firstDate As System.DateTime
            Dim secondDate As System.DateTime
            Dim firstDec As Decimal
            Dim secondDec As Decimal
            Dim firstInt As Integer
            Dim secondInt As Integer

            If CType(x, ListViewItem).SubItems.Count <= col Or CType(y, ListViewItem).SubItems.Count <= col Then
                Return returnValue
            End If
            If (CType(x, ListViewItem).SubItems.Count >= col) AndAlso (CType(y, ListViewItem).SubItems.Count >= col) Then
                'Dim firstDate As System.DateTime = DateTime.Parse(CType(x, ListViewItem).SubItems(col).Text)
                'Dim secondDate As System.DateTime = DateTime.Parse(CType(y, ListViewItem).SubItems(col).Text)
                If DateTime.TryParse(CType(x, ListViewItem).SubItems(col).Text.Trim, firstDate) _
                   And DateTime.TryParse(CType(y, ListViewItem).SubItems(col).Text.Trim, secondDate) Then
                    returnValue = DateTime.Compare(firstDate, secondDate)
                ElseIf Decimal.TryParse(CType(x, ListViewItem).SubItems(col).Text.Trim, firstDec) And Decimal.TryParse(CType(y, ListViewItem).SubItems(col).Text.Trim, secondDec) Then
                    returnValue = Decimal.Compare(firstDec, secondDec)
                ElseIf Integer.TryParse(CType(x, ListViewItem).SubItems(col).Text.Trim, firstInt) And Integer.TryParse(CType(y, ListViewItem).SubItems(col).Text.Trim, secondInt) Then
                    If firstInt = secondInt Then
                        returnValue = 0
                    ElseIf firstInt < secondInt Then
                        returnValue = -1
                    Else
                        returnValue = 1
                    End If
                Else
                    returnValue = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
                End If

            End If
        Catch ex As Exception
            If (CType(x, ListViewItem).SubItems.Count >= col) AndAlso (CType(y, ListViewItem).SubItems.Count >= col) Then
                returnValue = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
            End If
        End Try
        If order = SortOrder.Descending Then
            returnValue *= -1
        End If
        Return returnValue
    End Function
End Class
