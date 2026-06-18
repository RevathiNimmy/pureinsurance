Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class New_Business_SelectProduct
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken
    Dim oSAM As New SAMForInsuranceV2
    Dim StartDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        If Not Page.IsPostBack Then

            If Not Session("PolicyWording") Is Nothing Then
                With lstSelectedClause
                    .DataSource = DirectCast(Session("PolicyWording"), DocTemplate())
                    .DataTextField = "Description"
                    .DataValueField = "Code"
                    .DataBind()
                End With
            End If

            'JP Dim oRequest As New GetProductOrRiskClausesRequestType
            'JP Dim oResponse As New GetProductOrRiskClausesResponseType

            'JP  With oRequest
            'JP .BranchCode = Session("BranchCode")
            'JP .ClauseSelType = ClauseSelectionType.Product
            'JP .CurrentBranchCode = Session("BranchCode")
            'JP .IsDefault = False
            'JP .ProductOrRiskTypeCode = Session("ProductCode")
            'JP End With
            StartDate = Date.Now
            'JP oResponse = oSAM.GetProductOrRiskClauses(oRequest)
            WriteToLog(Session, "SelectClauses.aspx", "SAMForInsuranceV2", "GetProductOrRiskClauses", StartDate, Date.Now)

            'JP With oResponse
            'JP If .Errors IsNot Nothing Then
            'JP lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
            'JP Else
            'JP Dim oDocTemplates As DocTemplate()
            'JP ReDim oDocTemplates(.Documents.Length - 1)

            'JP For iDocCount As Integer = 0 To .Documents.Length - 1
            'JP oDocTemplates(iDocCount) = New DocTemplate
            'JP oDocTemplates(iDocCount).Code = .Documents(iDocCount).Code
            'JP oDocTemplates(iDocCount).Description = .Documents(iDocCount).Description
            'JP Next
            'JP With lstAvailableClause
            'JP .DataSource = oDocTemplates
            'JP .DataTextField = "Description"
            'JP .DataValueField = "Code"
            'JP .DataBind()
            'JP End With
            For Each oListItem As ListItem In lstSelectedClause.Items
                lstAvailableClause.Items.Remove(oListItem)
            Next


        End If
        'JP End With

        'JP End If


    End Sub

   
    Protected Sub lstSelectedClause_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSelectedClause.DataBound

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If lstAvailableClause.SelectedIndex <> -1 Then
            Dim oList As New ListItem
            oList.Text = lstAvailableClause.SelectedItem.Text
            oList.Value = lstAvailableClause.SelectedItem.Value

            lstAvailableClause.Items.Remove(oList)
            lstSelectedClause.Items.Add(oList)

            Dim oDocTemplates As DocTemplate()
            ReDim oDocTemplates(lstSelectedClause.Items.Count - 1)

            For iDocCount As Integer = 0 To lstSelectedClause.Items.Count - 1
                oDocTemplates(iDocCount) = New DocTemplate
                oDocTemplates(iDocCount).Code = lstSelectedClause.Items(iDocCount).Value
                oDocTemplates(iDocCount).Description = lstSelectedClause.Items(iDocCount).Text
            Next
            Session("PolicyWording") = oDocTemplates


        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If lstSelectedClause.SelectedIndex <> -1 Then
            Dim oList As New ListItem
            oList.Text = lstSelectedClause.SelectedItem.Text
            oList.Value = lstSelectedClause.SelectedItem.Value

            lstSelectedClause.Items.Remove(oList)
            lstAvailableClause.Items.Add(oList)

            Dim oDocTemplates As DocTemplate()
            ReDim oDocTemplates(lstSelectedClause.Items.Count - 1)

            For iDocCount As Integer = 0 To lstSelectedClause.Items.Count - 1
                oDocTemplates(iDocCount) = New DocTemplate
                oDocTemplates(iDocCount).Code = lstSelectedClause.Items(iDocCount).Value
                oDocTemplates(iDocCount).Description = lstSelectedClause.Items(iDocCount).Text
            Next
            Session("PolicyWording") = oDocTemplates
        End If
    End Sub

 

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click

    End Sub
End Class

