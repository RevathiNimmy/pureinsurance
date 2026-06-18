Imports NexusProvider.SAMForInsurance
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library.Portal

Namespace Nexus

    Partial Class Framework_summary
        Inherits CMS.Library.Frontend.clsCMSPage

#Region " Page Events "

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sDisplayReinsurance As String = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaim), NexusProvider.ClaimOpen).RiskType)
            'Get "Display Re-Insurance” option from User Authority”  
            Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
                oWebservice = New NexusProvider.ProviderManager().Provider
                oWebservice.GetUserAuthorityValue(oUserAuthority)

            'Check Display Re-Insurance” option is switched off from User Authority” 
            If oUserAuthority.UserAuthorityValue = "0" Or sDisplayReinsurance = "0" Or oUserAuthority.UserAuthorityValue Is Nothing Then
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PaymentConfirmation",
                                                       "<script language=""JavaScript"" type=""text/javascript"">function PaymentConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_AnotherPayment").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimCloseConfirmation",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function ClaimCloseConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_CloseClaim").ToString() & "'); document.getElementById('" & hidChlClaimClose.ClientID & "').value=r; if (document.getElementById('" & hidChkPaymentMsg.ClientID & "').value ==  '0' && r == false) {PaymentConfirmation();}}</script>")
        End Sub
        ''' <summary>
        ''' This event is fired on Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ucProgressBar.OverviewStyle = "complete"
            ucProgressBar.PerilsStyle = "complete"
            ucProgressBar.ReinsuranceStyle = "incomplete"
            ucProgressBar.SummaryStyle = "in-progress"
            ucProgressBar.CompleteStyle = "incomplete"
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim)

            If CType(Session.Item(CNMode), Mode) = Mode.EditClaim OrElse Session(CNMode) = Mode.PayClaim OrElse Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then
                Dim sRunAuthorizePayment As String
                Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                'Check RI2007 hidde product option
                Dim oRI2007 As NexusProvider.OptionTypeSetting = Nothing
                Dim sDisplayReinsurance As String
                Dim bDisplayReinsuranceConfig As Boolean = True

                oRI2007 = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)

                'Check the risk option to display of Reinsurance

                sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, oClaimDetails.RiskType)

                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                If (Not oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)) Is Nothing) Then
                    bDisplayReinsuranceConfig = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance
                End If

                'Check the User Authority to display of Reinsurance
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
                oWebservice = New NexusProvider.ProviderManager().Provider
                oWebservice.GetUserAuthorityValue(oUserAuthority)

                'get the Product Risk option setting named Run Authorize Payment
                sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceConfig = True Then
                    If String.IsNullOrEmpty(oRI2007.OptionValue) = True Or (String.IsNullOrEmpty(oRI2007.OptionValue) = False AndAlso oRI2007.OptionValue = "0") Then
                        SetMessage()
                    End If
                Else
                    SetMessage()
                End If
            End If
            If Session(CNMode) = Mode.PayClaim Then
                chkFinalPayment.Visible = True
                lblFinalPayment.Visible = True
            Else
                chkFinalPayment.Visible = False
                lblFinalPayment.Visible = False
                If CType(Session.Item(CNMode), Mode) = Mode.EditClaim Then
                    btnBack.Visible = True
                End If
            End If
            If Session(CNMode) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "Login.aspx", False)
            End If

            If CType(Session.Item(CNMode), Mode) = Mode.ViewClaim Then
                btnSubmit.Visible = True
            ElseIf CType(Session.Item(CNMode), Mode) = Mode.NewClaim Then
                btnBack.Visible = True
            ElseIf CType(Session.Item(CNMode), Mode) = Mode.Authorise Then
                btnAuthorise.Visible = True
                btnSubmit.Visible = False
                btnDecline.Visible = True
                btnCancel.Visible = True
            ElseIf CType(Session.Item(CNMode), Mode) = Mode.Recommend Then
                btnRecommend.Visible = True
                btnDecline.Visible = True
                btnSubmit.Visible = False
                btnCancel.Visible = True
            ElseIf CType(Session.Item(CNMode), Mode) = Mode.DeclinePayment Then
                btnDecline.Visible = True
                btnSubmit.Visible = False
                btnCancel.Visible = True
            ElseIf CType(Session.Item(CNMode), Mode) = Mode.ViewClaimPayment Then
                btnFinish.Visible = True
                btnSubmit.Visible = False

            End If

            If Not IsPostBack Then
                PopulateFields()
            End If

        End Sub
#End Region

#Region " Private Methods "
        Sub SetMessage()

            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim nClaimStatus As Integer
            Dim dCurrentReserve As Decimal
            Dim dCurrentRecovery As Decimal
            Dim sOptionSettings As String
            Dim bAllowNegativeReserve As Boolean = False
            Dim bStatus As Boolean = False

            sOptionSettings = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AllowNegativeReserve, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

            If sOptionSettings <> "0" Then
                bAllowNegativeReserve = True
            End If

            oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            oWebservice.CheckReserveRecovery(oClaim.ClaimKey, nClaimStatus, dCurrentReserve, dCurrentRecovery)

            If nClaimStatus <> 3 AndAlso (dCurrentReserve <= 0 AndAlso dCurrentRecovery <= 0) AndAlso (bAllowNegativeReserve = False OrElse (dCurrentReserve = 0 AndAlso dCurrentRecovery = 0)) Then
                bStatus = True
            Else
                bStatus = False
            End If

            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
            oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
            ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow

            If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                If oRunClaimWorkFlow.MakeFurtherPayments = True Then

                    If bStatus = False Then
                        'To make another receipt confirmation 
                        btnSubmit.Attributes.Add("onclick", "javascript:return Confirmation(this,'" + GetLocalResourceObject("msg_AnotherRecovery").ToString() + "');")
                    Else
                        hidChkPaymentMsg.Value = "0"
                        'If this value is set then paymentconfirmatio will be called from
                        'ClamCloseConfirmation javascript function
                        btnSubmit.Attributes.Add("onclick", "return ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                    End If
                ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then

                    If bStatus = True Then
                        hidChkPaymentMsg.Value = String.Empty
                        btnSubmit.Attributes.Add("onclick", "ClaimCloseConfirmation(this,'" + GetLocalResourceObject("msg_CloseClaim").ToString() + "');")
                    End If
                End If

            ElseIf Session(CNMode) = Mode.PayClaim Then

                Dim sRunAuthorizePayment As String

                'get the Product Risk option setting named Run Authorize Payment
                sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                'get the Claim Workflow Setting

                If oRunClaimWorkFlow.MakeFurtherPayments = True Then

                    If bStatus = False Then
                        btnSubmit.Attributes.Add("onclick", "javascript:return PaymentConfirmation();")
                    Else
                        hidChkPaymentMsg.Value = "0"
                        'If this value is set then paymentconfirmatio will be called from
                        'ClamCloseConfirmation javascript function
                        btnSubmit.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                    End If
                ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then

                    If bStatus = True Then
                        hidChkPaymentMsg.Value = String.Empty
                        btnSubmit.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                    End If
                End If

            ElseIf Session(CNMode) = Mode.EditClaim Then
                If bStatus Then
                    hidChkPaymentMsg.Value = String.Empty
                    btnSubmit.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                End If

            End If

        End Sub


        ''' <summary>
        ''' PopulateFields in repeater (outer/inner)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateFields()

            If Session.Item(CNClaim) IsNot Nothing Then

                Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim) 'changed

                ' fill up the session with the nice reserve descriptions 
                If Session(CNReserveDescriptions) Is Nothing Then
                    GetReserves(oClaimDetails.RiskKey)
                End If

                lblRTypeValue.Text = oClaimDetails.RiskTypeDescription
                lblLDateValue.Text = oClaimDetails.LossFromDate.ToShortDateString()
                lblRDateValue.Text = oClaimDetails.ReportedDate.ToShortDateString()
                lblBDescValue.Text = oClaimDetails.Description

                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oPrimaryCauses As NexusProvider.PrimaryCausesCollections = oWebservice.GetValidPrimaryCauses(v_iInsuranceFileKey:=Session(CNInsuranceFileKey), v_sBranchCode:=sBranchCode)

                If oPrimaryCauses IsNot Nothing Then
                    For iCount As Integer = 0 To oPrimaryCauses.Count - 1
                        If oClaimDetails.PrimaryCauseCode IsNot Nothing AndAlso oClaimDetails.PrimaryCauseCode.Trim.ToUpper = oPrimaryCauses(iCount).Code.Trim.ToUpper Then
                            lblCTypeValue.Text = oPrimaryCauses(iCount).Description.Trim
                        End If
                    Next
                End If

                lblAddInfoValue.Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, oClaimDetails.SecondaryCauseCode, "secondary_cause")

                dlOuter.DataSource = oClaimDetails.ClaimPeril
                dlOuter.DataBind()

            End If

        End Sub
        ''' <summary>
        ''' dlOuter_ItemDataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dlOuter_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles dlOuter.ItemDataBound
            If ((e.Item.ItemType = ListItemType.Header)) Then
                Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim) 'changed
                Dim oTabcontainer As HtmlGenericControl = CType(e.Item.FindControl("tab_container"), HtmlGenericControl)
                Dim lbltabContent As Literal = CType(e.Item.FindControl("lbltabContent"), Literal)
                Dim stabContent As String = Nothing
                For iCount As Integer = 0 To oClaimDetails.ClaimPeril.Count - 1
                    If iCount >= 9 Then
                        stabContent += "<li><a href='#ctl00_cntMainBody_dlOuter_ctl" & iCount + 1 & "_tab_claimsummary' data-bs-toggle='tab' aria-expanded='true'>" & oClaimDetails.ClaimPeril(iCount).Description.Trim & "</a></li>"
                    Else
                        stabContent += "<li><a href='#ctl00_cntMainBody_dlOuter_ctl0" & iCount + 1 & "_tab_claimsummary' data-bs-toggle='tab' aria-expanded='true'>" & oClaimDetails.ClaimPeril(iCount).Description.Trim & "</a></li>"
                    End If
                Next
                lbltabContent.Text = stabContent

            ElseIf ((e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem)) Then

                Dim oReserveInitialValues As New Hashtable
                If CType(e.Item.DataItem, NexusProvider.PerilSummary).Reserve IsNot Nothing Then
                    For Each oReserve As NexusProvider.Reserve In CType(e.Item.DataItem, NexusProvider.PerilSummary).Reserve
                        Select Case CType(Session.Item(CNMode), Mode)
                            Case Mode.NewClaim
                                If oReserve.TypeCode IsNot Nothing Then
                                    oReserveInitialValues.Add(oReserve.TypeCode, oReserve.InitialReserve)
                                End If
                            Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend
                                oReserveInitialValues.Add(oReserve.TypeCode, oReserve.InitialReserve + oReserve.RevisedReserve)
                        End Select
                    Next
                    Session.Item(CNReserveInitialValues) = oReserveInitialValues
                End If

                ' bind nested list
                Dim dl2 As Repeater = CType(e.Item.FindControl("dlInner"), Repeater)
                If Not dl2 Is Nothing Then
                    Dim oReserve2 As NexusProvider.ReserveCollection = CType(e.Item.DataItem, NexusProvider.PerilSummary).Reserve
                    dl2.DataSource = oReserve2
                    dl2.DataBind()
                End If
                ' calculate reserve totals
                Dim dTotal As Decimal = 0
                Dim dTotalPaid As Decimal = 0

                Try

                    Select Case CType(Session.Item(CNMode), Mode)
                        Case Mode.NewClaim
                            For Each oReserve As NexusProvider.Reserve _
                            In CType(e.Item.DataItem, NexusProvider.PerilSummary).Reserve
                                dTotal += oReserve.InitialReserve
                            Next
                        Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend
                            For Each oReserve As NexusProvider.Reserve _
                            In CType(e.Item.DataItem, NexusProvider.PerilSummary).Reserve

                                If oReserve.BaseReserveKey <> 0 Then
                                    If CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Or CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                                        dTotal += oReserve.InitialReserve + oReserve.RevisedReserve
                                    Else
                                        Dim currentReserve As Decimal = oReserve.InitialReserve + oReserve.RevisedReserve
                                        If currentReserve >= 0 OrElse (oReserve.TypeCode.ToUpper <> "SALVAGE" AndAlso oReserve.TypeCode.ToUpper <> "THIRDPARTY") Then
                                            dTotal += currentReserve
                                        End If
                                    End If
                                End If

                            Next
                            If CType(e.Item.DataItem, NexusProvider.PerilSummary) IsNot Nothing AndAlso CType(e.Item.DataItem, NexusProvider.PerilSummary).ClaimReserve IsNot Nothing Then
                                If CType(e.Item.DataItem, NexusProvider.PerilSummary).ClaimReserve.Count = 0 Then
                                    For Each oClaimReserve As NexusProvider.Reserve In CType(e.Item.DataItem, NexusProvider.PerilSummary).Reserve
                                        dTotalPaid += oClaimReserve.PaidAmount
                                    Next

                                Else
                                    For Each oClaimReserve As NexusProvider.ClaimPerilReservePaymentType In CType(e.Item.DataItem, NexusProvider.PerilSummary).ClaimReserve
                                        If CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                                            dTotalPaid += oClaimReserve.PaidToDate + oClaimReserve.CostToClaim
                                        Else
                                            dTotalPaid += oClaimReserve.PaidToDate
                                        End If
                                    Next
                                End If
                            End If
                    End Select
                Catch
                End Try

                If e.Item.FindControl("lblReserveTotal") IsNot Nothing Then
                    CType(e.Item.FindControl("lblReserveTotal"), Label).Text = New Money(dTotal, New Currency(Session.Item(CNCurrenyCode).ToString()).Type).Formatted.ToString()
                End If
                'SN     PN41611
                If e.Item.FindControl("lblSumInsured") IsNot Nothing Then
                    CType(e.Item.FindControl("lblSumInsured"), Label).Text = New Money(e.Item.DataItem.SumInsured, New Currency(Session.Item(CNCurrenyCode).ToString()).Type).Formatted.ToString()
                End If

                If e.Item.FindControl("lblPerilDescription") IsNot Nothing Then
                    CType(e.Item.FindControl("lblPerilDescription"), Label).Text = CType(e.Item.DataItem, NexusProvider.PerilSummary).Description.ToString
                End If

                If e.Item.FindControl("lblPaid") IsNot Nothing Then
                    CType(e.Item.FindControl("lblPaid"), Label).Text = New Money(dTotalPaid, New Currency(Session.Item(CNCurrenyCode).ToString()).Type).Formatted.ToString()
                End If
            End If
        End Sub

        ''' <summary>
        ''' dlInner_ItemDataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dlInner_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)

            ' retrieves the nice descriptions for the reserve items from the hashtable
            If ((e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem)) Then
                If CType(e.Item.DataItem, NexusProvider.Reserve).BaseReserveKey <> 0 Then
                    If e.Item.FindControl("lblReserveItem") IsNot Nothing Then
                        'SN     PN41611     TypeCode is same for both the tables 
                        Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString

                        CType(e.Item.FindControl("lblReserveItem"), Label).Text = CType(Session.Item(CNReserveDescriptions), Hashtable).Item(CType(e.Item.DataItem, NexusProvider.Reserve).TypeCode)

                        'line below amended to remove currency symbol - Etana 3.1
                        CType(e.Item.FindControl("lblReservePrice"), Label).Text = String.Format(oFormatString, CType(Session.Item(CNReserveInitialValues), Hashtable).Item(CType(e.Item.DataItem, NexusProvider.Reserve).TypeCode))
                    End If
                Else
                    e.Item.Visible = False
                End If
            End If
            If (e.Item.ItemType = ListItemType.Header) Then

                'Etana 3.1 Grid/Currency formatting
                CType(e.Item.FindControl("ltlReserveItem"), Literal).Text = Replace(CType(e.Item.FindControl("ltlReserveItem"), Literal).Text, "[!Currency!]", TransactionCurrency.Symbol)
                CType(e.Item.FindControl("ltlReservePrice"), Literal).Text = Replace(CType(e.Item.FindControl("ltlReservePrice"), Literal).Text, "[!Currency!]", TransactionCurrency.Symbol)

            End If

        End Sub
#End Region

#Region " Button Events "
        ''' <summary>
        ''' BtnSubmit_Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            Dim oClaimResponse As NexusProvider.ClaimResponse
            Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim)
            Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
            Dim oclaimRisk As New NexusProvider.ClaimRisk
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
            'IF display reinsurance is ON 
            'IF display reinsurance is ON 
            Dim sDisplayReinsurance As String
            Dim bDisplayReinsuranceConfig As Boolean = True
            Dim oreturncode() As Byte

            'Check RI2007 hidden product option
            Dim oRI2007 As NexusProvider.OptionTypeSetting = Nothing
            oRI2007 = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 88)

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim bClaimBuilder As Boolean = False
            Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
            If bClaimBuilder Then
                ' get latest TimeStamp
                oclaimRisk = GetClaimRiskCall(oClaimDetails.BaseClaimKey, oClaimDetails.ClaimKey, sBranchCode)
            End If

            'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey
            oclaimRisk.ClaimKey = oClaimDetails.BaseClaimKey
            oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
            oclaimRisk.XMLDataSet = Session.Item(CNDataSet)

            sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, oClaimDetails.RiskType)
            
            If (Not oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)) Is Nothing) Then
                bDisplayReinsuranceConfig = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance
            End If


            'Check the User Authority to display of Reinsurance
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
            oWebservice = New NexusProvider.ProviderManager().Provider
            oWebservice.GetUserAuthorityValue(oUserAuthority)

            Dim oExclusiveLocking As NexusProvider.OptionTypeSetting
            oExclusiveLocking = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock)

            If Session(CNMode) = Mode.NewClaim Then

                Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                If bClaimBuilder = True Then
                    'Update the claim builder risk screen
                    If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                        Exit Sub
                    End If
                    Session(CNClaimCallsTimeStamp) = oclaimRisk.TimeStamp
                    bClaimTimeStamp = oclaimRisk.TimeStamp
                End If

                If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceConfig = True Then
                    Response.Redirect("~/claims/ClaimReinsurance.aspx", True)
                Else
                    'Fire the Bind Claim
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                    End If

                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode)
                End If

            ElseIf Session(CNMode) = Mode.EditClaim Then

                Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                If bClaimBuilder = True Then
                    'Update the claim builder risk screen
                    ' oreturncode = oWebservice.UpdateClaimRisk(oclaimRisk, sBranchCode)
                    If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                        Exit Sub
                    End If
                    Session(CNClaimCallsTimeStamp) = oclaimRisk.TimeStamp
                    bClaimTimeStamp = oclaimRisk.TimeStamp
                End If

                If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceConfig = True Then
                    Response.Redirect("~/claims/ClaimReinsurance.aspx", True)
                Else
                    'Fire the Bind Claim
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                    End If

                    BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode)
                End If

            ElseIf Session(CNMode) = Mode.PayClaim Then
                'Fire the Bind Claim
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                Dim oClaimPerilPayment As New NexusProvider.ClaimPaymentCollection
                Dim oPayment As New NexusProvider.ClaimPayment
                If Request("PerilIndex") IsNot Nothing Then
                    Session(CNClaimPerilIndex) = Request("PerilIndex")
                End If
                If Session(CNClaimPerilIndex) IsNot Nothing Then
                    oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                    Dim dPayment As Decimal
                    For lCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count - 1
                        dPayment = dPayment + CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(lCount).Payment.PaymentAmount
                        lCount = lCount + 1
                    Next
                    oPayment.PaymentAmount = dPayment

                    CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(CInt(Session(CNClaimPerilIndex))).Payment.PaymentAmount = dPayment
                    'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                        oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                    Else
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                        oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                    End If
                    If chkFinalPayment.Checked = True Then
                        oOriginalClaim.CloseClaimOnFinalPayment = True
                        oPayment.CloseClaimOnFinalPayment = True
                    Else
                        oOriginalClaim.CloseClaimOnFinalPayment = False
                        oPayment.CloseClaimOnFinalPayment = False
                    End If
                    If (oPayment.ClaimVersionDescription Is Nothing Or oPayment.ClaimVersionDescription = "") AndAlso Session(CNChangeReason) IsNot Nothing Then
                        oPayment.ClaimVersionDescription = Session(CNChangeReason)
                    End If
                ElseIf Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                    For Each PerilItemIndex As Integer In PerilsIndex
                        oPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment
                        Dim dPayment As Decimal
                        For lCount As Integer = 0 To CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril.Count - 1
                            dPayment = dPayment + CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(lCount).Payment.PaymentAmount
                            lCount = lCount + 1
                        Next
                        oPayment.PaymentAmount = dPayment
                        CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment.PaymentAmount = dPayment
                        oClaimPerilPayment.Add(oPayment)
                        'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                        If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                            oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                            oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                        Else
                            oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                            oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                        End If

                        If (oPayment.ClaimVersionDescription Is Nothing Or oPayment.ClaimVersionDescription = "") AndAlso Session(CNChangeReason) IsNot Nothing Then
                            oPayment.ClaimVersionDescription = Session(CNChangeReason)
                        End If
                    Next
                End If

                Dim bPaymentAuthorized As Boolean = False

                Try
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                            Exit Sub
                        End If
                        Session(CNClaimCallsTimeStamp) = oclaimRisk.TimeStamp
                        bClaimTimeStamp = oclaimRisk.TimeStamp
                    End If

                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceConfig = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx")
                    Else

                        If Session(CNMode) = Mode.PayClaim AndAlso ((Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) OrElse chkFinalPayment.Checked = True) Then
                            'Fire the Bind Claim
                            If Session(CNClaimPerilIndex) IsNot Nothing Then
                                BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode, bPaymentAuthorized)
                            ElseIf Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                                BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, Nothing, sBranchCode, bPaymentAuthorized, oPaymentCollection:=oClaimPerilPayment)
                            End If
                        Else
                            'Fire the Bind Claim
                            If Session(CNClaimPerilIndex) IsNot Nothing Then
                                BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode, bPaymentAuthorized)
                            ElseIf Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                                BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, Nothing, sBranchCode, bPaymentAuthorized, oPaymentCollection:=oClaimPerilPayment)
                            End If

                        End If
                        If chkFinalPayment.Checked = True Then
                            GetClaimDetails(oClaimDetails.ClaimKey, oclaimRisk)
                            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                            oOpenClaim.ProgressStatusCode = "CLOSED"
                            Dim iTimestamp As Byte() = CType(Session.Item(CNClaimTimeStamp), Byte())
                            MaintainClaimCall(oOpenClaim, iTimestamp, sBranchCode)
                        End If

                        'Check to move to the accept another payment
                        oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")

                        If oRunClaimWorkFlow.MakeFurtherPayments = True And hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                            GetLatestDetails() 'Update the session with latest values
                            Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                            Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                            'Dummy Cal of PayClaim, to retreive back the latetst claim key
                            PayClaimWithZeroAmount()

                            If oExclusiveLocking.OptionValue = "1" Then
                                Dim oClaimDetails1 As NexusProvider.ClaimDetails = Nothing
                                oClaimDetails1 = oWebservice.GetClaimDetails(v_iClaimKey:=oOriginalClaim.ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=True)
                            End If

                            Dim sUrl As String = CheckClaimBuilder()
                            Response.Redirect(sUrl, False)
                            'Update the Claims session variable
                            'GetClaimDetails(oClaimDetails.ClaimKey, oclaimRisk)
                            Exit Sub
                        Else
                            If bPaymentAuthorized Then
                                Session(CNAuthorizeStatus) = ""
                            End If
                            Response.Redirect("~/Claims/Complete.aspx", False)
                        End If
                    End If

                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "331" Then   'DebtorUserGroupsAreNotSetup

                            Dim cstDebtorUserGroups As New CustomValidator
                            cstDebtorUserGroups.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                            cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstDebtorUserGroups)
                            Exit Sub
                        Else
                            Throw ex
                        End If
                    End Try
                ElseIf Session(CNMode) = Mode.SalvageClaim OrElse Session(CNMode) = Mode.TPRecovery Then

                    'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                    oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                Else
                    oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False

                End If

                Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                If bClaimBuilder = True Then
                    'Update the claim builder risk screen
                    oreturncode = oWebservice.UpdateClaimRisk(oclaimRisk, sBranchCode)
                    If oreturncode Is Nothing Then
                        Exit Sub
                    End If
                End If

                oWebservice.BindClaim(oOriginalClaim, bClaimTimeStamp, 5, Nothing, sBranchCode)

                'Check to move to the accept another payment
                oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")

                If oRunClaimWorkFlow.MakeFurtherPayments = True And hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                    GetLatestDetails() 'Update the session with latest values
                    Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                    Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                    If oExclusiveLocking.OptionValue = "1" Then
                        Dim oClaimDetails1 As NexusProvider.ClaimDetails = Nothing
                        oClaimDetails1 = oWebservice.GetClaimDetails(v_iClaimKey:=oOriginalClaim.ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=True)
                    End If
                    Dim sUrl As String = CheckClaimBuilder()
                    Response.Redirect(sUrl)
                ElseIf sDisplayReinsurance = "0" Then
                    Response.Redirect("~/Claims/Complete.aspx")
                End If
            End If

            If Session(CNMode) = Mode.ViewClaim Then
                If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso bDisplayReinsuranceConfig = True Then
                    If oRI2007.OptionValue = 1 Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx")
                    Else
                        Dim sClaimVersionDetail As String = IIf(oClaimDetails.ClaimVersionDescription Is Nothing, "", oClaimDetails.ClaimVersionDescription)
                        If Not (sClaimVersionDetail.IndexOf("Salvage Recovery") = 0 Or sClaimVersionDetail.IndexOf("Third Party Recovery") = 0) Then
                            Response.Redirect("~/claims/ClaimReinsurance.aspx")
                        End If
                    End If
                End If
            End If

            'Update the Claims session variable
            GetClaimDetails(oClaimDetails.ClaimKey, oclaimRisk)
            Response.Redirect("~/claims/complete.aspx", False)

        End Sub
        ''' <summary>
        ''' btnBack_Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            If Request.QueryString("ReturnUrl") IsNot Nothing Then
                Response.Redirect(Request.QueryString("ReturnUrl"), False)
            Else
                Response.Redirect("~/claims/Perils.aspx", False)
            End If
        End Sub
        ''' <summary>
        ''' Finish Button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinish.Click
            Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", False)
        End Sub
        ''' <summary>
        ''' Authorise Button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnAuthorise_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorise.Click
            'call AuthoriseClaimPayment
            Dim sPolicyNumber As String
            Dim nClaimKey As Integer
            Dim dtPaymentDate As Date
            Dim nClaimPaymentKey As Integer
            Dim sClaimNumber As String
            Dim sCreatedBy As String
            Dim sAuthoriseReason As String = "Claim Authorised"
            Dim dPaymentAmount As Double

            nClaimPaymentKey = CType(Session(CNClaimPaymentKey), Integer)
            sCreatedBy = CType(Session(CNClaimPaymentCreatedBy), String)

            Dim oClaimDetails As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)

            Try
                sPolicyNumber = oClaimDetails.PolicyNumber
                nClaimKey = oClaimDetails.ClaimKey
                sClaimNumber = oClaimDetails.ClaimNumber
                For Each oClaimPeril As NexusProvider.PerilSummary In oClaimDetails.ClaimPeril
                    If oClaimPeril.Payment IsNot Nothing Then
                        For lCount = 0 To oClaimPeril.ClaimPayment.Count - 1
                            If nClaimPaymentKey = oClaimPeril.ClaimPayment(lCount).BaseClaimPaymentKey Then
                                dtPaymentDate = oClaimPeril.ClaimPayment(lCount).PaymentDate
                                dPaymentAmount = oClaimPeril.ClaimPayment(lCount).ThisPaymentINCLTax '
                                Exit For
                            End If
                        Next

                    End If
                Next

                'redirect to ClaimPaymentDoc.aspx
                Response.Redirect("~/secure/ClaimPaymentDoc.aspx?FromPage=ACP&ClaimPaymentKey=" & nClaimPaymentKey & "&ClaimNumber=" & sClaimNumber & "&PaymentDate=" & dtPaymentDate & "&PaymentAmount=" & dPaymentAmount & "&ProductCode=" & CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode & "&FunctionalArea=4", False)

            Catch ex As NexusProvider.NexusException
                Throw ex
            Finally
                oClaimDetails = Nothing
            End Try

        End Sub
        ''' <summary>
        ''' Recommend Button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnRecommend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRecommend.Click
            Dim nClaimKey As Integer
            Dim sProductCode As String
            Dim sFailureReason As String
            Dim sMessage As String = ""
            Dim oClaim As NexusProvider.Claim = CType(Session(CNClaim), NexusProvider.Claim)
            Dim bTimeStamp() As Byte
            Try

                If oClaim IsNot Nothing Then
                    bTimeStamp = CType(Session(CNClaimTimeStamp), Byte())
                Else
                    sMessage = GetLocalResourceObject("msg_RecordModified").ToString()
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DeclinePayment", _
                                                                                           "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){DeclinePayment('" & sMessage & "');});</script>", False)
                    Exit Sub
                End If


                If Session.Item(CNClaim) IsNot Nothing Then
                    Dim oClaimDetails As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                    nClaimKey = oClaimDetails.ClaimKey
                    oClaimDetails = Nothing
                End If
                If Session.Item(CNClaimQuote) IsNot Nothing Then
                    'Dim oQuote As NexusProvider.Quote = CType(Session.Item(CNClaimQuote), NexusProvider.Quote)
                    sProductCode = CType(Session.Item(CNClaimQuote), NexusProvider.Quote).ProductCode
                    'oQuote = Nothing
                End If
                ''call RecommendClaimPayment
                RecommendClaimPayment(nClaimKey, sProductCode, sFailureReason, bTimeStamp)

                If String.IsNullOrEmpty(sFailureReason) Then
                    'redirect to AuthoriseClaimPayments.aspx
                    Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", False)
                Else
                    If sFailureReason = "200" Then
                        sMessage = GetLocalResourceObject("msg_ClaimLocked").ToString()
                    ElseIf sFailureReason = "206" Then
                        sMessage = GetLocalResourceObject("msg_RecordModified").ToString()
                    ElseIf sFailureReason = "331" Then
                        sMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                    Else
                        'Claim Payment Declined 
                        DeclineClaimPayment(CType(Session(CNClaimPaymentKey), Integer), CType(GetLocalResourceObject("lblDeclinedComments"), String))

                        If sFailureReason = "Bank" Then
                            sMessage = GetLocalResourceObject("msg_BankDefault").ToString()
                        ElseIf sFailureReason = "Mandatory" OrElse sFailureReason = "100" Then
                            sMessage = GetLocalResourceObject("msg_MandatoryFields").ToString()
                        End If
                    End If
                    'Give Error Messages
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DeclinePayment", _
                                                                          "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){DeclinePayment('" & sMessage & "');});</script>", False)
                End If
            Catch ex As NexusProvider.NexusException
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "200" 'Claim Locking
                        'Show Claim locking error as alert
                        Dim sMessage1 As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "policylocked", sMessage1, True)
                        Server.ClearError()
                        ClearQuote()
                        ClearClaims()
                        Exit Sub
                    Case Else

                        Throw ex
                End Select
            Finally
                oClaim = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Decline Button Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnDecline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDecline.Click
            Dim nClaimPaymentKey As Integer
            Dim sDeclineReason As String = "Claim Declined"
            Dim sProductCode As String
            Dim dPaymentAmount As Double = 0
            nClaimPaymentKey = CType(Session(CNClaimPaymentKey), Integer)
            If Session.Item(CNClaimQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = CType(Session.Item(CNClaimQuote), NexusProvider.Quote)
                sProductCode = oQuote.ProductCode
                oQuote = Nothing
            End If
            Session(CNMode) = Mode.DeclinePayment
            'redirect to ClaimPaymentDoc.aspx
            dPaymentAmount = Session(CNAmountToPay)
            Response.Redirect("~/secure/ClaimPaymentDoc.aspx?FromPage=ACP&ClaimPaymentKey=" & nClaimPaymentKey & "&ProductCode=" & sProductCode & "&FunctionalArea=4&PaymentAmount=" & dPaymentAmount, False)

        End Sub
        ''' <summary>
        ''' Cancel Button Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", False)
        End Sub
        ''' <summary>
        ''' GetClaimDetails
        ''' </summary>
        ''' <param name="v_iClaimKey"></param>
        ''' <param name="oClaimRisk"></param>
        ''' <remarks></remarks>
        Sub GetClaimDetails(ByVal v_iClaimKey As Integer, ByVal oClaimRisk As NexusProvider.ClaimRisk)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = CType(Session(CNClaimQuote), NexusProvider.Quote)
            Dim sBranchCode As String = oQuote.BranchCode
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey, sBranchCode)
            'updation of latest session values 
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
            Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber
            Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
            With oClaimDetails
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseCaseKey = .BaseCaseKey
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
        Sub GetLatestDetails()
            'Latest Claim Details
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimVersions As NexusProvider.VersionsCollections
            Dim iHighest As Integer = 0
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            oClaimVersions = oWebservice.GetVersionsForClaim(Session(CNClaimNumber), sBranchCode)

            'Clear the session Variable
            ClearClaims()

            If oClaimVersions IsNot Nothing Then
                'Find Highest Version
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version > iHighest Then
                        iHighest = oClaimVersions(iCount).Version
                    End If
                Next
            End If
            For iCount As Integer = 0 To oClaimVersions.Count - 1
                If oClaimVersions(iCount).Version = iHighest Then

                    'arch issue 268
                    oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)

                    With oClaimDetails
                        oOpenClaim.CatastropheCode = .CatastropheCode
                        oOpenClaim.BaseCaseKey = .BaseCaseKey
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
                        'Added for Insurer
                        oOpenClaim.Insurer = .Insurer
                        Session.Item(CNClaimTimeStamp) = .TimeStamp
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                        oOpenClaim.Client = .Client
                        'Session(CNInsurer_Header) = .ClientName
                        Session(CNClaimNumber) = .ClaimNumber
                        Session(CNStatus) = .ClaimStatus

                        If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                            'Arch issue 268
                            oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                            If oClaimRisk IsNot Nothing AndAlso oClaimRisk.XMLDataSet IsNot Nothing Then
                                Session(CNDataSet) = oClaimRisk.XMLDataSet
                            End If
                        End If
                        If Session(CNMode) = Mode.PayClaim Then
                            For iPeril As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                If oOpenClaim.ClaimPeril(iPeril).Reserve.Count > 0 Then
                                    Session(CNClaimPerilIndex) = iPeril
                                    Exit For
                                End If
                            Next
                        End If
                    End With
                End If
            Next
            Session(CNClaim) = oOpenClaim
        End Sub
#End Region

    End Class
End Namespace
