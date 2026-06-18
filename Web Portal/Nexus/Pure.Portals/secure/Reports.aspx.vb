Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports System.Collections.Generic

Namespace Nexus

    Partial Class secure_Reports : Inherits Frontend.clsCMSPage

        ''' <summary>
        ''' This method will be called on page load event of Reports.aspx
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Page.SetFocus(ddlReportsType)

            If Not IsPostBack Then

                'fill "ddlReportsType" with name of configured "Reports" from web.config
                Dim oReports As Config.Reports = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Reports

                For Each oReport As Config.Report In oReports
                    ddlReportsType.Items.Add(New ListItem(oReport.Display, oReport.Name))
                Next
                SortDDL(ddlReportsType)
                'default selected value, if text is available in resource file
                If GetLocalResourceObject("ddl_ReportsType_defaulttext").ToString().Trim.Length <> 0 Then
                    ddlReportsType.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_ReportsType_defaulttext"), ""))
                End If

            ElseIf IsPostBack Then

                'load the configured control for the selected Report into the placeholder 
                Dim sReportNameSelected As String = ddlReportsType.SelectedValue.ToString

                'get the location\name of control for selected Report
                Dim oReportControl As Config.Report = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).Reports.Report(sReportNameSelected)

                If oReportControl IsNot Nothing Then
                    'assign the control location to string
                    Dim WebControlPath As String = oReportControl.Control.ToString

                    'check the existance of the contol on the given location and load to placeholder if exists
                    If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                        plcReportForm.Visible = True

                        Dim tempControl As Control = LoadControl(WebControlPath)
                        plcReportForm.Controls.Clear()
                        plcReportForm.Controls.Add(tempControl)
                    End If
                End If

            End If

        End Sub
        Private Sub SortDDL(ByRef objDDL As DropDownList)
            Dim textList As ArrayList = New ArrayList()
            Dim valueList As ArrayList = New ArrayList()


            For Each li As ListItem In objDDL.Items
                textList.Add(li.Text)
            Next

            textList.Sort()


            For Each item In textList
                Dim value As String = objDDL.Items.FindByText(item.ToString()).Value
                valueList.Add(value)
            Next
            objDDL.Items.Clear()

            For i = 0 To textList.Count - 1
                Dim objItem As ListItem = New ListItem(textList(i).ToString(), valueList(i).ToString())
                objDDL.Items.Add(objItem)
            Next
        End Sub


    End Class

End Namespace