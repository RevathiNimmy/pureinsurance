Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Partial Class Lookup_Screens_FindDocumentTemplates
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim oSAM As New SAMForInsuranceV2
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object

        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType

        'Dim oRes As New FindDocumentTemplatesResponseType


        If Not (Page.IsPostBack) Then

            'Binding to ddlType
            oRequest.BranchCode = "HeadOff"
            oRequest.ListType = STSListType.PMLookup
            oRequest.ListCode = "document_type"

            Try
                StartDate = Date.Now
                oResponse = oSAM.GetList(oRequest)
                WriteToLog(Session, "FindDocumentTemplates.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
                With oResponse
                    If Not (.Errors) Is Nothing Then
                        Response.Write(GetMessageFromSamError(.Errors))
                    Else
                        ddlType.DataSource = oResponse.List
                        ddlType.DataTextField = "Description"
                        ddlType.DataValueField = "Code"
                        ddlType.DataBind()
                    End If
                End With

                Populate()

            Catch os As SamResponseException
                'should do some error handling here. Just output error for now
                Response.Write("An error occured calling SAM:<br>" & os.Message)

            Catch oe As Exception
                'should do some error handling here. Just output error for now
                Response.Write("An error occured:<br>" & oe.Message)
            End Try
        End If
    End Sub
    Private Sub Populate()
        'Binding to GridView


        Dim oFindDocumentTemplatesRequestType As New FindDocumentTemplatesRequestType()
        Dim oFindDocumentTemplatesResponseType As New FindDocumentTemplatesResponseType()
        oFindDocumentTemplatesRequestType.BranchCode = "HeadOff"
        oFindDocumentTemplatesRequestType.ProductCode = DirectCast(Session("SelectedProduct"), ListItem).Value
        oFindDocumentTemplatesRequestType.Code = txtCode.Text

        If txtEffectiveDate.Text <> "" Then

            oFindDocumentTemplatesRequestType.EffectiveDateSpecified = True
            oFindDocumentTemplatesRequestType.EffectiveDate = txtEffectiveDate.Text
        Else
            oFindDocumentTemplatesRequestType.EffectiveDateSpecified = False
        End If


        oFindDocumentTemplatesRequestType.TypeCode = ddlType.SelectedValue
        StartDate = Date.Now
        oFindDocumentTemplatesResponseType = oSAM.FindDocumentTemplates(oFindDocumentTemplatesRequestType)
        WriteToLog(Session, "FindDocumentTemplates.aspx", "SAMForInsuranceV2", "FindDocumentTemplates",StartDate, Date.Now)
        With oFindDocumentTemplatesResponseType
            If Not .Errors Is Nothing Then
                Response.Write(GetMessageFromSamError(.Errors))

            Else
                Session("FindDocumentTemplate") = oFindDocumentTemplatesResponseType
                gvDoctemplate.DataSource = oFindDocumentTemplatesResponseType.DocumentTemplates
                gvDoctemplate.DataBind()
            End If
        End With
    End Sub

    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        Populate()
    End Sub

    Protected Sub gvDoctemplate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDoctemplate.SelectedIndexChanged
        Dim oBaseFindDocumentTemplateRow As New BaseFindDocumentTemplatesResponseTypeRow

        Dim oDocumentTemplates As BaseGetStandardPolicyWordingsResponseTypeRow() = DirectCast(Session("PolicyWording"), BaseGetStandardPolicyWordingsResponseTypeRow())
        Dim oFindDocumentTemplateResponse As FindDocumentTemplatesResponseType = DirectCast(Session("FindDocumentTemplate"), FindDocumentTemplatesResponseType)


        If oDocumentTemplates Is Nothing Then
            ReDim Preserve oDocumentTemplates(0)
        Else
            ReDim Preserve oDocumentTemplates(oDocumentTemplates.Length)
        End If

        oDocumentTemplates(oDocumentTemplates.Length - 1) = New BaseGetStandardPolicyWordingsResponseTypeRow
        oDocumentTemplates(oDocumentTemplates.Length - 1).Code = oFindDocumentTemplateResponse.DocumentTemplates(gvDoctemplate.SelectedIndex).Code
        oDocumentTemplates(oDocumentTemplates.Length - 1).Description = oFindDocumentTemplateResponse.DocumentTemplates(gvDoctemplate.SelectedIndex).Description
        oDocumentTemplates(oDocumentTemplates.Length - 1).DocumentTemplateId = oFindDocumentTemplateResponse.DocumentTemplates(gvDoctemplate.SelectedIndex).DocumentTemplateKey
        Session("PolicyWording") = oDocumentTemplates

    End Sub
End Class
