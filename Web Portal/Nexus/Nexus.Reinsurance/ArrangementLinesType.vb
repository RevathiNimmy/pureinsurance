''' <summary>
''' this hold all then RI lines values.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ArrangementLinesType

    Private sName As String
    Private dDefaultPerc As Double
    Private dThisPerc As Double
    Private dsumInsured As Double
    Private dpremiumValue As Double
    Private dTax As Double
    Private dCommissionPerc As Double
    Private dCommissionValue, dLineLimit, dLowerLimit As Double
    Private dCommissionTax, dParticipationPercent, dPremiumPercent, dTaxPercentage As Double
    Private dPremiumTax, dRetained As Double
    Private sAgreementCode As String

    Private sRIPlacement As String
    Private sRIName, sReinsurerCode As String
    Private bIsDomiciledForTax As Boolean
    Private bIsBroker As Boolean
    Private iRIArrangementLineKey, iPartyKey, iPriority As Integer
    Private iRIArrangementKey, iGrouping As Integer
    Private dNumberOfLines As Decimal
    Private bIsCommissionModified As Boolean
    Private bCedePremiumOnly, bIsRIBroker As Boolean
    Private sReinsuranceTypeCode, sTreatyCode, sType, sAcctType As String
    Private oBrokerParticipants As BrokerParticipantsCollection
    Private sActionType As RowAction
    Private oFAXParticipants As FAXParticipantsCollection
    Private dBalance, dPaymentToDate, dRecoverToDate, dReserveToDate As Double
    Private dThisReserve, dThisSharePercent, dThisPayment, dIncurred As Double
    Private bIsObligatory As Boolean
    Private dFACPropPremiumPercent As Double
    Private bManuallyAdded As Boolean
    Private iTreatyId As Int32

    Public Sub New()
        oBrokerParticipants = New BrokerParticipantsCollection
        oFAXParticipants = New FAXParticipantsCollection
    End Sub
    '''<remarks/>
    Public Property Incurred() As Double
        Get
            Return Me.dIncurred
        End Get
        Set(ByVal value As Double)
            Me.dIncurred = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsObligatory() As Boolean
        Get
            Return Me.bIsObligatory
        End Get
        Set(ByVal value As Boolean)
            Me.bIsObligatory = value
        End Set
    End Property
    '''<remarks/>
    Public Property ThisPayment() As Double
        Get
            Return Me.dThisPayment
        End Get
        Set(ByVal value As Double)
            Me.dThisPayment = value
        End Set
    End Property
    '''<remarks/>
    Public Property ThisSharePercent() As Double
        Get
            Return Me.dThisSharePercent
        End Get
        Set(ByVal value As Double)
            Me.dThisSharePercent = value
        End Set
    End Property
    '''<remarks/>
    Public Property ThisReserve() As Double
        Get
            Return Me.dThisReserve
        End Get
        Set(ByVal value As Double)
            Me.dThisReserve = value
        End Set
    End Property
    '''<remarks/>
    Public Property ReserveToDate() As Double
        Get
            Return Me.dReserveToDate
        End Get
        Set(ByVal value As Double)
            Me.dReserveToDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property RecoverToDate() As Double
        Get
            Return Me.dRecoverToDate
        End Get
        Set(ByVal value As Double)
            Me.dRecoverToDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property PaymentToDate() As Double
        Get
            Return Me.dPaymentToDate
        End Get
        Set(ByVal value As Double)
            Me.dPaymentToDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property Balance() As Double
        Get
            Return Me.dBalance
        End Get
        Set(ByVal value As Double)
            Me.dBalance = value
        End Set
    End Property
    Public Property FAXParticipants() As FAXParticipantsCollection
        Get
            Return oFAXParticipants
        End Get
        Set(ByVal value As FAXParticipantsCollection)
            oFAXParticipants = value
        End Set
    End Property
    '''<remarks/>
    Public Property Type() As String
        Get
            Return Me.sType
        End Get
        Set(ByVal value As String)
            Me.sType = value
        End Set
    End Property
    '''<remarks/>
    Public Property TreatyCode() As String
        Get
            Return Me.sTreatyCode
        End Get
        Set(ByVal value As String)
            Me.sTreatyCode = value
        End Set
    End Property
    Public Property TreatyID() As Int32
        Get
            Return Me.iTreatyId
        End Get
        Set(ByVal value As Int32)
            Me.iTreatyId = value
        End Set
    End Property
    '''<remarks/>
    Public Property Retained() As Double
        Get
            Return Me.dRetained
        End Get
        Set(ByVal value As Double)
            Me.dRetained = value
        End Set
    End Property
    '''<remarks/>
    Public Property Priority() As Integer
        Get
            Return Me.iPriority
        End Get
        Set(ByVal value As Integer)
            Me.iPriority = value
        End Set
    End Property
    '''<remarks/>
    Public Property PremiumTax() As Double
        Get
            Return Me.dPremiumTax
        End Get
        Set(ByVal value As Double)
            Me.dPremiumTax = value
        End Set
    End Property
    '''<remarks/>
    Public Property PremiumPercent() As Double
        Get
            Return Me.dPremiumPercent
        End Get
        Set(ByVal value As Double)
            Me.dPremiumPercent = value
        End Set
    End Property
    Public Property FACPropPremiumPerc() As Double
        Get
            Return Me.dFACPropPremiumPercent
        End Get
        Set(ByVal value As Double)
            Me.dFACPropPremiumPercent = value
        End Set
    End Property
    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property ParticipationPercent() As Double
        Get
            Return Me.dParticipationPercent
        End Get
        Set(ByVal value As Double)
            Me.dParticipationPercent = value
        End Set
    End Property
    '''<remarks/>
    Public Property NumberOfLines() As Decimal
        Get
            Return Me.dNumberOfLines
        End Get
        Set(ByVal value As Decimal)
            Me.dNumberOfLines = value
        End Set
    End Property
    '''<remarks/>
    Public Property LowerLimit() As Double
        Get
            Return Me.dLowerLimit
        End Get
        Set(ByVal value As Double)
            Me.dLowerLimit = value
        End Set
    End Property
    '''<remarks/>
    Public Property LineLimit() As Double
        Get
            Return Me.dLineLimit
        End Get
        Set(ByVal value As Double)
            Me.dLineLimit = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsRIBroker() As Boolean
        Get
            Return Me.bIsRIBroker
        End Get
        Set(ByVal value As Boolean)
            Me.bIsRIBroker = value
        End Set
    End Property
    '''<remarks/>
    Public Property Grouping() As Integer
        Get
            Return Me.iGrouping
        End Get
        Set(ByVal value As Integer)
            Me.iGrouping = value
        End Set
    End Property
    '''<remarks/>
    Public Property ActionType() As RowAction
        Get
            Return Me.sActionType
        End Get
        Set(ByVal value As RowAction)
            Me.sActionType = value
        End Set
    End Property


    '''<remarks/>
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    '''<remarks/>
    Public Property DefaultPerc() As Double
        Get
            Return Me.dDefaultPerc
        End Get
        Set(ByVal value As Double)
            Me.dDefaultPerc = value
        End Set
    End Property

    '''<remarks/>
    Public Property ThisPerc() As Double
        Get
            Return Me.dThisPerc
        End Get
        Set(ByVal value As Double)
            Me.dThisPerc = value
        End Set
    End Property

    '''<remarks/>
    Public Property SumInsured() As Double
        Get
            Return Me.dsumInsured
        End Get
        Set(ByVal value As Double)
            Me.dsumInsured = value
        End Set
    End Property

    '''<remarks/>
    Public Property PremiumValue() As Double
        Get
            Return Me.dpremiumValue
        End Get
        Set(ByVal value As Double)
            Me.dpremiumValue = value
        End Set
    End Property

    '''<remarks/>
    Public Property Tax() As Double
        Get
            Return Me.dTax
        End Get
        Set(ByVal value As Double)
            Me.dTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property CommissionPerc() As Double
        Get
            Return Me.dCommissionPerc
        End Get
        Set(ByVal value As Double)
            Me.dCommissionPerc = value
        End Set
    End Property

    '''<remarks/>
    Public Property CommissionValue() As Double
        Get
            Return Me.dCommissionValue
        End Get
        Set(ByVal value As Double)
            Me.dCommissionValue = value
        End Set
    End Property

    '''<remarks/>
    Public Property CommissionTax() As Double
        Get
            Return Me.dCommissionTax
        End Get
        Set(ByVal value As Double)
            Me.dCommissionTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxPercentage() As Double
        Get
            Return Me.dTaxPercentage
        End Get
        Set(ByVal value As Double)
            Me.dTaxPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property AgreementCode() As String
        Get
            Return Me.sAgreementCode
        End Get
        Set(ByVal value As String)
            Me.sAgreementCode = value
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
    Public Property ReinsurerCode() As String
        Get
            Return Me.sReinsurerCode
        End Get
        Set(ByVal value As String)
            Me.sReinsurerCode = value
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
            Return Me.iRIArrangementKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIArrangementKey = value
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
    Public Property AcctType() As String
        Get
            Return Me.sAcctType
        End Get
        Set(ByVal value As String)
            Me.sAcctType = value
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

    '''<remarks/>
    Public Property ManuallyAdded() As Boolean
        Get
            Return Me.bManuallyAdded
        End Get
        Set(ByVal value As Boolean)
            Me.bManuallyAdded = value
        End Set
    End Property

    Private bIsEdited As Boolean

    Public Property IsEdited() As Boolean
        Get
            Return bIsEdited
        End Get
        Set(ByVal value As Boolean)
            bIsEdited = value
        End Set
    End Property

    Private bIsEditedDB As Boolean

    Public Property IsEditedDB() As Boolean
        Get
            Return bIsEditedDB
        End Get
        Set(ByVal value As Boolean)
            bIsEditedDB = value
        End Set
    End Property
End Class

<Serializable()> Public Class ArrangementLinesTypeCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        'sbPrint.AppendLine("sName : " & sName & "<br />")
        'sbPrint.AppendLine("dDefaultPerc : " & dDefaultPerc.ToString() & "<br />")
        'sbPrint.AppendLine("dThisPerc : " & dThisPerc.ToString() & "<br />")
        'sbPrint.AppendLine("dsumInsured : " & dsumInsured.ToString() & "<br />")
        'sbPrint.AppendLine("dpremium: " & dpremium.ToString() & "<br />")
        'sbPrint.AppendLine("dTax : " & dTax.ToString() & "<br />")
        'sbPrint.AppendLine("dCommissionPerc : " & dCommissionPerc.ToString() & "<br />")
        'sbPrint.AppendLine("dCommission : " & dCommission.ToString() & "<br />")
        'sbPrint.AppendLine("dCommissionTax : " & dCommissionTax.ToString() & "<br />")
        'sbPrint.AppendLine("sAgreement : " & sAgreement & "<br />")


        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ArrangementLinesType object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oArrangementLinesType As ArrangementLinesType) As Integer
        Return List.Add(v_oArrangementLinesType)
    End Function

    ''' <summary>
    ''' Remove an ArrangementLinesType object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oArrangementLinesType As ArrangementLinesType)
        List.Remove(v_oArrangementLinesType)
    End Sub

    ''' <summary>
    ''' Remove an ArrangementLinesType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ArrangementsType object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ArrangementLinesType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ArrangementLinesType object</param>
    ''' <value>The replacement ArrangementLinesType object</value>
    ''' <returns>The ArrangementLinesType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ArrangementLinesType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ArrangementLinesType)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class BrokerParticipants
    Private iPartyKey As Integer
    Private dParticipationPercentage As Decimal
    Private sPartyCode, sPartyName As String
    Private dTotalParticipationPercentage As Decimal

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("PartyKey   : " & iPartyKey & "<br />")
        sbPrint.AppendLine("ParticipationPercentage : " & dParticipationPercentage & "<br />")

        Return sbPrint.ToString

    End Function
    '''<remarks/>
    Public Property PartyName() As String
        Get
            Return Me.sPartyName
        End Get
        Set(ByVal value As String)
            Me.sPartyName = value
        End Set
    End Property
    '''<remarks/>
    Public Property PartyCode() As String
        Get
            Return Me.sPartyCode
        End Get
        Set(ByVal value As String)
            Me.sPartyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ParticipationPercentage() As Decimal
        Get
            Return Me.dParticipationPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dParticipationPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalParticipationPercentage() As Decimal
        Get
            Return Me.dTotalParticipationPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalParticipationPercentage = value
        End Set
    End Property

End Class


<Serializable()> Public Class BrokerParticipantsCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oBrokerParticipants As BrokerParticipants In List
            sbPrint.AppendLine(oBrokerParticipants.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oBrokerParticipants As BrokerParticipants) As Integer
        Return List.Add(v_oBrokerParticipants)
    End Function

    Public Sub Remove(ByVal v_oBrokerParticipants As BrokerParticipants)
        List.Remove(v_oBrokerParticipants)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As BrokerParticipants
        Get
            Return List(i)
        End Get
        Set(ByVal value As BrokerParticipants)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class FAXParticipants
    Private iPartyKey, iRIArrangementLineKey As Integer
    Private dParticipationPercentage As Double
    Private sPartyCode, sPartyName As String
    Private sAccountType, sAgreementCode As String
    Private dCommissionPercent, dCommissionTax, dCommissionValue, dPremiumTax, dTaxPercentage As Double
    Private dPremiumValue, dSumInsured As Double
    Private oBrokerParticipants As BrokerParticipantsCollection
    Private dTotalPremium, dTotalSI, dTotalCommPerc, dTotalParticipationPercentage, dTotalCommission, dTotalTax, dTotalCommTax As Double
  Private sPaymentToDate, dRecoverToDate, dReserveToDate, dThisPayment, dThisReserve As Double
    Private sActionType As RowAction
    Public Sub New()
        oBrokerParticipants = New BrokerParticipantsCollection
    End Sub

    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("PartyKey   : " & iPartyKey & "<br />")
        sbPrint.AppendLine("ParticipationPercentage : " & dParticipationPercentage & "<br />")

        Return sbPrint.ToString

    End Function
    '''<remarks/>
    Public Property ActionType() As RowAction
        Get
            Return Me.sActionType
        End Get
        Set(ByVal value As RowAction)
            Me.sActionType = value
        End Set
    End Property
    '''<remarks/>
    Public Property ThisReserve() As Double
        Get
            Return Me.dThisReserve
        End Get
        Set(ByVal value As Double)
            Me.dThisReserve = value
        End Set
    End Property
    '''<remarks/>
    Public Property ThisPayment() As Double
        Get
            Return Me.dThisPayment
        End Get
        Set(ByVal value As Double)
            Me.dThisPayment = value
        End Set
    End Property
    '''<remarks/>
    Public Property ReserveToDate() As Double
        Get
            Return Me.dReserveToDate
        End Get
        Set(ByVal value As Double)
            Me.dReserveToDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property RecoverToDate() As Double
        Get
            Return Me.dRecoverToDate
        End Get
        Set(ByVal value As Double)
            Me.dRecoverToDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property PaymentToDate() As Double
        Get
            Return Me.sPaymentToDate
        End Get
        Set(ByVal value As Double)
            Me.sPaymentToDate = value
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
    '''<remarks/>
    Public Property SumInsured() As Double
        Get
            Return Me.dSumInsured
        End Get
        Set(ByVal value As Double)
            Me.dSumInsured = value
        End Set
    End Property
    '''<remarks/>
    Public Property PremiumValue() As Double
        Get
            Return Me.dPremiumValue
        End Get
        Set(ByVal value As Double)
            Me.dPremiumValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property PremiumTax() As Double
        Get
            Return Me.dPremiumTax
        End Get
        Set(ByVal value As Double)
            Me.dPremiumTax = value
        End Set
    End Property
    '''<remarks/>
    Public Property CommissionValue() As Double
        Get
            Return Me.dCommissionValue
        End Get
        Set(ByVal value As Double)
            Me.dCommissionValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property CommissionTax() As Double
        Get
            Return Me.dCommissionTax
        End Get
        Set(ByVal value As Double)
            Me.dCommissionTax = value
        End Set
    End Property
    '''<remarks/>
    Public Property CommissionPercent() As Double
        Get
            Return Me.dCommissionPercent
        End Get
        Set(ByVal value As Double)
            Me.dCommissionPercent = value
        End Set
    End Property
    '''<remarks/>
    Public Property TaxPercentage() As Double
        Get
            Return Me.dTaxPercentage
        End Get
        Set(ByVal value As Double)
            Me.dTaxPercentage = value
        End Set
    End Property

    '''<remarks/>
    Public Property RIArrangementLineKey() As Integer
        Get
            Return Me.iRIArrangementLineKey
        End Get
        Set(ByVal value As Integer)
            Me.iRIArrangementLineKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property AgreementCode() As String
        Get
            Return Me.sAgreementCode
        End Get
        Set(ByVal value As String)
            Me.sAgreementCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property AccountType() As String
        Get
            Return Me.sAccountType
        End Get
        Set(ByVal value As String)
            Me.sAccountType = value
        End Set
    End Property
    '''<remarks/>
    Public Property PartyName() As String
        Get
            Return Me.sPartyName
        End Get
        Set(ByVal value As String)
            Me.sPartyName = value
        End Set
    End Property
    '''<remarks/>
    Public Property PartyCode() As String
        Get
            Return Me.sPartyCode
        End Get
        Set(ByVal value As String)
            Me.sPartyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ParticipationPercentage() As Double
        Get
            Return Me.dParticipationPercentage
        End Get
        Set(ByVal value As Double)
            Me.dParticipationPercentage = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalSI() As Double
        Get
            Return Me.dTotalSI
        End Get
        Set(ByVal value As Double)
            Me.dTotalSI = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalParticipationPercentage() As Double
        Get
            Return Me.dTotalParticipationPercentage
        End Get
        Set(ByVal value As Double)
            Me.dTotalParticipationPercentage = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalPremium() As Double
        Get
            Return Me.dTotalPremium
        End Get
        Set(ByVal value As Double)
            Me.dTotalPremium = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalTax() As Double
        Get
            Return Me.dTotalTax
        End Get
        Set(ByVal value As Double)
            Me.dTotalTax = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalCommission() As Double
        Get
            Return Me.dTotalCommission
        End Get
        Set(ByVal value As Double)
            Me.dTotalCommission = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalCommTax() As Double
        Get
            Return Me.dTotalCommTax
        End Get
        Set(ByVal value As Double)
            Me.dTotalCommTax = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalCommPerc() As Double
        Get
            Return Me.dTotalCommPerc
        End Get
        Set(ByVal value As Double)
            Me.dTotalCommPerc = value
        End Set
    End Property
End Class


<Serializable()> Public Class FAXParticipantsCollection : Inherits CollectionBase
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oBrokerParticipants As BrokerParticipants In List
            sbPrint.AppendLine(oBrokerParticipants.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oFAXParticipants As FAXParticipants) As Integer
        Return List.Add(v_oFAXParticipants)
    End Function

    Public Sub Remove(ByVal v_oFAXParticipants As FAXParticipants)
        List.Remove(v_oFAXParticipants)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As FAXParticipants
        Get
            Return List(i)
        End Get
        Set(ByVal value As FAXParticipants)
            List(i) = value
        End Set
    End Property

End Class
Public Enum RowAction

    '''<remarks/>
    AddRow

    '''<remarks/>
    EditRow

    '''<remarks/>
    DeleteRow
End Enum

