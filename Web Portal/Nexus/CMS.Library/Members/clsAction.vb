Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Web.HttpContext

Imports Nexus.Utils

Namespace Members

    Public Class Action

        Private iID As Integer
        Private sName, sLabel, sDescription As String
        Private bAllowed As Boolean

        Public Sub New(ByVal v_iID As Integer, ByVal v_sName As String, ByVal v_sLabel As String, _
                        ByVal v_sDescription As String, ByVal v_bAllowed As Boolean)

            iID = v_iID
            sName = v_sName
            sLabel = v_sLabel
            sDescription = v_sDescription
            bAllowed = v_bAllowed

        End Sub

        Public Property ID() As Integer
            Get
                Return iID
            End Get
            Set(ByVal value As Integer)
                iID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return sName
            End Get
            Set(ByVal value As string)
                sName = value
            End Set
        End Property

        Public Property Label() As String
            Get
                Return sLabel
            End Get
            Set(ByVal value As String)
                sLabel = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return sDescription
            End Get
            Set(ByVal value As String)
                sDescription = value
            End Set
        End Property

        Public Property Allowed() As Boolean
            Get
                Return bAllowed
            End Get
            Set(ByVal value As Boolean)
                bAllowed = value
            End Set
        End Property

        Public Shared Function GetActionCategories() As ArrayList

            Dim oActionCategories As ArrayList = Nothing

            Dim command As New SqlCommand("usp_GetActionCategories")
            command.CommandType = CommandType.StoredProcedure

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(command, "CMS")

            If sdrTmp.HasRows Then

                oActionCategories = New ArrayList()

                While sdrTmp.Read

                    oActionCategories.Add(New ActionCategory(sdrTmp("action_cat_id"), sdrTmp("name")))

                End While

            End If

            sdrTmp.Close()
            command.Dispose()

            Return oActionCategories

        End Function

        Public Shared Function GetUserActions(ByVal v_sUserName As String) As Hashtable

            'Initialize hashtable here, even if there are no permissions, as otherwise an
            'empty object will be added to session causing all the permission checks to fail.
            Dim oActions As New Hashtable()

            Dim oCommand As New SqlCommand("usp_GetUserActions")
            oCommand.Parameters.Add("@username", SqlDbType.NVarChar, 256).Value = v_sUserName

            Dim sdrTmp As SqlDataReader = funcDB.ExecSql(oCommand, "CMS")

            If sdrTmp.HasRows Then

                While sdrTmp.Read

                    oActions.Add(sdrTmp("name"), sdrTmp("action_id"))

                End While

            End If

            sdrTmp.Close()
            oCommand.Dispose()

            Return oActions

        End Function

        Public Shared Function CurrentUserHasPermission(ByVal v_sActionRegex As String) As Boolean

            'Use a regular expression to check if the users current
            'session has permission for the specified action(s)

            Dim bHasPermission As Boolean = False
            Dim i As IEnumerator = CType(Current.Session.Item("Actions"), Hashtable).Keys.GetEnumerator()

            While i.MoveNext() And bHasPermission = False
                If Regex.IsMatch(i.Current, v_sActionRegex) Then
                    bHasPermission = True
                End If
            End While

            Return bHasPermission

        End Function

    End Class

    Public Class ActionCategory

        Private iID As Integer
        Private sName As String

        Public Sub New(ByVal v_ID As Integer, ByVal v_sName As String)

            iID = v_ID
            sName = v_sName

        End Sub

        Public Property ID() As Integer
            Get
                Return iID
            End Get
            Set(ByVal value As Integer)
                iID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return sName
            End Get
            Set(ByVal value As String)
                sName = value
            End Set
        End Property

    End Class

End Namespace