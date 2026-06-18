<Serializable()> Public Class Client
    Private sShortName As String, sServiceLevelCode As String, sAreaCode As String, sCorrespondenceCode As String, sPaymentCode As String, sReminderCode As String, sPaymentTermCode As String, sRenewalStopCode As String, sLoyaltyNumber As String, sSeasonalGiftCode As String, sAgentReference As String, sStrengthCode As String, sStatusCode As String, sCurrentIntermediaryName As String, sLeadAgentCode As String, sLeadAgentName As String, sPreviousInsurerCode As String, sPreviousInsurerName As String, sPreviousBrokerCode As String, sPreviousBrokerName As String, sResolvedName As String, sBlacklistReasonCode As String
    Private iLeadAgentKey, iCurrentIntermediaryKey, iPreviousInsurerKey, iPreviousBrokerKey As Integer
    Private bIsProspect, bIsAgent, bIsAtFault As Boolean
    Private dCountyCourtJudgments, dAccountBalance, dYearToDateTurnover, dLastYearTurnover As Decimal
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bIsAtFault = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Short Name : " & sShortName & "<br />")
        sbPrint.AppendLine("Resolved Name : " & sResolvedName & "<br />")
        sbPrint.AppendLine("Service Level Code : " & sServiceLevelCode & "<br />")
        sbPrint.AppendLine("Area Code : " & sAreaCode & "<br />")
        sbPrint.AppendLine("Correspondence Code : " & sCorrespondenceCode & "<br />")
        sbPrint.AppendLine("Payment Code : " & sPaymentCode & "<br />")
        sbPrint.AppendLine("Reminder Code : " & sReminderCode & "<br />")
        sbPrint.AppendLine("Payment Term Code : " & sPaymentTermCode & "<br />")
        sbPrint.AppendLine("Renewal Stop Code : " & sRenewalStopCode & "<br />")
        sbPrint.AppendLine("Loyalty Number : " & sLoyaltyNumber & "<br />")
        sbPrint.AppendLine("Seasonal Gift Code : " & sSeasonalGiftCode & "<br />")
        sbPrint.AppendLine("Agent Reference : " & sAgentReference & "<br />")
        sbPrint.AppendLine("Strength Code : " & sStrengthCode & "<br />")
        sbPrint.AppendLine("Status Code : " & sStatusCode & "<br />")
        sbPrint.AppendLine("Current Intermediary Name : " & sCurrentIntermediaryName & "<br />")
        sbPrint.AppendLine("Lead Agent Code : " & sLeadAgentCode & "<br />")
        sbPrint.AppendLine("Previous Insurer Code : " & sPreviousInsurerCode & "<br />")
        sbPrint.AppendLine("Previous Insurer Name : " & sPreviousInsurerName & "<br />")
        sbPrint.AppendLine("Previous Broker Code : " & sPreviousBrokerCode & "<br />")
        sbPrint.AppendLine("Previous Broker Name : " & sPreviousBrokerName & "<br />")
        sbPrint.AppendLine("LeadAgent Key : " & iLeadAgentKey & "<br />")
        sbPrint.AppendLine("Current Intermediary Key : " & iCurrentIntermediaryKey & "<br />")
        sbPrint.AppendLine("Previous Insurer Key : " & iPreviousInsurerKey & "<br />")
        sbPrint.AppendLine("Previous Broker Key : " & iPreviousBrokerKey & "<br />")
        sbPrint.AppendLine("County Court Judgments : " & dCountyCourtJudgments & "<br />")
        sbPrint.AppendLine("Account Balance : " & dAccountBalance & "<br />")
        sbPrint.AppendLine("Year To Date Turnover : " & dYearToDateTurnover & "<br />")
        sbPrint.AppendLine("Last Year Turnover : " & dLastYearTurnover & "<br />")
        sbPrint.AppendLine("Is Prospect: " & IIf(bIsProspect, "true", "false") & "<br />")
        sbPrint.AppendLine("Is Agent: " & IIf(bIsAgent, "true", "false") & "<br />")
        Return sbPrint.ToString

    End Function
    Public Property ShortName() As String
        Get
            Return sShortName
        End Get
        Set(ByVal value As String)
            sShortName = value
        End Set
    End Property
    Public Property ServiceLevelCode() As String
        Get
            Return sServiceLevelCode
        End Get
        Set(ByVal value As String)
            sServiceLevelCode = value
        End Set
    End Property
    Public Property AreaCode() As String
        Get
            Return sAreaCode
        End Get
        Set(ByVal value As String)
            sAreaCode = value
        End Set
    End Property
    Public Property CorrespondenceCode() As String
        Get
            Return sCorrespondenceCode
        End Get
        Set(ByVal value As String)
            sCorrespondenceCode = value
        End Set
    End Property
    Public Property PaymentCode() As String
        Get
            Return sPaymentCode
        End Get
        Set(ByVal value As String)
            sPaymentCode = value
        End Set
    End Property
    Public Property ReminderCode() As String
        Get
            Return sReminderCode
        End Get
        Set(ByVal value As String)
            sReminderCode = value
        End Set
    End Property
    Public Property PaymentTermCode() As String
        Get
            Return sPaymentTermCode
        End Get
        Set(ByVal value As String)
            sPaymentTermCode = value
        End Set
    End Property
    Public Property RenewalStopCode() As String
        Get
            Return sRenewalStopCode
        End Get
        Set(ByVal value As String)
            sRenewalStopCode = value
        End Set
    End Property
    Public Property LoyaltyNumber() As String
        Get
            Return sLoyaltyNumber
        End Get
        Set(ByVal value As String)
            sLoyaltyNumber = value
        End Set
    End Property
    Public Property SeasonalGiftCode() As String
        Get
            Return sSeasonalGiftCode
        End Get
        Set(ByVal value As String)
            sSeasonalGiftCode = value
        End Set
    End Property
    Public Property AgentReference() As String
        Get
            Return sAgentReference
        End Get
        Set(ByVal value As String)
            sAgentReference = value
        End Set

    End Property
    Public Property StrengthCode() As String
        Get
            Return sStrengthCode
        End Get
        Set(ByVal value As String)
            sStrengthCode = value
        End Set

    End Property
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set

    End Property
    Public Property CurrentIntermediaryName() As String
        Get
            Return sCurrentIntermediaryName
        End Get
        Set(ByVal value As String)
            sCurrentIntermediaryName = value
        End Set

    End Property
    Public Property LeadAgentCode() As String
        Get
            Return sLeadAgentCode
        End Get
        Set(ByVal value As String)
            sLeadAgentCode = value
        End Set

    End Property
    Public Property LeadAgentName() As String
        Get
            Return sLeadAgentName
        End Get
        Set(ByVal value As String)
            sLeadAgentName = value
        End Set

    End Property
    Public Property PreviousInsurerCode() As String
        Get
            Return sPreviousInsurerCode
        End Get
        Set(ByVal value As String)
            sPreviousInsurerCode = value
        End Set

    End Property
    Public Property PreviousInsurerName() As String
        Get
            Return sPreviousInsurerName
        End Get
        Set(ByVal value As String)
            sPreviousInsurerName = value
        End Set

    End Property
    Public Property PreviousBrokerCode() As String
        Get
            Return sPreviousBrokerCode
        End Get
        Set(ByVal value As String)
            sPreviousBrokerCode = value
        End Set

    End Property
    Public Property PreviousBrokerName() As String
        Get
            Return sPreviousBrokerName
        End Get
        Set(ByVal value As String)
            sPreviousBrokerName = value
        End Set

    End Property
    Public Property LeadAgentKey() As Integer
        Get
            Return iLeadAgentKey
        End Get
        Set(ByVal value As Integer)
            iLeadAgentKey = value
        End Set

    End Property
    Public Property CurrentIntermediaryKey() As Integer
        Get
            Return iCurrentIntermediaryKey
        End Get
        Set(ByVal value As Integer)
            iCurrentIntermediaryKey = value
        End Set

    End Property
    Public Property PreviousInsurerKey() As Integer
        Get
            Return iPreviousInsurerKey
        End Get
        Set(ByVal value As Integer)
            iPreviousInsurerKey = value
        End Set

    End Property
    Public Property PreviousBrokerKey() As Integer
        Get
            Return iPreviousBrokerKey
        End Get
        Set(ByVal value As Integer)
            iPreviousBrokerKey = value
        End Set

    End Property

    Public Property IsProspect() As Boolean
        Get
            Return bIsProspect
        End Get
        Set(ByVal value As Boolean)
            bIsProspect = value
        End Set
    End Property
    Public Property IsAgent() As Boolean
        Get
            Return bIsAgent
        End Get
        Set(ByVal value As Boolean)
            bIsAgent = value
        End Set
    End Property
    Public Property CountyCourtJudgments() As Decimal
        Get
            Return dCountyCourtJudgments
        End Get
        Set(ByVal value As Decimal)
            dCountyCourtJudgments = value
        End Set
    End Property
    Public Property AccountBalance() As Decimal
        Get
            Return dAccountBalance
        End Get
        Set(ByVal value As Decimal)
            dAccountBalance = value
        End Set

    End Property
    Public Property YearToDateTurnover() As Decimal
        Get
            Return dYearToDateTurnover
        End Get
        Set(ByVal value As Decimal)
            dYearToDateTurnover = value
        End Set
    End Property
    Public Property LastYearTurnover() As Decimal
        Get
            Return dLastYearTurnover
        End Get
        Set(ByVal value As Decimal)
            dLastYearTurnover = value
        End Set
    End Property
    Public Property ResolvedName() As String
        Get
            Return sResolvedName
        End Get
        Set(ByVal value As String)
            sResolvedName = value
        End Set
    End Property

    Public Property BlacklistReasonCode() As String
        Get
            Return sBlacklistReasonCode
        End Get
        Set(ByVal value As String)
            sBlacklistReasonCode = value
        End Set
    End Property
End Class
