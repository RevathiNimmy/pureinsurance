<Serializable()> Public Class HeaderAndAgentCommission

    Private iInsuranceFileKeyField As Integer

    Private sClientCodeField As String

    Private sInsuranceFileRefField As String

    Private sAgentField As String

    Private dtInceptionDateField As DateTime

    Private dtCoverStartDateField As DateTime

    Private dtExpiryDateField As DateTime

    Private sCurrencyField As String

    Private dTotalCommissionLeadAgentField As Double

    Private dTotalTaxLeadAgentField As Double

    Private dTotalNetPremiumLeadAgentField As Double

    Private dTotalCommissionSubAgentField As Double

    Private dTotalTaxSubAgentField As Double

    Private dTotalNetPremiumSubAgentField As Double

    Private oAgentCommissionField As AgentCommissionCollection

    Private resultDatasetField As System.Xml.XmlElement

    Public Sub New()
        oAgentCommissionField = New AgentCommissionCollection
        'resultDatasetField = New System.Xml.XmlElement
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Insurance File Key : " & iInsuranceFileKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Client Code : " & sClientCodeField & "<br />")
        sbPrint.AppendLine("Insurance File Ref : " & sInsuranceFileRefField & "<br />")
        sbPrint.AppendLine("Agent : " & sAgentField & "<br />")
        sbPrint.AppendLine("Inception Date : " & dtInceptionDateField.ToString() & "<br />")
        sbPrint.AppendLine("Cover Start Date : " & dtCoverStartDateField.ToString() & "<br />")
        sbPrint.AppendLine("Expiry Date : " & dtExpiryDateField.ToString() & "<br />")
        sbPrint.AppendLine("Currency : " & sCurrencyField & "<br />")
        sbPrint.AppendLine("Total Commission Lead Agent : " & dTotalCommissionLeadAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Total Tax Lead Agent : " & dTotalTaxLeadAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Total NetPremium Lead Agent : " & dTotalNetPremiumLeadAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Total Commission Sub Agent : " & dTotalCommissionSubAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Total Tax Sub Agent : " & dTotalTaxSubAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Total Net Premium Sub Agent : " & dTotalNetPremiumSubAgentField.ToString() & "<br />")
        sbPrint.AppendLine("Agent Commission---------------- >")
        sbPrint.AppendLine(oAgentCommissionField.Print() & "<br />")
        'sbPrint.AppendLine("Result : " & resultDatasetField.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKeyField = value
        End Set
    End Property

    Public Property ClientCode() As String
        Get
            Return Me.sClientCodeField
        End Get
        Set(ByVal value As String)
            Me.sClientCodeField = value
        End Set
    End Property

    Public Property InsuranceFileRef() As String
        Get
            Return Me.sInsuranceFileRefField
        End Get
        Set(ByVal value As String)
            Me.sInsuranceFileRefField = value
        End Set
    End Property

    Public Property Agent() As String
        Get
            Return Me.sAgentField
        End Get
        Set(ByVal value As String)
            Me.sAgentField = value
        End Set
    End Property

    Public Property InceptionDate() As DateTime
        Get
            Return Me.dtInceptionDateField
        End Get
        Set(ByVal value As Date)
            Me.dtInceptionDateField = value
        End Set
    End Property

    Public Property CoverStartDate() As DateTime
        Get
            Return Me.dtCoverStartDateField
        End Get
        Set(ByVal value As DateTime)
            Me.dtCoverStartDateField = value
        End Set
    End Property

    Public Property ExpiryDate() As DateTime
        Get
            Return Me.dtExpiryDateField
        End Get
        Set(ByVal value As DateTime)
            Me.dtExpiryDateField = value
        End Set
    End Property

    Public Property Currency() As String
        Get
            Return Me.sCurrencyField
        End Get
        Set(ByVal value As String)
            Me.sCurrencyField = value
        End Set
    End Property

    Public Property TotalCommissionLeadAgent() As Double
        Get
            Return Me.dTotalCommissionLeadAgentField
        End Get
        Set(ByVal value As Double)
            Me.dTotalCommissionLeadAgentField = value
        End Set
    End Property

    Public Property TotalTaxLeadAgent() As Double
        Get
            Return Me.dTotalTaxLeadAgentField
        End Get
        Set(ByVal value As Double)
            Me.dTotalTaxLeadAgentField = value
        End Set
    End Property

    Public Property TotalNetPremiumLeadAgent() As Double
        Get
            Return Me.dTotalNetPremiumLeadAgentField
        End Get
        Set(ByVal value As Double)
            Me.dTotalNetPremiumLeadAgentField = value
        End Set
    End Property

    Public Property TotalCommissionSubAgent() As Double
        Get
            Return Me.dTotalCommissionSubAgentField
        End Get
        Set(ByVal value As Double)
            Me.dTotalCommissionSubAgentField = value
        End Set
    End Property

    Public Property TotalTaxSubAgent() As Double
        Get
            Return Me.dTotalTaxSubAgentField
        End Get
        Set(ByVal value As Double)
            Me.dTotalTaxSubAgentField = value
        End Set
    End Property

    Public Property TotalNetPremiumSubAgent() As Double
        Get
            Return Me.dTotalNetPremiumSubAgentField
        End Get
        Set(ByVal value As Double)
            Me.dTotalNetPremiumSubAgentField = value
        End Set
    End Property

    Public Property AgentCommission() As AgentCommissionCollection
        Get
            Return Me.oAgentCommissionField
        End Get
        Set(ByVal value As AgentCommissionCollection)
            Me.oAgentCommissionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ResultDataset() As System.Xml.XmlElement
        Get
            Return Me.resultDatasetField
        End Get
        Set(ByVal value As System.Xml.XmlElement)
            Me.resultDatasetField = value
        End Set
    End Property

End Class
