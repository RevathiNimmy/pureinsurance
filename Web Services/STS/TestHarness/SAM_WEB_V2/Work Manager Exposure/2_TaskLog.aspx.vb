
Partial Class Work_Manager_Exposure_3_TaskLog
    Inherits System.Web.UI.Page

    'Protected Sub btnAddTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTask.Click
    '    TaskLogView.ActiveViewIndex = 0
    'End Sub

    'Protected Sub btnViewTask_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewTask.Click
    '    TaskLogView.ActiveViewIndex = 1

    'End Sub

    Protected Sub MnuTaskLog_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles MnuTaskLog.MenuItemClick
        TaskLogView.ActiveViewIndex = Convert.ToInt32(e.Item.Value)
    End Sub
End Class
