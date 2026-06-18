Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Session
Imports Nexus.Constants
Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Globalization.CultureInfo
Imports System.Linq
Imports System.Xml.Linq


Namespace Nexus

    ''' <summary>
    ''' this page will show a detailed breakdown of all the constituent parts that make up the premium in a grid 
    ''' and will allow to subsequently edit the rates associated with one or more of the constituent parts.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Modal_PremiumSummary
        Inherits System.Web.UI.Page

        Dim sDataModelCode As String = String.Empty  'Data Model Code
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim bViewMode As Boolean = False
        Protected Sub Page_AbortTransaction(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AbortTransaction
            Dim bPostBack As Boolean = True
            'check if post required or not
            If Request.QueryString("PostBack") IsNot Nothing Then
                bPostBack = Convert.ToBoolean(Request.QueryString("PostBack"))
            End If
            If bPostBack Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                'refresh the parent page on postback with event argument RefreshGrid  
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'in case of NB- set the quote status
            If Session(CNMTAType) Is Nothing And Session(CNRenewal) Is Nothing Then
                'set oQuote from the session
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                'agent details from session 
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                'If status is agent pending then set the status of quote as pending for the underwriter
                If (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending And oUserDetails.Key = 0) Then
                    'create new instance of proxy
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Pending
                    Try
                        oWebService.UpdateQuoteStatus(oQuote)
                    Catch ex As NexusProvider.NexusException
                    Finally
                        oWebService = Nothing 'clear the object
                    End Try
                    'save oQuote to session
                    Session(CNQuote) = oQuote
                End If
            End If
            sDataModelCode = Session(CNDataModelCode) 'Data Model Code
            'check if view mode set for specific case
            If Request.QueryString("SetViewMode") IsNot Nothing Then
                bViewMode = Convert.ToBoolean(Request.QueryString("SetViewMode"))
            End If
            'If in view mode the Save and SetDefault button is not available 
            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                'btnSave.Visible = True
                btnDefault.Visible = True
            Else
                'btnSave.Visible = False
                btnDefault.Visible = False
            End If
            'do not bind data at any post back, it should only be at the page load
            If Not IsPostBack Then
                BindData() 'bind repeater
            End If
            'bind data when changes are done at adjust rates screen
            If Request("__EVENTARGUMENT") = "RefreshGrid" Then
                BindData() 'bind repeater
            End If

        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            'set the modal page theme
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))

        End Sub

        ''' <summary>
        ''' To bind the repeater
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindData()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)  'load the XML
            xmlTR.Close()  'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument       
            'To bind the repeater on - Fetch data from XML of output_premium breakdown object
            Dim PremiumBreakDown = _
                   From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns Where (PremiumBreakDowns.Attribute("IS_HEADER").Value = "1")
            'check if PremiumBreakDown has got some data
            If PremiumBreakDown IsNot Nothing Then
                If PremiumBreakDown.Count > 0 Then
                    'Bind Repeater
                    rptPremiumSummary.DataSource = PremiumBreakDown
                    rptPremiumSummary.DataBind()
                    PopulateTotals() 'Populate the total premium, sum insured
                End If
            End If

        End Sub

        ''' <summary>
        ''' Set back to the default values
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetDefaultValues()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument   
            'Fetch data from XML of output_premium breakdown object
            Dim PremiumBreakDown = _
                   From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns
            'check if PremiumBreakDown has got some data
            If PremiumBreakDown IsNot Nothing Then
                If PremiumBreakDown.Count > 0 Then
                    For Each row In PremiumBreakDown
                        'Fetch row from XML based on OI Key
                        Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN[@OI='" & row.Attribute("OI").Value & "']"
                        'Update XML - update the xml with original value
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_BASE", row.Attribute("PERCENT_ORIGINAL").Value)
                        'Update XML - update the value with overriden value
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_OVERRIDE", row.Attribute("PERCENT_APPLICABLE").Value)
                        'Update XML - set is_overriden as 1
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "IS_OVERRIDEN", 1)
                    Next
                End If
            End If
            Dim oWebService As NexusProvider.ProviderBase
            'retrieve the oQuote from session
            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'call update risk to rerun rating scripts
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
                ElseIf Session(CNRenewal) Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
                Else
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
                End If
                'error handling - in case quote is referred or declined
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
                ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
                End If
            Finally
                oWebService = Nothing  'clear the object
            End Try
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'call GetHeaderAndRisksByKey to retrieve the premium, tax etc.
                oWebService.GetHeaderAndRisksByKey(oQuote)
            Catch ex As NexusProvider.NexusException

            Finally
                oWebService = Nothing  'clear the object
            End Try
            'save the oQuote to session
            System.Web.HttpContext.Current.Session(CNQuote) = oQuote
            BindData() 'Bind repeater
            PopulateTotals() 'Calculate total premium

        End Sub

        ''' <summary>
        ''' To bind the grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rptPremiumSummary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPremiumSummary.ItemDataBound

            Dim item As RepeaterItem = e.Item
            'Find the XML document for current Category Node
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            xmlTR.Close()  'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument       
            If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
                'policy section with code - header of the grid
                Dim strGroup As String = CType(item.DataItem, System.Xml.Linq.XElement).Attribute("HEADER_GROUP").Value
                'Find Repeater Control for Sections
                If item.FindControl("lblHeader") IsNot Nothing Then
                    Dim lblHeaderString As Label = CType(item.FindControl("lblHeader"), Label)
                    'Bind the control with the selected sections                    
                    lblHeaderString.Text = CType(item.DataItem, System.Xml.Linq.XElement).Attribute("DESCRIPTION").Value
                End If

                'Find Repeater Control for Sections
                If item.FindControl("grdvPremiumSummary") IsNot Nothing Then
                    Dim grdSummary As GridView = CType(item.FindControl("grdvPremiumSummary"), GridView)
                    'Bind the control with the selected sections
                    Dim PremiumBreakDown = _
                   From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns Where PremiumBreakDowns.Attribute("IS_HEADER").Value = "0" And PremiumBreakDowns.Attribute("RISK_RATING_SECTION").Value = "0" And PremiumBreakDowns.Attribute("HEADER_GROUP").Value = strGroup
                    'check if PremiumBreakDown has got some data
                    If PremiumBreakDown IsNot Nothing Then
                        If PremiumBreakDown.Count > 0 Then
                            'bind the grid
                            grdSummary.DataSource = PremiumBreakDown
                            grdSummary.DataBind()
                        End If
                    End If
                    'To Show AP/RP columns only in case of MTA or MTC
                    If Session(CNMTAType) IsNot Nothing Then
                        grdSummary.Columns(4).Visible = True
                    Else
                        grdSummary.Columns(4).Visible = False
                    End If
                    'Show edit links only to selected user groups
                    If UserCanDoTask("ShowPremSummEditLinks") And Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                        grdSummary.Columns(5).Visible = True
                    Else
                        grdSummary.Columns(5).Visible = False
                    End If
                    'check if PremiumBreakDown has got some data
                    If PremiumBreakDown IsNot Nothing Then
                        If PremiumBreakDown.Count > 0 Then
                            'assign values to footer controls for showing total premium, ap/rp and sum insured
                            Dim lblTotals As Label = grdSummary.FooterRow.FindControl("lblTotals")
                            lblTotals.Text = "Total"
                            Dim lblSumInsuredTotal As Label = grdSummary.FooterRow.FindControl("lblSumInsuredTotal")
                            lblSumInsuredTotal.Text = CType(item.DataItem, System.Xml.Linq.XElement).Attribute("SUM_INSURED").Value
                            Dim lblNetAnnualPremiumTotal As Label = grdSummary.FooterRow.FindControl("lblNetAnnualPremiumTotal")
                            lblNetAnnualPremiumTotal.Text = CType(item.DataItem, System.Xml.Linq.XElement).Attribute("PREMIUM_OVERRIDE").Value
                            Dim lblNetAPRPTotal As Label = grdSummary.FooterRow.FindControl("lblNetAPRPTotal")
                            lblNetAPRPTotal.Text = CType(item.DataItem, System.Xml.Linq.XElement).Attribute("NET_AP_RP").Value
                        End If
                    End If
                End If
            End If

        End Sub

        Protected Sub grdvPremiumSummary_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

            If e.Row.RowType = DataControlRowType.DataRow Then
                'find the control for applicable rate
                Dim lblAppRate As Label = CType(e.Row.FindControl("lblApplicableRate"), Label)
                'find control for net annual premium
                Dim lblNetAnnualPremium As Label = CType(e.Row.FindControl("lblNetAnnualPremium"), Label)
                'find control for sum insured
                Dim lblSumInsured As Label = CType(e.Row.FindControl("lblSumInsured"), Label)
                'find control for edit link button
                Dim lnkEditButton As LinkButton = CType(e.Row.FindControl("lnkEdit"), LinkButton)
                'policy section with category - header of the grid
                Dim strGroup As String = CType(e.Row.DataItem, System.Xml.Linq.XElement).Attribute("HEADER_GROUP").Value
                'rating section
                Dim strLevel2 As String = CType(e.Row.DataItem, System.Xml.Linq.XElement).Attribute("LEVEL2").Value
                'policy section 
                Dim strLevel1 As String = CType(e.Row.DataItem, System.Xml.Linq.XElement).Attribute("LEVEL1").Value
                'Adjust rates screen
                Dim strURL As String = "tb_show(null , '" & ResolveUrl("~/Modal/AdjustRates.aspx") & "?HEADER_GROUP=" & strGroup & "&LEVEL2=" & strLevel2 & "&LEVEL1=" & strLevel1 & "&SetViewMode=" & bViewMode & "&modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=500&width=650', null);return false;"
                'To navigate to adjust rates screen on click of edit link button              
                lnkEditButton.Attributes.Add("onclick", strURL)
            End If

        End Sub

        Protected Sub btnDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefault.Click

            SetDefaultValues()  'To undo the changes, set the values back to default values

        End Sub

        Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
            Dim bPostBack As Boolean = True
            'check if post required or not
            If Request.QueryString("PostBack") IsNot Nothing Then
                bPostBack = Convert.ToBoolean(Request.QueryString("PostBack"))
            End If
            If bPostBack Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                'refresh the parent page on postback with event argument RefreshGrid  
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

        ''' <summary>
        ''' Calculating Total Premium, IPT, Total Premium
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTotals()
            'check if policy refer then do not display primium
            If Not CheckRefer() Then
                divTotals.Visible = True
                Dim dPremium As Double, dTax As Double, dTaxRate As Double, dTotalPremium As Double, dSumInsured As Double
                'calculate total premium and sum insured
                CalculatePremiumAndTax(dPremium, dTax, dTaxRate, dTotalPremium, dSumInsured)
                'assign the calulated totals
                lblTotalPremiumValue.Text = New Money(dPremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & "+" & New Money(dTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " IPT = " & New Money(dPremium + dTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Total"
            Else
                divTotals.Visible = False
            End If
        End Sub

        'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        '    Dim oWebService As NexusProvider.ProviderBase
        '    'retrieve oQuote from session
        '    Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)
        '    'create new instance of proxy
        '    oWebService = New NexusProvider.ProviderManager().Provider
        '    Try
        '        'call update risk to rerun rating scripts
        '        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
        '        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
        '        ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
        '        ElseIf Session(CNRenewal) Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
        '        Else
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
        '        End If
        '        'error handling - in case quote is referred or declined
        '    Catch ex As NexusProvider.NexusException
        '        If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
        '        ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
        '        End If
        '    Finally
        '        oWebService = Nothing  'clear the object
        '    End Try
        '    'create new instance of proxy
        '    oWebService = New NexusProvider.ProviderManager().Provider
        '    Try
        '        'call GetHeaderAndRisksByKey to retrieve the premium, tax etc.
        '        oWebService.GetHeaderAndRisksByKey(oQuote)
        '    Catch ex As NexusProvider.NexusException

        '    Finally
        '        oWebService = Nothing  'clear the object
        '    End Try
        '    'save the oQuote to session
        '    System.Web.HttpContext.Current.Session(CNQuote) = oQuote
        '    Dim bPostBack As Boolean = True
        '    'check if post required or not
        '    If Request.QueryString("PostBack") IsNot Nothing Then
        '        bPostBack = Convert.ToBoolean(Request.QueryString("PostBack"))
        '    End If
        '    If bPostBack Then
        '        Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
        '        'refresh the parent page on postback with event argument RefreshGrid  
        '        Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
        '    End If
        '    'Close the modal page
        '    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        'End Sub

    End Class

End Namespace
