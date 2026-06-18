Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.Web.Services3.Security.Tokens
Partial Class Work_Manager_Exposure_WMTaskLog
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim str As String = Request.QueryString("disable")
        If str = "YES" Then btnAdd.Enabled = False
        If Not Page.IsPostBack Then
            fetchdetails()
        End If


    End Sub

    'Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click

    '    Try

    '        Dim usertoken As UsernameToken = SAMHelper.GetUserToken("sirius", "sirius")
    '        Dim oSAM As New SAMForInsuranceV2()
    '        oSAM.SetClientCredential(usertoken)
    '        oSAM.SetPolicy("SamClientPolicy")
    '        Dim objgetwmreq As New AddWmTaskLogRequestType()
    '        Dim objgetwmres As New AddWmTaskLogResponseType()
    '        objgetwmreq.BranchCode = "HeadOff"
    '        Dim TaskInstanceKey As Integer
    '        TaskInstanceKey = Session("TaskInstanceKey") '1170
    '        objgetwmreq.TaskInstanceKey = TaskInstanceKey
    '        objgetwmreq.LogText = TextBox1.Text.ToString()
    '        objgetwmres = oSAM.AddWmTaskLog(objgetwmreq)


    '        If objgetwmres.Errors IsNot Nothing Then

    '            Throw New SamResponseException(objgetwmres.Errors)
    '        End If


    '        fetchdetails()
    '    Catch os As SamResponseException


    '        Response.Write("An error occured calling SAM:<br>" + os.Message)
    '    Catch oe As Exception
    '        Response.Write("An error occured:<br>" + oe.Message)
    '    Finally


    '    End Try


    'End Sub

    Private Sub fetchdetails()

        Try
            Dim usertoken As UsernameToken = SAMHelper.GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2()
            oSAM.SetClientCredential(usertoken)
            oSAM.SetPolicy("SamClientPolicy")


            Dim objgetwmreq As New GetWmTaskLogRequestType()
            Dim objgetwmres As New GetWmTaskLogResponseType()

            Dim TaskInstanceKey As Integer
            TaskInstanceKey = Session("TaskInstanceKey")

            objgetwmreq.BranchCode = "HeadOff"
            objgetwmreq.TaskInstanceKey = TaskInstanceKey
            objgetwmres = oSAM.GetWmTaskLog(objgetwmreq)


            If objgetwmres.Errors IsNot Nothing Then

                Throw New SamResponseException(objgetwmres.Errors)
            End If


            GridView1.DataSource = objgetwmres.TaskLog
            GridView1.DataBind()
        Catch os As SamResponseException


            Response.Write("An error occured calling SAM:<br>" + os.Message)
        Catch oe As Exception
            Response.Write("An error occured:<br>" + oe.Message)
        Finally


        End Try


    End Sub

    
    

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Dim usertoken As UsernameToken = SAMHelper.GetUserToken("sirius", "sirius")
            Dim oSAM As New SAMForInsuranceV2()
            oSAM.SetClientCredential(usertoken)
            oSAM.SetPolicy("SamClientPolicy")
            Dim objgetwmreq As New AddWmTaskLogRequestType()
            Dim objgetwmres As New AddWmTaskLogResponseType()
            objgetwmreq.BranchCode = "HeadOff"
            Dim TaskInstanceKey As Integer
            TaskInstanceKey = Session("TaskInstanceKey") '1170
            objgetwmreq.TaskInstanceKey = TaskInstanceKey
            objgetwmreq.LogText = TextBox1.Text.ToString()
            objgetwmres = oSAM.AddWmTaskLog(objgetwmreq)


            If objgetwmres.Errors IsNot Nothing Then

                Throw New SamResponseException(objgetwmres.Errors)
            End If


            fetchdetails()
        Catch os As SamResponseException


            Response.Write("An error occured calling SAM:<br>" + os.Message)
        Catch oe As Exception
            Response.Write("An error occured:<br>" + oe.Message)
        Finally


        End Try
    End Sub
End Class
