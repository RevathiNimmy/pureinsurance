''' <summary>
''' Nexus DME object, containing the collection of DocumentList and SubFolder
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class DME
    Private oSubFolder As SubFolderCollection
    Private oDocumentList As DocumentListCollection
    Private iParentNum As Integer
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        oSubFolder = New SubFolderCollection
        oDocumentList = New DocumentListCollection
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("ParentNum : " & iParentNum & "<br />")
        Return sbPrint.ToString

    End Function
    ''' <summary>
    ''' ParentNum
    ''' </summary>
    ''' <value>Set the ParentNum</value>
    ''' <returns>Get the ParentNum</returns>
    Public Property ParentNum() As Integer
        Get
            Return iParentNum
        End Get
        Set(ByVal value As Integer)
            iParentNum = value
        End Set
    End Property
    ''' <summary>
    ''' SubFolder
    ''' </summary>
    ''' <value>Set the SubFolder</value>
    ''' <returns>Get the SubFolder</returns>
    Public Property SubFolder() As SubFolderCollection
        Get
            Return oSubFolder
        End Get
        Set(ByVal value As SubFolderCollection)
            oSubFolder = value
        End Set
    End Property
    ''' <summary>
    ''' DocumentList
    ''' </summary>
    ''' <value>Set the DocumentList</value>
    ''' <returns>Get the DocumentList</returns>
    Public Property DocumentList() As DocumentListCollection
        Get
            Return oDocumentList
        End Get
        Set(ByVal value As DocumentListCollection)
            oDocumentList = value
        End Set
    End Property
End Class



