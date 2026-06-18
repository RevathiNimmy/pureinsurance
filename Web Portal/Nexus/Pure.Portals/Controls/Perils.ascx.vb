Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Xml.XmlReader
Imports System.Xml.XPath
Imports System.Xml
Imports CMS.Library

Namespace Nexus
    Partial Class Controls_Perils
        Inherits System.Web.UI.UserControl
        Dim bAllowMultipleClaimPayment As Boolean = True
        Dim sIsGrossClaimPaymentAmount As String
        Public m_sIsPaymentsReadOnly As String

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PaymentConfirmation",
                                                      "<script language=""JavaScript"" type=""text/javascript"">function PaymentConfirmation(){ var r= confirm('" & GetLocalResourceObject("msg_AnotherPayment").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimCloseConfirmation",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function ClaimCloseConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_CloseClaim").ToString() & "'); document.getElementById('" & hidChlClaimClose.ClientID & "').value=r; if (document.getElementById('" & hidChkPaymentMsg.ClientID & "').value ==  '0' && r == false) {PaymentConfirmation();}}</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MultipleClaimPaymentErrorMsg",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function MultipleClaimPaymentErrorMsg(){alert('" & GetLocalResourceObject("msg_AllowMultipleClaimPayment_error").ToString & "');}</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MultipleClaimPaymentErrorMsgForValue",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function MultipleClaimPaymentErrorMsgForValue(){alert('" & GetLocalResourceObject("msg_AllowMultipleClaimPayment_ErrorForValue").ToString & "');}</script>")

        End Sub
        Public WriteOnly Property ReBindGrid() As Boolean
            Set(ByVal value As Boolean)
                If value = True Then
                    BindPerils()
                End If
            End Set
        End Property


#Region " Private Methods "
        ''' <summary>
        ''' This is used to Bind Data to the Grid. ViewState("EnablePayClaim") holds the value whether Pay Claim 
        ''' should be enable or not
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindPerils()
            If Session.Item(CNClaim) IsNot Nothing Then
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim iIndex As Integer
                Dim dPaidAmount As Double = 0.0
                Dim dTaxPaidAmount As Double = 0.0
                Dim sIsGrossClaimPaymentAmount As String
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                iIndex = 0
                ViewState("dPaidAmount") = Nothing
                GetReserves(oClaim.RiskKey)
                Dim sOption As String
                sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If String.IsNullOrEmpty(sOption) Then
                    sIsGrossClaimPaymentAmount = "0"
                Else
                    sIsGrossClaimPaymentAmount = sOption
                End If

                Dim sClaimsIsPostTaxes As String

                sOption = String.Empty
                sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, Nothing, NexusProvider.RiskTypeOptions.ClaimsIsPostTaxes, Nothing, oClaim.RiskType)
                If String.IsNullOrEmpty(sOption) Then
                    sClaimsIsPostTaxes = "0"
                Else
                    sClaimsIsPostTaxes = sOption
                End If

                Dim hPaymentAmount As New Hashtable
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)

                If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                    PerilsIndex = Session(CNClaimMultiPerilIndex)
                ElseIf Session(CNClaimPerilIndex) IsNot Nothing Then
                    PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                End If

                If m_sIsPaymentsReadOnly = "1" Then
                    ViewState("EnablePayClaim") = Not CType(Session(CNEnablePayClaim), Boolean)
                    For iClaimIndex As Integer = 0 To oClaim.ClaimPeril.Count - 1
                        oClaim.ClaimPeril(iClaimIndex).TotalPaidAmount = oClaim.ClaimPeril(iClaimIndex).PaidAmount
                    Next
                Else
                    Dim PerilIndex As Integer = 0
                    For Each oClaimPeril As NexusProvider.PerilSummary In oClaim.ClaimPeril
                        If oClaimPeril.Payment IsNot Nothing Then
                            If oClaimPeril.Payment.ClaimPaymentItem.Count > 0 Then
                                dPaidAmount = 0
                                For i As Integer = 0 To oClaimPeril.Payment.ClaimPaymentItem.Count - 1

                                    dPaidAmount = dPaidAmount + (oClaimPeril.Payment.ClaimPaymentItem(i).PaymentAmount * oClaimPeril.Payment.ClaimPaymentItem(i).CurrencyRate)
                                    dTaxPaidAmount = dTaxPaidAmount + (oClaimPeril.Payment.ClaimPaymentItem(i).TaxAmount * oClaimPeril.Payment.ClaimPaymentItem(i).CurrencyRate)

                                    If i = Convert.ToInt32(Session(CNClaimPerilIndex)) Then
                                        oClaim.ClaimPeril.Item(i).TotalPaidAmount = oClaim.ClaimPeril(i).PaidAmount + dPaidAmount
                                    Else
                                        If i < oClaim.ClaimPeril.Count Then
                                            oClaim.ClaimPeril.Item(i).TotalPaidAmount = oClaim.ClaimPeril(i).PaidAmount
                                        End If
                                    End If
                                    oClaim.ClaimPeril.Item(PerilIndex).TotalPaidAmount = oClaim.ClaimPeril(PerilIndex).PaidAmount + dPaidAmount
                                Next
                                If Not hPaymentAmount.ContainsKey(PerilIndex) Then
                                    hPaymentAmount.Add(PerilIndex, dPaidAmount)
                                End If
                                iIndex = iIndex + 1
                                ViewState("EnablePayClaim") = True
                            Else
                                ViewState("EnablePayClaim") = False
                            End If
                        End If
                        PerilIndex = PerilIndex + 1
                    Next
                End If
                Dim claimsReserveForGross As NexusProvider.OptionTypeSetting
                claimsReserveForGross = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ClaimsReserveForGross)

                If claimsReserveForGross.OptionValue = "1" Then
                    ViewState("dPaidAmount") = dPaidAmount - dTaxPaidAmount
                ElseIf dTaxPaidAmount <> 0 Then
                    ViewState("dPaidAmount") = dPaidAmount - dTaxPaidAmount
                Else
                    ViewState("dPaidAmount") = dPaidAmount
                End If

                grdvPerils.DataSource = oClaim.ClaimPeril
                grdvPerils.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' This is used to check salvage and third party recovery options at product level. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CheckRecoveriesAtProductLevel(ByVal ProcessType As NexusProvider.ClaimProcessType)
            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            'get the Claim Workflow Setting
            oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(ProcessType, oQuote.ProductCode)
            ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow
            'check the salvage option and set the value in session
            If oRunClaimWorkFlow.SalvageRecovery = True Then
                Session(CNCheckSalvageRecovery) = True
            Else
                Session(CNCheckSalvageRecovery) = False
            End If
            'check the third party option and set the value in session
            If oRunClaimWorkFlow.ThirdPartyRecovery = True Then
                Session(CNCheckTPRecovery) = True
            Else
                Session(CNCheckTPRecovery) = False
            End If
        End Sub
#End Region

#Region " Grid View Events "

        ''' <summary>
        ''' This event is fired on Row Command of Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvPerils_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvPerils.RowCommand
            If e.CommandName = "SalvageClaim" Or e.CommandName = "TPRecovery" Then
                Session(CNClaimPerilIndex) = CInt(e.CommandArgument)

                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                If Not oClaim Is Nothing AndAlso Not oClaim.ClaimPeril Is Nothing AndAlso oClaim.ClaimPeril(e.CommandArgument).ClaimPerilKey > 0 Then
                    Response.Redirect("~/claims/PerilDetails.aspx?PerilIndex=" & e.CommandArgument & "&PerilID=" & oClaim.ClaimPeril(e.CommandArgument).ClaimPerilKey, False)
                Else
                    Response.Redirect("~/claims/PerilDetails.aspx?PerilIndex=" & e.CommandArgument, False)
                End If

            ElseIf e.CommandName = "PayClaim" Then
                Session(CNClaimPerilIndex) = CInt(e.CommandArgument)
            End If
        End Sub

        ''' <summary>
        ''' This event is fired on Row Data Bound of Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvPerils_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvPerils.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                    PerilsIndex = Session(CNClaimMultiPerilIndex)
                Else
                    PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
                End If

                Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                m_sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimKey)

                Dim dTotal As Decimal = 0
                Dim dTotalPaid As Decimal = 0
                Dim dRecoveryTotal As Decimal = 0
                Dim dSalvageTotal As Decimal = 0
                Dim oReserveList As NexusProvider.ReserveCollection = CType(e.Row.DataItem, NexusProvider.PerilSummary).Reserve
                Dim oTPRecoveryList As NexusProvider.PerilRecoveryCollection = CType(e.Row.DataItem, NexusProvider.PerilSummary).TPRecovery
                Dim oSalvageList As NexusProvider.PerilRecoveryCollection = CType(e.Row.DataItem, NexusProvider.PerilSummary).SalvageRecovery
                'calculation of the Reserve Total
                If oReserveList IsNot Nothing Then
                    Select Case CType(Session.Item(CNMode), Mode)
                        Case Mode.NewClaim
                            For Each oReserve As NexusProvider.Reserve In oReserveList
                                If oReserve.BaseReserveKey <> 0 Then
                                    dTotal += oReserve.InitialReserve
                                End If
                            Next
                        Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend, Mode.ViewClaimPayment
                            If Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.PayClaim _
                            Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                            Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment _
                            Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                For Each oReserve As NexusProvider.Reserve In oReserveList
                                    If oReserve.BaseReserveKey <> 0 Then
                                        Dim currentReserve As String = oReserve.InitialReserve + oReserve.RevisedReserve
                                        dTotal += currentReserve
                                    End If
                                Next
                                'ElseIf Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                '    For Each oReserve As NexusProvider.Reserve In oReserveList
                                '        If oReserve.BaseReserveKey = 0 Then
                                '            Dim currentReserve As String = oReserve.InitialReserve + oReserve.RevisedReserve
                                '            dTotal += currentReserve
                                '        End If
                                'Next
                            End If
                    End Select
                End If

                'calculation of the TPRecovery Total
                If oTPRecoveryList IsNot Nothing Then
                    Select Case CType(Session.Item(CNMode), Mode)
                        Case Mode.NewClaim
                            For Each oRecovery As NexusProvider.PerilRecovery In oTPRecoveryList
                                If oRecovery.BaseRecoveryKey <> 0 Then
                                    dRecoveryTotal += oRecovery.ThisReceiptINCLTax + oRecovery.ReceiptedAmount
                                End If
                            Next
                        Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend, Mode.ViewClaimPayment
                            If Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.PayClaim _
                            Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                            Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment _
                            Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                For Each oRecovery As NexusProvider.PerilRecovery In oTPRecoveryList
                                    If oRecovery.BaseRecoveryKey <> 0 Then
                                        Dim currentRecovery As String = oRecovery.ReceiptedAmount
                                        dRecoveryTotal += oRecovery.ThisReceiptINCLTax + currentRecovery
                                    End If
                                Next
                            End If
                    End Select
                End If

                'calculation of the Salvage Total
                If oSalvageList IsNot Nothing Then
                    Dim claimsReserveForGross As NexusProvider.OptionTypeSetting
                    claimsReserveForGross = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ClaimsReserveForGross)                    
                    Select Case CType(Session.Item(CNMode), Mode)
                        Case Mode.NewClaim
                            For Each oRecovery As NexusProvider.PerilRecovery In oSalvageList
                                If oRecovery.BaseRecoveryKey <> 0 Then
                                    dSalvageTotal += oRecovery.ThisReceiptINCLTax + oRecovery.ReceiptedAmount
                                End If
                            Next
                        Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend, Mode.ViewClaimPayment
                            If Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.PayClaim _
                            Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                            Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment _
                            Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                                For Each oRecovery As NexusProvider.PerilRecovery In oSalvageList
                                    If oRecovery.BaseRecoveryKey <> 0 AndAlso claimsReserveForGross.OptionValue = "0" Then
                                        Dim currentRecovery As String = oRecovery.ThisReceiptINCLTax + oRecovery.ReceiptedAmount - oRecovery.ThisReceiptTax
                                        dSalvageTotal += currentRecovery
                                    ElseIf oRecovery.BaseRecoveryKey <> 0 Then
                                        Dim currentRecovery As String = oRecovery.ThisReceiptINCLTax + oRecovery.ReceiptedAmount
                                        dSalvageTotal += currentRecovery
                                    End If
                                Next
                            End If
                    End Select
                End If
                If e.Row.FindControl("lblSumInsured") IsNot Nothing Then
                    CType(e.Row.FindControl("lblSumInsured"), Label).Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.PerilSummary).SumInsured)
                End If


                If e.Row.FindControl("lblReserveTotal") IsNot Nothing Then
                    If CType(Session(CNEnablePayClaim), Boolean) Then

                    Else
                        CType(e.Row.FindControl("lblReserveTotal"), Label).Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.PerilSummary).CurrentReserve)
                    End If
                End If

                If e.Row.FindControl("lblAmountPaid") IsNot Nothing Then
                    'Payment Amount doesnt Get updated in PaidAmount So populating the right value
                    If CType(Session(CNEnablePayClaim), Boolean) AndAlso Session(CNMode) = Mode.PayClaim Then
                        If ViewState("dPaidAmount") IsNot Nothing AndAlso e.Row.RowIndex = Convert.ToInt32(Session(CNClaimPerilIndex)) Then
                            dTotalPaid = String.Format(oFormatString, Convert.ToDecimal(ViewState("dPaidAmount")) + oClaim.ClaimPeril(e.Row.RowIndex).PaidAmount)
                        Else
                            dTotalPaid = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.PerilSummary).TotalPaidAmount)
                        End If
                    Else
                        dTotalPaid = oClaim.ClaimPeril(e.Row.RowIndex).PaidAmount
                    End If
                    CType(e.Row.FindControl("lblAmountPaid"), Label).Text = String.Format(oFormatString, dTotalPaid)
                End If

                If e.Row.FindControl("lblCurrentReserves") IsNot Nothing Then
                    If CType(Session(CNEnablePayClaim), Boolean) AndAlso Session(CNMode) = Mode.PayClaim Then
                        'calculate total amount paid, since SAM call is not made so need to calculate manually
                        Dim dCurrentReserve As Double = CType(e.Row.DataItem, NexusProvider.PerilSummary).CurrentReserve
                        If dCurrentReserve >= 0 Then
                            dCurrentReserve = dTotal - CType(e.Row.DataItem, NexusProvider.PerilSummary).PaidAmount
                        End If
                        For Each PerilItemIndex As Integer In PerilsIndex
                            If e.Row.RowIndex = Convert.ToInt32(PerilItemIndex) Then
                                dCurrentReserve = dCurrentReserve - (dTotalPaid - oClaim.ClaimPeril(e.Row.RowIndex).PaidAmount)
                            End If
                        Next
                        If dCurrentReserve < 0 Then
                            CType(e.Row.FindControl("lblCurrentReserves"), Label).Text = String.Format(oFormatString, (0.0))

                        Else
                            CType(e.Row.FindControl("lblCurrentReserves"), Label).Text = String.Format(oFormatString, dCurrentReserve)
                        End If
                    Else
                        Dim dCurrentReserve As Double
                        dCurrentReserve = dTotal - CType(e.Row.DataItem, NexusProvider.PerilSummary).PaidAmount
                        If dCurrentReserve < 0 Then
                            CType(e.Row.FindControl("lblCurrentReserves"), Label).Text = String.Format(oFormatString, (0.0))
                        Else
                            CType(e.Row.FindControl("lblCurrentReserves"), Label).Text = String.Format(oFormatString, dCurrentReserve)
                        End If
                    End If
                End If

                'Incurred (Reserve Total)
                If e.Row.FindControl("lblReserveTotal") IsNot Nothing AndAlso PerilsIndex IsNot Nothing Then
                    For Each PerilItemIndex As Integer In PerilsIndex
                        If CType(Session(CNEnablePayClaim), Boolean) And Session(CNMode) = Mode.PayClaim _
                   And CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey = oClaim.ClaimPeril(PerilItemIndex).ClaimPerilKey Then
                            CType(e.Row.FindControl("lblReserveTotal"), Label).Text = String.Format(oFormatString, dTotal - dRecoveryTotal - dSalvageTotal)
                        Else
                            CType(e.Row.FindControl("lblReserveTotal"), Label).Text = String.Format(oFormatString, dTotal - dRecoveryTotal - dSalvageTotal)
                        End If
                    Next

                End If

                If e.Row.Cells(5).Controls(0) IsNot Nothing Then
                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim _
                    Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim _
                    Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                    Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment Then
                        Dim bClaimBuilder As Boolean
                        Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                        If bClaimBuilder = True Then

                            Dim WebRoot As String = AppSettings("WebRoot")

                            'Check Peril Builder
                            Dim sFirstPage, sConfigFile, sFolder As String
                            sFolder = String.Empty

                            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.PerilTypes.PerilType(Trim(oClaim.ClaimPeril(e.Row.RowIndex).TypeCode)) IsNot Nothing Then
                                sFolder = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                                    & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(e.Row.RowIndex).TypeCode).Folder
                            End If

                            sConfigFile = sFolder & "/perilscreens.config"

                            If sFolder IsNot String.Empty AndAlso System.IO.File.Exists(Server.MapPath(sConfigFile)) = True Then
                                sFirstPage = FrameWorkFunctions.GetFirstRiskScreen("~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                                       & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(e.Row.DataItemIndex).TypeCode).Folder & "/perilscreens.config")

                                Dim sUrl As String = "~/Claims/ClientPages/" & oPortal.Claims.ScreenLocation & "/Perils/" _
                                    & oPortal.Claims.PerilTypes.PerilType(oClaim.ClaimPeril(e.Row.DataItemIndex).TypeCode).Folder _
                                    & "/" & sFirstPage & "?OI=CP" & oClaim.ClaimPeril(e.Row.DataItemIndex).ClaimPerilKey & "&PerilID=" & oClaim.ClaimPeril(e.Row.DataItemIndex).ClaimPerilKey & "&PerilIndex=" & e.Row.DataItemIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/")

                                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_grdvPerils_linkReserves_text")
                                ElseIf Session(CNMode) = Mode.PayClaim Then
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_PayReserve")
                                    If m_sIsPaymentsReadOnly = "1" AndAlso Not IsPaymentDoneViaScriptForPeril(oClaim.ClaimPeril(e.Row.DataItemIndex).ClaimPerilKey) Then
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Enabled = False
                                    End If
                                ElseIf Session(CNMode) = Mode.ViewClaim _
                                Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                                Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment Then
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_ViewReserve")
                                End If


                                If bAllowMultipleClaimPayment = True Then
                                    If oClaim.ClaimPeril Is Nothing OrElse oClaim.ClaimPeril.Count = 0 Then
                                        Dim sMsgNoScreenConfigured As String = GetLocalResourceObject("sMsgNoScreenConfigured")
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "alert('" + sMsgNoScreenConfigured + "');return false")
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Remove("href")
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("target", "_blank")
                                    End If
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).NavigateUrl = sUrl
                                Else
                                    If Session(CNMAXUNAUTHORISEDCLAIMVALUE) IsNot Nothing AndAlso Session(CNMAXUNAUTHORISEDCLAIMVALUE) = True Then
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsgForValue();")
                                    Else
                                        'if Allow Multiple Claim Payment is 'false' then multiple claim payment error message will display
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsg();")
                                    End If
                                End If

                            Else
                                'if Peril Builder Screen is not Configured
                                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_grdvPerils_linkReserves_text")
                                ElseIf Session(CNMode) = Mode.PayClaim Then
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_PayReserve")
                                    If m_sIsPaymentsReadOnly = "1" AndAlso Not IsPaymentDoneViaScriptForPeril(oClaim.ClaimPeril(e.Row.DataItemIndex).ClaimPerilKey) Then
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Enabled = False
                                    End If
                                ElseIf Session(CNMode) = Mode.ViewClaim _
                                Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                                Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment Then

                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_ViewReserve")
                                End If

                                If Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = True Then
                                    'If claim builder is ON and Perile builder Page is not found then 
                                    'still user will return to the page of the claim builder from where action where initiated
                                    If bAllowMultipleClaimPayment = True Then
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).NavigateUrl = "~/Claims/PerilDetails.aspx?FromPage=RI&PerilID=" & CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey & "&PerilIndex=" & e.Row.DataItemIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/")
                                    Else
                                        If Session(CNMAXUNAUTHORISEDCLAIMVALUE) IsNot Nothing AndAlso Session(CNMAXUNAUTHORISEDCLAIMVALUE) = True Then
                                            CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsgForValue();")
                                        Else
                                            'if Allow Multiple Claim Payment is 'false' then multiple claim payment error message will display
                                            CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsg();")
                                        End If
                                    End If

                                Else
                                    'If claim builder is OFF and Perile builder Page is not found then 
                                    'user will return to the perils.aspx page
                                    If bAllowMultipleClaimPayment = True Then
                                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).NavigateUrl = "~/Claims/PerilDetails.aspx?FromPage=RI&PerilID=" & CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey & "&PerilIndex=" & e.Row.DataItemIndex & ""
                                    Else
                                        If Session(CNMAXUNAUTHORISEDCLAIMVALUE) IsNot Nothing AndAlso Session(CNMAXUNAUTHORISEDCLAIMVALUE) = True Then
                                            CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsgForValue();")
                                        Else
                                            'if Allow Multiple Claim Payment is 'false' then multiple claim payment error message will display
                                            CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsg();")
                                        End If
                                    End If

                                End If
                            End If
                        Else
                            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
                                CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_grdvPerils_linkReserves_text")
                            ElseIf Session(CNMode) = Mode.PayClaim Then
                                CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_PayReserve")
                            ElseIf Session(CNMode) = Mode.ViewClaim _
                            Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.DeclinePayment _
                            Or Session(CNMode) = Mode.Recommend Or Session(CNMode) = Mode.ViewClaimPayment Then
                                CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Text = GetLocalResourceObject("lbl_ViewReserve")
                            End If

                            If bAllowMultipleClaimPayment = True Then
                                CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).NavigateUrl = "~/Claims/PerilDetails.aspx?FromPage=RI&PerilID=" & CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey & "&PerilIndex=" & e.Row.DataItemIndex & "&ReturnUrl=" & Request.Path.Replace(WebRoot, "~/")
                            Else
                                If Session(CNMAXUNAUTHORISEDCLAIMVALUE) IsNot Nothing AndAlso Session(CNMAXUNAUTHORISEDCLAIMVALUE) = True Then
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsgForValue();")
                                Else
                                    CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Attributes.Add("onclick", "javascript:MultipleClaimPaymentErrorMsg();")
                                End If
                            End If

                        End If
                    Else
                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Visible = False
                    End If
                End If
                'Check the flag ShowRecovery to display Recovery link in grid
                If oPortal.Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)) IsNot Nothing Then
                    If oPortal.Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).ShowRecovery = True AndAlso Session(CNMode) <> Mode.PayClaim Then
                        'Now check the Salvage or Thirdparty flag to display Recovery link in grid
                        If (Session(CNCheckSalvageRecovery) = True Or Session(CNCheckTPRecovery) = True) Then
                            If e.Row.Cells(5).Controls(0) IsNot Nothing Then
                                'Set the link visible true
                                e.Row.Cells(5).FindControl("lnkRecoveries").Visible = True
                                If Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = True Then
                                    Dim WebRoot As String = AppSettings("WebRoot")
                                    Dim sReturnUrl As String = Request.Path.Replace(WebRoot, "~/")
                                    CType(e.Row.Cells(5).FindControl("lnkRecoveries"), HyperLink).NavigateUrl = "~/Claims/Recoveries.aspx?PerilIndex=" & e.Row.DataItemIndex & "&PerilID=" & CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey & "&ReturnURL=" & sReturnUrl & ""
                                Else
                                    CType(e.Row.Cells(5).FindControl("lnkRecoveries"), HyperLink).NavigateUrl = "~/Claims/Recoveries.aspx?PerilIndex=" & e.Row.DataItemIndex & "&PerilID=" & CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey & ""
                                End If

                            End If
                        End If
                    End If
                End If

                If CType(Session(CNEnablePayClaim), Boolean) Then
                    If CType(Session.Item(CNMode), Mode) = Mode.PayClaim Then
                        CType(e.Row.Cells(5).FindControl("lnkReserves"), HyperLink).Enabled = False
                    ElseIf CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Then
                        CType(e.Row.Cells(5).FindControl("lnkSalvageClaim"), LinkButton).Enabled = False
                    ElseIf CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                        CType(e.Row.Cells(5).FindControl("lnkTPRecovery"), LinkButton).Enabled = False
                    End If
                End If

                'Enable/Disable of the link based on different condition
                If CType(Session.Item(CNMode), Mode) = Mode.SalvageClaim Then
                    If e.Row.Cells(5).Controls(0) IsNot Nothing Then
                        For iCount As Integer = 0 To oClaim.ClaimPeril.Count - 1
                            If CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey = oClaim.ClaimPeril(iCount).ClaimPerilKey Then
                                If oClaim.ClaimPeril(iCount).SalvageRecovery IsNot Nothing Then
                                    If oClaim.ClaimPeril(iCount).SalvageRecovery.Count > 0 Then
                                        If oClaim.ClaimPeril(iCount).SalvageRecovery(0).TotalRecovery > 0 Then
                                            CType(e.Row.Cells(5).FindControl("lnkSalvageClaim"), LinkButton).Visible = True
                                            CType(e.Row.Cells(5).FindControl("liSalvageClaim"), HtmlGenericControl).Visible = True
                                            CType(e.Row.Cells(5).FindControl("lnkSalvageClaim"), LinkButton).CommandName = "SalvageClaim"
                                            CType(e.Row.Cells(5).FindControl("lnkSalvageClaim"), LinkButton).CommandArgument = e.Row.DataItemIndex
                                        End If
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If

                'Enable/Disable of the link based on different condition
                If CType(Session.Item(CNMode), Mode) = Mode.TPRecovery Then
                    If e.Row.Cells(5).Controls(0) IsNot Nothing Then
                        For iCount As Integer = 0 To oClaim.ClaimPeril.Count - 1
                            If CType(e.Row.DataItem, NexusProvider.PerilSummary).ClaimPerilKey = oClaim.ClaimPeril(iCount).ClaimPerilKey Then
                                If oClaim.ClaimPeril(iCount).TPRecovery IsNot Nothing Then
                                    If oClaim.ClaimPeril(iCount).TPRecovery.Count > 0 Then
                                        For tpCount As Integer = 0 To oClaim.ClaimPeril(iCount).TPRecovery.Count - 1 'code changes for looping through every TPRecovery record -- TFS Defect No. 1347
                                            If oClaim.ClaimPeril(iCount).TPRecovery(tpCount).TotalRecovery > 0 Then
                                                CType(e.Row.Cells(5).FindControl("lnkTPRecovery"), LinkButton).Visible = True
                                                CType(e.Row.Cells(5).FindControl("liTPRecovery"), HtmlGenericControl).Visible = True
                                                CType(e.Row.Cells(5).FindControl("lnkTPRecovery"), LinkButton).CommandName = "TPRecovery"
                                                CType(e.Row.Cells(5).FindControl("lnkTPRecovery"), LinkButton).CommandArgument = e.Row.DataItemIndex
                                            End If
                                        Next
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
            End If
        End Sub

#End Region

#Region " Button Events "


#End Region

        Protected Sub IsPaymentReceived_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles IsPaymentReceived.ServerValidate
            Dim oClaimOpen As NexusProvider.ClaimOpen = Session.Item(CNClaim)
            Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
            If Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                PerilsIndex = Session(CNClaimMultiPerilIndex)
            Else
                PerilsIndex.Add(CInt(Session(CNClaimPerilIndex)))
            End If
            Dim dAmount As Decimal = 0.0

            If Session(CNMode) = Mode.PayClaim Then
                'For Claim Payments
                For Each PerilItemIndex As Integer In PerilsIndex
                    If m_sIsPaymentsReadOnly = "1" Then
                        dAmount = GetScriptPerilPaidAmount(Nothing, Nothing, PerilItemIndex)
                    ElseIf CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment IsNot Nothing Then
                        For Each oPaymentItem As NexusProvider.ClaimPaymentItemType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(PerilItemIndex).Payment.ClaimPaymentItem
                            dAmount += (oPaymentItem.PaymentAmount - oPaymentItem.TaxAmount)
                        Next
                    Else
                    End If
                    If dAmount = 0 Then
                        args.IsValid = False
                    End If
                    If dAmount = 0 OrElse Not CType(Session(CNEnablePayClaim) = True, Boolean) Then
                        args.IsValid = False
                        If m_sIsPaymentsReadOnly = "1" Then
                            If dAmount = 0 Then
                                IsPaymentReceived.ErrorMessage = GetLocalResourceObject("msg_ScriptPaymentNotRecieved_Error").ToString()
                            Else
                                IsPaymentReceived.ErrorMessage = GetLocalResourceObject("msg_ScriptPaymentNotEdited_Error").ToString()
                            End If
                        End If
                    End If
                Next
            ElseIf Session(CNMode) = Mode.SalvageClaim Then
                dAmount = 0
                For Each PerilItemIndex As Integer In PerilsIndex
                    'For Claim Salvage Receipts
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery.Count - 1
                        dAmount = dAmount + oClaimOpen.ClaimPeril(PerilItemIndex).SalvageRecovery(iCount).ThisReceiptINCLTax
                    Next
                    If dAmount = 0 Then
                        args.IsValid = False
                    End If
                Next
            ElseIf Session(CNMode) = Mode.TPRecovery Then
                'For Claim TPRecovery Receipt
                dAmount = 0
                For Each PerilItemIndex As Integer In PerilsIndex
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery.Count - 1
                        dAmount = dAmount + oClaimOpen.ClaimPeril(PerilItemIndex).TPRecovery(iCount).ThisReceiptINCLTax
                    Next
                    If dAmount = 0 Then
                        args.IsValid = False
                    End If
                Next
            Else
                args.IsValid = True
            End If

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
            Dim oClaimQuote As NexusProvider.Quote = Session(CNClaimQuote)

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
            oWebservice.RunValidationRules(Trim(UCase(v_sScreenCode)), oClaimRisk.XMLDataSet, oOriginalClaim.ClaimKey, Nothing, oClaimQuote.BranchCode)
            Session(CNDataSet) = oClaimRisk.XMLDataSet

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

        'Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinish.Click
        '    'Clear the selected peril key 
        '    Session.Remove(CNClaimPerilKey)
        '    If Page.IsValid Then
        '        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        '        Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
        '        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        '        If Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
        '            'For Salvage and TP Recover Mode
        '            Response.Redirect("~/claims/changeclaim.aspx")
        '        ElseIf Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Then
        '            'Validate the Claim builder risk screen
        '            If ValidateData() = False Then
        '                If Session(CNMode) = Mode.PayClaim Then
        '                    Dim sRunAuthorizePayment As String
        '                    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
        '                    'if error is not thrown
        '                    'get the Product Risk option setting named Run Authorize Payment
        '                    sRunAuthorizePayment = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.RunAuthorisationScriptsClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
        '                    'get the Claim Workflow Setting
        '                    oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
        '                    ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow

        '                    If sRunAuthorizePayment = "1" Then
        '                        Session(CNAuthorizeStatus) = "Authorize Payment"
        '                        ' Response.Redirect("~/claims/summary.aspx")
        '                        'if ShowSummary is false then skip the summary page
        '                        If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
        '                            'show the summary page
        '                            Response.Redirect("~/claims/summary.aspx")
        '                        Else
        '                            'skip the summary page
        '                            SkipSummaryPage()
        '                        End If
        '                    ElseIf oRunClaimWorkFlow.CashPaymentProcess = True Then
        '                        Dim dAmount As Decimal = 0.0
        '                        If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve IsNot Nothing Then
        '                            For Each oPaymentItem As NexusProvider.ClaimPerilReservePaymentType In CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
        '                                dAmount += oPaymentItem.ThisPaymentINCLTax
        '                            Next
        '                        End If
        '                        If dAmount > 0 Then
        '                            'Amount is positive so need to receive it
        '                            Response.Redirect("~/secure/payment/CashList.aspx")
        '                        Else
        '                            'Amount is negative so no need to receive it, sp process it directly
        '                            ' Response.Redirect("~/claims/summary.aspx")
        '                            'if ShowSummary is false then skip the summary page
        '                            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
        '                                'show the summary page
        '                                Response.Redirect("~/claims/summary.aspx")
        '                            Else
        '                                'skip the summary page
        '                                SkipSummaryPage()
        '                            End If
        '                        End If
        '                    Else
        '                        'If RunAuthorize is OFF and Cash Payment Process is OFF
        '                        '   Response.Redirect("~/claims/summary.aspx")
        '                        'if ShowSummary is false then skip the summary page
        '                        If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
        '                            'show the summary page
        '                            Response.Redirect("~/claims/summary.aspx")
        '                        Else
        '                            'skip the summary page
        '                            SkipSummaryPage()
        '                        End If
        '                    End If
        '                ElseIf Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Then
        '                    'if error is not thrown
        '                    '  Response.Redirect("~/claims/summary.aspx")
        '                    'if ShowSummary is false then skip the summary page
        '                    If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
        '                        'show the summary page
        '                        Response.Redirect("~/claims/summary.aspx")
        '                    Else
        '                        'skip the summary page
        '                        SkipSummaryPage()
        '                    End If
        '                End If
        '            End If
        '        ElseIf Session(CNMode) = Mode.ViewClaim Then
        '            '  Response.Redirect("~/claims/summary.aspx")
        '            'if ShowSummary is false then skip the summary page
        '            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = True Then
        '                'show the summary page
        '                Response.Redirect("~/claims/summary.aspx")
        '            Else
        '                'skip the summary page
        '                SkipSummaryPage()
        '            End If
        '        End If
        '    End If
        'End Sub
        Function ValidateData() As Boolean
            Dim bResult As Boolean = False
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim sResult As String = Nothing
            Dim sScreenCode As String
            Dim oclaimRisk As New NexusProvider.ClaimRisk
            'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey

            oclaimRisk.ClaimKey = oClaim.BaseClaimKey
            oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
            oclaimRisk.XMLDataSet = Session.Item(CNDataSet)

            Dim sFolder As String
            Dim sClaimConfigFile As String

            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)) Is Nothing Then
                'use the default folder If risk type is not configured
                sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                          & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.DefaultFolder
            ElseIf String.IsNullOrEmpty(oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder) = True Then
                'use the default folder if folder is empty
                sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                          & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.DefaultFolder
            Else
                'we have the risk type specified so use that folder
                sFolder = AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                          & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder
            End If

            sClaimConfigFile = sFolder & "/claimscreens.config"

            If System.IO.File.Exists(Server.MapPath(sClaimConfigFile)) = True Then

                sScreenCode = GetScreenCode(sClaimConfigFile)

                sResult = ValidateData(Trim(UCase(sScreenCode)), oclaimRisk)

                If String.IsNullOrEmpty(sResult) = False Then
                    bResult = True
                    IsValidReserve.ErrorMessage = sResult
                    IsValidReserve.IsValid = False
                End If
            End If

            Return bResult
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            If Session(CNMaxClaimPaymentValue) IsNot Nothing Then
                Session.Remove(CNMaxClaimPaymentValue)
            End If

            'Checking Allow Multiple Claim Payment
            Select Case Session(CNMode)
                Case Mode.ViewClaim, Mode.Authorise, Mode.DeclinePayment, Mode.Recommend, Mode.ViewClaimPayment
                    If GetLocalResourceObject("Perils_ViewReservesMessage_view") IsNot Nothing Then
                        ltViewReservesMessage.Text = GetLocalResourceObject("Perils_ViewReservesMessage_view").ToString()
                        ltViewReservesMessage.Text = ltViewReservesMessage.Text.Replace("Edit", GetLocalResourceObject("lbl_ViewReserve"))
                    End If

                Case Mode.PayClaim
                    If GetLocalResourceObject("Perils_ViewReservesMessage_pay") IsNot Nothing Then
                        ltViewReservesMessage.Text = GetLocalResourceObject("Perils_ViewReservesMessage_pay").ToString()
                        ltViewReservesMessage.Text = ltViewReservesMessage.Text.Replace("Edit", GetLocalResourceObject("lbl_PayReserve"))
                    End If

                Case Mode.SalvageClaim
                    If GetLocalResourceObject("Perils_ViewReservesMessage_salvage") IsNot Nothing Then
                        ltViewReservesMessage.Text = GetLocalResourceObject("Perils_ViewReservesMessage_salvage").ToString()
                        ltViewReservesMessage.Text = ltViewReservesMessage.Text.Replace("Edit", GetLocalResourceObject("lbl_grdvPerils_linkSalvage_text"))
                    End If

                Case Mode.TPRecovery
                    If GetLocalResourceObject("Perils_ViewReservesMessage_tpr") IsNot Nothing Then
                        ltViewReservesMessage.Text = GetLocalResourceObject("Perils_ViewReservesMessage_tpr").ToString()
                        ltViewReservesMessage.Text = ltViewReservesMessage.Text.Replace("Edit", GetLocalResourceObject("lbl_grdvPerils_linkTPRecovery_text"))
                    End If
                Case Mode.EditClaim, Mode.NewClaim
                    If GetLocalResourceObject("Perils_ViewReservesMessage") IsNot Nothing Then
                        ltViewReservesMessage.Text = GetLocalResourceObject("Perils_ViewReservesMessage").ToString()
                        ltViewReservesMessage.Text = ltViewReservesMessage.Text.Replace("Edit", GetLocalResourceObject("lbl_grdvPerils_linkReserves_text").ToString())
                    End If
            End Select

            If Session(CNMode) = Mode.PayClaim Then
                oWebservice = New NexusProvider.ProviderManager().Provider
                m_sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                If AllowMultipleClaimPayment() = True Then
                    bAllowMultipleClaimPayment = True
                Else
                    bAllowMultipleClaimPayment = False
                End If
            End If
            sIsGrossClaimPaymentAmount = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
            If String.IsNullOrEmpty(sIsGrossClaimPaymentAmount) Then
                sIsGrossClaimPaymentAmount = "0"
            End If
        End Sub


        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not IsPostBack Then

                'if page is loaded first time then setting of the status of progres bar
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                Select Case CType(Session.Item(CNMode), Mode)
                    Case Mode.NewClaim ' In case of New Claim
                        'Check the salvage and third party recovery at product level
                        CheckRecoveriesAtProductLevel(NexusProvider.ClaimProcessType.OpenClaim)

                    Case Mode.EditClaim, Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery

                        If CType(Session.Item(CNMode), Mode) = Mode.EditClaim Then
                            'Check the salvage and third party recovery at product level
                            CheckRecoveriesAtProductLevel(NexusProvider.ClaimProcessType.MaintainClaim)
                        End If
                End Select

                'Binding Peril information to GridView
                BindPerils()
            End If
        End Sub

    End Class

End Namespace

