<Serializable()> Public Class RenewalStatus

    Private sRenewalStatusTypeCode As String
    Private sRenewalStatusTypeDescription As String
    Private iRenewalStatusKey As Integer
    Private iInsuranceHolderKey As Integer
    Private iLeadAgentKey As Integer
    Private dDateCreated As DateTime
    Private dCriticalDate As DateTime
    Private iIsInvitePrinted As Integer
    Private iOriginalInsuranceFileKey As Integer
    Private dDateInvitePrinted As DateTime
    Private sRenewalExceptionNotes As String
    Private sEmailSent As String
    Private dEmailSentDate As DateTime
    Private sProductCode As String
    Private sRenewalExceptionReasonCode As String
    Private sRenewalExceptionReasonDescription As String
    Private bIsDuplicateRenewalField As Boolean

    Public Sub New()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Renewal Status Code : " & sRenewalStatusTypeCode & "<br />")
        sbPrint.AppendLine("Renewal Status Description : " & sRenewalStatusTypeDescription & "<br />")
        sbPrint.AppendLine("Renewal Status Key" & iRenewalStatusKey.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Holder Key : " & iInsuranceHolderKey.ToString() & "<br />")
        sbPrint.AppendLine("Lead Agent Key : " & iLeadAgentKey.ToString() & "<br />")
        sbPrint.AppendLine("Date Created : " & dDateCreated.ToString() & "<br />")
        sbPrint.AppendLine("Critical Date : " & dCriticalDate.ToString() & "<br />")
        sbPrint.AppendLine("Is Invite Printed : " & iIsInvitePrinted.ToString() & "<br />")
        sbPrint.AppendLine("Insurance File Key : " & iOriginalInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("Date of Invite Printed : " & dDateInvitePrinted.ToString() & "<br />")
        sbPrint.AppendLine("Renewal Exception Notes : " & sRenewalExceptionNotes & "<br />")
        sbPrint.AppendLine("Email Sent : " & sEmailSent & "<br />")
        sbPrint.AppendLine("Email Sent Date : " & dEmailSentDate.ToString() & "<br />")
        sbPrint.AppendLine("Product Code : " & sProductCode & "<br />")
        sbPrint.AppendLine("Renewal Exception Reason Code : " & sRenewalExceptionReasonCode & "<br />")
        sbPrint.AppendLine("Renewal Exception Reason Description : " & sRenewalExceptionReasonDescription & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property RenewalStatusTypeCode() As String
        Get
            Return sRenewalStatusTypeCode
        End Get
        Set(ByVal value As String)
            sRenewalStatusTypeCode = value
        End Set
    End Property

    Public Property RenewalStatusTypeDescription() As String
        Get
            Return sRenewalStatusTypeDescription
        End Get
        Set(ByVal value As String)
            sRenewalStatusTypeDescription = value
        End Set
    End Property

    Public Property RenewalStatusKey() As Integer
        Get
            Return iRenewalStatusKey
        End Get
        Set(ByVal value As Integer)
            iRenewalStatusKey = value
        End Set
    End Property

    Public Property InsuranceHolderKey() As Integer
        Get
            Return iInsuranceHolderKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceHolderKey = value
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

    Public Property DateCreated() As DateTime
        Get
            Return dDateCreated
        End Get
        Set(ByVal value As DateTime)
            dDateCreated = value
        End Set
    End Property

    Public Property CriticalDate() As DateTime
        Get
            Return dCriticalDate
        End Get
        Set(ByVal value As DateTime)
            dCriticalDate = value
        End Set
    End Property
    Public Property IsInvitePrinted() As Integer
        Get
            Return iIsInvitePrinted
        End Get
        Set(ByVal value As Integer)
            iIsInvitePrinted = value
        End Set
    End Property
    Public Property OriginalInsuranceFileKey() As Integer
        Get
            Return iOriginalInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iOriginalInsuranceFileKey = value
        End Set
    End Property
    Public Property DateInvitePrinted() As DateTime
        Get
            Return dDateInvitePrinted
        End Get
        Set(ByVal value As DateTime)
            dDateInvitePrinted = value
        End Set
    End Property
    Public Property RenewalExceptionNotes() As String
        Get
            Return sRenewalExceptionNotes
        End Get
        Set(ByVal value As String)
            sRenewalExceptionNotes = value
        End Set
    End Property
    Public Property EmailSent() As String
        Get
            Return sEmailSent
        End Get
        Set(ByVal value As String)
            sEmailSent = value
        End Set
    End Property
    Public Property EmailSentDate() As DateTime
        Get
            Return dEmailSentDate
        End Get
        Set(ByVal value As DateTime)
            dEmailSentDate = value
        End Set
    End Property
    Public Property ProductCode() As String
        Get
            Return sProductCode
        End Get
        Set(ByVal value As String)
            sProductCode = value
        End Set
    End Property
    Public Property RenewalExceptionReasonCode() As String
        Get
            Return sRenewalExceptionReasonCode
        End Get
        Set(ByVal value As String)
            sRenewalExceptionReasonCode = value
        End Set
    End Property
    Public Property RenewalExceptionReasonDescription() As String
        Get
            Return sRenewalExceptionReasonDescription
        End Get
        Set(ByVal value As String)
            sRenewalExceptionReasonDescription = value
        End Set
    End Property
    Public Property IsDuplicateRenewalExists() As Boolean
        Get
            Return bIsDuplicateRenewalField
        End Get
        Set(ByVal value As Boolean)
            bIsDuplicateRenewalField = value
        End Set
    End Property
End Class

