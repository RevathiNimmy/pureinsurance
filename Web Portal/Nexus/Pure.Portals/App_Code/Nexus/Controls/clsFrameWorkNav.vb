Imports System.Web.HttpContext

Namespace Nexus

    Public Class FrameWorkNav : Inherits System.Web.UI.UserControl

        Private alLinks As New ArrayList
        Private sCssClass As String
        Private sCurrentLinkCssClass As String

        Public Property CssClass() As String
            Get
                Return sCssClass
            End Get
            Set(ByVal value As String)
                sCssClass = value
            End Set
        End Property

        Public Property CurrentLinkCssClass() As String
            Get
                Return sCurrentLinkCssClass
            End Get
            Set(ByVal value As String)
                sCurrentLinkCssClass = value
            End Set
        End Property

        Public Sub AddSeperator(ByVal pLabel As String)
            alLinks.Add(New Pair(pLabel, "<seperator>"))
        End Sub

        Public Sub AddLink(ByVal pLabel As String, ByVal pUrl As String)

            alLinks.Add(New Pair(pLabel, pUrl))

        End Sub

        Public Sub RemoveLink(ByVal pLabel As String)

            For Each tmpLink As Pair In alLinks
                If LCase(tmpLink.First) = LCase(pLabel) Then
                    alLinks.Remove(tmpLink)
                End If
            Next

        End Sub

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.Render(writer)

            Dim phTmp As New PlaceHolder

            phTmp.Controls.Add(New LiteralControl("<ul" & IIf(sCssClass Is Nothing, "", " class=""" _
                & sCssClass & """") & ">" & vbCr))

            For Each tmpLink As Pair In alLinks

                If tmpLink.Second = "<seperator>" Then
                    phTmp.Controls.Add(New LiteralControl("<br />" & vbCr))
                Else
                    phTmp.Controls.Add(New LiteralControl("<li" & IIf(sCurrentLinkCssClass Is Nothing, "", _
                    IIf(IsCurrentPage(tmpLink.Second), " class=""" & sCurrentLinkCssClass & """", "")) _
                    & "><a href=""" & tmpLink.Second & """>" & tmpLink.First & "</a></li>" & vbCr))
                End If
            Next
            phTmp.Controls.Add(New LiteralControl("</ul>" & vbCr))

            phTmp.RenderControl(writer)

        End Sub

    End Class

End Namespace
