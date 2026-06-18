''' <summary>
''' Nexus Claim Document object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ClaimDocument

#Region "Private Variables"

    Private iMode As Integer
    Private iClaimKey As Integer
    Private sTransactionType As String
    Private sParameterXML As String
  

    Private sDocumentName As String
    Private sDocumentDescription As String

    Private oClaimDocument As ClaimDocumentCollection
#End Region

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

       
        Return sbPrint.ToString()

    End Function
    '''<remarks/>
    Public Property Mode() As Integer
        Get
            Return Me.iMode
        End Get
        Set(ByVal value As Integer)
            Me.iMode = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property TransactionType() As String
        Get
            Return Me.sTransactionType
        End Get
        Set(ByVal value As String)
            Me.sTransactionType = value
        End Set
    End Property

    '''<remarks/>
    Public Property ParameterXML() As String
        Get
            Return Me.sParameterXML
        End Get
        Set(ByVal value As String)
            Me.sParameterXML = value
        End Set
    End Property

    Public Property ClaimDocument() As ClaimDocumentCollection
        Get
            Return Me.oClaimDocument
        End Get
        Set(ByVal value As ClaimDocumentCollection)
            Me.oClaimDocument = value
        End Set
    End Property
    '''<remarks/>
    Public Property DocumentName() As String
        Get
            Return Me.sDocumentName
        End Get
        Set(ByVal value As String)
            Me.sDocumentName = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentDescription() As String
        Get
            Return Me.sDocumentDescription
        End Get
        Set(ByVal value As String)
            Me.sDocumentDescription = value
        End Set
    End Property
End Class
''' <summary>
''' Collection of Claim Document objects
''' </summary>
<Serializable()> Public Class ClaimDocumentCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oDocument As ClaimDocument In List
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
    Public Function Add(ByVal v_oDocument As ClaimDocument) As Integer
        Return List.Add(v_oDocument)
    End Function

    ''' <summary>
    ''' Remove an Document object from the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oDocument As ClaimDocument)
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
    Default Public Property Item(ByVal i As Integer) As ClaimDocument
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimDocument)
            List(i) = value
        End Set
    End Property

End Class
