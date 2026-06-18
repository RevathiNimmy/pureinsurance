Imports System.Text
<Serializable()> Public Class TaskGroup

    Private iTaskGroupKey As Integer
    Private sCode As String
    Private sDescription As String
    Private iCaptionId As Integer
    Private iTaskGroupCategoryKey As Integer
    Private bIsDeleted As Boolean
    Private dtEffectiveDate As Date
    Private bTimeStamp() As Byte
    Private iDisplaySequence As Integer
    Private bIsIncluded As Boolean
    Private iTaskKey As Integer
    Private sName As String
    Private bIsViewOnly As Boolean
    Private bIsAvailable As Boolean
    Private iTaskCategoryKey As Integer
    Private iDisplayIcon As Integer
    Private oTaskGroup As TaskGroupCollection

   

    Public Sub New()

    End Sub

    Public Function Print() As String

        Dim sbPrint As New StringBuilder
        sbPrint.AppendLine(":  <br />")
        sbPrint.AppendLine("WorkManager -----------------><br />")

        'If oTaskGroup IsNot Nothing Then
        'sbPrint.AppendLine(oTaskGroup.Print())
        'sbPrint.AppendLine("<br />")
        'End If

        Return sbPrint.ToString()

    End Function
    Public Property TaskGroup() As TaskGroupCollection
        Get
            Return oTaskGroup
        End Get
        Set(ByVal value As TaskGroupCollection)
            oTaskGroup = value
        End Set
    End Property

    Public Property DisplayIcon() As Integer
        Get
            Return iDisplayIcon
        End Get
        Set(ByVal value As Integer)
            iDisplayIcon = value
        End Set
    End Property

    Public Property TaskCategoryKey() As Integer
        Get
            Return iTaskCategoryKey
        End Get
        Set(ByVal value As Integer)
            iTaskCategoryKey = value
        End Set
    End Property
    Public Property IsAvailable() As Boolean
        Get
            Return bIsAvailable
        End Get
        Set(ByVal value As Boolean)
            bIsAvailable = value
        End Set
    End Property
    Public Property IsViewOnly() As Boolean
        Get
            Return bIsViewOnly
        End Get
        Set(ByVal value As Boolean)
            bIsViewOnly = value
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
    Public Property TaskKey() As Integer
        Get
            Return iTaskKey
        End Get
        Set(ByVal value As Integer)
            iTaskKey = value
        End Set
    End Property
    Public Property DisplaySequence() As Integer
        Get
            Return iDisplaySequence
        End Get
        Set(ByVal value As Integer)
            iDisplaySequence = value
        End Set
    End Property
    Public Property IsIncluded() As Boolean
        Get
            Return bIsIncluded
        End Get
        Set(ByVal value As Boolean)
            bIsIncluded = value
        End Set
    End Property
    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property
    Public Property EffectiveDate() As Date
        Get
            Return dtEffectiveDate
        End Get
        Set(ByVal value As Date)
            dtEffectiveDate = value
        End Set
    End Property
    Public Property IsDeleted() As Boolean
        Get
            Return bIsDeleted
        End Get
        Set(ByVal value As Boolean)
            bIsDeleted = value
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
    Public Property Code() As String
        Get
            Return sCode
        End Get
        Set(ByVal value As String)
            sCode = value
        End Set
    End Property
    Public Property TaskGroupKey() As Integer
        Get
            Return iTaskGroupKey
        End Get
        Set(ByVal value As Integer)
            iTaskGroupKey = value
        End Set
    End Property

    Public Property CaptionId() As Integer
        Get
            Return iCaptionId
        End Get
        Set(ByVal value As Integer)
            iCaptionId = value
        End Set
    End Property

    Public Property TaskGroupCategoryKey() As Integer
        Get
            Return iTaskGroupCategoryKey
        End Get
        Set(ByVal value As Integer)
            iTaskGroupCategoryKey = value
        End Set
    End Property
End Class

<Serializable()> Public Class TaskGroupCollection : Inherits CollectionBase
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New StringBuilder
        sbPrint.AppendLine()
        For Each oTaskGroup As TaskGroup In List
            sbPrint.AppendLine(oTaskGroup.Print())
            sbPrint.AppendLine("<Br/>")
        Next
        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oTaskGroup As TaskGroup) As Integer

        Return List.Add(v_oTaskGroup)

    End Function

    Public Sub Remove(ByVal v_oTaskGroup As TaskGroup)
        List.Remove(v_oTaskGroup)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As TaskGroup

        Get
            Return List(i)
        End Get
        Set(ByVal value As TaskGroup)
            List(i) = value
        End Set

    End Property
End Class