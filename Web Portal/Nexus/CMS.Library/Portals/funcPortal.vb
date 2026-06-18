Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web
Imports System.Web.HttpContext
Imports Nexus.Utils
Imports Nexus.Utils.Cache
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.SessionState.HttpSessionState

Namespace Portal

    Public Module Functions

        ''' <summary>
        ''' This function will check existance of portal id in "PortalID" cache.
        ''' If exist then will return from cache else will be retrieved from database.Session("PortalID") will not be usable anymore.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPortalID() As Integer
            Dim iPortalID As Integer
            If HttpContext.Current.Session Is Nothing OrElse HttpContext.Current.Session("PortalID") Is Nothing Then
                'Either we're not logged into the admin (in which case we shouldn't be here!) or
                'no frontend session exists so we need to determine the portal id

                Dim hPortalColl As New Hashtable()
                hPortalColl = CType(HttpContext.Current.Cache("PortalColl"), Hashtable)

                Dim iPortalMode As String = AppSettings("CMS")

                If iPortalMode = "full" Or iPortalMode = "portal" Then

                    Dim command As New SqlCommand("usp_GetPortalID")

                    Dim sPortalName As String
                    With Current.Request
                        sPortalName = .Url.Scheme & .Url.SchemeDelimiter & .Url.Authority & .ApplicationPath
                        If sPortalName.EndsWith("/") Then
                            sPortalName = sPortalName.Trim("/")
                        End If
                    End With

                    If Not hPortalColl Is Nothing AndAlso hPortalColl.ContainsKey(sPortalName) Then
                        Return CInt(hPortalColl(sPortalName))
                    End If

                    command.Parameters.Add("@hostname", SqlDbType.VarChar, 250).Value = sPortalName
                    command.Parameters.Add("@portal_id", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    funcDB.ExecNonQuery(command, "CMS")

                    If command.Parameters("@portal_id").Value > 0 Then
                        'Portal found in DB, use the ID returned
                        iPortalID = command.Parameters("@portal_id").Value
                        Try
                            'Added into collection
                            If hPortalColl Is Nothing Then
                                hPortalColl = New Hashtable
                            End If
                            hPortalColl.Add(sPortalName, iPortalID)
                            HttpContext.Current.Cache.Insert("PortalColl", hPortalColl)
                            ' When using the GetNavigation via global.asax (for url-rewriting, it calls this function, but the session has not been set up.)
                            Current.Session.Add("PortalID", iPortalID)
                        Catch ex As Exception

                        End Try

                    Else
                        'Not found ... not much we can do without this, so end request with 404                        
                        Current.Response.StatusCode = 404
                        Current.Response.StatusDescription = "Not Found"
                        Current.Response.Write("<h2>No site configured at this address</h2>")
                        Current.Response.End()
                    End If
                    command.Dispose()

                End If

            Else
                'We're either in the admin or a frontend session exists with the portal id set
                iPortalID = CType(HttpContext.Current.Session("PortalID"), Integer)
            End If

            Return iPortalID

        End Function

        
        Function GetPortal(ByVal pPortalID As Integer) As Portal

            Dim tmpPortal As Portal = Nothing

            Dim command As New SqlCommand("usp_GetPortal")
            command.Parameters.Add("@portal_id", SqlDbType.Int).Value = pPortalID

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            While sdrTmp.Read
                tmpPortal = New Portal(sdrTmp("portal_id"), sdrTmp("name"), sdrTmp("url"), _
                                    IIf(IsDBNull(sdrTmp("otherurls")), "", sdrTmp("otherurls")), _
                                    sdrTmp("culture_code"), sdrTmp("master_page"), sdrTmp("theme"), _
                                    False)
            End While

            sdrTmp.Close()
            command.Dispose()

            Return tmpPortal

        End Function


        ''' <summary>
        ''' This function will check existance of webroot from "WebRoot" cahce.If exists then will be returned from cache else will be retrieved from DB
        ''' </summary>
        ''' <param name="pPortalID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetWebRoot(ByVal pPortalID As Integer) As String

            Dim sWebRoot As String = String.Empty

            If HttpContext.Current.Cache("WebRoot") IsNot Nothing Then
                sWebRoot = HttpContext.Current.Cache("WebRoot")
            Else
                Dim command As New SqlCommand("usp_GetPortalUrl")

                With command.Parameters
                    .Add("@portal_id", SqlDbType.Int).Value = pPortalID
                    .Add("@url", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                End With

                funcDB.ExecNonQuery(command, "CMS")

                If Not IsDBNull(command.Parameters("@url").Value) Then
                    sWebRoot = command.Parameters("@url").Value()
                End If
                command.Dispose()
            End If


            Return sWebRoot

        End Function


        Function AddEditPortal(ByVal pPortalID As Integer, ByVal pName As String, _
                                    ByVal pUrl As String, ByVal pOtherUrls As String, _
                                    ByVal pCultureCode As String, ByVal pMasterPage As String, _
                                    ByVal pTheme As String) As Integer

            Dim command As New SqlCommand("usp_AddEditPortal")

            With command.Parameters
                .Add("@portal_id", SqlDbType.Int).Value = pPortalID
                .Item("@portal_id").Direction = ParameterDirection.InputOutput
                .Add("@name", SqlDbType.VarChar, 30).Value = pName.TrimEnd
                .Add("@url", SqlDbType.VarChar, 250).Value = pUrl.TrimEnd
                .Add("@otherurls", SqlDbType.VarChar, 2000).Value = pOtherUrls.TrimEnd
                .Add("@culture_code", SqlDbType.VarChar, 5).Value = pCultureCode
                .Add("@master_page", SqlDbType.VarChar, 250).Value = pMasterPage
                .Add("@theme", SqlDbType.VarChar, 250).Value = pTheme
            End With

            funcDB.ExecNonQuery(command, "CMS")

            Dim iPortalID As Integer = CInt(command.Parameters("@portal_id").Value)

            command.Dispose()
            Current.Cache.Remove("PortalList")

            Return iPortalID

        End Function

    End Module

End Namespace

