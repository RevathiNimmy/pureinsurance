Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Collections
Partial Class Work_Manager_Exposure_1_GetTasks
    Inherits System.Web.UI.Page
    Public ACDateRangeDesc As ListItemCollection
    Public ACListShowSystem As ListItemCollection
    Public ACListTaskType As ListItemCollection

    'For Task Status:
#Region "TastStatus Consts"
    Public Const ACListTaskTypeAll = "All"
    Public Const ACListTaskTypeNew = "New"
    Public Const ACListTaskTypeInProgress = "In Progress"
    Public Const ACListTaskTypeComplete = "Complete"
    Public Const ACListTaskTypeInComplete = "InComplete"
    Public Const ACListTaskTypeAllButComplete = "(Not Complete)"
#End Region

    'For System:
#Region "System Consts"
    Public Const ACListShowSystemOnly = "System"
    Public Const ACListShowSystemUser = "User"
    Public Const ACListShowSystemAll = "(All)"
#End Region

    'For Date Range  
#Region "Date Range Consts"
    Public Const ACDateRangeDescToday = "Today"
    Public Const ACDateRangeDescNext1 = "Tomorrow"
    Public Const ACDateRangeDescNext2 = "Next 2 Days"
    Public Const ACDateRangeDescNext3 = "Next 3 Days"
    Public Const ACDateRangeDescNext4 = "Next 4 Days"
    Public Const ACDateRangeDescNext5 = "Next 5 Days"
    Public Const ACDateRangeDescNext6 = "Next 6 Days"
    Public Const ACDateRangeDescNext7 = "Next 7 Days"
    Public Const ACDateRangeDescNext14 = "Next 14 Days"
    Public Const ACDateRangeDescNext28 = "Next 28 Days"
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

        'set up the proxy object
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")
        Dim oRequest As New GetWmTaskRequestType
        Dim oResponse As New GetWmTaskResponseType

        '''Binding Task group to ddlTaskGroup
        'DataBind to Dropdowns- Jigar
        If Not Page.IsPostBack Then
            BuildLists(oSAM, ddlUserGroup, STSListType.PMLookup, "PMUser_Group", "(All Groups)")
            BuildLists(oSAM, ddlUser, STSListType.PMLookup, "PMUser", "(All Users)")
        End If

    End Sub

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal strAll As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode

        Try
            oResponse = oSAM.GetList(oRequest)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Key"
                    objControl.DataBind()
                    objControl.Items.Insert(0, New ListItem(strAll, "0"))
                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
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

End Class
