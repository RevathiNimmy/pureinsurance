Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml

Namespace Nexus

    Partial Class Modal_InstalmentRcptDiff
        Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'btnCancel.Attributes.Add("onclick", "self.parent.tb_remove();return false;")
            btnTakeExactAmt.Attributes.Add("onclick", "self.parent.TakeExactAmount();return false;")
            'Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.setPolicy('" + e.CommandArgument.ToString + "');", True)
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'setting the default master page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub



        Protected Sub btnWriteOffDiff_Click(sender As Object, e As EventArgs) Handles btnWriteOffDiff.Click
            BindWriteOffReason()
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
                If oNodeList IsNot Nothing AndAlso oNodeList.Count > 0 Then
                    Dim sURL As String = String.Empty

                    If HttpContext.Current.Session.IsCookieless Then
                        sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/WriteOffReason.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                    Else
                        sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/WriteOffReason.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                    End If
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);  document.getElementById('liPaymentTab').style.display = 'none';});</script>")
                    Exit Sub
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('No writeoff reason configured.');document.getElementById('liPaymentTab').style.display = 'none';});</script>")
                    Exit Sub
                End If
            End If


        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            Dim oCashListItems As NexusProvider.ReceiptCashListItemType
            If Session(CNCashListItem) IsNot Nothing AndAlso Session("ModeType") = "Receipt" Then
                oCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                If oCashListItems IsNot Nothing AndAlso oCashListItems.ReceiptItems.Count > 0 Then
                    Dim iCount As Integer = oCashListItems.ReceiptItems.Count - 1
                    oCashListItems.ReceiptItems.Remove(iCount)
                End If
                Session(CNCashListItem) = oCashListItems
            End If
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub
    End Class
End Namespace
