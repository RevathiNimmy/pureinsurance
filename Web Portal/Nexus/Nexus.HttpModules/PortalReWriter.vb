Imports System
Imports System.IO
Imports System.Web
Imports Nexus.Library
Imports System.Configuration
Imports System.Web.HttpContext
Imports CMS.Library
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Diagnostics



Public Class PortalRewriter : Implements IHttpModule

	Private alFileList As ArrayList
	Private SearchDir As String

	Public Sub Dispose() Implements System.Web.IHttpModule.Dispose

	End Sub

	Public Sub Init(ByVal app As System.Web.HttpApplication) Implements System.Web.IHttpModule.Init

		AddHandler app.BeginRequest, AddressOf Application_BeginRequest

		'handlers required to enable session state, allow portal ID to be set in session and portal rewriting to still work
		AddHandler app.PostAcquireRequestState, AddressOf Me.Application_PostAcquireRequestState
		AddHandler app.PostMapRequestHandler, AddressOf Me.Application_PostMapRequestHandler
		AddHandler app.EndRequest, AddressOf Application_EndRequest

	End Sub

	''' <summary>
	''' Append session Id with request URL from cookieless sessions
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Application_PreRequestHandlerExecute(ByVal sender As Object, ByVal e As EventArgs)

		Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
		Dim oPortal As Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
		Dim app As HttpApplication = CType(sender, HttpApplication)

		Dim strWebRoot As String = System.Configuration.ConfigurationManager.AppSettings("webroot")
		Dim sCurrentFile As String = Right(HttpContext.Current.Request.Path, (HttpContext.Current.Request.Path.Length - strWebRoot.Length + 1)).ToLower
		Dim bIsModal As Boolean = False

		If HttpContext.Current.Request.QueryString("modal") = "true" Then
			bIsModal = True
		End If

		If HttpContext.Current.Session IsNot Nothing Then
			If HttpContext.Current.Session.IsCookieless And bIsModal = True Then
				app.Context.RewritePath(strWebRoot + "(S(" & Current.Session.SessionID.ToString() + "))" & sCurrentFile, True)
			End If
		End If

	End Sub

	Private Sub Application_PostAcquireRequestState(source As Object, e As EventArgs)
		Dim app As HttpApplication = DirectCast(source, HttpApplication)
		Dim resourceHttpHandler As MyHttpHandler = TryCast(HttpContext.Current.Handler, MyHttpHandler)

		If resourceHttpHandler IsNot Nothing Then
			' set the original handler back
			HttpContext.Current.Handler = resourceHttpHandler.OriginalHandler
		End If

	End Sub


	Private Sub Application_PostMapRequestHandler(ByVal sender As Object, ByVal e As EventArgs)
		Dim app As HttpApplication = CType(sender, HttpApplication)
		If TypeOf app.Context.Handler Is IReadOnlySessionState OrElse TypeOf app.Context.Handler Is IRequiresSessionState Then
			Return
		End If
		' swap the current handler

		app.Context.Handler = New MyHttpHandler(app.Context.Handler)
	End Sub
	''' <summary>
	''' Get list of files in the current portal directory
	''' If there is a file that matches the current request then rewrite the request to the portal version
	''' To use in Nexus needs to have the current portal added to the path
	''' Also needs a control adaptor to be added so that postbacks go to the correct URL
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
		Try
			'fetch name of current portal ...
			Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
			Dim iPortalID As Integer
			If HttpContext.Current.Cache("PortalID") IsNot Nothing Then
				iPortalID = HttpContext.Current.Cache("PortalID")
			Else
				iPortalID = Portal.Functions.GetPortalID
			End If
			Dim oPortal As Config.Portal = oNexusConfig.Portals.Portal(iPortalID)
			Dim app As HttpApplication = CType(sender, HttpApplication)

			'add portal name to the search directory
			SearchDir = HttpContext.Current.Server.MapPath("~/portal/" & oPortal.Name)
			Dim rootDir As New DirectoryInfo(SearchDir)
			Dim alCurrentFileList As ArrayList
			'If HttpContext.Current.Request.Cookies(".ASPXANONYMOUS") IsNot Nothing Then
			'    Dim cookieName As String = String.Empty
			'    Dim authCookie As HttpCookie = Nothing
			'    Dim reqAuthCookie As HttpCookie = Nothing
			'    authCookie = HttpContext.Current.Response.Cookies(".ASPXANONYMOUS")
			'    reqAuthCookie = HttpContext.Current.Request.Cookies(".ASPXANONYMOUS")
			'    authCookie.SameSite = SameSiteMode.None
			'    authCookie.Secure = True
			'    reqAuthCookie.Secure = True
			'Else
			'    If HttpContext.Current.Response.Cookies.AllKeys().Length > 0 Then
			'        For Each cnt As Integer In HttpContext.Current.Response.Cookies.AllKeys()
			'            HttpContext.Current.Response.Cookies(cnt).Secure = True
			'        Next

			'    End If

			'End If

			' check for the cache object for the current portal and use that if it exists
			If HttpContext.Current.Cache(oPortal.Name & "_filelist") Is Nothing Then
				alFileList = New ArrayList
				WalkDirectoryTree(rootDir)
				alCurrentFileList = alFileList

				'create an array of folders in the current portal folder
				'in order to set up a cache dependency on these folders


				Dim arFolderlist As New ArrayList

				Dim iCounter, i As Integer
				Dim sCurrentFolder As String = String.Empty
				For iCounter = 0 To alCurrentFileList.Count - 1
					'Go backwards through the full file path till we hit \
					'this allows us to just place the folder name in the cache dependency
					For i = Len(alCurrentFileList(iCounter)) To 1 Step -1
						Select Case Mid(alCurrentFileList(iCounter), i, 1)
							Case "/"
								' backslash aren't included in the result
								sCurrentFolder = Left(alCurrentFileList(iCounter), i - 1)
								Exit For
						End Select
					Next

					'check if the folder is already in the array, if not then add it
					If Not arFolderlist.Contains(SearchDir & sCurrentFolder) Then
						arFolderlist.Add(SearchDir & sCurrentFolder)
					End If
				Next
				'convert array list to array so that it can be used in the cache dependency
				Dim sFolderList() As String = arFolderlist.ToArray(GetType(String))

				'add filelist to cache.
				'a cache dependency is added so that cache is invalidated 
				'when contents of portal folder changes
				HttpContext.Current.Cache.Insert(oPortal.Name & "_filelist", alCurrentFileList, New System.Web.Caching.CacheDependency(sFolderList))

			Else
				alCurrentFileList = HttpContext.Current.Cache(oPortal.Name & "_filelist")
			End If

			Dim sCurrentFile As String = Right(HttpContext.Current.Request.Path, (HttpContext.Current.Request.Path.Length - System.Configuration.ConfigurationManager.AppSettings("webroot").Length + 1)).ToLower

			If alCurrentFileList.Contains(sCurrentFile) Then
				'rewrite the request so that the portal specfic page
				'CurrentPortal name inserted to load the pages for the correct portal
				app.Context.RewritePath("~/portal/" & oPortal.Name & sCurrentFile, False)
			End If
		Catch ex As Exception
			Dim sErrorMessage As String = ex.Message.ToString()
			Current.Response.Write(NexusProvider.ErrorFormatter.FormatErrorAsHtml(ex))
			HttpContext.Current.Server.ClearError()
			HttpContext.Current.Response.End()

		End Try
	End Sub
	Private Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim providerSection As Config.IdentityProvider = CType(ConfigurationManager.GetSection("IdentityProvider"), Config.IdentityProvider)
			Dim identityProvider = providerSection.DefaultIdentity.ToUpper
			If Not Equals(identityProvider, "SSO") AndAlso Not Equals(identityProvider, "KEYCLOAK") Then
				If HttpContext.Current.Request.Cookies(".ASPXANONYMOUS") IsNot Nothing Then
					Dim cookieName As String = String.Empty
					Dim authCookie As HttpCookie = Nothing
					Dim reqAuthCookie As HttpCookie = Nothing
					authCookie = HttpContext.Current.Response.Cookies(".ASPXANONYMOUS")
					reqAuthCookie = HttpContext.Current.Request.Cookies(".ASPXANONYMOUS")
					authCookie.SameSite = SameSiteMode.None
					Dim requireSsl As Boolean = authCookie.Secure
					If (requireSsl) Then
						authCookie.Secure = True
						authCookie.HttpOnly = True
						reqAuthCookie.Secure = True
					Else
						authCookie.Secure = False
						authCookie.HttpOnly = False
						reqAuthCookie.Secure = False
					End If
				ElseIf HttpContext.Current.Response.Cookies("SessionTimeout") IsNot Nothing Then
					Dim cookieName As String = String.Empty
					Dim authCookie As HttpCookie = Nothing
					Dim reqAuthCookie As HttpCookie = Nothing
					authCookie = HttpContext.Current.Response.Cookies("SessionTimeout")
					reqAuthCookie = HttpContext.Current.Request.Cookies("SessionTimeout")
					authCookie.SameSite = SameSiteMode.None
					Dim requireSsl As Boolean = authCookie.Secure
					If (requireSsl) Then
						authCookie.Secure = True
						authCookie.HttpOnly = True
						reqAuthCookie.Secure = True
					Else
						authCookie.Secure = False
						authCookie.HttpOnly = False
						reqAuthCookie.Secure = False
					End If
				Else
					If HttpContext.Current.Response.Cookies.AllKeys().Length > 0 Then
						For Each cnt As Integer In HttpContext.Current.Response.Cookies.AllKeys()
							HttpContext.Current.Response.Cookies(cnt).Secure = True
						Next

					End If

				End If

			End If
		Catch ex As Exception

		End Try
	End Sub

	''' <summary>
	''' Recursivley traverses directory and adds list of files, with relative path, to array list
	''' </summary>
	''' <param name="root"></param>
	''' <remarks></remarks>
	Private Sub WalkDirectoryTree(ByVal root As System.IO.DirectoryInfo)
		Dim files() As FileInfo = Nothing
		Dim subDirs() As DirectoryInfo = Nothing

		' First, process all the files directly under this folder
		Try
			files = root.GetFiles("*.*")
		Catch ex As Exception
			'do some error hanlding here
		End Try

		If (Not (files) Is Nothing) Then
			For Each fi As FileInfo In files
				alFileList.Add((Replace(Right(fi.FullName, (Len(fi.FullName) - Len(SearchDir))), "\", "/")).ToLower)
			Next
			' Now find all the subdirectories under this directory.
			subDirs = root.GetDirectories
			For Each dirInfo As System.IO.DirectoryInfo In subDirs
				' Resursive call for each subdirectory.
				WalkDirectoryTree(dirInfo)
			Next
		End If
	End Sub



	Public Class MyHttpHandler : Implements IHttpHandler, IRequiresSessionState
		Friend ReadOnly OriginalHandler As IHttpHandler
		Public Sub New(originalHandler__1 As IHttpHandler)
			OriginalHandler = originalHandler__1
		End Sub
		Public Sub ProcessRequest(context As HttpContext)

			' do not worry, ProcessRequest() will not be called, but let's be safe

			Throw New InvalidOperationException("MyHttpHandler cannot process requests.")

		End Sub

		Public ReadOnly Property IsReusable() As Boolean
			' IsReusable must be set to false since class has a member!

			Get
				Return False
			End Get
		End Property


		Public ReadOnly Property IsReusable1 As Boolean Implements IHttpHandler.IsReusable
			Get

			End Get
		End Property

		Public Sub ProcessRequest1(context As HttpContext) Implements IHttpHandler.ProcessRequest

		End Sub
	End Class

	'End Class
End Class


