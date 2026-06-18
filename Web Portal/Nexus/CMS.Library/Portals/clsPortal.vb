Namespace Portal

    <Serializable()> Public Class Portal
        Private iID As Integer
        Private sName As String
        Private sUrl As String
        Private sOtherUrls As String
        Private sCultureCode As String
        Private sMasterPageFolder As String
        Private sTheme As String
        Private bAccessAllowed As Boolean

        Public Sub New(ByVal ID As Integer, ByVal Name As String, ByVal Url As String, ByVal OtherUrls As String, _
                        ByVal CultureCode As String, ByVal MasterPageFolder As String, ByVal Theme As String, _
                        ByVal v_bAccessAllowed As Boolean)

            iID = ID
            sName = Name.TrimEnd
            sUrl = Url.TrimEnd
            If Not IsDBNull(OtherUrls) Then
                sOtherUrls = OtherUrls.TrimEnd
            Else
                sOtherUrls = ""
            End If
            sCultureCode = CultureCode
            sMasterPageFolder = MasterPageFolder
            sTheme = Theme
            bAccessAllowed = v_bAccessAllowed

        End Sub

        Public ReadOnly Property ID() As Integer
            Get
                Return iID
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return sName
            End Get
        End Property

        Public ReadOnly Property Url() As String
            Get
                Return sUrl
            End Get
        End Property

        Public ReadOnly Property OtherUrls() As String
            Get
                Return sOtherUrls
            End Get
        End Property

        Public ReadOnly Property CultureCode() As String
            Get
                Return sCultureCode
            End Get
        End Property

        Public ReadOnly Property MasterPageFolder() As String
            Get
                Return sMasterPageFolder
            End Get
        End Property

        Public ReadOnly Property Theme() As String
            Get
                Return sTheme
            End Get
        End Property

        Public ReadOnly Property AccessAllowed() As String
            Get
                Return bAccessAllowed
            End Get
        End Property

    End Class

End Namespace