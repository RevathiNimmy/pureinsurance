
''' <summary>
''' Gridview control enhanced to allow rows to be selectable
''' A new property is exposed "SelectedRow" which returns the ID of the row which has been clicked on
''' </summary>
''' <remarks></remarks>
Public Class GridView : Inherits System.Web.UI.WebControls.GridView

    'properties needed:

    'SelectedRowCssClass, HoverRowCssClass, more?

    Public Property SelectedRowCssClass() As String
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property



    Private Sub GridView_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding
        Dim tType As Type = Me.DataSource.GetType


    End Sub


    Private Sub GridView_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowDataBound
        For Each cCell As System.Web.UI.WebControls.TableCell In e.Row.Cells

        Next

    End Sub
End Class
