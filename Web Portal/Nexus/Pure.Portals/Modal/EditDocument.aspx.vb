Imports System.IO
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Xml
Imports System.Exception
Imports SiriusFS.SAM.Client


Namespace Nexus
    Partial Class Modal_EditDocument
        Inherits System.Web.UI.Page

        Protected sMode As String

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'check the mode.
                If Request.QueryString("Mode") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("Mode")) Then
                    sMode = Request.QueryString("Mode")
                    If sMode = "View" Then
                        'if mode is view then change the page and header text to "View"
                        lblPageHeader.Text = GetLocalResourceObject("lbl_View_g")
                        'set the save button to visible false if mode is view
                        btnSave.Visible = False
                    End If
                End If
                If Not Page.IsPostBack Then
                    'get the path from query string
                    Dim savePath As String = Request.QueryString("DocPath")
                    'set the instance of file info using savepath
                    Dim oFileInfo As New FileInfo(savePath)
                    If oFileInfo.Exists Then
                        'read the file
                        Dim byteArray As Byte() = File.ReadAllBytes(savePath)
                        Using docStream As New MemoryStream()
                            'using memory stream writr the file.
                            docStream.Write(byteArray, 0, CInt(byteArray.Length))
                            'set the position of cursor to first positon
                            docStream.Position = 0
                            Dim reader As New StreamReader(docStream) 'read the stream into a string
                            Dim strDoc As String = reader.ReadToEnd
                            Dim iStartTitle, iEndTitle As Integer
                            'remove the title from document
                            iStartTitle = InStr(1, strDoc, "<title>")
                            iEndTitle = InStr(1, strDoc, "</title>")
                            strDoc = strDoc.Replace("-aw-import:ignore", "")
                            If iStartTitle > 0 Then
                                Dim sSubstring As String = strDoc.Substring(iStartTitle - 1, (iEndTitle - iStartTitle) + 8)
                                'display in editor
                                txtDocumentEditor.Text = strDoc.Replace(sSubstring, "")
                            Else
                                txtDocumentEditor.Text = strDoc
                            End If
                        End Using
                    End If

                End If


            Finally

            End Try
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Try
                'get the path from query string
                Dim path As String = Request.QueryString("DocPath")
                'use stream wrtier to write the file
                Dim writer As New StreamWriter(path)

                'document returned from html editor does not returns valid html file so in order to make it in HTML format so that SAM can use, below tags should be added 
                'add  starting html tag
                writer.WriteLine("<html>")
                'add title tag
                writer.WriteLine("<title> </title>")
                'add head tag
                writer.WriteLine("<head> </head>")
                'add  start body tag
                writer.WriteLine("<body>")
                'write from  editor
                writer.WriteLine(txtDocumentEditor.Text.Trim())
                'add  closing body tag
                writer.WriteLine("</body>")
                'add  closing html tag
                writer.WriteLine("</html>")
                writer.Close()
                'Close form
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            Finally

            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ' on the click of cancel do not save the data
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class
End Namespace

