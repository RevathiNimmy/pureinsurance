Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Namespace Nexus
    Partial Class Controls_PFClientInfo
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
            If Session(CNFinancePlanDetails) IsNot Nothing Then
                oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                If oFinancePlanDetails IsNot Nothing Then
                    FillFormFields(oFinancePlanDetails)
                End If
            End If
        End Sub

        Protected Sub FillFormFields(ByVal oFinancePlanDetails As NexusProvider.FinancePlanDetails)
            With oFinancePlanDetails
                txtClientName.Text = .ClientName
                txtEmailAddress.Text = "" 'to be worked upon as Email Address is not coming from SAM in response
                txtFaxAreaCode.Text = .ClientFaxAreaCode
                txtFaxNumber.Text = .ClientFaxNo
                txtLocality.Text = .ClientTown
                txtPhAreaCode.Text = .ClientAreaCode
                txtPhExt.Text = .ClientExtension
                txtPhNumber.Text = .ClientPhoneNo
                txtPostCode.Text = .ClientPcode
                txtStreetNoName.Text = .ClientAreaCode
                txtTown.Text = .ClientTown

            End With
        End Sub

        Public Sub PFClientInfo_Save()
            Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
            'If Session(CNFinancePlanDetails) IsNot Nothing Then
            '    oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.GetHeaderAndSummariesPFPlanByKey).PremiumFinanceDetails
            If oFinancePlanDetails IsNot Nothing Then
                With oFinancePlanDetails
                    .ClientName = txtClientName.Text
                    .ClientFaxAreaCode = txtFaxAreaCode.Text
                    .ClientFaxNo = txtFaxNumber.Text
                    .ClientTown = txtLocality.Text
                    .ClientAreaCode = txtPhAreaCode.Text
                    .ClientExtension = txtPhExt.Text
                    .ClientPhoneNo = txtPhNumber.Text
                    .ClientPcode = txtPostCode.Text
                    .ClientAreaCode = txtStreetNoName.Text
                    .ClientTown = txtTown.Text
                    Session(CNFinancePlanDetails) = oFinancePlanDetails
                End With
                'End If
            End If

        End Sub
    End Class
End Namespace

