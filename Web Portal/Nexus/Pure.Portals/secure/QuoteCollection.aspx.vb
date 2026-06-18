Imports CMS.Library
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Library
Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus

Partial Class secure_QuoteCollection
    Inherits Frontend.clsCMSPage
#Region " CLEAR SESSION VALUE "

    ''' <summary> 
    ''' Clear QuoteCollection SessionValues .
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearQuoteCollectionSessionValues()
        Session.Remove(CNQuoteCollectionFiles)
        Session.Remove(CNQuoteCollectionSearchResults)
        Session.Remove(CNTotalForQuoteCollection)
        Session.Remove(CNPolicySummaryCollection)
        Session.Remove(CNPolicyNumber)
    End Sub

#End Region
    ''' <summary>
    ''' Pass the values entered in form to SAM, retrieve results and display.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        'Whenever find button is hit clear the Session Variables
        ClearQuoteCollectionSessionValues()
        BindData()
    End Sub

    Private Function CheckValidAgentorClient()
        'Validation for valid Agent or Client
        'Using Replace, we are searching for exactly Client/Agent Code
        Dim sAgentCode As String = txtAgentCode.Text
        Dim sClientCode As String = txtClient.Text
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria
        Dim oPartyCollection As NexusProvider.PartyCollection

        If Not String.IsNullOrEmpty(sClientCode) Then
            'User has enterd Client Code so validation for valid Client
            oPartySearchCriteria.ShortName = sClientCode
            oPartySearchCriteria.PartyType = "GC"
            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
            oPartyCollection = oWebService.FindParty(oPartySearchCriteria)

            If oPartyCollection IsNot Nothing AndAlso oPartyCollection.Count > 0 Then
                txtClientKey.Value = oPartyCollection(0).Key
            End If
        ElseIf Not String.IsNullOrEmpty(sAgentCode) Then
            'User has enterd Agent Code so validation for valid agent
            oPartySearchCriteria = New NexusProvider.PartySearchCriteria
            oPartySearchCriteria.AgentType = Nothing
            oPartySearchCriteria.ShortName = sAgentCode
            oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.AG
            oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)
            oPartyCollection = oWebService.FindParty(oPartySearchCriteria)
            'Checked for partycollection is not nothing
            If oPartyCollection IsNot Nothing AndAlso oPartyCollection.Count > 0 Then
                txtAgentKey.Value = oPartyCollection(0).Key
                'if collection is nothing clear agent key
            Else

                txtAgentKey.Value = 0
            End If
        Else
            'try to avoid the situation where user has entered only '%%%'
            Return False
        End If

        'cleaning up
        oWebService = Nothing
        oPartySearchCriteria = Nothing
        oPartyCollection = Nothing
        Return True
    End Function

    ''' <summary>
    ''' Will bind grid to search results returned by SAM method. 
    ''' For now bind grid to dummy values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        'If Not IsPostBack Then
        Session.Remove(CNQuoteCollectionSearchResults)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        '  Dim oQuote As NexusProvider.Quote = Nothing
        Dim iAgentKey, iPartyKey, iInsuranceFileKey As Integer
        Dim oProductListCollection As New ListItemCollection
        Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = Nothing
        Dim oQuoteCollection As NexusProvider.QuoteCollection = Nothing
        'Set the Agent Key when there is always the numeric value to avoid something like ABC or XYZ

        'Checked agentcode is not empty
        If String.IsNullOrEmpty(txtAgentCode.Text) = False Then

            CheckValidAgentorClient()
        Else

            txtAgentKey.Value = 0
        End If

        'Set the Party Key when there is always the numeric value to avoid something like ABC or XYZ

        If String.IsNullOrEmpty(txtClient.Text) = False Then
            If String.IsNullOrEmpty(txtClientKey.Value) = False AndAlso txtClientKey.Value.Trim <> "0" Then
                iPartyKey = txtClientKey.Value
            Else

                CheckValidAgentorClient()
            End If
        Else
            txtClientKey.Value = 0
        End If

        'only populate the product collection if there is some selection 
        If PickList.Visible = True Then
            If pckProduct.GetSelectedItems() IsNot Nothing Then
                oProductListCollection = pckProduct.GetSelectedItems()
            End If

        Else
            If drpProduct.SelectedValue <> "" Then
                Dim oProduct As New ListItem
                oProduct.Value = drpProduct.SelectedValue
                oProductListCollection.Add(oProduct)
            End If
        End If

        Dim oProductCodeCollection As New NexusProvider.ProductCollection

        Dim iCount As Integer
        For iCount = 0 To oProductListCollection.Count - 1
            Dim oProductCode As New NexusProvider.Product
            oProductCode.ProductCode = oProductListCollection(iCount).Value
            oProductCodeCollection.Add(oProductCode)
        Next

        Dim dStartDate As Date = IIf(String.IsNullOrEmpty(txtStartDate.Text), Date.MinValue, txtStartDate.Text)
        Dim dEndDate As Date = IIf(String.IsNullOrEmpty(txtEndDate.Text), Date.MinValue, txtEndDate.Text)

        'Set the InsuranceFile Key
        'Could nt find any other SAM method  to get the insurance file against the Insurance ref
        If String.IsNullOrEmpty(txtQuoteRef.Text) = False Or String.IsNullOrEmpty(txtRiskIndex.Text) = False Then
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim iMaxResultSet As Integer
            Dim sPolicyCode, sRiskIndex As String
            sPolicyCode = Nothing
            sRiskIndex = Nothing

            If String.IsNullOrEmpty(txtQuoteRef.Text) = False Then
                sPolicyCode = txtQuoteRef.Text
            End If

            If String.IsNullOrEmpty(txtRiskIndex.Text) = False Then
                sRiskIndex = txtRiskIndex.Text
            End If

            If String.IsNullOrEmpty(oPortal.MaxSearchResults) = False Then
                iMaxResultSet = oPortal.MaxSearchResults
            End If
            oInsuranceFileDetails = oWebService.FindPolicy(sPolicyCode, sRiskIndex, Nothing, NexusProvider.InsuranceFileTypes.ALL, False, iMaxResultSet)

            If oInsuranceFileDetails IsNot Nothing AndAlso oInsuranceFileDetails.Count > 0 Then
                Dim oTempQuoteCollection As NexusProvider.QuoteCollection
                oQuoteCollection = New NexusProvider.QuoteCollection
                For iRowCount As Integer = 0 To oInsuranceFileDetails.Count - 1
                    iInsuranceFileKey = oInsuranceFileDetails(iRowCount).InsuranceFileKey
                    oTempQuoteCollection = oWebService.GetQuotesMarkedForCollection("", iPartyKey, iInsuranceFileKey, iAgentKey, oProductCodeCollection, dStartDate, dEndDate, chkDirect.Checked)

                    If oTempQuoteCollection IsNot Nothing AndAlso oTempQuoteCollection.Count > 0 Then
                        For iResultCount As Integer = 0 To oTempQuoteCollection.Count - 1
                            oQuoteCollection.Add(oTempQuoteCollection(iResultCount))
                        Next
                    End If
                Next
            End If

            grdQuotes.Visible = True
            grdQuotes.DataSource = oQuoteCollection
            grdQuotes.DataBind()

            Session(CNQuoteCollectionSearchResults) = oQuoteCollection ' should be replaced with a constant value
        Else
            'Check for ahent key is not empty
            If txtAgentKey.Value <> 0 Then
                iAgentKey = txtAgentKey.Value

                oQuoteCollection = oWebService.GetQuotesMarkedForCollection("", iPartyKey, iInsuranceFileKey, iAgentKey, oProductCodeCollection, dStartDate, dEndDate, chkDirect.Checked)
                grdQuotes.Visible = True
                grdQuotes.DataSource = oQuoteCollection
                grdQuotes.DataBind()
                Session(CNQuoteCollectionSearchResults) = oQuoteCollection ' should be replaced with a constant value
                'if agent key is empty then make visibility of datagrid false
            ElseIf txtClientKey.Value <> 0 Then
                oQuoteCollection = oWebService.GetQuotesMarkedForCollection("", iPartyKey, iInsuranceFileKey, iAgentKey, oProductCodeCollection, dStartDate, dEndDate, chkDirect.Checked)
                grdQuotes.Visible = True
                grdQuotes.DataSource = oQuoteCollection
                grdQuotes.DataBind()
                Session(CNQuoteCollectionSearchResults) = oQuoteCollection ' should be replaced with a constant value
            ElseIf String.IsNullOrEmpty(txtClientKey.Value) = True Or String.IsNullOrEmpty(txtAgentKey.Value) = True Then
                grdQuotes.DataSource = Nothing
                grdQuotes.Visible = False
            Else


                oQuoteCollection = oWebService.GetQuotesMarkedForCollection("", iPartyKey, iInsuranceFileKey, iAgentKey, oProductCodeCollection, dStartDate, dEndDate, chkDirect.Checked)
                grdQuotes.Visible = True
                grdQuotes.DataSource = oQuoteCollection
                grdQuotes.DataBind()
                Session(CNQuoteCollectionSearchResults) = oQuoteCollection ' should be replaced with a constant value
            End If
        End If
    End Sub

    ''' <summary>
    ''' Create a client side array to hold the ids of the checkboxes which need to be targetted by the header checkbox
    ''' Also sets visibility of the make payment button so that it is only visible when results are returned
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdQuotes_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdQuotes.DataBound
        For Each gvr As GridViewRow In grdQuotes.Rows
            'Get a programmatic reference to the CheckBox control
            Dim cb As CheckBox = CType(gvr.FindControl("chkSelect"), CheckBox)
            ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", cb.ClientID, "'"))
        Next
        'show the make payment button if there are rows
        If grdQuotes.Rows.Count > 0 Then pnlMakePayment.Visible = True
    End Sub

    ''' <summary>
    ''' Set new page index and rebind the data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdQuotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdQuotes.PageIndexChanging
        grdQuotes.PageIndex = e.NewPageIndex
        grdQuotes.DataSource = Session(CNQuoteCollectionSearchResults)
        grdQuotes.DataBind()
    End Sub

    ''' <summary>
    ''' Hide search results and make payment button
    ''' Note that form is cleared client side 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        grdQuotes.Visible = False
        pnlMakePayment.Visible = False
        ' this method is not working on IE 7.0 OnClientClick = "aspnetForm.reset();"
        'So Couldn't find any better way to do this 
        txtAgentCode.Text = String.Empty
        txtClient.Text = String.Empty
        txtStartDate.Text = String.Empty
        txtEndDate.Text = String.Empty
        txtQuoteRef.Text = String.Empty
        txtRiskIndex.Text = String.Empty
        drpProduct.SelectedIndex = 0
        chkDirect.Checked = False
        chkProduct.Checked = False
        txtClientKey.Value = String.Empty
        txtAgentKey.Value = String.Empty
        'Called following event to hide picklist on clicking of New search button.
        Call chkProduct_CheckedChanged(sender, e)

    End Sub

    ''' <summary>
    ''' Method called by custom validator, checks that at least one checkbox is checked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CheckSomethingChecked(ByVal sender As Object, ByVal args As ServerValidateEventArgs)
        'Have commented this code as it was not checking for the pages which were already marked in last page
        'Say for eg we have 12 quotes marked and have selected 2 Quotes from 1 page and 1 quote from 
        ' next page then this function only shows the current page data
        Dim bResult As Boolean = False

        ''loop through the rows in the Quote
        Dim oQuoteCollection As NexusProvider.QuoteCollection
        oQuoteCollection = Session(CNQuoteCollectionSearchResults)

        For Each oQuote As NexusProvider.Quote In oQuoteCollection
            'if quote is selected then update the Quote object as well
            If oQuote.IsSelected Then
                'checkbox is checked so validator is valid
                bResult = True
                Exit For
            End If
        Next
        args.IsValid = bResult

    End Sub

    ''' <summary>
    ''' Method called by custom validator to ensure that the selected quotes all have the same agent
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CheckSameAgent(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

        args.IsValid = CheckSameValue(3)

    End Sub


    ''' <summary>
    ''' Method called by custom validator to ensure that the selected quotes all have the same branch
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CheckSameBranch(ByVal sender As Object, ByVal args As ServerValidateEventArgs) Handles cusvldSameBranch.ServerValidate

        args.IsValid = CheckSameValue(5)

    End Sub

    ''' <summary>
    ''' Method called by custom validator to ensure that the selected quotes all have the same client
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CheckSameClient(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

        args.IsValid = CheckSameValue(2)

    End Sub

    ''' <summary>
    ''' Helper method, used by the custom validator methods to check 
    ''' that the values in a particular column of the selected rows
    ''' all have the value.
    ''' </summary>
    ''' <param name="GridViewIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckSameValue(ByVal GridViewIndex As Integer) As Boolean
        'loop though grid view. check that all rows which have the 
        'checkbox checked have the same value in the column specified
        Dim bResult As Boolean = True
        Dim sInitialValue As String = String.Empty
        Dim bIsFirstRow As Boolean = True

        For Each gvr As GridViewRow In grdQuotes.Rows
            Dim cb As CheckBox = CType(gvr.FindControl("chkSelect"), CheckBox)
            If cb.Checked Then
                If bIsFirstRow Then
                    'save value of the first row, we'll check that all of the other rows are the same
                    sInitialValue = gvr.Cells(GridViewIndex).Text
                    bIsFirstRow = False
                End If
                'checkbox is checked so check
                If gvr.Cells(GridViewIndex).Text <> sInitialValue Then
                    bResult = False
                    Exit For
                End If
            End If
        Next
        'return the result
        Return bResult
    End Function

    ''' <summary>
    ''' loop through all checked quotes, store value, quote ref etc, set mode accordingly and redirect to payment select page
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnMakePayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMakePayment.Click
        If Page.IsValid Then
            'add selected quotes to array and store in session 
            'also store total value of premium to be collected
            Dim arrQuoteCollectionFiles As New ArrayList
            Dim decTotalForQuoteCollection As Decimal
            'Actually there is problem in acheiving the Page index through the Datatable method as Gridview only
            'Shows the rows which exist in the current page
            'so have used the session to populate the session objects here for insurance file

            Dim cDatastore As NexusProvider.QuoteCollection
            cDatastore = Session(CNQuoteCollectionSearchResults)

            For Each oQuote As NexusProvider.Quote In cDatastore
                'if quote is selected then update the Quote object as well
                If oQuote.IsSelected Then
                    Dim sRoundOff As String = String.Empty
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim m_cRoundOffAmount As Decimal

                    sRoundOff = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RoundOffToZero, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                    If sRoundOff.Equals("1") Then
                        m_cRoundOffAmount = Format((Math.Round(Convert.ToDecimal(oQuote.GrossTotal), 0) - oQuote.GrossTotal) + oQuote.GrossTotal, "0.00")
                        If (Math.Round(Convert.ToDecimal(oQuote.GrossTotal), 0) - oQuote.GrossTotal) = -0.5 Then
                            m_cRoundOffAmount = m_cRoundOffAmount + 1
                        Else
                            m_cRoundOffAmount = m_cRoundOffAmount
                        End If
                    Else
                        m_cRoundOffAmount = oQuote.GrossTotal
                    End If

                    If Session(CNCurrenyCode) Is Nothing AndAlso String.IsNullOrEmpty(oQuote.CurrencyCode) = False Then
                        Session(CNCurrenyCode) = oQuote.CurrencyCode.Trim
                    End If
                    'this quote is selected so add to the array list
                    arrQuoteCollectionFiles.Add(oQuote.InsuranceFileKey)
                    'Poupate the total Gross amount here
                    decTotalForQuoteCollection += m_cRoundOffAmount
                End If
            Next
            'store array list of insurance file keys and total for collection in session
            Session(CNQuoteCollectionFiles) = arrQuoteCollectionFiles
            Session(CNTotalForQuoteCollection) = decTotalForQuoteCollection

            'clear search results from session
            Session(CNQuoteCollectionSearchResults) = Nothing

            'redirect to paymentselect.aspx, passing quotecollection=true so that options are filtered accordingly
            Response.Redirect("~/secure/payment/paymentselect.aspx?quotecollection=true", False)
        End If
    End Sub

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'Cleaning of the session variable -Start
            ClearQuote()
            ClearClaims()
            ClearHeader()
            'Cleaning of the session variable -End
            FillProduct()
        End If
    End Sub
    Sub FillProduct()
        Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products

        For Each oProduct As Config.Product In oProducts
            drpProduct.Items.Add(New ListItem(oProduct.Name, oProduct.ProductCode))
        Next

        drpProduct.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_DefaultProduct"), ""))
    End Sub

    Protected Sub chkProduct_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkProduct.CheckedChanged
        If chkProduct.Checked Then
            PickList.Visible = True
            liSingleProduct.Visible = False

        Else
            PickList.Visible = False
            liSingleProduct.Visible = True

        End If
    End Sub

   

    Protected Sub grdQuotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdQuotes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oItem As NexusProvider.Quote = CType(e.Row.DataItem, NexusProvider.Quote)
            Dim TempGridRow As GridViewRow = e.Row
            Dim sRoundOff As String = String.Empty
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim dAmount As Decimal = Convert.ToDecimal(CType(e.Row.DataItem, NexusProvider.Quote).GrossTotal)

            sRoundOff = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RoundOffToZero, NexusProvider.RiskTypeOptions.None, CType(e.Row.DataItem, NexusProvider.Quote).ProductCode, Nothing, CType(e.Row.DataItem, NexusProvider.Quote).BranchCode).Trim()

            If sRoundOff.Equals("1") Then
                dAmount = Format((Math.Round(Convert.ToDecimal(dAmount), 0) - dAmount) + dAmount, "0.00")
                If (Math.Round(dAmount, 0) - dAmount) = -0.5 Then
                    dAmount += 1
                Else
                    dAmount = dAmount
                End If
            End If

            TempGridRow.Cells(6).Text = New Money(dAmount, oItem.CurrencyCode).Formatted.ToString
            Dim oCheckBox As CheckBox = e.Row.FindControl("chkSelect")
            'this helps in retaining the checkboxes already marked
            If CType(e.Row.DataItem, NexusProvider.Quote).IsSelected = True Then
                oCheckBox.Checked = True
            Else
                oCheckBox.Checked = False
            End If
        End If

    End Sub

    Protected Sub GrdChkSelected(ByVal sender As Object, ByVal e As System.EventArgs)
        'do this as in we need to know which all checkboxes were checked in case page is changed

        If Session(CNQuoteCollectionSearchResults) IsNot Nothing Then
            Dim oQuoteCol As NexusProvider.QuoteCollection = Session(CNQuoteCollectionSearchResults)
            Dim iClientKey As Integer = 0
            For jCount As Integer = 0 To grdQuotes.Rows.Count - 1
                Dim ChkSelected As CheckBox = grdQuotes.Rows(jCount).FindControl("chkSelect")
                If ChkSelected.Checked Then
                    For iCount As Integer = 0 To oQuoteCol.Count - 1
                        If grdQuotes.Rows(jCount).Cells(1).Text.Trim = oQuoteCol(iCount).InsuranceFileRef.Trim Then
                            oQuoteCol(iCount).IsSelected = True
                            'Following line will store the first selected record from grid
                            iClientKey = oQuoteCol(iCount).PartyKey
                            Exit For
                        End If
                    Next
                End If
            Next

            'Deselct unchecked policies
            For jCount As Integer = 0 To grdQuotes.Rows.Count - 1
                Dim ChkSelected As CheckBox = grdQuotes.Rows(jCount).FindControl("chkSelect")
                If ChkSelected.Checked = False Then
                    For iCount As Integer = 0 To oQuoteCol.Count - 1
                        If oQuoteCol(iCount).IsSelected = True And grdQuotes.Rows(jCount).Cells(1).Text.Trim = oQuoteCol(iCount).InsuranceFileRef.Trim Then
                            oQuoteCol(iCount).IsSelected = False
                            Exit For
                        End If
                    Next
                End If
            Next

            Dim bShowMakePayment As Boolean = True
            'Following code will check if grid contains checked records of same or different client 
            For pCount As Integer = 0 To oQuoteCol.Count - 1
                If oQuoteCol(pCount).IsSelected = True Then
                    If iClientKey <> oQuoteCol(pCount).PartyKey Then
                        bShowMakePayment = False
                        Exit For
                    End If
                End If
            Next

            'This is to make payment button visible
            btnMakePayment.Visible = bShowMakePayment

            'repopulate the sesssion back here
            Session(CNQuoteCollectionSearchResults) = oQuoteCol
        End If


    End Sub
  

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If HttpContext.Current.Session.IsCookieless Then
            btnQuote.OnClientClick = "tb_show(null ,'../Modal/FindInsuranceFile.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
            btnClient.OnClientClick = "tb_show(null ,'../secure/agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
            btnAgent.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?FromPage=MainDetails&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
        Else
            btnQuote.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindInsuranceFile.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
            btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
            btnAgent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindAgent.aspx?FromPage=MainDetails&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
        End If
    End Sub
End Class
