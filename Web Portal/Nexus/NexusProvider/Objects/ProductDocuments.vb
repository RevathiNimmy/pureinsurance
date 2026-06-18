
<Serializable()> Public Class ProductDocuments
    ''' <summary>
    ''' Document Link Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PMBDocLinkKey As Integer

    ''' <summary>
    ''' GIS Scheme Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GISSchemeKey As Integer

    ''' <summary>
    ''' Document Process Type Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProcessTypeKey As Integer

    ''' <summary>
    ''' Document Type Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DocumentTypeKey As Integer

    ''' <summary>
    ''' Document Template Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DocumentTemplateKey As Integer

    ''' <summary>
    ''' Document Template Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DocumentTemplateCode As String

    ''' <summary>
    ''' Spool Document
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SpoolDocument As Integer

    ''' <summary>
    ''' Process Types Docs Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProcessTypesDocsKey As Integer

    ''' <summary>
    ''' Functional Area
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FunctionalArea As Integer

    ''' <summary>
    ''' Product Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProductKey As Integer

    ''' <summary>
    ''' Source Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SourceKey As Integer

    ''' <summary>
    ''' Client Check : Logged in User
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsClient As Boolean

    ''' <summary>
    ''' Logged in User Check
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsAgent As Boolean

    ''' <summary>
    ''' Logged in User Check
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsOffice As Boolean

    ''' <summary>
    ''' Production Order
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProductionOrder As Integer

    ''' <summary>
    ''' PTDescription
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PTDescription As String

    ''' <summary>
    ''' Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Description As String

    ''' <summary>
    ''' PTD Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PTDDescription As String

    ''' <summary>
    ''' DT Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DTDescription As String

    ''' <summary>
    ''' Document Generation through BO
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GenerateThroughBO As Boolean

    ''' <summary>
    ''' Document Generation through SAM
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GenerateThroughSAM As Boolean
        

End Class

<Serializable()> Public Class ProductDocumentsCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        For Each oProductDocuments As ProductDocuments In List
            sbPrint.AppendLine("<br />")
        Next
        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oProductDocuments As ProductDocuments) As Integer
        Return List.Add(v_oProductDocuments)
    End Function
    Public Sub Remove(ByVal v_oProductDocuments As ProductDocuments)
        List.Remove(v_oProductDocuments)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As ProductDocuments
        Get
            Return List(i)
        End Get
        Set(ByVal value As ProductDocuments)
            List(i) = value
        End Set
    End Property

    Public Function NumberOfRows() As Integer
        Return List.Count()
    End Function

End Class
