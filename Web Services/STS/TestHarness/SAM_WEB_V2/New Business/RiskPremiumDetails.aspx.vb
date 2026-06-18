Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTC_RiskPremiumDetails
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim dRiskPremiumTotal As Decimal
    Dim dRiskFeeTotal As Decimal
    Dim dRiskTaxTotal As Decimal



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MultiView1.ActiveViewIndex = 0
        If Not IsPostBack Then
            GetHeaderAndRisk()
        End If

    End Sub

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        
        Dim iSelectedIndex As Integer
        iSelectedIndex = Menu1.Items.IndexOf(Menu1.SelectedItem)
        MultiView1.ActiveViewIndex = iSelectedIndex
    End Sub

    Protected Sub grd_Output_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles grd_Output.RowCancelingEdit
        grd_Output.EditIndex = -1
    End Sub

    Protected Sub grd_Output_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grd_Output.RowEditing
        
    End Sub

    Protected Sub grd_Output_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles grd_Output.RowUpdating
        
    End Sub

    Protected Sub btnAddMTAQuote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMTAQuote.Click
        
        Session("Mode") = "Add"
        Response.Redirect("AddRiskPremiumDetails.aspx")
    End Sub

    Protected Sub grd_Output_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grd_Output.RowCommand

        If (e.CommandName = "Edit") Then
            Session("Mode") = "Update"
            Session("RowIndex") = CInt(e.CommandArgument)
            Response.Redirect("AddRiskPremiumDetails.aspx")
        End If
    End Sub
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Response.Redirect("RiskReinsuranceArrangements.aspx")
    End Sub


    Private Sub GetHeaderAndRisk()
        Dim oGetHeaderAndRiskFeesByKeyRequestType As New GetHeaderAndRiskFeesByKeyRequestType
        Dim oGetHeaderAndRiskFeesByKeyResponseType As New GetHeaderAndRiskFeesByKeyResponseType

        Dim oGetHeaderAndRiskTaxByKeyRequestType As New GetHeaderAndRiskTaxByKeyRequestType
        Dim oGetHeaderAndRiskTaxByKeyResponseType As New GetHeaderAndRiskTaxByKeyResponseType

        Dim oGetRatingDetailsRequestType As New GetRatingDetailsRequestType
        Dim oGetRatingDetailsResponseType As New GetRatingDetailsResponseType


        Dim oSAM As New SAMForInsuranceV2
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        With oGetRatingDetailsRequestType
            .BranchCode = "HeadOff" ' Session("BranchCode")
            .InsuranceFileKey = Session("InsuranceFileKey")
            .RiskKey = Session("RiskKey")
        End With

        StartDate = Date.Now
        oGetRatingDetailsResponseType = oSAM.GetRatingDetails(oGetRatingDetailsRequestType)
        WriteToLog(Session, "RiskPremiumDetails.aspx", "SAMForInsuranceV2", "GetRatingDetails",StartDate, Date.Now)
        With oGetRatingDetailsResponseType
            If Not (.Errors) Is Nothing Then
                'errors returned, so throw an exception
                lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
            End If

        End With

        dRiskPremiumTotal = 0.0

        grd_Output.DataSource = oGetRatingDetailsResponseType.RatingDetails
        grd_Output.DataBind()
        lblNetTotal.Text = dRiskPremiumTotal.ToString()
        Session("Ratings") = oGetRatingDetailsResponseType.RatingDetails

        
        lblOutput.Text = "List Update Rating Details"
        

        lblInsuranceFileRef.Text = ""
        lblClientCode.Text = ""
        lblCurrency.Text = ""

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        With oGetHeaderAndRiskFeesByKeyRequestType
            .BranchCode = "HeadOff"
            .InsuranceFileKey = Session("InsuranceFileKey")
            .RiskKey = Session("RiskKey")
        End With
        StartDate = Date.Now
        oGetHeaderAndRiskFeesByKeyResponseType = oSAM.GetHeaderAndRiskFeesByKey(oGetHeaderAndRiskFeesByKeyRequestType)
        WriteToLog(Session, "RiskPremiumDetails.aspx", "SAMForInsuranceV2", "GetHeaderAndRiskFeesByKey",StartDate, Date.Now)
        lblInsuranceFileKey.Text = oGetHeaderAndRiskFeesByKeyResponseType.InsuranceFileKey
        lblInsuranceFileRef.Text = oGetHeaderAndRiskFeesByKeyResponseType.InsuranceFileRef
        lblClientCode.Text = oGetHeaderAndRiskFeesByKeyResponseType.ClientCode
        lblCurrency.Text = oGetHeaderAndRiskFeesByKeyResponseType.Currency
        dRiskFeeTotal = 0.0
        grd_Output1.DataSource = oGetHeaderAndRiskFeesByKeyResponseType.RiskFees
        grd_Output1.DataBind()
        lblFeesTotal.Text = dRiskFeeTotal.ToString

        Label1.Text = "List Risk Fees Details"

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        With oGetHeaderAndRiskTaxByKeyRequestType
            .BranchCode = "HeadOff"
            .InsuranceFileKey = Session("InsuranceFileKey")
            .RiskKey = Session("RiskKey")
        End With
        StartDate = Date.Now
        oGetHeaderAndRiskTaxByKeyResponseType = oSAM.GetHeaderAndRiskTaxByKey(oGetHeaderAndRiskTaxByKeyRequestType)
        WriteToLog(Session, "RiskPremiumDetails.aspx", "SAMForInsuranceV2", "GetHeaderAndRiskTaxByKey",StartDate, Date.Now)
        lblInsuranceFileKey.Text = oGetHeaderAndRiskTaxByKeyResponseType.InsuranceFileKey
        lblInsuranceFileRef.Text = oGetHeaderAndRiskTaxByKeyResponseType.InsuranceFileRef
        lblClientCode.Text = oGetHeaderAndRiskTaxByKeyResponseType.ClientCode
        lblCurrency.Text = oGetHeaderAndRiskTaxByKeyResponseType.Currency
        dRiskTaxTotal = 0.0
        grd_Output2.DataSource = oGetHeaderAndRiskTaxByKeyResponseType.RiskTaxes
        grd_Output2.DataBind()
        lblTaxTotal.Text = dRiskTaxTotal.ToString


        Label3.Text = "List Risk Tax Details"

        lblGrossTotal.Text = (dRiskPremiumTotal + dRiskFeeTotal + dRiskTaxTotal).ToString

    End Sub

    Protected Sub grd_Output_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd_Output.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            dRiskPremiumTotal = dRiskPremiumTotal + DirectCast(e.Row.DataItem, BaseGetRatingDetailsResponseTypeRow).ThisPremium
        End If
    End Sub

    Protected Sub grd_Output1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd_Output1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            dRiskFeeTotal = dRiskFeeTotal + DirectCast(e.Row.DataItem, BaseGetHeaderAndRiskFeesByKeyResponseTypeRow).TotalAmount
        End If
    End Sub

    Protected Sub grd_Output2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grd_Output2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            dRiskTaxTotal = dRiskTaxTotal + DirectCast(e.Row.DataItem, BaseGetHeaderAndRiskTaxByKeyResponseTypeRow).TaxAmount
        End If
    End Sub

  
    Protected Sub grd_Output_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grd_Output.RowDeleting
        Dim oTempRatings As BaseUpdateRatingDetailsRequestTypeRatingDetailsRow()
        Dim oRatings As BaseGetRatingDetailsResponseTypeRow()
        oRatings = DirectCast(Session("Ratings"), BaseGetRatingDetailsResponseTypeRow())

        For Count As Integer = 0 To oRatings.Length - 1
            If Count <> e.RowIndex Then
                If oTempRatings Is Nothing Then
                    ReDim Preserve oTempRatings(0)
                Else
                    ReDim Preserve oTempRatings(oTempRatings.Length)
                End If
                oTempRatings(oTempRatings.Length - 1) = New BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
                oTempRatings(oTempRatings.Length - 1).AnnualPremium = oRatings(Count).AnnualPremium
                oTempRatings(oTempRatings.Length - 1).AnnualRate = oRatings(Count).AnnualRate
                oTempRatings(oTempRatings.Length - 1).CountryCode = oRatings(Count).CountryCode
                oTempRatings(oTempRatings.Length - 1).OverrideReason = oRatings(Count).OverrideReason
                oTempRatings(oTempRatings.Length - 1).RateTypeCode = oRatings(Count).RatingTypeCode
                oTempRatings(oTempRatings.Length - 1).RatingSectionTypeCode = oRatings(Count).RatingSectionTypeCode
                oTempRatings(oTempRatings.Length - 1).StateCode = oRatings(Count).StateCode
                oTempRatings(oTempRatings.Length - 1).SumInsured = oRatings(Count).SumInsured
                oTempRatings(oTempRatings.Length - 1).ThisPremium = oRatings(Count).ThisPremium
                oTempRatings(oTempRatings.Length - 1).EarningPatternCode = oRatings(Count).EarningPatternCode
            End If

        Next
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oUpdateRatingDetailsRequestType As New UpdateRatingDetailsRequestType
        Dim oUpdateRatingDetailsResponseType As New UpdateRatingDetailsResponseType

        'ReDim Preserve opUpdateRatingDetailsRequestType.RatingDetails(10)
        oUpdateRatingDetailsRequestType.InsuranceFileKey = Session("InsuranceFileKey")
        oUpdateRatingDetailsRequestType.RiskKey = Session("RiskKey")
        oUpdateRatingDetailsRequestType.BranchCode = "HeadOff" 'Session("BranchCode")
        oUpdateRatingDetailsRequestType.RatingDetails = oTempRatings
        oUpdateRatingDetailsRequestType.TimeStamp = Session("TimeStamp")


        Try
            StartDate = Date.Now
            oUpdateRatingDetailsResponseType = oSAM.UpdateRatingSections(oUpdateRatingDetailsRequestType)
            WriteToLog(Session, "RiskPremiumDetails.aspx", "SAMForInsuranceV2", "UpdateRatingSections",StartDate, Date.Now)
            With oUpdateRatingDetailsResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else
                    Session("TimeStamp") = .TimeStamp
                    GetHeaderAndRisk()
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean
        End Try






    End Sub
End Class

