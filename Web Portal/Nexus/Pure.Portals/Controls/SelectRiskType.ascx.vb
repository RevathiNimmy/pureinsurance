Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_SelectRiskType : Inherits System.Web.UI.UserControl
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim oProducts As Config.Products = CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim sProductCode As String = Request.QueryString("ProductCode")
              
                Me.grdvSelectRiskType.DataSource = FillRiskTypes()
                Me.grdvSelectRiskType.DataBind()
            End If
        End Sub
        Function FillRiskTypes() As NexusProvider.RiskTypeCollection
            Dim oProducts As Config.Products = CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sProductCode As String = Request.QueryString("ProductCode")
            Dim oRiskTypes As New NexusProvider.RiskTypeCollection

            For iTempVar As Integer = 0 To oProducts.Count - 1
                If oProducts.Product(iTempVar).ProductCode = sProductCode Then
                    For Each oRisk As Nexus.Library.Config.RiskType In oProducts.Product(iTempVar).RiskTypes()
                        If oRisk.IsMandatory = False Then
                            Dim oRiskType As New NexusProvider.RiskType
                            oRiskType.DataModelCode = oRisk.DataModelCode
                            oRiskType.Name = oRisk.Name
                            oRiskType.Path = oRisk.Path
                            oRiskType.RiskCode = oRisk.RiskCode
                            oRiskTypes.Add(oRiskType)
                        End If
                    Next
                End If
            Next

            Return oRiskTypes
        End Function

        Protected Sub grdvSelectRiskType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSelectRiskType.Load
            If grdvSelectRiskType.PageCount < 2 Then
                grdvSelectRiskType.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvSelectRiskType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSelectRiskType.PageIndexChanging
            Me.grdvSelectRiskType.PageIndex = e.NewPageIndex
            Me.grdvSelectRiskType.DataSource = FillRiskTypes()
            Me.grdvSelectRiskType.DataBind()
        End Sub

        Protected Sub grdvSelectRiskType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSelectRiskType.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("RiskCode") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.RiskType).RiskCode)
            End If
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSelectRiskType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSelectRiskType.SelectedIndexChanged
            Dim ctrlClicked As String = ""
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Session(CNDataModelCode) = Me.grdvSelectRiskType.SelectedRow.Cells(2).Text
            Dim oProducts As Config.Products = CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
            Dim sProductCode As String = Request.QueryString("ProductCode")
            Session("ProductCode") = sProductCode
            Dim sFromPage As String = Request.QueryString("FromPage")
            Dim oRiskType As Config.RiskType = oProducts.Product(sProductCode).RiskTypes.RiskType(Me.grdvSelectRiskType.SelectedRow.Cells(1).Text)
            Dim oRiskTypes As New NexusProvider.RiskType
            Try
                oRiskTypes.DataModelCode = oRiskType.DataModelCode
                oRiskTypes.Name = oRiskType.Name
                oRiskTypes.RiskCode = oRiskType.RiskCode
                oRiskTypes.Path = oRiskType.Path
                Session(CNRiskType) = oRiskTypes

                'set up javascript to postback the parent page
                'this will render as self.parent.__doPostBack('__Page', 'RiskTypeSelected');
                'Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RiskTypeSelected") & ";"
                'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','RiskTypeSelected');", True)
            Catch ex As System.Exception
            Finally
                oProducts = Nothing
                oQuote = Nothing
                sProductCode = Nothing
                oRiskType = Nothing
                oRiskTypes = Nothing
                sFromPage = Nothing
                sProductCode = Nothing
            End Try
        End Sub

    End Class
End Namespace