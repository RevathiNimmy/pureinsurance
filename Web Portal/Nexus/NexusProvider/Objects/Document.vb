''' <summary>
''' Nexus document object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Document

    Private iDocNum As Integer = 0
    Private bCompress As Boolean
    Private bConvertPdf As Boolean
    Private bPdfDocument() As Byte
    Private sDocDescription As String
    Private sFileExtension As String
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("IDocument Number : " & iDocNum.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Document Description : " & sDocDescription & "<br />")
        sbPrint.AppendLine("PDF Document (String format) : " & PdfDocument.Length & "<br />")
        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Document Description
    ''' </summary>
    ''' <value>Set the Document Description</value>
    ''' <returns>Get the Document Description</returns>
    Public Property DocDescription() As String
        Get
            Return sDocDescription
        End Get
        Set(ByVal value As String)
            sDocDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Document Number
    ''' </summary>
    ''' <value>Set the Document Number</value>
    ''' <returns>Get the Document Number</returns>
    Public Property DocNum() As Integer
        Get
            Return iDocNum
        End Get
        Set(ByVal value As Integer)
            iDocNum = value
        End Set
    End Property

    ''' <summary>
    ''' Byte stream of the PDF Document
    ''' </summary>
    ''' <value>Set the PDF Document</value>
    ''' <returns>Get the PDF Document</returns>
    Public Property PdfDocument() As Byte()
        Get
            Return bPdfDocument
        End Get
        Set(ByVal value() As Byte)
            bPdfDocument = value
        End Set
    End Property
    ''' <summary>
    ''' Document FileExtension
    ''' </summary>
    ''' <value>Set the Document FileExtension</value>
    ''' <returns>Get the Document FileExtension</returns>
    Public Property FileExtension() As String
        Get
            Return sFileExtension
        End Get
        Set(ByVal value As String)
            sFileExtension = value
        End Set
    End Property
    Public ReadOnly Property ConvertPdf() As Boolean
        Get
            Return bConvertPdf
        End Get
    End Property
    Public ReadOnly Property Compress() As Boolean
        Get
            Return bCompress
        End Get
    End Property

End Class

''' <summary>
''' Collection of Document objects
''' </summary>
<Serializable()> Public Class DocumentCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oDocument As Document In List
            sbPrint.AppendLine(oDocument.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Document object to the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oDocument As Document) As Integer
        Return List.Add(v_oDocument)
    End Function

    ''' <summary>
    ''' Remove an Document object from the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oDocument As Document)
        List.Remove(v_oDocument)
    End Sub

    ''' <summary>
    ''' Remove an Document object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Document object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Document object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement Document object</value>
    ''' <returns>The Document object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Document
        Get
            Return List(i)
        End Get
        Set(ByVal value As Document)
            List(i) = value
        End Set
    End Property


End Class

'SharePoint Integration 
'----------------------
'----------------------

<Serializable()> Public Class SharepointFileList

    Private oFolderPath As SharepointFolderPath
    Private oItemList As SharepointFileListResponseTypeItemListCollection

    Public Property FolderPath() As SharepointFolderPath
        Get
            Return Me.oFolderPath
        End Get
        Set(ByVal value As SharepointFolderPath)
            Me.oFolderPath = value
        End Set
    End Property

    Public Property ItemList() As SharepointFileListResponseTypeItemListCollection
        Get
            Return Me.oItemList
        End Get
        Set(ByVal value As SharepointFileListResponseTypeItemListCollection)
            Me.oItemList = value
        End Set
    End Property


End Class

<Serializable()> Public Class SharepointFolderPath

    Private sFolderPath As String

    Public Property FolderPath() As String
        Get
            Return Me.sFolderPath
        End Get
        Set(ByVal value As String)
            Me.sFolderPath = value
        End Set
    End Property

End Class


<Serializable()> Public Class SharepointFileListResponseTypeItemList

    Private sTitle As String
    Private sURL As String
    Private sItemType As String
    Private sPureUser As String
    Private sFilename As String
    Private dCreatedDate As DateTime
    Private dLastModifiedDate As DateTime
    Private iDocumentTemplateGroup As String
    Private iDocumentTemplateSubGroup As String
    Private bInternalOnly As Boolean

    Public Sub New()
    End Sub

    Public Property Title() As String
        Get
            Return Me.sTitle
        End Get
        Set(ByVal value As String)
            Me.sTitle = value
        End Set
    End Property

    Public Property URL() As String
        Get
            Return Me.sURL
        End Get
        Set(ByVal value As String)
            Me.sURL = value
        End Set
    End Property

    Public Property ItemType() As String
        Get
            Return Me.sItemType
        End Get
        Set(ByVal value As String)
            Me.sItemType = value
        End Set
    End Property

    Public Property PureUser() As String
        Get
            Return Me.sPureUser
        End Get
        Set(ByVal value As String)
            Me.sPureUser = value
        End Set
    End Property

    Public Property Filename() As String
        Get
            Return Me.sFilename
        End Get
        Set(ByVal value As String)
            Me.sFilename = value
        End Set
    End Property


    Public Property CreatedDate() As DateTime
        Get
            Return Me.dCreatedDate
        End Get
        Set(ByVal value As DateTime)
            Me.dCreatedDate = value
        End Set
    End Property

    Public Property LastModifiedDate() As DateTime
        Get
            Return Me.dLastModifiedDate
        End Get
        Set(ByVal value As DateTime)
            Me.dLastModifiedDate = value
        End Set
    End Property

    Public Property DocumentTemplateGroup() As String
        Get
            Return Me.iDocumentTemplateGroup
        End Get
        Set(ByVal value As String)
            Me.iDocumentTemplateGroup = value
        End Set
    End Property

    Public Property DocumentTemplateSubGroup() As String
        Get
            Return Me.iDocumentTemplateSubGroup
        End Get
        Set(ByVal value As String)
            Me.iDocumentTemplateSubGroup = value
        End Set
    End Property


    Public Property InternalOnly() As Boolean
        Get
            Return Me.bInternalOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bInternalOnly = value
        End Set
    End Property

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        'sbPrint.AppendLine("Title : " & sTitle.ToString() & "<br />")
        'sbPrint.AppendLine("URL : " & sURL.ToString() & "<br />")
        'sbPrint.AppendLine("ItemType : " & sItemType.ToString() & "<br />")
        'sbPrint.AppendLine("PureUser : " & sPureUser.ToString() & "<br />")
        'sbPrint.AppendLine("Filename : " & sFilename.ToString() & "<br />")
        'sbPrint.AppendLine("CreatedDate : " & dCreatedDate.ToString() & "<br />")
        'sbPrint.AppendLine("LastModifiedDate : " & dLastModifiedDate.ToString() & "<br />")
        'sbPrint.AppendLine("DocumentTemplateGroupID : " & iDocumentTemplateGroupID.ToString() & "<br />")
        'sbPrint.AppendLine("DocumentTemplateSubGroupID : " & iDocumentTemplateSubGroupID.ToString() & "<br />")
        'sbPrint.AppendLine("InternalOnly : " & bInternalOnly.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

End Class

<Serializable()> Public Class SharepointFileListResponseTypeItemListCollection : Inherits SortableCollectionBase

    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        For Each oDocument As SharepointFileListResponseTypeItemList In List
            sbPrint.AppendLine(oDocument.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()
    End Function

    Public Function Add(ByVal v_oSharePointFileDetails As SharepointFileListResponseTypeItemList) As Integer
        Return List.Add(v_oSharePointFileDetails)
    End Function

    Public Sub Remove(ByVal v_oSharePointFileDetails As SharepointFileListResponseTypeItemList)
        List.Remove(v_oSharePointFileDetails)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As SharepointFileListResponseTypeItemList
        Get
            Return List(i)
        End Get
        Set(ByVal value As SharepointFileListResponseTypeItemList)
            List(i) = value
        End Set
    End Property

    Public Function NumberOfRows() As Integer
        Return List.Count()
    End Function

End Class

