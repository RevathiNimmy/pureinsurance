Imports System.Collections.Generic
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Xml

Namespace Nexus

    Partial Class Framework_overview : Inherits CMS.Library.Frontend.clsCMSPage
        Dim bIsClosed As Boolean = False

#Region " Page Events "
        ''' <summary>
        ''' 'This event is fired on Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Dim bolCheckPartyKey As Boolean = True
        Public strErrorLossToDate As Boolean = False
        Dim sClaimsCoverBasis As String
        Public bDisableLossdate As Boolean = False

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request("__EVENTARGUMENT") = "LossDate" Then
                CONTROL__LOSS_DATE_TextChanged(sender, e)
                Exit Sub
            End If
            If Request("__EVENTARGUMENT") = "NextButton" Then
                ActionOnNextClick()
                Exit Sub
            End If

            btnNext.Attributes.Remove("onclick")
            If CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim OrElse CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                uctChangeReason.Visible = False
            End If
            rvLossDate.Enabled = False
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserDetails As NexusProvider.UserDetails
            Dim oInsuranceFileDetails As NexusProvider.PolicyCollection



            If Not IsPostBack Then
                Try
                    'Dim oCurrency As NexusProvider.LookupListCollection
                    'oCurrency = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Currency", True, False, Nothing, 1)

                    'If oCurrency IsNot Nothing And oCurrency.Count > 0 Then
                    '    For nCount As Integer = 0 To oCurrency.Count - 1
                    '        CONTROL__CLAIM_CURRENCY.Items.Add(oCurrency(nCount))
                    '    Next
                    'End If
                    Dim oCurrencyColl As NexusProvider.CurrencyCollection
                    oCurrencyColl = oWebservice.GetCurrenciesByBranch(Session(CNBranchCode))
                    Dim bCurrCodeFound As Boolean = False
                    For nCount As Integer = 0 To CONTROL__CLAIM_CURRENCY.Items.Count - 1
                        If nCount < CONTROL__CLAIM_CURRENCY.Items.Count Then

                            For ncurrCollCount As Integer = 0 To oCurrencyColl.Count - 1

                                If CONTROL__CLAIM_CURRENCY.Items(nCount).Code = oCurrencyColl.Item(ncurrCollCount).CurrencyCode.Trim Then
                                    bCurrCodeFound = True
                                    Exit For
                                Else
                                    bCurrCodeFound = False
                                End If
                            Next
                            If bCurrCodeFound = False Then

                                CONTROL__CLAIM_CURRENCY.Items.RemoveAt(nCount)
                                nCount = nCount - 1
                            End If
                        End If
                    Next

                    oCurrencyColl = Nothing
                Catch ex As Exception

                End Try

                Session(CNOriginalInsuranceFileKey) = Session(CNInsuranceFileKey)

                If CType(Session(CNMode), Mode) = Mode.NewClaim Then
                    'To set the Focus
                    Page.SetFocus(ddlRiskType)
                    'hide the reason for change control
                    uctChangeReason.Visible = False

                ElseIf CType(Session(CNMode), Mode) = Mode.EditClaim Then
                    'To set the Focus
                    Page.SetFocus(CONTROL__RISK_DESC)
                    ctrlLetterWriting.Visible = True
                ElseIf CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                    uctChangeReason.SetReasonForChange()
                    ctrlLetterWriting.Visible = True
                End If
                Dim olist As NexusProvider.LookupListCollection
                olist = oWebservice.GetList(NexusProvider.ListType.PMLookup, "progress_status", True, False, "is_closed_check_status", 1)
                ViewState("ProgressStatus") = olist
                If olist IsNot Nothing And olist.Count > 0 Then
                    For nCount As Integer = 0 To olist.Count - 1
                        hdnClosedStatusCode.Value = hdnClosedStatusCode.Value + "|" + olist(nCount).Code
                    Next
                End If
            End If
            If CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                uctChangeReason.SetReasonForChange()
            End If
            oUserDetails = oWebservice.GetUserDetails(HttpContext.Current.User.Identity.Name)
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oParty As NexusProvider.BaseParty
            'check if TPA is attached with the logged in user.
            'If so then the field will be prepopulated with the TPA code and will be read only
            'update the find party control properties as well
            If (Session(CNBaseCaseKey) IsNot Nothing AndAlso CInt(Session(CNBaseCaseKey)) > 0) Then
                Dim oCaseDetails As NexusProvider.CaseDetails = Nothing
                oCaseDetails = oWebservice.GetCaseDetails(CInt(Session(CNBaseCaseKey).ToString()))
                If (oCaseDetails IsNot Nothing) Then
                    CONTROL__CASE_NUMBER.Text = oCaseDetails.CaseNumber
                End If
            Else
                CONTROL__CASE_NUMBER.Visible = False
                lblCaseNumber.Visible = False

            End If
            If oUserDetails.PartyKey <> 0 Then
                If Trim(oUserDetails.PartyType) = OtherPartySearch(PartyName.Type) Then
                    hdnAttachedPartyName.Value = Trim(oUserDetails.PartyCode)
                    If oOpenClaim.TPA = 0 Then
                        PartyName.PartyCode = Trim(oUserDetails.PartyCode)
                        PartyName.PartyKey = oUserDetails.PartyKey
                    Else
                        oParty = oWebservice.GetParty(oOpenClaim.TPA)
                        If oParty IsNot Nothing Then
                            Select Case True
                                Case TypeOf oParty Is NexusProvider.OtherParty
                                    With CType(oParty, NexusProvider.OtherParty)
                                        PartyName.PartyCode = .ShortName.Trim
                                    End With
                            End Select
                        End If
                        PartyName.PartyKey = oOpenClaim.TPA
                    End If
                Else
                    hdnAttachedPartyName.Value = ""
                    PartyName.PartyCode = ""
                    PartyName.PartyKey = 0
                End If
            Else
                bolCheckPartyKey = False
            End If

            If CONTROL__PROGRESS_STATUS.SelectedValue <> "" Then
                Dim v_sOptionList As System.Xml.XmlElement = Nothing

                Dim oClaimProgreesStatus As NexusProvider.LookupListCollection

                oClaimProgreesStatus = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Progress_Status", True, False, , , , v_sOptionList)
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(sXML)
                Dim oNodeList As System.Xml.XmlNodeList
                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/Progress_Status[is_deleted=0]")
                If oNodeList IsNot Nothing And oNodeList.Count > 0 Then
                    If oNodeList IsNot Nothing AndAlso oNodeList.Count > 0 Then
                        For Each oNode As System.Xml.XmlNode In oNodeList
                            If oNode.ChildNodes(2) IsNot Nothing Then
                                If oNode.ChildNodes(2).InnerText.Trim().ToUpper().ToString() = CONTROL__PROGRESS_STATUS.SelectedValue.ToUpper().ToString() And oNode.ChildNodes(6).InnerText.ToUpper().ToString() = "1" Then
                                    bIsClosed = True
                                    Exit For
                                Else
                                    bIsClosed = False
                                End If
                            End If
                        Next
                    End If
                End If
            End If

            Dim strValidate As String = String.Empty

            If Session.Item(CNClaim) IsNot Nothing Then
                If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session.Item(CNMode), Mode) = Mode.PayClaim _
                Or CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery _
                Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                    strValidate = ""
                Else

                    Dim iCt As Integer
                    Dim jCt As Integer
                    Dim dReserveAmt As Double = 0
                    Dim dRecoveryAmt As Double = 0
                    Dim dThirdParty As Double = 0
                    For iCt = 0 To oOpenClaim.ClaimPeril.Count - 1
                        For jCt = 0 To oOpenClaim.ClaimPeril(iCt).Reserve.Count - 1
                            dReserveAmt = dReserveAmt + IIf((oOpenClaim.ClaimPeril(iCt).Reserve(jCt).InitialReserve + oOpenClaim.ClaimPeril(iCt).Reserve(jCt).RevisedReserve) + (-oOpenClaim.ClaimPeril(iCt).Reserve(jCt).PaidAmount) <> 0, (oOpenClaim.ClaimPeril(iCt).Reserve(jCt).InitialReserve + oOpenClaim.ClaimPeril(iCt).Reserve(jCt).RevisedReserve) + (-oOpenClaim.ClaimPeril(iCt).Reserve(jCt).PaidAmount), oOpenClaim.ClaimPeril(iCt).Reserve(jCt).CurrentReserve)
                        Next
                    Next
                    For iCt = 0 To oOpenClaim.ClaimPeril.Count - 1
                        For jCt = 0 To oOpenClaim.ClaimPeril(iCt).Recovery.Count - 1
                            dRecoveryAmt = dRecoveryAmt + IIf(oOpenClaim.ClaimPeril(iCt).Recovery(jCt).InitialRecovery <> 0, oOpenClaim.ClaimPeril(iCt).Recovery(jCt).InitialRecovery, oOpenClaim.ClaimPeril(iCt).Recovery(jCt).RevisedRecovery)
                        Next
                    Next
                    For iCt = 0 To oOpenClaim.ClaimPeril.Count - 1
                        For jCt = 0 To oOpenClaim.ClaimPeril(iCt).TPRecovery.Count - 1
                            dThirdParty = dThirdParty + IIf((oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).InitialRecovery + oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).RevisedRecovery) + (-oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).ReceiptedAmount) <> 0, (oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).InitialRecovery + oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).RevisedRecovery) + (-oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).ReceiptedAmount), oOpenClaim.ClaimPeril(iCt).TPRecovery(jCt).CurrentRecovery)
                        Next
                    Next
                    If dReserveAmt <> 0 Or dRecoveryAmt <> 0 Or dThirdParty <> 0 Then
                        ViewState("ReserverAndRecoveryNotZero") = True
                    End If

                    If oOpenClaim.ClaimVersion <> 0 AndAlso CType(Session(CNMode), Mode) <> Mode.NewClaim AndAlso bIsClosed Then
                        Dim oOnlyAllowClaimClosureWhenReserveIsZero As NexusProvider.OptionTypeSetting
                        oOnlyAllowClaimClosureWhenReserveIsZero = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5073)
                        ViewState("OnlyAllowClaimClosureWhenReserveIsZero") = oOnlyAllowClaimClosureWhenReserveIsZero.OptionValue
                        strValidate = "validateClaimStatus(" + dReserveAmt.ToString() + "," + dRecoveryAmt.ToString() + "," + dThirdParty.ToString() + "," + oOnlyAllowClaimClosureWhenReserveIsZero.OptionValue + ");"
                    End If

                End If



            End If
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)

            Dim oBtnEdit As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("BtnEdit"), LinkButton)
            If oBtnEdit IsNot Nothing Then
                If (CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Or CType(Session.Item(CNMode), Mode) = Mode.Review) Then
                    PartyName.DisableControl = True
                    If oUserDetails.PartyType <> "AG" Then
                        oBtnEdit.Visible = True
                        AddHandler oBtnEdit.Click, AddressOf BtnEdit_Click
                    End If
                Else
                    oBtnEdit.Visible = False
                End If

            End If

            Dim oBtnBack As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("btnBack"), LinkButton)
            If oBtnBack IsNot Nothing Then
                If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Then
                    oBtnBack.Visible = False
                Else
                    oBtnBack.Visible = True


                End If

            End If

            Dim oBtnPay As LinkButton = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("BtnPay"), LinkButton)
            If oBtnPay IsNot Nothing Then
                If CType(Session.Item(CNMode), Mode) = Mode.EditClaim Then
                    oBtnPay.Visible = True
                    AddHandler oBtnPay.Click, AddressOf BtnPay_Click
                Else
                    oBtnPay.Visible = False
                End If

            End If

            If strValidate <> "" Then
                btnNext.Attributes.Add("onclick", "javascript:return " + strValidate)
            End If

            CONTROL__CLAIM_LOCATION.Enabled = True

            If oOpenClaim IsNot Nothing AndAlso oOpenClaim.ClaimStatus IsNot Nothing AndAlso oOpenClaim.ClaimStatus.Length > 0 Then
                CONTROL__CLAIM_STATUS.Text = oOpenClaim.ClaimStatus
            Else
                CONTROL__CLAIM_STATUS.Text = GetLocalResourceObject("lbl_ClaimStatus")
            End If

            If Session(CNMode) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "Login.aspx", False)
            End If

            If Not IsPostBack Then
                If (Session(CNClaim) IsNot Nothing) Then
                    If (Session(CNMode) IsNot Nothing) Then
                        If CType(Session(CNMode), Mode) <> Mode.NewClaim Then
                            Dim oPartyClaim As NexusProvider.BaseParty = Session.Item(CNParty)
                            Dim bClaimBuilder As Boolean = False
                            Dim oOptionType As New NexusProvider.OptionTypeSetting
                            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)

                            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) AndAlso oOptionType.OptionValue = "1" Then
                                Session(CNClaimBuilder) = True
                            End If

                            Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                            If bClaimBuilder Then
                                Dim oClaimRiskResponse As NexusProvider.ClaimRisk = oWebservice.GetClaimRisk(oOpenClaim.BaseClaimKey, oOpenClaim.ClaimKey,
                                                                         oPartyClaim.BranchCode)
                                Session("CNClaimOldXML") = oClaimRiskResponse.XMLDataSet
                            End If
                        End If
                    End If
                End If
                SetValues()
                If ddlRiskType.Items.Count = 2 Then
                    ddlRiskType.SelectedIndex = 1
                Else
                    If CType(Session.Item(CNMode), Mode) = Mode.NewClaim Then
                        CONTROL__LOSS_DATE.Text = FormatDateTime(Session(CNLossToDate), DateFormat.ShortDate)
                        CONTROL__LOSS_TODATE.Text = FormatDateTime(Session(CNLossToDate), DateFormat.ShortDate)
                    End If
                    oInsuranceFileDetails = oWebservice.GetAllPolicyVersions(Session.Item(CNInsuranceFolderKey))
                End If

            ElseIf Request("__EVENTARGUMENT") = "Refresh" Then
                'Updation of the changed values in page
                SetValues()
            End If

            'set the relevant tab styles
            ucProgressBar.OverviewStyle = "in-progress"
            Select Case CType(Session(CNMode), Mode)
                Case Mode.NewClaim, Mode.EditClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery
                    '(can't navigate forwards using tabs in new / edit claim)
                    ucProgressBar.ReinsuranceStyle = "incomplete"
                    ucProgressBar.PerilsStyle = "incomplete"
                    ucProgressBar.SummaryStyle = "incomplete"
                    ucProgressBar.CompleteStyle = "incomplete"
            End Select

            If Session(CNMode) = Mode.PayClaim Then
                Session(CNDuplicateClaimPayment) = Nothing
                Session(CNDuplicateClaimPaymentReason) = Nothing
            End If
        End Sub
#End Region

#Region " Methods "
        ''' <summary>
        ''' This method is used to Load the controls.
        ''' </summary>
        ''' <param name="r_oClaim"></param>
        ''' <remarks></remarks>
        Sub LoadControls(ByRef r_oClaim As NexusProvider.ClaimOpen)

            With r_oClaim
                For index As Integer = 0 To ddlRiskType.Items.Count - 1
                    If ddlRiskType.Items(index).Value = Trim(.RiskKey) Then
                        ddlRiskType.Items(index).Selected = True
                        Exit For
                    End If
                Next

                CONTROL__RISK_DESC.Text = Nexus.Utils.Formatting.DoQuotes(Trim(.Description))
                CONTROL__CLAIM_CURRENCY.Value = Trim(.CurrencyISOCode)
                CONTROL__PROGRESS_STATUS.SelectedValue = Trim(.ProgressStatusCode)
                CONTROL__CLAIM_STATUSDATE.Text = Trim(.ClaimStatusDate.ToShortDateString)
                CONTROL__CLAIM_TOWN.Value = Trim(.TownCode)
                CONTROL__CLAIM_LOCATION.Text = Trim(.Location)

                'If TPA is attached the assign the values
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty
                If .TPA <> 0 Then
                    oParty = oWebservice.GetParty(.TPA)
                    If oParty IsNot Nothing Then
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.OtherParty
                                With CType(oParty, NexusProvider.OtherParty)
                                    PartyName.PartyCode = .ShortName.Trim
                                    PartyName.PartyKey = .Key
                                End With
                        End Select
                    End If
                End If

                If Session(CNMode) = Mode.EditClaim Then
                    CONTROL__CLAIM_LASTMODIFIEDDATE.Text = FormatDateTime(DateTime.Now.ToShortDateString, DateFormat.ShortDate)
                    CONTROL__CLAIM_LASTMODIFIEDTIME.Text = FormatDateTime(DateTime.Now.ToString(), DateFormat.ShortTime)
                ElseIf Session(CNMode) <> Mode.NewClaim Then
                    CONTROL__CLAIM_LASTMODIFIEDDATE.Text = FormatDateTime(.LastModifiedDate, DateFormat.ShortDate)
                    CONTROL__CLAIM_LASTMODIFIEDTIME.Text = FormatDateTime(.LastModifiedDate, DateFormat.ShortTime)
                End If
                CONTROL__CLAIM_NUMBER.Text = Trim(.ClaimNumber)
                If Session(CNMode) = Mode.NewClaim Then
                    CONTROL__CLAIM_VERSION.Text = 1
                ElseIf Session(CNMode) = Mode.ViewClaim OrElse Session(CNMode) = Mode.ViewClaimPayment Then
                    CONTROL__CLAIM_VERSION.Text = Trim(.ClaimVersion)
                Else
                    CONTROL__CLAIM_VERSION.Text = Trim(.ClaimVersion + 1)
                End If

                If .PrimaryCauseCode IsNot Nothing Then
                    For index As Integer = 0 To CONTROL__PRIMARY_CAUSE.Items.Count - 1
                        If CONTROL__PRIMARY_CAUSE.Items(index).Value = Trim(.PrimaryCauseCode) Then
                            CONTROL__PRIMARY_CAUSE.Items(index).Selected = True
                            Exit For
                        End If
                    Next

                    CONTROL__CAT_CODE.Value = Trim(.CatastropheCode)

                    FillValidSecondaryCauses()

                    CONTROL__SECONDARY_CAUSE.SelectedValue = Trim(.SecondaryCauseCode)

                End If

                rangevalLossToDate.MinimumValue = FormatDateTime(.LossFromDate, DateFormat.ShortDate)
                rangevalLossToDate.Enabled = False
                If .LossFromDate <> DateTime.MinValue Then
                    CONTROL__LOSS_DATE.Text = FormatDateTime(.LossFromDate, DateFormat.ShortDate)
                End If
                If .LossToDate <> DateTime.MinValue Then
                    CONTROL__LOSS_TODATE.Text = FormatDateTime(.LossToDate, DateFormat.ShortDate)
                End If
                If .ReportedDate <> DateTime.MinValue Then
                    CONTROL__REPORTED_DATE.Text = FormatDateTime(.ReportedDate, DateFormat.ShortDate)
                End If

                If CType(Session(CNMode), Mode) = Mode.EditClaim Then
                    CONTROL__LOSS_DATE_TextChanged(New Object, New EventArgs)
                End If
                CONTROL__HANDLER_CODE.Value = Trim(.HandlerCode)
                If .UnderwritingYearCode Is Nothing Then
                    CONTROL__CLAIM_UNDERWRITINGYEAR.Value = CType(Session(CNClaimQuote), NexusProvider.Quote).UnderwritingYear
                Else
                    CONTROL__CLAIM_UNDERWRITINGYEAR.Value = GetCodeForKey(NexusProvider.ListType.PMLookup, .UnderwritingYearCode, "Underwriting_Year", True) 'Trim(.UnderwritingYearCode)
                End If

                chkInformation.Checked = .InfoOnly
                chkLikelyToClaim.Checked = .LikelyClaim
                If .InfoOnly = True Then
                    chkInformation.Enabled = True
                    If CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                        btnNext.Visible = False
                    End If
                Else
                    chkInformation.Enabled = False
                    chkLikelyToClaim.Enabled = False
                End If
            End With

        End Sub
#End Region

#Region " Button Events "
        ''' <summary>
        ''' This event is fired on Back Button click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Select Case CType(Session(CNMode), Mode)
                Case Mode.NewClaim
                    Response.Redirect("~/Claims/FindInsuranceFile.aspx", False)
                Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.ViewClaimPayment, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend
                    If Session(CNParentPage) IsNot Nothing Then
                        If Session(CNParentPage) = "ACP" And (Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.ViewClaimPayment Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment Or Session(CNMode) = Mode.Recommend) Then
                            Response.Redirect("~/secure/Authoriseclaimpayments.aspx", False)

                        ElseIf Session(CNParentPage) = "CC" Then 'Coming from Claim Case Page
                            If Session(CNCaseKey) IsNot Nothing Then
                                Dim iCaseKey As Integer = Session(CNCaseKey)
                                Response.Redirect("~/Claims/ClaimCase.aspx?CaseKey=" & iCaseKey, False)
                            Else
                                Response.Redirect("~/Claims/FindCase.aspx", False)
                            End If
                        Else
                            Response.Redirect("~/Claims/FindClaim.aspx", False)
                        End If

                    Else
                        Dim oWebservice As NexusProvider.ProviderBase
                        Dim oLock As NexusProvider.Locks
                        Dim oLockCollection As NexusProvider.LockCollection
                        Try
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            oLock = New NexusProvider.Locks
                            oLockCollection = New NexusProvider.LockCollection
                            oLock.LockName = "claim_id"                    ''It is equivalent to lockname for locking claims in SAM
                            oLock.LockValue = CType(Session(CNClaim), NexusProvider.Claim).BaseClaimKey

                            oLockCollection.Add(oLock)
                            oWebservice.MaintainLock(oLockCollection, False, False, Session(CNBranchCode).ToString())
                            Session(CNClaimBackButton) = "BackButton"
                            Response.Redirect("~/Claims/FindClaim.aspx")
                        Catch ex As System.Exception
                            Dim sScript As String = "alert('" & GetLocalResourceObject("err_ErrorOccured") & "');"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "unlockerror", sScript, True)
                        End Try
                    End If

            End Select
        End Sub
        Sub ValidateValues()
            'Loss Date
            Dim sMessage As String
            Dim sMessageType As String
            Dim oQuote As NexusProvider.Quote
            oQuote = Session(CNClaimQuote)
            If Session(CNMode) = Mode.NewClaim And IsValidDates()  Then
            If CDate(CONTROL__LOSS_DATE.Text.Trim).ToShortDateString > Date.Now _
                Or CDate(CONTROL__LOSS_DATE.Text.Trim).ToShortDateString < oQuote.CoverStartDate _
                Or CDate(CONTROL__LOSS_DATE.Text.Trim).ToShortDateString > oQuote.CoverEndDate Then
                    rvLossDate.IsValid = False
                    rvLossDate.Enabled = True
                Else
                    rvLossDate.IsValid = True
                End If
            End If
            If Session(CNMode) = Mode.EditClaim Then
                If CDate(CONTROL__LOSS_DATE.Text.Trim).ToShortDateString > Date.Now _
                Or CDate(CONTROL__LOSS_DATE.Text.Trim).ToShortDateString < oQuote.CoverStartDate _
                Or CDate(CONTROL__LOSS_DATE.Text.Trim).ToShortDateString > oQuote.CoverEndDate Then
                    sMessage = GetLocalResourceObject("msg_ConfirmInceptionDates").ToString
                    sMessageType = "ALERT"
                    rvLossDate.IsValid = False
                    rvLossDate.Enabled = True
                Else
                    rvLossDate.IsValid = True
                End If
            End If
            If Session(CNMode) = Mode.PayClaim Then
                Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                Dim bPerilHasReserve As Boolean = False
                For iPeril As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                    If oClaimOpen.ClaimPeril(iPeril).Reserve.Count > 0 Then
                        Session(CNClaimPerilIndex) = iPeril
                        bPerilHasReserve = True
                        Exit For
                    End If
                Next
                If Not bPerilHasReserve Then
                    chkPerilReserve.IsValid = False
                Else
                    chkPerilReserve.IsValid = True
                End If
            End If
        End Sub
        Sub CompareValues()
            If Session(CNMode) = Mode.NewClaim Then
                Dim oQuote As NexusProvider.Quote
                oQuote = Session(CNClaimQuote)
                'this will check 
                Dim CONTROL__LOSS, CONTROL__REPORTED As DateTime

                If Not IsValidDates() Then
                    Exit Sub
                End If

                'Loss Date must not be later than Loss To Date
                If CDate(CONTROL__LOSS_DATE.Text) > CDate(CONTROL__LOSS_TODATE.Text) Then
                    rvLossToDate.IsValid = False
                Else
                    rvLossToDate.IsValid = True
                End If

                If CONTROL__LOSS_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__LOSS_TIME.Text.Trim) = True And CONTROL__REPORTED_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__REPORTED_TIME.Text.Trim) = True Then
                    CONTROL__LOSS = CONTROL__LOSS_DATE.Text.Trim + " " + CONTROL__LOSS_TIME.Text.Trim
                    CONTROL__REPORTED = CONTROL__REPORTED_DATE.Text.Trim + " " + CONTROL__REPORTED_TIME.Text.Trim
                    'check the Loss date in future and greater than Reported Date
                    If CDate(CONTROL__LOSS) <= CDate(CONTROL__REPORTED) And CDate(CONTROL__LOSS) < DateTime.Now Then
                        rvLossDate.IsValid = True
                    Else
                        'When loss date is greater than reporting date then  
                        If Date.Compare(CONTROL__REPORTED_DATE.Text, CONTROL__LOSS_DATE.Text) < 0 Then
                            rvLossDate.ErrorMessage = GetLocalResourceObject("lbl_ReportedDate")
                            'When loss date  = reporting date then  
                        ElseIf Date.Compare(CONTROL__REPORTED_DATE.Text, CONTROL__LOSS_DATE.Text) = 0 Then
                            'When loss time  is greater than reporting time then
                            If DateTime.Compare(CONTROL__REPORTED_TIME.Text, CONTROL__LOSS_TIME.Text) < 0 Then
                                rvLossDate.ErrorMessage = GetLocalResourceObject("lb_LossTime")
                            End If
                        End If
                        rvLossDate.IsValid = False
                    End If
                    'check the Reported date in future
                    If CDate(CONTROL__REPORTED) > DateTime.Now Then
                        'When reporting date is greater than Current date then
                        If Date.Compare(FormatDateTime(Now, DateFormat.ShortDate), CONTROL__REPORTED_DATE.Text) < 0 Then
                            rvReportedDate.ErrorMessage = GetLocalResourceObject("Report_date_Future_Error")
                            'When reporting date = Current date then
                        ElseIf Date.Compare(FormatDateTime(Now, DateFormat.ShortDate), CONTROL__REPORTED_DATE.Text) = 0 Then
                            'When reporting time is greater than Current time then 
                            If DateTime.Compare(FormatDateTime(Now, DateFormat.ShortTime), CONTROL__REPORTED_TIME.Text) < 0 Then
                                rvReportedDate.ErrorMessage = GetLocalResourceObject("Report_time_Future_Error")
                            End If
                        End If
                        rvReportedDate.IsValid = False
                    End If

                End If
            End If
        End Sub
        ''' <summary>
        ''' This event is fired on Next Button Click.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>

        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            Dim bIsClosed As Boolean = False
            If (Not ValidateDate()) Then
                Exit Sub
            End If
            ValidateValues()

            If Page.IsValid Then
                Dim sMessage As String = String.Empty
                Dim sMessageType As String = String.Empty
                ValidateClaimDates(sMessage, sMessageType)
                If sMessage <> String.Empty Then
                    'Open Modal
                    Dim sScript As String = "customConfirm('" + sMessage + "','" + sMessageType + "');"
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", "confirm('Are you sure');", True)
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", sScript, True)
                   ' Dim sScript As String = "customConfirm('" + sMessage + "');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", sScript, True)
                    If hdnMessageType.Value = "CONFIRM" Then
                        hdnMessageType.Value = Nothing
                        btnNext.Attributes.Clear()
                        ActionOnNextClick()
                    End If
                Else
                    ActionOnNextClick()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Further process after validating dates
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ActionOnNextClick()
            Dim bIsClosed As Boolean = False
            Dim sRedirectUrl As String

            If Session.Item(CNClaim) IsNot Nothing Then
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                If CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then

                    If chkInformation.Checked Or (chkInformation.Checked And chkLikelyToClaim.Checked) Then

                        sRedirectUrl = RedirectShowCheckUnpaidPremium("COMPLETE")
                        If sRedirectUrl.Trim <> "" Then
                            Response.Redirect(sRedirectUrl, False)
                        Else
                            Response.Redirect("~/Claims/complete.aspx", False)
                        End If
                    Else
                        uctChangeReason.SetReasonForChange()
                        'Response.Redirect("~/Claims/changeclaim.aspx")
                    End If
                ElseIf CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                    Response.Redirect("~/claims/perils.aspx", False)
                ElseIf CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                    Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    'If Risk Type Description is missing then retreive from DropDownList
                    If String.IsNullOrEmpty(oOpenClaim.RiskTypeDescription) = True Then
                        oOpenClaim.RiskTypeDescription = Trim(ddlRiskType.SelectedItem.Text)
                    End If

                    If chkInformation.Checked Or (chkInformation.Checked And chkLikelyToClaim.Checked) Then
                        Response.Redirect("~/Claims/complete.aspx", False)
                    Else
                        Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                        Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                        Dim sBranchCode As String = oClaimQuote.BranchCode
                        Dim sUrl As String = CheckClaimBuilder()
                        Dim bClaimBuilder As Boolean = False
                        Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                        With oOpenClaim
                            If bClaimBuilder Then
                                oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                Session(CNDataSet) = oClaimRisk.XMLDataSet
                            End If
                        End With
                        Response.Redirect(sUrl, False)
                    End If
                Else
                    Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    If CType(Session("OpenClaimFromCase"), Boolean) = True AndAlso Session("OpenClaimFromCase") IsNot Nothing Then
                        oOpenClaim.BaseCaseKey = Session("NewCaseKey")
                        Session.Remove("NewCaseKey")
                    End If
                    If ViewState("ProgressStatus") IsNot Nothing Then
                        Dim olist As NexusProvider.LookupListCollection
                        olist = CType(ViewState("ProgressStatus"), NexusProvider.LookupListCollection)
                        For nCount As Integer = 0 To olist.Count - 1
                            If olist.Item(nCount).Code.ToUpper = Trim(CONTROL__PROGRESS_STATUS.SelectedValue).ToUpper Then
                                bIsClosed = True
                                Exit For
                            End If
                        Next
                    End If

                    With oOpenClaim
                        .PolicyNumber = Session.Item(CNPolicyNumber)
                        .HandlerCode = Trim(CONTROL__HANDLER_CODE.Value)
                        .RiskKey = Trim(ddlRiskType.SelectedValue)
                        .RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                        .RiskTypeDescription = Trim(ddlRiskType.SelectedItem.Text)
                        .Description = Nexus.Utils.Formatting.ReplaceQuotes(CONTROL__RISK_DESC.Text)
                        .SecondaryCauseCode = Trim(CONTROL__SECONDARY_CAUSE.SelectedValue)
                        .SecondaryCauseDescription = Trim(CONTROL__SECONDARY_CAUSE.SelectedItem.Text)
                        .PrimaryCauseCode = Trim(CONTROL__PRIMARY_CAUSE.SelectedValue)
                        .PrimaryCauseDescription = Trim(CONTROL__PRIMARY_CAUSE.SelectedItem.Text)
                        .CatastropheCode = Trim(CONTROL__CAT_CODE.Value)
                        .Location = Trim(CONTROL__CLAIM_LOCATION.Text)
                        .ProgressStatusCode = Trim(CONTROL__PROGRESS_STATUS.SelectedValue)
                        .ClaimStatus = Trim(CONTROL__CLAIM_STATUS.Text)
                        .ClaimStatusDate = FormatDateTime(Trim(CONTROL__CLAIM_STATUSDATE.Text), DateFormat.LongDate) & " " & FormatDateTime(CONTROL__CLAIM_STATUSTIME.Text.Trim, DateFormat.LongTime)
                        .TownCode = Trim(CONTROL__CLAIM_TOWN.Value)
                        .TPA = PartyName.PartyKey 'pass the attached TPA
                        If Session(CNInsuranceFileKey) IsNot Nothing Then
                            .InsuranceFileKey = Session(CNInsuranceFileKey)
                        End If
                        If Session(CNMode) = Mode.EditClaim Then
                            .LastModifiedDate = FormatDateTime(CONTROL__CLAIM_LASTMODIFIEDDATE.Text, DateFormat.LongDate) & " " & FormatDateTime(CONTROL__CLAIM_LASTMODIFIEDTIME.Text.Trim, DateFormat.ShortTime)
                        End If
                        .CurrencyISOCode = Trim(CONTROL__CLAIM_CURRENCY.Value)
                        .ClaimNumber = Trim(CONTROL__CLAIM_NUMBER.Text)
                        .UnderwritingYearCode = Trim(CONTROL__CLAIM_UNDERWRITINGYEAR.Value)
                        If CONTROL__LOSS_DATE.Text <> "" Then
                            .LossFromDate = FormatDateTime(CONTROL__LOSS_DATE.Text, DateFormat.LongDate) & " " & FormatDateTime(CONTROL__LOSS_TIME.Text.Trim, DateFormat.LongTime)
                        End If
                        If CONTROL__LOSS_TODATE.Text <> "" Then
                            .LossToDate = FormatDateTime(CONTROL__LOSS_TODATE.Text, DateFormat.LongDate) & " " & FormatDateTime(CONTROL__LOSS_TIME.Text.Trim, DateFormat.LongTime)
                        End If
                        .ReportedDate = FormatDateTime(CONTROL__REPORTED_DATE.Text, DateFormat.LongDate) & " " & FormatDateTime(CONTROL__REPORTED_TIME.Text.Trim, DateFormat.LongTime)
                        '.ClaimVersionDescription = oClaim.ClaimVersionDescription
                        .ClaimVersion = CInt(Trim(CONTROL__CLAIM_VERSION.Text))
                        ' .Comments = oClaim.Comments
                        .InfoOnly = chkInformation.Checked
                        ' .InsuranceFileKey = oClaim.InsuranceFileKey
                        .LikelyClaim = chkLikelyToClaim.Checked
                        '.Location = oClaim.Location
                    End With

                    Session(CNClaim) = oOpenClaim
                    Session(CNCurrentRiskKey) = ddlRiskType.SelectedIndex - 1

                    Dim sIsDuplicateClaimCheckOn As String = ""
                    Dim bIsDuplicateClaims As Boolean = False
                    If CType(Session(CNMode), Mode) = Mode.NewClaim Then
                        sIsDuplicateClaimCheckOn = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsDuplicateClaimCheckEnabled, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), "")
                        If sIsDuplicateClaimCheckOn = "1" Then
                            If CheckDuplicateClaims(oOpenClaim) = True Then
                                bIsDuplicateClaims = True
                            Else
                                bIsDuplicateClaims = False
                            End If
                        End If
                    End If

                    If chkInformation.Checked Or (chkInformation.Checked And chkLikelyToClaim.Checked) Then
                        Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                        Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                        Dim oClaimOpen As New NexusProvider.ClaimOpen
                        Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                        Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                        Dim sBranchCode As String = oClaimQuote.BranchCode
                        Select Case CType(Session(CNMode), Mode)
                            Case Mode.NewClaim
                                Try
                                    If bIsDuplicateClaims = False Then
                                        'Arch issue 268 
                                        'oClaimResponse = oWebservice.OpenClaim(oOpenClaim, sBranchCode)
                                        If Session(CNBaseCaseKey) IsNot Nothing Then
                                            Integer.TryParse(Convert.ToString(Session(CNBaseCaseKey)), oOpenClaim.BaseCaseKey)
                                        End If
                                        oClaimResponse = OpenClaimCall(oOpenClaim, sBranchCode)
                                        If oClaimResponse.Warnings IsNot Nothing AndAlso oClaimResponse.Warnings.Count > 0 Then
                                            'create a custom validator for showing all the warnings
                                            Dim cstWarningsResults As New CustomValidator
                                            cstWarningsResults.IsValid = False

                                            cstWarningsResults.ErrorMessage = oClaimResponse.Warnings(0).Description
                                            cstWarningsResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                            cstWarningsResults.ValidationGroup = "ClaimsOverviewGroup"
                                            'add the validator to the page, this will have the effect of making the page invalid
                                            Page.Validators.Add(cstWarningsResults)
                                            Exit Select
                                        End If
                                        With oClaimResponse
                                            oClaimOpen.ClaimKey = .ClaimKey
                                            oClaimOpen.ClaimNumber = .ClaimNumber
                                            oClaimOpen.BaseClaimKey = .BaseClaimKey
                                            Session.Item(CNClaimTimeStamp) = .TimeStamp
                                            oClaimOpen.ClaimVersion = .Version
                                            oClaimOpen.ClaimStatus = .ResultingStatus
                                            oClaimOpen.Client = oOpenClaim.Client
                                            Session.Item(CNClaimNumber) = .ClaimNumber
                                        End With
                                        Session(CNClaim) = oClaimOpen

                                        'Check the Claim Builder Hidden product option
                                        Dim oOptionType As New NexusProvider.OptionTypeSetting
                                        oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                                        If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
            AndAlso oOptionType.OptionValue = "1" Then
                                            'arch issue 268
                                            oClaimRisk = AddClaimRiskCall(oClaimResponse.BaseClaimKey, Session.Item(CNClaimTimeStamp), sBranchCode)
                                            If oClaimRisk Is Nothing Then
                                                Exit Sub
                                            End If
                                        End If


                                        sRedirectUrl = RedirectShowCheckUnpaidPremium("ClaimComplete")
                                        If sRedirectUrl.Trim <> "" Then
                                            Response.Redirect(sRedirectUrl, False)
                                        Else
                                            Response.Redirect("~/Claims/complete.aspx", False)
                                        End If


                                    Else
                                        Session(CNClaim) = oOpenClaim
                                        Response.Redirect("~/Claims/DuplicateClaimCheck.aspx?Infoonly=1", False)
                                    End If
                                Finally
                                    oWebservice = Nothing
                                    oUserDetails = Nothing
                                    oOpenClaim = Nothing
                                End Try
                            Case Mode.EditClaim
                                Try
                                    Dim iTimestamp As Byte() = CType(Session.Item(CNClaimTimeStamp), Byte())
                                    'Arch issue 268
                                    'oClaimResponse = oWebservice.MaintainClaim(oOpenClaim, iTimestamp, sBranchCode)
                                    oClaimResponse = MaintainClaimCall(oOpenClaim, iTimestamp, sBranchCode)

                                    With oClaimResponse
                                        oClaimOpen.ClaimKey = .ClaimKey
                                        oClaimOpen.ClaimNumber = .ClaimNumber
                                        oClaimOpen.BaseClaimKey = .BaseClaimKey
                                        Session.Item(CNClaimTimeStamp) = .TimeStamp
                                        oClaimOpen.ClaimVersion = .Version
                                        oClaimOpen.ClaimStatus = .ResultingStatus
                                    End With
                                    Session(CNClaim) = oClaimOpen
                                    Response.Redirect("~/Claims/Complete.aspx", False)
                                Finally
                                    oWebservice = Nothing
                                    oUserDetails = Nothing
                                    oOpenClaim = Nothing
                                End Try
                        End Select
                    Else
                        If Session(CNMode) = Mode.EditClaim Then
                            Dim bAllowToCloseClaim As Boolean = True
                            If ViewState("OnlyAllowClaimClosureWhenReserveIsZero") IsNot Nothing Then
                                bAllowToCloseClaim = If(ViewState("OnlyAllowClaimClosureWhenReserveIsZero") = "1", False, True)
                            End If
                            If bIsClosed And bAllowToCloseClaim Then
                                ''If oOpenClaim.ProgressStatusCode.Trim.ToUpper = "CLOSED" Then
                                'CLose The CLaim
                                Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)
                                Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                                Dim oClaimOpen As New NexusProvider.ClaimOpen
                                oOpenClaim.ClaimVersionDescription = "Claim Closed"
                                Dim iTimestamp As Byte() = CType(Session.Item(CNClaimTimeStamp), Byte())

                                oClaimResponse = MaintainClaimCall(oOpenClaim, iTimestamp, oClaimQuote.BranchCode)
                                If oClaimResponse Is Nothing Then
                                    Exit Sub
                                End If
                                With oClaimResponse
                                    oClaimOpen.ClaimKey = .ClaimKey
                                    oClaimOpen.ClaimNumber = .ClaimNumber
                                    oClaimOpen.BaseClaimKey = .BaseClaimKey
                                    Session.Item(CNClaimTimeStamp) = .TimeStamp
                                    oClaimOpen.ClaimVersion = .Version
                                    oClaimOpen.ClaimStatus = .ResultingStatus
                                End With
                                Session(CNClaim) = oClaimOpen
                                Session(CNClaimStatus) = "CLOSED"
                                Response.Redirect("~/Claims/Complete.aspx")
                            Else
                                Session(CNClaimStatus) = Trim(CONTROL__PROGRESS_STATUS.SelectedValue)
                            End If
                        End If
                        Session(CNClaim) = oOpenClaim

                        If CType(Session.Item(CNMode), Mode) = Mode.NewClaim And bIsDuplicateClaims = True Then
                            Response.Redirect("~/Claims/DuplicateClaimCheck.aspx?Infoonly=0", False)
                        Else
                            'Code to fire the Open Claim and Ad Claim Risk
                            Dim sClaimNumber As String = Nothing
                            Dim sSubBranchCode As String = Nothing
                            Dim bTimeStamp() As Byte = Nothing
                            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
                            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
                            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                            Dim sBranchCode As String = oQuote.BranchCode

                            Select Case CType(Session(CNMode), Mode)
                                Case Mode.NewClaim
                                    Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                                    If Session(CNClaimNumber) IsNot Nothing AndAlso Session(CNClaimNumber).ToString.Trim.ToUpper = "TBA" Then
                                        'calling of sam method
                                        'To skip the posting first time
                                        oOriginalClaim.ReserveOnly = True
                                        If Session(CNBaseCaseKey) IsNot Nothing Then
                                            Integer.TryParse(Convert.ToString(Session(CNBaseCaseKey)), oOriginalClaim.BaseCaseKey)
                                        End If
                                        'Arch issue 268 
                                        'oClaimResponse = oWebservice.OpenClaim(oOriginalClaim, sBranchCode)
                                        oClaimResponse = OpenClaimCall(oOriginalClaim, sBranchCode)
                                        If oClaimResponse Is Nothing Then
                                            Exit Sub
                                        End If

                                        If oClaimResponse.Warnings IsNot Nothing AndAlso oClaimResponse.Warnings.Count > 0 Then
                                            'create a custom validator for showing all the warnings
                                            Dim cstWarningsResults As New CustomValidator
                                            cstWarningsResults.IsValid = False

                                            cstWarningsResults.ErrorMessage = oClaimResponse.Warnings(0).Description
                                            cstWarningsResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                            cstWarningsResults.ValidationGroup = "ClaimsOverviewGroup"
                                            'add the validator to the page, this will have the effect of making the page invalid
                                            Page.Validators.Add(cstWarningsResults)
                                            Exit Select
                                        End If

                                        'Check the Claim Builder Hidden product option
                                        Dim oOptionType As New NexusProvider.OptionTypeSetting
                                        oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                                        If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
            AndAlso oOptionType.OptionValue = "1" Then
                                            'sam call for claim risk
                                            'arch issue 268
                                            oClaimRisk = AddClaimRiskCall(oClaimResponse.BaseClaimKey, oClaimResponse.TimeStamp, sBranchCode)
                                            If oClaimRisk Is Nothing Then
                                                Exit Sub
                                            End If
                                        End If
                                        'Update the session variable
                                        GetClaimDetails(oClaimResponse.ClaimKey, oClaimRisk)
                                    End If

                                    sRedirectUrl = RedirectShowCheckUnpaidPremium("CLAIMBUILDER")

                                    If sRedirectUrl.Trim <> "" Then
                                        Response.Redirect(sRedirectUrl, False)
                                    Else
                                        'Checking of the Claim Builder
                                        Dim sUrl As String = CheckClaimBuilder()
                                        Response.Redirect(sUrl, False)
                                    End If


                                Case Mode.EditClaim
                                    uctChangeReason.SetReasonForChange()
                            End Select
                        End If
                    End If
                End If
            End If
        End Sub

#End Region
        Sub SetValues()
            'set navigator tab styles
            'liUnderwritingYear.Visible = True ' This field was not in BO screen so make is visible false
            Session.Item(CNScreenCode) = AppSettings("ScreenCode")
            rvReportedDate.MaximumValue = DateTime.Now.ToShortDateString

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOpenClaim As NexusProvider.ClaimOpen = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            Dim bLossCurrencyChange As Boolean
            Dim sJS As String = String.Empty
            Dim bModeSpecified As Boolean = False
            Dim iMode As Integer = 1
            If oQuote Is Nothing Then
                oQuote = oWebservice.GetHeaderAndSummariesByKey(Session(CNInsuranceFileKey), sBranchCode, True)
            End If

            Session(CNDisplayValidVersion) = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.ValidPolicyVersionAtLossDate, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, "")

            'Lossdate should not be greater than Current Date and not less than Cover Start Date
            If oQuote.InceptionDate <= Date.Now.ToShortDateString Then
                rvLossDate.MinimumValue = FormatDateTime(oQuote.InceptionDate, 2)
            End If

            If oQuote.CoverEndDate < Date.Now.ToShortDateString Then
                rvLossDate.MaximumValue = oQuote.CoverEndDate
            Else
                rvLossDate.MaximumValue = Date.Now.ToShortDateString
            End If

            If CDate(rvLossDate.MaximumValue) < CDate(rvLossDate.MinimumValue) Then
                rvLossDate.MinimumValue = oQuote.CoverStartDate
            End If
            'To Filter deleted risk type
            Dim oNonDeletedRiskTypes = New NexusProvider.RiskCollection

            Dim lstDeletedRisk As New List(Of String)()
            For Each tempRisk As NexusProvider.Risk In oQuote.Risks
                If Trim(tempRisk.StatusCode) = "DELETED" Then
                    lstDeletedRisk.Add(tempRisk.Key)
                End If
                oNonDeletedRiskTypes.Add(tempRisk)
            Next

            rvLossDate.Enabled = False

            Session.Item(CNInsuranceFolderKey) = oQuote.InsuranceFolderKey

            Dim oInsuranceFileDetails As NexusProvider.PolicyCollection
            oInsuranceFileDetails = oWebservice.GetAllPolicyVersions(Session.Item(CNInsuranceFolderKey))

            Session(CNInsuranceFileDetails) = oInsuranceFileDetails

            'sort collection before binding
            'oNonDeletedRiskTypes.SortColumn = "Description"
            'oNonDeletedRiskTypes.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            'oNonDeletedRiskTypes.Sort()

            ddlRiskType.DataSource = oNonDeletedRiskTypes
            ddlRiskType.DataTextField = "Description"
            ddlRiskType.DataValueField = "Key"
            ddlRiskType.DataBind()
            ddlRiskType.Items.Insert(0, New ListItem("Please Select", ""))

            If lstDeletedRisk IsNot Nothing Then
                If lstDeletedRisk.Count > 0 Then
                    For i = 0 To lstDeletedRisk.Count - 1
                        Dim index As Integer = ddlRiskType.Items.IndexOf(ddlRiskType.Items.FindByValue(lstDeletedRisk(i)))
                        ddlRiskType.Items(index).Enabled = False
                    Next
                End If
                lstDeletedRisk.Clear()
            End If



            Session(CNClaimQuote) = oQuote
            If (CType(Session(CNMode), Mode) = Mode.ViewClaim) Then
                bModeSpecified = True
                iMode = 0
            End If

            Dim oPrimaryCauses As New NexusProvider.PrimaryCausesCollections
            If Session(CNMode) IsNot Nothing AndAlso (Session(CNMode) = Mode.ViewClaim OrElse Session(CNMode) = Mode.ViewClaimPayment) Then
                oPrimaryCauses = oWebservice.GetValidPrimaryCauses(Session(CNInsuranceFileKey), iMode, bModeSpecified, sBranchCode, True)
            Else
                oPrimaryCauses = oWebservice.GetValidPrimaryCauses(Session(CNInsuranceFileKey), iMode, bModeSpecified, sBranchCode, False)
            End If


            'sort collection before binding
            oPrimaryCauses.SortColumn = "Description"
            oPrimaryCauses.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oPrimaryCauses.Sort()

            ViewState("PrimaryCauses") = oPrimaryCauses
            CONTROL__PRIMARY_CAUSE.DataSource = oPrimaryCauses
            CONTROL__PRIMARY_CAUSE.DataTextField = "Description"
            CONTROL__PRIMARY_CAUSE.DataValueField = "Code"
            CONTROL__PRIMARY_CAUSE.DataBind()
            CONTROL__PRIMARY_CAUSE.Items.Insert(0, New ListItem("Please Select", ""))
            FillProgressStatus()
            'If oQuote.BusinessTypeCode = "DIRECT" Then
            '    hypAgentDetails.Visible = False
            'Else
            '    hypAgentDetails.Visible = True
            'End If
            CONTROL__CLAIM_STATUSTIME.Enabled = False
            If CType(Session(CNMode), Mode) = Mode.PayClaim Or CType(Session(CNMode), Mode) = Mode.SalvageClaim _
            Or CType(Session(CNMode), Mode) = Mode.TPRecovery Then
                hypClaimVersion.Visible = False
            Else
                hypClaimVersion.Visible = True
                hypClaimVersion.Enabled = True
            End If
            Select Case CType(Session(CNMode), Mode)
                Case Mode.NewClaim
                    'set the relevant tab styles (can't navigate forwards using tabs in new claim)
                    ucProgressBar.ReinsuranceStyle = "incomplete"
                    ucProgressBar.PerilsStyle = "incomplete"
                    ucProgressBar.SummaryStyle = "incomplete"
                    ucProgressBar.CompleteStyle = "incomplete"

                    oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                    'If oQuote.Risks.Count > 0 And oQuote.Risks.Count = 1 Then
                    '    Dim oRiskCOde As NexusProvider.Risk = oQuote.Risks(0)
                    '    If ddlRiskType.Items.IndexOf _
                    '                        (ddlRiskType.Items.FindByValue(oRiskCOde.Key)) > 0 Then
                    '        ddlRiskType.SelectedIndex = ddlRiskType.Items.IndexOf _
                    '           (ddlRiskType.Items.FindByValue(oRiskCOde.Key))

                    '    End If
                    'End If
                    With oOpenClaim
                        .LossFromDate = Session.Item(CNLossDate)
                        .LossToDate = Session.Item(CNLossDate)
                        .ReportedDate = FormatDateTime(DateTime.Now(), DateFormat.ShortDate)
                        .InsuranceFileKey = Session.Item(CNInsuranceFileKey)
                        .ProgressStatusCode = oPortal.Claims.NewClaimStatus
                        .CurrencyISOCode = Session.Item(CNCurrenyCode)
                        .ClaimStatus = Session.Item(CNClaimStatus)
                        .ClaimStatusDate = FormatDateTime(DateTime.Now(), DateFormat.ShortDate)
                        .ClaimNumber = Session.Item(CNClaimNumber)
                        .ClaimVersion = 1
                    End With
                    Session(CNClaim) = oOpenClaim
                    LoadControls(oOpenClaim)
                    chkInformation.Enabled = True
                    chkLikelyToClaim.Enabled = True
                    hypClaimVersion.Visible = False
                    CONTROL__LOSS_DATE.Enabled = True
                    CONTROL__LOSS_TODATE.Enabled = True
                    'Issue126
                    'check product risk option
                    bLossCurrencyChange = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.LossCurrencyChange, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    'on open claim- for the first time claim currency dropdown is editable, once next is clicked dropdown will not be available.
                    If bLossCurrencyChange And Session(CNClaimNumber) IsNot Nothing And Session(CNClaimNumber).ToString.Trim.ToUpper = "TBA" Then
                        CONTROL__CLAIM_CURRENCY.Enabled = True
                    Else
                        CONTROL__CLAIM_CURRENCY.Enabled = False
                    End If
                    hypPaymentHistory.Visible = False

                    CONTROL__CLAIM_STATUSTIME.Text = FormatDateTime(Session.Item(CNLossDate), DateFormat.ShortTime)
                    CONTROL__LOSS_TIME.Enabled = True
                    CONTROL__LOSS_TIME.Text = FormatDateTime(Session.Item(CNLossDate), DateFormat.ShortTime)
                    CONTROL__REPORTED_TIME.Enabled = True
                    CONTROL__REPORTED_TIME.Text = FormatDateTime(Session.Item(CNLossDate), DateFormat.ShortTime)
                    If oQuote.UnderwritingYear IsNot Nothing AndAlso oQuote.UnderwritingYear.Length > 0 Then
                        CONTROL__CLAIM_UNDERWRITINGYEAR.Value = Trim(oQuote.UnderwritingYear)
                        CONTROL__CLAIM_UNDERWRITINGYEAR.Enabled = False
                    End If

                    Session(CNClaim) = oOpenClaim
                    btnNext.Attributes.Clear()
                    If CDate(CONTROL__LOSS_DATE.Text) > oQuote.CoverEndDate Then
                        Dim nChkAttachClaimOutsideOfPolicyPeriod As Integer
                        nChkAttachClaimOutsideOfPolicyPeriod = Check_AttachClaimOutsideOfPolicyPeriod()
                        If nChkAttachClaimOutsideOfPolicyPeriod = "1" Then
                            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "InvalidDatesValidation('" + GetLocalResourceObject("msg_InvalidDatesValidation") + "');", True)

                        Else
                            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "InvalidDatesConfirmation('" + GetLocalResourceObject("msg_ConfirmInvalidDates") + "');", True)

                        End If
                    End If
                    If oQuote.InsuranceFileStatusCode = "LAP" Then
                        btnNext.Attributes.Add("onclick", "javascript:return confirm('" & GetLocalResourceObject("msg_ConfirmLapse").ToString() & "');")
                        sJS = "if(confirm('" & GetLocalResourceObject("msg_ConfirmLapse").ToString() & "')==false){return false;}"
                    End If


                    If CDate(CONTROL__LOSS_DATE.Text) < oQuote.InceptionDate Then
                        If oQuote.InsuranceFileStatusCode = "CAN" Then
                            btnNext.Attributes.Add("onclick", "javascript:return InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", oQuote.CoverStartDate).Replace("#CoverToDate#", CDate(Session(CNLapseDate)).ToShortDateString()) & "');")
                            sJS = "if(InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", oQuote.CoverStartDate).Replace("#CoverToDate#", CDate(Session(CNLapseDate)).ToShortDateString()) & "')==false){return false;}"
                        Else
                            btnNext.Attributes.Add("onclick", "javascript:return InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", oQuote.CoverStartDate).Replace("#CoverToDate#", oQuote.CoverEndDate) & "');")
                            sJS = "if(InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", oQuote.CoverStartDate).Replace("#CoverToDate#", oQuote.CoverEndDate) & "')==false){return false;}"
                        End If
                    End If

                    If oQuote.InsuranceFileTypeCode = "MTACAN" _
                            And oQuote.CoverStartDate <= CDate(CONTROL__LOSS_DATE.Text) Then
                        btnNext.Attributes.Add("onclick", "javascript:return confirm('" & GetLocalResourceObject("msg_ConfirmCancel").ToString().Replace("#CoverFrom#", oQuote.CoverStartDate.ToShortDateString()) & "');")
                        sJS = "if(confirm('" & GetLocalResourceObject("msg_ConfirmCancel").ToString().Replace("#CoverFrom#", oQuote.CoverStartDate.ToShortDateString()) & "')==false){return false;}"
                    End If

                Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.ViewClaimPayment, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend

                    If Session(CNClaim) Is Nothing Then
                        Response.Redirect("~/Claims/findclaim.aspx", False)
                    Else
                        oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)

                        'If Risk Type is missing then find it from oQuote
                        If String.IsNullOrEmpty(oOpenClaim.RiskType) Then
                            If oQuote IsNot Nothing AndAlso oQuote.Risks.Count > 0 Then
                                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                                    If oQuote.Risks(iCount).Key = oOpenClaim.RiskKey Then
                                        oOpenClaim.RiskType = oQuote.Risks(iCount).RiskTypeCode
                                    End If
                                Next
                            End If
                        End If
                        Session(CNClaim) = oOpenClaim
                    End If

                    LoadControls(oOpenClaim)
                    If oOpenClaim.UnderwritingYearCode IsNot Nothing AndAlso oOpenClaim.UnderwritingYearCode.Length > 0 Then
                        CONTROL__CLAIM_UNDERWRITINGYEAR.Value = Trim(oOpenClaim.UnderwritingYearCode)
                        CONTROL__CLAIM_UNDERWRITINGYEAR.Enabled = False
                    End If

                    If Session.Item(CNClaim) IsNot Nothing Then

                        If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.PayClaim _
                        Or CType(Session(CNMode), Mode) = Mode.SalvageClaim Or CType(Session(CNMode), Mode) = Mode.TPRecovery _
                        Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend Then
                            DisableControls(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName))

                            For Each oClaimPeril As NexusProvider.PerilSummary In oOpenClaim.ClaimPeril
                                If oClaimPeril.ClaimPayment IsNot Nothing Then
                                    If oClaimPeril.ClaimPayment.Count > 0 Then
                                        hypPaymentHistory.Visible = True
                                        hypPaymentHistory.Enabled = True
                                        Exit For
                                    Else
                                        hypPaymentHistory.Visible = False
                                    End If
                                End If
                            Next

                            CONTROL__SECONDARY_CAUSE.Enabled = False
                            'if edit mode the other party control should be enabled only if
                            PartyName.DisableControl = True
                        End If

                        If Session(CNMode) = Mode.EditClaim Then

                            rangevalLossToDate.MaximumValue = FormatDateTime(oQuote.CoverEndDate, DateFormat.ShortDate)
                            EnableControls(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName))
                            rvLossDate.Enabled = False
                            CONTROL__REPORTED_DATE.Enabled = True  'Temporaray Change
                            CONTROL__REPORTED_TIME.Enabled = True
                            CONTROL__PRIMARY_CAUSE.Enabled = True
                            CONTROL__LOSS_TODATE.Enabled = True 'Temporaray Change
                            CONTROL__LOSS_TODATE.Enabled = True
                            CONTROL__CLAIM_NUMBER.Enabled = False
                            CONTROL__CLAIM_STATUS.Enabled = False
                            CONTROL__CLAIM_STATUSDATE.Enabled = False
                            CONTROL__CLAIM_STATUSTIME.Enabled = False
                            CONTROL__CLAIM_LOCATION.Enabled = True
                            CONTROL__CLAIM_LASTMODIFIEDDATE.Enabled = False
                            CONTROL__CLAIM_LASTMODIFIEDTIME.Enabled = False
                            If (oOpenClaim.InfoOnly) Then
                                Session(CNInfoOnly) = True
                                chkInformation.Enabled = True
                            Else
                                Session.Remove(CNInfoOnly)
                                chkInformation.Enabled = False
                            End If

                            CONTROL__CLAIM_VERSION.Enabled = False
                            rvLossDate.Enabled = False
                            If CDate(CONTROL__LOSS_DATE.Text) < oQuote.InceptionDate Then
                                If oQuote.InsuranceFileStatusCode = "CAN" Then
                                    btnNext.Attributes.Add("onclick", "javascript:return InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", oQuote.CoverStartDate).Replace("#CoverToDate#", CDate(Session(CNLapseDate)).ToShortDateString()) & "');")
                                Else
                                    btnNext.Attributes.Add("onclick", "javascript:return InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", oQuote.CoverStartDate).Replace("#CoverToDate#", oQuote.CoverEndDate) & "');")
                                End If
                            End If
                            'if edit mode the other party control should be enabled only if logged in user is not associated with TPA
                            If hdnAttachedPartyName.Value = "" Then
                                PartyName.DisableControl = False
                            End If
                            'Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                            Dim oDisableOptionSettings As NexusProvider.OptionTypeSetting
                            oDisableOptionSettings = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5176)
                            If oDisableOptionSettings.OptionValue Is Nothing Then
                                oDisableOptionSettings.OptionValue = "0"
                            End If

                            If (oDisableOptionSettings.OptionValue = "1") Then
                                CONTROL__LOSS_TODATE.Enabled = False
                                CONTROL__LOSS_DATE.Enabled = False
                                CONTROL__LOSS_TIME.Enabled = False
                                CoverStart_uctCalendarLookup2.Visible = False
                                CoverStart_uctCalenderlookup4.Visible = False
                            Else
                                CONTROL__LOSS_TODATE.Enabled = True
                                CONTROL__LOSS_DATE.Enabled = True
                                CONTROL__LOSS_TIME.Enabled = True
                                CoverStart_uctCalendarLookup2.Visible = True
                                CoverStart_uctCalenderlookup4.Visible = True
                            End If
                            'oWebservice = Nothing
                        Else
                            CONTROL__REPORTED_DATE.Enabled = False
                            CONTROL__PRIMARY_CAUSE.Enabled = False
                            'if edit mode the other party control should be enabled only if
                            PartyName.DisableControl = True
                        End If

                        ' hide calendar control if in view or edit modes
                        CoverStart_uctCalendarLookup2.Visible = False
                        CoverStart_uctCalenderLookup3.Visible = True
                        CoverStart_uctCalenderlookup4.Visible = False
                        CONTROL__CLAIM_CURRENCY.Enabled = False

                        Dim oVersions As NexusProvider.VersionsCollections = Nothing

                        oVersions = oWebservice.GetVersionsForClaim(Session(CNClaimNumber), sBranchCode)
                        Session.Item(CNClaimVersion) = oVersions
                        If (CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaimPayment Or CType(Session(CNMode), Mode) = Mode.Authorise Or CType(Session(CNMode), Mode) = Mode.DeclinePayment Or CType(Session(CNMode), Mode) = Mode.Recommend) And oVersions.Count > 1 Then
                            hypClaimVersion.Visible = True
                            hypClaimVersion.Enabled = True
                        Else
                            hypClaimVersion.Visible = False
                        End If

                        ddlRiskType.Enabled = False
                        If oOpenClaim.ClaimStatus.Trim().ToUpper() = "CLOSED" Then
                            CONTROL__CLAIM_STATUS.Text = GetLocalResourceObject("lbl_Closed_Status")
                        End If
                        If ViewState("ProgressStatus") IsNot Nothing Then
                            Dim olist As NexusProvider.LookupListCollection
                            olist = CType(ViewState("ProgressStatus"), NexusProvider.LookupListCollection)
                            For nCount As Integer = 0 To olist.Count - 1
                                If olist.Item(nCount).Code = CONTROL__PROGRESS_STATUS.Text.ToUpper Then
                                    CONTROL__CLAIM_STATUS.Text = GetLocalResourceObject("lbl_Closed_Status")
                                    Exit For
                                End If
                            Next
                        End If
                        'Setting of Time on Edit Claim
                        oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                        CONTROL__CLAIM_STATUS.Text = Trim(oOpenClaim.ClaimStatus)
                        CONTROL__CLAIM_STATUSTIME.Text = FormatDateTime(oOpenClaim.ClaimStatusDate, DateFormat.ShortTime)
                        CONTROL__LOSS_TIME.Text = FormatDateTime(oOpenClaim.LossToDate, DateFormat.ShortTime)
                        CONTROL__REPORTED_TIME.Text = FormatDateTime(oOpenClaim.ReportedDate, DateFormat.ShortTime)

                    Else
                        Response.Redirect("~/Claims/FindClaim.aspx", False)
                    End If
            End Select

        End Sub
        Protected Sub CONTROL__PRIMARY_CAUSE_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CONTROL__PRIMARY_CAUSE.SelectedIndexChanged
            If CONTROL__PRIMARY_CAUSE.SelectedValue.Trim.Length <> 0 Then
                FillValidSecondaryCauses()
            Else
                CONTROL__SECONDARY_CAUSE.Items.Clear()
            End If
        End Sub

        Sub FillValidSecondaryCauses()
            Dim olist, oValidSecCauses As NexusProvider.LookupListCollection
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            olist = oWebservice.GetList(NexusProvider.ListType.PMLookup, "secondary_cause", True, False)
            oValidSecCauses = New NexusProvider.LookupListCollection
            Dim oPrimaryCausesColl As NexusProvider.PrimaryCausesCollections = ViewState("PrimaryCauses")
            For iCount As Integer = 0 To olist.Count - 1
                If oPrimaryCausesColl IsNot Nothing Then
                    For jCount As Integer = 0 To oPrimaryCausesColl.Count - 1
                        If olist(iCount).ParentKey = oPrimaryCausesColl(jCount).PrimaryCauseId _
                        And oPrimaryCausesColl(jCount).Code.Trim.ToUpper = CONTROL__PRIMARY_CAUSE.SelectedValue.Trim.ToUpper Then
                            oValidSecCauses.Add(olist(iCount))
                        End If
                    Next
                End If
            Next


            oValidSecCauses.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)

            CONTROL__SECONDARY_CAUSE.DataSource = oValidSecCauses
            CONTROL__SECONDARY_CAUSE.DataTextField = "Description"
            CONTROL__SECONDARY_CAUSE.DataValueField = "Code"
            CONTROL__SECONDARY_CAUSE.DataBind()
            CONTROL__SECONDARY_CAUSE.Items.Insert(0, New ListItem("(Please Select)", ""))
        End Sub

        Sub FillProgressStatus()
            Dim sXmlElement As System.Xml.XmlElement = Nothing
            Dim oDNode As XmlNodeList
            Dim oClaimProgressStatusList As NexusProvider.LookupListCollection
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Cache.Remove("PMLookup_progress_status")
            Cache.Remove(NexusProvider.ListType.PMLookup & "_" & "progress_status")

            If CType(Session(CNMode), Mode) = Mode.NewClaim Then
                oClaimProgressStatusList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "progress_status",
                                                               True, False, "is_closed_check_status", 0,
                                                               v_sXmlElement:=sXmlElement)


                oDNode = sXmlElement.SelectNodes("//progress_status[is_closed_check_status='0']")

                For Each node As XmlNode In oDNode
                    Dim lstItems As ListItem = New ListItem()
                    If node.SelectSingleNode("is_deleted").InnerText.ToString() = "0" Then
                        lstItems.Text = node.SelectSingleNode("description").InnerText.ToString()
                        lstItems.Value = node.SelectSingleNode("code").InnerText.ToString()
                        CONTROL__PROGRESS_STATUS.Items.Add(lstItems)
                    End If
                Next
            Else
                oClaimProgressStatusList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "progress_status", True, False)
                'Sort collection before binding
                oClaimProgressStatusList.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)
                CONTROL__PROGRESS_STATUS.DataSource = oClaimProgressStatusList
                CONTROL__PROGRESS_STATUS.DataTextField = "Description"
                CONTROL__PROGRESS_STATUS.DataValueField = "Code"
            End If
            CONTROL__PROGRESS_STATUS.DataBind()
        End Sub

        Function CheckDuplicateClaims(ByVal oOpenClaim As NexusProvider.ClaimOpen) As Boolean
            Dim sInsuranceFileRef As String = oOpenClaim.PolicyNumber
            Dim dLossStartDate As Date = oOpenClaim.LossFromDate

            Dim oClaimSearchCriteria As New NexusProvider.ClaimSearchCriteria
            Dim htCriteria As New Hashtable()

            oClaimSearchCriteria.InsuranceFileRef = Trim(sInsuranceFileRef)
            oClaimSearchCriteria.IncludeClosedClaim = True
            oClaimSearchCriteria.LossDateFrom = FormatDateTime(dLossStartDate, DateFormat.ShortDate)
            oClaimSearchCriteria.LossDateTo = FormatDateTime(dLossStartDate, DateFormat.ShortDate) & " 11:59:59 PM"
            oClaimSearchCriteria.RiskKey = oOpenClaim.RiskKey

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaims As NexusProvider.ClaimCollection

            oClaims = oWebservice.FindClaim(oClaimSearchCriteria)
            If oClaims.Count = 0 Then
                Return False
            Else
                Session(CNClaimsSearchData) = oClaims
                Return True
            End If
        End Function


        Protected Sub VldTime_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles VldTime.ServerValidate
            'Validate Time
            If CONTROL__LOSS_TIME.Text.Trim.Length = 0 Then
                args.IsValid = False
            ElseIf CONTROL__REPORTED_TIME.Text.Trim.Length = 0 Then
                args.IsValid = False
            ElseIf CONTROL__CLAIM_STATUSTIME.Text.Trim.Length = 0 Then
                args.IsValid = False
            ElseIf CONTROL__LOSS_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__LOSS_TIME.Text.Trim) = False Then
                args.IsValid = False
            ElseIf CONTROL__REPORTED_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__REPORTED_TIME.Text.Trim) = False Then
                args.IsValid = False
            ElseIf CONTROL__CLAIM_STATUSTIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__CLAIM_STATUSTIME.Text.Trim) = False Then
                args.IsValid = False
            End If
        End Sub

        Function ValidTime(ByVal sTime As String) As Boolean
            Dim bStatus As Boolean = True
            Dim iResult As Integer
            Dim Time() As String = sTime.Split(":")
            If Time.Length > 2 Or Time.Length = 1 Then
                bStatus = False
            Else
                For iCount As Integer = 0 To Time.Length - 1
                    If Integer.TryParse(Time(iCount), iResult) = False Then
                        bStatus = False
                    Else
                        If CInt(Time(0)) > 24 Or CInt(Time(iCount)) < 0 Then
                            'Hour Validation
                            bStatus = False
                        End If
                        If CInt(Time(1)) > 59 Or CInt(Time(iCount)) < 0 Then
                            'Minutes Validation
                            bStatus = False
                        End If
                    End If
                Next
            End If
            Return bStatus
        End Function
        Public Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim sClaimNumber As String = CStr(Session(CNClaim).ClaimNumber)
            Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
            Dim oQuote As NexusProvider.Quote = Nothing
            Dim oBaseParty As NexusProvider.BaseParty = Nothing
            Dim iHighest As Integer = 0

            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

            Try
                oClaimVersions = oWebservice.GetVersionsForClaim(sClaimNumber)
                If oClaimVersions IsNot Nothing Then
                    'Find Highest Version

                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iCount).Version > iHighest Then
                            iHighest = oClaimVersions(iCount).Version
                        End If
                    Next

                    'Updating of claim quote oQuote
                    oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey)
                    If oQuote IsNot Nothing Then
                        oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                        Session.Item(CNParty) = oBaseParty
                        Session.Item(CNRisks) = oQuote.Risks
                        Session.Item(CNRenewalDate) = oQuote.RenewalDate
                        Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & oBaseParty.Addresses(0).Address4
                        Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                        Session(CNInsurer_Header) = oQuote.InsuredName
                        Session(CNProductCode) = oQuote.ProductCode
                        Session(CNClaimQuote) = oQuote
                    End If
                    Session(CNClaimVersion) = oClaimVersions
                    Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                    Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                    'Response.Redirect("FindClaim.aspx")
                End If
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version = iHighest Then
                        'To Check whether Payment is pending for Authorization
                        oCashListItem = oWebservice.GetReferredPayments()
                        For Each oCashList As NexusProvider.CashListItems In oCashListItem
                            If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                Session.Item(CNMode) = Mode.ViewClaim
                                AllowClaimPayment.Enabled = True
                                AllowClaimPayment.IsValid = False
                                Exit Sub
                            End If
                        Next

                        'Retreival of claim details
                        Dim sBranchCode As String = oQuote.BranchCode
                        'arch issue 268
                        oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)

                        'Updation of session with claim details
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
                            oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
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
                            oOpenClaim.LossFromDate = .LossFromDate
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
                            oOpenClaim.UserDefFldDCode = .UserDefFldDCode
                            oOpenClaim.UserDefFldECode = .UserDefFldECode
                            oOpenClaim.TPA = .TPA
                            'Added for Insurer
                            oOpenClaim.Insurer = .Insurer
                            Session.Item(CNClaimTimeStamp) = .TimeStamp
                            oOpenClaim.CurrencyISOCode = .CurrencyCode
                            Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                            oOpenClaim.Client = .Client
                            'this needs to be removed after SAM issue is resolved
                            If oOpenClaim.Client.PartyKey = 0 Then
                                oOpenClaim.Client.PartyKey = oQuote.PartyKey
                            End If
                            'Session(CNInsurer_Header) = .ClientName
                            Session(CNClaimNumber) = .ClaimNumber
                            Session(CNStatus) = .ClaimStatus

                            'Check Recovery Reserve
                            Dim bAvailableReserve As Boolean = False

                        End With
                    End If
                Next

                Session(CNClaim) = oOpenClaim
                Session.Item(CNMode) = Mode.EditClaim
                Response.Redirect("~/Claims/Overview.aspx", False)
            Finally
                oClaimDetails = Nothing
                oWebservice = Nothing
                oClaimRisk = Nothing
            End Try

        End Sub

        Public Sub BtnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim sClaimNumber As String = CStr(Session(CNClaim).ClaimNumber)
            Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
            Dim oQuote As NexusProvider.Quote = Nothing
            Dim oBaseParty As NexusProvider.BaseParty = Nothing
            Dim iHighest As Integer = 0

            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

            Try
                oClaimVersions = oWebservice.GetVersionsForClaim(sClaimNumber)
                If oClaimVersions IsNot Nothing Then
                    'Find Highest Version

                    For iCount As Integer = 0 To oClaimVersions.Count - 1
                        If oClaimVersions(iCount).Version > iHighest Then
                            iHighest = oClaimVersions(iCount).Version
                        End If
                    Next

                    'Updating of claim quote oQuote
                    oQuote = oWebservice.GetHeaderAndSummariesByKey(oClaimVersions(0).InsuranceFileKey)
                    If oQuote IsNot Nothing Then
                        oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                        Session.Item(CNParty) = oBaseParty
                        Session.Item(CNRisks) = oQuote.Risks
                        Session.Item(CNRenewalDate) = oQuote.RenewalDate
                        Session.Item(CNAddress) = oBaseParty.Addresses(0).Address1 & ", " & oBaseParty.Addresses(0).Address4
                        Session.Item(CNDate_Header) = oQuote.CoverStartDate.ToShortDateString & " - " & oQuote.CoverEndDate.ToShortDateString
                        Session(CNInsurer_Header) = oQuote.InsuredName
                        Session(CNProductCode) = oQuote.ProductCode
                        Session(CNClaimQuote) = oQuote
                    End If
                    Session(CNClaimVersion) = oClaimVersions
                    Session.Item(CNInsuranceFileKey) = oClaimVersions(0).InsuranceFileKey
                    Session.Item(CNPolicyNumber) = oClaimVersions(0).InsuranceRef
                    'Response.Redirect("FindClaim.aspx")
                End If
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version = iHighest Then

                        ' Begin - WPR VB 64 - Media Type Status 
                        Dim CheckMediatypeStatusAtPolicyRefund As String = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                   NexusProvider.ProductRiskOptions.CheckMediatypeStatusAtClaimPayment, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing, oQuote.BranchCode).Trim()

                        If CheckMediatypeStatusAtPolicyRefund.Contains("1") Then
                            Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
                            With oMediaTypeStatus
                                .InsuranceFileKey = oQuote.InsuranceFileKey
                                .LossDateSpecified = False
                            End With
                            oWebservice.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
                            'SAM Return the False intead of True, if unclear fund exist then it retirn False or else true
                            If Not oMediaTypeStatus.IsUnclearedCashListExists Then
                                vldMediaTypeStatus.IsValid = False
                                vldMediaTypeStatus.ClientValidationFunction = "ClaimValidation"
                                Exit Sub
                            End If
                        End If

                        ' End - WPR VB 64 - Media Type Status 

                        'To Check whether Payment is pending for Authorization

                        oCashListItem = oWebservice.GetReferredPayments()
                        For Each oCashList As NexusProvider.CashListItems In oCashListItem
                            If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                AllowClaimPayment.IsValid = False
                                AllowClaimPayment.ClientValidationFunction = "ClaimValidation"
                                Exit Sub
                            End If
                        Next
                        'Retreival of claim details
                        Dim sBranchCode As String = oQuote.BranchCode
                        'arch issue 268
                        oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)

                        'check for closed claim
                        If oClaimDetails IsNot Nothing Then
                            If Not String.IsNullOrEmpty(oClaimDetails.ClaimStatus) AndAlso oClaimDetails.ClaimStatus.Trim.ToUpper = "CLOSED" Then
                                ChkClosedClaim.IsValid = False
                                ChkClosedClaim.ClientValidationFunction = "ClaimValidation"
                                Exit Sub
                            End If
                        End If

                        'Updation of session with claim details
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
                            oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
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
                            oOpenClaim.LossFromDate = .LossFromDate
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
                            oOpenClaim.UserDefFldDCode = .UserDefFldDCode
                            oOpenClaim.UserDefFldECode = .UserDefFldECode
                            oOpenClaim.TPA = .TPA
                            'Added for Insurer
                            oOpenClaim.Insurer = .Insurer
                            Session.Item(CNClaimTimeStamp) = .TimeStamp
                            oOpenClaim.CurrencyISOCode = .CurrencyCode
                            Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                            oOpenClaim.Client = .Client
                            'this needs to be removed after SAM issue is resolved
                            If oOpenClaim.Client.PartyKey = 0 Then
                                oOpenClaim.Client.PartyKey = oQuote.PartyKey
                            End If
                            'Session(CNInsurer_Header) = .ClientName
                            Session(CNClaimNumber) = .ClaimNumber
                            Session(CNStatus) = .ClaimStatus

                            'Check Recovery Reserve
                            Dim bAvailableReserve As Boolean = False


                            'Retreival of the risk related values 
                            'Arch issue 268
                            oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                            Session(CNDataSet) = oClaimRisk.XMLDataSet

                        End With
                    End If
                Next

                Session(CNClaim) = oOpenClaim
                Session.Item(CNMode) = Mode.PayClaim
                Response.Redirect("~/Claims/Overview.aspx", False)
            Finally
                oClaimDetails = Nothing
                oWebservice = Nothing
                oClaimRisk = Nothing
            End Try

        End Sub


        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            If CType(Session(CNMode), Mode) = Mode.NewClaim Then

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "InvalidDatesConfirmation",
                             "<script language=""JavaScript"" type=""text/javascript"">function InvalidDatesConfirmation(){debugger;return confirm('" & GetLocalResourceObject("msg_ConfirmInvalidDates").ToString() & "');}</script>")

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "InvalidDatesValidation",
                             "<script language=""JavaScript"" type=""text/javascript"">function InvalidDatesValidation(){debugger; alert('" & GetLocalResourceObject("msg_InvalidDatesValidation").ToString() & "'); return false;}</script>")

            ElseIf CType(Session(CNMode), Mode) = Mode.EditClaim Then

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "InceptionConfirmation",
                  "<script language=""JavaScript"" type=""text/javascript"">function InceptionConfirmation(sMsg){ return confirm(sMsg);}</script>")
            End If

        End Sub

        Public Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oBtnAddWorkMgrTask As Button = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("btnAddWorkMgrTask"), Button)
            If oBtnAddWorkMgrTask IsNot Nothing Then
                If HttpContext.Current.Session.IsCookieless Then
                    oBtnAddWorkMgrTask.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/WrmTask.aspx?modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                Else
                    oBtnAddWorkMgrTask.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/WrmTask.aspx?modal=true&FromPage=WM&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                End If
            End If
        End Sub


        Protected Sub CONTROL__LOSS_DATE_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CONTROL__LOSS_DATE.TextChanged
            'Check if Date completely entered
            btnNext.Attributes.Remove("onclick")
            If IsDate(CONTROL__LOSS_DATE.Text) = False Or CONTROL__LOSS_DATE.Text.Length <> 10 Then
                Exit Sub
            End If

            If CDate(CONTROL__LOSS_DATE.Text) > Now.Date Then
                'btnNext.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("rgvClaimNotification").ToString() & "'); return false;")
                Dim ClientScriptName As String = "checkClaimNotificationDate"
                Dim PageType As Type = Me.GetType()
                Dim csm As ClientScriptManager = Page.ClientScript
                If (Not csm.IsClientScriptBlockRegistered(PageType, ClientScriptName)) Then
                    Dim ClientScriptText As New StringBuilder()
                    ClientScriptText.AppendLine("function compareDate(){if(fn_DateCompare(getClaimNotificationDate(),'" + Now.Date + "')){alert('" & GetLocalResourceObject("rgvClaimNotification").ToString() & "'); return false;} }")
                    csm.RegisterClientScriptBlock(PageType, ClientScriptName, ClientScriptText.ToString(), True)
                End If
                'btnNext.Attributes.Add("onclick", "return compareDate();")
                Exit Sub
            End If


            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails
            Dim oInsuranceFileDetails As NexusProvider.PolicyCollection

            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim dtStartDate As Date, dtEndDate As Date, dtInceptionDate As Date
            Dim nReturnCode As Integer
            Dim sInsuranceRef As String
            Dim sTypeCode As String
            Dim dLossDate As Date
            Dim iInsuranceFileKey As Integer
            Dim iReturnCode As Integer
            Const ReturnCode_Error As Integer = 0
            Const ReturnCode_Ok As Integer = 1
            Const ReturnCode_TooEarly As Integer = 2
            Const ReturnCode_TooLate As Integer = 3

            With oInsuranceFileSearchCriteria
                sInsuranceRef = CStr(Session.Item(CNPolicyNumber))
                dLossDate = CDate(Trim(CONTROL__LOSS_DATE.Text))
                iInsuranceFileKey = CInt(Session.Item(CNInsuranceFileKey))
            End With

            Dim dPreviousLossDate As Date
            Dim oOpenClaim As NexusProvider.ClaimOpen = Nothing
            Dim sJS As String = String.Empty
            oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            dPreviousLossDate = oOpenClaim.LossFromDate.Date
            rangevalLossToDate.MinimumValue = FormatDateTime(dLossDate, DateFormat.ShortDate)

            If ViewState("oInsuranceFileDetails") Is Nothing Then
                ViewState("oInsuranceFileDetails") = oWebService.GetAllPolicyVersions(Session.Item(CNInsuranceFolderKey), sBranchCode)
            End If
            oInsuranceFileDetails = DirectCast(ViewState("oInsuranceFileDetails"), NexusProvider.PolicyCollection)

            GetPolicyForClaimDate(oInsuranceFileDetails, dLossDate, iInsuranceFileKey, sInsuranceRef, dtStartDate, dtEndDate, nReturnCode, dtInceptionDate, sTypeCode)
            btnNext.Attributes.Clear()
            If (nReturnCode = ReturnCode_TooEarly) Then
                If CType(Session(CNMode), Mode) = Mode.NewClaim OrElse CType(Session(CNMode), Mode) = Mode.EditClaim Then
                    Dim nChkAttachClaimOutsideOfPolicyPeriod As Integer
                    nChkAttachClaimOutsideOfPolicyPeriod = Check_AttachClaimOutsideOfPolicyPeriod()
                    If nChkAttachClaimOutsideOfPolicyPeriod = "1" Then
                        'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "InvalidDatesValidation('" + GetLocalResourceObject("msg_InvalidDatesValidation") + "');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "InvalidDatesConfirmation('" + GetLocalResourceObject("msg_ConfirmInvalidDates") + "');", True)
                    End If
                Else
                    If Session(CNDisplayValidVersion) = "1" Then
                        btnNext.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtStartDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate)).Replace("Do you want to continue?", "") & "');return false;")
                    Else
                        btnNext.Attributes.Add("onclick", "javascript:if (InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtStartDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate)) & "')==true){return confirm('" & GetLocalResourceObject("msg_ConfirmAmendLoss").ToString() & "');}else {return false;}")
                    End If
                End If
            ElseIf Session(CNInsuranceFileStatus) = "CAN" And Session(CNLapseDate) <= CDate(CONTROL__LOSS_DATE.Text) Then
                btnNext.Attributes.Add("onclick", "javascript:return confirm('" & GetLocalResourceObject("msg_ConfirmCancel").ToString().Replace("#CoverFrom#", CDate(Session(CNLapseDate)).ToShortDateString()) & "');")
                sJS = "if(confirm('" & GetLocalResourceObject("msg_ConfirmCancel").ToString().Replace("#CoverFrom#", CDate(Session(CNLapseDate)).ToShortDateString()) & "')==false){return false;}"
            ElseIf Session(CNInsuranceFileStatus) = "LAP" Then
                btnNext.Attributes.Add("onclick", "javascript:return confirm('" & GetLocalResourceObject("msg_ConfirmLapse").ToString() & "');")
                sJS = "if(confirm('" & GetLocalResourceObject("msg_ConfirmLapse").ToString() & "')==false){return false;}"
            ElseIf (nReturnCode = ReturnCode_TooLate) Then
                If CType(Session(CNMode), Mode) = Mode.NewClaim OrElse CType(Session(CNMode), Mode) = Mode.EditClaim Then
                    Dim nChkAttachClaimOutsideOfPolicyPeriod As Integer
                    nChkAttachClaimOutsideOfPolicyPeriod = Check_AttachClaimOutsideOfPolicyPeriod()
                    If nChkAttachClaimOutsideOfPolicyPeriod = "1" Then
                        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "InvalidDatesValidation('" + GetLocalResourceObject("msg_InvalidDatesValidation") + "');", True)

                    Else
                        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "InvalidDatesConfirmation('" + GetLocalResourceObject("msg_ConfirmInvalidDates") + "');", True)

                    End If

                Else
                    If Session(CNDisplayValidVersion) = "1" Then
                        btnNext.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtStartDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate)).Replace("Do you want to continue?", "") & "');return false;")
                    Else
                        btnNext.Attributes.Add("onclick", "javascript:if (InceptionConfirmation('" & GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtStartDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate)) & "')==true){return confirm('" & GetLocalResourceObject("msg_ConfirmAmendLoss").ToString() & "');}else {return false;}")
                    End If
                End If
            ElseIf ((iInsuranceFileKey <> Session(CNInsuranceFileKey) Or CType(Session(CNMode), Mode) = Mode.EditClaim) And (Date.Compare(dPreviousLossDate, dLossDate) <> 0)) Then
                btnNext.Attributes.Add("onclick", "javascript:return confirm('" & GetLocalResourceObject("msg_ConfirmAmendLoss").ToString() & "');")
                sJS = "javascript:return confirm('" & GetLocalResourceObject("msg_ConfirmAmendLoss").ToString() & "');"
            End If

            If CDate(CONTROL__LOSS_DATE.Text) > Now.Date Then
                btnNext.Attributes.Add("onclick", "javascript:alert('" & GetLocalResourceObject("rgvClaimNotification").ToString() & "'); return false;")
            End If

            btnNext.Enabled = True
            Session(CNJavaScript) = sJS

            If CType(Session(CNMode), Mode) = Mode.NewClaim Then
                Dim oQuote As NexusProvider.Quote
                Dim oNonDeletedRiskTypes = New NexusProvider.RiskCollection

                If iInsuranceFileKey = 0 Then
                    iInsuranceFileKey = Session(CNOriginalInsuranceFileKey)
                End If

                oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey, sBranchCode, True)

                Dim lstDeletedRisk As New List(Of String)()
                For Each tempRisk As NexusProvider.Risk In oQuote.Risks
                    If Trim(tempRisk.StatusCode) = "DELETED" Then
                        lstDeletedRisk.Add(tempRisk.Key)
                    End If
                    oNonDeletedRiskTypes.Add(tempRisk)
                Next

                ddlRiskType.Items.Clear()
                ddlRiskType.DataSource = oNonDeletedRiskTypes
                ddlRiskType.DataTextField = "Description"
                ddlRiskType.DataValueField = "Key"
                ddlRiskType.DataBind()
                ddlRiskType.Items.Insert(0, New ListItem("Please Select", ""))

                If lstDeletedRisk IsNot Nothing Then
                    If lstDeletedRisk.Count > 0 Then
                        For i = 0 To lstDeletedRisk.Count - 1
                            Dim index As Integer = ddlRiskType.Items.IndexOf(ddlRiskType.Items.FindByValue(lstDeletedRisk(i)))
                            ddlRiskType.Items(index).Enabled = False
                        Next
                    End If
                    lstDeletedRisk.Clear()
                End If

                If ddlRiskType.Items.Count = 2 Then
                    ddlRiskType.SelectedIndex = 1
                End If

                Session(CNClaimQuote) = oQuote
                Session(CNInsuranceFileKey) = iInsuranceFileKey
            End If


            If CType(Session(CNMode), Mode) = Mode.NewClaim And ddlRiskType.SelectedIndex > 0 Then
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                Dim sRiskTypeCode As String = oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskTypeCode

                sClaimsCoverBasis = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.ClaimsCoverBasis, NexusProvider.RiskTypeOptions.ClaimsCoverBasis, "", sRiskTypeCode)
                ValidateLossDate(sClaimsCoverBasis, oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate, oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskInceptionDate, oQuote.CoverEndDate)
            End If

            If CType(Session(CNMode), Mode) = Mode.EditClaim AndAlso dLossDate <> dPreviousLossDate Then
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                Dim sRiskTypeCode As String = oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskTypeCode
                ValidateLossDate(sClaimsCoverBasis, oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate, oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskInceptionDate, oQuote.CoverEndDate)
            End If

        End Sub

        Private Sub ValidateLossDate(ByVal v_sClaimsCoverBasis As String, ByVal v_dtRetroactiveDate As Date, ByVal v_dtRiskInceptionDate As Date, ByVal v_dtInsFileExpiryDate As Date, Optional ByRef sMessage As String = "")
            Dim sJS As String
            If Session(CNJavaScript) IsNot Nothing Then
                sJS = Session(CNJavaScript)
            End If
            If v_sClaimsCoverBasis = "2" Or v_sClaimsCoverBasis = "1" Then 'Claim Made
                If v_dtRetroactiveDate <> Date.MinValue Then
                    If (CONTROL__LOSS_DATE.Text < v_dtRetroactiveDate Or CONTROL__LOSS_DATE.Text > v_dtInsFileExpiryDate) And (CONTROL__LOSS_DATE.Text < v_dtInsFileExpiryDate Or CONTROL__LOSS_DATE.Text > v_dtRetroactiveDate) Then
                        sMessage = GetLocalResourceObject("Claim_Made_Error").ToString()
                        Dim sScript As String = "ClaimMadeError('" + sMessage + "');"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "thickbox", sScript, True)
                    ElseIf (CONTROL__REPORTED_DATE.Text < v_dtRiskInceptionDate Or CONTROL__REPORTED_DATE.Text > v_dtInsFileExpiryDate) And (CONTROL__REPORTED_DATE.Text < v_dtInsFileExpiryDate Or CONTROL__REPORTED_DATE.Text > v_dtRiskInceptionDate) Then
                        btnNext.Attributes.Add("onclick", sJS & "alert('" & GetLocalResourceObject("Report_Date_Outside_Cover").ToString() & "'); return false;")
                        sMessage = GetLocalResourceObject("Report_Date_Outside_Cover").ToString()
                    End If
                    'Else
                    'btnNext.Attributes.Add("onclick", sJS & "alert('" & GetLocalResourceObject("Retroactive_Date_Error").ToString() & "');")
                    'sMessage = GetLocalResourceObject("Retroactive_Date_Error").ToString()
                End If


            ElseIf v_sClaimsCoverBasis = "3" Then 'Tail Cover
                If v_dtRetroactiveDate <> Date.MinValue Then

                    If (CONTROL__LOSS_DATE.Text < v_dtRiskInceptionDate Or CONTROL__LOSS_DATE.Text > v_dtRetroactiveDate) And (CONTROL__LOSS_DATE.Text < v_dtRetroactiveDate Or CONTROL__LOSS_DATE.Text > v_dtRiskInceptionDate) Then
                        sMessage = GetLocalResourceObject("Tail_Cover_Error").ToString()
                        btnNext.Attributes.Add("onclick", sJS & "alert('" & GetLocalResourceObject("Tail_Cover_Error").ToString() & "'); return false;")
                    ElseIf (CONTROL__REPORTED_DATE.Text < v_dtRiskInceptionDate Or CONTROL__REPORTED_DATE.Text > v_dtInsFileExpiryDate) And (CONTROL__REPORTED_DATE.Text < v_dtInsFileExpiryDate Or CONTROL__REPORTED_DATE.Text > v_dtRiskInceptionDate) Then
                        sMessage = GetLocalResourceObject("Report_Date_Outside_Cover").ToString()
                        btnNext.Attributes.Add("onclick", sJS & "alert('" & GetLocalResourceObject("Report_Date_Outside_Cover").ToString() & "'); return false;")
                    End If
                Else
                    sMessage = GetLocalResourceObject("Retroactive_Date_Error").ToString()
                    btnNext.Attributes.Add("onclick", sJS & "alert('" & GetLocalResourceObject("Retroactive_Date_Error").ToString() & "');")
                End If
            Else
                If Not String.IsNullOrEmpty(sJS) Then
                    btnNext.Attributes.Add("onclick", sJS)
                End If
            End If
        End Sub

        Public Function GetPolicyForClaimDate(ByVal v_oPolicies As NexusProvider.PolicyCollection, ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, ByRef r_dtEndDate As Date, Optional ByRef r_lReturnCode As Integer = 0, Optional ByRef r_dtInceptionDate As Date = #12:00:00 PM#, Optional ByRef r_sTypeCode As String = "") As Integer

            Dim result As Integer = 0
            Dim lCurrentPosition As Integer

            Dim dtFirstStartDate, dtLastExpiryDate As Date
            Dim bFoundDate As Boolean

            Const ReturnCode_Error As Integer = 0
            Const ReturnCode_Ok As Integer = 1
            Const ReturnCode_TooEarly As Integer = 2
            Const ReturnCode_TooLate As Integer = 3


            If v_oPolicies IsNot Nothing Then
                For lCount As Integer = 0 To v_oPolicies.Count - 1

                    If r_lInsuranceFileCnt = v_oPolicies.Item(lCount).InsuranceFileKey Then

                        r_dtStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)

                        r_dtEndDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                        bFoundDate = True
                        Exit For
                    End If
                Next lCount

                ' if we havent found a date yet then
                If Not bFoundDate Then

                    For lCount As Integer = 0 To v_oPolicies.Count - 1
                        If (v_oPolicies.Item(lCount).InsuranceFileTypeKey = 2 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 5 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 6 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 8 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 9) Then

                            r_dtStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)

                            r_dtEndDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                            bFoundDate = True
                            Exit For
                        End If
                    Next lCount
                End If

                ' Find a version of the policy which encompasses the claim date
                lCurrentPosition = -1

                For lCount As Integer = v_oPolicies.Count - 1 To 0 Step -1

                    If (v_oPolicies.Item(lCount).InsuranceFileTypeKey = 2 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 5 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 6 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 8 Or v_oPolicies.Item(lCount).InsuranceFileTypeKey = 9 OrElse v_oPolicies.Item(lCount).InsuranceFileTypeKey = 11) Then
                        If (v_oPolicies.Item(lCount).CoverStartDate <= v_dtClaimDate) And (v_dtClaimDate <= v_oPolicies.Item(lCount).ExpiryDate) Then

                            ' We found a valid one, note its position and exit!
                            lCurrentPosition = lCount
                            Exit For
                        End If

                        ' Save the earliest start date and the latest expiry date

                        If CDate(v_oPolicies.Item(lCount).CoverStartDate) < dtFirstStartDate Or lCount = 0 Then

                            dtFirstStartDate = CDate(v_oPolicies.Item(lCount).CoverStartDate)
                        End If

                        If CDate(v_oPolicies.Item(lCount).ExpiryDate) > dtLastExpiryDate Or lCount = 0 Then

                            dtLastExpiryDate = CDate(v_oPolicies.Item(lCount).ExpiryDate)
                        End If
                    End If
                Next lCount

                r_lInsuranceFileCnt = 0
                r_dtInceptionDate = dtFirstStartDate

                If lCurrentPosition <> -1 Then

                    ' Found one, we returns its details

                    r_sPolicyNumber = v_oPolicies.Item(lCurrentPosition).InsuranceFileRef

                    r_lInsuranceFileCnt = v_oPolicies.Item(lCurrentPosition).InsuranceFileKey

                    r_sTypeCode = v_oPolicies.Item(lCurrentPosition).InsuranceFileTypeCode

                    r_lReturnCode = ReturnCode_Ok

                Else

                    ' Else, we check why we haven't found one, to report to user
                    If v_dtClaimDate > dtLastExpiryDate Then
                        ' The claim date if after the latest expiry date
                        r_lReturnCode = ReturnCode_TooLate
                    ElseIf v_dtClaimDate < dtFirstStartDate Then
                        ' The claim date is before the earliest start date
                        r_lReturnCode = ReturnCode_TooEarly
                    Else
                        ' We can't find any policy, probably because the claim date is in between
                        ' two valid policy versions
                        r_lReturnCode = ReturnCode_Error
                    End If

                End If
            End If
            Return result

        End Function


        ''' <summary>
        ''' Validation to check if the TPA code is correct.
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Sub cvParty_ServerValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs)

            If PartyName.PartyCode.Trim <> "" And (PartyName.PartyKey = 0 And bolCheckPartyKey) Then
                args.IsValid = False
            End If

        End Sub

        Protected Sub ddlRiskType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRiskType.SelectedIndexChanged
            If CType(Session(CNMode), Mode) = Mode.NewClaim And ddlRiskType.SelectedIndex > 0 Then
                Dim sClaimsTypeBasis As String
                Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                Dim sRiskTypeCode As String = oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskTypeCode
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                sClaimsCoverBasis = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.ClaimsCoverBasis, NexusProvider.RiskTypeOptions.ClaimsCoverBasis, "", sRiskTypeCode)
                sClaimsTypeBasis = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.ClaimsTypeBasis, NexusProvider.RiskTypeOptions.ClaimsTypeBasis, "", sRiskTypeCode)

                If sClaimsTypeBasis = "1" And CONTROL__LOSS_DATE.Text = "" And ViewState("LOSS_DATE_OVERRIDE") <> True Then
                    CONTROL__LOSS_DATE.Text = FormatDateTime(Session(CNLossDate), DateFormat.ShortDate)
                    'CONTROL__LOSS_TODATE.Text = FormatDateTime(Session(CNLossToDate), DateFormat.ShortDate)
                    ViewState("LOSS_DATE_OVERRIDE") = True
                ElseIf sClaimsTypeBasis = "2" And CONTROL__REPORTED_DATE.Text = Now.Date And ViewState("REPORTED_DATE_OVERRIDE") <> True Then
                    CONTROL__REPORTED_DATE.Text = FormatDateTime(Session(CNLossDate), DateFormat.ShortDate)
                    ViewState("REPORTED_DATE_OVERRIDE") = True
                End If
                If sClaimsTypeBasis = "1" AndAlso CONTROL__LOSS_TODATE.Text = String.Empty AndAlso ViewState("LOSS_DATE_OVERRIDE") <> True Then
                    If CType(Session(CNLossToDate), Date) < CType(CONTROL__LOSS_DATE.Text, Date) Then
                        CONTROL__LOSS_TODATE.Text = CONTROL__LOSS_DATE.Text
                    Else
                        CONTROL__LOSS_TODATE.Text = FormatDateTime(Session(CNLossToDate), DateFormat.ShortDate)
                    End If
                    ViewState("LOSS_DATE_OVERRIDE") = True
                End If
                If IsDate(CONTROL__LOSS_DATE.Text) Then
                    ValidateLossDate(sClaimsCoverBasis, oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate, oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskInceptionDate, oQuote.CoverEndDate)
                End If
            End If
        End Sub
        Private Function Check_AttachClaimOutsideOfPolicyPeriod() As Integer
            Dim nResult As Integer = 0
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim iInsuranceFileKey As Integer
            Dim oQuote As NexusProvider.Quote
            Try

                If iInsuranceFileKey = 0 Then
                    iInsuranceFileKey = Session(CNOriginalInsuranceFileKey)
                End If
                If Not Session(CNClaimQuote) Is Nothing Then
                    oQuote = Session(CNClaimQuote)
                Else
                    oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey, sBranchCode, True)
                End If

                Dim sRiskTypeCode As String
                If ddlRiskType.SelectedIndex > 0 Then

                    sRiskTypeCode = oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskTypeCode
                Else

                    sRiskTypeCode = oQuote.Risks(ddlRiskType.SelectedIndex).RiskTypeCode
                End If
                Dim sAttachClaimOutsideOfPolicyPeriod As String
                sAttachClaimOutsideOfPolicyPeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, Nothing, NexusProvider.RiskTypeOptions.AttachClaimOutsideOfPolicyPeriod, "", sRiskTypeCode)
                sClaimsCoverBasis = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.ClaimsCoverBasis, NexusProvider.RiskTypeOptions.ClaimsCoverBasis, "", sRiskTypeCode)

                If sAttachClaimOutsideOfPolicyPeriod = "1" AndAlso sClaimsCoverBasis = "1" Then
                    nResult = 1
                Else
                    nResult = 0
                End If
            Finally
                oWebService = Nothing
                oUserDetails = Nothing
                oQuote = Nothing
            End Try


            Return nResult
        End Function
        ''' <summary>
        ''' ValidateClaimDates
        ''' </summary>
        ''' <param name="sMessage"></param>
        ''' <param name="sMessageType"></param>
        ''' <remarks></remarks>
        Private Sub ValidateClaimDates(ByRef sMessage As String, ByRef sMessageType As String)

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails
            Dim oInsuranceFileDetails As NexusProvider.PolicyCollection

            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim dtStartDate As Date, dtEndDate As Date, dtInceptionDate As Date
            Dim nReturnCode As Integer
            Dim sInsuranceRef As String, sTypeCode As String
            Dim dtLossDate As Date
            Dim nInsuranceFileKey As Integer
            Dim oQuote As NexusProvider.Quote
            Const nReturnCode_TooEarly As Integer = 2
            Const nReturnCode_TooLate As Integer = 3

            oQuote = Session(CNClaimQuote)
            If Session(CNMode) = Mode.NewClaim OrElse Session(CNMode) = Mode.EditClaim Then

                'this will check 
                Dim CONTROL__LOSS, CONTROL__REPORTED As DateTime
                'Loss Date must not be later than Loss To Date
                If CDate(CONTROL__LOSS_DATE.Text) > CDate(CONTROL__LOSS_TODATE.Text) Then
                    sMessage = GetLocalResourceObject("lb_LossToDate").ToString()
                    sMessageType = "ALERT"
                    Exit Sub
                End If

                If CONTROL__LOSS_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__LOSS_TIME.Text.Trim) = True And CONTROL__REPORTED_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__REPORTED_TIME.Text.Trim) = True Then
                    CONTROL__LOSS = CONTROL__LOSS_DATE.Text.Trim + " " + CONTROL__LOSS_TIME.Text.Trim
                    CONTROL__REPORTED = CONTROL__REPORTED_DATE.Text.Trim + " " + CONTROL__REPORTED_TIME.Text.Trim
                    'check the Loss date in future and greater than Reported Date
                    If CDate(CONTROL__LOSS) <= CDate(CONTROL__REPORTED) And CDate(CONTROL__LOSS) < DateTime.Now Then
                    Else
                        'When loss date is greater than the current date then
                        If CDate(CONTROL__LOSS_DATE.Text) > Now.Date Then
                            sMessage = GetLocalResourceObject("rgvClaimNotification").ToString()
                            'When loss date is greater than reporting date then  
                        ElseIf Date.Compare(CONTROL__REPORTED_DATE.Text, CONTROL__LOSS_DATE.Text) < 0 Then
                            sMessage = GetLocalResourceObject("lbl_ReportedDate").ToString()
                            'When loss date  = reporting date then  
                        ElseIf Date.Compare(CONTROL__REPORTED_DATE.Text, CONTROL__LOSS_DATE.Text) = 0 Then
                            'When loss time  is greater than reporting time then
                            If DateTime.Compare(CONTROL__REPORTED_TIME.Text, CONTROL__LOSS_TIME.Text) < 0 Then
                                sMessage = GetLocalResourceObject("lb_LossTime").ToString()
                            End If
                        End If
                        sMessageType = "ALERT"
                        Exit Sub
                    End If
                    'check the Reported date in future
                    If CDate(CONTROL__REPORTED) > DateTime.Now Then
                        'When reporting date is greater than Current date then
                        If Date.Compare(FormatDateTime(Now, DateFormat.ShortDate), CONTROL__REPORTED_DATE.Text) < 0 Then
                            sMessage = GetLocalResourceObject("Report_date_Future_Error").ToString()
                            'When reporting date = Current date then
                        ElseIf Date.Compare(FormatDateTime(Now, DateFormat.ShortDate), CONTROL__REPORTED_DATE.Text) = 0 Then
                            'When reporting time is greater than Current time then 
                            If DateTime.Compare(FormatDateTime(Now, DateFormat.ShortTime), CONTROL__REPORTED_TIME.Text) < 0 Then
                                sMessage = GetLocalResourceObject("Report_time_Future_Error").ToString()
                            End If
                        End If
                        sMessageType = "ALERT"
                        Exit Sub
                    End If
                End If
            End If

            If CDate(CONTROL__LOSS_DATE.Text) > Now.Date Then
                sMessage = GetLocalResourceObject("rgvClaimNotification").ToString()
                sMessageType = "ALERT"
                Exit Sub
            ElseIf Date.Compare(CONTROL__REPORTED_DATE.Text, CONTROL__LOSS_DATE.Text) < 0 Then
                sMessage = GetLocalResourceObject("lbl_ReportedDate").ToString()
                sMessageType = "ALERT"
                Exit Sub
            End If

            With oInsuranceFileSearchCriteria
                sInsuranceRef = CStr(Session.Item(CNPolicyNumber))
                dtLossDate = CDate(Trim(CONTROL__LOSS_DATE.Text))
                nInsuranceFileKey = CInt(Session.Item(CNInsuranceFileKey))
            End With

            Dim dPreviousLossDate As Date
            Dim oOpenClaim As NexusProvider.ClaimOpen = Nothing
            oOpenClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            dPreviousLossDate = oOpenClaim.LossFromDate.Date

            If ViewState("oInsuranceFileDetails") Is Nothing Then
                ViewState("oInsuranceFileDetails") = oWebService.GetAllPolicyVersions(Session.Item(CNInsuranceFolderKey), sBranchCode)
            End If
            oInsuranceFileDetails = DirectCast(ViewState("oInsuranceFileDetails"), NexusProvider.PolicyCollection)

            GetPolicyForClaimDate(oInsuranceFileDetails, dtLossDate, nInsuranceFileKey, sInsuranceRef, dtStartDate, dtEndDate, nReturnCode, dtInceptionDate, sTypeCode)

            If CType(Session(CNMode), Mode) = Mode.NewClaim AndAlso ddlRiskType.SelectedIndex > 0 AndAlso oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate <> Date.MinValue Then

                Dim sRiskTypeCode As String = oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskTypeCode
                sClaimsCoverBasis = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.ClaimsCoverBasis, NexusProvider.RiskTypeOptions.ClaimsCoverBasis, "", sRiskTypeCode)
                ValidateLossDate(sClaimsCoverBasis, oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate, oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskInceptionDate, oQuote.CoverEndDate, sMessage)
                If Not String.IsNullOrEmpty(sMessage) Then
                    sMessageType = "ALERT"
                    Exit Sub
                End If
            End If

            If CType(Session(CNMode), Mode) = Mode.EditClaim AndAlso CDate(Trim(CONTROL__LOSS_DATE.Text)) <> oOpenClaim.LossFromDate.Date AndAlso oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate <> Date.MinValue Then
                Dim sRiskTypeCode As String = oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskTypeCode
                ValidateLossDate(sClaimsCoverBasis, oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate, oQuote.Risks(ddlRiskType.SelectedIndex - 1).RiskInceptionDate, oQuote.CoverEndDate, sMessage)
                If Not String.IsNullOrEmpty(sMessage) Then
                    sMessageType = "ALERT"
                    Exit Sub
                End If
            End If


            Dim nChkAttachClaimOutsideOfPolicyPeriod As Integer
            If (nReturnCode = nReturnCode_TooEarly) And nInsuranceFileKey <> Session(CNInsuranceFileKey) AndAlso oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate = Date.MinValue Then

                nChkAttachClaimOutsideOfPolicyPeriod = Check_AttachClaimOutsideOfPolicyPeriod()
                If nChkAttachClaimOutsideOfPolicyPeriod = "1" AndAlso CType(Session(CNMode), Mode) = Mode.NewClaim Then
                    'sMessage = GetLocalResourceObject("msg_InvalidDatesValidation").ToString
                    sMessage = GetLocalResourceObject("msg_ConfirmInvalidDates").ToString
                    sMessageType = "CONFIRM"

                Else
                    If Session(CNDisplayValidVersion) = "1" AndAlso CType(Session(CNMode), Mode) = Mode.EditClaim Then
                        sMessage = GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtInceptionDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate)).Replace("Do you want to continue?", "")
                        sMessageType = "ALERT"
                    Else
                        sMessage = GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtInceptionDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate))
                        sMessageType = "CONFIRM"
                    End If
                End If


            ElseIf Session(CNInsuranceFileStatus) = "CAN" And Session(CNLapseDate) <= CDate(CONTROL__LOSS_DATE.Text) Then
                sMessage = GetLocalResourceObject("msg_ConfirmCancel").ToString().Replace("#CoverFrom#", CDate(Session(CNLapseDate)).ToShortDateString())
                sMessageType = "CONFIRM"
            ElseIf Session(CNInsuranceFileStatus) = "LAP" Then
                sMessage = GetLocalResourceObject("msg_ConfirmLapse").ToString()
                sMessageType = "CONFIRM"
            ElseIf (nReturnCode = nReturnCode_TooLate) And nInsuranceFileKey <> Session(CNInsuranceFileKey) AndAlso oQuote.Risks(ddlRiskType.SelectedIndex - 1).GISRetroactiveDate = Date.MinValue Then
                nChkAttachClaimOutsideOfPolicyPeriod = Check_AttachClaimOutsideOfPolicyPeriod()
                If nChkAttachClaimOutsideOfPolicyPeriod = "1" Then
                    sMessage = GetLocalResourceObject("msg_InvalidDatesValidation").ToString
                    sMessageType = "ALERT"

                Else
                    If Session(CNDisplayValidVersion) = "1" Then
                        sMessage = GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtInceptionDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate)).Replace("Do you want to continue?", "")
                        sMessageType = "ALERT"
                    Else
                        sMessage = GetLocalResourceObject("msg_ConfirmInceptionDates").ToString.Replace("#CoverStartDate#", dtInceptionDate).Replace("#CoverToDate#", IIf(Session(CNInsuranceFileStatus) = "CAN", CDate(Session(CNLapseDate)).ToShortDateString(), dtEndDate))
                        sMessageType = "CONFIRM"
                    End If
                End If
            End If

        End Sub
        ''' <summary>
        ''' This will validate the date format for Loss date,Reported date and Loss to date
        ''' </summary>
        ''' <returns>Boolean</returns>
        ''' <remarks></remarks>
        Private Function ValidateDate() As Boolean
            Dim bIsValid As Boolean = True
            Dim sFailerMsg As String = String.Empty

            'Check if Date entered is correct

            If IsDate(CONTROL__LOSS_DATE.Text) = False Or CONTROL__LOSS_DATE.Text.Length <> 10 Then
                sFailerMsg = GetLocalResourceObject("Invalid_Lose_Date_Error").ToString() + "\n"
                bIsValid = False
            End If

            If IsDate(CONTROL__LOSS_TODATE.Text) = False Or CONTROL__LOSS_TODATE.Text.Length <> 10 Then

                sFailerMsg &= GetLocalResourceObject("Invalid_Lose_To_Date_Error").ToString() + "\n"
                bIsValid = False
            End If

            If IsDate(CONTROL__REPORTED_DATE.Text) = False Or CONTROL__REPORTED_DATE.Text.Length <> 10 Then
                sFailerMsg &= GetLocalResourceObject("Invalid_Reported_Date_Error").ToString() + "\n"
                bIsValid = False
            End If

            If (Not bIsValid) Then
                Dim divInvalidDate As New HtmlGenericControl
                divInvalidDate.InnerHtml = "<script type='text/javascript'>alert('" & sFailerMsg & "')</script>"
                Page.Controls.Add(divInvalidDate)
            End If

            Return bIsValid
        End Function

        Protected Sub cstmLossDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cstmLossDate.ServerValidate
            If Not IsValidDates() Then
                Exit Sub
            End If

            If CDate(CONTROL__LOSS_DATE.Text) > Date.Today Then

                args.IsValid = False
            End If
        End Sub

        Protected Sub cstmLossTime_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cstmLossTime.ServerValidate
            Dim CONTROL__LOSS, CONTROL__REPORTED As DateTime

            If Not IsValidDates() Then
                Exit Sub
            End If

            If CONTROL__LOSS_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__LOSS_TIME.Text.Trim) = True And CONTROL__REPORTED_TIME.Text.Trim.Length <> 0 And ValidTime(CONTROL__REPORTED_TIME.Text.Trim) = True Then
                CONTROL__LOSS = CONTROL__LOSS_DATE.Text.Trim + " " + CONTROL__LOSS_TIME.Text.Trim
                CONTROL__REPORTED = CONTROL__REPORTED_DATE.Text.Trim + " " + CONTROL__REPORTED_TIME.Text.Trim


                If CDate(CONTROL__LOSS) > CDate(CONTROL__REPORTED) Then
                    cstmLossTime.ErrorMessage = GetLocalResourceObject("lb_LossTime")
                    cstmLossTime.ControlToValidate = "CONTROL__LOSS_TIME"
                    args.IsValid = False
                End If
                'check the Lost Time in future
                If CDate(CONTROL__LOSS) > DateTime.Now Then
                    If CDate(CONTROL__LOSS).TimeOfDay > DateTime.Now.TimeOfDay Then
                        cstmLossTime.ErrorMessage = GetLocalResourceObject("Lost_time_Future_Error")
                    End If
                    args.IsValid = False
                End If
            End If
        End Sub
        Function IsValidDates() As Boolean
            If IsDate(CONTROL__REPORTED_DATE.Text) And IsDate(CONTROL__LOSS_DATE.Text) Then
                cstmInvalidDate.IsValid = True
            Else
                cstmInvalidDate.IsValid = False
            End If
            Return cstmInvalidDate.IsValid
        End Function
    End Class


End Namespace
