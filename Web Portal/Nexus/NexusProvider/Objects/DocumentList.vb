<Serializable()> Public Class DocumentList
#Region "Private Variables"
    Private iDocNum As Integer
    Private sDocDescription As String
    Private sCreateDate As Date
    Private iFolderNum As Integer
    Private sFolderPath As String
    Private oDocumentType As DMEDocType
    Private iMaxRowsToFetch As Integer
    Private sUploadedBy As String
    Private sCategory As String
    Private sSubCategory As String

#End Region
    
    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("DocNum : " & iDocNum.ToString() & "<br />")
        sbPrint.AppendLine("DocDescription : " & sDocDescription.ToString() & "<br />")
        sbPrint.AppendLine("Created : " & sCreateDate.ToString() & "<br />")
        sbPrint.AppendLine("iFolderNum : " & iFolderNum.ToString() & "<br />")
        sbPrint.AppendLine("sFolderPath : " & sFolderPath.ToString() & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")
        Return sbPrint.ToString()
    End Function

#Region "Public Properties"
    ''' <summary>
    ''' DocNum
    ''' </summary>
    ''' <value>Set the DocNum</value>
    ''' <returns>Get the DocNum</returns>
    Public Property DocNum() As Integer
        Get
            Return Me.iDocNum
        End Get
        Set(ByVal value As Integer)
            Me.iDocNum = value
        End Set
    End Property
    ''' <summary>
    ''' DocDescription
    ''' </summary>
    ''' <value>Set the DocDescription</value>
    ''' <returns>Get the DocDescription</returns>
    Public Property DocDescription() As String
        Get
            Return Me.sDocDescription
        End Get
        Set(ByVal value As String)
            Me.sDocDescription = value
        End Set
    End Property
    ''' <summary>
    ''' FolderNum
    ''' </summary>
    ''' <value>Set the FolderNum</value>
    ''' <returns>Get the FolderNum</returns>
    Public Property FolderNum() As Integer
        Get
            Return Me.iFolderNum
        End Get
        Set(ByVal value As Integer)
            Me.iFolderNum = value
        End Set
    End Property
    ''' <summary>
    ''' FolderPath
    ''' </summary>
    ''' <value>Set the FolderPath</value>
    ''' <returns>Get the FolderPath</returns>
    Public Property FolderPath() As String
        Get
            Return Me.sFolderPath
        End Get
        Set(ByVal value As String)
            Me.sFolderPath = value
        End Set
    End Property
    ''' <summary>
    ''' CreateDate
    ''' </summary>
    ''' <value>Set the CreateDate</value>
    ''' <returns>Get the CreateDate</returns>
    Public Property CreateDate() As Date
        Get
            Return Me.sCreateDate
        End Get
        Set(ByVal value As Date)
            Me.sCreateDate = value
        End Set
    End Property
    ''' <summary>
    ''' DocumentType
    ''' </summary>
    ''' <value>Set the DocumentType</value>
    ''' <returns>Get the DocumentType</returns>
    Public Property DocumentType() As DMEDocType
        Get
            Return Me.oDocumentType
        End Get
        Set(ByVal value As DMEDocType)
            Me.oDocumentType = value
        End Set
    End Property
    ''' <summary>
    ''' MaxRowsToFetch
    ''' </summary>
    ''' <value>Set the MaxRowsToFetch</value>
    ''' <returns>Get the MaxRowsToFetch</returns>
    Public Property MaxRowsToFetch() As Integer
        Get
            Return iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            iMaxRowsToFetch = value
        End Set
    End Property
    ''' <summary>
    ''' UploadedBy - Name of user who uploaded the document
    ''' </summary>
    Public Property UploadedBy() As String
        Get
            Return Me.sUploadedBy
        End Get
        Set(ByVal value As String)
            Me.sUploadedBy = value
        End Set
    End Property
    ''' <summary>
    ''' Category - Document template group description
    ''' </summary>
    Public Property Category() As String
        Get
            Return Me.sCategory
        End Get
        Set(ByVal value As String)
            Me.sCategory = value
        End Set
    End Property
    ''' <summary>
    ''' SubCategory - Document template sub-group description
    ''' </summary>
    Public Property SubCategory() As String
        Get
            Return Me.sSubCategory
        End Get
        Set(ByVal value As String)
            Me.sSubCategory = value
        End Set
    End Property
#End Region
End Class

<Serializable()> Public Class DocumentListCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(DocumentList)
    End Sub
    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        For Each oDocumentList As DocumentType In List
            ' sbPrint.AppendLine(oDocumentList.Print())
            sbPrint.AppendLine("<br />")
        Next
        Return sbPrint.ToString()
    End Function

    Public Function Add(ByVal v_oDocumentList As DocumentList) As Integer
        Return List.Add(v_oDocumentList)
    End Function

    Public Sub Remove(ByVal v_oDocumentList As DocumentList)
        List.Remove(v_oDocumentList)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As DocumentList
        Get
            Return List(i)
        End Get
        Set(ByVal value As DocumentList)
            List(i) = value
        End Set
    End Property

End Class
