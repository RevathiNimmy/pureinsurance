Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class New_Business_CoInsurance
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
    Dim Total As New Double



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtShare.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtCommission.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")



        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Total = 0.0
        If Not Page.IsPostBack Then

            Dim oGetCoinsuranceDefaultsRequestRequest As New GetCoinsuranceDefaultsRequestType
            Dim oGetCoinsuranceDefaultsRequestResponse As New GetCoinsuranceDefaultsResponseType

            oGetCoinsuranceDefaultsRequestRequest.BranchCode = "HeadOff"

            Try
                StartDate = Date.Now
                oGetCoinsuranceDefaultsRequestResponse = oSAM.GetCoinsuranceDefaults(oGetCoinsuranceDefaultsRequestRequest)
                WriteToLog(Session, "CoInsurance.aspx", "SAMForInsuranceV2", "GetCoinsuranceDefaults",StartDate, Date.Now)
                With oGetCoinsuranceDefaultsRequestResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        ddlDefaults.DataSource = .Defaults
                        ddlDefaults.DataTextField = "CoinsuranceDefault"
                        ddlDefaults.DataValueField = "CoInsuranceDefaultId"
                        ddlDefaults.DataBind()

                        chkIsRecovered.Checked = .Defaults(ddlDefaults.SelectedIndex).IsRecovered
                        chkIsSurcharged.Checked = .Defaults(ddlDefaults.SelectedIndex).IsSurcharged
                        chkIsRecovered.Enabled = False
                        chkIsSurcharged.Enabled = True

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

            Dim oGetCoinsuranceValuesRequest As New GetCoinsuranceValuesRequestType
            Dim oGetCoinsuranceValuesResponse As New GetCoinsuranceValuesResponseType

            With oGetCoinsuranceValuesRequest
                .BranchCode = "HeadOff"
                .InsuranceFileKey = Session("InsuranceFileKey")
            End With

            Try
                StartDate = Date.Now
                oGetCoinsuranceValuesResponse = oSAM.GetCoinsuranceValues(oGetCoinsuranceValuesRequest)
                WriteToLog(Session, "CoInsurance.aspx", "SAMForInsuranceV2", "GetCoinsuranceValues",StartDate, Date.Now)

                With oGetCoinsuranceValuesResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        gvCoinsuranceValues.DataSource = .CoInsurers
                        gvCoinsuranceValues.DataBind()
                        Session("CoInsurers") = .CoInsurers

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

        End If
    End Sub


    Protected Sub btnCoInsurer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCoInsurer.Click
        Response.Write("<script>window.open('FindReinsurer.aspx','','scrollbars=1');</script>")
    End Sub

    Protected Sub btnnewOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnnewOk.Click
        pnlCoinsurance.Visible = False


        Dim oCoInsurers() As BaseGetCoinsuranceValuesResponseTypeRow
        oCoInsurers = DirectCast(Session("CoInsurers"), BaseGetCoinsuranceValuesResponseTypeRow())
        If Session("CoinsuranceEdit") = "New" Then
            If oCoInsurers IsNot Nothing Then
                ReDim Preserve oCoInsurers(oCoInsurers.Length)
            Else
                ReDim Preserve oCoInsurers(0)
            End If

            oCoInsurers(oCoInsurers.Length - 1) = New BaseGetCoinsuranceValuesResponseTypeRow
            oCoInsurers(oCoInsurers.Length - 1).ArrangementRef = txtArrangementRef.Text
            oCoInsurers(oCoInsurers.Length - 1).SharePerc = Convert.ToDouble(txtShare.Text)
            oCoInsurers(oCoInsurers.Length - 1).CommissionPerc = Convert.ToDouble(txtCommission.Text)
            oCoInsurers(oCoInsurers.Length - 1).CoInsurerKey = Session("ReinsurerKey")
            oCoInsurers(oCoInsurers.Length - 1).CoInsurer = txtCoinsurer.Text

            Session("CoInsurers") = oCoInsurers

            gvCoinsuranceValues.DataSource = oCoInsurers
            gvCoinsuranceValues.DataBind()
        Else

            oCoInsurers(gvCoinsuranceValues.SelectedIndex).ArrangementRef = txtArrangementRef.Text
            oCoInsurers(gvCoinsuranceValues.SelectedIndex).SharePerc = Convert.ToDouble(txtShare.Text)
            oCoInsurers(gvCoinsuranceValues.SelectedIndex).CommissionPerc = Convert.ToDouble(txtCommission.Text)
            oCoInsurers(gvCoinsuranceValues.SelectedIndex).CoInsurerKey = Session("ReinsurerKey")
            oCoInsurers(gvCoinsuranceValues.SelectedIndex).CoInsurer = txtCoinsurer.Text

            Session("CoInsurers") = oCoInsurers

            gvCoinsuranceValues.DataSource = oCoInsurers
            gvCoinsuranceValues.DataBind()

        End If



    End Sub

    Protected Sub btnNewCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewCancel.Click
        pnlCoinsurance.Visible = False


    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        pnlCoinsurance.Visible = True
        txtCoinsurer.Text = ""
        txtCommission.Text = ""
        txtShare.Text = ""
        txtArrangementRef.Text = ""
        Session("CoinsuranceEdit") = "New"
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'If gvCoinsuranceValues.Rows.Count > 0 Then
        '    Dim iPercent As Integer
        '    For i As Integer = 0 To gvCoinsuranceValues.Rows.Count - 1
        '        iPercent += gvCoinsuranceValues.Rows(i).Cells(4).Text
        '    Next
        '    If iPercent > 100 Then

        '        Exit Sub
        '    End If
        'End If
        If Convert.ToDecimal(lblTotalShare.Text) <= 100 Then

            Dim oUpdateCoinsuranceValuesRequest As New UpdateCoinsuranceValuesRequestType
            Dim oUpdateCoinsuranceValuesResponse As New UpdateCoinsuranceValuesResponseType

            Dim oCoInsurers() As BaseGetCoinsuranceValuesResponseTypeRow
            oCoInsurers = DirectCast(Session("CoInsurers"), BaseGetCoinsuranceValuesResponseTypeRow())

            Dim oUpdateCoinsurers() As BaseUpdateCoinsuranceValuesRequestTypeRow
            If oCoInsurers IsNot Nothing Then
                ReDim Preserve oUpdateCoinsurers(oCoInsurers.Length - 1)
                For Count As Integer = 0 To oCoInsurers.Length - 1
                    oUpdateCoinsurers(Count) = New BaseUpdateCoinsuranceValuesRequestTypeRow
                    oUpdateCoinsurers(Count).ArrangementRef = oCoInsurers(Count).ArrangementRef
                    oUpdateCoinsurers(Count).CoInsurerKey = oCoInsurers(Count).CoInsurerKey
                    oUpdateCoinsurers(Count).CommissionPerc = oCoInsurers(Count).CommissionPerc
                    oUpdateCoinsurers(Count).SharePerc = oCoInsurers(Count).SharePerc
                Next

                With oUpdateCoinsuranceValuesRequest
                    .BranchCode = "HeadOff"
                    .CoInsurers = oUpdateCoinsurers
                    .InsuranceFileKey = Session("InsuranceFileKey")
                    .IsRecovered = chkIsRecovered.Checked
                    .IsSurcharged = chkIsSurcharged.Checked
                    .TimeStamp = Session("TimeStamp")
                    .DefaultId = Convert.ToInt32(ddlDefaults.SelectedValue)
                End With
                Try
                    StartDate = Date.Now
                    oUpdateCoinsuranceValuesResponse = oSAM.UpdateCoinsuranceValues(oUpdateCoinsuranceValuesRequest)
                    WriteToLog(Session, "CoInsurance.aspx", "SAMForInsuranceV2", "UpdateCoinsuranceValues",StartDate, Date.Now)
                    With oUpdateCoinsuranceValuesResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                        Else
                            Session("TimeStamp") = .TimeStamp

                            Response.Redirect("GetListRisks_Risk.aspx")

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

            End If
        Else
            Response.Write("<script>alert('Share Percentage cannot be greater 100%')</script>")
            lblErrorMessage.Text = "Policy Under allocated"
        End If

    End Sub


    Protected Sub gvCoinsuranceValues_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvCoinsuranceValues.RowEditing
        gvCoinsuranceValues.EditIndex = e.NewEditIndex
        Dim oCoInsurers() As BaseGetCoinsuranceValuesResponseTypeRow
        oCoInsurers = DirectCast(Session("CoInsurers"), BaseGetCoinsuranceValuesResponseTypeRow())

        gvCoinsuranceValues.DataSource = oCoInsurers
        gvCoinsuranceValues.DataBind()
    End Sub

    Protected Sub gvCoinsuranceValues_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvCoinsuranceValues.RowUpdating

    End Sub

    Protected Sub gvCoinsuranceValues_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCoinsuranceValues.SelectedIndexChanged
        If gvCoinsuranceValues.SelectedIndex <> -1 Then
            btnEdit.Enabled = True
        Else
            btnEdit.Enabled = False
        End If
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        pnlCoinsurance.Visible = True
        Dim oCoInsurers() As BaseGetCoinsuranceValuesResponseTypeRow
        oCoInsurers = DirectCast(Session("CoInsurers"), BaseGetCoinsuranceValuesResponseTypeRow())
        txtArrangementRef.Text = oCoInsurers(gvCoinsuranceValues.SelectedIndex).ArrangementRef
        txtCoinsurer.Text = oCoInsurers(gvCoinsuranceValues.SelectedIndex).CoInsurer
        txtCommission.Text = oCoInsurers(gvCoinsuranceValues.SelectedIndex).CommissionPerc
        txtShare.Text = oCoInsurers(gvCoinsuranceValues.SelectedIndex).SharePerc
        Session("CoinsuranceEdit") = "Edit"

    End Sub

    Protected Sub gvCoinsuranceValues_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCoinsuranceValues.RowDataBound
        
        If e.Row.RowType = DataControlRowType.DataRow Then
            Total = Total + DirectCast(e.Row.DataItem, BaseGetCoinsuranceValuesResponseTypeRow).SharePerc
        End If
        lblTotalShare.Text = Total.ToString
    End Sub

    Protected Sub gvCoinsuranceValues_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvCoinsuranceValues.RowDeleting
        'Dim oSubAgents As BaseGetSubAgentsResponseTypeRow()
        Dim oTempcoinsurers As BaseGetCoinsuranceValuesResponseTypeRow()
        Dim oCoInsurers() As BaseGetCoinsuranceValuesResponseTypeRow
        oCoInsurers = DirectCast(Session("CoInsurers"), BaseGetCoinsuranceValuesResponseTypeRow())
        ' oSubAgents = DirectCast(Session("SubAgents"), BaseGetSubAgentsResponseTypeRow())

        For Count As Integer = 0 To oCoInsurers.Length - 1
            If Count <> e.RowIndex Then
                If oTempcoinsurers Is Nothing Then
                    ReDim Preserve oTempcoinsurers(0)
                Else
                    ReDim Preserve oTempcoinsurers(oTempcoinsurers.Length)
                End If
                oTempcoinsurers(oTempcoinsurers.Length - 1) = oCoInsurers(Count)
            Else
            End If

        Next
        Session("CoInsurers") = oTempcoinsurers
        gvCoinsuranceValues.DataSource = oTempcoinsurers
        gvCoinsuranceValues.DataBind()
    End Sub
End Class
