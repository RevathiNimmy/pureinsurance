Imports Microsoft.Web.Services3.Security.Tokens
Partial Class PolicyRenewal_wfrmFilterRenewals
    Inherits System.Web.UI.Page
      Dim StartDate As Date

     
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblOutput.Text = ""
        If (Not IsPostBack) Then
            calRenewalDate.Visible = chkRenewalDate.Checked
            calRenewalDate.SelectedDate = Today.Date
            PopulateBranchCode()
            PopulateProductCode()
        End If
        txtAgentCode.Attributes.Add("ReadOnly", "True")
        txtPartyShortName.Attributes.Add("ReadOnly", "True")
    End Sub

    'For the time being the cancel button is made as html button and uses window.history.back method. but it is not 
    ' a good solution. if we want to make the cancel as asp.net button with server handlin the back button functionality,
    'use this code
    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    'If Session("SourcePage") IsNot Nothing Then
    '    'Response.Redirect(Convert.ToString(Session("SourcePage")))
    '    'Else
    '    'Response.Write("<script> window.history.back();</script>")
    '    'End If
    'End Sub
    Private Sub PopulateBranchCode()
        ddlBranchCode.Items.Clear()
        ddlBranchCode.Items.Add("")
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetListRequestType As New GetListRequestType
        Dim oGetListResponseType As New GetListResponseType

        'set up request object with some values
        With oGetListRequestType
            .ListCode = "Source"
            .ListType = STSListType.PMLookup
            .BranchCode = "HeadOff"
        End With

        Try
            StartDate = Date.Now
            oGetListResponseType = oSAM.GetList(oGetListRequestType)
            WriteToLog(Session, "wtrmFilterRenewals.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oGetListResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                ddlBranchCode.DataSource = oGetListResponseType.List
                ddlBranchCode.DataTextField = "Description"
                ddlBranchCode.DataValueField = "Code"
                ddlBranchCode.DataBind()
            End With


        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
    End Sub

    Private Sub PopulateProductCode()
        ddlProductCode.Items.Clear()
        ddlProductCode.Items.Add(New ListItem("All", 0))
        'create user token from credentials
        'normally the credentials would come from the log in
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'create the request and response objects
        Dim oGetListRequestType As New GetListRequestType
        Dim oGetListResponseType As New GetListResponseType

        'set up request object with some values
        With oGetListRequestType
            .ListCode = "Product"
            .ListType = STSListType.PMLookup
            .BranchCode = "HeadOff"
        End With

        Try
            StartDate = Date.Now
            oGetListResponseType = oSAM.GetList(oGetListRequestType)
            WriteToLog(Session, "wfrmFilterRenewals.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)

            With oGetListResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
                ddlProductCode.DataSource = oGetListResponseType.List
                ddlProductCode.DataTextField = "Description"
                ddlProductCode.DataValueField = "Code"
                ddlProductCode.DataBind()
            End With


        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured calling SAM:<br>" & os.Message
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            lblOutput.Visible = True
            lblOutput.Text = "An error occured:<br>" & oe.Message
        Finally
            'clean up any objects here
        End Try
    End Sub
    
    'Protected Sub ddlBranchCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBranchCode.SelectedIndexChanged
    '    Session("BranchCode") = ddlBranchCode.SelectedValue.Trim
    '    'PopulateAgentCode()
    'End Sub

   
    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim bAgentProvided As Boolean
        Dim bPartyProvided As Boolean
        If ddlBranchCode.SelectedIndex > 0 Then
            Session("BranchCode") = ddlBranchCode.SelectedValue
        Else
            Session("BranchCode") = "HeadOff"
        End If
        If ddlProductCode.SelectedIndex > 0 Then
            Session("ProductCode") = ddlProductCode.SelectedValue
        Else
            Session("ProductCode") = Nothing
        End If


        If Not String.IsNullOrEmpty(txtAgentCode.Text) AndAlso txtAgentCode.Text.Trim <> "" Then
            bAgentProvided = True
        Else
            Session("AgentKey") = Nothing
        End If
        If Not String.IsNullOrEmpty(txtPartyShortName.Text) AndAlso txtPartyShortName.Text.Trim <> "" Then
            bPartyProvided = True
        Else
            Session("PartyKey") = Nothing
        End If
        If chkRenewalDate.Checked Then
            Session("RenewalDate") = calRenewalDate.SelectedDate
        Else
            Session("RenewalDate") = Nothing
        End If
        If chkDirectBusiness.Checked Then
            Session("AgentKey") = Nothing
            Session("OnlyDirect") = True
        Else
            Session("OnlyDirect") = Nothing
        End If
        If Not bAgentProvided AndAlso Not bPartyProvided AndAlso Not chkDirectBusiness.Checked Then
            lblOutput.Text = "Either Agent Code or Party Short Name should contain a valid entry"
        Else
            If Session("Process") IsNot Nothing Then
                Dim strTarget As String = Session("Process").ToString()
                If strTarget.Equals("Amend") Then
                    Response.Redirect("wfrmRenewals_Amend.aspx")
                ElseIf strTarget.Equals("Accept") Then
                    Response.Redirect("wfrmRenewals_Accept.aspx")
                End If
            Else
                lblOutput.Text = "Incorrect flow of process. Start from wfrmRenewalAmendment.aspx page "
            End If
        End If

    End Sub

 
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("~/UIIC_DEMO/HomePage.aspx")
    End Sub

    Protected Sub chkRenewalDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRenewalDate.CheckedChanged
        calRenewalDate.Visible = chkRenewalDate.Checked
    End Sub
End Class
