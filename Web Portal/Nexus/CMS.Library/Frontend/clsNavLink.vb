Namespace Frontend

    Public Class NavLink
        Private sText As String
        Private sUrl As String
        Private sTarget As String
        Private sSiteID As Integer
        Private sCSSclass As String
        Private iDepth As Integer
        Private sTitle As String
        Private iMenuId As Integer


        Public Sub New(ByVal pText As String)
            sText = pText
        End Sub

        Public ReadOnly Property Text() As String
            Get
                Return sText
            End Get
        End Property

        Public Property Url() As String
            Get
                Return sUrl
            End Get
            Set(ByVal Value As String)
                sUrl = Value
            End Set
        End Property

        Public Property Target() As String
            Get
                Return sTarget
            End Get
            Set(ByVal Value As String)
                sTarget = Value
            End Set
        End Property
        Public Property SiteID() As Integer
            Get
                Return sSiteID
            End Get
            Set(ByVal Value As Integer)
                sSiteID = Value
            End Set
        End Property

        Public Property CSSclass() As String
            Get
                Return sCSSclass
            End Get
            Set(ByVal Value As String)
                sCSSclass = Value
            End Set
        End Property

        Public Property Depth() As Int16
            Get
                Return iDepth
            End Get
            Set(ByVal Value As Int16)
                iDepth = Value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return sTitle
            End Get
            Set(ByVal Value As String)
                sTitle = Value
            End Set
        End Property

        Public Property MenuID() As Integer
            Get
                Return iMenuId
            End Get
            Set(ByVal Value As Integer)
                iMenuId = Value
            End Set
        End Property
    End Class

End Namespace