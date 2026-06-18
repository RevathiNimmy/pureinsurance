Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Imports Nexus.Library
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Namespace Nexus

    Partial Class Framework_Reserves
        Inherits CMS.Library.Frontend.clsCMSPage

#Region " Page Events "
        ''' <summary>
        ''' This Event is fired on Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'if user is not logged in
            If Session(CNLoginType) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "/Login.aspx", False)
            End If
            'Populate the grid
            PopulateGrid()

            If Not IsPostBack Then
                Dim oClaim As NexusProvider.ClaimOpen = Nothing
                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim

                    Case Mode.EditClaim

                    Case Mode.ViewClaim
                       
                End Select

            End If
            'Updation of Pregress bar status
            ucProgressBar.OverviewStyle = "complete"
            ' ucProgressBar.ReservesStyle = "in-progress"
            ucProgressBar.ReinsuranceStyle = "incomplete"
            ucProgressBar.PerilsStyle = "incomplete"
            ucProgressBar.SummaryStyle = "incomplete"
            ucProgressBar.CompleteStyle = "incomplete"

        End Sub
#End Region

#Region " Controls Events "
        ''' <summary>
        ''' This event gets fired onOn Row DataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvPerils_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPerils.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim dTotal As Decimal = 0
                Dim oReserveList As NexusProvider.ReserveCollection = CType(e.Row.DataItem, NexusProvider.PerilSummary).Reserve

                If oReserveList IsNot Nothing Then

                    Select Case CType(Session.Item(CNMode), Mode)
                        Case Mode.NewClaim
                            For Each oReserve As NexusProvider.Reserve In oReserveList
                                'this is required in case of New Claim as we dont need Revised reserve in Open Claim Mode
                                dTotal += oReserve.InitialReserve
                            Next
                        Case Mode.EditClaim
                            For Each oReserve As NexusProvider.Reserve In oReserveList
                                Dim currentReserve As String = oReserve.InitialReserve + oReserve.RevisedReserve
                                dTotal += currentReserve
                            Next
                    End Select

                End If

                If e.Row.FindControl("lblReserveTotal") IsNot Nothing Then
                    CType(e.Row.FindControl("lblReserveTotal"), Label).Text = New Money(dTotal, (New Currency(CType(Session(CNCurrenyCode), String))).Type).Formatted.ToString()
                End If

                If e.Row.FindControl("lblSumInsured") IsNot Nothing Then
                    CType(e.Row.FindControl("lblSumInsured"), Label).Text = New Money(CType(e.Row.DataItem, NexusProvider.PerilSummary).SumInsured, (New Currency(CType(Session.Item(CNCurrenyCode), String))).Type).Formatted.ToString()
                End If

                If e.Row.Cells(3).Controls(0) IsNot Nothing Then
                    CType(e.Row.Cells(3).FindControl("lnkRecoveries"), HyperLink).NavigateUrl = "~/Claims/Recoveries.aspx?PerilIndex=" & e.Row.DataItemIndex & ""
                End If
            End If

        End Sub
        ''' <summary>
        ''' This event is fired on Submit button click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            If Page.IsValid Then
                Dim sClaimNumber As String = Nothing
                Dim sSubBranchCode As String = Nothing
                Dim bTimeStamp() As Byte = Nothing
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode

                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                        If Not (oOriginalClaim Is Nothing) Then
                            'Reserve Validation Removed since BO does not limit the reserve against the sum assured
                            IsValidReserve.IsValid = True 'CheckReserveAmountAgainstSumInsured(oOriginalClaim)
                        End If
                        If Page.IsValid Then
                            If Session(CNClaimNumber) IsNot Nothing AndAlso Session(CNClaimNumber).ToString.Trim.ToUpper = "TBA" Then
                                'calling of sam method
                                'To skip the posting first time
                                oOriginalClaim.ReserveOnly = True
                                'Arch issue 268
                                'oClaimResponse = oWebservice.OpenClaim(oOriginalClaim, sBranchCode)
                                oClaimResponse = OpenClaimCall(oOriginalClaim, sBranchCode)
                                If oClaimResponse Is Nothing Then
                                    Exit Sub
                                End If

                                If oClaimResponse.Warnings IsNot Nothing AndAlso oClaimResponse.Warnings.Count >= 0 Then
                                    Exit Select
                                End If
                                'sam call for claim risk
                                'arch issue 268
                                oClaimRisk = AddClaimRiskCall(oClaimResponse.BaseClaimKey, oClaimResponse.TimeStamp, sBranchCode)
                                If oClaimRisk Is Nothing Then
                                    Exit Sub
                                End If

                                Dim bClaimBuilder As Boolean = False
                                Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                                If bClaimBuilder Then
                                    'sam call for claim risk
                                    oClaimRisk = oWebservice.AddClaimRisk(oClaimResponse.BaseClaimKey, oClaimResponse.TimeStamp, sBranchCode)
                                End If
                                'Update the session variable
                                GetClaimDetails(oClaimResponse.ClaimKey, oClaimRisk)
                            Else
                                'Need to write the code to update the recovery reserve
                                'arch issue 268
                                'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oOriginalClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
                                oClaimResponse = UpdateClaimReservesOrPaymentsCall(oOriginalClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
                                If oClaimResponse Is Nothing Then
                                    Exit Sub
                                End If
                                'Update the session variable
                                GetClaimDetails(oOriginalClaim.ClaimKey, Nothing)
                            End If

                            Response.Redirect("~/Claims/Perils.aspx", False)
                        End If
                    Case Mode.EditClaim
                            'Transform claims object object (for viewing only) into the claims object need for MaintainClaim
                            Dim oInitialClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                            If Not (oInitialClaim Is Nothing) Then
                                'Reserve Validation Removed since BO does not limit the reserve against the sum assured
                                IsValidReserve.IsValid = True 'CheckReserveAmountAgainstSumInsured(oOriginalClaim)
                            End If
                            If Page.IsValid Then
                                Response.Redirect("~/Claims/ChangeClaim.aspx", False)
                            End If
                    Case Mode.ViewClaim
                            'Should NEVER be in this mode
                End Select
            End If
        End Sub

        ''' <summary>
        ''' This event is fired on Back Button click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Response.Redirect("~/Claims/Overview.aspx", False)
        End Sub
        ''' <summary>
        ''' This event is fired on close of the Thick Box.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnRefreshReserves_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            grdvPerils.DataSource = oClaim.ClaimPeril
            grdvPerils.DataBind()
        End Sub
#End Region

#Region " Private Methods "
        ''' <summary>
        ''' This Function checks the Reserve Amount with that of the SumInsured.
        ''' </summary>
        ''' <param name="oOriginalClaim"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckReserveAmountAgainstSumInsured(ByVal oOriginalClaim As NexusProvider.ClaimOpen) As Boolean
            'Check to be placed for Peril SI must be greater than Reserve SI before "OpenClaim".
            Dim i As Integer = 0
            Dim iErrFlag As Boolean = True
            If oOriginalClaim.ClaimPeril.Count > 0 Then
                For i = 0 To oOriginalClaim.ClaimPeril.Count - 1
                    Dim dPerilSI As Double = 0
                    Dim dReserveSI As Double = 0
                    Dim j As Integer = 0
                    dPerilSI = dPerilSI + oOriginalClaim.ClaimPeril(i).SumInsured
                    If Not oOriginalClaim.ClaimPeril(i).Reserve Is Nothing Then
                        For j = 0 To oOriginalClaim.ClaimPeril(i).Reserve.Count - 1
                            dReserveSI += oOriginalClaim.ClaimPeril(i).Reserve(j).InitialReserve + oOriginalClaim.ClaimPeril(i).Reserve(j).RevisedReserve
                        Next j
                        If dReserveSI > dPerilSI Then
                            iErrFlag = False
                        End If
                    End If
                Next i
            End If
            Return iErrFlag
        End Function
#End Region
        Sub GetClaimDetails(ByVal v_iClaimKey As Integer, ByVal oClaimRisk As NexusProvider.ClaimRisk)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey)
            'updation of latest session values 
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            'If there is no need to update the claim risk details
            If oClaimRisk IsNot Nothing Then
                Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
            End If
            Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber

            With oClaimDetails
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseClaimKey = .BaseClaimKey
                oOriginalClaim.Claim = .Claim
                oOriginalClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOriginalClaim.ClaimDescription = .ClaimDescription
                oOriginalClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOriginalClaim.ClaimKey = .ClaimKey
                oOriginalClaim.ClaimNumber = .ClaimNumber
                oOriginalClaim.ClaimPeril = .ClaimPeril
                oOriginalClaim.ClaimStatus = .ClaimStatus
                oOriginalClaim.ClaimStatusDate = .ClaimStatusDate
                oOriginalClaim.ClaimStatusID = .ClaimStatusID
                oOriginalClaim.ClaimVersion = .ClaimVersion
                oOriginalClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOriginalClaim.ClientClaimNumber = .ClientClaimNumber
                oOriginalClaim.ClientEmail = .ClientEmail
                oOriginalClaim.ClientFaxNo = .ClientFaxNo
                oOriginalClaim.ClientMobileNo = .ClientMobileNo
                oOriginalClaim.ClientName = .ClientName
                oOriginalClaim.ClientShortName = .ClientShortName
                oOriginalClaim.ClientTelNo = .ClientTelNo
                oOriginalClaim.ClientTelNoOff = .ClientTelNoOff
                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOriginalClaim.Comments = .Comments
                oOriginalClaim.Contact = .Contact
                oOriginalClaim.CurrencyISOCode = .CurrencyISOCode
                oOriginalClaim.Description = .Description
                oOriginalClaim.ExternalHandler = .ExternalHandler
                oOriginalClaim.HandlerCode = .HandlerCode
                oOriginalClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOriginalClaim.InfoOnly = .InfoOnly
                oOriginalClaim.InsuranceFileKey = .InsuranceFileKey
                oOriginalClaim.InsuranceRef = .InsuranceRef
                oOriginalClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOriginalClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOriginalClaim.IsDeleted = .IsDeleted
                oOriginalClaim.LastModifiedDate = .LastModifiedDate
                oOriginalClaim.LikelyClaim = .LikelyClaim
                oOriginalClaim.Location = .Location
                oOriginalClaim.LossDate = .LossDate
                oOriginalClaim.LossDateFrom = .LossDateFrom
                oOriginalClaim.LossFromDate = .LossToDate
                oOriginalClaim.LossToDate = .LossToDate
                oOriginalClaim.LossToDateSpecified = .LossToDateSpecified
                oOriginalClaim.Payments = .Payments
                oOriginalClaim.PolicyNumber = .PolicyNumber
                oOriginalClaim.PolicyType = .PolicyType
                oOriginalClaim.PrimaryCause = .PrimaryCause
                oOriginalClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOriginalClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOriginalClaim.ProductDescription = .ProductDescription
                oOriginalClaim.ProgressStatusCode = .ProgressStatusCode
                oOriginalClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOriginalClaim.ReportedDate = .ReportedDate
                oOriginalClaim.RiskKey = .RiskKey
                oOriginalClaim.SecondaryCause = .SecondaryCause
                oOriginalClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOriginalClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOriginalClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOriginalClaim.TotalShare = .TotalShare
                oOriginalClaim.Town = .Town
                oOriginalClaim.TownCode = .TownCode
                oOriginalClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOriginalClaim.UserDefFldACode = .UserDefFldACode
                oOriginalClaim.UserDefFldBCode = .UserDefFldBCode
                oOriginalClaim.UserDefFldCCode = .UserDefFldCCode
                oOriginalClaim.UserDefFldDCode = .UserDefFldECode
                oOriginalClaim.UserDefFldECode = .UserDefFldECode
            End With
            Session.Item(CNClaim) = oOriginalClaim
        End Sub

        Function ValidateData(ByVal v_sScreenCode As String, ByVal oClaimRisk As NexusProvider.ClaimRisk) As String
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sbOutput As New StringBuilder
            Dim strValidationMsg As String = String.Empty
            'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
            'Read DataModelCode from DataSet if it's not already in session
            Dim sDataModelCode As String = String.Empty
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

            If Session.Item(CNDataModelCode) Is Nothing Then

                Dim XDoc As XPathDocument = New XPathDocument(New IO.StringReader(Session(CNDataSet)))
                Dim Navigator As XPathNavigator
                Navigator = XDoc.CreateNavigator()

                Dim i As XPathNodeIterator = Navigator.Select("DATA_SET")

                While (i.MoveNext)
                    sDataModelCode = i.Current.GetAttribute("DataModelCode", String.Empty)
                End While

                Session.Item(CNDataModelCode) = sDataModelCode
            Else

                sDataModelCode = Session.Item(CNDataModelCode)
            End If
            'To check the reserve validation
            oWebservice.RunValidationRules(oPortal.Claims.CorePerilScreenCode, oClaimRisk.XMLDataSet, oOriginalClaim.ClaimKey)

            Dim srDataset As New System.IO.StringReader(oClaimRisk.XMLDataSet)
            Dim xmlTRNew As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument

            Doc.Load(xmlTRNew)
            xmlTRNew.Close()


            Dim oNodes As XmlNodeList = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON]")

            Dim oNode As XmlNode

            For Each oNode In oNodes
                If oNode.Attributes("REFER_REASON").Value.Trim() <> "" Then
                    sbOutput.Append(oNode.Attributes("REFER_REASON").Value)
                    If oNode.NextSibling IsNot Nothing Then
                        sbOutput.Append("</li><li>")
                    End If
                End If
            Next

            oNodes = Doc.SelectNodes("//" & Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@DECLINE_REASON]")
            For Each oNode In oNodes
                If oNode.Attributes("DECLINE_REASON").Value.Trim() <> "" Then
                    sbOutput.Append(oNode.Attributes("DECLINE_REASON").Value)
                    If oNode.NextSibling IsNot Nothing Then
                        sbOutput.Append("</li><li>")
                    End If
                End If
            Next

            strValidationMsg = sbOutput.ToString()

            srDataset.Dispose()
            Return strValidationMsg

        End Function
        Sub PopulateGrid()
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            'Retreiving the claim details from session
            oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If oClaim.ClaimPeril.Count = 0 Then
                Dim oPerilTypes As NexusProvider.ClaimRiskLinkCollection = GetReserves(oClaim.RiskKey)
                If oPerilTypes IsNot Nothing Then
                    Dim oPerilSummary As NexusProvider.PerilSummary = Nothing
                    'Need to populate oClaim with perils for policy
                    For Each oPeril As NexusProvider.ClaimRiskLink In oPerilTypes
                        oPerilSummary = New NexusProvider.PerilSummary()
                        Dim oReserve As NexusProvider.Reserve = Nothing
                        With oPeril
                            oPerilSummary.Description = .Description
                            oPerilSummary.RecoveryType = .RecoveryItemType
                            oPerilSummary.ReserveType = .ReserveItemType
                            oPerilSummary.RiskKey = .RiskKey
                            oPerilSummary.SumInsured = .SumInsured
                            oPerilSummary.TypeCode = .Code
                            For Each oBaseReserveItem As NexusProvider.ReserveType In .ReserveItemType
                                With oBaseReserveItem
                                    oReserve = New NexusProvider.Reserve()
                                    oReserve.TypeCode = .Code
                                    oReserve.Description = .Description
                                End With
                                oPerilSummary.Reserve.Add(oReserve)
                            Next
                        End With
                        oClaim.ClaimPeril.Add(oPerilSummary)
                    Next
                End If
            End If
            Session(CNClaim) = oClaim
            grdvPerils.DataSource = oClaim.ClaimPeril
            grdvPerils.DataBind()
        End Sub
    End Class

End Namespace
