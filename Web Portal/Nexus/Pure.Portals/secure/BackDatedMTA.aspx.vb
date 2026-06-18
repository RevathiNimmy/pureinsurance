Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Reflection
Imports System.Web.Configuration
Imports System.Xml
Imports Nexus


Partial Class BackDatedMTA : Inherits Frontend.clsCMSPage

    Private Const sPremiumDisplayPath As String = "~/secure/PremiumDisplay.aspx"
    Private Const sStatementsPath As String = "~/secure/Statements.aspx"

    Private oBackdatedVersions As NexusProvider.PolicyCollection
    Private oWebService As NexusProvider.ProviderBase
    Private oQuote As NexusProvider.Quote
    Private iUnquotedCount As Integer

    ' Running total variables.
    Private dPremiumTotal As Double = 0
    Private dCommisionTotal As Double = 0
    Private dFeeTotal As Double = 0

    Private dEndorsementPremiumTotal As Double = 0
    Private dEndorsementCommisionTotal As Double = 0
    Private dEndorsementFeeTotal As Double = 0

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

    End Sub

    ''' <summary>
    ''' If the session is blank then call SAM Method GetBackdatedMTAPolicyVersions
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            If Session(CNBackDatedVersions) IsNot Nothing Then
                'Create a backdated versions collection from session.
                oBackdatedVersions = Session(CNBackDatedVersions)
            Else
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    'Call SAM Method for getting back dated versions.
                    oBackdatedVersions = oWebService.GetBackdatedMTAPolicyVersions(CType(Session(CNBaseInsuranceFileKey), Integer))
                    'Add backdated version in session for further use.
                    Session(CNBackDatedVersions) = oBackdatedVersions
                Finally
                    oWebService = Nothing
                End Try
            End If

            If Session(CNOriginalMTAType) IsNot Nothing Then
                Session(CNMTAType) = Session(CNOriginalMTAType)
            End If

            ' Bind the grid with backdatedversions
            iUnquotedCount = 0
            grdBackdatedMTA.DataSource = oBackdatedVersions
            grdBackdatedMTA.DataBind()


        End If

        ' Check if all risks for all quotes are quoted or not
        CheckAllRisksQuoteStatus()

    End Sub

    ''' <summary>
    ''' Check the status for all the risks. User can proceed if all risks are quoted.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckAllRisksQuoteStatus()

        oBackdatedVersions = Session(CNBackDatedVersions)

        If Session(CNMTAType) <> MTAType.CANCELLATION Then
            Dim bIsAllRiskQuoted As Boolean = True

            ' If any of the risks is unquoted then we need to display an alert on OK click.
            For Each oPolicyVersion As NexusProvider.Policy In oBackdatedVersions
                If oPolicyVersion.QuoteStatus.ToUpper() = RiskStatus.UnQuoted Then
                    bIsAllRiskQuoted = False
                    Exit For
                End If
            Next

            ' If all risks are not quoted then user should not be able to proceed.
            If bIsAllRiskQuoted = False Then
                btnOK.Attributes.Add("onclick", "alert('" & GetLocalResourceObject("msg_RiskUnquotedAlert").ToString() & "');return false;")
            Else
                If (Math.Abs(dEndorsementPremiumTotal) < Math.Abs(dEndorsementCommisionTotal)) Then
                    If Math.Abs(dCommisionTotal + dEndorsementCommisionTotal) >= 0.001 Then
                        btnOK.Attributes.Add("onclick", "alert('" & GetLocalResourceObject("msg_CommisionGreaterthanPremiumAlert").ToString() & "');return false;")
                    Else
                        btnOK.Attributes.Remove("onclick")
                    End If
                Else

                    btnOK.Attributes.Remove("onclick")
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Handle the grid data bound. Populate the footer values with the calculated total values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdBackdatedMTA_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBackdatedMTA.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim btnEdit As LinkButton = e.Row.FindControl("lnkbtnEdit")
            Dim btnView As LinkButton = e.Row.FindControl("lnkbtnView")
            Dim oPolicy As NexusProvider.Policy = CType(e.Row.DataItem, NexusProvider.Policy)
            Dim iInsuranceFileCnt = oPolicy.InsuranceFileKey

            btnEdit.CommandArgument = iInsuranceFileCnt
            btnView.CommandArgument = iInsuranceFileCnt

            ' Add to total variables.
            dPremiumTotal += oPolicy.OriginalMtaPremium
            dCommisionTotal += oPolicy.OriginalCommission
            dFeeTotal += oPolicy.OriginalFee

            dEndorsementPremiumTotal += oPolicy.Premium
            dEndorsementCommisionTotal += oPolicy.MtaCommission
            dEndorsementFeeTotal += oPolicy.MtaFee

            ' First row is always the base version. Do not allow to edit.
            If e.Row.DataItemIndex = 0 Then
                e.Row.Cells(3).Text = "Main"
                e.Row.Cells(10).Text = ""
                Return
            End If

            ' No status or edit link when MTC and MTR.
            If Session(CNMTAType) = MTAType.CANCELLATION OrElse Session(CNMTAType) = MTAType.REINSTATEMENT Then
                e.Row.Cells(10).Text = ""
            Else
                ' Tally the unquoted records.
                If oPolicy.QuoteStatus.ToUpper() = RiskStatus.UnQuoted Then
                    iUnquotedCount += 1
                End If

                ' Check if the iUnquotedCount > 1 then disable edit since editing is only allowed from top down.
                If iUnquotedCount > 1 Then
                    btnEdit.Enabled = False
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then

            ' Get the Currency format values from the config file.
            Dim oFormatString As Config.FormatString = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency")

            ' Set the values for the required total columns in the footer. Format of the formatter is used.
            e.Row.Cells(0).Text = "Total Adjusted"
            e.Row.Cells(5).Text = String.Format(oFormatString.DataFormatString, dPremiumTotal + dEndorsementPremiumTotal)
            e.Row.Cells(7).Text = String.Format(oFormatString.DataFormatString, dCommisionTotal + dEndorsementCommisionTotal)
            e.Row.Cells(9).Text = String.Format(oFormatString.DataFormatString, dFeeTotal + dEndorsementFeeTotal)

            ' Set the cell class to that of the formatter.
            e.Row.Cells(5).CssClass = oFormatString.HeaderStyleCssClass
            e.Row.Cells(7).CssClass = oFormatString.ItemStyleCssClass
            e.Row.Cells(9).CssClass = oFormatString.ItemStyleCssClass

        End If

    End Sub

    ''' <summary>
    ''' Handle the grid row command.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdBackdatedMTA_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdBackdatedMTA.RowCommand

        ' NOTE: Logic here is based on ClientQuotes.ascx.vb grdvQuotes_RowCommand.

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oQuote As NexusProvider.Quote
        Dim nCurrentInsuranceFileKey As Integer = CInt(e.CommandArgument)
        Dim nPrechangeInsuranceFileKey As Integer = 0
        Dim sCommand As String = e.CommandName
        Dim oCurrentGridRow As GridViewRow = Nothing

        If sCommand = "edit" Then
            oCurrentGridRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            nPrechangeInsuranceFileKey = Convert.ToInt32(grdBackdatedMTA.DataKeys(oCurrentGridRow.RowIndex).Value)
        End If

        Session.Remove(CNOldPremium) 'Remove the old premium from session
        Session.Remove(CNRiskType) 'Reset the Risk Type
        ClearClaims() 'to Clear the claim session variable if any

        ' Get the quote and details for the selected insurance file cnt.
        oQuote = oWebService.GetHeaderAndSummariesByKey(nCurrentInsuranceFileKey, v_nPreChangeInsuranceFileKey:=nPrechangeInsuranceFileKey)
        oWebService.GetHeaderAndRisksByKey(oQuote)

        ' QuoteExpiryDate doesn't get populated on the backdated version.
        ' Set it to the current date.
        oQuote.QuoteExpiryDate = Date.Now
        ' Place the quote back into the session.
        Session(CNQuote) = oQuote
        Session(CNCurrenyCode) = oQuote.CurrencyCode
        'Session(CNMTAType) = Nothing
        Session(CNInsuranceFileKey) = nCurrentInsuranceFileKey
        Session(CNCurrentRiskKey) = Nothing

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oProductConfiguration As New Nexus.Library.Config.Product
        Dim oPortalConfig As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)
        Dim oRiskType As Config.RiskType
        If oProductConfiguration.AllowMultiRisks = False Or oQuote.Risks.Count = 1 Then
            If oQuote.Risks(0).RiskCode Is Nothing Then
                oRiskType = oProductConfiguration.RiskTypes.RiskType(oQuote.Risks(0).RiskTypeCode.Trim)
            Else
                oRiskType = oProductConfiguration.RiskTypes.RiskType(oQuote.Risks(0).RiskCode)
            End If
            Session(CNDataModelCode) = oRiskType.DataModelCode
            oWebService.GetRisk(oQuote.Risks(0).Key, 0, oQuote)
            Session(CNQuote) = oQuote
        End If

        Select Case sCommand
            Case "view"
                Session(CNRenewal) = Nothing
                If oQuote.InsuranceFileTypeCode = "MTACAN" Then
                    Session(CNMTAType) = MTAType.CANCELLATION
                    'Hold the View Type of Selected InsuranceFileType
                    Session(CNViewType) = ViewType.CANCELLATION_MTA
                ElseIf oQuote.InsuranceFileTypeCode = "MTA PERM" Then
                    'Hold the View Type of Selected InsuranceFileType
                    Session(CNViewType) = ViewType.PERMANENT_MTA
                ElseIf oQuote.InsuranceFileTypeCode = "MTA TEMP" Then
                    'Hold the View Type of Selected InsuranceFileType
                    Session(CNViewType) = ViewType.TEMPORARY_MTA
                End If
                Session(CNMode) = Mode.View
                Session.Remove(CNOI)
                Session(CNQuoteInSync) = False
                Session(CNQuoteMode) = QuoteMode.MTAQuote

            Case "edit"

                Session(CNRenewal) = Nothing
                Session(CNMode) = Mode.Edit
                Session(CNQuoteInSync) = False
                Session.Remove(CNOI)
                Session(CNQuoteInSync) = False
                Session(CNQuoteMode) = QuoteMode.FullQuote
                Session.Remove(CNIsInteractiveBackdatedMTA)

                If grdBackdatedMTA.Rows(oCurrentGridRow.RowIndex).Cells.Count > 0 AndAlso grdBackdatedMTA.Rows(oCurrentGridRow.RowIndex).Cells(0) IsNot Nothing Then
                    If grdBackdatedMTA.Rows(oCurrentGridRow.RowIndex).Cells(0).Text.ToUpper = "MTA QUOTATION CANCELLATION" Then
                        Session(CNOriginalMTAType) = Session(CNMTAType)
                        Session(CNMTAType) = MTAType.CANCELLATION
                    ElseIf grdBackdatedMTA.Rows(oCurrentGridRow.RowIndex).Cells(0).Text.ToUpper = "MTA QUOTATION REINSTATEMENT" Then
                        Session(CNOriginalMTAType) = Session(CNMTAType)
                        Session(CNMTAType) = MTAType.REINSTATEMENT
                    End If
                End If
        End Select

        Response.Redirect(sPremiumDisplayPath, False)

    End Sub

    ''' <summary>
    ''' Handle the grid row editing.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdBackdatedMTA_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdBackdatedMTA.RowEditing

        ' Nothing to do here. Catching the event thrown by the grid.

    End Sub

    ''' <summary>
    ''' if all risks are quoted then we need to forward for payment process 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ' Update session and cleanup.
        Session(CNMode) = Mode.Edit
        Session(CNQuoteInSync) = False
        Session.Remove(CNOI)
        ' Assign CNInsuranceFileKey=CNBaseInsuranceFileKey as CNBaseInsuranceFileKey is going to be used for further process
        Session(CNInsuranceFileKey) = Session(CNBaseInsuranceFileKey)

        ' Create Quote Object for Base insurance file Key.
        oWebService = New NexusProvider.ProviderManager().Provider
        oQuote = oWebService.GetHeaderAndSummariesByKey(Session(CNInsuranceFileKey))

        For iCount As Integer = 0 To oQuote.Risks.Count - 1
            oWebService.GetRisk(oQuote.Risks(iCount).Key, iCount, oQuote)
        Next
        oWebService.GetHeaderAndRisksByKey(oQuote)

        Session(CNQuote) = oQuote

        'Remove exisitng sessions
        Session.Remove(CNIsInteractiveBackdatedMTA)
        Session.Remove(CNBackDatedVersions)
        Session.Remove(CNBaseInsuranceFileKey)
        Session.Remove(CNDeletedNode)

        ' Check statements is set to true in web.config and then will redirect to statements page.
        If CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).ShowStatements = True Then
            Response.Redirect(sStatementsPath)
        Else ' Else will redirect to transaction confirmation page directly.
            ' Update the premium with agent commision if agent type is BROKER.
            UpdatePremiumWithAgentCommision()
            Response.Redirect(CheckPremiumAndRedirect())
        End If

    End Sub

    ''' <summary>
    ''' Clear backdated sessions and redirect to PremiumDisplay page
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ' Create the original Quote object.
        Try
            oWebService = New NexusProvider.ProviderManager().Provider

            oWebService.DeleteBackDatedVersions(Session(CNBaseInsuranceFileKey))

            oQuote = oWebService.GetHeaderAndSummariesByKey(Session(CNBaseInsuranceFileKey))

            If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                oWebService.GetRisk(oQuote.Risks(0).Key, 0, oQuote, oQuote.BranchCode)
            End If
            oWebService.GetHeaderAndRisksByKey(oQuote)

            Session(CNQuote) = oQuote
        Finally
            oWebService = Nothing
        End Try

        ' Remove all backdated Sessions
        Session.Remove(CNIsInteractiveBackdatedMTA)
        Session.Remove(CNBackDatedVersions)
        Session.Remove(CNBaseInsuranceFileKey)
        Session.Remove(CNDeletedNode)
        Response.Redirect(sPremiumDisplayPath)

    End Sub
    ''' <summary>
    ''' this will save the OOS MTA quotes for current policy
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSaveOOSMTAQuote_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveOOSMTAQuote.Click
        If Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Convert.ToInt32(Session(CNBaseInsuranceFileKey)) > 0 Then
            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                oQuote = oWebService.GetHeaderAndSummariesByKey(Session(CNBaseInsuranceFileKey))

                If Not oQuote.Risks Is Nothing AndAlso oQuote.Risks.Count > 0 Then
                    oWebService.GetRisk(oQuote.Risks(0).Key, 0, oQuote, oQuote.BranchCode)
                End If
                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote
            Finally
                oWebService = Nothing
            End Try

            ' Remove all backdated Sessions
            Session.Remove(CNIsInteractiveBackdatedMTA)
            Session.Remove(CNBackDatedVersions)
            Session.Remove(CNBaseInsuranceFileKey)
            Session.Remove(CNDeletedNode)
            Response.Redirect(sPremiumDisplayPath)
        End If
    End Sub


End Class
