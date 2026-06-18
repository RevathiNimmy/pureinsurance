Imports System.Configuration
Imports Nexus
Namespace Config

    Public Class Portal : Inherits ConfigurationElement

        Private cpID As ConfigurationProperty
        Private cpName As ConfigurationProperty

        Private cpAddressControl As ConfigurationProperty
        Private cpCountries As ConfigurationProperty
        Private cpPaymentTypes As ConfigurationProperty
        Private cpProducts As ConfigurationProperty
        Private cpEServicing As ConfigurationProperty
        Private cpEmailTemplates As ConfigurationProperty
        Private cpTasks As ConfigurationProperty
        Private cpClaims As ConfigurationProperty
        Private cpReports As ConfigurationProperty
        Private cpDefaultUserType As ConfigurationProperty
        Private cpDetailedReferralReasons As ConfigurationProperty
        Private cpDetailedDeclineReasons As ConfigurationProperty
        Private cpShowRiskSummary As ConfigurationProperty
        Private cpShowStatements As ConfigurationProperty
        Private cpSkipPaymentSelect As ConfigurationProperty


        Private cpMTAReasonForCancellation As ConfigurationProperty

        Private cpEnableFileCodeSearch As ConfigurationProperty
        Private cpDocumentFormat As ConfigurationProperty
        Private cpAgentStartPage As ConfigurationProperty
        Private cpClientStartPage As ConfigurationProperty
        Private cpAddressMandatoryFields As ConfigurationProperty
        Private cpPCMandatoryFields As ConfigurationProperty
        Private cpCCMandatoryFields As ConfigurationProperty
        Private cpAllowFileCodeField As ConfigurationProperty
        Private cpConfirmDetailsOnRenewal As ConfigurationProperty
        Private cpAllowLapsePolicy As ConfigurationProperty
        Private cpPolicyLapseEmail As ConfigurationProperty
        Private cpAnnPartyKey As ConfigurationProperty
        Private cpViewOnlyLatestPolicyVersion As ConfigurationProperty
        Private cpAllowMultipleLogins As ConfigurationProperty
        Private cpPostcodevalidationRegex As ConfigurationProperty
        Private cpDuplicateClientCheckParameters As ConfigurationProperty
        Private cpDuplicateClientSearchType As ConfigurationProperty
        Private cpAllowRole As ConfigurationProperty
        Private cpMaxSearchResults As ConfigurationProperty
        Private cpTempFileLocation As ConfigurationProperty
        Private cpcProperties As ConfigurationPropertyCollection
        Private cpFileTypes As ConfigurationProperty
        Private cpAddressReadOnlyFields As ConfigurationProperty
        Private cpAddressLookupFields As ConfigurationProperty
        Private cpAddressLookupMandatoryFields As ConfigurationProperty
        Private cpShowEmailPopUp As ConfigurationProperty
        Private cpFormatStrings As ConfigurationProperty
        Private cpShowEmailButtonForError As ConfigurationProperty
        Private cpSupportEmailId As ConfigurationProperty
        Private cpEditEndorsements As ConfigurationProperty
        Private cpViewEndorsements As ConfigurationProperty
        Private cpUseCorePolicyHeader As ConfigurationProperty
        Private cpMaskBankAccountNumber As ConfigurationProperty

        Private cpBackGroundCashListProcess As ConfigurationProperty
        Private cpForceToViewClaimPayment As ConfigurationProperty
        Private cpEnableMasterClientAssociate As ConfigurationProperty


        Private cpAllocationUserGroup As ConfigurationProperty
        Private cpExternalTaskCategoryCode As ConfigurationProperty
        Private cpTaskGroup As ConfigurationProperty
        Private cpExternalWorkItemUserGroup As ConfigurationProperty
        Private cpFindPolicyMandatoryFields As ConfigurationProperty
        Private cpDefaultToLastPaymentMethod As ConfigurationProperty
        Private cpDoNotCreateClaimVersionOnSalvageReceipt As ConfigurationProperty
        Private cpDuplicateClaimPaymentCheckParameters As ConfigurationProperty
        Private cpShowSubBranchForPosting As ConfigurationProperty

        Private cpEnableTXTextControl As ConfigurationProperty
        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpID = New ConfigurationProperty("ID", GetType(Integer), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing)

            cpAddressControl = New ConfigurationProperty("AddressControl", GetType(AddressControl), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpCountries = New ConfigurationProperty("Countries", GetType(Countries), Nothing)

            cpPaymentTypes = New ConfigurationProperty("PaymentTypes", GetType(PaymentTypes), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpProducts = New ConfigurationProperty("Products", GetType(Products), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpEServicing = New ConfigurationProperty("EServicing", GetType(EServicing), Nothing)

            cpEmailTemplates = New ConfigurationProperty("EmailTemplates", GetType(EmailTemplates), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpTasks = New ConfigurationProperty("Tasks", GetType(Tasks), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpClaims = New ConfigurationProperty("Claims", GetType(Claims), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpReports = New ConfigurationProperty("Reports", GetType(Reports), Nothing,
             ConfigurationPropertyOptions.IsRequired)

            cpAgentStartPage = New ConfigurationProperty("AgentStartPage", GetType(String), Nothing,
             ConfigurationPropertyOptions.IsRequired)

            cpClientStartPage = New ConfigurationProperty("ClientStartPage", GetType(String), Nothing,
             ConfigurationPropertyOptions.IsRequired)

            cpDefaultUserType = New ConfigurationProperty("DefaultUserType", GetType(String), "Personal",
                ConfigurationPropertyOptions.IsRequired)

            cpDetailedReferralReasons = New ConfigurationProperty("DetailedReferralReasons", GetType(Boolean), False)
            cpDetailedDeclineReasons = New ConfigurationProperty("DetailedDeclineReasons", GetType(Boolean), False)

            cpShowRiskSummary = New ConfigurationProperty("ShowRiskSummary", GetType(Boolean), "true")

            cpShowStatements = New ConfigurationProperty("ShowStatements", GetType(Boolean), "true")
            cpSkipPaymentSelect = New ConfigurationProperty("SkipPaymentSelect", GetType(Boolean), "true")

            cpMTAReasonForCancellation = New ConfigurationProperty("MTAReasonForCancellation", GetType(String), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpEnableFileCodeSearch = New ConfigurationProperty("EnableFileCodeSearch", GetType(Boolean), Nothing,
             ConfigurationPropertyOptions.IsRequired)
            cpDocumentFormat = New ConfigurationProperty("DocumentFormat", GetType(String), Nothing,
                                                  ConfigurationPropertyOptions.IsRequired)
            cpAddressMandatoryFields = New ConfigurationProperty("AddressMandatoryFields", GetType(String), Nothing)
            cpPCMandatoryFields = New ConfigurationProperty("PCMandatoryFields", GetType(String), Nothing)
            cpCCMandatoryFields = New ConfigurationProperty("CCMandatoryFields", GetType(String), Nothing)

            cpAllowFileCodeField = New ConfigurationProperty("AllowFileCodeField", GetType(Boolean), Nothing, ConfigurationPropertyOptions.IsRequired)

            cpViewOnlyLatestPolicyVersion = New ConfigurationProperty("ViewOnlyLatestPolicyVersion", GetType(Boolean), Nothing,
              ConfigurationPropertyOptions.IsRequired)

            cpConfirmDetailsOnRenewal = New ConfigurationProperty("ConfirmDetailsOnRenewal", GetType(Boolean), Nothing,
                                    ConfigurationPropertyOptions.IsRequired)

            cpAllowLapsePolicy = New ConfigurationProperty("AllowLapsePolicy", GetType(Boolean), Nothing,
                                  ConfigurationPropertyOptions.IsRequired)

            cpPolicyLapseEmail = New ConfigurationProperty("PolicyLapseEmail", GetType(Boolean), Nothing,
                                 ConfigurationPropertyOptions.IsRequired)

            cpAnnPartyKey = New ConfigurationProperty("AnnPartyID", GetType(String), Nothing,
                                 ConfigurationPropertyOptions.IsRequired)

            cpAllowMultipleLogins = New ConfigurationProperty("AllowMultipleLogins", GetType(Boolean), Nothing,
                                 ConfigurationPropertyOptions.IsRequired)

            cpPostcodevalidationRegex = New ConfigurationProperty("PostcodeValidationRegex", GetType(String), Nothing,
                                ConfigurationPropertyOptions.IsRequired)

            cpDuplicateClientCheckParameters = New ConfigurationProperty("DuplicateClientCheckParameters", GetType(String), Nothing)

            cpAllowRole = New ConfigurationProperty("AllowRole", GetType(String), Nothing)

            cpDuplicateClientSearchType = New ConfigurationProperty("DuplicateClientSearchType", GetType(String), Nothing)

            cpAddressReadOnlyFields = New ConfigurationProperty("AddressReadOnlyFields", GetType(String), Nothing)
            cpAddressLookupFields = New ConfigurationProperty("AddressLookupFields", GetType(String), Nothing)
            cpAddressLookupMandatoryFields = New ConfigurationProperty("AddressLookupMandatoryFields", GetType(String), Nothing)
            cpMaxSearchResults = New ConfigurationProperty("MaxSearchResults", GetType(Integer), 0,
                                ConfigurationPropertyOptions.IsRequired)

            cpTempFileLocation = New ConfigurationProperty("TempFileLocation", GetType(String), Nothing, ConfigurationPropertyOptions.IsRequired)

            cpFileTypes = New ConfigurationProperty("FileTypes", GetType(FileTypes), Nothing, ConfigurationPropertyOptions.IsRequired)

            cpShowEmailPopUp = New ConfigurationProperty("ShowEmailPopUp", GetType(String), Nothing,
           ConfigurationPropertyOptions.IsRequired)

            cpFormatStrings = New ConfigurationProperty("FormatStrings", GetType(FormatStrings), Nothing,
               ConfigurationPropertyOptions.IsRequired)

            cpShowEmailButtonForError = New ConfigurationProperty("ShowEmailButtonForError", GetType(Boolean), False)
            cpSupportEmailId = New ConfigurationProperty("SupportEmailId", GetType(String), Nothing)
            cpEnableMasterClientAssociate = New ConfigurationProperty("EnableMasterClientAssociate", GetType(Boolean), False)

            cpEditEndorsements = New ConfigurationProperty("EditEndorsements", GetType(String), "word")
            cpViewEndorsements = New ConfigurationProperty("ViewEndorsements", GetType(String), "pdf")

            cpUseCorePolicyHeader = New ConfigurationProperty("UseCorePolicyHeader", GetType(Boolean), False)

            cpAllocationUserGroup = New ConfigurationProperty("AllocationUserGroup", GetType(String), Nothing)
            cpExternalTaskCategoryCode = New ConfigurationProperty("ExternalTaskCategoryCode", GetType(String), Nothing)
            cpTaskGroup = New ConfigurationProperty("TaskGroup", GetType(String), Nothing)
            cpExternalWorkItemUserGroup = New ConfigurationProperty("ExternalWorkItemUserGroup", GetType(String), Nothing)
            cpMaskBankAccountNumber = New ConfigurationProperty("MaskBankAccountNumber", GetType(Boolean), False)


            cpBackGroundCashListProcess = New ConfigurationProperty("BackGroundCashListProcess", GetType(Boolean), False)

            cpForceToViewClaimPayment = New ConfigurationProperty("ForceToViewClaimPayment", GetType(Boolean), False)

            cpFindPolicyMandatoryFields = New ConfigurationProperty("FindPolicyMandatoryFields", GetType(String), Nothing)
            cpDefaultToLastPaymentMethod = New ConfigurationProperty("DefaultToLastPaymentMethod", GetType(Boolean), False)
            cpDoNotCreateClaimVersionOnSalvageReceipt = New ConfigurationProperty("DoNotCreateClaimVersionOnSalvageReceipt", GetType(Boolean), False)
            cpDuplicateClaimPaymentCheckParameters = New ConfigurationProperty("DuplicateClaimPaymentCheckParameters", GetType(String), Nothing)
            cpShowSubBranchForPosting = New ConfigurationProperty("ShowSubBranchForPosting", GetType(Boolean), False)
            cpEnableTXTextControl = New ConfigurationProperty("EnableTXTextControl", GetType(Boolean), False)

            cpcProperties.Add(cpID)
            cpcProperties.Add(cpName)
            cpcProperties.Add(cpAddressControl)
            cpcProperties.Add(cpCountries)
            cpcProperties.Add(cpPaymentTypes)
            cpcProperties.Add(cpProducts)
            cpcProperties.Add(cpEServicing)
            cpcProperties.Add(cpClaims)
            cpcProperties.Add(cpEmailTemplates)
            cpcProperties.Add(cpTasks)
            cpcProperties.Add(cpReports)
            cpcProperties.Add(cpClientStartPage)
            cpcProperties.Add(cpAgentStartPage)
            cpcProperties.Add(cpDefaultUserType)
            cpcProperties.Add(cpDetailedReferralReasons)
            cpcProperties.Add(cpDetailedDeclineReasons)
            cpcProperties.Add(cpShowRiskSummary)
            cpcProperties.Add(cpShowStatements)
            cpcProperties.Add(cpSkipPaymentSelect)

            cpcProperties.Add(cpMTAReasonForCancellation)

            cpcProperties.Add(cpEnableFileCodeSearch)
            cpcProperties.Add(cpDocumentFormat)
            cpcProperties.Add(cpAddressMandatoryFields)
            cpcProperties.Add(cpPCMandatoryFields)
            cpcProperties.Add(cpCCMandatoryFields)
            cpcProperties.Add(cpAllowFileCodeField)
            cpcProperties.Add(cpConfirmDetailsOnRenewal)
            cpcProperties.Add(cpAllowLapsePolicy)
            cpcProperties.Add(cpPolicyLapseEmail)
            cpcProperties.Add(cpAnnPartyKey)
            cpcProperties.Add(cpViewOnlyLatestPolicyVersion)
            cpcProperties.Add(cpAllowMultipleLogins)
            cpcProperties.Add(cpPostcodevalidationRegex)
            cpcProperties.Add(cpDuplicateClientCheckParameters)
            cpcProperties.Add(cpDuplicateClientSearchType)
            cpcProperties.Add(cpAllowRole)
            cpcProperties.Add(cpAddressReadOnlyFields)
            cpcProperties.Add(cpAddressLookupFields)
            cpcProperties.Add(cpAddressLookupMandatoryFields)
            cpcProperties.Add(cpMaxSearchResults)
            cpcProperties.Add(cpTempFileLocation)
            cpcProperties.Add(cpFileTypes)
            cpcProperties.Add(cpShowEmailPopUp)
            cpcProperties.Add(cpFormatStrings)
            cpcProperties.Add(cpShowEmailButtonForError)
            cpcProperties.Add(cpSupportEmailId)
            cpcProperties.Add(cpEditEndorsements)
            cpcProperties.Add(cpViewEndorsements)
            cpcProperties.Add(cpUseCorePolicyHeader)
            cpcProperties.Add(cpBackGroundCashListProcess)
            cpcProperties.Add(cpForceToViewClaimPayment)
            cpcProperties.Add(cpEnableMasterClientAssociate)
            cpcProperties.Add(cpMaskBankAccountNumber)
            cpcProperties.Add(cpAllocationUserGroup)
            cpcProperties.Add(cpExternalTaskCategoryCode)
            cpcProperties.Add(cpTaskGroup)
            cpcProperties.Add(cpExternalWorkItemUserGroup)
            cpcProperties.Add(cpMaskBankAccountNumber)
            cpcProperties.Add(cpFindPolicyMandatoryFields)
            cpcProperties.Add(cpDefaultToLastPaymentMethod)
            cpcProperties.Add(cpDoNotCreateClaimVersionOnSalvageReceipt)
            cpcProperties.Add(cpDuplicateClaimPaymentCheckParameters)
            cpcProperties.Add(cpShowSubBranchForPosting)

            cpcProperties.Add(cpEnableTXTextControl)
        End Sub
        Public ReadOnly Property Reports() As Reports
            Get
                Return CType(MyBase.Item(cpReports), Reports)
            End Get
        End Property

        Public ReadOnly Property ID() As String
            Get
                Return CStr(MyBase.Item(cpID))
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property

        Public ReadOnly Property AddressControl() As AddressControl
            Get
                Return CType(MyBase.Item(cpAddressControl), AddressControl)
            End Get
        End Property

        Public ReadOnly Property Countries() As Countries
            Get
                Return CType(MyBase.Item(cpCountries), Countries)
            End Get
        End Property

        Public ReadOnly Property PaymentTypes() As PaymentTypes
            Get
                Return CType(MyBase.Item(cpPaymentTypes), PaymentTypes)
            End Get
        End Property

        Public ReadOnly Property Products() As Products
            Get
                Return CType(MyBase.Item(cpProducts), Products)
            End Get
        End Property

        Public ReadOnly Property EServicing() As EServicing
            Get
                Return CType(MyBase.Item(cpEServicing), EServicing)
            End Get
        End Property
        Public ReadOnly Property Claims() As Claims
            Get
                Return CType(MyBase.Item(cpClaims), Claims)
            End Get
        End Property

        Public ReadOnly Property EmailTemplates() As EmailTemplates
            Get
                Return CType(MyBase.Item(cpEmailTemplates), EmailTemplates)
            End Get
        End Property
        Public ReadOnly Property Tasks() As Tasks
            Get
                Return CType(MyBase.Item(cpTasks), Tasks)
            End Get
        End Property

        Public ReadOnly Property AgentStartPage() As String
            Get
                Return CStr(MyBase.Item(cpAgentStartPage))
            End Get
        End Property
        Public ReadOnly Property ClientStartPage() As String
            Get
                Return CStr(MyBase.Item(cpClientStartPage))
            End Get
        End Property


        Public ReadOnly Property DefaultUserType() As String
            Get
                Return CStr(MyBase.Item(cpDefaultUserType))
            End Get
        End Property

        Public ReadOnly Property DetailedReferralReasons() As Boolean
            Get
                Return CBool(MyBase.Item(cpDetailedReferralReasons))
            End Get
        End Property

        Public ReadOnly Property DetailedDeclineReasons() As Boolean
            Get
                Return CBool(MyBase.Item(cpDetailedDeclineReasons))
            End Get
        End Property

        Public ReadOnly Property ShowRiskSummary() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowRiskSummary))
            End Get
        End Property

        Public ReadOnly Property ShowStatements() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowStatements))
            End Get
        End Property
        Public ReadOnly Property SkipPaymentSelect() As Boolean
            Get
                Return CBool(MyBase.Item(cpSkipPaymentSelect))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property MTAReasonForCancellation() As String
            Get
                Return CStr(MyBase.Item(cpMTAReasonForCancellation))
            End Get
        End Property



        Public ReadOnly Property DocumentFormat() As String
            Get
                Return CStr(MyBase.Item(cpDocumentFormat))
            End Get
        End Property

        Public ReadOnly Property EnableFileCodeSearch() As Boolean
            Get
                Return CBool(MyBase.Item(cpEnableFileCodeSearch))
            End Get
        End Property

        Public ReadOnly Property AddressReadOnlyFields() As String
            Get
                Return CStr(MyBase.Item(cpAddressReadOnlyFields))
            End Get
        End Property
        Public ReadOnly Property AddressMandatoryFields() As String
            Get
                Return CStr(MyBase.Item(cpAddressMandatoryFields))
            End Get
        End Property

        Public ReadOnly Property PCMandatoryFields() As String
            Get
                Return CStr(MyBase.Item(cpPCMandatoryFields))
            End Get
        End Property

        Public ReadOnly Property CCMandatoryFields() As String
            Get
                Return CStr(MyBase.Item(cpCCMandatoryFields))
            End Get
        End Property
        Public ReadOnly Property AllowFileCodeField() As Boolean
            Get
                Return CBool(MyBase.Item(cpAllowFileCodeField))
            End Get
        End Property

        Public ReadOnly Property AllowLapsePolicy() As Boolean
            Get
                Return CBool(MyBase.Item(cpAllowLapsePolicy))
            End Get
        End Property
        Public ReadOnly Property ConfirmDetailsOnRenewal() As Boolean
            Get
                Return CBool(MyBase.Item(cpConfirmDetailsOnRenewal))
            End Get
        End Property
        Public ReadOnly Property PolicyLapseEmail() As Boolean
            Get
                Return CBool(MyBase.Item(cpPolicyLapseEmail))
            End Get
        End Property
        Public ReadOnly Property AnnPartyID() As String
            Get
                Return CStr(MyBase.Item(cpAnnPartyKey))
            End Get
        End Property

        Public ReadOnly Property ViewOnlyLatestPolicyVersion() As Boolean
            Get
                Return CBool(MyBase.Item(cpViewOnlyLatestPolicyVersion))
            End Get
        End Property

        Public ReadOnly Property AllowMultipleLogins() As Boolean
            Get
                Return CBool(MyBase.Item(cpAllowMultipleLogins))
            End Get
        End Property
        Public ReadOnly Property PostcodeValidationRegex() As String
            Get
                Return CStr(MyBase.Item(cpPostcodevalidationRegex))
            End Get
        End Property
        Public ReadOnly Property DuplicateClientCheckParameters() As String
            Get
                Return CStr(MyBase.Item(cpDuplicateClientCheckParameters))
            End Get
        End Property

        Public ReadOnly Property DuplicateClientSearchType() As String
            Get
                Return CStr(MyBase.Item(cpDuplicateClientSearchType))
            End Get
        End Property

        Public ReadOnly Property AllowRole() As String
            Get
                Return CStr(MyBase.Item(cpAllowRole))
            End Get
        End Property
        Public ReadOnly Property AddressLookupFields() As String
            Get
                Return CStr(MyBase.Item(cpAddressLookupFields))
            End Get
        End Property

        Public ReadOnly Property AddressLookupMandatoryFields() As String
            Get
                Return CStr(MyBase.Item(cpAddressLookupMandatoryFields))
            End Get
        End Property

        Public ReadOnly Property MaxSearchResults() As Integer
            Get
                Return CInt(MyBase.Item(cpMaxSearchResults))
            End Get
        End Property

        Public ReadOnly Property TempFileLocation() As String
            Get
                Return CStr(MyBase.Item(cpTempFileLocation))
            End Get
        End Property

        Public ReadOnly Property FileTypes() As FileTypes
            Get
                Return CType(MyBase.Item(cpFileTypes), FileTypes)
            End Get
        End Property
        Public ReadOnly Property FormatStrings() As FormatStrings
            Get
                Return CType(MyBase.Item(cpFormatStrings), FormatStrings)
            End Get
        End Property

        ''' <summary>
        ''' The user will be forced to View the Screens during Claim Payment process
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ForceToViewClaimPayment() As Boolean
            Get
                Return CBool(MyBase.Item(cpForceToViewClaimPayment))
            End Get
        End Property

        ''' <summary>
        ''' Flag for enable/disable the backgrounf cash list process
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BackGroundCashListProcess() As Boolean
            Get
                Return CBool(MyBase.Item(cpBackGroundCashListProcess))
            End Get
        End Property

        Public ReadOnly Property ShowEmailPopUp() As String
            Get
                Return CStr(MyBase.Item(cpShowEmailPopUp))
            End Get
        End Property

        ''' <summary>
        ''' Report to Admin button visibility for Error Page
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ShowEmailButtonForError() As Boolean
            Get
                Return CStr(MyBase.Item(cpShowEmailButtonForError))
            End Get
        End Property

        ''' <summary>
        ''' Email Id to send error from error page
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SupportEmailId() As String
            Get
                Return CStr(MyBase.Item(cpSupportEmailId))
            End Get
        End Property
        Public ReadOnly Property EditEndorsements() As String
            Get
                Return CStr(MyBase.Item(cpEditEndorsements))
            End Get
        End Property
        Public ReadOnly Property ViewEndorsements() As String
            Get
                Return CStr(MyBase.Item(cpViewEndorsements))
            End Get
        End Property

        ''' <summary>
        ''' Set True if want to use Core Main Detail page instead of Product Maindetail
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UseCorePolicyHeader() As String
            Get
                Return CStr(MyBase.Item(cpUseCorePolicyHeader))
            End Get
        End Property

        Public ReadOnly Property AllocationUserGroup() As String
            Get
                Return CStr(MyBase.Item(cpAllocationUserGroup))
            End Get
        End Property
        Public ReadOnly Property ExternalTaskCategoryCode() As String
            Get
                Return CStr(MyBase.Item(cpExternalTaskCategoryCode))
            End Get
        End Property
        Public ReadOnly Property TaskGroup() As String
            Get
                Return CStr(MyBase.Item(cpTaskGroup))
            End Get
        End Property
        ''' <summary>
        '''  For Activating/Deactivating the Display of the Associates with the Master Client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EnableMasterClientAssociate() As Boolean
            Get
                Return CStr(MyBase.Item(cpEnableMasterClientAssociate))
            End Get
        End Property

        ''' <summary>
        ''' E5 Work Item User Group
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ExternalWorkItemUserGroup() As String
            Get
                Return CStr(MyBase.Item(cpExternalWorkItemUserGroup))
            End Get
        End Property


        Public ReadOnly Property MaskBankAccountNumber() As Boolean
            Get
                Return CBool(MyBase.Item(cpMaskBankAccountNumber))
            End Get
        End Property

        ''' <summary>
        ''' to set comma separated mandatory fields for find policy
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FindPolicyMandatoryFields() As String
            Get
                Return CStr(MyBase.Item(cpFindPolicyMandatoryFields))
            End Get
        End Property

        ''' <summary>
        ''' to set default payment method to last payment method
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DefaultToLastPaymentMethod() As Boolean
            Get
                Return CBool(MyBase.Item(cpDefaultToLastPaymentMethod))
            End Get
        End Property

        Public ReadOnly Property DoNotCreateClaimVersionOnSalvageReceipt() As Boolean
            Get
                Return CBool(MyBase.Item(cpDoNotCreateClaimVersionOnSalvageReceipt))
            End Get
        End Property

        Public ReadOnly Property DuplicateClaimPaymentCheckParameters() As String
            Get
                Return CStr(MyBase.Item(cpDuplicateClaimPaymentCheckParameters))
            End Get
        End Property

        Public ReadOnly Property ShowSubBranchForPosting() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowSubBranchForPosting))
            End Get
        End Property



        Public ReadOnly Property EnableTXTextControl() As Boolean
            Get
                Return CBool(MyBase.Item(cpEnableTXTextControl))
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
