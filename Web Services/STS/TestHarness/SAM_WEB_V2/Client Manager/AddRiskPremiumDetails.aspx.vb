Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class MTC_AddRiskPremiumDetails
    Inherits System.Web.UI.Page

    Protected Sub btnAddRiskPremium_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRiskPremium.Click
        Dim oRatingDetails() As BaseGetRatingDetailsResponseTypeRow
        Dim oRatings() As BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
        If (Session("Ratings") IsNot Nothing) Then
            oRatingDetails = DirectCast(Session("Ratings"), BaseGetRatingDetailsResponseTypeRow())
            ReDim oRatings(oRatingDetails.Length - 1)
            Dim iCnt1 As Integer
            For iCnt1 = 0 To oRatingDetails.Length - 1
                oRatings(iCnt1) = New BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
                oRatings(iCnt1).AnnualPremium = oRatingDetails(iCnt1).AnnualPremium
                oRatings(iCnt1).AnnualRate = oRatingDetails(iCnt1).AnnualRate
                oRatings(iCnt1).CountryCode = oRatingDetails(iCnt1).CountryCode

                oRatings(iCnt1).EarningPatternCode = ddearningpattern.Items.FindByText(oRatingDetails(iCnt1).EarningPattern).Value
                oRatings(iCnt1).OverrideReason = oRatingDetails(iCnt1).OverrideReason
                oRatings(iCnt1).RateTypeCode = oRatingDetails(iCnt1).RatingTypeCode
                oRatings(iCnt1).StateCode = oRatingDetails(iCnt1).StateCode
                oRatings(iCnt1).SumInsured = oRatingDetails(iCnt1).SumInsured
                oRatings(iCnt1).ThisPremium = oRatingDetails(iCnt1).ThisPremium
                oRatings(iCnt1).RatingSectionTypeCode = oRatingDetails(iCnt1).RatingSectionTypeCode
            Next
        Else
            Throw New Exception("Session objects are not configured")
        End If



        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        'create the request and response objects
        Dim opUpdateRatingDetailsRequestType As New UpdateRatingDetailsRequestType
        Dim oUpdateRatingDetailsResponseType As New UpdateRatingDetailsResponseType

        'ReDim Preserve opUpdateRatingDetailsRequestType.RatingDetails(10)
        opUpdateRatingDetailsRequestType.InsuranceFileKey = txtInsuranceFileKey.Text
        opUpdateRatingDetailsRequestType.RiskKey = txtRiskKey.Text
        opUpdateRatingDetailsRequestType.BranchCode = txtBranchCode.Text

        Dim oRequestGetHeaderandSummaries As New GetHeaderAndSummariesByKeyRequestType
        Dim oResponseGetHeaderandSummaries As New GetHeaderAndSummariesByKeyResponseType

        oRequestGetHeaderandSummaries.InsuranceFileKey = txtInsuranceFileKey.Text
        oRequestGetHeaderandSummaries.BranchCode = txtBranchCode.Text
        oResponseGetHeaderandSummaries = oSAM.GetHeaderAndSummariesByKey(oRequestGetHeaderandSummaries)
        opUpdateRatingDetailsRequestType.TimeStamp = oResponseGetHeaderandSummaries.QuoteTimeStamp

        Session("QuoteTimeStamp") = oResponseGetHeaderandSummaries.QuoteTimeStamp

        If Session("Mode") = "Update" Then
            btnAddRiskPremium.Text = "Update"

            If (Session("RowIndex") IsNot Nothing) Then
                Dim iIndex As Integer = CInt(Session("RowIndex"))
                If (oRatings.Length - 1 < iIndex) Then
                    Throw New Exception("Session objects are not configured properly")
                Else
                    oRatings(iIndex) = New BaseUpdateRatingDetailsRequestTypeRatingDetailsRow

                    If (txtAnnualPremium.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtAnnualPremium.Text, oRatings(iIndex).AnnualPremium)) Then
                        oRatings(iIndex).AnnualPremium = 0
                    End If
                    If (txtAnnualRate.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtAnnualRate.Text, oRatings(iIndex).AnnualRate)) Then
                        oRatings(iIndex).AnnualRate = 0
                    End If
                    If (txtSumInsured.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtSumInsured.Text, oRatings(iIndex).SumInsured)) Then
                        oRatings(iIndex).SumInsured = 0
                    End If
                    If (txtRiskKey.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtRiskKey.Text, oRatings(iIndex).ThisPremium)) Then
                        oRatings(iIndex).ThisPremium = 0
                    End If

                    oRatings(iIndex).OverrideReason = txtOverrideReason.Text
                    oRatings(iIndex).EarningPatternCode = ddearningpattern.SelectedValue
                    oRatings(iIndex).CountryCode = ddcountry.SelectedValue
                    oRatings(iIndex).StateCode = ddstate.SelectedValue
                    oRatings(iIndex).RateTypeCode = ddratetype.SelectedValue
                    oRatings(iIndex).RatingSectionTypeCode = ddratingsection.SelectedValue
                    'ddcountry.Items.FindByValue(oRatings(iIndex).CountryId).Selected = True
                    'ddstate.Items.FindByValue(oRatings(iIndex).StateId).Selected = True
                    ''ddearningpattern.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).EarningPattern).Selected = True
                    'ddratetype.Items.FindByValue(oRatings(iIndex).RateTypeId).Selected = True
                    'ddratingsection.Items.FindByValue(oRatings(iIndex).RatingSectionId).Selected = True

                End If
            End If



            'Dim oGetRatingDetailsRequestType As New GetRatingDetailsRequestType
            'Dim oGetRatingDetailsResponseType As New GetRatingDetailsResponseType
            'Dim oSam1 As New SAMForInsuranceV2

            'Dim UserToken1 As UsernameToken = GetUserToken("sirius", "sirius")
            'oSam1.SetClientCredential(UserToken1)
            'oSam1.SetPolicy("SamClientPolicy")

            'With oGetRatingDetailsRequestType
            '    .BranchCode = Session("HeadOff")
            '    .InsuranceFileKey = Session("InsuranceFileKey")
            '    .RiskKey = Session("RiskKey")
            'End With

            'oGetRatingDetailsResponseType = oSam1.GetRatingDetails(oGetRatingDetailsRequestType)


            'txtAnnualPremium.Text = oGetRatingDetailsResponseType.RatingDetails(0).AnnualPremium
            'txtAnnualRate.Text = oGetRatingDetailsResponseType.RatingDetails(0).AnnualRate
            'txtBranchCode.Text = Session("HeadOff")
            'txtInsuranceFileKey.Text = Session("InsuranceFileKey")
            'txtOverrideReason.Text = oGetRatingDetailsResponseType.RatingDetails(0).OverrideReason
            'txtSumInsured.Text = oGetRatingDetailsResponseType.RatingDetails(0).SumInsured
            'txtThisPremium.Text = oGetRatingDetailsResponseType.RatingDetails(0).ThisPremium
            'txtRiskKey.Text = Session("RiskKey")

            'ddcountry.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).CountryId).Selected = True
            'ddstate.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).StateId).Selected = True
            ''ddearningpattern.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).EarningPattern).Selected = True
            'ddratetype.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).RateTypeId).Selected = True
            'ddratingsection.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).RatingSectionId).Selected = True



        ElseIf Session("Mode") = "Add" Then

            btnAddRiskPremium.Text = "Add New"

            If (Session("Ratings") IsNot Nothing) Then
                
                ReDim Preserve oRatings(oRatings.Length)

                Dim iIndex As Integer = oRatings.Length - 1

                oRatings(iIndex) = New BaseUpdateRatingDetailsRequestTypeRatingDetailsRow

                If (txtAnnualPremium.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtAnnualPremium.Text, oRatings(iIndex).AnnualPremium)) Then
                    oRatings(iIndex).AnnualPremium = 0
                End If
                If (txtAnnualRate.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtAnnualRate.Text, oRatings(iIndex).AnnualRate)) Then
                    oRatings(iIndex).AnnualRate = 0
                End If
                If (txtSumInsured.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtSumInsured.Text, oRatings(iIndex).SumInsured)) Then
                    oRatings(iIndex).SumInsured = 0
                End If
                If (txtRiskKey.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtRiskKey.Text, oRatings(iIndex).ThisPremium)) Then
                    oRatings(iIndex).ThisPremium = 0
                End If

                oRatings(iIndex).OverrideReason = txtOverrideReason.Text
                oRatings(iIndex).EarningPatternCode = ddearningpattern.SelectedValue
                oRatings(iIndex).CountryCode = ddcountry.SelectedValue
                oRatings(iIndex).StateCode = ddstate.SelectedValue
                oRatings(iIndex).RateTypeCode = ddratetype.SelectedValue
                oRatings(iIndex).RatingSectionTypeCode = ddratingsection.SelectedValue
            Else
                ReDim oRatings(1)

                Dim iIndex As Integer = 0

                oRatings(iIndex) = New BaseUpdateRatingDetailsRequestTypeRatingDetailsRow

                If (txtAnnualPremium.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtAnnualPremium.Text, oRatings(iIndex).AnnualPremium)) Then
                    oRatings(iIndex).AnnualPremium = 0
                End If
                If (txtAnnualRate.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtAnnualRate.Text, oRatings(iIndex).AnnualRate)) Then
                    oRatings(iIndex).AnnualRate = 0
                End If
                If (txtSumInsured.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtSumInsured.Text, oRatings(iIndex).SumInsured)) Then
                    oRatings(iIndex).SumInsured = 0
                End If
                If (txtRiskKey.Text.Trim = String.Empty OrElse Not Decimal.TryParse(txtRiskKey.Text, oRatings(iIndex).ThisPremium)) Then
                    oRatings(iIndex).ThisPremium = 0
                End If

                oRatings(iIndex).OverrideReason = txtOverrideReason.Text
                oRatings(iIndex).OverrideReason = txtOverrideReason.Text
                oRatings(iIndex).EarningPatternCode = ddearningpattern.SelectedValue
                oRatings(iIndex).CountryCode = ddcountry.SelectedValue
                oRatings(iIndex).StateCode = ddstate.SelectedValue
                oRatings(iIndex).RateTypeCode = ddratetype.SelectedValue
                oRatings(iIndex).RatingSectionTypeCode = ddratingsection.SelectedValue
            End If
            'ddcountry.Items.FindByValue(oRatings(iIndex).CountryId).Selected = True
            'ddstate.Items.FindByValue(oRatings(iIndex).StateId).Selected = True
            ''ddearningpattern.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).EarningPattern).Selected = True
            'ddratetype.Items.FindByValue(oRatings(iIndex).RateTypeId).Selected = True
            'ddratingsection.Items.FindByValue(oRatings(iIndex).RatingSectionId).Selected = True


        End If





        ''Dim iICount=
        ''Dim oRatings() As BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
        'opUpdateRatingDetailsRequestType1.AnnualPremium = txtAnnualPremium.Text
        'opUpdateRatingDetailsRequestType1.AnnualRate = txtAnnualRate.Text
        'opUpdateRatingDetailsRequestType1.CountryId = ddcountry.SelectedValue.ToString
        'opUpdateRatingDetailsRequestType1.EarningPatternId = ddearningpattern.SelectedValue
        'opUpdateRatingDetailsRequestType1.OverrideReason = txtOverrideReason.Text
        'opUpdateRatingDetailsRequestType1.RateTypeId = ddratetype.SelectedValue
        'opUpdateRatingDetailsRequestType1.RatingSectionTypeId = ddratingsection.SelectedValue
        'opUpdateRatingDetailsRequestType1.StateId = ddstate.SelectedValue
        'opUpdateRatingDetailsRequestType1.SumInsured = txtSumInsured.Text
        'opUpdateRatingDetailsRequestType1.ThisPremium = txtThisPremium.Text
        'opUpdateRatingDetailsRequestType1.OverrideReason = txtOverrideReason.Text

        opUpdateRatingDetailsRequestType.RatingDetails = oRatings
        oUpdateRatingDetailsResponseType = oSAM.UpdateRatingSections(opUpdateRatingDetailsRequestType)

        ReDim oRatingDetails(oRatings.Length - 1)

        Dim iCnt As Integer = 0
        For iCnt = 0 To oRatings.Length - 1
            oRatingDetails(iCnt) = New BaseGetRatingDetailsResponseTypeRow
            oRatingDetails(iCnt).AnnualPremium = oRatings(iCnt).AnnualPremium
            oRatingDetails(iCnt).AnnualRate = oRatings(iCnt).AnnualRate
            If Not String.IsNullOrEmpty(oRatings(iCnt).CountryCode) Then
                oRatingDetails(iCnt).CountryCode = oRatings(iCnt).CountryCode
                oRatingDetails(iCnt).Country = ddcountry.Items.FindByValue(oRatings(iCnt).CountryCode.Trim).Text
            End If
            If Not String.IsNullOrEmpty(oRatings(iCnt).EarningPatternCode) Then
                oRatingDetails(iCnt).EarningPattern = ddearningpattern.Items.FindByValue(oRatings(iCnt).EarningPatternCode.Trim).Text
            End If

            oRatingDetails(iCnt).OverrideReason = oRatings(iCnt).OverrideReason
            If Not String.IsNullOrEmpty(oRatings(iCnt).RateTypeCode) Then
                oRatingDetails(iCnt).RatingTypeCode = oRatings(iCnt).RateTypeCode
                oRatingDetails(iCnt).RateType = ddratetype.Items.FindByValue(oRatings(iCnt).RateTypeCode.Trim()).Text
            End If

            If Not String.IsNullOrEmpty(oRatings(iCnt).StateCode) Then
                oRatingDetails(iCnt).StateCode = oRatings(iCnt).StateCode
                oRatingDetails(iCnt).State = ddstate.Items.FindByValue(oRatings(iCnt).StateCode.Trim).Text
            End If

            oRatingDetails(iCnt).SumInsured = oRatings(iCnt).SumInsured
            oRatingDetails(iCnt).ThisPremium = oRatings(iCnt).ThisPremium
            'Not sure
            If Not String.IsNullOrEmpty(oRatings(iCnt).RatingSectionTypeCode) Then
                oRatingDetails(iCnt).RatingSectionTypeCode = oRatings(iCnt).RatingSectionTypeCode
                oRatingDetails(iCnt).RatingSectionType = ddratingsection.Items.FindByValue(oRatings(iCnt).RatingSectionTypeCode.Trim).Text
            End If
        Next
        Session("Ratings") = oRatingDetails
        If (btnAddRiskPremium.Text = "Add New") Then
            Label3.Text = "Added Sucessfully"
        Else
            Label3.Text = "Updated Sucessfully"
        End If
        ' Response.Redirect("RiskPremiumDetails.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label3.Text = String.Empty
        txtBranchCode.Text = Session("BranchCode")
        txtInsuranceFileKey.Text = Session("InsuranceFileKey")
        txtRiskKey.Text = Session("RiskKey")
      
        If IsPostBack = False Then

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            Dim oRequest As New GetListRequestType
            Dim oResponse As New GetListResponseType


            'Binding Progres Status Drop down - 
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Rating_Section_Type"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddratingsection.DataSource = oResponse.List
                        ddratingsection.DataTextField = "Description"
                        ddratingsection.DataValueField = "Key"
                        ddratingsection.DataBind()
                    End If


                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try



            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Earning_Pattern"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddearningpattern.DataSource = oResponse.List
                        ddearningpattern.DataTextField = "Description"
                        ddearningpattern.DataValueField = "code"
                        ddearningpattern.DataBind()
                    End If


                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "rate_type"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddratetype.DataSource = oResponse.List
                        ddratetype.DataTextField = "Description"
                        ddratetype.DataValueField = "code"
                        ddratetype.DataBind()
                    End If


                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try


            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "Country"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddcountry.DataSource = oResponse.List
                        ddcountry.DataTextField = "Description"
                        ddcountry.DataValueField = "code"
                        ddcountry.DataBind()
                    End If


                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try


            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "State"
            Try
                oResponse = oSAM.GetList(oRequest)

                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        ddstate.DataSource = oResponse.List
                        ddstate.DataTextField = "Description"
                        ddstate.DataValueField = "code"
                        ddstate.DataBind()
                    End If


                End With

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)

            Finally
                'clean up any objects here
            End Try

            If Session("Mode") = "Update" Then
                btnAddRiskPremium.Text = "Update"
                If (Session("Ratings") IsNot Nothing) Then
                    Dim oRatings() As BaseGetRatingDetailsResponseTypeRow = DirectCast(Session("Ratings"), BaseGetRatingDetailsResponseTypeRow())
                    If (Session("RowIndex") IsNot Nothing) Then
                        Dim iIndex As Integer = CInt(Session("RowIndex"))
                        If (oRatings.Length - 1 < iIndex) Then
                            Throw New Exception("Session objects are not configured properly")
                        Else
                            txtAnnualPremium.Text = oRatings(iIndex).AnnualPremium
                            txtAnnualRate.Text = oRatings(iIndex).AnnualRate
                            txtBranchCode.Text = Session("BranchCode")
                            txtInsuranceFileKey.Text = Session("InsuranceFileKey")
                            txtOverrideReason.Text = oRatings(iIndex).OverrideReason
                            txtSumInsured.Text = oRatings(iIndex).SumInsured
                            txtThisPremium.Text = oRatings(iIndex).ThisPremium
                            txtRiskKey.Text = Session("RiskKey")
                            If IsDBNull(oRatings(iIndex).CountryId) Or oRatings(iIndex).CountryId = 0 Then
                                ' ddcountry.Items.FindByValue(oRatings(iIndex).CountryId).Selected = 0
                            Else
                                ddcountry.Items.FindByValue(oRatings(iIndex).CountryId).Selected = True
                            End If
                            If IsDBNull(oRatings(iIndex).StateId) Or oRatings(iIndex).StateId = 0 Then
                                'ddstate.Items.FindByValue(oRatings(iIndex).StateId).Selected = 0
                            Else
                                ddstate.Items.FindByValue(oRatings(iIndex).StateId).Selected = True
                            End If

                            'ddearningpattern.Items.FindByText(oRatings(iIndex).EarningPattern).Selected = True
                            ddratetype.Items.FindByValue(oRatings(iIndex).RateTypeId).Selected = True
                            ddratingsection.Items.FindByText(oRatings(iIndex).RatingSectionType).Selected = True



                        End If
                End If

                End If

                'Dim oGetRatingDetailsRequestType As New GetRatingDetailsRequestType
                'Dim oGetRatingDetailsResponseType As New GetRatingDetailsResponseType
                'Dim oSam1 As New SAMForInsuranceV2

                'Dim UserToken1 As UsernameToken = GetUserToken("sirius", "sirius")
                'oSam1.SetClientCredential(UserToken1)
                'oSam1.SetPolicy("SamClientPolicy")

                'With oGetRatingDetailsRequestType
                '    .BranchCode = Session("HeadOff")
                '    .InsuranceFileKey = Session("InsuranceFileKey")
                '    .RiskKey = Session("RiskKey")
                'End With

                'oGetRatingDetailsResponseType = oSam1.GetRatingDetails(oGetRatingDetailsRequestType)


                'txtAnnualPremium.Text = oGetRatingDetailsResponseType.RatingDetails(0).AnnualPremium
                'txtAnnualRate.Text = oGetRatingDetailsResponseType.RatingDetails(0).AnnualRate
                'txtBranchCode.Text = Session("HeadOff")
                'txtInsuranceFileKey.Text = Session("InsuranceFileKey")
                'txtOverrideReason.Text = oGetRatingDetailsResponseType.RatingDetails(0).OverrideReason
                'txtSumInsured.Text = oGetRatingDetailsResponseType.RatingDetails(0).SumInsured
                'txtThisPremium.Text = oGetRatingDetailsResponseType.RatingDetails(0).ThisPremium
                'txtRiskKey.Text = Session("RiskKey")

                'ddcountry.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).CountryId).Selected = True
                'ddstate.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).StateId).Selected = True
                ''ddearningpattern.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).EarningPattern).Selected = True
                'ddratetype.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).RateTypeId).Selected = True
                'ddratingsection.Items.FindByValue(oGetRatingDetailsResponseType.RatingDetails(0).RatingSectionId).Selected = True



            ElseIf Session("Mode") = "Add" Then
                btnAddRiskPremium.Text = "Add New"
            End If


        End If



    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Response.Redirect("RiskPremiumDetails.aspx")
    End Sub
End Class
