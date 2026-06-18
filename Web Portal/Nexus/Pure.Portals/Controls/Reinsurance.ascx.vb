Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Linq
Imports System

Namespace Nexus

    Partial Class Control_Reinsurance : Inherits System.Web.UI.UserControl

        Public Const TREATYPREMIUMTAXTYPE As String = "TTRITP"
        Public Const TREATYCOMMISSIONTAXTYPE As String = "TTRITC"
        Public Const FACPREMIUMTAXTYPE As String = "TTRIFP"
        Public Const FACCOMMISSIONTAXTYPE As String = "TTRIFC"

        Dim oWebService As NexusProvider.ProviderBase
        Dim oQuote As NexusProvider.Quote
        Dim CheckRiManualPremiumAdjustment As String

        ''' <summary>
        ''' To initialize the onclick modals
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            Dim sUrl As String
            Dim sTreatyUrl As String
            If HttpContext.Current.Session.IsCookieless Then
                sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindFAC.aspx?" & "modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
            Else
                sUrl = AppSettings("WebRoot") & "/Modal/FindFAC.aspx?" & "ClientID=" + updSubmitArea.ClientID + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
            End If
            If HttpContext.Current.Session.IsCookieless Then
                sTreatyUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SelectTreaty.aspx?" & "modal=true&KeepThis=true&TB_iframe=true&height=275&width=800"
            Else
                sTreatyUrl = AppSettings("WebRoot") & "/Modal/SelectTreaty.aspx?" & "ClientID=" + updSubmitArea.ClientID + "&modal=true&KeepThis=true&TB_iframe=true&height=275&width=800"
            End If
            btnAddTreaty.OnClientClick = "tb_show(null , '" & sTreatyUrl & "' , null);return false;"
            btnAddFacProp.OnClientClick = "tb_show(null , '" & sUrl & "' , null);return false;"

            GISLookup_RIOverrideReason.Visible = False
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                If oQuote IsNot Nothing Then
                    oWebService = New NexusProvider.ProviderManager().Provider
                    CheckRiManualPremiumAdjustment = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                               NexusProvider.ProductRiskOptions.RiManualPremiumAdjustment, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                End If
                If Request("__EVENTARGUMENT") = "AddFac" Then
                    AddFac(Convert.ToInt32(hfKey.Value), hfCode.Value, hfName.Value, Convert.ToDecimal(hfCommissionPercentage.Value), Convert.ToDecimal(hfTaxPercentage.Value))
                    If GISLookup_RIOverrideReason.Visible Then
                        PopulateRiOverrideReason(True)
                    End If
                    Exit Sub
                End If
                If Request("__EVENTARGUMENT") = "AddTreaty" Then
                    Dim bTreatyAlreadyExist As Boolean = False
                    AddTreaty(Convert.ToInt32(hfKey.Value), hfCode.Value, hfName.Value, bTreatyAlreadyExist)
                    If Not bTreatyAlreadyExist Then
                        PopulateRiOverrideReason(True)
                    End If
                    Exit Sub
                End If

                If (Session(CNMode) = Mode.View Or CType(Session(CNMode), Mode) = Mode.Review Or Session(CNRiskMode) = RiskMode.View) Then
                    btnAddFacProp.Visible = False
                    btnAddTreaty.Visible = False
                    btnCancel.Visible = False
                End If

                If btnCancel.Visible = True Then
                    btnCancel.OnClientClick = "return confirm('" & GetLocalResourceObject("msg_ConfirmRI").ToString() & "');"
                End If

                'Catlin Performance Fix
                If (Not IsPostBack AndAlso Me.Visible = True) Or (Me.Visible = True And Request("__EVENTARGUMENT") = "RefreshRatingGrid") Then

                    ' ''Make the "submit" button invisile at parent page
                    ''If Me.Parent.FindControl("divButton") IsNot Nothing Then
                    ''    CType(Me.Parent.FindControl("divButton"), HtmlGenericControl).Visible = False
                    ''End If

                    'Clear Cache, so that fresh data can be retrieved from SAM service

                    If Session(CNRIBands) IsNot Nothing Then
                        Session(CNRIBands) = Nothing
                    End If
                    If Session(CNRIData) IsNot Nothing Then
                        Session(CNRIData) = Nothing
                    End If

                    'To set the Focus
                    Page.SetFocus(ddlReinsurance)

                    'Populate the BAnd
                    PopulateRIBands()
                    ' Populate the Grid    
                    PopulateRIGrid(False)

                    PopulateRiOverrideReason(False)
                End If
            Catch ex As System.Exception
                Throw
            Finally
                oWebService = Nothing
            End Try

        End Sub
        Private Sub PopulateRiOverrideReason(ByVal bVisble As Boolean)
            Dim dsArrangements As DataSet
            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            dsArrangements = TryCast(Session(CNRIData), DataSet)
            If bVisble Then
                GISLookup_RIOverrideReason.Visible = True
                lblRIOverrideReason.Visible = True
            End If
            GISLookup_RIOverrideReason.Value = 0
            If dsArrangements.Tables(sCurrentDataTableName) IsNot Nothing Then
                With dsArrangements.Tables(sCurrentDataTableName)
                    If .Rows IsNot Nothing AndAlso .Rows(0).Item("RiOverrideReasonId") IsNot Nothing AndAlso Not IsDBNull(.Rows(0).Item("RiOverrideReasonId")) AndAlso
                       (.Rows(0).Item("RiOverrideReasonId").ToString().Trim <> "" OrElse .Rows(0).Item("RiOverrideReasonId").ToString().Trim <> "0") AndAlso Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId").ToString().Trim) > 0 Then
                        GISLookup_RIOverrideReason.Visible = True
                        lblRIOverrideReason.Visible = True
                        GISLookup_RIOverrideReason.Value = Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId").ToString().Trim)

                    Else
                        If GISLookup_RIOverrideReason.Visible = True Then
                            GISLookup_RIOverrideReason.Value = 0
                        End If
                    End If
                End With
                uplDetail.Update()
                gvReinsurance.Visible = True
            Else
                gvReinsurance.Visible = False
            End If

        End Sub
        ''' <summary>
        ''' On RI grid load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvReinsurance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReinsurance.Load
            If gvReinsurance.PageCount = 1 Then
                gvReinsurance.AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' On change of page index
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvReinsurance_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReinsurance.PageIndexChanging

            gvReinsurance.PageIndex = e.NewPageIndex
            gvReinsurance.DataBind()

        End Sub

        ''' <summary>
        ''' On change of band from dropdown
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlReinsurance_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReinsurance.SelectedIndexChanged

            PopulateRIGrid(False)
            Dim dsArrangements As DataSet
            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue

            dsArrangements = TryCast(Session(CNRIData), DataSet)
            If dsArrangements.Tables(sCurrentDataTableName) IsNot Nothing Then
                With dsArrangements.Tables(sCurrentDataTableName)

                    GISLookup_RIOverrideReason.Visible = False
                    lblRIOverrideReason.Visible = False
                    uplDetail.Update()
                    For Each dr As DataRow In .Rows
                        If dr("ActionType") <> NexusProvider.RowAction.None AndAlso (dr("LineType") = "T" OrElse dr("LineType") = "R") Then
                            GISLookup_RIOverrideReason.Visible = True
                            lblRIOverrideReason.Visible = True
                            uplDetail.Update()
                            Exit For
                        End If
                    Next
                    With dsArrangements.Tables(sCurrentDataTableName)
                        If .Rows IsNot Nothing AndAlso .Rows(0).Item("RiOverrideReasonId") IsNot Nothing AndAlso Not IsDBNull(.Rows(0).Item("RiOverrideReasonId")) AndAlso
                                   (.Rows(0).Item("RiOverrideReasonId").ToString().Trim <> "" OrElse .Rows(0).Item("RiOverrideReasonId").ToString().Trim <> "0") AndAlso Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId").ToString().Trim) > 0 Then
                            GISLookup_RIOverrideReason.Visible = True
                            lblRIOverrideReason.Visible = True
                            GISLookup_RIOverrideReason.Value = Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId").ToString().Trim)
                            uplDetail.Update()
                        Else
                            If GISLookup_RIOverrideReason.Visible = True Then
                                GISLookup_RIOverrideReason.Value = 0
                            End If
                        End If
                    End With
                End With
                gvReinsurance.Visible = True
            Else
                gvReinsurance.Visible = False
            End If
            dsArrangements.AcceptChanges()

            Session(CNRIData) = dsArrangements

            Dim sOriginalDataTableName As String = "Arrangement_Original_" & ddlReinsurance.SelectedValue
            If dsArrangements.Tables(sOriginalDataTableName) IsNot Nothing Then
                With dsArrangements.Tables(sOriginalDataTableName)

                    GISLookup_RIOverrideReasonOrig.Visible = False
                    lblOrigRiOverrideResason.Visible = False

                    If .Rows IsNot Nothing AndAlso .Rows(0).Item("RiOverrideReasonId") IsNot Nothing AndAlso Not IsDBNull(.Rows(0).Item("RiOverrideReasonId")) AndAlso
                            (.Rows(0).Item("RiOverrideReasonId").ToString().Trim <> "" OrElse .Rows(0).Item("RiOverrideReasonId").ToString().Trim <> "0") AndAlso Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId").ToString().Trim) > 0 Then
                        GISLookup_RIOverrideReasonOrig.Visible = True
                        lblOrigRiOverrideResason.Visible = True
                        GISLookup_RIOverrideReasonOrig.Value = Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId").ToString().Trim)
                        GISLookup_RIOverrideReasonOrig.Enabled = False
                    Else
                        If GISLookup_RIOverrideReasonOrig.Visible = True Then
                            GISLookup_RIOverrideReasonOrig.Value = 0
                        End If
                    End If
                End With
                gvOrgReinsurance.Visible = True
                UpdatePanelReinsurance.Update()
            Else
                GISLookup_RIOverrideReasonOrig.Visible = False
                lblOrigRiOverrideResason.Visible = False
                gvOrgReinsurance.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' To Add a selected FAC lines
        ''' </summary>
        ''' <param name="nKey"></param>
        ''' <param name="sCode"></param>
        ''' <param name="sName"></param>
        ''' <remarks></remarks>
        Private Sub AddFac(nKey As Integer, sCode As String, sName As String, dCommissionPerc As Decimal, dTaxPerc As Decimal)
            'Add FAC Prop to dataset table
            Dim dsArrangements As DataSet
            Dim dtRIArrangement As DataTable
            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            Dim drArrangement As DataRow
            dsArrangements = TryCast(Session(CNRIData), DataSet)
            dtRIArrangement = dsArrangements.Tables(sCurrentDataTableName)


            If dtRIArrangement.Select("PartyKey=" & nKey.ToString()).Length > 0 Then
                Dim drExistingFacRow As DataRow = dtRIArrangement.Select("PartyKey=" & nKey.ToString())(0)
                If drExistingFacRow("ActionType") = NexusProvider.RowAction.DeleteRow Then
                    drExistingFacRow("ActionType") = NexusProvider.RowAction.EditRow
                    drExistingFacRow("TaxPerc") = Format(Math.Round(dTaxPerc, 2), "0.00")
                    drExistingFacRow("CommissionPerc") = Format(Math.Round(dCommissionPerc, 2), "0.00")
                    dsArrangements.AcceptChanges()
                    Session(CNRIData) = dsArrangements
                    PopulateRIGrid(True)
                Else
                    'Show validation message
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_FacExists").ToString().Replace("#Code", sCode.Trim()) & "');", True)
                End If
            Else
                drArrangement = dtRIArrangement.NewRow
                drArrangement("ActionType") = NexusProvider.RowAction.AddRow
                drArrangement("RIArrangementKey") = dtRIArrangement.Rows(1)("RIArrangementKey")
                drArrangement("LineType") = "F"
                drArrangement("PartyKey") = nKey
                drArrangement("Name") = sName
                drArrangement("DefaultPerc") = 0
                drArrangement("ThisPerc") = 0
                drArrangement("SumInsured") = "0.00"
                drArrangement("Premium") = "0.00"
                drArrangement("TaxPerc") = Format(Math.Round(dTaxPerc, 2), "0.00")
                drArrangement("Tax") = "0.00"
                drArrangement("CommissionPerc") = Format(Math.Round(dCommissionPerc, 2), "0.00")
                drArrangement("Commission") = "0.00"
                drArrangement("CommissionTax") = "0.00"
                drArrangement("Agreement") = ""
                drArrangement("DefaultLine") = 0
                If Session(CNMTAType) IsNot Nothing Then
                    If (ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = True) Then
                        dtRIArrangement.Rows.InsertAt(drArrangement, dtRIArrangement.Rows.Count - 4)
                    Else
                        dtRIArrangement.Rows.InsertAt(drArrangement, dtRIArrangement.Rows.Count - 5)
                    End If
                Else
                    If (ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = True) Then
                        dtRIArrangement.Rows.InsertAt(drArrangement, dtRIArrangement.Rows.Count - 2)
                    Else
                        dtRIArrangement.Rows.InsertAt(drArrangement, dtRIArrangement.Rows.Count - 3)
                    End If
                End If

                drArrangement = Nothing
                dtRIArrangement.AcceptChanges()
                dsArrangements.AcceptChanges()

                Session(CNRIData) = dsArrangements
                'Populate grid with updated dataset
                PopulateRIGrid(True)
            End If

        End Sub



        ''' <summary>
        ''' To populate the RI bands
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateRIBands()
            Try
                ' Obtaining the value of Reinsurance bands from SAM
                Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection
                If Session(CNRIBands) IsNot Nothing Then
                    oReinsurarerBandCollection = CType(Session(CNRIBands), NexusProvider.ReinsuranceBandsCollection)
                Else
                    oWebService = New NexusProvider.ProviderManager().Provider
                    ' Obtaining value of Quote from session
                    oQuote = Session(CNQuote)

                    If oQuote.Risks(Session(CNCurrentRiskKey)).ReturnPremiumMoreThanBilled = True Then
                        btnOk.OnClientClick = "alert('" & GetLocalResourceObject("msgReturnPremiumMoreThanBilled").ToString() & "'); return false;"
                    End If

                    ' Pass the RiskKey to get the Reinsurance band values for the Current risk key else 
                    If Session(CNCurrentRiskKey) Is Nothing Then
                        oReinsurarerBandCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(0).Key)
                    Else
                        oReinsurarerBandCollection = oWebService.GetRiskReinsuranceBands(CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key)
                    End If
                    Session(CNRIBands) = oReinsurarerBandCollection
                End If

                ddlReinsurance.Items.Clear()
                ' Adding value from the collection to the Dropdown
                For Each oReinsurarerBand As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
                    ddlReinsurance.Items.Add(New ListItem(oReinsurarerBand.Band, oReinsurarerBand.BandKey))
                Next
            Finally
                oWebService = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' To get default RI Lines (combination of SAM and internal logic)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetDefaultRIData()
            Try
                Dim nRiskKey As Integer
                Dim dsArrangements As New DataSet
                Dim drArrangement As DataRow
                Dim dtArrangement As New DataTable
                Dim dtCurrentArrangement As New DataTable
                Dim drAllocated As DataRow

                oWebService = New NexusProvider.ProviderManager().Provider

                If Session(CNCurrentRiskKey) Is Nothing Then
                    nRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(0).Key
                Else
                    nRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key
                End If

                Dim oReinsuranceTypeArragementsCollection As NexusProvider.ArrangementsTypeCollection
                Dim oReinsuranceArrangmentCollection As NexusProvider.ArrangementLinesTypeCollection
                Dim nArrangementId As Integer

                Dim dSumAssured, dPremium, dAllocatedSumAssured, dAllocatedPremium, dAllocatedThisPerc As Double
                Dim dAllocatedTax, dAllocatedCommision, dAllocatedCommisionTax As Double

                Dim dSectionThisPercTotal As Decimal = 0
                Dim dSectionSumAssuredTotal As Decimal = 0
                Dim dSectionPremiumTotal As Decimal = 0
                Dim dSectionCommisionTotal As Decimal = 0
                Dim dSectionCommisionTaxTotal As Decimal = 0
                Dim dSectionTaxTotal As Decimal = 0

                oReinsuranceTypeArragementsCollection = oWebService.GetRiskReinsuranceArrangements(nRiskKey)

                For Each oArrangementType As NexusProvider.ArrangementsType In oReinsuranceTypeArragementsCollection
                    dSumAssured = 0
                    dPremium = 0
                    dAllocatedSumAssured = 0
                    dAllocatedPremium = 0
                    dAllocatedThisPerc = 0
                    dAllocatedTax = 0
                    dAllocatedCommision = 0
                    dAllocatedCommisionTax = 0

                    dtArrangement = New DataTable
                    If oArrangementType.IsOriginal Then
                        dtArrangement.TableName = "Arrangement_Original_" & oArrangementType.BandId
                    Else
                        dtArrangement.TableName = "Arrangement_Current_" & oArrangementType.BandId
                    End If

                    dtArrangement.Columns.Add("ActionType", GetType(Integer))
                    dtArrangement.Columns.Add("RIArrangementKey", GetType(Integer))
                    dtArrangement.Columns.Add("RIArrangementLineKey", GetType(Integer))
                    dtArrangement.Columns.Add("TreatyCode", GetType(String))
                    dtArrangement.Columns.Add("PartyKey", GetType(Integer))
                    dtArrangement.Columns.Add("LineType", GetType(String))
                    dtArrangement.Columns.Add("Name", GetType(String))
                    dtArrangement.Columns.Add("DefaultPerc", GetType(String))
                    dtArrangement.Columns.Add("ThisPerc", GetType(String))
                    dtArrangement.Columns.Add("SumInsured", GetType(String))
                    dtArrangement.Columns.Add("Premium", GetType(String))
                    dtArrangement.Columns.Add("TaxPerc", GetType(String))
                    dtArrangement.Columns.Add("Tax", GetType(String))
                    dtArrangement.Columns.Add("CommissionPerc", GetType(String))
                    dtArrangement.Columns.Add("Commission", GetType(String))
                    dtArrangement.Columns.Add("CommissionTax", GetType(String))
                    dtArrangement.Columns.Add("Agreement", GetType(String))
                    dtArrangement.Columns.Add("RiOverrideReasonId", GetType(Integer))
                    dtArrangement.Columns.Add("DefaultLine", GetType(Integer))
                    nArrangementId = oArrangementType.ArrangementId

                    ' Obtaining the value of ArrangementLinesType for specific risk key from SAM
                    oReinsuranceArrangmentCollection = oWebService.GetRiskReinsuranceArrangementLines(nArrangementId)

                    'Add row for Band Total
                    drArrangement = dtArrangement.NewRow
                    drArrangement("ActionType") = NexusProvider.RowAction.None
                    drArrangement("Name") = GetLocalResourceObject("lbl_BandTotal")
                    drArrangement("RIArrangementKey") = nArrangementId
                    drArrangement("SumInsured") = String.Format("{0:n}", Math.Round(oArrangementType.SumInsured, 2))
                    drArrangement("Premium") = String.Format("{0:n4}", Math.Round(oArrangementType.Premium, 4))
                    drArrangement("RiOverrideReasonId") = IIf(IsDBNull(oArrangementType.RiOverrideReasonId), 0, oArrangementType.RiOverrideReasonId)
                    dtArrangement.Rows.Add(drArrangement)
                    drArrangement = Nothing

                    Dim oReinsuranceArrangmentNonFacLines = From nonFacLines In oReinsuranceArrangmentCollection Where nonFacLines.Type <> "F"
                                                        Select nonFacLines

                    For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentNonFacLines
                        With oArrangementLinesType
                            drArrangement = dtArrangement.NewRow
                            drArrangement("ActionType") = NexusProvider.RowAction.None
                            drArrangement("PartyKey") = .PartyKey
                            drArrangement("RIArrangementKey") = .RIarrangementKey
                            drArrangement("RIArrangementLineKey") = .RIArrangementLineKey
                            drArrangement("TreatyCode") = .TreatyCode
                            drArrangement("LineType") = .Type
                            drArrangement("Name") = .Name
                            drArrangement("DefaultPerc") = Format(Math.Round(.DefaultPerc, 2), "0.00")
                            drArrangement("ThisPerc") = Format(Math.Round(.ThisPerc, 2), "0.00")
                            If .SumInsured = "NaN" Then
                                .SumInsured = "0"
                            End If
                            drArrangement("SumInsured") = Format(Math.Round(.SumInsured, 2), "#,#0.00")
                            If drArrangement("SumInsured") = "NaN" Then
                                drArrangement("SumInsured") = "0"
                            End If
                            drArrangement("Premium") = String.Format("{0:n4}", Math.Round(.PremiumValue, 4))
                            drArrangement("TaxPerc") = Format(Math.Round(.TaxPerc, 2), "0.00")
                            drArrangement("Tax") = Format(Math.Round(.Tax, 2), "#,#0.00")
                            drArrangement("CommissionPerc") = Format(Math.Round(.CommissionPerc, 2), "0.00")
                            drArrangement("Commission") = Format(Math.Round(.CommissionValue, 2), "#,#0.00")
                            drArrangement("CommissionTax") = Format(Math.Round(.CommissionTax, 2), "#,#0.00")
                            drArrangement("Agreement") = .AgreementCode
                            drArrangement("RiOverrideReasonId") = IIf(IsDBNull(.RiOverrideReasonId), 0, .RiOverrideReasonId)
                            drArrangement("DefaultLine") = .DefaultLine
                            dtArrangement.Rows.Add(drArrangement)

                            drArrangement = Nothing
                            'for net line obligatory calculation
                            If .TreatyCode = "Obligatory" Then
                                drArrangement = dtArrangement.NewRow
                                drArrangement("ActionType") = NexusProvider.RowAction.None
                                drArrangement("Name") = GetLocalResourceObject("lbl_NetLine").ToString()
                                drArrangement("RIArrangementKey") = nArrangementId
                                drArrangement("SumInsured") = String.Format("{0:n}", Math.Round(oArrangementType.SumInsured - oArrangementLinesType.SumInsured, 2))
                                drArrangement("Premium") = String.Format("{0:n4}", Math.Round(oArrangementType.Premium - oArrangementLinesType.PremiumValue, 4))
                                dtArrangement.Rows.Add(drArrangement)
                                drArrangement = Nothing
                            End If

                            dSumAssured = dSumAssured + .SumInsured
                            dPremium = dPremium + Math.Round(.PremiumValue, 4)
                            If .TreatyCode <> "Obligatory" Then
                                dAllocatedThisPerc = dAllocatedThisPerc + .ThisPerc
                            End If
                            dSumAssured = dSumAssured + .SumInsured
                            dPremium = dPremium + Math.Round(.PremiumValue, 4)

                            dAllocatedThisPerc = dAllocatedThisPerc + .ThisPerc
                            dAllocatedSumAssured = dAllocatedSumAssured + .SumInsured
                            dAllocatedPremium = dAllocatedPremium + Math.Round(.PremiumValue, 4)
                            dAllocatedTax = dAllocatedTax + .Tax
                            dAllocatedCommision = dAllocatedCommision + .CommissionValue
                            dAllocatedCommisionTax = dAllocatedCommisionTax + .CommissionTax
                            'End If
                        End With
                    Next

                    dSectionThisPercTotal = dAllocatedThisPerc
                    dSectionSumAssuredTotal = dAllocatedSumAssured
                    dSectionPremiumTotal = dAllocatedPremium
                    dSectionCommisionTotal = dAllocatedCommision
                    dSectionTaxTotal = dAllocatedTax
                    dSectionCommisionTaxTotal = dAllocatedCommisionTax

                    'For treaty totals - Currently values will be same as for allocated row
                    drArrangement = dtArrangement.NewRow
                    drArrangement("ActionType") = NexusProvider.RowAction.None
                    drArrangement("RIArrangementKey") = nArrangementId
                    drArrangement("Name") = GetLocalResourceObject("lbl_TreatyTotal").ToString()
                    drArrangement("ThisPerc") = Format(Math.Round(dSectionThisPercTotal, 2), "0.00")
                    drArrangement("SumInsured") = Format(Math.Round(dSectionSumAssuredTotal, 2), "#,#0.00")
                    drArrangement("Premium") = String.Format("{0:n4}", Math.Round(dSectionPremiumTotal, 4))
                    drArrangement("Tax") = Format(Math.Round(dAllocatedTax, 2), "#,#0.00")
                    drArrangement("Commission") = Format(Math.Round(dSectionCommisionTotal, 2), "#,#0.00")
                    drArrangement("CommissionTax") = Format(Math.Round(dSectionCommisionTaxTotal, 2), "#,#0.00")
                    dtArrangement.Rows.Add(drArrangement)
                    drArrangement = Nothing


                    dSectionThisPercTotal = 0
                    dSectionSumAssuredTotal = 0
                    dSectionPremiumTotal = 0
                    dSectionCommisionTotal = 0
                    dSectionTaxTotal = 0
                    dSectionCommisionTaxTotal = 0

                    Dim oReinsuranceArrangmentFacLines = From facLines In oReinsuranceArrangmentCollection Where facLines.Type = "F"
                                                        Select facLines

                    For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentFacLines
                        With oArrangementLinesType
                            drArrangement = dtArrangement.NewRow
                            drArrangement("ActionType") = NexusProvider.RowAction.None
                            drArrangement("PartyKey") = .PartyKey
                            drArrangement("RIArrangementKey") = .RIarrangementKey
                            drArrangement("RIArrangementLineKey") = .RIArrangementLineKey
                            drArrangement("LineType") = .Type
                            drArrangement("Name") = .Name
                            drArrangement("DefaultPerc") = Format(Math.Round(.DefaultPerc, 2), "0.00")
                            drArrangement("ThisPerc") = Format(Math.Round(.ThisPerc, 2), "0.00")
                            drArrangement("SumInsured") = Format(Math.Round(.SumInsured, 2), "#,#0.00")
                            drArrangement("Premium") = String.Format("{0:n4}", Math.Round(.PremiumValue, 4))
                            drArrangement("TaxPerc") = Format(Math.Round(.TaxPerc, 2), "0.00")
                            drArrangement("Tax") = Format(Math.Round(.Tax, 2), "#,#0.00")
                            drArrangement("CommissionPerc") = Format(Math.Round(.CommissionPerc, 2), "0.00")
                            drArrangement("Commission") = Format(Math.Round(.CommissionValue, 2), "#,#0.00")
                            drArrangement("CommissionTax") = Format(Math.Round(.CommissionTax, 2), "#,#0.00")
                            drArrangement("Agreement") = .AgreementCode
                            drArrangement("RiOverrideReasonId") = IIf(IsDBNull(.RiOverrideReasonId), 0, .RiOverrideReasonId)
                            drArrangement("DefaultLine") = 0
                            dtArrangement.Rows.Add(drArrangement)

                            drArrangement = Nothing

                            dSumAssured = dSumAssured + .SumInsured
                            dPremium = dPremium + .PremiumValue

                            dAllocatedThisPerc = dAllocatedThisPerc + .ThisPerc
                            dAllocatedSumAssured = dAllocatedSumAssured + .SumInsured
                            dAllocatedPremium = dAllocatedPremium + .PremiumValue
                            dAllocatedTax = dAllocatedTax + .Tax
                            dAllocatedCommision = dAllocatedCommision + .CommissionValue
                            dAllocatedCommisionTax = dAllocatedCommisionTax + .CommissionTax

                            dSectionThisPercTotal = dSectionThisPercTotal + .ThisPerc
                            dSectionSumAssuredTotal = dSectionSumAssuredTotal + .SumInsured
                            dSectionPremiumTotal = dSectionPremiumTotal + .PremiumValue
                            dSectionCommisionTotal = dSectionCommisionTotal + .CommissionValue
                            dSectionTaxTotal = dSectionTaxTotal + .Tax
                            dSectionCommisionTaxTotal = dSectionCommisionTaxTotal + .CommissionTax
                        End With
                    Next

                    'For fac totals - all values will be 0
                    drArrangement = dtArrangement.NewRow
                    drArrangement("ActionType") = NexusProvider.RowAction.None
                    drArrangement("Name") = GetLocalResourceObject("lbl_FacTotal").ToString()
                    drArrangement("ThisPerc") = Format(Math.Round(dSectionThisPercTotal, 2), "0.00")
                    drArrangement("SumInsured") = Format(Math.Round(dSectionSumAssuredTotal, 2), "#,#0.00")
                    drArrangement("Premium") = String.Format("{0:n4}", Math.Round(dSectionPremiumTotal, 4))
                    drArrangement("Tax") = Format(Math.Round(dSectionTaxTotal, 2), "#,#0.00")
                    drArrangement("Commission") = Format(Math.Round(dSectionCommisionTotal, 2), "#,#0.00")
                    drArrangement("CommissionTax") = Format(Math.Round(dSectionCommisionTaxTotal, 2), "#,#0.00")
                    dtArrangement.Rows.Add(drArrangement)
                    drArrangement = Nothing

                    If oArrangementType.IsOriginal Then
                        dtCurrentArrangement = dsArrangements.Tables("Arrangement_Current_" & oArrangementType.BandId)
                        If dtCurrentArrangement IsNot Nothing Then
                            drArrangement = dtCurrentArrangement.NewRow
                            drArrangement("ActionType") = NexusProvider.RowAction.None
                            drArrangement("Name") = GetLocalResourceObject("lbl_OriginalRITotals")
                            drArrangement("SumInsured") = Format(Math.Round(oArrangementType.SumInsured, 2), "#,#0.00")
                            drArrangement("Premium") = String.Format("{0:n4}", Math.Round(oArrangementType.Premium, 4))
                            dtCurrentArrangement.Rows.Add(drArrangement)

                            drArrangement = Nothing

                            'Net
                            If Session(CNMode) <> Mode.View Then
                                Dim drCurrentBandTotal As DataRow
                                drCurrentBandTotal = dtCurrentArrangement.Select("name='" & GetLocalResourceObject("lbl_BandTotal").ToString() & "'")(0)

                                drArrangement = dtCurrentArrangement.NewRow
                                drArrangement("ActionType") = NexusProvider.RowAction.None
                                drArrangement("Name") = GetLocalResourceObject("lbl_NetTotals")
                                drArrangement("SumInsured") = Format(Math.Round(oArrangementType.SumInsured + Convert.ToDecimal(drCurrentBandTotal("SumInsured")), 2), "#,#0.00")
                                drArrangement("Premium") = String.Format("{0:n4}", Math.Round(oArrangementType.Premium + Convert.ToDecimal(drCurrentBandTotal("Premium")), 4))

                                dtCurrentArrangement.Rows.Add(drArrangement)
                                drArrangement = Nothing
                                drCurrentBandTotal = Nothing
                            End If
                            dtCurrentArrangement.AcceptChanges()
                        End If

                        dsArrangements.AcceptChanges()
                    Else
                        If Not (Session(CNMode) = Mode.View Or Session(CNRiskMode) = RiskMode.View) Then
                            drArrangement = dtArrangement.NewRow
                            drArrangement("ActionType") = NexusProvider.RowAction.None
                            drArrangement("Name") = GetLocalResourceObject("lbl_Allocated")
                            drArrangement("ThisPerc") = Format(Math.Round(dAllocatedThisPerc, 2), "0.00")
                            drArrangement("SumInsured") = Format(Math.Round(dAllocatedSumAssured, 2), "#,#0.00")
                            drArrangement("Premium") = String.Format("{0:n4}", Math.Round(dAllocatedPremium, 4))
                            drArrangement("Tax") = Format(Math.Round(dAllocatedTax, 2), "#,#0.00")
                            drArrangement("Commission") = Format(Math.Round(dAllocatedCommision, 2), "#,#0.00")
                            drArrangement("CommissionTax") = Format(Math.Round(dAllocatedCommisionTax, 2), "#,#0.00")
                            dtArrangement.Rows.Add(drArrangement)
                            drArrangement = Nothing
                        End If
                    End If

                    'Row for Unallocated Amount
                    If oArrangementType.IsOriginal = False Then
                        If (Math.Round(oArrangementType.SumInsured, 2) <> Math.Round(dAllocatedSumAssured, 2) Or Math.Round(oArrangementType.Premium, 2) <> Math.Round(dAllocatedPremium, 2)) Then
                            drArrangement = dtArrangement.NewRow
                            drArrangement("ActionType") = NexusProvider.RowAction.None
                            drArrangement("Name") = GetLocalResourceObject("lbl_UnAllocated").ToString()
                            drArrangement("ThisPerc") = Format(Math.Round(100.0 - dAllocatedThisPerc, 2), "0.00")
                            drArrangement("SumInsured") = Format(Math.Round(oArrangementType.SumInsured - dAllocatedSumAssured, 2), "#,#0.00")
                            drArrangement("Premium") = String.Format("{0:n4}", Math.Round(oArrangementType.Premium - dAllocatedPremium, 4))
                            dtArrangement.Rows.Add(drArrangement)

                            drArrangement = Nothing
                            ViewState("IsAllocated_" & oArrangementType.BandId) = False
                        Else
                            ViewState("IsAllocated_" & oArrangementType.BandId) = True
                        End If
                    End If

                    dsArrangements.Tables.Add(dtArrangement)
                    dsArrangements.AcceptChanges()

                Next

                Session(CNRIData) = dsArrangements

            Finally
                oWebService = Nothing
            End Try
        End Sub
        ''' <summary>
        ''' this call the SAM to get the TAX percentage
        ''' </summary>
        ''' <param name="dPremium"></param>
        ''' <param name="nPartyKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetTaxPercentage(ByVal dPremium As Double, ByVal nPartyKey As Integer) As Double
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim nRiskKey As Integer = 0
            Dim nInsuranceFileKey As Integer = 0
            Dim dTaxPercentage As Double = 0

            nRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key
            nInsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
            oWebService.CalculateRITax(nRiskKey, Convert.ToInt32(nPartyKey),
                               Convert.ToDouble(dPremium), nInsuranceFileKey, dTaxPercentage)

            Return dTaxPercentage
        End Function

        ''' <summary>
        ''' To Populate the RI Grid
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateRIGrid(ByVal v_bIgnoreOverrideReason As Boolean)
            Dim dsArrangements As DataSet
            Dim sCurrentDataTableName As String = String.Empty
            Dim sOriginalDataTableName As String = String.Empty

            If TryCast(Session(CNRIData), DataSet) Is Nothing Then
                GetDefaultRIData()
            End If

            dsArrangements = TryCast(Session(CNRIData), DataSet)
            sCurrentDataTableName = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            sOriginalDataTableName = "Arrangement_Original_" & ddlReinsurance.SelectedValue

            If dsArrangements.Tables(sCurrentDataTableName) IsNot Nothing AndAlso dsArrangements.Tables(sCurrentDataTableName).Rows.Count > 0 Then
                Dim dvNonDeletedArrangement As DataView = dsArrangements.Tables(sCurrentDataTableName).DefaultView
                dvNonDeletedArrangement.RowFilter = "ActionType<>" & NexusProvider.RowAction.DeleteRow
                With dsArrangements.Tables(sCurrentDataTableName)
                    If .Rows IsNot Nothing AndAlso Not String.IsNullOrEmpty(Convert.ToString(.Rows(0).Item("RiOverrideReasonId"))) AndAlso Not IsDBNull(.Rows(0).Item("RiOverrideReasonId")) AndAlso
                        .Rows(0).Item("RiOverrideReasonId") IsNot Nothing AndAlso Convert.ToInt32(dsArrangements.Tables(sCurrentDataTableName).Rows(0).Item("RiOverrideReasonId")) <> 0 Then
                        GISLookup_RIOverrideReasonOrig.Value = Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId"))
                        lblRIOverrideReason.Visible = True
                        GISLookup_RIOverrideReason.Visible = True
                        uplDetail.Update()
                    Else
                        If Not v_bIgnoreOverrideReason Then
                            lblRIOverrideReason.Visible = False
                            GISLookup_RIOverrideReason.Visible = False
                            uplDetail.Update()
                        End If
                    End If
                End With
                gvReinsurance.DataSource = dvNonDeletedArrangement
                gvReinsurance.DataBind()
            End If

            If dsArrangements.Tables(sOriginalDataTableName) IsNot Nothing AndAlso dsArrangements.Tables(sOriginalDataTableName).Rows.Count > 0 Then
                lblOrgReinsurance.Visible = True
                gvOrgReinsurance.DataSource = dsArrangements.Tables(sOriginalDataTableName)
                gvOrgReinsurance.DataBind()

                With dsArrangements.Tables(sOriginalDataTableName)
                    If .Rows IsNot Nothing AndAlso Not String.IsNullOrEmpty(Convert.ToString(.Rows(0).Item("RiOverrideReasonId"))) AndAlso Not IsDBNull(.Rows(0).Item("RiOverrideReasonId")) AndAlso
                        .Rows(0).Item("RiOverrideReasonId") IsNot Nothing AndAlso Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId")) <> 0 Then
                        GISLookup_RIOverrideReasonOrig.Value = Convert.ToInt32(.Rows(0).Item("RiOverrideReasonId"))
                        lblOrigRiOverrideResason.Visible = True
                        GISLookup_RIOverrideReasonOrig.Visible = True

                    Else
                        lblOrigRiOverrideResason.Visible = False
                        GISLookup_RIOverrideReasonOrig.Visible = False

                    End If
                    UpdatePanelReinsurance.Update()
                End With
                GISLookup_RIOverrideReasonOrig.Enabled = False
            Else
                lblOrgReinsurance.Visible = False
            End If

        End Sub


        ''' <summary>
        ''' On load of original RI Grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvOrgReinsurance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOrgReinsurance.Load
            If gvOrgReinsurance.PageCount = 1 Then
                gvOrgReinsurance.AllowPaging = False
            End If
        End Sub


        ''' <summary>
        ''' On row data bound for RI grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvReinsurance_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvReinsurance.RowDataBound
            e.Row.Cells(11).Visible = False
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim txtSumInsured As TextBox = e.Row.FindControl("txtSumInsured")
                Dim lblSumInsured As Label = e.Row.FindControl("lblSumInsured")

                Dim txtPremium As TextBox = e.Row.FindControl("txtPremium")
                Dim lblPremium As Label = e.Row.FindControl("lblPremium")

                Dim txtCommissionPerc As TextBox = e.Row.FindControl("txtCommissionPerc")
                Dim lblCommissionPerc As Label = e.Row.FindControl("lblCommissionPerc")
                'Dim rngCommissionPerc As RangeValidator = e.Row.FindControl("rngCommissionPerc")

                Dim txtAgreement As TextBox = e.Row.FindControl("txtAgreement")
                Dim lblAgreement As Label = e.Row.FindControl("lblAgreement")

                Dim lnkDelete As LinkButton = e.Row.FindControl("lnkDelete")

                If Session(CNMode) = Mode.View Or Session(CNMode) = Mode.Edit Then
                    lblSumInsured.Visible = True
                    lblPremium.Visible = True
                    lblCommissionPerc.Visible = True
                    lblAgreement.Visible = True
                Else
                    If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "LineType")) Then
                        If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "DefaultLine")) And Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DefaultLine")) = "0" Then
                            lnkDelete.Visible = True
                            lnkDelete.Attributes.Add("onclick", "return confirm('Do you really wish to Delete this Reinsurance Placement?');")
                        End If
                        txtAgreement.Visible = True
                        txtCommissionPerc.Visible = True
                        txtPremium.Visible = True
                        txtSumInsured.Visible = True
                    End If
                End If
                If txtSumInsured.Text = "NaN" OrElse txtSumInsured.Text = "NaN.Na" Then
                    txtSumInsured.Text = "0"
                End If
                If e.Row.Cells(3).Text = "NaN" Then
                    e.Row.Cells(3).Text = "0"
                End If
                If e.Row.Cells(4).Text = "NaN" Then
                    e.Row.Cells(4).Text = "0"
                End If


                If Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_BandTotal").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_Allocated").ToString() Or
                    Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_UnAllocated").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_TreatyTotal").ToString() Or
                    Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_FacTotal").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_OriginalRITotals").ToString() Or
                    Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_NetTotals").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_NetLine").ToString() Then

                    If Not (Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_OriginalRITotals").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_NetTotals").ToString()) Then
                        e.Row.CssClass = "summary"
                    End If

                    lblSumInsured.Visible = True
                    lblPremium.Visible = True

                    lblAgreement.Visible = True
                End If
                If Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "LineType")) AndAlso DataBinder.Eval(e.Row.DataItem, "LineType") = "R" Then
                    txtCommissionPerc.Visible = False
                    txtAgreement.Visible = False
                    lblAgreement.Visible = True
                    lblCommissionPerc.Visible = True
                End If

                If CheckRiManualPremiumAdjustment.Contains("0") AndAlso Not IsDBNull(DataBinder.Eval(e.Row.DataItem, "LineType")) Then
                    txtPremium.ReadOnly = True
                End If

            End If
        End Sub

        ''' <summary>
        ''' On row data bound for original RI grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvOrgReinsurance_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvOrgReinsurance.RowDataBound
            If Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_BandTotal").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_Allocated").ToString() Or
                Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_UnAllocated").ToString() Or Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_TreatyTotal").ToString() Or
                Trim(e.Row.Cells(0).Text) = GetLocalResourceObject("lbl_FacTotal").ToString() Then

                e.Row.CssClass = "summary"
            End If
        End Sub

        ''' <summary>
        ''' On change of value for Premium in RI Line
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub txtPremium_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim nRIArrangementLineKey As Integer
            Dim nPartyKey As Integer = 0
            Dim sTreatyCode As String = ""
            Dim dsArrangements As DataSet
            Dim drArrangement As DataRow
            Dim drBandTotal As DataRow
            Dim drAllocated As DataRow
            Dim drUnAllocated As DataRow
            Dim drSectionTotal As DataRow
            Dim dtRIArrangement As DataTable

            Dim dCommissionPercent, dTaxPercent As Decimal

            Dim dTotalSumInsured As Decimal
            Dim dTotalPremium As Decimal

            Dim dNewSumInsured As Decimal
            Dim dNewPremium As Decimal
            Dim dNewThisPercent As Decimal
            Dim dNewCommissionValue As Decimal = 0
            Dim dNewCommissionTaxValue As Decimal = 0
            Dim dNewTaxValue As Decimal = 0

            Dim dChangedSumInsured As Decimal = 0
            Dim dChangedPremium As Decimal = 0
            Dim dChangedCommissionValue As Decimal = 0
            Dim dChangedCommissionTaxValue As Decimal = 0
            Dim dChangedTaxValue As Decimal = 0
            Dim dChangedThisPercent As Decimal = 0

            Dim dAllocatedThisPercent As Decimal = 0
            Dim dAllocatedPremium As Decimal = 0
            Dim dAllocatedSumInsured As Decimal = 0
            Dim dAllocatedCommission As Decimal = 0
            Dim dAllocatedTax As Decimal = 0
            Dim dAllocatedCommissionTax As Decimal = 0

            Dim dUnAllocatedThisPercent As Decimal = 0
            Dim dUnAllocatedPremium As Decimal = 0
            Dim dUnAllocatedSumInsured As Decimal = 0

            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            Dim txtPremium As TextBox = CType(sender, TextBox)
            Dim gvrReinsurance As GridViewRow = CType(txtPremium.NamingContainer, GridViewRow)

            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1) <> 0 Then
                nPartyKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1), Integer)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2) <> "" Then
                sTreatyCode = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2), String)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0)) Then
                nRIArrangementLineKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0), Integer)
            End If

            dsArrangements = TryCast(Session(CNRIData), DataSet)
            dtRIArrangement = dsArrangements.Tables(sCurrentDataTableName)

            If nRIArrangementLineKey <> 0 Then
                drArrangement = dtRIArrangement.Select("RIArrangementLineKey=" & nRIArrangementLineKey.ToString())(0)
            Else
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    drArrangement = dtRIArrangement.Select("TreatyCode='" & sTreatyCode.ToString() & "'")(0)
                Else
                    drArrangement = dtRIArrangement.Select("PartyKey=" & nPartyKey.ToString())(0)
                End If
            End If

            If String.IsNullOrEmpty(txtPremium.Text) OrElse (Not IsNumeric(txtPremium.Text)) Then
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidPremiumAmount").ToString() & "');", True)
                If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                    PopulateRIGrid(False)
                Else
                    PopulateRIGrid(True)
                End If
                Exit Sub
            End If

            If txtPremium.Text = Convert.ToDecimal(drArrangement("Premium")) Then
                Exit Sub
            End If
            'Recalculate the values for current row
            drBandTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_BandTotal").ToString() & "'")(0)
            dTotalSumInsured = Convert.ToDecimal(drBandTotal("SumInsured"))
            dTotalPremium = Convert.ToDecimal(drBandTotal("Premium"))

            dCommissionPercent = Convert.ToDecimal(drArrangement("CommissionPerc").ToString().Replace("%", ""))
            dNewPremium = Math.Round(Convert.ToDecimal(txtPremium.Text), 2)
            dNewCommissionValue = Math.Round(((dNewPremium * dCommissionPercent) / 100), 2)
            Dim dTaxComm As Decimal = 0
            Dim dTaxPrem As Decimal = 0

            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                RecalculateTaxes(v_sCode:=drArrangement("TreatyCode"), v_nKey:=0, v_sLineType:=drArrangement("LineType"), v_dPremium:=dNewPremium, v_nRILineID:=nRIArrangementLineKey, v_dCommission:=dNewCommissionValue, v_bIgnoreDetails:=True, v_bIgnoreTax:=False, r_dCommissionPercent:=0, r_dCommTax:=dTaxComm, r_dPremTax:=dTaxPrem, r_nIsRetained:=0, r_sAgreementCode:="")
            Else
                If nPartyKey <> 0 Then
                    dTaxPercent = GetTaxPercentage(dPremium:=dNewPremium, nPartyKey:=nPartyKey)
                Else
                    dTaxPercent = Convert.ToDecimal(drArrangement("TaxPerc").ToString())
                End If
            End If

            If dTotalPremium <> 0 Then
                dNewThisPercent = (dNewPremium * 100) / dTotalPremium
            End If
            dNewSumInsured = Convert.ToDecimal(drArrangement("SumInsured"))
            dNewCommissionValue = Math.Round(((dNewPremium * dCommissionPercent) / 100), 2)

            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                dNewCommissionTaxValue = dTaxComm ' Math.Round(((dNewCommissionValue * dTaxCommPerc) / 100), 2)
                dNewTaxValue = dTaxPrem
            Else
                dNewCommissionTaxValue = Math.Round(((dNewCommissionValue * dTaxPercent) / 100), 2)
                dNewTaxValue = Math.Round(((dNewPremium * dTaxPercent) / 100), 2)
            End If

            dChangedSumInsured = dNewSumInsured - Convert.ToDecimal(drArrangement("SumInsured"))
            dChangedPremium = dNewPremium - Convert.ToDecimal(drArrangement("Premium"))
            dChangedCommissionValue = dNewCommissionValue - Convert.ToDecimal(drArrangement("Commission"))
            dChangedCommissionTaxValue = dNewCommissionTaxValue - Convert.ToDecimal(drArrangement("CommissionTax"))
            dChangedTaxValue = dNewTaxValue - Convert.ToDecimal(drArrangement("Tax"))
            dChangedThisPercent = dNewThisPercent - Convert.ToDecimal(drArrangement("ThisPerc"))

            drArrangement("SumInsured") = Format(Math.Round(dNewSumInsured, 2), "#,#0.00")
            drArrangement("Premium") = Format(Math.Round(dNewPremium, 2), "#,#0.00")
            drArrangement("Commission") = Format(Math.Round(dNewCommissionValue, 2), "#,#0.00")
            drArrangement("CommissionTax") = Format(Math.Round(dNewCommissionTaxValue, 2), "#,#0.00")
            drArrangement("Tax") = Format(Math.Round(dNewTaxValue, 2), "#,#0.00")
            drArrangement("ThisPerc") = Format(Math.Round(dNewThisPercent, 2), "0.00")

            If nRIArrangementLineKey <> 0 Then
                drArrangement("ActionType") = NexusProvider.RowAction.EditRow
            End If

            'Recalculate the values for Fac or treaty Total
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then ' Treaty
                drSectionTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_TreatyTotal").ToString() & "'")(0)
            Else 'Fac Prop
                drSectionTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_FacTotal").ToString() & "'")(0)
            End If
            drSectionTotal("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("ThisPerc")) + dChangedThisPercent, 2), "0.00")
            drSectionTotal("SumInsured") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("SumInsured")) + dChangedSumInsured, 2), "#,#0.00")
            drSectionTotal("Premium") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Premium")) + dChangedPremium, 2), "#,#0.00")
            drSectionTotal("Tax") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Tax") + dChangedTaxValue), 2), "#,#0.00")
            drSectionTotal("Commission") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Commission") + dChangedCommissionValue), 2), "#,#0.00")
            drSectionTotal("CommissionTax") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("CommissionTax") + dChangedCommissionTaxValue), 2), "#,#0.00")

            'Recalculate the values for allocated row
            drAllocated = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_Allocated").ToString() & "'")(0)
            drAllocated("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drAllocated("ThisPerc")) + dChangedThisPercent, 2), "0.00")
            drAllocated("SumInsured") = Format(Math.Round(Convert.ToDecimal(drAllocated("SumInsured")) + dChangedSumInsured, 2), "#,#0.00")
            drAllocated("Premium") = Format(Math.Round(Convert.ToDecimal(drAllocated("Premium")) + dChangedPremium, 2), "#,#0.00")
            drAllocated("Commission") = Format(Math.Round(Convert.ToDecimal(drAllocated("Commission")) + dChangedCommissionValue, 2), "#,#0.00")
            drAllocated("CommissionTax") = Format(Math.Round(Convert.ToDecimal(drAllocated("CommissionTax")) + dChangedCommissionTaxValue, 2), "#,#0.00")
            drAllocated("Tax") = Format(Math.Round(Convert.ToDecimal(drAllocated("Tax")) + dChangedTaxValue, 2), "#,#0.00")

            'Recalculate the values from UnAllocated row
            If dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_UnAllocated").ToString() & "'").Length > 0 Then
                drUnAllocated = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_UnAllocated").ToString() & "'")(0)

                dUnAllocatedThisPercent = Convert.ToDecimal(drUnAllocated("ThisPerc"))
                dUnAllocatedPremium = Convert.ToDecimal(drUnAllocated("Premium"))
                dUnAllocatedSumInsured = Convert.ToDecimal(drUnAllocated("SumInsured"))
                'Update existing row
                If dUnAllocatedPremium - dChangedPremium <> 0 OrElse (dTotalSumInsured - Convert.ToDecimal(drAllocated("SumInsured"))) <> 0 OrElse (dTotalPremium - Convert.ToDecimal(drAllocated("Premium"))) <> 0 Then
                    drUnAllocated("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("ThisPerc")) - dChangedThisPercent, 2), "0.00")
                    drUnAllocated("SumInsured") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("SumInsured")) - dChangedSumInsured, 2), "#,#0.00")
                    drUnAllocated("Premium") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("Premium")) - dChangedPremium, 2), "#,#0.00")
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = False
                Else
                    dtRIArrangement.Rows.Remove(drUnAllocated)
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = True
                End If
            Else
                drUnAllocated = dtRIArrangement.NewRow
                drUnAllocated("ActionType") = NexusProvider.RowAction.None
                drUnAllocated("Name") = GetLocalResourceObject("lbl_UnAllocated").ToString()
                drUnAllocated("ThisPerc") = Format(Math.Round(dChangedThisPercent * -1, 2), "0.00")
                drUnAllocated("SumInsured") = Format(Math.Round(dChangedSumInsured * -1, 2), "#,#0.00")
                drUnAllocated("Premium") = Format(Math.Round(dChangedPremium * -1, 2), "#,#0.00")
                If Session(CNMTAType) IsNot Nothing Then
                    dtRIArrangement.Rows.InsertAt(drUnAllocated, dtRIArrangement.Rows.Count - 2)
                Else
                    dtRIArrangement.Rows.Add(drUnAllocated)
                End If

                drUnAllocated = Nothing
                ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = False

            End If
            dtRIArrangement.AcceptChanges()
            dsArrangements.AcceptChanges()

            Session(CNRIData) = dsArrangements
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                PopulateRIGrid(False)
                PopulateRiOverrideReason(True)
            Else
                PopulateRIGrid(True)
            End If

        End Sub

        ''' <summary>
        ''' On change of value for Commission Percentage in RI Line
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub txtCommissionPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim value As String = CType(sender, TextBox).Text

            Dim nRIArrangementLineKey As Integer
            Dim nPartyKey As Integer = 0
            Dim sTreatyCode As String = ""
            Dim dsArrangements As DataSet
            Dim drArrangement As DataRow
            Dim drAllocated As DataRow
            Dim drSectionTotal As DataRow
            Dim dtRIArrangement As DataTable

            Dim dCommissionPercent As Decimal = 0
            Dim dPremium As Decimal
            Dim dAllocatedCommission As Decimal = 0
            Dim dAllocatedTax As Decimal = 0
            Dim dAllocatedCommissionTax As Decimal = 0

            Dim dChangedCommissionValue As Decimal = 0
            Dim dNewCommissionValue As Decimal = 0

            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue

            Dim txtCommissionPerc As TextBox = CType(sender, TextBox)

            If InStr(1, value, "%") <> 0 Then
                value = value.Substring(0, value.Trim.Length - 1)
            End If

            Dim gvrReinsurance As GridViewRow = CType(txtCommissionPerc.NamingContainer, GridViewRow)

            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1) <> 0 Then
                nPartyKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1), Integer)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2) <> "" Then
                sTreatyCode = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2), String)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0)) Then
                nRIArrangementLineKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0), Integer)
            End If

            dCommissionPercent = Math.Round(Convert.ToDecimal(txtCommissionPerc.Text.Replace("%", "")), 2)
            dsArrangements = TryCast(Session(CNRIData), DataSet)
            dtRIArrangement = dsArrangements.Tables(sCurrentDataTableName)

            If nRIArrangementLineKey <> 0 Then
                drArrangement = dtRIArrangement.Select("RIArrangementLineKey=" & nRIArrangementLineKey.ToString())(0)
            Else
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    drArrangement = dtRIArrangement.Select("TreatyCode='" & sTreatyCode.ToString() & "'")(0)
                Else
                    drArrangement = dtRIArrangement.Select("PartyKey=" & nPartyKey.ToString())(0)
                End If

            End If
            If String.IsNullOrEmpty(value) OrElse (Not IsNumeric(value)) Then
                ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "alert('Commission percentage must be a valid numeric between 0 and 100');", True)
                If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                    PopulateRIGrid(False)
                Else
                    PopulateRIGrid(True)
                End If
                Exit Sub
            ElseIf Convert.ToDouble(value) > 100 Or Convert.ToDouble(value) < 0 Then
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("err_InvaliRangeComm").ToString() & "');", True)
                If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                    PopulateRIGrid(False)
                Else
                    PopulateRIGrid(True)
                End If
                Exit Sub
            End If
            dPremium = Convert.ToDecimal(drArrangement("Premium").ToString())

            'Recalculate the values for current row
            drArrangement("CommissionPerc") = Format(dCommissionPercent, "0.00")
            'drArrangement("CommissionPerc") = String.Format("{0}{1}", Math.Round(dCommissionPercent, 2), "%")
            drAllocated = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_Allocated").ToString() & "'")(0)

            dAllocatedCommission = Convert.ToDecimal(drArrangement("Commission"))
            dNewCommissionValue = Math.Round((dPremium * dCommissionPercent / 100), 2)

            Dim dTaxComm As Decimal = 0
            Dim dTaxPrem As Decimal = 0
            Dim dTaxPercent As Decimal = 0
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                RecalculateTaxes(v_sCode:=drArrangement("TreatyCode"), v_nKey:=0, v_sLineType:=drArrangement("LineType"), v_dPremium:=dPremium, v_nRILineID:=nRIArrangementLineKey, v_dCommission:=dNewCommissionValue, v_bIgnoreDetails:=True, v_bIgnoreTax:=False, r_dCommissionPercent:=0, r_dCommTax:=dTaxComm, r_dPremTax:=dTaxPrem, r_nIsRetained:=0, r_sAgreementCode:="")
            Else
                If nPartyKey <> 0 Then
                    dTaxPercent = GetTaxPercentage(dPremium:=dPremium, nPartyKey:=nPartyKey)
                Else
                    dTaxPercent = Convert.ToDecimal(drArrangement("TaxPerc").ToString())
                End If
            End If

            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                drArrangement("CommissionTax") = Format(dTaxComm, "#,#0.00")
            Else
                drArrangement("CommissionTax") = Math.Round(((dNewCommissionValue * dTaxPercent) / 100), 2)
            End If

            dChangedCommissionValue = dNewCommissionValue - dAllocatedCommission

            drArrangement("Commission") = Format(dNewCommissionValue, "#,#0.00")
            If nRIArrangementLineKey <> 0 Then
                drArrangement("ActionType") = NexusProvider.RowAction.EditRow
            End If

            'Recalculate the values for Fac or treaty Total
            If drArrangement("LineType") = "T" Then ' Treaty
                drSectionTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_TreatyTotal").ToString() & "'")(0)
            Else 'Fac Prop
                drSectionTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_FacTotal").ToString() & "'")(0)
            End If
            drSectionTotal("Commission") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Commission")) + dChangedCommissionValue, 2), "#,#0.00")

            'Recalculate the values for allocated row
            drAllocated("Commission") = Format(Math.Round(Convert.ToDecimal(drAllocated("Commission")) + dChangedCommissionValue, 2), "#,#0.00")

            dtRIArrangement.AcceptChanges()
            dsArrangements.AcceptChanges()

            Session(CNRIData) = dsArrangements
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                PopulateRIGrid(False)
                PopulateRiOverrideReason(True)
            Else
                PopulateRIGrid(True)
            End If

        End Sub

        ''' <summary>
        ''' On change of value for Agreement in RI Line
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub txtAgreement_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim nRIArrangementLineKey As Integer
            Dim nPartyKey As Integer = 0
            Dim sTreatyCode As String = ""
            Dim dsArrangements As DataSet
            Dim drArrangement As DataRow
            Dim dtRIArrangement As DataTable

            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            Dim txtAgreement As TextBox = CType(sender, TextBox)
            Dim gvrReinsurance As GridViewRow = CType(txtAgreement.NamingContainer, GridViewRow)

            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1) <> 0 Then
                nPartyKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1), Integer)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2) <> "" Then
                sTreatyCode = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2), String)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0)) Then
                nRIArrangementLineKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0), Integer)
            End If

            dsArrangements = TryCast(Session(CNRIData), DataSet)
            dtRIArrangement = dsArrangements.Tables(sCurrentDataTableName)

            If nRIArrangementLineKey <> 0 Then
                drArrangement = dtRIArrangement.Select("RIArrangementLineKey=" & nRIArrangementLineKey.ToString())(0)
            Else
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    drArrangement = dtRIArrangement.Select("TreatyCode='" & sTreatyCode.ToString() & "'")(0)
                Else
                    drArrangement = dtRIArrangement.Select("PartyKey=" & nPartyKey.ToString())(0)
                End If
            End If

            'Recalculate the values for current row
            drArrangement("Agreement") = txtAgreement.Text

            If nRIArrangementLineKey <> 0 Then
                drArrangement("ActionType") = NexusProvider.RowAction.EditRow
            End If


            dtRIArrangement.AcceptChanges()
            dsArrangements.AcceptChanges()

            Session(CNRIData) = dsArrangements
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                PopulateRIGrid(False)
                PopulateRiOverrideReason(True)
            Else
                PopulateRIGrid(True)
            End If

        End Sub

        ''' <summary>
        ''' On change of value for SumInsured in RI Line
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub txtSumInsured_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim nRIArrangementLineKey As Integer
            Dim nPartyKey As Integer = 0

            Dim dsArrangements As DataSet
            Dim drArrangement As DataRow
            Dim drBandTotal As DataRow
            Dim drAllocated As DataRow
            Dim drUnAllocated As DataRow
            Dim drSectionTotal As DataRow
            Dim dtRIArrangement As DataTable

            Dim dCommissionPercent As Decimal
            Dim dTaxPercent As Decimal

            Dim dTotalSumInsured As Decimal
            Dim dTotalPremium As Decimal

            Dim dNewSumInsured As Decimal
            Dim dNewPremium As Decimal
            Dim dNewThisPercent As Decimal
            Dim dNewCommissionValue As Decimal = 0
            Dim dNewCommissionTaxValue As Decimal = 0
            Dim dNewTaxValue As Decimal = 0

            Dim dChangedSumInsured As Decimal = 0
            Dim dChangedPremium As Decimal = 0
            Dim dChangedCommissionValue As Decimal = 0
            Dim dChangedCommissionTaxValue As Decimal = 0
            Dim dChangedTaxValue As Decimal = 0
            Dim dChangedThisPercent As Decimal = 0

            Dim dAllocatedThisPercent As Decimal = 0
            Dim dAllocatedPremium As Decimal = 0
            Dim dAllocatedSumInsured As Decimal = 0
            Dim dAllocatedCommission As Decimal = 0
            Dim dAllocatedTax As Decimal = 0
            Dim dAllocatedCommissionTax As Decimal = 0

            Dim dUnAllocatedThisPercent As Decimal = 0
            Dim dUnAllocatedPremium As Decimal = 0
            Dim dUnAllocatedSumInsured As Decimal = 0

            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            Dim txtSumInsured As TextBox = CType(sender, TextBox)
            Dim gvrReinsurance As GridViewRow = CType(txtSumInsured.NamingContainer, GridViewRow)

            Dim sTreatyCode As String = ""

            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1) <> 0 Then
                nPartyKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(1), Integer)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2)) AndAlso gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2) <> "" Then
                sTreatyCode = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(2), String)
            End If
            If Not IsDBNull(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0)) Then
                nRIArrangementLineKey = CType(gvReinsurance.DataKeys(gvrReinsurance.RowIndex).Values(0), Integer)
            End If

            dsArrangements = TryCast(Session(CNRIData), DataSet)
            dtRIArrangement = dsArrangements.Tables(sCurrentDataTableName)

            If nRIArrangementLineKey <> 0 Then
                drArrangement = dtRIArrangement.Select("RIArrangementLineKey=" & nRIArrangementLineKey.ToString())(0)
            Else
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    drArrangement = dtRIArrangement.Select("TreatyCode='" & sTreatyCode.ToString() & "'")(0)
                Else
                    drArrangement = dtRIArrangement.Select("PartyKey=" & nPartyKey.ToString())(0)
                End If

            End If
            If String.IsNullOrEmpty(txtSumInsured.Text) OrElse (Not IsNumeric(txtSumInsured.Text)) Then
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_InvalidSumInsuredAmount").ToString() & "');", True)
                If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                    PopulateRIGrid(False)
                Else
                    PopulateRIGrid(True)
                End If
                Exit Sub
            End If
            'Recalculate the values for current row
            drBandTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_BandTotal").ToString() & "'")(0)
            dTotalSumInsured = Convert.ToDecimal(drBandTotal("SumInsured"))
            dTotalPremium = Convert.ToDecimal(drBandTotal("Premium"))

            dCommissionPercent = Convert.ToDecimal(drArrangement("CommissionPerc").ToString().Replace("%", ""))

            dNewSumInsured = Math.Round(Convert.ToDecimal(txtSumInsured.Text), 2)
            dNewThisPercent = (dNewSumInsured * 100) / dTotalSumInsured
            dNewPremium = Math.Round(((dTotalPremium * dNewThisPercent) / 100), 4)
            dNewCommissionValue = Math.Round(((dNewPremium * dCommissionPercent) / 100), 2)

            Dim dTaxComm As Decimal = 0
            Dim dTaxPrem As Decimal = 0
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                RecalculateTaxes(v_sCode:=drArrangement("TreatyCode"), v_nKey:=0, v_sLineType:=drArrangement("LineType"), v_dPremium:=dNewPremium, v_nRILineID:=nRIArrangementLineKey, v_dCommission:=dNewCommissionValue, v_bIgnoreDetails:=True, v_bIgnoreTax:=False, r_dCommissionPercent:=0, r_dCommTax:=dTaxComm, r_dPremTax:=dTaxPrem, r_nIsRetained:=0, r_sAgreementCode:="")
            Else
                If nPartyKey <> 0 Then
                    dTaxPercent = GetTaxPercentage(dPremium:=dNewPremium, nPartyKey:=nPartyKey)
                Else
                    dTaxPercent = Convert.ToDecimal(drArrangement("TaxPerc").ToString())
                End If
            End If

            dNewCommissionValue = Math.Round(((dNewPremium * dCommissionPercent) / 100), 2)

            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                dNewCommissionTaxValue = dTaxComm ' Math.Round(((dNewCommissionValue * dTaxCommPerc) / 100), 2)
                dNewTaxValue = dTaxPrem
            Else
                dNewCommissionTaxValue = Math.Round(((dNewCommissionValue * dTaxPercent) / 100), 2)
                dNewTaxValue = Math.Round(((dNewPremium * dTaxPercent) / 100), 2)
            End If
            'dNewCommissionTaxValue = Math.Round(((dNewCommissionValue * dTaxPercent) / 100), 2)

            dChangedSumInsured = dNewSumInsured - Convert.ToDecimal(drArrangement("SumInsured"))
            dChangedPremium = dNewPremium - Convert.ToDecimal(drArrangement("Premium"))
            dChangedCommissionValue = dNewCommissionValue - Convert.ToDecimal(drArrangement("Commission"))
            dChangedCommissionTaxValue = dNewCommissionTaxValue - Convert.ToDecimal(drArrangement("CommissionTax"))
            dChangedTaxValue = dNewTaxValue - Convert.ToDecimal(drArrangement("Tax"))
            dChangedThisPercent = dNewThisPercent - Convert.ToDecimal(drArrangement("ThisPerc"))

            drArrangement("SumInsured") = Format(dNewSumInsured, "#,#0.00")
            drArrangement("Premium") = String.Format("{0:n4}", Math.Round(dNewPremium, 4))
            drArrangement("Commission") = Format(Math.Round(dNewCommissionValue, 2), "#,#0.00")
            drArrangement("CommissionTax") = Format(Math.Round(dNewCommissionTaxValue, 2), "#,#0.00")
            drArrangement("Tax") = Format(Math.Round(dNewTaxValue, 2), "#,#0.00")
            drArrangement("ThisPerc") = Format(Math.Round(dNewThisPercent, 2), "0.00")

            If nRIArrangementLineKey <> 0 Then
                drArrangement("ActionType") = NexusProvider.RowAction.EditRow
            End If

            'Recalculate the values for Fac or treaty Total
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then ' Treaty
                drSectionTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_TreatyTotal").ToString() & "'")(0)
            Else 'Fac Prop
                drSectionTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_FacTotal").ToString() & "'")(0)
            End If
            drSectionTotal("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("ThisPerc")) + dChangedThisPercent, 2), "0.00")
            drSectionTotal("SumInsured") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("SumInsured")) + dChangedSumInsured, 2), "#,#0.00")
            drSectionTotal("Premium") = String.Format("{0:n4}", Math.Round(Convert.ToDecimal(drSectionTotal("Premium")) + dChangedPremium, 4))
            drSectionTotal("Commission") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Commission")) + dChangedCommissionValue, 2), "#,#0.00")
            drSectionTotal("CommissionTax") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("CommissionTax")) + dChangedCommissionTaxValue, 2), "#,#0.00")
            drSectionTotal("Tax") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Tax")) + dChangedTaxValue, 2), "#,#0.00")

            'Recalculate the values for allocated row
            drAllocated = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_Allocated").ToString() & "'")(0)
            drAllocated("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drAllocated("ThisPerc")) + dChangedThisPercent, 2), "0.00")
            drAllocated("SumInsured") = Format(Math.Round(Convert.ToDecimal(drAllocated("SumInsured")) + dChangedSumInsured, 2), "#,#0.00")
            drAllocated("Premium") = String.Format("{0:n4}", Math.Round(Convert.ToDecimal(drAllocated("Premium")) + dChangedPremium, 4))
            drAllocated("Commission") = Format(Math.Round(Convert.ToDecimal(drAllocated("Commission")) + dChangedCommissionValue, 2), "#,#0.00")
            drAllocated("CommissionTax") = Format(Math.Round(Convert.ToDecimal(drAllocated("CommissionTax")) + dChangedCommissionTaxValue, 2), "#,#0.00")
            drAllocated("Tax") = Format(Math.Round(Convert.ToDecimal(drAllocated("Tax")) + dChangedTaxValue, 2), "#,#0.00")

            'Recalculate the values from UnAllocated row
            If dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_UnAllocated").ToString() & "'").Length > 0 Then
                drUnAllocated = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_UnAllocated").ToString() & "'")(0)

                dUnAllocatedThisPercent = Convert.ToDecimal(drUnAllocated("ThisPerc"))
                dUnAllocatedPremium = Convert.ToDecimal(drUnAllocated("Premium"))
                dUnAllocatedSumInsured = Convert.ToDecimal(drUnAllocated("SumInsured"))

                'Update existing row
                If (Math.Round(dUnAllocatedSumInsured - dChangedSumInsured, 2) <> 0 And Math.Round(dUnAllocatedPremium - dChangedPremium, 2) <> 0) OrElse (dTotalSumInsured - Convert.ToDecimal(drAllocated("SumInsured"))) <> 0 OrElse Math.Round(dTotalPremium - Convert.ToDecimal(drAllocated("Premium")), 2) <> 0 Then
                    drUnAllocated("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("ThisPerc")) - dChangedThisPercent, 2), "0.00")
                    drUnAllocated("SumInsured") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("SumInsured")) - dChangedSumInsured, 2), "#,#0.00")
                    drUnAllocated("Premium") = String.Format("{0:n4}", Math.Round(Convert.ToDecimal(drUnAllocated("Premium")) - dChangedPremium, 4))
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = False
                Else
                    dtRIArrangement.Rows.Remove(drUnAllocated)
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = True
                End If
            Else
                drUnAllocated = dtRIArrangement.NewRow
                drUnAllocated("ActionType") = NexusProvider.RowAction.None
                drUnAllocated("Name") = GetLocalResourceObject("lbl_UnAllocated").ToString()
                drUnAllocated("ThisPerc") = Format(Math.Round(dChangedThisPercent * -1, 2), "0.00")
                drUnAllocated("SumInsured") = Format(Math.Round(dChangedSumInsured * -1, 2), "#,#0.00")
                drUnAllocated("Premium") = String.Format("{0:n4}", Math.Round(dChangedPremium * -1, 4))
                If Session(CNMTAType) IsNot Nothing Then
                    dtRIArrangement.Rows.InsertAt(drUnAllocated, dtRIArrangement.Rows.Count - 2)
                Else
                    dtRIArrangement.Rows.Add(drUnAllocated)
                End If

                drUnAllocated = Nothing
                ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = False

            End If
            dtRIArrangement.AcceptChanges()
            dsArrangements.AcceptChanges()

            Session(CNRIData) = dsArrangements
            If drArrangement("LineType") = "T" OrElse drArrangement("LineType") = "R" Then
                PopulateRIGrid(False)
                PopulateRiOverrideReason(True)
            Else
                PopulateRIGrid(True)
            End If

        End Sub

        ''' <summary>
        ''' To handle the cancel from RI screen
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            UpdateRiskStatus()
            Session(CNRIData) = Nothing
            Response.Redirect("~/secure/PremiumDisplay.aspx")
        End Sub

        ''' <summary>
        ''' To update the risk status - PIN Pending if RI bands not fully allocated
        ''' </summary>
        ''' <remarks></remarks>
        Sub UpdateRiskStatus()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If oQuote IsNot Nothing Then
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key, NexusProvider.RiskStatusType.PENDINGRI, oQuote.BranchCode)
            End If
        End Sub

        Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
            If Session(CNRiskViewStartPoint) = "ClientManager" Then
                Session.Remove(CNRiskViewStartPoint)
                Dim oParty As NexusProvider.BaseParty
                Dim sUrl As String = String.Empty
                oParty = Session(CNParty)
                Select Case True
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        sUrl = "~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & oParty.UserName & ""
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        sUrl = "~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & oParty.UserName & ""
                End Select
                Response.Redirect(sUrl, True)
            End If
            If Session(CNRiskMode) = RiskMode.View Or Session(CNMode) = Mode.View Then
                'Remove existing session before redirecting an user
                Session(CNRIData) = Nothing
                Session.Remove(CNBackDatedVersions)
                Session.Remove(CNRatingSections)

                If Session(CNIsInteractiveBackdatedMTA) = True Then
                    Response.Redirect("~/secure/BackDatedMTA.aspx", False)
                Else
                    Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                End If
            Else
                If ValidateRI() = True Then
                    UpdateRIArrangement()
                    Session(CNRIData) = Nothing
                    Session.Remove(CNBackDatedVersions)
                    Session.Remove(CNRatingSections)

                    If Session(CNIsInteractiveBackdatedMTA) = True Then
                        Response.Redirect("~/secure/BackDatedMTA.aspx", False)
                    Else
                        Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                    End If
                End If
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearTab", "localStorage.removeItem('activeMainTab');", True)
        End Sub

        ''' <summary>
        ''' To validate all RI bands to be 100% allocated
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidateRI() As Boolean
            Dim dsArrangements As DataSet
            Dim oSB As New StringBuilder
            Dim bOverrideReasonSelected As Boolean = True
            Dim bIsAllRIBandsAllocated As Boolean = True

            For Each oRIBand As ListItem In ddlReinsurance.Items
                Dim sCurrentDataTableName As String = "Arrangement_Current_" & oRIBand.Value
                dsArrangements = TryCast(Session(CNRIData), DataSet)

                Dim bAnyRowedited As Boolean = False
                Dim nRiOverrideReasonId As Integer = 0
                If dsArrangements IsNot Nothing Then
                    If CType(ViewState("IsAllocated_" & oRIBand.Value), Boolean) = False Then
                        If dsArrangements.Tables(sCurrentDataTableName) IsNot Nothing AndAlso dsArrangements.Tables(sCurrentDataTableName).Select("Name='Unallocated'") IsNot Nothing Then
                            Dim dSumValue As Double = 0
                            Dim dPremiumValue As Double = 0
                            Dim drUnallocated As DataRow
                            drUnallocated = dsArrangements.Tables(sCurrentDataTableName).Select("Name='Unallocated'")(0)
                            If drUnallocated IsNot Nothing Then
                                If Double.TryParse(drUnallocated.Item("SumInsured"), dSumValue) AndAlso dSumValue <> 0 Then
                                    bIsAllRIBandsAllocated = False
                                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_NotFullyAllocated").ToString().Replace("#BandName", oRIBand.Text) & "');", True)
                                    Return False
                                End If
                            End If
                            If Double.TryParse(drUnallocated.Item("Premium"), dPremiumValue) AndAlso Math.Round(dPremiumValue, 2) <> 0 Then
                                bIsAllRIBandsAllocated = False
                                ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_PremiumNotFullyAllocated").ToString().Replace("#BandName", oRIBand.Text) & "');", True)
                                Return False
                            End If
                        End If
                    End If
                    If dsArrangements.Tables(sCurrentDataTableName) IsNot Nothing Then
                        With dsArrangements.Tables(sCurrentDataTableName)
                            For Each drRIArrangement As DataRow In dsArrangements.Tables(sCurrentDataTableName).Rows
                                If nRiOverrideReasonId = 0 Then
                                    nRiOverrideReasonId = IIf(IsDBNull(drRIArrangement("RiOverrideReasonId")), 0, drRIArrangement("RiOverrideReasonId"))
                                End If
                                If drRIArrangement("ActionType") <> NexusProvider.RowAction.None AndAlso (drRIArrangement("LineType") = "T" OrElse drRIArrangement("LineType") = "R") Then
                                    bAnyRowedited = True
                                    Exit For
                                End If
                            Next
                            If bAnyRowedited AndAlso nRiOverrideReasonId = 0 Then
                                If oSB.Length <> 0 Then
                                    oSB.Append(",")
                                End If
                                oSB.Append(oRIBand.Text)
                                If bOverrideReasonSelected Then
                                    bOverrideReasonSelected = False
                                End If
                            End If
                        End With
                    End If
                End If
            Next

            If Not bOverrideReasonSelected Then
                ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_RIOverrideReason").ToString() & ":" & oSB.ToString() & "');", True)
                bIsAllRIBandsAllocated = False
            End If

            Return bIsAllRIBandsAllocated

        End Function

        ''' <summary>
        ''' To Update RI arrangement lines (New lines will be addedd, edited lines will be updated and deleted lines will be deleted from the table)
        ''' </summary>
        ''' <remarks></remarks>
        Sub UpdateRIArrangement()
            Dim dtRIArrangement As DataTable
            Dim dsArrangements As DataSet
            Dim oArrangementLine As NexusProvider.ArrangementLinesType
            Dim oArrangementLineColl As NexusProvider.ArrangementLinesTypeCollection
            Dim dTotalSumInsured As Decimal
            Dim drBandTotal As DataRow
            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                dsArrangements = TryCast(Session(CNRIData), DataSet)
                For Each oRIBand As ListItem In ddlReinsurance.Items
                    If dsArrangements.Tables("Arrangement_Current_" & oRIBand.Value) IsNot Nothing Then
                        dtRIArrangement = dsArrangements.Tables("Arrangement_Current_" & oRIBand.Value)
                        drBandTotal = dtRIArrangement.Select("name='" & GetLocalResourceObject("lbl_BandTotal").ToString() & "'")(0)
                        dTotalSumInsured = Convert.ToDecimal(drBandTotal("SumInsured"))
                        oArrangementLineColl = New NexusProvider.ArrangementLinesTypeCollection

                        For Each drRIArrangement As DataRow In dtRIArrangement.Rows
                            If drRIArrangement("ActionType") <> NexusProvider.RowAction.None Then
                                oArrangementLine = New NexusProvider.ArrangementLinesType
                                oArrangementLine.ActionType = drRIArrangement("ActionType")
                                oArrangementLine.RIarrangementKey = drRIArrangement("RIArrangementKey")
                                oArrangementLine.RIArrangementLineKey = IIf(IsDBNull(drRIArrangement("RIArrangementLineKey")), 0, drRIArrangement("RIArrangementLineKey"))
                                oArrangementLine.TreatyCode = IIf(IsDBNull(drRIArrangement("TreatyCode")), "", drRIArrangement("TreatyCode"))
                                oArrangementLine.PartyKey = IIf(IsDBNull(drRIArrangement("PartyKey")), 0, drRIArrangement("PartyKey"))
                                oArrangementLine.Type = drRIArrangement("LineType")
                                oArrangementLine.Name = drRIArrangement("Name")
                                oArrangementLine.DefaultPerc = drRIArrangement("DefaultPerc")
                                If (dTotalSumInsured > 0.0) Then
                                    oArrangementLine.ThisPerc = (Convert.ToDecimal(Convert.ToDecimal(drRIArrangement("SumInsured")) * 100.0) / dTotalSumInsured)
                                ElseIf (dTotalSumInsured = 0.0) Then
                                    oArrangementLine.ThisPerc = Convert.ToDecimal(drRIArrangement("ThisPerc"))
                                End If
                                oArrangementLine.SumInsured = drRIArrangement("SumInsured")
                                oArrangementLine.PremiumValue = drRIArrangement("Premium")
                                oArrangementLine.Tax = drRIArrangement("Tax")
                                oArrangementLine.CommissionPerc = drRIArrangement("CommissionPerc")
                                oArrangementLine.CommissionValue = drRIArrangement("Commission")
                                oArrangementLine.CommissionTax = drRIArrangement("CommissionTax")
                                oArrangementLine.AgreementCode = drRIArrangement("Agreement")
                                oArrangementLine.RiOverrideReasonId = IIf(IsDBNull(dtRIArrangement.Rows(0).Item("RiOverrideReasonId")), 0, dtRIArrangement.Rows(0).Item("RiOverrideReasonId"))
                                oArrangementLineColl.Add(oArrangementLine)
                            End If
                        Next
                        If oArrangementLineColl.Count > 0 Then
                            oWebService.UpdateArrangementLines(oArrangementLineColl)
                        Else
                            If dtRIArrangement IsNot Nothing AndAlso dtRIArrangement.Rows IsNot Nothing Then
                                Dim nRiArrangementId As Integer
                                Dim nRiOverrideReasonId As Integer
                                nRiArrangementId = Convert.ToInt32(IIf(IsDBNull(dtRIArrangement.Rows(0).Item("RIArrangementKey")), 0, dtRIArrangement.Rows(0).Item("RIArrangementKey")))
                                nRiOverrideReasonId = Convert.ToInt32(IIf(IsDBNull(dtRIArrangement.Rows(0).Item("RiOverrideReasonId")), 0, dtRIArrangement.Rows(0).Item("RiOverrideReasonId")))
                                oWebService.UpdateRiOverrideReasonInRiArrangement(nRiArrangementId, nRiOverrideReasonId, "")
                            End If

                        End If
                    End If
                Next
            Catch ex As Exception
                Throw
            Finally
                dtRIArrangement = Nothing
                dsArrangements = Nothing
                oArrangementLine = Nothing
                oArrangementLineColl = Nothing
                oWebService = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' TO handle the delete row from RI Lines
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvReinsurance_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvReinsurance.RowDeleting
            Dim nPartyKey, nRIArrangementLineKey As Integer
            Dim dsArrangements As DataSet
            Dim sCurrentDataTableName As String = String.Empty
            Dim dtArrangement As DataTable
            Dim drArrangement As DataRow
            Dim drSectionTotal As DataRow
            Dim drAllocated As DataRow
            Dim drUnAllocated As DataRow

            Dim dChangedSumInsured As Decimal = 0
            Dim dChangedPremium As Decimal = 0
            Dim dChangedCommissionValue As Decimal = 0
            Dim dChangedThisPercent As Decimal = 0

            Dim dAllocatedThisPercent As Decimal = 0
            Dim dAllocatedPremium As Decimal = 0
            Dim dAllocatedSumInsured As Decimal = 0
            Dim dAllocatedCommission As Decimal = 0
            Dim dAllocatedTax As Decimal = 0
            Dim dAllocatedCommissionTax As Decimal = 0

            Dim dUnAllocatedThisPercent As Decimal = 0
            Dim dUnAllocatedPremium As Decimal = 0
            Dim dUnAllocatedSumInsured As Decimal = 0
            Dim sTreatyCode As String = ""

            dsArrangements = TryCast(Session(CNRIData), DataSet)
            sCurrentDataTableName = "Arrangement_Current_" & ddlReinsurance.SelectedValue

            dtArrangement = dsArrangements.Tables(sCurrentDataTableName)

            If Not IsDBNull(e.Keys(1)) AndAlso e.Keys(1) <> 0 Then
                nPartyKey = CType(e.Keys(1), Integer)
            End If
            If Not IsDBNull(e.Keys(2)) AndAlso e.Keys(2) <> "" Then
                sTreatyCode = CType(e.Keys(2), String)
            End If
            If Not IsDBNull(e.Keys(0)) Then
                nRIArrangementLineKey = CType(e.Keys(0), Integer)
            End If

            If nRIArrangementLineKey <> 0 Then
                drArrangement = dtArrangement.Select("RIArrangementLineKey=" & nRIArrangementLineKey.ToString())(0)
            Else
                If Not String.IsNullOrEmpty(sTreatyCode) Then
                    drArrangement = dtArrangement.Select("TreatyCode='" & sTreatyCode.ToString() & "'")(0)
                Else
                    drArrangement = dtArrangement.Select("PartyKey=" & nPartyKey.ToString())(0)
                End If

            End If

            'Find values from deleted row
            dChangedSumInsured = Convert.ToDecimal(drArrangement("SumInsured"))
            dChangedPremium = Convert.ToDecimal(drArrangement("Premium"))
            dChangedCommissionValue = Convert.ToDecimal(drArrangement("Commission"))
            dChangedThisPercent = Convert.ToDecimal(drArrangement("ThisPerc"))

            'Recalculate the values for Fac or treaty Total
            If drArrangement("LineType") = "F" Then
                drSectionTotal = dtArrangement.Select("name='" & GetLocalResourceObject("lbl_FacTotal").ToString() & "'")(0)
            Else
                drSectionTotal = dtArrangement.Select("name='" & GetLocalResourceObject("lbl_TreatyTotal").ToString() & "'")(0)
            End If

            If Not IsDBNull(drArrangement("RIArrangementLineKey")) Then
                drArrangement("ActionType") = NexusProvider.RowAction.DeleteRow
                drArrangement("ThisPerc") = 0
                drArrangement("SumInsured") = 0
                drArrangement("Premium") = 0
                drArrangement("Tax") = 0
                drArrangement("CommissionPerc") = 0
                drArrangement("Commission") = 0
                drArrangement("CommissionTax") = 0
                drArrangement("Agreement") = ""
            Else
                dtArrangement.Rows.Remove(drArrangement)
            End If

            drSectionTotal("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("ThisPerc")) - dChangedThisPercent, 2), "0.00")
            drSectionTotal("SumInsured") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("SumInsured")) - dChangedSumInsured, 2), "0.00")
            drSectionTotal("Premium") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Premium")) - dChangedPremium, 2), "0.00")
            drSectionTotal("Commission") = Format(Math.Round(Convert.ToDecimal(drSectionTotal("Commission")) - dChangedCommissionValue, 2), "0.00")

            'Recalculate the values for allocated row
            drAllocated = dtArrangement.Select("name='" & GetLocalResourceObject("lbl_Allocated").ToString() & "'")(0)
            drAllocated("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drAllocated("ThisPerc")) - dChangedThisPercent, 2), "0.00")
            drAllocated("SumInsured") = Format(Math.Round(Convert.ToDecimal(drAllocated("SumInsured")) - dChangedSumInsured, 2), "0.00")
            drAllocated("Premium") = Format(Math.Round(Convert.ToDecimal(drAllocated("Premium")) - dChangedPremium, 2), "0.00")
            drAllocated("Commission") = Format(Math.Round(Convert.ToDecimal(drAllocated("Commission")) - dChangedCommissionValue, 2), "0.00")

            'Recalculate the values from UnAllocated row
            If dtArrangement.Select("name='" & GetLocalResourceObject("lbl_UnAllocated").ToString() & "'").Length > 0 Then
                drUnAllocated = dtArrangement.Select("name='" & GetLocalResourceObject("lbl_UnAllocated").ToString() & "'")(0)

                dUnAllocatedThisPercent = Convert.ToDecimal(drUnAllocated("ThisPerc"))
                dUnAllocatedPremium = Convert.ToDecimal(drUnAllocated("Premium"))
                dUnAllocatedSumInsured = Convert.ToDecimal(drUnAllocated("SumInsured"))
                'Update existing row
                If dUnAllocatedSumInsured + dChangedSumInsured <> 0 And dUnAllocatedPremium + dChangedPremium <> 0 Then
                    drUnAllocated("ThisPerc") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("ThisPerc")) + dChangedThisPercent, 2), "0.00")
                    drUnAllocated("SumInsured") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("SumInsured")) + dChangedSumInsured, 2), "0.00")
                    drUnAllocated("Premium") = Format(Math.Round(Convert.ToDecimal(drUnAllocated("Premium")) + dChangedPremium, 2), "0.00")
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = False
                Else
                    dtArrangement.Rows.Remove(drUnAllocated)
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = True
                End If
            Else
                If dChangedSumInsured <> 0 Or dChangedPremium <> 0 Then
                    drUnAllocated = dtArrangement.NewRow
                    drUnAllocated("ActionType") = NexusProvider.RowAction.None
                    drUnAllocated("Name") = GetLocalResourceObject("lbl_UnAllocated").ToString()
                    drUnAllocated("ThisPerc") = Format(Math.Round(dChangedThisPercent, 2), "0.00")
                    drUnAllocated("SumInsured") = New Money(Format(Math.Round(dChangedSumInsured, 2), "0.00"), Session(CNCurrenyCode)).Value
                    drUnAllocated("Premium") = New Money(Format(Math.Round(dChangedPremium, 2), "0.00"), Session(CNCurrenyCode)).Value
                    dtArrangement.Rows.Add(drUnAllocated)
                    drUnAllocated = Nothing
                    ViewState("IsAllocated_" & ddlReinsurance.SelectedValue) = False
                End If

            End If

            dtArrangement.AcceptChanges()
            dsArrangements.AcceptChanges()
            Session(CNRIData) = dsArrangements
            PopulateRIGrid(False)
        End Sub
        Private Sub RecalculateTaxes(ByVal v_sCode As String, ByVal v_nKey As Integer, ByVal v_sLineType As String, ByVal v_dPremium As Decimal, ByVal v_nRILineID As Integer,
                                     ByVal v_dCommission As Decimal, ByVal v_bIgnoreTax As Boolean, ByVal v_bIgnoreDetails As Boolean, ByRef r_dPremTax As Decimal, ByRef r_dCommTax As Decimal,
                                     ByRef r_dCommissionPercent As Decimal, ByRef r_nIsRetained As Integer, ByRef r_sAgreementCode As String)

            oQuote = CType(Session(CNQuote), NexusProvider.Quote)

            Dim oRITreatyDetailsWithTax As New NexusProvider.RITreatyPartyWithTax
            oRITreatyDetailsWithTax.InsuranceFileID = oQuote.InsuranceFileKey
            oRITreatyDetailsWithTax.Premium = Convert.ToDecimal(v_dPremium)
            oRITreatyDetailsWithTax.PremiumTransType = IIf(v_sLineType = "T", TREATYPREMIUMTAXTYPE, FACPREMIUMTAXTYPE)
            oRITreatyDetailsWithTax.RIArrangementLineID = v_nRILineID
            oRITreatyDetailsWithTax.RiskID = oQuote.Risks(Session(CNCurrentRiskKey)).Key
            oRITreatyDetailsWithTax.Commission = Convert.ToDecimal(v_dCommission)
            oRITreatyDetailsWithTax.CommissionTransType = IIf(v_sLineType = "T", TREATYCOMMISSIONTAXTYPE, FACCOMMISSIONTAXTYPE)
            oRITreatyDetailsWithTax.TreatyCode = v_sCode
            oRITreatyDetailsWithTax.TreatyID = v_nKey
            oRITreatyDetailsWithTax.IgnoreDetails = v_bIgnoreDetails
            oRITreatyDetailsWithTax.IgnoreTax = v_bIgnoreTax

            oWebService = New NexusProvider.ProviderManager().Provider
            oWebService.GetRITreatyPartyDetailsWithTax(oRITreatyDetailsWithTax, "")

            r_dPremTax = Convert.ToDecimal(oRITreatyDetailsWithTax.PremiumTax)
            r_dCommTax = Convert.ToDecimal(oRITreatyDetailsWithTax.CommissionTax)
            r_dCommissionPercent = Convert.ToDecimal(oRITreatyDetailsWithTax.CommissionPercent)
            r_nIsRetained = Convert.ToInt32(oRITreatyDetailsWithTax.IsRetained)
            r_sAgreementCode = Convert.ToString(oRITreatyDetailsWithTax.AgreementCode)
        End Sub
        ''' <summary>
        ''' To Add a selected Treaty lines
        ''' </summary>
        ''' <param name="nKey"></param>
        ''' <param name="sCode"></param>
        ''' <param name="sName"></param>
        ''' <remarks></remarks>
        Private Sub AddTreaty(ByVal nKey As Integer, ByVal sCode As String, ByVal sName As String, ByRef r_bTreatyAlreadyExist As Boolean)

            Dim dsArrangements As DataSet
            Dim dtRIArrangement As DataTable
            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            Dim drArrangement As DataRow
            dsArrangements = TryCast(Session(CNRIData), DataSet)
            dtRIArrangement = dsArrangements.Tables(sCurrentDataTableName)
            r_bTreatyAlreadyExist = False
            If dtRIArrangement.Select("TreatyCode='" & sCode.ToString() & "'").Length > 0 Then
                Dim drExistingFacRow As DataRow = dtRIArrangement.Select("TreatyCode='" & sCode.ToString() & "'")(0)
                If drExistingFacRow("ActionType") = NexusProvider.RowAction.DeleteRow Then
                    drExistingFacRow("ActionType") = NexusProvider.RowAction.EditRow
                    dsArrangements.AcceptChanges()
                    Session(CNRIData) = dsArrangements
                    PopulateRIGrid(False)
                Else
                    'Show validation message
                    ScriptManager.RegisterStartupScript(Page, GetType(Page), "script", "alert('" & GetLocalResourceObject("msg_TreatyExists").ToString().Replace("#Description", sName.Trim()) & "');", True)
                    r_bTreatyAlreadyExist = True
                End If
            Else
                Dim dTaxPrem As Decimal
                Dim dTaxComm As Decimal
                Dim dCommPerc As Decimal
                Dim nIsRetained As Integer
                Dim sAgreementCode As String

                ' RecalculateTaxes(sCode, nKey, "T", 0, 0, 0, dPremPerc, dCommPerc, False, False)
                RecalculateTaxes(v_sCode:=sCode, v_nKey:=nKey, v_sLineType:="T", v_dPremium:=0, v_nRILineID:=0, v_dCommission:=0, v_bIgnoreDetails:=False, v_bIgnoreTax:=False, r_dCommissionPercent:=dCommPerc, r_dCommTax:=dTaxComm, r_dPremTax:=dTaxPrem, r_nIsRetained:=nIsRetained, r_sAgreementCode:=sAgreementCode)

                drArrangement = dtRIArrangement.NewRow
                drArrangement("ActionType") = NexusProvider.RowAction.AddRow
                drArrangement("RIArrangementKey") = dtRIArrangement.Rows(1)("RIArrangementKey")
                drArrangement("LineType") = IIf(nIsRetained = 0, "T", "R")
                drArrangement("PartyKey") = 0
                drArrangement("Name") = sName
                drArrangement("DefaultPerc") = 100
                drArrangement("ThisPerc") = 0
                drArrangement("SumInsured") = "0.00"
                drArrangement("Premium") = "0.00"
                drArrangement("TaxPerc") = 0
                drArrangement("Tax") = Format(Math.Round(dTaxPrem, 2), "0.00")
                drArrangement("CommissionPerc") = Format(Math.Round(dCommPerc, 2), "0.00")
                drArrangement("Commission") = "0.00"
                drArrangement("CommissionTax") = Format(Math.Round(dTaxComm, 2), "0.00")
                drArrangement("Agreement") = sAgreementCode.Trim()
                drArrangement("TreatyCode") = sCode.Trim()
                drArrangement("DefaultLine") = 0
                Dim nRowCount As Integer = 0
                For Each dr As DataRow In dtRIArrangement.Rows
                    nRowCount += 1
                    If dr IsNot Nothing AndAlso dr.Item(2).ToString() = "" AndAlso dr.Item(3).ToString() = "" AndAlso dr.Item(4).ToString() = "" AndAlso dr.Item(5).ToString() = "" AndAlso dr.Item(6).ToString().ToUpper = "TREATY TOTAL" Then
                        dtRIArrangement.Rows.InsertAt(drArrangement, nRowCount - 1)
                        Exit For
                    End If
                Next

                drArrangement = Nothing
                dtRIArrangement.AcceptChanges()
                dsArrangements.AcceptChanges()

                Session(CNRIData) = dsArrangements
                'Populate grid with updated dataset
                PopulateRIGrid(False)

            End If

        End Sub

        Private Sub GISLookup_RIOverrideReason_SelectedIndexChange(sender As Object, e As EventArgs) Handles GISLookup_RIOverrideReason.SelectedIndexChange
            If String.IsNullOrEmpty(GISLookup_RIOverrideReason.Text.Trim) Then
                Exit Sub
            End If
            Dim dsArrangements As DataSet
            Dim sCurrentDataTableName As String = "Arrangement_Current_" & ddlReinsurance.SelectedValue
            dsArrangements = TryCast(Session(CNRIData), DataSet)
            If dsArrangements.Tables(sCurrentDataTableName) IsNot Nothing Then
                With dsArrangements.Tables(sCurrentDataTableName)
                    If .Rows IsNot Nothing AndAlso .Rows(0).Item("RiOverrideReasonId") IsNot Nothing Then
                        .Rows(0).Item("RiOverrideReasonId") = Convert.ToInt32(GISLookup_RIOverrideReason.Value)
                    End If
                End With
            End If
            dsArrangements.AcceptChanges()

            Session(CNRIData) = dsArrangements
        End Sub

        Private Sub btnAddTreaty_Click(sender As Object, e As EventArgs) Handles btnAddTreaty.Click

        End Sub

        Private Sub btnAddFacProp_Click(sender As Object, e As EventArgs) Handles btnAddFacProp.Click

        End Sub
    End Class
End Namespace