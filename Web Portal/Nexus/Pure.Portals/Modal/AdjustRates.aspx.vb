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
    ''' this page will allow user to edit the base and applicable rates for 
    ''' different rating sections associated with an item of cover.
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Modal_AdjustRates
        Inherits System.Web.UI.Page

        Dim sDataModelCode As String = String.Empty  'data model code
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        Protected Sub Page_AbortTransaction(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AbortTransaction

            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
            'refresh the parent page on postback with event argument RefreshGrid  
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            sDataModelCode = Session(CNDataModelCode) 'Data Model Code
            Dim bViewMode As Boolean = False
            'check if view mode set for specific case
            If Request.QueryString("SetViewMode") IsNot Nothing Then
                bViewMode = Convert.ToBoolean(Request.QueryString("SetViewMode"))
            End If
            'If in view mode the Save and SetDefault button is not available 
            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                btnSave.Visible = True
            Else
                btnSave.Visible = False
            End If
            If Not IsPostBack Then
                BindData() 'Bind the grid
                PopulateTotals() 'Populate Total, Sum Insured, Premium
            End If

        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            'set the modal page theme
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))

        End Sub

        ''' <summary>
        ''' To Bind the grid
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindData()

            'policy section with category
            Dim strGroup As String = CType(Request.QueryString("HEADER_GROUP"), String)
            'rating section
            Dim strLevel2 As String = CType(Request.QueryString("LEVEL2"), String)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)  'load the XML
            xmlTR.Close()  'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
            'Bind the grid with the data related to the rating sections associated with the rating group whose editlink is clicked.
            Dim PremiumBreakDown = _
                   From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns Where PremiumBreakDowns.Attribute("IS_HEADER").Value = "0" And PremiumBreakDowns.Attribute("HEADER_GROUP").Value = strGroup And PremiumBreakDowns.Attribute("LEVEL2").Value = strLevel2 And PremiumBreakDowns.Attribute("RISK_RATING_SECTION").Value <> "0"
            'check If PremiumBreakDown has got some data
            If PremiumBreakDown IsNot Nothing Then
                If PremiumBreakDown.Count > 0 Then
                    'Bind Grid
                    grdvAdjustRates.DataSource = PremiumBreakDown
                    grdvAdjustRates.DataBind()
                End If
            End If

        End Sub

        ''' <summary>
        ''' Populate Total, Sum Insured, Premium
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTotals()
            'policy section with category
            Dim strGroup As String = CType(Request.QueryString("HEADER_GROUP"), String)
            'rating section
            Dim strLevel2 As String = CType(Request.QueryString("LEVEL2"), String)
            'check if policy refer then do not display primium
            If Not CheckRefer() Then
                divTotals.Visible = True
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Doc.Load(xmlTR)  'load the XML
                xmlTR.Close()  'close the text reader
                Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
                'Fetch data from XML from output_premiumbreakdown object and group by rating section to find out total rate,sum insured and premium
                Dim PremiumBreakDownTotals = _
                         From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns Where PremiumBreakDowns.Attribute("IS_HEADER").Value = "0" And PremiumBreakDowns.Attribute("HEADER_GROUP").Value = strGroup And PremiumBreakDowns.Attribute("LEVEL2").Value = strLevel2 And PremiumBreakDowns.Attribute("RISK_RATING_SECTION").Value <> "0" _
                         Group PremiumBreakDowns By RISK_RATING_SECTION = PremiumBreakDowns.Attribute("LEVEL2").Value _
                         Into PERCENT_APPLICABLE = Sum(Convert.ToDecimal(PremiumBreakDowns.Attribute("PERCENT_OVERRIDE").Value)), _
                         SUM_INSURED = Sum(Convert.ToDecimal(PremiumBreakDowns.Attribute("SUM_INSURED").Value)), _
                         PREMIUM_APPLICABLE = Sum(Convert.ToDecimal(PremiumBreakDowns.Attribute("PREMIUM_OVERRIDE").Value))
                'set the total rate, premium and sum insured
                'check if PremiumBreakDownTotals has got some data
                If PremiumBreakDownTotals IsNot Nothing Then
                    If PremiumBreakDownTotals.Count > 0 Then
                        'assign the calculated total rate, sum insured, premium
                        lblTotalApplicable.Text = PremiumBreakDownTotals.ElementAt(0).PERCENT_APPLICABLE  'total rate
                        lblTotalSumInsured.Text = PremiumBreakDownTotals.ElementAt(0).SUM_INSURED  'total sum insured
                        lblTotalPremium.Text = PremiumBreakDownTotals.ElementAt(0).PREMIUM_APPLICABLE  'total premium
                    End If
                End If
            Else
                divTotals.Visible = False
            End If
            'Set the header for page                     
            lblHeader.Text = "Rates For : " & strLevel2 & " - " & strGroup

        End Sub

        ''' <summary>
        ''' set back to the default rates in case of cancel or on click of 'Default Rates' button
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetDefaultValues()

            'set oQuote from the session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            'rating section
            Dim strLevel2 As String = CType(Request.QueryString("LEVEL2"), String)
            'policy section with category
            Dim strGroup As String = CType(Request.QueryString("HEADER_GROUP"), String)
            Doc.Load(xmlTR)  'load the XML
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument

            'Bind the grid with the data related to the rating sections associated with the rating group whose editlink is clicked.
            'Fetch data from XML of output_premiumbreakdown object
            Dim PremiumBreakDown = _
                   From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns Where PremiumBreakDowns.Attribute("IS_HEADER").Value = "0" And PremiumBreakDowns.Attribute("HEADER_GROUP").Value = strGroup And PremiumBreakDowns.Attribute("LEVEL2").Value = strLevel2 And PremiumBreakDowns.Attribute("RISK_RATING_SECTION").Value <> "0"
            'check if PremiumBreakDown has got some data
            If PremiumBreakDown IsNot Nothing Then
                If PremiumBreakDown.Count > 0 Then
                    'for each row of output_premiumbreakdown object upadte XML
                    For Each row In PremiumBreakDown
                        'fetch the row from XML based on OI key
                        Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN[@OI='" & row.Attribute("OI").Value & "']"
                        'update XML- update the value of percent_base field with the original rate
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_BASE", row.Attribute("PERCENT_ORIGINAL").Value)
                        'update XML- update the value of percent_override with the overriden rate
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_OVERRIDE", row.Attribute("PERCENT_APPLICABLE").Value)
                        'update XML- update the value of is_overriden to 1
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
            BindData() 'bind the grid
            PopulateTotals() 'Populate Premium, IPT, Total Premium

        End Sub

        Protected Sub btnDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefault.Click

            SetDefaultValues() 'To undo the changes, set the values back to default values

        End Sub

        ''' <summary>
        ''' handle the change of base or applicable rate for each row.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAdjustRates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvAdjustRates.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'find and assign the text box control for base rate
                Dim txtBaseRate As TextBox = CType(e.Row.FindControl("txtBaseRate"), TextBox)
                'find and assign the text box control for applicable rate
                Dim txtAppRate As TextBox = CType(e.Row.FindControl("txtApplicableRate"), TextBox)
                'find and assign the hidden control for base rate
                Dim lblOriginalBaseRate As HiddenField = CType(e.Row.FindControl("lblOriginalBaseRate"), HiddenField)
                'find and assign the hidden control for applicable rate
                Dim lblOriginalAppRate As HiddenField = CType(e.Row.FindControl("lblOriginalAppRate"), HiddenField)
                'exclamation sign will appear if base rate is changed
                If lblOriginalBaseRate.Value <> txtBaseRate.Text Then
                    txtBaseRate.Attributes.Add("Class", "updated")
                End If
                txtBaseRate.Attributes.Add("onblur", "javascript:ReviseRate(" & txtBaseRate.ClientID & ".value, " & lblOriginalBaseRate.ClientID & ".value, " & txtBaseRate.ClientID & ")")
                'exclamation sign will appear if applicable rate is changed
                If lblOriginalAppRate.Value <> txtAppRate.Text Then
                    txtAppRate.Attributes.Add("Class", "updated")
                End If
                txtAppRate.Attributes.Add("onblur", "javascript:ReviseRate(" & txtAppRate.ClientID & ".value, " & lblOriginalAppRate.ClientID & ".value, " & txtAppRate.ClientID & ")")
            End If

        End Sub

        ''' <summary>
        ''' on click of save after entering 'reason for change', XML is updated with the 
        ''' changed rates and reason for change and update risk is called
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnApplySave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplySave.Click

            'section and policy section 
            Dim strLevel1 As String = CType(Request.QueryString("LEVEL1"), String)
            'rating section
            Dim strLevel2 As String = CType(Request.QueryString("LEVEL2"), String)
            'section with category
            Dim strGroup As String = CType(Request.QueryString("HEADER_GROUP"), String)
            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote
            'retrieve the oQuote from session
            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
            'update the XML with changed values- for every row of grid view
            For iCount As Integer = 0 To grdvAdjustRates.Rows.Count - 1
                'Fetch the row from XML based on OI Key
                Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN[@OI='" & CType(grdvAdjustRates.Rows(iCount).Cells(4).FindControl("lblOI"), Label).Text & "']"
                'Update XML- update the XML with the overriden value of base rate
                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_BASE", CType(grdvAdjustRates.Rows(iCount).Cells(1).FindControl("txtBaseRate"), TextBox).Text.Trim)
                'Update XML- update the XML with the overriden value of applicable rate
                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_OVERRIDE", CType(grdvAdjustRates.Rows(iCount).Cells(4).FindControl("txtApplicableRate"), TextBox).Text.Trim)
                'Update XML- If any of the base rate or applicable rate is overriden then set is_overriden as 1
                If (Convert.ToDouble(CType(grdvAdjustRates.Rows(iCount).Cells(1).FindControl("lblBaseRatePrev"), HiddenField).Value.Trim) <> Convert.ToDouble(CType(grdvAdjustRates.Rows(iCount).Cells(4).FindControl("txtBaseRate"), TextBox).Text.Trim)) Or (Convert.ToDouble(CType(grdvAdjustRates.Rows(iCount).Cells(1).FindControl("lblApplicableRatePrev"), HiddenField).Value.Trim) <> Convert.ToDouble(CType(grdvAdjustRates.Rows(iCount).Cells(4).FindControl("txtApplicableRate"), TextBox).Text.Trim)) Then
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "IS_OVERRIDEN", 1)
                End If
                'Update XML- Save the reason for override
                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "REASON_FOR_OVERRIDE", txtReason.Text.Trim)
            Next

            'if is_overriden is set to 1 in output_premiumbreakdown, is_overriden for output_commission is set to 0 for the same row         
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            xmlTR.Close()  'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
            'Fetch data from XML of output_commission object
            Dim CommissionOutput = _
                    From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                    Where CommissionOutputs.Attribute("COB_CODE").Value = strLevel1
            'check if CommissionOutput has got some data
            If CommissionOutput IsNot Nothing Then
                If CommissionOutput.Count > 0 Then
                    'for each row of output_commission object update XML
                    For Each row In CommissionOutput
                        'fetch the row from XML based on the OI Key
                        Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & row.Attribute("OI").Value & "']"
                        'Update XML - set is_overrioden of output_commission object as 0 for this row
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 0)
                    Next
                End If
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
            'refresh the parent page on postback with event argument RefreshGrid  
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub

        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
            'refresh the parent page on postback with event argument RefreshGrid  
            Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub

    End Class

End Namespace
