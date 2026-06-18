Namespace Nexus
    Partial Class controls_HelpLink
        Inherits System.Web.UI.UserControl


        Private m_Key As String


        Public Property Key() As String
            Get
                Return m_Key
            End Get
            Set(ByVal value As String)
                m_Key = value
            End Set
        End Property


        Private m_displayHelpText As String

        Public Property DisplayHelpText() As String
            Get
                Return m_displayHelpText
            End Get
            Set(ByVal value As String)
                m_displayHelpText = value
            End Set
        End Property


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim helpText As String = String.Empty


            If Not DisplayHelpText = String.Empty Then
                Me.lblHelpText.Text = DisplayHelpText
            ElseIf Not m_Key = String.Empty Then
                helpText = GetLocalResourceObject(m_Key)
                If helpText = String.Empty Then
                    Me.Visible = False
                Else
                    Me.lblHelpText.Text = helpText
                End If
            Else
                Me.Visible = False
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            Page.ClientScript.RegisterClientScriptInclude("HelpToggle", "~/js/helptoggle.js")

        End Sub
    End Class

End Namespace