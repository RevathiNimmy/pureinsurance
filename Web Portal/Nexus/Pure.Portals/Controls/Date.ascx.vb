Namespace Nexus
    Partial Class controls_Date
        Inherits System.Web.UI.UserControl

        Public WriteOnly Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                TxtRenewalDate.Enabled = value
                ImgCalendar.Visible = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Text = TxtRenewalDate.Text
            End Get
            Set(ByVal value As String)
                TxtRenewalDate.Text = value
            End Set
        End Property

        Public WriteOnly Property rvDateVGroup() As String
            Set(ByVal value As String)
                rvDate.ValidationGroup = value
            End Set
        End Property

        Public WriteOnly Property rvDateEnabled() As Boolean
            Set(ByVal value As Boolean)
                rvDate.Enabled = value
            End Set
        End Property

        Public WriteOnly Property vldrqdDateVGroup() As String
            Set(ByVal value As String)
                vldrqdDate.ValidationGroup = value
            End Set
        End Property

        Public WriteOnly Property vldrqdDateEnabled() As Boolean
            Set(ByVal value As Boolean)
                vldrqdDate.Enabled = value
            End Set
        End Property

        Public WriteOnly Property TabIndex() As Integer
            Set(ByVal value As Integer)
                TxtRenewalDate.TabIndex = value
            End Set
        End Property

        Public WriteOnly Property ImgCalendarTabIndex() As Integer
            Set(ByVal value As Integer)
                ImgCalendar.TabIndex = value
            End Set
        End Property

        Public WriteOnly Property MaxValue() As Date
            Set(ByVal value As Date)
                rvDate.MaximumValue = value
            End Set
        End Property

        Public WriteOnly Property MinValue() As Date
            Set(ByVal value As Date)
                rvDate.MinimumValue = value
            End Set
        End Property

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'rvDate.MaximumValue = Date.Today

            hypCalendarPopup.NavigateUrl = "javascript:GetDate('" & TxtRenewalDate.UniqueID & "')"
        End Sub

        Public WriteOnly Property rqdDateErrorMsg() As String
            Set(ByVal value As String)
                vldrqdDate.ErrorMessage = value
            End Set
        End Property

        Public WriteOnly Property rvDateErrorMsg() As String
            Set(ByVal value As String)
                rvDate.ErrorMessage = value
            End Set
        End Property
    End Class
End Namespace