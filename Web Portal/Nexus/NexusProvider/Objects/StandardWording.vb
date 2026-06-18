<Serializable()> Public Class StandardWording
    Private sStandardPolicyDiscription, sStandardPolicyWordingCode As String
    Private iStandardPolicyWordingID As Integer
    Sub New()

    End Sub
    Public Property StandardPolicyWordingCode() As String
        Get
            Return sStandardPolicyWordingCode
        End Get
        Set(ByVal value As String)
            sStandardPolicyWordingCode = value
        End Set
    End Property
    Public Property StandardPolicyDiscription() As String
        Get
            Return sStandardPolicyDiscription
        End Get
        Set(ByVal value As String)
            sStandardPolicyDiscription = value
        End Set
    End Property
    Public Property StandardPolicyWordingID() As Integer
        Get
            Return iStandardPolicyWordingID
        End Get
        Set(ByVal value As Integer)
            iStandardPolicyWordingID = value
        End Set
    End Property
End Class
<Serializable()> Public Class StandardWordings : Inherits Collections.CollectionBase
   
    Public Function Add(ByVal v_oStandardWording As StandardWording) As Integer

        Return List.Add(v_oStandardWording)

    End Function
    Public Sub Remove(ByVal v_oStandardWording As StandardWording)
        List.Remove(v_oStandardWording)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As StandardWording

        Get
            Return List(i)
        End Get
        Set(ByVal value As StandardWording)
            List(i) = value
        End Set

    End Property

End Class

