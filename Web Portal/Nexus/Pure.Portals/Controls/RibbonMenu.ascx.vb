Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports CMS.Library
Imports System.Xml.XPath
Imports System.Web.HttpContext
Imports System.Xml
Imports SiriusFS.SAM.Client
Imports System.IO
Imports System.Globalization
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Linq
Imports System.Xml.Linq
Imports System.Xml.Serialization
Imports System.Xml.Linq.XElement

Namespace Nexus
    Partial Class Controls_RibbonMenu
        Inherits System.Web.UI.UserControl

        Dim oRiskType As New NexusProvider.RiskType
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting = Nothing

        'Checking the code IsclientViewOnly 
        Dim oUserAuthority As New NexusProvider.UserAuthority

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            If Not IsPostBack Then
                oWebService = New NexusProvider.ProviderManager().Provider
                If Current.Session(Nexus.Constants.CNLoginName) IsNot Nothing Then
                    oPaymentHubEnabled = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
                    ViewState("PaymentHubEnabled") = oPaymentHubEnabled.OptionValue
                Else
                    rptCategory.Visible = False
                End If

                If Session(CNLoginName) IsNot Nothing Then
                    oUserAuthority.UserCode = Session(CNLoginName)
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsClientManagerViewonly
                    oWebService.GetUserAuthorityValue(oUserAuthority)
                    ViewState("bIsClientManagerViewOnly") = oUserAuthority.UserAuthorityValue
                End If
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim doc As XDocument
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                Dim sConfigFilePath As String
                Dim oFile As FileInfo

                sConfigFilePath = Server.MapPath("~\Portal\" + oPortal.Name.ToString() + "\RibbonMenu.config")
                oFile = New FileInfo((sConfigFilePath))

                If oFile.Exists() Then
                    'Load the xml config file from portal
                    If Cache("RibbonConfigFile" & oPortal.Name.ToString()) Is Nothing Then
                        doc = XDocument.Load(sConfigFilePath)
                        Dim dependency As New CacheDependency(sConfigFilePath)
                        Cache.Insert("RibbonConfigFile" & oPortal.Name.ToString(), doc, dependency)
                    Else
                        doc = Cache("RibbonConfigFile" & oPortal.Name.ToString())
                    End If

                    'Select category nodes using LINQ query
                    Dim categoryNodes =
                        From catNodes In doc.<ribbonMenu>.<ribbonCategory> Select catNodes

                    'Bind Toolbar Nodes
                    rptToolbar.DataSource = categoryNodes
                    rptToolbar.DataBind()

                    'Bind Category Nodes
                    rptCategory.DataSource = categoryNodes
                    rptCategory.DataBind()
                End If

            End If

        End Sub

        Protected Sub rptCategory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCategory.ItemDataBound

            Dim item As RepeaterItem = e.Item
            Dim rptSection As Repeater
            'Find the XML document for current Category Node
            Dim doc = CType(item.DataItem, System.Xml.Linq.XElement)
            'Find section nodes for current category using LINQ Query
            Dim sectionNodes =
                From secNodes In doc.<ribbonSection> Select secNodes

            If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then

                Dim RptCategorySpanText As HtmlGenericControl = e.Item.FindControl("RptCategorySpanText")
                Dim RptCategoryI As HtmlGenericControl = e.Item.FindControl("RptCategoryI")
                If RptCategorySpanText IsNot Nothing AndAlso RptCategoryI IsNot Nothing Then
                    If RptCategorySpanText.InnerText.Trim().ToUpper() = "HOME" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-home")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "CLIENT" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-users")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "POLICY" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-credit-card-alt")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "CLAIM" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-heart")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "FINANCE" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-bar-chart")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "REPORTS" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-th-large")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "OTHERS" Then
                        RptCategoryI.Attributes.Add("class", "icon fa fa-legal")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "FAVOURITES" Then
                        RptCategoryI.Attributes.Add("class", "icon  fa fa-briefcase")
                    ElseIf RptCategorySpanText.InnerText.Trim().ToUpper() = "ADMINISTRATOR" Then
                        RptCategoryI.Attributes.Add("class", "icon  fa fa-user")
                    End If
                End If
                If item.FindControl("rptSection") IsNot Nothing Then
                    rptSection = CType(item.FindControl("rptSection"), Repeater)
                    'Bind the control with the selected sections
                    rptSection.DataSource = sectionNodes
                    rptSection.DataBind()
                End If
            End If

        End Sub

        Protected Sub rptSection_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
            Dim item As RepeaterItem = e.Item
            Dim rptItem As Repeater
            'Find the XML document for current Section Node
            Dim doc = CType(item.DataItem, System.Xml.Linq.XElement)

            'Find item nodes for current section using LINQ query
            Dim itemNodes =
                From itmNodes In doc.<ribbonItem> Select itmNodes

            If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
                'Find Repeater Control for items
                If item.FindControl("rptItem") IsNot Nothing Then
                    rptItem = CType(item.FindControl("rptItem"), Repeater)
                    'Bind the control with the selected items
                    rptItem.DataSource = itemNodes
                    rptItem.DataBind()
                End If
            End If
            Dim oMultiStepApproval As New NexusProvider.OptionTypeSetting
            oWebService = New NexusProvider.ProviderManager().Provider
            oMultiStepApproval = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.MultiStepApproval)
            If doc.Attribute("title").Value = "Finance Authorisation" Then
                item.Visible = False
                If oMultiStepApproval IsNot Nothing AndAlso oMultiStepApproval.OptionValue = "1" Then
                    item.Visible = True
                End If
            End If

            If Not Nexus.UserCanDoTask("AuditTrailView") Then
                If doc.Attribute("title").Value = "System Events" Then
                    item.Visible = False
                End If
            End If
            Dim bHasItemInRole As Boolean = False
            For Each objXElement As XElement In itemNodes
                If UserIsInRoles(objXElement.Attribute("roles").Value()) Then
                    bHasItemInRole = True
                    Exit For
                End If
            Next
            'hide section if none of item in role
            If Not bHasItemInRole Then
                item.Visible = False
            End If
        End Sub

        Protected Sub rptItem_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            Dim item As RepeaterItem = e.Item
            'Find the XML document for current Section Node
            Dim doc = CType(item.DataItem, System.Xml.Linq.XElement)
            'check if call made for model page so we can launch model page for that particular link
            If doc.Attribute("url").Value().Contains("ShowModal=True") Then
                Dim strURL As String = doc.Attribute("url").Value().Replace("&ShowModal=True", "")
                strURL = strURL + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=750&IsFromReport=True"
                CType(e.Item.FindControl("A1"), HtmlAnchor).Attributes.Add("data", "modal")
                CType(e.Item.FindControl("A1"), HtmlAnchor).Attributes.Add("href", strURL)
            Else
                CType(e.Item.FindControl("A1"), HtmlAnchor).HRef = doc.Attribute("url").Value()
            End If

            If Not UserIsInRoles(doc.Attribute("roles").Value()) Then
                item.Visible = False
            End If
            If doc.Attribute("title").Value = "Card Registration" AndAlso (ViewState("PaymentHubEnabled") Is Nothing OrElse ViewState("PaymentHubEnabled").ToString() = "0") Then
                item.Visible = False
            End If

            If (doc.Attribute("title").Value.ToUpper() = "NEW PERSONAL CLIENT" Or doc.Attribute("title").Value.ToUpper() = "NEW CORPORATE CLIENT") AndAlso (ViewState("bIsClientManagerViewOnly") IsNot Nothing AndAlso ViewState("bIsClientManagerViewOnly").ToString() = "1") Then
                item.Visible = False
            End If

        End Sub

        Protected Sub rptToolbar_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptToolbar.ItemDataBound

            Dim item As RepeaterItem = e.Item
            Dim doc = CType(item.DataItem, System.Xml.Linq.XElement)

            If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
                If UserIsInRoles(doc.Attribute("roles").Value()) Then
                    If item.ItemIndex = 0 Then ' set class as selected for first toolbar item(home)
                        If item.FindControl("toolbaritem") IsNot Nothing Then
                            CType(item.FindControl("toolbaritem"), HtmlGenericControl).Attributes.Add("class", "selected first")
                        End If
                    End If
                Else
                    item.Visible = False
                End If

            End If

        End Sub

        Protected Function GetCategoryClass(ByVal itemIndex As String) As String

            If itemIndex = "0" Then
                Return "selected"
            Else
                Return ""
            End If

        End Function

    End Class

End Namespace