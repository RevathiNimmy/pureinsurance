
Imports System.Web.Configuration
Imports System.Xml
Imports Nexus.Utils
Imports Nexus
Imports System.Xml.XPath
Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Reflection


Partial Class Products_PAPRODUCT_SummaryOfCoverCntrl
    Inherits BaseRisk

    Dim oRiskType As New NexusProvider.RiskType
    Dim sAgentStartPage As String = CType(GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage

    Dim bUWHavePendingVersion As Boolean
    Dim bBrokerHavePendingVersion As Boolean

    'This is the status of the renewal Waiting Status in Back office
    Const sAwaiting_Manual_Preview = "Awaiting Manual Review"
    Const sAwaiting_Renewal_Notice = "Awaiting Renewal Notice Print"
    Const sAwaiting_Update = "Awaiting Update"

    Protected Sub btnBuy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuy.Click
        'Defect# 2489
        If Not Page.IsValid Then
            Exit Sub
        End If
        BuyButton(sender, e)
        '[Start]This code has been added against Issue #1736
        If (btnBuy.Text = GetLocalResourceObject("lblIssued")) Then
            Response.Redirect("SummaryOfCover.aspx?SendMail=true")
        Else
            Response.Redirect("SummaryOfCover.aspx")
        End If
        '[End]This code has been added against Issue #1736
    End Sub

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

    End Sub


    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session(CNIsTransactionConfirmationVisited) = Nothing ' Implemented as per Ashish's mail dated 29/12/2011

        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
        
        If UserCanDoTask("UserGroup") Then
            'PremiumConfirmation.Visible = False
        End If
        'added following code to redirect on page after seleting risk from Modal 
        If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
            'get risk type from session
            oRiskType = Session(CNRiskType)
            'redirect to first risk screen for the current risk type
            Dim sProductCode As String = oRiskType.RiskCode
            SetValuesAndRedirect(sProductCode)
        End If
        'Changes as per Ashish's Mail dated 07-02-2012
        Dim oWebService As NexusProvider.ProviderBase
        Dim oSubAgents As NexusProvider.SubAgentCollection
        'create new instance of proxy
        oWebService = New NexusProvider.ProviderManager().Provider
        Try
            oSubAgents = oWebService.GetSubAgents(oQuote.InsuranceFileKey)
            'error handling - in case quote is referred or declined
        Catch ex As NexusProvider.NexusException
            If ex.Errors(0).Code = "1000145" Then
                If oQuote.InsuranceFileTypeCode.Trim.ToUpper = "QUOTE" Then
                    If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' UW Portal
                        '    ' In Case of Underwriter
                        'btnBuy.Attributes.Add("onclick", "javascript:return showMessageQuoteSubAgentUW();")
                        btnBuy.Attributes.Add("onClick", "javascript:alert('" & GetLocalResourceObject("msgQuoteSubAgentUW").ToString() & "');return false;")
                    Else ' In Case of Broker
                        'btnBuy.Attributes.Add("onclick", "javascript:return showMessageQuoteSubAgentBroker();")
                        btnBuy.Attributes.Add("onClick", "javascript:alert('" & GetLocalResourceObject("msgQuoteSubAgentBroker").ToString() & "');return false;")
                    End If
                    'ElseIf oQuote.InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Then
                ElseIf oQuote.InsuranceFileTypeCode.Trim.ToUpper = "POLICY" Or oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTAQREINS" Then ' As Per Ashish's Mail on 30-03-12
                    If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' UW Portal
                        '    ' In Case of Underwriter
                        'btnBuy.Attributes.Add("onclick", "javascript:return showMessagePolicySubAgentUW();")
                        btnBuy.Attributes.Add("onClick", "javascript:alert('" & GetLocalResourceObject("msgPolicySubAgentUW").ToString() & "');return false;")
                    Else ' In Case of Broker
                        'btnBuy.Attributes.Add("onclick", "javascript:return showMessagePolicySubAgentBroker();")
                        btnBuy.Attributes.Add("onClick", "javascript:alert('" & GetLocalResourceObject("msgPolicySubAgentBroker").ToString() & "');return false;")
                    End If
                End If
            End If
        Finally
            oWebService = Nothing  'clear the object
        End Try

        ' lblTotalPremiumValue.Text = New Money(oQuote.NetTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Net + " & New Money(oQuote.TaxTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " IPT = " & New Money(oQuote.GrossTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Total"
        lblTotalPremiumValue.Text = New Money(oQuote.NetTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Net + " & New Money(oQuote.FeeTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Fee + " & New Money(oQuote.TaxTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " IPT = " & New Money(oQuote.GrossTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Total"
        If CheckRefer() = True Or CheckDecline() = True Then
            docMgr.Visible = False
            'btnSaveQuote.Visible = True
            tdSaveButton.Visible = True
            Referral.Visible = True
            lblReferal.Visible = True
            'btnBuy.Visible = False
            tdBuyButton.Visible = False
            Commission.Visible = False
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
            liPremium.Visible = False
        Else

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' UW Portal
                Commission.Visible = False
            Else ' In Case of Broker
                Commission.Visible = True
            End If
            docMgr.Visible = True

            'btnSaveQuote.Visible = False
            tdSaveButton.Visible = False
            'Referral.Visible = False
            lblReferal.Visible = False
            'btnBuy.Visible = True
            tdBuyButton.Visible = True
            'btnDecline.Visible = True
            tdDeclineButton.Visible = True
            liPremium.Visible = True
        End If

        If Not Page.IsPostBack Then


            'If UserCanDoTask("ShowPremConfButton") = True Then
            '   ' PremiumConfirmation.Visible = True
            'Else
            '    PremiumConfirmation.Visible = False
            'End If

            'If UserCanDoTask("ShowPremSummButton") = True Then
            '    PremiumSumary.Visible = True
            'Else
            '    PremiumSumary.Visible = False
            'End If

            'If UserCanDoTask("ShowSubjEndButton") = True Then
            '    btnSubjEnd.Visible = True
            'Else
            '    btnSubjEnd.Visible = False
            'End If

            'If UserCanDoTask("ShowExcessesButton") Then
            '    btnExcesses.Visible = True
            'Else
            '    btnExcesses.Visible = False
            'End If

            If UserCanDoTask("ShowCommUpdateButton") Then

            End If
            If UserCanDoTask("ShowPremConfEditLinks") Then

            End If
            If UserCanDoTask("ShowPremSummEditLinks") Then

            End If
            If UserCanDoTask("ShowCommEditLinks") Then

            End If

            'If UserCanDoTask("ShowExcessesButton") Then
            '    btnExcesses.Visible = True
            'Else
            '    btnExcesses.Visible = False
            'End If





            If Session(CNMTAType) <> Nothing Then
                'btnRequote.Visible = False
                tdRequoteButton.Visible = False

            End If
        End If

        Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
        If TypeOf oParty Is NexusProvider.CorporateParty Then
            With CType(oParty, NexusProvider.CorporateParty)
                lblClientNameValue.Text = .CompanyName
            End With
        ElseIf TypeOf oParty Is NexusProvider.PersonalParty Then
            With CType(oParty, NexusProvider.PersonalParty)
                lblClientNameValue.Text = .Title & " " & .Forename & " " & .Lastname
            End With
        End If

        If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' UW Portal
            lblClientNameValue.Visible = False
            lblClientName.Visible = False
        Else ' Broker Portal
            'btnRequote.Visible = False
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
            ' PremiumConfirmation.Visible = False
            'PremiumSumary.Visible = False
            lblTotalPremium.Visible = False
            lblTotalPremiumValue.Visible = False

            lblClientNameValue.Visible = True
            lblClientName.Visible = True

            'Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            'Dim lblPageheader As Label = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("lbl_Title"), Label)
            'Dim myControl1 As Label = CType(Page.FindControl("lbl_Title"), Label)

            ''VALIDATE RESOURCE FILE EXISTENCE
            'Dim sTemp As String = System.Web.HttpContext.Current.Request.ServerVariables("SCRIPT_NAME").ToString()
            'Dim iPos As Integer = sTemp.LastIndexOf("/")
            'Dim sResourceName As String = sTemp.Substring(iPos + 1)

            'If System.IO.File.Exists(Server.MapPath("App_LocalResources/" + sResourceName + ".resx")) Then
            '    'VALIDATE RESOURCE KEY NAME
            '    If GetLocalResourceObject("lbl_Title") IsNot Nothing Then
            '        Me.Title = GetLocalResourceObject("lbl_Title")
            '    End If
            'End If
        End If

        If (Not (oQuote.QuoteExpiryDate = Nothing) AndAlso (DateDiff(DateInterval.Day, Now, oQuote.QuoteExpiryDate) < 0)) Then
            'btnBuy.Visible = False
            tdBuyButton.Visible = False
            'btnRequote.Visible = False
            tdRequoteButton.Visible = False
        End If
       

        'If (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending And oUserDetails.Key = 0) Then

        'ElseIf (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Pending And oUserDetails.Key <> 0) Then

        'End If
        'DocumentsDisplay()
        RenewalDetails()
        If (Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.CANCELLATION Or Session(CNMTAType) = MTAType.REINSTATEMENT) Then
            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then
                'btnIssue.Visible = True
                tdIssueButton.Visible = True
            Else
                'btnIssue.Visible = False
                tdIssueButton.Visible = False
            End If
        Else
            'btnIssue.Visible = False
            tdIssueButton.Visible = False
        End If

        'If oQuote.InsuranceFileTypeCode IsNot Nothing Then
        '    If (Session(CNMTAType) = MTAType.REINSTATEMENT Or oQuote.InsuranceFileTypeCode.Trim.ToUpper = "MTAQREINS") Then ' As discussed with Ashish on 06-02-2012
        '        ' tdPremiumConfirmation.Visible = False
        '        ' tdSubjEnd.Visible = False
        '        'tdPremiumSummary.Visible = False
        '        ' tdExcesses.Visible = False
        '    End If
        'End If

        If Session(CNMode) = Mode.View Then 'Session(CNMode) = Mode.Review 
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
            'btnBuy.Visible = False
            tdBuyButton.Visible = False
            'btnDetails.Visible = False
            'btnNewQuote.Visible = False
            'btnRequote.Visible = False
            tdRequoteButton.Visible = False
            'btnSaveQuote.Visible = False
            tdSaveButton.Visible = False
        End If

        '[Start]This code has been added against Issue #1736
        If Request.QueryString("SendMail") = "true" Then 'This code (sendmail) has been moved from core to product because after calling the sendmail from core page we were making a postback to self on product page and hence the pop-up was getting closed. Issue #2343
            BuyButtonStatus()
            MyBase.SendMail()
            Exit Sub
        Else
            BuyButtonStatus() 'This has been called explicitly here because in the case of "btnBuy_Click" click, BuyButtonStatus method is being called previouly and hence no need to be called twice if  Request("__EVENTARGUMENT") = "RefreshGrid"
        End If
        '[Start]This code has been added against Issue #1736

        'Code to inform the user if referral(s) have generated
        Dim sReferralReason As String = Convert.ToString(GetDatafromXML("//MOTOR_OUTPUT_REFERRALS", "REASON", oQuote.Risks(0).XMLDataset))
        If (sReferralReason <> "") Then
            lblReferal.Text = GetLocalResourceObject("lblReferal").ToString() & ": <br/>" & sReferralReason
        End If
    End Sub

    Sub RenewalDetails()
        If Session(CNRenewal) Then
            Dim oQuote As NexusProvider.Quote = HttpContext.Current.Session(CNQuote)
            lblRenewalMessage.Visible = True
            lblRenewalMessage.Text = Replace(lblRenewalMessage.Text, "#RenewalDate", oQuote.CoverEndDate.ToString("dd MMMMMMMM yyyy"))
            'if Status is in "Awaiting Renewal Notice" then Premium is Visible to the user
            ' and is ready to be sent to invitation
            Dim oRenewalStatus As NexusProvider.RenewalStatus = Session(CNRenewalStatus)
            If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice Then
                Session(CNRenewalShowPremium) = True
                btnPrint.Visible = True
                'btnBuy.Visible = False
                tdBuyButton.Visible = False
                lblTotalPremiumRenewal.Text = GetLocalResourceObject("lbl_Renewal_Premium").ToString()
                Dim dTatalPremium As Decimal
                If oQuote.Risks.Count > 0 Then
                    dTatalPremium = oQuote.GrossTotal
                End If

                lblTotalPremiumRenewal.Text = Replace(lblTotalPremiumRenewal.Text, "#TotalPremium", New Money(dTatalPremium, Session(CNCurrenyCode)).Formatted)
                lblTotalPremiumRenewal.Visible = True
                lblPremium.Visible = True
                'dont show the Buy Button  and Premium in case of Manual Review
            ElseIf oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                lblTotalPremium.Visible = False
                lblPremium.Visible = False
                lblMessage.Text = GetLocalResourceObject("lbl_RnlSavePolicy").ToString()
                'btnBuy.Visible = False
                tdBuyButton.Visible = False

                Session(CNRenewalShowPremium) = True

                lblTotalPremiumRenewal.Text = GetLocalResourceObject("lbl_Renewal_Premium").ToString()
                Dim dTatalPremium As Decimal
                If oQuote.Risks.Count > 0 Then
                    dTatalPremium = oQuote.GrossTotal
                End If

                lblTotalPremiumRenewal.Text = Replace(lblTotalPremiumRenewal.Text, "#TotalPremium", New Money(dTatalPremium, Session(CNCurrenyCode)).Formatted)
                lblTotalPremiumRenewal.Visible = True
                lblPremium.Visible = True

            ElseIf oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Update Then
                'btnBuy.Visible = True
                tdBuyButton.Visible = True
                lblTotalPremiumRenewal.Text = GetLocalResourceObject("lbl_Renewal_Premium").ToString()
                lblTotalPremiumRenewal.Text = Replace(lblTotalPremiumRenewal.Text, "#TotalPremium", New Money(oQuote.GrossTotal, Session(CNCurrenyCode)).Formatted)
                lblTotalPremiumRenewal.Visible = True
                btnPrint.Visible = False
                'if the Policy is amended(using Edit\Requote) take the confirmation
                'and if user directly click Buy button without editing the policy this message box should NOT display.
                If Session(CNMode) = Mode.Edit Or Session(CNQuoteMode) = QuoteMode.ReQuote Then
                    btnBuy.Attributes.Add("onclick", "javascript:return ConfirmRenewalTermsAcceptence();")
                End If
            Else
                'btnBuy.Visible = False
                tdBuyButton.Visible = False
            End If

        End If
        'End Renewal Premium

        If CBool(CType(ConfigurationManager.GetSection("NexusFrameWork"),  _
                                        Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).AllowLapsePolicy) _
                                        And Session(CNRenewal) Then
            'If AllowLapsePolicy="true" and Policy in Renewal
            btnLapse.Visible = True
            'btnCancel.Visible = False ' Need NOT to show the Cancel button for Policy in Renewal
        End If
    End Sub

    Function CheckRefer() As Boolean
        Dim bReturn As Boolean = False
        Dim oProduct As Config.Product
        Dim oNexusFramework As Config.NexusFrameWork
        If HttpContext.Current.Session.Item(CNQuote) IsNot Nothing Then
            Dim oQuote As NexusProvider.Quote = HttpContext.Current.Session(CNQuote)
            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oProduct = oNexusFramework.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()
                'Check for Refer
                Dim oNode As XmlNode = Doc.SelectSingleNode("//" & HttpContext.Current.Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@REFER_REASON]")
                'Dim oNode As XmlNode = Doc.SelectSingleNode("//" & HttpContext.Current.Session.Item(CNDataModelCode).ToString() & "_OUTPUT_REFERRALS[@REASON]")

                If oNode IsNot Nothing Then
                    bReturn = True
                    If HttpContext.Current.Session(CNRiskType) Is Nothing Then
                        Dim oRiskType As Config.RiskType
                        If oQuote.Risks(HttpContext.Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskTypeCode.Trim)
                        Else
                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskCode.Trim)
                        End If

                        Dim oRisk As New NexusProvider.RiskType
                        oRisk.DataModelCode = oRiskType.DataModelCode
                        oRisk.Name = oRiskType.Name
                        oRisk.Path = oRiskType.Path
                        oRisk.RiskCode = oRiskType.RiskCode
                        HttpContext.Current.Session(CNRiskType) = oRisk
                    End If
                    Exit For
                End If
            Next
        End If
        Return bReturn
    End Function
    Function CheckDecline() As Boolean
        Dim bReturn As Boolean = False
        Dim oProduct As Config.Product
        Dim oNexusFramework As Config.NexusFrameWork
        If HttpContext.Current.Session.Item(CNQuote) IsNot Nothing Then
            Dim oQuote As NexusProvider.Quote = HttpContext.Current.Session(CNQuote)
            oNexusFramework = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            oProduct = oNexusFramework.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            For iCount As Integer = 0 To oQuote.Risks.Count - 1
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument

                Doc.Load(xmlTR)
                xmlTR.Close()
                'Check for Decline
                Dim oNode As XmlNode = Doc.SelectSingleNode("//" & HttpContext.Current.Session.Item(CNDataModelCode).ToString() & "_OUTPUT[@DECLINE_REASON]")
                If oNode IsNot Nothing Then
                    bReturn = True
                    If HttpContext.Current.Session(CNRiskType) Is Nothing Then
                        Dim oRiskType As Config.RiskType
                        If oQuote.Risks(HttpContext.Current.Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskTypeCode.Trim)
                        Else
                            oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(iCount).RiskCode.Trim)
                        End If

                        Dim oRisk As New NexusProvider.RiskType
                        oRisk.DataModelCode = oRiskType.DataModelCode
                        oRisk.Name = oRiskType.Name
                        oRisk.Path = oRiskType.Path
                        oRisk.RiskCode = oRiskType.RiskCode
                        HttpContext.Current.Session(CNRiskType) = oRisk
                    End If
                    Exit For
                End If
            Next
        End If
        Return bReturn
    End Function

    Sub BuyButtonStatus()
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oWebService As NexusProvider.ProviderBase
        oWebService = New NexusProvider.ProviderManager().Provider

        Dim oQuoteVersionSetting As NexusProvider.OptionTypeSetting
        oWebService = New NexusProvider.ProviderManager().Provider
        oQuoteVersionSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5089)

        If (oQuoteVersionSetting.OptionValue = "1") Then

            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

        ''for requote button
        'If oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Pending Or oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending Then
        '    btnRequote.Visible = False
        'End If
        If oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Pending Then
            'lblQuoteStatusValue.Text = "Pending"
            btnBuy.Text = "Issue"
            If CheckRefer() = True Or CheckDecline() = True Then
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Version " & oQuote.QuoteVersion & " (Pending)"
            Else
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Pending Version " & oQuote.QuoteVersion
                tdBuyButton.Visible = True
            End If
            'btnRequote.Visible = False
            tdRequoteButton.Visible = False
            'tdBuyButton.Visible = True
            'Session(CNMode) = Mode.Review
        ElseIf oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending Then
            'lblQuoteStatusValue.Text = "Agent Pending"
            btnBuy.Text = "Finish"
            If CheckRefer() = True Or CheckDecline() = True Then
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Version " & oQuote.QuoteVersion & " (Agent Pending)"
            Else
                If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' UW Portal
                    '    ' In Case of Underwriter
                    lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Agent Pending Version " & oQuote.QuoteVersion
                    'btnBuy.Visible = False
                    tdBuyButton.Visible = False
                Else ' In Case of Broker
                    lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Quote Incomplete Version " & oQuote.QuoteVersion
                    'btnBuy.Visible = True
                    If UserCanDoTask("ShowFinishButton") Then ' Done the Changes as discussed with Ashish
                        tdBuyButton.Visible = True
                    Else
                        tdBuyButton.Visible = False 'Buy Button Hideen for Broker if User Task is not available in Web.config in case of 'Finish' Caption
                    End If
                End If
            End If
            'btnRequote.Visible = False
            tdRequoteButton.Visible = False
        ElseIf oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Issued Then
            'lblQuoteStatusValue.Text = "Issue"
            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' In Case of Underwriter
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Issued Version " & oQuote.QuoteVersion
                'btnRequote.Visible = True
                tdRequoteButton.Visible = True
            Else ' In Case of Broker
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Quote Complete Version " & oQuote.QuoteVersion
                'btnRequote.Visible = False
                tdRequoteButton.Visible = False
            End If
            btnBuy.Text = "Buy Now"
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
                ' PremiumConfirmation.SetViewMode = True
                ' PremiumSumary.SetViewMode = True
            Commission.SetViewMode = True
        ElseIf oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentComplete Then
            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' In Case of Underwriter
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Agent Complete Version " & oQuote.QuoteVersion
            Else ' In Case of Broker
                lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Quote Complete Version " & oQuote.QuoteVersion
            End If
            'lblQuoteStatusValue.Text = "Agent Complete"
            'lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Agent Complete Version " & oQuote.QuoteVersion
            btnBuy.Text = "Buy Now"
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
            'btnRequote.Visible = True
            tdRequoteButton.Visible = True
            'Session(CNMode) = Mode.Review
                ' PremiumConfirmation.SetViewMode = True
                ' PremiumSumary.SetViewMode = True
            Commission.SetViewMode = True
        ElseIf oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Declined Then
            'lblQuoteStatusValue.Text = "Declined"
            'btnBuy.Visible = False
            tdBuyButton.Visible = False
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
            'btnRequote.Visible = False
            tdRequoteButton.Visible = False
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Declined Version " & oQuote.QuoteVersion
            'Session(CNMode) = Mode.Review
                ' PremiumConfirmation.SetViewMode = True
                '  PremiumSumary.SetViewMode = True
            Commission.SetViewMode = True
        ElseIf oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Live Then
            'lblQuoteStatusValue.Text = "Live"
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Live Version " & oQuote.QuoteVersion
            'Session(CNMode) = Mode.Review
        ElseIf oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.None Then
            'lblQuoteStatusValue.Text = "None"
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Version " & oQuote.QuoteVersion
        Else
            'lblQuoteStatusValue.Text = "None"
            btnBuy.Text = "Finish"
            'btnBuy.Visible = True 'done one 13-01-2011 after the issue was raised by testers
            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' In Case of Underwriter
                tdBuyButton.Visible = True
            Else
                If UserCanDoTask("ShowFinishButton") Then ' Done the Changes as discussed with Ashish
                    tdBuyButton.Visible = True
                Else
                    tdBuyButton.Visible = False 'Buy Button Hideen for Broker if User Task is not available in Web.config in case of 'Finish' Caption 
                End If
            End If
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef & " Version " & oQuote.QuoteVersion
        End If

        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.CANCELLATION Or Session(CNMTAType) = MTAType.REINSTATEMENT Then
            btnBuy.Text = "Buy Now"
            'btnBuy.Visible = True 'done one 13-01-2011 after the issue was raised by testers
            tdBuyButton.Visible = True
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef '& " Version " & oQuote.QuoteVersion ' Versioning not required during MTA/MTC/MTR or Renewal
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
        End If

        If Session(CNRenewal) IsNot Nothing Then
            btnBuy.Text = "Buy Now"
            'btnBuy.Visible = True 'done one 13-01-2011 after the issue was raised by testers
            'tdBuyButton.Visible = True
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef '& " Version " & oQuote.QuoteVersion ' Versioning not required during MTA/MTC/MTR or Renewal
            'btnDecline.Visible = False
            tdDeclineButton.Visible = False
            RenewalDetails()
        End If

        'Changes as required for Issue # 1425.
        If oUserDetails.Key = 0 Then
            'To check if underwriter have any AgentPending versions
            bUWHavePendingVersion = MyBase.HasPendingQuote(True)
            If bUWHavePendingVersion Then
                'btnRequote.Attributes.Add("onclick", "javascript:return showMessageUnderWriter();")
                btnRequote.Attributes.Add("onClick", "javascript:alert('" & GetLocalResourceObject("msgRequote").ToString() & "');return false;")
            Else
                btnRequote.Attributes.Remove("onclick")
            End If
        ElseIf oUserDetails.Key <> 0 Then
            'To check if broker have any Pending versions
            bBrokerHavePendingVersion = MyBase.HasPendingQuote()
            If bBrokerHavePendingVersion Then
                'btnRequote.Attributes.Add("onclick", "javascript:return showMessageBroker();")
                btnRequote.Attributes.Add("onClick", "javascript:alert('" & GetLocalResourceObject("msgRequoteBroker").ToString() & "');return false;")
            Else
                btnRequote.Attributes.Remove("onClick")
            End If
            End If

            'lblTotalPremiumValue.Text = New Money(oQuote.NetTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Net + " & New Money(oQuote.TaxTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " IPT = " & New Money(oQuote.GrossTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Total"
            lblTotalPremiumValue.Text = New Money(oQuote.NetTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Net + " & New Money(oQuote.FeeTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Fee + " & New Money(oQuote.TaxTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " IPT = " & New Money(oQuote.GrossTotal, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString & " Total"
        Else
            lblHeading.Text = GetLocalResourceObject("lblHeading") & oQuote.InsuranceFileRef
        End If
    End Sub


    Sub DocumentsDisplay()
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        'To Conditionally Display The Documents in Document Manager
        ' Load the control 
        Dim myUC As UserControl = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("docMgr"), UserControl) 'LoadControl("ucHeading.ascx")

        ' Set the Usercontrol Type 
        Dim ucType As Type = myUC.GetType()

        ' Get access to the property 
        Dim ucPageHeadingProperty As PropertyInfo = ucType.GetProperty("Documents")

        Dim oQuote As NexusProvider.Quote = Session(CNQuote)

        ' Set the property 
        'Dim sIsBroker As Integer
        'Double.TryParse(GetDatafromXML("//GENERAL", "IS_BROKER", oQuote.Risks(0).XMLDataset), sIsBroker)


        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
        If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then ' UW Portal
            ' In Case of Underwriter
            ucPageHeadingProperty.SetValue(myUC, "Quotation", Nothing)
        Else
            'In Case of Broker
            If UserCanDoTask("ShowCommEditLinks") Then
                If Session(CNMTAType) IsNot Nothing Then ' Issue 2425 CR
                    If CheckRefer() = False Then
                        ucPageHeadingProperty.SetValue(myUC, "Quotation", Nothing)
                    End If
                Else
                    If oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentComplete Or oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Issued Then ' After Finish Button Click
                        ucPageHeadingProperty.SetValue(myUC, "Quotation", Nothing)
                    End If
                End If
            Else
                ucPageHeadingProperty.SetValue(myUC, "Quotation", Nothing)
            End If
        End If

        'ucPageHeadingProperty.SetValue(myUC, "Quotation", Nothing)

    End Sub

    Protected Sub btnHomePage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHomePage.Click
        Response.Redirect(sAgentStartPage)
    End Sub


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)

        Dim sURL = AppSettings("WebRoot") & "/Products/Motor/PrivateMotor/StdWordingSubjectivity.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
        'If (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending And oUserDetails.Key = 0 AndAlso Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing) Then
        '    '  btnSubjEnd.Attributes.Add("onClick", "javascript:if(confirm('" & GetLocalResourceObject("msgConfirmSubjEnd").ToString() & "')){ tb_show(null,'" & sURL & "', null);return false;};")
        'Else
        '    ' btnSubjEnd.Attributes.Add("onClick", "{ tb_show(null,'" & sURL & "', null);return false;}")
        'End If

        'Changes done as suggested by Ashish for Issue # 1425  
        Dim sExcessURL = AppSettings("WebRoot") & "/Products/Motor/PrivateMotor/Excesses.aspx?modal=true&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
        'If (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending And oUserDetails.Key = 0 AndAlso Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing) Then
        '    ' btnExcesses.Attributes.Add("onClick", "javascript:if(confirm('" & GetLocalResourceObject("msgConfirmSubjEnd").ToString() & "')){ tb_show(null,'" & sExcessURL & "', null);return false;};")
        'Else
        '    ' btnExcesses.Attributes.Add("onClick", "{ tb_show(null,'" & sExcessURL & "', null);return false;}")
        'End If

        Dim sLapseURL = AppSettings("WebRoot") & "secure/PolicyLapsed.aspx"
        btnLapse.Attributes.Add("OnClick", "javascript:if(confirm('" & GetLocalResourceObject("msg_ConfirmLapsePolicy").ToString() & "')){window.location='" & sLapseURL & "';}return false;")
    End Sub

    Protected Sub btnSaveQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuote.Click
        SaveButton(sender, e)
        ApplyUpdateRisk() ' Changes as per Jasleen Mail dated 28/11/2011
        BuyButtonStatus()
    End Sub

    Sub ApplyUpdateRisk()
        Dim oWebService As NexusProvider.ProviderBase
        'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)

        oWebService = New NexusProvider.ProviderManager().Provider
        Try
            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
        Catch ex As NexusProvider.NexusException
            If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then

            ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then

            End If
        Finally
            oWebService = Nothing
        End Try
        System.Web.HttpContext.Current.Session(CNQuote) = oQuote
    End Sub

    Protected Sub btnNewQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewQuote.Click
        Dim sProductCode As String
        'sProductCode = CType(sender, LinkButton).Parent.ID
        sProductCode = "MOTOR"  ' Comment this line after setting correct Id for the Li controls
        AddNewQuote(sProductCode)
    End Sub

    Private Sub AddNewQuote(ByVal sProductCode As String)
        'find the risk type associated with this product
        Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
        Dim oProductConfiguration As Nexus.Library.Config.Product


        oProductConfiguration = oPortalConfig.Products.Product(sProductCode)
        'Check RiskTypes for selected product and for more than one RiskType open the Modal dialog Box

        If oProductConfiguration.RiskTypes.Count = 1 Or oProductConfiguration.AllowMultiRisks = False Then
            'there's only one risk type so add this risk type to session
            Dim oRisk As Config.RiskType = oProductConfiguration.RiskTypes.RiskType(0)
            oRiskType.DataModelCode = oRisk.DataModelCode
            oRiskType.Name = oRisk.Name
            oRiskType.Path = oRisk.Path
            oRiskType.RiskCode = oRisk.RiskCode
            Session(CNRiskType) = oRiskType

            'now redirect
            SetValuesAndRedirect(sProductCode)
        ElseIf oProductConfiguration.RiskTypes.Count > 1 And oProductConfiguration.AllowMultiRisks = True Then
            'more than one risk type so we need to open the modal dialog
            Dim sUrl As String = AppSettings("WebRoot") & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
        End If
    End Sub

    ''' <summary>
    ''' Set required session variable, Risk details and redirect to first risk screen or maindetail page
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetValuesAndRedirect(ByVal sProductCode As String)

        Dim sFirstPage As String
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim sFolder As String = AppSettings("WebRoot") & oNexusConfig.ProductsFolder & "/" & sProductCode
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim oProductConfig As Nexus.Library.Config.Product = oPortal.Products.Product(sProductCode)
        Dim sMainDetail As String = Nothing
        Dim oRisk As NexusProvider.RiskType = Session(CNRiskType)

        Session.Remove(CNQuote)
        If oProductConfig.QuickQuoteConfig = String.Empty Then
            sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Products/" & sProductCode & "/" & oRiskType.Path & "/fullquote.config", sMainDetail)
        Else
            sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Products/" & sProductCode & "/" & oRiskType.Path & "/quickquote.config", sMainDetail)
        End If
        'newquote is used to reset the quote's value.
        ClearClaims()
        ClearHeader()
        ClearQuote()

        Session(CNIsAnonymous) = True

        If sMainDetail.ToLower = "true" Then
            Response.Redirect(sFolder & "/" & sFirstPage & "?newquote=true&newclient=true")
        Else
            Response.Redirect(sFolder & "/" & oRisk.Path & "/" & sFirstPage & "?newquote=true&newclient=true")
        End If

    End Sub
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim sDocument As String = PrintButton(sender, e)
        If Not String.IsNullOrEmpty(sDocument) Then
            Print_Renewaldocument.Visible = True
            btnPrint.Visible = False
            'btnBuy.Visible = True
            tdBuyButton.Visible = True
            'btnMarkQuoteForCollection.Visible = True
        End If
    End Sub

    'Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Dim sDocument As String = PrintButton(sender, e)
    '    If Not String.IsNullOrEmpty(sDocument) Then
    '        Print_Renewaldocument.Visible = True
    '        btnPrint.Visible = False
    '        'btnBuy.Visible = True
    '        tdBuyButton.Visible = True
    '        'btnMarkQuoteForCollection.Visible = True
    '    End If
    'End Sub

    Protected Sub btnIssue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnIssue.Click
        'Defect# 2489
        'If Not Page.IsValid Then
        '    Exit Sub
        'End If
        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
        Dim sDesc As String = String.Empty
        Dim sTask As String = String.Empty
        Dim sTaskGroup As String = String.Empty
        Dim oQuote As NexusProvider.Quote = System.Web.HttpContext.Current.Session(CNQuote)

        If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 AndAlso oQuote.ContactUserName <> "" Then
            'If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then
            'If (Session(CNQuoteMode) = QuoteMode.FullQuote) Then 'If NB
            '    sTask = "UNDERNB"
            '    sTaskGroup = "UNDER"
            If (Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY) Then
                sTask = "UNDERMTA"
                sTaskGroup = "UNDER"
                sDesc = IIf(GetLocalResourceObject("lblTaskIssue") Is Nothing, "Your Quote with Ref No. XXXXX is with Issue Status", GetLocalResourceObject("lblTaskIssue"))
                sDesc = sDesc.Replace("XXXXX", oQuote.InsuranceFileRef)
                CreateTask(oQuote, sDesc, sTask, sTaskGroup)
            End If
            Exit Sub
        End If
    End Sub
    'Defect# 2489
    Protected Sub vldChkStatus_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles vldChkStatus.ServerValidate

        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
            If oQuote.Risks IsNot Nothing Then
                If oQuote.Risks(iTempVar).IsRisk = True Then
                    args.IsValid = True
                    Exit For
                Else
                    args.IsValid = False
                End If
            End If
        Next

        'Chekc Quote Status
        If args.IsValid = True Then
            Dim bFound As Boolean = False
            If oQuote.Risks IsNot Nothing Then
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If IsDataSetNexusQuoteStatus(iCount) = True AndAlso Session(CNQuoteMode) = QuoteMode.FullQuote _
                            And NexusQuoteStatus(oQuote.Risks(iCount)) = False And oQuote.Risks(iCount).IsRisk = True _
                            And args.IsValid = True And Session(CNRenewal) Is Nothing Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If
            If bFound = True Then
                vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_FullQuote")
                args.IsValid = False
            End If
        End If

        'if Risk is UNQUOTED then Buy Now should throw a message
        If args.IsValid = True Then
            For iTempVar As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks IsNot Nothing Then
                    If oQuote.Risks(iTempVar).IsRisk = True AndAlso oQuote.Risks(iTempVar).StatusCode.Trim.ToUpper <> "QUOTED" Then
                        vldChkStatus.ErrorMessage = GetLocalResourceObject("Err_UnQuoted")
                        args.IsValid = False
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub


    Public Function GetDatafromXML(ByVal Xpath As String, ByVal field As String, ByVal strXMLDataSet As String) As String
        Dim dStrValue As String = ""
        If Not String.IsNullOrEmpty(strXMLDataSet) Then
            Dim srDataset As New System.IO.StringReader(strXMLDataSet)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)
            xmlTR.Close()

            Dim oNode As XmlNode = Doc.SelectSingleNode(Xpath)
            If oNode IsNot Nothing Then
                If oNode.Attributes(field) IsNot Nothing Then
                    dStrValue = oNode.Attributes(field).Value
                End If
            End If
        End If
        Return dStrValue
    End Function
End Class



