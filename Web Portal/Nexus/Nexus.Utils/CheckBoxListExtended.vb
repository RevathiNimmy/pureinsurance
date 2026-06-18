Imports System.Web.UI.WebControls
Imports System.Web.HttpContext

Namespace Custom

    Public Class CheckBoxListExtended : Inherits CheckBoxList

        Private bIndeterminate As Boolean = False

        Public Sub New()

            ViewState.Add("AllSelected", False)

        End Sub

        Public Property AllSelected() As Boolean
            Get
                Return ViewState("AllSelected")
            End Get
            Set(ByVal value As Boolean)
                ViewState.Add("AllSelected", value)
            End Set
        End Property

        Public Property UseIndeterminate() As Boolean
            Get
                Return bIndeterminate
            End Get
            Set(ByVal value As Boolean)
                bIndeterminate = value
            End Set
        End Property

        Private Sub CheckBoxListExtended_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            Page.ClientScript.RegisterClientScriptInclude("CheckBoxListExtended", _
                Page.ClientScript.GetWebResourceUrl(Me.GetType, "MM.Controls.CheckBoxListExtended.js"))

            If CType(ViewState("AllSelected"), Boolean) Then
                For Each oItem As ListItem In Items
                    oItem.Selected = True
                Next
            End If

        End Sub

        Protected Overrides Function LoadPostData(ByVal postDataKey As String, ByVal postCollection As System.Collections.Specialized.NameValueCollection) As Boolean

            If Current.Request.Form(ID) IsNot Nothing Then

                Dim sCheckedItems() As String = Current.Request.Form(ID).Split(",")
                Dim bAllSelected As Boolean

                If Array.BinarySearch(sCheckedItems, "0") >= 0 Then
                    bAllSelected = True
                Else
                    bAllSelected = False
                End If

                For Each oItems As ListItem In Items

                    If bAllSelected Then
                        oItems.Selected = True
                    Else
                        If Array.BinarySearch(sCheckedItems, oItems.Value) >= 0 Then
                            oItems.Selected = True
                        Else
                            oItems.Selected = False
                        End If
                    End If

                Next

                Return True

            Else
                Return False
            End If

        End Function

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

            'Render 'All' Checkbox
            Dim ltLabel As New Literal
            Dim ltCheckBox As New Literal

            ltLabel.Text = "<label for=""" & ID & "_0"""
            ltLabel.Text &= IIf(CssClass <> "", ">", " class=""" & CssClass & """>")
            ltLabel.Text &= " All</label>"

            ltCheckBox.Text = "<input type=""checkbox"" id=""" & ID & "_0"" name=""" & ID _
                & """ value=""0"" onclick=""toggleChecked(this)""" _
                & IIf(ViewState("AllSelected"), " checked", "") & IIf(Enabled, " enabled", "") & " />"

            Select Case TextAlign
                Case Web.UI.WebControls.TextAlign.Left

                    ltLabel.RenderControl(writer)
                    ltCheckBox.Text &= "<br />" & vbCr
                    ltCheckBox.RenderControl(writer)

                Case Web.UI.WebControls.TextAlign.Right

                    ltCheckBox.RenderControl(writer)
                    ltLabel.Text &= "<br />" & vbCr
                    ltLabel.RenderControl(writer)

            End Select

            'Render the items bound to the control as checkboxs
            For i As Integer = 1 To Items.Count

                ltLabel = New Literal
                ltCheckBox = New Literal

                ltLabel.Text = "<label for=""" & ID & "_" & i & """"
                ltLabel.Text &= IIf(CssClass <> "", ">", " class=""" & CssClass & """>")
                ltLabel.Text &= " " & Items(i - 1).Text & "</label>"

                ltCheckBox.Text = "<input type=""checkbox"" id=""" & ID & "_" & i & """ name=""" & ID _
                    & """ value=""" & Items(i - 1).Value & """ onclick=""" _
                    & IIf(bIndeterminate, "toggleIndeterminate", "toggleController") & "(this)""" _
                    & IIf(Items(i - 1).Selected, " checked", "") & IIf(Enabled, " enabled", "") & " />"

                Select Case TextAlign
                    Case Web.UI.WebControls.TextAlign.Left

                        ltLabel.RenderControl(writer)
                        ltCheckBox.Text &= "<br />" & vbCr
                        ltCheckBox.RenderControl(writer)

                    Case Web.UI.WebControls.TextAlign.Right

                        ltCheckBox.RenderControl(writer)
                        ltLabel.Text &= "<br />" & vbCr
                        ltLabel.RenderControl(writer)

                End Select

            Next

        End Sub

    End Class

End Namespace
