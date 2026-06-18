Option Strict On

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS

Imports System
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SSP.Shared.gPMConstants

Public MustInherit Class STSBaseOutput
    Public STSError As STSErrorType
End Class

' Start of EDI structures

<Serializable()>
Public MustInherit Class EDIBaseInputClass
    Public DataModel As String = "FPLHEDI"
    Public XML As Xml.XmlNode
End Class

<Serializable()>
Public MustInherit Class EDIBaseOutputClass
    Inherits STSBaseOutput
End Class

<Serializable()>
Public Class EDINewBusinessInput
    Inherits EDIBaseInputClass
End Class

<Serializable()>
Public Class EDINewBusinessOutput
    Inherits EDIBaseOutputClass

    Public PolicyRef As String = String.Empty
    Public ClientShortCode As String = String.Empty
End Class

<Serializable()>
Public Class EDIAdjustmentInput
    Inherits EDIBaseInputClass
End Class

<Serializable()>
Public Class EDIAdjustmentOutput
    Inherits EDIBaseOutputClass
    Public PolicyRef As String = String.Empty
    Public ClientShortCode As String = String.Empty
End Class

<Serializable()>
Public Class EDIRenewalConfirmationInput
    Inherits EDIBaseInputClass
End Class

<Serializable()>
Public Class EDIRenewalConfirmationOutput
    Inherits EDIBaseOutputClass
End Class

' End of EDI structures

' The Following Classes Define the Input and Output Parameters for the LoginAgent Method 
Public Class LoginAgentIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public UserName As String = ""
    Public Password As String = ""

    ' In/Out Parameters
    Public AgentCnt As Int32
    Public PMUserID As Int32
    Public UnrestrictedSearch As Boolean
    Public PasswordChangeDate As Date
    Public Lastlogin As Date
    Public Forename As String = ""
    Public Surname As String = ""
    Public EmailAddress As String = ""
    Public LanguageId As Int32
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class LoginAgentOut

    'Public Property AgentCnt As Int32
    Public AgentCnt As Int32
    Public PMUserID As Int32
    Public UnrestrictedSearch As Boolean
    Public PasswordChangeDate As Date
    Public Lastlogin As Date
    Public Forename As String = ""
    Public Surname As String = ""
    Public EmailAddress As String = ""
    Public LanguageId As Int32
    Public LoginFailure As Int32
    Public SourceList As DataSet = Nothing
    Public AdditionalDataArray() As AdditionalData
    Public Token As String = String.Empty

End Class

' The Following Classes Define the Input and Output Parameters for the LogoffAgent Method 
Public Class LogoffAgentIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public UserName As String = ""

    ' In/Out Parameters
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class LogoffAgentOut

    Public AdditionalDataArray() As AdditionalData

End Class

' The Following Classes Define the Input and Output Parameters for the UpdateAgentLogonDetails Method 
Public Class UpdateAgentLogonDetailsIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public UserName As String = ""
    Public Password As String = ""
    Public NewPassword As String = ""

    ' In/Out Parameters
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class UpdateAgentLogonDetailsOut

    Public LoginFailure As Int32
    Public AdditionalDataArray() As AdditionalData

End Class

' The Following Classes Define the Input and Output Parameters for the FindParty Method 
<Serializable()>
Public Class FindPartyIn

    Public sGisDataModelCode As String = ""
    Public sGisBusinessTypeCode As String = ""
    Public sPartyType As String = ""
    Public sShortname As String = ""
    Public sResolvedName As String = ""
    Public sUserID As String = ""
    Public sTelephoneNumber As String = ""
    Public sPostcode As String = ""
    Public lLeadAgentCnt As System.Int32
    Public vAdditionalDataArray() As AdditionalData
    Public sAddress1 As String = ""
    Public sPolicyNo As String = ""
    Public FileCode As String = ""

    'Public Sub New()

End Class

<Serializable()>
Public Class FindPartyOut
    Inherits STSBaseOutput

    Public ResultArray As System.Data.DataSet

    Public Sub Populate(ByVal oResultArray As Object)

        Dim vResultArray As System.Array
        Dim iRowFrom As Integer
        Dim iRowTo As Integer
        Dim iColFrom As Integer
        Dim iColTo As Integer
        Dim iCol As Integer
        Dim iRow As Integer

        Dim myTable As DataTable = Nothing
        Dim myRow As DataRow
        Dim myColumn As DataColumn

        ' More to do here, need to trap if there are no results, therefore No Array
        vResultArray = CType(oResultArray, System.Array)

        iColFrom = vResultArray.GetLowerBound(0)
        iColTo = vResultArray.GetUpperBound(0)

        iRowFrom = vResultArray.GetLowerBound(1)
        iRowTo = vResultArray.GetUpperBound(1)

        ' Create a New Dataset
        ResultArray = New DataSet
        ' And a Data Table
        myTable = New DataTable("ResultArray")

        ' For Each Column in the result create a Data Column
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        For iCol = iColFrom To iColTo
            ' Create a New Datacolumn
            myColumn = New DataColumn
            ' Set the type using the value from the first row
            myColumn.DataType = vResultArray.GetValue(iCol, 0).GetType
            ' Set the column name
            myColumn.ColumnName = "Col" + iCol.ToString()
            myColumn.ReadOnly = True
            myColumn.Unique = False
            ' Add the Column to the DataColumnCollection.
            myTable.Columns.Add(myColumn)
        Next

        ' Populate the Table
        For iRow = iRowFrom To iRowTo
            ' Create a New Row
            myRow = myTable.NewRow()
            ' Populate the Columns
            For iCol = iColFrom To iColTo
                myRow("Col" + iCol.ToString()) = vResultArray.GetValue(iCol, iRow)
            Next
            ' Add the Row to the Table
            myTable.Rows.Add(myRow)
        Next

        ' Finally add the Table to the ResultArray
        ResultArray.Tables.Add(myTable)

    End Sub

End Class

' The Following Classes Define the Input and Output Parameters for the AddParty Method 
Public Class AddPartyIn
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public PartyTypeCode As String = ""
    Public Forename As String = ""
    Public Surname As String = ""
    Public DateOfBirth As Date
    Public EmailAddress As String = ""
    Public CurrentRenewalDate As Date
    Public Address1 As String = ""
    Public Address2 As String = ""
    Public Address3 As String = ""
    Public Address4 As String = ""
    Public Postcode As String = ""
    Public CountryCode As String = ""
    Public Title As String = ""
    Public MaritalStatusCode As String = ""
    Public GenderCode As String = ""
    Public Initials As String = ""
    Public TelephoneNumber As String = ""
    Public TradingName As String = ""
    Public SourceID As String = "1"
    Public FileCode As String = ""
    Public AdditionalDataArray() As AdditionalData
End Class

Public Class AddPartyOut
    Inherits STSBaseOutput

    Public PartyCnt As Int32
    Public PartyCode As String = String.Empty
    Public AdditionalDataArray() As AdditionalData
    Public Currencies As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the UpdateCoverDetails Method 
Public Class UpdateCoverDetailsIn

    ' In ONLY Parameters
    Public InsuranceFileCnt As Int32
    Public EffectiveDate As Date
    Public ExpirationDate As Date
    Public CurrencyID As Int32
    Public InsuredName As String = String.Empty
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class UpdateCoverDetailsOut
    Inherits STSBaseOutput

End Class

' The Following Classes Define the Input and Output Parameters for the UpdateParty Method 
Public Class UpdatePartyIn
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public PartyCnt As Int32
    Public Forename As String = ""
    Public Surname As String = ""
    Public DateOfBirth As Date
    Public EmailAddress As String = ""
    Public CurrentRenewalDate As Date
    Public Address1 As String = ""
    Public Address2 As String = ""
    Public Address3 As String = ""
    Public Address4 As String = ""
    Public Postcode As String = ""
    Public CountryCode As String = ""
    Public Title As String = ""
    Public MaritalStatusCode As String = ""
    Public GenderCode As String = ""
    Public Initials As String = ""
    Public TelephoneNumber As String = ""
    Public AdditionalDataArray() As AdditionalData
End Class

Public Class UpdatePartyOut
    Public AdditionalDataArray() As AdditionalData
End Class

' The Following Classes Define the Input and Output Parameters for the GetParty Method 
Public Class GetPartyIn
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public PartyCnt As Int32 = 0
End Class

Public Class GetPartyOut

    Public Forename As String = ""
    Public Surname As String = ""
    Public PartyType As String = ""
    Public Address1 As String = ""
    Public Address2 As String = ""
    Public Address3 As String = ""
    Public Address4 As String = ""
    Public Postcode As String = ""
    Public CountryCode As String = ""
    Public DateOfBirth As Date = CDate("1/1/1")
    Public EmailAddress As String = ""
    Public UserID As String = ""
    Public Password As String = ""
    Public ShortName As String = ""
    Public ResolvedName As String = ""

    Public MothersMaidenName As Object = ""
    Public TPUserCode As Object = ""
    Public TPIntroducer As Object = ""
    Public AQuestion As Object = ""
    Public TheAnswer As Object = ""
    Public MemorableDate As Object = "01/01/0001" 'CDate("1/1/1")
    Public CurrentRenewalDate As Object = "01/01/0001" ' CDate("1/1/1")
    Public Title As Object = ""
    Public MaritalStatusCode As Object = ""
    Public GenderCode As Object = ""
    Public Initials As Object = ""
    Public TelephoneNumber As Object = ""
    Public ContactName As Object = ""
    Public TradingName As Object = ""
    Public PartyGroupTypeID As Object = 0
    Public IsRegisteredCharity As Object = False
    Public NumberOfMembers As Object = ""
    Public CharityNumber As Object = ""
    Public OccupationCode As String = ""
    Public AllContacts As DataSet = Nothing
    Public SourceID As Integer = 0

    'Public MothersMaidenName As String = ""
    'Public TPUserCode As String = ""
    'Public TPIntroducer As String = ""
    'Public AQuestion As String = ""
    'Public TheAnswer As String = ""
    'Public MemorableDate As String = "01/01/0001" 'CDate("1/1/1")
    'Public CurrentRenewalDate As String = "01/01/0001" ' CDate("1/1/1")
    'Public Title As String = ""
    'Public MaritalStatusCode As String = ""
    'Public GenderCode As String = ""
    'Public Initials As String = ""
    'Public TelephoneNumber As String = ""
    'Public ContactName As String = ""
    'Public TradingName As String = ""
    'Public PartyGroupTypeID As Int32 = 0
    'Public IsRegisteredCharity As Boolean = False
    'Public NumberOfMembers As String = ""
    'Public CharityNumber As String = ""

End Class

' The Following Classes Define the Input and Output Parameters for the AddQuote Method 
Public Class AddQuoteIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public EffectiveDate As Date
    Public ExpirationDate As Date
    Public InsuredName As String = ""
    Public PartyCnt As Int32 = 0
    Public AgentCnt As Int32 = 0
    Public InsuranceFolderDescription As String = ""
    Public AlternateReference As String = ""
    Public BrokerABIID As String = ""
    Public RiskGroup As String = ""
    Public RiskCode As String = ""
    Public SourceId As Int32
    Public InsurerCnt As Int32
    Public ScreenId As Int32
    Public CurrencyId As Int32
    Public AnalysisId As Integer
    Public PolicyStatusCode As String = String.Empty
    Public PolicyVersion As Integer
    Public LapsedReasonCode As String = ""
    Public LapsedReasonID As Integer = 0
    Public LapsedDate As Date = InternalSAMConstants.GISLowDate
    Public LapsedReasonDescription As String = ""
    Public InceptionDate As Date = InternalSAMConstants.GISLowDate
    Public InceptionDateTPI As Date = InternalSAMConstants.GISLowDate
    Public RenewalDate As Date = InternalSAMConstants.GISLowDate
    Public OldPolicyNumber As String = ""
    Public AccountExecutiveShortname As String = ""
    Public AccountHandlerShortname As String = ""
    Public PolicyVersionTypeCode As String = ""
    Public sCoInsurancePlacement As String = String.Empty
    ' In/Outs
    Public InsuranceFolderCnt As Int32
    Public InsuranceFileCnt As Int32
    Public InsuranceFileRef As String = ""
    Public AdditionalDataArray() As AdditionalData
    Public RiskCnt As Int32 = 0
    Public RiskGroupId As Int32
    Public RiskCodeId As Int32

End Class

Public Class AddQuoteOut
    Inherits STSBaseOutput

    Public InsuranceFolderCnt As Int32 = 0
    Public InsuranceFileCnt As Int32 = 0
    Public InsuranceFileRef As String = ""

    Public InsurerCnt As Int32 = 0
    Public AgentCnt As Int32 = 0
    Public AdditionalDataArray() As AdditionalData
    Public RiskCnt As Int32 = 0
    Public RiskGroupId As Int32
    Public RiskCodeId As Int32
    Public GISSchemeID As Int32 = 0

End Class

' The Following Classes Define the Input and Output Parameters for the AddQuote Method 
Public Class AddMTAQuoteIn

    ' In ONLY Parameters
    Public MTAType As String = ""
    Public NewMessageVersion As Int32 = 0
    Public GISDataModelCode As String = ""
    Public EffectiveDate As Date
    Public ThisPremium As Decimal = 0
    Public NetPremium As Decimal = 0
    Public TaxAmount As Decimal = 0
    Public TaxRate As Decimal = 0
    Public InsuranceRef As String = ""
    Public AlternateReference As String = ""
    ' In/Outs
    Public InsuranceFileCnt As Int32 = 0
    Public InsuranceFolderCnt As Int32 = 0
    Public RiskFolderCnt As Int32 = 0
    Public CoverEndDate As Date
    Public AdditionalDataArray() As AdditionalData
    Public sCoInsurancePlacement As String
End Class

Public Class AddMTAQuoteOut
    Inherits STSBaseOutput

    Public InsuranceFileCnt As Int32 = 0
    Public InsuranceFolderCnt As Int32 = 0
    Public RiskFolderCnt As Int32 = 0
    Public AdditionalDataArray() As AdditionalData

End Class

' The Following Classes Define the Input and Output Parameters for the NBQuote Method 
Public Class NBQuoteIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public EffectiveDate As Date
    Public GISSchemeID As Int32
    Public RiskGroupID As Int32
    Public RiskScreenId As Int32
    Public Username As String = String.Empty
    Public isClaimValidation As Boolean
    ' In/Outs
    Public XMLDataset As String = ""
    Public AdditionalDataArray() As AdditionalData
    Public ClaimTransactiontypeId As Integer = 0

End Class

Public Class NBQuoteOut

    Public XMLDataset As String = ""
    Public AdditionalDataArray() As AdditionalData

End Class

' The Following Classes Define the Input and Output Parameters for the NBQuote Method 
Public Class NewDatasetIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public InsuranceFileCnt As Int32
    Public RiskID As Int32
    Public InsuranceFolderCnt As Int32

End Class

Public Class NewDatasetOut

    Public PolicyLinkID As Int32
    Public TopOIKey As String = ""
    Public XMLDataSetDef As String = ""
    Public XMLDataset As String = ""
    Public QuoteRef As String = ""
    Public QuoteRefPassword As String = ""

End Class

' The Following Classes Define the Input and Output Parameters for the AddRisk Method 
Public Class AddRiskIn

    ' In ONLY Parameters
    Public BackOfficeMapperCode As String = ""
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public InsuranceFolderCnt As Int32
    Public InsuranceFileCnt As Int32
    Public PartyCnt As Int32
    Public RiskTypeId As Int32
    Public RiskScreenId As Int32
    Public RiskDescription As String = ""
    Public ProductID As Long

    ' In/Outs
    Public RiskFolderCnt As Int32
    Public RiskCnt As Int32
    Public XMLDataSetDef As String = ""
    Public XMLDataset As String = ""
    Public PolicyLinkID As Int32
    Public TopOIKey As String = ""
    Public QuoteRef As String = ""
    Public QuoteRefPassword As String = ""
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class AddRiskOut
    Inherits STSBaseOutput

    Public RiskFolderCnt As Int32
    Public RiskCnt As Int32
    Public XMLDataSetDef As String = ""
    Public XMLDataset As String = ""
    Public PolicyLinkID As Int32
    Public TopOIKey As String = ""
    Public QuoteRef As String = ""
    Public QuoteRefPassword As String = ""
    Public AdditionalDataArray() As AdditionalData

End Class

' The Following Classes Define the Input and Output Parameters for the DeleteRisk Method 
Public Class DeleteRiskIn

    ' In ONLY Parameters
    Public RiskCnt As Int32

    Public InsuranceFileCnt As Int32

    Public InsuranceFolderCnt As Int32

    Public TransactionType As String
    Public nOrignalRiskKey As Integer
End Class

Public Class DeleteRiskOut
    Inherits STSBaseOutput

End Class

' The Following Classes Define the Input and Output Parameters for the GetDocTemplate Method 
Public Class GetDocTemplateIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public SchemeId As Int32
    Public AgentCnt As Int32

End Class

Public Class GetDocTemplateOut

    Public DocTemplates As DataSet = Nothing
    Public ReturnValue As Int32

End Class

' The Following Classes Define the Input and Output Parameters for the GetQuotes Method 
Public Class GetQuotesIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public PartyCnt As Int32

End Class

Public Class GetQuotesOut

    Public Quotes As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetQuoteDetails Method 
Public Class GetQuoteDetailsIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public InsuranceFileCnt As Int32
    Public Underwriting As Boolean = True

End Class

Public Class GetQuoteDetailsOut

    Public Quotes As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetQuoteRisks Method 
Public Class GetQuoteRisksIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public InsuranceFileCnt As Int32

End Class

Public Class GetQuoteRisksOut

    Public Quotes As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetQuoteDetails Method 
Public Class GetProductByAgentIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public AgentPartyCnt As Int32
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class GetProductByAgentOut

    Public Products As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetRatingDetails Method 
Public Class GetRatingDetailsIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public InsuranceFolderCnt As Int32
    Public InsuranceFileCnt As Int32
    Public RiskCnt As Int32

End Class

Public Class GetRatingDetailsOut

    Public RatingSections As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetRatingDetails Method 
Public Class GetRiskByProductIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public ProductID As Int32
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class GetRiskByProductOut

    Public Risks As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetSchemeList Method 
Public Class GetRiskCodeListIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public RiskGroupCode As String = ""
    Public EffectiveDate As Date

End Class

Public Class GetRiskCodeListOut

    Public RiskCodes As DataSet = Nothing

End Class

' The Following Classes Define the Input and Output Parameters for the GetSchemeList Method 
Public Class GetSchemeListIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""

End Class

Public Class GetSchemeListOut

    Public Schemes As DataSet = Nothing

End Class

Public Class LoginUserReturn

    Private m_iPartyCnt As System.Int32
    Private m_iPMUserID As System.Int32
    Private m_sSurname As System.String = ""
    Private m_sForename As System.String = ""
    Private m_dtDateOfBirth As System.DateTime
    Private m_sEmailAddress As System.String = ""
    Private m_vAdditionalDataArray() As AdditionalData
    Private m_sToken As String = ""

    Public Property Token() As String
        Get
            Return m_sToken
        End Get
        Set(ByVal Value As String)
            m_sToken = Value
        End Set
    End Property

    Public Property PartyCnt() As System.Int32
        Get
            Return m_iPartyCnt
        End Get
        Set(ByVal Value As System.Int32)
            m_iPartyCnt = Value
        End Set
    End Property

    Public Property PMUserID() As System.Int32
        Get
            Return m_iPMUserID
        End Get
        Set(ByVal Value As System.Int32)
            m_iPMUserID = Value
        End Set
    End Property

    Public Property Surname() As System.String
        Get
            Return m_sSurname
        End Get
        Set(ByVal Value As System.String)
            m_sSurname = Value
        End Set
    End Property

    Public Property Forename() As System.String
        Get
            Return m_sForename
        End Get
        Set(ByVal Value As System.String)
            m_sForename = Value
        End Set
    End Property

    Public Property DateOfBirth() As System.DateTime
        Get
            Return m_dtDateOfBirth
        End Get
        Set(ByVal Value As System.DateTime)
            m_dtDateOfBirth = Value
        End Set
    End Property

    Public Property EmailAddress() As System.String
        Get
            Return m_sEmailAddress
        End Get
        Set(ByVal Value As System.String)
            m_sEmailAddress = Value
        End Set
    End Property

    Public Property AdditionalDataArray() As AdditionalData()
        Get
            Return m_vAdditionalDataArray
        End Get
        Set(ByVal Value As AdditionalData())
            m_vAdditionalDataArray = Value
        End Set
    End Property

End Class

' The Following Classes Define the Input and Output Parameters for the GeneratePolicyDocument Method 
' PN20978
<XmlType(Namespace:="http://siriusgroup.co.uk/SiriusTransactionService/")>
Public Class GenerateDocumentIn

    ' In ONLY Parameters  PN20978
    Public DocumentTemplateCode As String = String.Empty
    Public Mode As Int32
    Public PartyCnt As Int32
    Public InsuranceFileCnt As Int32
    Public InsuranceFolderCnt As Int32
    Public ParameterXML As String = String.Empty
    Public OutputAsHTML As Boolean
    Public CalledFromSwift As Boolean
End Class

' PN20978
<XmlType(Namespace:="http://siriusgroup.co.uk/SiriusTransactionService/")>
Public Class GenerateDocumentOut

    Public SpooledZipFile As Byte()
    Public MergedFilePath As String = ""

End Class

' The Following Classes Define the Input and Output Parameters for the GeneratePolicyDocument Method 
Public Class GeneratePolicyDocumentIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public PartyCnt As Int32
    Public InsuranceFolderCnt As Int32
    Public InsuranceFileCnt As Int32
    Public ProcessTypesDocID As Int32
    Public DocumentTemplateId As Int32
    Public DocumentTypeId As Int32

End Class

Public Class GeneratePolicyDocumentOut

    Public SpooledZipFile As Byte()
    Public MergedFilePath As String = ""

End Class

' The Following Classes Define the Input and Output Parameters for the Load Risk From DB Method 
'LoadRiskFromDB

Public Class LoadRiskFromDBIn

    Public XMLDataSetDef As String = ""
    Public XMLDataSet As String = ""
    Public DataModelCode As String = ""
    Public RiskID As Int32 = -1
    Public InsuranceFileCnt As Int32 = -1
    Public InsuranceFolderCnt As Int32 = -1

End Class

Public Class LoadRiskFromDBOut

    Public XMLDataSetDef As String = ""
    Public XMLDataSet As String = ""
    Public DataModelCode As String = ""
    Public RiskID As Int32 = -1
    Public InsuranceFileCnt As Int32 = -1
    Public InsuranceFolderCnt As Int32 = -1

End Class

' The Following Classes Define the Input and Output Parameters for the Load From DB Method 
Public Class LoadFromDBIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""

    ' In/Outs
    Public InsuranceFileCnt As Int32 = -1
    Public RiskID As Int32 = -1
    Public PolicyLinkID As Int32 = -1
    Public QuoteRef As String = ""
    Public QuoteRefPassword As String = ""
    Public InsuranceFolderCnt As Int32 = -1

End Class

Public Class LoadFromDBOut

    ' Out only Params
    Public XMLDataSetDef As String = ""
    Public XMLDataset As String = ""
    Public GuaranteedQuoteDate As Date

    ' In/Outs
    Public InsuranceFileCnt As Int32
    Public RiskID As Int32
    Public PolicyLinkID As Int32
    Public QuoteRef As String = ""
    Public QuoteRefPassword As String = ""
    Public InsuranceFolderCnt As Int32

End Class

' The Following Classes Define the Input and Output Parameters for the Save To DB Method 
Public Class SaveToDBIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public XMLDataset As String = ""

End Class

Public Class SaveToDBOut

    ' In/Outs
    Public XMLDataset As String = ""

End Class

' The Following Classes Define the Input and Output Parameters for the Data Cash Method 

Public Class DataCashIn
    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public RequestType As String = ""
    Public DataCashRef As String = ""
    Public CardNumber As String = ""
    Public ExpirationMonth As Int32 = -1
    Public ExpirationYear As Int32 = -1
    Public Amount As String = ""
    Public AuthCode As String = ""
    Public SwitchExtraInfo As String = String.Empty
    Public PolicyLinkID As Int32 = -1
    Public TransactionType As String = ""
End Class

Public Class DataCashOut
    ' In/Outs
    Public RawResponseArray As DataSet = Nothing

    Public Result As Int16 = 0
    Public AuthCode As String = ""
    Public UniqueRef As String = ""
    Public TimeStamp As String = ""
    Public CardType As String = ""
    Public Issuer As String = ""
    Public Country As String = ""
    Public Reason As String = ""

End Class

' The Following Classes Define the Input and Output Parameters for the Statistics Processing Method 
Public Class ProcessAccountsIn
    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public InsuranceFileCnt As Int32 = -1
    Public TransactionType As String = ""
    Public MTAInstallments As Boolean = False
    Public CancelRefundAmt As String = "0"
    Public RenewalInstallments As Boolean = False
    Public AccountID As Integer = 0
    Public DebitAgainst As Integer = 0
End Class

Public Class ProcessAccountsOut
    Inherits STSBaseOutput
    ' In/Outs
    Public TransactionArray As Object = Nothing
End Class

Public Class NBTransactIn

    ' In ONLY Parameters
    Public sGisDataModelCode As String = ""
    Public sGisBusinessTypeCode As String = ""
    Public lGISSchemeID As Int32 = -1
    Public AdditionalDataArray() As AdditionalData
    Public sXMLDataSet As String = ""

    Public InsuranceFileCnt As Int32 = 0
    Public DebitCredit As String = ""
    Public LastTransType As String = ""
    Public LastTransDate As DateTime
    Public PolicyStartDate As DateTime
    Public PostingAmount As Double = 0
    Public Reason As String = ""
    Public RealInsuranceFileCnt As Int32 = 0

    Public Sub New()

    End Sub
End Class

Public Class NBTransactOut
    Inherits STSBaseOutput

    ' In/Outs
    Public sXMLDataset As String = ""
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class MTATransactIn

    ' In ONLY Parameters
    Public sGisDataModelCode As String = ""
    Public sGisBusinessTypeCode As String = ""
    Public AdditionalDataArray() As AdditionalData
    Public sXMLDataSet As String = ""
    Public PolicyStartDate As DateTime

End Class

Public Class MTATransactOut
    Inherits STSBaseOutput

    ' In/Outs
    Public sXMLDataset As String = ""
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class GetSFBQuoteLinkInput

    ' Input
    Public DataModelCode As String = ""
    Public InsuranceFileCnt As Int32 = 0

End Class

Public Class GetSFBQuoteLinkOutput

    ' Output
    Public XMLDataSet As String = ""
    Public XMLDataSetDef As String = ""

End Class

<Serializable()>
Public Class AdditionalData
    Public Name As String = ""
    Public Value As Object = Nothing

    Public Sub New()

    End Sub

    Public Sub New(ByVal v_sName As String, ByVal v_oValue As Object)
        Name = v_sName
        Value = v_oValue
    End Sub
End Class

Public Class PostcodeSearchInput

    Public HouseNameNum As String = ""
    Public Postcode As String = ""

End Class

Public Class PostcodeSearchOutput

    Public MatchArray As DataSet = Nothing

End Class

Public Class GetAddressInput

    Public AddressCnt As Int32 = 0

End Class

Public Class GetAddressOutput

    Public AddressCnt As Int32 = 0
    Public Address1 As String = ""
    Public Address2 As String = ""
    Public Address3 As String = ""
    Public Address4 As String = ""
    Public PostalCode As String = ""
    Public CountryID As Int32 = 0

End Class

Public Class GetCurrenciesByBranchInput

    Public SourceID As Integer

End Class

Public Class GetCurrenciesByBranchOutput

    Public Currencies As DataSet = Nothing

End Class

Public Class GetSourceListForUserInput

    Public UserID As Integer

End Class

Public Class GetSourceListForUserOutput

    Public Sources As DataSet = Nothing

End Class

Public Class AddAddressInput

    Public Address1 As String = ""
    Public Address2 As String = ""
    Public Address3 As String = ""
    Public Address4 As String = ""
    Public PostalCode As String = ""
    Public CountryID As Int32 = 0

End Class

Public Class AddAddressOutput

    Public AddressCnt As Int32 = 0

End Class

Public Class DefaultRulesInput

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public EffectiveDate As Date
    Public GISSchemeID As Int32
    Public RiskGroupID As Int32
    Public RiskScreenId As Int32

    ' In/Outs
    Public XMLDataset As String = ""
    Public AdditionalDataArray() As AdditionalData

End Class

Public Class DefaultRulesOutput
    Inherits NBQuoteOut
End Class

Public Class ValidationRulesInput
    Inherits NBQuoteIn
End Class

Public Class ValidationRulesOutput
    Inherits NBQuoteOut
End Class

Public Class ForgottenPasswordIn
    Public DataModelCode As String = ""
    Public BusinessTypeCode As String = ""
    Public Username As String = ""
    Public IPAddress As String = ""
End Class

Public Class ForgottenPasswordOut
    Public EmailAddress As String = ""
    Public NewPassword As String = ""
    Public ReturnValue As Int32 = 0
End Class

Public Class UpdateSumInsuredIn

    Public PolicyLinkID As Int32
    Public DataModelCode As String = String.Empty
    Public SumInsuredType As Int32
    Public SumInsured As DataSet = Nothing

End Class

Public Class UpdateSumInsuredOut
    Inherits UpdateSumInsuredIn

End Class

Public Class GetSumInsuredIn

    Public PolicyLinkID As Int32
    Public DataModelCode As String = String.Empty
    Public SumInsuredType As Int32

End Class

Public Class GetSumInsuredOut
    Inherits GetSumInsuredIn

    Public SumInsured As DataSet = Nothing

End Class

<Serializable()>
Public Class FindPoliciesIn

    Public DateOfLoss As DateTime
    Public PolicyNumber As String = String.Empty
    Public ClientSurname As String = String.Empty
    Public ClientPostCode As String = String.Empty
    Public AgentName As String = String.Empty
    Public BranchCode As String = String.Empty

End Class

<Serializable()>
Public Class FindPoliciesOut
    Public Policies As DataSet = Nothing
End Class

<Serializable()>
Public Class GetMTADetailsIn

    Public PolicyNumber As String = String.Empty

End Class

<Serializable()>
Public Class GetMTADetailsOut

End Class

' The Following Classes Define the Input and Output Parameters for the ProcessClaim Method 
Public Class ProcessClaimIn

    Public bIsNewClaim As Boolean = False
    Public lInsuranceFileCnt As Int32 = 0
    Public dtLossDate As DateTime  '= Now
    Public sClaimHandler As String = ""
    Public sProgressStatus As String = ""
    Public sDescription As String = ""
    Public sPrimaryCause As String = ""
    Public dtDateReported As DateTime '= Now
    Public lClaimStatusId As Int32 = 0
    Public sClaimNumber As String = ""
    Public sPerilType As String = ""
    Public sReserveType As String = ""
    Public dReserveAmount As Decimal = 0
    Public dtPaymentDate As DateTime = Now
    Public dPaymentAmount As Decimal = 0
    Public sSalvageRecoveryType As String = ""
    Public dSalvageAmount As Decimal = 0
    Public sTPRecoveryType As String = ""
    Public dTPAmount As Decimal = 0
    Public sInsurerClaimNo As String = ""

End Class

Public Class ProcessClaimOut
    Inherits STSBaseOutput

End Class

Public Class NewRiskDatasetIn

    ' In ONLY Parameters
    Public DataModelCode As String = ""
    Public RiskID As Int32
    Public InsuranceFileCnt As Int32
    Public InsuranceFolderCnt As Int32

End Class

Public Class NewRiskDatasetOut

    Public PolicyLinkID As Int32
    Public TopOIKey As String = ""
    Public XMLDataSetDef As String = ""
    Public XMLDataset As String = ""
    Public QuoteRef As String = ""
    Public QuoteRefPassword As String = ""

End Class

<Serializable()> Public Class Source
    Public Id As Integer
    Public Code As String = String.Empty
    Public Description As String = String.Empty
End Class

Public Class Utilities

    Public Shared Sub AddCallFromSTS(ByRef oAdditionalArray() As AdditionalData)

        Dim oData As AdditionalData
        Const CNCalledFromSTS As String = "CalledFromSTS"
        Const CNSourceOfBusiness As String = "SOB"
        Const CNAgentsOnline As String = "AOL"

        If (oAdditionalArray Is Nothing) = False Then

            For Each oData In oAdditionalArray
                If oData.Name = CNCalledFromSTS Then
                    ' Make sure its True, because it should be
                    oData.Value = True
                    Exit Sub
                End If
            Next

            ' Still here, so add it
            oData = New AdditionalData

            ' Add the values
            oData.Name = CNCalledFromSTS
            oData.Value = True

            If (oAdditionalArray.Length > 0) Then
                ReDim Preserve oAdditionalArray(Microsoft.VisualBasic.UBound(oAdditionalArray) + 1)
                oAdditionalArray(Microsoft.VisualBasic.UBound(oAdditionalArray)) = oData
            Else
                ReDim oAdditionalArray(0)
                oAdditionalArray(0) = oData
            End If

            For Each oData In oAdditionalArray
                If oData.Name = CNSourceOfBusiness Then
                    ' Make sure its True, because it should be
                    'oData.Value = True
                    oData.Value = "AOL"
                    Exit Sub
                End If
            Next

            ' add it
            oData = New AdditionalData

            ' Add the values
            oData.Name = CNSourceOfBusiness
            oData.Value = CNAgentsOnline

            If (oAdditionalArray.Length > 0) Then
                ReDim Preserve oAdditionalArray(Microsoft.VisualBasic.UBound(oAdditionalArray) + 1)
                oAdditionalArray(Microsoft.VisualBasic.UBound(oAdditionalArray)) = oData
            Else
                ReDim oAdditionalArray(0)
                oAdditionalArray(0) = oData
            End If

        Else

            ReDim oAdditionalArray(1)

            ' Still here, so add it
            oData = New AdditionalData

            ' Add the values
            oData.Name = CNCalledFromSTS
            oData.Value = True

            oAdditionalArray(0) = oData

            ' Add the values
            oData.Name = CNSourceOfBusiness
            oData.Value = CNAgentsOnline

            oAdditionalArray(1) = oData

        End If

    End Sub
    ' Note: There is an overloaded version of this function below, which allows column
    ' titles and types to be passed in, and optionally lookup table names.
    Public Shared Function ArrayToDataSet(ByVal oResultArray As Object, Optional ByVal sResultDataSetName As String = "ResultDataSet") As System.Data.DataSet

        Dim ds As System.Data.DataSet
        Dim vResultArray As System.Array
        Dim iRowFrom As Integer
        Dim iRowTo As Integer
        Dim iColFrom As Integer
        Dim iColTo As Integer
        Dim iCol As Integer
        Dim iRow As Integer

        Dim myTable As DataTable = Nothing
        Dim myRow As DataRow
        Dim myColumn As DataColumn

        ' More to do here, need to trap if there are no results, therefore No Array
        Try
            vResultArray = CType(oResultArray, System.Array)
        Catch
            vResultArray = Nothing
        End Try

        ' If we do not have an Array then return Nothing
        If vResultArray Is Nothing Then
            Return Nothing
        End If

        iColFrom = vResultArray.GetLowerBound(0)
        iColTo = vResultArray.GetUpperBound(0)

        Try
            iRowFrom = vResultArray.GetLowerBound(1)
            iRowTo = vResultArray.GetUpperBound(1)
        Catch ex As IndexOutOfRangeException
            ' If we're here then we have a 1D array
            Dim vNewArray As Object(,)
            Dim iLoop1 As Int32

            ReDim vNewArray(0, iColTo)

            For iLoop1 = iColFrom To iColTo
                vNewArray(0, iLoop1) = vResultArray.GetValue(iLoop1)
            Next

            ' Process the new array
            Return ArrayToDataSet(vNewArray)

        End Try

        ' Create a New Dataset
        ds = New DataSet(sResultDataSetName)

        ' And a Data Table
        myTable = New DataTable("Row")

        ' For Each Column in the result create a Data Column
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        For iCol = iColFrom To iColTo
            ' Create a New Datacolumn
            myColumn = New DataColumn
            ' Set the type using the value from the first row
            If (vResultArray.GetValue(iCol, iRowFrom) Is Nothing) = True Then
                myColumn.DataType = System.Type.GetType("System.String")
            ElseIf (vResultArray.GetValue(iCol, iRowFrom).GetType.Name <> "DBNull") Then
                myColumn.DataType = vResultArray.GetValue(iCol, iRowFrom).GetType
            Else
                ' If it's a DBNull then see if there's a non-null entry further
                ' down the array
                Dim bFound As Boolean
                For iRow = iRowFrom To iRowTo
                    If (vResultArray.GetValue(iCol, iRow) Is Nothing) = False AndAlso
                        vResultArray.GetValue(iCol, iRow).GetType.Name <> "DBNull" Then
                        bFound = True
                        myColumn.DataType = vResultArray.GetValue(iCol, iRow).GetType
                        Exit For
                    End If
                Next
                If Not bFound Then
                    myColumn.DataType = System.Type.GetType("System.String")
                End If
            End If

            ' Set the column name
            myColumn.ColumnName = "Col" + iCol.ToString()
            myColumn.ReadOnly = True
            myColumn.Unique = False
            ' Add the Column to the DataColumnCollection.
            myTable.Columns.Add(myColumn)
        Next

        ' Populate the Table
        For iRow = iRowFrom To iRowTo
            ' Create a New Row            
            myRow = myTable.NewRow()
            ' Populate the Columns
            For iCol = iColFrom To iColTo
                myRow("Col" + iCol.ToString()) = vResultArray.GetValue(iCol, iRow)
            Next
            ' Add the Row to the Table
            myTable.Rows.Add(myRow)
        Next

        ' Finally add the Table to the ResultArray
        ds.Tables.Add(myTable)

        ' Return the Dataset
        Return ds

    End Function

    ' Note the arrColumnDetails array is a 2-dimensional array with the following columns:
    ' 0 - Dataset column Name
    ' 1 - Dataset column type
    ' 2 - IsLookup (optional)
    ' 3 - Lookup Table Name (optional)
    Public Shared Function ArrayToDataSet(ByVal oResultArray As Object, ByVal arrColumnDetails As System.Array, Optional ByVal sResultDataSetName As String = "ResultDataSet") As System.Data.DataSet

        Dim ds As System.Data.DataSet
        Dim vResultArray As System.Array
        Dim iRowFrom As Integer
        Dim iRowTo As Integer
        Dim iColFrom As Integer
        Dim iColTo As Integer
        Dim iCol As Integer
        Dim iRow As Integer

        Dim myTable As DataTable = Nothing
        Dim myRow As DataRow
        Dim myColumn As DataColumn
        Dim sColumnName As String = String.Empty

        Dim sCodeValue As String = String.Empty
        Dim oBusiness As New CoreBusiness

        Const CNColDetailsColumnName As Integer = 0
        Const CNColDetailsColumnType As Integer = 1
        Const CNColDetailsIsLookup As Integer = 2
        Const CNColDetailsLookupTableName As Integer = 3

        ' More to do here, need to trap if there are no results, therefore No Array
        Try
            vResultArray = CType(oResultArray, System.Array)
        Catch
            vResultArray = Nothing
        End Try

        ' If we do not have an Array then return Nothing
        If vResultArray Is Nothing Then
            Return Nothing
        End If

        ' Must supply column types with column names
        If arrColumnDetails.Rank = 1 Then
            Return Nothing
        End If

        iColFrom = vResultArray.GetLowerBound(0)
        iColTo = vResultArray.GetUpperBound(0)

        ' If the column details have not been supplied or they do not match 
        ' the data array then process the dataset using the basic conversion.
        If iColTo <> arrColumnDetails.GetUpperBound(1) Then
            Return ArrayToDataSet(oResultArray)
        End If

        Try
            iRowFrom = vResultArray.GetLowerBound(1)
            iRowTo = vResultArray.GetUpperBound(1)
        Catch ex As IndexOutOfRangeException
            ' If we're here then we have a 1D array
            Dim vNewArray As Object(,)
            Dim iLoop1 As Int32

            ReDim vNewArray(0, iColTo)

            For iLoop1 = iColFrom To iColTo
                vNewArray(0, iLoop1) = vResultArray.GetValue(iLoop1)
            Next

            ' Process the new array
            Return ArrayToDataSet(vNewArray)

        End Try

        ' Create a New Dataset
        ds = New DataSet(sResultDataSetName)

        ' And a Data Table
        myTable = New DataTable("Row")

        ' For Each Column in the result create a Data Column
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        For iCol = iColFrom To iColTo
            ' Create a New Datacolumn
            myColumn = New DataColumn
            ' Set the type using the passed type
            myColumn.DataType = CType(arrColumnDetails.GetValue(CNColDetailsColumnType, iCol), System.Type)
            ' Set the column name
            myColumn.ColumnName = Cast.ToString(arrColumnDetails.GetValue(CNColDetailsColumnName, iCol), String.Empty)
            myColumn.ReadOnly = True
            myColumn.Unique = False
            ' Add the Column to the DataColumnCollection.
            myTable.Columns.Add(myColumn)
        Next

        ' Populate the Table
        For iRow = iRowFrom To iRowTo
            ' Create a New Row            
            myRow = myTable.NewRow()
            ' Populate the Columns
            For iCol = iColFrom To iColTo

                sColumnName = Cast.ToString(arrColumnDetails.GetValue(CNColDetailsColumnName, iCol), String.Empty)
                ' If lookup details passed in arrColumnDetails array, do lookup
                If arrColumnDetails.GetUpperBound(0) = 3 AndAlso
                   BooleanDataConvert.ToBoolean(Cast.ToInt32(arrColumnDetails.GetValue(CNColDetailsIsLookup, iCol), 0)) = True Then
                    sCodeValue = oBusiness.GetListItemFromID(Core.STSListType.PMLookup, Cast.ToString(arrColumnDetails.GetValue(CNColDetailsLookupTableName, iCol), String.Empty), Cast.ToInt32(vResultArray.GetValue(iCol, iRow), 0))
                    myRow(sColumnName) = sCodeValue
                Else

                    If vResultArray.GetValue(iCol, iRow) IsNot Nothing Then

                        If (vResultArray.GetValue(iCol, iRow).ToString = String.Empty) Then

                            If (myTable.Columns(sColumnName).DataType Is GetType(System.String)) Then
                                myRow(sColumnName) = String.Empty
                            Else
                                myRow(sColumnName) = System.DBNull.Value
                            End If

                        Else

                            If (myTable.Columns(sColumnName).DataType Is GetType(System.String)) Then
                                myRow(sColumnName) = vResultArray.GetValue(iCol, iRow)
                            Else
                                myRow(sColumnName) = Cast.DefaultIfNull(vResultArray.GetValue(iCol, iRow), System.DBNull.Value)
                            End If

                        End If
                    Else
                        myRow(sColumnName) = System.DBNull.Value
                    End If

                End If

            Next
            ' Add the Row to the Table
            myTable.Rows.Add(myRow)
        Next

        ' Finally add the Table to the ResultArray
        ds.Tables.Add(myTable)

        ' Return the Dataset
        Return ds

    End Function

    Public Shared Function DataSetToArray(ByVal oDataSet As System.Data.DataSet) As System.Array

        Dim iRowFrom As Integer
        Dim iRowTo As Integer
        Dim iColFrom As Integer
        Dim iColTo As Integer
        Dim iCol As Integer
        Dim iRow As Integer

        Dim myTable As DataTable = Nothing
        'Dim myRow As DataRow
        'Dim myColumn As DataColumn

        If oDataSet Is Nothing Then
            Return Nothing
        End If

        ' Get a Reference to the Table (There should only be one)
        myTable = oDataSet.Tables(0)

        ' If we do not have a table then return nothing
        If myTable Is Nothing Then
            Return Nothing
        End If

        iRowFrom = 0
        iRowTo = myTable.Rows.Count - 1
        iColFrom = 0
        iColTo = myTable.Columns.Count - 1

        Dim vResultArray(iColTo, iRowTo) As Object

        ' For Each Row
        For iRow = iRowFrom To iRowTo
            ' For Each Column 
            For iCol = iColFrom To iColTo
                vResultArray(iCol, iRow) = myTable.Rows(iRow).Item(iCol)
            Next iCol
        Next iRow

        ' Return the Array
        Return vResultArray

    End Function

    Public Shared Function ClassesToArray(ByVal AdditionalDataArray() As AdditionalData) As Object
        Dim iRow As Int32
        Dim oAddData As AdditionalData

        If AdditionalDataArray Is Nothing Then
            Return Nothing
        End If

        ' Dimension the Array
        'RDT 13032003 - Add an additional element into the array for the CalledFromSTS Flag
        Dim vAdditionalData(1, AdditionalDataArray.GetUpperBound(0) + 1) As Object

        'RDT 13032003 - Set a flag in the Additional Data Array that tells any subsequent object 
        '               that the call has originated from the STS Layer
        iRow = 0
        vAdditionalData(1, iRow) = CType(True, Object)
        vAdditionalData(0, iRow) = CType("CalledFromSTS", Object)

        ' Move the Data from the AdditionalData class to the Array
        iRow = 1
        For Each oAddData In AdditionalDataArray
            vAdditionalData(1, iRow) = CType(oAddData.Value, Object)
            vAdditionalData(0, iRow) = CType(oAddData.Name, Object)
            iRow = iRow + 1
        Next

        Return vAdditionalData

    End Function

    Public Shared Function ArrayToClasses(ByVal oAdditionalData As Object) As AdditionalData()

        Dim iRow As Int32
        Dim vAdditionalData As System.Array
        Dim iRowFrom As Int32
        Dim iRowTo As Int32
        Dim iColFrom As Int32
        Dim iColTo As Int32
        Dim iCol As Int32

        ' More to do here, need to trap if there are no results, therefore No Array
        Try
            vAdditionalData = CType(oAdditionalData, System.Array)
        Catch
            vAdditionalData = Nothing
        End Try

        ' If we do not have an Array then return Nothing
        If vAdditionalData Is Nothing Then
            Return Nothing
        End If

        iColFrom = vAdditionalData.GetLowerBound(0)
        iColTo = vAdditionalData.GetUpperBound(0)

        iRowFrom = vAdditionalData.GetLowerBound(1)
        iRowTo = vAdditionalData.GetUpperBound(1)

        ' Create the Array of Additional Data Classes
        Dim oAddDatas(iRowTo) As AdditionalData

        For iRow = iRowFrom To iRowTo
            For iCol = iColFrom To iColTo
                oAddDatas(iRow) = New AdditionalData
                oAddDatas(iRow).Name = CType(vAdditionalData.GetValue(0, iRow), String)
                oAddDatas(iRow).Value = vAdditionalData.GetValue(1, iRow)
            Next iCol
        Next iRow

        Return oAddDatas

    End Function

    Public Shared Function ClassesToSourceArray(ByVal SourceArray() As Source) As Object
        Dim iRow As Int32
        Dim oSource As Source

        If SourceArray Is Nothing Then
            Return Nothing
        End If

        ' Dimension the Array
        Dim vSource(1, SourceArray.GetUpperBound(0)) As Object

        ' Move the Data from the AdditionalData class to the Array
        iRow = 0
        For Each oSource In SourceArray
            vSource(1, iRow) = CType(oSource.Id, Object)
            vSource(2, iRow) = CType(oSource.Code, Object)
            vSource(3, iRow) = CType(oSource.Description, Object)
            iRow = iRow + 1
        Next

        Return vSource

    End Function

    Public Shared Sub EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer,
                                                     ByVal v_lTransactionType As Integer,
                                                     ByVal v_lGISScreenId As Integer,
                                                     ByVal v_lQuoteType As Integer)

        'new format 1TTTSSSSYY
        r_lEncoded = 1000000000 + (v_lTransactionType * 1000000) + (v_lGISScreenId * 100) + v_lQuoteType

    End Sub

    Public Shared Function SourceArrayToClasses(ByVal oSource As Object) As Collection

        Dim iRow As Int32
        Dim vSource As System.Array
        Dim iRowFrom As Int32
        Dim iRowTo As Int32
        Dim iColFrom As Int32
        Dim iColTo As Int32
        'Dim iCol As Int32

        ' More to do here, need to trap if there are no results, therefore No Array
        Try
            vSource = CType(oSource, System.Array)
        Catch
            vSource = Nothing
        End Try

        ' If we do not have an Array then return Nothing
        If vSource Is Nothing Then
            Return Nothing
        End If

        iColFrom = vSource.GetLowerBound(0) - 1
        iColTo = vSource.GetUpperBound(0) - 1

        iRowFrom = vSource.GetLowerBound(1) - 1
        iRowTo = vSource.GetUpperBound(1) - 1

        ' Create the Array of Additional Data Classes
        Dim oSourceList As New Collection
        Dim oSourceItem As Source

        For iRow = iRowFrom To iRowTo
            oSourceItem = New Source
            oSourceItem.Id = CType(vSource.GetValue(1, iRow + 1), Int32)
            oSourceItem.Code = CType(vSource.GetValue(2, iRow + 1), String)
            oSourceItem.Description = CType(vSource.GetValue(3, iRow + 1), String)
            oSourceList.Add(oSourceItem)
        Next iRow

        Return oSourceList

    End Function

End Class

Public Class AddonsArray

    Public Name As String = String.Empty
    Public PartyCnt As Int32
    Public FeePct As Double
    Public FeeAmt As Double
    Public CommPct As Double
    Public CommAmt As Double
    Public IsIPTable As Int16

End Class

'' These are in iCNQuoteDisplay too...
'Protected Friend Const ACAddonsName As Integer = 0
'Protected Friend Const ACAddonsPartyCnt As Integer = 1
'Protected Friend Const ACAddonsFeePct As Integer = 2
'Protected Friend Const ACAddonsFeeAmt As Integer = 3
'Protected Friend Const ACAddonsCommPct As Integer = 4
'Protected Friend Const ACAddonsCommAmt As Integer = 5
'Protected Friend Const ACAddonsDisplayQuotes As Integer = 6
'Protected Friend Const ACAddonsIsFeeAccount As Integer = 7
'Protected Friend Const ACAddonsActive As Integer = 8
'Protected Friend Const ACAddonsDiscount As Integer = 9
'Protected Friend Const ACAddonsOIKey As Integer = 10
'Protected Friend Const ACAddonsMax As Integer = 10

Public Class SFBPreTransactInput

    ' These are in iCNQuoteDisplay too...
    Private Const ACAddonsName As Integer = 0
    Private Const ACAddonsPartyCnt As Integer = 1
    Private Const ACAddonsFeePct As Integer = 2
    Private Const ACAddonsFeeAmt As Integer = 3
    Private Const ACAddonsCommPct As Integer = 4
    Private Const ACAddonsCommAmt As Integer = 5
    Private Const ACAddonsDisplayQuotes As Integer = 6
    Private Const ACAddonsIsFeeAccount As Integer = 7
    Private Const ACAddonsActive As Integer = 8
    Private Const ACAddonsDiscount As Integer = 9
    Private Const ACAddonsOIKey As Integer = 10
    Private Const ACAddonsMax As Integer = 10

    Public GISDataModelCode As String = ""
    Public GISBusinessTypeCode As String = ""
    Public SchemeID As Int32 = 0
    Public PolicyLinkID As Int32 = 0
    Public BranchID As Int32 = 1
    Public InsuranceFileCnt As Int32 = 0
    Public KeepOutputID As Int32 = 0
    Public Addons() As AddonsArray

    Private m_vAddonsArray(ACAddonsMax, 0) As Object

    ' Transforms the class into the private array
    Public Function TransformArray() As Int32

        Dim oAdd As AddonsArray
        Dim iCount As Integer = 0

        If (Addons Is Nothing) Then
            Return PMEReturnCode.PMTrue
        End If

        ReDim m_vAddonsArray(ACAddonsMax, Addons.Length - 1)

        For Each oAdd In Addons

            m_vAddonsArray(ACAddonsName, iCount) = CType(oAdd.Name, Object)
            m_vAddonsArray(ACAddonsCommAmt, iCount) = CType(oAdd.CommAmt, Object)
            m_vAddonsArray(ACAddonsCommPct, iCount) = CType(oAdd.CommPct, Object)
            m_vAddonsArray(ACAddonsFeeAmt, iCount) = CType(oAdd.FeeAmt, Object)
            m_vAddonsArray(ACAddonsFeePct, iCount) = CType(oAdd.FeePct, Object)
            m_vAddonsArray(ACAddonsPartyCnt, iCount) = CType(oAdd.PartyCnt, Object)
            m_vAddonsArray(ACAddonsActive, iCount) = CType(1, Object)
            iCount += 1

        Next

        Return PMEReturnCode.PMTrue

    End Function

End Class

Public Class SFBPreTransactOutput

    Public LeadInsurerCnt As Int32 = 0

End Class

Public Class SIRIUSUSER

    '<remarks/>
    Public UserID As Int16 = SiriusUserDefaults.UserID

    '<remarks/>
    Public PartyCnt As Int32 = 1

    '<remarks/>
    Public LanguageID As Int16 = SiriusUserDefaults.LanguageID

    '<remarks/>
    Public Username As String = SiriusUserDefaults.Username

    '<remarks/>
    Public Password As String = SiriusUserDefaults.Password

    '<remarks/>
    Public EmailAddress As String = ""

    '<remarks/>
    Public SourceID As Int16 = SiriusUserDefaults.SourceID

    '<remarks/>
    Public CurrencyID As Int16 = SiriusUserDefaults.CurrencyID

End Class

