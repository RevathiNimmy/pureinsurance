''' <summary>
''' Property Class for User information
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class User

    Private iUserIdField As Integer

    Private sUserNameField As String

    Private sFullNameField As String

    Private iUserKey As Integer

    Private sName As String

    Private dtEffectiveDateField As DateTime

    Private sEmailAddressField As String

    '[Start]Property Added as per WPR 73_74

    '[End]Property Added as per WPR 73_74

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("User Id : " & iUserIdField & "<br />")
        sbPrint.AppendLine("User Name : " & sUserNameField & "<br />")
        sbPrint.AppendLine("Full Name : " & sFullNameField & "<br />")
        sbPrint.AppendLine("Effective Date : " & dtEffectiveDateField & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property EmailAddress() As String
        Get
            Return Me.sEmailAddressField
        End Get
        Set(ByVal value As String)
            Me.sEmailAddressField = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property
    Public Property UserKey() As Integer
        Get
            Return Me.iUserKey
        End Get
        Set(ByVal value As Integer)
            Me.iUserKey = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return Me.sUserNameField
        End Get
        Set(ByVal value As String)
            Me.sUserNameField = value
        End Set
    End Property

    Public Property FullName() As String
        Get
            Return Me.sFullNameField
        End Get
        Set(ByVal value As String)
            Me.sFullNameField = value
        End Set
    End Property

    Public Property UserId() As Integer
        Get
            Return Me.iUserIdField
        End Get
        Set(ByVal value As Integer)
            Me.iUserIdField = value
        End Set
    End Property

    Public Property EffectiveDate() As DateTime
        Get
            Return Me.dtEffectiveDateField
        End Get
        Set(ByVal value As Date)
            Me.dtEffectiveDateField = value
        End Set
    End Property
End Class

''' <summary>
''' Collection Class to hold user objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UserCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(User)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oUser As User In List
            sbPrint.AppendLine(oUser.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oUser As User) As Integer
        Return List.Add(v_oUser)
    End Function

    Public Sub Remove(ByVal v_oUser As User)
        List.Remove(v_oUser)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As User
        Get
            Return List(i)
        End Get
        Set(ByVal value As User)
            List(i) = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iUserKey"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public ReadOnly Property UserItem(ByVal v_iUserKey As Integer) As User
    '    Get
    '        Return FindItemByUserKey(v_iUserKey)
    '    End Get
    'End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iUserKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindItemByUserKey(ByVal v_iUserKey As Integer) As User

        For Each oUser As User In List
            If oUser.UserKey = v_iUserKey Then
                Return oUser
            End If
        Next

        Return Nothing

    End Function
End Class
