''' <summary>
''' Object named 'Parameters' for Nexus
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Parameters

    Private sParamNameField, sParamValueField As String

    ''' <summary>
    ''' Default constructor for class 'Parameters'
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Use for debug the object at Nexus application
    ''' </summary>
    ''' <returns>Returns a HTML string with all the contents of the object</returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Param Name Field : " & sParamNameField.ToString & "<br />")
        sbPrint.AppendLine("Param Value Field  : " & sParamValueField.ToString & "<br />")

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' ParamName will represent the name of request parameter to be passed
    ''' </summary>
    ''' <value>Name of the parameter to passed in collection</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ParamNameField() As String
        Get
            Return sParamNameField
        End Get
        Set(ByVal value As String)
            sParamNameField = value
        End Set
    End Property

    ''' <summary>
    ''' ParamValue will represent the value of request parameter to be passed
    ''' </summary>
    ''' <value>Value of the parameter to passed in collection</value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ParamValueField() As String
        Get
            Return sParamValueField
        End Get
        Set(ByVal value As String)
            sParamValueField = value
        End Set
    End Property

End Class

<Serializable()> Public Class ParametersCollection : Inherits CollectionBase

    ''' <summary>
    ''' Use for debug the object at Nexus application
    ''' </summary>
    ''' <returns>Returns a HTML string with all the contents of the object</returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oParameter As Parameters In List
            sbPrint.AppendLine(oParameter.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Use to add Parameter in collection
    ''' </summary>
    ''' <param name="v_oParameter"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_oParameter As Parameters) As Integer
        Return List.Add(v_oParameter)
    End Function

    ''' <summary>
    ''' Use to remove Parameter in collection
    ''' </summary>
    ''' <param name="v_oParameter"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal v_oParameter As Parameters)
        List.Remove(v_oParameter)
    End Sub

    ''' <summary>
    ''' Use to remove Parameter in collection using index
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Can set\get the parameter at desired index
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As Parameters
        Get
            Return List(i)
        End Get
        Set(ByVal value As Parameters)
            List(i) = value
        End Set
    End Property

End Class