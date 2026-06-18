Imports System.Text
Imports System.IO
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace Frontend

    Public Class MetaTags

        Private hldrMetaTags As PlaceHolder

        Public Sub New()
            hldrMetaTags = New PlaceHolder
        End Sub

        Private Sub WriteMetaTag(ByVal pName As String, ByVal pContent As String)

            Dim tmpHTMLControl As New HtmlGenericControl("meta")
            tmpHTMLControl.Attributes("name") = pName
            tmpHTMLControl.Attributes("content") = pContent

            hldrMetaTags.Controls.Add(tmpHTMLControl)

            tmpHTMLControl.Dispose()

        End Sub

        Public WriteOnly Property Keywords() As String
            Set(ByVal Value As String)
                WriteMetaTag("Keywords", Value)
            End Set
        End Property

        Public WriteOnly Property Description() As String
            Set(ByVal Value As String)
                WriteMetaTag("Description", Value)
            End Set
        End Property

        Public WriteOnly Property LastModified() As Date
            Set(ByVal Value As Date)
                WriteMetaTag("Last-Modified", Value.ToString)
            End Set
        End Property

        Public WriteOnly Property Title() As String
            Set(ByVal Value As String)
                WriteMetaTag("Title", Value)
            End Set
        End Property

        Public ReadOnly Property HTML() As String
            Get

                Dim sbTmp As New StringBuilder
                Dim swTmp As New StringWriter(sbTmp)
                Dim htmlOutput As New HtmlTextWriter(swTmp)

                hldrMetaTags.RenderControl(htmlOutput)

                Return sbTmp.ToString

            End Get
        End Property

    End Class

End Namespace