Imports CMS.Library
Imports System.Data
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports System.Xml
Imports Nexus.Reinsurance
Partial Class Modal_FACPlacement
    Inherits Frontend.clsCMSPage
    Dim oReinsurerColl As New NexusProvider.ReinsurerCollection
    Dim oFilteredReinsurer As New NexusProvider.Reinsurer
    Dim oRIArrangementLinescol As New NexusProvider.ReinsuranceArrangementLineCollection
    Dim oRIArrangemenline As NexusProvider.ReinsuranceArrangementLines
    Dim oFilteredReinsurerColl As New NexusProvider.ReinsurerCollection
    Dim oFilteredReinsurerXOLColl As New FAXParticipantsCollection
    Dim oReinsurerXOLColl As New ArrangementLinesTypeCollection
    Dim oFilteredReinsurerXOL As New FAXParticipants
    Dim FilteredReinsurerXOLpageCacheID As New Guid
    Dim oFilteredReinsurerXOLTemp As New ArrangementLinesType
    Dim sRICode As String
    Dim oWebService As NexusProvider.ProviderBase
    Dim oQuote As NexusProvider.Quote
    Dim oFormatStringCurrency As String
    Dim oFormatStringPercentage As String

#Region "Page Event"
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sType As String = String.Empty
        Dim sVMode As String = String.Empty
        Dim sCMode As String = String.Empty
        Dim iRIArrangementLineKey As Integer
        Dim oArrangementLineType As ArrangementLinesType = Nothing
        oFormatStringCurrency = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
        oFormatStringPercentage = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Percentage").DataFormatString
        'Type carries the RI Type i.e. FAC Prop or  FAC XOL
        If Request.QueryString("Type") IsNot Nothing Then
            sType = Request.QueryString("Type")
        End If

        'Mode carries values like Edit,View etc
        If Request.QueryString("Mode") IsNot Nothing Then
            sVMode = Request.QueryString("Mode").ToUpper.Trim
        End If

        'This mode is populated in the case of FAC XOL only, it carries Add FAX Pariticipant Mode
        If Request.QueryString("CMode") IsNot Nothing Then
            sCMode = Request.QueryString("CMode").ToUpper.Trim
        End If



        If Not IsPostBack Then
            'create a unique key and add this to viewstate
            'this will be used to cache the results of the SAM call
            Dim ReinsurerpageCacheID As Guid

            ReinsurerpageCacheID = Guid.NewGuid()
            ViewState.Add("ReinsurerpageCacheID", ReinsurerpageCacheID.ToString)


            'Retreival of RI Arrangement Line Key
            Integer.TryParse(Request.QueryString("RIArrangementLineKey"), iRIArrangementLineKey)

            'In case of Claim only
            If Session(CNMode) = Mode.NewClaim OrElse Session(CNMode) = Mode.EditClaim OrElse Session(CNMode) = Mode.PayClaim _
            OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery OrElse (sVMode = "VIEW" And Session(CNMode) = Mode.ViewClaim) Then
                If sVMode = "VIEW" Then
                    If iRIArrangementLineKey > 0 Then
                        oArrangementLineType = GetRIArrangementLineFromXML(iRIArrangementLineKey, Session(CNRIXMLData), "", True)
                        Session(CNRIFACXol) = oArrangementLineType
                    End If
                End If
            ElseIf String.IsNullOrEmpty(sCMode) = True Then
                'NB/MTA/MTC/MTR/REN
                If iRIArrangementLineKey > 0 Then
                    oArrangementLineType = GetRIArrangementLineFromXML(iRIArrangementLineKey, Session(CNRIXMLData), sType, False)
                    Session(CNRIFACXol) = oArrangementLineType
                End If
            End If

            'Changing the Title based on Type i.e. FAC Prop or FAC XOL
            If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                lblTitle.Text = GetLocalResourceObject("lbl_FACProp_title")
                PopulateDropDown()
            ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                lblTitle.Text = GetLocalResourceObject("lbl_FACXOL_title")
                pnlParticipants.Visible = True
                PopulateDropDown()
                Dim bFAXPresent As Boolean = False
                'If session(CNRIFACXol) is populated then 
                'show the lower limit and line limit
                If Session(CNRIFACXol) IsNot Nothing Then
                    Dim oOrgArrangementLine As ArrangementLinesType = Session(CNRIFACXol)
                    txtLower.Text = oOrgArrangementLine.LowerLimit.ToString
                    txtUpper.Text = oOrgArrangementLine.LineLimit.ToString
                    pnlFACPlacement.Visible = True
                    grdPlacements.Visible = True
                    'populating the grid
                    grdPlacements.DataSource = oOrgArrangementLine.FAXParticipants
                    grdPlacements.DataBind()

                    'calculating the percentage
                    processtextchange()
                End If
            End If


            'In case of View mode these should be disabled
            If sVMode = "VIEW" Then
                pnlFACPlacement.Enabled = False
                btnSearch.Enabled = False
                btnNewSearch.Enabled = False
                grdvSearchResults.Enabled = False
                pnlParticipants.Visible = True
            End If
        End If
        FindReInsurer()

    End Sub
#End Region

#Region "Grid Events"


    Protected Sub grdvSearchResults_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.DataBound
        Dim sType As String
        If Request.QueryString("Type") IsNot Nothing Then
            sType = Request.QueryString("Type")
            If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                grdvSearchResults.Columns(2).Visible = False
            End If
        End If
    End Sub

    Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging
        grdvSearchResults.PageIndex = e.NewPageIndex
        FindReInsurer()
        ' uPFindReinsurer.Update()
    End Sub

    Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
        Dim sVMode As String
        If Request.QueryString("Mode") IsNot Nothing Then
            sVMode = Request.QueryString("Mode").Trim
        End If
        If e.CommandName IsNot Nothing Then
            Select Case e.CommandName
                Case "Select"
                    Dim sType As String
                    If Request.QueryString("Type") IsNot Nothing Then
                        sType = Request.QueryString("Type")
                        'FAC Prop
                        If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                            Dim bBroker As Boolean
                            Dim oFilteredReinsurerColl As New NexusProvider.ReinsurerCollection
                            Dim oRIArrangementLines As New ArrangementLinesType

                            If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
                                oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
                            End If
                            'Populating the objects with selected values
                            For iCount As Integer = 0 To oReinsurerColl.Count - 1
                                If Server.HtmlEncode(Trim(oReinsurerColl.Item(iCount).ReinsurerCode)).ToUpper = Server.HtmlEncode(Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text)).ToUpper Then
                                    oRIArrangementLines.ReinsurerCode = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                                    oRIArrangementLines.RIName = oReinsurerColl.Item(iCount).ReinsurerCode
                                    oRIArrangementLines.PartyKey = oReinsurerColl.Item(iCount).ReinsurerKey

                                    oRIArrangementLines.ParticipationPercent = oReinsurerColl.Item(iCount).ParticipationPercentage
                                    oRIArrangementLines.ReinsuranceTypeCode = oReinsurerColl.Item(iCount).ReinsuranceTypeCode
                                    oRIArrangementLines.RIPlacement = "FAC Prop"
                                    oRIArrangementLines.IsBroker = oReinsurerColl.Item(iCount).IsBroker
                                    oRIArrangementLines.Type = "F"
                                    oRIArrangementLines.CommissionPerc = oReinsurerColl.Item(iCount).CommissionPercentage
                                    bBroker = oReinsurerColl.Item(iCount).IsBroker
                                    Exit For
                                End If
                            Next
                            oRIArrangementLines.LineLimit = Convert.ToDouble(txtUpper.Text)
                            oRIArrangementLines.LowerLimit = Convert.ToDouble(txtLower.Text)
                            'Populating the session with selected/populated objects
                            Session.Add(CNRIFACProp, oRIArrangementLines)
                            sRICode = oRIArrangementLines.ReinsurerCode.Trim
                            'in case of broker user need to be on anoter screen
                            If bBroker Then
                                Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Code=" & Uri.EscapeDataString(sRICode.Trim) & "&Type=" & Request.QueryString("Type") & "&BandKey=" & Session(CNRIBandKey))
                            Else
                                'or else it should be added into the RI grid with child
                                oFilteredReinsurerColl.Add(oFilteredReinsurer)
                                Dim iNewId As Integer
                                GenerateUniqueRILineId(iNewId)
                                Dim oReinsurance2007 As New Reinsurance
                                If Session(CNMode) = Mode.PortFolioTransferAmendment OrElse Session(CNMode) = Mode.ClonedTransferAmendment Then
                                    oReinsurance2007.AddRIArrangementLines(Session(CNRIXMLData), Session(CNRIFACProp), Session(CNRIBandKey), iNewId.ToString, Session(CNRIArrangementkey), "F", Session(CNRITransactionType), bIsPortfolioRIAmendment:=True)
                                Else
                                    oReinsurance2007.AddRIArrangementLines(Session(CNRIXMLData), Session(CNRIFACProp), Session(CNRIBandKey), iNewId.ToString, Session(CNRIArrangementkey), "F", Session(CNRITransactionType))
                                End If
                                Session.Remove(CNRIFACProp)
                                Response.Redirect("~/Secure/ReInsuranceDetails.aspx?RICode=" & Uri.EscapeDataString(oRIArrangementLines.RIName.ToString.Trim))
                            End If
                        ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                            Dim bBroker As Boolean
                            'Checking the broker existence
                            If CheckCollectionOfPartcipants(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text) = True Then
                                'javascript message
                                Dim sMessage As String
                                sMessage = GetLocalResourceObject("RIBrokerExist_Msg")
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                                Exit Sub
                            Else
                                If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
                                    oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
                                End If
                                For iCount As Integer = 0 To oReinsurerColl.Count - 1
                                    If Server.HtmlEncode(oReinsurerColl.Item(iCount).ReinsurerCode.Trim).ToUpper = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToUpper) Then
                                        If oReinsurerColl.Item(iCount).IsBroker = True Then
                                            bBroker = True
                                            sRICode = oReinsurerColl.Item(iCount).ReinsurerCode
                                        End If
                                        'if this session variable is nothing means that user is added the FAC XOL for first time
                                        If Session(CNRIFACXol) Is Nothing Then
                                            oFilteredReinsurerXOLTemp.ReinsurerCode = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                                            oFilteredReinsurerXOLTemp.RIName = oReinsurerColl.Item(iCount).RIName
                                            oFilteredReinsurerXOLTemp.ParticipationPercent = 100
                                            oFilteredReinsurerXOLTemp.ReinsuranceTypeCode = oReinsurerColl.Item(iCount).ReinsuranceTypeCode
                                            oFilteredReinsurerXOLTemp.RIPlacement = "FAC XOL"
                                            oFilteredReinsurerXOLTemp.IsBroker = oReinsurerColl.Item(iCount).IsBroker
                                            oFilteredReinsurerXOLTemp.Type = "FX"
                                            oFilteredReinsurerXOLTemp.LowerLimit = Convert.ToDouble(txtLower.Text)
                                            oFilteredReinsurerXOLTemp.LineLimit = Convert.ToDouble(txtUpper.Text)
                                            oFilteredReinsurerXOLTemp.PartyKey = oReinsurerColl.Item(iCount).ReinsurerKey
                                            oFilteredReinsurerXOLTemp.IsRIBroker = bBroker
                                            Session(CNRIFACXol) = oFilteredReinsurerXOLTemp
                                        Else
                                            'or else retreive the already added data from session
                                            Dim oFXArrangementLine As ArrangementLinesType = Session(CNRIFACXol)
                                            Double.TryParse(txtLower.Text, oFXArrangementLine.LowerLimit)
                                            Double.TryParse(txtUpper.Text, oFXArrangementLine.LineLimit)
                                            Session(CNRIFACXol) = oFXArrangementLine
                                        End If
                                        'Populating the objects with selected values
                                        oFilteredReinsurerXOL.PartyCode = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text)
                                        oFilteredReinsurerXOL.PartyName = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(1).Text)
                                        oFilteredReinsurerXOL.AccountType = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(2).Text)
                                        oFilteredReinsurerXOL.PartyKey = oReinsurerColl.Item(iCount).ReinsurerKey
                                        oFilteredReinsurerXOL.SumInsured = 0
                                        oFilteredReinsurerXOL.PremiumValue = 0
                                        oFilteredReinsurerXOL.PremiumTax = 0
                                        oFilteredReinsurerXOL.CommissionPercent = oReinsurerColl.Item(iCount).CommissionPercentage
                                        oFilteredReinsurerXOL.CommissionTax = 0
                                        oFilteredReinsurerXOL.CommissionValue = 0
                                        oFilteredReinsurerXOL.AgreementCode = ""
                                        oFilteredReinsurerXOL.ParticipationPercentage = 0

                                        Exit For
                                    End If
                                Next

                                If bBroker Then
                                    'if it is broker then update the session temporarely to access the same in next page
                                    Session.Add(CNRIFACXolTemp, oFilteredReinsurerXOL)
                                    If sVMode IsNot Nothing Then
                                        If sVMode.ToUpper.Trim = "EDIT" Then
                                            Dim iRIArrLineKey As Integer = 0
                                            If Request.QueryString("RIArrangementLineKey") IsNot Nothing Then
                                                iRIArrLineKey = Convert.ToInt32(Request.QueryString("RIArrangementLineKey").Trim)
                                            End If
                                            Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Code=" & Uri.EscapeDataString(sRICode.Trim) & "&Type=" & Request.QueryString("Type").Trim & "&Mode=" & sVMode.Trim & "&RIArrangementLineKey=" & iRIArrLineKey.ToString.Trim)
                                        Else
                                            Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Code=" & Uri.EscapeDataString(sRICode.Trim) & "&Type=" & Request.QueryString("Type").Trim & "&Mode=" & sVMode.Trim)
                                        End If
                                    Else
                                        Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Code=" & Uri.EscapeDataString(sRICode.Trim) & "&Type=" & Request.QueryString("Type").Trim)
                                    End If
                                Else
                                    'if not broker then add it into child grid
                                    Dim oFXArrangementLine As ArrangementLinesType = Session(CNRIFACXol)
                                    oFXArrangementLine.FAXParticipants.Add(oFilteredReinsurerXOL)
                                    Session(CNRIFACXol) = oFXArrangementLine
                                    pnlFACPlacement.Visible = True
                                    grdPlacements.Visible = True
                                    grdPlacements.DataSource = oFXArrangementLine.FAXParticipants
                                    grdPlacements.DataBind()
                                    If grdPlacements.Rows.Count > 1 Then
                                        'calculating the grid footer row, if any changes happened
                                        'along with other functionality
                                        processtextchange()
                                    End If
                                End If
                            End If
                        End If
                    End If
            End Select
        End If
    End Sub

    Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oReInsurer As NexusProvider.Reinsurer = CType(e.Row.DataItem, NexusProvider.Reinsurer)
            Dim lnkSelect As LinkButton = e.Row.FindControl("btnSelect")

            'NOTE - this will need to be changed to give each row a unique id
            'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ReinsurerCode") %>")
            e.Row.Attributes.Add("id", oReInsurer.ReinsurerCode.Trim)
            e.Row.Cells(6).Text = "Reinsurer"
            If oReInsurer.IsBroker Then
                lnkSelect.CommandArgument = e.Row.RowIndex
            Else
                lnkSelect.CommandArgument = e.Row.RowIndex
            End If
        End If
    End Sub

    Protected Sub grdPlacements_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPlacements.DataBound
        grdPlacements.Columns(12).Visible = False
        If Session(CNMode) = Mode.NewClaim OrElse Session(CNMode) = Mode.EditClaim OrElse Session(CNMode) = Mode.PayClaim _
           OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery OrElse Session(CNMode) = Mode.ViewClaim Then
            grdPlacements.Columns(5).Visible = False
            grdPlacements.Columns(6).Visible = False
            grdPlacements.Columns(6).Visible = False
            grdPlacements.Columns(7).Visible = False
            grdPlacements.Columns(8).Visible = False
            grdPlacements.Columns(9).Visible = False
        End If
    End Sub
    ''' <summary>
    ''' grdPlacements RowCommand
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdPlacements_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdPlacements.RowCommand
        Dim sMode As String = String.Empty
        Dim sPartyCode As String
        Dim sRIArrLineKey As String = "0"
        If Request.QueryString("RIArrangementLineKey") IsNot Nothing Then
            sRIArrLineKey = Request.QueryString("RIArrangementLineKey").Trim
        End If
        If Request.QueryString("Mode") IsNot Nothing Then
            sMode = Request.QueryString("Mode").ToUpper.ToString
        End If
        If e.CommandName IsNot Nothing Then
            Select Case e.CommandName
                Case "EditPlacement", "View"
                    Dim bBrokerPresent As Boolean = False
                    Dim oArrangementLineType As ArrangementLinesType = Session(CNRIFACXol)
                    sPartyCode = grdPlacements.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToUpper.Trim
                    'checking the broker existance
                    If oArrangementLineType IsNot Nothing AndAlso oArrangementLineType.FAXParticipants IsNot Nothing Then
                        For iCount As Integer = 0 To oArrangementLineType.FAXParticipants.Count - 1
                            If oArrangementLineType.FAXParticipants(iCount).PartyCode.ToUpper.Trim = sPartyCode _
                            AndAlso (oArrangementLineType.FAXParticipants(iCount).BrokerParticipants IsNot Nothing _
                                     AndAlso oArrangementLineType.FAXParticipants(iCount).BrokerParticipants.Count > 0) Then
                                bBrokerPresent = True
                                Exit For
                            End If
                        Next
                    End If
                    If bBrokerPresent = True Then
                        If String.IsNullOrEmpty(sPartyCode) = False Then
                            If String.IsNullOrEmpty(sMode) = False Then
                                Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Code=" & Uri.EscapeDataString(sPartyCode.Trim) & "&Type=FACXOL" & "&Mode=" & sMode & "&RIArrangementLineKey=" & sRIArrLineKey.Trim & "&PMode=" & e.CommandName.ToUpper.Trim)
                            Else
                                Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Code=" & Uri.EscapeDataString(sPartyCode.Trim) & "&Type=FACXOL" & "&RIArrangementLineKey=" & sRIArrLineKey.Trim & "&PMode=" & e.CommandName.ToUpper.Trim)
                            End If

                        Else
                            If String.IsNullOrEmpty(sMode) = False Then
                                Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Type=FACXOL" & "&Mode=" & sMode & "&RIArrangementLineKey=" & sRIArrLineKey.Trim & "&PMode=" & e.CommandName.ToUpper.Trim)
                            Else
                                Response.Redirect("~/Modal/RIBrokerParticipant.aspx?Type=FACXOL" & "&RIArrangementLineKey=" & sRIArrLineKey.Trim & "&PMode=" & e.CommandName.ToUpper.Trim)
                            End If
                        End If
                    Else
                        'javascript message
                        Dim sMessage As String
                        sMessage = GetLocalResourceObject("RIBroker_Err")
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                        Exit Sub
                    End If

                Case "Remove"
                    'Removing the data from grid and session as well updating the session with current child
                    Dim oFXArrangementLine As ArrangementLinesType = Session(CNRIFACXol)
                    sPartyCode = grdPlacements.Rows(Convert.ToInt32(e.CommandArgument)).Cells(0).Text
                    Dim iRowCount As Integer
                    If oFXArrangementLine IsNot Nothing AndAlso oFXArrangementLine.FAXParticipants IsNot Nothing Then
                        iRowCount = oFXArrangementLine.FAXParticipants.Count - 1
                    End If

                    For iCount As Integer = 0 To iRowCount
                        If oFXArrangementLine.FAXParticipants(iCount).PartyCode.ToUpper.Trim = sPartyCode.ToUpper.Trim Then
                            oFXArrangementLine.FAXParticipants.RemoveAt(iCount)
                            Exit For
                        End If
                    Next
                    Session(CNRIFACXol) = oFXArrangementLine
                    grdPlacements.DataSource = oFXArrangementLine.FAXParticipants
                    grdPlacements.DataBind()
                    If grdPlacements.Rows.Count = 0 Then
                        grdPlacements.Visible = False
                    Else
                        'calculating the grid footer row, if any changes happened
                        'along with other functionality
                        processtextchange()
                    End If
            End Select
        End If
    End Sub

    Protected Sub grdPlacements_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPlacements.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            grdPlacements.Columns(12).Visible = True
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim linkView As LinkButton = e.Row.FindControl("btnView")
            linkView.CommandArgument = e.Row.RowIndex

            Dim linkRemove As LinkButton = e.Row.FindControl("btnRemove")
            linkRemove.CommandArgument = e.Row.RowIndex
            Dim lnkedit As LinkButton = e.Row.FindControl("btnEdit")
            lnkedit.CommandArgument = e.Row.RowIndex
            Dim txtParticipation As TextBox = e.Row.FindControl("txtParticipation")
            Dim s1 As String = txtParticipation.ClientID
            txtParticipation.Visible = True
            Dim txtPremium As TextBox = e.Row.FindControl("txtPremium")

            Dim txtComPerc As TextBox = e.Row.FindControl("txtComPerc")
            Dim txtAgreeCode As TextBox = e.Row.FindControl("txtAgreeCode")
            If Request.QueryString("Mode") IsNot Nothing Then
                If Request.QueryString("Mode").ToUpper.Trim = "VIEW" Then
                    btnSearch.Enabled = False
                    btnNewSearch.Enabled = False
                    lnkedit.Enabled = False
                    linkRemove.Enabled = False
                    txtUpper.Enabled = False
                    txtLower.Enabled = False
                    btnCancel.Enabled = True
                    btnOk.Enabled = True
                    txtParticipation.Enabled = False
                    txtPremium.Enabled = False
                    txtComPerc.Enabled = False
                    txtAgreeCode.Enabled = False
                End If
            End If
        End If
    End Sub

    Protected Sub grdvSearchResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSearchResults.SelectedIndexChanged
        Dim row As GridViewRow = grdvSearchResults.SelectedRow
        sRICode = row.Cells(0).Text
    End Sub

#End Region

#Region "Button Event"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FindReInsurer(True, False)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim sType As String = Request.QueryString("Type")
        Dim sVMode As String = String.Empty

        If Request.QueryString("Mode") IsNot Nothing Then
            sVMode = Request.QueryString("Mode")
        End If

        If Session(CNMode) = Mode.NewClaim OrElse Session(CNMode) = Mode.EditClaim OrElse Session(CNMode) = Mode.PayClaim _
         OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery OrElse Session(CNMode) = Mode.ViewClaim Then
            Response.Redirect("~/Claims/ClaimReinsurance.aspx")
        Else
            If sType.ToUpper.Trim = "FACXOL" AndAlso sVMode.ToUpper.Trim = "EDIT" Then
                Dim nRIArrLineKey As Integer
                If Request.QueryString("RIArrangementLineKey") IsNot Nothing Then
                    nRIArrLineKey = Convert.ToInt32(Request.QueryString("RIArrangementLineKey").Trim)
                End If
                UpdateDeletedNode(Session(CNRIXMLData), Session(CNRIBandKey), nRIArrLineKey.ToString.Trim, Session(CNRITransactionType))
            End If
            Response.Redirect("~/Secure/ReInsuranceDetails.aspx")
        End If
    End Sub
    ''' <summary>
    ''' This Function is used to update the Delete flag from true to false to hold the previous added FACXOL node for the band.
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sTransType"></param>
    ''' <remarks></remarks>
    Private Sub UpdateDeletedNode(ByRef sXML As String,
                                    ByVal sRIBANDID As String,
                                   Optional ByVal sRIArrangementLineKey As String = "",
                                    Optional ByVal sTransType As String = "NB")
        'Initialize the xml  doc element
        Dim oXMLDoc As New XmlDocument
        'Load the passed xml
        oXMLDoc.LoadXml(sXML)
        Dim oDocElement As XmlElement = oXMLDoc.DocumentElement
        Dim sElement As String = "RIBAND"
        Dim RIBAND As XmlNode = oDocElement.SelectSingleNode("//*[@Name='Current_" & sRIBANDID & "']")
        'get the node as per the RI arrangement Line key
        Dim oFNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        If oFNode IsNot Nothing AndAlso oFNode.Attributes("IsDeleted") IsNot Nothing Then
            'if it is not new line and alos not returned by SAm then delete it from XMl 
            oFNode.Attributes("IsDeleted").Value = "False"
        End If
        ''Get the xml
        sXML = oDocElement.OuterXml
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ' in case of FAC XOL only OK button would be required
        Dim sType As String = Request.QueryString("Type")
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)

        If sType.ToUpper.Trim = "FACXOL" Then
            'if there are not a single child in the child grid
            If grdPlacements.Rows.Count < 1 Then
                Dim sMessage As String
                sMessage = GetLocalResourceObject("NoPlacement_Err")
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                Exit Sub

            Else
                Dim sVMode As String = String.Empty
                Dim cMode As String = String.Empty
                Dim oReinsurance2007 As New Reinsurance
                If Request.QueryString("Mode") IsNot Nothing Then
                    sVMode = Request.QueryString("Mode")
                End If
                If sVMode.ToUpper.Trim = "VIEW" Then
                    Session.Remove(CNRIFACXol)
                    If Session(CNMode) = Mode.NewClaim OrElse Session(CNMode) = Mode.EditClaim OrElse Session(CNMode) = Mode.PayClaim _
               OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery OrElse (sVMode = "VIEW" And Session(CNMode) = Mode.ViewClaim) Then
                        Response.Redirect("~/Claims/ClaimReinsurance.aspx")
                    Else
                        Response.Redirect("~/Secure/ReInsuranceDetails.aspx")
                    End If
                End If

                Dim dTotalPart As Double = 0
                Dim iCount As Integer
                'Calculating the total participation percentage
                CalculateTotalPart(dTotalPart)

                'Participation percentage should be 100
                If dTotalPart <> 100 Then
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("RIInVldPartPerc_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                    processtextchange()
                    Exit Sub
                End If

                'Lower Limit Validation
                If Convert.ToDouble(txtLower.Text) < Convert.ToDouble("0.01") Then
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("FACXOLLT_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                    Exit Sub

                End If

                'Upper Limit ot line limit validation
                If Convert.ToDouble(txtUpper.Text) = Convert.ToDouble("0.00") Then
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("FACXOLUT_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                    Exit Sub
                End If

                If Convert.ToDouble(txtUpper.Text) <= Convert.ToDouble(txtLower.Text) Then
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("FACXOLLTGTUT_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                    Exit Sub
                End If
                'if there are no more error then process further
                Dim oArrLine As ArrangementLinesType = Session(CNRIFACXol)
                If sVMode.ToUpper.Trim = "EDIT" Then
                    Dim iRILineId As Integer
                    Dim iRIArrLineKey As Integer = 0
                    If Request.QueryString("RIArrangementLineKey") IsNot Nothing Then
                        iRIArrLineKey = Convert.ToInt32(Request.QueryString("RIArrangementLineKey").Trim)
                    End If
                    If iRIArrLineKey = 0 Then
                        GetRILineId(iRILineId, oArrLine.RIName)
                    Else
                        iRILineId = iRIArrLineKey
                    End If
                    oReinsurance2007.EditRIArrangementLines(Session(CNRIXMLData), oArrLine, Session(CNRIBandKey), iRILineId.ToString.Trim, Session(CNRIArrangementkey), "FX", Session(CNRITransactionType))
                End If
                If oArrLine IsNot Nothing AndAlso oArrLine.FAXParticipants IsNot Nothing Then
                    For iCount = 0 To oArrLine.FAXParticipants.Count - 1
                        If oArrLine.FAXParticipants(iCount).PremiumValue = 0 AndAlso oArrLine.FAXParticipants(iCount).AccountType.ToUpper.Trim <> "RETAINED" Then
                            If Not ((oQuote.IsInBackDatedMode = True) AndAlso oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN") Then
                                Dim sMessage As String
                                sMessage = GetLocalResourceObject("FACXOLPremMan_Err")
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                                Exit Sub
                            End If
                        End If
                        If oArrLine.FAXParticipants(iCount).AccountType.ToUpper.Trim = "RETAINED" Then
                            oArrLine.FAXParticipants(iCount).PremiumValue = 0
                            If oArrLine.FAXParticipants.Count = 1 Then
                                oArrLine.FAXParticipants(iCount).SumInsured = 0
                            End If
                        End If
                    Next
                End If

                Dim oXMLDoc As New XmlDocument
                Dim oXMLNode As XmlNode
                oXMLDoc.LoadXml(Session(CNRIXMLData))
                Dim dFACTotal As Double = 0
                Dim dBandTotal As Double = 0
                Dim oFXMLNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & Session(CNRIBandKey) & "']/ArrangementRow[@Placement='FAC PROP']")

                For Each oXMLNode In oFXMLNodeList
                    If oXMLNode.Attributes("IsDeleted").Value = "False" Then
                        If oXMLNode.Attributes("LineLimit").Value <> "" Then
                            dFACTotal += Convert.ToDouble(oXMLNode.Attributes("LineLimit").Value)
                        End If
                    End If
                Next

                Dim oBXMLNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & Session(CNRIBandKey) & "']/ArrangementRow[@Name='Band Total']")
                If oBXMLNode IsNot Nothing Then
                    If Not (String.IsNullOrEmpty(oBXMLNode.Attributes.GetNamedItem("SumInsured").Value)) Then
                        If oBXMLNode.Attributes("SumInsured").Value <> "" Then
                            dBandTotal = Convert.ToDouble(oBXMLNode.Attributes("SumInsured").Value)
                        End If
                    End If
                End If
                If Convert.ToDouble(txtUpper.Text) > (dBandTotal - dFACTotal) Then
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("FACXOLULLTLL_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                    Exit Sub
                End If

                Dim oXMLNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & Session(CNRIBandKey) & "']/ArrangementRow[@Placement='FAC XOL']")

                Dim dLineLimit As Double = 0
                Dim dLowerLimit As Double = 0
                Dim dtxtLowerLimit As Double = 0
                Dim dtxtLineLimit As Double = 0

                For Each oXMLNode In oXMLNodeList
                    If oXMLNode.Attributes("IsDeleted").Value = "False" Then
                        dLineLimit = Convert.ToDouble(oXMLNode.Attributes("LineLimit").Value)
                        dLowerLimit = Convert.ToDouble(oXMLNode.Attributes("LowerLimit").Value)
                        dtxtLowerLimit = Convert.ToDouble(txtLower.Text)
                        dtxtLineLimit = Convert.ToDouble(txtUpper.Text)
                        If (dtxtLowerLimit < dLineLimit AndAlso dtxtLowerLimit > dLowerLimit) OrElse (dtxtLowerLimit > dLowerLimit AndAlso dtxtLineLimit = dLineLimit) Then
                            Dim sMessage As String
                            sMessage = GetLocalResourceObject("FACXOLLLOVLPFXP_Err")
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                            Exit Sub
                        End If

                        If dtxtLineLimit > dLowerLimit AndAlso dtxtLineLimit < dLineLimit Then
                            Dim sMessage As String
                            sMessage = GetLocalResourceObject("FACXOLULOVLPFXP_Err")
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                            Exit Sub
                        End If

                        If (dtxtLineLimit = dLineLimit) OrElse (dtxtLowerLimit = dLowerLimit) Then
                            Dim sMessage As String
                            sMessage = GetLocalResourceObject("FACXOLLMTOVLPFXP_Err")
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                            Exit Sub
                        End If
                    End If
                Next

                If dTotalPart = 100 Then
                    Dim iNewId As Integer = 0
                    GenerateUniqueRILineId(iNewId)
                    Dim oArrangementLine As ArrangementLinesType = Session(CNRIFACXol)
                    oArrangementLine.LineLimit = Convert.ToDouble(txtUpper.Text)
                    oArrangementLine.LowerLimit = Convert.ToDouble(txtLower.Text)
                    oArrangementLine.AgreementCode = oArrLine.FAXParticipants(0).AgreementCode
                    oArrangementLine.RIName = oArrLine.FAXParticipants(0).PartyCode ' oGridUpFAXParticipationCol.Item(0).PartyName
                    oArrangementLine.ReinsurerCode = oArrLine.FAXParticipants(0).PartyCode
                    oArrangementLine.RIPlacement = "FAC XOL"
                    oArrangementLine.Type = "FX"
                    oArrangementLine.RIarrangementKey = Convert.ToInt32(Session(CNRIArrangementkey))
                    If oArrLine.FAXParticipants(0).AccountType.ToUpper.Trim = "RETAINED" Then
                        If oArrangementLine.FAXParticipants.Count = 1 Then
                            oArrangementLine.SumInsured = 0
                            oArrangementLine.PremiumValue = 0
                        Else
                            oArrangementLine.SumInsured = oArrangementLine.SumInsured - oArrLine.FAXParticipants(0).SumInsured
                        End If
                        oArrangementLine.Retained = 100
                        oArrangementLine.CommissionPerc = 0
                    End If
                    If oArrangementLine.FAXParticipants.Count > 1 Then
                        oArrangementLine.RIName = "Multiple Acts"
                        oArrangementLine.AgreementCode = ""
                    End If
                    GenerateUniqueRILineId(iNewId)
                    If Session(CNMode) = Mode.PortFolioTransferAmendment OrElse Session(CNMode) = Mode.ClonedTransferAmendment Then
                        oReinsurance2007.AddRIArrangementLines(Session(CNRIXMLData), oArrangementLine, Session(CNRIBandKey), iNewId.ToString, Session(CNRIArrangementkey), "FX", Session(CNRITransactionType), bIsPortfolioRIAmendment:=True)
                    Else
                        oReinsurance2007.AddRIArrangementLines(Session(CNRIXMLData), oArrangementLine, Session(CNRIBandKey), iNewId.ToString, Session(CNRIArrangementkey), "FX", Session(CNRITransactionType))
                    End If
                    ''If Any of participation value is 0 then raise a error ''Participation% is mandatory
                    Session.Remove(CNRIFACXol)
                    Response.Redirect("~/Secure/ReInsuranceDetails.aspx?RICode=" & Uri.EscapeDataString(oArrangementLine.RIName.ToUpper.Trim))
                End If
            End If
        End If
    End Sub


    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        btnOk.Enabled = False
        If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
            Cache.Remove(ViewState("ReinsurerpageCacheID"))
        End If
        FindReInsurer(False, True)
    End Sub

#End Region

#Region "Private Methods"

    Protected Sub txtAgreeCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
            Dim iRowCount As Integer = grdPlacements.Rows.Count
            If iRowCount > 0 Then
                Dim oArrangementLineType As ArrangementLinesType = Session(CNRIFACXol)
                For iCount As Integer = 0 To iRowCount - 1
                    oArrangementLineType.FAXParticipants(iCount).AgreementCode = CType(sender, TextBox).Text
                Next
                Session(CNRIFACXol) = oArrangementLineType
            End If
        End If
    End Sub

    Protected Sub txtLower_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLower.TextChanged
        If (String.IsNullOrEmpty(txtLower.Text)) OrElse Not (IsNumeric(txtLower.Text)) Then
            txtLower.Text = "0.00"
            Dim sMessage As String
            sMessage = GetLocalResourceObject("FACXOLInVldAmt_Err")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
            Exit Sub
        End If

        If Not (String.IsNullOrEmpty(txtLower.Text)) AndAlso (IsNumeric(txtLower.Text)) Then
            processtextchange()
        End If
    End Sub

    Protected Sub txtUpper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUpper.TextChanged
        If (String.IsNullOrEmpty(txtUpper.Text)) OrElse Not (IsNumeric(txtUpper.Text)) Then
            txtUpper.Text = "0.00"
            Dim sMessage As String
            sMessage = GetLocalResourceObject("FACXOLInVldAmt_Err")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
            Exit Sub
        End If
        If Not (String.IsNullOrEmpty(txtUpper.Text)) AndAlso (IsNumeric(txtUpper.Text)) Then
            processtextchange()
        End If
    End Sub
    ''' <summary>
    ''' this will get selected ri line id
    ''' </summary>
    ''' <param name="iRIArrangementId"></param>
    ''' <param name="sRIName"></param>
    ''' <remarks></remarks>
    Sub GetRILineId(ByRef iRIArrangementId As Integer, ByVal sRIName As String)
        Dim oXMLDoc As New XmlDocument
        Dim sXML As String = Session(CNRIXMLData)
        Dim sRIBandID As String = "Current_" & Session(CNRIBandKey)
        oXMLDoc.LoadXml(sXML)
        iRIArrangementId = 0
        Dim oFXMLNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Name='" & sRIName & "']")
        If oFXMLNode IsNot Nothing Then
            iRIArrangementId = Convert.ToInt32(oFXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
        End If
    End Sub
    ''' <summary>
    ''' this will processs Participation text box chenge event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtParticipation_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim value As String = CType(sender, TextBox).Text
        processtextchange(value)
    End Sub
    ''' <summary>
    ''' this will handle all the events fired by grid fields value
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Sub processtextchange(Optional ByVal value As String = "0")
        Dim iRowCount As Integer = grdPlacements.Rows.Count
        Dim dTotalPerc, dTotalSI, dTotalPremium, dTotalTax, dTotalComPerc, dTotalCom, dTotalComTax As Double
        Dim dTaxPercentage As Double = 0
        Dim oArrangementLineType As ArrangementLinesType = Session(CNRIFACXol)

        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        If (oQuote IsNot Nothing) Then
            oQuote.IsInBackDatedMode = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing And Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
            Session(CNQuote) = oQuote
        End If

        If iRowCount > 0 Then
            'process through each line
            For iCount As Integer = 0 To iRowCount - 1
                Dim txtpart As TextBox = grdPlacements.Rows(iCount).FindControl("txtParticipation")
                'validate the Participation percentage
                If String.IsNullOrEmpty(txtpart.Text) OrElse (Not IsNumeric(txtpart.Text)) Then
                    txtpart.Text = "0.0000"
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("RIPercVldNo_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")

                    Exit Sub
                Else
                    If Convert.ToDouble(txtpart.Text) > 100 OrElse Convert.ToDouble(txtpart.Text) < 0 Then
                        txtpart.Text = "0.0000"
                        Dim sMessage As String
                        sMessage = GetLocalResourceObject("RIInVldRate_Err")
                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                        Exit Sub

                    End If
                End If

                'get the total participation
                dTotalPerc += Convert.ToDouble(txtpart.Text)
                txtpart.Text = Format(Convert.ToDouble(txtpart.Text), "0.0000")
                Dim lblSI As Label = grdPlacements.Rows(iCount).FindControl("lblSI")
                'set the default value to "0.00" if values are blank
                If txtUpper.Text = String.Empty Then
                    txtUpper.Text = "0.00"
                End If
                If txtLower.Text = String.Empty Then
                    txtLower.Text = "0.00"
                End If
                'get the total si
                lblSI.Text = (Convert.ToDouble(txtUpper.Text) - Convert.ToDouble(txtLower.Text)) * (Convert.ToDouble(txtpart.Text) / 100)
                dTotalSI += Convert.ToDouble(lblSI.Text)
                lblSI.Text = String.Format(oFormatStringCurrency, Convert.ToDouble(lblSI.Text))
                Dim txtPremium As TextBox = grdPlacements.Rows(iCount).FindControl("txtPremium")
                If Convert.ToDouble(txtPremium.Text) < 0 Then
                    txtPremium.Text = "0"
                    Dim sMessage As String
                    sMessage = GetLocalResourceObject("RINegativePremium_Err")
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                       "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                    Exit Sub
                End If
                'process premium value
                If grdPlacements.Rows(iCount).Cells(2).Text.ToUpper.Trim = "RETAINED" Then
                    If Convert.ToDouble(txtPremium.Text.ToUpper.Trim) <> 0 Then
                        txtPremium.Text = "0.00"
                        Dim sMessage As String
                        sMessage = GetLocalResourceObject("RIInVldFACXOLPrem_Err")
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowMsg", "ShowMsg('" & sMessage & "');", True)
                        Exit Sub
                    End If
                End If
                'validate premium amount
                If String.IsNullOrEmpty(txtPremium.Text) = False Then
                    If Convert.ToDouble(txtPremium.Text) > 0 Then
                        dTaxPercentage = GetTaxPercentage(Convert.ToDouble(txtPremium.Text), Convert.ToInt32(grdPlacements.Rows(iCount).Cells(12).Text))
                    End If
                End If

                If (oQuote IsNot Nothing) Then
                    If Not ((oQuote.IsInBackDatedMode = True) AndAlso oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN") Then
                        txtPremium.Enabled = True
                    Else
                        txtPremium.Enabled = False
                        txtPremium.Text = "0"
                    End If
                End If


                dTotalPremium += Convert.ToDouble(txtPremium.Text)
                Dim lblTax As Label = grdPlacements.Rows(iCount).FindControl("lblTax")
                lblTax.Text = (Convert.ToDouble(txtPremium.Text) * dTaxPercentage) / 100
                dTotalTax += Convert.ToDouble(lblTax.Text)
                lblTax.Text = String.Format(oFormatStringCurrency, Convert.ToDouble(lblTax.Text))
                'get the comm perc
                Dim txtComPerc As TextBox = grdPlacements.Rows(iCount).FindControl("txtComPerc")
                'dTotalComPerc += Convert.ToDouble(txtComPerc.Text)
                Dim lblCom As Label = grdPlacements.Rows(iCount).FindControl("lblCom")
                lblCom.Text = String.Format(oFormatStringCurrency, (Convert.ToDouble(txtComPerc.Text) * Convert.ToDouble(txtPremium.Text)) / 100)
                dTotalCom = dTotalCom + Convert.ToDouble(lblCom.Text)
                lblCom.Text = String.Format(oFormatStringCurrency, Convert.ToDouble(lblCom.Text))

                Dim lblComTax As Label = grdPlacements.Rows(iCount).FindControl("lblComTax")
                'get the comm tax
                If lblCom.Text <> "" AndAlso dTaxPercentage <> 0 Then
                    lblComTax.Text = (Convert.ToDouble(lblCom.Text) * dTaxPercentage) / 100
                End If
                dTotalComTax += Convert.ToDouble(lblComTax.Text)
                lblComTax.Text = String.Format(oFormatStringCurrency, Convert.ToDouble(lblComTax.Text))

                If oArrangementLineType IsNot Nothing AndAlso oArrangementLineType.FAXParticipants IsNot Nothing Then
                    'update the collection
                    Double.TryParse(txtpart.Text, oArrangementLineType.FAXParticipants(iCount).ParticipationPercentage)
                    Double.TryParse(lblSI.Text, oArrangementLineType.FAXParticipants(iCount).SumInsured)
                    Double.TryParse(txtPremium.Text, oArrangementLineType.FAXParticipants(iCount).PremiumValue)
                    Double.TryParse(lblTax.Text, oArrangementLineType.FAXParticipants(iCount).PremiumTax)
                    Double.TryParse(txtComPerc.Text, oArrangementLineType.FAXParticipants(iCount).CommissionPercent)
                    Double.TryParse(lblCom.Text, oArrangementLineType.FAXParticipants(iCount).CommissionValue)
                    Double.TryParse(lblComTax.Text, oArrangementLineType.FAXParticipants(iCount).CommissionTax)
                    oArrangementLineType.FAXParticipants(iCount).AgreementCode = CType(grdPlacements.Rows(iCount).FindControl("txtAgreeCode"), TextBox).Text
                End If
            Next
            If dTotalCom = 0 OrElse dTotalPremium = 0 Then
                dTotalComPerc = 0
            Else
                dTotalComPerc = (dTotalCom * 100) / dTotalPremium
            End If

            'calculate the total of all the columns and add it in the footer column
            Dim txtfooterpart As Label = grdPlacements.FooterRow.FindControl("lblParticipationTotal")
            txtfooterpart.Text = String.Format(oFormatStringPercentage, dTotalPerc)

            Dim txtfooterSi As Label = grdPlacements.FooterRow.FindControl("lblSITotal")
            txtfooterSi.Text = String.Format(oFormatStringCurrency, dTotalSI)

            Dim txtfooterpremium As Label = grdPlacements.FooterRow.FindControl("lblPremiumTotal")
            txtfooterpremium.Text = String.Format(oFormatStringCurrency, dTotalPremium)

            Dim lblfooterTax As Label = grdPlacements.FooterRow.FindControl("lblTaxTotal")
            lblfooterTax.Text = String.Format(oFormatStringCurrency, dTotalTax)

            Dim txtfootercomperc As Label = grdPlacements.FooterRow.FindControl("lblComPercTotal")
            txtfootercomperc.Text = String.Format(oFormatStringPercentage, dTotalComPerc)

            Dim lblfootercom As Label = grdPlacements.FooterRow.FindControl("lblComTotal")
            lblfootercom.Text = String.Format(oFormatStringCurrency, dTotalCom)

            Dim txtfootercomTax As Label = grdPlacements.FooterRow.FindControl("lblComTaxTotal")
            txtfootercomTax.Text = String.Format(oFormatStringCurrency, dTotalComTax)

            'update the collection
            If oArrangementLineType IsNot Nothing Then
                oArrangementLineType.SumInsured = dTotalSI
                oArrangementLineType.PremiumValue = dTotalPremium
                oArrangementLineType.CommissionPerc = dTotalComPerc
                oArrangementLineType.CommissionValue = dTotalCom
                oArrangementLineType.Tax = dTotalTax
                oArrangementLineType.CommissionTax = dTotalComTax
            End If
            'session updated with latest collection
            Session(CNRIFACXol) = oArrangementLineType
        End If
    End Sub

    Protected Sub txtPremium_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim value As String = CType(sender, TextBox).Text
        If (String.IsNullOrEmpty(value)) OrElse Not (IsNumeric(value)) Then
            CType(sender, TextBox).Text = "0.00"
            Dim sMessage As String
            sMessage = GetLocalResourceObject("RIInVldCurrValue_Err")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
            ' Exit Sub
        End If
        processtextchange(value)
    End Sub

    Protected Sub txtComPerc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtCommissionPer As TextBox = CType(sender, TextBox)

        'validate the commission percentage
        If String.IsNullOrEmpty(txtCommissionPer.Text) OrElse (Not IsNumeric(txtCommissionPer.Text)) Then
            txtCommissionPer.Text = "0.0000"
            Dim sMessage As String
            sMessage = GetLocalResourceObject("RIInVldCommPerc_Err")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
        Else
            If Convert.ToDouble(txtCommissionPer.Text) > 100 OrElse Convert.ToDouble(txtCommissionPer.Text) < 0 Then
                txtCommissionPer.Text = "0.0000"
                Dim sMessage As String
                sMessage = GetLocalResourceObject("RIInvalidCommPer_Err")
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg",
                                                   "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
            End If
        End If

        processtextchange(Convert.ToDouble(txtCommissionPer.Text))
    End Sub
    ''' <summary>
    ''' this wiil return the max ri line id
    ''' </summary>
    ''' <param name="iNewId"></param>
    ''' <remarks></remarks>
    Sub GenerateUniqueRILineId(ByRef iNewId As Integer)
        Dim oXMLDoc As New XmlDocument
        Dim sXML As String = Session(CNRIXMLData)
        Dim sRIBandID As String = "Current_" & Session(CNRIBandKey)
        oXMLDoc.LoadXml(sXML)
        iNewId = 0
        Dim oFXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='F']")
        For Each oFXMLNode As XmlNode In oFXMLNodes
            If Convert.ToInt32(oFXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oFXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next

        Dim oFXXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='FX']")
        For Each oFXXMLNode As XmlNode In oFXXMLNodes
            If Convert.ToInt32(oFXXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oFXXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oTXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='T']")
        For Each oTXMLNode As XmlNode In oTXMLNodes
            Dim bIsManuallyAdded As Boolean = False
            If oTXMLNode.Attributes("ManuallyAdded") IsNot Nothing Then
                Boolean.TryParse(oTXMLNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
            End If
            Dim sLineKey As String = oTXMLNode.Attributes("RIArrangementLineKey").Value.Trim
            If bIsManuallyAdded AndAlso String.IsNullOrEmpty(sLineKey) Then
                sLineKey = "0"
            End If
            If Convert.ToInt32(sLineKey) > iNewId Then
                iNewId = Convert.ToInt32(sLineKey)
            End If
        Next
        Dim oTXXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='TX']")
        For Each oTXXMLNode As XmlNode In oTXXMLNodes
            Dim bIsManuallyAdded As Boolean = False
            If oTXXMLNode.Attributes("ManuallyAdded") IsNot Nothing Then
                Boolean.TryParse(oTXXMLNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
            End If
            Dim sLineKey As String = oTXXMLNode.Attributes("RIArrangementLineKey").Value.Trim
            If bIsManuallyAdded AndAlso String.IsNullOrEmpty(sLineKey) Then
                sLineKey = "0"
            End If
            If Convert.ToInt32(sLineKey) > iNewId Then
                iNewId = Convert.ToInt32(sLineKey)
            End If
        Next
        Dim oTCXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='TC']")
        For Each oTCXMLNode As XmlNode In oTCXMLNodes
            If Convert.ToInt32(oTCXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oTCXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oRXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='R']")
        For Each oRXMLNode As XmlNode In oRXMLNodes
            If Convert.ToInt32(oRXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oRXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oTFSXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='TFS']")
        For Each oTFSXMLNode As XmlNode In oTFSXMLNodes
            If Convert.ToInt32(oTFSXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oTFSXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oPXXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='PX']")
        For Each oPXXMLNode As XmlNode In oPXXMLNodes
            If Convert.ToInt32(oPXXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId Then
                iNewId = Convert.ToInt32(oPXXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        iNewId += 1

    End Sub
    ''' <summary>
    ''' this will populate the Reinsure type drop down
    ''' </summary>
    ''' <remarks></remarks>
    Sub PopulateDropDown()
        Dim sType As String
        If Request.QueryString("Type") IsNot Nothing Then
            sType = Request.QueryString("Type")
            If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                drpType.Items.Insert(0, New ListItem("Insurer", "Insurer"))
                drpType.DataBind()
                drpType.SelectedIndex = 0
                drpType.Enabled = False
            ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                drpType.Items.Insert(0, New ListItem("<ALL>", "ALL"))
                drpType.Items.Insert(1, New ListItem("Reinsurer", "Reinsurer"))
                drpType.DataBind()
                drpType.SelectedIndex = 1
            End If
        End If
    End Sub
    ''' <summary>
    ''' this will search the reinsurer
    ''' </summary>
    ''' <param name="bSearchButton"></param>
    ''' <param name="bNewSearch"></param>
    ''' <remarks></remarks>
    Sub FindReInsurer(Optional ByVal bSearchButton As Boolean = False, Optional ByVal bNewSearch As Boolean = False)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oReinsurer As New NexusProvider.ReinsurerSearchCriteria
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim sType As String
        If bSearchButton = False AndAlso bNewSearch = False AndAlso ViewState("ReinsurerpageCacheID") IsNot Nothing Then
            oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
            If oReinsurerColl Is Nothing Then

                If Request.QueryString("Type") IsNot Nothing Then
                    sType = Request.QueryString("Type")
                    If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                        oReinsurer.IsFAX = False
                        oReinsurer.IsFAXSpecified = False
                    ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                        oReinsurer.IsFAX = True
                        oReinsurer.IsFAXSpecified = True
                    End If
                End If
                oReinsurer.MaxRowsToFetch = oPortal.MaxSearchResults

                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If
        Else
            If bSearchButton Then
                If String.IsNullOrEmpty(txtReinsurerCode.Text) = False Then
                    oReinsurer.RICode = txtReinsurerCode.Text
                End If
                If String.IsNullOrEmpty(txtName.Text) = False Then
                    oReinsurer.RIName = txtName.Text
                End If
                If String.IsNullOrEmpty(txtFileCode.Text) = False Then
                    oReinsurer.FileCode = txtFileCode.Text
                End If

                If Request.QueryString("Type") IsNot Nothing Then
                    sType = Request.QueryString("Type")
                    If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                        oReinsurer.IsFAX = False
                        oReinsurer.IsFAXSpecified = False
                    ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                        oReinsurer.IsFAX = True
                        oReinsurer.IsFAXSpecified = True
                    End If
                End If
                'to limit the search return from SAM
                oReinsurer.MaxRowsToFetch = oPortal.MaxSearchResults

                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                If ViewState("ReinsurerpageCacheID") Is Nothing Then
                    Dim ReinsurerpageCacheID As Guid
                    ReinsurerpageCacheID = Guid.NewGuid()
                    ViewState.Add("ReinsurerpageCacheID", ReinsurerpageCacheID.ToString)
                End If
                Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If
            If bNewSearch Then
                oReinsurer.MaxRowsToFetch = 0
                txtReinsurerCode.Text = ""
                txtName.Text = ""
                txtFileCode.Text = ""
                oReinsurerColl = Nothing
                ViewState.Remove("ReinsurerpageCacheID")
            End If
        End If

        grdvSearchResults.Visible = True
        grdvSearchResults.DataSource = oReinsurerColl

        If oReinsurerColl IsNot Nothing AndAlso oReinsurerColl.Count > 0 Then
            grdvSearchResults.AllowPaging = True
        Else
            grdvSearchResults.AllowPaging = False
        End If
        grdvSearchResults.DataBind()


        If oReinsurerColl IsNot Nothing Then
            If oReinsurerColl.Count > 0 Then
                btnOk.Enabled = True
            Else
                'Make “OK button disable on click of new search button
                btnOk.Enabled = False
            End If
        Else
            'Make “OK button disable on click of new search button
            btnOk.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' this call the SAM to get the TAX percentage
    ''' </summary>
    ''' <param name="dPremium"></param>
    ''' <param name="iPartyKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetTaxPercentage(ByVal dPremium As Double, ByVal iPartyKey As Integer) As Double
        oWebService = New NexusProvider.ProviderManager().Provider
        Dim iRiskKey As Integer = 0
        Dim iInsuranceFileKey As Integer = 0
        Dim dTaxPercentage As Double = 0

        iRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key
        iInsuranceFileKey = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
        oWebService.CalculateRITax(iRiskKey, Convert.ToInt32(iPartyKey),
                           Convert.ToDouble(dPremium), iInsuranceFileKey, dTaxPercentage)

        Return dTaxPercentage
    End Function
    ''' <summary>
    ''' this will be called to get the RI details from xml 
    ''' </summary>
    ''' <param name="v_iRIArrangementLineKey"></param>
    ''' <param name="v_sXML"></param>
    ''' <param name="sType"></param>
    ''' <param name="v_bIsClaim"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetRIArrangementLineFromXML(ByVal v_iRIArrangementLineKey As Integer, ByVal v_sXML As String,
 Optional ByVal sType As String = "", Optional ByVal v_bIsClaim As Boolean = False) As ArrangementLinesType
        'Variable to Hold the returned data
        Dim oArrangementLineType As New ArrangementLinesType
        'Load the XML Data 
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(v_sXML)

        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']")

        If v_bIsClaim Then
            'For Claim
            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details
                With oArrangementLineType

                    .ActionType = oNode.Attributes("ActionType").Value
                    .AgreementCode = oNode.Attributes("AgreementCode").Value
                    Boolean.TryParse(oNode.Attributes("CedePremiumOnly").Value, .CedePremiumOnly)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Integer.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIArrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, .RecoverToDate)
                    Decimal.TryParse(oNode.Attributes("ThisPayment").Value, .ThisPayment)
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, .ReserveToDate)
                    Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, .PaymentToDate)
                    Decimal.TryParse(oNode.Attributes("ThisReserve").Value, .ThisReserve)
                    Decimal.TryParse(oNode.Attributes("Balance").Value, .Balance)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("Incurred").Value, .Incurred)

                    If .Type = "F" Then
                        'Populate Broker Details
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    End If
                    If .Type = "FX" Then
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart

                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PaymentToDate").Value, .PaymentToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("RecoverToDate").Value, .RecoverToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ReserveToDate").Value, .ReserveToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ThisPayment").Value, .ThisPayment)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ThisReserve").Value, .ThisReserve)
                                    .ActionType = oFAXPartNode.Attributes("ActionType").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow[@PartyCode='" & Server.HtmlEncode(.PartyCode.Replace("'", "&apos;")) & "']/FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants

                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If
        Else

            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details

                With oArrangementLineType

                    .ActionType = oNode.Attributes("ActionType").Value
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    .CedePremiumOnly = oNode.Attributes("CedePremiumOnly").Value
                    Decimal.TryParse(oNode.Attributes("CommissionPerc").Value, .CommissionPerc)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsCommissionModified").Value, .IsCommissionModified)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Integer.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Decimal.TryParse(oNode.Attributes("PremiumPercent").Value, .PremiumPercent)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIarrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("ParticipationPercent").Value, .ParticipationPercent)
                    Decimal.TryParse(oNode.Attributes("Premium").Value, .PremiumValue)
                    Decimal.TryParse(oNode.Attributes("Tax").Value, .Tax)
                    Decimal.TryParse(oNode.Attributes("Commission").Value, .CommissionValue)
                    Decimal.TryParse(oNode.Attributes("CommissionTax").Value, .CommissionTax)

                    'Populate Broker Details
                    If sType.Trim <> "FACXOL" Then
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    Else
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart

                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    If oFAXPartNode.Attributes("ParticipationPercentage") IsNot Nothing Then
                                        Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercentage").Value, .ParticipationPercentage)
                                    Else
                                        Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                    End If
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionPercent").Value, .CommissionPercent)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionTax").Value, .CommissionTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionValue").Value, .CommissionValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumTax").Value, .PremiumTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumValue").Value, .PremiumValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow[@PartyCode='" & .PartyCode & "']/FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants

                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If
        End If
        'Return the Data
        Return oArrangementLineType
    End Function
    ''' <summary>
    ''' this will calulate total participation amount
    ''' </summary>
    ''' <param name="dTotalPart"></param>
    ''' <remarks></remarks>
    Sub CalculateTotalPart(Optional ByRef dTotalPart As Double = 0)
        Dim iTotalRow, iCount As Integer
        Dim dTotalParticipation As Double
        Dim txtParticipant As TextBox
        iTotalRow = grdPlacements.Rows.Count - 1
        For iCount = 0 To iTotalRow
            txtParticipant = grdPlacements.Rows(iCount).FindControl("txtParticipation")
            Double.TryParse(txtParticipant.Text, dTotalParticipation)
            dTotalPart += dTotalParticipation
        Next
    End Sub
    ''' <summary>
    ''' this will check whether broker participation is present or not
    ''' </summary>
    ''' <param name="sRICode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CheckCollectionOfPartcipants(ByVal sRICode As String) As Boolean
        Dim bPresent As Boolean = False
        Dim iRowCount As Integer = grdPlacements.Rows.Count - 1
        For iCount As Integer = 0 To iRowCount
            If sRICode.ToUpper.Trim = grdPlacements.Rows(iCount).Cells(0).Text.ToUpper.Trim Then
                bPresent = True
                Exit For
            End If
        Next
        Return bPresent
    End Function
#End Region

End Class
