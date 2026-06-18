Imports System.Data.SqlClient
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Utils.Cache
Namespace SiteMap

    Public Class ContentHistory
        Private iHistoryID As Integer
        Private dTimeStamp As Date
        Private sAdminName As String
        Private sAction As String
        Public MyCache As System.Web.Caching.Cache

        Public Sub New(ByVal SiteMapID As Integer, ByVal Draft As Boolean)
            'Only latest history entry is required so we can make the db call in the constructor

            'Dim command As New SqlCommand("usp_GetLastEditDetails")
            Dim dt As DataTable
            Dim command As New SqlCommand
            MyCache = System.Web.HttpContext.Current.Cache

            Dim sCacheKey As String = "GetLastEditDetails_SiteMapID_" & SiteMapID.ToString & "_Draft_" & Draft.ToString

            If MyCache.Item(sCacheKey) Is Nothing Then
                command = New SqlCommand("usp_GetLastEditDetails")
                command.Parameters.Add("@sitemap_id", SqlDbType.Int).Value = SiteMapID
                command.Parameters.Add("@draft", SqlDbType.Bit).Value = Draft
                'Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")
                dt = funcDB.GetDataTable(command, "CMS")
                MyCache.Insert(sCacheKey, dt, Nothing)
                command.Dispose()
            Else
                dt = CType(MyCache.Item(sCacheKey), DataTable)
            End If
            'While sdrTmp.Read
            '    iHistoryID = CType(sdrTmp("history_id"), Integer)
            '    dTimeStamp = CType(sdrTmp("edit_date"), DateTime)
            '    'sAdminName = CType(sdrTmp("admin_name"), String)
            '    sAction = CType(sdrTmp("action"), String)
            'End While
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                With dt.Rows(0)
                    iHistoryID = CType(.Item("history_id"), Integer)
                    dTimeStamp = CType(.Item("edit_date"), DateTime)
                    sAction = CType(.Item("action"), String)
                End With
            End If

            'sdrTmp.Close()
            'command.Dispose()

        End Sub

        Public Sub New(ByVal HistoryID As Integer, ByVal TimeStamp As Date, _
                        ByVal AdminName As String, ByVal Action As String)
            'Constructor used when all sitemap node history is returned, to minimize db calls

            iHistoryID = HistoryID
            dTimeStamp = TimeStamp
            sAdminName = AdminName
            sAction = Action

        End Sub

        Public ReadOnly Property HistoryID() As Integer
            Get
                Return iHistoryID
            End Get
        End Property

        Public ReadOnly Property TimeStamp() As Date
            Get
                Return dTimeStamp
            End Get
        End Property

        Public ReadOnly Property AdminName() As String
            Get
                Return sAdminName
            End Get
        End Property

        Public ReadOnly Property Action() As String
            Get
                Return sAction
            End Get
        End Property

    End Class

End Namespace