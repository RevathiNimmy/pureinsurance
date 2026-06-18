Imports System.Xml
Imports System.IO
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web
Imports System.Text
Imports System.Web.Security
Imports Nexus.Utils
Imports System.Web.Caching

Namespace SiteMap

    Public Class SiteMapContent

        Inherits ContentHistory

        Private iSiteMapID As Integer
        Private iSchemaID As Integer
        Private sXMLSchema As String
        Private sRoles As String = ""

        Private iParentID As Integer
        Private iDepth As Integer

        Private sLabel As String
        Private dLiveDate As DateTime
        Private dExpiryDate As DateTime

        Private bHidden As Boolean
        Private bHiddenAnonymous As Boolean
        Private bHiddenAuthenticated As Boolean
        Private bRestricted As Boolean

        Private sXMLContent As String
        Private dsXMLContent As DataSet

        Private bValid As Boolean
        Private bEditMode As Boolean
        Public MyCache As System.Web.Caching.Cache

        Public Sub New()
            MyBase.New(0, New Date(1900, 1, 1), "", "")
            bValid = False
        End Sub

        Public Sub New(ByVal HistoryID As Integer)
            'ARCHIVE CONTENT

            'Dont need history, as this constructor is only used for previewing historical content
            MyBase.New(HistoryID, New Date(1900, 1, 1), "", "")

            'Dont allow historical content to be editted
            bEditMode = False

            'Get Content
            Dim command As New SqlCommand("usp_GetHistoricalContent")

            With command.Parameters
                .Add("@history_id", SqlDbType.Int).Value = HistoryID
            End With

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            While sdrTmp.Read

                iSiteMapID = CType(sdrTmp("sitemap_id"), Integer)
                iParentID = CType(sdrTmp("parent_id"), Integer)
                iDepth = CType(sdrTmp("depth"), Integer)
                sLabel = CType(sdrTmp("label"), String)

                If IsDBNull(sdrTmp("live_date")) Then
                    dLiveDate = DateTime.MinValue
                Else
                    dLiveDate = CType(sdrTmp("live_date"), DateTime)
                End If

                If IsDBNull(sdrTmp("expiry_date")) Then
                    dExpiryDate = DateTime.MinValue
                Else
                    dExpiryDate = CType(sdrTmp("expiry_date"), DateTime)
                End If

                bHidden = CType(sdrTmp("hidden"), Boolean)
                bHiddenAnonymous = CType(sdrTmp("anonymousOnly"), Boolean)
                bHiddenAuthenticated = CType(sdrTmp("authenticatedOnly"), Boolean)

                bRestricted = CType(sdrTmp("restricted"), Boolean)
                'sXMLContent = CStr(sdrTmp("xml_content"))

                sXMLContent = CStr(sdrTmp("xml_content")) _
                    .Replace("/[ReplacePathWithWebRoot]/", _
                    System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") _
                    & System.Web.Configuration.WebConfigurationManager.AppSettings("MediaRoot") & "/")

                iSchemaID = CType(sdrTmp("page_type_id"), Integer)
                sXMLSchema = CType(sdrTmp("xsd_schema"), String)

                ParseXML()

            End While

            sdrTmp.Close()
            command.Dispose()

        End Sub

        Public Sub New(ByVal SiteMapID As Integer, ByVal Draft As Boolean, Optional ByVal Restricted As Boolean = True)
            'LIVE OR DRAFT(active) CONTENT

            'Get Last Action History
            MyBase.New(SiteMapID, Draft)
            MyCache = System.Web.HttpContext.Current.Cache
            Dim dt As DataTable
            Dim command As New SqlCommand

            Dim sCacheKey As String = "GetContent_SiteMapID_" & SiteMapID.ToString & "_Draft_" & Draft.ToString

            'Allow values to be set from properties
            bEditMode = True

            iSiteMapID = SiteMapID
            'Retrieve from List from cache if available
            If MyCache.Item(sCacheKey) Is Nothing Then
                'Get Content
                command = New SqlCommand("usp_GetContent")

                With command.Parameters
                    .Add("@sitemap_id", SqlDbType.Int).Value = SiteMapID
                    .Add("@draft", SqlDbType.Bit).Value = Draft
                    If Restricted Then
                        '.Add("@user_id", SqlDbType.Int).Value = funcUtils.NullToZero(HttpContext.Current.User.Identity.Name)
                        '.Add("@user_id", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey

                    Else
                        '***ALLOW UNRESTRICTED ACCESS TO RESTRICTED CONTENT
                        'this was added to allow the news summaries to be visible
                        'to all users, but not allow viewing of the full story
                        .Add("@user_id", SqlDbType.Int).Value = 1
                        '?? TOdo new userid... 
                    End If
                End With

                'Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

                dt = funcDB.GetDataTable(command, "CMS")
                MyCache.Insert(sCacheKey, dt, Nothing)
                command.Dispose()
            Else
                dt = CType(MyCache.Item(sCacheKey), DataTable)
            End If

            'While sdrTmp.Read

            'iParentID = CType(sdrTmp("parent_id"), Integer)
            'iDepth = CType(sdrTmp("depth"), Integer)
            'sLabel = CType(sdrTmp("label"), String)


            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                With dt.Rows(0)

                    iParentID = CType(.Item("parent_id"), Integer)
                    iDepth = CType(.Item("depth"), Integer)
                    sLabel = CType(.Item("label"), String)


                    If IsDBNull(.Item("live_date")) Then
                        dLiveDate = DateTime.MinValue
                    Else
                        dLiveDate = CType(.Item("live_date"), DateTime)
                    End If

                    If IsDBNull(.Item("expiry_date")) Then
                        dExpiryDate = DateTime.MinValue
                    Else
                        dExpiryDate = CType(.Item("expiry_date"), DateTime)
                    End If

                    bHidden = CType(.Item("hidden"), Boolean)
                    bHiddenAnonymous = CType(.Item("anonymousOnly"), Boolean)
                    bHiddenAuthenticated = CType(.Item("authenticatedOnly"), Boolean)

                    bRestricted = CType(.Item("restricted"), Boolean)
                    sXMLContent = CStr(.Item("xml_content")) _
                        .Replace("/[ReplacePathWithWebRoot]/", _
                        System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") _
                        & System.Web.Configuration.WebConfigurationManager.AppSettings("MediaRoot") & "/")
                    iSchemaID = CType(.Item("page_type_id"), Integer)
                    sXMLSchema = CType(.Item("xsd_schema"), String)

                    'If sdrTmp.FieldCount > 10 Then
                    '    If Not IsDBNull(.Item("roles")) Then
                    '        sRoles = CType(.Item("roles"), String)
                    '    End If
                    'End If
                    If Not IsDBNull(.Item("roles")) Then
                        sRoles = CType(.Item("roles"), String)
                    End If

                End With
                ParseXML()
            End If

            'End While
            'sdrTmp.Close()
            'command.Dispose()

        End Sub

        Public Sub New(ByVal SiteMapID As Integer, ByVal ParentID As Integer, ByVal Depth As Integer, _
                        ByVal Label As String, ByVal LiveDate As Date, ByVal ExpiryDate As Date, _
                        ByVal Hidden As Boolean, ByVal HiddenAnonymous As Boolean, ByVal HiddenAuthenticated As Boolean, ByVal Restricted As Boolean, ByVal XMLContent As String, _
                        ByVal PageTypeID As Integer, ByVal XSDSchema As String)

            'LIVE CONTENT ONLY, this is only really used for generating the news panels

            'Get Last Action History
            MyBase.New(SiteMapID, 0)

            'Dont allow editting, as its not need and anything could have
            'been passed to this constructor, bypassing the validation
            bEditMode = False

            iSiteMapID = SiteMapID
            iParentID = ParentID
            iDepth = Depth
            sLabel = Label
            dLiveDate = LiveDate
            dExpiryDate = ExpiryDate
            bHidden = Hidden
            bHiddenAnonymous = HiddenAnonymous
            bHiddenAuthenticated = HiddenAuthenticated
            bRestricted = Restricted
            sXMLContent = XMLContent
            iSchemaID = PageTypeID
            sXMLSchema = XSDSchema

            ParseXML()

        End Sub

        Private Sub ParseXML()

            'Big ******* Bodge

            bValid = True
            dsXMLContent = New DataSet
            dsXMLContent.ReadXmlSchema(New XmlTextReader(New StringReader(sXMLSchema)))

            If sXMLContent Is Nothing Or sXMLContent = "" Then
                dsXMLContent.Tables(0).Rows.Add(dsXMLContent.Tables(0).NewRow)
            Else
                dsXMLContent.ReadXml(New XmlTextReader(New StringReader(sXMLContent)))
            End If

        End Sub

        Private Sub ValidationCallBack(ByVal sender As System.Object, ByVal e As Schema.ValidationEventArgs)
            bValid = False
        End Sub

        Public Property Label() As String
            Get
                Return sLabel
            End Get
            Set(ByVal Value As String)
                If bEditMode Then
                    sLabel = Value
                End If
            End Set
        End Property

        Public Property LiveDate() As DateTime
            Get
                If dLiveDate = DateTime.MinValue Then
                    Return Nothing
                Else
                    Return dLiveDate
                End If
            End Get
            Set(ByVal Value As DateTime)
                If bEditMode Then
                    dLiveDate = Value
                End If
            End Set
        End Property

        Public Property ExpiryDate() As DateTime
            Get
                If dExpiryDate = DateTime.MinValue Then
                    Return Nothing
                Else
                    Return dExpiryDate
                End If
            End Get
            Set(ByVal Value As DateTime)
                If bEditMode Then
                    dExpiryDate = Value
                End If
            End Set
        End Property

        Public Property Hidden() As Boolean
            Get
                Return bHidden
            End Get
            Set(ByVal Value As Boolean)
                If bEditMode Then
                    bHidden = Value
                End If
            End Set
        End Property

        Public Property AnonymousOnly() As Boolean
            Get
                Return bHiddenAnonymous
            End Get
            Set(ByVal value As Boolean)
                If bEditMode Then
                    bHiddenAnonymous = value
                End If
            End Set
        End Property

        Public Property AuthenticatedOnly() As Boolean
            Get
                Return bHiddenAuthenticated
            End Get
            Set(ByVal value As Boolean)
                If bEditMode Then
                    bHiddenAuthenticated = value
                End If
            End Set
        End Property

        Public Property Restricted() As Boolean
            Get
                Return bRestricted
            End Get
            Set(ByVal Value As Boolean)
                If bEditMode Then
                    bRestricted = Value
                End If
            End Set
        End Property

        Public Property RawXML()
            Get
                GenerateXML()
                Return sXMLContent
            End Get
            Set(ByVal value)
                sXMLContent = value
                ParseXML()
            End Set
        End Property

        Public ReadOnly Property RawSchema()
            Get
                Return sXMLSchema
            End Get
        End Property

        Public ReadOnly Property IsValid() As Boolean
            Get
                Return bValid
            End Get
        End Property

        Public Property Element(ByVal Key As String) As Object
            Get
                'How to bind this ??????

                Try
                    If IsDBNull(dsXMLContent.Tables(0).Rows(0).Item(Key)) Then
                        Return ""
                    Else
                        Return dsXMLContent.Tables(0).Rows(0).Item(Key)
                    End If


                Catch ex As Exception
                    'Element not found so return nothing
                    Return ""
                End Try

            End Get
            Set(ByVal Value As Object)
                If bEditMode Then
                    Try
                        dsXMLContent.Tables(0).Rows(0).Item(Key) = Value
                    Catch ex As Exception
                        'Error, but what to do ....
                    End Try
                End If
            End Set
        End Property

        Public ReadOnly Property SiteMapID() As Integer
            Get
                Return iSiteMapID
            End Get
        End Property

        Public ReadOnly Property ParentID() As Integer
            Get
                Return iParentID
            End Get
        End Property

        Public ReadOnly Property Depth() As Integer
            Get
                Return iDepth
            End Get
        End Property

        Public ReadOnly Property SchemaID() As Integer
            Get
                Return iSchemaID
            End Get
        End Property

        Public Property Roles() As String
            Get
                Return sRoles
            End Get
            Set(ByVal Value As String)
                If bEditMode Then
                    sRoles = Value
                End If
            End Set
        End Property

        Public Function WriteToDB() As SPResponse

            Dim response As SPResponse

            If bEditMode Then
                If bValid Then

                    GenerateXML()

                    Dim command As New SqlCommand("usp_AddEditContent")

                    With command
                        .Parameters.Add("@sitemap_id", SqlDbType.Int).Value = iSiteMapID
                        .Parameters.Add("@label", SqlDbType.VarChar, 100).Value = sLabel

                        If dLiveDate = DateTime.MinValue Then
                            .Parameters.Add("@live_date", SqlDbType.DateTime).Value = System.DBNull.Value
                        Else
                            .Parameters.Add("@live_date", SqlDbType.DateTime).Value = dLiveDate
                        End If

                        If dExpiryDate = DateTime.MinValue Then
                            .Parameters.Add("@expiry_date", SqlDbType.DateTime).Value = System.DBNull.Value
                        Else
                            .Parameters.Add("@expiry_date", SqlDbType.DateTime).Value = dExpiryDate
                        End If

                        .Parameters.Add("@hidden", SqlDbType.Bit).Value = bHidden
                        .Parameters.Add("@restricted", SqlDbType.Bit).Value = bRestricted
                        .Parameters.Add("@xml_content", SqlDbType.Text).Value = sXMLContent _
                            .Replace(System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") _
                            & System.Web.Configuration.WebConfigurationManager.AppSettings("MediaRoot"), _
                            "/[ReplacePathWithWebRoot]/")
                        .Parameters.Add("@admin_guid", SqlDbType.UniqueIdentifier).Value = Membership.GetUser.ProviderUserKey
                        .Parameters.Add("@roles", SqlDbType.VarChar).Value = sRoles
                        .Parameters.Add("@HiddenAnonymous", SqlDbType.Bit).Value = bHiddenAnonymous
                        .Parameters.Add("@HiddenAuthenticated", SqlDbType.Bit).Value = bHiddenAuthenticated


                    End With

                    funcDB.ExecNonQuery(command, "CMS")
                    command.Dispose()

                    response = Nothing
                Else
                    response = New SPResponse(0, "XML Content does NOT match schema")
                End If

            Else
                response = New SPResponse(0, "Content can NOT be editted") 'Either historical or live view
            End If

            Return response

        End Function

        Private Sub GenerateXML()

            'Create a stream to output the xml as a string for input into sql
            Dim swContent As New StringWriter
            Dim xmlwContent As New XmlTextWriter(swContent)

            dsXMLContent.WriteXml(swContent, XmlWriteMode.IgnoreSchema)
            sXMLContent = swContent.ToString()

            xmlwContent.Close()
            swContent.Close()

        End Sub

        Public Property CustomProperty(ByVal Name As String) As String
            Get
                Dim dr() As DataRow = dsXMLContent.Tables(2).Select("Name = '" & Name & "'")
                If dr.Length > 0 Then
                    Return dr(0).Item("Value")
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                If dsXMLContent.Tables(1).Rows.Count = 0 Then
                    Dim values() As String = {0, 0}
                    dsXMLContent.Tables(1).Rows.Add(values)
                End If
                Dim dr() As DataRow = dsXMLContent.Tables(2).Select("Name = '" & Name & "'")
                If dr.Length > 0 Then
                    dr(0).Item("Value") = value
                Else
                    Dim values() As String = {Name, value, 0}
                    dsXMLContent.Tables(2).Rows.Add(values)
                End If
            End Set
        End Property

        Public ReadOnly Property CustomProperties() As DataRowCollection
            Get
                Dim ret As DataRowCollection = Nothing
                Try
                    If dsXMLContent IsNot Nothing Then
                        If dsXMLContent.Tables.Count > 2 Then ret = dsXMLContent.Tables(2).Rows
                    End If
                Catch
                End Try
                Return ret
            End Get
        End Property

        Public Sub SetCustomProperties(ByVal props As Collections.Generic.List(Of System.Web.UI.Pair))
            Try
                dsXMLContent.Tables(2).Rows.Clear()
                For Each p As System.Web.UI.Pair In props
                    CustomProperty(p.First) = p.Second
                Next
            Catch
            End Try

        End Sub

    End Class

End Namespace