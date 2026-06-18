Namespace Frontend

    Public MustInherit Class CMSMasterPage
        Inherits System.Web.UI.MasterPage

        Private iDepth As Integer = 0
        Private iSiteMapID As Integer = -1
        Private sBanner As String
        Private iParentID As Integer = 0

        Public Overridable WriteOnly Property Path() As String
            Set(ByVal value As String)
            End Set
        End Property

        Public Overridable WriteOnly Property Title() As String
            Set(ByVal value As String)
                Page.Title = value
            End Set
        End Property

        Public Overridable WriteOnly Property SubTitle() As String
            Set(ByVal value As String)
            End Set
        End Property

        Public Overridable WriteOnly Property CornerImageUrl() As String
            Set(ByVal value As String)
            End Set
        End Property

        Public Overridable WriteOnly Property MetaTags() As String
            Set(ByVal value As String)
            End Set
        End Property

        Public Property Depth() As Integer
            Get
                Return iDepth
            End Get
            Set(ByVal value As Integer)
                iDepth = value
            End Set
        End Property

        Public Overridable Property ParentID() As Integer
            Set(ByVal value As Integer)
                iParentID = value
            End Set
            Get
                Return iParentID
            End Get
        End Property

        Public Overridable Property SiteMapID() As Integer
            Set(ByVal value As Integer)
                iSiteMapID = value
            End Set
            Get
                Return iSiteMapID
            End Get
        End Property

        Public Property BannerLiteral() As String
            Get
                Return sBanner
            End Get
            Set(ByVal value As String)
                sBanner = value
            End Set
        End Property

        Public Sub SetBannerFromID(ByVal strBannerID As String)
            Dim BannerID As Integer
            Try
                BannerID = Integer.Parse(strBannerID)
            Catch
                Exit Sub
            End Try
            Dim BannerContent As String = ""
            Select Case BannerID
                Case 0 'No Banner
                  Case Else 'Specific Banner
                    BannerContent = Banner.Functions.ExposeBannerContent(BannerID)
            End Select
            sBanner = BannerContent
        End Sub

    End Class

End Namespace