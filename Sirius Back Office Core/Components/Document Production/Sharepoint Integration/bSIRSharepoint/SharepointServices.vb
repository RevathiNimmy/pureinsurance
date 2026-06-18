Imports System.IO
Imports System.Net
Imports System.Xml
Namespace SharepointServices

    Public NotInheritable Class Sharepoint

        '  Private m_listService As ListsService.Lists
        Private m_credentials As ICredentials
        Private m_lists As ListInfoCollection
        Private m_bDebugMode As Boolean = False
        Private m_sErrorString As String = ""
        Private m_bFolderCreated As Boolean = False
        '  Private m_CopyService As CopyService.Copy

        Public ReadOnly Property ErrorString() As String
            Get
                Return m_sErrorString
            End Get
        End Property

        Public Sub New()
            m_credentials = CredentialCache.DefaultNetworkCredentials
            ' m_listService = New ListsService.Lists()
            'm_listService.Credentials = m_credentials

            '   m_lists = New ListInfoCollection(m_listService)
            '  m_CopyService = New CopyService.Copy()
            '  m_CopyService.Credentials = m_credentials

        End Sub

        Public Class ListInfo
            Public m_rootFolder As String
            Public m_listName As String
            Public m_version As String
            Public m_webUrl As String

            Public Sub New(ByVal listResponse As XmlNode)

                m_rootFolder = listResponse.Attributes("RootFolder").Value & "/"
                m_listName = listResponse.Attributes("ID").Value
                m_version = listResponse.Attributes("Version").Value
            End Sub

            Public Function IsMatch(ByVal url As String) As Boolean
                Try
                    url &= "/"
                    Return url.Substring(0, m_rootFolder.Length) = m_rootFolder
                Catch
                End Try

                Return False

            End Function

        End Class

        Public Property DebugMode() As Boolean
            Get
                DebugMode = m_bDebugMode
            End Get
            Set(ByVal value As Boolean)
                m_bDebugMode = value
            End Set
        End Property

        Public NotInheritable Class ListInfoCollection
            Implements IEnumerable(Of ListInfo)

            '  Private m_listService As ListsService.Lists
            Private m_lists As New Dictionary(Of String, ListInfo)()

            'Public Sub New(ByVal listService As ListsService.Lists)
            '    m_listService = listService
            'End Sub

            Public Function GetEnumerator() As IEnumerator(Of ListInfo) Implements IEnumerable(Of Sharepoint.ListInfo).GetEnumerator
                Return m_lists.Values.GetEnumerator()
            End Function

            Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return Me.GetEnumerator()
            End Function

            'Public Function Find(ByVal fileInfo As FileInfo) As ListInfo
            '    If m_lists.ContainsKey(fileInfo.LookupName) Then
            '        Return m_lists(fileInfo.LookupName)
            '    End If

            '    For Each li As ListInfo In m_lists.Values
            '        If li.IsMatch(fileInfo.LookupName) Then
            '            Return li
            '        End If
            '    Next

            '    Dim webUrl As String = fileInfo.m_URL

            '    If fileInfo.m_listInfo IsNot Nothing AndAlso Not String.IsNullOrEmpty(fileInfo.m_listInfo.m_listName) Then


            '        Dim listInfo As New ListInfo(CallService(webUrl, Function() m_listService.GetList(fileInfo.LookupName)))

            '        listInfo.m_webUrl = webUrl


            '        Return listInfo
            '    Else
            '        Dim lists As XmlNode = CallService(webUrl, Function() m_listService.GetListCollection())

            '        If lists Is Nothing Then
            '            Throw New Exception("Could not find web.")
            '        End If

            '        'Find list by RootFolder (which doesn't seem to be populated in GetListCollection response so must iterate GetList response)

            '        For Each list As XmlNode In lists.ChildNodes
            '            Dim listInfo As New ListInfo(m_listService.GetList(list.Attributes("Name").Value))

            '            listInfo.m_webUrl = webUrl

            '            m_lists.Add(listInfo.m_listName, listInfo)

            '            If listInfo.IsMatch(fileInfo.LookupName) Then
            '                Return listInfo
            '            End If
            '        Next
            '    End If

            '    Throw New Exception("Could not find list.")

            'End Function

            Private Delegate Function ServiceOperation() As XmlNode

            'Private Function CallService(ByRef webURL As String, ByVal serviceOperation As ServiceOperation) As XmlNode
            '    Try
            '        webURL = webURL.Substring(0, webURL.LastIndexOf("/"))

            '        Try
            '            m_listService.Url = webURL & "/_vti_bin/Lists.asmx"
            '            Return serviceOperation()
            '        Catch
            '            Return CallService(webURL, serviceOperation)
            '        End Try

            '    Catch
            '        webURL = Nothing

            '        Return Nothing
            '    End Try

            'End Function

        End Class

        Public NotInheritable Class FileInfo
            Public m_URL As String
            Public m_bytes As Byte()
            Public m_properties As Dictionary(Of String, Object)
            Public m_listInfo As ListInfo
            Public m_ensureFolders As Boolean = True
            Private m_uri As Uri

            Public ReadOnly Property HasProperties() As Boolean
                Get
                    Return m_properties IsNot Nothing AndAlso m_properties.Count > 0
                End Get
            End Property

            Public ReadOnly Property RelativeFilePath() As String
                Get
                    Return m_URL.Substring(m_URL.IndexOf(m_listInfo.m_rootFolder) + 1)
                End Get
            End Property

            Public ReadOnly Property URI() As Uri
                Get
                    If m_uri Is Nothing Then
                        m_uri = New Uri(m_URL)
                    End If

                    Return m_uri
                End Get
            End Property

            Public ReadOnly Property LookupName() As String
                Get
                    If m_listInfo IsNot Nothing AndAlso Not String.IsNullOrEmpty(m_listInfo.m_listName) Then
                        Return m_listInfo.m_listName
                    End If

                    Return URI.LocalPath
                End Get
            End Property

            Public Sub New(ByVal url As String, ByVal bytes As Byte(), ByVal properties As Dictionary(Of String, Object))
                m_URL = url.Replace("%20", " ")
                m_bytes = bytes
                m_properties = properties
            End Sub

        End Class

        'Public Function GetFileList(ByVal rootUrl As String, ByVal docLib As String, ByVal destinationUrl As String) As XmlNode
        '    Try
        '        Dim listUrl As String = "_vti_bin/Lists.asmx"

        '        If Not rootUrl.EndsWith("/") Then
        '            listUrl = "/" & listUrl
        '        End If
        '        m_listService.Url = rootUrl & listUrl
        '        m_listService.UseDefaultCredentials = True

        '        ' set up xml  doc for getting list of files under a folder
        '        Dim doc As XmlDocument = New XmlDocument()
        '        Dim query As XmlElement = doc.CreateElement("Query")
        '        Dim ViewFields As XmlElement = doc.CreateElement("ViewFields")
        '        Dim queryOptions As XmlElement = doc.CreateElement("QueryOptions")
        '        queryOptions.InnerXml = "<Folder>" & destinationUrl & "</Folder>"

        '        ViewFields.InnerXml = "<FieldRef Name='Title'/><FieldRef Name='InternalOnly'/><FieldRef Name='PureUser'/><FieldRef Name='DocumentGroup'/><FieldRef Name='DocumentSubGroup'/><ViewAttributes Scope='Recursive' />"

        '        ' get the list of files
        '        Dim listItemsNode As XmlNode = m_listService.GetListItems(
        '                                                docLib, Nothing,
        '                                                query, ViewFields, 500, queryOptions, Nothing)

        '        Return listItemsNode

        '    Catch ex As Exception
        '        Throw New Exception(ex.Message)
        '        Return Nothing
        '    End Try
        'End Function

        'Public Function GetList(ByVal rootUrl As String, ByVal docLib As String, ByVal destinationUrl As String) As Xml.XmlNode
        '    Try
        '        Dim listUrl As String = "_vti_bin/Lists.asmx"
        '        If Not rootUrl.EndsWith("/") Then
        '            listUrl = "/" & listUrl
        '        End If
        '        m_listService.Url = rootUrl & listUrl
        '        m_listService.UseDefaultCredentials = True

        '        ' set up xml  doc for getting list of files under a folder
        '        Dim doc As XmlDocument = New XmlDocument()
        '        Dim query As XmlElement = doc.CreateElement("Query")
        '        query.InnerXml = "<Where><Eq><FieldRef Name='FileRef' /> <Value Type='Text'>" & destinationUrl & "</Value></Eq></Where>"
        '        Dim queryOptions As XmlElement = doc.CreateElement("QueryOptions")
        '        Dim strFolderPath As String
        '        'extract the folder path from url
        '        strFolderPath = Left(destinationUrl, Microsoft.VisualBasic.Strings.InStrRev(destinationUrl, "/"))
        '        queryOptions.InnerXml = "<Folder>" & strFolderPath & "</Folder>"
        '        ' set up xml  doc for getting list of files under a folder
        '        doc = New XmlDocument()
        '        Dim ViewFields As XmlElement = doc.CreateElement("ViewFields")
        '        ViewFields.InnerXml = "<FieldRef Name='ID'/><ViewAttributes Scope='Recursive' />"

        '        ' get the list of files
        '        Dim listItemsNode As XmlNode = m_listService.GetListItems(
        '                                                docLib, Nothing,
        '                                                query, ViewFields, Nothing, queryOptions, Nothing)

        '        Return listItemsNode

        '    Catch ex As Exception
        '        Throw New Exception(ex.Message)
        '        Return Nothing
        '    End Try
        'End Function
        '''' <summary>
        ''' Upload the document
        ''' </summary>
        ''' <param name="destinationUrl"></param>
        ''' <param name="bytes"></param>
        ''' <param name="properties"></param>
        ''' <param name="docLib"></param>
        ''' <param name="isDMEMigration"></param>
        ''' <param name="sSourceFile"></param>
        ''' <param name="obSIRSharePointApi"></param>
        ''' <returns></returns>
        Public Function Upload(ByVal destinationUrl As String, ByVal bytes As Byte(), ByVal properties As Dictionary(Of String, Object), ByVal docLib As String, ByVal isDMEMigration As Boolean, Optional ByVal sSourceFile As String = "", Optional ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls = Nothing) As Boolean
            Return Upload(New FileInfo(destinationUrl, bytes, properties), docLib, isDMEMigration, sSourceFile, obSIRSharePointApi)
        End Function
        ''' <summary>
        ''' 'Upload the document
        ''' </summary>
        ''' <param name="fileInfo"></param>
        ''' <param name="docLib"></param>
        ''' <param name="isDMEMigration"></param>
        ''' <param name="sSourceFile"></param>
        ''' <param name="obSIRSharePointApi"></param>
        ''' <returns></returns>
        Public Function Upload(ByVal fileInfo As FileInfo, ByVal docLib As String, ByVal isDMEMigration As Boolean, Optional ByVal sSourceFile As String = "", Optional ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls = Nothing) As Boolean
            Try

                Dim result As Boolean = False

                
                If Not result AndAlso fileInfo.m_ensureFolders Then

                    Dim uri As String = fileInfo.URI.AbsoluteUri
                    Dim sFolderNames As String = ""
                    Dim sSharePointUrl As String = obSIRSharePointApi.model.SharePointSiteURL
                    obSIRSharePointApi.model.SharePointDocumentLibrary = docLib
                    If sSharePointUrl.EndsWith("/") Then
                        If uri.Contains(" ") OrElse uri.Contains("%20") Then
                            Dim libpath As String = uri.Replace(sSharePointUrl, "")
                            Dim lstDocFolder As String()
                            lstDocFolder = libpath.Split("/")
                            sFolderNames = uri.Replace(sSharePointUrl + lstDocFolder(1) + "/", "")
                        Else
                            sFolderNames = uri.Replace(sSharePointUrl + docLib + "/", "")
                        End If
                    ElseIf sSharePointUrl.Length > 0 Then
                        If uri.Contains(" ") OrElse uri.Contains("%20") Then
                            Dim libpath As String = uri.Replace(sSharePointUrl, "")
                            Dim lstDocFolder As String()
                            lstDocFolder = libpath.Split("/")
                            sFolderNames = uri.Replace(sSharePointUrl + "/" + lstDocFolder(1) + "/", "")
                        Else
                            sFolderNames = uri.Replace(sSharePointUrl + "/" + docLib + "/", "")
                        End If

                    End If

                    Dim fileName As String = System.IO.Path.GetFileName(uri)
                    sFolderNames = sFolderNames.Replace(fileName, "")
                    Dim lstFolderNames As String()
                    If sFolderNames.Length > 0 Then
                        If sFolderNames.EndsWith("/") Then
                            sFolderNames = sFolderNames.Substring(0, sFolderNames.Length - 1)
                        End If
                        lstFolderNames = sFolderNames.Split("/")

                        m_bFolderCreated = obSIRSharePointApi.CreateSharePointFolders(lstFolderNames)
                    End If

                    If m_bFolderCreated Then
                        result = TryToUpload(fileInfo, docLib, sSourceFile, obSIRSharePointApi)
                    End If
                End If

                Return result
            Catch ex As Exception
                Throw (ex)
            End Try
        End Function
        ''' <summary>
        ''' TryToUpload the document
        ''' </summary>
        ''' <param name="fileInfo"></param>
        ''' <param name="docLib"></param>
        ''' <param name="sSourceFile"></param>
        ''' <param name="obSIRSharePointApi"></param>
        ''' <returns></returns>
        Private Function TryToUpload(ByVal fileInfo As FileInfo, ByVal docLib As String, Optional ByVal sSourceFile As String = "", Optional ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls = Nothing) As Boolean

            Try

                Dim request As WebRequest = WebRequest.Create(fileInfo.m_URL)

                request.Credentials = System.Net.CredentialCache.DefaultCredentials
                request.Method = "PUT"
                request.Headers.Add("OVERWRITE", "T")

                Dim buffer As Byte() = New Byte(1023) {}

                Using stream As Stream = request.GetRequestStream()
                    Using ms As New MemoryStream(fileInfo.m_bytes)
                        Dim i As Integer = ms.Read(buffer, 0, buffer.Length)
                        While i > 0
                            stream.Write(buffer, 0, i)
                            i = ms.Read(buffer, 0, buffer.Length)
                        End While
                    End Using
                End Using

                Dim response As WebResponse = request.GetResponse()

                response.Close()

                If fileInfo.HasProperties Then
                    Dim iDocID As Integer = GetFileID(fileInfo.m_listInfo.m_webUrl, docLib, fileInfo.m_URL, obSIRSharePointApi)

                    If m_bDebugMode Then
                        'This is useful if you have changed the Content Type for PureDocument 
                        'and need to check failures of a particular field
                        'this path is not performant and should only be used during development
                        Dim sFailures As String = ""

                        For Each [property] As KeyValuePair(Of String, Object) In fileInfo.m_properties
                            obSIRSharePointApi.UpdateListItems(fileInfo.m_properties, iDocID, fileInfo.m_URL)
                        Next

                        If sFailures <> "" Then
                            Throw New Exception(sFailures)
                        End If
                    Else
                        obSIRSharePointApi.UpdateListItems(fileInfo.m_properties, iDocID, fileInfo.m_URL)
                    End If
                End If

                Return True

            Catch generatedExceptionName As WebException
                'We must not throw the exception here - as we may be attempting to create a file in a 
                'folder that doesn't exist yet and need to trap it.
                If generatedExceptionName.Response IsNot Nothing AndAlso Not DirectCast(generatedExceptionName.Response, System.Net.HttpWebResponse).StatusCode = HttpStatusCode.Conflict Then
                    Throw New Exception("TryToUpload - " & generatedExceptionName.Message)
                End If
                Return False
            Catch ex As Exception
                m_sErrorString = ex.Message
                Return False
            End Try

        End Function
        ''' <summary>
        ''' Create Folders under document library
        ''' </summary>
        ''' <param name="folderURL"></param>
        ''' <param name="properties"></param>
        ''' <returns></returns>
        Public Function CreateFolders(ByVal folderURL As String, ByVal properties As Dictionary(Of String, Object)) As Boolean
            Try
                Dim bytes(0) As Byte
                Dim info As FileInfo = New FileInfo(folderURL, bytes, properties)

                If info.m_ensureFolders Then
                    Dim root As String = info.URI.AbsoluteUri.Replace(info.URI.AbsolutePath, "")

                    For i As Integer = 0 To info.URI.Segments.Length - 2
                        root += info.URI.Segments(i)

                        If i > 1 Then
                            CreateFolder(root)
                        End If
                    Next
                End If
                Return True
            Catch ex As Exception
                m_sErrorString = ex.Message
                Throw ex
            End Try
        End Function
        ''' <summary>
        ''' Create Folder under DocumentLibrary
        ''' </summary>
        ''' <param name="folderURL"></param>
        ''' <returns></returns>
        Public Function CreateFolder(ByVal folderURL As String) As Boolean
            Dim response As WebResponse
            Try

                Dim request As WebRequest = WebRequest.Create(folderURL)

                request.Credentials = m_credentials
                request.Method = "MKCOL"

                response = request.GetResponse()

                response.Close()
                m_bFolderCreated = True
                Return True

            Catch generatedExceptionName As WebException
                'Ignore if folder already exist else throw error, refer msdn blogs - http://msdn.microsoft.com/en-us/library/aa142923(v=exchg.65).aspx
                If generatedExceptionName.Response IsNot Nothing AndAlso Not DirectCast(generatedExceptionName.Response, System.Net.HttpWebResponse).StatusCode = HttpStatusCode.MethodNotAllowed Then
                    Throw New Exception("CreateFolder - " & generatedExceptionName.Message)
                    'End If
                ElseIf generatedExceptionName.Message.Contains("(405)") Then
                    m_sErrorString = ""
                    Return True
                Else
                    m_sErrorString = generatedExceptionName.Message
                    Return False
                End If
            End Try

        End Function

        ''' <summary>
        ''' Get file id of document 
        ''' </summary>
        ''' <param name="rootUrl"></param>
        ''' <param name="docLib"></param>
        ''' <param name="destinationUrl"></param>
        ''' <param name="obSIRSharePointApi"></param>
        ''' <returns></returns>
        Public Function GetFileID(ByVal rootUrl As String, ByVal docLib As String, ByVal destinationUrl As String, ByRef obSIRSharePointApi As bSIRSharepointApi.bSIRSharepointApiCls) As Integer
            Try
                Return obSIRSharePointApi.GetFileId(rootUrl, docLib, destinationUrl)
            Catch ex As Exception
                m_sErrorString = ex.Message
                Throw New Exception(ex.Message)
                Return Nothing
            End Try
        End Function
    End Class
End Namespace
