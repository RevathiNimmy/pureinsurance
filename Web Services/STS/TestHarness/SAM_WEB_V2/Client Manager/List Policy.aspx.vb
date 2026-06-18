Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_List_Policy
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oGetPartySummaryRequest As New GetPartySummaryRequestType
            Dim oGetPartySummaryResponseType As GetPartySummaryResponseType

            With oGetPartySummaryRequest
                .BranchCode = "HEADOFF" ' Session("BRANCHCODE").ToString()
                .PartyKey = Convert.ToInt32(Session("PARTYKEY"))
            End With

            Try
                oGetPartySummaryResponseType = oSAM.GetPartySummary(oGetPartySummaryRequest)

                With oGetPartySummaryResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else

                        txtClientCode.Text = oGetPartySummaryResponseType.Policies(0).PartyShortName
                        Dim dsPolicies As Data.DataSet = Nothing
                        dsPolicies = New Data.DataSet

                        dsPolicies.Tables.Add()
                        dsPolicies.Tables(0).Columns.Add("PolicyRef", GetType(System.String))
                        dsPolicies.Tables(0).Columns.Add("ProductDesc", GetType(System.String))
                        dsPolicies.Tables(0).Columns.Add("RiskTypeDescription", GetType(System.String))
                        dsPolicies.Tables(0).Columns.Add("InsuranceFolderKey", GetType(System.Int32))
                        dsPolicies.Tables(0).Columns.Add("InsuranceFileTypeCode", GetType(System.String))



                        Dim iCount As Integer = 0
                        With dsPolicies.Tables(0)
                            For iCount = 0 To oGetPartySummaryResponseType.Policies.Length - 1
                                .Rows.Add()
                                .Rows(iCount).Item("PolicyRef") = oGetPartySummaryResponseType.Policies(iCount).PolicyRef
                                .Rows(iCount).Item("ProductDesc") = oGetPartySummaryResponseType.Policies(iCount).ProductDesc
                                .Rows(iCount).Item("RiskTypeDescription") = oGetPartySummaryResponseType.Policies(iCount).RiskTypeDescription
                                .Rows(iCount).Item("InsuranceFolderKey") = oGetPartySummaryResponseType.Policies(iCount).InsuranceFolderKey
                                .Rows(iCount).Item("InsuranceFileTypeCode") = oGetPartySummaryResponseType.Policies(iCount).InsuranceFileTypeCode

                            Next

                        End With

                       
                        Dim dtbl As Data.DataTable



                        Dim dv As New Data.DataView(dsPolicies.Tables(0))

                        dv.RowFilter = "InsuranceFileTypeCode='Policy'"

                        'gvResult.DataSource = dv
                        'gvResult.DataBind()

                        dtbl = dv.ToTable(True, "PolicyRef", "ProductDesc", "RiskTypeDescription", "InsuranceFolderKey", "InsuranceFileTypeCode")
                        Dim dv4 As New Data.DataView(dtbl)
                        gvResult.DataSource = dv4
                        gvResult.DataBind()


                        dtbl = dv.ToTable(True, "PolicyRef")

                        Dim dv1 As New Data.DataView(dsPolicies.Tables(0))

                        dv1.RowFilter = "InsuranceFileTypeCode='QUOTE'"

                        GridView1.DataSource = dv1
                        GridView1.DataBind()

                        Dim dv2 As New Data.DataView(dsPolicies.Tables(0))


                        dv2.RowFilter = "InsuranceFileTypeCode='Renewal'"

                        GridView2.DataSource = dv2
                        GridView2.DataBind()


                        '  MTA Quotation Permanent


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
    End Sub

    Dim oGetAllPolicyVersionsRequest As New GetAllPolicyVersionsRequestType
    Dim oGetAllPolicyVersionsResponseType As GetAllPolicyVersionsResponseType

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If (e.CommandName.Equals("Select")) Then
            Session("INSURANCEFOLDERKEY") = e.CommandArgument
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")



            With oGetAllPolicyVersionsRequest
                .BranchCode = "Headoff"
                .InsuranceFolderKey = Session("InsuranceFolderKey")
            End With

            Try
                oGetAllPolicyVersionsResponseType = oSAM.GetAllPolicyVersions(oGetAllPolicyVersionsRequest)

                With oGetAllPolicyVersionsResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        txtClientCode.Text = oGetAllPolicyVersionsResponseType.Policies(0).PartyShortName
                        GridView3.DataSource = oGetAllPolicyVersionsResponseType.Policies
                        GridView3.DataBind()
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
        'gvResult.SelectedRow.Cells(4).Text
        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Try
            oGetHeaderAndRisksByKeyRequestType.BranchCode = Session("BRANCHCODE").ToString()
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = oGetAllPolicyVersionsResponseType.Policies(0).insuranceFileKey  'Convert.ToInt32(Session("INSURANCEFILEKEY"))


            Dim oSAM As New SAMForInsuranceV2

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)

            If Not (oGetHeaderAndRisksByKeyResponseType.Errors) Is Nothing Then
                Throw New SamResponseException(oGetHeaderAndRisksByKeyResponseType.Errors)
            End If

            grdLiskRisk.DataSource = oGetHeaderAndRisksByKeyResponseType.Risks
            Session("InsuranceFileKey") = oGetHeaderAndRisksByKeyResponseType.InsuranceFileKey
            grdLiskRisk.DataBind()
        Catch ex As Exception
            'lblError.Text = "Error occured: " + ex.Message
        End Try
    End Sub

    Protected Sub GridView2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView2.RowCommand
        If (e.CommandName.Equals("Select")) Then
            Session("INSURANCEFOLDERKEY") = e.CommandArgument
            'Response.Redirect("ListPolicyVersions.aspx")
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")



            With oGetAllPolicyVersionsRequest
                .BranchCode = "Headoff"
                .InsuranceFolderKey = Session("InsuranceFolderKey")
            End With

            Try
                oGetAllPolicyVersionsResponseType = oSAM.GetAllPolicyVersions(oGetAllPolicyVersionsRequest)

                With oGetAllPolicyVersionsResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Throw New SamResponseException(.Errors)
                    Else
                        txtClientCode.Text = oGetAllPolicyVersionsResponseType.Policies(0).PartyShortName
                        GridView3.DataSource = oGetAllPolicyVersionsResponseType.Policies
                        GridView3.DataBind()
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

        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Try

            oGetHeaderAndRisksByKeyRequestType.BranchCode = Session("BRANCHCODE").ToString()
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = oGetAllPolicyVersionsResponseType.Policies(0).insuranceFileKey  'Convert.ToInt32(Session("INSURANCEFILEKEY"))


            Dim oSAM As New SAMForInsuranceV2

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)

            If Not (oGetHeaderAndRisksByKeyResponseType.Errors) Is Nothing Then
                Throw New SamResponseException(oGetHeaderAndRisksByKeyResponseType.Errors)
            End If

            grdLiskRisk.DataSource = oGetHeaderAndRisksByKeyResponseType.Risks
            Session("InsuranceFileKey") = oGetHeaderAndRisksByKeyResponseType.InsuranceFileKey
            grdLiskRisk.DataBind()
        Catch ex As Exception
            'lblError.Text = "Error occured: " + ex.Message
        End Try
    End Sub

    Protected Sub GridView3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView3.RowCommand
        If (e.CommandName.Equals("Select")) Then
            Session("INSURANCEFILEKEY") = e.CommandArgument
        End If
    End Sub
    Private Sub PopulatePolicyListGrid(ByVal oGetAllPolicyVersionsResponseType As GetHeaderAndRiskFeesByKeyResponseType)

    End Sub

    Protected Sub gvResult_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.SelectedIndexChanged
        
    End Sub

    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        If (e.CommandName.Equals("Select")) Then
            Session("INSURANCEFOLDERKEY") = e.CommandArgument
        End If
        Dim oGetPartySummaryRequest As New GetPartySummaryRequestType
        Dim oGetPartySummaryResponseType As GetPartySummaryResponseType
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim oGetAllPolicyVersionsRequest As New GetAllPolicyVersionsRequestType
        Dim oGetAllPolicyVersionsResponseType As GetAllPolicyVersionsResponseType

        With oGetAllPolicyVersionsRequest
            .BranchCode = "Headoff"
            .InsuranceFolderKey = Session("InsuranceFolderKey")
        End With

        Try
            oGetAllPolicyVersionsResponseType = oSAM.GetAllPolicyVersions(oGetAllPolicyVersionsRequest)

            With oGetAllPolicyVersionsResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    txtClientCode.Text = oGetAllPolicyVersionsResponseType.Policies(0).PartyShortName
                    GridView3.DataSource = oGetAllPolicyVersionsResponseType.Policies
                    GridView3.DataBind()
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
        Dim oGetHeaderAndRisksByKeyRequestType As New GetHeaderAndRisksByKeyRequestType
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Try
            oGetHeaderAndRisksByKeyRequestType.BranchCode = Session("BRANCHCODE").ToString()
            oGetHeaderAndRisksByKeyRequestType.InsuranceFileKey = oGetAllPolicyVersionsResponseType.Policies(0).insuranceFileKey
            oGetHeaderAndRisksByKeyResponseType = oSAM.GetHeaderAndRisksByKey(oGetHeaderAndRisksByKeyRequestType)

            If Not (oGetHeaderAndRisksByKeyResponseType.Errors) Is Nothing Then
                Throw New SamResponseException(oGetHeaderAndRisksByKeyResponseType.Errors)
            End If
            grdLiskRisk.DataSource = oGetHeaderAndRisksByKeyResponseType.Risks
            Session("InsuranceFileKey") = oGetHeaderAndRisksByKeyResponseType.InsuranceFileKey
            grdLiskRisk.DataBind()
        Catch ex As Exception
            'lblError.Text = "Error occured: " + ex.Message
        End Try

    End Sub
   

    Protected Sub grdLiskRisk_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdLiskRisk.RowCommand
       
    End Sub

    Protected Sub GridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView3.SelectedIndexChanged
        Session("COVERFROM") = GridView3.SelectedRow.Cells(5).Text
        Session("COVERTO") = GridView3.SelectedRow.Cells(6).Text
        If (GridView3.SelectedRow.Cells(3).Text = "Live") Then
            Session("PROCESS") = "MTA"
            Response.Redirect("PolicyHeader.aspx")
        Else
            Session("PROCESS") = "NONMTA"
            Response.Redirect("PolicyHeader.aspx")
        End If
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        
    End Sub

    Protected Sub grdLiskRisk_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdLiskRisk.SelectedIndexChanged
        Session("RiskKey") = grdLiskRisk.SelectedRow.Cells(8).Text
        Dim oGetHeaderAndRisksByKeyResponseType As New GetHeaderAndRisksByKeyResponseType
        Response.Redirect("RiskPremiumDetails.aspx")
    End Sub
End Class
