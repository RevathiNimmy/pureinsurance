<Serializable()> Public Class UserAuthority

    Private sUserCode As String
    Private sUserAuthorityValue As String
    Private iUserAuthorityOptionalValue1 As Integer

    Private oUserAuthorityOption As UserAuthorityOptionType
    Private dUserAuthorityOptionalValue2 As Double
    Private sUserAuthorityOptionalValue3 As String
    Private nUserAuthorityOptionalValue3_baseAmount As Double

    Public Property UserCode() As String
        Get
            Return sUserCode
        End Get
        Set(ByVal value As String)
            sUserCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property UserAuthorityValue() As String
        Get
            Return Me.sUserAuthorityValue
        End Get
        Set(ByVal value As String)
            Me.sUserAuthorityValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property UserAuthorityOptionalValue1() As Integer
        Get
            Return Me.iUserAuthorityOptionalValue1
        End Get
        Set(ByVal value As Integer)
            Me.iUserAuthorityOptionalValue1 = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserAuthorityOptionalValue2() As Double
        Get
            Return Me.dUserAuthorityOptionalValue2
        End Get
        Set(ByVal value As Double)
            Me.dUserAuthorityOptionalValue2 = value
        End Set
    End Property
    Public Property UserAuthorityOptionalValue3_baseAmount() As Double
        Get
            Return Me.nUserAuthorityOptionalValue3_baseAmount
        End Get
        Set(ByVal value As Double)
            Me.nUserAuthorityOptionalValue3_baseAmount = value
        End Set
    End Property

    Public Property UserAuthorityOptionalValue3() As String
        Get
            Return Me.sUserAuthorityOptionalValue3
        End Get
        Set(ByVal value As String)
            Me.sUserAuthorityOptionalValue3 = value
        End Set
    End Property

    Public Property UserAuthorityOption() As UserAuthorityOptionType
        Get
            Return Me.oUserAuthorityOption
        End Get
        Set(ByVal value As UserAuthorityOptionType)
            Me.oUserAuthorityOption = value
        End Set
    End Property


    Public Enum UserAuthorityOptionType

        HasUnrestrictedEnquiry
        HasUnrestrictedUpdate
        HasWriteOffAuthority
        HasPaymentsAuthority
        CanOverridePrePolicyDate
        CanOverridePrePolicyRate
        CanOverrideDate
        CanOverrideRate
        CanDuplicateClaimOverride
        CanOverridePostingPeriod
        CanPerformBrokerTransfer
        HasClaimPaymentsAuthority

        CanUserChangeReserves
        CanMakeLiveInvoice
        CanMakeLivePayNow
        CanMakeLiveInstalments
        HasPaynowWriteOffAuthority
        AllowAddRemoveRatingSections
        AllowEditRatingSections
        CanOverrideCollectionDate
        CanMakeLiveBankGuarantee
        AllowEditAgentCommission
        '''<remarks/>
        CanMakeLiveCashDeposit
        '''<remarks/>
        BackDatedMtaAndMtcAuthorityType
        '''<remarks/>
        IsRecommender

        '''<remarks/>
        RecommenderCurrencyKey

        '''<remarks/>
        RecommenderCurrencyAmount

        '''<remarks/>
        DisplayReinsurance

        '''<remarks/>
        DisplayClaimReinsurance

        '''<remarks/>
        IsClientManagerViewonly

        '''<remarks/>
        AllowReverseAllocation

        ''' <summary>
        ''' Edit Default Commission during NB and RN
        ''' </summary>
        ''' <remarks></remarks>
        EditDefaultCommissionNBRN

        ''' <summary>
        ''' Edit Default Commission during MTA
        ''' </summary>
        ''' <remarks></remarks>
        EditDefaultCommissionMTA

        ''' <summary>
        ''' Edit Default Commission during MTC
        ''' </summary>
        ''' <remarks></remarks>
        EditDefaultCommissionMTC

        ''' <summary>
        ''' Edit Default Commission during MTR
        ''' </summary>
        ''' <remarks></remarks>
        EditDefaultCommissionMTR

        ''' <summary>
        ''' User is allowed to edit Agent during MTA and MTC
        ''' </summary>
        ''' <remarks></remarks>
        AgentEditableDuringMTAMTC

        ''' <summary>
        ''' User is allowed to edit Agent during MTA and MTC
        ''' </summary>
        ''' <remarks></remarks>
        AllowReverseReceipt
        'wpr10
        CanChangeInstalmentDefaultCurrency = 182

        '''<remarks/>
        CanUpdateInstalmentStatus

        '''<remarks/>
        CanUpdateInstalmentDueDate

        '''<remarks/>
        EditInstalmentNoOfDays
        
        '''<summary>
        '''User is allowed to authorise manual journal Trans
        HasManualJournalAuthority

        '''<summary>
        '''User can extract client data (GDPR SAR)
        CanExtractClientData
    End Enum


End Class
