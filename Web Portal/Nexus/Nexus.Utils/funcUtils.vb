Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Xml.Linq
Imports System.Xml.XPath
Imports System.Runtime.InteropServices
Imports System.Xml.Serialization
Imports Nexus.Library.RdlReportSchema

Public Module funcUtils

    'Random collection of functions that have no home anywhere else

    Public ReadOnly Property WebRoot() As String
        Get
            Dim sWebRoot As String

            If Not HttpContext.Current.Cache("WebRoot") Is Nothing Then
                sWebRoot = CType(HttpContext.Current.Cache("WebRoot"), String)
            Else
                Dim iDepth As Integer = Configuration.WebConfigurationManager.AppSettings("DepthBelowParent")

                'Get Top Level WebRoot
                sWebRoot = Configuration.WebConfigurationManager.AppSettings("WebRoot")

                If iDepth > 0 Then
                    'Get Child application webroot and append to top level webroot

                    Dim sPath As String = "~/"
                    While iDepth > 0
                        sPath &= "../"
                        iDepth -= 1
                    End While

                    Dim WebConfig As System.Configuration.Configuration _
                        = Configuration.WebConfigurationManager.OpenWebConfiguration(sPath)

                    Dim ConfElement As System.Configuration.KeyValueConfigurationElement _
                        = WebConfig.AppSettings.Settings("WebRoot")

                    If ConfElement Is Nothing Then
                    Else
                        sWebRoot = WebConfig.AppSettings.Settings("WebRoot").Value & sWebRoot
                    End If

                End If

                HttpContext.Current.Cache.Insert("WebRoot", sWebRoot, Nothing, Cache.CacheExpiration(Cache.CacheLengthTypes.CacheLong), TimeSpan.Zero)

            End If

            Return sWebRoot

        End Get
    End Property

    Function GetRootConfigElement(ByVal pElementName As String) As String

        'Gets a value from the root web application, application
        'calling this function may be a sub level application

        Dim iDepth As Integer = Configuration.WebConfigurationManager.AppSettings("DepthBelowParent")

        Dim sPath As String = "~/"

        While iDepth > 0
            sPath &= "../"
            iDepth -= 1
        End While

        Dim sElementValue As String

        Dim WebConfig As System.Configuration.Configuration _
            = Configuration.WebConfigurationManager.OpenWebConfiguration(sPath)

        Dim ConfElement As System.Configuration.KeyValueConfigurationElement _
            = WebConfig.AppSettings.Settings(pElementName)

        If ConfElement Is Nothing Then
            sElementValue = String.Empty
        Else
            sElementValue = ConfElement.Value
        End If

        Return sElementValue

    End Function

    Function GetThemeConfigElement(ByRef sender As Object,
                                                ByVal pConfigFileName As String,
                                                ByVal pElementName As String,
                                                Optional ByVal pThemeName As String = Nothing) As String

        Dim sElementValue As String = String.Empty
        Dim sPath As String = String.Empty

        If pThemeName Is Nothing Then
            sPath = HttpContext.Current.Server.MapPath("~/App_Themes/" & sender.Theme & "/" & pConfigFileName)
        Else
            sPath = HttpContext.Current.Server.MapPath("~/App_Themes/" & pThemeName & "/" & pConfigFileName)
        End If

        Dim WebConfig As System.Configuration.Configuration _
            = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(sPath)

        Dim ConfElement As System.Configuration.KeyValueConfigurationElement _
            = WebConfig.AppSettings.Settings(pElementName)

        If ConfElement Is Nothing Then
            sElementValue = String.Empty
        Else
            sElementValue = ConfElement.Value
        End If

        Return sElementValue

    End Function

    Function GetTemplate(ByVal pTemplateFile As String, ByRef pPage As UI.Page) As ITemplate

        If Not HttpContext.Current.Cache(pTemplateFile) Is Nothing Then
            Return CType(HttpContext.Current.Cache(pTemplateFile), ITemplate)
        Else
            Dim Template As ITemplate = pPage.LoadTemplate(pTemplateFile)
            ' ...and save it into the cache, with a   dependency to the original 
            ' file, so that if the file is modified the cache  item is  removed
            HttpContext.Current.Cache.Insert(pTemplateFile, Template, New Caching.CacheDependency(HttpContext.Current.Server.MapPath(pTemplateFile)), Cache.CacheExpiration(Cache.CacheLengthTypes.CacheLong), TimeSpan.Zero)
            Return Template
        End If

    End Function

    'Common function used to set the default item on all of the above ddl population functions
    Sub SetDefaultOnList(ByRef pListObject As ListControl, ByVal pDefault As String)

        If pListObject.GetType Is GetType(DropDownList) _
            Or pListObject.GetType Is GetType(RadioButtonList) _
            Or LCase(pListObject.GetType.Name) = "gislist" Then

            'Set selected item
            'updated to look for the default in either the text or value field
            If pDefault Is Nothing Then
                pListObject.SelectedIndex = 0
            Else
                Dim liFound As ListItem = pListObject.Items.FindByText(pDefault)
                If liFound Is Nothing Then
                    'see if the default is one of the values
                    liFound = pListObject.Items.FindByValue(pDefault)
                    If liFound Is Nothing Then
                        pListObject.SelectedIndex = 0
                    Else
                        pListObject.SelectedValue = liFound.Value
                    End If
                Else
                    pListObject.SelectedValue = liFound.Value
                End If
            End If
        End If

    End Sub

    Sub CreateMessageAlert(ByVal senderPage As System.Web.UI.Page, ByVal pMsg As String, ByVal pKey As String)

        Dim strScript As String = "<script language=JavaScript>alert('" & pMsg & "')</script>"
        If Not senderPage.ClientScript.IsStartupScriptRegistered(pKey) Then
            senderPage.ClientScript.RegisterStartupScript(GetType(String), pKey, strScript)
        End If

    End Sub

    Function DecodeAmp(ByVal pStr As String) As String

        Dim strTmp As String = String.Empty
        If pStr <> "" Then
            strTmp = Replace(pStr, "&amp;", "&")
        End If
        DecodeAmp = strTmp

    End Function

    Function makePassword(ByVal maxLen As Int16) As String

        Dim strNewPass As String = String.Empty
        Dim iCharacter As Int16
        Dim whatsNext As Int16
        Dim upper As Int16
        Dim lower As Int16
        Dim intCounter As Int16
        Randomize()

        For intCounter = 1 To maxLen
            whatsNext = Int((1 - 0 + 1) * Rnd() + 0)
            If whatsNext = 0 Then
                'character
                upper = 122
                lower = 65
            Else
                upper = 57
                lower = 50
            End If
            iCharacter = Int((upper - lower + 1) * Rnd() + lower)

            Dim sAmbiguousCharacters As String = "568[]\^_`BILOSUVbilosuv"

            If sAmbiguousCharacters.Contains(Chr(iCharacter)) Then
                intCounter -= 1
            Else
                strNewPass = strNewPass & Chr(iCharacter)
            End If
        Next

        Return strNewPass

    End Function

    Function CleanUpEditor(ByVal pContent As String) As String

        'Correct bookmarks within page
        pContent = pContent.Replace(HttpContext.Current.Request.RawUrl() & "#", "#")

        Dim sAdminPath As String = String.Empty
        Dim x As Int16
        For x = 0 To HttpContext.Current.Request.Url.Segments.Length - 2
            sAdminPath += HttpContext.Current.Request.Url.Segments(x)
        Next
        pContent = pContent.Replace(sAdminPath, "")

        Return pContent

    End Function

    Function LoadXMLToString(ByVal vFileName As String) As String

        Try
            Return System.IO.File.OpenText(vFileName).ReadToEnd
        Catch ex As Exception
            Return String.Empty
        End Try

    End Function

    Public Sub SetSafeButton(ByVal thisPage As System.Web.UI.Page,
            ByVal button As System.Web.UI.WebControls.Button,
            Optional ByVal sValidationGroup As String = Nothing)

        Dim sb As New System.Text.StringBuilder()
        sb.Append("if (typeof(Page_ClientValidate) == 'function') { ")

        ' set the validation group
        sb.Append("if (Page_ClientValidate(")
        If sValidationGroup Is Nothing Then
        Else
            sb.Append("'")
            sb.Append(sValidationGroup)
            sb.Append("'")
        End If
        sb.Append(") == false) { return false; }} ")

        sb.Append("this.value = 'Please wait...';")
        sb.Append("this.disabled = true;")

        ' use this if using .NET1 - sb.Append(thisPage.Page.GetPostBackEventReference(button))
        'sb.Append(ClientScript.GetPostBackEventReference(button, ""))
        sb.Append(thisPage.Page.ClientScript.GetPostBackEventReference(button, ""))
        sb.Append(";")
        button.Attributes.Add("onclick", sb.ToString())

    End Sub

    Function GetMasterPlaceHolder(ByVal v_oPage As Page,
                                    ByVal v_sPlaceHolderName As String) As ContentPlaceHolder

        'Assumes all pages have at least one masterpage
        Dim oTmp As Object = v_oPage.Master
        Dim oMaster As MasterPage = Nothing

        'Go down to the lowest nested masterpage
        Do
            oMaster = oTmp
            oTmp = oMaster.Master
        Loop Until oTmp Is Nothing

        'Return the request placeholder in the masterpage
        Return oMaster.FindControl(v_sPlaceHolderName)

    End Function

    ''' <summary>
    ''' To encode the URL
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PureUrlEncode(ByVal s As String) As String

        s = Trim(s)
        If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.IndexOf("Chrome", StringComparison.OrdinalIgnoreCase) > -1 Then
            s = Replace(s, "&", "%26")
            s = System.Web.HttpUtility.UrlEncode(s)
            s = Replace(s, "%22", """")
        Else
            s = System.Web.HttpUtility.UrlEncode(s)
            s = Replace(s, "'", "%27")
            s = Replace(s, """", "%22")
        End If
        Return s
    End Function

    ''' <summary>
    ''' To decode the URL
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PureUrlDecode(ByVal s As String) As String

        s = Trim(s)
        s = System.Web.HttpUtility.UrlDecode(s)

        Return s

    End Function

    ''' <summary>
    ''' To encode a string
    ''' </summary>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PureEncode(ByVal sValue As String) As String
        If Not (String.IsNullOrEmpty(sValue)) Then
            sValue = Trim(sValue)
            sValue = Replace(sValue, "'", "%27")
            sValue = Replace(sValue, """", "%22")

        End If

        Return sValue

    End Function

    ''' <summary>
    ''' To decode a string
    ''' </summary>
    ''' <param name="sValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PureDecode(ByVal sValue As String) As String

        sValue = Trim(sValue)
        sValue = System.Web.HttpUtility.HtmlDecode(sValue)
        sValue = Replace(sValue, "%27", "'")

        Return sValue

    End Function

    ''' <summary>
    ''' Encrypt Password Entered into New password Text field with old Encrypting Logic
    ''' </summary>
    ''' <param name="sPassword">New password</param>
    ''' <returns>Encrypted New password</returns>
    ''' <remarks></remarks>
    Public Function Encrypt(ByVal sPassword As String) As String
        Dim sEncryptedPassword As String = String.Empty
        Dim sEncryptBuilder As New System.Text.StringBuilder
        Dim nCntr As Integer
        Dim sChar1 As Char
        Dim sChar2 As Char
        Dim nSumAsc As Integer
        Dim sCodeString As String = ""
        Dim nClen As Integer

        Try
            ' Encrypts the supplied string returning the encrypted
            ' result. Encrypted string will always be 2 characters
            ' longer than original (leave space!)
            '
            ' Encrypted string contains only ASCII characters in
            ' range 32-126

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            nClen = sCodeString.Length

            nCntr = sPassword.Length

            If nCntr < 1 Then
                sEncryptedPassword = ""
                Return sEncryptedPassword
            End If

            sChar1 = sCodeString.Substring((Strings.Asc(sPassword.Substring(0, 1)(0)) + nCntr) Mod nClen, 1)
            sChar2 = sCodeString.Substring(Strings.Asc(sPassword.Substring(sPassword.Length - 1)(0)) Mod nClen, 1)
            nSumAsc = ((Strings.Asc(sChar1) + Strings.Asc(sChar2)) Mod nClen) + 1
            sEncryptBuilder = New System.Text.StringBuilder(sChar2)

            For iCntr2 As Integer = 1 To nCntr
                sEncryptBuilder.Append(sCodeString.Substring((Strings.Asc(sPassword.Substring(iCntr2 - 1, 1)(0)) + nSumAsc + iCntr2) Mod nClen, 1))
            Next iCntr2

            sEncryptBuilder.Append(sChar1)

            ' Return the result.
            sEncryptedPassword = sEncryptBuilder.ToString().Trim()
            Return sEncryptedPassword

        Catch excep As System.Exception
            sEncryptedPassword = ""
            Return sEncryptedPassword
        End Try
    End Function
    Public Function PureStringEncode(ByVal value As String) As String
        value = Trim(value)
        value = Replace(value, "'", "\'")
        Return value
    End Function
    Public Function stripDomainPrefix(FQDUser As String) As String
        Dim sUserPath As String() = FQDUser.Split(New Char() {"\"c})
        stripDomainPrefix = sUserPath((sUserPath.Length - 1))
    End Function
    Public Function GetSubReportDetails(reportPath As String, reportName As String) As SubReportQuery
        Dim reportWithPath As String = reportPath + reportName + ".rdlc"
        Dim doc As New XDocument
        doc = XDocument.Load(reportPath & "\" & reportName & ".rdlc")
        Dim query As New SubReportQuery
        query.DataSourceName = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']/*[local-name()='DataSet']/*[local-name()='Query']/*[local-name()='DataSourceName']")
        query.CommandText = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']/*[local-name()='DataSet']/*[local-name()='Query']/*[local-name()='CommandText']")
        query.CommandType = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']/*[local-name()='DataSet']/*[local-name()='Query']/*[local-name()='CommandType']")

        Return query
    End Function
    Private Sub RemoveNamespaces(ByRef element As XElement)
        If element.HasAttributes Then
            element.Attributes().Where(Function(a) a.IsNamespaceDeclaration).Remove()
        End If

        element.Name = element.Name.LocalName

        For Each child As XElement In element.Elements()
            RemoveNamespaces(child)
        Next
    End Sub
    Public Function GetReportDetails(reportPath As String, reportName As String,
<[Optional]> isServerPath As Boolean) As Report
        If Not isServerPath Then
            Dim doc As New XDocument
            doc = XDocument.Load(reportPath & reportName & ".rdl")
            Dim xElementDataSets = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='DataSets']")
            RemoveNamespaces(xElementDataSets)
            Dim xElementParameters = doc.XPathSelectElement("/*[local-name() = 'Report']/*[local-name()='ReportParameters']")
            RemoveNamespaces(xElementParameters)

            Dim serializer = New XmlSerializer(GetType(DataSets))
            Dim report = New Report()
            report.DataSets = New DataSets()
            report.DataSets = CType(serializer.Deserialize(xElementDataSets.CreateReader()), DataSets)

            serializer = New XmlSerializer(GetType(ReportParameters))
            report.ReportParameters = New ReportParameters()
            report.ReportParameters = CType(serializer.Deserialize(xElementParameters.CreateReader()), ReportParameters)
            Return report
        End If
        Return Nothing
    End Function

End Module

