Imports System.Configuration.Provider
Imports System.IO
Imports System.Security.Cryptography
Imports System.ServiceModel
Imports System.Text
Imports System.Web
Imports System.Web.Configuration
Imports System.Web.HttpContext
Imports System.Web.Script.Serialization
Imports System.Xml
Imports Ionic.Zip
Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Filters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners


Partial Public Class ProviderSAMForInsuranceV2
    Private sWebServiceUrl As String
    'Test Comment
    Private sDefaultBranchCode As String
    Private iCacheLengthInHours As Integer
    Private nDefaultTimeout As Integer
    Private WebServiceUrl As String
    Private DefaultUserName As String
    Private DefaultPassword As String

    Private oLock As New Object()

    Private sBranchCode As String 'hold the first BranchCode for the Agent
    Private Const CNAgentDetails As String = "AGENT_DETAILS" 'Object returned from a SAM login, contains all the agents details
    Private Const CNBranchCode As String = "BRANCH_CODES"

    Public Overrides Sub Initialize(ByVal name As String, ByVal config As System.Collections.Specialized.NameValueCollection)

        'take the First BranchCode of the Agent in sBranchCode to use throughout the application
        Dim oUserDetails As NexusProvider.UserDetails
        If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session(CNAgentDetails) IsNot Nothing Then
            If HttpContext.Current.Session(CNBranchCode) IsNot Nothing Then
                sBranchCode = HttpContext.Current.Session(CNBranchCode)
            Else
                oUserDetails = HttpContext.Current.Session(CNAgentDetails)
                sBranchCode = oUserDetails.ListOfBranches(0).Code
            End If
        End If

        'set the name and description of the provider if not specified in the config
        If config Is Nothing Then
            Throw New ArgumentNullException("config")
        End If

        If String.IsNullOrEmpty(name) Then
            name = "SAMForInsuranceNexusProvider"
        End If


        'initialize the provider with corrected name/description
        MyBase.Initialize(name, config)


        'Get the default branch code
        sDefaultBranchCode = config("DefaultBranchCode")

        If String.IsNullOrEmpty(sDefaultBranchCode) Then
            Throw New ArgumentNullException("DefaultBranchCode")
        End If
        config.Remove("DefaultBranchCode")


        Dim sCacheLengthInHours As String = config.Item("CacheLengthInHours")

        If String.IsNullOrEmpty(sCacheLengthInHours) Then
            iCacheLengthInHours = PROVIDER_CACHE_HOURS
        Else
            If IsNumeric(sCacheLengthInHours) Then
                iCacheLengthInHours = CInt(sCacheLengthInHours)
            Else
                Throw New ArgumentException("Parameter is does not contain an integer", "CacheLengthInHours")
            End If
        End If
        config.Remove("CacheLengthInHours")
        Dim sDefaultTimeout As String = config.Item("DefaultTimeout")
        If Not String.IsNullOrEmpty(sDefaultTimeout) Then
            If IsNumeric(sDefaultTimeout) Then
                nDefaultTimeout = CInt(sDefaultTimeout)
            Else
                Throw New ArgumentException("Parameter does not contain an integer value", "DefaultTimeout")
            End If
        End If
        config.Remove("DefaultTimeout")

        WebServiceUrl = config("WebServiceUrl")
        config.Remove("WebServiceUrl")
        DefaultUserName = config("DefaultUserName")
        config.Remove("DefaultUserName")
        DefaultPassword = config("DefaultPassword")
        config.Remove("DefaultPassword")

        'If any configuration attributes are left, throw an exception
        If config.Count > 0 Then
            Throw New ProviderException("There are too many configuration attributes specified")
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InitializeCoreServiceMethod() As PureService.PureCoreServiceClient
        Dim oSAM As New PureService.PureCoreServiceClient
        'Write definition for other security things after getting WCF security implemented
        Return oSAM
    End Function

    Private Function InitializePartyServiceMethod() As PureService.PurePartyServiceClient
        Dim oSAM As New PureService.PurePartyServiceClient
        'Write definition for other security things after getting WCF security implemented
        Return oSAM
    End Function
    ''' <summary>
    ''' It will read WCFSecurityToken from config file then return encrypted token as string
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SecurityToken() As String
        Dim sSecurityToken As String = String.Empty
        Dim oCache As System.Web.Caching.Cache = HttpContext.Current.Cache()
        If oCache("WCFEncrptedSecurityToken") IsNot Nothing Then
            sSecurityToken = oCache("WCFEncrptedSecurityToken")
        ElseIf WebConfigurationManager.AppSettings("WCFSecurityToken") IsNot Nothing AndAlso WebConfigurationManager.AppSettings("WCFSecurityToken").Length > 0 Then
            sSecurityToken = BCrypt.Net.BCrypt.HashPassword(WebConfigurationManager.AppSettings("WCFSecurityToken").ToString, 5)
            HttpContext.Current.Cache.Insert("WCFEncrptedSecurityToken", sSecurityToken)
        End If
        Return sSecurityToken
    End Function

    Private Function InitializePolicyServiceMethod() As PureService.PurePolicyServiceClient
        Dim oSAM As New PureService.PurePolicyServiceClient
        'Write definition for other security things after getting WCF security implemented
        Return oSAM
    End Function

    Private Function InitializeClaimServiceMethod() As PureService.PureClaimServiceClient
        Dim oSAM As New PureService.PureClaimServiceClient
        'Write definition for other security things after getting WCF security implemented
        Return oSAM
    End Function

    Private Function InitializeAccountServiceMethod() As PureService.PureAccountServiceClient
        Dim oSAM As New PureService.PureAccountServiceClient
        'Write definition for other security things after getting WCF security implemented
        Return oSAM
    End Function

    Private Function InitializeSecurityServiceMethod() As PureService.PureSecurityServiceClient
        Dim oSAM As New PureService.PureSecurityServiceClient
        'Write definition for other security things after getting WCF security implemented
        Return oSAM
    End Function

    Public Overrides Function GetCurrenciesByBranch(Optional ByVal v_sBranchCode As String = Nothing) As CurrencyCollection

        SyncLock oLock
            Dim sThisBranchCode As String
            Dim oCurrencyCollections As CurrencyCollection = Nothing
            Dim oGetCurrenciesByBranchRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrenciesByBranchQuery
            Dim oGetCurrenciesByBranchResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrenciesByBranchQueryResponse
            Dim oNewCurrency As Currency
            Dim sbLogMessage As StringBuilder

            Try
                oGetCurrenciesByBranchRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrenciesByBranchQuery
                oGetCurrenciesByBranchResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrenciesByBranchQueryResponse
                sbLogMessage = New StringBuilder


                'determine where to get the branch code from
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        sThisBranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        sThisBranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    sThisBranchCode = v_sBranchCode
                End If

                If Current.Cache("CurrenciesForBranch_" & sThisBranchCode) Is Nothing Then


                    With oGetCurrenciesByBranchRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        'set branch code
                        .BranchCode = sThisBranchCode
                    End With


                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)

                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetCurrenciesByBranch, oGetCurrenciesByBranchRequest)
                        oGetCurrenciesByBranchResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrenciesByBranchQueryResponse)(result)
                    End Using




                    'If 1 = 0 Then

                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(oGetCurrenciesByBranchResponse.Errors)
                    'Else

                    oCurrencyCollections = New CurrencyCollection


                    For Each oCurrency As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetCurrenciesByBranchResponseTypeRow In oGetCurrenciesByBranchResponse.Currencies
                        oNewCurrency = New Currency()
                        With oNewCurrency
                            .CurrencyCode = oCurrency.CurrencyCode
                            .Description = oCurrency.Description
                        End With
                        With oGetCurrenciesByBranchResponse
                            oNewCurrency.BaseCurrencyCode = .BaseCurrencyCode
                            oNewCurrency.BaseCurrencyDesc = .BaseCurrencyDescription
                        End With
                        oCurrencyCollections.Add(oNewCurrency)
                    Next
                    'End If
                    Current.Cache.Insert("CurrenciesForBranch_" & sThisBranchCode, oCurrencyCollections, Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                Else
                    'fetch the currencies from cache
                    oCurrencyCollections = CType(Current.Cache("CurrenciesForBranch_" & sThisBranchCode), CurrencyCollection)
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCurrenciesByBranch executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oCurrencyCollections.Count.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetCurrenciesByBranchRequest = Nothing
                oGetCurrenciesByBranchResponse = Nothing
                sThisBranchCode = Nothing
            End Try


            Return oCurrencyCollections
        End SyncLock

    End Function

    ''' <summary>
    ''' This Function is used to retrieve lookup values for the dropdowns. ListType,ListCode and some other parameters are passed as input and returns ?LookupListCollection? object 
    ''' which contains key, parentkey, Code, Description, EffectiveDate, IsDeleted Properties.  BranchCode, is passed as optional Parameter.
    ''' </summary>
    ''' <param name="v_oListType"></param>
    ''' <param name="v_sListCode"></param>
    ''' <param name="v_bExcludeDeletedRecords"></param>
    ''' <param name="v_bExcludeEffectiveDate"></param>
    ''' <param name="v_sParentFieldName"></param>
    ''' <param name="v_iParentFieldValue"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetList(ByVal v_oListType As ListType,
                                ByVal v_sListCode As String,
                                ByVal v_bExcludeDeletedRecords As Boolean,
                                ByVal v_bExcludeEffectiveDate As Boolean,
                                Optional ByVal v_sParentFieldName As String = Nothing,
                                Optional ByVal v_iParentFieldValue As Integer = -1,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByRef v_sXmlElement As System.Xml.XmlElement = Nothing,
                                Optional ByVal v_dEffectiveDate As Date = Nothing,
                                Optional ByVal v_sWhereClause As List(Of ListFilterOptions) = Nothing
                                ) As LookupListCollection

        SyncLock oLock
            Dim oLookupListCollection As LookupListCollection
            Dim oGetListRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQuery
            Dim oGetListResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQueryResponse
            Dim sbLogMessage As StringBuilder
            Try
                oGetListRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQuery
                oGetListResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQueryResponse
                oLookupListCollection = New LookupListCollection
                sbLogMessage = New StringBuilder

                '------------ Added Code to check if the list is in cache ---------------'

                Dim v_sWhereColumn As New List(Of String)
                Dim v_sWhereOperator As New List(Of String)
                Dim v_sWhereValue As New List(Of String)

                If v_sWhereClause IsNot Nothing Then
                    'Split object into separate lists to send to WebServices
                    For Each Item As ListFilterOptions In v_sWhereClause
                        v_sWhereColumn.Add(Trim(Item.ColumnName))
                        v_sWhereOperator.Add(Trim(Item.FilterOperator))
                        v_sWhereValue.Add(Trim(Item.FilterValue))
                    Next
                    'Check if the List is in cache
                    If Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString) IsNot Nothing Then
                        Dim cacheColumn As List(Of String) = Nothing
                        Dim cacheOperator As List(Of String) = Nothing
                        Dim cacheValue As List(Of String) = Nothing

                        cacheColumn = Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereColumn")
                        cacheOperator = Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereOperator")
                        cacheValue = Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereValue")

                        Dim WhereEqual As Boolean = False
                        Dim countColumn As Integer = cacheColumn.Count
                        Dim countOperator As Integer = cacheOperator.Count
                        Dim countValue As Integer = cacheValue.Count

                        If v_sWhereColumn.Count = countColumn And v_sWhereOperator.Count = countOperator And v_sWhereValue.Count = countValue Then
                            WhereEqual = True
                            If countColumn = countOperator And countOperator = countValue Then
                                For i As Integer = 0 To countColumn - 1
                                    If Not (cacheColumn(i) Is v_sWhereColumn(i) And cacheOperator(i) Is v_sWhereOperator(i) And cacheValue(i) Is v_sWhereValue(i)) Then
                                        WhereEqual = False
                                        Exit For
                                    End If
                                Next
                            Else
                                WhereEqual = False
                            End If
                        End If

                        'Remove previous list from cache if new where clause is provided
                        If Not WhereEqual Then
                            Current.Cache.Remove(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString)
                            Current.Cache.Remove(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereColumn")
                            Current.Cache.Remove(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereOperator")
                            Current.Cache.Remove(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereValue")
                            Current.Cache.Remove(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails")
                        End If
                    End If
                End If

                '------------- End ---------------'
                'Cache the definition as this is a waste of bandwidth otherwise
                'Replaced PROVIDER_LOOKUPLIST with v_oListType.ToString & "_" in this method inorder to
                'distinguish "type of list PMlookup/GIS/Userdefined" being cached and retreived from cache.
                If Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString) Is Nothing Then

                    With oGetListRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        'if the passed parameter v_sBranchCode is empty
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode

                        End If

                        .ListCode = v_sListCode
                        .ListType = v_oListType
                        .ExcludeDeletedRecords = v_bExcludeDeletedRecords
                        .ExcludeEffectiveDate = v_bExcludeEffectiveDate
                        .ParentFieldName = v_sParentFieldName
                        .WhereColumnName = v_sWhereColumn
                        .WhereOperator = v_sWhereOperator
                        .WhereValue = v_sWhereValue
                        If v_iParentFieldValue >= 0 Then
                            .ParentFieldValue = v_iParentFieldValue
                            .ParentFieldValueSpecified = True
                        Else
                            .ParentFieldValueSpecified = False
                        End If

                        If v_dEffectiveDate = DateTime.MinValue Then
                            .EffectiveDateSpecified = False
                        Else
                            .EffectiveDateSpecified = True
                            .EffectiveDate = v_dEffectiveDate
                        End If
                    End With


                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.GetList, oGetListRequest)
                        oGetListResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQueryResponse)(result)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                    With oGetListResponse

                        'If 1 = 0 Then
                        'Process the error object if errors, and throw as a single exception
                        'Throw New NexusException(.Errors)
                        'Else

                        oLookupListCollection = New LookupListCollection

                        If .AdditionalResult IsNot Nothing Then
                            Dim xmlDoc As New System.Xml.XmlDocument()
                            Dim payload As String = .AdditionalResult.Trim()

                            xmlDoc.LoadXml("<AdditionalDetails>" & payload & "</AdditionalDetails>")
                            v_sXmlElement = xmlDoc.DocumentElement

                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails", v_sXmlElement,
                                                Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)

                        End If

                        If .GetListResponse IsNot Nothing Then

                            For Each olistItem As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetListResponseType In .GetListResponse
                                With olistItem
                                    'oLookupListCollection.Add(New LookupListItem(.Key, .ParentKey, .Code.Trim(),
                                    '    .Description.Trim(), .EffectiveDate, .IsDeleted, .IsDefault))
                                    oLookupListCollection.Add(New LookupListItem(
                                                                v_iKey:= .Key,
                                                                v_iParentKey:=If(.ParentKey.HasValue, .ParentKey.Value, 0),          ' default 0
                                                                v_sCode:=If(.Code, String.Empty).Trim(),
                                                                v_sDescription:=If(.Description, String.Empty).Trim(),
                                                                v_dtEffectiveDate:= .EffectiveDate,
                                                                v_bIsDeleted:= .IsDeleted,
                                                                v_bIsDefault:= .IsDefault
                                                            ))
                                End With
                            Next

                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString, oLookupListCollection,
                                                    Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        End If

                        '------------ Added to cache where clause fields ---------------'

                        If v_sWhereColumn IsNot Nothing And v_sWhereOperator IsNot Nothing And v_sWhereValue IsNot Nothing Then
                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereColumn", v_sWhereColumn,
                                                    Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereOperator", v_sWhereOperator,
                                                    Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_WhereValue", v_sWhereValue,
                                                    Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        End If

                        '------------- End ---------------'

                        'End If

                    End With

                Else
                    oLookupListCollection = CType(Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString), LookupListCollection)

                    If Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails") IsNot Nothing Then
                        v_sXmlElement = CType(Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails"), System.Xml.XmlElement)
                    End If

                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetList executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oListType = " & v_oListType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sListCode = " & v_sListCode.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oLookupListCollection.Count.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetListRequest = Nothing
                oGetListResponse = Nothing
            End Try


            Return oLookupListCollection

        End SyncLock

    End Function



    Public Overrides Function GetOptionSetting(ByVal v_oOptionType As NexusProvider.OptionType,
                                 ByVal v_iOptionNumber As Integer,
                                 Optional ByVal v_sBranchCode As String = Nothing) As OptionTypeSetting

        SyncLock oLock
            Dim oOptionTypeSetting As OptionTypeSetting
            Dim oGetOptionSettingRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetOptionSettingQuery
            Dim oGetOptionSettingResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetOptionSettingQueryResponse
            Dim sbLogMessage As StringBuilder

            Try
                oOptionTypeSetting = New OptionTypeSetting
                oGetOptionSettingRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetOptionSettingQuery
                oGetOptionSettingResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetOptionSettingQueryResponse
                sbLogMessage = New StringBuilder

                If Current.Cache("OptionSetting_" & v_iOptionNumber.ToString & "_" & v_oOptionType) Is Nothing Then

                    With oGetOptionSettingRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If
                        'Branch Code is Hard Code, since for all setting HeadOff is required
                        v_sBranchCode = "HeadOff"
                        .BranchCode = v_sBranchCode
                        .OptionNumber = v_iOptionNumber
                        .OptionType = v_oOptionType

                    End With


                    Using trace As New Tracer(Category.Trace)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetOptionSetting, oGetOptionSettingRequest)
                        oGetOptionSettingResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetOptionSettingQueryResponse)(result)
                    End Using




                    With oGetOptionSettingResponse
                        'If 1 = 0 Then
                        'Process the error object if errors, and throw as a single exception
                        'Throw New NexusException(.Errors)
                        'Else
                        oOptionTypeSetting.OptionValue = .OptionValue
                        oOptionTypeSetting.UnderwritingType = .UnderwritingType
                        oOptionTypeSetting.AccountType = .AccountType
                        'Put it in Cache in order to read it from cache instead of SAM call
                        Current.Cache.Insert("OptionSetting_" & v_iOptionNumber.ToString & "_" & v_oOptionType, oOptionTypeSetting,
                                                                          Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        'End If
                    End With
                Else
                    oOptionTypeSetting = CType(Current.Cache("OptionSetting_" & v_iOptionNumber.ToString & "_" & v_oOptionType), OptionTypeSetting)
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetOptionSetting executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    'sbLogMessage.AppendLine(OptionType.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetOptionSettingRequest = Nothing
                oGetOptionSettingResponse = Nothing
            End Try

            Return oOptionTypeSetting

        End SyncLock

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AddCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oAddCoverNoteBookRequest As AddCoverNoteBookRequestType
            'Dim oAddCoverNoteBookResponse As AddCoverNoteBookResponseType
            'Dim oCoverNoteProduct As BaseCoverNoteBookTypeRow
            Dim iCounter As Integer
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oAddCoverNoteBookRequest = New AddCoverNoteBookRequestType
                'oAddCoverNoteBookResponse = New AddCoverNoteBookResponseType
                sbLogMessage = New StringBuilder


                'With oAddCoverNoteBookRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode
                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode

                '    End If

                '    If r_oCoverNote.AgentKey > 0 Then
                '        .AgentKey = r_oCoverNote.AgentKey
                '        .AgentKeySpecified = True
                '    Else
                '        .AgentKeySpecified = False
                '    End If
                '    .BookNumber = r_oCoverNote.BookNumber
                '    .CoverNoteBranchCode = r_oCoverNote.CoverNoteBranchCode

                '    .CoverNoteProducts = New List(Of BaseCoverNoteBookTypeRow)
                '    For iCounter = 0 To r_oCoverNote.CoverNoteBookProducts.Count - 1
                '        oCoverNoteProduct = New BaseCoverNoteBookTypeRow
                '        oCoverNoteProduct.ProductCode = r_oCoverNote.CoverNoteBookProducts(iCounter).ProductCode
                '        .CoverNoteProducts.Add(oCoverNoteProduct)
                '    Next

                '    .CoverNoteStatusCode = r_oCoverNote.CoverNoteStatusCode
                '    .EffectiveDate = r_oCoverNote.EffectiveDate
                '    .StartNumber = r_oCoverNote.StartNumber
                '    .EndNumber = r_oCoverNote.EndNumber

                'End With


                Using trace As New Tracer(Category.Trace)
                    ' oAddCoverNoteBookResponse = oSAM.AddCoverNoteBook(oAddCoverNoteBookRequest)
                End Using

                'With oAddCoverNoteBookResponse
                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else
                '        r_oCoverNote.CoverNoteBookTimestamp = oAddCoverNoteBookResponse.CoverNoteBookTimestamp
                '    End If
                'End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddCoverNoteBook executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(r_oCoverNote.Print.Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    sbLogMessage.AppendLine("CoverNoteBookTimeStamp: " & r_oCoverNote.CoverNoteBookTimestamp.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oCoverNote.StartNumber) Then
                        sbLogMessage.AppendLine("r_oCoverNote.StartNumber" & r_oCoverNote.StartNumber.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("r_oCoverNote.StartNumber=nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oCoverNote.EndNumber) Then
                        sbLogMessage.AppendLine("r_oCoverNote.EndNumber" & r_oCoverNote.EndNumber.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("r_oCoverNote.EndNumber=nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oCoverNote.EffectiveDate) Then
                        sbLogMessage.AppendLine("r_oCoverNote.EffectiveDate" & r_oCoverNote.EffectiveDate.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("r_oCoverNote.EffectiveDate=nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oAddCoverNoteBookRequest = Nothing
                'oAddCoverNoteBookResponse = Nothing
            End Try

        End SyncLock
    End Sub

    Public Overrides Sub AddCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                              Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oAddCoverNoteSheetRequest As AddCoverNoteSheetRequestType
            'Dim oAddCoverNoteSheetResponse As AddCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oAddCoverNoteSheetRequest = New AddCoverNoteSheetRequestType
                'oAddCoverNoteSheetResponse = New AddCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                'With oAddCoverNoteSheetRequest
                '.LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '.WCFSecurityToken = SecurityToken()
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        '.BranchCode = sDefaultBranchCode

                    Else
                        'Use the branch code in session 
                        '.BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    '.BranchCode = v_sBranchCode
                End If

                'Checking the CoverNoteBookKey
                'If r_oCoverNote.CoverNoteBookKey > 0 Then
                '    .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                'Else
                '    Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                'End If

                'Checking the CoverNoteSheetNumber
                'If r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber > 0 Then
                '    '.CoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber
                'Else
                '    Throw New ArgumentNullException("CoverNote.CoverNoteSheets.CoverNoteSheetNumber")
                'End If


                '.CoverNoteStatusCode = r_oCoverNote.CoverNoteBookStatusCode
                '    .Comments = r_oCoverNote.Comments
                '    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                'End With


                Using trace As New Tracer(Category.Trace)
                    'oAddCoverNoteSheetResponse = oSAM.AddCoverNoteSheet(oAddCoverNoteSheetRequest)
                End Using

                'With oAddCoverNoteSheetResponse
                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else
                '        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                '    End If
                'End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetNumber" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If


                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oAddCoverNoteSheetRequest = Nothing
                'oAddCoverNoteSheetResponse = Nothing
            End Try


        End SyncLock

    End Sub

#Region "WPR 29"
    ''' <summary>
    ''' This Function is used to Insert a File(Import Document). 
    ''' This method takes ?oAddDocumentToDocumasterType?,'v_sBranchCode' as input and returns an integer of added document number 
    ''' Class.
    ''' </summary>
    ''' <param name="oAddDocumentToDocumasterType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function AddDocumentToDocumaster(ByVal oAddDocumentToDocumasterType As AddDocumentToDocumasterType,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As Integer
        SyncLock oLock
            Dim oAddDocumentToDocumasterRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddDocumentToDocumasterCommand
            Dim oAddDocumentToDocumasterResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddDocumentToDocumasterCommandResponse
            Dim docNumField As Integer = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oAddDocumentToDocumasterRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddDocumentToDocumasterCommand
                oAddDocumentToDocumasterResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddDocumentToDocumasterCommandResponse
                sbLogMessage = New StringBuilder


                With oAddDocumentToDocumasterRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    With oAddDocumentToDocumasterType
                        If .v_iClaimKey > 0 Then
                            oAddDocumentToDocumasterRequest.ClaimKey = .v_iClaimKey
                        End If

                        If .v_iFolderNum > 0 Then
                            oAddDocumentToDocumasterRequest.FolderNum = .v_iFolderNum
                        End If

                        If .v_iInsuranceFolderKey > 0 Then
                            oAddDocumentToDocumasterRequest.InsuranceFolderKey = .v_iInsuranceFolderKey
                        End If

                        If .v_iPartyKey > 0 Then
                            oAddDocumentToDocumasterRequest.PartyKey = .v_iPartyKey
                        End If

                        oAddDocumentToDocumasterRequest.VisibleFromWeb = .v_bVisibleFromWeb

                        If IO.File.Exists(.v_sFileName) Then
                            'Create a unique byte array retrieved from the Web Service
                            Dim fsInputFile As IO.FileStream = New IO.FileStream(.v_sFileName, FileMode.Open, FileAccess.Read)
                            Dim bytes() As Byte = New Byte((fsInputFile.Length) - 1) {}
                            fsInputFile.Read(bytes, 0, fsInputFile.Length)
                            fsInputFile.Close()
                            oAddDocumentToDocumasterRequest.Document = bytes
                            'Delete the temp Folder
                            Directory.Delete(Path.GetDirectoryName(.v_sFileName), True)
                        End If

                        If Not String.IsNullOrEmpty(.v_sDescription) Then
                            oAddDocumentToDocumasterRequest.Description = .v_sDescription
                        End If

                        If Not String.IsNullOrEmpty(.v_sFileName) Then
                            oAddDocumentToDocumasterRequest.FileName = Path.GetFileName(.v_sFileName)
                        End If
                        If .v_iDocumentTemplateGroupId > 0 Then
                            oAddDocumentToDocumasterRequest.DocumentTemplateGroupId = .v_iDocumentTemplateGroupId
                        End If
                        If .v_iDocumentTemplateSubGroupId > 0 Then
                            oAddDocumentToDocumasterRequest.DocumentTemplateSubGroupId = .v_iDocumentTemplateSubGroupId
                        End If

                    End With
                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddDocumentToDocumaster, oAddDocumentToDocumasterRequest)
                    oAddDocumentToDocumasterResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddDocumentToDocumasterCommandResponse)(result)
                End Using

                With oAddDocumentToDocumasterResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        docNumField = oAddDocumentToDocumasterResponse.DocNum
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddDocumentToDocumaster executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iClaimKey = " & oAddDocumentToDocumasterType.v_iClaimKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFolderKey = " & oAddDocumentToDocumasterType.v_iInsuranceFolderKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & oAddDocumentToDocumasterType.v_iPartyKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bVisibleFromWeb = " & oAddDocumentToDocumasterType.v_bVisibleFromWeb.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDescription = " & oAddDocumentToDocumasterType.v_sDescription.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sFileName = " & oAddDocumentToDocumasterType.v_sFileName.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDocument " & oAddDocumentToDocumasterType.v_bDocument.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddDocumentToDocumasterRequest = Nothing
                oAddDocumentToDocumasterResponse = Nothing
            End Try


            Return docNumField

        End SyncLock

    End Function


#End Region

    Public Overrides Sub AddEventNote(ByRef r_oEventDetails As EventDetails,
                                  Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oAddEventNoteRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventNoteCommand
            Dim oAddEventNoteResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventNoteCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oAddEventNoteRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventNoteCommand
                oAddEventNoteResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventNoteCommandResponse
                sbLogMessage = New StringBuilder


                With oAddEventNoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .EventKey = r_oEventDetails.EventKey
                    .EventText = r_oEventDetails.EventText

                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddEventNote, oAddEventNoteRequest)
                    oAddEventNoteResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventNoteCommandResponse)(result)
                End Using

                With oAddEventNoteResponse
                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oEventDetails.EventKey = .EventKey
                        r_oEventDetails.EventPublicTextKey = .EventPublicTextKey

                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddEventNote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddEventNoteRequest = Nothing
                oAddEventNoteResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub AddTaskGroup(ByRef v_oAddTaskGroup As TaskGroup,
                   Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oAddTaskGroupRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddTaskGroupCommandBase
            Dim oAddTaskGroupResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddTaskGroupCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oAddTaskGroupRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddTaskGroupCommandBase
                oAddTaskGroupResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddTaskGroupCommandResponse
                sbLogMessage = New StringBuilder


                With oAddTaskGroupRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Code = v_oAddTaskGroup.Code
                    .Description = v_oAddTaskGroup.Description

                    If v_oAddTaskGroup.CaptionId > 0 Then
                        .CaptionId = v_oAddTaskGroup.CaptionId
                    Else
                        Throw New ArgumentException("TaskGroup.CaptionId")
                    End If
                    If v_oAddTaskGroup.EffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("TaskGroup.EffectiveDate")
                    Else
                        .EffectiveDate = v_oAddTaskGroup.EffectiveDate
                    End If

                    .IsDeleted = v_oAddTaskGroup.IsDeleted

                    If v_oAddTaskGroup.TaskGroupCategoryKey > 0 Then
                        .TaskGroupCategoryKey = v_oAddTaskGroup.TaskGroupCategoryKey
                    Else
                        Throw New ArgumentException("TaskGroup.TaskGroupCategoryKey")
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddTaskGroups, oAddTaskGroupRequest)
                    oAddTaskGroupResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddTaskGroupCommandResponse)(result)
                End Using




                With oAddTaskGroupResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    v_oAddTaskGroup.TaskGroupKey = .TaskGroupKey
                    v_oAddTaskGroup.TimeStamp = .ApiTimeStamp
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddTaskGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oAddTaskGroup.Print.Replace("<br />", vbCrLf))

                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    If Not IsNothing(v_oAddTaskGroup.TaskGroupKey) Then
                        sbLogMessage.AppendLine("Task Group Key: " & v_oAddTaskGroup.TaskGroupKey.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Group Key: nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_oAddTaskGroup.TimeStamp) Then
                        sbLogMessage.AppendLine("Time Stamp: " & v_oAddTaskGroup.TimeStamp.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Time Stamp: nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oAddTaskGroupRequest = Nothing
                oAddTaskGroupResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub AddUserGroup(ByRef v_oAddUserGroup As UserGroup,
                Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oAddUserGroupRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddUserGroupCommandBase
            Dim oAddUserGroupResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddUserGroupCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oAddUserGroupRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddUserGroupCommandBase
                oAddUserGroupResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddUserGroupCommandResponse
                sbLogMessage = New StringBuilder


                With oAddUserGroupRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Code = v_oAddUserGroup.Code
                    .Description = v_oAddUserGroup.Description

                    If v_oAddUserGroup.EffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("UserGroup.EffectiveDate")
                    Else
                        .EffectiveDate = v_oAddUserGroup.EffectiveDate
                    End If

                    .IsDeleted = v_oAddUserGroup.IsDeleted
                    .IsSysAdmin = v_oAddUserGroup.IsSysAdmin
                End With

                Using trace As New Tracer(Category.Trace)

                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddUserGroup, oAddUserGroupRequest)
                    oAddUserGroupResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddUserGroupCommandResponse)(result)
                End Using




                With oAddUserGroupResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    v_oAddUserGroup.UserGroupKey = .UserGroupKey
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddUserGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oAddUserGroup.Print().Replace("<br />", vbCrLf))

                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    If Not IsNothing(v_oAddUserGroup.UserGroupKey) Then
                        sbLogMessage.AppendLine("Task Group Key: " & v_oAddUserGroup.UserGroupKey.ToString() & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Group Key: nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddUserGroupRequest = Nothing
                oAddUserGroupResponse = Nothing
            End Try

        End SyncLock
    End Sub

    Public Overrides Sub AddWmTaskLog(ByVal v_iTaskInstanceKey As Integer,
             ByVal v_sLogText As String,
             Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oAddWmTaskLogRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddWmTaskLogCommand
            Dim oAddWmTaskLogResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddWmTaskLogCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oAddWmTaskLogRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddWmTaskLogCommand
                oAddWmTaskLogResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddWmTaskLogCommandResponse
                sbLogMessage = New StringBuilder

                With oAddWmTaskLogRequest

                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iTaskInstanceKey > 0 Then
                        .TaskInstanceKey = v_iTaskInstanceKey
                    Else
                        Throw New ArgumentException("WMTaskLog.TaskInstanceKey")
                    End If

                    .LogText = v_sLogText

                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddWmTaskLog, oAddWmTaskLogRequest)
                    oAddWmTaskLogResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddWmTaskLogCommandResponse)(result)
                End Using




                With oAddWmTaskLogResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddWmTaskLog executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_iTaskInstanceKey) Then
                        sbLogMessage.AppendLine("Task Instance Key: nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Instance Key: " & v_iTaskInstanceKey & vbCrLf)
                    End If

                    If IsNothing(v_sLogText) Then
                        sbLogMessage.AppendLine("Log Text: nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Log Text: " & v_sLogText & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddWmTaskLogRequest = Nothing
                oAddWmTaskLogResponse = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' This Method Calculate the UnAllocated Band
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateUnAllocated(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        'Calculate/Retreive Band Total
        Dim dBANDSumInsured, dBANDPremium As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("Premium").Value, dBANDPremium)
        End If

        'Calculate/Retreive Allocated Total
        Dim dAllocatedSumInsured, dAllocatedPremium As Decimal
        oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Allocated']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dAllocatedSumInsured)
            Decimal.TryParse(oNode.Attributes("Premium").Value, dAllocatedPremium)
        End If

        'Add UnAllocated if there is Any
        Dim dUnAllocatedSumInsured, dUnAllocatedPremium As Decimal
        dUnAllocatedSumInsured = dBANDSumInsured - dAllocatedSumInsured
        dUnAllocatedPremium = dBANDPremium - dAllocatedPremium

        If dUnAllocatedSumInsured <> 0 Or dUnAllocatedPremium <> 0 Then
            'Add into the XML
            Dim sArrangementRow As String = "ArrangementRow"
            Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
            Dim myCol As DataColumn
            Dim sValue As String = ""
            For Each myCol In oRITable.Columns
                If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "Premium" _
                Or myCol.ColumnName = "Name" Or myCol.ColumnName = "IsDeleted" Then

                    'Name
                    If myCol.ColumnName = "Name" Then
                        ArrangementRow.SetAttribute(myCol.ColumnName, "Unallocated")
                    End If

                    'Sum Insured
                    sValue = ""
                    If myCol.ColumnName = "SumInsured" Then
                        sValue = dUnAllocatedSumInsured
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'Premium
                    sValue = ""
                    If myCol.ColumnName = "Premium" Then
                        sValue = dUnAllocatedPremium
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'IsDeleted
                    If myCol.ColumnName = "IsDeleted" Then
                        ArrangementRow.SetAttribute(myCol.ColumnName, "False")
                    End If

                Else
                    sValue = ""
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            Next

            RIBand.InsertAfter(ArrangementRow, RIBand.LastChild)
        End If
    End Sub

    Public Overrides Function CreateBackgroundJob(ByVal v_sDescription As String,
                                                   ByVal v_sJobXML As String,
                                                   ByVal v_dJobWhenToStart As Date,
                                                           Optional ByVal v_sBranchCode As String = Nothing) As Integer

        SyncLock oLock

            Dim oCreateBackgroundJobRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateBackgroundJobCommand
            Dim oCreateBackgroundJobResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateBackgroundJobCommandResponse
            Dim iBackgroundJobID As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oCreateBackgroundJobRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateBackgroundJobCommand
                oCreateBackgroundJobResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateBackgroundJobCommandResponse
                sbLogMessage = New StringBuilder


                With oCreateBackgroundJobRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .Description = v_sDescription
                    .JobWhenToStart = v_dJobWhenToStart
                    .JobXML = v_sJobXML

                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CreateBackgroundJob, oCreateBackgroundJobRequest)
                    oCreateBackgroundJobResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateBackgroundJobCommandResponse)(result)

                End Using






                With oCreateBackgroundJobResponse

                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)

                    'Else
                    iBackgroundJobID = oCreateBackgroundJobResponse.CreateBackgroundJobResponse.BackgroundJobID

                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreateBackgroundJob executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_sDescription) Then
                        sbLogMessage.AppendLine("Description : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Description : " & v_sDescription.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    If IsNothing(iBackgroundJobID) Then
                        sbLogMessage.AppendLine("Background Job ID : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Background Job ID : " & iBackgroundJobID.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oCreateBackgroundJobRequest = Nothing
                oCreateBackgroundJobResponse = Nothing
            End Try


            Return iBackgroundJobID
        End SyncLock
    End Function

    Public Overrides Function CreateWmTask(ByVal v_oCreateWmTask As WorkManager,
                                           Optional ByVal v_sBranchCode As String = Nothing) As WorkManager

        SyncLock oLock

            Dim oCreateWmTaskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateWmTaskCommand
            Dim oCreateWmTaskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateWmTaskCommandResponse
            Dim iCounter As Integer = 0
            Dim oKeyData As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreateWmTaskRequestTypeRow
            Dim sbLogMessage As StringBuilder
            Dim oWorkManager As New WorkManager
            Dim parsed As SSP.PureInsuranceRestAPIHandler.Enums.TaskLockName

            Try
                oCreateWmTaskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateWmTaskCommand
                oCreateWmTaskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateWmTaskCommandResponse
                sbLogMessage = New StringBuilder

                With oCreateWmTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .AllocationUser = v_oCreateWmTask.AllocationUser
                    .AllocationUserGroup = v_oCreateWmTask.AllocationUserGroup
                    .Client = v_oCreateWmTask.Client
                    .Description = v_oCreateWmTask.Description
                    .IsExternalItem = v_oCreateWmTask.IsExternalItem
                    .ExternalTaskCategoryCode = v_oCreateWmTask.ExternalTaskCategoryCode
                    .ParentTaskId = v_oCreateWmTask.ParentTaskId
                    .GuidPMExternalItem = v_oCreateWmTask.GuidPMExternalItem

                    If [Enum].TryParse(Of SSP.PureInsuranceRestAPIHandler.Enums.TaskLockName)(v_oCreateWmTask.LockName, parsed) Then
                        .LockName = parsed
                    End If
                    .LockValue = v_oCreateWmTask.LockValue
                    If v_oCreateWmTask.DueDate = DateTime.MinValue Then
                        Throw New ArgumentException("WmTask.DueDateTime")
                    Else
                        .DueDateTime = v_oCreateWmTask.DueDate
                    End If
                    .IsComplete = v_oCreateWmTask.IsComplete
                    .IsTaskReview = v_oCreateWmTask.IsTaskReview
                    .IsUrgent = v_oCreateWmTask.IsUrgent
                    .Task = v_oCreateWmTask.Task
                    .TaskGroup = v_oCreateWmTask.TaskGroup
                    .ExternalTaskStatus = v_oCreateWmTask.ExternalTaskStatus 'Send -1 if you do not want to use it.


                    .IsExternalChildTask = v_oCreateWmTask.IsExternalChildTask
                    'Adding KeyData Collection
                    If v_oCreateWmTask.KeyData IsNot Nothing AndAlso v_oCreateWmTask.KeyData.Count > 0 Then

                        .KeyData = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreateWmTaskRequestTypeRow)

                        For iCounter = 0 To v_oCreateWmTask.KeyData.Count - 1
                            oKeyData = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreateWmTaskRequestTypeRow
                            oKeyData.KeyName = v_oCreateWmTask.KeyData(iCounter).KeyName
                            oKeyData.KeyValue = v_oCreateWmTask.KeyData(iCounter).KeyValue
                            .KeyData.Add(oKeyData)
                        Next


                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CreateWmTasks, oCreateWmTaskRequest)
                    oCreateWmTaskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CreateWmTaskCommandResponse)(result)
                End Using


                With oCreateWmTaskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oWorkManager.GuidPMExternalItem = oCreateWmTaskResponse.GuidPMExternalItem
                        oWorkManager.TaskInstanceKey = oCreateWmTaskResponse.TaskInstanceKey
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreateWmTask executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oCreateWmTask.Print().Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If


                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oCreateWmTaskRequest = Nothing
                oCreateWmTaskResponse = Nothing
            End Try
            Return oWorkManager

        End SyncLock
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub DeleteCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                      Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oDeleteCoverNoteSheetRequest As DeleteCoverNoteSheetRequestType
            'Dim oDeleteCoverNoteSheetResponse As DeleteCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oDeleteCoverNoteSheetRequest = New DeleteCoverNoteSheetRequestType
                'oDeleteCoverNoteSheetResponse = New DeleteCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                'With oDeleteCoverNoteSheetRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode

                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode
                '    End If
                '    'Checking the CoverNoteBookKey
                '    If r_oCoverNote.CoverNoteBookKey > 0 Then
                '        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                '    Else
                '        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                '    End If

                '    'Checking the CoverNoteSheetKey
                '    If r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber > 0 Then
                '        .CoverNoteSheetKey = r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber
                '    Else
                '        Throw New ArgumentNullException("CoverNote.CoverNoteSheets.CoverNoteSheetKey")
                '    End If

                '    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                'End With


                'Using trace As New Tracer(Category.Trace)
                '    oDeleteCoverNoteSheetResponse = oSAM.DeleteCoverNoteSheet(oDeleteCoverNoteSheetRequest)
                'End Using

                'With oDeleteCoverNoteSheetResponse

                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else
                '        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                '    End If
                'End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetKey" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine(" r_oCoverNote.CoverNoteBookTimestamp" & r_oCoverNote.CoverNoteBookTimestamp.ToString() & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(" r_oCoverNote.CoverNoteBookTimestamp" & r_oCoverNote.CoverNoteBookTimestamp.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oDeleteCoverNoteSheetRequest = Nothing
                'oDeleteCoverNoteSheetResponse = Nothing
            End Try

        End SyncLock

    End Sub

    Public Overrides Sub DeleteUndeleteUserGroup(ByVal v_bDeleted As Boolean,
                                              ByVal v_sUserGroupCode As String,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oDeleteUndeleteUserGroupRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteUndeleteUserGroupCommand
            Dim oDeleteUndeleteUserGroupResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteUndeleteUserGroupCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oDeleteUndeleteUserGroupRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteUndeleteUserGroupCommand
                oDeleteUndeleteUserGroupResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteUndeleteUserGroupCommandResponse
                sbLogMessage = New StringBuilder


                With oDeleteUndeleteUserGroupRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Deleted = v_bDeleted
                    .UserGroupCode = v_sUserGroupCode
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Delete(ApiMethods.DeleteUndeleteUserGroup, oDeleteUndeleteUserGroupRequest)
                    oDeleteUndeleteUserGroupResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteUndeleteUserGroupCommandResponse)(result)
                End Using




                With oDeleteUndeleteUserGroupResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteUndeleteUserGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oDeleteUndeleteUserGroupRequest = Nothing
                oDeleteUndeleteUserGroupResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub DeleteWmTask(ByRef r_oWorkManager As WorkManager,
                             Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oDeleteWmTaskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteWmTaskCommand
            Dim oDeleteWmTaskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteWmTaskCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oDeleteWmTaskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteWmTaskCommand
                oDeleteWmTaskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteWmTaskCommandResponse
                sbLogMessage = New StringBuilder


                With oDeleteWmTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                    .TaskTimeStamp = r_oWorkManager.TimeStamp

                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Delete(ApiMethods.DeleteWmTasks, oDeleteWmTaskRequest)
                    oDeleteWmTaskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteWmTaskCommandResponse)(result)
                End Using




                With oDeleteWmTaskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                    r_oWorkManager.TimeStamp = .TaskTimeStamp
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteWmTask executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If


                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oDeleteWmTaskRequest = Nothing
                oDeleteWmTaskResponse = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objFaultException"></param>
    ''' <remarks></remarks>
    '''         
    Public Const EVENT_LOG_APP_NAME As String = "Sirius"
    Public Const EVENT_LOG_FILE_NAME As String = "Application"
    Public Property LogWriter() As LogWriter
        Get
            Return m_LogWriter
        End Get
        Private Set(value As LogWriter)
            m_LogWriter = value
        End Set
    End Property
    Private m_LogWriter As LogWriter

    Public Sub FaultErrorHandler(ByVal objFaultException As FaultException(Of PureService.SAMMethodResponseData))
        ' Dim objSAMMethodResponseData As PureService.SAMMethodResponseData = objFaultException.
        '  Throw New (objSAMMethodResponseData.ExtensionData)
        'TBD
        Try

            CreateManualWriter()
        Catch

        End Try

        If Not EventLog.SourceExists(EVENT_LOG_APP_NAME, ".") Then
            Dim creationData As New EventSourceCreationData(EVENT_LOG_APP_NAME, EVENT_LOG_FILE_NAME)
            creationData.MachineName = "."
            EventLog.CreateEventSource(creationData)
        End If

        Dim logEntry As New LogEntry()
        logEntry.Priority = 1
        logEntry.Severity = TraceEventType.Warning
        logEntry.Message = objFaultException.StackTrace
        logEntry.ExtendedProperties = Nothing
        m_LogWriter.Write(logEntry)
    End Sub

    Public Sub CreateManualWriter()
        Dim GeneralCategory As String = "General"
        Dim ErrorCategory As String = "Errors"
        Dim formatter = New TextFormatter("Timestamp: {timestamp}{newline}" + "Category: {category}{newline}" + "Message: {message}{newline}" + "Extended Properties: {dictionary({key} - {value}{newline})}")

        ' Log messages to event log 
        Dim logListener = New FormattedEventLogTraceListener(EVENT_LOG_APP_NAME, EVENT_LOG_FILE_NAME, ".", formatter)

        'this source has our listener
        Dim mainLogSource As LogSource = New LogSource(GeneralCategory, SourceLevels.All)
        mainLogSource.Listeners.Add(logListener)

        ' Don't log to this source
        Dim emptyLogSource As LogSource = New LogSource("Empty")

        ' "Error" category goes to main log source
        Dim traceSources As Dictionary(Of String, LogSource) = New Dictionary(Of String, LogSource)() From {
            {GeneralCategory, mainLogSource}
        }

        ' filter "Error" category
        Dim categoryFilter As CategoryFilter = New CategoryFilter("All", New List(Of String)() From {
            GeneralCategory
        }, CategoryFilterMode.DenyAllExceptAllowed)
        Dim filters As IEnumerable(Of ILogFilter) = New List(Of ILogFilter)() From {
            categoryFilter
        }

        ' in EntLib5 can't use LogWriter (it's abstract) or LogWriterFactory (which uses IServiceLocator)
        'The collection of filters to use when processing an entry
        'The trace sources to dispatch entries to
        'The special LogSource to which all log entries should be logged.
        'The special LogSource to which log entries with at least one non-matching category should be logged
        'The special LogSource to which internal errors must be logged
        'The default category to set when entry categories list of a log entry is empty
        'The tracing status
        'true if warnings should be logged when a non-matching category is found
        m_LogWriter = New LogWriterImpl(filters, traceSources, mainLogSource, mainLogSource, mainLogSource, GeneralCategory,
            True, True)
    End Sub

#Region "WPR32 - PostCode and Address Validation"
    ''' <summary>
    ''' this method will fetch the address details on the basis of below mentioned parameters and will return address collection
    ''' </summary>
    ''' <param name="v_oAddress"></param>
    ''' <remarks></remarks>
    Public Overrides Function FindAddress(ByVal v_oAddress As Address,
                                     Optional ByVal v_sBranchCode As String = Nothing) As AddressCollection

        SyncLock oLock

            Dim oFindAddressRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindAddressQuery
            Dim oFindAddressResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindAddressQueryResponse
            Dim oAddressCollection As AddressCollection
            Dim oAddresses As Address
            Dim iAddressKey As Integer = 1
            Try
                oFindAddressRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindAddressQuery
                oFindAddressResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindAddressQueryResponse
                oAddressCollection = New AddressCollection


                With oFindAddressRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_oAddress IsNot Nothing Then
                        If Not (String.IsNullOrEmpty(v_oAddress.Address1)) Then
                            .AddressLine1 = CStr(v_oAddress.Address1.Trim)
                        End If
                        If Not (String.IsNullOrEmpty(v_oAddress.Address2)) Then
                            .AddressLine2 = CStr(v_oAddress.Address2.Trim)
                        End If
                        If Not (String.IsNullOrEmpty(v_oAddress.Address3)) Then
                            .AddressLine3 = CStr(v_oAddress.Address3.Trim)
                        End If
                        If Not (String.IsNullOrEmpty(v_oAddress.Address4)) Then
                            .AddressLine4 = CStr(v_oAddress.Address4.Trim)
                        End If
                        If Not (String.IsNullOrEmpty(v_oAddress.PostCode)) Then
                            .PostCode = CStr(v_oAddress.PostCode.Trim)
                        End If
                        If Not (String.IsNullOrEmpty(v_oAddress.CountryCode)) Then
                            .CountryCode = CStr(v_oAddress.CountryCode.Trim)
                        End If
                    End If
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.AddEventNote, oFindAddressRequest)
                    oFindAddressResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.FindAddressQueryResponse)(result)
                End Using




                'Make an instance of AddressCollection


                With oFindAddressResponse.FindAddressResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else

                    If .AddressLookup IsNot Nothing Then
                        If .AddressLookup.Count > 0 Then

                            For Each oAddress As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressLookupType In .AddressLookup
                                If Not (String.IsNullOrEmpty(oAddress.AddressLine1) _
                                     AndAlso String.IsNullOrEmpty(oAddress.AddressLine2) _
                                     AndAlso String.IsNullOrEmpty(oAddress.AddressLine3) _
                                     AndAlso String.IsNullOrEmpty(oAddress.AddressLine4) _
                                     AndAlso String.IsNullOrEmpty(oAddress.PostCode) _
                                     AndAlso String.IsNullOrEmpty(oAddress.CountryCode)) Then
                                    oAddresses = New Address
                                    oAddresses.Key = iAddressKey
                                    oAddresses.Address1 = oAddress.AddressLine1
                                    oAddresses.Address2 = oAddress.AddressLine2
                                    oAddresses.Address3 = oAddress.AddressLine3
                                    oAddresses.Address4 = oAddress.AddressLine4
                                    oAddresses.PostCode = oAddress.PostCode
                                    oAddresses.CountryKey = oAddress.CountryCode
                                    oAddressCollection.Add(oAddresses)
                                    iAddressKey += 1
                                End If
                            Next
                        End If
                    End If
                    'End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oFindAddressRequest = Nothing
                oFindAddressResponse = Nothing
                oAddresses = Nothing
                iAddressKey = Nothing

            End Try
            Return oAddressCollection
        End SyncLock

    End Function

#End Region

    Public Overrides Function FindControlSearch(ByVal oSearchCriteria As FindControlCriteriaCollection,
                                            ByVal v_iFindControlKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As System.Xml.XmlElement

        SyncLock oLock

            Dim oFindControlSearchRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindControlSearchQuery
            Dim oFindControlSearchResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindControlSearchQueryResponse
            Dim sSearchCriteriaHash As String = String.Empty
            Dim x As Integer = 0
            Dim sbLogMessage As StringBuilder
            Dim oMatches As Xml.XmlElement = Nothing

            Try

                oFindControlSearchRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindControlSearchQuery
                oFindControlSearchResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindControlSearchQueryResponse
                'oSAM = InitializeCoreServiceMethod()
                sbLogMessage = New StringBuilder

                With oFindControlSearchRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iFindControlKey > 0 Then
                        .FindControlKey = v_iFindControlKey
                    End If

                    If oSearchCriteria Is Nothing Then
                        Throw New ArgumentNullException("SearchCriteria")
                    ElseIf oSearchCriteria.Count <= 0 Then
                        Throw New ArgumentException("At least one Search Criteria is required", "SearchCriteria")
                    Else

                        Dim temp As New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSearchCriteriaType)()

                        sSearchCriteriaHash = v_iFindControlKey
                        ' Start with empty array (optional; we overwrite at the end anyway)
                        .SearchCriteria = Array.Empty(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSearchCriteriaType)()

                        For i As Integer = 0 To oSearchCriteria.Count - 1
                            If Not oSearchCriteria(i).Duplicate Then

                                Dim sc As New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSearchCriteriaType() With {
                                                .ObjectName = oSearchCriteria(i).ObjectName,
                                                .PropertyName = oSearchCriteria(i).PropertyName,
                                                .Value = oSearchCriteria(i).Value
                                            }

                                sSearchCriteriaHash &= oSearchCriteria(i).Value
                                temp.Add(sc)

                            End If
                        Next

                        ' Assign the final immutable array
                        .SearchCriteria = temp.ToArray()

                    End If

                End With


                sSearchCriteriaHash = sSearchCriteriaHash.GetHashCode()

                If Current.Cache(PROVIDER_FINDCONTROLSEARCH & sSearchCriteriaHash) Is Nothing Then

                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.FindControlSearch, oFindControlSearchRequest)
                        oFindControlSearchResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.FindControlSearchQueryResponse)(result)
                    End Using


                    With oFindControlSearchResponse

                        If .Errors IsNot Nothing Then

                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)

                        Else

                            oMatches = .Matches
                            Current.Cache.Insert(PROVIDER_FINDCONTROLSEARCH & sSearchCriteriaHash, oMatches, Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)

                        End If

                    End With

                Else
                    oMatches = CType(Current.Cache(PROVIDER_FINDCONTROLSEARCH & sSearchCriteriaHash), Xml.XmlElement)
                End If
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindControlSearch executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("oSearchCriteria = " & oSearchCriteria.Count.ToString & vbCrLf)

                    sbLogMessage.AppendLine("v_iFindControlKey = " & v_iFindControlKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    ' for counting the return records
                    Dim oXMLNodes As XmlNodeList = oMatches.SelectNodes("Row")

                    sbLogMessage.AppendLine("Returned " & oXMLNodes.Count.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oFindControlSearchRequest = Nothing
                oFindControlSearchResponse = Nothing
            End Try

            Return oMatches

        End SyncLock

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindCoverNoteBooks(ByRef v_oCoverNote As CoverNote,
                                       Optional ByVal v_sBranchCode As String = Nothing) As CoverNoteCollection

        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oFindCoverNoteBooksRequest As FindCoverNoteBooksRequestType
            'Dim oFindCoverNoteBooksResponse As FindCoverNoteBooksResponseType
            Dim oCoverNoteCollection As CoverNoteCollection
            Dim oCoverNote As CoverNote
            Dim sbLogMessage As StringBuilder
            Try
                'oSAM = InitializeCoreServiceMethod()
                'oFindCoverNoteBooksRequest = New FindCoverNoteBooksRequestType
                'oFindCoverNoteBooksResponse = New FindCoverNoteBooksResponseType
                oCoverNoteCollection = New CoverNoteCollection
                sbLogMessage = New StringBuilder
                'With oFindCoverNoteBooksRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    'if the passed parameter v_sBranchCode is empty
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode
                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode
                '    End If
                '    If v_oCoverNote.AgentKey > 0 Then
                '        .AgentKey = v_oCoverNote.AgentKey
                '        .AgentKeySpecified = True
                '    Else
                '        .AgentKeySpecified = False
                '    End If
                '    If v_oCoverNote.AssignedDate <> Date.MinValue Then
                '        .AssignedDate = v_oCoverNote.AssignedDate
                '        .AssignedDateSpecified = True
                '    Else
                '        .AssignedDateSpecified = False
                '    End If
                '    .BookNumber = v_oCoverNote.BookNumber
                '    .CoverNoteBranchCode = v_oCoverNote.CoverNoteBranchCode
                '    .CoverNoteStatusCode = v_oCoverNote.CoverNoteBookStatusCode
                '    If v_oCoverNote.StartNumber <> 0 Then
                '        .StartNumber = v_oCoverNote.StartNumber
                '        .StartNumberSpecified = True
                '    Else
                '        .StartNumberSpecified = False
                '    End If
                '    If v_oCoverNote.EndNumber <> 0 Then
                '        .EndNumber = v_oCoverNote.EndNumber
                '        .EndNumberSpecified = True
                '    Else
                '        .EndNumberSpecified = False
                '    End If

                '    If v_oCoverNote.LastUpdated <> Date.MinValue Then
                '        .LastUpdated = v_oCoverNote.LastUpdated
                '        .LastUpdatedSpecified = True
                '    Else
                '        .LastUpdatedSpecified = False
                '    End If

                '    .PolicyNumber = v_oCoverNote.PolicyNumber

                '    If v_oCoverNote.MaxRowsToFetch > 0 Then
                '        .MaxRowsToFetch = v_oCoverNote.MaxRowsToFetch
                '        .MaxRowsToFetchSpecified = True
                '    Else
                '        .MaxRowsToFetchSpecified = False
                '    End If

                'End With
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ' oFindCoverNoteBooksResponse = oSAM.FindCoverNoteBooks(oFindCoverNoteBooksRequest)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                'With oFindCoverNoteBooksResponse

                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else

                '        If .FindCoverNoteBooks IsNot Nothing AndAlso .FindCoverNoteBooks.Count > 0 Then
                '            For Each oFindCoverNoteBooksRow As BaseFindCoverNoteBooksResponseTypeRow In .FindCoverNoteBooks
                '                oCoverNote = New CoverNote()
                '                oCoverNote.CoverNoteBookKey = oFindCoverNoteBooksRow.CoverNoteBookKey
                '                oCoverNote.BookNumber = oFindCoverNoteBooksRow.BookNumber
                '                oCoverNote.StartNumber = oFindCoverNoteBooksRow.StartNumber
                '                oCoverNote.EndNumber = oFindCoverNoteBooksRow.EndNumber
                '                oCoverNote.AgentKey = oFindCoverNoteBooksRow.AgentKey
                '                oCoverNote.AgentName = oFindCoverNoteBooksRow.AgentName
                '                oCoverNote.CoverNoteStatusKey = oFindCoverNoteBooksRow.CoverNoteStatusKey
                '                oCoverNote.CoverNoteStatusDescription = oFindCoverNoteBooksRow.CoverNoteStatusDescription
                '                oCoverNote.CoverNoteBranchKey = oFindCoverNoteBooksRow.CoverNoteBranchKey
                '                oCoverNote.CoverNoteBranchDescription = oFindCoverNoteBooksRow.CoverNoteBranchDescription
                '                oCoverNote.LastUpdated = oFindCoverNoteBooksRow.LastUpdated
                '                oCoverNote.DateCreated = oFindCoverNoteBooksRow.DateCreated
                '                oCoverNote.EffectiveDate = oFindCoverNoteBooksRow.EffectiveDate
                '                oCoverNoteCollection.Add(oCoverNote)
                '            Next
                '        End If
                '    End If

                'End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindCoverNoteBooks executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oFindCoverNoteBooksRequest = Nothing
                'oFindCoverNoteBooksResponse = Nothing
            End Try


            Return oCoverNoteCollection

        End SyncLock


    End Function

    ''' <summary>
    ''' This Function is used to Get FindDMEDocuments details of a DME. 
    ''' This method takes ?v_oDMESearchCriteria?,'v_sBranchCode' as input and returns an folder and document collection 
    ''' Class.
    ''' </summary>
    ''' <param name="v_oDMESearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindDMEDocuments(ByVal v_oDMESearchCriteria As DMESearchCriteria,
                                        Optional ByVal v_sBranchCode As String = Nothing) As DME
        SyncLock oLock
            Dim oFindDMEDocumentsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindDMEDocumentsQuery
            Dim oFindDMEDocumentsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindDMEDocumentsQueryResponse
            Dim oListOfDME As DME
            Dim oSubFolder As NexusProvider.SubFolder
            Dim oDocumentList As NexusProvider.DocumentList
            Dim sbLogMessage As StringBuilder

            Try
                oFindDMEDocumentsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindDMEDocumentsQuery
                oFindDMEDocumentsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindDMEDocumentsQueryResponse
                oListOfDME = New DME
                oSubFolder = New NexusProvider.SubFolder
                oDocumentList = New NexusProvider.DocumentList
                sbLogMessage = New StringBuilder


                With oFindDMEDocumentsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_oDMESearchCriteria.ParentNum > 0 Then
                        'use the passed parameter ParentNum
                        .ParentNum = v_oDMESearchCriteria.ParentNum
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.FolderName) Then
                        'use the passed parameter FolderName
                        .FolderName = v_oDMESearchCriteria.FolderName
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.PartyCode) Then
                        'use the passed parameter PartyCode
                        .PartyCode = v_oDMESearchCriteria.PartyCode
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.PartyName) Then
                        'use the passed parameter PartyName
                        .PartyName = v_oDMESearchCriteria.PartyName
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.PolicyNumber) Then
                        'use the passed parameter PolicyNumber
                        .PolicyNumber = v_oDMESearchCriteria.PolicyNumber
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.ClaimNumber) Then
                        'use the passed parameter ClaimNumber
                        .ClaimNumber = v_oDMESearchCriteria.ClaimNumber
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.RiskIndex) Then
                        'use the passed parameter RiskIndex
                        .RiskIndex = v_oDMESearchCriteria.RiskIndex
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.PostCode) Then
                        'use the passed parameter PostCode
                        .PostCode = v_oDMESearchCriteria.PostCode
                    End If

                    If Not String.IsNullOrEmpty(v_oDMESearchCriteria.DocumentDescription) Then
                        'use the passed parameter DocumentDescription
                        .DocumentDescription = v_oDMESearchCriteria.DocumentDescription
                    End If

                    .IncludeFiles = v_oDMESearchCriteria.IncludeFiles

                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.FindDMEDocuments, oFindDMEDocumentsRequest)
                    oFindDMEDocumentsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.FindDMEDocumentsQueryResponse)(result)

                End Using

                oListOfDME = New DME()

                With oFindDMEDocumentsResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else


                    If oFindDMEDocumentsResponse.Folders IsNot Nothing Then
                        For iCount As Integer = 0 To oFindDMEDocumentsResponse.Folders.Count - 1
                            oSubFolder = New SubFolder()
                            oSubFolder.Name = oFindDMEDocumentsResponse.Folders(iCount).Name
                            oSubFolder.CreateDate = oFindDMEDocumentsResponse.Folders(iCount).CreateDate
                            oSubFolder.ExternalCode = oFindDMEDocumentsResponse.Folders(iCount).ExternalCode
                            oSubFolder.FolderLevel = oFindDMEDocumentsResponse.Folders(iCount).FolderLevel
                            oSubFolder.FolderNum = oFindDMEDocumentsResponse.Folders(iCount).FolderNum
                            oSubFolder.ParentNum = oFindDMEDocumentsResponse.Folders(iCount).ParentNum
                            oListOfDME.SubFolder.Add(oSubFolder)
                        Next
                    End If


                    If oFindDMEDocumentsResponse.Documents IsNot Nothing Then
                        For jCount As Integer = 0 To oFindDMEDocumentsResponse.Documents.Count - 1
                            oDocumentList = New DocumentList()
                            oDocumentList.CreateDate = oFindDMEDocumentsResponse.Documents(jCount).CreateDate
                            oDocumentList.DocDescription = oFindDMEDocumentsResponse.Documents(jCount).DocDescription
                            oDocumentList.DocNum = oFindDMEDocumentsResponse.Documents(jCount).DocNum
                            oDocumentList.DocumentType = oFindDMEDocumentsResponse.Documents(jCount).DocumentType
                            oDocumentList.FolderNum = oFindDMEDocumentsResponse.Documents(jCount).FolderNum
                            oDocumentList.FolderPath = oFindDMEDocumentsResponse.Documents(jCount).FolderPath
                            oDocumentList.UploadedBy = oFindDMEDocumentsResponse.Documents(jCount).UploadedBy
                            oDocumentList.Category = oFindDMEDocumentsResponse.Documents(jCount).Category
                            oDocumentList.SubCategory = oFindDMEDocumentsResponse.Documents(jCount).SubCategory
                            oListOfDME.DocumentList.Add(oDocumentList)
                        Next
                    End If
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindDMEDocuments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oDMESearchCriteria = " & v_oDMESearchCriteria.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oFindDMEDocumentsRequest = Nothing
                oFindDMEDocumentsResponse = Nothing
            End Try

            Return oListOfDME

        End SyncLock

    End Function

    Public Overrides Function FindUsers(ByVal v_oFindUserSearchCriteria As FindUsersSearchCriteria,
                    Optional ByVal v_sBranchCode As String = Nothing) As UserCollection
        SyncLock oLock

            Dim oFindUsersRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindUsersQuery
            Dim oFindUsersResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.FindUsersQueryResponse
            Dim oUsers As UserCollection = Nothing
            Dim oUser As User = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oFindUsersRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindUsersQuery
                oFindUsersResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.FindUsersQueryResponse
                sbLogMessage = New StringBuilder


                With oFindUsersRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If Not String.IsNullOrEmpty(v_oFindUserSearchCriteria.FullName) Then
                        .FullName = v_oFindUserSearchCriteria.FullName
                    Else
                        .FullName = Nothing
                    End If

                    If Not String.IsNullOrEmpty(v_oFindUserSearchCriteria.UserName) Then
                        .UserName = v_oFindUserSearchCriteria.UserName
                    Else
                        .UserName = Nothing
                    End If

                    If v_oFindUserSearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oFindUserSearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.FindUser, oFindUsersRequest)
                    oFindUsersResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.FindUsersQueryResponse)(result)
                End Using

                With oFindUsersResponse.FindUsersResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'End If

                    If .Users IsNot Nothing AndAlso .Users.Count > 0 Then


                        oUsers = New UserCollection()

                        For Each oBaseUser As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseFindUsersResponseTypeRow In .Users
                            oUser = New User()
                            With oUser
                                .UserId = oBaseUser.UserId
                                .UserName = oBaseUser.UserName
                                .FullName = oBaseUser.FullName
                                .EffectiveDate = oBaseUser.EffectiveDate
                            End With
                            oUsers.Add(oUser)
                        Next
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("FindUsers executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)
                    sbLogMessage.AppendLine(v_oFindUserSearchCriteria.Print().Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    'sbLogMessage.AppendLine(oUsers.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oFindUsersRequest = Nothing
                oFindUsersResponse = Nothing
            End Try


            Return oUsers
        End SyncLock
    End Function

    ''' <summary>
    ''' Requests document via SAM, saves the file to the specified location and returns this location
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iInsuranceFolderKey"></param>
    ''' <param name="v_sDocumentCode"></param>
    ''' <param name="v_oDocumentType"></param>
    ''' <param name="v_sDocumentExtractionDirectory"></param>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_bSpoolDocumentOnly"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <param name="v_iMode"></param>
    ''' <returns>String giving the location of the saved file</returns>
    ''' <remarks></remarks>
    Public Overrides Function GenerateDocument(ByVal v_iPartyKey As Integer,
                                            ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iInsuranceFolderKey As Integer,
                                            ByVal v_sDocumentCode As String,
                                            ByVal v_oDocumentType As DocumentType,
                                            ByVal v_sDocumentExtractionPath As String,
                                            Optional ByVal v_iClaimKey As Integer = 0,
                                            Optional ByVal v_bSpoolDocumentOnly As Boolean = False,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sDocumentRef As String = Nothing,
                                            Optional ByVal v_bSkipArchiveonEdit As Boolean = False,
                                            Optional ByVal v_iMode As Integer = 4,
                                            Optional ByVal bIsSuppressArchive As Boolean = False,
                                            Optional ByVal sDocumentName As String = Nothing) As String
        SyncLock oLock

            Dim oGenerateDocumentRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GenerateDocumentCommand
            Dim oGenerateDocumentResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GenerateDocumentCommandResponse
            Dim sDirectoryName As String = Left(v_sDocumentExtractionPath, v_sDocumentExtractionPath.LastIndexOf("\"))

            Dim sbLogMessage As StringBuilder

            Try
                oGenerateDocumentRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GenerateDocumentCommand
                oGenerateDocumentResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GenerateDocumentCommandResponse
                sbLogMessage = New StringBuilder


                With oGenerateDocumentRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If String.IsNullOrEmpty(v_sDocumentCode) Then
                        Throw New ArgumentNullException("DocumentCode")
                    Else
                        .DocumentTemplateCode = v_sDocumentCode
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    End If

                    If v_iInsuranceFolderKey > 0 Then
                        .InsuranceFolderKey = v_iInsuranceFolderKey
                    End If

                    If String.IsNullOrEmpty(v_sDocumentExtractionPath) Then
                        Throw New ArgumentNullException("DocumentExtractionPath")
                    Else
                        If Not IO.Directory.Exists(sDirectoryName) Then
                            'create the directory that the file will be written to
                            IO.Directory.CreateDirectory(sDirectoryName)
                        End If

                        .SpoolDocumentOnly = v_bSpoolDocumentOnly
                        .SpoolDocumentOnlySpecified = v_bSpoolDocumentOnly
                        .IsSuppressArchive = bIsSuppressArchive
                    End If

                    Select Case v_oDocumentType
                        Case DocumentType.None
                            Throw New ArgumentException("Can not be DocumentType.None", "DocumentType")
                        Case DocumentType.HTML
                            'changes as of Pure 2.01 mean that
                            Throw New ArgumentException("Can not be DocumentType.HTML, no longer supported", "DocumentType")
                        Case DocumentType.PDF
                            .OutputAsPDF = True
                            .OutputAsHTML = False
                        Case DocumentType.DOCX
                            .OutputAsPDF = False
                            .OutputAsHTML = False
                        Case DocumentType.TXT
                            .OutputAsHTML = False
                            .OutputAsPDF = False
                            .OutputAsTXT = True
                        Case DocumentType.XML
                            .OutputAsPDF = False
                            .OutputAsHTML = False
                    End Select

                    'Sure what these represent, but they've always be set to 4 and nothing
                    .Mode = v_iMode
                    .ParameterXML = Nothing
                    If v_iClaimKey > 0 Then
                        .ClaimKey = v_iClaimKey
                        .ClaimKeySpecified = True
                    Else
                        .ClaimKeySpecified = False
                    End If

                    If String.IsNullOrEmpty(v_sDocumentRef) = False Then
                        .DocumentRef = v_sDocumentRef
                    End If
                    .SkipArchiveOnEdit = v_bSkipArchiveonEdit

                    If (v_sDocumentExtractionPath.Length > 0 AndAlso (sDocumentName Is Nothing OrElse (sDocumentName IsNot Nothing AndAlso Not sDocumentName.ToUpper.Contains("RENEWALINVITE")))) Then
                        Dim nStartIndex As Integer = 0
                        Dim nEndIndex As Integer = 0
                        Dim nSubStrLength As Integer = 0
                        nStartIndex = v_sDocumentExtractionPath.LastIndexOf("\") + 1
                        nEndIndex = IIf(v_sDocumentExtractionPath.IndexOf(".") > -1, v_sDocumentExtractionPath.IndexOf("."), v_sDocumentExtractionPath.Length)
                        nSubStrLength = nEndIndex - nStartIndex
                        .ArchiveDocFileName = v_sDocumentExtractionPath.Substring(nStartIndex, nSubStrLength)
                    End If
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)

                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.GenerateDocument, oGenerateDocumentRequest)
                    oGenerateDocumentResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GenerateDocumentCommandResponse)(result)
                End Using

                With oGenerateDocumentResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'write the file, returned as a byte array in SpooledZipFile to disk
                        Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(v_sDocumentExtractionPath)
                        fsOutputFile.Write(.SpooledZipFile, 0, .SpooledZipFile.Length)
                        fsOutputFile.Close()

                        ' Write split documents to disk and add to document cache
                        If .SplitDocuments IsNot Nothing AndAlso .SplitDocuments.Count > 0 Then
                            Dim sSplitDir As String = IO.Path.GetDirectoryName(v_sDocumentExtractionPath)
                            Dim sParentNameNoExt As String = IO.Path.GetFileNameWithoutExtension(v_sDocumentExtractionPath)
                            Dim sParentExt As String = IO.Path.GetExtension(v_sDocumentExtractionPath)
                            Dim oCachedCollection As DocumentDefaultsCollection = Nothing
                            Dim sCacheKey As String = String.Empty
                            If Current.Session(Nexus.Constants.CNInsuranceFileKey) IsNot Nothing Then
                                sCacheKey = CType(Current.Cache.Item("hdnKeyDocMan" & Current.Session(Nexus.Constants.CNInsuranceFileKey).ToString()), String)
                            End If
                            If Not String.IsNullOrEmpty(sCacheKey) Then
                                oCachedCollection = CType(Current.Cache.Item(sCacheKey), DocumentDefaultsCollection)
                            End If
                            Dim iSplitIndex As Integer = 1
                            For Each oSplitDoc As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGenerateDocumentV2ResponseTypeDocument In .SplitDocuments
                                If oSplitDoc.SpooledZipFile IsNot Nothing AndAlso oSplitDoc.SpooledZipFile.Length > 0 Then
                                    ' Name split doc using parent naming convention + split index + same extension
                                    Dim sSplitDocName As String = sParentNameNoExt & "_" & oSplitDoc.DocumentCode & "_" & iSplitIndex & sParentExt
                                    Dim sSplitPath As String = IO.Path.Combine(sSplitDir, sSplitDocName)
                                    Dim fsSplit As IO.FileStream = IO.File.OpenWrite(sSplitPath)
                                    fsSplit.Write(oSplitDoc.SpooledZipFile, 0, oSplitDoc.SpooledZipFile.Length)
                                    fsSplit.Close()
                                    If oCachedCollection IsNot Nothing Then
                                        Dim oSplitDefaults As New DocumentDefaults()
                                        oSplitDefaults.DocumentName = sSplitDocName
                                        oSplitDefaults.documentTemplateCode = v_sDocumentCode
                                        oSplitDefaults.documentTemplateDescription = oSplitDoc.DocumentDescription
                                        oSplitDefaults.FileLocation = sSplitPath
                                        oSplitDefaults.FileType = If(sParentExt.TrimStart(".").ToUpper() = "PDF", "PDF", "WORD")
                                        oSplitDefaults.Key = oCachedCollection.Count
                                        oCachedCollection.Add(oSplitDefaults)
                                    End If
                                    iSplitIndex += 1
                                End If
                            Next
                            If oCachedCollection IsNot Nothing AndAlso Not String.IsNullOrEmpty(sCacheKey) Then
                                Current.Cache.Insert(sCacheKey, oCachedCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(25))
                            End If
                        End If
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GenerateDocument executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFolderKey = " & v_iInsuranceFolderKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDocumentCode = " & v_sDocumentCode.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oDocumentType = " & v_oDocumentType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDocumentExtractionDirectory = " & v_sDocumentExtractionPath.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & v_sDocumentExtractionPath & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGenerateDocumentRequest = Nothing
                oGenerateDocumentResponse = Nothing
                sDirectoryName = Nothing
            End Try


            Return v_sDocumentExtractionPath 'todo - should be a sub, we're just passing back an input

        End SyncLock

    End Function

    ''' <summary>
    ''' To get agent setting for an agent key
    ''' </summary>
    ''' <param name="v_iAgentKey">agent key</param>
    ''' <param name="v_sBranchCode">branch code(optional)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetAgentSettings(ByVal v_iAgentKey As Integer,
                                                                Optional ByVal v_sBranchCode As String = Nothing) As AgentSettings

        SyncLock oLock

            Dim oGetAgentSettingsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAgentSettingsQuery 'Request Type
            Dim oGetAgentSettingsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAgentSettingsQueryResponse 'Response Type
            Dim oAgentSettings As AgentSettings = Nothing
            Dim oUsers As UserCollection = Nothing
            Dim oUser As User = Nothing
            Dim oContacts As ContactCollection = Nothing
            Dim oContact As Contact = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetAgentSettingsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAgentSettingsQuery
                oGetAgentSettingsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAgentSettingsQueryResponse
                sbLogMessage = New StringBuilder


                With oGetAgentSettingsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_iAgentKey is empty
                    If v_iAgentKey > 0 Then
                        .AgentKey = v_iAgentKey
                        .AgentKeySpecified = True
                    Else
                        Throw New ArgumentNullException("v_iAgentKey")
                    End If

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetAgentSettings, oGetAgentSettingsRequest)
                    oGetAgentSettingsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAgentSettingsQueryResponse)(result)
                End Using

                With oGetAgentSettingsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oAgentSettings = New AgentSettings()

                        oAgentSettings.AllowConsolidatedCommission = .AllowConsolidatedCommission
                        oAgentSettings.AlternateReferenceForEachTransaction = .AlternateReferenceForEachTransaction
                        oAgentSettings.IsAlternateReferenceMandatory = .AlternateReferenceMandatory
                        oAgentSettings.CanMakeLiveBankGuarantee = .CanMakeLiveBankGuarantee
                        oAgentSettings.CanMakeLiveCashDeposit = .CanMakeLiveCashDeposit
                        oAgentSettings.CanMakeLiveInstalments = .CanMakeLiveInstalments
                        oAgentSettings.CanMakeLiveInvoice = .CanMakeLiveInvoice
                        oAgentSettings.CanMakeLivePaynow = .CanMakeLivePaynow
                        oAgentSettings.DaysAllowed = .DaysAllowed
                        oAgentSettings.IsDomiciledForTax = .DomiciledForTax
                        oAgentSettings.ExpectedDailyPremium = .ExpectedDailyPremium
                        oAgentSettings.FloatBalanceLimit = .FloatBalanceLimit
                        oAgentSettings.IsFloatBalanceAccount = .IsFloatBalanceAccount
                        oAgentSettings.IsOverdraftAccount = .IsOverdraftAccount
                        oAgentSettings.IsPrepaymentAccount = .IsPrepaymentAccount
                        oAgentSettings.IsStandardAccount = .IsStandardAccount
                        oAgentSettings.OverdraftExpiry = .OverdraftExpiry
                        oAgentSettings.OverdraftLimit = .OverdraftLimit
                        oAgentSettings.UseOverrideCommissionRate = .UseOverrideCommissionRate
                        oAgentSettings.AgencyCancellationDate = .PartyAgentDateCancelled
                        oAgentSettings.AgentKey = v_iAgentKey
                        ' Agent allowed to work on below Branchs
                        ' Response item contains branch collection
                        If .Branches IsNot Nothing Then
                            For Each AgentBranch As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetAgentSettingsResponseTypeBranchesRow In .Branches
                                Dim oAgentBranches As New NexusProvider.Branch()
                                oAgentBranches.Code = AgentBranch.SourceCode
                                oAgentBranches.Description = AgentBranch.Description
                                oAgentBranches.BranchKey = AgentBranch.SourceId
                                oAgentSettings.AgentBranchCollection.Add(oAgentBranches)
                            Next
                        End If
                        oAgentSettings.CorrespondenceType = .CorrespondenceType
                        oAgentSettings.IsReceiveClientCorrespondence = .IsReceiveClientCorrespondence
                    End If

                    If .Users IsNot Nothing Then


                        oUsers = New UserCollection()
                        For Each oBaseUser As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUserDetailsType In .Users
                            'If oBaseUser.EffectiveDate < Date.Now Then
                            oUser = New User()
                            With oUser
                                .UserKey = oBaseUser.UserKey
                                .UserName = oBaseUser.UserName
                                .FullName = oBaseUser.FullName
                                .EmailAddress = oBaseUser.EmailAddress
                                '.EffectiveDate = oBaseUser.EffectiveDate

                            End With
                            oUsers.Add(oUser)
                            'End If
                        Next
                    End If


                    oAgentSettings.AssociatedUsers = oUsers
                    If .Contacts IsNot Nothing Then
                        oContacts = New ContactCollection()
                        For Each oBaseContact As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType In .Contacts
                            'If oBaseUser.EffectiveDate < Date.Now Then
                            oContact = New Contact()
                            With oContact
                                .AreaCode = oBaseContact.AreaCode
                                .ContactType = oBaseContact.ContactTypeCode
                                .Description = oBaseContact.Description
                                .Extension = oBaseContact.Extension
                                .Number = oBaseContact.Description
                                .OtherContactTypeCode = oBaseContact.OtherContactTypeCode
                            End With
                            oContacts.Add(oContact)
                            'End If
                        Next
                    End If
                    oAgentSettings.Contacts = oContacts
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAgentSettings executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iAgentKey = " & v_iAgentKey.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oAgentSettings.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetAgentSettingsRequest = Nothing
                oGetAgentSettingsResponse = Nothing
            End Try


            Return oAgentSettings

        End SyncLock

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                            Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oGetCoverNoteBookRequest As GetCoverNoteBookRequestType
            'Dim oGetCoverNoteBookResponse As GetCoverNoteBookResponseType
            Dim oCoverNoteBookType As CoverNoteBookTypeCollection
            Dim oProduct As Product
            Dim oCoverNoteSheet As CoverNoteSheetType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oGetCoverNoteBookRequest = New GetCoverNoteBookRequestType
                'oGetCoverNoteBookResponse = New GetCoverNoteBookResponseType
                oCoverNoteBookType = New CoverNoteBookTypeCollection
                sbLogMessage = New StringBuilder


                'With oGetCoverNoteBookRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode
                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode

                '    End If
                '    .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                'End With


                Using trace As New Tracer(Category.Trace)
                    'oGetCoverNoteBookResponse = oSAM.GetCoverNoteBook(oGetCoverNoteBookRequest)
                End Using

                'With oGetCoverNoteBookResponse

                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else
                '        r_oCoverNote.BookNumber = .BookNumber
                '        r_oCoverNote.StartNumber = .StartNumber
                '        r_oCoverNote.EndNumber = .EndNumber
                '        r_oCoverNote.EffectiveDate = .EffectiveDate
                '        r_oCoverNote.AgentKey = .AgentKey
                '        r_oCoverNote.AgentName = .AgentName
                '        r_oCoverNote.CoverNoteBranchKey = .CoverNoteBranchKey
                '        r_oCoverNote.CoverNoteBranchCode = .CoverNoteBranchCode
                '        r_oCoverNote.CoverNoteBookStatusKey = .CoverNoteBookStatusKey
                '        r_oCoverNote.CoverNoteBookStatusCode = .CoverNoteBookStatusCode
                '        r_oCoverNote.DateCreated = .DateCreated
                '        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp

                '        If .CoverNoteBookProducts IsNot Nothing AndAlso .CoverNoteBookProducts.Count > 0 Then
                '            For Each oCoverNoteProducts As BaseGetCoverNoteBookResponseTypeRow In .CoverNoteBookProducts
                '                oProduct = New Product
                '                oProduct.ProductKey = oCoverNoteProducts.ProductKey
                '                oProduct.ProductCode = oCoverNoteProducts.ProductCode
                '                oProduct.Chosen = oCoverNoteProducts.Chosen
                '                oProduct.Description = oCoverNoteProducts.Description

                '                r_oCoverNote.CoverNoteBookProducts.Add(oProduct)
                '            Next
                '        End If
                '        If .CoverNoteSheets IsNot Nothing AndAlso .CoverNoteSheets.Count > 0 Then

                '            For Each oCoverNoteSheets As BaseGetCoverNoteBookResponseTypeRow1 In .CoverNoteSheets
                '                oCoverNoteSheet = New CoverNoteSheetType
                '                oCoverNoteSheet.CoverNoteSheetKey = oCoverNoteSheets.CoverNoteSheetKey
                '                oCoverNoteSheet.CoverNoteSheetNumber = oCoverNoteSheets.CoverNoteSheetNumber
                '                oCoverNoteSheet.CustomerName = oCoverNoteSheets.CustomerName
                '                oCoverNoteSheet.CoverNoteSheetStatusKey = oCoverNoteSheets.CoverNoteSheetStatusKey
                '                oCoverNoteSheet.CoverNoteSheetStatusCode = oCoverNoteSheets.CoverNoteSheetStatusCode
                '                oCoverNoteSheet.CoverNoteSheetStatusDescription = oCoverNoteSheets.CoverNoteSheetStatusDescription
                '                oCoverNoteSheet.PolicyNumber = oCoverNoteSheets.PolicyNumber
                '                oCoverNoteSheet.BranchName = oCoverNoteSheets.BranchName
                '                oCoverNoteSheet.AgentName = oCoverNoteSheets.AgentName
                '                oCoverNoteSheet.DateImported = oCoverNoteSheets.DateImported

                '                r_oCoverNote.CoverNoteSheets.Add(oCoverNoteSheet)
                '            Next
                '        End If

                '        oCoverNoteBookType.Add(r_oCoverNote)

                '    End If

                'End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCoverNoteBook executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    'to complete
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oGetCoverNoteBookRequest = Nothing
                'oGetCoverNoteBookResponse = Nothing
            End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oGetCoverNoteSheetRequest As GetCoverNoteSheetRequestType
            'Dim oGetCovernoteSheetResponse As GetCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oGetCoverNoteSheetRequest = New GetCoverNoteSheetRequestType
                'oGetCovernoteSheetResponse = New GetCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                ''With oGetCoverNoteSheetRequest
                '.LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode

                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode
                '    End If
                '    'Checking the CoverNoteBookKey
                '    If r_oCoverNote.CoverNoteBookKey > 0 Then
                '        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                '    Else
                '        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                '    End If

                '    'Checking the CoverNoteSheetNumber

                '    .CoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber

                'End With


                Using trace As New Tracer(Category.Trace)
                    'oGetCovernoteSheetResponse = oSAM.GetCoverNoteSheet(oGetCoverNoteSheetRequest)
                End Using

                'With oGetCovernoteSheetResponse
                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else
                '        r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetKey = .CoverNoteSheetKey
                '        r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber = .CoverNoteSheetNumber
                '        r_oCoverNote.CoverNoteSheets(0).PolicyNumber = .InsuranceRef
                '        r_oCoverNote.InsuranceFileDetails.InsuranceFileCnt = .InsuranceFileCnt
                '        r_oCoverNote.AssignedDate = .AssignedDate
                '        r_oCoverNote.CoverNoteStausKey = .CoverNoteStatusKey
                '        r_oCoverNote.Code = .Code
                '        r_oCoverNote.Description = .Description
                '        r_oCoverNote.Comments = .Comments
                '        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp

                '    End If
                'End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetNumber" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oGetCoverNoteSheetRequest = Nothing
                'oGetCovernoteSheetResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' Get the CurrencyToCurrency Conversion Rate
    ''' </summary>
    ''' <param name="sCurrencyCodeFrom"></param>
    ''' <param name="sCurrencyCodeTo"></param>
    ''' <param name="dCurrencyAmountUnRounded"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ConvertCurrencyToBase(ByVal v_oParameters As ConvertCurrencytoBaseParameters,
                                                     Optional ByVal v_sBranchCode As String = Nothing) As ConvertCurrencytoBaseResponseParameters
        SyncLock oLock
            Dim oConvertCurrencyToBaseRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ConvertCurrencytoBaseCommand
            Dim oConvertCurrencyToBaseResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ConvertCurrencytoBaseCommandResponse
            Dim oResult As ConvertCurrencytoBaseResponseParameters
            Dim sbLogMessage As StringBuilder

            Try
                oConvertCurrencyToBaseRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ConvertCurrencytoBaseCommand
                oConvertCurrencyToBaseResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ConvertCurrencytoBaseCommandResponse
                oResult = New ConvertCurrencytoBaseResponseParameters
                sbLogMessage = New StringBuilder

                With oConvertCurrencyToBaseRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If

                    .CurrencyID = v_oParameters.CurrencyID
                    .CompanyID = v_oParameters.CompanyID
                    .BaseAmount = v_oParameters.BaseAmount
                    .CurrencyAmount = v_oParameters.CurrencyAmount
                    .ConversionDate = v_oParameters.ConversionDate
                    .ConversionRate = v_oParameters.ConversionRate
                    .IsMultiplier = v_oParameters.IsMultiplier
                    .Rounded = v_oParameters.Rounded
                    .BaseRoundingDifference = v_oParameters.BaseRoundingDifference
                    .CurrencyRoundingDifference = v_oParameters.CurrencyRoundingDifference
                    .FormattedBase = v_oParameters.FormattedBase
                    .FormattedCurrency = v_oParameters.FormattedCurrency
                    .Euro = v_oParameters.Euro
                    .EuroAmount = v_oParameters.EuroAmount
                    .EuroCCyXrate = v_oParameters.EuroCCyXrate
                    .EuroBaseXRate = v_oParameters.EuroBaseXRate
                    .CcyAmountUnRounded = v_oParameters.CcyAmountUnRounded
                    .BaseAmountUnRounded = v_oParameters.BaseAmountUnRounded
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.ConvertCurrencyToBase, oConvertCurrencyToBaseRequest)
                    oConvertCurrencyToBaseResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ConvertCurrencytoBaseCommandResponse)(result)
                End Using

                With oConvertCurrencyToBaseResponse
                    oResult.Result = .Result
                    oResult.BaseAmount = .BaseAmount
                    oResult.BaseAmountUnRounded = .BaseAmountUnRounded
                    oResult.ConversionDate = .ConversionDate
                    oResult.ConversionRate = .ConversionRate
                    oResult.IsMultiplier = .IsMultiplier
                    oResult.Rounded = .Rounded
                    oResult.BaseRoundingDifference = .BaseRoundingDifference
                    oResult.CurrencyRoundingDifference = .CurrencyRoundingDifference
                    oResult.FormattedBase = .FormattedBase
                    oResult.FormattedCurrency = .FormattedCurrency
                    oResult.Euro = .Euro
                    oResult.EuroAmount = .EuroAmount
                    oResult.EuroCCyXrate = .EuroCCyXrate
                    oResult.EuroBaseXRate = .EuroBaseXRate
                    oResult.CcyAmountUnRounded = .CcyAmountUnRounded
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ConvertCurrencyToBase executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oParameters.CurrencyID = " & v_oParameters.CurrencyID.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oParameters.CompanyID = " & v_oParameters.CompanyID.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oParameters.CurrencyAmount = " & v_oParameters.CurrencyAmount.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                Return oResult
            Catch ex As Exception
                Throw
            Finally
                oConvertCurrencyToBaseRequest = Nothing
                oConvertCurrencyToBaseResponse = Nothing
            End Try

            Return oResult
        End SyncLock
    End Function

    Public Overrides Function GetCurrencyToCurrencyExchangeRate(ByVal sCurrencyCodeFrom As String,
                                                      ByVal sCurrencyCodeTo As String,
                                                      ByVal dCurrencyAmountUnRounded As Decimal,
                                         Optional ByVal v_sBranchCode As String = Nothing) As Currency
        SyncLock oLock
            Dim oGetCurrencyToCurrencyExchangeRateRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyToCurrencyExchangeRateQuery
            Dim oGetCurrencyToCurrencyExchangeRateResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyToCurrencyExchangeRateQueryResponse
            Dim oCurrency As Currency
            Dim sbLogMessage As StringBuilder

            Try
                oGetCurrencyToCurrencyExchangeRateRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyToCurrencyExchangeRateQuery
                oGetCurrencyToCurrencyExchangeRateResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyToCurrencyExchangeRateQueryResponse
                oCurrency = New Currency
                sbLogMessage = New StringBuilder


                With oGetCurrencyToCurrencyExchangeRateRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .CurrencyCodeFrom = sCurrencyCodeFrom
                    .CurrencyCodeTo = sCurrencyCodeTo

                    If dCurrencyAmountUnRounded > 0 Then
                        .CurrencyAmountUnRounded = dCurrencyAmountUnRounded
                        .CurrencyAmountUnRoundedSpecified = True
                    Else
                        .CurrencyAmountUnRoundedSpecified = False
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetCurrencyToCurrencyExchangeRate, oGetCurrencyToCurrencyExchangeRateRequest)
                    oGetCurrencyToCurrencyExchangeRateResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyToCurrencyExchangeRateQueryResponse)(result)
                End Using



                With oGetCurrencyToCurrencyExchangeRateResponse

                    'If 1 = 0 Then

                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(oGetCurrencyToCurrencyExchangeRateResponse.Errors)
                    'Else

                    oCurrency.BaseAmount = .BaseAmount
                    oCurrency.BaseAmountUnrounded = .BaseAmount
                    oCurrency.TransactionCurrencyRate = .ConversionRate


                    'End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCurrencyExchangeRates executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oCurrency.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetCurrencyToCurrencyExchangeRateRequest = Nothing
                oGetCurrencyToCurrencyExchangeRateResponse = Nothing
            End Try

            Return oCurrency

        End SyncLock
    End Function

    Public Overrides Sub GetDatasetSchema(ByRef r_oRisk As Risk,
                                         Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oGetDatasetSchemaRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetSchemaQuery
            Dim oGetDatasetSchemaResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetSchemaQueryResponse
            Dim sbLogMessage As StringBuilder

            Try
                oGetDatasetSchemaRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetSchemaQuery
                oGetDatasetSchemaResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetSchemaQueryResponse
                sbLogMessage = New StringBuilder


                With oGetDatasetSchemaRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .DataModelCode = r_oRisk.DataModelCode

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDatasetSchema, oGetDatasetSchemaRequest)
                    oGetDatasetSchemaResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetSchemaQueryResponse)(result)
                End Using





                With oGetDatasetSchemaResponse

                    'If 1 = 0 Then

                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)

                    'Else
                    r_oRisk.DatasetSchema = .DatasetSchema
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetDatasetSchema executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("sDataModelCode = " & r_oRisk.DataModelCode & vbCrLf)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetDatasetSchemaRequest = Nothing
                oGetDatasetSchemaResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub GetDocument(ByRef r_oDocument As Document,
                                         Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oGetDocumentRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentQuery
            Dim oGetDocumentResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentQueryResponse
            Dim sbLogMessage As StringBuilder

            Try
                oGetDocumentRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentQuery
                oGetDocumentResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentQueryResponse
                sbLogMessage = New StringBuilder


                With oGetDocumentRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .DocNum = r_oDocument.DocNum
                    .Compress = r_oDocument.Compress
                    .ConvertPdf = r_oDocument.ConvertPdf
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDocuments, oGetDocumentRequest)
                    oGetDocumentResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentQueryResponse)(result)

                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oGetDocumentResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oDocument.PdfDocument = oGetDocumentResponse.PdfDocument
                        r_oDocument.FileExtension = oGetDocumentResponse.FileExtension
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetDocument executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oDocument = " & r_oDocument.Print.Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("v_iDocNum = " & r_oDocument.DocNum.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetDocumentRequest = Nothing
                oGetDocumentResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Function GetDocumentList(ByVal v_iInsuranceFolderKey As Integer,
                                      Optional ByVal v_sBranchCode As String = Nothing) As DocumentCollection

        SyncLock oLock

            Dim oGetDocumentListRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentListQuery
            Dim oGetDocumentListResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentListQueryResponse
            Dim oDocuments As DocumentCollection = Nothing
            Dim oNewDocument As Document
            Dim sbLogMessage As StringBuilder

            Try
                oGetDocumentListRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentListQuery
                oGetDocumentListResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentListQueryResponse
                sbLogMessage = New StringBuilder


                With oGetDocumentListRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iInsuranceFolderKey > 0 Then
                        .InsuranceFolderKey = v_iInsuranceFolderKey
                    Else
                        Throw New ArgumentException("InsuranceFolderKey")
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDocumentList, oGetDocumentListRequest)
                    oGetDocumentListResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentListQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oGetDocumentListResponse

                    'If 1 = 0 Then

                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    If .Documents IsNot Nothing Then
                        oDocuments = New DocumentCollection


                        For Each oDocument As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseDocumentType In .Documents
                            oNewDocument = New Document()
                            With oNewDocument
                                .DocNum = oDocument.DocNum
                                .DocDescription = oDocument.DocDescription
                            End With
                            oDocuments.Add(oNewDocument)
                        Next
                    End If

                    'End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetDocumentList executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFolderKey = " & v_iInsuranceFolderKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oDocuments) Then
                        sbLogMessage.AppendLine("Returned " & oDocuments.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetDocumentListRequest = Nothing
                oGetDocumentListResponse = Nothing
            End Try


            Return oDocuments

        End SyncLock
    End Function

    ''' <summary>
    ''' Returns the defaults for a given document template
    ''' </summary>
    ''' <param name="DocumentTemplateCodes">Comma separated list of document template codes</param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocumentDefaults(ByVal DocumentTemplateCodes As List(Of String),
                                        Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal DocumentTemplateKeys As List(Of String) = Nothing) As DocumentDefaultsCollection

        Dim oGetDocumentDefaultsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentDefaultsQuery
        Dim oGetDocumentDefaultsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentDefaultsQueryResponse
        Dim sDocumentTemplateCodes As String = String.Empty
        Dim sDocumentTemplateKeys As String = String.Empty
        Dim oDocumentDefaultsCollection As DocumentDefaultsCollection
        Dim sbLogMessage As StringBuilder

        Try
            oGetDocumentDefaultsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentDefaultsQuery
            oGetDocumentDefaultsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentDefaultsQueryResponse
            oDocumentDefaultsCollection = New DocumentDefaultsCollection
            sbLogMessage = New StringBuilder


            'need to convert the list of strings into a comma separated list

            For Each sItem As String In DocumentTemplateCodes
                sDocumentTemplateCodes += sItem & ","
            Next
            'take off the trailing ,
            If sDocumentTemplateCodes <> String.Empty Then
                sDocumentTemplateCodes = Left(sDocumentTemplateCodes, Len(sDocumentTemplateCodes) - 1)
            End If
            'create object to hold the collection returned

            If (DocumentTemplateKeys IsNot Nothing AndAlso DocumentTemplateKeys.Count > 0) Then
                For Each sItem As String In DocumentTemplateKeys
                    sDocumentTemplateKeys += sItem & ","
                Next
            End If

            'take off the trailing ,
            If sDocumentTemplateKeys <> String.Empty Then
                sDocumentTemplateKeys = Left(sDocumentTemplateKeys, Len(sDocumentTemplateKeys) - 1)
            End If

            'nothing in cache so make the call to sam
            With oGetDocumentDefaultsRequest
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode

                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode

                End If

                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                .DocumentTemplateCodes = If(String.IsNullOrEmpty(sDocumentTemplateCodes), Nothing, sDocumentTemplateCodes)
                .DocumentTemplateKeys = If(String.IsNullOrEmpty(sDocumentTemplateKeys), Nothing, sDocumentTemplateKeys)
            End With


            Using trace As New Tracer(Category.Trace)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDocumentDefaults, oGetDocumentDefaultsRequest)
                oGetDocumentDefaultsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDocumentDefaultsQueryResponse)(result)
            End Using




            With oGetDocumentDefaultsResponse
                'If 1 = 0 Then
                'Process the error object if errors, and throw as a single exception
                'Throw New NexusException(.Errors)
                'Else
                If .DocumentTemplates IsNot Nothing Then
                    For Each oDocumentTemplates As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetDocumentDefaultsResponseTypeDocumentTemplates In .DocumentTemplates
                        ' make sure Document Template Code not nothing
                        If sDocumentTemplateKeys IsNot Nothing AndAlso Not String.IsNullOrEmpty(sDocumentTemplateKeys) Then
                            oDocumentDefaultsCollection.Add(New DocumentDefaults With {
                                                                    .documentTemplateCode = oDocumentTemplates.DocumentTemplateCode,
                                                                    .documentTemplateDescription = oDocumentTemplates.DocumentTemplateDescription,
                                                                    .documentTemplateID = oDocumentTemplates.DocumentTemplateID,
                                                                    .documentGroupCode = oDocumentTemplates.DocumentGroupCode,
                                                                    .documentGroupDescription = oDocumentTemplates.DocumentGroupDescription,
                                                                    .documentGroupID = oDocumentTemplates.DocumentGroupID,
                                                                    .documentSubGroupCode = oDocumentTemplates.DocumentSubGroupCode,
                                                                    .documentSubGroupDescription = oDocumentTemplates.DocumentSubGroupDescription,
                                                                    .documentSubGroupID = oDocumentTemplates.DocumentSubGroupID,
                                                                    .InternalOnly = oDocumentTemplates.InternalOnly,
                                                                    .Selected = oDocumentTemplates.SelectedByDefault,
                                                                    .EmailDocumentSubjectCode = oDocumentTemplates.EmailDocumentSubjectCode,
                                                                    .EmailDocumentAttachmentCode = oDocumentTemplates.EmailDocumentAttachmentCode
                                                                    })
                        Else
                            If oDocumentTemplates.DocumentTemplateCode IsNot Nothing AndAlso DocumentTemplateCodes.Contains(oDocumentTemplates.DocumentTemplateCode) Then
                                oDocumentDefaultsCollection.Add(New DocumentDefaults With {
                                                                    .documentTemplateCode = oDocumentTemplates.DocumentTemplateCode,
                                                                    .documentTemplateDescription = oDocumentTemplates.DocumentTemplateDescription,
                                                                    .documentTemplateID = oDocumentTemplates.DocumentTemplateID,
                                                                    .documentGroupCode = oDocumentTemplates.DocumentGroupCode,
                                                                    .documentGroupDescription = oDocumentTemplates.DocumentGroupDescription,
                                                                    .documentGroupID = oDocumentTemplates.DocumentGroupID,
                                                                    .documentSubGroupCode = oDocumentTemplates.DocumentSubGroupCode,
                                                                    .documentSubGroupDescription = oDocumentTemplates.DocumentSubGroupDescription,
                                                                    .documentSubGroupID = oDocumentTemplates.DocumentSubGroupID,
                                                                    .InternalOnly = oDocumentTemplates.InternalOnly,
                                                                    .Selected = oDocumentTemplates.SelectedByDefault,
                                                                    .EmailDocumentSubjectCode = oDocumentTemplates.EmailDocumentSubjectCode,
                                                                    .EmailDocumentAttachmentCode = oDocumentTemplates.EmailDocumentAttachmentCode
                                                                    })
                            End If
                        End If
                    Next
                End If
                'End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetDocumentDefaults executed ok" & vbCrLf)

                sbLogMessage.AppendLine("Input : " & vbCrLf)

                If IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                Else
                    sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                End If

                sbLogMessage.AppendLine("Output : " & vbCrLf)

                'If IsNothing(iBackgroundJobID) Then
                '    sbLogMessage.AppendLine("Background Job ID : nothing" & vbCrLf)
                'Else
                '    sbLogMessage.AppendLine("Background Job ID : " & iBackgroundJobID.ToString & vbCrLf)
                'End If
                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oGetDocumentDefaultsRequest = Nothing
            oGetDocumentDefaultsResponse = Nothing
        End Try


        Return oDocumentDefaultsCollection

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iEventKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEventNote(ByVal v_iEventKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As EventDetailsCollection
        SyncLock oLock
            Dim oGetEventNoteRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventNoteQuery
            Dim oGetEventNoteResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventNoteQueryResponse
            Dim oEventDetails As EventDetailsCollection = Nothing
            Dim oNewEventDetails As EventDetails
            Dim sbLogMessage As StringBuilder

            Try
                oGetEventNoteRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventNoteQuery
                oGetEventNoteResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventNoteQueryResponse
                oNewEventDetails = New EventDetails
                sbLogMessage = New StringBuilder


                With oGetEventNoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'Checking the EventKey
                    If v_iEventKey > 0 Then
                        .EventKey = v_iEventKey
                    Else
                        Throw New ArgumentNullException("v_iEventKey")
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetEventNote, oGetEventNoteRequest)
                    oGetEventNoteResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventNoteQueryResponse)(result)
                End Using

                With oGetEventNoteResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    oEventDetails = New EventDetailsCollection


                    If .EventNotes IsNot Nothing AndAlso .EventNotes.Count > 0 Then

                        For Each oEventDetailsRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetEventNoteResponseTypeRow In .EventNotes
                            oNewEventDetails = New EventDetails
                            With oNewEventDetails
                                .EventKey = oEventDetailsRow.EventKey
                                .EventPublicTextKey = oEventDetailsRow.EventPublicTextKey
                                .EventText = oEventDetailsRow.EventText
                            End With
                            oEventDetails.Add(oNewEventDetails)
                            oNewEventDetails = Nothing
                        Next

                    End If

                    'End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetEventNoteResponse executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_iEventKey) Then
                        sbLogMessage.AppendLine("v_iEventKey = " & v_iEventKey.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_iEventKey = nothing" & vbCrLf)
                    End If
                    sbLogMessage.AppendLine("Returned " & oEventDetails.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetEventNoteRequest = Nothing
                oGetEventNoteResponse = Nothing
            End Try
            Return oEventDetails

        End SyncLock
    End Function

    Public Overrides Sub GetUserAuthorityValue(ByRef r_oUserAuthority As UserAuthority,
                                                            Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock


            Dim oUserAuthority As UserAuthority
            Dim oGetUserAuthorityValueRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserAuthorityValueQuery
            Dim oGetUserAuthorityValueResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserAuthorityValueQueryResponse


            Try
                oUserAuthority = New UserAuthority
                oGetUserAuthorityValueRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserAuthorityValueQuery
                oGetUserAuthorityValueResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserAuthorityValueQueryResponse
                Dim sCacheKey As String = "UserAuthority_" & r_oUserAuthority.UserCode.ToString & "_" & [Enum].GetName(GetType(UserAuthority.UserAuthorityOptionType), r_oUserAuthority.UserAuthorityOption).ToString

                If Current.Cache(sCacheKey) Is Nothing Then

                    With oGetUserAuthorityValueRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If

                        .UserCode = r_oUserAuthority.UserCode
                        .UserAuthorityOption = r_oUserAuthority.UserAuthorityOption

                    End With


                    Using trace As New Tracer(Category.Trace)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUserAuthorityValue, oGetUserAuthorityValueRequest)
                        oGetUserAuthorityValueResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserAuthorityValueQueryResponse)(result)
                    End Using




                    With oGetUserAuthorityValueResponse.GetUserAuthorityValueResponse
                        'If 1 = 0 Then
                        'Process the error object if errors, and throw as a single exception
                        'Else
                        'add response parameters here
                        r_oUserAuthority.UserAuthorityValue = .UserAuthorityValue
                        r_oUserAuthority.UserAuthorityOptionalValue1 = .UserAuthorityOptionalValue1
                        r_oUserAuthority.UserAuthorityOptionalValue2 = .UserAuthorityOptionalValue2
                        r_oUserAuthority.UserAuthorityOptionalValue3 = .UserAuthorityOptionalValue3
                        r_oUserAuthority.UserAuthorityOptionalValue3_baseAmount = If(.UserAuthorityOptionalValue3_baseAmount, 0R)
                        'Put it in Cache in order to read it from cache instead of SAM call
                        oUserAuthority.UserAuthorityOption = r_oUserAuthority.UserAuthorityOption
                        oUserAuthority.UserCode = oGetUserAuthorityValueRequest.UserCode
                        oUserAuthority.UserAuthorityValue = .UserAuthorityValue
                        oUserAuthority.UserAuthorityOptionalValue1 = .UserAuthorityOptionalValue1
                        oUserAuthority.UserAuthorityOptionalValue2 = .UserAuthorityOptionalValue2
                        oUserAuthority.UserAuthorityOptionalValue3 = .UserAuthorityOptionalValue3
                        oUserAuthority.UserAuthorityOptionalValue3_baseAmount = If(.UserAuthorityOptionalValue3_baseAmount, 0R)
                        Current.Cache.Insert(sCacheKey, oUserAuthority, Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        'End If
                    End With
                Else
                    r_oUserAuthority = CType(Current.Cache(sCacheKey), UserAuthority)
                End If

                If Logger.IsLoggingEnabled Then
                    Dim sbLogMessage As StringBuilder
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("GetUserAuthorityValue executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    'sbLogMessage.AppendLine(OptionType.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetUserAuthorityValueRequest = Nothing
                oGetUserAuthorityValueResponse = Nothing
            End Try

        End SyncLock

    End Sub

    Public Overrides Function GetUserDetails(ByVal sUserName As String, Optional ByVal bIsSSO As Boolean = False) As UserDetails
        SyncLock oLock

            Dim oGetUserDetailsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserDetailsQuery
            Dim oGetUserDetailsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserDetailsQueryResponse
            Dim oGetProductsForUserBranchRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductsForUserBranchQuery
            Dim oGetProductsForUserBranchResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductsForUserBranchQueryResponse
            Dim oUserDetails As UserDetails = Nothing
            Dim oUserGroup As UserGroup
            Dim oUserGroupCollection As UserGroupCollection
            Dim oUserProductByBranch As UserProductByBranch
            Dim oUserProductByBranchCollection As New UserProductByBranchCollection
            Dim sbLogMessage As StringBuilder

            Try
                oGetUserDetailsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserDetailsQuery
                oGetUserDetailsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserDetailsQueryResponse
                oUserGroupCollection = New UserGroupCollection
                sbLogMessage = New StringBuilder


                'oGetUserDetailsRequest.IsSSO = bIsSSO
                oGetUserDetailsRequest.LoginUserName = sUserName
                oGetUserDetailsRequest.UserName = sUserName
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUserDetails, oGetUserDetailsRequest)
                    oGetUserDetailsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserDetailsQueryResponse)(result)
                End Using

                With oGetUserDetailsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    End If

                    oUserDetails = New UserDetails(.PartyKey, .FullUsername)

                    oUserDetails.EmailAddress = .EmailAddress
                    oUserDetails.PartyName = .PartyName
                    oUserDetails.LastLogin = .LastLogin
                    oUserDetails.PasswordChange = .PasswordChangeDate
                    oUserDetails.PartyType = .PartyType
                    oUserDetails.ConsolidatedAgentCommission = .ConsolidatedAgentCommission
                    oUserDetails.PartyKey = .PartyKey
                    oUserDetails.PartyCode = .PartyShortName
                    oUserDetails.PasswordChange = .PasswordChangeDate
                    oUserDetails.PureUsername = .PureUsername
                    oUserDetails.UserId = .UserKey
                    If .UserGroups IsNot Nothing Then
                        For Each oUsergroupDetails As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetUserDetailsResponseTypeRow In .UserGroups
                            If oUsergroupDetails.IsAssociated <> 0 Then
                                oUserGroup = New UserGroup
                                oUserGroup.Code = oUsergroupDetails.Code.Trim
                                oUserGroup.Description = oUsergroupDetails.Description.Trim
                                oUserGroup.IsSysAdmin = oUsergroupDetails.IsSystemAdmin
                                oUserGroup.IsSupervisor = oUsergroupDetails.IsSupervisor
                                oUserGroup.UserGroupKey = oUsergroupDetails.UserGroupKey
                                oUserGroupCollection.Add(oUserGroup)
                            End If
                        Next
                        oUserDetails.AvailableUsergroups = oUserGroupCollection
                    End If

                    For Each oBranchType As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseBranchType In .SourceList
                        oUserDetails.ListOfBranches.Add(New Branch(oBranchType.BranchCode, oBranchType.Description, oBranchType.BranchKey, oBranchType.AgentCode, oBranchType.BusinessType, oBranchType.AgentKey))
                    Next

                End With

                ' ''call GetProductsForUserBranch to get product collection of this user
                oGetProductsForUserBranchRequest.LoginUserName = oUserDetails.PureUsername
                Dim sDefaultBranch = "HeadOff"
                If String.IsNullOrEmpty(sBranchCode) Then
                    'Use the default branch code
                    oGetProductsForUserBranchRequest.BranchCode = sDefaultBranch
                Else
                    'Use the branch code in session 
                    oGetProductsForUserBranchRequest.BranchCode = sBranchCode
                End If
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetProductsForUserBranch, oGetProductsForUserBranchRequest)
                    oGetProductsForUserBranchResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductsForUserBranchQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oGetProductsForUserBranchResponse

                    'If 1 = 0 Then

                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    If .Products IsNot Nothing Then
                        For Each oUserProductByBranchDetails As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetProductsForUserBranchResponseTypeProductsRow In .Products

                            oUserProductByBranch = New UserProductByBranch
                            oUserProductByBranch.Code = oUserProductByBranchDetails.ProductCode.Trim
                            oUserProductByBranch.Description = oUserProductByBranchDetails.ProductDescription.Trim
                            oUserProductByBranch.UserProductKey = oUserProductByBranchDetails.ProductKey
                            For Each oBranchType As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseBranchType In oUserProductByBranchDetails.Branches
                                oUserProductByBranch.ListOfBranches.Add(New Branch(oBranchType.BranchCode, oBranchType.Description.Trim))
                            Next
                            oUserProductByBranchCollection.Add(oUserProductByBranch)
                        Next
                        oUserDetails.AvailableUserProductsByBranch = oUserProductByBranchCollection
                    End If
                    'End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserDetails executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine(oUserDetails.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw

            Finally
                oGetUserDetailsRequest = Nothing
                oGetUserDetailsResponse = Nothing
                oGetProductsForUserBranchRequest = Nothing
                oGetProductsForUserBranchResponse = Nothing
            End Try
            Return oUserDetails

        End SyncLock

    End Function


    Public Overrides Function GetAudittrailModule(ByRef r_oAudittrailModule As AuditTrail,
                                           Optional ByVal v_sBranchCode As String = Nothing) As AuditTrailCollection
        SyncLock oLock

            Dim oGetAudittrailModuleRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailModuleQuery
            Dim oGetAudittrailModuleResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailModuleQueryResponse
            Dim oAuditTrail As AuditTrail = Nothing
            Dim oAuditTrailCollection As AuditTrailCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetAudittrailModuleRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailModuleQuery
                oGetAudittrailModuleResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailModuleQueryResponse

                sbLogMessage = New StringBuilder

                With oGetAudittrailModuleRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If


                End With
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetAuditTrailModule, oGetAudittrailModuleRequest)
                    oGetAudittrailModuleResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailModuleQueryResponse)(result)
                End Using

                oAuditTrailCollection = New AuditTrailCollection

                With oGetAudittrailModuleResponse  'With Response Type
                    'WorkManager Response
                    'Fetching from the  WorkManager Response Collection 
                    If .AuditTrailModule IsNot Nothing AndAlso .AuditTrailModule.Count > 0 Then
                        For Each oAuditModule As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetAuditTrailModuleResponseType In .AuditTrailModule
                            oAuditTrail = New NexusProvider.AuditTrail
                            oAuditTrail.ModuleId = oAuditModule.ModuleKey
                            oAuditTrail.ModuleName = oAuditModule.Moduledesc
                            oAuditTrailCollection.Add(oAuditTrail)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("oAuditTrail executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oAuditTrail.Print() & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oAuditTrailCollection.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetAudittrailModuleRequest = Nothing
            End Try


            Return oAuditTrailCollection  'Returning Audit Trail Module Collection
        End SyncLock

    End Function

    Public Overrides Function GetAudittrailUser(ByRef r_oAudittrailUser As AuditTrail,
                                           Optional ByVal v_sBranchCode As String = Nothing) As AuditTrailCollection
        SyncLock oLock

            Dim oGetAudittrailUserRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailUserQuery
            Dim oGetAudittrailUserResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailUserQueryResponse
            Dim oAuditTrail As AuditTrail = Nothing
            Dim oAuditTrailCollection As AuditTrailCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetAudittrailUserRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailUserQuery
                oGetAudittrailUserResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailUserQueryResponse

                sbLogMessage = New StringBuilder

                With oGetAudittrailUserRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If


                End With
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetAuditTrailUser, oGetAudittrailUserRequest)
                    oGetAudittrailUserResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailUserQueryResponse)(result)
                End Using

                oAuditTrailCollection = New AuditTrailCollection

                With oGetAudittrailUserResponse  'With Response Type
                    'WorkManager Response
                    'Fetching from the  WorkManager Response Collection 
                    If .AuditTrailUser IsNot Nothing AndAlso .AuditTrailUser.Count > 0 Then
                        For Each oAuditUser As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetAuditTrailUserResponseType In .AuditTrailUser
                            oAuditTrail = New NexusProvider.AuditTrail
                            oAuditTrail.UserId = oAuditUser.UserKey
                            oAuditTrail.UserName = oAuditUser.Userdesc
                            oAuditTrailCollection.Add(oAuditTrail)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("oAuditTrail executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oAuditTrail.Print() & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oAuditTrailCollection.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetAudittrailUserRequest = Nothing
            End Try


            Return oAuditTrailCollection  'Returning Audit Trail User Collection
        End SyncLock

    End Function

    Public Overrides Function GetAuditTrails(ByRef r_oAuditTrails As AuditTrail,
                                           Optional ByVal v_sBranchCode As String = Nothing) As AuditTrailCollection

        SyncLock oLock

            Dim oGetAuditTrailRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailDetailsQuery
            Dim oGetAuditTrailResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailDetailsQueryResponse
            Dim oEventDetailsCollection As AuditTrailCollection
            Dim oEventDetails As AuditTrail
            Dim sbLogMessage As StringBuilder

            Try
                oGetAuditTrailRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailDetailsQuery
                oGetAuditTrailResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailDetailsQueryResponse
                oEventDetailsCollection = New AuditTrailCollection
                sbLogMessage = New StringBuilder




                With oGetAuditTrailRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    If r_oAuditTrails.UserKey <> 0 Then
                        .UserId = r_oAuditTrails.UserKey
                        '    .UserIdSpecified = True
                        'Else
                        '    .UserIdSpecified = False
                    End If

                    If r_oAuditTrails.ModuleKey > 0 Then
                        .ModuleId = r_oAuditTrails.ModuleKey
                        '    .ModuleIdSpecified = True
                        'Else
                        '    .ModuleIdSpecified = False
                    End If


                    If r_oAuditTrails.FromDateSpecified Then
                        .FromDate = r_oAuditTrails.FromDate
                        '    .FromDateSpecified = True
                        'Else
                        '    .FromDateSpecified = False
                    End If

                    If r_oAuditTrails.DateToSpecified Then
                        .DateTo = r_oAuditTrails.DateTo
                        '    .DateToSpecified = True
                        'Else
                        '    .DateToSpecified = False
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetAuditTrailDetails, oGetAuditTrailRequest)
                    oGetAuditTrailResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAuditTrailDetailsQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.





                With oGetAuditTrailResponse
                    If .AuditTrails IsNot Nothing Then


                        For Each oEventDetailsRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetAuditTrailResponseType In oGetAuditTrailResponse.AuditTrails
                            oEventDetails = New AuditTrail

                            oEventDetails.ConfigurationAuditdetailId = oEventDetailsRow.ConfigurationAuditdetailId
                            oEventDetails.ModuleName = oEventDetailsRow.ModuleName
                            oEventDetails.ScreenDescription = oEventDetailsRow.ScreenDescription
                            oEventDetails.FieldDescription = oEventDetailsRow.FieldDescription
                            oEventDetails.ModifiedOn = oEventDetailsRow.ModifiedOn
                            oEventDetails.OldValue = oEventDetailsRow.OldValue.ToString()
                            oEventDetails.NewValue = oEventDetailsRow.NewValue.ToString()
                            oEventDetails.UserName = oEventDetailsRow.UserName

                            oEventDetailsCollection.Add(oEventDetails)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAuditTrail executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oAuditTrails = " & r_oAuditTrails.Print.Replace("<br />", vbCrLf))


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetAuditTrailRequest = Nothing
                oGetAuditTrailResponse = Nothing
            End Try


            Return oEventDetailsCollection
        End SyncLock
    End Function


    Public Overrides Function GetUserGroups(Optional ByVal v_sBranchCode As String = Nothing) As UserGroupCollection
        SyncLock oLock

            Dim oGetUserGroupsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsQuery
            Dim oGetUserGroupsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsQueryResponse
            Dim oUserGroups As UserGroupCollection = Nothing
            Dim oUserGroup As UserGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetUserGroupsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsQuery
                oGetUserGroupsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsQueryResponse
                sbLogMessage = New StringBuilder


                With oGetUserGroupsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUserGroups, oGetUserGroupsRequest)
                    oGetUserGroupsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsQueryResponse)(result)
                End Using






                With oGetUserGroupsResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'End If

                    If .UserGroups IsNot Nothing AndAlso .UserGroups.Count > 0 Then

                        oUserGroups = New UserGroupCollection()
                        For Each oBaseUserGroup As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetUserGroupsResponseTypeRow In .UserGroups
                            oUserGroup = New UserGroup()
                            With oUserGroup
                                .Code = oBaseUserGroup.Code.Trim
                                .Description = oBaseUserGroup.Description.Trim
                                .EffectiveDate = oBaseUserGroup.EffectiveDate
                                .IsDeleted = oBaseUserGroup.IsDeleted
                                .IsSysAdmin = oBaseUserGroup.IsSystemAdmin
                                .UserGroupKey = oBaseUserGroup.UserGroupKey
                                .IsDebtorPMUserGroup = oBaseUserGroup.IsDebtorPMUserGroup
                            End With
                            oUserGroups.Add(oUserGroup)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroups.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetUserGroupsRequest = Nothing
                oGetUserGroupsResponse = Nothing
            End Try


            Return oUserGroups
        End SyncLock
    End Function

    Public Overrides Function GetWorkManagerScheduledTasks(ByVal oWorkManager As WorkManager,
                                           Optional ByVal v_sBranchCode As String = Nothing) As WorkManagerCollection
        SyncLock oLock

            Dim oGetWmScheduledtasksRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWorkManagerScheduledTasksQuery   ' Request Type
            Dim oNewWorkManager As WorkManager  'Object of WorkManager Class
            Dim oNewWorkCollection As WorkManagerCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetWmScheduledtasksRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWorkManagerScheduledTasksQuery
                sbLogMessage = New StringBuilder

                Static oGetWmScheduledtasksResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWorkManagerScheduledTasksQueryResponse  ' Response Type

                With oGetWmScheduledtasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    'Added on 22 DEC for the creation of WorkManager.aspx 
                    '------------------------------------------------------
                    'Enum Values for Task Status
                    Select Case oWorkManager.TaskStatus
                        Case TaskStatus.All
                            .TaskStatusKey = TaskStatus.All
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.Complete
                            .TaskStatusKey = TaskStatus.Complete
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.InComplete
                            .TaskStatusKey = TaskStatus.InComplete
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.InProgress
                            .TaskStatusKey = TaskStatus.InProgress
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.[New]
                            .TaskStatusKey = TaskStatus.[New]
                            .TaskStatusKeySpecified = True
                        Case TaskStatus.NotComplete
                            .TaskStatusKey = TaskStatus.NotComplete
                            .TaskStatusKeySpecified = True
                    End Select
                    ' EnumValues for DateRange
                    Select Case oWorkManager.DateRange
                        Case DateRange.AllDates
                            .Date = DateRange.AllDates
                            .DateSpecified = True
                        Case DateRange.Next14Days
                            .Date = DateRange.Next14Days
                            .DateSpecified = True
                        Case DateRange.Next28Days
                            .Date = DateRange.Next28Days
                            .DateSpecified = True
                        Case DateRange.Next2Days
                            .Date = DateRange.Next2Days
                            .DateSpecified = True
                        Case DateRange.Next3Days
                            .Date = DateRange.Next3Days
                            .DateSpecified = True
                        Case DateRange.Next4Days
                            .Date = DateRange.Next4Days
                            .DateSpecified = True
                        Case DateRange.Next5Days
                            .Date = DateRange.Next5Days
                            .DateSpecified = True
                        Case DateRange.Next6Days
                            .Date = DateRange.Next6Days
                            .DateSpecified = True
                        Case DateRange.Next7Days
                            .Date = DateRange.Next7Days
                            .DateSpecified = True
                        Case DateRange.Today
                            .Date = DateRange.Today
                            .DateSpecified = True
                        Case DateRange.Tomorrow
                            .Date = DateRange.Tomorrow
                            .DateSpecified = True
                    End Select
                    'EnumValues for ShowType
                    Select Case oWorkManager.ShowType
                        Case ShowType.All
                            .ShowSystemKEY = ShowType.All
                            .ShowSystemKEYSpecified = True
                        Case ShowType.Sys
                            .ShowSystemKEY = ShowType.Sys
                            .ShowSystemKEYSpecified = True
                        Case ShowType.User
                            .ShowSystemKEY = ShowType.User
                            .ShowSystemKEYSpecified = True
                    End Select

                    'Added on 22 DEC for the creation of WorkManager.aspx 
                    '---------------------------------------------------------------------
                    .UserGroupCODE = oWorkManager.UserGroupCode
                    .UserCODE = oWorkManager.UserCode
                    .PartyKey = oWorkManager.PartyKey
                    .ReferenceNumber = oWorkManager.ReferenceNumber
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetWorkManagerScheduledTasks, oGetWmScheduledtasksRequest)
                    oGetWmScheduledtasksResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWorkManagerScheduledTasksQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object
                oNewWorkCollection = New WorkManagerCollection

                With oGetWmScheduledtasksResponse  'With Response Type
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    'WorkManager Response
                    'Fetching from the  WorkManager Response Collection 
                    If .Tasks IsNot Nothing AndAlso .Tasks.Count > 0 Then
                        For Each oWork As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetWorkManagerScheduledTasksResponseTypeRow In .Tasks
                            oNewWorkManager = New NexusProvider.WorkManager
                            oNewWorkManager.Urgent = oWork.Urgent
                            oNewWorkManager.TaskInstanceKey = oWork.TaskInstanceKey
                            oNewWorkManager.DueDate = oWork.DueDate
                            oNewWorkManager.Description = oWork.Description
                            oNewWorkManager.Customer = oWork.Customer
                            oNewWorkManager.Branch = oWork.Branch
                            oNewWorkManager.Type = oWork.Type
                            oNewWorkManager.UserGroupKey = oWork.UserGroupKey
                            oNewWorkManager.UserKey = oWork.UserKey
                            oNewWorkManager.TaskStatusKey = oWork.TaskStatusKey 'Changed  on 5-1-2009
                            oNewWorkManager.UserGroupCode = oWork.UserGroupCode
                            oNewWorkManager.UserGroupDescription = oWork.UserGroupDescription
                            oNewWorkManager.UserCode = oWork.UserCode
                            oNewWorkManager.TaskGroupKey = oWork.TaskGroupKey
                            oNewWorkManager.TaskKey = oWork.TaskKey
                            oNewWorkManager.PartyName = oWork.PartyName
                            oNewWorkCollection.Add(oNewWorkManager)
                        Next
                    End If
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oNewWorkCollection.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetWmScheduledtasksRequest = Nothing
            End Try


            Return oNewWorkCollection  'Returning WorkManager Collection
        End SyncLock
    End Function

    Public Overrides Sub AddEvent(ByRef r_oEventDetails As EventDetails,
                                  Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oAddEventRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventCommand
            Dim oAddEventResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oAddEventRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventCommand
                oAddEventResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventCommandResponse
                sbLogMessage = New StringBuilder


                With oAddEventRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .EventDate = r_oEventDetails.EventDate
                    .EventLogSubjectKey = r_oEventDetails.EventLogSubjectKey
                    .EventTypeKey = r_oEventDetails.EventTypeKey
                    .PartyKey = r_oEventDetails.PartyKey
                    .Priority = r_oEventDetails.Priority
                    .RtfText = r_oEventDetails.RtfText
                    .StatusKey = r_oEventDetails.StatusKey
                    .UserName = r_oEventDetails.UserName
                    .Document_Path = r_oEventDetails.Document_Path
                    If r_oEventDetails.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oEventDetails.InsuranceFileKey
                    End If
                    If r_oEventDetails.ClaimKey > 0 Then
                        .ClaimKey = r_oEventDetails.ClaimKey
                    End If


                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddEvent, oAddEventRequest)
                    oAddEventResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddEventCommandResponse)(result)
                End Using


                With oAddEventResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    r_oEventDetails.EventKey = .EventKey

                    'End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddEvent executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(r_oEventDetails.UserName) Then
                        sbLogMessage.AppendLine("v_sUserName = " & r_oEventDetails.UserName.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sUserName = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oAddEventRequest = Nothing
                oAddEventResponse = Nothing
            End Try


        End SyncLock
    End Sub

        Public Overrides Sub AddClientDataExtractAuditTrail(ByVal v_iPartyKey As Integer, ByVal v_sClientCode As String, Optional ByVal v_sBranchCode As String = Nothing)
    SyncLock oLock
        Dim oRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClientDataExtractAuditTrailCommand
        Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClientDataExtractAuditTrailCommandResponse
        Dim sbLogMessage As StringBuilder

        Try
            oRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClientDataExtractAuditTrailCommand
            oResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClientDataExtractAuditTrailCommandResponse
            sbLogMessage = New StringBuilder

            With oRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    If String.IsNullOrEmpty(sBranchCode) Then
                        .BranchCode = sDefaultBranchCode
                    Else
                        .BranchCode = sBranchCode
                    End If
                Else
                    .BranchCode = v_sBranchCode
                End If
                .PartyKey = v_iPartyKey
                .ClientCode = v_sClientCode
            End With

            SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
            Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddClientDataExtractAuditTrail, oRequest)
            oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClientDataExtractAuditTrailCommandResponse)(result)

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("AddClientDataExtractAuditTrail executed ok" & vbCrLf)
                sbLogMessage.AppendLine("PartyKey = " & v_iPartyKey.ToString & vbCrLf)
                LogMessageEntry(sbLogMessage)
            End If

        Catch ex As Exception
            Throw
        Finally
            oRequest = Nothing
            oResponse = Nothing
        End Try
    End SyncLock
End Sub

    Public Overrides Function GetEventDetails(ByRef r_oEventDetails As EventDetails,
                                           Optional ByVal v_sBranchCode As String = Nothing) As EventDetailsCollection

        SyncLock oLock

            Dim oGetEventDetailsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventDetailsQuery
            Dim oGetEventDetailsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventDetailsQueryResponse
            Dim oEventDetailsCollection As EventDetailsCollection
            Dim oEventDetails As EventDetails
            Dim sbLogMessage As StringBuilder

            Try
                oGetEventDetailsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventDetailsQuery
                oGetEventDetailsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventDetailsQueryResponse
                oEventDetailsCollection = New EventDetailsCollection
                sbLogMessage = New StringBuilder




                With oGetEventDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    If r_oEventDetails.BGKey > 0 Then
                        .BGKey = r_oEventDetails.BGKey
                        .BGKeySpecified = True
                    Else
                        .BGKeySpecified = False
                    End If

                    If r_oEventDetails.InsuranceFolderKey > 0 Then
                        .InsuranceFolderKey = r_oEventDetails.InsuranceFolderKey
                        .InsuranceFolderKeySpecified = True
                    Else
                        .InsuranceFolderKeySpecified = False
                    End If

                    If r_oEventDetails.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oEventDetails.InsuranceFileKey
                        .InsuranceFileKeySpecified = True
                    Else
                        .InsuranceFileKeySpecified = False
                    End If

                    If r_oEventDetails.ClaimKey > 0 Then
                        .ClaimKey = r_oEventDetails.ClaimKey
                        .ClaimKeySpecified = True
                    Else
                        .ClaimKeySpecified = False
                    End If

                    If r_oEventDetails.OldPartyTypeKey > 0 Then
                        .OldPartyTypeKey = r_oEventDetails.OldPartyTypeKey
                        .OldPartyTypeKeySpecified = True
                    Else
                        .OldPartyTypeKeySpecified = False
                    End If

                    If r_oEventDetails.FSAComplaintFolderKey > 0 Then
                        .FSAComplaintFolderKey = r_oEventDetails.FSAComplaintFolderKey
                        .FSAComplaintFolderKeySpecified = True
                    Else
                        .FSAComplaintFolderKeySpecified = False
                    End If

                    If r_oEventDetails.AccountKey > 0 Then

                        .AccountKey = r_oEventDetails.AccountKey
                        .AccountKeySpecified = True

                    Else
                        .AccountKeySpecified = False

                    End If

                    If r_oEventDetails.FromDateSpecified Then
                        .FromDate = r_oEventDetails.FromDate
                        .FromDateSpecified = True
                    Else
                        .FromDateSpecified = False
                    End If

                    If r_oEventDetails.DateToSpecified Then
                        .DateTo = r_oEventDetails.DateTo
                        .DateToSpecified = True
                    Else
                        .DateToSpecified = False
                    End If

                    If r_oEventDetails.BaseClaimKey > 0 Then

                        .BaseClaimKey = r_oEventDetails.BaseClaimKey
                        .BaseClaimKeySpecified = True

                    Else
                        .BaseClaimKeySpecified = False

                    End If

                    If r_oEventDetails.ClaimNumber <> "" Then

                        .ClaimNumber = r_oEventDetails.ClaimNumber
                        .ClaimNumberSpecified = True
                    Else
                        .ClaimNumberSpecified = False
                    End If

                    If r_oEventDetails.CaseKey > 0 Then

                        .CaseKey = r_oEventDetails.CaseKey
                        .CaseKeySpecified = True

                    Else
                        .CaseKeySpecified = False

                    End If
                    If r_oEventDetails.BaseCaseKey > 0 Then

                        .BaseCaseKey = r_oEventDetails.BaseCaseKey
                        .BaseCaseKeySpecified = True

                    Else
                        .BaseCaseKeySpecified = False

                    End If

                    If r_oEventDetails.PartyKey > 0 And r_oEventDetails.CaseKey = 0 Then
                        .PartyKey = r_oEventDetails.PartyKey
                    End If

                    If r_oEventDetails.UserId > 0 Then
                        .UserId = r_oEventDetails.UserId
                        .UserIdSpecified = True
                    Else
                        .UserIdSpecified = False
                    End If

                    If r_oEventDetails.EventTypeKey > 0 Then
                        .EventTypeKey = r_oEventDetails.EventTypeKey
                        .EventTypeKeySpecified = True
                    Else
                        .EventTypeKeySpecified = False
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetEventDetails, oGetEventDetailsRequest)
                    oGetEventDetailsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetEventDetailsQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.


                With oGetEventDetailsResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    If .EventDetails IsNot Nothing Then


                        For Each oEventDetailsRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetEventDetailsResponseTypeRow In oGetEventDetailsResponse.EventDetails
                            oEventDetails = New EventDetails

                            oEventDetails.BGKey = oEventDetailsRow.BGKey
                            oEventDetails.EventKey = oEventDetailsRow.EventKey
                            oEventDetails.InsuranceFolderKey = oEventDetailsRow.InsuranceFolderKey
                            oEventDetails.InsuranceFileKey = oEventDetailsRow.InsuranceFileKey
                            oEventDetails.DocumentKey = oEventDetailsRow.DocumentKey
                            oEventDetails.EventDate = oEventDetailsRow.EventDate
                            oEventDetails.TypeKey = oEventDetailsRow.TypeKey
                            oEventDetails.PolicyCode = oEventDetailsRow.PolicyCode
                            oEventDetails.ClaimNumber = oEventDetailsRow.ClaimNumber
                            oEventDetails.ClaimKey = oEventDetailsRow.ClaimKey
                            oEventDetails.Description = oEventDetailsRow.Description
                            oEventDetails.EventDescription = oEventDetailsRow.EventDescription
                            oEventDetails.UserName = oEventDetailsRow.UserName
                            oEventDetails.Priority = oEventDetailsRow.Priority
                            oEventDetails.StatusKey = oEventDetailsRow.StatusKey
                            oEventDetails.EventNoteExist = oEventDetailsRow.EventNoteExist
                            oEventDetails.EventType = oEventDetailsRow.EventType
                            oEventDetails.CaseNumber = oEventDetailsRow.CaseNumber
                            oEventDetails.Document_Path = oEventDetailsRow.Document_Path

                            oEventDetailsCollection.Add(oEventDetails)
                        Next
                    End If
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetEventDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oEventDetails = " & r_oEventDetails.Print.Replace("<br />", vbCrLf))


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetEventDetailsRequest = Nothing
                oGetEventDetailsResponse = Nothing
            End Try


            Return oEventDetailsCollection
        End SyncLock
    End Function

    Public Overrides Function GetDatasetDefinition(ByVal v_sDataModelCode As String,
                                  Optional ByVal v_sBranchCode As String = Nothing,
                                  Optional ByVal v_sDatasetDefinitionFileName As String = Nothing) As String

        SyncLock oLock

            Dim sDataSetDefinition As String = String.Empty
            Dim oGetDatasetDefinitionRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetDefinitionQuery
            Dim oGetDatasetDefinitionResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetDefinitionQueryBaseResponse
            Dim sbLogMessage As StringBuilder

            Try
                oGetDatasetDefinitionRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetDefinitionQuery
                oGetDatasetDefinitionResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetDefinitionQueryBaseResponse
                sbLogMessage = New StringBuilder


                'Cache the definition as this is a waste of bandwidth otherwise
                If Current.Cache(PROVIDER_DATASETDEFINITION & v_sDataModelCode) Is Nothing Then


                    With oGetDatasetDefinitionRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        'if the passed parameter v_sBranchCode is empty
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If

                        .DataModelCode = v_sDataModelCode

                    End With


                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDatasetDefinition, oGetDatasetDefinitionRequest)
                        oGetDatasetDefinitionResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDatasetDefinitionQueryBaseResponse)(result)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                    With oGetDatasetDefinitionResponse

                        'If 1 = 0 Then

                        'Process the error object if errors, and throw as a single exception
                        'Throw New NexusException(.Errors)
                        'Else
                        sDataSetDefinition = .XMLDatasetDefinition
                        Current.Cache.Insert(PROVIDER_DATASETDEFINITION & v_sDataModelCode, sDataSetDefinition,
                            Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        'End If

                    End With

                Else
                    sDataSetDefinition = CType(Current.Cache(PROVIDER_DATASETDEFINITION & v_sDataModelCode), String)
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetDatasetDefinition executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sDataModelCode = " & v_sDataModelCode.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sDatasetDefinitionFileName) Then
                        sbLogMessage.AppendLine("v_sDatasetDefinitionFileName = " & v_sDatasetDefinitionFileName.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sDatasetDefinitionFileName = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output:" & vbCrLf & sDataSetDefinition.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetDatasetDefinitionRequest = Nothing
                oGetDatasetDefinitionResponse = Nothing
                sbLogMessage = Nothing
            End Try


            Return sDataSetDefinition

        End SyncLock

    End Function

    ''' <summary>
    ''' This Function is used to Get DMEFolder details of a DME. 
    ''' This method takes ?FolderNum?,'FolderPath','IncludeFiles','v_sBranchCode' as input  and returns an folder and document collection 
    ''' Class.
    ''' </summary>
    ''' <param name="v_ifolderNumField"></param>
    ''' <param name="v_sfolderPathField"></param>
    ''' <param name="v_bincludeFilesField"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDMEFolder(ByVal v_ifolderNumField As Integer,
                                                          ByVal v_sfolderPathField As String,
                                                          ByVal v_bincludeFilesField As Boolean,
                                                         Optional ByVal v_sBranchCode As String = Nothing,
                                                         Optional ByVal v_iFilterCategoryId As Integer = 0,
                                                         Optional ByVal v_iFilterSubCategoryId As Integer = 0,
                                                         Optional ByVal v_sFilterDocName As String = Nothing) As DME
        SyncLock oLock
            Dim oGetDMEFolderRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDMEFolderQuery
            Dim oGetDMEFolderResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDMEFolderQueryResponse
            Dim oDME As NexusProvider.DME
            Dim oSubFolder As NexusProvider.SubFolder
            Dim oDocumentList As NexusProvider.DocumentList
            Dim sbLogMessage As StringBuilder

            Try
                oGetDMEFolderRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDMEFolderQuery
                oGetDMEFolderResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDMEFolderQueryResponse
                oDME = New NexusProvider.DME
                oSubFolder = New NexusProvider.SubFolder
                oDocumentList = New NexusProvider.DocumentList
                sbLogMessage = New StringBuilder



                With oGetDMEFolderRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_ifolderNumField > 0 Then
                        'use the passed parameter v_ifolderNumField
                        .FolderNum = v_ifolderNumField
                    End If

                    If Not String.IsNullOrEmpty(v_sfolderPathField) Then
                        'use the passed parameter v_sfolderPathField
                        .FolderPath = v_sfolderPathField
                    End If

                    'use the passed parameter v_bincludeFilesField
                    .IncludeFiles = v_bincludeFilesField
                    'use the passed filter parameters
                    If v_iFilterCategoryId > 0 Then
                        .FilterCategoryId = v_iFilterCategoryId
                    End If
                    If v_iFilterSubCategoryId > 0 Then
                        .FilterSubCategoryId = v_iFilterSubCategoryId
                    End If
                    If Not String.IsNullOrEmpty(v_sFilterDocName) Then
                        .FilterDocName = v_sFilterDocName
                    End If
                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDMEFolder, oGetDMEFolderRequest)
                    oGetDMEFolderResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDMEFolderQueryResponse)(result)

                End Using

                With oGetDMEFolderResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else


                    If oGetDMEFolderResponse.SubFolders IsNot Nothing Then
                        For iCount As Integer = 0 To oGetDMEFolderResponse.SubFolders.Count - 1
                            oSubFolder = New SubFolder()
                            oSubFolder.Name = oGetDMEFolderResponse.SubFolders(iCount).Name
                            oSubFolder.CreateDate = oGetDMEFolderResponse.SubFolders(iCount).CreateDate
                            oSubFolder.ExternalCode = oGetDMEFolderResponse.SubFolders(iCount).ExternalCode
                            oSubFolder.FolderLevel = oGetDMEFolderResponse.SubFolders(iCount).FolderLevel
                            oSubFolder.FolderNum = oGetDMEFolderResponse.SubFolders(iCount).FolderNum
                            oSubFolder.ParentNum = oGetDMEFolderResponse.SubFolders(iCount).ParentNum
                            oDME.SubFolder.Add(oSubFolder)
                        Next
                    End If


                    If oGetDMEFolderResponse.Documents IsNot Nothing Then
                        For jCount As Integer = 0 To oGetDMEFolderResponse.Documents.Count - 1
                            oDocumentList = New DocumentList()
                            oDocumentList.CreateDate = oGetDMEFolderResponse.Documents(jCount).CreateDate
                            oDocumentList.DocDescription = oGetDMEFolderResponse.Documents(jCount).DocDescription
                            oDocumentList.DocNum = oGetDMEFolderResponse.Documents(jCount).DocNum
                            oDocumentList.DocumentType = oGetDMEFolderResponse.Documents(jCount).DocumentType
                            oDocumentList.FolderNum = oGetDMEFolderResponse.Documents(jCount).FolderNum
                            oDocumentList.FolderPath = oGetDMEFolderResponse.Documents(jCount).FolderPath
                            oDocumentList.UploadedBy = oGetDMEFolderResponse.Documents(jCount).UploadedBy
                            oDocumentList.Category = oGetDMEFolderResponse.Documents(jCount).Category
                            oDocumentList.SubCategory = oGetDMEFolderResponse.Documents(jCount).SubCategory
                            oDME.DocumentList.Add(oDocumentList)
                        Next
                    End If

                    oDME.ParentNum = oGetDMEFolderResponse.ParentNum
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetDMEFolder executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_folderNumField = " & v_ifolderNumField.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bincludeFilesField = " & v_bincludeFilesField.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetDMEFolderRequest = Nothing
                oGetDMEFolderResponse = Nothing
            End Try

            Return oDME

        End SyncLock

    End Function

    'WPR14-MID
#Region "WPR 14"

    Public Overrides Function GetMidFiles(ByVal v_dtStartDate As Date,
                                ByVal v_dtEndDate As Date,
                                ByVal v_bFailuresOnly As Boolean,
                                ByVal v_iMIDFileKey As Integer,
                                Optional ByVal v_sBranchCode As String = Nothing) As MidFileCollection

        Dim oBaseGetMIDFilesRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetMIDFilesQuery   ' Request Type
        Dim oBaseGetMIDFilesResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetMIDFilesQueryResponse    ' Response Type

        With oBaseGetMIDFilesRequest
            'if the passed parameter v_sBranchCode is empty 
            If String.IsNullOrEmpty(v_sBranchCode) Then
                'if the branch code is NOT in session 
                If String.IsNullOrEmpty(sBranchCode) Then
                    'Use the default branch code
                    .BranchCode = sDefaultBranchCode
                Else
                    'Use the branch code in session 
                    .BranchCode = sBranchCode
                End If
            Else
                'use the passed parameter v_sBranchCode
                .BranchCode = v_sBranchCode
            End If
            .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
            If v_dtStartDate <> Date.MinValue Then
                .StartDate = v_dtStartDate
            End If

            If v_dtEndDate <> Date.MinValue Then
                .EndDate = v_dtEndDate
            End If

            .FailuresOnly = v_bFailuresOnly
            .MIDFileKey = v_iMIDFileKey

        End With

        Try
            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetMidFiles, oBaseGetMIDFilesRequest)
                oBaseGetMIDFilesResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetMIDFilesQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
        Finally
            ' Disposing the SAM's object
        End Try

        Dim oMIdFileColl As New NexusProvider.MidFileCollection

        With oBaseGetMIDFilesResponse  'With Response Type
            'If 1 = 0 Then
            'Process the error object if errors, and throw as a single exception
            'Throw New NexusException(.Errors.ToArray)
            'Else
            If .MIDFiles IsNot Nothing AndAlso .MIDFiles.Count > 0 Then

                For Each oMidFile As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetMIDFilesResponseTypeRow In .MIDFiles
                    Dim oMid As New NexusProvider.MidFile
                    oMid.DateGenerated = oMidFile.DateGenerated
                    oMid.FileSequenceNumber = oMidFile.FileSequenceNumber
                    oMid.MIDFileKey = oMidFile.MIDFileKey
                    oMid.StatusDescription = oMidFile.StatusDescription
                    oMid.FileName = oMidFile.FileName
                    oMIdFileColl.Add(oMid)
                Next
            End If
            'End If
        End With

        Dim sbLogMessage As New StringBuilder
        sbLogMessage.AppendLine("GetMidFile executed ok" & vbCrLf)

        sbLogMessage.AppendLine("Input:" & vbCrLf)
        sbLogMessage.AppendLine("start date = " & v_dtStartDate.ToShortDateString & vbCrLf)
        sbLogMessage.AppendLine("end date = " & v_dtEndDate.ToShortDateString & vbCrLf)
        sbLogMessage.AppendLine("Failures Only = " & v_bFailuresOnly.ToString & vbCrLf)

        sbLogMessage.AppendLine("Returned " & oMIdFileColl.ToString & vbCrLf)

        Dim logEntry As New LogEntry()
        logEntry.Categories.Clear()
        logEntry.Categories.Add(Category.General)
        logEntry.Priority = Priority.Normal
        logEntry.Severity = TraceEventType.Verbose
        logEntry.Message = sbLogMessage.ToString
        Logger.Write(logEntry)

        Return oMIdFileColl
    End Function

    Public Overrides Function GetMidFileDetails(ByVal v_bFailuresOnly As Boolean,
                                  ByVal v_iMIDFileKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As MidFileDetails

        Dim oBaseGetMIDFileDetailsRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetMIDFileDetailsQuery   ' Request Type
        Dim oBaseGetMIDFileDetailsResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetMIDFileDetailsQueryResponse    ' Response Type
        Dim oMidFileDetails As New NexusProvider.MidFileDetails

        With oBaseGetMIDFileDetailsRequest
            'if the passed parameter v_sBranchCode is empty 
            If String.IsNullOrEmpty(v_sBranchCode) Then
                'if the branch code is NOT in session 
                If String.IsNullOrEmpty(sBranchCode) Then
                    'Use the default branch code
                    .BranchCode = sDefaultBranchCode
                Else
                    'Use the branch code in session 
                    .BranchCode = sBranchCode
                End If
            Else
                'use the passed parameter v_sBranchCode
                .BranchCode = v_sBranchCode
            End If
            .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
            .FailuresOnly = v_bFailuresOnly
            .MIDFileKey = v_iMIDFileKey

        End With

        Try
            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetMidFileDetails, oBaseGetMIDFileDetailsRequest)
                oBaseGetMIDFileDetailsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetMIDFileDetailsQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




            With oBaseGetMIDFileDetailsResponse  'With Response Type
                'If 1 = 0 Then
                'Process the error object if errors, and throw as a single exception
                'Throw New NexusException(.Errors.ToArray)
                'Else

                oMidFileDetails.FailuresOnly = .FailuresOnly
                oMidFileDetails.FileSequenceNumber = .FileSequenceNumber

                If .Policies IsNot Nothing AndAlso .Policies.Count > 0 Then
                    Dim oMidPolicyColl As New NexusProvider.MidPolicyCollection

                    For Each oMidPolicy As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetMIDFileDetailsResponseTypeRow In .Policies
                        Dim oMid As New NexusProvider.MidPolicy

                        oMid.BatchKey = oMidPolicy.BatchKey
                        oMid.BatchRef = oMidPolicy.BatchRef
                        oMid.ExpectedPPPC = oMidPolicy.ExpectedPPPC
                        oMid.InsuranceFileKey = oMidPolicy.InsuranceFileKey
                        oMid.InsuranceFileRef = oMidPolicy.InsuranceFileRef
                        oMid.MidPolicyKey = oMidPolicy.MidPolicyKey
                        oMid.MidPolicyStatusCode = oMidPolicy.MidPolicyStatusCode
                        oMid.PPPC = oMidPolicy.PPPC
                        oMid.RejectErrorCodes = oMidPolicy.RejectErrorCodes
                        oMid.RejectReference = oMidPolicy.RejectReference
                        oMid.StatusCode = oMidPolicy.StatusCode
                        oMid.UpdateType = oMidPolicy.UpdateType

                        If oMidPolicy.Vehicles IsNot Nothing AndAlso oMidPolicy.Vehicles.Count > 0 Then
                            Dim oMidVehicleColl As New NexusProvider.MIDVehicleCollection

                            For Each oMidVehicle As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetMidFileDetailsResponseTypeRowRow In oMidPolicy.Vehicles
                                Dim oMVehicle As New NexusProvider.MIDVehicle
                                oMVehicle.IsForeignReg = oMidVehicle.IsForeignReg
                                oMVehicle.IsTradeReg = oMidVehicle.IsTradeReg
                                oMVehicle.Make = oMidVehicle.Make
                                oMVehicle.MIDPolicyKey = oMidVehicle.MIDPolicyKey
                                oMVehicle.MIDVehicleKey = oMidVehicle.MIDVehicleKey
                                oMVehicle.Model = oMidVehicle.Model
                                oMVehicle.OffDate = oMidVehicle.OffDate
                                oMVehicle.OnDate = oMidVehicle.OnDate
                                oMVehicle.Registration = oMidVehicle.Registration
                                oMVehicle.RejectErrorCodes = oMidVehicle.RejectErrorCodes
                                oMVehicle.RejectReference = oMidVehicle.RejectReference
                                oMVehicle.StatusCode = oMidVehicle.StatusCode
                                oMVehicle.UpdateType = oMidVehicle.UpdateType

                                oMidVehicleColl.Add(oMVehicle)
                            Next

                            oMid.Vehicles = oMidVehicleColl
                        End If
                        oMidPolicyColl.Add(oMid)
                    Next

                    oMidFileDetails.Policies = oMidPolicyColl

                End If
                'End If
            End With

            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("GetMidFileDetails executed ok" & vbCrLf)
            sbLogMessage.AppendLine("Input:" & vbCrLf)
            sbLogMessage.AppendLine("MID File Key = " & v_iMIDFileKey.ToString & vbCrLf)
            sbLogMessage.AppendLine("Failures Only = " & v_bFailuresOnly.ToString & vbCrLf)

            sbLogMessage.AppendLine("Returned " & oMidFileDetails.ToString & vbCrLf)

            Dim logEntry As New LogEntry()
            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString
            Logger.Write(logEntry)
            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oBaseGetMIDFileDetailsRequest = Nothing
            oBaseGetMIDFileDetailsResponse = Nothing
        End Try
        Return oMidFileDetails
    End Function

#End Region
    'END WPR14-MID

    ''' <summary>
    ''' Fetches report via SAM and stores it to the specified location
    ''' </summary>
    ''' <param name="v_sReportName">Name of the Report to be generated</param>
    ''' <param name="v_oDocumentFormatType">Type of document format requested to receive</param>
    ''' <param name="v_oParameters">Collection of parameters</param>
    ''' <param name="v_sDocumentExtractionDirectory">Name of the directory where received file need to be extract</param>
    ''' <param name="v_sBranchCode">Optional Parameter BranchCode</param>
    Public Overrides Function GetReport(ByVal v_sReportName As String,
                                   ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                   ByVal v_oParameters As ParametersCollection,
                                   ByVal v_sDocumentExtractionDirectory As String,
                                                      Optional ByVal v_sBranchCode As String = Nothing) As String
        SyncLock oLock

            Dim oGetReportRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetReportQuery
            Dim oGetReportResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetReportQueryResponse
            Dim sFileName As String = String.Empty
            Dim sbLogMessage As StringBuilder
            Dim oParameters As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetReportRequestTypeParameters

            Try
                oGetReportRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetReportQuery
                oGetReportResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetReportQueryResponse
                sbLogMessage = New StringBuilder


                With oGetReportRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .ReportName = v_sReportName

                    If String.IsNullOrEmpty(v_sDocumentExtractionDirectory) Then
                        'DocumentExtractionDirectory is mandatory to pass
                        Throw New ArgumentNullException("DocumentExtractionDirectory")
                    Else
                        'create directory with the same name if not exist
                        If Not IO.Directory.Exists(v_sDocumentExtractionDirectory) Then
                            IO.Directory.CreateDirectory(v_sDocumentExtractionDirectory)
                        End If
                    End If

                    .FormatType = v_oDocumentFormatType

                    'run the loop for parameters and make the request ready

                    .Parameters = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetReportRequestTypeParameters)
                    For iCount As Integer = 0 To v_oParameters.Count - 1
                        oParameters = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetReportRequestTypeParameters

                        oParameters.ParamName = v_oParameters(iCount).ParamNameField
                        oParameters.ParamValue = v_oParameters(iCount).ParamValueField
                        .Parameters.Add(oParameters)
                    Next
                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetReport, oGetReportRequest)
                    oGetReportResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetReportQueryResponse)(result)
                End Using

                With oGetReportResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        Dim sGUID As String = Guid.NewGuid.ToString

                        Dim file As System.IO.FileInfo = Nothing

                        'Create destination directory on the web server, if it doesn't already exist
                        IO.Directory.CreateDirectory(v_sDocumentExtractionDirectory)
                        'Create a unique zip file from the byte array retrieved from the Web Service
                        Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(v_sDocumentExtractionDirectory & "\" & sGUID & ".zip")
                        fsOutputFile.Write(.ReportDocument, 0, .ReportDocument.Length)
                        fsOutputFile.Close()

                        ''Invoke the unzip method
                        'duz1.ActionDZ = CDUnZipNET.DUZACTION.UNZIP_EXTRACT
                        Dim zipToRead As String = v_sDocumentExtractionDirectory & "\" & sGUID & ".zip"
                        Dim extractDir As String = v_sDocumentExtractionDirectory
                        Using zip As ZipFile = ZipFile.Read(zipToRead)
                            ' When extraction would overwrite an existing file, overwrite the file silently.The overwrite will happen even if the target file is marked as read-only.
                            zip.ExtractAll(extractDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                        End Using
                        'Remove zip file, as it been unzipped
                        IO.File.Delete(v_sDocumentExtractionDirectory & "\" & sGUID & ".zip")


                        'name of the source directory
                        Dim sourceDir As DirectoryInfo = New DirectoryInfo(v_sDocumentExtractionDirectory)

                        'for copy the unzip file
                        Dim sFileToCopy As String = Nothing
                        Dim sFileTargetLocation As String = Nothing


                        'find the name of the file which has been extracted, we need to return this
                        For Each file In sourceDir.GetFiles()
                            If file.Extension = ".rpt" Then
                                'set sFileName to the name of the rpt file
                                sFileName = file.Name
                                Exit For
                            End If
                            If file.Extension = ".pdf" Then
                                sFileName = file.FullName
                            End If
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReport executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    sbLogMessage.AppendLine("v_sReportName = " & v_sReportName.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oDocumentFormatType = " & v_oDocumentFormatType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oParameters = " & v_oParameters.Count & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetReportRequest = Nothing
                oGetReportResponse = Nothing
            End Try


            Return sFileName
        End SyncLock
    End Function

    ''' <summary>
    ''' This Method is created to Get the SharePointFile List
    ''' </summary>
    ''' <param name="v_sPartyShortName"></param>
    ''' <param name="v_sPolicyNumber"></param>
    ''' <param name="v_sClaimNumber"></param>
    ''' <param name="v_sFolderPath"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_bCreateFolder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetSharePointFileList(ByVal v_sPartyShortName As String,
                                                   Optional ByVal v_sPolicyNumber As String = Nothing,
                                                   Optional ByVal v_sClaimNumber As String = Nothing,
                                                   Optional ByVal v_sFolderPath As String = Nothing,
                                                   Optional ByVal v_sBranchCode As String = Nothing,
                                                   Optional ByVal v_bCreateFolder As Boolean = False) As SharepointFileList

        Dim oGetSharepointFileListRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetSharepointFileListQuery    ' Request Type
        Dim oGetSharepointFileListResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetSharepointFileListQueryResponse   ' Response Type
        Dim oSPFileListItemCollection As SharepointFileListResponseTypeItemListCollection
        Dim oSharepointFileList As SharepointFileList
        Dim oSharepointFolderPath As SharepointFolderPath
        Dim oSharepointFileListResponseTypeItemList As SharepointFileListResponseTypeItemList
        Dim oListItem As NexusProvider.SharepointFileListResponseTypeItemList
        Dim sbLogMessage As StringBuilder

        Try
            oGetSharepointFileListRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetSharepointFileListQuery
            oGetSharepointFileListResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetSharepointFileListQueryResponse
            oListItem = New NexusProvider.SharepointFileListResponseTypeItemList
            sbLogMessage = New StringBuilder


            With oGetSharepointFileListRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                'if the passed parameter v_sBranchCode is empty 
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode

                End If

                .PartyShortname = v_sPartyShortName
                .PolicyNumber = v_sPolicyNumber
                .ClaimNumber = v_sClaimNumber
                .FolderPath = v_sFolderPath
                .CreateFolder = v_bCreateFolder 'pass this flag to automatically create folders in sharepoint

            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetSharePointFileList, oGetSharepointFileListRequest)
                oGetSharepointFileListResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetSharepointFileListQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object







            With oGetSharepointFileListResponse  'With Response Type
                'If 1 = 0 Then
                'Process the error object if errors, and throw as a single exception
                'Throw New NexusException(.Errors)
                'Else
                'WorkManager Response
                'Fetching from the  SubAgents Response Collection 
                'v_sFolderPath = .FolderPath
                oSharepointFileList = New SharepointFileList

                oSharepointFolderPath = New SharepointFolderPath
                oSharepointFolderPath.FolderPath = .FolderPath

                oSharepointFileList.FolderPath = oSharepointFolderPath

                oSPFileListItemCollection = New SharepointFileListResponseTypeItemListCollection

                If .ItemList IsNot Nothing Then

                    'For Each oBaseSharepointItemList As ArrayOfBaseGetSharepointFileListResponseTypeItemListBaseGetSharepointFileListResponseTypeItemList In .ItemList
                    For iCt As Integer = 0 To .ItemList.Count - 1
                        oSharepointFileListResponseTypeItemList = New SharepointFileListResponseTypeItemList
                        oSharepointFileListResponseTypeItemList.Title = .ItemList(iCt).Title
                        oSharepointFileListResponseTypeItemList.URL = .ItemList(iCt).URL
                        oSharepointFileListResponseTypeItemList.ItemType = .ItemList(iCt).ItemType
                        oSharepointFileListResponseTypeItemList.InternalOnly = .ItemList(iCt).InternalOnly
                        oSharepointFileListResponseTypeItemList.PureUser = .ItemList(iCt).PureUser
                        oSharepointFileListResponseTypeItemList.Filename = .ItemList(iCt).Filename
                        oSharepointFileListResponseTypeItemList.CreatedDate = .ItemList(iCt).CreatedDate
                        oSharepointFileListResponseTypeItemList.LastModifiedDate = .ItemList(iCt).LastModifiedDate
                        oSharepointFileListResponseTypeItemList.DocumentTemplateGroup = .ItemList(iCt).DocumentTemplateGroup
                        oSharepointFileListResponseTypeItemList.DocumentTemplateSubGroup = .ItemList(iCt).DocumentTemplateSubGroup

                        oSPFileListItemCollection.Add(oSharepointFileListResponseTypeItemList)
                    Next
                    oSharepointFileList.ItemList = oSPFileListItemCollection
                End If
                'End If
            End With
            'oSharepointFileList.FolderPath = oSharepointFolderPath
            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("SharePoint Item List executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & oSPFileListItemCollection.Print() & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If

                sbLogMessage.AppendLine("Returned " & oSPFileListItemCollection.Print() & "results" & vbCrLf)

                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            oGetSharepointFileListRequest = Nothing
            oGetSharepointFileListResponse = Nothing
        End Try

        Return oSharepointFileList

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sDocumentTemplateCode"></param>
    ''' <param name="v_iDocumentTemplateKey"></param>
    ''' <param name="v_sTemplateExtractionDirectory"></param>
    ''' <param name="v_oDocumentFormatType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetStandardWordingsTemplate(ByVal v_sDocumentTemplateCode As String,
                                                      ByVal v_iDocumentTemplateKey As Integer,
                                                      ByVal v_sTemplateExtractionDirectory As String,
                                                      ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                                      Optional ByVal v_sBranchCode As String = Nothing,
                                                      Optional ByVal v_IsTXTextControlEnabled As Boolean = False) As String

        SyncLock oLock

            Dim oGetStandardWordingTemplateRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetStandardWordingTemplateQuery
            Dim oGetStandardWordingTemplateResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetStandardWordingTemplateQueryResponse
            Dim sFileName As String = Right(v_sTemplateExtractionDirectory, (v_sTemplateExtractionDirectory.Length - v_sTemplateExtractionDirectory.LastIndexOf("\")))
            sFileName = sFileName.Remove(0, 1)

            'Dim sFileName As String = Right(v_sTemplateExtractionDirectory, (v_sTemplateExtractionDirectory.Length - v_sTemplateExtractionDirectory.LastIndexOf("\")))
            Dim oDocumentTemplatesCollection As DocumentTemplateCollection
            Dim sReturnString As String
            Dim sZipFilePath As String = v_sTemplateExtractionDirectory & ".zip"
            Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(sZipFilePath)
            Dim Comletepath As String
            Dim sbLogMessage As StringBuilder

            Try
                oGetStandardWordingTemplateRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetStandardWordingTemplateQuery
                oGetStandardWordingTemplateResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetStandardWordingTemplateQueryResponse
                oDocumentTemplatesCollection = New DocumentTemplateCollection
                sbLogMessage = New StringBuilder

                sFileName = sFileName.Remove(0, 1)
                With oGetStandardWordingTemplateRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .DocumentTemplateCode = RTrim(v_sDocumentTemplateCode)

                    If v_iDocumentTemplateKey <> 0 Then
                        .DocumentTemplateKey = v_iDocumentTemplateKey
                        .DocumentTemplateKeySpecified = True
                    Else
                        .DocumentTemplateKeySpecified = False
                    End If
                    'Checking the Document Type
                    Select Case v_oDocumentFormatType
                        Case DocumentFormatType.HTML
                            .DocumentTemplateFormat = DocumentFormatType.HTML
                            .DocumentTemplateFormatSpecified = True
                        Case DocumentFormatType.PDF
                            .DocumentTemplateFormat = DocumentFormatType.PDF
                            .DocumentTemplateFormatSpecified = True
                        Case Else
                            .DocumentTemplateFormatSpecified = False
                    End Select
                    .IsTXTextControlEnabled = v_IsTXTextControlEnabled
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetStandardWordingTemplate, oGetStandardWordingTemplateRequest)
                    oGetStandardWordingTemplateResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetStandardWordingTemplateQueryResponse)(result)
                End Using

                With oGetStandardWordingTemplateResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)

                    'Else

                    'Create unique zip directory on the web server
                    IO.Directory.CreateDirectory(v_sTemplateExtractionDirectory)

                    'Create a unique zip file from the byte array retrieved from the Web Service

                    fsOutputFile.Write(.DocumentTemplate, 0, .DocumentTemplate.Length)
                    fsOutputFile.Close()

                    ''Invoke the unzip method

                    Using zip As ZipFile = ZipFile.Read(sZipFilePath)
                        zip.ExtractAll(v_sTemplateExtractionDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                        zip.Dispose()
                    End Using

                    'Remove zip file, as it been unzipped
                    IO.File.Delete(sZipFilePath)

                    'Remove the drive letter, and colon from the file path
                    If (Left(.MergedFilePath, 2) = "\\") Then
                        .MergedFilePath = .MergedFilePath.Remove(0, 1)
                    Else
                        .MergedFilePath = .MergedFilePath.Remove(0, 2)
                    End If

                    Dim sTemp As String = v_sTemplateExtractionDirectory & .MergedFilePath

                    Dim file As System.IO.FileInfo = Nothing
                    'name of the source directory
                    Dim sourceDir As DirectoryInfo = New DirectoryInfo(sTemp) 'duz1.Destination
                    'for copy the unzip file
                    Dim sFileToCopy As String = Nothing
                    Dim sFileTargetLocation As String = Nothing
                    Dim bIsXMLFile As Boolean = False

                    'find the name of the file which has been extracted, we need to return this
                    For Each file In sourceDir.GetFiles()
                        If file.Extension.ToUpper = ".XML" Or file.Extension.ToUpper = ".PDF" Or file.Extension.ToUpper = ".HTML" Or file.Extension.ToUpper = ".HTM" Or file.Extension.ToUpper = ".DOCX" Then
                            'set sFileName to the name of the htm file 
                            sFileName = file.Name
                            If file.Extension.ToUpper = ".XML" Then
                                bIsXMLFile = True
                            End If
                            Exit For
                        End If
                    Next

                    Comletepath = sTemp & "\" & sFileName
                    If bIsXMLFile Then
                        sReturnString = Comletepath.Replace(".xml", ".doc")
                        IO.File.Move(Comletepath, sReturnString)
                    Else
                        sReturnString = Comletepath
                    End If

                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetStandardPolicyWordings executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_iDocumentTemplateKey) Then
                        sbLogMessage.AppendLine("Document Template Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Key : " & v_iDocumentTemplateKey.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oDocumentTemplatesCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetStandardWordingTemplateRequest = Nothing
                oGetStandardWordingTemplateResponse = Nothing
                sFileName = Nothing
            End Try


            Return sReturnString
        End SyncLock
    End Function

    Public Overrides Function GetTaskGroups(Optional ByVal v_sBranchCode As String = Nothing) As TaskGroupCollection
        SyncLock oLock

            Dim oGetTaskGroupsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupsQuery
            Dim oGetTaskGroupsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupsQueryResponse
            Dim oTaskGroups As TaskGroupCollection = Nothing
            Dim oTaskGroup As TaskGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetTaskGroupsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupsQuery
                oGetTaskGroupsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupsQueryResponse
                sbLogMessage = New StringBuilder


                With oGetTaskGroupsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetTaskGroups, oGetTaskGroupsRequest)
                    oGetTaskGroupsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupsQueryResponse)(result)
                End Using






                With oGetTaskGroupsResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'End If

                    If .TaskGroups IsNot Nothing AndAlso .TaskGroups.Count > 0 Then


                        oTaskGroups = New TaskGroupCollection()
                        For Each oBaseTaskGroup As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetTaskGroupsResponseTypeRow In .TaskGroups
                            oTaskGroup = New TaskGroup()
                            With oTaskGroup
                                .CaptionId = oBaseTaskGroup.CaptionID
                                .Code = oBaseTaskGroup.Code
                                .Description = oBaseTaskGroup.Description
                                .EffectiveDate = oBaseTaskGroup.EffectiveDate
                                .IsDeleted = oBaseTaskGroup.IsDeleted
                                .TaskGroupCategoryKey = oBaseTaskGroup.TaskGroupCategoryKey
                                .TaskGroupKey = oBaseTaskGroup.TaskGroupKey
                            End With
                            oTaskGroups.Add(oTaskGroup)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTaskGroup executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oTaskGroups.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetTaskGroupsRequest = Nothing
                oGetTaskGroupsResponse = Nothing
            End Try

            Return oTaskGroups

        End SyncLock
    End Function

    Public Overrides Function GetTaskGroupTasks(ByVal v_iTaskGroupKey As Integer,
                                                ByVal v_dtEffectiveDate As Date,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaskGroupCollection
        SyncLock oLock

            Dim oGetTaskGroupTasksRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupTasksQuery
            Dim oGetTaskGroupTasksResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupTasksQueryResponse
            Dim oTaskGroupTaskCollection As TaskGroupCollection = Nothing
            'Dim oTaskGroupTasksDetails As TaskGroupTasksDetails = Nothing
            Dim oTaskGroupTask As TaskGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oGetTaskGroupTasksRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupTasksQuery
                oGetTaskGroupTasksResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupTasksQueryResponse
                sbLogMessage = New StringBuilder


                With oGetTaskGroupTasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iTaskGroupKey > 0 Then
                        .TaskGroupKey = v_iTaskGroupKey
                    Else
                        Throw New ArgumentException("TaskGroupTasks.TaskGroupKey")
                    End If

                    If v_dtEffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("TaskGroupTasks.EffectiveDate")
                    Else
                        .EffectiveDate = v_dtEffectiveDate
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()

                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetTaskGroupTasks, oGetTaskGroupTasksRequest)
                    oGetTaskGroupTasksResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskGroupTasksQueryResponse)(result)
                End Using







                With oGetTaskGroupTasksResponse
                    'oTaskGroupTasksDetails = New TaskGroupTasksDetails
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'End If

                    If .TaskGroupTasks IsNot Nothing AndAlso .TaskGroupTasks.Count > 0 Then


                        oTaskGroupTaskCollection = New TaskGroupCollection
                        For Each oBaseTaskGroupTask As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetTaskGroupTasksResponseTypeRow In .TaskGroupTasks
                            oTaskGroupTask = New TaskGroup()
                            oTaskGroupTask.TaskKey = oBaseTaskGroupTask.TaskKey
                            oTaskGroupTask.Name = oBaseTaskGroupTask.Name.Trim
                            oTaskGroupTask.EffectiveDate = oBaseTaskGroupTask.EffectiveDate
                            oTaskGroupTask.Description = oBaseTaskGroupTask.Description
                            oTaskGroupTask.IsDeleted = oBaseTaskGroupTask.IsDeleted
                            oTaskGroupTask.IsIncluded = oBaseTaskGroupTask.IsIncluded
                            oTaskGroupTask.IsViewOnly = oBaseTaskGroupTask.IsViewOnly
                            oTaskGroupTask.IsAvailable = oBaseTaskGroupTask.IsAvailable
                            oTaskGroupTask.TaskCategoryKey = oBaseTaskGroupTask.TaskCategoryKey
                            oTaskGroupTask.DisplayIcon = oBaseTaskGroupTask.DisplayIcon
                            oTaskGroupTaskCollection.Add(oTaskGroupTask)
                        Next
                        'oTaskGroupTasksDetails = New TaskGroupTasksDetails
                        'oTaskGroupTasksDetails.TaskGroupTasks = oTaskGroupTaskCollection
                        'oTaskGroupTasksDetails.TimeStamp = .TimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTaskGroupTasks executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_iTaskGroupKey) Then
                        sbLogMessage.AppendLine("Task Group Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Task Group Key : " & v_iTaskGroupKey.ToString() & vbCrLf)
                    End If

                    If IsNothing(v_dtEffectiveDate) Then
                        sbLogMessage.AppendLine("Effective Date : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Effective Date : " & v_dtEffectiveDate.ToString() & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oTaskGroupTaskCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetTaskGroupTasksRequest = Nothing
                oGetTaskGroupTasksResponse = Nothing
            End Try


            Return oTaskGroupTaskCollection
        End SyncLock
    End Function

    Public Overrides Function GetUserGroupsbyTask(ByVal v_sTaskGroupCode As String,
                Optional ByVal v_sBranchCode As String = Nothing) As UserGroupCollection
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oGetUserGroupsbyTaskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsbyTaskQuery
            Dim oGetUserGroupsbyTaskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsbyTaskQueryResponse
            Dim oUserGroupsbyTask As UserGroupCollection = Nothing
            Dim oUserGroupbyTask As UserGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oGetUserGroupsbyTaskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsbyTaskQuery
                oGetUserGroupsbyTaskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsbyTaskQueryResponse
                sbLogMessage = New StringBuilder


                With oGetUserGroupsbyTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .TaskGroupCode = v_sTaskGroupCode
                End With

                Using trace As New Tracer(Category.Trace)
                    'oGetUserGroupsbyTaskResponse = oSAM.GetUserGroupsbyTask(oGetUserGroupsbyTaskRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUserGroupsbyTask, oGetUserGroupsbyTaskRequest)
                    oGetUserGroupsbyTaskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupsbyTaskQueryResponse)(result)
                End Using

                oUserGroupsbyTask = New UserGroupCollection()

                With oGetUserGroupsbyTaskResponse


                    If .UserGroups IsNot Nothing AndAlso .UserGroups.Count > 0 Then

                        For Each oBaseUserGroupbyTask As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetUserGroupsbyTaskResponseTypeRow In .UserGroups
                            oUserGroupbyTask = New UserGroup()
                            With oUserGroupbyTask
                                .Code = oBaseUserGroupbyTask.UserGroupCode.Trim
                                .Description = oBaseUserGroupbyTask.UserGroupDescription.Trim
                                .UserGroupKey = oBaseUserGroupbyTask.UserGroupKey
                            End With
                            oUserGroupsbyTask.Add(oUserGroupbyTask)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_sTaskGroupCode) Then
                        sbLogMessage.AppendLine("TaskGroup Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("TaskGroup Code : " & v_sTaskGroupCode & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroupsbyTask.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetUserGroupsbyTaskRequest = Nothing
                oGetUserGroupsbyTaskResponse = Nothing
            End Try


            Return oUserGroupsbyTask
        End SyncLock
    End Function

    Public Overrides Function GetUserGroupTaskGroups(ByVal v_sUserGroupCode As String,
                                                         ByVal v_dtEffectiveDate As Date,
                                                         Optional ByVal v_sBranchCode As String = Nothing) As TaskGroup
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oGetUserGroupTaskGroupsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupTaskGroupsQuery
            Dim oGetUserGroupTaskGroupsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupTaskGroupsQueryResponse
            Dim oTaskGroups As TaskGroupCollection = Nothing
            Dim oUserGroupTaskGroupDetails As TaskGroup = Nothing
            Dim oTaskGroup As TaskGroup = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                ' oSAM = InitializeCoreServiceMethod()
                oGetUserGroupTaskGroupsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupTaskGroupsQuery
                oGetUserGroupTaskGroupsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupTaskGroupsQueryResponse
                sbLogMessage = New StringBuilder


                With oGetUserGroupTaskGroupsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .UserGroupCode = v_sUserGroupCode
                    If v_dtEffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("UserGroupTaskGroups.EffectiveDate")
                    Else
                        .EffectiveDate = v_dtEffectiveDate
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    'oGetUserGroupTaskGroupsResponse = oSAM.GetUserGroupTaskGroups(oGetUserGroupTaskGroupsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUserGroupTaskGroups, oGetUserGroupTaskGroupsRequest)
                    oGetUserGroupTaskGroupsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupTaskGroupsQueryResponse)(result)
                End Using


                With oGetUserGroupTaskGroupsResponse


                    If .TaskGroups IsNot Nothing AndAlso .TaskGroups.Count > 0 Then


                        oTaskGroups = New TaskGroupCollection()
                        For Each oBaseUserGroupTaskGroup As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetUserGroupTaskGroupsResponseTypeRow In .TaskGroups
                            oTaskGroup = New TaskGroup()
                            With oTaskGroup
                                .Code = oBaseUserGroupTaskGroup.Code
                                .Description = oBaseUserGroupTaskGroup.Description
                                .EffectiveDate = oBaseUserGroupTaskGroup.EffectiveDate
                                .IsDeleted = oBaseUserGroupTaskGroup.IsDeleted
                                .TaskGroupKey = oBaseUserGroupTaskGroup.TaskGroupKey
                                .DisplaySequence = oBaseUserGroupTaskGroup.DisplaySequence
                                .IsIncluded = oBaseUserGroupTaskGroup.IsIncluded
                            End With
                            oTaskGroups.Add(oTaskGroup)
                        Next
                    End If
                    oUserGroupTaskGroupDetails.TaskGroup = oTaskGroups
                    oUserGroupTaskGroupDetails.TimeStamp = .ApiTimeStamp
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroupTaskGroups executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_dtEffectiveDate) Then
                        sbLogMessage.AppendLine("Effective Date : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Effective Date : " & v_dtEffectiveDate.ToString() & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroupTaskGroupDetails.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetUserGroupTaskGroupsRequest = Nothing
                oGetUserGroupTaskGroupsResponse = Nothing
            End Try


            Return oUserGroupTaskGroupDetails

        End SyncLock
    End Function
    '     Public Overrides sub GetWmTask(ByVal r_oWorkManager As WorkManager, _
    '                                            Optional ByVal v_sBranchCode As String = Nothing)
    'For  converting the Sub to Function with TaskLogCollection as returnType.

    Public Overrides Function GetWmTask(ByVal r_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As WorkManager
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient 'SAMForInsuranceV2's Object
            Dim oGetWmTaskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskQuery ' Request Type
            Dim oGetWmTaskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskQueryResponse  ' Response Type
            Dim oNewKey As KeyData
            Dim oNewKeyDataCollection As KeyDataCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oGetWmTaskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskQuery
                oGetWmTaskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskQueryResponse
                sbLogMessage = New StringBuilder

                With oGetWmTaskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request
                    If r_oWorkManager.TaskInstanceKey = 0 Then
                        Throw New ArgumentNullException("WorkManager.TaskInstanceKey")
                    Else
                        .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                    End If
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ' oGetWmTaskResponse = oSAM.GetWmTask(oGetWmTaskRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetWmTask, oGetWmTaskRequest)
                    oGetWmTaskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oGetWmTaskResponse  'With Response Type
                    r_oWorkManager.TaskInstanceKey = oGetWmTaskResponse.TaskInstanceKey
                    r_oWorkManager.TaskGroupKey = oGetWmTaskResponse.TaskGroupKey
                    r_oWorkManager.TaskGroupCode = oGetWmTaskResponse.TaskGroupCode
                    r_oWorkManager.TaskKey = oGetWmTaskResponse.TaskKey
                    r_oWorkManager.TaskCode = oGetWmTaskResponse.TaskCode
                    r_oWorkManager.Customer = oGetWmTaskResponse.Customer
                    r_oWorkManager.DueDate = oGetWmTaskResponse.DueDate
                    r_oWorkManager.UserGroupKey = oGetWmTaskResponse.UserGroupKey
                    r_oWorkManager.UserGroupCode = oGetWmTaskResponse.UserGroupCode
                    r_oWorkManager.UserKey = oGetWmTaskResponse.UserKey

                    r_oWorkManager.UserCode = oGetWmTaskResponse.UserCode
                    r_oWorkManager.Description = oGetWmTaskResponse.Description
                    r_oWorkManager.TaskStatusKey = oGetWmTaskResponse.TaskStatusKey
                    r_oWorkManager.IsUrgent = oGetWmTaskResponse.IsUrgent
                    r_oWorkManager.IsTaskReview = oGetWmTaskResponse.IsTaskReview
                    r_oWorkManager.CreatedByKey = oGetWmTaskResponse.CreatedByKey
                    r_oWorkManager.DateCreated = oGetWmTaskResponse.DateCreated
                    r_oWorkManager.ModifiedByKey = oGetWmTaskResponse.ModifiedByKey
                    r_oWorkManager.LastModified = oGetWmTaskResponse.LastModified
                    r_oWorkManager.Isvisible = oGetWmTaskResponse.Isvisible
                    r_oWorkManager.WorkflowInformation = oGetWmTaskResponse.WorkflowInformation
                    r_oWorkManager.TimeStamp = oGetWmTaskResponse.TaskTimestamp
                    r_oWorkManager.CreatedBy = oGetWmTaskResponse.CreatedByUser
                    r_oWorkManager.ModifiedBy = oGetWmTaskResponse.ModifiedByUser

                    'Code Changed  on December 24th Begin 


                    oNewKeyDataCollection = New KeyDataCollection
                    If .KeyData IsNot Nothing AndAlso .KeyData.Count > 0 Then
                        For Each oKey As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetWmTaskResponseTypeRow In .KeyData
                            oNewKey = New KeyData
                            oNewKey.KeyName = oKey.KeyName
                            oNewKey.KeyValue = oKey.KeyValue
                            oNewKeyDataCollection.Add(oNewKey)
                        Next
                        r_oWorkManager.KeyData = oNewKeyDataCollection
                        'Code Changed  on December 24th End 
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                ' oSAM.Close()
                oGetWmTaskRequest = Nothing
                oGetWmTaskResponse = Nothing
            End Try

            Return r_oWorkManager

        End SyncLock
    End Function
    ' Public Overrides sub GetWmTaskLog(ByVal r_oWorkManager As WorkManager, _
    '                                           Optional ByVal v_sBranchCode As String = Nothing)
    ' For  converting the Sub to Function with TaskLogCollection as returnType.
    Public Overrides Function GetWmTaskLog(ByVal r_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaskLogCollection
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient 'SAMForInsuranceV2's Object
            Dim oGetWmTaskLogRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskLogQuery  ' Request Type
            Dim oGetWmTaskLogResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskLogQueryResponse   ' Response Type
            Dim oNewTaskLog As TaskLog   'Object of WorkManager Class
            Dim oNewTaskLogCollection As TaskLogCollection = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oGetWmTaskLogRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskLogQuery
                oGetWmTaskLogResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskLogQueryResponse
                sbLogMessage = New StringBuilder

                With oGetWmTaskLogRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request

                    'if r_oWorkManager.TaskInstanceKey is string.empty then 
                    If r_oWorkManager.TaskInstanceKey = 0 Then 'Changed  on 23rd December

                        Throw New ArgumentNullException("WorkManager.TaskInstanceKey")
                    Else
                        .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                    End If
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oGetWmTaskLogResponse = oSAM.GetWmTaskLog(oGetWmTaskLogRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetWmTaskLog, oGetWmTaskLogRequest)
                    oGetWmTaskLogResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetWmTaskLogQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object



                oNewTaskLogCollection = New TaskLogCollection

                With oGetWmTaskLogResponse  'With Response Type

                    'WorkManager Response
                    'Fetching from the  WorkManager Response Collection 
                    'Code Changed  on December 24th Begin 
                    If .TaskLog IsNot Nothing AndAlso .TaskLog.Count > 0 Then
                        For Each oTask As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetWmTaskLogResponseTypeTaskLogRow In .TaskLog
                            oNewTaskLog = New NexusProvider.TaskLog
                            oNewTaskLog.CreatedByKey = oTask.CreatedByKey
                            oNewTaskLog.DateCreated = oTask.DateCreated
                            oNewTaskLog.LogText = oTask.LogText
                            oNewTaskLog.TaskInstanceKey = oTask.TaskInstanceKey
                            oNewTaskLog.UserName = oTask.UserName
                            oNewTaskLogCollection.Add(oNewTaskLog)
                            'Code Changed  on December 24th End 
                        Next
                    End If


                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetWmTaskLogRequest = Nothing
                oGetWmTaskLogResponse = Nothing
            End Try


            Return oNewTaskLogCollection
        End SyncLock
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sbLogMessage"></param>
    ''' <param name="v_Category"></param>
    ''' <param name="v_Priority"></param>
    ''' <param name="v_Severity"></param>
    ''' <remarks></remarks>
    Private Sub LogMessageEntry(ByVal sbLogMessage As StringBuilder, Optional ByVal v_Category As String = Category.General,
                                Optional ByVal v_Priority As Integer = Priority.Normal, Optional ByVal v_Severity As TraceEventType = TraceEventType.Verbose)

        Dim logEntry As New LogEntry()
        logEntry.Categories.Clear()
        logEntry.Categories.Add(v_Category)
        logEntry.Priority = v_Priority
        logEntry.Severity = v_Severity
        logEntry.Message = sbLogMessage.ToString

        Logger.Write(logEntry)

    End Sub

    Public Overrides Sub ReAssignMultipleWMTasks(ByVal v_oWorkManagerCollection As WorkManagerCollection,
                                                           Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oReAssignMultipleWMTasksRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ReAssignMultipleWmTasksCommand
            Dim oReAssignMultipleWMTasksResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ReAssignMultipleWmTasksCommandResponse
            Dim iCounter As Integer
            Dim oReAssignMultipleWmTasks(v_oWorkManagerCollection.Count) As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseReAssignMultipleWmTasksRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oReAssignMultipleWMTasksRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ReAssignMultipleWmTasksCommand
                oReAssignMultipleWMTasksResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ReAssignMultipleWmTasksCommandResponse
                sbLogMessage = New StringBuilder


                With oReAssignMultipleWMTasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    End If


                    For iCounter = 0 To v_oWorkManagerCollection.Count - 1
                        oReAssignMultipleWmTasks(iCounter) = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseReAssignMultipleWmTasksRequestTypeRow

                        If (v_oWorkManagerCollection(iCounter).TaskInstanceKey > 0) Then
                            oReAssignMultipleWmTasks(iCounter).TaskInstanceKey = v_oWorkManagerCollection(iCounter).TaskInstanceKey
                        Else
                            Throw New ArgumentNullException("ReAssignMultipleWmTasks.TaskInstanceKey")
                        End If

                        oReAssignMultipleWmTasks(iCounter).UserCode = v_oWorkManagerCollection(iCounter).UserCode
                        oReAssignMultipleWmTasks(iCounter).UserGroupCode = v_oWorkManagerCollection(iCounter).UserGroupCode

                    Next

                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.ReassignWMTasks, oReAssignMultipleWMTasksRequest)
                    oReAssignMultipleWMTasksResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ReAssignMultipleWmTasksCommandResponse)(result)
                End Using

                With oReAssignMultipleWMTasksResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ReAssignMultipleWMTasksResponse executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(v_oWorkManagerCollection.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oReAssignMultipleWMTasksRequest = Nothing
                oReAssignMultipleWMTasksResponse = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oUpdateCoverNoteBookRequest As UpdateCoverNoteBookRequestType
            'Dim oUpdateCoverNoteBookResponse As UpdateCoverNoteBookResponseType
            'Dim oCoverNoteProduct As BaseCoverNoteBookTypeRow
            Dim iCounter As Integer
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oUpdateCoverNoteBookRequest = New UpdateCoverNoteBookRequestType
                'oUpdateCoverNoteBookResponse = New UpdateCoverNoteBookResponseType
                sbLogMessage = New StringBuilder


                'With oUpdateCoverNoteBookRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode

                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode

                '    End If
                '    If r_oCoverNote.AgentKey > 0 Then
                '        .AgentKey = r_oCoverNote.AgentKey
                '        .AgentKeySpecified = True
                '    Else
                '        .AgentKeySpecified = False
                '    End If
                '    If r_oCoverNote.CoverNoteBookKey > 0 Then
                '        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                '    Else
                '        Throw New ArgumentNullException("CoverNoteBookKey")
                '    End If
                '    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                '    .CoverNoteBranchCode = r_oCoverNote.CoverNoteBranchCode


                '    .CoverNoteProducts = New List(Of BaseCoverNoteBookTypeRow)
                '    For iCounter = 0 To r_oCoverNote.CoverNoteBookProducts.Count - 1
                '        oCoverNoteProduct = New BaseCoverNoteBookTypeRow
                '        oCoverNoteProduct.ProductCode = r_oCoverNote.CoverNoteBookProducts(iCounter).ProductCode
                '        .CoverNoteProducts.Add(oCoverNoteProduct)
                '    Next

                '    .CoverNoteStatusCode = r_oCoverNote.CoverNoteStatusCode
                '    .EffectiveDate = r_oCoverNote.EffectiveDate

                '    'Checking the CoverNoteBookKey


                'End With


                'Using trace As New Tracer(Category.Trace)
                '    oUpdateCoverNoteBookResponse = oSAM.UpdateCoverNoteBook(oUpdateCoverNoteBookRequest)
                'End Using

                'With oUpdateCoverNoteBookResponse
                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else
                '        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                '    End If
                'End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateCoverNoteBook executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    '  sbLogMessage.AppendLine("v_iCoverNoteBookKey" & v_iCoverNoteBookKey.ToString() & vbCrLf)
                    ' sbLogMessage.AppendLine("v_bCoverNoteBookTimestamp" & v_bCoverNoteBookTimestamp.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    'to complete
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oUpdateCoverNoteBookRequest = Nothing
                'oUpdateCoverNoteBookResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oCoverNote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            'Dim oUpdateCoverNoteSheetRequest As UpdateCoverNoteSheetRequestType
            'Dim oUpdateCoverNoteSheetResponse As UpdateCoverNoteSheetResponseType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oUpdateCoverNoteSheetRequest = New UpdateCoverNoteSheetRequestType
                'oUpdateCoverNoteSheetResponse = New UpdateCoverNoteSheetResponseType
                sbLogMessage = New StringBuilder


                'With oUpdateCoverNoteSheetRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode

                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode
                '        End If
                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode
                '    End If
                '    'Checking the CoverNoteBookKey
                '    If r_oCoverNote.CoverNoteBookKey > 0 Then
                '        .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey
                '    Else
                '        Throw New ArgumentNullException("CoverNote.CoverNoteBookKey")
                '    End If

                '    'Checking the OldCoverNoteSheetNumber
                '    .OldCoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).OldCoverNoteSheetNumber


                '    'Checking the NewCoverNoteSheetNumber
                '    .NewCoverNoteSheetNumber = r_oCoverNote.CoverNoteSheets(0).NewCoverNoteSheetNumber

                '    .CoverNoteBookKey = r_oCoverNote.CoverNoteBookKey

                '    If r_oCoverNote.InsuranceFileDetails.InsuranceFileCnt > 0 Then
                '        .InsuranceFileCnt = r_oCoverNote.InsuranceFileDetails.InsuranceFileCnt
                '        .InsuranceFileCntSpecified = True
                '    Else
                '        .InsuranceFileCntSpecified = False
                '    End If

                '    .AssignedDate = r_oCoverNote.AssignedDate
                '    .CoverNoteStatusCode = r_oCoverNote.CoverNoteStatusCode
                '    .Comments = r_oCoverNote.Comments
                '    .CoverNoteBookTimestamp = r_oCoverNote.CoverNoteBookTimestamp
                'End With


                'Using trace As New Tracer(Category.Trace)
                '    oUpdateCoverNoteSheetResponse = oSAM.UpdateCoverNoteSheet(oUpdateCoverNoteSheetRequest)
                'End Using

                'With oUpdateCoverNoteSheetResponse
                '    If .Errors IsNot Nothing Then
                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)
                '    Else

                '        r_oCoverNote.CoverNoteBookTimestamp = .CoverNoteBookTimestamp
                '    End If
                'End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateCoverNoteSheet executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteBookKey" & r_oCoverNote.CoverNoteBookKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("r_oCoverNote.CoverNoteSheets.CoverNoteSheetNumber" & r_oCoverNote.CoverNoteSheets(0).CoverNoteSheetNumber.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oUpdateCoverNoteSheetRequest = Nothing
                'oUpdateCoverNoteSheetResponse = Nothing
            End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sDocumentTemplateCode"></param>
    ''' <param name="v_iDocumentTemplateKey"></param>
    ''' <param name="v_sUpdatedTemplateLocation"></param>
    ''' <param name="v_sTempFileLocation"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateStandardWordingsTemplate(ByVal v_sDocumentTemplateCode As String,
                                                             ByVal v_iDocumentTemplateKey As Integer,
                                                             ByVal v_sUpdatedTemplateLocation As String,
                                                             ByVal v_sTempFileLocation As String,
                                                             ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                                             Optional ByVal v_sBranchCode As String = Nothing,
                                                             Optional ByVal v_IsTxTextControlEnabled As Boolean = False) As DocumentTemplate

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oUpdateStandardWordingTemplateRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateStandardWordingTemplateCommand
            Dim oUpdateStandardWordingTemplateResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateStandardWordingTemplateCommandResponse
            Dim filepathName1 As String = v_sUpdatedTemplateLocation
            Dim dir1 As New DirectoryInfo(filepathName1)
            Dim Folder1Files As FileInfo() = dir1.GetFiles()
            Dim sfilename As String

            Dim oDocumentTemplate As DocumentTemplate
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oUpdateStandardWordingTemplateRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateStandardWordingTemplateCommand
                oUpdateStandardWordingTemplateResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateStandardWordingTemplateCommandResponse
                oDocumentTemplate = New DocumentTemplate
                sbLogMessage = New StringBuilder


                With oUpdateStandardWordingTemplateRequest
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If




                    If IO.Directory.Exists(v_sUpdatedTemplateLocation) Then

                        If Folder1Files.Length > 0 Then
                            For Each aFile As FileInfo In Folder1Files
                                sfilename = aFile.ToString()
                            Next
                        End If

                        If sfilename <> "" Then
                            v_sUpdatedTemplateLocation = v_sUpdatedTemplateLocation + "\" + sfilename
                        End If

                        Dim sourceDir As DirectoryInfo = New DirectoryInfo(v_sUpdatedTemplateLocation)
                        'for copy the unzip file
                        Dim sFileToCopy As String = Nothing
                        Dim sFileTargetLocation As String = Nothing
                        Dim sTempFileLocation As String = v_sTempFileLocation & "\"
                        Dim strText As String
                        strText = Replace(v_sUpdatedTemplateLocation, sTempFileLocation, "")
                        Dim astrSplitItems As String() = Split(strText, "\")
                        Dim sOldGUID As String
                        Dim intX As Integer
                        For intX = 0 To UBound(astrSplitItems)
                            If intX = 0 Then
                                sOldGUID = astrSplitItems(intX)
                                Exit For
                            End If
                        Next
                        sTempFileLocation = sTempFileLocation & sOldGUID

                        Dim sGUID As String = Guid.NewGuid.ToString

                        'Use the UniqueID for the Zip folder and file names
                        Dim sTempFilePath As String = sTempFileLocation & "\"
                        Dim sZipFilePath As String = v_sTempFileLocation & "\" & sGUID & ".zip"

                        Dim zip As New ZipFile

                        zip.AddDirectory(sTempFilePath)
                        zip.TempFileFolder = sTempFileLocation & "\"
                        zip.Save(sZipFilePath)
                        zip.Dispose()


                        ''Use the UniqueID for the Zip folder and file names
                        'duz1.ZipSubOptions = CDZipNET.ZIPSUBOPTION.ZSO_RELATIVEPATHFLAG
                        'duz1.TempPath = sTempFileLocation & "\"
                        'duz1.ZIPFile = v_sTempFileLocation & "\" & sGUID & ".zip"
                        'duz1.ItemList = sTempFileLocation & "\*.*"
                        'duz1.ExcludeFollowing = sTempFileLocation
                        'duz1.ExcludeFollowingFlag = True
                        'duz1.RecurseFlag = True
                        'duz1.LargeZIPFilesFlag = True
                        'duz1.NoDirectoryEntriesFlag = False
                        'duz1.NoDirectoryNamesFlag = False
                        ''Invoke the zip method
                        'duz1.ActionDZ = CDZipNET.DZACTION.ZIP_ADD

                        'Create a unique zip file from the byte array retrieved from the Web Service
                        Try
                            Dim fsInputFile As IO.FileStream = New IO.FileStream(sZipFilePath, FileMode.Open, FileAccess.Read)
                            Dim bytes() As Byte = New Byte((fsInputFile.Length) - 1) {}
                            fsInputFile.Read(bytes, 0, fsInputFile.Length)
                            fsInputFile.Close()

                            ''Remove zip file, as it been unzipped
                            IO.File.Delete(sZipFilePath)

                            .DocumentTemplate = bytes
                        Catch ex As Exception

                        End Try
                    End If

                    .DocumentTemplateCode = RTrim(v_sDocumentTemplateCode)

                    If v_iDocumentTemplateKey <> 0 Then
                        .DocumentTemplateKey = v_iDocumentTemplateKey
                        .DocumentTemplateKeySpecified = True
                    Else
                        .DocumentTemplateKeySpecified = False
                    End If
                    'Checking the Document Type
                    Select Case v_oDocumentFormatType
                        Case DocumentFormatType.HTML
                            .DocumentTemplateFormat = DocumentFormatType.HTML
                            .DocumentTemplateFormatSpecified = True
                        Case DocumentFormatType.PDF
                            .DocumentTemplateFormat = DocumentFormatType.PDF
                            .DocumentTemplateFormatSpecified = True
                        Case Else
                            .DocumentTemplateFormatSpecified = False
                    End Select

                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    ' .WCFSecurityToken = SecurityToken()
                    .IsTXTextControlEnabled = v_IsTxTextControlEnabled
                End With


                Using trace As New Tracer(Category.Trace)
                    'oUpdateStandardWordingTemplateResponse = oSAM.UpdateStandardWordingTemplate(oUpdateStandardWordingTemplateRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateStandardWordingsTemplate, oUpdateStandardWordingTemplateRequest)
                    oUpdateStandardWordingTemplateResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateStandardWordingTemplateCommandResponse)(result)
                End Using


                With oUpdateStandardWordingTemplateResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oDocumentTemplate.Code = .NewDocumentTemplateCode
                        oDocumentTemplate.DocumentTemplateId = .NewDocumentTemplateKey
                        oDocumentTemplate.Description = .NewDocumentTemplateDescription
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateStandardWordingTemplate executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_iDocumentTemplateKey) Then
                        sbLogMessage.AppendLine("Document Template Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Key : " & v_iDocumentTemplateKey.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    If IsNothing(oDocumentTemplate.DocumentTemplateId) Then
                        sbLogMessage.AppendLine("Document Template Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Key : " & oDocumentTemplate.DocumentTemplateId.ToString & vbCrLf)
                    End If

                    If IsNothing(oDocumentTemplate.Code) Then
                        sbLogMessage.AppendLine("Document Template Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Document Template Code : " & oDocumentTemplate.Code & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oUpdateStandardWordingTemplateRequest = Nothing
                oUpdateStandardWordingTemplateResponse = Nothing
            End Try


            Return oDocumentTemplate
        End SyncLock
    End Function

    Public Overrides Sub UpdateTaskGroups(ByRef oTaskGroup As TaskGroup,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oUpdatetaskgrouptasksRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupsCommand  ' Request Type
            Dim oUpdatetaskgrouptasksResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupsCommandResponse   ' Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oUpdatetaskgrouptasksRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupsCommand
                oUpdatetaskgrouptasksResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupsCommandResponse
                sbLogMessage = New StringBuilder


                With oUpdatetaskgrouptasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    If oTaskGroup.TaskGroupKey = 0 Then
                        Throw New ArgumentNullException("WorkManager")
                    Else
                        .TaskGroupKey = oTaskGroup.TaskGroupKey
                        .Code = oTaskGroup.Code
                        .Description = oTaskGroup.Description
                        .CaptionId = oTaskGroup.CaptionId
                        .IsDeleted = oTaskGroup.IsDeleted
                        .TaskGroupCategoryKey = oTaskGroup.TaskGroupCategoryKey
                        .EffectiveDate = oTaskGroup.EffectiveDate
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateTaskGroups, oUpdatetaskgrouptasksRequest)
                    oUpdatetaskgrouptasksResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupsCommandResponse)(result)

                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatetaskgrouptasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        'No Response Object..
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oTaskGroup.Print() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oTaskGroup.Print() & "results" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdatetaskgrouptasksRequest = Nothing
                oUpdatetaskgrouptasksResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub UpdateTaskGroupTasks(ByRef oTaskGroup As TaskGroup,
                                              Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock
            Dim oUpdatetaskgrouptasksRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupTasksCommand    ' Request Type
            Dim oUpdatetaskgrouptasksResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupTasksCommandResponse    ' Response Type
            Dim oNewKey As Task
            Dim sbLogMessage As StringBuilder

            Try

                oUpdatetaskgrouptasksRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupTasksCommand
                oUpdatetaskgrouptasksResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupTasksCommandResponse
                sbLogMessage = New StringBuilder


                With oUpdatetaskgrouptasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    If oTaskGroup.TaskGroupKey = String.Empty Then
                        Throw New ArgumentNullException("WorkManager")
                    Else
                        .TaskGroupKey = oTaskGroup.TaskGroupKey
                        .ApiTimeStamp = oTaskGroup.TimeStamp


                        oNewKey = New Task
                        .Tasks = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateTaskGroupTasksRequestTypeRow)
                        For Each oGroup As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateTaskGroupTasksRequestTypeRow In .Tasks
                            oGroup.TaskCode = oNewKey.TaskCode
                            oGroup.DisplaySequence = oNewKey.DisplaySequence
                            oGroup.DisplaySequenceSpecified = oNewKey.DisplaySequenceSpecified
                        Next
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateTaskGroupsTasks, oUpdatetaskgrouptasksRequest)
                    oUpdatetaskgrouptasksResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskGroupTasksCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatetaskgrouptasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        'oUpdatetaskgrouptasksResponse.TimeStamp
                        oTaskGroup.TimeStamp = .ApiTimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & oTaskGroup.Print() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oTaskGroup.Print() & "results" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdatetaskgrouptasksRequest = Nothing
                oUpdatetaskgrouptasksResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub UpdateUserGroup(ByRef r_oWorkManager As WorkManager,
                                        Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oUpdateUserGroupRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupCommand   ' Request Type
            Dim oUpdateUserGroupResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupCommandResponse   ' Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oUpdateUserGroupRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupCommand
                oUpdateUserGroupResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupCommandResponse
                sbLogMessage = New StringBuilder


                With oUpdateUserGroupRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    'WorkManager Request
                    If r_oWorkManager.UserGroupKey = 0 Then
                        Throw New ArgumentNullException("UserGroupKey")
                    Else
                        .UserGroupKey = r_oWorkManager.UserGroupKey
                        .Code = r_oWorkManager.Code
                        .Description = r_oWorkManager.Description
                        .EffectiveDate = r_oWorkManager.EffectiveDate
                        .IsDeleted = r_oWorkManager.IsDeleted
                        .IsSysAdmin = r_oWorkManager.IsSysAdmin
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateUserGroups, oUpdateUserGroupRequest)
                    oUpdateUserGroupResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdateUserGroupResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the UserGroup Response Collection 
                        'No Response Object..
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdateUserGroupRequest = Nothing
                oUpdateUserGroupResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Function UpdateUsersGroupUsers(ByVal v_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As WorkManager

        'Dim oSAM As PureCoreServiceClient 'SAMForInsuranceV2's Object
        Dim oUpdateUserGroupUsersRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupUsersCommand  ' Request Type
        Dim oUpdateUserGroupUsersResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupUsersCommandResponse  ' Response Type
        Dim v_oNewWorkManager As WorkManager  'Object of WorkManager Class
        Dim sbLogMessage As StringBuilder

        Try
            'oSAM = InitializeCoreServiceMethod()
            oUpdateUserGroupUsersRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupUsersCommand
            oUpdateUserGroupUsersResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupUsersCommandResponse
            sbLogMessage = New StringBuilder


            With oUpdateUserGroupUsersRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '.WCFSecurityToken = SecurityToken()
                'if the passed parameter v_sBranchCode is empty 
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode

                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode
                End If
                'WorkManager Request
                If v_oWorkManager.TaskInstanceKey = String.Empty Then
                    Throw New ArgumentNullException("WorkManager.TaskInstanceKey")
                Else
                    .ApiTimeStamp = v_oWorkManager.TimeStamp
                    .UserGroupKey = v_oWorkManager.UserGroupKey
                    .Users = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateUserGroupUsersRequestTypeUsersRow)
                    For Each v_oUser As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateUserGroupUsersRequestTypeUsersRow In .Users ' BaseUpdateUserGroupUsersRequestTypeRow In .Users

                        v_oUser.DisplaySequence = v_oWorkManager.DisplaySequence
                        v_oUser.DisplaySequenceSpecified = v_oWorkManager.DisplaySequenceSpecified
                        v_oUser.IsSupervisor = v_oWorkManager.IsSupervisor
                        v_oUser.UserKey = v_oWorkManager.UserKey
                    Next

                End If
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                'oUpdateUserGroupUsersResponse = oSAM.UpdateUserGroupUsers(oUpdateUserGroupUsersRequest)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateUsersGroupUsers, oUpdateUserGroupUsersRequest)
                oUpdateUserGroupUsersResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateUserGroupUsersCommandResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            With oUpdateUserGroupUsersResponse  'With Response Type
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else

                    'WorkManager Response
                    'Fetching from the  WorkManager Response Collection
                    v_oNewWorkManager = New NexusProvider.WorkManager
                    v_oNewWorkManager.TimeStamp = oUpdateUserGroupUsersResponse.ApiTimeStamp
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & v_oWorkManager.Print() & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If

                sbLogMessage.AppendLine("Returned " & v_oNewWorkManager.Print() & " results" & vbCrLf)

                LogMessageEntry(sbLogMessage)
            End If

            'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            'FaultErrorHandler(ex) ' handling fault error messages 

        Catch ex As Exception
            Throw
        Finally
            'oSAM.Close()
            oUpdateUserGroupUsersRequest = Nothing
            oUpdateUserGroupUsersResponse = Nothing
        End Try


        Return v_oNewWorkManager  'Returning WorkManager Collection

    End Function

    Public Overrides Sub UpdateWmTask(ByRef r_oWorkManager As WorkManager,
                                         Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oUpdatewmTasktasksRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateWmTaskCommand ' Request Type
            Dim oUpdatewmTasktasksResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateWmTaskCommandResponse ' Response Type
            Dim iKeyDataCount As Integer
            Dim oNewKey(r_oWorkManager.KeyData.Count) As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateWmTaskRequestTypeRow
            Dim sbLogMessage As StringBuilder

            Try
                oUpdatewmTasktasksRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateWmTaskCommand
                oUpdatewmTasktasksResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateWmTaskCommandResponse
                sbLogMessage = New StringBuilder


                With oUpdatewmTasktasksRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request
                    If r_oWorkManager.TaskInstanceKey = 0 Then
                        Throw New ArgumentNullException("WorkManager")
                    Else
                        .TaskInstanceKey = r_oWorkManager.TaskInstanceKey
                        .DueDate = r_oWorkManager.DueDate
                        .Client = r_oWorkManager.Client
                        .Description = r_oWorkManager.Description
                        .Urgent = r_oWorkManager.IsUrgentForUpdate
                        .TaskStatusKey = r_oWorkManager.TaskStatusKey
                        .UserGroupCode = r_oWorkManager.UserGroupCode
                        .UserCode = r_oWorkManager.UserCode
                        .TaskTimeStamp = r_oWorkManager.TaskTimeStamp
                        .ActionType = WMActionType.Update



                        For iKeyDataCount = 0 To r_oWorkManager.KeyData.Count - 1
                            oNewKey(iKeyDataCount) = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateWmTaskRequestTypeRow
                            oNewKey(iKeyDataCount).KeyName = r_oWorkManager.KeyData(iKeyDataCount).KeyName
                            oNewKey(iKeyDataCount).KeyValue = r_oWorkManager.KeyData(iKeyDataCount).KeyValue
                        Next

                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateWmTask, oUpdatewmTasktasksRequest)
                    oUpdatewmTasktasksResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateWmTaskCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatewmTasktasksResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 
                        r_oWorkManager.TaskTimeStamp = .TaskTimeStamp
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & r_oWorkManager.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oWorkManager.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdatewmTasktasksRequest = Nothing
                oUpdatewmTasktasksResponse = Nothing
            End Try

        End SyncLock
    End Sub

    Public Overrides Function RunDefaultRulesAdd(ByVal v_sScreenCode As String,
                                             ByVal v_sXMLDataset As String,
                                             Optional ByVal v_sBranchCode As String = Nothing,
                                             Optional ByVal v_bSkipDaveToDB As Boolean = True) As String

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oRunDefaultRulesRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesAddCommand
            Dim oRunDefaultRulesResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesAddCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oRunDefaultRulesRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesAddCommand
                oRunDefaultRulesResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesAddCommandResponse
                sbLogMessage = New StringBuilder


                With oRunDefaultRulesRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If


                    .ScreenCode = v_sScreenCode.ToUpper()
                    .XMLDataSet = v_sXMLDataset

                    'as per nexus requirement we  need not to save the data to v_bSkipDaveToDB must be true
                    .SkipSaveToDB = v_bSkipDaveToDB

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oRunDefaultRulesResponse = oSAM.RunDefaultRulesAdd(oRunDefaultRulesRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.RunDefaultRulesAdd, oRunDefaultRulesRequest)
                    oRunDefaultRulesResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesAddCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                If oRunDefaultRulesResponse.RunDefaultRulesAddResponse IsNot Nothing Then
                    For Each oRunDef As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseRunDefaultRulesAddResponseType In oRunDefaultRulesResponse.RunDefaultRulesAddResponse
                        RunDefaultRulesAdd = oRunDef.XMLDataSet
                    Next
                End If


                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("RunDefaultRulesAdd executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sScreenCode = " & v_sScreenCode.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sXMLDataset = " & v_sXMLDataset.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("v_bSkipDaveToDB = " & v_bSkipDaveToDB.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & RunDefaultRulesAdd.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oRunDefaultRulesRequest = Nothing
                oRunDefaultRulesResponse = Nothing
            End Try


            'Return RunDefaultRulesAdd

        End SyncLock

    End Function

    Public Overrides Function RunDefaultRulesEdit(ByVal v_sScreenCode As String,
                                                    ByVal v_sXMLDataset As String,
                                                    Optional ByVal v_oPerilSummary As PerilSummary = Nothing,
                                                    Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal v_sTransactionType As String = Nothing,
                                                    Optional ByVal v_bSkipSaveToDB As Boolean = False,
                                                    Optional ByVal nClaimPerilKey As Integer = 0,
                                                    Optional ByVal v_dtInceptionDateTPI As Date = Nothing) As String

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oRunDefaultRulesRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesEditCommand
            Dim oRunDefaultRulesResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesEditCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oRunDefaultRulesRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesEditCommand
                oRunDefaultRulesResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesEditCommandResponse
                sbLogMessage = New StringBuilder


                With oRunDefaultRulesRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_oPerilSummary IsNot Nothing Then
                        If v_oPerilSummary.ClaimKey > 0 Then
                            .ClaimKey = v_oPerilSummary.ClaimKey
                            .ClaimKeySpecified = True
                        Else
                            .ClaimKeySpecified = False
                        End If
                        If v_oPerilSummary.ClaimPerilKey > 0 Then
                            .ClaimPerilKey = v_oPerilSummary.ClaimPerilKey
                            .ClaimPerilKeySpecified = True
                        Else
                            .ClaimPerilKeySpecified = False
                        End If
                    ElseIf nClaimPerilKey > 0 Then
                        .ClaimKey = nClaimPerilKey
                        .ClaimKeySpecified = True
                    End If

                    .ScreenCode = v_sScreenCode.ToUpper()
                    .XMLDataSet = v_sXMLDataset
                    .TransactionType = v_sTransactionType
                    .SkipSaveToDB = v_bSkipSaveToDB
                    .InceptionDateTPI = v_dtInceptionDateTPI

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oRunDefaultRulesResponse = oSAM.RunDefaultRulesEdit(oRunDefaultRulesRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.RunDefaultRulesEdit, oRunDefaultRulesRequest)
                    oRunDefaultRulesResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.RunDefaultRulesEditCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oRunDefaultRulesResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        RunDefaultRulesEdit = .XMLDataSet
                    End If

                End With


                sbLogMessage.AppendLine("RunDefaultRulesEdit executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_sScreenCode = " & v_sScreenCode.ToString & vbCrLf)
                sbLogMessage.AppendLine("v_sXMLDataset = " & v_sXMLDataset.ToString & vbCrLf)
                If v_oPerilSummary IsNot Nothing Then
                    sbLogMessage.AppendLine("iClaimKey = " & v_oPerilSummary.ClaimKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("iClaimPerilKey = " & v_oPerilSummary.ClaimPerilKey.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("iClaimKey = nothing" & vbCrLf)
                    sbLogMessage.AppendLine("iClaimPerilKey =  nothing" & vbCrLf)
                End If

                If Logger.IsLoggingEnabled Then
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If


                    sbLogMessage.AppendLine("Returned " & vbCrLf & RunDefaultRulesEdit.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oRunDefaultRulesRequest = Nothing
                oRunDefaultRulesResponse = Nothing
            End Try


            Return RunDefaultRulesEdit

        End SyncLock

    End Function

#Region "WPR28"


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCaseKey"></param>
    ''' <param name="v_oCaseNumber"></param>
    ''' <param name="v_oCaseOpenDate"></param>
    ''' <param name="v_oAssistant"></param>
    ''' <param name="v_oAnalyst"></param>
    ''' <param name="v_oProgressStatusCode"></param>
    ''' <param name="v_oEventDescription"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetCaseDetails(Optional ByVal v_oCaseKey As Integer = 0,
                                    Optional ByVal v_oCaseNumber As String = Nothing,
                                    Optional ByVal v_oCaseOpenDate As Date = Nothing,
                                    Optional ByVal v_oAssistant As String = Nothing,
                                    Optional ByVal v_oAnalyst As String = Nothing,
                                    Optional ByVal v_oProgressStatusCode As String = Nothing,
                                    Optional ByVal v_oEventDescription As String = Nothing,
                                    Optional ByVal v_sBranchCode As String = Nothing) As CaseDetails

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient 'SAMForInsuranceV2's Object
            Dim oCaseItemsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCaseDetailsQuery = Nothing ' Request Type
            Dim oCaseItemsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCaseDetailsQueryResponse = Nothing   ' Response Type
            Dim sbLogMessage As StringBuilder = Nothing
            Dim oCaseDetails As NexusProvider.CaseDetails = Nothing
            Try
                'oSAM = InitializeCoreServiceMethod()
                oCaseItemsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCaseDetailsQuery
                oCaseItemsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCaseDetailsQueryResponse
                With oCaseItemsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Analyst = v_oAnalyst
                    .Assistant = v_oAssistant
                    .CaseNumber = v_oCaseNumber
                    If (v_oCaseKey > 0) Then
                        .CaseKey = v_oCaseKey
                        .CaseKeySpecified = True
                    Else
                        .CaseKeySpecified = False
                    End If

                    .CaseOpenDate = v_oCaseOpenDate
                    .ProgressStatusCode = v_oProgressStatusCode
                    .EventDescription = v_oEventDescription
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oCaseItemsResponse = oSAM.GetCaseDetails(oCaseItemsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetCaseDetails, oCaseItemsRequest)
                    oCaseItemsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCaseDetailsQueryResponse)(result)

                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                oCaseDetails = New NexusProvider.CaseDetails
                With oCaseItemsResponse  'With Response Type


                    oCaseDetails.CaseKey = .CaseKey
                    oCaseDetails.CaseNumber = .CaseNumber
                    oCaseDetails.Analyst = .Analyst
                    oCaseDetails.Assistant = .Assistant
                    oCaseDetails.CaseOpenDate = .CaseOpenedDate
                    oCaseDetails.BaseCaseKey = .BaseCaseKey
                    oCaseDetails.ProgressStatusCode = .CaseProgressStatusCode
                    oCaseDetails.ClaimVersion = .CaseVersion + 1
                    If .LinkedClaims IsNot Nothing Then
                        Dim oLinkCaseCollection As New NexusProvider.LinkedClaimCollection
                        For Each oLinkedClaims As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCaseItemsResponseTypeLinkedClaimsRow In .LinkedClaims

                            Dim oCDetails As New NexusProvider.CaseDetails
                            oCDetails.ClaimKey = oLinkedClaims.ClaimKey
                            oCDetails.ClaimNumber = oLinkedClaims.ClaimNumber
                            oCDetails.LossDate = oLinkedClaims.LossDate
                            oCDetails.ClaimHandlerDescription = oLinkedClaims.ClaimHandler
                            oCDetails.RiskType = oLinkedClaims.RiskType
                            oCDetails.ClaimStatus = oLinkedClaims.Status
                            oCDetails.InsuranceFileKey = oLinkedClaims.InsuranceFileKey

                            oLinkCaseCollection.Add(oCDetails)
                        Next
                        oCaseDetails.LinkedClaims = oLinkCaseCollection
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("GetCaseDetails executed ok" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oCaseItemsRequest = Nothing
                oCaseItemsResponse = Nothing
                sbLogMessage = Nothing
            End Try
            Return oCaseDetails
        End SyncLock
    End Function
#End Region


#Region "Rnd 009"
    ''' <summary>
    ''' To settle multiple claim payments in one go
    ''' </summary>
    ''' <param name="v_oUnallocatedClaimPayments"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function SettleAllClaimPayments(ByVal v_oUnallocatedClaimPayments As UnallocatedClaimPaymentsCollection,
                                        Optional ByVal v_sBranchCode As String = Nothing) As SettleAllClaimPaymentsResults
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oSettleAllClaimPaymentsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.SettleAllClaimPaymentsCommand
            Dim oSettleAllClaimPaymentsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.SettleAllClaimPaymentsCommandResponse
            Dim oResSummaryCollection As ClaimPaymentsSummaryCollection
            Dim oResSummary As ClaimPaymentsSummary
            Dim sbLogMessage As StringBuilder
            Dim oResWarningCollection As WarningCollection
            Dim oResWarning As Warnings
            Dim oSettleAllClaimPayments As SettleAllClaimPaymentsResults

            Try
                'oSAM = InitializeCoreServiceMethod()
                oSettleAllClaimPaymentsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.SettleAllClaimPaymentsCommand
                oSettleAllClaimPaymentsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.SettleAllClaimPaymentsCommandResponse

                sbLogMessage = New StringBuilder

                With oSettleAllClaimPaymentsRequest
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    Dim oReqClaimPaymentsItem As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSettleClaimPaymentType
                    .ClaimPayments = New System.Collections.Generic.List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSettleClaimPaymentType)
                    For Each oPayment As UnallocatedClaimPayments In v_oUnallocatedClaimPayments
                        oReqClaimPaymentsItem = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSettleClaimPaymentType
                        With oReqClaimPaymentsItem
                            .AccountCode = oPayment.AccountCode
                            .BankAccountCode = oPayment.BankAccountCode
                            .ClaimPaymentAmount = Math.Abs(oPayment.Amount)
                            .ClaimPaymentBranchCode = oPayment.ClaimPaymentBranchCode
                            .ClaimPaymentKey = oPayment.ClaimPaymentKey
                            .CurrencyCode = oPayment.AmountCurrencyCode
                            .DocumentKey = oPayment.DocumentKey
                            .MediaTypeCode = oPayment.MediaTypeCode
                            .OurRef = oPayment.OurRef
                            .PayeeName = oPayment.PayeeName
                            .DocumentRef = oPayment.DocumentRef
                        End With
                        .ClaimPayments.Add(oReqClaimPaymentsItem)
                    Next
                End With

                Using trace As New Tracer(Category.Trace)
                    'oSettleAllClaimPaymentsResponse = oSAM.SettleAllClaimPayments(oSettleAllClaimPaymentsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.SettleAllClaimPayments, oSettleAllClaimPaymentsRequest)
                    oSettleAllClaimPaymentsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.SettleAllClaimPaymentsCommandResponse)(result)
                End Using

                With oSettleAllClaimPaymentsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oSettleAllClaimPayments = New SettleAllClaimPaymentsResults
                        If .Summary IsNot Nothing AndAlso .Summary.Count > 0 Then
                            oResSummaryCollection = New ClaimPaymentsSummaryCollection
                            For Each oSummary As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSettleAllClaimPaymentSummaryResponseType In .Summary
                                oResSummary = New ClaimPaymentsSummary
                                oResSummary.MediaTypeCode = oSummary.MediaTypeCode
                                oResSummary.Amount = oSummary.Amount
                                oResSummary.NoOfTransactions = oSummary.NoOfTransactions
                                oResSummary.StatusOfTransaction = oSummary.StatusOfTransaction
                                oResSummaryCollection.Add(oResSummary)
                            Next
                            oSettleAllClaimPayments.Summary = oResSummaryCollection
                        End If
                        If .Warnings IsNot Nothing AndAlso .Warnings.Count > 0 Then
                            oResWarningCollection = New WarningCollection
                            For Each oWarning As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGeneralWarningResponseType In .Warnings
                                oResWarning = New Warnings
                                oResWarning.Code = oWarning.Code
                                oResWarning.Description = oWarning.Description
                                oResWarningCollection.Add(oResWarning)
                            Next
                            oSettleAllClaimPayments.Warning = oResWarningCollection
                        End If
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oResSummaryCollection) Then
                        sbLogMessage.AppendLine("Returned " & oResSummaryCollection.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oSettleAllClaimPaymentsRequest = Nothing
                oSettleAllClaimPaymentsResponse = Nothing
            End Try

            Return oSettleAllClaimPayments
        End SyncLock
    End Function
#End Region

    Public Overrides Sub UpdatePartyRisk(ByVal v_iPartyKey As Integer,
                                         ByRef r_sXMLDataSet As String,
                                         ByRef r_bTimeStamp() As Byte,
                                         Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oUpdatePartyRiskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdatePartyRiskCommand ' Request Type
            Dim oUpdatePartyRiskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdatePartyRiskCommandResponse ' Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oUpdatePartyRiskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdatePartyRiskCommand
                oUpdatePartyRiskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdatePartyRiskCommandResponse
                sbLogMessage = New StringBuilder

                With oUpdatePartyRiskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                    Else
                        Throw New ArgumentNullException("PartyKeyField")
                    End If
                    If String.IsNullOrEmpty(r_sXMLDataSet) Then
                        .XMLDataSet = r_sXMLDataSet
                    Else
                        Throw New ArgumentNullException("XmlDataSetField")
                    End If
                    .ApiTimeStamp = r_bTimeStamp
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdatePartyRisk, oUpdatePartyRiskRequest)
                    oUpdatePartyRiskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdatePartyRiskCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdatePartyRiskResponse.UpdatePartyRiskResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    r_sXMLDataSet = .XMLDataSet
                    r_bTimeStamp = .ApiTimeStamp
                    'End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdatePartyRisk executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_ipartyKeyField = " & v_iPartyKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sxMLDataSetField = " & r_sXMLDataSet.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_btimeStampField = " & r_bTimeStamp.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdatePartyRiskRequest = Nothing
                oUpdatePartyRiskResponse = Nothing
            End Try


        End SyncLock
    End Sub


    Public Overrides Sub UpdateCoInsuranceValues(ByVal v_iInsuranceFileKey As Integer,
                                                ByVal v_bIsRecovered As Boolean,
                                                ByVal v_bIsSurcharged As Boolean,
                                                ByRef v_bTimeStampField() As Byte,
                                                ByVal v_oCoInsurers As CoInsurersCollections,
                                                Optional ByVal v_iDefaultId As Integer = Nothing,
                                                Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oUpdateCoInsuranceValuesRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateCoinsuranceValuesCommand
            Dim oUpdateCoInsuranceValuesResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateCoinsuranceValuesCommandResponse
            Dim oNewCoInsurers As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateCoinsuranceValuesRequestTypeRow
            Dim i As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oUpdateCoInsuranceValuesRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateCoinsuranceValuesCommand
                oUpdateCoInsuranceValuesResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateCoinsuranceValuesCommandResponse
                sbLogMessage = New StringBuilder


                With oUpdateCoInsuranceValuesRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .DefaultId = v_iDefaultId
                    .InsuranceFileKey = v_iInsuranceFileKey
                    .IsRecovered = v_bIsRecovered
                    .IsSurcharged = v_bIsSurcharged
                    .ApiTimeStamp = v_bTimeStampField



                    .CoInsurers = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateCoinsuranceValuesRequestTypeRow)
                    For i = 0 To (v_oCoInsurers.Count - 1)

                        oNewCoInsurers = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseUpdateCoinsuranceValuesRequestTypeRow
                        oNewCoInsurers.CoInsurerKey = v_oCoInsurers(i).CoInsurerKey
                        oNewCoInsurers.ArrangementRef = v_oCoInsurers(i).ArrangementRef
                        oNewCoInsurers.SharePerc = v_oCoInsurers(i).SharePerc
                        oNewCoInsurers.CommissionPerc = v_oCoInsurers(i).CommissionPerc

                        .CoInsurers.Add(oNewCoInsurers)
                    Next


                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()

                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateCoinsuranceDetails, oUpdateCoInsuranceValuesRequest)
                    oUpdateCoInsuranceValuesResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateCoinsuranceValuesCommandResponse)(result)
                End Using

                ' Disposing the SAM's object


                With oUpdateCoInsuranceValuesResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_bTimeStampField = .ApiTimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateCoInsuranceValues executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bIsRecovered = " & v_bIsRecovered.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bIsSurcharged = " & v_bIsSurcharged.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iDefaultId = " & v_iDefaultId.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oCoInsurers = " & v_oCoInsurers.Count & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdateCoInsuranceValuesRequest = Nothing
                oUpdateCoInsuranceValuesResponse = Nothing
            End Try


        End SyncLock
    End Sub


    Public Overrides Function GetUserGroupUsers(ByVal v_sUserGroupCode As String,
                    ByVal v_dtEffectiveDate As DateTime, ByVal v_bRestrictUserList As Boolean,
                    ByVal v_bRestrictUserListSpecified As Boolean,
                    Optional ByVal v_sBranchCode As String = Nothing) As UserCollection
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oGetUserGroupUsersRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupUsersQuery
            Dim oGetUserGroupUsersResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupUsersQueryResponse
            Dim oUserGroupUsers As UserCollection = Nothing
            Dim oUserGroup As User = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                oGetUserGroupUsersRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupUsersQuery
                oGetUserGroupUsersResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupUsersQueryResponse
                sbLogMessage = New StringBuilder


                With oGetUserGroupUsersRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .UserGroupCode = v_sUserGroupCode
                    If v_dtEffectiveDate = DateTime.MinValue Then
                        Throw New ArgumentException("UserGroupTaskGroups.EffectiveDate")
                    Else
                        '.EffectiveDate = v_dtEffectiveDate.UtcNow.ToString("yyyy-MM-dd")
                        .EffectiveDate = v_dtEffectiveDate.ToString("yyyy-MM-dd")
                    End If
                    .RestrictUserList = v_bRestrictUserList
                    .RestrictUserListSpecified = v_bRestrictUserListSpecified
                End With


                Using trace As New Tracer(Category.Trace)
                    'oGetUserGroupUsersResponse = oSAM.GetUserGroupUsers(oGetUserGroupUsersRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUserGroupUsers, oGetUserGroupUsersRequest)
                    oGetUserGroupUsersResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUserGroupUsersQueryResponse)(result)

                End Using


                With oGetUserGroupUsersResponse

                    If .UserGroupUsers IsNot Nothing AndAlso .UserGroupUsers.Count > 0 Then


                        oUserGroupUsers = New UserCollection()
                        For Each oBaseUserGroupUser As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetUserGroupUsersResponseTypeRow In .UserGroupUsers
                            oUserGroup = New User()
                            With oUserGroup

                                .UserId = oBaseUserGroupUser.UserKey
                                .UserName = oBaseUserGroupUser.Name
                                .EmailAddress = oBaseUserGroupUser.EmailAddress
                            End With
                            oUserGroupUsers.Add(oUserGroup)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUserGroupUsers executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If IsNothing(v_sUserGroupCode) Then
                        sbLogMessage.AppendLine("UserGroup Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("UserGroup Code : " & v_sUserGroupCode & vbCrLf)
                    End If

                    If IsNothing(v_dtEffectiveDate) Then
                        sbLogMessage.AppendLine("Effective Date : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Effective Date : " & v_dtEffectiveDate.ToString() & vbCrLf)
                    End If

                    If IsNothing(v_bRestrictUserList) Then
                        sbLogMessage.AppendLine("RestrictUser List : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("RestrictUser List : " & v_bRestrictUserList.ToString() & vbCrLf)
                    End If

                    If IsNothing(v_bRestrictUserListSpecified) Then
                        sbLogMessage.AppendLine("RestrictUser List Specified : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("RestrictUser List Specified : " & v_bRestrictUserListSpecified.ToString() & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oUserGroupUsers.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetUserGroupUsersRequest = Nothing
                oGetUserGroupUsersResponse = Nothing
            End Try


            Return oUserGroupUsers
        End SyncLock
    End Function

    Public Overrides Sub RunRenewalInvite(ByVal v_iInsuranceFileKey As Integer,
                                        ByVal v_iBatchRenewalJobKey As Integer,
                                        ByVal v_iRecordsCount As Integer,
                                        ByVal v_sGUID As String,
                                        Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oRunRenewalInviteRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.RunRenewalInvitationCommand 'Request Type
            Dim oRunRenewalInviteResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.RunRenewalInvitationCommandResponse
            Dim sbLogMessage As StringBuilder

            Try

                oRunRenewalInviteRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.RunRenewalInvitationCommand
                oRunRenewalInviteResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.RunRenewalInvitationCommandResponse
                sbLogMessage = New StringBuilder


                With oRunRenewalInviteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                    If v_iBatchRenewalJobKey > 0 Then
                        .BatchRenewalJobKey = v_iBatchRenewalJobKey
                    Else
                        Throw New ArgumentNullException("v_iBatchRenewalJobKey")
                    End If

                    If v_iRecordsCount > 0 Then
                        .RecordsCount = v_iRecordsCount
                    Else
                        Throw New ArgumentNullException("v_iRecordsCount")
                    End If

                    If String.IsNullOrEmpty(v_sGUID) Then
                        Throw New ArgumentNullException("v_sGUID")
                    Else
                        .GUID = v_sGUID
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.GetCurrenciesByBranch, oRunRenewalInviteRequest)
                    oRunRenewalInviteResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.RunRenewalInvitationCommandResponse)(result)
                End Using

                If Logger.IsLoggingEnabled Then
                    ' Disposing the SAM's object
                    sbLogMessage.AppendLine("RunRenewalInvite executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iBatchRenewalJobKey = " & v_iBatchRenewalJobKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iRecordsCount = " & v_iRecordsCount.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sGUID = " & v_sGUID.ToString & vbCrLf)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oRunRenewalInviteRequest = Nothing
            End Try


        End SyncLock


    End Sub

#Region "CATLIN - 1.13.5 Anonymous Quote"
    ''' <summary>
    ''' To Transfer Anonymous quote to real party
    ''' </summary>
    ''' <param name="v_iPartyFrom"></param>
    ''' <param name="v_iPartyTo"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub TransferQuoteToRealParty(ByVal v_iPartyFrom As Integer, ByVal v_iPartyTo As Integer,
        ByVal v_iInsuranceFileKey As Integer, Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            'Dim oTransferQuoteRequest As TransferQuoteRequestType
            'Dim oTransferQuoteResponse As TransferQuoteResponseType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeCoreServiceMethod()
                'oTransferQuoteRequest = New TransferQuoteRequestType
                'oTransferQuoteResponse = New TransferQuoteResponseType
                sbLogMessage = New StringBuilder



                'With oTransferQuoteRequest
                '    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '    .WCFSecurityToken = SecurityToken()
                '    If String.IsNullOrEmpty(v_sBranchCode) Then
                '        'if the branch code is NOT in session 
                '        If String.IsNullOrEmpty(sBranchCode) Then
                '            'Use the default branch code
                '            .BranchCode = sDefaultBranchCode
                '        Else
                '            'Use the branch code in session 
                '            .BranchCode = sBranchCode

                '        End If

                '    Else
                '        'use the passed parameter v_sBranchCode
                '        .BranchCode = v_sBranchCode
                '    End If
                '    .PartyToKey = v_iPartyTo
                '    .PartyFromKey = v_iPartyFrom

                '    If v_iInsuranceFileKey > 0 Then
                '        .InsuranceFileKey = v_iInsuranceFileKey
                '    Else
                '        Throw New ArgumentNullException("InsuranceFileKey")
                '    End If

                'End With


                'oTransferQuoteResponse = oSAM.TransferQuote(oTransferQuoteRequest)

                'With oTransferQuoteResponse

                '    If .Errors IsNot Nothing Then

                '        'Process the error object if errors, and throw as a single exception
                '        Throw New NexusException(.Errors)

                '    Else

                '    End If

                'End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("TransferQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                'oTransferQuoteRequest = Nothing
                'oTransferQuoteResponse = Nothing
            End Try

        End SyncLock
    End Sub
#End Region


    ''' <summary>
    ''' Function to Get Task on keys
    ''' </summary>
    ''' <param name="oKeyData"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetTaskOnKeys(ByVal oKeyDataCollection As KeyDataCollection,
                                           Optional ByVal v_sBranchCode As String = Nothing) As TaskCollection

        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oGetTaskOnKeysRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskOnKeysQuery
            Dim oGetTaskOnKeysResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskOnKeysQueryResponse
            Dim oWorkManager As WorkManager = Nothing
            Dim oTaskCollection As TaskCollection = Nothing
            Dim iCounter As Integer = 0
            Dim oKeyData As KeyData = Nothing
            Try
                'oSAM = InitializeCoreServiceMethod()
                oGetTaskOnKeysRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskOnKeysQuery
                oGetTaskOnKeysResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskOnKeysQueryResponse

                With oGetTaskOnKeysRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    Dim oGetTaskOnKeysKeyData As New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreateWmTaskRequestTypeKeyData
                    oGetTaskOnKeysKeyData.rowField = New System.Collections.Generic.List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreateWmTaskRequestTypeKeyDataRow)
                    For iCounter = 0 To oKeyDataCollection.Count - 1
                        oKeyData = New KeyData
                        Dim oGetTaskOnKeysKeyDataRow As New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreateWmTaskRequestTypeKeyDataRow
                        oGetTaskOnKeysKeyDataRow.KeyName = oKeyDataCollection(iCounter).KeyName
                        oGetTaskOnKeysKeyDataRow.KeyValue = oKeyDataCollection(iCounter).KeyValue
                        oGetTaskOnKeysKeyData.rowField.Add(oGetTaskOnKeysKeyDataRow)
                    Next

                    .KeyData = oGetTaskOnKeysKeyData
                End With
                Using trace As New Tracer(Category.Trace)
                    'oGetTaskOnKeysResponse = oSAM.GetTaskOnKeys(oGetTaskOnKeysRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetTaskOnKeys, oGetTaskOnKeysRequest)
                    oGetTaskOnKeysResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaskOnKeysQueryResponse)(result)


                End Using

                With oGetTaskOnKeysResponse

                    If .Tasks IsNot Nothing Then
                        oTaskCollection = New TaskCollection()
                        For iCounter = 0 To .Tasks.Count - 1
                            Dim oTask As New Task
                            oTask.Urgent = .Tasks(iCounter).Urgent
                            oTask.UserCode = .Tasks(iCounter).UserCode
                            oTask.UserGroupCode = .Tasks(iCounter).UserGroupCode
                            oTask.UserGroupDescription = .Tasks(iCounter).UserGroupDescription
                            oTask.UserGroupKey = .Tasks(iCounter).UserGroupKey
                            oTask.UserKey = .Tasks(iCounter).UserKey
                            oTask.Customer = .Tasks(iCounter).Customer
                            oTask.Description = .Tasks(iCounter).Description
                            oTask.DueDate = .Tasks(iCounter).DueDate
                            oTask.GuidPMExternalItem = .Tasks(iCounter).GuidPMExternalItem.ToString()
                            oTask.IsExternalItem = .Tasks(iCounter).IsExternalItem
                            oTask.ParentTaskKey = .Tasks(iCounter).ParentTaskKey
                            oTask.PartyKey = .Tasks(iCounter).PartyKey
                            oTask.PartyName = .Tasks(iCounter).PartyName
                            oTask.TaskGroupKey = .Tasks(iCounter).TaskGroupKey
                            oTask.TaskInstanceKey = .Tasks(iCounter).TaskInstanceKey
                            oTask.TaskKey = .Tasks(iCounter).TaskKey
                            oTask.TaskStatusKey = .Tasks(iCounter).TaskStatusKey
                            oTask.Type = .Tasks(iCounter).Type
                            oTaskCollection.Add(oTask)
                        Next
                    End If

                End With

                Dim sbLogMessage As New StringBuilder
                sbLogMessage.AppendLine("GetTaskOnKeys executed ok" & vbCrLf)
                '' we don't have input here, we have the input on Credentials Method
                sbLogMessage.AppendLine("Input: " & vbCrLf)
                If oKeyDataCollection IsNot Nothing Then
                    For iCounter = 0 To oKeyDataCollection.Count - 1
                        sbLogMessage.AppendLine(oKeyDataCollection(iCounter).Print().Replace("<br />", vbCrLf))
                    Next
                End If
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If
                Dim logEntry As New LogEntry()

                logEntry.Categories.Clear()
                logEntry.Categories.Add(Category.General)
                logEntry.Priority = Priority.Normal
                logEntry.Severity = TraceEventType.Verbose
                logEntry.Message = sbLogMessage.ToString

                Logger.Write(logEntry)

                Return oTaskCollection

            Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw (ex)
            Finally
                'oSAM.Close()
                oGetTaskOnKeysRequest = Nothing
                oGetTaskOnKeysResponse = Nothing
                oTaskCollection = Nothing
            End Try

        End SyncLock
    End Function
    ''' <summary>
    ''' Function to UpdateTaskStatus
    ''' </summary>
    ''' <param name="v_oUpdateWmTask"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateTaskStatus(ByVal v_oUpdateWmTask As WorkManager,
                                          Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock
            Dim oUpdateTaskStatusRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskStatusCommand
            Dim oUpdateTaskStatusResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskStatusCommandResponse
            Dim oWorkManager As WorkManager = Nothing
            Dim iCounter As Integer = 0
            Try
                oUpdateTaskStatusRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskStatusCommand
                oUpdateTaskStatusResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskStatusCommandResponse

                With oUpdateTaskStatusRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .TaskStatusKey = v_oUpdateWmTask.TaskStatusKey
                    .TaskInstanceKey = v_oUpdateWmTask.TaskInstanceKey
                    .ExternalTaskStatus = v_oUpdateWmTask.ExternalTaskStatus
                    .GuidPMExternalItem = v_oUpdateWmTask.GuidPMExternalItem
                    .ActionType = v_oUpdateWmTask.ActionType
                End With


                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateTaskStatus, oUpdateTaskStatusRequest)
                    oUpdateTaskStatusResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateTaskStatusCommandResponse)(result)
                End Using


                With oUpdateTaskStatusResponse
                    'If 1 = 0 Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'End If

                End With
                If Logger.IsLoggingEnabled Then
                    Dim sbLogMessage As New StringBuilder
                    sbLogMessage.AppendLine("UpdateTaskStatus executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input: " & vbCrLf)
                    sbLogMessage.AppendLine(v_oUpdateWmTask.Print().Replace("<br />", vbCrLf))
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    Dim logEntry As New LogEntry()

                    logEntry.Categories.Clear()
                    logEntry.Categories.Add(Category.General)
                    logEntry.Priority = Priority.Normal
                    logEntry.Severity = TraceEventType.Verbose
                    logEntry.Message = sbLogMessage.ToString

                    Logger.Write(logEntry)
                End If

            Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw (ex)
            Finally
                oUpdateTaskStatusRequest = Nothing
                oUpdateTaskStatusResponse = Nothing
                oWorkManager = Nothing
            End Try
        End SyncLock
    End Sub

    ''' <summary>
    ''' Function to Generate Unique Error Id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GenerateUniqueSSPExceptionRef() As String
        SyncLock oLock
            Dim iLength As Integer = ERROR_NO_LENGTH
            Dim sResult As New StringBuilder
            sResult.Append(ERROR_LABEL)

            Try
                Dim rdm As New Random()
                Dim allowChrs() As Char = "ABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()

                For i As Integer = 0 To iLength - 1
                    sResult.Append(allowChrs(rdm.Next(0, allowChrs.Length)))
                Next
            Catch
                sResult = New StringBuilder
                sResult.Append(ERROR_LABEL)
                sResult.Append(New String("9", iLength))
            End Try
            sResult.Append(" - ")
            sResult.Append(DateTime.Now())

            Return sResult.ToString()
        End SyncLock
    End Function

    Public Overloads Function GetList(ByVal v_oListType As ListType,
                           ByVal spuICCSName As String,
                           ByVal spuICCSParams As List(Of PureService.ICCSParam),
                           ByVal spuICCSUseCache As Boolean,
                           ByVal v_sListCode As String,
                           ByVal v_bExcludeDeletedRecords As Boolean,
                           ByVal v_bExcludeEffectiveDate As Boolean,
                           Optional ByVal v_sParentFieldName As String = Nothing,
                           Optional ByVal v_iParentFieldValue As Integer = -1,
                           Optional ByVal v_sBranchCode As String = Nothing,
                           Optional ByRef v_sXmlElement As System.Xml.XmlElement = Nothing,
                           Optional ByVal v_dEffectiveDate As Date = Nothing) As LookupListCollection

        SyncLock oLock
            Dim oLookupListCollection As LookupListCollection
            Dim oGetListRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListSPUICCSQuery
            Dim oGetListResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQueryResponse
            Dim sbLogMessage As StringBuilder

            Try
                oGetListRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListSPUICCSQuery
                oGetListResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQueryResponse
                oLookupListCollection = New LookupListCollection
                sbLogMessage = New StringBuilder
                'Cache the definition as this is a waste of bandwidth otherwise
                'Replaced PROVIDER_LOOKUPLIST with v_oListType.ToString & "_" in this method inorder to
                'distinguish "type of list PMlookup/GIS/Userdefined" being cached and retreived from cache.
                If Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString) Is Nothing Then

                    With oGetListRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                        'if the passed parameter v_sBranchCode is empty
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If

                        .ListCode = v_sListCode
                        .ListType = v_oListType
                        .ExcludeDeletedRecords = v_bExcludeDeletedRecords
                        .ExcludeEffectiveDate = v_bExcludeEffectiveDate
                        .ParentFieldName = v_sParentFieldName
                        If v_iParentFieldValue >= 0 Then
                            .ParentFieldValue = v_iParentFieldValue
                            .ParentFieldValueSpecified = True
                        Else
                            .ParentFieldValueSpecified = False
                        End If

                        If v_dEffectiveDate = DateTime.MinValue Then
                            .EffectiveDateSpecified = False
                        Else
                            .EffectiveDateSpecified = True
                            .EffectiveDate = v_dEffectiveDate
                        End If

                    End With

                    oGetListRequest.SpuICCSName = spuICCSName
                    'oGetListRequest.SpuICCSParameters = spuICCSParams
                    oGetListRequest.UseCache = spuICCSUseCache

                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.GetListSPUICCS, oGetListRequest)
                        oGetListResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetListQueryResponse)(result)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                    With oGetListResponse

                        'If .Errors IsNot Nothing Then
                        '    'Process the error object if errors, and throw as a single exception
                        '    Throw New NexusException(.Errors)
                        'Else

                        oLookupListCollection = New LookupListCollection

                        If .AdditionalResult IsNot Nothing Then
                            Dim xmlDoc As New System.Xml.XmlDocument()
                            Dim payload As String = .AdditionalResult.Trim()

                            xmlDoc.LoadXml("<AdditionalDetails>" & payload & "</AdditionalDetails>")
                            v_sXmlElement = xmlDoc.DocumentElement

                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails", v_sXmlElement,
                                                    Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)

                        End If

                        If .GetListResponse IsNot Nothing Then

                            For Each olistItem As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetListResponseType In .GetListResponse
                                With olistItem
                                    oLookupListCollection.Add(New LookupListItem(.Key, .ParentKey, .Code.Trim(),
                                            .Description.Trim(), .EffectiveDate, .IsDeleted, .IsDefault))
                                End With
                            Next

                            Current.Cache.Insert(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString, oLookupListCollection,
                                                        Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        End If

                        'End If

                    End With

                Else
                    oLookupListCollection = CType(Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString), LookupListCollection)

                    If Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails") IsNot Nothing Then
                        v_sXmlElement = CType(Current.Cache(v_oListType.ToString & "_" & v_sListCode & "_" & v_sParentFieldName & "_" & v_iParentFieldValue.ToString & "_" & v_dEffectiveDate.ToString & "_AdditionalDetails"), System.Xml.XmlElement)
                    End If

                End If
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetList executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oListType = " & v_oListType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sListCode = " & v_sListCode.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oLookupListCollection.Count.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw (ex)
            Finally
                oGetListRequest = Nothing
                oGetListResponse = Nothing
            End Try


            Return oLookupListCollection

        End SyncLock

    End Function

#Region "Get Policy Outstanding Amount"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_dOutstandingAmount"></param>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function GetPolicyOutstandingAmount(ByRef r_dOutstandingAmount As Decimal,
                                                           ByVal v_nInsuranceFileKey As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As Decimal


        'Dim oSAM As PureCoreServiceClient
        Dim oGetPolicyOustandingAmountRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyOutstandingAmountQuery
        Dim oGetPolicyOustandingAmountResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyOutstandingAmountQueryResponse
        'oSAM = InitializeCoreServiceMethod()
        With oGetPolicyOustandingAmountRequest
            .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
            '.WCFSecurityToken = SecurityToken()
            'if the passed parameter v_sBranchCode is empty
            If String.IsNullOrEmpty(v_sBranchCode) Then
                'if the branch code is NOT in session 
                If String.IsNullOrEmpty(sBranchCode) Then
                    'Use the default branch code
                    .BranchCode = sDefaultBranchCode
                Else
                    'Use the branch code in session 
                    .BranchCode = sBranchCode
                End If
            Else
                'use the passed parameter v_sBranchCode
                .BranchCode = v_sBranchCode
            End If
            If v_nInsuranceFileKey > 0 Then
                .InsuarnaceFilecnt = v_nInsuranceFileKey
            Else
                Throw New ArgumentNullException("v_nInsuranceFileKey")
            End If

        End With

        Try
            Using trace As New Tracer(Category.Trace)
                'oGetPolicyOustandingAmountResponse = oSAM.GetPolicyOutstandingAmount(oGetPolicyOustandingAmountRequest)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetPolicyOutstandingAmount, oGetPolicyOustandingAmountRequest)
                oGetPolicyOustandingAmountResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyOutstandingAmountQueryResponse)(result)
                r_dOutstandingAmount = oGetPolicyOustandingAmountResponse.OutstandingAmount
            End Using
        Finally
            ' oSAM.Close()
        End Try

        With oGetPolicyOustandingAmountResponse
            If .Errors IsNot Nothing Then
                'Process the error object if errors, and throw as a single exception
                Throw New NexusException(.Errors)
            End If

        End With
        If Logger.IsLoggingEnabled Then
            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("GetPolicyOutstandingAmount executed ok" & vbCrLf)
            '' we don't have input here, we have the input on Credentials Method
            sbLogMessage.AppendLine("Input: " & vbCrLf)
            sbLogMessage.AppendLine(v_nInsuranceFileKey.ToString().Replace("<br />", vbCrLf))

            Dim logEntry As New LogEntry()

            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString

            Logger.Write(logEntry)
        End If
        Return r_dOutstandingAmount
    End Function
#End Region

#Region "Get PaymentHub System Option"
    ''' <summary>
    ''' Get Payment Hub System Options
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function GetPaymentHubSystemOptions(Optional ByVal v_sBranchCode As String = Nothing) As PaymentHubConfig
        'Dim oSAM As PureCoreServiceClient
        Dim oGetPaymentHubSystemOptionsRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPaymentHubSystemOptionsQuery
        Dim oGetPaymentHubSystemOptionsResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPaymentHubSystemOptionsQueryResponse
        Dim oPaymentHub As New NexusProvider.PaymentHubConfig
        If Current.Cache("PaymentHubConfig") Is Nothing Then
            'oSAM = InitializeCoreServiceMethod()
            With oGetPaymentHubSystemOptionsRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '.WCFSecurityToken = SecurityToken()
                'if the passed parameter v_sBranchCode is empty
                If String.IsNullOrEmpty(v_sBranchCode) Then
                    'if the branch code is NOT in session 
                    If String.IsNullOrEmpty(sBranchCode) Then
                        'Use the default branch code
                        .BranchCode = sDefaultBranchCode
                    Else
                        'Use the branch code in session 
                        .BranchCode = sBranchCode
                    End If
                Else
                    'use the passed parameter v_sBranchCode
                    .BranchCode = v_sBranchCode
                End If

            End With

            Try
                Using trace As New Tracer(Category.Trace)
                    'oGetPaymentHubSystemOptionsResponse = oSAM.GetPaymentHubSystemOptions(oGetPaymentHubSystemOptionsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetPaymentHubSystemOptions, oGetPaymentHubSystemOptionsRequest)
                    oGetPaymentHubSystemOptionsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPaymentHubSystemOptionsQueryResponse)(result)

                End Using
            Finally
                ' oSAM.Close()
            End Try

            With oGetPaymentHubSystemOptionsResponse
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                End If
                oPaymentHub.AccountID = GetOVal(Convert.ToString(.AccountID))
                oPaymentHub.BrokerSCID = .BrokerSCID
                oPaymentHub.CaptureMethod = .CaptureMethod
                oPaymentHub.ClientName = .ClientName
                oPaymentHub.Customer = .Customer
                oPaymentHub.Donotuseoldcarddetailsforsubsequentpayments = .Donotuseoldcarddetailsforsubsequentpayments
                oPaymentHub.MarkDefaultCreditCard = .MarkDefaultCreditCard
                oPaymentHub.MerchantID = .MerchantID
                oPaymentHub.Password = GetOVal(Convert.ToString(.Password))
                oPaymentHub.RefundPremiumthroughInvoice = .RefundPremiumthroughInvoice
                oPaymentHub.RefundPasscode = GetOVal(Convert.ToString(.RefundPasscode))
                oPaymentHub.ReturnURL = .ReturnURL
                oPaymentHub.SystemGUID = GetOVal(Convert.ToString(.SystemGUID))
                oPaymentHub.SystemPasscode = GetOVal(Convert.ToString(.SystemPasscode))
                oPaymentHub.SystemUserName = .SystemUserName
                oPaymentHub.TransactionIPAddress = .TransactionIPAddress
                oPaymentHub.LanguageTemplateID = .LanguageTemplateID
                oPaymentHub.MerchantTemplateID = .MerchantTemplateID
                oPaymentHub.AccountPassCode = GetOVal(Convert.ToString(.AccountPassCode))
                oPaymentHub.PaymentHubServiceUrl = .PaymentHubServiceUrl
            End With
            Current.Cache.Insert("PaymentHubConfig", oPaymentHub)
        Else
            oPaymentHub = Current.Cache("PaymentHubConfig")
        End If

        If Logger.IsLoggingEnabled Then
            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("GetPaymentHubSystemOptions executed ok" & vbCrLf)
            sbLogMessage.AppendLine("Input:" & vbCrLf)
            If Not IsNothing(v_sBranchCode) Then
                sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
            Else
                sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
            End If

            sbLogMessage.AppendLine("Output:" & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.BrokerSCID" & oPaymentHub.BrokerSCID.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.CaptureMethod" & oPaymentHub.CaptureMethod.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.ClientName" & oPaymentHub.ClientName.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.Customer" & oPaymentHub.Customer.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.Donotuseoldcarddetailsforsubsequentpayments" & oPaymentHub.Donotuseoldcarddetailsforsubsequentpayments.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.MarkDefaultCreditCard" & oPaymentHub.MarkDefaultCreditCard.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.MerchantID" & oPaymentHub.MerchantID.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.RefundPremiumthroughInvoice" & oPaymentHub.RefundPremiumthroughInvoice.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.RefundPasscode" & oPaymentHub.RefundPasscode.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.SystemUserName" & oPaymentHub.SystemUserName.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.TransactionIPAddress" & oPaymentHub.TransactionIPAddress.ToString() & vbCrLf)
            sbLogMessage.AppendLine("oPaymentHub.PaymentHubServiceUrl" & oPaymentHub.PaymentHubServiceUrl.ToString() & vbCrLf)
            LogMessageEntry(sbLogMessage)
        End If

        Return oPaymentHub
    End Function
    Private Function GetOVal(ByVal encryptedtext As String) As String
        Dim sRetVal As String = ""

        Dim TripleDes As New TripleDESCryptoServiceProvider
        Dim sKey As String = "!@$1R1U5"
        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)


        Try

            ' Convert the encrypted text string to a byte array. 
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream. 
            Dim ms As New System.IO.MemoryStream
            ' Create the decoder to write to the stream. 
            Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string. 
            sRetVal = System.Text.Encoding.Unicode.GetString(ms.ToArray)

            TripleDes = Nothing
        Catch ex As Exception
            Return ""
        End Try

        Return sRetVal
    End Function
    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key. 
        Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash. 
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

#End Region
    Public Overrides Function CallNamedStoredProcedure(ByVal oBaseParamter() As StoredProcedureParameterType, ByVal sStoredProcedureName As String, Optional ByVal v_sBranchCode As String = Nothing) As StoredProcedureResponseType
        SyncLock oLock

            'Dim oSAM As New PureCoreServiceClient
            Dim oCallNamedStoredProcedureRequestType As New SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommand
            Dim oCallNamedStoredProcedureResponseType As New SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommandResponse
            Dim oStoredProcedureResponseType As New StoredProcedureResponseType
            Dim oBaseParameterType() As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseParameterType

            Try
                With oCallNamedStoredProcedureRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If oBaseParamter IsNot Nothing AndAlso oBaseParamter.GetUpperBound(0) >= 0 Then
                        If .Parameters Is Nothing Then
                            .Parameters = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseParameterType)()
                        End If

                        ReDim Preserve oBaseParameterType(oBaseParamter.GetUpperBound(0))

                        For i As Integer = 0 To oBaseParamter.GetUpperBound(0)
                            oBaseParameterType(i) = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseParameterType()
                            oBaseParameterType(i).ParamName = oBaseParamter(i).ParamName
                            oBaseParameterType(i).ParamValue = oBaseParamter(i).ParamValue
                            .Parameters.Add(oBaseParameterType(i))
                        Next
                        .ProcedureName = sStoredProcedureName
                    End If
                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oCallNamedStoredProcedureResponseType = oSAM.CallNamedStoredProcedure(oCallNamedStoredProcedureRequestType)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CallNamedStoredProcedure, oCallNamedStoredProcedureRequestType)
                    oCallNamedStoredProcedureResponseType = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommandResponse)(result)
                End Using

                With oCallNamedStoredProcedureResponseType
                    If .Errors IsNot Nothing Then
                        Throw New NexusException(.Errors)
                    Else
                        If .Results IsNot Nothing Then
                            oStoredProcedureResponseType.Results = oCallNamedStoredProcedureResponseType.Results
                            Return oStoredProcedureResponseType
                        End If

                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    Dim sbLogMessage As New StringBuilder
                    ' Disposing the SAM's object
                    sbLogMessage.AppendLine("CallNamedStoredProcedure executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
            Catch ex As Exception
                Throw
            Finally
                ' oSAM.Close()
                oCallNamedStoredProcedureRequestType = Nothing
                oCallNamedStoredProcedureResponseType = Nothing
            End Try

            Return Nothing

        End SyncLock
    End Function
    Public Overrides Function CallNamedStoredProcedure(ByVal v_sProcedureName As String,
                                   ByVal v_oParameters As NexusProvider.ParametersCollection,
                                   ByVal v_bReport As Boolean,
                                   Optional ByVal v_sBranchCode As String = Nothing) As DataSet
        SyncLock oLock

            'Dim oSAM As PureCoreServiceClient
            Dim oCallNamedStoredProcedureRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommand
            Dim oCallNamedStoredProcedureResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommandResponse
            Dim sResultDataSet As New DataSet
            Dim sbLogMessage As StringBuilder
            Dim oParameters As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseParameterType

            Try
                'oSAM = InitializeCoreServiceMethod()
                oCallNamedStoredProcedureRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommand
                oCallNamedStoredProcedureResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommandResponse
                sbLogMessage = New StringBuilder


                With oCallNamedStoredProcedureRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If


                    If String.IsNullOrEmpty(v_sProcedureName) Then
                        Throw New ArgumentNullException("v_sProcedureName")
                    Else
                        .ProcedureName = v_sProcedureName
                    End If

                    .IsReportDataset = v_bReport

                    'run the loop for parameters and make the request ready

                    .Parameters = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseParameterType)
                    If v_oParameters IsNot Nothing Then
                        For iCount As Integer = 0 To v_oParameters.Count - 1
                            oParameters = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseParameterType

                            oParameters.ParamName = v_oParameters(iCount).ParamNameField
                            oParameters.ParamValue = v_oParameters(iCount).ParamValueField
                            .Parameters.Add(oParameters)
                        Next
                    End If
                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oCallNamedStoredProcedureResponse = oSAM.CallNamedStoredProcedure(oCallNamedStoredProcedureRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CallNamedStoredProcedure, oCallNamedStoredProcedureRequest)
                    oCallNamedStoredProcedureResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CallNamedStoredProcedureCommandResponse)(result)
                End Using

                With oCallNamedStoredProcedureResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        Dim dt As New DataTable("ReportData")
                        If .ReportDataset IsNot Nothing AndAlso .ReportDataset.Count > 0 Then
                            For Each kvp As KeyValuePair(Of String, Object) In .ReportDataset(0)
                                Dim colType As Type = If(kvp.Value IsNot Nothing, kvp.Value.GetType(), GetType(String))
                                dt.Columns.Add(kvp.Key, colType).AllowDBNull = True
                            Next
                            For Each row As Dictionary(Of String, Object) In .ReportDataset
                                Dim dr As DataRow = dt.NewRow()
                                For Each kvp As KeyValuePair(Of String, Object) In row
                                    If kvp.Value Is Nothing OrElse String.Empty.Equals(kvp.Value) Then
                                        dr(kvp.Key) = DBNull.Value
                                    Else
                                        Try
                                            dr(kvp.Key) = Convert.ChangeType(kvp.Value, dt.Columns(kvp.Key).DataType)
                                        Catch
                                            dr(kvp.Key) = DBNull.Value
                                        End Try
                                    End If
                                Next
                                dt.Rows.Add(dr)
                            Next
                        End If
                        sResultDataSet = New DataSet()
                        sResultDataSet.Tables.Add(dt)
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CallNamedStoredProcedure executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input : " & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oCallNamedStoredProcedureRequest = Nothing
                oCallNamedStoredProcedureResponse = Nothing
            End Try

            Return sResultDataSet
        End SyncLock


    End Function
    Public Overrides Function GetProductRiskEvents(ByVal v_iInsuranceFileKey As Integer,
                                                       ByVal v_sProductCode As String,
                                                       ByVal v_sEventType As String,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As LookupListCollection
        SyncLock oLock

            '            Dim oSAM As PureCoreServiceClient = Nothing
            Dim oBaseGetProductRiskEventsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskEventsQuery   ' Request Type
            Dim oBaseGetProductRiskEventsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskEventsQueryResponse    ' Response Type
            Dim ollCollection As NexusProvider.LookupListCollection = Nothing
            Dim olllist As LookupListItem
            Try
                'oSAM = InitializeCoreServiceMethod()
                oBaseGetProductRiskEventsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskEventsQuery
                oBaseGetProductRiskEventsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskEventsQueryResponse
                ollCollection = New NexusProvider.LookupListCollection

                With oBaseGetProductRiskEventsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                        .InsuranceFileKeySpecified = True
                    Else
                        .InsuranceFileKeySpecified = False
                    End If

                    If v_sEventType.Trim.ToUpper = "MTA" Then
                        .EventType = SSP.PureInsuranceRestAPIHandler.Enums.ProductEventActionType.MTAEvent
                    ElseIf v_sEventType.Trim.ToUpper = "CLAIM" Then
                        .EventType = SSP.PureInsuranceRestAPIHandler.Enums.ProductEventActionType.ClaimEvent
                    End If

                    .ProductCode = v_sProductCode
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oBaseGetProductRiskEventsResponse = oSAM.GetProductRiskEvents(oBaseGetProductRiskEventsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetProductRiskEvents, oBaseGetProductRiskEventsRequest)
                    oBaseGetProductRiskEventsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskEventsQueryResponse)(result)

                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                With oBaseGetProductRiskEventsResponse

                    If .Events IsNot Nothing AndAlso .Events.Count > 0 Then
                        For Each oEvent As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetProductRiskEventsResponseTypeRow In .Events
                            olllist = New LookupListItem
                            olllist.Code = oEvent.EventCode
                            olllist.Description = oEvent.EventDescription
                            olllist.Key = oEvent.EventKey
                            olllist.IsDefault = oEvent.IsDefault
                            ollCollection.Add(olllist)
                        Next
                    End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oBaseGetProductRiskEventsRequest = Nothing
                oBaseGetProductRiskEventsResponse = Nothing
            End Try
            Return ollCollection
        End SyncLock
    End Function

    Public Overrides Function GetProductRiskOptionValue(ByVal ActionType As NexusProvider.ProductConfigActionType,
                                                       ByVal ProductRiskOption As NexusProvider.ProductRiskOptions,
                                                       ByVal RiskTypeOption As NexusProvider.RiskTypeOptions,
                                                       ByVal ProductCode As String,
                                                       ByVal RiskTypeCode As String,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As String
        SyncLock oLock
            Dim sProductRiskOptionValue As String
            'Dim oSAM As PureCoreServiceClient 'SAMForInsuranceV2's Object
            Dim oProductRiskOptionValueRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskOptionValueQuery    ' Request Type
            Dim oProductRiskOptionValueResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskOptionValueQueryResponse    ' Response Type
            Try
                'oSAM = InitializeCoreServiceMethod()
                oProductRiskOptionValueRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskOptionValueQuery
                oProductRiskOptionValueResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskOptionValueQueryResponse

                If Current.Cache("ProductOption_" & ActionType.ToString & "_" & ProductCode & "_" & ProductRiskOption & "_" & RiskTypeOption & "_" & RiskTypeCode) Is Nothing Then

                    With oProductRiskOptionValueRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        '.WCFSecurityToken = SecurityToken()
                        'if the passed parameter v_sBranchCode is empty 
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode

                            End If

                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If
                        Select Case ActionType
                            Case NexusProvider.ProductConfigActionType.ProductRiskMaintenance
                                .ProducRiskOption = ProductRiskOption
                                .ProducRiskOptionSpecified = True
                                .RiskTypeOptionSpecified = False
                            Case NexusProvider.ProductConfigActionType.RiskTypeMaintenance
                                .RiskTypeOption = RiskTypeOption
                                .RiskTypeOptionSpecified = True
                                .ProducRiskOptionSpecified = False
                        End Select
                        .ActionType = ActionType
                        .ProductCode = ProductCode
                        If RiskTypeCode Is Nothing Then
                            .RiskTypeCode = Nothing
                        Else
                            .RiskTypeCode = RemoveWhitespace(RiskTypeCode)
                        End If
                        '.RiskTypeCode = RiskTypeCode
                    End With

                    'Calling the SAM Method with Request Type
                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        'oProductRiskOptionValueResponse = oSAM.GetProductRiskOptionValue(oProductRiskOptionValueRequest)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetProductRiskOptionValue, oProductRiskOptionValueRequest)
                        oProductRiskOptionValueResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductRiskOptionValueQueryResponse)(result)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                    ' Disposing the SAM's object
                    With oProductRiskOptionValueResponse
                        If .Errors IsNot Nothing Then
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else
                            sProductRiskOptionValue = .ProductRiskOptionValue
                            If sProductRiskOptionValue IsNot Nothing Then
                                Current.Cache.Insert("ProductOption_" & ActionType.ToString & "_" & ProductCode & "_" & ProductRiskOption & "_" & RiskTypeOption & "_" & RiskTypeCode, sProductRiskOptionValue,
                                                          Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                            Else
                                sProductRiskOptionValue = 0
                            End If
                        End If
                    End With

                Else
                    sProductRiskOptionValue = CType(Current.Cache("ProductOption_" & ActionType.ToString & "_" & ProductCode & "_" & ProductRiskOption & "_" & RiskTypeOption & "_" & RiskTypeCode), String)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oProductRiskOptionValueRequest = Nothing
                oProductRiskOptionValueResponse = Nothing
            End Try
            Return sProductRiskOptionValue
        End SyncLock
    End Function
    Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function

    ''' <summary>
    ''' Updates insurance file with currency and rate information
    ''' </summary>
    Public Overrides Function UpdateInsuranceFile(ByVal v_iInsuranceFileCnt As Integer,
                                             Optional ByVal v_dCurrencyBaseRate As Double? = Nothing,
                                             Optional ByVal v_dtCurrencyBaseDate As DateTime? = Nothing,
                                             Optional ByVal v_dAccountBaseRate As Double? = Nothing,
                                             Optional ByVal v_dtAccountBaseDate As DateTime? = Nothing,
                                             Optional ByVal v_dSystemBaseRate As Double? = Nothing,
                                             Optional ByVal v_dtSystemBaseDate As DateTime? = Nothing,
                                             Optional ByVal v_iRateOverrideReasonID As Integer? = Nothing,
                                             Optional ByVal v_iBaseCurrencyID As Integer? = Nothing,
                                             Optional ByVal v_iAccountCurrencyID As Integer? = Nothing,
                                             Optional ByVal v_sBranchCode As String = Nothing) As Boolean
        SyncLock oLock
            Dim oUpdateInsuranceFileRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateInsuranceFileCommand
            Dim oUpdateInsuranceFileResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateInsuranceFileCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                oUpdateInsuranceFileRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateInsuranceFileCommand
                oUpdateInsuranceFileResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateInsuranceFileCommandResponse
                sbLogMessage = New StringBuilder

                With oUpdateInsuranceFileRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFileCnt = v_iInsuranceFileCnt
                    .CurrencyBaseRate = v_dCurrencyBaseRate
                    .CurrencyBaseDate = v_dtCurrencyBaseDate
                    .AccountBaseRate = v_dAccountBaseRate
                    .AccountBaseDate = v_dtAccountBaseDate
                    .SystemBaseRate = v_dSystemBaseRate
                    .SystemBaseDate = v_dtSystemBaseDate
                    .RateOverrideReasonID = v_iRateOverrideReasonID
                    .BaseCurrencyID = v_iBaseCurrencyID
                    .AccountCurrencyID = v_iAccountCurrencyID
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateInsuranceFile, oUpdateInsuranceFileRequest)
                    oUpdateInsuranceFileResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateInsuranceFileCommandResponse)(result)
                End Using

                With oUpdateInsuranceFileResponse
                    If .Errors IsNot Nothing Then
                        Throw New NexusException(.Errors)
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateInsuranceFile executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileCnt = " & v_iInsuranceFileCnt.ToString & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                Return oUpdateInsuranceFileResponse.Success
            Catch ex As Exception
                Throw
            Finally
                oUpdateInsuranceFileRequest = Nothing
                oUpdateInsuranceFileResponse = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets currency override information
    ''' </summary>
    Public Overrides Function GetCurrencyOverride(Optional ByVal v_sBranchCode As String = Nothing) As CurrencyOverride
        SyncLock oLock
            Dim oGetCurrencyOverrideRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyOverrideQuery
            Dim oGetCurrencyOverrideResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyOverrideQueryResponse
            Dim oCurrencyOverride As CurrencyOverride = New CurrencyOverride
            Dim sbLogMessage As StringBuilder

            Try
                oGetCurrencyOverrideRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyOverrideQuery
                oGetCurrencyOverrideResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyOverrideQueryResponse
                sbLogMessage = New StringBuilder

                With oGetCurrencyOverrideRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetCurrencyOverride, oGetCurrencyOverrideRequest)
                    oGetCurrencyOverrideResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetCurrencyOverrideQueryResponse)(result)
                End Using

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCurrencyOverride executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                If oGetCurrencyOverrideResponse IsNot Nothing Then
                    oCurrencyOverride.DateOverrideAllowed = oGetCurrencyOverrideResponse.DateOverrideAllowed
                    oCurrencyOverride.RateOverrideAllowed = oGetCurrencyOverrideResponse.RateOverrideAllowed
                    oCurrencyOverride.PrePolicyDateOverrideAllowed = oGetCurrencyOverrideResponse.PrePolicyDateOverrideAllowed
                    oCurrencyOverride.PrePolicyRateOverrideAllowed = oGetCurrencyOverrideResponse.PrePolicyRateOverrideAllowed
                End If

                Return oCurrencyOverride

            Catch ex As Exception
                Throw
            Finally
                oGetCurrencyOverrideRequest = Nothing
                oGetCurrencyOverrideResponse = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets insurance file information by insurance file count
    ''' </summary>
    Public Overrides Function GetInsuranceFileInformation(ByVal v_iInsuranceFileCnt As Integer,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As GetInsuranceFileInformation
        SyncLock oLock
            Dim oGetInsuranceFileInformationRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInsuranceFileInformationQuery
            Dim oGetInsuranceFileInformationResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInsuranceFileInformationQueryResponse
            Dim oResult As NexusProvider.GetInsuranceFileInformation
            Dim sbLogMessage As StringBuilder

            Try
                oGetInsuranceFileInformationRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInsuranceFileInformationQuery
                oGetInsuranceFileInformationResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInsuranceFileInformationQueryResponse
                oResult = New NexusProvider.GetInsuranceFileInformation
                sbLogMessage = New StringBuilder

                With oGetInsuranceFileInformationRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFileCnt = v_iInsuranceFileCnt
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetInsuranceFileInformation, oGetInsuranceFileInformationRequest)
                    oGetInsuranceFileInformationResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInsuranceFileInformationQueryResponse)(result)
                End Using

                With oResult
                    .CompanyId = oGetInsuranceFileInformationResponse.CompanyId
                    .AccountId = oGetInsuranceFileInformationResponse.AccountId
                    .CurrencyId = oGetInsuranceFileInformationResponse.CurrencyId
                    .Premium = oGetInsuranceFileInformationResponse.Premium
                    .CurrencyBaseXrate = oGetInsuranceFileInformationResponse.CurrencyBaseXrate
                    .CurrencyBaseDate = oGetInsuranceFileInformationResponse.CurrencyBaseDate
                    .AccountBaseXrate = oGetInsuranceFileInformationResponse.AccountBaseXrate
                    .AccountBaseDate = oGetInsuranceFileInformationResponse.AccountBaseDate
                    .SystemBaseXrate = oGetInsuranceFileInformationResponse.SystemBaseXrate
                    .SystemBaseDate = oGetInsuranceFileInformationResponse.SystemBaseDate
                    .RateOverrideReasonId = oGetInsuranceFileInformationResponse.RateOverrideReasonId
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetInsuranceFileInformation executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileCnt = " & v_iInsuranceFileCnt.ToString & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                Return oResult

            Catch ex As Exception
                Throw
            Finally
                oGetInsuranceFileInformationRequest = Nothing
                oGetInsuranceFileInformationResponse = Nothing
                oResult = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets account ID from party count
    ''' </summary>
    Public Overrides Function GetAccountIdFromPartyCnt(ByVal v_iPartyCnt As Integer,
                                                       ByVal v_iCompanyId As Integer,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As Integer
        SyncLock oLock
            Dim oGetAccountIdFromPartyCntRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAccountIdFromPartyCntQuery
            Dim oGetAccountIdFromPartyCntResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAccountIdFromPartyCntQueryResponse
            Dim sbLogMessage As StringBuilder

            Try
                oGetAccountIdFromPartyCntRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAccountIdFromPartyCntQuery
                oGetAccountIdFromPartyCntResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAccountIdFromPartyCntQueryResponse
                sbLogMessage = New StringBuilder

                With oGetAccountIdFromPartyCntRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If

                    .PartyCnt = v_iPartyCnt
                    .CompanyId = v_iCompanyId
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetAccountIdFromPartyCnt, oGetAccountIdFromPartyCntRequest)
                    oGetAccountIdFromPartyCntResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetAccountIdFromPartyCntQueryResponse)(result)
                End Using

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAccountIdFromPartyCnt executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyCnt = " & v_iPartyCnt.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iCompanyId = " & v_iCompanyId.ToString & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                Return oGetAccountIdFromPartyCntResponse.AccountId


            Catch ex As Exception
                Throw
            Finally
                oGetAccountIdFromPartyCntRequest = Nothing
                oGetAccountIdFromPartyCntResponse = Nothing
            End Try
        End SyncLock
    End Function

    ''' <summary>
    ''' Performs currency conversion
    ''' </summary>
    Public Overrides Function DoCurrencyConversion(ByVal v_iAccountId As Integer,
                                                   ByVal v_iCompanyId As Integer,
                                                   ByVal v_iCurrencyId As Integer,
                                                   ByVal v_dCurrencyAmountUnrounded As Decimal,
                                                   ByVal v_iBaseCurrencyID As Integer,
                                                   ByVal v_cBaseAmount As Decimal,
                                                   ByVal v_iAccountCurrencyID As Integer,
                                                   ByVal v_cAccountAmount As Decimal,
                                                   ByVal v_iSystemCurrencyID As Integer,
                                                   ByVal v_cSystemAmount As Decimal,
                                                   ByVal v_dCurrencyBaseXrate As Double,
                                                   ByVal v_dtCurrencyBaseDate As Date,
                                                   ByVal v_dAccountBaseXrate As Double,
                                                   ByVal v_dtAccountBaseDate As Date,
                                                   ByVal v_dSystemBaseXrate As Double,
                                                   ByVal v_dtSystemBaseDate As Date,
                                                   Optional ByVal v_sBranchCode As String = Nothing) As DoCurrencyConversion
        SyncLock oLock
            Dim oDoCurrencyConversionRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.DoCurrencyConversionQuery
            Dim oDoCurrencyConversionResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.DoCurrencyConversionQueryResponse
            Dim oResult As DoCurrencyConversion = New DoCurrencyConversion
            Dim sbLogMessage As StringBuilder

            Try
                oDoCurrencyConversionRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DoCurrencyConversionQuery
                oDoCurrencyConversionResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DoCurrencyConversionQueryResponse
                sbLogMessage = New StringBuilder

                With oDoCurrencyConversionRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If

                    .AccountId = v_iAccountId
                    .CompanyId = v_iCompanyId
                    .CurrencyId = v_iCurrencyId
                    .CurrencyAmountUnrounded = v_dCurrencyAmountUnrounded
                    .BaseCurrencyID = v_iBaseCurrencyID
                    .BaseAmount = v_cBaseAmount
                    .AccountCurrencyID = v_iAccountCurrencyID
                    .AccountAmount = v_cAccountAmount
                    .SystemCurrencyID = v_iSystemCurrencyID
                    .SystemAmount = v_cSystemAmount
                    .CurrencyBaseXrate = v_dCurrencyBaseXrate
                    .CurrencyBaseDate = v_dtCurrencyBaseDate
                    .AccountBaseXrate = v_dAccountBaseXrate
                    .AccountBaseDate = v_dtAccountBaseDate
                    .SystemBaseXrate = v_dSystemBaseXrate
                    .SystemBaseDate = v_dtSystemBaseDate
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.DoCurrencyConversion, oDoCurrencyConversionRequest)
                    oDoCurrencyConversionResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.DoCurrencyConversionQueryResponse)(result)
                End Using

                With oResult
                    .BaseCurrencyId = oDoCurrencyConversionResponse.BaseCurrencyId
                    .BaseAmount = oDoCurrencyConversionResponse.BaseAmount
                    .AccountCurrencyId = oDoCurrencyConversionResponse.AccountCurrencyId
                    .AccountAmount = oDoCurrencyConversionResponse.AccountAmount
                    .SystemCurrencyId = oDoCurrencyConversionResponse.SystemCurrencyId
                    .SystemAmount = oDoCurrencyConversionResponse.SystemAmount
                    .CurrencyBaseXrate = oDoCurrencyConversionResponse.CurrencyBaseXrate
                    .CurrencyBaseDate = oDoCurrencyConversionResponse.CurrencyBaseDate
                    .AccountBaseXrate = oDoCurrencyConversionResponse.AccountBaseXrate
                    .AccountBaseDate = oDoCurrencyConversionResponse.AccountBaseDate
                    .SystemBaseXrate = oDoCurrencyConversionResponse.SystemBaseXrate
                    .SystemBaseDate = oDoCurrencyConversionResponse.SystemBaseDate
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DoCurrencyConversion executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iAccountId = " & v_iAccountId.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iCompanyId = " & v_iCompanyId.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iCurrencyId = " & v_iCurrencyId.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_dCurrencyAmountUnrounded = " & v_dCurrencyAmountUnrounded.ToString & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                Return oResult

            Catch ex As Exception
                Throw
            Finally
                oDoCurrencyConversionRequest = Nothing
                oDoCurrencyConversionResponse = Nothing
            End Try
        End SyncLock
    End Function


End Class
