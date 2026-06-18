Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTC_RiskPremiumDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If (Session("Ratings") IsNot Nothing) Then
                Dim oRatings() As BaseGetRatingDetailsResponseTypeRow = DirectCast(Session("Ratings"), BaseGetRatingDetailsResponseTypeRow())
                grd_Output.DataSource = oRatings
                grd_Output.DataBind()
            End If
            'Session("BranchCode") = "HeadOff"
            'Session("InsuranceFileKey") = "2444"
            'Session("RiskKey") = "2662 "
        End If
    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim oGetHeaderAndRiskFeesByKeyRequestType As New GetHeaderAndRiskFeesByKeyRequestType
        Dim oGetHeaderAndRiskFeesByKeyResponseType As New GetHeaderAndRiskFeesByKeyResponseType

        Dim oGetHeaderAndRiskTaxByKeyRequestType As New GetHeaderAndRiskTaxByKeyRequestType
        Dim oGetHeaderAndRiskTaxByKeyResponseType As New GetHeaderAndRiskTaxByKeyResponseType

        Dim oGetRatingDetailsRequestType As New GetRatingDetailsRequestType
        Dim oGetRatingDetailsResponseType As New GetRatingDetailsResponseType


        Dim oSAM As New SAMForInsuranceV2

        Dim iSelectedIndex As Integer
        iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
        'MultiView1.ActiveViewIndex = iSelectedIndex

        If (iSelectedIndex = 0) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetRatingDetailsRequestType
                .BranchCode = Session("BranchCode")
                .InsuranceFileKey = Session("InsuranceFileKey")
                .RiskKey = Session("RiskKey")

            End With


            oGetRatingDetailsResponseType = oSAM.GetRatingDetails(oGetRatingDetailsRequestType)

            With oGetRatingDetailsResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If

            End With

            grd_Output.DataSource = oGetRatingDetailsResponseType.RatingDetails
            'Dim dv As New Data.DataView
            'dv.RowFilter = ""
            grd_Output.DataBind()
            Session("Ratings") = oGetRatingDetailsResponseType.RatingDetails

            lblOutput.Visible = True
            Label1.Visible = False
            Label3.Visible = False
            lblOutput.Text = "List Update Rating Details"
            grd_Output1.Visible = False
            grd_Output2.Visible = False
            grd_Output.Visible = True
            lblInsuranceFileKey.Text = ""
            lblInsuranceFileRef.Text = ""
            lblClientCode.Text = ""
            lblCurrency.Text = ""
            btnAddMTAQuote.Visible = True

        ElseIf (iSelectedIndex = 1) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetHeaderAndRiskFeesByKeyRequestType
                .BranchCode = Session("BranchCode")
                .InsuranceFileKey = Session("InsuranceFileKey")
                .RiskKey = Session("RiskKey")
            End With

            oGetHeaderAndRiskFeesByKeyResponseType = oSAM.GetHeaderAndRiskFeesByKey(oGetHeaderAndRiskFeesByKeyRequestType)

            lblInsuranceFileKey.Text = oGetHeaderAndRiskFeesByKeyResponseType.InsuranceFileKey
            lblInsuranceFileRef.Text = oGetHeaderAndRiskFeesByKeyResponseType.InsuranceFileRef
            lblClientCode.Text = oGetHeaderAndRiskFeesByKeyResponseType.ClientCode
            lblCurrency.Text = oGetHeaderAndRiskFeesByKeyResponseType.Currency

            grd_Output1.DataSource = oGetHeaderAndRiskFeesByKeyResponseType.RiskFees
            grd_Output1.DataBind()

            Label1.Visible = True
            Label3.Visible = False
            lblOutput.Visible = False
            Label1.Text = "List Risk Fees Details"
            grd_Output.Visible = False
            grd_Output2.Visible = False
            grd_Output1.Visible = True
            btnAddMTAQuote.Visible = False

        ElseIf (iSelectedIndex = 2) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            With oGetHeaderAndRiskTaxByKeyRequestType
                .BranchCode = Session("BranchCode")
                .InsuranceFileKey = Session("InsuranceFileKey")
                .RiskKey = Session("RiskKey")
            End With

            oGetHeaderAndRiskTaxByKeyResponseType = oSAM.GetHeaderAndRiskTaxByKey(oGetHeaderAndRiskTaxByKeyRequestType)

            lblInsuranceFileKey.Text = oGetHeaderAndRiskTaxByKeyResponseType.InsuranceFileKey
            lblInsuranceFileRef.Text = oGetHeaderAndRiskTaxByKeyResponseType.InsuranceFileRef
            lblClientCode.Text = oGetHeaderAndRiskTaxByKeyResponseType.ClientCode
            lblCurrency.Text = oGetHeaderAndRiskTaxByKeyResponseType.Currency

            grd_Output2.DataSource = oGetHeaderAndRiskTaxByKeyResponseType.RiskTaxes
            grd_Output2.DataBind()
            Label3.Visible = True
            Label1.Visible = False
            lblOutput.Visible = False
            Label3.Text = "List Risk Tax Details"
            grd_Output.Visible = False
            grd_Output1.Visible = False
            grd_Output2.Visible = True
            btnAddMTAQuote.Visible = False
        End If
    End Sub

    Protected Sub grd_Output_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles grd_Output.RowCancelingEdit
        grd_Output.EditIndex = -1
    End Sub

    Protected Sub grd_Output_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grd_Output.RowEditing
        'Dim oGetRatingDetailsRequestType As New GetRatingDetailsRequestType
        'Dim oGetRatingDetailsResponseType As New GetRatingDetailsResponseType
        'Dim oSam As New SAMForInsuranceV2

        'Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'oSam.SetClientCredential(UserToken)
        'oSam.SetPolicy("SamClientPolicy")

        'With oGetRatingDetailsRequestType
        '    .BranchCode = txtBranchCode.Text
        '    .InsuranceFileKey = txtInsuranceFile.Text
        '    .RiskKey = txtRiskKey.Text
        'End With


        'oGetRatingDetailsResponseType = oSam.GetRatingDetails(oGetRatingDetailsRequestType)
        'grd_Output.EditIndex = e.NewEditIndex
        'grd_Output.DataSource = oGetRatingDetailsResponseType.RatingDetails
        'grd_Output.DataBind()

    End Sub

    Protected Sub grd_Output_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles grd_Output.RowUpdating
        'Dim oGetRatingDetailsRequestType As New GetRatingDetailsRequestType
        'Dim oGetRatingDetailsResponseType As New GetRatingDetailsResponseType

        'Dim oUpdateRatingDetailsRequestType As New UpdateRatingDetailsRequestType
        'Dim oUpdateRatingDetailsResponseType As New UpdateRatingDetailsResponseType


        'Dim oSam As New SAMForInsuranceV2
        '' Dim gvRow As GridViewRow

        'Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        'oSam.SetClientCredential(UserToken)
        'oSam.SetPolicy("SamClientPolicy")

        'With oUpdateRatingDetailsRequestType
        '    .BranchCode = txtBranchCode.Text
        '    .InsuranceFileKey = txtInsuranceFile.Text
        '    .RiskKey = txtRiskKey.Text


        '    Dim gvRow As GridViewRow
        '    gvRow = grd_Output.Rows(e.RowIndex)
        '    Dim txtRatingSectionType As New TextBox
        '    Dim txtEarningPattern As New TextBox
        '    Dim txtRateType As New TextBox
        '    Dim txtAnnualRate As New TextBox
        '    Dim txtSumInsured As New TextBox
        '    Dim txtThisPremium As New TextBox
        '    Dim txtAnnualPremium As New TextBox
        '    Dim txtCountry As New TextBox
        '    Dim txtState As New TextBox

        '    txtRatingSectionType = DirectCast(gvRow.FindControl("txtRatingSectionType"), TextBox)
        '    txtEarningPattern = DirectCast(gvRow.FindControl("txtEarningPattern"), TextBox)
        '    txtRateType = DirectCast(gvRow.FindControl("txtRateType"), TextBox)
        '    txtAnnualRate = DirectCast(gvRow.FindControl("txtAnnualRate"), TextBox)
        '    txtSumInsured = DirectCast(gvRow.FindControl("txtSumInsured"), TextBox)
        '    txtThisPremium = DirectCast(gvRow.FindControl("txtThisPremium"), TextBox)
        '    txtAnnualPremium = DirectCast(gvRow.FindControl("txtAnnualPremium"), TextBox)
        '    txtCountry = DirectCast(gvRow.FindControl("txtCountry"), TextBox)
        '    txtState = DirectCast(gvRow.FindControl("txtState"), TextBox)


        '    .RatingDetails(0).RatingSectionTypeId = txtRatingSectionType.Text
        '    .RatingDetails(0).EarningPatternId = txtRatingSectionType.Text
        '    .RatingDetails(0).RateTypeId = txtRatingSectionType.Text
        '    .RatingDetails(0).AnnualRate = txtRatingSectionType.Text
        '    .RatingDetails(0).SumInsured = txtRatingSectionType.Text
        '    .RatingDetails(0).ThisPremium = txtRatingSectionType.Text
        '    .RatingDetails(0).AnnualPremium = txtRatingSectionType.Text
        '    .RatingDetails(0).CountryId = txtRatingSectionType.Text
        '    .RatingDetails(0).StateId = txtRatingSectionType.Text


        'End With

        'oUpdateRatingDetailsResponseType = oSam.UpdateRatingSections(oUpdateRatingDetailsRequestType)

        'oGetRatingDetailsResponseType = oSam.GetRatingDetails(oGetRatingDetailsRequestType)
        ''gvRow = grd_Output.Rows(e.RowIndex)
        'grd_Output.EditIndex = -1
        'grd_Output.DataSource = oGetRatingDetailsResponseType.RatingDetails
        'grd_Output.DataBind()
    End Sub

    Protected Sub btnAddMTAQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMTAQuote.Click
        Session("Mode") = "Add"
        Response.Redirect("AddRiskPremiumDetails.aspx")
    End Sub
    Protected Sub btnAddMTAQuote_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMTAQuote.Load

    End Sub

    Protected Sub grd_Output_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd_Output.RowCommand

        If (e.CommandName = "Edit") Then
            'Session("InsuranceFileKey") = txtInsuranceFile.Text
            'Session("RiskKey") = txtRiskKey.Text
            'Session("BranchCode") = txtBranchCode.Text

            'Session("update") = "update"
            Session("Mode") = "Update"
            Session("RowIndex") = CInt(e.CommandArgument)
            Response.Redirect("AddRiskPremiumDetails.aspx")
        End If
    End Sub






End Class

