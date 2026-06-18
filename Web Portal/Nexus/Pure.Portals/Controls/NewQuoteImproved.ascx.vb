Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Xml.XPath
Imports NexusProvider.SAMForInsurance
Imports System.Xml.XmlReader
Imports System.Xml


Namespace Nexus

    Partial Class Controls_NewQuoteImproved
        Inherits System.Web.UI.UserControl

        Dim oRiskType As New NexusProvider.RiskType
        ''' <summary>
        ''' Fills allowed product in product dropdown
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oPartyCheck As NexusProvider.BaseParty = Session(CNParty)
            If oPartyCheck IsNot Nothing AndAlso Not String.IsNullOrEmpty(oPartyCheck.BlacklistReasonCode) Then
                Dim sBLackListReason As String = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oPartyCheck.BlacklistReasonCode, "BlackList_Reason")
                btnNewQuote.OnClientClick = "return showBlacklistConfirm('" & sBLackListReason.Replace("'", "\'") & "','" & hfBlacklistConfirmed.ClientID & "');"
            End If
            If Not IsPostBack Then
                Dim sAllowedAgent() As String
                Dim oUserDetails As NexusProvider.UserDetails = Session.Item(CNAgentDetails)
                Dim iCounter As Integer = 0
                Dim bMatched As Boolean = False
                Dim UserRoles As String
                Dim oProducts As Config.Products = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                Dim lstSortedProducts As SortedList = New SortedList()

                For Each oProduct As Config.Product In oProducts
                    'Retreive all the roles set for product in web.config
                    UserRoles = oProduct.AllowRole
                    'Roles is  available
                    If UserRoles IsNot Nothing AndAlso UserIsInRoles(UserRoles) = True AndAlso UserCanDoTask("NewBusiness") _
                        AndAlso FrameWorkFunctions.IsProductAssignedToUserBranch(oProduct, CType(Session(CNAgentDetails), NexusProvider.UserDetails).AvailableUserProductsByBranch) Then
                        'if logged user is agent
                        If CType(Session(CNLoginType), LoginType) = LoginType.Agent Then
                            If String.IsNullOrEmpty(oProduct.AllowedAgent.Trim) Then
                                bMatched = True
                            Else
                                sAllowedAgent = oProduct.AllowedAgent.Split(",")
                                For iCounter = 0 To sAllowedAgent.Length - 1
                                    If sAllowedAgent(iCounter).ToUpper() = oUserDetails.PartyName.ToUpper() Then
                                        bMatched = True
                                        Exit For
                                    End If
                                Next
                            End If
                        Else
                            'for Direct Customer
                            bMatched = True
                        End If
                    Else
                        'Roles is not available
                        bMatched = False
                    End If
                    'if bMatch is True means product will be added
                    If bMatched = True Then
                        lstSortedProducts.Add(oProduct.Name, oProduct.ProductCode)
                    End If
                    bMatched = False
                Next

                'Bind sorted product list

                ddlProductlst.DataSource = lstSortedProducts
                ddlProductlst.DataValueField = "Value"
                ddlProductlst.DataTextField = "Key"
                ddlProductlst.DataBind()

                If ddlProductlst.Items.Count = 2 Then
                    'only one product so hide the drop down list
                    lblSelectProduct.Visible = False
                    ddlProductlst.Visible = False
                    ddlProductlst.SelectedValue = lstSortedProducts.GetByIndex(0).ToString()
                ElseIf ddlProductlst.Items.Count = 1 Then
                    lblSelectProduct.Visible = False
                    ddlProductlst.Visible = False
                    btnNewQuote.Visible = False
                Else
                    lblSelectProduct.Visible = True
                    ddlProductlst.Visible = True
                    btnNewQuote.Visible = True
                End If
                Session("ProductCode") = ddlProductlst.SelectedValue
            End If

        End Sub

        ''' <summary>
        ''' Adds a new quote and redirects a user to core MainDetails page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNewQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewQuote.Click
            'newquote is used to reset the quote's value.
            ClearClaims()
            ClearHeader()
            ClearQuote()
            Session.Remove(CNRiskType)
            Session(CNUnAllocatedClaimPayment) = Nothing
            AddQuoteAndRedirect()
        End Sub

        ''' <summary>
        ''' Adds a new quote and redirects a user to core MainDetails page
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AddQuoteAndRedirect()
            If ddlProductlst.Items.Count <= 0 OrElse ddlProductlst.SelectedValue.Trim() = String.Empty Then
                Exit Sub
            End If

            Dim oPartyCheck As NexusProvider.BaseParty = Session(CNParty)
            If oPartyCheck IsNot Nothing AndAlso Not String.IsNullOrEmpty(oPartyCheck.BlacklistReasonCode) AndAlso hfBlacklistConfirmed.Value <> "yes" Then
                Exit Sub
            End If
            hfBlacklistConfirmed.Value = String.Empty

            Dim bIsTrueMonthlyPolicy As Boolean
            ' Dim bIsUnifiedRenewalDayReadOnly As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sUnifiedRenewalDay As String
            'Dim sDefaultCoverToDateToLastDay As String
            Dim iUnifiedRenewalDay As Integer
            Dim oProductConfiguration As Nexus.Library.Config.Product
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            oProductConfiguration = oPortalConfig.Products.Product(ddlProductlst.SelectedValue)

            ''Set Unified Renewal Day form Product Risk option.
            sUnifiedRenewalDay = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.UnifiedRenewalDay, NexusProvider.RiskTypeOptions.Code, oProductConfiguration.ProductCode, oRiskType.RiskCode)
            If sUnifiedRenewalDay Is Nothing Or sUnifiedRenewalDay = "" Or sUnifiedRenewalDay = "0" Then
                iUnifiedRenewalDay = 1
            Else
                iUnifiedRenewalDay = CType(sUnifiedRenewalDay, Integer)
            End If

            bIsTrueMonthlyPolicy = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.Code, oProductConfiguration.ProductCode, oRiskType.RiskCode)
            'sDefaultCoverToDateToLastDay = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.DefaultCoverToDateToLastDay, NexusProvider.RiskTypeOptions.Code, oProductConfiguration.ProductCode, oRiskType.RiskCode)

            Session(CNIsTrueMonthlyPolicy) = bIsTrueMonthlyPolicy

            'If bIsTrueMonthlyPolicy = True Then
            '    'Check Product Risk Option for Unified Renewal Day ReadOnly field
            '    bIsUnifiedRenewalDayReadOnly = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.UnifiedRenewalDayIsReadOnly, NexusProvider.RiskTypeOptions.Code, oProductConfiguration.ProductCode, oRiskType.RiskCode)
            '    Session(CNIsUnifiedRenewalDayReadOnly) = bIsUnifiedRenewalDayReadOnly
            'End If

            Dim oEnableUnderwritingYearLabelling As NexusProvider.OptionTypeSetting = Nothing
            oEnableUnderwritingYearLabelling = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 68)


            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            If oQuote Is Nothing Then
                'No policy so create one.
                Dim sOptionValue As Integer = Nothing

                Dim sOptionTypeSetting As String = Nothing
                Dim iGracePeriod As Integer = 0
                Dim oOptionSettings As NexusProvider.OptionTypeSetting
                Dim dCoverStartDate, dCoverEndDate As Date
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

                dCoverStartDate = GetCoverStartDate()
                dCoverEndDate = GetCoverEndDateForProduct(dCoverStartDate)


                oQuote = New NexusProvider.Quote(dCoverStartDate, dCoverEndDate, "Nexus Web Quote")

                'S4B has no product codes, but S4I does so, so populate the field for S4B even though it won't
                'be used, is a required attribute in the web.config (for S4I purposes) so leave blank for S4B
                oQuote.ProductCode = oProductConfiguration.ProductCode

                sOptionTypeSetting = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsMidnightRenewal, NexusProvider.RiskTypeOptions.Code, oProductConfiguration.ProductCode, oRiskType.RiskCode)
                sOptionValue = Val(sOptionTypeSetting.ToString)
                iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oProductConfiguration.ProductCode, oRiskType.RiskCode)
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)

                With oQuote
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            'if logged in user if Corporate
                            With CType(oParty, NexusProvider.CorporateParty)
                                oQuote.InsuredName = .CompanyName
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            'if logged in user if Personal
                            With CType(oParty, NexusProvider.PersonalParty)
                                oQuote.InsuredName = .Title & " " & .Forename & " " & .Lastname
                            End With
                    End Select

                    oQuote.CoverStartDate = DateTime.Now.ToShortDateString() 'DateTime.Now.ToString("MM/dd/yyyy")
                    If (bIsTrueMonthlyPolicy) Then
                        'When TRUE MONTHLY POLICY ON -  'Adding one Month to cover start date
                        Dim DCoverEnd As Date
                        DCoverEnd = oQuote.CoverStartDate.AddMonths(1).ToShortDateString()
                        If IsDate(CInt(iUnifiedRenewalDay) & "/" & CDate(DCoverEnd).Month & "/" & CDate(DCoverEnd).Year) And iUnifiedRenewalDay <> "0" Then
                            If DCoverEnd < CDate((CInt(iUnifiedRenewalDay) & "/" & CDate(DCoverEnd).Month & "/" & CDate(DCoverEnd).Year)) AndAlso CDate(oQuote.CoverStartDate).Day < CInt(iUnifiedRenewalDay) Then
                                oQuote.CoverEndDate = CDate(CInt(iUnifiedRenewalDay) & "/" & CDate(oQuote.CoverStartDate).Month & "/" & CDate(oQuote.CoverStartDate).Year)
                            ElseIf DCoverEnd < CDate((CInt(iUnifiedRenewalDay) & "/" & CDate(DCoverEnd).Month & "/" & CDate(DCoverEnd).Year)) AndAlso CDate(oQuote.CoverStartDate).Day > CInt(iUnifiedRenewalDay) Then
                                oQuote.CoverEndDate = CDate(CInt(iUnifiedRenewalDay) & "/" & CDate(DCoverEnd).Month & "/" & CDate(DCoverEnd).Year)
                            Else
                                oQuote.CoverEndDate = CDate(CInt(iUnifiedRenewalDay) & "/" & CDate(DCoverEnd).Month & "/" & CDate(DCoverEnd).Year)
                            End If
                        Else
                            oQuote.CoverEndDate = oQuote.CoverStartDate.AddMonths(1).ToShortDateString()
                        End If

                    Else
                        'Normal Cover End Date
                        'If sDefaultCoverToDateToLastDay = "1" Then
                        '    'Cover End Date will be last date of 12th month

                        '    'Add 13 months
                        '    oQuote.CoverEndDate = oQuote.CoverStartDate.AddYears(1)

                        '    'Add -1 day from 1st date of 13th month
                        '    oQuote.CoverEndDate = CDate("01/" + oQuote.CoverEndDate.Month.ToString() + "/" + oQuote.CoverEndDate.Year.ToString()).AddDays(-1)
                        'Else
                        '    oQuote.CoverEndDate = oQuote.CoverStartDate.AddYears(1).ToShortDateString()
                        'End If
                        oQuote.CoverEndDate = oQuote.CoverStartDate.AddYears(1).ToShortDateString()
                    End If

                    oQuote.InceptionDate = oQuote.CoverStartDate.ToShortDateString()
                    oQuote.InceptionTPI = DateTime.Now.ToShortDateString()
                    oQuote.QuoteExpiryDate = Date.Now.AddDays(iGracePeriod).ToShortDateString()
                    oQuote.ProposalDate = oQuote.CoverStartDate.ToShortDateString()

                    'Checkhing the Value of Midnight Renewal Settings
                    If sOptionTypeSetting = 1 Then
                        'Adding 366 days to Renewal Date and cover to date
                        'If sDefaultCoverToDateToLastDay <> "1" Then
                        '    oQuote.CoverEndDate = oQuote.CoverEndDate.AddDays(-1).ToShortDateString()
                        'End If
                        oQuote.CoverEndDate = oQuote.CoverEndDate.AddDays(-1).ToShortDateString()
                        oQuote.RenewalDate = oQuote.CoverEndDate.AddDays(1).ToShortDateString()
                    Else
                        'Adding 365 days to Renewal Date
                        oQuote.RenewalDate = oQuote.CoverEndDate.ToShortDateString()
                    End If

                    If oEnableUnderwritingYearLabelling.OptionValue = "1" Then
                        Dim sUnderwritingYearId = GetUnderwritingYearIDForCoverDate(oQuote)
                        If sUnderwritingYearId IsNot Nothing Then
                            oQuote.UnderwritingYearId = Convert.ToInt32(sUnderwritingYearId)
                        End If
                    End If
                    'Checkhing the Value of UnifiedRenewalDay Settings When True Monthly Policies ON
                    If (bIsTrueMonthlyPolicy) Then
                        If IsNumeric(iUnifiedRenewalDay) Then
                            If iUnifiedRenewalDay = "0" Then
                                oQuote.RenewalDayNo = oQuote.CoverStartDate.Day
                            Else
                                oQuote.RenewalDayNo = iUnifiedRenewalDay
                            End If
                        End If
                    End If

                    'Checkhing the Value of UnifiedRenewalDay Settings When True Monthly Policies ON
                    If (bIsTrueMonthlyPolicy) Then
                        Dim dAnniversaryDate As Date
                        If IsDate(CDate(oQuote.RenewalDayNo & "/" & oQuote.CoverStartDate.Month & "/" & oQuote.CoverStartDate.AddYears(1).Year).ToShortDateString()) Then
                            dAnniversaryDate = CDate(oQuote.RenewalDayNo & "/" & oQuote.CoverStartDate.Month & "/" & oQuote.CoverStartDate.AddYears(1).Year).ToShortDateString()
                        End If
                        oQuote.AnniversaryDate = dAnniversaryDate
                    End If

                    If Not oParty Is Nothing Then
                        .CurrencyCode = oParty.Currency
                    Else
                        .CurrencyCode = oProductConfiguration.Currencies.Split(",")(0)
                    End If
                    Session(CNCurrenyCode) = .CurrencyCode

                    If Session(CNBranchCode) IsNot Nothing Then
                        .BranchCode = Session(CNBranchCode)
                        .SubBranchCode = Session(CNBranchCode)
                    Else
                        .BranchCode = oParty.BranchCode
                        .SubBranchCode = oParty.BranchCode
                    End If

                    If oUserDetails IsNot Nothing Then
                        If oUserDetails.Key > 0 Then
                            .BusinessTypeCode = "AGENCY"
                            Dim oTempParty As NexusProvider.PartyCollection
                            Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria
                            oTempSearchCriteria.AgentType = Nothing
                            oTempSearchCriteria.Name = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyName
                            oTempSearchCriteria.PartyType = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
                            oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)
                            oWebService = New NexusProvider.ProviderManager().Provider
                            oTempParty = oWebService.FindParty(oTempSearchCriteria)

                            If oTempParty IsNot Nothing Then
                                If oTempParty.Count > 0 Then
                                    .AgentCode = oTempParty(0).UserName
                                    .Agent = CType(Session(CNAgentDetails), NexusProvider.UserDetails).Key
                                End If
                            End If
                        Else
                            .BusinessTypeCode = "DIRECT"
                        End If
                    Else
                        .BusinessTypeCode = "DIRECT"
                    End If
                    If (bIsTrueMonthlyPolicy) Then
                        .FrequencyCode = "MONTH"
                    Else
                        .FrequencyCode = "ANNUAL"
                    End If
                End With

                'count the risk minus IsMandatory=true
                Dim iRiskCount As Integer = 0
                For Each oTempRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                    If oTempRisk.IsMandatory = False Then
                        iRiskCount += 1
                    End If
                Next

                With oQuote
                    .PartyKey = oParty.Key
                End With

                Try
                    Session(CNCurrentRiskKey) = 0
                    Session(CNFreshPolicySW) = 1
                    oWebService.AddQuoteV2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                    'need to retreive the latest risk details if only mandatory risk is configured
                    If iRiskCount = 0 Then
                        oWebService.GetHeaderAndRisksByKey(oQuote)
                    End If
                    oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                    If oOptionSettings.OptionValue = "1" Then
                        oWebService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=oQuote.InsuranceFileKey, v_sBranchCode:=oQuote.BranchCode, bExclusiveLock:=True)
                    End If
                    Session.Add(CNQuote, oQuote)
                Finally
                    oWebService = Nothing
                End Try


            End If

            Response.Redirect("~/secure/MainDetails.aspx?newquote=true", False)

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="oQuote"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUnderwritingYearIDForCoverDate(ByVal oQuote As NexusProvider.Quote) As String
            Dim dtStartDate As Date
            Dim dtEndDate As Date
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection
            Dim v_sXML As System.Xml.XmlElement = Nothing
            Dim sUnderwritingYearId As String = Nothing

            oList = oWebService.GetList(NexusProvider.ListType.PMLookup, "UnderWriting_Year", True, True, , , oQuote.BranchCode, v_sXML)

            If Not v_sXML Is Nothing Then
                Dim sXML As String = v_sXML.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.PreserveWhitespace = False
                xmlDoc.LoadXml(sXML)

                Dim oNodeList As XmlNodeList
                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/UnderWriting_Year")
                For Each oNode As XmlNode In oNodeList

                    If oNode.ChildNodes(3) IsNot Nothing AndAlso oNode.ChildNodes(4) IsNot Nothing AndAlso oNode.ChildNodes(6).InnerText <> "1" Then
                        dtStartDate = Convert.ToDateTime(oNode.ChildNodes(3).InnerText).ToShortDateString()
                        dtEndDate = Convert.ToDateTime(oNode.ChildNodes(4).InnerText).ToShortDateString()
                        If oQuote.CoverStartDate >= dtStartDate And oQuote.CoverStartDate <= dtEndDate Then
                            sUnderwritingYearId = oNode.ChildNodes(0).InnerText
                            Exit For
                        End If
                    End If
                Next
            End If
            Return sUnderwritingYearId
        End Function

        ''' <summary>
        ''' Returns the cover end date of the quote/policy according to the Nexus config
        ''' </summary>
        ''' <param name="dCoverStartDate">The cover state date</param>
        ''' <returns>Cover end date</returns>
        ''' <remarks></remarks>
        Public Function GetCoverEndDateForProduct(ByVal dCoverStartDate As Date) As Date
            Dim dCoverEndDate As Date
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

            Dim oProductConfig As Nexus.Library.Config.Product
            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
            oProductConfig = oPortalConfig.Products.Product(ddlProductlst.SelectedValue)


            Select Case oProductConfig.CoverDate.TimeScale
                Case Config.TimeScale.Day
                    dCoverEndDate = dCoverStartDate.AddDays(oProductConfig.CoverDate.Period)
                Case Config.TimeScale.Week
                    dCoverEndDate = dCoverStartDate.AddDays(oProductConfig.CoverDate.Period * 7)
                Case Config.TimeScale.Month
                    If oProductConfig.CoverDate.TrueMonthlyPolicy Then
                        'End the cover, at the end of the month after period e.g 16/04 - 31/05
                        'DH - not sure if this is right, do we end at the end of current month or next if 1 month policy?
                        dCoverEndDate = dCoverStartDate.AddMonths(oProductConfig.CoverDate.Period)
                        dCoverEndDate = dCoverEndDate.AddDays(CInt(Date.DaysInMonth(dCoverEndDate.Year, dCoverEndDate.Month) - dCoverEndDate.Day))
                    Else
                        'One months time e.g 16/04 - 16-05
                        dCoverEndDate = dCoverStartDate.AddMonths(oProductConfig.CoverDate.Period)
                    End If
                Case Config.TimeScale.Year
                    dCoverEndDate = dCoverStartDate.AddYears(oProductConfig.CoverDate.Period)
                Case Else
                    'Default to the StartDate, as this should be overridden by the risk screen
                    'DH - Not sure if a zero length policy will work?
                    dCoverEndDate = dCoverStartDate
            End Select

            Select Case oProductConfig.CoverDate.MidnightRenewal
                Case "true"
                    'If MidnightRenewal is true this means product need 365 days policy
                    dCoverEndDate = dCoverEndDate.AddDays(-1)
                Case "false"
                    'If MidnightRenewal is false this means product need 366 days policy
                    dCoverEndDate = dCoverEndDate
            End Select

            Return dCoverEndDate
        End Function

        Protected Sub ddlProductlst_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProductlst.SelectedIndexChanged
            Session("ProductCode") = ddlProductlst.SelectedValue
        End Sub
    End Class

End Namespace
