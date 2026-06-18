Imports Nexus.Constants.Session
Imports Nexus.Constants

Namespace Nexus

    Partial Class Controls_ReportControls_Diary : Inherits BaseReport

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If IsPostBack Then
                'set default value of 'DateExtracedTo' to current date.
                RP__END_DATE.Text = Date.Now.ToShortDateString()
                'set validations for 'DateExtracedTo'.
                rngvldEndDate.MinimumValue = Date.MinValue.ToShortDateString()
                rngvldEndDate.MaximumValue = Date.MaxValue.ToShortDateString()


                FillAvailableUserName()
            End If
        End Sub
        ''' <summary>
        ''' fill Username in the drop down list
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillAvailableUserName()
            'fill product code in the drop down list
            'clear all items in the drop down list
            RP__USERNAME.Items.Clear()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUser As NexusProvider.UserCollection = Nothing
            oUser = oWebService.GetUserGroupUsers(Nothing, DateTime.Now, False, False)
            RP__USERNAME.DataSource = oUser
            RP__USERNAME.DataTextField = "UserName"
            RP__USERNAME.DataValueField = "UserName" '"userid"
            RP__USERNAME.DataBind()
            'set default option as "All" at zero index if user has given in resource file
            If GetLocalResourceObject("ddl_UserName_defaulttext").ToString().Trim.Length <> 0 Then
                'client can change 'text' to be displayed but can't change the value
                RP__USERNAME.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_UserName_defaulttext"), "ALL"))
            End If


        End Sub
    End Class

End Namespace

