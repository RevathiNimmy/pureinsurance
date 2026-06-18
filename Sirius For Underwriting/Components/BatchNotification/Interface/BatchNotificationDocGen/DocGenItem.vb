Public NotInheritable Class DocGenItem 

#Region "Fields"
    Private m_iBatchNotificationId As Integer
    Private m_iPartyKey As Integer
    Private m_iInsuranceFileKey As Integer
    Private m_iInsuranceFolderKey As Integer
    Private m_iClaimKey As Integer
    Private m_sDocumentCode As String
#End Region

#Region "Properties"

    Public Property BatchNotificationId() As Integer
        Get
            Return m_iBatchNotificationId
        End Get
        Set(ByVal value As Integer)
            m_iBatchNotificationId = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return m_iPartyKey
        End Get
        Set(ByVal value As Integer)
            m_iPartyKey = value
        End Set
    End Property

    Public Property InsuranceFileKey() As Integer
        Get
            Return m_iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            m_iInsuranceFileKey = value
        End Set
    End Property

    Public Property InsuranceFolderKey() As Integer
        Get
            Return m_iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            m_iInsuranceFolderKey = value
        End Set
    End Property

    Public Property ClaimKey() As Integer
        Get
            Return m_iClaimKey
        End Get
        Set(ByVal value As Integer)
            m_iClaimKey = value
        End Set
    End Property

    Public Property DocumentCode() As String
        Get
            Return m_sDocumentCode
        End Get
        Set(ByVal value As String)
            m_sDocumentCode = value
        End Set
    End Property

#End Region

#Region "Creator"
    Public Sub New(ByVal iBatchNotificationId As Integer, ByVal iPartyKey As Integer, ByVal iInsuranceFileKey As Integer, _
                   ByVal iInsuranceFolderKey As Integer, ByVal iClaimKey As Integer, ByVal sDocumentCode As String)

        BatchNotificationId = iBatchNotificationId
        PartyKey = iPartyKey
        InsuranceFileKey = iInsuranceFileKey
        InsuranceFolderKey = iInsuranceFolderKey
        ClaimKey = iClaimKey
        DocumentCode = sDocumentCode

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

#End Region

End Class
