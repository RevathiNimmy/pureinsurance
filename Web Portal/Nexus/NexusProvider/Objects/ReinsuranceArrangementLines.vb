Public Class ReinsuranceArrangementLines

    Private iClaimId As Integer
    Private iArrangementId As Integer
    Private iMode As Integer
    Private iBandId As Integer

    Private sName As String
    Private dDefaultPerc As Double
    Private dThisPerc As Double
    Private dSumInsured As Double
    Private dReserveToDate, dRecoveryToDate As Double
    Private dThisReserve As Double
    Private dPaymentToDate As Double
    Private dThisPayment As Double
    Private dBalance As Double
    Private sAgreement As String

    Private sRIPlacement As String
    Private sRIName As String
    Private bIsDomiciledForTax As Boolean
    Private bIsBroker As Boolean
    Private iRIArrangementLineKey As Integer
    Private iRIArrangementKey As Integer
    Private bIsCommissionModified As Boolean
    Private bCedePremiumOnly As Boolean
    Private sReinsuranceTypeCode As String
    Private oBrokerParticipants As BrokerParticipantsCollection
    Private sRICode As String
    Private dParticipation As Double
    Public Sub New()
        oBrokerParticipants = New BrokerParticipantsCollection
    End Sub
    Public Property ClaimId() As Integer
        Get
            Return Me.iClaimId
        End Get
        Set(ByVal value As Integer)
            Me.iClaimId = value
        End Set
    End Property

    Public Property ArrangementId() As Integer
        Get
            Return Me.iArrangementId
        End Get
        Set(ByVal value As Integer)
            Me.iArrangementId = value
        End Set
    End Property

    Public Property Mode() As Integer
        Get
            Return Me.iMode
        End Get
        Set(ByVal value As Integer)
            Me.iMode = value
        End Set
    End Property

    Public Property BandId() As Integer
        Get
            Return Me.iBandId
        End Get
        Set(ByVal value As Integer)
            Me.iBandId = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    Public Property DefaultPerc() As Double
        Get
            Return Me.dDefaultPerc
        End Get
        Set(ByVal value As Double)
            Me.dDefaultPerc = value
        End Set
    End Property

    Public Property ThisPerc() As Double
        Get
            Return Me.dThisPerc
        End Get
        Set(ByVal value As Double)
            Me.dThisPerc = value
        End Set
    End Property

    Public Property SumInsured() As Double
        Get
            Return Me.dSumInsured
        End Get
        Set(ByVal value As Double)
            Me.dSumInsured = value
        End Set
    End Property

    Public Property ReserveToDate() As Double
        Get
            Return Me.dReserveToDate
        End Get
        Set(ByVal value As Double)
            Me.dReserveToDate = value
        End Set
    End Property
    Public Property RecoveryToDate() As Double
        Get
            Return Me.dRecoveryToDate
        End Get
        Set(ByVal value As Double)
            Me.dRecoveryToDate = value
        End Set
    End Property
    Public Property ThisReserve() As Double
        Get
            Return Me.dThisReserve
        End Get
        Set(ByVal value As Double)
            Me.dThisReserve = value
        End Set
    End Property

    Public Property PaymentToDate() As Double
        Get
            Return Me.dPaymentToDate
        End Get
        Set(ByVal value As Double)
            Me.dPaymentToDate = value
        End Set
    End Property

    Public Property ThisPayment() As Double
        Get
            Return Me.dThisPayment
        End Get
        Set(ByVal value As Double)
            Me.dThisPayment = value
        End Set
    End Property

    Public Property Balance() As Double
        Get
            Return Me.dBalance
        End Get
        Set(ByVal value As Double)
            Me.dBalance = value
        End Set
    End Property

    Public Property Agreement() As String
        Get
            Return Me.sAgreement
        End Get
        Set(ByVal value As String)
            Me.sAgreement = value
        End Set
    End Property

    Public Property RIPlacement() As String
        Get
            Return Me.sRIPlacement
        End Get
        Set(ByVal value As String)
            Me.sRIPlacement = value
        End Set
    End Property

    Public Property RIName() As String
        Get
            Return Me.sRIName
        End Get
        Set(ByVal value As String)
            Me.sRIName = value
        End Set
    End Property

    Public Property RICode() As String
        Get
            Return Me.sRICode
        End Get
        Set(ByVal value As String)
            Me.sRICode = value
        End Set
    End Property

    Public Property IsDomiciledForTax() As Boolean
        Get
            Return Me.bIsDomiciledForTax
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDomiciledForTax = value
        End Set
    End Property

    Public Property IsBroker() As Boolean
        Get
            Return Me.bIsBroker
        End Get
        Set(ByVal value As Boolean)
            Me.bIsBroker = value
        End Set
    End Property

    Public Property RIArrangementLineKey() As Integer
        Get
            Return Me.iRIArrangementLineKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIArrangementLineKey = value
        End Set
    End Property

    Public Property RIarrangementKey() As Integer
        Get
            Return Me.iRIarrangementKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIarrangementKey = value
        End Set
    End Property

    Public Property IsCommissionModified() As Boolean
        Get
            Return Me.bIsCommissionModified
        End Get
        Set(ByVal value As Boolean)
            Me.bIsCommissionModified = value
        End Set
    End Property

    Public Property CedePremiumOnly() As Boolean
        Get
            Return Me.bCedePremiumOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bCedePremiumOnly = value
        End Set
    End Property

    Public Property ReinsuranceTypeCode() As String
        Get
            Return Me.sReinsuranceTypeCode
        End Get
        Set(ByVal value As String)
            Me.sReinsuranceTypeCode = value
        End Set
    End Property

    Public Property BrokerParticipants() As BrokerParticipantsCollection
        Get
            Return oBrokerParticipants
        End Get
        Set(ByVal value As BrokerParticipantsCollection)
            oBrokerParticipants = value
        End Set
    End Property

    Public Property Participation() As Double
        Get
            Return Me.dParticipation
        End Get
        Set(ByVal value As Double)
            Me.dParticipation = value
        End Set
    End Property
    '''' <summary>
    '''' Debug interface
    '''' </summary>
    '''' <returns>A HTML string containing the contents of the object</returns>
    'Public Function Print() As String

    '    Dim sbPrint As New Text.StringBuilder()

    '    sbPrint.AppendLine("Claim Key : " & iClaimKey.ToString() & "<br />")
    '    sbPrint.AppendLine("<br />")
    '    sbPrint.AppendLine("Include Totals : " & bIncludeTotals & "<br />")
    '    sbPrint.AppendLine("<br />")
    '    sbPrint.AppendLine("Include TPRecovery : " & bIncludeTPRecovery & "<br />")
    '    sbPrint.AppendLine("<br />")
    '    sbPrint.AppendLine("Include SalvageRecovery : " & bIncludeSalvageRecovery & "<br />")
    '    sbPrint.AppendLine("<br />")
    '    sbPrint.AppendLine("Include ReserveTypes : " & bIncludeReserveTypes & "<br />")
    '    Return sbPrint.ToString()

    'End Function

End Class

<Serializable()> Public Class ReinsuranceArrangementLineCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the ReinsuranceArrangementLine contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As ReinsuranceArrangementLine In List
        '    sbPrint.AppendLine(oReinsuranceArrangementLine.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ReinsuranceArrangementLine object to the collection
    ''' </summary>
    ''' <param name="v_oReinsuranceArrangementLines">The ReinsuranceArrangementLine object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oReinsuranceArrangementLines As ReinsuranceArrangementLines) As Integer
        Return List.Add(v_oReinsuranceArrangementLines)
    End Function

    ''' <summary>
    ''' Remove an ReinsuranceArrangementLines object from the collection
    ''' </summary>
    ''' <param name="v_oReinsuranceArrangementLines">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oReinsuranceArrangementLines As ReinsuranceArrangementLines)
        List.Remove(v_oReinsuranceArrangementLines)
    End Sub

    ''' <summary>
    ''' Remove an ReinsuranceArrangementLines object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ReinsuranceArrangementLines object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ReinsuranceArrangementLines object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement ReinsuranceArrangementLines object</value>
    ''' <returns>The ReinsuranceArrangementLines object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ReinsuranceArrangementLines
        Get
            Return List(i)
        End Get
        Set(ByVal value As ReinsuranceArrangementLines)
            List(i) = value
        End Set
    End Property

End Class

