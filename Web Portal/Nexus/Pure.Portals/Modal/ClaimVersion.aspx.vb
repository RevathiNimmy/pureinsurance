Imports Nexus.Utils
Imports NexusProvider.SAMForInsurance
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_ClaimVersion
        Inherits CMS.Library.Frontend.clsCMSPage

        Protected Sub grdvClaims_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvClaims.RowCommand
            Select Case e.CommandName
                Case "Select"
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                    Session.Remove(CNClaim)
                    Session.Remove(CNRIXMLData)
                    With CType(Session.Item(CNClaimVersion), NexusProvider.VersionsCollections)(e.CommandArgument)
                        Dim oCLaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                        Dim oOpenClaim As New NexusProvider.ClaimOpen
                        Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                        Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
                        Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                        Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                        Dim sBranchCode As String = oCLaimQuote.BranchCode
                        Try
                            'arch issue 268
                            oClaimDetails = GetClaimDetailsCall(.ClaimKey, sBranchCode, 1)

                            With oClaimDetails
                                oOpenClaim.CatastropheCode = .CatastropheCode
                                oOpenClaim.BaseClaimKey = .BaseClaimKey
                                oOpenClaim.Claim = .Claim
                                oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                                oOpenClaim.ClaimDescription = .ClaimDescription
                                oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                                oOpenClaim.ClaimKey = .ClaimKey
                                oOpenClaim.ClaimNumber = .ClaimNumber
                                oOpenClaim.ClaimPeril = .ClaimPeril
                                oOpenClaim.ClaimStatus = .ClaimStatus
                                oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                                oOpenClaim.ClaimStatusID = .ClaimStatusID
                                oOpenClaim.ClaimVersion = .ClaimVersion
                                oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                                oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                                oOpenClaim.ClientEmail = .ClientEmail
                                oOpenClaim.ClientFaxNo = .ClientFaxNo
                                oOpenClaim.ClientMobileNo = .ClientMobileNo
                                oOpenClaim.ClientName = .ClientName
                                oOpenClaim.ClientShortName = .ClientShortName
                                oOpenClaim.ClientTelNo = .ClientTelNo
                                oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                                oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                                oOpenClaim.Comments = .Comments
                                oOpenClaim.Contact = .Contact
                                oOpenClaim.CurrencyISOCode = .CurrencyCode
                                oOpenClaim.Description = .Description
                                oOpenClaim.ExternalHandler = .ExternalHandler
                                oOpenClaim.HandlerCode = .HandlerCode
                                oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                                oOpenClaim.InfoOnly = .InfoOnly
                                oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                                oOpenClaim.InsuranceRef = .InsuranceRef
                                oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                                oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                                oOpenClaim.IsDeleted = .IsDeleted
                                oOpenClaim.LastModifiedDate = .LastModifiedDate
                                oOpenClaim.LikelyClaim = .LikelyClaim
                                oOpenClaim.Location = .Location
                                oOpenClaim.LossDate = .LossDate
                                oOpenClaim.LossDateFrom = .LossDateFrom
                                oOpenClaim.LossFromDate = .LossToDate
                                oOpenClaim.LossToDate = .LossToDate
                                oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                                oOpenClaim.Payments = .Payments
                                oOpenClaim.PolicyNumber = .PolicyNumber
                                oOpenClaim.PolicyType = .PolicyType
                                oOpenClaim.PrimaryCause = .PrimaryCause
                                oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                                oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                                oOpenClaim.ProductDescription = .ProductDescription
                                oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                                oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                                oOpenClaim.ReportedDate = .ReportedDate
                                oOpenClaim.Reserve = .Reserve
                                oOpenClaim.RiskKey = .RiskKey
                                oOpenClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                                oOpenClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                                oOpenClaim.SecondaryCause = .SecondaryCause
                                oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                                oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                                oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                                oOpenClaim.TotalShare = .TotalShare
                                oOpenClaim.Town = .Town
                                oOpenClaim.TownCode = .TownCode
                                oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                                oOpenClaim.UserDefFldACode = .UserDefFldACode
                                oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                                oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                                oOpenClaim.UserDefFldDCode = .UserDefFldECode
                                oOpenClaim.UserDefFldECode = .UserDefFldECode
                                oOpenClaim.CoinsuranceTreatmentCode = .CoinsuranceTreatmentCode
                                If Session(CNMode) = Mode.ViewClaim Then
                                    oOpenClaim.IsRecovery = .IsRecovery
                                End If
                                oOpenClaim.TPA = .TPA 'WPR08
                                'Added for Insurer
                                oOpenClaim.Insurer = .Insurer
                                Session.Item(CNClaimTimeStamp) = .TimeStamp
                                oOpenClaim.CurrencyISOCode = .CurrencyCode
                                Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                                oOpenClaim.Client = .Client
                                'Session(CNInsurer_Header) = .ClientName
                                Session(CNClaimNumber) = .ClaimNumber
                                Session(CNStatus) = .ClaimStatus

                                'Arch issue 268
                                oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                If oClaimRisk IsNot Nothing Then
                                    Session(CNDataSet) = oClaimRisk.XMLDataSet
                                End If

                            End With
                            Session(CNClaim) = oOpenClaim
                            ' Response.Redirect("~\Claims\Overview.aspx")

                            'set up javascript to postback the parent page
                            'this will render as self.parent.__doPostBack('__Page', 'RiskTypeSelected');
                            Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Refresh") & ";"
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ParentPostBack", PostBackStr, True)
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

                        Finally
                            oClaimDetails = Nothing
                            oWebservice = Nothing
                            oUserDetails = Nothing
                            oClaimRisk = Nothing
                        End Try
                    End With
            End Select
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Session.Item(CNClaimVersion) IsNot Nothing Then
                If Not IsPostBack Then
                    Dim oClaimVersions As NexusProvider.VersionsCollections = CType(Session(CNClaimVersion), NexusProvider.VersionsCollections)

                    grdvClaims.DataSource = oClaimVersions
                    grdvClaims.DataBind()
                End If

            Else
                Response.Redirect("~/Claims/FindClaim.aspx", False)
            End If
        End Sub

        Protected Sub grdvClaims_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvClaims.PageIndexChanging
            Dim oClaimVersions As NexusProvider.VersionsCollections = CType(Session(CNClaimVersion), NexusProvider.VersionsCollections)

            grdvClaims.PageIndex = e.NewPageIndex
            If Session.Item(CNClaimVersion) IsNot Nothing Then
                grdvClaims.DataSource = oClaimVersions
                grdvClaims.DataBind()
            End If
            'Dont bother rebinding as for some reason ddlPageSize_SelectedIndexChanged is called after this !?
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub grdvClaims_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvClaims.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Versions).ClaimKey)

                'WI 32243 - Adjust TotalIncurred based on system option 5241 and hidden option 88
                Dim oVersion As NexusProvider.Versions = CType(e.Row.DataItem, NexusProvider.Versions)
                Dim sTransactionType As String = oVersion.TransactionType.Trim().ToUpper()

                If sTransactionType = "CLAIM SALVAGE" OrElse sTransactionType = "CLAIM RECOVERY" Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oHiddenOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)
                    Dim oSystemOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5263)

                    If oHiddenOption IsNot Nothing AndAlso oHiddenOption.OptionValue = "1" Then
                        If oSystemOption IsNot Nothing AndAlso oSystemOption.OptionValue = "0" Then
                            Dim oClaimVersions As NexusProvider.VersionsCollections = CType(Session(CNClaimVersion), NexusProvider.VersionsCollections)
                            Dim originalIncurred As Decimal = 0

                            For Each ver As NexusProvider.Versions In oClaimVersions
                                If ver.TransactionType.Trim().ToUpper() = "OPEN CLAIM" Then
                                    originalIncurred = ver.TotalIncurred
                                End If
                            Next

                            e.Row.Cells(8).Text = originalIncurred.ToString("N2")
                        End If
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
