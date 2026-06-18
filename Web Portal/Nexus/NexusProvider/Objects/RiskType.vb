<Serializable()> Public Class RiskType
    Private sName As String
    Private sRiskCode As String
    Private sDataModelCode As String
    Private sDataSetDefinitionFile As String
    Private sPath As String

    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Name : " & sName & "<br />")
        sbPrint.AppendLine("Risk Code : " & sRiskCode & "<br />")
        sbPrint.AppendLine("Data Model Code : " & sDataModelCode & "<br />")
        sbPrint.AppendLine("DataSet Definition File : " & sDataSetDefinitionFile & "<br />")
        sbPrint.AppendLine("Path : " & sPath & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString

    End Function
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    Public Property RiskCode() As String
        Get
            Return Me.sRiskCode
        End Get
        Set(ByVal value As String)
            Me.sRiskCode = value
        End Set
    End Property
    Public Property DataModelCode() As String
        Get
            Return Me.sDataModelCode
        End Get
        Set(ByVal value As String)
            Me.sDataModelCode = value
        End Set
    End Property
    Public Property DataSetDefinitionFile() As String
        Get
            Return Me.sDataSetDefinitionFile
        End Get
        Set(ByVal value As String)
            Me.sDataSetDefinitionFile = value
        End Set
    End Property
    Public Property Path() As String
        Get
            Return Me.sPath
        End Get
        Set(ByVal value As String)
            Me.sPath = value
        End Set
    End Property
End Class

''' <summary>
''' Collection of RiskType objects
''' </summary>
<Serializable()> Public Class RiskTypeCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oRiskType As RiskType In List
            sbPrint.AppendLine(oRiskType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a RiskType object to the collection
    ''' </summary>
    ''' <param name="v_oRiskType">The RiskType object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oRiskType As RiskType) As Integer
        Return List.Add(v_oRiskType)
    End Function

    ''' <summary>
    ''' Remove an RiskType object from the collection
    ''' </summary>
    ''' <param name="v_oRiskType">The Bank object to be removed</param>
    Public Sub Remove(ByVal v_oRiskType As RiskType)
        List.Remove(v_oRiskType)
    End Sub

    Public Sub Update(ByVal v_oRiskType As RiskType)
        List.Item(v_oRiskType.RiskCode) = v_oRiskType
    End Sub

    Public Sub Update(ByVal v_oRiskType As RiskType, ByVal index As Integer)
        List.Item(index) = v_oRiskType
    End Sub


    Default Public Property Item(ByVal i As Integer) As RiskType
        Get
            Return List(i)
        End Get
        Set(ByVal value As RiskType)
            List(i) = value
        End Set
    End Property

End Class

