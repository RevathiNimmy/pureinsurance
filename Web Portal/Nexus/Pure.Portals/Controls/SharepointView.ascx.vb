Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Net
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.IO
Imports System.Web.HttpContext
Imports System.Web
Imports System
Imports System.Data
Imports System.Security
Imports Microsoft.SharePoint.Client
Imports Microsoft.SharePoint.Client.Utilities
Imports BCrypt.Net
Imports System.Security.Cryptography
Imports System.Security.Cryptography.HashAlgorithm

Namespace Nexus

    Partial Class Controls_SharepointView
        Inherits System.Web.UI.UserControl

#Region "Properties"
        'private declarations used in properties below
        Private _PolicyNumber As String = Nothing
        Private _ClaimNumber As String = Nothing
        Private _PartyShortName As String = Nothing
        Private _Path As String
        Private _EnableEmail As Boolean = True
        Private _Reports As Boolean = False
        Private _Branch As String = String.Empty

        ''' <summary>
        ''' Sets the policy which archived documents will be retrieved for. 
        ''' If not set then will be picked up automatically
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PolicyNumber() As String
            Get
                Return _PolicyNumber
            End Get
            Set(ByVal value As String)
                _PolicyNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the claim which archived documents will be retrieved for. 
        ''' If not set then will be picked up automatically
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClaimNumber() As String
            Get
                Return _ClaimNumber
            End Get
            Set(ByVal value As String)
                _ClaimNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Sets the party which archived documents will be retrieved for. 
        ''' If not set then will be picked up automatically
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PartyShortName() As String
            Get
                Return _PartyShortName
            End Get
            Set(ByVal value As String)
                _PartyShortName = value
            End Set
        End Property

        '''' <summary>
        '''' A Property Path to store the Value of the Folder Path
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
        Public Property Path() As String
            Get
                Return Server.UrlDecode(_Path)
            End Get
            Set(ByVal Value As String)
                _Path = Server.UrlDecode(Value)
            End Set
        End Property


        ''' <summary>
        ''' Boolean value controlling availability of email functionality
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EnableEmail() As Boolean
            Get
                Return _EnableEmail
            End Get
            Set(ByVal value As Boolean)
                _EnableEmail = value
            End Set
        End Property

        ''' <summary>
        ''' A Property Reports flag to identify whether called for reports only
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Reports() As Boolean
            Get
                Return _Reports
            End Get
            Set(ByVal value As Boolean)
                _Reports = value
            End Set
        End Property

        '''' <summary>
        '''' A Property Branch to store the Value of the Report Path
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>
        '''' <remarks></remarks>
        Public Property Branch() As String
            Get
                Return _Branch
            End Get
            Set(ByVal Value As String)
                _Branch = Value
            End Set
        End Property
#End Region

        'set up objets used throughout various methods
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim sBranchCode As String = Nothing
        Dim SharePointCacheID As String

        ''' <summary>
        ''' Page Load Event- Display DocumentList based on  CurrentFolder Path
        ''' </summary> 
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'loop through the controls in the request. if it's a button then that's the one that's been clicked
            For Each sControlName As String In Page.Request.Form
                If sControlName IsNot Nothing Then
                    Dim ctrl As Control = Page.FindControl(sControlName)
                    If TypeOf (ctrl) Is Button Then
                        'there should be only one button in the collection, check it's id and exit
                        If ctrl.ID = "btnRefresh" Then
                            'btnRefresh has been clicked so this is a refresh request
                            'clear the cache then trigger reloading the data
                            Cache.Remove(hdnKey.Value)
                            loadFileList()
                        End If
                        Exit For
                    End If
                End If
            Next

            If Not IsPostBack Then
                'if the user is an agent login then they shouldn't see the view in sharepoint button
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                If (oUserDetails IsNot Nothing AndAlso oUserDetails.Key <> 0) Or Not UserCanDoTask("ViewSharepoint") Then
                    'agent user logged in, or user doesn't have permissions, so hide the view in sharepoint button
                    btnViewInSharepoint.Visible = False
                End If
                'load data into the file list and the folder navigator
                loadFileList()
            End If
        End Sub

        ''' <summary>
        ''' Loads data into the file list 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub loadFileList()
            Dim oSPFileList As New NexusProvider.SharepointFileList

            'Check if we have set location or if we need to get values from session
            If PartyShortName Is Nothing And ClaimNumber Is Nothing And PolicyNumber Is Nothing Then
                'we need to get these values from context held in session
                'todo - this should work for a claim as well as a quote depending on the mode
                Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                    oQuote = CType(Session(CNClaimQuote), NexusProvider.Quote)
                Else
                    oQuote = CType(Session(CNQuote), NexusProvider.Quote)
                End If

                If oQuote IsNot Nothing Then
                    'Getting PartyShortName, PolicyNumber and BranchCode from selected Quote/Policy
                    PartyShortName = Trim(oQuote.ClientCode)
                    PolicyNumber = oQuote.InsuranceFileRef
                    sBranchCode = oQuote.BranchCode
                End If

                If String.IsNullOrEmpty(PartyShortName) Then
                    'get the partyshort name from the party in session rather than the quote
                    'this is missing from the quote when we're in the middle of a new quote
                    If Session(CNParty) IsNot Nothing Then
                        PartyShortName = CType(Session(CNParty), NexusProvider.BaseParty).UserName
                    End If
                End If

                If String.IsNullOrEmpty(PartyShortName) Then
                    'still no partyshort name, try to get it from the claim
                    If Session(CNClaim) IsNot Nothing Then
                        PartyShortName = CType(Session(CNClaim), NexusProvider.Claim).ClientShortName
                    End If
                End If

                If Session(CNClaim) IsNot Nothing Then
                    'During Claim Process
                    'Get Session OClaim Claim Number
                    ClaimNumber = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimNumber
                End If
            End If

            If String.IsNullOrEmpty(hdnSpLoc.Value) And Reports = False Then
                'we haven't specified a path and specifically not called for reports, so fetch the file list using context

                'get the cache id from the hidden field
                SharePointCacheID = hdnKey.Value

                If String.IsNullOrEmpty(SharePointCacheID) Then
                    'we've not set the cache key yet, so generate a new key and add it to the hidden field
                    SharePointCacheID = Guid.NewGuid.ToString()
                    hdnKey.Value = SharePointCacheID
                End If

                If String.IsNullOrEmpty(PartyShortName) Then
                    'still no partyshort name, try to get it from the claim
                    If Session(CNClaim) IsNot Nothing Then
                        PartyShortName = CType(Session(CNClaim), NexusProvider.Claim).ClientShortName
                    End If
                End If

                'check if we have the file list in cache
                If Cache.Item(hdnKey.Value) Is Nothing Then
                    'nothing in cache so get the list of files from SAM and store it in cache
                    oSPFileList = GetSharepointList(PartyShortName, PolicyNumber, ClaimNumber, Path, sBranchCode)
                    If oSPFileList IsNot Nothing Then
                        Cache.Insert(hdnKey.Value, oSPFileList, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    End If
                Else
                    'get the list of files from cache
                    oSPFileList = CType(Cache.Item(hdnKey.Value), NexusProvider.SharepointFileList)
                End If


            ElseIf String.IsNullOrEmpty(hdnSpLoc.Value) And Reports = True Then
                'we haven't specified a path and specifically called for reports, so fetch the file list using context
                If String.IsNullOrEmpty(Branch) Then
                    Path = "General/Reports/" & Replace(Date.Today, "/", "-")
                Else
                    Path = Branch & "/General/Reports/" & Replace(Date.Today, "/", "-")
                End If
                oSPFileList = GetSharepointList(Nothing, Nothing, Nothing, Path, sBranchCode, True)
														 
														
									 
            Else
                'use specified path to get the file list
                Path = hdnSpLoc.Value
                oSPFileList = GetSharepointList(Nothing, Nothing, Nothing, Path, sBranchCode)
            End If

            If oSPFileList IsNot Nothing Then
                'set the path property from the file list
                Path = oSPFileList.FolderPath.FolderPath
                hdnSpLoc.Value = Path

                grdvSharePoint.AllowPaging = True
                'bind the file list to the gridview
                grdvSharePoint.DataSource = oSPFileList.ItemList
                grdvSharePoint.DataBind()
                'load the folder navigation
                loadFolderNavigation()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "SharepointNotFound", "alert('Access to SharePoint failed. Please contact your system administrator.');", True)
            End If
        End Sub

        ''' <summary>
        ''' Loads folder navigation according to the current path
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub loadFolderNavigation()

            If Not String.IsNullOrEmpty(hdnSpLoc.Value) Then
                Dim aryPath As String()
                Dim sPath As String
                Dim aryFolder As String()
                If Reports Then
                    'check if reports flag true specifically called for reports
                    aryPath = Regex.Split(hdnSpLoc.Value, Regex.Escape(Trim(Branch).ToString)) 'split the full path using client code
                    sPath = aryPath(1) 'the path to sharepoint is the first part
                    sPath = Branch & sPath 'add the party code again to keep the full folder chain 
                    aryFolder = sPath.Split("/") 'the rest of sPath will be the sub folders which we need to 
                    aryFolder(0) = Branch 'first part of the array will be missing the first character so ammend to be the client code                
                Else
                    aryPath = Regex.Split(hdnSpLoc.Value, Regex.Escape(Trim(PartyShortName).ToString)) 'split the full path using client code
                    sPath = aryPath(1) 'the path to sharepoint is the first part
                    sPath = Trim(PartyShortName).ToString & sPath 'add the party code again to keep the full folder chain 
                    aryFolder = sPath.Split("/") 'the rest of sPath will be the sub folders which we need to 
                    aryFolder(0) = Trim(PartyShortName) 'first part of the array will be missing the first character so ammend to be the client code                
                End If
                'set up a datatable which we will bind to the repeater to show the path navigation
                Dim dt As New Data.DataTable
                dt.Columns.Add("Name")
                dt.Columns.Add("FullPath")
                For Each sFolder In aryFolder
                    If Not String.IsNullOrEmpty(sFolder) Then
                        If UserCanDoTask("NavigateUpFolders") Then ' If the User is authorized to Nevigate Up the Folder Path then display the complete node
                            dt.Rows.Add(sFolder, aryPath(0) + sFolder + "/")
                            aryPath(0) = aryPath(0) + sFolder + "/"
                        Else ' if the user is not authorized to Nevigate up the Folder Path then display the Current Node only.
                            dt.Rows.Add(sFolder, sFolder)
                            aryPath(0) = sFolder
                        End If
                    End If
                Next

                rptrFolderNavigation.DataSource = dt
                rptrFolderNavigation.DataBind()

            End If
        End Sub

        ''' <summary>
        ''' On load of the gridview check if we have more than one page of data. If not the disable paging so that the page number is not shown
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSharePoint_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvSharePoint.Load
            If grdvSharePoint.PageCount = 1 Then
                grdvSharePoint.AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' Sets the page index and calls method to populate file list
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSharePoint_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSharePoint.PageIndexChanging
            grdvSharePoint.PageIndex = e.NewPageIndex
            loadFileList()
        End Sub

        ''' <summary>
        ''' On binding each row we need to set the visibility of the links depending on whether the row is a file or a folder
        ''' Also sets the class on the checkbox which selects the files so that this can be used client side when launching the email modal
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSharePoint_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSharePoint.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hypFile As LinkButton = e.Row.FindControl("hypFile")
                Dim lnkFolder As LinkButton = e.Row.FindControl("lnkFolder")

                Dim chkMarkedOutTran As CheckBox = CType(e.Row.FindControl("chkMarkedOutTran"), CheckBox)
                If (CType(e.Row.DataItem, NexusProvider.SharepointFileListResponseTypeItemList).ItemType) = Nothing Then
                    'if we don't have an item type then assume this row is a folder, not a file
                    hypFile.Visible = False
                    lnkFolder.Visible = True
                    'hide the checkbox as we shouldn't be able to select a folder
                    If chkMarkedOutTran IsNot Nothing Then
                        chkMarkedOutTran.Visible = False
                    End If
                Else
                    hypFile.Visible = True
                    lnkFolder.Visible = False
                    If chkMarkedOutTran IsNot Nothing Then
                        'add a class name containing the document id to the checkbox so that we can get this client side
                        chkMarkedOutTran.CssClass = "asp-check spID" & e.Row.DataItemIndex.ToString
                    End If
                    'we get the category and sub category ids, so we need to do a reverse lookup to get the text and output these values
                    Dim sCategory As String = String.Empty
                    Dim sSubCategory As String = String.Empty

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                End If
                ScriptManager.GetCurrent(Me.Page).RegisterPostBackControl(hypFile)
            End If


        End Sub

        ''' <summary>
        ''' Method handles click of either a folder link or a file
        ''' In case of a folder then set the path accordingly and rebind the file list
        ''' In the case of a file then this should be streamed out to the browser
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvSharePoint_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSharePoint.RowCommand
            Dim oSPFileList As New NexusProvider.SharepointFileList
            Dim oSystemOption As NexusProvider.OptionTypeSetting
            Dim sSiteRootURl As String = String.Empty
            oWebService = New NexusProvider.ProviderManager().Provider
            oSystemOption = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5177)


            Dim fileurl As String = e.CommandArgument 'should come from event args of linkbutton clicked
            If e.CommandName = "Folder" Then
                'a folder link has been clicked so we need to update the path and reload the data
                'add the clicked folder to the path
                hdnSpLoc.Value = hdnSpLoc.Value + e.CommandArgument + "/"
                Path = hdnSpLoc.Value
                'load the file list and navigation using the new path
                loadFileList()
            ElseIf e.CommandName <> "Page" Then

                If Trim(oSystemOption.OptionValue) = "1" Then

                    oWebService = New NexusProvider.ProviderManager().Provider
                    oSystemOption = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5085) 'URl
                    If oSystemOption IsNot Nothing Then
                        sSiteRootURl = oSystemOption.OptionValue
                    End If

                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolTypeExtensions.Tls12 Or SecurityProtocolType.Ssl3
                    Using clientContext As ClientContext = New ClientContext(sSiteRootURl)
                        clientContext.Credentials = SetSystemoptionCredentials()
                        Dim web As Microsoft.SharePoint.Client.Web = clientContext.Web
                        clientContext.Load(web, Function(website) website.ServerRelativeUrl)
                        clientContext.ExecuteQuery()

                        Dim regex As New Regex(sSiteRootURl, RegexOptions.IgnoreCase)
                        Dim strSiteRelavtiveURL As String = regex.Replace(fileurl, String.Empty)
                        Dim strServerRelativeURL As String
                        strServerRelativeURL = UrlCombine(web.ServerRelativeUrl, strSiteRelavtiveURL)
                        Dim oFile As Microsoft.SharePoint.Client.File = web.GetFileByServerRelativeUrl(strServerRelativeURL)
                        clientContext.Load(oFile)
                        Dim stream As ClientResult(Of Stream) = oFile.OpenBinaryStream()
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                        clientContext.ExecuteQuery()
                        Using mStream As New System.IO.MemoryStream()
                            If stream IsNot Nothing Then
                                stream.Value.CopyTo(mStream)
                                Dim nbyteArray As Byte() = mStream.ToArray()
                                Dim b64String As String = Convert.ToBase64String(nbyteArray)

                                'write the buffer back out to the response
                                Response.Clear()
                                Response.ClearContent()
                                Response.ClearHeaders()

                                Response.Buffer = True
                                'Response.AddHeader("Content-Disposition", "attachment; filename=" & fileurl.Split("/")(fileurl.Split("/").Length - 1))
                                Response.AddHeader("Content-Disposition", "attachment; filename=""" & fileurl.Split("/")(fileurl.Split("/").Length - 1) & "")

                                Response.ContentType = "Content-Disposition"
                                Response.BinaryWrite(mStream.ToArray)
                                Response.End()
                            End If
                        End Using
                    End Using
                Else
                    'we've clicked on a file, so we need to stream this from sharepoint to the browser

                    Dim oRequest As HttpWebRequest = CType(WebRequest.Create(fileurl), HttpWebRequest)
                    oRequest.UseDefaultCredentials = True
                    oRequest.Method = "GET"

                    'set up a buffer and read the file into it
                    Using oMemoryStream As New MemoryStream
                        Using oResponse As HttpWebResponse = CType(oRequest.GetResponse(), HttpWebResponse)
                            'set the buffer length by reading the header from the response
                            Dim oBuffer(oResponse.Headers("Content-Length")) As Byte
                            Using oStream As Stream = oResponse.GetResponseStream()
                                Dim count As Integer
                                Do
                                    count = oStream.Read(oBuffer, 0, oBuffer.Length)
                                    oMemoryStream.Write(oBuffer, 0, count)
                                Loop While (count > 0)
                            End Using

                            'write the buffer back out to the response
                            Response.Clear()
                            Response.ClearContent()
                            Response.ClearHeaders()

                            Response.Buffer = True
                            Response.AddHeader("Content-Disposition", "attachment; filename=" & fileurl.Split("/")(fileurl.Split("/").Length - 1))

                            Response.ContentType = oResponse.ContentType
                            Response.BinaryWrite(oMemoryStream.ToArray)
                            Response.End()
                        End Using
                    End Using
                End If

            End If
        End Sub


#Region "Sharepoint Online methods"
        Private Function SetSystemoptionCredentials() As SharePointOnlineCredentials
            Dim oSystemOption As NexusProvider.OptionTypeSetting
            Dim sSharePointUserName As String = String.Empty
            Dim sSharepointPassword As String = String.Empty
            Dim bIsSharepointOnline As Boolean = False
            oWebService = New NexusProvider.ProviderManager().Provider
            oSystemOption = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5178) 'Username
            If oSystemOption IsNot Nothing Then
                sSharePointUserName = oSystemOption.OptionValue
            End If
            oSystemOption = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5179) 'Pass
            If oSystemOption IsNot Nothing Then
                sSharepointPassword = DecryptPassword(oSystemOption.OptionValue, "SiriusArchitecture")
            End If

            Return New SharePointOnlineCredentials(sSharePointUserName, SecureStore(sSharepointPassword))
        End Function
        ''' <summary>
        ''' DecryptPassword 
        ''' </summary>
        ''' <param name="sEncryptedString"></param>
        ''' <param name="sKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
    Public Function DecryptPassword(ByVal sEncryptedString As String, ByVal sKey As String) As String
            Try
                Dim key As Byte() = SHA256Hash(sKey)
                Dim fullCipher As Byte() = Convert.FromBase64String(sEncryptedString)

                Using aes As Aes = Aes.Create()
                    aes.Key = key
                    aes.Mode = CipherMode.CBC
                    aes.Padding = PaddingMode.PKCS7

                    ' Extract IV (first 16 bytes) and cipher text
                    Dim iv(15) As Byte
                    Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length)
                    aes.IV = iv

                    Dim cipherText As Byte() = New Byte(fullCipher.Length - iv.Length - 1) {}
                    Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length)

                    Dim decryptor As ICryptoTransform = aes.CreateDecryptor()
                    Dim decryptedBytes As Byte() = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length)
                    Return Encoding.UTF8.GetString(decryptedBytes)
                End Using
            Catch excep As Exception
                Return String.Empty
        End Try
    End Function
		Private Function SHA256Hash(ByVal value As String) As Byte()
			Using sha256 As SHA256 = SHA256.Create()
				Return sha256.ComputeHash(Encoding.UTF8.GetBytes(value))
			End Using
		End Function
        ''' <summary>
        ''' Securing the string to pass in CSOM lib
        ''' </summary>
        ''' <param name="sPassword"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SecureStore(ByVal sPassword As String) As SecureString
            Dim objSecureString As New SecureString
            For Each c In sPassword.ToCharArray()
                objSecureString.AppendChar(c)
            Next
            Return objSecureString
        End Function
        ''' <summary>
        ''' Combine the urlCombine
        ''' </summary>
        ''' <param name="url1"></param>
        ''' <param name="url2"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UrlCombine(ByVal url1 As String, ByVal url2 As String) As String
            If url1.Length = 0 Then
                Return url2
            End If

            If url2.Length = 0 Then
                Return url1
            End If

            url1 = url1.TrimEnd("/"c, "\"c)
            url2 = url2.TrimStart("/"c, "\"c)

            Return String.Format("{0}/{1}", url1, url2)
        End Function
       

#End Region

        ''' <summary>
        ''' Sets the path according to the link clicked in the folder navigation
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rptrFolderNavigation_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptrFolderNavigation.ItemCommand
            'set the path according to the link clicked, and then rebind the file list and navigation
            hdnSpLoc.Value = e.CommandArgument
            Path = e.CommandArgument
            loadFileList()
        End Sub

        Protected Sub rptrFolderNavigation_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptrFolderNavigation.ItemDataBound
            If Not UserCanDoTask("NavigateUpFolders") Then
                If (e.Item.ItemType = ListItemType.Item) Or _
                (e.Item.ItemType = ListItemType.AlternatingItem) Then
                    Dim btnSelectButton As LinkButton = CType(e.Item.FindControl("hypPath"), LinkButton)
                    btnSelectButton.Enabled = False
                End If
            End If

        End Sub
        Private Function GetSharepointList(ByVal sPartyShortName As String, ByVal sPolicyNumber As String, ByVal sClaimNumber As String, ByVal sPath As String, _
                                      ByVal sBranchCode As String, Optional ByVal bCreateFolder As Boolean = False) As NexusProvider.SharepointFileList
            Try
                Dim oSPFileList As New NexusProvider.SharepointFileList
                oSPFileList = oWebService.GetSharePointFileList(PartyShortName, PolicyNumber, ClaimNumber, Path, sBranchCode, bCreateFolder)
                Return oSPFileList
            Catch ex As Exception
                Return Nothing
            End Try
        End Function																				
    End Class
End Namespace
