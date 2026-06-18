Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Math
Namespace Nexus

    Partial Class secure_RiskDetails : Inherits Frontend.clsCMSPage

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote

            If Session(CNMTAType) IsNot Nothing Then
                RatingDetails2.Visible = True
            ElseIf Session(CNViewType) IsNot Nothing Then
                'During View mode of MTA display ShowOriginal Rating
                If Session(CNViewType) <> ViewType.POLICY Then
                    RatingDetails2.Visible = True
                End If
            End If
            Dim dTotalFeeTax As Decimal = 0
            Dim dTotalRiskTax As Decimal = 0
            Dim dTotalRiskLevelTax As Decimal = 0
            If Not IsPostBack Or Request("__EVENTARGUMENT") = "RefreshFees" Then
                If Session(CNMode) <> Mode.View Then
                    btnCancel.OnClientClick = "return ConfirmRIMsg('" & GetLocalResourceObject("msg_ConfirmRI").ToString() & "');"
                End If
                SetPageProgress(4)

                oWebService = New NexusProvider.ProviderManager().Provider
                Dim sDisplayReinsurance As String
                oQuote = Session(CNQuote)
                If oQuote.Risks.Count > 0 AndAlso oQuote.Risks(Session(CNCurrentRiskKey)) IsNot Nothing AndAlso oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    sDisplayReinsurance = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayReinsurance, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                Else
                    sDisplayReinsurance = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayReinsurance, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                End If
                If sDisplayReinsurance = "0" Then
                    Me.Title = GetLocalResourceObject("PageTitleRating")
                End If
                'Get the Fee Tota, Net Total, Gross Total to be displayed
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNQuote) = oQuote
                Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
                oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)
                For Each oRiskFee As NexusProvider.Fee In oHeaderandRisk.RiskFees
                    dTotalFeeTax = dTotalFeeTax + oRiskFee.TaxAmount
                Next
                oHeaderandRisk = Nothing

                'to get risk taxes
                Dim oQuoteForRiskTax As NexusProvider.Quote
                oQuoteForRiskTax = oWebService.GetHeaderAndRiskTaxByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)

                For Each oRiskTax As NexusProvider.Tax In oQuoteForRiskTax.RiskTaxes
                    dTotalRiskTax = dTotalRiskTax + oRiskTax.TaxAmount
                Next

                dTotalRiskLevelTax = (dTotalRiskTax + dTotalFeeTax).ToString("N2")
                txtNetTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).PremiumDueNet, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtFeeTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtTaxTotal.Text = New Money(dTotalRiskLevelTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtGrossTotal.Text = New Money((oQuote.Risks(Session(CNCurrentRiskKey)).PremiumDueNet + oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium + dTotalRiskLevelTax), New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString



                'Check the User Authority to display of Reinsurance
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Convert.ToString(Session(CNLoginName))
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayReinsurance
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.GetUserAuthorityValue(oUserAuthority)

                If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" Then
                    'Check for RI2007 product hidden option
                    Dim oRI2007 As NexusProvider.OptionTypeSetting = Nothing
                    oRI2007 = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)

                    divButton.Visible = False
                    btnOk.Visible = True
                    oRI2007 = Nothing
                Else
                    btnOk.Visible = False
                    divButton.Visible = True
                End If
                Session(CNStatus) = True
                oWebService = Nothing
                Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
                Dim oProductConfiguration As Nexus.Library.Config.Product
                oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)

                If Not oPortalConfig.ShowRiskSummary Then
                    If Not oProductConfiguration.ShowRiskSummary Then
                        Response.Redirect("~/secure/PremiumDisplay.aspx", False)
                    End If
                End If

            ElseIf Request("__EVENTARGUMENT") = "RefreshRatingGrid" Then
                oWebService = New NexusProvider.ProviderManager().Provider
                oQuote = CType(Session(CNQuote), NexusProvider.Quote)
                'Get the Fee Total, Net Total, Gross Total to be displayed
                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote


                Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
                oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)
                For Each oRiskFee As NexusProvider.Fee In oHeaderandRisk.RiskFees
                    dTotalFeeTax = dTotalFeeTax + oRiskFee.TaxAmount
                Next
                oHeaderandRisk = Nothing
                'to get risk taxes
                Dim oQuoteForRiskTax As NexusProvider.Quote
                oQuoteForRiskTax = oWebService.GetHeaderAndRiskTaxByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)
                For Each oRiskTax As NexusProvider.Tax In oQuoteForRiskTax.RiskTaxes
                    dTotalRiskTax = dTotalRiskTax + oRiskTax.TaxAmount
                Next

                dTotalRiskLevelTax = (dTotalRiskTax + dTotalFeeTax).ToString("N2")
                txtNetTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).Premium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtFeeTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtTaxTotal.Text = New Money(dTotalRiskLevelTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtGrossTotal.Text = New Money((oQuote.Risks(Session(CNCurrentRiskKey)).Premium + oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium + dTotalRiskLevelTax), New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                oWebService = Nothing
            Else
                oWebService = New NexusProvider.ProviderManager().Provider
                oQuote = Session(CNQuote)
                Dim oHeaderandRisk As NexusProvider.HeaderAndRisk
                oHeaderandRisk = oWebService.GetHeaderAndRiskFeesByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)
                For Each oRiskFee As NexusProvider.Fee In oHeaderandRisk.RiskFees
                    dTotalFeeTax = dTotalFeeTax + oRiskFee.TaxAmount
                Next
                oHeaderandRisk = Nothing

                'to get risk taxes
                Dim oQuoteForRiskTax As NexusProvider.Quote
                oQuoteForRiskTax = oWebService.GetHeaderAndRiskTaxByKey(oQuote.InsuranceFileKey, oQuote.Risks(Session(CNCurrentRiskKey)).Key)

                For Each oRiskTax As NexusProvider.Tax In oQuoteForRiskTax.RiskTaxes
                    dTotalRiskTax = dTotalRiskTax + oRiskTax.TaxAmount
                Next

                dTotalRiskLevelTax = (dTotalRiskTax + dTotalFeeTax).ToString("N2")
                txtNetTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).PremiumDueNet, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtFeeTotal.Text = New Money(oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtTaxTotal.Text = New Money(dTotalRiskLevelTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                txtGrossTotal.Text = New Money((oQuote.Risks(Session(CNCurrentRiskKey)).PremiumDueNet + oQuote.Risks(Session(CNCurrentRiskKey)).FeePremium + dTotalRiskLevelTax), New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
            End If
            If oQuote.Risks(Session(CNCurrentRiskKey)).ReturnPremiumMoreThanBilled = True Then
                btnSubmit.Attributes.Add("onclick", "alert('" & GetLocalResourceObject("msgReturnPremiumMoreThanBilled") & "'); return false;")

            End If

            If (Not IsNothing(Session(CNIsTrueMonthlyPolicy)) AndAlso Session(CNIsTrueMonthlyPolicy) = True) Then
                RatingDetails1.TableRowHeaders = "Rating Section Type,Rate Type,Rate ,Sum Insured([!Currency!]),Premium([!Currency!]), Monthly Premium([!Currency!])"
                RatingDetails2.TableRowHeaders = "Rating Section Type,Rate Type,Rate ,Sum Insured([!Currency!]),Premium([!Currency!]), Monthly Premium([!Currency!])"
            End If
            btnBacktoSummary.Visible = (Session(CNMode) = Mode.View)
        End Sub


        Private Sub CalculatePremium()
            Dim oWebService As NexusProvider.ProviderBase
            oWebService = New NexusProvider.ProviderManager().Provider

            Dim oRatingCollection As NexusProvider.RatingCollection
            'Dim oRating As NexusProvider.Rating = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            Dim InsuranceFileKey As String = String.Empty
            Dim RiskKey As String = String.Empty
            Dim bTimeStamp() As Byte

            bTimeStamp = oQuote.TimeStamp
            InsuranceFileKey = oQuote.InsuranceFileKey
            RiskKey = oQuote.Risks(Session(CNCurrentRiskKey)).Key

            oRatingCollection = oWebService.GetRatingDetails(RiskKey, InsuranceFileKey)
            Session(CNRatingSections) = oRatingCollection

            If Not oRatingCollection Is Nothing Then
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oWebService.UpdateRatingSections(v_iriskKeyField:=RiskKey, i_InsuranceFileKey:=InsuranceFileKey, r_bTimeStamp:=bTimeStamp, oRatingCollection:=oRatingCollection)
                    oQuote.TimeStamp = bTimeStamp
                Catch
                Finally
                    oWebService = Nothing
                End Try
            End If
            'oRating = Nothing
            oRatingCollection = Nothing
        End Sub



        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Session.Remove(CNRatingSections)
            Session.Remove(CNRatingSections)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearTab", "localStorage.removeItem('activeMainTab');", True)

            If Session(CNIsInteractiveBackdatedMTA) = True Then
                Response.Redirect("~/secure/BackDatedMTA.aspx")
            ElseIf Session(CNRiskViewStartPoint) = "ClientManager" Then
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
            ElseIf Session(CNMode) = Mode.PortFolioTransferAmendment OrElse Session(CNMode) = Mode.ClonedTransferAmendment Then
                Response.Redirect("~/secure/RIAmendRiskList.aspx")
            Else
                Response.Redirect("~/secure/PremiumDisplay.aspx", False)
            End If
        End Sub


        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            'Update the Status as PENDINGRI
            If Session(CNMode) <> Mode.View Then
                UpdateRiskStatus()
            End If
            Session.Remove(CNRIArrangementkey)
            Session.Remove(CNRIBandKey)
            Session.Remove(CNRIFACProp)
            Session.Remove(CNRIFACXolTemp)
            Session.Remove(CNRIXMLData)
            If Session(CNMode) = Mode.PortFolioTransferAmendment OrElse Session(CNMode) = Mode.ClonedTransferAmendment Then
                Response.Redirect("~/secure/RIAmendRiskList.aspx")
            Else
                Response.Redirect("~/secure/PremiumDisplay.aspx")
            End If
        End Sub

        ''' <summary>
        ''' Use to update risk
        ''' </summary>
        ''' <remarks></remarks>
        Sub UpdateRiskStatus()
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            If oQuote IsNot Nothing Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oWebService.UpdateRiskStatus(v_iInsuranceFileKey:=oQuote.InsuranceFileKey, v_iRiskKey:=oQuote.Risks(Convert.ToInt32(Session(CNCurrentRiskKey))).Key, v_sBranchCode:=CStr(oQuote.BranchCode), v_RiskStatusType:=NexusProvider.RiskStatusType.PENDINGRI)
            End If
        End Sub

        Protected Sub btnok_Click(sender As Object, e As EventArgs) Handles btnOk.Click
            Response.Redirect("~/secure/ReInsuranceDetails.aspx")
            Session(CNStatus) = True
        End Sub

        Private Sub btnBacktoSummary_Click(sender As Object, e As EventArgs) Handles btnBacktoSummary.Click
            If Session(CNParty) IsNot Nothing Then
                Dim oParty As NexusProvider.BaseParty
                Session(CNRiskBackFlag) = "RiskBackFlag"
                Session(CNPolicyBackFlag) = "BackFlag"
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Response.Redirect("~/secure/agent/CorporateClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        Response.Redirect("~/secure/agent/PersonalClientDetails.aspx?PartyKey=" & oParty.Key & "&Code=" & PureUrlEncode(oParty.UserName) & "#tab-policies")
                        'Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        '    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub
    End Class

End Namespace
