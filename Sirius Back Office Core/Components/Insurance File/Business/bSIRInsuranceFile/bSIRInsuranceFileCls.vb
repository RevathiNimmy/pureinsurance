Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'Developer Guide No. 129
Friend NotInheritable Class SIRInsuranceFile

    Implements IDisposable
#Region "Private Constants"
    Private Const ACClass As String = "SIRInsuranceFile"
    Private Const m_lDefaultPartyCnt As Integer = 2
#End Region

#Region "Private Variables"
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRInsuranceFile As dSIRInsuranceFile.SIRInsuranceFile

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer

    ' Primary Folder Key to work with
    Private m_lInsuranceFolderCnt As Integer

    'DN 16/08/01
    Private m_lTransInsuranceFolderCnt As Integer

    Private m_bEvent As Boolean

    Private m_bEventRaised As Boolean




    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer

    Private m_oCollectionFrequencyID As Object
    Private m_oPaymentTermsID As Object
    ' ************************************************
    ' PRIVATE Data Members (End)



#Region "Public Properties"



    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value

        End Set
    End Property

    'DN 16/08/01
    'DN 16/08/01
    Public Property TransInsuranceFolderCnt() As Integer
        Get

            Return m_lTransInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lTransInsuranceFolderCnt = Value

        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    Public Property EventRaised() As Boolean
        Get

            Return m_bEventRaised

        End Get
        Set(ByVal Value As Boolean)

            m_bEventRaised = Value

        End Set
    End Property
#End Region

    Public Property CollectionFrequencyID() As Object
        Get

            Return m_oCollectionFrequencyID

        End Get
        Set(ByVal Value As Object)

            m_oCollectionFrequencyID = Value

        End Set
    End Property

    Public Property PaymentTermsID() As Object
        Get

            Return m_oPaymentTermsID

        End Get
        Set(ByVal Value As Object)

            m_oPaymentTermsID = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)
#Region "Public Methods"

#End Region
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of data class
            m_dSIRInsuranceFile = New dSIRInsuranceFile.SIRInsuranceFile()

            m_lReturn = m_dSIRInsuranceFile.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_dSIRInsuranceFile IsNot Nothing Then
                    m_dSIRInsuranceFile.Dispose()
                End If
                m_dSIRInsuranceFile = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRInsuranceFile.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(ByRef r_vFieldArray() As Object) As Integer
        Return GetDefaults(r_vFieldArray:=r_vFieldArray, v_vSubType:=Nothing)
    End Function

    Public Function GetDefaults(ByRef r_vFieldArray() As Object, ByVal v_vSubType As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            m_lReturn = CType(DefaultParameters(v_bDefaultAll:=True, r_vFieldArray:=r_vFieldArray, v_vSubType:=v_vSubType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' SetProperties
    ''' </summary>
    ''' <param name="v_iStatus"></param>
    ''' <param name="v_vFieldArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProperties(ByVal v_iStatus As Integer, ByVal v_vFieldArray() As Object) As Integer
        Return SetProperties(v_iStatus:=v_iStatus, v_vFieldArray:=v_vFieldArray, v_vSubType:=Nothing)
    End Function

    ''' <summary>
    ''' SetProperties
    ''' </summary>
    ''' <param name="v_iStatus"></param>
    ''' <param name="v_vFieldArray"></param>
    ''' <param name="v_vSubType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProperties(ByVal v_iStatus As Integer, ByVal v_vFieldArray() As Object, ByVal v_vSubType As Object) As Integer

        Dim nResult As Integer = 0
        Dim bDataChanged As Boolean

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If v_iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = CType(DefaultParameters(v_bDefaultAll:=False, r_vFieldArray:=v_vFieldArray,
                                                    v_vSubType:=v_vSubType),
                                    gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(v_vFieldArray:=v_vFieldArray, v_vSubType:=v_vSubType),
                                gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn


            End If
            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRInsuranceFile
                ' Set Property values.
                bDataChanged = DataDifferent(.InsuranceFileCnt, v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt)) Or
                                DataDifferent(.InsuranceFileStructureID,
                                                v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)) Or
                                DataDifferent(.InsuranceFileTypeID,
                                                v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)) Or
                                DataDifferent(.InsuranceFileStatusID,
                                                v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)) Or
                                DataDifferent(.InsuranceFileID, v_vFieldArray(InsuranceFileConst.ACInsuranceFileID)) Or
                                DataDifferent(.SourceID, v_vFieldArray(InsuranceFileConst.ACSourceID)) Or
                                DataDifferent(.InsuranceFolderCnt, v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt)) Or
                                DataDifferent(.InsuranceRef, v_vFieldArray(InsuranceFileConst.ACInsuranceRef)) Or
                                DataDifferent(.ProductID, v_vFieldArray(InsuranceFileConst.ACProductID)) Or
                                DataDifferent(.LeadInsurerCnt, v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt))
                'sj 19/07/2002 changed BranchId to SubBranchId

                'sj 19/07/2002 changed BranchId to SubBranchId
                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.LeadAgentCnt, v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)) Or
                                    DataDifferent(.LeadAgentPercent, v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)) Or
                                    DataDifferent(.AccountHandlerCnt,
                                                    v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)) Or
                                    DataDifferent(.InsuredCnt, v_vFieldArray(InsuranceFileConst.ACInsuredCnt)) Or
                                    DataDifferent(.BusinessTypeID, v_vFieldArray(InsuranceFileConst.ACBusinessTypeID)) Or
                                    DataDifferent(.CollectTypeID, v_vFieldArray(InsuranceFileConst.ACCollectTypeID)) Or
                                    DataDifferent(.CollectionFromCnt,
                                                    v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)) Or
                                    DataDifferent(.SubBranchId, v_vFieldArray(InsuranceFileConst.ACSubBranchID)) Or
                                    DataDifferent(.CurrencyID, v_vFieldArray(InsuranceFileConst.ACCurrencyID)) Or
                                    DataDifferent(.LanguageID, v_vFieldArray(InsuranceFileConst.ACLanguageID))
                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.DateIssued, v_vFieldArray(InsuranceFileConst.ACDateIssued)) Or
                                    DataDifferent(.CoverStartDate, v_vFieldArray(InsuranceFileConst.ACCoverStartDate)) Or
                                    DataDifferent(.ExpiryDate, v_vFieldArray(InsuranceFileConst.ACExpiryDate)) Or
                                    DataDifferent(.RenewalDate, v_vFieldArray(InsuranceFileConst.ACRenewalDate)) Or
                                    DataDifferent(.RenewalMethodID, v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)) Or
                                    DataDifferent(.RenewalFrequencyID,
                                                    v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)) Or
                                    DataDifferent(.IsReferredAtRenewal,
                                                    v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal)) Or
                                    DataDifferent(.LapsedReasonID, v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)) Or
                                    DataDifferent(.LapsedDate, v_vFieldArray(InsuranceFileConst.ACLapsedDate)) Or
                                    DataDifferent(.LapsedDescription,
                                                    v_vFieldArray(InsuranceFileConst.ACLapsedDescription))

                End If

                If Not bDataChanged Then
                    bDataChanged =
                        DataDifferent(.IsReferredOnMta, v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta)) Or
                        DataDifferent(.PolicyVersion, v_vFieldArray(InsuranceFileConst.ACPolicyVersion)) Or
                        DataDifferent(.GeminiPolicyStatus, v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)) Or
                        DataDifferent(.GeminiBusinessType, v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)) Or
                        DataDifferent(.DeferredInd, v_vFieldArray(InsuranceFileConst.ACDeferredInd)) Or
                        DataDifferent(.PolicyIgnore, v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)) Or
                        DataDifferent(.BrokerCnt, v_vFieldArray(InsuranceFileConst.ACBrokerCnt)) Or
                        DataDifferent(.RiskCodeID, v_vFieldArray(InsuranceFileConst.ACRiskCodeId)) Or
                        DataDifferent(.AnalysisCodeID, v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)) Or
                        DataDifferent(.PolicyDeductiblesID, v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)) Or
                        DataDifferent(.PolicyLimitsID, v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)) Or
                        DataDifferent(.ProposalDate, v_vFieldArray(InsuranceFileConst.ACProposalDate))

                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.DiaryDate, v_vFieldArray(InsuranceFileConst.ACDiaryDate)) Or
                                    DataDifferent(.ReviewDate, v_vFieldArray(InsuranceFileConst.ACReviewDate)) Or
                                    DataDifferent(.RenewalDayNumber, v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)) Or
                                    DataDifferent(.PolicyTypeID, v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)) Or
                                    DataDifferent(.Indicator, v_vFieldArray(InsuranceFileConst.ACIndicator)) Or
                                    DataDifferent(.Clause, v_vFieldArray(InsuranceFileConst.ACClause)) Or
                                    DataDifferent(.Cover, v_vFieldArray(InsuranceFileConst.ACCover)) Or
                                    DataDifferent(.Area, v_vFieldArray(InsuranceFileConst.ACArea)) Or
                                    DataDifferent(.LongTermUndertakingDate,
                                                    v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)) Or
                                    DataDifferent(.RenewalStopCodeID,
                                                    v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID))

                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.VBSType, v_vFieldArray(InsuranceFileConst.ACVBSType)) Or
                                    DataDifferent(.VBSStatus, v_vFieldArray(InsuranceFileConst.ACVBSStatus)) Or
                                    DataDifferent(.IsInsurerRateTable,
                                                    v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)) Or
                                    DataDifferent(.IsRelatedPolicies,
                                                    v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)) Or
                                    DataDifferent(.IsRetainedDocuments,
                                                    v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)) Or
                                    DataDifferent(.SchemesPostcode, v_vFieldArray(InsuranceFileConst.ACSchemesPostcode)) Or
                                    DataDifferent(.PaidDirect, v_vFieldArray(InsuranceFileConst.ACPaidDirect)) Or
                                    DataDifferent(.Scheme, v_vFieldArray(InsuranceFileConst.ACScheme)) Or
                                    DataDifferent(.BrokerageAmount, v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)) Or
                                    DataDifferent(.IsMinimumBrokerageFlag,
                                                    v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag))

                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.AnnualPremium, v_vFieldArray(InsuranceFileConst.ACAnnualPremium)) Or
                                    DataDifferent(.ThisPremium, v_vFieldArray(InsuranceFileConst.ACThisPremium)) Or
                                    DataDifferent(.NetPremium, v_vFieldArray(InsuranceFileConst.ACNetPremium)) Or
                                    DataDifferent(.CommissionAmount, v_vFieldArray(InsuranceFileConst.ACCommissionAmount)) Or
                                    DataDifferent(.IPTAbleAmount, v_vFieldArray(InsuranceFileConst.ACIPTableAmount)) Or
                                    DataDifferent(.IPTPercentage, v_vFieldArray(InsuranceFileConst.ACIPTPercentage)) Or
                                    DataDifferent(.ISIPTOverridden, v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)) Or
                                    DataDifferent(.TaxAmount, v_vFieldArray(InsuranceFileConst.ACTaxAmount)) Or
                                    DataDifferent(.VatableAmount, v_vFieldArray(InsuranceFileConst.ACVatableAmount)) Or
                                    DataDifferent(.VATPercentage, v_vFieldArray(InsuranceFileConst.ACVatPercentage))

                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.VATAmount, v_vFieldArray(InsuranceFileConst.ACVatAmount)) Or
                                    DataDifferent(.PaymentMethod, v_vFieldArray(InsuranceFileConst.ACPaymentMethod)) Or
                                    DataDifferent(.UserDefinedDataID,
                                                    v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)) Or
                                    DataDifferent(.CommissionPercentage,
                                                    v_vFieldArray(InsuranceFileConst.ACCommissionPercentage)) Or
                                    DataDifferent(.InvariantKey, v_vFieldArray(InsuranceFileConst.ACInvariantKey))
                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.InsuredName, v_vFieldArray(InsuranceFileConst.ACInsuredName)) Or
                                    DataDifferent(.AlternateReference,
                                                    v_vFieldArray(InsuranceFileConst.ACAlternateReference)) Or
                                    DataDifferent(.IsClientInvoiced, v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)) Or
                                    DataDifferent(.OldPolicyNumber, v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)) Or
                                    DataDifferent(.QuoteExpiryDate, v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)) Or
                                    DataDifferent(.AlternateAccountCnt,
                                                    v_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt))
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACAccountExecutiveCnt Then
                        bDataChanged = DataDifferent(.AccountExecutiveCnt,
                                                        v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACAnniversaryDate Then
                        bDataChanged = DataDifferent(.AnniversaryDate,
                                                        v_vFieldArray(InsuranceFileConst.ACAnniversaryDate))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACPolicyStyleID Then
                        bDataChanged = DataDifferent(.PolicyStyleID, v_vFieldArray(InsuranceFileConst.ACPolicyStyleID))
                    End If
                End If

                If Not bDataChanged Then

                    If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACTransDescription), Nothing) Then
                        bDataChanged = True
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACUnderwritingYearID Then
                        bDataChanged = DataDifferent(.UnderwritingYearID,
                                                        v_vFieldArray(InsuranceFileConst.ACUnderwritingYearID))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACPolicyStatusID Then
                        bDataChanged = DataDifferent(.PolicyStatusID, v_vFieldArray(InsuranceFileConst.ACPolicyStatusID))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACInceptionTPI Then
                        bDataChanged = DataDifferent(.InceptionTPI, v_vFieldArray(InsuranceFileConst.ACInceptionTPI))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSACustomerCategoryID Then
                        bDataChanged = DataDifferent(.FSACustomerCategoryID,
                                                        v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID))
                    End If
                End If
                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSAContractLocationID Then
                        bDataChanged = DataDifferent(.FSAContractLocationID,
                                                        v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID))
                    End If
                End If
                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSAUnderwriterCnt Then
                        bDataChanged = DataDifferent(.FSAUnderwriterCnt,
                                                        v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt))
                    End If
                End If
                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSATypeOfSaleID Then
                        bDataChanged = DataDifferent(.FSATypeOfSaleID,
                                                        v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID))
                    End If
                End If
                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSARenewalConsent Then
                        bDataChanged = DataDifferent(.FSARenewalConsent,
                                                        v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountReasonID Then
                        bDataChanged = DataDifferent(.DiscountReasonID,
                                                        v_vFieldArray(InsuranceFileConst.ACDiscountReasonID))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountedPremium Then
                        bDataChanged = DataDifferent(.DiscountedPremium,
                                                        v_vFieldArray(InsuranceFileConst.ACDiscountedPremium))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountPercentage Then
                        bDataChanged = DataDifferent(.DiscountPercentage,
                                                        v_vFieldArray(InsuranceFileConst.ACDiscountPercentage))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACMatchDiscountedPremiumFlag Then
                        bDataChanged = DataDifferent(.MatchDiscountedPremiumFlag,
                                                        v_vFieldArray(InsuranceFileConst.ACMatchDiscountedPremiumFlag))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal _
                        Then
                        bDataChanged = DataDifferent(.PutOnNextInstalmentRenewal,
                                                        v_vFieldArray(
                                                            InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACInsuranceFileAnniversaryCopy Then
                        bDataChanged = DataDifferent(.AnniversaryCopy,
                                                        v_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountRecurringTypeId Then
                        bDataChanged = DataDifferent(.DiscountRecurringTypeId,
                                                        v_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId))
                    End If
                End If

                If Not bDataChanged Then
                    bDataChanged = DataDifferent(.CCTermsAgreed, v_vFieldArray(InsuranceFileConst.ACCCTermsAgreed)) Or
                                    DataDifferent(.CCTermsAgreedDate,
                                                    v_vFieldArray(InsuranceFileConst.ACCCTermsAgreedDate)) Or
                                    DataDifferent(.CCInceptionDate, v_vFieldArray(InsuranceFileConst.ACCCInceptionDate)) Or
                                    DataDifferent(.CCPolicyDocumentsIssuedDate,
                                                    v_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentsIssuedDate)) Or
                                    DataDifferent(.CCPolicyDocumentCorrect,
                                                    v_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentCorrect)) Or
                                    DataDifferent(.CCErrorNotificationDate,
                                                    v_vFieldArray(InsuranceFileConst.ACCCErrorNotificationDate))
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACLeadAllowConsolidatedCommission Then
                        bDataChanged = DataDifferent(.ConsolidatedLeadCommission,
                                                        v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACSubAllowConsolidatedCommission Then
                        bDataChanged = DataDifferent(.ConsolidatedSubCommission,
                                                        v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSARiskTransferAgreement Then
                        bDataChanged = DataDifferent(.RiskTransferAgreement,
                                                        v_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACManualDiscountPercentage Then
                        bDataChanged = DataDifferent(.ManualDiscountPercentage,
                                                        v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACQuoteStatusID Then
                        bDataChanged = DataDifferent(.QuoteStatusID, v_vFieldArray(InsuranceFileConst.ACQuoteStatusID))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACQuoteVersionID Then
                        bDataChanged = DataDifferent(.QuoteVersionID, v_vFieldArray(InsuranceFileConst.ACQuoteVersionID))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACBaseInsuranceFolderCnt Then
                        bDataChanged = DataDifferent(.BaseInsuranceFolderCnt,
                                                        v_vFieldArray(InsuranceFileConst.ACBaseInsuranceFolderCnt))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACContactuserId Then
                        bDataChanged = DataDifferent(.ContactuserId, v_vFieldArray(InsuranceFileConst.ACContactuserId))
                    End If
                End If
                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACMTAReasonId Then
                        bDataChanged = DataDifferent(.MTAReasonId, v_vFieldArray(InsuranceFileConst.ACMTAReasonId))
                    End If
                End If
                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACIsMarketPlacePolicy Then
                        bDataChanged = DataDifferent(.IsMarketPlacePolicy, v_vFieldArray(InsuranceFileConst.ACIsMarketPlacePolicy))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.kCollectionFrequencyId Then
                        bDataChanged = DataDifferent(.CollectionFrequencyID, v_vFieldArray(InsuranceFileConst.kCollectionFrequencyId))
                    End If
                End If

                If Not bDataChanged Then
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.kPaymentTermId Then
                        bDataChanged = DataDifferent(.PaymentTermsID, v_vFieldArray(InsuranceFileConst.kPaymentTermId))
                    End If
                End If
                If bDataChanged Then

                    .InsuranceFileCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt))

                    .InsuranceFileStructureID = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID))

                    .InsuranceFileTypeID = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID))

                    If v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID) Is DBNull.Value Then
                        v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID) = 0
                    End If

                    .InsuranceFileStatusID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)

                    .InsuranceFileID = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileID))

                    .SourceID = CInt(v_vFieldArray(InsuranceFileConst.ACSourceID))

                    .InsuranceFolderCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt))

                    .InsuranceRef = CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceRef))

                    .ProductID = CInt(v_vFieldArray(InsuranceFileConst.ACProductID))

                    .LeadInsurerCnt = v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)

                    .LeadAgentCnt = v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)

                    .LeadAgentPercent = v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)

                    .AccountHandlerCnt = v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)

                    .InsuredCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuredCnt))

                    .BusinessTypeID = CInt(v_vFieldArray(InsuranceFileConst.ACBusinessTypeID))

                    If v_vFieldArray(InsuranceFileConst.ACCollectTypeID) Is DBNull.Value Then
                        v_vFieldArray(InsuranceFileConst.ACCollectTypeID) = 0
                    End If
                    If v_vFieldArray(InsuranceFileConst.ACCollectTypeID) Is DBNull.Value Then
                        v_vFieldArray(InsuranceFileConst.ACCollectTypeID) = 0
                    End If

                    .CollectTypeID = v_vFieldArray(InsuranceFileConst.ACCollectTypeID)

                    .CollectionFromCnt = v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)

                    .SubBranchId = CInt(v_vFieldArray(InsuranceFileConst.ACSubBranchID))

                    .CurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACCurrencyID))

                    .BaseCurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACBaseCurrencyID))

                    .LanguageID = CInt(v_vFieldArray(InsuranceFileConst.ACLanguageID))

                    .DateIssued = v_vFieldArray(InsuranceFileConst.ACDateIssued)

                    .CoverStartDate = CDate(v_vFieldArray(InsuranceFileConst.ACCoverStartDate))

                    .ExpiryDate = CDate(v_vFieldArray(InsuranceFileConst.ACExpiryDate))

                    .RenewalDate = CDate(v_vFieldArray(InsuranceFileConst.ACRenewalDate))

                    If v_vFieldArray(InsuranceFileConst.ACRenewalMethodID) Is DBNull.Value Then
                        v_vFieldArray(InsuranceFileConst.ACRenewalMethodID) = 0
                    End If

                    .RenewalMethodID = v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)

                    .RenewalFrequencyID = CInt(v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID))

                    .IsReferredAtRenewal = CInt(v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal))

                    If v_vFieldArray(InsuranceFileConst.ACLapsedReasonID) Is DBNull.Value Then
                        v_vFieldArray(InsuranceFileConst.ACLapsedReasonID) = 0
                    End If

                    .LapsedReasonID = v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)

                    .LapsedDate = v_vFieldArray(InsuranceFileConst.ACLapsedDate)

                    .LapsedDescription = v_vFieldArray(InsuranceFileConst.ACLapsedDescription)

                    .IsReferredOnMta = CInt(v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta))

                    .PolicyVersion = CInt(v_vFieldArray(InsuranceFileConst.ACPolicyVersion))

                    .GeminiPolicyStatus = v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)

                    .GeminiBusinessType = v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)

                    .DeferredInd = v_vFieldArray(InsuranceFileConst.ACDeferredInd)

                    .PolicyIgnore = v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)

                    .BrokerCnt = v_vFieldArray(InsuranceFileConst.ACBrokerCnt)

                    .RiskCodeID = v_vFieldArray(InsuranceFileConst.ACRiskCodeId)

                    .AnalysisCodeID = v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)

                    .PolicyDeductiblesID = v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)

                    .PolicyLimitsID = v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)

                    .ProposalDate = v_vFieldArray(InsuranceFileConst.ACProposalDate)

                    .DiaryDate = v_vFieldArray(InsuranceFileConst.ACDiaryDate)

                    .ReviewDate = v_vFieldArray(InsuranceFileConst.ACReviewDate)

                    .RenewalDayNumber = v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)

                    .PolicyTypeID = v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)

                    .Indicator = v_vFieldArray(InsuranceFileConst.ACIndicator)

                    .Clause = v_vFieldArray(InsuranceFileConst.ACClause)

                    .Cover = v_vFieldArray(InsuranceFileConst.ACCover)

                    .Area = v_vFieldArray(InsuranceFileConst.ACArea)

                    .LongTermUndertakingDate = v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)

                    .RenewalStopCodeID = v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)

                    .VBSType = v_vFieldArray(InsuranceFileConst.ACVBSType)

                    .VBSStatus = v_vFieldArray(InsuranceFileConst.ACVBSStatus)

                    .IsInsurerRateTable = v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)
                    .IsRelatedPolicies = v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)

                    .IsRetainedDocuments = v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)

                    .SchemesPostcode = v_vFieldArray(InsuranceFileConst.ACSchemesPostcode)

                    .PaidDirect = v_vFieldArray(InsuranceFileConst.ACPaidDirect)

                    .Scheme = v_vFieldArray(InsuranceFileConst.ACScheme)

                    .BrokerageAmount = v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)

                    .IsMinimumBrokerageFlag = v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag)

                    .AnnualPremium = v_vFieldArray(InsuranceFileConst.ACAnnualPremium)

                    .ThisPremium = v_vFieldArray(InsuranceFileConst.ACThisPremium)

                    .NetPremium = v_vFieldArray(InsuranceFileConst.ACNetPremium)

                    .CommissionAmount = v_vFieldArray(InsuranceFileConst.ACCommissionAmount)

                    .IPTAbleAmount = v_vFieldArray(InsuranceFileConst.ACIPTableAmount)

                    .IPTPercentage = v_vFieldArray(InsuranceFileConst.ACIPTPercentage)

                    .ISIPTOverridden = v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)

                    .TaxAmount = v_vFieldArray(InsuranceFileConst.ACTaxAmount)

                    .VatableAmount = v_vFieldArray(InsuranceFileConst.ACVatableAmount)

                    .VATPercentage = v_vFieldArray(InsuranceFileConst.ACVatPercentage)

                    .VATAmount = v_vFieldArray(InsuranceFileConst.ACVatAmount)

                    .PaymentMethod = v_vFieldArray(InsuranceFileConst.ACPaymentMethod)
                    .UserDefinedDataID = v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)
                    .CommissionPercentage = v_vFieldArray(InsuranceFileConst.ACCommissionPercentage)
                    .InvariantKey = v_vFieldArray(InsuranceFileConst.ACInvariantKey)

                    .InsuredName = v_vFieldArray(InsuranceFileConst.ACInsuredName)
                    'Developer Guide No. 24

                    .IsClientInvoiced = v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)
                    .OldPolicyNumber = v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)
                    .QuoteExpiryDate = v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)
                    .AlternateReference = v_vFieldArray(InsuranceFileConst.ACAlternateReference)
                    .AlternateAccountCnt = v_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt)

                    If _
                        Not _
                        (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACTransDescription)) Or
                            Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACTransDescription))) Then

                        .TransDescription = CStr(v_vFieldArray(InsuranceFileConst.ACTransDescription))
                        'Developer Guide No. 24

                        .CCTermsAgreed = v_vFieldArray(InsuranceFileConst.ACCCTermsAgreed)
                        .CCTermsAgreedDate = v_vFieldArray(InsuranceFileConst.ACCCTermsAgreedDate)

                        .CCPolicyDocumentsIssuedDate = v_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentsIssuedDate)

                        .CCPolicyDocumentCorrect = v_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentCorrect)

                        .CCErrorNotificationDate = v_vFieldArray(InsuranceFileConst.ACCCErrorNotificationDate)
                        If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt))) Then
                            'Developer Guide No. 24

                            .AccountExecutiveCnt = v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt)
                        End If
                    End If
                    .CCInceptionDate = v_vFieldArray(InsuranceFileConst.ACCCInceptionDate)
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACAnniversaryDate Then

                        If _
                            Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)) Or
                            Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)) Then

                            .AnniversaryDate = Nothing
                        Else
                            .AnniversaryDate = v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)
                        End If
                    End If

                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACPolicyStyleID Then

                        If _
                            Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyStyleID)) Or
                            Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyStyleID)) Then

                            .PolicyStyleID = Nothing
                        Else
                            .PolicyStyleID = v_vFieldArray(InsuranceFileConst.ACPolicyStyleID)
                        End If
                    End If

                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACUnderwritingYearID Then
                        If gPMFunctions.NullToLong(v_vFieldArray(InsuranceFileConst.ACUnderwritingYearID)) = 0 Then

                            .UnderwritingYearID = Nothing
                        Else
                            .UnderwritingYearID = v_vFieldArray(InsuranceFileConst.ACUnderwritingYearID)
                        End If
                    End If

                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACPolicyStatusID Then
                        If gPMFunctions.NullToLong(v_vFieldArray(InsuranceFileConst.ACPolicyStatusID)) = 0 Then

                            .PolicyStatusID = Nothing
                        Else
                            .PolicyStatusID = v_vFieldArray(InsuranceFileConst.ACPolicyStatusID)
                        End If
                    End If

                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACInceptionTPI Then

                        If gPMFunctions.NullToDate(v_vFieldArray(InsuranceFileConst.ACInceptionTPI)) = #12/30/1899# Then

                            .InceptionTPI = Nothing
                        Else
                            .InceptionTPI = v_vFieldArray(InsuranceFileConst.ACInceptionTPI)
                        End If
                    End If

                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSACustomerCategoryID Then
                        .FSACustomerCategoryID = v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID)
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSAContractLocationID Then

                        .FSAContractLocationID = v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID)
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSAUnderwriterCnt Then
                        .FSAUnderwriterCnt = v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt)
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSATypeOfSaleID Then
                        .FSATypeOfSaleID = v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID)
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFSARenewalConsent Then
                        .FSARenewalConsent = v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent)
                    End If
                    If Not v_vFieldArray(InsuranceFileConst.ACCountryID) Is DBNull.Value Then
                        .CountryID = CInt(v_vFieldArray(InsuranceFileConst.ACCountryID))
                    End If

                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountReasonID Then
                        .DiscountReasonID = CInt(v_vFieldArray(InsuranceFileConst.ACDiscountReasonID))
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountedPremium Then

                        If v_vFieldArray(InsuranceFileConst.ACDiscountedPremium) Is DBNull.Value Then
                            v_vFieldArray(InsuranceFileConst.ACDiscountedPremium) = 0
                        End If
                        If v_vFieldArray(InsuranceFileConst.ACDiscountedPremium) Is DBNull.Value Then
                            v_vFieldArray(InsuranceFileConst.ACDiscountedPremium) = 0
                        End If
                        .DiscountedPremium = v_vFieldArray(InsuranceFileConst.ACDiscountedPremium)
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountPercentage Then

                        If v_vFieldArray(InsuranceFileConst.ACDiscountPercentage) Is DBNull.Value Then
                            v_vFieldArray(InsuranceFileConst.ACDiscountPercentage) = 0
                        End If
                        .DiscountPercentage = v_vFieldArray(InsuranceFileConst.ACDiscountPercentage)
                    End If
                    If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACMatchDiscountedPremiumFlag Then
                        .MatchDiscountedPremiumFlag = CInt(v_vFieldArray(InsuranceFileConst.ACMatchDiscountedPremiumFlag))
                    End If
                    .PutOnNextInstalmentRenewal = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal))


                    .AnniversaryCopy = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy))
                End If

                If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACDiscountRecurringTypeId Then
                    .DiscountRecurringTypeId = CInt(v_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId))
                End If

                If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACLeadAllowConsolidatedCommission Then
                    .ConsolidatedLeadCommission =
                        gPMFunctions.ToSafeInteger(
                            v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission), 0)
                End If
                If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACSubAllowConsolidatedCommission Then
                    .ConsolidatedSubCommission =
                        gPMFunctions.ToSafeInteger(
                            v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission), 0)
                End If

                If v_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement) Is DBNull.Value Then
                    v_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement) = 0
                End If

                .RiskTransferAgreement = v_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement)

                .RenewalProductID = v_vFieldArray(InsuranceFileConst.ACRenewalProductID)

                If v_vFieldArray(InsuranceFileConst.ACOriginalProductID) Is DBNull.Value Then
                    v_vFieldArray(InsuranceFileConst.ACOriginalProductID) = 0
                End If
                .OriginalProductID = v_vFieldArray(InsuranceFileConst.ACOriginalProductID)

                If v_vFieldArray(InsuranceFileConst.ACFSARiskTransferEditable) Is DBNull.Value Then
                    v_vFieldArray(InsuranceFileConst.ACFSARiskTransferEditable) = 0
                End If
                .RiskTransferEditable = v_vFieldArray(InsuranceFileConst.ACFSARiskTransferEditable)

                ' If we have changed one of the properties, update the status
                m_iDatabaseStatus = v_iStatus

                If v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) Is DBNull.Value Then
                    v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) = 0
                End If
                If v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) Is DBNull.Value Then
                    v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) = 0
                End If
                .ManualDiscountPercentage = v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage)

                .QuoteStatusID = v_vFieldArray(InsuranceFileConst.ACQuoteStatusID)
                .QuoteVersionID = v_vFieldArray(InsuranceFileConst.ACQuoteVersionID)
                .BaseInsuranceFolderCnt = v_vFieldArray(InsuranceFileConst.ACBaseInsuranceFolderCnt)

                .ContactuserId = v_vFieldArray(InsuranceFileConst.ACContactuserId)
                .MTAReasonId = v_vFieldArray(InsuranceFileConst.ACMTAReasonId)

                If v_vFieldArray(InsuranceFileConst.kCoInsPlacement) Is DBNull.Value Then
                    v_vFieldArray(InsuranceFileConst.kCoInsPlacement) = String.Empty
                End If
                .CoInsPlacement = v_vFieldArray(InsuranceFileConst.kCoInsPlacement)

                .IsMarketPlacePolicy = v_vFieldArray(InsuranceFileConst.ACIsMarketPlacePolicy)
                .CollectionFrequencyID = v_vFieldArray(InsuranceFileConst.kCollectionFrequencyId)
                .PaymentTermsID = v_vFieldArray(InsuranceFileConst.kPaymentTermId)
                .CorrespondenceType = v_vFieldArray(InsuranceFileConst.ACCorrespondenceType)
                .DefaultPreferredCorrespondence = v_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence)
                .IsAgentCorrespondence = v_vFieldArray(InsuranceFileConst.ACIsAgentCorrespondence)
                .MediaType = v_vFieldArray(InsuranceFileConst.ACMediaType)
                .SenderEmail = v_vFieldArray(InsuranceFileConst.ACSenderEmail)
                .ReceiverEmail = v_vFieldArray(InsuranceFileConst.ACReceiverEmail)
                .OriginalInsuranceFileTypeID = v_vFieldArray(InsuranceFileConst.ACOriginalInsuranceFileTypeId)
            End With
            Return nResult

        Catch excep As System.Exception




            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function GetProperties(ByRef r_iStatus As Integer, ByRef r_vFieldArray() As Object) As Integer
        Return GetProperties(r_iStatus:=r_iStatus, r_vFieldArray:=r_vFieldArray, v_vSubType:=Nothing)
    End Function

    Public Function GetProperties(ByRef r_iStatus As Integer, ByRef r_vFieldArray() As Object, ByVal v_vSubType As Object) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ReDim r_vFieldArray(InsuranceFileConst.ACFieldArraySize)

            ' Set Property values.
            With m_dSIRInsuranceFile

                r_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt) = .InsuranceFileCnt

                r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID) = .InsuranceFileStructureID

                r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID) = .InsuranceFileTypeID

                r_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID) = .InsuranceFileStatusID

                r_vFieldArray(InsuranceFileConst.ACInsuranceFileID) = .InsuranceFileID

                r_vFieldArray(InsuranceFileConst.ACSourceID) = .SourceID

                r_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt) = .InsuranceFolderCnt

                r_vFieldArray(InsuranceFileConst.ACInsuranceRef) = .InsuranceRef

                r_vFieldArray(InsuranceFileConst.ACProductID) = .ProductID

                r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt) = .LeadInsurerCnt

                r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt) = .LeadAgentCnt

                r_vFieldArray(InsuranceFileConst.ACLeadAgentPercent) = .LeadAgentPercent

                r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt) = .AccountHandlerCnt

                r_vFieldArray(InsuranceFileConst.ACInsuredCnt) = .InsuredCnt

                r_vFieldArray(InsuranceFileConst.ACBusinessTypeID) = .BusinessTypeID

                r_vFieldArray(InsuranceFileConst.ACCollectTypeID) = .CollectTypeID

                r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt) = .CollectionFromCnt

                r_vFieldArray(InsuranceFileConst.ACSubBranchID) = .SubBranchId

                r_vFieldArray(InsuranceFileConst.ACCurrencyID) = .CurrencyID

                r_vFieldArray(InsuranceFileConst.ACBaseCurrencyID) = .BaseCurrencyID

                r_vFieldArray(InsuranceFileConst.ACLanguageID) = .LanguageID

                r_vFieldArray(InsuranceFileConst.ACDateIssued) = .DateIssued

                r_vFieldArray(InsuranceFileConst.ACCoverStartDate) = .CoverStartDate

                r_vFieldArray(InsuranceFileConst.ACExpiryDate) = .ExpiryDate

                r_vFieldArray(InsuranceFileConst.ACRenewalDate) = .RenewalDate

                r_vFieldArray(InsuranceFileConst.ACRenewalMethodID) = .RenewalMethodID

                r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID) = .RenewalFrequencyID

                r_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal) = .IsReferredAtRenewal

                r_vFieldArray(InsuranceFileConst.ACLapsedReasonID) = .LapsedReasonID

                r_vFieldArray(InsuranceFileConst.ACLapsedDate) = .LapsedDate

                r_vFieldArray(InsuranceFileConst.ACLapsedDescription) = .LapsedDescription

                r_vFieldArray(InsuranceFileConst.ACIsReferredOnMta) = .IsReferredOnMta

                r_vFieldArray(InsuranceFileConst.ACPolicyVersion) = .PolicyVersion

                r_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus) = .GeminiPolicyStatus

                r_vFieldArray(InsuranceFileConst.ACGeminiBusinessType) = .GeminiBusinessType

                r_vFieldArray(InsuranceFileConst.ACDeferredInd) = .DeferredInd

                r_vFieldArray(InsuranceFileConst.ACPolicyIgnore) = .PolicyIgnore

                r_vFieldArray(InsuranceFileConst.ACBrokerCnt) = .BrokerCnt

                r_vFieldArray(InsuranceFileConst.ACRiskCodeId) = .RiskCodeID

                r_vFieldArray(InsuranceFileConst.ACAnalysisCodeId) = .AnalysisCodeID

                r_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId) = .PolicyDeductiblesID

                r_vFieldArray(InsuranceFileConst.ACPolicyLimitsId) = .PolicyLimitsID

                r_vFieldArray(InsuranceFileConst.ACProposalDate) = .ProposalDate

                r_vFieldArray(InsuranceFileConst.ACDiaryDate) = .DiaryDate

                r_vFieldArray(InsuranceFileConst.ACReviewDate) = .ReviewDate

                r_vFieldArray(InsuranceFileConst.ACRenewalDayNumber) = .RenewalDayNumber

                r_vFieldArray(InsuranceFileConst.ACPolicyTypeId) = .PolicyTypeID

                r_vFieldArray(InsuranceFileConst.ACIndicator) = .Indicator

                r_vFieldArray(InsuranceFileConst.ACClause) = .Clause

                r_vFieldArray(InsuranceFileConst.ACCover) = .Cover

                r_vFieldArray(InsuranceFileConst.ACArea) = .Area

                r_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate) = .LongTermUndertakingDate

                r_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID) = .RenewalStopCodeID

                r_vFieldArray(InsuranceFileConst.ACVBSType) = .VBSType

                r_vFieldArray(InsuranceFileConst.ACVBSStatus) = .VBSStatus

                r_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable) = .IsInsurerRateTable

                r_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies) = .IsRelatedPolicies

                r_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments) = .IsRetainedDocuments

                r_vFieldArray(InsuranceFileConst.ACSchemesPostcode) = .SchemesPostcode

                r_vFieldArray(InsuranceFileConst.ACPaidDirect) = .PaidDirect

                r_vFieldArray(InsuranceFileConst.ACScheme) = .Scheme

                r_vFieldArray(InsuranceFileConst.ACBrokerageAmount) = .BrokerageAmount

                r_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag) = .IsMinimumBrokerageFlag

                r_vFieldArray(InsuranceFileConst.ACAnnualPremium) = .AnnualPremium

                r_vFieldArray(InsuranceFileConst.ACThisPremium) = .ThisPremium

                r_vFieldArray(InsuranceFileConst.ACNetPremium) = .NetPremium

                r_vFieldArray(InsuranceFileConst.ACCommissionAmount) = .CommissionAmount

                r_vFieldArray(InsuranceFileConst.ACIPTableAmount) = .IPTAbleAmount

                r_vFieldArray(InsuranceFileConst.ACIPTPercentage) = .IPTPercentage

                r_vFieldArray(InsuranceFileConst.ACIsIPTOverridden) = .ISIPTOverridden

                r_vFieldArray(InsuranceFileConst.ACTaxAmount) = .TaxAmount

                r_vFieldArray(InsuranceFileConst.ACVatableAmount) = .VatableAmount

                r_vFieldArray(InsuranceFileConst.ACVatPercentage) = .VATPercentage

                r_vFieldArray(InsuranceFileConst.ACVatAmount) = .VATAmount

                r_vFieldArray(InsuranceFileConst.ACPaymentMethod) = .PaymentMethod

                r_vFieldArray(InsuranceFileConst.ACUserDefinedDataID) = .UserDefinedDataID

                r_vFieldArray(InsuranceFileConst.ACCommissionPercentage) = .CommissionPercentage

                r_vFieldArray(InsuranceFileConst.ACInvariantKey) = .InvariantKey

                r_vFieldArray(InsuranceFileConst.ACInsuredName) = .InsuredName

                r_vFieldArray(InsuranceFileConst.ACAlternateReference) = .AlternateReference

                r_vFieldArray(InsuranceFileConst.ACIsClientInvoiced) = .IsClientInvoiced

                r_vFieldArray(InsuranceFileConst.ACOldPolicyNumber) = .OldPolicyNumber

                r_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate) = .QuoteExpiryDate

                r_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt) = .AlternateAccountCnt

                r_vFieldArray(InsuranceFileConst.ACAnniversaryDate) = .AnniversaryDate

                If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACAccountExecutiveCnt Then
                    r_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt) = .AccountExecutiveCnt
                    r_iStatus = m_iDatabaseStatus
                End If

                r_vFieldArray(InsuranceFileConst.ACPolicyStyleID) = .PolicyStyleID

                r_vFieldArray(InsuranceFileConst.ACPolicyStatusID) = .PolicyStatusID

                r_vFieldArray(InsuranceFileConst.ACInceptionTPI) = .InceptionTPI

                r_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID) = .FSACustomerCategoryID

                r_vFieldArray(InsuranceFileConst.ACFSAContractLocationID) = .FSAContractLocationID

                r_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt) = .FSAUnderwriterCnt

                r_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID) = .FSATypeOfSaleID

                r_vFieldArray(InsuranceFileConst.ACFSARenewalConsent) = .FSARenewalConsent

                r_vFieldArray(InsuranceFileConst.ACCountryID) = .CountryID

                r_vFieldArray(InsuranceFileConst.ACDiscountReasonID) = .DiscountReasonID

                r_vFieldArray(InsuranceFileConst.ACDiscountedPremium) = .DiscountedPremium

                r_vFieldArray(InsuranceFileConst.ACDiscountPercentage) = .DiscountPercentage
                r_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal) = .PutOnNextInstalmentRenewal

                r_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal) = .PutOnNextInstalmentRenewal

                r_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy) = .AnniversaryCopy

                r_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId) = .DiscountRecurringTypeId

                r_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission) = .ConsolidatedLeadCommission

                r_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission) = .ConsolidatedSubCommission

                r_vFieldArray(InsuranceFileConst.ACCCTermsAgreed) = .CCTermsAgreed

                r_vFieldArray(InsuranceFileConst.ACCCTermsAgreedDate) = .CCTermsAgreedDate

                r_vFieldArray(InsuranceFileConst.ACCCInceptionDate) = .CCInceptionDate

                r_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentsIssuedDate) = .CCPolicyDocumentsIssuedDate

                r_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentCorrect) = .CCPolicyDocumentCorrect

                r_vFieldArray(InsuranceFileConst.ACCCErrorNotificationDate) = .CCErrorNotificationDate

                r_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement) = .RiskTransferAgreement

                r_vFieldArray(InsuranceFileConst.ACRenewalProductID) = .RenewalProductID

                r_vFieldArray(InsuranceFileConst.ACOriginalProductID) = .OriginalProductID

                r_vFieldArray(InsuranceFileConst.ACFSARiskTransferEditable) = .RiskTransferEditable

                r_vFieldArray(InsuranceFileConst.ACCurrencyToBaseXRate) = .CurrencyToBaseXRate

                r_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) = .ManualDiscountPercentage

                r_vFieldArray(InsuranceFileConst.ACQuoteStatusID) = .QuoteStatusID
                r_vFieldArray(InsuranceFileConst.ACQuoteVersionID) = .QuoteVersionID
                r_vFieldArray(InsuranceFileConst.ACBaseInsuranceFolderCnt) = .BaseInsuranceFolderCnt

                If .CollectionFrequencyID > 0 Then
                    r_vFieldArray(InsuranceFileConst.kCollectionFrequencyId) = .CollectionFrequencyID
                Else
                    r_vFieldArray(InsuranceFileConst.kCollectionFrequencyId) = m_oCollectionFrequencyID
                End If

                If .PaymentTermsID > 0 Then
                    r_vFieldArray(InsuranceFileConst.kPaymentTermId) = .PaymentTermsID
                Else
                    r_vFieldArray(InsuranceFileConst.kPaymentTermId) = m_oPaymentTermsID
                End If
                r_vFieldArray(InsuranceFileConst.ACContactuserId) = .ContactuserId
                r_vFieldArray(InsuranceFileConst.ACMTAReasonId) = .MTAReasonId
                r_vFieldArray(InsuranceFileConst.ACIsMarketPlacePolicy) = .IsMarketPlacePolicy
                r_vFieldArray(InsuranceFileConst.kCoInsPlacement) = .CoInsPlacement
                r_vFieldArray(InsuranceFileConst.ACAnniversaryDate) = .AnniversaryDate
                r_vFieldArray(InsuranceFileConst.ACFeesTaxes) = .FeesTaxes
                r_vFieldArray(InsuranceFileConst.ACUnderwritingYearID) = .UnderwritingYearID
                r_vFieldArray(InsuranceFileConst.ACCorrespondenceType) = .CorrespondenceType
                r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = .DefaultPreferredCorrespondence
                r_vFieldArray(InsuranceFileConst.ACIsAgentCorrespondence) = .IsAgentCorrespondence
                r_vFieldArray(InsuranceFileConst.ACMediaType) = .MediaType
                r_vFieldArray(InsuranceFileConst.ACSenderEmail) = .SenderEmail
                r_vFieldArray(InsuranceFileConst.ACReceiverEmail) = .ReceiverEmail
                r_vFieldArray(InsuranceFileConst.ACOriginalInsuranceFileTypeId) = .OriginalInsuranceFileTypeID
            End With

            Return nResult

        Catch excep As System.Exception




            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFile

                ' Set Data object primary key
                .InsuranceFileCnt = InsuranceFileCnt

                'And if we're coming from events
                .FromEvent = FromEvent

                ' Select a record from the database

                m_lReturn = .SelectSingle()


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'We need this before we get to call the getitem
                m_lInsuranceFolderCnt = .InsuranceFolderCnt

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFile

                'Set the new one we've just assigned
                .InsuranceFolderCnt = InsuranceFolderCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRInsuranceFile Added
                InsuranceFileCnt = .InsuranceFileCnt

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateLapsed() As Integer
        Return UpdateLapsed(v_dtLapsedDate:=Nothing)
    End Function

    Public Function UpdateLapsed(ByVal v_dtLapsedDate As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFile

                'Set the insurancefilecnt
                .InsuranceFileCnt = InsuranceFileCnt

                ' Add a record to the database from the object
                m_lReturn = .UpdateLapsed(v_dtLapsedDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLapsed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLapsed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CancelPolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFile

                'Set the insurancefilecnt
                .InsuranceFileCnt = InsuranceFileCnt

                ' Add a record to the database from the object
                m_lReturn = .CancelPolicy()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFile

                ' Set Data object primary key
                .InsuranceFileCnt = InsuranceFileCnt

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFile

                ' Set Data object primary key
                .InsuranceFileCnt = InsuranceFileCnt

                'DN 16/08/01 - Pass TransInsFolderCnt incase InsFolderCnt has been changed
                .TransInsuranceFolderCnt = TransInsuranceFolderCnt

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyToEvent (Public)
    '
    ' Description: Makes a copy of the insurance details to the event table.
    '
    ' ***************************************************************** '
    Public Function CopyToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_dSIRInsuranceFile.CopyFolderToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRInsuranceFile.CopyFileToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR20060915 - datasure coinsurer development
            m_lReturn = m_dSIRInsuranceFile.CopyCoinsurerSectionsToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck030801
    ' ***************************************************************** '
    ' Name: CopyToTransactionEvent (Public)
    '
    ' Description: Makes a copy of the insurance details to the event table.
    '
    ' ***************************************************************** '
    Public Function CopyToTransactionEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_dSIRInsuranceFile.CopyFolderToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRInsuranceFile.CopyFileToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRInsuranceFile.CopyCoinsurerSectionsToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyToTransactionEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyToTransactionEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'EK 05/09/99 New Method to Update Insurance recs from Editable Event
    ' ***************************************************************** '
    ' Name: CopyFromEvent (Public)
    '
    ' Description: Makes a copy of the insurance details to the event table.
    '
    ' eck080900 pass the transaction type through
    '
    ' ***************************************************************** '
    Public Function CopyFromEvent(ByVal v_lEventCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lLastTransType As Integer, ByVal v_sTransDebitCredit As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CopyFromEvent"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_dSIRInsuranceFile.CopyFolderFromEvent(v_lEventCnt, v_lInsuranceFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_dSIRInsuranceFile.CopyFolderFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_dSIRInsuranceFile.CopySystemFromEvent(v_lEventCnt, v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_dSIRInsuranceFile.CopySystemFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_lLastTransType = 3 And m_dSIRInsuranceFile.PolicyTypeID = 3 Then 'Adjustment (MTA) transaction on a general broking policy
                If v_sTransDebitCredit = "D" Then
                    m_lReturn = m_dSIRInsuranceFile.AddFileToEvent(v_lEventCnt, v_lInsuranceFileCnt, v_lInsuranceFolderCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_dSIRInsuranceFile.AddFileToEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = m_dSIRInsuranceFile.UpdateCoinsurerToEvent(v_lEventCnt, v_lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_dSIRInsuranceFile.UpdateCoinsurerToEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    m_lReturn = m_dSIRInsuranceFile.TakeFileFromEvent(v_lEventCnt, v_lInsuranceFileCnt, v_lInsuranceFolderCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_dSIRInsuranceFile.TakeFileFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = m_dSIRInsuranceFile.UpdateCoinsurerFormEvent(v_lEventCnt, v_lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_dSIRInsuranceFile.UpdateCoinsurerFormEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = m_dSIRInsuranceFile.RecalculateFeesAfterEvent(v_lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_dSIRInsuranceFile.RecalculateFeesAfterEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = m_dSIRInsuranceFile.RecalculateAgentsAfterEvent(v_lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_dSIRInsuranceFile.RecalculateAgentsAfterEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If
            Else
                m_lReturn = m_dSIRInsuranceFile.CopyFileFromEvent(v_lEventCnt, v_lInsuranceFileCnt, v_lInsuranceFolderCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_dSIRInsuranceFile.CopyFileFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_dSIRInsuranceFile.CopySectionsFromEvent(v_lEventCnt, v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_dSIRInsuranceFile.CopySectionsFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_dSIRInsuranceFile.CopyCoinsurerSectionsFromEvent(v_lEventCnt, v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_dSIRInsuranceFile.CopyCoinsurerSectionsFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If v_lLastTransType <> 3 Then 'Not an adjustment (MTA) transaction
                m_lReturn = m_dSIRInsuranceFile.CopyExtrasFromEvent(v_lEventCnt, v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_dSIRInsuranceFile.CopyExtrasFromEvent", "v_lEventCnt:=" & v_lEventCnt, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    'EK 05/09/99 New Method to Delete Editable Event
    ' ***************************************************************** '
    ' Name: DeleteEvent (Public)
    '
    ' Description: Makes a copy of the insurance details to the event table.
    '
    ' ***************************************************************** '
    Public Function DeleteEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_dSIRInsuranceFile.DeleteFolderEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRInsuranceFile.DeleteFileEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRInsuranceFile.DeleteSystemEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRInsuranceFile.DeleteExtrasEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
#End Region

#Region "Private Methods"


    ''' <summary>
    ''' DefaultParameters - Sets the Default Values for a SIRInsuranceFile.
    ''' </summary>
    ''' <param name="v_bDefaultAll"></param>
    ''' <param name="r_vFieldArray"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function DefaultParameters(ByVal v_bDefaultAll As Boolean, ByRef r_vFieldArray() As Object,
                                       Optional ByVal v_vSubType As Object = Nothing) As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt), Nothing)) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt) = 0
        End If


        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID) = 1
        End If


        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID) = 1
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID), Nothing)) Or (v_bDefaultAll) _
            Then


            r_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFileID), Nothing)) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACInsuranceFileID) = 0
        End If


        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACSourceID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACSourceID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACSourceID) = m_iSourceID
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt), Nothing)) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt) = 0
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuranceRef), Nothing)) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACInsuranceRef) = ""
        End If


        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACProductID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACProductID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACProductID) = 1
        End If


        If r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)) = 0) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt) = DBNull.Value
        End If


        If r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)) = 0) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLeadAgentPercent), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACLeadAgentPercent) = DBNull.Value
        End If


        If r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)) = 0) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt) = DBNull.Value
        End If


        If r_vFieldArray(InsuranceFileConst.ACInsuredCnt) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACInsuredCnt) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuredCnt), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACInsuredCnt)) = 0) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACInsuredCnt) = DBNull.Value
        End If


        If r_vFieldArray(InsuranceFileConst.ACBusinessTypeID) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACBusinessTypeID) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACBusinessTypeID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACBusinessTypeID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACBusinessTypeID) = 1
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCollectTypeID), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACCollectTypeID) = DBNull.Value
        End If


        If r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)) = 0) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt) = DBNull.Value
        End If

        'sj 19/07/2002 - start
        '    If ((IsEmpty(r_vFieldArray(ACBranchID)) = True) _
        ''    Or (r_vFieldArray(ACBranchID) = 0) Or (v_bDefaultAll = True)) Then
        '        r_vFieldArray(ACBranchID) = 1
        '    End If

        'MKW270503 PN4205 - 1.8.5 to 1.8.6 Catchup START
        'CJB150103 Mult-Branch Accounting related change to store the correct SubBranch in insurance_file
        'If comes thru as zero then do NOT default to 1 as the revised sp will set the correct value
        'If ((IsEmpty(r_vFieldArray(ACSubBranchID)) = True) _
        ''Or (r_vFieldArray(ACSubBranchID) = 0) Or (v_bDefaultAll = True)) Then
        '    r_vFieldArray(ACSubBranchID) = 1
        'End If
        If v_bDefaultAll Then

            r_vFieldArray(InsuranceFileConst.ACSubBranchID) = 1
        End If
        'MKW270503 PN4205 - 1.8.5 to 1.8.6 Catchup END
        'sj 19/07/2002 - end


        If r_vFieldArray(InsuranceFileConst.ACCurrencyID) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACCurrencyID) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCurrencyID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACCurrencyID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACCurrencyID) = m_iCurrencyID
        End If


        If r_vFieldArray(InsuranceFileConst.ACLanguageID) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACLanguageID) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLanguageID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACLanguageID)) = 0) Or (v_bDefaultAll) Then

            r_vFieldArray(InsuranceFileConst.ACLanguageID) = m_iLanguageID
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACDateIssued), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACDateIssued) = DateTime.Now
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCoverStartDate), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACCoverStartDate) = DateTime.Now
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACExpiryDate), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACExpiryDate) = DateTime.Now.AddYears(1)
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalDate), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACRenewalDate) = r_vFieldArray(InsuranceFileConst.ACExpiryDate)
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalMethodID), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACRenewalMethodID) = DBNull.Value
        End If

        If r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)) = 0) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID) = 1
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal) = 0
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLapsedReasonID), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACLapsedReasonID) = DBNull.Value
        End If

        'If r_vFieldArray(InsuranceFileConst.ACLapsedDate) Is DBNull.Value Or r_vFieldArray(InsuranceFileConst.ACLapsedDate) Is Nothing Then
        '    r_vFieldArray(InsuranceFileConst.ACLapsedDate) = DateTime.MinValue
        'End If

        'If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLapsedDate), Nothing)) Or (CDate(r_vFieldArray(InsuranceFileConst.ACLapsedDate)) = CDate(String.Format("{0: yyyy/MM/dd} ", r_vFieldArray(InsuranceFileConst.ACLapsedDate)))) Or (v_bDefaultAll) Then
        '    r_vFieldArray(InsuranceFileConst.ACLapsedDate) = DBNull.Value
        'End If
        If _
            (r_vFieldArray(InsuranceFileConst.ACLapsedDate) Is DBNull.Value) OrElse
            (r_vFieldArray(InsuranceFileConst.ACLapsedDate) Is Nothing) OrElse
            (CDate(r_vFieldArray(InsuranceFileConst.ACLapsedDate)) = ToSafeDate("29/12/1899")) OrElse
            (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACLapsedDate) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLapsedDescription), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACLapsedDescription) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsReferredOnMta), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACIsReferredOnMta) = 0
        End If

        If r_vFieldArray(InsuranceFileConst.ACPolicyVersion) Is DBNull.Value Then
            r_vFieldArray(InsuranceFileConst.ACPolicyVersion) = 0
        End If
        If _
            (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPolicyVersion), Nothing)) Or
            (CDbl(r_vFieldArray(InsuranceFileConst.ACPolicyVersion)) = 0) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACPolicyVersion) = 1
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACGeminiBusinessType), Nothing)) Or (v_bDefaultAll) Then
            r_vFieldArray(InsuranceFileConst.ACGeminiBusinessType) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACDeferredInd), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACDeferredInd) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPolicyIgnore), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACPolicyIgnore) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACBrokerCnt), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACBrokerCnt) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACRiskCodeId), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACRiskCodeId) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACAnalysisCodeId), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACAnalysisCodeId) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPolicyLimitsId), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACPolicyLimitsId) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACProposalDate), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACProposalDate) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACDiaryDate), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACDiaryDate) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACReviewDate), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACReviewDate) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalDayNumber), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACRenewalDayNumber) =
                CDate(r_vFieldArray(InsuranceFileConst.ACRenewalDate)).Day
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPolicyTypeId), Nothing)) Or (v_bDefaultAll) Then
            'Default it to PMB

            r_vFieldArray(InsuranceFileConst.ACPolicyTypeId) = PMBConst.PMBPolicyTypeGeneral
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIndicator), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIndicator) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACClause), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACClause) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCover), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACCover) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACArea), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACArea) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate), Nothing)) Or (v_bDefaultAll) _
            Then


            r_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACVBSType), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACVBSType) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACVBSStatus), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACVBSStatus) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACSchemesPostcode), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACSchemesPostcode) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPaidDirect), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACPaidDirect) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACScheme), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACScheme) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACBrokerageAmount), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACBrokerageAmount) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag), Nothing)) Or (v_bDefaultAll) _
            Then


            r_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACAnnualPremium), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACAnnualPremium) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACThisPremium), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACThisPremium) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACNetPremium), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACNetPremium) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCommissionAmount), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACCommissionAmount) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIPTableAmount), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIPTableAmount) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIPTPercentage), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIPTPercentage) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsIPTOverridden), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIsIPTOverridden) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACTaxAmount), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACTaxAmount) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACVatableAmount), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACVatableAmount) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACVatPercentage), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACVatPercentage) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACVatAmount), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACVatAmount) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPaymentMethod), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACPaymentMethod) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACUserDefinedDataID), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACUserDefinedDataID) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACCommissionPercentage), Nothing)) Or (v_bDefaultAll) _
            Then


            r_vFieldArray(InsuranceFileConst.ACCommissionPercentage) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInvariantKey), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACInvariantKey) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACInsuredName), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACInsuredName) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACAlternateReference), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACAlternateReference) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACIsClientInvoiced), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACIsClientInvoiced) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACOldPolicyNumber), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACOldPolicyNumber) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate) = DBNull.Value
        End If


        If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt) = DBNull.Value
        End If

        ' SJP (CMG) 04042003 PS235
        'PSL 23/04/2003 Iss3742 Check that the array that has been sent has this new element
        If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACAccountExecutiveCnt Then

            If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt), Nothing)) Or (v_bDefaultAll) _
                Then


                r_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt) = DBNull.Value
            End If
        End If

        If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACPolicyStyleID Then

            If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACPolicyStyleID), Nothing)) Or (v_bDefaultAll) Then

                r_vFieldArray(InsuranceFileConst.ACPolicyStyleID) = 0
            End If
        End If

        If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACUnderwritingYearID Then

            If (Object.Equals(r_vFieldArray(InsuranceFileConst.ACUnderwritingYearID), Nothing)) Or (v_bDefaultAll) _
                Then


                r_vFieldArray(InsuranceFileConst.ACUnderwritingYearID) = DBNull.Value
            End If
        End If
        ' TMP
        If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACLeadAllowConsolidatedCommission Then

            If _
                (Object.Equals(r_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission), Nothing)) Or
                (v_bDefaultAll) Then


                r_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission) = DBNull.Value
            End If
        End If

        If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACSubAllowConsolidatedCommission Then

            If _
                (Object.Equals(r_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission), Nothing)) Or
                (v_bDefaultAll) Then


                r_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission) = DBNull.Value
            End If
        End If


        If Object.Equals(r_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement), Nothing) Or v_bDefaultAll _
            Then


            r_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement) = DBNull.Value
        End If

        'Wr25 1.12

        If Object.Equals(r_vFieldArray(InsuranceFileConst.ACRenewalProductID), Nothing) Then


            r_vFieldArray(InsuranceFileConst.ACRenewalProductID) = DBNull.Value
        End If


        If Object.Equals(r_vFieldArray(InsuranceFileConst.ACOriginalProductID), Nothing) Then


            r_vFieldArray(InsuranceFileConst.ACOriginalProductID) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.kCoInsPlacement), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.kCoInsPlacement) = DBNull.Value
        End If

        If (Object.Equals(r_vFieldArray(InsuranceFileConst.kCoInsPlacement), Nothing)) Or (v_bDefaultAll) Then


            r_vFieldArray(InsuranceFileConst.kCoInsPlacement) = DBNull.Value
        End If

        If Object.Equals(r_vFieldArray(InsuranceFileConst.ACOriginalInsuranceFileTypeId), Nothing) Then


            r_vFieldArray(InsuranceFileConst.ACOriginalInsuranceFileTypeId) = DBNull.Value
        End If


        ' {* USER DEFINED CODE (End) *}

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRInsuranceFile for Consistency.
    '
    ' SJP (CMG)     04/04/2003      PS235
    ' ***************************************************************** '
    Private Function Validate(ByVal v_vFieldArray() As Object, Optional ByVal v_vSubType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sValue As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt), Nothing) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID), Nothing) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID), Nothing) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID))) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuranceFileID), Nothing) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFileID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACSourceID), Nothing) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACSourceID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt), Nothing) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACProductID), Nothing) Then

            Dim dbNumericTemp8 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACProductID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        '    If (IsEmpty(v_vFieldArray(ACLeadInsurerCnt)) = False) Then
        '        If (IsNumeric(v_vFieldArray(ACLeadInsurerCnt)) = False) Then
        '            Validate = PMFalse
        '            Exit Function
        '        End If
        '    End If



        If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt))) Then

            Dim dbNumericTemp9 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent))) Then

                Dim dbNumericTemp10 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If





        'If (Not CBool(v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt))).Equals(False) Then
        If (Not Informations.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt))) Then

            ' If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Then
            ' NEW CODE ADDED DUE TO CHECK IF ARRAY HAS NOTHING VALUE IN ELEMENTS
            If Not (v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)) Is Nothing Then
                If Not Informations.IsNumeric(CStr(v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt))) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'TMP

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission))) Then

                Dim dbNumericTemp12 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission))) Then

                Dim dbNumericTemp13 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        ' SJP (CMG) 04042003 PS235
        ' See if we want to handle the Account Executive entry.
        m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for Account Executive (" & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC230403 -ISS3730 -if no option set as not set
        If sValue.Length = 0 Then
            sValue = CStr(0)
        End If

        If CBool(sValue) = 1 Then
            'PSL 23/04/2003 Iss3742 Check that the array that has been sent has this new element
            If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACAccountExecutiveCnt Then


                If (Not CBool(v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt))).Equals(False) Then

                    Dim dbNumericTemp14 As Double
                    If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
        End If
        'end PS235


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACInsuredCnt), Nothing) Then

            Dim dbNumericTemp15 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACInsuredCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACBusinessTypeID), Nothing) Then

            Dim dbNumericTemp16 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACBusinessTypeID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACCollectTypeID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACCollectTypeID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACCollectTypeID))) Then

                Dim dbNumericTemp17 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACCollectTypeID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt))) Then

                Dim dbNumericTemp18 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'sj 19/07/2002 - start
        '    If (IsEmpty(v_vFieldArray(ACBranchID)) = False) Then
        '        If (IsNumeric(v_vFieldArray(ACBranchID)) = False) Then
        '            Validate = PMFalse
        '            Exit Function
        '        End If
        '    End If

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACSubBranchID), Nothing) Then

            Dim dbNumericTemp19 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACSubBranchID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp19) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'sj 19/07/2002 - end


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACCurrencyID), Nothing) Then

            Dim dbNumericTemp20 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACCurrencyID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp20) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACLanguageID), Nothing) Then

            Dim dbNumericTemp21 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACLanguageID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp21) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACDateIssued), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACDateIssued)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACDateIssued))) Then
                If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACDateIssued)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACCoverStartDate), Nothing) Then
            If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACCoverStartDate)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACExpiryDate), Nothing) Then
            If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACExpiryDate)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACRenewalDate), Nothing) Then
            If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACRenewalDate)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACRenewalMethodID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACRenewalMethodID))) Then

                Dim dbNumericTemp22 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp22) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID), Nothing) Then

            Dim dbNumericTemp23 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp23) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal), Nothing) Then

            Dim dbNumericTemp24 As Double
            If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp24) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACLapsedReasonID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACLapsedReasonID))) Then

                Dim dbNumericTemp25 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp25) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACLapsedDate), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLapsedDate)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACLapsedDate))) Then
                If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACLapsedDate)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta))) Then

                Dim dbNumericTemp26 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp26) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACPolicyVersion), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyVersion)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyVersion))) Then

                Dim dbNumericTemp27 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACPolicyVersion)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp27) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus))) Then

                Dim dbNumericTemp28 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp28) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType))) Then

                Dim dbNumericTemp29 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp29) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACDeferredInd), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACDeferredInd)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACDeferredInd))) Then

                Dim dbNumericTemp30 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACDeferredInd)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp30) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACPolicyIgnore), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyIgnore))) Then

                Dim dbNumericTemp31 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp31) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACBrokerCnt), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACBrokerCnt)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACBrokerCnt))) Then

                Dim dbNumericTemp32 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACBrokerCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp32) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACRiskCodeId), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACRiskCodeId)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACRiskCodeId))) Then

                Dim dbNumericTemp33 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACRiskCodeId)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp33) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId))) Then

                Dim dbNumericTemp34 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp34) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId))) Then

                Dim dbNumericTemp35 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp35) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId))) Then

                Dim dbNumericTemp36 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp36) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACProposalDate), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACProposalDate)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACProposalDate))) Then
                If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACProposalDate)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACDiaryDate), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACDiaryDate)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACDiaryDate))) Then
                If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACDiaryDate)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACReviewDate), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACReviewDate)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACReviewDate))) Then
                If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACReviewDate)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber))) Then

                Dim dbNumericTemp37 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp37) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACPolicyTypeId), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACPolicyTypeId))) Then

                Dim dbNumericTemp38 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp38) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'Indicator, Clause, Cover and Area are strings, so need no validation...


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate))) Then
                If Not Informations.IsDate(v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID))) Then

                Dim dbNumericTemp39 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp39) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'VBSType and VBSStatus are strings


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable))) Then

                Dim dbNumericTemp40 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp40) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies))) Then

                Dim dbNumericTemp41 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp41) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments))) Then

                Dim dbNumericTemp42 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp42) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'SchemesPostcode and PaidDirect are strings


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACScheme), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACScheme)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACScheme))) Then

                Dim dbNumericTemp43 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACScheme)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp43) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACBrokerageAmount), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACBrokerageAmount))) Then

                Dim dbNumericTemp44 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp44) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag))) Then

                Dim dbNumericTemp45 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp45) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACAnnualPremium), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACAnnualPremium)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACAnnualPremium))) Then

                Dim dbNumericTemp46 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACAnnualPremium)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp46) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACThisPremium), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACThisPremium)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACThisPremium))) Then

                Dim dbNumericTemp47 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACThisPremium)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp47) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACNetPremium), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACNetPremium)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACNetPremium))) Then

                Dim dbNumericTemp48 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACNetPremium)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp48) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACCommissionAmount), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACCommissionAmount)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACCommissionAmount))) Then

                Dim dbNumericTemp49 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACCommissionAmount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp49) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIPTableAmount), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIPTableAmount)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIPTableAmount))) Then

                Dim dbNumericTemp50 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIPTableAmount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp50) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIPTPercentage), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIPTPercentage)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIPTPercentage))) Then

                Dim dbNumericTemp51 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIPTPercentage)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp51) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden))) Then

                Dim dbNumericTemp52 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp52) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACTaxAmount), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACTaxAmount)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACTaxAmount))) Then

                Dim dbNumericTemp53 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACTaxAmount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp53) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACVatableAmount), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACVatableAmount)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACVatableAmount))) Then

                Dim dbNumericTemp54 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACVatableAmount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp54) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACVatPercentage), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACVatPercentage)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACVatPercentage))) Then

                Dim dbNumericTemp55 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACVatPercentage)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp55) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACVatAmount), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACVatAmount)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACVatAmount))) Then

                Dim dbNumericTemp56 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACVatAmount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp56) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID))) Then

                Dim dbNumericTemp57 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp57) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'FSA Phase III

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID))) Then

                Dim dbNumericTemp58 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp58) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID))) Then

                Dim dbNumericTemp59 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp59) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt))) Then

                Dim dbNumericTemp60 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp60) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID))) Then

                Dim dbNumericTemp61 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp61) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        If Not Object.Equals(v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent), Nothing) Then

            If Not (Convert.IsDBNull(v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent)) Or Informations.IsNothing(v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent))) Then

                Dim dbNumericTemp62 As Double
                If Not Double.TryParse(CStr(v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp62) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        'FSA Phase III End


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' Name: DataDifferent (Private)
    '
    ' Description: Compares two variants
    '
    ' ***************************************************************** '
    Private Function DataDifferent(ByVal v_vValue1 As Object, ByVal v_vValue2 As Object) As Boolean

        Dim result As Boolean = False



        If Convert.IsDBNull(v_vValue1) Or Informations.IsNothing(v_vValue1) Then

            If Convert.IsDBNull(v_vValue2) Or Informations.IsNothing(v_vValue2) Then
                Return False
            Else
                Return True
            End If
        Else

            If Convert.IsDBNull(v_vValue2) Or Informations.IsNothing(v_vValue2) Then
                Return True
            End If
        End If

        'So now we have two values to compare
        'Numbers first

        Dim dbNumericTemp As Double
        If Double.TryParse(CStr(v_vValue1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then


            Return CDbl(v_vValue1) <> CDbl(v_vValue2)
        End If

        'Now dates
        If Informations.IsDate(v_vValue1) Then


            Return CDate(v_vValue1) <> CDate(v_vValue2)
        End If

        'So now it's a string
        'EK 13/12/99 Comparing same value ? not a good idea
        '    DataDifferent = (Trim$(v_vValue1) <> Trim$(v_vValue1))



        ' {* USER DEFINED CODE (End) *}

        Return CStr(v_vValue1).Trim() <> CStr(v_vValue2).Trim()

    End Function

#End Region



    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class