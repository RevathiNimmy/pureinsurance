Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports System.Web.Configuration
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant

Namespace Nexus
    Partial Class Products_Motor_MainDetails : Inherits BaseRisk
        Protected iMode As Integer
        Private coverNoteBookKey As Integer = 0
        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Shared iGracePeriod As Integer = 0
        Shared oOptionSettings, oFrmDateOptionSettings As NexusProvider.OptionTypeSetting
        Shared sOptionTypeSetting As String
        Protected oUserCollection As New NexusProvider.UserCollection
        Public Overrides Sub PostDataSetWrite()
        End Sub

        Public Overrides Sub PreDataSetWrite()
        End Sub

        Protected Shadows Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Required for Co-Insurance-Lead
            Session(CNCoInsurancePage) = Nothing

            'Used in Javascript for cover date -Start
            If Session(CNRenewal) Then
                iMode = 1
            Else
                iMode = 0
            End If
            'Following line is added to fix issue 2368
            'POLICYHEADER__PROPOSALDATE.Text = Nothing


            If Not IsPostBack Then
                Dim oMaster As ContentPlaceHolder
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProducts As Config.Products = CType(WebConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Products
                Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                Dim oSubAgents As NexusProvider.SubAgentCollection = Nothing
                Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                'Added folloing condition to set value in policy status code on New business Issue No 2362
                If Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.QuickQuote Or Session(CNQuoteMode) = QuoteMode.ReQuote Is Nothing Then
                    POLICYHEADER__POLICYSTATUSCODE.Value = "CUR"
                End If
                lblRenewedTimes.Text = oQuote.RenewalCount
                'Option Setting from BO to Re Calculate the date
                iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)
                oFrmDateOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1008)
                hiddenCoverFromDate.Value = oFrmDateOptionSettings.OptionValue
                sOptionTypeSetting = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsMidnightRenewal, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                Dim sOptionValue As Integer = Val(sOptionTypeSetting.ToString)
                hiddenGracePeriod.Value = iGracePeriod
                hiddenOptionSetting.Value = oOptionSettings.OptionValue
                hiddenMidnightRenewalSettings.Value = sOptionValue

                POLICYHEADER__BUSINESSTYPE.Value = oQuote.BusinessTypeCode
                'Enable/Disable of the Cover Note control
                OnChnageBusinessType()

                'Disable the controls on view mode
                If CType(Session.Item(CNMode), Mode) = Mode.View Or CType(Session.Item(CNMode), Mode) = Mode.Review Then
                    oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                    DisableControls(oMaster)
                End If
            End If
            'Used in Javascript for covewr date -End

            'Refreshing of agent 
            If Request("__EVENTARGUMENT") = "RefreshAgent" Then
                Session(CNAgentType) = hAgentType.Value
                FillCoverNoteBook() 'Population of covernotbook
                chkIsCoverNoteUsed.Enabled = True 'Enable teh cover not book checkbox
                EnableAlternateRef() 'Check the AlternateRef settings
            End If

            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 Then
                'Enable properties for UW 

                hdnIsBroker.Value = 0
                DRIVERDET__IS_BROKER.Value = 0
                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyName IsNot Nothing Then
                    DRIVERDET__AGENTCODE.Value = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyName
                End If

            Else ' For Broker
                hdnIsBroker.Value = 1
                DRIVERDET__IS_BROKER.Value = 1

                DRIVERDET__AGENTCODE.Value = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyName
                rfvWhoCanContact.Enabled = False
                If Not String.IsNullOrEmpty(oUserDetails.ResolvedName) Then
                    POLICYHEADER__CONTACT_NAME.Items.FindByText(oUserDetails.ResolvedName).Selected = True
                    POLICYHEADER__CONTACT_NAME_SelectedIndexChanged(sender, e)
                End If

            End If

            If Request("__EVENTARGUMENT") = "RefreshAgent" Then
                If (POLICYHEADER__AGENT.Value <> "") Then
                    POLICYHEADER__AGENTCODE.Text = hvAgentName.Value
                    FillContactedDropDown(POLICYHEADER__AGENT.Value)
                Else
                    POLICYHEADER__CONTACT_NAME.Items.Clear()
                End If
                POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender, e)
            End If
            'In case of MTA/MTC, “Cover Start Date” will be disabled.
            If Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.CANCELLATION Then
                calCoverFromDate.Enabled = False
            End If

            'In case of MTC and Backdated MTA , "Cover End Date" will be disabled.
            If Session(CNMTAType) = MTAType.CANCELLATION Or CType(Session(CNIsBackDatedMTA), Boolean) = True Then
                calCoverEndDate.Enabled = False
            End If
            'In case of MTA/MTC, “Cover Start Date” will be disabled.
            If Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.CANCELLATION Then
                calCoverFromDate.Enabled = False
            End If

            Dim sSessionCancel As String = Request.QueryString("CancelType")
            'In case of MTC and Backdated MTA , "Cover End Date" will be disabled.
            If Session(CNMTAType) = MTAType.CANCELLATION Or CType(Session(CNIsBackDatedMTA), Boolean) = True Or sSessionCancel = "Cancel" Then
                calCoverEndDate.Enabled = False
            End If
            SetstatusCode()
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LapseConfirmation", _
            '               "<script language=""JavaScript"" type=""text/javascript"">function LapseConfirmation(){return confirm('Are you sure to lapse this quote.');}</script>")
            'btnLapseQuote.Attributes.Add("OnClick", "javascript:return LapseConfirmation();")
        End Sub

        Protected Sub POLICYHEADER__FREQUENCY_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__FREQUENCY.SelectedIndexChanged
            Dim sSelectedVal As String = CType(sender, DropDownList).SelectedValue
            Dim iMonth As Integer
            Dim dtResult As Date
            Select Case sSelectedVal
                Case "ANNUAL"
                    iMonth = 12
                Case "BIANNUAL"
                    iMonth = 6
                Case "MONTH"
                    iMonth = 1
                Case "QUARTER"
                    iMonth = 3
                Case "TWYEAR"
                    iMonth = 24
            End Select

            If Date.TryParse(POLICYHEADER__COVERSTARTDATE.Text, dtResult) Then
                POLICYHEADER__COVERENDDATE.Text = CDate(POLICYHEADER__COVERSTARTDATE.Text).AddMonths(iMonth)
                POLICYHEADER__COVERENDDATE.Text = CDate(POLICYHEADER__COVERENDDATE.Text).AddDays(-1)
                POLICYHEADER__INCEPTION.Text = POLICYHEADER__COVERSTARTDATE.Text
                POLICYHEADER__INCEPTIONTPI.Text = DateTime.Now.ToShortDateString()
                POLICYHEADER__QUOTEEXPIRYDATE.Text = CDate(POLICYHEADER__COVERSTARTDATE.Text).AddDays(iGracePeriod).ToShortDateString()
                POLICYHEADER__PROPOSALDATE.Text = POLICYHEADER__COVERSTARTDATE.Text

                'Checkhing the Value of Midnight Renewal Settings
                If hiddenMidnightRenewalSettings.Value = "1" Then
                    'Adding 366 days to Renewal Date and cover to date
                    POLICYHEADER__COVERENDDATE.Text = CDate(POLICYHEADER__COVERENDDATE.Text).AddDays(-1).ToShortDateString()
                    POLICYHEADER__RENEWAL.Text = CDate(POLICYHEADER__COVERENDDATE.Text).AddDays(1).ToShortDateString()
                Else
                    'Adding 365 days to Renewal Date
                    POLICYHEADER__RENEWAL.Text = CDate(POLICYHEADER__COVERENDDATE.Text).ToShortDateString()
                End If
            End If
        End Sub

        Protected Sub FillContactedDropDown(ByVal iAgentKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing

            Dim oUserCollectionWithCorrectedUserName As NexusProvider.UserCollection

            oAgentSettings = oWebService.GetAgentSettings(iAgentKey)
            If (oAgentSettings IsNot Nothing AndAlso oAgentSettings.AssociatedUsers IsNot Nothing) Then
                oUserCollection = oAgentSettings.AssociatedUsers
            End If

            'If (oUserCollection.Count > 0) Then
            oUserCollectionWithCorrectedUserName = New NexusProvider.UserCollection
            Dim oCorrectedUser As NexusProvider.User
            For Each oUser As NexusProvider.User In oUserCollection
                oCorrectedUser = New NexusProvider.User
                oCorrectedUser = oUser
                oCorrectedUser.FullName = IIf(oUser.FullName.ToString = "", oUser.UserName.ToString(), oUser.FullName.ToString)
                oUserCollectionWithCorrectedUserName.Add(oCorrectedUser)
            Next
            oUserCollectionWithCorrectedUserName.SortColumn = "FullName"
            oUserCollectionWithCorrectedUserName.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oUserCollectionWithCorrectedUserName.Sort()
            POLICYHEADER__CONTACT_NAME.DataValueField = "UserKey"
            POLICYHEADER__CONTACT_NAME.DataTextField = "FullName"
            POLICYHEADER__CONTACT_NAME.DataSource = oUserCollectionWithCorrectedUserName
            POLICYHEADER__CONTACT_NAME.DataBind()
            POLICYHEADER__CONTACT_NAME.Enabled = True

            If (oUserCollection.Count > 0) Then
                rfvWhoCanContact.Enabled = True
            Else
                POLICYHEADER__CONTACT_NAME.Items.Clear()
                POLICYHEADER__CONTACT_NAME.Enabled = False

            End If

            'POLICYHEADER__CONTACT_NAME.Text = Session(CNLoginName).ToString ' SB:This Code is not required as it is valid only for Broker Login and for that the code is already written above.
            POLICYHEADER__CONTACT_NAME.Items.Insert(0, New ListItem("Select", ""))
        End Sub

        Protected Sub POLICYHEADER__CONTACT_NAME_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__CONTACT_NAME.SelectedIndexChanged
            If (POLICYHEADER__CONTACT_NAME.SelectedValue <> "") Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                If (oUserCollection Is Nothing Or oUserCollection.Count = 0) Then
                    Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                    If (oQuote.Agent IsNot Nothing AndAlso oQuote.Agent <> "" AndAlso oQuote.Agent = POLICYHEADER__AGENT.Value) Then
                        If (oQuote.Agent <> 0) Then
                            oAgentSettings = oWebService.GetAgentSettings(oQuote.Agent)
                        End If
                    Else
                        oAgentSettings = oWebService.GetAgentSettings(POLICYHEADER__AGENT.Value)
                    End If

                    If (oAgentSettings IsNot Nothing) Then
                        oUserCollection = oAgentSettings.AssociatedUsers
                        If (oUserCollection.FindItemByUserKey(POLICYHEADER__CONTACT_NAME.SelectedValue).EmailAddress IsNot Nothing) Then
                            DRIVERDET__CONTACT_EMAIL.Value = oUserCollection.FindItemByUserKey(POLICYHEADER__CONTACT_NAME.SelectedValue).EmailAddress
                        End If
                    End If
                Else
                    If (oUserCollection.FindItemByUserKey(POLICYHEADER__CONTACT_NAME.SelectedValue).EmailAddress IsNot Nothing) Then
                        DRIVERDET__CONTACT_EMAIL.Value = oUserCollection.FindItemByUserKey(POLICYHEADER__CONTACT_NAME.SelectedValue).EmailAddress
                    End If
                End If
            End If
        End Sub



        Protected Sub SetstatusCode()
            'Added folloing condition to set value in policy status code on New business
            If Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.QuickQuote Or Session(CNQuoteMode) = QuoteMode.ReQuote Is Nothing Then
                POLICYHEADER__POLICYSTATUSCODE.Value = "CUR"
            End If

        End Sub

        Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
            Session("Frequency") = POLICYHEADER__FREQUENCY.SelectedValue
        End Sub
        Protected Sub POLICYHEADER__BUSINESSTYPE_SelectedIndexChange1(ByVal sender As Object, ByVal e As System.EventArgs) Handles POLICYHEADER__BUSINESSTYPE.SelectedIndexChange

            If (POLICYHEADER__COINSURANCEPLACEMENT IsNot Nothing) Then
                POLICYHEADER__COINSURANCEPLACEMENT.SelectedIndex = -1
            End If
            DirectCast(Session(CNQuote), NexusProvider.Quote).BusinessTypeCode = POLICYHEADER__BUSINESSTYPE.Value
            If POLICYHEADER__BUSINESSTYPE.Value = "AGENCY" Then
                rfvWhoCanContact.Enabled = False
                POLICYHEADER__pnlcoinsplacement.Visible = False
            ElseIf (POLICYHEADER__pnlcoinsplacement IsNot Nothing) Then
                If (POLICYHEADER__BUSINESSTYPE.Value = "COIN LEAD" OrElse POLICYHEADER__BUSINESSTYPE.Value = "COIN FOLL") Then
                    POLICYHEADER__pnlcoinsplacement.Visible = True
                    vldCoinsurancePlacementRequired.Enabled = True

                Else
                    POLICYHEADER__pnlcoinsplacement.Visible = False
                    vldCoinsurancePlacementRequired.Enabled = False
                End If

            Else
                POLICYHEADER__CONTACT_NAME.Items.Clear()
                POLICYHEADER__CONTACT_NAME.Enabled = False

            End If
            POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender, e)
        End Sub
        Protected Sub POLICYHEADER__CORRESPONDENCETYPE_SelectedIndexChange(sender As Object, e As EventArgs) Handles POLICYHEADER__CORRESPONDENCETYPE.SelectedIndexChange
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)

            If POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE IsNot Nothing Then
                POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE.Value = 0
            End If

            If POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE IsNot Nothing AndAlso POLICYHEADER__CORRESPONDENCETYPE IsNot Nothing AndAlso POLICYHEADER__CORRESPONDENCETYPE.Value IsNot Nothing Then
                If POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "DIRECT" OrElse POLICYHEADER__BUSINESSTYPE.Value.Trim.ToUpper = "" Then
                    lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Client Correspondence"
                    If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetClientDefaultPreferredCorrespondence()
                    Else
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                        POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                    End If
                Else
                    If Not String.IsNullOrEmpty(POLICYHEADER__AGENT.Value.ToString) Then
                        oAgentSettings = oWebService.GetAgentSettings(POLICYHEADER__AGENT.Value)
                        If oAgentSettings IsNot Nothing Then
                            If oAgentSettings.IsReceiveClientCorrespondence Then
                                lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Agent Correspondence"
                                POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE.Value = 1
                                If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = oAgentSettings.CorrespondenceType.Trim.ToUpper
                                Else
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                                End If
                            Else
                                lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Client Correspondence"
                                If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetClientDefaultPreferredCorrespondence()
                                Else
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                                End If
                            End If
                        End If
                    Else
                        lblPOLICYHEADER_CORRESPONDENCETYPE.Text = "Client Correspondence"
                        If POLICYHEADER__CORRESPONDENCETYPE.Value.Trim.ToUpper = "DEFAULT" Then
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = True
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetClientDefaultPreferredCorrespondence()
                        Else
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Visible = False
                            POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = String.Empty
                        End If
                    End If
                End If
                If Not String.IsNullOrEmpty(POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text) Then
                    POLICYHEADER__DEFAULTCORRESPONDENCECODE.Value = POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text
                    POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text = GetListItemDescriptionfromCode("PMLookUp", "Contact_Type", POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE.Text)
                End If
            End If
        End Sub

        Private Function GetClientDefaultPreferredCorrespondence() As String
            Dim oParty As NexusProvider.BaseParty
            Dim sDefaultPreferredCorrespondence As String = String.Empty
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        With CType(oParty, NexusProvider.CorporateParty)
                            sDefaultPreferredCorrespondence = .ClientSharedData.CorrespondenceCode
                        End With
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                        With CType(oParty, NexusProvider.PersonalParty)
                            sDefaultPreferredCorrespondence = .ClientSharedData.CorrespondenceCode
                        End With
                End Select
            End If
            Return sDefaultPreferredCorrespondence
        End Function

        Protected Function GetListItemDescriptionfromCode(ByVal sListType As String, ByVal sListCode As String, ByVal sItemCode As String) As String
            Dim sItemDescription As String = String.Empty

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As New NexusProvider.LookupListCollection

            ' sam call to retreive the list of items from user defined list
            If sListType = "UserDefined" Then
                oList = oWebService.GetList(NexusProvider.ListType.UserDefined, sListCode, False, False)
            Else
                oList = oWebService.GetList(NexusProvider.ListType.PMLookup, sListCode, False, False)
            End If

            ' Get code for ID
            For iListCount As Integer = 0 To oList.Count - 1
                If oList(iListCount).Code = sItemCode Then
                    sItemDescription = oList(iListCount).Description
                    Exit For
                End If
            Next
            Return sItemDescription
        End Function
    End Class
End Namespace
