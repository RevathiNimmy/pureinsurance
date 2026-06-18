<Serializable()> Public Class CashList
#Region "PrivateFields"
    Private iCashListKey As Integer
    Private iVersion As Integer
    Private bTimeStamp As Byte()
    Private oWarning As WarningCollection
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
        sbPrint.AppendLine("Cash List Key : " & iCashListKey & "<br />")
        sbPrint.AppendLine("Version: " & iVersion & "<br />")
        'sbPrint.AppendLine("Time Stamp: " & bTimeStamp & "<br />")
        sbPrint.AppendLine("Warnings ---------------><br />")

        If oWarning IsNot Nothing Then
            sbPrint.AppendLine(oWarning.Print())
        End If
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    Public Property CashListKey() As Integer
        Get
            Return Me.iCashListKey
        End Get
        Set(ByVal value As Integer)
            Me.iCashListKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property Version() As Integer
        Get
            Return Me.iVersion
        End Get
        Set(ByVal value As Integer)
            Me.iVersion = value
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

    Public Property Warning() As WarningCollection
        Get
            Return Me.oWarning
        End Get
        Set(ByVal value As WarningCollection)
            Me.oWarning = value
        End Set
    End Property
#End Region
End Class
