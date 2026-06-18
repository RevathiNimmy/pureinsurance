<Serializable()> Public Class DocumentTemplate
    Private sCode As String
    Private dtEffectiveDate As DateTime
    Private sTypeCode As String
    Private sProductCode As String
    Private sDescription As String
    Private iDocumentTemplateId As Integer
    Private iInsuranceFileKey As Integer
    Private bTimeStamp() As Byte
    Private sUpdateFilePath As String
    Private Is_Edited As Boolean
    Private sPropertyName, sObjectName As String
    Private Is_Default As Boolean
    Private oDocumentFormatTypeField As DocumentFormatType
    Private sExistingDocumentPath As String
    Private sFileURL As String
    Private sOriginalCode As String
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'sbPrint.AppendLine("IDocument Number : " ublic Property ObjectN& iDocNum.ToString() & "<br />")
        'sbPrint.AppendLine("<br />")
        'sbPrint.AppendLine("Document Description : " & sDocDescription & "<br />")
        'sbPrint.AppendLine("PDF Document (String format) : " & PdfDocument.Length & "<br />")
        Return sbPrint.ToString()

    End Function
    Public Property Edited() As Boolean
        Get
            Return Me.Is_Edited
        End Get
        Set(ByVal value As Boolean)
            Me.Is_Edited = value
        End Set
    End Property
    Public Property UpdateFilePath() As String
        Get
            Return sUpdateFilePath
        End Get
        Set(ByVal value As String)
            sUpdateFilePath = value
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

    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property
    Public Property DocumentTemplateId() As Integer
        Get
            Return iDocumentTemplateId
        End Get
        Set(ByVal value As Integer)
            iDocumentTemplateId = value
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
    Public Property OriginalCode() As String
        Get
            Return sOriginalCode
        End Get
        Set(ByVal value As String)
            sOriginalCode = value
        End Set
    End Property
    Public Property EffectiveDate() As DateTime
        Get
            Return dtEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            dtEffectiveDate = value
        End Set
    End Property
    Public Property ProductCode() As String
        Get
            Return sProductCode
        End Get
        Set(ByVal value As String)
            sProductCode = value
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
    Public Property TypeCode() As String
        Get
            Return sTypeCode
        End Get
        Set(ByVal value As String)
            sTypeCode = value
        End Set
    End Property
    Public Property PropertyName() As String
        Get
            Return sPropertyName
        End Get
        Set(ByVal value As String)
            sPropertyName = value
        End Set
    End Property
    Public Property ObjectName() As String
        Get
            Return sObjectName
        End Get
        Set(ByVal value As String)
            sObjectName = value
        End Set
    End Property
    Public Property IsDefault() As Boolean
        Get
            Return Me.Is_Default
        End Get
        Set(ByVal value As Boolean)
            Me.Is_Default = value
        End Set
    End Property
    Public Property ExistingDocumentPath() As String
        Get
            Return sExistingDocumentPath
        End Get
        Set(ByVal value As String)
            sExistingDocumentPath = value
        End Set
    End Property
    Public Property DocumentFormatTypeField() As DocumentFormatType
        Get
            Return oDocumentFormatTypeField
        End Get
        Set(ByVal value As DocumentFormatType)
            oDocumentFormatTypeField = value
        End Set
    End Property
    Public Property FileURL() As String
        Get
            Return sFileURL
        End Get
        Set(ByVal value As String)
            sFileURL = value
        End Set
    End Property
End Class

''' <summary>
''' Collection of Document objects
''' </summary>
<Serializable()> Public Class DocumentTemplateCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oDocumentTemplate As DocumentTemplate In List
            sbPrint.AppendLine(oDocumentTemplate.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Document object to the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oDocumentTemplate As DocumentTemplate) As Integer
        Return List.Add(v_oDocumentTemplate)
    End Function

    ''' <summary>
    ''' Remove an Document object from the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oDocumentTemplate As DocumentTemplate)
        List.Remove(v_oDocumentTemplate)
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
    Default Public Property Item(ByVal i As Integer) As DocumentTemplate
        Get
            Return List(i)
        End Get
        Set(ByVal value As DocumentTemplate)
            List(i) = value
        End Set
    End Property


End Class
