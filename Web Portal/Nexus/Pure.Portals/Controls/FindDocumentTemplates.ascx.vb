Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.IO
Imports System.Xml
Imports System.Exception
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants

Partial Class Controls_FindDocumentTemplates
    Inherits System.Web.UI.UserControl

    Protected Sub btnFindNow_Click(sender As Object, e As EventArgs) Handles btnFindNow.Click
        FillDocumentTemplate()
    End Sub


    ''' <summary>
    ''' Populate the picklist control with tempaltes that are available for selection
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub FillDocumentTemplate()
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oDocumentTemplate As New NexusProvider.DocumentTemplate
        Dim oDocumentTemplateColl As NexusProvider.DocumentTemplateCollection = Nothing
        Dim oDocumentTemplateCollFiltered As New NexusProvider.DocumentTemplateCollection
        Dim oRiskType As NexusProvider.RiskType = Session(CNRiskType)
        'Initialize xml doc to get the risk xml from xml dataset
        Dim Doc As New XmlDocument
        Dim sProductPath() As String = CStr(Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                 .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
        Dim oPortalConfig As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Dim oProductConfiguration As New Nexus.Library.Config.Product
        'If product code is in session, fill from session otherwise fill from Session("ProductCode")
        If oQuote IsNot Nothing Then
            oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)
        ElseIf Session("ProductCode") IsNot Nothing Then
            oProductConfiguration = oPortalConfig.Products.Product(Session("ProductCode"))
        End If

        Dim iRiskCount As Integer = 0
        If oProductConfiguration IsNot Nothing Then
            For Each oRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                If oRisk.IsMandatory = False Then
                    iRiskCount += 1
                End If
            Next
        End If

        If oProductConfiguration IsNot Nothing Then
            'Check RiskTypes for selected product and for more than one RiskType open the Modal dialog Box
            If oProductConfiguration.RiskTypes.Count = 1 AndAlso iRiskCount = 0 Then
                'if only risk is there and it is mandatory 
                Dim oRisk As Nexus.Library.Config.RiskType = oProductConfiguration.RiskTypes.RiskType(0)
                ''set up the risk type object from the details in config
                oRiskType.DataModelCode = oRisk.DataModelCode
                oRiskType.Name = oRisk.Name
                oRiskType.Path = oRisk.Path
                oRiskType.RiskCode = oRisk.RiskCode
                Session(CNRiskType) = oRiskType
                'now redirect
            ElseIf iRiskCount >= 1 Or (oProductConfiguration.AllowMultiRisks IsNot Nothing AndAlso oProductConfiguration.AllowMultiRisks = False) Then
                'there's only one risk type so add this risk type to session
                oRiskType = New NexusProvider.RiskType
                For Each oRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                    If oRisk.IsMandatory = False Then
                        ''set up the risk type object from the details in config
                        oRiskType.DataModelCode = oRisk.DataModelCode
                        oRiskType.Name = oRisk.Name
                        oRiskType.Path = oRisk.Path
                        oRiskType.RiskCode = oRisk.RiskCode
                        Session(CNRiskType) = oRiskType

                        Exit For
                    End If
                Next
            End If
        End If
        oDocumentTemplate.TypeCode = ddlType.SelectedValue
        If txtCode.Text <> "" Then
            oDocumentTemplate.Code = txtCode.Text
        End If
        If txtEffectiveDate.Text <> "" Then
            oDocumentTemplate.EffectiveDate = txtEffectiveDate.Text
        End If

        If ddlType.SelectedIndex = 0 Then
            oDocumentTemplateColl = oWebService.FindDocumentTemplates(oDocumentTemplate, "", Nothing, v_bViaClientManager:=False)
        Else
            oDocumentTemplateColl = oWebService.FindDocumentTemplates(oDocumentTemplate, "", Nothing, v_bViaClientManager:=True)
        End If
       
        If oDocumentTemplateColl IsNot Nothing Then
            For Each oDocumentTemplateRow As NexusProvider.DocumentTemplate In oDocumentTemplateColl
                If oDocumentTemplateRow.TypeCode.ToUpper().Trim() <> "CLAUSES" Then
                    oDocumentTemplateCollFiltered.Add(oDocumentTemplateRow)
                End If
            Next
            grdvTemplates.DataSource = oDocumentTemplateCollFiltered
            grdvTemplates.DataBind()
        Else
            grdvTemplates.DataSource = Nothing
            grdvTemplates.DataBind()
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindClauses()
        End If
    End Sub

    Protected Sub BindClauses()
        Dim oLookUP, oStatusLookup As New NexusProvider.LookupListCollection
        Dim oWebService = New NexusProvider.ProviderManager().Provider
        oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "document_type", True, False)
        oLookUP.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)
        ddlType.Items.Clear()
        For iProductCount As Integer = 0 To oLookUP.Count - 1
            Dim lstProductCount As New ListItem
            If Trim(oLookUP(iProductCount).Code).ToUpper() <> "CLAUSES" Then
                lstProductCount.Text = oLookUP(iProductCount).Description
                lstProductCount.Value = Trim(oLookUP(iProductCount).Code)
                ddlType.Items.Add(lstProductCount)
                ddlType.DataBind()
            End If
        Next
        ddlType.Items.Insert(0, (New ListItem("(All)", "")))
        ddlType.SelectedIndex = 0
    End Sub

    Protected Sub btnNewSearch_Click(sender As Object, e As EventArgs) Handles btnNewSearch.Click
        grdvTemplates.DataSource = Nothing
        grdvTemplates.DataBind()
        txtCode.Text = ""
        txtEffectiveDate.Text = ""
        ddlType.SelectedIndex = 0
    End Sub

    Protected Sub grdvTemplates_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdvTemplates.RowCommand
        If e.CommandName = "Select" Then
            Dim sCode As String = e.CommandArgument
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If (oWebService.CheckDocumentTemplateExists(sCode)) Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Refresh") & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.SetTemplate('" & sCode & "');", True)
                'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            Else
                Page.ClientScript.RegisterStartupScript(GetType(String), "alert", "alert('" & GetLocalResourceObject("msgTemplateDoesntExists").ToString() & "');", True)
                Exit Sub
            End If

        End If
    End Sub
End Class
