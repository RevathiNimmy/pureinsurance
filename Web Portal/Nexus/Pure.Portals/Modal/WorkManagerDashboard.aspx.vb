Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml.Linq
Imports Nexus.Constants.Session
Imports Nexus.Constants
Imports NexusProvider

Namespace Nexus
    Partial Class Modal_WorkManagerDashboard
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not IsPostBack Then
                LoadFilters()
            End If
            BindDashboard()
        End Sub

        Private Sub LoadFilters()
            Try
                Dim oWebService As ProviderBase = New ProviderManager().Provider
                Dim oUserDetails As UserDetails = oWebService.GetUserDetails(HttpContext.Current.User.Identity.Name)

                If oUserDetails.ListOfBranches IsNot Nothing AndAlso oUserDetails.ListOfBranches.Count > 0 Then
                    rptBranches.DataSource = oUserDetails.ListOfBranches
                    rptBranches.DataBind()
                End If

                If oUserDetails.AvailableUsergroups IsNot Nothing AndAlso oUserDetails.AvailableUsergroups.Count > 0 Then
                    Dim oActiveGroups As New UserGroupCollection()
                    For Each oGroup As UserGroup In oUserDetails.AvailableUsergroups
                        If Not oGroup.IsDeleted Then oActiveGroups.Add(oGroup)
                    Next
                    ddlUserGroup.DataSource = oActiveGroups
                    ddlUserGroup.DataTextField = "Description"
                    ddlUserGroup.DataValueField = "UserGroupKey"
                    ddlUserGroup.DataBind()
                    If oActiveGroups.Count > 1 Then
                        ddlUserGroup.Items.Insert(0, New ListItem("All Groups", ""))
                    Else
                        ddlUserGroup.Enabled = False
                    End If
                End If

                ddlDateRange.DataSource = [Enum].GetNames(GetType(NexusProvider.DateRange))
                ddlDateRange.DataBind()
                ddlDateRange.SelectedIndex = 0
            Catch ex As Exception
            End Try
        End Sub

        Private Function CallSPWithCache(oWebservice As ProviderBase, params() As StoredProcedureParameterType, spName As String) As StoredProcedureResponseType
            Dim cacheKey As String = spName
            For Each p As StoredProcedureParameterType In params
                cacheKey &= "_" & p.ParamName & "=" & If(p.ParamValue, "")
            Next

            Dim cached As StoredProcedureResponseType = TryCast(HttpContext.Current.Cache(cacheKey), StoredProcedureResponseType)
            If cached IsNot Nothing Then
                Return cached
            End If

            Dim result As StoredProcedureResponseType = oWebservice.CallNamedStoredProcedure(params, spName)

            If result IsNot Nothing Then
                HttpContext.Current.Cache.Insert(cacheKey, result, Nothing, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration)
            End If

            Return result
        End Function

        Private sChartScripts As String = ""

        Private Sub BindDashboard()
            Dim oWebservice As ProviderBase = New ProviderManager().Provider
            Dim sGroupIds As String = If(String.IsNullOrEmpty(ddlUserGroup.SelectedValue), "", ddlUserGroup.SelectedValue)

            ' Collect selected branch IDs from hidden field
            Dim sBranchIds As String = If(String.IsNullOrEmpty(hdnSelectedBranches.Value), "", hdnSelectedBranches.Value)

            Dim iDateRange As String = ddlDateRange.SelectedIndex.ToString()
            sChartScripts = "var perfData={percent:0};var trendData={categories:[],currentWeek:[],previousWeek:[]};var groupData={labels:[],series:[]};var monthlyData={categories:[],totalTasks:[],completedTasks:[]};"

            Try
                BindSummary(oWebservice, sGroupIds, sBranchIds, iDateRange)
            Catch ex As Exception
            End Try
            Try
                BindTrend(oWebservice, sGroupIds, sBranchIds)
            Catch ex As Exception
            End Try
            Try
                BindTasksByGroup(oWebservice, sGroupIds, sBranchIds)
            Catch ex As Exception
            End Try
            Try
                BindCompletedVsTotal(oWebservice, sGroupIds, sBranchIds)
            Catch ex As Exception
            End Try

            litChartScripts.Text = "<script type='text/javascript'>" & sChartScripts & "</script>"
        End Sub

        Private Sub BindSummary(oWebservice As ProviderBase, sGroupIds As String, sBranchIds As String, iDateRange As String)
            Dim params As New List(Of StoredProcedureParameterType) From {
                New StoredProcedureParameterType With {.ParamName = "suser_group_ids", .ParamValue = sGroupIds},
                New StoredProcedureParameterType With {.ParamName = "sbranch_ids", .ParamValue = sBranchIds},
                New StoredProcedureParameterType With {.ParamName = "ldate_range", .ParamValue = iDateRange}
            }
            Dim result = CallSPWithCache(oWebservice, params.ToArray(), "spu_SAM_WorkManagerDashboardSummary")
            Dim xdoc As XDocument = XDocument.Parse(result.Results)
            Dim row = xdoc.Descendants("Row").FirstOrDefault()
            If row IsNot Nothing Then
                Dim assignedCount As Integer = GetXInt(row, "DueTaskCount")
                Dim completedCount As Integer = GetXInt(row, "CompletedTaskCount")
                Dim inProgressCount As Integer = GetXInt(row, "InProgressCount")

                litAssignedCount.Text = assignedCount.ToString("N0")
                litCompletedCount.Text = completedCount.ToString("N0")
                litInProgressCount.Text = inProgressCount.ToString("N0")

                ' Calculate percentages for progress bars
                Dim completedPct As Integer = If(assignedCount > 0, CInt((completedCount * 100) / assignedCount), 0)
                Dim inProgressPct As Integer = If(assignedCount > 0, CInt((inProgressCount * 100) / assignedCount), 0)
                progCompleted.Style("width") = completedPct.ToString() & "%"
                progInProgress.Style("width") = inProgressPct.ToString() & "%"

                ' RadialBar percentage
                Dim perfPercent As Integer = completedPct
                sChartScripts &= "var perfData = { percent: " & perfPercent.ToString() & " };"
            End If
        End Sub

        Private Sub BindTrend(oWebservice As ProviderBase, sGroupIds As String, sBranchIds As String)
            Dim params As New List(Of StoredProcedureParameterType) From {
                New StoredProcedureParameterType With {.ParamName = "suser_group_ids", .ParamValue = sGroupIds},
                New StoredProcedureParameterType With {.ParamName = "sbranch_ids", .ParamValue = sBranchIds}
            }
            Dim result = CallSPWithCache(oWebservice, params.ToArray(), "spu_SAM_WorkManagerDashboardTrend")
            Dim xdoc As XDocument = XDocument.Parse(result.Results)

            Dim days As New List(Of String)
            Dim currentWeek As New List(Of String)
            Dim previousWeek As New List(Of String)
            Dim currentTotal As Integer = 0
            Dim previousTotal As Integer = 0

            For Each row In xdoc.Descendants("Row")
                days.Add("""" & GetXString(row, "DayOfWeek").Substring(0, 3) & """")
                Dim cw As Integer = GetXInt(row, "CurrentWeekCount")
                Dim pw As Integer = GetXInt(row, "PreviousWeekCount")
                currentWeek.Add(cw.ToString())
                previousWeek.Add(pw.ToString())
                currentTotal += cw
                previousTotal += pw
            Next

            Dim todayRow = xdoc.Descendants("Row1").FirstOrDefault()
            If todayRow IsNot Nothing Then
                litTodayDue.Text = GetXInt(todayRow, "TodayDueCount").ToString("N0")
            End If

            litCurrentWeekTotal.Text = currentTotal.ToString("N0")
            litPreviousWeekTotal.Text = previousTotal.ToString("N0")

            If days.Count > 0 Then
                Dim script As String = "var trendData = { categories: [" & String.Join(",", days) & "], currentWeek: [" & String.Join(",", currentWeek) & "], previousWeek: [" & String.Join(",", previousWeek) & "] };"
                sChartScripts &= script
            End If
        End Sub

        Private Sub BindTasksByGroup(oWebservice As ProviderBase, sGroupIds As String, sBranchIds As String)
            litGroupLegend.Text = ""
            Dim params As New List(Of StoredProcedureParameterType) From {
                New StoredProcedureParameterType With {.ParamName = "suser_group_ids", .ParamValue = sGroupIds},
                New StoredProcedureParameterType With {.ParamName = "sbranch_ids", .ParamValue = sBranchIds}
            }
            Dim result = CallSPWithCache(oWebservice, params.ToArray(), "spu_SAM_WorkManagerDashboardTasksByGroup")
            Dim xdoc As XDocument = XDocument.Parse(result.Results)

            Dim labels As New List(Of String)
            Dim values As New List(Of String)
            Dim colors() As String = {"#727cf5", "#0acf97", "#fa5c7c", "#ffbc00", "#39afd1", "#e3eaef", "#6c757d", "#fd7e14"}
            Dim legend As New System.Text.StringBuilder()
            Dim i As Integer = 0

            For Each row In xdoc.Descendants("Row")
                Dim groupName As String = GetXString(row, "UserGroupName")
                Dim count As Integer = GetXInt(row, "DueTaskCount")
                labels.Add("""" & groupName.Replace("""", "'") & """")
                values.Add(count.ToString())
                Dim color As String = colors(i Mod colors.Length)
                legend.AppendLine("<p><i class=""mdi mdi-square"" style=""color:" & color & """></i> " & groupName & " <span class=""float-end"">" & count.ToString() & "</span></p>")
                i += 1
            Next

            litGroupLegend.Text = legend.ToString()
            If labels.Count > 0 Then
                Dim script As String = "var groupData = { labels: [" & String.Join(",", labels) & "], series: [" & String.Join(",", values) & "] };"
                sChartScripts &= script
            End If
        End Sub

        Private Sub BindCompletedVsTotal(oWebservice As ProviderBase, sGroupIds As String, sBranchIds As String)
            Dim params As New List(Of StoredProcedureParameterType) From {
                New StoredProcedureParameterType With {.ParamName = "suser_group_ids", .ParamValue = sGroupIds},
                New StoredProcedureParameterType With {.ParamName = "sbranch_ids", .ParamValue = sBranchIds},
                New StoredProcedureParameterType With {.ParamName = "lmonths", .ParamValue = "18"}
            }
            Dim result = CallSPWithCache(oWebservice, params.ToArray(), "spu_SAM_WorkManagerDashboardCompletedVsTotal")
            Dim xdoc As XDocument = XDocument.Parse(result.Results)

            Dim months As New List(Of String)
            Dim totalTasks As New List(Of String)
            Dim completedTasks As New List(Of String)

            For Each row In xdoc.Descendants("Row")
                months.Add("""" & GetXString(row, "MonthYear") & """")
                totalTasks.Add(GetXInt(row, "TotalTasks").ToString())
                completedTasks.Add(GetXInt(row, "CompletedTasks").ToString())
            Next

            If months.Count > 0 Then
                Dim script As String = "var monthlyData = { categories: [" & String.Join(",", months) & "], totalTasks: [" & String.Join(",", totalTasks) & "], completedTasks: [" & String.Join(",", completedTasks) & "] };"
                sChartScripts &= script
            End If
        End Sub


        Private Function GetXString(row As XElement, name As String) As String
            If row.Element(name) IsNot Nothing Then Return row.Element(name).Value
            Return ""
        End Function

        Private Function GetXInt(row As XElement, name As String) As Integer
            Dim s As String = GetXString(row, name)
            Dim i As Integer = 0
            Integer.TryParse(s, i)
            Return i
        End Function

        Private Function GetXDecimal(row As XElement, name As String) As Decimal
            Dim s As String = GetXString(row, name)
            Dim d As Decimal = 0
            Decimal.TryParse(s, d)
            Return d
        End Function

#Region "Event Handlers"
        Protected Sub ddlUserGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUserGroup.SelectedIndexChanged
        End Sub
        Protected Sub ddlDateRange_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDateRange.SelectedIndexChanged
        End Sub
        Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        End Sub
#End Region

    End Class
End Namespace
