<Serializable()> Public Class BaseClaimRecoveryReceiptType
    Private iRecoveryKey As Integer
    Private sTypeCode As String
    Private dTotalRecoveryAmount As Decimal
    Private dTotalReceiptAmount As Decimal
    Private dThisReceiptINCLTaxAmount As Decimal
    Private dThisReceiptTaxAmount As Decimal
    Private dThisReceiptNetAmount, dThisReceiptAmount As Decimal
    Private dBalanceAmount As Decimal
    Private sTaxCode As String
    Private iReceiptQueue As Integer
    Private iClaimReceiptItemKeyField As Integer
    Private sRecoveryTypeCodeField As String

    Public Property ThisReceiptAmount() As Decimal
        Get
            Return Me.dThisReceiptAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dThisReceiptAmount = value
        End Set
    End Property
    Public Property ReceiptQueue() As Integer
        Get
            Return Me.iReceiptQueue
        End Get
        Set(ByVal value As Integer)
            Me.iReceiptQueue = value
        End Set
    End Property
    Public Property RecoveryKey() As Integer
        Get
            Return Me.iRecoveryKey
        End Get
        Set(ByVal value As Integer)
            Me.iRecoveryKey = value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property

    Public Property TotalRecoveryAmount() As Decimal
        Get
            Return Me.dTotalRecoveryAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalRecoveryAmount = value
        End Set
    End Property
    Public Property ThisReceiptTaxAmount() As Decimal
        Get
            Return Me.dThisReceiptTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dThisReceiptTaxAmount = value
        End Set
    End Property


    Public Property TotalReceiptAmount() As Decimal
        Get
            Return Me.dTotalReceiptAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalReceiptAmount = value
        End Set
    End Property

    Public Property ThisReceiptINCLTaxAmount() As Decimal
        Get
            Return Me.dThisReceiptINCLTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dThisReceiptINCLTaxAmount = value
        End Set
    End Property

    Public Property ThisReceiptNetAmount() As Decimal
        Get
            Return Me.dThisReceiptNetAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dThisReceiptNetAmount = value
        End Set
    End Property

    Public Property BalanceAmount() As Decimal
        Get
            Return Me.dBalanceAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dBalanceAmount = value
        End Set
    End Property
    Public Property TaxCode() As String
        Get
            Return Me.sTaxCode
        End Get
        Set(ByVal value As String)
            Me.sTaxCode = value
        End Set
    End Property
    Public Property ClaimReceiptItemKey() As Integer
        Get
            Return Me.iClaimReceiptItemKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iClaimReceiptItemKeyField = value
        End Set
    End Property

    Public Property RecoveryTypeode() As String
        Get
            Return Me.sRecoveryTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sRecoveryTypeCodeField = value
        End Set
    End Property
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsTaxOverridden() As Boolean
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OverriddedTaxAmount() As Decimal

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("BaseRecoveryKey : " & iRecoveryKey.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("TypeCode : " & sTypeCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("TotalRecoveryAmount : " & dTotalRecoveryAmount.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("TotalReceiptAmount : " & dTotalReceiptAmount.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("ThisReceiptINCLTaxAmount : " & dThisReceiptINCLTaxAmount.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("ThisReceiptNetAmount : " & dThisReceiptNetAmount.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("BalanceAmount : " & dBalanceAmount.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        Return sbPrint.ToString()

    End Function
End Class

<Serializable()> Public Class RecoveryReceiptTypeCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oRecoveryReceiptType As RecoveryReceiptType In List
        '    sbPrint.AppendLine(oRecoveryReceiptType.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a RecoveryReceiptType object to the collection
    ''' </summary>
    ''' <param name="v_oRecoveryReceiptType">The RecoveryReceiptType object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oRecoveryReceiptType As BaseClaimRecoveryReceiptType) As Integer
        Return List.Add(v_oRecoveryReceiptType)
    End Function

    ''' <summary>
    ''' Remove an RecoveryReceiptType object from the collection
    ''' </summary>
    ''' <param name="v_oRecoveryReceiptType">The RecoveryReceiptType object to be removed</param>
    Public Sub Remove(ByVal v_oRecoveryReceiptType As BaseClaimRecoveryReceiptType)
        List.Remove(v_oRecoveryReceiptType)
    End Sub

    ''' <summary>
    ''' Remove an RecoveryReceiptType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the RecoveryReceiptType object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an RecoveryReceiptType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the RecoveryReceiptType object</param>
    ''' <value>The replacement RecoveryReceiptType object</value>
    ''' <returns>The RecoveryReceiptType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As BaseClaimRecoveryReceiptType
        Get
            Return List(i)
        End Get
        Set(ByVal value As BaseClaimRecoveryReceiptType)
            List(i) = value
        End Set
    End Property

End Class
