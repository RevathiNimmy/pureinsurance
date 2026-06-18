Imports SiriusFS.SAM.Structure.SFI.Messaging.WCF

<ServiceContract(Namespace:="PureMessagingService")> _
Public Interface IPureMessagingService

#Region "New Business"
    ''' <summary>
    ''' this will use to Create New Business
    ''' </summary>
    ''' <param name="NewBusinessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function NewBusiness(ByVal NewBusinessRequest As NewBusinessRequestType) As NewBusinessResponseType
#End Region

#Region "New Buinsess Transact"
    ''' <summary>
    ''' this will use to transact New Business
    ''' </summary>
    ''' <param name="NewBusinessTransactRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function NewBusinessTransact(ByVal NewBusinessTransactRequest As NBTransactRequestType) As NBTransactResponseType
#End Region

#Region "GetDatasetSchema"
    ''' <summary>
    ''' this will use to get dataset schema
    ''' </summary>
    ''' <param name="GetDatasetSchemaRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function GetDatasetSchema(ByVal GetDatasetSchemaRequest As GetDatasetSchemaRequestType) As GetDatasetSchemaResponseType
#End Region

#Region "ProcessClaim"
    ''' <summary>
    ''' this will use to process claim
    ''' </summary>
    ''' <param name="ProcessClaimRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function ProcessClaim(ByVal ProcessClaimRequest As BaseClaimProcessRequestType) As BaseClaimProcessResponseType
#End Region

#Region "PolicyProcess"
    ''' <summary>
    ''' this will use to process policy
    ''' </summary>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function PolicyProcess(ByVal PolicyProcessRequest As PolicyProcessRequestType) As PolicyProcessResponseType
#End Region

#Region "PolicyProcessV2"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="PolicyProcessRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function PolicyProcessV2(ByVal PolicyProcessRequest As PolicyProcessV2RequestType) As PolicyProcessV2ResponseType
#End Region

#Region "GenerateDocumentsV2"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GenerateDocumentsV2Request"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function GenerateDocumentsV2(ByVal GenerateDocumentsV2Request As GenerateDocumentsV2RequestType) As GenerateDocumentsV2ResponseType
#End Region
#Region "LoadService"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="LoadServiceRequest"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract>
    Function LoadService(ByVal LoadServiceRequest As LoadServiceRequestType) As Integer
#End Region
End Interface
