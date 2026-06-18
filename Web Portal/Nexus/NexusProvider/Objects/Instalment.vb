<Serializable()> Public Class Instalment

#Region "Private Variables"

    Private nInstalmentNumber As Integer
    Private dAmount As Double
    Private dtDueDate As Date
    Private dtPaymentDate As Date
    Private sStatus As String
    Private sReason As String
    Private nPFInstalmentsKeyField As Integer
    Private dFeeField As Double
    Private sTransactionDescriptionField As String
    Private sStatusDescriptionField As String
    Private sBatchRefField As String
    Private dtExportDateField As Date
    Private dtPostedDateField As Date
    Private nPFTransactionKeyField As Integer
    Private dTaxField As Double
    Private dCommissionField As Double
    Private oHistoryField As InstalmentsHistoryCollection
    Private sStatusCode As String
    Private sInstalmentReasonCode As String

#End Region

#Region "Public Properties"

    '''<remarks/>
    Public Property InstalmentNumber() As Integer
        Get
            Return Me.nInstalmentNumber
        End Get
        Set(ByVal value As Integer)
            Me.nInstalmentNumber = value
        End Set
    End Property


    '''<remarks/>
    Public Property Amount() As Double
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Double)
            Me.dAmount = value
        End Set
    End Property


    '''<remarks/>
    Public Property DueDate() As Date
        Get
            Return Me.dtDueDate
        End Get
        Set(ByVal value As Date)
            Me.dtDueDate = value
        End Set
    End Property


    '''<remarks/>
    Public Property PaymentDate() As Date
        Get
            Return Me.dtPaymentDate
        End Get
        Set(ByVal value As Date)
            Me.dtPaymentDate = value
        End Set
    End Property



    '''<remarks/>
    Public Property Status() As String
        Get
            Return Me.sStatus
        End Get
        Set(ByVal value As String)
            Me.sStatus = value
        End Set
    End Property


    '''<remarks/>
    Public Property Reason() As String
        Get
            Return Me.sReason
        End Get
        Set(ByVal value As String)
            Me.sReason = value
        End Set
    End Property


    '''<remarks/>
    Public Property PFInstalmentsKey() As Integer
        Get
            Return Me.nPFInstalmentsKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nPFInstalmentsKeyField = value
        End Set
    End Property


    '''<remarks/>
    Public Property Fee() As Double
        Get
            Return Me.dFeeField
        End Get
        Set(ByVal value As Double)
            Me.dFeeField = value
        End Set
    End Property


    '''<remarks/>
    Public Property TransactionDescription() As String
        Get
            Return Me.sTransactionDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sTransactionDescriptionField = value
        End Set
    End Property


    '''<remarks/>
    Public Property StatusDescription() As String
        Get
            Return Me.sStatusDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sStatusDescriptionField = value
        End Set
    End Property


    '''<remarks/>
    Public Property BatchRef() As String
        Get
            Return Me.sBatchRefField
        End Get
        Set(ByVal value As String)
            Me.sBatchRefField = value
        End Set
    End Property


    '''<remarks/>
    Public Property ExportDate() As Date
        Get
            Return Me.dtExportDateField
        End Get
        Set(ByVal value As Date)
            Me.dtExportDateField = value
        End Set
    End Property


    '''<remarks/>
    Public Property PostedDate() As Date
        Get
            Return Me.dtPostedDateField
        End Get
        Set(ByVal value As Date)
            Me.dtPostedDateField = value
        End Set
    End Property


    '''<remarks/>
    Public Property PFTransactionKey() As Integer
        Get
            Return Me.nPFTransactionKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nPFTransactionKeyField = value
        End Set
    End Property


    '''<remarks/>
    Public Property Tax() As Double
        Get
            Return Me.dTaxField
        End Get
        Set(ByVal value As Double)
            Me.dTaxField = value
        End Set
    End Property


    '''<remarks/>
    Public Property Commission() As Double
        Get
            Return Me.dCommissionField
        End Get
        Set(ByVal value As Double)
            Me.dCommissionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property History() As InstalmentsHistoryCollection
        Get
            Return Me.oHistoryField
        End Get
        Set(ByVal value As InstalmentsHistoryCollection)
            Me.oHistoryField = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property InstalmentReasonCode() As String
        Get
            Return Me.sInstalmentReasonCode
        End Get
        Set(ByVal value As String)
            Me.sInstalmentReasonCode = value
        End Set
    End Property

    Public Property IsPartialPayment As Boolean

    Public Property IsWriteOffPayment As Boolean

    Public Property WriteOffReasonID As Integer

    Public Property OverPaymentWriteOffAmount As Decimal

    Public Property ActualAmount As Decimal

    Public Property CurrencyDesc() As String
#End Region

End Class


<Serializable()> Public Class InstalmentsCollection : Inherits CollectionBase

#Region "Public Properties"

    '''<remarks/>
    Default Public Property Item(ByVal i As Integer) As Instalment
        Get
            Return List(i)
        End Get
        Set(ByVal value As Instalment)
            List(i) = value
        End Set
    End Property

#End Region

#Region "Public Methods"


    ''' <summary>
    ''' Used to Print Instalments
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oInstalments As Instalment In List
            'sbPrint.AppendLine(oInstalments.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function


    ''' <summary>
    ''' Add New Instalment Type to collection
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_oInstalment As Instalment) As Integer
        Return List.Add(v_oInstalment)
    End Function


    ''' <summary>
    ''' Used to Remove Instalment Item from Instalments collection
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal v_oInstalment As Instalment)
        List.Remove(v_oInstalment)
    End Sub


    ''' <summary>
    ''' Used to Remove Instalment Item from Instalments collection on Index basis
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub


#End Region

End Class







