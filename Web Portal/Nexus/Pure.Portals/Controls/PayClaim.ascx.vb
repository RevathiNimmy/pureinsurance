Imports Nexus.Utils
Imports Nexus.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Imports System.Web.Configuration.WebConfigurationManager
Namespace Nexus
    Partial Class Controls_PayClaim
        Inherits System.Web.UI.UserControl
        Public Const CNClaimPerilReservePaymentCollection As String = "ClaimPerilReservePaymentCollection"
        Public Const CNClaimPerilRecoveryCollection As String = "ClaimPerilRecoveryCollection"
        Private oMaster As ContentPlaceHolder
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Public bdomiciledTax As Boolean = False
        Private nClaimPerilId As Integer = 0
        Public m_sIsPaymentsReadOnly As String
        Private bIsWithholdingTax As Boolean = False
        Private oPerilRecoveryCollection As NexusProvider.PerilRecoveryCollection
        Private sPartyId As String = String.Empty

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PayClaimWarningConfirmation",
                                                                    "<script language=""JavaScript"" type=""text/javascript"">function PayClaimWarningConfirmation(){ var r= confirm('" & GetLocalResourceObject("msg_AllowMultipleClaimPayment_error").ToString() & "'); document.getElementById('" & HidPayClaimWarningConfirmation.ClientID & "').value=r;}</script>")
            If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Or CType(Session.Item(CNMode), Mode) = Mode.EditClaim Or CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Then
                hfRememberTabs.Value = "0"
            ElseIf CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                m_sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If m_sIsPaymentsReadOnly = "1" Then
                    hfRememberTabs.Value = "2"
                Else
                    hfRememberTabs.Value = "1"
                End If
            End If

            Dim sUrl As String()
            Dim Depthindex As Integer = -1
            Dim flag As Boolean = False
            sUrl = Request.Url.ToString().Split("/")
            For i = 0 To sUrl.Length - 1
                If sUrl(i).ToString().Contains("?") Then
                    Exit For
                End If
                If sUrl(i).ToString().ToLower().Contains("claims") Then
                    flag = True
                End If
                If flag Then
                    Depthindex = Depthindex + 1
                End If
            Next
            If Depthindex < 3 Then
                Depthindex = 3
            End If


        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                calChequeDate.Enabled = False
            End If
            If Request.QueryString("PerilID") IsNot Nothing Then
                nClaimPerilId = CInt(Request.QueryString("PerilID"))
            End If
            Dim oTaxGroupCollection As NexusProvider.TaxGroupCollection = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim IswithholdingTax As Boolean = False
            oTaxGroupCollection = oWebservice.GetTaxGroupsForClaims(IswithholdingTax)
            If oTaxGroupCollection IsNot Nothing AndAlso oTaxGroupCollection.Count > 0 Then
                bIsWithholdingTax = oTaxGroupCollection.Item(0).IsWithHoldingTax
            End If
            If CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Tasks.Task("ClaimPayableAvailable") IsNot Nothing Then
                If Session(CNMode) = Mode.PayClaim Then
                    If UserCanDoTask("ClaimPayableAvailable") Then
                        rblPayee.Items(0).Enabled = True
                        If rblPayee.Items(0).Selected Then
                            txtAccountType.Visible = True
                            ddlAccountType.Visible = False
                        Else
                            txtAccountType.Visible = False
                            ddlAccountType.Visible = True
                        End If
                    Else
                        rblPayee.Items(0).Enabled = False
                        txtAccountType.Visible = False
                        ddlAccountType.Visible = True
                    End If
                End If
            End If
            If Not IsPostBack Then
                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM or Session call
                Dim CNClaimPerilReservePaymentCollectionCacheID As Guid
                CNClaimPerilReservePaymentCollectionCacheID = Guid.NewGuid
                ViewState.Add(CNClaimPerilReservePaymentCollection, CNClaimPerilReservePaymentCollectionCacheID.ToString)

                'create a unique key and add this to viewstate
                'this will be used to cache the results of the SAM or Session call
                Dim CNClaimPerilRecoveryCollectionCacheID As Guid
                CNClaimPerilRecoveryCollectionCacheID = Guid.NewGuid
                ViewState.Add(CNClaimPerilRecoveryCollection, CNClaimPerilRecoveryCollectionCacheID.ToString)

                If rblPayee.SelectedIndex = -1 Then
                    gvPaymentDetails.Columns(9).Visible = False
                    gvSalvageDetails.Columns(9).Visible = False
                End If
                Dim sOption As String
                sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsAdvancedTaxScriptEnabled, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                HidATSoption.Value = sOption
                If Not String.IsNullOrEmpty(sOption) AndAlso sOption = "1" AndAlso Session(CNMode) = Mode.PayClaim Then
                    ddlPayment_To.Visible = True
                    lblpayment_To.Visible = True
                    outerDivATS.Style("display") = "block"
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Policy Holder"))).Enabled = False
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Party"))).Enabled = False
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Claim Payable"))).Enabled = False
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Agent"))).Enabled = False

                    Dim oLookUP As New NexusProvider.LookupListCollection

                    'sam call to retreive the list of branch from table source
                    oLookUP = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Claim_Payment_To", True, False)

                    If ddlPayment_To IsNot Nothing Then
                        'existing items cleared
                        ddlPayment_To.Items.Clear()
                        ddlPayment_To.Items.Add("")
                        For iPaymentCount As Integer = 0 To oLookUP.Count - 1
                            Dim lstPayment As New ListItem
                            lstPayment.Text = oLookUP(iPaymentCount).Description
                            lstPayment.Value = Trim(oLookUP(iPaymentCount).Code)
                            ddlPayment_To.Items.Add(lstPayment)
                            ddlPayment_To.DataBind()
                        Next
                    End If

                    txtPercentageITA.Attributes.Add("onkeypress", "javascript:return fnValidatePercentage(this.value,event);")
                    txtPercentage_ITA.Attributes.Add("onkeypress", "javascript:return isInteger(event);")
                    'txtPercentagePTA.Attributes.Add("onkeypress", "javascript:return isInteger(event);")
                    txtPercentagePTA.Attributes.Add("onkeypress", "javascript:return fnValidatePercentage(this.value,event);")
                    txtPercentageRTS.Attributes.Add("onkeypress", "javascript:return isInteger(event);")
                End If
                If Not String.IsNullOrEmpty(sOption) AndAlso sOption = "1" AndAlso Session(CNMode) = Mode.ViewClaim Then
                    ddlPayment_To.Visible = False
                    lblpayment_To.Visible = False
                    outerDivATS.Style("display") = "none"
                End If

                If sOption = "0" Then
                    outerDivATS.Visible = False
                    divsalvage.Visible = False
                End If

                ' For Salvage and Recovery
                If Not String.IsNullOrEmpty(sOption) AndAlso sOption = "1" AndAlso Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                    divsalvage.Style("display") = "block"
                End If
            End If
            If Request("__EVENTARGUMENT") = "PaymentUpdation" Then
                'check if the postback has been triggered by the modal dialog "PaymentDetails"
                'Removed PayClaimError Session, so that new payment amount store in PayClaim SAM Method.
                Session.Remove(CNPayClaimError)
                Session.Remove(CNEnablePayClaim)
                Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    Cache.Remove(ViewState(CNClaimPerilRecoveryCollection))
                    PopulateSalvageClaim(PerilIndex, oClaimOpen)

                ElseIf Session(CNMode) = Mode.PayClaim Then
                    Cache.Remove(ViewState(CNClaimPerilReservePaymentCollection))
                    PopulatePayClaim(PerilIndex, oClaimOpen)
                End If
                PopulateThisPayment(PerilIndex, oClaimOpen)
            ElseIf Request("__EVENTARGUMENT") = "PaymentCancelled" Then
                ' Modal was cancelled � only reset the selected row (re-enable all buttons)
                ' if the row has never been receipted (first-time cancel).
                ' If OK was pressed before (receipt amount > 0), keep the row selected so
                ' other edit buttons remain disabled.
                Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim iSelectedRow As Integer = -1
                Integer.TryParse(hfSelectedRecoveryRow.Value, iSelectedRow)
                Dim bHasReceiptAmount As Boolean = False
                If iSelectedRow >= 0 Then
                    If Session(CNMode) = Mode.SalvageClaim Then
                        If iSelectedRow < oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery.Count Then
                            bHasReceiptAmount = oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iSelectedRow).LossThisReceiptINCLTax <> 0 OrElse
                                                oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iSelectedRow).ReceiptedAmount <> 0
                        End If
                    ElseIf Session(CNMode) = Mode.TPRecovery Then
                        If iSelectedRow < oClaimOpen.ClaimPeril(PerilIndex).TPRecovery.Count Then
                            bHasReceiptAmount = oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iSelectedRow).LossThisReceiptINCLTax <> 0 OrElse
                                                oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iSelectedRow).ReceiptedAmount <> 0
                        End If
                    End If
                End If
                If Not bHasReceiptAmount Then
                    ' First-time cancel � reset so all edit buttons are re-enabled
                    hfSelectedRecoveryRow.Value = "-1"
                End If
                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    Cache.Remove(ViewState(CNClaimPerilRecoveryCollection))
                    PopulateSalvageClaim(PerilIndex, oClaimOpen)
                ElseIf Session(CNMode) = Mode.PayClaim Then
                    Cache.Remove(ViewState(CNClaimPerilReservePaymentCollection))
                    PopulatePayClaim(PerilIndex, oClaimOpen)
                End If
                PopulateThisPayment(PerilIndex, oClaimOpen)
            End If

            'Check MediaTypeFieldMandatory on Claim Payment       
            CheckMediaTypeFieldMandatory()
            If HidMediaTypeFieldMandatory.Value = "1" Then
                'Mandatory Field active 
                emMediatype.Visible = True
                GISLookup_MediaType.Enabled = True
                If Not GISLookup_MediaType.CssClass.Contains("field-mandatory") Then
                    GISLookup_MediaType.CssClass = GISLookup_MediaType.CssClass & " field-mandatory"
                End If
            End If

            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PaymentClientSearch Then
                btnClientParty.Visible = True
            Else
                btnClientParty.Visible = False
            End If
            'whenever user select any other party , should Display Selected party detail on screen
            If Request("__EVENTARGUMENT") = "OtherPartySelection" Then
                DisplayAccountTypeInformation()
                gvPaymentDetails.Columns(9).Visible = True
                gvSalvageDetails.Columns(9).Visible = True
            End If

            'whenever user select any ReInsure
            If Request("__EVENTARGUMENT") = "ReInsureSelection" Then
                txtParty.Text = hInsurerName.Value
                gvPaymentDetails.Columns(9).Visible = True
                gvSalvageDetails.Columns(9).Visible = True
            End If

            If Request("__EVENTARGUMENT") = "ClientPartySelection" Then
                DisplayAccountTypeInformation()
                gvPaymentDetails.Columns(9).Visible = True
                gvSalvageDetails.Columns(9).Visible = True
            End If

            ''''Update Session InsuranceTaxNumber,PayeeTaxNumber and InsuredTaxNumber of payment and Receipt
            If (Session(CNMode) = Mode.PayClaim OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) AndAlso (txtTaxnoITA.Text.Length > 0 OrElse txtTaxNoPTA.Text.Length > 0 OrElse txtTaxNo_ITA.Text.Length > 0) Then

                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaimOpen IsNot Nothing Then
                    Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                    If Session(CNMode) = Mode.PayClaim Then
                        oClaimOpen.ClaimPeril(iPeril).Payment.PaymentAdvancedTaxDetails.InsuranceTaxNumber = txtTaxnoITA.Text
                        oClaimOpen.ClaimPeril(iPeril).Payment.PaymentAdvancedTaxDetails.PayeeTaxNumber = txtTaxNoPTA.Text
                    ElseIf Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                        oClaimOpen.ClaimPeril(iPeril).Receipt.AdvancedTaxDetails.InsuredTaxNumber = txtTaxNo_ITA.Text
                    End If
                    Session(CNClaim) = oClaimOpen
                End If
            End If
            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or Session(CNMode) = Mode.PayClaim Then
                If rblPayee.SelectedValue = "1" And txtParty.Text IsNot Nothing And txtParty.Text <> "" Then
                    hfRememberTabs.Value = "2"
                End If
            End If
            If IsPostBack Then
                If Session(CNMode) = Mode.PayClaim Then
                    If Request("__EVENTARGUMENT") = "ProcessClaimPaymentNextButton" Then
                        Session(CNDuplicateClaimPayment) = True
                        Session(CNDuplicateClaimPaymentReason) = hdnDuplicateClaimPaymentReason.Value
                    ElseIf Request("__EVENTARGUMENT") = "ProcessClaimPaymentFinishButton" Then
                        Session(CNDuplicateClaimPayment) = True
                        Session(CNDuplicateClaimPaymentReason) = hdnDuplicateClaimPaymentReason.Value
                    Else

                        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                        hdnDuplicateClaimPayment.Value = False
                        Dim oClaimPaymentCollection As NexusProvider.ClaimPaymentCollection
                        Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                        Dim sBranchCode As String = Nothing
                        sBranchCode = Session(CNBranchCode)
                        Dim oClaimVersions1 As NexusProvider.VersionsCollections = Nothing
                        oClaimVersions1 = oWebservice.GetVersionsForClaim(oClaimOpen.ClaimNumber)
                        For iCounterinner = 0 To oClaimVersions1.Count - 1
                            oClaimDetails = GetClaimDetailsCall(oClaimVersions1(iCounterinner).ClaimKey, sBranchCode, 0)
                            If oClaimDetails.ClaimPeril.Count > 0 Then
                                For iPerilout As Integer = 0 To oClaimDetails.ClaimPeril.Count - 1
                                    For iPerilReserve As Integer = 0 To oClaimDetails.ClaimPeril(iPerilout).ClaimPayment.Count - 1
                                        If oClaimDetails.ClaimPeril(iPerilout).ClaimPayment(iPerilReserve).BaseAmount > 0 Then
                                            'oClaimPaymentCollection = oClaimPaymentCollection.Add(oClaimOpen.ClaimPeril(i).ClaimPayment)
                                            If oClaimDetails.ClaimPeril(iPerilout).ClaimPayment(iPerilReserve).UltimatePayee IsNot Nothing Then
                                                If txtUltimatePayee.Text.ToUpper = oClaimDetails.ClaimPeril(iPerilout).ClaimPayment(iPerilReserve).UltimatePayee.ToUpper Then
                                                    ViewState("Ultimate") = oClaimDetails.ClaimPeril(iPerilout).ClaimPayment(iPerilReserve).UltimatePayee.ToUpper
                                                End If
                                            End If
                                            If oClaimDetails.ClaimPeril.Item(iPerilout).ClaimPayment(iPerilReserve).OurRef IsNot Nothing Then
                                                If txtOurRef.Text.ToUpper = oClaimDetails.ClaimPeril.Item(iPerilout).ClaimPayment(iPerilReserve).OurRef.ToUpper Then
                                                    ViewState("OurRef") = oClaimDetails.ClaimPeril(iPerilout).ClaimPayment.Item(iPerilReserve).OurRef.ToUpper
                                                End If
                                            End If
                                            ViewState("PartyBank") = oClaimDetails.ClaimPeril.Item(iPerilout).ClaimPayment.Item(iPerilReserve).Payee.PartyBankKey
                                            ViewState("PaymentDate") = oClaimDetails.ClaimPeril.Item(iPerilout).ClaimPayment.Item(iPerilReserve).PaymentDate
                                        End If
                                    Next
                                Next
                            End If
                        Next
                        oClaimVersions1 = oWebservice.GetVersionsForClaim(oClaimOpen.ClaimNumber)
                        oClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        If oClaimOpen IsNot Nothing Then
                            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                            oClaimPaymentCollection = oClaimOpen.ClaimPeril(iPeril).ClaimPayment
                        End If
                        If oPortal.DuplicateClaimPaymentCheckParameters IsNot Nothing Then
                            If oPortal.DuplicateClaimPaymentCheckParameters.Trim.Length <> 0 Then
                                Dim sParameters() As String
                                If oClaimOpen IsNot Nothing Then
                                    Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                                    oClaimPaymentCollection = oClaimOpen.ClaimPeril(iPeril).ClaimPayment
                                    If oClaimPaymentCollection.Count = 0 Then
                                        oClaimPaymentCollection = oClaimOpen.ClaimPeril(0).ClaimPayment
                                    End If
                                End If
                                Dim iprmcnt As Integer = 0
                                sParameters = oPortal.DuplicateClaimPaymentCheckParameters.Split(",")
                                For iCounter As Integer = 0 To sParameters.Length - 1
                                    For Each oPayment As NexusProvider.ClaimPayment In oClaimPaymentCollection


                                        'if sParameters retrive Name option, Attached SearchCriteria with Name
                                        If sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ACCNAME" Then
                                            If txtPayeeName.Text.Length > 0 Then
                                                If oPayment.Payee.Name IsNot Nothing Then
                                                    If oPayment.Payee.Name.ToUpper = (txtPayeeName.Text).ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "ACCNAME", txtPayeeName.Text.ToUpper)
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ACCNUMBER" Then
                                            If txtBankAccNumber.Text.Length > 0 Then
                                                If oPayment.Payee.BankNumber IsNot Nothing Then
                                                    If oPayment.Payee.BankNumber.ToUpper = (txtBankAccNumber.Text).ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "ACCNUMBER", txtBankAccNumber.Text.ToUpper)
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "BANKCODE" Then
                                            If txtBankCode.Text.Length > 0 Then
                                                If oPayment.Payee.BankCode IsNot Nothing Then
                                                    If oPayment.Payee.BankCode.ToUpper = (txtBankCode.Text).ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "BANKCODE", txtBankCode.Text.ToUpper)
                                                    End If
                                                End If
                                            End If

                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "BANKBRANCH" Then
                                            If txtBankCode.Text.Length > 0 Then
                                                If oPayment.Payee.BankCode IsNot Nothing Then
                                                    If oPayment.Payee.BankCode.ToUpper = (txtBankCode.Text).ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "BANKBRANCH", txtBankCode.Text.ToUpper)
                                                    End If
                                                End If
                                            End If

                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "BANKNAME" Then
                                            If txtBankName.Text.Length > 0 Then
                                                If oPayment.Payee.BankName IsNot Nothing Then
                                                    If oPayment.Payee.BankName.ToUpper = (txtBankName.Text).ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "BANKNAME", txtBankName.Text.ToUpper)
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "GRSPAYAMOUNT" Then
                                            If txtGrossPayment.Text.Length > 0 Then
                                                If ViewState("taxamount") IsNot Nothing Then
                                                    If Convert.ToDecimal(ViewState("taxamount")) = oPayment.TaxAmount Then
                                                        If Convert.ToDecimal(Convert.ToDecimal(txtNetPayment.Text)) = oPayment.PaymentAmount Then
                                                            If (oPayment.PaymentAmount) + (oPayment.TaxAmount) = Convert.ToDecimal(txtNetPayment.Text) + Convert.ToDecimal(ViewState("taxamount")) Then
                                                                hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "GRSPAYAMOUNT", Convert.ToDecimal(txtNetPayment.Text) + Convert.ToDecimal(ViewState("taxamount")))
                                                            End If
                                                        End If

                                                    End If
                                                End If
                                            End If

                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "MEDIATYPE" Then
                                            If GISLookup_MediaType.Value IsNot Nothing Then
                                                If GISLookup_MediaType.Value.Length > 0 Then
                                                    If oPayment.Payee.MediaTypeCode IsNot Nothing Then
                                                        If oPayment.Payee.MediaTypeCode.Trim() = (GISLookup_MediaType.Value) Then
                                                            hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "MEDIATYPE", GISLookup_MediaType.Value)
                                                        End If
                                                    End If
                                                End If
                                            Else
                                                hdnDuplicateClaimPayment.Value = True
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "PAYDATE" Then
                                            If txtChequeDate.Text.Length > 0 Then
                                                If ViewState("PaymentDate") IsNot Nothing Then
                                                    Dim Paymentdate As Date = ViewState("PaymentDate")
                                                    If Paymentdate.ToString("dd/MM/yyyy") = Convert.ToDateTime(txtChequeDate.Text).ToString("dd/MM/yyyy") Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "PAYDATE", Convert.ToDateTime(txtChequeDate.Text).ToString("dd/MM/yyyy"))
                                                    End If

                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "PAYEENAME" Then
                                            If txtPayeeName.Text.Length > 0 Then
                                                If oPayment.Payee.Name IsNot Nothing Then
                                                    If oPayment.Payee.Name.ToUpper = txtPayeeName.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "PAYEENAME", txtPayeeName.Text.ToUpper)
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "THEIRREF" Then
                                            If txtThisReference.Text.Length > 0 Then
                                                If oPayment.Payee.TheirReference IsNot Nothing Then
                                                    If oPayment.Payee.TheirReference.ToUpper = txtThisReference.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "THEIRREF", txtThisReference.Text.ToUpper)
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "PARTYTYPE" Then

                                            If rblPayee.SelectedItem IsNot Nothing Then
                                                If rblPayee.SelectedValue = 0 Then
                                                    If txtParty.Text.Length > 0 Then
                                                        If oPayment.PartyPaidName = "Claim Payable" Then
                                                            txtParty.Text = "Claim Payable"
                                                            If oClaimDetails.ClaimPeril.Item(0).ClaimPayment.Count > 1 And oPayment.BaseAmount > 0 Then
                                                                If oPayment.PartyPaidName = txtParty.Text Then
                                                                    hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "PARTYTYPE", txtParty.Text)
                                                                End If
                                                            End If
                                                            If oPayment.PartyPaidName = "Claim Payable" Then
                                                                txtParty.Text = "CLMPAYABLE"
                                                            End If
                                                        End If
                                                    End If
                                                ElseIf rblPayee.SelectedValue = 3 Or rblPayee.SelectedValue = 2 Or rblPayee.SelectedValue = 4 Then

                                                    If txtParty.Text.Length > 0 Then
                                                        If oPayment.PartyPaidName.Trim.ToUpper = txtParty.Text.ToUpper Then
                                                            hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "PARTYTYPE", txtParty.Text)
                                                        End If
                                                    End If

                                                ElseIf rblPayee.SelectedValue = 1 Then
                                                    If oPayment.PartyPaidCode IsNot Nothing Then
                                                        If txtParty.Text.Length > 0 Then
                                                            If oPayment.PartyPaidCode.Trim.ToUpper = txtParty.Text.ToUpper Then
                                                                hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "PARTYTYPE", txtParty.Text)
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ULTIMATEPAYEE" Then
                                            If txtUltimatePayee.Text.Length > 0 Then
                                                If ViewState("Ultimate") IsNot Nothing Then
                                                    If ViewState("Ultimate") = txtUltimatePayee.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "ULTIMATEPAYEE", txtUltimatePayee.Text.ToUpper)

                                                    End If
                                                End If

                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "NETPAYAMOUNT" Then
                                            If txtNetPayment.Text.Length > 0 Then
                                                If oPayment.PaymentAmount = Convert.ToDecimal(txtNetPayment.Text) Then
                                                    hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "NETPAYAMOUNT", txtNetPayment.Text)
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "PAYCURRENCY" Then

                                            If oPayment.CurrencyCode IsNot Nothing Then
                                                If oPayment.CurrencyCode.Trim() = oClaimOpen.ClaimPeril.Item(0).Payment.CurrencyCode Then
                                                    hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "PAYCURRENCY", oClaimOpen.ClaimPeril.Item(0).Payment.CurrencyCode)
                                                End If
                                            End If


                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "LOSSCURRENCY" Then

                                            If oPayment.LossCurrencyCode IsNot Nothing Then
                                                If oPayment.LossCurrencyCode.Trim() = oClaimOpen.ClaimPeril.Item(0).Payment.LossCurrencyCode.Trim() Then
                                                    hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "LOSSCURRENCY", oClaimOpen.ClaimPeril.Item(0).Payment.LossCurrencyCode.Trim)
                                                End If
                                            End If


                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ACCOUNTTYPE" Then
                                            If ddlAccountType.SelectedValue.Length > 0 Then
                                                If ViewState("PartyBank") IsNot Nothing Then
                                                    If ViewState("PartyBank") = ddlAccountType.SelectedValue Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "ACCOUNTTYPE", ddlAccountType.SelectedValue)
                                                    End If
                                                End If
                                            Else
                                                If oClaimDetails.ClaimPeril.Item(0).ClaimPayment.Count - 1 > 0 Then
                                                    hdnDuplicateClaimPayment.Value = True
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "MEDIAREF" Then
                                            If txtMediaRef.Text.Length > 0 Then
                                                If oPayment.Payee.MediaReference IsNot Nothing Then
                                                    If oPayment.Payee.MediaReference.ToUpper = txtMediaRef.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "MEDIAREF", txtMediaRef.Text.ToUpper)
                                                    End If
                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "OURREF" Then
                                            If txtOurRef.Text.Length > 0 Then
                                                If ViewState("OurRef") IsNot Nothing Then
                                                    If ViewState("OurRef") = txtOurRef.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "OURREF", txtOurRef.Text.ToUpper)
                                                    End If
                                                End If
                                            End If

                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "BIC" Then
                                            If txtBIC.Text.Length > 0 Then
                                                If oPayment.Payee.BIC IsNot Nothing Then
                                                    If oPayment.Payee.BIC.ToUpper = txtBIC.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "BIC", txtBIC.Text.ToUpper)
                                                    End If
                                                End If
                                            Else
                                                hdnDuplicateClaimPayment.Value = True
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "IBAN" Then
                                            If txtIBAN.Text.Length > 0 Then
                                                If oPayment.Payee.IBAN IsNot Nothing Then
                                                    If oPayment.Payee.IBAN.ToUpper = txtIBAN.Text.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "IBAN", txtIBAN.Text.ToUpper)
                                                    End If
                                                End If
                                            Else
                                                hdnDuplicateClaimPayment.Value = True
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "ADD1" Then
                                            If Address.Address1.Length > 0 Then
                                                If oPayment.Payee.Address.Address1 IsNot Nothing Then
                                                    If oPayment.Payee.Address.Address1.ToUpper = Address.Address1.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "ADD1", Address.Address1.ToUpper)
                                                    End If
                                                End If

                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "POSTCODE" Then
                                            If oPayment.Payee.Address.PostCode IsNot Nothing Then
                                                If Address.Postcode.Length > 0 Then

                                                    If oPayment.Payee.Address.PostCode.ToUpper = Address.Postcode.ToUpper Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "POSTCODE", Address.Postcode.ToUpper)
                                                    End If

                                                End If
                                            End If
                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "COUNTRY" Then
                                            If oPayment.Payee.Address.CountryCode IsNot Nothing Then
                                                If Address.CountryCode IsNot Nothing Then
                                                    If oPayment.Payee.Address.CountryCode.Trim = Address.CountryCode.Trim Then
                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "COUNTRY", Address.CountryCode)
                                                    End If
                                                End If

                                            End If

                                        ElseIf sParameters(iCounter).Trim.Length <> 0 And sParameters(iCounter).Trim.ToUpper = "COMPLETEADD" Then

                                            If oPayment.Payee.Address.Address1 IsNot Nothing Then

                                                If oPayment.Payee.Address.Address3 IsNot Nothing Then

                                                    If oPayment.Payee.Address.CountryCode IsNot Nothing Then
                                                        If oPayment.Payee.Address.PostCode IsNot Nothing Then

                                                            If oPayment.Payee.Address.PostCode.Length > 0 Then
                                                                If oPayment.Payee.Address.Address2 IsNot Nothing Then
                                                                    If oPayment.Payee.Address.Address2.Length > 0 Then
                                                                        If oPayment.Payee.Address.Address4 IsNot Nothing Then
                                                                            If oPayment.Payee.Address.Address4.Length > 0 Then
                                                                                If Address.CountryCode IsNot Nothing Then

                                                                                    ViewState("COMPLETEADD") = oPayment.Payee.Address.Address1.ToUpper.Trim + " " + oPayment.Payee.Address.Address2.ToUpper.Trim + " " + oPayment.Payee.Address.Address3.ToUpper.Trim + " " + oPayment.Payee.Address.Address4.ToUpper.Trim + " " + oPayment.Payee.Address.PostCode.ToUpper.Trim + " " + oPayment.Payee.Address.CountryCode.Trim

                                                                                    Dim completeAdd As String = Address.Address1.ToUpper.Trim + " " + Address.Address2.ToUpper.Trim + " " + Address.Address3.ToUpper.Trim + " " + Address.Address4.ToUpper.Trim + " " + Address.Postcode.ToUpper.Trim + " " + Address.CountryCode.Trim
                                                                                    If ViewState("COMPLETEADD") = completeAdd Then
                                                                                        hdnDuplicateClaimPayment.Value = MatchDuplicateClaimPayment(oPayment, "COMPLETEADD", completeAdd)
                                                                                    End If
                                                                                End If
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If


                                        End If
                                        If hdnDuplicateClaimPayment.Value = True Then
                                            iprmcnt += 1
                                            hdnDuplicateClaimPayment.Value = False
                                            Exit For
                                        End If


                                    Next

                                    If iprmcnt = sParameters.Length Then
                                        hdnDuplicateClaimPayment.Value = True

                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            End If


        End Sub
        Protected Function MatchDuplicateClaimPayment(ByVal oClaimPayment As NexusProvider.ClaimPayment, ByVal searchParameter As String, ByVal searchValue As String) As Boolean


            Select Case searchParameter
                Case "ACCNAME"
                    If oClaimPayment.Payee.Name.Trim().ToUpper = searchValue Then
                        Return True
                    Else
                        Return False
                    End If
                Case "ACCNUMBER"
                    If oClaimPayment.Payee.BankNumber.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
                Case "BANKCODE"
                    If oClaimPayment.Payee.BankCode.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "BANKBRANCH"
                    If oClaimPayment.Payee.BankCode.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "BANKNAME"
                    If oClaimPayment.Payee.BankName.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
                Case "GRSPAYAMOUNT"
                    If (oClaimPayment.PaymentAmount) + (oClaimPayment.TaxAmount) = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "MEDIATYPE"
                    If oClaimPayment.Payee.MediaTypeCode.Trim() = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "PAYDATE"
                    Dim Paymentdate As Date = ViewState("PaymentDate")
                    If Paymentdate.ToShortDateString() = searchValue Then
                        'If oClaimPayment.PaymentDate.ToShortDateString()).ToShortDateString() = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "PAYEENAME"
                    If oClaimPayment.Payee.Name.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "THEIRREF"
                    If oClaimPayment.Payee.TheirReference.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "PARTYTYPE"
                    If rblPayee.SelectedValue = 0 Or rblPayee.SelectedValue = 3 Or rblPayee.SelectedValue = 2 Or rblPayee.SelectedValue = 4 Then
                        If oClaimPayment.PartyPaidName.Trim.ToUpper = searchValue.ToUpper Then
                            Return True
                        Else
                            Return False
                        End If
                    ElseIf rblPayee.SelectedValue = 1 Then
                        If oClaimPayment.PartyPaidCode.Trim.ToUpper = searchValue.ToUpper Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Case "ACCOUNTTYPE"
                    If ViewState("PartyBank") IsNot Nothing Then
                        If ViewState("PartyBank") = searchValue Then
                            Return True
                        End If
                    Else
                        Return False

                    End If


                Case "ULTIMATEPAYEE"
                    If ViewState("Ultimate") IsNot Nothing Then
                        If ViewState("Ultimate") = searchValue Then
                            Return True
                        End If
                    Else
                        Return False

                    End If
                Case "NETPAYAMOUNT"
                    If oClaimPayment.PaymentAmount = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "PAYCURRENCY"
                    If oClaimPayment.CurrencyCode.Trim = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "LOSSCURRENCY"
                    If oClaimPayment.LossCurrencyCode.Trim = searchValue Then
                        Return True
                    Else
                        Return False

                    End If



                Case "MEDIAREF"
                    If oClaimPayment.Payee.MediaReference.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "OURREF"
                    If ViewState("OurRef") IsNot Nothing Then


                        If ViewState("OurRef") = searchValue Then
                            Return True
                        End If
                    Else
                        Return False

                    End If


                Case "BIC"
                    If oClaimPayment.Payee.BIC.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If


                Case "IBAN"
                    If oClaimPayment.Payee.IBAN.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "ADD1"
                    If oClaimPayment.Payee.Address.Address1.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If

                Case "POSTCODE"
                    If oClaimPayment.Payee.Address.PostCode.ToUpper = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
                Case "COUNTRY"
                    If oClaimPayment.Payee.Address.CountryCode.Trim() = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
                Case "COMPLETEADD"
                    If ViewState("COMPLETEADD") = searchValue Then
                        Return True
                    Else
                        Return False

                    End If
            End Select


        End Function
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'make Party ReadOnly
            EnableControls(Me)
            txtParty.Attributes.Add("readonly", "readonly")
            txtParty.ReadOnly = True
            txtChequeDate.Text = Date.Now.ToShortDateString()
            txtChequeDate.ReadOnly = True
            Dim oSelectedBaseParty As NexusProvider.BaseParty = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Select Case rblPayee.SelectedValue
                Case "1"
                    btnParty.Enabled = True
                    btnClientParty.Enabled = True
                Case "4"
                    btnClientParty.Enabled = False
                    btnParty.Enabled = True
                Case Else
                    btnClientParty.Enabled = False
                    btnParty.Enabled = False
            End Select

            If Not IsPostBack Then
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                EnableControls(Me)
                Dim oLookupListMediaType As NexusProvider.LookupList
                If Session(CNMode) = Mode.PayClaim Then
                    oLookupListMediaType = FindControl("GISLookup_MediaType")
                    oLookupListMediaType.ParentFieldName = "Is_Claim_Payment"
                    oLookupListMediaType.ParentFieldValue = 1
                ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    oLookupListMediaType = FindControl("GISLookup_MediaType")
                    oLookupListMediaType.ParentFieldName = "Is_Claim_Recovery"
                    oLookupListMediaType.ParentFieldValue = 1
                End If
                Dim oClaimOpen As NexusProvider.ClaimOpen = Nothing
                Dim oExGratiaOptionSettings As NexusProvider.OptionTypeSetting
                'Retreiving the claim quote information from session
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                If Session.Item(CNClaim) IsNot Nothing Then
                    'Retreiving the claim  information from session
                    m_sIsPaymentsReadOnly = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                    oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    GetReserves(oClaimOpen.RiskKey)
                    lblLossCurrency.Text = GetCurrencyForCode(oClaimOpen.CurrencyISOCode)
                    lblDateOfLoss.Text = oClaimOpen.LossToDate
                    lblPerilInfo.Text = oClaimOpen.ClaimPeril(iPeril).Description
                    If oQuote.BusinessTypeCode = "DIRECT" Then
                        rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Agent"))).Enabled = False
                    End If

                    For nCnt As Integer = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(nCnt).Key = oClaimOpen.RiskKey Then
                            lblRiskType.Text = Convert.ToString(oQuote.Risks(nCnt).Description)
                            Exit For
                        End If
                    Next

                    rblPayee.Visible = True
                    If oClaimOpen.ClaimPeril(iPeril).Payment IsNot Nothing Then
                        txtUltimatePayee.Text = oClaimOpen.ClaimPeril(iPeril).Payment.UltimatePayee
                    End If
                    If oClaimOpen.ClaimPeril(iPeril).ClaimPayment.Count > 0 Then
                        If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(0) IsNot Nothing AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimPayment(0).IsExGratia = True Then
                            chkExGratia.Checked = True
                        End If
                    End If
                    'If oClaimOpen.ClaimPeril(iPeril).ClaimPayment.Count > 0 Then
                    '    If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(oClaimOpen.ClaimPeril(iPeril).ClaimPayment.Count - 1).IsExGratia = True Then
                    '        chkExGratia.Checked = True
                    '    End If
                    'End If
                    If Session(CNMode) = Mode.ViewClaim OrElse
                        Session(CNMode) = Mode.SalvageClaim OrElse
                        Session(CNMode) = Mode.TPRecovery Then
                        Dim oSalvageAndTPRecoveryReservesExcludeTax As NexusProvider.OptionTypeSetting
                        oSalvageAndTPRecoveryReservesExcludeTax = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5067)
                        hSalvageAndTPRecoveryReservesExcludeTax.Value = oSalvageAndTPRecoveryReservesExcludeTax.OptionValue
                    End If

                    If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim _
                    Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment _
                    Or CType(Session(CNMode), Mode) = Mode.Authorise _
                    Or CType(Session(CNMode), Mode) = Mode.DeclinePayment _
                    Or CType(Session(CNMode), Mode) = Mode.Recommend Then

                        Dim sOption As String
                        sOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                        If String.IsNullOrEmpty(sOption) Then
                            hIsGrossClaimPaymentAmount.Value = "0"
                        Else
                            hIsGrossClaimPaymentAmount.Value = sOption
                        End If
                        sOption = String.Empty
                        sOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, Nothing, NexusProvider.RiskTypeOptions.ClaimsIsPostTaxes, Nothing, oClaimOpen.RiskType)
                        If String.IsNullOrEmpty(sOption) Then
                            hClaimsIsPostTaxes.Value = "0"
                        Else
                            hClaimsIsPostTaxes.Value = sOption
                        End If
                        chkExGratia.Visible = True
                        lblExGratia.Visible = True
                        dvExGratia.Visible = True
                        'Check the new system option by calling oWebService.GetOptionSetting
                        oExGratiaOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5114)

                        If (oExGratiaOptionSettings Is Nothing Or String.IsNullOrEmpty(oExGratiaOptionSettings.OptionValue) = True) Then
                            chkExGratia.Enabled = False
                        End If

                        'Clean the Value if exist in "Claim reserve"
                        If Session(CNEnablePayClaim) Is Nothing Then
                            For iPerilCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                                If oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve IsNot Nothing AndAlso oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve.Count > 0 Then
                                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve.Count - 1
                                        oClaimOpen.ClaimPeril(iPerilCount).ClaimReserve.Remove(0)
                                    Next
                                End If
                            Next
                            'Populating the reserve item for each peril
                            PopulateReserveItem()

                            'Populating the Payment
                            If Not String.IsNullOrEmpty(oClaimOpen.ClaimVersionDescription) _
                               AndAlso oClaimOpen.ClaimVersionDescription.Trim().IndexOf("Recovery") <> -1 Then
                                PopulateViewRecoveryClaim(iPeril, oClaimOpen)
                            Else
                                PopulatePayClaim(iPeril, oClaimOpen)
                                If m_sIsPaymentsReadOnly = "1" Then
                                    PopulateScriptThisPayment(iPeril, oClaimOpen)
                                    DisplayAccountTypeInformation()

                                Else
                                    'Populate Party Bank Details
                                    DisplayAccountTypeInformation()
                                End If
                            End If

                            'Populate Party Bank Details
                            DisplayAccountTypeInformation()
                        ElseIf Session(CNMode) = Mode.PayClaim And m_sIsPaymentsReadOnly = "0" Then
                            'populate the grid and this payment with selecetd values
                            Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
                            Cache.Remove(ViewState(CNClaimPerilReservePaymentCollection))
                            PopulatePayClaim(PerilIndex, oClaimOpen)
                            PopulateThisPayment(PerilIndex, oClaimOpen)
                        End If

                        rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimPayable")
                        ltPageHeading.Text = GetLocalResourceObject("lbl_PaymentDetails")
                        ltThisPayment.Text = GetLocalResourceObject("lt_ThisPayment")

                        'Hide "Insurer" option in case of Payclaim
                        rblPayee.Items(4).Enabled = False

                        If Session(CNMode) = Mode.PayClaim And m_sIsPaymentsReadOnly = "0" Then

                            'Fill payee Details
                            If oClaimOpen.ClaimPeril(iPeril).Payment.Payee IsNot Nothing Then
                                Address.Address1 = oClaimOpen.ClaimPeril(iPeril).Payment.Payee.Address.Address1
                                Address.Address2 = oClaimOpen.ClaimPeril(iPeril).Payment.Payee.Address.Address2
                                Address.Address3 = oClaimOpen.ClaimPeril(iPeril).Payment.Payee.Address.Address3
                                Address.Address4 = oClaimOpen.ClaimPeril(iPeril).Payment.Payee.Address.Address4
                                Address.CountryCode = oClaimOpen.ClaimPeril(iPeril).Payment.Payee.Address.CountryCode
                                Address.Postcode = oClaimOpen.ClaimPeril(iPeril).Payment.Payee.Address.PostCode
                            End If
                        End If

                    ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        'WE ARE NOT TAKING IN CONSIDERATION CHANGES DONE FOR WPR101112 FOR THIS MODE
                        m_sIsPaymentsReadOnly = "0"
                        ltThisPayment.Text = GetLocalResourceObject("gvPaymentDetails_Header")
                        rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimReceivable")
                        ltThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                        lblClaimInformation.Text = GetLocalResourceObject("lt_ReciptClaimInformation")
                        ltPayeeName.Text = GetLocalResourceObject("lbl_PayerName")


                        'Retreival of Description based on code (TypeCode)
                        For iCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                            For jCount As Integer = 0 To oClaimOpen.ClaimPeril(iCount).SalvageRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iCount).SalvageRecovery(jCount).Description = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oClaimOpen.ClaimPeril(iCount).SalvageRecovery(jCount).TypeCode, "recovery_type")
                            Next
                            For jCount As Integer = 0 To oClaimOpen.ClaimPeril(iCount).TPRecovery.Count - 1
                                oClaimOpen.ClaimPeril(iCount).TPRecovery(jCount).Description = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oClaimOpen.ClaimPeril(iCount).TPRecovery(jCount).TypeCode, "recovery_type")
                            Next
                        Next

                        'Fill Client Details
                        If oClaimOpen.Client IsNot Nothing Then
                            '    Address.Address1 = oClaimOpen.Client.Address.Address1
                            '    Address.Address2 = oClaimOpen.Client.Address.Address2
                            '    Address.Address3 = oClaimOpen.Client.Address.Address3
                            '    Address.Address4 = oClaimOpen.Client.Address.Address4
                            Address.CountryCode = oClaimOpen.Client.Address.CountryCode
                            '    Address.Postcode = oClaimOpen.Client.Address.PostCode
                        End If

                        'Populating the reserve item for each peril
                        If Session(CNMode) = Mode.SalvageClaim Then

                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                Dim oClaimReceiptItemType As New NexusProvider.BaseClaimRecoveryReceiptType
                                oClaimOpen.ClaimPeril(iPeril).Receipt.ReceiptItem.Add(oClaimReceiptItemType)
                            Next
                        ElseIf Session(CNMode) = Mode.TPRecovery Then

                            For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                Dim oClaimReceiptItemType As New NexusProvider.BaseClaimRecoveryReceiptType
                                oClaimOpen.ClaimPeril(iPeril).Receipt.ReceiptItem.Add(oClaimReceiptItemType)
                            Next
                        End If

                        'Populating the Salvage/TPRecovery
                        PopulateSalvageClaim(iPeril, oClaimOpen)

                        rblPayee.Items(4).Enabled = True
                    ElseIf Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                        DisableControls(Me)
                    End If

                    'Set the Payee if Already Selected
                    If Session(CNMode) = Mode.SalvageClaim Then
                        For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                            If oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE" Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMRECEIVABLE"

                            ElseIf oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).PartyReceiptCode
                                btnParty.Enabled = True

                            ElseIf oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oQuote.AgentCode

                            ElseIf oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName

                            End If
                        Next
                        rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimReceivable")
                        ltThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                    ElseIf Session(CNMode) = Mode.TPRecovery Then
                        For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                            If oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode = "CLMRECEIVABLE" Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMRECEIVABLE"

                            ElseIf oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).PartyReceiptCode
                                btnParty.Enabled = True

                            ElseIf oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oQuote.AgentCode

                            ElseIf oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName

                            End If
                        Next
                    Else
                        If oClaimOpen.ClaimPeril(iPeril).Payment IsNot Nothing Then
                            If oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode IsNot Nothing AndAlso
                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE.ToString Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMPAYABLE"
                            ElseIf oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode
                                hPartyKey.Value = oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey
                                btnParty.Enabled = True

                            ElseIf oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oQuote.AgentCode

                            ElseIf oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName
                                hPartyKey.Value = oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey
                            End If
                        End If
                        'payment can not edited once received/paid
                        If (Session(CNLockPaymentGrid) IsNot Nothing AndAlso CType(Session(CNLockPaymentGrid), Boolean) = True) _
                        Or (Session(CNEnablePayClaim) IsNot Nothing AndAlso Session(CNEnablePayClaim) = True) Then
                            gvPaymentDetails.Columns(9).Visible = False
                            DisableControls(Me)
                        End If
                    End If
                    If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then

                        Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))

                        If (oClaimOpen.ClaimVersionDescription IsNot Nothing _
                             AndAlso oClaimOpen.ClaimVersionDescription.Trim().IndexOf("Recovery") <> -1) Then
                            'PopulateViewRecoveryClaim(iPeril, oClaimOpen)
                            rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimReceivable")
                            ltThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                            DisableControls(Me)
                        Else

                            PopulateThisPayment(PerilIndex, oClaimOpen)
                            PopulatePayClaim(iPeril, oClaimOpen)
                            DisableControls(Me)
                        End If
                        DisableControls(Me)
                        gvPaymentDetails.Enabled = True
                        DisplayAccountTypeInformation()
                        txtThisReference.Attributes.Add("readonly", "readonly")
                    End If
                Else
                    Response.Redirect("~/claims/FindClaim.aspx")
                End If
            ElseIf Request("__EVENTARGUMENT") = "PaymentUpdation" Then
                'Populate Party Bank Details
                DisplayAccountTypeInformation()
                ddlAccountType.Enabled = True
                GISLookup_MediaType.Enabled = True
                txtMediaRef.Attributes.Remove("readonly")
                txtPayeeName.Attributes.Remove("readonly")
                txtBankName.Attributes.Remove("readonly")
                txtBankCode.Attributes.Remove("readonly")
                txtBankAccNumber.Attributes.Remove("readonly")
                txtOurRef.Attributes.Remove("readonly")
                txtComments.Attributes.Remove("readonly")

                ' Check if recovery rows were auto-selected and maintain disabled state
                If (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) AndAlso
                   ViewState("RecoveryAutoSelected") IsNot Nothing AndAlso CBool(ViewState("RecoveryAutoSelected")) Then
                    ' Re-disable the payee controls after PaymentDetails modal return
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "MaintainRecoveryControlsDisabled",
                        "$('input[name=""" & rblPayee.UniqueID & """]').prop('disabled', true); " &
                        "$('#" & btnParty.ClientID & "').prop('disabled', true); " &
                        "$('#" & btnClientParty.ClientID & "').prop('disabled', true);", True)
                End If
            Else
                gvPaymentDetails.Enabled = True
                'gvTaxesonThisReceipt.DataSource = oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem
                'gvTaxesonThisReceipt.DataBind()
                txtMediaRef.Attributes.Remove("readonly")
                txtOurRef.Attributes.Remove("readonly")
                txtComments.Attributes.Remove("readonly")

                If CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                    GISLookup_MediaType.Enabled = False
                    txtPayeeName.Enabled = False
                    txtBankAccNumber.Enabled = False
                    txtMediaRef.Enabled = False
                    txtOurRef.Enabled = False
                End If
            End If
            If rblPayee.SelectedValue = "1" AndAlso Not String.IsNullOrEmpty(sPartyId) AndAlso sPartyId <> "0" Then
                oSelectedBaseParty = oWebService.GetParty(sPartyId)
                txtParty.Text = If(oSelectedBaseParty.ShortName IsNot Nothing, oSelectedBaseParty.ShortName, txtParty.Text)
            End If

            ' Re-apply edit button and payee control disable state after every postback.
            ' Only applies to recovery modes (SalvageClaim/TPRecovery) � never PayClaim.
            If (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) Then
                Dim iSelectedRecoveryRow As Integer = -1
                If Integer.TryParse(hfSelectedRecoveryRow.Value, iSelectedRecoveryRow) AndAlso iSelectedRecoveryRow >= 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "SalvageEditDisablePreRender",
                        "fnSalvageEditClick(" & iSelectedRecoveryRow & ", '" & gvSalvageDetails.ClientID & "');" &
                        "$('input[name=""" & rblPayee.UniqueID & """]').prop('disabled', true);" &
                        "$('#" & btnParty.ClientID & "').prop('disabled', true);" &
                        "$('#" & btnClientParty.ClientID & "').prop('disabled', true);", True)
                End If
            End If
        End Sub

#Region " GridView Events "

        Protected Sub gvSalvageDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSalvageDetails.Load
            If gvSalvageDetails.PageCount = 1 Then
                gvSalvageDetails.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' Salvage grid page index change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSalvageDetails.PageIndexChanging
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            gvSalvageDetails.PageIndex = e.NewPageIndex
            PopulateSalvageClaim(iPeril, oClaimOpen)
        End Sub
        ''' <summary>
        ''' Salvage grid Row command
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSalvageDetails.RowCommand
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            If e.CommandName = "Lock" Then
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                ViewState("IsLockPayment") = 1
                If Session(CNMode) = Mode.SalvageClaim Then
                    oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(e.CommandArgument).IsLocked = True
                ElseIf Session(CNMode) = Mode.TPRecovery Then
                    oClaimOpen.ClaimPeril(iPeril).TPRecovery(e.CommandArgument).IsLocked = True
                End If
                PopulateSalvageClaim(iPeril, oClaimOpen)
            ElseIf e.CommandName = "Edit" Then
                ' Auto-fill party fields from the clicked recovery row (SalvageClaim/TPRecovery only)
                If Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                    Dim iRowIndex As Integer = CInt(e.CommandArgument)
                    Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    Dim oRecoveryRow As NexusProvider.PerilRecovery = Nothing
                    If Session(CNMode) = Mode.SalvageClaim Then
                        If iRowIndex < oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count Then
                            oRecoveryRow = oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iRowIndex)
                        End If
                    ElseIf Session(CNMode) = Mode.TPRecovery Then
                        If iRowIndex < oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count Then
                            oRecoveryRow = oClaimOpen.ClaimPeril(iPeril).TPRecovery(iRowIndex)
                        End If
                    End If
                    If oRecoveryRow IsNot Nothing Then
                        ' Use RecoveryPartyTypeCode: AG=Agent(2), CL=Client(3), IN=Insurer(4), OT=Other Party(1)
                        Select Case oRecoveryRow.RecoveryPartyTypeCode.ToUpper().Trim()
                            Case "AG"
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oRecoveryRow.PartyShortName
                                hPartyKey.Value = oRecoveryRow.RecoveryPartyKey
                            Case "CL"
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oRecoveryRow.PartyShortName
                                hPartyKey.Value = oRecoveryRow.RecoveryPartyKey
                            Case "IN"
                                rblPayee.SelectedValue = "4"
                                txtParty.Text = oRecoveryRow.PartyShortName
                                hPartyKey.Value = oRecoveryRow.RecoveryPartyKey
                            Case "OT"
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = If(Not String.IsNullOrEmpty(oRecoveryRow.PartyShortName), oRecoveryRow.PartyShortName, oRecoveryRow.PartyReceiptCode)
                                hPartyKey.Value = oRecoveryRow.RecoveryPartyKey
                            Case Else
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMRECEIVABLE"
                                hPartyKey.Value = 0
                        End Select
                        gvSalvageDetails.Columns(9).Visible = True
                        DisplayAccountTypeInformation()
                        ' Persist selected row � JS in OnEndRequest will re-apply the disable
                        hfSelectedRecoveryRow.Value = iRowIndex
                    End If
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sOption As String
                sOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsAdvancedTaxScriptEnabled, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If Not String.IsNullOrEmpty(sOption) AndAlso sOption = "1" Then
                    SavePaymentReceiptDetailsToSessionClaims(iPeril, "", sOption)
                End If
            End If
        End Sub
        ''' <summary>
        ''' Salvage grid row data bound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSalvageDetails.RowDataBound
            'Dim hypEditPayment As LinkButton = CType(e.Row.FindControl("hypEditPayment"), LinkButton)
            If e.Row.RowType = DataControlRowType.DataRow Then
                'If CType(e.Row.DataItem, NexusProvider.PerilRecovery).ClaimPerilId <> nClaimPerilId Then
                '    e.Row.Visible = False
                'End If
                'If HttpContext.Current.Session.IsCookieless Then
                '    hypEditPayment.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700' , null);return false;"
                'Else
                '    hypEditPayment.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700' , null);return false;"
                'End If

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("BaseRecoveryKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PerilRecovery).BaseRecoveryKey)

                Dim oRecovery As NexusProvider.PerilRecovery = CType(e.Row.DataItem, NexusProvider.PerilRecovery)
                Dim dBalance As Decimal = oRecovery.InitialRecovery - oRecovery.ReceiptedAmount - oRecovery.ThisReceiptINCLTax
                Dim lblBalance As Label = CType(e.Row.FindControl("lblBalance"), Label)
                If lblBalance IsNot Nothing Then
                    Dim sFormatString As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                    lblBalance.Text = String.Format(sFormatString, dBalance)
                End If
                ' Store the selected row index when Edit is clicked; disable of other buttons
                ' only happens on return from modal (OK pressed via PaymentUpdation postback).
                Dim oSalvageEditBtn As LinkButton = CType(e.Row.FindControl("hypEditPayment"), LinkButton)
                If oSalvageEditBtn IsNot Nothing Then
                    oSalvageEditBtn.OnClientClick = "document.getElementById('" & hfSelectedRecoveryRow.ClientID & "').value='" & e.Row.DataItemIndex & "';return true;"
                End If

                ' Disable controls for recovery rows after auto-selection
                If (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) AndAlso
                   Not String.IsNullOrEmpty(oRecovery.RecoveryPartyTypeCode) AndAlso
                   Not String.IsNullOrEmpty(oRecovery.PartyShortName) Then
                    ' This recovery row has been auto-filled, disable payee selection controls
                    ' Store the auto-selected state in ViewState for persistence
                    ViewState("RecoveryAutoSelected") = True
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "DisableRecoveryControls_" & e.Row.DataItemIndex,
                        "$('input[name=""" & rblPayee.UniqueID & """]').prop('disabled', true); " &
                        "$('#" & btnParty.ClientID & "').prop('disabled', true); " &
                        "$('#" & btnClientParty.ClientID & "').prop('disabled', true);", True)
                End If
            End If
        End Sub

        Protected Sub gvPaymentDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymentDetails.Load
            If gvPaymentDetails.PageCount = 1 Then
                gvPaymentDetails.AllowPaging = False
            End If
        End Sub
        ''' <summary>
        ''' On change of page index
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPaymentDetails.PageIndexChanging
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            DisplayAccountTypeInformation()
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            gvPaymentDetails.PageIndex = e.NewPageIndex
            PopulatePayClaim(iPeril, oClaimOpen)
        End Sub
        ''' <summary>
        ''' On selection of different row command
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPaymentDetails.RowCommand
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            If e.CommandName = "Lock" Then
                ViewState("IsLockPayment") = 1
                oClaimOpen.ClaimPeril(iPeril).ClaimReserve(e.CommandArgument).IsLocked = True
                PopulatePayClaim(iPeril, oClaimOpen)
            ElseIf e.CommandName = "Edit" Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sOption As String

                sOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsAdvancedTaxScriptEnabled, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If Not String.IsNullOrEmpty(sOption) AndAlso sOption = "1" Then
                    Dim oGridView As GridView
                    oGridView = CType(sender, GridView)
                    Dim nReserveKey As Integer = CInt(e.CommandArgument)
                    Dim s As String '= oGridView.Rows(oGridView.SelectedIndex).Cells(10).Text
                    s = "false"
                    SavePaymentReceiptDetailsToSessionClaims(iPeril, s, sOption, nReserveKey)
                End If
            End If
        End Sub

        Private Sub SavePaymentReceiptDetailsToSessionClaims(ByVal nPeril As Integer, ByVal s As String, ByVal sOption As String, Optional ByVal nReserveKey As Integer = 0)
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

            With oClaimOpen.ClaimPeril(nPeril)
                If Session(CNMode) = Mode.PayClaim Then
                    Dim oPaymentAdvancedTaxDetails As New NexusProvider.PaymentAdvancedTaxDetails
                    With oPaymentAdvancedTaxDetails

                        .PaymentTo = ddlPayment_To.SelectedValue
                        .InsuredDomiciled = chk_DomiciledITA.Checked
                        If Not String.IsNullOrEmpty(txtPercentageITA.Text) Then
                            .InsuredPercentage = Convert.ToDecimal(txtPercentageITA.Text)
                        End If
                        .InsuranceTaxNumber = txtTaxnoITA.Text
                        .PayeeDomiciled = chk_DomicilePTA.Checked
                        If Not String.IsNullOrEmpty(txtPercentagePTA.Text) Then
                            .PayeePercentage = Convert.ToDecimal(txtPercentagePTA.Text)
                        End If
                        .PayeeTaxNumber = txtTaxNoPTA.Text
                        .IsTaxExempt = chk_Taxexempt.Checked
                        .IsWHTExempt = chk_WHTExempt.Checked
                        .PayeeName = txtParty.Text.Trim
                        If s.ToUpper = "TRUE" Then
                            .IsExcess = True
                        Else
                            .IsExcess = False
                        End If
                        If sOption = "1" Then
                            .AdvancedTaxScriptOptionOn = True
                        End If
                        .ReserveKey = nReserveKey
                    End With

                    .Payment.PaymentAdvancedTaxDetails = oPaymentAdvancedTaxDetails
                End If
                If Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                    Dim oReceiptAdvancedTaxDetails As New NexusProvider.ClaimReceiptAdvancedTaxDetails
                    With oReceiptAdvancedTaxDetails

                        .PayeeName = txtParty.Text.Trim

                        If sOption = "1" Then
                            .AdvancedTaxScriptOptionOn = True
                        End If

                        .InsuredDomiciled = chk_DomicileITA.Checked
                        If Not String.IsNullOrEmpty(txtPercentage_ITA.Text) Then
                            .InsuredPercentage = Convert.ToDecimal(txtPercentage_ITA.Text)
                        End If

                        .InsuredTaxNumber = txtTaxNo_ITA.Text
                        .IsTaxExempt = chk_TaxexemptRTS.Checked
                        If Not String.IsNullOrEmpty(txtPercentageRTS.Text) Then
                            .ReceivableTaxPercentage = Convert.ToDecimal(txtPercentageRTS.Text)
                        End If

                    End With
                    .Receipt.AdvancedTaxDetails = oReceiptAdvancedTaxDetails

                End If
                Session.Item(CNClaim) = oClaimOpen
            End With

        End Sub

        ''' <summary>
        ''' On Payment grid RowDataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPaymentDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim sUrl As String = Nothing

                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = "' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PerilIndex=" & Request.QueryString("PerilIndex") & "&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700'"
                Else
                    sUrl = "' " & AppSettings("WebRoot") & "Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PerilIndex=" & Request.QueryString("PerilIndex") & "&PaymentIndex=" & e.Row.DataItemIndex & "&TB_iframe=true&height=500&width=700'"
                End If

                'Check whether Excess Reserve is accepted first or not
                'if it is not accepted first then it has to ask the Reset Confirmation
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.ClaimPerilReservePaymentType).BaseReserveKey)

                Dim bFirstElement As Boolean = True
                If CType(e.Row.DataItem, NexusProvider.ClaimPerilReservePaymentType).IsExcess = True Then
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimReserve.Count - 1
                        If oClaimOpen.ClaimPeril(iPeril).ClaimReserve(iCount).IsExcess = False _
                        AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimReserve(iCount).PayQueue > 0 Then
                            bFirstElement = False
                        End If
                    Next
                End If

                e.Row.Cells(3).Text = String.Format("{0:N}", Convert.ToDecimal(CType(e.Row.DataItem, NexusProvider.ClaimPerilReservePaymentType).PaidToDate))
                e.Row.Cells(4).Text = String.Format("{0:N}", Convert.ToDecimal(CType(e.Row.DataItem, NexusProvider.ClaimPerilReservePaymentType).PaidToDateTax))

                If Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.ViewClaimPayment Then
                    e.Row.Cells(3).Text = String.Format("{0:N}", Convert.ToDecimal(e.Row.Cells(3).Text) - (Convert.ToDecimal(e.Row.Cells(6).Text) - Convert.ToDecimal(e.Row.Cells(7).Text)))
                    e.Row.Cells(4).Text = String.Format("{0:N}", Convert.ToDecimal(e.Row.Cells(4).Text) - (Convert.ToDecimal(e.Row.Cells(7).Text)))
                End If
                If Not String.IsNullOrEmpty(e.Row.Cells(3).Text) AndAlso Convert.ToDecimal(e.Row.Cells(3).Text) = 0 Then
                    e.Row.Cells(4).Text = String.Format("{0:N}", Convert.ToDecimal(e.Row.Cells(3).Text))
                End If
                If bFirstElement = False Then
                    'Reset confirmation
                    CType(e.Row.FindControl("hypEditPayment"), LinkButton).OnClientClick = "javascript:setConfirmation('" & GetLocalResourceObject("msg_ResetConfirm").ToString() & "',1," & sUrl & ");return false;"
                Else
                    CType(e.Row.FindControl("hypEditPayment"), LinkButton).OnClientClick = "javascript:setConfirmation('" & GetLocalResourceObject("msg_ResetConfirm").ToString() & "',0," & sUrl & ");return false;"
                End If
                ''''''''
            End If
        End Sub

        Protected Sub gvPaymentDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvPaymentDetails.RowEditing
            Dim sUrl As String = Nothing
            Dim nNewEditIndex As Integer
            nNewEditIndex = gvPaymentDetails.PageSize * gvPaymentDetails.PageIndex + e.NewEditIndex
            If HttpContext.Current.Session.IsCookieless Then
                sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" &
            updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & nNewEditIndex & "&TB_iframe=true&height=500&width=700"
            Else
                sUrl = AppSettings("WebRoot") & "/Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" &
            updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & nNewEditIndex & "&TB_iframe=true&height=500&width=700"
            End If

            e.NewEditIndex = -1
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "PaymentDetails", "tb_show(null , '" & sUrl & "' , null);", True)
        End Sub

        ''' <summary>
        ''' On Payment grid Sorting
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvPaymentDetails_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPaymentDetails.Sorting
            'sort the Payment details according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Cache.Item(ViewState(CNClaimPerilReservePaymentCollection)), NexusProvider.ClaimPerilReservePaymentTypeCollection)

            oClaimReserve.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oClaimReserve.SortingOrder = _sortDirection
            oClaimReserve.Sort()
            CType(sender, GridView).DataSource = oClaimReserve
            CType(sender, GridView).Columns(10).Visible = True
            CType(sender, GridView).DataBind()
            CType(sender, GridView).Columns(10).Visible = False
        End Sub

        Protected Sub gvSalvageDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvSalvageDetails.RowEditing
            Dim sUrl As String = Nothing
            Dim nNewEditIndex As Integer
            DisplayAccountTypeInformation()
            nNewEditIndex = gvSalvageDetails.PageSize * gvSalvageDetails.PageIndex + e.NewEditIndex
            If HttpContext.Current.Session.IsCookieless Then
                sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" _
                            & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & nNewEditIndex & "&TB_iframe=true&height=500&width=700"
            Else
                sUrl = AppSettings("WebRoot") & "/Modal/PaymentDetails.aspx?DomiciledTax=" & bdomiciledTax.ToString & "&PostbackTo=" _
                & updThisPayment.ClientID.ToString & "&modal=true&KeepThis=true&PaymentIndex=" & nNewEditIndex & "&TB_iframe=true&height=500&width=700"
            End If

            e.NewEditIndex = -1
            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "PaymentDetails", "tb_show(null , '" & sUrl & "' , null);", True)
        End Sub

        ''' <summary>
        ''' SalvageDetails grid Sorting
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvSalvageDetails_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSalvageDetails.Sorting
            'sort the Salvage Details according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oClaimSalvage As NexusProvider.PerilRecoveryCollection = CType(Cache.Item(ViewState(CNClaimPerilRecoveryCollection)), NexusProvider.PerilRecoveryCollection)

            oClaimSalvage.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oClaimSalvage.SortingOrder = _sortDirection
            oClaimSalvage.Sort()
            CType(sender, GridView).DataSource = oClaimSalvage
            CType(sender, GridView).DataBind()
        End Sub
        ''' <summary>
        ''' Radio button Selected Index Change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rblPayee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblPayee.SelectedIndexChanged
            Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = Nothing
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim sOption As String = HidATSoption.Value

            If ddlPayment_To.SelectedValue IsNot Nothing AndAlso ddlPayment_To.SelectedValue <> "SUPPLIER" Then
                chk_DomiciledITA.Enabled = True
                txtPercentageITA.Enabled = True
                txtTaxnoITA.Enabled = True
                chk_DomicilePTA.Checked = False
                txtPercentageITA.Text = "0.00"
                txtTaxnoITA.Text = ""
                txtPercentagePTA.Text = "0.00"
                txtTaxNoPTA.Text = ""
            End If

            If ViewState(CNClaimPerilReservePaymentCollection) IsNot Nothing Then
                Cache.Remove(ViewState(CNClaimPerilReservePaymentCollection))
            End If

            Select Case rblPayee.SelectedValue
                Case "0"
                    txtParty.Text = String.Empty
                    If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                        txtParty.Text = "CLMRECEIVABLE"
                        Session(CNPayee) = "CLMRECEIVABLE"
                    ElseIf Session(CNMode) = Mode.PayClaim Then
                        txtParty.Text = "CLMPAYABLE"
                        Session(CNPayee) = "CLMPAYABLE"
                    End If
                    hPartyKey.Value = 0
                    btnParty.Enabled = False
                    btnClientParty.Enabled = False
                    txtMediaRef.ReadOnly = False
                    txtPayeeName.ReadOnly = False
                    txtBankName.ReadOnly = False
                    txtBankCode.ReadOnly = False
                    txtBankAccNumber.ReadOnly = False
                    txtOurRef.ReadOnly = False
                    txtComments.ReadOnly = False
                    txtBIC.ReadOnly = False
                    txtIBAN.ReadOnly = False
                    '  ReqPartyKey.IsValid = True
                    '  ReqPartyKey.Enabled = False
                    gvPaymentDetails.Columns(9).Visible = True
                    gvSalvageDetails.Columns(9).Visible = True
                    If sOption = "1" AndAlso Session(CNMode) = Mode.PayClaim Then
                        chk_DomiciledITA.Checked = True
                        txtPercentageITA.Text = "0.00"
                        lbl_TaxnoITA.Font.Bold = True
                        lbl_PercentageITA.Font.Bold = True
                        chk_DomicilePTA.Enabled = False
                        txtPercentagePTA.Enabled = False
                        txtTaxNoPTA.Enabled = False
                    End If
                    txtAccountType.Text = ""

                    CustVldValidateAccountNumber.ValidateEmptyText = False
                    CustVldValidateAccountNumber.Enabled = True
                Case "1"
                    txtParty.Text = String.Empty
                    hPartyKey.Value = 0
                    btnParty.Enabled = True
                    btnClientParty.Enabled = True

                    If HttpContext.Current.Session.IsCookieless Then
                        btnParty.OnClientClick = "ActiveTabValueSet();tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        btnParty.OnClientClick = "ActiveTabValueSet();tb_show(null , '" & AppSettings("WebRoot") & "Modal/FindOtherParty.aspx?modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If

                    If HttpContext.Current.Session.IsCookieless Then
                        btnClientParty.OnClientClick = "ActiveTabValueSet();tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
                    Else
                        btnClientParty.OnClientClick = "ActiveTabValueSet();tb_show(null ,'" & AppSettings("WebRoot") & "secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
                    End If

                    ' ReqPartyKey.Enabled = True
                    gvPaymentDetails.Columns(9).Visible = False
                    gvSalvageDetails.Columns(9).Visible = False
                    If sOption = "1" Then
                        chk_DomiciledITA.Checked = True
                    End If
                    CustVldValidateAccountNumber.Enabled = False
                Case "2"
                    If oClaimOpen.Insurer IsNot Nothing Then
                        txtParty.Text = oClaimOpen.Insurer.ContactName
                        txtParty.Text = oQuote.AgentDesc
                    End If
                    btnParty.Enabled = False
                    btnClientParty.Enabled = False
                    'ReqPartyKey.IsValid = True
                    'ReqPartyKey.Enabled = False
                    gvPaymentDetails.Columns(9).Visible = True
                    gvSalvageDetails.Columns(9).Visible = True
                    If sOption = "1" Then
                        chk_DomiciledITA.Checked = True
                    End If
                    CustVldValidateAccountNumber.Enabled = False
                Case "3"
                    txtParty.Text = oQuote.InsuredName
                    btnParty.Enabled = False
                    btnClientParty.Enabled = False
                    'ReqPartyKey.IsValid = True
                    'ReqPartyKey.Enabled = False
                    gvPaymentDetails.Columns(9).Visible = True
                    gvSalvageDetails.Columns(9).Visible = True
                    If sOption = "1" Then
                        chk_DomiciledITA.Checked = True
                    End If
                    CustVldValidateAccountNumber.Enabled = False
                Case "4"
                    txtParty.Text = String.Empty
                    hPartyKey.Value = 0
                    btnParty.Enabled = True
                    btnClientParty.Enabled = False

                    If HttpContext.Current.Session.IsCookieless Then
                        btnParty.OnClientClick = "ActiveTabValueSet();tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                    Else
                        btnParty.OnClientClick = "ActiveTabValueSet();tb_show(null , ' " & AppSettings("WebRoot") & "Modal/FindReinsurer.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700' , null);return false;"
                    End If

                    'ReqPartyKey.Enabled = True
                    gvPaymentDetails.Columns(9).Visible = False
                    gvSalvageDetails.Columns(9).Visible = False
                    If sOption = "1" Then
                        chk_DomiciledITA.Checked = True
                    End If
            End Select

            If ddlPayment_To.SelectedValue IsNot Nothing AndAlso ddlPayment_To.SelectedValue = "SUPPLIER" Then
                chk_DomiciledITA.Enabled = False
                chk_DomiciledITA.Checked = False
                txtPercentageITA.Enabled = False
                'txtPercentageITA.Text = "0.00"
                'txtTaxnoITA.Text = ""
                txtTaxnoITA.Enabled = False

                chk_DomicilePTA.Enabled = True
                chk_DomicilePTA.Checked = True
                txtPercentagePTA.Enabled = True
                txtTaxNoPTA.Enabled = True
                'txtPercentagePTA.Text = "0.00"
                'txtTaxNoPTA.Text = ""
            End If

            gvSalvageDetails.Enabled = True

            'Repopulate payment detail based on payee Type
            DisplayAccountTypeInformation()

            If Session(CNMode) = Mode.PayClaim Then
                'Refresh the tax group and amounts if user change the payee options
                If oClaimOpen IsNot Nothing Then
                    For iInnerCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilIndex).Payment.ClaimPaymentItem.Count - 1
                        CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilIndex).Payment.ClaimPaymentItem(iInnerCount).TaxGroupCode = ""
                        CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilIndex).Payment.ClaimPaymentItem(iInnerCount).TaxAmount = 0
                        CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilIndex).Payment.ClaimPaymentItem(iInnerCount).PaymentAmount = 0
                    Next

                    For iInnerCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilIndex).ClaimReserve.Count - 1
                        CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilIndex).ClaimReserve(iInnerCount).ThisPaymentTax = 0
                        CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilIndex).ClaimReserve(iInnerCount).ThisPaymentINCLTax = 0
                    Next
                End If
                Dim oClaimPerilReservePaymentCollection As NexusProvider.ClaimPerilReservePaymentTypeCollection =
                    CType(Cache.Item(ViewState(CNClaimPerilReservePaymentCollection)), NexusProvider.ClaimPerilReservePaymentTypeCollection)
                If ViewState(CNClaimPerilReservePaymentCollection) Is Nothing OrElse Cache.Item(ViewState(CNClaimPerilReservePaymentCollection)) Is Nothing Then
                    Dim iPeril As Integer
                    iPeril = CInt(Session(CNClaimPerilIndex))
                    oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    PopulatePayClaim(iPeril, oClaimOpen)
                End If
                oClaimPerilReservePaymentCollection = CType(Cache.Item(ViewState(CNClaimPerilReservePaymentCollection)), NexusProvider.ClaimPerilReservePaymentTypeCollection)
                If oClaimPerilReservePaymentCollection IsNot Nothing Then
                    For iCount As Integer = 0 To oClaimPerilReservePaymentCollection.Count - 1
                        oClaimPerilReservePaymentCollection(iCount).ThisPaymentTax = 0
                        oClaimPerilReservePaymentCollection(iCount).ThisPaymentINCLTax = 0
                        oClaimPerilReservePaymentCollection(iCount).CostToClaim = 0
                        oClaimPerilReservePaymentCollection(iCount).CurrentReserve = oClaimPerilReservePaymentCollection(iCount).OldReserve
                    Next
                End If
                'Repopulate payment detail grid with domiciled_for_tax information of selected payee
                PopulatePayClaim(PerilIndex, oClaimOpen)
            ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                'Refresh the tax group and amounts if user change the payee options
                If Session(CNMode) = Mode.SalvageClaim Then
                    If oClaimOpen IsNot Nothing Then
                        For iInnerCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery.Count - 1
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).LossThisNet = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).LossThisReceiptINCLTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).LossThisReceiptTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).LossAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).ThisNet = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).ThisReceiptINCLTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).ThisReceiptTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).SalvageRecovery(iInnerCount).PartyReceiptCode = ""
                        Next
                        For iInnerCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem.Count - 1
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).ThisReceiptNetAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).ThisReceiptTaxAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).TotalReceiptAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).TaxCode = ""
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).ThisReceiptINCLTaxAmount = 0
                        Next
                    End If
                ElseIf Session(CNMode) = Mode.TPRecovery Then
                    If oClaimOpen IsNot Nothing Then
                        For iInnerCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilIndex).TPRecovery.Count - 1
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).LossThisNet = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).LossThisReceiptINCLTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).LossThisReceiptTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).LossAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).ThisNet = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).ThisReceiptINCLTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).ThisReceiptTax = 0
                            oClaimOpen.ClaimPeril(PerilIndex).TPRecovery(iInnerCount).PartyReceiptCode = ""
                        Next
                        For iInnerCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem.Count - 1
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).ThisReceiptNetAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).ThisReceiptTaxAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).TotalReceiptAmount = 0
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).TaxCode = ""
                            oClaimOpen.ClaimPeril(PerilIndex).Receipt.ReceiptItem(iInnerCount).ThisReceiptINCLTaxAmount = 0
                        Next
                    End If
                End If
                Session(CNClaim) = oClaimOpen
                Cache.Remove(ViewState(CNClaimPerilRecoveryCollection))
                'Repopulate payment detail grid with domiciled_for_tax information of selected payee
                PopulateSalvageClaim(PerilIndex, oClaimOpen)
            End If
        End Sub
        Protected Sub gvTaxesonThisPayment_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTaxesonThisPayment.Load
            If gvTaxesonThisPayment.PageCount = 1 Then
                gvTaxesonThisPayment.AllowPaging = False
            End If
        End Sub


#End Region
#Region " Private Methods "
        ''' <summary>
        ''' It populates the payment grid
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <param name="oClaimOpen"></param>
        ''' <remarks></remarks>
        Protected Sub PopulatePayClaim(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)

            'try to get the Resrve Grid from the cache
            Dim oClaimPerilReservePaymentCollection As NexusProvider.ClaimPerilReservePaymentTypeCollection =
            CType(Cache.Item(ViewState(CNClaimPerilReservePaymentCollection)), NexusProvider.ClaimPerilReservePaymentTypeCollection)
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oTempClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            visibleGrid()

            If oClaimPerilReservePaymentCollection Is Nothing Then
                oClaimPerilReservePaymentCollection = New NexusProvider.ClaimPerilReservePaymentTypeCollection
                gvPaymentDetails.Visible = True
                For iCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count - 1
                    If iCount = iPeril Then
                        For jCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve.Count - 1
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).BaseReserveKey <> 0 Then
                                oClaimPerilReservePaymentCollection.Add(CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount))
                            End If
                        Next
                    End If
                Next
                Cache.Insert(ViewState(CNClaimPerilReservePaymentCollection), oClaimPerilReservePaymentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            gvPaymentDetails.DataSource = oClaimPerilReservePaymentCollection
            gvPaymentDetails.Columns(10).Visible = True
            gvPaymentDetails.DataBind()
            gvPaymentDetails.Columns(10).Visible = False
        End Sub
        ''' <summary> 
        ''' It populates the Salvage/TPRecovery grid
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <param name="oClaimOpen"></param>
        ''' <remarks></remarks>
        Protected Sub PopulateSalvageClaim(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)

            'try to get the Salvage and Recovery Grid from the cache
            oPerilRecoveryCollection = CType(Cache.Item(ViewState(CNClaimPerilRecoveryCollection)), NexusProvider.PerilRecoveryCollection)
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            gvSalvageDetails.Visible = True

            If oPerilRecoveryCollection Is Nothing Then
                If Session(CNMode) = Mode.SalvageClaim Then
                    'store the data in ViewState to use again for page indexing
                    oPerilRecoveryCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery
                ElseIf Session(CNMode) = Mode.TPRecovery Then
                    'store the data in ViewState to use again for page indexing
                    oPerilRecoveryCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery
                End If
                Dim oPerilRecoveryCollection_temp As New NexusProvider.PerilRecoveryCollection

                Cache.Insert(ViewState(CNClaimPerilRecoveryCollection), oPerilRecoveryCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            End If

            gvSalvageDetails.Visible = True

            If Session(CNMode) = Mode.SalvageClaim Then
                'store the data in ViewState to use again for page indexing
                gvSalvageDetails.DataSource = oPerilRecoveryCollection
                gvSalvageDetails.DataBind()
                gvSalvageDetails.Columns(9).Visible = True
            ElseIf Session(CNMode) = Mode.TPRecovery Then
                'store the data in ViewState to use again for page indexing
                gvSalvageDetails.DataSource = oPerilRecoveryCollection
                gvSalvageDetails.DataBind()
                gvSalvageDetails.Columns(9).Visible = True
            End If
            ' Re-apply disable of other edit buttons if a row was previously selected
            Dim iSelectedRow As Integer = -1
            If Integer.TryParse(hfSelectedRecoveryRow.Value, iSelectedRow) AndAlso iSelectedRow >= 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "SalvageEditDisable",
                    "fnSalvageEditClick(" & iSelectedRow & ", '" & gvSalvageDetails.ClientID & "');", True)
            End If

            ' Re-apply disabled state for auto-selected recovery controls
            If (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) AndAlso
               ViewState("RecoveryAutoSelected") IsNot Nothing AndAlso CBool(ViewState("RecoveryAutoSelected")) Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ReapplyRecoveryControlsDisabled",
                    "$('input[name=""" & rblPayee.UniqueID & """]').prop('disabled', true); " &
                    "$('#" & btnParty.ClientID & "').prop('disabled', true); " &
                    "$('#" & btnClientParty.ClientID & "').prop('disabled', true);", True)
            End If
            If HidATSoption.Value = "1" Then
                oPerilRecoveryCollection(0).ReceiptedAmount = oPerilRecoveryCollection(0).InitialRecovery
                oPerilRecoveryCollection(0).ReceiptedTaxAmount = oPerilRecoveryCollection(0).ThisReceiptTax
                oPerilRecoveryCollection(0).LossThisReceiptINCLTax = oPerilRecoveryCollection(0).ThisNet
            End If

        End Sub
        ''' <summary>
        ''' It populates the "This Payment" section
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <param name="oClaimOpen"></param>
        ''' <remarks></remarks>
        Protected Sub PopulateThisPayment(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)
            Dim dTax As Decimal
            Dim dTaxWH As Decimal
            If (Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim) AndAlso m_sIsPaymentsReadOnly = "1" Then
                PopulateScriptThisPayment(iPerilIndex, oClaimOpen)
            ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                Dim amount As Decimal
                Dim tax As Decimal

                If oClaimOpen.ClaimPeril(iPerilIndex).Receipt IsNot Nothing Then
                    'Calculation of the amount and tax
                    If oClaimOpen.ClaimPeril(iPerilIndex).Receipt.ReceiptItem IsNot Nothing Then
                        For Each oReceiptItem As NexusProvider.BaseClaimRecoveryReceiptType In oClaimOpen.ClaimPeril(iPerilIndex).Receipt.ReceiptItem
                            amount += oReceiptItem.ThisReceiptNetAmount
                            tax += oReceiptItem.ThisReceiptTaxAmount
                        Next
                    End If
                    'if tax is grater than 0
                    If tax <> 0 Then
                        Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                        Dim oClaimReceipt As NexusProvider.ClaimReceipt = Nothing
                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                        'Clear the Tax if exist
                        If oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem IsNot Nothing _
                        AndAlso oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem.Count > 0 Then
                            oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem.Clear()
                        End If

                        Select Case rblPayee.SelectedValue
                            Case "0"
                                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then

                                    With oClaimOpen.ClaimPeril(iPeril).Receipt
                                        .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE
                                        .PartyReceiptCode = "CLMRECEIVABLE"
                                    End With
                                End If

                            Case "1"
                                'Party
                                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then

                                    With oClaimOpen.ClaimPeril(iPeril).Receipt
                                        .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                        .PartyReceiptCode = txtParty.Text.Trim
                                        .PartyKey = CInt(hPartyKey.Value.Trim)
                                    End With
                                End If
                            Case "2"
                                'Agent
                                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then

                                    With oClaimOpen.ClaimPeril(iPeril).Receipt
                                        .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT
                                        .PartyReceiptCode = oQuote.AgentCode
                                        '.PartyKey = Convert.ToInt32(oQuote.Agent)
                                    End With
                                End If
                            Case "3"
                                'Client
                                'Dim oParty As NexusProvider.BaseParty = Session(CNParty)

                                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then

                                    With oClaimOpen.ClaimPeril(iPeril).Receipt
                                        .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT
                                        .PartyReceiptCode = oClaimOpen.ClientShortName
                                        '.PartyKey = oParty.Key
                                    End With
                                End If

                            Case "4"
                                'Insurer
                                If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then

                                    With oClaimOpen.ClaimPeril(iPeril).Receipt
                                        .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY
                                        .PartyReceiptCode = txtParty.Text.Trim
                                        .PartyKey = CInt(hPartyKey.Value.Trim)
                                    End With
                                End If
                        End Select

                        Try
                            'Sam call to retreive the tax details
                            oWebservice.GetClaimReceiptTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Receipt, oQuote.BranchCode)
                        Finally
                            oWebservice = Nothing
                        End Try
                        'populating the grid based on results returned from sam
                        gvTaxesonThisReceipt.Visible = True
                        'gvTaxesonThisReceipt.DataSource = oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem
                        If HidATSoption.Value = 1 Then
                            oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem(0).Amount = tax
                        End If
                        gvTaxesonThisReceipt.DataSource = oClaimOpen.ClaimPeril(iPerilIndex).Receipt.TaxItem
                        gvTaxesonThisReceipt.DataBind()
                    Else
                        gvTaxesonThisReceipt.Visible = False
                    End If
                    txtTotalWHTax.Text = "0.00"
                    If amount = 0 Then
                        'need to show the static value in decimal
                        txtGrossPayment.Text = "0.00"
                        txtTotalTax.Text = "0.00"
                        txtNetPayment.Text = "0.00"
                        If (Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery) Then
                            gvSalvageDetails.Columns(9).Visible = True
                        Else
                            gvSalvageDetails.Columns(9).Visible = False
                        End If
                    Else
                        Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                        'if amount is not 0
                        pnlThisPaymentTab.Visible = True
                        If hSalvageAndTPRecoveryReservesExcludeTax.Value = "1" Then
                            txtGrossPayment.Text = String.Format(oFormatString, Math.Round((amount - tax), 2))
                            txtNetPayment.Text = String.Format(oFormatString, Math.Round((CDbl(txtGrossPayment.Text.Trim) + tax), 2))
                        Else
                            txtGrossPayment.Text = String.Format(oFormatString, Math.Round((amount + tax), 2))
                            txtNetPayment.Text = String.Format(oFormatString, Math.Round((CDbl(txtGrossPayment.Text.Trim) - tax), 2))
                        End If
                        txtTotalTax.Text = Math.Round(tax, 2)
                        If tax <> 0 AndAlso bdomiciledTax AndAlso bIsWithholdingTax Then
                            txtTotalWHTax.Text = Math.Round((amount + tax), 2)
                        Else
                            txtTotalWHTax.Text = "0.00"
                        End If
                        'txtNetPayment.Text = String.Format(oFormatString, Math.Round((CDbl(txtGrossPayment.Text.Trim) - tax), 2))
                        If Session(CNMode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                            gvPaymentDetails.Columns(9).Visible = False
                            gvSalvageDetails.Columns(9).Visible = False
                        End If
                    End If

                    If oClaimOpen.ClaimPeril(iPerilIndex).Payment IsNot Nothing AndAlso oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee IsNot Nothing Then
                        Address.Address1 = oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee.Address.Address1
                        Address.Address2 = oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee.Address.Address2
                        Address.Address3 = oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee.Address.Address3
                        Address.Address4 = oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee.Address.Address4
                        Address.CountryCode = oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee.Address.CountryCode
                        Address.Postcode = oClaimOpen.ClaimPeril(iPerilIndex).Payment.Payee.Address.PostCode
                    End If

                    'Set The label for Receipt
                    lblThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                    ThisPaymentSummary.Text = GetLocalResourceObject("ThisReceiptSummary")
                    ltGrossPayment.Text = GetLocalResourceObject("ltGrossReceipt")
                    ltNetPayment.Text = GetLocalResourceObject("ltNetReceipt")
                    lblTaxesOnThisPayment.Text = GetLocalResourceObject("ltTaxesOnThisReceipt")
                    lblPaymentDetails.Text = GetLocalResourceObject("ltReceiptDetails")
                    liChequeDate.Visible = False
                    liThisReference.Visible = False
                    divAddress.Visible = False

                End If
            ElseIf Session(CNMode) = Mode.ViewClaim AndAlso
              oClaimOpen.ClaimVersionDescription IsNot Nothing _
             AndAlso (oClaimOpen.ClaimVersionDescription.Trim().IndexOf("Recovery") <> -1) Then
            Else
                With oClaimOpen.ClaimPeril(iPerilIndex)

                    Dim amount As Decimal = 0.0
                    Dim tax As Decimal = 0.0
                    Dim iCount As Integer = 0

                    If .ClaimReserve IsNot Nothing Then
                        'Calculation of the amount and tax
                        Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                        If Session(CNMode) = Mode.ViewClaim OrElse
                            CType(Session(CNMode), Mode) = Mode.ViewClaimPayment OrElse
                            CType(Session(CNMode), Mode) = Mode.Authorise OrElse
                            CType(Session(CNMode), Mode) = Mode.DeclinePayment OrElse
                            CType(Session(CNMode), Mode) = Mode.Recommend Then
                            For Each oPaymentItem As NexusProvider.ClaimPayment In .ClaimPayment

                                iCount = iCount + 1
                                If (.ClaimPayment.Count = iCount AndAlso oPaymentItem.PaymentAmount <> 0 AndAlso oClaimOpen.ClaimVersionDescription <> "Maintained Claim") Then
                                    If Session(CNClaimPaymentKey) Is Nothing Then
                                        Session(CNClaimPaymentKey) = oPaymentItem.BaseClaimPaymentKey
                                    End If

                                    If oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE Then
                                        rblPayee.SelectedValue = "0"
                                        txtParty.Text = "CLMPAYABLE"
                                        hPartyKey.Value = 0
                                        If oPaymentItem.Payee IsNot Nothing AndAlso oPaymentItem.Payee.AccountType IsNot Nothing _
                                            AndAlso Not String.IsNullOrEmpty(oPaymentItem.Payee.AccountType.Trim) Then
                                            txtAccountType.Text = oPaymentItem.Payee.AccountType
                                            txtAccountType.Visible = True
                                            ddlAccountType.Visible = False
                                        Else
                                            txtAccountType.Visible = False
                                            ddlAccountType.Visible = True
                                            txtAccountType.Text = ""
                                        End If
                                    ElseIf oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY Then
                                        rblPayee.SelectedValue = "1"
                                        txtParty.Text = oPaymentItem.PartyPaidName
                                        btnParty.Enabled = False
                                        btnClientParty.Enabled = False
                                        hPartyKey.Value = oPaymentItem.PartyKey
                                    ElseIf oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT Then
                                        rblPayee.SelectedValue = "2"
                                        txtParty.Text = oPaymentItem.PartyPaidName
                                        hPartyKey.Value = 0
                                    ElseIf oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT Then
                                        rblPayee.SelectedValue = "3"
                                        txtParty.Text = oQuote.InsuredName
                                        hPartyKey.Value = 0
                                    End If
                                    amount += oPaymentItem.PaymentAmount + oPaymentItem.TaxAmount
                                    tax = oPaymentItem.TaxAmount
                                End If
                            Next
                        End If

                        If Session(CNMode) <> Mode.ViewClaim And CType(Session(CNMode), Mode) <> Mode.ViewClaimPayment And CType(Session(CNMode), Mode) <> Mode.Authorise And CType(Session(CNMode), Mode) <> Mode.DeclinePayment And CType(Session(CNMode), Mode) <> Mode.Recommend Then
                            For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In .ClaimReserve
                                amount += oPaymentItem.ThisPaymentINCLTax
                                tax += oPaymentItem.ThisPaymentTax
                            Next
                        End If
                        'If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.Recommend Or Session(CNUnAllocatedClaimPayment) IsNot Nothing Then
                        '    Dim iMediaTypeId As Integer
                        '    Dim iBankAccountId As Integer
                        '    GetBankAccountDefault(iMediaTypeId, iBankAccountId)
                        '    If iMediaTypeId > 0 Then
                        '        Dim strMediaTypeCode As String
                        '        strMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iMediaTypeId, "MediaType", True, Session(CNBranchCode))
                        '        ddlMediaType.SelectedValue = strMediaTypeCode
                        '    End If
                        'End If
                        If Session(CNMode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment _
                        Or CType(Session(CNMode), Mode) = Mode.Authorise _
                        Or CType(Session(CNMode), Mode) = Mode.DeclinePayment _
                        Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                            Dim sCurrencyCode As String
                            If .ClaimPayment IsNot Nothing Then
                                Dim oClaimPayment As NexusProvider.ClaimPayment = Nothing

                                For Each oPaymentItem As NexusProvider.ClaimPayment In .ClaimPayment
                                    sCurrencyCode = oPaymentItem.CurrencyCode
                                    tax = oPaymentItem.TaxAmount
                                    ' oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyKey = 0
                                    oClaimOpen.ClaimPeril(iPerilIndex).Payment.CurrencyCode = sCurrencyCode
                                    If Session(CNClaimPaymentKey) = oPaymentItem.BaseClaimPaymentKey Then
                                        'Set the Media Type
                                        If oPaymentItem.Payee IsNot Nothing AndAlso String.IsNullOrEmpty(oPaymentItem.Payee.MediaTypeCode) = False Then
                                            If String.IsNullOrEmpty(oPaymentItem.Payee.MediaTypeCode) = False Then
                                                GISLookup_MediaType.Value = oPaymentItem.Payee.MediaTypeCode.Trim
                                            End If
                                        End If

                                        'set the Payee Address
                                        If oPaymentItem.Payee.Address IsNot Nothing Then
                                            'Address population
                                            Address.Address1 = Trim(oPaymentItem.Payee.Address.Address1)
                                            Address.Address2 = Trim(oPaymentItem.Payee.Address.Address2)
                                            Address.Address3 = Trim(oPaymentItem.Payee.Address.Address3)
                                            Address.Address4 = Trim(oPaymentItem.Payee.Address.Address4)
                                            Address.CountryCode = Trim(oPaymentItem.Payee.Address.CountryCode)
                                            Address.Postcode = Trim(oPaymentItem.Payee.Address.PostCode)
                                        End If
                                        'set UltimatePayee
                                        txtUltimatePayee.Text = oPaymentItem.UltimatePayee
                                        'Set the ThisReference
                                        If oPaymentItem.Payee IsNot Nothing AndAlso String.IsNullOrEmpty(oPaymentItem.Payee.TheirReference) = False Then

                                            If String.IsNullOrEmpty(oPaymentItem.Payee.TheirReference) = False Then
                                                txtThisReference.Text = oPaymentItem.Payee.TheirReference
                                                txtThisReference.ReadOnly = True
                                            End If

                                            'Set the Media Reference 
                                            If String.IsNullOrEmpty(oPaymentItem.Payee.MediaReference) = False Then
                                                txtMediaRef.Text = oPaymentItem.Payee.MediaReference
                                                txtMediaRef.ReadOnly = True
                                            End If

                                            'Set the Comments
                                            If String.IsNullOrEmpty(oPaymentItem.Payee.Comments) = False Then
                                                txtComments.Text = oPaymentItem.Payee.Comments
                                            Else
                                                txtChequeDate.Text = Date.Now.ToShortDateString()
                                            End If
                                            txtComments.ReadOnly = True

                                        End If

                                        'Set the Payment Date 
                                        If String.IsNullOrEmpty(oPaymentItem.PaymentDate) = False Then
                                            txtChequeDate.Text = oPaymentItem.PaymentDate
                                            txtChequeDate.ReadOnly = True
                                        End If

                                        'Set the txtOurRef
                                        If String.IsNullOrEmpty(oPaymentItem.OurRef) = False Then
                                            txtOurRef.Text = oPaymentItem.OurRef
                                            txtOurRef.ReadOnly = True
                                        End If

                                        'populate party bank details
                                        If oPaymentItem.Payee IsNot Nothing AndAlso oPaymentItem.Payee.PartyBankKey > 0 Then
                                            'Set the Party Key for population of Party Bank Details
                                            hPartyBankKey.Value = oPaymentItem.Payee.PartyBankKey
                                            DisplayAccountTypeInformation()
                                        ElseIf oPaymentItem.Payee IsNot Nothing Then
                                            txtBankAccNumber.Text = oPaymentItem.Payee.BankNumber
                                            txtPayeeName.Text = oPaymentItem.Payee.Name
                                            txtBankCode.Text = oPaymentItem.Payee.BankCode
                                            txtBankName.Text = oPaymentItem.Payee.BankName
                                            txtBIC.Text = oPaymentItem.Payee.BIC
                                            txtIBAN.Text = oPaymentItem.Payee.IBAN
                                            txtBankAccNumber.ReadOnly = True
                                            txtPayeeName.ReadOnly = True
                                            txtBankCode.ReadOnly = True
                                            txtBankName.ReadOnly = True
                                            txtBIC.ReadOnly = True
                                            txtIBAN.ReadOnly = True
                                        End If
                                        If oPaymentItem.Payee IsNot Nothing AndAlso oPaymentItem.Payee.PartyBankKey > 0 Then
                                            'Set the Party Key for population of Party Bank Details
                                            hPartyBankKey.Value = oPaymentItem.Payee.PartyBankKey
                                            DisplayAccountTypeInformation()
                                        End If
                                        For iCount = 0 To oPaymentItem.PaymentItems.Count - 1
                                            For Each oClaimPaymentItem As NexusProvider.ClaimPaymentItemType In .Payment.ClaimPaymentItem
                                                If oPaymentItem.PaymentItems(iCount).BaseReserveKey = oClaimPaymentItem.BaseReserveKey Then
                                                    oClaimPaymentItem.PaymentAmount = oPaymentItem.PaymentItems(iCount).PaymentAmount
                                                    oClaimPaymentItem.LossPaymentAmount = oPaymentItem.PaymentItems(iCount).LossAmount
                                                    oClaimPaymentItem.TaxAmount = oPaymentItem.PaymentItems(iCount).TaxAmount 'tax
                                                    oClaimPaymentItem.ReverseExcess = oPaymentItem.PaymentItems(iCount).ReverseExcess ' bReserveExcess
                                                    oClaimPaymentItem.TaxGroupCode = oPaymentItem.PaymentItems(iCount).TaxGroupCode '"ORIGINAL"
                                                End If
                                            Next
                                        Next
                                    End If
                                Next

                                If tax <> 0 Then
                                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                    Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                                    Select Case rblPayee.SelectedValue
                                        Case "0"
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = "CLMPAYABLE"
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = 0
                                        Case "1"
                                            'Party
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = Convert.ToInt32(hPartyKey.Value.Trim)
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = txtParty.Text.Trim

                                        Case "2"
                                            'Agent
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = oQuote.AgentCode

                                        Case "3"
                                            'Client
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = oClaimOpen.ClientShortName

                                    End Select

                                    Try
                                        If Session(CNMode) = Mode.ViewClaim Then
                                            oClaimOpen.ClaimPeril(iPerilIndex).Payment.ViewMode = True
                                        End If

                                        If HidATSoption.Value = 0 Then
                                            'Sam call to retreive the tax details
                                            oClaimPayment = oWebservice.GetClaimPaymentTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Payment, oQuote.BranchCode)
                                            'populating the grid based on results returned from sam
                                            gvTaxesonThisPayment.Visible = True
                                            gvTaxesonThisPayment.DataSource = oClaimPayment.ClaimPaymentTaxItems
                                            gvTaxesonThisPayment.DataBind()
                                        Else
                                            Dim oClaimPaymentTax As NexusProvider.ClaimPaymentTaxItemCollection = Nothing
                                            oClaimPaymentTax = oClaimOpen.ClaimPeril(iPerilIndex).Payment.ClaimPaymentTaxItems
                                            If oClaimPaymentTax IsNot Nothing AndAlso oClaimPaymentTax.Count > 0 Then
                                                gvTaxesonThisPayment.Visible = True
                                                gvTaxesonThisPayment.DataSource = oClaimPaymentTax
                                                gvTaxesonThisPayment.DataBind()
                                            Else
                                                oClaimPayment = oWebservice.GetClaimPaymentTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Payment, oQuote.BranchCode)
                                                'populating the grid based on results returned from sam
                                                gvTaxesonThisPayment.Visible = True
                                                gvTaxesonThisPayment.DataSource = oClaimPayment.ClaimPaymentTaxItems
                                                gvTaxesonThisPayment.DataBind()
                                            End If
                                        End If
                                    Finally
                                        oWebservice = Nothing
                                    End Try

                                Else
                                    gvTaxesonThisPayment.Visible = False
                                End If
                            End If
                        Else
                            If tax <> 0 Then
                                Dim oClaimPayment As NexusProvider.ClaimPayment = Nothing
                                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                                Select Case rblPayee.SelectedValue
                                    Case "0"
                                        If Session(CNMode) = Mode.PayClaim Then
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = "CLMPAYABLE"
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = 0
                                        End If

                                    Case "1"
                                        'Party
                                        If Session(CNMode) = Mode.PayClaim Then
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = Convert.ToInt32(hPartyKey.Value.Trim)
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = txtParty.Text.Trim
                                            bdomiciledTax = IsDomiciledForTax(oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey)
                                        End If
                                    Case "2"
                                        'Agent
                                        If Session(CNMode) = Mode.PayClaim Then
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = oQuote.AgentCode
                                            'oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = Convert.ToInt32(oQuote.Agent)
                                        End If
                                    Case "3"
                                        'Client
                                        If Session(CNMode) = Mode.PayClaim Then
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT
                                            oClaimOpen.ClaimPeril(iPeril).Payment.PartyPaidCode = oClaimOpen.ClientShortName
                                            'Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                                            'oClaimOpen.ClaimPeril(iPeril).Payment.PartyKey = oParty.Key
                                        End If

                                End Select

                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PaymentAdvancedTaxDetails.PaymentTo = oClaimOpen.ClaimPeril(iPerilIndex).Payment.PaymentAdvancedTaxDetails.PaymentTo
                                Try
                                    'Sam call to retreive the tax details
                                    If HidATSoption.Value = 0 Then
                                        oClaimPayment = oWebservice.GetClaimPaymentTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Payment, oQuote.BranchCode)
                                        'populating the grid based on results returned from sam
                                        gvTaxesonThisPayment.Visible = True
                                        gvTaxesonThisPayment.DataSource = oClaimPayment.ClaimPaymentTaxItems
                                        gvTaxesonThisPayment.DataBind()
                                    Else

                                        Dim oClaimPaymentTax As NexusProvider.ClaimPaymentTaxItemCollection = Nothing
                                        oClaimPaymentTax = oClaimOpen.ClaimPeril(iPerilIndex).Payment.ClaimPaymentTaxItems
                                        If oClaimPaymentTax IsNot Nothing AndAlso oClaimPaymentTax.Count > 0 Then
                                            gvTaxesonThisPayment.Visible = True
                                            gvTaxesonThisPayment.DataSource = oClaimPaymentTax
                                            gvTaxesonThisPayment.DataBind()
                                            If oClaimPayment IsNot Nothing Then
                                                If oClaimPayment.ClaimPaymentTaxItems.Count > 0 Then
                                                    ViewState("taxamount") = oClaimPayment.ClaimPaymentTaxItems.Item(0).Amount
                                                End If
                                            End If
                                        Else
                                            oClaimPayment = oWebservice.GetClaimPaymentTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Payment, oQuote.BranchCode)
                                            'populating the grid based on results returned from sam
                                            gvTaxesonThisPayment.Visible = True
                                            gvTaxesonThisPayment.DataSource = oClaimPayment.ClaimPaymentTaxItems
                                            gvTaxesonThisPayment.DataBind()

                                        End If

                                    End If
                                Finally
                                    oWebservice = Nothing
                                End Try
                            Else
                                gvTaxesonThisPayment.Visible = False
                                ViewState("taxamount") = "0.00"
                            End If
                        End If

                        'End If
                        txtTotalWHTax.Text = "0.00"
                        If amount = 0 Then
                            'need to show the static value in decimal
                            txtGrossPayment.Text = "0.00"
                            txtTotalTax.Text = "0.00"
                            txtNetPayment.Text = "0.00"
                            'View Mode will not display Edit link of Paymentdetails Grid
                            If Session(CNMode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                                gvPaymentDetails.Columns(9).Visible = False
                            End If
                        Else
                            'if amount is not 0
                            'Format changed - Etana Nexus 3.1 Grid 
                            Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                            pnlThisPaymentTab.Visible = True
                            txtGrossPayment.Text = String.Format(oFormatString, amount)
                            ViewState("taxamount") = String.Format(oFormatString, tax)
                            txtTotalTax.Text = String.Format(oFormatString, tax)
                            txtTotalWHTax.Text = "0.00"
                            If tax <> 0 AndAlso bIsWithholdingTax Then
                                txtTotalWHTax.Text = String.Format(oFormatString, tax)
                            Else
                                txtTotalWHTax.Text = "0.00"
                                txtTotalTax.Text = String.Format(oFormatString, Math.Round((tax), 2))
                            End If
                            txtNetPayment.Text = String.Format(oFormatString, (CDbl(txtGrossPayment.Text.Trim) - tax))


                            'View Mode will not display Edit link of Paymentdetails Grid
                            If Session(CNMode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                                gvPaymentDetails.Columns(9).Visible = False
                                txtThisReference.ReadOnly = True
                            End If
                        End If
                    End If
                End With
            End If

            'Make read only these fields
            txtGrossPayment.Attributes.Add("readonly", "readonly")
            txtTotalTax.Attributes.Add("readonly", "readonly")
            txtTotalWHTax.Attributes.Add("readonly", "readonly")
            txtNetPayment.Attributes.Add("readonly", "readonly")
            txtGrossPayment.ReadOnly = True
            txtTotalTax.ReadOnly = True
            txtTotalWHTax.ReadOnly = True
            txtNetPayment.ReadOnly = True
        End Sub

        Protected Sub PopulateScriptThisPayment(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)
            Dim dTax As Double
            Dim dTaxWH As Double
            If Not (m_sIsPaymentsReadOnly = "1" AndAlso Session(CNMode) = Mode.PayClaim) Then Exit Sub

            With oClaimOpen.ClaimPeril(iPerilIndex)

                Dim amount As Decimal = 0.0
                Dim tax As Decimal = 0.0
                Dim iCount As Integer = 0

                If .ClaimReserve IsNot Nothing Then
                    'Calculation of the amount and tax
                    Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)

                    For Each oPaymentItem As NexusProvider.ClaimPayment In .ClaimPayment
                        If oPaymentItem.IsThisPayment Then
                            If Session(CNClaimPaymentKey) Is Nothing Then
                                Session(CNClaimPaymentKey) = oPaymentItem.BaseClaimPaymentKey
                            End If

                            If oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMPAYABLE"

                            ElseIf oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = oPaymentItem.PartyPaidCode
                                btnParty.Enabled = False
                                hPartyKey.Value = oPaymentItem.PartyKey
                            ElseIf oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                txtParty.Text = oPaymentItem.PartyPaidCode
                            ElseIf oPaymentItem.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName
                                hPartyKey.Value = oPaymentItem.PartyKey
                            End If
                            amount += oPaymentItem.ThisPaymentINCLTax
                            tax += oPaymentItem.ThisPaymentTax



                            Dim sCurrencyCode As String = oPaymentItem.CurrencyCode
                            ' oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyKey = 0
                            oClaimOpen.ClaimPeril(iPerilIndex).Payment.CurrencyCode = sCurrencyCode
                            If Session(CNClaimPaymentKey) = oPaymentItem.BaseClaimPaymentKey Then
                                'Set the Media Type
                                If oPaymentItem.Payee IsNot Nothing AndAlso String.IsNullOrEmpty(oPaymentItem.Payee.MediaTypeCode) = False Then
                                    If String.IsNullOrEmpty(oPaymentItem.Payee.MediaTypeCode) = False Then
                                        GISLookup_MediaType.Value = oPaymentItem.Payee.MediaTypeCode.Trim
                                    End If
                                End If

                                'set the Payee Address
                                If oPaymentItem.Payee.Address IsNot Nothing Then
                                    'Address population
                                    Address.Address1 = Trim(oPaymentItem.Payee.Address.Address1)
                                    Address.Address2 = Trim(oPaymentItem.Payee.Address.Address2)
                                    Address.Address3 = Trim(oPaymentItem.Payee.Address.Address3)
                                    Address.Address4 = Trim(oPaymentItem.Payee.Address.Address4)
                                    Address.CountryCode = Trim(oPaymentItem.Payee.Address.CountryCode)
                                    Address.Postcode = Trim(oPaymentItem.Payee.Address.PostCode)
                                End If
                                'set UltimatePayee
                                txtUltimatePayee.Text = oPaymentItem.UltimatePayee
                                'Set the ThisReference
                                If oPaymentItem.Payee IsNot Nothing Then
                                    If String.IsNullOrEmpty(oPaymentItem.Payee.TheirReference) = False Then
                                        txtThisReference.Text = oPaymentItem.Payee.TheirReference
                                    End If

                                    'Set the Media Reference 
                                    If String.IsNullOrEmpty(oPaymentItem.Payee.MediaReference) = False Then
                                        txtMediaRef.Text = oPaymentItem.Payee.MediaReference
                                    End If

                                    'Set the Comments
                                    If String.IsNullOrEmpty(oPaymentItem.Payee.Comments) = False Then
                                        txtComments.Text = oPaymentItem.Payee.Comments
                                    End If

                                End If


                                'Set the Payment Date 
                                If String.IsNullOrEmpty(oPaymentItem.PaymentDate) = False Then
                                    txtChequeDate.Text = oPaymentItem.PaymentDate
                                    txtChequeDate.ReadOnly = True
                                End If

                                'Set the txtOurRef
                                If String.IsNullOrEmpty(oPaymentItem.OurRef) = False Then
                                    txtOurRef.Text = oPaymentItem.OurRef
                                End If

                                'populate party bank details
                                If oPaymentItem.Payee IsNot Nothing AndAlso oPaymentItem.Payee.PartyBankKey > 0 Then
                                    'Set the Party Key for population of Party Bank Details
                                    hPartyBankKey.Value = oPaymentItem.Payee.PartyBankKey
                                    DisplayAccountTypeInformation()
                                End If

                                If oPaymentItem.Payee IsNot Nothing Then
                                    txtBankAccNumber.Text = oPaymentItem.Payee.BankNumber
                                    txtPayeeName.Text = oPaymentItem.Payee.Name
                                    txtBankCode.Text = oPaymentItem.Payee.BankCode
                                    txtBankName.Text = oPaymentItem.Payee.BankName
                                End If

                                For iCount = 0 To oPaymentItem.PaymentItems.Count - 1
                                    For Each oClaimPaymentItem As NexusProvider.ClaimPaymentItemType In .Payment.ClaimPaymentItem
                                        If oPaymentItem.PaymentItems(iCount).BaseReserveKey = oClaimPaymentItem.BaseReserveKey Then
                                            oClaimPaymentItem.PaymentAmount = amount
                                            oClaimPaymentItem.LossPaymentAmount = amount
                                            oClaimPaymentItem.TaxAmount = oPaymentItem.PaymentItems(iCount).TaxAmount 'tax
                                            oClaimPaymentItem.ReverseExcess = oPaymentItem.PaymentItems(iCount).ReverseExcess ' bReserveExcess
                                            oClaimPaymentItem.TaxGroupCode = oPaymentItem.PaymentItems(iCount).TaxGroupCode '"ORIGINAL"
                                        End If
                                    Next
                                Next

                            End If
                        End If
                    Next


                    If tax <> 0 Then
                        Dim oClaimPayment As NexusProvider.ClaimPayment = Nothing

                        Select Case rblPayee.SelectedValue
                            Case "0"
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyPaidCode = "CLMPAYABLE"
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyKey = 0
                            Case "1"
                                'Party
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyKey = Convert.ToInt32(hPartyKey.Value.Trim)
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyPaidCode = txtParty.Text.Trim
                                bdomiciledTax = IsDomiciledForTax(oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyKey)

                            Case "2"
                                'Agent
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyPaidCode = oQuote.AgentCode

                            Case "3"
                                'Client
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT
                                oClaimOpen.ClaimPeril(iPerilIndex).Payment.PartyPaidCode = oClaimOpen.ClientShortName

                        End Select

                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        m_sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)

                        Try
                            If HidATSoption.Value = 0 Then
                                'Sam call to retreive the tax details
                                oClaimPayment = oWebservice.GetClaimPaymentTaxes(oClaimOpen.ClaimPeril(iPerilIndex).Payment, oQuote.BranchCode, oClaimOpen.ClaimPeril(iPerilIndex).ClaimPerilKey)
                                'populating the grid based on results returned from sam
                                gvTaxesonThisPayment.Visible = True
                                gvTaxesonThisPayment.DataSource = oClaimPayment.ClaimPaymentTaxItems
                                gvTaxesonThisPayment.DataBind()
                            Else
                                Dim oClaimPaymentTax As NexusProvider.ClaimPaymentTaxItemCollection = Nothing
                                oClaimPaymentTax = oClaimOpen.ClaimPeril(iPerilIndex).Payment.ClaimPaymentTaxItems
                                gvTaxesonThisPayment.Visible = True
                                gvTaxesonThisPayment.DataSource = oClaimPaymentTax
                                gvTaxesonThisPayment.DataBind()
                            End If

                        Finally
                            oWebservice = Nothing
                        End Try

                    Else
                        gvTaxesonThisPayment.Visible = False
                    End If
                End If

                If amount <> 0 Then
                    'if amount is not 0
                    'Format changed - Etana Nexus 3.1 Grid 
                    Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                    pnlThisPaymentTab.Visible = True
                    txtGrossPayment.Text = String.Format(oFormatString, amount)
                    txtTotalTax.Text = String.Format(oFormatString, tax)
                    If tax > 0 AndAlso bIsWithholdingTax Then
                        txtTotalWHTax.Text = String.Format(oFormatString, tax)
                    Else
                        txtTotalWHTax.Text = "0.00"
                        If dTax + dTaxWH <> 0 Then
                            If bdomiciledTax Then
                                dTaxWH += dTax
                                txtTotalWHTax.Text = String.Format(oFormatString, Math.Round((dTaxWH), 2))
                            Else
                                txtTotalWHTax.Text = String.Format(oFormatString, Math.Round((dTaxWH), 2))
                                txtTotalTax.Text = String.Format(oFormatString, Math.Round((dTax), 2))
                            End If
                        End If

                        txtNetPayment.Text = String.Format(oFormatString, (CDbl(txtGrossPayment.Text.Trim) - (dTax + dTaxWH)))
                        gvPaymentDetails.Columns(9).Visible = False
                    End If
                    txtNetPayment.Text = String.Format(oFormatString, (CDbl(txtGrossPayment.Text.Trim) - tax))

                    gvPaymentDetails.Columns(9).Visible = False
                End If
            End With

            'Make read only these fields
            txtGrossPayment.Attributes.Add("readonly", "readonly")
            txtTotalTax.Attributes.Add("readonly", "readonly")
            txtTotalWHTax.Attributes.Add("readonly", "readonly")
            txtNetPayment.Attributes.Add("readonly", "readonly")

            txtGrossPayment.ReadOnly = True
            txtTotalTax.ReadOnly = True
            txtTotalWHTax.ReadOnly = True
            txtNetPayment.ReadOnly = True

            rblPayee.Enabled = False
            btnParty.Enabled = False
            txtParty.ReadOnly = True
            txtUltimatePayee.Attributes.Add("readonly", "readonly")
            txtUltimatePayee.ReadOnly = True

            GISLookup_MediaType.Enabled = True
            txtMediaRef.Attributes.Remove("readonly")
            txtPayeeName.Attributes.Remove("readonly")
            txtBankName.Attributes.Remove("readonly")
            txtBankCode.Attributes.Remove("readonly")
            txtBankAccNumber.Attributes.Remove("readonly")
            txtThisReference.Attributes.Remove("readonly")
            txtOurRef.Attributes.Remove("readonly")
            txtComments.Attributes.Remove("readonly")

        End Sub
#End Region

        ''' <summary>
        ''' PopulateReserveItem
        ''' </summary>
        ''' <remarks></remarks>
        Sub PopulateReserveItem()
            If (m_sIsPaymentsReadOnly = "1" AndAlso Session(CNMode) = Mode.PayClaim) Then
                PopulateScriptReserveItem()
                Exit Sub
            End If
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = Nothing
            Dim oClaimPayment As New NexusProvider.ClaimPayment
            'Retreiving the claim quote information from session
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            'Retreiving the claim  information from session
            oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

            For Each oCPeril As NexusProvider.PerilSummary In oClaimOpen.ClaimPeril
                For Each oReserveItem As NexusProvider.Reserve In oCPeril.Reserve
                    If oReserveItem.BaseReserveKey <> 0 Then
                        Dim oClaimPaymentItem As New NexusProvider.ClaimPaymentItemType
                        Dim oClaimReserve As New NexusProvider.ClaimPerilReservePaymentType
                        oClaimPayment.BaseReserveKey = oReserveItem.BaseReserveKey
                        oClaimPaymentItem.BaseReserveKey = oReserveItem.BaseReserveKey

                        With oClaimReserve
                            .TypeCode = oReserveItem.TypeCode
                            .BaseReserveKey = oReserveItem.BaseReserveKey

                            'Total Tax Paid and Amount Paid
                            If oClaimOpen.ClaimPeril(iPeril).ClaimPayment IsNot Nothing Then
                                Dim dPaymentBaseReserveKey As Integer = 0
                                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimPayment.Count - 1
                                    For kCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems.Count - 1
                                        If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems IsNot Nothing _
                                        AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems.Count > 0 Then
                                            dPaymentBaseReserveKey = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).BaseReserveKey
                                        End If

                                        If dPaymentBaseReserveKey = oReserveItem.BaseReserveKey Then
                                            If dPaymentBaseReserveKey = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).BaseReserveKey Then
                                                .PaidToDateTax += oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).LossTaxAmount
                                                .PaidToDate += oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).LossAmount

                                                ''Only View Mode will retrive Latest Payment details
                                                If oClaimOpen.ClaimVersionDescription <> "Maintained Claim" Then
                                                    If Session(CNMode) = Mode.ViewClaim OrElse CType(Session(CNMode), Mode) = Mode.ViewClaimPayment OrElse CType(Session(CNMode), Mode) = Mode.Authorise OrElse CType(Session(CNMode), Mode) = Mode.DeclinePayment OrElse CType(Session(CNMode), Mode) = Mode.Recommend Then
                                                        .ThisPaymentTax = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).LossTaxAmount
                                                        .ThisPaymentINCLTax = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).LossAmount + .ThisPaymentTax
                                                        .CostToClaim = .ThisPaymentINCLTax - .ThisPaymentTax
                                                    End If

                                                    If (Session(CNMode) = Mode.ViewClaim OrElse CType(Session(CNMode), Mode) = Mode.ViewClaimPayment) Then
                                                        .ThisRevision = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(kCount).ThisRevision
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next

                                Next
                            End If
                            .PaidToDate = oReserveItem.PaidAmount
                            .TotalReserve = oReserveItem.InitialReserve + oReserveItem.RevisedReserve
                            .GrossReserve = oReserveItem.GrossReserve
                            .Tax = oReserveItem.Tax
                            .RevisedGrossReserve = oReserveItem.RevisedGrossReserve
                            .RevisedTaxReserve = oReserveItem.RevisedTaxReserve
                            .IsExcess = oReserveItem.IsExcess
                            .IsIndemnity = oReserveItem.IsIndemnity
                            .IsExpense = oReserveItem.IsExpense

                            'Calculation of Current Reserve and other values
                            If oReserveItem.IsExcess Then
                                .CurrentReserve = (oReserveItem.InitialReserve + oReserveItem.RevisedReserve) + (- .PaidToDate)
                            Else
                                .CurrentReserve = .TotalReserve - .PaidToDate
                            End If
                            If .OldReserve <= 0.0 Then
                                .OldReserve = .CurrentReserve
                            End If

                            If Session.Item(CNReserveDescriptions) IsNot Nothing Then
                                Dim oReserveDescriptions As Hashtable = Session.Item(CNReserveDescriptions)
                                Dim HData As DictionaryEntry
                                For Each HData In oReserveDescriptions
                                    If HData.Key.ToString.Trim.ToUpper = oReserveItem.TypeCode.Trim.ToUpper Then
                                        .Description = HData.Value
                                    End If
                                Next
                            End If
                        End With
                        'All claim reserve will be added
                        If oCPeril.ClaimReserve IsNot Nothing Then
                            oCPeril.ClaimReserve.Add(oClaimReserve)
                        End If
                        'Only selected peril will have payment item
                        If oClaimOpen.ClaimPeril(iPeril).ClaimPerilKey = oCPeril.ClaimPerilKey Then
                            oClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItem)
                        End If
                    End If
                Next
            Next

            'Updating values into oClaimPayment object
            oClaimPayment.LossCurrencyCode = oClaimOpen.CurrencyISOCode  'Issue126
            oClaimPayment.RiskType = oQuote.Risks(0).Description
            oClaimPayment.BaseClaimKey = oClaimOpen.BaseClaimKey
            oClaimPayment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPeril).BaseClaimPerilKey
            oClaimPayment.ClientKey = oClaimOpen.Client.PartyKey
            oClaimOpen.ClaimPeril(iPeril).Payment = oClaimPayment
            Session(CNClaim) = oClaimOpen
        End Sub

        Sub PopulateScriptReserveItem()

            If Not (m_sIsPaymentsReadOnly = "1" AndAlso Session(CNMode) = Mode.PayClaim) Then Exit Sub

            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = Nothing
            Dim oClaimPayment As New NexusProvider.ClaimPayment
            'Retreiving the claim quote information from session
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            'Retreiving the claim  information from session
            oClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)



            For Each oCPeril As NexusProvider.PerilSummary In oClaimOpen.ClaimPeril
                For Each oReserveItem As NexusProvider.Reserve In oCPeril.Reserve
                    If oReserveItem.BaseReserveKey <> 0 Then
                        Dim oClaimPaymentItem As New NexusProvider.ClaimPaymentItemType
                        Dim oClaimReserve As New NexusProvider.ClaimPerilReservePaymentType
                        oClaimPayment.BaseReserveKey = oReserveItem.BaseReserveKey
                        oClaimPaymentItem.BaseReserveKey = oReserveItem.BaseReserveKey

                        With oClaimReserve
                            .TypeCode = oReserveItem.TypeCode
                            .BaseReserveKey = oReserveItem.BaseReserveKey

                            'Total Tax Paid and Amount Paid
                            If oClaimOpen.ClaimPeril(iPeril).ClaimPayment IsNot Nothing Then


                                Dim dPaymentBaseReserveKey As Integer = 0
                                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimPayment.Count - 1
                                    If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems IsNot Nothing _
                                    AndAlso oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems.Count > 0 Then
                                        dPaymentBaseReserveKey = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(0).BaseReserveKey
                                    End If


                                    If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).IsThisPayment AndAlso m_sIsPaymentsReadOnly = "1" Then
                                        For iPIIndex As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems.Count - 1
                                            If oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(iPIIndex).BaseReserveKey = oReserveItem.BaseReserveKey Then
                                                Dim crPayment As Decimal = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(iPIIndex).PaymentAmount
                                                Dim crTax As Decimal = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentItems(iPIIndex).TaxAmount
                                                .ThisPaymentINCLTax += crPayment + crTax
                                                .ThisPaymentTax += crTax
                                                .CostToClaim += crPayment
                                            End If
                                        Next
                                    Else
                                        If dPaymentBaseReserveKey = oReserveItem.BaseReserveKey Then
                                            .PaidToDateTax += oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).TaxAmount
                                            .CostToClaim += .ThisPaymentINCLTax - .ThisPaymentTax
                                            '.PaidToDate = oClaimOpen.ClaimPeril(iPeril).Reserve(iCount).PaidAmount
                                            .PaidToDate += oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).PaymentAmount

                                            ''Only View Mode will retrive Latest Payment details
                                            If Session(CNMode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                                                .ThisPaymentTax = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).TaxAmount
                                                If hIsGrossClaimPaymentAmount.Value = "1" Then
                                                    .ThisPaymentINCLTax = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).LossAmount
                                                Else
                                                    .ThisPaymentINCLTax = oClaimOpen.ClaimPeril(iPeril).ClaimPayment(iCount).LossAmount + .ThisPaymentTax
                                                End If
                                                .CostToClaim = .ThisPaymentINCLTax - .ThisPaymentTax
                                            End If

                                        End If

                                    End If
                                Next
                            End If



                            .PaidToDate = oReserveItem.PaidAmount
                            .TotalReserve = oReserveItem.InitialReserve + oReserveItem.RevisedReserve
                            .IsExcess = oReserveItem.IsExcess
                            .IsIndemnity = oReserveItem.IsIndemnity
                            .IsExpense = oReserveItem.IsExpense

                            'Calculation of Current Reserve and other values
                            If oReserveItem.IsExcess Then
                                .CurrentReserve = (oReserveItem.InitialReserve + oReserveItem.RevisedReserve) + (- .PaidToDate) 'oReserveItem.PaidAmount)
                            Else
                                .CurrentReserve = .TotalReserve - .PaidToDate
                                'If hIsGrossClaimPaymentAmount.Value = "0" Then
                                '    .CurrentReserve = .TotalReserve - (.PaidToDate + .PaidToDateTax)
                                'Else
                                '    .CurrentReserve = .TotalReserve - .PaidToDate
                                'End If
                            End If
                            If .OldReserve <= 0.0 Then
                                .OldReserve = .CurrentReserve
                            End If

                            If Session.Item(CNReserveDescriptions) IsNot Nothing Then
                                Dim oReserveDescriptions As Hashtable = Session.Item(CNReserveDescriptions)
                                Dim HData As DictionaryEntry
                                For Each HData In oReserveDescriptions
                                    If HData.Key.ToString.Trim.ToUpper = oReserveItem.TypeCode.Trim.ToUpper Then
                                        .Description = HData.Value
                                    End If
                                Next
                            End If
                        End With
                        'All claim reserve will be added
                        If oCPeril.ClaimReserve IsNot Nothing Then
                            oCPeril.ClaimReserve.Add(oClaimReserve)
                        End If
                        'Only selected peril will have payment item
                        If oClaimOpen.ClaimPeril(iPeril).ClaimPerilKey = oCPeril.ClaimPerilKey Then
                            oClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItem)
                        End If
                    End If
                Next
            Next

            'Updating values into oClaimPayment object
            oClaimPayment.LossCurrencyCode = oClaimOpen.CurrencyISOCode  'Issue126
            oClaimPayment.RiskType = oQuote.Risks(0).Description
            oClaimPayment.BaseClaimKey = oClaimOpen.BaseClaimKey
            oClaimPayment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPeril).BaseClaimPerilKey
            oClaimPayment.ClientKey = oClaimOpen.Client.PartyKey
            oClaimOpen.ClaimPeril(iPeril).Payment = oClaimPayment
            Session(CNClaim) = oClaimOpen
        End Sub

        Protected Sub IsPaymentReceived_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles IsPaymentReceived.ServerValidate
            ' Skip receipt/payment validation when Instalments tab is active (This Receipt hidden per FR-003)
            If liInstalments IsNot Nothing AndAlso liInstalments.Visible Then
                args.IsValid = True
                Exit Sub
            End If
            ' If condition Implemented against WorkItem 2099. If PayClaim and Reserves are on same PerilBuilder page then condition created to stop Pay validations during Open Claim Mode  
            IsPaymentReceived.ErrorMessage = ""
            If (Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery) Then
                'Valid Party Key Validation
                Select Case rblPayee.SelectedValue
                    Case "1", "4"
                        If String.IsNullOrEmpty(txtParty.Text) = True Then
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("Err_VldPartyKey")
                            args.IsValid = False
                            Exit Sub
                        End If

                    Case ""
                        IsPaymentReceived.ErrorMessage = GetLocalResourceObject("Err_VldPartyKey")
                        gvPaymentDetails.Columns(9).Visible = False
                        args.IsValid = False
                        Exit Sub
                End Select

                Dim oClaimOpen As NexusProvider.ClaimOpen = Session.Item(CNClaim)
                Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                If Session(CNMode) = Mode.PayClaim Then
                    'For Claim Payments
                    Dim dAmount As Decimal = 0.0
                    If oClaimReserve IsNot Nothing Then
                        For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oClaimReserve
                            dAmount += oPaymentItem.CostToClaim
                        Next
                    End If
                    If dAmount = 0 Then
                        args.IsValid = False
                        'Check link ClaimReserve
                        If oClaimReserve IsNot Nothing Then
                            If oClaimReserve.Count = 0 Then
                                'No Reserve error message display
                                IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_ClaimReserveNothing_Error")
                                args.IsValid = False
                            Else
                                'No amount error message display
                                IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_PaymentReceived_Error")
                                args.IsValid = False
                            End If
                        End If
                    Else
                        'Check Media Type Field Mandatory on Claim Payment       
                        If HidMediaTypeFieldMandatory.Value = "1" And GISLookup_MediaType.Value = "" AndAlso GISLookup_MediaType.Enabled = True Then
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_MediatypeError")
                            args.IsValid = False
                        End If

                        'Check Claim Payment Amount
                        ''Dim oUserAuthority As New NexusProvider.UserAuthority
                        'Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        'oUserAuthority.UserCode = Session(CNLoginName)
                        'oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
                        'oWebservice = New NexusProvider.ProviderManager().Provider
                        'oWebservice.GetUserAuthorityValue(oUserAuthority)
                        ''Check the user's "Claim Payment Authority"
                        'If oUserAuthority.UserAuthorityValue = 1 Or oUserAuthority.UserAuthorityValue IsNot Nothing Then
                        '    If oUserAuthority.UserAuthorityOptionalValue2 < dAmount Then
                        '        IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_Paymentuserlimit_Error") & " " & CStr(oUserAuthority.UserAuthorityOptionalValue2) & "."
                        '        args.IsValid = False
                        '    End If
                        'End If
                    End If

                ElseIf Session(CNMode) = Mode.SalvageClaim Then
                    'For Claim Salvage Receipt
                    Dim dAmount As Decimal
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                        dAmount = dAmount + oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iCount).ThisReceiptINCLTax
                    Next
                    If dAmount = 0 Then
                        IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_PaymentReceived_Error")
                        args.IsValid = False
                    Else
                        If Not ValidateReceiptBalance(iPeril) Then
                            IsPaymentReceived.IsValid = False
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("msg_OverReceipt_Error").ToString()
                            args.IsValid = False
                            Exit Sub
                        End If
                        If HidMediaTypeFieldMandatory.Value = "1" And GISLookup_MediaType.Value = "" AndAlso GISLookup_MediaType.Enabled = True Then
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_MediatypeError")
                            args.IsValid = False
                        End If
                    End If

                ElseIf Session(CNMode) = Mode.TPRecovery Then
                    'For Claim TPRecovery Receipt
                    Dim dAmount As Decimal
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                        dAmount = dAmount + oClaimOpen.ClaimPeril(iPeril).TPRecovery(iCount).ThisReceiptINCLTax
                    Next
                    If dAmount = 0 Then
                        IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_PaymentReceived_Error")
                        args.IsValid = False
                    Else
                        If Not ValidateReceiptBalance(iPeril) Then
                            IsPaymentReceived.IsValid = False
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("msg_OverReceipt_Error").ToString()
                            args.IsValid = False
                            Exit Sub
                        End If
                        If HidMediaTypeFieldMandatory.Value = "1" And GISLookup_MediaType.Value = "" AndAlso GISLookup_MediaType.Enabled = True Then
                            IsPaymentReceived.ErrorMessage = GetLocalResourceObject("lbl_MediatypeError")
                            args.IsValid = False
                        End If
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' Check MediaType Field Mandatory option after postback has been triggered by the modal dialog "PaymentDetails"
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Sub CheckMediaTypeFieldMandatory()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sMediaTypeFieldMandatory As String
            sMediaTypeFieldMandatory = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MediaTypeMandatory, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            HidMediaTypeFieldMandatory.Value = sMediaTypeFieldMandatory
            If sMediaTypeFieldMandatory <> "1" And (Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery) Then
                'Get newly added system option
                Dim oOptionSettings As NexusProvider.OptionTypeSetting
                'Pass system option for "Automate receipt generation for Salvage/Third Party receipt"
                oOptionSettings = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5117)
                'If System Option for "Automate receipt generation for Salvage/Third Party receipt" is ON then mediatype should be mandatory
                If oOptionSettings IsNot Nothing AndAlso Not String.IsNullOrEmpty(oOptionSettings.OptionValue) Then
                    If oOptionSettings.OptionValue(0) = "1" Then
                        HidMediaTypeFieldMandatory.Value = oOptionSettings.OptionValue(0)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' DisplayAccountTypeInformation
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DisplayAccountTypeInformation()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim nPartyKey As Integer = 0
            Dim oBankDetails As NexusProvider.BankCollection = Nothing
            Dim oTempBankDetailsCollection As New NexusProvider.BankCollection
            Dim oTempBankDetails As NexusProvider.Bank
            Dim oClaimOpen As NexusProvider.ClaimOpen = Session.Item(CNClaim)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oSelectedPayeeAddress As NexusProvider.Address = Nothing  'Issue126- when party is selected for payment, payee address is populated with its correspondence address

            'create a unique key and add this to viewstate
            'this will be used to cache the results of the SAM call
            Dim uPartyBankpageCacheID As Guid
            uPartyBankpageCacheID = Guid.NewGuid()
            ViewState.Add("PartyBankpageCacheID", uPartyBankpageCacheID.ToString)

            If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim _
           Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment _
           Or CType(Session(CNMode), Mode) = Mode.Authorise _
           Or CType(Session(CNMode), Mode) = Mode.DeclinePayment _
           Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                'Make visible the ddl_AccountType 
                If (Session(CNMode) = Mode.ViewClaim AndAlso Not String.IsNullOrEmpty(oClaimOpen.ClaimVersionDescription) AndAlso oClaimOpen.ClaimVersionDescription.Trim().IndexOf("Recovery") <> -1) Then
                    liAccountType.Visible = False
                Else
                    liAccountType.Visible = True
                End If
            End If

            'Retreive the party key as per selection
            Select Case rblPayee.SelectedValue
                Case "1"
                    Integer.TryParse(hPartyKey.Value, nPartyKey)
                    If nPartyKey <> 0 Then
                        Dim oSelectedBaseParty As NexusProvider.BaseParty = Nothing
                        oSelectedBaseParty = oWebService.GetParty(nPartyKey)
                        If oSelectedBaseParty IsNot Nothing And Session(CNMode) <> Mode.ViewClaim And CType(Session(CNMode), Mode) <> Mode.ViewClaimPayment And CType(Session(CNMode), Mode) <> Mode.Authorise And CType(Session(CNMode), Mode) <> Mode.DeclinePayment And CType(Session(CNMode), Mode) <> Mode.Recommend Then
                            txtParty.Text = If(oSelectedBaseParty.ShortName IsNot Nothing, oSelectedBaseParty.ShortName.Trim, "")
                            Select Case True
                                Case TypeOf oSelectedBaseParty Is NexusProvider.CorporateParty
                                    txtParty.Text = CType(oSelectedBaseParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim()
                                Case TypeOf oSelectedBaseParty Is NexusProvider.PersonalParty
                                    txtParty.Text = CType(oSelectedBaseParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim()
                                Case TypeOf oSelectedBaseParty Is NexusProvider.OtherParty
                                    txtParty.Text = oSelectedBaseParty.ShortName.Trim
                            End Select
                            'Issue126
                            If DisplayPayeeAddress(oSelectedBaseParty.Addresses) IsNot Nothing Then
                                oSelectedPayeeAddress = DisplayPayeeAddress(oSelectedBaseParty.Addresses)
                            End If
                            'getting value of ?IsDomiciledForTax? of selected other party
                            bdomiciledTax = oSelectedBaseParty.DomiciledForTax
                        End If
                    End If
                Case "2"
                    nPartyKey = oQuote.Agent

                    If Session(CNMode) <> Mode.ViewClaim And CType(Session(CNMode), Mode) <> Mode.ViewClaimPayment And CType(Session(CNMode), Mode) <> Mode.Authorise And CType(Session(CNMode), Mode) <> Mode.DeclinePayment And CType(Session(CNMode), Mode) <> Mode.Recommend Then
                        oSelectedPayeeAddress = oClaimOpen.Insurer.Address 'Issue126
                    End If
                    Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                    'checking ?IsDomiciledForTax? option of selected Agent 
                    GetAgentSettingsCall(oAgentSettings, oQuote.Agent)
                    If oAgentSettings IsNot Nothing Then
                        'getting value of ?IsDomiciledForTax? of selected Agent
                        'to do rakesh
                        bdomiciledTax = oAgentSettings.IsDomiciledForTax
                    End If

                Case "3"
                    If Session(CNParty) IsNot Nothing Then
                        Dim oBaseParty As NexusProvider.BaseParty = Session(CNParty)
                        nPartyKey = oBaseParty.Key
                        If Session(CNMode) <> Mode.ViewClaim And CType(Session(CNMode), Mode) <> Mode.ViewClaimPayment And CType(Session(CNMode), Mode) <> Mode.Authorise And CType(Session(CNMode), Mode) <> Mode.DeclinePayment And CType(Session(CNMode), Mode) <> Mode.Recommend Then
                            oSelectedPayeeAddress = oClaimOpen.Client.Address  'Issue126
                        End If
                        'getting value of ?IsDomiciledForTax? of selected Client
                        bdomiciledTax = oBaseParty.DomiciledForTax
                    End If
                Case "4"
                    Integer.TryParse(hPartyKey.Value, nPartyKey)
            End Select
            If m_sIsPaymentsReadOnly = "0" Then
                'Making of Query string with DomicileTax
                If Session(CNMode) = Mode.PayClaim Then
                    'repopulate the payment grid with IsDomiciledForTax info. 
                    Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
                    'Populating the reserve item for each peril
                    If (oClaimOpen.ClaimPeril(PerilIndex).ClaimReserve.Count = 0) Then
                        PopulateReserveItem()
                    End If
                    PopulatePayClaim(PerilIndex, oClaimOpen)
                ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    'repopulate the payment grid with IsDomiciledForTax info. 
                    Dim PerilIndex As Integer = CInt(Session(CNClaimPerilIndex))
                    PopulateSalvageClaim(PerilIndex, oClaimOpen)
                    'Set extra details to create claim receipt and cashlist items
                    oClaimOpen.ClaimPeril.Item(PerilIndex).Receipt.Payee.Address = oSelectedPayeeAddress
                    oClaimOpen.ClaimPeril.Item(PerilIndex).Receipt.Payee.MediaTypeCode = GISLookup_MediaType.Value
                End If
            End If
            'If party key is available means gretaer than 0
            'populate party bank details
            If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim _
            Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment _
            Or CType(Session(CNMode), Mode) = Mode.Authorise _
            Or CType(Session(CNMode), Mode) = Mode.DeclinePayment _
            Or CType(Session(CNMode), Mode) = Mode.Recommend Or CType(Session(CNMode), Mode) = Mode.SalvageClaim Or CType(Session(CNMode), Mode) = Mode.TPRecovery Then
                ' to do rakesh
                If nPartyKey > 0 Then
                    oBankDetails = oWebService.GetPartyBankDetails(nPartyKey)
                End If

                If Session(CNMode) = Mode.PayClaim Then
                    'Address empty
                    Address.Address1 = String.Empty
                    Address.Address2 = String.Empty
                    Address.Address3 = String.Empty
                    Address.Address4 = String.Empty
                    Address.Postcode = String.Empty

                    'Make blank each box
                    txtBankAccNumber.Text = String.Empty
                    txtPayeeName.Text = String.Empty
                    txtBankCode.Text = String.Empty
                    txtBankName.Text = String.Empty
                    txtThisReference.Text = String.Empty
                    txtOurRef.Text = String.Empty
                    txtBIC.Text = String.Empty
                    txtIBAN.Text = String.Empty
                    If m_sIsPaymentsReadOnly = "0" Then
                        txtOurRef.Text = String.Empty
                    End If
                End If

                'If oBankDetails is populated then filter the records as per BO criteria
                'to do rakesh
                If oBankDetails IsNot Nothing Then
                    For icount = 0 To oBankDetails.Count - 1
                        If (oBankDetails.Item(icount).BankPaymentTypeCode = "ANY" Or oBankDetails.Item(icount).BankPaymentTypeCode = "CLM") And oBankDetails.Item(icount).IsActive Then ' Claim Payment Type
                            oTempBankDetailsCollection.Add(oBankDetails(icount))
                        End If
                    Next
                End If
                Dim bClaimView As Boolean = False

                If (CType(Session(CNMode), Mode) = Mode.ViewClaim AndAlso Not String.IsNullOrEmpty(oClaimOpen.ClaimVersionDescription) AndAlso oClaimOpen.ClaimVersionDescription.Trim().IndexOf("Recovery") <> -1) Then
                    bClaimView = False
                Else
                    bClaimView = True
                End If

                If bClaimView Then
                    'populate the drop down list 
                    ddlAccountType.Items.Clear()
                    If oTempBankDetailsCollection.Count > 0 Then
                        ddlAccountType.DataTextField = "AccountType"
                        ddlAccountType.DataValueField = "PartyBankKey"
                        ddlAccountType.DataSource = oTempBankDetailsCollection
                        ddlAccountType.DataBind()
                    End If

                    ddlAccountType.Items.Insert(0, New ListItem("--Select Account Type--", ""))

                    'If filter records are greater than 0 then populate
                    If oTempBankDetailsCollection.Count > 0 Then
                        ddlAccountType.Enabled = True
                        GISLookup_MediaType.Enabled = True
                        ' if count is 1 then it should populated by default
                        If oTempBankDetailsCollection.Count = 1 Then
                            ' to do rakesh
                            'Set first item as selected from dropdown
                            ddlAccountType.SelectedValue = oTempBankDetailsCollection(0).PartyBankKey
                            CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment.Payee.PartyBankKey = ddlAccountType.SelectedValue

                            If txtBankAccNumber.Text = "" Then
                                txtBankAccNumber.Text = oTempBankDetailsCollection(0).AccountNumber
                            End If
                            If txtPayeeName.Text = "" Then
                                txtPayeeName.Text = oTempBankDetailsCollection(0).AccountHolderName
                            End If
                            If txtBankCode.Text = "" Then
                                txtBankCode.Text = oTempBankDetailsCollection(0).BranchCode
                            End If
                            If txtBankName.Text = "" Then
                                txtBankName.Text = oTempBankDetailsCollection(0).BankName
                            End If
                            txtBankAccNumber.Text = oTempBankDetailsCollection(0).AccountNumber
                            txtBIC.Text = oTempBankDetailsCollection(0).BIC
                            txtIBAN.Text = oTempBankDetailsCollection(0).IBAN
                            If Session(CNMode) = Mode.PayClaim Then
                                txtThisReference.Text = oTempBankDetailsCollection(0).Reference1
                                'Address population
                                Address.Address = oTempBankDetailsCollection(0).PartyBankAddress
                            End If

                            'Disabling the textboxes
                            txtBankAccNumber.ReadOnly = True
                            txtPayeeName.ReadOnly = True
                            txtBankCode.ReadOnly = True
                            txtBankName.ReadOnly = True
                            txtBIC.ReadOnly = True
                            txtIBAN.ReadOnly = True
                            ddlAccountType.Enabled = False
                        End If
                    Else
                        ddlAccountType.Enabled = False
                        GISLookup_MediaType.Value = ""
                        If Session(CNMode) = Mode.PayClaim Then
                            'Make blank each box
                            txtBankAccNumber.Text = String.Empty
                            txtPayeeName.Text = String.Empty
                            txtBankCode.Text = String.Empty
                            txtBankName.Text = String.Empty
                            txtThisReference.Text = String.Empty
                            txtOurRef.Text = String.Empty
                            txtBIC.Text = String.Empty
                            txtIBAN.Text = String.Empty
                        End If
                    End If
                End If
                'Fill Selected Client Details -'Issue126
                If oSelectedPayeeAddress IsNot Nothing Then
                    Address.Address1 = oSelectedPayeeAddress.Address1
                    Address.Address2 = oSelectedPayeeAddress.Address2
                    Address.Address3 = oSelectedPayeeAddress.Address3
                    Address.Address4 = oSelectedPayeeAddress.Address4
                    Address.CountryCode = oSelectedPayeeAddress.CountryCode
                    Address.Postcode = oSelectedPayeeAddress.PostCode
                End If


                'storing in cache
                Cache.Insert(ViewState("PartyBankpageCacheID"), oTempBankDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If

            'populate party bank details
            'to do rakesh
            If Session(CNMode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                Dim iPartyBankKey As Integer
                Integer.TryParse(hPartyBankKey.Value, iPartyBankKey)
                If oTempBankDetailsCollection IsNot Nothing AndAlso oTempBankDetailsCollection.Count > 0 Then
                    For iCount As Integer = 0 To oTempBankDetailsCollection.Count - 1
                        If oTempBankDetailsCollection(iCount).PartyBankKey = iPartyBankKey Then
                            ddlAccountType.SelectedValue = iPartyBankKey

                            If txtBankAccNumber.Text = "" Then
                                txtBankAccNumber.Text = oTempBankDetailsCollection(iCount).AccountNumber
                            End If
                            If txtPayeeName.Text = "" Then
                                txtPayeeName.Text = oTempBankDetailsCollection(iCount).AccountHolderName
                            End If
                            If txtBankCode.Text = "" Then
                                txtBankCode.Text = oTempBankDetailsCollection(iCount).BranchCode
                            End If
                            If txtBankName.Text = "" Then
                                txtBankName.Text = oTempBankDetailsCollection(iCount).BankName
                            End If
                            txtBIC.Text = oTempBankDetailsCollection(iCount).BIC
                            txtIBAN.Text = oTempBankDetailsCollection(iCount).IBAN
                            If m_sIsPaymentsReadOnly = "0" Then
                                'Disabling the textboxes
                                txtBankAccNumber.ReadOnly = True
                                txtPayeeName.ReadOnly = True
                                txtBankCode.ReadOnly = True
                                txtBankName.ReadOnly = True
                                txtThisReference.ReadOnly = True
                                txtOurRef.ReadOnly = True
                                txtBIC.ReadOnly = True
                                txtIBAN.ReadOnly = True
                            End If
                        End If
                    Next
                End If
                ddlAccountType.Enabled = False
                GISLookup_MediaType.Enabled = False
            End If

            sPartyId = nPartyKey

            'Cleaning Up
            oBankDetails = Nothing
            oTempBankDetails = Nothing
            oTempBankDetailsCollection = Nothing
        End Sub

        ''' <summary>
        ''' Display Payee CorrespondenceAddress
        ''' </summary>
        ''' <remarks></remarks>
        Private Function DisplayPayeeAddress(ByVal AddressColl As NexusProvider.AddressCollection) As NexusProvider.Address
            Dim oSelectedPayeeAddress As NexusProvider.Address
            For Each oAddress As NexusProvider.Address In AddressColl
                If oAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                    oSelectedPayeeAddress = oAddress
                    Exit For
                End If
            Next
            Return oSelectedPayeeAddress
        End Function

        Protected Sub ddlAccountType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAccountType.SelectedIndexChanged
            If ddlAccountType.SelectedValue <> "" Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                Dim oBankCollection As NexusProvider.BankCollection = CType(Cache.Item(ViewState("PartyBankpageCacheID")), NexusProvider.BankCollection)

                If oBankCollection Is Nothing Then
                    DisplayAccountTypeInformation()
                    oBankCollection = CType(Cache.Item(ViewState("PartyBankpageCacheID")), NexusProvider.BankCollection)
                End If

                If ddlAccountType.SelectedValue <> "" Then 'If oBankCollection Is Nothing, ddlAccountType is reset
                    If oBankCollection IsNot Nothing Then
                        With oBankCollection
                            For i = 0 To oBankCollection.Count - 1
                                If .Item(i).PartyBankKey = ddlAccountType.SelectedValue Then
                                    txtBankAccNumber.Text = .Item(i).AccountNumber
                                    txtPayeeName.Text = .Item(i).AccountHolderName
                                    txtBankCode.Text = .Item(i).BranchCode
                                    txtBankName.Text = .Item(i).BankName
                                    txtThisReference.Text = .Item(i).Reference1
                                    txtBIC.Text = .Item(i).BIC
                                    txtIBAN.Text = .Item(i).IBAN

                                    'Address population
                                    Address.Address = .Item(i).PartyBankAddress

                                    'Disabling the textboxes
                                    txtBankAccNumber.ReadOnly = True
                                    txtPayeeName.ReadOnly = True
                                    txtBankCode.ReadOnly = True
                                    txtBankName.ReadOnly = True
                                    txtBIC.ReadOnly = True
                                    txtIBAN.ReadOnly = True
                                    CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment.Payee.PartyBankKey = ddlAccountType.SelectedValue
                                    'CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.PartyBankKey()
                                End If
                            Next
                        End With
                    End If
                End If
                'Cleaning Up
                oWebService = Nothing
                oParty = Nothing
                oBankCollection = Nothing
            Else
                'Enabling the textboxes
                txtBankAccNumber.ReadOnly = False
                txtPayeeName.ReadOnly = False
                txtBankCode.ReadOnly = False
                txtBankName.ReadOnly = False
                txtThisReference.ReadOnly = False
                txtBIC.ReadOnly = False
                txtIBAN.ReadOnly = False

                'Address empty
                Address.Address1 = String.Empty
                Address.Address2 = String.Empty
                Address.Address3 = String.Empty
                Address.Address4 = String.Empty
                Address.Postcode = String.Empty

                txtBankAccNumber.Text = String.Empty
                txtPayeeName.Text = String.Empty
                txtBankCode.Text = String.Empty
                txtBankName.Text = String.Empty
                txtThisReference.Text = String.Empty
                txtOurRef.Text = String.Empty
                txtBIC.Text = String.Empty
                txtIBAN.Text = String.Empty
            End If
        End Sub

        Protected Sub CustVldProductRiskMaintenance_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldProductRiskMaintenance.ServerValidate
            If Session(CNMode) = Mode.PayClaim Then
                'Check ProductRiskMaintenance Paymentcannotexceedreserve  option
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oPaymentCannotExceedReserve As String
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))

                Dim claimsReservesAreGross As NexusProvider.OptionTypeSetting
                claimsReservesAreGross = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ClaimsReserveForGross)

                'finding value of PaymentCannotExceedReserve
                oPaymentCannotExceedReserve = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.PaymentCannotExceedReserve, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                If String.IsNullOrEmpty(oPaymentCannotExceedReserve) = False AndAlso oPaymentCannotExceedReserve = "1" Then
                    m_sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)

                    Dim bValidatePayment As Boolean = True
                    For iCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count - 1
                        If iCount = iPeril Then
                            For jCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve.Count - 1
                                If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).BaseReserveKey <> 0 _
                                AndAlso CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).IsExcess = False Then
                                    Dim dTotalPaid As Decimal = 0.0

                                    If m_sIsPaymentsReadOnly = "1" Then
                                        'AS PAYMENT IS MADE VIA SCRIPT SO PAID TO DATE WILL INCLUDE THIS PAYMENT AS WELL
                                        dTotalPaid = Format(CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).PaidToDate, "#.00")
                                    Else
                                        dTotalPaid = Format(CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).PaidToDate + (CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).ThisPaymentINCLTax - CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).ThisPaymentTax), "#.00")
                                    End If
                                    If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).TotalReserve > 0 Then
                                        If claimsReservesAreGross.OptionValue = "1" Then

                                            Dim totalPaidInclThisPayment As Decimal = 0.0
                                            Dim totalReserve As Decimal = 0.0
                                            totalPaidInclThisPayment = Format(CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).PaidToDate +
                                                                              CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).PaidToDateTax +
                                                                              (CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).ThisPaymentINCLTax), "#.00")
                                            totalReserve = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).TotalReserve +
                                                CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).RevisedTaxReserve +
                                                CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).PaidToDateTax

                                            Dim dTolerance As Double = 0.05
                                            If Math.Round(totalPaidInclThisPayment, 2) > Math.Round(totalReserve, 2) + dTolerance Then
                                                bValidatePayment = False
                                                Exit For
                                            End If
                                        Else
                                            If Math.Round(dTotalPaid, 2) > Math.Round((CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).TotalReserve), 2) Then
                                                bValidatePayment = False
                                                Exit For
                                            End If
                                        End If
                                    ElseIf CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).TotalReserve = 0 Then
                                        If Math.Round(dTotalPaid, 2) <> 0 Then
                                            bValidatePayment = False
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next
                            'Check Netpayment amount is greater than balance amount
                            If bValidatePayment = False Then
                                args.IsValid = False
                                'Display Paymentcannotexceedreserve error message
                                CustVldProductRiskMaintenance.ErrorMessage = GetLocalResourceObject("lbl_PaymentCannotExceedReserveConfirmMsg")
                                Exit Sub
                            End If
                        End If
                    Next
                End If


            End If
        End Sub
        Protected Sub CustAllowMultipleClaimPayment_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustAllowMultipleClaimPayment.ServerValidate
            If Session(CNMode) = Mode.PayClaim AndAlso Session(CNMaxClaimPaymentValue) IsNot Nothing Then
                Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))

                'Check Session variable (set in perils.ascx) payment amount is greater than payment amount enter on screen
                'calculate total  payment amount
                Dim dTotalPaymentPaid As Decimal
                For iCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count - 1
                    If iCount = iPeril Then

                        For jCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve.Count - 1
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).BaseReserveKey <> 0 _
                            AndAlso CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).IsExcess = False Then
                                dTotalPaymentPaid += CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).PaidToDate + (CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).ThisPaymentINCLTax - CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReserve(jCount).ThisPaymentTax)
                            End If
                        Next
                    End If
                Next
                Dim dMaxClaimPaymentValue As Decimal
                Decimal.TryParse(Session(CNMaxClaimPaymentValue), dMaxClaimPaymentValue)
                If dMaxClaimPaymentValue < dTotalPaymentPaid Then
                    args.IsValid = False
                    Exit Sub
                End If
            End If
        End Sub

        Protected Sub chkExGratia_CheckedChanged(sender As Object, e As EventArgs) Handles chkExGratia.CheckedChanged
            Dim oExGratiaOptionSettings As NexusProvider.OptionTypeSetting
            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
            Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)

            If chkExGratia.Checked Then
                oExGratiaOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5114)

                If (oExGratiaOptionSettings IsNot Nothing AndAlso String.IsNullOrEmpty(oExGratiaOptionSettings.OptionValue) = False) Then
                    oAccountSearchCriteria.ShortCode = oExGratiaOptionSettings.OptionValue
                    oAccountSearchResultCollection = oWebService.FindAccounts(oAccountSearchCriteria)
                    If oAccountSearchResultCollection IsNot Nothing AndAlso oAccountSearchResultCollection.Count > 0 Then
                        If CType(Session(CNMode), Mode) = Mode.PayClaim AndAlso oClaimOpen IsNot Nothing AndAlso oClaimOpen.ClaimPeril(iPeril).Payment IsNot Nothing Then
                            oClaimOpen.ClaimPeril(iPeril).Payment.IsExGratia = chkExGratia.Checked
                        End If
                    Else
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid.ToString, "alert('Ex-gratia Expense Account does not exist. Please contact your System Administrator.');", True)
                        chkExGratia.Checked = False
                        Return
                    End If
                End If
            End If
        End Sub
        Protected Sub PopulateViewRecoveryClaim(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)
            Dim oPerilRecoveryCollection As NexusProvider.PerilRecoveryCollection
            Dim tempPerilRecovery As NexusProvider.PerilRecovery
            Dim oPerilClaimReceiptCollection As NexusProvider.ClaimReceiptCollection

            Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
            Dim bIsSalvage As Boolean = False

            If oClaimOpen.ClaimVersionDescription.Trim().IndexOf("Salvage") <> -1 Then
                bIsSalvage = True
            Else
                bIsSalvage = False
            End If
            visibleGrid()
            gvSalvageDetails.Visible = True
            ltPageHeading.Text = ""
            rblPayee.Items(0).Text = GetLocalResourceObject("li_ClaimReceivable")
            ltThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
            If oPerilRecoveryCollection Is Nothing Then
                oPerilRecoveryCollection = New NexusProvider.PerilRecoveryCollection
                oPerilClaimReceiptCollection = New NexusProvider.ClaimReceiptCollection
                gvSalvageDetails.Visible = True
                Dim receiptCount As Integer = 0
                For iCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count - 1
                    If iCount = iPeril Then
                        For jCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReceipt.Count - 1
                            If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReceipt IsNot Nothing Then
                                oPerilClaimReceiptCollection.Add(CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iCount).ClaimReceipt(jCount))
                            End If
                        Next
                    End If
                Next
            End If


            Dim amount As Decimal = 0.0
            Dim tax As Decimal = 0.0

            For iCount As Integer = 0 To oPerilClaimReceiptCollection.Count - 1
                If (oPerilClaimReceiptCollection.Item(iCount).ClaimReceiptKey = oPerilClaimReceiptCollection.Item(iCount).BaseClaimReceiptKey) Then
                    For jCount As Integer = 0 To oPerilClaimReceiptCollection.Item(iCount).ClaimReceiptItem.Count - 1
                        tempPerilRecovery = New NexusProvider.PerilRecovery
                        GetRecoveryDetail(tempPerilRecovery, NexusProvider.ListType.PMLookup, CType(oPerilClaimReceiptCollection.Item(iCount), NexusProvider.ClaimReceipt).ClaimReceiptItem.Item(jCount).RecoveryTypeCode, "recovery_type", bIsSalvage) 'oPerilRecoveryTotal.Item(jCount).TypeCode
                        tempPerilRecovery.LossThisReceiptINCLTax = CType(oPerilClaimReceiptCollection.Item(iCount), NexusProvider.ClaimReceipt).ClaimReceiptItem.Item(jCount).ReceiptAmount
                        tempPerilRecovery.LossThisReceiptTax = CType(oPerilClaimReceiptCollection.Item(iCount), NexusProvider.ClaimReceipt).ClaimReceiptItem.Item(jCount).TaxAmount
                        tempPerilRecovery.BaseRecoveryKey = CType(oPerilClaimReceiptCollection.Item(iCount), NexusProvider.ClaimReceipt).ClaimReceiptItem.Item(jCount).BaseRecoveryKey
                        If hSalvageAndTPRecoveryReservesExcludeTax.Value = "1" Then
                            tempPerilRecovery.LossThisNet = tempPerilRecovery.LossThisReceiptINCLTax + tempPerilRecovery.LossThisReceiptTax
                        Else
                            tempPerilRecovery.LossThisNet = tempPerilRecovery.LossThisReceiptINCLTax - tempPerilRecovery.LossThisReceiptTax
                        End If

                        If CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                            If oPerilClaimReceiptCollection.Item(jCount).BaseClaimReceiptKey = oPerilClaimReceiptCollection.Item(jCount).ClaimReceiptKey Then
                                oPerilRecoveryCollection.Add(tempPerilRecovery)
                            End If
                        Else
                            oPerilRecoveryCollection.Add(tempPerilRecovery)
                        End If
                    Next
                End If
            Next
            pnlThisPaymentTab.Visible = True
            txtTotalWHTax.Text = "0.00"
            txtGrossPayment.Text = String.Format("{0:N2}", amount)
            txtTotalTax.Text = String.Format("{0:N2}", tax)
            txtNetPayment.Text = String.Format("{0:N2}", (CDbl(txtGrossPayment.Text.Trim) - tax))
            gvSalvageDetails.Visible = True
            'store the data in ViewState to use again for page indexing
            gvSalvageDetails.DataSource = oPerilRecoveryCollection
            gvSalvageDetails.DataBind()
            PopulateViewThisRecovery(iPeril, oClaimOpen)
        End Sub

        Protected Sub PopulateViewThisRecovery(ByVal iPerilIndex As Integer, ByVal oClaimOpen As NexusProvider.ClaimOpen)
            '*****-----------------
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            Dim amount As Decimal
            Dim tax As Decimal
            Dim iCount As Integer

            If oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt IsNot Nothing Then
                For iCount = 0 To oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt.Count - 1
                    'Calculation of the amount and tax
                    With oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount)
                        If .ClaimReceiptKey = .BaseClaimReceiptKey Then
                            If .ClaimReceiptItem IsNot Nothing Then
                                For jCount As Integer = 0 To .ClaimReceiptItem.Count - 1
                                    If CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                                        If .ClaimReceiptItem.Item(jCount).ClaimReceiptItemKey = .ClaimReceiptItem.Item(jCount).BaseClaimReceiptItemKey Then
                                            amount += .ClaimReceiptItem(jCount).ReceiptAmount
                                            tax += .ClaimReceiptItem(jCount).TaxAmount
                                        End If
                                    Else
                                        amount += .ClaimReceiptItem(jCount).ReceiptAmount
                                        tax += .ClaimReceiptItem(jCount).TaxAmount
                                    End If
                                Next
                            End If

                            If .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLMRECEIVABLE Or .PartyKey = 0 Then
                                rblPayee.SelectedValue = "0"
                                txtParty.Text = "CLMPAYABLE"
                                .ReceiptPartyType = 0
                            ElseIf .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.PARTY Then
                                rblPayee.SelectedValue = "1"
                                txtParty.Text = .PartyReceiptCode
                                btnParty.Enabled = False
                                hPartyKey.Value = .PartyKey
                            ElseIf .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.AGENT Then
                                rblPayee.SelectedValue = "2"
                                If oClaimOpen.Insurer IsNot Nothing Then
                                    txtParty.Text = oClaimOpen.Insurer.ContactName
                                End If
                                .PartyKey = 0
                            ElseIf .ReceiptPartyType = NexusProvider.ClaimReceiptPartyTypeType.CLIENT Then
                                rblPayee.SelectedValue = "3"
                                txtParty.Text = oQuote.InsuredName
                                .PartyKey = 0

                            End If
                            'Set the Media Type
                            If oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).Payee IsNot Nothing Then

                                If String.IsNullOrEmpty(oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).Payee.MediaTypeCode) = False Then
                                    GISLookup_MediaType.Value = oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).Payee.MediaTypeCode
                                End If
                                If String.IsNullOrEmpty(oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).Payee.MediaReference) = False Then
                                    txtMediaRef.Text = oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).Payee.MediaReference
                                End If
                            End If
                            'if tax is grater than 0
                            If tax > 0 Then
                                Try
                                    'Sam call to retreive the tax details
                                    oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).TaxItem = New NexusProvider.TaxItemTypeCollection
                                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                    With oClaimOpen.ClaimPeril(iPerilIndex)
                                        .ClaimReceipt(iCount).ClaimKey = oClaimOpen.ClaimKey
                                        .ClaimReceipt(iCount).BaseClaimKey = oClaimOpen.BaseClaimKey
                                        .ClaimReceipt(iCount).BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPerilIndex).BaseClaimPerilKey
                                        .ClaimReceipt(iCount).ClaimPerilKey = oClaimOpen.ClaimPeril(iPerilIndex).ClaimPerilKey
                                        oWebservice.GetClaimReceiptTaxes(.ClaimReceipt(iCount), oQuote.BranchCode)
                                    End With
                                Finally
                                    'oWebservice = Nothing
                                End Try
                                'populating the grid based on results returned from sam
                                gvTaxesonThisReceipt.Visible = True
                                gvTaxesonThisReceipt.DataSource = oClaimOpen.ClaimPeril(iPerilIndex).ClaimReceipt(iCount).TaxItem
                                gvTaxesonThisReceipt.DataBind()
                            Else
                                gvTaxesonThisReceipt.Visible = False
                            End If
                            txtTotalWHTax.Text = "0.00"
                            If amount = 0 Then
                                'need to show the static value in decimal
                                txtGrossPayment.Text = "0.00"
                                txtTotalTax.Text = "0.00"
                                txtNetPayment.Text = "0.00"
                                If CType(Session.Item(CNMode), Mode) <> Mode.ViewClaim Then
                                    gvSalvageDetails.Columns(8).Visible = False
                                End If
                            Else
                                'if amount is not 0
                                pnlThisPaymentTab.Visible = True
                                txtGrossPayment.Text = Math.Round((amount), 2)
                                txtTotalTax.Text = Math.Round(tax, 2)
                                If hSalvageAndTPRecoveryReservesExcludeTax.Value = "1" Then
                                    txtNetPayment.Text = Format(Math.Round((CDbl(txtGrossPayment.Text.Trim) + tax), 2), "#0.00")
                                Else
                                    txtNetPayment.Text = Format(Math.Round((CDbl(txtGrossPayment.Text.Trim) - tax), 2), "#0.00")
                                End If
                            End If

                            If .Payee IsNot Nothing Then
                                Address.Address1 = .Payee.Address.Address1
                                Address.Address2 = .Payee.Address.Address2
                                Address.Address3 = .Payee.Address.Address3
                                Address.Address4 = .Payee.Address.Address4
                                Address.CountryCode = .Payee.Address.CountryCode
                                Address.Postcode = .Payee.Address.PostCode
                            End If
                            If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim AndAlso .Payee IsNot Nothing Then
                                GISLookup_MediaType.Value = .Payee.MediaTypeCode
                                txtBankAccNumber.Text = .Payee.BankNumber
                                txtPayeeName.Text = .Payee.Name
                                txtBankCode.Text = .Payee.BankCode
                                txtBankName.Text = .Payee.BankName
                                txtMediaRef.Text = .Payee.MediaReference
                                txtOurRef.Text = .Payee.TheirReference
                                txtComments.Text = .Payee.Comments
                            End If
                            'Set The label for Receipt
                            lblThisPayment.Text = GetLocalResourceObject("ltThisReceipt")
                            ThisPaymentSummary.Text = GetLocalResourceObject("ThisReceiptSummary")
                            ltGrossPayment.Text = GetLocalResourceObject("ltGrossReceipt")
                            ltNetPayment.Text = GetLocalResourceObject("ltNetReceipt")
                            lblTaxesOnThisPayment.Text = GetLocalResourceObject("ltTaxesOnThisReceipt")
                            lblPaymentDetails.Text = GetLocalResourceObject("ltReceiptDetails")
                            liChequeDate.Visible = False
                            liThisReference.Visible = False
                            divAddress.Visible = False

                        End If
                    End With
                Next
            End If

        End Sub

        Public Function GetRecoveryDetail(ByRef tempPerilRecovery As NexusProvider.PerilRecovery, ByVal v_oListType As NexusProvider.ListType,
                                           ByVal v_sCodeValue As String, ByVal v_sListCode As String, ByVal v_bIsSalvage As String, Optional ByVal v_sBranchCode As String = Nothing) As String

            'Dim r_oList() As Object = Nothing
            Dim sDescription As String = Nothing
            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim r_oList As NexusProvider.LookupListCollection
            r_oList = owebservice.GetList(v_oListType, v_sListCode, True, False, , , v_sBranchCode)
            'List.GetList(GetUserToken(), STSListType.PMLookup, v_sBranchCode, r_oList, v_sListCode)

            If Not r_oList Is Nothing Then
                For Each tmpRow As Object In r_oList
                    If Trim(tmpRow.Code) = Trim(v_sCodeValue) Then
                        tempPerilRecovery.Description = tmpRow.Description
                        Dim iPeril As Integer = CInt(Session(CNClaimPerilIndex))
                        Dim oPerilRecoveryTotal As NexusProvider.PerilRecoveryCollection =
                        CType(Cache.Item(ViewState(CNClaimPerilRecoveryCollection)), NexusProvider.PerilRecoveryCollection)
                        If v_bIsSalvage Then
                            oPerilRecoveryTotal = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery
                        Else
                            oPerilRecoveryTotal = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery
                        End If
                        For iCount As Integer = 0 To oPerilRecoveryTotal.Count - 1
                            If tmpRow.Code.ToString().Trim() = oPerilRecoveryTotal.Item(iCount).TypeCode.Trim() Then
                                tempPerilRecovery.TotalRecovery = oPerilRecoveryTotal.Item(iCount).TotalRecovery
                                tempPerilRecovery.ReceiptedAmount = oPerilRecoveryTotal.Item(iCount).ReceiptedAmount
                                tempPerilRecovery.ReceiptedTaxAmount = oPerilRecoveryTotal.Item(iCount).ReceiptedTaxAmount
                            End If
                        Next
                        Exit For
                    End If
                Next
            End If


            Return sDescription

        End Function

        Private Sub visibleGrid()
            If CType(Session(CNMode), Mode) <> Mode.PayClaim Then
                gvSalvageDetails.Enabled = False
                gvPaymentDetails.Enabled = False
            End If
        End Sub

        Private Sub SetThisPayeeAddress()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim iPartyKey As Integer = 0
            Dim oBankDetails As NexusProvider.BankCollection = Nothing
            Dim oTempBankDetailsCollection As New NexusProvider.BankCollection
            ' Dim oTempBankDetails As NexusProvider.Bank
            Dim oClaimOpen As NexusProvider.ClaimOpen = Session.Item(CNClaim)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oSelectedPayeeAddress As NexusProvider.Address = Nothing

            Select Case rblPayee.SelectedValue
                Case "1"
                    Integer.TryParse(hPartyKey.Value, iPartyKey)
                    If iPartyKey <> 0 Then
                        Dim oSelectedBaseParty As NexusProvider.BaseParty = Nothing
                        oSelectedBaseParty = oWebService.GetParty(iPartyKey)
                        If oSelectedBaseParty IsNot Nothing Then
                            'Issue126
                            If DisplayPayeeAddress(oSelectedBaseParty.Addresses) IsNot Nothing Then
                                oSelectedPayeeAddress = DisplayPayeeAddress(oSelectedBaseParty.Addresses)
                            End If
                            'getting value of ???IsDomiciledForTax??? of selected other party
                            bdomiciledTax = oSelectedBaseParty.DomiciledForTax
                        End If
                    End If
                Case "2"
                    iPartyKey = oQuote.Agent
                    'getting value of ???IsDomiciledForTax??? of selected Agent
                    oSelectedPayeeAddress = oClaimOpen.Insurer.Address 'Issue126

                    Dim oAgentSettings As NexusProvider.AgentSettings = Nothing
                    'checking ???IsDomiciledForTax??? option of selected Agent 
                    oAgentSettings = oWebService.GetAgentSettings(oQuote.Agent)
                    If oAgentSettings IsNot Nothing Then
                        'getting value of ???IsDomiciledForTax??? of selected Agent
                        'to do rakesh
                        'bdomiciledTax = oAgentSettings.IsDomiciledForTax
                    End If

                Case "3"
                    If Session(CNParty) IsNot Nothing Then
                        Dim oBaseParty As NexusProvider.BaseParty = Session(CNParty)
                        iPartyKey = oBaseParty.Key
                        oSelectedPayeeAddress = oClaimOpen.Client.Address  'Issue126
                        'getting value of ???IsDomiciledForTax??? of selected Client
                        bdomiciledTax = oBaseParty.DomiciledForTax
                    End If
                Case "4"
                    Integer.TryParse(hPartyKey.Value, iPartyKey)
            End Select

            'Fill Selected Client Details -'Issue126
            If oSelectedPayeeAddress IsNot Nothing Then
                Address.Address1 = oSelectedPayeeAddress.Address1
                Address.Address2 = oSelectedPayeeAddress.Address2
                Address.Address3 = oSelectedPayeeAddress.Address3
                Address.Address4 = oSelectedPayeeAddress.Address4
                Address.CountryCode = oSelectedPayeeAddress.CountryCode
                Address.Postcode = oSelectedPayeeAddress.PostCode
            End If

            oWebService = Nothing
            oBankDetails = Nothing
            oTempBankDetailsCollection = Nothing
            oClaimOpen = Nothing
            oQuote = Nothing
            oSelectedPayeeAddress = Nothing
        End Sub

        Function IsDomiciledForTax(ByVal iPartyKey As Integer) As Boolean
            Dim bIsDomiciledForTax As Boolean = False
            If iPartyKey = 0 Then
                Return bIsDomiciledForTax
            End If

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oSelectedBaseParty As NexusProvider.BaseParty = Nothing

            oSelectedBaseParty = oWebService.GetParty(iPartyKey)
            bIsDomiciledForTax = oSelectedBaseParty.DomiciledForTax
            oWebService = Nothing
            oSelectedBaseParty = Nothing

            Return bIsDomiciledForTax
        End Function
        Protected Sub ddlPayment_To_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPayment_To.SelectedIndexChanged

            Dim oLookUP As New NexusProvider.LookupListCollection
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            Dim iIsClaimPayable, iIsParty, iIsAgent, iIsClient As Integer
            Dim oClaimPayment As NexusProvider.ClaimPayment = Nothing
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            'sam call to retreive the list of branch from table source
            If v_sOptionList Is Nothing Then
                oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Claim_Payment_To", True, False, , , , v_sOptionList)
            End If
            'Load the xml element 
            If v_sOptionList IsNot Nothing Then
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument()
                xmlDoc.LoadXml(sXML)
                If xmlDoc.ChildNodes IsNot Nothing Then
                    For iCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes.Count - 1

                        For iChildCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes.Count - 1
                            Dim ST As String
                            ST = xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText.Trim.ToUpper
                            If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText.Trim.ToUpper = ddlPayment_To.SelectedValue.ToUpper Then
                                For iChildCount1 As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes.Count - 1

                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).Name.Trim.ToUpper = "IS_TO_CLAIM_PAYABLE" Then
                                        iIsClaimPayable = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).InnerText)
                                    End If
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).Name.Trim.ToUpper = "IS_TO_PARTY" Then
                                        iIsParty = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).InnerText)
                                    End If
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).Name.Trim.ToUpper = "IS_TO_AGENT" Then
                                        iIsAgent = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).InnerText)
                                    End If
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).Name.Trim.ToUpper = "IS_TO_CLIENT" Then
                                        iIsClient = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount1).InnerText)
                                    End If

                                Next

                            End If
                        Next
                    Next
                End If
            End If

            If iIsClaimPayable = 1 Then
                payeeEnableDisabled()
                rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Claim Payable"))).Enabled = True
            Else
                rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Claim Payable"))).Enabled = False
            End If

            If iIsParty = 1 Then
                payeeEnableDisabled()
                rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Party"))).Enabled = True
            Else
                rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Party"))).Enabled = False
            End If
            If iIsAgent = 1 Then
                If rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Agent"))).Enabled = False Then
                    payeeEnableDisabled()
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Policy Holder"))).Enabled = False
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Party"))).Enabled = False
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Claim Payable"))).Enabled = False
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Agent"))).Enabled = False
                    Exit Sub
                Else
                    payeeEnableDisabled()
                    rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Agent"))).Enabled = True
                End If
            End If

            If iIsClient = 1 Then
                payeeEnableDisabled()
                rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Policy Holder"))).Enabled = True
            Else
                rblPayee.Items(rblPayee.Items.IndexOf(rblPayee.Items.FindByText("Policy Holder"))).Enabled = False
            End If

        End Sub

        Private Sub payeeEnableDisabled()
            chk_DomiciledITA.Checked = False

            rblPayee.Items(0).Selected = False
            rblPayee.Items(1).Selected = False
            rblPayee.Items(2).Selected = False
            rblPayee.Items(3).Selected = False
            txtParty.Text = String.Empty
            btnParty.Enabled = False
        End Sub

        Protected Sub chk_DomiciledITA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_DomiciledITA.CheckedChanged
            If chk_DomiciledITA.Checked Then
                txtPercentageITA.Text = "0.00"
                lbl_PercentageITA.Font.Bold = True
                lbl_TaxnoITA.Font.Bold = True


                chk_DomicilePTA.Enabled = False
                txtPercentagePTA.Enabled = False
                txtTaxNoPTA.Enabled = False
            Else
                txtPercentageITA.Text = ""
                lbl_PercentageITA.Font.Bold = False
                lbl_TaxnoITA.Font.Bold = False

                chk_DomicilePTA.Enabled = True
                txtPercentagePTA.Enabled = True
                txtTaxNoPTA.Enabled = True
            End If
        End Sub


        Protected Sub chk_DomicilePTA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_DomicilePTA.CheckedChanged
            If chk_DomicilePTA.Checked = True Then
                chk_DomiciledITA.Enabled = False
                txtPercentageITA.Enabled = False
                txtTaxnoITA.Enabled = False
                lbl_PercentageITA.Font.Bold = False
                lbl_TaxnoITA.Font.Bold = False

                txtPercentagePTA.Text = "0.00"
                txtTaxNoPTA.Text = ""

            Else
                chk_DomiciledITA.Enabled = True
                txtPercentageITA.Enabled = True
                txtTaxnoITA.Enabled = True
                txtTaxNoPTA.Text = ""
                txtPercentagePTA.Text = ""
                txtTaxNoPTA.Text = ""
                txtTaxnoITA.Text = ""
                lbl_PercentageITA.Font.Bold = False
                lbl_TaxnoITA.Font.Bold = False
            End If

        End Sub

        '11340 for Recovery
        Protected Sub chk_TaxexemptRTS_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_TaxexemptRTS.CheckedChanged
            Dim sOption As String = HidATSoption.Value
            If sOption = "1" AndAlso (Session(CNMode) = Mode.TPRecovery) Then
                If chk_TaxexemptRTS.Checked Then
                    chk_DomicileITA.Enabled = False
                    txtPercentage_ITA.Enabled = False
                    txtTaxNo_ITA.Enabled = False

                    chk_TaxexemptRTS.Enabled = True
                    txtPercentageRTS.Enabled = True
                Else
                    chk_DomicileITA.Enabled = True
                    txtPercentage_ITA.Enabled = True
                    txtTaxNo_ITA.Enabled = True
                    txtTaxnoITA.Text = ""

                    chk_TaxexemptRTS.Enabled = True
                    txtPercentageRTS.Enabled = True
                End If
            End If
        End Sub

        Protected Sub chk_DomicileITA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_DomicileITA.CheckedChanged
            Dim sOption As String = HidATSoption.Value
            If sOption = "1" AndAlso (Session(CNMode) = Mode.TPRecovery) Then
                If chk_DomicileITA.Checked Then
                    txtTaxNo_ITA.Enabled = True
                    txtPercentage_ITA.Enabled = True

                    chk_TaxexemptRTS.Enabled = False
                    txtPercentageRTS.Enabled = False
                Else
                    txtTaxNo_ITA.Enabled = True
                    txtPercentage_ITA.Enabled = True

                    chk_TaxexemptRTS.Enabled = True
                    txtPercentageRTS.Enabled = True
                End If
            End If
        End Sub

        Protected Sub CustTaxnoITA_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustTaxnoITA.ServerValidate
            If Session(CNMode) = Mode.PayClaim Then
                If chk_DomiciledITA.Checked AndAlso txtTaxnoITA.Text.ToString.Length = 0 AndAlso txtPercentageITA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoITA.ErrorMessage = GetLocalResourceObject("msg_ReqPerTaxnoITA")

                ElseIf chk_DomiciledITA.Checked AndAlso txtTaxnoITA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoITA.ErrorMessage = GetLocalResourceObject("msg_ReqTaxnoITA")

                ElseIf chk_DomiciledITA.Checked AndAlso txtPercentageITA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoITA.ErrorMessage = GetLocalResourceObject("msg_ReqPerITA")

                Else
                    args.IsValid = True
                End If

            ElseIf Session(CNMode) = Mode.TPRecovery OrElse Session(CNMode) = Mode.SalvageClaim Then
                If chk_DomicileITA.Checked AndAlso txtTaxNo_ITA.Text.ToString.Length = 0 AndAlso txtPercentage_ITA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoITA.ErrorMessage = GetLocalResourceObject("msg_ReqPerTaxnoITA")

                ElseIf chk_DomicileITA.Checked AndAlso txtTaxNo_ITA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoITA.ErrorMessage = GetLocalResourceObject("msg_ReqTaxnoITA")

                ElseIf chk_DomicileITA.Checked AndAlso txtPercentage_ITA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoITA.ErrorMessage = GetLocalResourceObject("msg_ReqPerITA")

                Else
                    args.IsValid = True
                End If
            Else
                args.IsValid = True
            End If

        End Sub

        Protected Sub CustTaxnoPTA_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustTaxnoPTA.ServerValidate
            If Session(CNMode) = Mode.PayClaim Then
                If chk_DomicilePTA.Checked AndAlso txtTaxNoPTA.Text.ToString.Length = 0 AndAlso txtPercentagePTA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoPTA.ErrorMessage = GetLocalResourceObject("msg_ReqPerTaxnoPTA")

                ElseIf chk_DomicilePTA.Checked AndAlso txtTaxNoPTA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoPTA.ErrorMessage = GetLocalResourceObject("msg_ReqTaxnoPTA")

                ElseIf chk_DomicilePTA.Checked AndAlso txtPercentagePTA.Text.ToString.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustTaxnoPTA.ErrorMessage = GetLocalResourceObject("msg_ReqPerPTA")
                Else
                    args.IsValid = True
                End If
            Else
                args.IsValid = True
            End If
        End Sub

        Protected Sub CustPerRTS_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustPerRTS.ServerValidate
            If Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                If chk_TaxexemptRTS.Checked AndAlso txtPercentageRTS.Text.Length = 0 Then
                    args.IsValid = False
                    CType(Me.Parent.FindControl("hfRememberTabs"), HiddenField).Value = 2
                    CustPerRTS.ErrorMessage = GetLocalResourceObject("msg_ReqPerRTS")
                Else
                    args.IsValid = True
                End If
            Else
                args.IsValid = True
            End If
        End Sub
        Private Sub custCLMPAYBankValidation_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custCLMPAYBankValidation.ServerValidate
            If Session(CNMode) = Mode.PayClaim AndAlso rblPayee.Items(0).Selected Then
                Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
                Dim dAmount As Decimal = 0.0
                Dim sAccountNumber As String
                If oClaimReserve IsNot Nothing Then
                    For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In oClaimReserve
                        dAmount += oPaymentItem.CostToClaim
                    Next
                End If
                If dAmount <> 0 Then


                    Dim oValidationDetailsCollection As NexusProvider.ValidationDetailsCollection
                    Dim iBankMediaId As Integer = 0
                    'Account Number should be passed as BankBranchCode|AccountNumber
                    If CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Tasks.Task("ClaimPayableAvailable") IsNot Nothing Then
                        sAccountNumber = txtBankCode.Text & "|" & txtBankAccNumber.Text & "|" & txtAccountType.Text
                    Else
                        sAccountNumber = txtBankCode.Text & "|" & txtBankAccNumber.Text & "|" & ""
                    End If
                    Dim iCountryID As Integer = 0 'GetKeyForDescription(NexusProvider.ListType.PMLookup, GISCountry.Text, "Country")
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim sMediaTypeCode As String = GISLookup_MediaType.Value
                    sMediaTypeCode += "_CLMPAYABLE"

                    'Validating Duplicate Account Type 
                    'Start
                    Dim oBankDetails As New NexusProvider.BankCollection
                    Dim bAccountTypeValidateion As Boolean = True

                    oValidationDetailsCollection = oWebService.ValidateBankAccountNumber(iBankMediaId, iCountryID, sAccountNumber, sMediaTypeCode, sBIC:=txtBIC.Text, sIBAN:=txtIBAN.Text, sBankName:=txtBankName.Text)

                    If oValidationDetailsCollection(0).IsValid Then
                        args.IsValid = True
                        'CType(Me.Parent.FindControl("btn_Next"), LinkButton).Attributes.Add("onclick", "javascript: alert('mera code chal gaya');")
                    Else
                        'If the Account Number is Invalid
                        args.IsValid = False
                        If oValidationDetailsCollection(0).ValidationMessageDataset <> "" AndAlso oValidationDetailsCollection(0).ValidationMessageDataset IsNot Nothing Then
                            'If the Collection returna any message from Back office Script
                            custCLMPAYBankValidation.ErrorMessage = oValidationDetailsCollection(0).ValidationMessageDataset
                        Else
                            'If the Collection does not returns any BO script message, and IsValid key is false, then a custom message is passed 
                            custCLMPAYBankValidation.ErrorMessage = GetLocalResourceObject("msg_CLMPAYBankValidationFailed").ToString()
                        End If
                    End If
                    'cleaning up
                    oWebService = Nothing
                    oValidationDetailsCollection = Nothing
                End If
            End If
        End Sub

        ''' <summary>
        ''' Custom Validator to trigger bank account validations
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub CustVldValidateAccountNumber_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldValidateAccountNumber.ServerValidate

            If rblPayee.SelectedIndex = 0 AndAlso GISLookup_MediaType.Value <> "" Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oValidationDetailsCollection As NexusProvider.ValidationDetailsCollection
                Dim oLookUP As New NexusProvider.LookupListCollection

                Dim nBankMediaId As Integer = 0
                Dim sBankMediaCode As String = String.Empty

                oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "MediaType", True, False)

                nBankMediaId = (oLookUP.FindItemByCode(GISLookup_MediaType.Value)).Key
                'Account Number should be passed as BankBranchCode|AccountNumber
                Dim sAccountNumber As String = Trim(txtBankCode.Text) & "|" & Trim(txtBankAccNumber.Text)
                Dim nCountryID As Integer = GetKeyForDescription(NexusProvider.ListType.PMLookup, Address.Country, "Country")


                'Validate the Bank Account Number using SAM call ValidateBankAccountNumber
                oValidationDetailsCollection = oWebService.ValidateBankAccountNumber(nBankMediaId, nCountryID, sAccountNumber, sBankMediaCode)

                If oValidationDetailsCollection(0).IsValid Then
                    'If the Account Number is Valid
                    args.IsValid = True
                Else
                    'If the Account Number is Invalid
                    args.IsValid = False
                    If oValidationDetailsCollection(0).ValidationMessageDataset <> "" AndAlso oValidationDetailsCollection(0).ValidationMessageDataset IsNot Nothing Then
                        'If the Collection returna any message from Back office Script
                        CustVldValidateAccountNumber.ErrorMessage = oValidationDetailsCollection(0).ValidationMessageDataset
                    Else
                        'If the Collection does not returns any BO script message, and IsValid key is false, then a custom message is passed 
                        CustVldValidateAccountNumber.ErrorMessage = GetLocalResourceObject("lbl_errmsg_ValidateAccountNumber").ToString()
                    End If
                End If

                'cleaning up
                oWebService = Nothing
                oValidationDetailsCollection = Nothing
                nBankMediaId = Nothing
                sAccountNumber = Nothing
                nCountryID = Nothing
            ElseIf rblPayee.SelectedIndex = 0 AndAlso GISLookup_MediaType.Value <> "" Then
                CustVldValidateAccountNumber.ErrorMessage = GetLocalResourceObject("lbl_errmsg_ValidateAccountNumber").ToString()
                args.IsValid = False
            Else
                args.IsValid = True
            End If
        End Sub

        ''' <summary>
        ''' MediaType get Media Type Validation Id for firing Media Type Validations
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub GISLookup_MediaType_SelectedIndexChange(sender As Object, e As EventArgs) Handles GISLookup_MediaType.SelectedIndexChange

            If Me.FindControl("hdMediaTypeValidationId") IsNot Nothing Then
                Dim hdMediaTypeValidationId As HiddenField = CType(Me.FindControl("hdMediaTypeValidationId"), HiddenField)
                If rblPayee.SelectedIndex = 0 AndAlso GISLookup_MediaType.Value <> "" Then
                    Dim oLookUP, oStatusLookup As New NexusProvider.LookupListCollection
                    Dim oWebService = New NexusProvider.ProviderManager().Provider
                    Dim oXmlElement As System.Xml.XmlElement = Nothing
                    oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "MediaType", True, False, , , , oXmlElement, )
                    Dim nodeMediaType As XmlNode
                    Dim bFound As Boolean = False
                    For Each nodeMediaType In oXmlElement.ChildNodes
                        For Each nodeSingleMedia In nodeMediaType.ChildNodes
                            If nodeSingleMedia.Name.ToLower() = "code" Then
                                If GISLookup_MediaType.Value = nodeSingleMedia.InnerText.ToString.Trim() Then
                                    bFound = True
                                    Exit For
                                End If
                            End If
                        Next
                        If bFound Then
                            hdMediaTypeValidationId.Value = nodeMediaType.ChildNodes(6).InnerText
                            Exit For
                        End If
                    Next
                End If

                If ViewState("RecoveryAutoSelected") IsNot Nothing AndAlso CBool(ViewState("RecoveryAutoSelected")) Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "PreserveAutoSelectedState",
                    "$('input[name=""" & rblPayee.UniqueID & """]').prop('disabled', true); " &
                    "$('#" & btnParty.ClientID & "').prop('disabled', true); " &
                    "$('#" & btnClientParty.ClientID & "').prop('disabled', true);", True)
                End If

                Dim sScript As String = "self.parent.EnableValidators('" + hdMediaTypeValidationId.Value + "');"

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", sScript, True)
                Exit Sub

            End If

        End Sub

        Private Function ValidateReceiptBalance(ByVal iPerilIndex As Integer) As Boolean
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oCollection As NexusProvider.PerilRecoveryCollection

            If Session(CNMode) = Mode.SalvageClaim Then
                oCollection = oClaimOpen.ClaimPeril(iPerilIndex).SalvageRecovery
            Else
                oCollection = oClaimOpen.ClaimPeril(iPerilIndex).TPRecovery
            End If

            For i As Integer = 0 To oCollection.Count - 1
                Dim dBalance As Decimal = oCollection(i).InitialRecovery - oCollection(i).ReceiptedAmount - oCollection(i).LossThisReceiptINCLTax
                If dBalance < 0 Then
                    Return False
                End If
            Next
            Return True
        End Function

        ''' <summary>
        ''' Saves the selected recovery instalment plan.
        ''' </summary>
        ''' <summary>
        ''' Shows the Instalments tab and binds instalment schemes.
        ''' </summary>
        Public Sub ShowAndBindInstalments(ByVal dAmount As Double)
            liInstalments.Visible = True
            Session(CNAmountToPay) = dAmount
            ucInstalments.bindInstalments()
        End Sub

        Protected Sub btnSaveInstalmentPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveInstalmentPlan.Click
            Try
                Dim oSelectedQuote As NexusProvider.InstalmentQuote = ucInstalments.SelectedInstalmentQuote

                ' Fallback: if cache expired, get from the grid's data source
                If oSelectedQuote Is Nothing Then
                    Dim oGrid As GridView = CType(ucInstalments.FindControl("grdInstallmentQuotes"), GridView)
                    If oGrid IsNot Nothing AndAlso oGrid.SelectedIndex >= 0 AndAlso oGrid.DataSource IsNot Nothing Then
                        Dim oQuotes As NexusProvider.InstallmentQuoteCollection = TryCast(oGrid.DataSource, NexusProvider.InstallmentQuoteCollection)
                        If oQuotes IsNot Nothing AndAlso oGrid.SelectedIndex < oQuotes.Count Then
                            oSelectedQuote = oQuotes(oGrid.SelectedIndex)
                        End If
                    End If
                End If

                ' Fallback: re-invoke bindInstalments to repopulate cache, then retry
                If oSelectedQuote Is Nothing Then
                    ' Ensure CNQuote is set for bindInstalments
                    If Session(CNQuote) Is Nothing AndAlso Session(CNClaimQuote) IsNot Nothing Then
                        Session(CNQuote) = Session(CNClaimQuote)
                    End If
                    ucInstalments.bindInstalments()
                    oSelectedQuote = ucInstalments.SelectedInstalmentQuote
                End If

                If oSelectedQuote Is Nothing Then
                    lblInstalmentMessage.Text = "Please select an instalment scheme before saving."
                    lblInstalmentMessage.Visible = True
                    Return
                End If
                ucInstalments.SaveInstallmentPlan()

                ' Call SavePremiumFinanceDetails to persist the plan and create ICC/ICD transactions
                Dim oPayment As NexusProvider.Payment = CType(Session(CNPayment), NexusProvider.Payment)
                If oPayment IsNot Nothing Then
                    Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
                    If oQuote Is Nothing Then
                        oQuote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                    End If
                    Dim nInsuranceFileKey As Integer = 0
                    If oQuote IsNot Nothing Then
                        nInsuranceFileKey = oQuote.InsuranceFileKey
                    End If
                    Dim sProcessMode As String = "SR"
                    If Session(CNMode) = Mode.TPRecovery Then
                        sProcessMode = "TPR"
                    End If
                    ' Ensure CNQuote is set with BranchCode for the API call
                    If Session(CNQuote) Is Nothing AndAlso oQuote IsNot Nothing Then
                        Session(CNQuote) = oQuote
                    End If
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    oWebService.SavePremiumFinanceDetails(oPayment, nInsuranceFileKey, sProcessMode)
                End If

                lblInstalmentMessage.Text = "Instalment plan created successfully."
                lblInstalmentMessage.Visible = True
                lblInstalmentMessage.CssClass = "text-success"
            Catch ex As Exception
                lblInstalmentMessage.Text = "Error creating instalment plan: " & ex.Message
                lblInstalmentMessage.Visible = True
            End Try
        End Sub

    End Class
End Namespace
