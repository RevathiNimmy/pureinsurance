Option Strict On

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS'''

#Region " Imports "

Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure
Imports Sirius.Architecture.ExceptionHandling
Imports System.Collections.Generic

'Imports SiriusFS.SAM.ServiceAgent.PMEReturnCode

#End Region

Namespace BaseImplementationTypes


    '''<remarks/>
    Public MustInherit Class SAMError
    End Class


    '''<remarks/>
    Public Class SAMErrorBusinessRule
        Inherits SAMError

        Private codeField As Integer

        Private reasonField As SAMErrorCode

        Private descriptionField As String

        Private detailField As String

        '''<remarks/>
        Public Property Code() As Integer

            Get
                Return Me.codeField
            End Get
            Set(ByVal value As Integer)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Reason() As SAMErrorCode
            Get
                Return Me.reasonField
            End Get
            Set(ByVal value As SAMErrorCode)
                Me.reasonField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Detail() As String
            Get
                Return Me.detailField
            End Get
            Set(ByVal value As String)
                Me.detailField = value
            End Set
        End Property
    End Class

    Public Class SAMErrorFatal
        Inherits SAMError

        Private typeField As String

        '''<remarks/>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set(ByVal value As String)
                Me.typeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Enum SAMErrorCode

        '''<remarks/>
        GeneralFailure

        '''<remarks/>
        BackOfficeComponentFailed

        '''<remarks/>
        MandatoryInputMissing

        '''<remarks/>
        InvalidDateFormat

        '''<remarks/>
        InvalidLookupListValue

        '''<remarks/>
        InvalidFormat

        '''<remarks/>
        RecordLockedByAnotherUser

        '''<remarks/>
        RecordNotLockedByCurrentUser

        '''<remarks/>
        BackOfficeUnavailable

        '''<remarks/>
        UserNotAuthorisedToActOnData

        '''<remarks/>
        SecurityCheckFailed

        '''<remarks/>
        TokenExpired

        '''<remarks/>
        RecordChanged

        '''<remarks/>
        BranchCodeInvalid

        '''<remarks/>
        BranchMismatch

        '''<remarks/>
        PolicyMismatch

        '''<remarks/>
        PartyDOBIsInFuture

        '''<remarks/>
        PartyDOBIsTooOld

        '''<remarks/>
        PolicyRiskLinkRecordNotFound

        '''<remarks/>
        QuoteHeaderRecordNotFound

        '''<remarks/>
        CoverStartDateIsInThePast

        '''<remarks/>
        CoverEndDateIsBeforeCoverStartDate

        '''<remarks/>
        PartyRecordNotFound

        '''<remarks/>
        PolicyRecordNotFound

        '''<remarks/>
        FailedToLoadRiskDB

        '''<remarks/>
        FailedToRetrievePremiumDetails

        '''<remarks/>
        ListTypeNotFound

        '''<remarks/>
        AddressRecordNotFound

        '''<remarks/>
        RiskRecordNotFound

        '''<remarks/>
        DefaultXMLFileNotAvailable

        '''<remarks/>
        DefaultXMLFilePathNotFound

        '''<remarks/>
        DefaultXMLFileFailedToLoad

        '''<remarks/>
        DefaultXMLFilePathTooLong

        '''<remarks/>
        ConfigurationFileNotAvailable

        '''<remarks/>
        ConfigurationFilePathNotFound

        '''<remarks/>
        ConfigurationFileFailedToLoad

        '''<remarks/>
        ConfigurationFilePathTooLong

        '''<remarks/>
        XMLDocumentBadlyFormed

        '''<remarks/>
        FailedToCreateBackofficeComponent

        '''<remarks/>
        FailedToInitialiseBackofficeComponent

        '''<remarks/>
        BackOfficeFailed

        '''<remarks/>
        FailedToConnectToTheSiriusDatabase

        '''<remarks/>
        SQLServerReturnedAnError

        '''<remarks/>
        FailedToRetrieveDatamodelCodeFromXml

        '''<remarks/>
        XMLDataSetBadlyFormed

        '''<remarks/>
        DatasetPathRegistrySettingNotFound

        '''<remarks/>
        FailedToAddRiskRecord

        '''<remarks/>
        FailedToMergeRiskDataset

        '''<remarks/>
        UserAuthorityLevelsCheckFailed

        '''<remarks/>
        ValidationRulesFailed

        '''<remarks/>
        FailedToQuoteTheRisk

        '''<remarks/>
        FailedToSaveRiskToDatabase

        '''<remarks/>
        SchemeVersionNumberMissing

        '''<remarks/>
        SchemaForVersionMissing

        '''<remarks/>
        FileNotFound

        '''<remarks/>
        RecordNotFound

        '''<remarks/>
        StatusOfRiskPreventsDeletion

        '''<remarks/>
        AgentRecordNotFound

        '''<remarks/>
        BackOfficeComponentReturnedRecordInUse

        '''<remarks/>
        BackOfficeComponentReturnedNotFound

        '''<remarks/>
        BrokerOrSchemeInvalid

        '''<remarks/>
        ValidationRulesReferred

        '''<remarks/>
        ValidationRulesDeclined

        '''<remarks/>
        UALRulesReferred

        '''<remarks/>
        UALRulesDeclined

        '''<remarks/>
        RatingRulesReferred

        '''<remarks/>
        RatingRulesDeclined

        '''<remarks/>
        LoginFailureIncorrectUsername

        '''<remarks/>
        LoginFailureIncorrectPassword

        '''<remarks/>
        LoginFailureLoggedInElsewhere

        '''<remarks/>
        LoginFailureNotLinkedToAgent
    End Enum

    '''<remarks/>
    Public Class SAMErrorInvalidData
        Inherits SAMError

        Private codeField As Integer

        Private reasonField As SAMErrorCode

        Private descriptionField As String

        Private fieldNameField As String

        Private suppliedValueField As String

        '''<remarks/>
        Public Property Code() As Integer

            Get
                Return Me.codeField
            End Get
            Set(ByVal value As Integer)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Reason() As SAMErrorCode
            Get
                Return Me.reasonField
            End Get
            Set(ByVal value As SAMErrorCode)
                Me.reasonField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FieldName() As String
            Get
                Return Me.fieldNameField
            End Get
            Set(ByVal value As String)
                Me.fieldNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SuppliedValue() As String
            Get
                Return Me.suppliedValueField
            End Get
            Set(ByVal value As String)
                Me.suppliedValueField = value
            End Set
        End Property
    End Class

    Public Class SAMMethodResponseData

        Private errorsField() As SAMError
        Private handlingInstanceIDField As System.Guid

        Public Property Errors() As SAMError()
            Get
                Return Me.errorsField
            End Get
            Set(ByVal value As SAMError())
                Me.errorsField = value
            End Set
        End Property

        Public Property HandlingInstanceID() As System.Guid
            Get
                Return Me.handlingInstanceIDField
            End Get
            Set(ByVal value As System.Guid)
                Me.handlingInstanceIDField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class STSErrorInternalExceptionType

        Private descriptionField As String

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class STSErrorSiriusBackOfficeType

        Private returnValueField As String

        Private descriptionField As String

        '''<remarks/>
        Public Property ReturnValue() As String
            Get
                Return Me.returnValueField
            End Get
            Set(ByVal value As String)
                Me.returnValueField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class STSErrorSTSBusinessRuleType

        Private codeField As String

        Private descriptionField As String

        Private detailField As String

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Detail() As String
            Get
                Return Me.detailField
            End Get
            Set(ByVal value As String)
                Me.detailField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class STSErrorInvalidDataType

        Private fieldNameField As String

        Private codeField As String

        Private descriptionField As String

        Private suppliedValueField As String

        '''<remarks/>
        Public Property FieldName() As String
            Get
                Return Me.fieldNameField
            End Get
            Set(ByVal value As String)
                Me.fieldNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SuppliedValue() As String
            Get
                Return Me.suppliedValueField
            End Get
            Set(ByVal value As String)
                Me.suppliedValueField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class STSErrorType

        Private invalidDataField() As STSErrorInvalidDataType

        Private sTSBusinessRuleField As STSErrorSTSBusinessRuleType

        Private siriusBackOfficeField As STSErrorSiriusBackOfficeType

        Private internalExceptionField As STSErrorInternalExceptionType

        Private webServiceField As String

        Private webMethodField As String

        Private labelField As String

        '''<remarks/>
        Public Property InvalidData() As STSErrorInvalidDataType()
            Get
                Return Me.invalidDataField
            End Get
            Set(ByVal value As STSErrorInvalidDataType())
                Me.invalidDataField = value
            End Set
        End Property

        '''<remarks/>
        Public Property STSBusinessRule() As STSErrorSTSBusinessRuleType
            Get
                Return Me.sTSBusinessRuleField
            End Get
            Set(ByVal value As STSErrorSTSBusinessRuleType)
                Me.sTSBusinessRuleField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SiriusBackOffice() As STSErrorSiriusBackOfficeType
            Get
                Return Me.siriusBackOfficeField
            End Get
            Set(ByVal value As STSErrorSiriusBackOfficeType)
                Me.siriusBackOfficeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InternalException() As STSErrorInternalExceptionType
            Get
                Return Me.internalExceptionField
            End Get
            Set(ByVal value As STSErrorInternalExceptionType)
                Me.internalExceptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WebService() As String
            Get
                Return Me.webServiceField
            End Get
            Set(ByVal value As String)
                Me.webServiceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property WebMethod() As String
            Get
                Return Me.webMethodField
            End Get
            Set(ByVal value As String)
                Me.webMethodField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Label() As String
            Get
                Return Me.labelField
            End Get
            Set(ByVal value As String)
                Me.labelField = value
            End Set
        End Property
    End Class




End Namespace