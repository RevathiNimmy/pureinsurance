Option Strict Off
Option Explicit On
Imports System.Data
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Services_NET.Services")>
Public NotInheritable Class Services
    Implements IDisposable


#Region "Private Constants"
    Private Const ACClass As String = "Services"
#End Region


#Region "Private Variables"
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
    Private m_iBaseCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer


    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' Instance of Business class
    Private m_oSIRInsuranceFile As Business

    ' Instance of InsuranceFileSystem Business class
    Private m_oSIRInsuranceFileSystem As bSIRInsuranceFileSystem.Business

    ' Instance of InsuranceFolder Business class
    Private m_oSIRInsuranceFolder As bSIRInsuranceFolder.Business

    ' Instance of Solution Specific class
    Private m_oSolutionInsuranceFile As Object

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer

    Private m_bEvent As Boolean

    ' DataBase Attributes for Insurance_File
    'Developer Guide No. 17 
    Private m_lInsuranceFileStructureID As Object
    Private m_lInsuranceFileTypeID As Object
    Private m_vInsuranceFileStatusID As Object
    Private m_lInsuranceFileID As Object
    Private m_lInsuranceFolderCnt As Object
    Private m_sInsuranceRef As Object
    Private m_lProductID As Object
    Private m_lLeadInsurerCnt As Object
    Private m_lLeadAgentCnt As Object
    Private m_sLeadAgentCode As Object
    Private m_vLeadAgentPercent As Object
    Private m_lAccountHandlerCnt As Object
    Private m_lAccountExecutiveCnt As Object
    Private m_lInsuredCnt As Object
    Private m_iBusinessTypeID As Object
    Private m_vCollectTypeID As Object
    Private m_lCollectionFromCnt As Object
    'sj 19/07/2002 - start
    'Private m_iBranchID As Variant
    Private m_vSubBranchID As Object
    'sj 19/07/2002 - end
    Private m_vDateIssued As Object
    Private m_dtCoverStartDate As Object
    Private m_dtExpiryDate As Object
    Private m_dtRenewalDate As Object
    Private m_vRenewalMethodID As Object
    Private m_iRenewalFrequencyID As Object
    Private m_iIsReferredAtRenewal As Object
    'eck150500
    Private m_sRiskCode As String = ""
    Private m_vLapsedReasonID As Object
    Private m_vLapsedDate As Object
    Private m_vLapsedDescription As Object
    Private m_iIsReferredOnMta As Object
    Private m_iPolicyVersion As Integer
    Private m_vGeminiPolicyStatus As Object
    Private m_vGeminiBusinessType As Object
    Private m_vDeferredInd As Object
    Private m_vPolicyIgnore As Object
    Private m_vBrokerCnt As Object
    Private m_vRiskCodeID As Object
    'eck150500
    Private m_vRiskCode As Object
    Private m_vAnalysisCodeID As Object
    Private m_vPolicyDeductiblesID As Object
    Private m_vPolicyLimitsID As Object
    Private m_vProposalDate As Object
    Private m_vDiaryDate As Object
    Private m_vReviewDate As Object
    Private m_vRenewalDayNumber As Object
    Private m_vPolicyTypeId As Object
    Private m_vIndicator As Object
    Private m_vClause As Object
    Private m_vCover As Object
    Private m_vArea As Object
    Private m_vLongTermUndertakingDate As Object
    Private m_vRenewalStopCodeID As Object
    Private m_vVBSType As Object
    Private m_vVBSStatus As Object
    Private m_vIsInsurerRateTable As Object
    Private m_vIsRelatedPolicies As Object
    Private m_vIsRetainedDocuments As Object
    Private m_vSchemesPostcode As Object
    Private m_vPaidDirect As Object
    Private m_vScheme As Object
    Private m_vBrokerageAmount As Object
    Private m_vIsMinimumBrokerageFlag As Object
    Private m_vAnnualPremium As Object
    Private m_vThisPremium As Object
    Private m_vNetPremium As Object
    Private m_vCommissionAmount As Object
    Private m_vIPTableAmount As Object
    Private m_vIPTPercentage As Object
    Private m_vIsIPTOverridden As Object
    Private m_vTaxAmount As Object
    Private m_vVatableAmount As Object
    Private m_vVatPercentage As Object
    Private m_vVatAmount As Object
    Private m_vPaymentMethod As Object
    Private m_vUserDefinedDataID As Object
    Private m_vCommissionPercentage As Object 'sj 28/9/99
    'DC 14/07/00
    Private m_vInvariantKey As Object

    'TN20000818 - Doc Ref 10 (Start)
    Private m_vInsuredName As Object
    Private m_vAlternateReference As Object
    Private m_vIsClientInvoiced As Object
    Private m_vOldPolicyNumber As Object
    Private m_vQuoteExpiryDate As Object
    Private m_vAlternateAccountCnt As Object
    'TN20000818 - Doc Ref 10 (End)

    'TF190600
    ' No need to use variants here as
    ' will only be set if m_vRiskCodeID is valid
    Private m_lRiskGroupID As Integer
    Private m_sRiskGroup As String = ""

    ' Additional Properties from Lookups
    Private m_sInsuranceFileStructure As Object
    Private m_sInsuranceFileType As Object
    Private m_vInsuranceFileStatus As Object
    Private m_sSource As Object
    Private m_sProduct As Object
    Private m_sLeadInsurer As Object
    Private m_sLeadAgent As Object
    Private m_sAccountHandler As Object
    Private m_sAccountHandlerCode As Object
    Private m_sAccountExecutive As Object
    Private m_sInsured As Object
    Private m_sBusinessType As Object
    Private m_sCollectType As Object
    Private m_sCollectionFrom As Object
    'sj 19/07/2002 - start
    'Private m_sBranch As Variant
    Private m_vSubBranch As Object
    'sj 19/07/2002 - end
    Private m_sCurrencyCode As Object
    Private m_sLanguage As Object
    Private m_sRenewalMethod As Object
    Private m_sRenewalFrequency As Object
    Private m_sLapsedReason As Object
    Private m_sPolicyType As Object

    Private m_sLeadInsurerABICode As Object

    ' DataBase Attributes for Insurance_File_System
    Private m_lEndorsementCount As Object
    Private m_iCreatedByID As Object
    Private m_dtDateCreated As Object
    Private m_vModifiedByID As Object
    Private m_vLastModified As Object
    Private m_vLastTransDate As Object
    'Developer Guide No 101
    Private m_vLastTransTypeID As Object
    Private m_vLastTransDescription As Object
    Private m_vLastTransDebitCredit As Object
    Private m_vLastTransDocumentRef As Object
    Private m_vLastTransCoverStartDate As Object
    Private m_vLastTransExpiryDate As Object

    Private m_sLastTransType As Object

    ' DataBase Attributes for Insurance_Folder
    Private m_lInsuranceFolderID As Object
    Private m_lInsuranceHolderCnt As Object
    Private m_sInsuranceFolderCode As Object
    Private m_vInsuranceFolderDescription As Object
    Private m_vInceptionDate As Object
    Private m_vArcArchiveFolderID As Object
    Private m_vQuoteInsuranceRef As Object
    Private m_vNextInsuranceRef As Object
    Private m_vLastInsuranceRef As Object
    Private m_lRenewalCount As Object

    Dim m_lDiscountRecurringTypeID As Integer

    ' DataBase Attributes for MBPInsurance_File
    Private m_dAnnualPremium As Object
    Private m_dThisPremium As Object
    Private m_dTrailerSumInsured As Object
    Private m_iIsTrailerCovered As Object
    Private m_sVehicleUse As Object
    Private m_dMBPFee As Object
    Private m_vCreditRef As Object
    Private m_dTaxAmount As Object
    Private m_dCommissionAmount As Object
    Private m_vStPaulsRef As Object
    Private m_vNUInstallmentFee As Object
    Private m_vMBPInstallmentFee As Object
    Private m_vNURef As Object
    Private m_sUnderwritingOrAgency As String = ""

    ' Additional Properties from Lookups
    Private m_sInsuranceHolder As Object
    'Private m_sArcArchiveFolderCode As String

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    'Locking...
    Private m_oPMLock As bpmlock.User

    ' To see if used via S4B Data Transfer
    'DC 24/11/99
    Private m_iDataTransfer As Integer

    'Description for events...
    Private m_vEventDescription As Object
    ' AMB 24-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added
    Private m_vAnniversaryDate As Object

    Private m_vPolicyStyleID As Object
    Private m_vPolicyStatusID As Object
    Private m_vPolicyStatus As Object
    Private m_vInceptionTPI As Object
    Private m_vUnderwritingYearID As Object
    'FSA Phase III
    Private m_vFSACustomerCategoryID As Object
    Private m_vFSAContractLocationID As Object
    Private m_vFSAUnderwriterCnt As Object
    Private m_vFSATypeOfSaleID As Object
    Private m_vFSARenewalConsent As Object
    'FSA Phase IIIEnd
    ' CJB 100804 PN13723 Add two new SubAgent properties
    Private m_vSubAgentName As Object
    Private m_vSubAgentCode As Object

    'Policy Discount
    Private m_iDiscountReasonID As Integer
    Private m_vDiscountedPremium As Object
    Private m_vDiscountPercentage As Object
    Private m_iMatchDiscountedPremiumFlag As Integer

    Private m_lPutOnNextInstalmentRenewal As Object
    Private m_lAnniversaryCopy As Object

    'MKW 150606
    Private m_lCountryId As Object

    Private m_vPolicyDeductibles As Object

    Private m_vPolicyLimits As Object

    'TMP
    Private m_iLeadAgentAllowCommission As Integer
    Private m_iSubAgentAllowCommission As Integer

    'MKW PN31153 Contract Certainty. START
    Private m_vCCTermsAgreed As Object
    Private m_vCCTermsAgreedDate As Object
    Private m_vCCInceptionDate As Object
    Private m_vCCPolicyDocumentsIssuedDate As Object
    Private m_vCCPolicyDocumentCorrect As Object
    Private m_vCCErrorNotificationDate As Object

    'PN38002 add new field to hold renewal premium at point of MTA quote
    Private m_vRenewalPremium As Object
    Private m_dRenewalPremium As Object

    Private m_vRiskTransferEditable As Object
    Private m_vRiskTransferAgreement As Object

    '1.12 WR25
    Private m_vRenewalProductID As Object
    Private m_vOriginalProductID As Object

    Private m_vCurrencyToBaseXRate As Object

    'RFC-PLICO14 - Amit
    Private m_vManualDiscountPercentage As Object

    'WPR 63
    Private m_vQuoteStatus As Integer
    Private m_vQuoteVersion As Integer
    Private m_vBaseInsuranceFolder As Integer

    'WPR73-74
    Private m_vContactuserId As Integer
    Private m_iMTAReasonId As Integer
    Private m_vNumberOfFleetVehicles As Object
    Private m_nIsMarketPlacePolicy As Integer = 0
    Private m_sCoInsPlacement As String
    Private m_oCollectionFrequencyID As Object
    Private m_oPaymentTermsID As Object
    Private m_iCorrespondenceType As Integer
    Private m_iDefaultPreferredCorrespondence As Integer
    Private m_bIsAgentCorrepondence As Boolean
    Private m_vSenderEmail As Object
    Private m_vReceiverEmail As Object
    Private m_vOriginalInsuranceFileTypeId As Object
#End Region
#Region "Public Properties"

    Public Property CoInsPlacement() As String
        Get
            Return m_sCoInsPlacement
        End Get
        Set(ByVal value As String)

            m_sCoInsPlacement = value
        End Set
    End Property


    Public Property NumberOfFleetVehicles() As Integer
        Get
            Return m_vNumberOfFleetVehicles
        End Get
        Set(ByVal vNumberOfFleetVehicles As Integer)
            m_vNumberOfFleetVehicles = vNumberOfFleetVehicles
        End Set
    End Property

    Public Property ContactuserId() As Integer
        Get
            Return m_vContactuserId
        End Get
        Set(ByVal value As Integer)

            m_vContactuserId = value
        End Set
    End Property

    Public Property MTAReasonId() As Integer
        Get
            Return m_iMTAReasonId
        End Get
        Set(ByVal value As Integer)
            m_iMTAReasonId = value
        End Set
    End Property

    Public Property CCTermsAgreed() As Object
        Get
            Return m_vCCTermsAgreed
        End Get
        Set(ByVal Value As Object)


            m_vCCTermsAgreed = Value
        End Set
    End Property
    Public Property CCTermsAgreedDate() As Object
        Get
            Return m_vCCTermsAgreedDate
        End Get
        Set(ByVal Value As Object)


            m_vCCTermsAgreedDate = Value
        End Set
    End Property
    Public Property CCInceptionDate() As Object
        Get
            Return m_vCCInceptionDate
        End Get
        Set(ByVal Value As Object)


            m_vCCInceptionDate = Value
        End Set
    End Property
    Public Property CCPolicyDocumentsIssuedDate() As Object
        Get
            Return m_vCCPolicyDocumentsIssuedDate
        End Get
        Set(ByVal Value As Object)


            m_vCCPolicyDocumentsIssuedDate = Value
        End Set
    End Property
    Public Property CCPolicyDocumentCorrect() As Object
        Get
            Return m_vCCPolicyDocumentCorrect
        End Get
        Set(ByVal Value As Object)


            m_vCCPolicyDocumentCorrect = Value
        End Set
    End Property
    Public Property CCErrorNotificationDate() As Object
        Get
            Return m_vCCErrorNotificationDate
        End Get
        Set(ByVal Value As Object)


            m_vCCErrorNotificationDate = Value
        End Set
    End Property

    Public Property DiscountRecurringTypeId() As Integer
        Get
            Return m_lDiscountRecurringTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lDiscountRecurringTypeID = Value
        End Set
    End Property

    Public Property AnniversaryCopy() As Integer
        Get

            Return CInt(m_lAnniversaryCopy)
        End Get
        Set(ByVal Value As Integer)

            m_lAnniversaryCopy = Value
        End Set
    End Property

    Public Property LeadAgentAllowCommission() As Integer
        Get
            Return m_iLeadAgentAllowCommission
        End Get
        Set(ByVal Value As Integer)
            m_iLeadAgentAllowCommission = Value
        End Set
    End Property

    Public Property SubAgentAllowCommission() As Integer
        Get
            Return m_iSubAgentAllowCommission
        End Get
        Set(ByVal Value As Integer)
            m_iSubAgentAllowCommission = Value
        End Set
    End Property

    Public Property PutOnNextInstalmentRenewal() As Integer
        Get

            Return CInt(m_lPutOnNextInstalmentRenewal)
        End Get
        Set(ByVal Value As Integer)

            m_lPutOnNextInstalmentRenewal = Value
        End Set
    End Property

    Public Property DiscountReasonID() As Integer
        Get
            Return m_iDiscountReasonID
        End Get
        Set(ByVal Value As Integer)
            m_iDiscountReasonID = Value
        End Set
    End Property

    Public Property DiscountedPremium() As Object
        Get
            Return m_vDiscountedPremium
        End Get
        Set(ByVal Value As Object)

            m_vDiscountedPremium = Value
        End Set
    End Property

    Public Property DiscountPercentage() As Object
        Get
            Return m_vDiscountPercentage
        End Get
        Set(ByVal Value As Object)

            m_vDiscountPercentage = Value
        End Set
    End Property

    Public Property MatchDiscountedPremiumFlag() As Integer
        Get
            Return m_iMatchDiscountedPremiumFlag
        End Get
        Set(ByVal Value As Integer)
            m_iMatchDiscountedPremiumFlag = Value
        End Set
    End Property

    Public Property UnderwritingYearID() As Object
        Get
            Return m_vUnderwritingYearID
        End Get
        Set(ByVal Value As Object)

            m_vUnderwritingYearID = Value
        End Set
    End Property

    Public Property InceptionTPI() As Object
        Get
            Return m_vInceptionTPI
        End Get
        Set(ByVal Value As Object)

            m_vInceptionTPI = Value
        End Set
    End Property

    Public Property FSACustomerCategoryID() As Object
        Get
            Return m_vFSACustomerCategoryID
        End Get
        Set(ByVal Value As Object)

            m_vFSACustomerCategoryID = Value
        End Set
    End Property
    Public Property FSAContractLocationID() As Object
        Get
            Return m_vFSAContractLocationID
        End Get
        Set(ByVal Value As Object)

            m_vFSAContractLocationID = Value
        End Set
    End Property
    Public Property FSAUnderwriterCnt() As Object
        Get
            Return m_vFSAUnderwriterCnt
        End Get
        Set(ByVal Value As Object)

            m_vFSAUnderwriterCnt = Value
        End Set
    End Property
    Public Property FSATypeOfSaleID() As Object
        Get
            Return m_vFSATypeOfSaleID
        End Get
        Set(ByVal Value As Object)

            m_vFSATypeOfSaleID = Value
        End Set
    End Property
    Public Property FSARenewalConsent() As Object
        Get
            Return m_vFSARenewalConsent
        End Get
        Set(ByVal Value As Object)

            m_vFSARenewalConsent = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

            If Value > 0 Then
                'DC 24/11/99
                If m_iDataTransfer = gPMConstants.PMEReturnCode.PMFalse Then
                    m_lReturn = GetDetails()
                End If
            End If

        End Set
    End Property

    Public Property InsuranceFileStructureID() As Object
        Get

            Return m_lInsuranceFileStructureID

        End Get
        Set(ByVal Value As Object)

            m_lInsuranceFileStructureID = Value

        End Set
    End Property

    Public Property InsuranceFileStructure() As Object
        Get

            Return Trim(m_sInsuranceFileStructure)

        End Get
        Set(ByVal Value As Object)

            Dim lInsuranceFileStructureID As Integer

            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sInsuranceFileStructure = Value

                m_lInsuranceFileStructureID = Nothing
            ElseIf (Value = "") Then

                m_sInsuranceFileStructure = Value

                m_lInsuranceFileStructureID = Nothing
            Else

                m_sInsuranceFileStructure = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupInsuranceFileStructure, v_sCode:=m_sInsuranceFileStructure, v_dtEffectiveDate:=DateTime.Now, r_lID:=lInsuranceFileStructureID)

                m_lInsuranceFileStructureID = lInsuranceFileStructureID
            End If

        End Set
    End Property

    Public Property InsuranceFileTypeID() As Object
        Get

            Return m_lInsuranceFileTypeID

        End Get
        Set(ByVal Value As Object)

            m_lInsuranceFileTypeID = Value

        End Set
    End Property

    Public Property InsuranceFileType() As Object
        Get

            Return Trim(m_sInsuranceFileType)

        End Get
        Set(ByVal Value As Object)

            Dim lInsuranceFileTypeID As Integer

            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sInsuranceFileType = Value

                m_lInsuranceFileTypeID = Nothing
            ElseIf (Value = "") Then

                m_sInsuranceFileType = Value

                m_lInsuranceFileTypeID = Nothing
            Else

                m_sInsuranceFileType = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupInsFileType, v_sCode:=m_sInsuranceFileType, v_dtEffectiveDate:=DateTime.Now, r_lID:=lInsuranceFileTypeID)

                m_lInsuranceFileTypeID = lInsuranceFileTypeID
            End If

        End Set
    End Property

    Public Property InsuranceFileStatusID() As Object
        Get

            Return m_vInsuranceFileStatusID

        End Get
        Set(ByVal Value As Object)

            m_vInsuranceFileStatusID = Value

        End Set
    End Property

    Public Property InsuranceFileStatus() As Object
        Get

            If Convert.IsDBNull(m_vInsuranceFileStatus) Then
                Return String.Empty
            ElseIf String.IsNullOrEmpty(m_vInsuranceFileStatus) Then
                Return String.Empty
            End If
            Return m_vInsuranceFileStatus.Trim()
        End Get
        Set(ByVal Value As Object)

            Dim lInsuranceFileStatusID As Integer

            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_vInsuranceFileStatus = Value

                m_vInsuranceFileStatusID = Nothing
            ElseIf Value = "" Then

                m_vInsuranceFileStatus = Value

                m_vInsuranceFileStatusID = Nothing
            Else

                m_vInsuranceFileStatus = CStr(Value).Trim()

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupInsFileStatus, v_sCode:=m_vInsuranceFileStatus, v_dtEffectiveDate:=DateTime.Now, r_lID:=lInsuranceFileStatusID)

                m_vInsuranceFileStatusID = lInsuranceFileStatusID
            End If

        End Set
    End Property

    Public Property InsuranceFileID() As Object
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Object)

            m_lInsuranceFileID = Value

        End Set
    End Property

    Public Property SourceID() As Object
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Object)

            m_iSourceID = CInt(Value)

        End Set
    End Property

    Public Property Source() As Object
        Get

            Return Trim(m_sSource)

        End Get
        Set(ByVal Value As Object)

            Dim lSourceID As Integer

            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then
                m_sSource = Value

                m_iSourceID = Nothing
            ElseIf (Value = "") Then
                m_sSource = Value

                m_iSourceID = Nothing
            Else
                m_sSource = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupSource, v_sCode:=m_sSource, v_dtEffectiveDate:=DateTime.Now, r_lID:=lSourceID)

                m_iSourceID = lSourceID
            End If

        End Set
    End Property

    Public Property InsuranceFolderCnt() As Object
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Object)

            m_lInsuranceFolderCnt = Value

        End Set
    End Property

    Public Property InsuranceRef() As Object
        Get
            Return Trim(m_sInsuranceRef)

        End Get
        Set(ByVal Value As Object)

            m_sInsuranceRef = Trim(Value)

        End Set
    End Property

    Public Property ProductID() As Object
        Get

            Return m_lProductID

        End Get
        Set(ByVal Value As Object)

            m_lProductID = Value

        End Set
    End Property


    Public Property Product() As Object
        Get

            Return Trim(m_sProduct)

        End Get
        Set(ByVal Value As Object)

            Dim lProductID As Integer

            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sProduct = Value

                m_lProductID = Nothing
            ElseIf (Value = "") Then

                m_sProduct = Value

                m_lProductID = Nothing
            Else

                m_sProduct = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupProduct, v_sCode:=m_sProduct, v_dtEffectiveDate:=DateTime.Now, r_lID:=lProductID)

                m_lProductID = lProductID
            End If

        End Set
    End Property

    Public Property LeadInsurerCnt() As Object
        Get

            Return m_lLeadInsurerCnt

        End Get
        Set(ByVal Value As Object)

            m_lLeadInsurerCnt = Value

        End Set
    End Property

    Public Property LeadInsurer() As Object
        Get

            Return Trim(m_sLeadInsurer)

        End Get
        Set(ByVal Value As Object)


            m_sLeadInsurer = Value

        End Set
    End Property

    Public Property LeadInsurerABICode() As Object
        Get

            Return Trim(m_sLeadInsurerABICode)

        End Get
        Set(ByVal Value As Object)

            Dim lInsurerCnt As Object = Nothing
            Dim sShortName As String = String.Empty
            Dim sResolvedName As String = String.Empty



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sLeadInsurerABICode = Value


                m_lLeadInsurerCnt = Nothing
            ElseIf (Value = "") Then

                m_sLeadInsurerABICode = Value


                m_lLeadInsurerCnt = Nothing
            Else

                m_sLeadInsurerABICode = Trim(Value)

                m_lReturn = GetPartyCntFromABI(v_sABICode:=m_sLeadInsurerABICode, r_lPartyCnt:=lInsurerCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)



                m_lLeadInsurerCnt = lInsurerCnt
                m_sLeadInsurer = sResolvedName
            End If

        End Set
    End Property

    Public WriteOnly Property BrokerAbiId() As String
        Set(ByVal Value As String)

            Dim lPartyCnt As Integer


            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Or Value.Trim() = "" Then
                Exit Property
            End If



            m_lLeadAgentCnt = Nothing
            m_lReturn = bUnderwritingBranchFunc.GetPartyCntFromBrokerAbiId(v_sBrokerAbiId:=Value, v_oDatabase:=m_oDatabase, v_sUsername:=m_sUsername, r_lPartyCnt:=lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Property
            End If
            If lPartyCnt > 0 Then

                m_lLeadAgentCnt = lPartyCnt
            End If

        End Set
    End Property
    Public Property LeadAgentCnt() As Object
        Get

            Return m_lLeadAgentCnt

        End Get
        Set(ByVal Value As Object)



            m_lLeadAgentCnt = Value

        End Set
    End Property

    Public Property LeadAgentCode() As Object
        Get

            Return m_sLeadAgentCode

        End Get
        Set(ByVal Value As Object)


            m_sLeadAgentCode = gPMFunctions.ToSafeString(Value)

        End Set
    End Property

    Public Property LeadAgent() As Object
        Get

            Return Trim(m_sLeadAgent)

        End Get
        Set(ByVal Value As Object)


            m_sLeadAgent = Value

        End Set
    End Property

    Public Property LeadAgentPercent() As Object
        Get

            Return m_vLeadAgentPercent

        End Get
        Set(ByVal Value As Object)



            m_vLeadAgentPercent = Value

        End Set
    End Property

    ' CJB 100804 PN13723 Start
    Public ReadOnly Property SubAgentName() As Object
        Get
            Return m_vSubAgentName
        End Get
    End Property

    Public ReadOnly Property SubAgentCode() As Object
        Get
            Return m_vSubAgentCode
        End Get
    End Property
    ' CJB 100804 PN13723 End

    Public Property AccountHandlerCnt() As Object
        Get

            Return m_lAccountHandlerCnt

        End Get
        Set(ByVal Value As Object)



            m_lAccountHandlerCnt = Value

        End Set
    End Property

    Public Property AccountExecutiveCnt() As Object
        Get

            Return m_lAccountExecutiveCnt

        End Get
        Set(ByVal Value As Object)



            m_lAccountExecutiveCnt = Value

        End Set
    End Property

    Public Property AccountHandler() As Object
        Get

            Return Trim(m_sAccountHandler)

        End Get
        Set(ByVal Value As Object)


            m_sAccountHandler = Value

        End Set
    End Property

    Public Property AccountHandlerCode() As Object
        Get

            Return Trim(m_sAccountHandlerCode)

        End Get
        Set(ByVal Value As Object)


            m_sAccountHandlerCode = Value

        End Set
    End Property

    Public Property AccountExecutive() As Object
        Get

            Return Trim(m_sAccountExecutive)

        End Get
        Set(ByVal Value As Object)


            m_sAccountExecutive = Value

        End Set
    End Property

    Public Property InsuredCnt() As Object
        Get

            Return m_lInsuredCnt

        End Get
        Set(ByVal Value As Object)



            m_lInsuredCnt = Value

        End Set
    End Property

    Public Property Insured() As Object
        Get

            Return Trim(m_sInsured)

        End Get
        Set(ByVal Value As Object)


            m_sInsured = Value

        End Set
    End Property

    Public Property BusinessTypeID() As Object
        Get

            Return m_iBusinessTypeID

        End Get
        Set(ByVal Value As Object)



            m_iBusinessTypeID = Value

        End Set
    End Property

    Public Property BusinessType() As Object
        Get

            Return Trim(m_sBusinessType)

        End Get
        Set(ByVal Value As Object)

            Dim lBusinessTypeID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sBusinessType = Value


                m_iBusinessTypeID = Nothing
            ElseIf (Value = "") Then

                m_sBusinessType = Value


                m_iBusinessTypeID = Nothing
            Else

                m_sBusinessType = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupBusinessType, v_sCode:=m_sBusinessType, v_dtEffectiveDate:=DateTime.Now, r_lID:=lBusinessTypeID)


                m_iBusinessTypeID = lBusinessTypeID
            End If

        End Set
    End Property

    Public Property CollectTypeID() As Object
        Get

            Return m_vCollectTypeID

        End Get
        Set(ByVal Value As Object)



            m_vCollectTypeID = Value

        End Set
    End Property

    Public Property CollectType() As Object
        Get

            Return Trim(m_sCollectType)

        End Get
        Set(ByVal Value As Object)

            Dim lCollectTypeID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sCollectType = Value


                m_vCollectTypeID = Nothing
            ElseIf (Value = "") Then

                m_sCollectType = Value



                m_vCollectTypeID = Nothing
            Else

                m_sCollectType = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupCollectType, v_sCode:=m_sCollectType, v_dtEffectiveDate:=DateTime.Now, r_lID:=lCollectTypeID)


                m_vCollectTypeID = lCollectTypeID
            End If

        End Set
    End Property

    Public Property CollectionFromCnt() As Object
        Get

            Return m_lCollectionFromCnt

        End Get
        Set(ByVal Value As Object)



            m_lCollectionFromCnt = Value

        End Set
    End Property

    Public Property CollectionFrom() As Object
        Get

            Return Trim(m_sCollectionFrom)

        End Get
        Set(ByVal Value As Object)


            m_sCollectionFrom = Value

        End Set
    End Property

    'sj 19/07/2002 - start
    'Public Property Let BranchID(iBranchID As Variant)
    '
    '    m_iBranchID = iBranchID
    '
    'End Property
    'Public Property Get BranchID() As Variant
    '
    '    BranchID = m_iBranchID
    '
    'End Property


    Public Property SubBranchID() As Object
        Get

            Return m_vSubBranchID

        End Get
        Set(ByVal Value As Object)



            m_vSubBranchID = Value

        End Set
    End Property

    Public Property ManualDiscountPercentage() As Object
        Get
            Return m_vManualDiscountPercentage
        End Get
        Set(ByVal Value As Object)


            m_vManualDiscountPercentage = Value
        End Set
    End Property

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

    Public Property SubBranch() As Object
        Get

            Return Trim(m_vSubBranch)

        End Get
        Set(ByVal Value As Object)

            Dim lSubBranchID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_vSubBranch = Value



                m_vSubBranchID = Nothing
            ElseIf (Value = "") Then

                m_vSubBranch = Value



                m_vSubBranchID = Nothing
            Else

                m_vSubBranch = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupSubBranch, v_sCode:=m_vSubBranch, v_dtEffectiveDate:=DateTime.Now, r_lID:=lSubBranchID)


                m_vSubBranchID = lSubBranchID
            End If

        End Set
    End Property
    'sj 19/07/2002 - end

    Public Property CurrencyID() As Object
        Get

            Return m_iCurrencyID

        End Get
        Set(ByVal Value As Object)


            m_iCurrencyID = CInt(Value)

        End Set
    End Property

    Public Property BaseCurrencyID() As Object
        Get

            Return m_iBaseCurrencyID

        End Get
        Set(ByVal Value As Object)


            m_iBaseCurrencyID = CInt(Value)

        End Set
    End Property

    Public Property CurrencyCode() As Object
        Get

            Return Trim(m_sCurrencyCode)

        End Get
        Set(ByVal Value As Object)

            Dim lCurrencyID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sCurrencyCode = Value

                m_iCurrencyID = Nothing
            ElseIf (Value = "") Then

                m_sCurrencyCode = Value

                m_iCurrencyID = Nothing
            Else

                m_sCurrencyCode = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gPMConstants.PMLookupCurrency, v_sCode:=m_sCurrencyCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=lCurrencyID)

                m_iCurrencyID = lCurrencyID
            End If

        End Set
    End Property

    Public Property LanguageID() As Object
        Get

            Return m_iLanguageID

        End Get
        Set(ByVal Value As Object)


            m_iLanguageID = CInt(Value)

        End Set
    End Property

    Public Property Language() As Object
        Get

            Return Trim(m_sLanguage)

        End Get
        Set(ByVal Value As Object)

            Dim lLanguageID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sLanguage = Value

                m_iLanguageID = Nothing
            ElseIf (Value = "") Then

                m_sLanguage = Value

                m_iLanguageID = Nothing
            Else

                m_sLanguage = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gPMConstants.PMLookupLanguage, v_sCode:=m_sLanguage, v_dtEffectiveDate:=DateTime.Now, r_lID:=lLanguageID)

                m_iLanguageID = lLanguageID
            End If

        End Set
    End Property

    Public Property DateIssued() As Object
        Get

            Return m_vDateIssued

        End Get
        Set(ByVal Value As Object)



            m_vDateIssued = Value

        End Set
    End Property

    Public Property CoverStartDate() As Object
        Get

            Return m_dtCoverStartDate

        End Get
        Set(ByVal Value As Object)



            m_dtCoverStartDate = Value

        End Set
    End Property

    Public Property ExpiryDate() As Object
        Get

            Return m_dtExpiryDate

        End Get
        Set(ByVal Value As Object)



            m_dtExpiryDate = Value

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
    Public Property RenewalDate() As Object
        Get

            Return m_dtRenewalDate

        End Get
        Set(ByVal Value As Object)



            m_dtRenewalDate = Value

        End Set
    End Property

    Public Property RenewalMethodID() As Object
        Get

            Return m_vRenewalMethodID

        End Get
        Set(ByVal Value As Object)



            m_vRenewalMethodID = Value

        End Set
    End Property

    Public Property RenewalMethod() As Object
        Get

            Return Trim(m_sRenewalMethod)

        End Get
        Set(ByVal Value As Object)

            Dim lRenewalMethodID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sRenewalMethod = Value



                m_vRenewalMethodID = Nothing
            ElseIf (Value = "") Then

                m_sRenewalMethod = Value



                m_vRenewalMethodID = Nothing
            Else

                m_sRenewalMethod = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupRenewalMethod, v_sCode:=m_sRenewalMethod, v_dtEffectiveDate:=DateTime.Now, r_lID:=lRenewalMethodID)


                m_vRenewalMethodID = lRenewalMethodID
            End If

        End Set
    End Property

    Public Property RenewalFrequencyID() As Object
        Get

            Return m_iRenewalFrequencyID

        End Get
        Set(ByVal Value As Object)



            m_iRenewalFrequencyID = Value

        End Set
    End Property

    Public Property RenewalFrequency() As Object
        Get

            Return Trim(m_sRenewalFrequency)

        End Get
        Set(ByVal Value As Object)

            Dim lRenewalFrequencyID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sRenewalFrequency = Value



                m_iRenewalFrequencyID = Nothing
            ElseIf (Value = "") Then

                m_sRenewalFrequency = Value



                m_iRenewalFrequencyID = Nothing
            Else

                m_sRenewalFrequency = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupRenewalFrequency, v_sCode:=m_sRenewalFrequency, v_dtEffectiveDate:=DateTime.Now, r_lID:=lRenewalFrequencyID)


                m_iRenewalFrequencyID = lRenewalFrequencyID
            End If

        End Set
    End Property

    Public Property IsReferredAtRenewal() As Object
        Get

            Return m_iIsReferredAtRenewal

        End Get
        Set(ByVal Value As Object)



            m_iIsReferredAtRenewal = Value

        End Set
    End Property

    Public Property LapsedReasonID() As Object
        Get

            Return m_vLapsedReasonID

        End Get
        Set(ByVal Value As Object)



            m_vLapsedReasonID = Value

        End Set
    End Property

    Public Property LapsedReason() As Object
        Get

            Return Trim(m_sLapsedReason)

        End Get
        Set(ByVal Value As Object)

            Dim lLapsedReasonID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sLapsedReason = Value



                m_vLapsedReasonID = Nothing
            ElseIf (Value = "") Then

                m_sLapsedReason = Value



                m_vLapsedReasonID = Nothing
            Else

                m_sLapsedReason = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupLapsedReason, v_sCode:=m_sLapsedReason, v_dtEffectiveDate:=DateTime.Now, r_lID:=lLapsedReasonID)


                m_vLapsedReasonID = lLapsedReasonID
            End If

        End Set
    End Property

    Public Property LapsedDate() As Object
        Get

            Return m_vLapsedDate

        End Get
        Set(ByVal Value As Object)



            m_vLapsedDate = Value

        End Set
    End Property

    Public Property LapsedDescription() As Object
        Get

            Return m_vLapsedDescription

        End Get
        Set(ByVal Value As Object)



            m_vLapsedDescription = Value

        End Set
    End Property

    Public Property IsReferredOnMta() As Object
        Get

            Return m_iIsReferredOnMta

        End Get
        Set(ByVal Value As Object)



            m_iIsReferredOnMta = Value

        End Set
    End Property

    Public Property PolicyVersion() As Object
        Get

            Return m_iPolicyVersion

        End Get
        Set(ByVal Value As Object)


            m_iPolicyVersion = CInt(Value)

        End Set
    End Property

    Public Property GeminiPolicyStatus() As Object
        Get

            Return m_vGeminiPolicyStatus

        End Get
        Set(ByVal Value As Object)



            m_vGeminiPolicyStatus = Value

        End Set
    End Property

    Public Property GeminiBusinessType() As Object
        Get

            Return m_vGeminiBusinessType

        End Get
        Set(ByVal Value As Object)



            m_vGeminiBusinessType = Value

        End Set
    End Property

    Public Property DeferredInd() As Object
        Get

            Return m_vDeferredInd

        End Get
        Set(ByVal Value As Object)



            m_vDeferredInd = Value

        End Set
    End Property

    Public Property PolicyIgnore() As Object
        Get

            Return m_vPolicyIgnore

        End Get
        Set(ByVal Value As Object)



            m_vPolicyIgnore = Value

        End Set
    End Property

    Public Property BrokerCnt() As Object
        Get

            Return m_vBrokerCnt

        End Get
        Set(ByVal Value As Object)



            m_vBrokerCnt = Value

        End Set
    End Property

    Public Property RiskCodeID() As Object
        Get

            Return m_vRiskCodeID

        End Get
        Set(ByVal Value As Object)



            m_vRiskCodeID = Value

        End Set
    End Property
    'eck150500
    Public Property RiskCode() As String
        Get

            Return m_sRiskCode.Trim()

            'eck150500
        End Get
        Set(ByVal Value As String)

            Dim lRiskCodeID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sRiskCode = CStr(Value)



                m_vRiskCodeID = Nothing
            ElseIf (CStr(Value) = "") Then

                m_sRiskCode = CStr(Value)



                m_vRiskCodeID = Nothing
            Else

                m_sRiskCode = CStr(Value).Trim()

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="risk_code", v_sCode:=m_sRiskCode, v_dtEffectiveDate:=DateTime.Now, r_lID:=lRiskCodeID)


                m_vRiskCodeID = lRiskCodeID
            End If

        End Set
    End Property

    ' TF190600
    Public Property RiskGroupID() As Integer
        Get

            Return m_lRiskGroupID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGroupID = Value

            m_lReturn = m_oLookup.GetCodeFromID(v_sTableName:="risk_group", v_lID:=Value, r_sCode:=m_sRiskGroup)

        End Set
    End Property

    Public Property RiskGroup() As String
        Get

            Return m_sRiskGroup.Trim()

            'eck150500
        End Get
        Set(ByVal Value As String)

            Dim lRiskGroupID As Integer

            If Value = "" Then
                m_sRiskGroup = Value
                m_lRiskGroupID = 0
            Else
                m_sRiskGroup = Value.Trim()

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="risk_group", v_sCode:=m_sRiskGroup, v_dtEffectiveDate:=DateTime.Now, r_lID:=lRiskGroupID)

                m_lRiskGroupID = lRiskGroupID
            End If

        End Set
    End Property

    Public Property AnalysisCodeId() As Object
        Get

            Return m_vAnalysisCodeID

        End Get
        Set(ByVal Value As Object)



            If (Convert.IsDBNull(Value) Or Informations.IsNothing(Value)) Or (CDbl(Value) <= 0) Then



                m_vAnalysisCodeID = Nothing
            Else


                m_vAnalysisCodeID = Value
            End If

        End Set
    End Property

    Public Property PolicyDeductiblesId() As Object
        Get

            Return m_vPolicyDeductiblesID

        End Get
        Set(ByVal Value As Object)



            If (Convert.IsDBNull(Value) Or Informations.IsNothing(Value)) Or (CDbl(Value) <= 0) Then



                m_vPolicyDeductiblesID = Nothing
            Else


                m_vPolicyDeductiblesID = Value
            End If

        End Set
    End Property


    Public Property PolicyLimitsId() As Object
        Get

            Return m_vPolicyLimitsID

        End Get
        Set(ByVal Value As Object)



            If (Convert.IsDBNull(Value) Or Informations.IsNothing(Value)) Or (CDbl(Value) <= 0) Then



                m_vPolicyLimitsID = Nothing
            Else


                m_vPolicyLimitsID = Value
            End If

        End Set
    End Property

    Public ReadOnly Property PolicyDeductibles() As Object
        Get

            If Not Object.Equals(PolicyDeductiblesId, Nothing) Then

                If Not (Convert.IsDBNull(PolicyDeductiblesId) Or Informations.IsNothing(PolicyDeductiblesId)) Then
                    'Get from DB

                    'Developer Guide No. 98

                    m_lReturn = m_oSIRInsuranceFile.GetPolicyDeductibleDesc(v_iPolicyDeductiblesId:=PolicyDeductiblesId, v_vPolicyDeductibles:=m_vPolicyDeductibles)
                End If
            End If

            Return m_vPolicyDeductibles

        End Get
    End Property
    Public ReadOnly Property PolicyLimits() As Object
        Get


            If Not Object.Equals(PolicyLimitsId, Nothing) Then

                If Not (Convert.IsDBNull(PolicyLimitsId) Or Informations.IsNothing(PolicyLimitsId)) Then
                    'Get from DB

                    'Developer Guide No. 98

                    m_lReturn = m_oSIRInsuranceFile.GetPolicyLimitsDesc(v_iPolicyLimitsId:=PolicyLimitsId, v_vPolicyLimits:=m_vPolicyLimits)

                End If
            End If

            Return m_vPolicyLimits

        End Get
    End Property

    Public Property ProposalDate() As Object
        Get

            Return m_vProposalDate

        End Get
        Set(ByVal Value As Object)



            m_vProposalDate = Value

        End Set
    End Property

    Public Property DiaryDate() As Object
        Get

            Return m_vDiaryDate

        End Get
        Set(ByVal Value As Object)



            m_vDiaryDate = Value

        End Set
    End Property

    Public Property ReviewDate() As Object
        Get

            Return m_vReviewDate

        End Get
        Set(ByVal Value As Object)



            m_vReviewDate = Value

        End Set
    End Property

    Public Property RenewalDayNumber() As Object
        Get

            Return m_vRenewalDayNumber

        End Get
        Set(ByVal Value As Object)



            m_vRenewalDayNumber = Value

        End Set
    End Property

    Public Property PolicyType() As Object
        Get

            Return Trim(m_sPolicyType)

        End Get
        Set(ByVal Value As Object)

            Dim lPolicyTypeID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sPolicyType = Value



                m_vPolicyTypeId = Nothing
            ElseIf (Value = "") Then

                m_sPolicyType = Value



                m_vPolicyTypeId = Nothing
            Else

                m_sPolicyType = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="policy_type", v_sCode:=m_sPolicyType, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPolicyTypeID)


                m_vPolicyTypeId = lPolicyTypeID
            End If

        End Set
    End Property

    Public Property PolicyTypeId() As Object
        Get

            Return m_vPolicyTypeId

        End Get
        Set(ByVal Value As Object)



            m_vPolicyTypeId = Value

        End Set
    End Property

    Public Property Indicator() As Object
        Get

            Return m_vIndicator

        End Get
        Set(ByVal Value As Object)



            m_vIndicator = Value

        End Set
    End Property

    Public Property Clause() As Object
        Get

            Return m_vClause

        End Get
        Set(ByVal Value As Object)



            m_vClause = Value

        End Set
    End Property

    Public Property Cover() As Object
        Get

            Return m_vCover

        End Get
        Set(ByVal Value As Object)



            m_vCover = Value

        End Set
    End Property

    Public Property Area() As Object
        Get

            Return m_vArea

        End Get
        Set(ByVal Value As Object)



            m_vArea = Value

        End Set
    End Property

    Public Property LongTermUndertakingDate() As Object
        Get

            Return m_vLongTermUndertakingDate

        End Get
        Set(ByVal Value As Object)



            m_vLongTermUndertakingDate = Value

        End Set
    End Property

    Public Property RenewalStopCodeId() As Object
        Get

            Return m_vRenewalStopCodeID

        End Get
        Set(ByVal Value As Object)



            m_vRenewalStopCodeID = Value

        End Set
    End Property

    Public Property AnniversaryDate() As Object
        Get
            ' AMB 27-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added

            Return m_vAnniversaryDate

        End Get
        Set(ByVal Value As Object)
            ' AMB 27-Oct-03: 1.8.6 MMM True Monthly Policies - anniversary_date added



            m_vAnniversaryDate = Value

        End Set
    End Property

    Public Property PolicyStatusID() As Object
        Get

            Return m_vPolicyStatusID

        End Get
        Set(ByVal Value As Object)



            m_vPolicyStatusID = Value

        End Set
    End Property

    Public Property PolicyStatus() As Object
        Get

            Return Trim(m_vPolicyStatus)

        End Get
        Set(ByVal Value As Object)

            Dim lPolicyStatusID As Integer



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                'Developer Guide No. 98
                m_vPolicyStatus = Value



                m_vPolicyStatusID = Nothing
            ElseIf (Value = "") Then

                'Developer Guide No. 98
                m_vPolicyStatus = Value



                m_vPolicyStatusID = Nothing
            Else

                'Developer Guide No. 98
                m_vPolicyStatus = Trim(Value)

                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPolicyStatus, v_sCode:=m_vPolicyStatus, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPolicyStatusID)


                m_vPolicyStatusID = lPolicyStatusID
            End If

        End Set
    End Property
    Public Property PolicyStyleID() As Object
        Get
            Return m_vPolicyStyleID
        End Get
        Set(ByVal Value As Object)


            m_vPolicyStyleID = Value
        End Set
    End Property
    'PN38002 add new field to hold renewal premium at point of MTA quote
    Public Property RenewalPremium() As Object
        Get

            Return m_vRenewalPremium

        End Get
        Set(ByVal Value As Object)



            m_vRenewalPremium = Value

        End Set
    End Property

    Public Property VBSType() As Object
        Get

            Return m_vVBSType

        End Get
        Set(ByVal Value As Object)



            m_vVBSType = Value

        End Set
    End Property

    Public Property VBSStatus() As Object
        Get

            Return m_vVBSStatus

        End Get
        Set(ByVal Value As Object)



            m_vVBSStatus = Value

        End Set
    End Property

    Public Property IsInsurerRateTable() As Object
        Get

            Return m_vIsInsurerRateTable

        End Get
        Set(ByVal Value As Object)



            m_vIsInsurerRateTable = Value

        End Set
    End Property

    Public Property IsRelatedPolicies() As Object
        Get

            Return m_vIsRelatedPolicies

        End Get
        Set(ByVal Value As Object)



            m_vIsRelatedPolicies = Value

        End Set
    End Property

    Public Property IsRetainedDocuments() As Object
        Get

            Return m_vIsRetainedDocuments

        End Get
        Set(ByVal Value As Object)



            m_vIsRetainedDocuments = Value

        End Set
    End Property

    Public Property SchemesPostcode() As Object
        Get

            Return m_vSchemesPostcode

        End Get
        Set(ByVal Value As Object)



            m_vSchemesPostcode = Value

        End Set
    End Property

    Public Property PaidDirect() As Object
        Get

            Return m_vPaidDirect

        End Get
        Set(ByVal Value As Object)



            m_vPaidDirect = Value

        End Set
    End Property

    Public Property Scheme() As Object
        Get

            Return m_vScheme

        End Get
        Set(ByVal Value As Object)



            m_vScheme = Value

        End Set
    End Property

    Public Property BrokerageAmount() As Object
        Get

            Return m_vBrokerageAmount

        End Get
        Set(ByVal Value As Object)



            m_vBrokerageAmount = Value

        End Set
    End Property

    Public Property IsMinimumBrokerageFlag() As Object
        Get

            Return m_vIsMinimumBrokerageFlag

        End Get
        Set(ByVal Value As Object)



            m_vIsMinimumBrokerageFlag = Value

        End Set
    End Property

    Public Property AnnualPremium() As Object
        Get

            Return m_vAnnualPremium

        End Get
        Set(ByVal Value As Object)



            m_vAnnualPremium = Value

        End Set
    End Property

    Public Property ThisPremium() As Object
        Get

            Return m_vThisPremium

        End Get
        Set(ByVal Value As Object)



            m_vThisPremium = Value

        End Set
    End Property

    Public Property NetPremium() As Object
        Get

            Return m_vNetPremium

        End Get
        Set(ByVal Value As Object)



            m_vNetPremium = Value

        End Set
    End Property


    Public Property CommissionAmount() As Object
        Get

            Return m_vCommissionAmount

        End Get
        Set(ByVal Value As Object)



            m_vCommissionAmount = Value

        End Set
    End Property


    Public Property CommissionPercentage() As Object
        Get

            Return m_vCommissionPercentage

        End Get
        Set(ByVal Value As Object)



            m_vCommissionPercentage = Value

        End Set
    End Property
    'DC 14/07/00
    'DC 14/07/00
    Public Property InvariantKey() As Object
        Get

            Return m_vInvariantKey

        End Get
        Set(ByVal Value As Object)



            m_vInvariantKey = Value

        End Set
    End Property

    'TN20000817 - Doc Ref 10 (Start)
    Public Property InsuredName() As Object
        Get
            Return m_vInsuredName
        End Get
        Set(ByVal Value As Object)


            m_vInsuredName = Value
        End Set
    End Property

    Public Property AlternateReference() As Object
        Get
            Return m_vAlternateReference
        End Get
        Set(ByVal Value As Object)


            m_vAlternateReference = Value
        End Set
    End Property

    Property IsClientInvoiced() As Object
        Get
            Return m_vIsClientInvoiced
        End Get
        Set(ByVal Value As Object)


            m_vIsClientInvoiced = Value
        End Set
    End Property

    Property OldPolicyNumber() As Object
        Get
            Return m_vOldPolicyNumber
        End Get
        Set(ByVal Value As Object)


            m_vOldPolicyNumber = Value
        End Set
    End Property

    Public Property QuoteExpiryDate() As Object
        Get
            Return m_vQuoteExpiryDate
        End Get
        Set(ByVal Value As Object)


            m_vQuoteExpiryDate = Value
        End Set
    End Property

    Public Property AlternateAccountCnt() As Object
        Get
            Return m_vAlternateAccountCnt
        End Get
        Set(ByVal Value As Object)


            m_vAlternateAccountCnt = Value
        End Set
    End Property
    'TN20000817 - Doc Ref 10 (End)


    Public Property IPTableAmount() As Object
        Get

            Return m_vIPTableAmount

        End Get
        Set(ByVal Value As Object)



            m_vIPTableAmount = Value

        End Set
    End Property

    Public Property IPTPercentage() As Object
        Get

            Return m_vIPTPercentage

        End Get
        Set(ByVal Value As Object)



            m_vIPTPercentage = Value

        End Set
    End Property

    Public Property IsIPTOverridden() As Object
        Get

            Return m_vIsIPTOverridden

        End Get
        Set(ByVal Value As Object)



            m_vIsIPTOverridden = Value

        End Set
    End Property

    Public Property TaxAmount() As Object
        Get

            Return m_vTaxAmount

        End Get
        Set(ByVal Value As Object)



            m_vTaxAmount = Value

        End Set
    End Property

    Public Property VatableAmount() As Object
        Get

            Return m_vVatableAmount

        End Get
        Set(ByVal Value As Object)



            m_vVatableAmount = Value

        End Set
    End Property

    Public Property VatPercentage() As Object
        Get

            Return m_vVatPercentage

        End Get
        Set(ByVal Value As Object)



            m_vVatPercentage = Value

        End Set
    End Property

    Public Property VatAmount() As Object
        Get

            Return m_vVatAmount

        End Get
        Set(ByVal Value As Object)



            m_vVatAmount = Value

        End Set
    End Property

    Public Property PaymentMethod() As Object
        Get

            Return m_vPaymentMethod

        End Get
        Set(ByVal Value As Object)



            m_vPaymentMethod = Value

        End Set
    End Property

    Public Property UserDefinedDataId() As Object
        Get

            Return m_vUserDefinedDataID

        End Get
        Set(ByVal Value As Object)



            m_vUserDefinedDataID = Value

        End Set
    End Property

    Public Property QuoteInsuranceRef() As Object
        Get

            Return m_vQuoteInsuranceRef

        End Get
        Set(ByVal Value As Object)



            m_vQuoteInsuranceRef = Value

        End Set
    End Property

    Public Property NextInsuranceRef() As Object
        Get

            Return m_vNextInsuranceRef

        End Get
        Set(ByVal Value As Object)



            m_vNextInsuranceRef = Value

        End Set
    End Property

    Public Property LastInsuranceRef() As Object
        Get

            Return m_vLastInsuranceRef

        End Get
        Set(ByVal Value As Object)



            m_vLastInsuranceRef = Value

        End Set
    End Property

    Public Property EndorsementCount() As Object
        Get

            Return m_lEndorsementCount

        End Get
        Set(ByVal Value As Object)



            m_lEndorsementCount = Value

        End Set
    End Property

    Public Property RenewalCount() As Object
        Get

            Return m_lRenewalCount

        End Get
        Set(ByVal Value As Object)



            m_lRenewalCount = Value

        End Set
    End Property


    Public Property CreatedByID() As Object
        Get

            Return m_iCreatedByID

        End Get
        Set(ByVal Value As Object)



            m_iCreatedByID = Value

        End Set
    End Property

    'Public Property Let CreatedBy(sCreatedBy As String)
    '
    '    m_sCreatedBy$ = sCreatedBy$
    '
    'End Property
    'Public Property Get CreatedBy() As String
    '
    '    CreatedBy = m_sCreatedBy$
    '
    'End Property
    'DC 24/11/99
    'DC 24/11/99
    'developer guide no.101
    Public Property DataTransfer() As Object
        Get

            Return m_iDataTransfer

        End Get
        Set(ByVal Value As Object)



            m_iDataTransfer = Value

        End Set
    End Property
    Public Property DateCreated() As Object
        Get

            Return m_dtDateCreated

        End Get
        Set(ByVal Value As Object)



            m_dtDateCreated = Value

        End Set
    End Property

    Public Property ModifiedByID() As Object
        Get

            Return m_vModifiedByID

        End Get
        Set(ByVal Value As Object)



            m_vModifiedByID = Value

        End Set
    End Property

    'Public Property Let ModifiedBy(sModifiedBy As String)
    '
    '    m_sModifiedBy$ = sModifiedBy$
    '
    'End Property
    'Public Property Get ModifiedBy() As String
    '
    '    ModifiedBy = m_sModifiedBy$
    '
    'End Property

    Public Property LastModified() As Object
        Get

            Return m_vLastModified

        End Get
        Set(ByVal Value As Object)



            m_vLastModified = Value

        End Set
    End Property

    Public Property LastTransDate() As Object
        Get

            Return m_vLastTransDate

        End Get
        Set(ByVal Value As Object)



            m_vLastTransDate = Value

        End Set
    End Property

    'Developer Guide No 101
    Public Property LastTransTypeID() As Object
        Get

            Return m_vLastTransTypeID

        End Get
        Set(ByVal Value As Object)


            m_vLastTransTypeID = Value

        End Set
    End Property

    Public Property LastTransType() As Object
        Get

            Return Trim(m_sLastTransType)

        End Get
        Set(ByVal Value As Object)

            Dim lLastTransTypeID As Integer
            Dim sLookupTable As String = ""



            If Convert.IsDBNull(Value) Or Informations.IsNothing(Value) Then

                m_sLastTransType = Value

                m_vLastTransTypeID = Nothing
            ElseIf (Value = "") Then

                m_sLastTransType = Value

                m_vLastTransTypeID = Nothing
            Else
                'sj 06/11/2002 - start
                If m_sUnderwritingOrAgency = "" Then
                    m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)
                End If
                sLookupTable = gSIRLibrary.SIRLookupTransactionType
                'sj 06/11/2002 - end


                m_sLastTransType = Trim(Value)
                'EK130300 Changed lookup table
                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=sLookupTable, v_sCode:=m_sLastTransType, v_dtEffectiveDate:=DateTime.Now, r_lID:=lLastTransTypeID)

                m_vLastTransTypeID = lLastTransTypeID
            End If

        End Set
    End Property

    Public Property LastTransDescription() As Object
        Get

            Return m_vLastTransDescription

        End Get
        Set(ByVal Value As Object)



            m_vLastTransDescription = Value

        End Set
    End Property

    Public Property LastTransDebitCredit() As Object
        Get

            Return m_vLastTransDebitCredit

        End Get
        Set(ByVal Value As Object)



            m_vLastTransDebitCredit = Value

        End Set
    End Property

    Public Property LastTransDocumentRef() As Object
        Get

            Return m_vLastTransDocumentRef

        End Get
        Set(ByVal Value As Object)



            m_vLastTransDocumentRef = Value

        End Set
    End Property

    Public Property LastTransCoverStartDate() As Object
        Get

            Return m_vLastTransCoverStartDate

        End Get
        Set(ByVal Value As Object)



            m_vLastTransCoverStartDate = Value

        End Set
    End Property

    Public Property LastTransExpiryDate() As Object
        Get

            Return m_vLastTransExpiryDate

        End Get
        Set(ByVal Value As Object)



            m_vLastTransExpiryDate = Value

        End Set
    End Property

    Public Property InsuranceFolderID() As Object
        Get

            Return m_lInsuranceFolderID

        End Get
        Set(ByVal Value As Object)



            m_lInsuranceFolderID = Value

        End Set
    End Property

    Public Property InsuranceHolderCnt() As Object
        Get

            Return m_lInsuranceHolderCnt

        End Get
        Set(ByVal Value As Object)



            m_lInsuranceHolderCnt = Value

        End Set
    End Property

    Public Property InsuranceHolder() As Object
        Get

            Return Trim(m_sInsuranceHolder)

        End Get
        Set(ByVal Value As Object)


            m_sInsuranceHolder = Value

        End Set
    End Property

    Public Property InsuranceFolderCode() As String
        Get

            Return m_sInsuranceFolderCode

        End Get
        Set(ByVal Value As String)


            m_sInsuranceFolderCode = CStr(Value)

        End Set
    End Property

    Public Property InsuranceFolderDescription() As Object
        Get

            Return m_vInsuranceFolderDescription

        End Get
        Set(ByVal Value As Object)



            m_vInsuranceFolderDescription = Value

        End Set
    End Property

    Public Property InceptionDate() As Object
        Get

            Return m_vInceptionDate

        End Get
        Set(ByVal Value As Object)



            m_vInceptionDate = Value

        End Set
    End Property

    Public Property ArcArchiveFolderID() As Object
        Get

            Return m_vArcArchiveFolderID

        End Get
        Set(ByVal Value As Object)



            m_vArcArchiveFolderID = Value

        End Set
    End Property

    'Public Property Let ArcArchiveFolderCode(sArcArchiveFolderCode As String)
    '
    '    m_sArcArchiveFolderCode$ = sArcArchiveFolderCode$
    '
    'End Property
    'Public Property Get ArcArchiveFolderCode() As String
    '
    '    ArcArchiveFolderCode = m_sArcArchiveFolderCode$
    '
    'End Property

    Public Property TrailerSumInsured() As Object
        Get

            Return m_dTrailerSumInsured

        End Get
        Set(ByVal Value As Object)



            m_dTrailerSumInsured = Value

        End Set
    End Property

    Public Property IsTrailerCovered() As Object
        Get

            Return m_iIsTrailerCovered

        End Get
        Set(ByVal Value As Object)



            m_iIsTrailerCovered = Value

        End Set
    End Property

    Public Property VehicleUse() As Object
        Get

            Return m_sVehicleUse

        End Get
        Set(ByVal Value As Object)



            m_sVehicleUse = Value

        End Set
    End Property

    Public Property MBPFee() As Object
        Get

            Return m_dMBPFee

        End Get
        Set(ByVal Value As Object)



            m_dMBPFee = Value

        End Set
    End Property

    Public Property CreditRef() As Object
        Get

            Return m_vCreditRef

        End Get
        Set(ByVal Value As Object)



            m_vCreditRef = Value

        End Set
    End Property

    Public Property StPaulsRef() As Object
        Get

            Return m_vStPaulsRef

        End Get
        Set(ByVal Value As Object)



            m_vStPaulsRef = Value

        End Set
    End Property

    Public Property NUInstallmentFee() As Object
        Get

            Return m_vNUInstallmentFee

        End Get
        Set(ByVal Value As Object)



            m_vNUInstallmentFee = Value

        End Set
    End Property

    Public Property MBPInstallmentFee() As Object
        Get

            Return m_vMBPInstallmentFee

        End Get
        Set(ByVal Value As Object)



            m_vMBPInstallmentFee = Value

        End Set
    End Property

    Public Property NURef() As Object
        Get

            Return m_vNURef

        End Get
        Set(ByVal Value As Object)



            m_vNURef = Value

        End Set
    End Property

    Public Property EventDescription() As Object
        Get

            Return m_vEventDescription

        End Get
        Set(ByVal Value As Object)



            m_vEventDescription = Value

        End Set
    End Property

    'MKW 150606 START
    Public Property CountryID() As Object
        Get
            Return m_lCountryId
        End Get
        Set(ByVal Value As Object)


            m_lCountryId = Value
        End Set
    End Property


    '1.12 WR25

    Public Property RenewalProductID() As Object
        Get
            Return m_vRenewalProductID
        End Get
        Set(ByVal Value As Object)


            m_vRenewalProductID = Value
        End Set
    End Property


    Public Property OriginalProductID() As Object
        Get
            Return m_vOriginalProductID
        End Get
        Set(ByVal Value As Object)


            m_vOriginalProductID = Value
        End Set
    End Property


    Public Property CurrencyToBaseXRate() As Object
        Get
            Return m_vCurrencyToBaseXRate
        End Get
        Set(ByVal Value As Object)


            m_vCurrencyToBaseXRate = Value
        End Set
    End Property


    ' WPR 63 , Start

    Public Property QuoteStatusID() As Integer
        Get
            Return m_vQuoteStatus
        End Get
        Set(ByVal value As Integer)
            m_vQuoteStatus = value
        End Set
    End Property

    Public Property QuoteVersionID() As Integer
        Get
            Return m_vQuoteVersion
        End Get
        Set(ByVal value As Integer)
            m_vQuoteVersion = value
        End Set
    End Property

    Public Property BaseInsuranceFolderCnt() As Integer
        Get
            Return m_vBaseInsuranceFolder
        End Get
        Set(ByVal value As Integer)
            m_vBaseInsuranceFolder = value
        End Set
    End Property

    ''' <summary>
    ''' To read and write property is this a Market Place policy or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMarketPlacePolicy() As Integer
        Get
            Return m_nIsMarketPlacePolicy
        End Get
        Set(ByVal value As Integer)
            m_nIsMarketPlacePolicy = value
        End Set
    End Property
    ' WPR 63 End

    Public Property CorrespondenceType() As Integer
        Get
            Return m_iCorrespondenceType
        End Get
        Set(ByVal value As Integer)
            m_iCorrespondenceType = value
        End Set
    End Property

    Public Property DefaultPreferredCorrespondence() As Integer
        Get
            Return m_iDefaultPreferredCorrespondence
        End Get
        Set(ByVal value As Integer)
            m_iDefaultPreferredCorrespondence = value
        End Set
    End Property

    Public Property IsAgentCorrespondence() As Integer
        Get
            Return m_bIsAgentCorrepondence
        End Get
        Set(ByVal value As Integer)
            m_bIsAgentCorrepondence = value
        End Set
    End Property

    Public Property SenderEmail() As Object
        Get
            Return m_vSenderEmail
        End Get
        Set(ByVal value As Object)
            m_vSenderEmail = value
        End Set
    End Property

    Public Property ReceiverEmail() As Object
        Get
            Return m_vReceiverEmail
        End Get
        Set(ByVal value As Object)
            m_vReceiverEmail = value
        End Set
    End Property

    Public Property OriginalInsuranceFileTypeID() As Object
        Get
            Return m_vOriginalInsuranceFileTypeId
        End Get
        Set(ByVal value As Object)
            m_vOriginalInsuranceFileTypeId = value
        End Set
    End Property
#End Region

#Region "Public Methods"

    Public Function UpdateLapsed() As Integer
        Return UpdateLapsed(v_dtLapsedDate:=Nothing)
    End Function

    Public Function UpdateLapsed(ByVal v_dtLapsedDate As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim oSIRInsuranceFile As SIRInsuranceFile

            result = gPMConstants.PMEReturnCode.PMTrue

            oSIRInsuranceFile = New SIRInsuranceFile()

            m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oSIRInsuranceFile

                'Set the insurancefilecnt
                .InsuranceFileCnt = InsuranceFileCnt

                ' Add a record to the database from the object
                m_lReturn = .UpdateLapsed(v_dtLapsedDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLapsed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLapsed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CancelPolicy() As Integer

        Dim result As Integer = 0
        Try

            Dim oSIRInsuranceFile As SIRInsuranceFile

            result = gPMConstants.PMEReturnCode.PMTrue

            oSIRInsuranceFile = New SIRInsuranceFile()

            m_lReturn = oSIRInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oSIRInsuranceFile

                'Set the insurancefilecnt
                .InsuranceFileCnt = InsuranceFileCnt

                ' Add a record to the database from the object
                m_lReturn = .CancelPolicy()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            oSIRInsuranceFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function




    ''' <summary>
    ''' Initialise (Standard Method) - Entry point for any initialisation code for thisobject.
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns>Long</returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer,
                               ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer,
                               ByVal iLogLevel As Integer, ByVal sCallingAppName As String,
                               Optional ByVal bStandAlone As Boolean = False,
                               Optional ByVal vDatabase As Object = Nothing) As Long


        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
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


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID,
                                                           v_lPMProductFamily:=PMProductFamily,
                                                           r_bNewInstanceCreated:=m_bCloseDatabase,
                                                           r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            ' Create instance of InsuranceFile Business class
            m_oSIRInsuranceFile = New Business()

            m_lReturn = m_oSIRInsuranceFile.Initialise(sUsername:=sUsername, sPassword:=sPassword,
                                                       iUserID:=iUserID, iSourceID:=iSourceID,
                                                       iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID,
                                                       iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName,
                                                       vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of InsuranceFileSystem Business class
            m_oSIRInsuranceFileSystem = New bSIRInsuranceFileSystem.Business()

            m_lReturn = m_oSIRInsuranceFileSystem.Initialise(sUserName:=sUsername, sPassword:=sPassword,
                                                             iUserID:=iUserID, iSourceID:=iSourceID,
                                                             iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID,
                                                             iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName,
                                                             vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create instance of InsuranceFolder Business class
            m_oSIRInsuranceFolder = New bSIRInsuranceFolder.Business()

            m_lReturn = m_oSIRInsuranceFolder.Initialise(sUsername:=sUsername, sPassword:=sPassword,
                                                         iUserID:=iUserID, iSourceID:=iSourceID,
                                                         iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID,
                                                         iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName,
                                                         vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID,
                                             iSourceID:=iSourceID, iLanguageID:=iLanguageID,
                                             iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel,
                                             sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'JK111198
            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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
                If m_oSIRInsuranceFile IsNot Nothing Then
                    m_oSIRInsuranceFile.Dispose()
                    m_oSIRInsuranceFile = Nothing
                End If
                If m_oSIRInsuranceFileSystem IsNot Nothing Then
                    m_oSIRInsuranceFileSystem.Dispose()
                    m_oSIRInsuranceFileSystem = Nothing
                End If
                If m_oSIRInsuranceFolder IsNot Nothing Then
                    m_oSIRInsuranceFolder.Dispose()
                    m_oSIRInsuranceFolder = Nothing
                End If
                If m_oSolutionInsuranceFile IsNot Nothing Then
                    m_oSolutionInsuranceFile.Dispose()
                    m_oSolutionInsuranceFile = Nothing
                End If
                If m_oPMLock IsNot Nothing Then
                    m_oPMLock.Dispose()
                    m_oPMLock = Nothing
                End If
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: CreatePolicy
    '
    ' Description: Create new Policy using defaults and supplied properties.
    '
    ' ***************************************************************** '
    Public Function CreatePolicy() As Integer

        Dim result As Integer = 0
        Dim vFieldArray() As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim bCopyAgents As Boolean
            Dim lOldInsuranceFileCnt As Integer

            ' Set Defaults
            m_lReturn = SetDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 05/09/2002 - start
            If InsuranceFileCnt > 0 Then
                bCopyAgents = True
                lOldInsuranceFileCnt = InsuranceFileCnt
            End If
            'sj 05/09/2002 - end

            ' Start Transaction
            m_lReturn = BeginTrans()

            ' Create Folder if necessary
            If InsuranceFolderCnt < 1 Then

                With m_oSIRInsuranceFolder

                    m_lReturn = .DirectAdd(vInsuranceFolderCnt:=InsuranceFolderCnt, vInsuranceHolderCnt:=InsuranceHolderCnt, vCode:=InsuranceFolderCode, vDescription:=InsuranceFolderDescription, vInceptionDate:=InceptionDate, vQuoteInsuranceRef:=QuoteInsuranceRef, vNextInsuranceRef:=NextInsuranceRef, vLastInsuranceRef:=LastInsuranceRef, vRenewalCount:=RenewalCount, vSourceID:=m_iSourceID)


                End With

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If


                m_lInsuranceFolderCnt = m_oSIRInsuranceFolder.InsuranceFolderCnt

            End If

            ' WPR 63
            BaseInsuranceFolderCnt = InsuranceFolderCnt
            QuoteVersionID = 1
            QuoteStatusID = 1

            With m_oSIRInsuranceFile

                'sj 28/9/99 - start

                .InsuranceFolderCnt = CInt(m_lInsuranceFolderCnt)
                'sj 28/9/99 - end


                m_lReturn = GetProperties(r_vFieldArray:=vFieldArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If


                'Tomo230300


                .EventDescription = EventDescription

                m_lReturn = .DirectAdd(v_vFieldArray:=vFieldArray)



                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                vFieldArray = Nothing

            End With

            m_lInsuranceFileCnt = m_oSIRInsuranceFile.InsuranceFileCnt

            With m_oSIRInsuranceFileSystem

                'sj 28/9/99 - start
                .InsuranceFileCnt = InsuranceFileCnt 'sj temp
                'sj 28/9/99 - end

                m_lReturn = .DirectAdd(vInsuranceFileCnt:=InsuranceFileCnt, vEndorsementCount:=EndorsementCount, vLastTransDate:=LastTransDate, vLastTransTypeID:=LastTransTypeID, vLastTransDescription:=LastTransDescription, vLastTransDebitCredit:=LastTransDebitCredit, vLastTransDocumentRef:=LastTransDocumentRef, vLastTransCoverStartDate:=LastTransCoverStartDate, vLastTransExpiryDate:=LastTransExpiryDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

            End With

            ' Create Solution specific details
            m_lReturn = GetSolutionInsFile()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            With m_oSolutionInsuranceFile

                'PN38002 add new field to hold renewal premium at point of MTA quote

                Select Case InsuranceFileStructure
                    Case "MBP"

                        m_lReturn = .DirectAdd(vInsuranceFileCnt:=ToSafeInteger(InsuranceFileCnt), vAnnualPremium:=AnnualPremium, vThisPremium:=ThisPremium, vTrailerSumInsured:=TrailerSumInsured, vIsTrailerCovered:=IsTrailerCovered, vVehicleUse:=VehicleUse, vPaymentMethod:=PaymentMethod, vMbpFee:=MBPFee, vCreditRef:=CreditRef, vTaxAmount:=TaxAmount, vCommissionAmount:=CommissionAmount, vStPaulsRef:=StPaulsRef, vNUInstallmentFee:=NUInstallmentFee, vMBPInstallmentFee:=MBPInstallmentFee, vNURef:=NURef, vRenewalPremium:=RenewalPremium)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = RollbackTrans()
                            Return result
                        End If
                End Select

            End With

            'sj 05/09/2002 - start
            If bCopyAgents Then
                'Copy Policy Agents
                m_lReturn = CopyPolicyAgents(v_lOldInsuranceFileCnt:=lOldInsuranceFileCnt, v_lNewInsuranceFileCnt:=m_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicy")

                    Return result
                End If
            End If
            'sj 05/09/2002 - end

            ' Commit Transaction
            m_lReturn = CommitTrans()

            ' Not Required For Data Transfer
            'DC 24/11/99
            If m_iDataTransfer = gPMConstants.PMEReturnCode.PMFalse Then
                'Populate Object
                m_lReturn = GetDetails()
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePolicyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicy
    '
    ' Description: Update Policy using supplied properties.
    '
    ' ***************************************************************** '
    Public Function UpdatePolicy() As Integer

        Dim result As Integer = 0
        Dim vFieldArray() As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for InsuranceFile identity
            m_lReturn = GetIdentity()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start Transaction
            m_lReturn = BeginTrans()


            m_lReturn = UpdateStatusLapsePolicyVersions(v_nInsuranceFileCnt:=m_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            With m_oSIRInsuranceFile
                'sj 18/10/99
                m_lReturn = SetDefaults()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If


                m_lReturn = GetProperties(r_vFieldArray:=vFieldArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If
                'sj 18/10/99

                .PartyCnt = InsuredCnt


                m_lReturn = .EditUpdate(lRow:=1, r_vFieldArray:=vFieldArray)

                '        m_lReturn& = .EditUpdate( _
                'lRow:=1, _
                'vInsuranceFileCnt:=InsuranceFileCnt, _
                'vInsuranceFileStructureID:=InsuranceFileStructureID, _
                'vInsuranceFileTypeID:=InsuranceFileTypeID, _
                'vInsuranceFileStatusID:=InsuranceFileStatusID, _
                'vSourceID:=SourceID, _
                'vInsuranceFolderCnt:=InsuranceFolderCnt, _
                'vInsuranceRef:=InsuranceRef, _
                'vProductID:=ProductID, _
                'vLeadInsurerCnt:=LeadInsurerCnt, _
                'vLeadAgentCnt:=LeadAgentCnt, vLeadAgentPercent:=LeadAgentPercent, _
                'vAccountHandlerCnt:=AccountHandlerCnt, _
                'vInsuredCnt:=InsuredCnt, _
                'vBusinessTypeID:=BusinessTypeID, _
                'vCollectTypeID:=CollectTypeID, vCollectionFromCnt:=CollectionFromCnt, _
                'vBranchID:=BranchID, vCurrencyID:=m_iCurrencyID, vLanguageID:=LanguageID, _
                'vDateIssued:=DateIssued, _
                'vCoverStartDate:=CoverStartDate, vExpiryDate:=ExpiryDate, _
                'vRenewalDate:=RenewalDate, vRenewalMethodID:=RenewalMethodID, _
                'vRenewalFrequencyID:=RenewalFrequencyID, _
                'vIsReferredAtRenewal:=IsReferredAtRenewal, _
                'vLapsedReasonID:=LapsedReasonID, vLapsedDate:=LapsedDate, vLapsedDescription:=LapsedDescription, _
                'vIsReferredOnMta:=IsReferredOnMta, _
                'vVarDataRef:=VarDataRef)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                'Tomo230300


                'Developer Guide No. 24

                .EventDescription = EventDescription

                m_lReturn = .Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                vFieldArray = Nothing

            End With

            With m_oSIRInsuranceFileSystem
                'sj 18/10/99
                .InsuranceFileCnt = InsuranceFileCnt

                m_lReturn = .EditUpdate(lRow:=1, vEndorsementCount:=EndorsementCount, vLastTransDate:=LastTransDate, vLastTransTypeID:=LastTransTypeID, vLastTransDescription:=LastTransDescription, vLastTransDebitCredit:=LastTransDebitCredit, vLastTransDocumentRef:=LastTransDocumentRef, vLastTransCoverStartDate:=LastTransCoverStartDate, vLastTransExpiryDate:=LastTransExpiryDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                m_lReturn = .Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

            End With

            With m_oSIRInsuranceFolder

                m_lReturn = .EditUpdate(lRow:=1, vInsuranceHolderCnt:=InsuranceHolderCnt, vCode:=InsuranceFolderCode, vDescription:=InsuranceFolderDescription, vInceptionDate:=InceptionDate, vQuoteInsuranceRef:=QuoteInsuranceRef, vNextInsuranceRef:=NextInsuranceRef, vLastInsuranceRef:=LastInsuranceRef, vRenewalCount:=RenewalCount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                m_lReturn = .Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

            End With

            ' Update Solution specific details
            m_lReturn = GetSolutionInsFile()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            With m_oSolutionInsuranceFile

                Select Case InsuranceFileStructure
                    Case "MBP"
                        ' Get details if InsuranceFileCnt not set

                        If .InsuranceFileCnt <> m_lInsuranceFileCnt Then


                            m_lReturn = .GetDetails(vInsuranceFileCnt:=ToSafeInteger(m_lInsuranceFileCnt))
                            'PN38002 add new field to hold renewal premium at point of MTA quote


                            m_lReturn = .GetNext(vAnnualPremium:=m_dAnnualPremium, vThisPremium:=m_dThisPremium, vTrailerSumInsured:=m_dTrailerSumInsured, vIsTrailerCovered:=m_iIsTrailerCovered, vVehicleUse:=m_sVehicleUse, vPaymentMethod:=m_vPaymentMethod, vMbpFee:=m_dMBPFee, vCreditRef:=m_vCreditRef, vTaxAmount:=m_dTaxAmount, vCommissionAmount:=m_dCommissionAmount, vStPaulsRef:=m_vStPaulsRef, vNUInstallmentFee:=m_vNUInstallmentFee, vMBPInstallmentFee:=m_vMBPInstallmentFee, vNURef:=m_vNURef, vRenewalPremium:=m_dRenewalPremium)

                        End If
                        'PN38002 add new field to hold renewal premium at point of MTA quote


                        m_lReturn = .EditUpdate(lRow:=1, vInsuranceFileCnt:=ToSafeInteger(InsuranceFileCnt), vAnnualPremium:=AnnualPremium, vThisPremium:=ThisPremium, vTrailerSumInsured:=TrailerSumInsured, vIsTrailerCovered:=IsTrailerCovered, vVehicleUse:=VehicleUse, vPaymentMethod:=PaymentMethod, vMbpFee:=MBPFee, vCreditRef:=CreditRef, vTaxAmount:=TaxAmount, vCommissionAmount:=CommissionAmount, vStPaulsRef:=StPaulsRef, vNUInstallmentFee:=NUInstallmentFee, vMBPInstallmentFee:=MBPInstallmentFee, vNURef:=NURef, vRenewalPremium:=RenewalPremium)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = RollbackTrans()
                            Return result
                        End If


                        m_lReturn = .Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = RollbackTrans()
                            Return result
                        End If
                End Select

            End With

            ' Commit Transaction
            m_lReturn = CommitTrans()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***************************************************************** '
    'Name: DeletePolicy

    'Description: Delete Current Policy.

    '***************************************************************** '
    Public Function DeletePolicy() As Integer

        Dim result As Integer = 0
        Dim lNumberOfFiles As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for InsuranceFile identity
            m_lReturn = GetIdentity()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check number of Policies in Folder

            m_lReturn = CheckFolder(v_lInsFolderCnt:=CInt(m_lInsuranceFolderCnt), r_lNumberOfFiles:=lNumberOfFiles)

            ' Start Transaction
            m_lReturn = BeginTrans()

            ' Delete Solution specific data first
            m_lReturn = GetSolutionInsFile()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            With m_oSolutionInsuranceFile

                Select Case InsuranceFileStructure
                    Case "MBP"
                        ' Get details if InsuranceFileCnt not set

                        If .InsuranceFileCnt <> m_lInsuranceFileCnt Then


                            m_lReturn = .GetDetails(vInsuranceFileCnt:=ToSafeInteger(m_lInsuranceFileCnt))
                            'PN38002 add new field to hold renewal premium at point of MTA quote


                            m_lReturn = .GetNext(vAnnualPremium:=m_dAnnualPremium, vThisPremium:=m_dThisPremium, vTrailerSumInsured:=m_dTrailerSumInsured, vIsTrailerCovered:=m_iIsTrailerCovered, vVehicleUse:=m_sVehicleUse, vPaymentMethod:=m_vPaymentMethod, vMbpFee:=m_dMBPFee, vCreditRef:=m_vCreditRef, vTaxAmount:=m_dTaxAmount, vCommissionAmount:=m_dCommissionAmount, vStPaulsRef:=m_vStPaulsRef, vNUInstallmentFee:=m_vNUInstallmentFee, vMBPInstallmentFee:=m_vMBPInstallmentFee, vNURef:=m_vNURef, vRenewalPremium:=m_dRenewalPremium)

                        End If


                        m_lReturn = .EditDelete(lRow:=1)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = RollbackTrans()
                            Return result
                        End If


                        m_lReturn = .Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = RollbackTrans()
                            Return result
                        End If

                End Select

            End With

            With m_oSIRInsuranceFileSystem

                'MSS210901 - Added for merge
                'TN20010216 Start
                .InsuranceFileCnt = m_lInsuranceFileCnt
                'TN20010216 End
                'MSS210901 - Merge end

                m_lReturn = .EditDelete(lRow:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                m_lReturn = .Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

            End With

            With m_oSIRInsuranceFile

                m_lReturn = .EditDelete(lRow:=1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

                m_lReturn = .Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If

            End With

            If lNumberOfFiles = 1 Then
                With m_oSIRInsuranceFolder

                    m_lReturn = .EditDelete(lRow:=1)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = RollbackTrans()
                        Return result
                    End If

                    m_lReturn = .Update()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = RollbackTrans()
                        Return result
                    End If

                End With
            End If

            ' Commit Transaction
            m_lReturn = CommitTrans()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRInsuranceFile entity properties.
    '
    ' ***************************************************************** '
    Public Function GetDetails() As Integer

        Dim result As Integer = 0
        Dim vTableArray(,) As Object = Nothing
        Dim vResultArray(,) As Object = Nothing
        Dim sShortName As String = String.Empty
        Dim sResolvedName As String = String.Empty
        Dim sLastTransType As String = String.Empty
        Dim vFieldArray As Object = Nothing
        Dim sLookupTable As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for InsuranceFile identity
            m_lReturn = GetIdentity()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the InsuranceFile Properties
            With m_oSIRInsuranceFile
                'ECK 9/8/99
                .FromEvent = FromEvent
                m_lReturn = .GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)

                m_lReturn = .GetNext(r_vFieldArray:=vFieldArray)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = SetProperties(v_vFieldArray:=vFieldArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                vFieldArray = Nothing

            End With

            ' Set the InsuranceFileSystem Properties
            With m_oSIRInsuranceFileSystem
                'ECK 9/8/99
                .FromEvent = FromEvent

                m_lReturn = .GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)

                m_lReturn = .GetNext(vEndorsementCount:=m_lEndorsementCount, vCreatedByID:=m_iCreatedByID, vDateCreated:=m_dtDateCreated, vModifiedByID:=m_vModifiedByID, vLastModified:=m_vLastModified, vLastTransDate:=m_vLastTransDate, vLastTransTypeID:=m_vLastTransTypeID, vLastTransDescription:=m_vLastTransDescription, vLastTransDebitCredit:=m_vLastTransDebitCredit, vLastTransDocumentRef:=m_vLastTransDocumentRef, vLastTransCoverStartDate:=m_vLastTransCoverStartDate, vLastTransExpiryDate:=m_vLastTransExpiryDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            ' Set the InsuranceFolder Properties
            With m_oSIRInsuranceFolder
                'ECK 9/8/99
                .FromEvent = FromEvent
                If Not FromEvent Then

                    m_lReturn = .GetDetails(vInsuranceFolderCnt:=m_lInsuranceFolderCnt)
                Else
                    m_lReturn = .GetDetails(vInsuranceFolderCnt:=m_lInsuranceFileCnt)
                End If

                m_lReturn = .GetNext(vInsuranceFolderID:=m_lInsuranceFolderID, vInsuranceHolderCnt:=m_lInsuranceHolderCnt, vCode:=m_sInsuranceFolderCode, vDescription:=m_vInsuranceFolderDescription, vInceptionDate:=m_vInceptionDate, vArcArchiveFolderID:=m_vArcArchiveFolderID, vQuoteInsuranceRef:=m_vQuoteInsuranceRef, vNextInsuranceRef:=m_vNextInsuranceRef, vLastInsuranceRef:=m_vLastInsuranceRef, vRenewalCount:=m_lRenewalCount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            '----------------------------------------------------------
            'JK021198 Lookups moved up above GetSolutionInsFile()
            'Get Lookup codes
            '    m_lReturn& = m_oSIRInsuranceFile.GetLookupValues( _
            'iLookupType:=PMLookupSingle, _
            'vTableArray:=vTableArray, _
            'iLanguageID:=m_iLanguageID%, _
            'vResultArray:=vResultArray)


            m_lReturn = m_oSIRInsuranceFile.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, vResultArray:=vResultArray)



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupInsuranceFileStructure, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sInsuranceFileStructure)



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupInsFileType, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sInsuranceFileType)

            ' TF180599 - Check for Null value first

            If Not Object.Equals(m_vInsuranceFileStatusID, Nothing) Then

                If Not (Convert.IsDBNull(m_vInsuranceFileStatusID) Or Informations.IsNothing(m_vInsuranceFileStatusID)) Then


                    m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupInsFileStatus, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_vInsuranceFileStatus)
                End If
            End If



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSource, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sSource)



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupProduct, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sProduct)



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupBusinessType, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sBusinessType)



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupCollectType, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sCollectType)

            'sj 19/07/2002 - start
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=SIRLookupBranch, _
            ''        vTableArray:=vTableArray, _
            ''        vResultArray:=vResultArray, _
            ''        vResult:=m_sBranch)



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSubBranch, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_vSubBranch)
            'sj 19/07/2002 - end



            m_lReturn = GetLookupDetails(sLookupTable:=gPMConstants.PMLookupCurrency, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sCurrencyCode)



            m_lReturn = GetLookupDetails(sLookupTable:=gPMConstants.PMLookupLanguage, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sLanguage)

            ' TF180599 - Check for Null value first

            If Not Object.Equals(m_vRenewalMethodID, Nothing) Then

                If Not (Convert.IsDBNull(m_vRenewalMethodID) Or Informations.IsNothing(m_vRenewalMethodID)) Then


                    m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalMethod, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sRenewalMethod)
                End If
            End If



            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalFrequency, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sRenewalFrequency)

            ' TF180599 - Check for Null value first

            If Not Object.Equals(m_vLapsedReasonID, Nothing) Then

                If Not (Convert.IsDBNull(m_vLapsedReasonID) Or Informations.IsNothing(m_vLapsedReasonID)) Then


                    m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupLapsedReason, vTableArray:=vTableArray, vResultArray:=vResultArray, vResult:=m_sLapsedReason)
                End If
            End If

            ' T180199 - Lookup required from InsFileSystem table
            'EK130300 Changed lookup table



            'Developer Guide No 44
            If Not Informations.IsNothing(m_vLastTransTypeID) AndAlso Not m_vLastTransTypeID.Equals(0) Then

                If Not (Convert.IsDBNull(m_vLastTransTypeID) Or Informations.IsNothing(m_vLastTransTypeID)) Then
                    'sj 06/11/2002 - start
                    If m_sUnderwritingOrAgency = "" Then
                        m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)
                    End If
                    sLookupTable = gSIRLibrary.SIRLookupTransactionType
                    'sj 06/11/2002 - end
                    m_lReturn = m_oLookup.GetCodeFromID(v_sTableName:=sLookupTable, v_lID:=m_vLastTransTypeID, r_sCode:=sLastTransType)
                    m_sLastTransType = sLastTransType
                End If
            End If

            ' TF190600 - Get RiskGroup details

            If Not (Convert.IsDBNull(m_vRiskCodeID) Or Informations.IsNothing(m_vRiskCodeID)) Then

                If CInt(m_vRiskCodeID) > 0 Then
                    ' Get Risk Group ID

                    m_lReturn = m_oSIRInsuranceFile.GetRiskGroupForCode(v_lRiskCodeID:=CInt(m_vRiskCodeID), r_lRiskGroupId:=m_lRiskGroupID)
                    ' Cant do this yet as lookup not set !
                    '            If (m_lReturn& = PMTrue) Then
                    '                If (m_lRiskGroupID& > 0) Then
                    '                    m_lReturn& = GetLookupDetails( _
                    ''                        sLookupTable:=SIRLookupRiskGroup, _
                    ''                        vTableArray:=vTableArray, _
                    ''                        vResultArray:=vResultArray, _
                    ''                        vResult:=m_sRiskGroup$)
                    '                End If
                    '            End If
                End If
            End If

            ' Get Party names

            If Convert.IsDBNull(m_lLeadInsurerCnt) Or Informations.IsNothing(m_lLeadInsurerCnt) Then
                sResolvedName = ""
            Else

                'Developer Guide No. 98

                m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lLeadInsurerCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)
            End If

            m_sLeadInsurer = sResolvedName


            If Convert.IsDBNull(m_lLeadAgentCnt) Or Informations.IsNothing(m_lLeadAgentCnt) Then
                sResolvedName = ""
            Else

                'Developer Guide No. 98

                m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lLeadAgentCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)
            End If

            ' CJB 100804 PN13723 Get SubAgent name and code (if one has been assigned)



            'Developer Guide No. 98

            m_lReturn = CInt(GetPolicySubAgentDetails(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vSubAgentName:=m_vSubAgentName, r_vSubAgentCode:=m_vSubAgentCode))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch the Policy Sub-Agent Details for v_lInsuranceFileCnt " & m_lInsuranceFileCnt & ", m_lReturn: " & CStr(m_lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_sLeadAgent = sResolvedName

            'TODO LIST

            m_sLeadAgentCode = sShortName


            If Convert.IsDBNull(m_lAccountHandlerCnt) Or Informations.IsNothing(m_lAccountHandlerCnt) Then
                sResolvedName = ""
                sShortName = ""
            Else

                'Developer Guide No. 98

                m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lAccountHandlerCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)
            End If

            m_sAccountHandler = sResolvedName
            m_sAccountHandlerCode = sShortName

            ' SJP (CMG) 09/04/2003 PS235

            If Convert.IsDBNull(m_lAccountExecutiveCnt) Or Informations.IsNothing(m_lAccountExecutiveCnt) Then
                sResolvedName = ""
            Else

                'Developer Guide No. 98

                m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lAccountExecutiveCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)
            End If

            m_sAccountExecutive = sResolvedName


            'Developer Guide No. 98

            m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lInsuredCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)

            m_sInsured = sResolvedName


            If Convert.IsDBNull(m_lCollectionFromCnt) Or Informations.IsNothing(m_lCollectionFromCnt) Then
                sResolvedName = ""
            Else

                'Developer Guide No. 98

                m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lCollectionFromCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)
            End If

            m_sCollectionFrom = sResolvedName


            'Developer Guide No. 98

            m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lInsuranceHolderCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)

            m_sInsuranceHolder = sResolvedName

            'EK130300 Changed lookup table
            ' Get Transaction Type code
            'sj 06/11/2002 - start
            'Already called a few lines earlier
            '    If (IsNull(m_vLastTransTypeID) = False) Then
            '        m_lReturn = m_oLookup.GetCodeFromID( _
            ''            v_sTableName:=SIRLookupPostingType, _
            ''            v_lId:=m_vLastTransTypeID, _
            ''            r_sCode:=(m_sLastTransType))
            '    End If
            'sj 06/11/2002 - end

            '---------------------------------------------------

            ' Get Solution specific details

            m_lReturn = GetSolutionInsFile()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oSolutionInsuranceFile

                Select Case InsuranceFileStructure
                    Case "MBP"

                        m_lReturn = .GetDetails(vInsuranceFileCnt:=ToSafeInteger(m_lInsuranceFileCnt))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'PN38002 add new field to hold renewal premium at point of MTA quote
                        ' TF131198 - return parameters changed to use member variables

                        m_lReturn = .GetNext(vAnnualPremium:=m_dAnnualPremium, vThisPremium:=m_dThisPremium, vTrailerSumInsured:=m_dTrailerSumInsured, vIsTrailerCovered:=m_iIsTrailerCovered, vVehicleUse:=m_sVehicleUse, vPaymentMethod:=m_vPaymentMethod, vMbpFee:=m_dMBPFee, vCreditRef:=m_vCreditRef, vTaxAmount:=m_dTaxAmount, vCommissionAmount:=m_dCommissionAmount, vStPaulsRef:=m_vStPaulsRef, vNUInstallmentFee:=m_vNUInstallmentFee, vMBPInstallmentFee:=m_vMBPInstallmentFee, vNURef:=m_vNURef, vRenewalPremium:=m_dRenewalPremium)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                End Select

            End With

            'JK021198
            'Lookups moved up from here..

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            ' PWF 06/11/2002 - Moved from before Exit Function (potential infinite loops)



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LockPolicy
    '
    ' Description:
    '
    ' History: 24/03/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function LockPolicy(ByRef sLockedBy As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPMLock Is Nothing Then



                m_oPMLock = New bpmlock.User
                m_lReturn = m_oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            m_lReturn = m_oPMLock.LockKey(sKeyName:="insurance_file_cnt", vKeyValue:=m_lInsuranceFileCnt, iUserID:=m_iUserID, sCurrentlyLockedBy:=sLockedBy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockPolicy
    '
    ' Description:
    '
    ' History: 24/03/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UnlockPolicy() As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPMLock Is Nothing Then



                m_oPMLock = New bpmlock.User
                m_lReturn = m_oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            m_lReturn = m_oPMLock.UnLockKey(sKeyName:="insurance_file_cnt", vKeyValue:=m_lInsuranceFileCnt, iUserID:=m_iUserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


#End Region

#Region "Private Methods"

    ' ***************************************************************** '
    ' Name: GetSolutionInsFile
    '
    ' Description: Ensure SolutionCode has been provided & create object.
    '
    ' ***************************************************************** '
    Private Function GetSolutionInsFile() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If String.IsNullOrEmpty(m_sInsuranceFileStructure) Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Solution Code not provided", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSolutionInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        'sj 28/9/99 - start
        ' No file required for Gemini II at the moment
        'EK 18/10/99 None required for S4B either
        If (m_sInsuranceFileStructure.Trim() = "GEM") Or (m_sInsuranceFileStructure.Trim() = "PMB") Then
            Return result
        End If
        'sj 28/9/99 - end

        ' Exit if already exists
        If Not (m_oSolutionInsuranceFile Is Nothing) Then

            If m_oSolutionInsuranceFile.InsuranceFileCnt = m_lInsuranceFileCnt Then
                Return result
            Else
                ' Terminate existing object prior to creating new instance

                m_oSolutionInsuranceFile.Dispose()
            End If
        End If

        ' Create instance of Solution Specific InsuranceFile Business class
        ' SP071298 - Ensure Solution Specific code is trimmed
        'm_oSolutionInsuranceFile = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType("b" & InsuranceFileStructure & "InsuranceFile.Business" + "," + ("b" & InsuranceFileStructure & "InsuranceFile.Business").Substring(0, ("b" & InsuranceFileStructure & "InsuranceFile.Business").LastIndexOf(".")))).FullName, "b" & InsuranceFileStructure & "InsuranceFile.Business").Unwrap()


        'oSIRRiskScreen = New bSIRRiskScreen.Business
        m_oSolutionInsuranceFile = Nothing
        result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSolutionInsuranceFile, v_sClassName:="bSIRRiskScreen.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim r_sMessage As String = "Failed to create an instance of bSIRRiskScreen.Business"
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="b" & InsuranceFileStructure & "InsuranceFile.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = m_oSolutionInsuranceFile.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetIdentity
    '
    ' Description: Get InsuranceFileCnt form supplied value or IDs.
    '
    ' ***************************************************************** '
    Private Function GetIdentity() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lInsuranceFileCnt < 1 Then

            If (m_iSourceID.Equals(0)) Or (Object.Equals(m_lInsuranceFileID, Nothing)) Then

                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insufficient Identity Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIdentity ", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            Else
                With m_oDatabase

                    sSQL = "SELECT insurance_file_cnt FROM Insurance_File"
                    sSQL = sSQL & " WHERE source_id=" & SourceID
                    sSQL = sSQL & " AND insurance_file_id=" & InsuranceFileID

                    m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=ACGetInsFileCntFromIDName, bStoredProcedure:=ACGetInsFileCntFromIDStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lInsuranceFileCnt = gPMFunctions.NullToLong(.Records.Item(0).Fields("insurance_file_cnt"))
                End With
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the variable passed.
    '
    ' ***************************************************************** '
    'Developer Guide No 17. 
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef vTableArray(,) As Object, ByRef vResultArray(,) As Object, ByRef vResult As Object) As Integer

        Dim result As Integer = 0
        Dim m_vLookupValues, m_vLookupDetails As Object

        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailCode As Integer = 2



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the standard parameters to local variables

        m_vLookupValues = vTableArray

        m_vLookupDetails = vResultArray

        ' Get the lookup values.

        bFoundMatch = False


        For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
            ' Check for a match of the table name.

            If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim().ToUpper() = sLookupTable.Trim().ToUpper() Then
                ' Found a match
                bFoundMatch = True
                Exit For
            End If
        Next lRow

        ' Check if there has been a table match.
        If Not bFoundMatch Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

            Return result
        End If

        ' Using the lookup values, populate the result parameter with
        ' the details from the lookup details array.

        ' TF160600 - Add test for required ID



        'Developer Guide No. 63
        For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((m_vLookupValues(ACValueStartPos, lRow)) + (m_vLookupValues(ACValueNumber, lRow) - 1))


            If Not m_vLookupValues(ACValueID, lRow) Is DBNull.Value AndAlso Not String.IsNullOrEmpty(m_vLookupValues(ACValueID, lRow)) Then
                If Val(CStr(m_vLookupDetails(ACDetailKey, lCntr))) = CDbl(m_vLookupValues(ACValueID, lRow)) Then

                    vResult = CStr(m_vLookupDetails(ACDetailCode, lCntr))
                    Exit For
                End If
            End If
        Next lCntr

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetDefaults
    '
    ' Description: Set Policy Default Values for skeletal policy.
    '
    ' ***************************************************************** '
    Private Function SetDefaults() As Integer

        Dim result As Integer = 0
        Dim lValue As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        'DC 14/07/00

        If Object.Equals(m_vInvariantKey, Nothing) Then

            m_vInvariantKey = 0
        End If

        'TN20000818 - Doc Ref 10 (Start)

        If Object.Equals(m_vInsuredName, Nothing) Then



            m_vInsuredName = Nothing
        End If


        If Object.Equals(m_vAlternateReference, Nothing) Then



            m_vAlternateReference = Nothing
        End If


        If Object.Equals(m_vIsClientInvoiced, Nothing) Then

            m_vIsClientInvoiced = Nothing
        End If


        If Object.Equals(m_vOldPolicyNumber, Nothing) Then



            m_vOldPolicyNumber = Nothing
        End If


        If Object.Equals(m_vQuoteExpiryDate, Nothing) Then



            m_vQuoteExpiryDate = Nothing
        End If


        If Object.Equals(m_vAlternateAccountCnt, Nothing) Then



            m_vAlternateAccountCnt = Nothing
        End If
        'TN20000818 - Doc Ref 10 (End)

        'sj 28/9/99 - start

        If Not (m_vCommissionPercentage Is Nothing) Then

            m_vCommissionPercentage = 0
        End If
        'sj 28/9/99 - end

        ' Set Insurance_Folder.code to Insurance_File.insurance_ref

        If String.IsNullOrEmpty(m_sInsuranceFolderCode) Then
            m_sInsuranceFolderCode = InsuranceRef
        End If

        ' Default Renewal Frequency to Annual

        If Object.Equals(m_iRenewalFrequencyID, Nothing) Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupRenewalFrequency, v_sCode:="Annually", v_dtEffectiveDate:=DateTime.Now, r_lID:=lValue)

            m_iRenewalFrequencyID = lValue
        End If

        ' Default Collection Type to Direct

        If Not (m_vCollectTypeID Is Nothing) Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupCollectType, v_sCode:="Direct", v_dtEffectiveDate:=DateTime.Now, r_lID:=lValue)

            m_vCollectTypeID = lValue
        End If

        ' Default Insurance File Type to Quote

        If Object.Equals(m_lInsuranceFileTypeID, Nothing) Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupInsFileType, v_sCode:="QUOTE", v_dtEffectiveDate:=DateTime.Now, r_lID:=lValue)

            m_lInsuranceFileTypeID = lValue
        End If

        ' Default Business Type to Direct

        If Object.Equals(m_iBusinessTypeID, Nothing) Then
            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupBusinessType, v_sCode:="DIRECT", v_dtEffectiveDate:=DateTime.Now, r_lID:=lValue)

            m_iBusinessTypeID = lValue
        End If

        ' Default Parties to Insurance Holder

        If Object.Equals(m_lInsuredCnt, Nothing) Then


            m_lInsuredCnt = m_lInsuranceHolderCnt
            '        m_lPartyCnt = m_lInsuranceHolderCnt
        End If

        'WR25 1.2

        If Object.Equals(m_vRenewalProductID, Nothing) Then



            m_vRenewalProductID = Nothing
        End If


        If Object.Equals(m_vOriginalProductID, Nothing) Then



            m_vOriginalProductID = Nothing
        End If

        If Object.Equals(m_vOriginalInsuranceFileTypeId, Nothing) Then



            m_vOriginalInsuranceFileTypeId = Nothing
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetPartyNameFromCnt
    '
    ' Description: Creates SQL on the fly to get Party Shortname & resolved name
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function GetPartyNameFromCnt(ByVal v_lPartyCnt As Object, ByRef r_sShortName As String, ByRef r_sResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        'Dim oFields As ADODB.Fields
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        '    With m_oDatabase

        m_oDatabase.Parameters.Clear()

        sSQL = "SELECT shortname, resolved_name"
        sSQL = sSQL & " FROM Party"
        sSQL = sSQL & " WHERE party_cnt = " & v_lPartyCnt

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetPartyNameFromIDName, bStoredProcedure:=ACGetPartyNameFromIDStored, bKeepNulls:=True)

        'eck020101 Tim Fuller added if for error trapping
        If m_oDatabase.Records.Count() <> 0 Then
            oFields = m_oDatabase.Records.Item(0).Fields()
            With oFields

                If Not (Convert.IsDBNull(oFields("shortname")) Or Informations.IsNothing(oFields("shortname"))) Then
                    r_sShortName = oFields("shortname")
                Else
                    r_sShortName = ""
                End If

                If Not (Convert.IsDBNull(oFields("resolved_name")) Or Informations.IsNothing(oFields("resolved_name"))) Then
                    r_sResolvedName = oFields("resolved_name")
                Else
                    r_sResolvedName = ""
                End If
            End With
            oFields = Nothing
        End If

        '    End With


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetPartyCntFromABI
    '
    ' Description: Creates SQL on the fly to get Party Shortname & resolved name
    ' eck040603 include source_id when calculating the party
    ' ***************************************************************** '
    Private Function GetPartyCntFromABI(ByVal v_sABICode As String, ByRef r_lPartyCnt As Object, ByRef r_sShortName As String, ByRef r_sResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        'Developer Guide No. 112
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue


        With m_oDatabase

            .Parameters.Clear()

            sSQL = "SELECT party_cnt, shortname, resolved_name"
            sSQL = sSQL & " FROM Party"
            sSQL = sSQL & " WHERE abi_code_on_81 = '" & v_sABICode & "'"
            sSQL = sSQL & " AND source_id = '" & CStr(m_iSourceID) & "'"

            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=ACGetPartyDataFromABIName, bStoredProcedure:=ACGetPartyDataFromABIStored, bKeepNulls:=True)

            If .Records.Count() = 0 Then
                'No match...


                r_lPartyCnt = DBNull.Value
                r_sShortName = ""
                r_sResolvedName = ""
            Else

                oFields = .Records.Item(0).Fields()
                With oFields

                    If Not (Convert.IsDBNull(oFields("party_cnt")) Or Informations.IsNothing(oFields("party_cnt"))) Then

                        r_lPartyCnt = oFields("party_cnt")
                    Else


                        r_lPartyCnt = DBNull.Value
                    End If

                    If Not (Convert.IsDBNull(oFields("shortname")) Or Informations.IsNothing(oFields("shortname"))) Then
                        r_sShortName = oFields("shortname").Trim()
                    Else
                        r_sShortName = ""
                    End If

                    If Not (Convert.IsDBNull(oFields("resolved_name")) Or Informations.IsNothing(oFields("resolved_name"))) Then
                        r_sResolvedName = oFields("resolved_name").Trim()
                    Else
                        r_sResolvedName = ""
                    End If
                End With
                oFields = Nothing
            End If
        End With


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckFolder
    '
    ' Description: Check Number of Policies in Folder
    '
    ' ***************************************************************** '
    Private Function CheckFolder(ByVal v_lInsFolderCnt As Integer, ByRef r_lNumberOfFiles As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT insurance_file_cnt"
        sSQL = sSQL & " FROM Insurance_File "
        sSQL = sSQL & " WHERE insurance_folder_cnt = " & CStr(v_lInsFolderCnt)

        With m_oDatabase
            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="CheckInsFolderSQL", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return number of InsuranceFiles in this Folder
            r_lNumberOfFiles = .Records.Count()

        End With

        Return result

    End Function

#End Region
    '*************************************************************************
    ' Name :            BeginTrans
    ' Description :     Begins a Transaction.
    ' History :         01/08/2003 - Tracy Richards -Changed from private to
    '                   public to support an external transaction in
    '                   iPMURenSelection.
    '*************************************************************************
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "BeginTrans Failed", ACApp, ACClass, "BeginTrans", Informations.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name :            CommitTrans
    ' Description :     Commits a Transaction (Saves changes to DB).
    ' History :         01/08/2003 - Tracy Richards -Changed from private to
    '                   public to support an external transaction in
    '                   iPMURenSelection.
    '*************************************************************************
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "CommitTrans Failed", ACApp, ACClass, "CommitTrans", Informations.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name :            RollbackTrans
    ' Description :     Rollback a Transaction (Undo changes to DB).
    ' History :         01/08/2003 - Tracy Richards -Changed from private to
    '                   public to support an external transaction in
    '                   iPMURenSelection.
    '*************************************************************************
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "RollbackTrans Failed", ACApp, ACClass, "RollbackTrans", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ''' <summary>
    ''' SetProperties
    ''' </summary>
    ''' <param name="v_vFieldArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetProperties(ByVal v_vFieldArray() As Object) As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        m_lInsuranceFileCnt = CInt(v_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt))

        m_lInsuranceFileStructureID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID)

        m_lInsuranceFileTypeID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID)

        m_vInsuranceFileStatusID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID)

        m_lInsuranceFileID = v_vFieldArray(InsuranceFileConst.ACInsuranceFileID)

        m_iSourceID = CInt(v_vFieldArray(InsuranceFileConst.ACSourceID))

        m_lInsuranceFolderCnt = v_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt)

        m_sInsuranceRef = v_vFieldArray(InsuranceFileConst.ACInsuranceRef)

        m_lProductID = v_vFieldArray(InsuranceFileConst.ACProductID)

        m_lLeadInsurerCnt = v_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt)

        m_lLeadAgentCnt = v_vFieldArray(InsuranceFileConst.ACLeadAgentCnt)

        m_vLeadAgentPercent = v_vFieldArray(InsuranceFileConst.ACLeadAgentPercent)

        m_lAccountHandlerCnt = v_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt)

        m_lInsuredCnt = v_vFieldArray(InsuranceFileConst.ACInsuredCnt)

        m_iBusinessTypeID = v_vFieldArray(InsuranceFileConst.ACBusinessTypeID)

        m_vCollectTypeID = v_vFieldArray(InsuranceFileConst.ACCollectTypeID)

        m_lCollectionFromCnt = v_vFieldArray(InsuranceFileConst.ACCollectionFromCnt)

        m_vSubBranchID = v_vFieldArray(InsuranceFileConst.ACSubBranchID)

        m_iCurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACCurrencyID))

        m_iBaseCurrencyID = CInt(v_vFieldArray(InsuranceFileConst.ACBaseCurrencyID))

        m_iLanguageID = CInt(v_vFieldArray(InsuranceFileConst.ACLanguageID))

        m_vDateIssued = v_vFieldArray(InsuranceFileConst.ACDateIssued)

        m_dtCoverStartDate = v_vFieldArray(InsuranceFileConst.ACCoverStartDate)

        m_dtExpiryDate = v_vFieldArray(InsuranceFileConst.ACExpiryDate)

        m_dtRenewalDate = v_vFieldArray(InsuranceFileConst.ACRenewalDate)

        m_vRenewalMethodID = v_vFieldArray(InsuranceFileConst.ACRenewalMethodID)

        m_iRenewalFrequencyID = v_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID)

        m_iIsReferredAtRenewal = v_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal)

        m_vLapsedReasonID = v_vFieldArray(InsuranceFileConst.ACLapsedReasonID)

        m_vLapsedDate = v_vFieldArray(InsuranceFileConst.ACLapsedDate)

        m_vLapsedDescription = v_vFieldArray(InsuranceFileConst.ACLapsedDescription)

        m_iIsReferredOnMta = v_vFieldArray(InsuranceFileConst.ACIsReferredOnMta)

        m_iPolicyVersion = CInt(v_vFieldArray(InsuranceFileConst.ACPolicyVersion))

        m_vGeminiPolicyStatus = v_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus)

        m_vGeminiBusinessType = v_vFieldArray(InsuranceFileConst.ACGeminiBusinessType)

        m_vDeferredInd = v_vFieldArray(InsuranceFileConst.ACDeferredInd)

        m_vPolicyIgnore = v_vFieldArray(InsuranceFileConst.ACPolicyIgnore)

        m_vBrokerCnt = v_vFieldArray(InsuranceFileConst.ACBrokerCnt)

        m_vRiskCodeID = v_vFieldArray(InsuranceFileConst.ACRiskCodeId)

        m_vAnalysisCodeID = v_vFieldArray(InsuranceFileConst.ACAnalysisCodeId)

        m_vPolicyDeductiblesID = v_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId)

        m_vPolicyLimitsID = v_vFieldArray(InsuranceFileConst.ACPolicyLimitsId)

        m_vProposalDate = v_vFieldArray(InsuranceFileConst.ACProposalDate)

        m_vDiaryDate = v_vFieldArray(InsuranceFileConst.ACDiaryDate)

        m_vReviewDate = v_vFieldArray(InsuranceFileConst.ACReviewDate)

        m_vRenewalDayNumber = v_vFieldArray(InsuranceFileConst.ACRenewalDayNumber)

        m_vPolicyTypeId = v_vFieldArray(InsuranceFileConst.ACPolicyTypeId)

        m_vIndicator = v_vFieldArray(InsuranceFileConst.ACIndicator)

        m_vClause = v_vFieldArray(InsuranceFileConst.ACClause)

        m_vCover = v_vFieldArray(InsuranceFileConst.ACCover)

        m_vArea = v_vFieldArray(InsuranceFileConst.ACArea)

        m_vLongTermUndertakingDate = v_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate)

        m_vRenewalStopCodeID = v_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID)

        m_vVBSType = v_vFieldArray(InsuranceFileConst.ACVBSType)

        m_vVBSStatus = v_vFieldArray(InsuranceFileConst.ACVBSStatus)

        m_vIsInsurerRateTable = v_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable)

        m_vIsRelatedPolicies = v_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies)

        m_vIsRetainedDocuments = v_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments)

        m_vSchemesPostcode = v_vFieldArray(InsuranceFileConst.ACSchemesPostcode)

        m_vPaidDirect = v_vFieldArray(InsuranceFileConst.ACPaidDirect)

        m_vScheme = v_vFieldArray(InsuranceFileConst.ACScheme)

        m_vBrokerageAmount = v_vFieldArray(InsuranceFileConst.ACBrokerageAmount)

        m_vIsMinimumBrokerageFlag = v_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag)

        m_vAnnualPremium = v_vFieldArray(InsuranceFileConst.ACAnnualPremium)

        m_vThisPremium = v_vFieldArray(InsuranceFileConst.ACThisPremium)

        m_vNetPremium = v_vFieldArray(InsuranceFileConst.ACNetPremium)

        m_vCommissionAmount = v_vFieldArray(InsuranceFileConst.ACCommissionAmount)

        m_vIPTableAmount = v_vFieldArray(InsuranceFileConst.ACIPTableAmount)

        m_vIPTPercentage = v_vFieldArray(InsuranceFileConst.ACIPTPercentage)

        m_vIsIPTOverridden = v_vFieldArray(InsuranceFileConst.ACIsIPTOverridden)

        m_vTaxAmount = v_vFieldArray(InsuranceFileConst.ACTaxAmount)

        m_vVatableAmount = v_vFieldArray(InsuranceFileConst.ACVatableAmount)

        m_vVatPercentage = v_vFieldArray(InsuranceFileConst.ACVatPercentage)

        m_vVatAmount = v_vFieldArray(InsuranceFileConst.ACVatAmount)

        m_vPaymentMethod = v_vFieldArray(InsuranceFileConst.ACPaymentMethod)

        m_vUserDefinedDataID = v_vFieldArray(InsuranceFileConst.ACUserDefinedDataID)

        m_vInsuredName = v_vFieldArray(InsuranceFileConst.ACInsuredName)

        m_vAlternateReference = v_vFieldArray(InsuranceFileConst.ACAlternateReference)

        m_vIsClientInvoiced = v_vFieldArray(InsuranceFileConst.ACIsClientInvoiced)

        m_vOldPolicyNumber = v_vFieldArray(InsuranceFileConst.ACOldPolicyNumber)

        m_vQuoteExpiryDate = v_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate)

        m_vAlternateAccountCnt = v_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt)

        m_vCommissionPercentage = v_vFieldArray(InsuranceFileConst.ACCommissionPercentage)

        If v_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFieldArraySize Then
            m_lAccountExecutiveCnt = v_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt)
        End If

        m_vAnniversaryDate = v_vFieldArray(InsuranceFileConst.ACAnniversaryDate)

        m_vPolicyStyleID = v_vFieldArray(InsuranceFileConst.ACPolicyStyleID)
        m_vPolicyStatusID = v_vFieldArray(InsuranceFileConst.ACPolicyStatusID)
        m_vInceptionTPI = v_vFieldArray(InsuranceFileConst.ACInceptionTPI)

        m_vUnderwritingYearID = v_vFieldArray(InsuranceFileConst.ACUnderwritingYearID)
        'FSA Phase III
        m_vFSACustomerCategoryID = v_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID)
        m_vFSAContractLocationID = v_vFieldArray(InsuranceFileConst.ACFSAContractLocationID)
        m_vFSAUnderwriterCnt = v_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt)
        m_vFSATypeOfSaleID = v_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID)
        m_vFSARenewalConsent = v_vFieldArray(InsuranceFileConst.ACFSARenewalConsent)
        'FSA Phase III End
        m_iDiscountReasonID = CInt(v_vFieldArray(InsuranceFileConst.ACDiscountReasonID))
        m_vDiscountedPremium = v_vFieldArray(InsuranceFileConst.ACDiscountedPremium)
        m_vDiscountPercentage = v_vFieldArray(InsuranceFileConst.ACDiscountPercentage)

        m_iMatchDiscountedPremiumFlag = CInt(v_vFieldArray(InsuranceFileConst.ACMatchDiscountedPremiumFlag))
        m_lPutOnNextInstalmentRenewal = v_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal)
        m_lAnniversaryCopy = v_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy)

        m_lDiscountRecurringTypeID = CInt(v_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId))

        m_lCountryId = v_vFieldArray(InsuranceFileConst.ACCountryID)

        m_iLeadAgentAllowCommission = CInt(v_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission))
        m_iSubAgentAllowCommission = CInt(v_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission))
        m_vCCTermsAgreed = v_vFieldArray(InsuranceFileConst.ACCCTermsAgreed)
        m_vCCTermsAgreedDate = v_vFieldArray(InsuranceFileConst.ACCCTermsAgreedDate)
        m_vCCInceptionDate = v_vFieldArray(InsuranceFileConst.ACCCInceptionDate)
        m_vCCPolicyDocumentsIssuedDate = v_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentsIssuedDate)
        m_vCCPolicyDocumentCorrect = v_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentCorrect)
        m_vCCErrorNotificationDate = v_vFieldArray(InsuranceFileConst.ACCCErrorNotificationDate)

        m_vRenewalPremium = gPMFunctions.ToSafeInteger(v_vFieldArray(InsuranceFileConst.ACRenewalPremium), 0)

        m_vRenewalProductID = v_vFieldArray(InsuranceFileConst.ACRenewalProductID)
        m_vOriginalProductID = v_vFieldArray(InsuranceFileConst.ACOriginalProductID)
        m_vRiskTransferAgreement = v_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement)
        m_vRiskTransferEditable = v_vFieldArray(InsuranceFileConst.ACFSARiskTransferEditable)
        m_vCurrencyToBaseXRate = v_vFieldArray(InsuranceFileConst.ACCurrencyToBaseXRate)
        m_vManualDiscountPercentage = v_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage)

        m_vQuoteStatus = CInt(v_vFieldArray(InsuranceFileConst.ACQuoteStatusID))
        m_vQuoteVersion = CInt(v_vFieldArray(InsuranceFileConst.ACQuoteVersionID))
        m_vBaseInsuranceFolder = CInt(v_vFieldArray(InsuranceFileConst.ACBaseInsuranceFolderCnt))

        m_vContactuserId = CInt(v_vFieldArray(InsuranceFileConst.ACContactuserId))
        m_iMTAReasonId = CInt(v_vFieldArray(InsuranceFileConst.ACMTAReasonId))
        m_sCoInsPlacement = CStr(v_vFieldArray(InsuranceFileConst.kCoInsPlacement))
        m_nIsMarketPlacePolicy = CInt(v_vFieldArray(InsuranceFileConst.ACIsMarketPlacePolicy))

        m_oCollectionFrequencyID = v_vFieldArray(InsuranceFileConst.kCollectionFrequencyId)
        m_oPaymentTermsID = v_vFieldArray(InsuranceFileConst.kPaymentTermId)

        m_iCorrespondenceType = CInt(v_vFieldArray(InsuranceFileConst.ACCorrespondenceType))
        m_iDefaultPreferredCorrespondence = CInt(v_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence))
        m_bIsAgentCorrepondence = CInt(v_vFieldArray(InsuranceFileConst.ACIsAgentCorrespondence))
        m_vSenderEmail = ToSafeString(v_vFieldArray(InsuranceFileConst.ACSenderEmail))
        m_vReceiverEmail = ToSafeString(v_vFieldArray(InsuranceFileConst.ACReceiverEmail))
        m_vOriginalInsuranceFileTypeId = ToSafeString(v_vFieldArray(InsuranceFileConst.ACOriginalInsuranceFileTypeId))

        Return nResult

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_vFieldArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetProperties(ByRef r_vFieldArray() As Object) As Integer

        Dim result As Integer = 0
        Dim sShortName As String = String.Empty
        Dim sResolvedName As String = String.Empty



        result = gPMConstants.PMEReturnCode.PMTrue

        ReDim r_vFieldArray(InsuranceFileConst.ACFieldArraySize)

        r_vFieldArray(InsuranceFileConst.ACInsuranceFileCnt) = m_lInsuranceFileCnt

        r_vFieldArray(InsuranceFileConst.ACInsuranceFileStructureID) = m_lInsuranceFileStructureID

        r_vFieldArray(InsuranceFileConst.ACInsuranceFileTypeID) = m_lInsuranceFileTypeID

        r_vFieldArray(InsuranceFileConst.ACInsuranceFileStatusID) = m_vInsuranceFileStatusID

        r_vFieldArray(InsuranceFileConst.ACInsuranceFileID) = m_lInsuranceFileID

        r_vFieldArray(InsuranceFileConst.ACSourceID) = m_iSourceID

        r_vFieldArray(InsuranceFileConst.ACInsuranceFolderCnt) = m_lInsuranceFolderCnt

        r_vFieldArray(InsuranceFileConst.ACInsuranceRef) = m_sInsuranceRef

        r_vFieldArray(InsuranceFileConst.ACProductID) = m_lProductID

        r_vFieldArray(InsuranceFileConst.ACLeadInsurerCnt) = m_lLeadInsurerCnt

        r_vFieldArray(InsuranceFileConst.ACLeadAgentCnt) = m_lLeadAgentCnt

        r_vFieldArray(InsuranceFileConst.ACLeadAgentPercent) = m_vLeadAgentPercent

        r_vFieldArray(InsuranceFileConst.ACAccountHandlerCnt) = m_lAccountHandlerCnt

        r_vFieldArray(InsuranceFileConst.ACInsuredCnt) = m_lInsuredCnt

        r_vFieldArray(InsuranceFileConst.ACBusinessTypeID) = m_iBusinessTypeID

        r_vFieldArray(InsuranceFileConst.ACCollectTypeID) = m_vCollectTypeID

        r_vFieldArray(InsuranceFileConst.ACCollectionFromCnt) = m_lCollectionFromCnt

        r_vFieldArray(InsuranceFileConst.ACSubBranchID) = m_vSubBranchID

        r_vFieldArray(InsuranceFileConst.ACCurrencyID) = m_iCurrencyID

        r_vFieldArray(InsuranceFileConst.ACBaseCurrencyID) = m_iBaseCurrencyID

        r_vFieldArray(InsuranceFileConst.ACLanguageID) = m_iLanguageID

        r_vFieldArray(InsuranceFileConst.ACDateIssued) = m_vDateIssued

        r_vFieldArray(InsuranceFileConst.ACCoverStartDate) = m_dtCoverStartDate

        r_vFieldArray(InsuranceFileConst.ACExpiryDate) = m_dtExpiryDate

        r_vFieldArray(InsuranceFileConst.ACRenewalDate) = m_dtRenewalDate

        r_vFieldArray(InsuranceFileConst.ACRenewalMethodID) = m_vRenewalMethodID

        r_vFieldArray(InsuranceFileConst.ACRenewalFrequencyID) = m_iRenewalFrequencyID

        r_vFieldArray(InsuranceFileConst.ACIsReferredAtRenewal) = m_iIsReferredAtRenewal

        r_vFieldArray(InsuranceFileConst.ACLapsedReasonID) = m_vLapsedReasonID

        r_vFieldArray(InsuranceFileConst.ACLapsedDate) = m_vLapsedDate

        r_vFieldArray(InsuranceFileConst.ACLapsedDescription) = m_vLapsedDescription

        r_vFieldArray(InsuranceFileConst.ACIsReferredOnMta) = m_iIsReferredOnMta

        r_vFieldArray(InsuranceFileConst.ACPolicyVersion) = m_iPolicyVersion

        r_vFieldArray(InsuranceFileConst.ACGeminiPolicyStatus) = m_vGeminiPolicyStatus

        r_vFieldArray(InsuranceFileConst.ACGeminiBusinessType) = m_vGeminiBusinessType

        r_vFieldArray(InsuranceFileConst.ACDeferredInd) = m_vDeferredInd

        r_vFieldArray(InsuranceFileConst.ACPolicyIgnore) = m_vPolicyIgnore

        r_vFieldArray(InsuranceFileConst.ACBrokerCnt) = m_vBrokerCnt

        r_vFieldArray(InsuranceFileConst.ACRiskCodeId) = m_vRiskCodeID

        r_vFieldArray(InsuranceFileConst.ACAnalysisCodeId) = m_vAnalysisCodeID

        r_vFieldArray(InsuranceFileConst.ACPolicyDeductibleId) = m_vPolicyDeductiblesID

        r_vFieldArray(InsuranceFileConst.ACPolicyLimitsId) = m_vPolicyLimitsID

        r_vFieldArray(InsuranceFileConst.ACProposalDate) = m_vProposalDate

        r_vFieldArray(InsuranceFileConst.ACDiaryDate) = m_vDiaryDate

        r_vFieldArray(InsuranceFileConst.ACReviewDate) = m_vReviewDate

        r_vFieldArray(InsuranceFileConst.ACRenewalDayNumber) = m_vRenewalDayNumber

        r_vFieldArray(InsuranceFileConst.ACPolicyTypeId) = m_vPolicyTypeId

        r_vFieldArray(InsuranceFileConst.ACIndicator) = m_vIndicator

        r_vFieldArray(InsuranceFileConst.ACClause) = m_vClause

        r_vFieldArray(InsuranceFileConst.ACCover) = m_vCover

        r_vFieldArray(InsuranceFileConst.ACArea) = m_vArea

        r_vFieldArray(InsuranceFileConst.ACLongTermUndertakingDate) = m_vLongTermUndertakingDate

        r_vFieldArray(InsuranceFileConst.ACRenewalStopCodeID) = m_vRenewalStopCodeID

        r_vFieldArray(InsuranceFileConst.ACVBSType) = m_vVBSType

        r_vFieldArray(InsuranceFileConst.ACVBSStatus) = m_vVBSStatus

        r_vFieldArray(InsuranceFileConst.ACIsInsurerRateTable) = m_vIsInsurerRateTable

        r_vFieldArray(InsuranceFileConst.ACIsRelatedPolicies) = m_vIsRelatedPolicies

        r_vFieldArray(InsuranceFileConst.ACIsRetainedDocuments) = m_vIsRetainedDocuments

        r_vFieldArray(InsuranceFileConst.ACSchemesPostcode) = m_vSchemesPostcode

        r_vFieldArray(InsuranceFileConst.ACPaidDirect) = m_vPaidDirect

        r_vFieldArray(InsuranceFileConst.ACScheme) = m_vScheme

        r_vFieldArray(InsuranceFileConst.ACBrokerageAmount) = m_vBrokerageAmount

        r_vFieldArray(InsuranceFileConst.ACIsMinimumBrokerageFlag) = m_vIsMinimumBrokerageFlag

        r_vFieldArray(InsuranceFileConst.ACAnnualPremium) = m_vAnnualPremium

        r_vFieldArray(InsuranceFileConst.ACThisPremium) = m_vThisPremium

        r_vFieldArray(InsuranceFileConst.ACNetPremium) = m_vNetPremium

        r_vFieldArray(InsuranceFileConst.ACCommissionAmount) = m_vCommissionAmount

        r_vFieldArray(InsuranceFileConst.ACIPTableAmount) = m_vIPTableAmount

        r_vFieldArray(InsuranceFileConst.ACIPTPercentage) = m_vIPTPercentage

        r_vFieldArray(InsuranceFileConst.ACIsIPTOverridden) = m_vIsIPTOverridden

        r_vFieldArray(InsuranceFileConst.ACTaxAmount) = m_vTaxAmount

        r_vFieldArray(InsuranceFileConst.ACVatableAmount) = m_vVatableAmount

        r_vFieldArray(InsuranceFileConst.ACVatPercentage) = m_vVatPercentage

        r_vFieldArray(InsuranceFileConst.ACVatAmount) = m_vVatAmount

        r_vFieldArray(InsuranceFileConst.ACPaymentMethod) = m_vPaymentMethod

        r_vFieldArray(InsuranceFileConst.ACUserDefinedDataID) = m_vUserDefinedDataID

        r_vFieldArray(InsuranceFileConst.ACCommissionPercentage) = m_vCommissionPercentage

        r_vFieldArray(InsuranceFileConst.ACInvariantKey) = m_vInvariantKey

        If (Convert.IsDBNull(m_vInsuredName) Or Informations.IsNothing(m_vInsuredName)) And CInt(m_lInsuredCnt) <> 0 Then

            m_lReturn = GetPartyNameFromCnt(v_lPartyCnt:=m_lInsuredCnt, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName)
            m_vInsuredName = sResolvedName
        End If

        r_vFieldArray(InsuranceFileConst.ACInsuredName) = m_vInsuredName

        r_vFieldArray(InsuranceFileConst.ACAlternateReference) = m_vAlternateReference

        r_vFieldArray(InsuranceFileConst.ACIsClientInvoiced) = m_vIsClientInvoiced

        r_vFieldArray(InsuranceFileConst.ACOldPolicyNumber) = m_vOldPolicyNumber

        r_vFieldArray(InsuranceFileConst.ACQuoteExpiryDate) = m_vQuoteExpiryDate

        r_vFieldArray(InsuranceFileConst.ACAlternateAccountCnt) = m_vAlternateAccountCnt

        If r_vFieldArray.GetUpperBound(0) >= InsuranceFileConst.ACFieldArraySize Then
            r_vFieldArray(InsuranceFileConst.ACAccountExecutiveCnt) = m_lAccountExecutiveCnt
        End If

        r_vFieldArray(InsuranceFileConst.ACAnniversaryDate) = m_vAnniversaryDate

        r_vFieldArray(InsuranceFileConst.ACPolicyStyleID) = m_vPolicyStyleID

        r_vFieldArray(InsuranceFileConst.ACPolicyStatusID) = m_vPolicyStatusID

        r_vFieldArray(InsuranceFileConst.ACInceptionTPI) = m_vInceptionTPI

        r_vFieldArray(InsuranceFileConst.ACUnderwritingYearID) = m_vUnderwritingYearID
        r_vFieldArray(InsuranceFileConst.ACFSACustomerCategoryID) = m_vFSACustomerCategoryID

        r_vFieldArray(InsuranceFileConst.ACFSAContractLocationID) = m_vFSAContractLocationID

        r_vFieldArray(InsuranceFileConst.ACFSAUnderwriterCnt) = m_vFSAUnderwriterCnt

        r_vFieldArray(InsuranceFileConst.ACFSATypeOfSaleID) = m_vFSATypeOfSaleID

        r_vFieldArray(InsuranceFileConst.ACFSARenewalConsent) = m_vFSARenewalConsent

        r_vFieldArray(InsuranceFileConst.ACDiscountReasonID) = m_iDiscountReasonID

        r_vFieldArray(InsuranceFileConst.ACDiscountedPremium) = m_vDiscountedPremium

        r_vFieldArray(InsuranceFileConst.ACDiscountPercentage) = m_vDiscountPercentage

        r_vFieldArray(InsuranceFileConst.ACMatchDiscountedPremiumFlag) = m_iMatchDiscountedPremiumFlag

        r_vFieldArray(InsuranceFileConst.ACInsuranceFilePutOnNextInstalmentRenewal) = m_lPutOnNextInstalmentRenewal

        r_vFieldArray(InsuranceFileConst.ACInsuranceFileAnniversaryCopy) = m_lAnniversaryCopy

        r_vFieldArray(InsuranceFileConst.ACDiscountRecurringTypeId) = m_lDiscountRecurringTypeID

        r_vFieldArray(InsuranceFileConst.ACCountryID) = m_lCountryId

        r_vFieldArray(InsuranceFileConst.ACLeadAllowConsolidatedCommission) = m_iLeadAgentAllowCommission

        r_vFieldArray(InsuranceFileConst.ACSubAllowConsolidatedCommission) = m_iSubAgentAllowCommission

        r_vFieldArray(InsuranceFileConst.ACCCTermsAgreed) = m_vCCTermsAgreed

        r_vFieldArray(InsuranceFileConst.ACCCTermsAgreedDate) = m_vCCTermsAgreedDate

        r_vFieldArray(InsuranceFileConst.ACCCInceptionDate) = m_vCCInceptionDate

        r_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentsIssuedDate) = m_vCCPolicyDocumentsIssuedDate

        r_vFieldArray(InsuranceFileConst.ACCCPolicyDocumentCorrect) = m_vCCPolicyDocumentCorrect

        r_vFieldArray(InsuranceFileConst.ACCCErrorNotificationDate) = m_vCCErrorNotificationDate

        r_vFieldArray(InsuranceFileConst.ACRenewalPremium) = m_vRenewalPremium

        r_vFieldArray(InsuranceFileConst.ACRenewalProductID) = m_vRenewalProductID

        r_vFieldArray(InsuranceFileConst.ACOriginalProductID) = m_vOriginalProductID

        r_vFieldArray(InsuranceFileConst.ACFSARiskTransferAgreement) = m_vRiskTransferAgreement

        r_vFieldArray(InsuranceFileConst.ACFSARiskTransferEditable) = m_vRiskTransferEditable

        r_vFieldArray(InsuranceFileConst.ACCurrencyToBaseXRate) = m_vCurrencyToBaseXRate

        r_vFieldArray(InsuranceFileConst.ACManualDiscountPercentage) = m_vManualDiscountPercentage

        r_vFieldArray(InsuranceFileConst.ACQuoteStatusID) = m_vQuoteStatus
        r_vFieldArray(InsuranceFileConst.ACQuoteVersionID) = m_vQuoteVersion
        r_vFieldArray(InsuranceFileConst.ACBaseInsuranceFolderCnt) = m_vBaseInsuranceFolder

        r_vFieldArray(InsuranceFileConst.ACContactuserId) = m_vContactuserId
        r_vFieldArray(InsuranceFileConst.ACMTAReasonId) = m_iMTAReasonId
        r_vFieldArray(InsuranceFileConst.kCoInsPlacement) = m_sCoInsPlacement
        r_vFieldArray(InsuranceFileConst.ACIsMarketPlacePolicy) = m_nIsMarketPlacePolicy

        r_vFieldArray(InsuranceFileConst.kCollectionFrequencyId) = m_oCollectionFrequencyID
        r_vFieldArray(InsuranceFileConst.kPaymentTermId) = m_oPaymentTermsID

        r_vFieldArray(InsuranceFileConst.ACCorrespondenceType) = m_iCorrespondenceType
        r_vFieldArray(InsuranceFileConst.ACDefaultPreferredCorrespondence) = m_iDefaultPreferredCorrespondence
        r_vFieldArray(InsuranceFileConst.ACIsAgentCorrespondence) = m_bIsAgentCorrepondence

        r_vFieldArray(InsuranceFileConst.ACSenderEmail) = m_vSenderEmail
        r_vFieldArray(InsuranceFileConst.ACReceiverEmail) = m_vReceiverEmail
        r_vFieldArray(InsuranceFileConst.ACOriginalInsuranceFileTypeId) = m_vOriginalInsuranceFileTypeId

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CopyPolicyAgents
    '
    ' Description: Update the scheme id in the gis_policy_link table.
    '              THese can then be used for
    '
    ' ***************************************************************** '
    Private Function CopyPolicyAgents(ByRef v_lOldInsuranceFileCnt As Integer, ByRef v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' GIS policy link id
        lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add OldInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAgents")

            Return result
        End If

        ' GIS scheme id
        lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add NewInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAgents")

            Return result
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPolicyAgentsSQL, sSQLName:=ACCopyPolicyAgentsName, bStoredProcedure:=ACCopyPolicyAgentsStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed for " & ACCopyPolicyAgentsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAgents")

            Return result
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)


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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'MSS210901 - Added for merge

    ' ***************************************************************** '
    '
    ' Name: MakeEvent
    '
    ' Description:
    '
    ' History: 24/04/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function MakeEvent() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue




            m_oSIRInsuranceFile.EventDescription = EventDescription

            m_lReturn = m_oSIRInsuranceFile.MakeEvent()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MakeEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MakeEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Function Name : GetPolicySubAgentDetails
    ' Description   : Function to get the Policy SubAgent Details for the
    '                 given Insurance_file_cnt (if one has been assigned).
    ' Author        : Chris Barnes
    ' Edit History  :
    ' CJB20040810   : Created       Ref. PN Issue No. 13723
    ' ***************************************************************** '
    'Developer Guide No 101
    Private Function GetPolicySubAgentDetails(ByVal v_lInsuranceFileCnt As Object, ByRef r_vSubAgentName As Object, ByRef r_vSubAgentCode As Object) As Object

        Dim result As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResultArray(,) As Object = Nothing

        ' Check if we have a valid insurance file cnt
        If v_lInsuranceFileCnt <= 0 Then
            ' Invalid Insurance File Cnt
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid insurance_file_cnt. v_lInsuranceFileCnt = " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicySubAgentDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetPolicySubAgentSQL, sSQLName:=ACGetPolicySubAgentName, bStoredProcedure:=ACGetPolicySubAgentStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch the Policy SubAgent Information for v_lInsuranceFileCnt = " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicySubAgentDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End With

        If Not (Informations.IsArray(vResultArray)) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' We got an array
        ' NOTE : It is only possible to have one subagent for a policy

        r_vSubAgentName = CStr(vResultArray(1, 0)).Trim()

        r_vSubAgentCode = CStr(vResultArray(0, 0)).Trim()

        Return result

    End Function
    'MKW 150606 END

    Public Function FixDuplicatePolicyNumber() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oSIRInsuranceFolder

                ' Add a record to the database from the object
                m_lReturn = .RemoveDuplicatePolicy(InsuranceFolderCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Fix duplicate policy number Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FixDuplicatePolicyNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetDetailsNoLookup (Public)
    '
    ' Description: Gets the required SIRInsuranceFile entity properties.
    '
    ' ***************************************************************** '
    Public Function GetDetailsNoLookup() As Integer

        Dim result As Integer = 0
        Dim vFieldArray As Object = Nothing
        Dim sLookupTable As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for InsuranceFile identity
            m_lReturn = GetIdentity()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the InsuranceFile Properties
            With m_oSIRInsuranceFile
                'ECK 9/8/99
                .FromEvent = FromEvent
                m_lReturn = .GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)

                m_lReturn = .GetNext(r_vFieldArray:=vFieldArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = SetProperties(v_vFieldArray:=vFieldArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                vFieldArray = Nothing

            End With

            ' Set the InsuranceFileSystem Properties
            With m_oSIRInsuranceFileSystem
                'ECK 9/8/99
                .FromEvent = FromEvent
                m_lReturn = .GetDetails(vInsuranceFileCnt:=m_lInsuranceFileCnt)

                m_lReturn = .GetNext(vEndorsementCount:=m_lEndorsementCount, vCreatedByID:=m_iCreatedByID, vDateCreated:=m_dtDateCreated, vModifiedByID:=m_vModifiedByID, vLastModified:=m_vLastModified, vLastTransDate:=m_vLastTransDate, vLastTransTypeID:=m_vLastTransTypeID, vLastTransDescription:=m_vLastTransDescription, vLastTransDebitCredit:=m_vLastTransDebitCredit, vLastTransDocumentRef:=m_vLastTransDocumentRef, vLastTransCoverStartDate:=m_vLastTransCoverStartDate, vLastTransExpiryDate:=m_vLastTransExpiryDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            ' Set the InsuranceFolder Properties
            With m_oSIRInsuranceFolder
                'ECK 9/8/99
                .FromEvent = FromEvent
                If Not FromEvent Then

                    'Developer Guide No. 98

                    m_lReturn = .GetDetails(vInsuranceFolderCnt:=m_lInsuranceFolderCnt)
                Else
                    m_lReturn = .GetDetails(vInsuranceFolderCnt:=m_lInsuranceFileCnt)
                End If

                m_lReturn = .GetNext(vInsuranceFolderID:=m_lInsuranceFolderID, vInsuranceHolderCnt:=m_lInsuranceHolderCnt, vCode:=m_sInsuranceFolderCode, vDescription:=m_vInsuranceFolderDescription, vInceptionDate:=m_vInceptionDate, vArcArchiveFolderID:=m_vArcArchiveFolderID, vQuoteInsuranceRef:=m_vQuoteInsuranceRef, vNextInsuranceRef:=m_vNextInsuranceRef, vLastInsuranceRef:=m_vLastInsuranceRef, vRenewalCount:=m_lRenewalCount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            m_sInsuranceFileStructure = "PMB"

            m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsNoLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsNoLookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            ' PWF 06/11/2002 - Moved from before Exit Function (potential infinite loops)



            Return result
        End Try
    End Function

    ''' <summary>
    ''' Get Do Not Merge Clauses
    ''' </summary>
    ''' <param name="r_oClauses"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDoNotMergeClauses(ByRef r_oClauses As Object) As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=m_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=kGetDoNotMergeClausesSQL, sSQLName:=kGetDoNotMergeClausesName, bStoredProcedure:=kGetDoNotMergeClausesStored, vResultArray:=r_oClauses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch the Policy do not merge clauses information for v_lInsuranceFileCnt = " & m_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicySubAgentDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
            End With

            If Not (Informations.IsArray(r_oClauses)) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDoNotMergeClauses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDoNotMergeClauses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Update Policy status->Update all lapse versions to replace status
    ''' </summary>     
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateStatusLapsePolicyVersions(ByVal v_nInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nResult = m_oDatabase.SQLAction(sSQL:=ACUpdateLapseVersionsStatusSQL, sSQLName:=ACUpdateLapseVersionsStatusName, bStoredProcedure:=ACUpdateLapseVersionsStatusStored)


            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStatusLapsePolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatusLapsePolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

End Class
