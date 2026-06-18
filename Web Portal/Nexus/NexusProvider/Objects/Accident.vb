<Serializable()> Public Class Accident
    Private iAccidentKey As Integer
    Private dtDate As DateTime
    Private sDescription As String
    Private bIsAtFault As Boolean
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bIsAtFault = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("AccidentKey : " & iAccidentKey & "<br />")
        sbPrint.AppendLine("Date : " & dtDate & "<br />")
        sbPrint.AppendLine("sDescription : " & sDescription & "<br />")
        sbPrint.AppendLine("IsAtFault: " & IIf(bIsAtFault, "true", "false") & "<br />")
        Return sbPrint.ToString

    End Function
    Public Property AccidentKey() As Integer
        Get
            Return iAccidentKey
        End Get
        Set(ByVal value As Integer)
            iAccidentKey = value
        End Set
    End Property
    Public Property AccidentDate() As DateTime
        Get
            Return dtDate
        End Get
        Set(ByVal value As DateTime)
            dtDate = value
        End Set
    End Property
    Public Property newDate() As DateTime
        Get
            Return dtDate
        End Get
        Set(ByVal value As DateTime)
            dtDate = value
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
    Public Property IsAtFault() As Boolean
        Get
            Return bIsAtFault
        End Get
        Set(ByVal value As Boolean)
            bIsAtFault = value
        End Set
    End Property
End Class

<Serializable()> Public Class AccidentCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAccident As Accident In List
            sbPrint.AppendLine(oAccident.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Accident object to the collection
    ''' </summary>
    ''' <param name="v_oAccident">The Accident object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAccident As Accident) As Integer
        v_oAccident.AccidentKey = List.Add(v_oAccident)
        Return v_oAccident.AccidentKey
    End Function

    ''' <summary>
    ''' Remove an Accident object from the collection
    ''' </summary>
    ''' <param name="v_oAccident">The Accident object to be removed</param>
    Public Sub Remove(ByVal v_oAccident As Accident)
        List.Remove(v_oAccident)
    End Sub

    ''' <summary>
    ''' Remove an Accident object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Accident object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Accident object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Accident object</param>
    ''' <value>The replacement Accident object</value>
    ''' <returns>The Accident object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Accident
        Get
            Return List(i)
        End Get
        Set(ByVal value As Accident)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oAccident As Accident)
        List.Item(v_oAccident.AccidentKey) = v_oAccident
    End Sub

    Public Sub Update(ByVal v_oAccident As Accident, ByVal index As Integer)
        List.Item(index) = v_oAccident
    End Sub

End Class
