Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Library
Imports Nexus.Utils
Imports CMS.Library
Imports System.Collections.Generic
Imports System.Xml.Linq
Imports Nexus
Imports System.IO

Partial Class secure_Download
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(CNDocumentToDownload) IsNot Nothing Then
            Dim oFileTypes As Config.FileTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(Portal.GetPortalID()).FileTypes
            Dim oDocument As NexusProvider.DocumentDefaults = Session(CNDocumentToDownload)

            Response.ClearHeaders()
            If oFileTypes.FileType(oDocument.FileType.ToUpper()) Is Nothing Then
                Response.ContentType = oFileTypes.FileType(oDocument.FileType.ToLower()).ContentType
            Else
                Response.ContentType = oFileTypes.FileType(oDocument.FileType.ToUpper()).ContentType
            End If
            Response.AddHeader("Content-Disposition", "attachment; filename=" & oDocument.DocumentName)
            Response.WriteFile(oDocument.FileLocation)
            Response.Flush()
            Response.End()

            Session(CNDocumentToDownload) = Nothing
            oDocument = Nothing
            oFileTypes = Nothing
        End If
        
    End Sub
End Class
