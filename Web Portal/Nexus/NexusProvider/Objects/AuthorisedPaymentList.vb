Imports System.Net.NetworkInformation


<Serializable()> Public Class AuthorisedPaymentList
    Private sBranch As String
    Private sCurrency As String
    Private sBankAccount As String
    Private dTransactionDate As DateTime
    Private sPaymentType As String
    Private sMediaType As String
    Private sMediaRef As String
    Private sPolicyRef As String
    Private sClaimRer As String
    Private sPayeeAccountName As String
    Private iAmount As Decimal
    Private sCreatedBy As String
    Private sAssignedto As String
    Private dDateAssigned As String
    Private sBaseCurrencyAmount As String
    Private sStatus As String
    Private iCashListId As Integer
    Private iCashListItemId As Integer


    Public Property CashListId() As Integer
        Get
            Return Me.iCashListId
        End Get
        Set(value As Integer)
            iCashListId = value
        End Set
    End Property

    Public Property CashListItemId() As Integer
        Get
            Return Me.iCashListItemId
        End Get
        Set(value As Integer)
            iCashListItemId = value
        End Set
    End Property

    Public Property Branch() As String
        Get
            Return Me.sBranch
        End Get
        Set(ByVal value As String)
            Me.sBranch = value
        End Set
    End Property
    Public Property Currency() As String
        Get
            Return sCurrency
        End Get
        Set(value As String)
            sCurrency = value
        End Set
    End Property
    Public Property BankAccount() As String
        Get
            Return sBankAccount
        End Get
        Set(value As String)
            sBankAccount = value
        End Set
    End Property
    Public Property TransactionDate() As DateTime
        Get
            Return dTransactionDate
        End Get
        Set(value As DateTime)
            dTransactionDate = value
        End Set
    End Property
    Public Property PaymentType() As String
        Get
            Return sPaymentType
        End Get
        Set(value As String)
            sPaymentType = value
        End Set
    End Property
    Public Property MediaType() As String
        Get
            Return sMediaType
        End Get
        Set(value As String)
            sMediaType = value
        End Set
    End Property
    Public Property MediaRef() As String
        Get
            Return sMediaRef
        End Get
        Set(value As String)
            sMediaRef = value
        End Set
    End Property
    Public Property PolicyRef() As String
        Get
            Return sPolicyRef
        End Get
        Set(value As String)
            sPolicyRef = value
        End Set
    End Property
    Public Property ClaimRef() As String
        Get
            Return sClaimRer
        End Get
        Set(value As String)
            sClaimRer = value
        End Set
    End Property
    Public Property PayeeAccountName() As String
        Get
            Return sPayeeAccountName
        End Get
        Set(value As String)
            sPayeeAccountName = value
        End Set
    End Property
    Public Property Amount() As Decimal
        Get
            Return iAmount
        End Get
        Set(value As Decimal)
            iAmount = value
        End Set
    End Property
    Public Property CreatedBy() As String
        Get
            Return sCreatedBy
        End Get
        Set(value As String)
            sCreatedBy = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return sStatus
        End Get
        Set(value As String)
            sStatus = value
        End Set
    End Property
    Public Property Assignedto() As String
        Get
            Return sAssignedto
        End Get
        Set(value As String)
            sAssignedto = value
        End Set
    End Property
    Public Property DateAssigned() As String
        Get
            Return dDateAssigned
        End Get
        Set(value As String)
            dDateAssigned = value
        End Set
    End Property
    Public Property BaseCurrencyAmount() As String
        Get
            Return sBaseCurrencyAmount
        End Get
        Set(value As String)
            sBaseCurrencyAmount = value
        End Set
    End Property

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder
        sbPrint.AppendLine("Branch : " & sBranch.ToString & "<br />")
        sbPrint.AppendLine("Currency: " & sCurrency.ToString() & "<br />")
        sbPrint.AppendLine("BankAccount : " & sBankAccount & "<br />")
        sbPrint.AppendLine("TransactionDate : " & dTransactionDate & "<br />")
        sbPrint.AppendLine("PaymentType : " & sPaymentType.ToString() & "<br />")
        sbPrint.AppendLine("MediaType : " & sMediaType.ToString() & "<br />")
        sbPrint.AppendLine("MediaRef : " & sMediaRef & "<br />")
        sbPrint.AppendLine("PolicyRef : " & sPolicyRef & "<br />")
        sbPrint.AppendLine("ClaimRer : " & sClaimRer & "<br />")
        sbPrint.AppendLine("PayeeAccountName : " & sPayeeAccountName & "<br />")
        sbPrint.AppendLine("Amount : " & iAmount & "<br />")
        sbPrint.AppendLine("CreatedBy : " & sCreatedBy & "<br />")
        sbPrint.AppendLine("Assignedto : " & sAssignedto & "<br />")
        sbPrint.AppendLine("DateAssigned : " & dDateAssigned & "<br />")
        sbPrint.AppendLine("BaseCurrencyAmount : " & sBaseCurrencyAmount & "<br />")
        sbPrint.AppendLine("Status : " & sStatus & "<br />")
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class
<Serializable()> Public Class AuthorisedPaymentCollection : Inherits SortableCollectionBase

        Public Sub New()
            MyBase.SortObjectType = GetType(AuthorisedPaymentList)
        End Sub

        Public Function Print() As String

            Dim sbPrint As New Text.StringBuilder()

            For Each oCashListItems As AuthorisedPaymentList In List
                sbPrint.AppendLine(oCashListItems.Print())
                sbPrint.AppendLine("<br />")
            Next

            Return sbPrint.ToString()

        End Function

        Public Function Add(ByVal v_oCashListItems As AuthorisedPaymentList) As Integer
            Return List.Add(v_oCashListItems)
        End Function

        Public Sub Remove(ByVal v_oCashListItems As AuthorisedPaymentList)
            List.Remove(v_oCashListItems)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            List.RemoveAt(index)
        End Sub

        Default Public Property Item(ByVal i As Integer) As AuthorisedPaymentList
            Get
                Return List(i)
            End Get
            Set(ByVal value As AuthorisedPaymentList)
                List(i) = value
            End Set
        End Property
    End Class


    Public Class AutorisedPaymentRequestType

    Private _cashListItemKey As String
    Private _payeeName As String
    Private _date As Date
    Private _createdBy As String
    Private _assignedTo As String
    Private _Branch As String
    Private _dateTo As Date
    Private _paymentType As String
    Private _showAllOtherPayments As String
    Public Property DateFrom() As Date
        Get
            Return _date
        End Get
        Set(value As Date)
            _date = value
        End Set
    End Property
    Public Property DateTo() As Date
        Get
            Return _dateTo
        End Get
        Set(value As Date)
            _dateTo = value
        End Set
    End Property
    Public Property CreatedBy() As String
        Get
            Return _createdBy
        End Get
        Set(value As String)
            _createdBy = value
        End Set
    End Property
    Public Property AssignedTo() As String
        Get
            Return _assignedTo
        End Get
        Set(value As String)
            _assignedTo = value
        End Set
    End Property
    Public Property Branch() As String
        Get
            Return _Branch
        End Get
        Set(value As String)
            _Branch = value
        End Set
    End Property
    Public Property PaymentType() As String
        Get
            Return _paymentType
        End Get
        Set(value As String)
            _paymentType = value
        End Set
    End Property
    Public Property ShowAllOtherPayments() As String
        Get
            Return _showAllOtherPayments
        End Get
        Set(value As String)
            _showAllOtherPayments = value
        End Set
    End Property
    Public Property PayeeName() As String
        Get
            Return _payeeName
        End Get
        Set(value As String)
            _payeeName = value
        End Set
    End Property
    Public Property CashListItemKey() As String
        Get
            Return _cashListItemKey
        End Get
        Set(value As String)
            _cashListItemKey = value
        End Set
    End Property
End Class
