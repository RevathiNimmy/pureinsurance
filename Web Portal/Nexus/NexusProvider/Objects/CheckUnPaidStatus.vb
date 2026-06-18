<Serializable()> Public Class CheckUnPaidDetails

    Private sPolicyNumber As String
    Private dtClaimDate As Date
    Private sClaimNumber As String
    Private sClientname As String
    Private nInstalmentOverdue As Integer
    Private oPolicyTransactions As  TransactionCollection

      
    Public Property PolicyTransactions() As TransactionCollection
        Get
            Return oPolicyTransactions
        End Get
        Set(ByVal value As TransactionCollection)
            Me.oPolicyTransactions = value
        End Set
    End Property
       Public Property PayeeName() As String
        Get
            Return Me.sClientname
        End Get
        Set(ByVal value As String)
            Me.sClientname = value
        End Set
    End Property
    '''<remarks/>
     Public Property ClaimDate() As Date
        Get
            Return Me.dtClaimDate
        End Get
        Set(ByVal value As date)
            Me.dtClaimDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
        End Set
    End Property

    
    '''<remarks/>
    Public Property InstalmentOverdue() As Integer
        Get
            Return Me.nInstalmentOverdue
        End Get
        Set(ByVal value As Integer)
            Me.nInstalmentOverdue = value
        End Set
    End Property
        Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("InstalmentOverdue: " & nInstalmentOverdue & "<br />")
   
        Return sbPrint.ToString()

    End Function

End Class
