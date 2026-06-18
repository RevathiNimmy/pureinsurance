''' <summary>
''' Holds default values for a given document template
''' Properties added to allow uploaded documents to be added to a collection, and location of any generated documents to be recorded
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class DocumentDefaults
    Private _documentTemplateID As Integer
    Private _documentTemplateCode As String
    Private _documentTemplateDescription As String

    Private _documentGroupID As Integer
    Private _documentGroupCode As String
    Private _documentGroupDescription As String

    Private _documentSubGroupID As Integer
    Private _documentSubGroupCode As String
    Private _documentSubGroupDescription As String

    Private _internalOnly As Boolean
    Private _isUpload As Boolean = False
    Private _fileLocation As String
    Private _documentName As String
    Private _fileType As String
    Private _selected As Boolean
    Private _key As Integer
    Private _isExternal As Boolean
    Private _isTimeStampAppended As Boolean
    Private _DocumentEmailSubjectCode As String
    Private _DocumentEmailAttachmentCode As String

    ''' <summary>
    ''' Prints debug information for the DocumentDefaults class
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Document Template ID : " & _documentTemplateID.ToString() & "<br />")
        sbPrint.AppendLine("Document Template Code : " & _documentTemplateCode.ToString() & "<br />")
        sbPrint.AppendLine("Document Template Description : " & _documentTemplateDescription.ToString() & "<br />")
        sbPrint.AppendLine("Document Group ID : " & _documentGroupID.ToString() & "<br />")
        sbPrint.AppendLine("Document Group Code : " & _documentGroupCode.ToString() & "<br />")
        sbPrint.AppendLine("Document Group Description : " & _documentGroupDescription.ToString() & "<br />")
        sbPrint.AppendLine("Document SubGroup ID : " & _documentSubGroupID.ToString() & "<br />")
        sbPrint.AppendLine("Document SubGroup Code : " & _documentSubGroupCode.ToString() & "<br />")
        sbPrint.AppendLine("Document SubGroup Description : " & _documentSubGroupDescription.ToString() & "<br />")
        sbPrint.AppendLine("Selected By Default : " & _selected.ToString() & "<br />")
        sbPrint.AppendLine("Internal Only : " & _internalOnly.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()

    End Function

    Public Property documentTemplateID() As Integer
        Get
            Return _documentTemplateID
        End Get
        Set(ByVal value As Integer)
            _documentTemplateID = value
        End Set
    End Property

    Public Property documentTemplateCode() As String
        Get
            Return _documentTemplateCode
        End Get
        Set(ByVal value As String)
            _documentTemplateCode = value
        End Set
    End Property

    Public Property documentTemplateDescription() As String
        Get
            Return _documentTemplateDescription
        End Get
        Set(ByVal value As String)
            _documentTemplateDescription = value
        End Set
    End Property

    Public Property documentGroupID() As Integer
        Get
            Return _documentGroupID
        End Get
        Set(ByVal value As Integer)
            _documentGroupID = value
        End Set
    End Property

    Public Property documentGroupCode() As String
        Get
            Return _documentGroupCode
        End Get
        Set(ByVal value As String)
            _documentGroupCode = value
        End Set
    End Property

    Public Property documentGroupDescription() As String
        Get
            Return _documentGroupDescription
        End Get
        Set(ByVal value As String)
            _documentGroupDescription = value
        End Set
    End Property

    Public Property documentSubGroupID() As Integer
        Get
            Return _documentSubGroupID
        End Get
        Set(ByVal value As Integer)
            _documentSubGroupID = value
        End Set
    End Property

    Public Property documentSubGroupCode() As String
        Get
            Return _documentSubGroupCode
        End Get
        Set(ByVal value As String)
            _documentSubGroupCode = value
        End Set
    End Property

    Public Property documentSubGroupDescription() As String
        Get
            Return _documentSubGroupDescription
        End Get
        Set(ByVal value As String)
            _documentSubGroupDescription = value
        End Set
    End Property

    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            _Selected = value
        End Set
    End Property

    Public Property IsUpload() As Boolean
        Get
            Return _IsUpload
        End Get
        Set(ByVal value As Boolean)
            _IsUpload = value
        End Set
    End Property

    Public Property FileLocation() As String
        Get
            Return _FileLocation
        End Get
        Set(ByVal value As String)
            _FileLocation = value
        End Set
    End Property

    Public Property DocumentName() As String
        Get
            Return _DocumentName
        End Get
        Set(ByVal value As String)
            _DocumentName = value
        End Set
    End Property

    Public Property FileType() As String
        Get
            Return _FileType
        End Get
        Set(ByVal value As String)
            _FileType = value
        End Set
    End Property

    Public Property InternalOnly() As Boolean
        Get
            Return _InternalOnly
        End Get
        Set(ByVal value As Boolean)
            _InternalOnly = value
        End Set
    End Property

    Public Property Key() As Integer
        Get
            Return _Key
        End Get
        Set(ByVal value As Integer)
            _Key = value
        End Set
    End Property

    Public Property IsExternal() As Boolean
        Get
            Return _isExternal
        End Get
        Set(ByVal value As Boolean)
            _isExternal = value
        End Set
    End Property

    ''' <summary>
    ''' Hold The Flag wheather the time stamp format applied in Portal webConfig or not 
    '''     ''' </summary>
    ''' <returns></returns>
    Public Property IsTimeStampAppended() As Boolean
        Get
            Return _isTimeStampAppended
        End Get
        Set(ByVal value As Boolean)
            _isTimeStampAppended = value
        End Set
    End Property

    Public Property EmailDocumentSubjectCode() As String
        Get
            Return Me._DocumentEmailSubjectCode
        End Get
        Set(ByVal value As String)
            Me._DocumentEmailSubjectCode = value
        End Set
    End Property

    Public Property EmailDocumentAttachmentCode() As String
        Get
            Return Me._DocumentEmailAttachmentCode
        End Get
        Set(ByVal value As String)
            Me._DocumentEmailAttachmentCode = value
        End Set
    End Property

End Class

''' <summary>
''' Collection of Document default
''' </summary>
<Serializable()> Public Class DocumentDefaultsCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oDocumentDefaults As DocumentDefaults In List
            sbPrint.AppendLine(oDocumentDefaults.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a DocumentDefault object to the collection
    ''' </summary>
    ''' <param name="v_oDocument">The DocumentDefault object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oDocumentDefaults As DocumentDefaults) As Integer
        Return List.Add(v_oDocumentDefaults)
    End Function

    ''' <summary>
    ''' Remove an DocumentDefault object from the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oDocumentDefaults As DocumentDefaults)
        List.Remove(v_oDocumentDefaults)
    End Sub

    ''' <summary>
    ''' Remove an DocumentDefault object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Document object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace a DocumentDefault object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement Document object</value>
    ''' <returns>The Document object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As DocumentDefaults
        Get
            Return List(i)
        End Get
        Set(ByVal value As DocumentDefaults)
            List(i) = value
        End Set
    End Property


    ''' <summary>
    ''' Retrieve or replace a DocumentDefault object with a specified document template code
    ''' </summary>
    ''' <param name="i">The template code of the DocumentDefault object</param>
    ''' <value>The replacement DocumentDefault object</value>
    ''' <returns>The DocumentDefault object with the specified template code</returns>
    Default Public Property Item(ByVal sTempalteCode As String) As DocumentDefaults
        Get
            For Each oDocumentDefaults As DocumentDefaults In List
                If oDocumentDefaults.documentTemplateCode = sTempalteCode Then
                    Return oDocumentDefaults
                    Exit For
                End If
            Next
        End Get
        Set(ByVal value As DocumentDefaults)
            For Each oDocumentDefaults As DocumentDefaults In List
                If oDocumentDefaults.documentTemplateCode = sTempalteCode Then
                    oDocumentDefaults = value
                    Exit For
                End If
            Next
        End Set
    End Property

End Class