Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus.Utils
Imports Nexus
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Linq
Imports System
Namespace Nexus

    Partial Class Controls_MultiRisk : Inherits System.Web.UI.UserControl
        'This is the status of the renewal Waiting Status in Back office
        Const sAwaiting_Manual_Preview = "Awaiting Manual Review"
        Const sAwaiting_Renewal_Notice = "Awaiting Renewal Notice Print"
        Const sAwaiting_Update = "Awaiting Update"

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oRiskType As New NexusProvider.RiskType
        Dim bIsInBackDatedMode As Boolean
        Dim bNotTraverse As Boolean = False
        Private Const sCtrlTabIndexControlID As String = "_ctrlTabIndex"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnMarkedQuoteConfirmation",
                       "<script language=""JavaScript"" type=""text/javascript"">function UnMarkedQuoteConfirmation(){var IsConfirm; IsConfirm=confirm('" & GetLocalResourceObject("msg_ConfirmUnMarkedCollection").ToString() & "');return IsConfirm;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteConfirmation",
                "<script language=""JavaScript"" type=""text/javascript"">function DeleteConfirmation(){return confirm('" & GetLocalResourceObject("msg_DelRisk").ToString() & "');}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteClaimRiskConfirmation",
            "<script language=""JavaScript"" type=""text/javascript"">function DeleteClaimRiskConfirmation(){return confirm('" & GetLocalResourceObject("msg_DelClaimRisk").ToString() & "');}</script>")


        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Session.Remove(CNRatingSections)
            ' Check if a backdated MTA version is in session.
            bIsInBackDatedMode = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)

            If (Not IsPostBack) Or (Request("__EVENTARGUMENT") = "CopyRiskTypeSelected") Or (Request("__EVENTARGUMENT") = "RefreshFees") Then
                Dim olblTotalPremium As Label = CType(CType(GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("lblPremiumValue"), Label)
                'count the risk minus IsMandatory=true
                Dim iRiskCount As Integer = 0
                For Each oRisk As Nexus.Library.Config.RiskType In oProduct.RiskTypes
                    If oRisk.IsMandatory = False Then
                        iRiskCount += 1
                    End If
                Next

                Dim iQuoteAllRiskNB As Integer
                Dim iQuoteAllRiskMTC As Integer
                Dim iQuoteAllRiskMTA As Integer
                Dim iQuoteAllRiskRenewal As Integer
                Dim bEnableBtnQuoteAll As Boolean = False
                Dim bEnableBtnQuoteAll1 As Boolean = False
                btnQuoteAll.Enabled = False

                iQuoteAllRiskNB = ConvertToSafeInteger(oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                                            NexusProvider.ProductRiskOptions.QuoteAllRiskNB,
                                                                            NexusProvider.RiskTypeOptions.None,
                                                                           oQuote.ProductCode,
                                                                            Nothing))
                If iQuoteAllRiskNB = 1 AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "QUOTE" Then
                    bEnableBtnQuoteAll = True
                End If

                iQuoteAllRiskMTC = ConvertToSafeInteger(oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                                         NexusProvider.ProductRiskOptions.QuoteAllRiskMTC,
                                                                         NexusProvider.RiskTypeOptions.None,
                                                                        oQuote.ProductCode,
                                                                         Nothing))

                If iQuoteAllRiskMTC = 1 AndAlso (oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQREINS" Or oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQCAN") Then
                    bEnableBtnQuoteAll = True
                End If

                iQuoteAllRiskMTA = ConvertToSafeInteger(oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                         NexusProvider.ProductRiskOptions.QuoteAllRiskMTA,
                                                         NexusProvider.RiskTypeOptions.None,
                                                        oQuote.ProductCode,
                                                         Nothing))

                If iQuoteAllRiskMTA = 1 AndAlso (oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQUOTE") Then
                    bEnableBtnQuoteAll = True
                End If

                iQuoteAllRiskRenewal = ConvertToSafeInteger(oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                                                         NexusProvider.ProductRiskOptions.QuoteAllRiskRenewal,
                                                         NexusProvider.RiskTypeOptions.None,
                                                        oQuote.ProductCode,
                                                         Nothing))

                If iQuoteAllRiskRenewal = 1 AndAlso (oQuote.InsuranceFileTypeCode.Trim().ToUpper = "RENEWAL") Then
                    bEnableBtnQuoteAll = True
                End If


                If bEnableBtnQuoteAll = True Then
                    For jCount As Integer = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(jCount).StatusCode.Trim().ToUpper() = "UNQUOTED" Then
                            If oQuote.Risks(jCount).IsAutoRated = True Then
                                btnQuoteAll.Enabled = True
                            ElseIf oQuote.Risks(jCount).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "QUOTE" Then
                                btnQuoteAll.Enabled = True
                            ElseIf oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQUOTE" OrElse oQuote.InsuranceFileTypeCode.Trim().ToUpper = "RENEWAL" OrElse oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQREINS" OrElse oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQCAN" Then
                                btnQuoteAll.Enabled = True
                            End If

                        End If
                    Next
                End If

                'count whether only mandatory risk is only configured then AddRisk would not be visible
                If Session(CNMode) = Mode.View Or iRiskCount = 0 Then
                    btnAddRisk.Visible = False
                End If

                oWebService.GetHeaderAndRisksByKey(oQuote)


                Dim oRiskCollection As New NexusProvider.RiskCollection
                oRiskCollection = oQuote.Risks

                btnNoChangeAll.Enabled = False
                btnNoChangeAll.Visible = False

                If bIsInBackDatedMode Then

                    btnNoChangeAll.Visible = True

                    Dim oRisk = From oRiskData In oRiskCollection
                                Where (oRiskData.StatusCode.ToUpper.Trim <> "QUOTED" And oRiskData.StatusCode.ToUpper.Trim <> "DELETED")
                                Select oRiskData.Key

                    For Each oRiskDetails In oRisk
                        If Convert.ToInt32(oRiskDetails) <> 0 Then
                            btnNoChangeAll.Enabled = True
                            Exit For
                        End If

                    Next

                End If


                grdvRisk.DataSource = oRiskCollection
                grdvRisk.DataBind()
                CalculatePremium()

                ' In case of cancel Policy we should not Display the Add Risk Button
                If Session(CNMTAType) = MTAType.CANCELLATION Or oProduct.AllowMultiRisks = False Then
                    btnAddRisk.Visible = False
                End If

                If iRiskCount > 1 AndAlso oProduct.AllowMultiRisks = True Then
                    If HttpContext.Current.Session.IsCookieless Then
                        btnAddRisk.OnClientClick = "tb_show(null , '" & "../" & "(S(" & Session.SessionID.ToString() + "))" & "/modal/SelectRiskType.aspx?ProductCode=" & oProduct.ProductCode & "&modal=true&KeepThis=true&FromPage=MultiRisk1&TB_iframe=true&height=500&width=700' , null);return false;"
                    Else
                        btnAddRisk.OnClientClick = "tb_show(null , '../modal/SelectRiskType.aspx?ProductCode=" & oProduct.ProductCode & "&modal=true&KeepThis=true&FromPage=MultiRisk1&TB_iframe=true&height=500&width=700' , null);return false;"
                    End If
                End If
                If (Request("__EVENTARGUMENT") = "CopyRiskTypeSelected") Then
                    NavigateRiskScreenOnCopy()
                End If
            Else
                'check if the postback has been triggered by the modal dialog
                If Request("__EVENTARGUMENT") = "RiskTypeSelected" Then
                    'get risk type from session
                    oRiskType = Session(CNRiskType)
                    'redirect to first risk screen for the current risk type
                    AddRiskAndRedirect()
                End If
            End If

            'This will open a risk selection window if no risk added(except mandatory risk) with quote
            'AllowMultirisk functionality will remain same as previous
            If Not Page.IsPostBack Then
                If Session(CNQuote) IsNot Nothing Then
                    'We need to open risk selection window if no risk added already
                    If (Session(CNMode) = Mode.Add Or Session(CNMode) = Mode.Edit) Then
                        Dim bShowRiskSelection As Boolean = False
                        If oQuote.Risks Is Nothing Then
                            bShowRiskSelection = True
                        Else
                            If oQuote.Risks.Count < 1 Then
                                bShowRiskSelection = True
                            ElseIf oQuote.Risks.Count = 1 Then
                                If oQuote.Risks(0).IsMandatoryRisk = True Then
                                    bShowRiskSelection = True
                                End If
                            End If
                        End If
                        If bShowRiskSelection = True Then
                            ''find the risk type associated with this product
                            Dim oNexus As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                            Dim oPortalConfig As Nexus.Library.Config.Portal = oNexus.Portals.Portal(Portal.GetPortalID())
                            Dim oProductConfiguration As Nexus.Library.Config.Product
                            Dim oRiskType As NexusProvider.RiskType = New NexusProvider.RiskType
                            oProductConfiguration = oPortalConfig.Products.Product(oQuote.ProductCode)

                            'count the risk minus IsMandatory=true
                            Dim iRiskCount As Integer = 0
                            For Each oRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                                If oRisk.IsMandatory = False Then
                                    iRiskCount += 1
                                End If
                            Next

                            'Check RiskTypes for selected product and for more than one RiskType open the Modal dialog Box
                            If oProductConfiguration.RiskTypes.Count = 1 AndAlso iRiskCount = 0 Then
                                'if only risk is there and it is mandatory 
                                Dim oRisk As Nexus.Library.Config.RiskType = oProductConfiguration.RiskTypes.RiskType(0)
                                ''set up the risk type object from the details in config
                                oRiskType.DataModelCode = oRisk.DataModelCode
                                oRiskType.Name = oRisk.Name
                                oRiskType.Path = oRisk.Path
                                oRiskType.RiskCode = oRisk.RiskCode
                                Session(CNRiskType) = oRiskType
                                'now redirect
                                AddRiskAndRedirect()
                            ElseIf iRiskCount = 1 Or oProductConfiguration.AllowMultiRisks = False Then
                                'there's only one risk type so add this risk type to session
                                For Each oRisk As Nexus.Library.Config.RiskType In oProductConfiguration.RiskTypes
                                    If oRisk.IsMandatory = False Then
                                        ''set up the risk type object from the details in config
                                        oRiskType.DataModelCode = oRisk.DataModelCode
                                        oRiskType.Name = oRisk.Name
                                        oRiskType.Path = oRisk.Path
                                        oRiskType.RiskCode = oRisk.RiskCode
                                        Session(CNRiskType) = oRiskType
                                        Exit For
                                    End If
                                Next

                                'now redirect
                                AddRiskAndRedirect()
                            ElseIf iRiskCount > 1 AndAlso oProductConfiguration.AllowMultiRisks = True Then
                                'more than one risk type so we need to open the modal dialog
                                Dim sUrl As String
                                If HttpContext.Current.Session.IsCookieless Then
                                    sUrl = "../Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                                Else
                                    sUrl = AppSettings("WebRoot") & "/Modal/SelectRiskType.aspx?ProductCode=" & oProductConfiguration.ProductCode & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=500&width=700"
                                End If

                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
                            End If
                        End If
                    End If
                End If

                If bIsInBackDatedMode Then
                    btnAddRisk.Visible = False
                End If
            End If
        End Sub

        Private Sub AddRiskAndRedirect()
            'Sub sets session variables and redirects to the correct screen for current risk type
            'This is either called from:
            'a - the add risk button click or
            'b - if there is more than one risk type for this product then called when postback if triggered from modal dialog

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name

            Session(CNMode) = Mode.Add

            Session(CNQuoteInSync) = False
            Session.Remove(CNOI)
            Session(CNQuoteInSync) = False
            Session(CNQuoteMode) = QuoteMode.FullQuote
            Session.Remove(CNTabState & sCtrlTabIndexControlID)
            If Session(CNRiskType) IsNot Nothing Then
                oRiskType = Session(CNRiskType)

                Dim sRiskFolder As String = sProductFolder & "/" & oRiskType.Path & "/"
                Dim sScreenCode As String = GetScreenCode(sRiskFolder & "/" & oProduct.FullQuoteConfig)

                'set up risk object and add a new risk to the quote
                Dim oRisk As New NexusProvider.Risk(sScreenCode, oRiskType.Name)
                oRisk.DataModelCode = oRiskType.DataModelCode
                oRisk.RiskCode = oRiskType.RiskCode
                oQuote.Risks.Add(oRisk)

                Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1

                ' Req 12: Rollback any existing discount BEFORE adding the new risk, so the new
                ' risk is added on top of original (undiscounted) this_premium values.
                ' Only rollback if there are already quoted risks AND the insurance_file has a
                ' discount applied — checked from DB to avoid stale session state.
                Dim bHasQuotedRisks As Boolean = oQuote.Risks IsNot Nothing AndAlso
                    oQuote.Risks.Cast(Of NexusProvider.Risk)().Any(Function(r)
                                                                       Return r.StatusCode IsNot Nothing AndAlso
                               r.StatusCode.Trim().ToUpper() = "QUOTED"
                                                                   End Function)

                If bHasQuotedRisks Then
                    Dim oDiscount As NexusProvider.PolicyDiscount = oWebService.GetPolicyDiscountInfo(oQuote.InsuranceFileKey)
                    If oDiscount IsNot Nothing AndAlso oDiscount.IsDiscountApplied Then
                        Dim sDiscountTxType As String = "NB"
                        If Session(CNMTAType) IsNot Nothing Then
                            sDiscountTxType = "MTA"
                        ElseIf Session(CNRenewal) IsNot Nothing AndAlso CBool(Session(CNRenewal)) Then
                            sDiscountTxType = "REN"
                        End If
                        oWebService.RollbackPolicyDiscount(oQuote.InsuranceFileKey, 0, sDiscountTxType, 0)
                        ' Clear flags but preserve percentage/reason/recurring — PremiumDisplay
                        ' handler will recalculate discounted premium from fresh DB total.
                        Session(CNPolicyDiscountApplied) = False
                        Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                        Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = Nothing
                        Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_NEW_RISK") = True
                    End If
                End If

                oWebService.AddRisk(oQuote, oQuote.Risks.Count - 1, oQuote.BranchCode)
                Session(CNQuote) = oQuote
                Session.Remove(CNPolicyAllTaxesColl)

                If Session(CNIsBackDatedMTA) AndAlso Not bIsInBackDatedMode Then
                    oWebService.DeleteBackDatedVersions(oQuote.InsuranceFileKey)
                End If

                'Redirect to correct risk screen
                Response.Redirect(sRiskFolder & GetFirstRiskScreen(sRiskFolder & oProduct.FullQuoteConfig), False)

            End If
        End Sub

        Protected Sub btnAddRisk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRisk.Click
            'Only one risk type so set risk type to the first risk in this product and call AddRisk
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            'get the values for the risk type from config
            'there's only one risk type so add this risk type to session
            For Each oRisk As Nexus.Library.Config.RiskType In oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode).RiskTypes
                If oRisk.IsMandatory = False Then
                    ''set up the risk type object from the details in config
                    oRiskType.DataModelCode = oRisk.DataModelCode
                    oRiskType.Name = oRisk.Name
                    oRiskType.Path = oRisk.Path
                    oRiskType.RiskCode = oRisk.RiskCode
                    Session(CNRiskType) = oRiskType
                End If
            Next
            Session.Remove(CNDeletedNode)
            Session(CNRatingSections) = Nothing
            AddRiskAndRedirect()
        End Sub

        Protected Sub grdvRisk_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvRisk.Load
            If grdvRisk.PageCount = 1 Then
                grdvRisk.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvRisk_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvRisk.RowCreated

            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(5).Visible = False 'Hide the Key column
            End If

        End Sub

        Protected Sub grdvRisk_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvRisk.RowCommand

            If Not LCase(e.CommandName).Equals("page") Then

                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                oQuote = Session(CNQuote)

                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"

                Session.Remove(CNDeletedNode)

                Dim bIsInBackDatedMode As Boolean = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)
                Select Case e.CommandName
                    Case "Edit"
                        Session(CNRatingSections) = Nothing
                        Dim nRiskIndex As Integer = CInt(e.CommandArgument)
                        Dim oRiskType As Config.RiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(nRiskIndex).RiskTypeCode.Trim)
                        Dim sStatusFlag As String = oQuote.Risks(nRiskIndex).RiskLinkStatusFlag.ToUpper()
                        sStatusFlag = IIf(Constants.CheckStatusFlags.IndexOf(sStatusFlag) = -1, Nothing, sStatusFlag)

                        If CType(e.CommandSource, LinkButton).Text = GetLocalResourceObject("lbl_View").ToString Then
                            Session(CNMode) = Mode.View
                            Session(CNRiskMode) = RiskMode.View
                            If oQuote.Risks(nRiskIndex).XMLDataset Is Nothing Then
                                oWebService.GetRisk(oQuote.Risks(nRiskIndex).Key, nRiskIndex, oQuote, oQuote.BranchCode, False, sStatusFlag, False)
                                Session(CNQuote) = oQuote
                            End If
                        Else
                            Session(CNMode) = Mode.Edit
                            'Edit link should work as view only mode for deleted risks

                            If oQuote.Risks(nRiskIndex).StatusCode.Trim.ToUpper = RiskStatus.Deleted Then
                                Session(CNRiskMode) = RiskMode.View
                            Else
                                Session(CNRiskMode) = RiskMode.Edit
                            End If

                            'Get the risk data after creating a copy. A new copy of risk will created if bIsForEdit=true passed to getrisk function.
                            oWebService.GetRisk(oQuote.Risks(nRiskIndex).Key, nRiskIndex, oQuote, oQuote.BranchCode, False, sStatusFlag, True)
                            If Session(CNRiskMode) = RiskMode.Edit AndAlso oQuote.Risks(nRiskIndex).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.UNQUOTED.ToString() AndAlso oQuote.Risks(nRiskIndex).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.REFERRED.ToString() AndAlso oQuote.Risks(nRiskIndex).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.DECLINED.ToString() Then
                                oWebService.UpdateRiskStatus(0, oQuote.Risks(nRiskIndex).Key, NexusProvider.RiskStatusType.UNQUOTED, oQuote.BranchCode)
                                oQuote.Risks(nRiskIndex).StatusCode = NexusProvider.RiskStatusType.UNQUOTED.ToString()
                                oQuote.Risks(nRiskIndex).StatusDescription = "Unquoted"
                                If Session(CNIsBackDatedMTA) AndAlso Not bIsInBackDatedMode Then
                                    oWebService.DeleteBackDatedVersions(oQuote.InsuranceFileKey)
                                End If
                            End If
                            Session(CNQuote) = oQuote

                            ' Rollback any existing discount BEFORE the risk is edited, so the
                            ' re-quoted risk gets clean undiscounted base values. Without this,
                            ' discount_original_this_premium is NULL on fresh rating sections
                            ' and a later rollback would corrupt the edited risk's premium.
                            ' Same pattern as AddRiskAndRedirect — immediate DB mutation = immediate rollback.
                            Dim bHasOtherQuotedRisks As Boolean = oQuote.Risks IsNot Nothing AndAlso
                                oQuote.Risks.Cast(Of NexusProvider.Risk)().Any(Function(r)
                                                                                   Return r.StatusCode IsNot Nothing AndAlso
                                           r.StatusCode.Trim().ToUpper() = "UNQUOTED"
                                                                               End Function)

                            If bHasOtherQuotedRisks Then
                                Dim oDiscount As NexusProvider.PolicyDiscount = oWebService.GetPolicyDiscountInfo(oQuote.InsuranceFileKey)
                                If oDiscount IsNot Nothing AndAlso oDiscount.IsDiscountApplied Then
                                    ' Discount is applied in DB — rollback + set flag
                                    Dim sDiscountTxType As String = "NB"
                                    If Session(CNMTAType) IsNot Nothing Then
                                        sDiscountTxType = "MTA"
                                    ElseIf Session(CNRenewal) IsNot Nothing AndAlso CBool(Session(CNRenewal)) Then
                                        sDiscountTxType = "REN"
                                    End If
                                    oWebService.RollbackPolicyDiscount(oQuote.InsuranceFileKey, 0, sDiscountTxType, 0)
                                    ' Clear flags but preserve percentage/reason/recurring — PremiumDisplay
                                    ' handler will recalculate discounted premium from fresh DB total.
                                    Session(CNPolicyDiscountApplied) = False
                                    Session("POLICY_DISCOUNT_APPLIED_TO_RISKS") = False
                                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = Nothing
                                    Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_EDIT_RISK") = True
                                ElseIf Session(CNPolicyDiscountPercentage) IsNot Nothing AndAlso
                                       CDec(Session(CNPolicyDiscountPercentage)) > 0 Then
                                    ' Discount NOT in DB but session has preserved values from a prior edit —
                                    ' just set the flag so PremiumDisplay refreshes the total from DB.
                                    ' No rollback needed since nothing is applied to risk premiums.
                                    Session("POLICY_DISCOUNT_ORIGINAL_TOTAL") = Nothing
                                    Session("POLICY_DISCOUNT_ROLLED_BACK_FOR_EDIT_RISK") = True
                                End If
                            End If
                            'If oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.UNQUOTED.ToString() Then
                            '    oWebService.UpdateRiskStatus(0, oQuote.Risks(nRiskIndex).Key, NexusProvider.RiskStatusType.UNQUOTED, oQuote.BranchCode)
                            '    oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode = NexusProvider.RiskStatusType.UNQUOTED.ToString()
                            '    Session(CNQuote) = oQuote
                            'End If
                            'This Code will check that MarkedQuote exists as well as user has agreed to unmark the Quote
                            If oQuote.MarkedQuoteForCollection = True Then
                                oQuote.MarkedQuoteForCollection = False
                                oQuote.MarkedDateforCollection = Date.Now.Date
                                oWebService.UpdateQuotev2(oQuote, oQuote.BranchCode, oQuote.SubBranchCode)
                                Session(CNQuote) = oQuote
                            End If
                        End If

                        Session.Remove(CNOI)
                        Session.Remove(CNPolicyAllTaxesColl)
                        Session(CNCurrentRiskKey) = nRiskIndex

                    Case "Copy"
                        Session(CNCurrentRiskKey) = e.CommandArgument

                        If Session(CNIsBackDatedMTA) AndAlso Not bIsInBackDatedMode Then
                            oWebService.DeleteBackDatedVersions(oQuote.InsuranceFileKey)
                        End If
                        Session(CNRatingSections) = Nothing
                    Case "Delete"

                        Session(CNMode) = Mode.DeleteRisk

                        Dim nRiskIndex As Integer = CInt(e.CommandArgument)
                        Dim oRiskType As Config.RiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(nRiskIndex).RiskTypeCode.Trim)
                        Dim sStatusFlag As String = If(oQuote.Risks(nRiskIndex).RiskLinkStatusFlag IsNot Nothing, oQuote.Risks(nRiskIndex).RiskLinkStatusFlag.ToUpper(), String.Empty)
                        If sStatusFlag <> String.Empty Then
                            sStatusFlag = IIf(Constants.CheckStatusFlags.IndexOf(sStatusFlag) = -1, Nothing, sStatusFlag)
                        End If
                        oWebService.GetRisk(oQuote.Risks(nRiskIndex).Key, nRiskIndex, oQuote, oQuote.BranchCode, False, sStatusFlag, True)
                        Session(CNQuote) = oQuote
                        oQuote.IsSelected = 0

                        If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
                            If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTA
                                oQuote = oWebService.UpdateRiskSelection(oQuote, nRiskIndex, oQuote.BranchCode)

                                oWebService.DeleteRisk(oQuote, e.CommandArgument, oQuote.BranchCode, "MTA", oQuote.Risks(nRiskIndex).Key)
                                'Session(CNCurrentRiskKey) = Nothing
                                'If oQuote.Risks(nRiskIndex).OriginalRiskKey = 0 Then
                                '    oQuote.Risks.Remove(nRiskIndex)
                                '    bNotTraverse = True
                                'End If
                            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTC
                                oQuote = oWebService.UpdateRiskSelection(oQuote, nRiskIndex, oQuote.BranchCode)

                                oWebService.DeleteRisk(oQuote, nRiskIndex, oQuote.BranchCode, "MTC", oQuote.Risks(nRiskIndex).Key)
                                'Session(CNCurrentRiskKey) = Nothing
                                'If oQuote.Risks(nRiskIndex).OriginalRiskKey = 0 Then
                                '    oQuote.Risks.Remove(nRiskIndex)
                                '    bNotTraverse = True
                                'End If
                            ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTR
                                oQuote = oWebService.UpdateRiskSelection(oQuote, nRiskIndex, oQuote.BranchCode)

                                oWebService.DeleteRisk(oQuote, nRiskIndex, oQuote.BranchCode, "MTR", oQuote.Risks(nRiskIndex).Key)
                                'Session(CNCurrentRiskKey) = Nothing
                                'If oQuote.Risks(nRiskIndex).OriginalRiskKey = 0 Then
                                '    oQuote.Risks.Remove(nRiskIndex)
                                '    bNotTraverse = True
                                'End If
                            End If
                        ElseIf Session(CNMTAType) Is Nothing And Session(CNRenewal) IsNot Nothing Then
                            oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.REN
                            oQuote = oWebService.UpdateRiskSelection(oQuote, nRiskIndex, oQuote.BranchCode)

                            oWebService.DeleteRisk(oQuote, nRiskIndex, oQuote.BranchCode, "REN", oQuote.Risks(nRiskIndex).Key)
                            'Session(CNCurrentRiskKey) = Nothing
                            'If oQuote.Risks(nRiskIndex).OriginalRiskKey = 0 Then
                            '    oQuote.Risks.Remove(nRiskIndex)
                            '    bNotTraverse = True
                            'End If
                        Else
                            oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.NB
                            oQuote = oWebService.UpdateRiskSelection(oQuote, nRiskIndex, oQuote.BranchCode)

                            oWebService.DeleteRisk(oQuote, nRiskIndex, oQuote.BranchCode, "NB", oQuote.Risks(nRiskIndex).Key)
                            'Session(CNCurrentRiskKey) = Nothing
                            'If oQuote.Risks(nRiskIndex).OriginalRiskKey = 0 Then
                            '    oQuote.Risks.Remove(nRiskIndex)
                            '    bNotTraverse = True
                            'End If
                        End If

                        oWebService.GetHeaderAndRisksByKey(oQuote)

                        Dim oRiskCollection As New NexusProvider.RiskCollection
                        oRiskCollection = oQuote.Risks
                        grdvRisk.DataSource = oRiskCollection
                        grdvRisk.DataBind()
                        CalculatePremium()

                        If Session(CNIsBackDatedMTA) AndAlso Not bIsInBackDatedMode Then
                            oWebService.DeleteBackDatedVersions(oQuote.InsuranceFileKey)
                        End If

                    Case "NoChange"

                        Dim nRiskIndex As Integer = CInt(e.CommandArgument)
                        Dim oRiskType As Config.RiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(nRiskIndex).RiskTypeCode.Trim)
                        Dim sStatusFlag As String = oQuote.Risks(nRiskIndex).RiskLinkStatusFlag.ToUpper()
                        Dim sScreenCode As String = Nothing
                        Dim oRiskTypeConfig As Config.RiskType
                        Dim nCurrentRiskCounter As Integer

                        sStatusFlag = IIf(Constants.CheckStatusFlags.IndexOf(sStatusFlag) = -1, Nothing, sStatusFlag)

                        oQuote = CType(Session(CNQuote), NexusProvider.Quote)

                        Session(CNMode) = Mode.Edit

                        nCurrentRiskCounter = oQuote.Risks(nRiskIndex).Key
                        Session(CNCurrentRiskKey) = nRiskIndex

                        If oQuote.Risks(nRiskIndex).XMLDataset Is Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Session(CNQuote) = oQuote
                            Dim bIgnoreLocking As Boolean = True
                            'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                            oWebService.GetRisk(nCurrentRiskCounter, nRiskIndex, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                        End If

                        If oQuote.Risks(nRiskIndex).RiskCode Is Nothing Then
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nRiskIndex).RiskTypeCode.Trim)
                        Else
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nRiskIndex).RiskCode.Trim)
                        End If
                        If oQuote.Risks(nRiskIndex).ScreenCode Is Nothing Then
                            sScreenCode = GetScreenCode(sProductFolder & "\" & oRiskTypeConfig.Path & "\" & oProduct.FullQuoteConfig)
                            oQuote.Risks(nRiskIndex).ScreenCode = sScreenCode
                        End If

                        Session(CNQuote) = oQuote

                        If Session(CNMTAType) IsNot Nothing Then
                            If Session(CNMTAType) = MTAType.CANCELLATION Then
                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTC
                            ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTR
                            ElseIf Session(CNMTAType) = MTAType.TEMPORARY Or MTAType.PERMANENT Then
                                oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTA
                            End If
                        ElseIf Session(CNRenewal) IsNot Nothing Then
                            oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.REN
                        End If

                        oWebService.UpdateRisk(oQuote, nRiskIndex, Nothing, , oQuote.TransactionType.ToString, True)
                        Session(CNQuote) = oQuote

                        'Refill the session CNQuote
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)

                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next
                        oWebService.GetHeaderAndRisksByKey(oQuote)

                        Dim oRiskCollection As New NexusProvider.RiskCollection
                        oRiskCollection = oQuote.Risks
                        If oRiskCollection.Count > 1 Then
                            oRiskCollection.SortColumn = "FolderKey"
                            oRiskCollection.SortObjectType = GetType(NexusProvider.Risk)
                            oRiskCollection.SortingOrder = SortDirection.Ascending
                            oRiskCollection.Sort()
                        End If
                        oQuote.Risks = oRiskCollection
                        Session(CNQuote) = oQuote
                        Dim sDisplayReinsurance As String
                        sDisplayReinsurance = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayReinsurance, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, CType(Session(CNQuote), NexusProvider.Quote).Risks(0).RiskTypeCode)

                        'Check the User Authority to display of Reinsurance
                        Dim oUserAuthority As New NexusProvider.UserAuthority
                        oUserAuthority.UserCode = Session(CNLoginName)
                        oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayReinsurance
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oWebService.GetUserAuthorityValue(oUserAuthority)
                        Session(CNRatingSections) = Nothing
                        If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" Then
                            Response.Redirect("~/secure/RiskDetails.aspx", False)
                        Else
                            Response.Redirect("~/secure/premiumdisplay.aspx")
                        End If
                End Select
            End If
        End Sub

        Protected Sub grdvRisk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvRisk.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim oRisk As NexusProvider.Risk = CType(e.Row.DataItem, NexusProvider.Risk)
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "../" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                Dim sProductFolderCheck As String = oNexusFrameWork.ProductsFolder + "\" + oProduct.Name

                Dim sRiskFolder As String = sProductFolder & "/" & oProduct.RiskTypes.RiskType(0).Path & "/"
                Dim sRiskFolderCheck As String = sProductFolderCheck + "\" + oProduct.RiskTypes.RiskType(0).Path & "\"

                Dim lnkbtnDelete As LinkButton = e.Row.Cells(9).FindControl("lnkbtnDelete")
                Dim lnkbtnCopy As LinkButton = e.Row.Cells(9).FindControl("lnkbtnCopy")
                Dim lnkbtnEdit As LinkButton = e.Row.Cells(9).FindControl("lnkbtnEdit")
                Dim hypDetails As HyperLink = e.Row.Cells(9).FindControl("hypDetails")
                Dim chkRiskSelect As CheckBox = e.Row.Cells(9).FindControl("chkRiskSelect")

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("RiskNumber") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Risk).RiskNumber)

                'Change the link status code to descriptive text by getting the text from the resource file.
                Dim sRiskLinkStatusFlag As String = oRisk.RiskLinkStatusFlag
                ' Display the status as Added if the status is C and the OriginalRiskKey = 0.
                If (sRiskLinkStatusFlag IsNot Nothing AndAlso sRiskLinkStatusFlag.ToUpper() = "C" AndAlso (oRisk.OriginalRiskKey = 0)) Then
                    sRiskLinkStatusFlag = sRiskLinkStatusFlag & "_Added"
                End If
                e.Row.Cells(7).Text = GetLinkStatusText(sRiskLinkStatusFlag)
                ' Show a - when the link status date is empty.
                If e.Row.Cells(8).Text = "01/01/0001" Then
                    e.Row.Cells(8).Text = "-"
                End If

                ' Set the DataItemIndex to the command argument as to get the record by index from the risk collection.
                lnkbtnEdit.CommandArgument = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)
                lnkbtnDelete.CommandArgument = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)
                lnkbtnCopy.CommandArgument = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)

                If bIsInBackDatedMode Then
                    Dim lnkbtnNoChange As LinkButton = e.Row.Cells(9).FindControl("lnkbtnNoChange")
                    lnkbtnNoChange.Visible = True
                    lnkbtnNoChange.CommandArgument = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)
                    If CType(e.Row.DataItem, NexusProvider.Risk).StatusCode.Trim.ToUpper <> "QUOTED" AndAlso CType(e.Row.DataItem, NexusProvider.Risk).StatusCode.Trim.ToUpper <> "DELETED" Then
                        lnkbtnNoChange.Enabled = True
                        lnkbtnEdit.Enabled = True
                    Else
                        lnkbtnNoChange.Enabled = False
                        If CType(e.Row.DataItem, NexusProvider.Risk).IsRiskEditableForBackDatedMTA = False Then
                            DisableLinkButton(lnkbtnEdit)
                        End If
                    End If

                End If

                'Enable/disable of the checkbox
                chkRiskSelect.ToolTip = CType(e.Row.DataItem, NexusProvider.Risk).RiskNumber
                If CType(e.Row.DataItem, NexusProvider.Risk).IsRisk = True Then
                    chkRiskSelect.Checked = True
                Else
                    chkRiskSelect.Checked = False
                End If

                'In case of cancellation and allowmultirisk=false
                If Session(CNMTAType) = MTAType.CANCELLATION Or oProduct.AllowMultiRisks = False _
                Or CType(e.Row.DataItem, NexusProvider.Risk).IsMandatoryRisk = True Then
                    'For Cancelled Policy "Copy" and "Delete" options will not ne available
                    lnkbtnDelete.Visible = False
                    lnkbtnCopy.Visible = False
                Else

                    Dim nQuotedRiskCount As Integer = 0
                    Dim nRiskCount As Integer = 0
                    For nRiskCount = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(nRiskCount).StatusCode <> "DELETED" Then
                            nQuotedRiskCount = nQuotedRiskCount + 1
                        End If
                    Next

                    If nQuotedRiskCount > 1 Then
                        lnkbtnDelete.CommandName = "Delete"
                        If CType(e.Row.DataItem, NexusProvider.Risk).HasClaimLink Then
                            lnkbtnDelete.Attributes.Add("onclick", "javascript:return DeleteClaimRiskConfirmation();")
                        Else
                            lnkbtnDelete.Attributes.Add("onclick", "javascript:return DeleteConfirmation();")
                        End If
                    Else
                        'if only 1 record than delete should be disabled
                        'lnkbtnDelete.Enabled = False
                        DisableLinkButton(lnkbtnDelete)
                    End If

                    lnkbtnCopy.CommandName = "Copy"
                    Session(CNCurrentRiskKey) = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)
                    If HttpContext.Current.Session.IsCookieless Then
                        lnkbtnCopy.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/modal/CopyRiskTypeSelect.aspx?PostbackTo=" & UpdMultiRisk.ClientID.ToString & "&RiskNumber=" & CType(e.Row.DataItem, NexusProvider.Risk).RiskNumber & "&RiskKey=" & CType(e.Row.DataItem, NexusProvider.Risk).Key & "&RiskIndex=" & lnkbtnCopy.CommandArgument & "&modal=true&KeepThis=true&FromPage=MultiRisk1&TB_iframe=true&height=500&width=700' , null);return false;"
                    Else
                        lnkbtnCopy.OnClientClick = "tb_show(null , '../modal/CopyRiskTypeSelect.aspx?PostbackTo=" & UpdMultiRisk.ClientID.ToString & "&RiskNumber=" & CType(e.Row.DataItem, NexusProvider.Risk).RiskNumber & "&RiskKey=" & CType(e.Row.DataItem, NexusProvider.Risk).Key & "&RiskIndex=" & lnkbtnCopy.CommandArgument & "&modal=true&KeepThis=true&FromPage=MultiRisk1&TB_iframe=true&height=500&width=700' , null);return false;"
                    End If

                End If
                If CType(e.Row.DataItem, NexusProvider.Risk).StatusCode IsNot Nothing Then
                    'if risk is deleted during MTA then no link should be there
                    If CType(e.Row.DataItem, NexusProvider.Risk).StatusCode.Trim.ToUpper = "DELETED" Then
                        hypDetails.Enabled = False
                        DisableLinkButton(lnkbtnCopy)
                        DisableLinkButton(lnkbtnDelete)
                        If Session(CNMode) = Mode.View Then
                            lnkbtnEdit.Text = GetLocalResourceObject("lbl_View").ToString
                        End If
                    End If

                    If CType(e.Row.DataItem, NexusProvider.Risk).StatusCode.Trim.ToUpper = "QUOTED" OrElse CType(e.Row.DataItem, NexusProvider.Risk).StatusCode.Trim.ToUpper = "RIDEFERRED" _
                    AndAlso CType(e.Row.DataItem, NexusProvider.Risk).IsMandatoryRisk = False Then
                        If Session(CNMode) = Mode.View Then
                            'in VIEW mode these options should NOT enable
                            chkRiskSelect.Enabled = False
                            lnkbtnEdit.Text = GetLocalResourceObject("lbl_View").ToString
                            DisableLinkButton(lnkbtnCopy)
                            DisableLinkButton(lnkbtnDelete)
                        End If
                    Else
                        If CType(e.Row.DataItem, NexusProvider.Risk).IsMandatoryRisk = True Then
                            If Session(CNMode) = Mode.View Then
                                lnkbtnEdit.Text = GetLocalResourceObject("lbl_View").ToString
                                DisableLinkButton(lnkbtnCopy)
                                DisableLinkButton(lnkbtnDelete)
                            Else
                                If oQuote.Risks.Count = 1 Then
                                    DisableLinkButton(lnkbtnEdit)
                                End If
                            End If
                        End If
                        chkRiskSelect.Enabled = False
                    End If
                Else
                    chkRiskSelect.Enabled = False
                End If

                'In case of cancellation and allowmultirisk=false
                If Session(CNMTAType) = MTAType.CANCELLATION Or oProduct.AllowMultiRisks = False Then
                    'For Cancelled Policy "Copy" and "Delete" options will not ne available
                    lnkbtnDelete.Visible = False
                    lnkbtnCopy.Visible = False
                Else
                    Dim nQuotedRiskCount As Integer = 0
                    Dim nRiskCount As Integer = 0
                    For nRiskCount = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(nRiskCount).StatusCode <> "DELETED" Then
                            nQuotedRiskCount = nQuotedRiskCount + 1
                        End If
                    Next
                    If nQuotedRiskCount > 1 Then
                        lnkbtnDelete.CommandName = "Delete"

                        'Add pagesize so that it hold correct index when paging is on
                        lnkbtnDelete.CommandArgument = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)
                        lnkbtnDelete.Attributes.Add("onclick", "javascript:return DeleteConfirmation();")
                    Else
                        lnkbtnDelete.Enabled = False 'if only 1 record than delete should be disabled
                    End If

                    lnkbtnCopy.CommandName = "Copy"

                    'Add pagesize so that it hold correct index when paging is on
                    lnkbtnCopy.CommandArgument = e.Row.RowIndex + (grdvRisk.PageIndex * grdvRisk.PageSize)
                    If HttpContext.Current.Session.IsCookieless Then
                        lnkbtnCopy.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/modal/CopyRiskTypeSelect.aspx?PostbackTo=" & UpdMultiRisk.ClientID.ToString & "&RiskNumber=" & CType(e.Row.DataItem, NexusProvider.Risk).RiskNumber & "&RiskKey=" & CType(e.Row.DataItem, NexusProvider.Risk).Key & "&RiskIndex=" & e.Row.RowIndex & "&modal=true&KeepThis=true&FromPage=MultiRisk1&TB_iframe=true&height=500&width=700' , null);return false;"
                    Else
                        lnkbtnCopy.OnClientClick = "tb_show(null , '../modal/CopyRiskTypeSelect.aspx?PostbackTo=" & UpdMultiRisk.ClientID.ToString & "&RiskNumber=" & CType(e.Row.DataItem, NexusProvider.Risk).RiskNumber & "&RiskKey=" & CType(e.Row.DataItem, NexusProvider.Risk).Key & "&RiskIndex=" & lnkbtnCopy.CommandArgument & "&modal=true&KeepThis=true&FromPage=MultiRisk1&TB_iframe=true&height=500&width=700' , null);return false;"
                    End If
                End If
                'end


                'Details Link
                Dim FileSystem As System.IO.FileInfo = New System.IO.FileInfo(Request.PhysicalApplicationPath + sRiskFolderCheck + "SummaryOfRisk.aspx")
                Dim oItem As NexusProvider.Risk = CType(e.Row.DataItem, NexusProvider.Risk)

                If FileSystem.Exists Then
                    hypDetails.Attributes.Add("onclick", "tb_show(null ,'" & sRiskFolder + "SummaryOfRisk.aspx?Riskkey=" + oItem.Key.ToString() + "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;")
                    hypDetails.Visible = True
                Else
                    hypDetails.Visible = False
                End If

                'This code is added for unmarking the quote for collection
                If oQuote.MarkedQuoteForCollection Then
                    lnkbtnEdit.Attributes.Add("OnClick", "javascript:return UnMarkedQuoteConfirmation();")
                End If

                'During MTA:: You cannot unselect a Risk that is already part of the policy, so making those checkboxes always Checked and showing error message just like BO
                If CType(e.Row.DataItem, NexusProvider.Risk).OriginalRiskKey > 0 Or CType(e.Row.DataItem, NexusProvider.Risk).RiskLinkStatusFlag = "U" Then
                    Dim sErrorMessage As String = GetLocalResourceObject("msg_RiskIsPartOfPolicy")
                    chkRiskSelect.Attributes.Add("OnClick", "javascript:RiskIsPartOfPolicy('" + sErrorMessage + "');return false;")
                End If


                If bIsInBackDatedMode = True Then
                    If Session(CNMode) = Mode.View Then
                        lnkbtnEdit.Text = GetLocalResourceObject("lbl_View").ToString
                    End If
                    DisableLinkButton(lnkbtnCopy)
                    DisableLinkButton(lnkbtnDelete)
                End If

                If Session(CNMode) = Mode.View Then
                    'in VIEW mode these options should NOT enable
                    chkRiskSelect.Enabled = False
                    lnkbtnEdit.Text = GetLocalResourceObject("lbl_View").ToString
                    DisableLinkButton(lnkbtnCopy)
                    DisableLinkButton(lnkbtnDelete)
                End If


                Dim lblGridTempRiskNumber As Label = CType(e.Row.FindControl("lblTempRiskNumber"), Label)
                If (lblGridTempRiskNumber IsNot Nothing) Then
                    lblGridTempRiskNumber.Text = e.Row.DataItemIndex + 1
                End If
            End If
        End Sub
        ''' <summary>
        ''' Clear postback on click because Chrome doesn't disable the actual control.
        ''' </summary>
        ''' <param name="tag">ControlId</param>
        ''' <remarks></remarks>
        Private Sub DisableLinkButton(ByVal tag As LinkButton)

            tag.Enabled = False
            tag.PostBackUrl = "#"
            tag.OnClientClick = ""
            tag.Attributes.Remove("onclick")

        End Sub

        ''' <summary>
        ''' Change the status code to descriptive text by getting the text from the resource file.
        ''' </summary>
        ''' <param name="code">Link Status Code</param>
        ''' <returns>Link Status Description from the resource file</returns>
        ''' <remarks>Returns - if code not found.</remarks>
        Private Function GetLinkStatusText(ByVal code As String) As String

            Dim description As String = GetLocalResourceObject("lbl_ChangedStatus_" & code)
            If description Is Nothing Then
                description = "-"
            End If
            GetLinkStatusText = description

        End Function
        ''' <summary>
        ''' Fires bellow event when Row editing of grid starts
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvRisk_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdvRisk.RowEditing

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sRedirectPath As String = String.Empty
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name

            For iCounter As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks(iCounter).Key = CInt(grdvRisk.Rows(e.NewEditIndex).Cells(5).Text.Trim) Then
                    Session(CNCurrentRiskKey) = iCounter
                    Exit For
                End If
            Next

            If oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset Is Nothing Then
                oWebService.GetHeaderAndRisksByKey(oQuote)
                Session(CNQuote) = oQuote
                Dim bIgnoreLocking As Boolean = True
                'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                oWebService.GetRisk(grdvRisk.Rows(e.NewEditIndex).Cells(5).Text, Session(CNCurrentRiskKey), oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                Session(CNQuote) = oQuote
            End If
            Dim oRiskType As Config.RiskType
            If oQuote.Risks.Count > 0 Then
                If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                End If
            End If

            Dim oRisk As New NexusProvider.RiskType
            oRisk.DataModelCode = oRiskType.DataModelCode
            oRisk.Name = oRiskType.Name
            oRisk.Path = oRiskType.Path
            oRisk.RiskCode = oRiskType.RiskCode
            Session(CNRiskType) = oRisk

            Dim sRiskFolder As String = sProductFolder & "/" & oRisk.Path & "/"
            Dim bMaindetails As String = String.Empty
            Dim sPage As String = GetFirstRiskScreen(sProductFolder & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" & oProduct.FullQuoteConfig, bMaindetails)

            If Session(CNMode) = Mode.Edit Then
                If oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode.Trim.ToUpper = "DELETED" Then
                    Session(CNMode) = Mode.DeleteRisk
                Else
                    If oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.UNQUOTED.ToString() AndAlso oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.REFERRED.ToString() AndAlso
                    oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode.Trim.ToUpper <> NexusProvider.RiskStatusType.DECLINED.ToString() Then
                        oWebService.UpdateRiskStatus(0, oQuote.Risks(Session(CNCurrentRiskKey)).Key, NexusProvider.RiskStatusType.UNQUOTED, oQuote.BranchCode, o_nTimeStamp:=oQuote.TimeStamp)
                        oQuote.Risks(Session(CNCurrentRiskKey)).StatusCode = NexusProvider.RiskStatusType.UNQUOTED.ToString()
                        oQuote.Risks(Session(CNCurrentRiskKey)).StatusDescription = "Unquoted"
                        Session(CNQuote) = oQuote
                    End If
                    Session(CNMode) = Mode.Edit
                    EditDefaultRule()
                End If
            End If
            Session(CNDataModelCode) = oRisk.DataModelCode
            Session.Remove(CNTabState & sCtrlTabIndexControlID)

            If String.IsNullOrEmpty(bMaindetails) = False AndAlso bMaindetails.ToLower = "false" Then
                Response.Redirect(sRiskFolder & sPage, False)
            Else
                Response.Redirect(sRiskFolder & "/" & sPage, False)
            End If
        End Sub

        Sub EditDefaultRule()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"
            Dim sScreenCode As String = Nothing
            Dim oRiskType As Config.RiskType
            If oQuote.Risks.Count > 0 Then
                If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                End If
            End If
            If oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode IsNot Nothing Then
                If oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode.Trim.Length <> 0 Then
                    sScreenCode = oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode
                Else
                    sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                End If
            Else
                sScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
            End If
            If oQuote IsNot Nothing AndAlso Session(CNCurrentRiskKey) IsNot Nothing AndAlso oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode Is Nothing Then
                oQuote.Risks(Session(CNCurrentRiskKey)).ScreenCode = sScreenCode
            End If
            If Session(CNMTAType) IsNot Nothing And Session(CNRenewal) Is Nothing Then
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTA")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTC")
                ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then
                    oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "MTR")
                End If
            ElseIf (Not String.IsNullOrEmpty(oQuote.InsuranceFileTypeCode) AndAlso oQuote.InsuranceFileTypeCode.Trim.ToUpper = "RENEWAL") OrElse (Session(CNMTAType) Is Nothing And (Session(CNRenewal) IsNot Nothing)) Then
                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "REN")
            Else
                oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset = oWebService.RunDefaultRulesEdit(sScreenCode, oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset, Nothing, Nothing, "NB")
            End If
        End Sub

        Sub CalculatePremium(Optional ByVal bCalledForGrid As Boolean = False, Optional ByVal nCurrentRiskNumber As Integer = -1)

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim olblTotalPremium As Label = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("lblPremiumValue"), Label)
            Dim oSummaryCtrl As UserControl = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("SummaryCoverCntrl"), UserControl)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            If Session(CNMode) <> Mode.View AndAlso bCalledForGrid = True Then
                For iTempVar As Integer = 0 To grdvRisk.Rows.Count - 1
                    Dim chkSelected As CheckBox
                    chkSelected = DirectCast(grdvRisk.Rows(iTempVar).FindControl("chkRiskSelect"), CheckBox)
                    If chkSelected.Checked = False Then
                        For jCount As Integer = 0 To oQuote.Risks.Count - 1
                            If (nCurrentRiskNumber = -1 OrElse oQuote.Risks(jCount).RiskNumber = nCurrentRiskNumber) AndAlso oQuote.Risks(jCount).Key.ToString = grdvRisk.Rows(iTempVar).Cells(5).Text.Trim Then
                                oQuote.IsSelected = 0
                                'PN 63149 - Agent Commission is not retrieved based on Transaction Type
                                If Session(CNMTAType) IsNot Nothing Then
                                    If Session(CNMTAType) = MTAType.CANCELLATION Then
                                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTC
                                    ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTR
                                    ElseIf Session(CNMTAType) = MTAType.TEMPORARY Or MTAType.PERMANENT Then
                                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTA
                                    End If
                                ElseIf Session(CNRenewal) IsNot Nothing Then
                                    oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.REN
                                End If
                                oQuote = oWebService.UpdateRiskSelection(oQuote, jCount, oQuote.BranchCode)
                                oQuote.Risks(jCount).IsRiskSelected = False
                            End If
                        Next
                    Else
                        For jCount As Integer = 0 To oQuote.Risks.Count - 1
                            If (nCurrentRiskNumber = -1 OrElse oQuote.Risks(jCount).RiskNumber = nCurrentRiskNumber) AndAlso oQuote.Risks(jCount).Key.ToString = grdvRisk.Rows(iTempVar).Cells(5).Text.Trim Then
                                'If UpdateRiskSelection will be called for unchanged risk then it will effect the original risk which may be pointing to any other risk.
                                'So do'nt call this for unchanged risks
                                If oQuote.Risks(jCount).RiskLinkStatusFlag <> "U" AndAlso oQuote.Risks(jCount).RiskLinkStatusFlag <> "R" Then
                                    oQuote.IsSelected = 1
                                    'PN 63149 - Agent Commission is not retrieved based on Transaction Type
                                    If Session(CNMTAType) IsNot Nothing Then
                                        If Session(CNMTAType) = MTAType.CANCELLATION Then
                                            oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTC
                                        ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                                            oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTR
                                        ElseIf Session(CNMTAType) = MTAType.TEMPORARY Or MTAType.PERMANENT Then
                                            oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTA
                                        End If
                                    ElseIf Session(CNRenewal) IsNot Nothing Then
                                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.REN
                                    End If
                                    oQuote = oWebService.UpdateRiskSelection(oQuote, jCount, oQuote.BranchCode)
                                    oQuote.Risks(jCount).IsRiskSelected = True
                                End If
                            End If
                        Next
                    End If
                Next

                oWebService.GetHeaderAndRisksByKey(oQuote)

                UpdatePremiumWithAgentCommision()

                grdvRisk.DataSource = oQuote.Risks
                grdvRisk.DataBind()
            Else
                If Session("SelectedPageIndex") Is Nothing Then
                    Dim nGridPageIndex As Integer = grdvRisk.PageIndex
                    Dim nRiskCnt As Integer = 0
                    For nIndex As Integer = 0 To grdvRisk.PageCount - 1
                        grdvRisk.SetPageIndex(nIndex)
                        For iTempVar As Integer = 0 To grdvRisk.Rows.Count - 1
                            Dim chkSelected As CheckBox
                            chkSelected = DirectCast(grdvRisk.Rows(iTempVar).FindControl("chkRiskSelect"), CheckBox)
                            If chkSelected.Checked = True Then
                                oQuote.Risks(nRiskCnt).IsRiskSelected = True
                            End If
                            nRiskCnt = nRiskCnt + 1
                        Next
                    Next
                Else
                    grdvRisk.SetPageIndex(Convert.ToInt32(Session("SelectedPageIndex")))
                    Session.Remove("SelectedPageIndex")
                End If
            End If


            'Enable Mark For Collection Button
            If IsDebitOrderProcessingEnabled() = False Then
                EnableMarkForCollection()
            End If

            Dim m_cRoundOffAmount As Decimal = CheckAndCalculateRoundOff()
            olblTotalPremium.Text = New Money(m_cRoundOffAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
            Session(CNQuote) = oQuote

        End Sub
        Sub EnableMarkForCollection()
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            'checking of the risk for MarkForCollection, if risk is selected and if it is quoted 
            'and oQuote.MarkedQuoteForCollection=False then show the markforcollection button otherwise not
            'Added another condition ignore QuickQuote Mode to show visiblity of Mark Quote button PN65580
            'Register the marking of Quote and make visiblity of Mark Quote button
            Dim btnMarkQuoteForCollection As UI.WebControls.LinkButton
            btnMarkQuoteForCollection = CType(CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnMarkQuoteForCollection"), LinkButton)
            If Session(CNMode) <> Mode.View AndAlso Session(CNQuoteMode) <> QuoteMode.QuickQuote AndAlso UserCanDoTask("MarkQuote") _
            AndAlso oQuote.MarkedQuoteForCollection = False AndAlso oQuote.GrossTotal > 0.0 Then
                'Check Quote Status when we are coming through Quick Quote and 
                'directly go to buy from there then it should check FullQuote all mandatory fields has been filled
                'also make visible in Renewal case and MTA Quote case as this are alreayd processsed once
                Dim bFound As Boolean = False
                If oQuote.Risks IsNot Nothing Then
                    For iCount As Integer = 0 To oQuote.Risks.Count - 1
                        If oQuote.Risks(iCount).IsRiskSelected = True Or oQuote.Risks(iCount).IsRisk = True Then
                            If (Session(CNQuoteMode) = QuoteMode.FullQuote _
                              AndAlso oQuote.Risks(iCount).PremiumDueGross > 0 _
                              AndAlso Session(CNRenewal) Is Nothing) Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                              OrElse Session(CNRenewal) IsNot Nothing Then
                                If oQuote.Risks(iCount).StatusCode = "QUOTED" OrElse
                                     oQuote.Risks(iCount).StatusCode = "DELETED" Then
                                    bFound = True
                                ElseIf IsDataSetNexusQuoteStatus(iCount) = True AndAlso NexusQuoteStatus(oQuote.Risks(iCount)) = True Then
                                    'NEXUSQS is applied and is in Full Quote
                                    bFound = True
                                ElseIf IsDataSetNexusQuoteStatus(iCount) = True AndAlso NexusQuoteStatus(oQuote.Risks(iCount)) = False Then
                                    'NEXUSQS is applied and is in Quick Quote
                                    bFound = False
                                    Exit For
                                End If
                            Else
                                bFound = False
                                Exit For
                            End If
                        End If
                    Next

                    If bFound = True Then
                        If Session(CNRenewal) Is Nothing And Session(CNMTAType) Is Nothing Or (Session(CNMTAType) IsNot Nothing And Session(CNMTAType) <> MTAType.CANCELLATION) Then
                            btnMarkQuoteForCollection.Visible = True
                            btnMarkQuoteForCollection.Attributes.Add("OnClick", "javascript:return MarkedConfirmation();")
                        ElseIf Session(CNRenewal) Then
                            Dim oRenewalStatus As NexusProvider.RenewalStatus
                            oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                            If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice _
                            Or oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                                btnMarkQuoteForCollection.Visible = False
                            ElseIf oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Update Then
                                btnMarkQuoteForCollection.Visible = True
                            End If
                        Else
                            btnMarkQuoteForCollection.Visible = False
                        End If
                    Else
                        btnMarkQuoteForCollection.Visible = False
                    End If
                Else
                    'if NexusQs field doesnt exist then markQuote button should be visible
                    If Session(CNRenewal) Is Nothing And Session(CNMTAType) Is Nothing Or (Session(CNMTAType) IsNot Nothing And Session(CNMTAType) <> MTAType.CANCELLATION) Then
                        btnMarkQuoteForCollection.Visible = True
                        btnMarkQuoteForCollection.Attributes.Add("OnClick", "javascript:return MarkedConfirmation();")
                    ElseIf Session(CNRenewal) Then
                        Dim oRenewalStatus As NexusProvider.RenewalStatus
                        oRenewalStatus = oWebService.GetRenewalStatus(oQuote.InsuranceFileKey)
                        If oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Renewal_Notice _
                        Or oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Manual_Preview Then
                            btnMarkQuoteForCollection.Visible = False
                        ElseIf oRenewalStatus.RenewalStatusTypeDescription = sAwaiting_Update Then
                            btnMarkQuoteForCollection.Visible = True
                        End If
                    Else
                        btnMarkQuoteForCollection.Visible = False
                    End If
                End If
            Else
                btnMarkQuoteForCollection.Visible = False
            End If

        End Sub
        Protected Sub chkRiskSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            If IsPostBack Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim iRiskNumber As Integer = Integer.Parse(CType(sender, CheckBox).ToolTip.Split(",")(0))

                ' Get the current page index of the GridView
                Dim grdvPageIndex As Integer = grdvRisk.PageIndex
                Session.Add("SelectedPageIndex", grdvPageIndex)

                'If risk number is same (comparative)
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If CType(sender, CheckBox).Checked = True Then
                        If oQuote.Risks(iCount).RiskNumber = iRiskNumber And oQuote.Risks(iCount).IsRisk = True Then
                            vldCompQuote.IsValid = False
                            CType(sender, CheckBox).Checked = False
                            Exit Sub
                        Else
                            vldCompQuote.IsValid = True
                        End If
                    End If
                Next
                CalculatePremium(True, iRiskNumber)
                Response.Redirect(Request.RawUrl)
            End If
        End Sub

        Protected Sub grdvRisk_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvRisk.PageIndexChanging

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Me.grdvRisk.PageIndex = e.NewPageIndex
            Me.grdvRisk.DataSource = oQuote.Risks
            Me.grdvRisk.DataBind()

        End Sub



        Private Sub SetupButtons(ByRef oQuote As NexusProvider.Quote)
            If oQuote.IsMarketPlacePolicy Then
                btnAddRisk.Visible = False
            End If
        End Sub

        Protected Sub btnQuoteAll_Click(sender As Object, e As EventArgs) Handles btnQuoteAll.Click
            QuoteAllRisk()
            Session(CNQuoteAllProcessComplete) = True
        End Sub


        Public Sub QuoteAllRisk()
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID().ToString()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim sScreenCode As String = Nothing
            Dim oRiskTypeConfig As Config.RiskType
            Dim nCurrentRiskCounter As Integer
            Dim bFacRiskExist As Boolean = False

            If Session(CNQuote) IsNot Nothing Then
                oQuote = CType(Session(CNQuote), NexusProvider.Quote)
            Else
                Exit Sub
            End If

            Session(CNMode) = Mode.Edit
            Try

                ' Remove paging to Quote all the Risk.
                grdvRisk.AllowPaging = False
                grdvRisk.DataSource = oQuote.Risks
                grdvRisk.DataBind()

                For Each row As GridViewRow In grdvRisk.Rows
                    If row.Cells(6).Text.ToUpper.Trim() = "UNQUOTED" Then
                        For nCounter As Integer = 0 To oQuote.Risks.Count - 1
                            'Case when session is NB and risk is non-mandatory
                            If Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing AndAlso oQuote.Risks(nCounter).Key = CInt(row.Cells(5).Text.Trim) Then
                                Session(CNCurrentRiskKey) = nCounter
                                nCurrentRiskCounter = nCounter
                                Exit For
                            ElseIf oQuote.Risks(nCounter).Key = CInt(row.Cells(5).Text.Trim) AndAlso oQuote.Risks(nCounter).IsMandatoryRisk = False Then
                                Session(CNCurrentRiskKey) = nCounter
                                nCurrentRiskCounter = nCounter
                                Exit For
                            End If
                        Next

                        If oQuote.Risks(nCurrentRiskCounter).XMLDataset Is Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Session(CNQuote) = oQuote
                            Dim bIgnoreLocking As Boolean = True
                            'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                            oWebService.GetRisk(CInt(row.Cells(5).Text), nCurrentRiskCounter, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                        End If

                        If oQuote.Risks(nCurrentRiskCounter).RiskCode Is Nothing Then
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskCounter).RiskTypeCode.Trim)
                        Else
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskCounter).RiskCode.Trim)
                        End If
                        If oQuote.Risks(nCurrentRiskCounter).ScreenCode Is Nothing Then
                            sScreenCode = GetScreenCode(sProductFolder & "\" & oRiskTypeConfig.Path & "\" & oProduct.FullQuoteConfig)
                            oQuote.Risks(nCurrentRiskCounter).ScreenCode = sScreenCode
                        End If

                        Session(CNQuote) = oQuote
                        If oQuote.Risks(nCurrentRiskCounter).IsAutoRated AndAlso oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk = False Then
                            If Session(CNMTAType) = MTAType.CANCELLATION Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTC")
                                Session(CNQuote) = oQuote
                            ElseIf Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.PERMANENT Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTA")
                                Session(CNQuote) = oQuote
                            ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTR")
                                Session(CNQuote) = oQuote
                            ElseIf Session(CNRenewal) IsNot Nothing Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "REN")
                                Session(CNQuote) = oQuote
                            ElseIf oQuote.InsuranceFileTypeCode.Trim().ToUpper = "QUOTE" Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "NB")
                                Session(CNQuote) = oQuote
                            End If

                            'Refill the session CNQuote
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)

                            ' If the risk in session is updated check if it has FacProp to update it further to Review Fac.
                            If oQuote.Risks(nCurrentRiskCounter).HasFacProp Then
                                oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oQuote.Risks(nCurrentRiskCounter).Key,
                                                             NexusProvider.RiskStatusType.REVIEWFAC, oQuote.BranchCode)
                                bFacRiskExist = True
                            End If
                        ElseIf oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "QUOTE" AndAlso IsMandatoryStatusToUpdate(nCurrentRiskCounter, oQuote) Then
                            oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "NB")
                            Session(CNQuote) = oQuote
                        End If

                        oWebService.GetHeaderAndRisksByKey(oQuote)

                        Session(CNQuote) = oQuote

                        btnQuoteAll.Enabled = False

                    End If
                Next

                For Each row As GridViewRow In grdvRisk.Rows
                    If row.Cells(6).Text.ToUpper.Trim() = "UNQUOTED" Then
                        For nCounter As Integer = 0 To oQuote.Risks.Count - 1
                            If oQuote.Risks(nCounter).Key = CInt(row.Cells(5).Text.Trim) AndAlso oQuote.Risks(nCounter).IsMandatoryRisk = True Then
                                Session(CNCurrentRiskKey) = nCounter
                                nCurrentRiskCounter = nCounter
                                Exit For
                            End If
                        Next

                        If oQuote.Risks(nCurrentRiskCounter).XMLDataset Is Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Session(CNQuote) = oQuote
                            Dim bIgnoreLocking As Boolean = True
                            'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                            oWebService.GetRisk(CInt(row.Cells(5).Text), nCurrentRiskCounter, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                        End If

                        If oQuote.Risks(nCurrentRiskCounter).RiskCode Is Nothing Then
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskCounter).RiskTypeCode.Trim)
                        Else
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskCounter).RiskCode.Trim)
                        End If
                        If oQuote.Risks(nCurrentRiskCounter).ScreenCode Is Nothing Then
                            sScreenCode = GetScreenCode(sProductFolder & "\" & oRiskTypeConfig.Path & "\" & oProduct.FullQuoteConfig)
                            oQuote.Risks(nCurrentRiskCounter).ScreenCode = sScreenCode
                        End If

                        Session(CNQuote) = oQuote
                        If oQuote.Risks(nCurrentRiskCounter).IsAutoRated AndAlso oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk = True AndAlso bFacRiskExist = False Then
                            If Session(CNMTAType) = MTAType.CANCELLATION Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTC")
                                Session(CNQuote) = oQuote
                            ElseIf Session(CNMTAType) = MTAType.TEMPORARY Or Session(CNMTAType) = MTAType.PERMANENT Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTA")
                                Session(CNQuote) = oQuote
                            ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTR")
                                Session(CNQuote) = oQuote
                            ElseIf Session(CNRenewal) IsNot Nothing Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "REN")
                                Session(CNQuote) = oQuote
                            ElseIf oQuote.InsuranceFileTypeCode.Trim().ToUpper = "QUOTE" Then
                                oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "NB")
                                Session(CNQuote) = oQuote
                            End If

                            'Refill the session CNQuote
                            oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)

                            ' If the risk in session is updated check if it has FacProp to update it further to Review Fac.
                            If bFacRiskExist = True Then
                                oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oQuote.Risks(nCurrentRiskCounter).Key,
                                                                 NexusProvider.RiskStatusType.UNQUOTED, oQuote.BranchCode)
                            ElseIf oQuote.Risks(nCurrentRiskCounter).HasFacProp Then
                                oWebService.UpdateRiskStatus(oQuote.InsuranceFileKey, oQuote.Risks(nCurrentRiskCounter).Key,
                                                                 NexusProvider.RiskStatusType.REVIEWFAC, oQuote.BranchCode)
                            End If

                        End If


                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next

                        oWebService.GetHeaderAndRisksByKey(oQuote)

                        Session(CNQuote) = oQuote

                        btnQuoteAll.Enabled = False

                    End If
                    'exit because there will be only on mandatory risk 
                    Exit For
                Next
                If oQuote.Risks(nCurrentRiskCounter).RiskCode Is Nothing Then
                    oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskCounter).RiskTypeCode.Trim)
                Else
                    oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskCounter).RiskCode.Trim)
                End If
                If oQuote.Risks(nCurrentRiskCounter).ScreenCode Is Nothing Then
                    sScreenCode = GetScreenCode(sProductFolder & "\" & oRiskTypeConfig.Path & "\" & oProduct.FullQuoteConfig)
                    oQuote.Risks(nCurrentRiskCounter).ScreenCode = sScreenCode
                End If
                If oQuote.Risks(0).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "QUOTE" AndAlso IsMandatoryStatusToUpdate(nCurrentRiskCounter, oQuote) Then
                    oWebService.UpdateRisk(oQuote, 0, Nothing, , "NB")
                    Session(CNQuote) = oQuote
                ElseIf oQuote.Risks(0).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQUOTE" AndAlso IsMandatoryStatusToUpdate(nCurrentRiskCounter, oQuote) Then
                    oWebService.UpdateRisk(oQuote, 0, Nothing, , "MTA")
                    Session(CNQuote) = oQuote
                ElseIf oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQCAN" AndAlso IsMandatoryStatusToUpdate(nCurrentRiskCounter, oQuote) Then
                    oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTC")
                    Session(CNQuote) = oQuote
                ElseIf oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "MTAQREINS" AndAlso IsMandatoryStatusToUpdate(nCurrentRiskCounter, oQuote) Then
                    oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "MTR")
                    Session(CNQuote) = oQuote
                ElseIf oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk AndAlso oQuote.InsuranceFileTypeCode.Trim().ToUpper = "RENEWAL" AndAlso IsMandatoryStatusToUpdate(nCurrentRiskCounter, oQuote) Then
                    oWebService.UpdateRisk(oQuote, nCurrentRiskCounter, Nothing, , "REN")
                    Session(CNQuote) = oQuote
                End If
            Catch ex As NexusProvider.NexusException
                Select Case CType(ex.Errors(0), NexusProvider.NexusError).Code
                    Case "275"
                        Dim sMessage As String = "alert('" + ex.Errors(0).Description + ".\n" + ex.Errors(0).Detail + "')"
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "claimlocked", sMessage, True)
                        Server.ClearError()
                        Exit Sub
                    Case Else
                        Throw ex
                End Select
            Finally
            End Try
            Response.Redirect("~/secure/Premiumdisplay.aspx", False)

        End Sub
        ''' <summary>
        ''' This will check debit order processing
        ''' </summary>
        ''' <remarks></remarks>
        Private Function IsDebitOrderProcessingEnabled() As Boolean

            If ViewState("DebitOrderOptionType") Is Nothing Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oDebitOrderOptionType As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oDebitOrderOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 105)
                ViewState.Add("DebitOrderOptionType", oDebitOrderOptionType.OptionValue)
            End If

            If ViewState("DebitOrderOptionType") = "1" Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' btnNoChangeAll_Click: Thi button will try to quote all the unquoted risks.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNoChangeAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoChangeAll.Click

            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oQuote As NexusProvider.Quote
            oQuote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID().ToString()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Dim sScreenCode As String = Nothing
            Dim oRiskTypeConfig As Config.RiskType
            Dim nCurrentRiskCounter As Integer
            Dim nMandatoryRiskKeyIndex As Integer
            Dim nMandatoryRiskKey As Integer
            Try

                Session(CNMode) = Mode.Edit

                If Session(CNMTAType) IsNot Nothing Then
                    If Session(CNMTAType) = MTAType.CANCELLATION Then
                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTC
                    ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTR
                    ElseIf Session(CNMTAType) = MTAType.TEMPORARY Or MTAType.PERMANENT Then
                        oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.MTA
                    End If
                ElseIf Session(CNRenewal) IsNot Nothing Then
                    oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.REN
                End If

                For iCnt As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCnt).IsMandatoryRisk Then
                        nMandatoryRiskKeyIndex = iCnt
                        nMandatoryRiskKey = oQuote.Risks(iCnt).Key
                    Else
                        If oQuote.Risks(iCnt).StatusCode.ToUpper.Trim <> "QUOTED" AndAlso oQuote.Risks(iCnt).StatusCode.ToUpper.Trim <> "DELETED" Then

                            Session(CNCurrentRiskKey) = iCnt
                            nCurrentRiskCounter = oQuote.Risks(iCnt).Key
                            If oQuote.Risks(iCnt).XMLDataset Is Nothing Then
                                oWebService.GetHeaderAndRisksByKey(oQuote)
                                Session(CNQuote) = oQuote
                                Dim bIgnoreLocking As Boolean = True
                                'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                                oWebService.GetRisk(nCurrentRiskCounter, iCnt, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                            End If

                            If oQuote.Risks(iCnt).RiskCode Is Nothing Then
                                oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(iCnt).RiskTypeCode.Trim)
                            Else
                                oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(iCnt).RiskCode.Trim)
                            End If
                            If oQuote.Risks(iCnt).ScreenCode Is Nothing Then
                                sScreenCode = GetScreenCode(sProductFolder & "\" & oRiskTypeConfig.Path & "\" & oProduct.FullQuoteConfig)
                                oQuote.Risks(iCnt).ScreenCode = sScreenCode
                            End If
                            Session(CNQuote) = oQuote
                            oWebService.UpdateRisk(oQuote, iCnt, Nothing, , oQuote.TransactionType.ToString, True)
                            Session(CNQuote) = oQuote

                        End If
                    End If
                Next

                If nMandatoryRiskKey <> 0 Then

                    Dim oUpdatedQuote As New NexusProvider.Quote
                    Dim bAllRiskQuoted As Boolean = True
                    oUpdatedQuote = CType(Session(CNQuote), NexusProvider.Quote)
                    oUpdatedQuote = oWebService.GetHeaderAndSummariesByKey(oUpdatedQuote.InsuranceFileKey)

                    For i As Integer = 0 To oUpdatedQuote.Risks.Count - 1
                        If nMandatoryRiskKeyIndex <> i Then
                            oWebService.GetRisk(oUpdatedQuote.Risks(i).Key, i, oUpdatedQuote, oUpdatedQuote.BranchCode, True)
                        End If
                    Next
                    oWebService.GetHeaderAndRisksByKey(oUpdatedQuote)

                    Dim oRiskCollection As New NexusProvider.RiskCollection
                    oRiskCollection = oUpdatedQuote.Risks

                    Dim oRisk = From oRiskData In oRiskCollection
                                Where (oRiskData.StatusCode.ToUpper.Trim <> "QUOTED" And oRiskData.IsMandatoryRisk = False And oRiskData.StatusCode.ToUpper.Trim <> "DELETED")
                                Select oRiskData.Key

                    For Each oRiskDetails In oRisk
                        If Convert.ToInt32(oRiskDetails) <> 0 Then
                            bAllRiskQuoted = False
                        End If
                    Next
                    oUpdatedQuote = Nothing
                    oRiskCollection = Nothing

                    If bAllRiskQuoted Then
                        oQuote = CType(Session(CNQuote), NexusProvider.Quote)
                        Session(CNCurrentRiskKey) = nMandatoryRiskKeyIndex
                        nCurrentRiskCounter = nMandatoryRiskKey
                        If oQuote.Risks(nMandatoryRiskKeyIndex).XMLDataset Is Nothing Then
                            oWebService.GetHeaderAndRisksByKey(oQuote)
                            Session(CNQuote) = oQuote
                            Dim bIgnoreLocking As Boolean = True
                            'Populate XML dataset atleast for first risk as it will help to get datamodal code and quick quote flag
                            oWebService.GetRisk(nCurrentRiskCounter, nMandatoryRiskKeyIndex, oQuote, oQuote.BranchCode, v_bIgnoreLocking:=bIgnoreLocking)
                        End If

                        If oQuote.Risks(nMandatoryRiskKeyIndex).RiskCode Is Nothing Then
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nMandatoryRiskKeyIndex).RiskTypeCode.Trim)
                        Else
                            oRiskTypeConfig = oProduct.RiskTypes.RiskType(oQuote.Risks(nMandatoryRiskKeyIndex).RiskCode.Trim)
                        End If
                        If oQuote.Risks(nMandatoryRiskKeyIndex).ScreenCode Is Nothing Then
                            sScreenCode = GetScreenCode(sProductFolder & "\" & oRiskTypeConfig.Path & "\" & oProduct.FullQuoteConfig)
                            oQuote.Risks(nMandatoryRiskKeyIndex).ScreenCode = sScreenCode
                        End If
                        Session(CNQuote) = oQuote
                        oWebService.UpdateRisk(oQuote, nMandatoryRiskKeyIndex, Nothing, , oQuote.TransactionType.ToString, True)
                        Session(CNQuote) = oQuote
                    End If

                End If

                'Refill the session CNQuote
                oQuote = oWebService.GetHeaderAndSummariesByKey(oQuote.InsuranceFileKey)

                For i As Integer = 0 To oQuote.Risks.Count - 1
                    oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode, True)
                Next

                oWebService.GetHeaderAndRisksByKey(oQuote)

                Session(CNQuote) = oQuote

                Response.Redirect("~/secure/premiumdisplay.aspx")

            Finally

                oNexusFrameWork = Nothing
                oQuote = Nothing
                oProduct = Nothing
                oRiskTypeConfig = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' this method will open risk screen during copy risk
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub NavigateRiskScreenOnCopy()
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sRedirectPath As String = String.Empty
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
            Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
            Session(CNRatingSections) = Nothing
            Dim oRiskType As Config.RiskType
            If oQuote.Risks.Count > 0 Then
                If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode.Trim)
                End If
            End If
            Dim oRisk As New NexusProvider.RiskType
            oRisk.DataModelCode = oRiskType.DataModelCode
            oRisk.Name = oRiskType.Name
            oRisk.Path = oRiskType.Path
            oRisk.RiskCode = oRiskType.RiskCode
            Session(CNRiskType) = oRisk

            Dim sRiskFolder As String = sProductFolder & "/" & oRisk.Path & "/"
            Dim bMaindetails As String = String.Empty
            Dim sPage As String = GetFirstRiskScreen(sProductFolder & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" & oProduct.FullQuoteConfig, bMaindetails)

            Session(CNMode) = Mode.Edit
            EditDefaultRule()
            Session(CNDataModelCode) = oRisk.DataModelCode

            If String.IsNullOrEmpty(bMaindetails) = False AndAlso bMaindetails.ToLower = "false" Then
                Response.Redirect(sRiskFolder & sPage, False)
            Else
                Response.Redirect(sRiskFolder & "/" & sPage, False)
            End If

        End Sub

        'FUNCTION :ConvertToSafeInteger
        'This function changes any string to Integer without exception
        'If string is blank then we assume integer value is 0
        'If string has a floating point value we take its integer part if it is there.
        Private Function ConvertToSafeInteger(ByVal sValue As String) As Integer
            Dim rIntegerValue As Integer = 0
            If IsNumeric(sValue) Then
                If sValue.IndexOf(".") > 0 Then
                    sValue = sValue.Remove(sValue.IndexOf("."))
                ElseIf sValue.IndexOf(".") = 0 Then
                    sValue = "0"
                End If
                rIntegerValue = Convert.ToInt32(sValue)
            End If
            Return rIntegerValue
        End Function
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvRisk_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdvRisk.RowDeleting

            If Not bNotTraverse Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim sRedirectPath As String = String.Empty
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name
                Dim oRiskType As Config.RiskType
                Dim oRisk As New NexusProvider.RiskType
                Dim nCurrentRiskKey As Integer = 0

                For Each oQuoteRisk As NexusProvider.Risk In oQuote.Risks
                    If oQuoteRisk.Key = CInt(grdvRisk.Rows(e.RowIndex).Cells(5).Text.Trim) Then
                        Session(CNCurrentRiskKey) = nCurrentRiskKey
                        Exit For
                    End If
                    nCurrentRiskKey += 1
                Next

                If oQuote.Risks(Session(CNCurrentRiskKey)).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskKey).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(nCurrentRiskKey).RiskCode.Trim)
                End If

                With oRisk
                    .DataModelCode = oRiskType.DataModelCode
                    .Name = oRiskType.Name
                    .Path = oRiskType.Path
                    .RiskCode = oRiskType.RiskCode
                End With

                Session(CNRiskType) = oRisk
                Session(CNDataModelCode) = oRisk.DataModelCode

                If oProduct.NavigationOnRiskDeletion = DeleteRiskNavigation.None Or Session(CNMTAType) Is Nothing Or Session(CNRenewal) IsNot Nothing Or
                    oQuote.Risks(nCurrentRiskKey).OriginalRiskKey = 0 Then

                    Dim oRiskCollection As New NexusProvider.RiskCollection
                    oRiskCollection = oQuote.Risks
                    If oRiskCollection.Count > 1 Then
                        oRiskCollection.SortColumn = "RiskNumber"
                        oRiskCollection.SortObjectType = GetType(NexusProvider.Risk)
                        oRiskCollection.SortingOrder = SortDirection.Ascending
                        oRiskCollection.Sort()
                    End If

                    If oQuote.Risks.Count > grdvRisk.PageSize Then
                        grdvRisk.AllowPaging = True
                    Else
                        grdvRisk.AllowPaging = False
                    End If
                    grdvRisk.DataSource = oRiskCollection
                    grdvRisk.DataBind()

                    oQuote.Risks.Remove(nCurrentRiskKey)
                    Session(CNCurrentRiskKey) = oQuote.Risks.Count - 1
                    Session(CNRatingSections) = Nothing
                    Response.Redirect("~/secure/premiumdisplay.aspx")
                ElseIf oProduct.NavigationOnRiskDeletion = DeleteRiskNavigation.RatingSection Then
                    Response.Redirect("~/secure/RiskDetails.aspx", False)
                ElseIf oProduct.NavigationOnRiskDeletion = DeleteRiskNavigation.RiskScreen Then
                    Dim sRiskFolder As String = sProductFolder & "/" & oRisk.Path & "/"
                    Dim bMaindetails As String = String.Empty
                    Dim sPage As String = GetFirstRiskScreen(sProductFolder & "/" & CType(Session(CNRiskType), NexusProvider.RiskType).Path & "/" & oProduct.FullQuoteConfig, bMaindetails)

                    If String.IsNullOrEmpty(bMaindetails) = False AndAlso bMaindetails.ToLower = "false" Then
                        Response.Redirect(sRiskFolder & sPage, False)
                    Else
                        Response.Redirect(sRiskFolder & "/" & sPage, False)
                    End If
                End If
            End If
        End Sub
        Private Function IsMandatoryStatusToUpdate(ByVal nCurrentRiskCounter As Integer, ByVal oQuote As NexusProvider.Quote) As Boolean
            Dim IsAllRiskQuotedOtherThanMandatory As Boolean = False
            For nCounter As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks(nCurrentRiskCounter).IsMandatoryRisk AndAlso nCounter > 0 Then
                    If oQuote.Risks(nCounter).StatusDescription.ToLower() = "quoted" AndAlso nCounter > 0 Then
                        IsAllRiskQuotedOtherThanMandatory = True
                    Else
                        IsAllRiskQuotedOtherThanMandatory = False
                        Exit For
                    End If
                End If
            Next
            Return IsAllRiskQuotedOtherThanMandatory
        End Function
    End Class

End Namespace
