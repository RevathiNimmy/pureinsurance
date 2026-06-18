<Serializable()> Public Class SubFolder
#Region "Private Variables"

    Private iFolderNum As Integer
    Private iParentNum As Integer
    Private sName As String
    Private sExternalCode As String
    Private iFolderLevel As Integer
    Private dCreateDate As Date
    Private iMaxRowsToFetch As Integer
    Private oSubFolder As SubFolderCollection

#End Region
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("folderNum : " & iFolderNum.ToString() & "<br />")
        sbPrint.AppendLine("ParentNum : " & iParentNum.ToString() & "<br />")
        sbPrint.AppendLine("Name : " & sName.ToString() & "<br />")
        sbPrint.AppendLine("ExternalCode : " & sExternalCode.ToString() & "<br />")
        sbPrint.AppendLine("FolderLevel : " & iFolderLevel.ToString() & "<br />")
        sbPrint.AppendLine("CreateDate : " & dCreateDate.ToString() & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")

        Return sbPrint.ToString()

    End Function

#Region "Public Properties"
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
    ''' ParentNum
    ''' </summary>
    ''' <value>Set the ParentNum</value>
    ''' <returns>Get the ParentNum</returns>
    Public Property ParentNum() As Integer
        Get
            Return Me.iParentNum
        End Get
        Set(ByVal value As Integer)
            Me.iParentNum = value
        End Set
    End Property
    ''' <summary>
    ''' Name
    ''' </summary>
    ''' <value>Set the Name</value>
    ''' <returns>Get the Name</returns>
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property
    ''' <summary>
    ''' ExternalCode
    ''' </summary>
    ''' <value>Set the ExternalCode</value>
    ''' <returns>Get the ExternalCode</returns>
    Public Property ExternalCode() As String
        Get
            Return Me.sExternalCode
        End Get
        Set(ByVal value As String)
            Me.sExternalCode = value
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
    ''' FolderLevel
    ''' </summary>
    ''' <value>Set the FolderLevel</value>
    ''' <returns>Get the FolderLevel</returns>
    Public Property FolderLevel() As Integer
        Get
            Return iFolderLevel
        End Get
        Set(ByVal value As Integer)
            iFolderLevel = value
        End Set
    End Property
    ''' <summary>
    ''' CreateDate
    ''' </summary>
    ''' <value>Set the CreateDatel</value>
    ''' <returns>Get the CreateDate</returns>
    Public Property CreateDate() As Date
        Get
            Return dCreateDate
        End Get
        Set(ByVal value As Date)
            dCreateDate = value
        End Set
    End Property
#End Region

End Class

<Serializable()> Public Class SubFolderCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oSubFolder As SubFolder In List
            sbPrint.AppendLine(oSubFolder.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oSubFolder As SubFolder) As Integer
        Return List.Add(v_oSubFolder)
    End Function

    Public Sub Remove(ByVal v_oSubFolder As SubFolder)
        List.Remove(v_oSubFolder)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As SubFolder
        Get
            Return List(i)
        End Get
        Set(ByVal value As SubFolder)
            List(i) = value
        End Set
    End Property

End Class
