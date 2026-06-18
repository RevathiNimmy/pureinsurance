<Serializable()> Public Class BaseInsuranceFileKey

    Private nBaseInsuranceFileKey As Integer

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

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("BaseInsuranceFileKey : " & nBaseInsuranceFileKey & "<br />")

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' InsuranceFolderKey
    ''' </summary>
    ''' <value>Set the InsuranceFolderKey</value>
    ''' <returns>Get the InsuranceFolderKey</returns>
    Public Property BaseInsuranceFileKey() As Integer
        Get
            Return nBaseInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            nBaseInsuranceFileKey = value
        End Set
    End Property

End Class


<Serializable()> Public Class BaseInsuranceFileKeyCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oBaseInsuranceFileKey As RecoveryType In List
        '    sbPrint.AppendLine(oBaseInsuranceFileKey.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a BaseInsuranceFileKey object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oBaseInsuranceFileKey As BaseInsuranceFileKey) As Integer
        Return List.Add(v_oBaseInsuranceFileKey)
    End Function

    ''' <summary>
    ''' Remove an BaseInsuranceFileKey object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oBaseInsuranceFileKey As BaseInsuranceFileKey)
        List.Remove(v_oBaseInsuranceFileKey)
    End Sub

    ''' <summary>
    ''' Remove an BaseInsuranceFileKey object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the BaseInsuranceFileKey object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an BaseInsuranceFileKey object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the BaseInsuranceFileKey object</param>
    ''' <value>The replacement BaseInsuranceFileKey object</value>
    ''' <returns>The BaseInsuranceFileKey object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As BaseInsuranceFileKey
        Get
            Return List(i)
        End Get
        Set(ByVal value As BaseInsuranceFileKey)
            List(i) = value
        End Set
    End Property

End Class

