Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class New_Business_SelectProduct
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")


        If Not Page.IsPostBack Then


            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oRequest As New GetListRequestType
            Dim oResponse As New GetListResponseType

            With oRequest
                .BranchCode = "HeadOff"
                .ListCode = "Product"
                .ListType = STSListType.PMLookup
            End With
            Try
                
                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "SelectProduct.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    Else
                        lstProducts.DataSource = oResponse.List
                        lstProducts.DataTextField = "Description"
                        lstProducts.DataValueField = "Code"
                        lstProducts.DataBind()
                    End If
                End With
                lstProducts.SelectedIndex = 0


            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)
            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)
            Finally
                'clean
            End Try
            BuildLists(oSAM, ddlBranch, STSListType.PMLookup, "Source")

            If ddlBranch.Items.Count > 0 Then
                ddlBranch.SelectedValue = "HeadOff"
                'ddlBranch.SelectedIndex = 6
            End If

        End If


    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBranch.SelectedIndexChanged

    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode


        Try
            
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "SelectProduct.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Code"
                    objControl.DataBind()

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

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Session("SelectedProduct") = lstProducts.SelectedItem
        Session("ProductCode") = lstProducts.SelectedValue
        Session("BranchCode") = ddlBranch.SelectedValue
        Response.Redirect("PolicyHeader.aspx")
    End Sub

    Protected Sub lstProducts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstProducts.SelectedIndexChanged

    End Sub
End Class

