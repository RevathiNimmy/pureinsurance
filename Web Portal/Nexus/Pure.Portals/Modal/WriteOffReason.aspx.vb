Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml

Namespace Nexus

    Partial Class Modal_WriteOffReason
        Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            
            BindWriteOffReason()
            If ddlWriteOffReason.SelectedValue <> "" Then
                hdnWriteOffReasonID.Value = ddlWriteOffReason.SelectedValue
            End If
            btnOk.Attributes.Add("onclick", "self.parent.ReadWriteOffReason('" + hdnWriteOffReasonID.Value + "');return false;")
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub BindWriteOffReason()
            Dim oWebService = New NexusProvider.ProviderManager().Provider
            Dim oMediaList As NexusProvider.LookupListCollection
            Dim oReceiptMediaList As New NexusProvider.LookupListCollection
            Dim oPaymentMediaList As New NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing

            oMediaList = oWebService.GetList(NexusProvider.ListType.PMLookup, "WRITE_OFF_REASON", True, False, , , , v_sOptionList)
            Dim hCurrentOptionColl As New Hashtable()
            Dim iSourceId As Integer
            iSourceId = GetCodeForKey(NexusProvider.ListType.PMLookup, HttpContext.Current.Session(CNBranchCode), "Source", False)
            'Load the xml element 
            If v_sOptionList IsNot Nothing Then
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(sXML)
                Dim oNodeList As XmlNodeList
                'Filtering the XML with the Description of the UDL
                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/WRITE_OFF_REASON[is_valid_for_instalments=1 and is_deleted=0]")
                'GENERAL__CONTACT.Items.Clear()
                Dim iCount As Integer = 0
                If oNodeList IsNot Nothing And oNodeList.Count > 0 Then
                    'Adding the list in the DDL 
                    For Each oNode As XmlNode In oNodeList
                        ddlWriteOffReason.Items.Insert(iCount, New ListItem(oNode.SelectSingleNode("description").InnerXml.ToString(), oNode.SelectSingleNode("write_off_reason_id").InnerXml.ToString()))
                    Next
                End If
            End If
        End Sub

        Protected Sub ddlWriteOffReason_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWriteOffReason.SelectedIndexChanged
            hdnWriteOffReasonID.Value = ddlWriteOffReason.SelectedValue
        End Sub

       
        'Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        '    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.ReadWriteOffReason('" + ddlWriteOffReason.SelectedValue + "');return false;", True)
        '    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "closeThickBox", "self.parent.setClaimReference('" + e.CommandArgument.ToString + "','" + iClaimKey.ToString + "','CC');", True)
        'End Sub
    End Class
End Namespace
