<Serializable()> Public Class Warnings
#Region "PrivateFields"
    Private iCode As Integer
    Private sDescription As String
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Code : " & iCode & "<br />")
        sbPrint.AppendLine("Description: " & sDescription & "<br />")
        Return sbPrint.ToString

    End Function
#Region "properties"
    Public Property Code() As Integer
        Get
            Return Me.iCode
        End Get
        Set(ByVal value As Integer)
            Me.iCode = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
#End Region

End Class
<Serializable()> Public Class WarningCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oWarning As Warnings In List
            sbPrint.AppendLine(oWarning.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Warnings object to the collection
    ''' </summary>
    ''' <param name="v_oWarning">The Warnings object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oWarning As Warnings) As Integer
        Return List.Add(v_oWarning)
    End Function

    ''' <summary>
    ''' Remove an Warnings object from the collection
    ''' </summary>
    ''' <param name="v_oWarning">The Warnings object to be removed</param>
    Public Sub Remove(ByVal v_oWarning As Warnings)
        List.Remove(v_oWarning)
    End Sub



    ''' <summary>
    ''' Retrieve or replace an Warnings object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Warnings object</param>
    ''' <value>The replacement Warnings object</value>
    ''' <returns>The Warnings object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Warnings
        Get
            Return List(i)
        End Get
        Set(ByVal value As Warnings)
            List(i) = value
        End Set
    End Property

End Class
