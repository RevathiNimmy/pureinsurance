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
Imports TXTextControl

Namespace Nexus
    Partial Class Modal_OpenTXTextControl
        Inherits System.Web.UI.Page

        Protected sMode As String
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                btnSave.Visible = False
                'check the mode.
                If Request.QueryString("Mode") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("Mode")) Then
                    sMode = Request.QueryString("Mode")
                    If sMode = "View" Then
                        'if mode is view then change the page and header text to "View"
                        lblPageHeader.Text = GetLocalResourceObject("lbl_View_g")
                        'lblHeader.Text = GetLocalResourceObject("lbl_View_g")

                        'set the save button to visible false if mode is view
                        btnSave.Visible = False
                    End If
                End If
                 If Not Page.IsPostBack Then
                'get the path from query string
                Dim savePath As String = Request.QueryString("DocPath")

                    Dim sFileExtension As String = ".HTM"
                    Dim sTxtControlStreamType As TXTextControl.Web.StreamType = TXTextControl.Web.StreamType.HTMLFormat
                    If savePath IsNot Nothing AndAlso Not String.IsNullOrEmpty(savePath) Then
                        sFileExtension = Path.GetExtension(savePath)

                        Select Case sFileExtension.ToUpper
                            Case ".PDF"
                                sTxtControlStreamType = TXTextControl.Web.StreamType.AdobePDF
                            Case ".HTM", ".HTML"
                                sTxtControlStreamType = TXTextControl.Web.StreamType.HTMLFormat
                            Case ".DOC", ".DOCX"
                                sTxtControlStreamType = TXTextControl.Web.StreamType.WordprocessingML
                            Case Else
                                sTxtControlStreamType = TXTextControl.Web.StreamType.HTMLFormat
                        End Select
                    End If

                    TextControl1.LoadTextAsync(savePath, sTxtControlStreamType)

                 End If
            Catch ex As Exception

            Finally
            End Try
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Try
                ''get the path from query string
                Dim path As String = Request.QueryString("DocPath")

                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            Finally

            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ' on the click of cancel do not save the data
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
        Protected Sub DownloadAttachments()
            Dim sFileName As String = String.Empty
            Dim docPath As String = Request.QueryString("DocPath")

            sFileName = Path.GetFileName(docPath)

            Response.Clear()
            Response.BufferOutput = False
            Response.ContentType = "application/docx"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & sFileName)
            Response.WriteFile(docPath)
            Response.Flush()
            Response.End()

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnblockUI", "$.unblockUI();", True)
        End Sub

    End Class
End Namespace

