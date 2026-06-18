<Serializable()> Public Class MTA

    Private sBranchCode As String
    Private bCanBeAddedToPfPlan As Boolean
    Private dtDateCreated As DateTime
    Private iInsuranceFileKey As Integer
    Private bIsMtaQuoteValid As Boolean
    Private sMtaDescription As String
    Private sMtaType As String
    Private bQuoteTimeStamp As Byte()


    Private iAccountHandlerCnt As Integer

    Private sTypeOfMta As String

    Private dEffectiveDate As Date

    Private dExpiryDate As Date

    Private sInsuredName As String

    Private sPolicyKey As String

    Private sRegarding As String

    Private sAlternateReference As String

    Private sPolicyStatusCode As String

    Private sAnalysisCode As String

    Private sBusinessTypeCode As String

    Private dIssueDate As Date

    Private dProposalDate As Date

    Private sFrequencyCode As String

    Private dLTUExpiryDate As Date

    Private sStopReasonCode As String

    Private sRenewalMethodCode As String

    Private sLapseCancelReasonCode As String

    Private dLapseCancelDate As Date

    Private bReferredAtRenewal As Boolean

    Private bReferredOnMTA As Boolean

    Private sPolicyStyleCode As String

    Private bIsReinstatement As Boolean
    Private sTranactionType As String
    Private iRenewalDayNo As Integer
    Private iInsuranceFolderKey As Integer
    Private iPartyKey As Integer
    Private bIsInteractive As Boolean
    Private CorrespondenceTypeField As String
    Private DefaultPreferredCorrespondenceField As String

    Private dtAnniversaryDate As Date
    Private bIsAnniversaryCopy As Boolean
#Region "Sachin"
    Private sMtaReason As String
    Private dtMtaEffectiveDate As DateTime
    Private dtMtaExpiryDate As DateTime
    Private dtQuoteExpiryDate As DateTime
    Private iRiskKey As Integer
    Private sXmlDataset As String
#End Region


    Public Sub New()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
        sbPrint.AppendLine("Can Be Added to PF Plan : " & IIf(bCanBeAddedToPfPlan, "true", "false") & "<br />")
        sbPrint.AppendLine("Date Created : " & dtDateCreated.ToString() & "<br />")
        sbPrint.AppendLine("Insurance File Key : " & iInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("Is MTA Quote Valid : " & IIf(bIsMtaQuoteValid, "true", "false") & "<br />")
        sbPrint.AppendLine("MTA Description : " & sMtaDescription & "<br />")
        sbPrint.AppendLine("MTA Type : " & sMtaType & "<br />")
        sbPrint.AppendLine("Renewal Day No : " & iRenewalDayNo.ToString() & "<br />")
        sbPrint.Append("Quote TimeStamp : ")

        If bQuoteTimeStamp IsNot Nothing Then

            For Each oByte As Byte In bQuoteTimeStamp
                sbPrint.Append(oByte.ToString & " | ")
            Next

        End If

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
    Public Property TranactionType() As String
        Get
            Return sTranactionType
        End Get
        Set(ByVal value As String)
            sTranactionType = value
        End Set
    End Property
    Public Property BranchCode() As String
        Get
            Return sBranchCode
        End Get
        Set(ByVal value As String)
            sBranchCode = value
        End Set
    End Property

    Public Property CanBeAddedToPFPlan() As Boolean
        Get
            Return bCanBeAddedToPfPlan
        End Get
        Set(ByVal value As Boolean)
            bCanBeAddedToPfPlan = value
        End Set
    End Property

    Public Property DateCreated() As DateTime
        Get
            Return dtDateCreated
        End Get
        Set(ByVal value As DateTime)
            dtDateCreated = value
        End Set
    End Property



    Public Property IsMTAQuoteValid() As Boolean
        Get
            Return bIsMtaQuoteValid
        End Get
        Set(ByVal value As Boolean)
            bIsMtaQuoteValid = value
        End Set
    End Property

    Public Property MTADescription() As String
        Get
            Return sMtaDescription
        End Get
        Set(ByVal value As String)
            sMtaDescription = value
        End Set
    End Property

    Public Property MTAType() As String
        Get
            Return sMtaType
        End Get
        Set(ByVal value As String)
            sMtaType = value
        End Set
    End Property

    Public Property QuoteTimeStamp() As Byte()
        Get
            Return bQuoteTimeStamp
        End Get
        Set(ByVal value As Byte())
            bQuoteTimeStamp = value
        End Set
    End Property



    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountHandlerCnt() As Integer
        Get
            Return Me.iAccountHandlerCnt
        End Get
        Set(ByVal value As Integer)
            Me.iAccountHandlerCnt = value
        End Set
    End Property


    '''<remarks/>
    Public Property TypeOfMta() As String
        Get
            Return Me.sTypeOfMta
        End Get
        Set(ByVal value As String)
            Me.sTypeOfMta = value
        End Set
    End Property


    '''<remarks/>
    Public Property EffectiveDate() As Date
        Get
            Return Me.dEffectiveDate
        End Get
        Set(ByVal value As Date)
            Me.dEffectiveDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExpiryDate() As Date
        Get
            Return Me.dExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dExpiryDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredName() As String
        Get
            Return Me.sInsuredName
        End Get
        Set(ByVal value As String)
            Me.sInsuredName = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyKey() As String
        Get
            Return Me.sPolicyKey
        End Get
        Set(ByVal value As String)
            Me.sPolicyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Regarding() As String
        Get
            Return Me.sRegarding
        End Get
        Set(ByVal value As String)
            Me.sRegarding = value
        End Set
    End Property

    '''<remarks/>
    Public Property AlternateReference() As String
        Get
            Return Me.sAlternateReference
        End Get
        Set(ByVal value As String)
            Me.sAlternateReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyStatusCode() As String
        Get
            Return Me.sPolicyStatusCode
        End Get
        Set(ByVal value As String)
            Me.sPolicyStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property AnalysisCode() As String
        Get
            Return Me.sAnalysisCode
        End Get
        Set(ByVal value As String)
            Me.sAnalysisCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property BusinessTypeCode() As String
        Get
            Return Me.sBusinessTypeCode
        End Get
        Set(ByVal value As String)
            Me.sBusinessTypeCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property IssueDate() As Date
        Get
            Return Me.dIssueDate
        End Get
        Set(ByVal value As Date)
            Me.dIssueDate = value
        End Set
    End Property


    '''<remarks/>
    Public Property ProposalDate() As Date
        Get
            Return Me.dProposalDate
        End Get
        Set(ByVal value As Date)
            Me.dProposalDate = value
        End Set
    End Property



    '''<remarks/>
    Public Property FrequencyCode() As String
        Get
            Return Me.sFrequencyCode
        End Get
        Set(ByVal value As String)
            Me.sFrequencyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property LTUExpiryDate() As Date
        Get
            Return Me.dLTUExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dLTUExpiryDate = value
        End Set
    End Property



    '''<remarks/>
    Public Property StopReasonCode() As String
        Get
            Return Me.sStopReasonCode
        End Get
        Set(ByVal value As String)
            Me.sStopReasonCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property RenewalMethodCode() As String
        Get
            Return Me.sRenewalMethodCode
        End Get
        Set(ByVal value As String)
            Me.sRenewalMethodCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property LapseCancelReasonCode() As String
        Get
            Return Me.sLapseCancelReasonCode
        End Get
        Set(ByVal value As String)
            Me.sLapseCancelReasonCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property LapseCancelDate() As Date
        Get
            Return Me.dLapseCancelDate
        End Get
        Set(ByVal value As Date)
            Me.dLapseCancelDate = value
        End Set
    End Property



    '''<remarks/>
    Public Property ReferredAtRenewal() As Boolean
        Get
            Return Me.bReferredAtRenewal
        End Get
        Set(ByVal value As Boolean)
            Me.bReferredAtRenewal = value
        End Set
    End Property


    '''<remarks/>
    Public Property ReferredOnMTA() As Boolean
        Get
            Return Me.bReferredOnMTA
        End Get
        Set(ByVal value As Boolean)
            Me.bReferredOnMTA = value
        End Set
    End Property



    '''<remarks/>
    Public Property PolicyStyleCode() As String
        Get
            Return Me.sPolicyStyleCode
        End Get
        Set(ByVal value As String)
            Me.sPolicyStyleCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsReinstatement() As Boolean
        Get
            Return Me.bIsReinstatement
        End Get
        Set(ByVal value As Boolean)
            Me.bIsReinstatement = value
        End Set
    End Property
    ''' <summary>
    ''' RenewalDayNo for selected MTA version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RenewalDayNo() As Integer
        Get
            Return iRenewalDayNo
        End Get
        Set(ByVal value As Integer)
            iRenewalDayNo = value
        End Set
    End Property


    ''' <summary>
    ''' Party key for selected quote/policy version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    ''' <summary>
    ''' Insurance Folder Key for the selected quote
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Property InsuranceFolderKey() As Integer
        Get
            Return Me.iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFolderKey = value
        End Set
    End Property

    Public Property IsInteractive() As Boolean
        Get
            Return Me.bIsInteractive
        End Get
        Set(ByVal value As Boolean)
            Me.bIsInteractive = value
        End Set
    End Property

    Public Property CorrespondenceType As String
        Get
            Return Me.CorrespondenceTypeField
        End Get
        Set(value As String)
            Me.CorrespondenceTypeField = value
        End Set
    End Property

    Public Property DefaultPreferredCorrespondence As String
        Get
            Return Me.DefaultPreferredCorrespondenceField
        End Get
        Set(value As String)
            Me.DefaultPreferredCorrespondenceField = value
        End Set
    End Property

    Public Property IsAgentReceiveCorrespondence As Boolean = False
    ''' <summary>
    ''' Anniversary Date property used true monthly Policy.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AnniversaryDate() As Date
        Get
            Return dtAnniversaryDate
        End Get
        Set(ByVal value As Date)
            dtAnniversaryDate = value
        End Set
    End Property

    Public Property CoInsurancePlacement() As String

#Region "Sachin"

    Public Property MtaReason() As String
        Get
            Return sMtaReason
        End Get
        Set(ByVal value As String)
            sMtaReason = value
        End Set
    End Property

    Public Property MtaEffectiveDate() As DateTime
        Get
            Return dtMtaEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            dtMtaEffectiveDate = value
        End Set
    End Property

    Public Property MtaExpiryDate() As DateTime
        Get
            Return dtMtaExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtMtaExpiryDate = value
        End Set
    End Property

    Public Property QuoteExpiryDate() As DateTime
        Get
            Return dtQuoteExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtQuoteExpiryDate = value
        End Set
    End Property

    Public Property RiskKey() As Integer
        Get
            Return iRiskKey
        End Get
        Set(ByVal value As Integer)
            iRiskKey = value
        End Set
    End Property

    Public Property XmlDataset() As String
        Get
            Return sXmlDataset
        End Get
        Set(ByVal value As String)
            sXmlDataset = value

        End Set
    End Property
#End Region

    Private oldPolicyNumberField As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OldPolicyNumber() As String
        Get
            Return Me.oldPolicyNumberField
        End Get
        Set(ByVal value As String)
            Me.oldPolicyNumberField = value
        End Set
    End Property

End Class

<Serializable()> Public Class MTACollection : Inherits CollectionBase

    Public Function Add(ByVal v_oMTA As MTA) As Integer
        Return List.Add(v_oMTA)
    End Function

    Public Sub Remove(ByVal v_oMTA As MTA)
        List.Remove(v_oMTA)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As MTA
        Get
            Return List(i)
        End Get
        Set(ByVal value As MTA)
            List(i) = value
        End Set
    End Property

End Class
