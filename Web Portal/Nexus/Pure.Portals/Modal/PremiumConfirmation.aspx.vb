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
    ''' this page will show standard Net Annual Premium and Gross Annual Premium for each policy section, along with the respective
    ''' totals for each policy section,also the Net AP/RP and Gross AP/RP will be displayed if an MTA or Cancellation transaction 
    ''' is undertaken in a grid and will allow to subsequently edit the Net Premium and Net AP/RP for each section of the policy.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Modal_PremiumConfirmation
        Inherits System.Web.UI.Page
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim sDataModelCode As String = String.Empty 'Data model code

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
                'set oQuote from session
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
                        oWebService = Nothing  'clear the object
                    End Try
                    'save oQuote to session
                    Session(CNQuote) = oQuote
                End If
            End If
            sDataModelCode = Session(CNDataModelCode) 'Data Model code
            'Show NetAP/RP and GrossAP/RP only in case of MTA or MTC
            If Session(CNMTAType) IsNot Nothing Then
                grdvPremiumConfirmation.Columns(4).Visible = True  'Net AP/RP
                grdvPremiumConfirmation.Columns(5).Visible = True  'Gross AP/RP
            Else
                grdvPremiumConfirmation.Columns(4).Visible = False  'Net AP/RP
                grdvPremiumConfirmation.Columns(5).Visible = False  'Gross AP/RP
            End If
            Dim bViewMode As Boolean = False
            'check if view mode set for specific case
            If Request.QueryString("SetViewMode") IsNot Nothing Then
                bViewMode = Convert.ToBoolean(Request.QueryString("SetViewMode"))
                CommissionUpdate.SetViewMode = bViewMode
            End If
            'Show/hide edit link buttons according to selected user groups
            If UserCanDoTask("ShowPremConfEditLinks") And Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                grdvPremiumConfirmation.Columns(6).Visible = True
            Else
                grdvPremiumConfirmation.Columns(6).Visible = False
            End If
            'If in view mode Save and SetDefaults are not available
            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                'btnSave.Visible = True
                btnDefault.Visible = True
            Else
                'btnSave.Visible = False
                btnDefault.Visible = False
            End If
            'do not bind data at any post back, it should only be at the page load
            If Not IsPostBack Then
                BindData() 'Bind the grid
            End If
            'bind data when changes are done at update commission screen
            If Request("__EVENTARGUMENT") = "RefreshGrid" Then
                BindData() 'Bind the grid
            End If
            PopulateTotals() 'Populate Premium, IPT, Total Premium

        End Sub

        'do not delete this
        Protected Sub grdvPremiumConfirmation_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles grdvPremiumConfirmation.RowCancelingEdit

        End Sub

        Protected Sub grdvPremiumConfirmation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvPremiumConfirmation.RowCommand

            Dim strQuery As String
            Dim iKey As String
            For iTempVar As Integer = 0 To grdvPremiumConfirmation.Rows.Count - 1
                'fetch data from XML of output_commission object based on OI Key 
                strQuery = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & CType(grdvPremiumConfirmation.Rows(iTempVar).Cells(6).FindControl("lblOI"), Label).Text & "']"
                'policy section code - used to compare with command argument so as to identify the row
                iKey = CType(grdvPremiumConfirmation.Rows(iTempVar).Cells(0).FindControl("lblPolicySectionCode"), Label).Text.Trim
                If CStr(e.CommandArgument) = iKey Then
                    'show save and cancel link buttons in case the screen is in edit mode
                    If e.CommandName = "Edit" Then
                        grdvPremiumConfirmation.EditIndex = iTempVar 'enable the edit mode
                        BindData() 'bind the grid
                        grdvPremiumConfirmation.Rows(iTempVar).Cells(5).FindControl("lnkEdit").Visible = False
                        grdvPremiumConfirmation.Rows(iTempVar).Cells(5).FindControl("lnkSave").Visible = True
                        grdvPremiumConfirmation.Rows(iTempVar).Cells(5).FindControl("lnkCancel").Visible = True
                        Session("RowIndex") = iTempVar
                        Exit For
                        'on click of cancel, hide save and cancel link buttons and show edit link
                    ElseIf e.CommandName = "Cancel" Then
                        grdvPremiumConfirmation.EditIndex = -1 'disable the edit mode
                        BindData() 'bind the grid
                        grdvPremiumConfirmation.Rows(iTempVar).Cells(5).FindControl("lnkEdit").Visible = True
                        grdvPremiumConfirmation.Rows(iTempVar).Cells(5).FindControl("lnkSave").Visible = False
                        grdvPremiumConfirmation.Rows(iTempVar).Cells(5).FindControl("lnkCancel").Visible = False
                        Exit For
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' To Bind the grid
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindData()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            xmlTR.Close() 'close the text reader
            Dim oNode As XmlNode
            'check if this object exists in XML
            oNode = Doc.SelectSingleNode("/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION")
            'If the node exists in the XML
            If oNode IsNot Nothing Then
                Dim docX As XDocument = XDocument.Parse(Doc.OuterXml)  'Convert from XML document to XDocument
                'Fetch data from XML of output_commissin object grouped according to the class of business
                Dim CommissionOutput = _
                        From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                        Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0)
                'check if the CommissionOutput has got data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        'Bind the grid
                        grdvPremiumConfirmation.DataSource = CommissionOutput
                        grdvPremiumConfirmation.DataBind()
                        PopulateTotals() 'Populate total premium
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Populate Premium, IPT, Total Premium
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTotals()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'Load the XML
            xmlTR.Close() 'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
            Dim strPolicyBinder As String = sDataModelCode & "_POLICY_BINDER_ID"
            'Calculate the totals
            'Fetch data from XML of output_commission object grouped on policy section and find sum of comm percentage, net annual comm, gross annual comm, net ap/rp and gross ap/rp  for each policy section
            Dim CommissionOutputTotals = _
                   From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                   Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0) _
                Group CommissionOutputs By PolicyBinder = CommissionOutputs.Attribute(strPolicyBinder).Value _
                Into TotalCommPerc = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("COMMISSION_PERCENT").Value)), _
                TotalNetAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("NET_ANNUAL_COMMISSION_OVERRIDEN").Value)), _
                TotalGrossAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("GROSS_ANNUAL_COMMISSION").Value)), _
                TotalNetAPRP = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("NET_AP_RP_OVERRIDEN").Value)), _
                TotalGrossAPRP = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("GROSS_AP_RP").Value))
            'check if the CommissionOutputTotals has got data
            If CommissionOutputTotals IsNot Nothing Then
                If CommissionOutputTotals.Count > 0 Then
                    'assign values to footer controls for showing total
                    'Total net annual premium per policy section
                    Dim lblTotalNetAnnualPremium As Label = grdvPremiumConfirmation.FooterRow.FindControl("lblTotalNetAnnualPremium")
                    lblTotalNetAnnualPremium.Text = CommissionOutputTotals.ElementAt(0).TotalNetAnnualComm.ToString
                    'Total gross annual premium per policy section
                    Dim lblTotalGrossAnnualPremium As Label = grdvPremiumConfirmation.FooterRow.FindControl("lblTotalGrossAnnualPremium")
                    lblTotalGrossAnnualPremium.Text = CommissionOutputTotals.ElementAt(0).TotalGrossAnnualComm.ToString
                    'Total net ap/rp per policy section
                    Dim lblTotalNetAPRP As Label = grdvPremiumConfirmation.FooterRow.FindControl("lblTotalNetAPRP")
                    lblTotalNetAPRP.Text = CommissionOutputTotals.ElementAt(0).TotalNetAPRP.ToString
                    'Total gross ap/rp per policy section
                    Dim lblTotalGrossAPRP As Label = grdvPremiumConfirmation.FooterRow.FindControl("lblTotalGrossAPRP")
                    lblTotalGrossAPRP.Text = CommissionOutputTotals.ElementAt(0).TotalGrossAPRP.ToString
                End If
            End If
            'check if policy refer then do not display primium
            If Not CheckRefer() Then
                divTotals.Visible = True
                Dim dPremium As Double, dTax As Double, dTaxRate As Double, dTotalPremium As Double, dSumInsured As Double
                'calculate total premium and sum insured
                CalculatePremiumAndTax(dPremium, dTax, dTaxRate, dTotalPremium, dSumInsured)
                'assign the values of calculated totals
                lblPremiumValue.Text = New Money(dPremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                lblIPTValue.Text = New Money(dTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                lblTotalPremiumValue.Text = New Money(dTotalPremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
            Else
                divTotals.visible = False
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

        '''' <summary>
        '''' set back to the default rates in case of cancel or on click of 'Default Rates' button
        '''' </summary>
        '''' <remarks></remarks>
        'Private Sub SetDefaultValues()

        '    'set the oQuote from session
        '    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        '    Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
        '    Dim xmlTR As New XmlTextReader(srDataset)
        '    Dim Doc As New XmlDocument
        '    Doc.Load(xmlTR) 'load the XML
        '    Dim docX As XDocument = XDocument.Parse(Doc.OuterXml)  'Convert from XML document to XDocument
        '    'fetch data from XML of output_commission object grouped according to the class of business
        '    Dim CommissionOutput = _
        '            From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
        '            Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0)
        '    'check if the output_commission object has got data
        '    If CommissionOutput IsNot Nothing Then
        '        If CommissionOutput.Count > 0 Then
        '            For Each row In CommissionOutput
        '                'fetch row from XML based on the OI key
        '                Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@IS_LEAD='0' and @IS_RETAINED='0' and @OI='" & row.Attribute("OI").Value & "']"
        '                'Update XML - set the overriden commission as default commission
        '                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_ANNUAL_COMMISSION_OVERRIDEN", row.Attribute("NET_ANNUAL_COMMISSION").Value)
        '                'Update XML - set the overriden ap/rp as default ap/rp
        '                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_AP_RP_OVERRIDEN", row.Attribute("NET_AP_RP").Value)
        '                'Update XML - set is_overriden as 1
        '                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
        '            Next
        '            Dim oWebService As NexusProvider.ProviderBase
        '            'retrieve the oQuote from session
        '            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
        '            'create new instance of proxy
        '            oWebService = New NexusProvider.ProviderManager().Provider
        '            Try
        '                'call update risk to rerun rating scripts
        '                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
        '                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
        '                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
        '                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
        '                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
        '                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
        '                ElseIf Session(CNRenewal) Then
        '                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
        '                Else
        '                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
        '                End If
        '                'error handling - in case quote is referred or declined
        '            Catch ex As NexusProvider.NexusException
        '                If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
        '                ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
        '                End If
        '            Finally
        '                oWebService = Nothing  'clear the object
        '            End Try
        '            'create new instance of proxy
        '            oWebService = New NexusProvider.ProviderManager().Provider
        '            Try
        '                'call GetHeaderAndRisksByKey to retrieve the premium, tax etc.
        '                oWebService.GetHeaderAndRisksByKey(oQuote)
        '            Catch ex As NexusProvider.NexusException

        '            Finally
        '                oWebService = Nothing  'clear the object
        '            End Try
        '            'save the oQuote to session
        '            System.Web.HttpContext.Current.Session(CNQuote) = oQuote
        '            BindData() 'bind the grid
        '        End If
        '    End If

        'End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            'set the modal page theme
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))

        End Sub


        Protected Sub grdvPremiumConfirmation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPremiumConfirmation.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'Find and assign the control lblOriginalNetPrem 
                Dim lblOriginalNetPrem As HiddenField = CType(e.Row.FindControl("lblOriginalNetPrem"), HiddenField)
                'Find and assign the control txtNetAnnualPremium
                Dim txtNetAnnualPremium As TextBox = CType(e.Row.FindControl("txtNetAnnualPremium"), TextBox)
                'Find and assign the control lblNetAnnualPremium
                Dim lblNetAnnualPremium As Label = CType(e.Row.FindControl("lblNetAnnualPremium"), Label)
                'Find and assign the control lblOriginalNetAPRP
                Dim lblOriginalNetAPRP As HiddenField = CType(e.Row.FindControl("lblOriginalNetAPRP"), HiddenField)
                'To Show AP/RP columns only in case of MTA or MTC
                If Session(CNMTAType) IsNot Nothing Then
                    'Find and assign the control txtNetAPRP
                    Dim txtNetAPRP As TextBox = CType(e.Row.FindControl("txtNetAPRP"), TextBox)
                    'Find and assign the control lblNetAPRP
                    Dim lblNetAPRP As Label = CType(e.Row.FindControl("lblNetAPRP"), Label)
                    'show exclamation sign in case of any change
                    If lblNetAPRP Is Nothing Then
                        If lblOriginalNetAPRP.Value <> txtNetAPRP.Text Then
                            txtNetAPRP.Attributes.Add("Class", "updated")
                        End If
                        txtNetAPRP.Attributes.Add("onblur", "javascript:Revise(" & txtNetAPRP.ClientID & ".value, " & lblOriginalNetAPRP.ClientID & ".value, " & txtNetAPRP.ClientID & ")")
                    End If
                End If
                'show exclamation sign in case of any change
                If lblNetAnnualPremium Is Nothing Then
                    If lblOriginalNetPrem.Value <> txtNetAnnualPremium.Text Then
                        txtNetAnnualPremium.Attributes.Add("Class", "updated")
                    End If
                    txtNetAnnualPremium.Attributes.Add("onblur", "javascript:Revise(" & txtNetAnnualPremium.ClientID & ".value, " & lblOriginalNetPrem.ClientID & ".value, " & txtNetAnnualPremium.ClientID & ")")
                End If
            End If

        End Sub

        'Do Not delete this
        Protected Sub grdvPremiumConfirmation_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdvPremiumConfirmation.RowEditing

        End Sub

        Protected Sub btnDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefault.Click

            SetDefaultValues() 'set the values back to the default values on click of SetDefault button 

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

        Protected Sub btnApplySave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplySave.Click

            'When we save from link buttons we have session RowIndex
            If Session("RowIndex") IsNot Nothing Then
                SaveData(Session("RowIndex")) 'update the XML with all overriden values and call update risk to again run the script so as to calculate changed premium
                Session.Remove("RowIndex") 'clear the session
                BindData() 'bind the grid
            Else
                SaveData() 'update the XML with all overriden values and call update risk to again run the script so as to calculate changed premium
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
            End If

        End Sub

        ''' <summary>
        ''' update the XML with all overriden values and call update risk to again run the script so as 
        ''' to calculate changed premium
        ''' </summary>
        ''' <param name="iRowIndex"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Optional ByVal iRowIndex As Integer = -1)

            Dim oQuote As NexusProvider.Quote
            'retrieve the oQuote from session
            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase
            Dim dCurrentValue As Double

            'save changes to the XML
            'in case of change- set is_overriden of commission_output=1 for that COB and is_overriden of premium_breakdown=1 for the respective COB
            If iRowIndex = -1 Then 'not in edit mode
                For iCount As Integer = 0 To grdvPremiumConfirmation.Rows.Count - 1
                    'Fetch the row based on the OI key
                    Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[ @OI='" & CType(grdvPremiumConfirmation.Rows(iCount).Cells(6).FindControl("lblOI"), Label).Text & "']"
                    If CType(grdvPremiumConfirmation.Rows(iCount).Cells(1).FindControl("txtNetAnnualPremium"), TextBox) IsNot Nothing Then
                        'Update XML - with the overriden net annual premium
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_ANNUAL_COMMISSION_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iCount).Cells(1).FindControl("txtNetAnnualPremium"), TextBox).Text.Trim)
                        'overriden net annual premium
                        dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iCount).FindControl("txtNetAnnualPremium"), TextBox).Text)
                    ElseIf CType(grdvPremiumConfirmation.Rows(iCount).Cells(1).FindControl("lblNetAnnualPremium"), Label) IsNot Nothing Then
                        'Update XML - with the overriden net annual premium 
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_ANNUAL_COMMISSION_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iCount).Cells(1).FindControl("lblNetAnnualPremium"), Label).Text.Trim)
                        'overriden net annual premium
                        dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iCount).FindControl("lblNetAnnualPremium"), Label).Text)
                    End If
                    'in case of MTA or MTC update the values of net ap/rp
                    If Session(CNMTAType) IsNot Nothing Then
                        If CType(grdvPremiumConfirmation.Rows(iCount).Cells(1).FindControl("txtNetAPRP"), TextBox) IsNot Nothing Then
                            'Update XML - with the overriden net ap/rp
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_AP_RP_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iCount).Cells(3).FindControl("txtNetAPRP"), TextBox).Text.Trim)
                            'overriden net ap/rp
                            dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iCount).FindControl("txtNetAPRP"), TextBox).Text)
                        ElseIf CType(grdvPremiumConfirmation.Rows(iCount).Cells(1).FindControl("lblNetAPRP"), Label) IsNot Nothing Then
                            'Update XML - with the overriden net ap/rp
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_AP_RP_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iCount).Cells(3).FindControl("lblNetAPRP"), Label).Text.Trim)
                            'overriden net ap/rp
                            dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iCount).FindControl("lblNetAPRP"), Label).Text)
                        End If

                        'show exclamation sign in case Net AP/RP is changed
                        If (dCurrentValue <> CType(grdvPremiumConfirmation.Rows(iCount).Cells(4).FindControl("lblOriginalNetAPRP"), HiddenField).Value) Then
                            CType(grdvPremiumConfirmation.Rows(iCount).Cells(2).FindControl("imgNetAPRP"), HtmlImage).Visible = True
                        End If
                    End If

                    'Update XML - save the reason
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "REASON_FOR_OVERRIDE", txtReason.Text.Trim)
                    txtReason.Text = ""
                    Dim bOverride As Boolean = False
                    'check if net annual premium has been overriden, if so set bOverride as true
                    If Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iCount).FindControl("lblOriginalNetPrem"), HiddenField).Value) <> dCurrentValue Then
                        bOverride = True
                    End If
                    'in case of MTA n MTC - check if net ap/rp has been overriden, if so set bOverride as true
                    If Session(CNMTAType) IsNot Nothing Then
                        If Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iCount).FindControl("lblOriginalNetAPRP"), HiddenField).Value) <> dCurrentValue Then
                            bOverride = True
                        End If
                    End If
                    'set is_overriden as 0 of output_premiumbreakdown object for the COB for which net premium or net ap/rp has been overriden
                    If bOverride Then
                        'Update XML - set is_overriden as 1
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                        Dim strCOBCode As String = CType(grdvPremiumConfirmation.Rows(iCount).FindControl("lblPolicySectionCode"), Label).Text
                        oQuote = Session(CNQuote)
                        Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                        Dim xmlTR As New XmlTextReader(srDataset)
                        Dim Doc As New XmlDocument
                        Doc.Load(xmlTR) 'load the XML
                        Dim docX As XDocument = XDocument.Parse(Doc.OuterXml)  'Convert from XML document to XDocument
                        'fetch data from the XML of output_premiumbreakdown  
                        Dim PremiumBreakDown = _
                            From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns _
                            Where PremiumBreakDowns.Attribute("LEVEL1").Value = strCOBCode
                        'show the edit link button
                        CType(grdvPremiumConfirmation.Rows(iCount).FindControl("lnkEdit"), LinkButton).Visible = True
                        'check if PremiumBreakDown has got some data
                        If PremiumBreakDown IsNot Nothing Then
                            If PremiumBreakDown.Count > 0 Then
                                For Each row In PremiumBreakDown
                                    'Fetch the row based on the OI key
                                    Dim strPremOverriden As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN[@OI='" & row.Attribute("OI").Value & "']"
                                    'Update XML - set is_overriden as 0
                                    UpdateXML(strPremOverriden, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "IS_OVERRIDEN", 0)
                                Next
                            End If
                        End If
                    End If
                Next
            Else  'in edit mode
                'fetch the row based on the OI key
                Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[ @OI='" & CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(6).FindControl("lblOI"), Label).Text & "']"
                If CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(1).FindControl("txtNetAnnualPremium"), TextBox) IsNot Nothing Then
                    'Update XML - with the overriden net annual premium
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_ANNUAL_COMMISSION_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(1).FindControl("txtNetAnnualPremium"), TextBox).Text.Trim)
                    'overriden net annual premium
                    dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("txtNetAnnualPremium"), TextBox).Text)
                ElseIf CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(1).FindControl("lblNetAnnualPremium"), Label) IsNot Nothing Then
                    'Update XML - with the overriden net annual premium
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_ANNUAL_COMMISSION_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(1).FindControl("lblNetAnnualPremium"), Label).Text.Trim)
                    'overriden net annual premium
                    dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lblNetAnnualPremium"), Label).Text)
                End If
                Dim bOverride As Boolean = False
                'check if net annual premium has been overriden, if so set bOverride as true
                If Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lblOriginalNetPrem"), HiddenField).Value) <> dCurrentValue Then
                    bOverride = True
                End If
                'in case of MTA n MTC save the overriden net ap/rp
                If Session(CNMTAType) IsNot Nothing Then
                    If CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(1).FindControl("txtNetAPRP"), TextBox) IsNot Nothing Then
                        'Update XML - with the overriden net ap/rp
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_AP_RP_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(3).FindControl("txtNetAPRP"), TextBox).Text.Trim)
                        'overriden net annual premium
                        dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("txtNetAPRP"), TextBox).Text)
                    ElseIf CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(1).FindControl("lblNetAPRP"), Label) IsNot Nothing Then
                        'Update XML - with the overriden net annual premium
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "NET_AP_RP_OVERRIDEN", CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(3).FindControl("lblNetAPRP"), Label).Text.Trim)
                        'overriden net annual premium
                        dCurrentValue = Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lblNetAPRP"), TextBox).Text)
                        'show exclamation sign in case Net AP/RP is changed
                        If (CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(2).FindControl("lblNetAPRP"), Label).Text.Trim <> CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(4).FindControl("lblOriginalNetAPRP"), HiddenField).Value) Then
                            CType(grdvPremiumConfirmation.Rows(iRowIndex).Cells(2).FindControl("imgNetAPRP"), HtmlImage).Visible = True
                        End If
                    End If
                End If

                'save the reason
                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "REASON_FOR_OVERRIDE", txtReason.Text.Trim)
                txtReason.Text = ""
                'in case of MTA n MTC - check if net ap/rp has been overriden, if so set bOverride as true
                If Session(CNMTAType) IsNot Nothing Then
                    If Convert.ToDouble(CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lblOriginalNetAPRP"), HiddenField).Value) <> dCurrentValue Then
                        bOverride = True
                    End If
                End If

                'set is_overriden as 0 of output_premiumbreakdown object for the COB for which net premium or net ap/rp has been overriden
                If bOverride Then
                    'Update XML - set is_overriden as 1
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                    Dim strCOBCode As String = CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lblPolicySectionCode"), Label).Text
                    oQuote = Session(CNQuote)
                    Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim Doc As New XmlDocument
                    Doc.Load(xmlTR) 'load the XML
                    Dim docX As XDocument = XDocument.Parse(Doc.OuterXml)  'Convert from XML document to XDocument
                    'fetch data from the XML of output_premiumbreakdown  
                    Dim PremiumBreakDown = _
                        From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns _
                        Where PremiumBreakDowns.Attribute("LEVEL1").Value = strCOBCode
                    'show the edit link button
                    CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lnkEdit"), LinkButton).Visible = True
                    'check if PremiumBreakDown has got some data
                    If PremiumBreakDown IsNot Nothing Then
                        If PremiumBreakDown.Count > 0 Then
                            For Each row In PremiumBreakDown
                                'Fetch the row based on the OI key
                                Dim strPremOverriden As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN[@OI='" & row.Attribute("OI").Value & "']"
                                'Update XML - set is_overriden as 0
                                UpdateXML(strPremOverriden, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "IS_OVERRIDEN", 0) 'check it being set 0 for only that 
                            Next
                        End If
                    End If
                End If
                grdvPremiumConfirmation.EditIndex = -1 'disable the edit mode
                BindData() 'bind the grid
                'save/cancel link button will not be available since edit mode is disabled
                CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lnkSave"), LinkButton).Visible = False
                CType(grdvPremiumConfirmation.Rows(iRowIndex).FindControl("lnkCancel"), LinkButton).Visible = False
            End If
            'reterive the oQuote from session
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
        End Sub

        'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        '    SaveData() 'update the XML with all overriden values and call update risk to again run the script so as to calculate changed premium
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
        '    'close the modal page
        '    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        'End Sub

    End Class

End Namespace
