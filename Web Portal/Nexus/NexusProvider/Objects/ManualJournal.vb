<Serializable()> Public Class ManualJournal
    Private dJournalDate As Date
    Private sJournalComment As String
    Private bJournalDateSpecified As Boolean
    Private sJournalTypeCode As String
    Private sJournalBranchCode As String
    Private dJournalReverseDate As Date
    Private bJournalReverseDateSpecified As Boolean
    Private iRecurringOccurs As Integer
    Private iRecurringPerPeriodOnDay As Integer
    Private bRecurringPerPeriodOnDayBool As Boolean
    Private iRecurringPerMonthOnDay As Integer
    Private bRecurringPerMonthOnDayBool As Boolean
    Private iRecurringPerQuarterOnDay As Integer
    Private bRecurringPerQuarterOnDayBool As Boolean
    Private iJournalKey As Integer
    Private cManualJournalItemCollection As NexusProvider.ManualJournalItemCollection
    Private sManualJournalDocumentRef As String
    Private sJournalSubBranchCode As String
    Private iIs_Reffered As Integer
    Private bIs_RefferedSpecified As Boolean
    Private iManualJournalId As Integer
    Private bManualJournalIdSpecified As Boolean
    Private iApproved As Integer

    Public Property CurrentStep() As Integer
    Public Property IsLastStep() As Boolean
    Public Property PMUserGroup() As String
    Public Property ValidationMessage() As String
    Public Property JournalAmount() As Decimal

    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        Return sbPrint.ToString()

    End Function

    Sub New()
        cManualJournalItemCollection = New NexusProvider.ManualJournalItemCollection
    End Sub
    Public Property ManualJournalId() As Integer
        Get
            Return Me.iManualJournalId
        End Get
        Set(ByVal value As Integer)
            Me.iManualJournalId = value
        End Set
    End Property
    Public Property ManualJournalIdSpecified() As Boolean
        Get
            Return bManualJournalIdSpecified
        End Get
        Set(ByVal value As Boolean)
            bManualJournalIdSpecified = value
        End Set
    End Property
    Public Property Is_Reffered() As Integer
        Get
            Return Me.iIs_Reffered
        End Get
        Set(ByVal value As Integer)
            Me.iIs_Reffered = value
        End Set
    End Property
    Public Property Is_RefferedSpecified() As Boolean
        Get
            Return bIs_RefferedSpecified
        End Get
        Set(ByVal value As Boolean)
            bIs_RefferedSpecified = value
        End Set
    End Property
    Public Property JournalKey() As Integer
        Get
            Return iJournalKey
        End Get
        Set(ByVal value As Integer)
            iJournalKey = value
        End Set
    End Property
    Public Property JournalBranchCode() As String
        Get
            Return sJournalBranchCode
        End Get
        Set(ByVal value As String)
            sJournalBranchCode = value
        End Set
    End Property
    Public Property JournalDate() As Date
        Get
            Return dJournalDate
        End Get
        Set(ByVal value As Date)
            dJournalDate = value
        End Set
    End Property

    Public Property JournalComment() As String
        Get
            Return sJournalComment
        End Get
        Set(ByVal value As String)
            sJournalComment = value
        End Set
    End Property

    Public Property JournalReverseDateSpecified() As Boolean
        Get
            Return bJournalReverseDateSpecified
        End Get
        Set(ByVal value As Boolean)
            bJournalReverseDateSpecified = value
        End Set
    End Property
    Public Property JournalDateSpecified() As Boolean
        Get
            Return bJournalDateSpecified
        End Get
        Set(ByVal value As Boolean)
            bJournalDateSpecified = value
        End Set
    End Property
    Public Property JournalTypeCode() As String
        Get
            Return sJournalTypeCode
        End Get
        Set(ByVal value As String)
            sJournalTypeCode = value
        End Set
    End Property

    Public Property JournalReverseDate() As Date
        Get
            Return dJournalReverseDate
        End Get
        Set(ByVal value As Date)
            dJournalReverseDate = value
        End Set
    End Property

    Public Property RecurringOccurs() As Integer
        Get
            Return iRecurringOccurs
        End Get
        Set(ByVal value As Integer)
            iRecurringOccurs = value
        End Set
    End Property

    Public Property RecurringPerPeriodOnDay() As Integer
        Get
            Return iRecurringPerPeriodOnDay
        End Get
        Set(ByVal value As Integer)
            iRecurringPerPeriodOnDay = value
        End Set
    End Property

    Public Property RecurringPerPeriodOnDayBool() As Boolean
        Get
            Return bRecurringPerPeriodOnDayBool
        End Get
        Set(ByVal value As Boolean)
            bRecurringPerPeriodOnDayBool = value
        End Set
    End Property

    Public Property RecurringPerMonthOnDay() As Integer
        Get
            Return iRecurringPerMonthOnDay
        End Get
        Set(ByVal value As Integer)
            iRecurringPerMonthOnDay = value
        End Set
    End Property

    Public Property RecurringPerMonthOnDayBool() As Boolean
        Get
            Return bRecurringPerMonthOnDayBool
        End Get
        Set(ByVal value As Boolean)
            bRecurringPerMonthOnDayBool = value
        End Set
    End Property

    Public Property RecurringPerQuarterOnDay() As Integer
        Get
            Return iRecurringPerQuarterOnDay
        End Get
        Set(ByVal value As Integer)
            iRecurringPerQuarterOnDay = value
        End Set
    End Property

    Public Property RecurringPerQuarterOnDayBool() As Boolean
        Get
            Return bRecurringPerQuarterOnDayBool
        End Get
        Set(ByVal value As Boolean)
            bRecurringPerQuarterOnDayBool = value
        End Set
    End Property

    Public Property ManualJournalItemCollection() As NexusProvider.ManualJournalItemCollection
        Get
            Return cManualJournalItemCollection
        End Get
        Set(ByVal value As NexusProvider.ManualJournalItemCollection)
            cManualJournalItemCollection = value
        End Set
    End Property


    Public Property ManualJournalDocumentRef() As String
        Get
            Return sManualJournalDocumentRef
        End Get
        Set(ByVal value As String)
            sManualJournalDocumentRef = value
        End Set
    End Property

    Public Property JournalSubBranchCode() As String
        Get
            Return sJournalSubBranchCode
        End Get
        Set(ByVal value As String)
            sJournalSubBranchCode = value
        End Set
    End Property

    Public Property Approved() As Integer
        Get
            Return iApproved
        End Get
        Set(ByVal value As Integer)
            iApproved = value
        End Set
    End Property

End Class

''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ManualJournalCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(ManualJournal)
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oManualJournal As ManualJournal In List
            sbPrint.AppendLine(oManualJournal.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oManualJournal As ManualJournal) As Integer
        Return List.Add(v_oManualJournal)
    End Function

    Public Sub Remove(ByVal v_oManualJournal As ManualJournal)
        List.Remove(v_oManualJournal)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ManualJournal
        Get
            Return List(i)
        End Get
        Set(ByVal value As ManualJournal)
            List(i) = value
        End Set
    End Property



End Class
<Serializable()> Public Class ManualJournalItem
    Private dAmount As Decimal
    Private sAltReference As String
    Private sUnderwritingYearCode As String
    Private sUnderwritingYearDescription As String
    Private dCurrencyRate As Decimal
    Private dBaseAmount As Decimal
    Private sComment As String
    Private sCostCentreCode As String
    Private sCostCentreDescription As String
    Private sInsuranceRef As String
    Private sPurchaseOrderNumber As String
    Private sPurchaseInvoiceNumber As String
    Private sCurrencyTypeCode As String
    Private sCurrencyTypeDescription As String
    Private sAccountKey As String
    Private sAccountName As String
    Private iManualJournalKey As Integer
    Private iManualJournalDetailId As Integer



    Public Property ManualJournalDetailId() As Integer
        Get
            Return Me.iManualJournalDetailId
        End Get
        Set(ByVal value As Integer)
            Me.iManualJournalDetailId = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return dAmount
        End Get
        Set(ByVal value As Decimal)
            dAmount = value
        End Set
    End Property

    Public Property AltReference() As String
        Get
            Return sAltReference
        End Get
        Set(ByVal value As String)
            sAltReference = value
        End Set
    End Property

    Public Property UnderwritingYearCode() As String
        Get
            Return sUnderwritingYearCode
        End Get
        Set(ByVal value As String)
            sUnderwritingYearCode = value
        End Set
    End Property

    Public Property UnderwritingYearDescription() As String
        Get
            Return sUnderwritingYearDescription
        End Get
        Set(ByVal value As String)
            sUnderwritingYearDescription = value
        End Set
    End Property

    Public Property CurrencyRate() As Decimal
        Get
            Return dCurrencyRate
        End Get
        Set(ByVal value As Decimal)
            dCurrencyRate = value
        End Set
    End Property

    Public Property BaseAmount() As Decimal
        Get
            Return dBaseAmount
        End Get
        Set(ByVal value As Decimal)
            dBaseAmount = value
        End Set
    End Property

    Public Property Comment() As String
        Get
            Return sComment
        End Get
        Set(ByVal value As String)
            sComment = value
        End Set
    End Property

    Public Property CostCentreCode() As String
        Get
            Return sCostCentreCode
        End Get
        Set(ByVal value As String)
            sCostCentreCode = value
        End Set
    End Property

    Public Property CostCentreDescription() As String
        Get
            Return sCostCentreDescription
        End Get
        Set(ByVal value As String)
            sCostCentreDescription = value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return sInsuranceRef
        End Get
        Set(ByVal value As String)
            sInsuranceRef = value
        End Set
    End Property

    Public Property PurchaseOrderNumber() As String
        Get
            Return sPurchaseOrderNumber
        End Get
        Set(ByVal value As String)
            sPurchaseOrderNumber = value
        End Set
    End Property

    Public Property PurchaseInvoiceNumber() As String
        Get
            Return sPurchaseInvoiceNumber
        End Get
        Set(ByVal value As String)
            sPurchaseInvoiceNumber = value
        End Set
    End Property

    Public Property CurrencyTypeCode() As String
        Get
            Return sCurrencyTypeCode
        End Get
        Set(ByVal value As String)
            sCurrencyTypeCode = value
        End Set
    End Property

    Public Property CurrencyTypeDescription() As String
        Get
            Return sCurrencyTypeDescription
        End Get
        Set(ByVal value As String)
            sCurrencyTypeDescription = value
        End Set
    End Property

    Public Property AccountKey() As String
        Get
            Return sAccountKey
        End Get
        Set(ByVal value As String)
            sAccountKey = value
        End Set
    End Property

    Public Property AccountName() As String
        Get
            Return sAccountName
        End Get
        Set(ByVal value As String)
            sAccountName = value
        End Set
    End Property

    Public Property ManualJournalKey() As Integer
        Get
            Return iManualJournalKey
        End Get
        Set(ByVal value As Integer)
            iManualJournalKey = value
        End Set
    End Property

End Class

<Serializable()> Public Class ManualJournalItemCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oManualJournalItem As ManualJournalItem) As Integer
        Return List.Add(v_oManualJournalItem)
    End Function

    Public Sub Remove(ByVal v_oManualJournalItem As ManualJournalItem)
        List.Remove(v_oManualJournalItem)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ManualJournalItem
        Get
            Return List(i)
        End Get
        Set(ByVal value As ManualJournalItem)
            List(i) = value
        End Set
    End Property
    
End Class

<Serializable()> Public Class ManualJournalTransactionsCollection : Inherits SortableCollectionBase

        Public Sub New()
            MyBase.SortObjectType = GetType(AuthorisedManualJournalTransactionsList)
        End Sub

        Public Function Print() As String

            Dim sbPrint As New Text.StringBuilder()

            For Each oJournalListItems As AuthorisedManualJournalTransactionsList In List
                sbPrint.AppendLine(oJournalListItems.Print())
                sbPrint.AppendLine("<br />")
            Next

            Return sbPrint.ToString()

        End Function

        Public Function Add(ByVal v_oJournalListItems As AuthorisedManualJournalTransactionsList) As Integer
            Return List.Add(v_oJournalListItems)
        End Function

        Public Sub Remove(ByVal v_oJournalListItems As AuthorisedManualJournalTransactionsList)
            List.Remove(v_oJournalListItems)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            List.RemoveAt(index)
        End Sub

        Default Public Property Item(ByVal i As Integer) As AuthorisedManualJournalTransactionsList
            Get
                Return List(i)
            End Get
            Set(ByVal value As AuthorisedManualJournalTransactionsList)
                List(i) = value
            End Set
        End Property
    End Class

Public Class AuthorisedManualJournalTransactionRequestType

    Private _dateFrom As Date
    Private _dateTo As Date
    Private _accountCode As String
    Private _journalTypeCode As String
    Public Property DateFrom() As Date
        Get
            Return _dateFrom
        End Get
        Set(value As Date)
            _dateFrom = value
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
    Public Property AccountCode() As String
        Get
            Return _accountCode
        End Get
        Set(value As String)
            _accountCode = value
        End Set
    End Property
    Public Property JournalTypeCode() As String
        Get
            Return _journalTypeCode
        End Get
        Set(value As String)
            _journalTypeCode = value
        End Set
    End Property
End Class

Public Class ManualJournalTransactionApprovalRequestType
    Private _iManualJournalId As Integer

    Public Property ManualJournalId() As String
        Get
            Return _iManualJournalId
        End Get
        Set(value As String)
            _iManualJournalId = value
        End Set
    End Property
End Class


<Serializable()> Public Class ManualJournalTransactionApprovalMasterCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(ManualJournalTransactionsMasterList)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oJournalListItems As ManualJournalTransactionsMasterList In List
            sbPrint.AppendLine(oJournalListItems.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oJournalListItems As ManualJournalTransactionsMasterList) As Integer
        Return List.Add(v_oJournalListItems)
    End Function

    Public Sub Remove(ByVal v_oJournalListItems As ManualJournalTransactionsMasterList)
        List.Remove(v_oJournalListItems)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ManualJournalTransactionsMasterList
        Get
            Return List(i)
        End Get
        Set(ByVal value As ManualJournalTransactionsMasterList)
            List(i) = value
        End Set
    End Property
End Class

<Serializable()> Public Class ManualJournalTransactionsMasterList
    Private sBranch As String

    Private sComment As String

    Private dCreatedDate As Date

    Private sDocumentType As String

    Private iIsReferred As Integer
    Private dReversesOn As Date
    Private iRecurringOccurs As Integer
    Private iPerPeriodOnDay As Integer
    Private iPerMonthOnDay As Integer
    Private iPerQuarterOnDay As Integer
    Private sAuthorisationComment As String


    Public Property Branch() As String
        Get
            Return Me.sBranch
        End Get
        Set
            Me.sBranch = Value
        End Set
    End Property


    Public Property Comment() As String
        Get
            Return Me.sComment
        End Get
        Set
            Me.sComment = Value
        End Set
    End Property


    Public Property CreatedDate() As Date
        Get
            Return Me.dCreatedDate
        End Get
        Set
            Me.dCreatedDate = Value
        End Set
    End Property


    Public Property DocumentType() As String
        Get
            Return Me.sDocumentType
        End Get
        Set
            Me.sDocumentType = Value
        End Set
    End Property

    Public Property IsReferred() As Integer
        Get
            Return Me.iIsReferred
        End Get
        Set
            Me.iIsReferred = Value
        End Set
    End Property


    Public Property ReversesOn() As Date
        Get
            Return Me.dReversesOn
        End Get
        Set
            Me.dReversesOn = Value
        End Set
    End Property

    Public Property RecurringOccurs() As Integer
        Get
            Return Me.iRecurringOccurs
        End Get
        Set
            Me.iRecurringOccurs = Value
        End Set
    End Property


    Public Property PerPeriodOnDay() As Integer
        Get
            Return Me.iPerPeriodOnDay
        End Get
        Set
            Me.iPerPeriodOnDay = Value
        End Set
    End Property

    Public Property PerMonthOnDay() As Integer
        Get
            Return Me.iPerMonthOnDay
        End Get
        Set
            Me.iPerMonthOnDay = Value
        End Set
    End Property


    Public Property PerQuarterOnDay() As Integer
        Get
            Return Me.iPerQuarterOnDay
        End Get
        Set
            Me.iPerQuarterOnDay = Value
        End Set
    End Property

    Public Property AuthorisationComment() As String
        Get
            Return Me.sAuthorisationComment
        End Get
        Set
            Me.sAuthorisationComment = Value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder
        sbPrint.AppendLine("Branch : " & sBranch.ToString & "<br />")
        sbPrint.AppendLine("Created: " & dCreatedDate.ToString() & "<br />")
        'sbPrint.AppendLine("CurrencyRate : " & dCurrencyRate & "<br />")
        'sbPrint.AppendLine("BaseAmount : " & dBaseAmount & "<br />")
        'sbPrint.AppendLine("Amount : " & dAmount.ToString() & "<br />")
        'sbPrint.AppendLine("CostCenter : " & sCostCenterId.ToString() & "<br />")
        'sbPrint.AppendLine("Underwritingyear : " & iUnderwritingYearId & "<br />")
        'sbPrint.AppendLine("Insuranceref : " & sInsuranceRef & "<br />")
        'sbPrint.AppendLine("PurchaseInvoice : " & sPurchaseInvoiceNumber & "<br />")
        'sbPrint.AppendLine("PurchaseOrder : " & sPurchaseOrderNumber & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class


<Serializable()> Public Class AuthorisedManualJournalTransactionsList
        Private sAccountCode As String
        Private sCurrency As String
        Private sCurrencyCode As String
        Private dAmount As Decimal
        Private dCurrencyRate As Decimal
        Private dBaseAmount As Decimal
        Private sAlternateRef As String
        Private sComment As String
        
        Private iManualJournalId As Integer
        Private iUnderwritingYearId As Integer
        Private sCostCenterId As String
        Private sInsuranceRef As String
        Private sPurchaseOrderNumber As String
        Private sPurchaseInvoiceNumber As String
    Private sCreatedBy As String
    Private sStatus As String
    Private dCreateddate As String

    Public Property AccountCode() As String
            Get
                Return Me.sAccountCode
            End Get
            Set(ByVal value As String)
                Me.sAccountCode = value
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
        
        Public Property CurrencyCode() As String
            Get
                Return Me.sCurrencyCode
            End Get
            Set(ByVal value As String)
                Me.sCurrencyCode = value
            End Set
        End Property
        Public Property Amount() As Decimal
            Get
                Return dAmount
            End Get
            Set(value As Decimal)
                dAmount = value
            End Set
        End Property
        Public Property CurrencyRate() As Decimal
            Get
                Return dCurrencyRate
            End Get
            Set(value As Decimal)
                dCurrencyRate = value
            End Set
        End Property
        Public Property BaseAmount() As Decimal
            Get
                Return dBaseAmount
            End Get
            Set(value As Decimal)
                dBaseAmount = value
            End Set
        End Property
        Public Property AlternateRef() As String
            Get
                Return sAlternateRef
            End Get
            Set(value As String)
                sAlternateRef = value
            End Set
        End Property
        Public Property Comment() As String
            Get
                Return sComment
            End Get
            Set(value As String)
                sComment = value
            End Set
        End Property
        Public Property UnderwritingYearId() As Integer
            Get
                Return iUnderwritingYearId
            End Get
            Set(value As Integer)
                iUnderwritingYearId = value
            End Set
        End Property
        Public Property ManualJournalId() As Integer
            Get
                Return iManualJournalId
            End Get
            Set(value As Integer)
                iManualJournalId = value
            End Set
        End Property
        Public Property CostCenterId() As String
            Get
                Return sCostCenterId
            End Get
            Set(value As String)
                sCostCenterId = value
            End Set
        End Property
        Public Property InsuranceRef() As String
            Get
                Return sInsuranceRef
            End Get
            Set(value As String)
                sInsuranceRef = value
            End Set
        End Property 
         Public Property PurchaseOrderNumber() As String
            Get
                Return sPurchaseOrderNumber
            End Get
            Set(value As String)
                sPurchaseOrderNumber = value
            End Set
        End Property 
         Public Property PurchaseInvoiceNumber() As String
            Get
                Return sPurchaseInvoiceNumber
            End Get
            Set(value As String)
                sPurchaseInvoiceNumber = value
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

    Public Property CreatedDate() As String
        Get
            Return dCreateddate
        End Get
        Set(value As String)
            dCreateddate = value
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
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder
        sbPrint.AppendLine("AccountCode : " & sAccountCode.ToString & "<br />")
        sbPrint.AppendLine("Currency: " & sCurrency.ToString() & "<br />")
        sbPrint.AppendLine("CurrencyRate : " & dCurrencyRate & "<br />")
        sbPrint.AppendLine("BaseAmount : " & dBaseAmount & "<br />")
        sbPrint.AppendLine("Amount : " & dAmount.ToString() & "<br />")
        sbPrint.AppendLine("CostCenter : " & sCostCenterId.ToString() & "<br />")
        sbPrint.AppendLine("Underwritingyear : " & iUnderwritingYearId & "<br />")
        sbPrint.AppendLine("Insuranceref : " & sInsuranceRef & "<br />")
        sbPrint.AppendLine("PurchaseInvoice : " & sPurchaseInvoiceNumber & "<br />")
        sbPrint.AppendLine("PurchaseOrder : " & sPurchaseOrderNumber & "<br />")
        
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class



     