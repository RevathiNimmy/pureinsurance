Public Class ClosedClaim

    Private m_iClaimID As Integer
    Public Property ClaimID() As Integer
        Get
            Return m_iClaimID
        End Get
        Set(ByVal Value As Integer)
            m_iClaimID = Value
        End Set
    End Property

    Private m_sClaimNumber As String
    Public Property ClaimNumber() As String
        Get
            Return m_sClaimNumber
        End Get
        Set(ByVal Value As String)
            m_sClaimNumber = Value
        End Set
    End Property

    Private m_sInsured As String
    Public Property Insured() As String
        Get
            Return m_sInsured
        End Get
        Set(ByVal Value As String)
            m_sInsured = Value
        End Set
    End Property

    Private m_iPolicyID As Integer
    Public Property PolicyID() As Integer
        Get
            Return m_iPolicyID
        End Get
        Set(ByVal Value As Integer)
            m_iPolicyID = Value
        End Set
    End Property

    Private m_sPolicyNumber As String
    Public Property PolicyNumber() As String
        Get
            Return m_sPolicyNumber
        End Get
        Set(ByVal Value As String)
            m_sPolicyNumber = Value
        End Set
    End Property

    Private m_sDateofLoss As String
    Public Property DateofLoss() As String
        Get
            Return m_sDateofLoss
        End Get
        Set(ByVal Value As String)
            m_sDateofLoss = Value
        End Set
    End Property

    Private m_sClassofBusiness As String
    Public Property ClassofBusiness() As String
        Get
            Return m_sClassofBusiness
        End Get
        Set(ByVal Value As String)
            m_sClassofBusiness = Value
        End Set
    End Property

    Private m_sClaimStatus As String
    Public Property ClaimStatus() As String
        Get
            Return m_sClaimStatus
        End Get
        Set(ByVal Value As String)
            m_sClaimStatus = Value
        End Set
    End Property

    Private m_sClaimProgressStatus As String
    Public Property ClaimProgressStatus() As String
        Get
            Return m_sClaimProgressStatus
        End Get
        Set(ByVal Value As String)
            m_sClaimProgressStatus = Value
        End Set
    End Property

    Private m_dOutstandingAmount As Double
    Public Property OutstandingAmount() As Double
        Get
            Return m_dOutstandingAmount
        End Get
        Set(ByVal Value As Double)
            m_dOutstandingAmount = Value
        End Set
    End Property

End Class
