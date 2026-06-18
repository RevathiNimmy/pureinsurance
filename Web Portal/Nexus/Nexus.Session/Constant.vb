

Public Module Constant

    Public Enum Mode
        Add = 0 'New policy
        Edit = 1 'Edit an existing policy
        View = 2 'View an existing policy
        Save = 5 'Save a Quick Quote
        Buy = 6 'Buy a Quick Quote ( .. carry onto Full Quote)
        'Added for Claims
        NewClaim = 7
        EditClaim = 8
        ViewClaim = 9
        PayClaim = 10
        SalvageClaim = 11
        TPRecovery = 12
        Review = 13
        Authorise = 14
        Recommend = 15
        DeclinePayment = 16
        ViewClaimPayment = 17
        PortFolioTransferAmendment = 18
        ClonedTransferAmendment = 19
        DeleteRisk = 20

    End Enum
    Public Enum PaymentHubMode
        NewCard
        OldCard
    End Enum
    Public Enum RiskMode
        Add = 0 'New risk
        Edit = 1 'Edit an existing risk
        View = 2 'View Risk
        Delete = 3 'Delete Risk
        PortfolioTransferAmendment = 4
    End Enum

    Public Enum MTAType
        TEMPORARY = 1
        PERMANENT = 2
        CANCELLATION = 3
        REINSTATEMENT = 4
    End Enum

    Public Enum QuoteMode
        QuickQuote = 1
        FullQuote = 2
        MTAQuote = 3
        ReQuote = 4
    End Enum

    Public Enum LoginType
        Agent = 1
        Customer = 2
    End Enum

    Public Enum LoginButtonType
        Image = 1
        Button = 2
    End Enum

    Public Enum PaymentTypes
        Invoice
        PayNow
        BankGuarantee
        CreditCard
        CashDeposit
        DirectDebit
        PaymentHub
        AgentCollection
        PutMTAOnNextInstalment
    End Enum

    Public Enum WMMode
        Add = 0 'New Task
        Edit = 1 'Edit the Task
        NewTaskLog = 2 'Add New Task Log
        ViewTaskLog = 3 'View the task Log
    End Enum

    Public Enum InsurerPaymentsMarkedStatus
        No
        Yes
        Any
    End Enum

    Public Enum Month
        All
        January
        February
        March
        April
        May
        June
        July
        August
        September
        October
        November
        December
    End Enum

    ' Enum for selecting the type of payment 
    Public Enum PaymentType
        ' Payment
        P
        ' Receipt
        R
        ' Claim Payment
        CP
    End Enum
    ' U = Unchanged and R = Renewed
    ' Sample usage: If Constants.CNCheckStatusFlag.indexOf(sStatusToCheck) = -1 Then
    Public Const CheckStatusFlags As String = "UR"

    ' Enum for selecting the type of INSURANCEFILE 
    Public Enum ViewType
        POLICY = 0
        PERMANENT_MTA = 1
        TEMPORARY_MTA = 2
        CANCELLATION_MTA = 3
        PERMANENT_MTAQUOTE = 4
        TEMPORARY_MTAQUOTE = 5
        CANCELLATION_MTAQUOTE = 6
    End Enum

    Public Const CNDataSetDef As String = "DATA_SET_DEF_" 'prefix for dataset definition object name when cached, full cache name will "DATA_SET_DEF_" & DataModelCode
    Public Const CNMTATypeDesc As String = "PERMANENT" ' as this is fixed for both the cases either PERMANENT or CANCELLATION, used in MTAReason.aspx with ADDMTAQUOTE
    Public Const CNAnonymous As String = "ANONYMOUSUSER" 'This will be set in case of any Anonymous

    ''' <summary>
    ''' This Enum is used for displaying SAM returned values of Instalment Plan Status
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum FinancePlanStatus As Long
        Item000
        Item010
        Item011
        Item012
        Item040
        Item140
        Item900
        Item990
        Item999
    End Enum

    ''' <summary>
    ''' Get Status ID for Finance plan desc
    ''' </summary>
    ''' <param name="eFinancePlanStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FinancePlanStatusDesc(ByVal eFinancePlanStatus As FinancePlanStatus) As String
        Select Case eFinancePlanStatus
            Case FinancePlanStatus.Item000
                Return 0
            Case FinancePlanStatus.Item010
                Return "Saved"
            Case FinancePlanStatus.Item011
                Return "Updated"
            Case FinancePlanStatus.Item012
                Return "QuotePrinted"
            Case FinancePlanStatus.Item040
                Return "Live"
            Case FinancePlanStatus.Item140
                Return "OnHold"
            Case FinancePlanStatus.Item900
                Return "Completed"
            Case FinancePlanStatus.Item990
                Return "Superseded"
            Case FinancePlanStatus.Item999
                Return "Cancelled"
            Case Else
                Return ""
        End Select
    End Function

    ''' <summary>
    ''' Get instalment type
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum InstalmentPlanType
        'Creating an instalment plan
        Add
        'Maintenance an instalment plan
        edit
        'viewing an instalment plan
        View
    End Enum

    ''' <summary>
    ''' PF plan enum
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CancelPFPlanType
        DeletePlan
        SettlePlan
        CancelPlan
    End Enum
    Public Const Cloned As String = "CloneTransfer"
    Public Const PortfolioTransfer As String = "PortfolioTransfer"

    Public Enum PortfolioStatus
        Failed = 1
        RICalculated = 2
        Posted = 3
    End Enum

    Public Enum DeleteRiskNavigation
        None
        RatingSection
        RiskScreen
    End Enum

    ' U = Unchanged and R = Renewed
    ' Sample usage: If Constants.CNCheckStatusFlag.indexOf(sStatusToCheck) = -1 Then
    Public Const CNCheckStatusFlags As String = "UR"

    Public Class RiskStatus
        Public Const Quoted As String = "QUOTED"
        Public Const UnQuoted As String = "UNQUOTED"
        Public Const Deleted As String = "DELETED"
        Public Const Referred As String = "REFERRED"
        Public Const Declined As String = "DECLINED"
    End Class

    Public Function FinancePlanStatusString(ByVal eFinancePlanStatus As FinancePlanStatus) As String
        Select Case eFinancePlanStatus
            Case FinancePlanStatus.Item000
                Return "000"
            Case FinancePlanStatus.Item010
                Return "010"
            Case FinancePlanStatus.Item011
                Return "011"
            Case FinancePlanStatus.Item012
                Return "012"
            Case FinancePlanStatus.Item040
                Return "040"
            Case FinancePlanStatus.Item140
                Return "140"
            Case FinancePlanStatus.Item900
                Return "900"
            Case FinancePlanStatus.Item990
                Return "990"
            Case FinancePlanStatus.Item999
                Return "999"
            Case Else
                Return "000"
        End Select
    End Function

End Module

