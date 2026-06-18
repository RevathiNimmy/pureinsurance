''' <summary>
''' Property Class for TaskGroup
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Task

    Private sCodeField As String

    Private sDescriptionField As String

    Private sCaptionIdField As Integer

    Private bIsDeletedField As Boolean

    Private iTaskGroupCategoryKeyField As Integer

    Private dtEffectiveDateField As DateTime

    Private nTaskGroupKeyField As Integer

    Private sTaskCode As String

    Private iDisplaySequence As Integer

    Private bDisplaySequenceSpecified As Boolean

    Private bTimeStampField() As Byte

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Code : " & sCodeField & "<br />")
        sbPrint.AppendLine("Description : " & sDescriptionField & "<br />")
        sbPrint.AppendLine("Caption Id : " & sCaptionIdField.ToString() & "<br />")
        sbPrint.AppendLine("Deleted : " & bIsDeletedField.ToString() & "<br />")
        sbPrint.AppendLine("Task Group Category Key : " & iTaskGroupCategoryKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Effective Date : " & dtEffectiveDateField & "<br />")
        sbPrint.AppendLine("Task Group Key : " & nTaskGroupKeyField.ToString() & "<br />")
        sbPrint.Append("Time Stamp : ")

        If bTimeStampField IsNot Nothing Then

            For Each oByte As Byte In bTimeStampField
                sbPrint.Append(oByte.ToString & " | ")
            Next
        End If

        sbPrint.AppendLine("<br />")
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

    Public Property CaptionId() As Integer
        Get
            Return Me.sCaptionIdField
        End Get
        Set(ByVal value As Integer)
            Me.sCaptionIdField = value
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

    Public Property TaskGroupCategoryKey() As Integer
        Get
            Return Me.iTaskGroupCategoryKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iTaskGroupCategoryKeyField = value
        End Set
    End Property

    Public Property TaskGroupKey() As Integer
        Get
            Return Me.nTaskGroupKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nTaskGroupKeyField = value
        End Set
    End Property

    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStampField
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStampField = value
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

    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaskCode() As String
        Get
            Return Me.sTaskCode
        End Get
        Set(ByVal value As String)
            Me.sTaskCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property DisplaySequence() As Integer
        Get
            Return Me.iDisplaySequence
        End Get
        Set(ByVal value As Integer)
            Me.iDisplaySequence = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property DisplaySequenceSpecified() As Boolean
        Get
            Return Me.bDisplaySequenceSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bDisplaySequenceSpecified = value
        End Set
    End Property

    Public Property Urgent() As Integer

    Public Property TaskStatusKey() As Integer

    Public Property DueDate() As Date

    Public Property Customer() As String

    Public Property Type() As String

    Public Property UserGroupKey() As Integer

    Public Property UserKey() As Integer

    Public Property TaskInstanceKey() As Integer

    Public Property UserGroupCode() As String

    Public Property UserGroupDescription() As String

    Public Property UserCode() As String

    Public Property TaskKey() As Int32

    Public Property PartyKey() As Integer

    Public Property PartyName() As String

    Public Property IsExternalItem() As Boolean

    Public Property GuidPMExternalItem() As String

    Public Property ParentTaskKey() As Integer

End Class

''' <summary>
''' Collection Class for Task Group
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class TaskCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oTask As Task In List
            sbPrint.AppendLine(oTask.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oTask As Task) As Integer
        Return List.Add(v_oTask)
    End Function

    Public Sub Remove(ByVal v_oTask As Task)
        List.Remove(v_oTask)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Task
        Get
            Return List(i)
        End Get
        Set(ByVal value As Task)
            List(i) = value
        End Set
    End Property
End Class
