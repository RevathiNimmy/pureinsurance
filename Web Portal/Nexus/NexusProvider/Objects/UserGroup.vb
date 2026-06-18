''' <summary>
''' Property Class for User group
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UserGroup

    Private sCodeField As String

    Private sDescriptionField As String

    Private bIsDeletedField As Boolean

    Private dtEffectiveDateField As DateTime

    Private bIsSysAdminField As Boolean

    Private bIsSupervisorField As Boolean

    Private iUserGroupKey As Integer

    Private IsDebtorPMUserGroupField As Boolean

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Description : " & sDescriptionField & "<br />")
        sbPrint.AppendLine("Code : " & sCodeField & "<br />")
        sbPrint.AppendLine("Effective Date : " & dtEffectiveDateField.ToString() & "<br />")
        sbPrint.AppendLine("Deleted : " & bIsDeletedField.ToString() & "<br />")
        sbPrint.AppendLine("SysAdmin : " & bIsSysAdminField.ToString() & "<br />")
        sbPrint.AppendLine("UserGroup Key : " & iUserGroupKey.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property Code() As String
        Get
            Return Me.sCodeField
        End Get
        Set(ByVal value As String)
            Me.sCodeField = value
        End Set
    End Property

    Public Property IsDeleted() As Boolean
        Get
            Return Me.bIsDeletedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDeletedField = value
        End Set
    End Property

    Public Property EffectiveDate() As DateTime
        Get
            Return Me.dtEffectiveDateField
        End Get
        Set(ByVal value As DateTime)
            Me.dtEffectiveDateField = value
        End Set
    End Property

    Public Property IsSysAdmin() As Boolean
        Get
            Return Me.bIsSysAdminField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSysAdminField = value
        End Set
    End Property

    Public Property IsSupervisor() As Boolean
        Get
            Return Me.bIsSupervisorField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSupervisorField = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    Public Property UserGroupKey() As Integer
        Get
            Return iUserGroupKey
        End Get
        Set(ByVal value As Integer)
            iUserGroupKey = value
        End Set
    End Property

    Public Property IsDebtorPMUserGroup() As Boolean
        Get
            Return Me.IsDebtorPMUserGroupField
        End Get
        Set(ByVal value As Boolean)
            Me.IsDebtorPMUserGroupField = value
        End Set
    End Property

End Class


''' <summary>
''' Collection Class to hold usergroup objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UserGroupCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oUserGroup As UserGroup In List
            sbPrint.AppendLine(oUserGroup.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oUserGroup As UserGroup) As Integer
        Return List.Add(v_oUserGroup)
    End Function

    Public Sub Remove(ByVal v_oUserGroup As UserGroup)
        List.Remove(v_oUserGroup)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As UserGroup
        Get
            Return List(i)
        End Get
        Set(ByVal value As UserGroup)
            List(i) = value
        End Set
    End Property
End Class