<Serializable()> Public Class TaskGroupTask

    Private iTaskKeyField As Integer

    Private sNameField As String

    Private dteffectiveDateField As DateTime

    Private sDescriptionField As String

    Private bIsDeletedField As Boolean

    Private bIsIncludedField As Boolean

    Private bIsViewOnlyField As Boolean

    Private bIsAvailableField As Boolean

    Private iTaskCategoryKeyField As Integer

    Private iDisplayIconField As Integer

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Task Key : " & iTaskKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Name : " & sNameField & "<br />")
        sbPrint.AppendLine("Effective Date : " & dteffectiveDateField.ToString() & "<br />")
        sbPrint.AppendLine("Description : " & sDescriptionField & "<br />")
        sbPrint.AppendLine("Deleted : " & bIsDeletedField.ToString() & "<br />")
        sbPrint.AppendLine("Included : " & bIsIncludedField.ToString() & "<br />")
        sbPrint.AppendLine("View Only : " & bIsViewOnlyField.ToString() & "<br />")
        sbPrint.AppendLine("Available : " & bIsAvailableField.ToString() & "<br />")
        sbPrint.AppendLine("Task Category Key : " & iTaskCategoryKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Display Icon : " & iDisplayIconField.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property TaskKey() As Integer
        Get
            Return Me.iTaskKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iTaskKeyField = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return Me.sNameField
        End Get
        Set(ByVal value As String)
            Me.sNameField = value
        End Set
    End Property

    Public Property EffectiveDate() As Date
        Get
            Return Me.dteffectiveDateField
        End Get
        Set(ByVal value As Date)
            Me.dteffectiveDateField = value
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

    Public Property IsDeleted() As Boolean
        Get
            Return Me.bIsDeletedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDeletedField = value
        End Set
    End Property

    Public Property IsIncluded() As Boolean
        Get
            Return Me.bIsIncludedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsIncludedField = value
        End Set
    End Property

    Public Property IsViewOnly() As Boolean
        Get
            Return Me.bIsViewOnlyField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsViewOnlyField = value
        End Set
    End Property

    Public Property IsAvailable() As Boolean
        Get
            Return Me.bIsAvailableField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsAvailableField = value
        End Set
    End Property

    Public Property TaskCategoryKey() As Integer
        Get
            Return Me.iTaskCategoryKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iTaskCategoryKeyField = value
        End Set
    End Property

    Public Property DisplayIcon() As Integer
        Get
            Return Me.iDisplayIconField
        End Get
        Set(ByVal value As Integer)
            Me.iDisplayIconField = value
        End Set
    End Property
End Class

''' <summary>
''' Collection Class to hold taskgroup task objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class TaskGroupTaskCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oTaskGroupTask As TaskGroupTask In List
            sbPrint.AppendLine(oTaskGroupTask.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oTaskGroupTask As TaskGroupTask) As Integer
        Return List.Add(v_oTaskGroupTask)
    End Function

    Public Sub Remove(ByVal v_oTaskGroupTask As TaskGroupTask)
        List.Remove(v_oTaskGroupTask)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As TaskGroupTask
        Get
            Return List(i)
        End Get
        Set(ByVal value As TaskGroupTask)
            List(i) = value
        End Set
    End Property
End Class