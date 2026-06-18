Public Class BasePartyType
    Public Property AccountExecutive As String
    Public Property AccountExecutiveCode As String
    Public Property Addresses As List(Of BaseAddressType)
    Public Property BranchCode As String
    Public Property Contacts As List(Of BaseContactType)
    Public Property Currency As String
    Public Property DomiciledForTax As Boolean
    Public Property FileCode As String
    Public Property SubBranchCode As String
    Public Property TPIntroducer As String
    Public Property TPUserCode As String
    Public Property TaxExempt As Boolean
    Public Property TaxNumber As String
    Public Property TaxPercentage As Decimal
    Public Property XMLDataset As String
    Public Property Agent As String
    Public Property BlacklistReasonCode As String
    Public Property RenewalStopCode As String
End Class