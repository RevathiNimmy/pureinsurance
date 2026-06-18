Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class FixedLogin
    Inherits System.Web.UI.WebControls.Login

    'Gets rid of those wonderful tables from the control

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        Dim div As WebControl = New WebControl(HtmlTextWriterTag.Div)
        LayoutTemplate.InstantiateIn(div)

        Controls.Clear()
        Controls.Add(div)
        div.CopyBaseAttributes(Me)
        div.RenderControl(writer)

    End Sub

End Class

