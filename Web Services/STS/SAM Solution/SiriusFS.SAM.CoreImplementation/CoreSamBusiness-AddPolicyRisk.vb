Option Strict Off

Imports dPMDAOBridge
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SSP.Shared

Partial Public Class CoreSAMBusiness

    ''' <summary>   
    ''' This method will call another method where the real business happens
    '''</summary>
    '''<param name="oGetDefaultRiskClausesInput"> It is an object of an class BaseGetDefaultRiskClausesRequestType></param>    
    '''<remarks></remarks>
    Public Overloads Function GetDefaultRiskClauses(ByVal oGetDefaultRiskClausesInput As BaseGetDefaultRiskClausesRequestType) As BaseGetDefaultRiskClausesResponseType

        ' validate the request structure against the specified business rules
        Using conRiskTypeClauses As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Dim oResponse As BaseGetDefaultRiskClausesResponseType
            oResponse = GetDefaultRiskClauses(conRiskTypeClauses, oGetDefaultRiskClausesInput)

            Return oResponse
        End Using
    End Function
    ''' <summary>   
    ''' This method will do the business
    '''</summary>
    '''<param name="oGetDefaultRiskClausesInput"> It is an object of an class BaseGetDefaultRiskClausesRequestType></param>   
    '''<param name="conRiskTypeClauses"> It is an object of an class SiriusConnection></param>    
    '''<remarks></remarks>

    Public Overloads Function GetDefaultRiskClauses(ByVal conRiskTypeClauses As SiriusConnection, ByVal oGetDefaultRiskClausesInput As BaseGetDefaultRiskClausesRequestType) As BaseGetDefaultRiskClausesResponseType

        Dim oGetDefaultRiskClausesResponse As BaseImplementationTypes.BaseGetDefaultRiskClausesResponseType
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage

        If oGetDefaultRiskClausesInput.GetType Is GetType(SAMForInsuranceImplementationTypes.GetDefaultRiskClausesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetDefaultRiskClausesResponse = New SAMForInsuranceImplementationTypes.GetDefaultRiskClausesResponseType
        ElseIf oGetDefaultRiskClausesInput.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetDefaultRiskClausesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oGetDefaultRiskClausesResponse = New SAMForInsuranceV2ImplementationTypes.GetDefaultRiskClausesResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If
        '************************************************************************************************
        'Mandatory Validation
        '************************************************************************************************
        oGetDefaultRiskClausesInput.Validate(CObj(oSAMErrorCollection))

        oSAMErrorCollection.CheckForErrors()

        '************************************************************************************************
        'Data Validation
        '************************************************************************************************

        oGetDefaultRiskClausesInput.RiskTypeKey = GetAndValidateSpecifiedTableCode(conRiskTypeClauses, "Risk_Type", "Risk_Type_id", "code", oGetDefaultRiskClausesInput.RiskTypeCode,
         oSAMErrorCollection, "Risk Type Code")
        oGetDefaultRiskClausesInput.CurrentBranchKey = GetAndValidateSpecifiedTableCode(conRiskTypeClauses, "source", "source_id", "code", oGetDefaultRiskClausesInput.CurrentBranchCode,
         oSAMErrorCollection, "Current Branch Code")

        oSAMErrorCollection.CheckForErrors()

        Dim dsRiskTypeClauses As DataSet
        Dim bDefault As Boolean = True
        'Execute the stored procedure "spu_Risk_Type_Linked_Clauses_Sel_New"
        Using conRiskTypeClauses
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Risk_Type_Linked_Clauses_Sel_New")
                cmd.AddInParameter("@Risk_Type_Id", SqlDbType.Int).Value = oGetDefaultRiskClausesInput.RiskTypeKey
                cmd.AddInParameter("@Branch_Id", SqlDbType.Int).Value = oGetDefaultRiskClausesInput.CurrentBranchKey
                cmd.AddInParameter("@Default", SqlDbType.Bit).Value = bDefault
                dsRiskTypeClauses = conRiskTypeClauses.ExecuteDataSet(cmd, "RiskClauses")
            End Using
        End Using

        If (dsRiskTypeClauses IsNot Nothing AndAlso dsRiskTypeClauses.Tables.Count > 0 AndAlso dsRiskTypeClauses.Tables(0).Rows.Count > 0) Then
            oGetDefaultRiskClausesResponse.Documents = New BaseGetDefaultRiskClausesResponseTypeDocuments
            For iRowCount As Integer = 0 To dsRiskTypeClauses.Tables(0).Rows.Count - 1
                With dsRiskTypeClauses.Tables(0)
                    ReDim Preserve oGetDefaultRiskClausesResponse.Documents.Row(iRowCount)
                    oGetDefaultRiskClausesResponse.Documents.Row(iRowCount) = New BaseGetDefaultRiskClausesResponseTypeDocumentsRow
                    oGetDefaultRiskClausesResponse.Documents.Row(iRowCount).Code = Cast.ToString(.Rows(iRowCount).Item("code"), Nothing)
                    oGetDefaultRiskClausesResponse.Documents.Row(iRowCount).Description = Cast.ToString(.Rows(iRowCount).Item("Description"), Nothing)
                End With
            Next

        End If
        Return oGetDefaultRiskClausesResponse
    End Function

    ' ***************************************************************** '
    ' Name: AddPolicy
    '
    ' Description: This method accepts the base policy implementation type 
    '              and adds a new policy and risk records.
    '
    ' ***************************************************************** '
    Public Overloads Function AddPolicy(ByVal AddPolicyRequest As BaseQuoteRiskMsgType) As BaseNBQuoteResponseTypePolicy
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseNBQuoteResponseTypePolicy

            oResponse = AddPolicy(con, AddPolicyRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function AddPolicy(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType) As BaseNBQuoteResponseTypePolicy

        'Declare the Response object
        Dim oResponse As New BaseImplementationTypes.BaseNBQuoteResponseTypePolicy
        Dim oQuoteRiskResponse As BaseRiskResultType

        ' Declare the Core Business object
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)

        Const ACMethodName As String = "AddPolicy"
        Dim Utils As New Utilities

        'Dim iRiskCodeId As Int32
        'Dim iScreenId As Int32
        'Dim iProductId As Int32
        'Dim iSourceId As Int32
        'Dim STSListType As Core.STSListType = Nothing

        Dim STSError As New STSErrorPublisher

        Dim oAddQuoteIn As New SAMForInsuranceImplementationTypes.AddQuoteRequestType
        Dim oAddQuoteOut As SAMForInsuranceImplementationTypes.AddQuoteResponseType

        ' Set the inputs for the AddQuote method
        With oAddQuoteIn
            .BranchCode = AddPolicyRequest.BranchCode
            .CoverEndDate = AddPolicyRequest.CoverEndDate
            .CoverStartDate = AddPolicyRequest.CoverStartDate
            .Description = AddPolicyRequest.Description
            .InsuredName = AddPolicyRequest.InsuredName
            .PartyKey = AddPolicyRequest.PartyKey
            .ProductCode = AddPolicyRequest.ProductCode
            .QuoteRef = AddPolicyRequest.QuoteRef
            .PolicyStatusCode = AddPolicyRequest.PolicyStatusCode
            .InsuranceFolderKey = AddPolicyRequest.InsuranceFolderKey
            .PolicyVersion = AddPolicyRequest.PolicyVersion
            .LapsedReasonCode = AddPolicyRequest.LapsedReasonCode
            .LapsedDate = AddPolicyRequest.LapsedDate
            .LapsedDateSpecified = AddPolicyRequest.LapsedDateSpecified
            .LapsedReasonDescription = AddPolicyRequest.LapsedReasonDescription
            .InceptionDate = AddPolicyRequest.InceptionDate
            .InceptionDateSpecified = AddPolicyRequest.InceptionDateSpecified
            .InceptionDateTPI = AddPolicyRequest.InceptionDateTPI
            .InceptionDateTPISpecified = AddPolicyRequest.InceptionDateTPISpecified
            .RenewalDate = AddPolicyRequest.RenewalDate
            .RenewalDateSpecified = AddPolicyRequest.RenewalDateSpecified
            .AlternateReference = AddPolicyRequest.AlternateReference
            .OldPolicyNumber = AddPolicyRequest.OldPolicyNumber
            .AccountExecutiveShortname = AddPolicyRequest.AccountExecutiveShortname
            .AnalysisCode = AddPolicyRequest.AnalysisCode
            .AgentKey = AddPolicyRequest.AgentKey
            .AgentKeySpecified = AddPolicyRequest.AgentKeySpecified
            .AccountHandlerShortname = AddPolicyRequest.AccountHandlerShortname
            .PolicyVersionTypeCode = AddPolicyRequest.PolicyVersionTypeCode
            .Regarding = AddPolicyRequest.LastTransDescription
            .CoInsurancePlacement = AddPolicyRequest.CoInsurancePlacement
        End With

        ' Call the AddQuote method
        oAddQuoteOut = DirectCast(AddQuote(con, oAddQuoteIn), SAMForInsuranceImplementationTypes.AddQuoteResponseType)

        ' Move AddQuote response to AddPolicy response
        With oResponse
            .InsuranceFileKey = oAddQuoteOut.InsuranceFileKey
            .InsuranceFolderKey = oAddQuoteOut.InsuranceFolderKey
            .PolicyID = oAddQuoteOut.InsuranceFileKey
            .QuoteRef = oAddQuoteOut.InsuranceFileRef
            .STSError = oAddQuoteOut.STSError
            If Not .STSError Is Nothing Then
                ' Error
                Return oResponse
            End If
        End With

        Dim iCnt As Integer
        Dim iLBnd As Integer
        Dim iUBnd As Integer

        Dim oAddRiskRequest As BaseAddRiskRequestType
        Dim oAddRiskResponse As BaseAddRiskResponseType
        'Dim oBasePolicyDataImportRequest As BasePolicyDataImportRequestType

        ' Process the address structure
        If AddPolicyRequest.Risks IsNot Nothing Then

            iLBnd% = AddPolicyRequest.Risks.GetLowerBound(0)
            iUBnd% = AddPolicyRequest.Risks.GetUpperBound(0)

            ReDim oResponse.Risks(iUBnd%)

            For iCnt% = iLBnd% To iUBnd%

                ' Add the risk
                oAddRiskRequest = New BaseImplementationTypes.BaseAddRiskRequestType
                oAddRiskRequest.DataModelCode = AddPolicyRequest.Risks(iCnt%).DataModelCode
                oAddRiskRequest.QuoteTimeStamp = AddPolicyRequest.Risks(iCnt%).QuoteTimeStamp
                oAddRiskRequest.RiskDescription = AddPolicyRequest.Risks(iCnt%).RiskDescription
                oAddRiskRequest.RiskTypeCode = AddPolicyRequest.Risks(iCnt%).RiskTypeCode
                oAddRiskRequest.RunDefaultRules = AddPolicyRequest.Risks(iCnt%).RunDefaultRules
                oAddRiskRequest.ScreenCode = AddPolicyRequest.Risks(iCnt%).ScreenCode
                oAddRiskRequest.XMLDataSet = AddPolicyRequest.Risks(iCnt%).XMLDataSet
                oAddRiskRequest.InsuranceFileKey = oAddQuoteOut.InsuranceFileKey
                oAddRiskRequest.InsuranceFolderKey = oAddQuoteOut.InsuranceFolderKey
                oAddRiskRequest.ProductCode = AddPolicyRequest.ProductCode
                oAddRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                oAddRiskRequest.RatingSections = AddPolicyRequest.Risks(iCnt%).RatingSections
                oAddRiskRequest.RIArrangement = AddPolicyRequest.Risks(iCnt%).RIArrangement
                oAddRiskRequest.Data_Transfer = AddPolicyRequest.DataTransfer
                oAddRiskRequest.IsMarketplacePolicy = AddPolicyRequest.IsMarketPlacePolicy

                oAddRiskResponse = AddRisk(con, oAddRiskRequest)

                ' Convert the risk response to the baserisktype required on the quote response
                oQuoteRiskResponse = New BaseImplementationTypes.BaseRiskResultType
                oQuoteRiskResponse.RiskFolderID = oAddRiskResponse.RiskFolderKey
                oQuoteRiskResponse.RiskID = oAddRiskResponse.RiskKey
                oQuoteRiskResponse.STSError = oAddRiskResponse.STSError
                oQuoteRiskResponse.XMLDataSet = oAddRiskResponse.XMLDataSet

                ' TODO : MEVANS : NEED TO RETURN THE SAM STAGING RISK KEY 
                oQuoteRiskResponse.SAMStagingRiskKey = AddPolicyRequest.Risks(iCnt%).SAMStagingRiskKey

                oResponse.Risks(iCnt%) = oQuoteRiskResponse

                If oQuoteRiskResponse.STSError IsNot Nothing Then
                    ' Error
                    oResponse.STSError = oResponse.Risks(iCnt).STSError
                    Return oResponse
                End If

            Next iCnt%
        End If

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        Dim PolicyPremiumDueGross As Decimal = 0
        Dim PolicyPremiumDueNet As Decimal = 0
        Dim PolicyPremiumDueTax As Decimal = 0
        Dim PolicyTotalAnnualTax As Decimal = 0
        Dim PolicyCommissionAmount As Decimal = 0

        Dim oGetQuoteRisksIn As New GetQuoteRisksIn
        Dim oGetQuoteRisksOut As GetQuoteRisksOut

        oGetQuoteRisksIn.DataModelCode = "AOL"
        oGetQuoteRisksIn.BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
        oGetQuoteRisksIn.InsuranceFileCnt = oAddQuoteOut.InsuranceFileKey

        oGetQuoteRisksOut = oCoreBusiness.GetQuoteRisks(con, oGetQuoteRisksIn)

        ' Process the risks structure
        If IsArray(oResponse.Risks) = True Then

            iLBnd% = oResponse.Risks.GetLowerBound(0)
            iUBnd% = oResponse.Risks.GetUpperBound(0)

            For iCnt% = iLBnd% To iUBnd%

                oQuoteRiskResponse = oResponse.Risks(iCnt%)

                ' Get the DataView For the table
                dv = oGetQuoteRisksOut.Quotes.Tables(0).DefaultView

                ' Specify the Filter
                sFilter = "col1 = " + oQuoteRiskResponse.RiskID.ToString

                Try
                    ' Create a New Data View filtered by the ID
                    dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

                    If dvID Is Nothing Then
                        Dim STSErrorEx As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No risk details found for RiskID : " + oQuoteRiskResponse.RiskID.ToString)
                        STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "DataView", True)
                    End If
                Catch ex As Exception
                    Dim STSErrorEx As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No risk details found for RiskID : " + oQuoteRiskResponse.RiskID.ToString)
                    STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
                End Try

                ' Return the Premium Details
                oQuoteRiskResponse.PremiumDueGross = CDec(Val(Cast.DefaultIfNull(dvID.Item(0).Item(8), 0))) + CDec(Val(Cast.DefaultIfNull(dvID.Item(0).Item(22), 0)))
                oQuoteRiskResponse.PremiumDueNet = CDec(Val(Cast.DefaultIfNull(dvID.Item(0).Item(8), 0)))
                oQuoteRiskResponse.PremiumDueTax = CDec(Val(Cast.DefaultIfNull(dvID.Item(0).Item(22), 0)))
                oQuoteRiskResponse.TotalAnnualTax = CDec(Val(Cast.DefaultIfNull(dvID.Item(0).Item(22), 0)))
                oQuoteRiskResponse.CommissionAmount = CDec(Val(Cast.DefaultIfNull(dvID.Item(0).Item(23), 0)))

                PolicyPremiumDueGross = PolicyPremiumDueGross + oQuoteRiskResponse.PremiumDueGross
                PolicyPremiumDueNet = PolicyPremiumDueNet + oQuoteRiskResponse.PremiumDueNet
                PolicyPremiumDueTax = PolicyPremiumDueTax + oQuoteRiskResponse.PremiumDueTax
                PolicyTotalAnnualTax = PolicyTotalAnnualTax + oQuoteRiskResponse.TotalAnnualTax
                PolicyCommissionAmount = PolicyCommissionAmount + oQuoteRiskResponse.CommissionAmount

                oResponse.Risks(iCnt%) = oQuoteRiskResponse

            Next iCnt%
        End If

        oResponse.PremiumDueGross = PolicyPremiumDueGross
        oResponse.PremiumDueNet = PolicyPremiumDueNet
        oResponse.PremiumDueTax = PolicyPremiumDueTax
        oResponse.TotalAnnualTax = PolicyTotalAnnualTax
        oResponse.CommissionAmount = PolicyCommissionAmount

        Return oResponse

    End Function

    Private Sub GetRenewalStatusForPolicy(ByVal con As SiriusConnection,
                                          ByVal insuranceFileCnt As Integer,
                                          ByRef policyRenewalStatusTypeCode As String,
                                          ByRef policyRenewalInsuranceFileCnt As Integer,
                                          ByRef policyRenewalStatusCnt As Integer)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_renewal_for_policy")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileCnt
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.Rows(0)

            policyRenewalStatusTypeCode = Cast.ToString(dr.Item("code"), "")
            policyRenewalInsuranceFileCnt = Cast.ToInt32(dr.Item("renewal_insurance_file_cnt"), 0)
            policyRenewalStatusCnt = Cast.ToInt32(dr.Item("renewal_status_cnt"), 0)

        End If

    End Sub

    Private Sub AddPolicyV2UpdateQuote(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType, ByVal oResponse As BaseImplementationTypes.BaseNBQuoteResponseTypePolicy)

        Dim oUpdateQuoteIn As New SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2RequestType
        Dim oUpdateQuoteOut As SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2ResponseType
        Dim iProductId As Integer
        Dim iMidNightRenewal As Integer
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim oGetProductRiskOptionValueRequest As New BaseProductRiskOptionValueRequestType
        iProductId = GetAndValidateSpecifiedTableCode(con, "product", "product_id", "code", AddPolicyRequest.ProductCode, oSAMErrorCollection, "Current Product Code")

        oSAMErrorCollection.CheckForErrors()

        If iProductId > 0 Then
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsMidnightRenewal
            iMidNightRenewal = Convert.ToInt32(GetProductRiskOptions(con, iProductId, oGetProductRiskOptionValueRequest))
        End If
        ' Set the inputs for the UpdateQuote method
        With oUpdateQuoteIn
            .BranchCode = AddPolicyRequest.BranchCode
            .CoverEndDate = AddPolicyRequest.CoverEndDate
            .CoverStartDate = AddPolicyRequest.CoverStartDate
            .Description = AddPolicyRequest.Description
            .InsuredName = AddPolicyRequest.InsuredName
            .PartyKey = AddPolicyRequest.PartyKey
            .InsuranceFileKey = oResponse.InsuranceFileKey
            .InsuranceFolderKey = AddPolicyRequest.InsuranceFolderKey
            .ProductCode = AddPolicyRequest.ProductCode
            If String.IsNullOrEmpty(AddPolicyRequest.NewQuoteRef) = False Then
                .QuoteRef = AddPolicyRequest.NewQuoteRef
            Else
                .QuoteRef = AddPolicyRequest.QuoteRef
            End If
            .PolicyStatusCode = AddPolicyRequest.PolicyStatusCode
            '.InceptionDate = AddPolicyRequest.InceptionDate
            '.RenewalDate = AddPolicyRequest.RenewalDate
            '.AnalysisCode = AddPolicyRequest.AnalysisCode
            .AgentKey = AddPolicyRequest.AgentKey
            .AgentKeySpecified = AddPolicyRequest.AgentKeySpecified
            .CurrencyCode = AddPolicyRequest.CurrencyCode
            .SubBranchCode = AddPolicyRequest.BranchCode
            .AlternativeRef = AddPolicyRequest.AlternativeRef
            If AddPolicyRequest.IsBDXRequest Then
                If AddPolicyRequest.AgentKeySpecified AndAlso AddPolicyRequest.AgentKey <> 0 Then
                    If Not String.IsNullOrEmpty(AddPolicyRequest.BusinessTypeCode) AndAlso
                        AddPolicyRequest.BusinessTypeCode.Trim.ToUpper = "COIN FOLL" OrElse
                        AddPolicyRequest.BusinessTypeCode.Trim.ToUpper = "COIN LEAD" Then
                        .BusinessTypeCode = AddPolicyRequest.BusinessTypeCode
                    Else
                        .BusinessTypeCode = "AGENCY"
                    End If
                Else
                    .BusinessTypeCode = "DIRECT"
                End If
            Else
                .BusinessTypeCode = IIf(AddPolicyRequest.AgentKeySpecified And AddPolicyRequest.AgentKey <> 0, "AGENCY", "DIRECT").ToString
            End If
            .QuoteExpiryDate = AddPolicyRequest.CoverStartDate.AddDays(30)
            .AnalysisCode = AddPolicyRequest.AnalysisCode
            .Regarding = AddPolicyRequest.LastTransDescription
            .OldPolicyNumber = AddPolicyRequest.OldPolicyNumber
            .UnderwritingYearCode = AddPolicyRequest.UnderwritingYearCode

            If AddPolicyRequest.RenewalDateSpecified = True Then
                .RenewalDate = AddPolicyRequest.RenewalDate
            Else
                If iMidNightRenewal = 1 Then
                    .RenewalDate = AddPolicyRequest.CoverEndDate.AddDays(1)
                Else
                    .RenewalDate = AddPolicyRequest.CoverEndDate
                End If
            End If
            If AddPolicyRequest.TransactionTypeCode <> "REN" Then
                If AddPolicyRequest.InceptionDateSpecified = True Then
                    .InceptionDate = AddPolicyRequest.InceptionDate
                Else
                    .InceptionDate = AddPolicyRequest.CoverStartDate
                End If
                If AddPolicyRequest.InceptionDateSpecified = True Then
                    .InceptionTPI = AddPolicyRequest.InceptionDateTPI
                Else
                    .InceptionTPI = .InceptionDate
                End If
            Else
                .InceptionTPI = AddPolicyRequest.CoverStartDate
            End If

            .FrequencyCode = "ANNUAL"

            .Timestamp = oResponse.QuoteTimeStamp
            .CoInsurancePlacement = AddPolicyRequest.CoInsurancePlacement 'WPR11
            .IsMarketPlacePolicy = AddPolicyRequest.IsMarketPlacePolicy
            .CoInsurancePlacement = AddPolicyRequest.CoInsurancePlacement
        End With

        ' Call the UpdateQuote method
        oUpdateQuoteOut = DirectCast(UpdateQuoteV2(con, oUpdateQuoteIn), SAMForInsuranceV2ImplementationTypes.UpdateQuoteV2ResponseType)

        ' Move UpdateQuote response to AddPolicy response
        With oResponse
            .QuoteRef = oUpdateQuoteOut.InsuranceFileRef
            .STSError = oUpdateQuoteOut.STSError
            .QuoteTimeStamp = oUpdateQuoteOut.TimeStamp
        End With

    End Sub
    ''' <summary>
    ''' AddPolicyV2AddQuote
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="AddPolicyRequest"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks></remarks>
    Private Sub AddPolicyV2AddQuote(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType, ByVal oResponse As BaseImplementationTypes.BaseNBQuoteResponseTypePolicy)

        Dim oAddQuoteIn As New SAMForInsuranceV2ImplementationTypes.AddQuoteV2RequestType
        Dim oAddQuoteOut As SAMForInsuranceV2ImplementationTypes.AddQuoteV2ResponseType
        Dim iProductCodeId As Integer
        Dim oErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nGracePeriod As Integer = 30
        If Not String.IsNullOrEmpty(AddPolicyRequest.ProductCode) Then
            Try
                iProductCodeId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Product", AddPolicyRequest.ProductCode)
            Catch ex As Exception
                oErrorCollection.AddInvalidData(SAMInvalidData.InvalidLookupListValue,
                                                        SAMInvalidData.InvalidLookupListValue.ToString,
                                                        "ProductCode",
                                                        AddPolicyRequest.ProductCode)
            End Try
        End If

        oCoreBusiness = Nothing
        oErrorCollection.CheckForErrors()
        Dim oGetProductRiskOptionValueRequest As New BaseProductRiskOptionValueRequestType
        Dim bIsMidNightRenewal As Boolean = False
        If iProductCodeId > 0 Then
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.IsMidnightRenewal
            bIsMidNightRenewal = Cast.ToBoolean(Convert.ToInt32(GetProductRiskOptions(con, iProductCodeId, oGetProductRiskOptionValueRequest)), False)
            oGetProductRiskOptionValueRequest.ProducRiskOption = ProductRiskOptions.GracePeriod
            nGracePeriod = Convert.ToInt32(GetProductRiskOptions(con, iProductCodeId, oGetProductRiskOptionValueRequest))
        End If
        ' Set the inputs for the AddQuote method
        With oAddQuoteIn
            .BranchCode = AddPolicyRequest.BranchCode
            .CoverEndDate = AddPolicyRequest.CoverEndDate
            .CoverStartDate = AddPolicyRequest.CoverStartDate
            .Description = AddPolicyRequest.Description
            .InsuredName = AddPolicyRequest.InsuredName
            .PartyKey = AddPolicyRequest.PartyKey
            .ProductCode = AddPolicyRequest.ProductCode
            .QuoteRef = AddPolicyRequest.QuoteRef
            '.PolicyStatusCode = AddPolicyRequest.PolicyStatusCode
            .InceptionDate = AddPolicyRequest.InceptionDate
            .RenewalDate = AddPolicyRequest.RenewalDate
            .AnalysisCode = AddPolicyRequest.AnalysisCode
            .AgentKey = AddPolicyRequest.AgentKey
            .AgentKeySpecified = AddPolicyRequest.AgentKeySpecified
            .CurrencyCode = AddPolicyRequest.CurrencyCode
            .SubBranchCode = AddPolicyRequest.BranchCode
            .IsBDXRequest = AddPolicyRequest.IsBDXRequest
            If AddPolicyRequest.IsBDXRequest Then
                If AddPolicyRequest.AgentKeySpecified And AddPolicyRequest.AgentKey <> 0 Then
                    If Not String.IsNullOrEmpty(AddPolicyRequest.BusinessTypeCode) AndAlso
                        AddPolicyRequest.BusinessTypeCode.Trim.ToUpper = "COIN FOLL" OrElse
                        AddPolicyRequest.BusinessTypeCode.Trim.ToUpper = "COIN LEAD" Then
                        .BusinessTypeCode = AddPolicyRequest.BusinessTypeCode
                    Else
                        .BusinessTypeCode = "AGENCY"
                    End If
                Else
                    .BusinessTypeCode = "DIRECT"
                End If
            Else
                .BusinessTypeCode = IIf(AddPolicyRequest.AgentKeySpecified And AddPolicyRequest.AgentKey <> 0, "AGENCY", "DIRECT").ToString
            End If
            .QuoteExpiryDate = AddPolicyRequest.CoverStartDate.AddDays(30)
            .OldPolicyNumber = AddPolicyRequest.OldPolicyNumber
            .UnderwritingYearCode = AddPolicyRequest.UnderwritingYearCode
            If AddPolicyRequest.RenewalDateSpecified = True Then
                .RenewalDate = AddPolicyRequest.RenewalDate
            Else
                If bIsMidNightRenewal Then
                    .RenewalDate = AddPolicyRequest.CoverEndDate.AddDays(1)
                Else
                    .RenewalDate = AddPolicyRequest.CoverEndDate
                End If
            End If
            If AddPolicyRequest.InceptionDateSpecified = True Then
                .InceptionDate = AddPolicyRequest.InceptionDate
            Else
                .InceptionDate = AddPolicyRequest.CoverStartDate
            End If
            If AddPolicyRequest.InceptionDateSpecified = True Then
                .InceptionTPI = AddPolicyRequest.InceptionDateTPI
            Else
                .InceptionTPI = .InceptionDate
            End If
            .AlternativeRef = AddPolicyRequest.AlternativeRef
            .FrequencyCode = "ANNUAL"
            If ToSafeString(AddPolicyRequest.CollectionFrequencyCode) <> "" Then
                .CollectionFrequencyCode = AddPolicyRequest.CollectionFrequencyCode
            End If
            .PaymentTermCode = AddPolicyRequest.PaymentTermCode
            .Regarding = AddPolicyRequest.LastTransDescription
            .IsMarketPlacePolicy = AddPolicyRequest.IsMarketPlacePolicy
            .CoInsurancePlacement = AddPolicyRequest.CoInsurancePlacement
        End With

        ' Call the AddQuote method
        oAddQuoteOut = DirectCast(AddQuoteV2(con, oAddQuoteIn), SAMForInsuranceV2ImplementationTypes.AddQuoteV2ResponseType)

        ' Move AddQuote response to AddPolicy response
        With oResponse
            .InsuranceFileKey = oAddQuoteOut.InsuranceFileKey
            .InsuranceFolderKey = oAddQuoteOut.InsuranceFolderKey
            .PolicyID = oAddQuoteOut.InsuranceFileKey
            .QuoteRef = oAddQuoteOut.InsuranceFileRef
            .STSError = oAddQuoteOut.STSError
            .QuoteTimeStamp = oAddQuoteOut.QuoteTimeStamp
            .SkipNewPolicyNumber = oAddQuoteOut.SkipNewPolicyNumber
        End With

    End Sub
    ''' <summary>
    '''  AddPolicyV2AddMTAQuote
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="AddPolicyRequest"></param>
    ''' <param name="oResponse"></param>
    ''' <remarks></remarks>
    Private Sub AddPolicyV2AddMTAQuote(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType, ByVal oResponse As BaseImplementationTypes.BaseNBQuoteResponseTypePolicy)

        Dim oAddQuoteIn As New SAMForInsuranceV2ImplementationTypes.AddMtaQuoteRequestType
        Dim oAddQuoteOut As SAMForInsuranceV2ImplementationTypes.AddMtaQuoteResponseType
        Dim nInsuranceFileTypeKey As Integer = 0
        Dim sInsuranceFileTypeCode As String = String.Empty

        Dim iCnt As Integer
        Dim iLBnd As Integer
        Dim iUBnd As Integer
        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Current_Policy_Version")
            cmd.AddInParameter("@InsuranceFolderCnt", SqlDbType.Int).Value = AddPolicyRequest.InsuranceFolderKey
            cmd.AddInParameter("@InsuranceFileCnt", SqlDbType.Int).Value = oAddQuoteIn.InsuranceFileKey

            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            oAddQuoteIn.InsuranceFileKey = Cast.ToInt32(dt.Rows(0).Item(0), 0)
        End If
        ' in case of MTR continue if previous version of policy is in cancelled state
        If AddPolicyRequest.IsBDXRequest Then
            If AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.ReinstatePolicy Then
                oAddQuoteIn.TransactionType = TransactionType.MTR
                GetInsuranceFileType(con, oAddQuoteIn.InsuranceFileKey, nInsuranceFileTypeKey, sInsuranceFileTypeCode)
                If sInsuranceFileTypeCode <> InsuranceFileType.MTACancellation Then
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyRecordNotFound, "Policy does not have cancelled status.", "")
                    STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "AddPolicyV2AddMTAQuote", "Policy Process Validation", True)
                    Exit Sub
                End If
            End If
        End If
        ' Set the inputs for the AddQuote method
        With oAddQuoteIn
            .BranchCode = AddPolicyRequest.BranchCode
            .ExpiryDate = AddPolicyRequest.CoverEndDate
            .EffectiveDate = AddPolicyRequest.CoverStartDate
            If AddPolicyRequest.MTAReasonCode Is Nothing Then
                .MtaReason = "OTHER"
            Else
                .MtaReason = AddPolicyRequest.MTAReasonCode
            End If
            .mtaReasonDescription = AddPolicyRequest.Description
            .InsuredName = AddPolicyRequest.InsuredName
            .PolicyStatusCode = AddPolicyRequest.PolicyStatusCode
            .AnalysisCode = AddPolicyRequest.AnalysisCode
            If AddPolicyRequest.IsBDXRequest Then
                If AddPolicyRequest.AgentKeySpecified And AddPolicyRequest.AgentKey <> 0 Then
                    If Not String.IsNullOrEmpty(AddPolicyRequest.BusinessTypeCode) AndAlso
                        AddPolicyRequest.BusinessTypeCode.Trim.ToUpper = "COIN FOLL" OrElse
                        AddPolicyRequest.BusinessTypeCode.Trim.ToUpper = "COIN LEAD" Then
                        .BusinessTypeCode = AddPolicyRequest.BusinessTypeCode
                    Else
                        .BusinessTypeCode = "AGENCY"
                    End If
                Else
                    .BusinessTypeCode = "DIRECT"
                End If
            Else
                .BusinessTypeCode = IIf(AddPolicyRequest.AgentKeySpecified And AddPolicyRequest.AgentKey <> 0, "AGENCY", "DIRECT").ToString
            End If
            .TypeOfMta = "PERMANENT"
            .QuoteExpiryDate = AddPolicyRequest.CoverStartDate.AddDays(30)
            .FrequencyCode = "ANNUAL"
            .PolicyKey = AddPolicyRequest.QuoteRef
            .AlternateReference = AddPolicyRequest.AlternateReference
            .OldPolicyNumber = AddPolicyRequest.OldPolicyNumber
            .Regarding = AddPolicyRequest.LastTransDescription
            .UnderWritingYearCode = AddPolicyRequest.UnderwritingYearCode
            .CoInsurancePlacement = AddPolicyRequest.CoInsurancePlacement
            .IsMarketPlacePolicy = AddPolicyRequest.IsMarketPlacePolicy
            .AnniversaryDate = AddPolicyRequest.AnniversaryDate
            .AnniversaryDateSpecified = AddPolicyRequest.AnniversaryDateSpecified
            [Enum].TryParse(AddPolicyRequest.TransactionTypeCode.Trim(), .TransactionType)
            .IsBDXRequest = AddPolicyRequest.IsBDXRequest
            If AddPolicyRequest.TransactionTypeCode = "MTC" Then
                .TransactionType = TransactionType.MTC
            End If
            If AddPolicyRequest.TransactionTypeCode = "MTR" Then
                .IsReinstatement = True
            End If
        End With

        ' Call the AddQuote method
        oAddQuoteOut = DirectCast(AddMtaQuote(oAddQuoteIn), SAMForInsuranceV2ImplementationTypes.AddMtaQuoteResponseType)

        ' Move AddQuote response to AddPolicy response
        With oResponse
            .InsuranceFileKey = oAddQuoteOut.InsuranceFileKey
            .InsuranceFolderKey = AddPolicyRequest.InsuranceFolderKey
            .PolicyID = oAddQuoteOut.InsuranceFileKey
            .QuoteRef = AddPolicyRequest.QuoteRef
            .STSError = oAddQuoteOut.STSError
            .QuoteTimeStamp = oAddQuoteOut.QuoteTimeStamp
        End With

        Dim quoteTimeStamp() As Byte = oResponse.QuoteTimeStamp
        quoteTimeStamp = oResponse.QuoteTimeStamp

        If AddPolicyRequest.IsBDXRequest AndAlso AddPolicyRequest.TransactionTypeCode = "MTC" Then
            If AddPolicyRequest.Risks IsNot Nothing Then

                iLBnd% = AddPolicyRequest.Risks.GetLowerBound(0)
                iUBnd% = AddPolicyRequest.Risks.GetUpperBound(0)


                '  If AddPolicyRequest.IsBDXRequest AndAlso AddPolicyRequest.TransactionTypeCode = "MTC" Then
                For iCnt% = iLBnd% To iUBnd%
                    Dim riskKey As Integer = 0
                    Dim riskFolderKey As Integer = AddPolicyRequest.Risks(iCnt%).RiskFolderKey
                    Dim OriginalRiskCnt As Integer = AddPolicyRequest.Risks(iCnt%).OriginalRiskKey

                    If riskFolderKey <> 0 Then
                        ' Dim dt1 As DataTable = Nothing
                        dt = Nothing
                        ' Try and identify the matching risk by the Risk Folder passed in.

                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_risk_cnt_by_folder")
                            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oResponse.InsuranceFileKey
                            cmd.AddInParameter("@risk_folder_cnt", SqlDbType.Int).Value = AddPolicyRequest.Risks(iCnt%).RiskFolderKey
                            dt = con.ExecuteDataTable(cmd)
                        End Using

                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                            Dim dr As DataRow = dt.Rows(0)

                            riskKey = Cast.ToInt32(dr.Item("risk_cnt"), 0)

                            Dim getRiskRequest As New BaseGetRiskRequestType
                            Dim getRiskResponse As BaseGetRiskResponseType

                            getRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                            getRiskRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                            getRiskRequest.InsuranceFolderKey = oResponse.InsuranceFolderKey
                            getRiskRequest.RiskKey = riskKey

                            getRiskRequest.QuoteTimeStamp = quoteTimeStamp

                            getRiskResponse = RetrieveRisk(con, getRiskRequest)

                            quoteTimeStamp = getRiskResponse.QuoteTimeStamp

                            Dim updateRiskRequest As New BaseUpdateRiskRequestType
                            Dim updateRiskResponse As New BaseUpdateRiskResponseType

                            updateRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                            updateRiskRequest.QuoteTimeStamp = quoteTimeStamp
                            updateRiskRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                            updateRiskRequest.InsuranceFolderKey = oResponse.InsuranceFolderKey
                            updateRiskRequest.RiskDescription = AddPolicyRequest.Risks(iCnt%).RiskDescription
                            updateRiskRequest.RiskKey = riskKey
                            updateRiskRequest.ScreenCode = AddPolicyRequest.Risks(iCnt%).ScreenCode
                            updateRiskRequest.XMLDataSet = getRiskResponse.XMLDataSet
                            updateRiskRequest.TransactionType = AddPolicyRequest.TransactionTypeCode
                            updateRiskRequest.SourceId = AddPolicyRequest.SourceId
                            updateRiskRequest.SubBranchCode = AddPolicyRequest.BranchCode
                            updateRiskResponse = UpdateRisk(con, updateRiskRequest)
                            quoteTimeStamp = updateRiskResponse.QuoteTimeStamp

                            AddPolicyRequest.Risks(iCnt%).RiskKey = riskKey

                        End If
                    End If
                Next
            End If
            oResponse.QuoteTimeStamp = quoteTimeStamp
        End If



    End Sub

    Private Sub AddPolicyV2AddRenewal(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType, ByVal oResponse As BaseImplementationTypes.BaseNBQuoteResponseTypePolicy)

        Dim insuranceFileCnt As Integer = 0
        Dim runRenewalSelectionByPolicyRequest As New BaseRunRenewalSelectionByPolicyRequestType
        Dim runRenewalSelectionByPolicyResponse As BaseRunRenewalSelectionByPolicyResponseType
        Dim oDeleteRenewalRequest As New SAMForInsuranceV2ImplementationTypes.DeleteRenewalRequestType
        Dim oDeleteRenewalResponseType As SAMForInsuranceV2ImplementationTypes.DeleteRenewalResponseType


        Dim ds As New DataSet
        Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Policy_Version_For_Renewal")
            Cmd.AddInParameter("@insurance_ref", SqlDbType.VarChar, 30).Value = AddPolicyRequest.QuoteRef
            ds = con.ExecuteDataSet(Cmd, "table1")
        End Using

        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            insuranceFileCnt = Convert.ToInt32(ds.Tables(0).Rows(0).Item(0).ToString)
            GetInsuranceFolderAndParty(con, AddPolicyRequest.QuoteRef, AddPolicyRequest.InsuranceFolderKey, 0)
            ds = Nothing
        Else
            ds = Nothing
            RaiseComMethodException("No Valid Version Exists For Renewal")
        End If

        If insuranceFileCnt <> 0 Then

            Dim policyRenewalStatusTypeCode As String = String.Empty
            Dim policyRenewalInsuranceFileCnt As Integer = 0
            Dim policyRenewalStatusCnt As Integer = 0

            GetRenewalStatusForPolicy(con, insuranceFileCnt, policyRenewalStatusTypeCode, policyRenewalInsuranceFileCnt, policyRenewalStatusCnt)
            If policyRenewalInsuranceFileCnt > 0 Then
                If AddPolicyRequest.DeletePolicyUnderRenewal = 1 Then

                    'delete policy under renewal if exists
                    oDeleteRenewalRequest.BranchCode = AddPolicyRequest.BranchCode
                    oDeleteRenewalRequest.InsuranceFileKey = policyRenewalInsuranceFileCnt
                    oDeleteRenewalRequest.QuoteTimeStamp = oResponse.QuoteTimeStamp
                    oDeleteRenewalResponseType = DeleteRenewal(con, oDeleteRenewalRequest)
                    policyRenewalStatusTypeCode = String.Empty
                    AddPolicyRequest.RenewalStatusCode = String.Empty
                End If
            End If


            If policyRenewalStatusTypeCode = String.Empty AndAlso AddPolicyRequest.RenewalStatusCode = String.Empty Then

                ' Set the inputs for the AddQuote method
                With runRenewalSelectionByPolicyRequest
                    .BranchCode = AddPolicyRequest.BranchCode
                    .InsuranceFileKey = insuranceFileCnt
                    .SetRenewalInviteAsSent = True
                    .SourceId = AddPolicyRequest.SourceId
                    .DoNotCopyRiskAtRenSelection = AddPolicyRequest.DoNotCopyRiskAtRenSelection
                    .Regarding = AddPolicyRequest.LastTransDescription
                    .SkipGenerateRenewalPolicyNumber = AddPolicyRequest.SkipGenerateRenewalPolicyNumber
                End With


                ' Call the AddQuote method
                runRenewalSelectionByPolicyResponse = RunRenewalSelectionByPolicy(con, runRenewalSelectionByPolicyRequest)

                ' Move AddQuote response to AddPolicy response
                With oResponse
                    .InsuranceFileKey = runRenewalSelectionByPolicyResponse.RenewalInsuranceFileKey
                    .InsuranceFolderKey = AddPolicyRequest.InsuranceFolderKey
                    .QuoteRef = runRenewalSelectionByPolicyResponse.RenewalInsuranceQuoteRef
                    .STSError = runRenewalSelectionByPolicyResponse.STSError
                    .QuoteTimeStamp = runRenewalSelectionByPolicyResponse.QuoteTimeStamp
                End With
            ElseIf AddPolicyRequest.RenewalStatusCode <> String.Empty Then
                policyRenewalInsuranceFileCnt = insuranceFileCnt
                UpdateInsuranceFileSystem(con, _SiriusUser.UserID, policyRenewalInsuranceFileCnt, "REN", True)

                Using Cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Renewal_Status_Invite_Printed_upd")
                    Cmd.AddInParameter("@renewal_insurance_file_cnt", SqlDbType.Int).Value = policyRenewalInsuranceFileCnt
                    con.ExecuteNonQuery(Cmd)
                End Using

                With oResponse
                    .InsuranceFileKey = policyRenewalInsuranceFileCnt
                    .InsuranceFolderKey = AddPolicyRequest.InsuranceFolderKey
                End With

            Else

                With oResponse
                    .InsuranceFileKey = policyRenewalInsuranceFileCnt
                    .InsuranceFolderKey = AddPolicyRequest.InsuranceFolderKey
                End With

            End If

        End If

    End Sub

    Private Shared Sub UpdateRiskTaxAmount(ByVal con As SiriusConnection, ByVal TransTypeCode As String, ByVal RiskKey As Integer, ByVal TaxItem As BaseTaxesType)
        Dim coreBusiness As New CoreBusiness
        Dim errorCollection As New SAMErrorCollection

        Try
            TaxItem.TaxBandID = coreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "tax_band", TaxItem.TaxBandCode)
        Catch ex As Exception
            errorCollection.AddInvalidData(SAMConstants.SAMInvalidData.InvalidLookupListValue,
                                            SAMConstants.SAMInvalidData.InvalidLookupListValue.ToString,
                                            "Policy.Risk.Taxes.TaxBandCode",
                                            TaxItem.TaxBandCode)
        End Try
        errorCollection.CheckForErrors()

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Risk_Tax_select")
            cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = RiskKey
            cmd.AddInParameter("@Mode", SqlDbType.Int).Value = 0
            cmd.AddInParameter("@TransType", SqlDbType.VarChar, 4).Value = TransTypeCode
            cmd.AddInParameter("@RecalculateTaxes", SqlDbType.Int).Value = 0

            dt = con.ExecuteDataTable(cmd)

        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            For Each dr As DataRow In dt.Rows

                Dim taxBandID As Integer = Cast.ToInt32(dr.Item("tax_band_id"), 0)

                If taxBandID = TaxItem.TaxBandID Then

                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Risk_Tax_Upd")

                        cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = RiskKey
                        cmd.AddInParameter("@tax_band_id", SqlDbType.Int).Value = taxBandID
                        cmd.AddInParameter("@premium", SqlDbType.Money).Value = dr.Item("premium")
                        cmd.AddInParameter("@percentage", SqlDbType.Float).Value = dr.Item("percentage")
                        cmd.AddInParameter("@value", SqlDbType.Money).Value = TaxItem.Amount
                        cmd.AddInParameter("@is_value", SqlDbType.TinyInt).Value = 1
                        cmd.AddInParameter("@is_manually_changed", SqlDbType.TinyInt).Value = 1
                        cmd.AddInParameter("@basis_value", SqlDbType.Money).Value = dr.Item("basis_value")
                        cmd.AddInParameter("@calc_basis", SqlDbType.Int).Value = dr.Item("calc_basis")
                        cmd.AddInParameter("@sum_insured", SqlDbType.Money).Value = dr.Item("sum_insured")
                        cmd.AddInParameter("@sum_insured_rounded", SqlDbType.TinyInt).Value = dr.Item("sum_insured_rounded")
                        cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = dr.Item("currency_id")
                        cmd.AddInParameter("@allow_tax_credit", SqlDbType.TinyInt).Value = dr.Item("allow_tax_credit")
                        cmd.AddInParameter("@original_sum_insured", SqlDbType.Money).Value = dr.Item("original_sum_insured")
                        cmd.AddInParameter("@country_id", SqlDbType.Int).Value = dr.Item("country_id")
                        cmd.AddInParameter("@state_id", SqlDbType.Int).Value = dr.Item("state_id")
                        cmd.AddInParameter("@class_of_business_id", SqlDbType.Int).Value = dr.Item("class_of_business_id")
                        cmd.AddInParameter("@tax_group_id", SqlDbType.Int).Value = dr.Item("tax_group_id")
                        cmd.AddInParameter("@sequence", SqlDbType.TinyInt).Value = dr.Item("sequence")
                        cmd.AddInParameter("@is_deleted", SqlDbType.TinyInt).Value = 0
                        cmd.AddInParameter("@tax_calculation_cnt", SqlDbType.Int).Value = dr.Item("tax_calculation_cnt")
                        cmd.AddInParameter("@apply_tax_by", SqlDbType.Int).Value = 0

                        dt = con.ExecuteDataTable(cmd)
                    End Using

                    Exit For
                End If

            Next dr

        End If
    End Sub

    Private Sub AddPolicyV2ProcessRisks(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType, ByVal oResponse As BaseImplementationTypes.BaseNBQuoteResponseTypePolicy)

        Dim oQuoteRiskResponse As BaseRiskResultType
        Dim iCnt As Integer
        Dim iLBnd As Integer
        Dim iUBnd As Integer
        Dim deleteRisk As Boolean = False
        Dim dtDeleted As New DataTable

        Dim coreBusiness As New CoreBusiness
        Dim deleteRiskRequest As New DeleteRiskIn
        Dim deleteRiskResponse As DeleteRiskOut

        Dim oAddRiskRequest As BaseAddRiskRequestType
        Dim oAddRiskResponse As BaseAddRiskResponseType
        ' upadate coinsurer
        If AddPolicyRequest.IsBDXRequest AndAlso AddPolicyRequest.CoInsurers IsNot Nothing Then
            Dim UpdateCoinsuranceValuesRequest As New BaseUpdateCoinsuranceValuesRequestType
            Dim oBaseCoinsurerResponse As BaseUpdateCoinsuranceValuesResponseType
            UpdateCoinsuranceValuesRequest.InsuranceFileKey = oResponse.InsuranceFileKey
            UpdateCoinsuranceValuesRequest.CoInsurers = AddPolicyRequest.CoInsurers
            UpdateCoinsuranceValuesRequest.BranchCode = AddPolicyRequest.BranchCode
            UpdateCoinsuranceValuesRequest.SourceId = AddPolicyRequest.SourceId
            UpdateCoinsuranceValuesRequest.TimeStamp = oResponse.QuoteTimeStamp
            UpdateCoinsuranceValuesRequest.IsSurcharged = True
            UpdateCoinsuranceValuesRequest.IsRecovered = True
            UpdateCoinsuranceValuesRequest.DefaultId = 1
            oBaseCoinsurerResponse = UpdateCoinsuranceValues(con, UpdateCoinsuranceValuesRequest)
            oResponse.QuoteTimeStamp = oBaseCoinsurerResponse.TimeStamp
            SAMErrorCollection.CheckForErrorsFromSTS(oBaseCoinsurerResponse.STSError)
        End If

        ' Process the Risks
        If AddPolicyRequest.Risks IsNot Nothing Then

            iLBnd% = AddPolicyRequest.Risks.GetLowerBound(0)
            iUBnd% = AddPolicyRequest.Risks.GetUpperBound(0)

            ReDim oResponse.Risks(iUBnd%)

            Dim quoteTimeStamp() As Byte = oResponse.QuoteTimeStamp

            ' If marked as cancellation then check the number of risks and if there are more 
            ' live risks on the policy then treat as an MTA and the DELETE each risk in this request.
            If AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.CancelPolicy Then

                ' Get the number of Active risks on the policy
                Dim dt As DataTable = Nothing
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_risks_for_policy_saa")
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oResponse.InsuranceFileKey
                    cmd.AddInParameter("@risk_status", SqlDbType.Char).Value = "A"
                    dt = con.ExecuteDataTable(cmd)
                End Using

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Dim dr As DataRow = dt.Rows(0)
                    ' If there are more risks on the policy then treat this as a delete risk.
                    If (dt.Rows.Count) > (iUBnd% + 1) Then
                        deleteRisk = True
                        AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.MTA
                    End If
                Else
                    ' The policy has been identified as a Cancellation but no live Risks can be found
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyMismatch, "The policy has been identified as a Cancellation but no live Risks can be found", "The policy certificate - '" & oResponse.QuoteRef & " has been identified as a Cancellation but no live Risks can be found")
                    STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "Policy Process Validation", True)
                    Exit Sub
                End If
            ElseIf AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.MTA _
                  OrElse AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.Renewals _
                  OrElse AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.NewBusiness _
                  OrElse AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.ReinstatePolicy Then

                ' Get the number of Active risks on the policy
                Dim dt As DataTable = Nothing
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_risks_for_policy_saa")
                    cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oResponse.InsuranceFileKey
                    cmd.AddInParameter("@risk_status", SqlDbType.Char).Value = "A"
                    dtDeleted = con.ExecuteDataTable(cmd)
                End Using

                If dtDeleted IsNot Nothing AndAlso dtDeleted.Rows.Count > 0 Then

                    dtDeleted.Columns.Add("IsActive", Type.GetType("System.Int32"))
                    dtDeleted.Columns("IsActive").DefaultValue = 0
                End If
            End If

            For iCnt% = iLBnd% To iUBnd%

                Dim xmlDataset As String = String.Empty

                Dim riskKey As Integer = 0
                Dim riskFolderKey As Integer = AddPolicyRequest.Risks(iCnt%).RiskFolderKey
                Dim OriginalRiskCnt As Integer = AddPolicyRequest.Risks(iCnt%).OriginalRiskKey
                Dim matchingRiskFound As Boolean = False

                If AddPolicyRequest.IsMarketPlacePolicy Then
                    TransformRequestListValues(AddPolicyRequest.Risks(iCnt%).XMLDataSet, AddPolicyRequest.Risks(iCnt%).DataModelCode)
                    TransformRequestUDLValues(AddPolicyRequest.Risks(iCnt%).XMLDataSet, AddPolicyRequest.Risks(iCnt%).DataModelCode)
                    MakeXMLForAddRisk(AddPolicyRequest.Risks(iCnt%).XMLDataSet)
                End If
                If AddPolicyRequest.IsBDXRequest Then
                    If riskFolderKey <> 0 Then

                        ' Try and identify the matching risk by the Risk Folder passed in.
                        Dim dt As DataTable = Nothing
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_risk_cnt_by_folder")
                            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oResponse.InsuranceFileKey
                            cmd.AddInParameter("@risk_folder_cnt", SqlDbType.Int).Value = AddPolicyRequest.Risks(iCnt%).RiskFolderKey
                            cmd.AddInParameter("@nOriginalrisk_cnt", SqlDbType.Int).Value = AddPolicyRequest.Risks(iCnt%).OriginalRiskKey
                            cmd.AddInParameter("@sTransactiontype", SqlDbType.VarChar).Value = AddPolicyRequest.TransactionTypeCode
                            dt = con.ExecuteDataTable(cmd)
                        End Using

                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                            Dim dr As DataRow = dt.Rows(0)

                            riskKey = Cast.ToInt32(dr.Item("risk_cnt"), 0)

                            Dim getRiskRequest As New BaseGetRiskRequestType
                            Dim getRiskResponse As BaseGetRiskResponseType

                            getRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                            getRiskRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                            getRiskRequest.InsuranceFolderKey = oResponse.InsuranceFolderKey
                            getRiskRequest.RiskKey = riskKey
                            If quoteTimeStamp Is Nothing Then
                                coreBusiness = New CoreBusiness
                                Dim currentlyLocked As Boolean = False
                                ' Check that the timestamp matches and put lock on the given insurance folder key
                                coreBusiness.GetSAMTimestamp(Con:=con,
                                                                BranchCode:=getRiskRequest.BranchCode,
                                                                Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                                                                LockValue:=getRiskRequest.InsuranceFolderKey,
                                                                TStamp:=getRiskRequest.QuoteTimeStamp,
                                                                bCurrentlyLocked:=currentlyLocked)

                            Else
                                getRiskRequest.QuoteTimeStamp = quoteTimeStamp
                            End If

                            getRiskResponse = RetrieveRisk(con, getRiskRequest)

                            xmlDataset = getRiskResponse.XMLDataSet
                            quoteTimeStamp = getRiskResponse.QuoteTimeStamp
                            matchingRiskFound = True
                            'here marking the risk to be deleted which are not passed in BDX excel file.
                            If AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.NewBusiness Then
                                'Passing Risk_cnt here for comparision because quote version don't have original risk_cnt
                                UpdateActiveRisk(dtDeleted, "risk_cnt", AddPolicyRequest.Risks(iCnt%).OriginalRiskKey)
                            Else
                                UpdateActiveRisk(dtDeleted, "risk_cnt", riskKey)
                            End If
                        End If
                    End If

                Else
                    If riskFolderKey <> 0 Then

                        ' Try and identify the matching risk by the Risk Folder passed in.
                        Dim dt As DataTable = Nothing
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_get_risk_cnt_by_folder")
                            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = oResponse.InsuranceFileKey
                            cmd.AddInParameter("@risk_folder_cnt", SqlDbType.Int).Value = AddPolicyRequest.Risks(iCnt%).RiskFolderKey
                            dt = con.ExecuteDataTable(cmd)
                        End Using

                        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                            Dim dr As DataRow = dt.Rows(0)

                            riskKey = Cast.ToInt32(dr.Item("risk_cnt"), 0)
                            If Not AddPolicyRequest.IsMarketPlacePolicy Then
                                Dim getRiskRequest As New BaseGetRiskRequestType
                                Dim getRiskResponse As BaseGetRiskResponseType

                                getRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                                getRiskRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                                getRiskRequest.InsuranceFolderKey = oResponse.InsuranceFolderKey
                                getRiskRequest.RiskKey = riskKey
                                If quoteTimeStamp Is Nothing Then
                                    Dim currentlyLocked As Boolean = False
                                    ' Check that the timestamp matches and put lock on the given insurance folder key
                                    coreBusiness.GetSAMTimestamp(Con:=con,
                                                                    BranchCode:=getRiskRequest.BranchCode,
                                                                    Lockname:=CoreBusiness.LockName.InsuranceFolderCnt,
                                                                    LockValue:=getRiskRequest.InsuranceFolderKey,
                                                                    TStamp:=getRiskRequest.QuoteTimeStamp,
                                                                    bCurrentlyLocked:=currentlyLocked)

                                Else
                                    getRiskRequest.QuoteTimeStamp = quoteTimeStamp
                                End If

                                getRiskResponse = RetrieveRisk(con, getRiskRequest)

                                xmlDataset = getRiskResponse.XMLDataSet
                                quoteTimeStamp = getRiskResponse.QuoteTimeStamp
                                matchingRiskFound = True
                            Else
                                deleteRiskRequest.InsuranceFileCnt = oResponse.InsuranceFileKey
                                deleteRiskRequest.InsuranceFolderCnt = oResponse.InsuranceFolderKey
                                deleteRiskRequest.RiskCnt = riskKey
                                deleteRiskRequest.TransactionType = AddPolicyRequest.TransactionTypeCode

                                deleteRiskResponse = coreBusiness.DeleteRisk(deleteRiskRequest)
                                matchingRiskFound = False
                            End If
                        End If

                    End If
                End If
                ' If no match was found then add the new risk
                If matchingRiskFound = False Then

                    ' Add the risk
                    oAddRiskRequest = New BaseImplementationTypes.BaseAddRiskRequestType
                    oAddRiskRequest.DataModelCode = AddPolicyRequest.Risks(iCnt%).DataModelCode
                    'Pass the updated timestamp
                    oAddRiskRequest.QuoteTimeStamp = quoteTimeStamp 'AddPolicyRequest.Risks(iCnt%).QuoteTimeStamp
                    oAddRiskRequest.RiskDescription = AddPolicyRequest.Risks(iCnt%).RiskDescription
                    oAddRiskRequest.RiskTypeCode = AddPolicyRequest.Risks(iCnt%).RiskTypeCode
                    oAddRiskRequest.RunDefaultRules = AddPolicyRequest.Risks(iCnt%).RunDefaultRules
                    oAddRiskRequest.ScreenCode = AddPolicyRequest.Risks(iCnt%).ScreenCode
                    oAddRiskRequest.XMLDataSet = AddPolicyRequest.Risks(iCnt%).XMLDataSet
                    oAddRiskRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                    oAddRiskRequest.InsuranceFolderKey = oResponse.InsuranceFolderKey
                    oAddRiskRequest.ProductCode = AddPolicyRequest.ProductCode
                    oAddRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                    oAddRiskRequest.RatingSections = AddPolicyRequest.Risks(iCnt%).RatingSections
                    oAddRiskRequest.RIArrangement = AddPolicyRequest.Risks(iCnt%).RIArrangement
                    oAddRiskRequest.IsMarketplacePolicy = AddPolicyRequest.IsMarketPlacePolicy
                    oAddRiskResponse = AddRisk(con, oAddRiskRequest)

                    xmlDataset = oAddRiskResponse.XMLDataSet
                    quoteTimeStamp = oAddRiskResponse.QuoteTimeStamp
                    riskKey = oAddRiskResponse.RiskKey
                    riskFolderKey = oAddRiskResponse.RiskFolderKey

                End If

                If AddPolicyRequest.Risks(iCnt%).ProductBuilderDetail IsNot Nothing Then
                    ProcessPBRiskData(xmlDataset, AddPolicyRequest.Risks(iCnt%).ProductBuilderDetail)
                End If

                Dim updateRiskRequest As New BaseUpdateRiskRequestType
                Dim updateRiskResponse As New BaseUpdateRiskResponseType

                updateRiskRequest.BranchCode = AddPolicyRequest.BranchCode
                updateRiskRequest.QuoteTimeStamp = quoteTimeStamp
                updateRiskRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                updateRiskRequest.InsuranceFolderKey = oResponse.InsuranceFolderKey
                updateRiskRequest.RiskDescription = AddPolicyRequest.Risks(iCnt%).RiskDescription
                updateRiskRequest.RiskKey = riskKey
                updateRiskRequest.ScreenCode = AddPolicyRequest.Risks(iCnt%).ScreenCode
                updateRiskRequest.XMLDataSet = xmlDataset
                updateRiskRequest.TransactionType = AddPolicyRequest.TransactionTypeCode
                updateRiskRequest.SourceId = AddPolicyRequest.SourceId
                updateRiskRequest.SubBranchCode = AddPolicyRequest.BranchCode

                updateRiskResponse = UpdateRisk(con, updateRiskRequest)

                ' Convert the risk response to the baserisktype required on the quote response
                oQuoteRiskResponse = New BaseImplementationTypes.BaseRiskResultType
                oQuoteRiskResponse.RiskFolderID = riskFolderKey
                oQuoteRiskResponse.RiskID = riskKey
                oQuoteRiskResponse.STSError = updateRiskResponse.STSError
                oQuoteRiskResponse.XMLDataSet = updateRiskResponse.XMLDataSet

                quoteTimeStamp = updateRiskResponse.QuoteTimeStamp

                oQuoteRiskResponse.SAMStagingRiskKey = AddPolicyRequest.Risks(iCnt%).SAMStagingRiskKey

                AddPolicyRequest.Risks(iCnt%).RiskKey = riskKey

                oResponse.Risks(iCnt%) = oQuoteRiskResponse

                ' If this has been identified as a deletion then call the delete risk method
                If deleteRisk = True Then
                    deleteRiskRequest = New DeleteRiskIn
                    deleteRiskResponse = New DeleteRiskOut

                    deleteRiskRequest.InsuranceFileCnt = oResponse.InsuranceFileKey
                    deleteRiskRequest.InsuranceFolderCnt = oResponse.InsuranceFolderKey
                    deleteRiskRequest.RiskCnt = riskKey
                    deleteRiskRequest.TransactionType = AddPolicyRequest.TransactionTypeCode

                    deleteRiskResponse = coreBusiness.DeleteRisk(deleteRiskRequest)

                End If

            Next iCnt%
            'Clear the deleted RISK FROM MTA,RENEWAL
            If AddPolicyRequest.IsBDXRequest AndAlso Not dtDeleted Is Nothing Then

                For Each dtrow As DataRow In dtDeleted.Rows

                    If IsDBNull(dtrow("IsActive")) OrElse Val(dtrow("IsActive")) = 0 Then
                        coreBusiness = New CoreBusiness
                        deleteRiskRequest = New DeleteRiskIn
                        deleteRiskResponse = New DeleteRiskOut

                        deleteRiskRequest.InsuranceFileCnt = oResponse.InsuranceFileKey
                        deleteRiskRequest.InsuranceFolderCnt = oResponse.InsuranceFolderKey
                        deleteRiskRequest.RiskCnt = dtrow("risk_cnt")
                        deleteRiskRequest.TransactionType = AddPolicyRequest.TransactionTypeCode

                        deleteRiskResponse = coreBusiness.DeleteRisk(deleteRiskRequest)
                    End If

                Next
            End If


            oResponse.QuoteTimeStamp = quoteTimeStamp

        End If

    End Sub
    ''' <summary>
    ''' Update the flag which are processed through BDX excel file.
    ''' </summary>
    ''' <param name="dtActiveRisk"></param>
    ''' <param name="scolumnName"></param>
    ''' <param name="nSearchValue"></param>
    ''' <returns></returns>
    Private Function UpdateActiveRisk(ByRef dtActiveRisk As DataTable,
                                      scolumnName As String,
                                      nSearchValue As Integer) As Boolean
        Dim iCount As Integer = 0
        For Each dtrow As DataRow In dtActiveRisk.Rows
            If IsDBNull(dtrow(scolumnName)) = False AndAlso Val(dtrow(scolumnName)) = nSearchValue Then
                dtActiveRisk.Rows(iCount)("IsActive") = 1
                Exit For
            End If
            iCount += 1
        Next

        Return False
    End Function
    Public Overloads Function AddPolicyV2(ByVal AddPolicyRequest As BaseQuoteRiskMsgType) As BaseNBQuoteResponseTypePolicy
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseNBQuoteResponseTypePolicy

            oResponse = AddPolicyV2(con, AddPolicyRequest)

            Return oResponse

        End Using
    End Function

    Public Overloads Function AddPolicyV2(ByVal con As SiriusConnection, ByVal AddPolicyRequest As BaseQuoteRiskMsgType) As BaseNBQuoteResponseTypePolicy
        Dim oResponse As New BaseImplementationTypes.BaseNBQuoteResponseTypePolicy
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        'Const ACMethodName As String = "AddPolicyV2"
        Dim Utils As New Utilities

        Dim STSError As New STSErrorPublisher
        Dim dOldTotalPremium As Double = 0.0
        ' Check if the policy record already exists
        Dim getPolicyRequest As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefRequestType
        Dim getPolicyReponse As New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefResponseType
        Dim noTimeStamp() As Byte

        If AddPolicyRequest.QuoteRef <> String.Empty Then

            getPolicyRequest.BranchCode = AddPolicyRequest.BranchCode
            getPolicyRequest.InsuranceRef = AddPolicyRequest.QuoteRef
            getPolicyRequest.SkipPolicyLevelTaxesRecalculation = True
            getPolicyReponse = DirectCast(GetHeaderAndSummariesByRef(getPolicyRequest), SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesByRefResponseType)

            ' If the policy exists then check that it belongs to the correct Party
            If getPolicyReponse.InsuranceFileKey <> 0 Then
                dOldTotalPremium = GetTotalPremiumAmount(con, getPolicyReponse.InsuranceFileKey)
                AddPolicyRequest.RenewalStatusCode = getPolicyReponse.RenewalStatusTypeCode
                If AddPolicyRequest.IsMarketPlacePolicy Then
                    If getPolicyReponse.ResultDataset IsNot Nothing AndAlso getPolicyReponse.ResultDataset.SelectSingleNode("//Row/Col1").InnerXml IsNot Nothing AndAlso Not String.IsNullOrEmpty(getPolicyReponse.ResultDataset.SelectSingleNode("//Row/Col1").InnerXml) Then
                        'Till now we have requirement for single risk only so fetching first row riskfolderkey
                        AddPolicyRequest.Risks(0).RiskFolderKey = CInt(getPolicyReponse.ResultDataset.SelectSingleNode("//Row[" & getPolicyReponse.ResultDataset.ChildNodes.Count & "]/Col1").InnerXml)
                        AddPolicyRequest.Risks(0).RiskFolderKeySpecified = True
                    End If
                End If
                If (getPolicyReponse.PartyKey <> AddPolicyRequest.PartyKey) And (AddPolicyRequest.TransactionTypeCode <> "MTA PERM") Then
                    ' The policy Number exists on the system but it belongs to another client
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyMismatch, "The Policy record already exists", "The policy certificate - '" & getPolicyRequest.InsuranceRef & " already exists on the system but belongs to the Party record - '" & AddPolicyRequest.InsuredName & "'")
                    STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "Policy Process Validation", True)
                    Return oResponse
                ElseIf (AddPolicyRequest.TransactionTypeCode = "POLICY") And (AddPolicyRequest.PolicyStatusCode = "POLICY") And (getPolicyReponse.InsuranceFileTypeCode <> "QUOTE") Then
                    ' The policy number exists on the system against this client but a new business record was submitted
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyMismatch, "The Policy record already exists", "The policy certificate - '" & getPolicyRequest.InsuranceRef & " already exists on the system and a duplicate new business request has been made")
                    STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "Policy Process Validation", True)
                    Return oResponse
                ElseIf (getPolicyReponse.InsuranceFileTypeCode = "QUOTE") Then
                    AddPolicyRequest.PolicyStatusCode = "QUOTE"
                    oResponse.InsuranceFileKey = getPolicyReponse.InsuranceFileKey
                    oResponse.InsuranceFolderKey = getPolicyReponse.InsuranceFolderKey
                    oResponse.QuoteTimeStamp = getPolicyReponse.QuoteTimeStamp
                End If
                AddPolicyRequest.InsuranceFolderKey = getPolicyReponse.InsuranceFolderKey


            ElseIf AddPolicyRequest.TransactionTypeCode = "MTA PERM" Then
                ' The policy number doesn't exist on the system but an MTA was requested
                Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyMismatch, "The Policy record does not exist", "The policy certificate - '" & getPolicyRequest.InsuranceRef & " does not exist on the system but an MTA was requested")
                STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "Policy Process Validation", True)
                Return oResponse
            End If
        End If

        Select Case AddPolicyRequest.TransactionTypeCode
            Case "POLICY"
                ' If the policy status indicates a renewal but we have to existing record then treat this as new business
                If AddPolicyRequest.PolicyStatusCode = "RENEWAL" And AddPolicyRequest.InsuranceFolderKey = 0 Then
                    AddPolicyRequest.PolicyStatusCode = "POLICY"
                    AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.NewBusiness
                ElseIf AddPolicyRequest.PolicyStatusCode = "POLICY" Or AddPolicyRequest.PolicyStatusCode = "QUOTE" Then
                    AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.NewBusiness
                Else
                    AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.Renewals
                End If
            Case "MTA PERM"
                AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.MTA
                oResponse.InsuranceFileKey = getPolicyReponse.InsuranceFileKey
            Case "MTACAN"
                AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.CancelPolicy
                oResponse.InsuranceFileKey = getPolicyReponse.InsuranceFileKey
            Case "RENEWAL"
                ' If the policy status indicates a renewal but we have to existing record then treat this as new business
                If AddPolicyRequest.InsuranceFolderKey = 0 Then
                    AddPolicyRequest.PolicyStatusCode = "POLICY"
                    AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.NewBusiness
                Else
                    AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.Renewals
                End If
            Case "MTR"
                AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.ReinstatePolicy
                oResponse.InsuranceFileKey = getPolicyReponse.InsuranceFileKey
        End Select

        If AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.NewBusiness And (AddPolicyRequest.PolicyStatusCode = "POLICY" Or AddPolicyRequest.PolicyStatusCode = "QUOTE") Then

            If oResponse.InsuranceFileKey = 0 Then
                AddPolicyV2AddQuote(con, AddPolicyRequest, oResponse)
                If oResponse.STSError IsNot Nothing Then
                    Return oResponse
                End If
            Else
                If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                    'don't need to add or edit risk also no need to re rate the policy
                Else
                    AddPolicyV2UpdateQuote(con, AddPolicyRequest, oResponse)
                    If oResponse.STSError IsNot Nothing Then
                        Return oResponse
                    End If
                End If
            End If
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                If oResponse.InsuranceFileKey <> 0 Then
                    AddPolicyV2ProcessRisks(con, AddPolicyRequest, oResponse)
                    If oResponse.STSError IsNot Nothing Then
                        Return oResponse
                    ElseIf oResponse.Risks IsNot Nothing Then
                        For Each riskResponse As BaseRiskResultType In oResponse.Risks
                            If riskResponse.STSError IsNot Nothing Then
                                oResponse.STSError = riskResponse.STSError
                                Return oResponse
                            End If
                        Next
                    End If
                End If
            End If
        ElseIf (AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.MTA Or AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.ReinstatePolicy) Then
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                AddPolicyV2AddMTAQuote(con, AddPolicyRequest, oResponse)
                If oResponse.STSError IsNot Nothing Then
                    Return oResponse
                End If
            End If
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                If oResponse.InsuranceFileKey <> 0 Then
                    AddPolicyV2ProcessRisks(con, AddPolicyRequest, oResponse)
                    If oResponse.STSError IsNot Nothing Then
                        Return oResponse
                    ElseIf oResponse.Risks IsNot Nothing Then
                        For Each riskResponse As BaseRiskResultType In oResponse.Risks
                            If riskResponse.STSError IsNot Nothing Then
                                oResponse.STSError = riskResponse.STSError
                                Return oResponse
                            End If
                        Next
                    End If
                End If
            End If
        ElseIf AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.CancelPolicy Then
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                AddPolicyV2AddMTAQuote(con, AddPolicyRequest, oResponse)
                If oResponse.STSError IsNot Nothing Then
                    Return oResponse
                End If
            End If
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                If oResponse.InsuranceFileKey <> 0 And AddPolicyRequest.IsBDXRequest = False Then
                    AddPolicyV2ProcessRisks(con, AddPolicyRequest, oResponse)
                    If oResponse.STSError IsNot Nothing Then
                        Return oResponse
                    ElseIf oResponse.Risks IsNot Nothing Then
                        For Each riskResponse As BaseRiskResultType In oResponse.Risks
                            If riskResponse.STSError IsNot Nothing Then
                                oResponse.STSError = riskResponse.STSError
                                Return oResponse
                            End If
                        Next
                    End If
                End If
            End If
        ElseIf AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.Renewals And AddPolicyRequest.PolicyStatusCode = "RENEWAL" Then

            'Process Renewal
            oResponse.QuoteTimeStamp = getPolicyReponse.QuoteTimeStamp
            AddPolicyV2AddRenewal(con, AddPolicyRequest, oResponse)
            If oResponse.STSError IsNot Nothing Then
                Return oResponse
            End If
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                If oResponse.InsuranceFileKey <> 0 Then
                    AddPolicyRequest.PolicyStatusCode = "REN"
                    AddPolicyV2UpdateQuote(con, AddPolicyRequest, oResponse)
                    AddPolicyV2ProcessRisks(con, AddPolicyRequest, oResponse)
                    If oResponse.STSError IsNot Nothing Then
                        Return oResponse
                    ElseIf oResponse.Risks IsNot Nothing Then
                        For Each riskResponse As BaseRiskResultType In oResponse.Risks
                            If riskResponse.STSError IsNot Nothing Then
                                oResponse.STSError = riskResponse.STSError
                                Return oResponse
                            End If
                        Next
                    End If
                End If
            End If
        End If

        If oResponse IsNot Nothing Then
            noTimeStamp = oResponse.QuoteTimeStamp
        End If
        If oResponse.InsuranceFileKey <> 0 Then
            If AddPolicyRequest.IsMarketPlacePolicy And AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                'don't need to add or edit risk also no need to re rate the policy
            Else
                ' upadate coinsurer
                If AddPolicyRequest.IsBDXRequest AndAlso AddPolicyRequest.CoInsurers IsNot Nothing Then
                    Dim UpdateCoinsuranceValuesRequest As New BaseUpdateCoinsuranceValuesRequestType
                    Dim oBaseCoinsurerResponse As BaseUpdateCoinsuranceValuesResponseType
                    UpdateCoinsuranceValuesRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                    UpdateCoinsuranceValuesRequest.CoInsurers = AddPolicyRequest.CoInsurers
                    UpdateCoinsuranceValuesRequest.BranchCode = AddPolicyRequest.BranchCode
                    UpdateCoinsuranceValuesRequest.SourceId = AddPolicyRequest.SourceId
                    UpdateCoinsuranceValuesRequest.TimeStamp = oResponse.QuoteTimeStamp
                    UpdateCoinsuranceValuesRequest.IsSurcharged = True
                    UpdateCoinsuranceValuesRequest.IsRecovered = True
                    UpdateCoinsuranceValuesRequest.DefaultId = 1
                    oBaseCoinsurerResponse = UpdateCoinsuranceValues(con, UpdateCoinsuranceValuesRequest)
                    SAMErrorCollection.CheckForErrorsFromSTS(oBaseCoinsurerResponse.STSError)

                    If oBaseCoinsurerResponse IsNot Nothing Then
                        noTimeStamp = oBaseCoinsurerResponse.TimeStamp

                    End If
                End If
                If UpdateOverridedTaxes(con, oResponse.InsuranceFileKey, AddPolicyRequest) <> 1 Then
                    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyMismatch, "The Policy record could not be created nor identified.", "The policy certificate - '" & getPolicyRequest.InsuranceRef & " Could not be created nor identified.")
                    STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "Policy Process Validation", True)
                    Return oResponse
                End If

                If ToSafeString(AddPolicyRequest.BusinessTypeCode, "") = "AGENCY" OrElse
                 ToSafeString(AddPolicyRequest.BusinessTypeCode, "") = "COIN FOLL" OrElse
                 ToSafeString(AddPolicyRequest.BusinessTypeCode, "") = "COIN LEAD" Then
                    If AddPolicyRequest.CommissionRate > 0 Or AddPolicyRequest.CommissionValue <> 0 Then
                        Dim oAgentCommissionRequest As New BaseGetAgentCommissionRequestType
                        Dim oAgentCommissionResponse As New BaseGetAgentCommissionResponseType

                        Dim oUpdateAgentCommissionRequest As New BaseUpdateAgentCommissionRequestType
                        Dim oUpdateAgentCommissionResponse As New BaseUpdateAgentCommissionResponseType
                        Dim oUpdateCommission As New BaseUpdateAgentCommissionRequestTypeAgentCommission
                        Dim irow As Integer = 0
                        oAgentCommissionRequest.BranchCode = AddPolicyRequest.BranchCode
                        oAgentCommissionRequest.InsuranceFileKey = oResponse.InsuranceFileKey

                        oAgentCommissionResponse = GetAgentCommission(con, oAgentCommissionRequest)
                        If oAgentCommissionResponse IsNot Nothing AndAlso oAgentCommissionResponse.AgentCommission IsNot Nothing AndAlso oAgentCommissionResponse.AgentCommission.Row IsNot Nothing Then
                            oUpdateAgentCommissionRequest.BranchCode = AddPolicyRequest.BranchCode
                            oUpdateAgentCommissionRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                            ReDim oUpdateCommission.Row(oAgentCommissionResponse.AgentCommission.Row.Length - 1)
                            For Each oCommission As BaseAgentCommissionResponseTypeAgentCommissionRow In oAgentCommissionResponse.AgentCommission.Row
                                oUpdateCommission.Row(irow) = New BaseUpdateAgentCommissionRequestTypeAgentCommissionRow
                                oUpdateCommission.Row(irow).Agent = oCommission.Agent
                                oUpdateCommission.Row(irow).AgentType = oCommission.AgentType
                                oUpdateCommission.Row(irow).CommissionBand = oCommission.CommissionBand
                                oUpdateCommission.Row(irow).CommissionRate = oCommission.CommissionRate
                                oUpdateCommission.Row(irow).CommissionValue = oCommission.CommissionValue

                                oUpdateCommission.Row(irow).IsLeadAgent = oCommission.IsLeadAgent
                                oUpdateCommission.Row(irow).IsValue = oCommission.IsValue
                                oUpdateCommission.Row(irow).MaximumRate = oCommission.MaximumRate
                                oUpdateCommission.Row(irow).Premium = oCommission.Premium
                                oUpdateCommission.Row(irow).RiskType = oCommission.RiskType
                                oUpdateCommission.Row(irow).TaxGroupCode = oCommission.TaxGroup
                                irow = irow + 1
                            Next

                            irow = 0

                            If AddPolicyRequest.CommissionRate > 0 Then
                                oUpdateCommission.Row(0).CommissionRate = AddPolicyRequest.CommissionRate
                                oUpdateCommission.Row(0).CommissionValue = oUpdateCommission.Row(0).Premium * AddPolicyRequest.CommissionRate / 100
                                oUpdateCommission.Row(0).IsAmended = True
                            Else
                                oUpdateCommission.Row(0).CommissionRate = AddPolicyRequest.CommissionValue
                                oUpdateCommission.Row(0).CommissionValue = AddPolicyRequest.CommissionValue
                                oUpdateCommission.Row(0).IsAmended = True
                                oUpdateCommission.Row(0).IsValue = True
                            End If


                            oUpdateAgentCommissionRequest.AgentCommission = oUpdateCommission

                            oUpdateAgentCommissionResponse = UpdateAgentCommission(con, oUpdateAgentCommissionRequest)
                        End If
                    End If

                    'Now requirement to override premium only for single risk single rating
                    If AddPolicyRequest.OverrideNetPremium > 0 Then
                        Dim oGetRatingDetailsRequest As New BaseImplementationTypes.BaseGetRatingDetailsRequestType
                        Dim oGetRatingDetailsResponse As BaseImplementationTypes.BaseGetRatingDetailsResponseType
                        With oGetRatingDetailsRequest
                            .BranchCode = AddPolicyRequest.BranchCode
                            .InsuranceFileKey = oResponse.InsuranceFileKey
                            .RiskKey = oResponse.Risks(0).RiskID
                        End With
                        oGetRatingDetailsResponse = GetRatingDetails(oGetRatingDetailsRequest)
                        If oGetRatingDetailsResponse IsNot Nothing AndAlso oGetRatingDetailsResponse.RatingDetails IsNot Nothing AndAlso oGetRatingDetailsResponse.RatingDetails.HasChildNodes Then
                            Dim oSFIRatingResponse As New SFI.SAMForInsuranceV2.WCF.BaseGetRatingDetailsResponseType
                            oSFIRatingResponse.RatingDetails = SAMFunc.GetDeserializedValues(Of List(Of SFI.SAMForInsuranceV2.WCF.BaseGetRatingDetailsResponseTypeRow))(elmResultDataSet:=oGetRatingDetailsResponse.RatingDetails, sFromTypeName:="BaseGetRatingDetailsResponseTypeRatingDetails", sConvertToTypeName:="BaseGetRatingDetailsResponseTypeRow")


                            Dim oUpdateRatingSectionRequest As New BaseUpdateRatingDetailsRequestType
                            Dim oUpdateRatingSectionResponse As New BaseUpdateRatingDetailsResponsetType
                            oUpdateRatingSectionRequest.BranchCode = AddPolicyRequest.BranchCode
                            oUpdateRatingSectionRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                            oUpdateRatingSectionRequest.RiskKey = oResponse.Risks(0).RiskID
                            oUpdateRatingSectionRequest.SourceId = AddPolicyRequest.SourceId
                            oUpdateRatingSectionRequest.TransactionType = AddPolicyRequest.TransactionTypeCode '"NB"
                            oUpdateRatingSectionRequest.TimeStamp = oResponse.QuoteTimeStamp
                            oUpdateRatingSectionRequest.RatingDetails = New BaseUpdateRatingDetailsRequestTypeRatingDetails
                            ReDim oUpdateRatingSectionRequest.RatingDetails.Row(0)
                            oUpdateRatingSectionRequest.RatingDetails.Row(0) = New BaseUpdateRatingDetailsRequestTypeRatingDetailsRow
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).AnnualPremium = oSFIRatingResponse.RatingDetails(0).AnnualPremium
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).AnnualRate = oSFIRatingResponse.RatingDetails(0).AnnualRate
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).CountryCode = oSFIRatingResponse.RatingDetails(0).CountryCode
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).CurrencyCode = oSFIRatingResponse.RatingDetails(0).CurrencyCode 'AddPolicyRequest.Risks(0).RatingSections(0).c
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).EarningPatternCode = oSFIRatingResponse.RatingDetails(0).EarningPatternCode
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).OriginalFlag = oSFIRatingResponse.RatingDetails(0).OriginalFlag
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).OverrideReason = oSFIRatingResponse.RatingDetails(0).OverrideReason
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).RateTypeCode = oSFIRatingResponse.RatingDetails(0).RatingTypeCode
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).RatingSectionTypeCode = oSFIRatingResponse.RatingDetails(0).RatingSectionTypeCode
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).StateCode = oSFIRatingResponse.RatingDetails(0).StateCode
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).SumInsured = oSFIRatingResponse.RatingDetails(0).SumInsured
                            oUpdateRatingSectionRequest.RatingDetails.Row(0).ThisPremium = AddPolicyRequest.OverrideNetPremium

                            oUpdateRatingSectionResponse = UpdateRatingDetails(oUpdateRatingSectionRequest)
                            If oUpdateRatingSectionResponse IsNot Nothing Then
                                noTimeStamp = oUpdateRatingSectionResponse.TimeStamp
                            End If
                        End If
                    End If
                End If
                ''Untested code left for future use (requirement only for single rating section)
                'For iRiskCount As Integer = 0 To AddPolicyRequest.Risks.GetUpperBound(0)
                '    If AddPolicyRequest.Risks(iRiskCount).RatingSections IsNot Nothing Then
                '        For iRatingCount As Integer = 0 To AddPolicyRequest.Risks(iRiskCount).RatingSections.GetUpperBound(0)
                '            If AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).IsPremiumOverride Then
                '                Dim oUpdateRatingSectionRequest As New BaseUpdateRatingDetailsRequestType
                '                Dim oUpdateRatingSectionResponse As New BaseUpdateRatingDetailsResponsetType
                '                oUpdateRatingSectionRequest.BranchCode = AddPolicyRequest.BranchCode
                '                oUpdateRatingSectionRequest.InsuranceFileKey = oResponse.InsuranceFileKey
                '                oUpdateRatingSectionRequest.RiskKey = AddPolicyRequest.Risks(iRiskCount).RiskKey
                '                oUpdateRatingSectionRequest.SourceId = AddPolicyRequest.SourceId
                '                oUpdateRatingSectionRequest.TransactionType = AddPolicyRequest.TransactionTypeCode '"NB"

                '                ReDim oUpdateRatingSectionRequest.RatingDetails.Row(0)
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).AnnualPremium = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).AnnualPremium
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).AnnualRate = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).AnnualRate
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).CountryCode = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).CountryCode
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).CurrencyCode = AddPolicyRequest.CurrencyCode 'AddPolicyRequest.Risks(0).RatingSections(0).c
                '                'oUpdateRatingSectionRequest.RatingDetails.Row(0).EarningPatternCode = AddPolicyRequest.Risks(0).RatingSections(0).
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).OriginalFlag = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).OriginalFlag
                '                'oUpdateRatingSectionRequest.RatingDetails.Row(0).OverrideReason = AddPolicyRequest.Risks(0).RatingSections(0).over
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).RateTypeCode = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).RateTypeCode
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).RatingSectionTypeCode = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).RatingSectionTypeCode
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).StateCode = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).StateCode
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).SumInsured = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).SumInsured
                '                oUpdateRatingSectionRequest.RatingDetails.Row(0).ThisPremium = AddPolicyRequest.Risks(iRiskCount).RatingSections(iRatingCount).OverridePremium

                '                oUpdateRatingSectionResponse = UpdateRatingDetails(oUpdateRatingSectionRequest)
                '            End If
                '        Next
                '    End If
                'Next

            End If

            If AddPolicyRequest.TransactionTypeCode <> TransactionTypeCode.Renewals AndAlso AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then

                Dim bindQuoteRequest As New BaseBindQuoteRequestType
                Dim bindQuoteResponse As BaseBindQuoteResponseType

                With bindQuoteRequest
                    .QuoteTimeStamp = noTimeStamp
                    .BranchCode = AddPolicyRequest.BranchCode
                    .InsuranceFileKey = oResponse.InsuranceFileKey
                    ' .QuoteTimeStamp = oResponse.QuoteTimeStamp
                    .TransactionType = AddPolicyRequest.TransactionTypeCode

                    If Not AddPolicyRequest.IsMarketPlacePolicy Then
                        .SkipNewPolicyNumber = oResponse.SkipNewPolicyNumber
                        .SkipPolicyLevelTaxesRecalculation = True
                    End If

                    .MarketPlaceTotalPremium = dOldTotalPremium
                    .IsMarketPlacePolicy = AddPolicyRequest.IsMarketPlacePolicy
                End With

                bindQuoteResponse = BindQuote(bindQuoteRequest)
                If bindQuoteResponse.STSError IsNot Nothing Then
                    oResponse.STSError = bindQuoteResponse.STSError
                    Return oResponse
                End If
                If AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.ReinstatePolicy Then
                    UpdateInsuranceFileDetails(con, bindQuoteRequest.InsuranceFileKey, "MTAREINS")
                End If

                If AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.MTA Then
                    Dim mtaReasonId As Integer
                    Dim sMTAReasonDescription As String = AddPolicyRequest.Description

                    mtaReasonId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.MTAEventDescription, AddPolicyRequest.MTAReasonCode, "code", , AddPolicyRequest.Description)

                    If String.IsNullOrEmpty(sMTAReasonDescription) = False Then
                        AddPolicyRequest.Description = AddPolicyRequest.Description + " - " + sMTAReasonDescription
                    End If

                    CreateEvent(con, AddPolicyRequest.PartyKey,
                        oResponse.InsuranceFolderKey, oResponse.InsuranceFileKey,
                        _SiriusUser.UserID, DateTime.Now,
                         AddPolicyRequest.Description, "POLCHANGE", True)
                End If

            ElseIf AddPolicyRequest.TransactionTypeCode = TransactionTypeCode.Renewals AndAlso AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then

                Dim runRenewalAcceptRequest As New BaseRunRenewalAcceptRequestType

                ' Set the inputs for the AddQuote method
                With runRenewalAcceptRequest
                    .BranchCode = AddPolicyRequest.BranchCode
                    .InsuranceFileKey = oResponse.InsuranceFileKey
                    .RecordsCount = 1
                    .GUID = bPMFunc.GetGUID
                End With

                RunRenewalAccept(con, runRenewalAcceptRequest)

            End If

            If AddPolicyRequest.PolicyProcessType = PolicyProcessTypes.Bind Then
                ' Arch BDX ----------------------------------------- Start
                'Update Due date of TransDetail Table in case if it is passed from BDX File
                If AddPolicyRequest.TransactionDueDate > Date.MinValue Then
                    UpdateTransactionDueDate(con, oResponse.InsuranceFileKey, AddPolicyRequest.TransactionDueDate)
                End If
                ' Arch BDX ----------------------------------------- Start
            End If
        Else
            ' Report an error that the insurance file couldn't be created or identified
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.PolicyMismatch, "The Policy record could not be created nor identified.", "The policy certificate - '" & getPolicyRequest.InsuranceRef & " Could not be created nor identified.")
            STSErrorEx.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), "PolicyProcess", "Policy Process Validation", True)
            Return oResponse

        End If

        Return oResponse

    End Function

    Private Function GetPBNode(ByRef xDoc As XmlDocument, ByVal xpath As String) As XmlNode
        Dim xNode As XmlNode
        Dim previousElementLocation As Integer = xpath.LastIndexOfAny(New [Char]() {"/"c})
        Dim newXpath As String = xpath.Substring(0, previousElementLocation)
        Dim newElement As String = xpath.Substring(previousElementLocation + 1)

        ' Select the node using the XPath
        xNode = xDoc.SelectSingleNode(xpath)
        If xNode Is Nothing Then
            xNode = GetPBNode(xDoc, newXpath)
        End If

        If (xDoc.SelectSingleNode(xpath)) Is Nothing Then
            If newElement.IndexOf("[", StringComparison.OrdinalIgnoreCase) + 1 > 0 Then
                newElement = Mid(newElement, 1, newElement.IndexOf("[", StringComparison.OrdinalIgnoreCase))
            End If
            Dim newNode As XmlNode = xDoc.CreateNode(XmlNodeType.Element, newElement, "")
            Dim attrUS As XmlAttribute = xDoc.CreateAttribute("US")
            attrUS.Value = "1"

            newNode.Attributes.Append(attrUS)

            Dim dataset As XmlNode = xDoc.SelectSingleNode("/DATA_SET")
            If dataset IsNot Nothing Then

                Dim nextOIAttr As XmlAttribute = dataset.Attributes("NextOINumber")
                If nextOIAttr IsNot Nothing Then

                    Dim nextOINumber As Integer = Convert.ToInt32(nextOIAttr.Value)
                    Dim attrOI As XmlAttribute = xDoc.CreateAttribute("OI")
                    nextOINumber = nextOINumber + 1
                    attrOI.Value = "OI" & nextOINumber.ToString
                    nextOIAttr.Value = nextOINumber.ToString

                    newNode.Attributes.Append(attrOI)

                End If
            End If

            xNode.AppendChild(newNode)

            xNode = xDoc.SelectSingleNode(xpath)

        End If

        Return xNode
    End Function

    Private Sub ProcessPBRiskData(ByRef sRiskXML As String, ByVal oAmendments() As BaseImplementationTypes.BaseProductBuilderRiskType)
        Dim xDoc As New XmlDocument()

        'Load XML
        xDoc.LoadXml(sRiskXML)

        'Manipluate XML
        Dim xNode As XmlNode
        ' For each ClaimBuilder data item
        For Each oAmend As BaseImplementationTypes.BaseProductBuilderRiskType In oAmendments
            ' Get the XPath value
            Dim xpath As String = oAmend.ProductBuilderData.ItemName
            ' Select the node using the XPath
            xNode = xDoc.SelectSingleNode(xpath)
            ' If the node has been found then
            If Not xNode Is Nothing Then
                ' Set the value of the attribute to the value passed in
                xNode.InnerText = oAmend.ProductBuilderData.Value
                ' Set the UpdateStatus of the Object
                Dim attr As XmlAttribute = CType(xNode, XmlAttribute)
                If (attr.OwnerElement.Attributes("US") Is Nothing) = False Then
                    If attr.OwnerElement.Attributes("US").Value = "0" Then
                        attr.OwnerElement.Attributes("US").Value = "2"
                    End If
                End If
                ' Else no attribute found so create one
            Else
                Dim objectName As String = String.Empty
                ' Split the Object and property name of the Xpath
                Dim propertyName As String = String.Empty
                Dim split As String() = xpath.Split(New [Char]() {"@"c})
                If split.Length = 2 Then
                    objectName = IIf(split(0).EndsWith("/"), split(0).Substring(0, split(0).Length - 1), split(0)).ToString
                    propertyName = split(1)
                    ' Find the Element/Object
                    xNode = GetPBNode(xDoc, objectName)
                    If Not xNode Is Nothing Then

                        ' Create the attribute and set the value
                        Dim attr As XmlNode = xDoc.CreateNode(XmlNodeType.Attribute, propertyName, "")
                        attr.Value = oAmend.ProductBuilderData.Value
                        xNode.Attributes.SetNamedItem(attr)
                        ' Set the UpdateStatus of the Object
                        If (xNode.Attributes("US") Is Nothing) = False Then
                            If xNode.Attributes("US").Value = "0" Then
                                xNode.Attributes("US").Value = "2"
                            End If
                        End If
                    End If
                End If
            End If
        Next
        Dim oNodeList As XmlNodeList = xDoc.SelectNodes("//*")

        For Each oNode As XmlNode In oNodeList
            Dim bValueFound As Boolean = False
            For iAttCnt As Integer = 0 To oNode.Attributes.Count - 1
                If oNode.Attributes(iAttCnt).Name <> "US" AndAlso oNode.Attributes(iAttCnt).Name <> "OI" AndAlso oNode.Attributes(iAttCnt).Value <> "" Then
                    bValueFound = True
                    Exit For
                End If
            Next
            If Not bValueFound Then
                Dim attribute As XmlAttribute = oNode.Attributes("US")
                If attribute IsNot Nothing Then
                    attribute.Value = "3"
                End If
            End If
        Next


        'Update the return string
        sRiskXML = xDoc.OuterXml

    End Sub

    Private Function UpdateOverridedTaxes(ByVal con As SiriusConnection, ByVal iInsuranceFileKey As Integer, ByVal oAddPolicyRequest As BaseQuoteRiskMsgType) As Integer

        Dim insuranceFileTypeId As Integer = 0
        Dim insuranceFileTypeCode As String = ""
        Dim sProductCode As String = ""

        Try
            GetInsuranceFileType(con, iInsuranceFileKey, insuranceFileTypeId, insuranceFileTypeCode)

            Select Case insuranceFileTypeCode
                Case InsuranceFileType.LivePolicy, InsuranceFileType.Quote
                    sProductCode = "NB"
                Case InsuranceFileType.MTAPermanentQuotation, InsuranceFileType.MTATemporaryQuotation,
                     InsuranceFileType.MTAPermanent, InsuranceFileType.MTATemporary,
                     InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated
                    sProductCode = "MTA"
                Case InsuranceFileType.Renewal
                    sProductCode = "REN"
                Case InsuranceFileType.MTACancellation
                    sProductCode = "MTC"
            End Select

            Dim sTransType As String

            sTransType = GetOverridedTaxTransType(con, iInsuranceFileKey)
            Dim ds As DataSet

            If oAddPolicyRequest.Taxes IsNot Nothing Then

                ds = New DataSet
                Dim r_vInsuranceFileTax As Object(,)

                ds = GetOverridedPolicyLevelTaxes(con, iInsuranceFileKey, 0, sTransType)

                If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    With ds.Tables(0)
                        For irow As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            For Each oTax As BaseTaxesType In oAddPolicyRequest.Taxes
                                If ds.Tables(0).Rows(irow).Item("TaxBandCode").ToString.Trim() = oTax.TaxBandCode Then
                                    If oTax.TaxRate > 0 Then
                                        ds.Tables(0).Rows(irow).Item("percentage") = oTax.TaxRate
                                        ds.Tables(0).Rows(irow).Item("is_value") = False
                                    Else
                                        ds.Tables(0).Rows(irow).Item("value") = oTax.Amount
                                        ds.Tables(0).Rows(irow).Item("percentage") = oTax.Amount
                                        ds.Tables(0).Rows(irow).Item("is_value") = True
                                    End If
                                End If
                            Next
                        Next
                    End With
                    r_vInsuranceFileTax = DirectCast(Utilities.DataSetToArray(ds), Object(,))
                    UpdateOverridedPolicyTaxes(con, oAddPolicyRequest.BranchCode, iInsuranceFileKey, sProductCode, r_vInsuranceFileTax)

                End If
            End If

            If oAddPolicyRequest.Risks IsNot Nothing Then

                ds = New DataSet

                Dim oRiskTax As Object(,)
                Dim iCount As Integer = 0
                Dim dsRiskTaxes As New DataSet
                Dim dtDataTable As New DataTable

                Dim iRowCount As Integer = 0

                For Each oRisk As BaseQuoteRiskMsgTypeRisks In oAddPolicyRequest.Risks
                    If oRisk.TaxesAndFees IsNot Nothing AndAlso oRisk.TaxesAndFees(0).Taxes IsNot Nothing Then
                        ds = GetOverridedRiskLevelTaxes(con, oRisk.RiskKey, 0, sTransType, iInsuranceFileKey)
                        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                            ds.Tables(0).Columns.Add("IsModified", Type.GetType("System.Int32"))
                            ds.Tables(0).Columns("IsModified").DefaultValue = 0
                            With ds.Tables(0)
                                iCount = 0
                                For iCount = 0 To ds.Tables(0).Rows.Count - 1
                                    For Each oTax As BaseTaxesType In oRisk.TaxesAndFees(0).Taxes
                                        If .Rows(iCount).Item("TaxBandCode").ToString.Trim() = oTax.TaxBandCode Then
                                            .Rows(iCount).Item("IsModified") = 1
                                            If oTax.TaxRate > 0 Then
                                                .Rows(iCount).Item("percentage") = oTax.TaxRate
                                                .Rows(iCount).Item("is_value") = False
                                            Else
                                                .Rows(iCount).Item("value") = oTax.Amount
                                                .Rows(iCount).Item("percentage") = oTax.Amount
                                                .Rows(iCount).Item("is_value") = True
                                            End If
                                            Exit For
                                        End If
                                    Next
                                Next
                            End With


                            Dim dsModifiedTax As New DataSet

                            Dim dt As New DataTable
                            dt = ds.Tables(0).Clone()
                            Dim results As DataRow() = ds.Tables(0).Select("IsModified = 1")

                            'if any row is modified only then recalculate the tax. 
                            If results IsNot Nothing AndAlso results.Length > 0 Then
                                For Each dr As DataRow In results
                                    dt.ImportRow(dr)
                                Next
                                dsModifiedTax.Tables.Add(dt)
                                oRiskTax = DirectCast(Utilities.DataSetToArray(dsModifiedTax), Object(,))

                                UpdateOverridedPolicyTaxes(con, oAddPolicyRequest.BranchCode, iInsuranceFileKey, sProductCode, oRiskTax)
                            End If
                        End If
                    End If
                Next

            End If
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function GetOverridedTaxTransType(ByVal con As SiriusConnection, ByVal iInsuranceFileKey As Integer) As String
        Dim ds As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_TransType_By_RiskKey")
            'add all the parameters to update
            cmd.Parameters.Clear()
            cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = iInsuranceFileKey
            ds = con.ExecuteDataSet(cmd, "table")
        End Using
        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            Return ds.Tables(0).Rows(0).Item(0).ToString
        Else
            Return ""
        End If
    End Function
    Private Function GetOverridedPolicyLevelTaxes(ByVal con As SiriusConnection, ByVal iInsuranceFileKey As Integer, ByVal iMode As Integer, ByVal sTransType As String) As DataSet

        Dim ds As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Insurance_File_Tax_Select")
            'add all the parameters to update
            cmd.Parameters.Clear()
            cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = iInsuranceFileKey
            cmd.AddInParameter("@Mode", SqlDbType.Int).Value = iMode
            cmd.AddInParameter("@TransType", SqlDbType.VarChar, 4).Value = sTransType
            cmd.AddInParameter("@RecalculateTaxes", SqlDbType.Int).Value = 0
            ds = con.ExecuteDataSet(cmd, "table")
        End Using
        Return ds

    End Function
    Private Function GetOverridedRiskLevelTaxes(ByVal con As SiriusConnection, ByVal iRiskKey As Integer, ByVal iMode As Integer, ByVal sTransType As String, ByVal nInsuranceFileKey As Integer) As DataSet

        Dim ds As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Risk_OverrideTax_Select")
            'add all the parameters to update
            cmd.Parameters.Clear()
            cmd.AddInParameter("@Risk_Cnt", SqlDbType.Int).Value = iRiskKey
            cmd.AddInParameter("@Mode", SqlDbType.Int).Value = iMode
            cmd.AddInParameter("@TransType", SqlDbType.VarChar, 4).Value = sTransType
            cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = nInsuranceFileKey
            ds = con.ExecuteDataSet(cmd, "table")
        End Using
        Return ds

    End Function
    Private Function UpdateOverridedPolicyTaxes(ByVal con As SiriusConnection, ByVal sBranchCode As String, ByVal insuranceFileKey As Integer, ByVal sProductCode As String, ByRef arrInsFileTax As Object(,))

        Const ACMethodName As String = "UpdateOverridedPolicyTaxes"

        Dim iRet As Integer
        Dim oSIRTax As bSIRRITax.Business = Nothing
        Try
            oSIRTax = New bSIRRITax.Business
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bSIRRITax.Business", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "Create bSIRRITax.Business Failed", True)
        End Try

        ' Initialise the business object
        Try
            SAMFunc.InitialiseSBOObject(Con:=con, oObject:=oSIRTax, SiriusUser:=_SiriusUser, sBranchCode:=sBranchCode, sObjectName:="bSIRRITax.Business")
        Catch ex As Exception
            If oSIRTax IsNot Nothing Then
                oSIRTax.Dispose()
                oSIRTax = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bSIRRITax.Business", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRRITax.Business.Initialise", True)
        End Try

        oSIRTax.InsuranceFileCnt = insuranceFileKey

        ' Set the Task mode and Transaction type
        iRet = oSIRTax.SetProcessModes(vTask:=2, vTransactionType:=Cast.ToObjString(sProductCode))
        If (iRet <> 1) Then
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeComponentFailed, "The call to bSIRRITax.Business.SetProcessModes failed.", iRet.ToString)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRRITax.Business.SetProcessModes failed.", True)
        End If

        Try

            If IsArray(arrInsFileTax) = True Then
                iRet = oSIRTax.CalculateTaxes(arrInsFileTax)
                iRet = oSIRTax.UpdateInsuranceFileTax(arrInsFileTax)
            End If
        Catch
            If oSIRTax IsNot Nothing Then
                oSIRTax.Dispose()
                oSIRTax = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeComponentFailed, "The call to bSIRRITax.Business.SetProcessModes failed.", iRet.ToString)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRRITax.Business.SetProcessModes failed.", True)

        End Try

        ' Kill off the com reference
        If oSIRTax IsNot Nothing Then
            oSIRTax.Dispose()
            oSIRTax = Nothing
        End If
    End Function

    ' ***************************************************************** '
    ' Name: AddRisk
    '
    ' Description: This method accepts the base policy implementation type 
    '              and adds a new policy and risk records.
    '
    ' ***************************************************************** '
    Public Overloads Function AddRisk(ByVal AddRiskRequest As BaseAddRiskRequestType) As BaseAddRiskResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseAddRiskResponseType

            oResponse = AddRisk(con, AddRiskRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function AddRisk(ByVal con As SiriusConnection, ByVal AddRiskRequest As BaseAddRiskRequestType) As BaseAddRiskResponseType

        'Declare the Response object
        Dim oResponse As BaseAddRiskResponseType = Nothing

        'Declare the input and output objects for core add quote method
        Dim oAddRiskIn As New AddRiskIn
        Dim oAddRiskOut As AddRiskOut = Nothing
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim nTypeOfPackage As enumTypeOfPackage

        Dim Utils As New Utilities

        'Dim iRiskCodeId As Int32
        Dim iScreenId As Int32
        Dim iProductId As Int32
        Dim iSourceId As Int32
        Dim iSubBranchId As Int32
        'Dim STSListType As Core.STSListType = Nothing
        Dim STSError As New STSErrorPublisher
        Dim iRiskTypeId As Int32
        Dim iDataModelId As Int32
        Dim oAdditionalData() As AdditionalData = Nothing
        Dim sXMLDataset As String = String.Empty

        Const ACMethodName As String = "AddRisk"

        ' Set type of package
        If AddRiskRequest.GetType Is GetType(BaseImplementationTypes.BaseAddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.MessagingPackage
            oResponse = New BaseImplementationTypes.BaseAddRiskResponseType
        ElseIf AddRiskRequest.GetType Is GetType(CustomerImplementationTypes.AddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.CustomersPackage
            oResponse = New CustomerImplementationTypes.AddRiskResponseType
        ElseIf AddRiskRequest.GetType Is GetType(AgentImplementationTypes.AddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AgentsPackage
            oResponse = New AgentImplementationTypes.AddRiskResponseType
        ElseIf AddRiskRequest.GetType Is GetType(AnonymousImplementationTypes.AddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.AnonymousPackage
            oResponse = New AnonymousImplementationTypes.AddRiskResponseType
        ElseIf AddRiskRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.AddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.AddRiskResponseType
        ElseIf AddRiskRequest.GetType Is GetType(SAMForBrokingImplementationTypes.AddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SamForBrokingPackage
            oResponse = New SAMForBrokingImplementationTypes.AddRiskResponseType
        ElseIf AddRiskRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddRiskRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddRiskResponseType
        End If

        ' Check Mandatory fields
        If AddRiskRequest.BranchCode = "" Then
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "BranchCode"), "")
        End If

        If AddRiskRequest.InsuranceFolderKey = 0 Then
            STSError.AddInvalidField("InsuranceFolderCnt", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFolderCnt"), "")
        End If

        If AddRiskRequest.InsuranceFileKey = 0 Then
            STSError.AddInvalidField("InsuranceFileCnt", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "InsuranceFileCnt"), "")
        End If

        If AddRiskRequest.ProductCode = "" Then
            STSError.AddInvalidField("ProductCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ProductCode"), "")
        End If

        If AddRiskRequest.RiskTypeCode = "" Then
            STSError.AddInvalidField("RiskTypeCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "RiskTypeCode"), "")
        End If

        If AddRiskRequest.ScreenCode = "" Then
            STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "ScreenCode"), "")
        End If

        If AddRiskRequest.RiskDescription = "" Then
            STSError.AddInvalidField("RiskDescription", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "RiskDescription"), "")
        End If

        If AddRiskRequest.DataModelCode = "" Then
            STSError.AddInvalidField("DataModelCode", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "DataModelCode"), "")
        End If

        If nTypeOfPackage <> enumTypeOfPackage.MessagingPackage Then
            If AddRiskRequest.QuoteTimeStamp Is Nothing OrElse Not IsArray(AddRiskRequest.QuoteTimeStamp) Then
                STSError.AddInvalidField("QuoteTimeStamp", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "QuoteTimeStamp"), "")
            End If
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' Branch Code
        Try
            iSourceId% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", AddRiskRequest.BranchCode)
        Catch ex As Exception
            ' not set
            STSError.AddInvalidField("BranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "BranchCode"), AddRiskRequest.BranchCode)
        End Try

        ' Sub Branch Code
        If AddRiskRequest.SubBranchCode <> "" Then
            Try
                iSubBranchId = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Source", AddRiskRequest.SubBranchCode)
            Catch ex As Exception
                ' not set
                STSError.AddInvalidField("SubBranchCode", CStr(STSErrorCodes.BranchCodeInvalid), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SubBranchCode"), AddRiskRequest.SubBranchCode)
            End Try
        End If

        ' DataModelCode
        Try
            iDataModelId% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "GIS_Data_Model", AddRiskRequest.DataModelCode)
        Catch ex As Exception
            ' not set
            STSError.AddInvalidField("DataModelCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "DataModelCode"), AddRiskRequest.DataModelCode)
        End Try

        ' Lookup screen id
        Try
            iScreenId% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "GIS_Screen", AddRiskRequest.ScreenCode)
        Catch ex As Exception
            STSError.AddInvalidField("ScreenCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ScreenCode"), AddRiskRequest.ScreenCode)
        End Try

        ' lookup Product Id
        Try
            iProductId% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Product", AddRiskRequest.ProductCode)
        Catch ex As Exception
            STSError.AddInvalidField("ProductCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ProductCode"), AddRiskRequest.ProductCode)
        End Try

        ' lookup RiskType Id
        Try
            iRiskTypeId% = oCoreBusiness.GetListItemFromCode(Core.STSListType.PMLookup, "Risk_Type", AddRiskRequest.RiskTypeCode)
        Catch ex As Exception
            STSError.AddInvalidField("RiskTypeCode", CStr(STSErrorCodes.InvalidLookupListValue), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "RiskTypeCode"), AddRiskRequest.RiskTypeCode)
        End Try

        ' Check for any errors
        If STSError.HasErrors Then
            ' Set the context
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Input Validation", True)
            ' Return the errors
            Return oResponse
        End If

        ' Check the insurance folder cnt is valid
        If oCoreBusiness.CheckInsuranceFolder(con, AddRiskRequest.InsuranceFolderKey) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance Folder validation failed", "The Insurance Folder record does not exist for key: " & AddRiskRequest.InsuranceFolderKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        ' Check the insurance file cnt is valid
        Dim iFileFolderCnt As Integer
        If oCoreBusiness.CheckInsuranceFile(con, AddRiskRequest.InsuranceFileKey, iFileFolderCnt) = False Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyRecordNotFound, "Insurance File validation failed", "The Insurance File record does not exist for key: " & AddRiskRequest.InsuranceFileKey)
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If
        If iFileFolderCnt <> AddRiskRequest.InsuranceFolderKey Then
            STSError = New STSErrorPublisher(STSErrorCodes.PolicyMismatch, "Insurance File validation failed", "The Insurance File's Folder does not match the passed InsuranceFolder")
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            Return oResponse
        End If

        ' Input Validation Passed

        With oAddRiskIn
            .BackOfficeMapperCode = InternalSAMConstants.CNAgentsOnline
            .DataModelCode = AddRiskRequest.DataModelCode
            .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
            .InsuranceFileCnt = AddRiskRequest.InsuranceFileKey
            .InsuranceFolderCnt = AddRiskRequest.InsuranceFolderKey
            .RiskScreenId = iScreenId
            .RiskTypeId = iRiskTypeId
            .ProductID = iProductId
            .AdditionalDataArray = oAdditionalData
            .RiskDescription = AddRiskRequest.RiskDescription
        End With

        Try
            oAddRiskOut = oCoreBusiness.AddRisk(con, oAddRiskIn)
        Catch ex As ApplicationException
            Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCodes.FailedToAddRiskRecord, "Failed to add risk record", "Failed to add risk record")
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
        End Try

        '  Add the rating sections if they're present
        If AddRiskRequest.RatingSections IsNot Nothing Then

            Dim oSAMErrorCollection As New SAMErrorCollection
            AddRiskRatingSections(con, oCoreBusiness, AddRiskRequest.RatingSections, AddRiskRequest.InsuranceFileKey, oAddRiskOut.RiskCnt, oSAMErrorCollection)

        End If

        If AddRiskRequest.XMLDataSet <> "" Then
            ProcessRiskXMLDataset(con, AddRiskRequest, oAddRiskIn, oAddRiskOut, iProductId, sXMLDataset)
        Else
            sXMLDataset = oAddRiskOut.XMLDataset
        End If

        '  Add the ReInsurance arrangments if they're present
        If AddRiskRequest.RIArrangement IsNot Nothing Then

            Dim iRIArrangementID As Integer = 0
            Dim oSAMErrorCollection As New SAMErrorCollection
            AddRiskRIArrangements(con, oCoreBusiness, AddRiskRequest.RIArrangement, oAddRiskOut.RiskCnt, iRIArrangementID, oSAMErrorCollection)

        End If

        ' Transform the dataset back from its PB format to the SAM format, i.e. with the address structures etc.
        sXMLDataset = sXMLDataset

        ' Setup the return Class.
        With oResponse

            .RiskFolderKey = oAddRiskOut.RiskFolderCnt
            .RiskKey = oAddRiskOut.RiskCnt
            .XMLDataSet = sXMLDataset
            .BranchID = iSourceId%
            .RiskTypeId = iRiskTypeId
        End With

        ' Get the Timestamp
        Dim AnyError As STSErrorType
        Dim bIsLocked As Boolean
        AnyError = oCoreBusiness.GetTimestamp(con,
                            AddRiskRequest.BranchCode,
                            CoreBusiness.LockName.InsuranceFolderCnt,
                            AddRiskRequest.InsuranceFolderKey,
                            oResponse.QuoteTimeStamp,
                            bIsLocked)
        ' Return AnyErrors
        If AnyError Is Nothing = False Then
            oResponse.STSError = AnyError
        End If

        If AddRiskRequest.RunDefaultRules Then
            'RUN DEFAULT RULES (ADD)
            'Declare Implementation Type for Running Default Rules
            Dim oRulesIn As New SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddRequestType
            Dim oRulesOut As SiriusFS.SAM.Structure.BaseImplementationTypes.BaseRunDefaultRulesAddResponseType
            'Set Inputs In
            oRulesIn.BranchCode = AddRiskRequest.BranchCode
            oRulesIn.ScreenCode = AddRiskRequest.ScreenCode
            oRulesIn.DataModelCode = AddRiskRequest.DataModelCode
            oRulesIn.XMLDataSet = sXMLDataset 'Take the output from previous calls
            'Run Code
            oRulesOut = RunDefaultRulesAdd(oRulesIn)
            'Set Outputs Back
            oResponse.XMLDataSet = oRulesOut.XMLDataSet
            If Not (oRulesOut.STSError Is Nothing) Then
                oResponse.STSError = oRulesOut.STSError
            End If
        End If

        ''''UpdateRiskStatus-- RITU ''2561
        If AddRiskRequest.Data_Transfer = True Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SIR_Update_Risk_Status")
                cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = oResponse.RiskKey
                cmd.AddInParameter("@risk_status_code", SqlDbType.VarChar, 20).Value = "QUOTED"
                con.ExecuteNonQuery(cmd)
            End Using


            ' Update the Risk table values from the special Dataset Properties
            ' TODO do we need to do this here too, or just on updaterisk???
            'UpdateRiskFromGIS(con, AddRiskRequest.InsuranceFileKey, oResponse.RiskKey, oResponse.XMLDataSet, AddRiskRequest.DataModelCode)


            '-------------------------------

            ' Dim ACMethodName As String
            Dim iRet As Integer
            Dim oGIS As bGIS.Application = Nothing
            'Dim oUpdateRatingDetailsResponse As New BaseUpdateRatingDetailsResponsetType
            Try
                oGIS = New bGIS.Application
            Catch ex As Exception
                Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCodes.FailedToAddRiskRecord, "Failed to add risk record", "Failed to add risk record")
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
            End Try

            Try
                SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser, sBranchCode:=AddRiskRequest.BranchCode)
            Catch ex As Exception
                If oGIS IsNot Nothing Then
                    oGIS.Dispose()
                    oGIS = Nothing
                End If
                Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCodes.FailedToAddRiskRecord, "Failed to add risk record", "Failed to add risk record")
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
            End Try

            iRet = oGIS.SetProcessModes(SAMComponentAction.PMEdit, 0, 0, Cast.ToObjString("NB"), Now)

            Try
                iRet = oGIS.UpdateRiskAfter(
                                v_lInsuranceFileCnt:=AddRiskRequest.InsuranceFileKey,
                                v_lInsuranceFolderCnt:=AddRiskRequest.InsuranceFolderKey,
                                v_lRiskCnt:=oResponse.RiskKey,
                                sTransactionType:="NB")

            Catch ex As Exception
                Dim STSErrorEx As STSErrorPublisher = New STSErrorPublisher(STSErrorCodes.FailedToAddRiskRecord, "Failed to add risk record", "Failed to add risk record")
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
            End Try

            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If

        End If

        Return oResponse

    End Function

    Private Sub ProcessRiskXMLDataset(ByVal con As SiriusConnection, ByVal AddRiskRequest As BaseAddRiskRequestType, ByVal oAddRiskIn As AddRiskIn, ByVal oAddRiskOut As AddRiskOut, ByVal iProductId As Int32, ByRef sXMLDataset As String)

        ' Declare the Core Business object
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim STSError As New STSErrorPublisher

        Const ACMethodName As String = "ProcessRiskXMLDataset"

        ' Transform the Dataset into its PB Format
        Dim sDataModelCode As String = String.Empty
        Dim lGisPolicyLinkId As Int32
        Dim lPolicyBinderId As Int32
        Dim sOI As String = ""
        Dim outputXMLDataset As String = String.Empty
        Dim oSaveToDBIn As New SaveToDBIn
        Dim oSaveToDBOut As New SaveToDBOut

        Try
            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.LoadXml(oAddRiskOut.XMLDataset)
            sDataModelCode = xmlDoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value.ToUpper
            lGisPolicyLinkId = XmlSafeConvert.ToInt32(xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(sDataModelCode & "_POLICY_BINDER").Attributes("GIS_POLICY_LINK_ID").Value, 0)
            'sNextOiNumber = xmlDoc.SelectSingleNode("DATA_SET").Attributes("NextOINumber").Value
            sOI = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(sDataModelCode & "_POLICY_BINDER").Attributes("OI").Value
            lPolicyBinderId = XmlSafeConvert.ToInt32(xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode(sDataModelCode & "_POLICY_BINDER").Attributes(sDataModelCode & "_POLICY_BINDER_ID").Value, 0)
        Catch ex As Exception
        End Try

        sXMLDataset = AddRiskRequest.XMLDataSet

        Try
            Dim xmlDoc As New System.Xml.XmlDocument
            xmlDoc.LoadXml(sXMLDataset)
            'xmlDoc.DocumentElement.SetAttribute("NextOINumber", sNextOiNumber)
            xmlDoc.DocumentElement.Item("RISK_OBJECTS").Item(sDataModelCode & "_POLICY_BINDER").SetAttribute("OI", sOI)
            xmlDoc.DocumentElement.Item("RISK_OBJECTS").Item(sDataModelCode & "_POLICY_BINDER").SetAttribute("US", "2")
            xmlDoc.DocumentElement.SetAttribute("DataModelCode", sDataModelCode)
            xmlDoc.DocumentElement.Item("RISK_OBJECTS").Item(sDataModelCode & "_POLICY_BINDER").SetAttribute("GIS_POLICY_LINK_ID", lGisPolicyLinkId.ToString)
            xmlDoc.DocumentElement.Item("RISK_OBJECTS").Item(sDataModelCode & "_POLICY_BINDER").SetAttribute(sDataModelCode & "_POLICY_BINDER_ID", lPolicyBinderId.ToString)
            sXMLDataset = xmlDoc.OuterXml
        Catch ex As Exception
        End Try

        sXMLDataset = SAMFunc.TransformDatasetSAMtoPB(con, sXMLDataset, , AddRiskRequest.IsMarketplacePolicy)

        With oSaveToDBIn
            .DataModelCode = oAddRiskIn.DataModelCode
            .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
            .XMLDataset = sXMLDataset$
        End With

        ' Save the new dataset to the DB
        Try
            oSaveToDBOut = oCoreBusiness.SaveToDB(con, oSaveToDBIn)
        Catch ex As ApplicationException
            STSError = New STSErrorPublisher("An error occured calling CoreBusiness.SaveToDB", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
        End Try

        If (oSaveToDBOut Is Nothing) = True Then
            STSError = New STSErrorPublisher(STSErrorCodes.FailedToSaveRiskToDatabase, "Failed to save risk data to the database", "Failed to save risk data to the database")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "SiriusFS.STS." & ACMethodName, True)
        End If

        Dim oNBQuoteIn As New NBQuoteIn
        Dim oNBQuoteOut As NBQuoteOut

        Dim vAdditionalDataArray() As AdditionalData
        Dim oData As New AdditionalData

        ReDim vAdditionalDataArray(3)

        oData = New AdditionalData
        oData.Name = InternalSAMConstants.CNInsuranceFileCnt
        oData.Value = oAddRiskIn.InsuranceFileCnt
        vAdditionalDataArray(0) = oData

        oData = New AdditionalData
        oData.Name = InternalSAMConstants.CNInsuranceFolderCnt
        oData.Value = oAddRiskIn.InsuranceFolderCnt
        vAdditionalDataArray(1) = oData

        oData = New AdditionalData
        oData.Name = InternalSAMConstants.CNCurrentRiskCnt
        oData.Value = oAddRiskOut.RiskCnt
        vAdditionalDataArray(2) = oData

        oData = New AdditionalData
        oData.Name = InternalSAMConstants.CNTransactionType
        oData.Value = gPMConstants.PMTypeOfBusinessNB
        vAdditionalDataArray(3) = oData

        With oNBQuoteIn
            .AdditionalDataArray = vAdditionalDataArray
            .DataModelCode = oAddRiskIn.DataModelCode
            .BusinessTypeCode = gPMConstants.PMTypeOfBusinessNB
            .EffectiveDate = Now
            .GISSchemeID = 1
            .RiskGroupID = iProductId
            .RiskScreenId = oAddRiskIn.RiskScreenId
            .XMLDataset = oSaveToDBOut.XMLDataset
        End With

        ' Perform the Quote
        oNBQuoteOut = oCoreBusiness.RunQuote(con, oNBQuoteIn, QuoteTypeRules.Quote)

        outputXMLDataset = oNBQuoteOut.XMLDataset

        sXMLDataset = outputXMLDataset
    End Sub

    ' ***************************************************************** '
    ' Name: NBTransact
    '
    ' Description: This method 
    '
    ' ***************************************************************** '
    Public Function NBTransact(ByVal NBTransactRequest As BaseTransactRequestType) As BaseTransactResponseType

        'Declare the Response object
        Dim oResponse As New BaseImplementationTypes.BaseTransactResponseType

        'Declare the input and output objects for core add quote method
        Dim oNBTransactIn As New NBTransactIn
        Dim oNBTransactOut As NBTransactOut
        Dim oProcessAccountsIn As New ProcessAccountsIn
        Dim oProcessAccountsOut As ProcessAccountsOut
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oAdditionalData() As AdditionalData

        Dim STSError As New STSErrorPublisher

        Dim lInsuranceFolderCnt As Integer
        Dim lInsuranceFileCnt As Integer
        Dim lRiskCnt As Integer
        Dim lPolicyLinkID As Integer
        Dim dGrossAmount As Decimal
        Dim dCommissionAmount As Decimal
        Dim dIPTAmount As Decimal
        Dim sPolicyNum As String = String.Empty
        Dim nRecsFound As Integer

        Dim sXmlDataset As String = String.Empty
        Dim lGisSchemeID As Int32
        Dim sGisDataModelCode As String = String.Empty

        Const ACMethodName As String = "NBTransact"

        ' Note. At the time of writing we are assuming that the NBTransact
        ' will be called with exactly the same risks as were used to create
        ' the quote, therefore the risks array is effectively redundant. In
        ' the GIS NBTransact, nothing is done with the XMLDataset when
        ' called from the Webservice
        If SAMFunc.NothingToString(NBTransactRequest.QuoteRef) = "" Then
            STSError.AddInvalidField("QuoteRef", CStr(STSErrorCodes.MandatoryInputMissing), [String].Format(MandatoryInputMissing, "QuoteRef"), "")
        End If

        ' exit if there are any missing parameters
        If STSError.HasErrors Then
            STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oResponse
        End If

        ' Get the Ins Folder and File PK Values for this Quote
        nRecsFound = GetQuoteDetails(
                NBTransactRequest.QuoteRef,
                lInsuranceFolderCnt, lInsuranceFileCnt,
                lRiskCnt, lPolicyLinkID, dGrossAmount,
                dCommissionAmount, dIPTAmount)

        Select Case nRecsFound
            Case 0
                STSError.AddInvalidField("QuoteRef", CStr(STSErrorCodes.QuoteHeaderRecordNotFound), [String].Format(STSErrorPublisher.MandatoryInputInvalid, "QuoteRef"), NBTransactRequest.QuoteRef)
                STSError.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Data value invalid", True)
                Return oResponse
            Case Is > 1
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.GeneralFailure, "More than 1 Quote found for QuoteRef", NBTransactRequest.QuoteRef)
                STSErrorEX.SetContext(oResponse.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "More than 1 Quote found for QuoteRef", True)
                Return oResponse
        End Select

        ' Populate the Structure for the Call to Transact
        oAdditionalData = InitialiseAdditionalData(1)

        oAdditionalData(0).Value = lInsuranceFileCnt
        oAdditionalData(0).Name = InternalSAMConstants.CNInsuranceFileCnt
        oAdditionalData(1).Value = 2  ' Hardcoded for Now
        oAdditionalData(1).Name = InternalSAMConstants.CNClaimMode

        sXmlDataset = ""
        lGisSchemeID = -1
        sGisDataModelCode = ""

        With oNBTransactIn
            .lGISSchemeID = lGisSchemeID
            .sGisBusinessTypeCode = "NB" 'CNQuoteBusinessType
            .sGisDataModelCode = sGisDataModelCode
            .AdditionalDataArray = oAdditionalData
            .InsuranceFileCnt = lInsuranceFileCnt
            .sXMLDataSet = sXmlDataset
        End With

        ' Call the Transact
        oNBTransactOut = oCoreBusiness.NBTransact(oNBTransactIn)

        If oNBTransactOut.STSError Is Nothing = False Then
            oResponse.STSError = oNBTransactOut.STSError
            Return oResponse
        End If

        ' Get the Data from the Transact
        oAdditionalData = oNBTransactOut.AdditionalDataArray
        Dim iUBound As Integer = oAdditionalData.GetUpperBound(0)
        For iCounter As Integer = 0 To iUBound
            Select Case oAdditionalData(iCounter).Name
                'Case CNNBTransactMessage
                '    sTransactMsg = oAdditionalData(iCounter).Value
                Case InternalSAMConstants.CNNewPolicyNumber
                    sPolicyNum = Cast.ToString(oAdditionalData(iCounter).Value, String.Empty)
            End Select
        Next

        'Now that we made the policy live...ProcessAccounts
        With oProcessAccountsIn
            .DataModelCode = "AOL"
            .BusinessTypeCode = InternalSAMConstants.Header_BusinessType
            .InsuranceFileCnt = lInsuranceFileCnt
            .TransactionType = gPMConstants.PMTypeOfBusinessNB
        End With

        oProcessAccountsOut = oCoreBusiness.ProcessAccounts(oProcessAccountsIn)

        If oProcessAccountsOut.STSError Is Nothing = False Then
            oResponse.STSError = oProcessAccountsOut.STSError
            Return oResponse
        End If

        ' Get the Premium, tax and commission details for this Quote
        GetQuoteDetails(sPolicyNum, lInsuranceFolderCnt, lInsuranceFileCnt, lRiskCnt, lPolicyLinkID, dGrossAmount, dCommissionAmount, dIPTAmount)

        ' Return the details
        oResponse.Policy = New BaseImplementationTypes.BaseTransactResponseTypePolicy
        oResponse.Policy.PolicyRef = sPolicyNum
        oResponse.Policy.PremiumDueGross = CDec(dGrossAmount)
        oResponse.Policy.PremiumDueNet = CDec(dGrossAmount) - CDec(dIPTAmount)
        oResponse.Policy.PremiumDueTax = CDec(dIPTAmount)
        oResponse.Policy.TotalAnnualTax = CDec(dIPTAmount) ' TODO where should this come from?
        oResponse.Policy.CommissionAmount = CDec(dCommissionAmount)

        ' If everything is OK, return the Policy Number
        Return oResponse

    End Function

    Private Function InitialiseAdditionalData(ByVal v_iUpper As Int32) As AdditionalData()

        Dim iLoop1 As Integer
        Dim oAdditionalData(v_iUpper) As AdditionalData

        For iLoop1 = 0 To v_iUpper
            Try
                oAdditionalData(iLoop1) = New AdditionalData
            Catch oe As Exception
                'ExceptionManager.Publish(oe)
                Throw New Exception("Failed to create new AdditionalData", oe)
            End Try
        Next

        Return oAdditionalData

    End Function

    ' Arch BDX ----------------------------------------- Start
    Public Sub UpdateTransactionDueDate(ByVal con As SiriusConnection, ByVal insuranceFileKey As Integer, ByVal dueDate As Date)
        ' Update Due Date of Transaction in Trans Detail if specfied from BDX
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_UpdateTransactionDueDate")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey
            cmd.AddInParameter("@Due_Date", SqlDbType.Date).Value = dueDate
            con.ExecuteNonQuery(cmd)
        End Using

    End Sub
    ' Arch BDX ----------------------------------------- End
    Private Function UpdateOverriddenTaxes(ByVal con As SiriusConnection, ByVal iInsuranceFileKey As Integer, ByVal oAddPolicyRequest As BaseQuoteRiskMsgType) As Integer

        Dim insuranceFileTypeId As Integer = 0
        Dim insuranceFileTypeCode As String = ""
        Dim sProductCode As String = ""

        Try
            GetInsuranceFileType(con, iInsuranceFileKey, insuranceFileTypeId, insuranceFileTypeCode)

            Select Case insuranceFileTypeCode
                Case InsuranceFileType.LivePolicy, InsuranceFileType.Quote
                    sProductCode = "NB"
                Case InsuranceFileType.MTAPermanentQuotation, InsuranceFileType.MTATemporaryQuotation,
                     InsuranceFileType.MTAPermanent, InsuranceFileType.MTATemporary,
                     InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated
                    sProductCode = "MTA"
                Case InsuranceFileType.Renewal
                    sProductCode = "REN"
                Case InsuranceFileType.MTACancellation
                    sProductCode = "MTC"
            End Select

            Dim sTransType As String

            sTransType = GetOverriddenTaxTransType(con, iInsuranceFileKey)
            Dim ds As DataSet

            If oAddPolicyRequest.Taxes IsNot Nothing Then

                ds = New DataSet
                Dim oInsuranceFileTax As Object(,)

                ds = GetOverriddenPolicyLevelTaxes(con, iInsuranceFileKey, 0, sTransType)

                If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    With ds.Tables(0)
                        For irow As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            For Each oTax As BaseTaxesType In oAddPolicyRequest.Taxes
                                If ds.Tables(0).Rows(irow).Item("TaxBandCode").ToString.Trim() = oTax.TaxBandCode Then
                                    If oTax.TaxRate > 0 Then
                                        ds.Tables(0).Rows(irow).Item("percentage") = oTax.TaxRate
                                        ds.Tables(0).Rows(irow).Item("is_value") = False
                                    Else
                                        ds.Tables(0).Rows(irow).Item("value") = oTax.Amount
                                        ds.Tables(0).Rows(irow).Item("percentage") = oTax.Amount
                                        ds.Tables(0).Rows(irow).Item("is_value") = True
                                    End If
                                End If
                            Next
                        Next
                    End With
                    oInsuranceFileTax = DirectCast(Utilities.DataSetToArray(ds), Object(,))
                    UpdateOverriddenPolicyTaxes(con, oAddPolicyRequest.BranchCode, iInsuranceFileKey, sProductCode, oInsuranceFileTax)

                End If
            End If

            If oAddPolicyRequest.Risks IsNot Nothing Then

                ds = New DataSet

                Dim oRiskTax As Object(,)
                Dim iCount As Integer = 0
                Dim dsRiskTaxes As New DataSet
                Dim dtDataTable As New DataTable

                Dim iRowCount As Integer = 0

                For Each oRisk As BaseQuoteRiskMsgTypeRisks In oAddPolicyRequest.Risks
                    If oRisk.TaxesAndFees IsNot Nothing AndAlso oRisk.TaxesAndFees(0).Taxes IsNot Nothing Then
                        ds = GetOverriddenRiskLevelTaxes(con, oRisk.RiskKey, 0, sTransType)
                        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                            With ds.Tables(0)
                                iCount = 0
                                For iCount = 0 To ds.Tables(0).Rows.Count - 1
                                    For Each oTax As BaseTaxesType In oRisk.TaxesAndFees(0).Taxes
                                        If .Rows(iCount).Item("TaxBandCode").ToString.Trim() = oTax.TaxBandCode Then
                                            If oTax.TaxRate > 0 Then
                                                .Rows(iCount).Item("percentage") = oTax.TaxRate
                                                .Rows(iCount).Item("is_value") = False
                                            Else
                                                .Rows(iCount).Item("value") = oTax.Amount
                                                .Rows(iCount).Item("percentage") = oTax.Amount
                                                .Rows(iCount).Item("is_value") = True
                                            End If
                                            Exit For
                                        End If
                                    Next

                                Next

                            End With
                            oRiskTax = DirectCast(Utilities.DataSetToArray(ds), Object(,))
                            UpdateOverriddenPolicyTaxes(con, oAddPolicyRequest.BranchCode, iInsuranceFileKey, sProductCode, oRiskTax)
                        End If
                    End If
                Next

            End If
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function GetOverriddenTaxTransType(ByVal con As SiriusConnection, ByVal iInsuranceFileKey As Integer) As String
        Dim ds As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_TransType_By_RiskKey")
            'add all the parameters to update
            cmd.Parameters.Clear()
            cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = iInsuranceFileKey
            ds = con.ExecuteDataSet(cmd, "table")
        End Using
        If ds IsNot Nothing AndAlso ds.Tables IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
            Return ds.Tables(0).Rows(0).Item(0).ToString
        Else
            Return ""
        End If
    End Function
    Private Function GetOverriddenPolicyLevelTaxes(ByVal con As SiriusConnection, ByVal iInsuranceFileKey As Integer, ByVal iMode As Integer, ByVal sTransType As String) As DataSet

        Dim ds As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Insurance_File_Tax_Select")
            'add all the parameters to update
            cmd.Parameters.Clear()
            cmd.AddInParameter("@Insurance_File_Cnt", SqlDbType.Int).Value = iInsuranceFileKey
            cmd.AddInParameter("@Mode", SqlDbType.Int).Value = iMode
            cmd.AddInParameter("@TransType", SqlDbType.VarChar, 4).Value = sTransType
            cmd.AddInParameter("@RecalculateTaxes", SqlDbType.Int).Value = 0
            ds = con.ExecuteDataSet(cmd, "table")
        End Using
        Return ds

    End Function
    Private Function GetOverriddenRiskLevelTaxes(ByVal con As SiriusConnection, ByVal iRiskKey As Integer, ByVal iMode As Integer, ByVal sTransType As String) As DataSet

        Dim ds As New DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Risk_Tax_Select")
            'add all the parameters to update
            cmd.Parameters.Clear()
            cmd.AddInParameter("@Risk_Cnt", SqlDbType.Int).Value = iRiskKey
            cmd.AddInParameter("@Mode", SqlDbType.Int).Value = iMode
            cmd.AddInParameter("@TransType", SqlDbType.VarChar, 4).Value = sTransType
            cmd.AddInParameter("@RecalculateTaxes", SqlDbType.Int).Value = 0
            ds = con.ExecuteDataSet(cmd, "table")
        End Using
        Return ds

    End Function
    Private Function UpdateOverriddenPolicyTaxes(ByVal con As SiriusConnection, ByVal sBranchCode As String, ByVal insuranceFileKey As Integer, ByVal sProductCode As String, ByRef arrInsFileTax As Object(,))

        Const ACMethodName As String = "UpdateOverriddenPolicyTaxes"

        Dim iRet As Integer
        Dim oSIRTax As bSIRRITax.Business = Nothing
        Try
            oSIRTax = New bSIRRITax.Business
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bSIRRITax.Business", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "Create bSIRRITax.Business Failed", True)
        End Try

        ' Initialise the business object
        Try
            SAMFunc.InitialiseSBOObject(Con:=con, oObject:=oSIRTax, SiriusUser:=_SiriusUser, sBranchCode:=sBranchCode, sObjectName:="bSIRRITax.Business")
        Catch ex As Exception
            If oSIRTax IsNot Nothing Then
                oSIRTax.Dispose()
                oSIRTax = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bSIRRITax.Business", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRRITax.Business.Initialise", True)
        End Try

        oSIRTax.InsuranceFileCnt = insuranceFileKey

        ' Set the Task mode and Transaction type
        iRet = oSIRTax.SetProcessModes(vTask:=2, vTransactionType:=Cast.ToObjString(sProductCode))
        If (iRet <> 1) Then
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeComponentFailed, "The call to bSIRRITax.Business.SetProcessModes failed.", iRet.ToString)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRRITax.Business.SetProcessModes failed.", True)
        End If

        Try

            If IsArray(arrInsFileTax) = True Then
                iRet = oSIRTax.CalculateTaxes(arrInsFileTax)
                iRet = oSIRTax.UpdateInsuranceFileTax(arrInsFileTax)
            End If
        Catch
            If oSIRTax IsNot Nothing Then
                oSIRTax.Dispose()
                oSIRTax = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorCodes.BackofficeComponentFailed, "The call to bSIRRITax.Business.SetProcessModes failed.", iRet.ToString)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRRITax.Business.SetProcessModes failed.", True)

        End Try

        ' Kill off the com reference
        If oSIRTax IsNot Nothing Then
            oSIRTax.Dispose()
            oSIRTax = Nothing
        End If
    End Function

End Class

