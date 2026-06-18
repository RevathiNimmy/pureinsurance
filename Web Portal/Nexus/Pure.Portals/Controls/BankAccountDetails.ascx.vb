Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_BankAccountDetails : Inherits System.Web.UI.UserControl

        Dim InstalmentQuotesCacheID As Guid
        Dim PartyBankdetailsCacheID As Guid
        Dim SelectedAccountTypeCacheId As Guid
        Dim SelectedInstalmentQuoteCacheId As Guid
        Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
        Dim oBankDetails As NexusProvider.BankCollection = Nothing
        Dim oParty As NexusProvider.BaseParty = Nothing
        Dim oBank As NexusProvider.Bank = Nothing
       
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim oBankHistoryDetails As New NexusProvider.FinancePlanBankHistoryCollection
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    'oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    oBankHistoryDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFBankHistory
                    'Load the Bank controls only when MediaTypeCode is of Bank type: Javascript code written only to hide the page, doesn't stops the page from loading controls
                    If oFinancePlanDetails.MediaTypeCode.ToString().ToUpper().Trim() = "DD" Then
                        If oBankHistoryDetails IsNot Nothing AndAlso oBankHistoryDetails.Count > 0 Then
                            FillGrid(oBankHistoryDetails)
                        End If

                        If Session(CNMode) = Mode.View Then
                            If oFinancePlanDetails IsNot Nothing Then
                                PopulateAccountFields()
                                PopulateAccountType()
                                FillFormFields(oFinancePlanDetails)
                            End If
                        Else
                            PopulateAccountFields()
                            PopulateAccountType()
                            FillFormFields(oFinancePlanDetails)
                        End If
                    End If
                End If
                DisableControls(Me)
                'Dont enable account type dropdown if instalment plan type is "view" else enable in everyother case
                If Session(CNInstalmentPlanMode) <> InstalmentPlanType.View AndAlso oFinancePlanDetails.StatusInd <> NexusProvider.FinancePlanStatus.Item999 Then
                    EnableRequiredFields()
                End If
            End If

            If Request("__EVENTARGUMENT") = "UpdateBank" Then
                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sBankData() As String = txtBankDetailData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sBankData(0).ToUpper = "ADD" Then

                    Dim oNewBank As New NexusProvider.Bank
                    PopulatePartyBankDetails(oNewBank, sBankData)
                    oParty.BankDetails.Add(oNewBank)
                    Session(CNParty) = oParty
                ElseIf sBankData(0).ToUpper = "UPDATE" Then

                    Dim oUpdateBankCollection As NexusProvider.BankCollection = oParty.BankDetails
                    Dim nBankData As Integer = 0
                    Int32.TryParse(sBankData(17), nBankData)
                    Dim oUpdateBanks As NexusProvider.Bank = oParty.BankDetails.Item(nBankData)
                    PopulatePartyBankDetails(oUpdateBanks, sBankData)
                    With oUpdateBanks
                        If String.IsNullOrEmpty(.PartyBankKey) = False AndAlso .PartyBankKey > 0 Then
                            .TaskMode = NexusProvider.Bank.Mode.Edit
                        Else
                            .TaskMode = NexusProvider.Bank.Mode.Add
                        End If
                    End With
                    oUpdateBankCollection.Update(oUpdateBanks)
                    Session(CNParty) = oParty

                    'Re-populate instlament plan details with updated values
                    Dim nFinancePlanKey As Integer
                    Dim nFinancePlanVersion As Integer
                    If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                        nFinancePlanKey = Request.QueryString("FinancePlanKey")
                        nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "tb_updatedEditMode('RedirectFinancePlanDetailsEdit','~/PremiumFinance/FinancePlanDetails.aspx?FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion & "&#tab-BankDetails ');", True)
                End If
                Dim objBaseClient As New Nexus.BaseClient
                objBaseClient.UpdatePartyBank()
                'Repopulate the fields for the newly added account
                PopulateAccountFields()
                PopulateAccountType()
            End If

            If Session(CNInstalmentPlanMode) = InstalmentPlanType.View Then
                hypBank.Visible = False
                hypBankEdit.Visible = False
            ElseIf ddlAccountType.SelectedValue <> "" Then
                hypBankEdit.Visible = True

                'Need to Retreive the Data from Session
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                End If

                Dim BankKey As Integer = 0
                For BankKey = 0 To oParty.BankDetails.Count - 1
                    If oParty.BankDetails(BankKey).PartyBankKey = ddlAccountType.SelectedValue Then
                        hypBankEdit.Visible = True
                        If HttpContext.Current.Session.IsCookieless Then
                            If oParty.BankDetails(BankKey).IsPartyBankLinkedWithInst Then
                                hypBankEdit.OnClientClick = "if( confirm('" & GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                            Else
                                hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        Else
                            If oParty.BankDetails(BankKey).IsPartyBankLinkedWithInst Then
                                hypBankEdit.OnClientClick = "if( confirm('" & GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                            Else
                                hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        End If


                        Exit For
                    End If
                Next
            End If
            If Request.QueryString("ProcessType") IsNot Nothing AndAlso Request.QueryString("ProcessType") = "MTA" Then
                hypBank.Visible = True
                If ddlAccountType.SelectedValue <> "" Then
                    hypBankEdit.Visible = True
                End If
            End If

            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function() {SetDropDownInitial();});</script>")

        End Sub

        Private Sub PopulatePartyBankDetails(ByVal oBank As NexusProvider.Bank, ByVal sBankData() As String)
            With oBank
                .BankPaymentTypeCode = sBankData(1)
                .AccountHolderName = sBankData(2)
                .AccountType = sBankData(3)
                .AccountNumber = sBankData(4)
                .AccountCode = sBankData(4)
                .BranchCode = sBankData(5)
                .BankBranch = sBankData(6)

                .BankCode = sBankData(7)
                .BankName = sBankData(8)
                .StreetName = sBankData(9)
                .Locality = sBankData(10)
                .PostTown = sBankData(11)
                .County = sBankData(12)
                .PostCode = sBankData(13)
                .Country = sBankData(14)
                .PartyBankAddress.Address1 = sBankData(9)
                .PartyBankAddress.Address2 = sBankData(10)
                .PartyBankAddress.Address3 = sBankData(11)
                .PartyBankAddress.Address4 = sBankData(12)
                .PartyBankAddress.PostCode = sBankData(13)
                .PartyBankAddress.CountryCode = sBankData(14)
                .TaskMode = NexusProvider.Bank.Mode.Add
                .BIC = sBankData(15)
                .IBAN = sBankData(16)
                '.BankKey = sBankData(14)
            End With
        End Sub
        ''' <summary>
        ''' Retreive the Party data from Session
        ''' </summary>
        ''' <remarks></remarks>
        Sub RetreiveData()
            'Need to Retreive the Data from Session
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                End Select
            End If
        End Sub
        Protected Sub PopulateAccountFields()
            PartyBankdetailsCacheID = Guid.NewGuid()
            SelectedAccountTypeCacheId = Guid.NewGuid()
            ViewState.Add("PartyBankdetailsCacheID", PartyBankdetailsCacheID.ToString)
            ViewState.Add("SelectedAccountTypeCacheId", SelectedAccountTypeCacheId.ToString)

            Dim oPartyBankDetails As NexusProvider.BankCollection
            Dim oPartyBankDetail As NexusProvider.Bank
            Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
            Dim oPartyBankDetailsForInstalment As New NexusProvider.BankCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim paymentOptions As New Config.PaymentTypes
            Dim oPayment As New NexusProvider.Payment(NexusProvider.PaymentTypes.None)
            'Find all the account types for party/agent using sam method "GetPartyBankDetails"
            'Filter all the accounts for instalment payment method and save the details in cache
            'So that party bank details can be used througout the page

            If (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).Trim.ToUpper = "BROKER" AndAlso oQuote.BusinessTypeCode <> "DIRECT") Then
                'If Session(CNAgentType) IsNot Nothing Then
                'If agent type is broker then we need to get agent bank details 
                'else party bank details would be required
                Dim iAgentId As Integer = 0
                Integer.TryParse(oQuote.Agent, iAgentId)
                oPartyBankDetails = oWebService.GetPartyBankDetails(iAgentId)
            Else

                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                End Select
                oPartyBankDetails = oWebService.GetPartyBankDetails(oParty.Key)
            End If

            'Populate Party bank Details
            oParty.BankDetails = oPartyBankDetails
            Session(CNParty) = oParty

            For Each oPartyBankDetail In oPartyBankDetails
                If (oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "INS" Or oPartyBankDetail.BankPaymentTypeCode.ToUpper() = "ANY") AndAlso _
                    oPartyBankDetail.AccountType.Trim() <> "" AndAlso oPartyBankDetail.IsDeleted = False Then
                    oPartyBankDetailsForInstalment.Add(oPartyBankDetail)
                End If
            Next

            'Add filtered party bank details to cache
            Cache.Insert(ViewState("PartyBankdetailsCacheID"), oPartyBankDetailsForInstalment, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
        End Sub

        ''' <summary>
        ''' Fill the party bank details. 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub FillFormFields(ByVal oFinancePlanDetails As NexusProvider.FinancePlanDetails)
            Dim bMaskBankAccountNumber As Boolean = _
                    CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).MaskBankAccountNumber
            With oFinancePlanDetails
                If .PartyBankKey <> 0 Then
                    ddlAccountType.SelectedValue = .PartyBankKey
                End If

                txtAccountName.Text = .BankAccountName
                txtAccountNumber.Text = .BankAccountNo.Trim()
                txtAddressLine1.Text = .BankAddress1
                txtAddressLine2.Text = .BankAddress2
                txtAddressLine3.Text = .BankTown
                txtAddressLine4.Text = .BankRegion
                txtBankName.Text = .BankName
                txtBranch.Text = .BankBranch
                txtBranchCode.Text = .BankSortCode
                txtDateChanged.Text = .DateBankDetailsChanged
                txtFaxAreaCode.Text = .BankFaxAreaCode
                txtFaxNumber.Text = .BankFaxNo
                txtPhAreaCode.Text = .BankAreaCode
                txtPhExt.Text = .BankExtension
                txtPhNumber.Text = .BankPhoneNo
                txtPostCode.Text = .BankPostCode
                chkDDCancelled.Checked = .IsDDCancelled
                chkPaperDD.Checked = .IsPaperlessDD
                'Bank key though being a string property for some records returns integer (as string)/for some records returns string
                If .BankCountryKey <> 0 Then
                    GISAddress_Country.Value = .BankCountryKey
                 End If
                    Dim oPartyBankDetails As NexusProvider.BankCollection
                    Dim oPartyBankDetail As NexusProvider.Bank
                    Dim sFirstStr As String = String.Empty
                    Dim sLastStr As String = String.Empty

                    If ViewState("PartyBankdetailsCacheID") Is Nothing OrElse Cache.Item(ViewState("PartyBankdetailsCacheID")) Is Nothing Then
                        PopulateAccountFields() 'Fill the PartyBankdetailsCacheID cache
                        PopulateAccountType()
                    End If
                    oPartyBankDetails = Cache.Item(ViewState("PartyBankdetailsCacheID"))
                   
                    If ddlAccountType.SelectedValue <> "0" Then
                        For Each oPartyBankDetail In oPartyBankDetails
                            If oPartyBankDetail.PartyBankKey = ddlAccountType.SelectedValue Then

                                GISAddress_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, oPartyBankDetail.Country, "COUNTRY", False)
                                'Putting the Mask
                                If bMaskBankAccountNumber And txtAccountNumber.Text.Length > 4 Then
                                    sFirstStr = Mid(txtAccountNumber.Text, 1, txtAccountNumber.Text.Length - 4)
                                    sLastStr = Mid(txtAccountNumber.Text, sFirstStr.Length + 1)
                                    For iCount As Integer = 0 To sFirstStr.Length - 1
                                        sFirstStr = sFirstStr.Replace(sFirstStr.Chars(iCount), "*")
                                    Next
                                    txtAccountNumber.Text = sFirstStr & sLastStr
                                End If
                            End If
                        Next
                    End If
               
            End With
        End Sub

        Protected Sub FillGrid(ByVal oBankHistoryDetails As NexusProvider.FinancePlanBankHistoryCollection)
            grdBankHistory.DataSource = oBankHistoryDetails
            grdBankHistory.DataBind()
        End Sub

        ''' <summary>
        ''' Populate account type(ddlAccount) dropdown for party and set first account as selected
        ''' And show detail for selected account 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateAccountType()
            Dim sSelectDefaultText As String = GetLocalResourceObject("lbl_Select_DefaultText")
            'Create PartyBankDetail object from cache
            Dim oPartyBankDetail As NexusProvider.BankCollection = Cache.Item(ViewState("PartyBankdetailsCacheID"))
            Dim oTempBankDetailsCollection As New NexusProvider.BankCollection

            'ddlAccountType.Items.Clear()
            If oPartyBankDetail IsNot Nothing Then
                For icount = 0 To oPartyBankDetail.Count - 1
                    If oPartyBankDetail.Item(icount).IsDeleted = False And oPartyBankDetail.Item(icount).IsBankItem Then
                        oTempBankDetailsCollection.Add(oPartyBankDetail(icount))
                    End If
                Next
            End If

            oPartyBankDetail = oTempBankDetailsCollection

            If oPartyBankDetail.Count > 0 Then

                ddlAccountType.DataSource = oPartyBankDetail
                ddlAccountType.DataTextField = "AccountType"
                ddlAccountType.DataValueField = "PartyBankKey"
                ddlAccountType.DataBind()
            End If
            ddlAccountType.Items.Insert(0, New ListItem(sSelectDefaultText, 0))

            If oPartyBankDetail.Count = 1 Then
                'Set first item as selected from dropdown
                ddlAccountType.SelectedValue = oPartyBankDetail(0).PartyBankKey
                'Show party bank details for selected account type
                ddlAccountType_SelectedIndexChanged(Me, Nothing)
            Else
                ddlAccountType_SelectedIndexChanged(Me, Nothing)
            End If
            SetMandatory()
        End Sub

        Private Sub ClearFields()
            txtAccountName.Text = String.Empty
            txtAccountNumber.Text = String.Empty
            txtAddressLine1.Text = String.Empty
            txtAddressLine2.Text = String.Empty
            txtAddressLine3.Text = String.Empty
            txtAddressLine4.Text = String.Empty
            txtBankName.Text = String.Empty
            txtBranch.Text = String.Empty
            txtBranchCode.Text = String.Empty
            txtDateChanged.Text = String.Empty
            txtFaxAreaCode.Text = String.Empty
            txtFaxNumber.Text = String.Empty
            txtPhAreaCode.Text = String.Empty
            txtPhExt.Text = String.Empty
            txtPhNumber.Text = String.Empty
            txtPostCode.Text = String.Empty
            ddlAccountType.SelectedValue = 0
            GISAddress_Country.Value = ""

        End Sub

        ''' <summary>
        ''' Show party bank details for selected account from dropdown
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlAccountType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles ddlAccountType.SelectedIndexChanged
            'Populate the detail for selected account type.
            'Details will be either for Credit card or Bank(Direct Debit)
            Dim oPartyBankDetails As NexusProvider.BankCollection
            If ViewState("PartyBankdetailsCacheID") Is Nothing OrElse Cache.Item(ViewState("PartyBankdetailsCacheID")) Is Nothing Then
                PopulateAccountFields()
            End If
            oPartyBankDetails = Cache.Item(ViewState("PartyBankdetailsCacheID"))
            Dim oPartyBankDetail As NexusProvider.Bank
            Dim bMaskBankAccountNumber As Boolean = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID().ToString()).MaskBankAccountNumber
            Dim sFirstStr As String = String.Empty
            Dim sLastStr As String = String.Empty
            If ddlAccountType.SelectedValue <> "0" AndAlso oPartyBankDetails IsNot Nothing Then
                For Each oPartyBankDetail In oPartyBankDetails
                    If oPartyBankDetail.PartyBankKey = ddlAccountType.SelectedValue Then

                        txtAccountName.Text = oPartyBankDetail.AccountHolderName.Trim()
                        txtAccountNumber.Text = oPartyBankDetail.AccountNumber.Trim()
                        txtAddressLine1.Text = oPartyBankDetail.StreetName
                        txtAddressLine2.Text = oPartyBankDetail.Locality
                        txtAddressLine3.Text = oPartyBankDetail.PostTown
                        txtAddressLine4.Text = oPartyBankDetail.County
                        txtBankName.Text = oPartyBankDetail.BankName
                        txtBranch.Text = oPartyBankDetail.BankBranch
                        txtBranchCode.Text = oPartyBankDetail.BranchCode
                        txtDateChanged.Text = ""
                        txtFaxAreaCode.Text = ""
                        txtFaxNumber.Text = ""
                        txtPhAreaCode.Text = ""
                        txtPhExt.Text = ""
                        txtPhNumber.Text = ""
                        txtPostCode.Text = oPartyBankDetail.PostCode
                        GISAddress_Country.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, oPartyBankDetail.Country, "COUNTRY", False)

                        'Putting the Mask
                        If bMaskBankAccountNumber And txtAccountNumber.Text.Length > 4 Then
                            sFirstStr = Mid(txtAccountNumber.Text, 1, txtAccountNumber.Text.Length - 4)
                            sLastStr = Mid(txtAccountNumber.Text, sFirstStr.Length + 1)
                            For iCount As Integer = 0 To sFirstStr.Length - 1
                                sFirstStr = sFirstStr.Replace(sFirstStr.Chars(iCount), "*")
                            Next
                            txtAccountNumber.Text = sFirstStr & sLastStr
                        End If

                        'Insert selected party bank account to cache.So that it can be used to create oPayment object at Next click
                        Cache.Insert(ViewState("SelectedAccountTypeCacheId"), oPartyBankDetail, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                        Exit For
                    End If
                Next
                SetMandatory()
                Dim oParty As NexusProvider.BaseParty = Nothing

                'Need to Retreive the Data from Session
                If Session(CNParty) IsNot Nothing Then
                    Select Case True
                        Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                        Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    End Select
                End If

                Dim iBankKey As Integer = 0
                For iBankKey = 0 To oParty.BankDetails.Count - 1
                    If oParty.BankDetails(iBankKey).PartyBankKey = ddlAccountType.SelectedValue Then
                        hypBankEdit.Visible = True
                        If HttpContext.Current.Session.IsCookieless Then
                            If oParty.BankDetails(iBankKey).IsPartyBankLinkedWithInst Then
                                hypBankEdit.OnClientClick = "if( confirm('" & _
                                                            GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & _
                                                            "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & _
                                                            "(S(" & Session.SessionID.ToString() + "))" & _
                                                            "/Modal/BankDetail.aspx?PostbackTo=" & _
                                                            upBankDetails.ClientID.ToString & "&BankKey=" & iBankKey & _
                                                            "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                            Else
                                hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & _
                                                            Session.SessionID.ToString() + "))" & _
                                                            "/Modal/BankDetail.aspx?PostbackTo=" & _
                                                            upBankDetails.ClientID.ToString & "&BankKey=" & iBankKey & _
                                                            "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        Else
                            If oParty.BankDetails(iBankKey).IsPartyBankLinkedWithInst Then
                                hypBankEdit.OnClientClick = "if( confirm('" & _
                                                            GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & _
                                                            "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & _
                                                            "/Modal/BankDetail.aspx?PostbackTo=" & _
                                                            upBankDetails.ClientID.ToString & "&BankKey=" & iBankKey & _
                                                            "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                            Else
                                hypBankEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & _
                                                            "/Modal/BankDetail.aspx?PostbackTo=" & _
                                                            upBankDetails.ClientID.ToString & "&BankKey=" & iBankKey & _
                                                            "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                            End If
                        End If

                        Exit For
                    End If
                Next
                oParty = Nothing
                DisableControls(Me)
            Else
                ClearFields()
                hypBankEdit.Visible = False
            End If

            EnableRequiredFields()
           
              
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Allow "Edit Client" as per BO and web.config file settings
            'If both are TRUE than only allow "Edit Client" otherwise "Edit" button will not be visible
            'Initalize the variables to get and set the editing fucntionality of the user. 
            Dim bEditClient As Boolean
            Dim bIsClientManagerViewOnly As Boolean
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sReturnCode As NexusProvider.OptionTypeSetting
            Dim sReturnCodePaymentType As NexusProvider.OptionTypeSetting 'Payment Type can only be edited on Party
            Try
                'Get the system Option "Enable Editing in Client Manager"
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5000)

                'Get the system Option "Payment Type can only be edited on Party"
                sReturnCodePaymentType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5062)

                'Checking of User Authority for Editing the Client using client manager
                Dim oUserAuthority As New NexusProvider.UserAuthority
                'Get the user name from session
                oUserAuthority.UserCode = Session(CNLoginName)
                'set the authority options for reverse allocation
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.IsClientManagerViewonly
                oWebService = New NexusProvider.ProviderManager().Provider
                'initiate the GetUserAuthority method
                oWebService.GetUserAuthorityValue(oUserAuthority)
                If oUserAuthority.UserAuthorityValue = "1" Then
                    bIsClientManagerViewOnly = True
                Else
                    bIsClientManagerViewOnly = False
                End If

                bEditClient = UserCanDoTask("EditClientDetails")
                'If bEditClientViaClientManager = True and User Can has authority to edit a client
                If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                    If sReturnCode.OptionValue = "1" AndAlso bEditClient AndAlso bIsClientManagerViewOnly = False Then
                        If sReturnCodePaymentType IsNot Nothing AndAlso sReturnCodePaymentType.OptionValue IsNot Nothing AndAlso sReturnCodePaymentType.OptionValue <> "1" Then
                            liEditBank.Visible = True
                        Else
                            liEditBank.Visible = False
                        End If
                    ElseIf bIsClientManagerViewOnly Then
                        liEditBank.Visible = False
                    End If
                ElseIf bIsClientManagerViewOnly Then
                    liEditBank.Visible = False
                End If
            Catch ex As NexusProvider.NexusException
                sReturnCode = Nothing
                sReturnCodePaymentType = Nothing
                bEditClient = Nothing
            End Try
            If HttpContext.Current.Session.IsCookieless Then
                hypBank.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&loc=Instalments&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypBank.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & upBankDetails.ClientID.ToString & "&loc=Instalments&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "abc", "MakeReadOnly();")
        End Sub

        Protected Sub custValAccountType_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValAccountType.ServerValidate
            If ddlAccountType.SelectedValue = "" Then
                custValAccountType.IsValid = False
                custValAccountType.ErrorMessage = GetLocalResourceObject("lbl_ErrorMsg_AccountType")
            Else
                custValAccountType.IsValid = True
            End If
        End Sub

        Protected Sub SetMandatory()
            If ddlAccountType.SelectedValue <> "" Then
                rfvBankName.Enabled = True
                rfvAddress1.Enabled = True
            Else
                rfvAddress1.Enabled = False
                rfvBankName.Enabled = False
            End If
        End Sub

        Public Sub BankAccountDetails_Save()
            Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan
            'If Session(CNFinancePlanDetails) IsNot Nothing Then
            '    oBankAccountDetails = CType(Session(CNFinancePlanDetails), NexusProvider.ProcessPFPlan).PFBankDetails
            If Session(CNFinancePlanDetails) IsNot Nothing Then
                oProcessPFPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)
            End If
            oProcessPFPlan.PFBankDetails = New NexusProvider.PFBankDetails
            oProcessPFPlan.PFBankDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFBankDetails
            'Sending multiple objects because of redundant requests of same type
            With oProcessPFPlan.PremiumFinanceDetails
                If ddlAccountType.Text = "" Then
                    .PartyBankKey = 0
                Else
                    .PartyBankKey = ddlAccountType.Text
                End If

                .BankAccountName = txtAccountName.Text

                .BankAccountNo = txtAccountNumber.Text
                .BankAddress1 = txtAddressLine1.Text
                .BankAddress2 = txtAddressLine2.Text
                .BankAddress3 = txtAddressLine3.Text

                .BankBranch = txtAddressLine3.Text

                .BankName = txtBankName.Text
                .BankBranch = txtBranch.Text
                .BankSortCode = txtBranchCode.Text
                .BankFaxNo = txtFaxNumber.Text
                .BankAreaCode = txtPhAreaCode.Text
                .BankFaxAreaCode = txtFaxAreaCode.Text
                .BankPostCode = txtPostCode.Text
                .BankPhoneNo = txtPhNumber.Text
                .IsDDCancelled = chkDDCancelled.Checked
                .IsPaperlessDD = chkPaperDD.Checked
                .BankCountry = GISAddress_Country.Value
                .BankCountryKey = GetCodeForKey(NexusProvider.ListType.PMLookup, GISAddress_Country.Value, "COUNTRY", False)
                'SAM requires status to be sent from Nexus - OhHold in case of Release button click
                If chkDDCancelled.Checked Then
                    .StatusInd = NexusProvider.FinancePlanStatus.Item140
                End If
                Session(CNFinancePlanDetails) = oProcessPFPlan
            End With

        End Sub

        Private Sub EnableRequiredFields()
            If Session(CNFinancePlanStatus) IsNot Nothing AndAlso Session(CNFinancePlanStatus).ToString() = "On Hold" Then
                chkDDCancelled.Enabled = False
            Else
                chkDDCancelled.Enabled = True
            End If
            ddlAccountType.Enabled = True
            chkPaperDD.Enabled = True
            txtPhAreaCode.Attributes.Remove("readonly")
            txtPhExt.Attributes.Remove("readonly")
            txtPhNumber.Attributes.Remove("readonly")
            txtFaxAreaCode.Attributes.Remove("readonly")
            txtFaxNumber.Attributes.Remove("readonly")
            txtPhExt.Attributes.Remove("readonly")

        End Sub

        Public Sub ReleasePlanCall()
            'Set first item as selected from dropdown
            ddlAccountType.SelectedValue = "0"
            'Show party bank details for selected account type
            ddlAccountType_SelectedIndexChanged(Me, Nothing)
            rfvAccountName.Enabled = True
            rfvAccountNumber.Enabled = True
            chkDDCancelled.Checked = False
             ddlAccountType.SelectedIndex = 0
            txtAccountName.Text = String.Empty
            txtAccountNumber.Text = String.Empty
            txtAddressLine1.Text = String.Empty
            txtAddressLine2.Text = String.Empty
            txtAddressLine3.Text = String.Empty
            txtAddressLine4.Text = String.Empty
            txtBankName.Text = String.Empty
            txtBranch.Text = String.Empty
            txtBranchCode.Text = String.Empty
            txtDateChanged.Text = String.Empty
            txtFaxAreaCode.Text = String.Empty
            txtFaxNumber.Text = String.Empty
            txtFaxNumber.Text = String.Empty
            txtPhAreaCode.Text = String.Empty
            txtPhExt.Text = String.Empty
            txtPhNumber.Text = String.Empty
            txtPostCode.Text = String.Empty
        End Sub
    End Class


End Namespace