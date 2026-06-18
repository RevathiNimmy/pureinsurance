<Serializable()> Public Class ClaimRisk
    Private iClaimKey As Integer
    Private sXMLDataSet As String
    Private bTimeStamp As Byte()
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

        sbPrint.AppendLine("iClaimKey : " & iClaimKey & "<br />")
        sbPrint.AppendLine("sXMLDataSet : " & sXMLDataSet & "<br />")

        Return sbPrint.ToString

    End Function
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    Public Property XMLDataSet() As String
        Get
            Return Me.sXMLDataSet
        End Get
        Set(ByVal value As String)
            Me.sXMLDataSet = value
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

End Class
