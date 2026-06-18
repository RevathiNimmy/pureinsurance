Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Utils
Namespace Nexus
    Partial Class Controls_PFPlanDetails
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
            'Get the product configuration for decimal suppression.
            If hdnIsSuppressDecimals.Value Is Nothing OrElse Trim(hdnIsSuppressDecimals.Value) = "" Then
                Dim oWebService As NexusProvider.ProviderBase = Nothing
                Dim oSuppressDecimalOptionType As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oSuppressDecimalOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.SuppressDecimalValues)
                If oSuppressDecimalOptionType IsNot Nothing Then
                    hdnIsSuppressDecimals.Value = oSuppressDecimalOptionType.OptionValue
                End If
                oWebService = Nothing
            End If
            If Not Page.IsPostBack Then

                If oFinancePlanDetails IsNot Nothing Then

                    If Session(CNFinancePlanDetails) IsNot Nothing Then
                        oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                        If oFinancePlanDetails IsNot Nothing Then
                            FillFormFields(oFinancePlanDetails)

                        End If
                        If CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFHistory IsNot Nothing _
                        AndAlso CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PFHistory.Count > 1 Then
                            btnHistory.Enabled = True
                        Else
                            btnHistory.Enabled = False
                        End If
                    End If
                End If
                lblSchemeName.Text = oFinancePlanDetails.PFSchemeName & ", " & oFinancePlanDetails.PFFrequencyDesc & ", " & oFinancePlanDetails.PaymentMethod
            End If
            If (Session(CNInstalmentPlanMode) = InstalmentPlanType.edit AndAlso oFinancePlanDetails.StatusInd <> NexusProvider.FinancePlanStatus.Item999) OrElse (Request.QueryString("ProcessType") IsNot Nothing AndAlso Request.QueryString("ProcessType") = "MTA") Then
                DisableControls(Me)
                
            ElseIf Session(CNInstalmentPlanMode) = InstalmentPlanType.View OrElse oFinancePlanDetails.StatusInd = NexusProvider.FinancePlanStatus.Item999 Then
                DisableControls(Me)
            End If
         
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnTrans.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PlanTransactions.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                btnHistory.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PlanHistory.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
            Else
                btnTrans.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PlanTransactions.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                btnHistory.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PlanHistory.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
            End If
            

        End Sub

        Protected Sub FillFormFields(ByVal oFinancePlanDetails As NexusProvider.FinancePlanDetails)
            With oFinancePlanDetails
                If .StatusInd <> NexusProvider.FinancePlanStatus.Item000 AndAlso .StatusInd.ToString() <> "" Then
                    ddlStatus.SelectedValue = oFinancePlanDetails.StatusInd.ToString().Substring(4)
                    Session(CNFinancePlanStatus) = ddlStatus.SelectedItem.Text

                End If
                ddlCancelReason.Value = .PFPremiumFinanceCancelReason
                txtAPR.Text = .APR
                txtDaysDelay.Text = .DaysDelay
                txtDateCreated.Text = .DateCreated
                txtModified.Text = .DateModified
                txtDeposit.Text = .Deposit
                txtFinanceAmount.Text = .FinanceAmount
                If Trim(hdnIsSuppressDecimals.Value) = "1" Then
                    txtFinanceCharge.Text = .FinanceFee + Math.Round((.FinanceAmount * .InterestRate / 100), 0, MidpointRounding.AwayFromZero)
                Else
                    txtFinanceCharge.Text = .FinanceFee + (.FinanceAmount * .InterestRate / 100)
                End If
                txtFirstInstalment.Text = .FirstInstallment
                txtFirstInstalmentDate.Text = .FirstInstalmentDate
                txtInstalments.Text = .NoOfInstallments
                txtInterestRate.Text = .InterestRate
                txtLastInstalmentDate.Text = .LastInstalmentDate
                txtNextInstalmentDate.Text = .NextInstalmentDate
                txtOriginalDebt.Text = .OriginalAmount
                txtOtherInstalments.Text = .OtherInstallments
                txtProtection.Text = .CostOfProtection
                txtReference.Text = .AutoGeneratedPlanRef
                txtStartDate.Text = .StartDate
                txtTaxes.Text = .TaxCost
                txtTotalAmount.Text = .TotalCost
                hvMediaTypeCode.Value = .MediaTypeCode
            End With

        End Sub

        Protected Sub FillDropDown()
            Dim itemValues As Array = System.Enum.GetValues(GetType(NexusProvider.FinancePlanStatus))
            Dim itemNames As Array = System.Enum.GetNames(GetType(NexusProvider.FinancePlanStatus))

            For i As Integer = 0 To itemNames.Length - 1
                Dim item As New ListItem(FinancePlanStatusDesc(CType(itemValues(i).ToString().Substring(4), NexusProvider.FinancePlanStatus)), itemValues(i))
                ddlStatus.Items.Add(item)
            Next
        End Sub

        Public Sub PFPlanDetails_Save()
            Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan
            oProcessPFPlan.PremiumFinanceDetails = New NexusProvider.FinancePlanDetails
            If Session(CNFinancePlanDetails) IsNot Nothing Then
                oProcessPFPlan = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan)
            End If
            If oProcessPFPlan IsNot Nothing Then

                With oProcessPFPlan.PremiumFinanceDetails
                    .APR = txtAPR.Text
                    .DaysDelay = txtDaysDelay.Text
                    .DateCreated = txtDateCreated.Text
                    .DateModified = txtModified.Text
                    .Deposit = txtDeposit.Text
                    .FinanceAmount = txtFinanceAmount.Text
                    .FinanceFee = txtFinanceCharge.Text
                    .FirstInstallment = txtFirstInstalment.Text
                    .FirstInstalmentDate = txtFirstInstalmentDate.Text
                    .NoOfInstallments = txtInstalments.Text
                    .InterestRate = txtInterestRate.Text
                    .LastInstalmentDate = txtLastInstalmentDate.Text
                    .NextInstalmentDate = txtNextInstalmentDate.Text
                    .OriginalAmount = txtOriginalDebt.Text
                    .OtherInstallments = txtOtherInstalments.Text
                    .PayProtection = txtProtection.Text
                    .AutoGeneratedPlanRef = txtReference.Text
                    .StartDate = txtStartDate.Text
                    .TaxCost = txtTaxes.Text
                    .TotalCost = txtTotalAmount.Text
                    Session(CNFinancePlanDetails) = oProcessPFPlan
                End With

            End If


        End Sub
    End Class
End Namespace
